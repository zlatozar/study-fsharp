## Learned from this challenge

## Learned during development

```f#

let isEmpty (body:byte[]) = 
            Array.forall (fun b -> b = 0uy) body
...
if not <| isEmpty body then ...
```
## Challenge notes

## Elegant solution from EXERCISM community