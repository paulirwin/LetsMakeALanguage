// Give our grammar a name. This ends up being used for the generated classes, i.e. FizzyParser
grammar Fizzy;

// ******************************************************************
//  Begin parser rules. Note how they start with a lowercase letter.
// ******************************************************************

// Entry point for parsing. A program consists of zero or more statements 
// or blank lines, followed by the end of the file.
// Note the Regex-like syntax.
program: 
    (statement | NEWLINE)* EOF;

// This rule enforces that all statement types end with a newline.
statement: 
    statementType NEWLINE;
    
// This rule switches on the types of statements that our language supports.
// A statement can either be a variable declaration, a while loop, OR 
// an expression statement (see below).
statementType:
    variableDeclarationStatement
    | expressionStatement
    | whileStatement;
    
// A statement that declares a new variable and assigns it to the result 
// of the expression. Note that you can't declare a new variable in an
// expression, only via this statement.
// i.e. x := 4 + y
variableDeclarationStatement: 
    identifier VARIABLE_DECLARE expression;
    
// A statement that evaluates an expression and yields it. The last yielded
// value is essentially "returned." This allows for calling void functions
// like print_line, or returning a value in the REPL.
// i.e. print_line("foo") 
expressionStatement:
    expression;
    
// A while loop that continuously executes the block as long as it matches the condition.
// i.e.:
// while x < 4 do
// begin
//     print_line(x)
// end
whileStatement:
    WHILE condition DO NEWLINE blockStatement;
    
// Technically this rule isn't strictly needed, but it allows us to consider conditions 
// separately from general expressions, which helps in parsing.
condition:
    expression;
    
// Represents an expression that yields a value (or void).
// Note that binary/ternary expressions are not able to be refactored out into separate rules,
// otherwise you get an error about left recursion. All other expression types fall under
// simpleExpression.
// i.e. x <= 100
expression:
    simpleExpression
    | expression MODULO expression
    | expression ADDITION expression
    | expression SUBTRACTION expression
    | expression MULTIPLICATION expression
    | expression DIVISION expression
    | expression EQUALS expression
    | expression LT expression
    | expression LTE expression
    | expression GT expression
    | expression GTE expression;
    //| expression IF condition ELSE expression;
    
// Represents a simple (non-left-recursive) expression. See sub-rules for examples.
simpleExpression:
    literalExpression
    | identifierExpression
    | assignmentExpression
    | parenthesizedExpression
    | callExpression;
    
// An integer or string literal.
// i.e. 42
// i.e. "foo"
literalExpression:
    INTEGER
    | STRING;

// An expression that references an identifier (such as a variable) and yields its value.
// i.e. x
identifierExpression:
    identifier;
    
// An expression that assigns the result of an expression to an identifier (that must
// already have been declared previously), and yields the result.
// i.e. x = x + 1
assignmentExpression:
    identifier ASSIGN expression;
    
// An expression wrapped in parenthesis to help with readability or order of operations.
// The result of the expression is yielded when evaluated.
// i.e. (x % 3)
parenthesizedExpression:
    LPAREN expression RPAREN;
    
// An expression that calls a function with zero or more arguments. The return value
// of the function is the result of the expression (or void).
// i.e. print_line("foo")
callExpression:
    identifier LPAREN argumentList RPAREN;
    
// Not exactly a proper "statement" currently. Represents a sequence of other statements.
// Note that currently blocks do not define child scopes, as all variables are currently
// global. Used only by the while loop syntax.
blockStatement:
    BEGIN NEWLINE (statement | NEWLINE)* END;
    
// A sub-rule representing zero or more arguments (separated by a comma) for a callExpression. 
// i.e. 1, "foo", 3
argumentList:
    expression? (COMMA expression)*;

// An identifier for i.e. a variable or function name. A sub-rule used by other expressions above.
// i.e. x
identifier:
    IDENTIFIER;
    
// ******************************************************************
//  Begin lexer rules. Note how they start with an uppercase letter.
//  Rule names like If and Else would be allowed, but we're just
//  using an all uppercase convention.
// ******************************************************************

IF: 'if';
ELSE: 'else';
BEGIN: 'begin';
END: 'end';
DO: 'do';
WHILE: 'while';
VARIABLE_DECLARE: ':=';
EQUALS: '==';
MODULO: '%';
ADDITION: '+';
SUBTRACTION: '-';
MULTIPLICATION: '*';
DIVISION: '/';
ASSIGN: '=';
LPAREN: '(';
RPAREN: ')';
COMMA: ',';
LT: '<';
LTE: '<=';
GT: '>';
GTE: '>=';

INTEGER: [0-9]+;
IDENTIFIER: [a-zA-Z_]+;

STRING: '"' ( ~'"' | '\\' '"' )* '"';

NEWLINE: '\r\n' | '\n';

LINE_COMMENT: '//' ~[\r\n]* -> skip;

WHITESPACE: (' ' | '\t')+ -> channel(HIDDEN);

GARBAGE: . ;