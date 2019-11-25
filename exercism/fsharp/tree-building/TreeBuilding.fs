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

let markIfRoot treeBuilder ({ RecordId=recId; ParentId=parentId } as record) =
        if recId = 0 && parentId = 0 then treeBuilder.hasRoot <- true
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

    let rec private remove elm lst =
        match lst with
        | h::t when h = elm -> t
        | h::t              -> h::remove elm t
        | _                 -> []

    // Add in sorted set
    let rec private add (node: Tree) (children: Tree list) =
        match children with
        | []       -> [node]
        | hd :: tl -> if node.id = hd.id then hd :: tl
                      else
                          if node.id < hd.id then node :: hd :: tl
                          else hd :: add node tl

    let private values (d: Dictionary<int, Tree list>) =
        seq {
            for kv in d do
                yield (kv.Key, kv.Value)
        }

    let private change treeBuilder idx (newBranch: Tree list) =
        if treeBuilder.waitingParents.Remove idx then
            treeBuilder.waitingParents.Add (idx, newBranch)
        else
            failwith (sprintf "Waitings do not include parentId=%i" idx)

    let newBuilder = fun () -> TreeBuilder.empty

    // waitings must conatain parentId
    let group treeBuilder parentId (newLeaf: Tree) =
        let waitings = treeBuilder.waitingParents.Item parentId

        let newBranch = if parentId = 0 then
                            match waitings.[0] with
                            | Branch (_, children) -> if newLeaf.id = 0 then
                                                          [Branch (0, ref waitings)]
                                                      else
                                                          // has root with children
                                                          [Branch (0, ref (add newLeaf !children))]
                            | Leaf id when id = 0   -> [Branch (0, ref [newLeaf])]
                            | Leaf _ as leaf        -> if newLeaf.id = 0 then
                                                          [Branch (0, ref [leaf])]
                                                       else
                                                          // wait for root
                                                          add newLeaf waitings
                        else
                            add newLeaf waitings

        change treeBuilder parentId newBranch

    // waitings must contain recId
    let wrap treeBuilder (newLeaf: Tree) =
        let waitings = treeBuilder.waitingParents.Item newLeaf.id
        Branch (newLeaf.id, ref waitings)

    // parentId must not be part of waiting ids
    let placeLeaf treeBuilder parentId (newBranch: Tree) =

        let rec innerLoop (parent: Tree option) idx children =
            match children with
            | []              -> false
            | Leaf id as leaf :: rest
                              -> if parentId = id then
                                     match parent with
                                     | Some p -> let withoutParent = remove leaf !p.children
                                                 p.children := add (Branch(id, ref [newBranch])) withoutParent

                                     | None   -> let restBranch = treeBuilder.waitingParents.Item idx
                                                                      |> remove leaf

                                                 let branch = add (Branch (id, ref [newBranch])) restBranch
                                                 change treeBuilder idx branch
                                     true

                                 else if parentId < id then false
                                 else innerLoop parent idx rest

            | Branch (id, children) as branch :: rest
                              -> if parentId = id then
                                     branch.children := add newBranch !branch.children
                                     true
                                 else if parentId < id then false
                                 else
                                     let found = innerLoop (Some branch) idx !children

                                     if not found then innerLoop parent idx rest
                                     else found

        Seq.tryFind (fun (idx, children) -> innerLoop None idx children)
                        (values treeBuilder.waitingParents) |> Option.isSome

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

    let place treeBuilder { RecordId=recId; ParentId=parentId } =
        let newLeaf = Leaf recId

        if treeBuilder.waitingParents.ContainsKey recId && recId <> 0 then
            let newBranch = TreeBuilder.wrap treeBuilder newLeaf

            if treeBuilder.waitingParents.ContainsKey parentId then
                TreeBuilder.group treeBuilder parentId newLeaf

            else if not (TreeBuilder.placeLeaf treeBuilder parentId newBranch) then
                treeBuilder.waitingParents.Add (parentId, [newBranch])

        else if treeBuilder.waitingParents.ContainsKey parentId then
            TreeBuilder.group treeBuilder parentId newLeaf
            // TODO: check for indirect cycles
        else
            treeBuilder.waitingParents.Add (parentId, [newLeaf])

    let buildTree record =
        validateRecord record
            |> markIfRoot treeBuilder
            |> place treeBuilder

    // TODO: Check for detached nodes

    List.iter buildTree records

    treeBuilder.waitingParents.[0].[0]
