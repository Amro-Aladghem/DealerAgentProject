import api from '@/services/api';
import type { ApiResponse } from '@/types/api';
import type { AgentRequestDto, ChatDto } from '@/types/chat';
import type { CarDto } from '@/types/car';
import type { CarAccessoryDto } from '@/types/carAccessory';
import type { CartDto, CartItemDto } from '@/types/cart';
import type { OrderDto, OrderItemDto } from '@/types/order';
import type { NotificationDto } from '@/types/notification';

const baseUri = '/stores';

const toFailure = (error: unknown): ApiResponse<never> => {
  const { message, statusCode } = error as { message: string; statusCode: number };
  return { isSuccess: false, error: message, statusCode };
};

export const storeService = {
  // ---------- chats ----------
  async createChat(storeId: string, chatDto: ChatDto): Promise<ApiResponse<ChatDto>> {
    try {
      const response = await api.post<ChatDto>(`${baseUri}/${storeId}`, chatDto);
      return { isSuccess: true, data: response.data };
    } catch (error) {
      return toFailure(error);
    }
  },

  async chatWithAgent(
    storeId: string,
    chatSignature: string,
    agentRequestDto: AgentRequestDto,
    askUser = false,
  ): Promise<ApiResponse<string>> {
    try {
      const response = await api.post<string>(
        `${baseUri}/${storeId}/chats/${chatSignature}?askUser=${askUser}`,
        agentRequestDto,
      );
      return { isSuccess: true, data: response.data };
    } catch (error) {
      return toFailure(error);
    }
  },

  async getChatHistory(
    storeId: string,
    chatSignature: string,
  ): Promise<ApiResponse<ChatDto[]>> {
    try {
      const response = await api.get<ChatDto[] | null>(
        `${baseUri}/${storeId}/chats/${chatSignature}`,
      );
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },

  async getNotificationsForChat(
    storeId: string,
    chatSignature: string,
  ): Promise<ApiResponse<NotificationDto[]>> {
    try {
      const response = await api.get<NotificationDto[]>(
        `${baseUri}/${storeId}/chats/${chatSignature}/notifications`,
      );
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },

  // ---------- cars ----------
  async getCars(storeId: string): Promise<ApiResponse<CarDto[]>> {
    try {
      const response = await api.get<CarDto[]>(`${baseUri}/${storeId}/cars`);
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },

  async getCarDetails(storeId: string, carId: string): Promise<ApiResponse<CarDto>> {
    try {
      const response = await api.get<CarDto>(`${baseUri}/${storeId}/cars/${carId}`);
      return { isSuccess: true, data: response.data };
    } catch (error) {
      return toFailure(error);
    }
  },

  // ---------- accessories ----------
  async getAccessories(storeId: string): Promise<ApiResponse<CarAccessoryDto[]>> {
    try {
      const response = await api.get<CarAccessoryDto[]>(`${baseUri}/${storeId}/accessories`);
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },

  async getAccessoryDetails(
    storeId: string,
    accessoryId: string,
  ): Promise<ApiResponse<CarAccessoryDto>> {
    try {
      const response = await api.get<CarAccessoryDto>(
        `${baseUri}/${storeId}/accessories/${accessoryId}`,
      );
      return { isSuccess: true, data: response.data };
    } catch (error) {
      return toFailure(error);
    }
  },

  // ---------- cart ----------
  async getCart(storeId: string): Promise<ApiResponse<CartDto>> {
    try {
      const response = await api.get<CartDto>(`${baseUri}/${storeId}/cart`);
      return { isSuccess: true, data: response.data };
    } catch (error) {
      return toFailure(error);
    }
  },

  async getCartItems(storeId: string, cartId: string): Promise<ApiResponse<CartItemDto[]>> {
    try {
      const response = await api.get<CartItemDto[]>(
        `${baseUri}/${storeId}/cart/${cartId}/items`,
      );
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },

  // ---------- orders ----------
  async getOrders(storeId: string): Promise<ApiResponse<OrderDto[]>> {
    try {
      const response = await api.get<OrderDto[]>(`${baseUri}/${storeId}/orders`);
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },

  async getOrderItems(
    storeId: string,
    orderId: string,
  ): Promise<ApiResponse<OrderItemDto[]>> {
    try {
      const response = await api.get<OrderItemDto[]>(
        `${baseUri}/${storeId}/orders/${orderId}/items`,
      );
      return { isSuccess: true, data: Array.isArray(response.data) ? response.data : [] };
    } catch (error) {
      return toFailure(error);
    }
  },
};

export default storeService;
