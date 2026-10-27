import type { eOrderItemType, eOrderStatus } from '@/types/enums';

export interface OrderDto {
  id?: string | null;
  storeId: string;
  createdAt: string;
  status: eOrderStatus;
  totalAmount: number;
}

export interface OrderItemDto {
  id?: string | null;
  orderId: string;
  productId: string;
  itemType: eOrderItemType;
  quantity: number;
  unitPrice: number;
}
