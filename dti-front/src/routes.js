import React from 'react'
import { Route, IndexRoute } from 'react-router-dom';

import App from './App'
import EditProduct from './pages/EditProduct'
import SearchProduct from './pages/SearchProduct';
import SingleProduct from './pages/SingleProduct';

export default (
    <Route path="/" component={App}>
        <IndexRoute component={App} />
        <Route path="/edit" component={EditProduct} />
        <Route path="/search" component={SearchProduct} />
        <Route path="product" component={SingleProduct} />
    </Route>
)