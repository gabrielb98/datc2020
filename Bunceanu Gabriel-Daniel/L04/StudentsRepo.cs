using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Students;

namespace AzureWebAPI
{
    public class StudentRepository : IStudentRepository
    {
        private string _conn_String;
        private CloudTableClient _client_table;
        private CloudTable _stud_Table;

        public StudentRepository(IConfiguration config)
        {
            _conn_String = config.GetValue<string>("AzureStorageAccountConnectionString");
           Task.Run(async () => { await InitializeTable();}).GetAwaiter().GetResult();
        }
    public async Task Insert_Student(StudentEntity student)
        {
            
            TableOperation insert = TableOperation.InsertOrMerge(student);
            TableResult result = await _stud_Table.ExecuteAsync(insert);
        }

    public async Task<List<StudentEntity>> Get_Students()
        {
            List<StudentEntity> students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            do{
                TableQuerySegment<StudentEntity> resultSegment = await _stud_Table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results);
            }while(token != null);
            return students;
        }

    public async Task Delete_Student(string partitionKey, string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
            TableResult result = await _stud_Table.ExecuteAsync(retrieve);

            StudentEntity delete_student = (StudentEntity)result.Result;
            TableOperation delete = TableOperation.Delete(delete_student);
            await _stud_Table.ExecuteAsync(delete);
        }
    public async Task Update_Student(string partitionKey, string rowKey, StudentEntity student)
        {
            student.PartitionKey = partitionKey;
            student.RowKey = rowKey;
            student.ETag = "*";
            TableOperation update = TableOperation.Replace(student);
            await _stud_Table.ExecuteAsync(update);
        }
    private async Task InitializeTable()
        {
                var azure_storage_account = CloudStorageAccount.Parse(_conn_String);
                _client_table = azure_storage_account.CreateCloudTableClient();
                _stud_Table = _client_table.GetTableReference("students");

                await _stud_Table.CreateIfNotExistsAsync();
        }
    }
}