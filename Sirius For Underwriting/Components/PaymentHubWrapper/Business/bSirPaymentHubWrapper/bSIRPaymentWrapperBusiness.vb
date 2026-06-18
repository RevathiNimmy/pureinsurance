Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml
'Developer Guide No. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' Handle action_code for history 

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_nReturn As gPMConstants.PMEReturnCode
    Private m_nError As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public m_oPaymentHubConfigurationParameters As Object
    Public m_oPaymentHubResponseParameters As Object
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long
        Try
            m_nReturn = PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Set Username and Password
            m_lCurrentRecord = 0

            m_sTransactionType = PMTransactionTypeGeneric
            m_nReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If m_oPaymentHubConfigurationParameters Is Nothing Then
                m_oPaymentHubConfigurationParameters = New PaymentHubConfigurationParameters()

                Dim r_oResults(,) As Object = Nothing
                m_nReturn = GetPaymentHubSystemOptions(r_oResults)
                If m_nReturn = PMEReturnCode.PMNotFound OrElse Not Informations.IsArray(r_oResults) Then
                    Return PMEReturnCode.PMNotFound
                End If
                For iCount As Integer = 0 To r_oResults.GetUpperBound(1)
                    With CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters)
                        Select Case ToSafeString(r_oResults(0, iCount))
                            Case "5185"
                                .SystemUserName = ToSafeString(r_oResults(1, iCount))
                            Case "5186"
                                .Password = bPMFunc.GetOVal(ToSafeString(r_oResults(1, iCount)))
                            Case "5187"
                                .ClientName = ToSafeString(r_oResults(1, iCount))
                            Case "5188"
                                .BrokerSCID = ToSafeString(r_oResults(1, iCount))
                            Case "5189"
                                .MerchantID = ToSafeString(r_oResults(1, iCount))
                            Case "5190"
                                .Customer = ToSafeString(r_oResults(1, iCount))
                            Case "5191"
                                .SystemGUID = bPMFunc.GetOVal(ToSafeString(r_oResults(1, iCount)))
                            Case "5192"
                                .SystemPasscode = bPMFunc.GetOVal(ToSafeString(r_oResults(1, iCount)))
                            Case "5193"
                                .AccountID = bPMFunc.GetOVal(ToSafeString(r_oResults(1, iCount)))
                            Case "5194"
                                .TransactionIPAddress = ToSafeString(r_oResults(1, iCount))
                            Case "5195"
                                .RefundPasscode = bPMFunc.GetOVal(ToSafeString(r_oResults(1, iCount)))
                            Case "5196"
                                .CaptureMethod = ToSafeString(r_oResults(1, iCount))
                            Case "5197"
                                .RefundPremiumthroughInvoice = ToSafeString(r_oResults(1, iCount))
                            Case "5198"
                                .Donotuseoldcarddetailsforsubsequentpayments = ToSafeString(r_oResults(1, iCount))
                            Case "5199"
                                .MarkDefaultCreditCard = ToSafeString(r_oResults(1, iCount))
                            Case "5203"
                                .LanguageTemplateID = ToSafeString(r_oResults(1, iCount))
                            Case "5204"
                                .MerchantTemplateID = ToSafeString(r_oResults(1, iCount))
                            Case "5205"
                                .AccountPassCode = bPMFunc.GetOVal(ToSafeString(r_oResults(1, iCount)))
                            Case "5241"
                                .PaymentHubServiceUrl = ToSafeString(r_oResults(1, iCount))
                        End Select

                    End With

                Next


            End If
            Return m_nReturn

        Catch excep As System.Exception

            m_nReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_nReturn

        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Sub New()
        MyBase.New()

        Try
            Dim oDatabase As Object = Nothing

            ' Class Initialise
            If m_oDatabase Is Nothing Then
                m_oDatabase = New dPMDAO.Database()
            End If
            m_nReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=oDatabase), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Function GetPaymentHubSystemOptions(ByRef v_oPaymentHubSystemOptions(,) As Object) As Integer

        Const kMethodName As String = "GetPaymentHubSystemOptions"

        Dim oResult As Object = Nothing
        Try
            m_nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=kGetPaymentHUBConfigurationsSql, sSQLName:=kGetPaymentHUBConfigurationsName, bStoredProcedure:=True, vResultArray:=v_oPaymentHubSystemOptions)

            If m_nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kGetPaymentHUBConfigurationsSql & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return m_nReturn

        Catch ex As Exception
            m_nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_nReturn, excep:=ex)
            Return m_nReturn
        End Try

    End Function
    Private Function InitializePaymentHubIntegrationService() As PaymentHub.IntegrationService
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Ssl3
        Dim oPaymentHub As New PaymentHub.IntegrationService
        'Write definition for other security things after getting WCF security implemented

        Dim oSecurityValue As New PaymentHub.Security
        'CyberSource
        'oSecurityValue.SystemUsername = "1323678132"
        'oSecurityValue.SystemPassword = "66FE51AC-6D53-41C5-AF38-EEE8D38F893B"


        ''FatZebra
        oSecurityValue.SystemUsername = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).SystemUserName '("PaymentHub.SystemUsername")
        oSecurityValue.SystemPassword = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).Password 'ConfigurationManager.AppSettings("PaymentHub.Password")

        oSecurityValue.SystemID = 1

        oPaymentHub.SecurityValue = oSecurityValue
        oPaymentHub.Url = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).PaymentHubServiceUrl
        Return oPaymentHub
    End Function
    Private Function InitializePaymentHubClientHeader() As PaymentHub.RequestHeader
        Dim oRequestHeader As New PaymentHub.RequestHeader
        Dim oClientHeader As New PaymentHub.ClientHeader

        oClientHeader.ClientName = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).ClientName
        oClientHeader.BrokerSCID = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).BrokerSCID
        oRequestHeader.ClientHeader = oClientHeader
        Return oRequestHeader
    End Function
    ''' <summary>
    ''' ProcessPurchase
    ''' </summary>
    ''' <param name="strTransactionID"></param>
    ''' <param name="IntegrationToken"></param>
    ''' <param name="TokenID"></param>
    ''' <param name="oPaymentHubResponseParameters"></param>
    ''' <param name="v_dTransactionValue"></param>
    ''' <param name="v_sTransactionCurrencyCode"></param>
    ''' <returns></returns>
    Public Function ProcessPurchase(ByVal strTransactionID As String, ByVal IntegrationToken As String, ByRef TokenID As String,
                                    ByRef oPaymentHubResponseParameters As PaymentHubResponseParameters,
                                    ByVal v_dTransactionValue As Decimal, ByVal v_sTransactionCurrencyCode As String,
                                    Optional ByVal v_nPartyCnt As Integer = 0) As String

        Dim nResult As Integer = 0

        Try
            Dim oPaymentHub = InitializePaymentHubIntegrationService()

            Dim oRequest As New PaymentHub.TransactionData
            Dim oResponce As New PaymentHub.ProviderResponse
            Dim oRequestHeader As PaymentHub.RequestHeader = InitializePaymentHubClientHeader()

            '' UserDetails
            'Dim oUserDetails As New PaymentHub.UserDetails
            'oUserDetails.Username = "development,electra"
            'oUserDetails.Forename = "electra"
            'oUserDetails.Surname = "development"
            'oRequestHeader.UserDetails = oUserDetails


            ''MessageHeader
            Dim oMessageHeader As New PaymentHub.MessageHeader
            oMessageHeader.TransactionID = Guid.NewGuid().ToString() 'strTransactionID
            oMessageHeader.MessageDateTime = System.DateTime.Now
            oRequestHeader.MessageHeader = oMessageHeader

            oRequest.Header = oRequestHeader

            ''ClientHeader
            Dim oTransactionDataBody As New PaymentHub.TransactionDataBody
            Dim oPaymentProviderData As New PaymentHub.PaymentProviderData

            With oPaymentProviderData
                .IntegrationToken = IntegrationToken
                .RequestType = PaymentHub.RequestType.Purchase
                ' .JavascriptEnabled = PaymentHub.JavascriptEnabled.Y

                Dim oMerchant As New PaymentHub.Merchant

                oMerchant.MerchantID = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).MerchantID 'ConfigurationManager.AppSettings("PaymentHub.MerchantID")
                oMerchant.SystemPassCode = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).SystemPasscode
                oMerchant.SystemGUID = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).SystemGUID

                .Merchant = oMerchant
                .ReturnURL = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).ReturnURL
                .AccountID = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).AccountID
                .CaptureMethod = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).CaptureMethod
                If CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).TransactionIPAddress IsNot Nothing Then
                    .TransactionIPAddress = CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).TransactionIPAddress
                End If
                If v_nPartyCnt <> 0 Then
                    Dim oCustomer As New PaymentHub.Customer

                    nResult = GetCustomerAddressAndContacts(v_nPartyCnt, oCustomer, v_dTransactionValue)
                    If nResult <> PMEReturnCode.PMTrue Then
                        Return nResult
                    End If
                    'oCustomer.Firstname = "Michelle"
                    'oCustomer.Lastname = "Rippingale Peters"

                    If CType(m_oPaymentHubConfigurationParameters, PaymentHubConfigurationParameters).ClientName <> "" Then
                        .Customer = oCustomer
                    End If
                End If
                .Description = "Insurance Premium"
                .ProcessingIdentifier = PaymentHub.ProcessingIdentifier.AuthAndCharge
                .HideDeliveryDetails = True
                .HidePaymentResultSuccess = True
                .IsRecurring = False
                Dim oToken As New PaymentHub.Token

                oToken.TokenId = TokenID
                .Token = oToken
                .TransactionValue = v_dTransactionValue
                .TransactionCurrencyCode = GetCurrencyIdFromXml(v_sTransactionCurrencyCode)

            End With
            oTransactionDataBody.PaymentProviderData = oPaymentProviderData
            oRequest.Body = oTransactionDataBody

            'Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.TransactionData))
            'Dim file As New System.IO.StreamWriter("c:\temp\ProcessPurchaseRequest.xml")
            'writer.Serialize(file, oRequest)
            'file.Close()

            oResponce = oPaymentHub.ProcessPurchase(oRequest)

            'Dim writer1 As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.ProviderResponse))
            'Dim file1 As New System.IO.StreamWriter("c:\temp\ProcessPurchaseResponce.xml")
            'writer1.Serialize(file1, oResponce)
            'file1.Close()

            Dim oReturnResponse As New PaymentHubResponseParameters()
            oReturnResponse.ErrorCode = oResponce.Header.ErrorHeader.ErrorCode
            oReturnResponse.ErrorDescription = oResponce.Header.ErrorHeader.ErrorDescription

            If oResponce.Body IsNot Nothing AndAlso oResponce.Body.PaymentProviderResponseData IsNot Nothing Then
                oReturnResponse.ProviderResponseCode = oResponce.Body.PaymentProviderResponseData.ProviderResponseCode
                oReturnResponse.ProviderResponseMessage = oResponce.Body.PaymentProviderResponseData.ProviderResponseMessage
                oReturnResponse.ResultCode = oResponce.Body.PaymentProviderResponseData.ResultCode
                oReturnResponse.TransactionResult = oResponce.Body.PaymentProviderResponseData.TransactionResult
            End If
            If oReturnResponse.TransactionResult <> "0" Then
                nResult = PMEReturnCode.PMFalse


                Dim sbLogMessage As New StringBuilder
                Dim swRequest = New StringWriter()

                Dim writerRequest As New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.TransactionData))
                writerRequest.Serialize(New XmlTextWriter(swRequest), oRequest)
                sbLogMessage.AppendLine("ProcessPurchase executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine(swRequest.ToString() & vbCrLf)
                Dim swResponse = New StringWriter()
                Dim writerResponse = New System.Xml.Serialization.XmlSerializer(GetType(PaymentHub.ProviderResponse))
                writerResponse.Serialize(New XmlTextWriter(swResponse), oResponce)
                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine(swResponse.ToString() & vbCrLf)


                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessPurchase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPurchase", vErrNo:=Informations.Err().Number, vErrDesc:=sbLogMessage.ToString())
            End If
            oPaymentHubResponseParameters = oReturnResponse
            Return oReturnResponse.TransactionResult
        Catch excep As Exception

            nResult = PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessPurchase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPurchase", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private Function GetCustomerAddressAndContacts(v_nPartyCnt As Integer, ByRef v_oCustomer As PaymentHub.Customer, ByVal v_dTransValue As Decimal) As Integer
        Dim nResult As Integer = 0
        Dim oResult(,) As Object = Nothing

        Try

            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=ToSafeString(v_nPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetPartyCorrospondanceAddressCntSQL, sSQLName:=kGetPartyCorrospondanceAddressCntName, bStoredProcedure:=kGetPartyCorrospondanceAddressCntStored, vResultArray:=oResult)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not Informations.IsArray(oResult) Then
                Return nResult
            End If

            Dim nAddressKey As Integer = ToSafeInteger(oResult(0, 0), 0)

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="address_cnt", vValue:=ToSafeString(nAddressKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kAddressSelSQL, sSQLName:=kAddressSelName, bStoredProcedure:=kAddressSelStored, vResultArray:=oResult)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not Informations.IsArray(oResult) Then
                Return nResult
            End If
            Dim oCustomer As New PaymentHub.Customer
            Dim oCustomerAddress As New PaymentHub.CustomerAddress With {
                .Address1 = ToSafeString(oResult(3, 0), ""),
                .Address2 = ToSafeString(oResult(4, 0), ""),
                .Address3 = ToSafeString(oResult(5, 0), ""),
                .Town = ToSafeString(oResult(6, 0), ""),
                .County = "County",
                .Country = ToSafeString(oResult(14, 0), ""),
                .Postcode = ToSafeString(oResult(7, 0), "")
            }
            Dim oDeliveryAddress As New PaymentHub.DeliveryAddress With {
               .Address1 = ToSafeString(oResult(3, 0), ""),
               .Address2 = ToSafeString(oResult(4, 0), ""),
               .Address3 = ToSafeString(oResult(5, 0), ""),
               .Town = ToSafeString(oResult(6, 0), ""),
               .County = "County",
               .Country = ToSafeString(oResult(14, 0), ""),
               .Postcode = ToSafeString(oResult(7, 0), "")
           }
            oCustomer.Address = oCustomerAddress
            oCustomer.DeliveryAddress = oDeliveryAddress
            oCustomer.Basket = New PaymentHub.CustomerBasket
            oCustomer.Basket.ShippingAmount = 0
            oCustomer.Basket.VATAmount = 0
            oCustomer.Basket.TotalAmount = v_dTransValue
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=ToSafeString(v_nPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="contact_type", vValue:=ToSafeString("MEMAIL"), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kSelEmailContactSQL, sSQLName:=kSelEmailContactName, bStoredProcedure:=kSelEmailContactStored, vResultArray:=oResult)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not Informations.IsArray(oResult) Then
                Return nResult
            End If

            oCustomer.Email = ToSafeString(oResult(4, 0), "")
            oCustomer.Firstname = ToSafeString(oResult(6, 0), "")
            oCustomer.Lastname = ToSafeString(oResult(6, 0), "")
            v_oCustomer = oCustomer

            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetCustomerAddressAndContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCustomerAddressAndContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try

    End Function
    Private Function GetCurrencyIdFromXml(ByVal v_sCurrencyCode As String) As Integer
        Dim nCurrencyId As Integer
        Dim result As Integer
        Dim xmlDoc As XmlDocument = New XmlDocument
        Dim PureInstallationFolder As String = ""

        'delete the file regardless of whether the item was in the cache or not as this forces the refresh of the cache
        result = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup,
                                                   v_sSettingName:=gPMConstants.PMRegInstallationFolder, r_sSettingValue:=PureInstallationFolder)

        xmlDoc.Load(PureInstallationFolder + "\Pure\Application" + "\list_one_Currencies.xml")

        Dim nodeList As XmlNodeList = xmlDoc.SelectNodes("/ISO_4217/CcyTbl/CcyNtry[Ccy='" + v_sCurrencyCode + "']")

        'Loop through the selected Nodes.
        For Each node As XmlNode In nodeList
            'Fetch the Node and Attribute values.
            nCurrencyId = ToSafeInteger(node("CcyNbr").InnerText, 0)
            Exit For
        Next
        Return nCurrencyId
    End Function
    ''' <summary>
    ''' AddAndUpdateCashListDetails
    ''' </summary>
    ''' <param name="v_nOldInsuranceFileCnt"></param>
    ''' <param name="v_nNewInsuranceFileCnt"></param>
    ''' <param name="v_sTokenId"></param>
    ''' <param name="dPremiumAmount"></param>
    ''' <returns></returns>
    Public Function AddAndUpdateCashListDetails(v_nOldInsuranceFileCnt As String, v_nNewInsuranceFileCnt As String, v_sTokenId As String, dPremiumAmount As Decimal) As Integer
        Dim nResult As Integer
        Dim oResult(,) As Object = Nothing

        Try
            nResult = PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=ToSafeString(v_nOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=ToSafeString(v_nNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="ccTokenId", vValue:=ToSafeString(v_sTokenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kAddAndUpdateCashListDetailsSQL, sSQLName:=kAddAndUpdateCashListDetailsName, bStoredProcedure:=kAddAndUpdateCashListDetailsStored, vResultArray:=oResult)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not Informations.IsArray(oResult) Then
                Return nResult
            End If

            Dim nCashListKey As Integer = ToSafeInteger(oResult(0, 0), 0)
            Dim nCashListItemKey As Integer = ToSafeInteger(oResult(1, 0), 0)

            nResult = PostAndAllocateCashListItem(nCashListKey, nCashListItemKey, ToSafeInteger(v_nNewInsuranceFileCnt, 0), dPremiumAmount)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
        Catch excep As Exception
            nResult = PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="AddAndUpdateCashListDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAndUpdateCashListDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
        Return nResult

    End Function

    Private Function PostAndAllocateCashListItem(ByVal nCashListKey As Integer, ByVal nCashListItemKey As Integer, ByVal v_nInsuranceFileCnt As Integer, ByVal v_cAmtTobePosted As Decimal) As Integer

        Dim oCashListPost As bACTCashListPost.Automated = Nothing
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue


            ' instantiate the business object
            oCashListPost = New bACTCashListPost.Automated



            nResult = oCashListPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                               iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If (nResult <> PMEReturnCode.PMTrue) Then
                Return nResult
            End If

            ' set the process modes
            nResult = oCashListPost.SetProcessModes(0, 0, 0, 0, Date.Today)
            If (nResult <> PMEReturnCode.PMTrue) Then
                Return nResult
            End If

            ' Post the unallocated cash
            nResult = oCashListPost.PostUnallocatedCash(
                v_vCashListID:=nCashListKey,
                v_vCashListItemID:=nCashListItemKey,
                v_dTransactionDate:=Date.Today)

            If (nResult <> PMEReturnCode.PMTrue) Then
                Return nResult
            End If
            Dim nTransDetailKey = oCashListPost.CashTransId
            Dim nWriteOffReasonID As Integer
            Dim cWriteOffAmount As Decimal = 0
            Dim nAllocationStatus As Integer

            oCashListPost.CashListTransactionID = nTransDetailKey

            nResult = oCashListPost.PostAllocatedCashListItem(lCashListID:=nCashListKey,
lCashListItemId:=nCashListItemKey, lInsuranceFileCnt:=v_nInsuranceFileCnt, sDocumentRef:="", lWriteOffReasonID:=nWriteOffReasonID,
cWriteOffAmount:=cWriteOffAmount, bCurrencyWriteOff:=False, r_iAllocationStatus:=nAllocationStatus, bIsPosted:=True,
cAmtTobePosted:=v_cAmtTobePosted, bSpecificCashListItemId:=True, lPostAccountId:=0)
            If (nResult <> PMEReturnCode.PMTrue) Then
                Return nResult
            End If
        Catch excep As Exception
            nResult = PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="PostAndAllocateCashListItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostAndAllocateCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
        Return nResult
    End Function
End Class
