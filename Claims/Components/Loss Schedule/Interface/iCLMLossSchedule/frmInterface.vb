Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Sudhanshu Behera on 6/24/2010 11:26:20 AM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	
	Private Const ACClass As String = "frmInterface"
	
	Private Const ACDisplayNormal As String = "Normal"
	Private Const ACDisplayCompressed As String = "Compressed"
	Private Const ACLSTypeMVPC As String = "MVPC"
	Private Const ACLSTypeGeneral As String = "General"
	
	' Private variables
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bLossSchedule As Boolean
	Private m_lLossScheduleTypeId As Integer
	Private m_sLossScheduleType As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	' Declare an instance of the Business object.

	Private m_oBusiness As bCLMLossSchedule.Business
	
	Private m_iItemNumber As Integer
	Private m_colLossSchedule As Collection
	
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property LossSchedule() As Boolean
		Get
			Return m_bLossSchedule
		End Get
		Set(ByVal Value As Boolean)
			m_bLossSchedule = Value
		End Set
	End Property
	Public Property LossScheduleTypeId() As Integer
		Get
			Return m_lLossScheduleTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lLossScheduleTypeId = Value
		End Set
	End Property
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		ShowItemDetails()
	End Sub
	
	
	Private Sub ShowItemDetails()
		
		Dim oFrmAdd As frmAdd
		Dim oClass As Add
		Dim lstNewItem As ListViewItem

		Try 
			
			oFrmAdd = New frmAdd()
			oFrmAdd.ShowDialog()
			
			
			If m_colLossSchedule Is Nothing Then
				m_colLossSchedule = New Collection()
			End If
			
			oClass = New Add()
			With oFrmAdd
				If .txtDateEntered.Text <> "" Then oClass.dtDateEntered = CDate(.txtDateEntered.Text)
				oClass.iItemNumber = m_iItemNumber
				If .txtItemClaimed.Text <> "" Then oClass.sItemClaimed = .txtItemClaimed.Text
				If .txtDescription.Text <> "" Then oClass.sItemDescription = .txtDescription.Text
				'TODO not tag
				If .cboSettlementMethod.Text <> "" Then oClass.sSettlementMethod = .cboSettlementMethod.Text
				If .txtStartingValue.Text <> "" Then oClass.dStartingValue = CDbl(.txtStartingValue.Text)
				If .txtAge.Text <> "" Then oClass.lAge = CInt(.txtAge.Text)
				If .txtLife.Text <> "" Then oClass.lLife = CInt(.txtLife.Text)
				If .txtDepreciationPercent.Text <> "" Then oClass.dDepreciationPercent = CDbl(.txtDepreciationPercent.Text)
				If .txtDepreciation.Text <> "" Then oClass.sDepreciation = .txtDepreciation.Text
				If .txtItemAmount.Text <> "" Then oClass.dItemAmount = CDbl(.txtItemAmount.Text)
				
				If .txtGST.Text <> "" Then oClass.dGST = CDbl(.txtGST.Text)
				If .txtItemAmount.Text <> "" Then oClass.dItemAmount = CDbl(.txtItemAmount.Text)
				If .txtPaymentAmount.Text <> "" Then oClass.dPaymentAmount = CDbl(.txtPaymentAmount.Text)
				If .txtExcess.Text <> "" Then oClass.dExcess = CDbl(.txtExcess.Text)
				If .txtPayeeOrSupplier.Text <> "" Then oClass.lPayeeOrSupplier = CInt(.txtPayeeOrSupplier.Text)
				'TODO not tag
				If .cboStatus.Text <> "" Then oClass.sItemStatus = .cboStatus.Text
				If .txtPODate.Text <> "" Then oClass.dtPODate = CDate(.txtPODate.Text)
				If .txtDatePaid.Text <> "" Then oClass.dtDatePaid = CDate(.txtDatePaid.Text)
				'TODO not count x 2
				'If .optSalvage.Value <> "" Then oClass.lSalvage = .optSalvage.Item(0)
			End With
			
			m_colLossSchedule.Add(oClass, CStr(m_iItemNumber))
			
			m_iItemNumber += 1
			
			oClass = Nothing
			
			lvwItems.Items.Clear()
			
			For	Each oClass2 As Add In m_colLossSchedule
				oClass = oClass2
				lstNewItem = lvwItems.Items.Add("")
				lstNewItem.Text = DateTimeHelper.ToString(oClass.dtDateEntered)
				ListViewHelper.GetListViewSubItem(lstNewItem, 1).Text = CStr(oClass.iItemNumber)
				ListViewHelper.GetListViewSubItem(lstNewItem, 2).Text = oClass.sItemClaimed
				ListViewHelper.GetListViewSubItem(lstNewItem, 3).Text = oClass.sItemDescription
				ListViewHelper.GetListViewSubItem(lstNewItem, 4).Text = oClass.sSettlementMethod
				ListViewHelper.GetListViewSubItem(lstNewItem, 5).Text = CStr(oClass.dStartingValue)
				ListViewHelper.GetListViewSubItem(lstNewItem, 6).Text = CStr(oClass.lAge)
				ListViewHelper.GetListViewSubItem(lstNewItem, 7).Text = CStr(oClass.lLife)
				ListViewHelper.GetListViewSubItem(lstNewItem, 8).Text = CStr(oClass.dDepreciationPercent)
				'TODO Which is it? lstNewItem.SubItems(8) = oClass.sDepreciation
				ListViewHelper.GetListViewSubItem(lstNewItem, 9).Text = CStr(oClass.dGST)
				ListViewHelper.GetListViewSubItem(lstNewItem, 10).Text = CStr(oClass.dItemAmount)
				ListViewHelper.GetListViewSubItem(lstNewItem, 11).Text = CStr(oClass.dPaymentAmount)
				ListViewHelper.GetListViewSubItem(lstNewItem, 12).Text = CStr(oClass.dExcess)
				ListViewHelper.GetListViewSubItem(lstNewItem, 13).Text = CStr(oClass.lPayeeOrSupplier)
				ListViewHelper.GetListViewSubItem(lstNewItem, 14).Text = oClass.sItemStatus
				ListViewHelper.GetListViewSubItem(lstNewItem, 15).Text = DateTimeHelper.ToString(oClass.dtPODate)
				ListViewHelper.GetListViewSubItem(lstNewItem, 16).Text = DateTimeHelper.ToString(oClass.dtDatePaid)
				ListViewHelper.GetListViewSubItem(lstNewItem, 17).Text = CStr(oClass.lSalvage)
				lstNewItem.Tag = CStr(oClass.iItemNumber)
			Next oClass2
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowItemDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowItemDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
		
	End Sub
	
	Private Sub cmdAssign_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAssign.Click
		ShowAssign()
	End Sub
	
	Private Sub ShowAssign()
		
		Dim oFrmAssign As frmAssign
		
		Try 
			
			oFrmAssign = New frmAssign()
			oFrmAssign.ShowDialog()
			
			If oFrmAssign.Status <> gPMConstants.PMEReturnCode.PMOK Then
				oFrmAssign = Nothing
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowAssign Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowAssign", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMLossSchedule.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim oFrmLossScheduleType As frmLossScheduleType
		Dim vDataArray(,) As Object
		Const ACLossScheduleTypeId As Integer = 0
		Const ACDescription As Integer = 3
		
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		'Ensure window at the top
		BringWindowToTop(Me.Handle.ToInt32())
		
		'Set the interface default values.
		m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If
		
		If LossScheduleTypeId = 0 Then
			'Loss Schedule Type Not Set for this peril, so well ask for one

			m_lReturn = CType(GetLossScheduleTypes(vDataArray), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			oFrmLossScheduleType = New frmLossScheduleType()
			
			' Get Loss Schedule types and populate the cboLossScheduleType combobox
			If Information.IsArray(vDataArray) Then

				For lLossScheduleType As Integer = vDataArray.GetLowerBound(1) To vDataArray.GetUpperBound(1)
					Dim cboLossScheduleType_NewIndex As Integer = -1

                    cboLossScheduleType_NewIndex = oFrmLossScheduleType.cboLossScheduleType.Items.Add(CStr(vDataArray(ACDescription, lLossScheduleType)))

                    VB6.SetItemData(oFrmLossScheduleType.cboLossScheduleType, cboLossScheduleType_NewIndex, CInt(vDataArray(ACLossScheduleTypeId, lLossScheduleType)))
				Next lLossScheduleType
			End If
			
			If oFrmLossScheduleType.cboLossScheduleType.Items.Count > 0 Then
				oFrmLossScheduleType.cboLossScheduleType.SelectedIndex = 0
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			oFrmLossScheduleType.ShowDialog()
			
			m_lStatus = oFrmLossScheduleType.Status
			
			If m_lStatus <> gPMConstants.PMEReturnCode.PMOK Then
				'TODO Stop loss schedule loading
				oFrmLossScheduleType = Nothing
				Exit Sub
			End If
			
			m_lLossScheduleTypeId = oFrmLossScheduleType.cboLossScheduleType.SelectedIndex
			
		End If
		
		MessageBox.Show("LossScheduleTypeID = " & m_lLossScheduleTypeId, Application.ProductName)
		
		Select Case m_lLossScheduleTypeId
			Case 0
				'Motor Vehicle / Pleasure Craft
				m_sLossScheduleType = ACLSTypeMVPC
			Case Else
				'General
				m_sLossScheduleType = ACLSTypeGeneral
		End Select
		
		'Create the appropriate list view
		CreateListView()
		
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

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Tabs

			SSTabHelper.SetTabCaption(tabLossSchedule, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTab1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabLossSchedule, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTab2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabLossSchedule, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTab3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Buttons

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACApplyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdAssign.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAssignButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			
			'Form Controls

			fraTotals.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTotals, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblPaymentAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACPaymentAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblSubTotal.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACSubTotal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblExtras.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACExtras, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblTotal.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTotal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblExcess.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblViewOptions.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACViewOptions, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			optView(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNormal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


			optView(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCompressed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblFilter.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACFilter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
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
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub CreateListView()
		
		Dim ColumnX As ColumnHeader
		
		Select Case m_sLossScheduleType
			Case ACLSTypeGeneral
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemClaimed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwSettlementMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwStartingValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwAge, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwLife, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDepreciation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwGST, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPaymentAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPayeeOrSupplier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPODate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDatePaid, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwSalvage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
			Case ACLSTypeMVPC
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDamagedArea, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwRepairable, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwModelNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPartsRequest, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=CInt(AClvwStripFit), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwStripFitHrs, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=CInt(AClvwParts), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=CInt(AClvwFreight), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=CInt(AClvwPaint), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPaintHrs, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=CInt(AClvwPanel), iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPanelHrs, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwOutwork, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwGST, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPaymentAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPayeeOrSupplier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPODate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDatePaid, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				ColumnX = lvwItems.Columns.Add("", 94)

				ColumnX.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwSalvage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		End Select
		
		'Resize listview
        ListViewFunc.ListViewAutoSize(lvwItems, True, True)
		
	End Sub
	
	Private Function GetLossScheduleTypes(ByRef r_vDataArray( ,  ) As Object) As Integer
		' ***************************************************************** '
		' Name:        GetLossScheduleTypes
		'
		' Description:
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oBusiness.GetLossScheduleTypes(r_vDataArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			Return result
		
		Catch excep As System.Exception
			
			result = gPMConstants.PMEReturnCode.PMFalse
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Loss Schedule Types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLossScheduleTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
		End Try
	End Function
	
	Private isInitializingComponent As Boolean
	Private Sub optView_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optView_1.CheckedChanged, _optView_0.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			Dim Index As Integer = Array.IndexOf(optView, eventSender)
			Select Case m_sLossScheduleType
				Case ACLSTypeGeneral
					If Index = 1 Then
						'Compressed mode
						
						'Shorten the column headers


						lvwItems.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwCompItemNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwCompItemClaimed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwCompItemDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwCompSettlementMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(13).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwCompPayeeOrSupplier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
						
                        ListViewFunc.ListViewAutoSize(lvwItems, True, True)
						
						lvwItems.Columns.Item(1).Width = CInt(0)
						lvwItems.Columns.Item(5).Width = CInt(0)
						lvwItems.Columns.Item(6).Width = CInt(0)
						lvwItems.Columns.Item(7).Width = CInt(0)
						lvwItems.Columns.Item(8).Width = CInt(0)
						lvwItems.Columns.Item(15).Width = CInt(0)
						lvwItems.Columns.Item(16).Width = CInt(0)
						lvwItems.Columns.Item(17).Width = CInt(0)
					Else
						'Normal mode
						


						lvwItems.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemClaimed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwItemDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwSettlementMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


						lvwItems.Columns.Item(13).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwPayeeOrSupplier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
						
                        ListViewFunc.ListViewAutoSize(lvwItems, True, True)
					End If
					
				Case ACLSTypeMVPC
					If Index = 1 Then
						'Compressed mode
						
						'Shorten the column headers


						lvwItems.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwCompDamaged, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
						
                        ListViewFunc.ListViewAutoSize(lvwItems, True, True)
						
						lvwItems.Columns.Item(4).Width = CInt(0)
						lvwItems.Columns.Item(7).Width = CInt(0)
						lvwItems.Columns.Item(8).Width = CInt(0)
						lvwItems.Columns.Item(9).Width = CInt(0)
						lvwItems.Columns.Item(10).Width = CInt(0)
						lvwItems.Columns.Item(11).Width = CInt(0)
						lvwItems.Columns.Item(12).Width = CInt(0)
						lvwItems.Columns.Item(13).Width = CInt(0)
						lvwItems.Columns.Item(14).Width = CInt(0)
						lvwItems.Columns.Item(15).Width = CInt(0)
						lvwItems.Columns.Item(16).Width = CInt(0)
						lvwItems.Columns.Item(22).Width = CInt(0)
						lvwItems.Columns.Item(23).Width = CInt(0)
						lvwItems.Columns.Item(24).Width = CInt(0)
						
						
					Else
						'Normal mode
						


						lvwItems.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=AClvwDamagedArea, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
						
                        ListViewFunc.ListViewAutoSize(lvwItems, True, True)
					End If
			End Select
			
		End If
	End Sub
End Class
