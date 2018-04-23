module SumOfMultiples

let naturalNums x = [1..x - 1]

let numsSet upTo = fun divBy -> naturalNums upTo 
                                |> List.filter (fun num -> (num % divBy) = 0) 
                                |> Set.ofList

let sumSetElm aSet =
    Set.fold (fun x y -> x + y) 0 aSet

let collectMults upperBound mults =
    Set.map (numsSet upperBound) (Set.ofList mults)

let unionAll sets =
    Set.fold (Set.union) Set.empty sets

let sum (numbers: int list) (upperBound: int): int = 
    let multSet = collectMults upperBound numbers
    match numbers with
    | []  -> 0
    | _   -> unionAll multSet |> sumSetElm
