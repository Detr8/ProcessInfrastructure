// Learn more about F# at http://fsharp.org

open System
open ProcessCommands
open InfrastructureTypes
open ProcessTypes

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    
    let newItemCmd= {
        Name="New1";
        //Process={ProcessId=None};
    }

   
    let msg= ToDoItemProcess.ConvertCmdToMessage (ToDoItemCommands.NewToDoItem newItemCmd)

    ProcessBus.Send msg

    0 // return an integer exit code
