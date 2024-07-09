import { Component } from '@angular/core';
import { PaymentService } from '../../../../../payment.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PlacesOwnerService } from '../../../services/places-owner.service';
//import { app } from '../../../../../server';
//import { app } from '../../../../../server';

@Component({
  selector: 'app-vodafone',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './vodafone.component.html',
  styleUrl: './vodafone.component.css',
})
export class VodafoneComponent {
  constructor(private paymentService: PaymentService,private router: Router,private _place:PlacesOwnerService) {}
  walletNumber: any;
  amount: any;
  pin: any;
  otp: any;
  apartmentPrice:any;
  app:any;
ngOnInit(): void {
 this.apartmentPrice=this._place.price;
  // this.paymentService.getApartmentPrice(this.app).subscribe(
  //   (price: any) => {
  //     this.apartmentPrice = price;
  //   },
  //   (error) => {
  //     console.error('Error fetching apartment price:', error);
  //   }
  // );
}
  submitPaymentForm(): void {
   //console.log(this.app.apartmentId )
   //console.log(this.app.apartmentPrice )
    // Implement your form submission logic here
    // For example, you can call a service method to process the payment
    // console.log('Form submitted!');
    // console.log('Wallet Number:', this.walletNumber);
    // console.log('Amount:', this.amount);
    // console.log('Pin:', this.pin);
    // console.log('OTP:', this.otp);
  }
  VodafoneRequestt(): void {
    
    // console.log(this.apartmentPrice)
    // console.log(this.app)
   



    Swal.fire({
      position: 'center',
      title: 'successful',
      icon: 'success',
      showCancelButton: false,
      timer: 2000,
      width: '400px'
    }).then(() => {
        this.paymentService.VodafonerRequest(); 
        this.router.navigate(['/home']);
    });
  }
  
}
