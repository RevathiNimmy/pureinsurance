Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmAdjustment
	Inherits System.Windows.Forms.Form
	Private Sub frmAdjustment_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	' ***************************************************************** '
	' Form Name: frmAdjustment
	'
	' Date: 02nd October 2002
	'
	' Description: Adjustments Form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmAdjustment"
	
	' PUBLIC Data Members (Begin)
	'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)
	Public CompanyID As Integer
	Public m_sDrawerName As String = ""
	Public m_lCashDrawerID As Integer
	Public m_lCashlistID As Integer
	Public m_sLockName As String = ""
	Public m_vResultArray As Object
	Public m_blnHasCash As Boolean
	Public m_lAdjustment_ID As Integer
	
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
	
	' Status members
	Private m_sProcessStatus As New FixedLengthString(2)
	Private m_sMapStatus As New FixedLengthString(2)
	Private m_sStepStatus As New FixedLengthString(2)
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTCashList.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Form control
	Private m_oFormfields As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores private details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	'Private m_oSelectedItem As ListItem
	
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	' PRIVATE Const Members (Begin)
	' {* USER DEFINED CODE (Begin) *}
	
	'Result Array columns for CashListDrawer (ARRAY and LIST)
	Private Const ACUser As Integer = 0
    'developer guide no.7
    Private Const vbFormCode As Integer = 0
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Const Members (End)
	
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
	
	Public ReadOnly Property StepStatus() As String
		Get
			
			' Standard Property.
			
			' Return the Steps Status
			Return m_sStepStatus.Value
			
		End Get
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	
	Public WriteOnly Property Business() As Object
		Set(ByVal Value As Object)
			
			m_oBusiness = Value
			
		End Set
	End Property
	
	Public WriteOnly Property General() As iACTCashList.General
		Set(ByVal Value As iACTCashList.General)
			
			m_oGeneral = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
	' ***************************************************************** '
	' Name: SetStatus (Standard Method)
	'
	' Description: Set the Process, Map and Step status.
	' Note:        A Property Get is provided for the Step Status only
	'              as this is the only one which this component can
	'              alter directly.
	' ***************************************************************** '
	Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the current Status settings.
			m_sProcessStatus.Value = sProcessStatus.Trim()
			m_sMapStatus.Value = sMapStatus.Trim()
			m_sStepStatus.Value = sStepStatus.Trim()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetCompany (Standard Method)
	'
	' Description: Gets valid Source ID's  and if nessessary displays selection
	'
	' ***************************************************************** '
	Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
		Dim result As Integer = 0

		Dim m_oBranch As iPMBBranch.Interface_Renamed
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_m_oBranch As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			m_oBranch = temp_m_oBranch
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

            m_oBranch.Dispose()
            m_oBranch = Nothing
            Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			
			' Update the interface details.
			
			'''    'Poulate the listview with the results
			'''    PopulateList
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			
			' Add the business object.
			'             m_lReturn& = m_oBusiness.DirectAdd( _
			''                                vCashListID:=m_lCashlistID, _
			''                                vCashliststatusID:=m_lCashListStatusID, _
			''                                vCashListTypeID:=m_lCashlistTypeID, _
			''                                vCashlistRef:=m_sCashListRef, _
			''                                vCompanyID:=m_iCompanyID, _
			''                                vBankAccountID:=m_lBankAccountID, _
			''                                vCurrencyID:=m_iCurrencyID, _
			''                                vListDate:=m_dtListDate, _
			''                                vControlTotal:=m_cControlTotal, _
			''                                vItemCount:=m_lItemCount)
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
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
			' Error Section.
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
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Dim sNewCaption As String = ""
		Dim vAdjustment As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			' Display all language specific captions.
			m_lReturn = iPMForms.DisplayCaptions(Me)
			
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
			
			' ************************************************************
			' Enter your code here to set up interface defaults.
			'
			' Example:-
			'
			'    cmdOK.Default = True
			'
			'   'Setup default data for Add
			'   If (m_iTask% = PMAdd) Then
			'       cmdPopulate.Enabled = True
			'       uctType.ListIndex = 0
			'   Else
			'       uctType.ListIndex = 1
			'   End If
			
			PopulateCombos()
			
			Select Case m_iTask
				Case gPMConstants.PMEComponentAction.PMView
					'Enable form to be set as View-Only
					DisableControls()
					'Get the Data...

					m_lReturn = m_oBusiness.GetAdjustment(vAdjustments:=vAdjustment, lCashList_Adjustment_ID:=m_lAdjustment_ID)

                    txtCashDrawer.Text = CStr(vAdjustment(7, 0))

                    cboPMUserLookup1.DefaultUserID = CInt(vAdjustment(2, 0))
					

                    txtAdjDate.Text = CDate(vAdjustment(3, 0)).ToString("dd MMMM yyyy")

                    txtAmount.Text = StringsHelper.Format(vAdjustment(4, 0), "0.00")
					For ii As Integer = 0 To cboMethod.Items.Count - 1

                        If VB6.GetItemData(cboMethod, ii) = CDbl(vAdjustment(5, 0)) Then
                            cboMethod.SelectedIndex = ii
                            Exit For
                        End If
					Next ii

                    txtComments.Text = CStr(vAdjustment(6, 0))
					Me.Text = "View Banking Adjustment"
					
				Case gPMConstants.PMEComponentAction.PMAdd
					'Setup default data for Add

					m_lReturn = m_oBusiness.GetCashDrawerName(vCashListName:=m_sDrawerName, lCashDrawerId:=m_lCashDrawerID)
					
					txtCashDrawer.Text = m_sDrawerName
					
					txtAdjDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=DateTime.Now)
					txtAdjDate.Enabled = False
					cboPMUserLookup1.UserID = g_iUserID
					cboPMUserLookup1.Enabled = False
			End Select
			
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
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
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ---------------------------------------------------------------------------
	' PROCEDURE NAME: cmdCancel_Click
	' PURPOSE:
	' AUTHOR: Paul Harris
	' DATE: 06 November 2002, 11:45:10
	' CHANGES:
	' ---------------------------------------------------------------------------
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ---------------------------------------------------------------------------
	' PROCEDURE NAME: cmdOK_Click
	' PURPOSE:
	' AUTHOR: Paul Harris
	' DATE: 04 November 2002, 14:25:10
	' CHANGES:
	' ---------------------------------------------------------------------------
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			'    frmBanking.txtAdjustments = Val(frmBanking.txtAdjustments) + Val(txtAmount)
			
			' Check mandatory fields

			m_lReturn = m_oFormfields.CheckMandatoryControls()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        m_lStatus& = PMCancel
				Exit Sub
			End If
			
			If Conversion.Val(txtAmount.Text) = 0 Then
				MessageBox.Show("Amount is zero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
				Exit Sub
			End If
			

			m_lReturn = m_oBusiness.AddAdjustment(v_lCashListID:=m_lCashlistID, v_lPMUserID:=g_iUserID, v_cAmount:=CDec(txtAmount.Text), v_lAdjustMethod:=VB6.GetItemData(cboMethod, cboMethod.SelectedIndex), v_sReason:=txtComments.Text)
			
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
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
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCashList.Form", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iACTCashList.General()
			
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
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
			
			
			
			' Error Section
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmAdjustment_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
			
			' Set the rules for validating fields.

			m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)
			
			'Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}
			
			
			' {* USER DEFINED CODE (End) *}
			
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
			'    m_lReturn& = m_oGeneral.GetInterfaceDetails()
			
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
	
	Private Sub frmAdjustment_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

            'developer guide no.7
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


            m_oFormfields.Dispose()

            m_oFormfields = Nothing

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
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	'''' ***************************************************************** '
	'''' Name: PopulateList
	''''
	'''' Description: Populates the listview with data.
	''''
	'''' ***************************************************************** '
	'''Public Function PopulateList() As Long
	'''
	'''Dim lRow As Long
	'''Dim lLower As Long
	'''Dim lUpper As Long
	'''Dim oListItem As ListItem
	'''
	'''    On Error GoTo Err_PopulateList
	'''
	'''    PopulateList = PMTrue
	'''
	'''    'Clear the existing items
	'''    lvwUsers.ListItems.Clear
	'''
	'''    'Get array limits (returns false if not dimensioned)
	'''    If GetArrayBounds( _
	''''            r_vArray:=m_vResultArray, _
	''''            r_lDimension:=klRowDimension, _
	''''            r_lLower:=lLower, _
	''''            r_lUpper:=lUpper) Then
	'''
	'''        'Turn off listview updating
	'''        ListViewBatchStart lvwUsers
	'''
	'''        With lvwUsers.ListItems
	'''            'Loop through the results array and populate the listview
	'''            For lRow = lLower To lUpper
	'''                'ADD a new listitem to the listview
	'''                AddOrEditListViewItem _
	''''                        r_oListItem:=Nothing, _
	''''                        r_sUser:=CStr(m_vResultArray(ACUser, lRow))
	'''
	'''            Next lRow
	'''        End With
	'''        'Turn on listview updating
	'''        ListViewBatchEnd
	'''
	'''        With lvwUsers
	'''            'Autosize the listview columns
	'''            '(ColumnHeaders collection is 1 based so add 1 to column const)
	'''            .ColumnHeaders(ACUser + 1).Width = .Width - 360
	'''            Set .SelectedItem = Nothing
	'''        End With
	'''
	'''        'Clear the data from the array as it's now stored in the listview
	'''        m_vResultArray = ""
	'''
	'''    End If
	'''
	'''    PopulateList = PMTrue
	'''
	'''    Exit Function
	'''
	'''Err_PopulateList:
	'''
	'''    ' Error Section.
	'''    PopulateList = PMError
	'''
	'''    ' Log Error.
	'''    LogMessage _
	''''            iType:=PMLogOnError, _
	''''            sMsg:="Failed to populate the list", _
	''''            vApp:=ACApp, _
	''''            vClass:=ACClass, _
	''''            vMethod:="PopulateList", _
	''''            vErrNo:=Err.Number, _
	''''            vErrDesc:=Err.Description
	'''
	'''    'Turn on listview updating
	'''    ListViewBatchEnd
	'''
	'''    Exit Function
	'''
	'''End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (ObtainLock) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ObtainLock(ByRef r_sKeyName As String, ByRef r_lKeyValue As Integer, ByRef r_sCurrentlyLockedBy As String) As Integer
		'Dim result As Integer = 0
		'Dim bPMLock As Object
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: ObtainLock
		' PURPOSE: Request a lock
		' AUTHOR: Paul Cunnigham
		' DATE: 15 October 2002, 14:45:27
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		'

		'Dim oLock As bPMLock.User
		'
		'
		'On Error GoTo Catch_Renamed
		'
		'result = gPMConstants.PMEReturnCode.PMTrue
		'
		'Request a reference to the business obect of the PMLock component
		'Dim temp_oLock As Object
		'm_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:="ClientManager")
		'oLock = temp_oLock
		'
		'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			'Return False
		'End If
		'
		'Attempt to obtain a lock

		'm_lReturn = oLock.LockKey(sKeyName:=r_sKeyName, vKeyValue:=r_lKeyValue, iUserID:=g_iUserID, sCurrentlyLockedBy:=r_sCurrentlyLockedBy)
		'
		'Test for an error
		'If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to obtain the requested lock", vApp:=ACApp, vClass:=ACClass, vMethod:="ObtainLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			'
			'result = gPMConstants.PMEReturnCode.PMFalse
		'End If
		'
		'GoTo Finally_Renamed
		'
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		'Resume 
		'
'Catch_Renamed: '
		'Select Case Information.Err().Number
			'Case Else
				' Log Error.
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ObtainLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				'
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				'GoTo Finally_Renamed
		'End Select
		'
'Finally_Renamed: '
		'Return result
		'
	'End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (ReleaseLock) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ReleaseLock(ByRef r_sKeyName As String, ByRef r_lKeyValue As Integer) As Integer
		'Dim result As Integer = 0
		'Dim bPMLock As Object
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: ReleaseLock
		' PURPOSE: Release a lock
		' AUTHOR: Paul Cunnigham
		' DATE: 15 October 2002, 14:45:27
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		'

		'Dim oLock As bPMLock.User
		'
		'
		'On Error GoTo Catch_Renamed
		'
		'result = gPMConstants.PMEReturnCode.PMTrue
		'
		'Request a reference to the business obect of the PMLock component
		'Dim temp_oLock As Object
		'm_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:="ClientManager")
		'oLock = temp_oLock
		'
		'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			'Return False
		'End If
		'

		'm_lReturn = oLock.UnLockKey(sKeyName:=r_sKeyName, vKeyValue:=r_lKeyValue, iUserID:=g_iUserID)
		'
		'GoTo Finally_Renamed
		'
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		'Resume 
		'
'Catch_Renamed: '
		'Select Case Information.Err().Number
			'Case Else
				' Log Error.
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseLock", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				'
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				'GoTo Finally_Renamed
		'End Select
		'
'Finally_Renamed: '
		'Return result
		'
	'End Function
	
	Private Sub txtAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Enter
		iPMFunc.SelectText(txtAmount)
		

		m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtAmount)
	End Sub
	
	Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave
		txtAmount.Text = StringsHelper.Format(txtAmount.Text, "0.00")

		m_lReturn = m_oFormfields.LostFocus(txtAmount)
	End Sub
	
	Private Sub txtAmount_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtAmount.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim dbNumericTemp As Double
		If Not Double.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			txtAmount.Text = CStr(0)
			'  Cancel = True
		End If
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub DisableControls()
		
		cmdOK.Enabled = False
		
		'PSL 03/03/2003 These fields aren't numbered 0 to 5
		'They have the names below
		'    For ii = 0 To 5
		For ii As Integer = 0 To 1
			lblAdjustment(ii).Enabled = False
		Next ii
		lblAdjDate.Enabled = False
		lblAmount.Enabled = False
		lblMethod.Enabled = False
		lblComments.Enabled = False
		
		txtCashDrawer.Enabled = False
		txtAdjDate.Enabled = False
		txtAmount.Enabled = False
		txtComments.Enabled = False
		cboPMUserLookup1.Enabled = False
		cboMethod.Enabled = False
		
	End Sub
	
	Private Function PopulateCombos() As Boolean
		Dim vAdjustMethods(,) As Object
		

		m_lReturn = m_oBusiness.GetAdjustmentMethods(vAdjustMethods)
		
		If Not Information.IsArray(vAdjustMethods) Then
			Return False
		End If
		

		For ii As Integer = 0 To vAdjustMethods.GetUpperBound(1)

            If CStr(vAdjustMethods(1, ii)).Trim() = "CHANGE" And Not m_blnHasCash Then
                'Don't add it as you can't have error with the Change if
                'there isn't any cash!!
            Else
                Dim cboMethod_NewIndex As Integer = -1

                cboMethod_NewIndex = cboMethod.Items.Add(CStr(vAdjustMethods(2, ii)))

                VB6.SetItemData(cboMethod, cboMethod_NewIndex, CInt(vAdjustMethods(0, ii)))
            End If
		Next ii
		
		'    cboPMUserLookup1.SingleUserID = g_iUserID
		'    cboPMUserLookup1.DefaultUserID = g_iUserID
	End Function

    Private Sub frmAdjustment_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub
End Class
