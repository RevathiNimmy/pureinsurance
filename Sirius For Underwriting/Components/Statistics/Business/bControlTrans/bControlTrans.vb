Option Strict Off
Option Explicit On
Imports SSP.Shared
Imports SSP.Shared.bPMWrkTaskInstance

<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")>
Public Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  25/04/1997
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bControlTrans"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const kSystemOptionCreditControlEnabled As Integer = 5001
    Public Const kSystemOptionChaseCycleEnabled As Integer = 5096

    Public Const ACCompanyId As Integer = 0
    Public Const ACPartyAccountId As Integer = 1
    Public Const ACCurrencyId As Integer = 2
    Public Const ACCurrenyBaseRate As Integer = 4
    Public Const ACAccountingDate As Integer = 7
    Public Const ACInsuranceReference As Integer = 11
    Public Const ACSubbranchId As Integer = 12
    Public Const kSystemOptionRoundOffAccount As Integer = 5080

    Sub Main_Renamed()

        ' Main entry point for the component
        ' TestAutomated
        ' Stop

    End Sub

    Sub TestAutomated()
        Dim vKeyArray(,) As Object
        ReDim vKeyArray(1, 1)
        Dim excep As SystemException

        Dim oControlTrans As bControlTrans.Automated = New bControlTrans.Automated()

        Dim lReturn As gPMConstants.PMEReturnCode = CType((oControlTrans).Initialise(sUsername:="basilb", sPassword:="basilb", iUserID:=1, iSourceId:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test"), gPMConstants.PMEReturnCode)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameSourceId

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = 1

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsFileID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 2

        lReturn = CType(oControlTrans.SetKeys(vKeyArray), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'MessageBox.Show("Error Set keys", Application.ProductName)
            bPMFunc.LogMessage("basilb", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Set keys", vApp:=ACApp, vClass:=ACClass, vMethod:="TestAutomated", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End If

        lReturn = oControlTrans.Start()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'MessageBox.Show("Error Start", Application.ProductName)
            bPMFunc.LogMessage("basilb", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Start", vApp:=ACApp, vClass:=ACClass, vMethod:="TestAutomated", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End If

        vKeyArray = Nothing

        lReturn = CType(oControlTrans.GetKeys(vKeyArray), gPMConstants.PMEReturnCode)

        'MessageBox.Show(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1)), Application.ProductName)
        bPMFunc.LogMessage("basilb", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1)), vApp:=ACApp, vClass:=ACClass, vMethod:="TestAutomated", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        'MessageBox.Show(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1)), Application.ProductName)
        bPMFunc.LogMessage("basilb", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1)), vApp:=ACApp, vClass:=ACClass, vMethod:="TestAutomated", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


    End Sub
End Module
