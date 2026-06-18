Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	 ' ***************************************************************** '
	 ' Form Name: frmInterface
	 '
	 ' Date: {17/2/98}
	 '
	 ' Description: Main interface.
	 '
	 ' Edit History:
	 ' ***************************************************************** '
	
	
	 ' Constant for the functions to identify
	 ' which class this is.
	Private Const ACClass As String = "frmInterface"
	
    Private objfrmChange As New frmChange
	 ' PUBLIC Data Members (Begin)
	 ' PUBLIC Data Members (End)
	
	
	 ' PRIVATE Data Members (Begin)
	
	
	 '***Insert Form Constants***
	
	 ' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	
	 ' {* USER DEFINED CODE (Begin) *}
	 ' {* USER DEFINED CODE (End) *}
	
	
	 ' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	 ' Stores the return value for the a
	 ' function call.
	Private m_lreturn As Integer
	
	
	 ' Stores the details from the business object.
	
	 ' {* USER DEFINED CODE (Begin) *}
	 ' {* USER DEFINED CODE (End) *}
	 ' PRIVATE Data Members (End)
	
	
	 ' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			 ' Standard Property.
			
			 ' Return any error number that might have
			 ' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			 ' Standard Property.
			
			 ' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	 ' {* USER DEFINED CODE (Begin) *}
	 ' {* USER DEFINED CODE (End) *}
	 ' PUBLIC Property Procedures (End)
	 ' PRIVATE Property Procedures (Begin)
	
	 'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	 'Private Sub Status(ByVal Value As Integer)
		 '
		 ' Standard Property.
		 '
		 ' Set the interface exit status.
		 'm_lStatus = Value
		 '
	 'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			 ' Standard Property.
			
			 ' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	 ' PRIVATE Property Procedures (End)
	
	
	 ' PUBLIC Methods (Begin)
	
	 ' ***************************************************************** '
	 ' Name: GetBusiness
	 '
	 ' Description: Retrieves the details from the business object.
	 '
	 ' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			 ' Get the details from the business object.
			
			 ' {* USER DEFINED CODE (Begin) *}
			
			 '    m_lreturn& = m_oBusiness.GetDetails()
			
			 ' {* USER DEFINED CODE (End) *}
			
			 ' Check for errors
			 '    If (m_lreturn& <> PMTrue) Then
			 '        ' Failed to get details.
			 '        GetBusiness = PMFalse
			 '
			 '        ' Log Error.
			 '        LogMessage _
			 ''            iType:=PMLogError, _
			 ''            sMsg:="Failed to get details from the business object", _
			 ''            vApp:=ACApp, _
			 ''            vClass:=ACClass, _
			 ''            vMethod:="GetBusiness"
			 '
			 '        Exit Function
			 '    End If
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			 ' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			 ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


     ' PRIVATE Methods (Begin)

     ' ***************************************************************** '
     ' Name: SetInterfaceDefaults
     '
     ' Description: Sets all of the interface default values.
     '
     ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

             ' Center the interface.
             'TODO
             'CenterForm(Me)

             ' Display all language specific captions.
            m_lreturn = DisplayCaptions()

             ' Check for errors.
            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


             ' Set any other default values to the interface.

             ' {* USER DEFINED CODE (Begin) *}
             ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



             ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
     ' ***************************************************************** '
     ' Name: ProcessCommand
     '
     ' Description: Determines which action to take on the details
     '              depending upon the task and interface state.
     '
     ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
     

        Try


             ' Check the task.
             '    Select Case (m_frmInterface.Task)
             '        Case PMAdd, PMEdit
             '    End Select

             ' Check the task.
             '    Select Case (m_frmInterface.Task)
             '        Case PMAdd
             '            ' Check if form has been cancelled, if so,
             '            ' prompt if you wish to lose details.
             '            If (m_frmInterface.Status = PMCancel) Then
             '                ' Get string messages
             '
             '                sTitle$ = GetResData( _
             ''                    iLangID:=g_iLanguageID%, _
             ''                    lID:=ACCancelDetailsTitle, _
             ''                    iDataType:=PMResString)
             '
             '                sMessage$ = GetResData( _
             ''                    iLangID:=g_iLanguageID%, _
             ''                    lID:=ACCancelDetails, _
             ''                    iDataType:=PMResString)
             '
             '                iMsgResult = MsgBox(sMessage$, _
             ''                    vbYesNo + vbDefaultButton2 + vbQuestion, sTitle$)
             '
             '                ' Check message result.
             '                If (iMsgResult = vbNo) Then
             '                    ' Set return to false, meaning
             '                    ' don't cancel.
             '                    ProcessCommand = PMFalse
             '                End If
             '            Else
             '                ' Form hasn't been cancelled, so we just go
             '                ' ahead and add the details.
             '
             '                ' Add the details using the business object.
             '                m_lreturn& = m_oBusiness.Update()
             '
             '                ' Check for errors.
             '                If (m_lreturn& <> PMTrue) Then
             '                   ' Failed to add the details
             '                   ProcessCommand = PMFalse
             '
             '                   ' Log Error.
             '                   LogMessage _
             ''                       iType:=PMLogError, _
             ''                       sMsg:="Failed to add the details", _
             ''                       vApp:=ACApp, _
             ''                       vClass:=ACClass, _
             ''                       vMethod:="ProcessCommand"
             '                End If
             '            End If
             '
             '        Case PMEdit
             '            ' Check if form has been cancelled, if so,
             '            ' check if the details have changed and if
             '            ' so, prompt if they wish to cancel.
             '            If (m_frmInterface.Status = PMCancel) Then
             '                ' Check the details havn't changed.
             '                m_lreturn& = m_oBusiness.Cancel()
             '
             '                If (m_lreturn& = PMDataChanged) Then
             '                    ' Get string messages
             '
             '                    sTitle$ = GetResData( _
             ''                        iLangID:=g_iLanguageID%, _
             ''                        lID:=ACCancelDetailsTitle, _
             ''                        iDataType:=PMResString)
             '
             '                    sMessage$ = GetResData( _
             ''                        iLangID:=g_iLanguageID%, _
             ''                        lID:=ACCancelDetails, _
             ''                        iDataType:=PMResString)
             '
             '                    iMsgResult = MsgBox(sMessage$, _
             ''                    vbYesNo + vbDefaultButton2 + vbQuestion, sTitle$)
             '
             '                    ' Check message result.
             '                    If (iMsgResult = vbNo) Then
             '                        ' Set return to false, meaning
             '                        ' don't cancel.
             '                        ProcessCommand = PMFalse
             '                    End If
             '                End If
             '            Else
             '                ' Update the details using the business object.
             '                m_lreturn& = m_oBusiness.Update()
             '
             '                ' Check for errors.
             '                If (m_lreturn& <> PMTrue) Then
             '                   ' Failed to update the details
             '                   ProcessCommand = PMFalse
             '
             '                   ' Log Error.
             '                   LogMessage _
             ''                       iType:=PMLogError, _
             ''                       sMsg:="Failed to update the details", _
             ''                       vApp:=ACApp, _
             ''                       vClass:=ACClass, _
             ''                       vMethod:="ProcessCommand"
             '                End If
             '            End If
             '    End Select

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



             ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


             ' Display all language specific captions.

             '    Me.Caption = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACInterfaceTitle, _
             ''        iDataType:=PMResString)
             '
             '    ' Check for an error.
             '    If (Me.Caption = "") Then
             '        ' Failed to get data from the resource file.
             '        DisplayCaptions = PMFalse
             '
             '        ' Log Error.
             '        LogMessage _
             ''            iType:=PMLogError, _
             ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
             ''            "Please check the file exists and the correct captions are available", _
             ''            vApp:=ACApp, _
             ''            vClass:=ACClass, _
             ''            vMethod:="DisplayCaptions"
             '
             '        Exit Function
             '    End If

             '    cmdOK.Caption = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACOKButton, _
             ''        iDataType:=PMResString)
             '
             '    cmdCancel.Caption = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACCancelButton, _
             ''        iDataType:=PMResString)
             '
             '    cmdHelp.Caption = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACHelpButton, _
             ''        iDataType:=PMResString)
             '
             '    cmdNavigate.Caption = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACNavigateButton, _
             ''        iDataType:=PMResString)
             '
             '    tabMainTab.TabCaption(0) = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACTabTitle1, _
             ''        iDataType:=PMResString)

             ' {* USER DEFINED CODE (Begin) *}

             ' ************************************************************
             ' Enter your code here to display all language specific
             ' captions.
             ' The GetResData function will allow you to do this.
             '
             ' Example:-
             '
             '    lblDesc.Caption = GetResData( _
             ''        iLangID:=g_iLanguageID%, _
             ''        lID:=ACDesc, _
             ''        iDataType:=PMResString)
             '
             ' NOTE: Replace this section with your new code.
             ' ************************************************************

             ' {* USER DEFINED CODE (End) *}

             '***Insert GetRes Calls***

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



             ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdChange_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChange.Click

        If lvwUsers.Items.Count > 0 Then

             ' Change the caption on the frame
            objfrmChange.fraChange.Text = "User : " & lvwUsers.FocusedItem.Text
            objfrmChange.fraChange.Tag = "USER" & lvwUsers.FocusedItem.Tag
             ' Set the combo box to show the current access level
            objfrmChange.cboNewLevel.Text = ListViewHelper.GetListViewSubItem(lvwUsers.Items.Item(lvwUsers.FocusedItem.Index), 1).Text

             ' show the form
            objfrmChange.ShowDialog(Me)

        End If

    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        Dim iUser As Integer

         ' only ask to remove if there's anything to actually remove
        If lvwUsers.Items.Count > 0 Then

             ' are they sure they want to remove the user?
            m_lreturn = MessageBox.Show("Are you sure you want to remove this user?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

             ' yes, so remove it
            If m_lreturn = System.Windows.Forms.DialogResult.Yes Then
                iUser = Conversion.Val(Convert.ToString(lvwUsers.FocusedItem.Tag))


                m_lreturn = m_oBusiness.RemoveUser(iUser:=iUser)

                m_lreturn = GetUsers()
            End If
        End If

    End Sub

     ' PRIVATE Methods (End)
     '

     ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Dim iAccessLevel As Integer

         ' Forms initialise event.

        Try

             ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

             ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

             ' Get an instance of the business object via
             ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lreturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bDOCUserAdmin.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

             ' Check for errors.
            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                 ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                 ' Display error stating the problem.

                 ' Get description from the resource file.
                 'sTitle$ = GetResData( _
                 'iLangID:=g_iLanguageID%, _
                 'lID:=ACBusinessFailTitle, _
                 'iDataType:=PMResString)

                 'sMessage$ = GetResData( _
                 'iLangID:=g_iLanguageID%, _
                 'lID:=ACBusinessFail, _
                 'iDataType:=PMResString)

                 ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

             ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            m_lreturn = m_oBusiness.GetAdminLevel(iAdminLevel:=iAccessLevel)

            cboAdminLevel.Text = CStr(iAccessLevel)

             ' Get user names and display them
            m_lreturn = GetUsers()
            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



             ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

         ' Forms load event.

        Try

             ' Check if we have had an error so far.
             ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                 ' We have already encountered an error,
                 ' so we MUST exit now.
                Exit Sub
            End If

             ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


             ' {* USER DEFINED CODE (Begin) *}
             ' {* USER DEFINED CODE (End) *}

             ' Set the interface default values.
            m_lreturn = SetInterfaceDefaults()

             ' Check for errors.
            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                 ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

             ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



             ' Error Section

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

         ' Forms query unload event.

        Try

             ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

             ' Check if the interface has been terminated by means
             ' other than pressing the command buttons.

             'TODO 1 replaced vbFormCode
            If UnloadMode <> 1 Then
                 ' Process the next set of actions depending
                 ' upon the interface task etc.
                m_lreturn = ProcessCommand()

                 ' Check the return value.
                If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                     ' Do not procced with the interface termination.
                    Cancel = 1

                     ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If


             ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
             ' from memory.
            m_oBusiness = Nothing

             ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



             ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

         ' Click event of the OK button.

        Dim iAdminLevel As Integer

        Try

             ' Get the admin level
            iAdminLevel = Conversion.Val(cboAdminLevel.Text)

             ' Set the access level

            m_lreturn = m_oBusiness.SetAdminLevel(iAdminLevel:=iAdminLevel)
            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                 ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the admin level.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

             ' Update the users
            m_lreturn = UpdateUserLevels()
            If m_lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                 ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the user access level.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

             ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

             ' Process the next set of actions depending
             ' upon the interface task etc.
            m_lreturn = ProcessCommand()

             ' Check the return value.
            If m_lreturn = gPMConstants.PMEReturnCode.PMTrue Then
                 ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



             ' Error Section.

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

         ' Click event of the Cancel button.

        Try

             ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

             ' Process the next set of actions depending
             ' upon the interface task etc.
            m_lreturn = ProcessCommand()

             ' Check the return value.
            If m_lreturn = gPMConstants.PMEReturnCode.PMTrue Then
                 ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



             ' Error Section.

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

     ' ***************************************************************************
     ' Function:         GetUsers
     '
     ' Description:      Displays the list of users and their access levels
     '
     ' Edit History :
     ' RAM20021204  : 1. NRMA Project Changes. Sirius Process No. 189
     '                2. Since the users are fetched from the PMUser Table, the Column index
     '                   values for the arrays are changes to get correct values
     ' ***************************************************************************
    Private Function GetUsers() As Integer

        Dim result As Integer = 0
        Const ACColPMUserUserID As Integer = 0
        Const ACColPMUserUserName As Integer = 3
        Const ACColPMUserIsDeleted As Integer = 12
         ' CTAF 20030807 - Changed doc_doc_user constants due to new PMUser columns
         ' CTAF 20040422 - As above.
        Const ACColDOC_doc_userAccessLevel As Integer = 29
         '   Const ACColDOC_doc_userRetired As Integer = 32

        Dim vUserNames As Object
        Dim sKey As String = ""
        Dim nodX As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lreturn = m_oBusiness.GetUserNames(vUserNames:=vUserNames)

            lvwUsers.Items.Clear()


            For lLoop1 As Integer = 0 To vUserNames.GetUpperBound(1)

                 ' Check whether the user is deleted in PMUser Table, Don't worrry about other

                If CDbl(vUserNames(ACColPMUserIsDeleted, lLoop1)) = 0 Then


                    sKey = "USER" & CStr(vUserNames(ACColPMUserUserID, lLoop1))


                    nodX = lvwUsers.Items.Add(sKey, CStr(vUserNames(ACColPMUserUserName, lLoop1)), "")


                    nodX.Tag = CStr(vUserNames(ACColPMUserUserID, lLoop1))
                     'Default access level to 9 if not set

                    If (CStr(vUserNames(ACColDOC_doc_userAccessLevel, lLoop1))) = "" Then
                        ListViewHelper.GetListViewSubItem(nodX, 1).Text = CStr(9)
                    Else

                        ListViewHelper.GetListViewSubItem(nodX, 1).Text = CStr(vUserNames(ACColDOC_doc_userAccessLevel, lLoop1))
                    End If

                End If
            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the user + user levels.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UpdateUserLevels() As Integer

        Dim result As Integer = 0
        Dim vAccessLevels As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim vAccessLevels(lvwUsers.Items.Count, 2)

        For iLoop1 As Integer = 1 To lvwUsers.Items.Count


            vAccessLevels(iLoop1, 1) = Convert.ToString(lvwUsers.Items.Item(iLoop1 - 1).Tag)

            vAccessLevels(iLoop1, 2) = Conversion.Val(ListViewHelper.GetListViewSubItem(lvwUsers.Items.Item(iLoop1 - 1), 1).Text)
        Next iLoop1


        m_lreturn = m_oBusiness.UpdateUsers(vAccessLevels:=vAccessLevels)

        Return result



        result = gPMConstants.PMEReturnCode.PMError

         ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update access levels.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserLevels", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
	
	Private Sub lvwUsers_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwUsers.DoubleClick
		
		cmdChange_Click(cmdChange, New EventArgs())
		
	End Sub
End Class
