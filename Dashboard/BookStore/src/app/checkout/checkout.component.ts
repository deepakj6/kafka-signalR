import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from '../models/inventory.model';
import { SignalRService } from '../signalr.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  cartItems: Book[] = [];

  constructor(private route: ActivatedRoute, private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.getCartItems();
  }
  getCartItems(): void {
    const cartItems = localStorage.getItem('cartItems');
    if (cartItems) {
      this.cartItems = JSON.parse(cartItems);
    }
  }

  confirmAndPay1(): void {
    // Perform the confirm and pay logic here
    console.log('Confirm and pay initiated. Items in cart:', this.cartItems);
    // Reset the cart after confirming and paying
    this.cartItems = [];
  }

  confirmAndPay(): void {
    this.signalRService.confirmAndPay(this.cartItems);
    // Reset the cart after confirming and paying
    this.cartItems = [];
  }
}
