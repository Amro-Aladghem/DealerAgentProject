import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
} from 'react';
import type { ReactNode } from 'react';
import storeService from '@/services/storeService';
import { STORE_ID } from '@/libs/config';
import { buildChatTimeline } from '@/libs/chatGrouping';
import { eChatRole } from '@/types/enums';
import type { AgentType, NotificationStatus } from '@/types/enums';
import type { ChatDto } from '@/types/chat';
import type { NotificationDto } from '@/types/notification';
import type { ChatTimelineItem, ChatTimelineUserItem } from '@/types/chatSession';

const STORAGE_KEY = 'dealeragent.chatId';
const POLL_INTERVAL_MS = 5000;
const FRESH_MESSAGE_AGENT_HINT: AgentType = 'DealerAgent';

const TERMINAL_STATUSES: NotificationStatus[] = [
  'WaitingForUser',
  'Completed',
  'Failed',
];

interface ChatContextValue {
  chatOpen: boolean;
  openChat: () => void;
  closeChat: () => void;
  chatId: string | null;
  timeline: ChatTimelineItem[];
  notifications: NotificationDto[];
  loading: boolean;
  creating: boolean;
  sending: boolean;
  hasMessages: boolean;
  startNewChat: () => Promise<void>;
  sendMessage: (text: string) => Promise<void>;
  reload: () => void;
}

const ChatContext = createContext<ChatContextValue | null>(null);

function readStoredChatId(): string | null {
  try {
    const value = window.sessionStorage.getItem(STORAGE_KEY);
    return value && value.trim().length > 0 ? value : null;
  } catch {
    return null;
  }
}

function writeStoredChatId(value: string): void {
  try {
    window.sessionStorage.setItem(STORAGE_KEY, value);
  } catch {
    // ignore storage failures
  }
}

function serialize(value: unknown): string {
  return JSON.stringify(value);
}

/**
 * The API already returns notifications ordered by UpdatedAt ascending,
 * so the last element is the newest one. Re-sorting on the client would
 * reintroduce ordering bugs (equal timestamps + random GUID tie-break).
 */
function lastNotification(notifications: NotificationDto[]): NotificationDto | null {
  return notifications.length > 0 ? notifications[notifications.length - 1] : null;
}

/**
 * Notifications from internal agents (Advisor / ProductsSearch / Complaints)
 * are ignored by the chat surface: only the DealerAgent drives the visible state.
 */
function lastDealerNotification(notifications: NotificationDto[]): NotificationDto | null {
  for (let index = notifications.length - 1; index >= 0; index -= 1) {
    if (notifications[index].agentType === 'DealerAgent') return notifications[index];
  }
  return null;
}

function waitingAgentType(notifications: NotificationDto[]): AgentType | null {
  // Keep the overall last notification here (not DealerAgent-only) so a reply
  // is routed with askUser=true to the agent that actually asked.
  const last = lastNotification(notifications);
  return last && last.notificationStatus === 'WaitingForUser' ? last.agentType : null;
}

export function ChatProvider({ children }: { children: ReactNode }) {
  const [chatOpen, setChatOpen] = useState(false);
  const [chatId, setChatId] = useState<string | null>(() => readStoredChatId());
  const [history, setHistory] = useState<ChatDto[]>([]);
  const [notifications, setNotifications] = useState<NotificationDto[]>([]);
  const [pending, setPending] = useState<ChatTimelineUserItem[]>([]);
  const [loadedKey, setLoadedKey] = useState<string | null>(null);
  const [refreshToken, setRefreshToken] = useState(0);
  const [creating, setCreating] = useState(false);
  const [sending, setSending] = useState(false);
  const creatingRef = useRef(false);
  const notificationsJsonRef = useRef<string>('');

  const loading = chatId !== null && loadedKey !== chatId;

  const timeline = useMemo(() => {
    const base = buildChatTimeline(history, notifications);
    return pending.length > 0 ? [...base, ...pending] : base;
  }, [history, notifications, pending]);

  const hasMessages = timeline.length > 0;
  const lastStatus = useMemo(
    () => lastDealerNotification(notifications)?.notificationStatus ?? null,
    [notifications],
  );

  useEffect(() => {
    if (!chatId) return;
    let active = true;
    const key = chatId;

    const load = async () => {
      try {
        const notificationsResult = await storeService.getNotificationsForChat(
          STORE_ID,
          key,
        );
        if (!active) return;
        const notificationsData = notificationsResult.isSuccess
          ? notificationsResult.data
          : [];
        notificationsJsonRef.current = serialize(notificationsData);
        setNotifications(notificationsData);

        const historyResult = await storeService.getChatHistory(STORE_ID, key);
        if (!active) return;
        setHistory(historyResult.isSuccess ? historyResult.data : []);
        setLoadedKey(key);
      } catch {
        if (!active) return;
        notificationsJsonRef.current = '[]';
        setHistory([]);
        setNotifications([]);
        setLoadedKey(key);
      }
    };

    void load();

    return () => {
      active = false;
    };
  }, [chatId, refreshToken]);

  useEffect(() => {
    if (!chatOpen || !chatId || !hasMessages) return;
    if (lastStatus !== null && TERMINAL_STATUSES.includes(lastStatus)) return;

    let cancelled = false;
    let inFlight = false;

    const run = async () => {
      if (inFlight) return;
      inFlight = true;
      try {
        const result = await storeService.getNotificationsForChat(STORE_ID, chatId);
        if (cancelled || !result.isSuccess) return;

        const serialized = serialize(result.data);
        const changed = serialized !== notificationsJsonRef.current;
        const status = lastDealerNotification(result.data)?.notificationStatus ?? null;
        const isTerminal = status !== null && TERMINAL_STATUSES.includes(status);

        if (!changed && !isTerminal) return;

        let nextHistory: ChatDto[] | null = null;
        if (isTerminal) {
          const historyResult = await storeService.getChatHistory(STORE_ID, chatId);
          if (historyResult.isSuccess) nextHistory = historyResult.data;
        }

        if (cancelled) return;

        notificationsJsonRef.current = serialized;
        setNotifications(result.data);
        if (nextHistory) setHistory(nextHistory);
      } catch {
        // polling failures are intentionally silent
      } finally {
        inFlight = false;
      }
    };

    const interval = window.setInterval(() => {
      void run();
    }, POLL_INTERVAL_MS);

    return () => {
      cancelled = true;
      window.clearInterval(interval);
    };
  }, [chatOpen, chatId, hasMessages, lastStatus]);

  const startNewChat = useCallback(async () => {
    if (creatingRef.current) return;
    creatingRef.current = true;
    setCreating(true);
    try {
      const signature = crypto.randomUUID();
      const result = await storeService.createChat(STORE_ID, {
        chatSignature: signature,
        storeId: STORE_ID,
        creationTime: new Date().toISOString(),
        chatRoleId: eChatRole.User,
        message: '',
        data: null,
        showMessage: false,
        isFinalResponse: false,
      });

      if (result.isSuccess && result.data?.chatSignature) {
        const id = result.data.chatSignature;
        writeStoredChatId(id);
        notificationsJsonRef.current = '[]';
        setHistory([]);
        setNotifications([]);
        setPending([]);
        setLoadedKey(id);
        setChatId(id);
      }
    } catch {
      // keep the UI in a valid state so the user can retry
    } finally {
      creatingRef.current = false;
      setCreating(false);
    }
  }, []);

  const openChat = useCallback(() => {
    setChatOpen(true);
    if (chatId) return;
    const stored = readStoredChatId();
    if (stored) {
      setChatId(stored);
      return;
    }
    void startNewChat();
  }, [chatId, startNewChat]);

  const closeChat = useCallback(() => {
    setChatOpen(false);
  }, []);

  const reload = useCallback(() => {
    setRefreshToken((value) => value + 1);
  }, []);

  const sendMessage = useCallback(
    async (text: string) => {
      const message = text.trim();
      if (!message) return;
      if (!chatId || sending) return;

      const pendingId = `pending-${crypto.randomUUID()}`;
      setPending((prev) => [
        ...prev,
        {
          kind: 'user',
          id: pendingId,
          message,
          imageUrl: null,
          imageType: null,
          createdAt: new Date().toISOString(),
        },
      ]);

      setSending(true);
      try {
        const agent = waitingAgentType(notifications);

        const result = await storeService.chatWithAgent(
          STORE_ID,
          chatId,
          {
            chatSignture: chatId,
            storeId: STORE_ID,
            message,
            imageUri: null,
            agentType: agent ?? FRESH_MESSAGE_AGENT_HINT,
          },
          agent !== null,
        );

        setPending((prev) => prev.filter((item) => item.id !== pendingId));
        if (result.isSuccess) reload();
      } catch {
        setPending((prev) => prev.filter((item) => item.id !== pendingId));
      } finally {
        setSending(false);
      }
    },
    [chatId, sending, notifications, reload],
  );

  const value = useMemo<ChatContextValue>(
    () => ({
      chatOpen,
      openChat,
      closeChat,
      chatId,
      timeline,
      notifications,
      loading,
      creating,
      sending,
      hasMessages,
      startNewChat,
      sendMessage,
      reload,
    }),
    [
      chatOpen,
      openChat,
      closeChat,
      chatId,
      timeline,
      notifications,
      loading,
      creating,
      sending,
      hasMessages,
      startNewChat,
      sendMessage,
      reload,
    ],
  );

  return <ChatContext.Provider value={value}>{children}</ChatContext.Provider>;
}

export function useChat(): ChatContextValue {
  const ctx = useContext(ChatContext);
  if (!ctx) throw new Error('useChat must be used within a ChatProvider');
  return ctx;
}
