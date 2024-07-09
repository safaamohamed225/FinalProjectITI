import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-pending-owner',
  standalone: true,
  imports: [

    CommonModule,
    RouterLink,
    RouterModule,
    NgxPaginationModule
  ],
  templateUrl: './pending-owner.component.html',
  styleUrl: './pending-owner.component.css'
})
export class PendingOwnerComponent {
  constructor(private _PlacesOwnerService:PlacesOwnerService,private router: Router,private Actived:ActivatedRoute){
    this.getAllpendingowner();
  }
  p:number=1;
  notApprovedAppartment:any;
  OwnerID:any;
  getToken(){
    let token = localStorage.getItem('userToken');
     if (token) {
       const decodedToken: any = jwtDecode(token);
       this.OwnerID =
         decodedToken[
           'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
         ];
       }
       console.log(this.OwnerID);
    }
  
    ngOnInit(): void {
      this.getToken()
      this.getAllpendingowner();
      
    }
    getAllpendingowner(){
    this._PlacesOwnerService.getallPendingByOneowner(this.OwnerID).subscribe({
      next:(data:any)=>{
        this.notApprovedAppartment = data;
        console.log(this.getToken());
      },
      error:(err)=>{
        this.router.navigate(['/error',{errormessage : err.error as string}]);
      }
    })
  }
}
