import React, { Component } from 'react';

class TableHeader extends Component {
    render() { 
        const{rowObject} =this.props;
        return ( 
            <tr>
                {this.getTableHeader(rowObject)}
            </tr>
         );
    }

    getTableHeader = (rowObject) => {
        if(rowObject !== undefined)
        return Object.keys(rowObject).map(keyName => <th key={keyName}>{keyName.replace(/^\w/, (c) => c.toUpperCase())}</th>);
    }
}
 
export default TableHeader;