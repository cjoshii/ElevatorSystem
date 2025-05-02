var elevator1 = new ElevatorController(new Elevator(1, 0));
var elevator2 = new ElevatorController(new Elevator(2, 0));
var elevator3 = new ElevatorController(new Elevator(3, 0));

elevator1.Start();
elevator2.Start();
elevator3.Start();

var elevatorControllers = new List<ElevatorController>
{
    elevator1,
    elevator2,
    elevator3
};
var system = new ElevatorSystem(elevatorControllers);

//Call to floor 5
system.AddRequest(new ElevatorRequest(5, ElevatorDirection.Up));

//Call to floor 3   
system.AddRequest(new ElevatorRequest(3, ElevatorDirection.Up));

//Now in the elevator 1 press floor 0
elevator1.AddRequest(new ElevatorRequest(0, ElevatorDirection.Down));