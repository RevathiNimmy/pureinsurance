Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Text
'refer Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business

    Implements IDisposable


    ' ************************************************
    ' Added to replace global variables 25/11/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    'DC150404 PN11570 -for EDI Header
    Private m_sEDIHeader As String = ""

    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name: GetNextEDIFileName
    '
    ' Description: Get next Edi File Name Based upon comments in PN12900
    '
    ' History: MKW 30/06/2004: Created
    '           AR 13/04/2005: Extended to cater for more than 300 files (PN18055)
    '
    ' ***************************************************************** '
    Private Function GetNextEDIFileName(ByRef sFolder As String, ByRef sFileName As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        If gPMFunctions.FolderExists(sFolder) Then
            For i As Integer = 0 To 2599
                'AR20050414 - PN18055 Keep Xnn,Ynn,Znn naming at first, but extend to Wnn,Vnn ... ,Ann after that
                Select Case i
                    Case Is <= 99
                        sFileName = "X"
                    Case Is <= 199
                        sFileName = "Y"
                    Case Is <= 299
                        sFileName = "Z"
                    Case Else
                        sFileName = Strings.ChrW(CInt(90 - Math.Floor(i / 100))).ToString()
                End Select

                sFileName = "s-tradan." & sFileName & StringsHelper.Format(i Mod 100, "00")

                If Not gPMFunctions.FileExists(sFolder & sFileName) Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                Else
                    sFileName = ""
                End If
            Next i
        End If

        Return result

    End Function

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: ProcessEDIMessage
    '
    ' Description:
    '
    ' DD 06/06/2003: Rewritten for new Instalments
    '
    ' Steve Watton 07/06/2004 Added in cancellation flag as optional parameter
    ' Steve Watton 08/06/2004 Added in NonTransactionalMTA flag as optional parameter
    '
    ' ***************************************************************** '
    Public Function ProcessEDIMessage(ByVal vPremFinanceArray(,) As Object, ByVal vPolicies(,) As Object, ByVal sReTransmit As String, Optional ByVal bIsCancellation As Boolean = False, Optional ByVal bIsNonTransactionalMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vEDIMessageArray() As Object = Nothing
        Dim vEDITransArray(,) As Object = Nothing
        Dim vSourceArray(,) As Object = Nothing
        Dim vPolicyArray(,) As Object = Nothing
        Dim vFPArray(,) As Object = Nothing
        Dim vSchemeArray(,) As Object = Nothing
        Dim vInformationArray(,) As Object = Nothing
        Dim vNewNumber As Object
        Dim sICCSNo As String = ""
        Dim iNoOfPolicies As Integer

        Const k_InformationRateCode As Integer = 0
        Const k_InformationDepositPC As Integer = 1
        Const k_InformationClientTitle As Integer = 2
        Const k_InformationClientForenames As Integer = 3
        Const k_InformationClientSurname As Integer = 4
        Const k_InformationClientDOB As Integer = 5
        Const k_InformationCompanyReg As Integer = 6

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'number of policies returned
            iNoOfPolicies = vPolicies.GetUpperBound(1)

            'Build the master EDI Message Array
            ReDim vEDIMessageArray(k_EDINoOfElements)
            ReDim vEDITransArray(k_EDINoOfTransElements, iNoOfPolicies)

            'Get the EDI information from the Scheme




            m_lReturn = CType(GetSchemeDetails(CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0)), CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0)), CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanSchemeVersion, 0)), vSchemeArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get scheme details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'Get the EDI information from the Rate File (PFRF)



            m_lReturn = CType(GetAdditionalDetails(CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanPFRF_ID, 0)), CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientId, 0)), vInformationArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get scheme details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'Get the Broker Information from the Source table


            m_lReturn = CType(GetSourceDetails(CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0)), vSourceArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Broker/Source details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'Get the Finance Provider


            m_lReturn = CType(GetFPDetails(CInt(vSchemeArray(3, 0)), vFPArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Finance Provider details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'DC290904 PN15338 no use proceeding if no edi message required

            If CInt(vFPArray(4, 0)) = 0 Then
                'No EDI Message To Produce
                Return result
            End If

            'Get ICCSNumber
            m_lReturn = CType(GetICCSNo(sICCSNo), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get ICCS number", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If



            vEDIMessageArray(k_EDIBrokerMailboxNumber) = CStr(vSourceArray(24, 0)).Trim()


            vEDIMessageArray(k_EDIFinanceMailboxNumber) = vSchemeArray(15, 0)

            'Put the number of messages sent number into the correct format


            vNewNumber = Conversion.Val(CStr(vSchemeArray(16, 0))) + 1


            vEDIMessageArray(k_EDINoOfMessagesSent) = vNewNumber

            'Finance Amounts


            vEDIMessageArray(k_EDIDaysDelay) = vPremFinanceArray(bSIRPremFinConst.k_PFPlanDaysDelay, 0)

            vEDIMessageArray(k_EDINoOfTransactions) = iNoOfPolicies + 1


            vEDIMessageArray(k_EDIInterestValue) = Conversion.Val(CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanInterestCost, 0)))


            vEDIMessageArray(k_EDITotalGrossPremium) = Conversion.Val(CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanTotalCost, 0)))

            'MKW 150604 PN12507 START - Should be P for Personal and C for commercial

            If CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientPartyType, 0)).Trim() = gSIRLibrary.SIRPartyTypePersonalClient Then

                vEDIMessageArray(k_EDIBusinessType) = "P"
            ElseIf (CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientPartyType, 0)).Trim() = gSIRLibrary.SIRPartyTypeCorporateClient) Or (CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientPartyType, 0)).Trim() = gSIRLibrary.SIRPartyTypeGroupClient) Then

                vEDIMessageArray(k_EDIBusinessType) = "C"
            Else

                vEDIMessageArray(k_EDIBusinessType) = " "
            End If
            'vEDIMessageArray(k_EDIBusinessType) = Trim(vPremFinanceArray(k_PFPlanTransactionType, 0))
            'MKW 150604 PN12507 END


            vEDIMessageArray(k_EDIFinanceAPR) = Conversion.Val(CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanAPR, 0)))


            vEDIMessageArray(k_EDIDateOfFirstPayment) = vPremFinanceArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0)


            vEDIMessageArray(k_EDIFinanceRate) = vPremFinanceArray(bSIRPremFinConst.k_PFPlanInterestRate, 0)


            vEDIMessageArray(k_EDINoOfInstalments) = Conversion.Val(CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanNoOfInstalments, 0)))


            vEDIMessageArray(k_EDIPreferredCustomerDate) = vPremFinanceArray(bSIRPremFinConst.k_PFPlanDayOfWeekOrMonth, 0)

            'Client Information


            vEDIMessageArray(k_EDIClientContactName) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientName, 0)).Trim()


            vEDIMessageArray(k_EDIClientName) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientName, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr1) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientAddress1, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr2) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr3) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientAddress3, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr4) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientAddress4, 0)).Trim()


            vEDIMessageArray(k_EDIClientPCode) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientPostcode, 0)).Trim()


            vEDIMessageArray(k_EDIClientPhoneNo) = (CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientAreaCode, 0)).Trim() & " " &
                                      CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientPhone, 0)).Trim()).Trim()

            'Bank Information


            vEDIMessageArray(k_EDIBankAccountName) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankAccountName, 0)).Trim()


            vEDIMessageArray(k_EDIBankAccountNumber) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankAccountNo, 0)).Trim()


            vEDIMessageArray(k_EDIBankSortCode) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankSortCode, 0)).Trim()


            vEDIMessageArray(k_EDIBankName) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankName, 0)).Trim()


            vEDIMessageArray(k_EDIBankBranchName) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankBranch, 0)).Trim()


            vEDIMessageArray(k_EDIBankAddr1) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankAddress1, 0)).Trim()


            vEDIMessageArray(k_EDIBankAddr2) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankAddress2, 0)).Trim()


            vEDIMessageArray(k_EDIBankAddr3) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankAddress3, 0)).Trim()


            vEDIMessageArray(k_EDIBankPCode) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBankPostcode, 0)).Trim()

            'Plan Information


            vEDIMessageArray(k_EDIAutoGenPlanRef) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanFinCollPlanRef, 0)).Trim() 'Trim(vPremFinanceArray(k_PFPlanAutoGenPlanRef, 0))

            vEDIMessageArray(k_EDIRateStyle) = "0"


            vEDIMessageArray(k_EDIPlanVersionNumber) = vPremFinanceArray(bSIRPremFinConst.k_PFPlanPFVersion, 0)

            'Broker Information

            vEDIMessageArray(k_EDIBrokerICCSNo) = sICCSNo


            vEDIMessageArray(k_EDIBrokerName) = CStr(vSourceArray(2, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr1) = CStr(vSourceArray(10, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr2) = CStr(vSourceArray(11, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr3) = CStr(vSourceArray(12, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr4) = CStr(vSourceArray(13, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerPostCode) = CStr(vSourceArray(14, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerPhoneNo) = CStr(vSourceArray(16, 0)).Trim() & " " & CStr(vSourceArray(17, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerABICodelist1Ref) = CStr(vSourceArray(25, 0)).Trim()


            vEDIMessageArray(k_EDIPMAssFinCmpyCodelist1Addr) = CStr(vFPArray(0, 0)).Trim()

            vEDIMessageArray(k_EDIBrokerContactName) = ""


            vEDIMessageArray(k_EDIBrokerClientRef) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientCode, 0)).Trim() 'Trim(vPremFinanceArray(k_PFPlanFinCollPlanRef, 0))
            'If Trim(vPremFinanceArray(k_PFPlanFinCollPlanRef, 0)) <> "" Then
            '    vEDIMessageArray(k_EDIBrokerUniqueKey) = Trim(vPremFinanceArray(k_PFPlanFinCollPlanRef, 0))
            'Else
            '    vEDIMessageArray(k_EDIBrokerUniqueKey) = "0"
            'End If


            vEDIMessageArray(k_EDIBrokerUniqueKey) = vPremFinanceArray(bSIRPremFinConst.k_PFPlanClientId, 0)

            'Status fields

            vEDIMessageArray(k_EDIDtOfEDIMessageCreation) = DateTime.Today 'Format(Now, "ddmmyyyy")

            vEDIMessageArray(k_EDITmOfEDIMessageCreation) = DateTime.Now

            vEDIMessageArray(k_EDIReTransmitFlag) = sReTransmit


            vEDIMessageArray(k_EDIPlanRefNumber) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanFinCollPlanRef, 0)).Trim() '""



            vEDIMessageArray(k_EDIFinanceCollatedPlanRef) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanFinCollPlanRef, 0)).Trim()

            'Provider specific fields
            'PN12831 Changes for Benfield
            '    vEDIMessageArray(k_EDISchemeName) = Trim(vPremFinanceArray(k_PFPlanSchemeName, 0))
            '    vEDIMessageArray(k_EDIRateCode) = Trim(vInformationArray(k_InformationRateCode, 0))


            vEDIMessageArray(k_EDISchemeName) = CStr(vInformationArray(k_InformationRateCode, 0)).Trim()

            vEDIMessageArray(k_EDIRateCode) = ""
            'PN12831End


            vEDIMessageArray(k_EDIPlanDepositAmount) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanDeposit, 0)).Trim()


            vEDIMessageArray(k_EDIPlanDepositPercent) = CStr(vInformationArray(k_InformationDepositPC, 0)).Trim()


            vEDIMessageArray(k_EDIClientDOB) = CStr(vInformationArray(k_InformationClientDOB, 0)).Trim()


            vEDIMessageArray(k_EDIClientTitle) = CStr(vInformationArray(k_InformationClientTitle, 0)).Trim()


            vEDIMessageArray(k_EDIClientForenames) = CStr(vInformationArray(k_InformationClientForenames, 0)).Trim()


            vEDIMessageArray(k_EDIClientSurname) = CStr(vInformationArray(k_InformationClientSurname, 0)).Trim()


            vEDIMessageArray(k_EDICompanyReg) = CStr(vInformationArray(k_InformationCompanyReg, 0)).Trim()


            vEDIMessageArray(k_EDICCNumber) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCCNumber, 0)).Trim()


            vEDIMessageArray(k_EDICCExpiry) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCCExpiryDate, 0)).Trim().Replace("/", "")
            'DC230904 PN13944 using wrong field to set flag
            'If conversion.Val(vPremFinanceArray(k_PFPlanPayProtection, 0)) <> 0 Then
            '    vEDIMessageArray(k_EDIPaymentProtection) = "Y"
            'Else
            '    vEDIMessageArray(k_EDIPaymentProtection) = "N"
            'End If

            If Conversion.Val(CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCostOfProtection, 0))) <> 0 Then

                vEDIMessageArray(k_EDIPaymentProtection) = "Y"
            Else

                vEDIMessageArray(k_EDIPaymentProtection) = "N"
            End If



            vEDIMessageArray(k_EDIProviderReference) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanAgentRef, 0)).Trim()


            vEDIMessageArray(k_EDIAuthCode) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanAuthCode, 0)).Trim()
            'PN12594


            vEDIMessageArray(k_EDIBusinessCode) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanBusinessCode, 0)).Trim()

            'PN13915 Start


            'Developer Guide No 98

            vEDIMessageArray(k_EDI_PC_Special1) = " " &
                          FormatEDIField(vEDIMessageArray(k_EDIPlanDepositAmount), 5, "LZ", CStr(2), "Y") &
                          FormatEDIField(vPremFinanceArray(bSIRPremFinConst.k_PFPlanOtherInstalments, 0).Trim(), 1, "LZ", CStr(2), "Y")



            'Developer Guide No 98

            vEDIMessageArray(k_EDI_PC_Special2) = "" &
                                      FormatEDIField(vPremFinanceArray(bSIRPremFinConst.k_PFPlanPFRFMnemonic, 0).Trim(), 5, "LS", CStr(0), "N")


            If CDbl(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCostOfProtection, 0)) > 0 Then


                vEDIMessageArray(k_EDI_PC_Special2) = CStr(vEDIMessageArray(k_EDI_PC_Special2)) &
                                          FormatEDIField(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCostOfProtection, 0), 1, "LZ", CStr(2), "Y")
            End If
            'PN13915 End

            'DC150404 PN11570 added header and footer lines



            m_sEDIHeader = "STX=ANA:1+" & CStr(vSourceArray(24, 0)).Trim() &
                           ":Sirius+" & CStr(vSchemeArray(15, 0)).Trim() & ":receiver+" &
                           DateTime.Now.ToString("yyMMdd") &
                           ":" &
                           DateTime.Now.ToString("HHMMss") &
                           "+" &
                           "T" & StringsHelper.Format(CStr(vNewNumber), "00000") &
                           "+PASSWORD+MEMO01+B'MHD=1+MEMO01:1'MML="

            'Go through each related Transaction
            For iPolicy As Integer = 0 To iNoOfPolicies

                If gPMFunctions.ToSafeLong(CInt(vPolicies(0, iPolicy))) <> 0 Then
                    ' TransType = 'MTA' - PN23648


                    If CDbl(vPremFinanceArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0)) = bSIRPremFinConst.PFThirdParty And (CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanTransactionType, 0)) = "M" Or CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanTransactionType, 0)) = "MTA") Then




                        m_lReturn = CType(GetPolicyDetailsMta(CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanPFCnt, 0)), CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanPFVersion, 0)), CInt(vPolicies(0, iPolicy)), vPolicyArray), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Mta Policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return m_lReturn
                        End If
                    Else


                        m_lReturn = CType(GetPolicyDetails(CInt(vPolicies(0, iPolicy)), vPolicyArray), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return m_lReturn
                        End If
                    End If
                    If Informations.IsArray(vPolicyArray) Then
                        'Fill in the array


                        vEDITransArray(k_EDITransactionFromDate, iPolicy) = vPolicyArray(k_EDIPol_CoverStartDate, 0)


                        vEDITransArray(k_EDITransactionInsurer, iPolicy) = CStr(vPolicyArray(k_EDIPol_Insurer, 0)).Trim()


                        vEDITransArray(k_EDITransactionRiskDescription, iPolicy) = CStr(vPolicyArray(k_EDIPol_RiskCode, 0)).Trim()


                        vEDITransArray(k_EDITransactionFees, iPolicy) = vPolicyArray(k_EDIPol_Fees, 0)


                        vEDITransArray(k_EDITransactionExtras, iPolicy) = vPolicyArray(k_EDIPol_Extras, 0)


                        vEDITransArray(k_EDITransactionToDate, iPolicy) = vPolicyArray(k_EDIPol_ExpiryDate, 0)


                        vEDITransArray(k_EDITransactionType, iPolicy) = CStr(vPremFinanceArray(bSIRPremFinConst.k_PFPlanTransactionType, 0)).Trim()

                        Select Case vEDITransArray(k_EDITransactionType, iPolicy)
                            Case "NB"

                                vEDIMessageArray(k_EDIRateCode) = "PRO"
                            Case "REN"

                                vEDIMessageArray(k_EDIRateCode) = "RNC"
                            Case "M", "MTA"

                                vEDIMessageArray(k_EDIRateCode) = "ADJ"
                        End Select

                        If bIsCancellation Then

                            vEDITransArray(k_EDITransactionType, iPolicy) = "RC"

                            vEDIMessageArray(k_EDIRateCode) = "CAN"
                        End If

                        If bIsNonTransactionalMTA Then

                            vEDITransArray(k_EDITransactionType, iPolicy) = "AP"

                            vEDIMessageArray(k_EDIRateCode) = "ADJ"
                        End If



                        vEDITransArray(k_EDITransactionUniquePolicyNumber, iPolicy) = CStr(vPolicyArray(k_EDIPol_InsurerRef, 0)).Trim()


                        vEDITransArray(k_EDITransactionPolicyNumber, iPolicy) = CStr(vPolicyArray(k_EDIPol_InsurerRef, 0)).Trim()


                        vEDITransArray(k_EDITransactionAmount, iPolicy) = vPolicyArray(k_EDIPol_TotalPremium, 0)


                        vEDITransArray(k_EDITransactionInsurerABICode, iPolicy) = vPolicyArray(k_EDIPol_Insurer_ABI, 0)


                        vEDITransArray(k_EDITransactionRiskABICode, iPolicy) = vPolicyArray(k_EDIPol_Business_ABI, 0)
                    End If
                End If

            Next iPolicy

            'DD 13/11/2003: Process the array.
            'The Definition is taken from the Financial Provider.


            m_lReturn = CType(ProcessArray(CInt(vFPArray(4, 0)), vEDIMessageArray, vEDITransArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Increment the Scheme EDI Message Count



            m_lReturn = CType(IncrementEDIMessageCount(CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0)), CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0)), CInt(vPremFinanceArray(bSIRPremFinConst.k_PFPlanSchemeVersion, 0))), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessEDIMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function ProcessAccidentCareEDIMessage(ByVal lInsuranceFileCnt As Integer, ByVal lPartyCnt As Integer, Optional ByVal sReTransmit As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ProcessAccidentCareEDIMessage
        ' PURPOSE: Generate an AccidentCare EDI Message
        ' AUTHOR: Danny Davis
        ' DATE: 26 November 2003, 15:36:12
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vEDIMessageArray() As Object = Nothing
        Dim vSourceArray(,) As Object = Nothing
        Dim vInformationArray(,) As Object = Nothing
        Dim sICCSNo As String = String.Empty
        Dim sValue As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim vResultArray(,) As Object = Nothing
        Dim vSplit As Object = Nothing
        Dim lMessageCount As Integer
        Dim sAccidentCareMailbox As String = String.Empty
        Dim sAccidentCareABIID As String = String.Empty
        Dim sTopLevelObjectName As String = String.Empty
        Dim sErrorMessage As String = String.Empty



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Build the master EDI Message Array
            ReDim vEDIMessageArray(k_EDINoOfElements)

            'Get the AccidentCare EDI Mailbox
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=104, r_sOptionValue:=sAccidentCareMailbox), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Get the AccidentCare ABI ID
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=105, r_sOptionValue:=sAccidentCareABIID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Get the EDI Message Count
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=103, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Increment it.
            lMessageCount = CInt(Conversion.Val(sValue) + 1)

            'Transmission fields

            vEDIMessageArray(k_EDIFinanceMailboxNumber) = sAccidentCareMailbox

            vEDIMessageArray(k_EDIPMAssFinCmpyCodelist1Addr) = sAccidentCareABIID

            vEDIMessageArray(k_EDINoOfMessagesSent) = "T" & StringsHelper.Format(CStr(lMessageCount), "00000")

            vEDIMessageArray(k_EDIDtOfEDIMessageCreation) = DateTime.Now.ToString("ddMMyyyy")

            vEDIMessageArray(k_EDITmOfEDIMessageCreation) = DateTime.Now

            vEDIMessageArray(k_EDIReTransmitFlag) = sReTransmit

            'Get the Broker Information from the Source table

            m_lReturn = CType(GetSourceDetails(m_iSourceID, vSourceArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Broker/Source details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccidentCareEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'Get ICCSNumber
            m_lReturn = CType(GetICCSNo(sICCSNo), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get ICCS number", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccidentCareEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'Get the Insured Person and Policy Information

            m_lReturn = CType(GetAccidentCareDetails(lInsuranceFileCnt, vInformationArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Additional information", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccidentCareEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return m_lReturn
            End If

            'Broker Information


            vEDIMessageArray(k_EDIBrokerMailboxNumber) = CStr(vSourceArray(24, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerABICodelist1Ref) = CStr(vSourceArray(25, 0)).Trim()

            vEDIMessageArray(k_EDIBrokerUniqueKey) = lInsuranceFileCnt

            vEDIMessageArray(k_EDIBrokerICCSNo) = sICCSNo


            vEDIMessageArray(k_EDIBrokerName) = CStr(vSourceArray(2, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr1) = CStr(vSourceArray(10, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr2) = CStr(vSourceArray(11, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr3) = CStr(vSourceArray(12, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerAddr4) = CStr(vSourceArray(13, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerPostCode) = CStr(vSourceArray(14, 0)).Trim()


            vEDIMessageArray(k_EDIBrokerPhoneNo) = CStr(vSourceArray(16, 0)).Trim() &
                                      CStr(vSourceArray(17, 0)).Trim() & CStr(vSourceArray(18, 0)).Trim()
            'DC290304 PN11062 use broker name


            vEDIMessageArray(k_EDIBrokerContactName) = CStr(vSourceArray(2, 0)).Trim()


            'Steve Watton 01/06/2004
            'Added in constants to identify array elements for the information array

            'Insured Person Information


            vEDIMessageArray(k_EDIClientTitle) = CStr(vInformationArray(k_EDIInfo_Title, 0)).Trim()


            vEDIMessageArray(k_EDIClientForenames) = CStr(vInformationArray(k_EDIInfo_Forenames, 0)).Trim()


            vEDIMessageArray(k_EDIClientSurname) = CStr(vInformationArray(k_EDIInfo_Surname, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr1) = CStr(vInformationArray(k_EDIInfo_Address1, 0)).Trim()

            'DC150404 PN11570 added other address lines


            vEDIMessageArray(k_EDIClientAddr2) = CStr(vInformationArray(k_EDIInfo_Address2, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr3) = CStr(vInformationArray(k_EDIInfo_Address3, 0)).Trim()


            vEDIMessageArray(k_EDIClientAddr4) = CStr(vInformationArray(k_EDIInfo_Address4, 0)).Trim()

            'Steve Watton 01/06/2004 PN 12139 added in postcode as it has been missing from the EDI message


            vEDIMessageArray(k_EDIClientPCode) = CStr(vInformationArray(k_EDIInfo_PostCode, 0)).Trim()

            'DC300604 PN12139 use GII phone numbers if they exist

            If CStr(vInformationArray(k_EDIInfo_GIIHomeTelephone, 0)).Trim() <> "" Then


                vEDIMessageArray(k_EDIClientPhoneNo) = CStr(vInformationArray(k_EDIInfo_GIIHomeTelephone, 0)).Trim()
            Else

                If CStr(vInformationArray(k_EDIInfo_GIIWorkTelephone, 0)).Trim() <> "" Then


                    vEDIMessageArray(k_EDIClientPhoneNo) = CStr(vInformationArray(k_EDIInfo_GIIWorkTelephone, 0)).Trim()
                Else


                    vEDIMessageArray(k_EDIClientPhoneNo) = CStr(vInformationArray(k_EDIInfo_Telephone, 0)).Trim()
                End If
            End If

            'DC050804 PN13913 added GII date of birth

            If CStr(vInformationArray(k_EDIInfo_GIIDOB, 0)).Trim() <> "" And Not CStr(vInformationArray(k_EDIInfo_GIIDOB, 0)).Trim().EndsWith("1899") Then

                Dim TempDate As Date

                vEDIMessageArray(k_EDIClientPhoneNo) = If(DateTime.TryParse(CStr(vInformationArray(k_EDIInfo_GIIDOB, 0)).Trim(), TempDate), TempDate.ToString("ddMMyy"), CStr(vInformationArray(k_EDIInfo_GIIDOB, 0)).Trim())
            Else

                If CStr(vInformationArray(k_EDIInfo_DOB, 0)).Trim().EndsWith("1899") Then

                    vEDIMessageArray(k_EDIClientDOB) = Nothing
                Else

                    Dim TempDate2 As Date

                    vEDIMessageArray(k_EDIClientDOB) = If(DateTime.TryParse(CStr(vInformationArray(k_EDIInfo_DOB, 0)).Trim(), TempDate2), TempDate2.ToString("ddMMyy"), CStr(vInformationArray(k_EDIInfo_DOB, 0)).Trim())
                End If
            End If


            vEDIMessageArray(k_EDIInsurerName) = CStr(vInformationArray(k_EDIInfo_InsurerName, 0)).Trim()


            vEDIMessageArray(k_EDIPolicyNumber) = CStr(vInformationArray(k_EDIInfo_PolicyNumber, 0)).Trim()


            vEDIMessageArray(k_EDITotalGrossPremium) = CStr(vInformationArray(k_EDIInfo_PolicyPremium, 0)).Trim()

            'DC270304 PN11062 -edi message was incorrect
            '    'Translate the Scheme Extra label to a product type
            '    If InStr(LCase(vInformationArray(10, 0)), "bronze") > 0 Then
            '        vEDIMessageArray(k_EDIProductType) = "1"
            '    ElseIf InStr(LCase(vInformationArray(10, 0)), "silver") > 0 Then
            '        vEDIMessageArray(k_EDIProductType) = "2"
            '    ElseIf InStr(LCase(vInformationArray(10, 0)), "gold") > 0 Then
            '        vEDIMessageArray(k_EDIProductType) = "3"
            '    End If
            'description of product given not number


            vEDIMessageArray(k_EDIProductType) = vInformationArray(k_EDIInfo_ACType, 0)
            'new fields for extra value and start date


            vEDIMessageArray(k_EDIExtraValue) = vInformationArray(k_EDIInfo_ACPremium, 0)

            Dim TempDate3 As Date

            vEDIMessageArray(k_EDIPolicyStartDate) = If(DateTime.TryParse(CStr(vInformationArray(k_EDIInfo_PolicyStartDate, 0)).Trim(), TempDate3), TempDate3.ToString("ddMMyyyy"), CStr(vInformationArray(k_EDIInfo_PolicyStartDate, 0)).Trim())

            'DC150404 PN11570 added header and footer lines

            m_sEDIHeader = "STX=ANA:1+" & CStr(vSourceArray(24, 0)).Trim() &
                           ":Sirius+" & sAccidentCareMailbox.Trim() & ":receiver+" &
                           DateTime.Now.ToString("yyMMdd") &
                           ":" &
                           DateTime.Now.ToString("HHMMss") &
                           "+" &
                           "T" & StringsHelper.Format(CStr(lMessageCount), "00000") &
                           "+PASSWORD+MEMO01+B'MHD=1+MEMO01:1'MML="

            'See if GII is installed
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=11, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Extract the GII information (this has to be embedded SQL because a system may not have
            'GII installed and the therefore the tables won't exist for a SP)
            If sValue = "1" Then

                'SJ 13/07/2004 - start
                'G2V2 Changes
                m_lReturn = CType(GetTopLevelObjectName(r_sErrorMessage:=sErrorMessage, r_sTopLevelObjectName:=sTopLevelObjectName, v_sGisDataModelCode:="GIIMotor"), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                'SJ 13/07/2004 - end
                ' SQL
                sSQL = "SELECT " &
                       "GIIMVEHICLE.reg_no," &
                       "GIIMVEHICLE.model_name," &
                       "GIIMVEHICLE.date_first_regd," &
                       "GIIMSAVED_QUOTE.total_excess," &
                       "GIIMCOVER.code " &
                       "FROM GIIMPOLICY " &
                       "INNER JOIN " &
                       "GIIMSAVED_QUOTE on GIIMPolicy." & sTopLevelObjectName & "_ID = " &
                       "GIIMSAVED_QUOTE." & sTopLevelObjectName & "_ID " &
                       "INNER JOIN " &
                       "GIIMCOVER on GIIMPolicy." & sTopLevelObjectName & "_ID = GIIMCOVER." & sTopLevelObjectName & "_ID " &
                       "INNER JOIN " &
                       "GIIMVEHICLE on GIIMPolicy." & sTopLevelObjectName & "_ID = GIIMVEHICLE." & sTopLevelObjectName & "_ID " &
                       "INNER JOIN " &
                       "GIS_POLICY_LINK on GIIMPolicy." & sTopLevelObjectName & "_ID = " &
                       "GIS_POLICY_LINK.gis_policy_link_id " &
                       "WHERE GIS_POLICY_LINK.Insurance_File_Cnt=" & CStr(lInsuranceFileCnt)

                With m_oDatabase
                    .Parameters.Clear()
                    m_lReturn = .SQLSelect(sSQL, "Select GII Information", False, , vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Warn the user through the log only
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not retrieve Gemini information from the database. Should Gemini be switched on?", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccidentCareEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If
                End With

                If Informations.IsArray(vResultArray) Then

                    'Put the values in the array


                    vEDIMessageArray(k_EDIVehicleReg) = vResultArray(0, 0)

                    'Split out the Make and Model

                    vEDIMessageArray(k_EDIVehicleMake) = ""

                    vEDIMessageArray(k_EDIVehicleModel) = ""


                    vSplit = CStr(vResultArray(1, 0)).Split("/"c)
                    If Informations.IsArray(vSplit) Then


                        vEDIMessageArray(k_EDIVehicleMake) = vSplit(0)

                        If vSplit.GetUpperBound(0) > 0 Then
                            vEDIMessageArray(k_EDIVehicleModel) = vSplit(1)
                        End If
                    End If

                    'Extract the year
                    If Informations.IsDate(vResultArray(2, 0)) Then
                        'Convert the year to 2 digit suffix


                        vEDIMessageArray(k_EDIVehicleYear) = CStr(CDate(vResultArray(2, 0)).Year).Substring(CStr(CDate(vResultArray(2, 0)).Year).Length - 2)
                    Else

                        vEDIMessageArray(k_EDIVehicleYear) = ""
                    End If

                    'The volutary excess


                    vEDIMessageArray(k_EDIPolicyExcess) = StringsHelper.Format(Conversion.Val(CStr(vResultArray(3, 0))), "0000")

                    'Policy Cover Type
                    'DC270304 PN11062 give code not numeric
                    'vEDIMessageArray(k_EDIPolicyCoverType) = Format(conversion.Val(vResultArray(4, 0)), "000")
                    'DC130404 PN11062 was using wrong array to check

                    If Conversion.Val(CStr(vResultArray(4, 0))) = 1 Then

                        vEDIMessageArray(k_EDIPolicyCoverType) = "COMP"
                    ElseIf Conversion.Val(CStr(vResultArray(4, 0))) = 2 Then

                        vEDIMessageArray(k_EDIPolicyCoverType) = "TPFT"
                    ElseIf Conversion.Val(CStr(vResultArray(4, 0))) = 3 Then

                        vEDIMessageArray(k_EDIPolicyCoverType) = "TPO "

                        vEDIMessageArray(k_EDIPolicyCoverType) = "    "
                    End If

                End If
            End If

            'The Definition is hardcoded for Accidentcare.

            m_lReturn = CType(ProcessArray(6, vEDIMessageArray, Nothing), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Update the System Option count for EDI Messages
            With m_oDatabase
                sSQL = "UPDATE System_Options SET value=" & lMessageCount &
                       " WHERE option_number=103 and branch_id=1"

                .Parameters.Clear()
                m_lReturn = .SQLAction(sSQL, "Update Accidentcare EDI Message count", False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End With
            Return result


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccidentCareEDIMessage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetFPDetails
    '
    ' Description: Returns the information about the Finance Provider.
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetFPDetails(ByVal lPartyCnt As Integer, ByRef r_vFPArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the FP Information
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'Developer Guide No 39
            m_lReturn = .SQLSelect("spe_party_finance_provider_sel", "Get Finance Provider", True, , r_vFPArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetSourceDetails
    '
    ' Description: Returns the information about the Broker from the
    '              Source table.
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetSourceDetails(ByVal lSourceID As Integer, ByRef r_vSourceArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the Broker Information from the Source table
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("source_id", CStr(lSourceID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PM_Select_source", "Get Source Information", True, , r_vSourceArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPolicyDetails
    '
    ' Description: Returns the information about a Policy
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetPolicyDetails(ByVal lInsuranceFileCnt As Integer, ByRef r_vPolicyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get underlying policy details from the insurance_file table
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("InsuranceFileCnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFEDI_GetPolicyDetails", "Get Policy " &
                        "Details", True, , r_vPolicyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPolicyDetailsMta
    '
    ' Description: Returns the information about a Policy ta
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetPolicyDetailsMta(ByVal lPFprem_finance_cnt As Integer, ByVal lPFprem_finance_version As Integer, ByVal lInsuranceFileCnt As Integer, ByRef r_vPolicyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get underlying policy details from the insurance_file table
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("PFprem_finance_cnt", CStr(lPFprem_finance_cnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("PFprem_finance_version", CStr(lPFprem_finance_version), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("InsuranceFileCnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFEDI_GetPolicyDetailsMta", "Get Policy " &
                        "Details", True, , r_vPolicyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDefinition
    '
    ' Description: Returns the information about an Instalments Scheme
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetDefinition(ByVal lDefinitionID As Integer, ByVal sSection As String, ByRef r_vDefintionArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the definition for the section
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("PFEDIDefinition_id", CStr(lDefinitionID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("Section", sSection, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFEDI_GetMessageDefinition", "Get Field Definitions", True, , r_vDefintionArray)
        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetSchemeDetails
    '
    ' Description: Returns the information about an Instalments Scheme
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetSchemeDetails(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByRef r_vSchemeArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the EDI information from the Scheme
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("CompanyNo", CStr(lCompanyNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("SchemeNo", CStr(lSchemeNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("SchemeVersion", CStr(lSchemeVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFScheme_sel", "Get Scheme", True, , r_vSchemeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetAccidentCareDetails
    '
    ' Description: Returns the additional information about an Insured Client.
    '              This is for custom information required by AccidentCare.
    '
    ' History: DD 26/11/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetAccidentCareDetails(ByVal lInsuranceFileCnt As Integer, ByRef r_vInformationArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the EDI information from the Insurance File
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("insurance_file_cnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFEDI_GetAccidentCareInformation", "Get AccidentCare EDI information", True, , r_vInformationArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetAdditionalDetails
    '
    ' Description: Returns the additional information about a Client
    '              or PFRF record. This is for custom information required
    '              by different Premium Finance Providers
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function GetAdditionalDetails(ByVal lPFRFID As Integer, ByVal lPartyCnt As Integer, ByRef r_vInformationArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the EDI information from the Scheme
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("pfrf_id", CStr(lPFRFID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFEDI_GetAdditionalInformation", "Get Additional EDI information", True, , r_vInformationArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: IncrementEDIMessageCount
    '
    ' Description: Increments the EDI Count on an Instalments Scheme
    '
    ' History: DD 10/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function IncrementEDIMessageCount(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the EDI information from the Scheme
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("CompanyNo", CStr(lCompanyNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("SchemeNo", CStr(lSchemeNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("SchemeVersion", CStr(lSchemeVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No 39
            m_lReturn = .SQLSelect("spu_PFEDI_IncrementMessageCount", "Get Scheme", True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessArray
    '
    ' Description: Get the field length definitions from the database
    '              and reprocess the standard array ready to export it.
    '
    ' History: DD 09/06/2003: Rewritten
    '
    ' ***************************************************************** '
    Private Function ProcessArray(ByVal lDefinitionID As Integer, ByVal vEDIMessageArray() As Object, ByVal vEDITransArray(,) As Object) As Integer
        Dim Err_ProcessArray As Boolean = False

        Dim result As Integer = 0
        Dim iBodyField, iOffset As Integer
        Dim vBodyDefinitions(,) As Object = Nothing
        Dim vTransDefinitions As Object = Nothing
        Dim vEDIExportArray As Object = Nothing
        Dim sEDIExport As String = ""

        Try
            Err_ProcessArray = True

            result = gPMConstants.PMEReturnCode.PMTrue

            'Initialise
            sEDIExport = ""
            iOffset = 0

            'Get the definitions for each section
            'FOR POSSIBLE FUTURE USE
            'm_lReturn = GetDefinition(lDefinitionID, "H", vHeaderDefinitions)
            'If m_lReturn <> PMTrue Then
            '    ProcessArray = m_lReturn
            '    Exit Function
            'End If


            m_lReturn = CType(GetDefinition(lDefinitionID, "B", vBodyDefinitions), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Informations.IsArray(vEDITransArray) Then

                m_lReturn = CType(GetDefinition(lDefinitionID, "T", vTransDefinitions), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            'Build the EDI export array
            If Informations.IsArray(vEDITransArray) Then
                'Steve Watton PN Issue 12145, 28/05/2004. The export array was being incorrectly sized


                ReDim vEDIExportArray(vBodyDefinitions.GetUpperBound(1) + (vEDITransArray.GetUpperBound(0) + 1) * 15)
            Else

                ReDim vEDIExportArray(vBodyDefinitions.GetUpperBound(1))
            End If
            'Build the Body elements

            For iBodyField = 0 To vBodyDefinitions.GetUpperBound(1)

                If CDbl(vBodyDefinitions(k_EDIDef_ArrayIndex, iBodyField)) < 0 Then





                    'Changes done as per Vb code
                    'vEDIExportArray(FormatEDIField("", CInt(vBodyDefinitions(k_EDIDef_ColumnSize, iBodyField)), CStr(vBodyDefinitions(k_EDIDef_ColumnType, iBodyField)), CStr(vBodyDefinitions(k_EDIDef_DecimalAccuracy, iBodyField)), CStr(vBodyDefinitions(k_EDIDef_SignedField, iBodyField))), vBodyDefinitions(k_EDIDef_OutputIndex, iBodyField))
                    vEDIExportArray(vBodyDefinitions(k_EDIDef_OutputIndex, iBodyField)) =
                    FormatEDIField("",
                    vBodyDefinitions(k_EDIDef_ColumnSize, iBodyField),
                    vBodyDefinitions(k_EDIDef_ColumnType, iBodyField),
                    vBodyDefinitions(k_EDIDef_DecimalAccuracy, iBodyField),
                    vBodyDefinitions(k_EDIDef_SignedField, iBodyField))

                Else






                    'Changes done as per Vb code
                    'vEDIExportArray(FormatEDIField(vEDIMessageArray(CInt(vBodyDefinitions(k_EDIDef_ArrayIndex, iBodyField))), CInt(vBodyDefinitions(k_EDIDef_ColumnSize, iBodyField)), CStr(vBodyDefinitions(k_EDIDef_ColumnType, iBodyField)), CStr(vBodyDefinitions(k_EDIDef_DecimalAccuracy, iBodyField)), CStr(vBodyDefinitions(k_EDIDef_SignedField, iBodyField))), vBodyDefinitions(k_EDIDef_OutputIndex, iBodyField))
                    vEDIExportArray(vBodyDefinitions(k_EDIDef_OutputIndex, iBodyField)) =
                    FormatEDIField(vEDIMessageArray(vBodyDefinitions(k_EDIDef_ArrayIndex,
                    iBodyField)),
                    vBodyDefinitions(k_EDIDef_ColumnSize, iBodyField),
                    vBodyDefinitions(k_EDIDef_ColumnType, iBodyField),
                    vBodyDefinitions(k_EDIDef_DecimalAccuracy, iBodyField),
                    vBodyDefinitions(k_EDIDef_SignedField, iBodyField))
                End If
            Next iBodyField

            'Build the transaction elements.
            'These are made up of 15 x vEDITransArray elements appended to the end of the
            'Export Array
            If Informations.IsArray(vEDITransArray) andalso vEDITransArray(0,0) IsNot Nothing Then
                For iTransIndex As Integer = 0 To 14
                    'Steve Watton PN Issue 12145, 28/05/2004. We have been overwriting the last element
                    'Of the previous vEDITransArray.

                    iOffset = (iTransIndex * (vEDITransArray.GetUpperBound(0) + 1)) + iBodyField

                    For iTransField As Integer = 0 To vEDITransArray.GetUpperBound(0)


                        If CDbl(vTransDefinitions(k_EDIDef_ArrayIndex, iTransField)) < 0 Or iTransIndex > vEDITransArray.GetUpperBound(1) Then






                            'Changes done as per VB code
                            'vEDIExportArray(FormatEDIField("", CInt(vTransDefinitions(k_EDIDef_ColumnSize, iTransField)), CStr(vTransDefinitions(k_EDIDef_ColumnType, iTransField)), CStr(vTransDefinitions(k_EDIDef_DecimalAccuracy, iTransField)), CStr(vTransDefinitions(k_EDIDef_SignedField, iTransField))), iOffset + CDbl(vTransDefinitions(k_EDIDef_OutputIndex, iTransField)))
                            vEDIExportArray(iOffset + vTransDefinitions(k_EDIDef_OutputIndex, iTransField)) _
                            =
                            FormatEDIField("",
                            vTransDefinitions(k_EDIDef_ColumnSize, iTransField),
                            vTransDefinitions(k_EDIDef_ColumnType, iTransField),
                            vTransDefinitions(k_EDIDef_DecimalAccuracy, iTransField),
                            vTransDefinitions(k_EDIDef_SignedField, iTransField))

                        Else






                            vEDIExportArray(iOffset + vTransDefinitions(k_EDIDef_OutputIndex, iTransField)) _
                            =
                            FormatEDIField(vEDITransArray(vTransDefinitions(k_EDIDef_ArrayIndex,
                            iTransField), iTransIndex),
                            vTransDefinitions(k_EDIDef_ColumnSize, iTransField),
                            vTransDefinitions(k_EDIDef_ColumnType, iTransField),
                            vTransDefinitions(k_EDIDef_DecimalAccuracy, iTransField),
                            vTransDefinitions(k_EDIDef_SignedField, iTransField))
                        End If
                    Next iTransField
                Next iTransIndex
            End If

            'Write the EDI Array to the file

            m_lReturn = CType(WriteEDIFile(vEDIExportArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception
            If Not Err_ProcessArray Then
                Throw excep
            End If


            'Developer Guide No 32


            If Err_ProcessArray Then


                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
        End Try
    End Function


    Private Function FormatEDIField(ByVal vValue As Object, ByVal iSize As Integer, ByVal sType As String, ByVal sDecimalPlaces As String, ByVal sSigned As String) As String
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: FormatEDIField
        ' PURPOSE: Formats an incoming value into an EDI result based on the
        '          parameters.
        ' AUTHOR: Danny Davis
        ' DATE: 10/06/2003, 15:47
        ' RETURNS: Formatted Result
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As String = String.Empty

        Try


            Select Case sType
                'Padded String (Left Aligned)
                Case "LS"
                    If vValue.ToString.Trim.Length > iSize Then

                        result = Informations.Left(vValue, iSize)
                    Else
                        result = CStr(vValue).Trim & New String(" "c, iSize - vValue.ToString.Trim.Length) ' Space(iSize - Len(Trim(vValue)))
                    End If
                    'Zero Padded Number
                Case "LZ"

                    If Convert.ToString(vValue) = "" Then
                        vValue = "0"
                    End If

                    If sDecimalPlaces = "0" Or sDecimalPlaces = "" Then
                        result = String.Format(vValue, Fill(iSize, "0"))
                        'result = String.Format("{0:D" & iSize & "}", vValue)

                    Else
                        If CStr(vValue).IndexOf(".") > 0 Then

                            'MKW 040304 PN10141 Changed format to correctly state values.
                            'FormatEDIField = Format(vValue, Fill(iSize, "0")) & "." & _
                            'Format(Mid(vValue, InStr(vValue, ".") + 1), Fill(CInt(sDecimalPlaces), "0"))
                            result = String.Format(vValue, Fill(iSize, "0") & "." & Fill(CInt(sDecimalPlaces), "0"))
                        Else
                            result = String.Format(vValue, Fill(iSize, "0")) & "." &
                                Fill(CInt(sDecimalPlaces), "0")
                        End If
                    End If

                    'Remove sign from non-signed fields
                    If vValue < 0 And sSigned <> "Y" Then
                        result = result.Replace("-", "")
                    End If

                    'Date
                Case "DT"
                    Select Case iSize
                        Case 8
                            result = String.Format(vValue, "yyyymmdd")
                        Case Else
                    End Select

                    'Time
                Case "TM"
                    Select Case iSize
                        Case 6
                            result = String.Format(vValue, "hhnnss")
                        Case Else
                    End Select

                    'Pass-through cropped to size (if necessary)
                Case Else

                    result = Informations.Left(vValue, iSize)
            End Select
            Return result
        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatEDIField", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = ""

                    Return result
            End Select
        Finally
        End Try



        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------


        Return result


    End Function


    Private Function Fill(ByRef iNumber As Integer, ByRef sChar As String) As String
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Fill
        ' PURPOSE: Produce a filled string with a number of characters
        ' AUTHOR: Danny Davis
        ' DATE: 10/06/2003, 17:29
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As String = String.Empty

        Try
            Dim sResult As New StringBuilder

            sResult = New StringBuilder("")

            For iCount As Integer = 1 To iNumber
                sResult.Append(sChar)
            Next iCount

            result = sResult.ToString()

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Fill", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    Return result
            End Select
        Finally
        End Try
        Return result


    End Function


    ' ***************************************************************** '
    '
    ' Name: WriteEDIFile
    '
    ' Description: Writes the contents of the EDI Export Array to a CSV
    '              file.
    '
    ' History: DD 11/06/2003: Created
    '
    ' ***************************************************************** '
    Private Function WriteEDIFile(ByVal vEDIExportArray() As Object) As Integer

        Dim result As Integer = 0
        Dim iFileNumber As Integer
        Dim sEDIString As New StringBuilder
        Dim sFilePath As String = String.Empty
        Dim sFileName As String = String.Empty




        result = gPMConstants.PMEReturnCode.PMTrue

        'MKW PN 12900 START - Rename Files Based upon Specifications

        '    'Need to generate a random number using the bPMGenerateNumber business object
        '
        '    ' Create an instance if needed
        '    If ((oGenerateNumber Is Nothing) = True) Then
        '
        '        ' New instance of component services
        '
        '        ' Get an instance of bSIRGenerateNumber
        '        m_lReturn = gPMComponentServices.CreateBusinessObject( _
        ''                r_oObject:=oGenerateNumber, _
        ''                v_sClassName:="bPMAutoNumber.Business", _
        ''                v_sCallingAppName:=ACApp, _
        ''                v_sUsername:=m_sUsername$, _
        ''                v_sPassword:=m_sPassword$, _
        ''                v_iUserID:=m_iUserID%, _
        ''                v_iSourceID:=m_iSourceID%, _
        ''                v_iLanguageID:=m_iLanguageID%, _
        ''                v_iCurrencyID:=m_iCurrencyID%, _
        ''                v_iLogLevel:=m_iLogLevel%, _
        ''                v_oDatabase:=m_oDatabase)
        '
        '        If (m_lReturn <> PMTrue) Then
        '            WriteEDIFile = PMFalse
        '            ' Log Error Message
        '            LogMessage m_sUsername, _
        ''                    iType:=PMLogOnError, _
        ''                    sMsg:="Failed to get instance of bSIRGenerateNumber.Business", _
        ''                    vApp:=ACApp, _
        ''                    vClass:=ACClass, _
        ''                    vMethod:="WriteEDIFile", _
        ''                    vErrNo:=Err.Number, _
        ''                    vErrDesc:=Err.Description
        '            Exit Function
        '        End If
        '
        '    End If
        '
        '    'Call the GetNumberRange function in the AutoNum business object
        '    m_lReturn = oGenerateNumber.GetNumberRange("EDIMESSAGE", "EDI", PMProductCode(PMProductFamily), lNumber)
        '
        '    If m_lReturn <> PMTrue Then
        '        oGenerateNumber.Terminate
        '        Set oGenerateNumber = Nothing
        '        WriteEDIFile = m_lReturn
        '        Exit Function
        '    End If
        '
        '    'Call the GenerateNumber function in AutoNum passing the vNumber variable
        '    m_lReturn = oGenerateNumber.GenerateNumber(lNumber, m_iUserID, lAutoNumber)
        '
        '    If m_lReturn <> PMTrue Then
        '        oGenerateNumber.Terminate
        '        Set oGenerateNumber = Nothing
        '        WriteEDIFile = m_lReturn
        '        Exit Function
        '    End If
        '
        '    'Set the object to nothing as there is no need for it anymore
        '
        '    oGenerateNumber.Terminate
        '    Set oGenerateNumber = Nothing
        '
        '    'Format the generated unique number in the correct format
        '    m_lReturn = GetEDINumber(lNumber:=lAutoNumber, sEDINumber:=sFileName)
        '
        '    If m_lReturn <> PMTrue Then
        '        WriteEDIFile = m_lReturn
        '        Exit Function
        '    End If

        'MKW PN 12900 END - Rename Files Based upon Specifications

        'Need to get the path name from the registry
        m_lReturn = CType(gPMFunctions.GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, gPMConstants.PMERegSettingLevel.pmeRSLServer, "EDIPathName", sFilePath), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get EDI directory from registry: HKLM\Pure\SiriusSolutions\Server\EDIPathName", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteEDIFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'if the path is not found in the registry then exit the function
        If sFilePath = "" Then
            'this means that the path has not been specified by the user
            result = gPMConstants.PMEReturnCode.PMNotFound

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EDI directory is not set in the registry: HKLM\Pure\SiriusSolutions\Server\EDIPathName", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteEDIFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'if the directory doesn't exist then create the directory
        If Not gPMFunctions.FolderExists(sFilePath) Then
            m_lReturn = CType(CreateDirectories(sFilePath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create EDI directory: " & sFilePath, vApp:=ACApp, vClass:=ACClass, vMethod:="WriteEDIFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If

        If Not sFilePath.EndsWith("\") Then
            sFilePath = sFilePath & "\"
        End If

        'MKW PN 12900 START - Rename Files Based upon Specifications
        m_lReturn = CType(GetNextEDIFileName(sFilePath, sFileName), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Next Edi File Name", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteEDIFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        sFileName = sFilePath & sFileName

        'Store the name of the file in the following variable
        'sFileName = sFilePath & sFileName & ".edi"

        'MKW PN 12900 END - Rename Files Based upon Specifications

        'Open the file for writing to
        iFileNumber = FileSystem.FreeFile()
        FileSystem.FileOpen(iFileNumber, sFileName, OpenMode.Append)

        'Build the EDI String
        'DC150404 PN11570 -added header to EDI message
        sEDIString = New StringBuilder(m_sEDIHeader.Trim())

        For iArrayCount As Integer = vEDIExportArray.GetLowerBound(0) To vEDIExportArray.GetUpperBound(0)

            sEDIString.Append(CStr(vEDIExportArray(iArrayCount)) & ",")
        Next iArrayCount

        'Remove the last comma
        sEDIString = New StringBuilder(sEDIString.ToString().Substring(0, sEDIString.ToString().Length - 1))

        'DC150404 PN11570 -added footer to EDI message
        sEDIString.Append("'MTR=3'END=1'")

        'Write the lot out
        FileSystem.PrintLine(iFileNumber, sEDIString.ToString())
        FileSystem.FileClose(iFileNumber)

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 28/06/2000 MSB - Created.
    '          DD 06/06/2003: Updated for passing through DB connection.
    '
    ' ***************************************************************** '
    '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            'Initialise Objects
            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, PMProductFamily, m_bCloseDatabase, m_oDatabase, vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 28/06/2000 MSB - Created.
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    'UPGRADE_NOTE: (7001) The following declaration (GetEDINumber) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetEDINumber(ByVal lNumber As Integer, ByRef sEDINumber As String) As Integer
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetEDINumber
    ' PURPOSE: This function gets the Unique number for the name of the EDI file.
    '          This function was extracted from the bDistributeDocument in Gemini 1
    ' AUTHOR:
    ' DATE: 12/06/2003, 10:40
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    '
    'Dim result As Integer = 0
    '
    'On Error GoTo Catch_Renamed
    '
    'Dim iTemp, iTemp2 As Integer
    'Dim sNumber As String = ""
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'iTemp2 = lNumber Mod 36
    '
    'If iTemp2 < 10 Then
    'sNumber = Strings.Chrw(48 + iTemp2).ToString()
    'Else
    'sNumber = Strings.Chrw(55 + iTemp2).ToString()
    'End If
    '
    'iTemp = (lNumber - iTemp2) / 36
    '
    'iTemp2 = iTemp Mod 36
    '
    'If iTemp2 < 10 Then
    'sNumber = Strings.Chrw(48 + iTemp2).ToString() & sNumber
    'Else
    'sNumber = Strings.Chrw(55 + iTemp2).ToString() & sNumber
    'End If
    '
    'iTemp = (iTemp - iTemp2) / 36
    '
    'If iTemp < 10 Then
    'sNumber = Strings.Chrw(48 + iTemp).ToString() & sNumber
    'Else
    'sNumber = Strings.Chrw(55 + iTemp).ToString() & sNumber
    'End If
    '
    'sEDINumber = sNumber
    '
    'GoTo Finally_Renamed
    '
    '----------------------------------------------------------------------------------------
    'Only for Debugging, the code will never execute this line
    '----------------------------------------------------------------------------------------
    'Resume 
    '
    'Catch_Renamed: '
    'Select Case InformationS.Err().Number
    'Case Else
    ' Log Error.
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEDINumber", vErrNo:=InformationS.Err().Number, vErrDesc:=InformationS.Err().Description)
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'GoTo Finally_Renamed
    'End Select
    '
    'Finally_Renamed: '
    'Return result
    '
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: CreateDirectories
    '
    ' Description:
    '
    ' History: 03/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Private Function CreateDirectories(ByRef sPathName As String) As Integer

        Dim result As Integer = 0
        Dim iPosition As Integer
        Dim vNameOfDir As String = ""
        Dim iStartPos, iEndPos As Integer
        Dim vDriveLetter As String = ""
        Dim vDirectory, sPathString As String
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        sPathString = sPathName
        iStartPos = 4
        iEndPos = sPathString.Length

        vDriveLetter = Informations.Mid(sPathString, 1, 3)
        vDirectory = vDriveLetter

        For iCount As Integer = iStartPos To iEndPos

            iPosition = Informations.inStr(iStartPos, sPathString, "\")

            If iPosition = 0 Then
                vNameOfDir = Informations.Mid(sPathString, iStartPos, iEndPos)
                vDirectory = vDirectory & "\" & vNameOfDir
                If Not Directory.Exists(vDirectory) Then
                    Directory.CreateDirectory(vDirectory)
                End If
                Exit For
            End If

            vNameOfDir = Informations.Mid(sPathString, iStartPos, iPosition - iStartPos)

            If iCount = 4 Then
                vDirectory = vDirectory & vNameOfDir
            Else
                vDirectory = vDirectory & "\" & vNameOfDir
            End If

            If Not Directory.Exists(vDirectory) Then
                Directory.CreateDirectory(vDirectory)
            End If

            iStartPos = iPosition + 1

        Next iCount

        lReturn = gPMConstants.PMEReturnCode.PMTrue

        Return result

    End Function

    Private Function GetICCSNo(ByRef r_sICCSNo As String) As Integer
        Dim Catch_Renamed As Boolean = False
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetICCSNo
        ' PURPOSE: Returns the Sirius ICCS number
        ' AUTHOR: Danny Davis
        ' DATE: 10/06/2003, 14:26
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try
            Catch_Renamed = True

            ' Select ICCSNo SQL

            result = gPMConstants.PMEReturnCode.PMTrue


            With m_oDatabase

                .Parameters.Clear()

                .Parameters.Add(sName:="iccs", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLAction(ACGetICCSNoSQL, ACGetICCSNoName, True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sICCSNo = ""
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sICCSNo = .Parameters.Item("iccs").Value
            End With


            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            Exit Function

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            'Developer Guide No 32


            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetICCSNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        Exit Function
                End Select

            End If

        End Try
    End Function
End Class
