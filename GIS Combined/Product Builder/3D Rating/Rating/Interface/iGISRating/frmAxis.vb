Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Vijay Pal on 5/21/2010 3:23:27 PM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmAxis
	Inherits System.Windows.Forms.Form
	'
	' Edit History:
	' 25/02/05 CJB PN16559 Changed cmdOK_Click to pass new GIS_Scheme_ID parameter which is now
	'              required as we have changed the unique index to be based upon description AND
	'              GIS_Scheme_ID in the GIS_Rate_type table.
	'
    'Modified by Vijay Pal on 5/21/2010 3:24:46 PM refer developer guide no. 50
    Dim objfrmMain As New frmMain
	Private m_lRateTypeID As Integer
	
	Private m_vRateType() As Object
	
	Public WriteOnly Property RateType() As Object()
		Set(ByVal Value() As Object)
			
			m_vRateType = Value
			
		End Set
	End Property
	
	
	Public WriteOnly Property RateTypeID() As Integer
		Set(ByVal Value As Integer)
			
			m_lRateTypeID = Value
			
		End Set
	End Property
	
	
	Private Sub cboZ_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboZ.Enter
		Try 
			
			If cboY.Text = "" Then
				MessageBox.Show("Y Axis value must be set first", "Rate Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
				cboY.Focus()
			End If
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="got focus", vApp:=ACApp, vClass:=ACClass, vMethod:="cboz_gotfocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unload", vApp:=ACApp, vClass:=ACClass, vMethod:="Unload frmaxis", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Try 
			Dim m_lReturn, lListType1, lListType2, lListType3 As Integer
			
			'validate input
			If cboX.Text = "" Or txtDescription.Text = "" Then
				MessageBox.Show("A description and at least an X Axis list type is required", "Rate Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Exit Sub
			End If
			
			'check list type input
			lListType1 = VB6.GetItemData(cboX, cboX.SelectedIndex)
			
			If cboY.Text = "" Then
				lListType2 = 0
			Else
				lListType2 = VB6.GetItemData(cboY, cboY.SelectedIndex)
			End If
			
			If cboZ.Text = "" Then
				lListType3 = 0
			Else
				lListType3 = VB6.GetItemData(cboZ, cboZ.SelectedIndex)
			End If
			
			'update or new save
			If m_lRateTypeID > 0 Then

				m_lReturn = g_oBusiness.saveRateType(g_lGISSchemeID, txtDescription.Text, lListType1, lListType2, lListType3, m_lRateTypeID)
				Me.Close()
				Exit Sub
			End If
			
			'check for duplication  PN16559

			If g_oBusiness.RateInUse(txtDescription.Text, g_lGISSchemeID) Then
				MessageBox.Show("This rate type already exists. Please choose another description", "Rate Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Exit Sub
			End If
			
			'new save to database

			m_lReturn = g_oBusiness.saveRateType(g_lGISSchemeID, txtDescription.Text, lListType1, lListType2, lListType3, m_lRateTypeID)
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OK", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	

	Private Sub frmAxis_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Try 
			
			'get Data needed for form and display it
			GetBusiness()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form Load", vApp:=ACApp, vClass:=ACClass, vMethod:="Form Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub GetBusiness()
		Try 
			Dim vData(,) As Object
			Dim m_lReturn As gPMConstants.PMEReturnCode
			

			m_lReturn = g_oBusiness.GetListTypes(vData)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
			End If
			
			'clear combos
			cboX.Items.Clear()
			cboY.Items.Clear()
			cboZ.Items.Clear()
			
			'show values on the screen
			If Information.IsArray(vData) Then
				
				'cycle through data

				For i As Integer = 0 To vData.GetUpperBound(1)
					Dim cboX_NewIndex As Integer = -1

                    cboX_NewIndex = cboX.Items.Add(CStr(vData(1, i)))

                    VB6.SetItemData(cboX, cboX_NewIndex, CInt(vData(2, i)))
                    Dim cboY_NewIndex As Integer = -1

                    cboY_NewIndex = cboY.Items.Add(CStr(vData(1, i)))

                    VB6.SetItemData(cboY, cboY_NewIndex, CInt(vData(2, i)))
                    Dim cboZ_NewIndex As Integer = -1

                    cboZ_NewIndex = cboZ.Items.Add(CStr(vData(1, i)))

                    VB6.SetItemData(cboZ, cboZ_NewIndex, CInt(vData(2, i)))
				Next i
				
			End If
			
			'what about existing record ?
			If m_lRateTypeID > 0 Then
				
				'disable description
				txtDescription.Enabled = False
                'Modified by Vijay Pal on 5/21/2010 3:25:30 PM refer developer guide no. 50
                'With frmMain.lvwRateTypes.FocusedItem
                With objfrmMain.lvwRateTypes.FocusedItem
					
					txtDescription.Text = CStr(m_vRateType(0))
					cboX.Text = CStr(m_vRateType(1))
					
					If Strings.Len(CStr(m_vRateType(2))) > 0 Then
						cboY.Text = CStr(m_vRateType(2))
					End If
					
					If Strings.Len(CStr(m_vRateType(3))) > 0 Then
						cboZ.Text = CStr(m_vRateType(3))
					End If
				End With
				
			End If
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to getbusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
End Class
