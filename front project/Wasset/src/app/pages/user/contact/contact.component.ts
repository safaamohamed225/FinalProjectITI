import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { PlacesOwnerService } from '../../../services/places-owner.service';

@Component({
    selector: 'app-contact',
    standalone: true,
    templateUrl: './contact.component.html',
    styleUrl: './contact.component.css',
    imports: [RouterModule,
      FormsModule,
      HttpClientModule,
      ReactiveFormsModule,
      CommonModule,
     
    ]
})
export class ContactComponent {
  contactForm!: FormGroup;
  // buttonClicked = false;
  // formSubmitted = false;

  constructor(private fb: FormBuilder,private _PlacesOwnerService:PlacesOwnerService , private router: Router,private Actived : ActivatedRoute) { }
  ngOnInit(): void {
    this.contactForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100), Validators.pattern('[a-zA-Z ]*')]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      subject: ['', [Validators.required, Validators.maxLength(100)]],
      message: ['', Validators.required]
    });
  }
  onFormSubmit(): void {
    if (this.contactForm.valid) {
      let sendform={
        name:this.contactForm.controls['name'].value,
        email:this.contactForm.controls['email'].value,
        phone:this.contactForm.controls['phone'].value,
        subject:this.contactForm.controls['subject'].value,
        message:this.contactForm.controls['message'].value,
      };
     
      this.contactForm.reset();
      this._PlacesOwnerService.contactuc(sendform).subscribe(() => {
        Swal.fire({
          title: "Success!",
          text: "send successfully.",
          icon: "success"
        }).then(() => {
          this.router.navigate(['/home']);
        });
      });
    }
    //  else {
    //   this.markFormGroupTouched(this.contactForm);
    // }
  }

  // markFormGroupTouched(formGroup: FormGroup) {
  //   Object.values(formGroup.controls).forEach(control => {
  //     control.markAsTouched();

  //     if (control instanceof FormGroup) {
  //       this.markFormGroupTouched(control);
  //     }
  //   });
  // }
  getErrorMessages(controlName: string): string[] {
    const control = this.contactForm.get(controlName);
    const errorMessages: string[] = [];
    if (control && control.touched && control.errors) {
      Object.keys(control.errors).forEach(errorKey => {
        switch (errorKey) {
          case 'required':
            errorMessages.push('This field is required');
            break;
          case 'email':
            errorMessages.push('Invalid email format');
            break;
          case 'pattern':
            errorMessages.push('Invalid phone number format');
            break;
          case 'maxlength':
            errorMessages.push('Exceeded maximum length');
            break;
          default:
            break;
        }
      });
    }
    return errorMessages;
  }
//   checkFormValidity(): void {
//     this.buttonClicked = true;
//     if (this.contactForm.invalid) {
//       this.getErrorMessages();
//       return;
//     }
// }
}
