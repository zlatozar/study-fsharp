module QueenAttack

let chessBoard = 
    seq { for i in [0..7] do
            for j in [0..7] do
                yield (i, j) }

let create (position: int * int) = 
    Seq.contains position chessBoard

let qDirections = [
        fun (x, y) -> (x-1, y);
        fun (x, y) -> (x-1, y+1);
        fun (x, y) -> (x,   y+1);
        fun (x, y) -> (x+1, y+1);
        fun (x, y) -> (x+1, y);
        fun (x, y) -> (x+1, y-1);
        fun (x, y) -> (x,   y-1);
        fun (x, y) -> (x-1, y-1)]

let direction (x, y) func =
    let rec calc (x, y) func acc =
        match func (x, y) with
        | x when create x -> calc x func (acc@[x])
        | _               -> acc
    calc (x, y) func []

let calcDirection (x, y) = fun func -> direction (x, y) func
    
let allMoves (x, y) =
    let move = calcDirection (x, y)
    Seq.fold (fun a func -> Seq.append a (move func)) Seq.empty qDirections
    
let canAttack (queen1: int * int) (queen2: int * int) =
    if (queen1 = queen2) then failwith "Can be in same position"
    else queen1
            |> allMoves
            |> Seq.contains queen2
