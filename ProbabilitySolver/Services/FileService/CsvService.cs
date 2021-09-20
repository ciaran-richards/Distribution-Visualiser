using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CsvHelper;
using Dasync.Collections;
using ProbabilitySolver.Distributions;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.FileService
{

    /// <summary>
    /// 
    /// </summary>
    public class CsvService : IFileService, IDistributionQuery
    {

        public Result ChangeDirectory(string newPath)
        {
            var appData = Pathing.CsvConfig;
            string[] lines = new string[1];
            lines[0] = newPath;
            try
            {
                if (!Directory.Exists(newPath))
                    return Result.Failure("Path does not exist");
                File.WriteAllLines(appData, lines);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
            return Result.Success();
        }

        public Result<string> GetSavePath()
        {
            string path;
            try
            {
                path = File.ReadAllLines(Pathing.CsvConfig)[0];
                if (path == string.Empty)
                    return Result.Success(Environment.SpecialFolder.CommonDocuments.ToString());
            }
            catch (Exception e)
            {
                LocalFolderService.RestoreLocalFolder();
                return Result.Failure<string>(e.Message);
            }

            return Result.Success(path);
        }
        public Result Save(DistributionRow row)
        {
            string filePath = null;
            if (row.IsFile())
            {
                return Result.Failure("This file already exists as a CSV");
            }
            var getPath = GetSavePath();
            var getRows = GetDistributionFromDB(row);

            Result[] results = new Result[2];
            results[0] = getPath;
            results[1] = getRows;

            if (results.Any((h) => h.IsFailure))
            {
                return Result.Combine(results);
            }

            filePath = MakeSavePath(getPath.Value, row).Value;
            return SaveCSV(getRows.Value, filePath);
        }

        public Result<List<DistributionRow>> GetAllDistributions()
        {
            var distRows = new List<DistributionRow>();
            var path = GetSavePath();
            if (path.IsFailure)
                return Result.Failure<List<DistributionRow>>("AppData File is missing");

            var distStrings = new List<string>(Directory.EnumerateFiles(path.Value));

            distStrings.RemoveAll(x => !x.EndsWith("].csv"));
            foreach (var str in distStrings)
            {
                var name = Path.GetFileNameWithoutExtension(str);
                var info = name.Split(' ').Last();
                distRows.Add(new DistributionRow(name, info, Guid.Empty, Guid.Empty));
            }

            return Result.Success(distRows);
        }

        public Result<string> GetFile(DistributionRow row)
        {
            var path = GetSavePath();
            var fileName = path.Value + "/" + row.name + ".csv";
            return Result.Success(fileName);
        }

        public Result Delete(DistributionRow row)
        {
            var filePath = GetFile(row);
            if (filePath.IsFailure)
                return filePath;
            try
            {
                File.Delete(filePath.Value);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
            return Result.Success();
        }

        public async Task<Result<List<GridRow>>> Load(DistributionRow row)
        {
            var filePath = GetFile(row);
            var gridRows = new List<GridRow>();
            try
            {
                using (StreamReader input = File.OpenText(filePath.Value))
                using (CsvReader reader = new CsvReader(input, CultureInfo.CurrentCulture))
                {
                    var keys = new double[256];
                    var values = new double[256];

                    var cases = reader.GetRecords<dynamic>();

                    foreach (var current in cases)
                    {
                        gridRows.Add(new GridRow(
                            double.Parse(current.cases), double.Parse(current.probability),
                            double.Parse(current.sumProbability)));

                    };
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<List<GridRow>>("The file has been corrupted");
            }
            return Result.Success(gridRows);
        }


        private Result<string> MakeSavePath(string path, DistributionRow dR)
        {
            string name = dR.info;

            string filePath = path + "/" + dR.name + ' ' + dR.info + ".csv";

            return Result.Success(filePath);
        }

        private Result<List<GridRow>> GetDistributionFromDB(DistributionRow dR)
        {
            var dataServiceS = new GridDataService();
            return dataServiceS.GetData(dR.dataId);
        }

        public Result SaveCSV(IDistribution distribution, DistributionRow distRow)
        {
            var folder = GetSavePath();
            if (folder.IsFailure) return folder;
            var filePath = MakeSavePath(GetSavePath().Value, distRow);
            if (filePath.IsFailure) return filePath;
            return SaveCSV(distribution, filePath.Value);
        }

        private Result SaveCSV(IDistribution distribution, string filePath)
        {
            var gridRow = new List<GridRow>();
            if (distribution is IDiscreteDistribution discDist)
            {
                for (int i = 0; i < distribution.Probabilities.Length; i++)
                {
                    gridRow.Add(new GridRow(discDist.Cases[i], discDist.Probabilities[i], discDist.SumProbability[i]));
                }

                return SaveCSV(gridRow, filePath);
            }

            if (distribution is IContinuousDistribution contDist)
            {
                for (int i = 0; i < distribution.Probabilities.Length; i++)
                {
                    gridRow.Add(new GridRow(contDist.Cases[i], contDist.Probabilities[i], contDist.SumProbability[i]));
                }

                return SaveCSV(gridRow, filePath);
            }
            return Result.Failure("Invalid Distribution Type");
        }
        private Result SaveCSV(List<GridRow> distribution, string filePath)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    var csv = new CsvWriter(writer, CultureInfo.CurrentCulture);
                    csv.WriteRecords(distribution); // where values implements IEnumerable
                }
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
            return Result.Success();
        }
    }
}
