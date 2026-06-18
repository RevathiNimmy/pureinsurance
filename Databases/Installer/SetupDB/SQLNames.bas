Attribute VB_Name = "MSQLNames"
' Module:   SQL naming and scripting functions
' Shared:   Yes
'
Option Explicit

Public Enum ESQLNameTypes
    knUnknown
    knServer
    knDatabase
    knUser
    knTable     ' name is optionally allowed to begin with #
    knCode
    knColumn
    knConstraint
    knVariable  ' name is optionally allowed to begin with @
End Enum

' Standard indent for all generated scripts.
Public Const ksIndent = "    "

' The list of all identifiers reserved by SQL Server and SQL-92.
Private m_vasReservedWords As Variant ' String()

' Make a valid NTFS file name.
Public Function MakeFileName(ByVal sName As String) As String

    Dim sOwnerName As String
    Dim sObjectName As String

    sName = Trim$(sName)
    If Len(sName) = 0 Then
        Exit Function
    End If

    sObjectName = ParseObjectName(sName, sOwnerName)

    If Len(sOwnerName) <> 0 Then
        sObjectName = sOwnerName & "." & sObjectName
    End If

    MakeFileName = sObjectName

End Function

' Make a valid SQL identifier name.
Public Function MakeObjectName(ByVal sName As String, ByVal nNameType As ESQLNameTypes) As String

    Dim sOwnerName As String
    Dim sObjectName As String

    sName = Trim$(sName)
    If Len(sName) = 0 Then
        Exit Function
    End If

    sObjectName = ParseObjectName(sName, sOwnerName)

    If NameNeedsDelimiting(sOwnerName, knUser) Then
        sOwnerName = "[" & sOwnerName & "]"
    End If
    If NameNeedsDelimiting(sObjectName, nNameType) Then
        sObjectName = "[" & sObjectName & "]"
    End If

    If Len(sOwnerName) <> 0 Then
        sObjectName = sOwnerName & "." & sObjectName
    End If

    MakeObjectName = sObjectName

End Function

' Make a valid SQL string literal.
Public Function MakeStringLiteral(ByVal sName As String, ByVal nNameType As ESQLNameTypes) As String

    Dim sOwnerName As String
    Dim sObjectName As String

    sName = Trim$(sName)
    If Len(sName) = 0 Then
        Exit Function
    End If

    sObjectName = ParseObjectName(sName, sOwnerName)

    If Len(sOwnerName) <> 0 Then
        sObjectName = sOwnerName & "." & sObjectName
    End If

    MakeStringLiteral = ToSQL(sObjectName)

End Function

' Make a valid part of a larger SQL identifier name.
Public Function MakePartOfName(ByVal sName As String, ByVal nNameType As ESQLNameTypes) As String

    Dim sOwnerName As String
    Dim sObjectName As String

    sName = Trim$(sName)
    If Len(sName) = 0 Then
        Exit Function
    End If

    sObjectName = ParseObjectName(sName, sOwnerName)

    sOwnerName = ReplaceBadCharacters(sOwnerName, knUser)
    sObjectName = ReplaceBadCharacters(sObjectName, nNameType)

    If Len(sOwnerName) <> 0 Then
        sObjectName = sOwnerName & "__" & sObjectName
    End If

    MakePartOfName = sObjectName

End Function

Private Function ParseObjectName(ByVal sName As String, ByRef o_sOwnerName As String) As String

    Const ksDatabaseOwner = "dbo"

    Dim sOwnerName As String
    Dim sObjectName As String

    ParseObjectName = ""
    o_sOwnerName = ""
    sName = Trim$(sName)
    If Len(sName) = 0 Then
        Exit Function
    End If

    If Left$(sName, 1) = "[" Then
        sName = Mid$(sName, 2)
        sOwnerName = ParseSep(sName, sName, "]")
        If Left$(sName, 1) = "." Then
            sName = Mid$(sName, 2)
            If Left$(sName, 1) = "[" Then
                sName = Mid$(sName, 2)
                sObjectName = ParseSep(sName, sName, "]")
            Else
                sObjectName = sName
            End If
        End If
    Else
        sOwnerName = ParseSep(sName, sName, ".")
        If Left$(sName, 1) = "[" Then
            sName = Mid$(sName, 2)
            sObjectName = ParseSep(sName, sName, "]")
        Else
            sObjectName = sName
        End If
    End If

    If Len(sObjectName) = 0 Then
        sObjectName = sOwnerName
        sOwnerName = ""
    End If

    If LCase$(sOwnerName) = ksDatabaseOwner Then
        sOwnerName = ""
    End If

    ParseObjectName = sObjectName
    o_sOwnerName = sOwnerName

End Function

Private Sub InitialiseReservedWords()

    '' The combined list of all reserved words in all DBMSs we know about.
    'm_vasReservedWords = Array("ABSOLUTE", "ACTION", "ADA", "ADD", "ADMIN", "AFTER", "AGGREGATE", "ALIAS", "ALL", "ALLOCATE", "ALTER", "AND", "ANY", "ARE", "ARRAY", "AS", "ASC", "ASSERTION", "AT", "AUTHORIZATION", "AVG", "BACKUP", "BEFORE", "BEGIN", "BETWEEN", "BINARY", "BIT", "BIT_LENGTH", "BLOB", "BOOLEAN", "BOTH", "BREADTH", "BREAK", "BROWSE", "BULK", "BY", "CALL", "CASCADE", "CASCADED", "CASE", "CAST", "CATALOG", "CHAR", "CHAR_LENGTH", "CHARACTER", "CHARACTER_LENGTH", "CHECK", "CHECKPOINT", "CLASS", "CLOB", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE", "COLLATION", "COLUMN", "COMMIT", "COMPLETION", "COMPUTE", "CONNECT", "CONNECTION", "CONSTRAINT", "CONSTRAINTS", "CONSTRUCTOR", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CORRESPONDING", "COUNT", "CREATE", "CROSS", "CUBE", "CURRENT", "CURRENT_DATE", "CURRENT_PATH", "CURRENT_ROLE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "CYCLE", "DATA", "DATABASE", "DATE", "DAY", "DBCC", "DEALLOCATE", "DEC", "DECIMAL", "DECLARE", _
        "DEFAULT", "DEFERRABLE", "DEFERRED", "DELETE", "DENY", "DEPTH", "DEREF", "DESC", "DESCRIBE", "DESCRIPTOR", "DESTROY", "DESTRUCTOR", "DETERMINISTIC", "DIAGNOSTICS", "DICTIONARY", "DISCONNECT", "DISK", "DISTINCT", "DISTRIBUTED", "DOMAIN", "DOUBLE", "DROP", "DUMMY", "DUMP", "DYNAMIC", "EACH", "ELSE", "END", "END_EXEC", "EQUALS", "ERRLVL", "ESCAPE", "EVERY", "EXCEPT", "EXCEPTION", "EXEC", "EXECUTE", "EXISTS", "EXIT", "EXTERNAL", "EXTRACT", "FALSE", "FETCH", "FILE", "FILLFACTOR", "FIRST", "FLOAT", "FOR", "FOREIGN", "FORTRAN", "FOUND", "FREE", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION", "GENERAL", "GET", "GLOBAL", "GO", "GOTO", "GRANT", "GROUP", "GROUPING", "HAVING", "HOLDLOCK", "HOST", "HOUR", "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IGNORE", "IMMEDIATE", "IN", "INCLUDE", "INDEX", "INDICATOR", "INITIALIZE", "INITIALLY", "INNER", "INOUT", "INPUT", "INSENSITIVE", "INSERT", "INT", "INTEGER", "INTERSECT", "INTERVAL", "INTO", "IS", "ISOLATION", "ITERATE", "JOIN", "KEY", _
        "KILL", "LANGUAGE", "LARGE", "LAST", "LATERAL", "LEADING", "LEFT", "LESS", "LEVEL", "LIKE", "LIMIT", "LINENO", "LOAD", "LOCAL", "LOCALTIME", "LOCALTIMESTAMP", "LOCATOR", "LOWER", "MAP", "MATCH", "MAX", "MIN", "MINUTE", "MODIFIES", "MODIFY", "MODULE", "MONTH", "NAMES", "NATIONAL", "NATURAL", "NCHAR", "NCLOB", "NEW", "NEXT", "NO", "NOCHECK", "NONCLUSTERED", "NONE", "NOT", "NULL", "NULLIF", "NUMERIC", "OBJECT", "OCTET_LENGTH", "OF", "OFF", "OFFSETS", "OLD", "ON", "ONLY", "OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET", "OPENXML", "OPERATION", "OPTION", "OR", "ORDER", "ORDINALITY", "OUT", "OUTER", "OUTPUT", "OVER", "OVERLAPS", "PAD", "PARAMETER", "PARAMETERS", "PARTIAL", "PASCAL", "PATH", "PERCENT", "PLAN", "POSITION", "POSTFIX", "PRECISION", "PREFIX", "PREORDER", "PREPARE", "PRESERVE", "PRIMARY", "PRINT", "PRIOR", "PRIVILEGES", "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READS", "READTEXT", "REAL", "RECONFIGURE", "RECURSIVE", "REF", "REFERENCES", "REFERENCING", "RELATIVE", _
        "REPLICATION", "RESTORE", "RESTRICT", "RESULT", "RETURN", "RETURNS", "REVOKE", "RIGHT", "ROLE", "ROLLBACK", "ROLLUP", "ROUTINE", "ROW", "ROWCOUNT", "ROWGUIDCOL", "ROWS", "RULE", "SAVE", "SAVEPOINT", "SCHEMA", "SCOPE", "SCROLL", "SEARCH", "SECOND", "SECTION", "SELECT", "SEQUENCE", "SESSION", "SESSION_USER", "SET", "SETS", "SETUSER", "SHUTDOWN", "SIZE", "SMALLINT", "SOME", "SPACE", "SPECIFIC", "SPECIFICTYPE", "SQL", "SQLCA", "SQLCODE", "SQLERROR", "SQLEXCEPTION", "SQLSTATE", "SQLWARNING", "START", "STATE", "STATEMENT", "STATIC", "STATISTICS", "STRUCTURE", "SUBSTRING", "SUM", "SYSTEM_USER", "TABLE", "TEMPORARY", "TERMINATE", "TEXTSIZE", "THAN", "THEN", "TIME", "TIMESTAMP", "TIMEZONE_HOUR", "TIMEZONE_MINUTE", "TO", "TOP", "TRAILING", "TRAN", "TRANSACTION", "TRANSLATE", "TRANSLATION", "TREAT", "TRIGGER", "TRIM", "TRUE", "TRUNCATE", "TSEQUAL", "UNDER", "UNION", "UNIQUE", "UNKNOWN", "UNNEST", "UPDATE", "UPDATETEXT", "UPPER", "USAGE", "USE", "USER", "USING", "VALUE", "VALUES", "VARCHAR", _
        "VARIABLE", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHENEVER", "WHERE", "WHILE", "WITH", "WITHOUT", "WORK", "WRITE", "WRITETEXT", "YEAR", "ZONE")

    ' The strict list of reserved words in SQL Server 2000 and below.
    m_vasReservedWords = Array("ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP", "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE", "COLUMN", "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP", "DUMMY", "DUMP", "ELSE", "END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "FETCH", "FILE", "FILLFACTOR", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION", "GOTO", "GRANT", "GROUP", "HAVING", "HOLDLOCK", "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IN", "INDEX", "INNER", "INSERT", "INTERSECT", "INTO", "IS", "JOIN", "KEY", "KILL", "LEFT", "LIKE", "LINENO", _
        "LOAD", "NATIONAL", "NOCHECK", "NONCLUSTERED", "NOT", "NULL", "NULLIF", "OF", "OFF", "OFFSETS", "ON", "OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET", "OPENXML", "OPTION", "OR", "ORDER", "OUTER", "OVER", "PERCENT", "PLAN", "PRECISION", "PRIMARY", "PRINT", "PROC", "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READTEXT", "RECONFIGURE", "REFERENCES", "REPLICATION", "RESTORE", "RESTRICT", "RETURN", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE", "SCHEMA", "SELECT", "SESSION_USER", "SET", "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", "TEXTSIZE", "THEN", "TO", "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE", "TSEQUAL", "UNION", "UNIQUE", "UPDATE", "UPDATETEXT", "USE", "USER", "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHERE", "WHILE", "WITH", "WRITETEXT")

End Sub

Private Function NameNeedsDelimiting(ByVal sName As String, ByVal nNameType As ESQLNameTypes) As Boolean

    Dim bFirst As Boolean
    Dim i As Long
    Dim vsReservedWord As Variant ' String

    NameNeedsDelimiting = False
    sName = UCase$(Trim$(sName))

    ' A blank name is always OK.
    If Len(sName) = 0 Then
        Exit Function
    End If

    ' Check for characters that would be illegal in a SQL Server
    ' identifier.
    bFirst = True
    For i = 1 To Len(sName)
        Select Case Mid$(sName, i, 1)
        Case "A" To "Z", "a" To "z", "_"
            ' OK
        Case "0" To "9", "$"
            ' OK if not first character
            If bFirst Then
                NameNeedsDelimiting = True
                Exit Function
            End If
        Case "#"
            ' OK if not first character or if table name
            If bFirst And Not nNameType = knTable Then
                NameNeedsDelimiting = True
                Exit Function
            End If
        Case "@"
            ' OK if not first character or if variable name
            If bFirst And Not nNameType = knVariable Then
                NameNeedsDelimiting = True
                Exit Function
            End If
        Case Else
            NameNeedsDelimiting = True
            Exit Function
        End Select
        bFirst = False
    Next

    ' Check whether the name matches a SQL Server reserved word.
    If Not IsArray(m_vasReservedWords) Then
        InitialiseReservedWords
    End If
    For Each vsReservedWord In m_vasReservedWords
        If vsReservedWord = sName Then
            NameNeedsDelimiting = True
            Exit Function
        End If
    Next

End Function

Private Function ReplaceBadCharacters(ByVal sName As String, ByVal nNameType As ESQLNameTypes) As String

    Dim bFirst As Boolean
    Dim i As Long
    Dim vsReservedWord As Variant ' String

    ' A blank name is always OK.
    If Len(sName) = 0 Then
        Exit Function
    End If

    ' Check for characters that would be illegal in a SQL Server
    ' identifier.
    bFirst = True
    For i = 1 To Len(sName)
        Select Case Mid$(sName, i, 1)
        Case "A" To "Z", "a" To "z", "_"
            ' OK
        Case "0" To "9", "$"
            ' OK if not first character
            If bFirst Then
                Mid$(sName, i, 1) = "_"
            End If
        Case "#"
            ' OK if not first character or if table name
            If bFirst And Not nNameType = knTable Then
                Mid$(sName, i, 1) = "_"
            End If
        Case "@"
            ' OK if not first character or if variable name
            If bFirst And Not nNameType = knVariable Then
                Mid$(sName, i, 1) = "_"
            End If
        Case Else
            Mid$(sName, i, 1) = "_"
        End Select
        bFirst = False
    Next

    ReplaceBadCharacters = sName

End Function
