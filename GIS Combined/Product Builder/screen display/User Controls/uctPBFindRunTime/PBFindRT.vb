Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129    
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PBFindRT_NET.PBFindRT")> _
Public Partial Class PBFindRT
	Inherits System.Windows.Forms.UserControl
	' database class
	Private m_oDatabase As dPMDAO.Database
	
	Private m_vDataArray( ,  ) As Object
	
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public m_oBusiness As Object
	
	' Close Database Flag (Private)
	Private m_bCloseDatabase As Boolean
	
	'the id of this particular control on the risk screen ( from GIS screen details )
	Private m_lFindControlID As Integer
	
	'fired in order to wipe out the values in the risk screen
	Public Event ClearValues(ByVal Sender As Object, ByVal e As EventArgs)
	
	'fired in order to allow the updating of the values on the risk screen
	Public Event FoundValues(ByVal Sender As Object, ByVal e As EventArgs)
	
	'fired in order to load in screen values
	Public Event StartFind(ByVal Sender As Object, ByVal e As EventArgs)
	
	Private m_bDisableWildcardSearchOption As Boolean
	Private m_bEnablePartialWildcardSearchOption As Boolean
	Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
	Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
	
    Private m_lInsurancefileCnt As Long
    Private m_lClaimId As Long

    Public Property InsuranceFileCnt() As Integer
        Set(ByVal value As Integer)
            m_lInsurancefileCnt = value
        End Set
        Get
            Return m_lInsurancefileCnt
        End Get
    End Property

    Public Property ClaimCnt() As Long
        Set(ByVal value As Long)
            m_lClaimId = value
        End Set
        Get
            Return m_lClaimId
        End Get

    End Property


	'properties
	'find control id
	<Browsable(False)> _
	Public WriteOnly Property FindControlID() As Integer
		Set(ByVal Value As Integer)
			m_lFindControlID = Value
		End Set
	End Property
	
	'search data array ( controltag, searchvalue, foundvalue ) passed bewteen control and risk screen
    <Browsable(True)> _
    Public Property DataArray() As Object(,)  'developer guide no.33
        Get
            Return VB6.CopyArray(m_vDataArray)
        End Get
        Set(ByVal Value As Object(,))  'developer guide no.33
            m_vDataArray = Value
        End Set
    End Property
	
	<Browsable(False)> _
	Public Shadows WriteOnly Property Enabled() As Boolean
		Set(ByVal Value As Boolean)
			btnClear.Enabled = Value
			btnFind.Enabled = Value
		End Set
	End Property
	
	Private Sub btnClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnClear.Click
		RaiseEvent ClearValues(Me, Nothing)
	End Sub
	
	Private Sub btnFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnFind.Click
		
		'get screen data
		RaiseEvent StartFind(Me, Nothing)
		
	End Sub
	
	Public Sub Find()
		
		Try 
            'developer guide no. 69
            Dim frmList As frmList = New frmList
			Dim sSQL As String = ""
            Dim vResultArray(,) As Object
			Dim oListItem As ListViewItem
            Dim sColumnName, sWidth As String
			Dim bIsFilterSupplied As Boolean
			Dim sWildcardErrorMessage As String = ""
			
			bIsFilterSupplied = True
			sWildcardErrorMessage = ""
			
			For iCount As Integer = 0 To m_vDataArray.GetUpperBound(ACControl - 1)
				If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=gPMFunctions.ToSafeString(m_vDataArray(ACSearchValue, iCount)), r_sErrorMessage:=sWildcardErrorMessage) Then
					MessageBox.Show(sWildcardErrorMessage, "Information")
					bIsFilterSupplied = False
					Exit For
				End If
			Next 
			
			'if filter criteria is supplied then procede further else displays
			'message to user
			If bIsFilterSupplied Then
				'get find results

				m_lReturn = m_oBusiness.finddata(m_vDataArray, vResultArray)
			Else
				MessageBox.Show("Please provide filter criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Exit Sub
			End If
			
			'check we have data
			If Not Information.IsArray(vResultArray) Then
				MessageBox.Show("No results found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Exit Sub
			End If
			
			'send in array

            frmList.DataArray = m_vDataArray
			
			'display results
			'add column headers
            		With frmList.lvwList
				
				For i As Integer = 0 To m_vDataArray.GetUpperBound(ACControl - 1)
					
					' get custom column name
					sColumnName = CStr(m_vDataArray(kMappingGridCaption, i)).Trim()
					
					' if there isnt a custom column name
					If sColumnName = "" Then
						' use the default field name from the lookup table
						sColumnName = CStr(m_vDataArray(kMappingViewFieldName, i)).Trim()
					End If
					
					.Columns.Add(sColumnName, 94)
					
				Next i
				.View = View.Details
				.GridLines = True
			End With
			
			'add data
			With frmList.lvwList

				For i As Integer = 0 To vResultArray.GetUpperBound(ACControl - 1)

					For j As Integer = 0 To vResultArray.GetUpperBound(ACProperty - 1)
						'if first time around add list item
						If j = 0 Then

                            oListItem = .Items.Add(CStr(vResultArray(j, i)).Trim())
                        Else

                            ListViewHelper.GetListViewSubItem(oListItem, j).Text = CStr(vResultArray(j, i)).Trim()
						End If
					Next j
				Next i
			End With
			
			' size columns
            With frmList.lvwList 
				
				For i As Integer = 0 To m_vDataArray.GetUpperBound(ACControl - 1)
					
					' if a custom width has not been supplied
					If CStr(m_vDataArray(kMappingGridWidth, i)) = "" Then
						sWidth = "AUTO"
					Else
						' else use the custom width
						sWidth = CStr(gPMFunctions.ToSafeLong(CStr(m_vDataArray(kMappingGridWidth, i)), 0))
					End If
					
					' size the column
     
                    frmList.SizeColumn(i + 1, sWidth)

					
				Next i
				
			End With
			

            m_lReturn = CType(iPMFunc.SetWindowPlacement(frmList.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)
      

            'If frmList.lvwList.Items.Count > 0 Then
            '    frmList.lvwList.Items(0).Focused = True
            '    frmList.lvwList.Items(0).Selected = True
            'End If
           
            frmList.ShowDialog(Me)
 
     
            'Me.set_DataArray(frmList.DataArray)
            Me.DataArray = frmList.DataArray

			
			'fire code to transfer values to screen
			If m_bFoundValues Then
				RaiseEvent FoundValues(Me, Nothing)
			End If
		
		Catch excep As System.Exception
			
			
			TempFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed find method", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
		End Try
		
	End Sub
	
	
	Private Function Initialise() As gPMConstants.PMEReturnCode
		'CJR 17/3/2003 converted initialise event to a function to prevent obdject creation in VB design time.
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Try 
			

			g_oObjectManager = New bObjectManager.ObjectManager()
			
			' Call the initialise method.
			m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACAPP)
			
			'get business object
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPBFindControl.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			
			
            m_oBusiness.InsuranceFileCnt = m_lInsurancefileCnt
            m_oBusiness.ClaimCnt = m_lClaimId

			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			TempFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
		End Try
	End Function
	
	
	
	Private Sub PBFindRT_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		'set to default size
		MyBase.Height = VB6.TwipsToPixelsY(555)
		MyBase.Width = VB6.TwipsToPixelsX(2895)
		
	End Sub
	
	Public Sub Start()
		Dim sValue As String = ""
		Const kMethodName As String = "Start"
		Try 
			
			'CJR 17/3/2003 converted initialise event to a function.
			m_lReturn = Initialise()
			If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
				Exit Sub
			End If
			
			' Get System Option for Disable Wildcard Search
			m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
				Exit Sub
			End If
			m_bDisableWildcardSearchOption = (sValue = "1")
			
			' Get System Option for m_bEnablePartialWildcardSearchOption
			m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
				Exit Sub
			End If
			m_bEnablePartialWildcardSearchOption = (sValue = "1")
			
			Dim sSQLString As String = ""
            Dim vResultArray(,) As Object
			
			'start business object

			m_lReturn = m_oBusiness.Start
			
			'get mappings

			m_lReturn = m_oBusiness.getMappings(m_lFindControlID, vResultArray)
			
			If Information.IsArray(vResultArray) Then

				m_vDataArray = vResultArray
			Else
				MessageBox.Show("Find control not configured", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			End If
		
		Catch excep As System.Exception
			
			
			TempFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
		End Try
		
	End Sub
End Class