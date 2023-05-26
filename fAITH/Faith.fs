namespace Faith

module Helpers = 

    open Parse
    open Types.State
    open Operation
    open System

    let splitLine (line: string) =
        line.Split(" ")
        |> Array.toList

    let processInput input =
       
        let state = Ok { NumberStack = []; Variables = []; Functions = []; Depth = 0 }

        let inputList = 
            splitLine input
            |> parseOperation
            |> List.fold operationValidationCheck (Ok [])

        match inputList with
        | Ok validInput ->     
            match List.fold matchElementType state validInput with
            | Ok returnState -> Ok returnState
            | Error err -> Error err
        | Error err -> Error err

    let processInputConsole =
       
        let mutable state = Ok { NumberStack = []; Variables = []; Functions = []; Depth = 0}
        
        while true do 
            printf "💲 "
            
            match Console.ReadLine() |> splitLine |> parseOperation |> List.fold operationValidationCheck (Ok []) with
            |Ok validInput ->     
                match List.fold matchElementType state validInput with
                | Ok returnState -> state <- Ok returnState
                | Error err -> printfn "%A" err
            |Error err -> printfn "%A" err


        
        

module Faith =
    open Helpers

    let faith input = processInput input 
    let faithConsole = processInputConsole

