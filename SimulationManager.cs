using System;
using System.Collections.Generic;
using System.Linq;

namespace FutureTlvTaxi
{
    public class SimulationManager
    {
        private List<Taxi> _taxis; // רשימת 10 המוניות 
        private Queue<RideRequest> _orderQueue; // תור ההזמנות הממתינות
        private Random _random;

        public SimulationManager()
        {
            _taxis = new List<Taxi>();
            _orderQueue = new Queue<RideRequest>();
            _random = new Random();
            InitializeTaxis();
        }

        // אתחול המוניות במיקומים אקראיים
        private void InitializeTaxis()
        {
            for (int i = 1; i <= 10; i++)
            {
                // 20X20 הגרלת מיקום על גריד
                double x = Math.Round(_random.NextDouble() * 20.0, 1);
                double y = Math.Round(_random.NextDouble() * 20.0, 1);
                _taxis.Add(new Taxi($"Taxi-{i}", new Location(x, y)));
            }
        }

        // פונקציה להדפסת המצב ההתחלתי של המערכת 
        public void PrintInitialState()
        {
            Console.WriteLine("Initial taxi locations:");
            foreach (var taxi in _taxis)
            {
                Console.WriteLine($"{taxi.Id}: {taxi.CurrentLocation.X:F1}Km, {taxi.CurrentLocation.Y:F1}Km ({taxi.State.ToString().ToLower()})");
            }
        }

        // הפעימה של המערכת - מתרחשת כל 20 שניות
        public void Tick(int elapsedSeconds)
        {
            GenerateNewRideRequest(); // הוספת הזמנה חדשה 

            // הזזת כל המוניות שנמצאות בתנועה 
            foreach (var taxi in _taxis)
            {
                taxi.Move();
            }

            AllocateRides(); // ניסיון להקצות מוניות להזמנות 
            PrintStatus(elapsedSeconds); // הדפסת הסטטוס 
        }

        private void GenerateNewRideRequest()
        {
            double startX = Math.Round(_random.NextDouble() * 20.0, 1);
            double startY = Math.Round(_random.NextDouble() * 20.0, 1);
            Location start = new Location(startX, startY);

            Location end;
            // יצירת יעד שאינו רחוק יותר מ-2 קילומטר ואינו זהה לנקודת ההתחלה
            do
            {
                double endX = Math.Round(_random.NextDouble() * 20.0, 1);
                double endY = Math.Round(_random.NextDouble() * 20.0, 1);
                end = new Location(endX, endY);
            } while (CalculateManhattanDistance(start, end) > 2.0 || CalculateManhattanDistance(start, end) == 0);

            _orderQueue.Enqueue(new RideRequest(start, end));
        }

        private void AllocateRides()
        {
            int currentQueueSize = _orderQueue.Count;

            // מעבר על התור הקיים. חייבים לשמור את הגודל ההתחלתי כדי לא ליצור לולאה אינסופית
            for (int i = 0; i < currentQueueSize; i++)
            {
                var request = _orderQueue.Dequeue(); // הוצאה מהתור

                // שליפת כל המוניות הפנויות
                var availableTaxis = _taxis.Where(t => t.State == TaxiState.Standing).ToList();

                if (availableTaxis.Any())
                {
                    // שימוש ב-LINQ כדי למצוא את המונית עם מרחק המנהטן הקטן ביותר לנקודת ההתחלה 
                    var nearestTaxi = availableTaxis.OrderBy(t => CalculateManhattanDistance(t.CurrentLocation, request.StartLocation)).First();
                    nearestTaxi.AssignRide(request);
                }
                else
                {
                    // אם כל המוניות תפוסות, מחזירים את ההזמנה לסוף התור 
                    _orderQueue.Enqueue(request);
                }
            }
        }

        // חישוב מרחק מנהטן - קריטי כי המוניות נוסעות רק בשתי וערב 
        private double CalculateManhattanDistance(Location a, Location b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }


        // הדפסת סטטוס המערכת לאחר כל פעימה
        private void PrintStatus(int elapsedSeconds)
        {
            Console.WriteLine($"\nAfter {elapsedSeconds} seconds:");
            Console.WriteLine("Order Queue:");
            if (_orderQueue.Count == 0)
            {
                Console.WriteLine("Empty");
            }
            else
            {
                Console.WriteLine($"{_orderQueue.Count} waiting orders");
            }

            Console.WriteLine("Taxi locations:");
            foreach (var taxi in _taxis)
            {
                Console.WriteLine($"{taxi.Id}: {taxi.CurrentLocation.X:F1}Km, {taxi.CurrentLocation.Y:F1}Km ({taxi.State.ToString().ToLower()})");
            }
        }
    }
}