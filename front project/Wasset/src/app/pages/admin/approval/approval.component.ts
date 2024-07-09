import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-approval',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterModule, NgxPaginationModule],
  templateUrl: './approval.component.html',
  styleUrl: './approval.component.css',
})
export class ApprovalComponent implements OnInit {
  constructor(
    private _PlacesOwnerService: PlacesOwnerService,
    private router: Router
  ) {}
  // sidebarOpen = false;

  // toggleSidebar() {
  //   this.sidebarOpen = !this.sidebarOpen;
  // }
  p: number = 1;
  notApprovedAppartment: any;
  getAllPlaces() {
    console.log();
    this._PlacesOwnerService.getallApprovalAdmin().subscribe({
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
}
