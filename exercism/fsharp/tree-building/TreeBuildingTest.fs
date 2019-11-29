// This file was created manually and its version is 1.0.0.

module TreeBuildingTest

open Xunit
open FsUnit.Xunit
open System

open TreeBuilding

[<Fact>]
let ``Attach record first level waiting leaf`` () =
    let t = [Leaf 1]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 1 (Leaf 20) |> should be True
    treeBuilder.waitingParents.[10] |> should equal [Branch (1, ref [Leaf 20])]

[<Fact>]
let ``Attach record first level waiting branch`` () =
    let t = [Branch (1, ref [Leaf 2])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 1 (Leaf 20) |> should be True
    treeBuilder.waitingParents.[10] |> should equal [Branch (1, ref [Leaf 2; Leaf 20])]

[<Fact>]
let ``Attach record in a deep leaf`` () =
    let t = [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 3 (Leaf 20) |> should be True
    treeBuilder.waitingParents.[10] |> should equal [Branch (0, ref [Leaf 1;  Branch (2, ref [Branch (3, ref [Leaf 20])])])]

[<Fact>]
let ``Attach record in a deep branch`` () =
    let t = [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 2 (Leaf 20) |> should be True
    treeBuilder.waitingParents.[10] |> should equal [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3; Leaf 20])])]

[<Fact>]
let ``Attach Leaf in a Leaf`` () =
    let t = [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 3 (Leaf 20) |> should be True
    treeBuilder.waitingParents.[10] |> should equal [Branch (0, ref [Leaf 1;  Branch (2, ref [Branch (3, ref [Leaf 20])])])]

[<Fact>]
let ``Attach Leaf in a Branch`` () =
    let t = [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 3 (Branch (20, ref [Leaf 30])) |> should be True
    treeBuilder.waitingParents.[10]
        |> should equal [Branch (0, ref [Leaf 1;  Branch (2, ref [Branch (3, ref [Branch (20, ref [Leaf 30])])])])]

[<Fact>]
let ``Attach Branch in a Leaf`` () =
    let t = [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 2 (Leaf 20) |> should be True
    treeBuilder.waitingParents.[10]
        |> should equal [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3; Leaf 20])])]

[<Fact>]
let ``Attach Branch in a Branch`` () =
    let t = [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 2 (Branch (20, ref [Leaf 30])) |> should be True
    treeBuilder.waitingParents.[10]
        |> should equal [Branch (0, ref [Leaf 1;  Branch (2, ref [Leaf 3; Branch (20, ref [Leaf 30])])])]

[<Fact>]
let ``Attach in a first level leaf`` () =
    let t = [Leaf 1]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 1 (Leaf 30) |> should be True
    treeBuilder.waitingParents.[10]
        |> should equal [Branch (1, ref [Leaf 30])]

[<Fact>]
let ``Attach record in a first level branch`` () =
    let t = [Branch (1, ref [Leaf 2])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 1 (Leaf 30) |> should be True
    treeBuilder.waitingParents.[10]
        |> should equal [Branch (1, ref [Leaf 2; Leaf 30])]

[<Fact>]
let ``Waiting for root`` () =
    let t = [Leaf 1]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (0, t)

    TreeBuilder.group treeBuilder 0 (Leaf 2)
    treeBuilder.waitingParents.[0] |> should equal [Leaf 1; Leaf 2]

[<Fact>]
let ``Root record already there`` () =
    let t = [Leaf 0]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (0, t)

    TreeBuilder.group treeBuilder 0 (Leaf 1)
    treeBuilder.waitingParents.[0] |> should equal [Branch (0, ref [Leaf 1])]

[<Fact>]
let ``Root record already there and has children`` () =
    let t = [Branch (0, ref [Leaf 2])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (0, t)

    TreeBuilder.group treeBuilder 0 (Leaf 1)
    treeBuilder.waitingParents.[0] |> should equal [Branch (0, ref [Leaf 1; Leaf 2])]

[<Fact>]
let ``Root record comes later`` () =
    let t = [Leaf 1]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (0, t)

    TreeBuilder.group treeBuilder 0 (Leaf 0)
    treeBuilder.waitingParents.[0] |> should equal [Branch (0, ref [Leaf 1])]

[<Fact>]
let ``Root record comes later 2`` () =
    let t = [Branch (1, ref [Leaf 2;  Branch (3, ref [Leaf 4])])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (0, t)

    TreeBuilder.group treeBuilder 0 (Leaf 0)
    treeBuilder.waitingParents.[0] |> should equal [Branch (0, ref [Branch (1, ref [Leaf 2;  Branch (3, ref [Leaf 4])])])]

[<Fact>]
let ``Waiting common parent`` () =
    let t = [Leaf 1]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.group treeBuilder 10 (Leaf 20)
    treeBuilder.waitingParents.[10] |> should equal [Leaf 1; Leaf 20]

[<Fact>]
let ``Depth First Search`` () =
    let t = [Branch (1, ref [Leaf 2;  Branch (3, ref [Branch (4, ref [Branch (5, ref [Leaf 6])])])]); Leaf 7; Branch (8, ref [Leaf 9])]

    let treeBuilder = TreeBuilder.empty
    treeBuilder.waitingParents.Add (10, t)

    TreeBuilder.placeLeaf treeBuilder 1 (Leaf 11) |> should be True
    TreeBuilder.placeLeaf treeBuilder 2 (Leaf 21) |> should be True

    TreeBuilder.placeLeaf treeBuilder 3 (Leaf 31) |> should be True
    TreeBuilder.placeLeaf treeBuilder 4 (Leaf 41) |> should be True
    TreeBuilder.placeLeaf treeBuilder 5 (Leaf 51) |> should be True

    TreeBuilder.placeLeaf treeBuilder 7 (Leaf 71) |> should be True
    TreeBuilder.placeLeaf treeBuilder 9 (Leaf 91) |> should be True

    TreeBuilder.placeLeaf treeBuilder 0 (Leaf 10) |> should be False

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
let ``Two nodes`` () =
    let input =
        [
            { RecordId = 0; ParentId = 0 }
            { RecordId = 1; ParentId = 0 }
        ]
    let tree = buildTree input

    children tree |> should equal [(Leaf 1)]

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
let ``Take and attach branch in a branch`` () =
    let input =
        [
            { RecordId = 5; ParentId = 1 };
            { RecordId = 4; ParentId = 1 };
            { RecordId = 0; ParentId = 0 };
            { RecordId = 1; ParentId = 0 };
        ]
    (*
        (0, 0)
            - (1, 0)
                - (4, 1)
                - (5, 1)

    *)

    let tree = buildTree input
    isBranch tree |> should equal true

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

        (*
            (0, 0)
                - (1, 0)
                    - (4, 1)
                    - (5, 1)
                - (2, 0)
                    - (3, 2)
                    - (6, 2)

        *)

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

[<Fact>]
let ``Branch id search`` () =
    let t = Branch (1, ref [Leaf 2;  Branch (3, ref [Branch (4, ref [Branch (5, ref [Leaf 6])])])])

    TreeBuilder.containsId 1 t |> should be True
    TreeBuilder.containsId 2 t |> should be True
    TreeBuilder.containsId 3 t |> should be True
    TreeBuilder.containsId 4 t |> should be True
    TreeBuilder.containsId 5 t |> should be True
    TreeBuilder.containsId 6 t |> should be True

    TreeBuilder.containsId 7 t |> should be False

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

            (2; 2) // Parent and id of incomming should be diffrent
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

    let tree = buildTree input
    tree |> should equal (Branch (0, ref [Leaf 1; Branch (2, ref [Leaf 4])]))

    (* Bug in the project was found. https://github.com/exercism/fsharp/issues/757 *)
