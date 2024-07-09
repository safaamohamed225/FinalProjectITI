import { CommonModule } from '@angular/common';
import { Component, Output } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { NgxPaginationModule } from 'ngx-pagination';
import { jwtDecode } from 'jwt-decode';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { PaymentService } from '../../../../../payment.service';
@Component({
  selector: 'app-approval-student',
  standalone: true,
  imports: [RouterModule,CommonModule,NgxPaginationModule],
  templateUrl: './approval-student.component.html',
  styleUrl: './approval-student.component.css'
})
export class ApprovalStudentComponent {
  id:any;
  Places:any;
  userId:any;
  p:number=1;
  
  getprice(price:any){
    this._PlacesOwnerService.price=price;
    this.router.navigate(["/vodafone"]);
    console.log(this._PlacesOwnerService.price);
  }
  
  constructor(private _PlacesOwnerService:PlacesOwnerService , private router: Router,private Actived : ActivatedRoute,private _CardRequest:PaymentService)
  {
    this.getall()
    //this.userId = this.Actived.snapshot.params["id"];
    //this.getUserIdFromToken();
   }
   getUserIdFromToken(){
    const token = localStorage.getItem('userToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.userId =
        decodedToken[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ];
      console.log(this.userId);
      //this.id.setValue(this.userId);
    }
    console.log(this.userId);
  }
   ngOnInit(): void {
    this.getUserIdFromToken()
    this.getall();
  }
  async getall(): Promise<void> {
    try {
      const data = await this._PlacesOwnerService.getAllApprovalStd(this.userId).toPromise();
      this.Places = data;
      console.log(data);
    } catch (err) {
      console.log(this.userId);
      console.log(this.Places);
      //this.router.navigate(['/error', { errormessage: err.error as string }]);
    }
  }


  deletee(id:any){
    console.log(id);
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
        this._PlacesOwnerService.delRentByOwner(id).subscribe(() => {
          Swal.fire({
            title: "Deleted!",
            text: "Rent has been deleted.",
            icon: "success" 
          }).then(() => {
            this.getall();
          });
        });
      }
    });
  }

  makeRwq(price:any){
    this._PlacesOwnerService.price=price;
    this._CardRequest.CardRequest();
  }
}
