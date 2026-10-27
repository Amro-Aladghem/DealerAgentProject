export interface CarDto {
  id?: string | null;
  storeId: string;
  carMakeId: number;
  name: string;
  description?: string | null;
  price: number;
  quantity: number;
  modelYear?: number | null;
  imageUrl?: string | null;
  productUrl?: string | null;
}

export const carMakeNames: Record<number, string> = {
  1: 'Toyota',
  2: 'Honda',
  3: 'Volkswagen',
  4: 'Hyundai',
  5: 'Ford',
  6: 'BMW',
  7: 'Mercedes-Benz',
  8: 'Nissan',
  9: 'Kia',
  10: 'Audi',
};
