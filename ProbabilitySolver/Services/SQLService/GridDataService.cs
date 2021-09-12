using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProbabilitySolver.Distributions;
using ProbabilitySolver.Services.FileService;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.SQLService
{
    public class GridDataService : SQLBaseService
    {
        public async Task<Result<List<GridRow>>> GetAsync(DistributionRow distRow)
        {
            var x = new List<GridRow>();
            if (distRow.IsFile())
            {
                var csvService = new CsvService();
                return await csvService.Load(distRow);
                //return grid.Value;
            }
            return await GetDataAsync(distRow.dataId);
        }

        public Result AddData(Guid Id, IDistribution distribution)
        {
            try
            {
                int ro = 0;
                int affected = 0;

                var dt = new DataTable();
                dt.Columns.Add("dataId", typeof(Guid));
                dt.Columns.Add("indexing", typeof(int));
                dt.Columns.Add("cases", typeof(double));
                dt.Columns.Add("probability", typeof(double));
                dt.Columns.Add("sumProbability", typeof(double));

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (distribution is IDiscreteDistribution a)
                    {
                        for (int i = 0; i <= a.Cases.Length - 1; i++)
                        {
                            var storeProc = new SqlCommand("CreateData", connection);
                            storeProc.CommandType = CommandType.StoredProcedure;
                            storeProc.Parameters.Add("dataId", SqlDbType.UniqueIdentifier).Value = (SqlGuid)Id;
                            storeProc.Parameters.Add("index", SqlDbType.Int).Value = (SqlInt32)i;
                            storeProc.Parameters.Add("cases", SqlDbType.Float).Value = (double)a.Cases[i];
                            storeProc.Parameters.Add("probability", SqlDbType.Float).Value = a.Probabilities[i];
                            storeProc.Parameters.Add("sumProbability", SqlDbType.Float).Value = a.SumProbability[i];
                            storeProc.ExecuteNonQuery();
                        }
                    }

                    if (distribution is IContinuousDistribution b)
                    {
                        for (int i = 0; i < b.Cases.Length; i++)
                        {
                            var storeProc = new SqlCommand("CreateData", connection);
                            storeProc.CommandType = CommandType.StoredProcedure;
                            storeProc.Parameters.Add("dataId", SqlDbType.UniqueIdentifier).Value = (SqlGuid)Id;
                            storeProc.Parameters.Add("index", SqlDbType.Int).Value = (SqlInt32)i;
                            storeProc.Parameters.Add("cases", SqlDbType.Float).Value = b.Cases[i];
                            storeProc.Parameters.Add("probability", SqlDbType.Float).Value = b.Probabilities[i];
                            storeProc.Parameters.Add("sumProbability", SqlDbType.Float).Value = b.SumProbability[i];
                            storeProc.ExecuteNonQuery();
                        }
                    }
                }
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        public async Task<Result> AddDataAsync(Guid Id, IDistribution distribution)
        {
            try
            {
                var listTask = new List<Task>();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    if (distribution is IDiscreteDistribution a)
                    {
                        for (int i = 0; i < a.Cases.Length; i++)
                        {
                            var sqlCommand = new SqlCommand("CreateData", connection);
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add("dataId", SqlDbType.UniqueIdentifier).Value = (SqlGuid)Id;
                            sqlCommand.Parameters.Add("index", SqlDbType.Int).Value = (SqlInt32)i;
                            sqlCommand.Parameters.Add("cases", SqlDbType.Float).Value = (double)a.Cases[i];
                            sqlCommand.Parameters.Add("probability", SqlDbType.Float).Value = a.Probabilities[i];
                            sqlCommand.Parameters.Add("sumProbability", SqlDbType.Float).Value = a.SumProbability[i];
                            listTask.Add(sqlCommand.ExecuteNonQueryAsync());
                        }

                        await Task.WhenAll(listTask.ToArray());
                    }

                    if (distribution is IContinuousDistribution b)
                    {
                        for (int i = 0; i <= b.Cases.Length - 1; i++)
                        {
                            var sqlCommand = new SqlCommand("CreateData", connection);
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add("dataId", SqlDbType.UniqueIdentifier).Value = (SqlGuid)Id;
                            sqlCommand.Parameters.Add("index", SqlDbType.Int).Value = (SqlInt32)i;
                            sqlCommand.Parameters.Add("cases", SqlDbType.Float).Value = (double)b.Cases[i];
                            sqlCommand.Parameters.Add("probability", SqlDbType.Float).Value = b.Probabilities[i];
                            sqlCommand.Parameters.Add("sumProbability", SqlDbType.Float).Value = b.SumProbability[i];
                            await sqlCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        public async Task<Result<List<GridRow>>> GetDataAsync(Guid dataId)
        {
            var selection = "SELECT cases, probability, sumProbability FROM dbo.datas " +
                            $"WHERE dataId = '{dataId}'";
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand(selection, connection);
                    var listData = new List<GridRow>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var cases = reader.GetDouble(0);
                                var probability = reader.GetDouble(1);
                                var sumProbability = reader.GetDouble(2);
                                listData.Add(new GridRow(cases, probability, sumProbability));
                            }

                            reader.NextResult();
                        }
                    }
                    return Result.Success(listData);
                }
            }
            catch (Exception e)
            {
                return Result.Failure<List<GridRow>>(e.Message);
            }
        }

        public Result<List<GridRow>> GetData(Guid DataID)
        {
            var selection = "SELECT cases, probability, sumProbability FROM dbo.datas " +
                            $"WHERE dataId = '{DataID}'";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(selection, connection);
                SqlDataReader reader = command.ExecuteReader();
                var listData = new List<GridRow>();
                try
                {
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var cases = reader.GetDouble(0);
                            var prob = reader.GetDouble(1);
                            var sumProb = reader.GetDouble(2);
                            listData.Add(new GridRow(cases, prob, sumProb));
                        }

                        reader.NextResult();
                    }

                    return Result.Success(listData);
                }
                catch (Exception e)
                {
                    return Result.Failure<List<GridRow>>(e.Message);
                }
            }
        }
    }
}


