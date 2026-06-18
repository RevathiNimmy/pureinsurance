Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmUsers
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmUsers
	'
	' Date: 12 July 2000
	'
	' Description: Main View Form.
	'
	' Edit History:
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmUsers"
	
	' PUBLIC Data Members (Begin)
	
	Public m_vDatArray As Object
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter member.
	Private m_vObjectParam As Object
	
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'Declare the column numbers from where the data will sought from
	Const colUserName As Integer = 0
	Const colLogonTime As Integer = 2
	Const colLoggedOnAtClient As Integer = 1
	'DAK060100
	Const colTaskInstance As Integer = 1
	
	Const colLicenceLimit As Integer = 0
	
	
	
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	' PRIVATE Property Procedures (Begin)
	
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	'*****************************************************************************
	'
	' Name: GetShowData
	'
	' Function to call the functions to get the data and populate the listview box
	'
	'*****************************************************************************
	Private Function GetShowData() As Integer
		Dim result As Integer = 0

		Try 
			
			cmdReset.Enabled = False
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'call the function selectdata from the business
			m_lReturn = g_oBusiness.Selectdata(r_vUserDataArray:=g_vUserData)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error.
				MessageBox.Show("Unable to Read Data from Sirius Architecture Database" & Strings.Chr(13) & Strings.Chr(10) & "UsersLoggedOn will be shut down.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Obtain and display data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			
			Return result
		End Try
	End Function
	
	'***************************************************************************
	'
	' Name: DisplayData
	'
	' Function to Display the data in the listview control
	'
	'***************************************************************************
	
	Private Function DisplayData() As Integer
		
		Dim result As Integer = 0
		Dim oItem As ListViewItem
		Dim sKey, sCode As String

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Loop through the data array
			lstInstances.Items.Clear()
			
			cmdOK.Enabled = True
			
			If Information.IsArray(g_vUserData) Then
				
				cmdOK.Enabled = False
				

				For lRow As Integer = g_vUserData.GetLowerBound(1) To g_vUserData.GetUpperBound(1)
					
					' Create key which contains the row number
					sKey = "L" & lRow
					
					' Get the code from the array

                    sCode = CStr(g_vUserData(colUserName, lRow)).Trim()

                    ' Add a new listitem to the listview

                    oItem = lstInstances.Items.Add(sKey, sCode, "")

                    ' Set the other data into the other columns

                    ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(g_vUserData(colLoggedOnAtClient, lRow)) & ""

                    ListViewHelper.GetListViewSubItem(oItem, 2).Text = CStr(g_vUserData(colLogonTime, lRow)) & ""
				Next lRow
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFormDefaults
	'
	' Description: Get and display all of the form's default values.
	'
	' ***************************************************************** '
	Private Function SetFormDefaults() As Integer
		Dim result As Integer = 0

		Try 
			
			
			' Get all lookup details
			
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the forms defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click Event Of The OK Button.
		Try 
			
			' Unload me.
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdMessage_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMessage.Click
		
		Dim vMachines As Object
		
		Dim oMsg As New frmMessage
		
		ReDim vMachines(lstInstances.Items.Count - 1)
		
		For iLoop As Integer = 1 To lstInstances.Items.Count

            vMachines(iLoop) = ListViewHelper.GetListViewSubItem(lstInstances.Items.Item(iLoop - 1), 1).Text
		Next 
		

		oMsg.Machines = vMachines
		
		If Not (lstInstances.FocusedItem Is Nothing) Then
            'oMsg.Machine = lstInstances.listViewHelper1.GetListViewSubItem(lstInstances.FocusedItem, 1).Text
            oMsg.Machine = lstInstances.FocusedItem.SubItems(1).Text
		Else
			oMsg.Machine = ""
		End If
		
		oMsg.ShowDialog()
		
		oMsg = Nothing
		
	End Sub
	
	Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click
		Dim iFilenum As Integer
		
		
		m_lReturn = CType(GetShowData(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			iFilenum = FileSystem.FreeFile()
			FileSystem.FileOpen(iFilenum, "C:\UsersLoggedOn.txt", OpenMode.Output)
			FileSystem.PrintLine(iFilenum, "Error")
			FileSystem.FileClose(iFilenum)
			
			Exit Sub
			
		End If
		
		m_lReturn = CType(DisplayData(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			iFilenum = FileSystem.FreeFile()
			FileSystem.FileOpen(iFilenum, "C:\UsersLoggedOn.txt", OpenMode.Output)
			FileSystem.PrintLine(iFilenum, "Error")
			FileSystem.FileClose(iFilenum)
			
			Exit Sub
			
		End If
		
		tmrRefreshInstances.Enabled = False
		tmrRefreshInstances.Enabled = True
		
	End Sub
	
	Private Sub cmdReset_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReset.Click
		Dim retval As DialogResult
		
		'If nothing is selected then prompt the user via a message box to
		'make a selection
		If lstInstances Is Nothing Then
			MessageBox.Show("A User needs to be selected" & Strings.Chr(10).ToString() & "before this command can be executed", "Licence Admin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub
		Else
			retval = MessageBox.Show("Reset the selected login?", "Find", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
			If retval = System.Windows.Forms.DialogResult.OK Then
				g_oBusiness.UpdatePMUser(lstInstances.FocusedItem.Text)
				cmdRefresh_Click(cmdRefresh, New EventArgs())
			End If
		End If
		
		Exit Sub
		
	End Sub
	
	' PRIVATE Methods (End)
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms Initialise Event.
		
        Try

            ' Default global variables
            g_iLanguageID = 1
            g_iSourceID = 1
            g_iCurrencyID = 1
            g_iUserID = 1

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the form object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	
	Private Sub frmUsers_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
            ' Sets up the form before displaying.
			
			Static bFormActivated As Boolean

			Try 
				
				' Check the static flag to see if this
				' function has been called.
				If bFormActivated Then
					Exit Sub
				End If
				
				' Set the static flag to true to indicate
				' we have called this function.
				bFormActivated = True
				
				m_lReturn = CType(DisplayData(), gPMConstants.PMEReturnCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Me.Close()
					Exit Sub
				End If
				
				tmrRefreshInstances.Enabled = True
				
				'Set some defaults
				
				cmdReset.Enabled = False
				
				Exit Sub
			
			Catch excep As System.Exception
				
				
				
				' Error Section
				
				m_lReturn = gPMConstants.PMEReturnCode.PMError
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the form ", vApp:=ACApp, vClass:=ACClass, vMethod:="Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Me.Close()
				Exit Sub
				
			End Try
		End If
	End Sub
	

	Private Sub frmUsers_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Sets up the forms defaults.
		
		
		' Set the form's default values
		Dim lErrorValue As Integer = SetFormDefaults()
		
		' Check for errors.
		If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
			m_lReturn = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set the form's defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
			
			Exit Sub
		End If
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click Event Of The OK Button.
		Try 
			
			' Unload me.
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmUsers_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		
		Dim iFilenum As Integer = FileSystem.FreeFile()
		FileSystem.FileOpen(iFilenum, "C:\UsersLoggedOn.txt", OpenMode.Output)
		FileSystem.PrintLine(iFilenum, CStr(lstInstances.Items.Count))
		FileSystem.FileClose(iFilenum)
		
	End Sub
	
	Private Sub lstInstances_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstInstances.Click
		
		If lstInstances.FocusedItem Is Nothing Then
			Exit Sub
		End If
		
		If lstInstances.FocusedItem.Text <> "" Then
			cmdReset.Enabled = True
			cmdMessage.Enabled = True
		Else
			cmdReset.Enabled = False
			cmdMessage.Enabled = False
		End If
		
	End Sub
	
	Private Sub tmrRefreshInstances_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrRefreshInstances.Tick
		
		Try 
			
			tmrRefreshInstances.Enabled = gPMConstants.PMEReturnCode.PMTrue
			
			cmdRefresh_Click(cmdRefresh, New EventArgs())
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the form details", vApp:=ACApp, vClass:=ACClass, vMethod:="tmrRefreshInstances_Timer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
End Class
