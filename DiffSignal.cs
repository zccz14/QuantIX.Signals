using System.Linq;
using QuantTC;
using QuantTC.Indicators.Generic;
using static System.Math;
using static QuantTC.X;

namespace QuantIX.Signals
{
    public class DiffSignal: Indicator<bool>
    {
        public DiffSignal(IIndicator<double> source, int period, double diff)
        {
            Source = source;
            Period = period;
            Diff = diff;
            LastCount = 0;
            Source.Update += Source_Update;
        }

        private void Source_Update()
        {
            Data.FillRange(Count, Source.Count, Calc);
            FollowUp();
        }

        private bool Calc(int argI)
        {
            LastCount = Max(LastCount, argI - Period + 1);
            var data = Source[argI];
            var high = Range(LastCount, argI + 1).Select(i => Source[i]).Max();
            var low = Range(LastCount, argI + 1).Select(i => Source[i]).Min();
            var result = high - data > Diff || data - low > Diff;
            if (result)
            {
                LastCount = Data.Count + 1;
            }
            return result;
        }

        private IIndicator<double> Source { get; }
        private int Period { get; }
        private double Diff { get; }
        private int LastCount { get; set; }
    }
}
