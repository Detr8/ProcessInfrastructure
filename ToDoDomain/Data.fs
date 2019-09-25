namespace ToDoDomain

module Data=
    let GetConnection connStr=
        PgSqlConnection.GetConnection connStr

