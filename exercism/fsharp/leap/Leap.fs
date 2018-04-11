module Leap

// Learned in this challenge:
//     - how to specify parameter's types
//     - how to specify the type of fuction result
//     - why to define leapYear using 'match...with'
//     - module fuction in F#
//     - there is a function in standard library
//     - test cases could be used in pattern matching
let leapYear (year: int): bool = 
    match year % 4 = 0, year % 100 = 0, year % 400 = 0 with
    | false, _, _       -> false
    | true, false, _    -> true
    | _, true, false    -> false
    | _, _, true        -> true