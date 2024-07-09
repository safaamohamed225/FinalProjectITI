import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { NgxPaginationModule } from 'ngx-pagination';
import { jwtDecode } from 'jwt-decode';


@Component({
  selector: 'app-places',
  standalone: true,
  imports: [ HttpClientModule, RouterModule,CommonModule,NgxPaginationModule],
  providers:[PlacesOwnerService],
  templateUrl: './places.component.html',
  styleUrl: './places.component.css'
})


export class PlacesComponent implements OnInit{
  Places:any;
  p:number=1;
  constructor(private _PlacesOwnerService:PlacesOwnerService , private router: Router){ }
  ngOnInit(): void {
    this.getToken();
    this.getAllPlaces();
  }
  clickAddPlaces(){
    this.router.navigate(['/addplaces']);
  }
  deleteplaces(id: any): void {
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
        this._PlacesOwnerService.deletePlaces(id).subscribe(() => {
          Swal.fire({
            title: "Deleted!",
            text: "Your Place has been deleted.",
            icon: "success"
          }).then(() => {
            this.getAllPlaces();
          });
        },
        (error) => {
          Swal.fire({
            title: "Error!",
            text: error.error,
            icon: "error"
          });
        });
      }
    });
  }
  
  
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
  getAllPlaces(){
    this._PlacesOwnerService.getallappByOneowner(this.OwnerID).subscribe({
      next:(data:any)=>{
        this.Places = data;

        console.log(this.Places)
        //.filter((c:any)=>c.isRented==false)
      },
      error:(err)=>{
        this.router.navigate(['/error',{errormessage : err.error as string}]);
      }
    })
  }
  // accept(id:any){
  //   console.log("This is id :" +id);

  //   this._PlacesOwnerService.getPlacesByID(id).subscribe({
  //     next:(res:any)=>{
  //       var objAppart={
  //         id:res.id,
  //         description :res.description,
  //         location:res.location,
  //         region:res.region,
  //         price :res.price,
  //         capacity:res.capacity,
  //         gender: res.gender,
  //         numofroom:res.numofroom,
  //         phone:res.phone,
  //         img:res.img,

  //         isApproved:res.isApproved,
  //         isRented:true,
  //         requestRent:res.requestRent
  //       }
  //       this._PlacesOwnerService.updatePlaces(id,objAppart).subscribe({
  //         next:(res:any)=>
  //         {
  //           this.getAllPlaces();
  //         }
  //       })
  //     }
  //   })

  //   this.getAllPlaces();
  // }
  
}



///////////////////////////////////////////////////////////////////////////
// import { HttpClientModule } from '@angular/common/http';
// import { Component, OnInit } from '@angular/core';
// import { Router, RouterModule } from '@angular/router';
// import { PlacesOwnerService } from '../../../services/places-owner.service';
// import { CommonModule } from '@angular/common';
// import Swal from 'sweetalert2';

// @Component({
//   selector: 'app-places',
//   standalone: true,
//   imports: [HttpClientModule, RouterModule, CommonModule],
//   providers: [PlacesOwnerService],
//   templateUrl: './places.component.html',
//   styleUrl: './places.component.css',
// })
// export class PlacesComponent implements OnInit {
//   Places: any;
//   constructor(
//     private _PlacesOwnerService: PlacesOwnerService,
//     private router: Router
//   ) {}
//   ngOnInit(): void {
//     this.getAllPlaces();
//     this.LoadApartmentToCancle();
//     // this._PlacesOwnerService.getAllPlaces().subscribe({
//     //   next:(data)=>{
//     //     this.Places = data;
//     //   },
//     //   error:(err)=>{
//     //     this.router.navigate(['/error',{errormessage : err.message as string}]);
//     //   }
//     // })
//   }
//   clickAddPlaces() {
//     this.router.navigate(['/addplaces']);
//   }

//   deleteplaces(id: any): void {
//     Swal.fire({
//       title: 'Are you sure?',
//       text: "You won't be able to revert this!",
//       icon: 'warning',
//       showCancelButton: true,
//       confirmButtonColor: '#3085d6',
//       cancelButtonColor: '#d33',
//       confirmButtonText: 'Yes, delete it!',
//     }).then((result) => {
//       if (result.isConfirmed) {
//         this._PlacesOwnerService.deletePlaces(id).subscribe(() => {
//           Swal.fire({
//             title: 'Deleted!',
//             text: 'Your Place has been deleted.',
//             icon: 'success',
//           }).then(() => {
//             this.accept(id);
//             //    this._PlacesOwnerService.getAllPlaces().subscribe((data) => {
//             //   this.Places = data;
//             // });
//             this.router.navigate(['/places']);
//           });
//         });
//       }
//     });
//   }
//   getAllPlaces() {
//     this._PlacesOwnerService.getAllPlaces().subscribe({
//       next: (data: any) => {
//         this.Places = data.filter((c: any) => c.isRented == false);
//       },
//       error: (err) => {
//         this.router.navigate([
//           '/error',
//           { errormessage: err.message as string },
//         ]);
//       },
//     });
//   }

//   AcceptedPlaces: any;
//   LoadApartmentToCancle() {
//     this._PlacesOwnerService.getAllPlaces().subscribe({
//       next: (data: any) => {
//         this.AcceptedPlaces = data.filter((c: any) => c.isRented == true);
//       },
//       error: (err) => {
//         this.router.navigate([
//           '/error',
//           { errormessage: err.message as string },
//         ]);
//       },
//     });
//   }
//   accept(id: any) {
//     console.log('This is id :' + id);

//     this._PlacesOwnerService.getPlacesByID(id).subscribe({
//       next: (res: any) => {
//         var objAppart = {
//           //id: res.id,
//           description: res.description,
//           location: res.location,
//           region: res.region,
//           price: res.price,
//           capacity: res.capacity,
//           gender: res.gender,
//           numofroom: res.numofroom,
//           phone: res.phone,
//           img: res.img,

//           isApproved: res.isApproved,
//           isRented: true,
//           requestRent: res.requestRent,
//         };
//         this._PlacesOwnerService.updatePlaces(id, objAppart).subscribe({
//           next: (res: any) => {
//             this.getAllPlaces();
//             this.LoadApartmentToCancle();
//           },
//         });
//       },
//     });

//     this.getAllPlaces();
//     this.LoadApartmentToCancle();
//   }

//   Cancle(id: any) {
//     this._PlacesOwnerService.getPlacesByID(id).subscribe({
//       next: (res: any) => {
//         var objAppart = {
//           //id: res.id,
//           description: res.description,
//           location: res.location,
//           region: res.region,
//           price: res.price,
//           capacity: res.capacity,
//           gender: res.gender,
//           numofroom: res.numofroom,
//           phone: res.phone,
//           isApproved: true,
//           img: res.img,
//           isRented: false,
//           requestRent: false,
//         };
//         this._PlacesOwnerService.updatePlaces(id, objAppart).subscribe({
//           next: (res: any) => {
//             this.getAllPlaces();
//             this.LoadApartmentToCancle();
//           },
//         });
//       },
//     });

//     this.getAllPlaces();
//   }
// }