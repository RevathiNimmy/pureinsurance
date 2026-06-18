Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Sudhanshu Behera on 6/24/2010 11:26:48 AM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmAdd
	Inherits System.Windows.Forms.Form
	
	
	Private Const ACClass As String = "frmInterface"
	
	' Private variables
	Private m_lReturn As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lItemDetailId As Integer
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the general interface object.
	Private m_oAdd As iCLMLossSchedule.Add
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Lookup value contants.
	Const ACValueTableName As Integer = 0
	Const ACValueID As Integer = 1
	Const ACValueStartPos As Integer = 2
	Const ACValueNumber As Integer = 3
	
	' Lookup detail contants.
	Const ACDetailKey As Integer = 0
	Const ACDetailDesc As Integer = 1
	
	
	
	Private Sub OpenSearchForm()
		
        Dim vDataArray(,) As Object

        'Modified by Sudhanshu Behera on 6/24/2010 11:29:47 AM refer developer guide no. ToDo List Project not found
        'Dim oItemDetailSearch As iCLMItemDetailsSearch.Interface_Renamed
        Dim oItemDetailSearch As Object
		Dim oAdd As iCLMLossSchedule.Add
		Dim vKeyArray(,) As Object
		
        'Const PMKeyRowLossSchedule As Integer = 0
        'Const PMKeyRowLossScheduleTypeId As Integer = 1
		
		
		Try 
			
			'Call the itemdetailsearch component
			Dim temp_oItemDetailSearch As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oItemDetailSearch, sClassName:="iCLMItemDetailsSearch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oItemDetailSearch = temp_oItemDetailSearch
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenSearchForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			Dim temp_oAdd As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oAdd, "iCLMLossSchedule.Add", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oAdd = temp_oAdd
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Add'.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenSearchForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			m_lReturn = CType(oAdd, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oAdd.Dispose()
                oAdd = Nothing
				
				Exit Sub
			End If
			

			m_lReturn = oItemDetailSearch.SetKeys(vKeyArray)
			
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Exit Sub
			End If
			
			

			m_lReturn = oItemDetailSearch.Start()

			m_lReturn = oItemDetailSearch.GetKeys(vKeyArray)

			For iLoop1 As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
				
                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)
                    Case "ItemDetailID" 'PMKeyNameItemDetailId

                        m_lItemDetailId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                End Select

            Next iLoop1


            oItemDetailSearch.Dispose()
            oItemDetailSearch = Nothing


            If m_lItemDetailId > 0 Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                m_lReturn = m_oBusiness.GetItemDetails(m_lItemDetailId, vDataArray)

                If Not Information.IsArray(vDataArray) Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                'Clear the controls we're potentially populating
                txtItemClaimed.Text = ""
                txtDescription.Text = ""
                txtItemAmount.Text = ""
                txtPayeeOrSupplier.Text = ""

                'We have data, populate the form Minor/Manufacturer/Model



                txtItemClaimed.Text = CStr(vDataArray(2, 0)) & "/" & _
                                      CStr(vDataArray(3, 0)) & "/" & _
                                      CStr(vDataArray(4, 0))

                txtDescription.Text = CStr(vDataArray(5, 0))

                txtItemAmount.Text = CStr(vDataArray(6, 0))

                'TODO get supplier name from party table

                txtPayeeOrSupplier.Text = CStr(vDataArray(8, 0))

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Remove the instance
            oItemDetailSearch = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenSearchForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenSearchForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	
	Private Sub cmdItemClaimed_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdItemClaimed.Click
		OpenSearchForm()
	End Sub
	
	Private Sub cmdPayeeOrSupplier_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPayeeOrSupplier.Click
		'TODO - get the party from Find Party Other
		'Note on a successful find the amount control should be reset.
	End Sub
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
	

	Private Sub frmAdd_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		'Ensure window at the top
		BringWindowToTop(Me.Handle.ToInt32())
		
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
		
		' Create an instance of the general interface object.
		m_oAdd = New iCLMLossSchedule.Add()
		
		'    ' Call the initialise method passing this interface
		'    ' and the business object as parameters.
		'    m_lReturn& = m_oAdd.Initialise( _
		''        frmInterface:=Me, _
		''        oBusiness:=m_oBusiness)
		
		' Set the interface status to cancelled. This is done
		' so that any interface termination will be noted
		' as cancelled except in the event of accepting
		' the interface.
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		'Set the interface default values.
		m_lReturn = SetInterfaceDefaults()
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If
		
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
	End Sub
	
	Private Function DisplayCaptions() As Integer
		' ***************************************************************** '
		' Name: DisplayCaptions
		'
		' Description: Display all language specific captions.
		'
		' History : 17092002 CMG/PB - Created
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			
			'Form

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabItemDetails, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDetailsTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Buttons

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Form Controls

			lblDateEntered.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddDateEntered, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblItemNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddItemNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblItemClaimed.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddItemClaimed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblItemDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddItemDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblSettlementMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddSettlementMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblStartingValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddStartingValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblAge.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddAge, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblLife.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddLife, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblDepreciationPercent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddDepreciationPercent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblDepreciation.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddDepreciation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblItemAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddItemAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblGST.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddGST, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblPaymentAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddPaymentAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblExcess.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblPayeeOrSupplier.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddPayeeOrSupplier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblPODate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddPODate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblDatePaid.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddDatePaid, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblSalvage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddSalvage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			optSalvage(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddSalvageYes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			optSalvage(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddSalvageNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function SetInterfaceDefaults() As Integer
		' ***************************************************************** '
		' Name: DisplayCaptions
		'
		' Description: Display all language specific captions.
		'
		' History : 17092002 CMG/PB - Created
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Display language specific form captions
			m_lReturn = DisplayCaptions()
			
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
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function DisplayLookupDetails() As Integer
		' ***************************************************************** '
		' Name: DisplayLookupDetails
		'
		' Description: Displays all of the lookup details using the lookup
		'              values/details.
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
		Const ACFirstItem As Integer = 0
		Const ACFirstRow As Integer = 0
		Const ACSecondRow As Integer = 1
        'Const ACThirdRow As Integer = 2
		Const ACNumberOfColumns As Integer = 4 ' Zero based
		Const ACNumberOfRows As Integer = 1 ' Zero based
		
		Try 
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			' Get the lookup values.

			m_vLookupValues = Nothing

			m_vLookupDetails = Nothing
			
			ReDim m_vLookupValues(ACNumberOfColumns, ACNumberOfRows) ' Zero based
			
			' Supplier
			m_vLookupValues(ACFirstItem, ACFirstRow) = "Loss_Schedule_Settlement_Method"
			
			' Status
			m_vLookupValues(ACFirstItem, ACSecondRow) = "Loss_Schedule_Item_Status"
			

			m_lReturn = m_oBusiness.GetLookupValues(gPMConstants.PMELookupType.PMLookupAll, m_vLookupValues, g_iLanguageID, m_vLookupDetails)
			
			m_lReturn = GetLookupValues()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Populate Progress status combo box
			m_lReturn = GetLookupDetails("Loss_Schedule_Settlement_Method", cboSettlementMethod)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			' Populate Secondary cause combo box
			m_lReturn = GetLookupDetails("Loss_Schedule_Item_Status", cboStatus)
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
		' ***************************************************************** '
		' Name: GetLookupValues
		'
		' Description: Gets all of the lookup values, ready to be used by
		'              the lookup function.
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.

			m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				
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
		' ***************************************************************** '
		' Name: GetLookupDetails
		'
		' Description: Gets all of the lookup details using the lookup
		'              values, then assigns them to the control passed.
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
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
