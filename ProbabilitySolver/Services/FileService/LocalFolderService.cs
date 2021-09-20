using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.FileService
{
    public static class LocalFolderService
    {
        [Description("Checks application folder and repairs files within it.")]
        
        public static Result RestoreLocalFolder()
        {
            try
            {
                if (!Directory.Exists(Pathing.CommonAppData))
                {
                    Directory.CreateDirectory(Pathing.CommonAppData);
                }

                if (!File.Exists(Pathing.CsvConfig) || !Directory.Exists(File.ReadAllText(Pathing.CsvConfig)))
                {
                    File.WriteAllLines(Pathing.CsvConfig, new []{Pathing.LocalDocs});
                }

                if (!File.Exists(Pathing.UseDB))
                {
                    File.WriteAllLines(Pathing.UseDB,new []{bool.FalseString});
                }
                else
                {
                    string useDB = File.ReadAllText(Pathing.UseDB);
                    if (!(string.Equals(useDB, bool.TrueString) && !string.Equals(useDB, bool.FalseString)))
                    {
                        File.WriteAllLines(Pathing.UseDB, new[] { bool.FalseString });
                    }
                }
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }
        
    }
}
