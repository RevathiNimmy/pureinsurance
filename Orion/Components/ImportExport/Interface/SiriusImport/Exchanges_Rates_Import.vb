'Start(Saurabh Agrawal) - Tech Spec WR39 Automatic ExchangeRate maintainence(4.3)
Imports System.Collections.ObjectModel
Imports System.Math
Imports System.Xml

Friend NotInheritable Class Exchange_Rates_Import : Inherits ImportBase
#Region "Fields"
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
#End Region

#Region "Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "CERA"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Currency Exchange Rates"
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
    Protected Overrides Sub ProcessElement()
        Try
            NoOfTotalRecords += 1
            ValidateExchangeRates()

            ImportExchangeRates()

        Catch ex As Exception
            Throw New Exception("Unable to process Exchange Rates import", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Validate the exchange rates
    ''' </summary>
    Private Sub ValidateExchangeRates()

        Dim iReturn As PMEReturnCode
        Dim lNumberOfRecords As Long = 0
        Dim iCurrencyID As Integer = 0
        Dim iCompanyID As Integer = 0
        Dim dRateAgainstBase As Decimal = 0


        dRateAgainstBase = GetAttribute("rate_against_base")
        iCurrencyID = GetAttribute("currency_id")
        iCompanyID = GetAttribute("company_id")

        If dRateAgainstBase < 0 Then
            Throw New Exception("Rate Against Base cannot be less then 0")
        End If

        AddParameterLite(m_oDatabase, "company_id", iCompanyID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "currency_id", iCurrencyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        iReturn = m_oDatabase.SQLSelect("spu_ACT_Do_CurrencyInCompany", "Currency In Company", True, lNumberOfRecords)

        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Do_CurrencyInCompany'")
        End If

        If lNumberOfRecords = 0 Then
            Throw New Exception("Currency Id – " & iCurrencyID & " not supported for Branch Id - " & iCompanyID)
        End If


    End Sub
    ''' <summary>
    ''' Import the exchange rates
    ''' </summary>
    Private Sub ImportExchangeRates()

        Dim dRateAgainstBase As Decimal
        Dim iCurrencyId As Integer
        Dim iCompanyId As Integer
        Dim iReturn As PMEReturnCode
        Dim lNumberOfRecords As Long = 0


        dRateAgainstBase = GetAttribute("rate_against_base")
        iCurrencyId = GetAttribute("currency_id")
        iCompanyId = GetAttribute("company_id")

        AddParameterLite(m_oDatabase, "effective_date", Date.Today.ToLongDateString, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

        AddParameterLite(m_oDatabase, "currency_id", iCurrencyId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        AddParameterLite(m_oDatabase, "company_id", iCompanyId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)


        iReturn = m_oDatabase.SQLSelect("spu_ACT_Get_CurrencyRate", "Select All Currency Rate", True, lNumberOfRecords)

        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute spu_ACT_Get_CurrencyRate")
        End If
        AddParameterLite(m_oDatabase, "effective_from", Date.Today.ToLongDateString, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

        AddParameterLite(m_oDatabase, "currency_id", iCurrencyId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        AddParameterLite(m_oDatabase, "company_id", iCompanyId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

        AddParameterLite(m_oDatabase, "rate_against_base", dRateAgainstBase, PMEParameterDirection.PMParamInput, PMEDataType.PMDecimal)

        If lNumberOfRecords = 0 Then

            ' If No then call the Add routine Then
            iReturn = m_oDatabase.SQLAction("spu_ACT_Add_CurrencyRate", "Add Currency Exchange Rates", True)


        Else
            ' Else If Call the Update routine
            iReturn = m_oDatabase.SQLAction("spu_ACT_Update_CurrencyRate", "Update Currency Exchange Rates", True)
        End If

        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_ACT_Add_CurrencyRate'")
        End If


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

'End(Saurabh Agrawal) - Tech Spec WR39 Automatic ExchangeRate maintainence(4.3)