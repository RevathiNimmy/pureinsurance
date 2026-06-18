Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmMain"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Business Object
	Public m_oBusiness As Integer
	
	' Data
    Private m_vDataArray As Object
	
	Private m_bDataChanged As Boolean
	
	' Values for resource values
	Private m_sDeleteCaption As String = ""
	Private m_sUnDeleteCaption As String = ""
	
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
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			' Return the objects task.
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iTask = Value
			
		End Set
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
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		m_lReturn = CType(ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Dim sMsg As String = ""
		
		If m_bDataChanged Then
			sMsg = "Are you sure you wish to exit? You will lose your changes."
			If MessageBox.Show(sMsg, "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
				Exit Sub
			End If
		End If
		
		' Set the interface status.
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		' Exit out
		Me.Hide()
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		m_lReturn = CType(DeleteItem(), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		m_lReturn = CType(ProcessDetails(v_iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Set the interface status.
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		' Set to busy pointer
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		If m_bDataChanged Then
			m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
		End If
		
		' Set to normal pointer
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		Me.Close()
		
	End Sub
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			' Set the resizer control values
			With uctPMResizer
				
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("tabMain", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lvwMain", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.FormMinHeight = 3975
				.FormMinWidth = 6495
				
			End With
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Data hasnt changed
			m_bDataChanged = False
			
			' Display the captions
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = m_lReturn
			End If
			
			' Get the list of data
			m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = m_lReturn
				Exit Sub
			End If
			
			m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = m_lReturn
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
		
		m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwMain.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: RefreshList
	'
	' Description:
	'
	' History: 23/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lstItem As ListViewItem
        Dim sText As String = String.Empty
        Dim sPartner As String = String.Empty
        Dim bInUse As Boolean = False
        Dim iTypeID As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the list
			lvwMain.Items.Clear()
			
			' Exit if no data
			If Not Information.IsArray(m_vDataArray) Then
				Return result
			End If
			
			For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
				
				If CDbl(m_vDataArray(ACArrayIsContactType, iLoop1)) = 1 Then
					
					' Add the item
					sText = CStr(m_vDataArray(ACArrayCode, iLoop1))

					lstItem = lvwMain.Items.Add("I" & iLoop1, sText, "icon")
					' Description
					ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vDataArray(ACArrayDescription, iLoop1))
					
					' Ghost the icon if it's marked as deleted
					If CStr(m_vDataArray(ACArrayIsDeleted, iLoop1)) = "1" Then

                        'TODO: handle at runtime
                        ' lstItem.Ghosted = True
					End If
					
					' Store the index of the array
					lstItem.Tag = CStr(iLoop1)
					
				End If
				
			Next iLoop1
			
			' Select the first one
			lvwMain.FocusedItem = lvwMain.Items.Item(0)
            lvwMain_ItemClick(lvwMain.SelectedItems.Item(0))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetArrayIndex
	'
	' Description:
	'
	' History: 24/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetArrayIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetArrayIndex(ByVal v_iID As Integer, ByRef r_iIndex As Integer) As Integer
		'
		'Dim result As Integer = 0
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'For 'iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
				'If CInt(m_vDataArray(ACArrayContactTypeID, iLoop1)) = v_iID Then
					'r_iIndex = iLoop1
					'Exit For
				'End If
			'Next iLoop1
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetArrayIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetArrayIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	'
	' Name: GetNextID
	'
	' Description:
	'
	' History: 24/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function GetNextID(ByRef r_lID As Integer) As Integer
		
		Dim result As Integer = 0
		Dim lNextID, lCurrentID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lNextID = 0
			
			For iLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
				
				lCurrentID = CInt(m_vDataArray(ACArrayContactTypeID, iLoop1))
				
				If lCurrentID > lNextID Then
					lNextID = lCurrentID
				End If
				
			Next iLoop1
			
			' Return the next id
			r_lID = lNextID + 1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: ProcessDetails
	'
	' Description:
	'
	' History: 23/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function ProcessDetails(ByVal v_iTask As Integer) As Integer
		
		Dim result As Integer = 0
		Dim oDetail As frmDetails
		Dim iIndex As Integer
		Dim sTag As String = ""
		Dim lNextFreeID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse's pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Get the index if in edit mode
			If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
				iIndex = Convert.ToString(lvwMain.FocusedItem.Tag)
			End If
			
			' Get a new instance of the form
			oDetail = New frmDetails()
			
			' Load it

            'Load(oDetail)
			
			' Set the task
			oDetail.Task = v_iTask
			
			' Pass the array through for the combo box
            oDetail.DataArray = m_vDataArray
			
			' Set the properties if in edit mode
			If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
				
				With oDetail
					.ContactTypeID = CInt(m_vDataArray(ACArrayContactTypeID, iIndex))
					.Code = CStr(m_vDataArray(ACArrayCode, iIndex))
					.Description = CStr(m_vDataArray(ACArrayDescription, iIndex))
				End With
				
			End If
			
			If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				
				oDetail.InUse = False
				
			End If
			
			' Initialise it
			m_lReturn = CType(CType(oDetail, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse's pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the mouse's pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Show it
			oDetail.ShowDialog()
			
			' Get the values
			If oDetail.Status <> gPMConstants.PMEReturnCode.PMCancel Then
				
				' Warn about data changed when exiting
				m_bDataChanged = True
				
				If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
					
					If Information.IsArray(m_vDataArray) Then
						iIndex = m_vDataArray.GetUpperBound(1) + 1
						ReDim Preserve m_vDataArray(7, iIndex)
					Else
						iIndex = 0
						ReDim m_vDataArray(7, iIndex)
					End If
					
					m_lReturn = CType(GetNextID(r_lID:=lNextFreeID), gPMConstants.PMEReturnCode)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
					
					' Get the next free number so that new contact can be set up to this one
					m_vDataArray(ACArrayContactTypeID, iIndex) = lNextFreeID
					
					' This is zero so that the business knows to do an add
					m_vDataArray(ACArrayCaptionID, iIndex) = 0
					
					' Set type of contact
					m_vDataArray(ACArrayIsContactType, iIndex) = 1
					m_vDataArray(ACArrayIsCorrespondenceType, iIndex) = 0
					
				End If
				
				m_vDataArray(ACArrayCode, iIndex) = oDetail.Code
				m_vDataArray(ACArrayDescription, iIndex) = oDetail.Description
				m_vDataArray(ACArrayIsDeleted, iIndex) = 0
				
			End If
			
			' Terminate it
            oDetail.Dispose()

			' CTAF (not DC) 160800 - Unload the form
			oDetail.Close()
			
			oDetail = Nothing
			
			' Refresh the list
			m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Set the mouse's pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: BusinessToData
	'
	' Description:
	'
	' History: 23/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the data off the table

			m_lReturn = g_oBusiness.GetDetails(r_vDetailArray:=m_vDataArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on GetDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					result = gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub lvwMain_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwMain.DoubleClick
		
		' Edit the item on a double click
		If cmdEdit.Enabled Then
			cmdEdit_Click(cmdEdit, New EventArgs())
		End If
		
	End Sub
	
	Private Sub lvwMain_ItemClick(ByVal Item As ListViewItem)
		
		

		Dim lID As Integer = Convert.ToString(Item.Tag)
		
		' Check for delete/undelete
		If CStr(m_vDataArray(ACArrayIsDeleted, lID)) = "0" Then
			cmdDelete.Text = m_sDeleteCaption
		Else
			cmdDelete.Text = m_sUnDeleteCaption
		End If
		
	End Sub
	
	Private Sub lvwMain_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwMain.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		Dim bEnable As Boolean
		
		' Check to enable or disable buttons
		bEnable = Not (lvwMain.GetItemAt(x, y) Is Nothing)
		
		cmdEdit.Enabled = bEnable
		cmdDelete.Enabled = bEnable
		
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: DeleteItem
	'
	' Description:
	'
	' History: 24/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function DeleteItem() As Integer
		
		Dim result As Integer = 0
		Dim iIndex, iIsDeleted As Integer
        Dim bInUse As Boolean = False
        Dim sMsg As String = String.Empty
        Dim iID As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the index
			iIndex = Convert.ToString(lvwMain.FocusedItem.Tag)
			
			' Get the property
			iIsDeleted = CInt(m_vDataArray(ACArrayIsDeleted, iIndex))
			
			' Invert it
			'iIsDeleted = Not iIsDeleted ' This gives 0-> -1.. what the?!?
			If iIsDeleted = 0 Then
				iIsDeleted = 1
			Else
				iIsDeleted = 0
			End If
			
			' Set it
			m_vDataArray(ACArrayIsDeleted, iIndex) = iIsDeleted
			
			' Make sure it's updated
			m_bDataChanged = True
			
			' Refresh the list
			m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: InterfaceToData
	'
	' Description: Save's all the data to the business
	'
	' History: 24/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = g_oBusiness.UpdateDetails(v_vArray:=m_vDataArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToData Failed on UpdateDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: DisplayCaptions
	'
	' Description: Get's captions from the resource file and sets them
	'              on the form.
	'
	' History: 25/05/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Store the value for Delete button

			m_sDeleteCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Store the value for Undelete button

			m_sUnDeleteCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACUnDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMain, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
