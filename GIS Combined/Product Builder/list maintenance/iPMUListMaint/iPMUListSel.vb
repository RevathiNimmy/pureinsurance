Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    'Developer Guide No. 69
    Public frmMaintain As frmMaintain
    Private Const ACClass As String = "frmSelectFile"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iLine As Integer

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

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


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property



    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
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
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************



            g_sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Me.Text = g_sTitle


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' KB 010801 reinstate help

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdView.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACViewCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExitCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    fraFileType.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACFileTypeCaption, _
            'iDataType:=PMResString)


            fraFileName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListFileCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
    ' Name: GetRegistrySettings (Private)
    '
    ' Description: Get defaults for dat/idx file paths etc

    ' ***************************************************************** '
    Private Function GetRegistrySettings() As Integer
        Dim result As Integer = 0


        Dim oGISListManager As bGISListManager.Form

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oGISListManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGISListManager, "bGISListManager.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oGISListManager = temp_oGISListManager

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oGISListManager Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oGISListManager.GetServerSettings(r_sServerListFilePath:=g_sServerListFilePath, r_sServerListVersion:=g_sServerListVersion, r_sServerListPrefVersion:=g_sServerListPrefVersion, r_bServerListFileCompressed:=g_bServerListFileCompressed, v_sGISDataModelCode:=g_sGISDataModelCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse


                oGISListManager.Dispose()

                oGISListManager = Nothing

                Return result
            End If


            oGISListManager.Dispose()

            oGISListManager = Nothing

            If g_sServerListFilePath.EndsWith("\") Then
                g_sServerListFilePathAndFile = g_sServerListFilePath & g_sGISDataModelCode & "List"
            Else
                g_sServerListFilePathAndFile = g_sServerListFilePath & "\" & g_sGISDataModelCode & "List"
            End If

            g_sServerListFilePathIdx = g_sServerListFilePath & ".idx"
            g_sServerListFilePathDat = g_sServerListFilePath & ".dat"

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowDetail
    '
    ' Description:
    '
    ' History: 18/04/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ShowDetail(ByRef iTask As Integer) As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim bSelected As Boolean
        Dim lVersion As Integer
        Dim sOldFile, sNewFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bSelected = False
            ' Developer Guide No. 
            frmMaintain = New frmMaintain()

            For iTemp As Integer = 0 To File1.Items.Count - 1
                bSelected = bSelected Or File1.GetSelected(iTemp)
            Next iTemp

            If Not bSelected Then
                Return result
            End If

            'Now we've got one, add one to the version, copy it, and let slip the dogs of war
            'If we're not viewing...
            If (m_iTask = gPMConstants.PMEComponentAction.PMView) Or (iTask = gPMConstants.PMEComponentAction.PMView) Then

                If g_sServerListFilePath.EndsWith("\") Then
                    sNewFile = g_sServerListFilePath & File1.FileName
                Else
                    sNewFile = g_sServerListFilePath & "\" & File1.FileName
                End If
                iTask = gPMConstants.PMEComponentAction.PMView
            Else
                If IsNumeric(g_sServerListVersion) Then
                    lVersion = CInt(g_sServerListVersion) + 1
                Else
                    lVersion = 1
                End If

                If g_sServerListFilePath.EndsWith("\") Then
                    sOldFile = g_sServerListFilePath & File1.FileName
                Else
                    sOldFile = g_sServerListFilePath & "\" & File1.FileName
                End If

                g_sServerListVersion = StringsHelper.Format(lVersion, "0000")
                g_sServerListPrefVersion = g_sServerListVersion.Substring(0, 2)

                If g_sServerListFilePath.EndsWith("\") Then
                    sNewFile = g_sServerListFilePath & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt"
                Else
                    sNewFile = g_sServerListFilePath & "\" & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt"
                End If

                If Not File.Exists(sNewFile) Then
                    File.Copy(sOldFile, sNewFile)
                    iTask = m_iTask

                End If


                'File.Copy(sOldFile, sNewFile)

                'iTask = m_iTask
            End If

                ' Assign the parameters to the interface properties.
                With frmMaintain
                .CallingAppName = m_sCallingAppName
                .Task = iTask
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                .LookupFile = sNewFile
                .NewFile = gPMConstants.PMEReturnCode.PMFalse
                ' {* USER DEFINED CODE (End) *}
            End With

            frmMaintain.ShowDialog()

            frmInterface_Load(Me, New EventArgs())

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' call help text
        ' KB 010801 There are two help files associated with this module call one
        ' from interface and the other from maintain, use ScreenID for maintain
        ' as a base address and add one to get selection text, assumes relationship
        ' between text files will be maintained - but what else can we do?
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID + 1), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            m_lReturn = CType(ShowDetail(iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process OK button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", excep:=excep)

            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        End Try

    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Try

            m_lReturn = CType(ShowDetail(iTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process View button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", excep:=excep)

            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        ' Initialise the error number value.
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

        m_lReturn = CType(GetRegistrySettings(), gPMConstants.PMEReturnCode)

        File1.Path = g_sServerListFilePath

        File1.Pattern = g_sGISDataModelCode & "List*.txt"

        File1.Refresh()

        For iTemp As Integer = 0 To File1.Items.Count - 1
            If File1.Items(iTemp) = g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt" Then
                File1.SetSelected(iTemp, True)
            End If
        Next iTemp

        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            '        optFileType(0).Enabled = False
            cmdOK.Enabled = False 'MKW010803 PN4514
        End If

    End Sub


End Class
