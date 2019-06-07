module ToDoItemProcess

open ProcessTypes
open System
open ProcessCommands
open InfrastructureTypes




let private ExecuteInnerCmd (cmd:ProcessCommand)=
    match cmd with 
    |ToDoItemCommand todoCmd->()
    ()



let private HandleMessage msg=
            match msg with 
            | Command cmd->()
            //| ProcessEvent ()->()

let NewProcessInstance:Process=
    {
        Id=Guid.NewGuid();
        CreatedAt=DateTime.Now;
        //StartingCommand=NewItem;
        NextActions=[
            (typeof<RemoveToDoItemCommand>).FullName;
            typeof<UpdateToDoItemCommand>.FullName
            ]
        //WaitingActions=[];
        Execute=HandleMessage;
        
    }

let CheckAndCreateInstance (msg:ProcessMessage):Option<Process>=
    //if true then Some(NewProcessInstance)
    //else None
    
    match msg with
    |Command cmd->
        
        match cmd with
        |(_,ToDoItemCommand tdCmd)->
            match tdCmd with
            |NewToDoItem _->Some(NewProcessInstance)

    |_ ->None

let ConvertCmdToMessage (innerCmd:ToDoItemCommands):ProcessMessage=
    match innerCmd with
    |NewToDoItem c-> ProcessMessage.Command ({ProcessId=None;},   ProcessCommand.ToDoItemCommand (ToDoItemCommands.NewToDoItem c) )
    


    