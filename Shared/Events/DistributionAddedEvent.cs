using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using ProbabilitySolver.Structs;

namespace Shared.Events
{
    public class DistributionAddedEvent : PubSubEvent<DistributionRow>
    {
    }
}
