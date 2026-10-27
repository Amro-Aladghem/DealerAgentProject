import { useState } from 'react';
import { MessageCircle, X } from 'lucide-react';

export function ChatLauncher() {
  const [hintOpen, setHintOpen] = useState(false);

  return (
    <div className="fixed bottom-5 right-5 z-40 flex flex-col items-end gap-2">
      {hintOpen && (
        <div className="animate-fade-in max-w-[220px] rounded-2xl border border-line bg-surface px-4 py-3 text-sm shadow-sm">
          <p className="font-medium text-ink">Talk to our agents</p>
          <p className="mt-0.5 text-xs text-muted">Chat is coming soon.</p>
        </div>
      )}

      <button
        type="button"
        aria-label="Chat with our agents"
        aria-expanded={hintOpen}
        onClick={() => setHintOpen((value) => !value)}
        className="flex h-14 w-14 items-center justify-center rounded-full bg-primary text-white shadow-lg shadow-primary/25 transition-colors hover:bg-primary-dark focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary-dark"
      >
        {hintOpen ? <X size={22} /> : <MessageCircle size={24} />}
      </button>
    </div>
  );
}
