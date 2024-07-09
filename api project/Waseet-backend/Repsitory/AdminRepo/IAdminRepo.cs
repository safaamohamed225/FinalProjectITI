using Waseet.DTO;
using Waseet.Models;

namespace Waseet.Repsitory.AdminRepo
{
    public interface IAdminRepo
    {
        //Get all
        List<Admin> GetAllAdmins();


        //Get By ID
        Admin GetAdminByID(int id);

        //Add New
        Admin AddAdmin(Admin admin);


        //Edit
        AdminDTO EditAdmin(int id, AdminDTO admin);


        //Delete
        Admin DeleteAdmin(int id);

        bool IsExistedAdmin(int id);

        bool IsExistedAdminSSN(string ssn);

        //int GetAdminID(String UserIDFromIdentity);
    }
}
