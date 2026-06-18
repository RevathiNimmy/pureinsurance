Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    Private Const ACClass As String = "Interface"

    Private m_lPMAuthorityLevel As Integer
    Private m_sCallingAppName As String = ""

    ' Process mode variables
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lStatus As Integer

    ' Party Count
    Private m_lPartyCnt As Integer
    ' Insurance stuff
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lProductId As Integer
    Private m_lBusinessTypeId As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lPolicySourceID As Integer
    Private m_iSourceID As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bFromRenewals As Boolean
    Private m_bPolicyLapsed As Boolean
    Private m_bIsSubAgentAdded As Boolean
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
    Private m_bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_sSelectedPolicyStatus As String = ""
    Private m_bIsMTATemp As Boolean
    Private m_dtRenewaldate As Date
    Private m_bIsPriorDate As Boolean
    Private m_bIsRenewed As Boolean
    Private m_dtLapsedDate As Date
    'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)


    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF

            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bObjectManager.ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

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
    ' History: 26/10/1999 CTAF - Created.
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
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: ProcessBranch
    '
    ' Description:
    '
    ' History: 30/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessBranch() As Integer
        Dim result As Integer = 0


        Dim oBranch As iPMBBranch.Interface_Renamed



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of iPMBBranch.Interface
        Dim temp_oBranch As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oBranch = temp_oBranch
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBBranch.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBranch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        'TN20010607 start

        oBranch.PartyCnt = m_lPartyCnt
        oBranch.ProductId = m_lProductId

        'TN20010607 end

        ' Start it up

        m_lReturn = oBranch.GetSource(iSourceID:=m_iSourceID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get source id", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBranch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            ' Dont exit the function, so we can clear up the object properly
        End If

        ' Terminate and clear-up

        oBranch.Dispose()
        oBranch = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TN20010125 Start
            'only get branch when in new business
            If m_lInsuranceFileCnt = 0 Then
                ' CTAF 300800 - Get the source
                m_lReturn = CType(ProcessBranch(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'TN20010125 End

            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)
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
    ' History: 26/10/1999 CTAF - Created.
    '          'PN20835 New key added for Subagent status
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sTransactionType = "REN" Then
                'Dimension the array for one extra param
                ReDim vKeyArray(1, 7)
            Else
                'Dimension the array the normal size
                ReDim vKeyArray(1, 7)
            End If
            ' Insurance File Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

            ' Insurance Folder Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFolderCnt

            ' Business Type Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameBusinessTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lBusinessTypeId

            If m_sTransactionType = "REN" Then
                ' Policy's Source ID required for the New "Renewal Process" tasks for GJW

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameSourceId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lPolicySourceID
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "quote_status"
                If m_bPolicyLapsed Then

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "Quote Lapsed"
                Else

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = ""
                End If
            End If

            'Subagent status

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vKeyArray.GetUpperBound(1)) = "is_subagent_added"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vKeyArray.GetUpperBound(1)) = m_bIsSubAgentAdded

            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "SelectedPolicyStatus"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sSelectedPolicyStatus


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "BackDatedMTAsAllowed"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_bBackDatedMTAsAllowed
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "IsTrueMonthlypolicyandNextInstalmentRenewal"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_bIsTrueMonthlypolicyandNextInstalmentRenewal
            'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)

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
    ' History: 26/10/1999 CTAF - Created.
    '
    ' Kevin Renshaw (CMG) 26/2/2003 - added renewals key
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

                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameProductID, "Product_id"

                        m_lProductId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "renewals"

                        m_bFromRenewals = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "Policy_Lapsed"

                        m_bPolicyLapsed = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                    Case "BackDatedMTAsAllowed"

                        m_bBackDatedMTAsAllowed = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case "SelectedPolicyStatus"

                        m_sSelectedPolicyStatus = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case "IsMTATemp"

                        m_bIsMTATemp = CBool(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                    Case "Renewaldate"

                        m_dtRenewaldate = CDate(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                    Case "IsPriorDate"

                        m_bIsPriorDate = CBool(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                    Case "IsRenewed"

                        m_bIsRenewed = CBool(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                    Case "LapsedDate"

                        m_dtLapsedDate = CDate(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))


                        'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

                End Select

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Renuka - (WPR87 Paralleling)
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)


                vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Policy"

                vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sInsuranceRef
            End If

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
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim oInterface As frmInterface



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new instance of the form
        oInterface = New frmInterface()

        If m_lInsuranceFileCnt <> 0 Then
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            End If
        End If

        ' Set the party count
        With oInterface
            .TransactionType = m_sTransactionType
            .Task = m_iTask
            .PartyCnt = m_lPartyCnt
            .SourceID = m_iSourceID
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .ProductId = m_lProductId
            .IsRenewal = m_bFromRenewals
            .PolicyLapsed = m_bPolicyLapsed
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            .BackDatedMTAsAllowed = m_bBackDatedMTAsAllowed
            .SelectedPolicyStatus = m_sSelectedPolicyStatus
            .IsMTATemp = CStr(m_bIsMTATemp)
            .IsPriorDate = m_bIsPriorDate
            .Renewaldate = m_dtRenewaldate
            .IsRenewed = m_bIsRenewed
            .LapsedDate = m_dtLapsedDate
            'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
            .IsTrueMonthlypolicyandNextInstalmentRenewal = m_bIsTrueMonthlypolicyandNextInstalmentRenewal
            'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
        End With
        ' Show the form
        oInterface.ShowDialog()

        ' Get the insurance file cnt
        With oInterface
            m_lInsuranceFileCnt = .InsuranceFileCnt
            m_lInsuranceFolderCnt = .InsuranceFolderCnt
            m_lBusinessTypeId = .BusinessTypeId
            m_sInsuranceRef = .InsuranceRef
            m_lPolicySourceID = .PolicySourceID
            m_lStatus = .Status
            m_bIsSubAgentAdded = .IsSubAgentAdded
            m_bPolicyLapsed = .PolicyLapsed
            'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
            m_bIsTrueMonthlypolicyandNextInstalmentRenewal = .IsTrueMonthlypolicyandNextInstalmentRenewal
            'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
        End With

        ' Remove the instance
        oInterface.Close()
        oInterface = Nothing

        Return result

    End Function
End Class

