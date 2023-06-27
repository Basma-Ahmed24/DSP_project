using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }



        List<float> amplitudes = new List<float>();
        List<float> phaseShift = new List<float>();
        List<float> samples = new List<float>();
        public override void Run()
        {

           // int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count; // Number of spectrum elements
            amplitudes = new List<float>(InputFreqDomainSignal.FrequenciesAmplitudes);
            phaseShift = new List<float>(InputFreqDomainSignal.FrequenciesPhaseShifts);
            for (int n = 0; n < InputFreqDomainSignal.FrequenciesAmplitudes.Count; n++)
            {
                Complex sumtion = 0;


                for (int k = 0; k < InputFreqDomainSignal.FrequenciesAmplitudes.Count; k++)
                {
                    Complex a = Complex.FromPolarCoordinates(amplitudes[k], phaseShift[k]);
                    sumtion+= a * Complex.Exp(Complex.ImaginaryOne * 2 * Math.PI * n * k / InputFreqDomainSignal.FrequenciesAmplitudes.Count);

                }
                sumtion = sumtion / InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                samples.Add((float)sumtion.Real);



            }


            OutputTimeDomainSignal = new Signal(samples, false);
        }
    }
}
