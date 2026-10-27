import type { MouseEvent, ReactNode } from 'react';
import { ArrowRight } from 'lucide-react';
import { ProductImage } from '@/components/ui/ProductImage';
import { formatPrice } from '@/libs/format';
import { useNavigate } from '@/libs/router';
import { cn } from '@/libs/cn';

interface ProductCardProps {
  to: string;
  title: string;
  subtitle?: string | null;
  meta?: string | null;
  price: number;
  imageUrl?: string | null;
  inStock?: boolean;
  badge?: ReactNode;
}

export function ProductCard({
  to,
  title,
  subtitle,
  meta,
  price,
  imageUrl,
  inStock = true,
  badge,
}: ProductCardProps) {
  const navigate = useNavigate();

  const handleClick = (event: MouseEvent<HTMLAnchorElement>) => {
    event.preventDefault();
    navigate(to);
  };

  return (
    <a
      href={to}
      onClick={handleClick}
      className="group flex flex-col overflow-hidden rounded-2xl border border-line bg-surface transition-colors hover:border-primary/40"
    >
      <div className="relative">
        <ProductImage
          src={imageUrl}
          alt={title}
          className="aspect-[4/3] w-full object-cover"
          fallbackClassName="aspect-[4/3] w-full"
        />
        {badge && <div className="absolute left-3 top-3">{badge}</div>}
      </div>

      <div className="flex flex-1 flex-col gap-1.5 p-4">
        {subtitle && (
          <p className="text-[11px] font-semibold uppercase tracking-wider text-muted">
            {subtitle}
          </p>
        )}

        <h3 className="line-clamp-1 text-base font-semibold text-ink">{title}</h3>

        {meta && <p className="text-sm text-muted">{meta}</p>}

        <div className="mt-auto flex items-center justify-between gap-3 pt-3">
          <div>
            <p className="text-lg font-semibold text-ink">{formatPrice(price)}</p>
            <p
              className={cn(
                'text-xs font-medium',
                inStock ? 'text-success' : 'text-danger',
              )}
            >
              {inStock ? 'In stock' : 'Out of stock'}
            </p>
          </div>

          <span className="inline-flex h-9 w-9 items-center justify-center rounded-full bg-primary-soft text-primary-dark">
            <ArrowRight size={17} />
          </span>
        </div>
      </div>
    </a>
  );
}
