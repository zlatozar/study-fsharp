// This file was created manually and its version is 1.0.0.

module TreeBuildingTest

open Xunit
open FsUnit.Xunit
open System

open TreeBuilding

[<Fact>]
let ``Compare notes`` () =
    let t0 = Branch (0, [])
    let t1 = Leaf 1
    let t2 = Branch (4, [Branch (5, [])])

    let t3 = Branch (2, [Branch (3, [])])

    let inital =
        List.fold (fun lst el -> TreeBuilder.add el lst) List.empty<Tree> [t2; t1; t0]
    let sorted = if t2 > t1 then TreeBuilder.add t3 inital
                 else []

    List.length sorted |> should equal 4
    List.item 0 sorted |> should equal t0
    List.item 1 sorted |> should equal t1
    List.item 2 sorted |> should equal t3
    List.item 3 sorted |> should equal t2

[<Fact>]
let ``Unite notes`` () =
    let t0 = Branch (0, [])
    let t1 = Leaf 1
    let t2 = Branch (4, [Branch (5, [])])

    let t3 = Branch (2, [Branch (3, [])])

    let sorted =
        List.fold (fun lst el -> TreeBuilder.union [el] lst) List.empty<Tree> [t2; t1; t0; t3]

    List.length sorted |> should equal 4
    List.item 0 sorted |> should equal t0
    List.item 1 sorted |> should equal t1
    List.item 2 sorted |> should equal t3
    List.item 3 sorted |> should equal t2

[<Fact>]
let ``Depth First Search`` () =
    let t = Branch (0, [Leaf 1;  Branch (2, [Branch (3, [Branch (4, [Leaf 5])])])])

    let treeBuilder = TreeBuilder.empty
    treeBuilder.subTree.Add (6, t)

    TreeBuilder.findParent {RecordId=6; ParentId=1} treeBuilder |> should equal (Some (Leaf 1))
    TreeBuilder.findParent {RecordId=6; ParentId=2} treeBuilder |> should equal (Some (Branch (2, [Branch (3, [Branch (4, [Leaf 5])])])))

    TreeBuilder.findParent {RecordId=6; ParentId=3} treeBuilder |> should equal (Some (Branch (3, [Branch (4, [Leaf 5])])))
    TreeBuilder.findParent {RecordId=6; ParentId=4} treeBuilder |> should equal (Some (Branch (4, [Leaf 5])))
    TreeBuilder.findParent {RecordId=6; ParentId=5} treeBuilder |> should equal (Some (Leaf 5))

    TreeBuilder.findParent {RecordId=6; ParentId=7} treeBuilder |> should equal None

[<Fact>]
let ``One node`` () =
    let input =
        [
            { RecordId = 0; ParentId = 0 }
        ]

    let tree = buildTree input

    isBranch tree |> should equal false
    recordId tree |> should equal 0
    children tree |> should be Empty

[<Fact>]
let ``Three nodes in order`` () =
    let input =
        [
            { RecordId = 0; ParentId = 0 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 2; ParentId = 0 };
        ]

    let tree = buildTree input

    isBranch tree |> should equal true
    recordId tree |> should equal 0
    children tree |> List.length |> should equal 2

    children tree |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 0 |> recordId |> should equal 1

    children tree |> List.item 1 |> isBranch |> should equal false
    children tree |> List.item 1 |> recordId |> should equal 2

[<Fact>]
let ``Three nodes in reverse order`` () =
    let input =
        [
            { RecordId = 2; ParentId = 0 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 };
        ]

    let tree = buildTree input

    isBranch tree |> should equal true
    recordId tree |> should equal 0
    children tree |> List.length |> should equal 2

    children tree |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 0 |> recordId |> should equal 1

    children tree |> List.item 1 |> isBranch |> should equal false
    children tree |> List.item 1 |> recordId |> should equal 2

[<Fact>]
let ``More than two children`` () =
    let input =
        [
            { RecordId = 3; ParentId = 0 };
            { RecordId = 2; ParentId = 0 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 };
        ]

    let tree = buildTree input

    isBranch tree |> should equal true
    recordId tree |> should equal 0
    children tree |> List.length |> should equal 3

    children tree |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 0 |> recordId |> should equal 1

    children tree |> List.item 1 |> isBranch |> should equal false
    children tree |> List.item 1 |> recordId |> should equal 2

    children tree |> List.item 2 |> isBranch |> should equal false
    children tree |> List.item 2 |> recordId |> should equal 3

[<Fact>]
let ``Binary tree`` () =
    let input =
        [
            { RecordId = 5; ParentId = 1 };
            { RecordId = 3; ParentId = 2 };
            { RecordId = 2; ParentId = 0 };
            { RecordId = 4; ParentId = 1 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 };
            { RecordId = 6; ParentId = 2 }
        ]

    let tree = buildTree input

    isBranch tree |> should equal true
    recordId tree |> should equal 0
    children tree |> List.length |> should equal 2

    children tree |> List.item 0 |> isBranch |> should equal true
    children tree |> List.item 0 |> recordId |> should equal 1
    children tree |> List.item 0 |> children |> List.length |> should equal 2

    children tree |> List.item 0 |> children |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 0 |> children |> List.item 0 |> recordId |> should equal 4

    children tree |> List.item 0 |> children |> List.item 1 |> isBranch |> should equal false
    children tree |> List.item 0 |> children |> List.item 1 |> recordId |> should equal 5

    children tree |> List.item 1 |> isBranch |> should equal true
    children tree |> List.item 1 |> recordId |> should equal 2
    children tree |> List.item 1 |> children |> List.length |> should equal 2

    children tree |> List.item 1 |> children |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 1 |> children |> List.item 0 |> recordId |> should equal 3

    children tree |> List.item 1 |> children |> List.item 1 |> isBranch |> should equal false
    children tree |> List.item 1 |> children |> List.item 1 |> recordId |> should equal 6

[<Fact>]
let ``Unbalanced tree`` () =
    let input =
        [
            { RecordId = 5; ParentId = 2 };
            { RecordId = 3; ParentId = 2 };
            { RecordId = 2; ParentId = 0 };
            { RecordId = 4; ParentId = 1 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 };
            { RecordId = 6; ParentId = 2 }
        ]
    (*
        (0; 0)
            - (1; 0)        // branch
                  - (4; 1)
            - (2; 0)        // branch
                  - (3; 2)
                  - (5; 2)
                  - (6; 2)
    *)

    let tree = buildTree input

    isBranch tree |> should equal true
    recordId tree |> should equal 0
    children tree |> List.length |> should equal 2

    children tree |> List.item 0 |> isBranch |> should equal true
    children tree |> List.item 0 |> recordId |> should equal 1
    children tree |> List.item 0 |> children |> List.length |> should equal 1

    children tree |> List.item 0 |> children |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 0 |> children |> List.item 0 |> recordId |> should equal 4

    children tree |> List.item 1 |> isBranch |> should equal true
    children tree |> List.item 1 |> recordId |> should equal 2
    children tree |> List.item 1 |> children |> List.length |> should equal 3

    children tree |> List.item 1 |> children |> List.item 0 |> isBranch |> should equal false
    children tree |> List.item 1 |> children |> List.item 0 |> recordId |> should equal 3

    children tree |> List.item 1 |> children |> List.item 1 |> isBranch |> should equal false
    children tree |> List.item 1 |> children |> List.item 1 |> recordId |> should equal 5

    children tree |> List.item 1 |> children |> List.item 2 |> isBranch |> should equal false
    children tree |> List.item 1 |> children |> List.item 2 |> recordId |> should equal 6

[<Fact>]
let ``Tricky Unbalanced tree`` () =
    let input =
        [
            { RecordId = 0; ParentId = 0 };
            { RecordId = 3; ParentId = 1 }; // detached
            { RecordId = 4; ParentId = 2 };
            { RecordId = 2; ParentId = 1 }; // unite them
            { RecordId = 1; ParentId = 0 };
        ]
    (*
        (0; 0)
           - (1; 0)
               - (2; 1)
                    - (4; 2)
               - (3; 1)
    *)
    let tree = buildTree input
    isBranch tree |> should equal true

// Type of errors

// 1. Validate input

[<Fact>]
let ``Empty input`` () =
    let input = []
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>

[<Fact>]
let ``Root node has parent`` () =
    let input =
        [ { RecordId = 0; ParentId = 1 };
          { RecordId = 1; ParentId = 0 } ]
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>

[<Fact>]
let ``Higher id parent of lower id`` () =
    let input =
        [
            { RecordId = 0; ParentId = 0 };
            { RecordId = 2; ParentId = 0 };
            { RecordId = 1; ParentId = 2 }
        ]
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>

    (*
        (0; 0)
            - (2; 0)
                - (1; 2)  // id should be bigger than parent id
    *)

[<Fact>]
let ``Cycle directly`` () =
    let input =
        [
            { RecordId = 5; ParentId = 2 };
            { RecordId = 3; ParentId = 2 };
            { RecordId = 2; ParentId = 2 };
            { RecordId = 4; ParentId = 1 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 };
            { RecordId = 6; ParentId = 3 }
        ]
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>

    (*
        (0; 0)
            - (1; 0)
                - (4; 1)

            (2; 2) // Parent id of incomming should be diffrent
            (3; 2)
            (5; 2)

            (6; 3)
    *)

[<Fact>]
let ``Cycle indirectly`` () =
    let input =
        [
            { RecordId = 5; ParentId = 2 };
            { RecordId = 3; ParentId = 2 };
            { RecordId = 2; ParentId = 6 };
            { RecordId = 4; ParentId = 1 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 };
            { RecordId = 6; ParentId = 3 }
        ]
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>

    (*
        (0; 0)
            - (1; 0)
                - (4; 1)
        (2; 6)            // waiting for node with id=6
            - (3; 2)
                - (6; 3)  // id not on waiting ids, parent not in leaves
            - (5; 2)
    *)

// 2. Analyze after build

[<Fact>]
let ``No root node`` () =
    let input = [ { RecordId = 1; ParentId = 0 } ]
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>


[<Fact>]
let ``Non-continuous`` () =
    let input =
        [
            { RecordId = 2; ParentId = 0 };
            { RecordId = 4; ParentId = 2 };
            { RecordId = 1; ParentId = 0 };
            { RecordId = 0; ParentId = 0 }
        ]
    (fun () -> buildTree input |> ignore) |> should throw typeof<Exception>

    (*
        (0; 0)
            - (1; 0)
        ------------ detached
        (2; 0)
            - (4; 2)

    *)
