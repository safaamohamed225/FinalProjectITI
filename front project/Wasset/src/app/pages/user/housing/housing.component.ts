import { Component , OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { CommonModule } from '@angular/common';
import { SearchPipe } from "../../../search.pipe";
import { NgxPaginationModule } from 'ngx-pagination';


@Component({
    selector: 'app-housing',
    standalone: true,
    templateUrl: './housing.component.html',
    styleUrl: './housing.component.css',
    imports: [
        RouterModule,
        CommonModule,
        SearchPipe,
        NgxPaginationModule
    ]
})

export class HousingComponent implements OnInit {
  constructor(private ownerService:PlacesOwnerService , private activatedRoute :ActivatedRoute , private router:Router) {}
 
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.getAllPlaces();
    //this.loadSearch();
    // this.filteration();
  }
  activeButton: string = ''; // Variable to track the active button
  isActive(route: string): boolean {
    return this.router.isActive(route, true);
  }
  appartment:any;
  p:number=1
  getAllPlaces(){
    this.ownerService.getallAppHousing().subscribe({
      next:(res:any)=>{
        this.appartment = res;
        console.log(res);

        // console.log(res);
        //this.appartment=res.filter((c:any)=>c.isApproved==true);
      }
    })

  }
// searchString:any;
//   loadSearch(){
//     this.activatedRoute.queryParamMap.subscribe({
//       next:(res:any)=>{
//         this.searchString=res.get("search");
//         // console.log(this.searchString);
//         if(this.searchString==="" || this.searchString===null || this.searchString===undefined){
//            this.appartment=this.appartment;
//           this.getAllPlaces();
//         }
//         else{
//            this.appartment=this.appartment.filter((c:any)=>c.gender.toLocaleLowerCase().includes(this.searchString.toLocaleLowerCase()));
//           this.ownerService.getAllPlaces().subscribe({
//             next:(res:any)=>{
//               this.appartment=res.filter((c:any)=>c.isApproved==true &&c.isRented==false && c.gender.toLocaleLowerCase()===this.searchString.toLocaleLowerCase())

//             }
//           })
//         }

//       }
//     })

//   }

  openDetails(id:any){
    this .router.navigate(["/details"], {queryParams:{id:id}})

  }
  // onSearch(value: any) {
  //   if (value === '' || value === null || value === undefined) {
  //     this.appartment=this.appartment;
  //     this.getAllPlaces();
  //   } else {
  //     this.appartment=this.appartment.filter((c:any)=>c.gender.toLocaleLowerCase().includes(this.searchString.toLocaleLowerCase()));
  //     this.ownerService.getAllPlaces().subscribe({
  //       next: (res: any) => {
  //         this.appartment = res.filter(
  //           (c: any) =>
  //             c.isApproved == true &&
  //             c.gender.toLocaleLowerCase() === value.toLocaleLowerCase()
  //         );
  //       },
  //     });
  //   }
  // }


}

