import { Component, SimpleChanges } from '@angular/core';
import { Router,RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';



@Component({
  selector: 'app-header',
  standalone: true,
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  imports: [RouterModule, HttpClientModule, CommonModule],
  providers: [AuthService],
})

export class HeaderComponent {
  isLogin: boolean = false;
  isStd: boolean = false;
  isadmin: boolean = false;

  userName :any;

  //userName: string = '';

  //this is to call logout in AuthService//
  logout() {
    this.isLogin = false;
    this._AuthService.logout();
  }

  ngOnInit(): void {
    this.getlogin();
  }
  constructor(
    private _AuthService: AuthService,
    private routerService: Router
  ) {
    //this.getlogin();
  }
  getlogin() {
    const token = localStorage.getItem('userToken');
    let userType:any ;
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.userName= decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
     userType =
        decodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ];}
        if (userType === 'student'){
            this.isStd=true;
        }
        if (userType === 'Admin'){
          this.isadmin=true;
      }
    if (token !== null) {
      this.isLogin = true;
      //window.location.reload();
      //this.userName = token;
    } else {

      this.isLogin = false;
    } //this.userName = ''
    console.log(token);
    //
  }

  onSearch(value: any) {
    this.routerService.navigate(['/hosing'], {
      queryParams: { search: value },
    });
  }
}




