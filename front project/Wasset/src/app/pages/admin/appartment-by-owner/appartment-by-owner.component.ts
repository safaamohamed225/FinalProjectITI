import { PlacesOwnerService } from './../../../services/places-owner.service';
import {
  ActivatedRoute,
  Router,
  RouterLink,
  RouterModule,
} from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';;

@Component({
  selector: 'app-appartment-by-owner',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterLink,NgxPaginationModule],
  templateUrl: './appartment-by-owner.component.html',
  styleUrl: './appartment-by-owner.component.css'
})
export class AppartmentByOwnerComponent {
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
        this.PlacesOwnerService.getallAppartmenByOwner(
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
}
