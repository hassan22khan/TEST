import React, { Component } from 'react';
import "../bootstrapCalculator.css";
class CalculatorBootstrap extends Component {
  state = { 
    inputBeforeCommand : "0",
    inputAfterCommand : "0",
    inputCheck : false,
    initialValue : 0,
    disabled : true,
    answer : 0
 }


 setStateButton = (buttonValue) => {
    const{inputBeforeCommand,inputAfterCommand,inputCheck} = this.state
    if(inputCheck == false)
    this.setState({inputBeforeCommand : inputBeforeCommand !== "0" ? inputBeforeCommand + buttonValue : buttonValue,disabled : false});
    else
    this.setState({inputAfterCommand : inputAfterCommand !== "0" ? inputAfterCommand + buttonValue : buttonValue});
}

 setInputAfterValue = (operationCommand) => {
    this.setState({inputCheck : true, operationCommand: operationCommand,disabled : true});
 }

 performOperation = () => {
     const{operationCommand,inputBeforeCommand,inputAfterCommand} = this.state;
     let inputValue;
     if(operationCommand === "Product") inputValue = inputBeforeCommand * inputAfterCommand;
     else if(operationCommand === "Sum") inputValue = parseInt(inputBeforeCommand) + parseInt(inputAfterCommand);
     else if(operationCommand === "Substraction") inputValue = parseInt(inputBeforeCommand) - parseInt(inputAfterCommand);
     else if(operationCommand === "Division") inputValue = parseInt(inputBeforeCommand) / parseInt(inputAfterCommand);
     this.setState({inputCheck : false, answer : inputValue,inputBeforeCommand : "0",inputAfterCommand : "0",disabled : false});
 }

 getIconPerOperation = () => {
     const{operationCommand} = this.state;
   if(operationCommand === "Product") return <span style={{color:'#fff',fontSize:"14px"}}>X</span>;
   else if(operationCommand === "Sum") return <span style={{color:'#fff',fontSize:"14px"}}>+</span>;
   else if(operationCommand === "Substraction") return <span style={{color:'#fff',fontSize:"14px"}}>-</span>;
   else if(operationCommand === "Division") return <span style={{color:'#fff',fontSize:"14px"}}>/</span>;
 }

 reset = () => {
    this.setState({inputBeforeCommand : "0", inputAfterCommand : "0", inputCheck : false, disabled : false, answer : 0, operationCommand : "none"});
 }

    render() { 
      const{inputBeforeCommand,inputAfterCommand,inputCheck,disabled,answer} = this.state
        return ( 
            <React.Fragment>
                <div class='container text-center'>
                 <div class='card'>
                  <div class='card-body'>
                    <input class='input' id='display' value={inputCheck === false ? inputBeforeCommand : inputAfterCommand}/>
                  </div>
      
      <div class='card-body'>
        <table class='table table-sm table-borderless'>
        <tbody><tr>
          <td><button class='btn btn-warning btn-lg'>C</button></td>
          <td><button class='btn btn-warning btn-lg' onClick={() => this.reset()}>CE</button></td>
          <td colspan='2'>
            <div class='btn-group'>
          <button class='btn btn-danger btn-lg' id='off'>off</button>
          <button class='btn btn-success btn-lg' id='on'>on</button>
            </div>
          </td>
        </tr><tr>
        <td><button class='btn btn-lg'  onClick={() => this.setStateButton("7")}>7</button></td>
        <td><button class='btn btn-lg'  onClick={() => this.setStateButton("8")}>8</button></td>
        <td><button class='btn btn-lg'  onClick={() => this.setStateButton("9")}>9</button></td>
        <td><button class='btn btn-warning btn-lg' disabled={disabled} onClick={() => this.setInputAfterValue("Division")}>/</button></td>
        </tr><tr>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("4")}>4</button></td>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("5")}>5</button></td>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("6")}>6</button></td>
        <td><button class='btn btn-warning btn-lg' disabled={disabled} onClick={() => this.setInputAfterValue("Product")}>*</button></td>
        </tr>
        <tr>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("1")}>1</button></td>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("2")}>2</button></td>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("3")}>3</button></td>
        <td><button class='btn btn-warning btn-lg' disabled={disabled} onClick={() => this.setInputAfterValue("Substraction")}>-</button></td>
        </tr><tr>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton(".")}>.</button></td>
        <td><button class='btn btn-lg' onClick={() => this.setStateButton("0")}>0</button></td>
        <td><button class='btn btn-primary btn-lg' onClick={() => this.performOperation("Equals")}>=</button></td>
        <td><button class='btn btn-warning btn-lg' disabled={disabled} onClick={() => this.setInputAfterValue("Sum")}>+</button></td>
        </tr></tbody></table>
      </div>
    </div>
        <div><h4 style={{padding:'20px',margin:"10px", border : "2px solid #28a745" ,borderRadius : '3px', opacity : answer !== 0 ? 1 : 0 , color: '#28a745', display:"inline-block"}}>{answer !== 0 ? answer : null}</h4>
</div>
    </div>
            </React.Fragment>
         );
    }
}
 
export default CalculatorBootstrap;