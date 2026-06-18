Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Milan- TODO: Needs discussion
Imports Artinsoft.VB6.Gui
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 07/11/2002
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
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
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lPartyCnt As Integer
	Private m_lLoyaltySchemeId As Integer
	Private m_lPartyLoyaltySchemeID As Integer
	Private m_sLoyaltySchemeName As String = ""
	Private m_sMemberNumber As String = ""
	Private m_sMainMemberNumber As String = ""
	Private m_sOtherRef As String = ""
	Private m_dtStartDate As Date
	Private m_dtEndDate As Date
	Private m_iIsActive As Integer
	
	Private m_vResultArray As Object
	
	Private Const ACTABDetails As Integer = 0
	
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public Property LoyaltySchemeID() As Integer
		Get
			Return m_lLoyaltySchemeId
		End Get
		Set(ByVal Value As Integer)
			m_lLoyaltySchemeId = Value
		End Set
	End Property
	
	Public Property PartyLoyaltySchemeID() As Integer
		Get
			Return m_lPartyLoyaltySchemeID
		End Get
		Set(ByVal Value As Integer)
			m_lPartyLoyaltySchemeID = Value
		End Set
	End Property
	
	Public ReadOnly Property LoyaltySchemeName() As String
		Get
			Return m_sLoyaltySchemeName
		End Get
	End Property
	Public ReadOnly Property MemberNumber() As String
		Get
			Return m_sMemberNumber
		End Get
	End Property
	Public ReadOnly Property MainMemberNumber() As String
		Get
			Return m_sMainMemberNumber
		End Get
	End Property
	Public ReadOnly Property OtherRef() As String
		Get
			Return m_sOtherRef
		End Get
	End Property
	Public ReadOnly Property StartDate() As Date
		Get
			Return m_dtStartDate
		End Get
	End Property
	Public ReadOnly Property EndDate() As Date
		Get
			Return m_dtEndDate
		End Get
	End Property
	Public ReadOnly Property IsActive() As Integer
		Get
			Return m_iIsActive
		End Get
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
		Const ACMethod As String = "GetBusiness"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetDetails(m_lPartyLoyaltySchemeID, m_lPartyCnt, m_lLoyaltySchemeId, m_sMemberNumber, m_sMainMemberNumber, m_sOtherRef, m_dtStartDate, m_dtEndDate, m_iIsActive)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
		Const ACMethod As String = "BusinessToInterface"
		
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface controls.

            ' Assign the details to the interface controls.
            cboPMLookupLoyaltyScheme.ItemId = m_lLoyaltySchemeId

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMemberNumber, vControlValue:=m_sMemberNumber)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMainMemberNumber, vControlValue:=m_sMainMemberNumber)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOtherRef, vControlValue:=m_sOtherRef)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtStartDate, vControlValue:=m_dtStartDate)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEndDate, vControlValue:=m_dtEndDate)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=chkActive, vControlValue:=m_iIsActive)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
		Const ACMethod As String = "InterfaceToBusiness"
		
		' This constant supplement the standard function return constants
		' Not an ideal approach I know but this way more information can be
		' passed back from DirectAdd without adding a new entry for PMEReturnCode enum
		Const ACAlreadyExists As Integer = -1234
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface controlsto the data storage vars.
			m_lLoyaltySchemeId = cboPMLookupLoyaltyScheme.ItemId
			m_sLoyaltySchemeName = cboPMLookupLoyaltyScheme.ItemCaption

			m_sMemberNumber = m_oFormFields.UnformatControl(txtMemberNumber)

			m_sMainMemberNumber = m_oFormFields.UnformatControl(txtMainMemberNumber)

			m_sOtherRef = m_oFormFields.UnformatControl(txtOtherRef)

			m_dtStartDate = m_oFormFields.UnformatControl(txtStartDate)
			If txtEndDate.Text = "" Then
                m_dtEndDate = #12/30/1899#
			Else

				m_dtEndDate = m_oFormFields.UnformatControl(txtEndDate)
			End If

			m_iIsActive = m_oFormFields.UnformatControl(chkActive)
			
			
			'If we're viewing there's nothing to update or cancel
			If Task = gPMConstants.PMEComponentAction.PMView Then
				Return result
			End If
			
			
			' Validate special conditions.
			m_lReturn = ValidateFormControls()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Inform the business object with a new data item.
					
					' {* USER DEFINED CODE (Begin) *}

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.DirectAdd(m_lPartyLoyaltySchemeID, m_lPartyCnt, m_lLoyaltySchemeId, m_sMemberNumber, m_sMainMemberNumber, m_sOtherRef, m_dtStartDate, m_dtEndDate, m_iIsActive)
					
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
					
					' {* USER DEFINED CODE (Begin) *}

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.DirectEdit(m_lPartyLoyaltySchemeID, m_lPartyCnt, m_lLoyaltySchemeId, m_sMemberNumber, m_sMainMemberNumber, m_sOtherRef, m_dtStartDate, m_dtEndDate, m_iIsActive)
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMDelete
					' Inform the business object with a deleted data item.
					
					' {* USER DEFINED CODE (Begin) *}

                    'developer guide no. 37(guide)
                    m_lReturn = m_oBusiness.DirectDelete(m_lPartyLoyaltySchemeID)
					' {* USER DEFINED CODE (End) *}
			End Select
			
			' Check for errors.
			If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) And (m_lReturn = ACAlreadyExists) Then
				result = gPMConstants.PMEReturnCode.PMFalse
				DisplayMessage(ACCaptionLoyaltySchemeTitle, ACCaptionLoyaltySchemeDuplicate, MsgBoxStyle.Critical)
				Return result
				
			ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then 
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Const ACMethod As String = "Initialise"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the instance of the calling List form
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			

            		m_lReturn = SetFieldValidation(Me, m_oFormFields)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        SetMousePointer PMMouseNormal
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: SetControlProperties
	'
	' Description: Sets all of the properties for each of the forms controls .
	'
	' ***************************************************************** '
	Private Function SetControlProperties() As Integer
		
		Dim result As Integer = 0
		Const ACMethod As String = "SetControlProperties"
		
		Dim bMandatoryPartyLoyaltySchemeId, bMandatoryPartyCnt, bMandatoryLoyaltySchemeId, bMandatoryMemberNumber, bMandatoryMainMemberNumber, bMandatoryOtherRef, bMandatoryStartDate, bMandatoryEndDate, bMandatoryIsActive As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' get mandatory status of each control from business object

            ' developer guide no. 37(guide)
            m_lReturn = m_oBusiness.GetMandatorystatus(bMandatoryPartyLoyaltySchemeId, bMandatoryPartyCnt, bMandatoryLoyaltySchemeId, bMandatoryMemberNumber, bMandatoryMainMemberNumber, bMandatoryOtherRef, bMandatoryStartDate, bMandatoryEndDate, bMandatoryIsActive)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
                MainModule.LogFailedCall(MainModule.ACApp, ACClass, ACMethod, "m_oBusiness.GetMandatorystatus")
				Return result
			End If
			
			' Set properties to control  mandatory status of controls
			' but first remove any M; strings that already exist
			' these statements will need to become more sophisticated if tag can ever hold
			' other values that end in M;
			With Me
				If bMandatoryLoyaltySchemeId Then
					.cboPMLookupLoyaltyScheme.Tag = "M;" &  _
					                                Convert.ToString(.cboPMLookupLoyaltyScheme.Tag).Replace("M;", "")
				End If
				If bMandatoryMemberNumber Then
					.txtMemberNumber.Tag = "M;" &  _
					                       Convert.ToString(.txtMemberNumber.Tag).Replace("M;", "")
				End If
				If bMandatoryMainMemberNumber Then
					.txtMainMemberNumber.Tag = "M;" &  _
					                           Convert.ToString(.txtMainMemberNumber.Tag).Replace("M;", "")
				End If
				If bMandatoryOtherRef Then
					.txtOtherRef.Tag = "M;" &  _
					                   Convert.ToString(.txtOtherRef.Tag).Replace("M;", "")
				End If
				If bMandatoryStartDate Then
					.txtStartDate.Tag = "M;" &  _
					                    Convert.ToString(.txtStartDate.Tag).Replace("M;", "")
				End If
				If bMandatoryEndDate Then
					.txtEndDate.Tag = "M;" &  _
					                  Convert.ToString(.txtEndDate.Tag).Replace("M;", "")
				End If
				If bMandatoryIsActive Then
					.chkActive.Tag = "M;" &  _
					                 Convert.ToString(.chkActive.Tag).Replace("M;", "")
				End If
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
		Const ACMethod As String = "SetInterfaceDefaults"
		
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'Milan-TODO: needs discussion
            ' Display all language specific captions.
            'm_lReturn = iPMForms.DisplayCaptions(Me)

            m_lReturn = Me.DisplayCaptions(Me)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                cboPMLookupLoyaltyScheme.Enabled = False
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                chkActive.CheckState = CheckState.Checked
            End If

            '    ' Set the status of the Navigate button.
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
            '    uctType.ListIndex = 0
            '    txtDate.Text = FormatField( _
            ''                        iFormatType:=PMFormatDateLong, _
            ''                        vFieldValue:=Now)
            '
            '   'Setup default data for Add
            '   If (m_iTask% = PMAdd) Then
            '       cmdPopulate.Enabled = True
            '       uctType.ListIndex = 0
            '   Else
            '       uctType.ListIndex = 1
            '   End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
		Const ACMethod As String = "SetFirstLastControls"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			
			m_ctlTabFirstLast = Array.CreateInstance(GetType(Control), New Integer(){ACControlEnd - ACControlStart + 1, ACTABDetails - ACTABDetails + 1}, New Integer(){ACControlStart, ACTABDetails})
			
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
			
            m_ctlTabFirstLast(MainModule.ACControlStart, ACTABDetails) = cboPMLookupLoyaltyScheme
            m_ctlTabFirstLast(MainModule.ACControlEnd, ACTABDetails) = chkActive
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ValidateFormControls
	'
	' Description: Contains any special validation rules not catered for by
	'              iPMFormControl (ie mandatory, data type etc)
	'
	' ***************************************************************** '
	Private Function ValidateFormControls() As Integer
		
		Dim result As Integer = 0
		Const ACMethod As String = "ValidateFormControls"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Ensure that End date > start date when it exists
			If (m_dtStartDate <> #12/30/1899#) And (m_dtEndDate <> #12/30/1899#) Then
				
				If m_dtEndDate < m_dtStartDate Then
					
					result = gPMConstants.PMEReturnCode.PMFalse
                    MainModule.DisplayMessage(MainModule.ACCaptionInvalidDateTitle, MainModule.ACCaptionStartEndDateMismatch, MsgBoxStyle.Critical)
					txtEndDate.Focus()
					Return result
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate form controls", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	
	' PRIVATE Functions (End)
	
	
	
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		Const ACMethod As String = "Form_Initialize"
		
		Dim sMessage, sTitle As String
		
		' Forms initialise event.
		Try 
            'developer guide no. 220(latest guide)
            cboPMLookupLoyaltyScheme.FirstItem = ""
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyLoyaltyScheme.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                ''developer guide no. 243
                sTitle = CStr(iPMFunc.GetResData(g_iLanguageID, MainModule.ACCaptionBusinessFailTitle, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                ''developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MainModule.ACCaptionBusinessFail, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
        Const ACMethod As String = "Form_Load"
		
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

            'developer guide no. 37(guide)
            m_lReturn = m_oBusiness.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
				
				Exit Sub
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMDelete Then
				' form is not to be displayed so action the delete straightaway
				' then get out of here
				m_lStatus = gPMConstants.PMEReturnCode.PMOK
				

                'developer guide no. 37(guide)
                m_lReturn = m_oBusiness.DirectDelete(m_lPartyLoyaltySchemeID)
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to action delete", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod)
				End If
			End If
			
			' If we have reached here then we are NOT processing a delete
			
			' Set the properties of each of the form's controls.
			m_lReturn = SetControlProperties()
			
			'Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			' Set the rules for validating fields.

            m_lReturn = SetFieldValidation(Me, m_oFormFields)
			
			'Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
    End Sub

    Public Sub FrmInterfaceLoad()
        frmInterface_Load(Nothing, Nothing)
    End Sub
	
    Private Const vbFormCode As Integer = 0
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Const ACMethod As String = "Form_QueryUnload"
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                ' For now it is assumed that the user has changed something on the form (ie = TRUE).
                ' Extra code needs to be added to detect any changes
                m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=True)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try
		
	End Sub
	
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Const ACMethod As String = "cmdOK_Click"
		
		Try 
			
			' Initialise the error number value.?????
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Check mandatory controls have been entered into.

			m_lReturn = m_oFormFields.CheckMandatoryControls()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Process the next set of actions depending
			' upon the interface task etc.
			' For now it is assumed that the user has changed something on the form (ie = TRUE).
			' Extra code needs to be added to detect any changes
			m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=True)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			Else
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Const ACMethod As String = "cmdCancel_Click"
		
		' Click event of the Cancel button.
		
		Try 
			
			' Initialise the error number value.????
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions depending
			' upon the interface task etc.
			' For now it is assumed that the user has changed something on the form (ie = TRUE).
			' Extra code needs to be added to detect any changes
			m_lReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=True)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			Else
				' a reply of No to the Cancel question will return PMFalse - dont really understand why though ?
				' Such a situation is not an error
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	' PRIVATE Events (End)
	Private Sub cboPMLookupLoyaltyScheme_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupLoyaltyScheme.GotFocus

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(cboPMLookupLoyaltyScheme)
	End Sub
	
	Private Sub cboPMLookupLoyaltyScheme_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupLoyaltyScheme.LostFocus

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(cboPMLookupLoyaltyScheme)
	End Sub
	
	Private Sub txtMemberNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMemberNumber.Enter

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(txtMemberNumber)
	End Sub
	
	Private Sub txtMemberNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMemberNumber.Leave

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(txtMemberNumber)
		If txtMainMemberNumber.Text = "" Then
			txtMainMemberNumber.Text = txtMemberNumber.Text
		End If
	End Sub
	
	Private Sub txtMainMemberNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMainMemberNumber.Enter

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(txtMainMemberNumber)
	End Sub
	
	Private Sub txtMainMemberNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMainMemberNumber.Leave

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(txtMainMemberNumber)
	End Sub
	
	Private Sub txtOtherRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOtherRef.Enter

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(txtOtherRef)
	End Sub
	
	Private Sub txtOtherRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOtherRef.Leave

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(txtOtherRef)
	End Sub
	
	Private Sub txtStartDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.Enter

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(txtStartDate)
	End Sub
	
	Private Sub txtStartDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStartDate.Leave

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(txtStartDate)
	End Sub
	
	Private Sub txtEndDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.Enter

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(txtEndDate)
	End Sub
	
	Private Sub txtEndDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEndDate.Leave

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(txtEndDate)
	End Sub
	
	Private Sub chkActive_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkActive.Enter

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.GotFocus(chkActive)
	End Sub
	
	Private Sub chkActive_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkActive.Leave

        'developer guide no. 37(guide)
        m_lReturn = m_oFormFields.LostFocus(chkActive)
    End Sub

    'Milan- TODO: This needs discussion. Severity- Critical
    ' Descirtion: Some methods like DisplayCaptions, GetResData are included in this
    ' file itself.
    ' ***************************************************************** '
    ' Name:         DisplayCaptions
    '
    ' Description:  Display all language specific captions.
    ' Histroy:      TR22112002 - TR23 Changed parameter from Form to Object
    '               to support UserControls as well as Forms. Explicitly
    '               named the assumed default properties.
    ' ***************************************************************** '
    Public Function DisplayCaptions(ByRef r_frmSource As Form) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCaptions
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 14 October 2002, 15:21:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim iSkip As Integer
        'Used in the .Tag property of ListView.CoulmnHeading
        Const ksHidden As String = "HIDDEN"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display all language specific captions.


        'Get the caption from the tag
        lCaptionID = GetCaptionID(Convert.ToString(r_frmSource.Tag))
        If lCaptionID > 0 Then
            'Get the caption from the res file using Id from tag property

            r_frmSource.Text = CStr(iPMFunc.GetResData(g_iLanguageID, lCaptionID, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            ' Check for an error.
            If r_frmSource.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", ACApp, ACClass, "DisplayCaptions")

                Return result
            End If
        End If



        For i As Integer = 0 To ContainerHelper.Controls(r_frmSource).Count - 1

            lCaptionID = GetCaptionID(Convert.ToString(ControlHelper.GetTag(ContainerHelper.Controls(r_frmSource)(i))))
            If lCaptionID > 0 Then


                Select Case ContainerHelper.Controls(r_frmSource)(i).GetType().Name
                    Case "SSTab"


                        For j As Integer = 0 To ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Tabs") - 1


                            ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "TabCaption", New Object() {j}, iPMFunc.GetResData(g_iLanguageID, lCaptionID + j, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                        Next j
                        'Added ListView column headers
                    Case "ListView"
                        iSkip = 0


                        For j As Integer = 1 To ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders"), "Count")
                            'Test for hidden ListView columns and skip as appropriate


                            If CStr(ReflectionHelper.GetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders", New Object() {j}), "Tag")).IndexOf(ksHidden) >= 0 Then
                                iSkip += 1
                            Else
                                'PWC - 16/10/2002 - No need to get caption if skipping this time



                                ReflectionHelper.SetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders", New Object() {j}), "Text", iPMFunc.GetResData(g_iLanguageID, lCaptionID + j - iSkip - 1, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            End If
                        Next j
                        'Added Picklist
                    Case "PickList"



                        ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "AvailableCaption", iPMFunc.GetResData(g_iLanguageID, lCaptionID, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                    Case Else



                        ContainerHelper.Controls(r_frmSource)(i).Text = CStr(iPMFunc.GetResData(g_iLanguageID, lCaptionID, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                End Select
            End If
        Next i

	Return result

        
        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "", ACApp, ACClass, "DisplayCaptions", Information.Err().Number, Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse

		Return result
        End Select

        Finally
        

        End Try
		Return result
    End Function

End Class
