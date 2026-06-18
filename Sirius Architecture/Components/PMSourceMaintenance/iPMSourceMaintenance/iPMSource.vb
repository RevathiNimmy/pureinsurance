Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    'Developer Guide No. 50
    Dim frmList As New frmList
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 11th July 1997
    '
    ' Description: Main public class to acSource the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sNavigatorTitle As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' to Return to caller
    Private m_iSelectedSourceID As Integer

    Private m_lPMAuthorityLevel As Integer

    ' PrivilegeLevel
    Private m_iPrivilegeLevel As gPMConstants.PMELookupEditPrivlegeLevel

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    ' DC 31/01/00
    ' DC 31/01/00
    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property SelectedSourceID() As Integer
        Get
            Return m_iSelectedSourceID
        End Get
    End Property

    Public Property PrivilegeLevel() As Integer
        Get
            Return m_iPrivilegeLevel
        End Get
        Set(ByVal Value As Integer)
            m_iPrivilegeLevel = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim lPMAuthorityLevel As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            'Check if we can continue
            m_lReturn = CType(GetPrivileges(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            If sHelpFile <> "" Then
                'Developer Guide No. 38(No Solutions)
            End If

            If m_sCallingAppName.Trim() = "" Then
                m_lReturn = CType(GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                PMAuthorityLevel = lPMAuthorityLevel
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    '            Case PMKeyNameName
                    '                m_sName$ = CStr(vKeyArray(PMKeyValue, lRow&))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameCnt
            '    vKeyArray(PMKeyValue, 0) = m_lNameCnt&

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$
            vSummaryArray = ""
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Call the List Form initialise method
        'Developer Guide No. 97
        m_lReturn = frmList.Initialise

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmList = Nothing
            Return result
        End If

        ' Assign the parameters to the interface properties.
        With frmList
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .PMAuthorityLevel = PMAuthorityLevel
            .PrivilegeLevel = PrivilegeLevel
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Call the Load method to setup the List Form details
        'Developer Guide No. 68
        m_lReturn = frmList.Load_Renamed()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmList = Nothing
            Return result
        End If

        ' Call the ShowForm method to show the form, allow user input etc.
        m_lReturn = CType(frmList.ShowForm(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmList = Nothing
            Return result
        End If

        ' Assign the property members from the interface parameters.
        With frmList
            m_lStatus = .Status
            m_iSelectedSourceID = .SelectedSourceID
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmList.Close()
        frmList = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPrivileges
    '
    ' Description:
    '
    ' History: 04/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function GetPrivileges() As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMSource.Business
        Dim bIsAdministrator As Boolean
        Dim vSupervisedGroups As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the Business object
        Dim temp_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMSource.Business", vInstanceManager:="ClientManager")
        oBusiness = temp_oBusiness
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            oBusiness.Dispose()
            oBusiness = Nothing

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMSource.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges")

            Return result
        End If


        m_lReturn = oBusiness.GetPrivilegeLevel(r_iPrivilegeLevel:=m_iPrivilegeLevel)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            MessageBox.Show("You do not have permission to access " & _
                            "PM Branch Maintenance." & _
                            Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                            "Please contact your System Administrator.", Application.ProductName)

            result = gPMConstants.PMEReturnCode.PMFalse

            oBusiness.Dispose()
            oBusiness = Nothing
            Return result

        End If

        Select Case m_iPrivilegeLevel
            Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly
                ' Can at least view - so exit

                oBusiness.Dispose()
                oBusiness = Nothing
                Return result

            Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit
                ' No access allowed
                MessageBox.Show("You do not have permission to access " & _
                                "PM Branch Maintenance." & _
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                                "Please contact your System Administrator.", Application.ProductName)

                oBusiness.Dispose()
                oBusiness = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse

            Case Else
                ' Check authority
        End Select


        m_lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)

        oBusiness.Dispose()
        oBusiness = Nothing
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Administrator access
        If bIsAdministrator Then
            Select Case m_iPrivilegeLevel
                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView

                    m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView

                    m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions

                Case Else

                    m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly

            End Select

            Return result

        End If

        ' Standard user access
        Select Case m_iPrivilegeLevel
            Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions

                m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions

            Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView

                m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly

            Case Else

                result = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show("You do not have permission to access " & _
                                "PM Branch Maintenance." & _
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                                "Please contact your System Administrator.", Application.ProductName)

                Return result

        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Returns whether the User is a Sys Admin or Supervisor
    '              or Normal User.
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_lPMAuthorityLevel As Integer, Optional ByVal v_lUserGroupID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMSource.Business
        Dim lReturn As gPMConstants.PMEReturnCode
        Static bIsAdministrator As Boolean
        Static vSupervisedGroups As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default the Authority to User
            r_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser

            ' If we have NOT previously got the User Authority details
            ' then get them.

            If Object.Equals(vSupervisedGroups, Nothing) Then

                'Get the Business object
                Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMSource.Business", vInstanceManager:="ClientManager")
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

