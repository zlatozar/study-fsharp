## Learned from this challenge

- how to implement ```state machine``` in F#
- pattern matching with records
- ```private``` usage
- records constructor exist

## Learned during development

I didn't know that this is possible

```fsharp

let putOnTruck package = 
    {package with PackageStatus=OutForDelivery}

let signedFor package signature = 
    let {PackageStatus=packageStatus} = package 
    if (packageStatus = Undelivered) 
    then 
        failwith "package not out for delivery"
    else if (packageStatus = OutForDelivery) 
    then 
        {package with 
            PackageStatus=OutForDelivery;
            DeliveryDate = DateTime.UtcNow;
            DeliverySignature=signature;
            }
    else
        failwith "package already delivered"

```

## Challenge notes

- To use ```state machine``` that is the clue

## Elegant solution from EXERCISM community

```fsharp
module RobotSimulator

type Bearing = North | South | East | West

type Coord = Coord of int * int

type Robot = Robot of Bearing * Coord

let private goLeft = function
    | North -> West
    | East -> North
    | South -> East
    | West -> South

let private goRight = function
    | North -> East
    | East -> South
    | South -> West
    | West -> North

let private advance (x, y) = function
    | North -> Coord (x, y + 1)
    | East -> Coord (x + 1, y)
    | South -> Coord (x, y - 1)
    | West -> Coord (x - 1, y)

let createRobot bearing coord = Robot (bearing, Coord coord)

let turnLeft (Robot (dir, c)) = Robot (goLeft dir, c)

let turnRight (Robot (dir, c)) = Robot (goRight dir, c)

let goForward (Robot (dir, Coord (x,y))) = Robot (dir, advance (x,y) dir)

let moveRobot r = function
    | 'R' -> turnRight r
    | 'L' -> turnLeft r
    | 'A' -> goForward r
    | _ -> failwith "Invalid command"

let simulate robot path = Seq.fold moveRobot robot path

```