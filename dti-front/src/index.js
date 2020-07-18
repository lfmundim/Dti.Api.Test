import React from 'react';
import ReactDOM from 'react-dom';
import './App.css';
import { HashRouter, Route, BrowserRouter } from "react-router-dom";
import * as serviceWorker from './serviceWorker';
import App from './App'
import Main from './components/Main'

ReactDOM.render((
  <BrowserRouter>
    <Main />
  </BrowserRouter>
  ), document.getElementById('root')
);

serviceWorker.unregister();