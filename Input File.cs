using System;
using System.Linq;
using System.Collections.Generic;

namespace Simulation1
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] TimesBetweenArrival = new double[9] { 0.4, 1.2, 0.5, 1.7, 0.2, 1.6, 0.2, 1.4, 1.9 };
            double[] ServiceTimes = new double[6] { 2.0, 0.7, 0.2, 1.1, 3.7, 0.6 };

            double Clock = 0;
            List<double> EventList = new List<double>();
            EventList.Add(TimesBetweenArrival[0]);
            EventList.Add(Double.MaxValue);

            //System State:
            int ServerStatus = 0;
            int NumberInQueue = 0;
            List<double> TimesOfArrival = new List<double>();
            double TimeOfLastEvent = 0;

            //Statistical Counters:
            int NumberServiced = 0;
            double TotalDelay = 0;
            double AreaUnderQt = 0;
            double AreaUnderBt = 0;

            int i = -1;
            int j = -1;
            double k = 0;
            TimeOfLastEvent = TimesBetweenArrival[0];
            while (NumberServiced < 6)
            {
                Clock = EventList.Min();
                if (Clock == EventList[0] & EventList[1] == Double.MaxValue)
                {
                    i++;
                    j++;
                    EventList[0] = Clock + TimesBetweenArrival[i + 1];
                    EventList[1] = Clock + ServiceTimes[j];
                    ServerStatus = 1;
                    k = k + (Clock - TimeOfLastEvent);
                    TimeOfLastEvent = Clock;
                    NumberServiced++;
                }
                else if (Clock == EventList[0])
                {
                    i++;
                    EventList[0] = Clock + TimesBetweenArrival[i + 1];
                    NumberInQueue++;
                    TimesOfArrival.Add(Clock);
                    TimeOfLastEvent = Clock;
                    AreaUnderQt = AreaUnderQt + (NumberInQueue * (EventList.Min() - Clock));
                    AreaUnderBt = Clock - TimesBetweenArrival[0];
                }
                else if (Clock == EventList[1] && NumberInQueue > 0)
                {
                    j++;
                    EventList[1] = Clock + ServiceTimes[j];
                    NumberInQueue--;
                    TotalDelay = TotalDelay + (Clock - TimesOfArrival[0]);
                    TimesOfArrival.RemoveAt(0);
                    TimeOfLastEvent = Clock;
                    NumberServiced++;
                    AreaUnderQt = AreaUnderQt + (NumberInQueue * (EventList.Min() - Clock));
                    AreaUnderBt = Clock - TimesBetweenArrival[0];
                }
                else if (Clock == EventList[1] && NumberInQueue == 0)
                {
                    EventList[1] = Double.MaxValue;
                    ServerStatus = 0;
                    TimeOfLastEvent = Clock;
                    AreaUnderBt = Clock - TimesBetweenArrival[0];
                }
	else
	{
                    
	}
                AreaUnderBt = AreaUnderBt - k;
            }
            double Wq = TotalDelay / NumberServiced;
            double Lq = AreaUnderQt / Clock;
            double p = AreaUnderBt / Clock;
            double L = Lq + p;
            double Es = 0;
            for (int n = 0; n < ServiceTimes.Length; n++)
            {
                Es = Es + ServiceTimes[n];
            }
            Es = Es / ServiceTimes.Length;
            double W = Wq + Es;
            Console.WriteLine("Wq: {0} \nLq: {1}\np: {2}\nL: {3}\nEs: {4}\nW: {5}", Wq, Lq, p, L, Es, W);
            Console.ReadLine();
        }
    }
}