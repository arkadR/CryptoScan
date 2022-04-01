import React, {useState} from 'react';
import logo from './logo.svg';
import './App.css';

function App() {
  
  let [successText, setSuccessText] = useState("");
  
  let onButtonClick = async () => {
    let response = await fetch("test", {
      method: "POST",
      headers: {
        'content-type': 'application/json;charset=UTF-8'
      }, 
      body: ""});
    let text = await response.text();
    setSuccessText(text);
  }
    
  return (
    <div className="App">
      <header className="App-header">
        <button onClick={onButtonClick}>
          Send message
        </button>
        <p>
          {successText}
        </p>
        
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
