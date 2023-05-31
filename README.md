# fAIth Interpreter for Basic Math Computation

This repository contains an interpreter written in F# that enables basic math computation, variable creation, function definition, and printing functionality. The interpreter follows a stack-based approach, where numbers (integer or double) are stored onto a stack, popped off for computation, and the resulting answer is placed back onto the stack.

## Key Features

- **Basic Math Computation:** Perform arithmetic operations such as addition, subtraction, multiplication, and division on numbers.
- **Variable Creation:** Define and assign values to variables, allowing for storing and retrieval of data during interpretation.
- **Function Definition:** Create custom functions with parameters, enabling code modularity and reuse.
- **Printing:** Output results or variables to the console for visual representation and debugging.

## How it works

1. The interpreter reads and tokenizes input code, breaking it down into individual components.
2. Numbers and variables are stored onto a stack, which acts as a temporary storage for computations.
3. Mathematical operations are performed by popping operands from the stack, calculating the result, and pushing it back onto the stack.
4. Variables are stored in a dictionary-like structure, allowing for efficient retrieval and updating.
5. Functions are defined using a syntax that supports "parameter passing" and execution.
6. The interpreter handles printing functionality, displaying results or variables to the console.

## Usage

1. Clone or download the repository.
2. Build and run the interpreter using an F# compiler or development environment.
3. Write code in an external .fth file using the provided syntax for math computation, variable creation, function definition, and printing.
4. Execute the interpreter on the code passing the file path of your fAIth file, and observe the output or printed results.

## Get Started

Explore the codebase and the provided examples to understand how the interpreter handles basic math computation, variable creation, function definition, and printing. Refer to the documentation for further details on the syntax and usage.

### Stack

Numbers are added to the front of the stack as they are read by the interpreter, and popped off the front as they are needed. Adding numbers to the stack can be done in one of three ways.

1. Numbers are written in line - The line  
 ``` 1 2 3 ```  
 results in a stack that looks something like ``` [3, 2, 1] ```.

2. Operations are performed on the stack - performing the operation ``` + ``` on a stack of ``` [3, 2, 1] ``` will result in ``` 3 and 2 ``` popping off the stack and ``` 5 ``` getting pushed to the stack. The stack will them appear like so ``` [5, 1] ``` .

3. A variable is fetched - Performing the operation ``` {variable-name} @ ``` will push the value assigned to the variable to the stack. Therefore if Variable x has a value of 2.1, calling ``` x @ ``` will result in a stack that looks something like ``` [2.1] ```.

### Operations 

There are currently _ operations that fAIth will recognize. 

#### Add
 ``` + ``` : Adds the first element of the stack to the second element of the stack and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2]
    command: +
    process: 1 + 2
    stack: [3]

Example 2)
    stack: []
    command: 1 2 +
    process: 2 + 1
    stack: [3]
```

#### Add'
 ``` +' ``` : Adds every element of the list together recursivley (Adds the first two elements) and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2, 3]
    command: +'
    process: ((1 + 2) + 3)
    stack: [6]

Example 2)
    stack: []
    command: 1 2 3 +'
    process: ((3 + 2) + 1)
    stack: [6]
```

#### Subtract
 ``` - ``` : Subracts the second element of the stack from the first element of the stack and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [3, 1]
    command: -
    process: 3 - 1
    stack: [2]

Example 2)
    stack: []
    command: 3 1 -
    process: 1 - 3
    stack: [-2]
```

#### Subtract'
 ``` -' ``` : Subtracts every element of the list recursivley (Subtracts the first two elements) and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2, 3]
    command: -'
    process: ((1 - 2) - 3)
    stack: [-4]

Example 2)
    stack: []
    command: 1 2 3 -'
    process: ((3 - 2) - 1)
    stack: [0]
```

#### Multiply
 ``` * ``` : Multiplies the first element of the stack to the second element of the stack and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2]
    command: *
    process: 1 * 2
    stack: [2]

Example 2)
    stack: []
    command: 1 2 *
    process: 2 * 1
    stack: [2]
```

#### Multiply'
 ``` *' ``` : Multiplies every element of the list together recursivley (Multiplies the first two elements) and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [1, 2, 5]
    command: *'
    process: ((1 * 2) * 5)
    stack: [10]

Example 2)
    stack: []
    command: 1 2 5 *'
    process: ((5 * 2) * 1)
    stack: [10]
```

#### Divide
 ``` / ``` : Divides the second element of the stack from the first element of the stack and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [6, 2]
    command: /
    process: 6 / 2
    stack: [3]

Example 2)
    stack: []
    command: 6 2 /
    process: 2 / 6
    stack: [0.33]
```

#### Divide'
 ``` /' ``` : Divides every element of the list recursivley (Divides the first two elements ) and pushes the result to the stack. e.x.
```fs
Example 1)
    stack: [12, 3, 2]
    command: /'
    process: ((12 / 3) / 2)
    stack: [2]

Example 2)
    stack: []
    command: 12 3 2 /'
    process: ((2 / 3) / 12)
    stack: [0.56]
```

#### Pop
 ``` . ``` : Pops the top element from the stack and prints it to the console.
```fs
Example 1)
    stack: [1, 2]
    command: .
    output: "1"
    stack: [2]

Example 2)
    stack: []
    command: 1 2 .
    output: "2"
    stack: [1]
```

#### Pop'
 ``` .' ``` : Pops every element from the stack and prints them to the console.
```fs
Example 1)
    stack: [1, 2, 3]
    command: .'
    output: "1 2 3"
    stack: []

Example 2)
    stack: []
    command: 1 2 3 .'
    output: "3 2 1"
    stack: []
```

#### Print
 ``` PRINT ``` : Prints the current stack.
```fs
Example 1)
    stack: [1, 2.0]
    command: PRINT
    output: "[INT(1), DOUBLE(2.0)]"
    stack: [1, 2.0]

Example 2)
    stack: []
    command: 1 2.0 PRINT
    output: "[DOUBLE(2.0), INT(1)]"
    stack: [2.0, 1]
```

#### Exit
 ``` EXIT ``` : Exits the program.
```fs
Example 1)
    stack: [1, 2, 3]
    command: EXIT
    process: Program ends
```

### Variables

fAIth supports the use of variables by assigning names with values. It allows those names to interact with the stack by either popping an element of the stack and assigning it to their value, or pushing their value to the stack. Variables cannot be redefined and all variables exist at a global scope. Their values can be reassigned and they are loosely typed, so a variable with an Integer value can be reassigned a Double value. Below are the operations that can be used to create, modify, and use variables.

#### Create Variable
 ``` VARIABLE <variable name> ``` : Creates a variable with no value and a name ```<variable name>```
```fs
Example 1)
    stack: [1]
    command: VARIABLE FOO
    variable list: Variables = [{name="FOO", value=None}]
    stack: [1]
```

#### Store Variable
 ``` <variable name> ! ``` : Pops the top element from the stack and assigns it as the value of ```<variable name>```
```fs
Example 1)
    stack: [3, 2]
    variable list: Variables = [{name="FOO", value=None}]
    command: FOO !
    variable list: Variables = [{name="FOO", value=3}]
    stack: [2]

Example 2)
    stack: []
    variable list: Variables = [{name="FOO", value=None}]
    command: 3 2 FOO !
    variable list: Variables = [{name="FOO", value=2}]
    stack: [3]

Example 3)
    stack: [2, 3]
    variable list: Variables = [{name="FOO", value=7.0}]
    command: FOO !
    variable list: Variables = [{name="FOO", value=2}]
    stack: [3]
```

#### Fetch Variable
 ``` <variable name> @ ``` : Pushes the value of ```<variable name>``` to the stack.
```fs
Example 1)
    stack: [2]
    variable list: Variables = [{name="FOO", value=3}]
    command: FOO @
    variable list: Variables = [{name="FOO", value=3}]
    stack: [3, 2]

Example 2)
    stack: []
    variable list: Variables = [{name="FOO", value=None}]
    command: 3 2 FOO @
    output: RuntimeError(UnassignedVariableError "FOO")
    stack: []
```

### Functions
Functions are implemented in fAIth by assigning blocks of code to a name. The code can be ran simply by calling the name of the function. Parameters are not dirrectly supported, but since all variables are within the global scope, they can be used to pass values in and out of functions. Recursion is possible, but condition matching has not yet been implemented. Recursion will likely cause a stack overflow error which is set to trigger at a call stack depth of 100 function calls by default. Below are the operarations to define and use functions.

#### Create Function
 ``` FUN <function name> <function operations> END ``` : Creates a function named ```<function name>``` to perform ```<function operations> ``` on the stack. 
```fs
Example 1)
    stack: []
    command: FUN BAR 1 2 + END
    functions list: Functions = [{name="BAR", operations="1 2 +"}]
    stack: []
```

#### Use Function
 ``` <function name> ``` : Uses function ```<function name>```'s operations on the stack. 
```fs
Example 1)
    stack: []
    functions list: Functions = [{name="BAR", operations="1 2 +"}]
    command: BAR
    process: 2 + 1
    stack: [3]

Example 2)
    stack: [7, 4, 3]
    functions list: Functions = [{name="PrintFirst2" operations=". ."}]
    command: PrintFirst2
    output: "7 4"
    stack: [3]
```