// Learn more about F# at http://fsharp.org

open System
open ProcessCommands
open ProcessTypes

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    
    let newItemCmd= {
        Name="New1";
    }

   
    let msg=ProcessMessage.Command {Data={ProcessId=None;}; Body=newItemCmd}

    ProcessBus.Send msg

    0 // return an integer exit code
