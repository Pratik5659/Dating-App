import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  ngOnInit(): void {
    this.getUsers();
  }
  registerMode=false;
  http=inject(HttpClient);
  data:any={};

  registerToggle(){
    this.registerMode=!this.registerMode;
  }
  cancelRegisterMode(event:boolean){
    this.registerMode=event;
  }
  getUsers(){
    this.http.get('http://localhost:5104/api/Users').subscribe({
      next:response=>{
        this.data=response;
      },
      error:err=>{
        console.log(err);
      }
    });
  }

}
