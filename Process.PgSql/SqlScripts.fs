namespace Process.PgSql

module SqlScripts =
    open Process.Infrastructure.Types

    let CreateTables=
        "create table if not exists \"ProcessStates\""

    let InsertProcessDapper =
        "insert into \"Processes\" (\"Id\", \"CreationDate\") values (@Id, @CreationDate)"


    let InsertNewStateDapper=
        "insert into \"ProcessStates\" (\"Id\", \"ProcessesId\", \"AwaitingMessages\", \"ChangedDate\", \"IsSuccess\", \"Error\") values (@Id, @ProcessesId, @AwaitingMessages, @ChangedDate, @IsSuccess, @Error)"

    let GetLastState=
        "select * from \"ProcessStates\" where \"ProcessId\"=@Id"

module SqlConnection=
    open Npgsql
    open System.Data

    let GetConnection connStr:IDbConnection=
        let conn=new NpgsqlConnection(connStr)
        conn :> IDbConnection