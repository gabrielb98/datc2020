using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace UPT_Students.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET students/
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentRepo.students;
        }

        // GET students/3
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return StudentRepo.students.FirstOrDefault(s => s.StudentID == id);
        }

        // POST students/
        [HttpPost]
        public string StudentAdd([FromBody] Student student_to_add)
        {
            try
            {
                StudentRepo.students.Add(student_to_add); 
                return "Student was added!";
            }
            catch(System.Exception e)
            {
                return "Error: " + e.Message;
                throw;
            }
        }

        // PUT students/
        [HttpPut()]
          public string StudentUpdate([FromBody] Student student){

            try
            {
                int UpdateID = StudentRepo.students.FindIndex(s => s.StudentID == student.StudentID); 

                StudentRepo.students[UpdateID].LastName = student.LastName;
                StudentRepo.students[UpdateID].FirstName = student.FirstName;
                StudentRepo.students[UpdateID].Faculty = student.Faculty;
                StudentRepo.students[UpdateID].StudyYear = student.StudyYear;
                return "List was updated!";
            }
            catch(System.Exception e)
            {
                return "Error: " + e.Message;
                throw;
            } 
            
        }

        // DELETE students/3
        [HttpDelete("{id}")]
       public string StudentDelete([FromRoute] int id)
        {
            try
            {
                Student studentToDelete = StudentRepo.students.First(s => s.StudentID == id); 
                StudentRepo.students.Remove(studentToDelete);
                return "Student was deleted!";
            }
            catch(System.Exception e)
            {
                return "Error: " + e.Message;
                throw;
            }
        }
        private readonly ILogger<StudentsController> _logger;
        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }


    }
}
