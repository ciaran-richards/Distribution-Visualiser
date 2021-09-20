using CSharpFunctionalExtensions;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.SQLService
{
    public abstract class SQLBaseService
    {
        public string connectionString;
        private string FileName = @"Proba";

        protected SQLBaseService()
        {
            ResetConnectionString();
        }

        protected Result ResetConnectionString()
        {
            SQLServiceSetup setup = new SQLServiceSetup();
            var getConfig = setup.GetSqlConfig();
            if (getConfig.IsFailure)
                return getConfig;

            var serverName = getConfig.Value;
            connectionString = ($"Server = {serverName};" +
                                $"Data source = {serverName};" +
                                "Integrated Security = True;" +
                                $"database={FileName};" +
                                "connection timeout = 2");
            return Result.Success();
        }

        public Result ToggleUseDatabase()
        {
            var setting = UseDatabaseFlag();
            if (setting.IsFailure)
            {
                try
                {
                    File.WriteAllText(Pathing.UseDB, "false");
                    return Result.Success();
                }
                catch (Exception e)
                {
                    return Result.Failure(e.Message);
                }
            }
            var newSetting = (!setting.Value).ToString();
            File.WriteAllText(Pathing.UseDB, newSetting);
            return Result.Success();
        }
        public Result<bool> UseDatabaseFlag()
        {
            try
            {
                return Result.Success(bool.Parse(File.ReadAllText(Pathing.UseDB)));
            }
            catch (Exception e)
            {
                return Result.Failure<bool>(e.Message);
            }
        }

        public Result TestConnection()
        {
            // Test SQL Setup Configurations
            var sqlSetup = new SQLServiceSetup();
            var sqlSetupResult = sqlSetup.GetSqlConfig();
            if (sqlSetupResult.IsFailure)
                return sqlSetupResult;
            
            //Test connection
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return Result.Success();
                }
                catch (SystemException ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
