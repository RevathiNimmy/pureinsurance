Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("CashDeposit_NET.CashDeposit")> _
Public Partial Class CashDeposit
	Inherits System.Windows.Forms.UserControl
	
	Private Const ACClass As String = "uctCashDepositControl"
	
	'Objects
	Private m_oBusiness As Object
	Private m_lReturn As gPMConstants.PMEReturnCode
	' generic interface details
	Private m_iLanguageID As Integer
	Private m_iSourceID As Integer
	Private m_iUserId As Integer
	
	'Variables
	Private m_sCashDepositRef As String = ""
	Private m_sPartyCode As String = ""
	Private m_sPartyName As String = ""
	Private m_sBankCode As String = ""
	Private m_iTask As Integer
	Private m_iSelectedIndex As Integer
	Private m_lPartyCnt As Integer
	Private m_bIsFind As Boolean
    Private m_vCashDepositDetails As Object
	Private m_vCashDepositItem As Object
	Private m_bIsInitialised As Boolean
	Private m_bIsClient As Boolean
	Private m_bIsSinglePolicy As Boolean
	Private m_bFromAgentOrClientMaintenance As Boolean
	
	Private Const ACLockName As String = "CashDeposit"
	
	'Start Arul-(PN 65553-Bug Fixing)
	Private m_sFindCashDepositRef As String = ""
	
	<Browsable(True)> _
	Public Property FindCashDepositRef() As String
		Get
			Return m_sFindCashDepositRef
		End Get
		Set(ByVal Value As String)
			m_sFindCashDepositRef = Value
		End Set
	End Property
	'End Arul-(PN 65553-Bug Fixing)
	
	
	<Browsable(True)> _
	Public Property CashDepositRef() As String
		Get
			Return m_sCashDepositRef
		End Get
		Set(ByVal Value As String)
			m_sCashDepositRef = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property PartyCode() As String
		Get
			Return m_sPartyCode
		End Get
		Set(ByVal Value As String)
			m_sPartyCode = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property PartyName() As String
		Get
			Return m_sPartyName
		End Get
		Set(ByVal Value As String)
			m_sPartyName = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property BankCode() As String
		Get
			Return m_sBankCode
		End Get
		Set(ByVal Value As String)
			m_sBankCode = Value
		End Set
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
	Public Property IsFind() As Boolean
		Get
			Return m_bIsFind
		End Get
		Set(ByVal Value As Boolean)
			m_bIsFind = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property IsClient() As Boolean
		Get
			Return m_bIsClient
		End Get
		Set(ByVal Value As Boolean)
			m_bIsClient = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property FromAgentOrClientMaintenance() As Boolean
		Get
			Return m_bFromAgentOrClientMaintenance
		End Get
		Set(ByVal Value As Boolean)
			m_bFromAgentOrClientMaintenance = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)

			m_lPartyCnt = CInt(Value)
		End Set
	End Property
	
	
	Private Function SetupCashDepositDetailsListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupCashDepositDetailsListView"
		
		Dim lColWidth As Integer
		Dim sCaption As String = ""
		
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lColWidth = CInt((VB6.PixelsToTwipsX(lvwCashDeposit.Width) - 100) / 6)
		
		lvwCashDeposit.Columns.Clear()
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBankName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexBankName, "", sCaption, CInt(VB6.TwipsToPixelsX(2129)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwCDNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexCDNo, "", sCaption, CInt(VB6.TwipsToPixelsX(2800)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexAmount, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAvailableBal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexAvailableBal, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexParty, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexProduct, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexBranch, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwSinglePolicy, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexSinglePolicy, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwUserName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositColHIndexUserName, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		
		
		lvwCashDeposit.LabelEdit = False
		
		If m_bIsFind Then
			lvwCashDeposit.Columns.Item(2).Width = CInt(0)
		End If
		lvwCashDeposit.Columns.Item(7).Width = CInt(0)
		lvwCashDeposit.Columns.Item(8).Width = CInt(0)
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	
	Private Sub cmdCashDepositAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCashDepositAdd.Click
		
		Dim sCaption As String = ""
		
		If m_sPartyCode.Trim() <> "" Then
			If m_sPartyCode.IndexOf("%"c) >= 0 Then
				If MessageBox.Show("Please enter valid party code", "Invalid Party Code", MessageBoxButtons.OK) = System.Windows.Forms.DialogResult.OK Then
					Exit Sub
				End If
				Exit Sub
			End If
			If Not m_bFromAgentOrClientMaintenance Then
				m_lReturn = CType(AddCashDeposit(), gPMConstants.PMEReturnCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					gPMFunctions.RaiseError("cmdAdd_Click", "AddCashDeposit failed", gPMConstants.PMELogLevel.PMLogError)
				End If
			ElseIf (m_bFromAgentOrClientMaintenance) Then 
				cmdCashDepositEdit_Click(cmdCashDepositEdit, New EventArgs())
			End If
		Else

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kCashDepositSelectClientOrAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			MessageBox.Show(sCaption, "Invalid Party Code", MessageBoxButtons.OK)
		End If
	End Sub
	
	Private Function AddCashDeposit() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "AddCashDeposit"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		Dim sLockedBy As String = ""
		

		m_lReturn = m_oBusiness.LockKey(v_sKeyName:=ACLockName.Trim(), v_lKeyValue:=m_lPartyCnt, v_lUserID:=m_iUserId, r_sLockedBy:=sLockedBy)
		
		Dim oCashDeposit As frmCashDepositDetails
		If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			
			oCashDeposit = New frmCashDepositDetails()
			m_sCashDepositRef = ""
			
			'Set the Properties
			oCashDeposit.Task = gPMConstants.PMEComponentAction.PMAdd
			oCashDeposit.PartyCnt = m_lPartyCnt
			oCashDeposit.PartyCode = m_sPartyCode
			oCashDeposit.PartyName = m_sPartyName
			oCashDeposit.CashDepositRef = m_sCashDepositRef
			oCashDeposit.IsClient = m_bIsClient
			oCashDeposit.bLocked = True
			oCashDeposit.ShowDialog()
			
			If oCashDeposit.Status = gPMConstants.PMEReturnCode.PMOK And IsFind Then
				m_lReturn = CType(LoadCashDepositScreen(gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					gPMFunctions.RaiseError(kMethodName, "LoadCashDepositScreen failed", gPMConstants.PMELogLevel.PMLogError)
				End If
			ElseIf oCashDeposit.Status = gPMConstants.PMEReturnCode.PMOK And Not IsFind Then 
				m_lReturn = GetBusiness()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					gPMFunctions.RaiseError(kMethodName, "GetBusiness failed", gPMConstants.PMELogLevel.PMLogError)
				End If
				
				m_lReturn = PopulateCashDepositDetailsList()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					gPMFunctions.RaiseError(kMethodName, "PopulateCashDepositDetailsList failed", gPMConstants.PMELogLevel.PMLogError)
				End If
			End If
		Else
			If sLockedBy = "ERROR" Then
				MessageBox.Show("Failed to lock Client/Agent code : " & m_sPartyCode, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			Else
				MessageBox.Show("The Client/Agent : " & m_sPartyCode & " is being locked by " & sLockedBy, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
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
	
	'UPGRADE_NOTE: (7001) The following declaration (SetCashDepositItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function SetCashDepositItem(ByVal vCashDepositItem() As Object, ByVal lIndex As Integer) As Integer
		'
		'Dim result As Integer = 0
		'Const kMethodName As String = "SetCashDepositItem"
		'On Error GoTo Catch_Renamed
		'
		'result = gPMConstants.PMEReturnCode.PMTrue
		'

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.AvailableBal, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.AvailableBal)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.BankName, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.BankName)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Branches, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.Branches)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.CashDepositID, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.CashDepositID)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Amount, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.Amount)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.CashDepositRef, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.CashDepositRef)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Is_SinglePolicy, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.Is_SinglePolicy)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.PartyId, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.PartyId)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Products, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.Products)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.PartyName, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.PartyName)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.AccountId, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.AccountId)

		'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Is_Deleted, lIndex) = vCashDepositItem(MainModule.ENCashDepositDBDetails.Is_Deleted)
		'
		'GoTo Finally_Renamed
'Catch_Renamed: '
		' DO Not Call any functions before here or the error will be lost
		'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
		'
		' If you want to rollback a transaction or something, do it here
'Finally_Renamed: '
		'Return result
		'Resume 
		'Return result
	'End Function
	
	
	Private Function PopulateCashDepositDetailsList() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "PopulateCashDepositDetailsList"
		Dim oListItem As ListViewItem
		Dim sBGStatusDescription As String = ""
		
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = ClearListView()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return result
		End If
		
		m_lReturn = SetupCashDepositDetailsListView()
		If (gPMFunctions.IsArrayEmpty(m_vCashDepositDetails)) Or Not Information.IsArray(m_vCashDepositDetails) Or m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
			SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=False)
			Return result
		End If
		
		For i As Integer = m_vCashDepositDetails.GetLowerBound(1) To m_vCashDepositDetails.GetUpperBound(1)
			If m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.CashDepositID, i) <> gPMConstants.PMEReturnCode.PMNotFound Then
				oListItem = lvwCashDeposit.Items.Add(CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.BankName, i)).Trim())
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexCDNo).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.CashDepositRef, i)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexAmount).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Amount, i)).Trim())))
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexAvailableBal).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.AvailableBal, i)).Trim())))
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexParty).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.PartyName, i)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexProduct).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Products, i)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexBranch).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Branches, i)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexSinglePolicy).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.Is_SinglePolicy, i)).Trim()
				ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexUserName).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.UserName, i)).Trim()
				oListItem.Tag = CStr(m_vCashDepositDetails(MainModule.ENCashDepositDBDetails.CashDepositID, i)).Trim()
			End If
		Next i
		
		If m_sPartyCode.Trim() <> "" Then
			SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=True)
		End If
		

		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally



		End Try
		Return result
	End Function
	
	
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetBusiness"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = GetCashDepositDetails()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetCashDepositDetails Failed", gPMConstants.PMELogLevel.PMLogError)
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
	
	
	Private Function GetCashDepositDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetCashDepositDetails"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If m_bIsFind Then

			m_lReturn = m_oBusiness.GetCashDepositDetails(r_vCashDepositDetails:=m_vCashDepositDetails, v_sParty_Code:=m_sPartyCode, v_sCashDeposit_Ref:=m_sCashDepositRef, v_sBankCode:=m_sBankCode)
		Else

			m_lReturn = m_oBusiness.GetLinkedCashDepositAccounts(r_vGetCashDepositAccounts:=m_vCashDepositDetails, v_lPartyCnt:=m_lPartyCnt)
		End If
		If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
			' Do Nothing
		ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
			gPMFunctions.RaiseError(kMethodName, "GetCashDepositDetails Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
	
	
	Public Function Load_Renamed() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Load"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Display all language specific captions.
		m_lReturn = SetInterfaceDefaults()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		'Start - Sankar - PN 65555
		If m_lPartyCnt <= 0 Then
			Return result
		End If
		'End - Sankar - PN 65555
		
		m_lReturn = GetBusiness()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetCaseClaimLink Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		m_lReturn = PopulateScreen()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateCaseClaimList Failed", gPMConstants.PMELogLevel.PMLogError)
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
	
	
	Public Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetInterfaceDefaults"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		m_lReturn = SetupListView()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
		If FromAgentOrClientMaintenance Then
			cmdCashDepositAdd.Text = "View"
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
	
	Private Function PopulateScreen() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "PopulateScreen"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = DataToInterface()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally



		End Try
		Return result
	End Function
	
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DisplayCaptions"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If Not m_bFromAgentOrClientMaintenance Then

            cmdCashDepositAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		ElseIf (m_bFromAgentOrClientMaintenance) Then 

            cmdCashDepositAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositViewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		End If
		

        cmdCashDepositEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCashDepositEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		

		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally



		End Try
		Return result
	End Function
	
	'***************************************************************** '
	' Name: SetupListView
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'***************************************************************** '
	Private Function SetupListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupListView"
		Dim lColWidth As Integer
		Dim sCaption As String = ""
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(SetupCashDepositDetailsListView(), gPMConstants.PMEReturnCode)
		

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	Public Sub SetupInViewOnlyMode(Optional ByVal EnableAdd As Boolean = False, Optional ByVal EnableEdit As Boolean = False)
		
		Const kMethodName As String = "SetupInViewOnlyMode"
		Try
		
		If Not EnableAdd Then
			cmdCashDepositAdd.Enabled = False
		ElseIf EnableAdd Then 
			cmdCashDepositAdd.Enabled = True
		End If
		If Not EnableEdit Then
			cmdCashDepositEdit.Enabled = False
		ElseIf EnableEdit Then 
			cmdCashDepositEdit.Enabled = True
		End If
		If m_bFromAgentOrClientMaintenance Then
			If lvwCashDeposit.Items.Count > 0 Then
				cmdCashDepositAdd.Enabled = True
				cmdCashDepositEdit.Enabled = False
			ElseIf lvwCashDeposit.Items.Count <= 0 Then 
				cmdCashDepositAdd.Enabled = False
				cmdCashDepositEdit.Enabled = False
			End If
		End If

        If (lvwCashDeposit.Items.Count > 0) Then
            If IsNothing(lvwCashDeposit.FocusedItem) Then
                lvwCashDeposit.Items(0).Selected = True
                lvwCashDeposit.Items(0).Focused = True
            End If
        End If


		Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=0, excep:=ex)
        ' If you want to rollback a transaction or something, do it here

		Finally
       
       		End Try
	End Sub
	
	' ***************************************************************** '
	' Name: Initialise
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	' ***************************************************************** '
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Initialise"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Check if already initialised
		If m_bIsInitialised Then
			Return result
		End If
		
		' Create an instance of the object manager.
		g_oObjectManager = New bObjectManager.ObjectManager()
		
		' Call the initialise method.
		m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' If UserID is 0 assume that user cancelled logon
		If g_oObjectManager.UserID = 0 Then
			' Exit application
			result = gPMConstants.PMEReturnCode.PMFalse
			Return result
		End If
		
		' Store the language ID from the object manager to the public variables,
		' to enable us to use them throughout the object.
		With g_oObjectManager
			m_iLanguageID = .LanguageID
			m_iSourceID = .SourceID
			m_iUserId = .UserID
		End With
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' Get an instance of the business object via the public object manager.
        Dim temp_m_oBusiness As Object = Nothing
		m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCashDeposit.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
		m_oBusiness = temp_m_oBusiness
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRCashDeposit.Business Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' hold Initialised status
		m_bIsInitialised = True
		
		Return result
		
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
	
	'UPGRADE_NOTE: (7001) The following declaration (BuildArrayIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function BuildArrayIndex() As Integer
		'
		'Dim result As Integer = 0
		'Const kMethodName As String = "BuildArrayIndex"
		'On Error GoTo Catch_Renamed
		'
		'
		'result = gPMConstants.PMEReturnCode.PMTrue
		'
		'If Information.IsArray(m_vCashDepositDetails) Then
			'For 'lBounds As Integer = m_vCashDepositDetails.GetLowerBound(1) To m_vCashDepositDetails.GetUpperBound(1)
				'm_vCashDepositDetails(MainModule.ENCashDepositDBDetails.RowIndex, lBounds) = lBounds
			'Next lBounds
		'End If
		'
		'GoTo Finally_Renamed
		'
'Catch_Renamed: '
		' DO Not Call any functions before here or the error will be lost
		'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
		' If you want to rollback a transaction or something, do it here
		'
'Finally_Renamed: '
		'Return result
		'Resume 
		'Return result
	'End Function
	
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DataToInterface"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(PopulateCashDepositDetailsList(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateCashDepositDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally



		End Try
		Return result
	End Function
	
	Private Sub cmdCashDepositEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCashDepositEdit.Click
		If lvwCashDeposit.Items.Count > 0 Then
			If lvwCashDeposit.FocusedItem.Index >= 0 And m_sPartyCode.Trim() <> "" Then
				m_iSelectedIndex = lvwCashDeposit.FocusedItem.Index + 1
				m_sCashDepositRef = ListViewHelper.GetListViewSubItem(lvwCashDeposit.Items.Item(m_iSelectedIndex - 1), kCashDepositColHIndexCDNo).Text
				m_lReturn = CType(EditCashDeposit(), gPMConstants.PMEReturnCode)
			End If
		ElseIf (lvwCashDeposit.Items.Count <= 0) Then 
			SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
		End If
	End Sub
	
	Private Sub lvwCashDeposit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwCashDeposit.Click
		
		Const kMethodName As String = "lvwCashDeposit_Click"
		
		'Start - Sankar - PN 65555
		If m_lPartyCnt > 0 Then
			If lvwCashDeposit.Items.Count > 0 Then
				If lvwCashDeposit.FocusedItem.Index >= 0 Then
					SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=True)
					m_iSelectedIndex = lvwCashDeposit.FocusedItem.Index + 1
					m_sCashDepositRef = ListViewHelper.GetListViewSubItem(lvwCashDeposit.Items.Item(m_iSelectedIndex - 1), kCashDepositColHIndexCDNo).Text
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						gPMFunctions.RaiseError(kMethodName, "Select CashDepositRef Failed", gPMConstants.PMELogLevel.PMLogError)
					End If
				ElseIf (lvwCashDeposit.Items.Count <= 0) And (m_sPartyCode.Trim() <> "") Then 
					SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=False)
				End If
			ElseIf (lvwCashDeposit.Items.Count <= 0) And m_sPartyCode.Trim() <> "" Then 
				SetupInViewOnlyMode(EnableAdd:=True, EnableEdit:=False)
			End If
		Else
			SetupInViewOnlyMode(EnableAdd:=False, EnableEdit:=False)
		End If
		'End - Sankar - PN 65555
		Exit Sub
		
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse)
		' If you want to rollback a transaction or something, do it here
		
		
		
		
	End Sub
	
	Private Function EditCashDeposit() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "EditCashDeposit"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Dim oCashDeposit As New frmCashDepositDetails
		
		'Set the Properties
		If Not m_bFromAgentOrClientMaintenance Then
			oCashDeposit.Task = gPMConstants.PMEComponentAction.PMEdit
		ElseIf (m_bFromAgentOrClientMaintenance) Then 
			oCashDeposit.Task = gPMConstants.PMEComponentAction.PMView
		End If
		oCashDeposit.PartyCnt = m_lPartyCnt
		oCashDeposit.PartyCode = m_sPartyCode
		oCashDeposit.PartyName = m_sPartyName
		oCashDeposit.CashDepositRef = m_sCashDepositRef

		oCashDeposit.CashDepositID = Convert.ToString(lvwCashDeposit.Items.Item(m_iSelectedIndex - 1).Tag)
		oCashDeposit.IsSinglePolicy = CInt(ListViewHelper.GetListViewSubItem(lvwCashDeposit.Items.Item(m_iSelectedIndex - 1), kCashDepositColHIndexSinglePolicy).Text)
		oCashDeposit.PreviousUserName = ListViewHelper.GetListViewSubItem(lvwCashDeposit.Items.Item(m_iSelectedIndex - 1), kCashDepositColHIndexUserName).Text
		
		oCashDeposit.ShowDialog()
		
		If oCashDeposit.Status = gPMConstants.PMEReturnCode.PMOK And IsFind Then
			LoadCashDepositScreen(gPMConstants.PMEComponentAction.PMEdit)
			
			'Start Arul-(PN 65553-Bug Fixing)
			If m_sFindCashDepositRef.Trim() = "" Then
				m_sCashDepositRef = ""
			End If
			
			m_lReturn = GetBusiness()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			m_lReturn = PopulateCashDepositDetailsList()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "PopulateCashDepositDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			'End Arul-(PN 65553-Bug Fixing)
		ElseIf oCashDeposit.Status = gPMConstants.PMEReturnCode.PMOK And Not IsFind Then 
			m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			m_lReturn = CType(PopulateCashDepositDetailsList(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "PopulateCashDepositDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
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
	
	Private Function LoadCashDepositScreen(ByVal Task As Integer) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "LoadCashDepositScreen"

        'developer guide no. 88
        Dim oiSIRCashDeposit As Object
        Dim lReturn As gPMConstants.PMEReturnCode
		Dim v_lAction As Integer
		
		Try 
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			'Create a component
            Dim temp_oiSIRCashDeposit As Object = Nothing
			lReturn = g_oObjectManager.GetInstance(temp_oiSIRCashDeposit, sClassName:="iSIRCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oiSIRCashDeposit = temp_oiSIRCashDeposit
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to initialise iSIRCashDeposit.Interface", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			' Set Process Modes

			lReturn = oiSIRCashDeposit.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			If m_sPartyCode.Trim() <> "" Then

				oiSIRCashDeposit.PartyCnt = m_lPartyCnt

				oiSIRCashDeposit.PartyCode = m_sPartyCode

				oiSIRCashDeposit.PartyName = m_sPartyName

				oiSIRCashDeposit.Task = Task
			End If
			
			' Start the Form

			lReturn = oiSIRCashDeposit.Start
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "iSIRCashDeposit.Start Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			Return result
		
        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here




            Return result
		End Try
	End Function
	
	Public Function ClearListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ClearListView"
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		lvwCashDeposit.Items.Clear()
		
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
	
	Private Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)
		
        Dim lColumnHeaderIndex As Integer
		
		Try 
			
			
			lColumnHeaderIndex = ColumnHeader.Index + 1 - 1
			
			With ListView
				Select Case lColumnHeaderIndex
					Case kCashDepositColHIndexAmount, kCashDepositColHIndexAvailableBal, kCashDepositColHIndexBankName, kCashDepositColHIndexBranch, kCashDepositColHIndexCDNo, kCashDepositColHIndexParty, kCashDepositColHIndexProduct, kCashDepositColHIndexSinglePolicy, kCashDepositColHIndexUserName
                        ListViewHelper.SetSortedProperty(ListView, False)
                        If ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending Then
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        End If
                        'ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
						'Use the special sort function for numerics
						ListViewSortByValue(ListView, lColumnHeaderIndex, ListViewHelper.GetSortOrderProperty(ListView))
					Case ListViewHelper.GetSortKeyProperty(ListView)
						' Set sort order opposite of
                        ' current direction.
                        If ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending Then
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        End If
                        'ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                    Case Else
                        ' Sort by this column (ascending).
                        ListViewHelper.SetSortedProperty(ListView, False)
                        ' Turn off sorting so that the list
                        ' is not sorted twice
                        ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
                        ListViewHelper.SetSortKeyProperty(ListView, lColumnHeaderIndex)
                        ListViewHelper.SetSortedProperty(ListView, True)
                End Select
			End With
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: ListViewSortByValue
	'
	' Description: Sorts the list view based on the column passed, and
	'              the order given.
	'
	' Note : This hasn't been tested on the first column. I suspect
	'        changes might need to be made if sorting on the first
	'        column is needed (CF 060899).
	'
	' ***************************************************************** '
	Private Function ListViewSortByValue(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer
		Dim result As Integer = 0
		Dim cValue As Decimal
		Dim sValue As String = ""
		Dim iIndex As Integer
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Try 
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the column
			'PSL Issue 6479 10/01/2003 should be width 0
			v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For lLoop1 As Integer = 1 To v_oListView.Items.Count
				
				sValue = ""
				' Alix - Check if item or sum-item
				If v_iSourceColumn = 0 Then
					Dim dbNumericTemp As Double
					If Double.TryParse(v_oListView.Items.Item(lLoop1 - 1).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
						' Alix - We are looking at a numeric value
						cValue = CDec(v_oListView.Items.Item(lLoop1 - 1).Text) + 1000000000
					Else
						' Alix - This is not a numeric value, we do a normal sort
						' This can happen when a field is a lookup field. It is defined as
						' a integer (the ID of the lookup), but what is displayed is
						' actually the description of the item.
						sValue = v_oListView.Items.Item(lLoop1 - 1).Text
					End If
				Else
					Dim dbNumericTemp2 As Double
					If Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
						cValue = CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text) + 1000000000
					Else
						sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
					End If
				End If
				If sValue.Trim() = "" Then
					' Alix - If it WAS a numeric value, we format it for the sorting to work
					sValue = StringsHelper.Format(cValue, "0000000000.00")
				End If
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue
				
			Next lLoop1
			
			' Sort now
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			' Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
			
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(iIndex)
			
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			Return result
			
			
			
		End Try
	End Function
	
	Private Sub lvwCashDeposit_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwCashDeposit.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwCashDeposit.Columns(eventArgs.Column)
		
		Const kMethodName As String = "lvwCashDeposit_ColumnClick"
		Try
		
		
		If lvwCashDeposit.Items.Count > 0 Then
			OnColumnClick(lvwCashDeposit, ColumnHeader)
		End If
		

		
		Catch ex As Exception
		
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
		Finally

		End Try
		Exit Sub
	End Sub
End Class
