import React, { Component } from 'react';
import axios from 'axios';

export default class TruckEdit extends Component {
  constructor(props) {
    super(props);
    this.onChangeChassis = this.onChangeChassis.bind(this);
	this.onChangeYear = this.onChangeYear.bind(this);
	this.onChangeModel = this.onChangeModel.bind(this);
    this.onSubmit = this.onSubmit.bind(this);

    this.state = {
	  truckId: '',		
      chassis: '',
	  year: '',
	  modelId: '',
	  models: []
    }
  }

  componentDidMount() {
	  axios.get("http://localhost:61020/api/truckmodel/")
		.then(response => {
			console.log(response);
			this.setState({ models: response.data });
		})
		.catch(error => console.log(error.response));
	  
      axios.get('http://localhost:61020/api/truck/'+this.props.match.params.id)
          .then(response => {
              this.setState({ 
				truckId: response.data.truckId,
                chassis: response.data.chassis, 
				year: response.data.year, 
				model: response.data.model
			});
          })
          .catch(function (error) {
              console.log(error);
          })
    }

  onChangeChassis(e) {
    this.setState({
      chassis: e.target.value
    });
  }
  onChangeYear(e) {
    this.setState({
      year: e.target.value
    });
  }
  onChangeModel(e) {
    this.setState({
      modelId: e.target.value
    });
  }

  onSubmit(e) {
    e.preventDefault();
	const obj = {
	  truckId: this.state.truckId,
      chassis: this.state.chassis,
	  year: this.state.year,
	  model: this.state.models.find(m => m.modelId == this.state.modelId)
    };
    axios.put('http://localhost:61020/api/truck/'+this.props.match.params.id, obj)
        .then(res => console.log(res.data));
    
    this.props.history.push('/truck');
  }
 
  render() {
	const models = this.state.models;

    return (
        <div style={{ marginTop: 10 }}>
            <h4 align="center">Trucks</h4>
            <form onSubmit={this.onSubmit}>
				<div className="form-group">
                    <label>Model:  </label>
					<select className="form-control" value={this.state.modelId} onChange={this.onChangeModel}>>
						{this.state.models.map((model) => <option key={model.modelId} value={model.modelId}>{model.description}</option>)}
					</select>
                </div>	
                <div className="form-group">
                    <label>Chassis:  </label>
                    <input 
                      type="text" 
                      className="form-control" 
                      value={this.state.chassis}
                      onChange={this.onChangeChassis}
                      />
					<label>Year:  </label>
                    <input 
                      type="text" 
                      className="form-control" 
                      value={this.state.year}
                      onChange={this.onChangeYear}
                      />
                </div>
                <div className="form-group">
                    <input type="submit" 
                      value="Update" 
                      className="btn btn-success"/>
                </div>
            </form>
        </div>
    )
  }
}