import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.fetchInventory();
  }
/*  addToCart(book: Book): void {
    const cartItems = localStorage.getItem('cartItems') ? JSON.parse(localStorage.getItem('cartItems')!) : [];
    cartItems.push(book);
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
    book.addedToCart = true; // Set the addedToCart property of the book to true
  }*/


  addToCart(book: Book): void {
    const cartItems = localStorage.getItem('cartItems') ? JSON.parse(localStorage.getItem('cartItems')!) : [];
    const quantityInCart = cartItems.filter((item: { id: string; }) => item.id === book.id).length;
    const availableQuantity = book.quantity - quantityInCart;

    if (availableQuantity > 0) {
      cartItems.push({ ...book, addedToCart: true }); // Set the addedToCart property to true
      localStorage.setItem('cartItems', JSON.stringify(cartItems));
      book.addedToCart = true;
    }
  }

  getBookCollectionName(bookCollectionId: string): string {
    const bookCollection = this.inventory?.bookCollections.find(collection => collection.id === bookCollectionId);
    return bookCollection ? bookCollection.name : '';
  }



  checkout(): void {
    // Perform the checkout logic here
    console.log('Checkout initiated. Items in cart:', this.cart);
    // Reset the cart after checkout
    //this.cart = [];
    //Navigate to the checkout page
    const cartItemsString = JSON.stringify(this.cart);
    this.router.navigateByUrl('/checkout');
    
    //this.router.navigate(['/checkout'], { queryParams: { cart: cartItemsString } });
  }
/*
  checkout(): void {
    const cartItems = localStorage.getItem('cartItems') ? JSON.parse(localStorage.getItem('cartItems')!) : [];
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
  }*/



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
