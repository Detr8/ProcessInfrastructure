module ProcessCommands

type ToDoItemCommands=
    |NewToDoItem of string
    |RemoveItem 
    |UpdateItem