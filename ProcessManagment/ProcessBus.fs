module ProcessBus

open ToDoItemProcess
open Process.Infrastructure.Types
open PgSqlDapper.DAL
open Process.PgSql
open System
open Railway.Infrastructure.Operatorts
open ProcessManagment





let execFunc p busSend connection=  
    
    let saveProcess cmd=
        let procData:ProcessData=p.ProcessData//{Id=newProcessId; State=p.InitialState; CreationDate=DateTime.Now}
        let saver= Repository.SaveNewProcess (fun()->connection)
        match saver procData with
        |Ok _->Ok(Command {cmd with Data={ProcessId=Some(procData.Id)}})
        |Error err->Error(err)

    let execNext newCmd=
        p.Execute busSend newCmd

    let saveProcess2 newState=
        Repository.UpdateState (fun()->connection) newState


    saveProcess >=> execNext >=> saveProcess2   

//need testing
let fakePost msg=
    let (msg, busSend)=msg
    
    match msg with 
    |Command cmd->
        match cmd.Data.ProcessId with
        |Some processId-> //existing process case
            let exisitingProcess= ProcessFactory.RestoreProcess cmd.Data.ProcessId.Value
            match exisitingProcess with
            |Some proc->
                use connection=Database.GetNewConnection Config.ConnectionString
                let saver=execFunc proc busSend connection
                saver cmd |> ignore   
            |None _->()
        |None _->
            (ProcessFactory.CreateProcess msg) //check processId before
            |> List.filter(fun p->p.IsSome) 
            |> List.map(fun p->p.Value)
            |> List.map(fun p->
                let connStr=Config.ConnectionString
                use connection=Database.GetNewConnection connStr
                let saver=execFunc p busSend connection
                saver cmd |> ignore            
            )|>ignore 
    
         
    |_ ->()
    

let agent=MailboxProcessor.Start(fun inbox->

    let rec loop()=async {
        let! msgPack=inbox.Receive()


        let (msg, busSend)=msgPack

        match msg with 
        |Command cmd-> 
            (ProcessFactory.CreateProcess msg) //check processId before
            |> List.filter(fun p->p.IsSome) 
            |> List.map(fun p->p.Value)
            |> List.map(fun p->              
                let connStr="Host=localhost;Port=5432;Database=TestProcesses;User Id=postgres;Password=123456;"
                use connection=Database.GetNewConnection connStr
                let saver=execFunc p busSend connection
                saver cmd |> ignore
                ()
            )
               
                
            |>ignore //here state can be saved
        
        |_ ->()
        
        return! loop()    
    }
    
    let ff=loop() 

    ff |> Async.RunSynchronously
)

let rec Send message=   
    let msg=(message,Send)
    //agent.Post msg 
    fakePost msg