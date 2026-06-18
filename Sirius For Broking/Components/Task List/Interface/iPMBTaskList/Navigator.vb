Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Navigator_NET.Navigator")> _
Public NotInheritable Class Navigator 
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Navigator
    '
    ' Date: 24/07/2001
    '
    ' Description: An instance of Navigator
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Navigator"

    Private lReturn As Integer

    Private WithEvents m_oNavigator As iPMNavigator.NavigateControl
    'DJM 18/02/2004 : Add XML Navigator functionality.
    'Developer Guide No. 108
    Private WithEvents m_oNavigatorXM As iPMNavigatorXM.Interface_Renamed
    Private WithEvents m_oComponent As iPMWrkComponentStarter.StartControl
    Private m_lPMWrkTaskInstanceCnt As Integer
    Private m_bIsNavigatorInstance As Boolean
    Private m_bStatusUpdated As Boolean
    Private m_sKey As String = ""
    'DJM 18/02/2004 : Add XML Navigator functionality.
    Private m_sNavXMLfile As String = ""

    Public Property Navigator() As iPMNavigator.NavigateControl
        Get
            Return m_oNavigator
        End Get
        Set(ByVal Value As iPMNavigator.NavigateControl)
            'Developer Guide No. 33
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is Object) Then
                m_oNavigator = Value
            Else
                m_oNavigator = Value
            End If
        End Set
    End Property

    'DJM 18/02/2004 : Add XML Navigator functionality.
    'Developer Guide No. 108
    Public Property NavigatorXM() As iPMNavigatorXM.Interface_Renamed
        Get
            Return m_oNavigatorXM
        End Get
        'Developer Guide No. 108
        Set(ByVal Value As iPMNavigatorXM.Interface_Renamed)
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is Object) Then
                m_oNavigatorXM = Value
            Else
                m_oNavigatorXM = Value
            End If
        End Set
    End Property

    'DJM 18/02/2004 : Add XML Navigator functionality.
    Public Property NavXMLfile() As String
        Get
            Return m_sNavXMLfile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLfile = Value
        End Set
    End Property

    Public Property Component() As iPMWrkComponentStarter.StartControl
        Get
            Return m_oComponent
        End Get
        Set(ByVal Value As iPMWrkComponentStarter.StartControl)
            'Developer Guide No. 33
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is Object) Then
                m_oComponent = Value
            Else
                m_oComponent = Value
            End If
        End Set
    End Property

    Public Property PMWrkTaskInstanceCnt() As Integer
        Get
            Return m_lPMWrkTaskInstanceCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPMWrkTaskInstanceCnt = Value
        End Set
    End Property

    Public Property IsNavigatorInstance() As Boolean
        Get
            Return m_bIsNavigatorInstance
        End Get
        Set(ByVal Value As Boolean)
            m_bIsNavigatorInstance = Value
        End Set
    End Property

    Public Property StatusUpdated() As Boolean
        Get
            Return m_bStatusUpdated
        End Get
        Set(ByVal Value As Boolean)
            m_bStatusUpdated = Value
        End Set
    End Property

    Public Property Key() As String
        Get
            Return m_sKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sKey = Value.Trim()
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_bIsNavigatorInstance As Boolean) As Integer

        Dim result As Integer = 0
        Dim sWhatFailed As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            IsNavigatorInstance = v_bIsNavigatorInstance
            StatusUpdated = False

            ' Is this is a Navigator Instance
            If v_bIsNavigatorInstance Then
                ' Yes, so create an Instance of Navigator

                'DJM 18/02/2004 : Add XML Navigator functionality.
                If NavXMLfile.Trim() = "" Then
                    sWhatFailed = "Navigator"

                    Component = Nothing
                    Navigator = New iPMNavigator.NavigateControl()

                    'Developer Guie No 9
                    lReturn = Navigator.Initialise()

                    Navigator.CallingAppName = ACApp
                Else
                    sWhatFailed = "Navigator XM"

                    Component = Nothing
                    'Developer Guide No. 108
                    NavigatorXM = New iPMNavigatorXM.Interface_Renamed()
                    'Developer Guide No. 9
                    lReturn = NavigatorXM.Initialise()

                    NavigatorXM.CallingAppName = ACApp
                End If

            Else

                ' No, so Create Component Starter
                sWhatFailed = "Component Starter"

                Navigator = Nothing
                Component = New iPMWrkComponentStarter.StartControl()
                'Developer Guie No 9
                lReturn = Component.Initialise()

                Component.CallingAppName = ACApp

            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Initialise : " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to create an instance of : " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If IsNavigatorInstance Then
                    'DJM 18/02/2004 : Add XML Navigator functionality.
                    If NavXMLfile.Trim() = "" Then
                        Navigator.Dispose()
                        Navigator = Nothing
                    Else
                        NavigatorXM.Dispose()
                        NavigatorXM = Nothing
                    End If
                Else
                    If Component IsNot Nothing Then
                        Component.Dispose()
                        Component = Nothing
                    End If
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub



    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Sub m_oComponent_Finished(ByVal v_bComplete As Boolean) Handles m_oComponent.Finished

        ' So we know that we have updated the Task Status for this Instance.
        StatusUpdated = True

    End Sub

    Private Sub m_oNavigator_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigator.SetProcessStatus

        ' So we know that we have updated the Task Status for this Instance.
        StatusUpdated = True

    End Sub

    Private Sub m_oNavigatorXM_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigatorXM.SetProcessStatus

        ' So we know that we have updated the Task Status for this Instance.
        StatusUpdated = True

    End Sub
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class