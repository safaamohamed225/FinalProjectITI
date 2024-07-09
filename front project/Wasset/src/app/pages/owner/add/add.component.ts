import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { PlacesOwnerService } from '../../../services/places-owner.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { jwtDecode } from 'jwt-decode';
//import { NgxPaginationModule } from 'ngx-pagination';


@Component({
  selector: 'app-add',
  standalone: true,
  imports: [    
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    CommonModule,
    RouterModule
    
  ],
    providers:[PlacesOwnerService],

  templateUrl: './add.component.html',
  styleUrl: './add.component.css'
})


export class AddComponent {
  Places:any;
  //p:number=1;
  //contain = true;
  userId:any;
  AddForm = new FormGroup({
    //id: new FormControl("", [Validators.required]),
    Description: new FormControl("", [Validators.required, Validators.maxLength(1000)]),
    Region: new FormControl("", [Validators.required, Validators.maxLength(100)]),
    NumofRoom: new FormControl("", [Validators.required, Validators.min(1), this.numberOnlyValidator]),
    ApartmentPrice: new FormControl("", [Validators.required, Validators.min(1), this.numberOnlyValidator]),
    GenderOfStudents: new FormControl("", [Validators.required]),
    Location: new FormControl("", [Validators.required, Validators.maxLength(100)]),
    Capacity: new FormControl("", [Validators.required, Validators.min(1), this.numberOnlyValidator]),
    //phone:new FormControl("", [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)] ),
    OwnerID: new FormControl("", [Validators.required]),

    images: new FormControl(null, [Validators.required])
   // images: new FormControl("", [Validators.required, this.imageRequiredValidator])


  });
  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      const files = event.target.files;
      this.AddForm.get('images')?.setValue(files);
    }
  }
 v:any;
getUserIdFromToken(): void {
  const token = localStorage.getItem('userToken');
  if (token) {
    const decodedToken: any = jwtDecode(token);
    this.userId =
      decodedToken[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ];
    //console.log(this.userId);
    //console.log(decodedToken);
    this.AddForm.get('OwnerID')?.setValue(this.userId);
    this.v=    decodedToken[
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
    ]
  }
}
  constructor(private _PlacesOwnerService:PlacesOwnerService , private router: Router){
    //this.getUserIdFromToken();
   }
  ngOnInit(): void {
    this.getUserIdFromToken();

  }
  numberOnlyValidator(control: FormControl): { [key: string]: any } | null {
    const value = control.value;
    if (isNaN(value)) {
      return { 'notNumber': true };
    }
    return null;
  }

  AddPlaces() {
    console.log("scvdcsad");
    if (this.AddForm.valid) 
    {
      const formData = new FormData();
      formData.append('Description', this.AddForm.controls.Description.value ?? '');
      formData.append('Region', this.AddForm.controls.Region.value ?? '');
      formData.append('NumofRoom', this.AddForm.controls.NumofRoom.value ?? '');
      formData.append('ApartmentPrice', this.AddForm.controls.ApartmentPrice.value ?? '');
      formData.append('GenderOfStudents', this.AddForm.controls.GenderOfStudents.value ?? '');
      formData.append('Location', this.AddForm.controls.Location.value ?? '');
      formData.append('Capacity', this.AddForm.controls.Capacity.value ?? '');
      formData.append('OwnerID', this.AddForm.controls.OwnerID.value ?? '');
      
      const images = this.AddForm.controls.images.value as unknown as any[];
      if (images !== null) { 
        for (let i = 0; i < images.length; i++) {
          formData.append('images', images[i]);
        }
      }
      console.log(formData);
      console.log(images)
      this._PlacesOwnerService.AddNewPlaces(formData).subscribe(() => {
        //this.router.navigate(['/places']);
        Swal.fire({
          title: "Success!",
          text: "Place added successfully.",
          imageUrl: "https://www.masrtimes.com/UploadCache/libfiles/39/3/600x338o/811.jpg",
          imageWidth: 400,
          imageHeight: 300,
          imageAlt: "Custom image"
        }).then(() => {
          this.router.navigate(['/places']);
        });
      });
    }
  }
}

