module ProcessBus

open ToDoItemProcess
open ProcessTypes



let agent=MailboxProcessor.Start(fun inbox->
    let rec loop()=async {
        let! msg=inbox.Receive()

        //find process and start it
        //or fetch existing 

        match msg with 
        |Command cmd-> 
            (ProcessFactory.CreateProcess msg) 
            |> List.filter(fun p->p.IsSome) 
            |> List.map(fun p->p.Value)
            |> List.map(fun p->p.Execute msg)
            |>ignore //here state can be saved
        
        |_ ->()

        return! loop()    
    }
    loop()
)

let Send message=
    //match message with
    //|Command c->()
    //|_ ->()

    //let newProcesses= ProcessFactory.CreateProcess message
    //newProcesses |> List.map (fun p->p.Value) |> List.iter (fun p->p.Execute message)
    agent.Post message

    //load saved processes
    //execute existing processes

    ()