Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface 
	Inherits System.Windows.Forms.Form
	  Implements IDisposable
' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: {TodaysDate}
	'
	' Description: Main interface.
	'
	' ***************************************************************** '
	
    'developer guide no. 50
    Dim objfrmCopy As New frmCopy
    'developer guide no. 50
    Dim objfrmKeySel As New frmKeySel
    'developer guide no. 50
    Dim objfrmProgress As New frmProgress
    'developer guide no. 50
    Dim objfrmOptions As New frmOptions
    'developer guide no. 50
    Dim objfrmLog As New frmLog
	'PRIVATE CONSTANTS
	
	' Constant for the functions to identify
	' which class this is.
Private Const ACClass As String = "frmInterface" 
    Private Const vbFormCode As Integer = 0
	'Constants for Apply Method
	Private Const ACNoChange As Integer = 0
	Private Const ACChanged As Integer = 1
	Private Const ACApplied As Integer = 2
	Private Const ACApplyLocked As Integer = 3
	
	'Toolbar constants
	Private Const ACToolNewComponent As Integer = 4
	Private Const ACToolNewProcess As Integer = 5
	Private Const ACToolNewMap As Integer = 6
	Private Const ACToolNewStep As Integer = 7
	Private Const ACToolKeys As Integer = 8
	Private Const ACToolDelete As Integer = 9
	Private Const ACToolCopy As Integer = 10
	Private Const ACToolPrint As Integer = 11
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' Status members
	Private m_sProcessStatus As New FixedLengthString(2)
	Private m_sMapStatus As New FixedLengthString(2)
	Private m_sStepStatus As New FixedLengthString(2)
	
	Private m_iEventIndex As Integer
	Private m_sEventsCaption As String = ""
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	'System DSN list
	Private m_cSystemDSN As Collection
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast() As Control
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' Stores the details from the business object.
	Private m_vNavArray As Object
	Private m_vMediaArray As Object
	Private m_sDescription As String = ""
	
	' Slider bar data.
	Private m_bSliderMoved As Boolean
	Private m_lSliderPosition As Integer
	
	'Selected Group
	Private m_sPMNavGroup As String = ""
	
	'Node Keys
	Private m_lID As Integer
	Private m_lParentID As Integer
	
	'Interface Frame Control
	Private m_Interface As Control
	
	'Current ProcessMap
	Private m_cProcessMap As Collection
	
	'Current Group ID
	Private m_lGroupID As Integer
	
	'Apply Details Status
	Private m_iStatus As Integer
	'Add or Edit Mode
	Private m_iMode As Integer
	
	'Flag to lock the apply button
	Private m_bApplyLocked As Boolean
	
	'Flag to decide whether to restore value on lookup update
	Private m_bRestoreLookup As Boolean
	
	'The field details for the selected navitem
	Private m_cPMNavFields As Collection
	
	'The Standard Collection of all field objects
	Private m_cPMNavStepFields As Collection
	Private m_cPMNavProcessFields As Collection
	Private m_cPMNavMapFields As Collection
	Private m_cPMNavComponentFields As Collection
	
	'LookUp Arrays
	Private m_vLTabPMNav_Map As Object
	Private m_vLTabPMNav_SubMap As Object
	Private m_vLTabPMNav_StartMap As Object
	Private m_vLTabPMNav_Component As Object
	Private m_vLTabPMNav_Process As Object
	Private m_vLTabPMNav_Step As Object
	Private m_vLTabPMProduct As Object
	Private m_vLTabPMProc_Lock_Group As Object
	Private m_vLTabTransaction_Type As Object
	Private m_vLTasks As Object
	Private m_vLNavigateStatus As Object
	Private m_vLActions As Object
	Private m_vLProcessModes As Object
	Private m_vLComponentTypes As Object
	Private m_vLNavigatorModes As Object
	
	'Enforce rules for the current Navigator Item
	Private m_bEnforceRules As Boolean
	Private m_bAllowRuleEnforcement As Boolean
	
	'Stores Node type for use with PrintOut function
	Private m_NodeType As String = ""
	
	' ***************************************************************** '
	' Name: BuildGroup
	'
	' Description:  Builds the List of PMNavItem Objects
	'               Based on the Selected Group
	'
	' ***************************************************************** '
	Private Function BuildGroup(ByRef sPMNavGroup As String) As Integer
		
		Dim result As Integer = 0
		Dim vGroupList As Object
		Dim iLevel As Integer
		Dim sProcessMapEntry, sKey, sPrefixKey As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get the LookUp List for the Selected Group
			
			
			Select Case sPMNavGroup
				Case NavProcConst.NavGrpComponent


                    vGroupList = m_vLTabPMNav_Component
                    iLevel = NavProcConst.NodeComponent
                    sPrefixKey = NavProcConst.ACRootComponent

                Case NavProcConst.NavGrpProcess


                    vGroupList = m_vLTabPMNav_Process
                    iLevel = NavProcConst.NodeProcess
                    sPrefixKey = NavProcConst.ACRootProcess

                Case NavProcConst.NavGrpMap


                    vGroupList = m_vLTabPMNav_Map
                    iLevel = NavProcConst.NodeMap
                    sPrefixKey = NavProcConst.ACRootMap

            End Select

            m_cProcessMap = Nothing
            m_cProcessMap = New Collection()

            'Build the List of PMNavGroupItems

            For iPtr As Integer = vGroupList.GetLowerBound(1) To vGroupList.GetUpperBound(1)

                'Format the key for uniformity with True ProcessMap

                sKey = "0000000" & sPrefixKey & CStr(CInt(vGroupList(0, iPtr)))


                sProcessMapEntry = NavProcConst.ACRoot & "," & _
                                   sKey & "," & _
                                   iLevel & "," & _
                                   CStr(vGroupList(1, iPtr))

                'Add Process entry to Collection
                m_cProcessMap.Add(sProcessMapEntry, sKey)

            Next iPtr

            m_lReturn = BusinessToInterface(bExpanded:=False)

            Return result

        Catch



            Return result
        End Try
    End Function

    Private Sub ClearMainControls()

        If m_lGroupID = 0 Then

            cboGroups.SelectedIndex = -1
            cboGroupDescription.SelectedIndex = -1

            'Clear All Nodes
            treMainData.Nodes.Clear()

            'Clear current Node IDs
            m_lID = 0
            m_lParentID = 0

        End If

    End Sub

    ' ***************************************************************** '
    ' Name: Copy ( Private )
    '
    ' Description: Copy Processes or Maps
    '
    ' ***************************************************************** '
    Private Function Copy() As Integer

        Dim result As Integer = 0
        Dim lNewID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'developer guide no. 50
            m_lReturn = objfrmCopy.Initialise(m_cSystemDSN, m_oBusiness)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'developer guide no. 50
                objfrmCopy.Close()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start the Copy form
            'developer guide no. 50
            m_lReturn = objfrmCopy.Start(lNewID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'developer guide no. 50
                objfrmCopy.Close()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Restore the current lookup value after update
            m_bRestoreLookup = True

            'Update Processes
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Process)

            'Update All-Map and Start-Map LookUps
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Map)
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_StartMap)

            m_bRestoreLookup = False

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Copy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Copy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintOut ( Private )
    '
    ' Description: Prints to printer the displayed map/process/etc
    '
    ' ***************************************************************** '
    Private Function PrintOut() As Integer

        ' holds indentation count ' holds y coord of print space ' holds key
        Dim iParentIndent As Integer ' parent key pos
        Dim sDetails As String = "" ' frame details
        Dim dsx, dsy As Double
        Dim sType As String = "" 'screen type
        Dim nNode As TreeNode
        Dim lThisEnd, lMaxEnd As Integer ' max tree line width
        Dim dPY As Double
        Dim lLineHeight As Integer
        Dim dBX, dBY As Double
        Dim bOneLine As Boolean
        Dim iEndOfRealControls As Integer ' pointer to end of real controls
        Dim bFirstKey As Boolean
        Dim oDummy As iPMNavEditor.PMNavField


        Dim cGetKeys As New Collection
        Dim cSetkeys As New Collection

        Dim iNum As Integer = treMainData.Nodes.Count
        If iNum = 0 Then
            MessageBox.Show("Nothing to Print", Application.ProductName)
            Exit Function
        End If

        Dim bKeys As Boolean

        bKeys = MessageBox.Show("Print Get and Set Keys Also?", Application.ProductName, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes

        ' dimension arrays
        Dim iIndent(iNum - 1) As Integer
        Dim sKey(iNum - 1) As String
        Dim lPos(iNum - 1) As Single
        'set array values for root
        iIndent(0) = 0 : sKey(0) = treMainData.Nodes.Item(0).Name
        lPos(0) = 0

        'calculate indents
        For lPtr As Integer = 2 To iNum
            'locate parent indent
            For lIPtr As Integer = 1 To lPtr
                If sKey(lIPtr - 1) = treMainData.Nodes.Item(lPtr - 1).Parent.Name Then
                    iParentIndent = lIPtr
                End If
            Next lIPtr
            'store this one with incremented indent of parent
            iIndent(lPtr - 1) = iIndent(iParentIndent - 1) + 1
            sKey(lPtr - 1) = treMainData.Nodes.Item(lPtr - 1).Name
        Next lPtr
        'print header info
        PrinterHelper.Printer.Orientation = PrinterHelper.PrinterObjectConstants.vbPRDPVertical
        PrinterHelper.Printer.FontSize = 12
        PrinterHelper.Printer.FontUnderline = True
        PrinterHelper.Printer.Print(True, "TYPE:", cboGroups.Text, "/" & cboGroupDescription.Text)

        PrinterHelper.Printer.CurrentX = CInt((PrinterHelper.Printer.Width / 2) - PrinterHelper.Printer.TextWidth(m_oBusiness.DSN))

        PrinterHelper.Printer.Print(True, "DSN:", ReflectionHelper.GetMember(m_oBusiness, "DSN"))
        PrinterHelper.Printer.CurrentX = CInt((PrinterHelper.Printer.Width * 0.9) - PrinterHelper.Printer.TextWidth(DateTime.Now.ToString("dd/MM/yyyy HH:mm")))
        PrinterHelper.Printer.Print(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
        PrinterHelper.Printer.FontUnderline = False
        PrinterHelper.Printer.FontSize = 8
        PrinterHelper.Printer.Print()

        'keep current selected node
        Dim nKeep As TreeNode = treMainData.SelectedNode


        ' cal max width of tree
        For lPtr As Integer = 2 To iNum
            lThisEnd = CInt((PrinterHelper.Printer.TextWidth("W") * (iIndent(lPtr - 1) * 2)) + PrinterHelper.Printer.TextWidth("  " & sType & ":" & treMainData.Nodes.Item(lPtr - 1).Text))
            If lThisEnd > lMaxEnd Then lMaxEnd = lThisEnd
        Next lPtr

        'for each node
        For lPtr As Integer = 2 To iNum
            nNode = treMainData.Nodes.Item(lPtr - 1)
            'click through each node
            treMainData_AfterSelect(treMainData, New TreeViewEventArgs(nNode))
            'determin screen type
            sDetails = ""
            Select Case m_NodeType
                Case NavProcConst.NavGrpComponent
                    sType = "Component"
                Case NavProcConst.NavGrpProcess
                    sType = "Process"
                Case NavProcConst.NavGrpMap, NavProcConst.NavGrpRoadMap
                    sType = "Map"
                Case NavProcConst.NavGrpStep
                    sType = "Step"
                Case Else
            End Select

            'set print offset
            PrinterHelper.Printer.CurrentX = CInt(PrinterHelper.Printer.TextWidth("W") * (iIndent(lPtr - 1) * 2))
            ' get parent
            For lIPtr As Integer = 1 To lPtr
                If sKey(lIPtr - 1) = treMainData.Nodes.Item(lPtr - 1).Parent.Name Then
                    iParentIndent = lIPtr
                End If
            Next lIPtr
            dsx = PrinterHelper.Printer.CurrentX : dsy = PrinterHelper.Printer.CurrentY
            lLineHeight = CInt(PrinterHelper.Printer.TextHeight("X"))
            dPY = lPos(iParentIndent - 1)
            dPY -= ((PrinterHelper.Printer.Page - 1) * PrinterHelper.Printer.Height)
            'draw tree lines going back from this node
            If lPtr > 2 Then
                PrinterHelper.Printer.Line(New Point(PrinterHelper.Printer.TextWidth("W") * ((iIndent(lPtr - 1) - 1) * 2), PrinterHelper.Printer.CurrentY + (lLineHeight / 2)), New Point(PrinterHelper.Printer.CurrentX, PrinterHelper.Printer.CurrentY + (lLineHeight / 2)), False, False, False, False)
                PrinterHelper.Printer.CurrentX = CInt(dsx) : PrinterHelper.Printer.CurrentY = CInt(dsy)
                PrinterHelper.Printer.Line(New Point(PrinterHelper.Printer.TextWidth("W") * ((iIndent(lPtr - 1) - 1) * 2), dPY + (lLineHeight / 2)), New Point(PrinterHelper.Printer.TextWidth("W") * ((iIndent(lPtr - 1) - 1) * 2), PrinterHelper.Printer.CurrentY + (lLineHeight / 2)), False, False, False, False)
            End If
            PrinterHelper.Printer.CurrentX = CInt(dsx) : PrinterHelper.Printer.CurrentY = CInt(dsy)
            'draw blob on line elbows
            dBX = PrinterHelper.Printer.CurrentX
            dBY = PrinterHelper.Printer.CurrentY + (lLineHeight / 2)
            'developer guide no. 212
            PrinterHelper.Printer.Line(New Point(dBX - (lLineHeight / 3), dBY - (lLineHeight / 3)), New Point(dBX + (lLineHeight / 3), dBY + (lLineHeight / 3)), False, False, True, True)
            PrinterHelper.Printer.CurrentX = CInt(dsx) : PrinterHelper.Printer.CurrentY = CInt(dsy)
            lPos(lPtr - 1) = ((PrinterHelper.Printer.Page - 1) * PrinterHelper.Printer.Height) + PrinterHelper.Printer.CurrentY
            'print tree text
            PrinterHelper.Printer.Print(True, "  " & sType & ":" & treMainData.Nodes.Item(lPtr - 1).Text)
            PrinterHelper.Printer.CurrentX = CInt(lMaxEnd + PrinterHelper.Printer.TextWidth("XXXXXX"))
            bOneLine = True
            'build up frame details

            'add set and get keys to collection of controls so they are printed out
            iEndOfRealControls = m_cPMNavFields.Count
            If bKeys Then
                GetKeysForPrint(nNode)
                'developer guide no. 50
                objfrmKeySel.ReadGetSetKeys(cGetKeys, cSetkeys)
                bFirstKey = True
                For Each okey As Object In cGetKeys
                    oDummy = New iPMNavEditor.PMNavField()
                    If bFirstKey Then

                        oDummy.Name = "GETKEYS:" & okey.Description.Trim()
                        bFirstKey = False
                    Else

                        oDummy.Name = okey.Description.Trim()
                    End If

                    If okey.InitialKeyValue.Trim() <> "" Then

                        oDummy.Name = oDummy.Name & "(" & okey.InitialKeyValue.Trim() & ")"
                    End If
                    m_cPMNavFields.Add(oDummy)
                    oDummy = Nothing
                Next okey
                bFirstKey = True
                For Each okey As Object In cSetkeys
                    oDummy = New iPMNavEditor.PMNavField()
                    If bFirstKey Then

                        oDummy.Name = "SETKEYS:" & okey.Description.Trim()
                        bFirstKey = False
                    Else

                        oDummy.Name = okey.Description.Trim()
                    End If

                    If okey.InitialKeyValue.Trim() <> "" Then

                        oDummy.Name = oDummy.Name & "(" & okey.InitialKeyValue.Trim() & ")"
                    End If
                    m_cPMNavFields.Add(oDummy)
                    oDummy = Nothing
                Next okey
                cSetkeys = Nothing
                cGetKeys = Nothing
            End If
            oDummy = Nothing
            ' now loop through controls and print
            For iCptr As Integer = 1 To m_cPMNavFields.Count

                If m_cPMNavFields.Item(iCptr).Name <> "caption_id" Then

                    sDetails = m_cPMNavFields.Item(iCptr).Name
                    If iCptr <= iEndOfRealControls Then
                        sDetails = sDetails & "="

                        If TypeOf m_cPMNavFields.Item(iCptr).InterfaceControl Is CheckBox Then

                            Select Case m_cPMNavFields.Item(iCptr).InterfaceControl.Value
                                Case 2 : sDetails = sDetails & "?"
                                Case 1 : sDetails = sDetails & "True"
                                Case 0 : sDetails = sDetails & "False"
                            End Select
                        Else

                            sDetails = CStr(CDbl(sDetails) + m_cPMNavFields.Item(iCptr).InterfaceControl)
                        End If
                    End If
                    If iCptr < m_cPMNavFields.Count Then sDetails = sDetails & ", "
                    'new line?
                    If PrinterHelper.Printer.CurrentX + PrinterHelper.Printer.TextWidth(sDetails) > (PrinterHelper.Printer.Width * 0.9) Or (bOneLine And iCptr = m_cPMNavFields.Count) Then
                        PrinterHelper.Printer.Print()
                        bOneLine = False
                        'pagebreak and reverse join of lines
                        If PrinterHelper.Printer.CurrentY > (PrinterHelper.Printer.Height - (PrinterHelper.Printer.TextHeight("X") * 6)) Then
                            dsx = PrinterHelper.Printer.CurrentX : dsy = PrinterHelper.Printer.CurrentY
                            'for each node on following pages
                            For lBptr As Integer = lPtr + 1 To iNum
                                iParentIndent = -1
                                For lIPtr As Integer = 1 To lPtr
                                    If sKey(lIPtr - 1) = treMainData.Nodes.Item(lBptr - 1).Parent.Name Then
                                        iParentIndent = lIPtr
                                    End If
                                Next lIPtr
                                'locate parent
                                If iParentIndent >= 0 Then
                                    dPY = lPos(iParentIndent - 1)
                                    dPY -= ((PrinterHelper.Printer.Page - 1) * PrinterHelper.Printer.Height)
                                    If dPY < 0 Then
                                        dPY = 0
                                    End If
                                    'draw lines from following nodes up through this page
                                    PrinterHelper.Printer.Line(New Point(PrinterHelper.Printer.TextWidth("W") * ((iIndent(lBptr - 1) - 1) * 2), dPY + (lLineHeight / 2)), New Point(PrinterHelper.Printer.TextWidth("W") * ((iIndent(lBptr - 1) - 1) * 2), PrinterHelper.Printer.CurrentY + (lLineHeight / 2)), False, False, False, False)
                                End If
                                PrinterHelper.Printer.CurrentX = CInt(dsx) : PrinterHelper.Printer.CurrentY = CInt(dsy)
                            Next lBptr
                            'page number and page break
                            PrinterHelper.Printer.CurrentX = CInt((PrinterHelper.Printer.Width * 0.95) - VB6.PixelsToTwipsX(CreateGraphics().MeasureString("Page 99", Font).Width))
                            PrinterHelper.Printer.CurrentY = 0
                            PrinterHelper.Printer.Print("Page " & PrinterHelper.Printer.Page)
                            PrinterHelper.Printer.NewPage()
                        End If

                        PrinterHelper.Printer.CurrentX = CInt(lMaxEnd + PrinterHelper.Printer.TextWidth("XXXXXX"))
                    End If
                    PrinterHelper.Printer.Print(True, sDetails)
                End If
            Next iCptr
            PrinterHelper.Printer.Print()
            'more lines
            PrinterHelper.Printer.CurrentY = CInt(PrinterHelper.Printer.CurrentY + (PrinterHelper.Printer.TextHeight("X") / 2))
            PrinterHelper.Printer.Line(New Point(lMaxEnd + PrinterHelper.Printer.TextWidth("XXXXXX"), PrinterHelper.Printer.CurrentY), New Point(PrinterHelper.Printer.Width, PrinterHelper.Printer.CurrentY), False, False, False, False)
            PrinterHelper.Printer.CurrentY = CInt(PrinterHelper.Printer.CurrentY + (PrinterHelper.Printer.TextHeight("X") / 4))
            'get rid of dummy controls we added


            For iCptr As Integer = m_cPMNavFields.Count To iEndOfRealControls + 1 Step -1
                m_cPMNavFields.Remove(iCptr)
            Next iCptr
        Next lPtr
        'page number and enddoc
        PrinterHelper.Printer.CurrentX = CInt((PrinterHelper.Printer.Width * 0.95) - VB6.PixelsToTwipsX(CreateGraphics().MeasureString("Page 99", Font).Width))
        PrinterHelper.Printer.CurrentY = 0
        PrinterHelper.Printer.Print("Page " & PrinterHelper.Printer.Page)
        PrinterHelper.Printer.EndDoc()
        'click on original focus
        treMainData_AfterSelect(treMainData, New TreeViewEventArgs(nKeep))

    End Function


    ' ***************************************************************** '
    ' Name: DeleteItem ( Private )
    '
    ' Description: DeleteItem Processes or Maps
    '
    ' ***************************************************************** '
    Private Function DeleteItem() As Integer

        Dim result As Integer = 0
        Dim nNode As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            nNode = treMainData.SelectedNode

            If nNode Is Nothing Then
                Return result
            End If

            'Ignore root
            If nNode.Index < 2 Then
                Return result
            End If

            If Interaction.MsgBox("Are you absolutely cetain that you wish to delete " & _
                                  nNode.Text & "?", CStr(MsgBoxStyle.Question) & CStr(MsgBoxStyle.YesNo), "Delete Item") = System.Windows.Forms.DialogResult.No Then
                Return result
            End If

            'Delete it

            m_lReturn = m_oBusiness.DeleteItem(sPMNavGroup:=m_sPMNavGroup, lID:=m_lID, lParentID:=m_lParentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("This item could not be deleted " & Strings.Chr(13) & _
                                "due to existing dependencies.", "Delete Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            'Update lookups for current group
            m_lReturn = UpdateLookUps()

            'Remove the node
            treMainData.Nodes.RemoveAt(nNode.Index - 1)

            'Clear the fields
            ClearInterfaceFields()

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetPMNavFields(ByRef sPMNavGroup As String, ByRef lID As Integer, Optional ByRef lParentID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vFieldArray As Object

        Dim sFieldList As New StringBuilder
        Dim vFieldControl As Control

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Build the Field list
            For iPtr As Integer = 1 To m_cPMNavFields.Count

                sFieldList.Append(m_cPMNavFields.Item(iPtr).Name & ", ")
            Next iPtr

            ' Remove trailing characters
            sFieldList = New StringBuilder(sFieldList.ToString().Substring(0, sFieldList.ToString().Length - 2))


            m_lReturn = m_oBusiness.GetFields(sPMNavGroup:=sPMNavGroup, sFieldList:=sFieldList.ToString(), lID:=lID, r_vResultArray:=vFieldArray, lParentID:=lParentID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vFieldArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Lock the apply button
            m_bApplyLocked = True

            'Get the Field Values
            For iPtr As Integer = 1 To m_cPMNavFields.Count


                m_cPMNavFields.Item(iPtr).Value = vFieldArray(iPtr - 1, 0)

                'Get the Control

                vFieldControl = m_cPMNavFields.Item(iPtr).InterfaceControl

                'Set the value in the control

                'If this is a ComboLookUp or a TextLookUp then get lookup
                If TypeOf vFieldControl Is ComboBox Then

                    'If LookUp, then set lookup value

                    SetLookUPControl(vFieldControl, m_cPMNavFields.Item(iPtr).Value)

                ElseIf (Convert.ToString(ControlHelper.GetTag(vFieldControl)) <> "") Then

                    'If this is a lookup text box then get value

                    ControlHelper.SetTag(vFieldControl, m_cPMNavFields.Item(iPtr).Value)
                    If Convert.ToString(ControlHelper.GetTag(vFieldControl)) = "" Then
                        ControlHelper.SetTag(vFieldControl, "0")
                    End If

                    'If LookUp, then set lookup value

                    SetLookUPControl(vFieldControl, Conversion.Val(m_cPMNavFields.Item(iPtr).Value))
                Else

                    vFieldControl = m_cPMNavFields.Item(iPtr).Value
                End If

            Next iPtr

            'UnLock the apply button
            m_bApplyLocked = False

            'Reset Rules Enforcement
            m_bEnforceRules = False

            UpdateApply(ACNoChange)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMNavFields Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMNavFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProcessMapEntry
    '
    ' Description:  Gets individual entries in a process map
    '
    '
    ' ***************************************************************** '
    Public Function GetProcessMapEntry(ByVal sPMEntry As String, ByRef sParent As String, ByRef sID As String, ByRef iLevel As Integer, ByRef sDescription As String) As Integer

        Dim result As Integer = 0
        Dim iStartPos, iEndPos As Integer

        Try

            result = 1

            sParent = ""
            sID = ""
            iLevel = 0
            sDescription = ""

            'Get Parent
            iStartPos = 1
            iEndPos = Strings.InStr(iStartPos, sPMEntry, ",")
            If iEndPos = 0 Then
                Return result
            End If
            sParent = sPMEntry.Substring(iStartPos - 1, Math.Min(sPMEntry.Length, iEndPos - iStartPos))
            iStartPos = iEndPos + 1

            'Get ID
            iEndPos = Strings.InStr(iStartPos, sPMEntry, ",")
            If iEndPos = 0 Then
                Return result
            End If
            sID = Mid(sPMEntry, iStartPos, iEndPos - iStartPos)
            iStartPos = iEndPos + 1

            'Get Level
            iEndPos = Strings.InStr(iStartPos, sPMEntry, ",")
            If iEndPos = 0 Then
                Return result
            End If
            iLevel = Conversion.Val(Mid(sPMEntry, iStartPos, iEndPos - iStartPos))
            iStartPos = iEndPos + 1

            'Get Description
            iEndPos = sPMEntry.Length + 1
            sDescription = Mid(sPMEntry, iStartPos, iEndPos - iStartPos)
            iStartPos = iEndPos + 1

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessMapEntry Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessMapEntry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function






    ' ***************************************************************** '
    ' Name: BuildLookUp
    '
    ' Description: Builds the lookup details
    '
    ' ***************************************************************** '
    Private Function BuildLookUp(ByRef sLookupTable As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case sLookupTable
                'All Maps LookUp
                Case NavProcConst.ACLTabPMNav_Map
                    m_vLTabPMNav_Map = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMNav_Map, r_vResultArray:=m_vLTabPMNav_Map)

                    BuildLookUpControl(cboPMNav_Steppmnav_map_id, m_vLTabPMNav_Map)

                    'Start Maps LookUp
                Case NavProcConst.ACLTabPMNav_StartMap
                    m_vLTabPMNav_StartMap = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMNav_StartMap, r_vResultArray:=m_vLTabPMNav_StartMap, v_vWhereClause:="is_start_map = 1")

                    BuildLookUpControl(cboPMNav_Processstart_nav_map_id, m_vLTabPMNav_StartMap)

                    'Sub Maps LookUp
                Case NavProcConst.ACLTabPMNav_SubMap
                    m_vLTabPMNav_SubMap = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMNav_SubMap, r_vResultArray:=m_vLTabPMNav_SubMap, v_vWhereClause:="is_start_map = 0")

                    BuildLookUpControl(cboPMNav_Stepsub_nav_map_id, m_vLTabPMNav_SubMap)

                    'All Components
                Case NavProcConst.ACLTabPMNav_Component
                    m_vLTabPMNav_Component = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMNav_Component, r_vResultArray:=m_vLTabPMNav_Component)

                    BuildLookUpControl(cboPMNav_Steppmnav_component_id, m_vLTabPMNav_Component)

                    'All Processes
                Case NavProcConst.ACLTabPMNav_Process
                    m_vLTabPMNav_Process = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMNav_Process, r_vResultArray:=m_vLTabPMNav_Process)

                    BuildLookUpControl(cboPMNav_Stepok_nav_process_id, m_vLTabPMNav_Process)
                    BuildLookUpControl(cboPMNav_Stepcancel_nav_process_id, m_vLTabPMNav_Process)

                    'All Steps
                Case NavProcConst.ACLTabPMNav_Step
                    m_vLTabPMNav_Step = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMNav_Step, r_vResultArray:=m_vLTabPMNav_Step)

                    'All Products
                Case NavProcConst.ACLTabPMProduct
                    m_vLTabPMProduct = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMProduct, r_vResultArray:=m_vLTabPMProduct)

                    BuildLookUpControl(cboPMNav_Processpmproduct_id, m_vLTabPMProduct)

                    'Lock Groups
                Case NavProcConst.ACLTabPMProc_Lock_Group
                    m_vLTabPMProc_Lock_Group = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabPMProc_Lock_Group, r_vResultArray:=m_vLTabPMProc_Lock_Group)

                    BuildLookUpControl(cboPMNav_Processpmproc_lock_group_id, m_vLTabPMProc_Lock_Group)

                    'Transaction types
                Case NavProcConst.ACLTabTransaction_Type
                    m_vLTabTransaction_Type = Nothing

                    m_lReturn = m_oBusiness.GetLookUpTable(v_iTableLookUp:=NavProcConst.ACLTabTransaction_Type, r_vResultArray:=m_vLTabTransaction_Type)

                    BuildLookUpControl(cboPMNav_Processtransaction_type_id, m_vLTabTransaction_Type)

                    'Tasks
                Case NavProcConst.ACLTasks
                    ReDim m_vLTasks(1, 3)

                    m_vLTasks(0, 0) = gPMConstants.PMEComponentAction.PMAdd

                    m_vLTasks(1, 0) = "PMAdd"

                    m_vLTasks(0, 1) = gPMConstants.PMEComponentAction.PMEdit

                    m_vLTasks(1, 1) = "PMEdit"

                    m_vLTasks(0, 2) = gPMConstants.PMEComponentAction.PMDelete

                    m_vLTasks(1, 2) = "PMDelete"

                    m_vLTasks(0, 3) = gPMConstants.PMEComponentAction.PMView

                    m_vLTasks(1, 3) = "PMView"

                    BuildLookUpControl(cboPMNav_Steptask, m_vLTasks)

                    'Navigate Status
                Case NavProcConst.ACLNavigateStatus
                    ReDim m_vLNavigateStatus(1, 2)

                    m_vLNavigateStatus(0, 0) = "00"

                    m_vLNavigateStatus(1, 0) = "00 Nav Button Invisible"

                    m_vLNavigateStatus(0, 1) = "01"

                    m_vLNavigateStatus(1, 1) = "01 Nav Button Enabled"

                    m_vLNavigateStatus(0, 2) = "02"

                    m_vLNavigateStatus(1, 2) = "02 Nav Button Disbaled"

                    BuildLookUpControl(cboPMNav_Stepnavigate_status, m_vLNavigateStatus)

                    'Actions
                Case NavProcConst.ACLActions
                    ReDim m_vLActions(1, 8)

                    m_vLActions(0, 0) = gPMConstants.PMNavActionStartProcess

                    m_vLActions(1, 0) = "StartProcess"

                    m_vLActions(0, 1) = gPMConstants.PMNavActionRepeatMap

                    m_vLActions(1, 1) = "RepeatMap"

                    m_vLActions(0, 2) = gPMConstants.PMNavActionForwardX

                    m_vLActions(1, 2) = "ForwardX"

                    m_vLActions(0, 3) = gPMConstants.PMNavActionForwardOne

                    m_vLActions(1, 3) = "ForwardOne"

                    m_vLActions(0, 4) = gPMConstants.PMNavActionExitMap

                    m_vLActions(1, 4) = "ExitMap"

                    m_vLActions(0, 5) = gPMConstants.PMNavActionCompleteProcess

                    m_vLActions(1, 5) = "CompleteProcess"

                    m_vLActions(0, 6) = gPMConstants.PMNavActionAbortProcess

                    m_vLActions(1, 6) = "AbortProcess"

                    m_vLActions(0, 7) = gPMConstants.PMNavActionBackX '(in gPMLibraries BUT NOT SUPPORTED)

                    m_vLActions(1, 7) = "BackX"

                    m_vLActions(0, 8) = gPMConstants.PMNavActionBackOne '(in gPMLibraries BUT NOT SUPPORTED)

                    m_vLActions(1, 8) = "BackOne"

                    BuildLookUpControl(cboPMNav_Stepok_action, m_vLActions)
                    BuildLookUpControl(cboPMNav_Stepcancel_action, m_vLActions)

                    'Process modes
                Case NavProcConst.ACLProcessModes
                    ReDim m_vLProcessModes(1, 16)


                    m_vLProcessModes(0, 0) = gPMConstants.PMEProcessMode.PMProcessModeEnquiry

                    m_vLProcessModes(1, 0) = "Enquiry"

                    m_vLProcessModes(0, 1) = gPMConstants.PMEProcessMode.PMProcessModeGeneric

                    m_vLProcessModes(1, 1) = "Generic"

                    m_vLProcessModes(0, 2) = gPMConstants.PMEProcessMode.PMProcessModeMTALive

                    m_vLProcessModes(1, 2) = "MTALive"

                    m_vLProcessModes(0, 3) = gPMConstants.PMEProcessMode.PMProcessModeMTAQuote

                    m_vLProcessModes(1, 3) = "MTAQuote"

                    m_vLProcessModes(0, 4) = gPMConstants.PMEProcessMode.PMProcessModeNBLive

                    m_vLProcessModes(1, 4) = "NBLive"

                    m_vLProcessModes(0, 5) = gPMConstants.PMEProcessMode.PMProcessModeNBQuote

                    m_vLProcessModes(1, 5) = "NBQuote"

                    m_vLProcessModes(0, 6) = gPMConstants.PMEProcessMode.PMProcessModeRNLive

                    m_vLProcessModes(1, 6) = "RNLive"

                    m_vLProcessModes(0, 7) = gPMConstants.PMEProcessMode.PMProcessModeRNQuote

                    m_vLProcessModes(1, 7) = "RNQuote"

                    'Gemini constants

                    m_vLProcessModes(0, 8) = gPMConstants.PMEProcessMode.PMProcessModeFull

                    m_vLProcessModes(1, 8) = "GEMFull"

                    m_vLProcessModes(0, 9) = gPMConstants.PMEProcessMode.PMProcessModePostQuote

                    m_vLProcessModes(1, 9) = "GEMPostQuote"

                    m_vLProcessModes(0, 10) = gPMConstants.PMEProcessMode.PMProcessModeSpecific

                    m_vLProcessModes(1, 10) = "GEMSpecific"

                    m_vLProcessModes(0, 11) = gPMConstants.PMEProcessMode.PMProcessModeStartAtQuote

                    m_vLProcessModes(1, 11) = "GEMStartAtQuote"

                    m_vLProcessModes(0, 12) = gPMConstants.PMEProcessMode.PMProcessModeDefault

                    m_vLProcessModes(1, 12) = "GEMDefault"

                    m_vLProcessModes(0, 13) = gPMConstants.PMEProcessMode.PMProcessModeReview

                    m_vLProcessModes(1, 13) = "GEMReview"

                    m_vLProcessModes(0, 14) = gPMConstants.PMEProcessMode.PMProcessModeCancellations

                    m_vLProcessModes(1, 14) = "GEMCancellations"

                    m_vLProcessModes(0, 15) = gPMConstants.PMEProcessMode.PMProcessModeClaims

                    m_vLProcessModes(1, 15) = "GEMClaims"

                    m_vLProcessModes(0, 16) = gPMConstants.PMEProcessMode.PMProcessModeOverride

                    m_vLProcessModes(1, 16) = "GEMOverride"

                    BuildLookUpControl(cboPMNav_Processprocess_mode, m_vLProcessModes)

                    'Component Types
                Case NavProcConst.ACLComponentTypes
                    ReDim m_vLComponentTypes(1, 3)


                    m_vLComponentTypes(0, 0) = gPMConstants.PMNavComponentFindForm

                    m_vLComponentTypes(1, 0) = "FindForm"

                    m_vLComponentTypes(0, 1) = gPMConstants.PMNavComponentDecisionForm

                    m_vLComponentTypes(1, 1) = "DecisionForm"

                    m_vLComponentTypes(0, 2) = gPMConstants.PMNavComponentDataForm

                    m_vLComponentTypes(1, 2) = "DataForm"

                    m_vLComponentTypes(0, 3) = gPMConstants.PMNavComponentBusinessObject

                    m_vLComponentTypes(1, 3) = "BusinessObject"

                    BuildLookUpControl(cboPMNav_Componentnav_component_type, m_vLComponentTypes)

                    'Navigator Modes
                Case NavProcConst.ACLNavigatorModes
                    ReDim m_vLNavigatorModes(1, 3)


                    m_vLNavigatorModes(0, 0) = 0

                    m_vLNavigatorModes(1, 0) = "Navigator Driven"

                    m_vLNavigatorModes(0, 1) = 1

                    m_vLNavigatorModes(1, 1) = "User Driven"

                    m_vLNavigatorModes(0, 2) = 2

                    m_vLNavigatorModes(1, 2) = "Navigable"

                    m_vLNavigatorModes(0, 3) = 3

                    m_vLNavigatorModes(1, 3) = "Data Capture"

                    BuildLookUpControl(cboPMNav_Processis_user_driven, m_vLNavigatorModes)

                Case Else

            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildLookUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: BuildLookUpControl
    '
    ' Description: Builds a lookup control
    '
    ' ***************************************************************** '
    Private Sub BuildLookUpControl(ByRef vControl As Control, ByRef vLookUpArray As Object)

        Try

            'Get the List index of the control

            'TODo check at run time 
            'lListIndex = vControl.ListIndex
            'TODo check at run time
            'Clear LookUp Control

            'vControl.Clear()

            If Not Information.IsArray(vLookUpArray) Then
                Exit Sub
            End If

            'Populate the List

            For i As Integer = vLookUpArray.GetLowerBound(1) To vLookUpArray.GetUpperBound(1)
                'developer guide no. 50
                objfrmProgress.Increment() 'Increment the progress bar

                'TODo check at run time
                'vControl.AddItem(vLookUpArray(1, i))
            Next i

            'Restore the list index
            If m_bRestoreLookup Then
                m_bApplyLocked = True

                'TODO check at run time
                'vControl.ListIndex = lListIndex
                m_bApplyLocked = False
            End If

        Catch
        End Try




    End Sub


    ' ***************************************************************** '
    ' Name: ClearInterfaceFields
    '
    ' Description:  Clears all Displayed InterfaceFields
    '               Based on the Selected Group
    '
    ' ***************************************************************** '
    Private Sub ClearInterfaceFields()

        Dim vInterfaceControl As Control
        Dim sValidation As String = ""

        Try

            'Lock the Apply button
            m_bApplyLocked = True

            For iPtr As Integer = m_cPMNavFields.Count To 1 Step -1


                sValidation = m_cPMNavFields.Item(iPtr).Validation

                'Get the interface control

                vInterfaceControl = m_cPMNavFields.Item(iPtr).InterfaceControl

                'If list lookup then set to null
                If TypeOf vInterfaceControl Is ComboBox Then
                    CType(vInterfaceControl, ComboBox).SelectedIndex = -1

                    'If checkbox then false
                ElseIf TypeOf vInterfaceControl Is CheckBox Then
                    CType(vInterfaceControl, CheckBox).Checked = 0

                    'If text look up then clear text and tag
                ElseIf StringsHelper.ToDoubleSafe(sValidation) = NavProcConst.ACTextLookUp Then

                    vInterfaceControl.Name = ""
                    ControlHelper.SetTag(vInterfaceControl, "0")

                    'If date field then place today's date
                ElseIf vInterfaceControl.Name.EndsWith("date") Then
                    'Modified by Vijay Pal on 5/24/2010 10:36:13 AM replaced vInterfaceControl = DateTime.Today.ToString("dd/MM/yy") by CType(vInterfaceControl, TextBox).Text = DateTime.Today.ToString("dd/MM/yy")
                    CType(vInterfaceControl, TextBox).Text = DateTime.Today.ToString("dd/MM/yy")

                    'Otherwise empty string
                Else
                    vInterfaceControl = Nothing

                End If

            Next iPtr

            vInterfaceControl.Focus()

            'UnLock the apply button
            m_bApplyLocked = False

        Catch
        End Try



    End Sub
    ' ***************************************************************** '
    ' Name: DisplayGroupDescription
    '
    ' Description:  Displays a SubGroupList in GroupDescription Combo
    '               Based on the Group Selected
    '
    ' ***************************************************************** '
    Private Function DisplayGroupDescription(ByRef sPMNavGroup As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboGroupDescription.SelectedIndex = -1
            cboGroupDescription.Items.Clear()


            Select Case sPMNavGroup
                'Build all Processes
                Case NavProcConst.NavGrpProcess
                    cboGroupDescription.Enabled = True
                    BuildLookUpControl(cboGroupDescription, m_vLTabPMNav_Process)

                    'Build All Maps
                Case NavProcConst.NavGrpMap
                    cboGroupDescription.Enabled = True
                    BuildLookUpControl(cboGroupDescription, m_vLTabPMNav_Map)

                    'Build All Components
                Case NavProcConst.NavGrpComponent
                    cboGroupDescription.Enabled = True
                    BuildLookUpControl(cboGroupDescription, m_vLTabPMNav_Component)

                Case Else
                    cboGroupDescription.Enabled = False
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute DisplayGroupDescription", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayGroupDescription", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayInterface
    '
    ' Description:  Displays the Correct Interface
    '               Based on the Group Selected
    '
    ' ***************************************************************** '
    Private Function DisplayInterface(ByRef sInterfaceName As String) As Integer

        Dim result As Integer = 0
        Dim iLeft, iWidth, iHeight As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Make sure we are in the correct group
            If m_sPMNavGroup <> sInterfaceName Then
                m_sPMNavGroup = sInterfaceName
            End If

            'Hide current interface frame

            iLeft = VB6.PixelsToTwipsX(m_Interface.Left)
            iWidth = VB6.PixelsToTwipsX(m_Interface.Width)
            iHeight = VB6.PixelsToTwipsY(m_Interface.Height)
            ControlHelper.SetVisible(m_Interface, False)

            m_NodeType = sInterfaceName

            'Set the PMNav Interface
            'Set the PMNav Field collection
            'Set the PMNav Table

            Select Case sInterfaceName
                Case NavProcConst.NavGrpComponent
                    m_Interface = fraComponent
                    m_cPMNavFields = m_cPMNavComponentFields

                Case NavProcConst.NavGrpProcess
                    m_Interface = fraProcess
                    m_cPMNavFields = m_cPMNavProcessFields

                Case NavProcConst.NavGrpMap
                    m_Interface = fraMap
                    m_cPMNavFields = m_cPMNavMapFields

                    'If this is edit then lock the IsStartMap field
                    If m_iMode = gPMConstants.PMEComponentAction.PMEdit Then
                        chkPMNav_Mapis_start_map.Enabled = True 'Dont lock
                    Else
                        chkPMNav_Mapis_start_map.Enabled = True
                    End If

                Case NavProcConst.NavGrpStep
                    m_Interface = fraStep
                    m_cPMNavFields = m_cPMNavStepFields

                Case NavProcConst.NavGrpRoadMap
                    m_Interface = fraBlank
                    'Set m_cPMNavFields = m_cPMNavComponentFields

                Case Else

            End Select

            'Show selected group interface frame
            m_Interface.Left = VB6.TwipsToPixelsX(iLeft)
            m_Interface.Width = VB6.TwipsToPixelsX(iWidth)
            m_Interface.Height = VB6.TwipsToPixelsY(iHeight)
            ControlHelper.SetVisible(m_Interface, True)
            m_Interface.SendToBack()

            'Clear Out Current Fields
            ClearInterfaceFields()

            'Reset Status
            UpdateApply(ACNoChange)

            Me.Refresh()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute DisplayInterface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property



    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property



    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetLookUpControl (see SetLookUPControl)
    '
    ' Description:  Gets LookUp Control Key Via The LookUp Array
    '               Based on the Selected Group
    '
    ' ***************************************************************** '
    Private Function GetLookUpControl(ByRef vLookUpControl As Control) As Object

        Dim result As Object = Nothing
        Dim vLookUpArray As Object

        Try

            result = 0


            'Get the lookup array for the control

            Select Case vLookUpControl.Name
                Case "cboPMNav_Steppmnav_map_id"


                    vLookUpArray = m_vLTabPMNav_Map

                Case "cboPMNav_Steppmnav_component_id"


                    vLookUpArray = m_vLTabPMNav_Component

                Case "cboPMNav_Stepsub_nav_map_id"


                    vLookUpArray = m_vLTabPMNav_SubMap

                Case "cboPMNav_Stepok_nav_process_id", "cboPMNav_Stepcancel_nav_process_id"


                    vLookUpArray = m_vLTabPMNav_Process

                Case "cboPMNav_Steptask"


                    vLookUpArray = m_vLTasks

                Case "cboPMNav_Stepnavigate_status"


                    vLookUpArray = m_vLNavigateStatus

                Case "cboPMNav_Stepok_action", "cboPMNav_Stepcancel_action"


                    vLookUpArray = m_vLActions

                Case "cboPMNav_Processpmproduct_id"


                    vLookUpArray = m_vLTabPMProduct

                Case "cboPMNav_Processpmproc_lock_group_id"


                    vLookUpArray = m_vLTabPMProc_Lock_Group

                Case "cboPMNav_Processtransaction_type_id"


                    vLookUpArray = m_vLTabTransaction_Type

                Case "cboPMNav_Processstart_nav_map_id"


                    vLookUpArray = m_vLTabPMNav_StartMap

                Case "cboPMNav_Processprocess_mode"


                    vLookUpArray = m_vLProcessModes

                Case "cboPMNav_Componentnav_component_type"


                    vLookUpArray = m_vLComponentTypes

                Case "cboPMNav_Processis_user_driven"


                    vLookUpArray = m_vLNavigatorModes

                Case "cboGroupDescription"

                    Select Case (cboGroups.Text)
                        Case NavProcConst.NavGrpProcess


                            vLookUpArray = m_vLTabPMNav_Process

                        Case NavProcConst.NavGrpMap


                            vLookUpArray = m_vLTabPMNav_Map

                        Case NavProcConst.NavGrpComponent


                            vLookUpArray = m_vLTabPMNav_Component
                    End Select

            End Select

            'If control not found then exit
            If Not Information.IsArray(vLookUpArray) Then
                Return result
            End If

            'Get the ID from the LookUp array using ListIndex from Control

            'TODO check at run time
            'Return vLookUpArray(0, vLookUpControl.ListIndex)

        Catch


            Return ""
        End Try

    End Function

    Private Sub GetKeysForPrint(ByRef nNode As TreeNode)

        Dim sDescription As String = ""

        Try

            If nNode Is Nothing Then
                sDescription = ""
            Else
                sDescription = nNode.Text
            End If

            'If we are in Process View then Set the Current Process
            'in the Business Object

            If cboGroups.Text = NavProcConst.NavGrpProcess Then

                m_oBusiness.CurrentProcessID = m_lGroupID
            End If

            'no need to reinvent the wheel here!
            'use initialise code from frmkeysel form
            'Initialise the Key Selector form
            'developer guide no. 50
            m_lReturn = objfrmKeySel.Initialise(m_oBusiness, m_sPMNavGroup, m_lID, sDescription, CStr(m_lParentID), m_bAllowRuleEnforcement)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Unload Keys form
            'developer guide no. 50
            objfrmKeySel.Close()

        Catch
        End Try






    End Sub


    ' ***************************************************************** '
    ' Name: GetSetKeys (Private)
    '
    '
    ' Description: Display the PMNavKey Editor
    '
    '
    ' ***************************************************************** '
    Private Sub GetSetKeys()

        Dim sDescription As String = ""
        Dim nNode As TreeNode

        Try

            nNode = treMainData.SelectedNode

            If nNode Is Nothing Then
                sDescription = ""
            Else
                sDescription = nNode.Text
            End If

            'If we are in Process View then Set the Current Process
            'in the Business Object

            If cboGroups.Text = NavProcConst.NavGrpProcess Then

                m_oBusiness.CurrentProcessID = m_lGroupID
            End If

            'Initialise the Key Selector form
            'developer guide no. 50
            m_lReturn = objfrmKeySel.Initialise(m_oBusiness, m_sPMNavGroup, m_lID, sDescription, CStr(m_lParentID), m_bAllowRuleEnforcement)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Show Keys form
            'developer guide no. 50
            objfrmKeySel.ShowDialog()

            'Unload Keys form
            'developer guide no. 50
            objfrmKeySel.Close()

        Catch
        End Try




    End Sub

    Public Function Initialise() As Integer



        Dim result As Integer = 0
        Dim sDSN As String

        ' Forms initialise event.

        Try

            ' Initialise the error number value.
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the main business object
            m_oBusiness = New bPMNavEditor.Form()

            'Select the DSN From the business object

            m_cSystemDSN = m_oBusiness.LoadSystemDSN()

            'Initialise the options
            'developer guide no. 50
            m_lReturn = objfrmOptions.Initialise(m_cSystemDSN, sDSN)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.DSN = sDSN

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Initialise the business object

            With g_oObjectManager



                m_lReturn = CType(m_oBusiness, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=.UserName, sPassword:=.Password, iUserID:=.UserID, iSourceID:=.SourceID, iLanguageID:=.LanguageID, iCurrencyID:=.CurrencyID, iLogLevel:=.LogLevel, sCallingAppName:=My.Application.Info.AssemblyName)

            End With

            ' Get an instance of the main business object via
            ' the public object manager.
            'm_lReturn& = g_oObjectManager.GetInstance( _
            'oObject:=m_oBusiness, _
            'sClassName:="bPMNavEditor.Form", _
            'vInstanceManager:="ClientManager")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Verify the Navigator Tables
            m_lReturn = VerifyNavigatorTables()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceConditions
    '
    ' Description: Sets Conditions peculiar to each individual Interface
    '
    ' ***************************************************************** '
    Private Sub InterfaceConditions(ByRef sPMNavGroup As String, ByRef vSubValue As Integer)

        Dim vLookUpValue As Object

        Try


            Select Case sPMNavGroup
                'Step Interface
                Case NavProcConst.NavGrpStep


                    'Display the LookUp values

                    Select Case vSubValue
                        Case 0
                            'Get the lookup value for the OkAction


                            vLookUpValue = GetLookUpControl(cboPMNav_Stepok_action)

                            'Display Process field if this is Process Action

                            If CStr(vLookUpValue) = gPMConstants.PMNavActionStartProcess Then
                                txtPMNav_Stepok_no_of_steps.Visible = False
                                cboPMNav_Stepok_nav_process_id.Visible = True

                                'Display Step Field if this is Step action
                            ElseIf (CStr(vLookUpValue) = gPMConstants.PMNavActionForwardX Or CStr(vLookUpValue) = gPMConstants.PMNavActionBackX) Then
                                txtPMNav_Stepok_no_of_steps.Visible = True
                                cboPMNav_Stepok_nav_process_id.Visible = False

                                'Hide fields otherwise
                            Else
                                txtPMNav_Stepok_no_of_steps.Visible = False
                                cboPMNav_Stepok_nav_process_id.Visible = False
                            End If

                            txtPMNav_Stepok_no_of_steps.Text = ""
                            cboPMNav_Stepok_nav_process_id.SelectedIndex = -1

                        Case 1
                            'Get the lookup value for the CancelAction


                            vLookUpValue = GetLookUpControl(cboPMNav_Stepcancel_action)

                            'Display Process field if this is Process Action

                            If CStr(vLookUpValue) = gPMConstants.PMNavActionStartProcess Then
                                txtPMNav_Stepcancel_no_of_steps.Visible = False
                                cboPMNav_Stepcancel_nav_process_id.Visible = True

                                'Display Step Field if this is Step action
                            ElseIf (CStr(vLookUpValue) = gPMConstants.PMNavActionForwardX Or CStr(vLookUpValue) = gPMConstants.PMNavActionBackX) Then
                                txtPMNav_Stepcancel_no_of_steps.Visible = True
                                cboPMNav_Stepcancel_nav_process_id.Visible = False

                                'Hide fields otherwise
                            Else
                                txtPMNav_Stepcancel_no_of_steps.Visible = False
                                cboPMNav_Stepcancel_nav_process_id.Visible = False
                            End If

                            txtPMNav_Stepcancel_no_of_steps.Text = ""
                            cboPMNav_Stepcancel_nav_process_id.SelectedIndex = -1

                        Case 2

                            'Rule: A Sub Map step cannot be hidden.

                            If cboPMNav_Stepsub_nav_map_id.Text <> "" Then
                                chkPMNav_Stepis_hidden.CheckState = CheckState.Unchecked
                            End If

                        Case 3

                            'Rule: A Hidden Step cannot return PMNavigate.
                            If chkPMNav_Stepis_hidden.CheckState = CheckState.Checked Then
                                cboPMNav_Stepok_action.SelectedIndex = -1
                                cboPMNav_Stepcancel_action.SelectedIndex = -1
                            End If

                    End Select

                    'Component Interface
                Case NavProcConst.NavGrpComponent


                    Select Case vSubValue
                        Case 0

                            'If this is a server side component, then it must be a
                            'business object
                            If chkPMNav_Componentis_server_side.CheckState <> CheckState.Unchecked Then
                                SetLookUPControl(cboPMNav_Componentnav_component_type, gPMConstants.PMNavComponentBusinessObject)
                            End If

                    End Select


            End Select

        Catch
        End Try




    End Sub

    ' ***************************************************************** '
    ' Name: PMNavNew (Private)
    '
    ' Description: Create a new PMNavGroup item
    '
    ' ***************************************************************** '
    Private Sub PMNavNew(ByRef sPMNavGroup As String)

        Try

            'If this is a step, then we are inserting above it
            If sPMNavGroup = NavProcConst.NavGrpStep Then

                m_oBusiness.InsertStepIDs(m_lParentID, m_lID)
            End If

            ClearMainControls()
            DisplayInterface(sPMNavGroup)
            m_iMode = gPMConstants.PMEComponentAction.PMAdd
            UpdateApply(ACNoChange)

            'Set the Step Defaults for a new step
            If sPMNavGroup = NavProcConst.NavGrpStep Then
                SetStepDefaults()
            End If

        Catch
        End Try



    End Sub

    ' ***************************************************************** '
    ' Name: SetLookUPControl (see GetLookUpControl)
    '
    ' Description:  Set LookUp Control Via The LookUp Array
    '               Based on the Selected Group
    '
    ' ***************************************************************** '
    Private Sub SetLookUPControl(ByRef vLookUpControl As Control, ByRef vValue As Object)

        Dim vLookUpArray As Object


        Try

            'TODO check at run time
            'vLookUpControl.ListIndex = -1
            Try


                'Get the lookup array for the control

                Select Case vLookUpControl.Name
                    Case "cboGroupDescription"

                        Select Case (cboGroups.Text)
                            Case NavProcConst.NavGrpProcess


                                vLookUpArray = m_vLTabPMNav_Process

                            Case NavProcConst.NavGrpMap


                                vLookUpArray = m_vLTabPMNav_Map

                            Case NavProcConst.NavGrpComponent


                                vLookUpArray = m_vLTabPMNav_Component
                        End Select

                    Case "cboPMNav_Steppmnav_map_id"


                        vLookUpArray = m_vLTabPMNav_Map

                    Case "cboPMNav_Steppmnav_component_id"


                        vLookUpArray = m_vLTabPMNav_Component

                    Case "cboPMNav_Stepsub_nav_map_id"


                        vLookUpArray = m_vLTabPMNav_SubMap

                    Case "cboPMNav_Stepok_nav_process_id", "cboPMNav_Stepcancel_nav_process_id"


                        vLookUpArray = m_vLTabPMNav_Process

                    Case "cboPMNav_Steptask"


                        vLookUpArray = m_vLTasks

                    Case "cboPMNav_Stepnavigate_status"


                        vLookUpArray = m_vLNavigateStatus

                    Case "cboPMNav_Stepok_action", "cboPMNav_Stepcancel_action"


                        vLookUpArray = m_vLActions

                    Case "cboPMNav_Processpmproduct_id"


                        vLookUpArray = m_vLTabPMProduct

                    Case "cboPMNav_Processpmproc_lock_group_id"


                        vLookUpArray = m_vLTabPMProc_Lock_Group

                    Case "cboPMNav_Processtransaction_type_id"


                        vLookUpArray = m_vLTabTransaction_Type

                    Case "cboPMNav_Processstart_nav_map_id"


                        vLookUpArray = m_vLTabPMNav_StartMap

                    Case "cboPMNav_Processprocess_mode"


                        vLookUpArray = m_vLProcessModes

                    Case "cboPMNav_Componentnav_component_type"


                        vLookUpArray = m_vLComponentTypes

                    Case "cboPMNav_Processis_user_driven"


                        vLookUpArray = m_vLNavigatorModes

                        'Get the LookUp Caption
                    Case "txtPMNav_Processcaption_id", "txtPMNav_Componentcaption_id", "txtPMNav_Mapcaption_id", "txtPMNav_Stepcaption_id"

                        'Get the lookup value straight from the table


                        vLookUpControl = m_oBusiness.GetLookUpValue(sTableName:="PMCaption", lID:=Conversion.Val(CStr(vValue)))

                        Exit Sub

                    Case Else

                End Select

                'If control not found then exit
                If Not Information.IsArray(vLookUpArray) Then
                    Exit Sub
                End If

                'Use LookUp Array to match key, and set description

                For i As Integer = vLookUpArray.GetLowerBound(1) To vLookUpArray.GetUpperBound(1)


                    If CStr(vValue) = CStr(vLookUpArray(0, i)) Then

                        'TODO check at run time
                        'vLookUpControl.ListIndex = i
                        Exit For
                    Else

                        'TODO check at run time
                        'vLookUpControl.ListIndex = -1
                    End If
                Next i

                Exit Sub

            Catch
            End Try

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


    End Sub


    Private Sub SetStepDefaults()

        Dim nNode As TreeNode
        Dim sNodeCode, sKeyIDs As String
        Static lNodeID As Integer

        Try

            'Get the currently selected node
            nNode = treMainData.SelectedNode

            If nNode Is Nothing Then
                Exit Sub
            End If

            'Ignore root
            If nNode.Index < 2 Then
                Exit Sub
            End If

            'Get the code eg: 0000000RM1011
            sNodeCode = Mid(nNode.Name, 9, 1)

            'Get the ID
            lNodeID = CInt(Conversion.Val(Mid(nNode.Name, 10, 99)))

            'Check if the node is a map
            If sNodeCode = "M" Then

                'If map then set map on step
                SetLookUPControl(cboPMNav_Steppmnav_map_id, lNodeID)

                'Check if the node is a component
            ElseIf (sNodeCode = "C") Then

                'If component then set component on step
                SetLookUPControl(cboPMNav_Steppmnav_component_id, lNodeID)

                'Check if the node is a step
            ElseIf (sNodeCode = "S") Then

                'If step then get mapID for this step (see treMainData.NodeClick)
                sKeyIDs = Mid(nNode.Name, 10, 99)
                lNodeID = CInt(Conversion.Val(SubStr(sKeyIDs, 2, ";")))

                'Set Map on Step
                SetLookUPControl(cboPMNav_Steppmnav_map_id, lNodeID)

            End If

        Catch
        End Try




    End Sub

    ' ***************************************************************** '
    ' Name: SetUpAllFields
    '
    ' Description: Sets up data for all fields
    '
    ' ***************************************************************** '
    Private Function SetUpAllFields() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set Up PMNav_Step Fields

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="pmnav_map_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=cboPMNav_Steppmnav_map_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="pmnav_component_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Steppmnav_component_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="sub_nav_map_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Stepsub_nav_map_id)


            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="task", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Steptask)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="navigate_status", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=False, iLength:=2, InterfaceControl:=cboPMNav_Stepnavigate_status)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="ok_action", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=False, iLength:=2, InterfaceControl:=cboPMNav_Stepok_action)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="cancel_action", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=False, iLength:=2, InterfaceControl:=cboPMNav_Stepcancel_action)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="ok_no_of_steps", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=txtPMNav_Stepok_no_of_steps)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="cancel_no_of_steps", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=txtPMNav_Stepcancel_no_of_steps)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="description", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=255, InterfaceControl:=txtPMNav_Stepdescription)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="caption_id", vValue:=0, iValidation:=NavProcConst.ACTextLookUp, iMandatory:=False, InterfaceControl:=txtPMNav_Stepcaption_id, sTag:="0")

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="is_hidden", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkPMNav_Stepis_hidden)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="is_logged", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkPMNav_Stepis_logged)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="ok_nav_process_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Stepok_nav_process_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavStepFields, sName:="cancel_nav_process_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Stepcancel_nav_process_id)

            '**************************************************************************************
            'Set Up PMNav_Process
            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="pmproduct_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=cboPMNav_Processpmproduct_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="pmproc_lock_group_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Processpmproc_lock_group_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="transaction_type_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=False, InterfaceControl:=cboPMNav_Processtransaction_type_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="start_nav_map_id", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=cboPMNav_Processstart_nav_map_id)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="process_mode", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=cboPMNav_Processprocess_mode)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="code", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=10, InterfaceControl:=txtPMNav_Processcode)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="description", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=255, InterfaceControl:=txtPMNav_Processdescription)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="effective_date", vValue:="", iValidation:=NavProcConst.ACDate, iMandatory:=True, InterfaceControl:=txtPMNav_Processeffective_date)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="caption_id", vValue:=0, iValidation:=NavProcConst.ACTextLookUp, iMandatory:=True, InterfaceControl:=txtPMNav_Processcaption_id, sTag:="0")

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="is_logged", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkPMNav_Processis_logged)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="is_user_driven", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=cboPMNav_Processis_user_driven)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavProcessFields, sName:="is_deleted", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkIsDeleted)

            '************************************************************************
            'Set Up PMNav_Component

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="object_name", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=30, InterfaceControl:=txtPMNav_Componentobject_name)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="nav_component_type", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=2, InterfaceControl:=cboPMNav_Componentnav_component_type)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="class_name", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=30, InterfaceControl:=txtPMNav_Componentclass_name)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="description", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=255, InterfaceControl:=txtPMNav_Componentdescription)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="effective_date", vValue:="", iValidation:=NavProcConst.ACDate, iMandatory:=True, InterfaceControl:=txtPMNav_Componenteffective_date)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="caption_id", vValue:=0, iValidation:=NavProcConst.ACTextLookUp, iMandatory:=False, InterfaceControl:=txtPMNav_Componentcaption_id, sTag:="0")

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="is_server_side", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkPMNav_Componentis_server_side)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavComponentFields, sName:="is_deleted", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkIsDeleted)

            '************************************************************************
            'Set Up PMNav_Map

            m_lReturn = SetUpField(cFieldList:=m_cPMNavMapFields, sName:="code", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=10, InterfaceControl:=txtPMNav_Mapcode)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavMapFields, sName:="caption_id", vValue:=0, iValidation:=NavProcConst.ACTextLookUp, iMandatory:=True, InterfaceControl:=txtPMNav_Mapcaption_id, sTag:="0")

            m_lReturn = SetUpField(cFieldList:=m_cPMNavMapFields, sName:="Description", vValue:="", iValidation:=NavProcConst.ACText, iMandatory:=True, iLength:=255, InterfaceControl:=txtPMNav_MapDescription)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavMapFields, sName:="effective_date", vValue:="", iValidation:=NavProcConst.ACDate, iMandatory:=True, InterfaceControl:=txtPMNav_Mapeffective_date)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavMapFields, sName:="is_start_map", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkPMNav_Mapis_start_map)

            m_lReturn = SetUpField(cFieldList:=m_cPMNavMapFields, sName:="is_deleted", vValue:=0, iValidation:=NavProcConst.ACNumeric, iMandatory:=True, InterfaceControl:=chkIsDeleted)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetUpAllFields Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpAllFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetUpField
    '
    ' Description: Sets up data for all fields
    '
    ' ***************************************************************** '
    Private Function SetUpField(Optional ByRef cFieldList As Collection = Nothing, Optional ByRef sName As String = "", Optional ByRef vValue As Object = Nothing, Optional ByRef iValidation As Integer = 0, Optional ByRef iMandatory As Integer = 0, Optional ByRef iLength As Integer = 0, Optional ByRef InterfaceControl As Object = Nothing, Optional ByRef sTag As String = "") As Integer

        Dim result As Integer = 0
        Dim oField As PMNavField

        Try

            oField = New PMNavField()

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set up the object details
            oField.Name = sName

            oField.Value = CStr(vValue)
            oField.Validation = iValidation
            oField.Mandatory = iMandatory

            oField.InterfaceControl = InterfaceControl

            'Tag used for lookup text box
            If Not (False) Then
                oField.InterfaceControl.Tag = sTag
            End If

            'Get the length if relevant
            If Not False Then
                oField.Length = iLength
            End If

            'Add the Field to the collection
            cFieldList.Add(oField, sName)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetUpField Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                'Close everthing down
                CloseInterface()
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function BusinessToInterface(ByRef bExpanded As Boolean) As Integer

        Dim result As Integer = 0
        Dim vEventIcon As String = ""
        Dim iLevel As Integer
        Dim sParent, sText, sTag, sKey As String
        Dim oNewNode As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Assign the details to the interface.

            'Clear All Nodes
            treMainData.Nodes.Clear()

            'Clear current Node IDs
            m_lID = 0
            m_lParentID = 0

            ' Display the root node.
            oNewNode = treMainData.Nodes.Add(NavProcConst.ACRoot, "", "Root", "Root")


            oNewNode.Tag = -1

            oNewNode.Expanded = True

            oNewNode.Selected = True

            '[ParentID,(RP|PM|MS|SM)ID,Level, Description]

            For lRow As Integer = 1 To m_cProcessMap.Count

                ' Get the details for this entry
                m_lReturn = GetProcessMapEntry(CStr(m_cProcessMap.Item(lRow)), sParent, sKey, iLevel, sText)

                sTag = sKey

                ' Determine position on tree

                Select Case iLevel
                    Case NavProcConst.NodeProcess
                        vEventIcon = "Process"

                    Case NavProcConst.NodeMap, NavProcConst.NodeSubMap
                        vEventIcon = "Map"

                    Case NavProcConst.NodeStepFindForm
                        vEventIcon = "StepFind"

                    Case NavProcConst.NodeStepDecisionForm
                        vEventIcon = "StepDecision"

                    Case NavProcConst.NodeStepDataForm
                        vEventIcon = "StepDataForm"

                    Case NavProcConst.NodeStepBusinessObject
                        vEventIcon = "StepNoForm"

                    Case NavProcConst.NodeStepSubMap
                        vEventIcon = "StepSubMap"

                    Case NavProcConst.NodeComponent
                        vEventIcon = "Component"

                End Select

                ' Add node to tree.
                m_lReturn = UpdateEventInterface(lMode:=gPMConstants.PMEComponentAction.PMAdd, bExpanded:=bExpanded, v_sTag:=sTag, vParent:=sParent, vKey:=sKey, vText:=sText, vEventIcon:=vEventIcon)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Return result
                End If

            Next lRow

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateApply
    '
    ' Description:  Update the Apply
    '
    ' ***************************************************************** '
    Private Sub UpdateApply(Optional ByRef iStatus As Integer = 0)

        'IF the apply button has been locked then exit
        If m_bApplyLocked Then
            Exit Sub
        End If

        If Not False Then

            If iStatus = m_iStatus Then
                Exit Sub
            End If

            m_iStatus = iStatus
        End If

        Select Case m_iStatus
            Case ACNoChange, ACApplied
                cmdApply.Enabled = False
            Case ACChanged
                cmdApply.Enabled = True
        End Select

    End Sub

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: UpdateTaxInterface
    '
    ' Description: Updates the party interface details.
    '
    ' ***************************************************************** '
    Private Function UpdateEventInterface(ByRef lMode As Integer, ByRef bExpanded As Boolean, ByRef v_sTag As String, Optional ByRef vParent As Object = Nothing, Optional ByRef vKey As Object = Nothing, Optional ByRef vText As Object = Nothing, Optional ByRef vSelected As Object = Nothing, Optional ByRef vEventIcon As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oNewNode As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the party interface.

            ' Check the task.
            Select Case (lMode)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Update the interface with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Add the new node.

                    oNewNode = treMainData.Nodes.Find(vParent, True)(0).Nodes.Add(vKey, CStr(vText).Trim(), vEventIcon)

                    ' {* USER DEFINED CODE (End) *}

                    ' Check if we need to select this node.

                    If Not Information.IsNothing(vSelected) Then
                        ' Select the tax node.

                        'developer guide no. 200
                        oNewNode.Checked = True
                    End If


                    'developer guide no. 201
                    oNewNode.Expand()

                    ' Store the unique ID.
                    'oNewNode.Tag = m_iEventIndex%
                    oNewNode.Tag = v_sTag

                    ' Increment the tax index value.
                    m_iEventIndex += 1

                    '            ' Check if we need to return the index value.
                    '            If (IsMissing(vIndex) = False) Then
                    '                vIndex = oNewNode.Tag
                    '            End If

                Case gPMConstants.PMEComponentAction.PMDelete
                    ' Update the interface with a deleted data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    treMainData.Nodes.RemoveAt(CInt(vKey) - 1)
                    ' {* USER DEFINED CODE (End) *}
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the event interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEventInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (InterfaceToData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function InterfaceToData() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' Update the data storage.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to assign all of the details from the
    ' interface to the data storage.
    ''
    ' Example:-
    ''
    '    m_DName$ = trim$(txtName.Text)
    '    m_DDate = CDate(txtDate.Text)
    '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.

            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***************************************************************************

            'Set default group variables
            m_sPMNavGroup = ""
            m_lGroupID = 0

            'Set Default Mode/Status variables
            m_iStatus = ACNoChange
            m_iMode = gPMConstants.PMEComponentAction.PMAdd
            m_bApplyLocked = False

            'Set default node keys
            m_lID = 0
            m_lParentID = 0

            'Set Rule enforcement
            m_bEnforceRules = False
            m_bAllowRuleEnforcement = True

            'Setup information for all database fields
            m_lReturn = SetUpAllFields()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***************************************************************************


            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            '    ReDim m_ctlTabFirstLast(1, )

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Display the Title for this Project

            Me.Text = "Navigator Process Editor v" & My.Application.Info.Version.Major & "." & _
                      My.Application.Info.Version.Minor & "." & CStr(My.Application.Info.Version.Revision) & " - [" & _
                      m_oBusiness.DSN & "]"

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Build all LookUps and LookUp Controls (Use Progress Bar)

            'Get all Maps
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Maps...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Map)

            'Get Start Maps
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Start Maps...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_StartMap)

            'Get Sub Maps
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Sub Maps...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_SubMap)

            'Get Processes
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Processes...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Process)

            'Get Products
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving PMProducts...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMProduct)

            'Get Lock Groups
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Lock Groups...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMProc_Lock_Group)

            'Get transaction types
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Transaction Types...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabTransaction_Type)

            'Get Components
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Components...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Component)

            'Get tasks
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Tasks...")
            m_lReturn = BuildLookUp(NavProcConst.ACLTasks)

            'Get Navigator Status
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Navigator Status...")
            m_lReturn = BuildLookUp(NavProcConst.ACLNavigateStatus)

            'Get actions
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Actions...")
            m_lReturn = BuildLookUp(NavProcConst.ACLActions)

            'Get Process modes
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Process Modes...")
            m_lReturn = BuildLookUp(NavProcConst.ACLProcessModes)

            'Get component types
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Component Types...")
            m_lReturn = BuildLookUp(NavProcConst.ACLComponentTypes)

            'Get Navigator modes
            'developer guide no. 50
            objfrmProgress.SetProgress("Retrieving Navigator Modes...")
            m_lReturn = BuildLookUp(NavProcConst.ACLNavigatorModes)

            'Complete progress
            'developer guide no. 50
            objfrmProgress.Complete()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Private)
    '
    ' Description: Gets the Keys for the selected node.
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetKeys) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
'Private Function GetKeys(ByVal v_lNodeID(,) As Integer, ByVal v_iLevel As Integer, ByVal v_lParentID As Integer, ByRef r_vKeyArray As Object ) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'm_lReturn = m_oBusiness.GetKeys(v_lNodeID:=v_lNodeID, v_iLevel:=v_iLevel, v_lParentID:=v_lParentID, r_vKeyArray:=r_vKeyArray)
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the keys", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: ResizeInterfaceDetails
    '
    ' Description: Resizes all of the interface details.
    '
    ' ***************************************************************** '
    Private Sub ResizeInterfaceDetails()

        Dim lRepositionValue As Integer

        Try

            With Me
                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(cmdApply.Width)) - 190) ' 150
                If lRepositionValue > 0 Then
                    cmdApply.Left = VB6.TwipsToPixelsX(lRepositionValue)
                End If
                lRepositionValue = CInt((VB6.PixelsToTwipsY(.Height) - VB6.PixelsToTwipsY(cmdApply.Height)) - 780) ' 1065
                If lRepositionValue > 0 Then
                    cmdApply.Top = VB6.TwipsToPixelsY(lRepositionValue)
                End If

                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(cmdExit.Width)) - 1390)
                If lRepositionValue > 0 Then
                    cmdExit.Left = VB6.TwipsToPixelsX(lRepositionValue)
                End If
                cmdExit.Top = cmdApply.Top

                lRepositionValue = CInt(VB6.PixelsToTwipsY(.Height) - 2000) '2280
                If lRepositionValue > 0 Then
                    treMainData.Height = VB6.TwipsToPixelsY(lRepositionValue)
                End If

                panSliderBar.Height = treMainData.Height

                'developer guide no. 27 (Latest guide)
                LinSliderBar.Height = VB6.PixelsToTwipsY(treMainData.Height)

                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 300) '180
                If lRepositionValue > 0 Then
                    m_Interface.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 180)
                    'm_Interface.Width = lRepositionValue&
                End If

                m_Interface.Height = treMainData.Height ' + 95

                cboGroupDescription.Width = m_Interface.Width - VB6.TwipsToPixelsX(10) '30


                'developer guide no.27(Latest guide) 
                linMenuLine1.Width = VB6.PixelsToTwipsX(.Width)

                'developer guide no. 27 (Latest guide)
                linMenuLine2.Width = VB6.PixelsToTwipsX(.Width)
            End With

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: InsertFields (Private)
    '
    ' Description: Inserts fields to database
    '
    ' ***************************************************************** '
    Private Function InsertFields() As Integer

        Dim result As Integer = 0
        Dim sFieldValue As String = ""
        Dim sFieldValues As New StringBuilder
        Dim sFieldNames As New StringBuilder
        Dim vFieldControl As Control
        Dim sValidation As String = ""
        Dim lFieldID, lParentID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Make sure that we are in Add Mode
            If m_iMode <> gPMConstants.PMEComponentAction.PMAdd Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sPMNavGroup = NavProcConst.NavGrpStep Then

                'If this is a Step then get the MapID

                lParentID = m_cPMNavFields.Item("pmnav_map_id").Value
            Else

                'Otherwise ignore the mapid
                lParentID = 0
            End If

            'Build the Field list
            For iPtr As Integer = 1 To m_cPMNavFields.Count

                'Get the interface control

                vFieldControl = m_cPMNavFields.Item(iPtr).InterfaceControl

                'Get the field's validation type

                sValidation = m_cPMNavFields.Item(iPtr).Validation

                'Get the field name

                sFieldNames.Append(m_cPMNavFields.Item(iPtr).Name & ", ")

                'Get the field value

                sFieldValue = m_cPMNavFields.Item(iPtr).Value

                'Check for nullable fields(lookup fields)
                If TypeOf vFieldControl Is ComboBox Then
                    If vFieldControl.ToString() = "" Then
                        sFieldValue = "Null"
                    End If
                End If

                'Get field details
                If StringsHelper.ToDoubleSafe(sValidation) <> NavProcConst.ACTextLookUp Then

                    'Add quotes for non numeric data
                    If StringsHelper.ToDoubleSafe(sValidation) <> NavProcConst.ACNumeric And sFieldValue <> "Null" Then
                        sFieldValue = "'" & sFieldValue & "'"
                    End If

                    'Set the field values
                    sFieldValues.Append(sFieldValue & ", ")

                Else

                    'Update the text lookup field return id
                    m_lReturn = UpdateFieldLookUp(vFieldControl)

                    'Get the returned id
                    If m_lReturn = 0 Then
                        sFieldValue = "Null"
                    Else
                        sFieldValue = CStr(m_lReturn)
                    End If

                    'Value returned is
                    sFieldValues.Append(sFieldValue & ", ")
                End If

            Next iPtr

            ' Remove trailing characters
            sFieldNames = New StringBuilder(sFieldNames.ToString().Substring(0, sFieldNames.ToString().Length - 2))
            sFieldValues = New StringBuilder(sFieldValues.ToString().Substring(0, sFieldValues.ToString().Length - 2))

            'Insert the fields (MapID only used for a Step)

            lFieldID = m_oBusiness.InsertFields(sPMNavGroup:=m_sPMNavGroup, sFieldNames:=sFieldNames.ToString(), sFieldValues:=sFieldValues.ToString(), lParentID:=lParentID, vEnforceRules:=m_bEnforceRules)

            If lFieldID <> 0 Then

                'If this is a Process then set the groupid
                If m_sPMNavGroup = NavProcConst.NavGrpProcess Then

                    If cboGroups.Text <> m_sPMNavGroup Then

                        m_iStatus = ACApplied
                        cboGroups.Text = m_sPMNavGroup

                    End If

                    m_lGroupID = lFieldID

                    m_oBusiness.CurrentProcessID = m_lGroupID

                    'If this is a Map or a component then set group to 0
                ElseIf ((m_sPMNavGroup = NavProcConst.NavGrpMap) Or (m_sPMNavGroup = NavProcConst.NavGrpComponent)) Then

                    m_lGroupID = 0

                    m_oBusiness.CurrentProcessID = m_lGroupID

                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertFields Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateFields (Private)
    '
    ' Description: Updates fields to database
    '
    ' ***************************************************************** '
    Private Function UpdateFields() As Integer

        Dim result As Integer = 0
        Dim sFieldName, sFieldValue As String
        Dim sFieldValues As New StringBuilder
        Dim vFieldControl As Control
        Dim sValidation As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Make sure that we have a current ID
            If m_lID = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Build the Field list
            For iPtr As Integer = 1 To m_cPMNavFields.Count

                'Get the field's validation type

                sValidation = m_cPMNavFields.Item(iPtr).Validation

                'Get the interface control

                vFieldControl = m_cPMNavFields.Item(iPtr).InterfaceControl

                'Get field details
                If StringsHelper.ToDoubleSafe(sValidation) <> NavProcConst.ACTextLookUp Then

                    'Get the field name

                    sFieldName = m_cPMNavFields.Item(iPtr).Name & " = "

                    'Get the field value

                    sFieldValue = m_cPMNavFields.Item(iPtr).Value

                    'Check for nullable fields(lookup fields)
                    If TypeOf vFieldControl Is ComboBox Then
                        If vFieldControl.ToString() = "" Then
                            sFieldValue = "Null"
                        End If
                    End If

                    'Add quotes for non numeric data
                    If StringsHelper.ToDoubleSafe(sValidation) <> NavProcConst.ACNumeric And sFieldValue <> "Null" Then
                        sFieldValue = "'" & sFieldValue & "'"
                    End If

                    'Set the field values
                    sFieldValues.Append( _
                                        sFieldName & sFieldValue & ", ")

                Else

                    'Get the field name

                    sFieldName = m_cPMNavFields.Item(iPtr).Name & " = "

                    'Update the text lookup field
                    m_lReturn = UpdateFieldLookUp(vFieldControl)

                    'Get the returned id
                    If m_lReturn = 0 Then
                        sFieldValue = "Null"
                    Else
                        sFieldValue = CStr(m_lReturn)
                    End If

                    'Set the field values
                    sFieldValues.Append( _
                                        sFieldName & sFieldValue & ", ")

                End If

            Next iPtr

            ' Remove trailing characters
            sFieldValues = New StringBuilder(sFieldValues.ToString().Substring(0, sFieldValues.ToString().Length - 2))

            'Update fields

            m_lReturn = m_oBusiness.UpdateFields(sPMNavGroup:=m_sPMNavGroup, sUpdateClause:=sFieldValues.ToString(), lID:=m_lID, lParentID:=m_lParentID, vEnforceRules:=m_bEnforceRules)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFields Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateFieldLookUp (Private)
    '
    ' Description: Updates a lookup field
    '
    ' ***************************************************************** '
    Private Function UpdateFieldLookUp(ByRef vFieldControl As Control) As Integer

        Dim result As Integer = 0
        Dim lID As Integer
        Dim sTextValue As String = ""

        Try


            lID = CInt(Convert.ToString(ControlHelper.GetTag(vFieldControl)))

            sTextValue = vFieldControl.Text

            'If no text then exit without a fuss
            If sTextValue = "" Then
                Return result
            End If


            Select Case vFieldControl.Name
                Case "txtPMNav_Processcaption_id", "txtPMNav_Componentcaption_id", "txtPMNav_Mapcaption_id", "txtPMNav_Stepcaption_id"

                    'Update the Field value on table

                    m_lReturn = m_oBusiness.UpdateFieldLookUp(sTableName:="PMCaption", lID:=lID, vValue:=sTextValue)

                    'Get id returned
                    result = m_lReturn

            End Select

            Return result

        Catch


            Return 0
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: UpdateLookUps (Private)
    '
    ' Description: Function to update the LookUp Tables after a new
    '
    ' ***************************************************************** '
    Private Function UpdateLookUps() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case m_sPMNavGroup
                'Update Process LookUps
                Case NavProcConst.NavGrpProcess
                    m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Process)

                    'Update Map LookUps
                Case NavProcConst.NavGrpMap
                    m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Map)
                    m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_StartMap)
                    m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_SubMap)

                    'Update Component LookUps
                Case NavProcConst.NavGrpComponent
                    m_lReturn = BuildLookUp(NavProcConst.ACLTabPMNav_Component)

                Case Else

            End Select

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateLookUps", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLookUps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateProcessMap (Private)
    '
    ' Description: Updates the current Process
    '
    ' ***************************************************************** '
    Private Function UpdateProcessMap(Optional ByRef vGroupID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""


        Dim nNode As TreeNode = Nothing
        Try
            'Get the selectedItem
            nNode = treMainData.SelectedNode

        Catch
        End Try




        result = gPMConstants.PMEReturnCode.PMTrue

        'If passed a group id, then use it

        If Not Information.IsNothing(vGroupID) Then

            m_lGroupID = CInt(vGroupID)
        End If

        'Exit if we have a 0 process
        If m_lGroupID = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'Clear the Process Map

        m_lReturn = m_oBusiness.ClearProcessMap()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result
        End If

        'Build this Group
        If m_lGroupID <> 0 Then


            Select Case cboGroups.Text
                Case NavProcConst.NavGrpProcess

                    m_lReturn = m_oBusiness.BuildProcess(m_lGroupID)

                Case NavProcConst.NavGrpMap

                    m_lReturn = m_oBusiness.BuildMap(m_lGroupID, NavProcConst.ACRoot)

                Case NavProcConst.NavGrpComponent

                    m_lReturn = m_oBusiness.BuildComponent(m_lGroupID, NavProcConst.ACRoot)

            End Select

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lGroupID = 0

            m_oBusiness.CurrentProcessID = 0
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result
        End If

        'Clear the Process Map
        m_cProcessMap = Nothing

        m_cProcessMap = m_oBusiness.ProcessMap

        m_lReturn = BusinessToInterface(bExpanded:=True)

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        'If a prior node was selected, then reselect it

        Try
            If Not (nNode Is Nothing) Then

                'developer guide no. 200
                treMainData.Nodes.Item(nNode.Name).Checked = True
                treMainData_AfterSelect(treMainData, New TreeViewEventArgs(nNode))
            End If

            Return result

Err_UpdateProcessMap:

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateProcessMap Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProcessMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateFields (Private)
    '
    ' Description: Validates the current set of fields
    '
    ' ***************************************************************** '
    Private Function ValidateFields() As Integer

        Dim result As Integer = 0
        Dim vIControl As Control
        Dim vValue As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iPtr As Integer = 1 To m_cPMNavFields.Count


                vIControl = m_cPMNavFields.Item(iPtr).InterfaceControl

                'Get the Control's value
                If TypeOf vIControl Is ComboBox Then


                    vValue = GetLookUpControl(vIControl)
                Else

                    vValue = (vIControl)
                End If

                'Mandatory Validation


                If m_cPMNavFields.Item(iPtr).Mandatory Then

                    'Make sure that there is an entry for this value

                    If CStr(vValue) = "" Then
                        MessageBox.Show("You must enter a value for this field.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        vIControl.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                ' Validation


                Select Case m_cPMNavFields.Item(iPtr).Validation
                    Case NavProcConst.ACText
                        'If text then truncate to correct length



                        m_cPMNavFields.Item(iPtr).Value = CStr(vValue).Substring(0, m_cPMNavFields.Item(iPtr).Length)

                    Case NavProcConst.ACTextLookUp
                        'If this is a text lookup then use tag

                        m_cPMNavFields.Item(iPtr).Value = Conversion.Val(Convert.ToString(ControlHelper.GetTag(vIControl)))

                    Case NavProcConst.ACNumeric
                        'If numeric then make sure the control is numeric


                        vValue = Conversion.Val(CStr(vValue))


                        m_cPMNavFields.Item(iPtr).Value = Conversion.Val(CStr(vValue))

                    Case NavProcConst.ACDate
                        'If date then make sure the control is date
                        If Not Information.IsDate(vValue) Then
                            MessageBox.Show("Please enter a valid date.", "Invalid Field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            vIControl.Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If



                        m_cPMNavFields.Item(iPtr).Value = CDate(vValue).ToString("MM/dd/yyyy")

                End Select

            Next iPtr

            'Validate the fields by the current pm navigator group
            m_lReturn = ValidateFieldsByGroup()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateFields Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateFieldsByGroup (Private)
    '
    ' Description: Special-case Validation for field groups
    '
    ' ***************************************************************** '
    Private Function ValidateFieldsByGroup() As Integer

        Dim result As Integer = 0
        Dim vVal As Object
        Dim sTitle, sError As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sTitle = "Incorrect Field"


            Select Case m_sPMNavGroup
                Case NavProcConst.NavGrpComponent, NavProcConst.NavGrpProcess, NavProcConst.NavGrpMap

                Case NavProcConst.NavGrpStep

                    'Define error message
                    sError = "Please enter a number of steps greater than 0"

                    'Get the step ok action code


                    vVal = CStr(GetLookUpControl(cboPMNav_Stepok_action))

                    'If number of steps is needed, then make sure it is >0

                    If CStr(vVal) = gPMConstants.PMNavActionForwardX Or CStr(vVal) = gPMConstants.PMNavActionBackX Then

                        If Conversion.Val(txtPMNav_Stepok_no_of_steps.Text) < 1 Then
                            MessageBox.Show(sError, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            txtPMNav_Stepok_no_of_steps.Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If

                    'Get the step cancel action code


                    vVal = GetLookUpControl(cboPMNav_Stepcancel_action)

                    'If number of steps is needed, then make sure it is >0

                    If CStr(vVal) = gPMConstants.PMNavActionForwardX Or CStr(vVal) = gPMConstants.PMNavActionBackX Then

                        If Conversion.Val(txtPMNav_Stepcancel_no_of_steps.Text) < 1 Then
                            MessageBox.Show(sError, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            txtPMNav_Stepcancel_no_of_steps.Focus()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If

                Case Else

            End Select

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateFieldsByGroup Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateFieldsByGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: VerifyNavigatorTables (Private)
    '
    ' Description:  Makes sure that all Navigator Tables are valid
    '
    '
    ' ***************************************************************** '
    Private Function VerifyNavigatorTables() As Integer

        Dim result As Integer = 0
        Dim vNavTables(0) As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vNavTables(0) = "PMNav_Process"

            'Show the progress form
            'developer guide no. 50
            m_lReturn = objfrmProgress.Initialise(sDescription:="Verifying Navigator Tables...", iStartValue:=0, iMin:=0, iMax:=4)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Verify all navigator tables
            For iPtr As Integer = vNavTables.GetLowerBound(0) To vNavTables.GetUpperBound(0)


                m_lReturn = m_oBusiness.VerifyTable(vNavTables(iPtr))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("This database is missing or has invalid Navigator Tables.", "NPE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'developer guide no. 50
                    objfrmProgress.Close()
                    Application.DoEvents()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'developer guide no. 50
                objfrmProgress.Increment()

            Next iPtr

            'Complete Progress
            'developer guide no. 50
            objfrmProgress.Complete()

            Return result

        Catch




            Return result
        End Try
    End Function

    Private Sub cboGroupDescription_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGroupDescription.SelectedIndexChanged

        Dim lID As Integer
        Dim bExpanded As Boolean

        Try

            If cboGroupDescription.Text = "" Then
                Exit Sub
            End If


            m_lReturn = m_oBusiness.ClearProcessMap()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            Select Case (cboGroups.Text)
                'If Process then Build The Navigator Process
                Case NavProcConst.NavGrpProcess


                    lID = CInt(Conversion.Val(CStr(GetLookUpControl(cboGroupDescription))))

                    'Set the current process
                    m_lGroupID = lID

                    m_oBusiness.CurrentProcessID = lID

                    If lID <> 0 Then
                        'Build this Process
                        bExpanded = True 'Expand the node

                        m_lReturn = m_oBusiness.BuildProcess(lID)
                    Else

                        'Build All Processes
                        bExpanded = False

                        For iPtr As Integer = m_vLTabPMNav_Process.GetLowerBound(1) To m_vLTabPMNav_Process.GetUpperBound(1)



                            m_lReturn = m_oBusiness.BuildProcess(CInt(m_vLTabPMNav_Process(0, iPtr)))

                        Next iPtr
                    End If

                    'If Map thenBuild the Navigator Map
                Case NavProcConst.NavGrpMap

                    lID = CInt(Conversion.Val(CStr(GetLookUpControl(cboGroupDescription))))

                    'Set the current process
                    m_lGroupID = lID

                    m_oBusiness.CurrentProcessID = 0

                    If lID <> 0 Then
                        'Build This Process
                        bExpanded = True

                        m_lReturn = m_oBusiness.BuildMap(lID, NavProcConst.ACRoot)
                    Else
                        'Build All Maps
                        bExpanded = False

                        For iPtr As Integer = m_vLTabPMNav_Map.GetLowerBound(1) To m_vLTabPMNav_Map.GetUpperBound(1)



                            m_lReturn = m_oBusiness.BuildMap(CInt(m_vLTabPMNav_Map(0, iPtr)), NavProcConst.ACRoot)

                        Next iPtr
                    End If

                    'If Component then Build the Navigator Components
                Case NavProcConst.NavGrpComponent

                    lID = CInt(Conversion.Val(CStr(GetLookUpControl(cboGroupDescription))))

                    'Set the current process
                    m_lGroupID = lID

                    m_oBusiness.CurrentProcessID = 0

                    If lID <> 0 Then
                        'Build This Process
                        bExpanded = True

                        m_lReturn = m_oBusiness.BuildComponent(lID, NavProcConst.ACRoot)

                    Else
                        'Build All Maps
                        bExpanded = False

                        For iPtr As Integer = m_vLTabPMNav_Component.GetLowerBound(1) To m_vLTabPMNav_Component.GetUpperBound(1)



                            m_lReturn = m_oBusiness.BuildComponent(CInt(m_vLTabPMNav_Component(0, iPtr)), NavProcConst.ACRoot)

                        Next iPtr
                    End If

                Case Else

            End Select


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Clear the Process Map
                m_cProcessMap = Nothing

                m_cProcessMap = m_oBusiness.ProcessMap

                m_lReturn = BusinessToInterface(bExpanded:=bExpanded)

            End If

            DisplayInterface(m_sPMNavGroup)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try

    End Sub


    Private Sub cboGroups_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGroups.SelectedIndexChanged

        Dim iMsgRet As Integer
        Static InProcess As Boolean

        'If We are already in this sub then do not recurse
        If InProcess Then
            Exit Sub
        End If

        'Check if details have changed
        If m_iStatus = ACChanged Then
            iMsgRet = MessageBox.Show("The Details have changed. " & _
                      "Do you wish to discard your changes?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If iMsgRet = System.Windows.Forms.DialogResult.No Then
                InProcess = True
                cboGroups.Text = m_sPMNavGroup
                InProcess = False
                Exit Sub
            End If

        End If

        'No Process
        m_lGroupID = 0

        m_oBusiness.CurrentProcessID = 0

        'Default To Add
        m_iMode = gPMConstants.PMEComponentAction.PMAdd

        'Set the Current GroupName
        m_sPMNavGroup = cboGroups.Text

        'Build the GroupDescription list if any
        m_lReturn = DisplayGroupDescription(m_sPMNavGroup)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'Display the interface for the selected group
        m_lReturn = DisplayInterface(m_sPMNavGroup)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'Build the selected group
        m_lReturn = BuildGroup(m_sPMNavGroup)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    Private Sub cboPMNav_Componentnav_component_type_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Componentnav_component_type.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Componentnav_component_type_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Componentnav_component_type.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Componentnav_component_type.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Componentnav_component_type_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Componentnav_component_type.Leave

        InterfaceConditions(NavProcConst.NavGrpComponent, 0)

    End Sub


    Private Sub cboPMNav_Processis_user_driven_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Processis_user_driven.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Processis_user_driven_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Processis_user_driven.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Processis_user_driven.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Processpmproc_lock_group_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Processpmproc_lock_group_id.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Processpmproc_lock_group_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Processpmproc_lock_group_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Processpmproc_lock_group_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Processpmproduct_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Processpmproduct_id.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Processpmproduct_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Processpmproduct_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Processpmproduct_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Processprocess_mode_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Processprocess_mode.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Processprocess_mode_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Processprocess_mode.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Processprocess_mode.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Processstart_nav_map_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Processstart_nav_map_id.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Processstart_nav_map_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Processstart_nav_map_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Processstart_nav_map_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Processtransaction_type_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Processtransaction_type_id.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Processtransaction_type_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Processtransaction_type_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Processtransaction_type_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Stepcancel_action_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepcancel_action.SelectedIndexChanged

        'Set the Interface Conditions
        InterfaceConditions(NavProcConst.NavGrpStep, 1)

        'Update the Apply
        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Stepcancel_action_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Stepcancel_action.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Stepcancel_action.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Stepcancel_nav_process_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepcancel_nav_process_id.SelectedIndexChanged

        'Set tool tips
        ToolTip1.SetToolTip(cboPMNav_Stepcancel_nav_process_id, cboPMNav_Stepcancel_nav_process_id.Text)

        'Selected a Sub Process so enforce rules
        m_bEnforceRules = m_bAllowRuleEnforcement

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Stepcancel_nav_process_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Stepcancel_nav_process_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Stepcancel_nav_process_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Stepnavigate_status_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepnavigate_status.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Stepnavigate_status_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Stepnavigate_status.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Stepnavigate_status.SelectedIndex = -1
        End If


    End Sub


    Private Sub cboPMNav_Stepok_action_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepok_action.SelectedIndexChanged

        'Set the Interface Conditions
        InterfaceConditions(NavProcConst.NavGrpStep, 0)

        'Update the Apply
        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Stepok_action_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Stepok_action.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Stepok_action.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Stepok_nav_process_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepok_nav_process_id.SelectedIndexChanged

        'Set tool tips
        ToolTip1.SetToolTip(cboPMNav_Stepok_nav_process_id, cboPMNav_Stepok_nav_process_id.Text)

        'Selected a Sub Process so enforce rules
        m_bEnforceRules = m_bAllowRuleEnforcement

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Stepok_nav_process_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Stepok_nav_process_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Stepok_nav_process_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Steppmnav_component_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Steppmnav_component_id.SelectedIndexChanged


        'Apply rules for component/submap
        Dim iListIndex As Integer = cboPMNav_Steppmnav_component_id.SelectedIndex
        cboPMNav_Stepsub_nav_map_id.SelectedIndex = -1
        cboPMNav_Steppmnav_component_id.SelectedIndex = iListIndex

        'The component has changed for this step, so enforce rules
        m_bEnforceRules = m_bAllowRuleEnforcement

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Steppmnav_component_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Steppmnav_component_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Steppmnav_component_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Steppmnav_map_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Steppmnav_map_id.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub



    Private Sub cboPMNav_Steppmnav_map_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Steppmnav_map_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Steppmnav_map_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Stepsub_nav_map_id_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepsub_nav_map_id.SelectedIndexChanged


        Dim iListIndex As Integer = cboPMNav_Stepsub_nav_map_id.SelectedIndex
        cboPMNav_Steppmnav_component_id.SelectedIndex = -1
        cboPMNav_Stepsub_nav_map_id.SelectedIndex = iListIndex

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Stepsub_nav_map_id_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Stepsub_nav_map_id.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Stepsub_nav_map_id.SelectedIndex = -1
        End If

    End Sub


    Private Sub cboPMNav_Stepsub_nav_map_id_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Stepsub_nav_map_id.Leave

        'Make sure a sub map is not hidden
        InterfaceConditions(NavProcConst.NavGrpStep, 2)

    End Sub


    Private Sub cboPMNav_Steptask_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMNav_Steptask.SelectedIndexChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cboPMNav_Steptask_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboPMNav_Steptask.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Delete Then
            cboPMNav_Steptask.SelectedIndex = -1
        End If

    End Sub


    Private Sub chkPMNav_Componentis_server_side_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Componentis_server_side.CheckStateChanged

        InterfaceConditions(NavProcConst.NavGrpComponent, 0)

        UpdateApply(ACChanged)

    End Sub
    Private Sub chkPMNav_Componentis_server_side_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Componentis_server_side.Leave

        InterfaceConditions(NavProcConst.NavGrpComponent, 0)

    End Sub


    Private Sub chkPMNav_Mapis_start_map_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Mapis_start_map.CheckStateChanged

        UpdateApply(ACChanged)

    End Sub

    Private Sub chkPMNav_Processis_logged_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Processis_logged.CheckStateChanged

        UpdateApply(ACChanged)

    End Sub

    Private Sub chkPMNav_Stepis_hidden_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Stepis_hidden.CheckStateChanged

        InterfaceConditions(NavProcConst.NavGrpStep, 3)

        UpdateApply(ACChanged)

    End Sub


    Private Sub chkPMNav_Stepis_hidden_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Stepis_hidden.Leave

        'Make sure a sub map is not hidden
        InterfaceConditions(NavProcConst.NavGrpStep, 2)

    End Sub


    Private Sub chkPMNav_Stepis_logged_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPMNav_Stepis_logged.CheckStateChanged

        UpdateApply(ACChanged)

    End Sub


    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        Dim sMessage As String = ""

        Try

            If m_iStatus <> ACChanged Then
                Exit Sub
            End If


            Select Case m_iMode
                Case gPMConstants.PMEComponentAction.PMEdit

                    'Validate the current fields
                    m_lReturn = ValidateFields()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'Update the database fields
                    m_lReturn = UpdateFields()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case gPMConstants.PMEComponentAction.PMAdd
                    'Update NEW LookUp Entries

                    'Validate the current fields
                    m_lReturn = ValidateFields()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'Create new record
                    m_lReturn = InsertFields()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    'Update the look up tables and restore the value afterwards
                    m_bRestoreLookup = True
                    UpdateLookUps()
                    m_bRestoreLookup = False

            End Select

            'Display the Rule Log results
            If m_bEnforceRules Then


                sMessage = m_oBusiness.RuleLog

                If sMessage <> "" Then

                    'Show rule log
                    'developer guide no. 50
                    objfrmLog.Initialise(sMessage)

                    'developer guide no. 50
                    objfrmLog.Close()

                End If

                'Clear rule enforcement
                m_bEnforceRules = False

            End If

            'If all goes well then applied
            UpdateApply(ACApplied)

            'Update the Navigator Map
            UpdateProcessMap()

            'Set the mode to Edit
            m_iMode = gPMConstants.PMEComponentAction.PMEdit

        Catch
        End Try





    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        m_lReturn = MessageBox.Show("Are you certain that you wish to exit.", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If m_lReturn = System.Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        'Terminate the form
        Dispose()

    End Sub

    Private Sub Form_Initialize_Renamed()

        m_cPMNavStepFields = New Collection()
        m_cPMNavProcessFields = New Collection()
        m_cPMNavMapFields = New Collection()
        m_cPMNavComponentFields = New Collection()

        'No error here
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            'Show Blank Interface Frame
            m_Interface = fraBlank
            ControlHelper.SetVisible(m_Interface, True)

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the status for the business object.

            m_lReturn = m_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            'Terminate NPE
            Dispose()

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        ResizeInterfaceDetails()

    End Sub



    Public Sub mnuEditCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditCopy.Click

        m_lReturn = Copy()

    End Sub


    Public Sub mnuEditEnforce_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditEnforce.Click


        Dim bEnforce As Boolean = Not mnuEditEnforce.Checked
        mnuEditEnforce.Checked = bEnforce
        m_bAllowRuleEnforcement = bEnforce

        If Not bEnforce Then
            m_bEnforceRules = False
        End If

    End Sub

    Public Sub mnuEditKeys_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEditKeys.Click

        GetSetKeys()

    End Sub



    Public Sub mnuGroupComponent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGroupComponent.Click

        Dim lID As Integer

        Try

            'Make sure that we are in step mode
            If m_sPMNavGroup <> NavProcConst.NavGrpStep Then
                Exit Sub
            End If

            'Make sure that the lookup control has a value
            If cboPMNav_Steppmnav_component_id.Text = "" Then
                Exit Sub
            End If

            'Get the value of the look up control

            lID = CInt(GetLookUpControl(cboPMNav_Steppmnav_component_id))

            'Display the component
            m_lReturn = DisplayInterface(NavProcConst.NavGrpComponent)

            'Get the Field data
            m_lReturn = GetPMNavFields(sPMNavGroup:=NavProcConst.NavGrpComponent, lID:=lID, lParentID:=0)

        Catch
        End Try





    End Sub

    Public Sub mnuGroupDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGroupDelete.Click

        m_lReturn = DeleteItem()

    End Sub

    Public Sub mnuKeys_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuKeys.Click

        GetSetKeys()

    End Sub

    Public Sub mnuNewItem_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNewItem.Click

        Try


            Select Case m_sPMNavGroup
                Case NavProcConst.NavGrpComponent, NavProcConst.NavGrpMap, NavProcConst.NavGrpStep

                    PMNavNew(NavProcConst.NavGrpStep)

                Case NavProcConst.NavGrpProcess

                    PMNavNew(NavProcConst.NavGrpMap)

                Case Else

            End Select

        Catch
        End Try




    End Sub


    Public Sub mnuStepComponents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuStepComponents.Click


        Dim bChecked As Boolean = Not mnuStepComponents.Checked
        mnuStepComponents.Checked = bChecked

        'Set the flag on the business

        m_oBusiness.StepComponent = bChecked

        'If we are in process/map mode, then redisplay
        If (m_lGroupID <> 0) And (cboGroups.Text = NavProcConst.NavGrpMap Or cboGroups.Text = NavProcConst.NavGrpProcess) Then
            UpdateProcessMap()
        End If

    End Sub

    Private Sub tlbToolBar_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tlbToolBar_Button1.Click, _tlbToolBar_Button2.Click, _tlbToolBar_Button3.Click, _tlbToolBar_Button4.Click, _tlbToolBar_Button5.Click, _tlbToolBar_Button6.Click, _tlbToolBar_Button7.Click, _tlbToolBar_Button8.Click, _tlbToolBar_Button9.Click, _tlbToolBar_Button10.Click, _tlbToolBar_Button11.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Dim iMsgRet As Integer

        Try

            'Check if details have changed
            If m_iStatus = ACChanged Then
                iMsgRet = MessageBox.Show("The Details have changed. " & _
                          "Do you wish to discard your changes?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If iMsgRet = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

            End If


            Select Case (Button.Owner.Items.IndexOf(Button))
                Case ACToolNewComponent

                    PMNavNew(NavProcConst.NavGrpComponent)

                Case ACToolNewProcess

                    PMNavNew(NavProcConst.NavGrpProcess)

                Case ACToolNewMap

                    PMNavNew(NavProcConst.NavGrpMap)

                Case ACToolNewStep

                    PMNavNew(NavProcConst.NavGrpStep)

                Case ACToolDelete
                    m_lReturn = DeleteItem()

                    m_iMode = gPMConstants.PMEComponentAction.PMEdit

                Case ACToolKeys
                    mnuEditKeys_Click(mnuEditKeys, New EventArgs())

                Case ACToolCopy
                    m_lReturn = Copy()

                Case ACToolPrint
                    m_lReturn = PrintOut()

                Case Else

            End Select

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to roll up tree view", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_tlbButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub treMainData_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles treMainData.DoubleClick

        Dim nNode As TreeNode
        Try

            'Do not dig, if we are in a group
            If cboGroupDescription.SelectedIndex <> -1 Then
                Exit Sub
            End If

            If (m_sPMNavGroup <> NavProcConst.NavGrpComponent) And (m_sPMNavGroup <> NavProcConst.NavGrpMap) And (m_sPMNavGroup <> NavProcConst.NavGrpProcess) Then

                Exit Sub

            End If

            nNode = treMainData.SelectedNode

            If nNode Is Nothing Then
                Exit Sub
            End If

            'cboGroupDescription = (nNode.Text)

            SetLookUPControl(cboGroupDescription, m_lID)

        Catch
        End Try




    End Sub

    Private Sub treMainData_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles treMainData.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Try

            ' Exit if not Right Mouse Click
            If Button <> MouseButtonConstants.RightButton Then
                Exit Sub
            End If

            'Set the captions for the menu

            Select Case m_sPMNavGroup
                Case NavProcConst.NavGrpComponent, NavProcConst.NavGrpMap
                    mnuNewItem.Text = "&New Step"
                    mnuGroupComponent.Available = False

                Case NavProcConst.NavGrpStep
                    mnuNewItem.Text = "&Insert Step"
                    mnuGroupComponent.Available = True

                Case NavProcConst.NavGrpProcess
                    mnuNewItem.Text = "&New Map"
                    mnuGroupComponent.Available = False

                Case Else

            End Select

            ' Display Sub-menu
            Ctx_mnuGroup.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the treMainData_MouseDown event", vApp:=ACApp, vClass:=ACClass, vMethod:="MouseDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub treMainData_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles treMainData.AfterSelect
        Dim Node As TreeNode = eventArgs.Node

        Dim sPMNavGroup As String = ""

        Dim iLevel As Integer
        Dim sParent, sText, sKey, iMsgRet, sKeyIDs As String

        Try

            'If root then do nothing
            If Convert.ToString(Node.Tag) = NavProcConst.ACRoot Or Convert.ToString(Node.Tag) = "-1" Then
                Exit Sub
            End If

            'Check if details have changed
            If m_iStatus = ACChanged Then
                iMsgRet = CStr(MessageBox.Show("The Details have changed. " & _
                          "Do you wish to discard your changes?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question))

                If iMsgRet = System.Windows.Forms.DialogResult.No Then

                    Exit Sub
                End If

            End If

            'Default To Edit
            m_iMode = gPMConstants.PMEComponentAction.PMEdit

            ' Get the details for the selected node
            m_lReturn = GetProcessMapEntry(CStr(m_cProcessMap.Item(Convert.ToString(Node.Tag))), sParent, sKey, iLevel, sText)

            'Get the ID for this node
            m_lID = CInt(Conversion.Val(sKey.Substring(9, Math.Min(sKey.Length, 99))))

            'Get the Parent ID

            m_lParentID = CInt(Conversion.Val(sParent.Substring(9, Math.Min(sParent.Length, 99))))

            'Get the Group

            Select Case iLevel
                Case NavProcConst.NodeProcess
                    sPMNavGroup = NavProcConst.NavGrpProcess

                    'Set the current process
                    m_lGroupID = m_lID

                    m_oBusiness.CurrentProcessID = m_lGroupID

                Case NavProcConst.NodeMap

                    sPMNavGroup = NavProcConst.NavGrpMap

                Case NavProcConst.NodeStepFindForm, NavProcConst.NodeStepDecisionForm, NavProcConst.NodeStepDataForm, NavProcConst.NodeStepBusinessObject, NavProcConst.NodeStepSubMap

                    sPMNavGroup = NavProcConst.NavGrpStep

                    'If this is a Step, then the MapID is included
                    'in the Key, so use as the ParentID
                    'sKey Format: 0000000MS[StepID];[MapID]
                    sKeyIDs = sKey.Substring(9, Math.Min(sKey.Length, 99))
                    m_lID = CInt(Conversion.Val(SubStr(sKeyIDs, 1, ";")))
                    m_lParentID = CInt(Conversion.Val(SubStr(sKeyIDs, 2, ";")))

                Case NavProcConst.NodeComponent

                    sPMNavGroup = NavProcConst.NavGrpComponent

                Case NavProcConst.NodeSubMap
                    sPMNavGroup = NavProcConst.NavGrpMap

            End Select

            'Set the Current GroupName
            If m_sPMNavGroup <> sPMNavGroup Then

                'Get the Group
                m_sPMNavGroup = sPMNavGroup

                'Display the correct interface
                m_lReturn = DisplayInterface(sPMNavGroup)
            End If

            'Get the Field data
            m_lReturn = GetPMNavFields(sPMNavGroup:=sPMNavGroup, lID:=m_lID, lParentID:=m_lParentID)

        Catch
        End Try




    End Sub
	
	Private Sub panSliderBar_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles panSliderBar.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		' Slider bar mouse down event.
		
		If Button = MouseButtonConstants.LeftButton And Not m_bSliderMoved Then
			m_bSliderMoved = True
			m_lSliderPosition = 0
			LinSliderBar.BackColor = SystemColors.GrayText
		End If
		
	End Sub
	
	Private Sub panSliderBar_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles panSliderBar.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		Static lx As Integer
		
		' Slider bar mouse move event.
		
		If m_bSliderMoved Then
			If Button > 0 Then
				If lx <> x Then
					If VB6.PixelsToTwipsX(panSliderBar.Left) + x > 1000 And VB6.PixelsToTwipsX(panSliderBar.Left) + x < VB6.PixelsToTwipsX(Me.Width) - 500 Then
						panSliderBar.Left += VB6.TwipsToPixelsX(x)
						lx = CInt(x)
						m_lSliderPosition = CInt(m_lSliderPosition + x)
					End If
				End If
			End If
		End If
		
	End Sub
	
	Private Sub panSliderBar_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles panSliderBar.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		' Slider bar mouse up event.
		
		If m_bSliderMoved Then
			m_bSliderMoved = False
			
			treMainData.Width += VB6.TwipsToPixelsX(m_lSliderPosition)
			m_Interface.Left += VB6.TwipsToPixelsX(m_lSliderPosition)
			m_Interface.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 180)
			
			cboGroups.Width = treMainData.Width - VB6.TwipsToPixelsX(15)
			
			cboGroupDescription.Left = m_Interface.Left + VB6.TwipsToPixelsX(15)
			cboGroupDescription.Width = m_Interface.Width - VB6.TwipsToPixelsX(30)
			
			LinSliderBar.BackColor = Me.BackColor
		End If
		
	End Sub
	
	Private Sub txtPMNav_Componentcaption_id_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Componentcaption_id.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Componentclass_name_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Componentclass_name.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Componentdescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Componentdescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		'Synchronise the caption
		txtPMNav_Componentcaption_id.Text = txtPMNav_Componentdescription.Text
		UpdateApply(ACChanged)
		
	End Sub
	
	
	Private Sub txtPMNav_Componenteffective_date_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Componenteffective_date.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Componentobject_name_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Componentobject_name.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Mapcaption_id_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Mapcaption_id.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Mapcode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Mapcode.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Strings.Len(txtPMNav_Mapcode.Text) > 10 Then
			txtPMNav_Mapcode.Text = txtPMNav_Mapcode.Text.Substring(0, 10)
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_MapDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_MapDescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		'Synchronise the caption
		txtPMNav_Mapcaption_id.Text = txtPMNav_MapDescription.Text
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Mapeffective_date_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Mapeffective_date.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Processcaption_id_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Processcaption_id.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Processcode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Processcode.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Strings.Len(txtPMNav_Processcode.Text) > 10 Then
			txtPMNav_Processcode.Text = txtPMNav_Processcode.Text.Substring(0, 10)
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Processdescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Processdescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		'Synchronise the caption
		txtPMNav_Processcaption_id.Text = txtPMNav_Processdescription.Text
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Processeffective_date_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Processeffective_date.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Stepcancel_no_of_steps_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Stepcancel_no_of_steps.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Stepcaption_id_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Stepcaption_id.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
	
	Private Sub txtPMNav_Stepdescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Stepdescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		'Synchronise the caption_id
		txtPMNav_Stepcaption_id.Text = txtPMNav_Stepdescription.Text
		
		UpdateApply(ACChanged)
		
	End Sub
	
	
	Private Sub txtPMNav_Stepok_no_of_steps_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMNav_Stepok_no_of_steps.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(ACChanged)
		
	End Sub
End Class
