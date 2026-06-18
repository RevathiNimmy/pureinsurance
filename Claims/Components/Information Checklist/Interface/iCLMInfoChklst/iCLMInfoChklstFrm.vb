Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

'Developer Guide No.: 129
Imports SharedFiles

Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 04/08/2000
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	
	Private Const ACClass As String = "frmInterface"
	
	Private Const ACColumn1 As Integer = 1
	Private Const ACColumn2 As Integer = 2
	Private Const ACColumn3 As Integer = 3
	Private Const ACColumn4 As Integer = 4
	'DC240901
	Private Const ACColumn5 As Integer = 5
	Private Const ACColumn6 As Integer = 6
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lRiskTypeID As Integer
	Private m_lClaimID As Integer
	
	Private m_sClaimNumber As String = ""
	Private m_nFindClaimMode As Integer
	
	
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	'Index of the Selected Recovery Type
	Private m_lSelectedIndex As Integer
	
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_iServTypeId As Integer
	Private m_sService As String = ""
	Private m_lPrtyClmId As Integer
	Private m_sReference As String = ""
    Private m_vDateReq As Object
	Private m_vTimeReq As Object
    Private m_vDateCrit As Object
	Private m_vTimeCrit As Object
    Private m_vDateRecv As Object
	Private m_vTimeRecv As Object
	Private m_sContact As String = ""
	Private m_sDesc As String = ""
	'JMK 25/05/2001 InfoOnly flag
	Private m_bInfoOnly As Boolean

    'Declared in VB.Net
    Dim frmRequirement As frmRequirement


    'Developer Guide No.: 7
    Private Const vbFormCode As Integer = 0

    'Object Declaration in VB.Net
    Dim frmService As frmService


	'TN20010604 - set to true to save data and exit road map....(info only claim)
	Private m_bWorkInfoOnlyFlag As Boolean
	
	'TN20010605 - set to pmtrue to delete work table when cancelled
	Private m_lDeleteWorkTableFlag As Integer
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMInfoChklst.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Stores the return value for the a function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' variable to collect the list item of the list view
	Private lst_item As ListViewItem
	
	'Count For No of Objects in Collection
	Public m_lCount As Integer
	'For Event trap
	Private m_bEventInfo As Boolean
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	
	Public Property CallingAppName() As String
		Get
			
			' Return the calling application name.
			Return m_sCallingAppName
			
		End Get
		Set(ByVal Value As String)
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	'TN20010605 end
	

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
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public Property TransactionType() As String
		Get
			
			Return m_sTransactionType
			
		End Get
		Set(ByVal Value As String)
			
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	Public Property InfoOnly() As Boolean
		Get
			Return m_bInfoOnly
		End Get
		Set(ByVal Value As Boolean)
			m_bInfoOnly = Value
		End Set
	End Property
	Public Property EventInfo() As Boolean
		Get
			Return m_bEventInfo
		End Get
		Set(ByVal Value As Boolean)
			m_bEventInfo = Value
		End Set
	End Property
	
	'TN20010604 start
	Public Property WorkInfoOnlyFlag() As Boolean
		Get
			Return m_bWorkInfoOnlyFlag
		End Get
		Set(ByVal Value As Boolean)
			WorkInfoOnlyFlag = Value
		End Set
	End Property
	'TN20010604 end
	'TN20010605 start
	
	Public Property DeleteWorkTableFlag() As Integer
		Get
			Return m_lDeleteWorkTableFlag
		End Get
		Set(ByVal Value As Integer)
			m_lDeleteWorkTableFlag = Value
		End Set
	End Property
	Public Property ClaimNumber() As String
		Get
			
			Return m_sClaimNumber
			
		End Get
		Set(ByVal Value As String)
			
			m_sClaimNumber = Value
			
		End Set
	End Property
	Public Property ClaimMode() As Integer
		Get
			
			Return g_nPMMode
			
		End Get
		Set(ByVal Value As Integer)
			
			g_nPMMode = Value
			
		End Set
	End Property
	
	Public Property ServTypeId() As Integer
		Get
			Return m_iServTypeId
		End Get
		Set(ByVal Value As Integer)
			m_iServTypeId = Value
		End Set
	End Property
	
	
	Public Property ClaimId() As Integer
		Get
			Return m_lClaimID
		End Get
		Set(ByVal Value As Integer)
			m_lClaimID = Value
		End Set
	End Property
	
	
	Public Property RiskTypeId() As Integer
		Get
			Return m_lRiskTypeID
		End Get
		Set(ByVal Value As Integer)
			m_lRiskTypeID = Value
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness(ByRef sScope As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			If m_iTask = StringsHelper.ToDoubleSafe("PMAdd") Then

				m_lReturn = m_oBusiness.GetDetails(m_lRiskTypeID, sScope)
				'DC281100 added option for view mode
			ElseIf m_iTask = StringsHelper.ToDoubleSafe("PMEdit") Or m_iTask = StringsHelper.ToDoubleSafe("PMView") Then 

				m_lReturn = m_oBusiness.GetDetails(m_lClaimID, sScope)
			End If
			
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
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '

	'Private Function BusinessToData() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Assign the details to the data storage.
			'
			' {* USER DEFINED CODE (Begin) *}
			'

			'm_lReturn = m_oBusiness.GetNext()
			'
			' {* USER DEFINED CODE (End) *}
			'
			' Check for errors.
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Log Error.
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Description: Updates the data storage from the interface details.
	'
	' ***************************************************************** '

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
		Dim vResultArray As Object
		Try 
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			lvwInfoChklst.Columns.Insert(ACColumn1 - 1, "", 94)
			lvwInfoChklst.Columns.Insert(ACColumn2 - 1, "", 94)
			lvwInfoChklst.Columns.Insert(ACColumn3 - 1, "", 94)
			lvwInfoChklst.Columns.Insert(ACColumn4 - 1, "", 94)
			'DC240901
			lvwInfoChklst.Columns.Insert(ACColumn5 - 1, "", 94)
			' Alix - 11/02/2004 - PN10166
			lvwInfoChklst.Columns.Insert(ACColumn6 - 1, "", 94)
			
			lvwInfoChklst.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
			lvwInfoChklst.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
			lvwInfoChklst.Columns.Item(ACColumn3 - 1).Width = CInt(VB6.TwipsToPixelsX(2000))
			lvwInfoChklst.Columns.Item(ACColumn4 - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
			'DC240901
			lvwInfoChklst.Columns.Item(ACColumn5 - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
			' Alix - 11/02/2004 - PN10166
			lvwInfoChklst.Columns.Item(ACColumn6 - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
			
			' Display all language specific captions.
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			m_lReturn = gPMConstants.PMEReturnCode.PMTrue
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'DC281100 - prepare form
			m_lReturn = CType(PrepareForm(g_nPMMode), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Update the controls with properties of frmInterface.
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
			
			'RWH(06/03/2001)
            'Made full row select on list views
            'developer guide no.303
            lvwInfoChklst.FullRowSelect = True
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwInfoChklst.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            '' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
			
			' Set any other default values to the interface.
			' {* USER DEFINED CODE (Begin) *}


			vResultArray = DBNull.Value
			
			If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
				cmdAddRequirement.Enabled = True
				cmdAddService.Enabled = True
			End If
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'DC281100 added
	' ***************************************************************** '
	' Name:     PrepareForm (Private)
	'
	' Description:  Depending on the mode of operation enable & disable the
	'               controls
	' ***************************************************************** '
	Private Function PrepareForm(ByVal vMode As Integer) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				vMode = g_nREADMODE
			End If
			
			
			Select Case vMode
				Case g_nADDMODE, g_nEDITMODE
					
					cmdAddRequirement.Enabled = True
					cmdAddService.Enabled = True
					lvwInfoChklst.Enabled = True
					
				Case g_nREADMODE
					
					cmdAddRequirement.Enabled = False
					cmdAddService.Enabled = False
					cmdEdit.Enabled = False
					lvwInfoChklst.Enabled = True
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failure to PrepareForm", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			' ReDim m_ctlTabFirstLast(1, 0)
			
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
	' Author: SK
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdAddRequirement.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddRequirementButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdAddService.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddServiceButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            Label1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Tab

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Service/Requirement


            lvwInfoChklst.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeader1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Description


            lvwInfoChklst.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeader2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Type


            lvwInfoChklst.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeader3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Reference


            lvwInfoChklst.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeader4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Date Critical


            lvwInfoChklst.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeader5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'DC240901
			'Date Received


            lvwInfoChklst.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeader6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Menu Items

            mnuFile.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMenuFile, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            mnuReturnToNavigator.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMenuNavigator, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	Private Sub cmdAddRequirement_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddRequirement.Click
		
		g_bAddExpSer = True
		
		'IF Add requirement button is clicked
		'the ServTypeId is set to 1
		ServTypeId = ACTypeReqmnt
        'developer guide no.50
        frmRequirement = New frmRequirement

		frmRequirement.Service = ""
		frmRequirement.Reference = ""
		frmRequirement.DateReq = ""
		frmRequirement.TimeReq = ""
		frmRequirement.DateCrit = ""
		frmRequirement.TimeCrit = ""
		frmRequirement.DateRecv = ""
		frmRequirement.TimeRecv = ""
		frmRequirement.Contact = ""
		frmRequirement.Desc = ""
		'AJM (25/07/2001) - set up Add/Edit value for caption ID
		frmRequirement.Task = gPMConstants.PMEComponentAction.PMAdd 'Add
		
		Dim tempLoadForm As frmRequirement = frmRequirement


		frmRequirement.ShowDialog()
		
		'once the OK button of the frmRequirement screen is pressed
		If frmRequirement.Status = gPMConstants.PMEReturnCode.PMOK Then
			
			'take the properties of frmRequirement(just unloaded)
			'& set it to the global variable
			'ESId not being passed
			m_lPrtyClmId = frmRequirement.PrtyClmId
			'Service type is not being passed as it is predefined
			'to 1-requirement as thats the form that has been caused
			m_sService = frmRequirement.Service
			m_sDesc = frmRequirement.Desc
			m_sReference = frmRequirement.Reference
			m_sContact = frmRequirement.Contact
			m_vDateReq = frmRequirement.DateReq & " " & frmRequirement.TimeReq
			m_vDateCrit = frmRequirement.DateCrit & " " & frmRequirement.TimeCrit
			m_vDateRecv = frmRequirement.DateRecv & " " & frmRequirement.TimeRecv
			
			
			m_lCount += 1
			'here the iStatus will always be equal to PMAdd & no check will be required for ClaimMode
			'as CmdAddRequirement button will only be pressed when tring to add something to the database.


			m_lReturn = g_oBusiness.EditAdd(m_lCount, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vClaim_Id:=ClaimId, vExpServId:=0, vPrtyClmId:=m_lPrtyClmId, vServTypeId:=ACTypeReqmnt, vService:=m_sService, vDescription:=m_sDesc, vReference:=m_sReference, vContact:=m_sContact, vDateReq:=m_vDateReq, vDateCrit:=m_vDateCrit, vDateRecv:=m_vDateRecv, vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				Exit Sub
				
			End If
			
			'update the values into the listview
			m_lReturn = AddToListView()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		End If
		
		frmRequirement.Close()
		
		'----------------------------------------------------------------------------------------
		cmdEdit.Enabled = False
		'----------------------------------------------------------------------------------------
		
	End Sub
	
	
	
	Private Sub cmdAddService_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddService.Click
		
		g_bAddExpSer = True
		'IF Add Service button is clicked
		'the ServTypeId is set to 1
		ServTypeId = ACTypeServ
        'developer guide no.50
        frmService = New frmService
		'Set the properties of the screen being called
		frmService.ClaimId = ClaimId
		frmService.Service = ""
		frmService.Reference = ""
		frmService.DateReq = ""
		frmService.TimeReq = ""
		frmService.DateCrit = ""
		frmService.TimeCrit = ""
		frmService.DateRecv = ""
		frmService.TimeRecv = ""
		frmService.Contact = ""
		frmService.Desc = ""
		'AJM (25/07/2001) - set up Add/Edit value for caption ID
		frmService.Task = gPMConstants.PMEComponentAction.PMAdd 'Add
		
		
		Dim tempLoadForm As frmService = frmService


		frmService.ShowDialog()
		
		'once the OK button of the frmService screen is pressed
		If frmService.Status = gPMConstants.PMEReturnCode.PMOK Then
			
			'take the properties of frmService(just unloaded)
			'& set it to the global variable
			'ESId not being passed
			m_lPrtyClmId = frmService.PrtyClmId
			'Service type is not being passed as it is predefined
			'to 1-requirement as thats the form that has been caused
			m_sService = frmService.Service
			m_sDesc = frmService.Desc
			m_sReference = frmService.Reference
			m_sContact = frmService.Contact
			m_vDateReq = frmService.DateReq & " " & frmService.TimeReq
			m_vDateCrit = frmService.DateCrit & " " & frmService.TimeCrit
			m_vDateRecv = frmService.DateRecv & " " & frmService.TimeRecv
			
			
			m_lCount += 1
			'here the iStatus will always be equal to PMAdd & no check will be required for ClaimMode
			'as CmdAddRequirement button will only be pressed when tring to add something to the database.


			m_lReturn = g_oBusiness.EditAdd(m_lCount, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vClaim_Id:=ClaimId, vExpServId:=0, vPrtyClmId:=m_lPrtyClmId, vServTypeId:=ACTypeServ, vService:=m_sService, vDescription:=m_sDesc, vReference:=m_sReference, vContact:=m_sContact, vDateReq:=m_vDateReq, vDateCrit:=m_vDateCrit, vDateRecv:=m_vDateRecv, vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				Exit Sub
				
			End If
			
			'update the values into the listview
			m_lReturn = AddToListView()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		End If
		
		frmService.Close()
		
		'----------------------------------------------------------------------------------------
		cmdEdit.Enabled = False
		'----------------------------------------------------------------------------------------
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
        Dim vClmExpServId, vClaim_Id, vExpServId, vPrtyClmId, vServTypeId As Integer
		Dim vService, vDescription, vReference, vContact As String
		Dim vDateReq, vDateCrit, vDateRecv As Object

        frmRequirement = New frmRequirement
        frmService = New frmService
		'do any validations if necessary
		Dim nCollectionObject As Integer
		
		g_bAddExpSer = False
		
		'JMK 29/05/2001
		' Defaults are set up for Requirements and Services when g_bAddExpSer is true.
		' This also needs to happen if the Claim was previously Info Only.
		If m_bInfoOnly Then
			g_bAddExpSer = True
		End If
		If lvwInfoChklst.FocusedItem.Selected Then
			'do only if there are any items in the listview
			If lvwInfoChklst.Items.Count > 0 Then
				
                'Developer Guide No. 52
                If lvwInfoChklst.FocusedItem.SubItems(2).Text = "Requirement" Then

                    'ASSIGN PROPERTIES TO THE Add FORM WHICH IS BEING LOADED
                    'from the listview

                    ' These values should ideally come from the collection-do later
                    'Developer Guide No. 52
                    'Condition Added check for nothing
                    If lvwInfoChklst.FocusedItem.SubItems.Count > 3 Then
                        frmRequirement.Reference = lvwInfoChklst.FocusedItem.SubItems(3).Text.Trim()
                    End If


                    m_lReturn = g_oBusiness.GetNext(v_lCurrentRecord:=Convert.ToString(lvwInfoChklst.FocusedItem.Tag), vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv)
                    'Minus 1 from lvwInfoChklst.SelectedItem.Index because it is 1-based,
                    'but the array is zero based

                    'Set the properties of the screen being called
                    frmRequirement.Service = vService
                    frmRequirement.DateReq = Format(vDateReq, "short date")
                    frmRequirement.TimeReq = StringsHelper.Format(vDateReq, ACTimeConversion)
                    frmRequirement.DateCrit = Format(vDateCrit, "short date")
                    frmRequirement.TimeCrit = StringsHelper.Format(vDateCrit, ACTimeConversion)
                    frmRequirement.DateRecv = Format(vDateRecv, "short date")
                    frmRequirement.TimeRecv = StringsHelper.Format(vDateRecv, ACTimeConversion)
                    frmRequirement.Contact = vContact
                    frmRequirement.Desc = vDescription
                    'AJM (25/07/2001) - set up Add/Edit value caption ID
                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                        frmRequirement.Task = gPMConstants.PMEComponentAction.PMView
                    Else
                        frmRequirement.Task = gPMConstants.PMEComponentAction.PMEdit
                    End If

                    m_lPrtyClmId = 0

                    Dim tempLoadForm As frmRequirement = frmRequirement

                    frmRequirement.ShowDialog()

                    If frmRequirement.Status = gPMConstants.PMEReturnCode.PMOK Then
                        'get the colection index from the tag property of the selected item

                        nCollectionObject = Convert.ToString(lvwInfoChklst.FocusedItem.Tag)

                        'make sure to change when Add Serive screen is called vServTypeId=0


                        m_lReturn = g_oBusiness.EditUpdate(nCollectionObject, vClmExpServId:=vClmExpServId, vClaim_Id:=ClaimId, vExpServId:=0, vPrtyClmId:=m_lPrtyClmId, vServTypeId:=ACTypeReqmnt, vService:=frmRequirement.Service, vDescription:=frmRequirement.Desc, vReference:=frmRequirement.Reference, vContact:=frmRequirement.Contact, vDateReq:=frmRequirement.DateReq & " " & frmRequirement.TimeReq, vDateCrit:=frmRequirement.DateCrit & " " & frmRequirement.TimeCrit, vDateRecv:=frmRequirement.DateRecv & " " & frmRequirement.TimeRecv, vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            Exit Sub

                        End If

                        m_lReturn = CType(EditToListView(nCollectionObject), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                    End If

                    frmRequirement.Close()

                    'EDIT SERVICE

                    'Developer Guide No 52
                ElseIf lvwInfoChklst.FocusedItem.SubItems(2).Text = "Service" Then

                    'ASSIGN PROPERTIES TO THE Add FORM WHICH IS BEING LOADED
                    'from the listview

                    'these values should ideally come from the collection-do later
                    'Developer Guide No 52
                    'Condition Added check for nothing
                    If lvwInfoChklst.FocusedItem.SubItems.Count > 3 Then
                        frmService.Reference = lvwInfoChklst.FocusedItem.SubItems(3).Text.Trim()
                    End If


                    m_lReturn = g_oBusiness.GetNext(v_lCurrentRecord:=Convert.ToString(lvwInfoChklst.FocusedItem.Tag), vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv)
                    'Minus 1 from lvwInfoChklst.SelectedItem.Index because it is 1-based,
                    'but the array is zero based

                    'Set the properties of the screen being called
                    frmService.ClaimId = ClaimId
                    frmService.Service = vService
                    frmService.PrtyClmId = vPrtyClmId
                    frmService.DateReq = Format(vDateReq, "short date")
                    frmService.TimeReq = StringsHelper.Format(vDateReq, ACTimeConversion)
                    frmService.DateCrit = Format(vDateCrit, "short date")
                    frmService.TimeCrit = StringsHelper.Format(vDateCrit, ACTimeConversion)
                    frmService.DateRecv = Format(vDateRecv, "short date")
                    frmService.TimeRecv = StringsHelper.Format(vDateRecv, ACTimeConversion)
                    frmService.Contact = vContact
                    frmService.Desc = vDescription
                    'AJM (25/07/2001) - set up Add/Edit value caption ID
                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                        frmService.Task = gPMConstants.PMEComponentAction.PMView
                    Else
                        frmService.Task = gPMConstants.PMEComponentAction.PMEdit
                    End If

                    Dim tempLoadForm2 As frmService = frmService

                    frmService.ShowDialog()

                    If frmService.Status = gPMConstants.PMEReturnCode.PMOK And Me.ClaimMode <> gPMConstants.PMEComponentAction.PMView Then
                        'get the colection index from the tag property of the selected item

                        nCollectionObject = Convert.ToString(lvwInfoChklst.FocusedItem.Tag)

                        'make sure to change when Add Serive screen is called vServTypeId=0


                        m_lReturn = g_oBusiness.EditUpdate(nCollectionObject, vClmExpServId:=vClmExpServId, vClaim_Id:=ClaimId, vExpServId:=0, vPrtyClmId:=frmService.PrtyClmId, vServTypeId:=ACTypeServ, vService:=frmService.Service, vDescription:=frmService.Desc, vReference:=frmService.Reference, vContact:=frmService.Contact, vDateReq:=frmService.DateReq & " " & frmService.TimeReq, vDateCrit:=frmService.DateCrit & " " & frmService.TimeCrit, vDateRecv:=frmService.DateRecv & " " & frmService.TimeRecv, vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If

                        m_lReturn = CType(EditToListViewServ(nCollectionObject), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                    End If
                    frmService.Close()
                    End If
            End If
        End If
		
	End Sub
	
	

	'Private Sub Command1_Click()
		'cmdEdit_Click(cmdEdit, New EventArgs())
	'End Sub
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMInfoChklst.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)
			
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
			
			'DC281100 - prepare form
			m_lReturn = CType(PrepareForm(g_nPMMode), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
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
			
			'TN20010604 start
			'we will always have info check list data
			'default to false so I don't have to comment Jude's code out from all over the place
			m_bInfoOnly = False
			
			
            m_lReturn = g_oBusiness.GetInfoOnlyFlag(v_lClaimID:=m_lClaimID, r_bStatus:=m_bWorkInfoOnlyFlag)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			'TN20010604 end
			
			'Get Expert Services
			If ClaimMode = gPMConstants.PMEComponentAction.PMAdd Then

				m_lReturn = g_oBusiness.GetExpServsAdd(g_vExpServ, RiskTypeId)
				
				'DC281100 added check for View mode
			ElseIf ClaimMode = gPMConstants.PMEComponentAction.PMEdit Or ClaimMode = gPMConstants.PMEComponentAction.PMView Then 
				If m_bInfoOnly Then

					m_lReturn = g_oBusiness.GetExpServsAdd(g_vExpServ, RiskTypeId)
				Else

					m_lReturn = g_oBusiness.GetExpServsEdit(g_vExpServ, ClaimId)
				End If
			End If
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			
			'End If
			'populate the listview
			'& call EDITADD
			m_lReturn = DataToInterface()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				' Failed to get details.
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
			
			' Terminate the general object.
            m_oGeneral.Dispose()
			
            ' Destroy the instance of the general object
			' from memory.
			m_oGeneral = Nothing
            ' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
            ' Destroy the instance of the form control object from memory.
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
						
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Display the first tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Else
							' Check we are not on the first tab.
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
						
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Set focus the the start control on the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
						
					Case Keys.End
						' End key has been pressed.
						
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Set focus the the start control on the tab.
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
	' Date:11/07/00
	'
	' Edit History:SK
	' ***************************************************************** '
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
		
		Catch 
			
			
			Exit Sub
		End Try
		
		
	End Sub
	' ***************************************************************** '
	' Name: ResizeInterface
	'
	' Description: Resizes the interface controls.
	'
	' Date :15/07/2000
	'
	' Edit History:SK
	' ***************************************************************** '
	Private Function ResizeInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			tabMainTab.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 330)
			tabMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 1710)
			
			cmdAddRequirement.Left = Me.Width - VB6.TwipsToPixelsX(4890)
			cmdAddRequirement.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 2235)
			
			cmdAddService.Left = Me.Width - VB6.TwipsToPixelsX(3360)
			cmdAddService.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 2235)
			
			cmdEdit.Left = Me.Width - VB6.TwipsToPixelsX(1830)
			cmdEdit.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 2235)
			
			cmdHelp.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 1305)
			cmdHelp.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 1425)
			
			cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 2505)
			cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 1425)
			
			cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 3705)
			cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 1425)
			
			lvwInfoChklst.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Width) - 600)
			lvwInfoChklst.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - 3405)
			
			Return result
		
		Catch 
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	
	Private Sub lvwInfoChklst_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwInfoChklst.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwInfoChklst.Columns(eventArgs.Column)
		' Column click event for the search details
		
		Try 
			
			With lvwInfoChklst
				
				If ColumnHeader.Index + 1 - 1 = 5 Then
					
					'Use Date-Sort Column if Date Column clicked
					
					ListViewHelper.SetSortedProperty(lvwInfoChklst, False)
					
					If ListViewHelper.GetSortKeyProperty(lvwInfoChklst) <> 4 Then
						ListViewHelper.SetSortKeyProperty(lvwInfoChklst, 4)
						ListViewHelper.SetSortOrderProperty(lvwInfoChklst, SortOrder.Ascending)
					Else
						ListViewHelper.SetSortOrderProperty(lvwInfoChklst, (ListViewHelper.GetSortOrderProperty(lvwInfoChklst) + 1) Mod 2)
					End If
					
					ListViewHelper.SetSortedProperty(lvwInfoChklst, True)
					
					' If current sort column header is
					' pressed.
				ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwInfoChklst)) Then 
					' Set sort order opposite of
					' current direction.
					ListViewHelper.SetSortOrderProperty(lvwInfoChklst, (ListViewHelper.GetSortOrderProperty(lvwInfoChklst) + 1) Mod 2)
				Else
					' Sort by this column (ascending).
					ListViewHelper.SetSortedProperty(lvwInfoChklst, False)
					
					' Turn off sorting so that the list
					' is not sorted twice
					ListViewHelper.SetSortOrderProperty(lvwInfoChklst, SortOrder.Ascending)
					ListViewHelper.SetSortKeyProperty(lvwInfoChklst, ColumnHeader.Index + 1 - 1)
					ListViewHelper.SetSortedProperty(lvwInfoChklst, True)
				End If
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwInfoChklst_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: lvwInfoChklst_GotFocus
	'
	' Description:Set Ok Button a default
	'
	' Date:11/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub lvwInfoChklst_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwInfoChklst.Enter
		
		' GotFocus Event for the search details
		

		'Try 
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwInfoChklst_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		
	End Sub
	' ***************************************************************** '
	' Name: lvwInfoChklst_lostfocus
	'
	' Description:Set find now as default
	'
	' Date:11/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub lvwInfoChklst_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwInfoChklst.Leave
		
		' LostFocus Event for the search details
		

		'Try 
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwInfoChklst_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name:lvwInfoChklst_Click
	'
	' Description:Fill the Claim Reference,Policy No.,Client Short Name
	'              in Text Box for the listitem clicked
	'
	' Date:11/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	Private Sub lvwInfoChklst_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwInfoChklst.Click

		If lvwInfoChklst.Items.Count > 0 Then
			
			If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Or ClaimMode = gPMConstants.PMEComponentAction.PMAdd Then
				
				'        cmdAdd.Enabled = True
				
			End If
			
			cmdEdit.Enabled = True
			'AJM (27/07/2001) - When the line is highlighted make the edit button the default.
			' Unset the OK as the default button so can select with enter key.
			VB6.SetDefault(cmdOK, False)
			VB6.SetDefault(cmdEdit, True)
			
			'assign the selected index to te global variable
			m_lSelectedIndex = lvwInfoChklst.FocusedItem.Index + 1
			
			
			
		End If
		
	End Sub
	' ***************************************************************** '
	' Name: lvwInfoChklst_DblClick
	'
	' Description:Move to the next form in the road map
	'
	' Date:11/07/00
	'
	' Edit History:Pandu
	' ***************************************************************** '
	Private Sub lvwInfoChklst_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwInfoChklst.DoubleClick
		
		' Double click event for the search details.
		
		Try 
			
			' Check if there are any items available.
			If lvwInfoChklst.Items.Count = 0 Then
				Exit Sub
			End If
			
			'Call code for Edit button
			cmdEdit_Click(cmdEdit, New EventArgs())
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwInfoChklst_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	Private Sub lvwInfoChklst_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwInfoChklst.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        'start
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end
		If ClaimMode <> gPMConstants.PMEComponentAction.PMView Then
			Me.cmdEdit.Enabled = Not (Me.lvwInfoChklst.GetItemAt(x, y) Is Nothing)
		End If
	End Sub
	
	Public Sub mnuReturnToNavigator_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReturnToNavigator.Click
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
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return to navigator", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				' Set the default button.
				VB6.SetDefault(cmdOK, True)
				
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
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				'TN20010604 start
				If m_bWorkInfoOnlyFlag Then
					
					If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
						m_lReturn = CType(CopyWorkToClaim(v_lWorkClaimID:=m_lClaimID), gPMConstants.PMEReturnCode)
						
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							Exit Sub
						End If
					End If
					
					m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				End If
				'TN20010604 end
				
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: PropertiesToInterface
	'
	' Description: Updates the interface details from the property
	'              members.
	'
	' Date :15/07/2000
	'
	' Edit History :Pandu
	' ***************************************************************** '
	Private Function PropertiesToInterface() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			txtClaimNumber.Text = m_sClaimNumber.Trim()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	Public Function RefreshInformationChecklist(ByRef Mode As Integer) As Integer
		If Mode = gPMConstants.PMEComponentAction.PMAdd Then
			'Add a Newly added Requrement/Service to Information checklist
			'Check for ES_Type if 1 then => Requirement
			'else if 2 then => Service in ES
			'DC281100 added check for view mode
		ElseIf Mode = gPMConstants.PMEComponentAction.PMEdit Or Mode = gPMConstants.PMEComponentAction.PMView Then 
			'Place the Edited values values in list
		End If
	End Function
	
	' ***************************************************************** '
	' Name: DataToInterface
	'
	' Description: Updates all interface details from the search data.
	'              storage.
	' Date:11/07/00
	'
	' Edit History:Pandu
	'
	'
	' ***************************************************************** '
	
	Public Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		
        'Const ACFindImage As String = "FindImage"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Clear the search details.
			lvwInfoChklst.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(g_vExpServ) Then
				Return result
			End If
			
			' Assign the details to the interface.
            Dim sDateReq, sDateCrit, sDateRecv As String

			For lRow As Integer = g_vExpServ.GetLowerBound(1) To g_vExpServ.GetUpperBound(1)
				'increment the collection count by 1
				m_lCount += 1
				'JMK 25/05/2001 if previously InfoOnly, these details havn't been saved
				' and need to be defaulted in the same way as "Add"
				If ClaimMode = gPMConstants.PMEComponentAction.PMAdd Or (ClaimMode = gPMConstants.PMEComponentAction.PMEdit And m_bInfoOnly) Then
					
					
					sDateReq = StringsHelper.Format(DateTime.Today, ACDateConversion) & " " & StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
					sDateCrit = StringsHelper.Format(DateTime.Today, ACDateConversion) & " " & StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
					sDateRecv = ""
					'add the record to the collection
					'DC160304 PN9254 added vPrtyClmId:=g_vExpServ(ACPrtyClmId, lRow) as party_claim_id set to null
					'DC041104 PN16389/PN16361 set prtyclmid to null as nothing set at this point



                    m_lReturn = g_oBusiness.EditAdd(m_lCount, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vClaim_Id:=ClaimId, vPrtyClmId:=DBNull.Value, vServTypeId:=g_vExpServ(ACServType, lRow), vDescription:=g_vExpServ(ACDesc, lRow), vDateReq:=sDateReq, vDateCrit:=sDateCrit, vDateRecv:=sDateRecv, vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)

                Else

                    If ClaimMode = gPMConstants.PMEComponentAction.PMEdit Then
                        'DC160304 PN9254 added vPrtyClmId:=g_vExpServ(ACPrtyClmId, lRow) as party_claim_id set to null


                        m_lReturn = g_oBusiness.EditAdd(m_lCount, iStatus:=gPMConstants.PMEComponentAction.PMEdit, vClaim_Id:=ClaimId, vPrtyClmId:=g_vExpServ(ACPrtyClmId, lRow), vServTypeId:=g_vExpServ(ACServType, lRow), vDescription:=g_vExpServ(ACDesc, lRow), vClmExpServId:=g_vExpServ(ACCESId, lRow), vService:=g_vExpServ(ACService, lRow), vReference:=g_vExpServ(ACRef, lRow), vContact:=g_vExpServ(ACContact, lRow), vDateReq:=g_vExpServ(ACDateReq, lRow), vDateCrit:=g_vExpServ(ACDateCrit, lRow), vDateRecv:=g_vExpServ(ACDateRecv, lRow), vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)

                        'DC281100 added check for View mode
                    Else
                        If ClaimMode = gPMConstants.PMEComponentAction.PMView Then
                            'DC160304 PN9254 added vPrtyClmId:=g_vExpServ(ACPrtyClmId, lRow) as party_claim_id set to null


                            m_lReturn = g_oBusiness.EditAdd(m_lCount, iStatus:=gPMConstants.PMEComponentAction.PMView, vClaim_Id:=ClaimId, vPrtyClmId:=g_vExpServ(ACPrtyClmId, lRow), vServTypeId:=g_vExpServ(ACServType, lRow), vDescription:=g_vExpServ(ACDesc, lRow), vClmExpServId:=g_vExpServ(ACCESId, lRow), vService:=g_vExpServ(ACService, lRow), vReference:=g_vExpServ(ACRef, lRow), vContact:=g_vExpServ(ACContact, lRow), vDateReq:=g_vExpServ(ACDateReq, lRow), vDateCrit:=g_vExpServ(ACDateCrit, lRow), vDateRecv:=g_vExpServ(ACDateRecv, lRow), vInfoStatus:=m_bInfoOnly, vUnderwritingOrAgency:=g_oBusiness.UnderwritingOrAgency)
                        End If
                    End If
                End If

                ' Assign the details to the first column.
                ' Column 1 Claim Type
                If (ClaimMode = gPMConstants.PMEComponentAction.PMView) Or (ClaimMode = gPMConstants.PMEComponentAction.PMEdit And Not m_bInfoOnly) Then

                    ' Col1 - Service/Requirement

                    oListItem = lvwInfoChklst.Items.Add(CStr(g_vExpServ(ACService, lRow)).Trim())

                    ' Col2 - Description

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(g_vExpServ(ACDesc, lRow)).Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")

                    'RWH(06/03/2001) Service is Id = 2.
                    'JMK 17/05/2001 -  Best use the constant.

                    If StringsHelper.ToDoubleSafe(CStr(g_vExpServ(ACServType, lRow)).Trim()) = ACTypeServ Then
                        ' Assign details to other the columns
                        ' Column 2 Claim Ref
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Service"
                    ElseIf StringsHelper.ToDoubleSafe(CStr(g_vExpServ(ACServType, lRow)).Trim()) = ACTypeReqmnt Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Requirement"
                    End If

                    'DC030603 -ISS4317 -allow for the fact when adding defaults can be given
                Else

                    ' Col1 - Service/Requirement
                    oListItem = lvwInfoChklst.Items.Add("") 'Trim$(g_vExpServ(ACService, lRow&)))

                    ' Col2 - Description

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(g_vExpServ(ACDesc, lRow)).Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")

                    'RWH(06/03/2001) Service is Id = 2.
                    'JMK 17/05/2001 -  Best use the constant.

                    If StringsHelper.ToDoubleSafe(CStr(g_vExpServ(ACServType, lRow)).Trim()) = ACTypeServ Then
                        ' Assign details to other the columns
                        ' Column 2 Claim Ref
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Service"
                    ElseIf StringsHelper.ToDoubleSafe(CStr(g_vExpServ(ACServType, lRow)).Trim()) = ACTypeReqmnt Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Requirement"
                    End If

                End If


                'JMK don't display reference and Date if previously Info Only, as it
                ' will be treated as "Add"

                If (ClaimMode = gPMConstants.PMEComponentAction.PMView) Or (ClaimMode = gPMConstants.PMEComponentAction.PMEdit And Not m_bInfoOnly) Then

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = Trim(g_vExpServ(ACRef, lRow))

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = Format(Trim(g_vExpServ(ACDateCrit, lRow)), "short date")
                    'DC240901 -start -added date received

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = Format(Trim(g_vExpServ(ACDateRecv, lRow)), "short date")
                    'DC240901 -end
                End If
                'put Collection index in Tag value
                oListItem.Tag = CStr(m_lCount)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwInfoChklst.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwInfoChklst.Refresh()
                End If

            Next lRow

            ' Select the first item.
            lvwInfoChklst.Items.Item(0).Selected = True

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    '
    ' ***************************************************************** '

    'Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
    'cmdAddRequirement.Enabled = Not bDisable
    '
    'cmdAddService.Enabled = Not bDisable
    '
    'cmdEdit.Enabled = Not bDisable
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Public Function AddToListView() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Update the interface details.
        Dim sRecoveryTypeDesc As String = ""
        Dim oListItem As ListViewItem

        'Const ACFindImage As String = "FindImage"

        ' Assign the details to the interface.

        If Me.ServTypeId = ACTypeServ Then

            ' Col1 - Service/Requirement
            oListItem = lvwInfoChklst.Items.Add(frmService.txtRequirement.Text.Trim())

            ' Col2 - Description
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = frmService.txtDescription.Text.Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Service"
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = frmService.txtReference.Text.Trim()
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = g_dtDateCrit.Trim()
            'DC240901 -start -add date received
            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = g_dtDateRecv.Trim()
            'DC240901 -end
            Me.ServTypeId = 0

        ElseIf Me.ServTypeId = ACTypeReqmnt Then

            ' Col1 - Service/Requirement
            oListItem = lvwInfoChklst.Items.Add(frmRequirement.txtRequirement.Text.Trim())

            ' Col2 - Description
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = frmRequirement.txtDescription.Text.Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Requirement"
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = frmRequirement.txtReference.Text.Trim()
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = g_dtDateCrit.Trim()
            'DC240901 -start -add date received
            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = g_dtDateRecv.Trim()
            'DC240901 -end

            Me.ServTypeId = 1

        End If

        oListItem.Tag = CStr(m_lCount)

        Return result
    End Function

    Public Function EditToListView(ByRef lSelectedIndex As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sRecoveryTypeDesc As String = ""

        'DC100403 -ISS3518 -was Desc
        lvwInfoChklst.Items.Item(lSelectedIndex - 1).Text = frmRequirement.Service.Trim()
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 1).Text = frmRequirement.Desc.Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 3).Text = frmRequirement.Reference.Trim()
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 4).Text = g_dtDateCrit.Trim()
        'DC240901 -start -added date received
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 5).Text = g_dtDateRecv.Trim()
        'DC240901 -end

        Return result
    End Function

    Public Function EditToListViewServ(ByRef lSelectedIndex As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sRecoveryTypeDesc As String = ""


        'DC100403 -ISS3518 -was Desc
        lvwInfoChklst.Items.Item(lSelectedIndex - 1).Text = frmService.Service.Trim()
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 1).Text = frmService.Desc.Trim().Replace(Strings.Chr(13) & Strings.Chr(10), " ")
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 3).Text = frmService.Reference.Trim()
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 4).Text = g_dtDateCrit.Trim()
        'DC240901 -start -added date recieved
        ListViewHelper.GetListViewSubItem(lvwInfoChklst.Items.Item(lSelectedIndex - 1), 5).Text = g_dtDateRecv.Trim()
        'DC240901 -end

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateDocument
    '
    ' Description: Retrieves document id from FindDocTemplate component for
    '               given type and Policy Id and generates document via
    '               Document Template component.
    '
    ' History: 06/03/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GenerateDocument(ByRef v_iDocType As Integer, ByRef v_sTransactionType As String, ByRef v_iMode As Integer, ByRef v_lClaimId As Integer, ByRef v_sSpoolDesc As String) As Integer

        Dim result As Integer = 0
        Dim lDocTemplateId, lDocTypeId As Integer
        Dim oDocTemplate As iPMBDocTemplate.Interface_Renamed
        Dim vResultArray(,) As Object
        Dim lInsFileCnt, lPartyCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetClientAndPolicyID(v_lClaimId, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the client ID

            lInsFileCnt = CInt(vResultArray(0, 0))

            ' Get the policy ID

            lPartyCnt = CInt(vResultArray(1, 0))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetDocID(lInsFileCnt, v_iDocType, v_sTransactionType, lDocTemplateId, lDocTypeId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lDocTemplateId = 0 Then
                Return result
            End If

            'Generate document.
            oDocTemplate = New iPMBDocTemplate.Interface_Renamed()

            If oDocTemplate Is Nothing Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMBDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInvite", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'initialise object
            If CType(oDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then

                oDocTemplate = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oDocTemplate.PartyCnt = lPartyCnt
            oDocTemplate.InsuranceFileCnt = lInsFileCnt

            oDocTemplate.SpoolDesc = v_sSpoolDesc '& CStr(v_lInsuranceFileCnt)
            oDocTemplate.DocumentTemplateId = lDocTemplateId
            oDocTemplate.DocumentTypeId = lDocTypeId
            oDocTemplate.Mode = v_iMode
            oDocTemplate.ClaimCnt = v_lClaimId

            If oDocTemplate.Start() <> gPMConstants.PMEReturnCode.PMTrue Then

                oDocTemplate = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oDocTemplate.Dispose()
            oDocTemplate = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: GetDocID (Private)
	'
	' Description: Get document template id and document type id
	'
	' ***************************************************************** '
	Private Function GetDocID(ByVal v_lInsuranceFileCnt As Integer, ByVal v_iDocType As Integer, ByVal v_sTransactionType As String, ByRef r_lDocTemplateID As Integer, ByRef r_lDocTypeID As Integer) As Integer
		
		Dim result As Integer = 0
		Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oFindDocTemplate = New iPMBFindDocTemplate.Interface_Renamed()
			
			If oFindDocTemplate Is Nothing Then
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMBFindDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			'initialise the object
			If CType(oFindDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
				oFindDocTemplate = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = oFindDocTemplate.SetProcessModes(vTransactionType:=v_sTransactionType)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oFindDocTemplate = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oFindDocTemplate.InsuranceFileCnt = v_lInsuranceFileCnt
			oFindDocTemplate.ProcessType = v_iDocType
			oFindDocTemplate.Mode = 2 'invisible merge
			
			If oFindDocTemplate.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
				oFindDocTemplate = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			r_lDocTemplateID = oFindDocTemplate.DocumentTemplateId
			r_lDocTypeID = oFindDocTemplate.DocumentTypeId
			
            oFindDocTemplate.Dispose()

            oFindDocTemplate = Nothing
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document template id and document type id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: CopyWorkToClaim
	' Created: JMK 21/05/2001
	' Description: Info Only Claims do not proceed to the 'Copy work to claim'
	'               part of the roadmap, so it is done here
	'
	' ***************************************************************** '
	Public Function CopyWorkToClaim(ByVal v_lWorkClaimID As Integer) As Integer
		Dim result As Integer = 0
		Dim oChangeClaimStatus As iCLMChangeClaimStatus.Interface_Renamed
        Dim vKeyArray(,) As Object
		
		Try 
			
			ReDim vKeyArray(1, 0)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			vKeyArray(0, 0) = "claim_cnt"

			vKeyArray(1, 0) = v_lWorkClaimID
			
			Dim temp_oChangeClaimStatus As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oChangeClaimStatus, sClassName:="iCLMChangeClaimStatus.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oChangeClaimStatus = temp_oChangeClaimStatus
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMChangeClaimStatus.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'JMK 28/02/2002 - Initialise BEFORE SetProcessModes

			m_lReturn = CType(oChangeClaimStatus, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = oChangeClaimStatus.SetProcessModes(vTask:=g_nPMMode, vTransactionType:=m_sTransactionType)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetProcessModes.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = oChangeClaimStatus.SetKeys(vKeyArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = oChangeClaimStatus.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

            oChangeClaimStatus.Dispose()

            oChangeClaimStatus = Nothing

			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Copy Work To Claim", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyWorkToClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: UnlockClaim
	'
	' Description:
	'
	' History: 17/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Public Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
		Dim result As Integer = 0
		Dim oPMLock As bPMLock.User
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get bPMLock
			Dim temp_oPMLock As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oPMLock = temp_oPMLock
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Return result
			End If
			

			m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)
			
			' DD 26/7/2004 - PN13122
			' Only error if return = PMError. If return = PMFalse, it just means
			' the claim was not locked in the first place.
			'If (m_lReturn <> PMTrue) Then
			If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
				
			End If
			
			oPMLock = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
