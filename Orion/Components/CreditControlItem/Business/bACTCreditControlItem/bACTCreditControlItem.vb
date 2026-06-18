Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  02nd October 2002
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTCreditControlItem"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"
    Public Const kDeleteCreditControlItemName As String = "Deletes the credit control item for the specified insurance file cnt"
    Public Const kDeleteCreditControlItemSQL As String = "spu_ACT_Del_Credit_Control_Item_InsFile"
    Public Const kDeleteCreditControlItemSQLStored As Boolean = True
    Sub Main_Renamed()

    End Sub

End Module