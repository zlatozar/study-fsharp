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

Cheating a little bit but it's clever

```f#
type DecisionTree =
    | Conditions of (string * DecisionTree) list
    | Result of string

let ocrTree = 
    Conditions [
        " _ ", Conditions [
            "| |", Result "0";
            " _|", Conditions [
                "|_ ", Result "2";
                " _|", Result "3"
            ];
            "|_ ", Conditions [
                " _|", Result "5";
                "|_|", Result "6"
            ];
            "  |", Result "7";
            "|_|", Conditions [
                "|_|", Result "8";
                " _|", Result "9"
            ]
        ];
        "   ", Conditions [
            "  |", Result "1";
            "|_|", Result "4"
        ]
    ]

let rec classify tree lines =
    match tree with
    | Result value -> value
    | Conditions conditions ->
        let node = List.tryFind (fun (s, _) -> s = Seq.head lines) conditions
        match node with
        | Some (_, subtree) -> classify subtree (Seq.tail lines)
        | None -> "?"

let convert (input: string) = 
    input.Split '\n'
    |> classify ocrTree

```