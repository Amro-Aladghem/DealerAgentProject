import type { MouseEvent } from 'react';
import { ProductImage } from '@/components/ui/ProductImage';
import { formatPrice } from '@/libs/format';
import { useNavigate } from '@/libs/router';
import { carMakeNames } from '@/types/car';
import type { CarDto } from '@/types/car';
import type { CarAccessoryDto } from '@/types/carAccessory';
import { cn } from '@/libs/cn';

interface MessageCardsProps {
  cars: CarDto[];
  carAccessories: CarAccessoryDto[];
  onOpenProduct?: () => void;
}

export function MessageCards({ cars, carAccessories, onOpenProduct }: MessageCardsProps) {
  const navigate = useNavigate();
  const hasContent = cars.length > 0 || carAccessories.length > 0;
  if (!hasContent) return null;

  const open = (to: string) => (event: MouseEvent<HTMLAnchorElement>) => {
    event.preventDefault();
    onOpenProduct?.();
    navigate(to);
  };

  return (
    <div className="scrollbar-thin mt-2 flex gap-2 overflow-x-auto pb-1">
      {cars.map((car) => (
        <a
          key={`car-${car.id ?? car.name}`}
          href={car.id ? `/cars/${car.id}` : '#'}
          onClick={car.id ? open(`/cars/${car.id}`) : undefined}
          className="flex w-40 shrink-0 flex-col gap-1.5 rounded-xl border border-line bg-canvas p-2 transition-colors hover:border-primary/40"
        >
          <ProductImage
            src={car.imageUrl}
            alt={car.name}
            className="aspect-[4/3] w-full rounded-lg object-cover"
            fallbackClassName="aspect-[4/3] w-full rounded-lg"
          />
          <div className="min-w-0">
            <p className="truncate text-xs font-semibold text-ink">{car.name}</p>
            <p className="truncate text-[10px] text-muted">
              {[
                carMakeNames[car.carMakeId] ?? 'Car',
                car.modelYear ?? null,
              ]
                .filter(Boolean)
                .join(' · ')}
            </p>
            <p className="mt-0.5 text-xs font-semibold text-ink">{formatPrice(car.price)}</p>
          </div>
        </a>
      ))}

      {carAccessories.map((accessory) => (
        <a
          key={`accessory-${accessory.id ?? accessory.name}`}
          href={accessory.id ? `/accessories/${accessory.id}` : '#'}
          onClick={accessory.id ? open(`/accessories/${accessory.id}`) : undefined}
          className={cn(
            'flex w-40 shrink-0 flex-col gap-1.5 rounded-xl border border-line bg-canvas p-2',
            'transition-colors hover:border-primary/40',
          )}
        >
          <ProductImage
            src={accessory.imageUrl}
            alt={accessory.name}
            className="aspect-[4/3] w-full rounded-lg object-cover"
            fallbackClassName="aspect-[4/3] w-full rounded-lg"
          />
          <div className="min-w-0">
            <p className="truncate text-xs font-semibold text-ink">{accessory.name}</p>
            <p className="truncate text-[10px] text-muted">Accessory</p>
            <p className="mt-0.5 text-xs font-semibold text-ink">
              {formatPrice(accessory.price)}
            </p>
          </div>
        </a>
      ))}
    </div>
  );
}
