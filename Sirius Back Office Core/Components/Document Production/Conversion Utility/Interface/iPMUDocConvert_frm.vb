Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Friend Class iPMUDocConvert_frm
    Inherits System.Windows.Forms.Form
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_sErrorDesc As String

    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date

    Private m_sStepStatus As String

    'Private m_oBusiness As Object

    'Users
    Private m_lReturn As Integer
    Private m_vTemplateArray As Object
    Private m_sClient As String
    Private m_sServer As String
    Private m_sTextServer As String

    Private m_oWord As Microsoft.Office.Interop.Word.Application
    Private m_bCreatedWord As Boolean

    'RWH(31/07/2000) Merge Field markers (These are inserted in Word
    'as "<@" and "@>" but when view as flat text appear as below).
    'Private Const m_sFIELD_START_MARKER As String = "&lt;@"
    'Private Const m_sFIELD_END_MARKER As String = "@&gt;"
    Private Const m_sFIELD_START_MARKER As String = "<@"
    Private Const m_sFIELD_END_MARKER As String = "@>"
    Private Const m_iFIELD_MARKER_LENGTH As Short = 5

    'Constants for TemplateArray indexes.
    Private Const iTEMPLATE_ID As Short = 0
    Private Const iTYPE_ID As Short = 1
    Private Const iCODE As Short = 2
    Private Const iDESCRIPTION As Short = 3
    Private Const iENTITY_TYPE_ID As Short = 4
    Private Const iSLOT As Short = 5
    Private Const iFILE_NUMBER As Short = 6

    'Entity Type Id's
    Private Const iCLIENT As Short = 1
    Private Const iPOLICY As Short = 2

    Private Const m_sZIP_DIRECTORY As String = "c:\Pure\DocZipTemp"


    Public ReadOnly Property ErrorDesc() As String
        Get
            ErrorDesc = m_sErrorDesc
        End Get
    End Property



    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            ErrorNumber = m_lErrorNumber

        End Get
    End Property


    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' We have a let status here, so if we're printing that we can set the status to PMOK
    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Status = m_lStatus

        End Get
        Set(ByVal Value As Integer)

            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Short
        Get

            Task = m_iTask

        End Get
        Set(ByVal Value As Short)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim sInsRef As String
        Dim sInsFileType As String

        Try

        GetBusiness = gPMConstants.PMEReturnCode.PMTrue

        ' Get the details from the business object.

        '    ' Display a searching message.
        '    DisplayStatusSearching

        ' {* USER DEFINED CODE (Begin) *}

        ' Get the details from the business object.

        '    g_oBusiness.SourceId = m_lSourceId
        '
        'UPGRADE_WARNING: Couldn't resolve default property of object g_oBusiness.GetAllTemplates. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_lReturn = g_oBusiness.GetAllTemplates(r_vResultArray:=m_vTemplateArray)

        ' {* USER DEFINED CODE (End) *}

        ' Check the return values.
        Select Case (m_lReturn)
            Case gPMConstants.PMEReturnCode.PMTrue
                ' Found search details.

            Case gPMConstants.PMEReturnCode.PMNotFound
                ' No search details found.

            Case Else
                ' Failed to get details.
                GetBusiness = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Exit Function
        End Select

        '    ' Display the number of item found message.
        '    DisplayStatusFound

        Exit Function


        Catch ex As Exception

        GetBusiness = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DataToInterface
    '
    ' Description:
    '
    ' History: 25/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer
        Dim iTemplateCount As Short
        Dim sDescription As String

        Try

        DataToInterface = gPMConstants.PMEReturnCode.PMTrue
        If (IsArray(m_vTemplateArray)) Then
            For iTemplateCount = 0 To UBound(m_vTemplateArray, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Not (Val(m_vTemplateArray(iTEMPLATE_ID, iTemplateCount)) = 0) Then
                    List1.Items.Add((m_vTemplateArray(iCODE, iTemplateCount)))
                Else
                    Select Case (m_vTemplateArray(iENTITY_TYPE_ID, iTemplateCount))
                        Case 1
                            sDescription = "Client Text File "
                        Case 2
                            sDescription = "Policy Text File "
                    End Select
                    'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sDescription = sDescription & m_vTemplateArray(iFILE_NUMBER, iTemplateCount)
                    List1.Items.Add((sDescription))
                End If
            Next iTemplateCount
        End If

        Exit Function

        Catch ex As Exception

        DataToInterface = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    Private Sub cmdConvert_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdConvert.Click
        cmdConvert.Enabled = False
        cmdExit.Enabled = False
        ConvertTemplates()
        cmdConvert.Enabled = True
        cmdExit.Enabled = True
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    'UPGRADE_NOTE: Form_Initialize was upgraded to Form_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Form_Initialize_Renamed()

        Dim sMessage As String
        Dim sTitle As String

        ' Forms initialise event.

        Try

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Initialise the error number value.
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        ' Set the interface status to cancelled. This is done
        ' so that any interface termination will be noted
        ' as cancelled except in the event of accepting
        ' the interface.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Exit Sub


        Catch ex As Exception

        m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Sub


        End Try
    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

        ' Check if we have had an error so far.
        ' Possibly creating the business object.
        If (m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse) Then
            ' We have already encountered an error,
            ' so we MUST exit now.
            Exit Sub
        End If

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Set the business keys.
        ' {* USER DEFINED CODE (Begin) *}
        ' {* USER DEFINED CODE (End) *}

        ProgressBar1.Value = 0
        ProgressBar1.Min = 0

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Exit Sub


        Catch ex As Exception

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Sub

        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
        ' Forms query unload event.

        Try

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Check if the interface has been terminated by means
        ' other than pressing the command buttons.
        'UPGRADE_ISSUE: Constant vbFormCode was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
        'If (UnloadMode <> vbFormCode) Then
        '    ' Process the next set of actions depending
        '    '        ' upon the interface task etc.
        '    '        m_lReturn& = m_oGeneral.ProcessCommand()

        '    ' Check the return value.
        '    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
        '        ' Do not procced with the interface termination.
        '        Cancel = 1

        '        ' Set the mouse pointer to normal.
        '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        '        Exit Sub
        '    End If
        'End If

        '    ' Terminate the general object.
        '    m_lReturn& = m_oGeneral.Terminate()
        '
        '    ' Check for errors.
        '    If (m_lReturn& <> PMTrue) Then
        '        m_lErrorNumber& = PMFalse
        '    End If
        '
        '    ' Destroy the instance of the general object
        '    ' from memory.
        '    Set m_oGeneral = Nothing

        '    ' Terminate the business object
        '    m_lReturn& = m_oBusiness.Terminate()

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload")
        End If

        '    ' Destroy the instance of the business object
        '    ' from memory.
        '    Set m_oBusiness = Nothing

        ' Reset the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        Catch ex As Exception

        m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Sub

        eventArgs.Cancel = Cancel
        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Try

        GetInterfaceDetails = gPMConstants.PMEReturnCode.PMTrue

        ' Check the task.
        If (Me.Task = gPMConstants.PMEComponentAction.PMEdit Or Me.Task = gPMConstants.PMEComponentAction.PMView Or Me.Task = gPMConstants.PMEComponentAction.PMDelete) Then
            ' Get the interface details from the
            ' business object.
            m_lReturn = Me.GetBusiness()

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Assign the details from the business object
            ' to the interface.
            m_lReturn = Me.DataToInterface()

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to assign the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If


        '    ' Check the task.
        '    If (frmInterface.Task = PMView) Or (frmInterface.Task = PMDelete) Then
        '            ' Disable the interface to only allow viewing.
        '            m_lReturn& = DisableForm( _
        ''                lDisabled:=True)
        '
        '            ' Check for errors.
        '            If (m_lReturn& <> PMTrue) Then
        '                ' Failed to disable the interface
        '                GetInterfaceDetails = PMFalse
        '            End If
        '    End If

        m_lReturn = GetServer()

        m_lReturn = GetClient()

        Exit Function

        Catch ex As Exception

        ' Error Section.

        GetInterfaceDetails = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessTemplates
    '
    ' Description: Oversees conversion of templates. Writes status
    '               information to log file.
    '
    ' History: 25/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function ConvertTemplates() As Integer
        Dim iTemplateCount As Short
        Dim sFileName As String
        Dim oTemplate As Microsoft.Office.Interop.Word.Document
        Dim iNoOfFilesProcessed As Short
        Dim lTimeTaken As Integer
        Dim lStart As Integer
        Dim iTotalTemplates As Short
        Dim sLogFile As String
        Dim iErrorCount As Short
        Dim iTemplateId As Short

        Try

        ConvertTemplates = gPMConstants.PMEReturnCode.PMTrue

        m_oWord = New Microsoft.Office.Interop.Word.Application

        oTemplate = m_oWord.Documents.Add
        g_oLog.WriteLog("")
        g_oLog.WriteLog("")
        g_oLog.WriteLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>.")
        g_oLog.WriteLog(">>>> Processing started >>>>.")
        g_oLog.WriteLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>.")
        lStart = VB.Timer()

        EnsureClientDirectoryClear()

        iNoOfFilesProcessed = 0
        ProgressBar1.Max = UBound(m_vTemplateArray, 2) + 1

        'Convert blank template.
        m_lReturn = CopyServerToClient(0, 0)
        If (CheckFileTypeIsDoc() = gPMConstants.PMEReturnCode.PMTrue) Then
            If (SaveDocumentAsHTML(oTemplate, 0) = gPMConstants.PMEReturnCode.PMTrue) Then
                g_oLog.WriteLog("Template successfully saved as HTML.")
            End If
            If (CopyClientToServer(0, 0) = gPMConstants.PMEReturnCode.PMTrue) Then
                g_oLog.WriteLog("Converted template successfully copied back to server.")
            End If
        End If
        'Remove file just processed.
        Call DeleteClient(m_sClient, sFileName)

        'Loop through all templates retrieved and process.
        For iTemplateCount = 0 To UBound(m_vTemplateArray, 2)
            'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Not (Val(m_vTemplateArray(iTEMPLATE_ID, iTemplateCount)) = 0) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iTemplateId = m_vTemplateArray(iTEMPLATE_ID, iTemplateCount)
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iTemplateId = m_vTemplateArray(iFILE_NUMBER, iTemplateCount)
            End If

            '        If iTemplateId = 46 Then
            '            Stop
            '        End If
            sFileName = "Doc " & iTemplateId
            g_oLog.WriteLog("")
            g_oLog.WriteLog("**** " & sFileName & " ****")

            'Step1 - Get next file from server.
            'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Not (Val(m_vTemplateArray(iTEMPLATE_ID, iTemplateCount)) = 0) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = CopyServerToClient(m_vTemplateArray(iTYPE_ID, iTemplateCount), iTemplateId)
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(iSLOT, iTemplateCount). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = CopyServerToClientText(m_vTemplateArray(iENTITY_TYPE_ID, iTemplateCount), iTemplateId, m_vTemplateArray(iSLOT, iTemplateCount))
            End If
            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                System.Windows.Forms.Application.DoEvents()
                'Must ensure '.doc' file is same no. as zip file and not '0' as when created.
                Call CheckFileHasCorrectName(m_sClient, iTemplateId)

                'Step2 - If copied down successfully check it is a .doc before bothering to convert it.
                If (CheckFileTypeIsDoc() = gPMConstants.PMEReturnCode.PMTrue) Then

                    'Keep a count of the number of files we actually process.
                    iNoOfFilesProcessed = iNoOfFilesProcessed + 1

                    'Step3 - Replace all bookmarks in document with appropriate merge fields.
                    m_lReturn = ConvertDocument(oTemplate, iTemplateId)
                    If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                        g_oLog.WriteLog("Bookmarks replaced successfully.")
                        System.Windows.Forms.Application.DoEvents()

                        'Step4 - Save the template in HTML format.
                        m_lReturn = SaveDocumentAsHTML(oTemplate, iTemplateId)
                        If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                            g_oLog.WriteLog("Template successfully saved as HTML.")
                            System.Windows.Forms.Application.DoEvents()

                            'Step5 - Zip the converted template and copy back to server.
                            'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If Not (Val(m_vTemplateArray(iTEMPLATE_ID, iTemplateCount)) = 0) Then
                                'UPGRADE_WARNING: Couldn't resolve default property of object m_vTemplateArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                m_lReturn = CopyClientToServer(m_vTemplateArray(iTYPE_ID, iTemplateCount), iTemplateId)
                            Else
                                m_lReturn = CopyClientToServerText(iTemplateId)
                            End If
                            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                                g_oLog.WriteLog("Converted template successfully copied back to server.")
                            Else
                                Call g_oLog.WriteLog("Failed to copy template back to server.", True)
                                Call g_oLog.WriteLog(m_lErrorNumber & " - " & m_sErrorDesc, True)
                                iErrorCount = iErrorCount + 1
                            End If
                        Else
                            Call g_oLog.WriteLog("Failed to save as HTML.", True)
                            Call g_oLog.WriteLog(m_lErrorNumber & " - " & m_sErrorDesc, True)
                            iErrorCount = iErrorCount + 1
                        End If
                    Else
                        Call g_oLog.WriteLog("Failed to replace bookmarks.", True)
                        Call g_oLog.WriteLog(m_lErrorNumber & " - " & m_sErrorDesc, True)
                        iErrorCount = iErrorCount + 1
                    End If
                End If
                System.Windows.Forms.Application.DoEvents()
                'Remove file just processed.
                Call DeleteClient(m_sClient, sFileName)

            Else
                Call g_oLog.WriteLog("Failed to copy template from server.", True)
                Call g_oLog.WriteLog(m_lErrorNumber & " - " & m_sErrorDesc, True)
                iErrorCount = iErrorCount + 1
            End If

            ProgressBar1.Value = iTemplateCount + 1
            System.Windows.Forms.Application.DoEvents()
        Next iTemplateCount
        'UPGRADE_NOTE: Object m_oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        m_oWord = Nothing

        lTimeTaken = VB.Timer() - lStart
        iTotalTemplates = UBound(m_vTemplateArray, 2) + 1

        'Write summary of processing to log.
        g_oLog.WriteLog("")
        g_oLog.WriteLog("Processing completed.")
        g_oLog.WriteLog("SUMMARY")
        g_oLog.WriteLog(iTotalTemplates & " templates downloaded from server.")
        g_oLog.WriteLog(iNoOfFilesProcessed & " templates processed.")
        g_oLog.WriteLog(iErrorCount & " errors encountered.")
        g_oLog.WriteLog("Time taken = " & lTimeTaken & " seconds.")

        'Establish full path and name of log file.
        sLogFile = g_oLog.Directory
        If (VB.Right(sLogFile, 1) <> "\") Then
            sLogFile = sLogFile & "\"
        End If
        sLogFile = sLogFile & g_oLog.FileName

        'Inform user that processing is complete and offer option of immediately
        'viewing log file.
        Dim RetVal As Object
        If MsgBox("Processing complete, for details see log file " & vbCrLf & vbCrLf & """" & sLogFile & """." & vbCrLf & vbCrLf & "Do you wish to view the log file now ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Document Conversion Utility") = MsgBoxResult.Yes Then


            'Launch log file if requested by user.
            'UPGRADE_WARNING: Couldn't resolve default property of object RetVal. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            RetVal = Shell("NOTEPAD.EXE " & sLogFile, 1)
        End If

        'Reset progress bar.
        ProgressBar1.Value = 0

        Exit Function

        Catch ex As Exception

        ConvertTemplates = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertTemplates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertTemplates", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClient
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetClient() As Integer

        Dim sClient As String

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

        GetClient = gPMConstants.PMEReturnCode.PMTrue

        If (Trim(m_sClient) > "") Then
            Exit Function
        End If

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        sClient = ""

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocClient", r_sSettingValue:=sClient)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Client from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClient", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            GetClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        Else
            'Tomo290200
            'For citrix, when everyone has the same client PC.
            '        m_sClient = sClient
            m_sClient = sClient & "\" & Trim(g_oObjectManager.UserName)
        End If

        Exit Function

        Catch ex As Exception

        GetClient = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClient Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClient", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetServer() As Integer

        Dim sServer As String

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

        GetServer = gPMConstants.PMEReturnCode.PMTrue

        If (Trim(m_sServer) > "") Then
            Exit Function
        End If

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        sServer = ""

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            GetServer = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        Else
            m_sServer = sServer
        End If

        Exit Function

        Catch ex As Exception

        GetServer = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyClientToServer
    '
    ' Description: copies the template from the client to the server
    '
    ' ***************************************************************** '
    Public Function CopyClientToServer(ByVal lTypeId As Integer, ByVal lTemplateId As Object) As Integer

        Dim sServer As String
        Dim sClient As String
        Dim sTemp As String

        Try

        CopyClientToServer = gPMConstants.PMEReturnCode.PMTrue

        CopyFilesToZipTemp()

        'Make sure the server directory's there
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(m_sServer, FileAttribute.Directory)
        If (sTemp = "") Then
            MkDir(m_sServer)
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object lTemplateId. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (lTemplateId = 0) Then
            sServer = m_sServer & "\Blank.zip"
        Else
            sServer = m_sServer & "\Type " & lTypeId

            'Make sure the directory's there
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            sTemp = Dir(sServer, FileAttribute.Directory)
            If (sTemp = "") Then
                MkDir(sServer)
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object lTemplateId. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sServer = sServer & "\Doc " & lTemplateId & ".zip"
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object lTemplateId. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sClient = m_sZIP_DIRECTORY & "\Doc " & lTemplateId & ".zip"

        m_lReturn = Zip(sClient)

        FileCopy(sClient, sServer)

        'Delete the local copy
        Kill(sClient)

        Exit Function


        Catch ex As Exception

        CopyClientToServer = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as process may be unattended.

        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

        '    ' Log Error.
        '    iPMFunc.LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="Failed to copy template from client to server", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="CopyClientToServer", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveDocumentAsHTML
    '
    ' Description:
    '
    ' History: 25/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function SaveDocumentAsHTML(ByRef oTemplate As Microsoft.Office.Interop.Word.Document, ByVal lFileNo As Integer) As Integer
        Dim sFileName As String
        Dim iFormat As Short

        '    iFormat = FileConverters("HTML").SaveFormat
        '    myDocName = ActiveDocument.Name
        '    pos = InStr(myDocName, ".")
        '    If pos > 0 Then
        '        myDocName = Left(myDocName, pos - 1)
        '        myDocName = myDocName & ".html"
        '        ActiveDocument.SaveAs filename:=myDocName, FileFormat:=intFormat
        '    End If

        Try

        SaveDocumentAsHTML = gPMConstants.PMEReturnCode.PMTrue

        sFileName = m_sClient & "\" & "Doc " & lFileNo & ".htm"

        oTemplate.SaveAs(sFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML)
        oTemplate.Close()

        Exit Function

        Catch ex As Exception

        SaveDocumentAsHTML = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as process may be unattended.

        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

        '    ' Log Error Message
        '    iPMFunc.LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="SaveDocumentAsHTML Failed", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="SaveDocumentAsHTML", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description

        Exit Function

        End Try
    End Function

    Private Function Zip(ByRef sPath As String) As Integer

        Dim iTemp As Short
        Dim sFileIn As String
        Dim sFileOut As String
        Dim sDependencyDir As String
        Dim sDependency As String
        Dim sParentFile As String

        Try

        'RWH(28/07/2000) Changed from doc to htm.
        '    sFileIn = Left$(sPath, Len(sPath) - 3) & "doc"

        sFileIn = VB.Left(sPath, Len(sPath) - 3) & "htm"
        sFileOut = sPath

        'UPGRADE_WARNING: Couldn't resolve default property of object g_oZipper.ZipFile(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iTemp = g_oZipper.ZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)

        If (iTemp = False) Then
            Zip = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'RWH(30/08/2000) RSAIB Process 108. Enable zipping up of multiple files
        'to include HTML dependencies.
        'Check for dependencies and add them to the zip file if they exist.
        sParentFile = VB.Left(sFileIn, Len(sPath) - 4)

        sDependencyDir = sParentFile & "_files"
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(sDependencyDir, FileAttribute.Directory) <> "" Then
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            sDependency = Dir(sDependencyDir & "\*.*")
            Do While (sDependency <> "")
                iTemp = g_oZipper.addFileToZIP(sFileOut, sDependencyDir & "\" & sDependency)
                Kill(sDependencyDir & "\" & sDependency)
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                sDependency = Dir()
            Loop
            RmDir(sDependencyDir)

        End If

        Kill(sFileIn)

        Exit Function

        Catch ex As Exception

        Zip = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to zip the document", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function
    ' Open a document, resolve the fields, and return the
    ' document object
    Private Function ConvertDocument(ByRef oDocument As Microsoft.Office.Interop.Word.Document, ByVal lFileNo As Integer) As Integer

        Dim oActiveWindow As Microsoft.Office.Interop.Word.Window
        Dim oBookmark As Microsoft.Office.Interop.Word.Bookmark
        Dim oBookmarks As Microsoft.Office.Interop.Word.Bookmarks
        Dim sFieldCode As String
        Dim sNewMergeField As String
        Dim iSep As Short
        Dim sFieldType As String
        Dim sFieldName As String
        Dim sQuestion As String
        Dim sAnswer As String
        Dim sFileName As String

        Select Case Err.Number
            Case Is < 0
                Error (5)
            Case 1
                GoTo Err_ConvertDocument
        End Select

        ConvertDocument = gPMConstants.PMEReturnCode.PMTrue

        sFileName = m_sClient & "\" & "Doc " & lFileNo & ".doc"

        ' Open the chosen template document
        oDocument = m_oWord.Documents.Open(sFileName)

        ' Get the active window
        oActiveWindow = oDocument.ActiveWindow

        ' Get the bookmarks collection
        oBookmarks = oDocument.Bookmarks

        If (oBookmarks.Count = 0) Then
            'UPGRADE_NOTE: Object oBookmarks may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oBookmarks = Nothing
            'UPGRADE_NOTE: Object oActiveWindow may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oActiveWindow = Nothing
            '        Set oDocument = Nothing
            Exit Function
        End If

        ' Reget the bookmarks collection
        oBookmarks = oDocument.Bookmarks

        If (oBookmarks.Count = 0) Then
            'UPGRADE_NOTE: Object oBookmarks may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oBookmarks = Nothing
            'UPGRADE_NOTE: Object oActiveWindow may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oActiveWindow = Nothing
            '        Set oDocument = Nothing
            Exit Function
        End If

        ' Load the bookmarks into an array
        For Each oBookmark In oBookmarks

            ' Get the field code for the bookmark
            sFieldCode = oBookmark.Name

            ' Determine the field type
            iSep = InStr(sFieldCode, "_")
            sFieldType = VB.Left(sFieldCode, iSep - 1)

            ' Select the bookmark so it can be overwritten.
            oBookmark.Select()

            Select Case sFieldType

                Case DbTag
                    ' extract the field name
                    sFieldName = VB.Right(sFieldCode, Len(sFieldCode) - iSep)

                    ' Strip off the file name at the beginning
                    iSep = InStr(sFieldName, "_")
                    If (iSep > 0) Then
                        sFieldName = Mid(sFieldName, iSep + 1)
                    End If

                    ' Strip off the id character at the end
                    iSep = InStr(sFieldName, "_")
                    If (iSep > 0) Then
                        sFieldName = VB.Left(sFieldName, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = m_sFIELD_START_MARKER & DbTag & Separator & sFieldName & m_sFIELD_END_MARKER

                    oActiveWindow.Selection.Text = sNewMergeField

                Case QuestionTag

                    sQuestion = oActiveWindow.Selection.Text

                    'Construct new merge field
                    sNewMergeField = m_sFIELD_START_MARKER & QuestionTag & Separator & sQuestion & m_sFIELD_END_MARKER

                    oActiveWindow.Selection.Text = sNewMergeField

                Case LoopTag
                    ' extract the field name
                    sFieldName = VB.Right(sFieldCode, Len(sFieldCode) - iSep)

                    ' Strip off the file name at the beginning
                    iSep = InStr(sFieldName, "_")
                    If (iSep > 0) Then
                        sFieldName = Mid(sFieldName, iSep + 1)
                    End If

                    ' Strip off the id character at the end
                    iSep = InStr(sFieldName, "_")
                    If (iSep > 0) Then
                        sFieldName = VB.Left(sFieldName, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = m_sFIELD_START_MARKER & LoopTag & Separator & sFieldName & m_sFIELD_END_MARKER

                    oActiveWindow.Selection.Text = sNewMergeField


                Case EndLoopTag
                    ' extract the field name
                    sFieldName = VB.Right(sFieldCode, Len(sFieldCode) - iSep)

                    ' Strip off the file name at the beginning
                    iSep = InStr(sFieldName, "_")
                    If (iSep > 0) Then
                        sFieldName = Mid(sFieldName, iSep + 1)
                    End If

                    ' Strip off the id character at the end
                    iSep = InStr(sFieldName, "_")
                    If (iSep > 0) Then
                        sFieldName = VB.Left(sFieldName, iSep - 1)
                    End If

                    'Construct new merge field
                    sNewMergeField = m_sFIELD_START_MARKER & EndLoopTag & Separator & sFieldName & m_sFIELD_END_MARKER

                    oActiveWindow.Selection.Text = sNewMergeField


                Case Else
                    'oBookmark.Range.Text = "Invalid Bookmark"

            End Select

        Next oBookmark

        'Update the fields
        WordGlobal_definst.ActiveDocument.Fields.Update()

        ' Return the document

        'UPGRADE_NOTE: Object oBookmark may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oBookmark = Nothing
        'UPGRADE_NOTE: Object oBookmarks may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oBookmarks = Nothing
        'UPGRADE_NOTE: Object oActiveWindow may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oActiveWindow = Nothing

        Exit Function

Err_ConvertDocument:

        ConvertDocument = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as process may be unattended.

        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

        '    ' Log Error.
        '    iPMFunc.LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="Failed to convert the document", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="ConvertDocument", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description

        Exit Function

    End Function


    ' ********************************************************************** '
    ' Name: CopyServerToClient
    '
    ' Description: copies the template from the server to the client
    '
    ' Changes:     RWH(18/08/2000) - parameterised to enable use in Clauses.
    ' ********************************************************************** '
    Private Function CopyServerToClient(ByVal lTypeId As Integer, ByVal lTemplateId As Integer) As Integer

        Dim sServer As String
        Dim sClient As String
        Dim sTemp As String
        Dim sTemp2 As String

        Try

        CopyServerToClient = gPMConstants.PMEReturnCode.PMTrue

        'Make sure the directory's there
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(m_sClient, FileAttribute.Directory)
        If (sTemp = "") Then
            MkDir(m_sClient)
        End If

        If (lTemplateId = 0) Then
            sServer = m_sServer & "\Blank.zip"
        Else
            sServer = m_sServer & "\Type " & lTypeId & "\Doc " & lTemplateId & ".zip"
        End If

        '    sClient = m_sClient & "\Doc " & lTemplateId & ".zip"
        sClient = m_sZIP_DIRECTORY & "\Doc " & lTemplateId & ".zip"

        'Make sure the file's not there
        '    sTemp = m_sClient & "\Doc " & lTemplateId & ".doc"
        sTemp = m_sZIP_DIRECTORY & "\Doc " & lTemplateId & ".doc"

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp2 = Dir(sTemp, FileAttribute.Normal)
        If (sTemp2 <> "") Then
            Kill(sTemp)
        End If

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(m_sZIP_DIRECTORY, FileAttribute.Directory) = "" Then
            MkDir((m_sZIP_DIRECTORY))
        End If

        g_oLog.WriteLog("COPYING '" & sServer & "' TO '" & sClient & "'....")

        FileCopy(sServer, sClient)

        '    m_lReturn = UnZip(sClient)
        m_lReturn = UnZip(m_sZIP_DIRECTORY, lTemplateId)

        'Copy unzipped file to m_sClient
        '    FileCopy sTemp, m_sClient
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(m_sZIP_DIRECTORY & "\")

        FileCopy(m_sZIP_DIRECTORY & "\" & sTemp, m_sClient & "\" & sTemp)

        Call DeleteClient(m_sZIP_DIRECTORY, sTemp)

        Exit Function

        Catch ex As Exception

        CopyServerToClient = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as may be unattended.

        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

        '    ' Log Error.
        '    iPMFunc.LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="Failed to copy template from server to client", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="CopyServerToClient", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description

        Exit Function

        End Try
    End Function

    Private Function UnZip(ByRef sPath As String, ByRef lTemplateId As Integer) As Integer

        Dim iTemp As Short
        Dim sFileIn As String
        Dim sFileOut As String
        Dim sTemp As String
        Dim bSuccess As Boolean

        Try

        '    sFileIn = sPath
        sFileIn = sPath & "\Doc " & lTemplateId & ".zip"

        sFileOut = VB.Left(sFileIn, Len(sFileIn) - 3) & "doc"

        'Tomo290200
        'Make sure the file's _not_ there
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(sFileOut, FileAttribute.Normal)
        If (sTemp <> "") Then
            Kill(sFileOut)
        End If

        '    iTemp% = g_oZipper.UnZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)
        bSuccess = g_oZipper.UnZipFiles(sZipFileName:=sFileIn, sDestDirectory:=sPath, sFileSpec:="*.*", bNoDirectoryNamesFlag:=True)

        If (bSuccess = False) Then
            UnZip = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Kill(sFileIn)


        Exit Function


        Catch ex As Exception

        UnZip = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip the document", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckFileType
    '
    ' Description: Opens file downloaded from server & checks first 2
    '               bytes to ensure this is a standard .doc document
    '               This is to see whether it is necessary to carry
    '               out a conversion. Unzip routine always renames
    '               unzipped file to .doc so checking extension is no good.
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckFileTypeIsDoc() As Integer
        Dim sFile As String
        Dim iFileCount As Short
        Dim iFileNum As Short
        Dim sLine As String

        Try

        CheckFileTypeIsDoc = gPMConstants.PMEReturnCode.PMFalse

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sFile = Dir(m_sClient & "\*.*")
        If (sFile <> "") Then
            iFileCount = 1
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Do While (Dir() <> "")
                iFileCount = iFileCount + 1
            Loop
        End If

        If iFileCount > 1 Then
            'Error
            g_oLog.WriteLog("Too many files in " & m_sClient)
            Exit Function

        Else
            iFileNum = FreeFile()
            FileOpen(iFileNum, m_sClient & "\" & sFile, OpenMode.Binary)

            sLine = InputString(iFileNum, 2)

            Select Case UCase(sLine)
                Case Chr(208) & Chr(207) 'This is always the first 2 bytes of any Word Doc.
                    CheckFileTypeIsDoc = gPMConstants.PMEReturnCode.PMTrue
                    g_oLog.WriteLog("Template '" & sFile & "' successfully copied from server.")

                Case "<H"
                    g_oLog.WriteLog("Template '" & sFile & "' was already in the required format and was not processed.")

                Case Else
                    g_oLog.WriteLog("WARNING : File '" & sFile & "' was of an unexpected format and was not processed.")

            End Select

        End If
        FileClose(iFileNum)


        Exit Function

        Catch ex As Exception

        CheckFileTypeIsDoc = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as may be unattended.

        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

        Call g_oLog.WriteLog("Invalid file format.", True)
        Call g_oLog.WriteLog(m_lErrorNumber & " - " & m_sErrorDesc, True)

        '    ' Log Error Message
        '    iPMFunc.LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="CheckFileType Failed", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="CheckFileType", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EnsureClientDirectoryClear
    '
    ' Description: Ensures document processing directory is clear. Any
    '               files in this directory will be copied to a
    '               date-stamped backup directory and deleted from the
    '               processing directory.
    '
    ' History: 29/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function EnsureClientDirectoryClear() As Integer
        Dim sFile As String
        Dim sBackupDirectory As String
        Dim iFileCount As Short
        Dim sFileArray() As String
        Dim iLoop As Short

        Try

        EnsureClientDirectoryClear = gPMConstants.PMEReturnCode.PMTrue

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sFile = Dir(m_sClient & "\*.*")
        If (sFile <> "") Then
            'If files exist in our processing directory, create a backup directory
            'to move them to.
            sBackupDirectory = VB6.Format(Now, "yyyymmddhhmmss") & "_bak\"
            MkDir(m_sClient & "\" & sBackupDirectory)

            Do While (sFile <> "")
                'Create array of file names for logging.
                ReDim Preserve sFileArray(iFileCount)
                sFileArray(iFileCount) = sFile

                'Copy file to backup directory.
                FileCopy(m_sClient & "\" & sFile, m_sClient & "\" & sBackupDirectory & sFile)
                iFileCount = iFileCount + 1
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                sFile = Dir()
            Loop

            Kill(m_sClient & "\*.*")

            'Log what files have been moved. This can't be odne in loop above betwee Dir
            'commands as this messes up the working of Dir.
            g_oLog.WriteLog("")
            If (iFileCount = 1) Then
                g_oLog.WriteLog("WARNING - The following file has been moved from")
            Else
                g_oLog.WriteLog("WARNING - The following " & iFileCount & " files have been moved from")
            End If
            g_oLog.WriteLog(m_sClient & "\")
            g_oLog.WriteLog("To")
            g_oLog.WriteLog(m_sClient & "\" & sBackupDirectory)
            For iLoop = 0 To iFileCount - 1
                g_oLog.WriteLog(sFileArray(iLoop))
            Next iLoop

        End If

        'Empty everything from temp zip directory. We don't care what's in there.
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If (Dir(m_sZIP_DIRECTORY & "\*.*") <> "") Then
            Kill(m_sZIP_DIRECTORY & "\*.*")
        End If

        Exit Function

        Catch ex As Exception

        EnsureClientDirectoryClear = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnsureClientDirectoryClear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnsureClientDirectoryClear", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    'Private Sub frmInterface_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    '    'UPGRADE_NOTE: Object frmInterface may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
    '    Me = Nothing
    'End Sub

    ' ***************************************************************** '
    ' Name: CopyServerToClientText
    '
    ' Description: copies a text template from the server to the client
    '               (Client Text Files, Policy Text Files.)
    '
    ' ***************************************************************** '
    Public Function CopyServerToClientText(ByVal lEntityTypeId As Integer, ByVal lFileNumber As Integer, ByVal lSlot As Integer) As Integer

        Dim sServer As String
        Dim sClient As String
        Dim sTemp As String

        Try

        CopyServerToClientText = gPMConstants.PMEReturnCode.PMTrue

        'If lFileNumber = 2 Then Stop

        m_lReturn = GetTextPath(lEntityTypeId, lFileNumber, lSlot)

        'Make sure the directory's there
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(m_sClient, FileAttribute.Directory)
        If (sTemp = "") Then
            MkDir(m_sZIP_DIRECTORY)
        End If

        '    sClient = m_sClient & "\Doc " & lFileNumber & ".zip"
        sClient = m_sZIP_DIRECTORY & "\Doc " & lFileNumber & ".zip"

        'Tomo290200
        'Make sure the file's _not_ there
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(sClient, FileAttribute.Normal)
        If (sTemp <> "") Then
            Kill(m_sZIP_DIRECTORY)
        End If

        g_oLog.WriteLog("COPYING '" & m_sTextServer & "' TO '" & sClient & "'....")

        FileCopy(m_sTextServer, sClient)

        m_lReturn = UnZip(m_sZIP_DIRECTORY, lFileNumber)

        sClient = VB.Left(sClient, Len(sClient) - 4) & ".doc"

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If (Dir(sClient) = "") Then
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            sTemp = Dir(m_sZIP_DIRECTORY & "\*.*")
            If sTemp <> "" Then
                Rename((m_sZIP_DIRECTORY & "\" & sTemp), sClient)
            End If
        End If

        'Make sure the directory's there
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(m_sClient, FileAttribute.Directory)
        If (sTemp = "") Then
            MkDir(m_sClient)
        End If

        'Copy unzipped file to m_sClient
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sTemp = Dir(m_sZIP_DIRECTORY & "\")

        FileCopy(m_sZIP_DIRECTORY & "\" & sTemp, m_sClient & "\" & sTemp)

        Call DeleteClient(m_sZIP_DIRECTORY, sTemp)

        Exit Function

        Catch ex As Exception

        CopyServerToClientText = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as may be unattended.

        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

        '    ' Log Error.
        '    iPMFunc.LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="Failed to copy template from server to client", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="CopyServerToClientText", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description

        Exit Function

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPath
    '
    ' Description:
    '
    ' History: 24/08/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetTextPath(ByRef lEntityTypeId As Integer, ByRef lFileNumber As Integer, ByRef lSlotNumber As Integer) As Integer

        Dim sTemp As String
        Dim lTemp As Integer
        Dim lTemp2 As Integer

        Try

        GetTextPath = gPMConstants.PMEReturnCode.PMTrue

        If (lFileNumber <> 0) Then
            lTemp = lFileNumber \ 500

            lTemp2 = lFileNumber - (lTemp * 500)

            Select Case (lEntityTypeId)
                Case iCLIENT
                    sTemp = "Client Text Files"
                Case iPOLICY
                    sTemp = "Policy Text Files"
                Case Else
                    'invalid
            End Select

            m_sTextServer = m_sServer & "\" & sTemp & "\Company " & g_iSourceID & "\Slot " & lSlotNumber & "\" & VB6.Format(lTemp, "000") & "\" & VB6.Format(lTemp2, "000") & ".zip"

            Exit Function
        End If

        Exit Function

        Catch ex As Exception

        GetTextPath = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTextPath", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyClientToServerText
    '
    ' Description: copies the template from the client to the server
    '
    ' ***************************************************************** '
    Public Function CopyClientToServerText(ByVal lFileNumber As Integer) As Integer

        Dim sServer As String
        Dim sClient As String
        Dim sTemp As String

        Try

        CopyClientToServerText = gPMConstants.PMEReturnCode.PMTrue

        '    m_lReturn = GetAndCreatePath(sPath:=sServer)

        CopyFilesToZipTemp()

        sClient = m_sZIP_DIRECTORY & "\Doc " & lFileNumber & ".zip"

        m_lReturn = Zip(sClient)

        FileCopy(sClient, m_sTextServer)

        'Delete the local copy
        Kill(sClient)

        Exit Function


        Catch ex As Exception

        CopyClientToServerText = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServerText", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteClient
    '
    ' Description: deletes the template from the client
    '
    ' Changes: RWH(01/08/2000) Ammended to deal with htm documents.
    '          RWH(31/08/2000) Remove dependencies if they exist.
    '
    ' ***************************************************************** '
    Public Function DeleteClient(ByRef sPath As String, ByRef sFileName As String) As Integer
        Dim sParentFile As String
        Dim sDependencyDir As String
        Dim sDependency As String
        Dim sClient As String

        Try

        DeleteClient = gPMConstants.PMEReturnCode.PMTrue

        '    sClient = sPath & "\" & sFileName & ".*"
        '    Kill sClient
        '    sClient = Dir(sPath & "\*.*")
        '    Do While (sClient <> "")
        '        Kill sPath & "\" & sClient
        '        sClient = Dir
        '    Loop

        Kill(sPath & "\*.*")

        'RWH(31/08/2000) RSAIB Process 108. Remove HTML dependencies.
        'Check for dependencies and remove them if they exist.
        sParentFile = sPath & "\" & sFileName

        sDependencyDir = sParentFile & "_files"
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(sDependencyDir, FileAttribute.Directory) <> "" Then
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            sDependency = Dir(sDependencyDir & "\*.*")
            Do While (sDependency <> "")
                Kill(sDependencyDir & "\" & sDependency)
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                sDependency = Dir()
            Loop
            RmDir(sDependencyDir)

        End If

        Exit Function

        Catch ex As Exception

        DeleteClient = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete text file from client", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClient", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CheckFileHasCorrectName
    '
    ' Description: Templates were formerly created with 'Doc 0' name
    '               and not converted to the correct number until
    '               they were first edited or merged. We need to check
    '               that the file we have unzipped has the correctname.
    '               If not, then we must convert it.
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckFileHasCorrectName(ByRef sPath As String, ByVal lCorrectFileNumber As Integer) As Integer
        Dim sFileName As String
        Dim sFileNumber As String
        Dim sCorrectedFile As String
        Dim sTemp As String

        Try

        CheckFileHasCorrectName = gPMConstants.PMEReturnCode.PMTrue

        '    If lCorrectFileNumber = 2 Then Stop

        'Get current file name.
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sFileName = Dir(sPath & "\*.*")
        If (sFileName <> "") Then
            sTemp = VB.Left(sFileName, Len(sFileName) - 4)

            'Extract file number.
            Do While (VB.Right(sTemp, 1) <> " ") And (Len(sTemp) > 0)
                sFileNumber = VB.Right(sTemp, 1) & sFileNumber
                sTemp = VB.Left(sTemp, Len(sTemp) - 1)

            Loop

            'If number is '0', convert it to correct value.
            If (Val(sFileNumber) = 0) Then
                sCorrectedFile = sTemp & CStr(lCorrectFileNumber) & ".doc"
                Rename((sPath & "\" & sFileName), (sPath & "\" & sCorrectedFile))

            End If
        End If

        Exit Function

        Catch ex As Exception

        CheckFileHasCorrectName = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileHasCorrectName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileHasCorrectName", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyFilesToZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyFilesToZipTemp() As Integer
        Dim sClient As String
        Dim sDependencyDir As String
        Dim sDependency As String
        Dim sParentFile As String
        Dim sDepDirName As String

        Try

        CopyFilesToZipTemp = gPMConstants.PMEReturnCode.PMTrue

        'Copy files to TempZip.
        'This gives us an absolute directory to zip & unzip to as apposed to the
        'client-specific processing directory.
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        sClient = Dir(m_sClient & "\*.htm")
        If (sClient <> "") Then

            'Copy parent file to ZipTemp.
            Rename((m_sClient & "\" & sClient), (m_sZIP_DIRECTORY & "\" & sClient))

            'Check for dependencies and copy them to the temp zip directory if they exist.
            sParentFile = m_sClient & "\" & sClient
            sParentFile = VB.Left(sParentFile, Len(sParentFile) - 4)

            sDependencyDir = sParentFile & "_files"
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(sDependencyDir, FileAttribute.Directory) <> "" Then
                sDepDirName = VB.Left(sClient, Len(sClient) - 4) & "_files"
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                If Dir(m_sZIP_DIRECTORY & "\" & sDepDirName, FileAttribute.Directory) = "" Then
                    MkDir((m_sZIP_DIRECTORY & "\" & sDepDirName))
                End If
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                sDependency = Dir(sDependencyDir & "\*.*")
                Do While (sDependency <> "")
                    Rename((sDependencyDir & "\" & sDependency), (m_sZIP_DIRECTORY & "\" & sDepDirName & "\" & sDependency))
                    'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    sDependency = Dir()
                Loop
                RmDir(sDependencyDir)

            End If
        End If

        Exit Function

        Catch ex As Exception

        CopyFilesToZipTemp = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesToZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyFilesFromZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyFilesFromZipTemp() As Integer

        Try

        CopyFilesFromZipTemp = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

        Catch ex As Exception

        CopyFilesFromZipTemp = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesFromZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesFromZipTemp", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function
End Class
