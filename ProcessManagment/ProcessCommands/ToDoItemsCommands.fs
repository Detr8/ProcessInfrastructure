module ProcessCommands

//open InfrastructureTypes
open System

type NewToDoItemCommand={
    Name:string;
}

type RemoveToDoItemCommand={
    ItemId:Guid;
}

type UpdateToDoItemCommand={
    ItemId:Guid;
    NewName:string
}

type ToDoItemCommands=
    |NewToDoItem of NewToDoItemCommand
    |RemoveItem of RemoveToDoItemCommand
    |UpdateItem