Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMListReceiptsC_NET.uctCLMListReceiptsC")> _
Public Partial Class uctCLMListReceiptsC
	Inherits System.Windows.Forms.UserControl
	Public Event InitialisedChange()
	Public Event RecoveryTypeChange()
	Public Event RecoveryIDChange()
	Public Event ClaimIDChange()
	Public Event visibleCmdViewChange()
	
	'-----------------------------------------------
	'Local Private Variables declaration section
	Private m_lClaimID As Integer
	Private m_lRecoveryID As Integer
	Private m_vReceiptList As Object
	Private m_oBusiness As Object
    Private m_lReturn As Integer
    Private m_nSalvageAndTPRecoveryReceipts As Integer

	Private m_oCurrencyConvert As bACTCurrencyConvert.Form
	Private m_bInitialised As Boolean
	Private m_bVisible As Boolean
	Private m_iCount As Integer
	Private m_iColumn() As String
    Private m_sSelItem As Integer
	Private m_vRecoveryType As Integer
	'-----------------------------------------------
	
	
	'-----------------------------------------------
	'Local Private Constants declaration section
	Private Const kACReceiptID As Integer = 0
	Private Const kACDate As Integer = 1
	Private Const kACResolvedName As Integer = 2
	Private Const kACPayee As Integer = 3
	Private Const kACAmount As Integer = 4
	Private Const kACCurrency As Integer = 5
	Private Const kACLossAmount As Integer = 6
	Private Const kACBaseAmount As Integer = 7
	Private Const kACReceiptCurrencyID As Integer = 8
	Private Const kACLossCurrencyID As Integer = 9
	Private Const kACBaseCurrencyID As Integer = 10
	Private Const kACTaxAmount As Integer = 11
	Private Const ACClass As String = "MainModule"
	
	'-----------------------------------------------
	
	
	
	<Browsable(True)> _
	Public Property selectedItem() As Integer
		Get
			Return m_sSelItem
		End Get
		Set(ByVal Value As Integer)
			m_sSelItem = Value
		End Set
	End Property
	
	
	
	<Browsable(True)> _
	Public Property CountColumn() As Integer
		Get
			Return m_iCount
		End Get
		Set(ByVal Value As Integer)
			m_iCount = Value
			ReDim m_iColumn(m_iCount - 1)
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property ColumnCaption(ByVal Index As Integer) As String
		Get
			Return m_iColumn(Index)
		End Get
		Set(ByVal Value As String)
			m_iColumn(Index) = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property visibleCmdView() As Boolean
		Get
			Return m_bVisible
		End Get
		Set(ByVal Value As Boolean)
			m_bVisible = Value
			If Not m_bVisible Then
				lvwReceipts.Height = MyBase.Height
				cmdViewReceipts.Visible = False
			Else
				lvwReceipts.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(MyBase.Height) - VB6.PixelsToTwipsY(cmdViewReceipts.Height) - 300)
				cmdViewReceipts.Visible = True
			End If
			RaiseEvent visibleCmdViewChange()
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property ClaimId() As Integer
		Get
			Return m_lClaimID
		End Get
		Set(ByVal Value As Integer)
			m_lClaimID = Value
			RaiseEvent ClaimIDChange()
		End Set
    End Property

    <Browsable(True)> _
    Public Property SalvageAndTPRecoveryReceipts() As Integer
        Get
            Return m_nSalvageAndTPRecoveryReceipts
        End Get
        Set(ByVal Value As Integer)
            m_nSalvageAndTPRecoveryReceipts = Value
            RaiseEvent ClaimIDChange()
        End Set
    End Property
	
	
	<Browsable(True)> _
	Public Property RecoveryID() As Integer
		Get
			Return m_lRecoveryID
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryID = Value
			RaiseEvent RecoveryIDChange()
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property RecoveryType() As Integer
		Get
			Return m_vRecoveryType
		End Get
		Set(ByVal Value As Integer)
			m_vRecoveryType = Value
			RaiseEvent RecoveryTypeChange()
		End Set
	End Property
	
	Private Property Initialised() As Boolean
		Get
			Return m_bInitialised
		End Get
		Set(ByVal Value As Boolean)
			m_bInitialised = Value
			RaiseEvent InitialisedChange()
		End Set
	End Property
	
	Private Function prepareListView() As Integer
		For iCount As Integer = 0 To CountColumn - 1
			lvwReceipts.Columns.Insert(iCount, ColumnCaption(iCount), 94)
		Next 
	End Function
	
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
		Try

		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		If Not Initialised Then
			m_lReturn = Initialise()
		End If
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Call to GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
		Else
			Initialised = True
		End If



        m_lReturn = m_oBusiness.GetReceiptList(lClaimId:=m_lClaimID, vRecoveryType:=m_vRecoveryType, r_vReceiptList:=m_vReceiptList, lRecoveryID:=m_lRecoveryID, nSalvageAndTPRecoveryReceipts:=m_nSalvageAndTPRecoveryReceipts)



        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Call to GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		m_lReturn = BusinessToInterface()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Call to GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' dont display the payment id column
		lvwReceipts.Columns.Item(0).Width = CInt(0)
		
		
		Catch ex As Exception
		
		
		
		'LogError
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		Finally
		
		
		End Try
		Return result
	End Function
	
	Private Function Initialise() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Initialise"
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			
			g_oObjectManager = New bObjectManager.ObjectManager()
			m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to Initialise ObjectManager", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to Initialise ObjectManager", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			Dim temp_m_oCurrencyConvert As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oCurrencyConvert = temp_m_oCurrencyConvert
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to Initialise ObjectManager", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Return result
		
        Catch ex As Exception



            'Error Section.GetBusiness = PMError
            'LogError

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
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
		Dim lStart, lEnd As Integer
		'Dim lvwRow As ListItem
		Dim sAmount As String = ""
        Const kMethodName As String = "BusinessToInterface"

        Try

            prepareListView()
            result = gPMConstants.PMEReturnCode.PMTrue

            'Assign the details to the interface.
            '    ListViewBatchStart lvwPayments

            lvwReceipts.Items.Clear()

            If Information.IsArray(m_vReceiptList) Then

                lStart = m_vReceiptList.GetLowerBound(1)

                lEnd = m_vReceiptList.GetUpperBound(1)

                For lRow As Integer = lStart To lEnd

                    lvwReceipts.Items.Insert(lRow, CStr(m_vReceiptList(kACReceiptID, lRow)))


                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(1, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), DateTime.Parse(m_vReceiptList(kACDate, lRow)).ToString("d")))




                    ' Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(2, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), CStr(m_vReceiptList(kACResolvedName, lRow)).Trim()))




                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(3, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), CStr(m_vReceiptList(kACPayee, lRow)).Trim()))


                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vReceiptList(kACReceiptCurrencyID, lRow), vCurrencyAmount:=m_vReceiptList(kACAmount, lRow), vFormattedCurrency:=sAmount)

                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(4, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), sAmount))

                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vReceiptList(kACReceiptCurrencyID, lRow), vCurrencyAmount:=m_vReceiptList(kACTaxAmount, lRow), vFormattedCurrency:=sAmount)


                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(5, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), sAmount))



                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(6, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), m_vReceiptList(kACCurrency, lRow).Trim()))


                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vReceiptList(kACLossCurrencyID, lRow), vCurrencyAmount:=m_vReceiptList(kACLossAmount, lRow), vFormattedCurrency:=sAmount)

                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(7, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), sAmount))

                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vReceiptList(kACBaseCurrencyID, lRow), vCurrencyAmount:=m_vReceiptList(kACBaseAmount, lRow), vFormattedCurrency:=sAmount)

                    'Todo List:check at runtime
                    lvwReceipts.Items.Item(lRow).SubItems.Insert(8, New ListViewItem.ListViewSubItem(lvwReceipts.Items.Item(lRow), sAmount))


                Next lRow

            End If


        Catch ex As Exception

            ListViewFunc.ListViewBatchEnd()

            'Error Section.BusinessToInterface = PMError

            ' LogError.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
		Return result
	End Function
    'Developer Guide no. 11
    Private Sub lvwReceipts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwReceipts.SelectedIndexChanged
        If lvwReceipts.SelectedItems.Count > 0 Then
            selectedItem = CInt(lvwReceipts.SelectedItems(0).Text)
        End If
    End Sub
    Private Sub cmdViewReceipts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewReceipts.Click


        Dim oReceiptList As Object
        Dim temp_oReceiptList As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oReceiptList, sClassName:="iCLMListReceipts.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oReceiptList = temp_oReceiptList

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Cannot get an instance of iCLMListReceipts", "Failed to get business object.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If


        m_lReturn = oReceiptList.InitilizeReceiptItem()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Display message.
            MessageBox.Show("Cannot get an instance of iCLMListReceipts", "Failed to get business object.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub



    Private Sub uctCLMListReceiptsC_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        lvwReceipts.Left = MyBase.ClientRectangle.Left + VB6.TwipsToPixelsX(50)
        lvwReceipts.Width = MyBase.Width - VB6.TwipsToPixelsX(100)
        lvwReceipts.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(MyBase.Height) - VB6.PixelsToTwipsY(cmdViewReceipts.Height) - 300)
        cmdViewReceipts.Left = MyBase.Width - VB6.TwipsToPixelsX(2385)
        cmdViewReceipts.Top = lvwReceipts.Height + VB6.TwipsToPixelsY(150)
    End Sub



    Private Sub UserControl_InitProperties()
        visibleCmdView = True
    End Sub



    'developer guide no. 1 (no solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)

        visibleCmdView = CBool(PropBag.ReadProperty("visibleCmdView", True))
    End Sub



    'developer guide no. 1 (no solution)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("visibleCmdView", m_bVisible, True)
    End Sub
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
