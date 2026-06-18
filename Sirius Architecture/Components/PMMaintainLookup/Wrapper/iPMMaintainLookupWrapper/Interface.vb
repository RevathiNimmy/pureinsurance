Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No 129. 
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' Edit History:
    ' DAK251199 - Make business object global
    ' VB 11/03/2005 PN19406 :GetsysAdminStatus() removed from initialise method and
    '                        placed in start method where we check Permission.
    ' ***************************************************************** '

    Private Const ACClass As String = "Interface"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Calling app name
    Private m_sCallingAppName As String = ""

    ' Private variable
    Private m_lPMAuthorityLevel As Integer

    ' Status
    Private m_lStatus As Integer

    Public ReadOnly Property Status() As Integer
        Get
            Return g_frmMain.Status
        End Get
    End Property

    ' Property Let
    ' Property Get
    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise
        Dim result As Integer = 0
        Dim lPMAuthorityLevel As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Instance of object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get instance of the business
            'DAK251199
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bPMMaintainLookupWrapper.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'VB 11/03/2005 PN 19406 This code removed from initialise method and placed in
            '                       start method where we check Permission.
            '' RDC 16102002
            '    'Check if we can continue
            '    m_lReturn = GetSysAdminStatus()
            '    If m_lReturn <> PMTrue Then
            '        Initialise = m_lReturn
            '        Exit Function
            '    End If

            ' Create a new instance of the main form
            g_frmMain = New frmMain()

            If m_sCallingAppName.Trim() = "" Then
                m_lReturn = CType(GetUserAuthority(r_lPMAuthorityLevel:=lPMAuthorityLevel), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                PMAuthorityLevel = lPMAuthorityLevel
            End If

            With g_frmMain
                .PMAuthorityLevel = PMAuthorityLevel
            End With

            ' Load it

            'Developer Guide No. 68(Guide)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description:
    '
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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                g_frmMain = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'VB 11/03/2005 PN19406 Check permission
            m_lReturn = CType(GetSysAdminStatus(), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMMNoAccess Then
                Return gPMConstants.PMEReturnCode.PMTrue
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            ' VB End

            ' Display it
            g_frmMain.ShowDialog()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetProcessModes
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(ByRef vTask As Object, ByRef vNavigate As Object, ByRef vProcessMode As Object, ByRef vTransactionType As Object, ByRef vEffectiveDate As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Returns whether the User is a Sys Admin or Supervisor
    '              or Normal User.
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_lPMAuthorityLevel As Integer, Optional ByVal v_lUserGroupID As Integer = 0) As Integer

        Dim result As Integer = 0
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

                'DAK251199

                lReturn = g_oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)
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

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Private Function GetSysAdminStatus() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim oBusiness As Object



        result = gPMConstants.PMEReturnCode.PMFalse


        m_lReturn = g_oBusiness.GetSysAdminStatus(lStatus)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lStatus = 0 Then

            oBusiness = Nothing

            MessageBox.Show("You do not have permission to access " & _
                            "Lookup Maintenance." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                            "Please contact your System Administrator.", Application.ProductName) 'PN19406
            Return gPMConstants.PMEReturnCode.PMMNoAccess
        End If

        oBusiness = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Class

