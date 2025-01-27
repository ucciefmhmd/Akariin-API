using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Enums
{
    public class ContractEnum
    {
        public enum ContractType
        {
            Residential,
            Commercial
        }

        public enum TerminationMethod
        {
            SpecificDuration,
            SpecificDate
        }
    }
}
