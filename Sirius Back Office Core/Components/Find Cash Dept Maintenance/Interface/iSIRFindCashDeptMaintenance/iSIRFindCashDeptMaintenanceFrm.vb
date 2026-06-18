Option Strict Off
Option Explicit On
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRFindCashDeptMaintenance.General

    Private m_oBusiness As Object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_sTransactionType As String = ""
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_iTask As Integer
    Private m_bIsClient As Boolean
    Private m_lReturn As Integer
    Private m_IIsComplaint As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyResolvedName As String = ""
    Private m_lPartyCnt As Integer
    Private m_sAgentCode As String = ""
    Private m_lAgentCnt As Integer
    Private m_sAgentResolvedName As String = ""
    Private m_sCashDepositRef As String = ""
    Private m_sBankCode As String = ""
    Private m_vBankNameId As String = ""
    Private m_sShortCode As String = ""
    Private m_sAccountName As String = ""
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    Public Property PartyCode() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
        End Set
    End Property


    Public Property AgentCode() As String
        Get
            Return m_sAgentCode
        End Get
        Set(ByVal Value As String)
            m_sAgentCode = Value
        End Set
    End Property

    Public ReadOnly Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
    End Property

    Public ReadOnly Property AgentCnt() As Integer
        Get
            Return m_lAgentCnt
        End Get
    End Property

    Public ReadOnly Property CashDepositRef() As String
        Get
            Return m_sCashDepositRef
        End Get
    End Property


    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
        End Set
    End Property


    Public Property EnablePartialWildcardSearchOption() As Boolean
        Get
            Return m_bEnablePartialWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bEnablePartialWildcardSearchOption = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Process the next set of actions.
        m_lReturn = m_oGeneral.ProcessCommand()

        ' Check the return value.
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            ' Everything OK, so we can hide the interface.
            Me.Hide()
        End If

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        m_lReturn = DataToProperties()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdFindNow_Click", "DataToProperties Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click
        ' Clear the interface details.
        m_lReturn = ClearInterface()
        uctCashDepositControl.ClearListView()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdNewSearch_Click", "ClearInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()
            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch ex As Exception


            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdOK_Click", r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try




    End Sub

    Private Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Initialise"

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRFindCashDeptMaintenance.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch ex As Exception




            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try



    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try
            'Developer Guide no. 220
            Me.cboBankName.FirstItem = "(None)"
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Load", "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the header of the uctCashDepositControl
            'developer guide no. 9
            uctCashDepositControl.Initialise()
            uctCashDepositControl.IsFind = True
            m_lReturn = uctCashDepositControl.SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Load", "SetInterfaceDefaults of uctCashDepositControl Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch ex As Exception


            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Form_Load", r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        End Try



    End Sub

    Private Sub CheckMandatoryEnable()
        ' Check mandatory and enable the Find Now button accordingly
        If CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue Then
            cmdFindNow.Enabled = True
        Else
            cmdFindNow.Enabled = False
            uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
        End If
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAgent_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgent.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If txtAgent.Text <> "" Then
            If txtClient.Text.Trim() = "" Then
                m_lReturn = ClearClientOrAgent(bClient:=False)
            End If
        Else
            txtClient.BackColor = SystemColors.Window
            txtClient.Enabled = True
        End If
        CheckMandatoryEnable()
    End Sub

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.3)
    Private Sub txtAgent_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgent.Leave
        Const kMethodName As String = "txtAgent_LostFocus"
        Try


            m_bIsClient = False
            If gPMFunctions.ToSafeString(txtAgent.Text.Trim()) <> "" Then
                m_lReturn = ValidateWildCardSearch(txtAgent.Text.Trim())
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    txtAgent.Text = ""
                    txtAgent.Focus()
                    cmdFindNow.Enabled = False
                    Exit Sub
                End If

                m_lReturn = GetAndValidateParty(v_bIsClient:=False)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lReturn = GetPartyInfo(v_bIsClient:=False, v_sShortName:=txtAgent.Text, v_bSearchPartyWithShortCode:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        gPMFunctions.RaiseError("CmdClient_Click", "GetPartyInfo Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=False)
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
                    gPMFunctions.RaiseError("CmdClient_Click", "GetAndValidateParty Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.3)

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.4)
    Private Sub cmdAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgent.Click

        'First Clear the interface
        m_lReturn = ClearClientOrAgent(bClient:=False)

        'Make a call to GetPartyInfo method to load Find Agent screen
        m_lReturn = GetPartyInfo(v_bIsClient:=False)

        m_bIsClient = False

    End Sub
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.4)

    Private Sub txtCashDepositNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCashDepositNumber.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtCashDepositNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCashDepositNumber.Leave
        m_lReturn = ValidateWildCardSearch(txtCashDepositNumber.Text.Trim())
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            txtCashDepositNumber.Text = ""
            txtCashDepositNumber.Focus()
            cmdFindNow.Enabled = False
            Exit Sub
        End If
    End Sub
    Private Sub txtClient_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClient.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If txtClient.Text.Trim() <> "" Then

            If txtAgent.Text.Trim() = "" Then
                m_lReturn = ClearClientOrAgent(bClient:=True)
            End If
        Else
            txtAgent.BackColor = SystemColors.Window
            txtAgent.Enabled = True
        End If
        CheckMandatoryEnable()
    End Sub

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.2)
    Private Sub txtClient_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClient.Leave
        Const kMethodName As String = "txtClient_LostFocus"
        Try


            m_bIsClient = True
            If gPMFunctions.ToSafeString(txtClient.Text.Trim()) <> "" Then
                m_lReturn = ValidateWildCardSearch(txtClient.Text.Trim())
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    txtClient.Text = ""
                    txtClient.Focus()
                    cmdFindNow.Enabled = False
                    Exit Sub
                End If

                m_lReturn = GetAndValidateParty(v_bIsClient:=True)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lReturn = GetPartyInfo(v_bIsClient:=True, v_sShortName:=txtClient.Text, v_bSearchPartyWithShortCode:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        gPMFunctions.RaiseError("CmdClient_Click", "GetPartyInfo Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=False)
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
                    gPMFunctions.RaiseError("CmdClient_Click", "GetAndValidateParty Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.2)

    'UPGRADE_NOTE: (7001) The following declaration (txtBankCode_Change) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtBankCode_Change()
    'CheckMandatoryEnable()
    'End Sub

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.5)
    Private Sub cmdClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClient.Click

        'First Clear the interface
        m_lReturn = ClearClientOrAgent(bClient:=True)
        '    If m_lReturn <> PMTrue Then
        '        RaiseError "CmdClient_Click", "ClearInterface Failed", PMLogError
        '    End If

        'Make a call to GetPartyInfo method to load Find Agent screen
        m_lReturn = GetPartyInfo(v_bIsClient:=True)

        m_bIsClient = True

    End Sub
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.5)

    Public Function GetAndValidateParty(ByVal v_bIsClient As Boolean) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "GetAndValidateParty"

        Dim oFindParty As bSIRFindParty.Business
        Dim sPartyCode As String = ""
        Dim vSearchData As Object = Nothing
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            'Arul-(PN 65553-Bug Fixing)
            If txtAgent.Text.Trim().IndexOf("%"c) >= 0 Or txtClient.Text.Trim().IndexOf("%"c) >= 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oFindParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oFindParty = temp_oFindParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of the business object", gPMConstants.PMELogLevel.PMLogError)
            End If

            oFindParty.GetStructure("")
            If v_bIsClient Then
                sPartyCode = txtClient.Text

                m_lReturn = oFindParty.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=vSearchData, v_vShortName:=sPartyCode, v_vClientType:="<ALL>", v_vStatusType:="Client")
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    If Not gPMFunctions.IsArrayEmpty(vSearchData) Then

                        m_lPartyCnt = gPMFunctions.ToSafeLong(CStr(vSearchData(0, 0)))

                        m_sPartyCode = gPMFunctions.ToSafeString(CStr(vSearchData(2, 0))).Trim()

                        m_sPartyResolvedName = gPMFunctions.ToSafeString(CStr(vSearchData(3, 0))).Trim()
                        'Start - Sankar - PN65555
                        uctCashDepositControl.PartyCode = m_sPartyCode
                        uctCashDepositControl.PartyCnt = m_lPartyCnt
                        uctCashDepositControl.PartyName = m_sPartyResolvedName
                        uctCashDepositControl.IsFind = True
                        'End - Sankar - PN65555
                    End If
                End If
            Else

                m_lReturn = oFindParty.SearchSpecialPartyByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=vSearchData, v_vShortName:=txtAgent.Text.Trim(), v_vClientType:="Agent", v_vAgentType:="<ALL>", v_vStatusType:="Client", v_vInsurerType:="<ALL>")
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    If Not gPMFunctions.IsArrayEmpty(vSearchData) Then

                        m_lAgentCnt = gPMFunctions.ToSafeLong(CStr(vSearchData(0, 0)))

                        m_sAgentCode = gPMFunctions.ToSafeString(CStr(vSearchData(2, 0))).Trim()

                        m_sAgentResolvedName = gPMFunctions.ToSafeString(CStr(vSearchData(3, 0))).Trim()
                        'Start - Sankar - PN65555
                        uctCashDepositControl.PartyCode = m_sAgentCode
                        uctCashDepositControl.PartyCnt = m_lAgentCnt
                        uctCashDepositControl.PartyName = m_sAgentResolvedName
                        uctCashDepositControl.IsFind = True
                        'End - Sankar - PN65555
                    End If
                End If
            End If

            Return m_lReturn

        Catch ex As Exception


            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here




            Return result
        End Try
    End Function
    Private Function ValidateWildCardSearch(ByVal sValidateText As String) As Integer

        Dim result As Integer = 0
        Dim sWildcardErrorMessage As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=sValidateText, r_sErrorMessage:=sWildcardErrorMessage) Then

            MessageBox.Show(sWildcardErrorMessage, "Find Cash Deposit")
            '        txtClient.SetFocus
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function

    Public Function GetPartyInfo(ByVal v_bIsClient As Boolean, Optional ByVal v_sShortName As String = "", Optional ByVal v_bSearchPartyWithShortCode As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object

        Const kMethodName As String = "GetPartyInfo"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oFindParty As iPMBFindParty.Interface_Renamed

            ' Create Find Party object
            Dim temp_oFindParty As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set component properties and start interface
            If Not v_bIsClient Then

                oFindParty.SpecialParty = "AG"
            End If


            oFindParty.CallingAppName = ACApp

            oFindParty.IsComplaint = m_IIsComplaint

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = 1

            oFindParty.EnableNewParty = True
            If gPMFunctions.ToSafeString(v_sShortName) <> "" Then

                oFindParty.SearchPartyWithShortCode = v_bSearchPartyWithShortCode
                If v_bIsClient Then

                    oFindParty.ShortName = txtClient.Text
                Else

                    oFindParty.ShortName = txtAgent.Text
                End If
            End If

            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Retrieve party details
                If v_bIsClient Then

                    m_sPartyCode = oFindParty.ShortName

                    m_sPartyResolvedName = oFindParty.LongName

                    m_lPartyCnt = oFindParty.PartyCnt
                    txtClient.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPartyCode.Trim())
                Else

                    m_sAgentCode = oFindParty.ShortName

                    m_lAgentCnt = oFindParty.PartyCnt

                    m_sAgentResolvedName = oFindParty.LongName
                    txtAgent.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sAgentCode.Trim())
                End If

                ' Destroy Find Party object

                oFindParty.Dispose()
                oFindParty = Nothing


                If v_bIsClient Then
                    txtAgent.Text = ""
                Else
                    txtClient.Text = ""
                End If

                'developer guide no. 9
                m_lReturn = uctCashDepositControl.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "uctCashDepositControl.Initialise Failed", gPMConstants.PMELogLevel.PMLogOnError)
                End If

                If v_bIsClient Then
                    uctCashDepositControl.PartyCode = m_sPartyCode
                    uctCashDepositControl.PartyCnt = m_lPartyCnt
                    uctCashDepositControl.PartyName = m_sPartyResolvedName
                    uctCashDepositControl.IsFind = True
                Else
                    uctCashDepositControl.PartyCode = m_sAgentCode
                    uctCashDepositControl.PartyCnt = m_lAgentCnt
                    uctCashDepositControl.PartyName = m_sAgentResolvedName
                    uctCashDepositControl.IsFind = True
                End If
                uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=False)
            ElseIf oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'Start - Sankar - PN65555
                If v_bIsClient Then
                    m_lPartyCnt = 0
                    m_sPartyCode = ""
                    uctCashDepositControl.ClearListView()
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
                Else
                    m_lAgentCnt = 0
                    m_sAgentCode = ""
                    uctCashDepositControl.ClearListView()
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
                End If
                'End - Sankar - PN65555
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


    Public Function ClearClientOrAgent(ByRef bClient As Boolean) As Integer
        If bClient Then
            txtAgent.Text = ""
            m_sAgentCode = ""
            m_lAgentCnt = 0
            m_sAgentResolvedName = ""
            txtAgent.Enabled = False
            txtAgent.BackColor = SystemColors.GrayText
            txtClient.Enabled = True
            txtClient.BackColor = SystemColors.Window
        Else
            txtClient.Text = ""
            m_sPartyCode = ""
            m_lPartyCnt = 0
            m_sPartyResolvedName = ""
            txtClient.Enabled = False
            txtClient.BackColor = SystemColors.GrayText
            txtAgent.Enabled = True
            txtAgent.BackColor = SystemColors.Window
        End If
    End Function

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.9)
    Public Function DataToProperties() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DataToProperties"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If txtAgent.Text.Trim() = "" And txtClient.Text.Trim() = "" Then
                MessageBox.Show("Please select a Client or Agent", "Validation Failed", MessageBoxButtons.OK)
                cmdFindNow.Enabled = False
                Return result
            End If

            If txtAgent.Text.Trim() = "%%%" Or txtClient.Text.Trim() = "%%%" Then
                MessageBox.Show("Please select a Client or Agent", "Validation Failed", MessageBoxButtons.OK)
                cmdFindNow.Enabled = False
                Return result
            End If

            If txtCashDepositNumber.Text.Trim() = "" And CBool(CStr(txtClient.Text = "").Trim()) And CBool(CStr(txtAgent.Text = "").Trim()) And cboBankName.ListIndex > 0 Then
                MessageBox.Show("Please enter search criteria", "Validation Failed", MessageBoxButtons.OK)
                cmdFindNow.Enabled = False
                Return result
            End If

            'Start - Sankar - PN 65555
            If CBool(CStr(txtClient.Text <> "").Trim()) Then
                m_lReturn = GetAndValidateParty(v_bIsClient:=True)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
                    m_lReturn = GetPartyInfo(v_bIsClient:=True, v_sShortName:=txtClient.Text, v_bSearchPartyWithShortCode:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        gPMFunctions.RaiseError("DataToProperties", "GetPartyInfo Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            If CBool(CStr(txtAgent.Text <> "").Trim()) Then
                m_lReturn = GetAndValidateParty(v_bIsClient:=False)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
                    m_lReturn = GetPartyInfo(v_bIsClient:=False, v_sShortName:=txtAgent.Text, v_bSearchPartyWithShortCode:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        gPMFunctions.RaiseError("DataToProperties", "GetPartyInfo Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If
            'End - Sankar - PN 65555

            m_sCashDepositRef = txtCashDepositNumber.Text
            '    Start -Sankar - PN65555
            '    m_sPartyCode = txtClient.Text
            '    m_sAgentCode = txtAgent.Text
            '    End - Sankar - PN65555
            If cboBankName.ListIndex <> 0 Then
                m_sBankCode = cboBankName.ItemCode
            Else
                m_sBankCode = ""
            End If
            'developer guide no. 97
            uctCashDepositControl.Initialise()
            If m_bIsClient Then
                uctCashDepositControl.PartyCode = m_sPartyCode
                uctCashDepositControl.PartyCnt = m_lPartyCnt
                uctCashDepositControl.PartyName = m_sPartyResolvedName
                uctCashDepositControl.IsClient = True
            Else
                uctCashDepositControl.PartyCode = m_sAgentCode
                uctCashDepositControl.PartyCnt = m_lAgentCnt
                uctCashDepositControl.PartyName = m_sAgentResolvedName
                uctCashDepositControl.IsClient = False
            End If

            uctCashDepositControl.BankCode = m_sBankCode
            uctCashDepositControl.CashDepositRef = m_sCashDepositRef
            uctCashDepositControl.IsFind = True
            'Arul-(PN 65553-Bug Fixing)
            uctCashDepositControl.FindCashDepositRef = txtCashDepositNumber.Text.Trim()

            'developer guide no. 108
            m_lReturn = uctCashDepositControl.Load_Renamed()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "uctCashDepositControl.Load Failed", gPMConstants.PMELogLevel.PMLogOnError)
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
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.1.2.9)

    'Start - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.2.1.2)

    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception



            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                gPMFunctions.RaiseError(kMethodName, "Unable to Retrive Information from Resource File", gPMConstants.PMELogLevel.PMLogError)
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClient, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBank, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDepositNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCDNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch ex As Exception



            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ClearInterface"

        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If


            txtAgent.Text = ""
            txtClient.Text = ""
            txtCashDepositNumber.Text = ""
            cboBankName.ListIndex = 0
            txtClient.Enabled = True
            txtAgent.Enabled = True
            txtClient.BackColor = SystemColors.Window
            txtAgent.BackColor = SystemColors.Window

            m_lAgentCnt = 0
            m_lPartyCnt = 0
            m_sAgentCode = ""
            m_sPartyCode = ""
            m_sCashDepositRef = ""
            m_sBankCode = ""
            m_sPartyResolvedName = ""
            m_sAgentResolvedName = ""

            'Start - Sankar - PN 65555
            uctCashDepositControl.PartyCnt = 0
            uctCashDepositControl.PartyName = ""
            uctCashDepositControl.BankCode = ""
            uctCashDepositControl.CashDepositRef = ""
            'End - Sankar - PN 65555

            ' Set focus to the search details.
            txtClient.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            Return result

        Catch ex As Exception




            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisableInterface"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Enabled = Not bDisable

            Return result

        Catch ex As Exception



            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckMandatory"

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.
            ' At least one field must be populated
            If (txtAgent.Text.Trim() = "") And (txtClient.Text.Trim() = "") Then
                uctCashDepositControl.SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
            End If

            If txtAgent.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtClient.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtCashDepositNumber.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cboBankName.ListIndex > 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch ex As Exception



            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function


End Class
