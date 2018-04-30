## Learned from this challenge

- ```type School = Map<int, string list>``` as type alias
- how to compose functions with ```|>```

```fsharp
let school =
        empty        
        |> add "Jennifer" 4
        |> add "Kareem" 6
 ```
 Function ```add``` has as last parameter ```School``` in this way we preset first two and the last one is
 set by ```|>```.

 - ```Map``` is list of pairs
 - Map in F# is immutable so ```Map.add``` return new one
 - ```Map.find`` throws an exception if there is no element with a given key

```fsharp
List.sort (Map.toList school)

// more fsharp-ish start from right to left
school 
        |> Map.toList
        |> List.sortBy fst
```

## Learned during development

[F# book](https://en.wikibooks.org/wiki/F_Sharp_Programming) 

```fsharp
let grade (number: int) (school: School): string list = 
    school 
        |> Map.tryFind number 
        |> Option.defaultValue List.empty
```

## Elegant solution from EXERCISM community

```fsharp
let empty = Map.empty

let roster s =
  Map.toList s
  |> List.sort

let grade g s =
  match Map.tryFind g s with
  | Some x -> x
  | _ -> []

let add name g s =
  let newList = List.sort (name::(grade g s))
  Map.add g newList s

```