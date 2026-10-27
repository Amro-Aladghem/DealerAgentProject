import type { eOrderItemType } from '@/types/enums';

export interface CartDto {
  id?: string | null;
  storeId: string;
}

export interface CartItemDto {
  id?: string | null;
  cartId: string;
  productId: string;
  itemType: eOrderItemType;
  quantity: number;
}
