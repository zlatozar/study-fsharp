module GradeSchool

type School = Map<int, string list>

let empty: School = Map.empty

let add (student: string) (grade: int) (school: School): School = 
    match Map.tryFind grade school with
    | Some(lst)  -> Map.add grade (List.sort (lst @ [student])) school
    | None       -> Map.add grade [student] school

let roster (school: School): (int * string list) list =
    List.sort (Map.toList school)

let grade (number: int) (school: School): string list = 
    match Map.tryFind number school with
    | Some(lst)  -> lst
    | None       -> []
