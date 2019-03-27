import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import TableRow from './TruckTableRow';

export default class TruckList extends Component {

  constructor(props) {
      super(props);
      this.state = {trucks: []};
    }
    componentDidMount(){
      axios.get('http://localhost:61020/api/truck/')
        .then(response => {
          this.setState({ trucks: response.data });
        })
        .catch(function (error) {
          console.log(error);
        })
    }
    tabRow(){
      return this.state.trucks.map(function(object, i){	
          return <TableRow obj={object} key={i} />;
      });
    }

    render() {
      return (
        <div>
          <h4 align="center">Trucks</h4>
		  <Link to={'/truck/create'} className="btn btn-success">New</Link>
          <table className="table table-striped" style={{ marginTop: 20 }}>
            <thead>
              <tr>
			    <th>Model</th>
                <th>Chassis</th>
				<th>Year</th>
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