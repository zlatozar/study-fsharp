module TwoFer

// Learned in this challenge:
//     - F# Options
//     - string concatenation
let twoFer (input: string option): string = 
    match input with
    | None      -> "One for you, one for me."
    | Some name -> "One for " + name + ", one for me."