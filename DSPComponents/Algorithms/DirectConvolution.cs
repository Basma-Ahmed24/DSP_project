using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }
       
        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> result = new List<float>();
            List<int> index = new List<int>();
            int start = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
            int end = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();
            for (int i = start; i <= end; i++)
            {
                float sum = 0;
                for (int j = start; j < InputSignal1.Samples.Count; j++)
                {
                    if (j>= InputSignal1.SamplesIndices.Min()&&j<= InputSignal1.SamplesIndices.Max()
                        && i-j >= InputSignal2.SamplesIndices.Min() && i - j <= InputSignal2.SamplesIndices.Max())
                    {
                        int idx1 = InputSignal1.SamplesIndices.IndexOf(j);
                        int idx2 = InputSignal2.SamplesIndices.IndexOf(i - j);
                        sum += InputSignal1.Samples[idx1] * InputSignal2.Samples[idx2];

                    }
                }
                if (i == end && sum == 0.0)
                    continue;
                result.Add(sum);
                index.Add(i);

            }
            OutputConvolvedSignal = new Signal(result, index, false);
        }
    }
    }

