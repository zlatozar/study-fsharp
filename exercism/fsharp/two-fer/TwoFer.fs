module TwoFer

let twoFer (input: string option): string = 
    match input with
    | None      -> "One for you, one for me."
    | Some name -> "One for " + name + ", one for me."