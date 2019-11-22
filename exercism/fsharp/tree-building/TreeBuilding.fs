module TreeBuilding

open System.Collections.Generic

type Record = { RecordId: int; ParentId: int }

[<CustomEquality; CustomComparison>]
type Tree =
    | Branch of int * Tree list ref
    | Leaf of int

    member t.id = match t with
                  | Branch (id, _) -> id
                  | Leaf id        -> id

    member t.children = match t with
                        | Branch (_, children) -> children
                        | Leaf _               -> ref []

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

    interface System.IComparable with
        member t1.CompareTo t =
            match t with
            | :? Tree as t2 -> // just enough for our case
                               compare t1.id t2.id
            | _             -> failwith "Cannot compare values of different types."

type TreeBuilder =
    { mutable hasRoot: bool; waitingParents: Dictionary<int, Tree list> }
    with
        static member empty = { hasRoot=false; waitingParents=new Dictionary<int, Tree list>() }

let rootRecord { RecordId=recId; ParentId=parentId } =
        if recId = 0 && parentId = 0 then
            true
        else false

let markIfRoot treeBuilder record =
        if rootRecord record then treeBuilder.hasRoot <- true
        record

let validateRecord ({ RecordId=id; ParentId=parentId } as record) =
    let idShouldBigger =
        if (id < parentId) then
            failwith (sprintf "Invalid record: %A. 'id' should be bigger than 'parentId'." record)

    let idDifferParentId =
        // except for root element
        if (id <> 0 && parentId <> 0) then
            if (id = parentId) then
                failwith (sprintf "Direct cycle was detected for record: %A" record)

    idShouldBigger; idDifferParentId
    record

[<RequireQualifiedAccess>]
module TreeBuilder =

    let newBuilder = fun () -> TreeBuilder.empty

    let rec remove elm lst =
        match lst with
        | h::t when h = elm -> t
        | h::t              -> h::remove elm t
        | _                 -> []

    // Add in sorted set
    let rec add (node: Tree) (children: Tree list) =
        match children with
        | []       -> [node]
        | hd :: tl -> if node.id = hd.id then hd :: tl
                      else
                          if node.id < hd.id then node :: hd :: tl
                          else hd :: add node tl

    let values (d: Dictionary<int, Tree list>) =
        seq {
            for kv in d do
                yield (kv.Key, kv.Value)
        }

    let tryToPlaceBranch treeBuilder parentId (branch: Tree) =

        let rec innerLoop (parent: Tree option) idx children =
            match children with
            | []              -> false
            | Leaf id as leaf :: rest
                              -> if parentId = id then
                                     match parent with
                                     | Some p -> let withoutParent = remove leaf !p.children
                                                 p.children := add (Branch(id, ref [branch])) withoutParent

                                     | None   -> let newBranch = treeBuilder.waitingParents.Item idx
                                                                     |> remove leaf
                                                                     |> add (Branch (id, ref [branch]))
                                                 treeBuilder.waitingParents.Remove idx |> ignore
                                                 treeBuilder.waitingParents.Add (idx, newBranch)
                                     true

                                 else if id > parentId then false
                                 else innerLoop parent idx rest

            | Branch (id, children) as parentBranch :: rest
                              -> if parentId = id then
                                     parentBranch.children := add branch !parentBranch.children
                                     true
                                 else if id > parentId then false
                                 else
                                     let found = innerLoop (Some parentBranch) idx !children

                                     if not found then innerLoop parent idx rest
                                     else found

        if treeBuilder.waitingParents.ContainsKey parentId then
            let waitings = treeBuilder.waitingParents.Item parentId

            let newBranch = if parentId = 0 && treeBuilder.hasRoot then
                                match waitings.[0] with
                                | Branch (_, children) -> [Branch (0, ref (add branch !children))]
                                | Leaf _               -> [Branch (0, ref [branch])]
                            else
                                add branch waitings

            treeBuilder.waitingParents.Remove parentId |> ignore
            treeBuilder.waitingParents.Add (parentId, newBranch)
            true

        else
            Seq.tryFind (fun (idx, children) -> innerLoop None idx children)
                            (values treeBuilder.waitingParents) |> Option.isSome

    let tryToPlaceLeaf treeBuilder ({ RecordId=recId; ParentId=parentId } as record) =

        // searching root
        let rec innerLoop (parent: Tree option) idx children =
            match children with
            | []              -> false
            | Leaf id as leaf :: rest
                              -> if parentId = id then
                                     let newLeaf = [Leaf recId]

                                     match parent with
                                     | Some p -> let withoutParent = remove leaf !p.children
                                                 p.children := add (Branch(id, ref newLeaf)) withoutParent

                                     | None   -> let restBranch = treeBuilder.waitingParents.Item idx
                                                                      |> remove leaf

                                                 let newBranch = add (Branch (id, ref [Leaf recId])) restBranch

                                                 treeBuilder.waitingParents.Remove idx |> ignore
                                                 treeBuilder.waitingParents.Add (idx, newBranch)
                                     true

                                 else if id > recId then false
                                 else innerLoop parent idx rest

            | Branch (id, children) as branch :: rest
                              -> if parentId = id then
                                     branch.children := add (Leaf recId) !branch.children
                                     true
                                 else if id > recId then false
                                 else
                                     let found = innerLoop (Some branch) idx !children

                                     if not found then innerLoop parent idx rest
                                     else found

        if treeBuilder.waitingParents.ContainsKey parentId then
            let waitings = treeBuilder.waitingParents.Item parentId

            let newBranch = if rootRecord record then
                                [Branch (0, ref waitings)]
                            else
                                if parentId = 0 && treeBuilder.hasRoot then
                                    match waitings.[0] with
                                    | Branch (_, children) -> [Branch (0, ref (add (Leaf recId) !children))]
                                    | Leaf _               -> [Branch (0, ref [Leaf recId])]
                                else
                                    add (Leaf recId) waitings

            treeBuilder.waitingParents.Remove parentId |> ignore
            treeBuilder.waitingParents.Add (parentId, newBranch)
            true

        else
            Seq.tryFind (fun (idx, children) -> innerLoop None idx children)
                            (values treeBuilder.waitingParents) |> Option.isSome

    let placeRoot treeBuilder record =
        if not (rootRecord record) then failwith (sprintf "Given record %A is not root record" record)
        else
            if treeBuilder.waitingParents.ContainsKey 0 then
                let waitings = treeBuilder.waitingParents.Item 0
                let newBranch = [Branch (0, ref waitings)]

                treeBuilder.waitingParents.Remove 0 |> ignore
                treeBuilder.waitingParents.Add (0, newBranch)
            else
                treeBuilder.waitingParents.Add (0, [Leaf 0])

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
    | Branch (_, c) -> !c
    | Leaf _        -> []

// Main
let buildTree (records: Record list) :Tree =
    if List.isEmpty records then failwith "Record list should not be empty."

    let treeBuilder = TreeBuilder.empty

    let place treeBuilder ({ RecordId=recId; ParentId=parentId } as record) =

        if rootRecord record then TreeBuilder.placeRoot treeBuilder record
        else
            // Is waited parent record came?
            if treeBuilder.waitingParents.ContainsKey recId then
                let children = treeBuilder.waitingParents.Item recId
                let newBranch = Branch (recId, ref children)

                if TreeBuilder.tryToPlaceBranch treeBuilder parentId newBranch then
                    treeBuilder.waitingParents.Remove recId |> ignore
                else
                    treeBuilder.waitingParents.Remove recId |> ignore
                    treeBuilder.waitingParents.Add (recId, [newBranch])
            else
                if not (TreeBuilder.tryToPlaceLeaf treeBuilder record) then
                    treeBuilder.waitingParents.Add (parentId, [Leaf recId])

    let buildTree record =
        validateRecord record
            |> markIfRoot treeBuilder
            |> place treeBuilder

    // TODO: Check for detached nodes

    List.iter buildTree records

    treeBuilder.waitingParents.[0].[0]
