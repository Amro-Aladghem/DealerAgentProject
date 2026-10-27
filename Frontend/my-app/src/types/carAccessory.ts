export interface CarAccessoryDto {
  id?: string | null;
  storeId: string;
  name: string;
  description?: string | null;
  price: number;
  quantity: number;
  imageUrl?: string | null;
  productUrl?: string | null;
}
