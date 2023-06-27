using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
 
namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }
 
        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            OutputYn = new Signal(new List<float>(), new List<int>(), false);
            float filtertype = 0;
            float deltaF = InputTransitionBand / InputFS;
            int N;
            float temp;
           // int n;
            float value;
 
            if (InputStopBandAttenuation <= 21)
            {
                value = 0.9f;
            }
            else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
            {
                value = 3.1f;
            }
            else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
            {
                value = 3.3f;
            }
            else
            {
                value = 5.5f;
            }
            float calc = value / deltaF;
            N = (int)calcn(calc);
            for (int i = 0, n = (int)-N / 2; i < N; i++, n++)
            {
                OutputHn.SamplesIndices.Add(n);
            }
            
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                float fclow = (float)(InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;

                for (int i = 0; i <N; i++)
                {
                    int idx = Math.Abs(OutputHn.SamplesIndices[i]);
                    float hLowRes,windres;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hLowRes = 2 * fclow;
 
                    }
                    else
                    {
                        hLowRes = (float)(2 * fclow * ((Math.Sin((idx * 2 * Math.PI * fclow))) / (idx * 2 * Math.PI * fclow)));
                    }
                    windres = newwindow(idx, N);
                    OutputHn.Samples.Add(hLowRes * windres);
                }
 
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {

                float fchigh = (float)(InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;
                for (int i = 0; i < N; i++)
                {
                    int idx = Math.Abs(OutputHn.SamplesIndices[i]);
                    float hHIGHRes, windres;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hHIGHRes = 1 - (2 * fchigh);
 
                    }
                    else
                    {
                        hHIGHRes = (float)(-2 * fchigh * ((Math.Sin((idx * 2 * Math.PI * fchigh))) / (idx * 2 * Math.PI * fchigh)));
 
                    }
                    windres = newwindow(idx, N);
                    OutputHn.Samples.Add(hHIGHRes * windres);
                }
 
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
 
 
                for (int i = 0; i < N; i++)
                {
                    int idx = Math.Abs(OutputHn.SamplesIndices[i]);
                    float hPassRes, windres;
                    float fcBandPass1 = (float)(InputF1 - (InputTransitionBand / 2)) / InputFS;
                    float fcBandPass2 = (float)(InputF2 + (InputTransitionBand / 2)) / InputFS;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hPassRes = (float)(2 * (fcBandPass2 - fcBandPass1));
 
                    }
                    else
                    {
                        float w2 = (float)( fcBandPass2 * (Math.Sin(idx * 2 * Math.PI * fcBandPass2) /
                           (idx * 2 * Math.PI * fcBandPass2)));
                        float w1 = (float)( fcBandPass1 * (Math.Sin((idx * 2 * Math.PI * fcBandPass1)) 
                            / (idx * 2 * Math.PI * fcBandPass1)));
                        hPassRes = (float)2*(w2 - w1); 
 
                    }
                    windres = newwindow(idx, N);
                    OutputHn.Samples.Add(hPassRes * windres);
                }
 
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
 
 
                for (int i = 0; i < N; i++)
                {
                    int idx = Math.Abs(OutputHn.SamplesIndices[i]);
                    float hStopRes, windres;
                    float fcBandStop1 = (float)(InputF1 + (InputTransitionBand / 2)) / InputFS;
                    float fcBandStop2 = (float)(InputF2 - (InputTransitionBand / 2)) / InputFS;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hStopRes = (float)(1 - (2 * (fcBandStop2 - fcBandStop1)));
 
                    }
                    else
                    {
                        hStopRes = (float)(2 * fcBandStop1 * (Math.Sin(idx * 2 * Math.PI * fcBandStop1) /
                           (idx * 2 * Math.PI * fcBandStop1))) - (float)(2 * fcBandStop2 * (Math.Sin((idx * 2 * Math.PI * fcBandStop2)) / (idx * 2 * Math.PI * fcBandStop2)));
 
                    }
                    windres = newwindow(idx, N);
                    OutputHn.Samples.Add(hStopRes * windres);
                }
 
            }
            DirectConvolution direct = new DirectConvolution();
            direct.InputSignal1 = InputTimeDomainSignal;
            direct.InputSignal2 = OutputHn;
            direct.Run();
            OutputYn = direct.OutputConvolvedSignal;
 
        }
 
 
 
        int calcn(float n)
        {
            int res = 0;
            if ((int)n % 2 == 0)
                res = (int)n + 1;
            else
            {
                int f = (int)Math.Floor(n);
                int c = (int)Math.Ceiling(n);
                if (f == c)
                    res = (int)n;
                else
                    res = (int)n + 2;
            }
 
            return res;
        }
 
 
 
 
        float newwindow(int idx, int N)
        {
            float result = 0;
            if (InputStopBandAttenuation <= 21)
            {
                result= 1;
            }
            else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
            {
 
                result = (float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * idx) / N)); ;
            }
            else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
            {
                result = (float)(0.54 +(float) 0.46 * Math.Cos((2 * Math.PI * idx) / N));
            }
            else
            {
                float num1 = (float)(0.5 * Math.Cos((2 * Math.PI * idx) / (N - 1)));
                float num2 = (float)(0.08 * Math.Cos((4 * Math.PI * idx) / (N - 1)));
                result = (float)(0.42 + num1 + num2);
            }
            return result;
        }
 
 
    }
}