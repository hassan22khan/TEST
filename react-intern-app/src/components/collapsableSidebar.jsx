import React, { Component } from "react";
import "../sidebar.css";
import $ from "jquery";
import { Link } from "react-router-dom";
class SideBar extends Component {
  state = {};

  toggleSidebar = () => {
    $("#sidebar").toggleClass("active");
  };

  render() {
    return (
      <div className="wrapper">
        <nav id="sidebar">
          <div className="sidebar-header">
            <h3>Dashboard</h3>
          </div>

          <ul className="list-unstyled components">
            <li>
              <Link to="/table">Students List</Link>
            </li>
            <li>
              <Link to="/add">Add Student</Link>
            </li>
            <li>
              <Link to="/calculator">Calculator</Link>
            </li>
          </ul>
        </nav>
        <div id="content">
          <nav className="navbar navbar-expand-lg navbar-light ">
            <div className="container-fluid">
              <button
                type="button"
                id="sidebarCollapse"
                className="btn"
                style={toggleStyle}
                onClick={() => this.toggleSidebar()}
              >
                <i className="fas fa-align-left"></i>
                <span>Toggle Sidebar</span>
              </button>
            </div>
          </nav>
        </div>
      </div>
    );
  }
}

let toggleStyle = {
  border: "2px solid #eee",
  backgroundColor: "rgba(0,0,0,0.6)",
  padding: "5px 10px",
  color: "#fff",
};

export default SideBar;
