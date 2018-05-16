module Allergies

open System

type Allergen =
    | Eggs
    | Peanuts
    | Shellfish
    | Strawberries
    | Tomatoes
    | Chocolate
    | Pollen
    | Cats
    
let mapScore score = 
    match score with 
    | 1   -> Eggs
    | 2   -> Peanuts
    | 4   -> Shellfish
    | 8   -> Strawberries
    | 16  -> Tomatoes
    | 32  -> Chocolate
    | 64  -> Pollen
    | 128 -> Cats
    | _   -> failwith "Unknown allergen"
    
let knownScores =
    List.fold (fun x y -> List.append  x [pown 2 y]) [] [0..7]
        |> List.rev
    
let findAllergies score =
    let rec calc score codes acc =
        if (List.isEmpty codes) then acc
        else
            if (List.contains score codes) then [score]
            else 
                let t = score - (List.head codes)
                if t < 0 then calc score (List.tail codes) acc
                    else calc t (List.tail codes) acc@[List.head codes]
 
    calc score knownScores []
    
let allKnownScores num =
    findAllergies num
        |> List.map mapScore
    
let allergicTo codedAllergies allergen = 
    allKnownScores codedAllergies
        |> List.contains allergen

let list codedAllergies = 
    allKnownScores codedAllergies