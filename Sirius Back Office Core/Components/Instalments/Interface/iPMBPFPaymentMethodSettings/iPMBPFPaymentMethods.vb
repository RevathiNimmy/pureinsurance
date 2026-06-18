Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 14-Aug-2001
	'
	' Description:  Entry point for the interface. This is a stub
	'               list view so that the user can jump choose
	'               the Payment Method to edit.
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
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iPMBPFPaymentMethodSettings.General
	
	' Declare an instance of the Business object.

	Private m_oBusiness As bSIRPFExport.Business
	
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
	Private m_ctlTabFirstLast() As Control
	
	Private m_oForm As frmPaymentMethodEdit
	
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
	
	
	' PRIVATE Property Procedures (Begin)
	
	Public Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the interface exit status.
			m_lStatus = Value
			
		End Set
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
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		lvPaymentMethods_DoubleClick(lvPaymentMethods, New EventArgs())
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
            Dim temp_m_oBusiness As Object = Nothing
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPFExport.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

				sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iPMBPFPaymentMethodSettings.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Create an instance of the form control object.
			'Set m_oFormFields = New iPMFormControl.FormFields
			
			' Set language
			'm_oFormFields.LanguageID = g_iLanguageID%
			
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
		Try 
			
			'Centre the form
			iPMFunc.CenterForm(Me)
			
			'Load up the grid
			m_lErrorNumber = RefreshGrid()
			
			'If there is only one entry in the grid then
			'automatically launch the Edit form
			With lvPaymentMethods
				If .Items.Count = 1 Then
					'Load the new form
					m_oForm = New frmPaymentMethodEdit()
					
					'pass through the ID

					m_oForm.PFPaymentMethod_cnt = Convert.ToString(.Items.Item(0).Tag)
					m_oForm.Task = gPMConstants.PMEComponentAction.PMEdit
                    Me.Dispose()
					'Show it
					m_oForm.ShowDialog()
					
					'Unload it
					m_oForm = Nothing
					
					'Tell the Navigator to Cancel this form
					Status = gPMConstants.PMEReturnCode.PMCancel
				Else
					Status = gPMConstants.PMEReturnCode.PMTrue
				End If
			End With
		
		Catch excep As System.Exception
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	Private Function RefreshGrid() As Integer
		Dim result As Integer = 0
		Dim lvItem As ListViewItem
		
        Dim vResultArray(,) As Object = Nothing
		
		'Get the data

		m_lReturn = m_oBusiness.GetPaymentMethods(vResultArray)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Log Error.
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="There are no payment methods set up.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			End If
			
			result = m_lReturn
		End If
		
		'Clear the grid and rebuild
		lvPaymentMethods.Items.Clear()
		lvPaymentMethods.Columns.Item(0).Width = CInt(lvPaymentMethods.Width - VB6.TwipsToPixelsX(380))
		

		Dim nStart As Integer = vResultArray.GetLowerBound(1)

		Dim nEnd As Integer = vResultArray.GetUpperBound(1)
		
		For nRow As Integer = nStart To nEnd

            lvItem = lvPaymentMethods.Items.Add(CStr(vResultArray(1, nRow)))


            lvItem.Tag = CStr(vResultArray(0, nRow))
			lvItem = Nothing
		Next nRow
		
		Return gPMConstants.PMEReturnCode.PMTrue
	End Function
	
	Private Sub lvPaymentMethods_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvPaymentMethods.DoubleClick
		'Edit the highlighted Payment Method
		With lvPaymentMethods
			If Not (.FocusedItem Is Nothing) Then
				'Load the new form
				m_oForm = New frmPaymentMethodEdit()
				
				'pass through the ID

				m_oForm.PFPaymentMethod_cnt = Convert.ToString(.FocusedItem.Tag)
				m_oForm.Task = gPMConstants.PMEComponentAction.PMEdit
				
				'Show it
				m_oForm.ShowDialog()
				
				'Unload it
				m_oForm.Close()
				m_oForm = Nothing
			End If
		End With
	End Sub
End Class
