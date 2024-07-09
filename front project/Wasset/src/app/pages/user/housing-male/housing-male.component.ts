import { Component , OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { CommonModule } from '@angular/common';
import { SearchPipe } from "../../../search.pipe";
import { NgxPaginationModule } from 'ngx-pagination';
@Component({
  selector: 'app-housing-male',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    SearchPipe,
    NgxPaginationModule
  ],
  templateUrl: './housing-male.component.html',
  styleUrl: './housing-male.component.css'
})
export class HousingMaleComponent {
  constructor(private ownerService:PlacesOwnerService , private activatedRoute :ActivatedRoute , private router:Router) {}
 
  ngOnInit(): void {
    this.getAllPlaces();

  }
  appartment:any;
  p:number=1
  getAllPlaces(){
    this.ownerService.getallMale().subscribe({
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
