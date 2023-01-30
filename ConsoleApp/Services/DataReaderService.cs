using ConsoleApp.Helpers;
using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Services
{
    public enum ImportedObjectTypes
    {
        [Description("database")]
        Database,
        [Description("table")]
        Table,
        [Description("column")]
        Column
    }

    public class DataReaderService
    {
        private Dictionary<ImportedObjectTypes, string> _importedTypesDics = new Dictionary<ImportedObjectTypes, string>();

        public List<ImportedDatabase> DatabasesImportedList = new List<ImportedDatabase>();

        public DataReaderService()
        {
            _importedTypesDics = Enum.GetValues(typeof(ImportedObjectTypes)).Cast<ImportedObjectTypes>().ToDictionary(p => p, p => EnumHelper.GetEnumDescription(p));
        }

        public List<ImportedDatabase> ImportData(string fileToImport)
        {
            if (string.IsNullOrEmpty(fileToImport))
            {
                throw new ArgumentNullException("Your file is empty or doesn't exist!");
            }

            try
            {
                var streamReader = new StreamReader(fileToImport);

                var importedLines = new List<string>();
                var importedTableList = new List<ImportedTable>();
                var importedColumnList = new List<ImportedColumn>();

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    importedLines.Add(line);
                }

                foreach (var line in importedLines.Skip(1).ToList())
                {
                    var values = line.Split(';').ToList();

                    if (!string.IsNullOrEmpty(values[0]))
                    {
                        if (_importedTypesDics[ImportedObjectTypes.Database] ==  values[0].ToLower())
                        {
                            var database = MapLinesToImportedDatabase(values);
                            DatabasesImportedList.Add(database);
                        }

                        if (_importedTypesDics[ImportedObjectTypes.Table] == values[0].ToLower())
                        {
                            var table = MapLinesToImportedTable(values);
                            importedTableList.Add(table);
                        }

                        if (_importedTypesDics[ImportedObjectTypes.Column] == values[0].ToLower())
                        {
                            var column = MapLinesToImportedColumns(values);
                            importedColumnList.Add(column);
                        }
                    }
                }

                // assign columns to tables
                foreach (var table in importedTableList)
                {
                    var tableName = table.Name.ToUpper();
                    table.Columns = importedColumnList.Where(p => p.ParentName.ToUpper() == tableName)?.ToList();

                    table.NumberOfColumn = table.Columns?.Count();
                }

                // assign tables to database
                foreach (var database in DatabasesImportedList)
                {
                    var databaseName = database.Name.ToUpper();

                    database.Tables = importedTableList.Where(p => p.ParentName.ToUpper() == databaseName)?.ToList();
                    database.NumberOfTables = database.Tables?.Count();
                }

                return DatabasesImportedList;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Something went wrong! {ex}");
            }

        }

        private ImportedTable MapLinesToImportedTable(List<string> values)
        {
            try
            {
                var table = new ImportedTable();

                table.Type = values[0]?.Trim();
                table.Name = values[1]?.Trim();
                table.Schema = values[2]?.Trim();
                table.ParentName = values[3]?.Trim();
                table.ParentType = values[4]?.Trim();
                table.DataType = values[5]?.Trim();
                table.IsNullable = values.Count == 7 ? values[6]?.Trim() : null;

                return table;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Reading from file went wrong! {ex}");
            }
        }

        private ImportedColumn MapLinesToImportedColumns(List<string> values)
        {
            try
            {
                var column = new ImportedColumn();

                column.Type = values[0]?.Trim();
                column.Name = values[1]?.Trim();
                column.Schema = values[2]?.Trim();
                column.ParentName = values[3]?.Trim();
                column.ParentType = values[4]?.Trim();
                column.DataType = values[5]?.Trim();
                column.IsNullable = values.Count == 7 ? values[6]?.Trim() : null;

                return column;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Reading from file went wrong! {ex}");
            }
        }

        private ImportedDatabase MapLinesToImportedDatabase(List<string> values)
        {
            try
            {
                var database = new ImportedDatabase();

                database.Type = values[0]?.Trim();
                database.Name = values[1]?.Trim() ?? throw new Exception("Database doesn't have name!");

                return database;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Reading from file went wrong! {ex}");
            }
        }
    }
}
