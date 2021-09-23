import { HttpClient } from '@angular/common/http';
import { Component, VERSION } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';


@Component({
  selector: 'my-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  mainDisplay = '';
  topDisplay = '';
  numPad = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.'];
  symPad = ['/', '*', '+', '-'];

  equation : { numbers: number[], symbols: string[] } = { 
    numbers: [], 
    symbols: [] 
  };
  

  constructor(private httpClient: HttpClient){}

  public numButtonIsClicked(n: string) {
    if( n === '.'){
      if(this.checkIfCommaIsOnDisplay()) return;
    }

    if(!this.mainDisplay && this.topDisplay){
      this.equation.symbols.push(this.topDisplay.slice(-1));
    }

    this.addNumberToDisplay(n);
  }

  public addNumberToDisplay(n: string) {
    this.mainDisplay += n;
    
  }

  checkIfCommaIsOnDisplay() : boolean{
    return this.mainDisplay.includes('.');
  }

  public symButtonIsClicked(s: string) {
    // continue after getResult flow : symbol clicked just after getting the result
    if(this.topDisplay[this.topDisplay.length-1] === '='){
      let tempNum = this.mainDisplay;
      this.clear();
      this.mainDisplay = tempNum;
    }

    // stnadard flow : symbol after number and so on; symobl added to equation
    if(this.mainDisplay) {
      this.equation.numbers.push(Number(this.mainDisplay));
      this.topDisplay += this.mainDisplay; 
      this.mainDisplay = '';
    }

    // changed flow : change last symbol to new one 
    if (this.checkIfEndsWithSymbol(this.topDisplay)){
      this.topDisplay = this.topDisplay.substring(0, this.topDisplay.length - 1);
    }
    this.topDisplay += s;
  }

  public checkIfEndsWithSymbol(text : string) : boolean{
    return isNaN(Number(text[text.length-1]));
  }

  public clear() {
    this.mainDisplay = '';
    this.topDisplay = '';
    this.equation.numbers = [];
    this.equation.symbols = [];
  }

  public equalButtonIsClicked(){
    if(!this.mainDisplay && !this.topDisplay) return;
    
    this.equation.numbers.push(Number(this.mainDisplay));
    if(this.checkIfEndsWithSymbol(this.topDisplay) && !this.mainDisplay) {
      this.equation.symbols.push(this.topDisplay[this.topDisplay.length-1]);
    }

    // send equation object and get equationDTO back

    console.log(this.equation);
    console.log('------------');
    
    const urlLocal : string = "https://localhost:5001/api/Calculations/solve";
    const urlDocker : string = "http://localhost:52001/api/Calculations/solve";

    const headers = {'content-type': 'application/json'};
    this.httpClient.post(urlDocker, JSON.stringify(this.equation), {'headers': headers} )
    .subscribe( 
      (data : EquationDTO) => {
        console.log(data);
        this.mainDisplay = data.result.toString();
        this.topDisplay = data.equation + '=';
      },
      error => {
        console.log(error);
        this.mainDisplay = '!!!! ERROR !!!!';
        setTimeout( () => window.alert('What have you done?!'), 100);
        setTimeout( ()=> this.clear(), 2000);
      });
  }
}

interface EquationDTO {
  "equation" : string;
  "numbers" : number[];
  "result" : number;
  "symbols" : string[];
  "id" : number;
}

