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
	' Date: {TodaysDate}
	'
	' Description: Main interface.
	'
	' Edit History:
	' VB 01/02/2005 PN18899: New Method(GetSources) added for retrieving the all brance ID's
	'                        that is accessible by logged user.
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	Private Const ACCompanyID As Integer = 6
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_sNavigatorTitle As String = ""
	Private m_sDocumentRef As String = ""
	Private m_lDocumentId As Integer
	Private m_iDocumenttypeId As Integer
	Private m_sComboAny As String = ""
	Private m_dtDocumentDate As Date
	Private m_sComment As String = ""
	Private m_iPostingstatusID As Integer
    'Developer Guide latest guide No.7
    Private Const vbFormCode As Integer = 0
	' CF290998
	Private m_bReversingDocument As Boolean
	Private m_lReversingDocumentId As Integer
	Private m_dtReverseDate As Date
	Private m_bRecurringDocument As Boolean
	Private m_iOccurances As Integer
	Private m_vRecurringDocumentIDs As Object
	Private m_vRecurringDocumentDates As Object
	
	'Private m_oDocument As iACTDocument.Interface
	'Private m_oTransaction As iACTTransaction.Interface
	

    Private m_oDocument As Object

    Private m_oTransaction As Object

    Private m_oDocumentReversal As Object
	
	'Private m_oTransaction As iACTTransaction.Interface
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTFindDocument.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the business object.
	Public m_vSearchData( ,  ) As Object
	
	Private m_sUnderwritingOrAgency As String = ""
	
	' Last size variables for screen resizing
	Private m_lWidth As Integer
	Private m_lHeight As Integer
	Private m_vSourceArray As Object
	
	
	
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
	Public ReadOnly Property NavigatorTitle() As String
		Get
			
			' Return the objects parameter value.
			Return m_sNavigatorTitle
			
		End Get
	End Property
	
	Public Property DocumentId() As Integer
		Get
			
			' Return the objects parameter value.
			Return m_lDocumentId
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the object parameter value.
			m_lDocumentId = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	
	
	Public Property DocumentTypeID() As Integer
		Get
			
			Return m_iDocumenttypeId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iDocumenttypeId = Value
			
		End Set
	End Property
	'JK091298
	Public Property DocumentDate() As Date
		Get
			
			' Return the effective date.
			Return m_dtDocumentDate
			
		End Get
		Set(ByVal Value As Date)
			
			' Set the object parameter value.
			m_dtDocumentDate = Value
			
		End Set
	End Property
	'JK091298
	Public Property DocumentRef() As String
		Get
			
			' Return the objects parameter value.
			Return m_sDocumentRef
			
		End Get
		Set(ByVal Value As String)
			
			' Set the object parameter value.
			m_sDocumentRef = Value
			
		End Set
	End Property
	
	'JK091298
	
	Public Property Comment() As String
		Get
			Return m_sComment
		End Get
		Set(ByVal Value As String)
			m_sComment = Value
		End Set
	End Property
	
	'JK091298
	
	Public Property Postingstatus() As Integer
		Get
			
			Return m_iPostingstatusID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iPostingstatusID = Value
			
		End Set
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
		Dim sDocumentDate As String = ""
		Dim lDocType, lDocStatus As Integer
		Dim sDateFrom, sDateTo As String
		Dim vSourceArray As Object
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' Display a searching message.
			DisplayStatusSearching()
			
			' Disable parts of the interface while
			' a search is in progress.
			m_lReturn = DisableInterface(bDisable:=True)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Check if document (from) date has been entered.
			If txtDateFrom.Text.Trim() = "" Then
				' Store a non entered value.
				sDateFrom = "-1"
			Else
				sDateFrom = txtDateFrom.Text.Trim()
			End If
			
			' Check if document (to) date has been entered.
			If txtDateTo.Text.Trim() = "" Then
				' Store a non entered value.
				sDateTo = "-1"
			Else
				sDateTo = txtDateTo.Text.Trim()
			End If
			
			' Check if document type has been selected.
			If cmbType.SelectedIndex = -1 Then
				lDocType = -1
			Else
				lDocType = VB6.GetItemData(cmbType, cmbType.SelectedIndex)
			End If
			
			' Check if document status has been selected.
			If cmbStatus.SelectedIndex = -1 Then
				lDocStatus = -1
			Else
				lDocStatus = VB6.GetItemData(cmbStatus, cmbStatus.SelectedIndex)
			End If
			
			'Check if source ID has been selected.
			If cboSource.SelectedIndex = 0 Then
				'show all branches


				vSourceArray = m_vSourceArray
			Else

				vSourceArray = VB6.GetItemData(cboSource, cboSource.SelectedIndex)
			End If
			
            m_lReturn = g_oBusiness.SearchByQuery(lNumberOfRecords:=ACMaxSearchDetails, vResultArray:=m_vSearchData, vDocumentRef:=txtDocumentRef.Text, vDateFrom:=sDateFrom, vDateTo:=sDateTo, vDocumentType:=lDocType, vPostingStatus:=lDocStatus, vSourceArray:=vSourceArray)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check the return values.
			Select Case (m_lReturn)
				Case gPMConstants.PMEReturnCode.PMTrue
					' Found search details.
					
				Case gPMConstants.PMEReturnCode.PMNotFound
					' No found search details
					
				Case Else
					' Failed to get details.
					result = gPMConstants.PMEReturnCode.PMFalse
					
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
					
					Return result
			End Select
			
			' Display the number of item found message.
			DisplayStatusFound()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' VB 01/02/2005 PN18899
	' Name: GetSources
	' Date: 01/02/2005
	' Description: List those Branches Name that is accessible by logged user.
	'
	' ***************************************************************** '
	
	Public Function GetSources() As Integer
		Dim result As Integer = 0
		Const ACSourceName As Integer = 3
		'Const ACSourceID As Integer = 1
        Dim cboSource_NewIndex As Integer = -1

        Dim oBusiness As Object
		Dim iUserID As Integer
		Dim lLower, lUpper As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oBusiness = temp_oBusiness
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			iUserID = g_oObjectManager.UserID
			

			m_lReturn = oBusiness.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=iUserID, v_bIncludeDeletedSources:=True)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vSourceArray) Then
				result = gPMConstants.PMEReturnCode.PMFalse
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get valid Branches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSources")
				
				Return result
			End If
			cboSource.Items.Clear()
			

			lLower = m_vSourceArray.GetLowerBound(1)

			lUpper = m_vSourceArray.GetUpperBound(1)
			
			' If we have more than one branch add an all branches options
			If lLower < lUpper Then

				cboSource_NewIndex = cboSource.Items.Add("All Branches")
				VB6.SetItemData(cboSource, cboSource_NewIndex, 0)
			End If
			

			For lSub As Integer = lLower To m_vSourceArray.GetUpperBound(1)

                If CStr(m_vSourceArray(ACSourceName, lSub)) <> "" Then

                    cboSource_NewIndex = cboSource.Items.Add(CStr(m_vSourceArray(ACSourceName, lSub)))

                    VB6.SetItemData(cboSource, cboSource_NewIndex, CInt(m_vSourceArray(0, lSub)))
                End If
			Next lSub
			

		oBusiness.Dispose()
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=CInt(gPMConstants.PMGetViaClientManager), sMsg:="Failed to get an instance of the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSources", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
		Dim sDocumentType, sPostingstatus As String
		
        'Const ACFindImage As String = "FindImage"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Clear the search details.
			lvwSearchDetails.Items.Clear()
			
			' Check that search details are valid before
			' continuing.
			If Not Information.IsArray(m_vSearchData) Then
				Return result
			End If
			
			' Assign the details to the interface.
			
			For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' Assign the details to the first column.

				oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIDocumentRef, lRow)).Trim(), "")
				
				' Get the document type description for the lookup values.
				m_lReturn = GetLookupDesc(sLookupTable:=gACTLibrary.ACTLookupDocumentType, lLookupID:=CInt(m_vSearchData(ACIDocumentType, lRow)), sLookupDesc:=sDocumentType)
				
				' Get the postingstatus description for the lookup values.
				m_lReturn = GetLookupDesc(sLookupTable:=gACTLibrary.ACTLookupPostingStatus, lLookupID:=CInt(m_vSearchData(ACIPostingStatus, lRow)), sLookupDesc:=sPostingstatus)
				
				' Assign details to other the columns
				ListViewHelper.GetListViewSubItem(oListItem, ACIDocumentDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACIDocumentDate, lRow)))
				ListViewHelper.GetListViewSubItem(oListItem, ACIDocumentType).Text = sDocumentType
				ListViewHelper.GetListViewSubItem(oListItem, ACIPostingStatus).Text = sPostingstatus
				ListViewHelper.GetListViewSubItem(oListItem, ACIComment).Text = CStr(m_vSearchData(ACIComment, lRow)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, ACIDocumentDateSort).Text = CDate(m_vSearchData(ACIDocumentDate, lRow)).ToString("yyyyMMdd")
				
				' {* USER DEFINED CODE (End) *}
				
				' Set the tag property with the index of
				' the search data storage.
				oListItem.Tag = CStr(lRow)
			Next lRow
			
			ListView6Autosize(lvwSearchDetails)
			
			' Select the first item.
			lvwSearchDetails.Items.Item(0).Selected = True
			
			
			' Enable the interface now that the search
			' has completed.
			m_lReturn = DisableInterface(bDisable:=False)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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

			lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
			
			' Update the property members.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_sDocumentRef = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()
			m_iDocumenttypeId = CInt(m_vSearchData(ACIDocumentType, lSelectedItem))
			m_lDocumentId = CInt(m_vSearchData(ACIDocumentId, lSelectedItem))
			m_dtDocumentDate = CDate(m_vSearchData(ACIDocumentDate, lSelectedItem))
			'JK091298
			m_sComment = CStr(m_vSearchData(ACIComment, lSelectedItem))
			m_iPostingstatusID = CInt(m_vSearchData(ACIPostingStatus, lSelectedItem))
			' Set the navigator title.
			m_sNavigatorTitle = CStr(m_vSearchData(ACIDocumentRef, lSelectedItem)).Trim()
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			m_lReturn = GetLookupValues()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Add (Any) as first entry
			Dim cmbType_NewIndex As Integer = -1
			cmbType_NewIndex = cmbType.Items.Add(m_sComboAny)
			VB6.SetItemData(cmbType, cmbType_NewIndex, -1)
			
			m_lReturn = GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupDocumentType, ctlLookup:=cmbType)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Add (Any) as first entry
			Dim cmbStatus_NewIndex As Integer = -1
			cmbStatus_NewIndex = cmbStatus.Items.Add(m_sComboAny)
			VB6.SetItemData(cmbStatus, cmbStatus_NewIndex, -1)
			
			m_lReturn = GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupPostingStatus, ctlLookup:=cmbStatus)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			cmbType.SelectedIndex = 0
			cmbStatus.SelectedIndex = 0
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
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
			
			txtDocumentRef.Text = m_sDocumentRef.Trim()
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			' Get the Source ID values.
			m_lReturn = GetSources()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			'EK 010300
			'    Select Case (m_lNavigate&)
			'        Case PMNavigateEnabled
			'            cmdNavigate.Visible = True
			'            cmdNavigate.Enabled = True
			'
			'        Case PMNavigateDisabled
			'            cmdNavigate.Visible = True
			'            cmdNavigate.Enabled = False
			'
			'        Case Else
			'            cmdNavigate.Visible = False
			'    End Select
			
			'Demo
			'cmdEdit.Visible = False
			'cmdNew.Visible = False
			
			' Update the interface details with the
			' property members.
			m_lReturn = PropertiesToInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = SetFirstLastControls()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Display all of the lookup details.
			m_lReturn = DisplayLookupDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			' Set the column widths for the search list.
			lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1000))
			lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1000))
			lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(2000))
			lvwSearchDetails.Columns.Item(5).Width = CInt(0)
			' {* USER DEFINED CODE (End) *}
			
			
			cmdReverse.Enabled = False
			cmdReverse.Visible = False
			
			
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
			

			sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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
			lvwSearchDetails.Items.Clear()
			
			' Clear the search status bar.
			stbStatus.Text = ""
			
			' {* USER DEFINED CODE (Begin) *}
			
			txtDocumentRef.Text = ""
			txtDateFrom.Text = ""
			txtDateTo.Text = ""
			
			'JK091298 -Changed index = 0 to display '(Any)' in combobox
			
			cmbType.SelectedIndex = 0 '-1
			cmbStatus.SelectedIndex = 0 '-1
			cboSource.SelectedIndex = 0
			
			' Set focus to the search details.
			
			' {* USER DEFINED CODE (End) *}
			
			' Disable parts of the interface, so the
			' user can now only enter a new search
			m_lReturn = DisableInterface(bDisable:=True)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			ReDim m_ctlTabFirstLast(1, 0)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtDocumentRef
			m_ctlTabFirstLast(ACControlEnd, 0) = cmbStatus
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			

			cmdOk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			'EK 010300 Was Navigate

			cmdReverse.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACReverseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			cboSource.SelectedIndex = 0
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			


			lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (Begin) *}
			
			


			lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblDocumentRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDocumentRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblDateFrom.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDateFrom, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblDateTo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDateTo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			m_sComboAny = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACComboAny, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'EK 010300
	' ***************************************************************** '
	' Name: ProcessReversal
	'
	' Description: Reverses the Selected Document
	'
	' ***************************************************************** '
	Private Function ProcessReversal() As Integer
		
		Dim result As Integer = 0
		Dim lSelectedItem As Integer
		
		'SD 24/07/2002 remove reference
		'Dim cServices As sPMServerCS.PMServerBusinessCS
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			Dim temp_m_oDocumentReversal As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oDocumentReversal = temp_m_oDocumentReversal
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			
			'    Set cServices = New sPMServerCS.PMServerBusinessCS
			

			lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
			
			' Update the property members.
			
			' {* USER DEFINED CODE (Begin) *}
			
			m_lDocumentId = CInt(m_vSearchData(ACIDocumentId, lSelectedItem))

			m_oDocumentReversal.DocumentId = m_lDocumentId

			m_lReturn = m_oDocumentReversal.Start()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			End If

		m_oDocumentReversal.Dispose()
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessReversal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			cmdOk.Enabled = Not bDisable
			cmdEdit.Enabled = Not bDisable
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
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
	' ***************************************************************** '
	Private Function GetLookupValues() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			' Get all of the lookup values with the correct
			' effective date.

			m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				
				Return result
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
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'developer guide No.29
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr)))
                ' Check if this is the selected index.
                If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


                    'Developer Guide no.28
                    ctlLookup.SelectedIndex = NewIndex
                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to first entry.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'Developer Guide no.28
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDesc
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDesc(ByRef sLookupTable As String, ByRef lLookupID As Integer, ByRef sLookupDesc As String) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        ' Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc")

                Return result
            End If

            ' Using the lookup values, populate the lookup
            ' string from the lookup details array when the
            ' lookup ID has been matched.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Check for a match on the ID.
                If CInt(m_vLookupDetails(ACDetailKey, lCntr)) = lLookupID Then
                    ' Found a match

                    ' Store the details to the lookup string.
                    sLookupDesc = CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim()
                End If
            Next lCntr

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            stbStatus.Text = " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

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
            stbStatus.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CallDocumentForm
    '
    ' Description: Call Document Form in Add or Edit mode
    '
    ' ***************************************************************** '
    Private Function CallDocumentForm(ByRef iTask As Integer) As Integer

        Dim result As Integer = 0
        Dim lSelectedIndex As Integer
        Dim iCompanyID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oDocument As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocument, sClassName:="iACTDocument.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oDocument = temp_m_oDocument
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oDocument.DocumentTypeID = m_iDocumenttypeId

            If (iTask = gPMConstants.PMEComponentAction.PMEdit) Or (iTask = gPMConstants.PMEComponentAction.PMView) Then

                'Retrieve the companyID from selected Row
                If CStr(m_vSearchData(ACCompanyID, lvwSearchDetails.FocusedItem.Index + 1 - 1)) <> "" Then
                    iCompanyID = CInt(m_vSearchData(ACCompanyID, lvwSearchDetails.FocusedItem.Index + 1 - 1))

                    m_oDocument.CompanyID = iCompanyID
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get selected list view item Document ID and document type
                m_lReturn = DataToProperties()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Pass ID and type to Document

                m_oDocument.DocumentId = m_lDocumentId
                'JK081298

                m_oDocument.DocumentDate = m_dtDocumentDate

                m_oDocument.Comment = m_sComment

                m_oDocument.DocumentRef = m_sDocumentRef

                m_oDocument.Postingstatus = m_iPostingstatusID

                m_oDocument.DocumentTypeID = m_iDocumenttypeId
            End If

            If (m_iPostingstatusID = 3) And (iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                iTask = gPMConstants.PMEComponentAction.PMView
            End If

            ' Set Form to Add, Edit or View
            m_lReturn = m_oDocument.SetProcessModes(vTask:=iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDocument.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If form not OK ie Cancelled return status

            If m_oDocument.Status <> gPMConstants.PMEReturnCode.PMOK Then

                result = m_oDocument.Status
            End If

            If iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' Return newly created Document ID & Date

                m_lDocumentId = m_oDocument.DocumentId

                m_dtDocumentDate = m_oDocument.DocumentDate

                ' CF290998 Extra properties

                m_bReversingDocument = m_oDocument.ReversingDocument

                m_lReversingDocumentId = m_oDocument.ReversingDocumentId

                m_dtReverseDate = m_oDocument.ReverseDate

                m_bRecurringDocument = m_oDocument.RecurringDocument

                m_iOccurances = m_oDocument.Occurances


                m_vRecurringDocumentIDs = m_oDocument.RecurringDocumentIDs


                m_vRecurringDocumentDates = m_oDocument.RecurringDocumentDates
            End If


            m_oDocument.Dispose()

            m_oDocument = Nothing

            ' Save the currently selected item list index
            If lvwSearchDetails.Items.Count > 0 Then
                lSelectedIndex = lvwSearchDetails.FocusedItem.Index + 1
            Else
                lSelectedIndex = -1
            End If

            ' Refresh any changes to the List Details from the database
            ' This is a bit of a fudge.
            ' Could update the list array directly from returned CashList properties

            If CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oGeneral.GetInterfaceDetails()
            End If

            ' Reset to currently selected item
            If lSelectedIndex <> -1 Then
                lvwSearchDetails.FocusedItem = lvwSearchDetails.Items.Item(lSelectedIndex - 1)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Document Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CallDocumentForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CallTransactionForm
    '
    ' Description: Call Transaction Form in Add or Edit mode
    '
    ' ***************************************************************** '
    Private Function CallTransactionForm(ByRef iTask As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Demo
            If iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ' Get selected list view item Document ID and document type
                m_lReturn = DataToProperties()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Dim temp_m_oTransaction As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oTransaction, sClassName:="iACTTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oTransaction = temp_m_oTransaction
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_iPostingstatusID = 3) And (iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                iTask = gPMConstants.PMEComponentAction.PMView
            End If

            ' Pass Document ID & Accounting Date

            m_oTransaction.DocumentId = m_lDocumentId

            m_oTransaction.AccountingDate = m_dtDocumentDate

            If iTask = gPMConstants.PMEComponentAction.PMAdd Or iTask = gPMConstants.PMEComponentAction.PMView Then
                ' CF290998 - Extra params

                m_oTransaction.ReversingDocument = m_bReversingDocument

                m_oTransaction.ReversingDocumentId = m_lReversingDocumentId

                m_oTransaction.ReverseDate = m_dtReverseDate

                m_oTransaction.RecurringDocument = m_bRecurringDocument

                m_oTransaction.Occurances = m_iOccurances


                m_oTransaction.RecurringDocumentIDs = m_vRecurringDocumentIDs


                m_oTransaction.RecurringDocumentDates = m_vRecurringDocumentDates
                ' CF040199 - Extra params

                m_oTransaction.DocumentRef = m_sDocumentRef
            End If

            If (m_iPostingstatusID = 3) And (iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                iTask = gPMConstants.PMEComponentAction.PMView
            End If

            ' Set Form to Add or Edit
            m_lReturn = m_oTransaction.SetProcessModes(vTask:=iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oTransaction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oTransaction.Dispose()

            m_oTransaction = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Transaction Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CallTransactionForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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

            If txtDocumentRef.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDateFrom.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If txtDateTo.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cmbType.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbType, cmbType.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If cmbStatus.SelectedIndex <> -1 Then
                If VB6.GetItemData(cmbStatus, cmbStatus.SelectedIndex) <> -1 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

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

    Private Sub cmbStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbStatus.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbType.SelectedIndexChanged
        CheckMandatoryEnable()
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer guide No.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            txtDocumentRef.Focus()
        End If
    End Sub

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTFindDocument.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


            m_sUnderwritingOrAgency = g_oBusiness.UnderwritingOrAgency

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            ' Set resize details for form controls
            SetResize()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check if the search contains more or equal
            ' to the miniumum search length.

            ' {* USER DEFINED CODE (Begin) *}

            If txtDocumentRef.Text.Trim().Length < ACMinSearchLength Then
                ' Because of the search length, we can't
                ' continue with the search.

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

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



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Enforce minimum sizes
        If Me.WindowState = FormWindowState.Normal Then
            If VB6.PixelsToTwipsX(Width) < 8295 Then Width = VB6.TwipsToPixelsX(8295)
            If VB6.PixelsToTwipsY(Height) < 5820 Then Height = VB6.TwipsToPixelsY(5820)
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            ' Resize the screen
            uctAnchor.Resize_Renamed(m_lWidth, m_lHeight, CInt(VB6.PixelsToTwipsX(ClientRectangle.Width)), CInt(VB6.PixelsToTwipsY(ClientRectangle.Height)))

            ' Store last sizes
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
        End If
    End Sub

    Private Sub lvwSearchDetails_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim lSelectedItem As Integer

        'TN20010702 - start (disable reverse button if document from sirius)
        If lvwSearchDetails.GetItemAt(x, y) Is Nothing Then
            cmdReverse.Enabled = False
        Else

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

        End If
        'TN20010702 - end

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                '        ' Set the default button.
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
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
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

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
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

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.

        Try

            'PN 25507
            CheckDates(txtDateFrom)
            If m_lReturn = 0 Then
                Exit Sub
            End If
            'PN 25507
            CheckDates(txtDateTo)
            If m_lReturn = 0 Then
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        ' Click event of the New Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Call Document header details form to Add
            m_lReturn = CallDocumentForm(iTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' If Cash List Form Error or Cancelled don't continue
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    m_lErrorNumber = m_lReturn
                End If
                Exit Sub
            End If

            ' Call Transaction object to Add and Edit items
            m_lReturn = CallTransactionForm(iTask:=gPMConstants.PMEComponentAction.PMAdd)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
                m_lErrorNumber = m_lReturn
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        ' Click event of the Edit Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Call Document header details form to Add
            m_lReturn = CallDocumentForm(iTask:=gPMConstants.PMEComponentAction.PMEdit)

            ' If Document Form Error or Cancelled don't continue
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'If we set it to cancel, we shag up the test on error number later.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    m_lErrorNumber = m_lReturn
                End If
                Exit Sub
            End If

            ' Call Document Items details object to Add and Edit items
            m_lReturn = CallTransactionForm(iTask:=gPMConstants.PMEComponentAction.PMEdit)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
                m_lErrorNumber = m_lReturn
            End If

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'EK 010300 This was Navigate
    Private Sub cmdReverse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReverse.Click
        Dim iReturn As DialogResult

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            'eck090800 Make sure they want to do it
            iReturn = MessageBox.Show("Reverse Document are you sure ? ", "Confirmation", MessageBoxButtons.YesNo)
            ' Process the next set of actions.
            If iReturn = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            ' Process the next set of actions.
            m_lReturn = ProcessReversal()

            ' Check the return value.

            'eck050900
            If m_lReturn = gPMConstants.PMEReturnCode.PMInvalidRequest Then
                MessageBox.Show("Unable to Reverse Document " & _
                                "Invalid document type or associated payment", Application.ProductName)
                Exit Sub
            End If

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to create Document Reversal", Application.ProductName)
                Exit Sub
            End If

            MessageBox.Show("Document Successfully Reversed", Application.ProductName)
            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()


            ' Set the focus.
            lvwSearchDetails.Focus()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Call Document Items details object to View Transactions
            m_lReturn = CallTransactionForm(iTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOk, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            With lvwSearchDetails

                'Change the sort key if column is date
                If ColumnHeader.Index + 1 - 1 = ACIDocumentDate Then
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
                    If ListViewHelper.GetSortKeyProperty(lvwSearchDetails) <> ACIDocumentDateSort Then
                        ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ACIDocumentDateSort)
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                    End If
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails)) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                End If
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub txtDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Enter

        ' Check date.
        iPMValidate.CheckDateGotFocus(txtDateFrom)

        ' Hightlight any text.
        iPMFunc.SelectText(txtDateFrom)

    End Sub

    Private Sub txtDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Leave

        ' Check date.
        iPMValidate.CheckDateLostFocus(txtDateFrom)

    End Sub
    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter

        ' Check date.
        iPMValidate.CheckDateGotFocus(txtDateTo)

        ' Hightlight any text.
        iPMFunc.SelectText(txtDateTo)

    End Sub

    Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave

        ' Check date.
        iPMValidate.CheckDateLostFocus(txtDateTo)

    End Sub
    Private Sub txtDateFrom_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    Private Sub txtDateTo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub

    ' PRIVATE Events (End)
    Private Sub txtDocumentRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CheckMandatoryEnable()
    End Sub


    ' ***********************************************************
    ' Set the resizing anchors
    ' ***********************************************************
    Private Sub SetResize()

        Try

            ' Set start dimensions
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))

            ' Search Block
            uctAnchor.Add(tabMainTab, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdFindNow, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(cmdNewSearch, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(ImgImage, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Search Details
            uctAnchor.Add(txtDocumentRef, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(lblDateFrom, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(txtDateFrom, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(lblDateTo, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(txtDateTo, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Document List
            uctAnchor.Add(lvwSearchDetails, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Control Buttons
            uctAnchor.Add(cmdNew, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdEdit, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdReverse, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdOk, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdCancel, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdHelp, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)

        Catch
        End Try




    End Sub

    Private Sub CheckDates(ByRef ctlControl As TextBox)

        Dim sDate As String = ""

        Try
            m_lReturn = 1
            If ctlControl.Text.Trim() <> "" Then
                sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, ctlControl.Text)

                If sDate = "" Then
                    MessageBox.Show("Invalid date", Application.ProductName)
                    ctlControl.Text = ""
                    ctlControl.Focus()
                    m_lReturn = 0

                Else
                    ctlControl.Text = sDate
                End If
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check the date on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDateLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
End Class
