Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmList
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmList
	'
	' Date: 11th July 1997
	'
	' Description: Main interface. Shows List of available details.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmList"
	
    'Developer Guide No.50
    Dim frmDetails As frmDetails
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As gPMConstants.PMEReturnCode
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_sNavigatorTitle As String = ""
	' Stores the last business data ID used
	Private m_lLastDataID As Integer
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the Business object.

	Private m_oBusiness As bACTCompany.Form
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0
	' to Return to caller
	Private m_iSelectedCompanyID As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast() As Control
	
	' Stores the List data from the business object.
	Public m_vListData( ,  ) As Object
	' PRIVATE Data Members (End)
	Public Property SelectedCompanyID() As Integer
		Get
			Return m_iSelectedCompanyID
		End Get
		Set(ByVal Value As Integer)
			m_iSelectedCompanyID = Value
		End Set
	End Property
	
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	Public ReadOnly Property NavigatorTitle() As String
		Get
			
			' Return the objects parameter value.
			Return m_sNavigatorTitle
			
		End Get
	End Property
	' PRIVATE Property Procedures (End)
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Initialise the form
	'
	' ***************************************************************** '
	Public Function Initialise() As gPMConstants.PMEReturnCode



		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim sMessage, sTitle As String
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCompany.Form", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				' Get description from the resource file.

				sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Load (Standard Method)
	'
	' Description: Load the form details
	'
	' ***************************************************************** '
	Public Function Load_Renamed() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the process modes for the busines object.

			m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Gets the interface details to be displayed.
			m_lReturn = GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: ShowForm (Standard Method)
	'
	' Description: Show the form using the display state passed
	'
	' ***************************************************************** '
	Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Show the the form, allow user input etc.
			VB6.ShowForm(Me, lDisplayState)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	' ***************************************************************** '
	' Name: GetInterfaceDetails
	'
	' Description: Gets the interface details.
	'
	' ***************************************************************** '
	Private Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the interface details from the
			' business object.
			m_lReturn = GetBusiness()
			
			' If we have some records populate array and list view
			If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to get the details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Assign the details from the business object
				' to the List data storage.
				m_lReturn = BusinessToData()
				
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to assign the details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Assign the details from the List data storage
				' to the interface.
				m_lReturn = DataToInterface()
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			'    ' Probably not needed by the List Form
			'    ' Display all of the lookup details.
			'    m_lReturn& = DisplayLookupDetails()
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        GetInterfaceDetails = PMFalse
			'        Exit Function
			'    End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Private Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			

			m_lReturn = m_oBusiness.GetDetails()
			
			' {* USER DEFINED CODE (End) *}
			
			' If no records found return NotFound
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				Return gPMConstants.PMEReturnCode.PMNotFound
			End If
			
			' Check for other errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
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
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		' {* USER DEFINED CODE (Begin) *}
		Dim result As Integer = 0
		Dim iCompanyID, iBaseCurrency As Integer
		Dim sCode As New FixedLengthString(10)
		Dim sDescription As New FixedLengthString(255)
		Dim lCaptionID As Integer
		Dim iParentID As Integer
		Dim sRegNo1 As New FixedLengthString(30)
		Dim sRegNo2 As New FixedLengthString(30)
		Dim sAddress1 As New FixedLengthString(40)
		Dim sAddress2 As New FixedLengthString(40)
		Dim sAddress3 As New FixedLengthString(40)
		Dim sAddress4 As New FixedLengthString(40)
		Dim sPostalCode As New FixedLengthString(20)
		Dim iCountryID As Integer
		Dim sPhoneAreaCode As New FixedLengthString(10)
		Dim sPhoneNumber As New FixedLengthString(15)
		Dim sPhoneExtension As New FixedLengthString(6)
		Dim sFaxAreaCode As New FixedLengthString(10)
		Dim sFaxNumber As New FixedLengthString(15)
		Dim sFaxExtension As New FixedLengthString(6)
		
		' DC 31/01/00
		Dim sEmail As New FixedLengthString(50)
		Dim sVatNo As New FixedLengthString(20)
		Dim sSenderMailboxId As New FixedLengthString(14)
		Dim sBrokerABIId As New FixedLengthString(6)
		Dim iUserLicenceId, iPMCompanyNumber As Integer
		Dim sDefaultIndicator As New FixedLengthString(1)
		
		' {* USER DEFINED CODE (End) *}
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage to be
			' used with the List.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Initialise the data array (Allow one extra field
			' for the unique key to been assigned).
			' *** This also needs to be edited in DetailsFormReturn ***
			' DC 31/01/00 was 20
			ReDim m_vListData(30, 0)
			
			' Retrieve all of the details from the business object.
			' DC 31/01/00
			' added email, vat no, sender mailbox id, broker abi id,
			' user licence id, pm company number and default indicator

			While m_oBusiness.GetNext(vCompanyID:=iCompanyID, vBaseCurrency:=iBaseCurrency, vCode:=sCode.Value, vDescription:=sDescription.Value, vCaptionID:=lCaptionID, vParentID:=iParentID, vRegNo1:=sRegNo1.Value, vRegNo2:=sRegNo2.Value, vAddress1:=sAddress1.Value, vAddress2:=sAddress2.Value, vAddress3:=sAddress3.Value, vAddress4:=sAddress4.Value, vPostalCode:=sPostalCode.Value, vCountryID:=iCountryID, vPhoneAreaCode:=sPhoneAreaCode.Value, vPhoneNumber:=sPhoneNumber.Value, vPhoneExtension:=sPhoneExtension.Value, vFaxAreaCode:=sFaxAreaCode.Value, vFaxNumber:=sFaxNumber.Value, vFaxExtension:=sFaxExtension.Value, vEmail:=sEmail.Value, vVatNo:=sVatNo.Value, vSenderMailboxId:=sSenderMailboxId.Value, vBrokerABIId:=sBrokerABIId.Value, vUserLicenceId:=iUserLicenceId, vPMCompanyNumber:=iPMCompanyNumber, vDefaultIndicator:=sDefaultIndicator.Value) = gPMConstants.PMEReturnCode.PMTrue
				
				' Store all of the data.
				
				' ************************************************************
				' Enter your code here to assign all of the details from the
				' business object to the List data storage
				'
				' Example:-
				'
				'    m_vListData(0, UBound(m_vListData, 2)) = m_DName$
				'
				' NOTE: Replace this section with your new code.
				' ************************************************************
				
				m_vListData(ACSubCompanyID, m_vListData.GetUpperBound(1)) = iCompanyID
				m_vListData(ACSubBaseCurrency, m_vListData.GetUpperBound(1)) = iBaseCurrency
				m_vListData(ACSubCode, m_vListData.GetUpperBound(1)) = sCode.Value
				m_vListData(ACSubDescription, m_vListData.GetUpperBound(1)) = sDescription.Value
				m_vListData(ACSubCaptionID, m_vListData.GetUpperBound(1)) = lCaptionID
				m_vListData(ACSubParentID, m_vListData.GetUpperBound(1)) = iParentID
				m_vListData(ACSubRegNo1, m_vListData.GetUpperBound(1)) = sRegNo1.Value
				m_vListData(ACSubRegNo2, m_vListData.GetUpperBound(1)) = sRegNo2.Value
				m_vListData(ACSubAddress1, m_vListData.GetUpperBound(1)) = sAddress1.Value
				m_vListData(ACSubAddress2, m_vListData.GetUpperBound(1)) = sAddress2.Value
				m_vListData(ACSubAddress3, m_vListData.GetUpperBound(1)) = sAddress3.Value
				m_vListData(ACSubAddress4, m_vListData.GetUpperBound(1)) = sAddress4.Value
				m_vListData(ACSubPostalCode, m_vListData.GetUpperBound(1)) = sPostalCode.Value
				m_vListData(ACSubCountryID, m_vListData.GetUpperBound(1)) = iCountryID
				m_vListData(ACSubPhoneAreaCode, m_vListData.GetUpperBound(1)) = sPhoneAreaCode.Value
				m_vListData(ACSubPhoneNumber, m_vListData.GetUpperBound(1)) = sPhoneNumber.Value
				m_vListData(ACSubPhoneExtension, m_vListData.GetUpperBound(1)) = sPhoneExtension.Value
				m_vListData(ACSubFaxAreaCode, m_vListData.GetUpperBound(1)) = sFaxAreaCode.Value
				m_vListData(ACSubFaxNumber, m_vListData.GetUpperBound(1)) = sFaxNumber.Value
				m_vListData(ACSubFaxExtension, m_vListData.GetUpperBound(1)) = sFaxExtension.Value
				' DC 31/01/00
				m_vListData(ACSubEmail, m_vListData.GetUpperBound(1)) = sEmail.Value
				m_vListData(ACSubVatNo, m_vListData.GetUpperBound(1)) = sVatNo.Value
				m_vListData(ACSubSenderMailboxId, m_vListData.GetUpperBound(1)) = sSenderMailboxId.Value
				m_vListData(ACSubBrokerABIId, m_vListData.GetUpperBound(1)) = sBrokerABIId.Value
				m_vListData(ACSubUserLicenceId, m_vListData.GetUpperBound(1)) = iUserLicenceId
				m_vListData(ACSubPMCompanyNumber, m_vListData.GetUpperBound(1)) = iPMCompanyNumber
				m_vListData(ACSubDefaultIndicator, m_vListData.GetUpperBound(1)) = sDefaultIndicator.Value
				
				' {* USER DEFINED CODE (End) *}
				
				' Store unique key for this row.
				m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1)) = m_vListData.GetUpperBound(1) + 1
				
				' Increment the data array.
				ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) + 1)
			End While
			' Check if we have data in the List array.
			If Information.IsArray(m_vListData) Then
				If m_vListData.GetUpperBound(1) > 0 Then
					' Decrement the data array.
					ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) - 1)
				End If
			End If
			
			' Store the last Data ID used by the business
			m_lLastDataID = m_vListData.GetUpperBound(1) + 1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToBusiness
	'
	' Description: Updates all business members from the data storage.
	'
	' ***************************************************************** '
	Private Function DataToBusiness(ByRef lMode As Integer, ByRef lRow As Integer) As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		
		' {* USER DEFINED CODE (Begin) *}
		Dim iCompanyID, iBaseCurrency As Integer
		Dim sCode As New FixedLengthString(10)
		Dim sDescription As New FixedLengthString(255)
		Dim lCaptionID As Integer
		Dim iParentID As Integer
		Dim sRegNo1 As New FixedLengthString(30)
		Dim sRegNo2 As New FixedLengthString(30)
		Dim sAddress1 As New FixedLengthString(40)
		Dim sAddress2 As New FixedLengthString(40)
		Dim sAddress3 As New FixedLengthString(40)
		Dim sAddress4 As New FixedLengthString(40)
		Dim sPostalCode As New FixedLengthString(20)
		Dim iCountryID As Integer
		Dim sPhoneAreaCode As New FixedLengthString(10)
		Dim sPhoneNumber As New FixedLengthString(15)
		Dim sPhoneExtension As New FixedLengthString(6)
		Dim sFaxAreaCode As New FixedLengthString(10)
		Dim sFaxNumber As New FixedLengthString(15)
		Dim sFaxExtension As New FixedLengthString(6)
		' DC 31/01/00
		Dim sEmail As New FixedLengthString(50)
		Dim sVatNo As New FixedLengthString(20)
		Dim sSenderMailboxId As New FixedLengthString(14)
		Dim sBrokerABIId As New FixedLengthString(6)
		Dim iUserLicenceId, iPMCompanyNumber As Integer
		Dim sDefaultIndicator As New FixedLengthString(1)
		
		' {* USER DEFINED CODE (End) *}
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details from the data storage to
			' the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Store all of the displayable data.
			
			' ************************************************************
			' Enter your code here to assign all of the details
			' from the List data array to the data storage.
			'
			' Example:-
			'
			'    m_DName$ = m_vListData(0, lRow&)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			iCompanyID = CInt(m_vListData(ACSubCompanyID, lRow))
            iBaseCurrency = CInt(m_vListData(ACSubBaseCurrency, lRow))
            'Developer guide No.236
            'sCode.Value = CType(m_vListData(ACSubCode, lRow), FixedLengthString)
            sCode.Value = m_vListData(ACSubCode, lRow)
            'Developer guide No.236
            'sDescription.Value = CType(m_vListData(ACSubDescription, lRow), FixedLengthString)
            sDescription.Value = m_vListData(ACSubDescription, lRow)
			lCaptionID = CInt(m_vListData(ACSubCaptionID, lRow))
			iParentID = CInt(m_vListData(ACSubParentID, lRow))
            '         'Developer guide No.236
            sRegNo1.Value = m_vListData(ACSubRegNo1, lRow)
            sRegNo2.Value = m_vListData(ACSubRegNo2, lRow)
            sAddress1.Value = m_vListData(ACSubAddress1, lRow)
            sAddress2.Value = m_vListData(ACSubAddress2, lRow)
            sAddress3.Value = m_vListData(ACSubAddress3, lRow)
            sAddress4.Value = m_vListData(ACSubAddress4, lRow)
            sPostalCode.Value = m_vListData(ACSubPostalCode, lRow)
            iCountryID = CInt(m_vListData(ACSubCountryID, lRow))
            'Developer guide No.236
            sPhoneAreaCode.Value = m_vListData(ACSubPhoneAreaCode, lRow)
            sPhoneNumber.Value = m_vListData(ACSubPhoneNumber, lRow)
            sPhoneExtension.Value = m_vListData(ACSubPhoneExtension, lRow)
            sFaxAreaCode.Value = m_vListData(ACSubFaxAreaCode, lRow)
            sFaxNumber.Value = m_vListData(ACSubFaxNumber, lRow)
            sFaxExtension.Value = m_vListData(ACSubFaxExtension, lRow)
            sEmail.Value = m_vListData(ACSubEmail, lRow)
            sVatNo.Value = m_vListData(ACSubVatNo, lRow)
            sSenderMailboxId.Value = m_vListData(ACSubSenderMailboxId, lRow)
            sBrokerABIId.Value = m_vListData(ACSubBrokerABIId, lRow)

			iUserLicenceId = CInt(m_vListData(ACSubUserLicenceId, lRow))
			iPMCompanyNumber = CInt(m_vListData(ACSubPMCompanyNumber, lRow))
            sDefaultIndicator.Value = m_vListData(ACSubDefaultIndicator, lRow)
			' {* USER DEFINED CODE (End) *}
			
			' Store unique key for this row.
			lBusinessDataID = CInt(m_vListData(m_vListData.GetUpperBound(0), lRow))
			
			' Check the task.
			Select Case (lMode)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Inform the business object with a new data item.
					
					' {* USER DEFINED CODE (Begin) *}
					
					' DC 31/01/00
					' added email, vat no, sender mailbox id, broker abi id,
					' user licence id, pm company number, default indicator

					m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vCompanyID:=iCompanyID, vBaseCurrency:=iBaseCurrency, vCode:=sCode.Value, vDescription:=sDescription.Value, vCaptionID:=lCaptionID, vParentID:=iParentID, vRegNo1:=sRegNo1.Value, vRegNo2:=sRegNo2.Value, vAddress1:=sAddress1.Value, vAddress2:=sAddress2.Value, vAddress3:=sAddress3.Value, vAddress4:=sAddress4.Value, vPostalCode:=sPostalCode.Value, vCountryID:=iCountryID, vPhoneAreaCode:=sPhoneAreaCode.Value, vPhoneNumber:=sPhoneNumber.Value, vPhoneExtension:=sPhoneExtension.Value, vFaxAreaCode:=sFaxAreaCode.Value, vFaxNumber:=sFaxNumber.Value, vFaxExtension:=sFaxExtension.Value, vEmail:=sEmail.Value, vVatNo:=sVatNo.Value, vSenderMailboxId:=sSenderMailboxId.Value, vBrokerABIId:=sBrokerABIId.Value, vUserLicenceId:=iUserLicenceId, vPMCompanyNumber:=iPMCompanyNumber, vDefaultIndicator:=sDefaultIndicator.Value)
					
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
					
					' {* USER DEFINED CODE (Begin) *}
					
					' DC 31/01/00
					' added email, vat no, sender mailbox id, broker abi id,
					' user licence id, pm company number, default indicator

					m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vCompanyID:=iCompanyID, vBaseCurrency:=iBaseCurrency, vCode:=sCode.Value, vDescription:=sDescription.Value, vCaptionID:=lCaptionID, vParentID:=iParentID, vRegNo1:=sRegNo1.Value, vRegNo2:=sRegNo2.Value, vAddress1:=sAddress1.Value, vAddress2:=sAddress2.Value, vAddress3:=sAddress3.Value, vAddress4:=sAddress4.Value, vPostalCode:=sPostalCode.Value, vCountryID:=iCountryID, vPhoneAreaCode:=sPhoneAreaCode.Value, vPhoneNumber:=sPhoneNumber.Value, vPhoneExtension:=sPhoneExtension.Value, vFaxAreaCode:=sFaxAreaCode.Value, vFaxNumber:=sFaxNumber.Value, vFaxExtension:=sFaxExtension.Value, vEmail:=sEmail.Value, vVatNo:=sVatNo.Value, vSenderMailboxId:=sSenderMailboxId.Value, vBrokerABIId:=sBrokerABIId.Value, vUserLicenceId:=iUserLicenceId, vPMCompanyNumber:=iPMCompanyNumber, vDefaultIndicator:=sDefaultIndicator.Value)
					
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMDelete
					' Inform the business object with a deleted data item.

					m_lReturn = m_oBusiness.EditDelete(lRow:=lBusinessDataID)
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the data details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToInterface
	'
	' Description: Updates all interface details from the List data.
	'              storage.
	'
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		
        'Const ACListImage As String = "CompanyImage"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Clear the List details.
			lvwListDetails.Items.Clear()
			
			' Disable the Remove button by default
			' Enabled in List.ItemClick when a deletable item is clicked
			cmdRemove.Enabled = False
			
			' Check that List details are present before
			' continuing.
			If Not Information.IsArray(m_vListData) Then
				' No details so disable Edit
				cmdEdit.Enabled = False
				Return result
			End If
			
			' We have some details so we can enable Edit
			cmdEdit.Enabled = True
			
			' Assign the details to the interface.
			
			For lRow As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
				
				' {* USER DEFINED CODE (Begin) *}
				
				' ************************************************************
				' Enter your code here to assign the all of the interface
				' details from the List data storage, using the FormatField
				' function for any type conversion.
				'
				' Example:-
				'
				'    ' Assign the details to the first column.
				'    Set oListItem = lvwListDetails.ListItems.Add(, , _
				''        Trim$(m_vListData(ACName, lRow&)), , ACListImage)
				'
				'    ' Assign details to other the columns
				'    oListItem.SubItems(1) = Trim$(m_vListData(ACCode, lRow&))
				'
				'
				' NOTE: Replace this section with your new code.
				' ************************************************************

				oListItem = lvwListDetails.Items.Add(CStr(m_vListData(ACSubDescription, lRow)).Trim(), "")
				
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vListData(ACSubCode, lRow)).Trim()
				
				' {* USER DEFINED CODE (End) *}
				
				' Set the tag property with the index of
				' the List data storage.
				oListItem.Tag = CStr(lRow)
			Next lRow
			
			' Select the first item.
			lvwListDetails.FocusedItem = lvwListDetails.Items.Item(0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the List data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	'UPGRADE_NOTE: (7001) The following declaration (DisplayLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function DisplayLookupDetails() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Get the lookup values.
			'
			'm_lReturn = GetLookupValues()
			'
			' Check for errors.
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'
			' Get all of the lookup details.
			'
			' {* USER DEFINED CODE (Begin) *}
			'
			' ************************************************************
			' Enter your code here to retreive all of the lookup
			' descriptions for a given lookup type.
			' The GetLookupDetails function will allow you to do this.
			''
			' Example:-
			''
			'    m_lReturn& = GetLookupDetails( _
			''        sLookupTable:=PMLookupCodeName, _
			''        ctlLookup:=cmbCodeName)
			''
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        DisplayLookupDetails = PMFalse
			'        Exit Function
			'    End If
			''
			' NOTE: Replace this section with your new code.
			' ************************************************************
			'
			' {* USER DEFINED CODE (End) *}
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
			Select Case m_iTask
				Case gPMConstants.PMEComponentAction.PMView
					cmdAdd.Visible = False
					cmdRemove.Visible = False
					cmdEdit.Visible = False
				Case Else
					cmdAdd.Visible = True
					cmdRemove.Visible = True
					cmdEdit.Visible = True
					
			End Select
			
			' Disable the Edit button by default.
			' Enabled in DataToInterface when know we have something to edit
			cmdEdit.Enabled = False
			
			' Disable the Remove button by default
			' Enabled in List.ItemClick when a deletable item is clicked
			cmdRemove.Enabled = False
			
			' {* USER DEFINED CODE (Begin) *}
			' Set the column widths for the List headers.
			lvwListDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2030))
			lvwListDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2030))
			
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
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdRemove.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACRemoveButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			SSTabHelper.SetSelectedIndex(tabMainTab, 0)
			


			lvwListDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			


			lvwListDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Get all of the lookup values.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Get all of the lookup values with the correct
					' effective date.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					
				Case gPMConstants.PMEComponentAction.PMView
					' Get lookup values for viewing only.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			End Select
			
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


				'ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(ACDetailKey, lCntr)
				'
				' Check if this is the selected index.
				'If CBool(CStr((CStr(m_vLookupValues(ACValueID, lRow))) = CStr(m_vLookupDetails(ACDetailKey, lCntr)))) Then


					'ctlLookup.ListIndex = ctlLookup.NewIndex
				'End If
			'Next lCntr
			'
			' If nothing yet selected (index = -1)
			' select first item (index = 0)

			'If ctlLookup.ListIndex < 0 Then

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
	' Name: ProcessCommand
	'
	' Description: Determines which action to take on the details
	'              depending upon the task and interface state.
	'
	' ***************************************************************** '
	Private Function ProcessCommand() As Integer
		
		Dim result As Integer = 0
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check the task.
			Select Case m_iTask
				Case gPMConstants.PMEComponentAction.PMView
					' Return the selection to the caller
					If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
						SelectedCompanyID = CInt(m_vListData(ACSubCompanyID, Convert.ToString(lvwListDetails.FocusedItem.Tag)))
					Else
						SelectedCompanyID = 0
					End If
				Case gPMConstants.PMEComponentAction.PMEdit
					' Check if form has been cancelled, if so,
					' check if the details have changed and if
					' so, prompt if they wish to cancel.
					If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
						' Check the details havn't changed.

						m_lReturn = m_oBusiness.Cancel()
						'MH Request - Force Cancellation Screen
						'                If (m_lReturn& = PMDataChanged) Then
						' Get string messages
						

						sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
						

						sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
						
						iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
						
						' Check message result.
						If iMsgResult = System.Windows.Forms.DialogResult.No Then
							' Set return to false, meaning
							' don't cancel.
							result = gPMConstants.PMEReturnCode.PMFalse
						End If
						'               End If
					Else
						' Update the details using the business object.

						m_lReturn = m_oBusiness.Update()
						
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							' Failed to update the details
							result = gPMConstants.PMEReturnCode.PMFalse
							
							' Log Error.
							iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
						End If
					End If
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DetailsFormProcess (Standard Method)
	'
	' Description: Display details form and Add, Edit or View
	'
	' ***************************************************************** '
	Private Function DetailsFormProcess(ByRef iTaskType As Integer) As Integer
		
		Dim result As Integer = 0
		Dim lRow As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Call the initialise method passing a business ref
			m_lReturn = frmDetails.Initialise(oBusiness:=m_oBusiness)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				frmDetails = Nothing
				Return result
			End If
			
			' Pass standard details to form properties
			With frmDetails
				.CallingAppName = m_sCallingAppName
				.Task = iTaskType
				.Navigate = m_lNavigate
				.ProcessMode = m_lProcessMode
				.TransactionType = m_sTransactionType
				.EffectiveDate = m_dtEffectiveDate
				' {* USER DEFINED CODE (Begin) *}
				' {* USER DEFINED CODE (End) *}
			End With
			
			' If Edit or View populate Details Form properties
			' from List data array
			Select Case (iTaskType)
				Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
					' Set the row to the selected item
					lRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)
					' Populate form properties
					m_lReturn = DataToDetailsForm(lRow:=lRow)
					' Check for errors.
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
					
					' Inform the business of the current record
					' so that the lookup ID match works

					m_oBusiness.CurrentRecord = m_vListData(m_vListData.GetUpperBound(0), lRow)
					
				Case gPMConstants.PMEComponentAction.PMAdd
					' Set the row to zero
					lRow = 0
					
				Case Else
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Case condition", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess")
					
			End Select
			
			' Call the Load method to setup the interface details
			m_lReturn = frmDetails.Load_Renamed()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				frmDetails = Nothing
				Return result
			End If
			
			' Call the ShowForm method to show the form, allow user input etc.
			m_lReturn = frmDetails.ShowForm(lDisplayState:=FormShowConstants.Modal)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				frmDetails = Nothing
				Return result
			End If
			
			' If OK was pressed and Edit or Add return List data
			' and update Business from Details Form properties
			If frmDetails.Status = gPMConstants.PMEReturnCode.PMOK Then
				Select Case (iTaskType)
					Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
						m_lReturn = DetailsFormReturn(lRow:=lRow, iTaskType:=iTaskType)
						' Check for errors.
						If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
							result = gPMConstants.PMEReturnCode.PMFalse
							frmDetails.Close()
							frmDetails = Nothing
							Return result
						End If
				End Select
			End If
			
			frmDetails.Close()
			frmDetails = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: DetailsDelete (Standard Method)
	'
	' Description: Deletes a newly added set of details
	'
	' ***************************************************************** '
	Private Function DetailsDelete() As Integer
		
		Dim result As Integer = 0
		Dim lSelRow As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If item has non zero ID then it has already been committed to the DB
			' so we won't allow a delete
			' This can be removed to allow deletion of committed records
			If CDbl(m_vListData(ACSubCompanyID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) <> 0 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the row to the selected item
			lSelRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)
			
			' Check the row is in range
			If lSelRow < 0 Or lSelRow > m_vListData.GetUpperBound(1) Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Delete details from Business object
			m_lReturn = DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMDelete, lRow:=lSelRow)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' If array has only one entry(ie 0) delete the array
			' otherwise delete the entry
			If m_vListData.GetUpperBound(1) = 0 Then
				m_vListData = Nothing
			Else
				' Shift the data in the data array to fill the empty space
				' vacated by the deleted row.
				For lRow As Integer = lSelRow To m_vListData.GetUpperBound(1) - 1
					For iCol As Integer = 0 To m_vListData.GetUpperBound(0)
						m_vListData(iCol, lRow) = m_vListData(iCol, lRow + 1)
					Next iCol
				Next lRow
				
				' Resize array to free storage space used by
				' deleted row.
				ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) - 1)
			End If
			
			' Refresh contents of List box
			m_lReturn = DataToInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete details", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DetailsFormReturn (Standard Method)
	'
	' Description: Return List data and update Business
	'              from Details Form properties
	'
	' ***************************************************************** '
	Private Function DetailsFormReturn(ByRef lRow As Integer, ByRef iTaskType As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If Add expand List array to take new entry
			If iTaskType = gPMConstants.PMEComponentAction.PMAdd Then
				
				' If the array exists increment it
				' otherwise must be the first entry so set it up
				If Information.IsArray(m_vListData) Then
					ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) + 1)
				Else
					' DC 31/01/00 was 20
					ReDim m_vListData(26, 0)
				End If
				
				' Store unique key for this row used to point to business collection.
				' This must always be last key added + 1, as deletion does not actually
				' remove the record from business collection it just flags for deletion
				m_lLastDataID += 1
				m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1)) = m_lLastDataID
				
				' Set row to point to newly added List data array row
				lRow = m_vListData.GetUpperBound(1)
				
			End If
			
			' Populate List array from Details Form properties
			m_lReturn = DetailsFormToData(lRow:=lRow)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Update Business from List array
			m_lReturn = DataToBusiness(lMode:=iTaskType, lRow:=lRow)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Refresh contents of List box
			m_lReturn = DataToInterface()
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Reselect the current item.
			lvwListDetails.FocusedItem = lvwListDetails.Items.Item(lRow)
			
			' If item has zero ID then it has not been committed to the DB
			' so we can allow Remove otherwise disable
			' This code also in List.ItemClick
			If CDbl(m_vListData(ACSubCompanyID, lRow)) = 0 Then
				' Enable the Remove button
				cmdRemove.Enabled = True
			Else
				' Disable the Remove button
				cmdRemove.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update data from details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormReturn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToDetailsForm (Standard Method)
	'
	' Description: Use data array to populate details form properties
	'
	' ***************************************************************** '
	Private Function DataToDetailsForm(ByRef lRow As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Pass details to form properties
			With frmDetails
				' {* USER DEFINED CODE (Begin) *}
				
				.CompanyID = CInt(m_vListData(ACSubCompanyID, lRow))
				.Code = CStr(m_vListData(ACSubCode, lRow))
				.Description = CStr(m_vListData(ACSubDescription, lRow))
				.CaptionID = CInt(m_vListData(ACSubCaptionID, lRow))
				.ParentID = CInt(m_vListData(ACSubParentID, lRow))
				.RegNo1 = CStr(m_vListData(ACSubRegNo1, lRow))
				.RegNo2 = CStr(m_vListData(ACSubRegNo2, lRow))
				.CurrencyID = CInt(m_vListData(ACSubBaseCurrency, lRow))
				.Address1 = CStr(m_vListData(ACSubAddress1, lRow))
				.Address2 = CStr(m_vListData(ACSubAddress2, lRow))
				.Address3 = CStr(m_vListData(ACSubAddress3, lRow))
				.Address4 = CStr(m_vListData(ACSubAddress4, lRow))
				.PostalCode = CStr(m_vListData(ACSubPostalCode, lRow))
				.CountryID = CInt(m_vListData(ACSubCountryID, lRow))
				.PhoneAreaCode = CStr(m_vListData(ACSubPhoneAreaCode, lRow))
				.PhoneNumber = CStr(m_vListData(ACSubPhoneNumber, lRow))
				.PhoneExtension = CStr(m_vListData(ACSubPhoneExtension, lRow))
				.FaxAreaCode = CStr(m_vListData(ACSubFaxAreaCode, lRow))
				.FaxNumber = CStr(m_vListData(ACSubFaxNumber, lRow))
				.FaxExtension = CStr(m_vListData(ACSubFaxExtension, lRow))
				' DC 31/01/00
				.Email = CStr(m_vListData(ACSubEmail, lRow))
				.VatNo = CStr(m_vListData(ACSubVatNo, lRow))
				.SenderMailboxId = CStr(m_vListData(ACSubSenderMailboxId, lRow))
				.BrokerABIId = CStr(m_vListData(ACSubBrokerABIId, lRow))
				.UserLicenceId = CInt(m_vListData(ACSubUserLicenceId, lRow))
				.PMCompanyNumber = CInt(m_vListData(ACSubPMCompanyNumber, lRow))
				.DefaultIndicator = CStr(m_vListData(ACSubDefaultIndicator, lRow))
				
				' {* USER DEFINED CODE (End) *}
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update details form from data", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetailsForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: DetailsFormToData (Standard Method)
	'
	' Description: Use details form properties to populate data array
	'
	' ***************************************************************** '
	Private Function DetailsFormToData(ByRef lRow As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			With frmDetails
				' {* USER DEFINED CODE (Begin) *}
				
				m_vListData(ACSubCompanyID, lRow) = .CompanyID
				m_vListData(ACSubCode, lRow) = .Code
				m_vListData(ACSubDescription, lRow) = .Description
				m_vListData(ACSubCaptionID, lRow) = .CaptionID
				m_vListData(ACSubParentID, lRow) = .ParentID
				m_vListData(ACSubRegNo1, lRow) = .RegNo1
				m_vListData(ACSubRegNo2, lRow) = .RegNo2
				m_vListData(ACSubBaseCurrency, lRow) = .CurrencyID
				m_vListData(ACSubAddress1, lRow) = .Address1
				m_vListData(ACSubAddress2, lRow) = .Address2
				m_vListData(ACSubAddress3, lRow) = .Address3
				m_vListData(ACSubAddress4, lRow) = .Address4
				m_vListData(ACSubPostalCode, lRow) = .PostalCode
				m_vListData(ACSubCountryID, lRow) = .CountryID
				m_vListData(ACSubPhoneAreaCode, lRow) = .PhoneAreaCode
				m_vListData(ACSubPhoneNumber, lRow) = .PhoneNumber
				m_vListData(ACSubPhoneExtension, lRow) = .PhoneExtension
				m_vListData(ACSubFaxAreaCode, lRow) = .FaxAreaCode
				m_vListData(ACSubFaxNumber, lRow) = .FaxNumber
				m_vListData(ACSubFaxExtension, lRow) = .FaxExtension
				' DC 31/01/00
				m_vListData(ACSubEmail, lRow) = .Email
				m_vListData(ACSubVatNo, lRow) = .VatNo
				m_vListData(ACSubSenderMailboxId, lRow) = .SenderMailboxId
				m_vListData(ACSubBrokerABIId, lRow) = .BrokerABIId
				m_vListData(ACSubUserLicenceId, lRow) = .UserLicenceId
				m_vListData(ACSubPMCompanyNumber, lRow) = .PMCompanyNumber
				m_vListData(ACSubDefaultIndicator, lRow) = .DefaultIndicator
				
				' {* USER DEFINED CODE (End) *}
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update data from details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PRIVATE Methods (End)
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		'Fire up the coresponding screen help file
        'Developer Guide No.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID3)
	End Sub
	
	' PRIVATE Events (Begin)
	
	
	Private Sub frmList_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
				m_lReturn = ProcessCommand()
				
				' Check the return value.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Do not procced with the interface termination.
					Cancel = 1
                    eventARgs.cancel = True
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Exit Sub
				End If
			End If
			
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
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions.
			m_lReturn = ProcessCommand()
			
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
			m_lReturn = ProcessCommand()
			
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
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		' Click event of the Add Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Add Details form
			m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMAdd)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (End) *}
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		' Click event of the Edit Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Edit Details form
			m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
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
	
	Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click
		Dim iMsgResult As DialogResult
		' Click event of the Remove Button.
		
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			'JK301198
			iMsgResult = MessageBox.Show("Are you sure you wish to remove the company?", "Remove Company", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			
			If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
				' Remove Details
				m_lReturn = DetailsDelete()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
					Exit Sub
				End If
			Else
				Exit Sub
			End If
			' {* USER DEFINED CODE (End) *}
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Remove button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRemove_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Navigate button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
			' Process the next set of actions.
			m_lReturn = ProcessCommand()
			
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
	
	Private Sub lvwListDetails_ItemClick(ByVal Item As ListViewItem)
		
		' Single ItemClick event for the List details.
		
		Try 
			
			' If item has zero ID then it has not been committed to the DB
			' so we can allow Remove otherwise disable
			' This code also in DetailsFormReturn
			If CDbl(m_vListData(ACSubCompanyID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) = 0 Then
				' Enable the Remove button
				cmdRemove.Enabled = True
			Else
				' Disable the Remove button
				cmdRemove.Enabled = False
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwListDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.DoubleClick
		
		' Double click event for the List details.
		
		Try 
			
			' Check if there are any items available.
			If lvwListDetails.Items.Count = 0 Then
				Exit Sub
			End If
			
			' Bring up Edit Details form
			m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMEdit)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwListDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.Enter
		
		' GotFocus Event for the List details
		
		Try 
			
			' Check if there are any items available.
			If lvwListDetails.Items.Count = 0 Then
				VB6.SetDefault(cmdAdd, True)
			Else
				VB6.SetDefault(cmdEdit, True)
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwListDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.Leave
		
		' LostFocus Event for the List details
		
		Try 
			
			' Set the default button.
			VB6.SetDefault(cmdOK, True)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwListDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwListDetails.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwListDetails.Columns(eventArgs.Column)
		
		' Column click event for the List details
		
		Try 
			
			With lvwListDetails
				' If current sort column header is
				' pressed.
				If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwListDetails) Then
					' Set sort order opposite of
					' current direction.
					ListViewHelper.SetSortOrderProperty(lvwListDetails, (ListViewHelper.GetSortOrderProperty(lvwListDetails) + 1) Mod 2)
				Else
					' Sort by this column (ascending).
					ListViewHelper.SetSortedProperty(lvwListDetails, False)
					
					' Turn off sorting so that the list
					' is not sorted twice
					ListViewHelper.SetSortOrderProperty(lvwListDetails, SortOrder.Ascending)
					ListViewHelper.SetSortKeyProperty(lvwListDetails, ColumnHeader.Index + 1 - 1)
					ListViewHelper.SetSortedProperty(lvwListDetails, True)
				End If
			End With
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' PRIVATE Events (End)
End Class
