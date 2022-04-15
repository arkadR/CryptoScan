import React, {useState} from 'react';
import { Symbol } from './Model/Symbol';
import { Subscription } from './Model/Subscription';
import {post, get, del} from './Http';
import './App.css';

function App() {

  const quoteAsset : string = "EUR";

  let testBinanceSymbols : Array<Symbol>= [
    { symbol: "ETHBTC", baseAsset: "ETH", quoteAsset: "BTC"},
    { symbol: "LTCBTC", baseAsset: "LTC", quoteAsset: "BTC"},
    { symbol: "BNBBTC", baseAsset: "BNB", quoteAsset: "BTC"},
  ]
  
  let [successText, setSuccessText] = useState("");
  let [symbols, setSymbols] = useState<Symbol[]>();
  let [symbol, setSymbol] = useState<Symbol>();

  let subscribe = async (subscription: Subscription) => {
    let response = await post("subscribe", JSON.stringify(subscription));
    let text = await response.text();
    setSuccessText(text);
  }

  // let getAvilableSymbols = async () => {
    // let response = await get("info/exchange/symbols/" + quoteAsset);
  //   let symbols = (await response.json()) as Symbol[];
  //   setSymbols(symbols);
  // }

  let getAvilableSymbols = () => {  
    setSymbols(testBinanceSymbols);
  }

  let selectionChanged = (event: React.ChangeEvent<HTMLSelectElement>) => {
    let selectedSymbol = symbols?.find(s => s.symbol === event.target.value)
    setSymbol(selectedSymbol);
  };

  // 1. after refresh only field to provide email
  // 2. load symbols (by quote asset (distinct))
  // 3. load subscriptions
  // 4. list all (at the top subscriptions, below symbols)
  // 4a. subscriptions have infos about, well, subscription (without email) + buttons (delete, patch)
  // 4b. symbols have symbols info (base asset may be sufficient) + threshold field + buttons (subscribe)
    
  return (
    <div className="App">
      <header className="App-header">
        {/* <button onClick={testPost}> Send message </button>
        <p> {successText} </p> */}
        <p>Get data:</p>
        <button onClick={getAvilableSymbols}> Get symbols </button>        
        <p>Selected symbol: {symbol?.symbol}</p>
        <select value={symbol?.symbol} onChange={selectionChanged}>
          {symbols?.map((symbol) => (
            <option value={symbol.symbol}>{symbol.symbol}</option>
          ))}
        </select>
      </header>
    </div>
  );
}

export default App;
