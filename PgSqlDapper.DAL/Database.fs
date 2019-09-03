namespace PgSqlDapper.DAL

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

    let ExecuteRW sqlScript (connection:IDbConnection) data=
        match Execute sqlScript connection data with
        |Ok r->Ok(data)
        |Error er->Error(er)

    let ReadOne<'a> sqlScript (connection:IDbConnection) param=
        try
            let res=connection.QuerySingle<'a>(sqlScript, param)
            Ok(res)
        with
        |ex->Error(ex.Message)
