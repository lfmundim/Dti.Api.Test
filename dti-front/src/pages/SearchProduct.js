import React, { Component } from 'react';
import logo from '../logo.svg';
import Products from '../components/products'
import { BrowserRouter as Router, Route } from "react-router-dom";
import '../App.css';
import Button from 'react-bootstrap/Button';
import { Link } from 'react-router-dom'
import { withRouter } from 'react-router-dom';

class SearchProduct extends Component {
    state = {
        product: Products
    }

    handleSumbit(event) {
        let data = new FormData(event.target)
        this.props.history.push(`/product/${parseInt(data.get('id'))}`);
    }

    render() {
        return (
            <div>
                <div className="Header">
                    <header className="App-header">
                        <img src={logo} className="App-logo" alt="logo" />
                    </header>
                </div>
                <form onSubmit={this.handleSumbit.bind(this)}>
                    <label>
                        ID: <input type="text" name="id" />
                    </label>
                    <input type="submit" value="Buscar" />
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

export default withRouter(SearchProduct)