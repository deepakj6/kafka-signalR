import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from '../models/inventory.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  cartItems: Book[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getCartItems();
  }
  getCartItems(): void {
    const cartItems = localStorage.getItem('cartItems');
    if (cartItems) {
      this.cartItems = JSON.parse(cartItems);
    }
  }

  confirmAndPay(): void {
    // Perform the confirm and pay logic here
    console.log('Confirm and pay initiated. Items in cart:', this.cartItems);
    // Reset the cart after confirming and paying
    this.cartItems = [];
  }
}
