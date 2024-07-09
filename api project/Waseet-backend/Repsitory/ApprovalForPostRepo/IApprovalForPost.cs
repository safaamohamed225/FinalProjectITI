using Waseet.Models;
namespace Waseet.Repsitory.ApprovalForPostRepo
{
    public interface IApprovalForPost
    {
        void ApproveApartmentForPost(int apart_ID , int admin_ID);
        void RejectApartmentForPost(int apart_ID, int admin_ID, string RejectionMsg);
        List<int> GetApprovedAppartmentsByAdmin(int admin_ID);
        int GetAdmin_Who_ApprovedApartment(int Apartment_ID);
        bool ISvalidAparmentID(int Apartment_ID);
        bool ISvalidAdminID(int Admin_ID);
        bool IsApprovedApartment(int Apartment_ID);

    }
}
