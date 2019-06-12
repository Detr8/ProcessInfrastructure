module InfrastructureTypes

open System
open StartCommands
open ProcessCommands

type ProcessData={
    ProcessId:Option<Guid>
}
type ProcessCommand=
    |ToDoItemCommand of ToDoItemCommands

type ProcessMessage=
    |Command of (ProcessData*ProcessCommand)
    |ProcessEvent
