namespace ThreadSumSharp
{
    // передає межі діапазону і id у потік
    public class Bound
    {
        public int ThreadId { get; }
        public int StartIndex { get; }
        public int FinishIndex { get; }

        public Bound(int threadId, int startIndex, int finishIndex)
        {
            ThreadId = threadId;
            StartIndex = startIndex;
            FinishIndex = finishIndex;
        }
    }
}
