public class ElevatorController
{
    private readonly Elevator _elevator;
    private Task? processingTask;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _running;
    //I need a data structure to hold the requests
    private PriorityQueue<int, int> _downRequests = new PriorityQueue<int, int>();
    private PriorityQueue<int, int> _upRequests = new PriorityQueue<int, int>();
    private readonly Lock _lock = new Lock();

    public ElevatorController(Elevator elevator)
    {
        _elevator = elevator;
    }

    public void Start()
    {
        if (_running) return;

        _cancellationTokenSource = new CancellationTokenSource();
        processingTask = Task.Run(() => StartProcessingRequests(_cancellationTokenSource.Token));
        _running = true;
    }

    public void Stop()
    {
        if (!_running) return;
        _cancellationTokenSource?.Cancel();
        processingTask?.Wait();
        _running = false;
    }

    public void AddRequest(ElevatorRequest request)
    {
        lock (_lock)
        {
            if (request.Direction == ElevatorDirection.Up)
            {
                _upRequests.Enqueue(request.DestinationFloor, request.DestinationFloor);
            }
            else if (request.Direction == ElevatorDirection.Down)
            {
                _downRequests.Enqueue(request.DestinationFloor, -request.DestinationFloor);
            }
        }
    }

    private async Task StartProcessingRequests(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            int? nextFloor = null;
            lock (_lock)
            {
                if (_upRequests.Count > 0 || _downRequests.Count > 0)
                {
                    nextFloor = GetNextFloor();
                }
            }

            if (nextFloor.HasValue)
            {
                await MoveToNextFloorAsync(nextFloor.Value);
            }
            else
            {
                _elevator.State = ElevatorState.Idle;
                _elevator.Direction = ElevatorDirection.None;
                await Task.Delay(1000);
            }
        }
    }

    private async Task MoveToNextFloorAsync(int floor)
    {
        if (_elevator.CurrentFloor < floor)
        {
            _elevator.State = ElevatorState.Moving;
            _elevator.Direction = ElevatorDirection.Up;

            // Simulate moving time
            await Task.Delay(Math.Abs(floor - _elevator.CurrentFloor) * 1000);
            _elevator.CurrentFloor = floor;
        }
        else
        {
            _elevator.State = ElevatorState.Moving;
            _elevator.Direction = ElevatorDirection.Down;

            // Simulate moving time
            await Task.Delay(Math.Abs(floor - _elevator.CurrentFloor) * 1000);
            _elevator.CurrentFloor = floor;
        }

        _elevator.DoorState = ElevatorDoorState.Open;
        // Simulate door open time
        await Task.Delay(2000);
        _elevator.DoorState = ElevatorDoorState.Closed;
    }

    private int GetNextFloor()
    {
        if (_elevator.Direction == ElevatorDirection.Up)
        {
            return _upRequests.Dequeue();
        }
        else if (_elevator.Direction == ElevatorDirection.Down)
        {
            return _downRequests.Dequeue();
        }

        // If the elevator is idle, check which queue has the next request
        if (_upRequests.Count > 0 && _downRequests.Count > 0)
        {
            return _upRequests.Peek() < _downRequests.Peek() ? _upRequests.Dequeue() : _downRequests.Dequeue();
        }
        else if (_upRequests.Count > 0)
        {
            return _upRequests.Dequeue();
        }
        else if (_downRequests.Count > 0)
        {
            return _downRequests.Dequeue();
        }

        return 0;
    }
    public Elevator GetElevator()
    {
        return _elevator;
    }
}