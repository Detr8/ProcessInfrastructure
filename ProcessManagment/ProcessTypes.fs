module ProcessTypes

open System

open InfrastructureTypes
open StartCommands




//union of commands from all processes





type ActionId=string
    

type Process={
    Id:Guid;
    CreatedAt:DateTime;
    NextActions:ActionId list;
    Execute: ProcessMessage->unit;
}

