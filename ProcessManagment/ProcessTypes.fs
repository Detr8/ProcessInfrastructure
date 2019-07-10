module ProcessTypes

open System


type ProcessId=Option<Guid>

//type ProcessCommand=
//    |ToDoItemCommand of ToDoItemCommands


type CommandData={ProcessId:ProcessId; }//по идее по этой штуке определяем какой процесс будет обрабатывать ф
type ProcessCommand={Data:CommandData; Body:obj}//саму доменную команду кладем в body

type ProcessMessage=
    |Command of ProcessCommand
    |ProcessEvent
//union of commands from all processes





type ActionId=string   
type ProcessState={AwaitingMessages:ActionId list; ChangedDate:DateTime;IsSuccess:bool; Error:string}
type ProcessData={Id:Guid; State:ProcessState; CreatedDate:DateTime; }//save in the store

type Process={
    //Id:Guid;
    //CreatedAt:DateTime;
    StartMessage:ActionId;
    ProcessData:ProcessData;
    InitialState:ProcessState;
    //NextCommands:ActionId list;
    //NextEvents:ActionId list;
    HandleMessage: ProcessMessage->ProcessData->(ProcessMessage->unit)->ProcessState;
} with 
    member this.Execute msg busSend=
        //check msg before handle

        let newState=this.HandleMessage msg this.ProcessData busSend

        {this.ProcessData with State=newState;}
        

