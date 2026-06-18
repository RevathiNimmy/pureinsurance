Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmUsers
    Inherits System.Windows.Forms.Form
    Private Const ACClass As String = "frmUsers"

    Private m_lReturn As Integer
    Private m_lXPos As Integer
    Private m_lYPos As Integer


    Private m_oBusiness As bPMUser.Business
    ' PartyCnt
    Private m_lPartyCnt As Integer
    'Agent Name
    Private m_sAgentName As String = ""
    'Interface Status
    Private m_lStatus As Integer

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public WriteOnly Property AgentName() As String
        Set(ByVal Value As String)
            m_sAgentName = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    '*************************************************************
    '
    ' Function Name:ShowForm()
    '
    ' Description: Shows form details which correspond with what
    '              the user has selected from the previous form
    '*************************************************************

    Public Function ShowForm() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Text = "Edit Members of " & m_sAgentName.Trim()

            'Show the form
            Me.ShowDialog()

            Return result

        Catch excep As System.Exception



            'Error Section

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Group Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function InitialForm() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function TerminateForm() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have an instance of the business object.
            If Not (m_oBusiness Is Nothing) Then
                ' Terminate the business object

		m_oBusiness.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing
            End If

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the business", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************************
    '
    ' Function Name: RefreshList
    '
    ' Description: Refreshes the listview boxes with data held in
    '              the business, not the database.
    '****************************************************************************

    Private Function RefreshList() As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim oListItem As ListViewItem
        Dim lUserId As Integer
        Dim sUserName As String = ""
        Dim lPartyCnt As Integer
        Dim iIsDeleted As gPMConstants.PMEReturnCode
        Dim dtEffectiveDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oBusiness.CurrentRecord = 0

            'Clear the items in the listview boxes
            lvwAll.Items.Clear()
            lvwContents.Items.Clear()

            'Set row count
            lRow = -1

            Do
                'Get id of first record

                m_lReturn = m_oBusiness.GetNext(vUserId:=lUserId, vUsername:=sUserName, vPartyCnt:=lPartyCnt, vIsDeleted:=iIsDeleted, vEffectiveDate:=dtEffectiveDate)

                'Exit if you get an invalid response
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Do
                End If

                'increment the count by 1
                lRow += 1

                If lPartyCnt = 0 Then
                    'Populate the listview box
                    'Records whcih are not allocated to any agent

                    oListItem = lvwAll.Items.Add("L" & lRow, sUserName.Trim(), "")
                    With oListItem
                        If (iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (dtEffectiveDate > DateTime.Now) Then

                            'developer guide no solution no. 12
                            '.Ghosted = True

                        End If
                    End With

                    oListItem = Nothing
                End If

                If lPartyCnt = m_lPartyCnt Then
                    'Records which are allocated to this agent

                    oListItem = lvwContents.Items.Add("L" & lRow, sUserName.Trim(), "")
                    With oListItem

                        If (iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (dtEffectiveDate > DateTime.Now) Then

                            'developer guide no solution no. 12
                            '.Ghosted = True
                        End If
                    End With
                    oListItem = Nothing
                End If
            Loop

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh group details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************************
    '
    ' Function Name: PopListBox
    '
    ' Description: Populates listview boxes
    '
    '****************************************************************************

    Public Function PopListBox() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Getdetails from business

            m_lReturn = m_oBusiness.GetDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Refresh the List

            Return RefreshList()

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load group details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    '
    ' Function Name: WriteToDb()
    '
    ' Description: Does the checks and updates the db
    '
    '***********************************************************************

    Private Function WriteToDb() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Changes to Agent Membership Details Could not be written to Database", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

		m_oBusiness.Dispose()
                m_oBusiness = Nothing
                m_lReturn = InitialForm()
                m_lReturn = PopListBox()
                Return result
            End If


            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write details to database", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteToDb", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormCustomResize
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Sub FormCustomResize()

        Try

            'We've resized the form, so we need to make sure that the buttons and list views
            'stay where they are relative to the form.

            cmdAddMembers.Left = (tabMembers.Width - cmdAddMembers.Width) / 2
            cmdAddAllMembers.Left = (tabMembers.Width - cmdAddAllMembers.Width) / 2
            cmdDeleteMembers.Left = (tabMembers.Width - cmdDeleteMembers.Width) / 2
            cmdDeleteAllMembers.Left = (tabMembers.Width - cmdDeleteAllMembers.Width) / 2

            lvwAll.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(tabMembers.Width) - 1785) / 2)

            lvwContents.Width = lvwAll.Width
            lvwContents.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMembers.Width) - VB6.PixelsToTwipsX(lvwContents.Width) - 240)

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormCustomResizeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormCustomResize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try



    End Sub
    ' ***************************************************************** '
    ' Name: NewUser
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function NewUser() As Integer

        Dim result As Integer = 0
        Dim oObject As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oObject = CreateLateBoundObject("iPMUserMaintenance.Interface_Renamed")

            m_lReturn = CType(oObject, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If

            'DC041103 -PN8111 -different way to add new users as user maintenance has changed
            '    m_lReturn = oObject.NewUser
            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If

		oObject.Dispose()

            oObject = Nothing

            m_lReturn = PopListBox()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddAllMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddAllMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAll.Items.Count
                ''developer guide no.  todolist
                'm_lReturn = AddMember(lvwAll.Items.Item(iTemp - 1).Text)
                m_lReturn = AddMember(New ListViewItem(lvwAll.Items.Item(iTemp - 1).Text))
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAllMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAll.Items.Count
                If lvwAll.Items.Item(iTemp - 1).Selected Then
                    'TODO:
                    'm_lReturn = AddMember(lvwAll.Items.Item(iTemp - 1).Text)
                    m_lReturn = AddMember(New ListViewItem(lvwAll.Items.Item(iTemp - 1).Text))
                End If
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: AddMember()
    '
    ' Description: Adds member to the database
    '********************************************************************

    Private Function AddMember(ByRef oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim sKey As String
        Dim sName As String = ""
        Dim lId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oListItem Is Nothing Then
                Return result
            End If

            sKey = oListItem.Name
            lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))

            '    sName$ = (lvwGroups.SelectedItem)
            lId = (lRow + 1)


            m_lReturn = m_oBusiness.EditUpdate(lRow:=lId, vPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add selected Member")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Member", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMember", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteAllMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwContents.Items.Count
                'TODO:
                'm_lReturn = DeleteMember(lvwContents.Items.Item(iTemp - 1).Text)
                m_lReturn = DeleteMember(New ListViewItem(lvwContents.Items.Item(iTemp - 1).Text))
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwContents.Items.Count
                If lvwContents.Items.Item(iTemp - 1).Selected Then
                    'TODO
                    'm_lReturn = DeleteMember(lvwContents.Items.Item(iTemp - 1).Text)
                    m_lReturn = DeleteMember(New ListViewItem(lvwContents.Items.Item(iTemp - 1).Text))
                End If
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: DeleteMember()
    '
    ' Description: Deletes member from the database
    '********************************************************************

    Private Function DeleteMember(ByRef oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim sKey As String
        Dim sName As String = ""
        Dim lId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oListItem Is Nothing Then
                Return result
            End If

            sKey = oListItem.Name
            lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))

            lId = (lRow + 1)


            m_lReturn = m_oBusiness.EditUpdate(lRow:=lId, vPartyCnt:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete selected Member")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Member", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMember", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	
	Private Sub cmdAddAllMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllMembers.Click
		
		m_lReturn = AddAllMembers()
		
	End Sub
	
	Private Sub cmdAddMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddMembers.Click
		
		m_lReturn = AddMembers()
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
		
	End Sub
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
		MessageBox.Show("There is no help associated with this screen", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
		
	End Sub
	
	Private Sub cmdDeleteAllMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAllMembers.Click
		
		m_lReturn = DeleteAllMembers()
		
	End Sub
	
	Private Sub cmdDeleteMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteMembers.Click
		
		m_lReturn = DeleteMembers()
		
	End Sub
	
	Private Sub cmdNewUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewUser.Click
		
		m_lReturn = NewUser()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

		'Set status to PMOK
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		m_lReturn = WriteToDb()
		
		'hide this form
		Me.Hide()
		
	End Sub
	
	Private Sub frmUsers_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("tabMembers", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdNewUser", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwAll", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lvwContents", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
			End With
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		'Initialise the form using selected function
		'    m_lReturn = InitialForm()
		
	End Sub
	

	Private Sub frmUsers_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		
		With uctPMResizer1
			.NoResizeByDefault = True
			.FormMinHeight = 6645
			.FormMinWidth = 9405
		End With
		
	End Sub
	
	Private Sub frmUsers_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		m_lReturn = TerminateForm()
	End Sub
	
	Private Sub lvwAll_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwAll.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwAll.Columns(eventArgs.Column)
		
		With lvwAll
			
			ListViewHelper.SetSortedProperty(lvwAll, False)
			ListViewHelper.SetSortKeyProperty(lvwAll, ColumnHeader.Index + 1 - 1)
			ListViewHelper.SetSortedProperty(lvwAll, True)
			
		End With
		
	End Sub
	
	Private Sub lvwAll_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAll.DoubleClick
		
		
		' Edit details of user if doubleclicked
		Dim oListItem As ListViewItem = lvwAll.GetItemAt(m_lXPos, m_lYPos)
		
		cmdAddMembers_Click(cmdAddMembers, New EventArgs())
		
	End Sub
	
	Private Sub lvwAll_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAll.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		m_lXPos = CInt(x)
		m_lYPos = CInt(y)
		
	End Sub
	
	Private Sub lvwContents_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwContents.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwContents.Columns(eventArgs.Column)
		
		With lvwAll
			
			ListViewHelper.SetSortedProperty(lvwAll, False)
			ListViewHelper.SetSortKeyProperty(lvwAll, ColumnHeader.Index + 1 - 1)
			ListViewHelper.SetSortedProperty(lvwAll, True)
			
		End With
		
	End Sub
	
	Private Sub lvwContents_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContents.DoubleClick
		
		
		' Edit details of user if doubleclicked
		Dim oListItem As ListViewItem = lvwAll.GetItemAt(m_lXPos, m_lYPos)
		
		cmdDeleteMembers_Click(cmdDeleteMembers, New EventArgs())
		
	End Sub
	
	Private Sub lvwContents_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContents.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		
		If Button <> 2 Then
			Exit Sub
		End If
		
		Dim oListItem As ListViewItem = lvwContents.GetItemAt(m_lXPos, m_lYPos)
		
		'    If (oListItem.SmallIcon = "group") Then
		'        Exit Sub
		'    End If
		
		'    If (mnuSuper.Checked = True) Then
		'        oListItem.SubItems(1) = "supervisor"
		'        oListItem.SmallIcon = "supervisor"
		'    Else
		'        oListItem.SubItems(1) = ""
		'        oListItem.SmallIcon = "user"
		'    End If
		'
		'    m_lReturn = ToggleSupervisor(oListItem)
		
	End Sub
	
	Private Sub lvwContents_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContents.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		m_lXPos = CInt(x)
		m_lYPos = CInt(y)
		
	End Sub

    Private Sub frmUsers_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMembers.SelectedIndex = 0
        End If
    End Sub
End Class
