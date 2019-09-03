namespace ToDoDomain

open System.Data
open PgSqlDapper.DAL

module Scripts=

    open ToDoTypes

    let private getConnection=
        Database.GetNewConnection("")

    let private saveToDoItem insertQuery updateQuery (queryExecuter:string->bool) (item:ToDoItem)=
        ()
    
    let SaveToDoItem ()=
        let sql=""
        use conn=getConnection
        Database.Execute<ToDoItem> sql conn

//let SaveToDoItem (item:ToDoItem)=
//    //get insert q
//    let insertQ=""
    
//    //get update q
//    let updateQ=""

//    saveToDoItem insertQ updateQ item

