Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Vijay Pal on 5/21/2010 3:23:45 PM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
    'Modified by Vijay Pal on 5/21/2010 3:28:02 PM refer developer guide no. 50
    Dim objfrmAxis As New frmAxis
	Private Const ACClass As String = "frmMain"
	
	Private m_lReturn As Integer
	
	Private m_vDataArray( ,  ) As Object
	
	Private m_vRiskType As Object
	
	' Error
	Private m_lError As Integer
	
	Public Property Error_Renamed() As Integer
		Get
			Return m_lError
		End Get
		Set(ByVal Value As Integer)
			m_lError = Value
		End Set
	End Property
	
	' ***************************************************************** '
	'
	' Name: BusinessToData
	'
	' Description:
	'
	' History: 29/11/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function BusinesstoData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Call the business

			m_lReturn = g_oBusiness.GetRateTypeTable(r_vResultArray:=m_vDataArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			

			Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: DataToInterface
	'
	' Description:
	'
	' History: 29/11/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Dim lstItem As ListViewItem
		Dim sKey, sText As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the list
			lvwRateTypes.Items.Clear()
			
			If Not Information.IsArray(m_vDataArray) Then
				Return result
			End If
			
			' Display the items
			For lLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)
				
				sKey = "R" & lLoop1
				sText = CStr(m_vDataArray(ACGRTArrayDescription, lLoop1))
				
				lstItem = lvwRateTypes.Items.Add(sText, sText, "")
				
				ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vDataArray(ACGRTArrayDescription1, lLoop1))
				ListViewHelper.GetListViewSubItem(lstItem, 2).Text = CStr(m_vDataArray(ACGRTArrayDescription2, lLoop1))
				ListViewHelper.GetListViewSubItem(lstItem, 3).Text = CStr(m_vDataArray(ACGRTArrayDescription3, lLoop1))
				ListViewHelper.GetListViewSubItem(lstItem, 4).Text = CStr(m_vDataArray(ACGRTArrayRateTypeID, lLoop1))
				
			Next lLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			

			Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			
			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: GetBusiness
	'
	' Description: Gets the data
	'
	' History: 29/11/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the data from the business object
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			m_lReturn = BusinesstoData()
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Show the data
			m_lReturn = DataToInterface()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			

			Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			
			Return result
		End Try
	End Function
	
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		Try 
			
			'show form for adding another lookup
            'Modified by Vijay Pal on 5/21/2010 3:28:26 PM refer developer guide no. 50
            'frmAxis.RateTypeID = 0
            objfrmAxis.RateTypeID = 0
            'Modified by Vijay Pal on 5/21/2010 3:28:34 PM refer developer guide no. 50
            'frmAxis.ShowDialog()
            objfrmAxis.ShowDialog()
			
			'update display
			m_lReturn = GetBusiness()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAdd_click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	
	
	Private Sub cmdDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetails.Click
		
		Dim fRates As frmRates
		
		Try 
			
			'validate
			If lvwRateTypes.FocusedItem Is Nothing Then Exit Sub
			
			'launch rates form
			fRates = New frmRates()
			fRates.RateType = lvwRateTypes.FocusedItem.Text
			fRates.ShowDialog()
			fRates = Nothing
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		Try 
			
			'validation
			If lvwRateTypes.FocusedItem Is Nothing Then Exit Sub
			
			'store values
			
			'send to next form
			Dim m_vRateType(3) As Object
			
			With lvwRateTypes.FocusedItem
				

				m_vRateType(0) = .Text

				m_vRateType(1) = ListViewHelper.GetListViewSubItem(lvwRateTypes.FocusedItem, 1).Text

				m_vRateType(2) = ListViewHelper.GetListViewSubItem(lvwRateTypes.FocusedItem, 2).Text

				m_vRateType(3) = ListViewHelper.GetListViewSubItem(lvwRateTypes.FocusedItem, 3).Text
				
			End With
            'Modified by Vijay Pal on 5/21/2010 4:08:25 PM refer developer guide no. 24
            'frmAxis.set_RateType(m_vRateType)
            objfrmAxis.RateType = m_vRateType
			
			'set ratetypeid
            'Modified by Vijay Pal on 5/21/2010 3:34:44 PM refer developer guide no. 52 and 50
            'frmAxis.RateTypeID = CInt(lvwRateTypes.listViewHelper1.GetListViewSubItem(lvwRateTypes.FocusedItem, 4).Text)
            objfrmAxis.RateTypeID = CInt(lvwRateTypes.FocusedItem.SubItems(4).Text)
			
			'show form
            'Modified by Vijay Pal on 5/21/2010 3:29:54 PM refer developer guide no. 50
            'frmAxis.ShowDialog()
            objfrmAxis.ShowDialog()
			
			'update display
			GetBusiness()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdedit", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdOK", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
			Error_Renamed = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Load the details
			m_lReturn = GetBusiness()
			
			' Set the mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Error_Renamed = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
			
			
			Error_Renamed = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
End Class
