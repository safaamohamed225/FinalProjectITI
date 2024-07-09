using Microsoft.EntityFrameworkCore;
using Waseet.DTO;
using Waseet.Models;

namespace Waseet.Repsitory.AdminRepo
{
    public class AdminRepo : IAdminRepo
    {
        private readonly RentalContext context;
        public AdminRepo(RentalContext _context) 
        { 
            context = _context;
        }

        //public AdminRepo()
        //{
        //    RentalContext context = new RentalContext();
        //}



        public Admin AddAdmin(Admin admin)
        {
            context.Admins.Add(admin);
            context.SaveChanges();
            return admin;
        }

        public bool IsExistedAdmin(int id) {
            return context.Admins.Any(m=>m.ID==id);
        }
        public Admin DeleteAdmin(int id)
        {
            Admin admin = context.Admins.Find(id);
            context.Admins.Remove(admin);
            context.SaveChanges();
            return admin;
        }

        public AdminDTO EditAdmin(int id, AdminDTO NewAdmin)
        {
            Admin old = context.Admins.Find(id);
            old.Email = NewAdmin.Email;    
            old.F_name = NewAdmin.F_name;
            old.L_name = NewAdmin.L_name;
            old.SSN = NewAdmin.SSN;
            old.Phone = NewAdmin.Phone;

            context.SaveChanges();
           
            return NewAdmin;
        }

        public Admin GetAdminByID(int id)
        {
            Admin admin = context.Admins.Where(m=>m.ID==id).FirstOrDefault();
            return admin;
        }

        public List<Admin> GetAllAdmins()
        {
            return context.Admins.Include(m=>m.ApprovalStatuses).ToList();
        }


        public bool IsExistedAdminSSN(string ssn)
        {
            return context.Admins.Any(m => m.SSN == ssn);
        }

        //public int GetAdminID(string UserIDFromIdentity)
        //{
        //    var user = context.Admins.SingleOrDefault(a => a.ID_Identity == UserIDFromIdentity);
        //    int studentID = user.ID;
        //    return studentID;
        //}


    }
}
