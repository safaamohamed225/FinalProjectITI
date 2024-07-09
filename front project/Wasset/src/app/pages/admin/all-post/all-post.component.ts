import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import Swal from 'sweetalert2';
import { PlacesOwnerService } from '../../../services/places-owner.service';
@Component({
  selector: 'app-all-post',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterModule, NgxPaginationModule],
  templateUrl: './all-post.component.html',
  styleUrl: './all-post.component.css',
})
export class AllPostComponent {
  constructor(
    private _PlacesOwnerService: PlacesOwnerService,
    private router: Router
  ) {}
  
  p: number = 1;
  notApprovedAppartment: any;
  getAllPlaces() {
    console.log();
    this._PlacesOwnerService.getAllPlaces().subscribe({
      next: (data: any) => {
        this.notApprovedAppartment = data;
        console.log(data);
      },
      error: (err) => {
        this.router.navigate(['/error', { errormessage: err.error as string }]);
      },
    });
  }

  
  delete(id: any) {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!',
    }).then((result) => {
      if (result.isConfirmed) {
        this._PlacesOwnerService.deletePlaces(id).subscribe(() => {
          Swal.fire({
            title: 'Deleted!',
            text: 'Your Place has been deleted.',
            icon: 'success',
          }).then(() => {
            this.getAllPlaces();
            //this.accept(id);
            //    this._PlacesOwnerService.getAllPlaces().subscribe((data) => {
            //   this.Places = data;
            // });
            // setTimeout(() => {
            //   window.location.reload();
            // }, 50);
            // this.getAllPlaces();
          });
        });
      }
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
          width: '400px',
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
          width: '400px',
        }).then(() => {
          window.location.reload();
        });
      },
    });
  }

  OpenSideBar(div: HTMLDivElement) {
    div.classList.toggle('MyClass');
  }
}
