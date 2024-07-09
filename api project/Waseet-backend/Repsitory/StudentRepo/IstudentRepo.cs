using Waseet.Models;

namespace Waseet.Repsitory.StudentRepo
{
    public interface IstudentRepo
    {
        //Get all
        List<Student> GetAllStudents();


        //Get By ID
        Student GetStudentByID(int id);

        //Add New
        Student AddStudent(Student student);


        //Edit
        bool EditStudent(int id, Student student);


        //Delete
        bool DeleteStudent(int id);


        int GetStudentID(String UserIDFromIdentity);
    }
}
