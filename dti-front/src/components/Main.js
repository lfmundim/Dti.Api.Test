import React from 'react'
import { Switch, Route } from 'react-router-dom'

import App from '../App'
import EditProduct from '../pages/EditProduct'
import CreateProduct from '../pages/CreateProduct'

const Main = () => {
    return(
        <Switch>
            <Route exact path='/' component={App}></Route>
            <Route path='/edit/:id/:name/:price/:stock' component={EditProduct}></Route>
            <Route exact path='/create' component={CreateProduct}></Route>
        </Switch>
    )
}

export default Main;