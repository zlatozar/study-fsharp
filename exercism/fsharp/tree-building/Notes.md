## Learned from this challenge

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