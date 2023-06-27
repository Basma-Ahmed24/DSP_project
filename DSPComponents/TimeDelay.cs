using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            
            List<float> output1 = new List<float>();
            List<float> output2 = new List<float>();
            List<float> cobysignal1 = new List<float>();
            float sum1 = 0;
            float sum2 = 0;
            float input = 0;
            float result = 0;
            float temp = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                cobysignal1.Add(InputSignal1.Samples[i]);
            }
            if (InputSignal2 == null)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {

                    sum1 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
                    sum2 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
                }

                result = (float)((Math.Pow(sum1 * sum2, 0.5))) / InputSignal1.Samples.Count;

                if (InputSignal1.Periodic == true)
                {
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {

                        for (int j = 0; j < InputSignal1.Samples.Count; j++)
                        {
                            input += (InputSignal1.Samples[j] * cobysignal1[j]);
                        }
                        input = input / InputSignal1.Samples.Count;
                        output1.Add(input / result);
                        output2.Add(input);
                        input = 0;
                        temp = cobysignal1[0];
                        for (int j = 0; j < cobysignal1.Count - 1; j++)
                        {

                            cobysignal1[j] = cobysignal1[j + 1];

                        }
                        cobysignal1[cobysignal1.Count - 1] = temp;
                    }
                }
                else
                {
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {

                        for (int j = 0; j < InputSignal1.Samples.Count; j++)
                        {

                            sum1 += (InputSignal1.Samples[j] * InputSignal1.Samples[j]);
                            sum2 += (cobysignal1[j] * cobysignal1[j]);

                        }
                        result = (float)((Math.Pow(sum1 * sum2, 0.5))) / InputSignal1.Samples.Count;
                        for (int j = 0; j < InputSignal1.Samples.Count; j++)
                        {


                            input += (InputSignal1.Samples[j] * cobysignal1[j]);

                        }
                        input = input / InputSignal1.Samples.Count;
                        output1.Add(input / result);
                        output2.Add(input);
                        input = 0;
                        temp = InputSignal1.Samples[0];
                        temp = cobysignal1[0];
                        for (int j = 0; j < cobysignal1.Count - 1; j++)
                        {

                            cobysignal1[j] = cobysignal1[j + 1];

                        }
                        cobysignal1[cobysignal1.Count - 1] = 0;
                    }
                }
            }
            else
            {
                if (InputSignal2.Periodic == true)
                {
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sum1 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
                        sum2 += (InputSignal2.Samples[i] * InputSignal2.Samples[i]);
                    }
                    result = (float)((Math.Pow(sum1 * sum2, 0.5))) / InputSignal1.Samples.Count;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {

                        for (int j = 0; j < InputSignal2.Samples.Count; j++)
                        {
                            input += (InputSignal1.Samples[j] * InputSignal2.Samples[j]);
                        }
                        input = input / InputSignal1.Samples.Count;
                        output1.Add(input / result);
                        output2.Add(input);
                        input = 0;
                        temp = InputSignal2.Samples[0];
                        for (int j = 0; j < InputSignal2.Samples.Count - 1; j++)
                        {

                            InputSignal2.Samples[j] = InputSignal2.Samples[j + 1];

                        }
                        InputSignal2.Samples[InputSignal2.Samples.Count - 1] = temp;
                    }
                }
                else
                {
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        for (int j = 0; j < InputSignal1.Samples.Count; j++)
                        {
                            sum1 += (InputSignal1.Samples[j] * InputSignal1.Samples[j]);
                            sum2 += (InputSignal2.Samples[j] * InputSignal2.Samples[j]);
                        }
                        result = (float)((Math.Pow(sum1 * sum2, 0.5))) / InputSignal1.Samples.Count;
                        for (int j = 0; j < InputSignal2.Samples.Count; j++)
                        {


                            input += (InputSignal1.Samples[j] * InputSignal2.Samples[j]);

                        }
                        input = input / InputSignal1.Samples.Count;
                        output1.Add(input / result);
                        output2.Add(input);
                        input = 0;
                        temp = InputSignal2.Samples[0];
                        for (int j = 0; j < InputSignal1.Samples.Count - 1; j++)
                        {
                            InputSignal2.Samples[j] = InputSignal2.Samples[j + 1];

                        }
                        InputSignal2.Samples[InputSignal2.Samples.Count - 1] = 0;
                    }
                }
            }


            float maximum = output2.Max();
            int index=0;
        for(int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                if (InputSignal1.Samples[i] == maximum)
                {
                    index = i;
                }
            }
            OutputTimeDelay = index * InputSamplingPeriod;


    }
    }
}
