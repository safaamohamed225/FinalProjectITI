import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import jwt_decode from 'jwt-decode';
import { Observable,BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userData = new BehaviorSubject<any>(null);
  decodeUserData()
  {
    let encodedToken =JSON.stringify( localStorage.getItem('userToken'));
    let decodedToken:any = jwtDecode(encodedToken);
    this.userData.next(decodedToken);
  }


  constructor(private myClient:HttpClient, private _Router:Router) {

    // if(localStorage.getItem('userToken')!==null)
    // {
    //   this.decodeUserData();

    // }
   }
   ngOnInit(): void {
      if(localStorage.getItem('userToken')!==null)
    {
      this.decodeUserData();
    }
   }

 registerToAPI(userData:object):Observable<any>
 {
  return this.myClient.post("https://localhost:44301/api/Account/Register",userData);
 }
 
 loginFormToAPI(email: string, password: string): Observable<any> {

  const userData = { email, password };
  return this.myClient.post("https://localhost:44301/api/Account/Login", userData);
}
 logout()
 {
  localStorage.removeItem('userToken');
  this.userData.next(null);
  this._Router.navigate(['/login']);
 }
 
}
