module StockTest

#if INTERACTIVE
#endif

open FsUnit.Xunit
open Xunit

open Stock

[<Fact>]
let ``The day should be 2012-03-13`` () =
    stockMarket |> should equal "2012-03-13"

