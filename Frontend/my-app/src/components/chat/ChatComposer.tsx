import { useEffect, useState } from 'react';
import type { FormEvent, KeyboardEvent } from 'react';
import { SendHorizontal } from 'lucide-react';
import { useAutoResizeTextarea } from '@/hooks/useAutoResizeTextarea';
import { Spinner } from '@/components/ui/Spinner';
import { cn } from '@/libs/cn';

interface ChatComposerProps {
  onSubmit: (message: string) => void | Promise<void>;
  loading?: boolean;
  placeholder?: string;
  focusToken?: number;
}

export function ChatComposer({
  onSubmit,
  loading = false,
  placeholder = 'Ask about cars, accessories, orders…',
  focusToken = 0,
}: ChatComposerProps) {
  const [value, setValue] = useState('');
  const { ref, resize } = useAutoResizeTextarea(160);

  useEffect(() => {
    resize();
  }, [value, resize]);

  useEffect(() => {
    if (focusToken > 0) ref.current?.focus();
  }, [focusToken, ref]);

  const canSend = value.trim().length > 0 && !loading;

  const submit = async () => {
    if (!canSend) return;
    const message = value.trim();
    setValue('');
    await onSubmit(message);
  };

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    void submit();
  };

  const handleKeyDown = (event: KeyboardEvent<HTMLTextAreaElement>) => {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      void submit();
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="rounded-2xl border border-line bg-surface p-2 shadow-sm focus-within:border-primary/40"
    >
        <div className="flex items-end gap-1">
          <textarea
            ref={ref}
            value={value}
            onChange={(event) => setValue(event.target.value)}
            onKeyDown={handleKeyDown}
            dir="auto"
            rows={1}
            placeholder={placeholder}
            aria-label={placeholder}
            className="max-h-40 flex-1 resize-none bg-transparent px-3 py-2.5 text-sm text-ink outline-none placeholder:text-muted"
          />
          <button
            type="submit"
            disabled={!canSend}
            aria-label="Send message"
            className={cn(
              'inline-flex h-10 w-10 items-center justify-center rounded-xl bg-primary text-white transition-colors',
              'hover:bg-primary-dark focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary',
              'disabled:cursor-not-allowed disabled:opacity-40',
            )}
          >
            {loading ? <Spinner className="text-white" /> : <SendHorizontal size={18} />}
          </button>
        </div>
      </form>
  );
}
