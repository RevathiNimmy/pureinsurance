Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmRenewalFilter
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmRenewalFilter"

    Private m_lReturn As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_oSearchPolicy As iPMBFindInsurance.Interface_Renamed

    'filter variables
    Private m_sInsuranceRef As String = ""
    Private m_dRenewalDate As Date
    Private m_lProductID As Integer
    Private m_lBranchID As Integer
    Private m_lRenewalType As Integer 'Amendment, Acceptance, Invite or all
    Private m_lRenewalMode As Integer
    Private m_bCanTransferBroker As Boolean
    Private m_lLeadAgentCnt As Integer
    Private m_lAgentcode As Integer

    Private m_bActivate As Boolean

    Dim m_oBusiness As bSIRRenewal.Business

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

    Public Property RenewalDate() As Date
        Get
            Return m_dRenewalDate
        End Get
        Set(ByVal Value As Date)
            m_dRenewalDate = Value
        End Set
    End Property

    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public Property BranchID() As Integer
        Get
            Return m_lBranchID
        End Get
        Set(ByVal Value As Integer)
            m_lBranchID = Value
        End Set
    End Property

    Public Property RenewalType() As Integer
        Get
            Return m_lRenewalType
        End Get
        Set(ByVal Value As Integer)
            m_lRenewalType = Value
        End Set
    End Property

    Public WriteOnly Property RenewalMode() As Integer
        Set(ByVal Value As Integer)
            m_lRenewalMode = Value
        End Set
    End Property

    Public WriteOnly Property CanTransferBroker() As Boolean
        Set(ByVal Value As Boolean)
            m_bCanTransferBroker = Value
        End Set
    End Property

    Public ReadOnly Property LeadAgentCnt() As Integer
        Get
            Return m_lLeadAgentCnt
        End Get
    End Property

    Public ReadOnly Property AgentCode() As Integer
        Get
            Return m_lAgentcode
        End Get
    End Property

    Private Sub cboAgentCode_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAgentCode.SelectedIndexChanged

        m_lAgentcode = VB6.GetItemData(cboAgentCode, cboAgentCode.SelectedIndex)

    End Sub

    Private Sub cboBranch_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectedIndexChanged


        m_lReturn = PopulateAgentCbo()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to cboBranch_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboBranch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

    End Sub

    Private Sub cmdAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgent.Click

        Dim sShortName As String = ""
        Dim lLeadAgentCnt As Integer

        m_lReturn = SelectParty(vPartyCnt:=lLeadAgentCnt, vShortName:=sShortName, vSpecialParty:="AG", bIsInTransferMode:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                MessageBox.Show("Failed to get agent", ACApp, MessageBoxButtons.OK)
            End If

            Exit Sub
        End If

        m_lLeadAgentCnt = lLeadAgentCnt
        txtAgent.Text = sShortName
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_sInsuranceRef = txtPolicy.Text.Trim()

        If m_lRenewalMode <> ACRenModeStandard And m_sInsuranceRef = "" Then
            MessageBox.Show("Please select policy", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmdPolicySearch.Focus()
            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_dRenewalDate = dtpRenewalDate.Value

        If cboProductType.ListIndex <> -1 Then
            m_lProductID = cboProductType.ItemId
        End If

        If cboBranch.SelectedIndex <> -1 Then
            m_lBranchID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
        End If

        '    If cboBranchCode.ListIndex <> -1 Then
        '        m_lBranchID = cboBranchCode.ItemId
        '    End If

        m_lRenewalType = IIf(cboRenewalType.SelectedIndex = -1, 0, cboRenewalType.SelectedIndex)

        Me.Hide()
    End Sub

    Private Sub cmdPolicySearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPolicySearch.Click


        Dim temp_m_oSearchPolicy As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oSearchPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        m_oSearchPolicy = temp_m_oSearchPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_oSearchPolicy.InsFileType = gSIRLibrary.SIRInsFileTypeRenewal

            m_oSearchPolicy.FindMode = 1
            'Include policies attached to closed branches

            m_oSearchPolicy.IncludeClosedBranches = True



        m_lReturn = m_oSearchPolicy.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        If m_oSearchPolicy.Status = gPMConstants.PMEReturnCode.PMOk Then

            txtPolicy.Text = m_oSearchPolicy.InsReference.Trim()
        End If

    End Sub

    Private Sub frmRenewalFilter_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If Not m_bActivate Then
                m_bActivate = True

                If m_lRenewalMode <> ACRenModeStandard Then
                    'hide multiple filter, broker portfolio transfer and resize screen if we are in GAJ renewal mode
                    fraFilterCriteria.Visible = False
                    fraAgentFilter.Visible = False
                    txtPolicy.Enabled = True
                    Me.Height = VB6.TwipsToPixelsY(2595)
                    cmdCancel.Top = VB6.TwipsToPixelsY(1395)
                    cmdOK.Top = cmdCancel.Top
                Else

                    If Not m_bCanTransferBroker Then
                        'hide broker portfolio transfer
                        fraAgentFilter.Visible = False

                        'Me.Height = 4710
                        Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraFilterCriteria.Top) + VB6.PixelsToTwipsY(fraFilterCriteria.Height) + VB6.PixelsToTwipsY(cmdOK.Height) + 1000)
                        cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraFilterCriteria.Top) + VB6.PixelsToTwipsY(fraFilterCriteria.Height) + 100)
                        cmdOK.Top = cmdCancel.Top
                    End If

                    'populate renewal type combo
                    cboRenewalType.Items.Add("All")
                    cboRenewalType.Items.Add("Amendment")
                    cboRenewalType.Items.Add("Acceptance")
                    cboRenewalType.Items.Add("Invite")

                    cboRenewalType.SelectedIndex = 0

                    dtpRenewalDate.Value = DateTime.Today
                End If
            End If

            cmdPolicySearch.Focus()
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        m_sInsuranceRef = ""
        m_dRenewalDate = DateTime.Today
        m_lProductID = 0
        m_lBranchID = 0
        m_lRenewalType = 0

        dtpRenewalDate.Value = DateTime.Today
    End Sub

    Private Sub frmRenewalFilter_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        If cboBranch.Items.Count <= 0 Then

            m_lReturn = PopulateBranchCbo()
            m_lReturn = PopulateAgentCbo()
            cboProductType.FirstItem = "(All)"
        End If
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
    End Sub

    Private Sub frmRenewalFilter_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        If Not (m_oSearchPolicy Is Nothing) Then

            m_oSearchPolicy.Dispose()
            m_oSearchPolicy = Nothing
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    ' Name: PopulateBranchCbo
    '
    ' Parameters: n/a
    '
    ' Description: Populates the branch combo with branches that this
    '               user has access to whether they are closed or not
    '
    ' History:
    '           Created : MEvans : 17-03-2005 : PN19562
    ' ***************************************************************** '
    Public Function PopulateBranchCbo() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBranchCbo"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vBranchDetails(,) As Object
        Dim llBound, lUBound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down the contents
            cboBranch.Items.Clear()

            ' get all branches available for this user

            lReturn = g_oBusiness.GetAllUserBranches(r_vResults:=vBranchDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSirRenewal.GetAllUserBranches Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if this user does have access to branches...
            If Information.IsArray(vBranchDetails) Then

                cboBranch.Items.Add("(All)")
                VB6.SetItemData(cboBranch, 0, 0)

                llBound = vBranchDetails.GetLowerBound(1)

                lUBound = vBranchDetails.GetUpperBound(1)

                For lBranch As Integer = llBound To lUBound

                    cboBranch.Items.Add(CStr(vBranchDetails(2, lBranch)))

                    VB6.SetItemData(cboBranch, cboBranch.Items.Count - 1, CInt(vBranchDetails(0, lBranch)))

                Next

            Else
                gPMFunctions.RaiseError(kMethodName, "Unable to find branches for user:" & g_oObjectManager.UserID, gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateAgentCbo
    '
    ' Parameters: n/a
    '
    ' Description: Populates the Agent combo
    '
    ' History:
    '           Created : Deepak 01 November 2006
    ' ***************************************************************** '
    Public Function PopulateAgentCbo() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateAgentCbo"

        Dim lReturn As Integer
        Dim vAgentArray(,) As Object
        Dim llBound, lUBound, lBranch As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If cboBranch.Text = "(All)" Or cboBranch.Text.Trim() = "" Then

                m_lReturn = m_oBusiness.GetAgents(vAgentArray)
            Else

                m_lReturn = m_oBusiness.GetAgents(vAgentArray, VB6.GetItemData(cboBranch, cboBranch.SelectedIndex))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' clear down the contents
            cboAgentCode.Items.Clear()

            Dim cboAgentCode_NewIndex As Integer = -1
            cboAgentCode_NewIndex = cboAgentCode.Items.Add("(All)")
            VB6.SetItemData(cboAgentCode, cboAgentCode_NewIndex, 0)

            cboAgentCode_NewIndex = cboAgentCode.Items.Add("<Direct>")
            VB6.SetItemData(cboAgentCode, cboAgentCode_NewIndex, -1)

            If Information.IsArray(vAgentArray) Then

                For iAgentCount As Integer = 0 To vAgentArray.GetUpperBound(1)

                    cboAgentCode_NewIndex = cboAgentCode.Items.Add(CStr(vAgentArray(1, iAgentCount)))

                    VB6.SetItemData(cboAgentCode, cboAgentCode_NewIndex, CInt(vAgentArray(0, iAgentCount)))
                Next iAgentCount
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PopulateBranchCbo(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vAddress1 As String = "", Optional ByRef bSuppressSubAgents As Boolean = False, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef bIsInTransferMode As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object

        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            oFindParty.CallingAppName = ACApp

            m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="REN", vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Set appropriate key if agent only

            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lReturn = oFindParty.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                If (vSpecialParty = "AG") Or (vSpecialParty = "UB") Or (vSpecialParty = "AH") Then

                    oFindParty.NotEditable = 1
                End If

                oFindParty.SuppressSubAgents = bSuppressSubAgents

                oFindParty.IsInTransferMode = bIsInTransferMode
            End If

            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOk Then

                vPartyCnt = oFindParty.PartyCnt

                vShortName = oFindParty.ShortName

                vDateCancelled = oFindParty.DateCancelled

                If Not Information.IsNothing(vName) Then

                    vName = oFindParty.LongName
                End If

                If Not Information.IsNothing(vResolvedName) Then

                    vResolvedName = oFindParty.ResolvedName
                End If

                ' Return address line1 if requested

                If Not Information.IsNothing(vAddress1) Then

                    m_lReturn = oFindParty.GetKeys(vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If

                    ' Walk the array to find the value

                    lLower = vKeyArray.GetLowerBound(1)

                    lUpper = vKeyArray.GetUpperBound(1)
                    For lCount As Integer = lLower To lUpper

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lCount)) = PMNavKeyConst.PMKeyNameAddLine1 Then

                            vAddress1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount))
                            Exit For
                        End If
                    Next
                End If
            Else

                result = oFindParty.Status
            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If Not (oFindParty Is Nothing) Then

                oFindParty.Dispose()

                oFindParty = Nothing
            End If


        End Try
        Return result
    End Function
End Class
