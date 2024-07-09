import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AboutComponent } from './pages/user/about/about.component';
import { HeaderComponent } from "./Components/header/header.component";
import { HomeComponent } from './pages/user/home/home.component';
import { FooterComponent } from './Components/footer/footer.component';
import { ErrorComponent } from './Components/error/error.component';
import { ContactComponent } from './pages/user/contact/contact.component';
import { RegisterComponent } from './Components/register/register.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { HousingComponent } from './pages/user/housing/housing.component';
import { HttpClientModule} from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

//import{BrowserAnimationsModule} from '@angular/platform-browser/animations'
import { CarouselModule } from 'ngx-owl-carousel-o';
import { NgxPaginationModule } from 'ngx-pagination';
import { DashBoardComponent } from './pages/user/dash-board/dash-board.component';
import { ApprovalStudentComponent } from './pages/user/approval-student/approval-student.component';
import { PendingStudentComponent } from './pages/user/pending-student/pending-student.component';
import { DetailsAdminComponent } from './pages/admin/details-admin/details-admin.component';
import { AllRentsComponent } from './pages/owner/all-rents/all-rents.component';
import { VodafoneComponent } from './pages/user/vodafone/vodafone.component';
import { PendingOwnerComponent } from './pages/owner/pending-owner/pending-owner.component';
import { ApprovalOwnerComponent } from './pages/owner/approval-owner/approval-owner.component';
import { AllPostComponent } from './pages/admin/all-post/all-post.component';
import { AppartmentRentedByStudentsComponent } from './pages/admin/appartment-rented-by-students/appartment-rented-by-students.component';
import { ListStudentsComponent } from './pages/admin/list-students/list-students.component';
import { ListOwnerComponent } from './pages/admin/list-owner/list-owner.component';
import { AppartmentByOwnerComponent } from './pages/admin/appartment-by-owner/appartment-by-owner.component';
import { PlacesOwnerService } from './services/places-owner.service';
import { PaymentService } from '../../payment.service';
import { HousingMaleComponent } from './pages/user/housing-male/housing-male.component';
import { HousingFemaleComponent } from './pages/user/housing-female/housing-female.component';
import { RejectOwnerComponent } from './pages/owner/reject-owner/reject-owner.component';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [
      RouterOutlet, 
      AboutComponent,
      HomeComponent, 
      RouterModule, 
      HeaderComponent,
      FooterComponent,
      ErrorComponent,
      HousingComponent,
      RegisterComponent,
      ContactComponent,
      HttpClientModule,
      ReactiveFormsModule,
      //BrowserAnimationsModule,
      CarouselModule,
      NgxPaginationModule,
      DashBoardComponent,
      ApprovalStudentComponent,
      PendingStudentComponent,
      DetailsAdminComponent,
      AllRentsComponent,
      PendingOwnerComponent,
      ApprovalOwnerComponent,
      AllPostComponent,
      AppartmentRentedByStudentsComponent,
      ListStudentsComponent,
      ListOwnerComponent,
      AppartmentByOwnerComponent,
      HousingMaleComponent,
      HousingFemaleComponent,
      RejectOwnerComponent
      
    ],
    providers:[PlacesOwnerService,PaymentService],
    
    //providers:[provideHttpClient().withFetch()]
    
})
export class AppComponent {
  title = 'Project';
}

