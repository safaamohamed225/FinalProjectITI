import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { OwlOptions } from 'ngx-owl-carousel-o';


@Component({
  selector: 'app-details-admin',
  standalone: true,
  imports: [HttpClientModule,RouterModule,CommonModule,CarouselModule],
  providers:[PlacesOwnerService],
  templateUrl: './details-admin.component.html',
  styleUrl: './details-admin.component.css'
})
export class DetailsAdminComponent {
  id:any;
  Places:any;
  constructor(private _PlacesOwnerService:PlacesOwnerService , private router: Router,private Actived : ActivatedRoute)
  {
    this.id = this.Actived.snapshot.params["id"];

   }
   customOptions: OwlOptions = {
    loop: true,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: false,
    dots: false,
    autoplay: true,
    autoplayTimeout: 2000,
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: {
        items: 1
      }
    },
    nav: true
  }
   ngOnInit(): void {

    this._PlacesOwnerService.getPlacesByID(this.id).subscribe({
      next:(data)=>{
        this.Places = data;
        console.log(data);
      },
      error:(err)=>{
        this.router.navigate(['/error',{errormessage : err.message as string}]);
      }
    })
  }
}
