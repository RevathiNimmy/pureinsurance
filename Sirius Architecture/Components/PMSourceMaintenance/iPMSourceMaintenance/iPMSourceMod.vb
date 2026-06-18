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
    ' Date: 11th July 1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMSource"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACListTitle1 As Integer = 102
    Public Const ACListTitle2 As Integer = 103

    Public Const ACMainTabTitle0 As Integer = 111
    Public Const ACMainTabTitle1 As Integer = 112
    Public Const ACExtensionCaption As Integer = 113
    Public Const ACAddress1Caption As Integer = 114
    Public Const ACAddress2Caption As Integer = 115
    Public Const ACAddress3Caption As Integer = 116
    Public Const ACAddress4Caption As Integer = 117
    Public Const ACPostalCodeCaption As Integer = 118
    Public Const ACCountryCaption As Integer = 119
    Public Const ACPhoneCaption As Integer = 120
    Public Const ACFaxCaption As Integer = 121
    Public Const ACAreaCodeCaption As Integer = 122
    Public Const ACNumberCaption As Integer = 123
    Public Const ACDescriptionCaption As Integer = 124
    Public Const ACCodeCaption As Integer = 125
    Public Const ACParentCaption As Integer = 126
    Public Const ACRegNo1Caption As Integer = 127
    Public Const ACRegNo2Caption As Integer = 128
    Public Const ACBaseCurrencyCaption As Integer = 129
    Public Const ACEmailCaption As Integer = 130
    Public Const ACVatNoCaption As Integer = 131
    Public Const ACSenderMailboxIdCaption As Integer = 132
    Public Const ACBrokerABIIdCaption As Integer = 133
    Public Const ACUserLicenceIdCaption As Integer = 134
    Public Const ACPMSourceNumberCaption As Integer = 135
    Public Const ACDefaultIndicatorCaption As Integer = 136
    Public Const ACMainTabTitle2 As Integer = 137
    Public Const ACMainTabTitle3 As Integer = 138
    Public Const ACMainTabTitle4 As Integer = 139
    Public Const ACCurrenciesPLCaption As Integer = 140
    Public Const ACCompanyCategoryCaption As Integer = 141
    Public Const ACStaffWordingCaption As Integer = 142
    Public Const ACClosedBranchTab As Integer = 143
    Public Const ACBankType As Integer = 144
    'DC150606 PN28906 for broking show Tax Number not VAT Number (Datasure)
    Public Const ACTaxNoCaption As Integer = 145

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACRemoveButton As Integer = 205
    Public Const ACEditButton As Integer = 206
    Public Const ACRatesButton As Integer = 207
    Public Const ACCloseButton As Integer = 208

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACAllowTaskCaption As Integer = 304

    Public Const ACAllowTempMTA As Integer = 305
    Public Const ACAllowPermMTA As Integer = 306
    Public Const ACAllowReports As Integer = 307
    Public Const ACAllowClaims As Integer = 308
    Public Const ACAllowAccounts As Integer = 309

    '040506 Datasure
    Public Const ACTaxNumber As Integer = 310
    ' Menus

    ' Constants for the List data array subscripts.
    'BB
    Public Const ACSubSourceID As Integer = 0
    Public Const ACSubCode As Integer = 1
    Public Const ACSubDescription As Integer = 2
    Public Const ACSubCaptionID As Integer = 3
    Public Const ACSubParentID As Integer = 4
    Public Const ACSubRegNo1 As Integer = 5
    Public Const ACSubRegNo2 As Integer = 6
    Public Const ACSubAddress1 As Integer = 7
    Public Const ACSubAddress2 As Integer = 8
    Public Const ACSubAddress3 As Integer = 9
    Public Const ACSubAddress4 As Integer = 10
    Public Const ACSubPostalCode As Integer = 11
    Public Const ACSubCountryID As Integer = 12
    Public Const ACSubPhoneAreaCode As Integer = 13
    Public Const ACSubPhoneNumber As Integer = 14
    Public Const ACSubPhoneExtension As Integer = 15
    Public Const ACSubFaxAreaCode As Integer = 16
    Public Const ACSubFaxNumber As Integer = 17
    Public Const ACSubFaxExtension As Integer = 18
    Public Const ACSubBaseCurrency As Integer = 19
    Public Const ACSubEmail As Integer = 20
    Public Const ACSubVatNo As Integer = 21
    Public Const ACSubSenderMailboxId As Integer = 22
    Public Const ACSubBrokerABIId As Integer = 23
    Public Const ACSubUserLicenceId As Integer = 24
    Public Const ACSubPMSourceNumber As Integer = 25
    Public Const ACSubDefaultIndicator As Integer = 26
    Public Const ACSubIsDeleted As Integer = 27
    Public Const ACSubEffectiveDate As Integer = 28
    Public Const ACFSA_CompanyCategory As Integer = 29 'MKW010903 PS255 - FSA Compliance
    Public Const ACFSA_StaffWording As Integer = 30 'MKW010903 PS255 - FSA Compliance
    Public Const ACUnderwritingBranchInd As Integer = 31

    Public Const ACChkAllowTempMTA As Integer = 32
    Public Const ACChkAllowPermMTA As Integer = 33
    Public Const ACChkAllowReports As Integer = 34
    Public Const ACChkAllowClaims As Integer = 35
    Public Const ACChkAllowAccounts As Integer = 36
    Public Const ACFSABankType As Integer = 37 'FSA Phase III
    Public Const ACUniqueId As Integer = 38
    Public Const ACScreenHierarchy As Integer = 39
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

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    'Product Family Name for Help

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

    'Screen context ID's for help
    Public Const ScreenHelpID1 As Integer = 2000
    Public Const ScreenHelpID2 As Integer = 3000
    Public Const ScreenHelpID3 As Integer = 1000

    'MKW290803 -fsa compliance
    Public m_lReturn As Integer


    Public Sub Main()

    End Sub

    'MKW290803 -PS255 -fsa compliance
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

    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module
