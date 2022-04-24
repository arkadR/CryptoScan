import { Symbol } from "./Symbol";

export type Subscription = {
  email: string;
  symbol: Symbol;
  threshold: number;
};