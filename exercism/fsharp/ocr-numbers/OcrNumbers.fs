module OcrNumbers

let decodeDigit pix =
    match pix with
    | [" _ ";"| |";"|_|"] -> Some 0
    | ["   ";"  |";"  |"] -> Some 1
    | [" _ ";" _|";"|_ "] -> Some 2
    | [" _ ";" _|";" _|"] -> Some 3
    | ["   ";"|_|";"  |"] -> Some 4
    | [" _ ";"|_ ";" _|"] -> Some 5
    | [" _ ";"|_ ";"|_|"] -> Some 6
    | [" _ ";"  |";"  |"] -> Some 7
    | [" _ ";"|_|";"|_|"] -> Some 8
    | [" _ ";"|_|";" _|"] -> Some 9
    | _                   -> None
     
let composeDigits lst =
    let rec composeDigit lst =
        if String.length lst = 0 then []
        else 
           [lst.[0..2]] @ (composeDigit lst.[3..])
    List.map composeDigit lst
    
let rec formLines lst =
    if List.length lst = 0 then []
    else 
        [lst.[0..3]] @ (formLines lst.[4..])
           
let formatDigits (lst: string list list) =
    let numOfDigits = List.length lst.[0] - 1
    let rec recognizeLine idx =
        if idx > numOfDigits then []
        else 
            List.append [ [lst.[0].[idx]] @ [lst.[1].[idx]] @ [lst.[2].[idx]] ] (recognizeLine (idx + 1))
    recognizeLine 0 

let recognizeDigits digits =
    digits
        |> composeDigits
        |> formatDigits
        |> List.map decodeDigit
            
let convert input =
    formLines input
        |> List.map recognizeDigits