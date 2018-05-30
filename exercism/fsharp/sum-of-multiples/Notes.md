## Learned from this challenge

- discover `Set` functionality
- better use of `|>`
- `Set.empty`
- higher forder functions

## Elegant solution from EXERCISM community

Tip: `Seq` is universal, study `List` and `Set` methods

```f#

let multiplesOf max n =
    [n..n..(max - 1)]

let sumOfMultiples numbers max =
    numbers
        |> Seq.map (multiplesOf max)
        |> Seq.concat
        |> Seq.distinct
        |> Seq.sum
```