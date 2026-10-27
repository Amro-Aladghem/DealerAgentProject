import { ArrowLeft, ExternalLink, PackageSearch } from 'lucide-react';
import { EmptyState } from '@/components/ui/EmptyState';
import { ErrorState } from '@/components/ui/ErrorState';
import { LoadingState } from '@/components/ui/LoadingState';
import { ProductImage } from '@/components/ui/ProductImage';
import { useAsyncData } from '@/hooks/useAsyncData';
import { STORE_ID } from '@/libs/config';
import { formatPrice } from '@/libs/format';
import { useNavigate } from '@/libs/router';
import storeService from '@/services/storeService';
import { carMakeNames } from '@/types/car';

interface ProductViewProps {
  kind: 'car' | 'accessory';
  id: string;
}

export function ProductView({ kind, id }: ProductViewProps) {
  const navigate = useNavigate();

  const product = useAsyncData(
    () =>
      kind === 'car'
        ? storeService.getCarDetails(STORE_ID, id)
        : storeService.getAccessoryDetails(STORE_ID, id),
    [kind, id],
  );

  const backToHome = () => navigate('/');

  if (product.loading) return <LoadingState label="Loading product…" />;

  if (product.error) {
    return (
      <div className="mx-auto max-w-4xl px-4 py-10 sm:px-6">
        <ErrorState message={product.error} onRetry={product.reload} />
      </div>
    );
  }

  if (!product.data) {
    return (
      <div className="mx-auto max-w-4xl px-4 py-10 sm:px-6">
        <EmptyState
          icon={<PackageSearch size={22} />}
          title="Product not found"
          description="This product may have been removed from the store."
        />
      </div>
    );
  }

  const item = product.data;
  const isCar = kind === 'car';
  const meta = isCar
    ? [carMakeNames[(item as { carMakeId?: number }).carMakeId ?? 0] ?? 'Car',
       (item as { modelYear?: number | null }).modelYear]
        .filter(Boolean)
        .join(' · ')
    : 'Accessory';

  return (
    <div className="mx-auto max-w-5xl px-4 py-6 sm:px-6 sm:py-10">
      <button
        type="button"
        onClick={backToHome}
        className="mb-5 inline-flex items-center gap-1.5 text-sm font-medium text-muted transition-colors hover:text-ink"
      >
        <ArrowLeft size={16} />
        Back to {isCar ? 'cars' : 'accessories'}
      </button>

      <div className="grid gap-6 md:grid-cols-2 md:items-start">
        <div className="overflow-hidden rounded-3xl border border-line bg-surface">
          <ProductImage
            src={item.imageUrl}
            alt={item.name}
            className="aspect-[4/3] w-full object-cover"
            fallbackClassName="aspect-[4/3] w-full"
          />
        </div>

        <div className="flex flex-col gap-4">
          <span className="w-fit rounded-full bg-primary-soft px-3 py-1 text-xs font-semibold text-primary-dark">
            {isCar ? 'Car' : 'Accessory'}
          </span>

          <div>
            <h1 className="text-2xl font-semibold tracking-tight text-ink sm:text-3xl">
              {item.name}
            </h1>
            <p className="mt-1 text-sm text-muted">{meta}</p>
          </div>

          <div className="flex items-baseline gap-3">
            <p className="text-2xl font-semibold text-ink">{formatPrice(item.price)}</p>
            <span
              className={
                item.quantity > 0
                  ? 'text-sm font-medium text-success'
                  : 'text-sm font-medium text-danger'
              }
            >
              {item.quantity > 0 ? `${item.quantity} in stock` : 'Out of stock'}
            </span>
          </div>

          {item.description && (
            <p className="text-sm leading-relaxed text-muted">{item.description}</p>
          )}

          {item.productUrl && (
            <a
              href={item.productUrl}
              target="_blank"
              rel="noreferrer"
              className="inline-flex w-fit items-center gap-1.5 rounded-xl border border-line bg-surface px-4 py-2.5 text-sm font-medium text-ink transition-colors hover:border-primary/40"
            >
              View product page
              <ExternalLink size={15} />
            </a>
          )}
        </div>
      </div>
    </div>
  );
}
