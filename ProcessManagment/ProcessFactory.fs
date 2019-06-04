module ProcessFactory

open ProcessTypes

let private processMapping=[
    NewToDoItem.CheckAndCreateInstance;
]

let CreateProcess (startCmd:ProcessMessage)=
    let processes= 
        processMapping 
        |> List.map(fun f->f startCmd) 
        |> List.filter (fun res->
        match res with
        |Some p->true
        |None _->false)

    processes