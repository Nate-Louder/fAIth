namespace MathInterpreter

open System

module main =
    open Types.BasicOperations
    open Types.Stack
    open Types.Errors
    open Operation
    open Parse

    [<EntryPoint>]
    let main args =

        let mutable state = Ok { NumberStack = []; Variables = []; Functions = [] }

        while true do
            printf "💲 " |> ignore

            let input =
                Console.ReadLine().Split(" ")
                |> Array.toList
                |> parseOperation
                |> List.fold operationValidationCheck (Ok [])

            match input with
            | Ok validInput ->
                match List.fold matchElementType state validInput with
                | Ok x -> state <- Ok x
                | Error(FailedOperationAttempt(y, x)) ->
                    state <- Ok x // Sets the stack to the stack created before the error occured.
                    printfn "Operation %O failed with parameters %A" y x
                | Error DivideByZero -> printfn "Cannot divide by zero"
                | Error err -> 
                    printfn "%A" err
                    state <- state
            | Error e -> printfn "%O" e


        0
