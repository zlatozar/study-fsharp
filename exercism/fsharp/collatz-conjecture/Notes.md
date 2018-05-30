## Elegant solution from EXERCISM community

```f#
let steps (number: int): int option =
    let rec stepsRec acc num =
        match num with
        | x when x < 1     -> None
        | x when x = 1     -> acc |> Some
        | x when x % 2 = 0 -> x / 2 |> stepsRec (acc + 1)
        | x                -> 3 * x + 1 |> stepsRec (acc + 1)

    stepsRec 0 number
```