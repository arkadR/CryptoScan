import {useState} from 'react';
import { CryptocurrencySymbol } from './Model/Symbol';
import { Subscription } from './Model/Subscription';
import {post, get, del, patch} from './Http';
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
  
  let [symbols, setSymbols] = useState<CryptocurrencySymbol[]>();
  let [subscriptions, setSubscriptions] = useState<Subscription[]>();
  let [isEmailSet, setIsEmailSet] = useState(false);
  let [email, setEmail] = useState("")

  let confirmEmail = () => {
    setIsEmailSet(true);
    getAvailableSymbols();
    getSubscriptions();
  }

  let getAvailableSymbols = async () => {
    let response = await get("info/exchange/symbols/" + quoteAsset);
    let symbols = (await response.json()) as CryptocurrencySymbol[];
    setSymbols(symbols);
  }

  let getSubscriptions = async () => {
    let response = await get("info/subscriptions");
    let subscriptions = (await response.json()) as Subscription[];
    setSubscriptions(subscriptions);
  }

  let subscribe = async (symbol: CryptocurrencySymbol, threshold: number) => {
    let subscription = { userId: email, symbol: symbol, threshold: threshold} as Subscription;
    await post("subscriptions/subscribe", JSON.stringify(subscription));
    let newSubscriptions = [...subscriptions as Subscription[], subscription];
    setSubscriptions(newSubscriptions);
  }

  let update = async (subscription: Subscription, threshold: number) => {
    let newSubscription = { userId: subscription.userId, symbol: subscription.symbol, threshold: threshold} as Subscription;
    await patch("subscriptions/update", JSON.stringify(newSubscription));
    subscriptions?.push(newSubscription);
    removeSubscription(subscription);
  }

  let deleteSubscription = async (subscription: Subscription) => {
    await del("subscriptions/unsubscribe", JSON.stringify(subscription));
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
              {subscriptions?.sort().map((subscription) => (
                <SubscriptionPanel 
                  subscription={subscription}
                  updateAction={update}
                  deleteAction={deleteSubscription}/>
              ))}
              {symbols?.filter(symbol => subscriptions?.find(sub => sub.symbol.symbol === symbol.symbol) === undefined).sort().map((symbol) => (
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
