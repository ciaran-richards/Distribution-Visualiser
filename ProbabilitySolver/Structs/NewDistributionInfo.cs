using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilitySolver.Structs
{
    public struct NewDistributionInfo
    {
        public DistributionRow distributionRow;
        public string info1;
        public string info2;
        public DistributionEnums distributionType;

        public NewDistributionInfo(string Name, string Info1, string Info2, string description, DistributionEnums DistributionType)
        {
            
            distributionRow = new DistributionRow(Name, description, Guid.NewGuid(), Guid.NewGuid());
            info1 = Info1;
            info2 = Info2;
            distributionType = DistributionType;
        }
    }
}
