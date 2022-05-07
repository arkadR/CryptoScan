import {useState} from 'react';
import { Symbol, Subscription, TimeRange, Trend } from './Models';
import {post, get, del, patch} from './Http';
import './App.css';
import SubscriptionPanel from "./SubscriptionPanel";
import { GoogleLogin, GoogleLogout, GoogleLoginResponse} from 'react-google-login';

const clientId = process.env.REACT_APP_GOOGLE_CLIENT_ID as string;
const quoteAsset : string = "EUR";

 export default function App() {
  
  let [symbols, setSymbols] = useState<Symbol[]>();
  let [subscriptions, setSubscriptions] = useState<Subscription[]>();
  let [userId, setUserId] = useState<string | null>(null);

  let handleLoginSuccess = (userId: string) => {
    setUserId(userId)
    getAvailableSymbols()
    getSubscriptions()
  }

  let getAvailableSymbols = async () => {
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
        <img src={require('./logo-name.png')} alt="logo" />
        {userId !== null
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
                  subscription={{userId: userId, symbol: symbol, timeRange: emptyTimeRange, trend: Trend.Unspecified} as Subscription}
                  symbolMode={true}
                  updateAction={update}
                  deleteAction={deleteSubscription}
                  subscribeAction={subscribe}/>
              ))}
              {<GoogleLogout
                clientId={clientId}
                buttonText={"Logout"}
                onLogoutSuccess={()=>setUserId(null)}
              />}
            </>
          : <GoogleLogin
              clientId={clientId}
              buttonText="Sign In with Google"
              onSuccess={response => handleLoginSuccess((response as GoogleLoginResponse).profileObj.googleId)}
              onFailure={response => console.log(response)}
              isSignedIn={true}
            />}
      </header>
    </div>
  );
}
