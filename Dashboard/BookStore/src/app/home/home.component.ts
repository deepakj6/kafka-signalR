import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Inventory, Book, BookCollection } from '../models/inventory.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  inventory: Inventory | undefined; // Update the type of inventory
  cart: Book[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchInventory();
  }
  addToCart(book: Book): void {
    this.cart.push(book);
    console.log('Book added to cart:', book);
  }

  checkout(): void {
    // Perform the checkout logic here
    console.log('Checkout initiated. Items in cart:', this.cart);
    // Reset the cart after checkout
    this.cart = [];
  }

  fetchInventory(): void {
    const inventoryUrl = 'https://localhost:5000/api/inventory';
    this.http.get<Inventory>(inventoryUrl).subscribe(
      (response: Inventory) => {
        this.inventory = response;
        console.log(this.inventory); // Log the inventory to the console for testing
      },
      (error) => {
        console.error(error);
      }
    );
  }
}
