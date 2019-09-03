module ProcessBus

open ToDoItemProcess
open Process.Infrastructure.Types
open PgSqlDapper.DAL
open Process.PgSql


let agent=MailboxProcessor.Start(fun inbox->
    let rec loop()=async {
        let! msgPack=inbox.Receive()

        //find process and start it
        //or fetch existing 
        let saveProcState res=
            let connStr=""
            use conn=Database.GetNewConnection connStr
            let saver= Repository.SaveNewProcess (fun()->conn)

            match res with
            |Ok newSt->saver newSt |> ignore
            |Error _->()

        let (msg, busSend)=msgPack

        match msg with 
        |Command cmd-> 
            (ProcessFactory.CreateProcess msg) 
            |> List.filter(fun p->p.IsSome) 
            |> List.map(fun p->p.Value)
            |> List.map(fun p->(p.Execute msg busSend) |> saveProcState)
            |>ignore //here state can be saved
        
        |_ ->()

        return! loop()    
    }
    loop()
)

let rec Send message=   
    let msg=(message,Send)
    agent.Post msg
