Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
Friend NotInheritable Class ACTInvoiceItem
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ACTInvoiceItem
    '
    ' Date: 02/11/1998
    '
    ' Description: Describes the ACTInvoiceItem attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ACTInvoiceItem"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' DataBase Attributes
    Private m_lInvoiceID As Integer
    Private m_lInvoiceItemNo As Integer
    Private m_sDescription As New FixedLengthString(255)

    'MKR 03/08/04 PN : 13642
    Private m_sNominalCode As New FixedLengthString(20)

    Private m_dValue As Double
    Private m_iCurrencyID As Integer

    ' CTAF 170400
    Private m_iDepartmentID As Integer
    Private m_vDeptAmount As Byte

    Private m_vVATRate As Byte

    Private m_bHasVat As Boolean

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Error Code
    Private m_lReturn As Integer
    Private m_sUsername As String = ""
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property

    Public Property InvoiceID() As Integer
        Get

            Return m_lInvoiceID

        End Get
        Set(ByVal Value As Integer)

            m_lInvoiceID = Value

        End Set
    End Property



    Public Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_iCurrencyID = Value

        End Set
    End Property


    Public Property Description() As String
        Get

            Return m_sDescription.Value

        End Get
        Set(ByVal Value As String)

            m_sDescription.Value = Value

        End Set
    End Property



    Public Property NominalCode() As String
        Get

            Return m_sNominalCode.Value

        End Get
        Set(ByVal Value As String)

            m_sNominalCode.Value = Value

        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return m_iDepartmentID
        End Get
        Set(ByVal Value As Integer)
            m_iDepartmentID = Value
        End Set
    End Property

    Public Property DeptAmount() As Byte
        Get
            Return m_vDeptAmount
        End Get
        Set(ByVal Value As Byte)

            m_vDeptAmount = CByte(Value)
        End Set
    End Property

    Public Property VATRate() As Byte
        Get
            Return m_vVATRate
        End Get
        Set(ByVal Value As Byte)

            m_vVATRate = CByte(Value)
        End Set
    End Property


    Public Property HasVat() As Boolean
        Get
            Return m_bHasVat
        End Get
        Set(ByVal Value As Boolean)
            m_bHasVat = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property


    Public Property Value() As Double
        Get

            Return m_dValue

        End Get
        Set(ByVal Value As Double)

            m_dValue = Value

        End Set
    End Property

    Public Property InvoiceItemNo() As Integer
        Get

            Return m_lInvoiceItemNo

        End Get
        Set(ByVal Value As Integer)

            m_lInvoiceItemNo = Value

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
    Public Function Initialise(Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
