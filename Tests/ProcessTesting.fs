module ProcessTesting

open NUnit.Framework
open FsUnit
open Swensen.Unquote

open ProcessCommands
open Process.Infrastructure.Types
open Process.PgSql

open System
open PgSqlDapper.DAL

let saveProcess processData connStr=
    //let connStr="Host=localhost;Port=5432;Database=TestProcesses;User Id=postgres;Password=123456;"
    use conn=Database.GetNewConnection connStr
    let saver= Repository.SaveNewProcess (fun()->conn)
    let res= saver processData
    //res |> should equal Ok

    match res with
    |Error _->failwith "Failed process saving"
    |_->()


[<Test>]
let ``ToDoItemProcess running test`` ()=
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}
    let newProcess= ToDoItemProcess.CheckAndCreateInstance message
    let successRes=Some(ToDoItemProcess.NewProcessInstance)
    newProcess |> should equal successRes


[<Test>]
let ``Test process saving``()=
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}
    let newProc=ToDoItemProcess.NewProcessInstance
    let processData={Id=Guid.NewGuid(); State=newProc.InitialState; CreationDate=DateTime.Now; Name=newProc.ProcessData.Name}
    let newState= newProc.HandleMessage message processData (fun a->())

    let testSaving newState=
        let connStr="Host=localhost;Port=5432;Database=TestProcesses;User Id=postgres;Password=123456;"
        use conn=Database.GetNewConnection connStr
        let saver= Repository.SaveNewProcess (fun()->conn)
        let res= saver newState
        res |> should equal Ok


    match newState with
    |Ok res-> testSaving res
    |_->failwith "Failed ProcessData!"

[<Test>]
let ``Change state after creating a new task``()=  
    let connStr="Host=localhost;Port=5432;Database=TestProcesses;User Id=postgres;Password=123456;"
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}   
    let newProc=ToDoItemProcess.NewProcessInstance
    


    let processData={Id=Guid.NewGuid(); State=newProc.InitialState; CreationDate=DateTime.Now; Name=newProc.ProcessData.Name}
    saveProcess processData connStr

    let res= newProc.HandleMessage message processData (fun a->())
    //res |> should equal Ok 

    
    match res with
    |Ok res->res.State.AwaitingMessages|>should equal [typeof<UpdateToDoItemCommand>.FullName; typeof<RemoveToDoItemCommand>.FullName]
    |_ ->failwith "Failed ProcessData!"

    




    