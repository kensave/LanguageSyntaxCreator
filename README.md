Language Syntax Parser
=
 Based on the LanguageCreator project created by Anton Kropp. I'm having fun trying to parse different programming languages or pieces of them.
 
  Parsing programming languages is always a challege, every language has it's own flavor and Syntax.
 
 LanguageSyntaxParser takes advantage of how flexible JSON could be and allows you to define all the Language Syntax in a JSON file by specifing the Keywords, Special Characters and Patterns of the language and avoiding you the process of creating an entire AST to parse a piece of code. 
 
 Since it follows a pattern matching aproach you don't have to define the entire language syntax if you are going to do something simple. So in order to parse just the code you want, it will be required to specifuy only the syntax used by that specific piece of code.
 
 How Does it work?
===

 Lets take a look an small example I did, to parse only the CREATE of Store Procedures inside SQL files. For this example, I needed to retrieve all the Store Procedures parameters to determine the type and also the default value if present.
 
 As you might think already I did not want to parse all the SQL syntax a variations but instead I just wanted to find the CREATE statement and extract the parameters and default values.
 
 So...What did I do?
 
Steps
===
 1. Create a JSON file with the name of the Language to parse.
 2. Add the Language to the Languages Enum.
 3. Define the Keywords and Special characters
 4. Define the patterns to find.
 5. Parse.
 6. An JSON ast is going to be generated.
 7. Use it :)
 
Patterns used to parse the SQL files
===
```
"Syntax": {
    "Document": "<^{CreateExpression}>*",
    "CreateExpression": "{Create} {Entity} {QualifiedIdentifier} {Parameters}",
    "QualifiedIdentifier": "<{Identifier}, {Dot}*>",
    "Parameters": "<{Param}, {Comma}*>",
    "Param": "{Identifier} {Type} {ParamDefaultValue}*",
    "Argument": "({LiteralExpression}|{Identifier})",
    "Arguments": "{OpenParenth}<{Argument},{Comma}*>{CloseParenth}",
    "ParamDefaultValue": "{Equals} ({QuotedString}|{Float}|{Int}|{Identifier}) as Value",
    "Type": "{Identifier} {Arguments}*",
    "LiteralExpression": "({QuotedString}|{Float}|{Int}) as Value",
    "Entity": "({Proc}|{Procedure}|{Table}|{Function}|{Database}) as Value"
  }
  ```
  
Syntax Explained
=== 
Syntax     | Purpose
-------- | ---
<>     | Sequence of elements
()     | Group
{}     | Back reference to syntax, special char, keyword or custom keyword
\*    | Nullable
\|     | Or, in an Or only the first element found will match
as    | Alias for groups and sequences
^ |  It means it will ignore everything before our syntax token

Example of generated AST
===
```
{
  "Document": [
    {
      "CreateExpression": {
        "Entity": "PROCEDURE",
        "QualifiedIdentifier": [
          {
            "Identifier": "[dbo]"
          },
          {
            "Identifier": "[Test]"
          }
        ],
        "Parameters": [
          {
            "Param": {
              "Identifier": "@Test_ID",
              "Type": {
                "Identifier": "char",
                "Arguments": [
                  {
                    "Argument": {
                      "LiteralExpression": "16"
                    }
                  }
                ]
              }
            }
          },
          {
            "Param": {
              "Identifier": "@Other_ID",
              "Type": {
                "Identifier": "int"
              }
            }
          },
          {
            "Param": {
              "Identifier": "@Current_Test_Rate",
              "Type": {
                "Identifier": "float"
              },
              "ParamDefaultValue": "0"
            }
          },
          {
            "Param": {
              "Identifier": "@Deferred_BOY_Rate",
              "Type": {
                "Identifier": "numeric",
                "Arguments": [
                  {
                    "Argument": {
                      "LiteralExpression": "15"
                    }
                  },
                  {
                    "Argument": {
                      "LiteralExpression": "0"
                    }
                  }
                ]
              },
              "ParamDefaultValue": "null"
            }
          },
          {
            "Param": {
              "Identifier": "@Deferred_Test_Rate",
              "Type": {
                "Identifier": "float"
              },
              "ParamDefaultValue": "0"
            }
          }
        ]
      }
    }
  ]
}
```

Pretty Print AST
===
 You can Pretty Print the AST by specifying the Pretty Print Rules, like this:
 ```
 "PrettyPrint": {
    "Document": "<{CreateExpression}>*",
    "QualifiedIdentifier": "<{Identifier},{Dot}*>",
    "CreateExpression": "{Create}{Space}{Entity}{Space}{QualifiedIdentifier}{Parameters}",
    "Parameters": "<{Space}{Param}, {Comma}*>",
    "Param": "{Identifier}{Space}{Type}{ParamDefaultValue}*",
    "Argument": "({LiteralExpression}|{Identifier})",
    "Arguments": "{OpenParenth}<{Argument},{Comma}*>{CloseParenth}",
    "ParamDefaultValue": "{Space}{Equals}{Space}{Value}",
    "Type": "{Identifier}{Arguments}*"
  }
 ```
  The Syntax is almos the same used to describe the Language Syntax itself.
  
Example of Pretty Print Output
===
 ```
 CREATE PROCEDURE [dbo].[Test] @Test_ID char(16), @Other_ID int, @Current_Test_Rate float = 0, @Deferred_BOY_Rate numeric(15,0) = null, @Deferred_Test_Rate float = 0
 ```
Implemented Languages
===

-Create Store Procedure SQL parsing.

