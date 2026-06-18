Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: 25/06/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              partyinsurer class.
    '
    ' Edit History:
    ' RAW 18/12/2002 : PS187 : Added new data items (WHTaxType, WHTaxRate, TaxRegNo, TaxCode, PaymentMethod, PaymentFrequency , BankAccount)
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select partyinsurer SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglepartyinsurer"
    Public Const ACSelectSingleSQL As String = "spe_party_insurer_sel"

    ' Add partyinsurer SQL
    'DC150803 -PS254 -fsa compliance
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "Addpartyinsurer"
    Public Const ACAddSQL As String = "spe_party_insurer_add"

    ' Delete partyinsurer SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "Deletepartyinsurer"
    Public Const ACDeleteSQL As String = "spe_party_insurer_del"

    ' Update partyinsurer SQL
    'DC150803 -PS254 -fsa compliance
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "Updatepartyinsurer"
    Public Const ACUpdateSQL As String = "spe_party_insurer_upd"
End Module