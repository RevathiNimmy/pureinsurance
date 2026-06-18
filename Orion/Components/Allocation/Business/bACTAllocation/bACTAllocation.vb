Option Strict Off
Option Explicit On
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Allocation_NET.Allocation")>
Public NotInheritable Class Allocation

    Implements IDisposable
    Private m_sUsername As String = ""

    Private m_iUserID As Integer
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Allocation"

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lAllocationID As Integer
    Private m_iCompanyID As Integer
    Private m_lAccountID As Integer
    Private m_dtAllocationDate As Date
    Private m_lAllocationstatusID As Integer

    Private m_nAllocationBatchID As Integer = 0

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property AllocationBatchID() As Integer
        Get
            Return m_nAllocationBatchID
        End Get
        Set(ByVal Value As Integer)
            m_nAllocationBatchID = Value
        End Set
    End Property

    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property


    Public Property AllocationID() As Integer
        Get

            Return m_lAllocationID

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationID = Value

        End Set
    End Property

    Public Property CompanyID() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

        End Set
    End Property

    Public Property AccountID() As Integer
        Get

            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            m_lAccountID = Value

        End Set
    End Property

    Public Property UserID() As Integer
        Get

            Return m_iUserID

        End Get
        Set(ByVal Value As Integer)

            m_iUserID = Value

        End Set
    End Property

    Public Property AllocationDate() As Date
        Get

            Return m_dtAllocationDate

        End Get
        Set(ByVal Value As Date)

            m_dtAllocationDate = Value

        End Set
    End Property

    Public Property AllocationstatusID() As Integer
        Get

            Return m_lAllocationstatusID

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationstatusID = Value

        End Set
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try
            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    Friend Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class