namespace FutureTlvTaxi
{
    public class RideRequest
    {
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }

        public RideRequest(Location start, Location end)
        {
            StartLocation = start;
            EndLocation = end;
        }
    }
}