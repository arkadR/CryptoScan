import React, {useState} from 'react';
import { Symbol } from './Model/Symbol';
import { Subscription } from './Model/Subscription';
import {post, get, del} from './Http';
import './App.css';
import SymbolPanel from "./SymbolPanel";
import { 
  Box,
  TableRow,
  TableCell,
  TextField,
  Button
 } from '@material-ui/core';

function App() {

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
  
  let [snackbarText, setSnackbarText] = useState("");
  let [isSnackbarOpen, setIsSnackbarOpen] = useState(false);
  let [symbols, setSymbols] = useState<Symbol[]>();
  let [subscriptions, setSubscriptions] = useState<Subscription[]>();
  let [isEmailSet, setIsEmailSet] = useState(false);
  let [email, setEmail] = useState("")

  let showSnackbar = (message: string) => {
    setSnackbarText(message);
    setIsSnackbarOpen(true);
  }

  let confirmEmail = () => {
    setIsEmailSet(true);
    getAvilableSymbols()
    // getSubscriptions()
  }

  let subscribe = async (symbol: Symbol, threshold: number) => {
    let subscription = {email: email, symbol: symbol, threshold: threshold} as Subscription;
    let response = await post("subscribe", JSON.stringify(subscription));
    let text = await response.text();
    showSnackbar(text);
  }

  // let getAvilableSymbols = async () => {
    // let response = await get("info/exchange/symbols/" + quoteAsset);
  //   let symbols = (await response.json()) as Symbol[];
  //   setSymbols(symbols);
  // }

  let getAvilableSymbols = () => {  
    setSymbols(testBinanceSymbols);
  }

  return (
    <div className="App">
      <header className="App-header">
        <img src={require('./logo.png')} alt="logo" className="App-logo" />
        <Box height="25px"/>
        <img src={require('./logo-name.png')} alt="logo" />
        {isEmailSet
          ? <>
              {/* {subscriptions?.map((subscription) => (
                <SubscriptionPanel subscription={subscription}/>
              ))} */}
              {symbols?.map((symbol) => (
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

export default App;
