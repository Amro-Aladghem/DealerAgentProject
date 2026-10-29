import type { LucideIcon } from 'lucide-react';
import {
  AlertTriangle,
  Car,
  CheckCircle2,
  Clock,
  Lightbulb,
  Loader2,
  MessageCircle,
  MessageSquareWarning,
  Search,
} from 'lucide-react';
import type { AgentType, NotificationStatus } from '@/types/enums';

interface AgentPresentation {
  Icon: LucideIcon;
  tint: string;
  label: string;
}

export const agentPresentation: Record<AgentType, AgentPresentation> = {
  DealerAgent: {
    Icon: Car,
    tint: 'bg-primary-soft text-primary-dark',
    label: 'Dealer Agent',
  },
  AdvisorAgent: {
    Icon: Lightbulb,
    tint: 'bg-sky-50 text-sky-700',
    label: 'Advisor',
  },
  ProductsSearchAgent: {
    Icon: Search,
    tint: 'bg-amber-50 text-amber-700',
    label: 'Products Search',
  },
  ComplaintAgent: {
    Icon: MessageSquareWarning,
    tint: 'bg-danger-soft text-danger',
    label: 'Complaints',
  },
};

interface StatusPresentation {
  Icon: LucideIcon;
  tint: string;
  spin?: boolean;
}

export const statusPresentation: Record<NotificationStatus, StatusPresentation> = {
  Queued: { Icon: Clock, tint: 'text-muted' },
  Running: { Icon: Loader2, tint: 'text-primary', spin: true },
  WaitingForUser: { Icon: MessageCircle, tint: 'text-amber-600' },
  Completed: { Icon: CheckCircle2, tint: 'text-success' },
  Failed: { Icon: AlertTriangle, tint: 'text-danger' },
};

export const statusLabel: Record<NotificationStatus, string> = {
  Queued: 'Queued',
  Running: 'Running',
  WaitingForUser: 'Waiting for you',
  Completed: 'Completed',
  Failed: 'Failed',
};
