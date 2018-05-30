## Learned from this challenge

- how to calculate chess possitions

## Learned during development

- `seq` with `for` loop

## Challenge notes

- position is defined by pair `(row, column)`
- chess board index starts from `0`, `0..7`
- coordinates are like in display starts from upper left corner

## Elegant solution from EXERCISM community

I didn't know that formula exist

```f#
module QueenAttack

let canAttack ((x1,y1) as p1) ((x2,y2) as p2) =
    if (p1 = p2) 
        then failwith "Cannot occupy same square"
    else 
        x1 = x2 || y1 = y2 || (abs (x1 - x2) = abs (y1 - y2))
```