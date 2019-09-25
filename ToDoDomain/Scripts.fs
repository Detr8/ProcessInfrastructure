namespace ToDoDomain

open System.Data
open PgSqlDapper.DAL

module Scripts=

    open ToDoTypes


    let SaveNewToDoItem (getConnection:unit->IDbConnection)=
        let sql=PgSqlScripts.InsertNew
        Database.ExecuteRW<ToDoItem> sql (getConnection ())
