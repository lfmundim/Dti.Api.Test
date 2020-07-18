import React from 'react'
import { Switch, Route } from 'react-router-dom'

import App from '../App'
import EditProduct from '../pages/EditProduct'

const Main = () => {
    return(
        <Switch>
            <Route exact path='/' component={App}></Route>
            <Route path='/edit/:id/:name/:price/:stock' component={EditProduct}></Route>
        </Switch>
    )
}

export default Main;