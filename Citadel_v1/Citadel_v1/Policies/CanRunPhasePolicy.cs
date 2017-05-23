using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class CanRunPhasePolicy
    {
        public static bool IsSatisfied(Phase phase)
        {
            var resultPhase = phase as ResultPhase;

            if (resultPhase == null)
            {
                return true;
            }

            return resultPhase.IsEnd;
        }
    }
}
