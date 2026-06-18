Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 06 May 1997
	'
	' Description: Main interface.
	'
	' Edit History: 12OCT98 CF  Updated due to changes to TransDetail
	'                           table
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Constants for the search data array indexes.
	Private Const ACIBankId As Integer = 0
	Private Const ACIShortCode As Integer = 1
	Private Const ACIBranchcode As Integer = 2
	Private Const ACIBankName As Integer = 3
	Private Const ACIIsHeadOffice As Integer = 4
	Private Const ACIAddressLine1 As Integer = 5
    Private Const vbFormCode As Integer = 0
	' Constants for SubItems of ListView (ColumnHeader indicies are these + 1)
	'Private Const ACListIShortCode = 0
	'Private Const ACListIAccountName = 1
	'Private Const ACListIAccountStatus = 2
	'Private Const ACListILedger = 3
	'Private Const ACListIAccountType = 4
	'Private Const ACListIFullKey = 5
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_iOmitBankId As Integer
	Private m_iBankID As Integer
	Private m_sBankCode As String = ""
	Private m_sShortCode As String = ""
	Private m_sBranchCode As String = ""
	Private m_sBankName As String = ""
	Private m_iHeadOFfice As String = ""
	Private m_sAddressLine1 As String = ""
	
	Private m_lMaxColWidth(5) As Integer
	
	'PWC30092002 - Issue351 - Added new field for form setup
	'(storage to allow a calling application to disable the 'Edit' and 'New' command buttons)
	Private m_bDisableCommands As Boolean
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTFindBank.General
	
	' Stores the return value for a function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the business object.
	Public m_vSearchData( ,  ) As Object
	' PRIVATE Data Members (End)
	
	' Authority Level
	Private m_lPMAuthorityLevel As Integer
	
	' Allow stopped accounts?
	Private m_bAllowStoppedAccounts As Boolean
	
	'TF100802
	Private m_oObjectManager As bObjectManager.ObjectManager


    Private m_oBusiness As bACTFindBank.Business

	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails As Object
	
	'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.8.2.1)
	Private m_bDisableWildcardSearchOption As Boolean
	Private m_bEnablePartialWildcardSearchOption As Boolean
	
	
	Public Property DisableWildcardSearchOption() As Boolean
		Get
			Return m_bDisableWildcardSearchOption
		End Get
		Set(ByVal Value As Boolean)
			m_bDisableWildcardSearchOption = Value
		End Set
	End Property
	
	
	Public Property EnablePartialWildcardSearchOption() As Boolean
		Get
			Return m_bEnablePartialWildcardSearchOption
		End Get
		Set(ByVal Value As Boolean)
			m_bEnablePartialWildcardSearchOption = Value
		End Set
	End Property
	'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.8.2.1)
	
	
	' PUBLIC Property Procedures (Begin)
	Public WriteOnly Property AllowStoppedAccounts() As Boolean
		Set(ByVal Value As Boolean)
			m_bAllowStoppedAccounts = Value
		End Set
	End Property
	
	
	Public Property PMAuthorityLevel() As Integer
		Get
			Return m_lPMAuthorityLevel
		End Get
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	Public WriteOnly Property OmitBankID() As Integer
		Set(ByVal Value As Integer)
			m_iOmitBankId = Value
		End Set
	End Property
	Public Property BankID() As Integer
		Get
			
			Return m_iBankID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iBankID = Value
			
		End Set
	End Property
	Public Property ShortCode() As String
		Get
			
			Return m_sShortCode
			
		End Get
		Set(ByVal Value As String)
			
			m_sShortCode = Value
			
		End Set
	End Property
	Public ReadOnly Property BankCode() As String
		Get
			
			Return m_sBankCode
			
		End Get
	End Property
	Public ReadOnly Property BankName() As String
		Get
			
			Return m_sBankName
			
		End Get
	End Property
	Public ReadOnly Property HeadOffice() As Integer
		Get
			
			Return CInt(m_iHeadOFfice)
			
		End Get
	End Property
	Public ReadOnly Property AddressLine1() As String
		Get
			
			Return m_sAddressLine1
			
		End Get
	End Property
	
	Friend WriteOnly Property ObjectManager() As bObjectManager.ObjectManager
		Set(ByVal Value As bObjectManager.ObjectManager)
			m_oObjectManager = Value
		End Set
	End Property

    Friend WriteOnly Property BusinessObject() As bACTFindBank.Business


        Set(ByVal Value As bACTFindBank.Business)

            m_oBusiness = Value
        End Set
    End Property
	
	Public Sub DisableCommands()
		m_bDisableCommands = True
	End Sub
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display a searching message.
			DisplayStatusSearching()
			
			' Disable parts of the interface while
			' a search is in progress.
			m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (Begin) *}
			' Get the details from the business object.

			m_lReturn = m_oBusiness.SelectBankQuery(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, vShortCode:=txtShortCode.Text, vBankName:=txtName.Text)
			
			
			' {* USER DEFINED CODE (End) *}
			
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
			
			' Display the number of item found message.
			DisplayStatusFound()
			
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
	Public Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		Dim sLookupDesc As String = ""
	
        'Const ACFindImage As String = "FindImage"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Clear the search details.
			lvwSearchResults.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vSearchData) Then
				Return result
			End If
			
			' CTAF 080200
            'developer guide no. 170
            m_lReturn = CType(ListViewFunc.ListViewBatchStart(lvwList:=lvwSearchResults), gPMConstants.PMEReturnCode)
			
			' Assign the details to the interface.
			For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
				If CInt(m_vSearchData(ACIBankId, lRow)) <> m_iOmitBankId Then
					
					' {* USER DEFINED CODE (Begin) *}
					
					
					' Assign the details to the first column.
					' Column 1 ShortName

                    oListItem = lvwSearchResults.Items.Add(CStr(m_vSearchData(ACIShortCode, lRow)).Trim(), "FindImage")
					
					' Assign details to other the columns
					With oListItem
						' Column 2 Branch Code
						ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIBranchcode, lRow)).Trim()
						
						' Column 3 Branch Name
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIBankName, lRow)).Trim()
						
						' Column 4 HeadOffice
						' PW250302 - change code to expect Head Office Name
						' from the stored procedure (instead of a number)
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIIsHeadOffice, lRow)).Trim()
						' Column 5 Address Line 1
						ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACIAddressLine1, lRow)).Trim()
					End With
                    'developer guide no. 170
                    m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwSearchResults), gPMConstants.PMEReturnCode)
					
					
					' {* USER DEFINED CODE (End) *}
					
					' Set the tag property with the index of
					' the search data storage.
					oListItem.Tag = CStr(m_vSearchData(ACIBankId, lRow))
					
					' Refresh the first X amount of rows, to
					' allow the user to see the results instantly.
					If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
						' Select the first item.
						lvwSearchResults.Items.Item(0).Selected = True
						
						' Refresh the initial results.
						lvwSearchResults.Refresh()
					End If
				End If
			Next lRow
			
			' Enable the interface now that the search has completed.
			m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
			
			' CTAF 080200
            'developer guide no. 170
            m_lReturn = CType(ListViewFunc.ListViewBatchEnd(), gPMConstants.PMEReturnCode)
			
			' Select the first item.
			lvwSearchResults.FocusedItem = lvwSearchResults.Items.Item(0)
			'lvwSearchResults.ListItems(1).Selected = True
			lvwSearchResults_Click(lvwSearchResults, New EventArgs())
			
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
			lSelectedItem = lvwSearchResults.FocusedItem.Index + 1
			
			' Update the property members.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_iBankID = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.FocusedItem.Index).Tag)
			For i As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
				If CInt(m_vSearchData(ACIBankId, i)) = m_iBankID Then
					m_sShortCode = CStr(m_vSearchData(ACIShortCode, i)).Trim()
					m_sBankName = CStr(m_vSearchData(ACIBankName, i)).Trim()
				End If
			Next i
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			
			' Get all of the lookup details.
			' See Interface SetProcessModes
			
			' {* USER DEFINED CODE (Begin) *}
			
			
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: PropertiesToInterface
	'
	' Description: Updates the interface details from the property
	'              members.
	'
	' ***************************************************************** '
	Private Function PropertiesToInterface() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			
			txtShortCode.Text = m_sShortCode.Trim()
			txtName.Text = m_sBankName.Trim()
			
			' {* USER DEFINED CODE (End) *}
			
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
			
			If m_iOmitBankId <> 0 Then
				cmdEdit.Enabled = False
				cmdEdit.Visible = False
				cmdNew.Enabled = False
				cmdNew.Visible = False
			End If
			
			'PWC30092002 - Issue351 - Disable the Edit and 'New' options if set by calling application
			If m_bDisableCommands Then
				cmdEdit.Enabled = False
				cmdNew.Enabled = False
			End If
			
			' Get all of the lookup values as related to effective date
			m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			' Display all of the lookup details.
			m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)
			
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
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Set the column widths for the search list.
			'**** SORT THIS OUT *****
			
			'    With lvwSearchResults
			'        .ColumnHeaders(1 + 1).Width = TextWidth(.ColumnHeaders(0 + 1).Text)
			'        m_lMaxColWidth(1) = .ColumnHeaders(0 + 1).Width
			'        .ColumnHeaders(2 + 1).Width = TextWidth(.ColumnHeaders(1 + 1).Text)
			'        m_lMaxColWidth(ACListIAccountStatus) = .ColumnHeaders(ACListIAccountStatus + 1).Width
			'        .ColumnHeaders(ACListIAccountName + 1).Width = TextWidth(.ColumnHeaders(2 + 1).Text)
			'        m_lMaxColWidth(ACListIAccountName) = .ColumnHeaders(ACListIAccountName + 1).Width
			'        .ColumnHeaders(ACListILedger + 1).Width = TextWidth(.ColumnHeaders(3 + 1).Text)
			'        m_lMaxColWidth(ACListILedger) = .ColumnHeaders(ACListILedger + 1).Width
			'        .ColumnHeaders(ACListIAccountType + 1).Width = TextWidth(.ColumnHeaders(4 + 1).Text)
			'        m_lMaxColWidth(ACListIAccountType) = .ColumnHeaders(ACListIAccountType + 1).Width
			''        .ColumnHeaders(ACListIFullKey + 1).Width = TextWidth(.ColumnHeaders(ACListIFullKey + 1).Text)
			''        m_lMaxColWidth(ACListIFullKey) = .ColumnHeaders(ACListIFullKey + 1).Width
			'    End With
			
			' Show the default tab (may be a parameter?)
			SSTabHelper.SetSelectedIndex(tabMain, 0)
			' Disable all the other tab panels
			For i As Integer = 0 To ACTabTitleCount - 1
				pnlMain(i).Enabled = (i = 0)
			Next i
			
			' CF090499
			
			
			' CTAF 080200
			m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchResults.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
			
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
	' Name: ClearInterface
	'
	' Description: Clears all of the interface details for a new
	'              search.
	'
	' ***************************************************************** '
	Private Function ClearInterface() As Integer
		
		Dim result As Integer = 0
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check if the user still wishes to clear
			' the interface.
			

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Display the message.
			iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			
			' Check message result.
			If iMsgResult = System.Windows.Forms.DialogResult.No Then
				' Don't continue with the clear.
				Return result
			End If
			
			' Clear the interface details.
			
			' Clear the search data array.
			m_vSearchData = Nothing
			
			' Clear the search list details.
			lvwSearchResults.Items.Clear()
			
            ' Clear the search status bar.
            'developer guide no. 168
            _stbStatus_Panel1.Text = ""
			
			' {* USER DEFINED CODE (Begin) *}
			
			txtShortCode.Text = ""
			txtName.Text = ""
			
			
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMain, 0)
			
			' Set focus to the search details.
			txtShortCode.Focus()
			
			' Set the default button.
			VB6.SetDefault(cmdFindNow, True)
			
			' {* USER DEFINED CODE (End) *}
			
			' Disable parts of the interface, so the
			' user can now only enter a new search
			m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			ReDim m_ctlTabFirstLast(1, ACTabTitleCount - 1)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			
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
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			For i As Integer = 0 To ACTabTitleCount - 1

                SSTabHelper.SetTabCaption(tabMain, i, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle + i, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			Next i
			


            lvwSearchResults.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColShortCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


            lvwSearchResults.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBranchId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


            lvwSearchResults.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColBranchName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


            lvwSearchResults.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColHeadOffice, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


            lvwSearchResults.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAddress1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (Begin) *}
			

            lblShortCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShortCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			

            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			
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
			
			' Add commands here eg.
			cmdOK.Enabled = Not bDisable
			'PWC30092002 - Issue351
			'Don't affect the state of the Edit command if the calling
			'application has disabled commands
			If Not m_bDisableCommands Then
				cmdEdit.Enabled = Not bDisable
			End If
			cmdEdit.Enabled = Not bDisable
			cmdFindNow.Enabled = Not bDisable
			cmdNewSearch.Enabled = Not bDisable
			
			' Always disable these if the results listview is empty
			If lvwSearchResults.Items.Count = 0 Then
				bDisable = True
				cmdOK.Enabled = Not bDisable
				'PWC30092002 - Issue351
				'Don't affect the state of the Edit command if the calling
				'application has disabled commands
				If Not m_bDisableCommands Then
					cmdEdit.Enabled = Not bDisable
				End If
				cmdEdit.Enabled = Not bDisable
			End If
			
			If m_sCallingAppName = "iACTBank.Interface" Then
				cmdEdit.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupValues
	'
	' Description: Gets all of the lookup values, ready to be used by
	'              the lookup function.
	'
	' Edit History: TF100802 - Moved from Main Module
	' ***************************************************************** '
	Private Function GetLookupValues() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			

			ReDim m_vLookupValues(3, ACLMax)
			
			' Setup Lookup Table Names
			m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLLedgerType) = gACTLibrary.ACTLookupLedgerType
			m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, ACLAccountType) = gACTLibrary.ACTLookupAccountType
			
			' Do not supply a key
			For i As Integer = 0 To ACLMax
				m_vLookupValues(gPMConstants.PMELookupInArrayColPos.PMLookupKey, i) = ""
			Next i
			
			' Get all of the lookup values with the correct
			' effective date.

			m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayStatusSearching
	'
	' Description: Display the status searching message.
	'
	' ***************************************************************** '
	Private Sub DisplayStatusSearching()
		
		Static sMessage As String = ""
		
		Try 
			
			' Get message text if not already present.
			If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			
            ' Display the status message.
            'developer guide no. 168
            _stbStatus_Panel1.Text = " " & sMessage
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: DisplayStatusFound
	'
	' Description: Display the status found message.
	'
	' ***************************************************************** '
	Private Sub DisplayStatusFound()
		
		Static sMessage As String = ""
		Dim lItemsFound As Integer
		
		Try 
			
			' Store the total of item found.
			If Not Information.IsArray(m_vSearchData) Then
				lItemsFound = 0
			Else
				lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
			End If
			
			' Get message text if not already present.
			If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			End If
			
            ' Display the status message.
            'developer guide no. 168
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CheckMandatory
	'
	' Description: Check if all mandatory fields have been entered in
	'              order for the search to proceed.
	'
	' ***************************************************************** '
	Private Function CheckMandatory() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Check all fields for data.
			' At least one field must be populated
			If txtShortCode.Text.Trim() <> "" Then
				If txtShortCode.Text.Trim().Length >= ACMinSearchLength Then
					Return gPMConstants.PMEReturnCode.PMTrue
				End If
			End If
			
			If txtName.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub CheckMandatoryEnable()
		
		' Check mandatory and enable the Find Now button accordingly
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
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
			
			cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			
			ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(975)
			
			tabMain.Width = Me.Width - VB6.TwipsToPixelsX(1560)
			
			lvwSearchResults.Width = Me.Width - VB6.TwipsToPixelsX(360)
			lvwSearchResults.Height = Me.Height - VB6.TwipsToPixelsY(3600)
			
			cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
			cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
			cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			
			If cmdNavigate.Visible Then
				cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1110)
			End If
			
			Return result
		
		Catch 
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: FindNow
	'
	' Description: Get the interface details from the query
	'
	' ***************************************************************** '
	Private Sub FindNow()
		'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.8.2.2)
		Dim sWildcardErrorMessage As String = ""
		'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.8.2.2)
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.8.2.2)
			'Check wildcard searches
			
			If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtShortCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				MessageBox.Show(sWildcardErrorMessage, "Find Bank")
				txtShortCode.Focus()
				
				Exit Sub
			End If
			
			If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				MessageBox.Show(sWildcardErrorMessage, "Find Bank")
				txtName.Focus()
				
				Exit Sub
			End If
			'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.8.2.2)
			
			' Gets the interface details to be displayed.
			m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
			End If
			
			If lvwSearchResults.Items.Count > 0 Then
				VB6.SetDefault(cmdFindNow, False)
				VB6.SetDefault(cmdOK, False)
			End If

			lvwSearchResults.Sorting = SortOrder.Ascending
			lvwSearchResults.Sort()
			lvwSearchResults.ListViewItemSorter = New ListViewItemComparer(0, lvwSearchResults.Sorting)
			If lvwSearchResults.Items.Count > 0 Then
				lvwSearchResults.Items.Item(0).Selected = True
				lvwSearchResults.Select()
			End If

			' Set the focus.
			lvwSearchResults.Focus()
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the FindNow command", vApp:=ACApp, vClass:=ACClass, vMethod:="FindNow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' PRIVATE Methods (End)
	
	' PRIVATE Events (Begin)
	
	
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
		' Fire up the help screen
        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

		
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			iPMFunc.ShowFormInTaskBar_Attach()
			
			' Set the object parameters to default values
			m_iBankID = 0
			m_sShortCode = ""
			m_sBankName = ""
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iACTFindBank.General()
			
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
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
		
		Try 
			
			iPMFunc.ShowFormInTaskBar_Detach()
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error,
				' so we MUST exit now.
				Exit Sub
			End If
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Check if there is sufficient search criteria
			If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
				' Inadequate data so cannot
				' continue with the search.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (End) *}
			
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
		
		Dim iCtrlDown, iNewTab As Integer
		Dim bProcessed As Boolean = False
		Dim bTabChanged As Boolean = False
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMain
				iNewTab = SSTabHelper.GetSelectedIndex(tabMain)
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.PageUp
						' Page Up key has been pressed.
						
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Display the first tab.
							iNewTab = 0
							' New tab must be visible
							Do Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
								iNewTab += 1
							Loop 
						Else
							Do 
								' If we are on the first tab.
								If iNewTab = 0 Then
									' Display the last tab.
									iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
								Else
									' Display the previous tab.
									iNewTab -= 1
								End If
								' New tab must be visible
							Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
						End If
						bProcessed = True
						bTabChanged = True
						
					Case Keys.PageDown
						' Page Down key has been pressed.
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Display the last tab.
							iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
							' New tab must be visible
							Do Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
								iNewTab -= 1
							Loop 
						Else
							Do 
								' If we are on the last tab.
								If iNewTab = (SSTabHelper.GetTabCount(tabMain) - 1) Then
									' Display the first tab.
									iNewTab = 0
								Else
									' Display the next tab.
									iNewTab += 1
								End If
								' New tab must be visible
							Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
						End If
						bProcessed = True
						bTabChanged = True
						
					Case Keys.Home
						' Home key has been pressed.
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Set focus to the first control on the tab.
							If iNewTab <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlStart, iNewTab).Focus()
							End If
							bProcessed = True
						End If
						
					Case Keys.End
						' End key has been pressed.
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							' Set focus to the last control on the tab.
							If iNewTab <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlEnd, iNewTab).Focus()
							End If
							bProcessed = True
						End If
				End Select
				' Change tabs?
				If bTabChanged Then
					SSTabHelper.SetSelectedIndex(tabMain, iNewTab)
				End If
			End With
			
			' If the key was processed
			If bProcessed Then
				KeyCode = 0
			End If

            'Developer Guie no 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMain.SelectedIndex = 0
            End If
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub frmInterface_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyUp
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown, iShiftDown, iNewTab As Integer
		Dim bProcessed As Boolean = False
		Dim bTabChanged As Boolean = False
		
		Const ACShiftMask As Integer = 1
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iShiftDown = (Shift And ACShiftMask) > 0
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMain
				iNewTab = SSTabHelper.GetSelectedIndex(tabMain)
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.Tab
						' Tab key has been pressed.
						' Check if the control key has also been pressed.
						If iCtrlDown Then
							Do 
								' Check if the shift key has also been pressed.
								If iShiftDown Then
									' If we are on the first tab.
									If iNewTab = 0 Then
										' Display the last tab.
										iNewTab = SSTabHelper.GetTabCount(tabMain) - 1
									Else
										' Display the previous tab.
										iNewTab -= 1
									End If
								Else
									' Check we are not on the last tab.
									If iNewTab = (SSTabHelper.GetTabCount(tabMain) - 1) Then
										' Display the first tab.
										iNewTab = 0
									Else
										' Display the next tab.
										iNewTab += 1
									End If
								End If
								' New tab must be visible
							Loop Until SSTabHelper.GetTabVisible(tabMain, iNewTab)
							bProcessed = True
							bTabChanged = True
						End If
				End Select
				' Change tabs?
				If bTabChanged Then
					SSTabHelper.SetSelectedIndex(tabMain, iNewTab)
				End If
			End With
			
			' If the key was processed
			If bProcessed Then
				KeyCode = 0
			End If
		
		Catch 
			
			
			
			
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
	
	Private Sub tabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMain.SelectedIndexChanged
		
		pnlMain(SSTabHelper.GetSelectedIndex(tabMain)).Enabled = True
		pnlMain(tabMainPreviousTab).Enabled = False
		
		With tabMain
			' Set focus to the first control on the tab.
			If SSTabHelper.GetSelectedIndex(tabMain) <= m_ctlTabFirstLast.GetUpperBound(1) Then
				m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMain)).Focus()
			End If
		End With
		
		tabMainPreviousTab = tabMain.SelectedIndex
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
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Reset the properties so they dont get taken back to the calling app
			m_iBankID = 0
			m_sShortCode = ""
			m_sBankName = ""
			
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
	
	Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
		' Click event of the Find Now button.
		FindNow()
		lvwSearchResults.FullRowSelect = True
	End Sub
	
	Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click
		
		' Click event of the New Search button.
		
		Try 
			
			' Clear the interface details.
			m_lReturn = CType(ClearInterface(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to clear the interface details.
			End If
		
		Catch excep As System.Exception
			
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Cancel button.
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
			' Process the next set of actions.
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
	
	Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

		' Click event of the New button.
		

		Dim oInterface As iACTBank.Interface_Renamed
		
		' Click event of the Edit Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_iBankID = 0
			m_sShortCode = ""
			m_sBankName = ""
			
			Dim temp_oInterface As Object
			m_lReturn = m_oObjectManager.GetInstance(temp_oInterface, sClassName:="iACTBank.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oInterface = temp_oInterface
			
			
			With oInterface

				.BankID = m_iBankID

				.PMAuthorityLevel = m_lPMAuthorityLevel

				.Task = gPMConstants.PMEComponentAction.PMAdd

				.CompanyID = g_iCompanyID
			End With
			

			m_lReturn = oInterface.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to start Account interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			End If
			
			'VB 16/02/2005 PN2074 Added for displaying the newly created Bank Account details
			'in Find Bank Screen

			If oInterface.Status = gPMConstants.PMEReturnCode.PMOK Then

				m_sBankCode = oInterface.BankCode.Trim()
				txtShortCode.Text = m_sBankCode
				txtName.Text = m_sBankName
				cmdFindNow_Click(cmdFindNow, New EventArgs())
			End If
			'VB End
			
			' Set the interface object to nothing.
			oInterface = Nothing
			
			' {* USER DEFINED CODE (End) *}
			
			Exit Sub
			
			
			Exit Sub
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Catch excep As System.Exception
			
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
		
		
		
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		Dim oInterface As iACTBank.Interface_Renamed
		
		' Click event of the Edit Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_lReturn = CType(DataToProperties(), gPMConstants.PMEReturnCode)
			
			
			Dim temp_oInterface As Object
			m_lReturn = m_oObjectManager.GetInstance(temp_oInterface, sClassName:="iACTBank.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oInterface = temp_oInterface
			
			
			With oInterface

				.BankID = m_iBankID

				.PMAuthorityLevel = m_lPMAuthorityLevel

				.Task = gPMConstants.PMEComponentAction.PMEdit

				.CompanyID = g_iCompanyID
			End With
			

			m_lReturn = oInterface.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to start Account interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			End If
			
			' Set the interface object to nothing.
			oInterface = Nothing
			
			' {* USER DEFINED CODE (End) *}
		
		Catch excep As System.Exception
			
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchResults_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Enter
		
		' GotFocus Event for the search details
		
		Try 
			
			' Unset any default buttons so can select by keys
			VB6.SetDefault(cmdFindNow, False)
			VB6.SetDefault(cmdOK, False)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchResults_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Leave
		
		' LostFocus Event for the search details
		
		Try 
			
			' Set the default button.
			VB6.SetDefault(cmdFindNow, True)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchResults_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.Click
		
		Dim iBankID As Integer
		Dim sShortCode, sBankName As String
		
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Populate controls from Data array for selected Account
			If lvwSearchResults.Items.Count > 0 Then
				iBankID = Convert.ToString(lvwSearchResults.FocusedItem.Tag)
				sShortCode = lvwSearchResults.FocusedItem.Text
                'developer guide no. 52
                sBankName = (lvwSearchResults.FocusedItem.SubItems(ACIBankName - 1).Text)
			End If
			
			'VB 16/02/2005 PN-2074 For displaying ShortCode and Name in Find Bank Screen
			txtShortCode.Text = sShortCode
			txtName.Text = sBankName
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchResults_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchResults.DoubleClick
		
		' Double click event for the search details.
		Try 
			
			' Check if there are any items available.
			If lvwSearchResults.Items.Count = 0 Then
				Exit Sub
			End If
			
			If Not cmdEdit.Visible Or Not cmdEdit.Enabled Then
				cmdOK_Click(cmdOK, New EventArgs())
			Else
				cmdEdit_Click(cmdEdit, New EventArgs())
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchResults_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchResults_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchResults.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwSearchResults.Columns(eventArgs.Column)
		
		' Column click event for the search details
		' Defer to the common interface
        'OnColumnClick(lvwSearchResults, ColumnHeader)
        ' Column click event for the search details

        Try

            With lvwSearchResults
                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchResults) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortedProperty(lvwSearchResults, True)
                    ListViewHelper.SetSortedProperty(lvwSearchResults, False)

                    If ListViewHelper.GetSortOrderProperty(lvwSearchResults) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, SortOrder.Ascending)
                    End If
                    ListViewHelper.SetSortKeyProperty(lvwSearchResults, ColumnHeader.Index + 1 - 1)


                    'ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                Else
                    ListViewHelper.SetSortedProperty(lvwSearchResults, True)
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchResults, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    'ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    If ListViewHelper.GetSortOrderProperty(lvwSearchResults) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchResults, SortOrder.Ascending)
                    End If
                    ListViewHelper.SetSortKeyProperty(lvwSearchResults, ColumnHeader.Index + 1 - 1)

                End If
            End With

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	Private Sub txtShortCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortCode.Enter
		' Hightlight any text.
		iPMFunc.SelectText(txtShortCode)
	End Sub
	
	Private Sub txtShortCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortCode.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		CheckMandatoryEnable()
	End Sub
	
	Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter
		' Hightlight any text.
		iPMFunc.SelectText(txtName)
	End Sub
	
	Private Sub txtName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		CheckMandatoryEnable()
	End Sub
	
	
	' PRIVATE Events (End)
End Class
