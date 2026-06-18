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
	' Date: 05/05/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' CJB 24/03/2005 PN14001 Changed lvwHeaderRates_Click and lvwHeaderRates_ColumnClick
	'                to not do any processing if listview is empty.
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lLookupHeaderId As Integer
	Private m_sRatesOrIndicators As String = ""
	Private m_sHeader As String = ""
	
	Private m_vLookupRates( ,  ) As Object
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iGISUserDefHeaderRates.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	'Private m_oBusiness As bGISUserDefHeaderRates.Business
	
	' Declare an instance of the edit interface.

	Private m_oLookupHeaderRate As iGISUserDefHeaderRate.Interface_Renamed
	'Private m_oLookupHeaderRate As iGISUserDefHeaderRate.Interface
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode

	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast(,) As Control
	Private m_sUniqueId As String
	Private m_sScreenHierarchy As String


	' Stores the details from the business object.

	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)


	' PUBLIC Property Procedures (Begin)

	Public Property LookupHeaderId() As Integer
		Get
			Return m_lLookupHeaderId
		End Get
		Set(ByVal Value As Integer)
			m_lLookupHeaderId = Value
		End Set
	End Property
	
	Public Property Header() As String
		Get
			Return m_sHeader
		End Get
		Set(ByVal Value As String)
			m_sHeader = Value
		End Set
	End Property
	
	Public Property RatesOrIndicators() As String
		Get
			Return m_sRatesOrIndicators
		End Get
		Set(ByVal Value As String)
			m_sRatesOrIndicators = Value
		End Set
	End Property
	
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
	
	
	Public Property StepStatus() As String
		Get
			
			Return m_sStepStatus
			
		End Get
		Set(ByVal Value As String)
			
			m_sStepStatus = Value
			
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

	Public Property UniqueId() As String
		Get

			Return m_sUniqueId

		End Get
		Set(ByVal Value As String)

			m_sUniqueId = Value

		End Set
	End Property

	Public Property ScreenHierarchy() As String
		Get

			Return m_sScreenHierarchy

		End Get
		Set(ByVal Value As String)

			m_sScreenHierarchy = Value

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
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
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
			

			m_oBusiness.LookupHeaderId = m_lLookupHeaderId

			m_oBusiness.RatesOrIndicators = m_sRatesOrIndicators
			

			m_lReturn = m_oBusiness.GetDetails(vLookupRates:=m_vLookupRates)
			
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
        Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
			
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
			
			txtHeader.Text = m_sHeader
			
			lvwHeaderRates.Items.Clear()
			
			If Not Information.IsArray(m_vLookupRates) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For iTemp As Integer = m_vLookupRates.GetLowerBound(1) To m_vLookupRates.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' Assign the details to the first column.
				' Column 1

                oListItem = lvwHeaderRates.Items.Add(CStr(m_vLookupRates(ACHCode, iTemp)), "Main")
				
				' Assign details to the other columns
				' Column 2
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLookupRates(ACHDescription, iTemp))
				
				If CDbl(m_vLookupRates(ACHIsDeleted, iTemp)) = 1 Then

                    ' developer guide no. 12
                    oListItem.ForeColor = Color.Gray
				End If
				
				' {* USER DEFINED CODE (End) *}
				' Set the tag property with the index of
				' the search data storage.
				
				oListItem.Tag = CStr(iTemp)
				
			Next iTemp
			lvwHeaderRates.Sorting = SortOrder.Ascending
			lvwHeaderRates.Sort()
			lvwHeaderRates.FullRowSelect = True

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
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If


			m_lReturn = m_oBusiness.Update(vLookupRates:=m_vLookupRates, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
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
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0

		Try 
			
			
			' Update the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			'
			' Example:-
			'
			'    m_DName$ = trim$(txtName.Text)
			'    m_DDate = CDate(txtDate.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			'Itemdata is made up of narrative id then code
			'    m_dtIPTDate = m_oFormFields.UnformatControl(txtDate)
			'    m_dRate = m_oFormFields.UnformatControl(txt)
			'    m_cAmount = m_oFormFields.UnformatControl(txtAmount)
			
			'Don't need to do anything, the array is kept up to date...
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
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
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwHeaderRates.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			cmdDelete.Enabled = False
			cmdEdit.Enabled = False
			
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 0)
			
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
			
			m_ctlTabFirstLast(ACControlStart, 0) = lvwHeaderRates
			m_ctlTabFirstLast(ACControlEnd, 0) = lvwHeaderRates
			
			' {* USER DEFINED CODE (End) *}
			
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
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwHeaderRates.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionHeader1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwHeaderRates.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionHeader2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblHeader.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHeader, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_sRatesOrIndicators = "I" Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim lTag As Integer
        Dim oListItem As ListViewItem

        Try

            '    m_lReturn = GetExistingCodes()

            'Create address component if not already done so
            If m_oLookupHeaderRate Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oLookupHeaderRate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLookupHeaderRate, sClassName:="iGISUserDefHeaderRate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLookupHeaderRate = temp_m_oLookupHeaderRate

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Lookup Header component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = CType(m_oLookupHeaderRate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oLookupHeaderRate.RatesOrIndicators = m_sRatesOrIndicators

            m_oLookupHeaderRate.LookupHeaderId = m_lLookupHeaderId

            m_oLookupHeaderRate.Header = m_sHeader

            m_oLookupHeaderRate.Code = ""

            m_oLookupHeaderRate.Description = ""


			m_lReturn = m_oLookupHeaderRate.Start()


			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oLookupHeaderRate.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            If Information.IsArray(m_vLookupRates) Then
                lTag = m_vLookupRates.GetUpperBound(1)
            Else
                lTag = -1
            End If

            lTag += 1

            If lTag = 0 Then
                ReDim m_vLookupRates(ACHEffectiveDate, lTag)
            Else
                ReDim Preserve m_vLookupRates(ACHEffectiveDate, lTag)
            End If

            m_vLookupRates(ACHLookupHeaderId, lTag) = m_lLookupHeaderId
            m_vLookupRates(ACHRecordCnt, lTag) = lTag + 1
            m_vLookupRates(ACHCaptionId, lTag) = 0

            m_vLookupRates(ACHCode, lTag) = m_oLookupHeaderRate.Code

            m_vLookupRates(ACHDescription, lTag) = m_oLookupHeaderRate.Description
            m_vLookupRates(ACHIsDeleted, lTag) = 0
			m_vLookupRates(ACHEffectiveDate, lTag) = DateTime.Today

			oListItem = lvwHeaderRates.Items.Add(m_oLookupHeaderRate.Code, "Main")


            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oLookupHeaderRate.Description

            oListItem.Tag = CStr(lTag)

            oListItem = Nothing

            lvwHeaderRates.Focus()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim lTag As Integer

        Try

			'Reset Interface
			'    cmdEdit.Enabled = False
			'    cmdDelete.Enabled = False

			' developer guide no. 12
			lvwHeaderRates.FocusedItem.ForeColor = IIf(lvwHeaderRates.FocusedItem.ForeColor.Equals(Color.Gray), Color.Black, Color.Gray)

            lTag = Convert.ToString(lvwHeaderRates.FocusedItem.Tag)

            m_vLookupRates(ACHIsDeleted, lTag) = 1 - CDbl(m_vLookupRates(ACHIsDeleted, lTag))
            'andym 07/07/2000

            'developer guide no. 12
            If lvwHeaderRates.FocusedItem.ForeColor.Equals(Color.Gray) Then

                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUndeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                cmdEdit.Enabled = False
            Else

                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                cmdEdit.Enabled = True
            End If
            'andym 07/07/2000
            lvwHeaderRates.Focus()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oListItem As ListViewItem
        Dim lTag As Integer

        Try

            '    m_lReturn = GetExistingCodes()

            'Create address component if not already done so
            If m_oLookupHeaderRate Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oLookupHeaderRate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLookupHeaderRate, sClassName:="iGISUserDefHeaderRate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLookupHeaderRate = temp_m_oLookupHeaderRate

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Lookup Header component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = CType(m_oLookupHeaderRate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            oListItem = lvwHeaderRates.FocusedItem


            lTag = Convert.ToString(oListItem.Tag)


            m_oLookupHeaderRate.RatesOrIndicators = m_sRatesOrIndicators

            m_oLookupHeaderRate.LookupHeaderId = m_vLookupRates(ACHLookupHeaderId, lTag)

            m_oLookupHeaderRate.Header = m_sHeader

            m_oLookupHeaderRate.Code = m_vLookupRates(ACHCode, lTag)

			m_oLookupHeaderRate.Description = m_vLookupRates(ACHDescription, lTag)

			m_lReturn = m_oLookupHeaderRate.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oLookupHeaderRate.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If


            m_vLookupRates(ACHCode, lTag) = m_oLookupHeaderRate.Code

			m_vLookupRates(ACHDescription, lTag) = m_oLookupHeaderRate.Description

			oListItem.Text = m_oLookupHeaderRate.Code

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oLookupHeaderRate.Description

            oListItem = Nothing

            lvwHeaderRates.Focus()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
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

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGISUserDefHeaderRates.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iGISUserDefHeaderRates.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

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

            ' Validate fields using Forms Control
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

            'developer guide no. 19
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

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

            If Not (m_oLookupHeaderRate Is Nothing) Then
                ' Terminate the LookupHeader object

                m_oLookupHeaderRate.Dispose()
                ' Destroy the instance of the LookupHeader object
                ' from memory.
                m_oLookupHeaderRate = Nothing
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
                    Case Keys.Escape
                        cmdCancel.Focus()
                        cmdCancel.PerformClick()
                End Select
            End With
            'developer guide no.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwHeaderRates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwHeaderRates.Click

        If lvwHeaderRates.Items.Count = 0 Then Exit Sub 'PN14001

        ' andym 07/07/2000

        'developer guide no. 12
        If lvwHeaderRates.FocusedItem.ForeColor.Equals(Color.Gray) Then

            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUndeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEdit.Enabled = False
        Else

            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEdit.Enabled = True
        End If
        ' andym 07/07/2000
    End Sub
	
	Private Sub lvwHeaderRates_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwHeaderRates.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwHeaderRates.Columns(eventArgs.Column)
		

		' Column click event for the campaigns
		
		Try 
			
			If lvwHeaderRates.Items.Count = 0 Then Exit Sub 'PN14001
            ListViewFunc.SortListView(lvwHeaderRates, eventArgs)
            'With lvwHeaderRates
            '	' If current sort column header is
            '	' pressed.

            '	'        If (.SortKey = 2) Then
            '	'            .SortKey = 1
            '	'        End If

            '	If ColumnHeader.Index + 1 - 1 = ACDateColumn Then

            '		If ListViewHelper.GetSortKeyProperty(lvwHeaderRates) <> ACDateColumn Then
            '			ListViewHelper.SetSortKeyProperty(lvwHeaderRates, ACDateColumn)
            '			iDirection = SortOrder.Ascending
            '		Else
            '			iDirection = (ListViewHelper.GetSortOrderProperty(lvwHeaderRates) + 1) Mod 2
            '		End If
            '                 'developer guide no. 170
            '                 m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwHeaderRates, v_iSourceColumn:=ACDateColumn, v_iDirection:=iDirection), gPMConstants.PMEReturnCode)
            '		' If current sort column header is
            '		' pressed.
            '	ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwHeaderRates)) Then 
            '		' Set sort order opposite of
            '		' current direction.
            '		ListViewHelper.SetSortOrderProperty(lvwHeaderRates, (ListViewHelper.GetSortOrderProperty(lvwHeaderRates) + 1) Mod 2)
            '	Else
            '		' Sort by this column (ascending).
            '		ListViewHelper.SetSortedProperty(lvwHeaderRates, False)

            '		' Turn off sorting so that the list
            '		' is not sorted twice
            '		ListViewHelper.SetSortOrderProperty(lvwHeaderRates, SortOrder.Ascending)
            '		ListViewHelper.SetSortKeyProperty(lvwHeaderRates, ColumnHeader.Index + 1 - 1)
            '		ListViewHelper.SetSortedProperty(lvwHeaderRates, True)
            '	End If
            'End With
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwHeaderRates_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwHeaderRates_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwHeaderRates.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
		
		If lvwHeaderRates.GetItemAt(x, y) Is Nothing Then
			' Nothing selected
			cmdEdit.Enabled = False
			cmdDelete.Enabled = False
		Else
			'Not if we're viewing...
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdEdit.Enabled = True
				cmdDelete.Enabled = True
			End If
		End If
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
			'
			'    With tabMainTab
			'        ' Set the default button.
			'        If (.Tab < cmdNext.Count) Then
			'            cmdNext(.Tab).Default = True
			'        Else
			'            cmdOK.Default = True
			'        End If
			''
			'        ' Now I know this is crap, this goes against
			'        ' all my principles, but for some reason when
			'        ' using the mouse to select a tab the setfocus
			'        ' code below doesn't work. The cursor sticks,
			'        ' and you can't tab off. Therefore I've used
			'        ' this to get around the problem.
			'        DoEvents
			''
			'        ' Set focus to the first control on the tab.
			'        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
			'            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
			'        End If
			'    End With
		'
		'Catch 
			'
			'
			'
			'
			'
			'tabMainTabPreviousTab = tabMainTab.SelectedIndex
		'End Try
		
	End Sub
	
	' PRIVATE Events (End)
End Class
