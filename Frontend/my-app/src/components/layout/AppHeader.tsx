import { ShoppingCart } from 'lucide-react';
import { Logo } from '@/components/layout/Logo';
import { Badge } from '@/components/ui/Badge';
import { IconButton } from '@/components/ui/IconButton';
import { useCart } from '@/contexts/CartContext';
import { cn } from '@/libs/cn';
import { NavLink, useNavigate } from '@/libs/router';

const linkClass =
  'rounded-lg px-3 py-2 text-sm font-medium text-muted transition-colors hover:text-ink';

export function AppHeader() {
  const { count } = useCart();
  const navigate = useNavigate();

  return (
    <header className="sticky top-0 z-30 border-b border-line bg-surface/90 backdrop-blur">
      <div className="mx-auto flex h-16 max-w-6xl items-center justify-between gap-3 px-4 sm:px-6">
        <Logo />

        <nav className="flex items-center gap-1">
          <NavLink
            to="/"
            className={({ isActive }) => cn(linkClass, isActive && 'text-ink')}
          >
            Home
          </NavLink>
          <NavLink
            to="/orders"
            className={({ isActive }) => cn(linkClass, isActive && 'text-ink')}
          >
            Orders
          </NavLink>

          <IconButton label="Cart" onClick={() => navigate('/cart')}>
            <ShoppingCart size={20} />
            {count > 0 && (
              <Badge className="pointer-events-none absolute -right-0.5 -top-0.5">
                {count > 99 ? '99+' : count}
              </Badge>
            )}
          </IconButton>
        </nav>
      </div>
    </header>
  );
}
