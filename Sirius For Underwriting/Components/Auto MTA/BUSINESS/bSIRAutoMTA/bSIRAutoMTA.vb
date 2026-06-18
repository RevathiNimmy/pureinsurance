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
    ' Date:  07/05/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRAutoMTA"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"





    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_oObjectManager As Object
    '***********************************************

    Public Const ACRiskPosCnt As Integer = 0

    Public Const ACIInsFileCnt As Integer = 0
    Public Const ACIRiskId As Integer = 1
    Public Const ACIRiskDescription As Integer = 2
    Public Const ACIRiskTypeDescription As Integer = 3
    ''Public Const ACIRiskInceptionDate = 4 'This is incorrect.Query Returns ifi.cover_start_date - Amit
    Public Const ACIRiskExpiryDate As Integer = 5
    ' AM 061200 Add new column for risk status
    Public Const ACIRiskStatus As Integer = 6
    Public Const ACIRiskTotalSumInsured As Integer = 7
    Public Const ACIRiskTotalAnnualPremium As Integer = 8
    Public Const ACIRiskGisScreen As Integer = 9
    Public Const ACIRiskTypeId As Integer = 10
    Public Const ACIInsuranceFolderCnt As Integer = 11
    Public Const ACIRiskStatusFlag As Integer = 12
    ' PW311002 - add new columns for Risk Variations / Quote management
    Public Const ACIRiskNo As Integer = 13
    Public Const ACIVariationNo As Integer = 14
    Public Const ACIIsSelected As Integer = 15
    Public Const ACICoverage As Integer = 16
    Public Const ACIInsuredItem As Integer = 17
    Public Const ACIExtensions As Integer = 18
    ' PW221102 - add risk tax
    ' PS411
    Public Const ACIRiskTax As Integer = 19
    Public Const ACIRiskFolderCnt As Integer = 24

    'Public Const ACIRiskInceptionDate As Integer = 25 'This risk.inception_date - Amit
    Public Const ACIRiskInceptionDate As Integer = 35 'This risk.inception_date - Amit

    Public Const ACBaseInsFileCnt As Integer = 0
    Public Const ACOriginalInsFileCnt As Integer = 1
    Public Const ACCancelledInsFileCnt As Integer = 2
    Public Const ACCoverStartDate As Integer = 3
    Public Const ACExpiryDate As Integer = 4
    Public Const ACRiskProcessed As Integer = 5

    Sub Main_Renamed()


    End Sub
End Module