// Learn more about F# at http://fsharp.org

open System
open ProcessCommands
open Process.Infrastructure.Types

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    
    let message = Command {Data={ProcessId=None}; Body={Name="A new task1"}}
    ProcessBus.Send message
   
    //let msg=ProcessMessage.Command {Data={ProcessId=None;}; Body=newItemCmd}

    //ProcessBus.Send msg

    0 // return an integer exit code
