using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1.Policies
{
    class CanDestroyDistrictPolicy
    {
        public static bool IsSatisfied(WarlordPlayerAction.DistrictCardToDestroy districtToDestroy, Player currentPlayer)
        {
            if (currentPlayer.Gold >= districtToDestroy.DistrictCard.Cost - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
