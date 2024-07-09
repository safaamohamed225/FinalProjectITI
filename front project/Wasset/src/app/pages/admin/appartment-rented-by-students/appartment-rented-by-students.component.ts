import { PlacesOwnerService } from './../../../services/places-owner.service';
import {
  ActivatedRoute,
  Route,
  Router,
  RouterLink,
  RouterModule,
} from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-appartment-rented-by-students',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLink,NgxPaginationModule],
  templateUrl: './appartment-rented-by-students.component.html',
  styleUrl: './appartment-rented-by-students.component.css',
})
export class AppartmentRentedByStudentsComponent implements OnInit {
  constructor(
    private ActivatedRoute: ActivatedRoute,
    private Router: Router,
    private PlacesOwnerService: PlacesOwnerService
  ) {}
  ngOnInit(): void {
    this.LoadstudnetId();
  }
  places: any;
  StudentId: any;
  p:number=1;

  LoadstudnetId() {
    this.ActivatedRoute.paramMap.subscribe({
      next: (res) => {
        this.PlacesOwnerService.getallAppartmentRentedByStudent(
          res.get('id')
        ).subscribe({
          next: (res) => {
            this.places = res;
            console.log(res);
          },
        });
      },
    });
  }

  // GoToAppartment(appId: any) {
  //   this.Router.navigate(['/details'], { queryParams: { id: appId } });
  // }
}
