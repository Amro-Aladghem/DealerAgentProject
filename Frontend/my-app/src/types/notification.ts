export interface NotificationDto {
  id: string;
  storeId: string;
  chatId: string;
  text: string;
  notificationStatus: string;
  agentType: string;
  data?: string | null;
  updatedAt: string;
}
