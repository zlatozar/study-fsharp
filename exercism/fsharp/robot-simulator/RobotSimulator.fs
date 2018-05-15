module RobotSimulator

type Direction = North | East | South | West

type Position = int * int

type Robot = { direction: Direction; position: Position }

let create direction position = 
    {direction=direction; position=position}

let turnLeft robot = 
    match robot with 
    | {direction=North; position=p} -> {direction=West; position=p}
    | {direction=West; position=p}  -> {direction=South; position=p}
    | {direction=South; position=p} -> {direction=East; position=p}
    | {direction=East; position=p}  -> {direction=North; position=p}
    
let turnRight robot = 
    match robot with 
    | {direction=North; position=p} -> {direction=East; position=p}
    | {direction=West; position=p}  -> {direction=North; position=p}
    | {direction=South; position=p} -> {direction=West; position=p}
    | {direction=East; position=p}  -> {direction=South; position=p}

let advance robot = 
    match robot with 
    | {direction=North; position=(x, y)} -> {direction=North; position=(x, y+1)}
    | {direction=West; position=(x, y)}  -> {direction=West; position=(x-1, y)}
    | {direction=South; position=(x, y)} -> {direction=South; position=(x, y-1)}
    | {direction=East; position=(x, y)}  -> {direction=East; position=(x+1, y)}    

let toList (s:string) =
    [for c in 
        s -> c]

let action robot command =
    match command with 
    | 'L' -> turnLeft robot
    | 'R' -> turnRight robot
    | 'A' -> advance robot
    | _   -> failwith "Invalid command"

let instructions instructions' robot = 
    instructions'
        |> toList
        |> List.fold action robot