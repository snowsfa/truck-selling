import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

class TruckModelTableRow extends Component {
	
	constructor(props) {
		super(props);
		this.delete = this.delete.bind(this);
	}
	delete() {
		axios.delete('http://localhost:61020/api/truckmodel/'+this.props.obj.modelId)
			.then(console.log('Deleted'))
			.catch(err => console.log(err))
	}
	
  render() {
	return (
		<tr>
          <td>
            {this.props.obj.description}
          </td>
          <td>
            <Link to={"/model/edit/"+this.props.obj.modelId} className="btn btn-primary">Edit</Link>
          </td>
          <td>
			<button onClick={this.delete} className="btn btn-danger">Delete</button>
          </td>
        </tr>
    );
  }
}

export default TruckModelTableRow;





