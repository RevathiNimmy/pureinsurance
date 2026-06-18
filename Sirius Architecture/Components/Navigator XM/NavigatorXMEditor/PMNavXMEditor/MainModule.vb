Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule

    Private m_bRet As Boolean
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_iPrivilegeLevel As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sUserName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sCallingAppName As String = ""

    ' user or admin (sirius) user mode
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_lUserMode As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public Const ACApp As String = "iPMNavXMEditor"
    Private Const ACClass As String = "MainModule"

    ' top node ie. the map
    Public Const TREEVIEW_NODE_ROOT As String = "ROOT"
    Public Const TREEVIEW_NODE_ROADMAP As String = "ROADMAP"

    ' standard and 'sirius' operating mode
    Public Const USER_MODE_USER As Integer = 1
    Public Const USER_MODE_ADMIN As Integer = 2

    ' step types - users cannot edit core elements
    Public Const ROADMAP_CORE_ADMIN As Integer = 1
    Public Const ROADMAP_CORE_USER As Integer = 0

    Public Const ID_NO_VALUE As Integer = -1

    ' restore file types
    Public Const SELECTED_RESTORE_CORE As Integer = 1
    Public Const SELECTED_RESTORE_CUSTOM As Integer = 2

    ' filename info
    Public Const FILENAME_EXTENSION_EXPORT As String = "_export"
    Public Const FILENAME_SEPARATOR As String = "\"

    Public Const FILENAME_SUFFIX_STANDARD As String = ".xml"
    Public Const FILENAME_SUFFIX_EXPORT As String = ".nxm"

    ' info for export file
    Public Const EXPORT_INFO_COMPANY_NAME As String = "Sirius Financial Systems Plc"
    Public Const EXPORT_INFO_TITLE As String = "Navigator XM Editor Export file"

    Public Const MSG_MODE_NEWMAP As Integer = 0
    Public Const MSG_MODE_OLDMAP As Integer = 1
    Public Const MSG_MODE_NEWTASK As Integer = 2

    ' the roadmap element types
    Public Const ELEMENT_BASENAME_MAP As String = "MAP"
    Public Const ELEMENT_BASENAME_SUBMAP As String = "SUBMAP"
    Public Const ELEMENT_BASENAME_STEP As String = "STEP"
    Public Const ELEMENT_BASENAME_KEY As String = "KEY"

    ' map attributes
    Public Const ATTRIB_MAP_WMTASKCODE As String = "WMTaskCode"
    Public Const ATTRIB_MAP_WMTASKDESCRIPTION As String = "WMTaskDescription"
    Public Const ATTRIB_MAP_IMAGEURL As String = "ImageURL"
    Public Const ATTRIB_MAP_TRANSACTIONTYPE As String = "TransactionType"
    Public Const ATTRIB_MAP_PROCESSMODE As String = "ProcessMode"
    Public Const ATTRIB_MAP_AUTOCLOSE As String = "AutoClose"
    Public Const ATTRIB_MAP_NAVIGATORDRIVEN As String = "NavigatorDriven"
    Public Const ATTRIB_MAP_CORE As String = "Core"
    Public Const ATTRIB_MAP_TITLE As String = "Title"
    Public Const ATTRIB_MAP_ROADMAPNAME As String = "RoadmapName"
    Public Const ATTRIB_MAP_ELEMENTID As String = "ElementID"

    ' step attributes
    Public Const ATTRIB_STEP_DESCRIPTION As String = "Description"
    Public Const ATTRIB_STEP_COMPONENT As String = "Component"
    Public Const ATTRIB_STEP_TYPE As String = "Type"
    Public Const ATTRIB_STEP_CANCELACTION As String = "CancelAction"
    Public Const ATTRIB_STEP_OKACTION As String = "OKAction"
    Public Const ATTRIB_STEP_OKSTEPS As String = "OKSteps"
    Public Const ATTRIB_STEP_CANCELSTEPS As String = "CancelSteps"
    Public Const ATTRIB_STEP_COMPONENTACTION As String = "ComponentAction"
    Public Const ATTRIB_STEP_SERVERSIDE As String = "ServerSide"
    Public Const ATTRIB_STEP_CREATEWMTASK As String = "CreateWMTask"
    Public Const ATTRIB_STEP_RESUMESTEP As String = "ResumeStep"
    Public Const ATTRIB_STEP_CORE As String = "Core"
    Public Const ATTRIB_STEP_SUBMAP As String = "Submap"
    Public Const ATTRIB_STEP_ELEMENTID As String = "ElementID"
    Public Const ATTRIB_STEP_OKNEWROADMAP As String = "OKNewRoadmap"
    Public Const ATTRIB_STEP_CANCELNEWROADMAP As String = "CancelNewRoadmap"

    ' submap attributes
    Public Const ATTRIB_SUBMAP_CODE As String = "Code"
    Public Const ATTRIB_SUBMAP_DESCRIPTION As String = "Description"
    Public Const ATTRIB_SUBMAP_ELEMENTID As String = "ElementID"

    ' key attributes
    Public Const ATTRIB_KEY_NAME As String = "Name"
    Public Const ATTRIB_KEY_VALUE As String = "Value"
    Public Const ATTRIB_KEY_ELEMENTID As String = "ElementID"

    ' user-definable steps
    Public Const AVAILABLE_STEP_DIARY As String = "DIARY"
    Public Const AVAILABLE_STEP_QUESTION As String = "QUESTION"
    Public Const AVAILABLE_STEP_EDITTEXT As String = "EDITTEXT"
    Public Const AVAILABLE_STEP_RAISEEVENT As String = "RAISEEVENT"
    Public Const AVAILABLE_STEP_STANDARDLETTER As String = "STANDARDLETTER"
    Public Const AVAILABLE_STEP_LAUNCHEXE As String = "LAUNCHEXE"
    Public Const AVAILABLE_STEP_USERCOMPONENT As String = "USERCOMPONENT"

    ' element ID
    Public Const ELEMENT_ID_PREFIX As String = "E"

    ' empty roadmap, used to create roadmap from scratch
    Public Const BLANK_ROADMAP_FILENAME As String = "BLANK.XML"

    ' treeviews
    Public Const TREEVIEW_MAP As String = "tvwMap"
    Public Const TREEVIEW_RESUMESTEP As String = "tvwResumeStep"

    ' expected errors
    Public Const ERROR_LET_PROCEDURE_NOT_DEFINED As Integer = 451
    Public Const ERROR_CANCEL_SELECTED As Integer = 32755

    Public Const CHECK_VALUE_OFF As CheckState = CheckState.Unchecked
    Public Const CHECK_VALUE_ON As CheckState = CheckState.Checked

    ' icons
    Public Const ACIconFindForm As String = "StepFind"
    Public Const ACIconDataForm As String = "StepDataForm"
    Public Const ACIconQuestion As String = "StepDecision"
    Public Const ACIconNavigate As String = "Process"
    Public Const ACIconBusiness As String = "StepNoForm"
    Public Const ACIconPrint As String = "StepPrint"
    Public Const ACIconSubMap As String = "SubMap"

    Public Const ACIconDiary As String = "Diary"
    Public Const ACIconEditText As String = "EditText"
    Public Const ACIconRaiseEvent As String = "RaiseEvent"
    Public Const ACIconStandardLetter As String = "StandardLetter"
    Public Const ACIconLaunchEXE As String = "LaunchEXE"
    Public Const ACIconUserComponent As String = "UserComponent"

    Public Const PMNavComponentPrintObject As String = "PO"

    Public Structure Map
        Dim WMTaskCode As String
        Dim WMTaskDescription As String
        Dim ImageURL As String
        Dim TransactionType As String
        Dim ProcessMode As String
        Dim AutoClose As String
        Dim NavigatorDriven As String
        Dim Title As String
        Dim Core As String
        Dim RoadmapName As String
        Dim Version As String
        Dim ElementID As String
        Public Shared Function CreateInstance() As Map
            Dim result As New Map
            result.WMTaskCode = String.Empty
            result.WMTaskDescription = String.Empty
            result.ImageURL = String.Empty
            result.TransactionType = String.Empty
            result.ProcessMode = String.Empty
            result.AutoClose = String.Empty
            result.NavigatorDriven = String.Empty
            result.Title = String.Empty
            result.Core = String.Empty
            result.RoadmapName = String.Empty
            result.Version = String.Empty
            result.ElementID = String.Empty
            Return result
        End Function
    End Structure

    Public Structure Step_Renamed
        Dim Description As String
        Dim Component As String
        Dim Type As String
        Dim CancelAction As String
        Dim OKAction As String
        Dim OKSteps As String
        Dim CancelSteps As String
        Dim ComponentAction As String
        Dim ServerSide As String
        Dim CreateWMTask As String
        Dim ElementID As String
        Dim ResumeStep As String
        Dim Core As String
        Dim Submap As String
        Dim OKNewRoadmap As String
        Dim CancelNewRoadmap As String
        Public Shared Function CreateInstance() As Step_Renamed
            Dim result As New Step_Renamed
            result.Description = String.Empty
            result.Component = String.Empty
            result.Type = String.Empty
            result.CancelAction = String.Empty
            result.OKAction = String.Empty
            result.OKSteps = String.Empty
            result.CancelSteps = String.Empty
            result.ComponentAction = String.Empty
            result.ServerSide = String.Empty
            result.CreateWMTask = String.Empty
            result.ElementID = String.Empty
            result.ResumeStep = String.Empty
            result.Core = String.Empty
            result.Submap = String.Empty
            result.OKNewRoadmap = String.Empty
            result.CancelNewRoadmap = String.Empty
            Return result
        End Function
    End Structure

    Public Structure Submap
        Dim Code As String
        Dim Description As String
        Dim ElementID As String
        Public Shared Function CreateInstance() As Submap
            Dim result As New Submap
            result.Code = String.Empty
            result.Description = String.Empty
            result.ElementID = String.Empty
            Return result
        End Function
    End Structure

    Public Structure Key
        Dim Name As String
        Dim Value As String
        Dim ElementID As String
        Public Shared Function CreateInstance() As Key
            Dim result As New Key
            result.Name = String.Empty
            result.Value = String.Empty
            result.ElementID = String.Empty
            Return result
        End Function
    End Structure

    Public Sub Main_Renamed()

        Dim lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel

        Dim Procesos() As Process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
        If Procesos.Length > 1 And Process.GetCurrentProcess().StartTime <> Procesos(0).StartTime Then
            MessageBox.Show("There is already an instance of the editor running", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Exit Sub
        End If

        If Interaction.Command().ToUpper().IndexOf("MODE=SIRIUS") >= 0 Then
            ' sirius mode
            g_lUserMode = USER_MODE_ADMIN
        Else
            ' standard (customer) mode
            g_lUserMode = USER_MODE_USER
        End If

        ' Create an instance of the object manager
        g_oObjectManager = New bObjectManager.ObjectManager()

        ' Call the initialise method.
        m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to call the initialise method.

            ' Set the object manager to nothing.
            g_oObjectManager = Nothing

            Exit Sub
        End If

        ' Store the language ID from the object manager
        ' to the public variables, to enable us to use
        ' them throughout the object.
        With g_oObjectManager
            g_iLanguageID = .LanguageID
            g_iSourceID = .SourceID
        End With

        m_lReturn = CType(GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        If lPMAuthorityLevel <> gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
            MessageBox.Show("Unauthorised access." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "This program can only be used by System Administrators.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        Dim oForm As New frmInterface

        m_lReturn = CType(oForm, SSP.S4I.Interfaces.ILocalInterface).Initialise()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to initialise frmInterface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Exit Sub
        End If

        'developer guide no.68
        'm_lReturn = oForm.Load()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load frmInterface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Exit Sub
        End If

        m_lReturn = CType(oForm.ShowForm(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        oForm.Close()


    End Sub

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Returns whether the User is a Sys Admin or Supervisor
    '              or Normal User.
    '
    ' History:
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_lPMAuthorityLevel As Integer, Optional ByVal v_lUserGroupID As Integer = 0) As Integer
        Dim result As Integer = 0


        Static bIsAdministrator As Boolean
        Static vSupervisedGroups As Object


        Dim oBusiness As bPMNavXMEditor.Business
        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default the Authority to User
            r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser

            ' If we have NOT previously got the User Authority details
            ' then get them.

            If Object.Equals(vSupervisedGroups, Nothing) Then

                'Get the Business object
                Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMNavXMEditor.Business", vInstanceManager:="ClientManager")
                oBusiness = temp_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    oBusiness.Dispose()
                    oBusiness = Nothing

                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lUserGroupID", v_lUserGroupID)
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMSource.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", oDicParms:=oDict)

                    Return result
                End If


                lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)


                oBusiness.Dispose()

                oBusiness = Nothing
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If

            ' Is the User an Administrator
            If bIsAdministrator Then
                ' Yes, so set the Level and exit.
                ' Note: There is no need to check if they are a Group
                '       Supervisor as SysAdmin is a higher level authority.
                r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
                Return result
            End If

            ' Has A GroupID been supplied ?
            If v_lUserGroupID > 0 Then
                ' Do they supervise any Groups ?
                If Information.IsArray(vSupervisedGroups) Then
                    ' Yes, so check to see if they Supervise the Group supplied

                    For lRow As Integer = vSupervisedGroups.GetLowerBound(1) To vSupervisedGroups.GetUpperBound(1)
                        ' If the GroupID's match

                        If (v_lUserGroupID) = CInt(vSupervisedGroups(0, lRow)) Then
                            ' Set Authority Level to Supervisor
                            r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSupervisor
                            Exit For
                        End If
                    Next lRow
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckFileExists
    '
    ' Description: check if a file exists
    '
    ' History:
    ' ***************************************************************** '
    Public Function CheckFileExists(ByVal sFilename As String, ByRef bExists As Boolean) As Integer

        Dim result As Integer = 0
        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oFSO = New Object()

            bExists = File.Exists(sFilename)

            oFSO = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileExists failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyXMLFile
    '
    ' Description: Copy file from source to target
    '
    ' History:
    ' ***************************************************************** '
    Public Function CopyXMLFile(ByVal sSourceFile As String, ByVal sTargetFile As String) As Integer

        Dim result As Integer = 0
        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oFSO = New Object()

            File.Copy(sSourceFile, sTargetFile, True)

            oFSO = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyXMLFile failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyXMLFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Module