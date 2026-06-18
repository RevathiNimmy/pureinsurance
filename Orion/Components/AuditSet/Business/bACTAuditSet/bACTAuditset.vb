Option Strict Off
Option Explicit On
'developer guide no 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Auditset_NET.Auditset")>
Public NotInheritable Class Auditset
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Auditset
    '
    ' Date: 08/08/1997
    '
    ' Description: Describes the Auditset attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Auditset"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lAuditsetID As Integer
    Private m_iCompanyID As Integer
    Private m_dtPostedDate As Date
    Private m_sComment As New StringsHelper.FixedLengthString(255)

    ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development
    Private m_lDocumentID As Integer
    Private m_lAuditSetTypeID As Integer
    Private m_dtApprovedDate As Date
    Private m_iApprovedUserID As Integer
    Private m_iRejected As Integer
    Private m_iRejectedUserID As Integer
    Private m_lCashListItemID As Integer


    Public Property ApprovedDate() As Date
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_dtApprovedDate

        End Get
        Set(ByVal Value As Date)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_dtApprovedDate = Value

        End Set
    End Property


    Public Property ApprovedUserID() As Integer
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_iApprovedUserID

        End Get
        Set(ByVal Value As Integer)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_iApprovedUserID = Value

        End Set
    End Property


    Public Property Rejected() As Integer
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_iRejected

        End Get
        Set(ByVal Value As Integer)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_iRejected = Value

        End Set
    End Property


    Public Property RejectedUserID() As Integer
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_iRejectedUserID

        End Get
        Set(ByVal Value As Integer)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_iRejectedUserID = Value

        End Set
    End Property


    Public Property DocumentID() As Integer
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_lDocumentID

        End Get
        Set(ByVal Value As Integer)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_lDocumentID = Value

        End Set
    End Property

    Public Property AuditSetTypeID() As Integer
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_lAuditSetTypeID

        End Get
        Set(ByVal Value As Integer)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_lAuditSetTypeID = Value

        End Set
    End Property

    Public Property CashListItemID() As Integer
        Get
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            Return m_lCashListItemID

        End Get
        Set(ByVal Value As Integer)
            ' AMB 05/02/2003 - Added for IAG 220 Manage Debtors development

            m_lCashListItemID = Value

        End Set
    End Property

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


    Public Property AuditsetID() As Integer
        Get

            Return m_lAuditsetID

        End Get
        Set(ByVal Value As Integer)

            m_lAuditsetID = Value

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

    Public Property UserID() As Integer
        Get

            Return m_iUserID

        End Get
        Set(ByVal Value As Integer)

            m_iUserID = Value

        End Set
    End Property

    Public Property PostedDate() As Date
        Get

            Return m_dtPostedDate

        End Get
        Set(ByVal Value As Date)

            m_dtPostedDate = Value

        End Set
    End Property

    Public Property Comment() As String
        Get

            Return m_sComment.Value

        End Get
        Set(ByVal Value As String)

            m_sComment.Value = Value

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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Friend Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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