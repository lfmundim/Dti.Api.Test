import React from 'react'
import '../App.css';
import Button from 'react-bootstrap/Button';
import { Link } from 'react-router-dom'

const Product = ({ product }) => {
    return (
        <div>
            <center><h1>Produtos</h1></center>
            <p class="edit-button">
                <Link to={`/`}>
                    <Button variant="info">Voltar</Button>
                </Link>
            </p>
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">{product.name}</h5>
                    <h6 class="card-subtitle mb-2 text-muted">R${product.price.toFixed(2)}</h6>
                    <p class="card-text">Estoque: {product.stock}</p>
                    <p class="delete-button">
                        <Button variant="danger" onClick={_ => deleteEntry(product.id)}>Excluir</Button>
                    </p>
                    <p class="edit-button">
                        <Link to={`/edit/${product.id}/${product.name}/${product.price}/${product.stock}`}>
                            <Button variant="info">Editar</Button>
                        </Link>
                    </p>
                </div>
            </div>
            )}
        </div>
    )
};

function deleteEntry(id) {
    console.log('click ', id)
    const requestOptions = {
        method: 'DELETE'
    }

    fetch('http://localhost:57406/api/DB?id=' + id, requestOptions)
        .catch(console.log)

    window.location.reload(true)
};

export default Product