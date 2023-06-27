using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            List<float> output = new List<float>();
            List<float> output1 = new List<float>();


            for (int i = 0; i < InputSignal.Samples.Count - 1; i++)
            {
                if (i == 0)
                {
                    output.Add(InputSignal.Samples[i] - 0);
                }
                else
                    output.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);

            }
            for (int i = 0; i < InputSignal.Samples.Count - 1; i++)
            {
                if (i == 0)
                {
                    output1.Add(((-2) * InputSignal.Samples[i]) + 0
                  + InputSignal.Samples[i + 1]);
                }
                else
                    output1.Add(((-2) * InputSignal.Samples[i]) + InputSignal.Samples[i - 1]
                        + InputSignal.Samples[i + 1]);

            }

            FirstDerivative = new Signal(output, false);
            SecondDerivative = new Signal(output1, false);
        }
    }
}
