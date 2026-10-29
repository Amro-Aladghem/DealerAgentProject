import { cn } from '@/libs/cn';

interface DealerAvatarProps {
  active?: boolean;
  className?: string;
  rounded?: 'full' | '2xl';
}

export function DealerAvatar({ active = false, className, rounded = 'full' }: DealerAvatarProps) {
  return (
    <span
      className={cn(
        'relative inline-flex shrink-0 overflow-hidden border border-line bg-white',
        rounded === 'full' ? 'rounded-full' : 'rounded-2xl',
        className,
      )}
    >
      {active ? (
        <video
          src="/DealerAgent.mp4"
          autoPlay
          loop
          muted
          playsInline
          preload="metadata"
          aria-hidden="true"
          className="h-full w-full object-cover"
        />
      ) : (
        <img
          src="/DealerAgent.jpg"
          alt="Dealer Agent"
          draggable={false}
          className="h-full w-full object-cover"
        />
      )}
    </span>
  );
}
