Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Process mode variables
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Party Count
    Private m_lPartyCnt As Integer
    ' Short Name
    Private m_sShortName As String = ""
    ' Insurance stuff
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""

    'TN20001117
    Private m_lFindType As Integer
    'TF040902

    Private m_bDisableInsFileType As Boolean
    Private m_sInsuranceFileType As String = ""

    Private m_lReturn As Integer
    'Kevin Renshaw
    Private m_lDummyFindType As Integer
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_sSelectedPolicyStatus As String = ""
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

    'TN20001117 (Start)
    Public WriteOnly Property FindType() As Integer
        Set(ByVal Value As Integer)
            m_lFindType = Value
        End Set
    End Property
    'TN20001117 (End)

    'TF040902
    Public WriteOnly Property DisableInsFileType() As Boolean
        Set(ByVal Value As Boolean)
            m_bDisableInsFileType = Value
        End Set
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'ED 18072002 (Start)
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
            'ED 18072002 (End)

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
            ReDim vKeyArray(1, 6)

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

            ' client shortname

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sShortName
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "BackDatedMTAsAllowed"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_bBackDatedMTAsAllowed


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "SelectedPolicyStatus"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_sSelectedPolicyStatus
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
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameShortName

                        m_sShortName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'TF020902 - Filter for Quotes etc.
                        'Note this is NOT the FindType which defines form (eg GIIM, PMU)
                        'Kevin Renshaw - NOTE comment above BUT it is for Underwritting
                    Case PMNavKeyConst.PMKeyNameInsFileType
                        'JES check if incoming value is numeric ( this is a string for Broking )

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            m_lDummyFindType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        End If


                        m_sInsuranceFileType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)


            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Policy"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sInsuranceRef

            Return result

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
    '          25/02/03 Kevin Renshaw (CMG) added dummy find type function
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim oInterface As frmInterface
        Dim sValue As String = ""
        'Start Written
        Const kMethodName As String = "ProcessInterface"
        Dim sPolicyStatus As String

        'End- Written


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new instance of the form
        oInterface = New frmInterface()

        ' Set the party count
        With oInterface
            'TN20001117 (Start)
            .FindType = m_lFindType
            'TN20001117 (End)
            'TF040902
            .InsFileType = m_sInsuranceFileType
            .DisableInsFileType = m_bDisableInsFileType

            .PartyCnt = m_lPartyCnt
            .ShortName = m_sShortName

            .DummyFindType = m_lDummyFindType

            m_lReturn = .SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
        End With

        'ED 18072002 - Product Options - disable New button for Underwriting
        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oInterface.IsUnderwriting = sValue = "U"

        'ED 18072002 (End)

        ' Load the form
        oInterface.frmInterface_Load()
        'Developer Guide No. 68 (Latest guide) comment the line

        'TN20001117 (Start)
        oInterface.uctListPolicyControl.FindType = m_lFindType
        'TN20001117 (End)

        oInterface.uctListPolicyControl.InsHolderCnt = m_lPartyCnt
        ' Set the short name
        oInterface.uctListPolicyControl.ShortName = m_sShortName

        ' Show the form
        oInterface.ShowDialog()

        ' Get the insurance file cnt
        With oInterface
            m_lInsuranceFileCnt = .InsuranceFileCnt
            m_lInsuranceFolderCnt = .InsuranceFolderCnt
            m_sInsuranceRef = .InsuranceRef
            m_sShortName = .ShortName
            m_lStatus = .Status
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            m_bBackDatedMTAsAllowed = .BackDatedMTAsAllowed
            m_sSelectedPolicyStatus = .SelectedPolicyStatus
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If
        End With

        'Lock Current Policy for MTA  'PN35753 --RC
        If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or (m_sTransactionType = "NB" And m_lInsuranceFileCnt <> 0) Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then


            Do While LockPolicy() = gPMConstants.PMEReturnCode.PMFalse
                ' Show the form
                oInterface.ShowDialog()

                ' Get the insurance file cnt
                With oInterface
                    m_lInsuranceFileCnt = .InsuranceFileCnt
                    m_lInsuranceFolderCnt = .InsuranceFolderCnt
                    m_sInsuranceRef = .InsuranceRef
                    m_sShortName = .ShortName
                    m_lStatus = .Status

                    'Close form if Cancel Clicked
                    If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                        Exit Do
                    End If
                End With
            Loop

            If m_sTransactionType = "NB" And m_lInsuranceFileCnt <> 0 Then
                m_lReturn = CheckPolicyStatus(m_lInsuranceFileCnt, sPolicyStatus)

                If Trim(sPolicyStatus) <> "QUOTE" Then
                    'Start Written
                    If Trim(sPolicyStatus) <> "WRITTEN" Then

                        MsgBox("Policy " & m_sInsuranceRef$ & " has been made live by another user ", MsgBoxStyle.Information)

                        UNLOCKPOLICY()
                        m_lReturn = oInterface.uctListPolicyControl.GetPolicies
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to call the method GetPolicies in User Control ", gPMConstants.PMEReturnCode.PMError)
                        End If

                        oInterface.ShowDialog()

                        With oInterface
                            m_lInsuranceFileCnt = .InsuranceFileCnt
                            m_lInsuranceFolderCnt = .InsuranceFolderCnt
                            m_sInsuranceRef = .InsuranceRef
                            m_sShortName = .ShortName
                            m_lStatus = .Status
                        End With

                    End If
                End If
                'End- Written
            End If
        End If

        ' Remove the instance
        oInterface.Close()
        oInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LockPolicy
    '
    ' Description: Lock Current Policy 'PN35753 --RC
    '
    '' ***************************************************************** '
    Public Function LockPolicy() As Integer
        Dim result As Integer = 0

        Dim sLockedBy As String = ""

        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sTransactionType = "NB" And m_lInsuranceFolderCnt = 0 Then Return result

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.LOCKKEY("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID, v_bOtherUserOnly:=False, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Policy currently locked by " & sLockedBy &
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Policy Lock")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function CheckPolicyStatus(ByVal m_lInsuranceFileCnt&, ByRef r_sPolicyStatus As String) As Long
        Dim result As Integer = 0
        Dim m_oBusiness As Object
        Try



            m_lReturn = g_oObjectManager.GetInstance(
                               oObject:=m_oBusiness,
                               sClassName:="bSIRFindInsurance.Form",
                               vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindInsurance.Form Initialise Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPolicyStatus")

                Return result
            End If

            m_lReturn = m_oBusiness.CheckPolicyStatus(m_lInsuranceFileCnt&, r_sPolicyStatus)

            Select Case m_lReturn

                Case gPMConstants.PMEReturnCode.PMTrue
                    result = m_lReturn

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying get policy status ", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPolicyStatus")

            End Select

            Dispose()
            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPolicyStatus", excep:=ex)
            Return result
        End Try
    End Function

    Private Function UNLOCKPOLICY() As Integer                             
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.UnLockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the policy", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            'Terminate the business object
              
            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="UNLOCKPOLICY Failed", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

End Class

