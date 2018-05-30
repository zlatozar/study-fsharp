## Learned from this challenge

- `string`(also other simpe types) is a function that converts passed paramter
- In F# it is possible to _inline_ function
- `pown` works with `int`s

## Learned during development

```f#
System.Char.GetNumericValue(x)
```

## Challenge notes

## Elegant solution from EXERCISM community

```f#
let isArmstrongNumber (number: int): bool = 
    let list = number 
               |> string 
               |> Seq.map (string >> int) // char to string then to int
               |> Seq.toList

    let prod = list 
               |> List.sumBy (fun x -> pown x list.Length)

    number = prod
```