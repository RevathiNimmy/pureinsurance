Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Sudhanshu Behera on 6/24/2010 11:26:34 AM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmAssign
	Inherits System.Windows.Forms.Form
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Declare an instance of the Business object.

	Private m_oBusiness As bCLMLossSchedule.Business
	
	' Lookup value contants.
	Const ACValueTableName As Integer = 0
	Const ACValueID As Integer = 1
	Const ACValueStartPos As Integer = 2
	Const ACValueNumber As Integer = 3
	
	' Lookup detail contants.
	Const ACDetailKey As Integer = 0
	Const ACDetailDesc As Integer = 1
	
	

	'Private Sub Status(ByVal Value As Integer)
		' Set the interface exit status.
		'm_lStatus = Value
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			' Return the interface exit status.
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
	End Sub
	

	Private Sub frmAssign_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Dim m_lReturn As Integer
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		' Get an instance of the business object via
		' the public object manager.
		Dim temp_m_oBusiness As Object
		m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMLossSchedule.Business", vInstanceManager:="ClientManager")
		m_oBusiness = temp_m_oBusiness
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If
		
		
		SetInterfaceDefaults()
		FileSystem.FileClose()
	End Sub
	
	Private Sub SetInterfaceDefaults()
		Dim m_lReturn As Integer = DisplayCaptions()
		m_lReturn = DisplayLookupDetails()
		
	End Sub
	
	Private Function DisplayCaptions() As Integer
		Dim result As Integer = 0
		Dim ACClass As Object
		' ***************************************************************** '
		' Name: DisplayCaptions
		'
		' Description: Display all language specific captions.
		'
		' History : 17092002 CMG/PB - Created
		'
		' ***************************************************************** '
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			
			'Form

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAssignTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Tabs

			SSTabHelper.SetTabCaption(tabAssign, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAssignTab1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Buttons

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Form Controls

			lblPayeeOrSupplier.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACPayeeOrSupplier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function DisplayLookupDetails() As Integer
		Dim result As Integer = 0
		Dim ACClass As Object
		Dim m_lReturn As gPMConstants.PMEReturnCode
		' ***************************************************************** '
		' Name: DisplayLookupDetails
		'
		' Description: Displays all of the lookup details using the lookup
		'              values/details.
		'
		' ***************************************************************** '

		Const ACFirstItem As Integer = 0
		Const ACFirstRow As Integer = 0
        'Const ACSecondRow As Integer = 1
        'Const ACThirdRow As Integer = 2
		Const ACNumberOfColumns As Integer = 4 ' Zero based
		Const ACNumberOfRows As Integer = 0 ' Zero based
		
		Try 
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			' Get the lookup values.

			m_vLookupValues = Nothing

			m_vLookupDetails = Nothing
			
			ReDim m_vLookupValues(ACNumberOfColumns, ACNumberOfRows) ' Zero based
			
			' Status
			m_vLookupValues(ACFirstItem, ACFirstRow) = "Loss_Schedule_Item_Status"
			

			m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAll, m_vLookupValues, g_iLanguageID, m_vLookupDetails)
			
			m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Populate status box
			m_lReturn = CType(GetLookupDetails("Loss_Schedule_Item_Status", cboStatus), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Private Function GetLookupValues() As Integer
		Dim result As Integer = 0
		Dim ACClass As Object
		Dim m_lReturn As gPMConstants.PMEReturnCode
		' ***************************************************************** '
		' Name: GetLookupValues
		'
		' Description: Gets all of the lookup values, ready to be used by
		'              the lookup function.
		'
		' ***************************************************************** '
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.

			m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.

				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=CStr(ACClass), vMethod:="GetLookupValues")
				
				Return result
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer
		Dim result As Integer = 0
		Dim ACClass As Object
		' ***************************************************************** '
		' Name: GetLookupDetails
		'
		' Description: Gets all of the lookup details using the lookup
		'              values, then assigns them to the control passed.
		'
		' ***************************************************************** '
		
		Dim lRow As Integer
		Dim bFoundMatch As Boolean
		
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
				Dim ctlLookup_NewIndex As Integer = -1
				ctlLookup_NewIndex = ctlLookup.Items.Add(CStr(m_vLookupDetails(ACDetailDesc, lCntr)))
				VB6.SetItemData(ctlLookup, ctlLookup_NewIndex, CInt(m_vLookupDetails(ACDetailKey, lCntr)))
				
				If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
					If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
						ctlLookup.SelectedIndex = ctlLookup_NewIndex
					End If
				End If
				
			Next lCntr
			
			' Check if the selected index is blank. If so,
			' we set the controls index to zero.
			If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then
				If ctlLookup.Items.Count > 0 Then
					'RWH(12/04/2001) Set box to blank unless value is specifically set.
					ctlLookup.SelectedIndex = -1
					'            ctlLookup.ListIndex = 0
				End If
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
End Class
