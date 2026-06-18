Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
'Modified by Archana Tokas on 4/30/2010 9:36:52 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("InstanceManager_NET.InstanceManager")> _
Public Partial Class InstanceManager
	Inherits System.Windows.Forms.UserControl
	
	Private Const ACClass As String = "InstanceManager"
	
	Private Const SCREENOFFSET As Integer = 3000
	Private Const sglSplitLimit As Integer = 500
	
	Private m_lReturn As Integer
	Private m_lPolicyKey As Integer
	
	Private m_vObjectInfo( ,  ) As Object
	Private m_vObjArray() As Object
	Private m_iObjDepth As Integer
	Private m_lObjID As Integer
	Private m_sNewInstKey As String = ""
	Private m_bMoving As Boolean
	Private m_bNoInstances As Boolean
	Private m_oComponentManager As Object
	Private m_oListManager As Object
	
	Private m_lObjectID As Integer
	Private m_iInst(5) As Integer
	Private m_bForceParents As Boolean
	Private m_bForceInstance As Boolean
	Private m_iMaxInstance As Integer
	
	Private m_lEdit As Integer
	
	Private m_oNewItem As ListViewItem
	
	'Ram - 09/08/99
	'For storing the Array of Keys ( ListView Control )
	Public InstanceKeys() As Object
	'For storing the Array of Keys ( TreeView Control )
	Public NodeKeys() As Object
	
    Private m_vNodeABIText(,) As Object
	Private m_sSelectedNode As String = ""
	
	
	' Public Event
	Public Event ShowInstance(ByVal Sender As Object, ByVal e As ShowInstanceEventArgs)
	
	<Browsable(False)> _
	Public WriteOnly Property ComponentManager() As Object
		Set(ByVal Value As Object)
			
			m_oComponentManager = Value
			
			' Get the edit flag from componentmanager

			m_lEdit = m_oComponentManager.AllowEdit
			
		End Set
	End Property
	<Browsable(False)> _
	Public WriteOnly Property ListManager() As Object
		Set(ByVal Value As Object)
			m_oListManager = Value
		End Set
	End Property
	<Browsable(False)> _
	Public WriteOnly Property PolicyKey() As Integer
		Set(ByVal Value As Integer)
			m_lPolicyKey = Value
		End Set
	End Property
	
	
	
	
	<Browsable(False)> _
	Public ReadOnly Property SelectedNode() As String
		Get
			Return m_sSelectedNode
		End Get
	End Property
	
	Public Shadows Sub Update()
		If Not (tvTreeView.SelectedNode Is Nothing) Then
			tvTreeView_AfterSelect(tvTreeView, New TreeViewEventArgs(tvTreeView.SelectedNode))
		End If
	End Sub
	Private Sub AddNew()
		Dim iLevel, iInst As Integer
		Dim sKey As String = ""
		
		Try 
			
			sKey = m_sNewInstKey
			
			If sKey = "" Then
				Exit Sub
			End If
			
			iLevel = Conversion.Val(Mid(sKey, 2, 1))
			iInst = Conversion.Val(Mid(sKey, 6, 3))
			
			' Set the selected instance in the hierarchy
			If iLevel > 0 Then
				m_iInst(iLevel - 1) = iInst
			End If
			
			' If this is the lowest level required,
			' Then show the screen
			If iLevel = m_iObjDepth Then
				
				' Add a Dummy instance temporarily
				If m_oNewItem Is Nothing Then

					m_oNewItem = lvListView.Items.Add(CStr(iInst), "")
					ListViewHelper.GetListViewSubItem(m_oNewItem, 1).Text = "<new>"
					m_oNewItem.Selected = True
				End If
				
				RaiseEvent ShowInstance(Me, New ShowInstanceEventArgs(m_iInst(0), m_iInst(1), m_iInst(2), m_iInst(3), m_iInst(4), m_iInst(5)))
				
				'm_lReturn& = m_oScreenObject.setinstancekeys(m_iInst(0), m_iInst(1), m_iInst(2))
				'm_lReturn& = m_oScreenObject.ShowScreen(Left + SCREENOFFSET)
			Else
				RaiseEvent ShowInstance(Me, New ShowInstanceEventArgs(0, 0, 0, 0, 0, 0))
				
			End If
		
		Catch 
			
			
			' Error
			'MsgBox Err.Number & ": " & Err.Description, , "Edit"

            'Modified by Archana Tokas on 12/04/2010 05:22:57 refer developer guide no. 35
            'tvTreeView.SelectedNode.Selected = False
            tvTreeView.SelectedNode.Checked = False
		End Try
		
		
	End Sub
	
	
	Private Sub cmdAddNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddNew.Click
		
		AddNew()
		' Ram (30-09-1999 ) One line
		cmdDelete.Enabled = False
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		
		Dim sKey As String = lvListView.FocusedItem.Name
		
		'will remove selected item from listview control
		lvListView.Items.RemoveAt(lvListView.FocusedItem.Index)
		
		If sKey = "" Then
			Exit Sub
		End If
		
		Dim iLevel As Integer = Conversion.Val(Mid(sKey, 2, 1))
		Dim iInst As Integer = Conversion.Val(Mid(sKey, 6, 3))
		
		' Set the selected instance in the hierarchy
		If iLevel > 0 Then
			m_iInst(iLevel - 1) = iInst
		End If
		
		' If this is the lowest level required,
		' Then delete the object
		If iLevel = m_iObjDepth Then
			
			If System.Windows.Forms.DialogResult.Yes = MessageBox.Show("Are you sure you want to delete this instance", "Delete Instance", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) Then
				

				m_lReturn = m_oComponentManager.DeleteObjectInstance(m_lPolicyKey, m_lObjID, m_iInst(0), m_iInst(1), m_iInst(2), m_iInst(3), m_iInst(4), m_iInst(5))
				
				' Ram - 11/08/1999
				DeleteInstanceKey(sKey)
				
			End If
			
		End If
		
		Update()
		
	End Sub
	
	Private Sub Edit()
		
		Dim iLevel, iInst As Integer
		Dim sKey As String = ""
		
		Try 
			
			sKey = lvListView.FocusedItem.Name
			
			If sKey = "" Then
				Exit Sub
			End If
			
			iLevel = Conversion.Val(Mid(sKey, 2, 1))
			iInst = Conversion.Val(Mid(sKey, 6, 3))
			
			' Set the selected instance in the hierarchy
			If iLevel > 0 Then
				m_iInst(iLevel - 1) = iInst
			End If
			
			' If this is the lowest level required,
			' Then show the screen
			If iLevel = m_iObjDepth Then
				
				RaiseEvent ShowInstance(Me, New ShowInstanceEventArgs(m_iInst(0), m_iInst(1), m_iInst(2), m_iInst(3), m_iInst(4), m_iInst(5)))
			Else
				' Hide the data tab
				RaiseEvent ShowInstance(Me, New ShowInstanceEventArgs(0, 0, 0, 0, 0, 0))
				
			End If
		
		Catch 
			
			


            'Modified by Archana Tokas on 12/04/2010 05:24:40 refer developer guide no. 35
            'tvTreeView.SelectedNode.Selected = False
            tvTreeView.SelectedNode.Checked = False
		End Try
		
		
	End Sub
	
	Private Sub lvListView_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvListView.Enter
		
		ClearNewItem()
		
	End Sub
	
	Private Sub tvTreeView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvTreeView.Click
		' Ram - 05-10-1999 ( To expose the covers Text )
		Dim bFound As Boolean
		If Information.IsArray(m_vNodeABIText) Then

			For iCounter As Integer = m_vNodeABIText.GetLowerBound(1) To m_vNodeABIText.GetUpperBound(1) Step 1

                If tvTreeView.SelectedNode.Text = CStr(m_vNodeABIText(1, iCounter)) Then

                    m_sSelectedNode = CStr(m_vNodeABIText(0, iCounter))
                    bFound = True
                    Exit For
                End If
			Next 
		End If
		If Not bFound Then
			m_sSelectedNode = tvTreeView.SelectedNode.Text
		End If
		
	End Sub
	
	Private Sub tvTreeView_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvTreeView.Enter
		
		ClearNewItem()
		
	End Sub
	
	Private Sub InstanceManager_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

		Try 
			If VB6.PixelsToTwipsY(Height) < 3000 Then Height = VB6.TwipsToPixelsY(3000)
			If VB6.PixelsToTwipsX(Width) < 400 Then Width = VB6.TwipsToPixelsX(400)
			
			SizeControls(VB6.PixelsToTwipsY(imgSplitter.Top))
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Sub
	
	
	Private Sub imgSplitter_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitter.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		With imgSplitter
			picSplitter.SetBounds(.Left, .Top, .Width, .Height / 2)
		End With
		picSplitter.Visible = True
		m_bMoving = True
	End Sub
	
	Private Sub imgSplitter_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitter.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Dim sglPos As Single
		
		If m_bMoving Then
			sglPos = y + VB6.PixelsToTwipsY(imgSplitter.Top)
			If sglPos < sglSplitLimit Then
				picSplitter.Top = VB6.TwipsToPixelsY(sglSplitLimit)
			ElseIf sglPos > VB6.PixelsToTwipsY(Height) - sglSplitLimit Then 
				picSplitter.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Height) - sglSplitLimit)
			Else
				picSplitter.Top = VB6.TwipsToPixelsY(sglPos)
			End If
		End If
	End Sub
	
	
	Private Sub imgSplitter_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitter.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		SizeControls(VB6.PixelsToTwipsY(picSplitter.Top))
		picSplitter.Visible = False
		m_bMoving = False
	End Sub
	
	
	Sub SizeControls(ByRef y As Single)

		Try 
			
			'set the width
			If y < 1000 Then y = 1000
			If y > (VB6.PixelsToTwipsY(Height) - 1500 - VB6.PixelsToTwipsY(picButtons.Height)) Then y = VB6.PixelsToTwipsY(Height) - 1500 - VB6.PixelsToTwipsY(picButtons.Height)
			
			tvTreeView.Height = VB6.TwipsToPixelsY(y)
			imgSplitter.Top = VB6.TwipsToPixelsY(y) + tvTreeView.Top
			lvListView.Top = VB6.TwipsToPixelsY(y) + pnlHeader.Height + imgSplitter.Height + tvTreeView.Top
			lvListView.Height = ClientRectangle.Height - (tvTreeView.Height + imgSplitter.Height + tvTreeView.Top + picButtons.Height + pnlHeader.Height)
			
			pnlHeader.Top = VB6.TwipsToPixelsY(y) + imgSplitter.Height + tvTreeView.Top
			pnlHeader.Left = tvTreeView.Left
			lvListView.Left = tvTreeView.Left
			
			tvTreeView.Width = ClientRectangle.Width - tvTreeView.Left * 2
			lvListView.Width = tvTreeView.Width
			pnlHeader.Width = tvTreeView.Width
			
			imgSplitter.Left = tvTreeView.Left
			imgSplitter.Width = tvTreeView.Width
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	Public Function LoadHierarchy(ByRef lObjectID As Integer, Optional ByRef vOptional As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Dim lObject As Integer
		Dim iDepth As Integer
		Dim sObjectName As String = ""
        Dim vItemArray(,) As Object = Nothing
        Dim oNode As TreeNode
		Dim oItem As ListViewItem
		Dim iCount, iOptional As Integer
		
		Try 
			
			

			If Information.IsNothing(vOptional) Then
				iOptional = 0
			Else

				iOptional = CInt(vOptional)
			End If
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_oNewItem = Nothing
			
			m_lObjID = lObjectID
			
			' Get information about the object we are handling

			m_lReturn = m_oComponentManager.GetComponentInfo(m_lObjID, m_iObjDepth, m_vObjArray, m_iMaxInstance)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadHierarchy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadHierarchy")
			End If
			
			
			' Get top level instances
			iDepth = 1
			
			lObject = CInt(m_vObjArray(iDepth - 1))
			
			' Get the ID properies for the object
			'LoadObjectInfo lObject&
			
			' Add the top node into the tree
			oNode = tvTreeView.Nodes.Add("N0000000000", "", "closed")


            'Modified by Archana Tokas on 12/04/2010 05:27:00 refer developer guide no soluitions 16
            'oNode.ExpandedImage = "open"
            oNode.Expand()
			

			m_lReturn = GetObjectItems(lObject, sObjectName, vItemArray)
			
			If iDepth = 1 Then
				oNode.Text = sObjectName
			End If
			
			'Ram - 12/08/99  Resizing the NodeKeys( Three Lines )
			'Include the Root Node "N0000000000" as First Element
			ResizeNodeKeysArray(0, 0)
			NodeKeys(0) = "N0000000000"

            m_vNodeABIText = Nothing
			
			' Load the sub-nodes into the tree
			If iDepth < m_iObjDepth Then

				m_lReturn = LoadSubTree(oNode, lObject, iDepth, vItemArray)
			End If
			
			' Select the first item


            'Modified by Archana Tokas on 12/04/2010 05:27:32 refer developer guide no. 35
            'tvTreeView.Nodes.Item("N0000000000").Selected = True
            tvTreeView.Nodes.Item("N0000000000").Checked = True

			tvTreeView_AfterSelect(tvTreeView, New TreeViewEventArgs(tvTreeView.Nodes.Item("N0000000000")))
			
			' Select the last item
			iCount = tvTreeView.Nodes.Count
			oNode = tvTreeView.Nodes.Item(iCount - 1)
			


            'Modified by Archana Tokas on 12/04/2010 05:27:32 refer developer guide no. 35
            'oNode.Selected = True
            oNode.Checked = True


            tvTreeView_AfterSelect(tvTreeView, New TreeViewEventArgs(oNode))
			oNode = Nothing
			
			' Automatically add one, if appropriate (don't do this if screen only has optional questions)
			If (lvListView.Items.Count = 0) And (iOptional = 0) Then

				If m_oComponentManager.UseProfileArray = gPMConstants.PMEReturnCode.PMTrue Then
					If cmdAddNew.Enabled Then
						cmdAddNew_Click(cmdAddNew, New EventArgs())
					End If
				End If
				
				' If only one instance, automatically select it
			ElseIf (lvListView.Items.Count = 1) Then 

				If m_oComponentManager.UseProfileArray = gPMConstants.PMEReturnCode.PMTrue Then
					oItem = lvListView.Items.Item(0)
					oItem.Selected = True
                    'Modified by Archana Tokas on 12/04/2010 05:28:21 changes oItem should be there instead of lvListView as done in vb6 code
                    'lvListView_ItemClick((lvListView, New EventArgs())
                    lvListView_ItemClick(oItem)
					oItem = Nothing
				End If
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadHierarchy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadHierarchy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Function LoadSubTree(ByRef Parent_Renamed As TreeNode, ByRef lObject As Integer, ByRef iDepth As Integer, ByRef vItemArray( ,  ) As Object) As Integer
		
		Dim oChild As TreeNode
        Dim vInstArray(,) As Object = Nothing
        Dim sKey, sTag As String
        Dim sObjectName As String = String.Empty
        Dim iMax, iParentInst As Integer
        Dim vArray(,) As Object = Nothing
		Dim sObjName As String = ""
		Dim iInstNo As Integer
        Dim oPolCall As Object = Nothing
		
		Try 
			
			' Get the ID properies for the object
			'LoadObjectInfo lObject&
			
			iParentInst = CInt(Parent_Renamed.Name.Substring(5, Math.Min(Parent_Renamed.Name.Length, 3)))
			

			sTag = CStr(vItemArray(2, 0))
			

			m_lReturn = m_oComponentManager.GetInstanceArray(sTag, vInstArray, iMax, iParentInst, 0, 0, 0, 0)
			
			Dim sDump As String = ""
			Dim iPropertyType As Integer
			Dim lPropertyID As Integer
			Dim sDescription, sPropertyID As String
			If Information.IsArray(vInstArray) Then
				

                If Not Object.Equals(vInstArray(0, 0), Nothing) Then

                    ' Now add them into the tree

                    For iInstance As Integer = vInstArray.GetLowerBound(1) To vInstArray.GetUpperBound(1)

                        If Not Information.IsArray(m_vNodeABIText) Then
                            ReDim m_vNodeABIText(1, 0)
                        Else

                            ReDim Preserve m_vNodeABIText(1, m_vNodeABIText.GetUpperBound(1) + 1)
                        End If
                        '---

                        m_lReturn = m_oComponentManager.getpolcall(oPolCall)

                        If oPolCall.SaveAsABI Then
                            'for each property
                            'developer guide no.(Given full qualified name)
                            m_lReturn = SharedFiles.GeminiFunctions.ParseTag(sTag, iPropertyType, lPropertyID, sDump, sDump)
                            'if list
                            If iPropertyType = GEMPolShortList Or iPropertyType = GEMPolLongList Then

                                If CStr(vInstArray(1, iInstance)) <> "" Then
                                    'xlate abi into full description
                                    sPropertyID = CStr(lPropertyID)
                                    sDescription = ""


                                    m_lReturn = m_oListManager.GetDescription(sPropertyID:=sPropertyID, sABICodeTarget:=CStr(vInstArray(1, iInstance)), sDescription:=sDescription)

                                    m_vNodeABIText(0, m_vNodeABIText.GetUpperBound(1)) = CStr(vInstArray(1, iInstance))

                                    m_vNodeABIText(1, m_vNodeABIText.GetUpperBound(1)) = sDescription

                                    vInstArray(1, iInstance) = sDescription
                                End If
                            End If
                        End If
                        oPolCall = Nothing
                        '---




                        iInstNo = CInt(vInstArray(0, iInstance))
                        'sKey$ = "N" & CStr(iDepth%) & Format$(lObject&, "000") & Format$(iInstance% + 1, "000") & Format$(iParentInst%, "000")
                        sKey = NodeKey(iDepth, lObject, iInstNo, iParentInst)
                        oChild = tvTreeView.Nodes.Find(Parent_Renamed.Name, True)(0).Nodes.Add(sKey, vInstArray(1, iInstance), "closed", "open")


                        'Modified by Archana Tokas on 12/04/2010 05:30:29 refer developer guide no solution 16
                        'oChild.ExpandedImage = "open"
                        oChild.Expand()
                        oChild.EnsureVisible()

                        'Ram - 12/08/99
                        ' Resizing the NodeKeys
                        ResizeNodeKeysArray(NodeKeys.GetUpperBound(0) + 1, 1)

                        'Assigning the TreeView Node Keys to NodeKeys
                        NodeKeys(NodeKeys.GetUpperBound(0)) = sKey

                        ' If we need to go down another level in the tree,
                        ' do it now.
                        If iDepth < m_iObjDepth - 1 Then

                            m_lReturn = GetObjectItems(CInt(m_vObjArray(iDepth)), sObjectName, vArray)

                            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                                m_bNoInstances = True
                            End If


                            m_lReturn = LoadSubTree(oChild, CInt(m_vObjArray(iDepth)), iDepth + 1, vArray)
                        End If

                    Next iInstance
                End If
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSubTree Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSubTree", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Function
	
	Private Sub ShowInstances(ByRef sParentNode As String)
		
        Dim vData(,) As Object = Nothing
        Dim vTitles() As Object
        Dim vItemArray(,) As Object = Nothing
        Dim sObjectName As String = String.Empty
        Dim iCols, iMax As Integer
		Dim vPropertyData As Object
        Dim oPolCall As Object = Nothing
        Dim sDescription As String = String.Empty
        Dim sProperty As String
		Dim iType As Integer
		Dim vPtr As String = ""
		
		vData = Nothing
		vTitles = Nothing
		vPropertyData = Nothing
		
		m_oNewItem = Nothing
		
		' Get the level and instance from the key
		Dim iLevel As Integer = Conversion.Val(Mid(sParentNode, 2, 1))
		Dim iParentInst As Integer = Conversion.Val(Mid(sParentNode, 6, 3))
		
		' Get the object number from the object arrray
		Dim lObject As Integer = CInt(m_vObjArray(iLevel))
		
		' Get the ID properies for the object
		'LoadObjectInfo lObject&
		
		' Get the display items for this object

		m_lReturn = GetObjectItems(lObject, sObjectName, vItemArray)
		
		If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
			m_bNoInstances = True
		End If
		
		' Set the object name above the listview


        'Modified by Archana Tokas on 12/04/2010 05:31:20 changes refer developer guide no. 51
        'pnlHeader.Caption = "  " & sObjectName
        pnlHeader.Name = "  " & sObjectName
		
		' Get the number of items (columns)
		' to be displayed

        If Object.Equals(vItemArray(0, 0), Nothing) Then
            iCols = 0
        Else

            iCols = vItemArray.GetUpperBound(1) + 1
        End If
		
		' Resize the arrays to hold the instance data
		ReDim vTitles(iCols)
		ReDim vData(iCols, 0)
		
		' Set up headings and put tags into array

        vTitles(0) = "Item"
		Dim vTag(iCols - 1) As Object
		
		For iCol As Integer = 1 To iCols
			
			' Put heading into vTitles array


            vTitles(iCol) = vItemArray(1, iCol - 1)
			
			' Put Tag into Property Array


            vTag(iCol - 1) = vItemArray(2, iCol - 1)
			
		Next 
		
		' Get the data from polaris

		m_lReturn = m_oComponentManager.GetInstanceArray(vTag, vData, iMax, m_iInst(0), m_iInst(1), m_iInst(2), m_iInst(3), m_iInst(4))
		
		'(IB)300799 - use list management if need to
		Dim iInst(5) As Integer
		
		If m_oListManager Is Nothing Then
			'if not passed ref to list manager then do nothing
		Else
			'otherwise use listmanagement to translate the description

			m_lReturn = m_oComponentManager.getpolcall(oPolCall)
			For iInstances As Integer = 0 To 4
				iInst(iInstances) = m_iInst(iInstances)
			Next iInstances
			

			For iInstances As Integer = vData.GetLowerBound(1) To vData.GetUpperBound(1)
				
				' Ram - 13-09-1999  ( One line is changed to other line )
				'iInst(iLevel) = iInstances% + 1

                iInst(iLevel) = CInt(vData(0, iInstances))
				
				' Ram - 22-09-1999  ( First Column )
				

				sProperty = CStr(vTag(0)).Substring(1).Trim()

				iType = Conversion.Val(CStr(vTag(0)).Substring(0, 1))
				If iType = GEMPolShortList Or iType = GEMPolLongList Then

                    m_lReturn = oPolCall(Conversion.Val(sProperty), iType, vPtr, iInst(0), iInst(1), iInst(2), iInst(3), iInst(4))
					If vPtr <> "" Then

						m_lReturn = m_oListManager.GetDescription(sPropertyID:=sProperty, sABICodeTarget:=vPtr, sDescription:=sDescription)

                        vData(1, iInstances) = sDescription
					End If
				End If
				
				' Ram - 22-09-1999 ( Second Column of List View Control)
				If vTag.GetUpperBound(0) = 1 Then

					sProperty = CStr(vTag(1)).Substring(1).Trim()

					iType = Conversion.Val(CStr(vTag(1)).Substring(0, 1))
					If iType = GEMPolShortList Or iType = GEMPolLongList Then

                        m_lReturn = oPolCall(Conversion.Val(sProperty), iType, vPtr, iInst(0), iInst(1), iInst(2), iInst(3), iInst(4))
						If vPtr <> "" Then

							m_lReturn = m_oListManager.GetDescription(sPropertyID:=sProperty, sABICodeTarget:=vPtr, sDescription:=sDescription)

                            vData(2, iInstances) = sDescription
						End If
					End If
				End If
				
			Next iInstances
			oPolCall = Nothing
		End If
		
		' Ram - 19-Aug-1999   /*  Start */
		' For updating the Detailed description of the Tree View Control Nodes.
		' The vdata holds the values for these. Merge two columns strings to one string.
        Dim vtvNodeText() As Object
		Dim sTemp As String = ""
		

		If vData.GetUpperBound(0) = 2 Then

			ReDim vtvNodeText(vData.GetUpperBound(1))

			For iLoopCounter As Integer = vData.GetLowerBound(1) To vData.GetUpperBound(1)


                vtvNodeText(iLoopCounter) = CStr(vData(2, iLoopCounter)).Trim()
			Next iLoopCounter
			

			For iLoopCounter As Integer = vtvNodeText.GetLowerBound(0) To vtvNodeText.GetUpperBound(0)
				For iNodeCounter As Integer = 1 To tvTreeView.Nodes.Count Step 1

                    If tvTreeView.Nodes.Item(iNodeCounter - 1).Text.Trim() = CStr(vData(1, iLoopCounter)).Trim() Then
                        ' if the Second column has descriptions

                        If CStr(vData(2, iLoopCounter)).Trim().Length > 0 Then

                            tvTreeView.Nodes.Item(iNodeCounter - 1).Text = CStr(vData(1, iLoopCounter)).Trim() & _
                                                                           " - " & _
                                                                           CStr(vData(2, iLoopCounter)).Trim()
                        Else

                            tvTreeView.Nodes.Item(iNodeCounter - 1).Text = CStr(vData(1, iLoopCounter)).Trim()
                        End If

                        ' Ram - 30-09-1999 ( If the data contains Double Apostopies then get rid of it 5 Lines)
                        sTemp = tvTreeView.Nodes.Item(iNodeCounter - 1).Text
                        If (sTemp.IndexOf("''") + 1) > 1 Then
                            RemoveAppostophe(sTemp)
                        End If
                        tvTreeView.Nodes.Item(iNodeCounter - 1).Text = sTemp

                        Exit For
                    End If
				Next iNodeCounter
			Next iLoopCounter
		End If
		' Ram - 19-Aug-1999   /*  END */
		


		FillListView(lObject, iLevel, iCols, vTitles, vData)
		
	End Sub
	
	Private Sub FillListView(ByRef lObj As Integer, ByRef iDepth As Integer, ByRef iCols As Integer, ByRef vTitles() As Object, ByRef vData( ,  ) As Object)
		
		Dim oItem As ListViewItem
		Dim oCH As ColumnHeader
        Dim sKey As String
        Dim sNewInstKey As String = String.Empty
		Dim iColWidth As Integer
		Dim bDepth As Boolean
		
		bDepth = (iDepth = m_iObjDepth - 1)
		
		Dim iWidth As Integer = 3000 - 250
		
		If iCols > 0 Then
			iColWidth = iWidth / iCols
		Else
			iColWidth = iWidth
		End If
		
		Dim sTemp As String = ""
		With lvListView
			
			' Clear data out
			.Columns.Clear()
			.Items.Clear()
			
			For iCol As Integer = 0 To iCols

				oCH = .Columns.Add(CStr(vTitles(iCol)), 94)
				If iCol = 0 Then
					oCH.Width = CInt(VB6.TwipsToPixelsX(250))
				Else
					oCH.Width = CInt(VB6.TwipsToPixelsX(iColWidth))
				End If
			Next 
			
			' Ram - 09/08/1999 - Resizing the Array ( One line )
			ResizeInstanceKeysArray(vData.GetUpperBound(1), 0)
			

			If Not Object.Equals(vData(0, 0), Nothing) Then
				For iRow As Integer = 0 To vData.GetUpperBound(1)
					For iCol As Integer = 0 To iCols

						sTemp = CStr(vData(iCol, iRow))
						
						If iCol = 0 Then

							sKey = "L" & iDepth + 1 & StringsHelper.Format(lObj, "000") & StringsHelper.Format(vData(iCol, iRow), "000")
							
							' Ram - 09-08-99  Assigning the Instances Keys (One Line)
							InstanceKeys(iRow) = sKey
							
							If bDepth Then

								oItem = .Items.Add(sKey, sTemp, "")
							Else

								oItem = .Items.Add(sKey, "  ", "")
							End If
						Else
							' Ram - 30-09-1999 ( If the data contains Double Apostopies then get rid of it)
							If (sTemp.IndexOf("''") + 1) > 1 Then
								RemoveAppostophe(sTemp)
							End If
							
							ListViewHelper.GetListViewSubItem(oItem, iCol).Text = sTemp
						End If
					Next 
				Next 
			End If
			
		End With
		
		cmdAddNew.Enabled = False
		'cmdEdit.Enabled = False
		cmdDelete.Enabled = False
		m_sNewInstKey = ""
		
		' If this is the required level, create a key for a New Instance
		' IF the maximum number of instances has not been exceeded
		If bDepth Then
			
			If (m_iMaxInstance = 0) Or (lvListView.Items.Count < m_iMaxInstance) Then
				m_lReturn = GetNewInstKey(iDepth, lObj, sNewInstKey)
				
				If m_lEdit = gPMConstants.PMEReturnCode.PMTrue Then
					cmdAddNew.Enabled = True
				End If
				
				m_sNewInstKey = sNewInstKey
			End If
			
		End If
		
	End Sub
	
	
	Private Sub lvListView_ItemClick(ByVal Item As ListViewItem)
		
		Dim iLevel, iInst As Integer
        Dim sKey As String

		
		'Ram - 09-08-99
		'To Avoid the Item_Click Event triggering Twice.
		'Since it's a bug in Microsoft List View control
		'If the selected item is the one and only item then we
		'have to take care.
		
		Static sPreviousKey As String = ""
		
		If lvListView.FocusedItem.Index + 1 = 1 Or sPreviousKey <> Item.Name Then
			' Assigning for History
			sPreviousKey = Item.Name
			
			Edit()
			sKey = Item.Name
			
			' CL 8/98 - This commented out to avoid strange bug when clicking
			'           on a newly added node (REF 795)
			'BEGIN
			'        If sKey$ = "" Then
			'            ShowInstances ""
			'            Exit Sub
			'        End If
			'END
			
			iLevel = Conversion.Val(Mid(sKey, 2, 1))
			iInst = Conversion.Val(Mid(sKey, 6, 3))
			
			' Set the selected instance in the hierarchy
			If iLevel > 0 Then
				m_iInst(iLevel - 1) = iInst
			End If
			
			If (iLevel = m_iObjDepth) And (m_lEdit = gPMConstants.PMEReturnCode.PMTrue) Then
				'cmdEdit.Enabled = True
				cmdDelete.Enabled = True
			Else
				'cmdEdit.Enabled = False
				cmdDelete.Enabled = False
				
				'        ' Get the corresponding node in the treeview
				'        sNodeKey$ = sKey$
				'        Mid$(sNodeKey, 1, 1) = "N"
				'        sNodeKey$ = sNodeKey$ & String(10 - Len(sNodeKey$), "0") & CStr(m_iInst(1))
				'
				'        On Error Resume Next
				'        Set oNode = tvTreeView.Nodes(sNodeKey$)
				'
				'        oNode.Selected = True
				'        Call tvTreeView_NodeClick(oNode)
				'
				'        Set oNode = Nothing
				
			End If
			
			' if sPreviousKey$ <> Item.Key     Ram - 09/08/1999
		End If
		
	End Sub
	
	Private Sub tvTreeView_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvTreeView.AfterSelect
		Dim Node As TreeNode = eventArgs.Node
		
		Dim iLevel, iInst As Integer
		Dim sKey, sParentKey As String
		Dim iParentLevel, iParentInst As Integer
		
		Try 
			
			'If m_bForceParents Then
			'    Call SetParent
			'    Set Node = Nothing
			'    Exit Sub
			'End If
			
			sKey = Node.Name
			
			If sKey = "" Then
				ShowInstances("")
				Exit Sub
			End If
			
			iLevel = Conversion.Val(Mid(sKey, 2, 1))
			iInst = Conversion.Val(Mid(sKey, 6, 3))
			
			' Set the selected instance in the hierarchy
			If iLevel > 0 Then
				m_iInst(iLevel - 1) = iInst
			End If
			
			' If lower than top level, get parent instance
			If iLevel > 1 Then
				sParentKey = Node.Parent.Name
				iParentLevel = Conversion.Val(Mid(sParentKey, 2, 1))
				iParentInst = Conversion.Val(Mid(sParentKey, 6, 3))
				
				' Set the selected instance in the hierarchy
				If iParentLevel > 0 Then
					m_iInst(iParentLevel - 1) = iParentInst
				End If
			End If
			
			ShowInstances(sKey)
			
			' Hide the data tab
			RaiseEvent ShowInstance(Me, New ShowInstanceEventArgs(0, 0, 0, 0, 0, 0))
		
		Catch 
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' Get the first Unused Polaris Instance Number for a new row.
	Private Function GetNewInstKey(ByRef iDepth As Integer, ByRef lObject As Integer, ByRef sInstKey As String) As Integer
		
		Dim result As Integer = 0
		Dim iNewInst, iInst As Integer
		Dim bUsed As Boolean
		
		Try 
			
			' Default return Value
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iNewInst = 1
			
			' Loop until free instance found,
			' or Max instance value reached
			Do While iNewInst <= m_iMaxInstance
				
				bUsed = False
				
				' Loop through items in list view
				For	Each oItem As ListViewItem In lvListView.Items
					
					' Get the instance of the item from the key
					iInst = Conversion.Val(oItem.Name.Substring(5, Math.Min(oItem.Name.Length, 3)))
					
					' If this matches, this instance is used, so skip to the next
					If iInst = iNewInst Then
						bUsed = True
						Exit For
					End If
					
				Next oItem
				
				' If not used, this one will do
				If Not bUsed Then
					sInstKey = "L" & iDepth + 1 & StringsHelper.Format(lObject, "000") & StringsHelper.Format(iNewInst, "000")
					result = gPMConstants.PMEReturnCode.PMTrue
					Exit Do
				End If
				
				iNewInst += 1
				
			Loop 
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in GetNewInstKey", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewInstKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	
	Private Function NodeKey(ByRef iDepth As Integer, ByRef lObject As Integer, ByRef iInstance As Integer, Optional ByRef iParentInst As Integer = 0) As String
		
		Return "N" & iDepth & StringsHelper.Format(lObject, "000") & StringsHelper.Format(iInstance, "000") & StringsHelper.Format(iParentInst, "000")
		
	End Function
	
	
	Public Function GetObjectItems(ByRef lObjID As Integer, ByRef sName As String, ByRef vItemArray( ,  ) As Object) As Integer
		
		Dim result As Integer = 0
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		vItemArray = Nothing
		ReDim vItemArray(2, 0)
		
		Dim bFound As Boolean = False
		Dim bLoaded As Boolean = False
		
		
Start: 
		Dim lItem As Integer = 0
		sName = ""
		
		If Not (Not Information.IsArray(m_vObjectInfo)) Then
			
			For lRecord As Integer = m_vObjectInfo.GetLowerBound(1) To m_vObjectInfo.GetUpperBound(1)
				
				If CDbl(m_vObjectInfo(0, lRecord)) = lObjID Then
					bFound = True
					
					If sName = "" Then
						sName = CStr(m_vObjectInfo(1, lRecord))
					End If
					
					ReDim Preserve vItemArray(2, lItem)
					

					vItemArray(0, lItem) = m_vObjectInfo(2, lRecord)

					vItemArray(1, lItem) = m_vObjectInfo(3, lRecord)

					vItemArray(2, lItem) = m_vObjectInfo(4, lRecord)
					
					lItem += 1
					
				ElseIf bFound Then 
					Exit For
				End If
				
			Next lRecord
			
			
		End If
		
		If Not bFound Then
			
			If Not bLoaded Then
				LoadObjectInfo(lObjID)
				bLoaded = True
				GoTo Start
				
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
		End If
		
		Return result
	End Function
	
	' Ad ID Properties for given object to array
	Public Sub LoadObjectInfo(ByRef lObjectID As Integer)
		
        Dim vItemArray(,) As Object = Nothing
		Dim lReturn As Integer
		Dim sName As String = ""

        Try

            'If GetObjectItems(lObjectID, sName, vArray) = PMTrue Then
            '    Exit Sub
            'End If


            lReturn = m_oComponentManager.GetIDProperties(lObjectID, vItemArray)

            If Not Information.IsArray(vItemArray) Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="There are no Identifying Properties set up for Object " & lObjectID, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadObjectInfo")

                Exit Sub

            End If



            For lRow As Integer = vItemArray.GetLowerBound(1) To vItemArray.GetUpperBound(1)





                LoadInfoLine(CStr(lObjectID), CStr(vItemArray(0, lRow)), CStr(Conversion.Val(CStr(vItemArray(1, lRow)))), CStr(vItemArray(2, lRow)), CStr(vItemArray(4, lRow)) & CStr(vItemArray(3, lRow)) & "    ")
            Next

        Catch excep As System.Exception



            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Error Loading Object Info for " & lObjectID, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadObjectInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        End Try
		
	End Sub
	
	' Procedure to update the Information for the Objects one by one
	Public Sub LoadInfoLine(ByRef vObject As String, ByRef vObjDesc As String, ByRef vIndex As String, ByRef vPropDesc As String, ByRef vTag As String)
		
		Static lRow As Integer
		
		If lRow = 0 Then
			ReDim m_vObjectInfo(4, lRow)
		Else
			ReDim Preserve m_vObjectInfo(4, lRow)
		End If
		
		m_vObjectInfo(0, lRow) = vObject
		m_vObjectInfo(1, lRow) = vObjDesc
		m_vObjectInfo(2, lRow) = vIndex
		m_vObjectInfo(3, lRow) = vPropDesc
		m_vObjectInfo(4, lRow) = vTag
		
		lRow += 1
		
	End Sub
	
	Private Sub UserControl_Terminate()
		
		m_oNewItem = Nothing
		
	End Sub
	
	Private Sub ClearNewItem()
		
		If Not (m_oNewItem Is Nothing) Then
			
			lvListView.Items.RemoveAt(m_oNewItem.Index)
			m_oNewItem = Nothing
			
			' Clear the data screen
			RaiseEvent ShowInstance(Me, New ShowInstanceEventArgs(0, 0, 0, 0, 0, 0))
			
		End If
		
	End Sub
	
	' Created by    :   Ram Chandrabose
	' Date          :   09-Aug-1999
	' Procedure for :   Activating the List View Node
	' Input Param   :   sKey ( Node Key )
	Public Sub ActivateNode(ByRef sKey As String)
		
		For	Each oItem As ListViewItem In lvListView.Items
			If oItem.Name = sKey Then
				lvListView.FocusedItem = oItem

                'Modified by Archana Tokas on 12/04/2010 05:31:47 changes oItem should be tere instead of lvListView as given in vb6 code
                'lvListView_ItemClick(lvListView, New EventArgs())
                lvListView_ItemClick(oItem)
				Exit For
			End If
		Next oItem
		
	End Sub
	
	' Created by    :   Ram Chandrabose
	' Date          :   09-Aug-1999
	' Procedure for :   Activating the TreeView Node
	' Input Param   :   sKey ( Node Key )
	Public Sub ActivateTreeNode(ByRef sKey As String)
		Dim oNode As TreeNode
		
		If sKey <> "" Then
			oNode = tvTreeView.Nodes.Item(sKey)

            'Modified by Archana Tokas on 12/04/2010 05:33:57 CHANGES refer developer guide no. 35
            'oNode.Selected = True
            oNode.Checked = True
			tvTreeView_AfterSelect(tvTreeView, New TreeViewEventArgs(oNode))
		End If
		
	End Sub
	
	' Created by    :   Ram Chandrabose
	' Date          :   11-Aug-1999
	' Procedure for :   Deleting the InstanceKey from the Array
	' Input Param   :   sInstanceKey ( Insatance Key )
	Private Sub DeleteInstanceKey(ByRef sInstanceKey As String)
		
		
		' Initialising
		Dim iPosition As Integer = -1
		Dim iNoofKeys As Integer = InstanceKeys.GetUpperBound(0)
		
		If Information.IsArray(InstanceKeys) Then
			' Locating where is the key
			For iCounter As Integer = 0 To iNoofKeys Step 1
				If CStr(InstanceKeys(iCounter)) = sInstanceKey Then
					' Capturing the location
					iPosition = iCounter
					' Deleting the Key in that location
					InstanceKeys(iPosition) = ""
					Exit For
				End If
			Next iCounter
			
			' If found then
			If iPosition > -1 Then
				' If the array size is more than one
				If iNoofKeys > 0 Then
					' if is not the last element
					If iPosition < iNoofKeys Then
						For iCounter As Integer = iPosition To iNoofKeys - 1 Step 1
							' Moving the keys one up
							InstanceKeys(iCounter) = InstanceKeys(iCounter + 1)
						Next iCounter
					End If
					' Resize the array to hold latest values
					ResizeInstanceKeysArray(iNoofKeys - 1, 1)
				End If
			End If
		End If
	End Sub
	
	' Created by    :   Ram Chandrabose
	' Date          :   11-Aug-1999
	' Procedure for :   Resizing the Insatance Keys Array
	' Input Param 1 :   iArraySize  ( Array Size )
	' Input Param 2 :   iOption     ( 0 - No   Preserve )
	'                               ( 1 - With Preserve )
	Private Sub ResizeInstanceKeysArray(ByRef iArraySize As Integer, ByRef iOption As Integer)
		' Since we have to redim only
		' when there is atleast one Instance
		If iArraySize >= 0 Then
			If iOption = 0 Then
				' Zero Base ie. 3 Instances means 0 to 2
				ReDim InstanceKeys(iArraySize)
			ElseIf iOption = 1 Then 
				ReDim Preserve InstanceKeys(iArraySize)
			End If
		End If
	End Sub
	' Created by    :   Ram Chandrabose
	' Date          :   12-Aug-1999
	' Procedure for :   Resizing the Node Keys Array
	' Input Param 1 :   iArraySize  ( Array Size )
	' Input Param 2 :   iOption     ( 0 - No   Preserve )
	'                               ( 1 - With Preserve )
	' Note          :   The array's First Element is always "N0000000000"
	Private Sub ResizeNodeKeysArray(ByRef iArraySize As Integer, ByRef iOption As Integer)
		' Since we have to redim only
		' when there is atleast one Instance
		If iArraySize >= 0 Then
			If iOption = 0 Then
				' Zero Base ie. 3 Instances means 0 to 2
				ReDim NodeKeys(iArraySize)
			ElseIf iOption = 1 Then 
				ReDim Preserve NodeKeys(iArraySize)
			End If
		End If
	End Sub
	
	' Created by    :   Ram Chandrabose
	' Date          :   30-Sep-1999
	' Procedure for :   Removing the Multiple Appostophes in the string
	' Input Param   :   sTempStr  ( String )
	' Note          :   Removes the multiple Appostophes present in a string
	
	Private Sub RemoveAppostophe(ByRef sTempStr As String)
		
		Dim sChar1, sChar2 As String
		
		Dim sTempString As New StringBuilder
		
		sTempStr = sTempStr.Trim()
		
		Dim iLen As Integer = sTempStr.Length
		For iCount As Integer = 1 To iLen Step 1
			
			sChar1 = sTempStr.Substring(iCount - 1, 1)
			
			If iCount < iLen Then
				sChar2 = sTempStr.Substring(iCount, 1)
			Else
				sChar2 = ""
			End If
			
			If sChar1 = "'" And sChar2 = "'" Then
			Else
				sTempString.Append(sChar1)
			End If
			
		Next iCount
		
		sTempStr = sTempString.ToString()
	End Sub
End Class
