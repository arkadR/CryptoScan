import {useState} from 'react';
import { Symbol, Subscription, TimeRange, Trend } from './Models';
import {post, get, del, patch} from './Http';
import './App.css';
import SubscriptionPanel from "./SubscriptionPanel";
import { 
  Box,
  TableRow,
  TableCell,
  TextField,
  Button,
  Divider
 } from '@material-ui/core';

 export default function App() {

  const quoteAsset : string = "EUR";
  
  let [symbols, setSymbols] = useState<Symbol[]>();
  let [subscriptions, setSubscriptions] = useState<Subscription[]>();
  let [isEmailSet, setIsEmailSet] = useState(false);
  let [email, setEmail] = useState("")

  let confirmEmail = () => {
    setIsEmailSet(true);
    getAvilableSymbols()
    getSubscriptions()
  }

  let getAvilableSymbols = async () => {
    let response = await get("info/exchange/symbols/" + quoteAsset);
    let symbols = (await response.json()) as Symbol[];
    setSymbols(symbols);
  }

  let getSubscriptions = async () => {
    let response = await get("info/subscriptions");
    let subscriptions = (await response.json()) as Subscription[];
    setSubscriptions(subscriptions);
  }

  let subscribe = async (newSubscription: Subscription) => {
    await post("subscriptions/subscribe", JSON.stringify(newSubscription));
    let newSubscriptions = [...subscriptions as Subscription[], newSubscription];
    setSubscriptions(newSubscriptions);
  }

  let update = async (newSubscription: Subscription) => {
    await patch("subscriptions/update", JSON.stringify(newSubscription));
    let newSubscriptions = subscriptions?.filter(sub => sub.symbol !== newSubscription.symbol);
    newSubscriptions?.push(newSubscription);
    setSubscriptions(newSubscriptions);    
  }

  let deleteSubscription = async (subscription: Subscription) => {
    await del("subscriptions/unsubscribe", JSON.stringify(subscription));
    let newSubscriptions = subscriptions?.filter(sub => sub.symbol !== subscription.symbol);
    setSubscriptions(newSubscriptions);
  }

  let emptyTimeRange = {startDate: null, endDate: null} as TimeRange;

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
                  symbolMode={false}
                  updateAction={update}
                  deleteAction={deleteSubscription}
                  subscribeAction={subscribe}/>
              ))}
              {symbols?.filter(sm => !subscriptions?.find(sub => sub.symbol.baseAsset === sm.baseAsset)).map(symbol => (
                <SubscriptionPanel 
                  subscription={{email: email, symbol: symbol, timeRange: emptyTimeRange, trend: Trend.Unspecified} as Subscription}
                  symbolMode={true}
                  updateAction={update}
                  deleteAction={deleteSubscription}
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
