import React, { Component } from 'react';
import logo from '../logo.svg';
import Products from '../components/products'
import { BrowserRouter as Router, Route } from "react-router-dom";
import '../App.css';

class EditProduct extends Component {
    state = {
        product: Products
    }

    handleSumbit(event) {
        let data = new FormData(event.target)
        let id = event.target.action.split('/').slice(-1)[0].split('?')[0]
        updateEntry(id, data)
    }

    render() {
        return (
            <div>
                <div className="Header">
                    <header className="App-header">
                        <img src={logo} className="App-logo" alt="logo" />
                    </header>
                </div>
                <form onSubmit={this.handleSumbit}>
                    <div className="card" label>
                        <label>
                            Nome: <input type="text" placeholder={this.props.match.params.name} name="name" />
                        </label>
                        <label>
                            Preço: <input type="text" placeholder={this.props.match.params.price} name="price" />
                        </label>
                        <label>
                            Estoque: <input type="text" placeholder={this.props.match.params.stock} name="stock" />
                        </label>
                        <input type="submit" value="Submeter" />
                    </div>
                </form>
            </div>
        )
    }
}

function updateEntry(itemId, data) {
    let requestBody = JSON.stringify({
        id: parseInt(itemId),
        name: data.get('name'),
        price: parseFloat(data.get('price')),
        stock: parseInt(data.get('stock'))
    })
    console.log(requestBody)

    fetch('http://localhost:57406/api/DB', {
        method: 'PATCH',
        body: requestBody,
        headers: {
            "Content-Type": "application/json; charset=UTF-8"
        }
    })
    .catch(console.log)

    // window.location.reload(true)
};

export default EditProduct