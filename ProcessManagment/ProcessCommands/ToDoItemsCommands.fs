module ProcessCommands

open InfrastructureTypes
open System

type NewToDoItemCommand={
    Name:string;
    Process:ProcessData;
}

type RemoveToDoItemCommand={
    ItemId:Guid;
    Process:ProcessData;
}

type ToDoItemCommands=
    |NewToDoItem of NewToDoItemCommand
    |RemoveItem of RemoveToDoItemCommand
    |UpdateItem