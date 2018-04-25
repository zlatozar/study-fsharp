module ArmstrongNumbers

let inline charToInt c = int c - int '0'

let calcArmstr (pwr, (digits: seq<char>)) =
    Seq.fold (fun x y -> x + pown (charToInt y) pwr) 0 digits

let digitToSeq num =
    num
        |> string
        |> seq
        |> fun x -> (Seq.length x, x)

let isArmstrongNumber (number: int): bool =
    number
        |> digitToSeq
        |> calcArmstr
        |> (=) number