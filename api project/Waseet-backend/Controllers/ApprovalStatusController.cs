using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.Models;
using Waseet.Repsitory.AdminRepo;
using Waseet.Repsitory.ApprovalForPostRepo;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalStatusController : ControllerBase
    {
        private readonly IApprovalForPost ds;
        public ApprovalStatusController(IApprovalForPost ds)
        {
            this.ds = ds;
        }

        int AdminIDG = 1;

        //Approve an Apartment For Post
        [HttpGet("Approve/{apartmentID:int}")]
        public IActionResult ApproveApartment(int apartmentID)
        {
           
                
                if (ds.ISvalidAparmentID(apartmentID))
                {
                    if (ds.IsApprovedApartment(apartmentID))
                    {
                        return BadRequest("Apartment Already Approved For Posting!");
                    }
                    else
                    {
                        if (ds.ISvalidAdminID(AdminIDG))
                        {
                            ds.ApproveApartmentForPost(apartmentID, AdminIDG);
                            return Ok();
                        }
                        return BadRequest("Invalid Admin Selection!");
                    }
            }
            else { return BadRequest(); }
     
            
        }



        //Reject an Apartment For Post
        [HttpGet("Reject/{apartmentID:int}/{RejectionMessage}")]
        public IActionResult RejectApartment(int apartmentID , string RejectionMessage)
        {
            if (ds.ISvalidAparmentID(apartmentID))
            {
                if (ds.IsApprovedApartment(apartmentID))
                {
                    return BadRequest("Apartment Already Approved For Posting!");
                }
                else
                {
                    if (ds.ISvalidAdminID(AdminIDG))
                    {
                        //ds.ApproveApartmentForPost(apartmentID, AdminIDG);
                        ds.RejectApartmentForPost(apartmentID, AdminIDG, RejectionMessage);
                        return Ok();
                    }
                    return BadRequest("Invalid Admin Selection!");
                }
            }
            else { return BadRequest(); }


        }



        // Get Approved Appartments ByAdmin
        [HttpGet("{id}")]
        public IActionResult GetAppartmentsByAdmin(string id) 
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid Admin Selection!");
            else
            {
                if (ds.ISvalidAdminID(AdminIDG))
                {
                    try
                    {
                        List<int> apartmentIDs = ds.GetApprovedAppartmentsByAdmin(AdminIDG);
                        return Ok(apartmentIDs);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Invalid Admin Selection!");
                    }

                }
                return BadRequest("Invalid Admin Selection!");
            }

            
        }


        // Get Admin Who Approved An Appartment
        [HttpGet("GetAdmin/{id}")]
        public IActionResult GetAdminWhoApproved(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            else
            {
                if (ds.ISvalidAparmentID(AdminIDG))
                {
                    try
                    {
                        int adminID = ds.GetAdmin_Who_ApprovedApartment(AdminIDG);
                        //AdminRepo adminContext = new AdminRepo();
                        //Admin admin = adminContext.GetAdminByID(adminID);
                        return Ok(adminID);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Invalid Apartment Selection!");
                    }

                }
                return BadRequest("Invalid Apartment Selection!");
            }
            
        }


        

    }
}
