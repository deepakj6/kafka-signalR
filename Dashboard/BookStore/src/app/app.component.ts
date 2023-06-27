import { Component } from '@angular/core';
import { HomeComponent } from './home/home.component';

@Component({
  selector: 'app-root',
  template: `
    
   <app-home></app-home> <!-- Add the HomeComponent as a child component -->
  `,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BookStore';
}


/*@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BookStore';
}*/
