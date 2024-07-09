using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using Waseet.DTO;
using Waseet.Migrations;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly RentalContext _context;
        private readonly IWebHostEnvironment environment;


        public OwnerController(RentalContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }

        [HttpGet("AllOwners")]
        public  ActionResult<IEnumerable<OwnerDTO>> GetAllOwners()
        {
            var owners =  _context.Owners
                .Select(o => new OwnerDTO { ID=o.ID , F_name=o.F_name , SSN=o.SSN , Address=o.Address , Phone = o.Phone })
                .ToList();

            

            return Ok(owners);
        }

        [HttpGet("GetAllApartmentsOnwer/{ownerId}")]
        public IActionResult GetAllApartmentsOnwer(int ownerId)
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Owner.ID == ownerId).ToList();

                var apartmentsWithImages = apartments.Select(apartment => new
                {
                    ApartmentId = apartment.Code,
                    Description = apartment.Description,
                    Location = apartment.Location,
                    ApartmentPrice = apartment.ApartmentPrice,
                    GenderOfStudents = apartment.GenderOfStudents,
                    Capacity = apartment.Capacity,
                    Region = apartment.Region,
                    NumofRoom = apartment.NumofRoom,
                    ImageUrls = GetImageUrlsForApartment(apartment.Code) // Modified to return multiple images
                }).ToList();

                return Ok(apartmentsWithImages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpDelete("{ownerId}")]
        public IActionResult DeleteOwner(int ownerId)
        {
            try
            {
                var owner = _context.Owners
                                    .Include(o => o.Apartments)
                                        .ThenInclude(a => a.Rents)
                                    .FirstOrDefault(o => o.ID == ownerId);

                if (owner == null)
                {
                    return NotFound($"Owner with ID {ownerId} not found.");
                }

                bool hasRentedApartments = owner.Apartments.Any(apartment =>
                    apartment.Rents.Any(rent => rent.Date <= DateTime.Now)); 

                if (hasRentedApartments)
                {
                    return BadRequest("Cannot delete the owner as they have rented apartments.");
                }

                _context.Owners.Remove(owner);
                _context.SaveChanges();

                return Ok($"Owner with ID {ownerId} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        private List<string> GetImageUrlsForApartment(int apartmentCode)
        {
            var imageUrls = new List<string>();
            // Construct the file path for the apartment's images
            string filePath = GetFilePath(apartmentCode);

            if (Directory.Exists(filePath))
            {
                string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var directoryInfo = new DirectoryInfo(filePath);
                foreach (var file in directoryInfo.GetFiles()) // Iterate over all files and add them to the list
                {
                    // Construct the URL for each image
                    string imageUrl = $"{hostUrl}/Images/Apartments/Apartment_{apartmentCode}/{file.Name}";
                    imageUrls.Add(imageUrl);
                }
            }

            // Return an empty list or a list with a default image URL if no images are found
            if (imageUrls.Count == 0)
            {
                string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var directoryInfo = new DirectoryInfo(Path.Combine(environment.WebRootPath, "Images", "Apartments", "DefaultImage"));
                foreach (var file in directoryInfo.GetFiles()) // Iterate over all files and add them to the list
                {
                    // Construct the URL for each image 
                    string imageUrl = $"{hostUrl}/Images/Apartments/DefaultImage/{file.Name}";
                    imageUrls.Add(imageUrl);
                }
            }
            return imageUrls;
        }

        private string GetFilePath(int apartmentCode)
        {
            return Path.Combine(environment.WebRootPath, "Images", "Apartments", $"Apartment_{apartmentCode}");
        }
    }
}
