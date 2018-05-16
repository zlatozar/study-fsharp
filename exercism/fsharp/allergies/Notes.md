## Learned from this challenge

- types are always in upper case
- how to use tagged values constructor
- tagged values constructor is a **function**
- constructor could be used in pattern matching

## Learned during development

- search of patterns is the root of mathematics/programming
- ```active patterns``` can't return more than 7 possibities

## Challenge notes

- the algorithm to find all scores in given digit was difficult
- bitwise operations could be helpful when have power of 2

## Elegant solution from EXERCISM community

Tricky:

```f#
type Allergen =
    | Eggs = 1
    | Peanuts = 2
    | Shellfish = 4
    | Strawberries = 8
    | Tomatoes = 16
    | Chocolate = 32
    | Pollen = 64
    | Cats = 128

let allergicTo codedAllergies (allergen: Allergen) = 
    let aller = int allergen
    (codedAllergies &&& aller) = aller
```