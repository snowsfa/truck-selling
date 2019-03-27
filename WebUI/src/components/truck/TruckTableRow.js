import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

class TruckTableRow extends Component {
	
	constructor(props) {
		super(props);
		this.delete = this.delete.bind(this);
	}
	delete() {
		axios.delete('http://localhost:61020/api/truck/'+this.props.obj.truckId)
			.then(console.log('Deleted'))
			.catch(err => console.log(err))
	}
	
  render() {
	return (
		<tr>
		  <td>
            {this.props.obj.model.description}
          </td>
          <td>
            {this.props.obj.chassis}
          </td>
		  <td>
            {this.props.obj.year}
          </td>
          <td>
            <Link to={"/truck/edit/"+this.props.obj.truckId} className="btn btn-primary">Edit</Link>
          </td>
          <td>
			<button onClick={this.delete} className="btn btn-danger">Delete</button>
          </td>
        </tr>
    );
  }
}

export default TruckTableRow;





