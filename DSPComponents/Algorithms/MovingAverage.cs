using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
        List<float> result = new List<float>();
        int index;
        public override void Run()
        {
            index = InputWindowSize / 2;

            for (int i = index; i < (InputSignal.Samples.Count - index); i++)
            {
                for (int j = - index; j <= index; j++)
                {
                    if (j == -index)
                    {
                       result.Add(InputSignal.Samples[i + j]);
                    }
                    else
                    {
                        result[i - index] += InputSignal.Samples[i + j];
                    }
                }

                result[i - index] /= InputWindowSize;
            }
            Signal sig = new Signal(result, false);
            OutputAverageSignal = sig;
        }
    }
}
