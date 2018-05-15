module Bob

open System

let trimIt (x:string) =
    x.Trim()

let isEmpty (trimmedSentence:string) =
    trimmedSentence.Length = 0

let analizeChar f = fun x ->
    if (Char.IsLetter x) then f x
    else true

// Tip: Take a look of Stirng.exist/Seq.exist...

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

let response (input: string): string = 
    let sentence = trimIt input
    if (isEmpty sentence) then "Fine. Be that way!"
    else 
        match isQuestion sentence, isYelling sentence with
        | true, false -> "Sure."
        | false, true -> "Whoa, chill out!"
        | true, true  -> "Calm down, I know what I'm doing!"
        | _, _        -> "Whatever."
