Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 10/05/1999
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' CJB 23/11/2005 PN25924 Changed GetInterfaceDetails to improve error recovery.
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Private instance of the interface form.
    Private m_frmInterface As Object

    ' Private instance of the business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRDocTemplate.Business

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
    Public Function Initialise(ByRef frmInterface As iPMBDocTemplate.frmInterface, ByRef oBusiness As Object) As Integer

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
            Me.disposedValue = True
            Me.disposedValue = True
            If disposing Then
                m_oBusiness = Nothing
                If Not m_frmInterface Is Nothing Then
                    m_frmInterface.Dispose()
                End If
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

            ' Check the task.
            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMDelete Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = CType(m_frmInterface.GetBusiness(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = CType(m_frmInterface.BusinessToInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display all of the lookup details.
            m_lReturn = CType(m_frmInterface.DisplayLookupDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the type editable.
            ' Note - cannot do it earlier - WHY??
            m_lReturn = CType(m_frmInterface.SetTypeEditable(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_frmInterface.GetType().Name.Trim().ToUpper() <> "IPMBDOCTEMPLATEFORMLESS") Then
                'Get system option CCMDocProduction
                Dim sCCMDocProduction As String = "0"

                If Not m_frmInterface.bFormlessDocument Then
                    Dim nDocType As Integer = DirectCast(m_frmInterface.cboType.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                    If nDocType <> PMBConst.PMBClauseTextFile Then ''working same as Pure for Clauses doc type
                        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem, r_sOptionValue:=sCCMDocProduction)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                Dim sKCMForSelectedTemplate As String = ""
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=KSystemOptionKCMForSelectedTemplate, r_sOptionValue:=sKCMForSelectedTemplate, v_iSourceID:=g_iSourceID)
                If sCCMDocProduction = "1" Then
                    If sKCMForSelectedTemplate = "1" AndAlso String.IsNullOrEmpty(m_frmInterface.CCMDocumentName) Then
                        ' Copy the document.
                        If (m_frmInterface.Mode = gSIRLibrary.ACNormalMode) Or (m_frmInterface.Mode = gSIRLibrary.ACSwiftEditMode) Then
                            m_lReturn = m_frmInterface.CopyServerToClient() ''only for PURE
                            'RWH(15/09/2000) Handle failed copy.
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                Else
                    ' Copy the document.
                    If (m_frmInterface.Mode = gSIRLibrary.ACNormalMode) Or (m_frmInterface.Mode = gSIRLibrary.ACSwiftEditMode) Then
                        m_lReturn = m_frmInterface.CopyServerToClient() ''only for PURE
                        'RWH(15/09/2000) Handle failed copy.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If
            ' Merge the document.
            m_lReturn = m_frmInterface.MergeDocument()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then ' PN25924
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we want to display the interface, or just print the document?
            If m_frmInterface.Mode <> gSIRLibrary.ACPrintMode _
                        And m_frmInterface.Mode <> gSIRLibrary.ACSpoolDocMode _
                        And m_frmInterface.Mode <> gSIRLibrary.ACPrintSilentMode _
                        And m_frmInterface.Mode <> gSIRLibrary.ACSpoolReportMode _
                        And m_frmInterface.Mode <> ACArchiveSilentMode _
                        And m_frmInterface.Mode <> ACPrintOnlySilentMode _
                        And m_frmInterface.Mode <> ACPrintSpoolArchiveSilentMode _
                        And m_frmInterface.Mode <> ACPrintSpoolSilentMode _
                        And m_frmInterface.Mode <> ACSpoolArchiveSilentMode _
                        And m_frmInterface.Mode <> ACSpoolSilentMode Then

                ' Display the document.
                '    m_lReturn = m_frmInterface.SetOLE
                '    'RWH(27/07/2000)
                m_lReturn = m_frmInterface.SetBrowser()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then ' PN25924
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Check the task.
            If (m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView) Or (m_frmInterface.Task = gPMConstants.PMEComponentAction.PMDelete) Then
                ' Disable the interface to only allow viewing.
                m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                    If m_frmInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                        'MKW 301003 PN7733 Moved Code Check to before InterfaceToBusiness START
                        ' using interface field direct as get is not populated until calling InterfaceToBusiness.
                        If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMAdd Then


                            m_lReturn = m_oBusiness.CheckCode(vCode:=m_frmInterface.txtCode.Text, v_dtEffectiveDate:=m_frmInterface.cboEffectiveDate.Value)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then



                                'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                                'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)


                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        End If
                        'MKW 301003 PN7733 Moved Code Check to before InterfaceToBusiness END

                        ' Update the business from the interface.
                        m_lReturn = CType(m_frmInterface.InterfaceToBusiness(), gPMConstants.PMEReturnCode)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return result
                        End If
                    End If
            End Select

            ' Check the task.

            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages




                        'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                        'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        'MKW 301003 PN7733 Moved Code Check to before InterfaceToBusiness START
                        '                m_lReturn = m_oBusiness.CheckCode(vCode:=m_frmInterface.DocumentTemplateCode)
                        '
                        '                If (m_lReturn <> PMNotFound) Then
                        '                    sTitle$ = iPMFunc.GetResData( _
                        ''                        iLangID:=g_iLanguageID%, _
                        ''                        lID:=ACAddDetailsTitle, _
                        ''                        iDataType:=PMResString)
                        '
                        '                    sMessage$ = iPMFunc.GetResData( _
                        ''                        iLangID:=g_iLanguageID%, _
                        ''                        lID:=ACAddDetails, _
                        ''                        iDataType:=PMResString)
                        '
                        '                    MsgBox sMessage$, vbOKOnly, sTitle$
                        '
                        '                    ProcessCommand = PMFalse
                        '
                        '                    Exit Function
                        '                End If
                        'MKW 301003 PN7733 Moved Code Check to before InterfaceToBusiness END

                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If


                        m_frmInterface.DocumentTemplateId = m_oBusiness.DocumentTemplateId

                    End If

                    'we're not really deleting anymore - we're just updating the IsDeleted flag...
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()

                        If m_lReturn = gPMConstants.PMEReturnCode.PMDataChanged Then
                            ' Get string messages



                            'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                            'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                            ' Check message result.
                            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                                ' Set return to false, meaning
                                ' don't cancel.
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    Else

                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

                    End If

                    '        Case PMDelete
                    '            ' Update the details using the business object.
                    '            m_lReturn& = m_oBusiness.Update()
                    '
                    '            ' Check for errors.
                    '            If (m_lReturn& <> PMTrue) Then
                    '               ' Failed to update the details
                    '               ProcessCommand = PMFalse
                    '
                    '               ' Log Error.
                    '               LogMessage _
                    ''                   iType:=PMLogError, _
                    ''                   sMsg:="Failed to update the details", _
                    ''                   vApp:=ACApp, _
                    ''                   vClass:=ACClass, _
                    ''                   vMethod:="ProcessCommand"
                    '            End If

            End Select

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
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set all of the forms controls to the disable state.
        If TypeOf m_frmInterface Is System.Windows.Forms.Form Then
            For Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is RadioButton) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                End If
            Next ctlFormControl

            '    m_frmInterface.OLE1.Enabled = Not lDisabled
        End If
        Return result
    End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
        ''


    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function InitialiseFormless(ByRef frmInterface As iPMBDocTemplate.iPMBDocTemplateFormless, ByRef oBusiness As Object) As Integer

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

End Class

