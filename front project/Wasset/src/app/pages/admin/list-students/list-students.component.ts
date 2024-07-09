import { CommonModule } from '@angular/common';
import { PlacesOwnerService } from './../../../services/places-owner.service';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-list-students',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLink,NgxPaginationModule],
  templateUrl: './list-students.component.html',
  styleUrl: './list-students.component.css',
})
export class ListStudentsComponent implements OnInit {
  constructor(
    private PlacesOwnerService: PlacesOwnerService,
    private Router: Router
  ) {}
  ngOnInit(): void {
    this.LoadStudents();
  }
  Students: any;
  p:number=1;
  LoadStudents() {
    this.PlacesOwnerService.getAllStudents().subscribe({
      next: (res) => {
        console.log(res);
        this.Students = res;
      },
    });
  }
  deleteStd(id: any): void {
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
        this.PlacesOwnerService.deleteStudent(id).subscribe(() => {
          Swal.fire({
            title: "Deleted!",
            text: "Student has been deleted.",
            icon: "success"
            
          }).then(() => {
            this.LoadStudents();

          });
        });
      }
    });

  }
  navigate() {
    this.Router.navigate(['/allpost']);
  }
}
