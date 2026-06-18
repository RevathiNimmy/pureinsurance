Option Strict Off
Option Explicit On
Imports SharedFiles
'UPGRADE_NOTE: Interface was upgraded to Interface_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date:
    '
    ' Description: Main interface Class.
    '
    ' Edit History:
    ' RAM20050826 : Bug fix for PN 23018 - Added OriginalInsuranceFileCnt
    '                                      Installments Tab enable / disable bug fix
    ' ***************************************************************** '

    Private Const ACClass As String = "Interface"

    ' Process mode variables
    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date

    Private m_sCallingAppName As String

    Private m_lPMAuthorityLevel As Integer
    Private m_frmInterface As iACTCommissionPayments.frmInterface

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Party Count
    Private m_lPartyCnt As Integer
    ' Short Name
    Private m_sShortName As String
    ' Insurance stuff
    Private m_iSpecifiedTab As Short
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lRiskId As Integer

    Private m_lReturn As Integer
    Private m_sTaskGroupCode As String
    Private m_lPFPremFinanceCnt As Integer
    Private m_lPFPremFinanceVersion As Integer
    Private m_sPaymentTerms As String
    Private m_sQuoteStatus As String
    Private m_iMode As Short
    Private m_sOldPolicyNumber As String
    Private m_sNewPolicyNumber As String
    Private m_bRoadmapDisablesInstalments As Boolean
    Private m_vSearchResultsArray As Object

    Private m_bAutoSearch As Boolean
    Private m_sStatementDate As String
    Private m_sTransDeteFrom As String
    Private m_sTransDeteTo As String
    Private m_lCurrencyItemID As Integer
    Private m_lProductItemID As Integer
    Private m_lBranchItemID As Integer
    Private m_iTransAuthLimit As Short

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            PMProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Status = m_lStatus

        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 07/06/2000 DC - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim bObjectManager As Object
        Const kMethodName As String = "Initialise"

        Try

            Initialise = gPMConstants.PMEReturnCode.PMTrue
            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager

            ' Call the initialise method.
            'UPGRADE_WARNING: Couldn't resolve default property of object g_oObjectManager.Initialise. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to call the initialise method.
                Initialise = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                'UPGRADE_NOTE: Object g_oObjectManager may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                g_oObjectManager = Nothing

                'Raise Error.
                RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initialise the object manager")
                Exit Function
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                'UPGRADE_WARNING: Couldn't resolve default property of object g_oObjectManager.LanguageID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_iLanguageID = .LanguageID
                'UPGRADE_WARNING: Couldn't resolve default property of object g_oObjectManager.SourceID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_iSourceID = .SourceID
                'UPGRADE_WARNING: Couldn't resolve default property of object g_oObjectManager.UserID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_iUserID = .UserID
                'UPGRADE_WARNING: Couldn't resolve default property of object g_oObjectManager.UserName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_sUsername.Value = .UserName
            End With

            'UPGRADE_WARNING: Couldn't resolve default property of object g_oObjectManager.GetInstance. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = g_oObjectManager.GetInstance(oObject:=g_oBusiness, sClassName:="bACTCommissionPayments.Business", vInstanceManager:="ClientManager")

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error.
                RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initilise bSIRAgentReconAgreement.Business")
                Exit Function
            End If

        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally



        End Try
        Exit Function

    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


	Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Try

            Start = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Start = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            Start = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeys
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Try

            GetKeys = gPMConstants.PMEReturnCode.PMTrue

            ' Return the insurance file/folder counts
            ReDim vKeyArray(1, 1)

            ' Insurance File Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMkeyName, 0) = "risk_id"
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lRiskId

            ' payment terms
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = SIRLookupPaymentMethod
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sPaymentTerms

            Exit Function

        Catch ex As Exception

            GetKeys = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim iLoop As Short
        Dim sSearchResult As String
        Dim sLocalProductCode As String

        Try

            SetKeys = gPMConstants.PMEReturnCode.PMTrue

            If (IsArray(vKeyArray) = False) Then
                Exit Function
            End If

            'm_vKeyArray = vKeyArray

            For iLoop = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)

                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMkeyName, iLoop)

                    Case PMNavKeyConst.PMKeyNamePartyCnt
                        m_vSearchResultsArray = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)

                    Case "AgentSelect"

                        m_vSearchResultsArray = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)
                    Case "SearchResults"

                        sSearchResult = Trim(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHAutoSearch
                        m_bAutoSearch = ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHStatementDate
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_sStatementDate = Trim(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHTransDateFrom
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_sTransDeteFrom = Trim(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHTransDateTo
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_sTransDeteTo = Trim(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHCurrencyItemID
                        m_lCurrencyItemID = ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHProductItemID
                        m_lProductItemID = ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHBranchItemID
                        m_lBranchItemID = ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                    Case PMKeySRCHTransAuthLimit
                        m_iTransAuthLimit = ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                End Select

            Next iLoop
            If sSearchResult <> "" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vSearchResultsArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_vSearchResultsArray = Split(sSearchResult, "|")
            End If
            Exit Function

        Catch ex As Exception

            SetKeys = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Try

            GetSummary = gPMConstants.PMEReturnCode.PMTrue

            If (Trim(m_sOldPolicyNumber) = Trim(m_sNewPolicyNumber)) Then
                Exit Function
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ReDim vSummaryArray(2, 0)

            ' Assign the key array with the parameter members.
            'UPGRADE_WARNING: Couldn't resolve default property of object vSummaryArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Insurance_Ref"
            'UPGRADE_WARNING: Couldn't resolve default property of object vSummaryArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sNewPolicyNumber

            Exit Function

        Catch ex As Exception

            GetSummary = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Try

            SetProcessModes = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.
            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If (IsNothing(vTask) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vTask. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_iTask = CShort(vTask)
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If (IsNothing(vNavigate) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vNavigate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lNavigate = CInt(vNavigate)
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If (IsNothing(vProcessMode) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vProcessMode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lProcessMode = CInt(vProcessMode)
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If (IsNothing(vTransactionType) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vTransactionType. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_sTransactionType = CStr(vTransactionType)
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If (IsNothing(vEffectiveDate) = False) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vEffectiveDate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Exit Function

        Catch ex As Exception

            ' Error Section.
            SetProcessModes = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessInterface
    '
    ' Description:
    '
    ' History: 07/06/00 DC - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim oInterface As frmInterface


        ProcessInterface = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new instance of the form
        oInterface = New frmInterface

        ' Set the party count
        With oInterface
            'UPGRADE_WARNING: Couldn't resolve default property of object m_vSearchResultsArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            .AgentIdsArray = m_vSearchResultsArray
            .PartyCnt = m_lPartyCnt
            .AutoSearch = m_bAutoSearch
            .StatementDate = m_sStatementDate
            .TransDateFrom = m_sTransDeteFrom
            .TransDateTo = m_sTransDeteTo
            .CurrencyItemId = m_lCurrencyItemID
            .ProductItemId = m_lProductItemID
            .BranchItemId = m_lBranchItemID
            .TransAuthLimit = m_iTransAuthLimit
        End With

        ' Load the form

        ''Load(oInterface)

        ' Show the form
        oInterface.ShowDialog()

        ' Get the insurance file cnt
        With oInterface



        End With

        ' Remove the instance
        oInterface.Close()
        'UPGRADE_NOTE: Object oInterface may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oInterface = Nothing

        Exit Function

    End Function


    ' ***************************************************************** '
    ' Name: ProcessCommand
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim iMsgResult As Short
        Dim sMessage As String
        Dim sTitle As String

        Const kMethodName As String = "ProcessCommand"
        Try


            ProcessCommand = gPMConstants.PMEReturnCode.PMTrue
            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            m_frmInterface = New iACTCommissionPayments.frmInterface
            If (m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel) Then
                ' Get string messages
                iMsgResult = MsgBox("Cancelling will not return any search details " & vbCrLf & "Do you really wish to cancel?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2 + MsgBoxStyle.Question, "Cancel Search")

                ' Check message result.
                If (iMsgResult = MsgBoxResult.No) Then
                    ' Set return to false, meaning
                    ' don't cancel.
                    ProcessCommand = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Update the property member from the interface.
                m_lReturn = m_frmInterface.DataToProperties()
                ' Check for errors.
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    'Raise Error.
                    RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to Process DataToProperties")
                    Exit Function
                End If
            End If

        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessCommand, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally


        End Try
        Exit Function

    End Function
End Class