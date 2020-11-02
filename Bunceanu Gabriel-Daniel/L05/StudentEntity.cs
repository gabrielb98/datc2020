using Microsoft.WindowsAzure.Storage.Table;

namespace WebJobAzureApp
{
    public class StudentEntity : TableEntity
    {
        public StudentEntity(string university, string cnp)
        {
            this.PartitionKey = university;
            this.RowKey = cnp;
        }
        public StudentEntity(){ }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public int Study_Year { get; set; }
        public string Phone_Number { get; set; }
        public string Faculty { get; set; }
    }
}