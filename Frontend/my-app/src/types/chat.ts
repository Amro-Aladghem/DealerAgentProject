import type { eChatRole } from '@/types/enums';

export interface ChatDto {
  chatSignature: string;
  storeId: string;
  name?: string | null;
  creationTime: string;
  imageUrl?: string | null;
  imageType?: string | null;
  chatRoleId: eChatRole;
  message: string;
  data?: string | null;
  agentType?: string | null;
  showMessage: boolean;
  isFinalResponse: boolean;
}

export interface AgentRequestDto {
  chatSignture?: string | null;
  storeId: string;
  message: string;
  imageUri?: string | null;
  agentType?: string | null;
}
