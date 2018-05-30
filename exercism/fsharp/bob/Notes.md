## Learned from this challenge

- trim string
- char funcitons
- convert string to sequence and manipulate it

## Learned during development

- function composition `>>` - use it if you have chain of operations on given input

If we have a chain of functions in a row, how are they combined?

```
let F x y z = x y z  <=> let F x y z = ((x y) z)  => left associative !
```

## Challenge notes

`Fine. Be that way!` - If the sentence contains only white spaces
`Sure.`              - The sentence contains '?' at the end. All kind of symbols are allowed, but not all upper case
`Whoa, chill out!`   - The sentence is all upper case. All kind of symbols and letter(s) all in upper case 

`Calm down, I know what I'm doing!` - "ask him" and "yell at him" 
`Whatever.`         - For the rest

 ## Elegant solution from EXERCISM community

 ```f#
 open System

let isUpper (s:string): bool    = s = s.ToUpper()
let isQuestion (s:string): bool = s.EndsWith("?")
let isNumeric (s:string): bool  = s |> Seq.exists Char.IsLetter |> not
let isEmpty (s:string): bool    = s = ""

let response (input: string): string =
    match input.Trim() with
        | i when i |> isEmpty                      -> "Fine. Be that way!"
        | i when i |> isNumeric && i |> isQuestion -> "Sure."
        | i when i |> isNumeric                    -> "Whatever."
        | i when i |> isUpper && i |> isQuestion   -> "Calm down, I know what I'm doing!"
        | i when i |> isQuestion                   -> "Sure."
        | i when i |> isUpper                      -> "Whoa, chill out!"
        | _                                        -> "Whatever."
```
	