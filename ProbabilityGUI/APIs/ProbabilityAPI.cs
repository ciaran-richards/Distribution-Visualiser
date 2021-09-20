using System;
using System.Threading.Tasks;
using System.Windows;
using CSharpFunctionalExtensions;
using Prism.Events;
using Prism.Mvvm;
using ProbabilitySolver;
using ProbabilitySolver.APIs;
using ProbabilitySolver.Distributions;
using ProbabilitySolver.Services.FileService;
using ProbabilitySolver.Services.InputService;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;
using Shared.Events;

namespace ProbabilityGUI.APIs
{
    public class ProbabilityAPI : BindableBase, IAPI
    {
        private IEventAggregator _ea;
        public ProbabilityAPI(IEventAggregator ea)
        {
            LocalFolderService.RestoreLocalFolder();
            _ea = ea;
            ea.GetEvent<NewDistributionRequest>().Subscribe(async data =>
            {
               var task = await CreateDistribution(data);
               if (task.IsFailure) MessageBox.Show(task.Error);
            }, true); //async await
            ea.GetEvent<DistributionDeleteRequest>().Subscribe(distRow => DeleteDistribution(distRow));
            ea.GetEvent<SaveCsvEvent>().Subscribe(row => SaveCsv(row));
        }

        public async Task<Result> CreateDistribution(NewDistributionInfo newinfo)
        {
            var sql = new GridDataService();
            var test = sql.TestConnection();
            var useDBFlag = sql.UseDatabaseFlag();
            Result<IDistribution> dist;
            Result result;
            switch (newinfo.distributionType)
            {
                case DistributionEnums.Binomial:
                    {
                        dist = CreateBinomialDistribution(newinfo);
                        if (dist.IsFailure) return dist;
                        break;
                    }

                case DistributionEnums.Normal:
                    {
                        dist = await CreateNormalDistribution(newinfo);
                        if (dist.IsFailure) return dist;
                        break;
                    }
                default:
                    return Result.Failure("No Distribution Type");
            }
            if (useDBFlag.IsFailure || test.IsFailure)
                result = SaveCsv(dist.Value, newinfo.distributionRow);
            else if (!useDBFlag.Value)
                result = SaveCsv(dist.Value, newinfo.distributionRow);
            else
                result = SaveToDB(newinfo.distributionRow, dist.Value);
            if (result.IsSuccess)
                PublishDistributionAdded(newinfo.distributionRow);
            return result;
        }

        private async Task<Result<IDistribution>> CreateNormalDistribution(NewDistributionInfo newInfo)
        {
            // Verify Inputs
            double mean;
            double stdDev;

            try
            {
                mean = double.Parse(newInfo.info1);
                stdDev = double.Parse(newInfo.info2);
            }
            catch (Exception ex)
            {
                return Result.Failure<IDistribution>(ex.Message);
            }

            var data = new NormalDistribution(mean, stdDev);
            await data.WriteData(mean, stdDev);

            return Result.Success(data as IDistribution);

        }


        private Result<IDistribution> CreateBinomialDistribution(NewDistributionInfo newInfo)
        {
            // Verify All Inputs
            var nService = new InputServiceSmallInt();
            var small = nService.Run(newInfo.info1);
            var pService = new InputServiceProbability();
            var prob = pService.Run(newInfo.info2);

            var verify = Result.Combine(small, prob);
            if (verify.IsFailure)
            {
                return Result.Failure<IDistribution>(verify.Error);
            }

            // Create The Data
            var data = new BinomialDistribution(small.Value, prob.Value);
            return Result.Success(data as IDistribution);
        }

        private Result SaveToDB(DistributionRow distRow, IDistribution data)
        {
            // Save Distribution
            var daS = new DistributionDataSerivice();
            var addDistribution = daS.AddData(distRow);


            //Save Data
            var dS = new GridDataService();
            var addData = dS.AddData(distRow.dataId, data);


            // Trigger Events and Return Result
            return Result.Combine(new Result[] { addDistribution, addData }, "  ");
          
        }

        public void PublishDistributionDeleted(Guid id)
        {
            _ea.GetEvent<DistributionDeletedEvent>().Publish(id);
        }

        Result SaveCsv(DistributionRow row)
        {
            var helper = new CsvService();

            var result = helper.Save(row);
            if (result.IsFailure)
            {

                MessageBox.Show(result.Error);

            }

            return result;
        }

        Result SaveCsv(IDistribution distribution, DistributionRow dr)
        {
            var helper = new CsvService();

            return helper.SaveCSV(distribution, dr);
        }

        public void PublishDistributionAdded(DistributionRow info)
        {
            _ea.GetEvent<DistributionAddedEvent>().Publish(info);
        }

        Result DeleteDistribution(DistributionRow distRow)
        {
            if (distRow.IsFile())
            {
                var csvHelper = new CsvService();
                return csvHelper.Delete(distRow);
            }

            var distributionService = new DistributionDataSerivice();
            var delete = distributionService.DeleteData(distRow.distributionId);
            if (delete.IsFailure)
            {
                MessageBox.Show(delete.Error);
            }
            else
            {
                PublishDistributionDeleted(distRow.distributionId);
            }

            return delete;
        }


    }
}
