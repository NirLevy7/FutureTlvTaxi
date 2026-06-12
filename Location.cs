using Microsoft.VisualBasic;

namespace FutureTlvTaxi
{
    public class Location
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Location(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}