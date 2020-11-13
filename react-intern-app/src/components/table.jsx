import React, { Component } from 'react';
import TableRow from './tableRow';
import TableHeader from './tableHeader';
class Table extends Component {
    render() { 
        const{dataArray} = this.props;
        return ( 
            <table className="table table-striped">
                <thead>
                    <TableHeader rowObject={this.getStudentFromApiArray(dataArray)}/>
                </thead>
                <tbody>
                    {dataArray
                    .map((arrayElement,index) => <TableRow key={index} rowObject={arrayElement} handleEdit={this.props.handleEdit} handleDelete={this.props.handleDelete}/> )}    
                </tbody>
            </table>
         );
    }
    getStudentFromApiArray = (dataArray) =>{
        if(dataArray !== undefined && dataArray.length !== 0) return dataArray[0].student;
    }
}
 
export default Table;