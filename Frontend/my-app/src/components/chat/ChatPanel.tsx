import { useEffect, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { Plus, RefreshCw, X } from 'lucide-react';
import { useChat } from '@/contexts/ChatContext';
import { AgentExecutionGroup } from '@/components/chat/AgentExecutionGroup';
import { ChatComposer } from '@/components/chat/ChatComposer';
import { DealerAvatar } from '@/components/chat/DealerAvatar';
import { Button } from '@/components/ui/Button';
import { LoadingState } from '@/components/ui/LoadingState';
import { IconButton } from '@/components/ui/IconButton';

export function ChatPanel() {
  const {
    chatOpen,
    closeChat,
    chatId,
    timeline,
    loading,
    creating,
    sending,
    startNewChat,
    sendMessage,
  } = useChat();
  const [focusToken, setFocusToken] = useState(0);
  const endRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    if (!chatOpen) return;
    setFocusToken((value) => value + 1);
  }, [chatOpen]);

  useEffect(() => {
    endRef.current?.scrollIntoView({ block: 'end' });
  }, [timeline]);

  if (!chatOpen) return null;

  return createPortal(
    <div
      className="fixed inset-0 z-50 flex items-end justify-center bg-black/40 backdrop-blur-[1px] sm:items-center sm:p-6"
      onMouseDown={closeChat}
      role="presentation"
    >
      <div
        role="dialog"
        aria-modal="true"
        aria-label="Chat with our agent"
        onMouseDown={(event) => event.stopPropagation()}
        className="flex h-[88dvh] w-full max-w-2xl animate-slide-up flex-col overflow-hidden rounded-t-3xl bg-surface shadow-xl sm:animate-pop-in sm:rounded-3xl"
      >
        <header className="flex items-center justify-between gap-3 border-b border-line px-4 py-3">
          <div className="flex min-w-0 items-center gap-2.5">
            <DealerAvatar className="h-10 w-10" />
            <div className="min-w-0">
              <p className="truncate text-sm font-semibold text-ink">Dealer Agent</p>
              <p className="truncate text-xs text-muted">
                Cars, accessories, orders & complaints
              </p>
            </div>
          </div>

          <div className="flex items-center gap-1">
            <IconButton
              label="Start a new chat"
              onClick={() => {
                void startNewChat();
                setFocusToken((value) => value + 1);
              }}
              disabled={creating}
            >
              <Plus size={18} />
            </IconButton>
            <IconButton label="Close chat" onClick={closeChat}>
              <X size={18} />
            </IconButton>
          </div>
        </header>

        <div className="flex-1 overflow-y-auto px-4 py-4">
          <div className="mx-auto flex w-full flex-col gap-2">
            {creating || loading ? (
              <LoadingState label="Preparing your chat…" />
            ) : !chatId ? (
              <div className="mt-6 flex flex-col items-center gap-3 text-center">
                <p className="text-sm text-muted">
                  Couldn't start the chat. Try again.
                </p>
                <Button
                  variant="secondary"
                  size="sm"
                  onClick={() => void startNewChat()}
                >
                  <RefreshCw size={14} />
                  Retry
                </Button>
              </div>
            ) : timeline.length === 0 ? (
              <div className="mt-4 flex flex-col items-center gap-3 text-center">
                <DealerAvatar active rounded="2xl" className="h-40 w-40 shadow-sm" />
                <div>
                  <p className="text-sm font-semibold text-ink">Hi, I'm your Dealer Agent</p>
                  <p className="mt-1 max-w-xs text-sm text-muted">
                    Ask me anything about our cars, accessories or your orders.
                  </p>
                </div>
              </div>
            ) : (
              timeline.map((item) =>
                item.kind === 'user' ? (
                  <div key={item.id} className="flex justify-end">
                    <div className="max-w-[80%] overflow-hidden rounded-2xl rounded-br-sm bg-primary px-3 py-2 text-sm text-white">
                      {item.message && (
                        <p dir="auto" className="whitespace-pre-wrap break-words">
                          {item.message}
                        </p>
                      )}
                    </div>
                  </div>
                ) : (
                  <AgentExecutionGroup
                    key={item.id}
                    group={item}
                    onOpenProduct={closeChat}
                  />
                ),
              )
            )}
            <div ref={endRef} />
          </div>
        </div>

        <div className="border-t border-line bg-surface px-4 py-3">
          <div className="mx-auto w-full">
            <ChatComposer
              onSubmit={sendMessage}
              focusToken={focusToken}
              loading={creating || sending}
            />
          </div>
        </div>
      </div>
    </div>,
    document.body,
  );
}
