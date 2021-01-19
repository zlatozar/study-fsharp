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

let rec private remove elm lst =
        match lst with
        | h::t when h = elm -> t
        | h::t              -> h::remove elm t
        | _                 -> []

let rec private add (node: Tree) (children: Tree list) =
    match children with
    | []       -> [node]
    | hd :: tl -> if node.id = hd.id then hd :: tl
                    else
                        if node.id < hd.id then node :: hd :: tl
                        else hd :: add node tl

let private elements (d: Dictionary<int, Tree list>) =
    seq {
        for kv in d do
            yield (kv.Key, kv.Value)
    }

// let (===) = fun x y -> Object.ReferenceEquals(x,y)

// Domain logic

let idOutOfRange max ({ RecordId=recId; ParentId=_ } as record) =
    if recId >= max then
        failwith (sprintf "Invalid record: %A. 'id' should be bigger than record set lenght." record)

let idShouldBigger ({ RecordId=recId; ParentId=parentId } as record) =
    if recId < parentId then
        failwith (sprintf "Invalid record: %A. 'id' should be bigger than 'parentId'." record)

let idDifferParentId ({ RecordId=recId; ParentId=parentId } as record) =
    // except for root element
    if recId <> 0 && parentId <> 0 && recId = parentId then
        failwith (sprintf "Direct cycle was detected for record: %A" record)

type TreeConstructor() =
    let mutable hasRoot = false
    let waitingParents = new Dictionary<int, Tree list>()

    member private __.change idx (newBranch: Tree list) =
        if waitingParents.Remove idx then
            waitingParents.Add (idx, newBranch)
        else
            failwith (sprintf "Waitings do not include parentId=%i" idx)

    // waitings must conatain 'parentId'
    member private this.group parentId (newLeaf: Tree) =
        let waitings = waitingParents.Item parentId

        let newBranch = if parentId = 0 then
                            match waitings with
                            | []                      -> failwith (sprintf "Waiting with id=%i withwout children" parentId)
                            | [Branch (id, children)] when id = 0
                                                      -> if newLeaf.id = 0 then
                                                             [Branch (0, ref waitings)]
                                                          else
                                                             [Branch (0, ref (add newLeaf !children))]
                            | [Leaf id] when id = 0   -> [Branch (0, ref [newLeaf])]
                            | _ as leaf               -> if newLeaf.id = 0 then
                                                             [Branch (0, ref leaf)]
                                                         else
                                                             add newLeaf waitings
                        else
                            add newLeaf waitings

        this.change parentId newBranch

    // 'parentId' must not be part of waiting ids
    member private this.placeLeaf parentId (newBranch: Tree) =
        let rec innerLoop (parent: Tree option) idx children =
            match children with
            | []              -> false
            | Leaf id as leaf :: rest
                              -> if parentId = id then
                                     match parent with
                                     | Some p -> let withoutParent = remove leaf !p.children
                                                 p.children := add (Branch(id, ref [newBranch])) withoutParent

                                     | None   -> let restBranch = waitingParents.Item idx
                                                                      |> remove leaf

                                                 let branch = add (Branch (id, ref [newBranch])) restBranch
                                                 this.change idx branch
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

        Seq.tryFind (fun (idx, children) -> if idx = 0 || parentId < idx then
                                                innerLoop None idx children
                                            else false)
                        (elements waitingParents) |> Option.isSome

    member private this.processRecId { RecordId=recId; ParentId=parentId } =
        if waitingParents.ContainsKey recId then
            let waitings = waitingParents.Item recId
            let branch = Branch (recId, ref waitings)

            if recId = 0 then // place root record
                this.change 0 [branch]
                None
            else
                waitingParents.Remove recId |> ignore
                Some (parentId, branch)
        else
            Some (parentId, Leaf recId)

    member private this.processParentId (newBranch: (int*Tree) option) =
        match newBranch with
        | None                    -> ()
        | Some (parentId, branch) -> if waitingParents.ContainsKey parentId then
                                        this.group parentId branch

                                     else
                                        if this.placeLeaf parentId branch then
                                            waitingParents.Remove parentId |> ignore
                                        else
                                            waitingParents.Add (parentId, [branch])

    member private this.add ({ RecordId=recId; ParentId=parentId } as record) =
        if recId = 0 && parentId = 0 then
            if hasRoot = false then hasRoot <- true
            else failwith "Root record is already placed"

        record
            |> this.processRecId
            |> this.processParentId

    member this.construct (records: Record list) =
        let max = List.length records

        let checkInput record =
            idOutOfRange max record; idShouldBigger record; idDifferParentId record

        List.iter (fun record -> checkInput record; this.add record) records

        if not hasRoot then failwith "Tree has no root."
        if waitingParents.Count > 1 then
             failwith (sprintf "There is detached branches with keys: %A" waitingParents.Keys)
        // resulted tree
        waitingParents.[0].[0]

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

let buildTree (records: Record list) :Tree =
    TreeConstructor().construct records
