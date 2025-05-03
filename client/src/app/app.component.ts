import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  
  title = 'client';

  http=inject(HttpClient);
  data:any;


  ngOnInit(): void {
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
