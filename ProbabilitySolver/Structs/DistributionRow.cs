using System;

namespace ProbabilitySolver.Structs
{
    public class DistributionRow
    {
        public  Guid distributionId { get; set; }
        public Guid dataId { get; set; }
         public string name { get; set; }
         public string info { get; set; }

         public DistributionRow(string name, string info, Guid dataId, Guid distributionId)
         {
             this.name = name;
             this.info = info;
             this.dataId = dataId;
             this.distributionId = distributionId;
         }

         public bool IsFile() => this.distributionId == Guid.Empty;

    }
}
