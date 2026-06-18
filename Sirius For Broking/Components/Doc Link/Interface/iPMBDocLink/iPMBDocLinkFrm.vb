Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Modified by Sumeet Singh on 5/10/2010 1:06:28 PM refer developer guide no. 129
Imports SharedFiles
'Modified by Sumeet Singh on 5/10/2010 2:53:50 PM refer developer guide no. 69
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date:  01/02/2001
	'
	' Created By: Ajit Kumar
	'
	' Description: Main interface.
	'
	' Edit History:
	'   26/06/2002 SJP  - Merged from Carole Nash into Broking
	' RAM20040225       - Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iPMBDocLink.General

	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	
	' StepStatus
	Private m_sStepStatus As String = ""
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bSuppressErrors As Boolean 'have to use this as m_lReturn gets trashed by the call stack
	
	' List of Document Links
	Private m_vDocLink( ,  ) As Object
	
	'List Of GISSchemes
	Private m_vGISScheme( ,  ) As Object
	
	'List of Processes
	Private m_vProcess As Object
	
	'DC040702
	'List of Agents
	Private m_vAgent As Object
	
	'List of Document types
	Private m_vDocType As Object
	
	'List of Document Templates
	Private m_vDocTemp As Object
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	'CLG RFC71 2004-09-01
	Private m_lSpoolAndArchiveMode As Integer
	
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	'                 Declared the following constants - START
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	Private Const ACSpool As String = "Spool"
	Private Const ACPrint As String = "Print"
	Private Const ACScheme As String = "Scheme:      "
	Private Const ACProcess As String = "Process:      "
	Private Const ACDocument As String = "Document:  "
	Private Const ACAutoArchiveTrue As String = "True"
	Private Const ACAutoArchiveFalse As String = "False"
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	' RAM20040225   : END
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	' PUBLIC Property Procedures (Begin)
	Public ReadOnly Property SuppressErrors() As Boolean
		Get
			Return m_bSuppressErrors
		End Get
	End Property
	
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public Property StepStatus() As String
		Get
			Return m_sStepStatus
		End Get
		Set(ByVal Value As String)
			m_sStepStatus = Value
		End Set
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
	' Edit History  :
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
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
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboGISScheme, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
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


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: RefreshList
	'
	' Description:
	'
	' History: 01/02/2002 AK - Created.
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	Private Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lstItem As ListViewItem
		Dim sKey, sText As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the list
			lvwDocLink.Items.Clear()
			
			' If we don't have anything then exit
			If Not Information.IsArray(m_vDocLink) Then
				Return result
			End If
			
			For iLoop1 As Integer = 0 To m_vDocLink.GetUpperBound(1)
				If CStr(m_vDocLink(ACArrayGISSchemeDesc, iLoop1)).Trim() = cboGISScheme.Text.Trim() And CStr(m_vDocLink(ACArrayDocSchemeVer, iLoop1)).Trim() = cboSchemeVersion.Text.Trim() And CDbl(m_vDocLink(ACArrayIsDeleted, iLoop1)) = 0 Then
					
					' Generate a key
					sKey = "R" & iLoop1
					' Get the text to use
					sText = CStr(m_vDocLink(ACArrayProcessDesc, iLoop1))
					
					' Add the item to the list view
					' Process Description
					lstItem = lvwDocLink.Items.Add(sKey, sText, "")
					
					' Document Type
					ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vDocLink(ACArrayDocTypeDesc, iLoop1)).Trim()
					
					' Document Template
					ListViewHelper.GetListViewSubItem(lstItem, 2).Text = CStr(m_vDocLink(ACArrayDocTempDesc, iLoop1)).Trim()
					
					' Agent
					ListViewHelper.GetListViewSubItem(lstItem, 3).Text = CStr(m_vDocLink(ACArrayAgentDesc, iLoop1)).Trim()
					
					' Spool Document
					If Conversion.Val(CStr(m_vDocLink(ACArraySpoolDocument, iLoop1))) = 1 Then
						ListViewHelper.GetListViewSubItem(lstItem, 4).Text = ACSpool
					Else
						ListViewHelper.GetListViewSubItem(lstItem, 4).Text = ACPrint
					End If
					
					' Auto Archive Document
					If Conversion.Val(CStr(m_vDocLink(ACArrayAutoArchiveDocument, iLoop1))) = 1 Then
						ListViewHelper.GetListViewSubItem(lstItem, 5).Text = ACAutoArchiveTrue
					Else
						ListViewHelper.GetListViewSubItem(lstItem, 5).Text = ACAutoArchiveFalse
					End If
					
				End If
			Next iLoop1
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	'DC190702 -start
	' ***************************************************************** '
	'
	' Name: Update Flag For Spooling Of Documents
	'
	' Description:
	'
	' History: 19/07/02 DC
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (UpdateFlag) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function UpdateFlag() As Integer
		'
		'Dim result As Integer = 0
		'Dim iLoop1 As Integer
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' If we don't have anything then exit
			'If Not Information.IsArray(m_vDocLink) Then
				'Return result
			'End If
			'
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFlag Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFlag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result


			'Return result
		'End Try
	'End Function
	'DC190702 -end
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		
		Dim result As Integer = 0
		Dim sPrevScheme As String = "" ' RAM20040225   : Declared this variable
		Dim sCurrentScheme As String = "" ' RAM20040225   : Declared this variable

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
			sCurrentScheme = ""
			sPrevScheme = ""
			If Information.IsArray(m_vGISScheme) Then
				cboGISScheme.Items.Clear()
				For iNum As Integer = 0 To m_vGISScheme.GetUpperBound(1)
					' Show only one entry for each scheme description in the combo box
					sCurrentScheme = CStr(m_vGISScheme(ACArrayGISScheme, iNum)).Trim()
					If sCurrentScheme <> sPrevScheme Then
						Dim cboGISScheme_NewIndex As Integer = -1
						cboGISScheme_NewIndex = cboGISScheme.Items.Add(sCurrentScheme)
						VB6.SetItemData(cboGISScheme, cboGISScheme_NewIndex, CInt(m_vGISScheme(ACArrayGISSchemeID, iNum)))
					End If
					sPrevScheme = sCurrentScheme
				Next 
			End If
			
			'Default to the first entry in the list
			If cboGISScheme.Items.Count > 0 Then
				cboGISScheme.SelectedIndex = 0
			End If
			
			If cboSchemeVersion.Items.Count > 0 Then
				cboSchemeVersion.SelectedIndex = 0
			End If
			
			' Populate the Listview
			m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
			
			' select the first item
			If lvwDocLink.Items.Count > 1 Then
				lvwDocLink.FocusedItem = lvwDocLink.Items.Item(0)
			End If
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


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
			
			If Not Information.IsArray(m_vDocLink) Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the business object

			m_lReturn = m_oBusiness.UpdateDocLink(v_vDocLink:=m_vDocLink)
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
			
			
			'    ' Get the lookup values.
			'
			'    m_lReturn& = GetLookupValues()
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        DisplayLookupDetails = PMFalse
			'        Exit Function
			'    End If
			
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
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetGlobalSettings
	'
	' Description:
	'
	' History: 07/03/2001 CTAF - Created.
	'
	' ***************************************************************** '
	
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	' Edit History  :
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			' Get the exisiting Document Link records

			m_lReturn = m_oBusiness.GetDocLink(r_vDocLink:=m_vDocLink)
			If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Or m_lReturn = gPMConstants.PMEReturnCode.PMError Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve DocLink data from business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
			End If
			
			' Get the Risk Groups

			m_lReturn = m_oBusiness.GetGISScheme(r_vGISScheme:=m_vGISScheme)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'This is a fatal error so display a nice message then exit
				MessageBox.Show("There are no suitable schemes to link to", "Document Link Fatal Error")
				result = gPMConstants.PMEReturnCode.PMInvalidRequest
				m_bSuppressErrors = True
				Return result
			End If
			' {* USER DEFINED CODE (End) *}
			
			'get m_lSpoolAndArchiveMode
			Dim sValue As String = ""
			iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTAllowSpoolAndArchive, gPMConstants.SIRBCHHeadOffice, sValue)
			m_lSpoolAndArchiveMode = IIf(sValue = "1", 1, 0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


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
			
			' Dont do anything with the array
			
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


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
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


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
	' Edit History  :
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessEdit
    '
    ' Description:
    '
    ' History: 01/02/2002 AK - Created.
    ' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
    ' RAM20040225   : Code changes related to PN Issue 7408
    ' ***************************************************************** '
    Private Function ProcessEdit() As Integer

        Dim result As Integer = 0
        Dim oForm As frmDetails
        Dim lIndex As Integer
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   26/14/2002 SJP - exits if no items available.
            If lvwDocLink.Items.Count = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the index of the selected item
            sKey = lvwDocLink.FocusedItem.Name
            sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))
            lIndex = CInt(sKey)

            ' Get a new instance of the form
            oForm = New frmDetails()

            ' Load it

            'Modified by Sumeet Singh on 5/10/2010 1:32:41 PM refer developer guide no. 68
            'Load(oForm)

            ' Set the properties on it
            With oForm
                ' Set the properties
                'set this first as setting some properties fires click events
                .SpoolAndArchiveMode = m_lSpoolAndArchiveMode

                '.set_ActionType(gPMConstants.PMEComponentAction.PMEdit)
                .ActionType = gPMConstants.PMEComponentAction.PMEdit
                .GISScheme = CStr(m_vDocLink(ACArrayGISSchemeDesc, lIndex))
                .Process = CStr(m_vDocLink(ACArrayProcessDesc, lIndex))
                .Agent = CStr(m_vDocLink(ACArrayAgentDesc, lIndex))
                .DocTemplateType = CStr(m_vDocLink(ACArrayDocTypeDesc, lIndex))
                .DocTemplate = CStr(m_vDocLink(ACArrayDocTempDesc, lIndex))
                .DocTemplateID = CInt(m_vDocLink(ACArrayDocTemplateID, lIndex))
                .SchemeVer = CInt(cboSchemeVersion.Text)
                .SpoolDocuments = CInt(m_vDocLink(ACArraySpoolDocument, lIndex))
                .AutoArchiveDocument = CInt(m_vDocLink(ACArrayAutoArchiveDocument, lIndex))
                .DocTemplateTypeID = CInt(m_vDocLink(ACArraylDocumentTypeID, lIndex))
                .ProcessID = CInt(m_vDocLink(ACArraylProcessID, lIndex))
                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(m_vDocLink(ACArraylAgentID, lIndex)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    .AgentID = CInt(m_vDocLink(ACArraylAgentID, lIndex))
                End If
            End With

            m_lReturn = CType(CType(oForm, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Show the form
            VB6.ShowForm(oForm, FormShowConstants.Modal, Me)

            ' Grab the values back if they pressed OK
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then

                With oForm
                    m_vDocLink(ACArrayGISSchemeDesc, lIndex) = .GISScheme
                    m_vDocLink(ACArrayProcessDesc, lIndex) = .Process
                    m_vDocLink(ACArrayAgentDesc, lIndex) = .Agent '.AgentCode       ' Stored Procedure Uses code
                    m_vDocLink(ACArrayDocTemplateID, lIndex) = .DocTemplateID
                    m_vDocLink(ACArrayDocSchemeVer, lIndex) = cboSchemeVersion.Text
                    m_vDocLink(ACArrayDocTypeDesc, lIndex) = .DocTemplateType
                    m_vDocLink(ACArrayDocTempDesc, lIndex) = .DocTemplate
                    m_vDocLink(ACArraySpoolDocument, lIndex) = .SpoolDocuments
                    m_vDocLink(ACArrayAutoArchiveDocument, lIndex) = .AutoArchiveDocument
                    m_vDocLink(ACArraylGISSchemeID, lIndex) = VB6.GetItemData(cboGISScheme, cboGISScheme.SelectedIndex)
                    m_vDocLink(ACArraylDocumentTypeID, lIndex) = .DocTemplateTypeID
                    m_vDocLink(ACArraylProcessID, lIndex) = .ProcessID
                    If .AgentID > 0 Then
                        m_vDocLink(ACArraylAgentID, lIndex) = .AgentID
                    End If
                End With

                ' Refresh the list
                m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)

            End If

            ' Unload it
            oForm.Close()

            ' Remove it from memory
            oForm = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessDelete
    '
    ' Description:
    '
    ' History: 01/02/2002 AK - Created.
    ' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
    ' ***************************************************************** '
    Private Function ProcessDelete() As Integer

        'Dim oForm As frmSettings
        Dim result As Integer = 0
        Dim lIndex As Integer
        Dim sKey, sTemp As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   26/14/2002 SJP - exits if no items available.
            If lvwDocLink.Items.Count = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the index of the selected item
            sKey = lvwDocLink.FocusedItem.Name
            sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))
            lIndex = CInt(sKey)
            sTemp = ACScheme & cboGISScheme.Text & Strings.Chr(13) & Strings.Chr(10)
            sTemp = sTemp & ACProcess & lvwDocLink.FocusedItem.Text & Strings.Chr(13) & Strings.Chr(10)
            sTemp = sTemp & ACDocument & ListViewHelper.GetListViewSubItem(lvwDocLink.Items.Item(lvwDocLink.FocusedItem.Name), 2).Text
            sTemp = sTemp & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Remove the link to this document"

            If MessageBox.Show(sTemp, "Remove Link", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                m_vDocLink(ACArrayIsDeleted, lIndex) = 1

                ' Refresh the list
                m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessAdd
    '
    ' Description:
    '
    ' History: 01/02/2002 AK - Created.
    ' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
    ' ***************************************************************** '
    Private Function ProcessAdd() As Integer

        Dim result As Integer = 0
        'Dim oForm As frmDetails
        Dim oForm As frmDetails
        Dim lIndex As Integer
        Dim sKey As String = ""
        'AK 110602

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a new instance of the form
            oForm = New frmDetails()

            ' Load it

            'Modified by Sumeet Singh on 5/10/2010 1:32:59 PM refer developer guide no. 68
            'Load(oForm)

            ' Set the properties on it
            With oForm
                ' Set the properties
                'set this first as setting some properties fires click events
                .SpoolAndArchiveMode = m_lSpoolAndArchiveMode

                'Developer Guide no.24
                '.set_ActionType(gPMConstants.PMEComponentAction.PMAdd)
                .ActionType = gPMConstants.PMEComponentAction.PMAdd

                .GISScheme = cboGISScheme.Text.Trim()
                .SchemeVer = CInt(cboSchemeVersion.Text)
            End With

            m_lReturn = CType(CType(oForm, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Show the form
            VB6.ShowForm(oForm, FormShowConstants.Modal, Me)

            ' Grab the values back if they pressed OK
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                If Information.IsArray(m_vDocLink) Then
                    ReDim Preserve m_vDocLink(ACArrayDocLinkMaxCol, m_vDocLink.GetUpperBound(1) + 1)
                Else
                    ReDim m_vDocLink(ACArrayDocLinkMaxCol, 0)
                End If
                lIndex = m_vDocLink.GetUpperBound(1)

                With oForm
                    m_vDocLink(ACArrayGISSchemeDesc, lIndex) = cboGISScheme.Text.Trim()
                    m_vDocLink(ACArrayProcessDesc, lIndex) = .Process
                    m_vDocLink(ACArrayAgentDesc, lIndex) = .AgentCode
                    m_vDocLink(ACArrayDocTypeDesc, lIndex) = .DocTemplateType
                    m_vDocLink(ACArrayDocTempDesc, lIndex) = .DocTemplate
                    m_vDocLink(ACArrayIsDeleted, lIndex) = 0
                    m_vDocLink(ACArraySpoolDocument, lIndex) = .SpoolDocuments
                    m_vDocLink(ACArrayDocTemplateID, lIndex) = .DocTemplateID
                    m_vDocLink(ACArrayDocSchemeVer, lIndex) = CStr(.SchemeVer)
                    m_vDocLink(ACArraySpoolDocument, lIndex) = .SpoolDocuments
                    m_vDocLink(ACArrayAutoArchiveDocument, lIndex) = .AutoArchiveDocument
                    m_vDocLink(ACArraylGISSchemeID, lIndex) = VB6.GetItemData(cboGISScheme, cboGISScheme.SelectedIndex)
                    m_vDocLink(ACArraylDocumentTypeID, lIndex) = .DocTemplateTypeID
                    m_vDocLink(ACArraylProcessID, lIndex) = .ProcessID
                    If .AgentID > 0 Then
                        m_vDocLink(ACArraylAgentID, lIndex) = .AgentID
                    End If
                End With

                ' Refresh the list
                m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)

            End If

            ' Unload it
            oForm.Close()

            ' Remove it from memory
            oForm = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    Private Sub cboGISScheme_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISScheme.SelectedIndexChanged
        ' First clear the items in the document list
        lvwDocLink.Items.Clear()
        m_lReturn = PopulateSchemeVersionComboBox()
    End Sub

    Private Sub cboSchemeVersion_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSchemeVersion.SelectedIndexChanged
        m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = CType(ProcessAdd(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        m_lReturn = CType(ProcessDelete(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_lReturn = CType(ProcessEdit(), gPMConstants.PMEReturnCode)

    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMBDocLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            'm_oGeneral = New iPMBDocLink.General()
            m_oGeneral = New iPMBDocLink.General()
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
				m_lErrorNumber = m_lReturn
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Exit Sub
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}
			
			
			'AK 110602
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = m_lReturn
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Extra ListView Properties for lvwDocLink", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Exit Sub
			End If
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
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
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
                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    'Set the mouse pointer to normal.
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
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		Try 
			
			' RAM20040225       - Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
			m_lReturn = CType(ApplyChanges(), gPMConstants.PMEReturnCode)
			
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
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
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
	
	' ***************************************************************** '
	'
	' Name: SortListView
	'
	' Description:
	'
	' History: 13/03/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function SortListView(ByVal v_iIndex As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Tell it that it's not sorted
			ListViewHelper.SetSortedProperty(lvwDocLink, False)
			
			' Set the column to sort on
			ListViewHelper.SetSortKeyProperty(lvwDocLink, v_iIndex)
			
			' Swap the ascending/descending around
			If ListViewHelper.GetSortOrderProperty(lvwDocLink) = SortOrder.Ascending Then
				ListViewHelper.SetSortOrderProperty(lvwDocLink, SortOrder.Descending)
			Else
				ListViewHelper.SetSortOrderProperty(lvwDocLink, SortOrder.Ascending)
			End If
			
			' Tell it that it's now sorted
			ListViewHelper.SetSortedProperty(lvwDocLink, True)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SortListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SortListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub lvwDocLink_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwDocLink.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwDocLink.Columns(eventArgs.Column)
		
		' Sort the data
		m_lReturn = CType(SortListView(v_iIndex:=ColumnHeader.Index + 1 - 1), gPMConstants.PMEReturnCode)
		
	End Sub
	
	' PRIVATE Events (End)
	
	Private Sub lvwDocLink_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDocLink.DoubleClick
		
		' If they've double clicked on an item then simulate a call to Edit
		If Not (lvwDocLink.FocusedItem Is Nothing) Then
			
			cmdEdit_Click(cmdEdit, New EventArgs())
			
		End If
		
	End Sub
	
	
	' ***************************************************************** '
	' Name          : PopulateSchemeVersionComboBox
	' Description   : Function to populate SchemeVersion Combo Box
	' Edit History  :
	' RAM20040225   : Created
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	Private Function PopulateSchemeVersionComboBox() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' First clear the item
			cboSchemeVersion.Items.Clear()
			
			For iLoop As Integer = 0 To m_vGISScheme.GetUpperBound(1)
				If CStr(m_vGISScheme(ACArrayGISSchemeDesc, iLoop)).Trim() = cboGISScheme.Text.Trim() Then
					' Add version information
					cboSchemeVersion.Items.Add(CStr(m_vGISScheme(ACArrayGISScheme + 1, iLoop)))
				End If
			Next iLoop
			
			' Default to the first entry
			If cboSchemeVersion.Items.Count > 0 Then
				cboSchemeVersion.SelectedIndex = 0 ' first entry
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Scheme Version ComboBox", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateSchemeVersionComboBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result


			Return result
		End Try
	End Function
	
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
		
		Try 
			
			' Call the function to the job
			m_lReturn = ApplyChanges()
		
		Catch excep As System.Exception
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdApply_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub


		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name          : ApplyChanges
	' Description   : Function to do the process, when the Apply Button is clicked
	' Edit History  :
	' RAM20040225   : Created
	' RAM20040225   : Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	Private Function ApplyChanges() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check mandatory controls have been entered into.
			m_lReturn = m_oFormFields.CheckMandatoryControls()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = m_lReturn
				' Set the mouse pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = m_lReturn
				' Set the mouse pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyChanges failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' ***************************************************************** '
	Private Function ResizeInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If VB6.PixelsToTwipsX(Me.Width) < 9060 Then
				Me.Width = VB6.TwipsToPixelsX(9060)
				Return result
			End If
			
			If VB6.PixelsToTwipsY(Me.Height) < 5715 Then
				Me.Height = VB6.TwipsToPixelsY(5715)
				Return result
			End If
			
			tabMain.Top = VB6.TwipsToPixelsY(120)
			tabMain.Width = Me.Width - VB6.TwipsToPixelsX(285)
			tabMain.Height = Me.Height - VB6.TwipsToPixelsY(1020)
			
			lvwDocLink.Width = tabMain.Width - VB6.TwipsToPixelsX(240)
			lvwDocLink.Height = tabMain.Height - VB6.TwipsToPixelsY(1900)
			
			cmdAdd.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(lvwDocLink.Top) + VB6.PixelsToTwipsY(lvwDocLink.Height) + 120)
			cmdEdit.Top = cmdAdd.Top
			cmdDelete.Top = cmdAdd.Top
			
			cmdApply.Left = Me.Width - VB6.TwipsToPixelsX(1255)
			cmdApply.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(tabMain.Top) + VB6.PixelsToTwipsY(tabMain.Height) + 90)
			
			cmdCancel.Left = cmdApply.Left - VB6.TwipsToPixelsX(1195)
			cmdCancel.Top = cmdApply.Top
			
			cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(1195)
			cmdOK.Top = cmdApply.Top
			
			If cmdNavigate.Visible Then
				cmdNavigate.Top = cmdApply.Top
			End If
			
			Return result
		
		Catch 
			
			
			
			' Error Section.
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Class
