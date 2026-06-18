Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmGroupSummary

    Inherits System.Windows.Forms.Form
    Private Const vbFormCode As Integer = 0

	Private Const ACClass As String = "frmGroupSummary"
	
	' Error
	Private m_lError As Integer
	Private m_lReturn As Integer
	' Details
	Private m_vDataArray( ,  ) As Object
	' GISListTypeID
	Private m_lGISListTypeID As Integer
	
	' Description
	Private m_sDescription As String = ""
	
	Private m_bDeleted As Boolean
	
	' Status
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public Property Description() As String
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value
		End Set
	End Property
	
	Public Property Error_Renamed() As Integer
		Get
			Return m_lError
		End Get
		Set(ByVal Value As Integer)
			m_lError = Value
		End Set
	End Property
	
	Public Property GISListTypeID() As Integer
		Get
			Return m_lGISListTypeID
		End Get
		Set(ByVal Value As Integer)
			m_lGISListTypeID = Value
		End Set
	End Property
	
	' ***************************************************************** '
	'
	' Name: ConfigureResize
	'
	' Description:
	'
	' History: 15/11/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function ConfigureResize() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			With uctPMResizer1
				
				' minimum form size
				.FormMinHeight = 4065
				.FormMinWidth = 5685
				
				' Only resize what we tell it to
				.NoResizeByDefault = True
				
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lvwGroups", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("chkDeleted", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfigureResize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfigureResize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub chkDeleted_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDeleted.CheckStateChanged

        ' Refresh the interface
        m_lReturn = DataToInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' boooooooom
        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = ShowItems(v_iTask:=gPMConstants.PMEComponentAction.PMAdd)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' woopsie
        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: DeleteItem
    '
    ' Description:
    '
    ' History: 27/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteItem() As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer
        Dim sDescription, sCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Exit if nothing selected
            If lvwGroups.FocusedItem Is Nothing Then
                Return result
            End If

            lIndex = Convert.ToString(lvwGroups.FocusedItem.Tag)

            sCode = CStr(m_vDataArray(ACGroupingArrayCode, lIndex)).Trim()
            sDescription = CStr(m_vDataArray(ACGroupingArrayDescription, lIndex)).Trim()

            If CDbl(m_vDataArray(ACGroupingArrayIsDeleted, lIndex)) = 0 Then
                ' Check if the user is sure
                m_lReturn = MessageBox.Show("Are you sure you wish to delete the group : " & Environment.NewLine & sDescription, sCode, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            Else
                ' Check if the user is sure
                m_lReturn = MessageBox.Show("Are you sure you wish to undelete the group : " & Environment.NewLine & sDescription, sCode, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If
            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

                ' Mark it as un/deleted
                m_vDataArray(ACGroupingArrayIsDeleted, lIndex) = CInt(m_vDataArray(ACGroupingArrayIsDeleted, lIndex)) Xor 1

                ' Make sure we update
                m_bDeleted = True

                ' Refresh the display
                m_lReturn = DataToInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessClose
    '
    ' Description:
    '
    ' History: 21/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessClose() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Close the interface
            If MessageBox.Show("Are you sure you wish to cancel? You will lose any changes", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                Me.Hide()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessClose Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClose", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        m_lReturn = ProcessClose()

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        m_lReturn = DeleteItem()

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_lReturn = ShowItems(v_iTask:=gPMConstants.PMEComponentAction.PMEdit)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' woopsie
        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ProcessDeleted
    '
    ' Description: Updates deleted/undeleted items
    '
    ' History: 28/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessDeleted() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Call the business object

            m_lReturn = g_oBusiness.ProcessDeleted(v_vDataArray:=m_vDataArray)

            ' set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDeleted", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        If m_bDeleted Then
            m_lReturn = ProcessDeleted()
        End If

        ' Close the interface
        Me.Hide()

    End Sub

    Private Sub frmGroupSummary_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            m_lReturn = ConfigureResize()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Fret not
            End If

        End If
    End Sub


    Private Sub frmGroupSummary_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            Error_Renamed = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Not deleted yet
            m_bDeleted = False

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

    ' ***************************************************************** '
    '
    ' Name: BusinessToData
    '
    ' Description:
    '
    ' History: 15/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the business object

            m_lReturn = g_oBusiness.GetItemsSummary(v_lGISListTypeID:=m_lGISListTypeID, r_vResultArray:=m_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get summary details", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
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
    ' History: 19/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sKey, sText As String
        Dim bShow, bDeleted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the caption
            Me.Text = Description

            ' Clear the queue
            lvwGroups.Items.Clear()

            ' Any values?
            If Not Information.IsArray(m_vDataArray) Then
                Return result
            End If

            For lLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                bDeleted = False

                If CInt(m_vDataArray(ACGroupingArrayIsDeleted, lLoop1)) = 1 Then
                    bDeleted = True
                End If

                bShow = True

                If chkDeleted.CheckState <> CheckState.Checked And bDeleted Then
                    bShow = False
                End If

                If bShow Then

                    sKey = "G" & lLoop1
                    sText = CStr(m_vDataArray(ACGroupingArrayCode, lLoop1)).Trim()

                    ' Code
                    If bDeleted Then

                        lstItem = lvwGroups.Items.Add(sKey, sText, ACDeletedIcon)
                    Else

                        lstItem = lvwGroups.Items.Add(sKey, sText, ACNormalIcon)
                    End If

                    ' Store the index
                    lstItem.Tag = CStr(lLoop1)

                    sText = CStr(m_vDataArray(ACGroupingArrayDescription, lLoop1)).Trim()

                    ' Description
                    ListViewHelper.GetListViewSubItem(lstItem, 1).Text = sText

                    ' Number of items
                    ListViewHelper.GetListViewSubItem(lstItem, 2).Text = CStr(m_vDataArray(ACGroupingArrayItemsCnt, lLoop1)).Trim()

                End If

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
    ' Description:
    '
    ' History: 15/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the data from the business object
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = BusinessToData()

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

    ' ***************************************************************** '
    '
    ' Name: ShowItems
    '
    ' Description:
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ShowItems(ByVal v_iTask As Integer) As Integer

        Dim result As Integer = 0
        Dim oForm As frmItems
        Dim lGISListGroupingID, lIndex As Integer
        Dim sCode, sDesc As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                If lvwGroups.FocusedItem Is Nothing Then
                    Return result
                End If
            End If

            oForm = New frmItems()

            ' Set it's task mode
            oForm.Task = v_iTask

            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ' Get the grouping id

                lIndex = Convert.ToString(lvwGroups.FocusedItem.Tag)
                lGISListGroupingID = CInt(m_vDataArray(ACGroupingArrayID, lIndex))
                sCode = CStr(m_vDataArray(ACGroupingArrayCode, lIndex))
                sDesc = CStr(m_vDataArray(ACGroupingArrayDescription, lIndex))
                ' pass the id through
                oForm.GISListGroupingID = lGISListGroupingID
                ' code
                oForm.Code = sCode.Trim()
                ' description
                oForm.Description = sDesc.Trim()
            End If

            oForm.GISListTypeID = m_lGISListTypeID

            ' Load it

            'Modified by Vijay Pal on 5/26/2010 5:01:34 PM refer developer guide no. 68
            'Load(oForm)


            If oForm.Error_Renamed <> gPMConstants.PMEReturnCode.PMFalse Then

                ' Show the form
                oForm.ShowDialog()

            End If

            ' Get the code and description back if we edited
            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then

                    m_vDataArray(ACGroupingArrayCode, lIndex) = oForm.Code
                    m_vDataArray(ACGroupingArrayDescription, lIndex) = oForm.Description
                    m_vDataArray(ACGroupingArrayItemsCnt, lIndex) = oForm.NewItemCount

                    ' Refresh the list
                    m_lReturn = DataToInterface()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                ' Reload them all
                m_lReturn = GetBusiness()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Unload the form
            oForm.Close()

            oForm = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Sub frmGroupSummary_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Close the interface


        If UnloadMode <> vbFormCode Then
            If MessageBox.Show("Are you sure you wish to cancel? You will lose any changes", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Cancel = 1
            End If
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub lvwGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwGroups.DoubleClick

        m_lReturn = ShowItems(v_iTask:=gPMConstants.PMEComponentAction.PMEdit)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' woopsie
        End If

    End Sub

    Private Sub lvwGroups_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwGroups.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim iDeleted As Integer
        Dim lIndex As Integer

        ' Get the item selected
        Dim lstItem As ListViewItem = lvwGroups.GetItemAt(x, y)

        ' Was there one?
        If Not (lstItem Is Nothing) Then


            lIndex = Convert.ToString(lstItem.Tag)

            iDeleted = CInt(m_vDataArray(ACGroupingArrayIsDeleted, lIndex))

            If iDeleted = 0 Then
                cmdDelete.Text = ACCaptionDelete
            Else
                cmdDelete.Text = ACCaptionUnDelete
            End If

        End If

    End Sub
End Class
