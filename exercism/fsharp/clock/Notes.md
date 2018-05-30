## Learned from this challenge

- tuple decomposition
- to use `sprintf` e.g `sprintf "%i" 10`
- `h'` is valid name

## Learned during development

- If you paste from somewhare be careful for "TAB"s

## Challenge notes

How to calculate clock - cool algorithm is needed.

# Elegant solution from EXERCISM community

```f#
open System

type Clock = int

let MIN_PER_DAY = 1440
let MIN_PER_HOUR = 60

let mkClock hr min =
    let clk = (hr * MIN_PER_HOUR + min) % MIN_PER_DAY
    if clk < 0 then clk + MIN_PER_DAY
    else clk

let add min clock =
    let clk = (clock + min) % MIN_PER_DAY
    if clk < 0 then clk + MIN_PER_DAY
    else clk

let subtract min clock =
    add -min clock

let display clock =
    sprintf "%02i:%02i" (clock/MIN_PER_HOUR) (clock%MIN_PER_HOUR)
```