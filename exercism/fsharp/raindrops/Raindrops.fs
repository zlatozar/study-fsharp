module Raindrops

let convert (number: int): string =
    seq {
        if number % 3 = 0 then
            yield "Pling"
        if number % 5 = 0 then
            yield "Plang"
        if number % 7 = 0 then
            yield "Plong"
    } |> String.concat ""
      |> function | "" -> string(number)
                  | s  -> s
