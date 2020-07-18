import React from 'react'
import { Route, IndexRoute } from 'react-router-dom';

import App from './App'
import EditProduct from './pages/EditProduct'

export default (
    <Route path="/" component={App}>
        <IndexRoute component={App} />
        <Route path="/edit" component={EditProduct} />
    </Route>
)