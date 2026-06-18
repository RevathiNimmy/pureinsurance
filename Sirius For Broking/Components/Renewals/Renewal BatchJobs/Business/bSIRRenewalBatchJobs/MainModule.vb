Option Strict Off
Option Explicit On
Imports SSP.Shared
'Modified by Sudhanshu Behera on 5/18/2010 6:34:16 PM refer developer guide no. 129 (guide)
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    ' Date:  29/03/2001
    ' Description: Main Module.
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRRenewalBatchJobs"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Insurance File Types
    Public Const ACFileTypeRenewal As String = "RENEWAL"
    Public Const ACFileTypeQuote As String = "QUOTE"
    Public Const ACFileTypeWhatIf As String = "RENEWALWIF"
    'AK 210801
    Public Const ACFileTypePolicy As String = "POLICY"

    ' SIR Renewal Completion Array
    Public Const LCArrayInsuranceFolderCnt As Single = 0
    Public Const LCArrayGISSchemeID As Single = 1
    Public Const LCArrayRenewalStatusCode As Single = 2
    Public Const LCArrayRenewalGISSchemeID As Single = 3
    Public Const LCArrayRenewalInsuranceFileCnt As Single = 4
    Public Const LCArrayProductID As Single = 5
    Public Const LCArrayRenewalDate As Single = 6
    Public Const LCArrayPartyCnt As Single = 7
    Public Const LCArrayRiskCodeID As Single = 8
    Public Const LCArrayGISDataModelID As Single = 9
    Public Const LCArrayRenewalEDIAuditID As Single = 10
    Public Const LCArrayGISDataModelCode As Single = 11
    Public Const LCArrayInsFileTypeCode As Single = 12
    Public Const LCArrayGisBusinessTypeId As Single = 13
    Public Const LCArrayOldInsuranceFileCnt As Single = 14
    'ED 17092002 - new Constant
    Public Const LCArrayBusinessTypeCode As Single = 15

    ' SIR Create new policy array
    Public Const ACArrayPCInsuranceFolderCnt As Single = 0
    Public Const ACArrayPCGisSchemeId As Single = 1
    Public Const ACArrayPCPartyCnt As Single = 2
    Public Const ACArrayPCGisBusinessTypeId As Single = 3
    Public Const ACArrayPCGisDataModelCode As Single = 4
    Public Const ACArrayPCGisBusinessTypeCode As Single = 5
    Public Const ACArrayPCRenewalInsuranceFileCnt As Single = 6

    Public Const ACLockKeyInsuranceFolder As String = "insurance_folder_cnt"

    ' Username.
    'Public g_sUsername As String * 12

    ' Password.
    'Public g_sPassword As String * 30

    ' User ID
    'Public g_iUserID As Integer

    ' Calling Application
    'Public g_sCallingAppName As String
    ' Source ID
    'Public g_iSourceID As Integer
    ' Language ID
    'Public g_iLanguageID As Integer
    ' Currency ID
    'Public g_iCurrencyID As Integer
    ' LogLevel
    'Public g_iLogLevel As Integer

    ' ***************************************************************** '
    '
    ' Name: LogAppError
    '
    ' Description: Routine to act as global error handler for this application.
    '              This is a fix to ensure that all errors messages are
    '              surpressed by the control and only displayed by the
    '              calling application
    '
    ' History: 06/07/2001 IJM - Created.
    '
    ' *****************************************************************
    Public Function LogAppError(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing, Optional ByRef sUserName As String = "") As Integer

        Try

            ' Log all messages to file





            gPMFunctions.LogMessageToFile(sUsername:=sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As Exception

            gPMFunctions.LogMessageToFile(sUsername:=sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=excep)
            Return gPMConstants.PMEReturnCode.PMFalse

        End Try

    End Function
End Module