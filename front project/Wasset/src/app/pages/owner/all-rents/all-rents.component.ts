import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { ActivatedRoute, Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import Swal from 'sweetalert2';
import { jwtDecode } from 'jwt-decode';
@Component({
  selector: 'app-all-rents',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterModule,
    NgxPaginationModule
  ],
  templateUrl: './all-rents.component.html',
  styleUrl: './all-rents.component.css'
})
export class AllRentsComponent {
  constructor(private _PlacesOwnerService:PlacesOwnerService,private router: Router,private Actived:ActivatedRoute){
    //this.OwnerID = this.Actived.snapshot.params["id"];
    //this.getUserIdFromToken();
    //console.log(this.OwnerID)
    this.getAllPlaces();
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
    this.getAllPlaces();
    
  }
getAllPlaces(){
  this._PlacesOwnerService.getAllrentOwner(this.OwnerID).subscribe({
    next:(data:any)=>{
      this.notApprovedAppartment = data;
      console.log(this.getToken());
    },
    error:(err)=>{
      this.router.navigate(['/error',{errormessage : err.error as string}]);
    }
  })
}

deletee(obj2:any){
  console.log(obj2);

  Swal.fire({
    title: "Are you sure?",
    text: "You won't be able to revert this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, delete it!"
  }).then((result) => {
    if (result.isConfirmed) {
      this._PlacesOwnerService.delRentByOwner(obj2).subscribe(() => {
        Swal.fire({
          title: "Deleted!",
          text: "Rent has been deleted.",
          icon: "success" 
        }).then(() => {
          this.getAllPlaces();
        });
      });
    }
  });
}

approval(obj:any): void {
  console.log(this.notApprovedAppartment)
  this._PlacesOwnerService.approvalRentByOwner(obj.id,obj.apartmentId).subscribe({
    next: () => {
      console.log("Approval successful");
      console.log(obj.id);
      console.log(obj.apartmentId);
      this.showSuccessMessage();
    },
    error: (err) => {
      console.error("Error occurred while approving requests:", err);
      console.log(obj);
      console.log(obj.id);
      console.log(obj.apartmentId);
      this.showErrorMessage(err);
    }
  });
}

showSuccessMessage(): void {
  Swal.fire({
    position: 'center',
    icon: 'success',
    title: 'Approval successful!',
    showConfirmButton: false,
    timer: 2000,
    width: '400px'
  }).then(() => { 
    setTimeout(() => {
      window.location.reload();
    }, 50);
  });
}


showErrorMessage(err: any): void {
  Swal.fire({
    icon: 'error',
    title: 'Oops...',
    text: 'An error occurred while approving requests!',
    footer: `Error message: ${err.error}`
  });
}


}
