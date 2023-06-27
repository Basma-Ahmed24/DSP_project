using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {

            Signal InputSignal = LoadSignal(SignalPath);


            //step2
            FIR myObject = new FIR();
            myObject.InputTimeDomainSignal = InputSignal;
            myObject.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            myObject.InputFS = Fs;
            myObject.InputStopBandAttenuation = 50;
            myObject.InputTransitionBand = 500;
            myObject.InputF1 = miniF;
            myObject.InputF2 = maxF;
            myObject.Run();
            using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\FirSamplesPractice2.ds"))
            {
                w.WriteLine("0");
                w.WriteLine("0");
                w.WriteLine(406.ToString());
                for (int i = 0; i < 406; i++)
                {

                    w.WriteLine(i + " " + myObject.OutputYn.Samples[i].ToString());

                }
            }
            if (newFs >= (2 * maxF))
            {
                Sampling mysample = new Sampling();

                mysample.L = L;
                mysample.M = M;
                mysample.InputSignal = myObject.OutputYn;
                mysample.Run();
                using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\LMSamplesPractice2.ds"))
                {
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine(153.ToString());
                    for (int i = 0; i < 153; i++)
                    {

                        w.WriteLine(i + " " + mysample.OutputSignal.Samples[i].ToString());

                    }
                }
                DC_Component dc = new DC_Component();
                dc.InputSignal = mysample.OutputSignal;
                dc.Run();
                using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\DcSamplesPractice2.ds"))
                {
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine(153.ToString());
                    for (int i = 0; i < 153; i++)
                    {

                        w.WriteLine(i.ToString() + " " + dc.OutputSignal.Samples[i].ToString());

                    }
                }
                Normalizer mynormalizer = new Normalizer();

                mynormalizer.InputMaxRange = 1;
                mynormalizer.InputMinRange = -1;
                mynormalizer.InputSignal = dc.OutputSignal;
                mynormalizer.Run();
                using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\NormSamplesPractice2.ds"))
                {
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine(153.ToString());
                    for (int i = 0; i < 153; i++)
                    {

                        w.WriteLine(i.ToString() + " " + mynormalizer.OutputNormalizedSignal.Samples[i].ToString());

                    }
                }
               
                DiscreteFourierTransform myDFT = new DiscreteFourierTransform();

                myDFT.InputSamplingFrequency = Fs;
                myDFT.InputTimeDomainSignal = mynormalizer.OutputNormalizedSignal;
                myDFT.Run();
               
                OutputFreqDomainSignal = myDFT.OutputFreqDomainSignal;
                using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\outputSamplesPractice2.ds"))
                {
                    w.WriteLine("1");
                    w.WriteLine("0");
                    w.WriteLine(153.ToString());
                    for (int i = 0; i < 153; i++)
                    {

                        w.WriteLine(i.ToString() + " " + myDFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i].ToString() + " " + myDFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i].ToString());

                    }
                }
            }
            else
            {
                DC_Component dc = new DC_Component();
                dc.InputSignal = myObject.OutputYn;
                dc.Run(); 
                using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\DcSamplesPractice2.ds"))
                {
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine(dc.OutputSignal.Samples.Count().ToString());
                    for (int i = 0; i < dc.OutputSignal.Samples.Count(); i++)
                    {

                        w.WriteLine(dc.OutputSignal.SamplesIndices[i].ToString() + " " + dc.OutputSignal.Samples[i].ToString());

                    }
                }
                
                Normalizer mynormalizer = new Normalizer();

                mynormalizer.InputMaxRange = 1;
                mynormalizer.InputMinRange = -1;
                mynormalizer.InputSignal = dc.OutputSignal;

                mynormalizer.Run();
                using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\NormSamplesPractice2.ds"))
                {
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine(mynormalizer.OutputNormalizedSignal.Samples.Count().ToString());
                    for (int i = 0; i < mynormalizer.OutputNormalizedSignal.Samples.Count(); i++)
                    {

                        w.WriteLine(mynormalizer.OutputNormalizedSignal.SamplesIndices[i].ToString() + " " + mynormalizer.OutputNormalizedSignal.Samples[i].ToString());

                    }
                }
                DiscreteFourierTransform myDFT = new DiscreteFourierTransform();

                myDFT.InputSamplingFrequency = Fs;
                myDFT.InputTimeDomainSignal = mynormalizer.OutputNormalizedSignal;
                myDFT.Run();
                for (int i = 0; i < myDFT.OutputFreqDomainSignal.Frequencies.Count; i++)
                    myDFT.OutputFreqDomainSignal.Frequencies[i] = (float)Math.Round((double)myDFT.OutputFreqDomainSignal.Frequencies[i], 1);
                OutputFreqDomainSignal = myDFT.OutputFreqDomainSignal;
                 using (StreamWriter w = new StreamWriter("C:\\Users\\Basma Ahmed\\OneDrive\\سطح المكتب\\outputSamplesPractice2.ds"))
                {
                    w.WriteLine("1");
                    w.WriteLine("0");
                    w.WriteLine(myDFT.OutputFreqDomainSignal.Samples.Count().ToString());
                    for (int i = 0; i < myDFT.OutputFreqDomainSignal.Samples.Count(); i++)
                    {

                        w.WriteLine(myDFT.OutputFreqDomainSignal.SamplesIndices[i].ToString() + " " + myDFT.OutputFreqDomainSignal.Samples[i].ToString());

                    }
                }

            }

        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }


    
        }
    }
