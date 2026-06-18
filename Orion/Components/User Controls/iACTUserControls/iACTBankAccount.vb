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
<System.Runtime.InteropServices.ProgId("BankAccount_NET.BankAccount")> _
Partial Public Class BankAccount
    Inherits System.Windows.Forms.UserControl
    Public Event BankAccountNoChange()
    Public Event BankAccountNameChange()
    Public Event AccountIdChange()
    Public Event CodeChange()
    Public Event DescriptionChange()
    Public Event FirstItemChange()
    Public Event DefaultIdChange()
    Public Event IdChange()
    Public Event WhatsThisHelpIDChange()
    Public Event ToolTipTextChange()
    Public Event TextChange()
    Public Event ListIndexChange()
    Public Event ListChange()
    Public Event ItemDataChange()
    Public Event FontChange()
    Public Event EnabledChange()

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oObjectManager As bObjectManager.ObjectManager
    Private m_colBankAccountDetails As Collection
    Private m_oBankAccountDetail As BankAccountDetail
    Private m_oBankBusiness As Object

    Private Const ACClass As String = "BankAccount"

    ' Public source and language ID's from the
    ' Object Manager.
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer

    'Default Property Values:
    Const m_def_Id As Integer = 0
    Const m_def_Description As String = ""
    Const m_def_Code As String = ""
    Const m_def_DefaultId As Integer = 0
    Const m_def_FirstItem As String = ""

    'Property Variables:
    Dim m_Id As Integer
    Dim m_Description As String = ""
    Dim m_Code As String = ""
    Dim m_DefaultId As Integer
    Dim m_FirstItem As String = ""

    Dim m_AccountId As Integer
    Dim m_BankAccountName As String = ""
    Dim m_BankAccountNo As String = ""
    Dim m_CurrencyId As Integer
    Dim m_iCompamyId As Integer
    Dim m_iIsCashReceiveInThisCurrencyOnly As Integer

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboLookup,cboLookup,-1,Click
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=UserControl,UserControl,-1,DblClick

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Enabled

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets a value that determines whether an object can respond to user-generated events.")> _
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
    'MappingInfo=cboLookup,cboLookup,-1,Font

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboLookup.Font
        End Get
        Set(ByVal Value As Font)
            cboLookup.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,ActiveControl
    <Browsable(False)> _
    <System.ComponentModel.Description("Returns the control that has focus.")> _
    Public Shadows ReadOnly Property ActiveControl() As Object
        Get
            Return MyBase.ActiveControl
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,ItemData

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets a specific number for each item in a ComboBox or ListBox control.")> _
    Public Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboLookup, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboLookup, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,List

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets the items contained in a control's list portion.")> _
    Public Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboLookup, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboLookup, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,ListCount
    <Browsable(False)> _
    <System.ComponentModel.Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboLookup.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,ListIndex

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboLookup.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                cboLookup.SelectedIndex = Value
            End If

            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,NewIndex
    <Browsable(False)> _
    <System.ComponentModel.Description("Returns the index of the item most recently added to a control.")> _
    Public ReadOnly Property NewIndex() As Integer
        Get
            Dim cboLookup_NewIndex As Integer = -1
            Return cboLookup_NewIndex
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,Text

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets the text contained in the control.")> _
    Public Overrides Property Text() As String
        Get
            Return cboLookup.Text
        End Get
        Set(ByVal Value As String)
            cboLookup.Text = Value
            RaiseEvent TextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,ToolTipText

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboLookup)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboLookup, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,WhatsThisHelpID

    <Browsable(True)> _
    <System.ComponentModel.Description("Returns/sets an associated context number for an object.")> _
    Public Property WhatsThisHelpID() As Integer
        Get

            'developer guide no solution no 15
            'Return cboLookup.WhatsThisHelpID
        End Get
        Set(ByVal Value As Integer)


            'developer guide no solution no 15
            'cboLookup.WhatsThisHelpID = Value

            RaiseEvent WhatsThisHelpIDChange()
        End Set
    End Property


    <Browsable(False)> _
    Public Property Id() As Integer
        Get
            Return m_Id
        End Get
        Set(ByVal Value As Integer)
            'If DesignMode Then Throw New System.Exception("382")


            m_Id = Value
            RaiseEvent IdChange()
            Dim nIndex As Integer = IndexOfItem(cboLookup, Value)

            If nIndex >= 0 Then
                cboLookup.SelectedIndex = nIndex
            End If

        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Description(Optional ByVal v_vItemId As Object = Nothing) As String
        Get

            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return m_Description
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return m_def_Description
                Else
                    Return m_oBankAccountDetail.Description
                End If
            End If

        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Code(Optional ByVal v_vItemId As Object = Nothing) As String
        Get

            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return m_Code
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return m_def_Code
                Else
                    Return m_oBankAccountDetail.Code
                End If
            End If
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property AccountId(Optional ByVal v_vItemId As Object = Nothing) As String
        Get
            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return CStr(m_AccountId)
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return CStr(0)
                Else
                    Return CStr(m_oBankAccountDetail.AccountId)
                End If
            End If
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property BankAccountName(Optional ByVal v_vItemId As Object = Nothing) As String
        Get
            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return m_BankAccountName
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return ""
                Else
                    Return m_oBankAccountDetail.BankAccountName
                End If
            End If

        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property BankAccountNo(Optional ByVal v_vItemId As Object = Nothing) As String
        Get
            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return m_BankAccountNo
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return ""
                Else
                    Return m_oBankAccountDetail.BankAccountNo
                End If
            End If

        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property CurrencyId(Optional ByVal v_vItemId As Object = Nothing) As String
        Get
            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return CStr(m_CurrencyId)
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return CStr(0)
                Else
                    Return CStr(m_oBankAccountDetail.CurrencyId)
                End If
            End If

        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property IsCashReceiveInThisCurrencyOnly(Optional ByVal v_vItemId As Object = Nothing) As String
        Get
            'If DesignMode Then Throw New System.Exception("382")


            If Information.IsNothing(v_vItemId) Then
                Return CStr(m_iIsCashReceiveInThisCurrencyOnly)
            Else

                m_oBankAccountDetail = Item(CInt(v_vItemId))
                If m_oBankAccountDetail Is Nothing Then
                    Return CStr(0)
                Else
                    Return CStr(m_oBankAccountDetail.IsCashReceiveInThisCurrencyOnly)
                End If
            End If

        End Get
    End Property

    <Browsable(True)> _
    Public Property DefaultId() As String
        Get
            Return CStr(m_DefaultId)
        End Get
        Set(ByVal Value As String)
            m_DefaultId = CInt(Value)
            RaiseEvent DefaultIdChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Property FirstItem() As String
        Get
            Return m_FirstItem
        End Get
        Set(ByVal Value As String)
            m_FirstItem = Value
            'developer guide no. 131
            If (Not String.IsNullOrEmpty(m_FirstItem)) Then
                If m_FirstItem.Substring(0, 1) <> "(" Then
                    m_FirstItem = "(" & m_FirstItem
                End If
                If Not m_FirstItem.EndsWith(")") Then
                    m_FirstItem = m_FirstItem & ")"
                End If
            End If
            RaiseEvent FirstItemChange()

            If Not DesignMode Then
                RefreshList()
            End If

        End Set
    End Property
    <Browsable(False)> _
    Public WriteOnly Property CompanyId() As Integer
        Set(ByVal Value As Integer)
            m_iCompamyId = Value
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,Refresh
    Public Overrides Sub Refresh()
        MyBase.Refresh()
    End Sub

    Private Sub cboLookup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLookup.SelectedIndexChanged
        With cboLookup
            If .SelectedIndex <> -1 Then
                m_oBankAccountDetail = Item(VB6.GetItemData(cboLookup, .SelectedIndex))
                m_Description = .Text
                RaiseEvent DescriptionChange()
                m_Id = VB6.GetItemData(cboLookup, .SelectedIndex)
                RaiseEvent IdChange()
                If Not (m_oBankAccountDetail Is Nothing) Then
                    m_Code = m_oBankAccountDetail.Code
                    RaiseEvent CodeChange()
                    m_AccountId = m_oBankAccountDetail.AccountId
                    RaiseEvent AccountIdChange()
                    m_BankAccountName = m_oBankAccountDetail.BankAccountName
                    RaiseEvent BankAccountNameChange()
                    m_BankAccountNo = m_oBankAccountDetail.BankAccountNo
                    RaiseEvent BankAccountNoChange()
                End If
            End If
        End With
        RaiseEvent Click(Me, Nothing)
    End Sub

    Private Sub BankAccount_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,AddItem
    Public Sub AddItem(ByRef Item As String, Optional ByRef vItemId As Integer = 0)
        Dim cboLookup_NewIndex As Integer = -1
        cboLookup_NewIndex = cboLookup.Items.Add(Item)

        If Not Information.IsNothing(vItemId) Then
            VB6.SetItemData(cboLookup, cboLookup_NewIndex, vItemId)
        End If
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboLookup,cboLookup,-1,RemoveItem
    Public Sub RemoveItem(ByRef Index As Integer)

        Dim lId As Integer = VB6.GetItemData(cboLookup, Index)
        If Not (Item(lId) Is Nothing) Then
            m_colBankAccountDetails.Remove(lId)
        End If

        cboLookup.Items.RemoveAt(CShort(Index))

    End Sub


    Private Sub UserControl_Initialize()

        Try

            ' Have we been here before ?
            If m_oObjectManager Is Nothing And Not IsBuildMachine() Then

                ' Create an instance of the object manager.
                m_oObjectManager = New bObjectManager.ObjectManager()

                ' Call the initialise method.
                m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to call the initialise method.

                    ' Set the object manager to nothing.
                    m_oObjectManager = Nothing

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                    Exit Sub
                End If

                ' Store the language ID from the object manager
                ' to the public variables, to enable us to use
                ' them throughout the object.
                With m_oObjectManager
                    g_iLanguageID = .LanguageID
                    g_iSourceID = .SourceID
                End With

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_Id = m_def_Id
        m_Description = m_def_Description
        m_Code = m_def_Code
        m_DefaultId = m_def_DefaultId
        m_FirstItem = m_def_FirstItem
    End Sub

    'Load property values from storage


    'developer guide no solution no 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))


        'developer guide no solution no 2
        Font = PropBag.ReadProperty("Font", Me.Font)


        ToolTip1.SetToolTip(cboLookup, CStr(PropBag.ReadProperty("ToolTipText", "")))



        'developer guide no solution no 15
        'cboLookup.WhatsThisHelpID = PropBag.ReadProperty("WhatsThisHelpID", 0)


        m_DefaultId = CInt(PropBag.ReadProperty("DefaultId", m_def_DefaultId))


        m_FirstItem = CStr(PropBag.ReadProperty("FirstItem", m_def_FirstItem))
        If Not DesignMode Then
            RefreshList()
        End If
    End Sub

    Private Sub BankAccount_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        MyBase.Height = cboLookup.Height
        cboLookup.Width = MyBase.Width
    End Sub

    Private Sub UserControl_Terminate()
        m_colBankAccountDetails = Nothing
        m_oBankAccountDetail = Nothing

        If Not (m_oBankBusiness Is Nothing) Then

            m_oBankBusiness.Dispose()
            m_oBankBusiness = Nothing
        End If

    End Sub

    'Write property values to storage


    'developer guide no solution no 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("Enabled", MyBase.Enabled, True)


        'developer guide no solution no 1
        PropBag.WriteProperty("Font", Font, Me.Font)

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboLookup), "")


        'developer guide no solution no 15
        'PropBag.WriteProperty("WhatsThisHelpID", cboLookup.WhatsThisHelpID, 0)

        PropBag.WriteProperty("DefaultId", m_DefaultId, m_def_DefaultId)

        PropBag.WriteProperty("FirstItem", m_FirstItem, m_def_FirstItem)
    End Sub

    ' Read or Reread the list of entries
    Public Sub RefreshList()

        Dim nIndex As Integer

        If Not DesignMode And Not IsBuildMachine() Then
            cboLookup.Items.Clear()
            m_colBankAccountDetails = Nothing
            m_colBankAccountDetails = New Collection()

            m_lReturn = CType(GetBanks(m_colBankAccountDetails), gPMConstants.PMEReturnCode)

            ' Entry at top of list for (All) or (None) etc
            If m_FirstItem <> "" Then
                AddItem(m_FirstItem, ACDefaultFirstItem)
            End If

            For Each m_oBankAccountDetail As BankAccountDetail In m_colBankAccountDetails
                With m_oBankAccountDetail
                    ' Pick up missing descriptions
                    Select Case True
                        Case .Description.Trim().Length
                            AddItem(.Description, .Id)
                        Case .BankAccountName.Trim().Length
                            AddItem(.BankAccountName, .Id)
                        Case Else
                            AddItem(.Code.Trim() & " " & .BankAccountNo.Trim(), .Id)
                    End Select
                End With
            Next m_oBankAccountDetail

            ' Having filled the combo set it to it's default position
            If m_DefaultId <> 0 Then
                nIndex = IndexOfItem(cboLookup, m_DefaultId)
                If nIndex > 0 Then
                    cboLookup.SelectedIndex = nIndex
                End If
            Else
                If cboLookup.Items.Count > 0 Then
                    cboLookup.SelectedIndex = 0
                End If
            End If
        End If

    End Sub

    Private Function Item(ByVal v_lId As Integer) As BankAccountDetail

        Try
            Return m_colBankAccountDetails(Conversion.Str(v_lId))


        Catch exc As System.Exception

        End Try
    End Function

    Private Function GetBankBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure that we have an object manager

            m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oBankBusiness Is Nothing Then
                ' Get a Bank Business Object
                Dim temp_m_oBankBusiness As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oBankBusiness, "bACTBankAccount.Form", vInstanceManager:="ClientManager")
                m_oBankBusiness = temp_m_oBankBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the Bank business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankBusiness")
                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type table business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBanks(ByRef colBankAccountDetails As Collection) As Integer

        Dim result As Integer = 0
        Dim lId As Integer
        Dim sCode, sDescription As String

        Dim lCurrencyID As Integer
        Dim iCompanyID As Integer
        Dim lAccountId, lBankID As Integer
        Dim sBankAccountNo, sBankAccountName As String

        Dim oBankAccountDetail As BankAccountDetail
        Dim iIsCashReceiveInThisCurrencyOnly As Integer

        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure that we have an object manager & business object

            m_lReturn = CType(GetBankBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Get list of banks


            m_lReturn = m_oBankBusiness.GetDetails(v_iSourceID:=m_iCompamyId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DJM 02/02/2004 : Don't display error if it didn't find any.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                    sMessage = "Unable to retrieve bank details."

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBanks")

                End If

                Return m_lReturn
            End If

            ' Load the collection with the returned fields

            Do While m_oBankBusiness.getnext(vBankAccountID:=lId, vCurrencyID:=lCurrencyID, vCompanyId:=iCompanyID, vAccountId:=lAccountId, vBankID:=lBankID, vCode:=sCode, vBankAccountNo:=sBankAccountNo, vBankAccountName:=sBankAccountName, vDescription:=sDescription, v_iIsCashReceiveInThisCurrencyOnly:=iIsCashReceiveInThisCurrencyOnly) = gPMConstants.PMEReturnCode.PMTrue


                oBankAccountDetail = New BankAccountDetail()

                oBankAccountDetail.AccountId = lAccountId
                oBankAccountDetail.BankAccountNo = sBankAccountNo.Trim()
                oBankAccountDetail.BankAccountName = sBankAccountName.Trim()
                oBankAccountDetail.CurrencyId = lCurrencyID

                oBankAccountDetail.Code = sCode.Trim()
                oBankAccountDetail.Description = sDescription.Trim()
                oBankAccountDetail.Id = lId
                oBankAccountDetail.IsCashReceiveInThisCurrencyOnly = iIsCashReceiveInThisCurrencyOnly

                colBankAccountDetails.Add(oBankAccountDetail, Conversion.Str(lId))

            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bank account details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBanks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class