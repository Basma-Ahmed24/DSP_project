using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }
        
        public override void Run()
        {
            List<float> magnitudes = new List<float>();
            List<float> phases = new List<float>();
            List<float> outputes = new List<float>();
            float A1, PH1,A2,PH2;
            Complex sig1,sig2;
            Complex complex = new Complex();
            int end = InputSignal1.Samples.Count + InputSignal2.Samples.Count-1;
            for (int i = InputSignal1.Samples.Count; i < end; i++)
            {
                InputSignal1.Samples.Add(0);

            }
            for (int i = InputSignal2.Samples.Count; i < end; i++)
            {
                InputSignal2.Samples.Add(0);

            } 
            DiscreteFourierTransform ds1 = new DiscreteFourierTransform();
            ds1.InputTimeDomainSignal = InputSignal1;
            ds1.Run();
            InputSignal1 = ds1.OutputFreqDomainSignal;
            ds1.InputTimeDomainSignal = InputSignal2;
            ds1.Run();
            InputSignal2 = ds1.OutputFreqDomainSignal;
            for (int i = 0; i < end; i++)
            {
                A1 = (InputSignal1.FrequenciesAmplitudes[i] * (float)Math.Cos(InputSignal1.FrequenciesPhaseShifts[i]));
                PH1 = (InputSignal1.FrequenciesAmplitudes[i] * (float)Math.Sin(InputSignal1.FrequenciesPhaseShifts[i]));
                sig1 = new Complex(A1, PH1);
                A2 = (InputSignal2.FrequenciesAmplitudes[i] * (float)Math.Cos(InputSignal2.FrequenciesPhaseShifts[i]));
                PH2 = (InputSignal2.FrequenciesAmplitudes[i] * (float)Math.Sin(InputSignal2.FrequenciesPhaseShifts[i]));
                sig2 = new Complex(A2, PH2);
                complex = Complex.Multiply(sig1, sig2);
                magnitudes.Add((float)complex.Magnitude);
                phases.Add((float)Math.Atan2(complex.Imaginary, complex.Real));

            }

            InverseDiscreteFourierTransform id = new InverseDiscreteFourierTransform();
            id.InputFreqDomainSignal = new Signal(false, outputes, magnitudes, phases);
            id.Run();
            OutputConvolvedSignal = new Signal(id.OutputTimeDomainSignal.Samples, false);

        }
    }
}