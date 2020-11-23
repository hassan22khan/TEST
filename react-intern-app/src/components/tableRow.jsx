import React, { Component } from "react";
import { Link } from "react-router-dom";

class TableRow extends Component {
  render() {
    const { rowObject } = this.props;
    return (
      <tr key={rowObject.student.id}>
        {this.getTableRows(rowObject)}
        <td>
          <button
            className="btn btn-warning"
            onClick={() => this.props.handleEdit(rowObject)}
          >
            <Link style={{ textDecoration: "none", color: "#fff" }} to="/edit">
              Edit
            </Link>
          </button>
        </td>
        <td>
          <button
            className="btn btn-danger"
            onClick={() => this.props.handleDelete(rowObject.student.id)}
          >
            Delete
          </button>
        </td>
      </tr>
    );
  }

  getTableRows = (rowObject) => {
    if (rowObject !== undefined)
      return Object.keys(rowObject.student).map((keyName) => (
        <td key={rowObject.student[keyName] + Math.random()}>
          {rowObject.student[keyName]}
        </td>
      ));
  };
}

export default TableRow;
