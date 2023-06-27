using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public override void Run()
        {
            List<float> downs = new List<float>();
            List<float> upsampling= new List<float>();
            Signal result1 = new Signal(new List<float>(), false); ;
            Signal result2 = new Signal(new List<float>(), false); ;
            OutputSignal = new Signal(new List<float>(), false);
            if (L != 0 && M == 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    upsampling.Add(InputSignal.Samples[i]);
                    for (int k = 0; k < L - 1; k++)
                    {
                        upsampling.Add(0);
                    }
                }

                InputSignal = new Signal(upsampling, false);
                result1 = low_filter(InputSignal);
                List<float> newupsamplinf = new List<float>();
                for (int i = 0; i < result1.Samples.Count; i++)
                {
                    newupsamplinf.Add(result1.Samples[i]);
                }
                for (int i = 0; i < newupsamplinf.Count; i++)
                {
                    newupsamplinf.Remove(0);
                }
                OutputSignal = new Signal(newupsamplinf, false);
            }
            else if (M != 0 && L == 0)
            {
                result1 = low_filter(InputSignal);
                result2 = down_sample(result1);
                OutputSignal = result2;
            }
            else if (L != 0 && M != 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    upsampling.Add(InputSignal.Samples[i]);
                    for (int k = 0; k < L - 1; k++)
                    {
                        upsampling.Add(0);
                    } 
                }
                InputSignal = new Signal(upsampling, false);
                
                result1= low_filter(InputSignal);

                for (int i = 0; i < result1.Samples.Count; i += M)
                {
                    downs.Add(result1.Samples[i]);

                }
                downs.Remove(0);
                OutputSignal = new Signal(downs, false);
            }
            else if (L == 0 && M == 0)
            {
                Console.WriteLine("error message");
            }
        }
        Signal low_filter(Signal lowSignal)
        {
            FIR mySamplingObj = new FIR();
            mySamplingObj.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            mySamplingObj.InputFS = 8000;
            mySamplingObj.InputStopBandAttenuation = 50;
            mySamplingObj.InputCutOffFrequency = 1500;
            mySamplingObj.InputTransitionBand = 500;
            mySamplingObj.InputTimeDomainSignal = lowSignal;
            mySamplingObj.Run();
            return mySamplingObj.OutputYn;
        }
        Signal down_sample(Signal downSignal)
        {
            Signal temp = new Signal(new List<float>(), false); 
            for (int i = 0; i < downSignal.Samples.Count; i += M)
            {
                temp.Samples.Add(downSignal.Samples[i]);
            }
            return temp;
        }
    }

    }