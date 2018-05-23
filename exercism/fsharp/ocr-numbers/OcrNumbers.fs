module OcrNumbers

let decodeDigit pix =
    match pix with
    | [" _ ";"| |";"|_|"] -> "0"
    | ["   ";"  |";"  |"] -> "1"
    | [" _ ";" _|";"|_ "] -> "2"
    | [" _ ";" _|";" _|"] -> "3"
    | ["   ";"|_|";"  |"] -> "4"
    | [" _ ";"|_ ";" _|"] -> "5"
    | [" _ ";"|_ ";"|_|"] -> "6"
    | [" _ ";"  |";"  |"] -> "7"
    | [" _ ";"|_|";"|_|"] -> "8"
    | [" _ ";"|_|";" _|"] -> "9"
    | _                   -> "?"

// all lines
let checkOcrScan lst=
    if (List.length lst % 4 = 0) then Some lst
    else None

let rec sliceLines lst =
    if List.length lst = 0 then []
    else 
        [lst.[0..3]] @ (sliceLines lst.[4..])

// every line
let checkOcrDigit lst =
    let ocrSize = 
        List.exists (fun elm -> not ((String.length elm) % 3 = 0)) lst
    if (ocrSize) then None
    else Some lst
     
let composeDigits lst =
    let rec composeDigit lst =
        if String.length lst = 0 then []
        else 
           [lst.[0..2]] @ (composeDigit lst.[3..])
    List.map composeDigit lst
    
let formatDigits (lst: string list list) =
    let numOfDigits = List.length lst.[0] - 1
    let rec recognizeLine idx =
        if idx > numOfDigits then []
        else 
            List.append [ [lst.[0].[idx]] @ [lst.[1].[idx]] @ [lst.[2].[idx]] ] (recognizeLine (idx + 1))
    recognizeLine 0 

// each digit
let recognizeDigits digits =
    checkOcrDigit digits
        |> Option.map composeDigits
        |> Option.map formatDigits
        |> Option.map (List.map decodeDigit)

// output
let reveal digits =
    match digits with 
    | Some d -> d
    | _      -> [] 
    
let createNum digits =
    let digitFragment = List.map (List.reduce (fun x y -> x + y)) digits
    List.reduce (fun a b -> a + "," + b) digitFragment
    
let revealNum digits =
    List.map reveal digits
        |> createNum

let display digits =
    match digits with 
    | Some [None]  -> None
    | Some d       -> Some (revealNum d)
    | _            -> None

// main
let convert input =
    checkOcrScan input
        |> Option.map sliceLines
        |> Option.map (List.map recognizeDigits)
        |> display