import React, { Component } from 'react';
import { Redirect } from 'react-router';
import axios from 'axios';

export default class TruckModelCreate extends Component {
  constructor(props) {
    super(props);
    this.onChangeDescription = this.onChangeDescription.bind(this);
    this.onSubmit = this.onSubmit.bind(this);

    this.state = {
      description: '',
      redirect: false
    }
  }
  onChangeDescription(e) {
    this.setState({
      description: e.target.value
    });
  }

  onSubmit(e) {
    e.preventDefault();
    const obj = {
      description: this.state.description,
    };
    axios.post('http://localhost:61020/api/truckmodel/', obj)
        .then(res => console.log(res.data));
    
    this.setState({
      description: '',
    })
	
	this.state.redirect = true;
  }
 
  render() {
    const { redirect } = this.state;
    if (redirect) {
      return <Redirect to="/model" />;
    }
    return (
        <div style={{ marginTop: 10 }}>
            <h4 align="center">Models</h4>
            <form onSubmit={this.onSubmit}>
                <div className="form-group">
                    <label>Description:  </label>
                    <input 
                      type="text" 
                      className="form-control" 
                      value={this.state.description}
                      onChange={this.onChangeDescription}
                      />
                </div>
                <div className="form-group">
                    <input type="submit" value="Save" className="btn btn-success"/>
                </div>
            </form>
        </div>
    )
  }
}