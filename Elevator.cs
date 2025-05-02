public class Elevator
{
    public int Id { get; private set; }
    public int CurrentFloor { get; set; }
    public ElevatorState State { get; set; }
    public ElevatorDirection Direction { get; set; }
    public ElevatorDoorState DoorState { get; set; }

    public Elevator(int id, int initialFloor)
    {
        Id = id;
        CurrentFloor = initialFloor;
        State = ElevatorState.Idle;
        Direction = ElevatorDirection.None;
        DoorState = ElevatorDoorState.Closed;
    }
}