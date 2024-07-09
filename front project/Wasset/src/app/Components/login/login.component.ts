import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import Swal from 'sweetalert2';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule,ReactiveFormsModule,CommonModule,HttpClientModule],
  providers:[AuthService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
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
    this._AuthService.loginFormToAPI(loginForm.value.email, loginForm.value.password).subscribe({
      next: (res) => {
        //if (res.message === 'success') {
          this.loginForm=res;
          localStorage.setItem('userToken', res.token);
          this.isLoading = false;
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Login successful!',
            showConfirmButton: false,
            timer: 1000,
            width: '400px'
          }).then(() => {
          let userType:any ;
            const token = localStorage.getItem('userToken');
            if (token) {
              const decodedToken: any = jwtDecode(token);
             userType =
                decodedToken[
                  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
                ];}
            
            console.log(userType)
            if (userType === 'student') {
              this._Router.navigate(['/home']);
              setTimeout(() => {
                window.location.reload();
              }, 50);
            } else if (userType === 'owner') {
              this._Router.navigate(['/places']);
              setTimeout(() => {
                window.location.reload();
              }, 50);
            }
            else if (userType === 'Admin') {
              this._Router.navigate(['/allpost']);
              setTimeout(() => {
                window.location.reload();
              }, 50);
            }else{
              this._Router.navigate(['/login']);

            }
            
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


