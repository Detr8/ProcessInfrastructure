namespace Process.PgSql

open System

module Repository=
    open SqlConnection
    open Dapper   
    open System.Data
    open Process.Infrastructure.Types
    open PgSqlDapper.DAL
    open Railway.Infrastructure.Operatorts

    let SaveNewProcess (getConnection:unit->IDbConnection) = //may be connectionString change to getConnection()
        //get sql
        let insSql=SqlScripts.InsertProcessDapper
        let conn=getConnection ()
        let saveProcess=Database.Execute insSql conn 
        let saveState=Database.Execute SqlScripts.InsertNewStateDapper conn
        saveProcess >=> saveState


    let UpdateState (getConnection:unit->IDbConnection) =
        let sql=SqlScripts.InsertNewStateDapper
        
        let fn= fun (processId:Guid) (state:ProcessState) ->
            let mergedState={| state with ProcessId=processId|}
            Database.Execute sql (getConnection()) mergedState
        fn

    let GetLastState (getConnection:unit->IDbConnection)=
        let sql=SqlScripts.GetLastState
        fun(processId:Guid) -> Database.ReadOne sql (getConnection()) {|Id=processId|}
        

