Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 23/06/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' RAW 18/12/2002 : PS187 : Add new constants for WHTax & Payment Details
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iSIRPartyIN"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 103
    Public Const ACIDReference As Integer = 104
    Public Const ACName As Integer = 105
    Public Const ACAgencyNumber As Integer = 106
    Public Const ACCurrency As Integer = 107
    Public Const ACTermsOfPayment As Integer = 110
    Public Const ACIsReinsurer As Integer = 111
    Public Const ACBinderIndicator As Integer = 112
    Public Const ACReportIndicator As Integer = 113
    Public Const ACStatements As Integer = 114
    Public Const ACfraReInsurance As Integer = 115
    Public Const ACReInsuranceType As Integer = 116
    Public Const ACDebitCredit As Integer = 117
    Public Const ACDefaultCommission As Integer = 118

    'RWH(24/07/2000) RSAIB Process 004
    ' Form Constants for Address ListView
    Public Const ACAddressListPostCode As Integer = 121
    Public Const ACAddressListUsage As Integer = 122
    Public Const ACAddressListLine1 As Integer = 123
    Public Const ACAddressListLine2 As Integer = 124
    Public Const ACAddressListLine3 As Integer = 125
    Public Const ACAddressListLine4 As Integer = 126

    'TN07072000
    Public Const ACTreatyNumber As Integer = 119
    Public Const ACInterfaceTitle2 As Integer = 120

    'JMK 19/10/2001
    Public Const ACBindingAuthority As Integer = 127
    Public Const ACfraInsurance As Integer = 128
    Public Const ACInsuranceType As Integer = 129
    Public Const ACInsDebitCredit As Integer = 130

    ' RAW 18/12/2002 : PS187 : Added
    Public Const ACfraPaymentTaxation As Integer = 132
    Public Const ACWithholdingTaxType As Integer = 133
    Public Const ACWithholdingTaxRate As Integer = 134
    Public Const ACTaxRegNumber As Integer = 135
    Public Const ACTaxCode As Integer = 136
    Public Const ACPaymentMethod As Integer = 137
    Public Const ACPaymentFrequency As Integer = 138
    Public Const ACBankAccount As Integer = 139
    ' RAW 18/12/2002 : PS187 : End

    ' TF031298
    Public Const ACFinancial As Integer = 150
    Public Const ACNotes As Integer = 153
    Public Const ACLetter As Integer = 154

    'DC150803 -PS254 -fsa compliance
    Public Const ACfraFSA As Integer = 155
    Public Const ACFSAStatus As Integer = 156
    Public Const ACFSARegistrationNumber As Integer = 157
    Public Const ACFSACreditRating As Integer = 158

    'S4BDAT005
    Public Const ACTabTitle5 As Integer = 159
    Public Const ACClaimsRatingDescription As Integer = 160

    Public Const ACInsurerType As Integer = 161

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206
    Public Const ACBureauButton As Integer = 207
    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACHeadOfficeMissing As Integer = 305
    Public Const ACRefExists As Integer = 306
    Public Const ACInvalidTaxRate As Integer = 307 ' RAW 18/12/2002 : PS187 : added
    Public Const ACBankAccountRequired As Integer = 308 ' RAW 18/12/2002 : PS187 : added
    Public Const ACInvalidPaymentMethod As Integer = 309 ' RAW 18/12/2002 : PS187 : added

    Public Const ACPaymentGroupTitle As Integer = 310
    Public Const ACPaymentGroupMsg As Integer = 311
    Public Const ACInsurerPaymentLocking As Integer = 312

    Public Const AC_CAPTION_RTA_CAN_BE_CHANGED As Integer = 313
    Public Const ACTabTitle4 As Integer = 314

    'Menus
    'Images
    Public Const AddressImage As String = "AddressImage"
    Public Const ContactImage As String = "ContactImage"

    Public Const m_ksWithholdingTaxCodeSpecial As String = "SPL" ' RAW 18/12/2002 : PS187 : Added
    Public Const m_ksPaymentMethodCodeNotApplicable As String = "NAP" ' RAW 18/12/2002 : PS187 : Added
    Public Const m_ksPaymentMethodCodeEFT As String = "EFT" ' RAW 18/12/2002 : PS187 : Added

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    'Global reference to ListManager
    'Global g_oListManager As Object

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGis As Object
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public m_lReturn As Integer

    Public Const ScreenHelpID As Integer = 30

    '****************************************
    ' Party Detail Array Position Constants
    Public Const kPartyDetailTaxNumber As Integer = 0
    Public Const kPartyDetailDomiciledForTax As Integer = 1
    Public Const kPartyDetailTaxExempt As Integer = 2
    Public Const kPartyDetailTaxPercentage As Integer = 3
    '****************************************

    Sub Main_Renamed()

    End Sub

    'DC150803 -PS254 -fsa compliance
    Public Function GetHiddenOption(ByVal v_lSourceId As Integer, Optional ByRef r_vEnableFSACompliance As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Link Account Executives To Commission

            If Not Information.IsNothing(r_vEnableFSACompliance) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=v_lSourceId, r_vUnderwriting:=vValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If vValue <> "" Then
                    r_vEnableFSACompliance = (CBool(vValue))
                Else
                    r_vEnableFSACompliance = False
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC110706 PN set the New Zealand Configuration option
    Public Function GetHiddenOptionNZ(ByVal v_lSourceId As Integer, Optional ByRef r_vEnableNZConfig As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsNothing(r_vEnableNZConfig) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, v_vBranch:=v_lSourceId, r_vUnderwriting:=vValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptionNZ")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If vValue <> "" Then
                    r_vEnableNZConfig = (CBool(vValue))
                Else
                    r_vEnableNZConfig = False
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOptionNZ Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptionNZ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

End Module
