module ProcessFactory

open Process.Infrastructure.Types
open PgSqlDapper.DAL
open Process.PgSql
open ProcessManagment

let private processMapping=[
    ToDoItemProcess.CheckAndCreateInstance;
]

let processNameMap=["ToDoItemProcess", ToDoItemProcess.NewProcessStartWithState;] |> Map.ofList

let CreateProcess (startCmd:ProcessMessage)=
    let processes= 
        processMapping 
        |> List.map(fun f->f startCmd) 
        |> List.filter (fun res->
            match res with
            |Some p->true
            |None _->false)

    processes 

let RestoreProcess processId=
    let connection=Database.GetNewConnection Config.ConnectionString
    let getter= Repository.GetProcess (fun()->connection)

    let res= getter processId
    match res with
    |Ok  procData->
        match processNameMap.TryFind(procData.Name) with
        |Some f->Some(f procData)
        |None _->None
    |_->None