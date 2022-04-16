import React, {useState} from 'react';
import { Symbol } from './Model/Symbol';
import { Subscription } from './Model/Subscription';
import {post, get, del} from './Http';
import './App.css';
import SymbolPanel from "./SymbolPanel";
import SubscriptionPanel from "./SubscriptionPanel";
import { 
  Box,
  TableRow,
  TableCell,
  TextField,
  Button
 } from '@material-ui/core';

 export default function App() {

  const quoteAsset : string = "EUR";

  let testBinanceSymbols : Array<Symbol>= [
    { symbol: "ETHBTC", baseAsset: "ETH", quoteAsset: "BTC"},
    { symbol: "LTCBTC", baseAsset: "LTC", quoteAsset: "BTC"},
    { symbol: "BNBBTC", baseAsset: "BNB", quoteAsset: "BTC"},
    { symbol: "ETHBTC", baseAsset: "ETH", quoteAsset: "BTC"},
    { symbol: "LTCBTC", baseAsset: "LTC", quoteAsset: "BTC"},
    { symbol: "BNBBTC", baseAsset: "BNB", quoteAsset: "BTC"},
    { symbol: "ETHBTC", baseAsset: "ETH", quoteAsset: "BTC"},
    { symbol: "LTCBTC", baseAsset: "LTC", quoteAsset: "BTC"},
    { symbol: "BNBBTC", baseAsset: "BNB", quoteAsset: "BTC"},
  ]

  let testSubscriptions : Array<Subscription>=[
    { email: "test.mail@test.com", symbol: { symbol: "BNBBTC", baseAsset: "BNB", quoteAsset: "BTC"} as Symbol, threshold: 3.5}
  ]
  
  let [symbols, setSymbols] = useState<Symbol[]>();
  let [subscriptions, setSubscriptions] = useState<Subscription[]>();
  let [isEmailSet, setIsEmailSet] = useState(false);
  let [email, setEmail] = useState("")

  let confirmEmail = () => {
    setIsEmailSet(true);
    getAvilableSymbols()
    getSubscriptions()
  }

  // let getAvilableSymbols = async () => {
    // let response = await get("info/exchange/symbols/" + quoteAsset);
  //   let symbols = (await response.json()) as Symbol[];
  //   setSymbols(symbols);
  // }

  let getAvilableSymbols = () => {  
    setSymbols(testBinanceSymbols);
  }

  // let getSubscriptions = async () => {
  //   let response = await get("info/subscriptions");
  //   let subscriptions = (await response.json()) as Subscription[];
  //   setSubscriptions(subscriptions);
  // }

  let getSubscriptions = () => {
    setSubscriptions(testSubscriptions.filter(sub => sub.email === email));
  }

  let subscribe = async (symbol: Symbol, threshold: number) => {
    let subscription = {email: email, symbol: symbol, threshold: threshold} as Subscription;
    // await post("subscribe", JSON.stringify(subscription));
    let newSubscriptions = [...subscriptions as Subscription[], subscription];
    setSubscriptions(newSubscriptions);
  }

  let update = async (subscription: Subscription, threshold: number) => {
    let newSubscription = {email: subscription.email, symbol: subscription.symbol, threshold: threshold} as Subscription;
    // await patch("subscribe", JSON.stringify(newSubscription));
    subscriptions?.push(newSubscription);
    removeSubscription(subscription);
  }

  let deleteSubscription = async (subscription: Subscription) => {
    // await del("unsubscribe", JSON.stringify(subscription));
    removeSubscription(subscription);
  }

  let removeSubscription = (subscription: Subscription) => {
    let newSubscriptions = subscriptions?.filter(sub => sub !== subscription);
    setSubscriptions(newSubscriptions);
  }

  return (
    <div className="App">
      <header className="App-header">
        <img src={require('./logo.png')} alt="logo" className="App-logo" />
        <Box height="25px"/>
        <img src={require('./logo-name.png')} alt="logo" />
        {isEmailSet
          ? <>
              {subscriptions?.map((subscription) => (
                <SubscriptionPanel 
                  subscription={subscription}
                  updateAction={update}
                  deleteAction={deleteSubscription}/>
              ))}
              {symbols?.filter(symbol => subscriptions?.find(sub => sub.symbol.symbol === symbol.symbol) === undefined).map((symbol) => (
                <SymbolPanel 
                  symbol={symbol}
                  subscribeAction={subscribe}/>
              ))}
            </>
          : <TableRow>
              <TableCell>Email: </TableCell>
              <TableCell>
                <TextField
                  id="email"
                  type="email"
                  variant="outlined"
                  onChange={(e) => setEmail(e.target.value)}
                />
                </TableCell>
                <TableCell>
                  <Button 
                    disabled={email.length <= 0} // email validation?
                    onClick={confirmEmail}>
                    Confirm email
                  </Button>
                </TableCell>
            </TableRow>  }
      </header>
    </div>
  );
}
