using System;

namespace FutureTlvTaxi
{
    // הגדרת מצבי המונית - עומדת או נוסעת
    public enum TaxiState
    {
        Standing,
        Driving
    }

    public class Taxi
    {
        public string Id { get; private set; }
        public Location CurrentLocation { get; set; }
        public TaxiState State { get; private set; }
        public RideRequest? CurrentRide { get; private set; }

        // משתנה עזר שיעזור לנו לדעת אם המונית בדרך לאיסוף או כבר בדרך ליעד
        public bool IsPickingUpPassenger { get; private set; }

        // קבוע המייצג את המרחק בקילומטרים שמונית עוברת ב-20 שניות (20 מטר/שנייה * 20 שניות = 400 מטר = 0.4 ק"מ)
        private const double DistancePerTickKm = 0.4;

        public Taxi(string id, Location startLocation)
        {
            Id = id;
            CurrentLocation = startLocation;
            State = TaxiState.Standing;
            CurrentRide = null;
        }

        // פונקציה להקצאת נסיעה למונית
        public void AssignRide(RideRequest ride)
        {
            CurrentRide = ride;
            State = TaxiState.Driving;
            IsPickingUpPassenger = true;
        }

        // פונקציית התנועה שתופעל כל 20 שניות
        public void Move()
        {
            // אם המונית עומדת או שאין לה נסיעה, לא עושים כלום
            if (State == TaxiState.Standing || CurrentRide == null) return;

            double remainingDistance = DistancePerTickKm;

            // לולאה שרצה כל עוד יש למונית (מרחק) לעבור באותה פעימה
            while (remainingDistance > 0 && CurrentRide != null)
            {
                // קביעת היעד הנוכחי: נקודת האיסוף או נקודת הסיום
                Location destination = IsPickingUpPassenger ? CurrentRide.StartLocation : CurrentRide.EndLocation;

                // תנועה של שתי וערב, תחילה בציר X
                if (Math.Round(CurrentLocation.X, 3) != Math.Round(destination.X, 3))
                {
                    double diff = destination.X - CurrentLocation.X;
                    double moveX = Math.Min(Math.Abs(diff), remainingDistance);
                    CurrentLocation.X += Math.Sign(diff) * moveX;
                    remainingDistance -= moveX;
                }
                // התאמה לתנועה שביצענו בציר X, כעת תנועה על ציר Y
                else if (Math.Round(CurrentLocation.Y, 3) != Math.Round(destination.Y, 3))
                {
                    double diff = destination.Y - CurrentLocation.Y;
                    double moveY = Math.Min(Math.Abs(diff), remainingDistance);
                    CurrentLocation.Y += Math.Sign(diff) * moveY;
                    remainingDistance -= moveY;
                }

                // בדיקה אם הגענו ליעד הנוכחי (איסוף או סיום)
                if (Math.Round(CurrentLocation.X, 3) == Math.Round(destination.X, 3) &&
                    Math.Round(CurrentLocation.Y, 3) == Math.Round(destination.Y, 3))
                {
                    if (IsPickingUpPassenger)
                    {
                        // הגענו לנוסע! נשנה סטטוס, ואם נשאר עודף מרחק ב-20 השניות הללו, הלולאה תמשיך ליעד הסופי
                        IsPickingUpPassenger = false;
                    }
                    else
                    {
                        // הגענו ליעד הסופי!
                        State = TaxiState.Standing;
                        CurrentRide = null;
                        break; // יציאה מהלולאה, המונית סיימה את חלקה בפעימה הזו
                    }
                }
            }
        }
    }
}