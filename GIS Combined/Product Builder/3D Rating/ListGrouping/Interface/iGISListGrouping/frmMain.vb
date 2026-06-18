Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmMain"
	
	' Error
	Private m_lError As Integer
	Private m_lReturn As Integer
	
	' Details
	Private m_vDataArray( ,  ) As Object
	
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
	' History: 15/11/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Call the business object

            m_lReturn = g_oBusiness.GetGroupSummary(r_vResultArray:=m_vDataArray)
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
    ' History: 15/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sKey, sText As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list
            lvwListType.Items.Clear()

            ' Check we have some results
            If Information.IsArray(m_vDataArray) Then

                ' Display the information
                For lLoop1 As Integer = 0 To m_vDataArray.GetUpperBound(1)

                    sText = CStr(m_vDataArray(ACSummaryArrayCode, lLoop1)).Trim()
                    sKey = "K" & lLoop1

                    lstItem = lvwListType.Items.Add(sKey, sText, ACNormalIcon)
                    lstItem.Tag = CStr(lLoop1)
                    ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vDataArray(ACSummaryArrayDescription, lLoop1)).Trim()
                    ListViewHelper.GetListViewSubItem(lstItem, 2).Text = CStr(m_vDataArray(ACSummaryArrayUsed, lLoop1))

                Next lLoop1

            Else

                ' Display a message
                lstItem = lvwListType.Items.Add("Error")
                ListViewHelper.GetListViewSubItem(lstItem, 1).Text = "You must configure the List Types first."


                cmdAuto.Enabled = False
                cmdEdit.Enabled = False

            End If

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
    ' Name: EditGroup
    '
    ' Description:
    '
    ' History: 19/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function EditGroup() As Integer

        Dim result As Integer = 0
        Dim oForm As frmGroupSummary
        Dim lGISListTypeID, lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwListType.FocusedItem Is Nothing Then
                Return result
            End If

            oForm = New frmGroupSummary()

            ' Get the type of the selected item

            lIndex = Convert.ToString(lvwListType.FocusedItem.Tag)
            lGISListTypeID = CInt(m_vDataArray(ACSummaryArrayListTypeID, lIndex))

            ' Set the group id
            oForm.GISListTypeID = lGISListTypeID

            ' Set the description
            oForm.Description = CStr(m_vDataArray(ACSummaryArrayDescription, lIndex))

            ' Load the form

            'Modified by Vijay Pal on 5/26/2010 5:02:12 PM refer developer guide no. 68
            'Load(oForm)

            ' Check for errors

            If oForm.Error_Renamed <> gPMConstants.PMEReturnCode.PMFalse Then

                ' Show the form
                oForm.ShowDialog()

            End If

            ' Unload the form
            oForm.Close()

            oForm = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AutoGroup
    '
    ' Description: Auto groups the items
    '
    ' History: 26/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function AutoGroup() As Integer

        Dim result As Integer = 0
        Dim lGISListTypeID, lIndex As Integer
        Dim sDescription, sCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwListType.FocusedItem Is Nothing Then
                Return result
            End If

            ' Get the item selected

            lIndex = Convert.ToString(lvwListType.FocusedItem.Tag)

            ' Get the list type and code
            lGISListTypeID = CInt(m_vDataArray(ACSummaryArrayListTypeID, lIndex))
            sDescription = CStr(m_vDataArray(ACSummaryArrayDescription, lIndex)).Trim()
            sCode = CStr(m_vDataArray(ACSummaryArrayCode, lIndex)).Trim()

            ' Prompt first
            m_lReturn = MessageBox.Show("Are you sure you wish to create groups for" & Environment.NewLine & _
                        sDescription & "?", sCode, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

                ' Set the mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


                m_lReturn = g_oBusiness.AutoGroup(v_lGISListTypeID:=lGISListTypeID)

                ' Set the mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to autogroup the items. Check that the items are configured correctly.", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoGroup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' Refresh the list
                m_lReturn = GetBusiness()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function


    Private Sub cmdAuto_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAuto.Click

        m_lReturn = AutoGroup()

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_lReturn = EditGroup()

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        ' hide the form
        Me.Hide()

    End Sub

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
                .SetControlResizeOption("cmdExit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdAuto", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("lvwListType", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

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


    Private Sub frmMain_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            m_lReturn = ConfigureResize()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Fret not
            End If

        End If
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
	
	Private Sub lvwListType_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListType.DoubleClick
		
		m_lReturn = EditGroup()
		
	End Sub
End Class
