using Microsoft.AspNetCore.Mvc;
using Waseet.Models;

namespace Waseet.Repsitory.RequestRepo
{
    public interface IRequestRepo
    {
        void makeRequest(int StudentID, int ApartmentID);
        void approveRequest(Request request);
        Request approveRequestById(int RequestID);
        Request deleteRequest(int id);
        List<Request> GetAllRequests();
        List<Request> GetAllApprovedRequests();
        Task<List<Request>> GetAllPendingRequests();
        Task<List<Request>> GetAllApprovedRequestsPerStudent(string StudentID);
        Task<List<Request>> GetAllPendingRequestsPerStudent(string StudentID);
        List<Request> GetAllRequestsRelatedToUserApartments(string userID);

        Task<int> GetStudentID(String UserIDFromIdentity);
        int GetOwnerID(String UserIDFromIdentity);


        Task<List<Request>> GetRequestsByPerStudent(int studentID);
        Task<List<Request>> GetRequestsByPerApartment(int ApartmentID);


        string GetUserEmail(int RequestID);
        string GetUserEmailByUserID(int RequestID);
        int SendMail(int RequestID);





        bool IsApprovedRequest(int RequestID);
        bool ISvalidAparmentID(int Apartment_ID);
        bool IsExistedAparmentID(int Apartment_ID);
        bool IExistedStudentID(int Student_ID);
        bool ISvalidStudentIDString(string Student_ID);
        bool ISvalidStudentID(int Student_ID);
        bool IExistedRequesttID(int requestID);
        bool IsEmptyTable();
        bool IsRepeatedRequest(int StudentID, int ApartmentID);
        bool IsRentedApartment(int ApartmentID);


    }
}
