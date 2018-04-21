﻿module SumOfMultiples

let naturalNums x = [1..x - 1]

let numsSet upTo = fun divBy -> naturalNums upTo 
                                |> List.filter (fun num -> (num % divBy) = 0) 
                                |> Set.ofList

let sumSetElm aSet =
    Set.fold (fun x y -> x + y) 0 aSet

let collectMults upperBound  = fun mults ->
                               Set.map (numsSet upperBound) (Set.ofList mults)

// Learned in this challenge:
//     - discover Set functionality
//     - better use of |>
//     - Set.empty
//     - higher forder functions
//     - start reading others solutions
let sum (numbers: int list) (upperBound: int): int = 
    let multSet = collectMults upperBound
    match numbers with
    | []  -> 0
    | _   -> Set.fold Set.union Set.empty (multSet numbers) |> sumSetElm

// EXERCISM solution: Seq is universal, study List, Set, Seq

//let multiplesOf max n =
//    [n..n..(max - 1)]

//let sumOfMultiples numbers max =
//    numbers
//        |> Seq.map (multiplesOf max)
//        |> Seq.concat
//        |> Seq.distinct
//        |> Seq.sum