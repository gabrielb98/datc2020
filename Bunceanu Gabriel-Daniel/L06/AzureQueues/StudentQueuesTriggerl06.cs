using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureStudentQueues
{
    public static class StudentQueuesTriggerl06
    {
        [FunctionName("StudentQueuesTriggerl06")]
        [return: Table("students")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "datcl05storage_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}
