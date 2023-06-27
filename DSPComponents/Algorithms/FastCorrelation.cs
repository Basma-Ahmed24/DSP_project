using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        public Signal newinputSignal1 { get; set; }
        public Signal newnputSignal2 { get; set; }
       
        public override void Run()
        {
            List<float> magnitudes = new List<float>();
            List<float> phases = new List<float>();
            List<float> outputes = new List<float>();
            List<float> output1 = new List<float>();
            List<float> norm_output = new List<float>();

            float A1;
            float p1;
            float A2;
            float p2;
            float output;
            float normOutput;
            float sum1 = 0;
            float sum2 = 0;
            float res = 1;

            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;
            }

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sum2 += (float)Math.Pow(InputSignal2.Samples[i], 2);

            }
            res = (float)(Math.Sqrt((sum1 * sum2)) / InputSignal1.Samples.Count);

            DiscreteFourierTransform dst1 = new DiscreteFourierTransform();
            dst1.InputTimeDomainSignal = InputSignal1;
            dst1.Run();
            newinputSignal1 = dst1.OutputFreqDomainSignal;
            Complex sig1;

            dst1.InputTimeDomainSignal = InputSignal2;
            dst1.Run();
            newnputSignal2 = dst1.OutputFreqDomainSignal;
            Complex sig2;

            Complex com1 = new Complex();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                A1 = (newinputSignal1.FrequenciesAmplitudes[i] * (float)Math.Cos(newinputSignal1.FrequenciesPhaseShifts[i]));
                p1 = (-1) * (newinputSignal1.FrequenciesAmplitudes[i] * (float)Math.Sin(newinputSignal1.FrequenciesPhaseShifts[i]));
               
                sig1 = new Complex(A1, p1);

                A2 = (newnputSignal2.FrequenciesAmplitudes[i] * (float)Math.Cos(newnputSignal2.FrequenciesPhaseShifts[i]));
                p2 = (newnputSignal2.FrequenciesAmplitudes[i] * (float)Math.Sin(newnputSignal2.FrequenciesPhaseShifts[i]));
              
                sig2 = new Complex(A2, p2);

                com1 = Complex.Multiply(sig1, sig2);
                magnitudes.Add((float)com1.Magnitude);
                phases.Add((float)Math.Atan2(com1.Imaginary, com1.Real));

            }

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = new Signal(false, outputes, magnitudes, phases);
            IDFT.Run();

            for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count; i++)
            {
                output = (IDFT.OutputTimeDomainSignal.Samples[i] / InputSignal1.Samples.Count);
                output1.Add(output);

            }
            OutputNonNormalizedCorrelation = output1;

            for (int i = 0; i < output1.Count; i++)
            {
                normOutput = output1[i] / res;
                norm_output.Add(normOutput);
            }
            OutputNormalizedCorrelation = norm_output;

        }
    }
}
