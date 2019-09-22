module ToDoItemProcess


open System
open ProcessCommands
open ToDoTypes
open Process.Infrastructure.Types
open ToDoDomain
open Railway.Infrastructure.Operatorts




//handlers
let CreateNewItemHandler getConnection (logger:string->unit) (cmd:NewToDoItemCommand) currState =
    let newItem={Name=cmd.Name; Id=Guid.NewGuid();CreationDate=DateTime.Now}
    sprintf "New item with Id=%A have been created" newItem.Id |> logger 

    let successRes={currState with AwaitingMessages=[typeof<UpdateToDoItemCommand>.FullName; typeof<RemoveToDoItemCommand>.FullName]; ChangedDate=DateTime.Now}
    let save (state:ProcessState)=
        let newItem={Name=cmd.Name; Id=Guid.NewGuid();CreationDate=DateTime.Now}
        ToDoDomain.Scripts.SaveToDoItem getConnection newItem

    let retSucces (item:ToDoItem)=Ok(successRes)

    save >=> retSucces

let GetCmdHandler msg (processData:ProcessData)=
    
    match msg with
    |Command cmd-> match cmd.Body with
        | :? NewToDoItemCommand as newItemCmd ->CreateNewItemHandler ToDoDomain.Scripts.getConnection (fun logStr->()) newItemCmd processData.State //should use chain of func cmd >=> next cmd
        |_ -> fun state->Ok(state)
    |_ ->fun state->Ok(state)

let private HandleMessage msg (processData:ProcessData) busSend=
   //route command
   let handler=GetCmdHandler msg processData
   let newState= handler processData.State
   //return a new state
   newState





let initiaState={AwaitingMessages=[]; ChangedDate=DateTime.Now; IsSuccess=true;Error=""}

let NewProcessInstance:Process=
    {
        //StartMessage=startMessage;
        ProcessData={Id=Guid.NewGuid(); State=initiaState; CreationDate=DateTime.Now};
        InitialState=initiaState;
        HandleMessage= fun msg procData-> HandleMessage msg procData;
        Name="ToDoItemProcess"
    }

let NewProcessStartWithState processData=
    {
        //StartMessage=startMessage;
        ProcessData=processData;
        InitialState=initiaState;
        HandleMessage= fun msg procData-> HandleMessage msg procData;
        Name="ToDoItemProcess"
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
    


    