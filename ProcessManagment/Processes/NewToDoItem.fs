module ToDoItemProcess

open ProcessTypes
open System
open ProcessCommands
open ToDoTypes

//handlers
let CreateNewItemHandler (saver:ToDoItem->unit) (logger:string->unit) (cmd:NewToDoItemCommand) currState:ProcessState =
    let newItem={Name=cmd.Name; Id=Guid.NewGuid();CreationDate=DateTime.Now}
    saver newItem
    sprintf "New item with Id=%A have been created" newItem.Id |> logger 
    let awaitingMessages=[nameof(UpdateItem); nameof(RemoveItem)];
    {currState with AwaitingMessages=[nameof(UpdateItem); nameof(RemoveItem)]; ChangedDate=DateTime.Now}


//end handlers


let startMessage=(typeof<NewToDoItemCommand>).FullName
let nextCommands=[
            (typeof<RemoveToDoItemCommand>).FullName;
            typeof<UpdateToDoItemCommand>.FullName
            ]
let nextEvents=[]
        

let GetCmdHandler msg=
    
    match msg with
    |Command cmd-> match cmd.Body with
        | :? NewToDoItemCommand as newItemCmd ->CreateNewItemHandler (ToDoItemScripts.SaveToDoItem) (fun logStr->()) newItemCmd//(fun () -> CreateNewItem (fun newItem->()) (fun logStr->()) newItemCmd)
        |_ -> fun state->state
    |_ ->fun state->state

let private HandleMessage msg (processData:ProcessData) busSend=
   //route command
   let handler=GetCmdHandler msg
   let newState= handler processData.State
   //return a new state
   newState





let initiaState={AwaitingMessages=[]; ChangedDate=DateTime.Now; IsSuccess=true;Error=""}

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
    


    