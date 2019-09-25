namespace ToDoDomain

open Npgsql
open System.Data

module PgSqlScripts=
    let InsertNew="insert into \"ToDoItems\" (\"Name\", \"CreationDate\", \"ProcessId\") values (@Name, @CreationDate, @ProcessId)"

module PgSqlConnection=
    let GetConnection connStr=
        let conn=new NpgsqlConnection(connStr)
        conn :> IDbConnection

