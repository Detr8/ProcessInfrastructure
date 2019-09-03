module ProcessTesting

open NUnit.Framework
open FsUnit
open Swensen.Unquote

open ProcessCommands
open Process.Infrastructure.Types
open Process.PgSql

open System
open PgSqlDapper.DAL

[<Test>]
let ``ToDoItemProcess running test`` ()=
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}
    let newProcess= ToDoItemProcess.CheckAndCreateInstance message
    let successRes=Some(ToDoItemProcess.NewProcessInstance)
    newProcess |> should equal successRes

[<Test>]
let ``Change state after creating a new task``()=  
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}    
    let newProc=ToDoItemProcess.NewProcessInstance

    let processData={Id=Guid.NewGuid(); State=newProc.InitialState; CreatedDate=DateTime.Now}


    let newState= newProc.HandleMessage message processData (fun a->())
    newState.AwaitingMessages|>should equal [typeof<UpdateToDoItemCommand>.FullName; typeof<RemoveToDoItemCommand>.FullName]


[<Test>]
let ``Test process saving``()=
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}    
    let newProc=ToDoItemProcess.NewProcessInstance
    let processData={Id=Guid.NewGuid(); State=newProc.InitialState; CreatedDate=DateTime.Now}
    let newState= newProc.HandleMessage message processData (fun a->())

    let connStr=""
    use conn=Database.GetNewConnection connStr
    let saver= Repository.SaveNewProcess (fun()->conn)
    let res= saver {processData with State=newState}
    res |> should equal Ok