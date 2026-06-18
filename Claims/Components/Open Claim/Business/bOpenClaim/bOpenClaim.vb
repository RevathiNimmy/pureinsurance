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
    ' Date:  {TodaysDate}
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    'Events
    Public Const PMBEventNewClient As Integer = 1
    Public Const PMBEventNewPolicy As Integer = 2
    Public Const PMBEventNewClaim As Integer = 3
    Public Const PMBEventAddChange As Integer = 4
    Public Const PMBEventPolChange As Integer = 5
    Public Const PMBEventClaChange As Integer = 6
    Public Const PMBEventDelClient As Integer = 7
    Public Const PMBEventDelPolicy As Integer = 8
    Public Const PMBEventDelClaim As Integer = 9
    Public Const PMBEventDocument As Integer = 10
    Public Const PMBEventReport As Integer = 11
    Public Const PMBEventMailshot As Integer = 12
    Public Const PMBEventTransaction As Integer = 13

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bOpenClaim"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const kInsFileTypeMTACancellation As Integer = 8
    Public Const kInsFileTypeMTAReinstatement As Integer = 9

    Public Const kPolVersArrayItemInsuranceFileCnt As Integer = 0
    Public Const kPolVersArrayItemTypeId As Integer = 1
    Public Const kPolVersArrayItemStatusId As Integer = 2
    Public Const kPolVersArrayItemPolicyVersion As Integer = 3
    Public Const kPolVersArrayItemCoverStartDate As Integer = 4

    ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development -
    Public Const ksDeferredRIRiskStatus As String = "RIDEFERRED"
    Sub Main_Renamed()

    End Sub
End Module