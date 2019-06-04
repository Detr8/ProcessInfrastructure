module NewToDoItem

open ProcessTypes
open System
open ProcessCommands




let private ExecuteInnerCmd (cmd:ProcessCommand)=
    match cmd with 
    |ToDoItemCommand todoCmd->()
    ()



let private ExecuteHandler msg=
            match msg with 
            | Command cmd->()
            //| ProcessEvent ()->()

let NewProcessInstance:Process=
    {
        Id=Guid.NewGuid();
        CreatedAt=DateTime.Now;
        //StartingCommand=NewItem;
        WaitingActions=[];
        Execute=ExecuteHandler;
        
    }

let CheckAndCreateInstance (msg:ProcessMessage):Option<Process>=
    if true then Some(NewProcessInstance)
    else None