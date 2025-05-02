public class ElevatorRequest
{
    public int DestinationFloor { get; set; }
    public ElevatorDirection Direction { get; set; }
    public ElevatorRequest(int destinationFloor, ElevatorDirection direction)
    {
        DestinationFloor = destinationFloor;
        Direction = direction;
    }
}