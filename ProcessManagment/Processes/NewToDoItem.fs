module ToDoItemProcess

open ProcessTypes
open System
open ProcessCommands




//let private ExecuteInnerCmd (cmd:ProcessCommand)=
//    match cmd with 
//    |ToDoItemCommand todoCmd->()
//    ()

let startMessage=(typeof<NewToDoItemCommand>).FullName
let nextCommands=[
            (typeof<RemoveToDoItemCommand>).FullName;
            typeof<UpdateToDoItemCommand>.FullName
            ]
let nextEvents=[]
        


let private HandleMessage msg (processData:ProcessData)=
   //route command

   //create a new item
   //log it
   //save it in to db

   //create a new state
   {AwaitingMessages=[];ChangedDate=DateTime.Now}



let initiaState={AwaitingMessages=[]; ChangedDate=DateTime.Now}

let NewProcessInstance:Process=
    {
        StartMessage=startMessage;
        ProcessData={Id=Guid.NewGuid(); State=initiaState; CreatedDate=DateTime.Now};
        InitialState=initiaState;
        HandleMessage= fun msg procData-> HandleMessage msg procData;
    }

//Check start conditions
let CheckAndCreateInstance (msg:ProcessMessage):Option<Process>=
    //if true then Some(NewProcessInstance)
    //else None
    
    match msg with
    |Command cmd->        
        match cmd.Body with
        | :? NewToDoItemCommand -> Some(NewProcessInstance)
        |_ ->None
            //match tdCmd with
            //|NewToDoItem _->Some(NewProcessInstance)

    |_ ->None

//let ConvertCmdToMessage (innerCmd:ToDoItemCommands):ProcessMessage=
//    match innerCmd with
//    |NewToDoItem c-> ProcessMessage.Command ({ProcessId=None;},   ProcessCommand.ToDoItemCommand (ToDoItemCommands.NewToDoItem c) )
    


    