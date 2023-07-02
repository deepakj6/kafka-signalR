import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Book } from './models/inventory.model';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;
  private bookAddedSubject: Subject<Book> = new Subject<Book>();
  private bookUpdatedSubject: Subject<Book> = new Subject<Book>();
  private bookDeletedSubject: Subject<Book> = new Subject<Book>();

  constructor() { }

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
    this.hubConnection.on('BookAdded', (book: Book) => {
      console.log('Received book added event:', book);
      this.bookAddedSubject.next(book);
    });

    this.hubConnection.on('BookUpdated', (book: Book) => {
      console.log('Received book updated event:', book);
      this.bookUpdatedSubject.next(book);
    });

    this.hubConnection.on('BookDeleted', (book: Book) => {
      console.log('Received book deleted event:', book);
      this.bookDeletedSubject.next(book);
    });
  }

  confirmAndPay(cartItems: Book[]): void {
    this.hubConnection.invoke('ConfirmAndPay', cartItems)
      .then(() => console.log('Confirm and pay initiated. Items in cart:', cartItems))
      .catch(err => console.error('Error while confirming and paying:', err));
  }

  getBookAdded(): Observable<Book> {
    return this.bookAddedSubject.asObservable();
  }

  getBookUpdated(): Observable<Book> {
    return this.bookUpdatedSubject.asObservable();
  }

  getBookDeleted(): Observable<Book> {
    return this.bookDeletedSubject.asObservable();
  }
}
