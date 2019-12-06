## Learned from this challenge

I assumed that records are not oredred and comes in arbitrary order.
This make task a little bit difficult.

**Do not make hasty conclusions. Read carefully and make decisions according to the situation.**

## Learned during development

```f#

let isEmpty (body:byte[]) =
            Array.forall (fun b -> b = 0) body
...
if not <| isEmpty body then ...
```
For iteractive tests you can load files with:
`fsharpi --load:TreeBuilding.fs`

open the module and do the job.

## Challenge notes

https://stackoverflow.com/questions/6391334/tree-representation-in-f

Another possible representations:

```f#
type Tree<'a> =
     | Node of 'a * Tree<'a> list

// Consing
type Tree =
    | Leaf of int
    | Branch of (Tree * Tree)
```

## Elegant solution from EXERCISM community

```f#
let buildTree records =
    if List.isEmpty records then
        failwith "Empty input"

    let leafs =
        records
        |> List.sortBy (fun x -> x.RecordId)
        |> List.mapi (fun i r ->
            match r : Record with
            | record when i = 0 && (record.ParentId <> 0 || record.RecordId <> 0) ->
                failwith "Root node is invalid"
            | record when record.RecordId <> i ->
                failwith "Non-contiguous list"
            | record when record.RecordId <> 0 && (record.ParentId > record.RecordId || record.ParentId = record.RecordId) ->
                failwith "Nodes with invalid parents"
            | _ ->
                match r.RecordId with
                | 0 -> (-1, r.RecordId)
                | _ -> (r.ParentId, r.RecordId))

    let grouped =
        leafs
        |> List.groupBy fst
        |> List.map (fun (x, y) -> (x, List.map snd y))

    let map =
        grouped
        |> Map.ofSeq

    let rec helper key =
        if Map.containsKey key map then
            Branch (key, List.map helper (map |> Map.find key))
        else
            Leaf key

    helper 0
```