module ProcessBus

open ToDoItemProcess
open ProcessTypes
open InfrastructureTypes

let Send message=
    match message with
    |Command c->()
    |_ ->()

    let newProcesses= ProcessFactory.CreateProcess message
    newProcesses |> List.map (fun p->p.Value) |> List.iter (fun p->p.Execute message)

    //load saved processes
    //execute existing processes

    ()