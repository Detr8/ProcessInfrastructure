module ProcessTesting

open NUnit.Framework
open FsUnit
open Swensen.Unquote

open ProcessTypes
open ProcessCommands

open System

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
    newState.AwaitingMessages|>should equal [nameof(UpdateItem); nameof(RemoveItem)]