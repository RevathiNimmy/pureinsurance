Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms

'Developer Guide No: 129
Imports SharedFiles

Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmInterface
	' Description   : Main interface.
	' Date          : 27/09/2000
	' Author        : DG
	' Edit History  :
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	Private Const ACColumn1 As Integer = 1
	Private Const ACColumn2 As Integer = 2
	Private Const ACColumn3 As Integer = 3
	Private Const ACColumn4 As Integer = 4
	Private Const ACColumn5 As Integer = 5
	Private Const ACColumn6 As Integer = 6
	Private Const ACColumn7 As Integer = 7
	
	'Constants for Defining Width of Columns in List View
	
	Private Const ColWidthBroking As Integer = 1700
	Private Const ACColWidthUnderWriting As Integer = 1300
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' Variables for Define Feilds Screen
	Private m_lMode As Integer
	Private m_lTypeId As Integer
	Private m_sTypeName As String = ""
	Private m_bCaptionChanged As Boolean
	Private m_lPerilType As Integer
	Private m_lPerilTypeReserveType As Integer
	Private m_lReserveType As Integer
	Private m_sPerilTypeDescription As String = ""
	
	'This variable is used to hold the value of the Display Order
	'which is used while checking te maximum display order permitable.
	'The value is assigned to it while the form is loading & whenever
	'a new value is being added to the database
	Private m_vDisplayOrder As Object
	
	'These variable is use to store the orignal value of the
	'text box when a value is selected from the listview
	'Its used for allowing the user to modify the text box value
	'back to its old value.
	Private m_sCaptionTxtBx As String = ""
	Private m_vDispOrdTxtBx As Object
	
	Private m_bListViewClick As Boolean
	
	'Declare an instance of the general interface object.
	Private m_oGeneral As iCLMPerilReserveType.General
	
	'Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'flag set to
	'0 -if an item in the listview is NOT selected
	'ie. your trying to add a new item to the database
	'1 _if an item in the listview IS selected
	'ie. your trying to Modify or Delete an item from the database
	
	Private m_lModify As Integer
	
	'Variable for Underwriting/Broking
	Private m_lSiriusUnderWritingBroking As String = ""
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the GetResvTypCount method of business object.
	Public m_vSearchData As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vArray As Object
	'Stores the data from the GetPerilsForReserve method of the business object.
	Public m_vTotalArray( ,  ) As Object
	Public m_vReserveTypeArray( ,  ) As Object
	
	Private Const ACCommandButtonHeight As Integer = 330
	Private Const ACCommandButtonWidth As Integer = 1095
	Private Const ACCommandButtonTop As Integer = 540
	
	Private Const ACLabelHeight As Integer = 255
	Private Const ACLabelWidth As Integer = 1575
	Private Const ACLabelLeft As Integer = 480
	Private Const ACTopOfLabelRelatedToTextBox As Integer = 45
	
	Private Const ACTextBoxHeight As Integer = 285
	Private Const ACTextBoxWidth As Integer = 4335
	Private Const ACTextBoxTop As Integer = 540
	Private Const ACTextBoxLeft As Integer = 2520
	
	Private Const ACTextBoxNormalGap As Integer = 360
	Private Const ACFormBorder As Integer = 120
	

    'Developer Guide No: 7
    Private Const vbFormCode As Integer = 0
	
	
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
	
	Public Property PerilType() As Integer
		Get
			
			Return m_lPerilType
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPerilType = Value
			
		End Set
	End Property
	'Is being used to store the Peril Type Description
	Public Property PerilTypeDescription() As String
		Get
			
			Return m_sPerilTypeDescription
			
		End Get
		Set(ByVal Value As String)
			
			m_sPerilTypeDescription = Value
			
		End Set
	End Property
	
	
	Public Property ReserveType() As Integer
		Get
			
			Return m_lReserveType
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lReserveType = Value
			
		End Set
	End Property
	
	Public Property PerilTypeReserveType() As Integer
		Get
			
			Return m_lPerilTypeReserveType
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPerilTypeReserveType = Value
			
		End Set
	End Property
	
	
	Public Property TypeName() As String
		Get
			
			Return m_sTypeName
			
		End Get
		Set(ByVal Value As String)
			
			m_sTypeName = Value
			
		End Set
	End Property
	Public Property TypeId() As Integer
		Get
			
			'Return TypeId
			Return m_lTypeId
			
		End Get
		Set(ByVal Value As Integer)
			
			'Set Claim Number
			m_lTypeId = Value
			
		End Set
	End Property
	Public Property Mode() As Integer
		Get
			
			Return m_lMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lMode = Value
			
		End Set
	End Property
	
	' ***************************************************************** '
	' Name:         GetBusiness
	'
	' Description:  Retrieves the details from the business object.
	'
	' ***************************************************************** '
	
	Public Function GetBusiness() As Integer
		Dim result As Integer = 0
		Try 

            'Const ACColDispOrd As Integer = 4
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = CType(DataToInterfaceSumm(), gPMConstants.PMEReturnCode)
			
			Me.Refresh()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'After loading & populating all the listviews
			'set the default to the 1st tab
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			' Check the return values.
			Select Case (m_lReturn)
				Case gPMConstants.PMEReturnCode.PMTrue
					' Found search details.
				Case gPMConstants.PMEReturnCode.PMNotFound
					' No search details found.
				Case Else
					' Failed to get details.
					result = gPMConstants.PMEReturnCode.PMFalse
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
					Return result
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToInterface
	'
	' Description: Updates all interface details from the search data.
	'              storage.
	'
	' ***************************************************************** '
	
	Public Function DataToInterface(ByRef iLstVwNo As Integer) As Integer
		
		Dim result As Integer = 0

		'Try 
		'
		'Catch 
		'End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	' ***************************************************************** '
	' Name: DataToInterfaceSumm
	'
	' Description: Updates all interface details from the search data.
	'              storage.
	' ***************************************************************** '
	Public Function DataToInterfaceSumm() As Integer
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			m_lReturn = CType(GetAllReserveTypes(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			PopuateComboBox()
			
			m_lReturn = GetRsrvTypForPrlTyp()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			' Clear the search details.
			lvwPerilTypeReservetype.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vTotalArray) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)
				' Assign the details to the first column.
				' Column 1 Caption
				'DC270201 - was name with description
				'Set oListItem = lvwPerilTypeReservetype.ListItems.Add(, , _
				''                Trim$(m_vTotalArray(ACColPrlRsvrTypeName, lRow&)))
				
				oListItem = lvwPerilTypeReservetype.Items.Add(CStr(m_vTotalArray(ACColPrlRsvrTypeDescription, lRow)).Trim())
				
				oListItem.Tag = CStr(m_vTotalArray(ACColPrlRsvrTypeID, lRow))
				' Assign details to other the columns
				' Column 2 Description
				'DC270201 - was description now name
				'oListItem.SubItems(1) = Trim$(m_vTotalArray(ACColPrlRsvrTypeDescription, lRow&))
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vTotalArray(ACColPrlRsvrTypeName, lRow)).Trim()
				

				If CStr(m_vTotalArray(ACColPrlRsvrTypeIncludeInTotal, lRow)).Trim() = "" Or m_vTotalArray(ACColPrlRsvrTypeIncludeInTotal, lRow) Is DBNull.Value Then
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No"
				Else
					If CBool(CStr(m_vTotalArray(ACColPrlRsvrTypeIncludeInTotal, lRow)).Trim()) Then
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes"
					Else
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No"
					End If
				End If
				

				If CStr(m_vTotalArray(ACColPrlRsvrTypeMainReserve, lRow)).Trim() = "" Or m_vTotalArray(ACColPrlRsvrTypeIncludeInTotal, lRow) Is DBNull.Value Then
					ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "No"
				Else
					If CBool(CStr(m_vTotalArray(ACColPrlRsvrTypeMainReserve, lRow)).Trim()) Then
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Yes"
					Else
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "No"
					End If
				End If
			Next lRow
			' Enable the interface now that the search has completed.
			m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterfaceSumm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToProperties
	'
	' Description: Updates the property member from the search data
	'              storage.
	'
	' ***************************************************************** '
	Public Function DataToProperties() As Integer
		Dim result As Integer = 0
		Dim lSelectedItem As Integer
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			' Store the selected item's tag, so we can use this
			' as the index to the search data storage details.

			lSelectedItem = Convert.ToString(lvwPerilTypeReservetype.Items.Item(lvwPerilTypeReservetype.FocusedItem.Index).Tag)
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	' ***************************************************************** '
	' Name: PropertiesToInterface
	'
	' Description: Updates the interface details from the property
	'              members.
	'
	' ***************************************************************** '
	Private Function PropertiesToInterface() As Integer
		Dim result As Integer = 0
		Dim sMode As String = ""
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			Me.Text = Me.Text & PerilTypeDescription
			
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
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
			'Add the columns to the listview
			lvwPerilTypeReservetype.Columns.Insert(ACColumn1 - 1, "", 94)
			lvwPerilTypeReservetype.Columns.Insert(ACColumn2 - 1, "", 94)
			lvwPerilTypeReservetype.Columns.Insert(ACColumn3 - 1, "", 94)
			lvwPerilTypeReservetype.Columns.Insert(ACColumn4 - 1, "", 94)
			
			lvwPerilTypeReservetype.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(1600))
			lvwPerilTypeReservetype.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
			lvwPerilTypeReservetype.Columns.Item(ACColumn3 - 1).Width = CInt(VB6.TwipsToPixelsX(600))
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			' Update the interface details with the
			' property members.
			m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			
			cmdModify.Enabled = False
			cmdDelete.Enabled = False
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ClearInterface
	'
	' Description: Clears all of the interface details for a new
	'              search.
	'
	' ***************************************************************** '

	'Private Function ClearInterface() As Integer
		'
		'Dim result As Integer = 0
		'Dim iMsgResult As DialogResult
		'Dim sMessage, sTitle As String
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Check if the user still wishes to clear
			' the interface.
			'

			'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'

			'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'
			' Display the message.
			'iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			'
			' Check message result.
			'If iMsgResult = System.Windows.Forms.DialogResult.No Then
				' Don't continue with the clear.
				'Return result
			'End If
			'
			' Clear the interface details.
			'
			' Clear the search data array.
			'm_vSearchData = Nothing
			'
			' Clear the search list details.
			'lvwPerilTypeReservetype.Items.Clear()
			'
			' Clear the search status bar.
			'    stbstatus.SimpleText = ""
			'
			'
			'
			' Set to the first tab.
			'SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			'
			' Set focus to the search details.
			'   txtTypeName.SetFocus
			'
			' Set the default button.
			'    cmdFindNow.Default = True
			'
			'
			' Disable parts of the interface, so the
			' user can now only enter a new search
			'm_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 2)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			
			m_ctlTabFirstLast(ACControlStart, 0) = cmbReserveTypeName
			m_ctlTabFirstLast(ACControlEnd, 0) = cmbReserveTypeName
			
			'DC270201 - txtReserveTypeDescription no longer displayed
			'    Set m_ctlTabFirstLast(ACControlStart, 1) = txtReserveTypeDescription
			'    Set m_ctlTabFirstLast(ACControlEnd, 1) = txtReserveTypeDescription
			'    Set m_ctlTabFirstLast(ACControlStart, 2) = chkIncludeIntotal
			'    Set m_ctlTabFirstLast(ACControlEnd, 2) = chkIncludeIntotal
			
			m_ctlTabFirstLast(ACControlStart, 1) = chkIncludeIntotal
			m_ctlTabFirstLast(ACControlEnd, 1) = chkIncludeIntotal
			m_ctlTabFirstLast(ACControlStart, 2) = chkMainReserve
			m_ctlTabFirstLast(ACControlEnd, 2) = chkMainReserve
			
			Return result
		
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
			


            'Developer Guide No: 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            'Developer Guide No:243
            lblReserveTypeName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'DC270201 do not require label for description as not displayed
			'    lblReserveTypeDescription.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=AClblDesc, _
			''        iDataType:=PMResString)
			


            'Developer Guide No: 243
            lblIncludeIntotal.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No:243
            lblMainReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblMainReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            'Developer Guide No: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            'Developer Guide No: 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            'Developer Guide No:243
            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            'Developer Guide No: 243
            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            'Developer Guide No: 243
            cmdModify.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACModButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No: 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'DC270201 swap name & description headings around


            'Developer Guide No: 243
            lvwPerilTypeReservetype.Columns.Item(ACColumn1 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveDesc, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No: 243
            lvwPerilTypeReservetype.Columns.Item(ACColumn2 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No: 243
            lvwPerilTypeReservetype.Columns.Item(ACColumn3 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveIncludeInTotal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            'Developer Guide No: 243
            lvwPerilTypeReservetype.Columns.Item(ACColumn4 - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveMainReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisableInterface
	'
	' Description: Disables parts of the interface while a search is
	'              in progress.
	'
	' ***************************************************************** '
	Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cmdOK.Enabled = Not bDisable
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' ***************************************************************** '

	'Private Function ResizeInterface() As Integer
		'
		'Try 
			'
			'
			'Return gPMConstants.PMEReturnCode.PMTrue
		'
		'Catch 
			'
			'
			'
			'Return gPMConstants.PMEReturnCode.PMError
		'End Try
	'End Function
	' PRIVATE Methods (End)
	

	Private isInitializingComponent As Boolean
	Private Sub cmbReserveTypeName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbReserveTypeName.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		SetReserveTypeData()
	End Sub
	Private Sub SetReserveTypeData()
		
		cmdAdd.Enabled = (cmbReserveTypeName.SelectedIndex > -1)
		If cmbReserveTypeName.SelectedIndex <= -1 Then Exit Sub
		
		txtReserveTypeDescription.Text = CStr(m_vReserveTypeArray(ACColReserveTypeDescription, cmbReserveTypeName.SelectedIndex))
		If CBool(m_vReserveTypeArray(ACColIncludeInTotal, cmbReserveTypeName.SelectedIndex)) Or CStr(m_vReserveTypeArray(ACColIncludeInTotal, cmbReserveTypeName.SelectedIndex)) = "1" Then
			chkIncludeIntotal.CheckState = CheckState.Checked
		Else
			chkIncludeIntotal.CheckState = CheckState.Unchecked
		End If
		ReserveType = CInt(m_vReserveTypeArray(ACColReseverTypeID, cmbReserveTypeName.SelectedIndex))
		chkMainReserve.CheckState = CheckState.Unchecked
		SetButtons()
	End Sub
	Private Sub SetButtons()
		If lvwPerilTypeReservetype.FocusedItem Is Nothing Then
			cmdDelete.Enabled = False
			cmdModify.Enabled = False
		Else
			cmdDelete.Enabled = True
			cmdModify.Enabled = True
		End If
	End Sub
	
	Private Sub cmbReserveTypeName_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbReserveTypeName.SelectedIndexChanged
		SetReserveTypeData()
		cmdDelete.Enabled = False
		cmdModify.Enabled = False
	End Sub
	
	
	
	Private Function AddPerilTypeReserveType(ByVal v_iPerilId As Integer, ByVal v_iReserveId As Integer, ByVal v_bMainReserve As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim bExists As Boolean
		Dim PrlRsrvListItem As ListViewItem
		Dim sTitle, sMessage As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check if the Reserve type for the Peril type already exists

			m_lReturn = m_oBusiness.ChckRsrvTypExstInPrlRsrTyp(v_iReserveId, v_iPerilId, bExists)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' If the Reserve type for the Peril type already exists then give out an error
			If bExists Then
				' Get description from the resource file.

                'Developer Guide No:243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveTypeAlreadyExists, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				

                'Developer Guide No:243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidAction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			' If the Reserve type for the Peril type does not exists then add

			m_lReturn = m_oBusiness.EditAdd(lRow:=1, vPerilTypeReserveTypeId:=m_lPerilTypeReserveType, vReserveTypeId:=VB6.GetItemData(cmbReserveTypeName, cmbReserveTypeName.SelectedIndex), vPerilTypeId:=m_lPerilType, vMainReserve:=chkMainReserve.CheckState, vMode:=Mode)
			
			'    m_lReturn = m_oBusiness.EditAdd(lRow:=1, _
			'vPerilTypeReserveTypeId:=m_lPerilTypeReserveType, _
			'vReserveTypeId:=cmbReserveTypeName.ItemData(cmbReserveTypeName.ListIndex), _
			'vPerilTypeId:=m_lPerilType, _
			'vMode:=Mode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			' Update the addition

			m_lReturn = m_oBusiness.Update
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			' Clear the Collection

			m_oBusiness.ClearColl()
			
			' Populate list view

            'Developer Guide No. 285
            PrlRsrvListItem = lvwPerilTypeReservetype.Items.Add("")

			'DC270201 - swap name & description around
			'    PrlRsrvListItem.Text = m_vReserveTypeArray(ACColReseverTypeName, cmbReserveTypeName.ListIndex)
			'    PrlRsrvListItem.SubItems(1) = m_vReserveTypeArray(ACColReserveTypeDescription, cmbReserveTypeName.ListIndex)
			PrlRsrvListItem.Text = CStr(m_vReserveTypeArray(ACColReserveTypeDescription, cmbReserveTypeName.SelectedIndex))
			ListViewHelper.GetListViewSubItem(PrlRsrvListItem, 1).Text = CStr(m_vReserveTypeArray(ACColReseverTypeName, cmbReserveTypeName.SelectedIndex))
			
			If CBool(m_vReserveTypeArray(ACColIncludeInTotal, cmbReserveTypeName.SelectedIndex)) Then
				ListViewHelper.GetListViewSubItem(PrlRsrvListItem, 2).Text = "Yes"
			Else
				ListViewHelper.GetListViewSubItem(PrlRsrvListItem, 2).Text = "No"
			End If
			
			If chkMainReserve.CheckState = CheckState.Unchecked Then
				ListViewHelper.GetListViewSubItem(PrlRsrvListItem, 3).Text = "No"
			Else
				ListViewHelper.GetListViewSubItem(PrlRsrvListItem, 3).Text = "Yes"
			End If

			PrlRsrvListItem.Tag = m_oBusiness.PerilTypeReserveType
			cmdAdd.Enabled = False
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPerilTypeReserveType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		Dim bCanDelete As Boolean
		Dim sTitle, sMessage As String
		Try 
			If lvwPerilTypeReservetype.FocusedItem Is Nothing Then Exit Sub

			m_lReturn = m_oBusiness.ChckDelForPrlRskTyp(PerilType, VB6.GetItemData(cmbReserveTypeName, cmbReserveTypeName.SelectedIndex), bCanDelete)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			If Not bCanDelete Then
				' Get description from the resource file.

                'Developer Guide No:243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCannotDeleteReserveType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				

                'Developer Guide No:243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidAction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				Exit Sub
			End If


			m_oBusiness.PerilTypeReserveType = Convert.ToString(lvwPerilTypeReservetype.FocusedItem.Tag)

			m_lReturn = m_oBusiness.EditDelete(1, Convert.ToString(lvwPerilTypeReservetype.FocusedItem.Tag), Mode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If

			m_oBusiness.Update()
			lvwPerilTypeReservetype.Items.RemoveAt(lvwPerilTypeReservetype.FocusedItem.Index)
			cmdDelete.Enabled = False
			cmdModify.Enabled = False
			cmdAdd.Enabled = cmbReserveTypeName.SelectedIndex > -1
		
		Catch excep As System.Exception
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDelete click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Exit Sub
			
		End Try
	End Sub
	
	
	Private Sub cmdModify_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdModify.Click
		Try 
			
			If lvwPerilTypeReservetype.FocusedItem Is Nothing Then Exit Sub


			m_oBusiness.PerilTypeReserveType = Convert.ToString(lvwPerilTypeReservetype.FocusedItem.Tag)
			'm_oBusiness.MainReserve = chkMainReserve.Value
			'Optional vPerilTypeReserveTypeId As Variant, _
			'Optional vReserveTypeId As Variant, _
			'Optional vPerilTypeId As Variant, _
			'Optional bMainReserve As Variant, _
			'Optional vMode As Variant
			

			m_lReturn = m_oBusiness.EditUpdate(lRow:=1, vPerilTypeReserveTypeId:=Convert.ToString(lvwPerilTypeReservetype.FocusedItem.Tag), vReserveTypeId:=VB6.GetItemData(cmbReserveTypeName, cmbReserveTypeName.SelectedIndex), vPerilTypeId:=PerilType, vMainReserve:=chkMainReserve.CheckState, vMode:=Mode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If

			m_oBusiness.Update()

			m_oBusiness.ClearColl()
			
            If chkMainReserve.CheckState = CheckState.Unchecked Then
                'Developer Guide No :52
                lvwPerilTypeReservetype.FocusedItem.SubItems(3).Text = "No"
            Else
                'Developer Guide No :52
                lvwPerilTypeReservetype.FocusedItem.SubItems(3).Text = "Yes"
            End If
			cmdDelete.Enabled = False
			cmdModify.Enabled = False
			cmdAdd.Enabled = False
		
		Catch excep As System.Exception
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdModify click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdModify_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: FormIntialise
	'
	' Description: Intialise all required details of the form
	'
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		Dim sTitle, sMessage As String
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMPerilReserveType.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                'Developer Guide no. :243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Developer Guide no. :243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMPerilReserveType.General()
			
			' Create an instance of the interface object.
			'    Set m_oInterface = New iCLMDefnFlds.Interface
			
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			
			' Set language
			m_oFormFields.LanguageID = g_iLanguageID
			
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
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
	' ***************************************************************** '
	' Name:         FormLoad
	' Description:  Loads all required details of the form
	' ***************************************************************** '

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
			
			' Validate fields using Forms Control
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			m_lModify = 0
			
			'Validate fields using Forms Control,
			'using FormFields object
			m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			
			
			' {* USER DEFINED CODE (End) *}
			
			'SINCE GetInterfaceDetails's GetBusiness method is no longer being called
			'from here, we can comment GetInterfaceDetails
			'GetBusiness isbeing called from Form Activate
			' Gets the interface details to be displayed.
			m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
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
            'developer guide no.(Resize the form accordingly)
            frmInterface_Resize(eventSender, eventArgs)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
		
		
	End Sub
	' ***************************************************************** '
	' Name: Form_Query Unload
	'
	' Description: Store all Property Details before unloading form
	' ***************************************************************** '
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
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
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
		m_oFormFields.Dispose()
			
			
			
			' Destroy the instance of the form control object
			' from memory.
			m_oFormFields = Nothing
			
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
	' ***************************************************************** '
	' Name:Form_KeyDown
	'
	' Description: Determine the Position of Tab and Control on
	'              pressing pageup,pagedown,home,end buttons
	' ***************************************************************** '
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
		
		Catch 
			

			Exit Sub
		End Try
		
		
	End Sub
	
	' ***************************************************************** '
	' Name:Form_Resize
	'
	' Description: Resize the the controls on form
	'
	' ***************************************************************** '
	
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			If WindowState = FormWindowState.Minimized Then Exit Sub
			
			
			If VB6.PixelsToTwipsX(Width) < 7560 Then Width = VB6.TwipsToPixelsX(7560)
			If VB6.PixelsToTwipsY(Height) < 4215 Then Height = VB6.TwipsToPixelsY(4215)
			
			
			
			tabMainTab.Left = VB6.TwipsToPixelsX(ACFormBorder)
			tabMainTab.Top = VB6.TwipsToPixelsY(ACFormBorder)
			tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(3 * ACFormBorder)
			tabMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - (6 * ACFormBorder) - ACCommandButtonHeight)
			
			cmdOK.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
			cmdOK.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
			
			cmdHelp.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
			cmdHelp.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
			
			cmdCancel.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
			cmdCancel.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
			
			cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(ACCommandButtonHeight + (ACFormBorder * 4) + 60)
			
			cmdHelp.Top = cmdCancel.Top
			cmdOK.Top = cmdCancel.Top
			
			cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(ACCommandButtonWidth + ACFormBorder * 2)
			cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(ACCommandButtonWidth + ACFormBorder)
			cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(ACCommandButtonWidth + ACFormBorder)
			
            cmbReserveTypeName.Top = VB6.TwipsToPixelsY(ACTextBoxTop)
			cmbReserveTypeName.Left = VB6.TwipsToPixelsX(ACTextBoxLeft)
			lblReserveTypeName.Left = VB6.TwipsToPixelsX(ACLabelLeft)
			lblReserveTypeName.Width = VB6.TwipsToPixelsX(ACLabelWidth)
            lblReserveTypeName.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblReserveTypeName.Top = cmbReserveTypeName.Top
			
			'DC270201 do not require as label and textbox not displayed
			'    txtReserveTypeDescription.Left = ACTextBoxLeft
			'    txtReserveTypeDescription.Top = cmbReserveTypeName.Top + ACTextBoxNormalGap
			'    lblReserveTypeDescription.Left = ACLabelLeft
			'    lblReserveTypeDescription.Width = ACLabelWidth
			'    lblReserveTypeDescription.Height = ACLabelHeight
			'    lblReserveTypeDescription.Top = txtReserveTypeDescription.Top + ACTopOfLabelRelatedToTextBox
			
			chkIncludeIntotal.Left = VB6.TwipsToPixelsX(ACTextBoxLeft)
			'DC270201 use code combo box not unused description textbox
			'    chkIncludeIntotal.Top = txtReserveTypeDescription.Top + ACTextBoxNormalGap
            chkIncludeIntotal.Top = cmbReserveTypeName.Top + VB6.TwipsToPixelsY(ACTextBoxNormalGap)
			
			lblIncludeIntotal.Left = VB6.TwipsToPixelsX(ACLabelLeft)
			lblIncludeIntotal.Width = VB6.TwipsToPixelsX(ACLabelWidth)
			lblIncludeIntotal.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblIncludeIntotal.Top = chkIncludeIntotal.Top + VB6.TwipsToPixelsY(ACTopOfLabelRelatedToTextBox)
			
			chkMainReserve.Left = VB6.TwipsToPixelsX(ACTextBoxLeft)
            chkMainReserve.Top = chkIncludeIntotal.Top + VB6.TwipsToPixelsY(ACTextBoxNormalGap)
			lblMainReserve.Left = VB6.TwipsToPixelsX(ACLabelLeft)
			lblMainReserve.Width = VB6.TwipsToPixelsX(ACLabelWidth)
			lblMainReserve.Height = VB6.TwipsToPixelsY(ACLabelHeight)
            lblMainReserve.Top = chkMainReserve.Top + VB6.TwipsToPixelsY(ACTopOfLabelRelatedToTextBox)
			
			cmdAdd.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
			cmdAdd.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
			cmdAdd.Top = VB6.TwipsToPixelsY(ACCommandButtonTop)
			cmdAdd.Left = tabMainTab.Width - VB6.TwipsToPixelsX(ACCommandButtonWidth + ACFormBorder)
			
			cmdModify.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
			cmdModify.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
			cmdModify.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdAdd.Top) + ACCommandButtonHeight + ACFormBorder)
			cmdModify.Left = tabMainTab.Width - VB6.TwipsToPixelsX(ACCommandButtonWidth + ACFormBorder)
			
			cmdDelete.Height = VB6.TwipsToPixelsY(ACCommandButtonHeight)
			cmdDelete.Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
			cmdDelete.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdModify.Top) + ACCommandButtonHeight + ACFormBorder)
			cmdDelete.Left = tabMainTab.Width - VB6.TwipsToPixelsX(ACCommandButtonWidth + ACFormBorder)
			
			'DC270201 add extra to cater for buttons
			'lvwPerilTypeReservetype.Top = chkMainReserve.Top + chkMainReserve.Height + ACFormBorder
			lvwPerilTypeReservetype.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(chkMainReserve.Top) + VB6.PixelsToTwipsY(chkMainReserve.Height) + ACFormBorder) + chkMainReserve.Height
			
			lvwPerilTypeReservetype.Left = VB6.TwipsToPixelsX(ACFormBorder)
			lvwPerilTypeReservetype.Width = tabMainTab.Width - VB6.TwipsToPixelsX(ACFormBorder * 2)
			lvwPerilTypeReservetype.Height = tabMainTab.Height - (lvwPerilTypeReservetype.Top + VB6.TwipsToPixelsY(ACFormBorder))

            'Developer Guide No 178
            'ListViewAutoSize(lvwPerilTypeReservetype, True, True)
            ListViewFunc.ListViewAutoSize(lvwPerilTypeReservetype, True, True)
		
		Catch 
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' ***************************************************************** '
	' Name          :lvwPerilTypeReservetype_Click
	' Description   :Fill the Claim Reference,Policy No.,Client Short Name
	'                   in Text Box for the listitem clicked
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Name: lvwPerilTypeReservetype_ColumnClick
	'
	' Description:Sort the Details of List View as per the column clicked
	'
	' ***************************************************************** '
	Private Sub lvwPerilTypeReservetype_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPerilTypeReservetype.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwPerilTypeReservetype.Columns(eventArgs.Column)
		Try 
			
			With lvwPerilTypeReservetype
				
				' If date column clicked, then sort by date sort column
				If ColumnHeader.Index + 1 - 1 = 4 Then
					ListViewHelper.SetSortedProperty(lvwPerilTypeReservetype, False)
					If ListViewHelper.GetSortKeyProperty(lvwPerilTypeReservetype) <> 5 Then
						ListViewHelper.SetSortKeyProperty(lvwPerilTypeReservetype, 5)
						ListViewHelper.SetSortOrderProperty(lvwPerilTypeReservetype, SortOrder.Ascending)
					Else
						ListViewHelper.SetSortOrderProperty(lvwPerilTypeReservetype, (ListViewHelper.GetSortOrderProperty(lvwPerilTypeReservetype) + 1) Mod 2)
					End If
					ListViewHelper.SetSortedProperty(lvwPerilTypeReservetype, True)
					
					' If current sort column header is
					' pressed.
				ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPerilTypeReservetype)) Then 
					' Set sort order opposite of
					' current direction.
					ListViewHelper.SetSortOrderProperty(lvwPerilTypeReservetype, (ListViewHelper.GetSortOrderProperty(lvwPerilTypeReservetype) + 1) Mod 2)
					ListViewHelper.SetSortedProperty(lvwPerilTypeReservetype, True)
				Else
					' Sort by this column (ascending).
					ListViewHelper.SetSortedProperty(lvwPerilTypeReservetype, False)
					
					' Turn off sorting so that the list
					' is not sorted twice
					ListViewHelper.SetSortOrderProperty(lvwPerilTypeReservetype, SortOrder.Ascending)
					ListViewHelper.SetSortKeyProperty(lvwPerilTypeReservetype, ColumnHeader.Index + 1 - 1)
					ListViewHelper.SetSortedProperty(lvwPerilTypeReservetype, True)
				End If
			End With
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPerilTypeReservetype_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: lvwPerilTypeReservetype_GotFocus
	'
	' Description:Set Ok Button a default
	' ***************************************************************** '
	Private Sub lvwPerilTypeReservetype_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPerilTypeReservetype.Enter
		' GotFocus Event for the search details
		
		Try 
			
			' Unset any default buttons so can select with Enter key.
			'    cmdFindNow.Default = False
			VB6.SetDefault(cmdOK, False)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPerilTypeReservetype_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
		
	End Sub
	
	Private Sub lvwPerilTypeReservetype_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwPerilTypeReservetype.KeyUp
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		If lvwPerilTypeReservetype.FocusedItem Is Nothing Then Exit Sub
		
		For lItem As Integer = 0 To cmbReserveTypeName.Items.Count - 1
			If lvwPerilTypeReservetype.FocusedItem.Text.Trim().ToUpper() = VB6.GetItemString(cmbReserveTypeName, lItem).Trim().ToUpper() Then
				cmbReserveTypeName.SelectedIndex = lItem
			End If
		Next lItem
		
        SetReserveTypeData()
        'Developer Guide No 52
        If lvwPerilTypeReservetype.FocusedItem.SubItems(3).Text = "No" Then
            chkMainReserve.CheckState = CheckState.Unchecked
        Else
            chkMainReserve.CheckState = CheckState.Checked
        End If

        cmdDelete.Enabled = True
        cmdModify.Enabled = True
        If cmbReserveTypeName.SelectedIndex > -1 Then
            cmdAdd.Enabled = lvwPerilTypeReservetype.FocusedItem Is Nothing
        Else
            cmdAdd.Enabled = False
        End If
		
		
	End Sub
	
	' ***************************************************************** '
	' Name:         lvwPerilTypeReservetype_LostFocus
	' Description:  Set find now as default
	' ***************************************************************** '
	Private Sub lvwPerilTypeReservetype_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPerilTypeReservetype.Leave
		' LostFocus Event for the search details
		
		Try 
			
			' Set the default button.
			VB6.SetDefault(cmdOK, True)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPerilTypeReservetype_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Exit Sub
		End Try
		
	End Sub
	
	Private Sub lvwPerilTypeReservetype_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPerilTypeReservetype.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000

        'Developer Guide No.: 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

		If lvwPerilTypeReservetype.GetItemAt(x, y) Is Nothing Then
			cmdDelete.Enabled = False
			cmdModify.Enabled = False
			cmdAdd.Enabled = cmbReserveTypeName.SelectedIndex > -1
			Exit Sub
		End If
		
		For lItem As Integer = 0 To cmbReserveTypeName.Items.Count - 1
			If lvwPerilTypeReservetype.GetItemAt(x, y).Text.Trim().ToUpper() = VB6.GetItemString(cmbReserveTypeName, lItem).Trim().ToUpper() Then
				cmbReserveTypeName.SelectedIndex = lItem
			End If
		Next lItem
		
		SetReserveTypeData()
		
		
		cmdDelete.Enabled = True
		cmdModify.Enabled = True
		If cmbReserveTypeName.SelectedIndex > -1 Then
			cmdAdd.Enabled = lvwPerilTypeReservetype.GetItemAt(x, y) Is Nothing
		Else
			cmdAdd.Enabled = False
		End If
	End Sub
	
	
	Private Sub lvwPerilTypeReservetype_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPerilTypeReservetype.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000

        'Developer Guide No.: 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'Developer Guide No 52
        If lvwPerilTypeReservetype.FocusedItem.SubItems(3).Text = "No" Then
            chkMainReserve.CheckState = CheckState.Unchecked
        Else
            chkMainReserve.CheckState = CheckState.Checked
        End If
	End Sub
	
	' ***************************************************************** '
	' Name: tabMainTab_Click
	'
	' Description:Set the Focus on the First control on the relevant Tab Clicked
	' ***************************************************************** '
	
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				' Set the default button.
				'        If (.Tab < cmdNext.Count) Then
				'            cmdNext(.Tab).Default = True
				'        Else
				'            cmdOK.Default = True
				'        End If
				
				' Now I know this is crap, this goes against
				' all my principles, but for some reason when
				' using the mouse to select a tab the setfocus
				' code below doesn't work. The cursor sticks,
				' and you can't tab off. Therefore I've used
				' this to get around the problem.
				'        DoEvents
				
				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
					m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
				End If
			End With
		
		Catch 
			
			
			
			
			
			tabMainTabPreviousTab = tabMainTab.SelectedIndex
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: cmdOK_Click
	'
	' Description:Set Properties of the form on clicking OK Button from the
	'               relevant list item under focus or clicked
	'
	' ***************************************************************** '
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			'     'force a lost focus event
			'    DoEvents
			'
			'     'Check mandatory controls have been entered into.
			'    m_lReturn = m_oFormFields.CheckMandatoryControls
			'
			'    ' Check for errors
			'    If m_lReturn <> PMTrue Then
			'      Exit Sub
			'    End If
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name: cmdCancel_Click
	'
	' Description:Unload the Form
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
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
	
	' ***************************************************************** '
	' Name: DisplayMessage
	'
	' Description: Display the Suitable Message
	' ***************************************************************** '

	'Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)
		'
		'Static sMessage As String = ""
		'
		'Try 
			'
			'

			'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			'
			'
			' Display the status message.
			'
			'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			'
			'    m_lReturn = m_oFormFields.AddNewFormField( _
			''                   ctlControl:=cboType, _
			''                   lFieldType:=PMCombo, _
			''                   lMandatory:=PMMandatory)
			'
			'    'Error checking
			'    If m_lReturn <> PMTrue Then
			'      SetFieldValidation = PMFalse
			'      Exit Function
			'    End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Private Function GetAllReserveTypes() As Integer
		Dim result As Integer = 0
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oBusiness.GetAllReserveTypes(m_vReserveTypeArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetAllReserveTypes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllReserveTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Private Sub PopuateComboBox()
		If Not Information.IsArray(m_vReserveTypeArray) Then Exit Sub
		cmbReserveTypeName.Items.Clear()
		For lRow As Integer = 0 To m_vReserveTypeArray.GetUpperBound(1)
			'DC270201 use description an not code in dropdown
			'cmbReserveTypeName.AddItem m_vReserveTypeArray(ACColReseverTypeName, lRow)
			Dim cmbReserveTypeName_NewIndex As Integer = -1
			cmbReserveTypeName_NewIndex = cmbReserveTypeName.Items.Add(CStr(m_vReserveTypeArray(ACColReserveTypeDescription, lRow)))
			VB6.SetItemData(cmbReserveTypeName, cmbReserveTypeName_NewIndex, CInt(m_vReserveTypeArray(ACColReseverTypeID, lRow)))
		Next lRow
	End Sub
	
	Private Function GetRsrvTypForPrlTyp() As gPMConstants.PMEReturnCode
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oBusiness.GetRsrvTypForPrlTyp(PerilType, m_vTotalArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetRsrvTypForPrlTyp", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRsrvTypForPrlTyp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Public Function CheckMainReserve() As Integer
		Dim result As Integer = 0
		Try 
            Dim vDataArray(,) As Object
			Dim sMessage, sTitle As String
			
			result = gPMConstants.PMEReturnCode.PMTrue

			m_lReturn = m_oBusiness.CheckMainReserve(PerilType, vDataArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vDataArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                If CInt(vDataArray(0, 0)) > 1 Then

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainReserveMoreThanOne, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidAction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
                    result = gPMConstants.PMEReturnCode.PMFalse
                ElseIf CInt(vDataArray(0, 0)) <= 0 Then

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainReserveNotSet, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidAction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMError
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckMainReserve", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMainReserve", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Try
            If cmbReserveTypeName.SelectedIndex < 0 Then Exit Sub
            AddPerilTypeReserveType(PerilType, VB6.GetItemData(cmbReserveTypeName, cmbReserveTypeName.SelectedIndex), chkMainReserve.CheckState)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAdd click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub


        End Try
    End Sub
End Class
