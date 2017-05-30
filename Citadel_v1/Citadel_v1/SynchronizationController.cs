using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class SynchronizationController
    {
        public ManualResetEvent ResetEventController { get; set; }
        public AutoResetEvent ResetEventModel { get; set; }

        public SynchronizationController(bool state)
        {
            ResetEventController = new ManualResetEvent(state);
            ResetEventModel = new AutoResetEvent(state);
        }
    }
}
