Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("SelectedItem_NET.SelectedItem")> _
Public NotInheritable Class SelectedItem 
	  Implements IDisposable
' ***************************************************************** '
    ' Class Name: SelectedItem
    '
    ' Date: 08/08/1997
    '
    ' Description: Describes the Transdetail SelectedItem attributes.
    '
    ' Edit History:
    '               AMB 11/02/2003 - IAG PS220 Manage Debtors development
    '               Flag, Insured Name and Insured Account added
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
Private Const ACClass As String = "SelectedItem" 

    ' DataBase Attributes
    Private m_lTransdetailID As Integer
    Private m_sDocumentRef As String = ""
    Private m_dtAccountingDate As Date
    Private m_sPeriodName As String = ""
    Private m_iCurrencyID As Integer
    Private m_cCurrencyAmount As Decimal
    Private m_sFormattedCurrencyAmount As String = ""
    Private m_cBaseAmount As Decimal
    Private m_sFormattedBaseAmount As String = ""
    Private m_lDocumentTypeId As Integer
    Private m_sDocumentTypeDescription As String = ""
    Private m_lDocTypeGroupId As Integer
    Private m_sDocTypeGroupDescription As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_sOperatorName As String = ""
    Private m_sPurchaseInvoiceNo As String = ""
    Private m_sPurchaseOrderNo As String = ""
    Private m_sDepartment As String = ""
    Private m_sSpare As String = ""
    Private m_lAccountID As Integer
    Private m_sAccountShortCode As String = ""
    Private m_cMatchAmount As Decimal
    Private m_sReason As String = ""
    Private m_sFlag As String = ""
    Private m_sInsuredName As String = ""
    Private m_sInsuredAccount As String = ""
    Private m_sPayeeName As String = ""
    Private m_sCurrencyText As String = ""
    Private m_sBaseCurrencyText As String = ""
    Private m_iBaseCurrencyID As Integer

    ' Declare simple properties
    Public TransDetailTypeCode As String = ""
    Public TaxBandCode As String = ""
    Public TaxGroupCode As String = ""
    Public AllocationSequence As Integer
    Public AllocationRule As Integer


    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property

    Public Property OperatorName() As String
        Get
            Return m_sOperatorName
        End Get
        Set(ByVal Value As String)
            m_sOperatorName = Value
        End Set
    End Property

    Public Property PurchaseInvoiceNo() As String
        Get
            Return m_sPurchaseInvoiceNo
        End Get
        Set(ByVal Value As String)
            m_sPurchaseInvoiceNo = Value
        End Set
    End Property

    Public Property PurchaseOrderNo() As String
        Get
            Return m_sPurchaseOrderNo
        End Get
        Set(ByVal Value As String)
            m_sPurchaseOrderNo = Value
        End Set
    End Property

    Public Property Department() As String
        Get
            Return m_sDepartment
        End Get
        Set(ByVal Value As String)
            m_sDepartment = Value
        End Set
    End Property

    Public Property Spare() As String
        Get
            Return m_sSpare
        End Get
        Set(ByVal Value As String)
            m_sSpare = Value
        End Set
    End Property

    Public Property TransDetailId() As Integer
        Get
            Return m_lTransdetailID
        End Get
        Set(ByVal Value As Integer)
            m_lTransdetailID = Value
        End Set
    End Property

    Public Property DocumentRef() As String
        Get
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            m_sDocumentRef = Value
        End Set
    End Property

    Public Property AccountingDate() As Date
        Get
            Return m_dtAccountingDate
        End Get
        Set(ByVal Value As Date)
            m_dtAccountingDate = Value
        End Set
    End Property

    Public Property PeriodName() As String
        Get
            Return m_sPeriodName
        End Get
        Set(ByVal Value As String)
            m_sPeriodName = Value
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

    Public Property BaseCurrencyID() As Integer
        Get
            Return m_iBaseCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iBaseCurrencyID = Value
        End Set
    End Property

    Public Property CurrencyAmount() As Decimal
        Get
            Return m_cCurrencyAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cCurrencyAmount = Value
        End Set
    End Property

    Public Property FormattedCurrencyAmount() As String
        Get
            Return m_sFormattedCurrencyAmount
        End Get
        Set(ByVal Value As String)
            m_sFormattedCurrencyAmount = Value
        End Set
    End Property

    Public Property BaseAmount() As Decimal
        Get
            Return m_cBaseAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cBaseAmount = Value
        End Set
    End Property

    Public Property FormattedBaseAmount() As String
        Get
            Return m_sFormattedBaseAmount
        End Get
        Set(ByVal Value As String)
            m_sFormattedBaseAmount = Value
        End Set
    End Property

    Public Property DocumentTypeID() As Integer
        Get
            Return m_lDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
        End Set
    End Property

    Public Property DocumentTypeDescription() As String
        Get
            Return m_sDocumentTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTypeDescription = Value
        End Set
    End Property

    Public Property DocTypeGroupID() As Integer
        Get
            Return m_lDocTypeGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lDocTypeGroupId = Value
        End Set
    End Property


    Public Property DocTypeGroupDescription() As String
        Get
            Return m_sDocTypeGroupDescription
        End Get
        Set(ByVal Value As String)
            m_sDocTypeGroupDescription = Value
        End Set
    End Property

    Public Property PayeeName() As String
        Get
            Return m_sPayeeName
        End Get
        Set(ByVal Value As String)
            m_sPayeeName = Value
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

    Public Property AccountShortCode() As String
        Get
            Return m_sAccountShortCode
        End Get
        Set(ByVal Value As String)
            m_sAccountShortCode = Value
        End Set
    End Property

    Public WriteOnly Property MatchAmountAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cMatchAmount = Value
        End Set
    End Property
    Public ReadOnly Property MatchAmount() As Decimal
        Get
            Return m_cMatchAmount
        End Get
    End Property

    Public Property Reason() As String
        Get
            Return m_sReason
        End Get
        Set(ByVal Value As String)
            m_sReason = Value
        End Set
    End Property

    Public Property Flag() As String
        Get
            Return m_sFlag
        End Get
        Set(ByVal Value As String)
            m_sFlag = Value
        End Set
    End Property

    Public Property InsuredName() As String
        Get
            Return m_sInsuredName
        End Get
        Set(ByVal Value As String)
            m_sInsuredName = Value
        End Set
    End Property

    Public Property InsuredAccount() As String
        Get
            Return m_sInsuredAccount
        End Get
        Set(ByVal Value As String)
            m_sInsuredAccount = Value
        End Set
    End Property

    Public Property CurrencyText() As String
        Get
            Return m_sCurrencyText
        End Get
        Set(ByVal Value As String)
            m_sCurrencyText = Value
        End Set
    End Property

    Public Property BaseCurrencyText() As String
        Get
            Return m_sBaseCurrencyText
        End Get
        Set(ByVal Value As String)
            m_sBaseCurrencyText = Value
        End Set
    End Property



    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this object.
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
        ' Class Initialise
        Exit Sub
    End Sub

	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

End Class
