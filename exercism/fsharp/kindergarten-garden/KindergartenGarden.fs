module KindergartenGarden

type Plant =
    | Clover
    | Grass
    | Radishes
    | Violets

let plantName = function
    | 'C' -> Clover
    | 'G' -> Grass
    | 'R' -> Radishes
    | 'V' -> Violets
    | _   -> failwith "Unknown plant"

let studentPos = function
    | "Alice"   -> 0
    | "Bob"     -> 2
    | "Charlie" -> 4
    | "David"   -> 6
    | "Eve"     -> 8
    | "Fred"    -> 10
    | "Ginny"   -> 12
    | "Harriet" -> 14
    | "Ileana"  -> 16
    | "Joseph"  -> 18
    | "Kincaid" -> 20
    | "Larry"   -> 22
    | _         -> failwith "Invalid student name"

let splitter (rows: string) =
    rows.Split('\n')
        |> Array.map (fun x -> x.ToCharArray())

let select student =
    let first = studentPos student
    Array.fold (fun x (row: char array) -> x @ [row.[first]] @ [row.[first + 1]]) [] 

let plants diagram student = 
    let studentPlants = select student
    splitter diagram
        |> studentPlants
        |> List.map (fun x -> plantName x)