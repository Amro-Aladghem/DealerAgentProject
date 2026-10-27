import { CarFront, PackageSearch } from 'lucide-react';
import { ProductCard } from '@/components/products/ProductCard';
import { EmptyState } from '@/components/ui/EmptyState';
import { ErrorState } from '@/components/ui/ErrorState';
import { LoadingState } from '@/components/ui/LoadingState';
import { useAsyncData } from '@/hooks/useAsyncData';
import { STORE_ID } from '@/libs/config';
import storeService from '@/services/storeService';
import { carMakeNames } from '@/types/car';

const sectionTitleClass = 'text-xl font-semibold tracking-tight text-ink sm:text-2xl';
const sectionHintClass = 'text-sm text-muted';

function SectionHeading({ title, hint }: { title: string; hint?: string }) {
  return (
    <div className="mb-4 flex items-end justify-between gap-4">
      <h2 className={sectionTitleClass}>{title}</h2>
      {hint && <p className={sectionHintClass}>{hint}</p>}
    </div>
  );
}

export function HomeView() {
  const cars = useAsyncData(() => storeService.getCars(STORE_ID), []);
  const accessories = useAsyncData(() => storeService.getAccessories(STORE_ID), []);

  return (
    <div className="mx-auto max-w-6xl px-4 py-6 sm:px-6 sm:py-10">
      <section className="overflow-hidden rounded-3xl border border-line bg-surface">
        <div className="flex flex-col gap-4 px-6 py-10 sm:px-10 sm:py-14">
          <h1 className="max-w-xl text-3xl font-semibold tracking-tight text-ink sm:text-4xl">
            Find the car you want, with the parts to match.
          </h1>
          <p className="max-w-lg text-sm text-muted sm:text-base">
            Browse our available cars and accessories, review your cart and track your orders.
          </p>
        </div>
      </section>

      <section id="cars" className="mt-10">
        <SectionHeading
          title="Cars"
          hint={cars.data ? `${cars.data.length} available` : undefined}
        />

        {cars.loading && <LoadingState label="Loading cars…" />}
        {!cars.loading && cars.error && (
          <ErrorState message={cars.error} onRetry={cars.reload} />
        )}
        {!cars.loading && !cars.error && cars.data && cars.data.length === 0 && (
          <EmptyState
            icon={<CarFront size={22} />}
            title="No cars yet"
            description="Cars added to this store will appear here."
          />
        )}
        {!cars.loading && !cars.error && cars.data && cars.data.length > 0 && (
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {cars.data.map((car) => (
              <ProductCard
                key={car.id}
                to={`/cars/${car.id}`}
                title={car.name}
                subtitle={carMakeNames[car.carMakeId] ?? 'Car'}
                meta={car.modelYear ? `${car.modelYear}` : car.description}
                price={car.price}
                imageUrl={car.imageUrl}
                inStock={car.quantity > 0}
              />
            ))}
          </div>
        )}
      </section>

      <section id="accessories" className="mt-12">
        <SectionHeading
          title="Accessories"
          hint={accessories.data ? `${accessories.data.length} products` : undefined}
        />

        {accessories.loading && <LoadingState label="Loading accessories…" />}
        {!accessories.loading && accessories.error && (
          <ErrorState message={accessories.error} onRetry={accessories.reload} />
        )}
        {!accessories.loading &&
          !accessories.error &&
          accessories.data &&
          accessories.data.length === 0 && (
            <EmptyState
              icon={<PackageSearch size={22} />}
              title="No accessories yet"
              description="Accessories added to this store will appear here."
            />
          )}
        {!accessories.loading &&
          !accessories.error &&
          accessories.data &&
          accessories.data.length > 0 && (
            <div className="grid grid-cols-2 gap-4 lg:grid-cols-4">
              {accessories.data.map((accessory) => (
                <ProductCard
                  key={accessory.id}
                  to={`/accessories/${accessory.id}`}
                  title={accessory.name}
                  subtitle="Accessory"
                  price={accessory.price}
                  imageUrl={accessory.imageUrl}
                  inStock={accessory.quantity > 0}
                />
              ))}
            </div>
          )}
      </section>
    </div>
  );
}
