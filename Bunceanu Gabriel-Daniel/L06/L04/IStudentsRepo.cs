using System.Collections.Generic;
using System.Threading.Tasks;
using Students;

public interface IStudentRepository
{
    Task<List<StudentEntity>> Get_Students();
    Task Insert_Student(StudentEntity student);
    Task Update_Student(string partitionKey, string rowKey, StudentEntity student);
    Task Delete_Student(string partitionKey, string rowKey);
}