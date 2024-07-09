import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './pages/user/about/about.component';
import { HomeComponent } from './pages/user/home/home.component';
import { ErrorComponent } from './Components/error/error.component';
import { ContactComponent } from './pages/user/contact/contact.component';
import { RegisterComponent } from './Components/register/register.component';
import { HousingComponent } from './pages/user/housing/housing.component';
import { DetailsComponent } from './pages/user/details/details.component';
import { LoginComponent } from './Components/login/login.component';
import { AddComponent } from './pages/owner/add/add.component';
import { PlacesComponent } from './pages/owner/places/places.component';
import { DetailsPlacesComponent } from './pages/owner/details-places/details-places.component';
import { UpdatePlacesComponent } from './pages/owner/update-places/update-places.component';
import { LoginStudentComponent } from './Components/login-student/login-student.component';
import { LoginAdminComponent } from './Components/login-admin/login-admin.component';
import { ApprovalComponent } from './pages/admin/approval/approval.component';
import { NgModule } from '@angular/core';
import { authGuard } from './auth.guard';
import { DashBoardComponent } from './pages/user/dash-board/dash-board.component';
import { ApprovalStudentComponent } from './pages/user/approval-student/approval-student.component';
import { PendingStudentComponent } from './pages/user/pending-student/pending-student.component';
import { DetailsAdminComponent } from './pages/admin/details-admin/details-admin.component';
import { AllRentsComponent } from './pages/owner/all-rents/all-rents.component';
import { VodafoneComponent } from './pages/user/vodafone/vodafone.component';
import { PendingOwnerComponent } from './pages/owner/pending-owner/pending-owner.component';
import { ApprovalOwnerComponent } from './pages/owner/approval-owner/approval-owner.component';
import { AllPostComponent } from './pages/admin/all-post/all-post.component';
import { PendingAdminComponent } from './pages/admin/pending-admin/pending-admin.component';
import { AppartmentRentedByStudentsComponent } from './pages/admin/appartment-rented-by-students/appartment-rented-by-students.component';
import { ListStudentsComponent } from './pages/admin/list-students/list-students.component';
import { ListOwnerComponent } from './pages/admin/list-owner/list-owner.component';
import { AppartmentByOwnerComponent } from './pages/admin/appartment-by-owner/appartment-by-owner.component';
import { HousingMaleComponent } from './pages/user/housing-male/housing-male.component';
import { HousingFemaleComponent } from './pages/user/housing-female/housing-female.component';
import { RejectOwnerComponent } from './pages/owner/reject-owner/reject-owner.component';

export const routes: Routes = [
    {path:"",redirectTo:"home",pathMatch:"full"},
    {path:"home",component:HomeComponent},//
    {path:"about",canActivate:[authGuard] ,component:AboutComponent},//
    {path:"contact",canActivate:[authGuard] ,component:ContactComponent},//
    {path:"hosing",canActivate:[authGuard] ,component:HousingComponent},//
    {path:"details",canActivate:[authGuard] ,component:DetailsComponent},//
    {path:"register",component:RegisterComponent},
    {path:"login",component:LoginComponent},
    {path:"loginstudent",component:LoginStudentComponent},
    {path:"loginadmin",component:LoginAdminComponent},
    {path:"places",canActivate:[authGuard],component:PlacesComponent},
    {path:"places/:id",canActivate:[authGuard],component:DetailsPlacesComponent},
    {path:"updateplaces/:id",canActivate:[authGuard],component:UpdatePlacesComponent},
    {path:"addplaces",canActivate:[authGuard],component:AddComponent},
    {path:"approval",canActivate:[authGuard],component:ApprovalComponent},
    {path:"dashboard",canActivate:[authGuard],component:DashBoardComponent},
    {path:"aprovalstudent",canActivate:[authGuard],component:ApprovalStudentComponent},
    {path:"pendingstudent",canActivate:[authGuard],component:PendingStudentComponent},
    {path:"approval/:id",canActivate:[authGuard],component:DetailsAdminComponent},
    {path:"allrents",canActivate:[authGuard],component:AllRentsComponent},
    { path: 'vodafone',canActivate:[authGuard], component: VodafoneComponent },
    { path: 'pendingowner',canActivate:[authGuard], component: PendingOwnerComponent },
    { path: 'approvalowner',canActivate:[authGuard], component: ApprovalOwnerComponent },
    { path: 'allpost',canActivate:[authGuard], component: AllPostComponent },
    { path: 'pendingadmin',canActivate:[authGuard], component: PendingAdminComponent },
    { path: 'housingmale',canActivate:[authGuard], component: HousingMaleComponent },
    { path: 'housingfemale',canActivate:[authGuard], component: HousingFemaleComponent },
    { path: 'reject',canActivate:[authGuard], component: RejectOwnerComponent },



    {
        path: 'students',
        canActivate: [authGuard],
        component: ListStudentsComponent,
      },
      {
        path: 'appartmentrentedbystudent/:id',
        canActivate: [authGuard],
        component: AppartmentRentedByStudentsComponent,
      },
      {
        path: 'listowner',
        canActivate: [authGuard],
        component: ListOwnerComponent,
      },
      {
        path: 'appartmentbyowner/:id',
        canActivate: [authGuard],
        component: AppartmentByOwnerComponent,
      },


    {path:"**",component:ErrorComponent}
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
  })
  export class AppRoutingModule {}