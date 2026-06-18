Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 19/01/2005
	'
	' Description: Main interface.
	'
	' Edit History:
	' RKS   19/01/2005  Created
	' RKS   21/01/2005  Created
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "frmInterface"
	
	Private m_vClientData( ,  ) As Object
	Private m_sPartyTypeCode As String = ""
	Private m_sOriginalClientCode As String = ""
	Private m_sUniqueClientCode As String = ""
	
	Private m_sSelectedClientCode As String = ""
	Private m_lSelectedClientPartyCnt As Integer
	Private m_iOKAction As Integer
	Private m_lStatus As Integer
	
	Private m_lReturn As Integer

	Private m_oUser As bPMUser.Business
	Private m_vUserSourceDetails( ,  ) As Object
	Private m_lErrorNumber As Integer

    'NIIT - Replaced with the Migrated code 1144 
    'Public WriteOnly Property ClientData() As Object()
    '	Set(ByVal Value() As Object)
    '		m_vClientData = Value
    '	End Set
    '   End Property
    Public WriteOnly Property ClientData() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vClientData = Value
        End Set
    End Property
	
	Public WriteOnly Property PartyTypeCode() As String
		Set(ByVal Value As String)
			m_sPartyTypeCode = Value
		End Set
	End Property
	
	Public WriteOnly Property OriginalClientCode() As String
		Set(ByVal Value As String)
			m_sOriginalClientCode = Value
		End Set
	End Property
	
	Public WriteOnly Property UniqueClientCode() As String
		Set(ByVal Value As String)
			m_sUniqueClientCode = Value
		End Set
	End Property
	
	Public ReadOnly Property SelectedClientCode() As String
		Get
			Return m_sSelectedClientCode
		End Get
	End Property
	
	Public ReadOnly Property SelectedClientPartyCnt() As Integer
		Get
			Return m_lSelectedClientPartyCnt
		End Get
	End Property
	
	Public ReadOnly Property OKAction() As Integer
		Get
			Return m_iOKAction
		End Get
	End Property
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Function SetInterfaceDefaults() As Integer
		Dim result As Integer = 0
		Dim iRows As Integer
		
		Try 
			' Set the controls properties
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lblPotentialDuplicate.Text = "Potential duplicate client found for '" &  _
			                             m_sOriginalClientCode & "':"
			
			iRows = m_vClientData.GetUpperBound(1)
			
			lvwClients.Items.Clear()
			
            'For iRow As Integer = 0 To iRows
            '	With lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim())
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientName).Text = CStr(m_vClientData(ACClientName, iRow))
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientAddress1).Text = CStr(m_vClientData(ACClientAddress1, iRow))
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientAddress2).Text = CStr(m_vClientData(ACClientAddress2, iRow))
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientPostcode).Text = CStr(m_vClientData(ACClientPostcode, iRow))
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientPartyType).Text = CStr(m_vClientData(ACClientPartyType, iRow))
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientBranch).Text = CStr(m_vClientData(ACClientBranch, iRow)).Trim()
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientPartyCnt).Text = CStr(m_vClientData(ACClientPartyCnt, iRow))
            '		ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim()), ACClientPartyTypeCode).Text = CStr(m_vClientData(ACClientPartyTypeCode, iRow)).Trim()
            '	End With
            'Next iRow
            Dim oListItem As ListViewItem
            For iRow As Integer = 0 To iRows
                oListItem = lvwClients.Items.Add(CStr(m_vClientData(ACClientCode, iRow)).Trim())
                ListViewHelper.GetListViewSubItem(oListItem, ACClientName).Text = CStr(m_vClientData(ACClientName, iRow))
                ListViewHelper.GetListViewSubItem(oListItem, ACClientAddress1).Text = CStr(m_vClientData(ACClientAddress1, iRow))
                ListViewHelper.GetListViewSubItem(oListItem, ACClientAddress2).Text = CStr(m_vClientData(ACClientAddress2, iRow))
                ListViewHelper.GetListViewSubItem(oListItem, ACClientPostcode).Text = CStr(m_vClientData(ACClientPostcode, iRow))
                ListViewHelper.GetListViewSubItem(oListItem, ACClientPartyType).Text = CStr(m_vClientData(ACClientPartyType, iRow))
                ListViewHelper.GetListViewSubItem(oListItem, ACClientBranch).Text = CStr(m_vClientData(ACClientBranch, iRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, ACClientPartyCnt).Text = CStr(m_vClientData(ACClientPartyCnt, iRow))
                ListViewHelper.GetListViewSubItem(oListItem, ACClientPartyTypeCode).Text = CStr(m_vClientData(ACClientPartyTypeCode, iRow)).Trim()
            Next iRow
			
			If lvwClients.Items.Count > 0 Then
				lvwClients.Items.Item(0).Selected = True
			End If
			optAction(1).Text = "Create a new unique code. The next available code is " & m_sUniqueClientCode & "."
			optAction(0).Checked = True
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		' Set the status to Cancelled
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		m_lSelectedClientPartyCnt = 0
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim bFoundSelectedItem As Boolean
		
		' Set the status to OK
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		If optAction(kAbandonNewRecordandUseSelectedClient).Checked Then
			
			' confirm a client has been selected
			For lItem As Integer = 1 To lvwClients.Items.Count
				If lvwClients.Items.Item(lItem - 1).Selected Then
					bFoundSelectedItem = True
					Exit For
				End If
			Next 
			
			If bFoundSelectedItem Then
				m_iOKAction = kAbandonNewRecordandUseSelectedClient
                m_sSelectedClientCode = lvwClients.FocusedItem.Text
                'Changes done as per VB code
                'm_lSelectedClientPartyCnt = CInt(lvwClients.FocusedItem.SubItems.Item(ACClientPartyCnt - 1).Text)
                m_lSelectedClientPartyCnt = CInt(lvwClients.FocusedItem.SubItems.Item(ACClientPartyCnt).Text)
				
				Me.Hide()
			Else
				MessageBox.Show("You must select a client", "Duplicate Client", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If
		End If
		
		If optAction(kCreateUniqueCode).Checked Then
			m_iOKAction = kCreateUniqueCode
			Me.Hide()
		End If
		
		
	End Sub
	
	Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click



        Dim oPMBCMManager As iPMBCMManager.Interface_Renamed
       
		If lvwClients.FocusedItem Is Nothing Then
			Exit Sub
		End If
		
		Dim temp_oPMBCMManager As Object
		m_lReturn = g_oObjectManager.GetInstance(temp_oPMBCMManager, sClassName:="iPMBCMManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
		oPMBCMManager = temp_oPMBCMManager
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		

		m_lReturn = CType(oPMBCMManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
        'Changes done as per VB6 code
        'Starts

        'oPMBCMManager.PartyCnt = lvwClients.FocusedItem.SubItems.Item(ACClientPartyCnt - 1).Text

        'oPMBCMManager.PartyResolvedName = lvwClients.FocusedItem.SubItems.Item(ACClientName - 1).Text

        'oPMBCMManager.PartyShortName = lvwClients.FocusedItem.Text

        'oPMBCMManager.PartyType = lvwClients.FocusedItem.SubItems.Item(ACClientPartyTypeCode - 1).Text.Substring(0, 1)

        oPMBCMManager.PartyCnt = lvwClients.FocusedItem.SubItems.Item(ACClientPartyCnt).Text

        oPMBCMManager.PartyResolvedName = lvwClients.FocusedItem.SubItems.Item(ACClientName).Text

        oPMBCMManager.PartyShortName = lvwClients.FocusedItem.Text

        oPMBCMManager.PartyType = lvwClients.FocusedItem.SubItems.Item(ACClientPartyTypeCode).Text.Substring(0, 1)
        'ends

		oPMBCMManager.Start()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		

		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oUser As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oUser = temp_m_oUser
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		' Set the status to Cancelled
        ' Changes as per done VB code
        'm_lStatus = gPMConstants.PMEReturnCode.PMCancel
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		ResizeControls()
	End Sub
	
	Private Sub lvwClients_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwClients.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwClients.Columns(eventArgs.Column)
		' Column click event for the search details
		
		Try 
			
			
			With lvwClients
				' If current sort column header is
				' pressed.
				If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwClients) Then
					' Set sort order opposite of
					' current direction.
					ListViewHelper.SetSortOrderProperty(lvwClients, (ListViewHelper.GetSortOrderProperty(lvwClients) + 1) Mod 2)
				Else
					' Sort by this column (ascending).
					ListViewHelper.SetSortedProperty(lvwClients, False)
					
					' Turn off sorting so that the list
					' is not sorted twice
					ListViewHelper.SetSortOrderProperty(lvwClients, SortOrder.Ascending)
					ListViewHelper.SetSortKeyProperty(lvwClients, ColumnHeader.Index + 1 - 1)
					ListViewHelper.SetSortedProperty(lvwClients, True)
				End If
			End With
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	Private Sub lvwClients_ItemClick(ByVal Item As ListViewItem)
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		If optAction(kAbandonNewRecordandUseSelectedClient).Checked Then
			lReturn = UserHasBranchAccess()
			If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				EnableOK()
			Else
				MessageBox.Show("You do not have access to this branch or any parties associated with it." & Strings.Chr(13) & Strings.Chr(10) & "If this is incorrect contact your system administrator.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				cmdOK.Enabled = False
			End If
		Else
			EnableOK()
		End If
		
	End Sub
	
	Private Sub optAction_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optAction_1.CheckedChanged, _optAction_0.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			Dim Index As Integer = Array.IndexOf(optAction, eventSender)
			
			If Index = 0 Then
				For lItem As Integer = 1 To lvwClients.Items.Count
					lvwClients.Items.Item(lItem - 1).Selected = False
				Next 
			End If
			
			EnableOK()
			
		End If
	End Sub
	
	Private Sub EnableOK()

        'TODO_MIlan: Need to discuss
        If lvwClients.SelectedItems.Count > 0 Then
            'Changes done as per VB Code
            'Dim sType As String = lvwClients.FocusedItem.SubItems.Item(ACClientPartyTypeCode - 1).Text
            Dim sType As String = lvwClients.SelectedItems(0).SubItems(ACClientPartyTypeCode).Text

            cmdOK.Enabled = False

            If optAction(kCreateUniqueCode).Checked Then
                cmdOK.Enabled = True
            End If

            'If optAction(kAbandonNewRecordandUseSelectedClient).Checked And lvwClients.FocusedItem.SubItems.Item(ACClientPartyTypeCode - 1).Text = m_sPartyTypeCode Then
            If optAction(kAbandonNewRecordandUseSelectedClient).Checked And lvwClients.SelectedItems(0).SubItems(ACClientPartyTypeCode).Text = m_sPartyTypeCode Then
                cmdOK.Enabled = True
            End If
            cmdView.Enabled = (sType = "PC" Or sType = "CC" Or sType = "GC")
        End If
    End Sub
	
	Private Sub ResizeControls()
		
		If Me.WindowState = FormWindowState.Minimized Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsY(Me.Height) < 3500 Then
			Me.Height = VB6.TwipsToPixelsY(3500)
		End If
		
		If VB6.PixelsToTwipsX(Me.Width) < 8000 Then
			Me.Width = VB6.TwipsToPixelsX(8000)
		End If
		
		
		cmdView.Top = Me.ClientRectangle.Height - (cmdView.Height + VB6.TwipsToPixelsY(150))
		
		cmdOK.Top = Me.ClientRectangle.Height - (cmdOK.Height + VB6.TwipsToPixelsY(150))
		cmdCancel.Top = Me.ClientRectangle.Height - (cmdCancel.Height + VB6.TwipsToPixelsY(150))
		
		
		cmdCancel.Left = Me.ClientRectangle.Width - (cmdCancel.Width + VB6.TwipsToPixelsX(150))
		cmdOK.Left = cmdCancel.Left - (cmdOK.Width + VB6.TwipsToPixelsX(150))
		
		optAction(kCreateUniqueCode).Top = cmdView.Top - VB6.TwipsToPixelsY(CInt(optAction(kCreateUniqueCode).Checked) + 300)
		optAction(kAbandonNewRecordandUseSelectedClient).Top = optAction(kCreateUniqueCode).Top - VB6.TwipsToPixelsY(CInt(optAction(kAbandonNewRecordandUseSelectedClient).Checked) + 300)
		
		optAction(kCreateUniqueCode).Width = Me.Width - (lvwClients.Left + VB6.TwipsToPixelsX(150))
		optAction(kAbandonNewRecordandUseSelectedClient).Width = Me.Width - (lvwClients.Left + VB6.TwipsToPixelsX(150))
		
		lvwClients.Height = optAction(kAbandonNewRecordandUseSelectedClient).Top - (lvwClients.Top + VB6.TwipsToPixelsY(150))
		Dim lWidth As Integer = CInt(VB6.PixelsToTwipsX(lvwClients.Width))
		lvwClients.Width = Me.ClientRectangle.Width - (lvwClients.Left + VB6.TwipsToPixelsX(150))
		
		'Resize listview columns (proportionate)
		For iCol As Integer = 1 To lvwClients.Columns.Count
			lvwClients.Columns.Item(iCol - 1).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwClients.Columns.Item(iCol - 1).Width) / lWidth * VB6.PixelsToTwipsX(lvwClients.Width)))
		Next iCol
		
	End Sub
	
	' ***************************************************************** '
	' Name: UserHasBranchAccess
	'
	' Parameters: n/a
	'
	' Description: Confirms whether or not the user has access to the
	'               selected parties branch... and therefore whether or
	'                not they will be able to access that party.
	'
	' History:
	'           Created : MEvans : 18-04-2005 : PN20248
	' ***************************************************************** '
	Private Function UserHasBranchAccess() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "UserHasBranchAccess"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Dim sSelectedBranch As String = ""
		Dim llBound, lUBound As Integer
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If Not Information.IsArray(m_vUserSourceDetails) Then
			' get the currenct users source access info

			lReturn = m_oUser.GetUserSourceInfo(r_lUserId:=g_oObjectManager.UserID, r_vUserSourceInfo:=m_vUserSourceDetails)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetUserSourceInfo Failed for UserId:" & g_oObjectManager.UserID)
			End If
		End If
		
		If Information.IsArray(m_vUserSourceDetails) Then
			
			'source  code       description  access (0 = access, 1 = no access)
			'1       HeadOff     Head Office  1
			'2       ABACO       Abaco        0
			'4       FREEPORT    Freeport     0
			'5       MINE        MINE         0
			'6       EUROB       EUROB        0
			
            ' get the selected duplicates branch
            ' Changes done as per VB 6 code
            'sSelectedBranch = lvwClients.FocusedItem.SubItems.Item(ACClientBranch - 1).Text
            sSelectedBranch = lvwClients.FocusedItem.SubItems.Item(ACClientBranch).Text
			
			llBound = m_vUserSourceDetails.GetLowerBound(1)
			lUBound = m_vUserSourceDetails.GetUpperBound(1)
			
			' for each branch
			For lBranch As Integer = llBound To lUBound
				
				' if the branch matches the selected parties branch
				If sSelectedBranch = CStr(m_vUserSourceDetails(2, lBranch)) Then
					
					' if the user doesnt have access to this branch
					If CStr(m_vUserSourceDetails(3, lBranch)) = "1" Then
						' set the return value to indicate the user doesnt have access
						' to the selected branch
						result = gPMConstants.PMEReturnCode.PMFalse
						Exit For
					End If
				End If
				
			Next 
			
		End If
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
End Class
