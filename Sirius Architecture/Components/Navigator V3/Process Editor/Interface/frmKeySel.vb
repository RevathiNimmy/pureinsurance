Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmKeySel
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmKeySel
	'
	' Date: {TodaysDate}
	'
	' Description: GetSet Keys Interface
	'
	' Edit History:
	' ***************************************************************** '
	
    'developer guide no. 50
    Dim objfrmLog As New frmLog
	Private Const ACClass As String = "frmKeySel"
	
	Private m_lReturn As Integer
	
	'Business Object
	Private m_oBusiness As Object
	
	'The Mode of Add/Edit
	Private m_iMode As gPMConstants.PMEComponentAction
	'Apply Details Status
	Private m_iStatus As gPMConstants.PMEReturnCode
	
	'The Navigator Group
	Private m_sPMNavGroup As String = ""
	
	'The current selected PMNavKey
	Private m_oCurrentKey As PMNavKey
	
	'The Get/Set Key Details
	Private m_lID As Integer
	Private m_lMapID As Integer 'Used with Step
	Private m_sDescription As String = "" 'Description of Item
	
	'List of Get/Set keys
	Private m_cGetKeys As Collection
	Private m_cSetKeys As Collection
	
	'Current selected PMNavKeyGSK
	Private m_oCurrentGSK As PMNavKeyGSK
	
	'Rule Enforcement
	Private m_iEnforceRules As CheckState
	
	Public Sub ReadGetSetKeys(ByRef cGetKeys As Collection, ByRef cSetkeys As Collection)
		
		cGetKeys = m_cGetKeys
		cSetkeys = m_cSetKeys
		
	End Sub
	
	
	
	
	Private Sub AddKey(ByRef sGSKType As String)
		Dim Err_NewKey As Boolean = False
		Dim Err_AddKey As Boolean = False
		
		Dim lPMNavKeyID As Integer
		Dim okey As PMNavKeyGSK
		Dim lstGSKList As ListBox
		Dim cGSKCollection As Collection
		Dim bChanged As Boolean
		
		Try 
			Err_AddKey = True
			Err_NewKey = False
			
			'Check if we have a navigator item
			If m_sDescription = "" Then
				Exit Sub
			End If
			
			'Get the correct list control
			If sGSKType = NavProcConst.ACSetKey Then
				lstGSKList = lstSKRequiredKeys
				cGSKCollection = m_cSetKeys
			Else
				lstGSKList = lstGKRequiredKeys
				cGSKCollection = m_cGetKeys
			End If
			
			For iListIndex As Integer = 0 To lstSystemKeys.Items.Count - 1
				
				If ListBoxHelper.GetSelected(lstSystemKeys, iListIndex) Then
					
					'Get the selected key
					lPMNavKeyID = VB6.GetItemData(lstSystemKeys, iListIndex)
					
					'Check if we already have this key in the getset collection
					Err_NewKey = True
					Err_AddKey = False
					
					okey = cGSKCollection.Item(CStr(lPMNavKeyID))
					
					'Set the description of this key

                    okey.Description = g_cSystemKeys.Item(CStr(lPMNavKeyID)).Description

                    'Set the deleted flag to false
                    okey.IsDeleted = False

                    Err_AddKey = True
                    Err_NewKey = False

                    bChanged = True

                End If

            Next iListIndex

            'Do nothing if nothing changed
            If Not (bChanged) Then
                Exit Sub
            End If


            'Redisplay the list
            DisplayGSKList(sGSKType)

            'Update the apply
            UpdateApplyGSK(True)

        Catch excep As System.Exception
            If Not Err_NewKey And Not Err_AddKey Then
                Throw excep
            End If

            If Err_NewKey Then


                'If the gsk is not in the collection then create a new one
                okey = New PMNavKeyGSK()
                okey.PMNavKeyID = lPMNavKeyID
                okey.HasChanged = True
                okey.IsNew = True
                cGSKCollection.Add(okey, CStr(lPMNavKeyID))




            End If
            If Err_AddKey Or Err_NewKey Then


                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End If
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayGSKList
    '
    ' Description: Display the list of Get/Set Keys
    '
    ' ***************************************************************** '
    Private Sub DisplayGSKList(ByRef sGSKType As String)

        Dim sPMNavKeyID As String = ""
        Dim lstGSKList As ListBox
        Dim cGSKCollection As Collection

        'Get the reauired list
        If sGSKType = NavProcConst.ACSetKey Then
            lstGSKList = lstSKRequiredKeys
            cGSKCollection = m_cSetKeys
        Else
            lstGSKList = lstGKRequiredKeys
            cGSKCollection = m_cGetKeys
        End If

        'Clear the list
        lstGSKList.Items.Clear()

        'Clear the Get/Set Details Fields
        ClearGSKFields()

        If cGSKCollection Is Nothing Then
            Exit Sub
        End If

        'Pointer used for itemdata
        Dim iGSKPtr As Integer = 0

        'Display get/set keys

        For iPtr As Integer = 1 To cGSKCollection.Count

            'Only display if not flagged as deleted

            If Not cGSKCollection.Item(iPtr).IsDeleted Then

                'Get the ID of this Key

                sPMNavKeyID = cGSKCollection.Item(iPtr).PMNavKeyID

                'Add the description

                lstGSKList.Items.Add(g_cSystemKeys.Item(sPMNavKeyID).Description)

                'Add the key

                VB6.SetItemData(lstGSKList, iGSKPtr, g_cSystemKeys.Item(sPMNavKeyID).PMNavKeyID)

                iGSKPtr += 1

            End If

        Next iPtr

        'Defocus
        ListBoxHelper.SetSelectedIndex(lstGSKList, -1)

    End Sub

    Private Sub DisplayPMTypesList()

        'Add the Datatypes

        cboDataType.Items.Add("Binary")
        cboDataType.Items.Add("Boolean")
        cboDataType.Items.Add("Currency")
        cboDataType.Items.Add("Date")
        cboDataType.Items.Add("Decimal")
        cboDataType.Items.Add("Double")
        cboDataType.Items.Add("Integer")
        cboDataType.Items.Add("Long")
        cboDataType.Items.Add("Lookup")
        cboDataType.Items.Add("Long")
        cboDataType.Items.Add("String")

        VB6.SetItemData(cboDataType, 0, gPMConstants.PMEDataType.PMBinary)
        VB6.SetItemData(cboDataType, 1, gPMConstants.PMEDataType.PMBoolean)
        VB6.SetItemData(cboDataType, 2, gPMConstants.PMEDataType.PMCurrency)
        VB6.SetItemData(cboDataType, 3, gPMConstants.PMEDataType.PMDate)
        VB6.SetItemData(cboDataType, 4, gPMConstants.PMEDataType.PMDecimal)
        VB6.SetItemData(cboDataType, 5, gPMConstants.PMEDataType.PMDouble)
        VB6.SetItemData(cboDataType, 6, gPMConstants.PMEDataType.PMInteger)
        VB6.SetItemData(cboDataType, 7, gPMConstants.PMEDataType.PMLong)
        VB6.SetItemData(cboDataType, 8, gPMConstants.PMEDataType.PMLookup)
        VB6.SetItemData(cboDataType, 9, gPMConstants.PMEDataType.PMLong)
        VB6.SetItemData(cboDataType, 10, gPMConstants.PMEDataType.PMString)

    End Sub

    ' ***************************************************************** '
    ' Name: EnforceGSKRules
    '
    ' Description: Enforce rules for Get and Set Keys according to Group
    '
    ' ***************************************************************** '
    Private Function EnforceGSKRules() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sTitle = "Enforce Rules"


            m_lReturn = m_oBusiness.EnforceGSKRules(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'If Rule enforcement failed then notify user
                MessageBox.Show("Rules could not be enforced.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else


                sMessage = m_oBusiness.RuleLog

                If sMessage <> "" Then

                    'Show rule log
                    'developer guide no. 50
                    objfrmLog.Initialise(sMessage)

                    'developer guide no. 50
                    objfrmLog.Close()
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnforceGSKRules Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnforceGSKRules", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub RemoveKey(ByRef sGSKType As String)

        Dim lstGSKList As ListBox
        Dim cGSKCollection As Collection
        Dim lPMNavKeyID As Integer
        Dim bChanged As Boolean

        Try

            'Get the correct list control
            If sGSKType = NavProcConst.ACSetKey Then
                lstGSKList = lstSKRequiredKeys
                cGSKCollection = m_cSetKeys
            Else
                lstGSKList = lstGKRequiredKeys
                cGSKCollection = m_cGetKeys
            End If

            'Check for all selected keys
            For iListIndex As Integer = 0 To lstGSKList.Items.Count - 1

                If ListBoxHelper.GetSelected(lstGSKList, iListIndex) Then

                    'Get the selected key
                    lPMNavKeyID = VB6.GetItemData(lstGSKList, iListIndex)

                    'Set the Key to deleted

                    cGSKCollection.Item(CStr(lPMNavKeyID)).IsDeleted = True

                    bChanged = True

                End If

            Next iListIndex

            'Do nothing if nothing changed
            If Not (bChanged) Then
                Exit Sub
            End If

            'Display the updated list
            DisplayGSKList(sGSKType)

            'Update the apply
            UpdateApplyGSK(True)

        Catch
        End Try



    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (RemoveRules) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RemoveRules(ByRef sGSKType As String, ByRef lPMNavKeyID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim okey As PMNavKey
    'Dim sTitle As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sTitle = "Remove Keys Rules"
    '
    '
    'Select Case m_sPMNavGroup
    'Case NavProcConst.NavGrpComponent, NavProcConst.NavGrpProcess, NavProcConst.NavGrpMap
    '
    'Case NavProcConst.NavGrpStep
    '
    'If sGSKType = NavProcConst.ACSetKey Then
    '
    'Only allow removal if this item is in  the
    'list of available keys
    '
    '
    'If g_cSystemKeys.Item(CStr(lPMNavKeyID)).IsHidden Then
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    'MessageBox.Show("You attempted to remove a non optional key.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '
    'End If
    '
    'Else
    '
    'If m_iEnforceRules = CheckState.Checked Then
    '
    'Do not allow removal of getkeys
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'MessageBox.Show("You cannot remove Step GetKeys", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    'End If
    '
    'End If
    '
    'End Select
    '
    'Return result
    '
    'Catch 
    '
    '
    '
    'Return result
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
        Dim sFormTitle As String = ""

        Try

            If m_sDescription <> "" Then

                'If we have a Navigator Group Item then set up Get/Set Keys details


                Select Case m_sPMNavGroup
                    Case NavProcConst.NavGrpComponent
                        sFormTitle = "Navigator Component - " & m_sDescription

                    Case NavProcConst.NavGrpProcess
                        sFormTitle = "Navigator Process - " & m_sDescription

                    Case NavProcConst.NavGrpMap
                        sFormTitle = "Navigator Map - " & m_sDescription

                        'No get keys for a map
                        lblGetKeys.Visible = False
                        lstGKRequiredKeys.Visible = False
                        cmdGKAdd.Visible = False
                        cmdGKRemove.Visible = False

                        'Make SetKeys taller
                        lstSKRequiredKeys.Height = lstSystemKeys.Height

                    Case NavProcConst.NavGrpStep
                        sFormTitle = "Navigator Step - " & m_sDescription
                        'lblAllKeys.Caption = "Optional Keys"

                        'Do not allow adding or removal of getkeys
                        'cmdGKAdd.Enabled = False
                        'cmdGKRemove.Enabled = False

                        'Do not allow editing of system keys here
                        txtDescription.ReadOnly = True
                        txtName.ReadOnly = True

                        'developer guide no. 6(no solutions)
                        'cboDataType.Locked = True
                        cboDataType.Enabled = True
                        txtEffectiveDate.ReadOnly = True
                        cmdAdd.Enabled = False

                End Select

            Else

                'No Selected Navigator Group Item

                sFormTitle = "Navigator Key Editor"

                'Disable the list of get/set keys
                lstSKRequiredKeys.Enabled = False
                lstGKRequiredKeys.Enabled = False

                'Disable the add/remove buttons
                cmdSKAdd.Enabled = False
                cmdSKRemove.Enabled = False
                cmdGKAdd.Enabled = False
                cmdGKRemove.Enabled = False

                'Hide the Get/Set Key details
                fraGSKDetails.Visible = False

            End If

            Me.Text = sFormTitle

            'Set default enforcement of rules
            chkEnforce.CheckState = m_iEnforceRules

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
    ' Name: UpdateApply
    '
    ' Description:  Update the Apply
    '
    ' ***************************************************************** '
    Private Sub UpdateApply(Optional ByRef iStatus As Integer = 0)

        If Not False Then

            If iStatus = m_iStatus Then
                Exit Sub
            End If

            m_iStatus = iStatus
        End If

        Select Case m_iStatus
            Case gPMConstants.PMEReturnCode.PMDataNotChanged
                cmdApply.Enabled = False
            Case gPMConstants.PMEReturnCode.PMDataChanged
                cmdApply.Enabled = True
        End Select

    End Sub

    Private Sub ClearGSKFields()

        txtGSKDescription.Text = ""

        If txtInitialKeyValue.Enabled Then
            txtInitialKeyValue.Text = ""
        End If

        If chkIsOptional.Enabled Then
            chkIsOptional.CheckState = CheckState.Unchecked
        End If

    End Sub

    Private Sub ClearKeyFields()

        'Clear fields
        txtDescription.Text = ""
        txtName.Text = ""
        txtEffectiveDate.Text = ""
        cboDataType.SelectedIndex = -1
        UpdateApply(gPMConstants.PMEReturnCode.PMDataNotChanged)

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayCurrentKey
    '
    ' Description: Display the current key details
    '
    ' ***************************************************************** '
    Private Sub DisplayCurrentKey()


        'Make sure that we have a current key
        If m_oCurrentKey Is Nothing Then
            Exit Sub
        End If

        'Set the interface details for the current key

        'Set the datatype
        For iPtr As Integer = 0 To cboDataType.Items.Count
            If VB6.GetItemData(cboDataType, iPtr) = m_oCurrentKey.DataType Then
                cboDataType.SelectedIndex = iPtr
                Exit For
            End If
        Next iPtr

        txtName.Text = m_oCurrentKey.Name.Trim()
        txtDescription.Text = m_oCurrentKey.Description.Trim()
        txtEffectiveDate.Text = m_oCurrentKey.EffectiveDate

        m_iMode = gPMConstants.PMEComponentAction.PMEdit
        UpdateApply(gPMConstants.PMEReturnCode.PMDataNotChanged)

        txtDescription.Focus()

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayCurrentGSK
    '
    ' Description: Display the current key details
    '
    ' ***************************************************************** '
    Private Sub DisplayCurrentGSK(ByRef sGSKType As String)

        'Make sure that we have a current key
        If m_oCurrentGSK Is Nothing Then
            Exit Sub
        End If

        'Set the interface details for the current gsk

        txtGSKDescription.Text = m_oCurrentGSK.Description.Trim()

        If sGSKType = NavProcConst.ACSetKey Then

            'This is a SetKey, so display required details
            fraGSKDetails.Text = "Set Key Details"

            If m_sPMNavGroup = NavProcConst.NavGrpComponent Then
                chkIsOptional.Visible = True
                txtInitialKeyValue.Visible = False
                lblInitialValue.Visible = False
                chkIsOptional.CheckState = m_oCurrentGSK.IsOptional
            Else
                chkIsOptional.Visible = False
                txtInitialKeyValue.Visible = True
                lblInitialValue.Visible = True
                txtInitialKeyValue.Text = m_oCurrentGSK.InitialKeyValue.Trim()
            End If

        Else

            'This is a GetKey, so set additional fields to invisible
            fraGSKDetails.Text = "Get Key Details"

            chkIsOptional.Visible = False
            txtInitialKeyValue.Visible = False
            lblInitialValue.Visible = False

        End If

        txtGSKDescription.Focus()

    End Sub


    ' ***************************************************************** '
    ' Name: DisplayKeyList
    '
    ' Description: Display the current key list
    '
    ' ***************************************************************** '
    Private Sub DisplayKeyList()


        'Clear list
        lstSystemKeys.Items.Clear()

        'Clear the PMNavKey Fields
        ClearKeyFields()


        'Item data pointer
        Dim iItemDataPtr As Integer = 0

        'Display all keys
        For iPtr As Integer = 1 To g_cSystemKeys.Count

            'Only show if not hidden

            If Not g_cSystemKeys.Item(iPtr).IsHidden Then


                lstSystemKeys.Items.Add(g_cSystemKeys.Item(iPtr).Description)

                VB6.SetItemData(lstSystemKeys, iItemDataPtr, g_cSystemKeys.Item(iPtr).PMNavKeyID)

                iItemDataPtr += 1
            End If

        Next iPtr

        ListBoxHelper.SetSelectedIndex(lstSystemKeys, -1)

    End Sub

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Initialise the Get/Set Keys form
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef oBusiness As Object, ByRef sPMNavGroup As String, ByRef lID As Integer, ByRef sDescription As String, Optional ByRef vMapID As String = "", Optional ByRef vEnforceRules As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Information.IsNothing(vEnforceRules) Then

                m_iEnforceRules = CInt(-CDbl(vEnforceRules))
            End If

            m_oBusiness = oBusiness
            m_sPMNavGroup = sPMNavGroup

            'Get the Navigator Item details
            m_lID = lID
            m_sDescription = sDescription
            m_lMapID = CInt(Conversion.Val(vMapID))

            'Add mode
            m_iMode = gPMConstants.PMEComponentAction.PMAdd

            'No change in data yet
            m_iStatus = gPMConstants.PMEReturnCode.PMDataNotChanged

            'Load all the system keys

            m_lReturn = LoadSystemKeys()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Load the Get/Set Keys if we have a Navigator item

            If m_sDescription <> "" Then

                m_lReturn = LoadGetSetKeys()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: LoadSystemKeys
    '
    ' Description: Load all the system keys
    '
    ' ***************************************************************** '
    Private Function LoadSystemKeys() As Integer

        Dim result As Integer = 0
        Dim vResultSet As Object
        Dim okey As PMNavKey

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not g_bSystemKeysLoaded Then

                'Get all the keys from the database

                m_lReturn = m_oBusiness.GetAllKeys(vResultSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check that we have a vaild result
                If Information.IsArray(vResultSet) Then

                    g_cSystemKeys = New Collection()


                    For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                        okey = New PMNavKey()

                        okey.PMNavKeyID = CInt(vResultSet(0, iPtr))

                        okey.Name = CStr(vResultSet(1, iPtr))

                        okey.Description = CStr(vResultSet(2, iPtr))

                        okey.DataType = CInt(vResultSet(3, iPtr))

                        okey.IsDeleted = CInt(vResultSet(4, iPtr))

                        okey.EffectiveDate = CStr(vResultSet(5, iPtr))

                        g_cSystemKeys.Add(okey, CStr(okey.PMNavKeyID))

                    Next iPtr
                End If

                g_bSystemKeysLoaded = True
            End If

            'Check if we have a valid Get/SetKey item
            If m_sDescription = "" Then

                'UnHide all system keys
                For iPtr As Integer = 1 To g_cSystemKeys.Count

                    g_cSystemKeys.Item(iPtr).IsHidden = False
                Next iPtr

            Else

                'Clear resultset
                vResultSet = Nothing

                'Get the keys to be displayed


                m_lReturn = m_oBusiness.GetKeys(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID, v_sGSKType:="ALL", r_vResultArray:=vResultSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Hide all system keys if rules are enforced
                If m_iEnforceRules = CheckState.Checked Then
                    For iPtr As Integer = 1 To g_cSystemKeys.Count

                        g_cSystemKeys.Item(iPtr).IsHidden = False
                    Next iPtr
                End If

                'Display group keys
                If Information.IsArray(vResultSet) Then


                    For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)


                        g_cSystemKeys.Item(CStr(vResultSet(0, iPtr))).IsHidden = False

                    Next iPtr

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSystemKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSystemKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LoadGetSetKeys
    '
    ' Description: Load all the system keys
    '
    ' ***************************************************************** '
    Private Function LoadGetSetKeys() As Integer

        Dim result As Integer = 0
        Dim vResultSet As Object
        Dim okey As PMNavKeyGSK

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the get/set collections
            m_cGetKeys = New Collection()
            m_cSetKeys = New Collection()

            'Get the Set keys from the database

            m_lReturn = m_oBusiness.GetGSKKeys(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID, r_vResultArray:=vResultSet, v_sGSKType:=NavProcConst.ACSetKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check that we have a vaild result
            If Information.IsArray(vResultSet) Then

                'Add all the Set Keys to List

                For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                    okey = New PMNavKeyGSK()

                    okey.PMNavKeyID = CInt(vResultSet(0, iPtr))

                    okey.Description = CStr(vResultSet(1, iPtr))

                    'Use IsOptional value only  for Component SetKey
                    If m_sPMNavGroup = NavProcConst.NavGrpComponent Then

                        okey.IsOptional = Conversion.Val(CStr(vResultSet(2, iPtr)))
                    Else

                        okey.InitialKeyValue = CStr(vResultSet(2, iPtr))
                    End If

                    'Type is SetKey
                    okey.GSKType = NavProcConst.ACSetKey

                    'Set defaults
                    okey.IsDeleted = False
                    okey.HasChanged = False
                    okey.IsNew = False

                    'Add Set Keys to collection of Get/Set Keys
                    m_cSetKeys.Add(okey, CStr(okey.PMNavKeyID))

                Next iPtr

            End If

            'Reuse result array
            vResultSet = Nothing

            'If this is a Map then do not load the Get Keys
            If m_sPMNavGroup = NavProcConst.NavGrpMap Then
                Return result
            End If

            'Get the Get keys from the database

            m_lReturn = m_oBusiness.GetGSKKeys(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID, r_vResultArray:=vResultSet, v_sGSKType:=NavProcConst.ACGetKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check that we have a vaild result
            If Not Information.IsArray(vResultSet) Then
                Return result
            End If

            'Add all the Set Keys to List

            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                okey = New PMNavKeyGSK()

                okey.PMNavKeyID = CInt(vResultSet(0, iPtr))

                okey.Description = CStr(vResultSet(1, iPtr))

                'Type is SetKey
                okey.GSKType = NavProcConst.ACGetKey

                'Set Defaults
                okey.IsDeleted = False
                okey.HasChanged = False
                okey.IsNew = False

                'Add Set Keys to collection of Get/Set Keys
                m_cGetKeys.Add(okey, CStr(okey.PMNavKeyID))

            Next iPtr

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadGetSetKeys", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGetSetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function




    ' ***************************************************************** '
    ' Name: NewKey
    '
    ' Description: Create a new system key
    '
    ' ***************************************************************** '
    Private Function NewKey() As Integer

        Dim result As Integer = 0
        Dim okey As PMNavKey

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate the on screen key
            m_lReturn = ValidateKey()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Key details
            okey = New PMNavKey()

            m_lReturn = okey.SetProperties(vDataType:=VB6.GetItemData(cboDataType, cboDataType.SelectedIndex), vName:=txtName, vDescription:=txtDescription, vEffectiveDate:=txtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the new PMNavKey and return the ID


            m_lReturn = m_oBusiness.AddPMNavKey(lDataType:=okey.DataType, sName:=okey.Name, sDescription:=okey.Description, sEffectiveDate:=okey.EffectiveDate)

            If m_lReturn = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the key to the list
            okey.PMNavKeyID = m_lReturn
            g_cSystemKeys.Add(okey, CStr(okey.PMNavKeyID))

            'Display the List of keys
            DisplayKeyList()

            'New key will be added to the end
            ListBoxHelper.SetSelectedIndex(lstSystemKeys, lstSystemKeys.Items.Count - 1)

            Return result

        Catch



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateGSK
    '
    ' Description: Updates Changed made to the Database GetSetKeys
    '               (Insert/Update/Delete)
    '
    ' ***************************************************************** '
    Private Function UpdateGSK(ByRef sGSKType As String) As Integer

        Dim result As Integer = 0
        Dim cGSKList As Collection
        Dim okey As PMNavKeyGSK
        Dim vValues(2) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sGSKType = NavProcConst.ACSetKey Then
                cGSKList = m_cSetKeys
            Else
                cGSKList = m_cGetKeys
            End If

            'Apply the  changes
            For iPtr As Integer = 1 To cGSKList.Count

                okey = cGSKList.Item(iPtr)

                'Set the values to be updated/inserted

                vValues(0) = okey.PMNavKeyID

                vValues(1) = okey.Description.Trim()

                'Set the third value
                If m_sPMNavGroup = NavProcConst.NavGrpComponent Then

                    vValues(2) = okey.IsOptional
                Else

                    vValues(2) = okey.InitialKeyValue.Trim()
                End If

                'If this GSK is flagged as deleted then delete
                If okey.IsDeleted Then

                    m_lReturn = m_oBusiness.DeleteGSKKeys(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID, v_lPMNavKeyID:=okey.PMNavKeyID, v_sGSKType:=sGSKType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'If this GSK is new then insert it
                ElseIf (okey.IsNew) Then


                    m_lReturn = m_oBusiness.InsertGSKKeys(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID, v_vValuesArray:=vValues, v_sGSKType:=sGSKType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Not new anymore
                    okey.IsNew = False
                    okey.HasChanged = False

                    'If this GSK has changed then update it
                ElseIf (okey.HasChanged) Then


                    m_lReturn = m_oBusiness.UpdateGSKKeys(v_sPMNavGroup:=m_sPMNavGroup, v_lID:=m_lID, v_lMapID:=m_lMapID, v_vValuesArray:=vValues, v_lPMNavKey:=okey.PMNavKeyID, v_sGSKType:=sGSKType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'No change
                    okey.HasChanged = False

                End If

            Next iPtr

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGSK Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGSK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateKey
    '
    ' Description: Create a new system key
    '
    ' ***************************************************************** '
    Private Function UpdateKey() As Integer

        Dim result As Integer = 0
        Dim okey As PMNavKey
        Dim lListIndex As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate the on screen key
            m_lReturn = ValidateKey()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Key details
            okey = m_oCurrentKey

            m_lReturn = okey.SetProperties(vDataType:=VB6.GetItemData(cboDataType, cboDataType.SelectedIndex), vName:=txtName, vDescription:=txtDescription, vEffectiveDate:=txtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the PMNavKey


            m_lReturn = m_oBusiness.UpdatePMNavKey(lPMNavKeyID:=okey.PMNavKeyID, lDataType:=okey.DataType, sName:=okey.Name, sDescription:=okey.Description, sEffectiveDate:=okey.EffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Save current list index
            lListIndex = ListBoxHelper.GetSelectedIndex(lstSystemKeys)

            'Display the List of keys
            DisplayKeyList()

            'Set list index
            ListBoxHelper.SetSelectedIndex(lstSystemKeys, lListIndex)

            Return result

        Catch



            Return result
        End Try
    End Function


    Private Sub UpdateApplyGSK(ByRef bFlag As Boolean)

        If cmdApplyGSK.Enabled <> bFlag Then
            cmdApplyGSK.Enabled = bFlag
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: ValidateKey
    '
    ' Description: Validate the PMNav Key
    '
    ' ***************************************************************** '
    Private Function ValidateKey() As Integer

        Dim result As Integer = 0
        Dim sTitle As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sTitle = "Add New Key"

            'Validation
            If txtDescription.Text = "" Then
                MessageBox.Show("Please enter the key description.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtDescription.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If txtName.Text = "" Then
                MessageBox.Show("Please enter the key name.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtName.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If cboDataType.SelectedIndex = -1 Then
                MessageBox.Show("Please select a data type.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cboDataType.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsDate(txtEffectiveDate.Text) Then
                MessageBox.Show("Please enter a valid date.", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtEffectiveDate.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Private Sub cboDataType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDataType.SelectedIndexChanged

        UpdateApply(gPMConstants.PMEReturnCode.PMDataChanged)

    End Sub


    Private Sub chkIsOptional_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsOptional.CheckStateChanged

        If Not (m_oCurrentGSK Is Nothing) Then
            UpdateApplyGSK(True)
            m_oCurrentGSK.HasChanged = True
            m_oCurrentGSK.IsOptional = chkIsOptional.CheckState
        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        mnuFileNew_Click(mnuFileNew, New EventArgs())

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        If m_iStatus = gPMConstants.PMEReturnCode.PMDataNotChanged Then
            Exit Sub
        End If


        Select Case m_iMode
            Case gPMConstants.PMEComponentAction.PMAdd
                m_lReturn = NewKey()

            Case gPMConstants.PMEComponentAction.PMEdit
                m_lReturn = UpdateKey()

            Case Else

        End Select

    End Sub


    ' ***************************************************************** '
    ' Name: cmdApplyGSK_Click
    '
    ' Description: Applies the changes to GSKs
    '
    ' ***************************************************************** '
    Private Sub cmdApplyGSK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApplyGSK.Click


        Dim vValues(2) As Object

        Try

            'Apply Set Key details
            m_lReturn = UpdateGSK(NavProcConst.ACSetKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not Apply SetKey details!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'Apply Get Key details
            m_lReturn = UpdateGSK(NavProcConst.ACGetKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not Apply GetKey details!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'Enforce the GSK Rules
            If chkEnforce.CheckState = CheckState.Checked Then
                m_lReturn = EnforceGSKRules()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            UpdateApplyGSK(False)

        Catch
        End Try





    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        Dim iMsgRet As DialogResult

        'Check if details have changed
        If cmdApply.Enabled Or cmdApplyGSK.Enabled Then
            iMsgRet = MessageBox.Show("The Details have changed. " & _
                      "Do you wish to discard your changes?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If iMsgRet = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

        End If

        Me.Hide()

    End Sub

    Private Sub cmdGKAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGKAdd.Click

        AddKey(NavProcConst.ACGetKey)

    End Sub

    Private Sub cmdGKRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGKRemove.Click

        RemoveKey(NavProcConst.ACGetKey)

    End Sub


    Private Sub cmdSKAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSKAdd.Click

        AddKey(NavProcConst.ACSetKey)

    End Sub

    Private Sub cmdSKRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSKRemove.Click

        RemoveKey(NavProcConst.ACSetKey)

    End Sub



	Private Sub frmKeySel_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		'Set the Interface Defaults
		m_lReturn = SetInterfaceDefaults()
		
		'Display the list of Navigator keys
		DisplayKeyList()
		
		'Display the list of Get/Set Keys
		If m_sDescription <> "" Then
			DisplayGSKList(NavProcConst.ACSetKey)
			DisplayGSKList(NavProcConst.ACGetKey)
		End If
		
		DisplayPMTypesList()
		
	End Sub
	
	
	Private Sub lstGKRequiredKeys_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstGKRequiredKeys.SelectedIndexChanged
		
		
		Dim lListIndex As Integer = ListBoxHelper.GetSelectedIndex(lstGKRequiredKeys)
		
		If lListIndex = -1 Then
			Exit Sub
		End If
		
		'Get the details of this key
		m_oCurrentGSK = m_cGetKeys.Item(CStr(VB6.GetItemData(lstGKRequiredKeys, lListIndex)))
		
		
		'Save the apply state
		Dim bApplyState As Boolean = cmdApplyGSK.Enabled
		
		DisplayCurrentGSK(NavProcConst.ACGetKey)
		
		'Set apply too prior state
		UpdateApplyGSK(bApplyState)
		
	End Sub
	
	Private Sub lstSKRequiredKeys_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstSKRequiredKeys.SelectedIndexChanged
		
		
		Dim lListIndex As Integer = ListBoxHelper.GetSelectedIndex(lstSKRequiredKeys)
		
		If lListIndex = -1 Then
			Exit Sub
		End If
		
		'Get the details of this key
		m_oCurrentGSK = m_cSetKeys.Item(CStr(VB6.GetItemData(lstSKRequiredKeys, lListIndex)))
		
		'Save the apply state
		Dim bApplyState As Boolean = cmdApplyGSK.Enabled
		
		DisplayCurrentGSK(NavProcConst.ACSetKey)
		
		'Set apply too prior state
		UpdateApplyGSK(bApplyState)
		
	End Sub
	
	Private Sub lstSystemKeys_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstSystemKeys.SelectedIndexChanged
		
		
		Dim lListIndex As Integer = ListBoxHelper.GetSelectedIndex(lstSystemKeys)
		
		If lListIndex = -1 Then
			Exit Sub
		End If
		
		'Edit mode
		m_iMode = gPMConstants.PMEComponentAction.PMEdit
		
		'Get the details of this key
		m_oCurrentKey = g_cSystemKeys.Item(CStr(VB6.GetItemData(lstSystemKeys, lListIndex)))
		
		DisplayCurrentKey()
		
	End Sub
	
	
	
	Public Sub mnuFileNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileNew.Click
		
		'Set current key to nothing
		m_oCurrentKey = Nothing
		
		'Set Add mode
		m_iMode = gPMConstants.PMEComponentAction.PMAdd
		
		'deselect list item
		ListBoxHelper.SetSelectedIndex(lstSystemKeys, -1)
		
		'Clear the PMNavKeyFields
		ClearKeyFields()
		
		'Set focus on first field
		txtDescription.Focus()
		
	End Sub
	
	
	Private isInitializingComponent As Boolean
	Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(gPMConstants.PMEReturnCode.PMDataChanged)
		
	End Sub
	
	
	Private Sub txtEffectiveDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(gPMConstants.PMEReturnCode.PMDataChanged)
		
	End Sub
	
	
	Private Sub txtGSKDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtGSKDescription.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Not (m_oCurrentGSK Is Nothing) Then
			UpdateApplyGSK(True)
			m_oCurrentGSK.HasChanged = True
		End If
		
	End Sub
	
	Private Sub txtGSKDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtGSKDescription.Leave
		
		If Not (m_oCurrentGSK Is Nothing) Then
			m_oCurrentGSK.Description = txtGSKDescription.Text
		End If
		
	End Sub
	
	
	Private Sub txtInitialKeyValue_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitialKeyValue.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Not (m_oCurrentGSK Is Nothing) Then
			UpdateApplyGSK(True)
			m_oCurrentGSK.HasChanged = True
		End If
		
	End Sub
	
	
	Private Sub txtInitialKeyValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitialKeyValue.Leave
		
		If Not (m_oCurrentGSK Is Nothing) Then
			m_oCurrentGSK.InitialKeyValue = txtInitialKeyValue.Text
		End If
		
	End Sub
	
	
	Private Sub txtName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		UpdateApply(gPMConstants.PMEReturnCode.PMDataChanged)
		
	End Sub
End Class
