using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.DTO;
using Waseet.Models;
using Waseet.Repsitory.AdminRepo;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepo ds;
        public AdminController(IAdminRepo _ds)
        {
            ds = _ds;
        }

        //get all admins details
        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            List<AdminDTO> admins = new List<AdminDTO>();
            List<Admin> adminsFromDS = ds.GetAllAdmins();
            foreach(Admin admin in adminsFromDS)
            {
                AdminDTO adminDTO = new AdminDTO();
                adminDTO.ID = admin.ID;
                adminDTO.F_name =admin.F_name;
                adminDTO.L_name =admin.L_name;
                adminDTO.Phone =admin.Phone;
                adminDTO.SSN =admin.SSN;
                adminDTO.Email =admin.Email;

                admins.Add(adminDTO);           
            }
            return Ok(admins);
        }

        //get admin details
        [HttpGet("{id:int}")]
        public IActionResult GetAdminByID(int id)
        {
            if (ds.IsExistedAdmin(id))
            {
                Admin admin= ds.GetAdminByID(id);
                return Ok(admin);
            }
            return BadRequest($"Invalid Admin Selection By ID {id}");
        }

       
        //Add New Admin
        [HttpPost]
        public IActionResult AddNewAdmin (AdminDTO adminDTO)
        {
            if (ds.IsExistedAdminSSN(adminDTO.SSN))
                return BadRequest($"Admin with SSN : {adminDTO.SSN} Already Exists");
            else
            {
                Admin admin = new Admin();
                admin.ID = adminDTO.ID;
                admin.F_name=adminDTO.F_name;   
                admin.L_name=adminDTO.L_name;
                admin.Phone =adminDTO.Phone;
                admin.SSN =adminDTO.SSN;
                admin.Email =adminDTO.Email;
                
                return Ok(ds.AddAdmin(admin));
            }
                
        }

        
        //Edit Existing Admin
        [HttpPut]
        public IActionResult EditAdmin (AdminDTO admin)
        {
            if (ds.IsExistedAdmin(admin.ID))
            {
                if (ds.IsExistedAdminSSN(admin.SSN))
                {
                    return BadRequest($"Admin SSN : {admin.SSN} Already Exists !");
                }

                //Admin old = ds.GetAdminByID(admin.ID);
                return Ok(ds.EditAdmin(admin.ID, admin)); 
            }
            
            return BadRequest("Invalid Admin Selection!");
            
        }

        //Delete Existing Admin
        [HttpDelete("{id:int}")]
        public IActionResult DeleteAdmin (int id)
        {
            if (ds.IsExistedAdmin(id))
            {
                return Ok(ds.DeleteAdmin(id)); 
            }
            return BadRequest("Invalid Admin Selection!");
        }



    }
}
