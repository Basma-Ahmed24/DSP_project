using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            List<float> endP = new List<float>();
            List<float> midP = new List<float>();
            List<float> quantize = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<String>();
            OutputSamplesError = new List<float>();
            float range;
            float min = InputSignal.Samples.Min();
            float max = InputSignal.Samples.Max();
            if (InputLevel == 0)
            {
                InputLevel = (int)(Math.Pow(2, InputNumBits));
            }
            else if (InputNumBits == 0)
            {
                InputNumBits = (int)(Math.Log(InputLevel, 2));
            }
            range = (max - min) / InputLevel;
            float var = min;
            endP.Add(var);
            while (var <= max)
            {
                endP.Add(var + range);
                var = var + range;
            }
            float mid_point;
            for (int i = 0; i < InputLevel; i++)
            {
                mid_point = (endP[i] + endP[i + 1]) / 2;
                midP.Add(mid_point);
            }
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < endP.Count; j++)
                {
                    if (InputSignal.Samples[i] >= endP[j] && InputSignal.Samples[i] < endP[j + 1] + 0.0001)
                    {
                        quantize.Add((float)Math.Round((Decimal)midP[j], 3, MidpointRounding.AwayFromZero));
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        OutputIntervalIndices.Add(j + 1);
                        break;
                    }

                }
            }
            OutputQuantizedSignal = new Signal(quantize, false);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputSamplesError.Add(quantize[i] - InputSignal.Samples[i]);
            }

        }
    }
}
