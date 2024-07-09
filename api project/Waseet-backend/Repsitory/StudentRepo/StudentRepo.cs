using Microsoft.EntityFrameworkCore;
using Waseet.Models;

namespace Waseet.Repsitory.StudentRepo
{
    public class StudentRepo : IstudentRepo
    {
        private readonly RentalContext context;
        public StudentRepo(RentalContext _context)
        {
            context = _context;
        }

        public List<Student> GetAllStudents()
        {
            return context.Students.Include(i => i.Rents).ToList();
        }

        public Student GetStudentByID(int id)
        {
            return context.Students.Include(i => i.Rents).SingleOrDefault(st => st.ID == id);
        }

        public Student AddStudent(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();

            return student;
        }

        public bool EditStudent(int id, Student NewStudent)
        {
            Student oldStudent = context.Students.Find(id);
            if (oldStudent != null)
            {
                oldStudent.SSN = NewStudent.SSN;
                oldStudent.F_name = NewStudent.F_name;
                oldStudent.L_name = NewStudent.L_name;
                oldStudent.Address = NewStudent.Address;
                oldStudent.Email = NewStudent.Email;
                oldStudent.Phone = NewStudent.Phone;
                context.SaveChanges();
                return true;
            }
            else
                return false;

        }

        public bool DeleteStudent(int id)
        {
            Student s = context.Students.Find(id);
            if (s != null)
            {
                context.Students.Remove(s);
                context.SaveChanges();

                return true;
            }

            else return false;

        }


        public int GetStudentID(string UserIDFromIdentity)
        {
            var user = context.Students.SingleOrDefault(s => s.ID_Identity == UserIDFromIdentity);
            int studentID = user.ID;
            return studentID;
        }


    }
}
