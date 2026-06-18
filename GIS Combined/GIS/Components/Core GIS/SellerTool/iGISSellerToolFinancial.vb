Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Financial_NET.Financial")> _
Public NotInheritable Class Financial
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Financial
    '
    ' Date: 29/07/1999
    '
    ' Description:
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Financial"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' GIS Data Set
    Private m_oDataSet As cGISDataSetControl.Application

    ' List Manager
    Private m_oListManager As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property








    'RFC190400 - Add Lookup Methods to GIS


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDataSet = New cGISDataSetControl.Application()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If m_oListManager IsNot Nothing Then
                    m_oListManager.Dispose()
                    m_oListManager = Nothing
                End If
                m_oDataSet = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub




    ' ***************************************************************** '
    ' Name: BankAccountValidation
    '
    ' Description: Perform BankAccountValidation transaction
    '
    ' Author: CJB 101000
    '
    '
    ' Notes: This function is used to validate the bank account details
    '        passed to it are valid and that the named person will be allowed
    '        finance. It is used when the payment method is DD and is called just
    '        before Datacash and Transact. Note that this is performed by building
    '        an XML message to send to Prompt. The response we receive back will
    '        determine if to proceed or not.
    '
    ' ***************************************************************** '
    Public Function BankAccountValidation(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sSenderID As String, ByVal v_sCoverType As String, ByVal v_sGnetClientCode As String, ByVal v_sBusinessStatus As String, ByVal v_sBankAccountName As String, ByVal v_sBankAccountNo As String, ByVal v_sBankSortCode As String, ByVal v_sQuoteReference As String, ByRef r_vStatusCode As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML As String = ""

        Dim lPolicyLinkID As Integer
        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lPolicyLinkID = m_oDataSet.PolicyLinkID()

            ' Set the Action Properties
            lReturn = CType(FormatActionXMLFinancial(v_lAction:=iGISSharedConstants.GISDSActionFinancialBankAccountValidation, v_sSellerGUID:="", v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, r_sActionXML:=sActionXML, v_lPolicyLinkID:=lPolicyLinkID, v_sBankAccountValidationSenderID:=v_sSenderID, v_sBankAccountValidationCoverType:=v_sCoverType, v_sGnetClientCode:=v_sGnetClientCode, v_sBusinessStatus:=v_sBusinessStatus, v_sBankAccountValidationBankAccountName:=v_sBankAccountName, v_sBankAccountValidationBankAccountNo:=v_sBankAccountNo, v_sBankAccountValidationBankSortCode:=v_sBankSortCode, v_sQuoteReference:=v_sQuoteReference), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML

            lReturn = CType(UnFormatActionReturnXMLFinancial(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vBankAccountValidationStatusCode:=CStr(r_vStatusCode)), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BankAccountValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PremiumFinanceQuote
    '
    ' Description: Perform PremiumFinanceQuote transaction
    '
    ' Author: CJB 061201 Rewrite (initially written 12/12/00 but never used)
    '
    ' Notes: Initiates the sending of an XML message to a payment house (this is Prompt
    '        at the time of writing) to obtain revised payment plan information when an
    '        MTA is done (although this could be used for other business processes such as
    '        NB etc). If the payment house would accept the change to the finance (which
    '        could either be the customer wishing to borrow more on the plan or borrow less
    '        on the plan in the case of a refund) then the new instalment amounts would be
    '        returned and displayed to the customer. If they then acceped it and proceeded
    '        with the MTA in this case then we would send a PremiumFinanceMTATransactRequest
    '        message to Prompt in the MTATransactAfter method to actually instruct Prompt to
    '        change the plan.
    '
    ' Edit History: CJB 060802 Add r_vInterestAmount return parameter
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceQuote(ByVal v_vDataModelCode As Object, ByVal v_vBusinessTypeCode As Object, ByVal v_vBusinessStatus As Object, ByVal v_vPremiumAmount As Object, ByVal v_vPremiumFinanceRef As Object, ByVal v_vEffectiveDate As Object, ByVal v_vPolicyNo As Object, ByRef r_vStatusCode As Object, ByRef r_vStatusExplanation As Object, ByRef r_vTotalPayable As Object, ByRef r_vNumberOfInstalmentsLeft As Object, ByRef r_vFirstInstalAmt As Object, ByRef r_vSubsequentInstalAmt As Object, ByRef r_vActualPaymentDate As Object, ByRef r_vInterestAmount As Object) As Integer

        Try

            'Pass parameters as class name, method name then ordered data parameters...

            Return GISCall("Financial", "PremiumFinanceQuote", v_vDataModelCode, v_vBusinessTypeCode, v_vBusinessStatus, v_vPremiumAmount, v_vPremiumFinanceRef, v_vEffectiveDate, v_vPolicyNo, r_vStatusCode, r_vStatusExplanation, r_vTotalPayable, r_vNumberOfInstalmentsLeft, r_vFirstInstalAmt, r_vSubsequentInstalAmt, r_vActualPaymentDate, r_vInterestAmount)

        Catch excep As System.Exception



            ' Log Error Message
            iGISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CalcPaymentMethodCharge
    '
    ' Description: Perform CalcPaymentMethodCharge transaction
    '
    ' Author: CJB 181000 (I may have written it but I didn't design it !!)
    '
    '
    ' Notes: This function is used to call the Premium Finance Rating Module
    '        (bGISPremiumFinance) passing to it an amount to finance (amongst
    '        other things that it uses to select the correct rating information
    '        from the gis_pfscheme and gis_pfrf tables in the relevant GIS dB).
    '        It outputs the calculated premium finance information as well as
    '        information on the premium finance scheme selected etc.
    '        Note that this method may be called at any point in the web pages
    '        as soon as the amount to finance is known. Be aware that if this is
    '        at the point of quoting then you may use the AfterQuote method in the
    '        BOM (see I4M BOM for an example) to automatically call the Premium
    '        Finance Rating Module and store the o/p information directly in the
    '        dataset. (Note it was not possible to do this in this component as
    '        we do not have access to the dataset and it was not possible to use
    '        the BOM's AfterQuote event as the solution (Xel) had an interactive
    '        quote screen that allowed the user to change the amount to finance
    '        (without requoting) by selecting different Vol XS amounts and Add-Ons.

    '        Note that at the time of writing the following I/P parameters are for
    '        possible future use in the Premium Finance Rating Module:
    '           v_sNoOfInstalments
    '           v_sRequestedDepositPercent
    '           v_sActionType (must always be set to "Quote")
    '
    ' ***************************************************************** '
    Public Function CalcPaymentMethodCharge(ByVal v_sProductFamily As String, ByVal v_sBusinessTypeCode As String, ByVal v_sDataModelCode As String, ByVal v_sTransactionType As String, ByVal v_sPaymentMethod As String, ByVal v_sStartDate As String, ByVal v_sAmountToFinance As String, ByVal v_sNoOfInstalments As String, ByVal v_sActionType As String, ByVal v_sRequestedDepositPercent As String, ByRef r_vInterestRate As Object, ByRef r_vAPR As Object, ByRef r_vInterestCost As Object, ByRef r_vNoOfInstalments As Object, ByRef r_vFirstInstalment As Object, ByRef r_vOthInstalments As Object, ByRef r_vDeposit As Object, ByRef r_vArrangementFee As Object, ByRef r_vDepositPercent As Object, ByRef r_vCompanyName As Object, ByRef r_vCompanyNo As Object, ByRef r_vSchemeName As Object, ByRef r_vSchemeNo As Object, ByRef r_vSchemeVer As Object, ByRef r_vBasisOfCalc As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML As String = ""

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties
            lReturn = CType(FormatActionXMLFinancial(v_lAction:=iGISSharedConstants.GISDSActionFinancialCalcPaymentMethodCharge, v_sSellerGUID:="", v_sDataModelCode:=v_sDataModelCode, r_sActionXML:=sActionXML, v_sCalcPaymentMethodChargeProductFamily:=v_sProductFamily, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sCalcPaymentMethodChargeTransactionType:=v_sTransactionType, v_sCalcPaymentMethodChargePaymentMethod:=v_sPaymentMethod, v_sCalcPaymentMethodChargeStartDate:=v_sStartDate, v_sCalcPaymentMethodChargeAmountToFinance:=v_sAmountToFinance, v_sCalcPaymentMethodChargeNoOfInstalments:=v_sNoOfInstalments, v_sCalcPaymentMethodChargeActionType:=v_sActionType, v_sCalcPaymentMethodChargeRequestedDepositPercent:=v_sRequestedDepositPercent), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML


            lReturn = CType(UnFormatActionReturnXMLFinancial(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vCalcPaymentMethodChargeInterestRate:=CStr(r_vInterestRate), r_vCalcPaymentMethodChargeAPR:=CStr(r_vAPR), r_vCalcPaymentMethodChargeInterestCost:=CStr(r_vInterestCost), r_vCalcPaymentMethodChargeNoOfInstalments:=CStr(r_vNoOfInstalments), r_vCalcPaymentMethodChargeFirstInstalment:=CStr(r_vFirstInstalment), r_vCalcPaymentMethodChargeOthInstalments:=CStr(r_vOthInstalments), r_vCalcPaymentMethodChargeDeposit:=CStr(r_vDeposit), r_vCalcPaymentMethodChargeArrangementFee:=CStr(r_vArrangementFee), r_vCalcPaymentMethodChargeDepositPercent:=CStr(r_vDepositPercent), r_vCalcPaymentMethodChargeCompanyName:=CStr(r_vCompanyName), r_vCalcPaymentMethodChargeCompanyNo:=CStr(r_vCompanyNo), r_vCalcPaymentMethodChargeSchemeName:=CStr(r_vSchemeName), r_vCalcPaymentMethodChargeSchemeNo:=CStr(r_vSchemeNo), r_vCalcPaymentMethodChargeSchemeVer:=CStr(r_vSchemeVer), r_vCalcPaymentMethodChargeBasisOfCalc:=CStr(r_vBasisOfCalc)), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcPaymentMethodCharge Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcPaymentMethodCharge", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Datacash
    '
    ' Description: Perform Datacash transaction
    '
    ' Author: CL190500 (home)
    '
    '
    ' Notes: Ref should be unique for EVERY new transaction
    ' unless fulfilling a transaction.
    '
    ' If a Switch card, then supply the issue no OR start date
    ' in v_sSwitchExtraInfo
    '
    ' SPW 170603 Change to the way datacash handles start date/issue no
    ' datacash XML API now supports both start date and issue no and will
    ' work out for itself which is required.
    ' ***************************************************************** '
    Public Function Datacash(ByVal v_sDataModelCode As String, ByVal v_sRequestType As String, ByVal v_sRef As String, ByVal v_sCardNum As String, ByVal v_iExpMonth As Integer, ByVal v_iExpYear As Integer, ByVal v_sAmt As String, ByVal v_sSwitchExtraInfo As String, ByVal v_sAuthCode As String, ByVal v_sTransactionType As String, ByRef r_vResponseArray As Object, Optional ByVal v_sStartDate As String = "", Optional ByVal v_sCV2Code As String = "", Optional ByVal v_vBillingAddressArray() As Object = Nothing, Optional ByVal v_vCustomerAddressArray() As Object = Nothing, Optional ByVal v_sCustomerPhoneNo As String = "", Optional ByVal v_sCustomerForename As String = "", Optional ByVal v_sCustomerSurname As String = "", Optional ByVal v_sCustomerEmail As String = "", Optional ByVal v_sCustomerVehicleReg As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim lPolicyLinkID As Integer



        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lPolicyLinkID = m_oDataSet.PolicyLinkID()

            'sDataModelCode = m_oDataSet.GISDataModelCode
            If Not Information.IsArray(v_vCustomerAddressArray) Then
                ReDim v_vCustomerAddressArray(iGISSharedConstants.GISDatacashAddressPostcode)
            End If
            If Not Information.IsArray(v_vBillingAddressArray) Then
                ReDim v_vBillingAddressArray(iGISSharedConstants.GISDatacashAddressPostcode)
            End If

            ' Set the Action Properties


            lReturn = CType(FormatActionXMLFinancial(v_sDataModelCode:=v_sDataModelCode, v_lAction:=iGISSharedConstants.GISDSActionFinancialDatacash, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_iDatacashRequestType:=v_sRequestType, v_sDatacashRef:=v_sRef, v_sDatacashCardNum:=v_sCardNum, v_iDatacashExpMonth:=v_iExpMonth, v_iDatacashExpYear:=v_iExpYear, v_sDatacashAmt:=v_sAmt, v_sDatacashAuthCode:=v_sAuthCode, v_sDatacashSwitchExtraInfo:=v_sSwitchExtraInfo, v_lPolicyLinkID:=lPolicyLinkID, v_sTransactionType:=v_sTransactionType, v_sCalcPaymentMethodChargeStartDate:=v_sStartDate, v_sCV2Code:=v_sCV2Code, v_sAVSStreet_Add1:=CStr(v_vBillingAddressArray(iGISSharedConstants.GISDatacashAddressHouseNo)), v_sAVSStreet_Add2:=CStr(v_vBillingAddressArray(iGISSharedConstants.GISDatacashAddressStreet)), v_sAVSStreet_Add3:=CStr(v_vBillingAddressArray(iGISSharedConstants.GISDatacashAddressTown)), v_sAVSStreet_Add4:=CStr(v_vBillingAddressArray(iGISSharedConstants.GISDatacashAddressCounty)), v_sAVSPostcode:=CStr(v_vBillingAddressArray(iGISSharedConstants.GISDatacashAddressPostcode)), v_sCustomerStreet_Add1:=CStr(v_vCustomerAddressArray(iGISSharedConstants.GISDatacashAddressHouseNo)), v_sCustomerStreet_Add2:=CStr(v_vCustomerAddressArray(iGISSharedConstants.GISDatacashAddressStreet)), v_sCustomerStreet_Add3:=CStr(v_vCustomerAddressArray(iGISSharedConstants.GISDatacashAddressTown)), v_sCustomerStreet_Add4:=CStr(v_vCustomerAddressArray(iGISSharedConstants.GISDatacashAddressCounty)), v_sCustomerPostcode:=CStr(v_vCustomerAddressArray(iGISSharedConstants.GISDatacashAddressPostcode)), v_sCustomerPhoneNo:=v_sCustomerPhoneNo, v_sCustomerForename:=v_sCustomerForename, v_sCustomerSurname:=v_sCustomerSurname, v_sCustomerEmail:=v_sCustomerEmail, v_sVehicleReg:=v_sCustomerVehicleReg), gPMConstants.PMEReturnCode)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML

            lReturn = CType(UnFormatActionReturnXMLFinancial(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vDatacashResponseArray:=r_vResponseArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
