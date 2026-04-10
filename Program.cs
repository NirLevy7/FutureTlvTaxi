using System;
using System.Threading;

namespace FutureTlvTaxi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting FutureTLV Autonomous Taxi Service Simulation...\n");

            // יצירת אובייקט מנהל הסימולציה
            SimulationManager simulation = new SimulationManager();

            // הדפסת המיקומים ההתחלתיים של המוניות
            simulation.PrintInitialState();

            int elapsedSeconds = 0;

            // לולאה שמריצה את הסימולציה. הגדרתי 15 פעימות (5 דקות זמן סימולציה) לצורך ההדגמה
            for (int i = 0; i < 15; i++)
            {
                elapsedSeconds += 20; // כל פעימה מייצגת 20 שניות

                // הרצת פעימה של המערכת, אשר מעדכנת את התור ואת המוניות
                simulation.Tick(elapsedSeconds);

                // השהיה של שניה וחצי במציאות, כדי שתוכל לראות את הפלט מתעדכן בזמן אמת ולא רץ בבת אחת
                Thread.Sleep(1500);
            }

            Console.WriteLine("\nSimulation ended.");
        }
    }
}