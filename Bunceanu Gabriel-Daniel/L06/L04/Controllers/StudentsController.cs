using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Students;

namespace AzureWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private IStudentRepository _studsRepo;
        public StudentsController( IStudentRepository studentsRepository)
        {
            _studsRepo = studentsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            return await _studsRepo.Get_Students();
        }

        [HttpPost]
        public async Task Post([FromBody] StudentEntity student)
        {
            await _studsRepo.Insert_Student(student);    
        }

        [HttpPut("{partitionKey}/{rowKey}")]
        public async Task Update([FromRoute] string partitionKey, [FromRoute] string rowKey, [FromBody] StudentEntity student)
        {
            await _studsRepo.Update_Student(partitionKey, rowKey, student);
        }

        [HttpDelete("{partitionKey}/{rowKey}")]
        public async Task Delete([FromRoute] string partitionKey, [FromRoute] string rowKey)
        {
            await _studsRepo.Delete_Student(partitionKey, rowKey);
        }

    }
}