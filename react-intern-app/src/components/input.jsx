import React, { Component } from "react";

class Input extends Component {
  render() {
    const { name, value, type, className, label, handleChange } = this.props;
    return (
      <div className="form-group">
        <label htmlFor={name}>{label}</label>
        <input
          onChange={handleChange}
          type={type}
          name={name}
          className={className}
          value={value}
          className="form-control"
        />
      </div>
    );
  }
}

export default Input;
