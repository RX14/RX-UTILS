using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RX14.Utils
{
    /// <summary>
    /// Contains threading utils.
    /// </summary>
    public class Threading
    {
        /// <summary>
        /// Starts a thread in STA
        /// </summary>
        /// <param name="work">The ParamerterizedThreadStart to Start.</param>
        public static void StartThreadSTA(ParameterizedThreadStart work)
        {
            Thread thread = new Thread(work);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
