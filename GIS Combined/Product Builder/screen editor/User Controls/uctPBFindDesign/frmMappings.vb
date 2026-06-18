Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
'Modified by Alkesh Kumar on 11/05/2010 16:22:12 refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMappings
	Inherits System.Windows.Forms.Form
	
	Private lFindControlID As Integer
	
	Private g_oObjectManager As bObjectManager.ObjectManager
	Private m_oBusiness As Object
	Private m_vScreenControlArray( ,  ) As Object
	
	Private m_lSelectedRow As Integer
	Private m_lSelectedCol As Integer
	Private m_bValidating As Boolean
	
	Public Event ClearMappings()
    Private ControlTable As New DataTable

	'screen control array
    Public WriteOnly Property ScreenControlArray() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vScreenControlArray = Value
        End Set
    End Property
	
	
	Public Property FindControlID() As Integer
		Get
			Return lFindControlID
		End Get
		Set(ByVal Value As Integer)
			lFindControlID = Value
		End Set
	End Property
	
	Private Sub cboView_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboView.SelectedIndexChanged
		
		Dim sSQL As String = ""
		Dim vViewFields(,) As Object
		Dim ListItem As ListViewItem
		
		Try 
			
            If (cboView.Text = "") Then Exit Sub
			
			'wipe list
			lvwViewFields.Items.Clear()
			
			' its also got to clear down any existing mappings
			

            m_lReturn = m_oBusiness.GetViewFields(cboView.SelectedItem, vViewFields)
			
			If Information.IsArray(vViewFields) Then

				For i As Integer = 0 To vViewFields.GetUpperBound(1)

                    ListItem = lvwViewFields.Items.Add(CStr(vViewFields(0, i)))
                Next i
            End If

            '
            ClearMappings_Renamed()

            ResizeMappingTo()
            DataGridView1.Columns(kGridColumnMappedTo).Visible = True

            'Call ResizeGrid

            ' Exit before error handler
            If Information.IsArray(m_vScreenControlArray) Then
                BindGridData()
            End If
        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to list views", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub cboView_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboView.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Try

            KeyAscii = 0

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try


        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Try

            Me.Close()

        Catch excep As System.Exception


            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub ClearMappings_Renamed()


        Try
            Dim g As System.Drawing.Graphics = Me.CreateGraphics()
            g.Clear(SystemColors.Control)
            'm_iLineCount = Line1.Length

            'If m_iLineCount > 1 Then
            '	'clear lines
            '             'For i As Integer = 1 To m_iLineCount - 1
            '             '	Line1(i).Visible = False
            '             'Next i
            'End If

            m_iMapCount = 0

            ' clear grid mappings
            ' developer guide no. 
            'For lGRidRow As Integer = 1 To MSFlexGrid1.RowsCount - 1
            For lGRidRow As Integer = 0 To DataGridView1.Rows.Count - 1
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnMappedTo).Value = kGridNotMapped
            Next

            If Information.IsArray(m_vMappings) Then
                Erase m_vMappings
            End If


            m_vMappings = Nothing
            ' Exit before error handler

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear mappings", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub cmdClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClear.Click
        Try

            ClearMappings_Renamed()


        Catch excep As System.Exception


            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim sSQL As String = ""
        Dim lControlIndex As Integer
        Dim bFound As Boolean
        Dim lDataArrayItem As Integer

        Try

            'validation
            If Not Information.IsArray(m_vMappings) Or cboView.Text = "" Then
                MessageBox.Show("No mappings specified", "Mapping Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'wipe existing array
            ReDim m_vDataArray(kMappingLastValue, lDataArrayItem)

            'ReDim m_vDataArray(kMappingLastValue, UBound(m_vMappings, 2))

            lDataArrayItem = 0

            'transfer to dataarray

            For i As Integer = 0 To m_vMappings.GetUpperBound(1)


                If CStr(m_vMappings(2, i)) = "Active" Then

                    ReDim Preserve m_vDataArray(kMappingLastValue, lDataArrayItem)

                    ' get general details

                    'm_vDataArray(kMappingFindControlId, lDataArrayItem) = lFindControlID
                    m_vDataArray(kMappingFindControlId, lDataArrayItem) = lFindControlID


                    m_vDataArray(kMappingViewFieldName, lDataArrayItem) = lvwViewFields.Items.Item(CInt(m_vMappings(0, i))).Text

                    m_vDataArray(kMappingFuzzy, lDataArrayItem) = 0

                    m_vDataArray(kMappingViewName, lDataArrayItem) = cboView.Text

                    ' find associated grid item

                    lControlIndex = CInt(m_vMappings(kMappingControlIndex, i))

                    For lRow As Integer = 0 To DataGridView1.Rows.Count - 1

                        bFound = False

                        If lRow = lControlIndex Then


                            m_vDataArray(kMappingControlType, lDataArrayItem) = IIf(DataGridView1.Rows(lRow).Cells(kGridColumnType).Value = kControlTypeText, kControlTypeIdText, kControlTypeIdList)


                            m_vDataArray(kMappingGisObjectId, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnGisObjectId).Value

                            m_vDataArray(kMappingGisPropertyId, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnGisPropertyId).Value

                            m_vDataArray(kMappingObjectName, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnObjectName).Value

                            m_vDataArray(kMappingPropertyName, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnPropertyName).Value

                            m_vDataArray(kMappingControlIndex, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnIndex).Value


                            m_vDataArray(kMappingGridCaption, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnGridCaption).Value

                            m_vDataArray(kMappingGridPosition, lDataArrayItem) = DataGridView1.Rows(lRow).Cells(kGridColumnGridPosition).Value


                            m_vDataArray(kMappingGridWidth, lDataArrayItem) = IIf(DataGridView1.Rows(lRow).Cells(kGridColumnGridWidth).Value = "AUTO", Nothing, DataGridView1.Rows(lRow).Cells(kGridColumnGridWidth).Value)

                            bFound = True
                        End If


                        If bFound Then
                            Exit For

                        End If
                    Next

                    lDataArrayItem += 1

                End If

            Next i


            m_lReturn = m_oBusiness.DeleteMappings(lFindControlID)

            'save new mappings
            If m_vDataArray(kControlTypeIdText, 0) <> "" Then

                m_lReturn = m_oBusiness.AddMappings(m_vDataArray)
                'get (new) control id if mappings were saved
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    lFindControlID = CInt(m_vDataArray(0, 0))
                End If

            Else
                Erase m_vMappings
                Erase m_vDataArray
            End If
            Me.Close()

            ' Exit before error handler

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save mappings", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Set up error trap
        Try

            '    'get business objects and set up data access
            '    Dim oComponentServices As PMServerBusinessCS
            '
            '    Set oComponentServices = New PMServerBusinessCS
            '
            '    m_lReturn& = oComponentServices.CheckDatabase(v_lpmproductFamily:=PMProductFamily, _
            ''                                                  r_bnewinstancecreated:=m_bCloseDatabase, _
            ''                                                  r_ocheckeddatabase:=m_oDatabase, _
            ''                                                  v_vdatabase:=vdatabase)
            '
            '    Set oComponentServices = Nothing



            ' Exit before error handler

            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACAPP)

            'get business object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPBFindControl.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness


            m_lReturn = m_oBusiness.Start()

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise control", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub


    Private Sub frmMappings_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try
            Dim sSQL As String = ""
            Dim vViews(,) As Object
            Dim ListItem As ListViewItem
            Dim iVFIndex, iCTindex As Integer
            Dim Success As Boolean


            'initial values
            m_iLineCount = 0

            'add column headers
            With lvwViewFields
                .Columns.Add("Fields", CInt(.Width * 0.98))
                .View = View.Details
                .GridLines = True
            End With

            With lvwControls
                .Columns.Add("Controls", CInt(.Width * 0.2))
                .Columns.Add("Type", CInt(.Width * 0.2))
                .Columns.Add("Index", CInt(0))
                .Columns.Add("Custom Caption", CInt(.Width * 0.2))
                .Columns.Add("Width", CInt(.Width * 0.2))
                .Columns.Add("Position", CInt(.Width * 0.2))
                .View = View.Details
                .GridLines = True
            End With

            'With DataGridView1
            With ControlTable

                ' clear it down
                .Clear()
                '.datasource = Nothing
                For i As Integer = 0 To 11
                    .Columns.Add()
                Next
            End With


            m_lReturn = m_oBusiness.GetViews(vViews)

            'add to combo
            If Information.IsArray(vViews) Then

                For i As Integer = 0 To vViews.GetUpperBound(1)

                    cboView.Items.Add(CStr(vViews(0, i)))
                Next i
            End If

            'list controls on tab in list view
            Dim lControlCount As Integer
            Dim iMultiplier As Integer
            If Information.IsArray(m_vScreenControlArray) Then

                'alter height of form if there are lots of controls

                iMultiplier = 231
                lControlCount = m_vScreenControlArray.GetUpperBound(1)
                If (lControlCount + 1) * iMultiplier > 3705 Then
                    lvwControls.Height = VB6.TwipsToPixelsY(iMultiplier * (lControlCount + 1))
                    'Begin - TFS Work Item No. 3252 - Jai Prakash 21/08/2012
                    'cmdClear.Top = cmdOK.Top + VB6.TwipsToPixelsY((lControlCount - 15) * iMultiplier)
                    cmdClear.Top = cmdOK.Top
                    'cmdOK.Top = cmdClear.Top
                    'cmdCancel.Top = cmdOK.Top
                    'End - TFS Work Item No. 3252 
                    Me.Height += VB6.TwipsToPixelsY((lControlCount - 15) * iMultiplier)
                End If

                For i As Integer = 0 To lControlCount 'UBound(m_vScreenControlArray, 2)

                    ListItem = lvwControls.Items.Add(CStr(m_vScreenControlArray(1, i)))

                    'set type
                    Select Case m_vScreenControlArray(3, i)
                        Case kControlTypeIdText
                            ListViewHelper.GetListViewSubItem(ListItem, 1).Text = kControlTypeText
                        Case kControlTypeIdList
                            ListViewHelper.GetListViewSubItem(ListItem, 1).Text = kControlTypeList
                    End Select

                    'set control index
                    'object_id, property_id, object_name,property_name, control offset (legacy)
                    ListViewHelper.GetListViewSubItem(ListItem, 2).Text = CStr(m_vScreenControlArray(4, i)) & "." & CStr(m_vScreenControlArray(5, i)) & "." & CStr(m_vScreenControlArray(6, i)) & "." & CStr(m_vScreenControlArray(7, i)) & "." & CStr(m_vScreenControlArray(0, i))
                    'set object id
                    'ListItem.SubItems(3) = m_vScreenControlArray(4, i)
                    ListViewHelper.GetListViewSubItem(ListItem, 3).Text = m_vScreenControlArray(4, i)
                    'set property id
                    'ListItem.SubItems(4) = m_vScreenControlArray(5, i)
                    ListViewHelper.GetListViewSubItem(ListItem, 4).Text = m_vScreenControlArray(5, i)
                Next i

            End If

            BindGridData()

            'get existing mappings if any
            '    sSQL = "SELECT * FROM GIS_Find_Mapping WHERE findcontrol_id=" & m_lFindControlID
            '
            '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=sSQL, _
            ''                                       sSQLName:="", _
            ''                                       bStoredProcedure:=False, _
            ''                                       lNumberRecords:=PMAllRecords, _
            ''                                       vResultArray:=m_vDataArray)




            'if we have a view then populate view listview
            If Information.IsArray(m_vDataArray) Then

                cboView.Text = CStr(m_vDataArray(kMappingViewName, 0))
                cboView_SelectedIndexChanged(cboView, New EventArgs())
            End If

            ' populate additional screen control details
            ' custom captions / width / position

            PopulateScreenControlGridDetails(m_vDataArray)

            SetupGrid()


            'add mappings to map array
            If Information.IsArray(m_vDataArray) Then

                m_vMappings = Nothing
                m_iMapCount = 0

                For i As Integer = 0 To m_vDataArray.GetUpperBound(1)

                    iVFIndex = lvwViewFields.FindItemWithText(CStr(m_vDataArray(kMappingViewFieldName, i)), False, 0, False).Index '+ 1 '2 = column name from lookup table



                    iCTindex = GetControlPosition(CStr(m_vDataArray(kMappingGisObjectId, i)) & "." & CStr(m_vDataArray(kMappingGisPropertyId, i)), CInt(m_vDataArray(kMappingControlType, i))) '8=object id, 9 = property id, 3 is control type
                    Success = Add_Mapping(vVFIndex:=iVFIndex, vCTIndex:=iCTindex)
                Next i

                ' update mapping display
                DisplayMapping()

                'display on screen
            Else
                'no mappings so ensure internal array is blank
                'UPGRADE_WARNING: (1037) Couldn't resolve default property of object m_vMappings. More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
                m_vMappings = Nothing
            End If

            ' Exit before error handler

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load mappings form", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
        End Try
    End Sub

    Private Function GetControlPosition(ByRef objectAndPropertyId As String, ByRef TheType As Integer) As Integer

        Dim result As Integer = 0
        Dim sControlType As String = ""

        Try

            Select Case TheType
                Case 1
                    sControlType = "Text"
                Case 2
                    sControlType = "List"
            End Select

            'get index value of this element in the combo list
            For i As Integer = 0 To lvwControls.Items.Count - 1

                If lvwControls.Items.Item(i).Text <> "" Then
                    If ListViewHelper.GetListViewSubItem(lvwControls.Items.Item(i), 1).Text = sControlType And ListViewHelper.GetListViewSubItem(lvwControls.Items.Item(i), 2).Text.Split("."c)(0) & "." & ListViewHelper.GetListViewSubItem(lvwControls.Items.Item(i), 2).Text.Split("."c)(1) = objectAndPropertyId Then

                        result = i
                    End If
                End If
            Next i

            ' Exit before error handler
            Return result

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get control position", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub BindGridData()
        Dim lRow As Integer
        Dim lControlCount As Integer
        If Information.IsArray(m_vScreenControlArray) Then
            lControlCount = m_vScreenControlArray.GetUpperBound(1)
            m_lReturn = m_oBusiness.GetMappings(lFindControlID, m_vDataArray)

            For i As Integer = 0 To lControlCount

                ' use array pos + 1 as 0 is column header
                lRow = i '+ 1

                If lRow >= ControlTable.Rows.Count Then
                    ControlTable.Rows.Add(CStr(m_vScreenControlArray(0, i)))
                Else
                    ControlTable.Rows(lRow)(kGridColumnBlank) = m_vScreenControlArray(0, i)
                End If

                ControlTable.Rows(lRow)(kGridColumnControls) = m_vScreenControlArray(1, i)

                'set type
                Select Case m_vScreenControlArray(3, i)
                    Case 1
                        ControlTable.Rows(lRow)(kGridColumnType) = "Text"
                    Case 2
                        ControlTable.Rows(lRow)(kGridColumnType) = "List"
                End Select


                ControlTable.Rows(lRow)(kGridColumnIndex) = m_vScreenControlArray(0, i)

                ControlTable.Rows(lRow)(kGridColumnGisObjectId) = m_vScreenControlArray(4, i)
                ControlTable.Rows(lRow)(kGridColumnGisPropertyId) = m_vScreenControlArray(5, i)
                ControlTable.Rows(lRow)(kGridColumnObjectName) = m_vScreenControlArray(6, i)
                ControlTable.Rows(lRow)(kGridColumnPropertyName) = m_vScreenControlArray(7, i)

                ' set the defaults for these columns on the grid
                If lvwViewFields.Items.Count = 0 Then
                    ControlTable.Rows(lRow)(kGridColumnMappedTo) = ""
                Else
                    ControlTable.Rows(lRow)(kGridColumnMappedTo) = kGridNotMapped


                End If
                ControlTable.Rows(lRow)(kGridColumnGridCaption) = ""
                ControlTable.Rows(lRow)(kGridColumnGridWidth) = "AUTO"
                ControlTable.Rows(lRow)(kGridColumnGridPosition) = lRow + 1

            Next

        ElseIf IsNothing(m_vScreenControlArray) Then

            ControlTable.Rows.Add()
            ControlTable.Rows(lRow)(kGridColumnBlank) = ""
            ControlTable.Rows(lRow)(kGridColumnControls) = ""

            ControlTable.Rows(lRow)(kGridColumnType) = ""

            ControlTable.Rows(lRow)(kGridColumnIndex) = ""

            ControlTable.Rows(lRow)(kGridColumnGisObjectId) = ""
            ControlTable.Rows(lRow)(kGridColumnGisPropertyId) = ""
            ControlTable.Rows(lRow)(kGridColumnObjectName) = ""
            ControlTable.Rows(lRow)(kGridColumnPropertyName) = ""

            ' set the defaults for these columns on the grid
            ControlTable.Rows(lRow)(kGridColumnMappedTo) = ""
            ControlTable.Rows(lRow)(kGridColumnGridCaption) = ""
            ControlTable.Rows(lRow)(kGridColumnGridWidth) = ""
            ControlTable.Rows(lRow)(kGridColumnGridPosition) = ""

        End If
        DataGridView1.DataSource = ControlTable


    End Sub
    Private Function Add_Mapping(ByVal vVFIndex As Integer, ByVal vCTIndex As Integer) As Boolean

        Dim result As Boolean = False

        Try


            'If vCTIndex = 0 Then
            '    MessageBox.Show("The links may need clearing and remapping", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    ' exit here
            '    Return result
            'End If

            'get dimensions of current array
            If Information.IsArray(m_vMappings) Then
                'resize array to accommodate new item
                m_iMapCount += 1
                ReDim Preserve m_vMappings(2, m_iMapCount)
            Else
                ReDim m_vMappings(2, 0)
                m_iLineCount = 0
                m_iMapCount = 0
            End If

            'delete old items if involved
            For i As Integer = 0 To m_iMapCount - 1

                If CDbl(m_vMappings(0, i)) = vVFIndex Then

                    m_vMappings(2, i) = "Deleted"
                End If

                If CDbl(m_vMappings(1, i)) = vCTIndex Then

                    m_vMappings(2, i) = "Deleted"
                End If
            Next i

            'add new item

            m_vMappings(0, m_iMapCount) = vVFIndex

            m_vMappings(1, m_iMapCount) = vCTIndex

            m_vMappings(2, m_iMapCount) = "Active"


            ' Exit before error handler
            Return True

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add mapping", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    Private Function DisplayMapping() As Object
        Dim lY1, lY2 As Integer
        Try

            For l As Integer = 0 To m_vMappings.GetUpperBound(1)


                If CStr(m_vMappings(2, l)) = "Active" Then

                    'co-ordinate

                    lY1 = CInt((CDbl(m_vMappings(0, l)) * ACGap) + VB6.TwipsToPixelsY(1150))

                    lY2 = CInt((CDbl(m_vMappings(1, l)) * ACGap2) + VB6.TwipsToPixelsY(1150))

                    ' get the ypos for the grid row


                    AddmappedTo(v_lListRow:=CInt(m_vMappings(0, l)), v_lGridRow:=CInt(m_vMappings(1, l)))


                    'Else
                    '    DataGridView1(m_vMappings(1, l), kGridColumnMappedTo).Value = kGridNotMapped
                End If

            Next

            ' exit before error trap

        Catch excep As System.Exception

            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display mapping", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", excep:=excep)

            Exit Function
        End Try
    End Function

    Private Sub frmMappings_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed


        For lItem As Integer = 1 To Line1.Length - 1
            ContainerHelper.UnloadControl(Me, "Line1", lItem)
        Next

    End Sub




    'Private Sub lvwControls_DragDrop(ByRef Source As Control, ByRef x As Single, ByRef y As Single)
    'Private Sub lvwControls_DragEnter(ByVal sender As System.Object, ByVal events As System.Windows.Forms.DragEventArgs) Handles lvwControls.DragEnter
    Private Sub lvwControls_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwControls.DragDrop



        'Dim x As Integer = Events.X
        'Dim y As Integer = Events.Y


        Try

            '    lvwControls.FocusedItem = lvwControls.GetItemAt(x, y)

            '    If lvwControls.FocusedItem Is Nothing Then



            '        'Modified by Alkesh Kumar on 11/05/2010 16:18:25 refer developer guide no. 13
            '        'lvwViewFields.Drag(vbEndDrag)
            '        lvwViewFields.DoDragDrop(sender, DragDropEffects.Move)

            '        ' Exit here
            '        Exit Sub
            '    End If

            'add mapping and lines
            'bSuccess = Add_Mapping(vVFIndex:=lvwViewFields.FocusedItem.Index + 1, vCTIndex:=lvwControls.FocusedItem.Index + 1)

            '' update mapping display
            'DisplayMapping()

            ' Exit before error trap

        Catch excep As System.Exception


            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to dragdrop item", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Private Sub lvwViewFields_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvwViewFields.DragEnter
        e.Effect = e.AllowedEffect

    End Sub

    Private Sub lvwViewFields_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles lvwViewFields.ItemDrag
        Dim Button As Integer = CInt(e.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Try

            lvwViewFields.DoDragDrop(e.Item, DragDropEffects.Move)

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed mouser down", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Private Sub lvwViewFields_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwViewFields.MouseDown
        'Dim Button As Integer = CInt(eventArgs.Button)
        'Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ''Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        ''Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        'Dim x As Single = (eventArgs.X)
        'Dim y As Single = (eventArgs.Y)
        'Dim point As Point = lvwViewFields.PointToClient(New Point(x, y))
        'Try

        '    'get selected item
        '    lvwViewFields.FocusedItem = lvwViewFields.GetItemAt(point.X, point.Y)
        '    lvwViewFields.Items(lvwViewFields.FocusedItem.Index).Selected = True

        '    If lvwViewFields.FocusedItem Is Nothing Then Exit Sub

        '    ' Exit before error trap
        '    'lvwControls.FocusedItem = lvwControls.GetItemAt(x, y)

        '    'If lvwControls.FocusedItem Is Nothing Then



        '    'Modified by Alkesh Kumar on 11/05/2010 16:18:25 refer developer guide no. 13
        '    'lvwViewFields.Drag(vbEndDrag)
        '    lvwViewFields.DoDragDrop(eventSender, DragDropEffects.Move)

        '    ' Exit here
        '    Exit Sub
        '    'End If

        'Catch excep As System.Exception



        '    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed mouser down", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        'End Try
    End Sub

    Private Sub lvwViewFields_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwViewFields.MouseMove
        'Dim Button As Integer = CInt(eventArgs.Button)
        'Dim Shift As Integer = Control.ModifierKeys \ &H10000
        ''Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        ''Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'Dim x As Single = (eventArgs.X)
        'Dim y As Single = (eventArgs.Y)
        'Try

        '    ' If lvwViewFields.FocusedItem Is Nothing Then Exit Sub

        '    ' If Button = MouseButtonConstants.LeftButton Then
        '    If eventArgs.Button = Windows.Forms.MouseButtons.Left Then


        '        'Modified by Alkesh Kumar on 11/05/2010 16:20:21 refer developer guide no. 13
        '        'lvwViewFields.Drag(vbBeginDrag)
        '        lvwViewFields.DoDragDrop(eventSender, DragDropEffects.Move)
        '        Cursor.Current = Cursors.Arrow

        '    End If

        '    ' Exit before error trap

        'Catch excep As System.Exception



        '    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed mouse move", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        'End Try
    End Sub

    ' ***************************************************************** '
    ' Name: PopulateScreenControlGridDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-09-2005 : Process ID
    ' ***************************************************************** '
    Private Function PopulateScreenControlGridDetails(ByVal v_vGisFindMapping(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateScreenControlGridDetails"

        Dim lReturn As Integer
        Dim oListItem As ListViewItem
        Dim nListItems, llBound, lUBound, lControlIndex, lMappingIndex As Integer
        Dim sCaption As String = ""
        Dim lWidth, lPosition, lIndexStartPos As Integer
        Dim sIndex, sDefaultCaption As String

        Dim lTop, lLeft, lHeight As Integer
        Dim sWidth As String = ""

        Dim lGisObjectId, lGisPropertyId As Integer
        Dim sObjectName, sPropertyName As String


        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' if there mapping items
        If Information.IsArray(v_vGisFindMapping) Then

            ' determine boundaries
            llBound = v_vGisFindMapping.GetLowerBound(1)
            lUBound = v_vGisFindMapping.GetUpperBound(1)

            ' get total number of list items
            nListItems = lvwControls.Items.Count

            ' for each list item
            'For lItem As Integer = 1 To nListItems
            For lItem As Integer = 0 To m_vScreenControlArray.GetUpperBound(1) ' - 1

                ' get the list item
                lIndexStartPos = 0
                lControlIndex = CInt(DataGridView1.Rows(lItem).Cells(kGridColumnIndex).Value)

                ' attempt to find associated entry in array
                For lMappingItem As Integer = llBound To lUBound

                    ' if this mapping item is for the current control

                    If CDbl(v_vGisFindMapping(kMappingControlIndex, lMappingItem)) = lControlIndex Then

                        ' get the mapping details

                        sCaption = CStr(v_vGisFindMapping(kMappingGridCaption, lMappingItem))

                        sDefaultCaption = CStr(v_vGisFindMapping(kMappingViewFieldName, lMappingItem))

                        lWidth = gPMFunctions.ToSafeLong(CStr(v_vGisFindMapping(kMappingGridWidth, lMappingItem)), 0)

                        lPosition = gPMFunctions.ToSafeLong(CStr(v_vGisFindMapping(kMappingGridPosition, lMappingItem)), 0)


                        lGisObjectId = gPMFunctions.ToSafeLong(CStr(v_vGisFindMapping(kMappingGisObjectId, lMappingItem)), 0)

                        lGisPropertyId = gPMFunctions.ToSafeLong(CStr(v_vGisFindMapping(kMappingGisPropertyId, lMappingItem)), 0)

                        sObjectName = CStr(v_vGisFindMapping(kMappingObjectName, lMappingItem))

                        sPropertyName = CStr(v_vGisFindMapping(kMappingPropertyName, lMappingItem))

                        If lWidth = 0 Then
                            sWidth = "AUTO"
                        Else
                            sWidth = CStr(lWidth)
                        End If

                        If lPosition = 0 Then
                            lPosition = lItem
                        End If

                        ' set the columns on the grid
                        DataGridView1.Rows(lItem).Cells(kGridColumnGridCaption).Value = sCaption
                        DataGridView1.Rows(lItem).Cells(kGridColumnGridWidth).Value = sWidth
                        DataGridView1.Rows(lItem).Cells(kGridColumnGridPosition).Value = lPosition

                    End If

                Next

            Next

        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Sub DataGridView1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridView1.DragDrop
        Dim lControlid As Integer

        Dim bFound As Boolean
        Dim lSelectedRow, lSelectedCol, lFirstVisibleRow As Integer
        Dim X As Integer = e.X
        Dim Y As Integer = e.Y
        Dim pt As Point = DataGridView1.PointToClient(New Point(X, Y))
        Try
            Dim hit As DataGridView.HitTestInfo = DataGridView1.HitTest(pt.X, pt.Y)

            ' save the current selection
            lSelectedRow = hit.RowIndex

            lSelectedCol = hit.ColumnIndex
            If lSelectedRow = -1 Or lSelectedCol = -1 Then
                Exit Sub
            End If


            With DataGridView1

                lFirstVisibleRow = DataGridView1.FirstDisplayedScrollingRowIndex
                lFirstVisibleRow = lSelectedRow

                ' dont redraw screen

                'Modified by Alkesh Kumar on 11/05/2010 16:20:27 refer developer guide no. To do list 
                '.Redraw = False

                ' for each row
                'For lRow As Integer = lFirstVisibleRow - 1 To .Rows.Count - 1
                For lRow As Integer = 0 To .Rows.Count - 1

                    ' select the row
                    '.Rows(lRow).Selected = True

                    ' if this selected "Y" position fall within the range of this row

                    If lRow = lSelectedRow Then
                        ' grab the control id
                        lControlid = lRow
                        ' and indicate success
                        bFound = True
                        Exit For
                    End If
                    ' If Y > DataGridView1.SelectedCells(0).Style.Alignment.TopLeft And Y < .SelectedCells(0).Style.Alignment.BottomCenter + .ColumnHeadersHeight Then
                    'If Y > hit.RowY And Y < hit.Then Then
                    '    ' grab the control id
                    '    lControlid = lRow
                    '    ' and indicate success
                    '    bFound = True
                    '    Exit For
                    'End If

                Next lRow

            End With

            ' if a specific row was at the drop position
            If bFound Then

                'add mapping and lines
                Add_Mapping(vVFIndex:=lvwViewFields.FocusedItem.Index + 1 - 1, vCTIndex:=lControlid)

                ' update mapping display
                DisplayMapping()

            Else



                'Modified by Alkesh Kumar on 11/05/2010 16:21:20 refer developer guide no. 13
                'lvwViewFields.Drag(vbEndDrag)

            End If

            ' reset the  original selection
            DataGridView1.Rows(lSelectedRow).Selected = True
            DataGridView1.Columns(lSelectedCol).Selected = True

            ' allow screen to redraw

            'Modified by Alkesh Kumar on 11/05/2010 16:21:27 refer developer guide no.  To do list
            'MSFlexGrid1.Redraw = True

            ' Exit before error trap

        Catch excep As System.Exception



            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to dragdrop item", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
        End Try
    End Sub

    Private Sub DataGridView1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridView1.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub MSFlexGrid1_Scroll(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles DataGridView1.Scroll
        Dim m_vMappingss(50) As Integer


        Dim mappedindex, mappedrow As Integer
        Dim g As System.Drawing.Graphics = Me.CreateGraphics()
        Dim listPen As New System.Collections.Generic.List(Of Pen)
        Dim listPoint As New System.Collections.Generic.List(Of Point())
        Try
            'Get all the mapped row index in an array
            mappedrow = 0
            For mappedindex = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(mappedindex).Cells(1).Value <> "Not Mapped" Then
                    m_vMappingss(mappedrow) = mappedindex
                    mappedrow = mappedrow + 1
                End If
            Next

            If Not m_vMappings Is Nothing Then
                For l As Integer = 0 To m_vMappings.GetUpperBound(1)
                    If CStr(m_vMappings(2, l)) = "Active" Then
                        Dim listIndex As Integer = m_vMappings(0, l)
                        'Dim gridindex As Integer = m_vMappings(1, l)
                        'Dim lY1 As Integer = CInt((CDbl(m_vMappings(0, l)) * ACGap) + VB6.TwipsToPixelsY(1150))

                        'Dim lY2 As Integer = CInt((CDbl(m_vMappings(1, l)) * ACGap2) + VB6.TwipsToPixelsY(1150))

                        'Dim X As Integer = DataGridView1.GetCellDisplayRectangle(1, i, False).Left + DataGridView1.Left
                        Dim Y As Integer = DataGridView1.GetCellDisplayRectangle(1, m_vMappingss(l), False).Bottom + DataGridView1.Top
                        Dim pen As System.Drawing.Pen = New Pen(Color.Black)
                        pen.Width = 2
                        listPen.Add(pen)
                        '21 is the limit of items display on the grid. So if Row are less than 21 set the pivot point to the cordinates of the first Row
                        'when we are scrolling down
                        If DataGridView1.GetCellDisplayRectangle(1, m_vMappingss(l), False).Bottom = 0 And m_vMappingss(l) < 21 Then
                            Y = Y + 41
                            'If rows are more than or equal to 21 then set the pivot point to the cordinates of 21th Row of the grid when we are scrolling up
                        ElseIf DataGridView1.GetCellDisplayRectangle(1, m_vMappingss(l), False).Bottom = 0 And m_vMappingss(l) >= 21 Then
                            Y = Y + 495
                        End If
                        Dim point() As System.Drawing.Point = {New Point(lvwViewFields.Right, lvwViewFields.Items(listIndex).Position.Y + lvwViewFields.Top + 6), New Point(DataGridView1.Left, Y - 20)}
                        listPoint.Add(point)


                    End If
                Next
                g.Clear(Me.BackColor)
                For index As Integer = 0 To listPen.Count - 1

                    g.DrawLines(listPen(index), listPoint(index))
                Next

            End If

        Catch ex As Exception
            Throw ex
        End Try
        DataGridView1.Focus()
    End Sub

    Public Function ValidateTextEntry(Optional ByVal row As Integer = 0, Optional ByVal col As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateTextEntry"

        Dim lReturn As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        m_bValidating = True


        Select Case col
            Case kGridColumnGridWidth

                ' must be numeric
                If DataGridView1.Rows(row).Cells(col).Value <> "0" And DataGridView1.Rows(row).Cells(col).Value.ToUpper() <> "AUTO" Then

                    ' if the value is invalid
                    If gPMFunctions.ToSafeLong(DataGridView1.Rows(row).Cells(col).Value, 0) = 0 Then

                        ' failed
                        result = gPMConstants.PMEReturnCode.PMFalse

                        MessageBox.Show("Grid Width must be numeric OR AUTO", Application.ProductName)
                    ElseIf gPMFunctions.ToSafeLong(DataGridView1.Rows(row).Cells(col).Value, 0) > 10000 Or gPMFunctions.ToSafeLong(DataGridView1.Rows(row).Cells(col).Value, 0) < 0 Then

                        ' failed
                        result = gPMConstants.PMEReturnCode.PMFalse

                        MessageBox.Show("Grid Width must be between 0 and 10000", Application.ProductName)

                    End If

                End If

            Case kGridColumnGridPosition

                ' must be numeric
                If DataGridView1.Rows(row).Cells(col).Value <> "0" Then

                    ' if the value is invalid
                    If gPMFunctions.ToSafeLong(DataGridView1.Rows(row).Cells(col).Value, 0) = 0 Then

                        ' failed
                        result = gPMConstants.PMEReturnCode.PMFalse

                        MessageBox.Show("Grid Position must be numeric", Application.ProductName)
                    ElseIf gPMFunctions.ToSafeLong(DataGridView1.Rows(row).Cells(col).Value, 0) > 100 Or gPMFunctions.ToSafeLong(DataGridView1.Rows(row).Cells(col).Value, 0) < 1 Then

                        ' failed
                        result = gPMConstants.PMEReturnCode.PMFalse

                        MessageBox.Show("Grid Position must be between 1 and 100", Application.ProductName)

                    End If

                End If

        End Select

        m_bValidating = False


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function

   

    ' ***************************************************************** '
    ' Name: AddmappedTo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub AddmappedTo(ByVal v_lListRow As Integer, ByVal v_lGridRow As Integer)

        Const kMethodName As String = "AddmappedTo"

        Dim lReturn, lSubValue As Integer
        Dim sFieldName As String = ""

        Try

            sFieldName = lvwViewFields.Items.Item(v_lListRow).Text
            DataGridView1.Rows(v_lGridRow).Cells(kGridColumnMappedTo).Value = sFieldName

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetupGrid
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub SetupGrid()

        Const kMethodName As String = "SetupGrid"

        Dim lSubValue, lMaxControlWidth, lWidth As Integer

        Try

            ' colour columns
            For lGRidRow As Integer = 0 To DataGridView1.Rows.Count - 1
                DataGridView1.AutoGenerateColumns = False
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnMappedTo).Style.BackColor = ColorTranslator.FromOle(&H8000000F)
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnControls).Style.BackColor = ColorTranslator.FromOle(&H8000000F)
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnType).Style.BackColor = ColorTranslator.FromOle(&H8000000F)
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnMappedTo).ReadOnly = True
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnControls).ReadOnly = True
                DataGridView1.Rows(lGRidRow).Cells(kGridColumnType).ReadOnly = True
            Next
            With DataGridView1
                DataGridView1.Columns(kGridColumnBlank).Width = 0
                DataGridView1.Columns(kGridColumnBlank).Visible = False
                ' populate column headers
                DataGridView1.Columns(kGridColumnMappedTo).HeaderText = "Mapping"
                DataGridView1.Columns(kGridColumnControls).HeaderText = "Controls"
                DataGridView1.Columns(kGridColumnType).HeaderText = "Type"
                DataGridView1.Columns(kGridColumnIndex).HeaderText = "Index"
                DataGridView1.Columns(kGridColumnGridCaption).HeaderText = "Caption"
                DataGridView1.Columns(kGridColumnGridWidth).HeaderText = "Width"
                DataGridView1.Columns(kGridColumnGridPosition).HeaderText = "Position"

                DataGridView1.Columns(kGridColumnGisObjectId).HeaderText = "GisObjectId"
                DataGridView1.Columns(kGridColumnGisPropertyId).HeaderText = "GisPropertyId"
                DataGridView1.Columns(kGridColumnObjectName).HeaderText = "ObjectName"
                DataGridView1.Columns(kGridColumnPropertyName).HeaderText = "PropertyName"

                DataGridView1.Columns(kGridColumnBlank).Width = 0
                DataGridView1.Columns(kGridColumnBlank).Visible = False
                If lvwViewFields.Items.Count = 0 Then
                    DataGridView1.Columns(kGridColumnMappedTo).Width = 0
                    DataGridView1.Columns(kGridColumnMappedTo).Visible = False
                End If
                DataGridView1.Columns(kGridColumnIndex).Width = 0
                DataGridView1.Columns(kGridColumnIndex).Visible = False

                DataGridView1.Columns(kGridColumnGisObjectId).Width = 0
                DataGridView1.Columns(kGridColumnGisObjectId).Visible = False

                DataGridView1.Columns(kGridColumnGisPropertyId).Width = 0
                DataGridView1.Columns(kGridColumnGisPropertyId).Visible = False

                DataGridView1.Columns(kGridColumnObjectName).Width = 0
                DataGridView1.Columns(kGridColumnObjectName).Visible = False

                DataGridView1.Columns(kGridColumnPropertyName).Width = 0
                DataGridView1.Columns(kGridColumnPropertyName).Visible = False

                'Control(widths)
                For lItem As Integer = 0 To DataGridView1.Rows.Count - 1
                    lWidth = Convert.ToString(DataGridView1.Rows(lItem).Cells(kGridColumnControls).Value).Length
                    If lMaxControlWidth < lWidth Then
                        lMaxControlWidth = lWidth
                    End If
                Next

                'size the other columns appropriately

                DataGridView1.Columns(kGridColumnGridCaption).Width = VB6.TwipsToPixelsX(3000)
                DataGridView1.Columns(kGridColumnControls).Width = lMaxControlWidth + (100)
                DataGridView1.Columns(kGridColumnGridWidth).Width = Trim(.Columns(kGridColumnGridWidth).HeaderText).Length + (100)
                DataGridView1.Columns(kGridColumnGridPosition).Width = Trim(.Columns(kGridColumnGridPosition).HeaderText.Length) + (100)
                DataGridView1.Columns(kGridColumnType).Width = .Columns(kGridColumnType).HeaderText.Length + (100)
                DataGridView1.Columns(0).Width = False
                .AllowDrop = True

            End With

            ResizeMappingTo()

            'Call ResizeGrid

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ResizeMappingTo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Sub ResizeMappingTo()

        Const kMethodName As String = "ResizeMappingTo"

        Dim lReturn, lSubValue, lWidth, lMaxViewWidth As Integer

        Try



            ' list view widths
            'For lItem As Integer = 1 To lvwViewFields.Items.Count
            For lItem As Integer = 0 To lvwViewFields.Items.Count - 1


                'lWidth = MSFlexGrid1.Container.TextWidth(lvwViewFields.Items.Item(lItem - 1))
                'lWidth = ReflectionHelper.GetMember(MSFlexGrid1.Container, "TextWidth", New Object() {lvwViewFields.Items.Item(lItem - 1)})
                lWidth = lvwViewFields.Items.Item(lItem).Text.Length
                If lMaxViewWidth < lWidth Then
                    lMaxViewWidth = lWidth
                End If
            Next


            DataGridView1.Columns(kGridColumnMappedTo).Width = lMaxViewWidth + (100)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub

    Private Sub frmMappings_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Dim g As System.Drawing.Graphics = Me.CreateGraphics()
        Dim listPen As New System.Collections.Generic.List(Of Pen)
        Dim listPoint As New System.Collections.Generic.List(Of Point())
        Try

            If Not m_vMappings Is Nothing Then
                For l As Integer = 0 To m_vMappings.GetUpperBound(1)
                    If CStr(m_vMappings(2, l)) = "Active" Then
                        Dim listIndex As Integer = m_vMappings(0, l)
                        Dim gridindex As Integer = m_vMappings(1, l)
                        Dim lY1 As Integer = CInt((CDbl(m_vMappings(0, l)) * ACGap) + VB6.TwipsToPixelsY(1150))

                        Dim lY2 As Integer = CInt((CDbl(m_vMappings(1, l)) * ACGap2) + VB6.TwipsToPixelsY(1150))
                        Dim pen As System.Drawing.Pen = New Pen(Color.Black)
                        pen.Width = 2
                        listPen.Add(pen)

                        'Dim point() As System.Drawing.Point = {New Point(lvwViewFields.Right, lvwViewFields.Items(listIndex).Position.Y + lvwViewFields.Top + 6), New Point(DataGridView1.Left, lY2 + 15)}
                        Dim point() As System.Drawing.Point = {New Point(lvwViewFields.Right, lvwViewFields.Items(listIndex).Position.Y + lvwViewFields.Top + 6), New Point(DataGridView1.Left, DataGridView1.Rows(gridindex).Height * gridindex + DataGridView1.Top + 30)}
                        listPoint.Add(point)

                    End If
                Next
                For index As Integer = 0 To listPen.Count - 1
                    g.DrawLines(listPen(index), listPoint(index))
                Next

            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub DataGridView1_CellBeginEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
        If String.IsNullOrEmpty(DataGridView1.CurrentRow().Cells(e.ColumnIndex).Value) Then
            Exit Sub
        End If
        Dim KeyCode As Integer = Strings.Asc(DataGridView1.CurrentRow().Cells(e.ColumnIndex).Value)
        Dim Shift As Integer = e.RowIndex \ &H10000

        Const kMethodName As String = "DataGridView1_CellBeginEdit"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bValid As Boolean

        Try

            bValid = True

            Select Case KeyCode
                ' if key is one of the special action keys
                Case Keys.Down, Keys.Up, Keys.Left, Keys.Right, Keys.Tab
                    If Not m_bValidating Then
                        ' prior to any action validate that the current
                        ' value is ok...
                        lReturn = ValidateTextEntry(e.RowIndex, e.ColumnIndex)
                        If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            bValid = False
                        End If

                        Application.DoEvents()
                    End If
            End Select

            If bValid = False Then
                e.Cancel = True

            Else
                ' select invalid value
                DataGridView1.CurrentRow().Cells(e.ColumnIndex).Selected = True

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub
End Class
