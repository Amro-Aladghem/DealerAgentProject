import { useState } from 'react';
import { Car } from 'lucide-react';
import { NavLink } from '@/libs/router';

export function Logo() {
  const [failed, setFailed] = useState(false);

  return (
    <NavLink to="/" className="flex items-center gap-2" aria-label="DealerAgent home">
      {failed ? (
        <>
          <span className="flex h-9 w-9 items-center justify-center rounded-xl bg-primary text-white">
            <Car size={20} />
          </span>
          <span className="text-lg font-semibold tracking-tight text-ink">DealerAgent</span>
        </>
      ) : (
        <img
          src="/logo-mark-light.png"
          alt="DealerAgent — Car Agent"
          className="h-10 w-auto"
          draggable={false}
          onError={() => setFailed(true)}
        />
      )}
    </NavLink>
  );
}
