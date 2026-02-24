using System.Threading;

namespace threaddemo
{
    public class BreakHandler
    {
        private volatile bool canBreak = false;

        public bool IsCanBreak {
            get {
                return canBreak;
            }
        }

        public void HoldSleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
            canBreak = true;
        }
    }
}