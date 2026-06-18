Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No 129. 
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As Integer

    ' Process mode variables
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Short Name
    Private m_sShortName As String = ""
    ' Insurance stuffF
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""
    'ED 16092002 - additional key
    Private m_lRiskId As Integer

    Private m_lReturn As Integer

    Private m_lOriginalInsuranceFileCnt As Integer ' RAM20050825 - PN 23018

    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_bIsMTATemp As Boolean
    Private m_sSelectedPolicyStatus As String = ""
    Private m_dtRenewaldate As Date
    Private m_bIsPriorDate As Boolean
    Private m_bIsRenewed As Boolean
    Private m_dtLapsedDate As Date
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = CStr(Value)

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'ED 19072002 (Start)
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
                g_sUsername.Value = .UserName
            End With
            'ED 19072002 (End)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return the insurance file/folder counts
            ReDim vKeyArray(1, 12) ' RAM20050824 - PN 23018 - Increased from 4 --> 5

            ' Insurance File Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

            ' Insurance Folder Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFolderCnt

            ' Insurance Ref

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "insurance_ref"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sInsuranceRef

            ' Product Id - necessary for when this is called as part of the MTA roadmap

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Product_id"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 0

            'ED 16092002 - Add Risk Id as this may have changed dependent upon the
            'policy version selected.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameRiskID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lRiskId

            ' RAM20050825 - PN 23018 - Added this key (This is used duing an MTA. This value will be
            '                           the InsuranceFileCnt of the Policy, which the user select)
            '                           Since this is needed in the ListRisk Screen

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameOriginalInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lOriginalInsuranceFileCnt

            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "BackDatedMTAsAllowed"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_bBackDatedMTAsAllowed


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "SelectedPolicyStatus"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_sSelectedPolicyStatus


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "IsMTATemp"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_bIsMTATemp


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = "Renewaldate"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_dtRenewaldate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = "IsPriorDate"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_bIsPriorDate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "IsRenewed"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_bIsRenewed


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = "LapsedDate"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_dtLapsedDate

            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            For iLoop1 As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)


                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "insurance_ref"

                        m_sInsuranceRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameShortName

                        m_sShortName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                    Case "BackDatedMTAsAllowed"

                        m_bBackDatedMTAsAllowed = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case "SelectedPolicyStatus"

                        m_sSelectedPolicyStatus = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

                End Select

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            'Don't do this - else we'll have the policy number on the title bar twice
            '    ReDim vSummaryArray(PMNavSummValue, 0)
            '
            '    vSummaryArray(PMNavSummHeading, 0) = "Policy"
            '    vSummaryArray(PMNavSummValue, 0) = m_sInsuranceRef

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0
        Dim oInterface As frmInterface
        Dim sSIROptUnderwriting As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new instance of the form
        oInterface = New frmInterface()

        ' Set the party count
        With oInterface
            .InsuranceRef = m_sInsuranceRef
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .ShortName = m_sShortName
            'Start  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            .BackDatedMTAsAllowed = m_bBackDatedMTAsAllowed
            .SelectedPolicyStatus = m_sSelectedPolicyStatus
            .IsMTATemp = CStr(m_bIsMTATemp)
            .IsPriorDate = m_bIsPriorDate
            .Renewaldate = m_dtRenewaldate
            .IsRenewed = m_bIsRenewed
            .LapsedDate = m_dtLapsedDate
            'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            m_lReturn = .SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
        End With



        oInterface.IsUnderwriting = True

        'ED 18072002 (End)

        ' Load the form

        'Developer Guide No.68 
        'Load(oInterface)

        ' Show the form

        oInterface.ShowDialog()

        ' Get the insurance file cnt
        With oInterface
            m_lInsuranceFileCnt = .InsuranceFileCnt
            m_lInsuranceFolderCnt = .InsuranceFolderCnt
            m_sInsuranceRef = .InsuranceRef.Trim()
            m_lStatus = .Status
            m_lOriginalInsuranceFileCnt = .OriginalInsuranceFileCnt ' RAM20050825 - PN 23018
            'Start  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            m_bIsMTATemp = CBool(.IsMTATemp)
            m_dtRenewaldate = .Renewaldate
            m_bIsPriorDate = .IsPriorDate
            m_bIsRenewed = .IsRenewed
            m_dtLapsedDate = .LapsedDate
            m_sSelectedPolicyStatus = .SelectedPolicyStatus
            'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
        End With

        ' Remove the instance
        oInterface.Close()
        oInterface = Nothing

        'ED 16092002 - Retrieve additional keys if for broking
        'Now get additional keys for Navigator
        'need riskid
        'ED 16092002 (END)

        Return result

    End Function
End Class

