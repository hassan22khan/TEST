import React, { Component } from 'react';
import {Link} from "react-router-dom";
class Header extends Component {
    getHeaderStyle = ()=> {
        return{
            textAlign : "center",
            padding :"15px",
            backgroundColor :"#333",
            color : "#fff"
        }
    }
    
    render() { 
        return (  
        <header style ={this.getHeaderStyle()}>
            <h1>Student Management System</h1>
            <Link style={linkStyle} to="/table">Home</Link> | <Link style={linkStyle} to="/about">About</Link> | <Link style={linkStyle} to="/login">Login</Link> | <Link style={linkStyle} to="/calculator">Calculator</Link>
        </header>);
    }
}
 
const linkStyle = {
    textDecoration : 'none',
    color:'#fff',
    cursor : 'pointer'
};
export default Header;