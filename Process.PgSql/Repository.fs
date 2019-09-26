namespace Process.PgSql

open System

module Repository=
    open SqlConnection
    open Dapper   
    open System.Data
    open Process.Infrastructure.Types
    open PgSqlDapper.DAL
    open Railway.Infrastructure.Operatorts

    type RawProcessState={ProcessId:Guid; Name:string; AwaitingMessages:string; ChangedDate:DateTime;IsSuccess:bool; CreationDate:DateTime; Error:string}


    let SaveNewProcess (getConnection:unit->IDbConnection) = //may be connectionString change to getConnection()
        //get sql
        let insSql=SqlScripts.InsertProcessDapper
        let conn=getConnection ()
        let saveProcess=Database.ExecuteRW insSql conn 
        let saveState=fun procData->
            let data={|procData.State with ProcessId=procData.Id; AwaitingMessages=procData.State.AwaitingMessages|> String.concat ";"|}
            Database.ExecuteRW SqlScripts.InsertNewStateDapper conn data
        saveProcess >=> saveState


    let UpdateState (getConnection:unit->IDbConnection) =
        let sql=SqlScripts.InsertNewStateDapper
        
        let fn= fun (processData:ProcessData)->
            let mergedState={| processData.State with ProcessId=processData.Id;AwaitingMessages=processData.State.AwaitingMessages|> String.concat ";"|}
            Database.Execute sql (getConnection()) mergedState
        fn

    let GetLastState (getConnection:unit->IDbConnection)=
        let sql=SqlScripts.GetLastState
        fun(processId:Guid) -> Database.ReadOne sql (getConnection()) {|Id=processId|}

    let GetProcess (getConnection:unit->IDbConnection) =
        let sql=SqlScripts.GetProcessWithStateById       
        
        
        let ss=Database.ReadOne2 sql (getConnection()) {|Id=Guid.Parse "b4019d72-d994-41f1-975c-4329e18da98a"|}

        let read= fun(proecssId:Guid)->Database.ReadOne<RawProcessState> sql (getConnection()) {|Id=proecssId|}

        let mapProcess (rawData:RawProcessState)=
            let awaitingMsgs=[|';'|] |> rawData.AwaitingMessages.Split |> List.ofArray 
            
            let (res:ProcessData)=  {
                Id=rawData.ProcessId; 
                State={AwaitingMessages=awaitingMsgs; ChangedDate=rawData.ChangedDate; IsSuccess=rawData.IsSuccess; Error=""};
                CreationDate=rawData.CreationDate;
                Name=rawData.Name
            }
            Ok(res)

        read >=> mapProcess
        

