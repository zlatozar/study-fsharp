## Learned from this challenge

- how to use `seq {}`
- how to use `function expressions`
- always is a good idea to see other solutions

## Elegant solution from EXERCISM community

TIP: Note that you can use `if...then` in yield, `else` is missing.

```f#
let convert (number: int): string =
    seq {
        if number % 3 = 0 then
            yield "Pling"
        if number % 5 = 0 then
            yield "Plang"
        if number % 7 = 0 then
            yield "Plong"
    } |> String.concat ""
      |> function | "" -> string(number)
                  | s  -> s
```