Option Strict Off
Option Explicit On
Module AutoNumberSql
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    ' ***************************************************************** '
    ' Class Name: AutoNumberSQL
    '
    ' Date: 20th August 1997
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) create a unique id
    '
    ' Edit History: 20/08/1997  Original created
    ' ***************************************************************** '

    'SQL Statements

    ' Select...
    Public Const ACAllocateNumberStored As Boolean = True
    Public Const ACAllocateNumberName As String = "AllocateNumber"
    ' the parameters are... 1=PMNumber_Range_ID, 2=Return variable (int), 3=UserID
    'DAK200700
    '                       4=return prefix char(20), 5=return suffix char(20),
    '                       6=return range_code char(10)
    'Developer Guide No 39. 
    Public Const ACAllocateNumberSQL As String = "spu_pm_allocate_number"
    ' selects relevant number range...
    Public Const ACNumberRangeFromSourceStored As Boolean = False
    Public Const ACNumberRangeFromSourceName As String = "SelectNumberRangeFromSource"
    Public Const ACNumberRangeFromSourceSQL As String = "{}"

    ' Get ReferenceID using code...
    Public Const ACGetReferenceIDFromCodeStored As Boolean = False
    Public Const ACGetReferenceIDFromCodeName As String = "SelectIDUsingCode"
    Public Const ACGetReferenceIDFromCodeSQL As String = "{}"

    ' GenerateReferenceSQL...
    Public Const ACGenerateReferenceStored As Boolean = True
    Public Const ACGenerateReferenceName As String = "GenerateReference"
    'Public Const ACGenerateReferenceSQL As String = "{call spu_pm_GenerateReference (?)}"
    Public Const ACGenerateReferenceSQL As String = "spu_pm_GenerateReference"

    ' GetNumberRangeSQL...
    Public Const ACGetNumberRangeStored As Boolean = False
    Public Const ACGetNumberRangeName As String = "GetNumberRange"
    Public Const ACGetNumberRangeSQL As String = "{}"

    ' GetNumberRageID SQL...
    Public Const ACGetNumberRangeIDStored As Boolean = True
    Public Const ACGetNumberRangeIDName As String = "GetNumberRangeID"
    'Public Const ACGetNumberRangeIDSQL As String = "{call spu_pm_get_number_range (?,?,?,?,?)}"
    Public Const ACGetNumberRangeIDSQL As String = "spu_pm_get_number_range"
End Module