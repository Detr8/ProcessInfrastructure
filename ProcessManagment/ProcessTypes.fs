module ProcessTypes

open System
open ProcessCommands

//union of commands from all processes
type ProcessCommand=
    |ToDoItemCommand of ToDoItemCommands

type ProcessMessage=
    |Command of ProcessCommand
    |ProcessEvent



    

type Process={
    Id:Guid;
    CreatedAt:DateTime;
    WaitingActions:ProcessMessage list;

    Execute: ProcessMessage->unit;
}

