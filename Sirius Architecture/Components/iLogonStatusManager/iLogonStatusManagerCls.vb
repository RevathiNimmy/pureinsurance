Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("LogonStatusManager_NET.LogonStatusManager")> _
Public NotInheritable Class LogonStatusManager
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: LogonStatusManager
    '
    ' Date: 09 July 1996
    '
    ' Description: Main Class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "LogonStatusManager"

    'developer guide no. 107
    <ThreadStatic()> _
    Public Shared objfrmInterface As frmInterface
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    Private m_vObjectParam As Object

    Public WriteOnly Property LanguageId() As Integer
        Set(ByVal Value As Integer)

            'set language_id
            g_iLanguageID = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)
    ' PUBLIC Property Procedures (Begin)

    Public Property ObjectParam() As Object
        Get

            ' Standard Property.

            ' Return the objects parameter value.
            Return m_vObjectParam
        End Get
        Set(ByVal Value As Object)

            ' Standard Property.

            ' Set the objects parameter value.

            m_vObjectParam = Value

        End Set
    End Property

    Public ReadOnly Property Cancelled() As Boolean
        Get

            ' Standard Property.

            ' Return the cancelled flag from the form.
            Return objfrmInterface.FormCancelled
        End Get
    End Property

    Public WriteOnly Property UserName() As String
        Set(ByVal Value As String)

            ' Set the username.
            g_sUserName = Value

        End Set
    End Property


    Public WriteOnly Property Password() As String
        Set(ByVal Value As String)

            ' Set the password.
            g_sPassword = Value

        End Set
    End Property

    Public WriteOnly Property LogonTime() As Date
        Set(ByVal Value As Date)

            ' Set the logon time.
            g_dLogonTime = Value

        End Set
    End Property

    ' RDC 13082002
    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            g_iSourceID = Value
        End Set
    End Property

    Public WriteOnly Property SourceName() As String
        Set(ByVal Value As String)

            DirectCast(iLogonStatusManager.MainModule.g_frmInterface, iLogonStatusManager.frmInterface).lblSource.Text = Value
            ' g_sSourceName = Value

        End Set
    End Property

    ' RDC 29112002

    Public Property CountryID() As Integer
        Get
            Return g_iCountryID
        End Get
        Set(ByVal Value As Integer)
            g_iCountryID = Value
        End Set
    End Property
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
    Public Function Initialise() As Integer

        Dim result As Integer = 0

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to process its
    '              parameters.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Process the objects event type.
            lErrorValue = ProcessFormMode()

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the event type.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process form mode", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process form start", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessFormMode (Standard Method)
    '
    ' Description: Calls the appropriate methods etc when using the
    '              interface form.
    '
    ' ***************************************************************** '
    Private Function ProcessFormMode() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer


        objfrmInterface = New frmInterface
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the form into memory.
        lErrorValue = LoadForm()

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the form.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load the interface form", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFormMode")

            Return result
        End If

        ' Display the form.
        lErrorValue = ShowForm()

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the form.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display the interface form", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFormMode")

            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadForm (Standard Method)
    '
    ' Description: Loads the instance of the interface form into
    '              memory.
    '
    ' ***************************************************************** '
    Private Function LoadForm() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the instance of the interface
        ' form into memory.

        ' Assign the parameters to the form
        ' properties.


        objfrmInterface.ObjectParam = ObjectParam

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Displays the instance of the interface form,
    '              modally.
    '
    ' ***************************************************************** '
    Private Function ShowForm() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display's the interface form.
        objfrmInterface.Show()

        Return result

    End Function
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

