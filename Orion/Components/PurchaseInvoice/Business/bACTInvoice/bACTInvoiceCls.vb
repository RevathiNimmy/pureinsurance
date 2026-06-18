Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
Friend NotInheritable Class ACTInvoice
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ACTInvoice
    '
    ' Date: 29/10/1998
    '
    ' Description: Describes the ACTInvoice attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ACTInvoice"

    ' PUBLIC Data Members (Begin)
    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Error Code
    Private m_lReturn As Integer

    ' PRIVATE Data Members (End)

    ' DataBase Attributes
    Private m_lInvoiceID As Integer
    Private m_sInvoiceNumber As New FixedLengthString(40)
    Private m_dtInvoiceDate As Date
    Private m_sOrderNo As New FixedLengthString(40)
    Private m_sDescription As New FixedLengthString(255)
    Private m_lSupplierID As Integer
    Private m_sCode As New FixedLengthString(30)
    Private m_sReference As New FixedLengthString(40)
    Private m_lCompanyID As Integer

    ' CTAF 120500
    Private m_vVATRate As Object

    Public Property VATRate() As Object
        Get
            Return m_vVATRate
        End Get
        Set(ByVal Value As Object)


            m_vVATRate = Value
        End Set
    End Property



    Public Property Code() As String
        Get

            Return m_sCode.Value

        End Get
        Set(ByVal Value As String)

            m_sCode.Value = Value

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


    Public Property InvoiceDate() As Date
        Get

            Return m_dtInvoiceDate

        End Get
        Set(ByVal Value As Date)

            m_dtInvoiceDate = Value

        End Set
    End Property


    Public Property InvoiceNumber() As String
        Get

            Return m_sInvoiceNumber.Value

        End Get
        Set(ByVal Value As String)

            m_sInvoiceNumber.Value = Value

        End Set
    End Property



    Public Property OrderNo() As String
        Get

            Return m_sOrderNo.Value

        End Get
        Set(ByVal Value As String)

            m_sOrderNo.Value = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property


    Public Property Reference() As String
        Get

            Return m_sReference.Value

        End Get
        Set(ByVal Value As String)

            m_sReference.Value = Value

        End Set
    End Property


    Public Property SupplierID() As Integer
        Get

            Return m_lSupplierID

        End Get
        Set(ByVal Value As Integer)

            m_lSupplierID = Value

        End Set
    End Property

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



    Public Property CompanyID() As Integer
        Get

            Return m_lCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_lCompanyID = Value

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
