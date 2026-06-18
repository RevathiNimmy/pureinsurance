Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles
Partial Friend Class frmAccumulationValues
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmAccumulationValues
    '
    ' Date: 20/09/00
    '
    ' Description: Interface for population of the accumulation values table.
    '
    ' Edit History: CT 20/09/2000 - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmAccumulationValues"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sInsuranceRef As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_bAnyFailed As Boolean


    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Public Property AnyFailed() As Boolean
        Get
            Return m_bAnyFailed
        End Get
        Set(ByVal Value As Boolean)
            m_bAnyFailed = Value
        End Set
    End Property


    Private Sub cmdAllValues_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAllValues.Click
        m_sInsuranceRef = ""
        m_lInsuranceFileCnt = 0
        m_lReturn = RepopulateValues()
        '    If m_lReturn <> PMTrue Then
        '        'Log Error Message
        '        LogMessage _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="An error has occured which has stopped this process", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="cmdAllValues_click", _
        ''            vErrNo:=Err.Number, _
        ''            vErrDesc:=Err.Description
        '    End If
        '    If m_bAnyFailed Then
        '        MsgBox "Finished generating accumulation values for all policies that require accumulation. Please check the log file as there were policies that failed to have accumulation values generated."
        '    Else
        '        MsgBox "Finished generating accumulation values for all policies that require accumulation."
        '    End If

    End Sub

    Private Sub cmdSinglePolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSinglePolicy.Click

        If g_lInsuranceFileCnt = 0 Then
            m_lReturn = GetPolicyInfo()
        End If

        m_lReturn = RepopulateValues()
        '    If m_lReturn <> PMTrue Then
        '        'Log Error Message
        '        LogMessage _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="An error has occured which has stopped this process", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="cmdSinglePolicy_click", _
        ''            vErrNo:=Err.Number, _
        ''            vErrDesc:=Err.Description
        '    End If
        '    If m_bAnyFailed Then
        '        MsgBox "Please check the log file as there was an error generating accumulation values for the policy " & m_sInsuranceRef
        '    Else
        '        MsgBox "Finished generating accumulation values for the policy " & m_sInsuranceRef
        '    End If

    End Sub

    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
            If g_lInsuranceFileCnt <> 0 Then
                m_lInsuranceFileCnt = g_lInsuranceFileCnt
            Else
                m_lInsuranceFileCnt = 0
            End If
            m_sInsuranceRef = ""


            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: GetPolicyInfo
    '
    ' Description:  Instance FindInsurance to retrieve Policy reference
    '
    ' ***************************************************************** '
    Public Function GetPolicyInfo() As Integer
        Dim result As Integer = 0
        Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oFindPolicy.CallingAppName = ACApp

            oFindPolicy.InsReference = ""


            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Retrieve InsuranceRef and set as PolicyRef

            m_sInsuranceRef = oFindPolicy.InsReference

            m_lInsuranceFileCnt = oFindPolicy.InsFileCnt

            ' Destroy Find Insurance object

            oFindPolicy.Dispose()
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyInfoFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RepopulateValues
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function RepopulateValues() As Integer
        Dim result As Integer = 0
        Dim oAccumulationValues As bSIRAccumulationValues.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bAnyFailed = False

            ' Create business  object
            Dim temp_oAccumulationValues As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccumulationValues, "bSIRAccumulationValues.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAccumulationValues = temp_oAccumulationValues

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'bSirAccumulationValues.Business'.", vApp:=ACApp, vClass:=ACClass, vMethod:="RepopulateValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oAccumulationValues.InsuranceFileCnt = m_lInsuranceFileCnt


            m_lReturn = oAccumulationValues.RepopulateAccumValues()


            m_bAnyFailed = oAccumulationValues.AnyFailed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                '        Exit Function
            End If

            ' Destroy Accumulate Values object

            oAccumulationValues.Dispose()
            oAccumulationValues = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RepopulateValues", vApp:=ACApp, vClass:=ACClass, vMethod:="RepopulateValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub frmAccumulationValues_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub
End Class