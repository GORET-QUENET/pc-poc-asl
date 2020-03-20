namespace HandPositionReader.Scripts.Structs
{
    public struct Metrics
    {
        public int TP; // True positives
        public int FP; // False positives
        public int FN; // False negatives
        public int TN; // True negatives 

        public static Metrics Zero
        {
            get
            {
                Metrics metrics = new Metrics();
                metrics.TP = 0;
                metrics.FP = 0;
                metrics.FN = 0;
                metrics.TN = 0;

                return metrics;
            }
        }
    }
}
