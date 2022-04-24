import {CryptocurrencySymbol } from "./Symbol";

export type Subscription = {
  userId: string;
  symbol: CryptocurrencySymbol;
  threshold: number;
};