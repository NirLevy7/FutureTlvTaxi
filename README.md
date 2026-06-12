# FutureTLV Autonomous Taxi Service Simulation

A C# console simulation of an autonomous taxi dispatch system operating across a 20×20 km city grid, inspired by a future Tel Aviv.

## Overview

The simulation models a fleet of 10 autonomous taxis that receive ride requests, navigate to passengers, and drop them off — all in real time. Each simulation "tick" represents 20 seconds, and the console updates every 1.5 seconds so you can watch the fleet evolve.

## How It Works

- **Fleet**: 10 taxis are spawned at random positions on a 20×20 km grid.
- **Ride requests**: A new request is generated every tick, with a random pickup point and a destination within 2 km (Manhattan distance).
- **Dispatch**: The nearest available (standing) taxi is assigned to each request using the Manhattan distance metric.
- **Movement**: Taxis travel at 20 m/s (0.4 km per tick) along grid lines — X-axis first, then Y-axis.
- **Phases**: Each taxi first drives to the passenger's pickup location, then continues to the drop-off point. After completing a ride it becomes available again.
- **Queue**: If all taxis are busy, pending requests wait in a queue and are retried each tick.

The simulation runs for **15 ticks** (5 minutes of simulated time).

## Project Structure

```
FutureTlvTaxi/
├── Program.cs           # Entry point — runs the simulation loop
├── SimulationManager.cs # Orchestrates ticks, ride generation, and dispatch
├── Taxi.cs              # Taxi entity with movement and state logic
├── RideRequest.cs       # Ride data (start & end location)
└── Location.cs          # 2D coordinate (X, Y in km)
```

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download) or later

### Run

```bash
dotnet run
```

### Sample Output

```
Starting FutureTLV Autonomous Taxi Service Simulation...

Initial taxi locations:
Taxi-1: 3.4Km, 12.7Km (standing)
Taxi-2: 18.1Km, 5.0Km (standing)
...

After 20 seconds:
Order Queue: Empty
Taxi locations:
Taxi-1: 3.8Km, 12.7Km (driving)
Taxi-2: 18.1Km, 5.0Km (standing)
...
```

## Key Design Decisions

| Decision | Rationale |
|---|---|
| Manhattan distance routing | Taxis travel on a street grid, not as the crow flies |
| Queue with retry | Requests are not dropped; they wait until a taxi is free |
| X-axis movement before Y-axis | Consistent, deterministic path planning |
| Fixed tick size (20 s) | Simple discrete-time model, easy to reason about |

## License

MIT
