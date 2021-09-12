using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilitySolver.Structs
{
    public static class Pathing
    {
        public static string LocalPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ProbabilityApp";
        public static string LocalDocs = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        public static string CsvConfig = LocalPath + @"\CsvLocation.txt";
        public static string SqlConfig = LocalPath + @"\SqlServer.txt";
        public static string UseDB = LocalPath + @"\UseDatabase.txt";
    }
}
