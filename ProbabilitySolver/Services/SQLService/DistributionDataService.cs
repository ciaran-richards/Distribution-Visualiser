using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace ProbabilitySolver.Services.SQLService
{
    public class DistributionDataSerivice : SQLBaseService, IDistributionQuery
    // Implement csv Methods here
    {
        private string FileName = @"Proba";

        public Result AddData(DistributionRow distData)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var storeProc = new SqlCommand("CreateDistribution", connection);
                    storeProc.CommandType = CommandType.StoredProcedure;
                    storeProc.Parameters.Add("distributionId", SqlDbType.UniqueIdentifier).Value =
                        (SqlGuid) distData.distributionId;
                    storeProc.Parameters.Add("dataId", SqlDbType.UniqueIdentifier).Value = (SqlGuid) distData.dataId;
                    storeProc.Parameters.Add("name", SqlDbType.Text).Value = distData.name;
                    storeProc.Parameters.Add("description", SqlDbType.Text).Value = distData.info;
                    int rows = storeProc.ExecuteNonQuery();
                    return rows > 0 ? Result.Success() : Result.Failure("No Rows Written");
                }
                catch (Exception e)
                {
                    return Result.Failure(e.Message);
                }
            }
        }

        public Result DeleteData(Guid distributionID)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var storeProc = new SqlCommand("DeleteDistribution", connection);
                    storeProc.CommandType = CommandType.StoredProcedure;
                    storeProc.Parameters.Add("distributionId", SqlDbType.UniqueIdentifier).Value = (SqlGuid)distributionID;
                    
                    connection.Open();
                    int rows = storeProc.ExecuteNonQuery();
                    return Result.Success();
                }
                catch (Exception e)
                {
                    return Result.Failure(e.Message);
                }
            }
        }


        public Result<List<DistributionRow>> GetAllDistributions()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var storeProc = new SqlCommand("GetAllDistributions", connection);
                    storeProc.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = storeProc.ExecuteReader())
                    {
                        var listDistributions = new List<DistributionRow>();
                        while (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                var name = reader.GetString(0);
                                var desc = reader.GetString(1);
                                var distributionId = reader.GetSqlGuid(2);
                                var dataId = reader.GetSqlGuid(3);
                                listDistributions.Add
                                    (new DistributionRow(name, desc, (Guid) dataId, (Guid) distributionId));
                            }

                            reader.NextResult();
                        }

                        return Result.Success(listDistributions);
                    }
                }
                catch (Exception e)
                {
                    return Result.Failure<List<DistributionRow>>(e.Message);
                }
            }
        }
    }
}
