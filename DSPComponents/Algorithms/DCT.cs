using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {

            List<float> result = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; ++i)
            {
                double temp1 = 0;

                double sumx = 0;
                for (int j = 0; j < InputSignal.Samples.Count; ++j)
                {

                    temp1 = (float)((float)Math.PI / (4 * InputSignal.Samples.Count))*((2 * j) - 1)*((2 * i) - 1);

                    sumx += InputSignal.Samples[j] * Math.Cos(temp1);
                }

                sumx = (((float)Math.Sqrt((float)2 / InputSignal.Samples.Count)) * sumx);

                result.Add((float)sumx);


            }

            OutputSignal = new Signal(result, false);
        }
    }
}