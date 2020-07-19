import React, { Component } from 'react';
import logo from '../logo.svg';
import Products from '../components/products'
import { BrowserRouter as Router, Route } from "react-router-dom";
import Button from 'react-bootstrap/Button';
import { Link } from 'react-router-dom'
import '../App.css';

class CreateProduct extends Component {
    state = {
        product: Products
    }

    handleSumbit(event) {
        let data = new FormData(event.target)
        updateEntry(data)
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
                            ID: <input type="text" name="id" />
                        </label>
                        <label>
                            Nome: <input type="text" name="name" />
                        </label>
                        <label>
                            Preço: <input type="text" name="price" />
                        </label>
                        <label>
                            Estoque: <input type="text" name="stock" />
                        </label>
                        <input type="submit" value="Submeter" />
                    </div>
                    <p class="edit-button">
                        <Link to={`/`}>
                            <Button variant="info">Voltar</Button>
                        </Link>
                    </p>
                </form>
            </div>
        )
    }
}

function updateEntry(data) {
    let requestBody = JSON.stringify({
        id: parseInt(data.get('id')),
        name: data.get('name'),
        price: parseFloat(data.get('price')),
        stock: parseInt(data.get('stock'))
    })
    console.log(requestBody)

    fetch('http://localhost:57406/api/DB', {
        method: 'POST',
        body: requestBody,
        headers: {
            "Content-Type": "application/json; charset=UTF-8"
        }
    })
        .catch(console.log)

    // window.location.reload(true)
};

export default CreateProduct