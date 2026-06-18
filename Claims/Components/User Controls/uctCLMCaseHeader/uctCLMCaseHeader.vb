Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMCaseHeader_NET.uctCLMCaseHeader")> _
Public Partial Class uctCLMCaseHeader
	Inherits System.Windows.Forms.UserControl
	Public Event EnabledChange()
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "uctCLMCaseHeader"
	
	' objects
	Private m_oObjectManager As bObjectManager.ObjectManager

	Private m_oBusiness As bCLMCase.Business
	
	' generic interface details
	Private m_iTask As Integer
	Private m_iLanguageID As Integer
	Private m_iSourceID As Integer
	Private m_iUserId As Integer
	Private m_sUserName As String = ""
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_dtEffectiveDate As Date
	Private m_sTransactionType As String = ""
	
	'Other
	Private m_lMinimumWidth As Integer
	Private m_lMinimumHeight As Integer
	Private m_bIsInitialised As Boolean
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bHasChanged As Boolean
	
	'Property
	Private m_lCaseID As Integer
	Private m_sCaseNumber As String = ""
    Private m_dtCaseOpenedDate As Date
	Private m_lCaseProgressStatusID As Integer
	Private m_lCaseAssistantID As Integer
	Private m_lCaseAnalystID As Integer
	Private m_lCaseVersion As Integer
	Private m_lBaseCaseID As Integer
    Private m_sCaseProgressStatusCode As String
    Private m_lClaimID As Integer

    'Default Property
    Const m_def_Enabled As Boolean = True
	
	'******************
	<Browsable(True)> _
	Public Property MinimumWidth() As Integer
		Get
			Return m_lMinimumWidth
		End Get
		Set(ByVal Value As Integer)
			m_lMinimumWidth = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property MinimumHeight() As Integer
		Get
			Return m_lMinimumHeight
		End Get
		Set(ByVal Value As Integer)
			m_lMinimumHeight = Value
		End Set
	End Property
	'*********************
	'*********************
	<Browsable(True)> _
	Public Property CaseID() As Integer
		Get
			Return m_lCaseID
		End Get
		Set(ByVal Value As Integer)
			m_lCaseID = Value
			m_bHasChanged = False
			'Call Load
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property CaseNumber() As String
		Get
			Return m_sCaseNumber
		End Get
		Set(ByVal Value As String)
            m_sCaseNumber = Value
            txtCaseNumber.Text = m_sCaseNumber
        End Set
	End Property
	
	<Browsable(True)> _
	Public Property CaseOpenedDate() As Date
		Get
			Return m_dtCaseOpenedDate
		End Get
		Set(ByVal Value As Date)
			m_dtCaseOpenedDate = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property CaseProgressStatusID() As Integer
		Get
			Return m_lCaseProgressStatusID
		End Get
		Set(ByVal Value As Integer)
			m_lCaseProgressStatusID = Value
		End Set
    End Property

    <Browsable(True)> _
    Public Property CaseProgressStatusCode() As String
        Get
            Return m_sCaseProgressStatusCode
        End Get
        Set(ByVal Value As String)
            m_sCaseProgressStatusCode = Value
        End Set
    End Property
	
	<Browsable(True)> _
	Public Property CaseAssistantID() As Integer
		Get
			Return m_lCaseAssistantID
		End Get
		Set(ByVal Value As Integer)
			m_lCaseAssistantID = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property CaseVersion() As Integer
		Get
			Return m_lCaseVersion
		End Get
		Set(ByVal Value As Integer)
			m_lCaseVersion = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property BaseCaseID() As Integer
		Get
			Return m_lBaseCaseID
		End Get
		Set(ByVal Value As Integer)
			m_lBaseCaseID = Value
		End Set
	End Property
	
	'Read Only Property
	<Browsable(False)> _
	Public ReadOnly Property HasChanged() As Boolean
		Get
			Return m_bHasChanged
		End Get
	End Property
	
	'Valid case
	<Browsable(False)> _
	Public ReadOnly Property ValidCase() As Boolean
		Get
			Return ValidateForm()
		End Get
	End Property
	
	<Browsable(True)> _
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property


    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal Value As Boolean)
            MyBase.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value

        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Initialise"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Check if already initialised
		If m_bIsInitialised Then
			Return result
		End If
		
		'Set m_colPaymentItems = New Collection
		
		' Create an instance of the object manager.
		m_oObjectManager = New bObjectManager.ObjectManager()
		
		' Call the initialise method.
		lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' If UserID is 0 assume that user cancelled logon
		If m_oObjectManager.UserID = 0 Then
			' Exit application
			result = gPMConstants.PMEReturnCode.PMFalse
			Return result
		End If
		
		' Store the language ID from the object manager to the public variables,
		' to enable us to use them throughout the object.
		With m_oObjectManager
			m_iLanguageID = .LanguageID
			m_iSourceID = .SourceID
			m_iUserId = .UserID
			m_sUserName = .UserName
		End With
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' Get an instance of the business object via the public object manager.
		Dim temp_m_oBusiness As Object
		lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMCase.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
		m_oBusiness = temp_m_oBusiness
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetInstance of bClMCase.Business Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		lReturn = DisplayCaptions()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		lReturn = SetupFormLayout()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "SetupFormLayout Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' hold Initialised status
		m_bIsInitialised = True
		
		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	Private Sub cboCaseOpenDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCaseOpenDate.ValueChanged
		m_bHasChanged = gPMFunctions.ToSafeDate(cboCaseOpenDate.Value) <> gPMFunctions.ToSafeDate(m_dtCaseOpenedDate)
	End Sub
	
	Private Sub cboCaseProgressStatus_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCaseProgressStatus.Click
		m_bHasChanged = gPMFunctions.ToSafeLong(cboCaseProgressStatus.ItemId) <> gPMFunctions.ToSafeLong(m_lCaseProgressStatusID)
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub txtCaseNumber_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCaseNumber.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		m_bHasChanged = m_sCaseNumber.Trim().ToUpper() <> gPMFunctions.ToSafeString(txtCaseNumber.Text).Trim().ToUpper()
	End Sub
	
	Private Sub UserControl_Initialize()
		MinimumWidth = 8520
		MinimumHeight = 1320
		SetupFormLayout()
	End Sub
	
	Private Sub uctCLMCaseHeader_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		Const iButtonMinHeight As Integer = 315
		
		
		If VB6.PixelsToTwipsY(MyBase.Height) < MinimumHeight Then
			MyBase.Height = VB6.TwipsToPixelsY(MinimumHeight)
		End If
		
		If VB6.PixelsToTwipsX(MyBase.Width) < MinimumWidth Then
			MyBase.Width = VB6.TwipsToPixelsX(MinimumWidth)
		End If
		
		'  fraCaseClaimDetails.Height = UserControl.Height - 50
		'  fraCaseClaimDetails.Width = UserControl.Width - 50
		
		m_lMinimumWidth = 8520
		m_lMinimumHeight = 1320
		
		
		
	End Sub
	
	
	
	' ***************************************************************** '
	' Name: Load
	' Parameters: n/a
	' Description:
	' History:
	' Created: VB : 06-07-2007
	' ***************************************************************** '
	Public Function Load_Renamed() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Load"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' set up taxes list view
		lReturn = SetupFormLayout()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "SetupFormLayout Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' Get case details
		lReturn = GetCaseDetails()
		If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
			'do nothing
		ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
			gPMFunctions.RaiseError(kMethodName, "GetCaseDetails Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		lReturn = DataToInterface()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
	
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: SetupFormLayout
	' Parameters: n/a
	' Description:
	' History:
	' Created :
	' ***************************************************************** '
	Private Function SetupFormLayout() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupFormLayout"
		
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		txtCaseNumber.Text = ""
		txtCaseVersion.Text = ""
        cboCaseOpenDate.Value = DateTime.Today
        'Added the lines to get values in the combobox
        'start change
        cboCaseProgressStatus.FirstItem = ""
        cboAnalyst.FirstItem = ""
        cboAssistant.FirstItem = ""
        'end change
		cboCaseProgressStatus.ListIndex = -1
		cboAnalyst.ListIndex = -1
		cboAssistant.ListIndex = -1
		

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		Finally



		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: GetCaseDetails
	' Parameters: n/a
	' Description:
	' History:
	' Created :
	' ***************************************************************** '
	Private Function GetCaseDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetCaseDetails"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue

			If Task = gPMConstants.PMEComponentAction.PMAdd Then

				Dim lReturnError As Integer = 0
				Dim sFailureReason As String = ""
				m_lReturn = m_oBusiness.GenerateCaseCode(m_sCaseNumber, m_lClaimID, lReturnError, sFailureReason)
				If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
					If lReturnError = gPMConstants.PMEReturnCode.PMNotFound Then
						MessageBox.Show("Case numbering scheme is not set.", "Case Numbering", MessageBoxButtons.OK, MessageBoxIcon.Error)
					ElseIf sFailureReason <> "" Then
						MessageBox.Show(sFailureReason, "Case Numbering", MessageBoxButtons.OK, MessageBoxIcon.Error)
					End If
					Return result
				ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					gPMFunctions.RaiseError(kMethodName, "GenerateClientCode" & " Failed", gPMConstants.PMELogLevel.PMLogError)
				End If

			Else

			lReturn = m_oBusiness.LoadCase(v_lCaseID:=m_lCaseID, r_sCaseNumber:=m_sCaseNumber, r_dtCaseOpenedDate:=m_dtCaseOpenedDate, r_lCaseProgressStatusID:=m_lCaseProgressStatusID, r_lCaseAnalystID:=m_lCaseAnalystID, r_lCaseAssistantID:=m_lCaseAssistantID, r_lCaseVersion:=m_lCaseVersion, r_lBaseCaseID:=m_lBaseCaseID)
			
			If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				result = lReturn
			ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
				gPMFunctions.RaiseError(kMethodName, "LoadCase Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
		End If

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		Finally



		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: DataToInterface
	'
	' Parameters: n/a
	'
	' Description:Updates all interface details from the search data.
	'
	' History:
	' Created :
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DataToInterface"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		If m_sCaseNumber.Trim().Length < 1 Then
			txtCaseNumber.ReadOnly = False
		Else
			txtCaseNumber.Text = m_sCaseNumber
        End If
        'Developer Guide no. 257
        cboCaseOpenDate.Value = IIf(m_dtCaseOpenedDate = DateTime.MinValue, DateTime.Today, m_dtCaseOpenedDate)
		cboCaseProgressStatus.ItemId = m_lCaseProgressStatusID
		cboAnalyst.ItemId = m_lCaseAnalystID
		cboAssistant.ItemId = m_lCaseAssistantID
        txtCaseVersion.Text = CStr(m_lCaseVersion)
        CaseProgressStatusCode = cboCaseProgressStatus.ItemCode.Trim
        'If m_sItemCode = "CLOSED" Then
        If cboCaseProgressStatus.ItemCode = "CLOSED" Then
            cboAnalyst.Enabled = False
            cboCaseOpenDate.Enabled = False
            cboAssistant.Enabled = False
            MessageBox.Show("This is a close Case. Please Reopen/Open the Case Progress Status for further Action", kMethodName, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else

            cboAnalyst.Enabled = True
            cboCaseOpenDate.Enabled = True
            cboAssistant.Enabled = True
        End If
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: ValidateForm
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created :
	' ***************************************************************** '
	Private Function ValidateForm() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ValidateForm"
		
		Dim lReturn As Integer
		Dim sMsg As String = ""
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If txtCaseNumber.Text.Trim().Length < 1 Then
			MessageBox.Show("The Case Number must be entered.", kMethodName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMFalse
                Return result
		End If
		
        If gPMFunctions.ToSafeDate(cboCaseOpenDate.Value) > gPMFunctions.ToSafeDate(DateTime.Today) Then

            sMsg = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstErrCaseOpendate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            MessageBox.Show(sMsg, kMethodName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            result = gPMConstants.PMEReturnCode.PMFalse
                Return result
        End If
		
		If cboCaseProgressStatus.ListIndex = -1 Then
			

            sMsg = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstErrCaseProgressStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			MessageBox.Show(sMsg, kMethodName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMFalse
                Return result
		End If
		
		If cboAssistant.ListIndex = -1 Then

            sMsg = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstErrCaseAssistant, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			MessageBox.Show(sMsg, kMethodName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMFalse
                Return result
		End If
		
		If cboAnalyst.ListIndex = -1 Then

            sMsg = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstErrCaseAnalyst, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			MessageBox.Show(sMsg, kMethodName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			result = gPMConstants.PMEReturnCode.PMFalse
                Return result
		End If

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: Save
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created :
	' ***************************************************************** '
	Public Function Save() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Save"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim lCaseID, bIsChanged As Integer
		Dim sDescription As String = ""
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		'    bIsChanged = True
		'
		'    If cboCaseOpenDate.Value = m_dtCaseOpenedDate And _
		''                    cboCaseProgressStatus.ItemId = m_lCaseProgressStatusID And _
		''                    cboAssistant.ItemId = m_lCaseAssistantID And _
		''                    cboAnalyst.ItemId = m_lCaseAnalystID Then
		'
		'        bIsChanged = False
		'    End If
		
		'    If Not bIsChanged And Task = PMEdit Then
		'        GoTo Finally
		'    End If
		
		lReturn = InterfaceToData()
		

		lReturn = m_oBusiness.SaveCase(v_lCaseID:=m_lCaseID, v_sCaseNumber:=m_sCaseNumber, v_dtCaseOpenedDate:=m_dtCaseOpenedDate, v_lCaseProgressStatusID:=m_lCaseProgressStatusID, v_lCaseAnalystID:=m_lCaseAnalystID, v_lCaseAssistantID:=m_lCaseAssistantID, v_lCaseVersion:=m_lCaseVersion, r_lNewCaseID:=lCaseID, r_lBaseCaseID:=m_lBaseCaseID)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "SaveCase Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		If lCaseID > 0 And m_lCaseID <> lCaseID Then
			m_lCaseID = lCaseID
		End If
		
		If Task <> gPMConstants.PMEComponentAction.PMAdd Then
			sDescription = Interaction.InputBox("What is the description for this change of Case?", "Case")
		End If
		
		If sDescription.Trim() = "" Then
			If Task = gPMConstants.PMEComponentAction.PMAdd Then
				sDescription = "Opened Case"
			ElseIf Task = gPMConstants.PMEComponentAction.PMEdit Then 
				sDescription = "Case Edited"
			End If
		End If
		

		lReturn = m_oBusiness.CreateEvent(v_lCaseID:=m_lCaseID, v_sEventTypeCode:="CASES", v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Failed to create event", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		
		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		Finally
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Parameters: n/a
	'
	' Description: Display all language specific captions.
	'
	' History:
	'           Created :
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DisplayCaptions"
		
		Dim lReturn As Integer
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' set the caption for the button

        lblCaseNumber.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstCaseNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        lblCaseOpenDate.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstCaseOpenDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        lblCaseProgressStatus.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstCaseProgressStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        lblAnalyst.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstAnalyst, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        lblAssistant.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstAssistant, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        lblCaseVersion.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstCaseVersion, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created :
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "InterfaceToData"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_sCaseNumber = gPMFunctions.ToSafeString(txtCaseNumber.Text)
		m_dtCaseOpenedDate = gPMFunctions.ToSafeDate(cboCaseOpenDate.Value)
		m_lCaseProgressStatusID = gPMFunctions.ToSafeLong(cboCaseProgressStatus.ItemId)
		m_lCaseAnalystID = gPMFunctions.ToSafeLong(cboAnalyst.ItemId)
		m_lCaseAssistantID = gPMFunctions.ToSafeLong(cboAssistant.ItemId)
		m_lCaseVersion = gPMFunctions.ToSafeLong(CInt(txtCaseVersion.Text))
		

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: SetProcessModes
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : VB
	' ***************************************************************** '
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetProcessModes"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		If Not Information.IsNothing(vTask) Then

			m_iTask = CInt(vTask)
		End If
		

		If Not Information.IsNothing(vNavigate) Then

			m_lNavigate = CInt(vNavigate)
		End If
		

		If Not Information.IsNothing(vProcessMode) Then

			m_lProcessMode = CInt(vProcessMode)
		End If
		

		If Not Information.IsNothing(vTransactionType) Then

			m_sTransactionType = CStr(vTransactionType)
		End If
		

		If Not Information.IsNothing(vEffectiveDate) Then

			m_dtEffectiveDate = CDate(vEffectiveDate)
		End If
		
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
    'developer guide no. 1 No Solution
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        PropBag.WriteProperty("Enabled", MyBase.Enabled, m_def_Enabled)
    End Sub
    'developer guide no. 1 No Solution
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))
    End Sub
End Class
