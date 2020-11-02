using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace WebJobAzureApp
{
    public class MetricsRepo
    {
        private string _conn_String;
        private CloudTableClient _client_table;
        private CloudTable _metrics_Table;
        private CloudTable _stud_Table;

        public MetricsRepo()
        {
            _conn_String = "DefaultEndpointsProtocol=https;AccountName=datcl05storage;AccountKey=DElWgp+kb6+o27yC1USDXp7XIw75MVlM/tsh0w2lEXkaZA2Fx82l9/c9n5Lf+VcflyUjaVQhv2VLn+6lHycqQA==;EndpointSuffix=core.windows.net";
            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();

        }
        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_conn_String);
            _client_table = account.CreateCloudTableClient();
            _metrics_Table = _client_table.GetTableReference("stats");
            _stud_Table = _client_table.GetTableReference("students");
            

            await _metrics_Table.CreateIfNotExistsAsync();
            await _stud_Table.CreateIfNotExistsAsync();
        }
        public void InsertStats(string PartitionKey, int count)
        {
            MetricsEntity myMetrics = new MetricsEntity(PartitionKey, DateTime.UtcNow.ToString().Replace("/", "."));
            myMetrics.Count = count;

            TableOperation generalInsert = TableOperation.Insert(myMetrics);
            TableResult generalResult = _metrics_Table.ExecuteAsync(generalInsert).GetAwaiter().GetResult();
        }
        public async Task GetStats()
        {
            List<StudentEntity> students = new List<StudentEntity>();
            TableQuery<StudentEntity> query_table = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<StudentEntity> result = await _stud_Table.ExecuteQuerySegmentedAsync(query_table, token);
                token = result.ContinuationToken;
                students.AddRange(result.Results);

            } while (token != null);
            
            InsertStats("General", students.Count); //general stats

            //Stats for every University
            List<StudentEntity> Sorted_StudList = new List<StudentEntity>();
            Sorted_StudList = students.OrderBy(student => student.Faculty).ToList();
            
            List<List<StudentEntity>> listOfList = Sorted_StudList.GroupBy(student => student.Faculty).Select(group => group.ToList()).ToList();
            listOfList.ForEach( list => {
                Console.WriteLine(list.Count + " " + list[0].Faculty);
                 InsertStats(list[0].Faculty, list.Count);
            });
        }
    }
}