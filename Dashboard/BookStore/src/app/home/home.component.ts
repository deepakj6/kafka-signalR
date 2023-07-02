import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Inventory, Book, BookCollection } from '../models/inventory.model';
import { SignalRService } from '../signalr.service';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  inventory: Inventory | undefined; // Update the type of inventory
  cart: Book[] = [];


  constructor(private http: HttpClient, private router: Router, private signalRService: SignalRService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.fetchInventory();
    this.loadCartItems();

    this.signalRService.startConnection();
    this.signalRService.getInventoryUpdates().subscribe((book: Book) => {
      // Handle the received inventory update
      console.log('Received inventory update:', book);
      // Update the UI or perform any necessary actions based on the update
      this.updateInventory(book);
      this.toastr.info(`Inventory updated for book: ${book.title}`);
      //this.cdr.detectChanges(); 
    });
  }


  updateInventory(book: Book): void {
    if (this.inventory) {
      const bookIndex = this.inventory.books.findIndex(b => b.id === book.id);
      if (bookIndex !== -1) {
        this.inventory.books[bookIndex] = { ...book }; // Replace the book with a new object containing updated properties
      }
    }
  }


  addToCart(book: Book): void {
    const quantityInCart = this.getQuantityInCart(book);
    const availableQuantity = book.quantity - quantityInCart;

    if (availableQuantity > 0) {
      const updatedCartItem = { ...book, quantity: quantityInCart + 1 };
      const existingCartItemIndex = this.cart.findIndex(item => item.id === book.id);

      if (existingCartItemIndex > -1) {
        this.cart[existingCartItemIndex] = updatedCartItem;
      } else {
        this.cart.push(updatedCartItem);
      }

      localStorage.setItem('cartItems', JSON.stringify(this.cart));
    }
  }



  getBookCollectionName(bookCollectionId: string): string {
    const bookCollection = this.inventory?.bookCollections.find(collection => collection.id === bookCollectionId);
    return bookCollection ? bookCollection.name : '';
  }

  getQuantityInCart(book: Book): number {
    const cartItem = this.cart.find(item => item.id === book.id);
    return cartItem ? cartItem.quantity : 0;
  }

  isAddToCartDisabled(book: Book): boolean {
    const quantityInCart = this.getQuantityInCart(book);
    const availableQuantity = book.quantity - quantityInCart;
    return availableQuantity <= 0;
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

  loadCartItems(): void {
    const cartItems = localStorage.getItem('cartItems');
    if (cartItems) {
      this.cart = JSON.parse(cartItems);
    }
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
