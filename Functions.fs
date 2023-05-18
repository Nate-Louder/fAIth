namespace MathInterpreter

open System
open Types.BasicOperations
open Types.Variables
open Types.Errors
open Types
open System.Text.RegularExpressions

module NumberOperations =
    let add (number1: Number) (number2: Number) : Number =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x + y)
        | Double(Some x), Double(Some y) -> Double <| Some(x + y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) + y)
        | Double(Some x), Int(Some y) -> Double <| Some(x + double (y))
        | _ -> Double None

    let subtract (number1: Number) (number2: Number) : Number =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x - y)
        | Double(Some x), Double(Some y) -> Double <| Some(x - y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) - y)
        | Double(Some x), Int(Some y) -> Double <| Some(x - double (y))
        | _ -> Double None

    let multiply (number1: Number) (number2: Number) : Number =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x * y)
        | Double(Some x), Double(Some y) -> Double <| Some(x * y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) * y)
        | Double(Some x), Int(Some y) -> Double <| Some(x * double (y))
        | _ -> Double None

    let divide (number1: Number) (number2: Number) =
        match number1, number2 with
        | Int(Some x), Int(Some y) -> Int <| Some(x / y)
        | Double(Some x), Double(Some y) -> Double <| Some(x / y)
        | Int(Some x), Double(Some y) -> Double <| Some(double (x) / y)
        | Double(Some x), Int(Some y) -> Double <| Some(x / double (y))
        | _ -> Double None

    let printfn number =
        match number with
        | Int(Some x) -> printfn "%i" x
        | Double(Some x) -> printfn "%f" x
        | _ -> ()

module StackOperations =
    let head list = List.head list
    let rest list = List.tail list
    let push stack value = (value :: stack)

    let add stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Add, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Add, stack)
        | x :: (y :: ys) ->
            Ok
                { stack with
                    NumberStack = (NumberOperations.add x y :: ys) }

    let add' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Add', stack)
        | x :: [] -> Error <| FailedOperationAttempt(Add', stack)
        | x :: xs ->
            Ok
                { stack with
                    NumberStack = ([ List.fold (fun sum element -> NumberOperations.add sum element) x xs ]) }

    let subtract stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Subtract, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Subtract, stack)
        | x :: (y :: ys) ->
            Ok
                { stack with
                    NumberStack = (NumberOperations.subtract x y :: ys) }

    let subtract' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Subtract', stack)
        | x :: [] -> Error <| FailedOperationAttempt(Subtract', stack)
        | x :: xs ->
            Ok
                { stack with
                    NumberStack =
                        ([ List.fold (fun inverseSum element -> NumberOperations.subtract inverseSum element) x xs ]) }

    let multiply stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Multiply, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Multiply, stack)
        | x :: (y :: ys) ->
            Ok
                { stack with
                    NumberStack = ((NumberOperations.multiply x y) :: ys) }

    let multiply' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Multiply', stack)
        | x :: [] -> Error <| FailedOperationAttempt(Multiply', stack)
        | x :: xs ->
            Ok
                { stack with
                    NumberStack =
                        ([ List.fold (fun product element -> NumberOperations.multiply product element) x xs ]) }

    let divide stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: (y :: ys) ->
            match y with
            | Int(Some 0) -> Error <| DivideByZero
            | Double(Some 0.) -> Error <| DivideByZero
            | _ ->
                Ok
                    { stack with
                        NumberStack = (NumberOperations.divide x y :: ys) }

    let divide' stack =
        match stack.NumberStack with
        | [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: [] -> Error <| FailedOperationAttempt(Divide, stack)
        | x :: xs ->
            match
                List.fold
                    (fun quotient element ->
                        match quotient, element with
                        | Error err, _ -> Error err
                        | Ok _, Int(Some 0) -> Error <| DivideByZero
                        | Ok _, Double(Some 0.) -> Error <| DivideByZero
                        | Ok q, e -> Ok(NumberOperations.divide q e))
                    (Ok x)
                    xs
            with
            | Ok x -> Ok { stack with NumberStack = [ x ] }
            | Error err -> Error err

    let popAndPrint stack =
        match stack.NumberStack with
        | [] ->
            printfn "Nothing to pop."
            Error <| FailedOperationAttempt(PAP, stack)
        | x :: xs ->
            NumberOperations.printfn x
            Ok { stack with NumberStack = xs }

    let popAndPrint' stack : Result<Stack, _> =
        match stack.NumberStack with
        | [] ->
            printfn "Nothing to pop."
            Error <| FailedOperationAttempt(PAP', stack)
        | _ ->
            List.map (fun x -> NumberOperations.printfn x) stack.NumberStack |> ignore
            Ok { stack with NumberStack = [] }

    let printStack stack =
        printfn "%A" stack.NumberStack
        Ok stack

module VariableOperations =

    let createVariable name stack =

        match List.tryFind (fun x -> name = x.Name) stack.Variables with
        | Some x -> Error <| VariableExistsAlready x.Name
        | None ->
            Ok
                { stack with
                    Variables = List.append stack.Variables [ { Name = name; Value = Int(None) } ] }

    let storeVariable
        name
        { NumberStack = numbers
          Variables = variables }
        =

        match List.tryFind (fun variable -> name = variable.Name) variables with
        | Some matchedVariable ->
            match numbers with
            | Int x :: _ ->
                Ok
                <| { NumberStack = List.tail numbers
                     Variables =
                       List.append (List.except [ matchedVariable ] variables) [ { Name = name; Value = Int x } ] }
            | Double x :: _ ->
                Ok
                <| { NumberStack = List.tail numbers
                     Variables =
                       List.append (List.except [ matchedVariable ] variables) [ { Name = name; Value = Double x } ] }
            | _ ->
                Error
                <| FailedOperationAttempt(
                    VStore name,
                    { NumberStack = numbers
                      Variables = variables }
                )
        | _ -> Error <| VariableDoesntExist name

    let fetchVariable
        name
        { NumberStack = numbers
          Variables = variables }
        =
        match List.tryFind (fun variable -> name = variable.Name) variables with
        | Some matchedVariable ->
            Ok
            <| { NumberStack = matchedVariable.Value :: numbers
                 Variables = variables }
        | _ -> Error <| VariableDoesntExist name

module Parse =
    open Types.Errors
    open Types.Variables

    let parseNumber (input: string) =
        match Int32.TryParse input with
        | (true, number) -> Ok <| Value(Int <| Some number)
        | (false, _) ->
            match Double.TryParse input with
            | (true, number) -> Ok <| Value(Double <| Some number)
            | (false, _) -> Error input

    let rec parseOperation input =
        match input with
        | x :: y ->
            match x with
            | "+" -> List.append [ Ok <| Operation Add ] (parseOperation y)
            | "+'" -> List.append [ Ok <| Operation Add' ] (parseOperation y)
            | "-" -> List.append [ Ok <| Operation Subtract ] (parseOperation y)
            | "-'" -> List.append [ Ok <| Operation Subtract' ] (parseOperation y)
            | "*" -> List.append [ Ok <| Operation Multiply ] (parseOperation y)
            | "*'" -> List.append [ Ok <| Operation Multiply' ] (parseOperation y)
            | "/" -> List.append [ Ok <| Operation Divide ] (parseOperation y)
            | "/'" -> List.append [ Ok <| Operation Divide' ] (parseOperation y)
            | "." -> List.append [ Ok <| Operation PAP ] (parseOperation y)
            | ".'" -> List.append [ Ok <| Operation PAP' ] (parseOperation y)
            | "PRINT" -> List.append [ Ok <| Operation Print ] (parseOperation y)
            | "EXIT" -> List.append [ Ok <| Operation Exit ] (parseOperation y)
            | "VARIABLE" -> List.append [ Ok <| Operation(VCreate(List.head y)) ] (parseOperation (List.tail y))
            | "FUN" ->
                match y with
                | y :: ys -> 
                    let name = List.head y
                    let functionOperationsStr =
                    List.append
                | y :: [] -> Error <| InvalidFunctionDeffinition
            | _ ->
                match y with
                | "@" :: _ -> List.append [ Ok <| Operation(VFetch x) ] (parseOperation (List.tail y))
                | "!" :: _ -> List.append [ Ok <| Operation(VStore x) ] (parseOperation (List.tail y))
                | _ -> List.append [ parseNumber x ] (parseOperation y)
        | _ -> []

module Operation =
    open StackOperations
    open VariableOperations

    let tryPerformOperarion (stack: Stack) operation =
        match operation with
        | Add -> add stack
        | Add' -> add' stack
        | Subtract -> subtract stack
        | Subtract' -> subtract' stack
        | Multiply -> multiply stack
        | Multiply' -> multiply' stack
        | Divide -> divide stack
        | Divide' -> divide' stack
        | PAP -> popAndPrint stack
        | PAP' -> popAndPrint' stack
        | Print -> printStack stack
        | Exit -> exit 0
        | VCreate name -> createVariable name stack
        | VStore name -> storeVariable name stack
        | VFetch name -> fetchVariable name stack

    let matchSingleOperationType stack processElement =
        match stack with
        | Ok stack ->
            match processElement with
            | Operation o -> tryPerformOperarion stack o
            | Value v ->
                Ok
                    { stack with
                        NumberStack = (v :: stack.NumberStack) }
        | Error e -> Error e
