Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    'Developer Guide No. 50 (guide)
    Dim objfrminterface As New frmInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/09/00
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPMAuthorityLevel As Integer

    Private m_lShowInterface As gPMConstants.PMEReturnCode

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'bSirInsuranceFile.Services

    Private m_oBusiness As bSIRChangePolicyStatus.Business

    Private m_lInsuranceFileCnt As Integer

    Private m_sOldPolicyNumber As String = ""
    'TN20001127 (Start)
    Private m_sNewPolicyNumber As String = ""
    'TN20001127 (End)
    Private m_sQuoteStatus As String = ""
    Private m_iMode As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            If g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRChangePolicyStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oBusiness = temp_m_oBusiness


                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                m_oBusiness = temp_m_oBusiness
            End If

            m_lShowInterface = gPMConstants.PMEReturnCode.PMTrue

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameOperateMode

                        m_iMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "ShowInterface"

                        m_lShowInterface = CType(CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), gPMConstants.PMEReturnCode)
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    ' VB 05/04/2005 PN19874: Assign current status of Quote.
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "insurance_ref"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sNewPolicyNumber

            ' Assign current status of Quote.
            If m_sQuoteStatus <> "" Then
                ReDim Preserve vKeyArray(1, 1)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "quote_status"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sQuoteStatus
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sOldPolicyNumber.Trim() = m_sNewPolicyNumber.Trim() Then
                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ReDim vSummaryArray(2, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "insurance_ref"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sNewPolicyNumber

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
                'JMK 02/08/2001 assign to business property

                m_oBusiness.TransactionType = m_sTransactionType
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

#Region " PRIVATE Methods"
    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessInterface() As Integer

        Dim nResult As Integer
        Dim vResult As DialogResult
        Dim vRisks(,) As Object
        Dim sMessage As String = ""
        Dim lLevel As Integer
        Dim bSelectedRisks As Boolean

        nResult = gPMConstants.PMEReturnCode.PMTrue

        'Only bother if we're really making it live...
        If m_iMode <> 1 Then

            If m_lShowInterface = gPMConstants.PMEReturnCode.PMTrue Then
                ' Display screen
                m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to load the interface.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Display the interface.
                m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to display the inteface.
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Destroy the interface from memory.
                m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to unload the interface.
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If we have cancel, we need to get back to policy screen
                ' without doing anything with the policy
                If m_lStatus <> gPMConstants.PMEReturnCode.PMOK Then
                    Return nResult
                End If
            End If

            ' Get risk

            m_lReturn = m_oBusiness.GetRisksByStatus(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vRisks:=vRisks)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '1 = Referred
            '2 = Declined
            '3 = Quoted
            '4 = Unquoted
            '5 = Purchase question to be answered
            '6 = Post quote questions to be answered
            '7 = Pre quote questions to be answered
            '8 = Pending Reinsurance

            'This had better have something in it...
            If Not Information.IsArray(vRisks) Then
                sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                           "There are no risks on this policy"
            Else
                sMessage = ""
                lLevel = 0

                For lTemp As Integer = vRisks.GetLowerBound(1) To vRisks.GetUpperBound(1)
                    ' PW311002 - if selected do checks, else don't bother

                    If ToSafeInteger(vRisks(1, lTemp)) = 1 Then
                        bSelectedRisks = True
                        Select Case vRisks(0, lTemp)
                            Case 1, 2, 4
                                If lLevel < 3 Then
                                    lLevel = 3
                                    sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                                               "At least one risk on this policy is unquoted"
                                End If
                            Case 8
                                If lLevel < 2 Then
                                    lLevel = 2
                                    sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                                               "At least one risk on this policy has no reinsurance"
                                End If
                            Case 5, 6, 7
                                If lLevel < 1 Then
                                    lLevel = 1
                                    sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                                               "At least one risk on this policy has questions to be answered"
                                End If
                        End Select
                    End If
                Next lTemp
            End If

            ' PW311002 - Check if any of the risks are flagged as "selected"
            ' PW021202 - Don't muscle in on someone else's message
            If Not bSelectedRisks And sMessage.Trim() = "" Then
                sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                           "At least one risk on this policy must be selected to make it live"
            End If

            If sMessage <> "" Then
                If m_lShowInterface = gPMConstants.PMEReturnCode.PMTrue Then
                    vResult = MessageBox.Show(sMessage, "Change Policy Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Return nResult
            End If

            ' PW151102 - If going live, delete any unselected risks' link
            '            records

            m_lReturn = m_oBusiness.DeleteRisks(v_vrisks:=vRisks)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002 - Re-jig the risk and variation numbers of the remaining
            '            risks on this policy

            m_lReturn = m_oBusiness.RenumberRisks(v_lInsuranceFileCnt:=CInt(vRisks(2, 0)))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            vRisks = Nothing

        End If

        m_oBusiness.Mode = m_iMode

        m_lReturn = m_oBusiness.ChangePolicyStatus(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oBusiness.UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'get back new policy number
        m_sOldPolicyNumber = m_oBusiness.OldPolicyNumber

        m_sNewPolicyNumber = m_oBusiness.NewPolicyNumber

        Return nResult

    End Function

#End Region

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        'Developer Guide No. 50 (guide)
        With objfrminterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .InsuranceFileCnt = CStr(m_lInsuranceFileCnt)


            'Developer Guide No. 24 (guide)
            .BusinessObject = m_oBusiness
        End With

        ' Load the instance of the interface into memory.
        'Developer Guide No. 50 
        Dim tempLoadForm As frmInterface = objfrminterface

        'Developer Guide No. 50 (guide)
        If objfrminterface.ErrorNumber = gPMConstants.PMEReturnCode.PMOK Then
            'Developer Guide No. 50 (guide)
            objfrminterface.Close()
            result = gPMConstants.PMEReturnCode.PMOK
            'Developer Guide No. 50(guide)
        ElseIf objfrminterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            'Developer Guide No. 50 (guide)
            result = objfrminterface.ErrorNumber
        End If

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        'Developer Guide No. 50 (guide)
        VB6.ShowForm(objfrminterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            'Developer Guide No. 50 (guide)
            If objfrminterface.ErrorNumber <> 0 Then
                'Developer Guide No. 50 (guide)
                result = objfrminterface.ErrorNumber
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        'Developer Guide No. 50 (guide)
        With objfrminterface
            m_lStatus = .Status
            m_sQuoteStatus = .QuoteStatus
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        'Developer Guide No. 50 (guide)
        objfrminterface.Close()
        'Developer Guide No. 50 (guide)
        objfrminterface = Nothing

        Return result

    End Function
End Class
