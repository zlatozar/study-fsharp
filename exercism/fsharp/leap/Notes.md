## Learned from this challenge

- how to specify parameter's types
- how to specify the type of function result
- ```match...with```
- same function in .NET framework ```DateTime.IsLeapYear```
- start using TDD using given test cases

## Elegant solution from EXERCISM community

```fsharp
let isLeapYear year = year % 400 = 0 ||
    (year % 4 = 0 && year % 100 <> 0)
```
