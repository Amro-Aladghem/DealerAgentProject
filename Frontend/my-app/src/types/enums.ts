export const eOrderItemType = {
  Car: 1,
  CarAccessory: 2,
} as const;

export type eOrderItemType = (typeof eOrderItemType)[keyof typeof eOrderItemType];

export const eOrderStatus = {
  Pending: 1,
  Confirmed: 2,
  Cancelled: 3,
  Completed: 4,
} as const;

export type eOrderStatus = (typeof eOrderStatus)[keyof typeof eOrderStatus];

export const eNotificationStatus = {
  Queued: 1,
  Running: 2,
  WaitingForUser: 3,
  Completed: 4,
  Failed: 5,
} as const;

export type eNotificationStatus = (typeof eNotificationStatus)[keyof typeof eNotificationStatus];

export const eChatRole = {
  DealerAgent: 1,
  AdvisorAgent: 2,
  ProductsSearchAgent: 3,
  User: 4,
  Tool: 5,
} as const;

export type eChatRole = (typeof eChatRole)[keyof typeof eChatRole];

export type AgentType =
  | 'DealerAgent'
  | 'AdvisorAgent'
  | 'ProductsSearchAgent'
  | 'ComplaintAgent';

export type NotificationStatus =
  | 'Queued'
  | 'Running'
  | 'WaitingForUser'
  | 'Completed'
  | 'Failed';

export const orderStatusLabels: Record<number, string> = {
  [eOrderStatus.Pending]: 'Pending',
  [eOrderStatus.Confirmed]: 'Confirmed',
  [eOrderStatus.Cancelled]: 'Cancelled',
  [eOrderStatus.Completed]: 'Completed',
};

export const orderItemTypeLabels: Record<number, string> = {
  [eOrderItemType.Car]: 'Car',
  [eOrderItemType.CarAccessory]: 'Accessory',
};
