Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared

Module PMTransactionErrors
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' Name of this class
    Public Const ACClass As String = "TransactionErrorCodes"

    ' Error codes
    Public Const ACTransErrPeriodNotDef As gPMConstants.PMEReturnCode = 1000
    Public Const ACTransErrInsurerNotConfig As Integer = 1001
End Module