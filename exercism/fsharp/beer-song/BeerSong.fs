module BeerSong

let stratePattern (x: int) =
    x 
      |> string 
      |> fun s -> ["";
                   s + " bottles of beer on the wall, " + s + " bottles of beer.";
                   "Take one down and pass it around, " + string (x - 1) + " bottle of beer on the wall.";]

let strate num =
    match num with 
    | 0 -> ["";
            "No more bottles of beer on the wall, no more bottles of beer.";
            "Go to the store and buy some more, 99 bottles of beer on the wall.";]
    | 1 -> ["";
            "1 bottle of beer on the wall, 1 bottle of beer.";
            "Take it down and pass it around, no more bottles of beer on the wall.";]
    | x -> (stratePattern x)

let arangeStrates bottles down =
    let strates = [(bottles - down + 1)..bottles]
    strates
        |> List.rev
        |> List.map (fun x -> if (x < 0)
                                then 0
                                else x)

let recite (startBottles: int) (takeDown: int) = 
    let theSong = arangeStrates startBottles takeDown
    List.collect strate theSong |> List.tail