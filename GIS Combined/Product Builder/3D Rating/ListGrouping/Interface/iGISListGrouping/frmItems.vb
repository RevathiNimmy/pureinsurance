Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmItems
	Inherits System.Windows.Forms.Form
	Private Sub frmItems_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private Const ACClass As String = "frmItems"
    Private Const vbFormCode As Integer = 0

	Private m_lReturn As Integer
	
	Private m_vDataArray( ,  ) As Object
	
	' GISListTypeID
	Private m_lGISListTypeID As Integer
	
	' GISListGroupingID
	Private m_lGISListGroupingID As Integer
	
	' Task
	Private m_iTask As gPMConstants.PMEComponentAction
	
	' Error
	Private m_lError As Integer
	
	' Code
	Private m_sCode As String = ""
	
	' Description
	Private m_sDescription As String = ""
	
	' NewItemCount
	Private m_lNewItemCount As Integer
	
	' Status
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property NewItemCount() As Integer
		Get
			Return m_lNewItemCount
		End Get
		Set(ByVal Value As Integer)
			m_lNewItemCount = Value
		End Set
	End Property
	
	Public Property Description() As String
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value
		End Set
	End Property
	
	Public Property Code() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
		End Set
	End Property
	
	Public Property Error_Renamed() As Integer
		Get
			Return m_lError
		End Get
		Set(ByVal Value As Integer)
			m_lError = Value
		End Set
	End Property
	
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public Property GISListGroupingID() As Integer
		Get
			Return m_lGISListGroupingID
		End Get
		Set(ByVal Value As Integer)
			m_lGISListGroupingID = Value
		End Set
	End Property
	
	Public Property GISListTypeID() As Integer
		Get
			Return m_lGISListTypeID
		End Get
		Set(ByVal Value As Integer)
			m_lGISListTypeID = Value
		End Set
	End Property
	
	' ***************************************************************** '
	'
	' Name: AddItem
	'
	' Description: Adds an item
	'
	' History: 20/11/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function AddItem() As Integer
		
		Dim result As Integer = 0
		Dim lIndex As Integer
		Dim bRefresh As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Dont need to refresh yet
			bRefresh = False
			
			For	Each lstItem As ListViewItem In lvwSource.Items
				
				If TypeOf lstItem Is ListViewItem Then
					
					If lstItem.Selected Then
						' Get the index

                        lIndex = Convert.ToString(lstItem.Tag)
                        ' Set the selected flag
                        m_vDataArray(ACListArraySelected, lIndex) = 1
                        ' Flag we need to refresh
                        bRefresh = True
                    End If

                End If

            Next lstItem

            ' Refresh the list?
            If bRefresh Then
                m_lReturn = RefreshList()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RemoveItem
    '
    ' Description:
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RemoveItem() As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer
        Dim bRefresh As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Dont need to refresh yet
            bRefresh = False

            For Each lstItem As ListViewItem In lvwList.Items

                If TypeOf lstItem Is ListViewItem Then

                    If lstItem.Selected Then
                        ' Get the index

                        lIndex = Convert.ToString(lstItem.Tag)
                        ' Set the selected flag
                        m_vDataArray(ACListArraySelected, lIndex) = 0
                        ' Flag we need to refresh
                        bRefresh = True
                    End If

                End If

            Next lstItem

            ' Refresh the list?
            If bRefresh Then
                m_lReturn = RefreshList()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = AddItem()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddAllItems
    '
    ' Description:
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ChangeAllItems(ByVal v_iStatus As Integer, ByVal v_oSource As ListView.ListViewItemCollection) As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For Each lstItem As ListViewItem In v_oSource

                If TypeOf lstItem Is ListViewItem Then


                    lIndex = Convert.ToString(lstItem.Tag)
                    ' Set the selected flag
                    m_vDataArray(ACListArraySelected, lIndex) = v_iStatus

                End If

            Next lstItem

            ' Refresh the lists
            m_lReturn = RefreshList()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeAllItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeAllItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub cmdAddAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAll.Click

        m_lReturn = ChangeAllItems(v_iStatus:=1, v_oSource:=lvwSource.Items)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ProcessClose
    '
    ' Description:
    '
    ' History: 21/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessClose() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Close the interface
            If MessageBox.Show("Are you sure you wish to cancel? You will lose any changes", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                Me.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessClose Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClose", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        m_lReturn = ProcessClose()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: UpdateItems
    '
    ' Description:
    '
    ' History: 21/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateItems() As Integer

        Dim result As Integer = 0
        Dim sNewCode, sNewDescription As String
        Dim bCheckUsed As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 111201
            ' Have we changed the code?
            bCheckUsed = (m_sCode <> txtCode.Text)

            ' Get the values off the screen
            m_sCode = txtCode.Text
            m_sDescription = txtDescription.Text
            m_lNewItemCount = lvwList.Items.Count

            ' Check if theres a code
            If m_sCode = "" Then
                MessageBox.Show("You need to provide a code.", "Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtCode.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if theres a description
            If m_sDescription = "" Then
                MessageBox.Show("You need to provide a description.", "Description", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtDescription.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check we have some items
            If m_lNewItemCount = 0 Then

                If MessageBox.Show("You have no items in this list." & Environment.NewLine & "Do you wish to continue?", "No Items", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Sort out the apostrophes
            ' Copy them so we dont display the one with '' in
            sNewCode = m_sCode.Replace("'", "''")
            sNewDescription = m_sDescription.Replace("'", "''")

            ' Update the items

            m_lReturn = g_oBusiness.UpdateGroupItems(v_sCode:=sNewCode, v_sDescription:=sNewDescription, v_lGISListGroupingID:=m_lGISListGroupingID, v_vDataArray:=m_vDataArray, v_bCheckUsed:=bCheckUsed)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' It's already in use
                If m_lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                    MessageBox.Show("The code '" & m_sCode & "' is already in use." & Environment.NewLine & _
                                    "You must chose a unique code.", "Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddItems
    '
    ' Description: Adds a new group
    '
    ' History: 27/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function AddItems() As Integer

        Dim result As Integer = 0
        Dim vDataArray() As Object
        Dim lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the values off the screen
            m_sCode = txtCode.Text
            m_sDescription = txtDescription.Text
            m_lNewItemCount = lvwList.Items.Count

            ' Check if theres a code
            If m_sCode = "" Then
                MessageBox.Show("You need to provide a code.", "Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtCode.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if theres a description
            If m_sDescription = "" Then
                MessageBox.Show("You need to provide a description.", "Description", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtDescription.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check we have some items
            If m_lNewItemCount = 0 Then

                If MessageBox.Show("You have no items in this list." & Environment.NewLine & "Do you wish to continue?", "No Items", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Get the items
            If m_lNewItemCount > 0 Then

                ReDim vDataArray(m_lNewItemCount - 1)

                For lLoop1 As Integer = 1 To lvwList.Items.Count

                    lIndex = Convert.ToString(lvwList.Items.Item(lLoop1 - 1).Tag)

                    ' Store the value

                    vDataArray(lLoop1 - 1) = m_vDataArray(ACListArrayID, lIndex)

                Next lLoop1

            End If

            ' Call the business

            m_lReturn = g_oBusiness.AddItems(v_sCode:=m_sCode, v_sDescription:=m_sDescription, v_lGISListTypeID:=m_lGISListTypeID, v_vDataArray:=vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Set the mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Update the items
        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            m_lReturn = UpdateItems()
        Else
            m_lReturn = AddItems()
        End If

        ' Set the mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Hide the form if it worked
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Me.Hide()
        End If

    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        m_lReturn = RemoveItem()

    End Sub

    Private Sub cmdRemoveAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveAll.Click

        m_lReturn = ChangeAllItems(v_iStatus:=0, v_oSource:=lvwList.Items)

    End Sub

    Private Sub Form_Initialize_Renamed()

        m_sDescription = ""
        m_sCode = ""

    End Sub


    Private Sub frmItems_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            Error_Renamed = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Load the details
            m_lReturn = GetBusiness()

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Error_Renamed = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception



            Error_Renamed = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: BusinessToData
    '
    ' Description:
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the business object

            m_lReturn = g_oBusiness.GetListItems(v_lGISListTypeID:=m_lGISListTypeID, v_lGISListGroupingID:=m_lGISListGroupingID, r_vResultArray:=m_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get list items", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RefreshList
    '
    ' Description:
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RefreshList() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim iSelected As Integer
        Dim sKey, sText As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the lists
            lvwList.Items.Clear()
            lvwSource.Items.Clear()

            If Not Information.IsArray(m_vDataArray) Then
                ' No need to do anything
                Return result
            End If

            For lLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                iSelected = CInt(m_vDataArray(ACListArraySelected, lLoop1))
                sText = CStr(m_vDataArray(ACListArrayCode, lLoop1)).Trim()
                sKey = "K" & lLoop1

                ' Decide which list to place it in
                If iSelected = 1 Then

                    lstItem = lvwList.Items.Add(sKey, sText, ACNormalIcon)

                Else

                    lstItem = lvwSource.Items.Add(sKey, sText, ACNormalIcon)

                End If

                ' set the tag
                lstItem.Tag = CStr(lLoop1)

                ' Description
                ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vDataArray(ACListArrayDescription, lLoop1))

            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DataToInterface
    '
    ' Description:
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the text box values
            txtCode.Text = m_sCode
            txtDescription.Text = m_sDescription

            ' Refresh the lists
            m_lReturn = RefreshList()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBusiness
    '
    ' Description:
    '
    ' History: 15/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the data from the business object
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = BusinessToData()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Show the data
            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub frmItems_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Close the interface


        If UnloadMode <> vbFormCode Then
            If MessageBox.Show("Are you sure you wish to cancel? You will lose any changes", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Cancel = 1
            End If
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmItems_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        m_lReturn = ResizeForm()

    End Sub

    Private Sub lvwList_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwList.DoubleClick

        m_lReturn = RemoveItem()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' hmmm
        End If

    End Sub

    Private Sub lvwSource_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSource.DoubleClick

        m_lReturn = AddItem()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' hmmm
        End If

    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter

        ' Highlight the text
        txtCode.SelectionStart = 0
        txtCode.SelectionLength = Strings.Len(txtCode.Text)

    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        ' Highlight the text
        txtDescription.SelectionStart = 0
        txtDescription.SelectionLength = Strings.Len(txtDescription.Text)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ResizeForm
    '
    ' Description:
    '
    ' History: 28/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ResizeForm() As Integer

        Dim result As Integer = 0
        Dim lGap As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check min sizes
            If VB6.PixelsToTwipsX(Me.Width) < 500 Then
                Me.Width = VB6.TwipsToPixelsX(500)
            End If

            If VB6.PixelsToTwipsY(Me.Height) < 500 Then
                Me.Height = VB6.TwipsToPixelsY(500)
            End If

            lGap = CInt(VB6.PixelsToTwipsX(cmdRemove.Width) + 470)

            tabMain.Width = Me.Width - VB6.TwipsToPixelsX(315)
            tabMain.Height = Me.Height - VB6.TwipsToPixelsY(1035)

            fraDetails.Width = tabMain.Width - VB6.TwipsToPixelsX(360)

            ' buttons
            cmdRemove.Top = Me.Height - VB6.TwipsToPixelsY(2130)
            cmdRemoveAll.Top = Me.Height - VB6.TwipsToPixelsY(1650)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(200) - cmdHelp.Width
            cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(100) - cmdCancel.Width
            cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(100) - cmdOK.Width
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(500) - cmdHelp.Height
            cmdCancel.Top = cmdHelp.Top
            cmdOK.Top = cmdHelp.Top

            ' list views
            lvwSource.Height = Me.Height - VB6.TwipsToPixelsY(3435)
            lvwList.Height = lvwSource.Height
            lvwSource.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(tabMain.Width) - lGap) / 2)
            lvwList.Width = lvwSource.Width
            lvwList.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSource.Width) + lGap - 120)
            lblSelected.Left = lvwList.Left

            ' Buttons
            cmdAdd.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSource.Left) + VB6.PixelsToTwipsX(lvwSource.Width) + 100)
            cmdAddAll.Left = cmdAdd.Left
            cmdRemove.Left = cmdAdd.Left
            cmdRemoveAll.Left = cmdAdd.Left

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResizeForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResizeForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
End Class
