import { ShoppingCart } from 'lucide-react';
import { EmptyState } from '@/components/ui/EmptyState';
import { ErrorState } from '@/components/ui/ErrorState';
import { LoadingState } from '@/components/ui/LoadingState';
import { ProductImage } from '@/components/ui/ProductImage';
import { useCart } from '@/contexts/CartContext';
import { useAsyncData } from '@/hooks/useAsyncData';
import { STORE_ID } from '@/libs/config';
import { formatPrice } from '@/libs/format';
import { useNavigate } from '@/libs/router';
import storeService from '@/services/storeService';
import type { ApiResponse } from '@/types/api';
import type { CarDto } from '@/types/car';
import type { CarAccessoryDto } from '@/types/carAccessory';
import type { CartItemDto } from '@/types/cart';
import { eOrderItemType } from '@/types/enums';

interface Catalog {
  cars: CarDto[];
  accessories: CarAccessoryDto[];
}

export function CartView() {
  const navigate = useNavigate();
  const { cart, items, loading, reload } = useCart();
  const hasItems = items.length > 0;

  const catalog = useAsyncData<Catalog>(async (): Promise<ApiResponse<Catalog>> => {
    const [carsResponse, accessoriesResponse] = await Promise.all([
      storeService.getCars(STORE_ID),
      storeService.getAccessories(STORE_ID),
    ]);

    if (!carsResponse.isSuccess) return carsResponse;
    if (!accessoriesResponse.isSuccess) return accessoriesResponse;

    return {
      isSuccess: true,
      data: { cars: carsResponse.data, accessories: accessoriesResponse.data },
    };
  }, []);

  const resolveProduct = (item: CartItemDto) => {
    if (!catalog.data) return null;

    if (item.itemType === eOrderItemType.Car) {
      const car = catalog.data.cars.find((entry) => entry.id === item.productId);
      return car
        ? { name: car.name, price: car.price, imageUrl: car.imageUrl, type: 'Car' }
        : null;
    }

    const accessory = catalog.data.accessories.find(
      (entry) => entry.id === item.productId,
    );
    return accessory
      ? {
          name: accessory.name,
          price: accessory.price,
          imageUrl: accessory.imageUrl,
          type: 'Accessory',
        }
      : null;
  };

  const subtotal = items.reduce((total, item) => {
    const product = resolveProduct(item);
    return total + (product ? product.price * item.quantity : 0);
  }, 0);

  return (
    <div className="mx-auto max-w-4xl px-4 py-6 sm:px-6 sm:py-10">
      <div className="mb-6 flex items-end justify-between gap-4">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight text-ink">Your cart</h1>
          <p className="mt-1 text-sm text-muted">
            {hasItems
              ? `${items.length} item${items.length === 1 ? '' : 's'} ready for review`
              : 'Everything you picked will show up here'}
          </p>
        </div>
        {hasItems && (
          <button
            type="button"
            onClick={reload}
            className="text-sm font-medium text-primary-dark transition-colors hover:text-primary"
          >
            Refresh
          </button>
        )}
      </div>

      {loading ? (
        <LoadingState label="Loading your cart…" />
      ) : !cart || !hasItems ? (
        <EmptyState
          icon={<ShoppingCart size={22} />}
          title="Your cart is empty"
          description="Explore our cars and accessories to start building your cart."
          action={
            <button
              type="button"
              onClick={() => navigate('/')}
              className="rounded-xl bg-primary px-4 py-2.5 text-sm font-medium text-white transition-colors hover:bg-primary-dark"
            >
              Browse products
            </button>
          }
        />
      ) : catalog.loading ? (
        <LoadingState label="Loading cart items…" />
      ) : catalog.error ? (
        <ErrorState message={catalog.error} onRetry={catalog.reload} />
      ) : (
        <div className="overflow-hidden rounded-2xl border border-line bg-surface">
          <ul className="divide-y divide-line">
            {items.map((item) => {
              const product = resolveProduct(item);

              return (
                <li key={item.id} className="flex items-center gap-3 p-4 sm:gap-4">
                  <ProductImage
                    src={product?.imageUrl}
                    alt={product?.name ?? 'Product'}
                    className="h-16 w-16 shrink-0 rounded-xl object-cover"
                    fallbackClassName="h-16 w-16 shrink-0 rounded-xl"
                  />

                  <div className="min-w-0 flex-1">
                    <p className="truncate text-sm font-semibold text-ink">
                      {product?.name ?? 'Unknown product'}
                    </p>
                    <p className="mt-0.5 text-xs text-muted">
                      {product?.type ?? 'Product'} · Qty {item.quantity}
                    </p>
                  </div>

                  <div className="text-right">
                    <p className="text-sm font-semibold text-ink">
                      {formatPrice(product ? product.price * item.quantity : null)}
                    </p>
                    {product && (
                      <p className="mt-0.5 text-xs text-muted">
                        {formatPrice(product.price)} each
                      </p>
                    )}
                  </div>
                </li>
              );
            })}
          </ul>

          <div className="flex items-center justify-between gap-4 border-t border-line bg-canvas px-4 py-4">
            <div>
              <p className="text-sm text-muted">Subtotal</p>
              <p className="text-lg font-semibold text-ink">{formatPrice(subtotal)}</p>
            </div>
            <p className="max-w-[220px] text-right text-xs text-muted">
              Contact the dealership to confirm and complete your order.
            </p>
          </div>
        </div>
      )}
    </div>
  );
}
