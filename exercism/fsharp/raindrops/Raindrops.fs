module Raindrops

let factorsOf upTo = 
    List.filter (fun num -> (upTo % num) = 0) [1..upTo]

let dropSound num sound = fun (lst, str) ->
                            match List.contains num lst with
                            | true -> (lst, str + sound)
                            | _    -> (lst, str)

let soundOf_3 = dropSound 3 "Pling"       
let soundOf_5 = dropSound 5 "Plang"
let soundOf_7 = dropSound 7 "Plong"

let convert (number: int): string =
    (factorsOf number, "")
        |> soundOf_3
        |> soundOf_5
        |> soundOf_7
        |> snd