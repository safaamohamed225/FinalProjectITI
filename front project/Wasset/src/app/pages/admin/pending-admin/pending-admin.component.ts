import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-pending-admin',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterModule,
    NgxPaginationModule
  ],
  templateUrl: './pending-admin.component.html',
  styleUrl: './pending-admin.component.css'
})
export class PendingAdminComponent {
  constructor(
    private _PlacesOwnerService: PlacesOwnerService,
    private router: Router
  ) {}
  p: number = 1;
  notApprovedAppartment: any;
  getAllPlaces() {
    console.log();
    this._PlacesOwnerService.getallPendingAdmin().subscribe({
      next: (data: any) => {
        this.notApprovedAppartment = data;
        console.log(data);
      },
      error: (err) => {
        this.router.navigate(['/error', { errormessage: err.error as string }]);
      },
    });
  }
  ngOnInit(): void {
    this.getAllPlaces();
  }
  approval(id: any) {
    this._PlacesOwnerService.approvalForPost(id).subscribe({
      next: () => {
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
          }, 500);
        });
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: err.error as string,
          width: '400px'
        }).then(() => {
          window.location.reload();
        });
      }
    });
  }
  delete(id:any){
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, reject it!",
      input: 'text',
      inputValidator: (value) => {
        if (!value) {
          return 'Reason for rejection is required!';
        }
        return null; // Return null if validation passes without any errors
      }
    }).then((result) => {
      if (result.isConfirmed) {
        const reason = result.value; // Retrieve the entered reason
        this._PlacesOwnerService.getreject(id, reason).subscribe(() => {
          Swal.fire({
            title: "Rejected!",
            text: "Your Place has been rejected.",
            icon: "success"
          }).then(() => {
            this.getAllPlaces();
          });
        });
      }
    });
  }
  
}
