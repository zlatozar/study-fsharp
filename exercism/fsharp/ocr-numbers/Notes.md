## Learned from this challenge

- how to use `Option` for error cases

- how to use `Option.map`

- decompose the problem into smaller

- the difference between `map` and `reduce`

```f#
let display digits =
    match digits with 
    | Some [None]  -> None
    | Some d       -> Some (revealNum d)
    | _            -> None
```

## Learned during development

1. If you have long workflow it is very important every fuction to be tested
   very well.
   
2. Start with brut force then refine

3. Clever ideas matter!

## Elegant solution from EXERCISM community