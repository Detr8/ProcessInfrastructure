namespace Process.PgSql

module SqlScripts =
    open Process.Infrastructure.Types

    let CreateTables=
        "create table if not exists \"ProcessStates\""

    let InsertProcessDapper =
        "insert into \"Processes\" (\"Id\", \"CreationDate\") values (@Id, @CreationDate)"


    let InsertNewStateDapper=
        "insert into \"ProcessStates\" (\"ProcessId\", \"AwaitingMessages\", \"ChangedDate\", \"IsSuccess\", \"Error\") values (@ProcessId, @AwaitingMessages, @ChangedDate, @IsSuccess, @Error)"

    let GetLastState=
        "select * from \"ProcessStates\" where \"ProcessId\"=@Id"

    let GetProcessWithStateById=
        """select 
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

module SqlConnection=
    open Npgsql
    open System.Data

    let GetConnection connStr:IDbConnection=
        let conn=new NpgsqlConnection(connStr)
        conn :> IDbConnection