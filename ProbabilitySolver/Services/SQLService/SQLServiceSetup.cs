using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.SQLService
{
    public class SQLServiceSetup
    {

        private string dbName;
        private string filePath = Pathing.CommonAppData;
        private string FileName = @"Proba";
        private string serverConnectionString;
        private string databaseConnectionString;

        public SQLServiceSetup(string name = "localhost")
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name))
                name = "localhost";
            dbName = name;
            serverConnectionString = $"Server={dbName}; Integrated security=SSPI; Trusted_Connection=yes; database=master";
            databaseConnectionString = $"Server = {dbName}; " +
                                                     $"Data source = {dbName}; " +
                                                     "Integrated Security = True; " +
                                                     $"database={FileName}; " +
                                                     "connection timeout = 3";
        }

        public Result TestConnection()
        {
            var service = new GridDataService();
            return service.TestConnection();
        }

        public Result Create()
        {
            if (TestConnection().IsSuccess)
            {
                return Result.Failure("There is already a valid database");
            }

            var dataBase = RunDatabaseCommand(CreateDB, serverConnectionString);
            if (dataBase.IsFailure)
            {
                return dataBase;
            }

            var distributionTable = RunDatabaseCommand(createdistributionTable, databaseConnectionString);
            if (distributionTable.IsFailure)
            {
                return distributionTable;
            }
            var dataTable = RunDatabaseCommand(createdataTable, databaseConnectionString);

            var addDataSP = RunDatabaseCommand(createDistributionSP, databaseConnectionString);
            var addDistributionSP = RunDatabaseCommand(createDataSP, databaseConnectionString);
            var deleteDistributionSP = RunDatabaseCommand(deleteDBSP, databaseConnectionString);
            var getDistributionsSP = RunDatabaseCommand(getAlldistributionSP, databaseConnectionString);
            var getDataSP = RunDatabaseCommand(this.getDataSP, databaseConnectionString);

            var commands = new List<Result>()
            {
                dataBase, distributionTable, dataTable,
                addDataSP, addDistributionSP, deleteDistributionSP,
                getDistributionsSP, getDataSP
            };
            if (commands.Any(x => x.IsFailure))
                return commands.First(x => x.IsFailure);
            return Result.Success();
        }

        private Result RunDatabaseCommand(string command, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand myCommand = new SqlCommand(command, connection);
                try
                {
                    connection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return Result.Failure("Please check ServerName "+ e.Message);
                }
            }
            return Result.Success();
        }

        public Result SaveSqlConfig(string sqlName)
        {
            var appData = Pathing.SqlConfig;
            string[] lines = new string[1];
            lines[0] = sqlName;
            try
            {
                File.WriteAllLines(appData, lines);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
            return Result.Success();
        }

        public Result<string> GetSqlConfig()
        {
            string path;
            try
            {
                path = File.ReadAllLines(Pathing.SqlConfig)[0];
                if (path == string.Empty)
                    return Result.Failure<string>("No file path selected, choose one in: Set Data Menu");
            }
            catch (Exception e)
            {
                return Result.Failure<string>(e.Message);
            }

            return Result.Success(path);
        }

        string CreateDB =>
            $"CREATE DATABASE {FileName} ON PRIMARY " +
            $"(NAME = {FileName}_Data, " +
            $@"FILENAME = '{filePath}\{FileName}_Data.mdf', " +
            "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
            $"LOG ON (NAME = {FileName}_Log, " +
            $@"FILENAME = '{filePath}\{FileName}_Log.ldf', " +
            "SIZE = 1MB, " +
            "MAXSIZE = 5MB, " +
            "FILEGROWTH = 10%)";

        string createdataTable => //
            "CREATE TABLE datas(" +
            "dataId UNIQUEIDENTIFIER NOT NULL, " +
            "indexing INT NOT NULL, " +
            "cases FLOAT NOT NULL," +
            "probability FLOAT NOT NULL," +
            "sumProbability FLOAT NOT NULL," +
            "PRIMARY KEY (dataId, indexing)" +
            ");";

        string createdistributionTable => //
            "CREATE TABLE distributions(" +
            "distributionId UNIQUEIDENTIFIER NOT NULL, " +
            "dataId UNIQUEIDENTIFIER NOT NULL, " +
            "name varchar(255) NOT NULL," +
            "description varchar(255) NULL," +
            "PRIMARY KEY (distributionId)" +
            ")";

        string createDistributionSP => //
            "CREATE PROCEDURE CreateDistribution " +
            "@distributionId UNIQUEIDENTIFIER, @dataId UNIQUEIDENTIFIER, " +
            "@name VARCHAR(50), @description VARCHAR(50) " +
            "AS " +
            "BEGIN " +
            "INSERT dbo.distributions " +
            "(" +
            "distributionId, " +
            "dataId, " +
            "name, " +
            "description " +
            ") " +
            "VALUES " +
            "(" +
            "@distributionId," +
            "@dataId, " +
            "@name," +
            "@description " +
            ") " +
            "END";

        string createDataSP => //
            "CREATE PROCEDURE CreateData " +
            "@dataId UNIQUEIDENTIFIER, @index INT, @cases FLOAT," +
            "@probability FLOAT, @sumProbability FLOAT " +
            "AS " +
            "BEGIN " +
            "INSERT dbo.datas " +
            "( " +
            "dataId, " +
            "indexing, " +
            "cases," +
            "probability, " +
            "sumProbability " +
            ") " +
            "VALUES " +
            "(" +
            "@dataId, " +
            "@index, " +
            "@cases, " +
            "@probability, " +
            "@sumProbability " +
            ") " +
            "END ";

        string deleteDBSP =>
            "CREATE PROCEDURE DeleteDistribution " +
            "@distributionId UNIQUEIDENTIFIER " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "DECLARE @dataId UNIQUEIDENTIFIER " +
            "SET @dataId = (SELECT dataId FROM dbo.distributions WHERE distributionId = @distributionId) " +
            "DELETE FROM dbo.datas " +
            "WHERE dataId = @dataId " +
            "DELETE FROM dbo.distributions " +
            "WHERE distributionId = @distributionId " +
            "END ";

        string getAlldistributionSP => //
            "CREATE PROCEDURE GetAllDistributions " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "SELECT name, description, distributionId, dataId FROM dbo.distributions " +
            "END ";

        string getDataSP => //
            "CREATE PROCEDURE GetData " +
            "@inputID UNIQUEIDENTIFIER " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "SELECT cases, probability, sumProbability FROM dbo.datas " +
            "WHERE dataId = @inputID " +
            "END ";
    }
}
