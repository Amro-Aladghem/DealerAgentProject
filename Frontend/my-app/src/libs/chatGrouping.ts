import type { AgentType, NotificationStatus } from '@/types/enums';
import type { ChatDto } from '@/types/chat';
import type { NotificationDto } from '@/types/notification';
import type { CarDto } from '@/types/car';
import type { CarAccessoryDto } from '@/types/carAccessory';
import type {
  ChatExecutionGroup,
  ChatTimelineItem,
  ChatTimelineUserItem,
} from '@/types/chatSession';

const USER_CHAT_ROLE_ID = 4;

const roleToAgentType: Record<number, AgentType> = {
  1: 'DealerAgent',
};

const ACTIVE_STATUSES: NotificationStatus[] = [
  'Queued',
  'Running',
  'WaitingForUser',
];

interface MutableGroup extends ChatExecutionGroup {
  closed: boolean;
}

interface NotificationRun {
  agentType: AgentType;
  status: NotificationStatus;
  startedAt: string;
  updatedAt: string;
}

function buildRuns(notifications: NotificationDto[]): NotificationRun[] {
  const runs: NotificationRun[] = [];
  let current: NotificationRun | null = null;

  // The API returns notifications ordered by UpdatedAt ascending.
  for (const notification of notifications) {
    // Only the dealer agent is user-facing; internal agents are hidden.
    if (notification.agentType !== 'DealerAgent') continue;

    if (
      current === null ||
      notification.notificationStatus === 'Queued'
    ) {
      current = {
        agentType: notification.agentType,
        status: notification.notificationStatus,
        startedAt: notification.updatedAt,
        updatedAt: notification.updatedAt,
      };
      runs.push(current);
    } else {
      current.status = notification.notificationStatus;
      current.updatedAt = notification.updatedAt;
    }
  }

  return runs;
}

function findActiveRun(runs: NotificationRun[]): NotificationRun | null {
  const last = runs[runs.length - 1];
  if (!last) return null;
  return ACTIVE_STATUSES.includes(last.status) ? last : null;
}

export function buildChatTimeline(
  history: ChatDto[],
  notifications: NotificationDto[],
): ChatTimelineItem[] {
  const timeline: ChatTimelineItem[] = [];
  const groups: MutableGroup[] = [];
  let current: MutableGroup | null = null;

  for (const [index, message] of history.entries()) {
    if (message.showMessage !== true) continue;

    if (message.chatRoleId === USER_CHAT_ROLE_ID) {
      current = null;
      const item: ChatTimelineUserItem = {
        kind: 'user',
        id: `user-${index}-${message.creationTime}`,
        message: message.message,
        imageUrl: message.imageUrl,
        imageType: message.imageType,
        createdAt: message.creationTime,
      };
      timeline.push(item);
      continue;
    }

    const agentType = roleToAgentType[message.chatRoleId];
    if (agentType === undefined) continue;

    if (current === null || current.agentType !== agentType || current.closed) {
      const group: MutableGroup = {
        kind: 'execution',
        id: `exec-${index}-${message.creationTime}`,
        agentType,
        status: null,
        messages: [message],
        startedAt: message.creationTime,
        updatedAt: message.creationTime,
        closed: message.isFinalResponse === true,
      };
      current = group;
      groups.push(group);
      timeline.push(group);
    } else {
      current.messages.push(message);
      current.updatedAt = message.creationTime;
      if (message.isFinalResponse === true) current.closed = true;
    }
  }

  for (const group of groups) {
    if (group.closed) group.status = 'Completed';
  }

  const run = findActiveRun(buildRuns(notifications));
  if (run) {
    const status: NotificationStatus =
      run.status === 'WaitingForUser' ? 'WaitingForUser' : 'Running';

    if (current && !current.closed && current.agentType === run.agentType) {
      current.status = status;
    } else {
      timeline.push({
        kind: 'execution',
        id: `run-${run.startedAt}-${run.agentType}`,
        agentType: run.agentType,
        status,
        messages: [],
        startedAt: run.startedAt,
        updatedAt: run.updatedAt,
      });
    }
  }

  return timeline;
}

export function resolveMessageText(raw: string): string {
  const trimmed = raw.trim();
  if (!trimmed.startsWith('{') && !trimmed.startsWith('[')) return raw;
  try {
    const parsed: unknown = JSON.parse(trimmed);
    if (
      parsed &&
      typeof parsed === 'object' &&
      'Message' in parsed &&
      typeof (parsed as { Message?: unknown }).Message === 'string'
    ) {
      return (parsed as { Message: string }).Message;
    }
    if (
      parsed &&
      typeof parsed === 'object' &&
      'message' in parsed &&
      typeof (parsed as { message?: unknown }).message === 'string'
    ) {
      return (parsed as { message: string }).message;
    }
  } catch {
    return raw;
  }
  return raw;
}

export interface AgentResponseData {
  cars: CarDto[];
  carAccessories: CarAccessoryDto[];
}

function asString(value: unknown): string | null {
  return typeof value === 'string' ? value : null;
}

function asNumber(value: unknown): number | null {
  return typeof value === 'number' ? value : null;
}

function pick(source: Record<string, unknown>, ...keys: string[]): unknown {
  for (const key of keys) {
    if (key in source && source[key] !== undefined && source[key] !== null) {
      return source[key];
    }
  }
  return null;
}

function toCarDto(value: unknown): CarDto | null {
  if (!value || typeof value !== 'object') return null;
  const raw = value as Record<string, unknown>;
  const name = asString(pick(raw, 'name', 'Name'));
  if (!name) return null;
  return {
    id: asString(pick(raw, 'id', 'Id')) ?? '',
    storeId: asString(pick(raw, 'storeId', 'StoreId')) ?? '',
    carMakeId: asNumber(pick(raw, 'carMakeId', 'CarMakeId')) ?? 0,
    name,
    description: asString(pick(raw, 'description', 'Description')),
    price: asNumber(pick(raw, 'price', 'Price')) ?? 0,
    quantity: asNumber(pick(raw, 'quantity', 'Quantity')) ?? 0,
    modelYear: asNumber(pick(raw, 'modelYear', 'ModelYear')),
    imageUrl: asString(pick(raw, 'imageUrl', 'ImageUrl')),
    productUrl: asString(pick(raw, 'productUrl', 'ProductUrl')),
  };
}

function toAccessoryDto(value: unknown): CarAccessoryDto | null {
  if (!value || typeof value !== 'object') return null;
  const raw = value as Record<string, unknown>;
  const name = asString(pick(raw, 'name', 'Name'));
  if (!name) return null;
  return {
    id: asString(pick(raw, 'id', 'Id')) ?? '',
    storeId: asString(pick(raw, 'storeId', 'StoreId')) ?? '',
    name,
    description: asString(pick(raw, 'description', 'Description')),
    price: asNumber(pick(raw, 'price', 'Price')) ?? 0,
    quantity: asNumber(pick(raw, 'quantity', 'Quantity')) ?? 0,
    imageUrl: asString(pick(raw, 'imageUrl', 'ImageUrl')),
    productUrl: asString(pick(raw, 'productUrl', 'ProductUrl')),
  };
}

function toList(value: unknown): unknown[] {
  return Array.isArray(value) ? value : [];
}

export function parseAgentResponseData(data?: string | null): AgentResponseData {
  if (!data) return { cars: [], carAccessories: [] };
  const trimmed = data.trim();
  if (!trimmed.startsWith('{') && !trimmed.startsWith('[')) {
    return { cars: [], carAccessories: [] };
  }

  try {
    const parsed: unknown = JSON.parse(trimmed);
    if (!parsed || typeof parsed !== 'object') {
      return { cars: [], carAccessories: [] };
    }

    const raw = parsed as Record<string, unknown>;
    const cars = toList(pick(raw, 'cars', 'Cars'))
      .map(toCarDto)
      .filter((car): car is CarDto => car !== null);
    const carAccessories = toList(pick(raw, 'carAccessories', 'CarAccessories'))
      .map(toAccessoryDto)
      .filter((accessory): accessory is CarAccessoryDto => accessory !== null);

    return { cars, carAccessories };
  } catch {
    return { cars: [], carAccessories: [] };
  }
}
