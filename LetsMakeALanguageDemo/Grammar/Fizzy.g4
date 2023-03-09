grammar Fizzy;

// Parser rules

program: 
    (statement | NEWLINE)* EOF;

statement: 
    statementType NEWLINE;
    
statementType:
    variableDeclarationStatement
    | expressionStatement
    | whileStatement;
    
variableDeclarationStatement: 
    identifier VARIABLE_DECLARE expression;
    
expressionStatement:
    expression;
    
whileStatement:
    WHILE condition DO NEWLINE blockStatement;
    
condition:
    expression;
    
expression:
    simpleExpression
    | expression MODULO expression
    | expression ADDITION expression
    | expression EQUALS expression
    | expression LT expression
    | expression LTE expression
    | expression GT expression
    | expression GTE expression;
    //| expression IF condition ELSE expression;
    
simpleExpression:
    literalExpression
    | identifierExpression
    | assignmentExpression
    | parenthesizedExpression
    | callExpression;
    
literalExpression:
    INTEGER
    | STRING;
    
identifierExpression:
    identifier;
    
assignmentExpression:
    identifier ASSIGN expression;
    
parenthesizedExpression:
    LPAREN expression RPAREN;
    
callExpression:
    identifier LPAREN argumentList RPAREN;
    
blockStatement:
    BEGIN NEWLINE (statement | NEWLINE)* END;
    
argumentList:
    expression? (COMMA expression)*;
        
identifier:
    IDENTIFIER;
    
// Lexer rules

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