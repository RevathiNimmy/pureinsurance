Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctLinkedAccounts_NET.uctLinkedAccounts")> _
Public Partial Class uctLinkedAccounts
	Inherits System.Windows.Forms.UserControl
	
	
	
	Private Const ACClass As String = "uctLinkedAccounts"
	
	' objects

	Private m_oBusiness As bSIRCashDeposit.Business
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' generic interface details
	Private m_iTask As Integer
	Private m_iLanguageID As Integer
	Private m_iSourceID As Integer
	Private m_iUserId As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_bViewMode As Boolean
	
	Private m_vCashDeposit( ,  ) As Object
	Private m_vCDLinkedAccountDetails( ,  ) As Object
	
	' Variables
	Private m_lAccountId As Integer
	Private m_bIsInitialised As Boolean
	Private m_lWidth As Integer
	Private m_lHeight As Integer
	Private m_lStatus As Integer
	Private m_iItemCount As Integer
	
	
	<Browsable(True)> _
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	<Browsable(False)> _
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	
	<Browsable(True)> _
	Public Property ViewMode() As Boolean
		Get
			Return m_bViewMode
		End Get
		Set(ByVal Value As Boolean)
			m_bViewMode = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property CDLinkedAccountDetails() As Object
		Get
			Return VB6.CopyArray(m_vCDLinkedAccountDetails)
		End Get
		Set(ByVal Value As Object)
			m_vCDLinkedAccountDetails = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property AccountId() As Integer
		Get
			Return m_lAccountId
		End Get
		Set(ByVal Value As Integer)
			m_lAccountId = Value
		End Set
	End Property
	
	'UPGRADE_NOTE: (7001) The following declaration (get CashDeposit) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function CashDeposit() As Object
		'Return VB6.CopyArray(m_vCashDeposit)
	'End Function
	
	
	<Browsable(True)> _
	Public Property ItemCount() As Integer
		Get
			Return m_iItemCount
		End Get
		Set(ByVal Value As Integer)
			m_iItemCount = Value
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
	'           Created : Sankar : 09-10-2009 :
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
	
	
	' ***************************************************************** '
	' Name: Load
	' Parameters: n/a
	' Description:
	' History:
	'           Created : Sankar : 09-10-2009 :
	' ***************************************************************** '
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
		
		m_lReturn = GetBusiness()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		m_lReturn = PopulateScreen()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
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
			
			
			
			'        m_lReturn = DisplayCaptions()
			'        If m_lReturn <> PMTrue Then
			'          RaiseError kMethodName, "DisplayCaptions Failed", PMLogError
			'        End If
			
			m_lReturn = SetupListView()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
		
		Catch 
		End Try
		
		
		GoTo Finally_Renamed
		' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
		
		' If you want to rollback a transaction or something, do it here
		
Finally_Renamed: 
		
		Return result


		
		Return result
	End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (DisplayCaptions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function DisplayCaptions() As Integer
		'Dim result As Integer = 0
		'Const kMethodName As String = "DisplayCaptions"
		'
		'On Error GoTo Catch_Renamed
		'
		'
		'
		'result = gPMConstants.PMEReturnCode.PMTrue
		'
		'
		'GoTo Finally_Renamed
		'
'Catch_Renamed: '
		'
		' DO Not Call any functions before here or the error will be lost
		'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
		'
		' If you want to rollback a transaction or something, do it here
		'
'Finally_Renamed: '
		'
		'Return result
		'Resume 
		'Return result
	'End Function
	
	'***************************************************************** '
	' Name: SetupListView
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : Sankar : 09-10-2009 :
	'***************************************************************** '
	Private Function SetupListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupListView"
		
		Dim lColWidth As Integer
		Dim sCaption As String = ""
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(SetupCashDepositListView(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
		End If

		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	
	'***************************************************************** '
	' Name: SetupCashDepositListView
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'
	'           Created : Sankar : 09-10-2009 :
	'***************************************************************** '
	Private Function SetupCashDepositListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupCashDepositListView"
		
		Dim lColWidth As Integer
		Dim sCaption As String = ""
		
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lColWidth = CInt((VB6.PixelsToTwipsX(lvwCashDeposit.Width) - 100) / 5)
		
		lvwCashDeposit.Columns.Clear()
		

		sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwCDNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositCDNumber - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Left, -1)
		

		sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositCDBranch - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Left, -1)
		

		sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositCDProduct - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

		sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDateCreated, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositCDDateCreated - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
		

		sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwUserName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
		
		lvwCashDeposit.Columns.Insert(kCashDepositCDUserName - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	Private Function SetCashDepositDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetCashDepositDetails"
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		ReDim m_vCashDeposit(m_vCDLinkedAccountDetails.GetUpperBound(0), m_vCDLinkedAccountDetails.GetUpperBound(1))
		
		For iCounter As Integer = m_vCDLinkedAccountDetails.GetLowerBound(1) To m_vCDLinkedAccountDetails.GetUpperBound(1)
			m_vCashDeposit(MainModule.ENCashDeposit.CDNumber, iCounter) = m_vCDLinkedAccountDetails(MainModule.ENCDLinkedAccountDetail.CashDeposit_Ref, iCounter)
			m_vCashDeposit(MainModule.ENCashDeposit.Branch, iCounter) = m_vCDLinkedAccountDetails(MainModule.ENCDLinkedAccountDetail.Branch, iCounter)
			m_vCashDeposit(MainModule.ENCashDeposit.Product, iCounter) = m_vCDLinkedAccountDetails(MainModule.ENCDLinkedAccountDetail.Product, iCounter)
			m_vCashDeposit(MainModule.ENCashDeposit.DateCreated, iCounter) = m_vCDLinkedAccountDetails(MainModule.ENCDLinkedAccountDetail.Date_Created, iCounter)
			m_vCashDeposit(MainModule.ENCashDeposit.UserName, iCounter) = m_vCDLinkedAccountDetails(MainModule.ENCDLinkedAccountDetail.UserName, iCounter)
			m_vCashDeposit(MainModule.ENCashDeposit.CashDepositID, iCounter) = m_vCDLinkedAccountDetails(MainModule.ENCDLinkedAccountDetail.CashDeposit_ID, iCounter)
		Next 
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DataToInterface"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		'    txtPartyCode.Text = m_sPartyCode
		'    txtPartyName.Text = m_sPartyName
		
		m_lReturn = CType(PopulateCashDeposit(), gPMConstants.PMEReturnCode)
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
	
	' ***************************************************************** '
	' Name: PopulateBankDetails
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'
	' Created : Sankar
	' ***************************************************************** '
	Private Function PopulateCashDeposit() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "PopulateCashDeposit"
		Dim oListItem As ListViewItem
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If gPMFunctions.IsArrayEmpty(m_vCashDeposit) Then
			Return result
		End If
		
		'Set max rows to number of addresses - though must be at least 5
		lvwCashDeposit.Items.Clear()
		
		For iCounter As Integer = m_vCashDeposit.GetLowerBound(1) To m_vCashDeposit.GetUpperBound(1)
			oListItem = lvwCashDeposit.Items.Add(CStr(m_vCashDeposit(MainModule.ENCashDeposit.CDNumber, iCounter)))
			'oListItem.SubItems(kCashDepositCDNumber) = m_vCashDeposit(ENCashDeposit.CDNumber, iCounter)
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositCDBranch - 1).Text = CStr(m_vCashDeposit(MainModule.ENCashDeposit.Branch, iCounter))
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositCDProduct - 1).Text = CStr(m_vCashDeposit(MainModule.ENCashDeposit.Product, iCounter))
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositCDDateCreated - 1).Text = CStr(m_vCashDeposit(MainModule.ENCashDeposit.DateCreated, iCounter))
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositCDUserName - 1).Text = CStr(m_vCashDeposit(MainModule.ENCashDeposit.UserName, iCounter))
		Next iCounter
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		' If you want to rollback a transaction or something, do it here
		
		Finally
		


		

		End Try
		Return result
	End Function
	
	Public Function BusinessToInterface() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "BusinessToInterface"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(PopulateCashDeposit(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateCashDeposit Failed", gPMConstants.PMELogLevel.PMLogError)
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
	
	
	Private Sub lvwCashDeposit_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwCashDeposit.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwCashDeposit.Columns(eventArgs.Column)
		' Sort the data
		m_lReturn = CType(SortListView(v_iIndex:=ColumnHeader.Index + 1 - 1), gPMConstants.PMEReturnCode)
	End Sub
	
	Private Sub UserControl_Initialize()
		SetResize()
	End Sub
	
	Private Sub SetResize()
		Try 
			
			' Set start dimensions
			m_lWidth = CInt(ClientRectangle.Width)
			m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
			
			' Search Block
			uctAnchor.Add(lvwCashDeposit, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
		
		Catch 
		End Try
		
		
		
		
	End Sub
	
	Private Function GetCashDepositDetails() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetCashDepositDetails"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		m_lReturn = m_oBusiness.GetLinkedCashDepositAccounts(r_vGetCashDepositAccounts:=m_vCDLinkedAccountDetails, v_lAccount_ID:=m_lAccountId)
		If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
			ItemCount = 0 ' Sankar - PN 65302
		ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
			gPMFunctions.RaiseError(kMethodName, "GetLinkedCashDepositAccounts Failed", gPMConstants.PMELogLevel.PMLogError)
		ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then 
			m_lReturn = CType(SetCashDepositDetails(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "SetCashDepositDetails Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			ItemCount = m_vCDLinkedAccountDetails.GetUpperBound(1) + 1 ' Sankar - PN 65302
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
		
		If m_lAccountId <> 0 Then
			'Get Cash Deposit Details
			m_lReturn = CType(GetCashDepositDetails(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetCashDepositDetails Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
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
	
	Private Function SortListView(ByVal v_iIndex As Integer) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SortListView"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Tell it that it's not sorted
		ListViewHelper.SetSortedProperty(lvwCashDeposit, False)
		
		' Set the column to sort on
		ListViewHelper.SetSortKeyProperty(lvwCashDeposit, v_iIndex)
		
		' Swap the ascending/descending around
		If ListViewHelper.GetSortOrderProperty(lvwCashDeposit) = SortOrder.Ascending Then
			ListViewHelper.SetSortOrderProperty(lvwCashDeposit, SortOrder.Descending)
		Else
			ListViewHelper.SetSortOrderProperty(lvwCashDeposit, SortOrder.Ascending)
		End If
		
		' Tell it that it's now sorted
		ListViewHelper.SetSortedProperty(lvwCashDeposit, True)
		
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
End Class
