import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-login-admin',
  standalone: true,
  imports: [RouterModule,ReactiveFormsModule,CommonModule,HttpClientModule],
  providers:[AuthService],
  templateUrl: './login-admin.component.html',
  styleUrl: './login-admin.component.css'
})
export class LoginAdminComponent {
  constructor(private _AuthService:AuthService,private _FormBuilder:FormBuilder , private _Router:Router){} 
  isLoading:boolean=false;
  errMsg:string='';
  loginForm : FormGroup = this._FormBuilder.group
  ({
    email:[null , [Validators.required, Validators.email] ],
    password:[null , [Validators.required, Validators.pattern(/^\w{6,}/)] ],
    //type: [null, [Validators.required]]

  });
  handleloginForm(loginForm:FormGroup)
  {
    this.isLoading=true
    if(loginForm.valid)
    {
      /*
    this._AuthService.loginFormToAPI(loginForm.value).subscribe({
      next: (res) => {
        if (res.message === 'success') {
          localStorage.setItem('userToken', resposne.token);
          this._AuthService.decodeUserData();
          this.isLoading = false;
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Login successful!',
            showConfirmButton: false,
            timer: 1000,
            width: '400px'
          }).then(() => {
            this._Router.navigate(['/home']);
          });
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errMsg = err.error.message;
      }
    });
    */
      

    this._AuthService.loginFormToAPI(loginForm.value.email, loginForm.value.password).subscribe({

      next: (res) => {
        //if (res.message === 'success') {
          this.loginForm=res;
          localStorage.setItem('userToken', res.token);
          this.isLoading = false;
          // Determine the user type from the response
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Login successful!',
            showConfirmButton: false,
            timer: 1000,
            width: '400px'
          }).then(() => {
            
            this._Router.navigate(['/approval']);
          });
        //}
      },
      error: (err) => {
        this.isLoading = false;
        this.errMsg = err.error;
      }
    });
    }
    
  }
  
}


