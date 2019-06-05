module ProcessBus

let Send message=
    let newProcesses= ProcessFactory.CreateProcess message
    newProcesses |> List.map (fun p->p.Value) |> List.iter (fun p->p.Execute message)

    //load saved processes
    //execute existing processes

    ()