export type Subscription = {
  email: string;
  symbol: Symbol;
  timeRange: TimeRange;
  threshold: number | null;
  percentageThreshold: number | null;
  trend: Trend
};

export type Symbol = {
  symbol: string;
  baseAsset: string;
  quoteAsset: string;
};

export type TimeRange = {
  startDate: Date | null;
  endDate: Date | null;
};

export enum Trend
{
  Unspecified,
  Up,
  Down
}