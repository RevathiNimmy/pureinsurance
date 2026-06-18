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
	' Date: 17/02/1997
	'
	' Description: Main interface.
	'
	' Edit History: 170297 - Created
	' TF240498 - ProcessPartyInterface() added to activate refresh on
	'           return to Find
	' SP011298 - changes to support new business roadmap
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    Public oFindParty As Interface_Renamed
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	'Variable for Underwriting/Broking
	
	'DC120701
	Private m_sSiriusUnderWritingBroking As String = ""
	
	'Dim m_oParty As New iCLMParty.Interface
	
	Private m_iNotEditable As Integer
	
	Private m_bDeleteMode As Boolean
	'eck120500
	Private m_vSourceArray As Object
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMFindParty.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookUpDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Declare an instance of the Lock object.

	Private m_oPMLock As bPMLock.User
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the search data from the business object.
	Public m_vSearchData( ,  ) As Object
	'sj 3/11/99 - start
	Private m_lInvariantKey As Integer
	
	Public Property InvariantKey() As Integer
		Get
			Return m_lInvariantKey
		End Get
		Set(ByVal Value As Integer)
			m_lInvariantKey = Value
		End Set
	End Property
	'sj 3/11/99 - end
	
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
	
	Public ReadOnly Property PartyClaimID() As Integer
		Get
			
			Return m_lPartyClaimID
			
		End Get
	End Property
	Public WriteOnly Property PartyName() As String
		Set(ByVal Value As String)
			
			m_sPartyName = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PartyAddress() As String
		Set(ByVal Value As String)
			
			m_sPartyAddress = Value
			
		End Set
	End Property
	
	'DC120701 more than one line of address for Broking
	Public WriteOnly Property PartyAddress1() As String
		Set(ByVal Value As String)
			
			m_sPartyAddress1 = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PartyAddress2() As String
		Set(ByVal Value As String)
			
			m_sPartyAddress2 = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PartyAddress3() As String
		Set(ByVal Value As String)
			
			m_sPartyAddress3 = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PartyAddress4() As String
		Set(ByVal Value As String)
			
			m_sPartyAddress4 = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PartyPostcode() As String
		Set(ByVal Value As String)
			
			m_sPartyPostcode = Value
			
		End Set
	End Property
	'DC120701 -end
	
	
	Public WriteOnly Property PartyPhoneNumber() As String
		Set(ByVal Value As String)
			
			m_sPartyPhoneNumber = Value
			
		End Set
	End Property
	
	Public Property Name1() As String
		Get
			
			Return m_sName
			
		End Get
		Set(ByVal Value As String)
			
			m_sName = Value
			
		End Set
	End Property
	
	' TF180199
	Public ReadOnly Property ClaimPartyType() As String
		Get
			
			Return m_sClaimPartyType
			
		End Get
	End Property
	
	
	Public Property NotEditable() As Integer
		Get
			
			Return m_iNotEditable
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iNotEditable = Value
			
		End Set
	End Property
	
	
	Public Property DeleteMode() As Boolean
		Get
			
			Return m_bDeleteMode
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bDeleteMode = Value
			
		End Set
	End Property
	'eck040500
	Public WriteOnly Property SourceArray() As Object
		Set(ByVal Value As Object)
			
			' Set the valid sources for the user


			m_vSourceArray = Value
			
		End Set
	End Property
	
	'DC120701 -start
	
	Public Property SiriusUnderwritingBroking() As String
		Get
			
			Return m_sSiriusUnderWritingBroking
			
		End Get
		Set(ByVal Value As String)
			
			m_sSiriusUnderWritingBroking = Value
			
		End Set
	End Property
	'DC120701 -end
	
	' ***************************************************************** '
	' Name:
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		Dim result As Integer = 0
		Dim sTitle, sMessage As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
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
			
			'TN20010504 Start
			'    ' Get the details from the business object.
			'    If txtPartyClaimID.Text <> "" Then
			'        If txtPartyClaimID.Text = "0" Then
			'            ' Display the number of item found message.
			'            DisplayStatusFound
			'            Exit Function
			'        ElseIf txtPartyClaimID.Text = "%" Or txtPartyClaimID.Text = "" Then
			'            g_oBusiness.PartyClaimID = 0
			'        Else
			'            g_oBusiness.PartyClaimID = CLng(txtPartyClaimID.Text)
			'        End If
			'    Else
			'        g_oBusiness.PartyClaimID = 0
			'    End If
			'TN20010504 End
			

			m_vSearchData = Nothing
			
			'TN20010504 Start - uncomment name, address and phone number
			m_sPartyName = txtName.Text
			
			'DC120701 -start -more than one line of address for Broking
			
			m_sPartyAddress = txtAddress.Text
			
			
			m_sPartyPhoneNumber = txtPhoneNumber.Text
			'TN20010504 End
			
			'DC120701 -start -more than one line of address for Broking
			'DC120701 -end

			m_lReturn = g_oBusiness.SearchByQuery(m_vSearchData, vName:=m_sPartyName, vAddress:=m_sPartyAddress, vPhoneNumber:=m_sPartyPhoneNumber, vClaimPartyType:=VB6.GetItemString(cmbType, cmbType.SelectedIndex), vBrokingOrUnderwriting:=m_sSiriusUnderWritingBroking)
			'DC120701
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			'm_lPartyClaimID = 0
			
			'Assign Values to Interface
			m_lReturn = DataToInterface()
			
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
			Select Case Information.Err().Number
				Case 13

					sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientCodeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
					

					sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
					
					m_lReturn = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
					
				Case Else
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			End Select
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
				
				' Assign the details to the first column.
				' Column 1 Name
				

				oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIName, lRow)).Trim(), "")
				
				' Assign details to other the columns
				
				
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIAddress, lRow)).Trim()
				
				
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIPhoneNumber, lRow)).Trim()
				
				Select Case CStr(m_vSearchData(ACIPartyType, lRow)).Trim()
					Case CStr(g_iDriver)
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Driver"
					Case CStr(g_iThirdParty)
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Third Party"
					Case CStr(g_iRepairer)
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Repairer"
					Case CStr(g_iWitness)
						ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Witness"
					Case Else
				End Select
				
				
				'  oListItem.Tag = Trim$(m_vSearchData(ACIPartyClaimID, lRow&))
				
				' Set the tag property with the index of
				' the search data storage.
				oListItem.Tag = CStr(lRow)
				
				' Refresh the first X amount of rows, to
				' allow the user to see the results instantly.
				If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
					' Select the first item.
					lvwSearchDetails.Items.Item(0).Selected = True
					
					' Refresh the initial results.
					lvwSearchDetails.Refresh()
				End If
				'       End If
			Next lRow
			
			' Select the first item.
			lvwSearchDetails.Items.Item(0).Selected = True
			
			' Enable the interface now that the search has completed.
			m_lReturn = DisableInterface(bDisable:=False)
			
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

			lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
			
			' Update the property members.
			
			
			m_lPartyClaimID = CInt(m_vSearchData(ACIPartyClaimID, lSelectedItem))
			m_lClaimPartyTypeID = CInt(m_vSearchData(ACIPartyType, lSelectedItem))
			m_sName = CStr(m_vSearchData(ACIName, lSelectedItem)).Trim()
			m_sPartyName = CStr(m_vSearchData(ACIName, lSelectedItem)).Trim()
			
			'DC120701 -start -more than one line of address for Broking
			
			m_sPartyAddress = CStr(m_vSearchData(ACIAddress, lSelectedItem)).Trim()
			
			m_vLicensetype = CStr(m_vSearchData(ACILicenseType, lSelectedItem)).Trim()
			m_vLicenseDescription = CStr(m_vSearchData(ACILicenseTypeDescription, lSelectedItem)).Trim()
			m_vLicenseNumber = CStr(m_vSearchData(ACILicenseNumber, lSelectedItem)).Trim()
			m_vDOB = CStr(m_vSearchData(ACIDOB, lSelectedItem)).Trim()
			m_vSex = CStr(m_vSearchData(ACISex, lSelectedItem)).Trim()
			m_vPartyStatus = CStr(m_vSearchData(ACIPartyStatus, lSelectedItem)).Trim()
			m_vDriverStatusDescription = CStr(m_vSearchData(ACIPartyStatusDescription, lSelectedItem)).Trim()
			m_vPhoneNumber = CStr(m_vSearchData(ACIPhoneNumber, lSelectedItem)).Trim()
			m_vFaxNumber = CStr(m_vSearchData(ACIFaxNumber, lSelectedItem)).Trim()
			m_vRegNumber = CStr(m_vSearchData(ACIRegNumber, lSelectedItem)).Trim()
			
			'sj 3/11/99
			'    bExistsOnSirius = True
			'    If UBound(m_vSearchData, 1) > 10 Then
			'        If IsNumeric(m_vSearchData(ACIInvariantKey, lSelectedItem&)) Then
			'            m_lInvariantKey = CLng(m_vSearchData(ACIInvariantKey, lSelectedItem&))
			'        End If
			'        If Trim$(m_vSearchData(ACISource, lSelectedItem&)) = "Broking" Then
			'            bExistsOnSirius = False
			'        End If
			'    End If
			'sj 3/11/99
			
			' TF180199 - Send the selected party type
			m_sClaimPartyType = CStr(m_vSearchData(ACIPartyType, lSelectedItem)).Trim()
			
			
			'Calculate the combined UIK
			'iSourceID% = CInt(m_vSearchData(ACISourceID, lSelectedItem&))
			m_lPartyClaimID = CInt(m_vSearchData(ACIPartyClaimID, lSelectedItem))
			
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

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			'TN20010504 Start
			'    If m_lPartyClaimID <> 0 Then
			'        txtPartyClaimID = Trim(m_lPartyClaimID)
			'    End If
			'TN20010504 End
			
			txtName.Text = m_sName.Trim()
			
			'DC120701 -start -more than one line of address for Broking
			
			txtAddress.Text = m_sAddress.Trim()
			
			txtPhoneNumber.Text = m_sPhoneNumber.Trim()
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: HideColumns
	'
	' Description: Hides columns. Leaves "v_iShowLeft" showing.
	'
	' ***************************************************************** '

	'Private Function HideColumns(ByVal v_iShowLeft As Integer) As Integer
		'
		'Dim result As Integer = 0
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'For 'iLoop1 As Integer = v_iShowLeft + 1 To lvwSearchDetails.Columns.Count
				'lvwSearchDetails.Columns.Item(iLoop1 - 1).Width = CInt(0)
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="HideColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
					'ECK 21/05/99 Don't nedd to see this anymore
					cmdNavigate.Visible = False
					cmdNavigate.Enabled = False
					
				Case Else
					cmdNavigate.Visible = False
			End Select
			
			'Populate combo box with Party Types (does not use lookup table)
			cmbType.Items.Clear()
			cmbType.Items.Insert(0, "<ALL>")
			cmbType.Items.Insert(1, ACIDriver)
			cmbType.Items.Insert(2, ACIThirdParty)
			cmbType.Items.Insert(3, ACIRepairer)
			cmbType.Items.Insert(4, ACIWitness)
			Select Case m_lClaimPartyTypeID
				Case 1 ' Driver
					cmbType.SelectedIndex = 1
					cmbType.Enabled = False
				Case 2 ' Third Party
					cmbType.SelectedIndex = 2
					cmbType.Enabled = False
				Case 3 ' Repairer
					cmbType.SelectedIndex = 3
					cmbType.Enabled = False
				Case 4 ' Witness
					cmbType.SelectedIndex = 4
					cmbType.Enabled = False
				Case Else
					cmbType.SelectedIndex = 0
					cmbType.Enabled = True
			End Select
			
			
			If m_iNotEditable = 1 Then
				cmdNew.Enabled = False
				cmdNew.Visible = False
				cmdEdit.Enabled = False
				cmdEdit.Visible = False
			End If
			
			' Set the column widths for the search list.
			lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1400))
			lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2000))
			lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2000))
			lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1300))
			
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
			
			' Set any other default values to the interface.
			
			m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'TN20010504 Start
			lblPartyClaimID.Visible = False
			txtPartyClaimID.Visible = False
			'TN20010504 End
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function OnRecentFilesList(ByRef vName As String) As Integer
		'  Dim i         ' Counter variable.
		'  OnRecentFilesList = 0
		'  For i = 0 To 4
		'    If mnuRecentFile(i + 1).Caption = Trim(vName) Then
		'      OnRecentFilesList = i + 1
		'      Exit Function
		'    End If
		'  Next i
		'    OnRecentFilesList = False
	End Function
	
	Sub UpdateFileMenu(ByRef vFileName As String)
		' Check if the open filename is already in the File menu control array.
		Dim vName As String = vFileName.Substring(0, vFileName.IndexOf(","c))
		Dim intRetVal As Integer = OnRecentFilesList(vName)
		'        If Not intRetVal Then
		' Write open filename to the registry.
		WriteRecentFiles(vFileName, intRetVal)
		'        End If
		' Update the list of the most recently opened files in the File menu control array.
		GetRecentFiles()
	End Sub
	Sub GetRecentFiles()
		' This procedure demonstrates the use of the GetAllSettings function,
		' which returns an array of values from the Windows registry. In this
		' case, the registry contains the files most recently opened.  Use the
		' SaveSetting statement to write the names of the most recent files.
		' That statement is used in the WriteRecentFiles procedure.
        Dim iTemp As Integer
		Dim sTemp, sName, sLongName As String
		' Get recent files from the registry using the GetAllSettings statement.
		' ThisApp and ThisKey are constants defined in this module.

		If String.IsNullOrEmpty(Interaction.GetSetting(ThisApp, ThisKey, "RecentFile1",  )) Then Exit Sub
		
		Dim varFiles As Object = Interaction.GetAllSettings(ThisApp, ThisKey) ' Variable to store the returned array.
		

		For i As Integer = 0 To varFiles.GetUpperBound(0)

            sTemp = CStr(varFiles(i, 1))
			iTemp = (sTemp.IndexOf(","c) + 1)
			sName = sTemp.Substring(0, iTemp - 1)
			sTemp = sTemp.Substring(iTemp)
			iTemp = (sTemp.IndexOf(","c) + 1)
			sLongName = sTemp.Substring(0, iTemp - 1)
			'        mnuRecentFile(0).Visible = True
			'        mnuRecentFile(i + 1).Caption = sName
			'        mnuRecentFile(i + 1).Tag = varFiles(i, 1)
			'        mnuRecentFile(i + 1).Visible = True
		Next i
		
	End Sub
	
	Sub WriteRecentFiles(ByRef vFileName As String, ByRef vId As Integer)
		' This procedure uses the SaveSettings statement to write the names of
		' recently opened files to the System registry. The SaveSetting
		' statement requires three parameters. Two of the parameters are
		' stored as constants and are defined in this module.  The GetAllSettings
		' function is used in the GetRecentFiles procedure to retrieve the
		' file names stored in this procedure.

		Dim strFile, key As String
		' Copy RecentFile1 to RecentFile2, and so on.
		If vId = 5 Or vId = 0 Then
			For i As Integer = 4 To 1 Step -1
				key = "RecentFile" & i
				
				strFile = Interaction.GetSetting(ThisApp, ThisKey, key,  )
				If strFile <> "" Then
					
					key = "RecentFile" & (CStr(i + 1))
					Interaction.SaveSetting(ThisApp, ThisKey, key, strFile)
				End If
			Next i
		Else
			For i As Integer = vId - 1 To 1 Step -1
				key = "RecentFile" & i
				
				strFile = Interaction.GetSetting(ThisApp, ThisKey, key,  )
				If strFile <> "" Then
					
					key = "RecentFile" & (CStr(i + 1))
					Interaction.SaveSetting(ThisApp, ThisKey, key, strFile)
				End If
			Next i
			
		End If
		strFile = vFileName
		Interaction.SaveSetting(ThisApp, ThisKey, "RecentFile1", strFile)
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: ClearInterface
	'
	' Description: Clears all of the interface details for a new
	'              search.
	'
	' ***************************************************************** '
	Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the interface details.
			
			If bConfirm Then
				
				' Check if the user still wishes to clear
				' the interface.
				

				sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display the message.
				iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
				
				' Check message result.
				If iMsgResult = System.Windows.Forms.DialogResult.No Then
					' Don't continue with the clear.
					Return result
				End If
				
			End If
			
			' Clear the search data array.
			m_vSearchData = Nothing
			
			' Clear the search list details.
			lvwSearchDetails.Items.Clear()
			
			' Clear the search status bar.
			stbStatus.Text = ""
			
			'TN20010504    txtPartyClaimID.Text = ""
			
			txtName.Text = ""
			txtAddress.Text = ""
			txtPhoneNumber.Text = ""
			
			' clear the variables too....
			'    m_lPartyClaimID = 0
			'    m_sPartyName = ""
			'    m_sPartyAddress = ""
			'    m_sPartyPhoneNumber = ""
			' Set to the first tab.
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			
			'TN20010504 Start
			' Set focus to the search details.
			'txtPartyClaimID.SetFocus
			txtName.Focus()
			'TN20010504 End
			
			' Set the default button.
			VB6.SetDefault(cmdFindNow, True)
			
			' Disable parts of the interface, so the
			' user can now only enter a new search
			m_lReturn = DisableInterface(bDisable:=True)
			
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
			ReDim m_ctlTabFirstLast(1, 2)
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			'TN20010504 Start
			'Set m_ctlTabFirstLast(ACControlStart, 0) = txtPartyClaimID
			'Set m_ctlTabFirstLast(ACControlEnd, 0) = txtName
			m_ctlTabFirstLast(ACControlStart, 0) = txtName
			m_ctlTabFirstLast(ACControlEnd, 0) = cmbType
			'TN20010504 End
			
			'    If cmbStatus.Visible = True Then
			'        Set m_ctlTabFirstLast(ACControlEnd, 0) = cmbStatus
			'    Else
			'        Set m_ctlTabFirstLast(ACControlEnd, 0) = cmbType
			'    End If
			'
			m_ctlTabFirstLast(ACControlStart, 1) = txtAddress
			m_ctlTabFirstLast(ACControlEnd, 1) = txtPhoneNumber
			
			
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
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			If m_bDeleteMode Then

				cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			Else

				cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			

			cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'TN20010504 Start
			'    lblPartyClaimID.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACPartyClaimID, _
			''        iDataType:=PMResString)
			'TN20010504 End
			

			lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddress, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblPhoneNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPhoneNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
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
			'If we're here we're searching.  Disable it until an item is clicked.
			cmdEdit.Enabled = Not bDisable
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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

				sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			
			' Display the status message.
			stbStatus.Text = " " & sMessage
		
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

				sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			End If
			
			' Display the status message.
			stbStatus.Text = " " & lItemsFound & " " & sMessage
		
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
			
			'TN20010504 Start
			'    If (Trim$(txtPartyClaimID.Text) <> "") Then
			'        If (Len(Trim$(txtPartyClaimID.Text)) >= ACMinSearchLength) Then
			'            CheckMandatory = PMTrue
			'            Exit Function
			'        End If
			'    End If
			'TN20010504 End
			
			If txtName.Text.Trim() <> "" Then
				If txtName.Text.Trim().Length >= ACMinSearchLength Then
					Return gPMConstants.PMEReturnCode.PMTrue
				End If
			End If
			
			'If (UCase$(cmbType.Text) <> "<ALL>") Then
			'    CheckMandatory = PMTrue
			'    Exit Function
			'End If
			
			If txtAddress.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			If txtPhoneNumber.Text.Trim() <> "" Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			'
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			
			'imgImage.Left = Me.Width - 975
			
			tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1560)
			'ECk 18/5/99 slight changes in repositioning to include menu
			lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
			lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(3200)
			lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(3360)
			
			cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
			'    cmdHelp.Top = Me.Height - 1110
			cmdHelp.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395
			
			cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
			'    cmdCancel.Top = Me.Height - 1110
			cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395
			
			cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
			'   cmdOK.Top = Me.Height - 1110
			cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395
			
			'    cmdNew.Top = Me.Height - 1110
			cmdNew.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395
			'    cmdEdit.Top = Me.Height - 1110
			cmdEdit.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395
			
			If cmdNavigate.Visible Then
				'        cmdNavigate.Top = Me.Height - 1110
				cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1395) ' changed 2395
			End If
			
			Return result
		
		Catch 
			
			
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: ProcessPartyInterface(Private)
	'
	' Description: Calls the appropriate Party Interface
	'
	' ***************************************************************** '
	'eck120500
	Private Function ProcessPartyInterface(Optional ByVal v_sPartyType As String = "", Optional ByVal v_lPartyClaimID As Integer = 0, Optional ByVal v_iTask As Integer = 0, Optional ByVal v_iSourceID As Integer = 0, Optional ByVal v_lIndex As Integer = 0) As Integer
		
		
		Dim result As Integer = 0
		Dim oParty As Object
		Dim sTmp As String = ""
        Dim vKeyArray(,) As Object
		Dim lSelectedItem As Integer
		
		Try 

			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = g_oObjectManager.GetInstance(oObject:=oParty, sClassName:="iCLMParty" & sTmp & ".Interface", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				If Not (oParty Is Nothing) Then

                    oParty.Dispose()
                    oParty = Nothing
				End If
				Throw New Exception()
			End If
			
			' set the party cnt and process mode if editing
			If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
				'oParty.PartyID = v_lPartyClaimID

				lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
				
				' Update the property members.
				
				m_lPartyClaimID = CInt(m_vSearchData(ACIPartyClaimID, lSelectedItem))
				m_lClaimPartyTypeID = CInt(m_vSearchData(ACIPartyType, lSelectedItem))
				m_sName = CStr(m_vSearchData(ACIName, lSelectedItem)).Trim()
				m_sPartyName = CStr(m_vSearchData(ACIName, lSelectedItem)).Trim()
				
				m_sPartyAddress = CStr(m_vSearchData(ACIAddress, lSelectedItem)).Trim()
				
				m_vLicensetype = CStr(m_vSearchData(ACILicenseType, lSelectedItem)).Trim()
				m_vLicenseDescription = CStr(m_vSearchData(ACILicenseTypeDescription, lSelectedItem)).Trim()
				m_vLicenseNumber = CStr(m_vSearchData(ACILicenseNumber, lSelectedItem)).Trim()
				m_vDOB = CStr(m_vSearchData(ACIDOB, lSelectedItem)).Trim()
				m_vSex = CStr(m_vSearchData(ACISex, lSelectedItem)).Trim()
				m_vPartyStatus = CStr(m_vSearchData(ACIPartyStatus, lSelectedItem)).Trim()
				m_vDriverStatusDescription = CStr(m_vSearchData(ACIPartyStatusDescription, lSelectedItem)).Trim()
				m_vPhoneNumber = CStr(m_vSearchData(ACIPhoneNumber, lSelectedItem)).Trim()
				m_vFaxNumber = CStr(m_vSearchData(ACIFaxNumber, lSelectedItem)).Trim()
				m_vRegNumber = CStr(m_vSearchData(ACIRegNumber, lSelectedItem)).Trim()
				

				m_lReturn = oParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					Throw New Exception()
				End If
				
				'TN20010504 Start
				'        If IsNumeric(txtPartyClaimID.Text) Then
				'            m_lPartyClaimID = txtPartyClaimID.Text
				'        End If
				'TN20010504 End
				
				m_sPartyName = txtName.Text
				
				'DC120701 -start -more than one line of address for Broking
				m_sPartyAddress = txtAddress.Text
				
				
				
				m_sPartyPhoneNumber = txtPhoneNumber.Text
				

				m_lReturn = oFindParty.GetKeys(vKeyArray, v_iTask)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					oFindParty = Nothing
					Throw New Exception()
				End If
				

				m_lReturn = oParty.SetKeys(vKeyArray)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					Throw New Exception()
				End If
			End If
			
			' set the process mode if adding a new party
			If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then

				m_lReturn = oParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					Throw New Exception()
				End If
				
				'TN20010504 Start
				'        If IsNumeric(frmInterface.txtPartyClaimID.Text) Then
				'            m_lPartyClaimID = frmInterface.txtPartyClaimID.Text
				'        End If
				'TN20010504 End
				
				m_sPartyName = Me.txtName.Text
				
				'DC120701 -start -more than one line of address for Broking
				m_sAddress = Me.txtAddress.Text
				
				
				m_sPhoneNumber = Me.txtPhoneNumber.Text
				Select Case v_sPartyType
					Case "Driver"
						m_lClaimPartyTypeID = 1
					Case "Third Party"
						m_lClaimPartyTypeID = 2
					Case "Repairer"
						m_lClaimPartyTypeID = 3
					Case "Witness"
						m_lClaimPartyTypeID = 4
					Case Else
				End Select
				

				m_lReturn = oFindParty.GetKeys(vKeyArray, v_iTask)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					oFindParty = Nothing
					Throw New Exception()
				End If
				

				m_lReturn = oParty.SetKeys(vKeyArray)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					Throw New Exception()
				End If
			End If
			
			' start the object

			m_lReturn = oParty.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse

                oParty.Dispose()
                oParty = Nothing
				Throw New Exception()
			End If
			

			m_lReturn = oParty.GetKeys(vKeyArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse

                oParty.Dispose()
                oParty = Nothing
				Throw New Exception()
			End If
			

			If oParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then
				

				m_lReturn = oFindParty.SetKeys(vKeyArray)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse

                    oParty.Dispose()
                    oParty = Nothing
					Throw New Exception()
				End If
				
				
				Select Case v_iTask
					Case gPMConstants.PMEComponentAction.PMEdit
						If v_lIndex <> 0 Then
							'update the details in the listview - they may have changed
							lvwSearchDetails.Items.Item(v_lIndex - 1).Text = m_sPartyName
							
							'DC120701 -start -more than one line of address for Broking
							
							ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 1).Text = m_sPartyAddress
							
							ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 2).Text = m_sPartyPhoneNumber
							
							Select Case m_lClaimPartyTypeID
								Case g_iDriver
									ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 3).Text = "Driver"
								Case g_iRepairer
									ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 3).Text = "Repairer"
								Case g_iThirdParty
									ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 3).Text = "ThirdParty"
								Case g_iWitness
									ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 3).Text = "Witness"
								Case Else
									' do nothing
							End Select
							lvwSearchDetails.Items.Item(v_lIndex - 1).Selected = True
							txtName.Text = lvwSearchDetails.Items.Item(v_lIndex - 1).Text
							txtAddress.Text = ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 1).Text
							txtPhoneNumber.Text = ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 2).Text
							cmdFindNow_Click(cmdFindNow, New EventArgs())
						End If
						
					Case gPMConstants.PMEComponentAction.PMAdd
						
						' Clear the interface details.
						m_lReturn = ClearInterface(bConfirm:=False)
						
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' Failed to clear the interface details.
						End If
						
						'Set the Name field and do another search to populate with the new party
						txtName.Text = m_sPartyName
						'TN20010504                txtPartyClaimID.Text = ""
						cmdFindNow_Click(cmdFindNow, New EventArgs())
						
						
					Case Else
						
						result = gPMConstants.PMEReturnCode.PMFalse
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Party Type - " & v_sPartyType, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        oParty.Dispose()
                        oParty = Nothing
						Return result
						
				End Select
				'    End If
				'ECK 18/05/99
				'    If v_iTask = PMEdit Then
				'        v_lIndex = lvwSearchDetails.SelectedItem.Index
				'        vFileName = m_sPartyName & "," & m_sPartyName & "," & m_lPartyClaimID & "," & lvwSearchDetails.ListItems(v_lIndex).SubItems(3)
				'        Call UpdateFileMenu(vFileName)
				'    Else
				'        If oParty.Status <> PMCancel Then
				'            If v_lIndex = 0 Then v_lIndex = lvwSearchDetails.ListItems.Count '+ 1
				'            vFileName = m_sPartyName & "," & m_sPartyName & "," & m_lPartyClaimID & "," & lvwSearchDetails.ListItems(v_lIndex).SubItems(3)
				'            Call UpdateFileMenu(vFileName)
				'        End If
				'    End If
				'ECK 15/10/99
			End If
			' Destroy the object

			m_lReturn = oParty.UnLoadInterface()

            oParty.Dispose()
            oParty = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Party Interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartyInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			'ECK 19/5/99
			' Clear the interface details.
			'        Resume
			m_lReturn = ClearInterface(bConfirm:=False)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to clear the interface details.
			End If
			Return result
			
		End Try
	End Function
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
		'    m_lReturn& = ShowHelp(dlgHelp:=dlgHelp, _
		''                          lContextID:=ScreenHelpID)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
		End If
		
	End Sub
	
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMFindParty.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			'Get bPMLock
			Dim temp_m_oPMLock As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oPMLock = temp_m_oPMLock
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				'Initialise = PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
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
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			'ECK 18/5/99
			GetRecentFiles()
			
			
			If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
				' Inadequate data so cannot
				' continue with the search.
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
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
		
	End Sub

    Private Const vbFormCode As Integer = 0
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

            'Developer Guide No.7
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

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

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then

                m_oPMLock = Nothing
            End If

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
		
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Try 
			
			m_lReturn = ResizeInterface()
		
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		GetRecentFiles()
		
	End Sub
	
	Private Sub lvwSearchDetails_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyUp
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		Dim irow As Integer
		
		'on pressing down arrow or up arrow key
		If (KeyCode = 38) Or (KeyCode = 40) Or (eventArgs.KeyCode = Keys.PageDown) Or (eventArgs.KeyCode = Keys.PageUp) Or (eventArgs.KeyCode = Keys.Home) Or (eventArgs.KeyCode = Keys.End) Then
			
			'check if there are any items in the listview
			If lvwSearchDetails.Items.Count > 0 Then
				
				'Down Arrow click
				If KeyCode = 40 Then
					'only move down if the current item is not the last item
					If lvwSearchDetails.FocusedItem.Index + 1 < lvwSearchDetails.Items.Count Then
						'move to the next item

						irow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
						
						'TN20010504 Start
						'txtPartyClaimID.Text = Trim(m_vSearchData(ACIPartyClaimID, irow))
						m_lClaimPartyTypeID = CInt(m_vSearchData(ACIPartyClaimID, irow))
						'TN20010504 End
						
						txtName.Text = CStr(m_vSearchData(ACIName, irow)).Trim()
						
						
						txtAddress.Text = CStr(m_vSearchData(ACIAddress, irow)).Trim()
						
						txtPhoneNumber.Text = CStr(m_vSearchData(ACIPhoneNumber, irow)).Trim()
					End If
					
					'Up arrow click
				ElseIf (KeyCode = 38) Then 
					'only move up if the current item is not the first item
					If lvwSearchDetails.FocusedItem.Index + 1 > 1 Then
						'move to the previous item

						irow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
						
						'TN20010504 Start
						'txtPartyClaimID.Text = Trim(m_vSearchData(ACIPartyClaimID, irow))
						m_lClaimPartyTypeID = CInt(m_vSearchData(ACIPartyClaimID, irow))
						'TN20010504 End
						
						txtName.Text = CStr(m_vSearchData(ACIName, irow)).Trim()
						
						txtAddress.Text = CStr(m_vSearchData(ACIAddress, irow)).Trim()
						
						
						txtPhoneNumber.Text = CStr(m_vSearchData(ACIPhoneNumber, irow)).Trim()
						
					End If
					
				Else

					irow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
					
					'TN20010504 Start
					'txtPartyClaimID.Text = Trim(m_vSearchData(ACIPartyClaimID, irow))
					m_lClaimPartyTypeID = CInt(m_vSearchData(ACIPartyClaimID, irow))
					'TN20010504 End
					
					txtName.Text = CStr(m_vSearchData(ACIName, irow)).Trim()
					
					'DC120701 -start -more than one line of address for Broking
					
					txtAddress.Text = CStr(m_vSearchData(ACIAddress, irow)).Trim()
					
					txtPhoneNumber.Text = CStr(m_vSearchData(ACIPhoneNumber, irow)).Trim()
					
				End If
				
				'    '------------------------------------------------------------------------
				'            cmdButton(ACDelete).Enabled = True
				'            cmdButton(ACModify).Enabled = False
				'            cmdButton(ACAdd).Enabled = False
				'            m_bModify = True
				'     '------------------------------------------------------------------------
				
				VB6.SetDefault(cmdOK, True)
				
			End If
		End If
	End Sub
	


	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		Try 
			
			With tabMainTab
				
				
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
			m_lReturn = m_oGeneral.ProcessCommand()
			
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
			m_lReturn = m_oGeneral.ProcessCommand()
			
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
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Gets the interface details to be displayed.
			m_lReturn = m_oGeneral.GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
			End If
			
			If lvwSearchDetails.Items.Count > 0 Then
				VB6.SetDefault(cmdFindNow, False)
				VB6.SetDefault(cmdOK, False)
				cmdEdit.Enabled = True
			End If
			
			' Set the focus.
			lvwSearchDetails.Focus()
			
			' populate the data controls
			lvwSearchDetails_Click(lvwSearchDetails, New EventArgs())
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
            ' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
    End Sub
	
	Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click
		
		' Click event of the New Search button.
		
		Try 
			
			' Clear the interface details.
			m_lReturn = ClearInterface(bConfirm:=True)
			
			' clear the variables values too....
			m_lPartyClaimID = 0
			m_sPartyName = ""
			m_sPartyAddress1 = ""
			'DC120701 added the following - more than one line of address
			m_sPartyAddress2 = ""
			m_sPartyAddress3 = ""
			m_sPartyAddress4 = ""
			m_sPartyPostcode = ""
			m_sPartyPhoneNumber = ""
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to clear the interface details.
			End If
		
		Catch excep As System.Exception
            ' Log Error.
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
	
	Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

		Dim sPartyType As String = ""
		Dim iSourceID As Integer
		' Click event of the New Button.
		
		Try 
			
			' Set the mouse pointer.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Process the Party Interface
			'eck120500 Pass source Id
			sPartyType = cmbType.Text
			m_lReturn = ProcessPartyInterface(v_sPartyType:=sPartyType, v_iSourceID:=iSourceID, v_iTask:=gPMConstants.PMEComponentAction.PMAdd)
			
			' Set the mouse pointer.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		Dim lRowID, lIndex As Integer
		Dim sPartyTypeText As String = ""
		Dim lPartyClaimID As Integer

		' Click event of the Edit Button.
		
		Try 
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			End If
			' Get id of the row that has been selected for an edit

			lRowID = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
			
			'Get the index
			lIndex = lvwSearchDetails.FocusedItem.Index + 1
			
			' Get code and cnt for the selected row id
			'sPartyTypeCode$ = Trim$(m_vSearchData(ACICode, lRowID))
			sPartyTypeText = cmbType.Text.Trim()
			'DC290601 was CInt now CLng
			lPartyClaimID = CInt(m_vSearchData(ACIPartyClaimID, lRowID))
			
			' Process the Party Interface
			m_lReturn = ProcessPartyInterface(v_sPartyType:=sPartyTypeText, v_lPartyClaimID:=lPartyClaimID, v_iTask:=gPMConstants.PMEComponentAction.PMEdit, v_lIndex:=lIndex)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter
		
		' GotFocus Event for the search details
		
		Try 
			
			' Unset any default buttons so can
			VB6.SetDefault(cmdFindNow, False)
			VB6.SetDefault(cmdOK, False)
		
		Catch excep As System.Exception
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

			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click
		
        Dim irow As Integer

		If lvwSearchDetails.Items.Count > 0 Then

			irow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
			
			' loop around and get the other details...
			' For iCount = LBound(m_vSearchData, 2) To UBound(m_vSearchData, 2)
			'   If (Trim(m_vSearchData(ACIPartyClaimID, iCount)) = sName) Then
			
			'TN20010504 Start
			'txtPartyClaimID.Text = Trim(m_vSearchData(ACIPartyClaimID, irow))
			m_lClaimPartyTypeID = CInt(m_vSearchData(ACIPartyClaimID, irow))
			'TN20010504 End
			
			txtName.Text = CStr(m_vSearchData(ACIName, irow)).Trim()
			
			'DC120701 -start -more than one line of address for Broking
			txtAddress.Text = CStr(m_vSearchData(ACIAddress, irow)).Trim()
			
			
			
			txtPhoneNumber.Text = CStr(m_vSearchData(ACIPhoneNumber, irow)).Trim()
			cmbType.SelectedIndex = CInt(m_vSearchData(ACIPartyType, irow))
			cmdNew.Enabled = cmbType.Text <> "<ALL>"
			
			
			VB6.SetDefault(cmdOK, True)
			cmdEdit.Enabled = True
		End If
		
	End Sub
	
	Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
		
		' Double click event for the search details.
		
		Try 
			
			' Check if there are any items available.
			If lvwSearchDetails.Items.Count = 0 Then
				Exit Sub
			End If
			
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
			
            ' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		If KeyCode <> 13 Then
			VB6.SetDefault(cmdOK, False)
		End If
		
	End Sub
	
	Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		Dim sName As String = ""
		
		If KeyAscii = 13 Then
			If lvwSearchDetails.Items.Count > 0 Then
				sName = lvwSearchDetails.FocusedItem.Text
				
				' loop around and get the other details...
				For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
					If CStr(m_vSearchData(ACIName, iCount)).Trim() = sName Then
						txtName.Text = CStr(m_vSearchData(ACIName, iCount)).Trim()
						
						'DC120701 -start -more than one line of address for Broking
						
						txtAddress.Text = CStr(m_vSearchData(ACIAddress, iCount)).Trim()
						
						txtPhoneNumber.Text = CStr(m_vSearchData(ACIPhoneNumber, iCount)).Trim()
						Exit For
					End If
				Next iCount
				
				VB6.SetDefault(cmdOK, True)
            End If
		End If
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)
		
		' Column click event for the search details
		
		Try 
			
			With lvwSearchDetails
				' If current sort column header is
				' pressed.
				If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
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
            ' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub txtAddress_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtAddress.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		If KeyAscii = 34 Or KeyAscii = 39 Then
			KeyAscii = 0
		End If
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter
		
		' Highlight any text.
		iPMFunc.SelectText(txtName)
		
		' Change the default button.
		VB6.SetDefault(cmdFindNow, True)
		
	End Sub
	
	Private Sub txtName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	Private Sub cmbType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbType.SelectedIndexChanged
		
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
		cmdNew.Enabled = cmbType.Text <> "<ALL>"
	End Sub
	
	Private Sub txtAddress_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress.Enter
		
		' Highlight any text.
		iPMFunc.SelectText(txtAddress)
		
	End Sub
	
	Private Sub txtAddress_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	Private Sub txtName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtName.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		If KeyAscii = 34 Or KeyAscii = 39 Then
			KeyAscii = 0
		End If
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	
	Private Sub txtPartyClaimID_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyClaimID.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
	End Sub
	
	
	
	
	Private Sub txtPartyClaimID_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPartyClaimID.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		'TN20010504 Start
		' If KeyAscii = 34 Or KeyAscii = 39 Then
		'        KeyAscii = 0
		'    End If
		'TN20010504 End
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtPhoneNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneNumber.Enter
		
		' Highlight any text.
		iPMFunc.SelectText(txtPhoneNumber)
		
	End Sub
	
	Private Sub txtPhoneNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneNumber.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
		
	End Sub
	
	Private Sub txtPhoneNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPhoneNumber.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		If KeyAscii = 34 Or KeyAscii = 39 Then
			KeyAscii = 0
		End If
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
End Class