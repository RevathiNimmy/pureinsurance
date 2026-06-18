Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No.: 129
Imports SharedFiles


Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 02/07/1998
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Private instance of the interface form.

    'Developer Guide No: 291
    Private m_frmInterface As Object


    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode




    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            m_oBusiness = oBusiness

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oBusiness = Nothing
                m_frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all of the lookup details.

            m_lReturn = m_frmInterface.DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Check the task.

            If m_frmInterface.ClaimMode = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.ClaimMode = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.

                m_lReturn = m_frmInterface.GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.

                m_lReturn = m_frmInterface.BusinessToInterface()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            Return result

        Catch excep As System.Exception




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Dim vClmExpServId, vClaim_Id, vExpServId, vPrtyClmId, vServTypeId As Integer
        Dim vService, vDescription, vReference, vContact As String
        Dim vDateReq As String = ""
        Dim vDateCrit, vDateRecv As Object
        Dim sClaimNumber As String = ""
        Dim lOriginalClaimId As Integer
        'For Event Description
        Const kEVENT_TYPE_UPDATECLAIM As Integer = 6

        Dim sEventDesc As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to PMFalse, meaning
                    ' don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse

                Else
                    'Check here if this is calling from Reserve From then don't delete Claim Details
                    If m_ofrmInterface.Task <> gPMConstants.PMEComponentAction.PMView And m_ofrmInterface.CallingAppName.Trim().ToUpper() <> "IPMURISK" Then

                        If m_ofrmInterface.TransactionType <> "C_CO" Then

                            'get base claim id

                            m_lReturn = g_oBusiness.GetOriginalClaimID(v_lClaimID:=m_ofrmInterface.ClaimId, r_lOriginalClaimID:=lOriginalClaimId)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' unlock claim
                            m_lReturn = CType(m_ofrmInterface.UnlockClaim(v_lOriginalClaimID:=lOriginalClaimId), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If


                        g_oBusiness.ClaimId = m_ofrmInterface.ClaimId


                        m_lReturn = g_oBusiness.DeleteClaim
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If

                    End If

                End If

            Else
                '**************START - Code to check if all the mandatory fields were filled for the
                '**************filled for the default services & requirements for ADD mode
                'JMK 25/05/2001 also for Edit Mode, as mandatory fields can be deleted
                'If (m_frmInterface.ClaimMode) = PMAdd Then

                If m_frmInterface.Task <> gPMConstants.PMEComponentAction.PMView
                    If (m_frmInterface.ClaimMode) = gPMConstants.PMEComponentAction.PMAdd Or ((m_frmInterface.ClaimMode) = gPMConstants.PMEComponentAction.PMEdit) Then

                        For iCount As Integer = 1 To m_ofrmInterface.lvwInfoChklst.Items.Count

                            'get the values from the collection to check if all the mandatory field
                            'were filled for the default services & requirements

                            result = g_oBusiness.GetNext(v_lCurrentRecord:=iCount, vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv)
                            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If vService = "" Then 'By SV

                                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryMainTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryMain, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                'MsgBox "Data in a mandatory field is missing"
                                '                         Call DisplayMessage(ACMandatoryFieldMsg, Mid(txtRequirement.Name, 4))
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                '   If all the Mandatory fields are having values SET the ProcessCommand = PMTrue
                                result = gPMConstants.PMEReturnCode.PMTrue
                            End If

                            If vDateReq = "" Then 'By SV

                                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryMainTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryMain, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                'MsgBox "Data in a mandatory field is missing"
                                '                         Call DisplayMessage(ACMandatoryFieldMsg, Mid(txtDateRequested.Name, 4))
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                '   If all the Mandatory fields are having values SET the ProcessCommand = PMTrue
                                result = gPMConstants.PMEReturnCode.PMTrue
                            End If

                        Next iCount

                        If m_ofrmInterface.lvwInfoChklst.Items.Count > 0 Then
                            ' Add the details using the business object.

                            result = g_oBusiness.Update()
                            If m_ofrmInterface.EventInfo Then
                                sEventDesc = Interaction.InputBox("Enter the Event Description", "Event Log", sEventDesc)

                                m_lReturn = m_oBusiness.CreateEvent(kEVENT_TYPE_UPDATECLAIM, sEventDesc, vClaim_Id)
                                m_ofrmInterface.EventInfo = False
                            End If
                        End If

                        ' Check for errors.
                        If result <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

                    End If
                End If
                '**************END - Code to check if all the mandatory fields were filled for the
                '**************filled for the default services & requirements for ADD mode


                'TN20010604 start
                If m_ofrmInterface.WorkInfoOnlyFlag Then
                    Return result
                End If
                'TN20010604 end


                'RWH(15/06/01) Added check on Previous InfoOnly status so that we do
                'external handler stuff if updating a previously Info Only claim.

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'PUBLIC Methods (End)


    'PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '

    'Private Function DisableForm(ByRef lDisabled As Integer) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Set all of the forms controls to the disable state.

    'For	Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
    ' Check the type of the control.
    'If TypeOf ctlFormControl Is TextBox Then
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is RadioButton) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'End If
    'Next ctlFormControl
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
