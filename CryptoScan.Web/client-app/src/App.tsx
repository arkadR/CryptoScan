import {useEffect, useState} from 'react';
import { Symbol, Subscription, TimeRange, Trend } from './Models';
import {post, get, del, patch} from './Http';
import './App.css';
import SubscriptionPanel from "./SubscriptionPanel";
import { GoogleOAuthProvider, GoogleLogin, googleLogout } from '@react-oauth/google';
import { Box, Button } from '@material-ui/core';

const clientId = "28315789494-bmoihsf48vkjshc45dhejeg0ipimnndv.apps.googleusercontent.com";
const quoteAsset : string = "EUR";

export default function App() {
  
  let [symbols, setSymbols] = useState<Symbol[]>();
  let [subscriptions, setSubscriptions] = useState<Subscription[]>();
  let [userId, setUserId] = useState<string | undefined>(undefined);
  let [email, setEmail] = useState<string | undefined>(undefined);

  let handleLoginSuccess = async (jwt: string | undefined) => {
    let parsed = parseJwt(jwt!);
    setUserId(parsed.sub);
    setEmail(parsed.email);
  }
  
  useEffect(() => {
    if (userId) {
      getAvailableSymbols().catch(console.error);
      getSubscriptions().catch(console.error);
    }
  }, [userId])

  let parseJwt = (token: string) => {
    let base64Url = token.split('.')[1];
    let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    let jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  };

  let handleLogout = () => {
    googleLogout();
    setUserId(undefined);
  }

  let getAvailableSymbols = async () => {
    let response = await get("info/exchange/symbols/" + quoteAsset);
    let symbols = (await response.json()) as Symbol[];
    setSymbols(symbols);
  }

  let getSubscriptions = async () => {
    let response = await get(`info/subscriptions?userId=${userId}`);
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
      <GoogleOAuthProvider clientId={clientId}>
        <header className="App-header">
          <img src={require('./logo.png')} alt="logo" className="App-logo" />
          <Box height="25px"/>
          <img src={require('./logo-name.png')} alt="logo" />
          {userId 
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
                    subscription={{userId: userId, email: email, symbol: symbol, timeRange: emptyTimeRange, trend: Trend.Unspecified} as Subscription}
                    symbolMode={true}
                    updateAction={update}
                    deleteAction={deleteSubscription}
                    subscribeAction={subscribe}/>
                ))}
                {<Button 
                  onClick={handleLogout} 
                  variant="contained"
                  color="secondary">
                    Logout
                </Button>}
              </>
            : <GoogleLogin
                onSuccess={response => handleLoginSuccess(response.credential)} 
                onError={() => console.log('Login Failed')}
                auto_select
                useOneTap
              />  }
        </header>
      </GoogleOAuthProvider>
    </div>
  );
}
