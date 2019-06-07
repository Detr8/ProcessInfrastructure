module ProcessTypes

open System
open ProcessCommands
open InfrastructureTypes




//union of commands from all processes
type ProcessCommand=
    |ToDoItemCommand of ToDoItemCommands

type ProcessMessage=
    |Command of (ProcessData*ProcessCommand)
    |ProcessEvent


type ActionId=string
    

type Process={
    Id:Guid;
    CreatedAt:DateTime;
    NextActions:ActionId list;
    Execute: ProcessMessage->unit;
}

