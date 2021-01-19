module Leap

let leapYear (year: int): bool = 
    match year % 4 = 0, year % 100 = 0, year % 400 = 0 with
    | false, _, _     -> false
    | true, false, _  -> true
    | _, true, false  -> false
    | _, _, true      -> true
