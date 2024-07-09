import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLinkActive, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule,HttpClient  } from '@angular/common/http';
//import { provideHttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';

import Swal from 'sweetalert2';
@Component({
  selector: 'app-register',
  standalone: true,
  imports :[RouterModule,ReactiveFormsModule,CommonModule,HttpClientModule,RouterLinkActive
    // provideHttpClient().withFetch()
  ],
   
  providers:[AuthService],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent  
{
  constructor(private _AuthService:AuthService,private _FormBuilder:FormBuilder , private _Router:Router){} 
  // ngOnInit(): void {
  //   //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
  //   //Add 'implements OnInit' to the class.
  //   //this.register()
  // }
  isLoading:boolean=false;
  errMsg:string='';
  stringOnlyValidator(control: any): {[key: string]: any} | null {
    const value = control.value;
    if (value && (typeof value !== 'string' || /\d/.test(value))) {
      return { 'notString': true };
    }
    return null;
  }
  register : FormGroup = this._FormBuilder.group
  ({
    userName:[null , [Validators.required, this.stringOnlyValidator, Validators.minLength(3),Validators.maxLength(20)]],
    password:[null , [Validators.required, Validators.pattern(/^\w{6,}/)] ],
    confirmPassword:[null , [Validators.required, Validators.pattern(/^\w{6,}/)] ],
    email:[null , [Validators.required, Validators.email] ],
    phone:[null , [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)] ],
    role: [null, [Validators.required]],
    ssn: [null, [Validators.required, Validators.pattern(/^\d{14}$/)]], 
    address: [null, [Validators.required]],
  },{validators:this.rePasswordMatch});

  rePasswordMatch(register:any){
  {
   let passwordControl= register.get('password');
   let rePasswordControl= register.get('confirmPassword');
   if(passwordControl.value===rePasswordControl.value)
   {
    return null;
   }else
   {
    rePasswordControl.setErrors({passwordMatch:' password and rePassword not match '});
    return {passwordMatch:' password and rePassword not match '}
   }

  }

    
  }
// ngOnInit(): void {
//   this.handleRegister(this.register)
// }
  handleRegister(register:FormGroup)
  { this.isLoading=true
    if(register.valid)
    {
    this._AuthService.registerToAPI(register.value).subscribe({
      next: (res) => {
        this.register=res;
        console.log(res);
        this.isLoading = false;
        // if (res.error){
        //   this.errMsg=res.error
        //   //window.alert(res.error);
        // }
          //else
        //{ //if (res.message === 'success') {
          //this.register=res;
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Registration successful!',
            showConfirmButton: false,
            timer: 1000,
            width: '400px'
          }).then(() => {
            this._Router.navigate(['/login']);
            // const userType = register.value.role;
            // if (userType === 'student') {
            //   this._Router.navigate(['/loginstudent']);
            // } else if (userType === 'owner') {
            //   this._Router.navigate(['/login']);
            // }
          });
         
        //}
          
       // }
      },
      error: (err) => {
        this.isLoading = false;
        // if (err.error.message === 'This email has already been used') {
        //   this.errMsg = 'This email has already been used';
        // } else {
          this.errMsg = err.error;
          //window.alert(err.error);
          //console.log(err.error);
        //}
      }
    });
    
    }
  }

}



// handleRegister(register: FormGroup) {
//   if (register.valid) {
//     this.isLoading = true;
//     this._AuthService.registerToAPI(register.value).subscribe({
//       next: (res) => {
//         if (res) {
//           if (res.error) {
//             this.errMsg = res.error;
//           } else {
//             Swal.fire({
//               position: 'top-end',
//               icon: 'success',
//               title: 'Registration successful!',
//               showConfirmButton: false,
//               timer: 1000,
//               width: '400px'
//             }).then(() => {
//               const userType = register.value.role;
//               if (userType === 'student') {
//                 this._Router.navigate(['/loginstudent']);
//               } else if (userType === 'owner') {
//                 this._Router.navigate(['/login']);
//               }
//             });
//           }
//         } else {
//           this.errMsg = 'An error occurred during registration.';
//         }
//         this.isLoading = false;
//       },
//       error: (err) => {
//         this.isLoading = false;
//         this.errMsg = err.error.message || 'An error occurred during registration.';
//       }
//     });
//   }
// }