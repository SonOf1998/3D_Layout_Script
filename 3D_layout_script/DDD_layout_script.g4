grammar DDD_layout_script;

EQ:             '=';
PLUS:           '+';    PLUS_ASS:   '+=';
MINUS:          '-';    MINUS_ASS:  '-=';
MULT:           '*';    MULT_ASS:   '*=';
DIV:            '/';    DIV_ASS:    '/=';
MOD:            '%';
COMMA:          ',';
COLON:          ':';
SEMI:           ';';
CURLY_O:        '{';
CURLY_C:        '}';
SQUARE_O:       '[';
SQUARE_C:       ']';
BRACKET_O:      '(';
BRACKET_C:      ')';


fragment LEQ:            '<=';
fragment LT:             '<';
fragment GEQ:            '>=';
fragment GT:             '>';
fragment IS_EQ:          '==';
fragment IS_NEQ:         '!=';

FOR:            'for';
IN:             'in';
IF:             'if';
ELSE:           'else';
STEP:           'step';
INCLUDE:        '@include';

fragment QUAD:           'quad';
fragment TRIANGLE:       'triangle';
fragment CIRCLE:         'circle';
fragment SPHERE:         'sphere';
fragment HEMISPHERE:     'hemisphere';
fragment CUBE:           'cube';
fragment CUBOID:         'cuboid';
fragment CONE:           'cone';
fragment CYLINDER:       'cylinder';

OBJECT_TYPE: QUAD | TRIANGLE | CIRCLE | SPHERE | HEMISPHERE | CUBE | CUBOID | CONE | CYLINDER;

ATTR_GROUP:     'attr-group';
STRING:         '\'' [a-zA-Z0-9_-]+ '\'';

CONST:  'const';
VAR:    'var';

ID:         '$'[a-zA-Z][a-zA-Z0-9_]*;
signed_id:  ('-' | '+')? ID;

fragment INT_KW:    'Int';
fragment FLOAT_KW:  'Float';
fragment VEC3_KW:   'Vec3';

TYPE: INT_KW | FLOAT_KW | VEC3_KW;
    INT:        ('-' | '+')? [0-9]+;
    FLOAT:      ('-' | '+')? [0-9]*'.'[0-9]+('f')?;
    vec3:       ('-' | '+')? SQUARE_O operation COMMA operation COMMA operation SQUARE_C;
type_val: INT | FLOAT | vec3;


binary_op:          PLUS | MINUS | MULT | DIV | MOD;
other_binary_op:    PLUS_ASS | MINUS_ASS | MULT_ASS | DIV_ASS;

xyz:			signed_id ('.x' | '.y' | '.z');
modifiable_xyz: ID ('.x' | '.y' | '.z');
simple_expression:      signed_id | type_val | xyz;
simple_modifyable_exp:  ID | modifiable_xyz;
operation_helper:		simple_expression | (BRACKET_O operation BRACKET_C);
operation:              (operation_helper binary_op)* operation_helper;
variable_decl:          (CONST)? VAR ID (COLON TYPE)?  (EQ operation)? SEMI;
assign_statement:       (simple_modifyable_exp EQ operation SEMI) |
                        (simple_modifyable_exp other_binary_op operation SEMI);

ATTRIBUTE:          'radius' | 'position' | 'height' | 'width' | 'depth' | 'rotation-axis' | 'rotation-angle' | 'quality';
attribute_value:    operation | STRING;
attr:               ATTRIBUTE COLON attribute_value SEMI;
attr_list:          (attr)+;
attr_group:         ATTR_GROUP BRACKET_O STRING BRACKET_C CURLY_O attr_list CURLY_C;

include_statement:  INCLUDE BRACKET_O STRING BRACKET_C SEMI;
object_content:     (include_statement)* attr*;
object_block:       OBJECT_TYPE CURLY_O object_content CURLY_C;

COMP_OP:			LEQ | LT | GEQ | GT | IS_EQ | IS_NEQ;
if_condition:       BRACKET_O operation COMP_OP operation BRACKET_C;
if_content:         variable_decl | assign_statement | object_block | for_loop | if_statement;
if_statement:       IF if_condition CURLY_O if_content* CURLY_C
                    else_if_statement*
                    else_statement?;
else_if_statement:  ELSE IF if_condition CURLY_O if_content* CURLY_C;
else_statement:		ELSE CURLY_O if_content* CURLY_C;		

for_loop_statement:     variable_decl | assign_statement | object_block | for_loop | if_statement;
range_and_step:         (BRACKET_O | SQUARE_O) operation COMMA operation (BRACKET_C | SQUARE_C) (STEP operation)?;
for_loop:               FOR BRACKET_O VAR ID IN range_and_step BRACKET_C
                        CURLY_O for_loop_statement* CURLY_C;


program_statement:  variable_decl | assign_statement | attr_group | object_block | for_loop | if_statement;
program:            program_statement*;

WHITESPACE:     [ \t\n\r]           -> skip;
BLOCK_COMMENT:  '/*' .*? '*/'       -> channel(HIDDEN);
LINE_COMMENT:   '//' .*? '\r'? '\n' -> channel(HIDDEN);
