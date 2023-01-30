using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    public class ImportedDatabase : ImportedObjectBaseClass
    {
        public int? NumberOfTables { get; set; }
        public List<ImportedTable> Tables { get; set; }
    }
}
