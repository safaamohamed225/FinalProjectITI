import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { PlacesOwnerService } from './../../../services/places-owner.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-list-owner',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLink,NgxPaginationModule],
  templateUrl: './list-owner.component.html',
  styleUrl: './list-owner.component.css'
})
export class ListOwnerComponent {
  constructor(
    private PlacesOwnerService: PlacesOwnerService,
    private Router: Router
  ) {this.LoadStudents();}
  ngOnInit(): void {
    this.LoadStudents();
    
  }
  Owners: any;
  p:number=1;
  LoadStudents() {
    this.PlacesOwnerService.getAllOwner().subscribe({
      next: (res) => {
        console.log(res);
        this.Owners = res;
      },
    });
  }
  deleteOwn(id: any): void {
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
        this.PlacesOwnerService.deleteOwner(id).subscribe(() => {
          Swal.fire({
            title: "Deleted!",
            text: "Owner has been deleted.",
            icon: "success"
            
          }).then(() => { 
          setTimeout(() => {
            window.location.reload();
          }, 50);
          });
        });
      }
    });

  }
  navigate() {
    this.Router.navigate(['/allpost']);
  }
}
