module Clock

let toPositive rawNegativeMin = 
    let rec calculate rawMin hours =
        if (rawMin >= 0) then hours
        else calculate (rawMin + 60) (hours + 1)

    calculate rawNegativeMin 0

let correction rawMin =
    let minutes = rawMin % 60
    if (rawMin >= 0) 
    then
        if (rawMin >= 60) then
            (((rawMin - minutes) / 60) % 24, minutes)
        else
            (0, rawMin)
    else
        (-(toPositive rawMin), 60 + minutes)

let normalize rawHours rawMin =
    let hourMinDeviation = correction rawMin
    let hours = (rawHours + (fst hourMinDeviation)) % 24
    if (hours < 0)
        then ((24 + hours), (snd hourMinDeviation))
        else (hours, (snd hourMinDeviation))
 
let humanDisplay clock =
    let h, m = clock
    sprintf "%02i:%02i" h m
 
let minOp f minutes clock =
    let h, m = clock
    let h', m' = correction minutes
    normalize (f h h') (f m m')

 //________________________________________________________
 // 

let create hours minutes =
    normalize hours minutes

let add minutes clock =
    minOp (+) minutes clock

let subtract minutes clock =
    minOp (-) minutes clock

let display clock = 
    clock |> humanDisplay