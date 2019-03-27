import React, { Component } from 'react';
import axios from 'axios';

export default class TruckModelEdit extends Component {
  constructor(props) {
    super(props);
    this.onChangeDescription = this.onChangeDescription.bind(this);
    this.onSubmit = this.onSubmit.bind(this);

    this.state = {
	  modelId: '',
      description: ''
    }
  }

  componentDidMount() {
      axios.get('http://localhost:61020/api/truckmodel/'+this.props.match.params.id)
          .then(response => {
              this.setState({ 
				modelId: response.data.modelId,
                description: response.data.description 
			});
          })
          .catch(function (error) {
              console.log(error);
          })
    }

  onChangeDescription(e) {
    this.setState({
      description: e.target.value
    });
  }

  onSubmit(e) {
    e.preventDefault();
    const obj = {
	  modelId: this.state.modelId,
      description: this.state.description
    };
    axios.put('http://localhost:61020/api/truckmodel/'+this.props.match.params.id, obj)
        .then(res => console.log(res.data));
    
    this.props.history.push('/model');
  }
 
  render() {
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
                    <input type="submit" 
                      value="Update" 
                      className="btn btn-success"/>
                </div>
            </form>
        </div>
    )
  }
}