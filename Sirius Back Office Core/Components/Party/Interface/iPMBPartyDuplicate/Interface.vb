Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 19/01/2005
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' RKS   19/01/2005  Created
    ' RKS   21/01/2005  Modified
    ' ***************************************************************** '


    ' Private constants
    Private Const ACClass As String = "Interface"


    ' Private variables
    Private m_sCallingAppName As String = ""

    Private m_vClientData(,) As Object
    Private m_sPartyTypeCode As String = ""
    Private m_sOriginalClientCode As String = ""
    Private m_sUniqueClientCode As String = ""

    Private m_sSelectedClientCode As String = ""
    Private m_lSelectedClientPartyCnt As Integer
    Private m_iOKAction As Integer

    Private m_lStatus As Integer
    Private m_lReturn As Integer

    ' Public Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    'NIIT - Replaced with the Migrated code 1144 
    'Public WriteOnly Property ClientData() As Object()
    '	Set(ByVal Value() As Object)
    '		m_vClientData = Value
    '	End Set
    '   End Property
    Public WriteOnly Property ClientData() As Object(,)
        Set(ByVal Value As Object(,))
            m_vClientData = Value
        End Set
    End Property

    Public WriteOnly Property PartyTypeCode() As String
        Set(ByVal Value As String)
            m_sPartyTypeCode = Value
        End Set
    End Property

    Public WriteOnly Property OriginalClientCode() As String
        Set(ByVal Value As String)
            m_sOriginalClientCode = Value
        End Set
    End Property

    Public WriteOnly Property UniqueClientCode() As String
        Set(ByVal Value As String)
            m_sUniqueClientCode = Value
        End Set
    End Property

    Public ReadOnly Property SelectedClientCode() As String
        Get
            Return m_sSelectedClientCode
        End Get
    End Property

    Public ReadOnly Property SelectedClientPartyCnt() As Integer
        Get
            Return m_lSelectedClientPartyCnt
        End Get
    End Property

    Public ReadOnly Property OKAction() As Integer
        Get
            Return m_iOKAction
        End Get
    End Property


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 19/01/2005 RKS - Created.
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
                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 19/01/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
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

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessInterface
    '
    ' Description:
    '
    ' History: 19/01/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim oInterface As frmInterface



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a new instance of the form
        oInterface = New frmInterface()

        ' Set the properties
        With oInterface
            ''developer guide no. 24
            .ClientData = m_vClientData
            .PartyTypeCode = m_sPartyTypeCode
            .OriginalClientCode = m_sOriginalClientCode
            .UniqueClientCode = m_sUniqueClientCode
        End With

        ' Load it

        'developer guide no. 7
        'Load(oInterface)
        'oInterface.ShowDialog()

        'Prepare interface
        oInterface.SetInterfaceDefaults()

        ' Display
        oInterface.ShowDialog()

        ' Get any properites
        With oInterface
            m_lStatus = .Status
            m_iOKAction = .OKAction
            m_sSelectedClientCode = .SelectedClientCode
            m_lSelectedClientPartyCnt = .SelectedClientPartyCnt
        End With

        ' Unload it
        oInterface.Close()

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 19/01/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

