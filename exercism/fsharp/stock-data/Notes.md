## Learned from this challenge

- strings slicing
- how to cast
- pattern matching on list and its content
- how to use ```List.fold```
- how to nest functions
- indentation in a module

## Learned during development

- Code in a local module must be indented relative to the module,
  but code in a top-level module does not have to be indented.
- Namespace elements do not have to be indented.
- How to enumerate list - ```List.indexed```
- Destructing with pairs

```fsharp
// For a given list of calculated margins returns biggest
let rec _biggestVariance days =
    match days with
    | []        -> 0.0
    | (_,v)::[] -> v
    | (_,v)::xs -> max v (_biggestVariance xs)  // reduce as leaving max
```