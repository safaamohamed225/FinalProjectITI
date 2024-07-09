// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';

// @Injectable({
//   providedIn: 'root'
// })
// export class PlacesOwnerService {
//   private readonly DB_URL = "http://localhost:3000/placesOwner";

//   constructor(private readonly myClient:HttpClient) { }
//   getAllPlaces(){
//     return this.myClient.get(this.DB_URL);
//   }
//   getPlacesByID(id:any){
//     return this.myClient.get(this.DB_URL+"/"+id);
//   }
//   AddNewPlaces(Places:any){
//     return this.myClient.post(this.DB_URL,Places);
//   }
//   updatePlaces(id:any,Places:any){
//     return this.myClient.put(this.DB_URL+"/"+id,Places);
//   }
//   deletePlaces(id:any){
//     return this.myClient.delete(this.DB_URL+"/"+id);
//   }
// }



import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PlacesOwnerService {
  //private readonly DB_URL = "http://localhost:3000/placesOwner";

  constructor(private readonly myClient:HttpClient) { }
  getAllPlaces(){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllApartments");
  }
  getPlacesByID(id:any){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetApartment"+"/"+id);
  }
  AddNewPlaces(Places:any){
    return this.myClient.post("https://localhost:44301/api/Apartment/AddApartment",Places);
  }
  
updatePlaces(apartmentId: any, Places: any) {
  const headers = new HttpHeaders({ apartmentId: apartmentId,});
  return this.myClient.put('https://localhost:44301/api/Apartment/UpdateApartment', Places,{ headers: headers });
}
  deletePlaces(id:any){
    return this.myClient.delete("https://localhost:44301/api/Apartment/DeleteApartment"+"/"+id);
  }
 

  approvalForPost(id:any){
    return this.myClient.get("https://localhost:44301/api/ApprovalStatus/Approve"+"/"+id);
  }
  getallAppHousing(){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllApprovalApartments");
  }
  makeRent(idStd:any,idDb:any){
    return this.myClient.post("https://localhost:44301/api/Request"+"/"+idStd+"/"+idDb,null);
  }
  getAllrentOwner(id:any){
    return this.myClient.get("https://localhost:44301/api/Request/GetAllRequestsRelatedToSpecificOwner"+"/"+id);
  }

  getallPendingAdmin(){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllPendingApartments");
  }
  getAllApprovalStd(id :any){
    return this.myClient.get("https://localhost:44301/api/Request/GetAllApprovedRequestsPerStudent"+"/"+id);
   }
   price:number=0;

   getAllPendingStd(id :any){
    return this.myClient.get("https://localhost:44301/api/Request/GetAllPendingRequestsPerStudent"+"/"+id);
   }
   approvalRentByOwner(idReq:any,idApart:any){
    return this.myClient.post("https://localhost:44301/api/Request/approvalfromurl"+"/"+idReq+"/"+idApart,null);
   }
 delRentByOwner(idReq:any){
    return this.myClient.delete("https://localhost:44301/api/Request"+"/"+idReq);
   }

   getallappByOneowner(id:any){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllApartmentsOnwer"+"/"+id)
   }

   getallPendingByOneowner(id:any){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllPendingApartmentsOnwer"+"/"+id)
   }
   getallRejectByOneowner(id:any){
    return this.myClient.get("   https://localhost:44301/api/Apartment/GetAllRejectedApartmentsOnwer"+"/"+id)
   }
   getallApprovalByOneowner(id:any){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllApprovalApartmentsOnwer"+"/"+id)
   }


   getallApprovalAdmin(){
    return this.myClient.get("https://localhost:44301/api/Apartment/GetAllApprovalApartments");
  }
 contactuc(ob:any){
  return this.myClient.post("https://localhost:44301/api/Mail/ContactUS",ob)
 }  





 getallAppartmentRentedByStudent(id: any) {
  return this.myClient.get(
    `https://localhost:44301/api/Apartment/GetAllApartmentsRentedByStudent/${id}`
  );
}

getAllStudents() {
  return this.myClient.get('https://localhost:44301/api/Student');
}
getAllOwner() {
  return this.myClient.get('https://localhost:44301/api/Owner/AllOwners');
}

getallAppartmenByOwner(id: any) {
  return this.myClient.get(
    `https://localhost:44301/api/Owner/GetAllApartmentsOnwer/${id}`
  );
}
deleteStudent(id:any){
  return this.myClient.delete("https://localhost:44301/api/Student"+"/"+id);
}
deleteOwner(id:any){
  return this.myClient.delete("https://localhost:44301/api/Owner"+"/"+id);
}


getallMale(){
  return this.myClient.get("https://localhost:44301/api/Apartment/GetAllMaleApprovalApartments");
}
getallFemale(){
  return this.myClient.get("https://localhost:44301/api/Apartment/GetAllFemaleApprovalApartments");
}
getreject(id:any,msg:any){
  return this.myClient.get("https://localhost:44301/api/ApprovalStatus/Reject"+"/"+id+"/"+msg);
}


}



