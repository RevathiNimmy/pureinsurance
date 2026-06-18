Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Vijay Pal on 5/21/2010 3:24:03 PM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmRates
	Inherits System.Windows.Forms.Form
	'
	' History:
	' CJB 02/03/05 PN19067 Cater for nicer handling of errors if setting up matrix before groups!
	
	
	Private vXData(,) As Object
	Private vYData(,) As Object
	Private vZData(,) As Object
	Private vData( ,  ) As Object
	
	Private m_lMinX As Integer
	Private m_lMaxX As Integer
	Private m_lMinY As Integer
	Private m_lMaxY As Integer
	
	Private m_sZText As String = ""
	
	Private m_bSetup As Boolean
	Private m_bZChanged As Boolean
	
	Private Const m_sMinMaxSeperator As String = " to "
	
	Private m_alDataID( ,  ) As Integer
	Private m_alXAxis() As Integer
	Private m_alYAxis() As Integer
	Private m_lZAxis As Integer
	Private oGridArray As XArrayHelper
	
	Private m_lModifiedCellsTotal As Integer
	Private m_alModifiedXAxis() As Integer
	Private m_alModifiedYAxis() As Integer
	Private m_alModifiedZAxis() As Integer
	Private m_adModifiedRates() As Double
	
	Private m_lReturn As Integer
	Private m_bError As Boolean 'PN19067
	Private m_sRateType As String = ""
	
	'lookup id and description in here
	Private vAxes( ,  ) As Object
	
	Private vMatrix As Object
	
	Public WriteOnly Property RateType() As String
		Set(ByVal Value As String)
			m_sRateType = Value
		End Set
	End Property
	
	Private Sub cboX_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboX.SelectedIndexChanged
		
		Dim sText As String = ""
		
		Try 
			
			sText = cboX.Text
			
			If sText = "" Then
				Exit Sub
			End If
			
			m_lMinX = CInt(sText.Substring(0, sText.IndexOf(m_sMinMaxSeperator))) - 1
			m_lMaxX = CInt(Mid(sText, (sText.IndexOf(m_sMinMaxSeperator) + 1) + m_sMinMaxSeperator.Length - 1)) - 1
			
			If Not m_bSetup Then
				
				GetMatrix()
				
				RefreshDisplay()
				
			End If
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboX", vApp:=ACApp, vClass:=ACClass, vMethod:="cboX_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
		End Try
		
	End Sub
	
	Private Sub cboY_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboY.SelectedIndexChanged
		
		Dim sText As String = ""
		
		Try 
			
			sText = cboY.Text
			
			If sText = "" Then
				Exit Sub
			End If
			
			m_lMinY = CInt(sText.Substring(0, sText.IndexOf(m_sMinMaxSeperator))) - 1
			m_lMaxY = CInt(Mid(sText, (sText.IndexOf(m_sMinMaxSeperator) + 1) + m_sMinMaxSeperator.Length - 1)) - 1
			
			If Not m_bSetup Then
				
				GetMatrix()
				
				RefreshDisplay()
				
			End If
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboY", vApp:=ACApp, vClass:=ACClass, vMethod:="cboY_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
		End Try
		
	End Sub
	
	Private Sub cboZ_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboZ.SelectedIndexChanged
		Try 
			
			If cboZ.Text = "" Then Exit Sub
			
			m_bZChanged = True
			
			GetMatrix()
			
			RefreshDisplay()
			
			m_bZChanged = False
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cboz", vApp:=ACApp, vClass:=ACClass, vMethod:="cboZ_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdCancel_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Try 
			
			TDBGrid.UpdateCurrentRow()
			
			Me.Cursor = Cursors.WaitCursor
			
			SaveMatrix()
			
			Me.Cursor = Cursors.Default
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdOK_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub cmdSave_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSave.Click
		Try 
			
			TDBGrid.UpdateCurrentRow()
			
			Me.Cursor = Cursors.WaitCursor
			
			SaveMatrix()
			
			Me.Cursor = Cursors.Default
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSave_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub frmRates_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			If m_bError Then 'PN19067
				Me.Close()
			End If
			
		End If
	End Sub
	

	Private Sub frmRates_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Try 
			
			m_bError = False
			
			m_bSetup = True
			
			oGridArray = New XArrayHelper()
			
			'initialise arrays

            vXData = Nothing

            vYData = Nothing

            vZData = Nothing
			
			'get data and pop screen
			GetBusiness()
			
			m_bSetup = False
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub GetAxes()
		
        Dim lTop As Integer
		
		Try 
			
			'Retrieve number of axes this matrix uses.

			m_lReturn = g_oBusiness.GetAxes(m_sRateType, g_lGISSchemeID, vAxes)
			If Not Information.IsArray(vAxes) Then
				Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.GetAxes Failed")
			End If
			
			If Strings.Len(CStr(vAxes(0, 0))) > 0 Then
				'Retrieve the columns(x-axis) for this matrix.

				m_lReturn = g_oBusiness.getAxis(g_lGISSchemeID, CInt(vAxes(0, 0)), vXData)
				If Not (Information.IsArray(vXData)) Then
					MessageBox.Show("Please go into Rating Groups and set up your grouping for the X Axis", "Matrix", MessageBoxButtons.OK, MessageBoxIcon.Error)
					m_bError = True
					Exit Sub
				End If
				
				'Populate X-axis combo box
				cboX.Items.Clear()

				For i As Integer = 0 To vXData.GetUpperBound(1) Step 1000
					

					If i + 1000 < vXData.GetUpperBound(1) Then
						cboX.Items.Add(CStr(i + 1) & m_sMinMaxSeperator & CStr(i + 1000))
					Else

						cboX.Items.Add(CStr(i + 1) & m_sMinMaxSeperator & CStr(vXData.GetUpperBound(1) + 1))
					End If
					
				Next 
				
				'Default X-Axis combo box
				cboX.SelectedIndex = 0
				
				'Set the top position of the last viewable combo box.
				lTop = CInt(VB6.PixelsToTwipsY(cboX.Top))
			End If
			
			If Strings.Len(CStr(vAxes(1, 0))) > 0 Then
				'Retrieve the rows(y-axis) for this matrix.

				m_lReturn = g_oBusiness.getAxis(g_lGISSchemeID, CInt(vAxes(1, 0)), vYData)
				If Not (Information.IsArray(vYData)) Then
					MessageBox.Show("Please go into Rating Groups and set up your grouping for the Y Axis", "Matrix", MessageBoxButtons.OK, MessageBoxIcon.Error)
					m_bError = True
					Exit Sub
				End If
				
				'Populate Y-axis combo box
				cboY.Items.Clear()

				For i As Integer = 0 To vYData.GetUpperBound(1) Step 1000
					

					If i + 1000 < vYData.GetUpperBound(1) Then
						cboY.Items.Add(CStr(i + 1) & m_sMinMaxSeperator & CStr(i + 1000))
					Else

						cboY.Items.Add(CStr(i + 1) & m_sMinMaxSeperator & CStr(vYData.GetUpperBound(1) + 1))
					End If
					
				Next 
				
				'Default
				cboY.SelectedIndex = 0
				
				'Set the top position of the last viewable combo box.
				lTop = CInt(VB6.PixelsToTwipsY(cboY.Top))
			Else
				'Disable Y-Axis combo box
				cboY.Visible = False
				lblY.Visible = False
			End If
			
			If Strings.Len(CStr(vAxes(2, 0))) > 0 Then
				'Retrieve the tables(x-axis) for this matrix.

				m_lReturn = g_oBusiness.getAxis(g_lGISSchemeID, CInt(vAxes(2, 0)), vZData)
				If Not (Information.IsArray(vZData)) Then
					MessageBox.Show("Please go into Rating Groups and set up your grouping for the Z Axis", "Matrix", MessageBoxButtons.OK, MessageBoxIcon.Error)
					m_bError = True
					Exit Sub
				End If
				
				'Populate Z combo box
				cboZ.Items.Clear()

				For i As Integer = 0 To vZData.GetUpperBound(1)

                    cboZ.Items.Add(CStr(vZData(0, i)))
                Next

                'Set the top position of the last viewable combo box.
                lTop = CInt(VB6.PixelsToTwipsY(cboZ.Top))
            Else
                'Disable Z-Axis combo box
                cboZ.Visible = False
                lblZ.Visible = False
            End If

            'Expand the grid to fill any gaps due to unviewable combo boxes.
            TDBGrid.Height += (TDBGrid.Top - (lTop + 480))
            TDBGrid.Top = lTop + 480

            'Create zero rate entries for all combinations that don't already have a rate.

            m_lReturn = g_oBusiness.FillMatrix(g_lGISSchemeID, m_sRateType)

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAxes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAxes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub SetAxes()

        Dim oColumn As DataGridViewColumn

        Try

            'Remove existing columns(x)
            With TDBGrid.Columns
                Do While .Count > 0
                    .RemoveAt(0)
                Loop
            End With

            Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing
            'Add columns(x) to matrix (backwards)

            For i As Integer = m_lMaxX To m_lMinX Step -1

                'Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing

                newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
                TDBGrid.Columns.Insert(0, newColumn)
                oColumn = newColumn

                oColumn.Visible = True

                oColumn.HeaderText = CStr(vXData(0, i)).Trim()
            Next

            'Add first column to matrix
            newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
            TDBGrid.Columns.Insert(0, newColumn)
            oColumn = newColumn

            oColumn.Visible = True
            oColumn.HeaderText = "Group Labels"


            If Information.IsArray(vYData) Then
                'Add rows(y) to matrix
                For i As Integer = m_lMinY To m_lMaxY

                    oGridArray(i - m_lMinY, 0) = CStr(vYData(0, i)).Trim()
                Next
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetAxes", vApp:=ACApp, vClass:=ACClass, vMethod:="SetAxes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
	
	
	Private Sub RefreshDisplay()
		

		Try 
			
			Me.Cursor = Cursors.WaitCursor
			

            'Modified by Vijay Pal on 5/21/2010 3:40:16 PM todolist 
            'TDBGrid.ClearFields()
			
			Dim bindingSource As BindingSource = New BindingSource(oGridArray, "")
			TDBGrid.DataSource = bindingSource
			
			If cboZ.Text = "" And Information.IsArray(vZData) Then
				'disable grid
				TDBGrid.Enabled = False
			Else
				'show me
				TDBGrid.Enabled = True
				If m_bZChanged Then
					TDBGrid.ReOpen()
				Else
					TDBGrid.ReBind()
					TDBGrid.MoveFirst()
				End If
			End If
			
			Me.Cursor = Cursors.Default
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Refresh Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Refresh Display", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub SaveMatrix()
		
		
		Try 
			
			'Loop through each modified rate and save it.
			For i As Integer = 0 To m_lModifiedCellsTotal - 1
				
				'Save the rate

				m_lReturn = g_oBusiness.SaveRate(g_lGISSchemeID, m_sRateType, m_alModifiedXAxis(i), m_alModifiedYAxis(i), m_alModifiedZAxis(i), m_adModifiedRates(i))
				
			Next i
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Save Matrix Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveMatrix", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub GetBusiness()
		Try 
			
			'Get axes
			GetAxes()
			
			If m_bError Then
				Exit Sub
			End If
			
			'Get matrix rates
			GetMatrix()
			
			'Display
			RefreshDisplay()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub GetMatrix()
		Dim sXLabel As String = ""
		Dim lX, lY As Integer
		
		Try 
			
			Me.Cursor = Cursors.WaitCursor
			
			'only when theres a zaxis chosen
			If Information.IsArray(vZData) And cboZ.Text = "" Then
				Exit Sub
			End If
			
			'Initialise variables
			lX = 0
			lY = 0
			sXLabel = ""
			
			oGridArray.Clear()
			oGridArray.RedimXArray(New Integer(){m_lMaxY - m_lMinY, m_lMaxX - m_lMinX + 1}, New Integer(){0, 0})
			ReDim m_alDataID(m_lMaxY - m_lMinY, m_lMaxX - m_lMinX + 1)
			ReDim m_alXAxis(m_lMaxX - m_lMinX + 1)
			ReDim m_alYAxis(m_lMaxY - m_lMinY)
			
			'Set the row and column names for the range selected.
			SetAxes()
			
			If Not Information.IsArray(vData) Or m_bZChanged Then
				'Set varible to the Z value that we are going to show.
				m_sZText = cboZ.Text
				
				'Get all the values for the matrix

				m_lReturn = g_oBusiness.GetMatrix(g_lGISSchemeID, m_sRateType, vData, cboZ.Text)
				
			End If
			
			If Information.IsArray(vData) Then
				For i As Integer = 0 To vData.GetUpperBound(1)
					
					'if x value is the same as before
					If CStr(vData(0, i)) = sXLabel And i <> 0 Then
						lY += 1
					Else
						lX += 1
						lY = 0
					End If
					
					
					If lX >= m_lMinX + 1 And lX <= m_lMaxX + 1 And lY >= m_lMinY And lY <= m_lMaxY Then
						
						'Assign ID's to array's
						m_alXAxis(lX - m_lMinX) = CInt(vData(0, i))
						If CStr(vData(1, i)) = "" Then
							m_alYAxis(lY - m_lMinY) = 0
						Else
							m_alYAxis(lY - m_lMinY) = CInt(vData(1, i))
						End If
						If CStr(vData(2, i)) = "" Then
							m_lZAxis = 0
						Else
							m_lZAxis = CInt(vData(2, i))
						End If
						'Assign rate to correct array pos
						oGridArray(lY - m_lMinY, lX - m_lMinX) = vData(3, i)
						m_alDataID(lY - m_lMinY, lX - m_lMinX) = i
						
					End If
					
					'remember x label
					sXLabel = CStr(vData(0, i))
					
				Next 
			End If
			
			If Information.Err().Number <> 0 Then
				MessageBox.Show("get load grid failed", Application.ProductName)
				Exit Sub
			End If
			
			Me.Cursor = Cursors.Default
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMatrix", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMatrix", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	Private Sub TDBGrid_CellUpdated(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles TDBGrid.CellUpdated
		Dim ColIndex As Integer = eventArgs.ColumnIndex
		
		ReDim Preserve m_alModifiedXAxis(m_lModifiedCellsTotal + 1)
		ReDim Preserve m_alModifiedYAxis(m_lModifiedCellsTotal + 1)
		ReDim Preserve m_alModifiedZAxis(m_lModifiedCellsTotal + 1)
		ReDim Preserve m_adModifiedRates(m_lModifiedCellsTotal + 1)
		
		m_alModifiedXAxis(m_lModifiedCellsTotal) = m_alXAxis(ColIndex)


        'Modified by Vijay Pal on 5/21/2010 3:39:39 PM todolist
        'm_alModifiedYAxis(m_lModifiedCellsTotal) = m_alYAxis(CInt(TDBGrid.GetBookmark(0)))
		m_alModifiedZAxis(m_lModifiedCellsTotal) = m_lZAxis
		m_adModifiedRates(m_lModifiedCellsTotal) = CDbl(TDBGrid.CurrentCell.Value)
		


        'Modified by Vijay Pal on 5/21/2010 3:39:58 PM todolist 
        'vData(3, m_alDataID(CInt(TDBGrid.GetBookmark(0)), ColIndex)) = TDBGrid.CurrentCell.Value
		
		m_lModifiedCellsTotal += 1
		
	End Sub
End Class
