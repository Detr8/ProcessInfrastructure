module ProcessBus

open ToDoItemProcess
open ProcessTypes



let agent=MailboxProcessor.Start(fun inbox->
    let rec loop()=async {
        let! msgPack=inbox.Receive()

        //find process and start it
        //or fetch existing 
        
        let (msg, busSend)=msgPack

        match msg with 
        |Command cmd-> 
            (ProcessFactory.CreateProcess msg) 
            |> List.filter(fun p->p.IsSome) 
            |> List.map(fun p->p.Value)
            |> List.map(fun p->p.Execute msg busSend)
            |>ignore //here state can be saved
        
        |_ ->()

        return! loop()    
    }
    loop()
)

let rec Send message=   
    let msg=(message,Send)
    agent.Post msg
