export type OverlayType = 'search' | 'control';

export interface OverlayState {
    Apparent?: boolean;
    ActiveOverlayType?: OverlayType;
    ActivePath?: string;
  }
  