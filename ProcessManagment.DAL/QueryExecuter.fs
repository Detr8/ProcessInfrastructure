﻿module QueryExecuter

open System.Data.SqlClient
open System.Data
open Dapper


let executeRead<'TEntiy> (connection:IDbConnection) sql args =
    connection.Query<'TEntiy>(sql,args)


let executeWrite (connection:IDbConnection) sql args=
    connection.Execute(sql, args)


let CreateReader<'TEntity> connection=
    executeRead connection

let CreateWriter connection=
    executeWrite connection


//flow: getFunc-> getSql(build or get from patterns)->getExecuter->execute sql-> 