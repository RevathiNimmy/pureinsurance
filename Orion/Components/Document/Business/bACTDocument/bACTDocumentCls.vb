Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Document_NET.Document")>
Public NotInheritable Class Document
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Document
    '
    ' Date: 08/08/1997
    '
    ' Description: Describes the Document attributes.
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
    Private Const ACClass As String = "Document"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As gPMConstants.PMEComponentAction

    ' DataBase Attributes
    Private m_lDocumentID As Integer
    Private m_iCompanyID As Integer
    Private m_iPostingstatusID As Integer
    Private m_iDocumenttypeID As Integer
    Private m_lAuditsetID As Integer
    Private m_lBatchID As Integer
    Private m_sDocumentRef As New StringsHelper.FixedLengthString(25)
    Private m_dtDocumentDate As Date
    Private m_dtCreatedDate As Date
    Private m_dtAuthorisedDate As Date
    Private m_sComment As New StringsHelper.FixedLengthString(255)
    Private m_lWriteOffReasonID As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sReason As String = ""
    Private m_lSubBranchID As Integer
    Private m_lClaimID As Integer
    'S4BDAT004
    Private m_lTermsOfPaymentId As Integer
    Private m_vPaymentDueDate As Date

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


    Public Property DocumentID() As Integer
        Get

            Return m_lDocumentID

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentID = Value

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

    Public Property PostingstatusID() As Integer
        Get

            Return m_iPostingstatusID

        End Get
        Set(ByVal Value As Integer)

            m_iPostingstatusID = Value

        End Set
    End Property

    Public Property DocumenttypeID() As Integer
        Get

            Return m_iDocumenttypeID

        End Get
        Set(ByVal Value As Integer)

            m_iDocumenttypeID = Value

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

    Public Property BatchID() As Integer
        Get

            Return m_lBatchID

        End Get
        Set(ByVal Value As Integer)

            m_lBatchID = Value

        End Set
    End Property

    Public Property DocumentRef() As String
        Get

            Return m_sDocumentRef.Value

        End Get
        Set(ByVal Value As String)

            m_sDocumentRef.Value = Value

        End Set
    End Property

    Public Property DocumentDate() As Date
        Get

            Return m_dtDocumentDate

        End Get
        Set(ByVal Value As Date)

            m_dtDocumentDate = Value

        End Set
    End Property

    Public Property CreatedDate() As Date
        Get

            Return m_dtCreatedDate

        End Get
        Set(ByVal Value As Date)

            m_dtCreatedDate = Value

        End Set
    End Property

    Public Property AuthorisedDate() As Date
        Get

            Return m_dtAuthorisedDate

        End Get
        Set(ByVal Value As Date)

            m_dtAuthorisedDate = Value

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


    Public Property WriteOffReasonID() As Integer
        Get

            Return m_lWriteOffReasonID

        End Get
        Set(ByVal Value As Integer)

            m_lWriteOffReasonID = Value

        End Set
    End Property

    'sj 01/08/2002 - start
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property Reason() As String
        Get
            Return m_sReason
        End Get
        Set(ByVal Value As String)
            m_sReason = Value
        End Set
    End Property

    ' pwf - add sub_branch_id
    Public Property SubBranchID() As Integer
        Get
            Return m_lSubBranchID
        End Get
        Set(ByVal Value As Integer)
            m_lSubBranchID = Value
        End Set
    End Property

    Public Property ClaimId() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property
    Public Property TermsOfPaymentId() As Integer
        Get
            Return m_lTermsOfPaymentId
        End Get
        Set(ByVal Value As Integer)
            m_lTermsOfPaymentId = Value
        End Set
    End Property
    Public Property PaymentDueDate() As Date
        Get
            Return m_vPaymentDueDate
        End Get
        Set(ByVal Value As Date)

            m_vPaymentDueDate = CDate(Value)
        End Set
    End Property



    'sj 01/08/2002 - end
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
