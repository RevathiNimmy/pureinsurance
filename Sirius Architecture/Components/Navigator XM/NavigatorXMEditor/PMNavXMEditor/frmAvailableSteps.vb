Option Strict Off
Option Explicit On
Imports SharedFiles
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmAvailableSteps
	Inherits System.Windows.Forms.Form
	Private Sub frmAvailableSteps_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_lReturn As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lAvailableStepID As Integer
	
	Private m_vSteps( ,  ) As Object
	

    Private m_oBusiness As bPMNavXMEditor.Business

    Private Const ACClass As String = "frmAvailableSteps"

    ' m_vTaskMaps array constants
    Private Const AVAILABLE_STEP_ID As Integer = 0
    Private Const AVAILABLE_STEP_CODE As Integer = 1
    Private Const AVAILABLE_STEP_DESC As Integer = 2
    Private Const AVAILABLE_STEP_ICON As Integer = 3


    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public ReadOnly Property AvailableStepID() As Integer
        Get
            Return m_lAvailableStepID
        End Get
    End Property

    Public WriteOnly Property Business() As bPMNavXMEditor.Business
        Set(ByVal Value As bPMNavXMEditor.Business)
            m_oBusiness = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Initialise the form
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Load (Standard Method)
    '
    ' Description: Load the form details
    '
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Gets the interface details to be displayed.
            m_lReturn = GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_lAvailableStepID = ID_NO_VALUE

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Load failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form using the display state passed
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Show the the form, allow user input etc.
            VB6.ShowForm(Me, lDisplayState)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowForm failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Get the details from the business object.


            m_lReturn = m_oBusiness.GetAvailableSteps(m_vSteps)

            ' Check for other errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: get the roadmap info to display on listview
    '
    ' History: RDC 28032003 created
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Get the interface details from the
            ' business object.
            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return result
            End If

            ' Assign the details from the List data storage
            ' to the interface.
            m_lReturn = DataToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInterfaceDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: display the roadmap info on the listview
    '
    ' History: RDC 28032003 created
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim lWidth As Integer
        Dim oItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            lvwSteps.View = View.Details

            lWidth = CInt((VB6.PixelsToTwipsX(lvwSteps.Width) / 2) - 45)

            lvwSteps.Columns.Clear()
            lvwSteps.Columns.Add("StepCode", "Step Code", CInt(VB6.TwipsToPixelsX(lWidth)))
            lvwSteps.Columns.Add("StepDesc", "Step Description", CInt(VB6.TwipsToPixelsX(lWidth)))

            lvwSteps.Items.Clear()

            For lLoop As Integer = m_vSteps.GetLowerBound(1) To m_vSteps.GetUpperBound(1)


                oItem = lvwSteps.Items.Add("T" & CStr(m_vSteps(AVAILABLE_STEP_ID, lLoop)), CStr(m_vSteps(AVAILABLE_STEP_CODE, lLoop)), m_vSteps(AVAILABLE_STEP_ICON, lLoop))

                ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vSteps(AVAILABLE_STEP_DESC, lLoop))

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	
	Private Sub frmAvailableSteps_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_lAvailableStepID = ID_NO_VALUE And m_lStatus = gPMConstants.PMEReturnCode.PMOk Then
			' nothing selected when Ok clicked
			Cancel = True
			Exit Sub
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub lvwSteps_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSteps.DoubleClick
		
		If m_lAvailableStepID <> ID_NO_VALUE Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Close()
		End If
		
	End Sub
	
	Private Sub lvwSteps_ItemClick(ByVal Item As ListViewItem)
		
		m_lAvailableStepID = CInt(Mid(Item.Name, 2))
		
	End Sub
End Class
