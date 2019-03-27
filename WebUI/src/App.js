// App.js

import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';

import ModelCreate from './components/truckmodel/truckmodel.create.component';
import ModelEdit from './components/truckmodel/truckmodel.edit.component';
import ModelList from './components/truckmodel/truckmodel.list.component';
import TruckCreate from './components/truck/truck.create.component';
import TruckEdit from './components/truck/truck.edit.component';
import TruckList from './components/truck/truck.list.component';

class App extends Component {
  render() {
    return (
      <Router>
        <div className="container">
          <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <Link to={'/'} className="navbar-brand">Volvo Trucks</Link>
            <div className="collapse navbar-collapse" id="navbarSupportedContent">
              <ul className="navbar-nav mr-auto">
              <li className="nav-item">
                  <Link to={'/'} className="nav-link">Home</Link>
                </li>
                <li className="nav-item">
                  <Link to={'/model'} className="nav-link">Models</Link>
                </li>
				<li className="nav-item">
                  <Link to={'/truck'} className="nav-link">Trucks</Link>
                </li>
              </ul>
            </div>
          </nav> <br/>
          <Switch>
              <Route exact path='/model/create' component={ ModelCreate } />
              <Route path='/model/edit/:id' component={ ModelEdit } />
              <Route path='/model' component={ ModelList } />
			  <Route exact path='/truck/create' component={ TruckCreate } />
              <Route path='/truck/edit/:id' component={ TruckEdit } />
              <Route path='/truck' component={ TruckList } />
          </Switch>
        </div>
      </Router>
    );
  }
}

export default App;