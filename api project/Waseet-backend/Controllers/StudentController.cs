using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.DTO;
using Waseet.Models;
using Waseet.Repsitory.StudentRepo;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Waseet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IstudentRepo ds;
        public StudentController(IstudentRepo _ds)
        {
            ds = _ds;
        }

        //get all students
        [HttpGet]
        public ActionResult GetAllStudents()
        {
            List<Student> allStudents = ds.GetAllStudents();
            
            List<StudentRentDTO> AllStList = new List<StudentRentDTO>();
            foreach (Student s in allStudents)
            {
                StudentRentDTO sd = new StudentRentDTO();

                sd.ID = s.ID;
                //sd.ID = s.ID_Identity;
                sd.Address = s.Address;
                sd.Phone = s.Phone;
                sd.F_name = s.F_name;
                sd.L_name = s.L_name;
                sd.Email = s.Email;
                sd.SSN = s.SSN;
               
                AllStList.Add(sd);
            }

            return Ok(AllStList);
        }


        //getById
        [HttpGet("{id:int}")]
        public ActionResult GetStudent(int id)
        {
            Student student = ds.GetStudentByID(id);
            if (student == null)
            {
                return NotFound("Please Enter A Valid ID");
            }
            else
            {
                StudentRentDTO s = new StudentRentDTO();
                s.ID = student.ID;
                //s.ID = student.ID_Identity;
                s.Address = student.Address;
                s.Phone = student.Phone;
                s.F_name=student.F_name;
                s.L_name=student.L_name;
                s.Email = student.Email;
                s.SSN=student.SSN;
                
                return Ok(s);
            }
            
        }


        //add Student
        [HttpPost]
        public ActionResult AddStudent([FromBody]StudentRentDTO student)
        {
            if (ModelState.IsValid)
            {
                if (student == null)
                    return BadRequest();
                else
                {
                    Student s = new Student();
                    s.ID = student.ID;
                    s.Address = student.Address;
                    s.Phone = student.Phone;
                    s.F_name = student.F_name;
                    s.L_name = student.L_name;
                    s.Email = student.Email;
                    s.SSN = student.SSN;
                    ds.AddStudent(s);
                    return Ok(s);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }     
        }


        //edit Student
        [HttpPut("{id:int}")]
        public ActionResult editStudent(int id , [FromBody]StudentRentDTO student)
        {
            Student s = new Student();
            s.ID = student.ID;
            s.Address = student.Address;
            s.Phone = student.Phone;
            s.F_name = student.F_name;
            s.L_name = student.L_name;
            s.Email = student.Email;
            s.SSN = student.SSN;

            if (ds.EditStudent(id, s)) 
                return Ok(s);
            else
                return NotFound("Invalid Selection");
        }



        //Delete Student
        [HttpDelete("{id:int}")]
        public ActionResult deleteStudent(int id)
        {
            if (ds.DeleteStudent(id))
            {
                Student s = ds.GetStudentByID(id);
                return Ok(s);
            }
                
            else
                return NotFound("Invalid Selection");
        }


    }
}
