import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import TableRow from './TruckModelTableRow';

export default class TruckModelList extends Component {

  constructor(props) {
      super(props);
      this.state = {models: []};
    }
    componentDidMount(){
      axios.get('http://localhost:61020/api/truckmodel/')
        .then(response => {
          this.setState({ models: response.data });
        })
        .catch(function (error) {
          console.log(error);
        })
    }
    tabRow(){
      return this.state.models.map(function(object, i){	
          return <TableRow obj={object} key={i} />;
      });
    }

    render() {
      return (
        <div>
          <h4 align="center">Models</h4>
		  <Link to={'/model/create'} className="btn btn-success">New</Link>
          <table className="table table-striped" style={{ marginTop: 20 }}>
            <thead>
              <tr>
                <th>Model</th>
                <th colSpan="2"></th>
              </tr>
            </thead>
            <tbody>
              { this.tabRow() }
            </tbody>
          </table>
        </div>
      );
    }
  }