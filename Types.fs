namespace MathInterpreter

module Types =

    type Number =
        | Int of int option
        | Double of double option

    module Variables =
        type Variable = { Name: string; Value: Number }

    module BasicOperations =
        open Variables

        type Operation =
            | Add
            | Add'
            | Subtract
            | Subtract'
            | Multiply
            | Multiply'
            | Divide
            | Divide'
            | PAP
            | PAP'
            | Print
            | Exit
            | VCreate of string
            | VFetch of string
            | VStore of string
            | FCreate of string * string list
            | FUse of string

        type ProcessElement =
            | Operation of Operation
            | Value of Number

    module Functions =
        open BasicOperations

        type Function =
            { Name: string
              Elements: ProcessElement list }

    module Stack =
        open Functions
        open Variables

        type Stack =
            { NumberStack: Number list
              Variables: Variable list
              Functions: Function list }

    module Errors =
        open BasicOperations
        open Stack


        type OperationError =
            | FailedOperationAttempt of Operation * Stack
            | DivideByZero
            | UnableToParseInput of string
            | VariableExistsAlready of string
            | InvalidVariableType of string
            | VariableDoesntExist of string
            | InvalidFunctionDeffinition
            | FunctionAlreadyExists of string
            | FunctionDoesntExist of string
