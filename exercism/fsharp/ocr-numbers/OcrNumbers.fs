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

let checkOcrScan lst=
    if (List.length lst % 4 = 0) then Some lst
    else None

let rec sliceLines lst =
    if List.length lst = 0 then []
    else 
        [lst.[0..3]] @ (sliceLines lst.[4..])
   
// checkOcrScan ["    "] |> Option.map sliceLines |> ignore
   
let checkOcrDigit lst =
    let ocrSize = 
        List.exists (fun elm -> not (String.length elm % 3 = 0)) lst
    if (ocrSize) then None
    else Some lst
     
let composeDigits lst =
    let rec composeDigit lst =
        if String.length lst = 0 then []
        else 
           [lst.[0..2]] @ (composeDigit lst.[3..])
    List.map composeDigit lst
    
// checkOcrDigit ["   "; "  |"; "  |"; "   "] |> Option.map composeDigits |> ignore 
           
let formatDigits (lst: string list list) =
    let numOfDigits = List.length lst.[0] - 1
    let rec recognizeLine idx =
        if idx > numOfDigits then []
        else 
            List.append [ [lst.[0].[idx]] @ [lst.[1].[idx]] @ [lst.[2].[idx]] ] (recognizeLine (idx + 1))
    recognizeLine 0 

// checkOcrDigit ["   "; "  |"; "  |"; "   "] |> Option.map composeDigits |> Option.map formatDigits |> ignore

let recognizeDigits digits =
    checkOcrDigit digits
        |> Option.map composeDigits
        |> Option.map formatDigits
        |> Option.map (List.map decodeDigit)

// recognizeDigits [ "   "; "  |"; "  |"; "   " ] |> ignore    
        
let convert input =
    checkOcrScan input
        |> Option.map sliceLines
        |> Option.map (List.map recognizeDigits)