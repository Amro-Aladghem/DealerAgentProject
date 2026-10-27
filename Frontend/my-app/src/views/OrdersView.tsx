import { useEffect, useState } from 'react';
import { PackageOpen } from 'lucide-react';
import { EmptyState } from '@/components/ui/EmptyState';
import { ErrorState } from '@/components/ui/ErrorState';
import { LoadingState } from '@/components/ui/LoadingState';
import { useAsyncData } from '@/hooks/useAsyncData';
import { STORE_ID } from '@/libs/config';
import { formatDate, formatPrice } from '@/libs/format';
import storeService from '@/services/storeService';
import type { ApiResponse } from '@/types/api';
import type { CarDto } from '@/types/car';
import type { CarAccessoryDto } from '@/types/carAccessory';
import { orderStatusLabels } from '@/types/enums';
import { eOrderItemType, eOrderStatus } from '@/types/enums';
import type { OrderItemDto } from '@/types/order';
import { cn } from '@/libs/cn';

interface Catalog {
  cars: CarDto[];
  accessories: CarAccessoryDto[];
}

const statusClasses: Record<number, string> = {
  [eOrderStatus.Pending]: 'bg-amber-50 text-amber-700',
  [eOrderStatus.Confirmed]: 'bg-sky-50 text-sky-700',
  [eOrderStatus.Completed]: 'bg-success-soft text-success',
  [eOrderStatus.Cancelled]: 'bg-danger-soft text-danger',
};

export function OrdersView() {
  const orders = useAsyncData(() => storeService.getOrders(STORE_ID), []);
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

  const [itemsByOrder, setItemsByOrder] = useState<Record<string, OrderItemDto[]>>({});
  const [itemsLoading, setItemsLoading] = useState(false);

  useEffect(() => {
    if (!orders.data || orders.data.length === 0) return;

    let active = true;
    setItemsLoading(true);

    Promise.all(
      orders.data.map(async (order) => {
        if (!order.id) return { orderId: '', items: [] as OrderItemDto[] };
        const response = await storeService.getOrderItems(STORE_ID, order.id);
        return {
          orderId: order.id,
          items: response.isSuccess ? response.data : [],
        };
      }),
    ).then((results) => {
      if (!active) return;
      setItemsByOrder(
        results.reduce<Record<string, OrderItemDto[]>>((acc, result) => {
          if (result.orderId) acc[result.orderId] = result.items;
          return acc;
        }, {}),
      );
      setItemsLoading(false);
    });

    return () => {
      active = false;
    };
  }, [orders.data]);

  const resolveProductName = (item: OrderItemDto) => {
    if (!catalog.data) return 'Product';

    if (item.itemType === eOrderItemType.Car) {
      return (
        catalog.data.cars.find((car) => car.id === item.productId)?.name ?? 'Car'
      );
    }

    return (
      catalog.data.accessories.find((accessory) => accessory.id === item.productId)
        ?.name ?? 'Accessory'
    );
  };

  if (orders.loading) return <LoadingState label="Loading your orders…" />;

  if (orders.error) {
    return (
      <div className="mx-auto max-w-4xl px-4 py-10 sm:px-6">
        <ErrorState message={orders.error} onRetry={orders.reload} />
      </div>
    );
  }

  const list = orders.data ?? [];

  return (
    <div className="mx-auto max-w-4xl px-4 py-6 sm:px-6 sm:py-10">
      <div className="mb-6">
        <h1 className="text-2xl font-semibold tracking-tight text-ink">Orders</h1>
        <p className="mt-1 text-sm text-muted">
          {list.length > 0
            ? `${list.length} order${list.length === 1 ? '' : 's'} placed with this store`
            : 'Your past orders will appear here'}
        </p>
      </div>

      {list.length === 0 ? (
        <EmptyState
          icon={<PackageOpen size={22} />}
          title="No orders yet"
          description="Once you place an order with the dealership, it will show up here."
        />
      ) : (
        <div className="flex flex-col gap-4">
          {list.map((order) => {
            const items = order.id ? itemsByOrder[order.id] ?? [] : [];

            return (
              <article
                key={order.id}
                className="overflow-hidden rounded-2xl border border-line bg-surface"
              >
                <header className="flex flex-wrap items-center justify-between gap-3 border-b border-line px-4 py-3.5">
                  <div>
                    <p className="text-sm font-semibold text-ink">
                      Order #{order.id?.slice(0, 8).toUpperCase()}
                    </p>
                    <p className="mt-0.5 text-xs text-muted">
                      {formatDate(order.createdAt)}
                    </p>
                  </div>

                  <div className="flex items-center gap-3">
                    <span
                      className={cn(
                        'rounded-full px-2.5 py-1 text-xs font-semibold',
                        statusClasses[order.status] ?? 'bg-primary-soft text-primary-dark',
                      )}
                    >
                      {orderStatusLabels[order.status] ?? 'Unknown'}
                    </span>
                    <p className="text-sm font-semibold text-ink">
                      {formatPrice(order.totalAmount)}
                    </p>
                  </div>
                </header>

                {itemsLoading && items.length === 0 ? (
                  <p className="px-4 py-3 text-xs text-muted">Loading items…</p>
                ) : items.length === 0 ? (
                  <p className="px-4 py-3 text-xs text-muted">No items on this order.</p>
                ) : (
                  <ul className="divide-y divide-line">
                    {items.map((item) => (
                      <li
                        key={item.id}
                        className="flex items-center justify-between gap-3 px-4 py-2.5"
                      >
                        <div className="min-w-0">
                          <p className="truncate text-sm text-ink">
                            {resolveProductName(item)}
                          </p>
                          <p className="text-xs text-muted">
                            Qty {item.quantity} × {formatPrice(item.unitPrice)}
                          </p>
                        </div>
                        <p className="text-sm font-medium text-ink">
                          {formatPrice(item.unitPrice * item.quantity)}
                        </p>
                      </li>
                    ))}
                  </ul>
                )}
              </article>
            );
          })}
        </div>
      )}
    </div>
  );
}
