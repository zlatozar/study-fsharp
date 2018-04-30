module CollatzConjecture

let rec calcSteps n acc =
        match n with 
        | 1                -> acc
        | n when n % 2 = 0 -> calcSteps (n/2) acc + 1
        | _                -> calcSteps (3*n + 1) acc + 1

let steps (number: int): int option = 
    match number with
    | n when n > 0 -> Some (calcSteps n 0)
    | _            -> None