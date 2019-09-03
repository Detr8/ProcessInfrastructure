namespace Railway.Infrastructure

module Operatorts=
    
    let (>=>) f1 f2 arg=
        match f1 arg with
        | Ok data-> f2 data
        | Error e->Error e
