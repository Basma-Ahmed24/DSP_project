using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                InputSignal.SamplesIndices[i] *= -1;
            }

            InputSignal.Samples.Reverse();
            InputSignal.SamplesIndices.Reverse();
            OutputFoldedSignal = new Signal(InputSignal.Samples, InputSignal.SamplesIndices,!InputSignal.Periodic);


           
        }
    }
}
