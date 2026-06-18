Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Public Class frmUserLink
    Inherits System.Windows.Forms.Form

    '
    ' History :
    ' CJB 121205 PN26314 Change cmdOK_Click and cmdEdit_Click to work with systems that have millions
    '                    of parties as opposed to hundreds of thousands!
    '
    Private Const ACClass As String = "frmUserLink"

    Private m_sLinkType As String = ""
    Private m_sCode As String = ""
    Private m_sLastName As String = ""
    Private m_sForeName As String = ""
    Private bSelected As Boolean
    Private m_vParty(,) As Object
    Private m_lClaimHandlerId As Integer
    Private m_lOldClaimHandlerId As Integer
    Private m_lPartyHandlerId As Integer
    Private m_lOldPartyhandlerId As Integer

    '(RC) WR34
    Private m_lOtherPartyId As Integer
    Private m_lOldOtherPartyId As Integer

    Private m_lPartyCnt As Integer
    Private m_lOldPartyCnt As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lCancel As Integer
    Private m_lSelected As Integer
    Private m_sFullname As String = ""
    Private m_sTitle As String = ""
    Private m_sInitials As String = ""
    Private m_lCurrencyID As Integer
    Private m_lDepartmentID As Integer

    Public Property Selected() As Integer
        Get
            Return m_lSelected
        End Get
        Set(ByVal Value As Integer)
            m_lSelected = Value
        End Set
    End Property
    Public Property Cancel() As Integer
        Get
            Return m_lCancel
        End Get
        Set(ByVal Value As Integer)
            m_lCancel = Value
        End Set
    End Property
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property PartyHandlerId() As Integer
        Get
            Return m_lPartyHandlerId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyHandlerId = Value
        End Set
    End Property
    Public Property ClaimHandlerId() As Integer
        Get
            Return m_lClaimHandlerId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimHandlerId = Value
        End Set
    End Property

    '(RC) WR34
    Public Property OtherPartyId() As Integer
        Get
            Return m_lOtherPartyId
        End Get
        Set(ByVal Value As Integer)
            m_lOtherPartyId = Value
        End Set
    End Property

    Public ReadOnly Property Fullname() As String
        Get
            'Fullname = Trim$(m_sForename & " " & m_sLastname)
            Return m_sFullname.Trim()
        End Get
    End Property

    Private Function InitAccExecs() As Integer

        Dim oListitem As ListViewItem

        lvwList.Items.Clear()

        m_lReturn = CType(GetParty("CO"), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For lRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lPartyHandlerId = CInt(m_vParty(0, lRow)) Then

                oListitem = lvwList.Items.Add("H" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("H" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, lRow)).Trim()
        Next lRow

    End Function

    ''' <summary>
    ''' Initialize For for Account Handlets
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InitAccHandlers() As Integer

        Dim oListitem As ListViewItem

        lvwList.Items.Clear()

        m_lReturn = CType(GetParty("AH"), gPMConstants.PMEReturnCode)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For iRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lPartyHandlerId = CInt(m_vParty(0, iRow)) Then

                oListitem = lvwList.Items.Add("H" & CStr(m_vParty(0, iRow)).Trim(), CStr(m_vParty(1, iRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("H" & CStr(m_vParty(0, iRow)).Trim(), CStr(m_vParty(1, iRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, iRow)).Trim()
        Next iRow

    End Function

    Private Function InitExecHandlers() As Integer

        Dim oListitem As ListViewItem

        lvwList.Items.Clear()

        m_lReturn = CType(GetParty("HC"), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For lRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lPartyHandlerId = CInt(m_vParty(0, lRow)) Then

                oListitem = lvwList.Items.Add("H" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("H" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, lRow)).Trim()
        Next lRow

    End Function

    Private Function InitAgents() As Integer

        Dim oListitem As ListViewItem

        'DC141003 -PN7420 -set here rather than later as may exit
        '                   function before chance to set
        cmdEdit.Enabled = False
        cmdNew.Enabled = False

        lvwList.Items.Clear()

        m_lReturn = CType(GetParty("AG"), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For lRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lPartyCnt = CInt(m_vParty(0, lRow)) Then

                oListitem = lvwList.Items.Add("A" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("A" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, lRow)).Trim()
        Next lRow

    End Function

    Private Function InitClaimHandlers() As Integer

        Dim oListitem As ListViewItem

        'DC141003 -PN7420 -set here rather than later as may exit
        '                   function before chance to set
        'cmdEdit.Enabled = False
        'cmdNew.Enabled = False

        cmdEdit.Visible = False
        cmdNew.Visible = False

        lvwList.Items.Clear()

        lvwList.Columns.Item(1).Text = "Description"

        GetClaimHandlers()

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For lRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lClaimHandlerId = CInt(m_vParty(0, lRow)) Then

                oListitem = lvwList.Items.Add("C" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("C" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, lRow)).Trim()
        Next lRow

    End Function

    Public Function Initialise(ByVal sLinkType As String, Optional ByVal sFullName As String = "") As Integer

        m_lOldPartyhandlerId = m_lPartyHandlerId
        m_lOldClaimHandlerId = m_lClaimHandlerId
        m_lOldOtherPartyId = m_lOtherPartyId '(RC) WR34
        m_lOldPartyCnt = m_lPartyCnt

        If sFullName = "" Then
            m_sForeName = ""
            m_sLastName = ""
        Else
        End If

        m_sLinkType = sLinkType
        With lvwList
            .Columns.Item(0).Text = "Code"
            .Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1500))
            .Columns.Item(1).Text = "Name"
            .Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(3550))

        End With

        Select Case m_sLinkType
            Case "AH"
                Text = "Link to Account Handler"
                InitAccHandlers()

            Case "CO"
                Text = "Link to Account Executive"
                InitAccExecs()

            Case "HC"
                Text = "Link to Executive Handler"
                InitExecHandlers()

            Case "CH"
                Text = "Link to Claims Handler"
                InitClaimHandlers()

            Case "IN"
                Text = "Link to Insurer"
                InitInsurers()

            Case "AG"
                Text = "Link to Agent"
                InitAgents()

            Case "OT" '(RC) WR34
                Text = "Link to Other Party"
                InitOtherParty()
        End Select

    End Function

    Private Function InitInsurers() As Integer

        Dim oListitem As ListViewItem

        'DC141003 -PN7420 -set here rather than later as may exit
        '                   function before chance to set
        cmdEdit.Enabled = False
        cmdNew.Enabled = False

        lvwList.Items.Clear()

        m_lReturn = CType(GetParty("IN"), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For lRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lPartyCnt = CInt(m_vParty(0, lRow)) Then

                oListitem = lvwList.Items.Add("I" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("I" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, lRow)).Trim()
        Next lRow

    End Function


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lCancel = 1
        m_lSelected = 0
        m_lPartyHandlerId = m_lOldPartyhandlerId
        m_lClaimHandlerId = m_lOldClaimHandlerId
        m_lOtherPartyId = m_lOldOtherPartyId '(RC) WR34
        m_lPartyCnt = m_lOldPartyCnt

        Me.Hide()

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim oParty As iPMBPartyAH.Interface_Renamed
        Dim sPartyName, sClientCode As String

        Dim sPartyType As String = m_sLinkType
        If sPartyType = "AE" Then
            sPartyType = "CO"
        End If

        Dim oListitem As ListViewItem = Me.lvwList.FocusedItem
        Dim lPartyCnt As Integer = CInt(Mid(oListitem.Name, 2, 8)) 'PN26314


        Dim temp_oParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oParty, sClassName:="iPMBPartyAH.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oParty = temp_oParty

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'ProcessPartyInterface = PMFalse
            If Not (oParty Is Nothing) Then

                oParty.Dispose()
                oParty = Nothing
            End If
            'GoTo Err_ProcessPartyInterface
        End If

        ' set the party cnt and process mode if editing


        oParty.PartyCnt = lPartyCnt

        m_lReturn = oParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oParty.Dispose()
            oParty = Nothing
        End If


        oParty.HandlerType = m_sLinkType

        ' start the object

        m_lReturn = oParty.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

		oParty.Dispose()
            oParty = Nothing
        End If


        If oParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then


            sClientCode = oParty.ShortName

            sPartyName = oParty.ResolvedName

            oListitem.Text = sClientCode
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = sPartyName

        End If

        ' Destroy the object

		oParty.Dispose()
        oParty = Nothing

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        Dim oParty As iPMBPartyAH.Interface_Renamed
        Dim oListitem As ListViewItem
        Dim lPartyCnt As Integer
        Dim sClientCode, sPartyName As String

        Dim sPartyType As String = m_sLinkType
        If sPartyType = "AE" Then
            sPartyType = "CO"
        End If

        Dim temp_oParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oParty, sClassName:="iPMBPartyAH.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oParty = temp_oParty

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If Not (oParty Is Nothing) Then

                oParty.Dispose()
                oParty = Nothing
            End If
        End If


        m_lReturn = oParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oParty.Dispose()
            oParty = Nothing
        End If


        oParty.SourceID = g_iSourceID

        oParty.HandlerType = m_sLinkType

        ' start the object

        m_lReturn = oParty.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'ProcessPartyInterface = PMFalse

            oParty.Dispose()
            oParty = Nothing
            'GoTo Err_ProcessPartyInterface
        End If


        If oParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then


            lPartyCnt = oParty.PartyCnt

            sClientCode = oParty.ShortName

            sPartyName = oParty.ResolvedName

            oListitem = lvwList.Items.Add("H" & lPartyCnt, sClientCode, "")
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = sPartyName

        End If

        ' Destroy the object

		oParty.Dispose()
        oParty = Nothing

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click


        m_lSelected = 0

        For Each iItem As ListViewItem In lvwList.Items



            If iItem.ImageKey = "user" Then
                m_lSelected = 1


                Select Case m_sLinkType
                    Case "AH", "CO", "HC"
                        m_lPartyHandlerId = CLng(Mid(iItem.Name, 2, 8))
                    Case "AG", "IN"
                        m_lPartyCnt = CLng(Mid(iItem.Name, 2, 8))
                    Case "CH"
                        m_lClaimHandlerId = CLng(Mid(iItem.Name, 2, 8))
                    Case "OT"
                        m_lOtherPartyId = CLng(Mid(iItem.Name, 2, 8))

                End Select

                m_sFullname = ListViewHelper.GetListViewSubItem(iItem, 1).Text.Trim()

            End If

        Next iItem

        If m_lSelected = 0 Then


            Select Case m_sLinkType
                Case "AH", "CO", "HC"
                    m_lPartyHandlerId = 0
                Case "AG", "IN"
                    m_lPartyCnt = 0
                Case "CH"
                    m_lClaimHandlerId = 0
                Case "OT"
                    m_lOtherPartyId = 0 '(RC) WR34

            End Select

            m_sFullname = ""

        End If

        Me.Hide()

    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click

        If lvwList.FocusedItem Is Nothing Then
            Exit Sub
        End If

        If cmdSelect.Text = "Deselect" Then



            lvwList.FocusedItem.ImageKey = Nothing
            lvwList.FocusedItem.Selected = False
            cmdSelect.Text = "Select"

        Else

            For Each iItem As ListViewItem In lvwList.Items
                If iItem.Text = lvwList.FocusedItem.Text Then


                    iItem.ImageKey = "user"
                Else


                    iItem.ImageKey = Nothing
                End If

            Next iItem
            cmdSelect.Text = "Deselect"

        End If

        lvwList.Visible = False
        lvwList.Visible = True

    End Sub



    Private Sub frmUserLink_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lCancel = 0
        m_lSelected = 0
        imgIcons.Images.Add("user", imgGroup_old.Images.Item("user"))

    End Sub


    ' ***************************************************************** '
    '
    ' Name: Get Claim Handlers
    '
    ' Description:
    '
    ' History: 18/09/03 -DC Created
    '
    ' ***************************************************************** '
    Private Function GetClaimHandlers() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetClaimHandlers(r_vParty:=m_vParty)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimHandlers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimHandlers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Get Party
    '
    ' Description:
    '
    ' History: 18/09/03 -DC Created
    '
    ' ***************************************************************** '
    Private Function GetParty(ByRef sPartyType As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetParty(r_sPartyType:=sPartyType, r_vParty:=m_vParty)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimHandlers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetOtherParty
    '
    ' Description:
    '
    ' History: 02 Nov 2006 - RC Created
    '
    ' ***************************************************************** '
    Private Function GetOtherParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetOtherParty(r_vOtherParty:=m_vParty)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimHandlers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: InitOtherParty
    '
    ' Description:
    '
    ' History: 02 Nov 2006 - (RC) Created
    '
    ' ***************************************************************** '
    Private Function InitOtherParty() As Integer

        Dim oListitem As ListViewItem

        'DC141003 -PN7420 -set here rather than later as may exit
        '                   function before chance to set
        cmdEdit.Enabled = False
        cmdNew.Enabled = False

        lvwList.Items.Clear()

        m_lReturn = CType(GetOtherParty(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not Information.IsArray(m_vParty) Then
            Exit Function
        End If

        For lRow As Integer = m_vParty.GetLowerBound(1) To m_vParty.GetUpperBound(1)
            If m_lOtherPartyId = CInt(m_vParty(0, lRow)) Then

                oListitem = lvwList.Items.Add("A" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "user")
            Else
                oListitem = lvwList.Items.Add("A" & CStr(m_vParty(0, lRow)).Trim(), CStr(m_vParty(1, lRow)).Trim(), "")
            End If
            ListViewHelper.GetListViewSubItem(oListitem, 1).Text = CStr(m_vParty(2, lRow)).Trim()
        Next lRow

    End Function


    Private Sub lvwList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwList.SelectedIndexChanged
        Dim sTemp As String = ""

        If lvwList.SelectedItems.Count > 0 Then
            If lvwList.SelectedItems.Item(0) Is Nothing Then
                cmdEdit.Enabled = False
                m_sCode = ""
            Else
                m_sCode = lvwList.SelectedItems.Item(0).Text

                Select Case m_sLinkType
                    Case "CO", "AH", "HC"
                        cmdEdit.Enabled = True
                        sTemp = ListViewHelper.GetListViewSubItem(lvwList.SelectedItems.Item(0), 1).Text.Substring(0, ListViewHelper.GetListViewSubItem(lvwList.SelectedItems.Item(0), 1).Text.IndexOf(" "c))

                    Case "CH", "IN", "AG"
                        m_sLastName = ListViewHelper.GetListViewSubItem(lvwList.SelectedItems.Item(0), 1).Text
                End Select


                If lvwList.SelectedItems.Item(0).ImageKey = "user" Then
                    cmdSelect.Text = "Deselect"
                Else
                    cmdSelect.Text = "Select"
                End If
            End If
        End If
    End Sub
End Class
