using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilitySolver.Structs
{
    public static class Pathing
    {
        //AppData
        public static string LocalPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ProbabilityApp";
        public static string CsvConfig = LocalPath + @"\CsvLocation.txt";
        public static string SqlConfig = LocalPath + @"\SqlServer.txt";
        public static string UseDB = LocalPath + @"\UseDatabase.txt";
        //Common Docs
        public static string LocalDocs = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        //Application
        static string ApplicationPath = Directory.GetCurrentDirectory();
        private static string SharedResources = ApplicationPath + @"\Shared";
        
        static string Images = SharedResources + @"\Images";
        public static string Icon = Images + @"\Icon.png";
        public static string ServerNameHelp = Images + @"\ServerNameHelp.png";

        static string Files = SharedResources + @"\Files";
        public static string ErrorFunction = Files + @"\ErrorFunction.csv";
    }
}
