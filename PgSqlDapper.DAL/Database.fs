namespace PgSqlDapper.DAL

open System

module Database =
    open System.Data
    open Dapper
    open Npgsql
    

    let GetNewConnection connStr:IDbConnection=
        let conn=new NpgsqlConnection(connStr)
        conn :> IDbConnection

    let Execute<'T> sqlScript (connection:IDbConnection) (data:'T)=
        try            
            let res= connection.Execute(sqlScript, data)
            Ok(res)
        with
        |ex->Error(ex.Message)

    let ExecuteRW<'T> sqlScript (connection:IDbConnection) (data:'T)=
        match Execute sqlScript connection data with
        |Ok r->Ok(data)
        |Error er->Error(er)

    let ReadOne<'a> sqlScript (connection:IDbConnection) param=
        try               
            let res=connection.Query<'a>(sqlScript, param)
            Ok(Seq.head res)
        with
        |ex->Error(ex.Message)


    type RawProcessState={ProcessId:Guid; AwaitingMessages:string; ChangedDate:DateTime;IsSuccess:bool; Error:string; CreationDate:DateTime}
    let ReadOne2 (sqlScript:string) (connection:IDbConnection) param=
        let _sql="""select 
        "ProcessId", 
        "AwaitingMessages",
        "ChangedDate",
        "IsSuccess",
        "Error",
        "CreationDate"
        from "ProcessStates" 
        join "Processes" on "Processes"."Id"="ProcessStates"."ProcessId"
        where "ProcessId"=@Id 
        order by "ChangedDate" desc limit 1"""
        try               
            let res=connection.QueryFirst<RawProcessState>(_sql, param)
            Ok(res)
        with
        |ex->Error(ex.Message)
