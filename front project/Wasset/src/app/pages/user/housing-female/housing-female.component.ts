import { Component , OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { CommonModule } from '@angular/common';
import { SearchPipe } from "../../../search.pipe";
import { NgxPaginationModule } from 'ngx-pagination';
@Component({
  selector: 'app-housing-female',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    SearchPipe,
    NgxPaginationModule
  ],
  templateUrl: './housing-female.component.html',
  styleUrl: './housing-female.component.css'
})
export class HousingFemaleComponent {
  constructor(private ownerService:PlacesOwnerService , private activatedRoute :ActivatedRoute , private router:Router) {}
 
  ngOnInit(): void {
    this.getAllPlaces();

  }
  appartment:any;
  p:number=1
  getAllPlaces(){
    this.ownerService.getallFemale().subscribe({
      next:(res:any)=>{
        this.appartment = res;
        console.log(res);
      }
    })

  }

  openDetails(id:any){
    this .router.navigate(["/details"], {queryParams:{id:id}})

  }

  activeButton: string = ''; // Variable to track the active button
  isActive(route: string): boolean {
    return this.router.isActive(route, true);
  }
}
