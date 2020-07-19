import React from 'react'
import { Switch, Route } from 'react-router-dom'

import App from '../App'
import EditProduct from '../pages/EditProduct'
import CreateProduct from '../pages/CreateProduct'
import SearchProduct from '../pages/SearchProduct'
import SingleProduct from '../pages/SingleProduct'

const Main = () => {
    return(
        <Switch>
            <Route exact path='/' component={App}></Route>
            <Route path='/edit/:id/:name/:price/:stock' component={EditProduct}></Route>
            <Route exact path='/create' component={CreateProduct}></Route>
            <Route exact path='/search' component={SearchProduct}></Route>
            <Route exact path='/product/:id' component={SingleProduct}></Route>
        </Switch>
    )
}

export default Main;