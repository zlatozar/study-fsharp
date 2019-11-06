module TreeBuilding

open System.Collections.Generic

type Record = { RecordId: int; ParentId: int }

[<CustomEquality; CustomComparison>]
type Tree =
    | Branch of int * Tree list
    | Leaf of int

    member t.id = match t with
                  | Branch (id, _) -> id
                  | Leaf id        -> id

    override t1.Equals t =
        match t with
        | :? Tree as t2 -> match t1, t2 with
                           | Branch (i1, l1), Branch (i2, l2) -> i1 = i2 && l1 = l2
                           | Leaf i1, Leaf i2                 -> i1 = i2
                           | _                                -> false
        | _             -> false

    override t.GetHashCode() = match t with
                               | Branch (i, l) -> hash i + hash l
                               | Leaf i        -> hash i

    override t.ToString() = match t with
                            | Branch (i, l) -> sprintf "%i %A" i l
                            | Leaf i        -> sprintf "%i" i

    interface System.IComparable with
        member t1.CompareTo t =
            match t with
            | :? Tree as t2 -> // just enough for our case
                               compare t1.id t2.id
            | _             -> failwith "Cannot compare values of different types."

type TreeBuilder =
    { hasRoot: bool; subTree: Dictionary<int, Tree> }
    with
        static member empty = { hasRoot=false; subTree=new Dictionary<int, Tree>() }

[<RequireQualifiedAccess>]
module TreeBuilder =

    let newBuilder = fun () -> TreeBuilder.empty

    let placeNew { RecordId=recId; ParentId=parentId } treeBuilder =
        treeBuilder.subTree.Add (parentId, Leaf recId)

    let findBranch { RecordId=recId; ParentId=_ } treeBuilder =
        if treeBuilder.subTree.ContainsKey recId
            then Some (treeBuilder.subTree.Item recId)
            else None

    // DFS
    let findParent { RecordId=recId; ParentId=parentId } treeBuilder  =
        let rec innerLoop children =
            match children with
            | []              -> None
            | Leaf id as leaf :: rest
                              -> if id = parentId then Some leaf
                                 else innerLoop rest
            | Branch (id, children) as branch :: rest
                              -> if id = parentId then Some branch
                                 else
                                     match innerLoop children with
                                     | None   -> innerLoop rest
                                     | _ as r -> r

        let waiting = treeBuilder.subTree.Item recId

        match waiting with
        | Leaf id as leaf -> if id = parentId then Some leaf
                              else None
        | Branch (id, children) as branch
                          -> if id = parentId then Some branch
                             else innerLoop children

    // Add in sorted set
    let rec add (node: Tree) (children: Tree list) =
        match children with
        | []       -> [node]
        | hd :: tl -> if node.id = hd.id then hd :: tl
                      else
                          if node.id < hd.id then node :: hd :: tl
                          else hd :: add node tl

    let addRec (record: Record) (tree: Tree list) =
        let leaf = Leaf record.RecordId
        add leaf tree

    let rec union (tree1: Tree list) (tree2: Tree list) =
        match tree1, tree2 with
        | [], _       -> tree2
        | _, []       -> tree1
        | hd :: tl, _ -> add hd (union tl tree2)

// Helper functions

let recordId t =
    match t with
    | Branch (id, _) -> id
    | Leaf id        -> id

let isBranch t =
    match t with
    | Branch _  -> true
    | Leaf _    -> false

let children t =
    match t with
    | Branch (_, c) -> c
    | Leaf _        -> []

let validateRecord ({ RecordId=id; ParentId=parentId }) =
    // could be used for optimization during searching
    let idShouldBigger =
        if (id < parentId) then failwith "Record 'id' should be bigger than 'parentId'."

    let idDifferParentId =
        // except for root element
        if (id <> 0 && parentId <> 0) then
            if (id = parentId)
                then failwith "Direct cycle is detected. 'id' should not equal to 'parentId'."

    idShouldBigger; idDifferParentId

// Refactor only this function
let buildTree (records: Record list) =
    if List.isEmpty records then failwith "Record list shoul not be empty."

    // Work with only one
    let record = records.Item 0

    let treeBuilder = TreeBuilder.empty


    // dummy
    Leaf 0
