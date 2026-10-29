import { Loader2, MessageCircle } from 'lucide-react';
import { ChatImage } from '@/components/chat/ChatImage';
import { DealerAvatar } from '@/components/chat/DealerAvatar';
import { MessageCards } from '@/components/chat/MessageCards';
import { Markdown } from '@/components/ui/Markdown';
import { statusLabel } from '@/components/notifications/notificationPresentation';
import { parseAgentResponseData, resolveMessageText } from '@/libs/chatGrouping';
import { cn } from '@/libs/cn';
import type { NotificationStatus } from '@/types/enums';
import type { ChatExecutionGroup as ChatExecutionGroupType } from '@/types/chatSession';

const statusStyles: Record<NotificationStatus, string> = {
  Queued: 'bg-ink/5 text-muted',
  Running: 'bg-primary-soft text-primary-dark',
  WaitingForUser: 'bg-amber-50 text-amber-700',
  Completed: 'bg-primary-soft text-primary-dark',
  Failed: 'bg-danger-soft text-danger',
};

interface AgentExecutionGroupProps {
  group: ChatExecutionGroupType;
  onOpenProduct?: () => void;
}

export function AgentExecutionGroup({ group, onOpenProduct }: AgentExecutionGroupProps) {
  const status = group.status;
  const waiting = status === 'WaitingForUser';
  const busy = status === 'Running' || status === 'Queued';

  return (
    <div className="flex justify-start">
      <div className="flex max-w-[85%] gap-2">
        <div className="w-8 shrink-0 pt-1">
          <DealerAvatar active={busy} className="h-8 w-8" />
        </div>

        <div className="flex min-w-0 flex-col gap-1.5">
          <div className="flex items-center gap-2">
            <span className="text-[11px] font-medium text-muted">Dealer Agent</span>
            {status !== null && (
              <span
                className={cn(
                  'inline-flex items-center gap-1 rounded-full px-2 py-0.5 text-[10px] font-medium',
                  statusStyles[status],
                )}
              >
                {busy && <Loader2 size={11} className="animate-spin" aria-hidden="true" />}
                {waiting && <MessageCircle size={11} aria-hidden="true" />}
                {statusLabel[status]}
              </span>
            )}
          </div>

          {group.messages.length === 0 ? (
            <div className="flex items-center gap-2.5 rounded-2xl rounded-tl-sm border border-line bg-surface px-2.5 py-2">
              <DealerAvatar active className="h-10 w-10" />
              <div className="min-w-0">
                <p className="text-xs font-semibold text-ink">Dealer Agent</p>
                <p className="text-xs text-muted">
                  {waiting ? 'Waiting for your answer…' : 'Thinking…'}
                </p>
              </div>
            </div>
          ) : (
            group.messages.map((message, index) => {
              const text = resolveMessageText(message.message);
              const data = parseAgentResponseData(message.data);

              return (
                <div
                  key={`${group.id}-${index}`}
                  className="overflow-hidden rounded-2xl rounded-tl-sm border border-line bg-surface px-3 py-2 text-sm text-ink"
                >
                  <ChatImage
                    src={message.imageUrl}
                    className="mb-2 max-h-56 w-full rounded-lg object-cover"
                  />
                  {text && <Markdown text={text} className="break-words" />}
                  <MessageCards
                    cars={data.cars}
                    carAccessories={data.carAccessories}
                    onOpenProduct={onOpenProduct}
                  />
                </div>
              );
            })
          )}
        </div>
      </div>
    </div>
  );
}
