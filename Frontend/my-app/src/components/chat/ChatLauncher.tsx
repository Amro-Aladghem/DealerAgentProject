import { useChat } from '@/contexts/ChatContext';

export function ChatLauncher() {
  const { chatOpen, openChat } = useChat();

  if (chatOpen) return null;

  return (
    <button
      type="button"
      aria-label="Chat with our agent"
      onClick={openChat}
      className="fixed bottom-5 right-5 z-40 flex h-16 w-16 items-center justify-center rounded-full border-2 border-primary bg-white shadow-lg shadow-primary/25 transition-colors hover:border-primary-dark focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary-dark"
    >
      <img
        src="/DealerAgent.jpg"
        alt=""
        draggable={false}
        className="h-full w-full rounded-full object-cover"
      />
    </button>
  );
}
