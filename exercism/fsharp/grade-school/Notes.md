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

// more fsharp-ish
school 
        |> Map.toList
        |> List.sortBy fst
```

## Learned during development

[F# book](https://en.wikibooks.org/wiki/F_Sharp_Programming) 

## Elegant solution from EXERCISM community

```fsharp
let grade (number: int) (school: School): string list = 
    school 
        |> Map.tryFind number 
        |> Option.defaultValue List.empty
```