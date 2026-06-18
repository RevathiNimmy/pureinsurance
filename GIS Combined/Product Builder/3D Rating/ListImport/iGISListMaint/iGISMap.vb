Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class FrmMap
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Module Name: frmView
	'
	' Date: 28/06/2002
	'
	' Description:
	'
	' Edit History:
	'   28/06/2002 SJP  - Tidied up after merge from Carole Nash
	' ***************************************************************** '
	
	Private m_vNewItems( ,  ) As Object
	
	'item for holding drag drop info
	Private m_sDraggedCode As String = ""
	Private m_vMappings( ,  ) As Object
	Private m_sListType As String = ""
    Private m_vUDC(,) As Object
    Private FrmMap As FrmMap
    Private FrmImport As FrmImport
	
	' ***************************************************************** '
	'
	' Name: SetListType
	'
	' Description:  This will set the private variable
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Public Sub SetListType(ByRef sListType As String)
		
		Try 
			
			m_sListType = sListType
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set list type", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: SetNewItems
	'
	' Description:  This will set a private variable
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Public Sub SetNewItems(ByRef vData( ,  ) As Object)
		
		Try 
			
			If Information.IsArray(vData) Then
				m_vNewItems = vData
			End If
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set new items array", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNewItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: cmdCancel_Click()
	'
	' Description:  This will unload form
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: cmdClearMapping_Click()
	'
	' Description:  This will clear the mappping
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdClearMapping_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClearMapping.Click
		
		
		Try 
			
			'delete lines
			For i As Integer = 0 To Line1.Length - 1
				If Not (Line1(i) Is Nothing) Then
					If ContainerHelper.GetControlIndex(Line1(i)) = 0 Then
						Line1(0).Visible = False
					Else

                        'TODO LIST
                        'Unload(Line1(i))
                    End If
                End If
            Next

            'blank array
            ReDim m_vMappings(2, 0)

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear mappings", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClearMapping_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: cmdUpdate_Click()
    '
    ' Description:  This will update the business object
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdUpdate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUpdate.Click

        Dim bFound As Boolean

        Try

            '   Checks if this is an array
            If Information.IsArray(m_vMappings) Then
                For i As Integer = 0 To m_vMappings.GetUpperBound(1)
                    'ignore if deleted
                    If CStr(m_vMappings(2, i)) = "Active" Then

                        'Replace custom field details in PM Lookup, list type , old item id, new data, index

                        m_lReturn = m_oBusiness.ReplaceListItem(m_sListType.TrimEnd(), CInt(ListViewHelper.GetListViewSubItem(lvwCustomFields.Items.Item(CInt(m_vMappings(1, i)) - 1), 1).Text), m_vNewItems, CInt(m_vMappings(0, i)))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception("1, cmdUpdate, Failed to update list entry")
                        End If

                        'Add usage

                        m_lReturn = m_oBusiness.addusage("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(m_vNewItems(0, i)), CInt(FrmImport.cboListVersion.Text), CDate(FrmImport.txtEffectiveDate.Text))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception("1, cmdUpdate, Failed to add list entry usage")
                        End If

                    End If
                Next i
            End If

            'add mappings for un mapped new items
            For i As Integer = 1 To lvwNewFields.Items.Count
                'set found flag to false
                bFound = False

                'mark found if in use
                For j As Integer = 0 To m_vMappings.GetUpperBound(1)
                    If CDbl(m_vMappings(0, j)) = i Then bFound = True
                Next j

                If Not bFound Then

                    'Developer Guide No. 101

                    'm_lReturn = m_oBusiness.addusage("UDL_" & str_ListType, m_vNewItems(0, i - 1), CInt(Int_ListVers), DT_EffDate)
                    m_lReturn = m_oBusiness.addusage("UDL_" & m_sListType, m_vNewItems(0, i - 1), CInt(Int_ListVers), DT_EffDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception("1, cmdUpdate, Failed to add list entry usage")
                    End If

                End If

            Next i

            Me.Close()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update list", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUpdate_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: Form_Load()
    '
    ' Description:  This will perform events whilst loading form
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '

    Private Sub FrmMap_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim oListItem As ListViewItem

        Try

            'set up column headers
            With lvwNewFields
                .Columns.Clear()
                .Columns.Add("Code", 94)
                .Columns.Add("Description", 94)
                .View = View.Details
            End With

            With lvwCustomFields
                .Columns.Clear()
                .Columns.Add("Code", 94)
                .Columns.Add("Description", 94)
                .View = View.Details
            End With

            'add new ones
            For i As Integer = 0 To m_vNewItems.GetUpperBound(1)
                oListItem = lvwNewFields.Items.Add(CStr(m_vNewItems(0, i)))
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vNewItems(1, i))
            Next i

            'find user defined ones

            m_lReturn = m_oBusiness.GetUserDefinedCodes(m_sListType, m_vUDC)

            'Add user defined list
            'add new ones
            If Information.IsArray(m_vUDC) Then

                For i As Integer = 0 To m_vUDC.GetUpperBound(1)
                    'display code
                    oListItem = lvwCustomFields.Items.Add(CStr(m_vUDC(1, i)))
                    'put id in sub item
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vUDC(0, i))
                Next i

            End If

            'set up mappings array
            ReDim m_vMappings(2, 0)

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddMapping
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub AddMapping(ByRef lNewIndex As Integer, ByRef lCustomIndex As Integer)


        Try

            'check duplication of line
            If Information.IsArray(m_vMappings) Then
                For i As Integer = 0 To m_vMappings.GetUpperBound(1)

                    If CDbl(m_vMappings(0, i)) = lNewIndex Or CDbl(m_vMappings(1, i)) = lCustomIndex Then
                        'mark as deleted
                        m_vMappings(2, i) = "Deleted"

                    End If

                Next i
            End If

            'resize array
            If CStr(m_vMappings(2, 0)) <> "" Then
                ReDim Preserve m_vMappings(2, m_vMappings.GetUpperBound(1) + 1)
            End If

            'add data
            m_vMappings(0, m_vMappings.GetUpperBound(1)) = lNewIndex
            m_vMappings(1, m_vMappings.GetUpperBound(1)) = lCustomIndex
            m_vMappings(2, m_vMappings.GetUpperBound(1)) = "Active"

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add mapping", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMapping", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: DisplayMapping()
    '
    ' Description:  This will display mappings
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub DisplayMapping()


        Try

            If Information.IsArray(m_vMappings) Then

                'clear lines
                For i_2 As Integer = 0 To Line1.Length - 1
                    If Not (Line1(i_2) Is Nothing) Then
                        If ContainerHelper.GetControlIndex(Line1(i_2)) = 0 Then
                            Line1(0).Visible = False
                        Else

                            'TODO LIST
                            'Unload(Line1(i_2))
                        End If
                    End If
                Next

                'cycle through array
                For i As Integer = 0 To m_vMappings.GetUpperBound(1)

                    'ignore if deleted
                    If CStr(m_vMappings(2, i)) <> "Deleted" Then

                        'create line 1st one already exists
                        If i <> 0 Then
                            'load instance of line
                            ContainerHelper.LoadControl(Me, "Line1", i)

                        End If

                        'set end positions
                        'TODO LIST

                        ' Line1(i).X1 = 3360
                        ' 
                        'Line1(i).Y1 = 1120 + 210 * CDbl(m_vMappings(0, i))
                        '
                        'Line1(i).X2 = 4560
                        '
                        'Line1(i).Y2 = 1120 + 210 * CDbl(m_vMappings(1, i))

                        'display it
                        Line1(i).Visible = True

                    End If
                Next i

            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display mapping", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMapping", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: lvwCustomFields_DragDrop
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '

    Private Sub lvwCustomFields_DragDrop(ByRef Source As Control, ByRef x As Single, ByRef y As Single)

        'Dim lCustomID As Integer

        'Try 

        '	'select dragged ID
        '	'lCustomID = lvwCustomFields.HitTest(x, y).SubItems(1)

        '	lvwCustomFields.FocusedItem = lvwCustomFields.GetItemAt(x, y)

        '	'add to mapping list
        '	AddMapping(lvwNewFields.FocusedItem.Index + 1, lvwCustomFields.FocusedItem.Index + 1)

        '	DisplayMapping()

        'Catch excep As System.Exception


        '	iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to drop item", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCustomfields_Dragdrop", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        'End Try

    End Sub

    ' TODO LIST

    ' ***************************************************************** '
    '
    ' Name: lvwNewFields_MouseDown
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub lvwNewFields_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwNewFields.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Try

            lvwNewFields.FocusedItem = lvwNewFields.GetItemAt(x, y)

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to pick up item", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwNewFields_MouseDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: lvwNewFields_MouseMove
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    ' TODO LIST

    'Private Sub lvwNewFields_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwNewFields.MouseMove
    '	Dim Button As Integer = CInt(eventArgs.Button)
    '	Dim Shift As Integer = Control.ModifierKeys \ &H10000
    '	Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
    '	Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

    '	Try 

    '		If lvwNewFields.FocusedItem Is Nothing Then Exit Sub

    '		If Button = MouseButtonConstants.LeftButton Then

    '			
    '			
    '			lvwNewFields.Drag(vbBeginDrag)

    '		End If

    '	Catch excep As System.Exception


    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to pick up item", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwnewfields_mouseMove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '	End Try

    'End Sub

    Private Sub lvwCustomFields_DragDrop1(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwCustomFields.DragDrop

        Try

            'select dragged ID
            'lCustomID = lvwCustomFields.HitTest(x, y).SubItems(1)

            'lvwCustomFields.FocusedItem = lvwCustomFields.GetItemAt(x, y)

            'add to mapping list
            AddMapping(lvwNewFields.FocusedItem.Index + 1, lvwCustomFields.FocusedItem.Index + 1)

            DisplayMapping()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to drop item", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCustomfields_Dragdrop", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub
End Class
