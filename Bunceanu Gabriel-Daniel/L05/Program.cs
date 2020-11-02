using System;

namespace WebJobAzureApp
{
    class Program
    {
        static void Main(string[] args)
        {
             new MetricsRepo().GetStats().GetAwaiter().GetResult();
        }
    }
}
