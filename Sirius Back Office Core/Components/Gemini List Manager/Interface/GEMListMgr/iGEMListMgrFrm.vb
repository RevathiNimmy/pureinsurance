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
	' Date: 10/02/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
    'developer guide no. 69
    Public m_ofrmMaintenance As frmMaintenance

    'developer guide no. 69
    Public m_ofrmStatus As frmStatus
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
    Private Const vbFormCode As Integer = 0
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private bIgnoreClick As Boolean
	Private m_bFastUpdate As Boolean
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iGEMListMgr.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	'Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	'List of items
	Private m_vListArray( ,  ) As Object
	
	'Object and Property Ids
	Private m_lObjectID As Integer
	Private m_lPropertyID As Integer
	
	'The number of list items
	Private m_lListItems As Integer
	
	'Flag to indicate we are building the lists
	Private m_bBuildingList As Boolean
	
	'User offset in list
	Private m_lUserOffset As Integer
	'Has the apply button been actioned
	Private m_bListsUpdated As Boolean
	
	Private Sub BuildListItems()
		
        'Flag building
		m_bBuildingList = True
		
		lstItems.Items.Clear()
		lstUser.Items.Clear()
		
		'Display custom items
		For lptr As Integer = 0 To m_lListItems
			
			If CDbl(m_vListArray(GEMListMgrConst.LSTType, lptr)) = GEMListMgrConst.LSTTypeCustom Then
				
				'Add to custom list
				Dim lstItems_NewIndex As Integer = -1
				lstItems_NewIndex = lstItems.Items.Add(CStr(m_vListArray(GEMListMgrConst.LSTText, lptr)))
				VB6.SetItemData(lstItems, lstItems_NewIndex, lptr)
				
				'   Show which items are deleted
				If CStr(m_vListArray(GEMListMgrConst.LSTCommand, lptr)) = GEMListMgrConst.LSTDeleted Then
					lstItems.SetItemChecked(lptr, False)
				Else
					lstItems.SetItemChecked(lptr, True)
				End If
				
			Else
				
				'Add to user list
				If (CDbl(m_vListArray(GEMListMgrConst.LSTType, lptr)) = GEMListMgrConst.LSTTypeUser) And (CStr(m_vListArray(GEMListMgrConst.LSTCommand, lptr)) <> GEMListMgrConst.LSTDeleted) Then
					
					Dim lstUser_NewIndex As Integer = -1
					lstUser_NewIndex = lstUser.Items.Add(CStr(m_vListArray(GEMListMgrConst.LSTText, lptr)))
					VB6.SetItemData(lstUser, lstUser_NewIndex, lptr)
				End If
				
			End If
			
		Next lptr
		
		'flag build complete
		m_bBuildingList = False
		
		'Deselect list item
		ListBoxHelper.SetSelectedIndex(lstItems, -1)
		ListBoxHelper.SetSelectedIndex(lstUser, -1)
		
	End Sub
	
	' ***************************************************************** '
	' Name: EditItem (Private)
	'
	' Description: Edit a selected item
	'
	' ***************************************************************** '
	Private Function EditItem() As Integer
		
		Dim result As Integer = 0
		Dim sCommand As String = ""
		Dim iMode As Integer
		Dim vCurrentItem(GEMListMgrConst.LSTMax) As Object
		Dim cListControl As ListBox
		Dim lListItem, lOffset As Integer
		Dim sText As String = ""

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			'Check what tab we are in Custom or user
			If SSTabHelper.GetSelectedIndex(tabMainTab) = 0 Then
				
				lOffset = 0
				cListControl = lstItems
			Else
				'User item
				
				lOffset = m_lUserOffset
				cListControl = lstUser
			End If
			
			'Make sure that an item has been selected
			If ListBoxHelper.GetSelectedIndex(cListControl) = -1 Then
				Return result
			End If
			
			lListItem = VB6.GetItemData(cListControl, ListBoxHelper.GetSelectedIndex(cListControl))
			
			'Set the current item

			vCurrentItem(GEMListMgrConst.LSTString) = m_vListArray(GEMListMgrConst.LSTString, lListItem)

			vCurrentItem(GEMListMgrConst.LSTFlags) = m_vListArray(GEMListMgrConst.LSTFlags, lListItem)

			vCurrentItem(GEMListMgrConst.LSTValueID) = m_vListArray(GEMListMgrConst.LSTValueID, lListItem)

			vCurrentItem(GEMListMgrConst.LSTPosID) = m_vListArray(GEMListMgrConst.LSTPosID, lListItem)

			vCurrentItem(GEMListMgrConst.LSTText) = m_vListArray(GEMListMgrConst.LSTText, lListItem)

			vCurrentItem(GEMListMgrConst.LSTABICode) = m_vListArray(GEMListMgrConst.LSTABICode, lListItem)

			vCurrentItem(GEMListMgrConst.LSTCommand) = m_vListArray(GEMListMgrConst.LSTCommand, lListItem)

			vCurrentItem(GEMListMgrConst.LSTType) = m_vListArray(GEMListMgrConst.LSTType, lListItem)
			
			'save the text
			sText = CStr(m_vListArray(GEMListMgrConst.LSTText, lListItem))
			
			'Initialise with cancel as default
			iMode = gPMConstants.PMEReturnCode.PMCancel
            'developer guide no. 69
            m_lReturn = m_ofrmMaintenance.Maintain(vCurrentItem, iMode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Check if item was edited
			If iMode = gPMConstants.PMEComponentAction.PMEdit Then
				
				'Enable the apply
				cmdOK.Enabled = True
				
				'If item has been edited, then update item details on custom table
				'Refresh list

				m_vListArray(GEMListMgrConst.LSTString, lListItem) = vCurrentItem(GEMListMgrConst.LSTString)

				m_vListArray(GEMListMgrConst.LSTFlags, lListItem) = vCurrentItem(GEMListMgrConst.LSTFlags)

				m_vListArray(GEMListMgrConst.LSTValueID, lListItem) = vCurrentItem(GEMListMgrConst.LSTValueID)

				m_vListArray(GEMListMgrConst.LSTPosID, lListItem) = vCurrentItem(GEMListMgrConst.LSTPosID)

				m_vListArray(GEMListMgrConst.LSTText, lListItem) = vCurrentItem(GEMListMgrConst.LSTText)

				m_vListArray(GEMListMgrConst.LSTABICode, lListItem) = vCurrentItem(GEMListMgrConst.LSTABICode)

				m_vListArray(GEMListMgrConst.LSTCommand, lListItem) = vCurrentItem(GEMListMgrConst.LSTCommand)
				m_vListArray(GEMListMgrConst.LSTChanged, lListItem) = True
				
				m_bFastUpdate = True
				'Fast list update
				If sText <> CStr(m_vListArray(GEMListMgrConst.LSTText, lListItem)) Then
					VB6.SetItemString(cListControl, ListBoxHelper.GetSelectedIndex(cListControl), m_vListArray(GEMListMgrConst.LSTText, lListItem))
				End If
				m_bFastUpdate = False
			End If
			
			Return result
		
		Catch 
			
			
			
			Return result
		End Try
	End Function
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
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
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the controls to
			' PMFormControl
			'
			' Example:-
			'
			'        ' Pass control and required settings to FormControl
			'        m_lReturn = m_oFormFields.AddNewFormField( _
			''                       ctlControl:=<Control Name>, _
			''                       lFieldType:=<PM field type>, _
			''                       lFormat:=<PM format string>, _
			''                       lMandatory:=<PMMandatory or PMNonMandatory)
			'
			'        'Error checking
			'        If m_lReturn <> PMTrue Then
			'          SetFieldValidation = PMFalse
			'          Exit Function
			'        End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'm_lReturn& = m_oBusiness.GetDetails()
			m_lReturn = gPMConstants.PMEReturnCode.PMTrue
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	'
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = BusinessToData()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign the all of the interface
			' details from the business object, using the FormatField
			' function for any type conversion.
			'
			' Example:-
			'
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToBusiness
	'
	' Description: Updates all business members from the interface
	'              details.
	'
	' ***************************************************************** '
	Public Function InterfaceToBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Set the object, property and list on business object
			

			m_oBusiness.ObjectID = glNPObjects.ItemId

			m_oBusiness.PropertyID = glNPProperties.ItemId

			m_oBusiness.ListDescription = glNPProperties.ItemDescription

			m_oBusiness.ObjectTable = glNPObjects.ItemDescription

			m_oBusiness.ListArray = VB6.CopyArray(m_vListArray)

			m_oBusiness.ListItems = m_lListItems
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayLookupDetails
	'
	' Description: Displays all of the lookup details using the lookup
	'              values/details.
	'
	' ***************************************************************** '
	Public Function DisplayLookupDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			'm_lReturn& = GetLookupValues()
			m_lReturn = gPMConstants.PMEReturnCode.PMTrue
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to retreive all of the lookup
			' descriptions for a given lookup type.
			' The GetLookupDetails function will allow you to do this.
			'
			' Example:-
			'
			'    m_lReturn& = GetLookupDetails( _
			''        sLookupTable:=PMLookupCodeName, _
			''        ctlLookup:=cmbCodeName)
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        DisplayLookupDetails = PMFalse
			'        Exit Function
			'    End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			

			m_lReturn = m_oBusiness.GetNext()
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Description: Updates the data storage from the interface details.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (InterfaceToData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function InterfaceToData() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'
			' Update the data storage.
			'
			' {* USER DEFINED CODE (Begin) *}
			'
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			''
			' Example:-
			''
			'    m_DName$ = trim$(txtName.Text)
			'    m_DDate = CDate(txtDate.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			''
			' NOTE: Replace this section with your new code.
			' ************************************************************
			'
			' {* USER DEFINED CODE (End) *}
			'
			'Return gPMConstants.PMEReturnCode.PMTrue
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
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
			iPMFunc.CenterForm(Me)
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			Select Case (m_lNavigate)
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = True
					
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = False
					
				Case Else
					cmdNavigate.Visible = False
			End Select
			
			m_lReturn = SetFirstLastControls()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFirstLastControls
	'
	' Description: Sets the first and last data entry controls for
	'              each tab to the control array, for use with the
	'              keyboard navigation.
	'
	' ***************************************************************** '
	Private Function SetFirstLastControls() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			'ReDim m_ctlTabFirstLast(1, )
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to set the first and last data entry
			' controls for all of the tabs.
			'
			' Example:-
			'
			'    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
			'    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupValues
	'
	' Description: Gets all of the lookup values, ready to be used by
	'              the lookup function.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLookupValues() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Gets all of the lookup values.
			'
			' Check the task.
			'Select Case (m_iTask)
				'Case gPMConstants.PMEComponentAction.PMAdd
					' Get all of the lookup values.

					'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					'
				'Case gPMConstants.PMEComponentAction.PMEdit
					' Get all of the lookup values with the correct
					' effective date.

					'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					'
				'Case gPMConstants.PMEComponentAction.PMView
					' Get lookup values for viewing only.

					'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			'End Select
			'
			' Check for errors.
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Log Error.
				'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				'
				'Return result
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
		'
		'Dim result As Integer = 0
		'Dim lRow As Integer
		'Dim bFoundMatch As Boolean
		'
		' Lookup value contants.
		'Const ACValueTableName As Integer = 0
		'Const ACValueID As Integer = 1
		'Const ACValueStartPos As Integer = 2
		'Const ACValueNumber As Integer = 3
		'
		' Lookup detail contants.
		'Const ACDetailKey As Integer = 0
		'Const ACDetailDesc As Integer = 1
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Get the lookup values.
			'
			'bFoundMatch = False
			'
			'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
				' Check for a match of the table name.
				'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
					' Found a match
					'bFoundMatch = True
					'Exit For
				'End If
			'Next lRow
			'
			' Check if there has been a table match.
			'If Not bFoundMatch Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Log Error.
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
				'
				'Return result
			'End If
			'
			' Using the lookup values, populate the control with
			' the details from the lookup details array.
			'
			'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
				' Add the details to the control.

				'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


				'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
				'
				'SP150998 - compare long value not string
				' Check if this is the selected index.
				'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
					'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


						'ctlLookup.ListIndex = ctlLookup.NewIndex
					'End If
				'End If
				'
			'Next lCntr
			'
			' Check if the selected index is blank. If so,
			' we set the controls index to zero.
			'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

				'ctlLookup.ListIndex = 0
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	' PRIVATE Methods (End)
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

		'Make sure a user item has been selected
		If ListBoxHelper.GetSelectedIndex(lstUser) = -1 Then
			Exit Sub
		End If
		
		m_lReturn = MessageBox.Show("Are you certain that you wish to delete this item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2)
		
		If m_lReturn = System.Windows.Forms.DialogResult.No Then
			Exit Sub
		End If
		
		'Get the user list item position in array
		Dim lListItem As Integer = VB6.GetItemData(lstUser, ListBoxHelper.GetSelectedIndex(lstUser))
		
		
		'Set to deleted
		m_vListArray(GEMListMgrConst.LSTCommand, lListItem) = GEMListMgrConst.LSTDeleted
		m_vListArray(GEMListMgrConst.LSTChanged, lListItem) = True
		cmdOK.Enabled = True
		
		'remove the item
		lstUser.Items.RemoveAt(CShort(ListBoxHelper.GetSelectedIndex(lstUser)))
		
		ListBoxHelper.SetSelectedIndex(lstUser, -1)
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		g_bIgnoreActivate = True
		EditItem()
		
		
	End Sub
	
	Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
		
		Dim vCurrentItem(GEMListMgrConst.LSTMax) As Object
		Dim iMode As Integer
		
		Try 
			
			'A new list item is user type

			vCurrentItem(GEMListMgrConst.LSTType) = GEMListMgrConst.LSTTypeUser
			
			'Initialise with cancel as default
			iMode = gPMConstants.PMEReturnCode.PMCancel
            'developer guide no. 69
            m_lReturn = m_ofrmMaintenance.Maintain(vCurrentItem, iMode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'Check if item was added
			If iMode = gPMConstants.PMEComponentAction.PMEdit Then
				
				'Enable the apply
				cmdOK.Enabled = True
				
				'If item has been added, then add to the listitems

				vCurrentItem(GEMListMgrConst.LSTChanged) = True
				
				'Add one slot to array
				If Information.IsArray(m_vListArray) Then
					
					'Increment the number of items
					m_lListItems += 1
					
					ReDim Preserve m_vListArray(GEMListMgrConst.LSTMax, m_lListItems)
					
				Else
					
					m_lListItems = 0

					ReDim m_vListArray(GEMListMgrConst.LSTMax, m_lListItems)
					
				End If
				
				'Add the new item

				m_vListArray(GEMListMgrConst.LSTString, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTString)

				m_vListArray(GEMListMgrConst.LSTFlags, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTFlags)

				m_vListArray(GEMListMgrConst.LSTValueID, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTValueID)

				m_vListArray(GEMListMgrConst.LSTPosID, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTPosID)

				m_vListArray(GEMListMgrConst.LSTText, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTText)

				m_vListArray(GEMListMgrConst.LSTABICode, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTABICode)

				m_vListArray(GEMListMgrConst.LSTCommand, m_lListItems) = vCurrentItem(GEMListMgrConst.LSTCommand)
				m_vListArray(GEMListMgrConst.LSTChanged, m_lListItems) = True
				m_vListArray(GEMListMgrConst.LSTType, m_lListItems) = GEMListMgrConst.LSTTypeUser
				
				'Add the new item to the list
				Dim lstUser_NewIndex As Integer = -1
				lstUser_NewIndex = lstUser.Items.Add(CStr(m_vListArray(GEMListMgrConst.LSTText, m_lListItems)))
				VB6.SetItemData(lstUser, lstUser_NewIndex, m_lListItems)
				
				'Set to new item
				ListBoxHelper.SetSelectedIndex(lstUser, m_lListItems - m_lUserOffset)
				
			End If
		
		Catch 
		End Try
		
		
		
		
	End Sub
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			'MN160799 - Now check the business type
			If g_bIgnoreActivate Then
				g_bIgnoreActivate = False
				Exit Sub
			End If

			If m_oBusiness.BusinessType = GemBusinessTypeMV Then
				
                lblDictionaryType.Text = "Marine Dictionary"
                'TO be cvered in Iteration 3
                'glNPObjects.Table = iGEMLookup.Lookup.voyTable.HKJnewpolarisobjects
                'glNPProperties.Table = iGEMLookup.Lookup.voyTable.HKJnewpolarisproperties
				
			Else
				
                lblDictionaryType.Text = "Commercial Dictionary"
                'TO be cvered in Iteration 3
                'glNPObjects.Table = iGEMLookup.Lookup.voyTable.newpolarisobjects
                'glNPProperties.Table = iGEMLookup.Lookup.voyTable.newpolarisproperties
				
				
			End If
			
		End If
	End Sub
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		Dim sMessage, sTitle As String
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			m_bListsUpdated = False
			
			' Get an instance of the business object via
			' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGEMListMgr.Form", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

				sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iGEMListMgr.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Create an instance of the form control object.
			' Set m_oFormFields = New iPMFormControl.FormFields
			
			' Set language
			'm_oFormFields.LanguageID = g_iLanguageID%
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			
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
			
			'MN160799 - Find out if we can have the HKJ dictionary:
			
			
			' Set the process modes for the busines object.

			m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Exit Sub
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			' Disable the second tab for now
			SSTabHelper.SetTabEnabled(tabMainTab, 1, False)
			
			' Validate fields using Forms Control
			m_lReturn = SetFieldValidation()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			'set edit button to visible if reg setting allows edit

			If m_oBusiness.AllowEdit Then
				Me.cmdEdit.Enabled = True
			End If
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Gets the interface details to be displayed.
			m_lReturn = m_oGeneral.GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			'Get sub details
			glNPObjects_Click(glNPObjects, Nothing)
		
		Catch excep As System.Exception
			
			
			
			
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


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

			' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
			' from memory.
			m_oGeneral = Nothing
			
			' Terminate the business object

            m_oBusiness.Dispose()

			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
			' Terminate the form control object.
			'm_lReturn& = m_oFormFields.Terminate()
			
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        m_lErrorNumber& = PMFalse
			'    End If
			
			' Destroy the instance of the form control object
			' from memory.
			'Set m_oFormFields = Nothing
			
			' Reset the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
		
		Catch excep As System.Exception
			
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
	Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown As Integer
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMainTab
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.PageUp
						' Page Up key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the first tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Else
							' Check we are not on the
							' first tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
								' Display the previous tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
							End If
						End If
						
					Case Keys.PageDown
						' Page Down key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the last tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
						Else
							' Check we are not on the
							' last tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
								' Display the next tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
							End If
						End If
						
					Case Keys.Home
						' Home key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
						
					Case Keys.End
						' End key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
				End Select
			End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
    'To be covered in Iteraion 3
    'Private Sub glNPObjects_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles glNPObjects.Click
    Private Sub glNPObjects_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        glNPProperties.ObjectID = glNPObjects.ItemId

    End Sub

    'To be covered in Iteraion 3
    'Private Sub glNPProperties_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles glNPProperties.Click
    Private Sub glNPProperties_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        Dim lNumItems As Integer

        Try

            'Clear List Array
            cmdOK.Enabled = False
            m_vListArray = Nothing
            m_lListItems = 0

            'Get the polaris data

            m_lReturn = m_oBusiness.GetPolData(v_lObjectID:=glNPObjects.ItemId, v_lPropertyID:=glNPProperties.ItemId, r_vListArray:=m_vListArray, r_lNumItems:=lNumItems)


            'Check if we have a list, and get the maximum index
            If Information.IsArray(m_vListArray) Then
                m_lListItems = m_vListArray.GetUpperBound(1)
            Else
                Exit Sub
            End If

            'Get the user offset (1st user in list)
            m_lUserOffset = m_vListArray.GetUpperBound(1) + 1
            For lptr As Integer = m_vListArray.GetLowerBound(1) To m_vListArray.GetUpperBound(1)

                If CDbl(m_vListArray(GEMListMgrConst.LSTType, lptr)) = GEMListMgrConst.LSTTypeUser Then

                    m_lUserOffset = lptr
                    Exit For

                End If

            Next lptr

            'Build the list of items into the listbox
            BuildListItems()

        Catch
        End Try




    End Sub
	
	
	
	Private Sub lstItems_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstItems.SelectedIndexChanged
		
		Exit Sub
		
        Dim sOriginal As String = ""
        Dim lReturn As Integer = Nothing
		
		
		'If we are building then do not execute
		If m_bBuildingList Then
			Exit Sub
		End If
		
		'lptr = Item

        Dim lptr As Integer = Nothing
        lptr = ListBoxHelper.GetSelectedIndex(lstItems)


		'Change deleted state
		If lptr <> -1 Then
			
			If lstItems.GetItemChecked(lptr) Then
				m_vListArray(GEMListMgrConst.LSTCommand, lptr) = ""
			Else
				'(IB)020999 - added warning and finished code for reverting back to old description

				lReturn = m_oBusiness.OriginalDescription(glNPProperties.ItemId, m_vListArray(GEMListMgrConst.LSTABICode, lptr), sOriginal)
				If sOriginal = VB6.GetItemString(lstItems, lptr) Then
					m_vListArray(GEMListMgrConst.LSTCommand, lptr) = GEMListMgrConst.LSTDeleted
					m_vListArray(GEMListMgrConst.LSTText, lptr) = sOriginal
				Else
					If MessageBox.Show("Removing '" & CStr(m_vListArray(GEMListMgrConst.LSTText, lptr)) & "' from the list will result in it reverting back to its standard Polaris description of '" & sOriginal & "'" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Are you sure you wish to do this?", Application.ProductName, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
						m_vListArray(GEMListMgrConst.LSTCommand, lptr) = GEMListMgrConst.LSTDeleted
						m_vListArray(GEMListMgrConst.LSTText, lptr) = sOriginal
						VB6.SetItemString(lstItems, lptr, sOriginal)
					Else
						lstItems.SetItemChecked(lptr, True)
					End If
				End If
			End If
			
			'Data has changed
			cmdOK.Enabled = True
			m_vListArray(GEMListMgrConst.LSTChanged, lptr) = True
			
		End If
		
		
	End Sub
	

	Private Sub lstItems_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lstItems.ItemCheck
		Dim sOriginal As String = ""
		Dim lReturn As Integer
		
		
		If m_bFastUpdate Then
			Exit Sub
		End If
		
		'If we are building then do not execute
		If m_bBuildingList Then
			Exit Sub
		End If
		
		Dim lptr As Integer = eventArgs.Index
		'lptr = lstItems.ListIndex
		
		'Change deleted state
		If lptr <> -1 Then
			
			If lstItems.GetItemChecked(lptr) Then
				m_vListArray(GEMListMgrConst.LSTCommand, lptr) = ""
			Else
				'(IB)020999 - added warning and finished code for reverting back to old description

				lReturn = m_oBusiness.OriginalDescription(glNPProperties.ItemId, m_vListArray(GEMListMgrConst.LSTABICode, lptr), sOriginal)
				If sOriginal = VB6.GetItemString(lstItems, lptr) Then
					m_vListArray(GEMListMgrConst.LSTCommand, lptr) = GEMListMgrConst.LSTDeleted
					m_vListArray(GEMListMgrConst.LSTText, lptr) = sOriginal
				Else
					If MessageBox.Show("Removing '" & CStr(m_vListArray(GEMListMgrConst.LSTText, lptr)) & "' from the list will result in it reverting back to its standard Polaris description of '" & sOriginal & "'" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Are you sure you wish to do this?", Application.ProductName, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
						m_vListArray(GEMListMgrConst.LSTCommand, lptr) = GEMListMgrConst.LSTDeleted
						m_vListArray(GEMListMgrConst.LSTText, lptr) = sOriginal
						VB6.SetItemString(lstItems, lptr, sOriginal)
					Else
						lstItems.SetItemChecked(lptr, True)
					End If
				End If
			End If
			
			'Data has changed
			cmdOK.Enabled = True
			m_vListArray(GEMListMgrConst.LSTChanged, lptr) = True
			
		End If
		
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				' Set the default button.
				'If (.Tab < cmdNext.Count) Then
				'    cmdNext(.Tab).Default = True
				'Else
				'    cmdOK.Default = True
				'End If
				
				' Now I know this is crap, this goes against
				' all my principles, but for some reason when
				' using the mouse to select a tab the setfocus
				' code below doesn't work. The cursor sticks,
				' and you can't tab off. Therefore I've used
				' this to get around the problem.
				Application.DoEvents()
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
					m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
				End If
			End With
		
		Catch 
			
			
			
			
			
			tabMainTabPreviousTab = tabMainTab.SelectedIndex
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Check mandatory controls have been entered into.
			'm_lReturn = m_oFormFields.CheckMandatoryControls()
			
			' Check for errors
			'    If m_lReturn <> PMTrue Then
			'      Exit Sub
			'    End If
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = m_oGeneral.ProcessCommand()
			
			Dim sSql As String = ""
			

			sSql = m_oBusiness.cSql
			
			' Check the return value.
			'If (m_lReturn& = PMTrue) Then
			'    ' Everything OK, so we can hide the interface.
			'    Me.Hide
			'End If
			' Changes have been applied set set indicator to false
			For i As Integer = 0 To m_vListArray.GetUpperBound(1)
				m_vListArray(GEMListMgrConst.LSTChanged, i) = False
			Next i
			'Disable apply
			cmdOK.Enabled = False
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			m_bListsUpdated = True
		
		Catch excep As System.Exception
			
			
			
			
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
			m_lReturn = m_oGeneral.ProcessCommand()
			
			
			' If changes have been made update the server RLDF
            If m_bListsUpdated Then
                'developer guide no. 69
                m_ofrmStatus.Business = m_oBusiness
                'developer guide no. 69
                m_ofrmStatus.ShowDialog()
                'developer guide no. 69
                m_ofrmStatus.Close()
            End If
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Navigate button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = m_oGeneral.ProcessCommand()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdNext_Click(ByRef Index As Integer)
		'
		'Try 
			'
			' Change to the next tab.
			'If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
				'SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
			'End If
			'
			' Set focus to the first control on the tab.
			'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
				'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
			'End If
		'
		'Catch 
			'
			'
			'
			'
			'Exit Sub
		'End Try
		'
		'
	'End Sub
	' PRIVATE Events (End)
End Class
