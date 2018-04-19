module Bob

open System

// If the sentence contains only white spaces              => 'Fine. Be that way!'

// "ask him" is when the sentence contains '?' at the end. All kind of symbols are allowed, but not all upper case => "Sure."
// "yell at him" is when sentence is all upper case. All kind of symbols and letter(s) all in upper case           => "Whoa, chill out!"
// "ask him" and "yell at him"                             => "Calm down, I know what I'm doing!"
// For all the rest                                        => "Whatever."


let trimIt (x:string) =
    x.Trim()

 // NOTE: If we have a chain of functions in a row, how are they combined?
 // let F x y z = x y z  <=> let F x y z = ((x y) z)  => left associative !

let isEmpty (trimmedSentence:string) =
    trimmedSentence.Length = 0

let analizeChar f = fun x ->
    if (Char.IsLetter x) then f x
    else true

let isRegular sentence =
    String.forall (analizeChar Char.IsLower) sentence

let selectUpperCase sentence =
    sentence |> Seq.filter Char.IsLetter |> String.Concat 

let containsUpperCaseOnly sentence =
    let upper = selectUpperCase sentence
    if (String.length upper > 0)
    then String.forall Char.IsUpper upper
    else false

let isYelling sentence =
    not (isRegular sentence) && containsUpperCaseOnly sentence

let isQuestion (sentence:string) =
    sentence.EndsWith "?"

// Learned in this challenge:
//     - trim string
//     - function composition (>>) use it if you have chain of operations on given input
//     - char funcitons
//     - convert string to sequence and manipulate it
let response (input: string): string = 
    let sentence = trimIt input
    if (isEmpty sentence) then "Fine. Be that way!"
    else 
        match isQuestion sentence, isYelling sentence with
        | true, false -> "Sure."
        | false, true -> "Whoa, chill out!"
        | true, true  -> "Calm down, I know what I'm doing!"
        | _, _        -> "Whatever."
