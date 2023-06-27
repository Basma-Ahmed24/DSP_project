using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        List<float> output = new List<float>();
        public override void Run()
        {
            float x = 0;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                x += InputSignal.Samples[i];
                output.Add(x);
            }
            OutputSignal = new Signal(output, false);
        }
    }
}
