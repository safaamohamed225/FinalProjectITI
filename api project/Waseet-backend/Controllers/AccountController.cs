using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Waseet.DTO;
using Waseet.Models;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly RentalContext context;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager, RentalContext context)
        {
            this.userManager = userManager;
            this.config = config;
            this.roleManager = roleManager;
            this.context = context;
        }

        //Create New Account (Regesteration)
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existedUser = await userManager.FindByEmailAsync(userDTO.Email);
            var existedStudentPhone = await context.Students.SingleOrDefaultAsync(n => n.Phone == userDTO.Phone);
            var existedOwnerPhone = await context.Owners.SingleOrDefaultAsync(n => n.Phone == userDTO.Phone);
            var existedStudentSSN = await context.Students.SingleOrDefaultAsync(n => n.SSN == userDTO.SSN);
            var existedOwnerSSN = await context.Owners.SingleOrDefaultAsync(n => n.SSN == userDTO.SSN);

            if (existedUser is not null)
                return BadRequest(" Email is taken before ! ");

            if (existedStudentPhone is not null)
                return BadRequest("Phone is Exist");

            if (existedOwnerPhone is not null)
                return BadRequest("phone is Exist");

            if (existedStudentSSN is not null)
                return BadRequest("SSN is Exist");

            if (existedOwnerSSN is not null)
                return BadRequest("SSN is Exist");

            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.Email = userDTO.Email;
            applicationUser.UserName = userDTO.UserName;

            var guid = Guid.NewGuid();
            applicationUser.Id = guid.ToString();

            IdentityResult result = await userManager.CreateAsync(applicationUser, userDTO.Password);

            if (!result.Succeeded)
                return BadRequest("username is Exist ");

            if (!string.IsNullOrEmpty(userDTO.Role))
            {
                if (!await roleManager.RoleExistsAsync(userDTO.Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(userDTO.Role));
                }
                else
                {
                    await userManager.AddToRoleAsync(applicationUser, userDTO.Role);
                 

                    //var ayhaga =context.Users.Last();
                    /////Add Record To Student/Owner Table
                    if (userDTO.Role == "student" || userDTO.Role == "Student")
                    {
                        Student student = new Student()
                        {
                            Address = userDTO.Address,
                            Phone = userDTO.Phone,
                            F_name = userDTO.UserName,
                            L_name = "",
                            Email = userDTO.Email,
                            SSN = userDTO.SSN,
                            ID_Identity = guid.ToString()

                        };
                        context.Students.Add(student);
                        context.SaveChanges();
                        //return Ok($"User Account Created Successfully with Role: {userDTO.Role}");
                        return Ok();
                    }
                    else if (userDTO.Role == "owner" || userDTO.Role == "Owner")
                    {
                        Owner owner = new Owner()
                        {
                            Address = userDTO.Address,
                            Phone = userDTO.Phone,
                            F_name = userDTO.UserName,
                            L_name = "",
                            SSN = userDTO.SSN,
                            ID_Identity = guid.ToString()

                        };
                        context.Owners.Add(owner);
                        context.SaveChanges();
                        return Ok();
                        //return Ok($"User Account Created Successfully with Role: {userDTO.Role}");
                    }
                }
            }
            else
            {
                return BadRequest("Role Is Required!");
            }

            return Ok();
        }

        //Check Account Validation (Login)
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await userManager.FindByEmailAsync(loginDTO.Email);
                if (applicationUser != null)
                {
                    bool found = await userManager.CheckPasswordAsync(applicationUser, loginDTO.Password);
                    if (found)
                    {
                        /////Generate Token/////
                        //Claims
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        //get role
                        var roles = await userManager.GetRolesAsync(applicationUser);
                        foreach (var itemRole in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemRole));
                        }
                        SecurityKey securityKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

                        SigningCredentials signincred =
                            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        //Create Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],  //url web api
                            audience: config["JWT:ValidAudiance"], //url consumer angular
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signincred

                            );
                        return Ok(
                            new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                                expiration = mytoken.ValidTo
                            });
                    }
                    return Unauthorized("Invalid Password!");
                }
                return Unauthorized("Not Registered e-mail!");
            }
            return Unauthorized(ModelState);
        }
    }
}