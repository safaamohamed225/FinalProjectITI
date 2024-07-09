import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { jwtDecode } from 'jwt-decode';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-pending-student',
  standalone: true,
  imports: [RouterModule,CommonModule,NgxPaginationModule,HttpClientModule],
  providers:[PlacesOwnerService],

  templateUrl: './pending-student.component.html',
  styleUrl: './pending-student.component.css'
})
export class PendingStudentComponent {
  id:any;
  Places:any;
  userId:any;
  p:number=1;
  constructor(private _PlacesOwnerService:PlacesOwnerService , private router: Router,private Actived : ActivatedRoute)
  {
    this.getall()
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
      const data = await this._PlacesOwnerService.getAllPendingStd(this.userId).toPromise();
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
  
}


