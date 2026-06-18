Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
Imports System.Windows.Forms

Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: 17/02/1997
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

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Private instance of the interface form.
    'Private m_frmInterface As Form
    Private m_frmInterface As frmInterface

    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sServer As String = ""

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

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

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

            ' Get the interface details from the business object.

            m_lReturn = m_frmInterface.GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the search data storage
            ' to the interface.

            m_lReturn = m_frmInterface.DataToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
                Return gPMConstants.PMEReturnCode.PMFalse
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
        Dim r_lDocId, r_lDocType As Integer
        Dim sDocPath As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.

            If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then

                ' Get string messages


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

                ' Get server path
                m_lReturn = CType(GetServer(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                m_frmInterface.getDocDetails(r_lDocId, r_lDocType)

                'MKW 07/01/02 - Only run subsquent procedures if an item was selected from search details list.


                If Not (m_frmInterface.lvwSearchDetails.SelectedItems.Count = 0) Then
                    'Item was selected from search details list, thus continue.

                    'KB 27/8/03 PN 5884 1.8.5 -> 1.8.6 catchup

                    ' JJ 05/08/2003 only do check when not creating new file
                    ' otherwise we can`t create new document templates
                    ' when the last one left is missing.


                    If m_frmInterface.Status <> gPMConstants.PMEComponentAction.PMAdd Then
                        sDocPath = m_sServer & "\Type " & CStr(r_lDocType) & "\doc " & CStr(r_lDocId) & ".zip"

                        If r_lDocType = lLETTER_TYPE_ID And FileSystem.Dir(sDocPath, FileAttribute.Normal) = "" Then
                            'Search in subdoc folder as well
                            If FileSystem.Dir(m_sServer & "\Type " & CStr(lSUBDOC_TYPE_ID) & "\Doc " & CStr(r_lDocId) & ".zip", FileAttribute.Normal) <> "" Then
                                sDocPath = m_sServer & "\Type " & CStr(lSUBDOC_TYPE_ID) & "\Doc " & CStr(r_lDocId) & ".zip"
                            End If
                        End If

                        'Get system option CCMDocProduction
                        Dim sCCMDocProduction As String = "0"
                        If m_frmInterface.DocumentTypeId <> PMBConst.PMBClauseTextFile Then ''working same as Pure for Clauses doc type
                            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem, r_sOptionValue:=sCCMDocProduction)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If

                        If String.IsNullOrEmpty(sCCMDocProduction) OrElse sCCMDocProduction = "0" Then ''only for PURE
                            'IJB 17/12/02 - Determine if file exists and present option to remove link from table if not
                            m_lReturn = CType(CheckFileExistsAndAllowLinkDeletion(sDocPath, r_lDocId), gPMConstants.PMEReturnCode)
                        End If

                        'RKS 240904 PN15082
                        result = m_lReturn

                        'RKS 011004 PN15082
                        'in case there is no items left in Search ListView
                        'stop further processing


                        If m_lReturn = gPMConstants.PMEReturnCode.PMRecordDeleted And (m_frmInterface.lvwSearchDetails.SelectedItems.Count = 0) Then

                            'No Listitem so update Interface command accordingly

                            m_frmInterface.cmdOK.Enabled = False

                            m_frmInterface.cmdEdit.Enabled = False

                            m_frmInterface.cmdDelete.Enabled = False
                            Return result

                        End If
                    End If
                End If
                'KB 27/8/03 PN 5884 1.8.5 -> 1.8.6 catchup end

                ' Update the property member from the interface.

                m_lReturn = m_frmInterface.DataToProperties()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Return result
                End If

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
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    ' INSERTED AS PART OF 1.6.9 --> 1.8.9 CATCHUP.
    ' ***************************************************************** '
    Private Function GetServer() As Integer

        Dim result As Integer = 0
        Dim sServer As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sServer.Trim() > "" Then
            Return result
        End If

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        sServer = ""

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            m_sServer = sServer
        End If

        Return result

    End Function

    Private Function CheckFileExistsAndAllowLinkDeletion(ByRef sServer As String, ByRef v_lDocID As Integer) As Integer
        Dim result As Integer = 0
        Dim oDocTemplate As bSIRDocTemplate.Business
        Dim vReferencingDocs(,) As Object
        Dim sReferencingDocs As New StringBuilder
        Dim llBound, lUBound As Integer
        Dim bReferencingDocs, bReferencingDocTemplates As Boolean
        Dim sCopiedDocCodes As New StringBuilder


        result = gPMConstants.PMEReturnCode.PMTrue

        If FileSystem.Dir(sServer, FileAttribute.Normal) = "" Then

            'File does not exist - RKS 240904
            'return PMFalse to ProcessMode Function to Stop Further
            'Processing of Edit or Delete Process

            result = gPMConstants.PMEReturnCode.PMFalse

            'Asking for removing the reference from database
            If MessageBox.Show("The document requested does not exist. " & _
                               " Would you like to remove the database reference to this document?", "Document Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:="ClientManager")
                oDocTemplate = temp_oDocTemplate

                'Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to get instance of bSIRDocTemplate.Business", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                ' determine if this document is referenced by other documents
                m_lReturn = oDocTemplate.GetDocumentTemplateWordingWordingLinks(v_lDocID, vReferencingDocs)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' if there are existing document templates referencing the template to be deleted
                    If Information.IsArray(vReferencingDocs) Then
                        ' set indicator
                        bReferencingDocs = True
                        llBound = vReferencingDocs.GetLowerBound(1)
                        lUBound = vReferencingDocs.GetUpperBound(1)
                        ' for each referencing document template
                        For lReferencingDoc As Integer = llBound To lUBound
                            'build string of referencing docs
                            sReferencingDocs.Append(CStr(vReferencingDocs(0, lReferencingDoc)) & ",")
                        Next
                        ' remove last ","
                        sReferencingDocs = New StringBuilder(sReferencingDocs.ToString().Substring(0, sReferencingDocs.ToString().Length - 1))
                        ' display message to the user indicating why document can be removed.
                        MessageBox.Show("The following document templates are still referencing " & _
                                        "the template you are attempting to remove. " & _
                                        "Please remove the following template/s first :- " & sReferencingDocs.ToString(), ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                ' determine if this document template has other copies
                If Not bReferencingDocs Then
                    m_lReturn = oDocTemplate.GetCopiesForDocumentTemplate(v_lDocID, vReferencingDocs)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        ' if there are copies for this document templates then it can not be deleted
                        If Information.IsArray(vReferencingDocs) Then
                            ' set indicator
                            bReferencingDocTemplates = True
                            llBound = vReferencingDocs.GetLowerBound(1)
                            lUBound = vReferencingDocs.GetUpperBound(1)

                            sCopiedDocCodes = New StringBuilder("")
                            For lReferencingDoc As Integer = llBound To lUBound

                                sCopiedDocCodes.Append(CStr(vReferencingDocs(0, lReferencingDoc)).Trim() & ",")
                            Next

                            ' remove last ","
                            sCopiedDocCodes = New StringBuilder(sCopiedDocCodes.ToString().Substring(0, sCopiedDocCodes.ToString().Length - 1))

                            ' display message to the user indicating why document can be removed.
                            MessageBox.Show("The following document templates has one or more copies as " & sCopiedDocCodes.ToString() & _
                                            " the template you are attempting to remove. " & _
                                            "Please remove all the copies first.", "Document Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If

                If Not bReferencingDocs And Not bReferencingDocTemplates Then
                    m_lReturn = oDocTemplate.DeleteDocumentLink(v_lDocID)

                    oDocTemplate.Dispose()
                    oDocTemplate = Nothing

                   
                        m_frmInterface.lvwSearchDetails.Items.Remove(m_frmInterface.lvwSearchDetails.SelectedItems(0))
                End If

            End If
        End If

        Return result

    End Function
End Class

