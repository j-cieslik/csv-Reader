using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Services
{
    public static class DataPrintService
    {

        public static void PrintDatabase(List<ImportedDatabase> importedDatabases)
        {
            foreach (var database in importedDatabases)
            {
                Console.WriteLine($"Database: {database.Name} ({database.NumberOfTables} tables) ");

                foreach (var table in database.Tables)
                {
                    Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfColumn} columns)");

                    foreach (var column in table.Columns)
                    {
                        Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable == "1" ? "accepts nulls" : "with no nulls")}");
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
