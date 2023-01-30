namespace ConsoleApp
{
    using ConsoleApp.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Program
    {
        static void Main(string[] args)
        {
            var reader = new DataReaderService();

            var databaseList = reader.ImportData("data.csv");
            DataPrintService.PrintDatabase(databaseList);

        }
    }
}
