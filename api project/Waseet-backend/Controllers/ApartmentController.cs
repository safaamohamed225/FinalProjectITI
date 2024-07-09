using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Drawing;
using System.IO;
using Waseet.DTO;
using Waseet.Models;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {

        private readonly RentalContext _context;
        private readonly IWebHostEnvironment environment;

        public ApartmentController(RentalContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }

        [HttpGet("GetApartment/{id:int}")]
        public IActionResult GetApartment(int id)
        {
            try
            {
                // Retrieve apartment by ID
                var apartment = _context.Apartments.FirstOrDefault(a => a.Code == id);
                if (apartment == null)
                {
                    return NotFound("Apartment not found");
                }


                //Generate file path
                string filePath = GetFilePath(apartment.Code);

                // Get image URLs
                List<string> imageUrls = new List<string>();
                if (Directory.Exists(filePath))
                {
                    string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string fileName = fileInfo.Name;
                        string imageUrl = hostUrl + $"/Images/Apartments/Apartment_{apartment.Code}/{fileName}";

                        imageUrls.Add(imageUrl);
                    }
                }


                string rentStatus;
                if (_context.Rents.Any(r => r.ApartmentId == id))
                {
                    rentStatus = "Rented";
                }
                else
                {
                    rentStatus = "NotRented";
                }


                //Create DTO with apartment and its images

                var apartmentWithImages = new
                {

                    OwnerID = apartment.OwnerID,
                    Capacity = apartment.Capacity,
                    ApartmentPrice = apartment.ApartmentPrice,
                    Description = apartment.Description,
                    GenderOfStudents = apartment.GenderOfStudents,
                    Location = apartment.Location,
                    NumofRoom = apartment.NumofRoom,
                    Region = apartment.Region,
                    Images = imageUrls,
                    RentStatus = rentStatus
                };

                return Ok(apartmentWithImages);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetApartmentImages/{id:int}")]
        public IActionResult GetApartmentImages(int id)
        {
            try
            {
                // Retrieve apartment by ID
                var apartment = _context.Apartments.FirstOrDefault(a => a.Code == id);
                if (apartment == null)
                {
                    return NotFound("Apartment not found");
                }


                // Generate file path
                string filePath = GetFilePath(apartment.Code);

                // Get image URLs
                List<string> imageUrls = new List<string>();
                if (Directory.Exists(filePath))
                {
                    string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string fileName = fileInfo.Name;
                        string imageUrl = hostUrl + $"/Images/Apartments/Apartment_{apartment.Code}/{fileName}";
                        imageUrls.Add(imageUrl);


                    }
                }

                return Ok(imageUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }





        //[Authorize(Roles ="owner,Owner")]
        [HttpPost("AddApartment")]
        public IActionResult AddApartment(ApartmentDTO apartmentDTO)
        {
            if (!_context.Owners.Any(e => e.ID_Identity == apartmentDTO.OwnerID))
            {
                return BadRequest("Owner does not exist.");
            }
            var user = _context.Owners.SingleOrDefault(s => s.ID_Identity == apartmentDTO.OwnerID);
            //int OwnerID = user.ID;

            if (ModelState.IsValid)
            {
                APIResponse response = new APIResponse();
                // Handle apartment images
                int passCount = 0;
                int errorCount = 0;


                // Map DTO to Apartment entity
                var apartment = new Apartment
                {
                    Location = apartmentDTO.Location,
                    Description = apartmentDTO.Description,
                    Region = apartmentDTO.Region,
                    ApartmentPrice = apartmentDTO.ApartmentPrice,
                    Status = "pending",
                    GenderOfStudents = apartmentDTO.GenderOfStudents,
                    Capacity = apartmentDTO.Capacity,
                    OwnerID = user.ID,
                    NumofRoom = apartmentDTO.NumofRoom
                };


                // You may need to map other properties here


                // Add apartment to database
                _context.Apartments.Add(apartment);
                _context.SaveChanges();


                try
                {

                    // Generate file path
                    string filePath = GetFilePath(apartment.Code);

                    // Create directory if it doesn't exist
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    foreach (var image in apartmentDTO.images)
                    {
                        string fileName = $"{apartment.Code}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                        string imagePath = Path.Combine(filePath, fileName);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        // Save image
                        using (FileStream stream = System.IO.File.Create(imagePath))
                        {
                            image.CopyTo(stream);
                            passCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    response.Message = ex.Message;
                }
                response.ResponseCode = 200;
                response.Result = passCount + " Files uploaded &" + errorCount + " files failed";

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet("GetAllApartments")]
        public IActionResult GetAllApartments()
        {
            try
            {
                var apartments = _context.Apartments.ToList();

                var apartmentsWithImages = apartments.Select(apartment => new
                {
                    ApartmentId = apartment.Code,
                    Description = apartment.Description,
                    Location = apartment.Location,
                    ApartmentPrice = apartment.ApartmentPrice,
                    GenderOfStudents=apartment.GenderOfStudents,
                    Capacity = apartment.Capacity,
                    Region=apartment.Region,
                    NumofRoom=apartment.NumofRoom,
                    Status=apartment.Status,
                    ImageUrls = GetImageUrlsForApartment(apartment.Code) // Modified to return multiple images
                }).ToList();

                return Ok(apartmentsWithImages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }



        [HttpGet("GetAllApartmentsRentedByStudent/{StudentID:int}")]
        public IActionResult GetAllApartmentsRentedByStudent(int StudentID)
        {
            try
            {
                List<int> RentedApartmentsIDs = _context.Rents.Where(r=>r.StudentId == StudentID).Select(s=>s.ApartmentId).ToList();
                if (RentedApartmentsIDs != null)
                {
                    var apartments = _context.Apartments.Where(a => RentedApartmentsIDs.Contains(a.Code)).ToList();
                    if (apartments != null)
                    {
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
                    else
                       {
                           return NotFound();
                       }
                }
                else
                    {
                       return NotFound();
                    }    
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("GetAllApartmentsOnwer/{onwerIdentity}")]
        public IActionResult GetAllApartmentsOnwer(string onwerIdentity)
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Owner.ID_Identity == onwerIdentity).ToList();

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
                    Status=apartment.Status, //modified 7-4
                    ImageUrls = GetImageUrlsForApartment(apartment.Code) // Modified to return multiple images
                }).ToList();

                return Ok(apartmentsWithImages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }





        [HttpGet("GetAllPendingApartments")]
        public IActionResult GetAllPendingApartments()
        {
            try
            {
                var apartments = _context.Apartments.Where(a=>a.Status=="pending").ToList();

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


        [HttpGet("GetAllPendingApartmentsOnwer/{onwerIdentity}")]
        public IActionResult GetAllPendingApartmentsOnwer(string onwerIdentity)
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Status == "pending").Where(a=>a.Owner.ID_Identity==onwerIdentity).ToList();

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

        [HttpGet("GetAllApprovalApartments")]
        public IActionResult GetAllApprovalApartments()
        {
            try
            {
                var apartments = _context.Apartments.Where(a=> a.Status=="approved").ToList();

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


        [HttpGet("GetAllMaleApprovalApartments")]
        public IActionResult GetAllMaleApprovalApartments()
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Status == "approved").Where(a=>a.GenderOfStudents=="Male"||a.GenderOfStudents=="male").ToList();

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

        [HttpGet("GetAllFemaleApprovalApartments")]
        public IActionResult GetAllFemaleApprovalApartments()
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Status == "approved").Where(a => a.GenderOfStudents == "Female" || a.GenderOfStudents == "female").ToList();

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




        [HttpGet("GetAllApprovalApartmentsOnwer/{onwerIdentity}")]
        public IActionResult GetAllApprovalApartmentsOnwer(string onwerIdentity)
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Status == "approved").Where(a=> a.Owner.ID_Identity==onwerIdentity).ToList();

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



        [HttpGet("GetAllRejectedApartmentsOnwer/{onwerIdentity}")]
        public IActionResult GetAllRejectedApartmentsOnwer(string onwerIdentity)
        {
            try
            {
                var apartments = _context.Apartments.Where(a => a.Status == "rejected").Where(a => a.Owner.ID_Identity == onwerIdentity).ToList();

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


        [HttpPost("UpdateApartmentImages/{apartmentCode:int}")]
        public IActionResult UpdateApartmentImages(int apartmentCode, [FromForm] IFormFileCollection newImages)
        {
            // Check if the apartment exists
            var apartmentExists = _context.Apartments.Any(a => a.Code == apartmentCode);
            if (!apartmentExists)
            {
                return NotFound($"No apartment found with code {apartmentCode}");
            }

            // Validate the incoming files
            if (newImages == null || !newImages.Any())
            {
                return BadRequest("No images provided for update.");
            }

            try
            {
                // Generate file path for the apartment images
                string filePath = GetFilePath(apartmentCode);

                // Delete existing images
                if (Directory.Exists(filePath))
                {
                    DirectoryInfo di = new DirectoryInfo(filePath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                else
                {
                    // If directory doesn't exist, create it
                    Directory.CreateDirectory(filePath);
                }

                // Save new images
                foreach (var image in newImages)
                {
                    if (image.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}"; // Generate a unique name for each file
                        string fullPath = Path.Combine(filePath, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }
                    }
                }

                return Ok($"Images for apartment {apartmentCode} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating images: {ex.Message}");
            }
        }

       

        private List<string> SaveImages(int apartmentCode, IFormFileCollection images)
        {
            var imagePaths = new List<string>();
            var webRoot = environment.WebRootPath; // Make sure you have injected IWebHostEnvironment _env in the constructor
            var imagePath = Path.Combine(webRoot, "Images", "Apartment", $"Apartment_{apartmentCode}");

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            foreach (var image in images)
            {
                // Ensure the file has content
                if (image.Length > 0)
                {
                    // Create a unique file name to prevent overwriting existing files
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var fullPath = Path.Combine(imagePath, fileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        image.CopyTo(fileStream); // Copy the image to the wwwroot directory
                        imagePaths.Add(fullPath); // Add the path to the list (you may want to save this list in the database)
                    }
                }
            }

            return imagePaths;
        }

        [HttpDelete("DeleteApartment/{apartmentId:int}")]
        public IActionResult DeleteApartment(int apartmentId)
        {
            // Find the apartment by ID
            var apartment = _context.Apartments
                        .Include(a => a.Rents) // Assuming there's a navigation property Rents in Apartment
                        .FirstOrDefault(a => a.Code == apartmentId);
            if (apartment == null)
            {
                return NotFound($"Apartment with ID {apartmentId} not found.");
            }

            if (apartment.Rents.Any(rent => rent.Date <= DateTime.Now)) // Adjust condition based on your model
            {
                return BadRequest("Cannot delete the apartment as it is currently rented.");
            }
            //Attempt to delete the images from the file system
             var imagesDeleted = DeleteApartmentImages(apartmentId);
            if (!imagesDeleted) 
            {
                // Optional: Decide how to handle the situation where the images couldn't be deleted.
                // You could choose to stop the process and return an error, or you could continue
                // and delete the apartment entity while logging the issue with the images.
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete apartment images.");
            }

            // Remove the apartment from the DbContext
            _context.Apartments.Remove(apartment);

            // Save changes to the database
            _context.SaveChanges();

            return Ok();
        }

        private bool DeleteApartmentImages(int apartmentId)
        {
            var webRoot = environment.WebRootPath; // Make sure you have injected IWebHostEnvironment _env in the constructor
            var imagePath = Path.Combine(webRoot, "Images", "Apartment", $"Apartment_{apartmentId}");
            if (Directory.Exists(imagePath))
            {
                try
                {
                    // Delete all files in the directory
                    DirectoryInfo di = new DirectoryInfo(imagePath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    // Then delete the directory itself
                    di.Delete();

                    return true;
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logging framework or custom logging logic)
                    // Optionally, you can re-throw the exception or handle it as needed
                    return false;
                }
            }
            // If the directory doesn't exist, treat it as a successful deletion
            return true;
        }

        [HttpPut("UpdateApartment")]
        public IActionResult UpdateApartment([FromHeader] string apartmentId,  ApartmentUpdateDTO apartmentDTO)
        {
            // Find the apartment by ID
            var apartment = _context.Apartments.FirstOrDefault(a => a.Code.ToString() == apartmentId);
            if (apartment == null)
            {
                return NotFound($"Apartment with ID {apartmentId} not found.");
            }

            // Update apartment properties
            apartment.Description = apartmentDTO.Description;
            apartment.Location = apartmentDTO.Location;
            apartment.ApartmentPrice = apartmentDTO.ApartmentPrice;
            apartment.GenderOfStudents = apartmentDTO.GenderOfStudents;
            apartment.Capacity= apartmentDTO.Capacity;
            apartment.NumofRoom= apartmentDTO.NumofRoom;
            apartment.Region= apartmentDTO.Region;



            // Save changes to the database

            _context.SaveChanges();

            return Ok();
        }

        //apartment.NumofRoom= apartmentDTO.NumofRoom;
        // ... other properties as needed from the DTO
        // _context.Entry(apartment).CurrentValues.SetValues(apartmentDTO); // Alternative if matching property names

        // If you have a method to delete old images
        //bool imagesDeleted = DeleteApartmentImages(apartmentId);
        //if (!imagesDeleted)
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete old images.");
        //}

        // If you have a method to save new images
        //var imagePaths = SaveImages(apartmentId, apartmentDTO.images);






        // Helper method to generate file path
        private string GetFilePath(int apartmentCode)
        {
            return Path.Combine(environment.WebRootPath ,"Images", "Apartments", $"Apartment_{apartmentCode}");
        }


        [HttpGet("{id}/price")]
        public ActionResult<decimal> GetApartmentPrice(int id)
        {
            // Using FirstOrDefault instead of Find as Find does not have a synchronous version
            var apartment = _context.Apartments.FirstOrDefault(a => a.Code == id);

            if (apartment == null)
            {
                return NotFound($"Apartment with ID {id} not found.");
            }

            return apartment.ApartmentPrice;
        }


    }

    public class APIResponse
    {
        public int ResponseCode { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
    }
}
