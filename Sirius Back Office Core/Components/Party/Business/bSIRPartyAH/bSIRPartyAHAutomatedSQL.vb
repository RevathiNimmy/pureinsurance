Option Strict Off
Option Explicit On
Module AutomatedSQL
    ' ***************************************************************** '
    ' Class Name: AutomatedSQL
    '
    ' Date: 11/08/1999
    '
    ' Description: Contains the SQL Statements required by the 
    '              bSIRPartyAH.Automated class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyAH SQL
    ' Public Const ACGetAllDetailsStored = True
    ' Public Const ACGetAllDetailsName = "SelectAllSIRPartyAH"
    ' Public Const ACGetAllDetailsSQL = "{call spe_SIRPartyAH_saa}"

    ' Check ID SQL
    ' Public Const ACCheckIDStored = True
    ' Public Const ACCheckIDName = "CheckSIRPartyAHID"
    ' Public Const ACCheckIDSQL = "{call spe_SIRPartyAH_check_id (?)}"
End Module