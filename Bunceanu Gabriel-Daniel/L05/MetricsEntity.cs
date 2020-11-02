using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebJobAzureApp
{
    class MetricsEntity : TableEntity
    {
        public MetricsEntity(string university, string timestamp)
        {
            this.PartitionKey = university;
            this.RowKey = timestamp;
        }
        public MetricsEntity(){ }
        public int Count {get; set;}
    }
}
