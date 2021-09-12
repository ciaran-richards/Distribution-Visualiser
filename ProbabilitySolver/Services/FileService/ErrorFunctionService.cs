using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Markup;
using CSharpFunctionalExtensions;
using Dasync.Collections;
using CsvHelper;
using CsvHelper.Configuration;

namespace ProbabilitySolver.Services.FileService
{
    public class ErrorFunctionService
    {
        public IAsyncEnumerable<double> key { get; set; }
        public IAsyncEnumerable<double> value { get; set; }

        /// <summary>
        /// Returns the Error Functon at several Standard Deviations
        /// </summary>
        /// <returns>
        /// Tuple<stDev/>,<values/>/>
        /// </returns>
        public async Task<Tuple<double[], double[]>> LoadCases()
        {
            var currentFolder = Environment.CurrentDirectory;
            var fileName = @"\ErrorFunction.csv";
            using (StreamReader input = File.OpenText(currentFolder + fileName))
            using (CsvReader reader = new CsvReader(input, CultureInfo.CurrentCulture))
            {
                var keys = new double[256];
                var values = new double[256];
                
                IAsyncEnumerable<dynamic> cases = reader.GetRecordsAsync<dynamic>();
                
                await cases.ForEachAsync(current =>
                {
                    int i = int.Parse(current.index); 
                    keys[i] = double.Parse(current.key);
                    values[i] = double.Parse(current.value);
                });
                return new Tuple<double[], double[]>(keys, values);
            }

        }
    }
}
