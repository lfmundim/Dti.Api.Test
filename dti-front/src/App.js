import React, { Component } from 'react';
import Products from './components/products'
import logo from './logo.svg';
import './App.css';

class App extends Component {
  state = {
    products: []
  }

  componentDidMount() {
    fetch('http://localhost:57406/api/DB')
      .then(res => res.json())
      .then((data) => {
        this.setState({ products: data })
      })
      .catch(console.log)
  }

  render() {
    return (
      <div>
        <div className="Header">
          <header className="App-header">
            <img src={logo} className="App-logo" alt="logo" />
          </header>
        </div>
        <div className="Product-Cards">
          <Products products={this.state.products} />
        </div>
      </div>
    );
  }
}

export default App;