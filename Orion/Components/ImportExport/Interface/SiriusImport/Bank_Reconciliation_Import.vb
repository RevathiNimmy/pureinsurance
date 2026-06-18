Imports System.Collections.ObjectModel
Imports System.Xml


Friend NotInheritable Class Bank_Reconciliation_Import : Inherits ImportBase 
    'Developer guide no.
    Dim m_vKeys() As Object
    Dim m_iCount As Integer = 0
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer

#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    ''given the Batch Code coz it is must overrides property of Import base class
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "BCP"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Bank_Reconciliation_Import"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Overrides Sub PostImportProcessing()
    End Sub

    Protected Overrides Sub ProcessHeader()
        ReDim m_vKeys(ElementCount)
    End Sub

    Protected Overrides Sub ProcessElement()
        ' Get parameters
        NoOfTotalRecords += 1
        Dim iTransDetailID As Integer
        iTransDetailID = GetAttribute("transdetail_id")
        If Not iTransDetailID.Equals(0) Then
            m_vKeys(m_iCount) = iTransDetailID
        Else
        End If
        m_iCount = m_iCount + 1
    End Sub


    Protected Overrides Sub FinaliseImport()
        Dim iReturn As Integer
        Dim oBankReconciliation As bACTBankReconciliation.Business

        oBankReconciliation = New bACTBankReconciliation.Business
        oBankReconciliation.Initialise( _
                sUsername:="", _
                sPassword:="", _
                iUserID:=1, _
                iSourceID:=1, _
                iLanguageID:=1, _
                iCurrencyID:=26, _
                iLogLevel:=PMELogLevel.PMLogError, _
                sCallingAppName:=ACApp, _
                vDatabase:=Nothing)
        ' oBankReconciliation = CreateBusiness("bACTBankReconciliation.Business")
        iReturn = oBankReconciliation.Reconcile(m_vKeys)

    End Sub
    ''' <summary>
    ''' Update batch Status
    ''' </summary>
    Protected Overrides Sub UpdateBatchStatus()
        UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)
    End Sub

#End Region

#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region
End Class
