Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles


Friend Partial Class frmWorkflowPackages
	Inherits System.Windows.Forms.Form
	' Description: Display the list of workflow packages
	'
	' Edit History:
	'               AMB 23/01/2003 - Shamelessly ripped from the Task Group Maintenance components
	'
	' ***************************************************************** '
	
	Private Const ACClass As String = "frmWorkflowPackages"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lError As gPMConstants.PMEReturnCode
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_lXPos As Integer
	Private m_lYPos As Integer
	Private m_lReturn As Integer
	Private m_bInitialised As Boolean
	
	Private m_iPackageID As Integer
	Private m_sPackageName As String = ""
	Private m_sDescription As String = ""
	Private m_dEffectiveDate As Date
	Private m_iIsDeleted As gPMConstants.PMEReturnCode
	
	' CurrentKey
	Private m_sCurrentKey As String = ""
	Private m_sCurrentItemID As String = ""
	
	Private Const LISTITEM_KEY_PREFIX As String = "KEY:"
	Private Const LISTITEM_KEY_DELIMITER As String = ":"
	Private Const LIST_COL_DESC As Integer = 1
	Private Const LIST_COL_DATE As Integer = 2
	

	Private m_oBusiness As bPMWorkflowMaintenance.Business
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		'Hide the form
		Me.Hide()
		
	End Sub
	
	
	Private Sub frmWorkflowPackages_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdApply", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabMain", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwPackages", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
			
			lvwPackages.Focus()
			
		End If
	End Sub
	
	Private Function InitialForm() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMWorkflowMaintenance.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Display message.
				MessageBox.Show(ACBusinessFailText, ACBusinessFailTitleText, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
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
	

	Private Sub frmWorkflowPackages_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		iPMFunc.ShowFormInTaskBar_Detach()
		
		' Check if we have had an error so far.
		' Possibly creating the business object.
		If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
			' We have already encountered an error,
			' so we MUST exit now.
			Exit Sub
		End If
		
		With uctPMResizer1
			.NoResizeByDefault = True
			.FormMinHeight = 6180
			.FormMinWidth = 8685
		End With
		
		'Populate the listview using the selected function
		m_lReturn = PopulateListview()
		If lvwPackages.Items.Count Then
            'Developer Guide No.185
            'lvwPackages_ItemClick(lvwPackages, New EventArgs())
            lvwPackages_ItemClick(lvwPackages.SelectedItems.Item(0))
		End If
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			m_lError = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Exit Sub
		End If
		
	End Sub
	
	Private Sub frmWorkflowPackages_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		'Terminate the form
		m_lReturn = TerminateForm()
		
	End Sub
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lError
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	
	Public Property CurrentKey() As String
		Get
			Return m_sCurrentKey.Trim()
		End Get
		Set(ByVal Value As String)
			m_sCurrentKey = Value.Trim()
			
			If Value.Length Then
				CurrentItemID = CInt(Mid(Value, (Value.IndexOf(LISTITEM_KEY_DELIMITER) + 1) + 1))
			End If
			
		End Set
	End Property
	
	Public Property CurrentItemID() As Integer
		Get
			Return CInt(m_sCurrentItemID)
		End Get
		Set(ByVal Value As Integer)
			m_sCurrentItemID = CStr(Value)
		End Set
	End Property
	
	
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
	' Function Name: Refreshlist
	'
	' Description: Refreshes the listview box with data held in
	'              the business, not the database.
	'****************************************************************************
	
	Public Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lIndex, lRow As Integer
		Dim oListItem As ListViewItem
		Dim lCurrIndex As Integer
		
		Try 
			
			' Set the mouse pointer to busy
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Me.Visible Then
				If Not (lvwPackages.FocusedItem Is Nothing) Then
					lCurrIndex = lvwPackages.FocusedItem.Index + 1
				End If
				lvwPackages.Focus()
			End If
			

			m_oBusiness.CurrentRecord = 0
			
			'Clear the items in the listview box
			lvwPackages.Items.Clear()
			
			'Set row count
			lRow = 0
			
			Do 
				'Get id of first record

				m_lReturn = m_oBusiness.GetNext(vPMPackageID:=m_iPackageID, vPackageCode:=m_sPackageName, vPackageDescription:=m_sDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate)
				
				'Exit if you get an invalid response
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Exit Do
				End If
				
				lIndex = (m_iPackageID)
				'Populate the listview box

				oListItem = lvwPackages.Items.Add(LISTITEM_KEY_PREFIX & CStr(lIndex), m_sPackageName.Trim(), "")
				With oListItem
					ListViewHelper.GetListViewSubItem(oListItem, LIST_COL_DESC).Text = m_sDescription
					ListViewHelper.GetListViewSubItem(oListItem, LIST_COL_DATE).Text = DateTimeHelper.ToString(m_dEffectiveDate)
					If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                        'Developer Guide No.11
                        '.Ghosted = True
					End If
				End With
				
			Loop 
			
			If lvwPackages.Items.Count = 0 Then
				CurrentKey = ""
				' Set the mouse pointer to normal
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Return result
			Else
				If (lCurrIndex > 0) And (lCurrIndex <= lvwPackages.Items.Count) Then
					lvwPackages.FocusedItem = lvwPackages.Items.Item(lCurrIndex - 1)
				Else
					lvwPackages.FocusedItem = lvwPackages.Items.Item(0)
				End If
			End If
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh Package details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'****************************************************************************
	'
	' Function Name: PopulateListview
	'
	' Description: Populates listview box
	'
	'****************************************************************************
	
	Public Function PopulateListview() As Integer
		
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
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load Package details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListview", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			' Set the mouse pointer to busy
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			

			m_lReturn = m_oBusiness.Update
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Changes to Package Details Could not be written to Database", "Package Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                m_oBusiness.Dispose()
				m_oBusiness = Nothing
				m_lReturn = InitialForm()
				m_lReturn = PopulateListview()
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = PopulateListview()
			
			cmdApply.Enabled = False
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write details to database", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteToDb", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: EditPackage()
	'
	' Description: Writes edited details of an existing user to the database
	'********************************************************************
	
	Private Function EditPackage(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Dim oPackageForm As frmWorkflowPackageDetail
		Dim lStatus As Integer

		Try 
			
			' Set the mouse pointer to busy
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' If User is Deleted/Marked for Deletion then exit

            'TODO atRuntime as per Developer Guide No.
            'If oListItem.Ghosted Then
            '	iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            '	Return result
            'End If
			
			CurrentKey = oListItem.Name
			
			'Getdetails from business

			m_oBusiness.CurrentRecord = CurrentItemID - 1
			

			m_lReturn = m_oBusiness.GetNext(vPMPackageID:=m_iPackageID, vPackageCode:=m_sPackageName, vPackageDescription:=m_sDescription, vEffectiveDate:=m_dEffectiveDate)
			
			'Exit if you get an invalid response
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Set the mouse pointer to normal
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			oPackageForm = New frmWorkflowPackageDetail()

            'Developer Guide No.68
            'Load(oPackageForm)

			
			With oPackageForm
				
				.PackageID = m_iPackageID
				.Code = m_sPackageName
				.Description = m_sDescription
				.EffectiveDate = m_dEffectiveDate
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					m_lError = gPMConstants.PMEReturnCode.PMFalse
					
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Return result
				End If
				
				' Set the mouse pointer to normal
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				.ShowForm(USREditPackage)
				
				' Set the mouse pointer to busy
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
				
				lStatus = .Status
				
				If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
					
					If Me.Visible Then
						lvwPackages.Focus()
					End If
					
				Else
					

					m_lReturn = m_oBusiness.EditUpdate(lRow:=CurrentItemID, vPMPackageID:=.PackageID, vPackageCode:=.Code, vPackageDescription:=.Description, vEffectiveDate:=.EffectiveDate, vIsDeleted:=.IsDeleted)
					
					If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
						
						m_lReturn = WriteToDb()
						
					End If
					cmdApply.Enabled = True
					
				End If
				
			End With
			
			oPackageForm.Close()
			
			oPackageForm = Nothing
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Package Details", vApp:=ACApp, vClass:=ACClass, vMethod:="EditPackage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: AddPackage()
	'
	' Description: Adds new user to the database
	'********************************************************************
	
	Private Function AddPackage() As Integer
		
		Dim result As Integer = 0
		Dim lStatus As Integer
        Dim lRow As Integer
		Dim sPrinter As String = ""
		
		Select Case Information.Err().Number
			Case Is < 0
				Conversion.ErrorToString(5)
			Case 1
				GoTo err_AddPackage
		End Select
		
		' Set the mouse pointer to busy
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Dim oPackageForm As New frmWorkflowPackageDetail

        'Developer Guide No.68
        'Load(oPackageForm)
		
		With oPackageForm
			
			lRow = lvwPackages.Items.Count + 1
			.Code = ""
			.EffectiveDate = DateTime.Now
			
			' Set the mouse pointer to normal
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			.ShowForm(USRAddPackage)
			
			' Set the mouse pointer to busy
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			lStatus = .Status
			
			If lStatus = gPMConstants.PMEReturnCode.PMOK Then
				

				m_lReturn = m_oBusiness.Editadd(lRow:=lRow, vPMPackageID:=lRow, vPackageCode:=.Code, vPackageDescription:=.Description, vEffectiveDate:=.EffectiveDate, vIsDeleted:=.IsDeleted)
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					m_lReturn = WriteToDb()
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Set the mouse pointer to normal
						iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
						Return result
					End If
				Else
					result = gPMConstants.PMEReturnCode.PMFalse
					' Set the mouse pointer to normal
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					Return result
				End If
				
				CurrentKey = LISTITEM_KEY_PREFIX & CStr(lRow)
				cmdApply.Enabled = True
				
			End If
			
			If Me.Visible Then
				lvwPackages.Focus()
				lvwPackages.Items.Item(CurrentKey).Selected = True

				lvwPackages.Items.Item(CurrentItemID - 1).EnsureVisible()
                'Developer Guide No.185
                'lvwPackages_ItemClick(lvwPackages, New EventArgs())
                lvwPackages_ItemClick(lvwPackages.SelectedItems.Item(0))
			End If
			
		End With
		
		oPackageForm.Close()
		oPackageForm = Nothing
		
		' Set the mouse pointer to normal
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Return result
		
err_AddPackage: 
		
		'Error Section
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Set the mouse pointer to normal
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Package", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPackage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	'********************************************************************
	'
	' Function Name: DeletePackage()
	'
	' Description: Deletes Package from the database
	'********************************************************************
	
	Private Function DeletePackage(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Try 
			
			' Set the mouse pointer to busy
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			If CurrentItemID = 0 Then
				Return result
			End If
			

            'If lvwPackages.SelectedItem.Ghosted Then

            '	m_lReturn = m_oBusiness.EditUpdate(lRow:=CurrentItemID, vIsDeleted:=gPMConstants.PMEReturnCode.PMFalse, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Undelete selected Package")

                ' Set the mouse pointer to normal
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If


            'lvwPackages.SelectedItem.Ghosted = False
            'Else

            '         m_lReturn = m_oBusiness.EditDelete(CurrentItemID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete selected Package")
                ' Set the mouse pointer to normal
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
                '  End If


                'lvwPackages.SelectedItem.Ghosted = True

            End If

            ' Refresh List & enable apply.
            lvwPackages.Refresh()
            cmdApply.Enabled = True

            ' Select the Item deleted so that the Delete button
            ' caption can be set correctly
            'Developer Guide No.185
            'lvwPackages_ItemClick(lvwPackages, New EventArgs())
            lvwPackages_ItemClick(lvwPackages.SelectedItems.Item(0))

            ' Set the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Package", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePackage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
	End Function
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		m_lReturn = AddPackage()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to show Add Package screen", "Package Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		End If
		
	End Sub
	
	Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
		
		m_lReturn = WriteToDb()
		
	End Sub
	
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		'delete selected user using the DeletePackage function
		m_lReturn = DeletePackage(lvwPackages.FocusedItem)
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		'Edit selected user using the EditPackage function
		m_lReturn = EditPackage(lvwPackages.FocusedItem)
		
		m_lReturn = PopulateListview()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lReturn = WriteToDb()
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		'Hide the form
		Me.Hide()
		
	End Sub
	
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lError = gPMConstants.PMEReturnCode.PMTrue
		
		iPMFunc.ShowFormInTaskBar_Attach()
		
		'Initialise the form using selected function
		m_lReturn = InitialForm()
		
	End Sub
	
	
	Private Sub lvwPackages_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPackages.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwPackages.Columns(eventArgs.Column)
		
		With lvwPackages
			
			ListViewHelper.SetSortedProperty(lvwPackages, False)
			ListViewHelper.SetSortKeyProperty(lvwPackages, ColumnHeader.Index + 1 - 1)
			ListViewHelper.SetSortedProperty(lvwPackages, True)
			
		End With
		
	End Sub
	
	Private Sub lvwPackages_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPackages.DoubleClick
		
		
		' Edit details of user if doubleclicked
		Dim oListItem As ListViewItem = lvwPackages.GetItemAt(m_lXPos, m_lYPos)
		
		If Not (oListItem Is Nothing) Then

            'Developer Guide No.11
            'If Not oListItem.Ghosted Then
            '	m_lReturn = EditPackage(oListItem)
            '	m_lReturn = PopulateListview()
            'End If
		End If
		
	End Sub
	
	Private Sub lvwPackages_ItemClick(ByVal Item As ListViewItem)
		

        'Developer Guide NO.11
        'TODO check at run time 
        'If Item.Ghosted Then
        '	cmdDelete.Text = "Undele&te"
        '	ToolTip1.SetToolTip(cmdDelete, "Undelete Selected Package")
        '	cmdDelete.Enabled = True
        '	cmdEdit.Enabled = False
        'Else
        '	cmdDelete.Text = "Dele&te"
        '	ToolTip1.SetToolTip(cmdDelete, "Delete Selected Package")
        '	cmdDelete.Enabled = True
        '	cmdEdit.Enabled = True
        'End If
		
		CurrentKey = Item.Name
		
	End Sub
	
	Private Sub lvwPackages_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPackages.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		m_lXPos = CInt(x)
		m_lYPos = CInt(y)
		
	End Sub
End Class
