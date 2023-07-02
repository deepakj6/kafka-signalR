import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Book } from './models/inventory.model';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;
  private inventoryUpdatesSubject: Subject<Book> = new Subject<Book>();

  constructor(private http: HttpClient) { }
  startConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:5000/inventoryhub') 
      .build();

    this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection started.');
        this.registerOnServerUpdates();
      })
      .catch(err => console.error('Error while starting SignalR connection:', err));
  }

  registerOnServerUpdates(): void {
    this.hubConnection.on('InventoryUpdated', (book: Book, action: string) => {
      console.log('Received inventory update:', book);
      // Emit the received book to the subscribers
      this.inventoryUpdatesSubject.next(book);
    });
  }

  getInventoryUpdates(): Observable<Book> {
    return this.inventoryUpdatesSubject.asObservable();
  }

  confirmAndPay(cartItems: Book[]): void {
    this.hubConnection.invoke('ConfirmAndPay', cartItems)
      .then(() => console.log('Confirm and pay initiated. Items in cart:', cartItems))
      .catch(err => console.error('Error while confirming and paying:', err));
  }

/*  confirmAndPay(cartItems: Book[]): void {
    // Make the necessary API request or perform any action you need
    this.http.post<any>('http://your-api-url/confirm-and-pay', cartItems)
      .subscribe(
        response => {
          console.log('Confirm and pay initiated. Items in cart:', cartItems);
          // Handle the response as needed
        },
        error => console.error('Error while confirming and paying:', error)
      );
  }*/
}
