module ToDoItemProcess


open System
open ProcessCommands
open ToDoTypes
open Process.Infrastructure.Types
open ToDoDomain
open Railway.Infrastructure.Operatorts


let GetConnection()=
    let connStr="Host=localhost;Port=5432;Database=TestProcesses;User Id=postgres;Password=123456;"
    ToDoDomain.Data.GetConnection(connStr)

//handlers
let CreateNewItemHandler getConnection (logger:string->unit) (cmd:NewToDoItemCommand) (currProcess:ProcessData) =
    let newItem={Name=cmd.Name; Id=0;CreationDate=DateTime.Now;ProcessId=currProcess.Id}
    sprintf "New item with Id=%A have been created" newItem.Id |> logger 


    let newState={currProcess.State with AwaitingMessages=[typeof<UpdateToDoItemCommand>.FullName; typeof<RemoveToDoItemCommand>.FullName]; ChangedDate=DateTime.Now}
    let save (state:ProcessData)=
        //let newItem={Name=cmd.Name; Id=0;CreationDate=DateTime.Now}
        let res= ToDoDomain.Scripts.SaveNewToDoItem getConnection newItem
        res

    let retSucces (item:ToDoItem)=Ok({currProcess with State=newState})

    save >=> retSucces

let GetCmdHandler msg (processData:ProcessData)=
    
    match msg with
    |Command cmd-> match cmd.Body with
        | :? NewToDoItemCommand as newItemCmd ->CreateNewItemHandler GetConnection (fun logStr->()) newItemCmd processData //should use chain of func cmd >=> next cmd
        |_ -> fun state->Ok(state)
    |_ ->fun state->Ok(state)

let private HandleMessage msg (processData:ProcessData) busSend=
   //route command
   let handler=GetCmdHandler msg processData
   let newState= handler processData
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
    


    