using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RX14.Utils
{
    public class Threading
    {
        public static void StartThread(ParameterizedThreadStart work)
        {
            Thread thread = new Thread(work);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
