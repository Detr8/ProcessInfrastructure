﻿namespace Process.Infrastructure

module Types =
    open System


    type ProcessId=Option<Guid>


    type CommandData={ProcessId:ProcessId; }//по идее по этой штуке определяем какой процесс будет обрабатывать ф
    type ProcessCommand={Data:CommandData; Body:obj}//саму доменную команду кладем в body



    type ProcessMessage=
        |Command of ProcessCommand// * ProcessId
        |ProcessEvent
    //union of commands from all processes





    type ActionId=string   
    type ProcessState={AwaitingMessages:ActionId list; ChangedDate:DateTime;IsSuccess:bool; Error:string}
    type ProcessData={Id:Guid; State:ProcessState; CreationDate:DateTime; Name:string;}//save in the store

    type Process={
        //Id:Guid;
        //CreatedAt:DateTime;
        //StartMessage:ActionId;
        ProcessData:ProcessData;
        InitialState:ProcessState;
        
        //NextCommands:ActionId list;
        //NextEvents:ActionId list;
        HandleMessage: ProcessMessage->ProcessData->(ProcessMessage->unit)->Result<ProcessData,string>;
    } with 
        member this.Execute busSend msg =
            //check msg before handle
            try
                let newState=this.HandleMessage msg this.ProcessData busSend
                match newState with
                |Ok newState-> Ok(newState)
                |Error err->Error(err)

            with
            |ex->Error(ex.Message)
        


