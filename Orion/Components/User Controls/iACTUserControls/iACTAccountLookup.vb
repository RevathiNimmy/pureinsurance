Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
'<System.Runtime.InteropServices.ProgId("AccountLookup_NET.AccountLookup")> _
Partial Public Class AccountLookup
    'Public Class AccountLookup
    Inherits System.Windows.Forms.UserControl
    Public Event AccountShortCodeChange()
    Public Event AccountNameChange()
    Public Event AccountCodeChange()
    Public Event DefaultChange()
    Public Event OnlyUpdatableAccountsChange()
    Public Event CompanyIdChange()
    Public Event AccountIdChange()
    Public Event WhatsThisHelpIDChange()
    Public Event ToolTipTextChange()
    Public Event TextChange()
    Public Event SelTextChange()
    Public Event SelStartChange()
    Public Event SelLengthChange()
    Public Event BackStyleChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()
    Public Event AccountLookup()
    ' History:
    ' CJB 070405 PN14472 Added new ShowEditOnFindAccount public property on
    '            uctAccountLookup to enable it to set iACTFindAccount.NotEditable.
    '

    Private m_oObjectManager As bObjectManager.ObjectManager

    'developer guide no. 88
    Private m_oAccountLookup As Object
    Private m_oAccount As Object


    ''Modified by Archana Tokas on 13/04/2010 04:54:25 comment as bACTExplorer is not converted yet
    'Private m_oExplorerBusiness As bACTExplorer.Form
    Private m_oExplorerBusiness As Object
    



    Private m_lReturn As Integer

    ' For drawing the control
    Const ACGapInTwips As Integer = 5

    Const ACClass As String = "AccountLookup"

    ' To save the last lookup

    Private m_lLookupAccountId As Integer
    Private m_sLookupAccountName As String = ""
    Private m_sLookupAccountShortCode As String = ""
    Private m_lLookupAccountType As Integer
    Private m_sLookupAccountCode As String = ""
    Private m_iLookupCompanyId As Integer 'PN6169


    ' For Validation of text on Property Get AccountId
    Private m_bTextChanged As Boolean
    Private m_sAccountCodeEntered As String = ""

    ' Allow selection of stopped accounts?
    Private m_bAllowStoppedAccounts As Boolean

    'DD 15/07/2002: True if new product option is enabled
    Private m_bEnhancedSecurity As Boolean

    'Default Property Values:
    Const m_def_AccountId As Integer = 0
    Const m_def_AccountName As String = ""
    Const m_def_AccountCode As String = ""
    Const m_def_AccountShortCode As String = ""
    Const m_def_AccountCompanyId As Integer = 0 'PN6169
    Const m_def_CompanyId As Integer = 0
    Const m_def_AllowStoppedAccounts As Boolean = False

    'Property Variables:
    Dim m_AccountId As Integer
    Dim m_AccountName As String = ""
    Dim m_AccountCode As String = ""
    Dim m_AccountShortCode As String = ""
    Dim m_AccountCompanyId As Integer 'PN6169

    Dim m_CompanyId As Integer
    Dim m_AccountType As Integer
    Private m_bShowEditOnFindAccount As Boolean 'PN14472
    'DD 15/07/2002
    Dim m_OnlyUpdatableAccounts As Boolean

    Private m_lNominalAccountID As Integer

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cmdAccountLookup,cmdAccountLookup,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=txtAccountCode,txtAccountCode,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=txtAccountCode,txtAccountCode,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=txtAccountCode,txtAccountCode,-1,KeyUp
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp
    Event Change(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=txtAccountCode,txtAccountCode,-1,Change

    <Browsable(True)> _
    Public Property AllowStoppedAccounts() As Boolean
        Get
            Return m_bAllowStoppedAccounts
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowStoppedAccounts = Value
        End Set
    End Property


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return txtAccountCode.BackColor
        End Get
        Set(ByVal Value As Color)
            txtAccountCode.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return txtAccountCode.ForeColor
        End Get
        Set(ByVal Value As Color)
            txtAccountCode.ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled

    <Browsable(True)> _
    <Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal Value As Boolean)
            MyBase.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return txtAccountCode.Font
        End Get
        Set(ByVal Value As Font)
            txtAccountCode.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,BackStyle

    <Browsable(True)> _
    <Description("Indicates whether a Label or the background of a Shape is transparent or opaque.")> _
    Public Property BackStyle() As Integer
        Get

            'developer guide no solution 14 
            'Return MyBase.BackStyle
        End Get
        Set(ByVal Value As Integer)

            'developer guide no solution 14 
            'MyBase.BackStyle = Value
            RaiseEvent BackStyleChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Property ShowEditOnFindAccount() As Boolean
        Get 'PN14472
            Return m_bShowEditOnFindAccount
        End Get
        Set(ByVal Value As Boolean) 'PN14472
            m_bShowEditOnFindAccount = Value
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,ActiveControl
    <Browsable(False)> _
    <Description("Returns the control that has focus.")> _
    Public Shadows ReadOnly Property ActiveControl() As Object
        Get
            Return MyBase.ActiveControl
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,SelLength

    <Browsable(True)> _
    <Description("Returns/sets the number of characters selected.")> _
    Public Property SelLength() As Integer
        Get
            Return txtAccountCode.SelectionLength
        End Get
        Set(ByVal Value As Integer)
            txtAccountCode.SelectionLength = Value
            RaiseEvent SelLengthChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,SelStart

    <Browsable(True)> _
    <Description("Returns/sets the starting point of text selected.")> _
    Public Property SelStart() As Integer
        Get
            Return txtAccountCode.SelectionStart
        End Get
        Set(ByVal Value As Integer)
            txtAccountCode.SelectionStart = Value
            RaiseEvent SelStartChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,SelText

    <Browsable(True)> _
    <Description("Returns/sets the string containing the currently selected text.")> _
    Public Property SelText() As String
        Get
            Return txtAccountCode.SelectedText
        End Get
        Set(ByVal Value As String)
            txtAccountCode.SelectedText = Value
            RaiseEvent SelTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,Text

    <Browsable(True)> _
    <Description("Returns/sets the text contained in the control.")> _
    Public Overrides Property Text() As String
        Get
            Return txtAccountCode.Text
        End Get
        Set(ByVal Value As String)
            txtAccountCode.Text = Value
            RaiseEvent TextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(txtAccountCode)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(txtAccountCode, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=txtAccountCode,txtAccountCode,-1,WhatsThisHelpID

    'Modified by Archana Tokas on 13/04/2010 05:25:31 refer developer guide no solution no 15
    '<Browsable(True)> _
    '<Description("Returns/sets an associated context number for an object.")> _
    'Public Property WhatsThisHelpID() As Integer
    '    Get

    '        Return txtAccountCode.WhatsThisHelpID
    '    End Get
    '    Set(ByVal Value As Integer)

    '        txtAccountCode.WhatsThisHelpID = Value
    '        RaiseEvent WhatsThisHelpIDChange()
    '    End Set
    'End Property


    <Browsable(False)> _
    Public Property AccountId() As Integer
        Get

            Dim vOptionValue As Object 'eck PN6169

            If m_sAccountCodeEntered <> "" And txtAccountCode.Text <> "" Then 'text may have changed so lets check it ...

                If (m_sAccountCodeEntered.IndexOf("\"c) + 1) = 0 Then 'it's a short code

                    If Me.Text <> Me.AccountShortCode Then

                        'eck PN6169

                        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=CStr(vOptionValue))


                        If CStr(vOptionValue) = "1" Then
                            'DJM 04/03/2004
                            'Need call the initialise function from main module to populate g_iSourceID.
                            Initialise()
                            'Tracy Richards - 27/04/03 For Multi Comapnay systems Use g_iSourceID instead of
                            'CompanyID as CompanyID is never populated
                            m_lReturn = GetAccountFromShort(Me.Text, m_OnlyUpdatableAccounts, m_AccountId, g_iSourceID)
                        Else
                            m_lReturn = GetAccountFromShort(Me.Text, m_OnlyUpdatableAccounts, m_AccountId)
                        End If
                        'eck PN6169 end
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            cmdAccountLookup_Click(cmdAccountLookup, New EventArgs())
                        Else
                            GetAccountDetail(m_AccountId)
                        End If

                    End If

                    m_sAccountCodeEntered = "" 'so we don't do this again unless a new code is typed in

                Else
                    'it's a full code

                    If Me.Text <> Me.AccountCode Then

                        m_lReturn = GetAccountFromFull(Me.Text, m_AccountId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            cmdAccountLookup_Click(cmdAccountLookup, New EventArgs())
                        Else
                            GetAccountDetail(m_AccountId)
                        End If

                    End If

                    m_sAccountCodeEntered = "" 'so we don't do this again unless a new code is typed in

                End If

                ' CF110399
                If Not m_bAllowStoppedAccounts Then
                    ' CF070399
                    If m_AccountId > 0 Then
                        m_lReturn = CheckAccountActive(v_lAccountId:=m_AccountId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            cmdAccountLookup_Click(cmdAccountLookup, New EventArgs())
                        End If
                    End If
                End If
            End If

            Return m_AccountId

        End Get
        Set(ByVal Value As Integer)
            Dim vOptionValue As String = ""

            'If Ambient.UserMode = False Then Err.Raise 382
            'If Ambient.UserMode Then Err.Raise 393
            m_AccountId = Value
            GetAccountDetail(m_AccountId)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                txtAccountCode.Text = m_sLookupAccountShortCode
                RaiseEvent AccountIdChange()
            Else
                Exit Property
            End If

        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property AccountType(Optional ByVal vAccountId As Object = Nothing) As Integer
        Get


            If Information.IsNothing(vAccountId) Then

                GetAccountDetail(CInt(vAccountId))
                Return m_lLookupAccountType
            Else
                Return m_AccountType
            End If

        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property AccountName(Optional ByVal vAccountId As Object = Nothing) As String
        Get


            If Not Information.IsNothing(vAccountId) Then

                GetAccountDetail(CInt(vAccountId))
                Return m_sLookupAccountName
            Else
                Return m_AccountName
            End If

        End Get
    End Property


    <Browsable(False)> _
    Public ReadOnly Property AccountCode(Optional ByVal vAccountId As Object = Nothing) As String
        Get

            If Not Information.IsNothing(vAccountId) Then

                GetAccountDetail(CInt(vAccountId))
                Return m_sLookupAccountCode
            Else
                Return m_AccountCode
            End If
        End Get
    End Property


    <Browsable(False)> _
    Public ReadOnly Property AccountShortCode(Optional ByVal vAccountId As Object = Nothing) As String
        Get

            If Not Information.IsNothing(vAccountId) Then

                GetAccountDetail(CInt(vAccountId))
                Return m_sLookupAccountShortCode
            Else
                Return m_AccountShortCode
            End If
        End Get
    End Property
    'eck PN6169
    <Browsable(False)> _
    Public ReadOnly Property AccountCompanyId(Optional ByVal vAccountId As Object = Nothing) As Integer
        Get

            If Not Information.IsNothing(vAccountId) Then

                GetAccountDetail(CInt(vAccountId))
                Return m_iLookupCompanyId
            Else
                Return m_CompanyId
            End If
        End Get
    End Property

    'CF 150199
    <Browsable(False)> _
    Public ReadOnly Property NominalAccountID() As Integer
        Get

            If m_AccountId = 0 Then
                m_lReturn = AccountId
            End If

            Return m_lNominalAccountID

        End Get
    End Property


    <Browsable(False)> _
    Public Property CompanyId() As Integer
        Get
            Return m_CompanyId
        End Get
        Set(ByVal Value As Integer)

            If DesignMode Then
                ' Throw New System.Exception("382")
            End If
            m_CompanyId = Value
            RaiseEvent CompanyIdChange()

        End Set
    End Property


    <Browsable(True)> _
    Public Property OnlyUpdatableAccounts() As Boolean
        Get
            Return m_OnlyUpdatableAccounts
        End Get
        Set(ByVal Value As Boolean)

            m_OnlyUpdatableAccounts = Value
            RaiseEvent OnlyUpdatableAccountsChange()

        End Set
    End Property


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cmdAccountLookup,cmdAccountLookup,-1,Default

    <Browsable(True)> _
    <Description("Determines which CommandButton control is the default command button on a form.")> _
    Public Property Default_Renamed() As Boolean
        Get
            If cmdAccountLookup.FindForm() Is Nothing Then
                Return False
            Else
                Return VB6.GetDefault(cmdAccountLookup)
            End If
        End Get
        Set(ByVal Value As Boolean)
            If Not (cmdAccountLookup.FindForm() Is Nothing) Then
                Me.FindForm.AcceptButton = cmdAccountLookup
            End If
            RaiseEvent DefaultChange()
        End Set
    End Property

    ' CF 200899 - Added following for customisation of
    ' user control from parent component.
    <Browsable(True)> _
    Public Property LookupCaption() As String
        Get
            Return cmdAccountLookup.Text
        End Get
        Set(ByVal Value As String)
            cmdAccountLookup.Text = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property LookupLeft() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(cmdAccountLookup.Left))
        End Get
        Set(ByVal Value As Integer)
            cmdAccountLookup.Left = VB6.TwipsToPixelsX(Value)
        End Set
    End Property
    <Browsable(True)> _
    Public Property LookupHeight() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsY(cmdAccountLookup.Height))
        End Get
        Set(ByVal Value As Integer)
            cmdAccountLookup.Height = VB6.TwipsToPixelsY(Value)
        End Set
    End Property
    <Browsable(True)> _
    Public Property LookupWidth() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(cmdAccountLookup.Width))
        End Get
        Set(ByVal Value As Integer)
            cmdAccountLookup.Width = VB6.TwipsToPixelsX(Value)
        End Set
    End Property
    <Browsable(True)> _
    Public Property LookupTextLeft() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(txtAccountCode.Left))
        End Get
        Set(ByVal Value As Integer)
            txtAccountCode.Left = VB6.TwipsToPixelsX(Value)
        End Set
    End Property
    <Browsable(True)> _
    Public Property LookupTextWidth() As Integer
        Get
            Return CInt(VB6.PixelsToTwipsX(txtAccountCode.Width))
        End Get
        Set(ByVal Value As Integer)
            txtAccountCode.Width = VB6.TwipsToPixelsX(Value)
        End Set
    End Property


    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Refresh
    Public Overrides Sub Refresh()
        MyBase.Refresh()
    End Sub


    Private Sub cmdAccountLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccountLookup.Click

        Dim vKeyArray(1, 8) As Object


        Dim iTextMode As Integer
        Dim sStartKey As String = ""

        Const CModeCode As Integer = 1
        Const CModeShort As Integer = 2
        Const CModeName As Integer = 3

        If m_oObjectManager Is Nothing Then
            m_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get instance of object manager", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        If m_oAccountLookup Is Nothing Then

            Dim temp_m_oAccountLookup As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oAccountLookup, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oAccountLookup = temp_m_oAccountLookup
        End If

        '  If m_oSettings Is Nothing Then
        '    Set m_oSettings = New iACTSettings.Interface
        '  End If

        With m_oAccountLookup

            If Me.CompanyId = 0 Then
                '      Me.CompanyId = m_oSettings.CompanyId
                Me.CompanyId = 1 ' fake it
                'PN6169 Pass company if Multi Structure
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.ACTKeyNameBranchID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = Me.CompanyId

            End If


            ' setup the key array to call find account

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = 0

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameLedgerID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 0

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameLedgerTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = 0

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = ""

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameFullKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = ""

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameAccountName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = ""

            'DD 15/07/2002: Filter the Accounts search if enhanced security
            'is enabled.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.ACTKeyNameOnlyUpdatableAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_OnlyUpdatableAccounts

            sStartKey = Me.Text

            ' A name ?
            'TODO This throws error if sStartKey is empty.
            If sStartKey.StartsWith("'") Then
                'If sStartKey.Substring(0, 1) = "'" Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = Mid(sStartKey, 2)
                iTextMode = CModeName
            Else
                If sStartKey.IndexOf("\"c) >= 0 Then ' its a full key
                    iTextMode = CModeCode

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameFullKey

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = sStartKey
                Else

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameShortCode

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = sStartKey
                    iTextMode = CModeShort
                End If
            End If

            ' CF110399

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyAllowStoppedAccounts


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = (m_bAllowStoppedAccounts)


            .CompanyId = Me.CompanyId


            m_lReturn = .Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error. Unable to initialise FindAccount object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            .NotEditable = Not ShowEditOnFindAccount 'PN14472


            m_lReturn = .SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=gPMConstants.PMTransactionTypeGeneric, vEffectiveDate:=DateTime.Now) 'vTypeOfBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = .SetKeys(vKeyArray)


            m_lReturn = .Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error. Unable to start FindAccount object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            If .Status <> gPMConstants.PMEReturnCode.PMCancel Then


                m_lReturn = .GetKeys(vKeyArray)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    m_AccountCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4))
                    '        m_AccountCode = .FullKey
                    RaiseEvent AccountCodeChange()

                    m_AccountId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0))
                    '        m_AccountId = .AccountId
                    RaiseEvent AccountIdChange()

                    m_AccountName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5))
                    'm_AccountName = .AccountName
                    RaiseEvent AccountNameChange()

                    m_AccountShortCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3))
                    '        m_AccountShortCode = .ShortCode
                    RaiseEvent AccountShortCodeChange()

                    Select Case iTextMode
                        Case CModeCode
                            Text = m_AccountCode
                        Case CModeShort
                            Text = m_AccountShortCode
                        Case CModeName
                            Text = "'" & m_AccountName
                    End Select


                    m_lNominalAccountID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7))

                    m_AccountCompanyId = Me.CompanyId 'PN6169

                    RaiseEvent Click(Me, Nothing)

                End If

                '    Else

                '       m_AccountShortCode = ""
                '       m_AccountName = ""
                '       m_AccountId = 0
                '       Text = ""
                '       m_AccountCompanyId = 0       'PN6169

            End If

        End With

    End Sub

    Private Sub txtAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Enter

        txtAccountCode.SelectionStart = 0
        txtAccountCode.SelectionLength = txtAccountCode.Text.Trim().Length

    End Sub

    Private Sub txtAccountCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.Leave

        If m_bTextChanged Then
            m_sAccountCodeEntered = txtAccountCode.Text 'we may have just entered some text so save it so we can test it later on the AccountId get
            m_bTextChanged = False

            RaiseEvent AccountLookup()
        End If


    End Sub

    Private Sub AccountLookup_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub txtAccountCode_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtAccountCode.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub
    'TODO
    'Private Sub txtAccountCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtAccountCode.KeyPress
    '    Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
    '    RaiseEvent KeyPress(Me, New KeyPressUserEventArgs(KeyAscii))
    '    If KeyAscii = 0 Then
    '        eventArgs.Handled = True
    '    End If
    '    eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub
    Private Sub txtAccountCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtAccountCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtAccountCode_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtAccountCode.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub


    Private Sub UserControl_Initialize()

        Try

            If m_oObjectManager Is Nothing And Not IsBuildMachine() Then
                m_oObjectManager = New bObjectManager.ObjectManager()

                m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to get instance of object manager", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If m_oExplorerBusiness Is Nothing Then
                    ' Get an Explorer Business Object
                    Dim temp_m_oExplorerBusiness As Object
                    m_lReturn = m_oObjectManager.GetInstance(temp_m_oExplorerBusiness, "bACTExplorer.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    m_oExplorerBusiness = temp_m_oExplorerBusiness

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the account explorer object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                        Exit Sub
                    End If
                End If
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub AccountLookup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub AccountLookup_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub AccountLookup_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAccountCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        RaiseEvent Change(Me, Nothing)
        m_bTextChanged = True
    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_AccountId = m_def_AccountId
        m_AccountName = m_def_AccountName
        m_AccountCode = m_def_AccountCode
        m_AccountShortCode = m_def_AccountShortCode
        m_AccountCompanyId = m_def_AccountCompanyId 'PN6169
        m_CompanyId = m_def_CompanyId
        m_bAllowStoppedAccounts = m_def_AllowStoppedAccounts
        'DD 15/07/2002
        m_OnlyUpdatableAccounts = False
    End Sub

    'Load property values from storage


    ''developer guide no solution no. 1 
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)



        txtAccountCode.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))



        txtAccountCode.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))


        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))



        'developer guide no solution no 2 
        Font = PropBag.ReadProperty("Font", Me.Font)




        'TODO no solution found yet
        'MyBase.BackStyle = PropBag.ReadProperty("BackStyle", 1)


        txtAccountCode.SelectionLength = CInt(PropBag.ReadProperty("SelLength", 0))


        txtAccountCode.SelectionStart = CInt(PropBag.ReadProperty("SelStart", 0))


        txtAccountCode.SelectedText = CStr(PropBag.ReadProperty("SelText", ""))


        txtAccountCode.Text = CStr(PropBag.ReadProperty("Text", ""))


        ToolTip1.SetToolTip(txtAccountCode, CStr(PropBag.ReadProperty("ToolTipText", "")))




        'developer guide no solution 15
        'txtAccountCode.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 460)



        m_AccountId = CInt(PropBag.ReadProperty("AccountId", m_def_AccountId))


        m_AccountName = CStr(PropBag.ReadProperty("AccountName", m_def_AccountName))


        m_AccountCode = CStr(PropBag.ReadProperty("AccountCode", m_def_AccountCode))


        m_AccountShortCode = CStr(PropBag.ReadProperty("AccountShortCode", m_def_AccountShortCode))


        m_AccountCompanyId = CInt(PropBag.ReadProperty("AccountCompanyId", m_def_AccountCompanyId)) 'PN6169


        m_CompanyId = CInt(PropBag.ReadProperty("CompanyId", m_def_CompanyId))


        If Not (cmdAccountLookup.FindForm() Is Nothing) Then
            'VB6.SetDefault(cmdAccountLookup, CBool(PropBag.ReadProperty("Default", False)))
            Me.FindForm.AcceptButton = cmdAccountLookup
        End If
        'DD 15/07/2002


        m_OnlyUpdatableAccounts = CBool(PropBag.ReadProperty("OnlyUpdatableAccounts", False))
    End Sub

    Private Sub AccountLookup_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        txtAccountCode.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(MyBase.Width) - VB6.PixelsToTwipsX(cmdAccountLookup.Width) - ACGapInTwips)
        cmdAccountLookup.Left = txtAccountCode.Width + VB6.TwipsToPixelsX(ACGapInTwips)
        MyBase.Height = txtAccountCode.Height
    End Sub

    Private Sub UserControl_Terminate()
        m_oAccountLookup = Nothing

        If Not (m_oExplorerBusiness Is Nothing) Then

            m_oExplorerBusiness.Dispose()
            m_oExplorerBusiness = Nothing
        End If

        If Not (m_oAccount Is Nothing) Then

            m_oAccount.Dispose()
            m_oAccount = Nothing
        End If
    End Sub

    'Write property values to storage



    'developer guide no solution no 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(txtAccountCode.BackColor), &H80000005)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(txtAccountCode.ForeColor), &H80000008)

        PropBag.WriteProperty("Enabled", MyBase.Enabled, True)



        'developer guide no solution no 2
        PropBag.WriteProperty("Font", Font, Me.Font)




        'developer guide no solution 14
        'PropBag.WriteProperty("BackStyle", MyBase.BackStyle, 1)

        PropBag.WriteProperty("SelLength", txtAccountCode.SelectionLength, 0)

        PropBag.WriteProperty("SelStart", txtAccountCode.SelectionStart, 0)

        PropBag.WriteProperty("SelText", txtAccountCode.SelectedText, "")

        PropBag.WriteProperty("Text", txtAccountCode.Text, "")

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(txtAccountCode), "")



        'developer guide no solution no 15
        'PropBag.WriteProperty("WhatsThisHelpID", txtAccountCode.WhatsThisHelpID, 460)


        PropBag.WriteProperty("AccountId", m_AccountId, m_def_AccountId)

        PropBag.WriteProperty("AccountName", m_AccountName, m_def_AccountName)

        PropBag.WriteProperty("AccountCode", m_AccountCode, m_def_AccountCode)

        PropBag.WriteProperty("AccountShortCode", m_AccountShortCode, m_def_AccountShortCode)

        PropBag.WriteProperty("AccountCompanyId", m_AccountCompanyId, m_def_AccountCompanyId) 'PN6169

        PropBag.WriteProperty("CompanyId", m_CompanyId, m_def_CompanyId)


        PropBag.WriteProperty("Default", ReflectionHelper.GetMember(cmdAccountLookup, "Default_Renamed"), False)
        'DD 15/07/2002

        PropBag.WriteProperty("OnlyUpdatableAccounts", m_OnlyUpdatableAccounts, False)

    End Sub

    Private Sub GetAccountDetail(ByRef lAccountId As Integer)

        ' don't look it up if no change
        If m_lLookupAccountId = lAccountId Then
            Exit Sub
        End If

        m_lLookupAccountId = lAccountId
        'PN6169 add company id
        m_lReturn = GetAccount(m_lLookupAccountId, m_sLookupAccountName, m_sLookupAccountShortCode, m_lLookupAccountType, m_sLookupAccountCode, m_lNominalAccountID, m_iLookupCompanyId)

        m_sAccountCodeEntered = m_sLookupAccountShortCode

    End Sub

    Private Function GetExplorerBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure that we have an object manager

            m_lReturn = Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oExplorerBusiness Is Nothing Then
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the account explorer business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExplorerBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckAccountActive
    '
    ' Description: Checks to see if an account's active or not
    '
    ' ***************************************************************** '
    Public Function CheckAccountActive(ByVal v_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Dim iAccountStatus As Integer
        Dim bIsStopped As Boolean

        Try

            result = GetAccountBusiness()
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Check if the account's active or stopped

            m_lReturn = m_oAccount.GetAccountStatus(v_lAccountId:=v_lAccountId, r_iAccountStatus:=iAccountStatus, r_bIsStopped:=bIsStopped)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If it's stopped then return false so the find account screen fires up
            If bIsStopped Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAccountActive Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAccountActive", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAccountBusiness() As Integer
        Dim result As Integer = 0
        Dim vValue As String = ""
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAccountBusiness
        ' PURPOSE: Connects to the Account.Form business object
        ' AUTHOR: Danny Davis
        ' DATE: 15/07/2002, 12:20
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' DD 15/07/2002: Moved here so that m_Account is available
        ' when we need it
        ' Make sure we have an instance of the account object
        If m_oAccount Is Nothing Then
            Dim temp_m_oAccount As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 15/07/2002: Get product option setting
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedOrionSecurity, g_iSourceID, vValue)
            m_bEnhancedSecurity = (vValue = "1")
        End If

        result = m_lReturn


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
         

        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse

        End Select

        Finally
        


        End Try
	Return result
    End Function

    'eck PN6169 Added optional company parameter
    Public Function GetAccountFromShort(ByVal v_sShortCode As String, ByVal v_bOnlyUpdatableAccounts As Boolean, ByRef r_lAccountId As Integer, Optional ByVal v_vCompanyId As Integer = 0) As Integer
        'DD 15/07/2002: Enhanced to support new security

        Dim result As Integer = 0
        Dim vAccountIds As Object
        Dim bHasUnrestrictedEnquiry, bHasUnrestrictedUpdate As Boolean
        Dim vCompanyId As Integer 'eck PN6169

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetExplorerBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'eck PN6169

            If Information.IsNothing(v_vCompanyId) Then

                vCompanyId = Nothing
            Else
                If v_vCompanyId > 0 Then
                    vCompanyId = v_vCompanyId
                Else

                    vCompanyId = Nothing
                End If
            End If



            'eck PN6169

            m_lReturn = m_oExplorerBusiness.GetAccountIdFromShort(v_sShortCode:=v_sShortCode, r_vAccountIds:=vAccountIds, v_vCompanyId:=vCompanyId)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Information.IsArray(vAccountIds) Then 'results returned

                If vAccountIds.GetUpperBound(0) > 0 Then 'multiple results so force look up
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    If m_bEnhancedSecurity Then
                        'Hook up the Accounts object
                        m_lReturn = GetAccountBusiness()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If

                        'Get the security on this account


                        m_lReturn = m_oAccount.GetAccountSecurity(v_lAccountId:=CInt(vAccountIds(0, 0)), r_bHasUnrestrictedEnquiry:=bHasUnrestrictedEnquiry, r_bHasUnrestrictedUpdate:=bHasUnrestrictedUpdate)

                        If (bHasUnrestrictedUpdate And v_bOnlyUpdatableAccounts) Or (bHasUnrestrictedEnquiry And Not v_bOnlyUpdatableAccounts) Then

                            r_lAccountId = CInt(vAccountIds(0, 0))
                        Else
                            'They don't have access to this Account
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else

                        r_lAccountId = CInt(vAccountIds(0, 0))
                    End If
                End If
            Else
                'no match
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get account details from short code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountFromShort", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetAccountFromFull(ByVal v_sFullCode As String, ByRef r_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure that we have an object manager & business object

            m_lReturn = GetExplorerBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oExplorerBusiness.GetAccountIdFromFullPath(v_sFullCode, r_lAccountId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get account details from short code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountFromFull", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'eck PN6169 Pass company id
    Public Function GetAccount(ByVal v_lAccountId As Integer, ByRef r_sAccountName As String, ByRef r_sAccountShortCode As String, ByRef r_lAccountType As Integer, ByRef r_sAccountCode As String, ByRef r_lNominalAccountID As Integer, ByRef r_iCompanyId As Integer) As Integer



        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure that we have an object manager & business object

            m_lReturn = GetExplorerBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_oExplorerBusiness.GetAccountDetails(lAccountId:=v_lAccountId, vAccountName:=r_sAccountName, vShortCode:=r_sAccountShortCode, vAccountType:=r_lAccountType, vFullKey:=r_sAccountCode, vNominalCode:=r_lNominalAccountID, vCompanyId:=r_iCompanyId) 'PN6169

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get account details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
