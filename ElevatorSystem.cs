public class ElevatorSystem
{
    private readonly IList<ElevatorController> _elevatorControllers;

    public ElevatorSystem(IList<ElevatorController> elevatorsControllers)
    {
        _elevatorControllers = elevatorsControllers;
    }

    public void AddRequest(ElevatorRequest request)
    {
        var bestElevator = GetBestElevator(request);
        if (bestElevator != null)
        {
            bestElevator.AddRequest(request);
        }
        else
        {
            Console.WriteLine("No available elevator to handle the request.");
        }
    }

    private ElevatorController? GetBestElevator(ElevatorRequest request)
    {
        ElevatorController? bestElevator = null;
        int minDistance = int.MaxValue;

        foreach (var controller in _elevatorControllers)
        {
            var elevator = controller.GetElevator();
            int distance = Math.Abs(elevator.CurrentFloor - request.DestinationFloo);

            bool directionMatches = elevator.Direction == ElevatorDirection.None ||
                                    elevator.Direction == request.Direction;

            if (directionMatches || elevator.State == ElevatorState.Idle)
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestElevator = controller;
                }
            }
        }

        return bestElevator;
    }
}