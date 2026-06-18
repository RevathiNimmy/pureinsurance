Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports bGISSchemeBusiness
Imports DocumentFormat.OpenXml.ExtendedProperties
Imports DocumentFormat.OpenXml.Presentation
Imports DocumentFormat.OpenXml.Wordprocessing
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Financial_NET.Financial")>
Public NotInheritable Class Financial
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Application
    '
    ' Date: 07/04/1999
    '
    ' Description:
    '
    ' Edit History:
    '
    'RFC13012000 - Return Guaranteed Quote Date
    'RFC13012000 - Tidy up EffectiveDate required by GetSchemes
    'CJB08022000 - Change PostcodeSearch function
    ' ***************************************************************** '

    ' ************************************************
    Public Const m_cCompanyName As Integer = 0
    Public Const m_cSchemeName As Integer = 1
    Public Const m_cSchemeNo As Integer = 2
    Public Const m_cSchemeVer As Integer = 3
    Public Const m_cStartDate As Integer = 4
    Public Const m_cEndDate As Integer = 5
    Public Const m_cProdClass As Integer = 6
    Public Const m_cTransType As Integer = 7
    Public Const m_cAmountToFinance As Integer = 8
    Public Const m_cAPR As Integer = 9
    Public Const m_cIntRate As Integer = 10
    Public Const m_cDaysDelay As Integer = 11
    Public Const m_cNoOfInstalments As Integer = 12
    Public Const m_cFirstInstalment As Integer = 13
    Public Const m_cOthInstalments As Integer = 14
    Public Const m_cCostOfProtect As Integer = 15
    Public Const m_cDeposit As Integer = 16
    Public Const m_cNetAmount As Integer = 17
    Public Const m_cTotalCost As Integer = 18
    Public Const m_cInterestCost As Integer = 19
    Public Const m_cMinFinanceCharge As Integer = 20
    Public Const m_cPayProtection As Integer = 21
    Public Const m_cQDocPath As Integer = 22
    Public Const m_cQDocName As Integer = 23
    Public Const m_cBDocPath As Integer = 24
    Public Const m_cBDocName As Integer = 25
    Public Const m_cCompanyNo As Integer = 26
    Public Const m_cClient As Integer = 27
    Public Const m_cClntAddr1 As Integer = 28
    Public Const m_cClntAddr2 As Integer = 29
    Public Const m_cClntAddr3 As Integer = 30
    Public Const m_cClntPCode As Integer = 31
    Public Const m_cClntAddr4 As Integer = 32
    Public Const m_cClntRegion As Integer = 33
    Public Const m_cClntAreaCode As Integer = 34
    Public Const m_cClntPhone As Integer = 35
    Public Const m_cClntExtn As Integer = 36
    Public Const m_cClntFaxCode As Integer = 37
    Public Const m_cClntFax As Integer = 38
    Public Const m_cClntCountry As Integer = 39
    Public Const m_cBankName As Integer = 40
    Public Const m_cBankSortCode As Integer = 41
    Public Const m_cBankAccountNo As Integer = 42
    Public Const m_cBankAccountName As Integer = 43
    Public Const m_cBankBranch As Integer = 44
    Public Const m_cBankAddr1 As Integer = 45
    Public Const m_cBankAddr2 As Integer = 46
    Public Const m_cBankAddr3 As Integer = 47
    Public Const m_cBankTown As Integer = 48
    Public Const m_cBankPCode As Integer = 49
    Public Const m_cBankAddr4 As Integer = 50
    Public Const m_cBankCountry As Integer = 51
    Public Const m_cBankAreaCode As Integer = 52
    Public Const m_cBankPhoneNo As Integer = 53
    Public Const m_cBankPhoneExt As Integer = 54
    Public Const m_cBankFaxAreaCode As Integer = 55
    Public Const m_cBankFaxNo As Integer = 56
    Public Const m_cBrkName As Integer = 57
    Public Const m_cBrkAddr1 As Integer = 58
    Public Const m_cBrkAddr2 As Integer = 59
    Public Const m_cBrkAddr3 As Integer = 60
    Public Const m_cBrkPCode As Integer = 61
    Public Const m_cBrkAddr4 As Integer = 62
    Public Const m_cBasisOfCalc As Integer = 63
    Public Const m_cArrangementFee As Integer = 64
    Public Const m_cDepositPercent As Integer = 65

    Public Const m_cCDocPath As Integer = 68
    Public Const m_cCdocName As Integer = 69
    Public Const m_cClientId As Integer = 70

    Public Const m_cPaymentMethod As Integer = 72
    Public Const m_cAutoGenPlanRef As Integer = 73
    Public Const m_cFinCollPlanRef As Integer = 74
    Public Const m_cPolicyCnt As Integer = 75
    Public Const m_cPremFinCnt As Integer = 76
    'The constant below should be set to the value of the last constant above
    'as this is used for dimensioning within the class module clsPremFinance.
    Public Const m_cMainArray As Integer = 76

    ' Added to replace global variables 19/09/2003
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
    Private m_bJustCalculateAnnual As Boolean
    Private m_vAnnualFromMonthlyPremium As Object
    ' ************************************************

    Public vRecipient As Object
    Public vSubject As Object
    Public vTextBody As Object
    Public vMailbox As Object
    Public vServer As Object

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Financial"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Data Set Definition
    Private m_oDataSet As cGISDataSetControl.Application

    ' bGISSchemeBusiness component - CL150200
    Private m_oGISSchemeBusiness As bGISSchemeBusiness.Business

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' SQL String used for Adding/Updating Objects in DB
    Private m_sAddSQL As String = ""
    Private m_sUpdateSQL As String = ""
    Private m_sDeleteSQL As String = ""
    Private m_sAddUpdateSQL As String = "" 'sj 18/08/99

    ' File Number used for Creating Referrals
    Private m_iFileNumber As Integer

    'SPW 230505 Datacash return code
    Private Const DC_SUCCESS As Integer = -1

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        'LogMessageToFile _
        'sUsername:="", _
        'iType:=PMLogOnError, _
        'sMsg:="bGIS.initialise starting...", _
        'vApp:=ACApp, _
        'vClass:=ACClass, _
        'vMethod:="bGIS"  ' TEMPDEBUG

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

            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
                m_oDataSet = Nothing
                m_oGISSchemeBusiness = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' Perform Datacash transaction CL190500 (home)
    Public Function DataCash(ByVal v_sGisDataModelCode As String, ByVal v_sDatacashRequestType As String, ByVal v_sDatacashRef As String, ByVal v_sDatacashCardNum As String, ByVal v_iDatacashExpMonth As Integer, ByVal v_iDatacashExpYear As Integer, ByVal v_sDatacashAmt As String, ByVal v_sDatacashAuthCode As String, ByVal v_sDatacashSwitchExtraInfo As String, ByVal v_lPolicyLinkID As Integer, ByVal v_sDatacashTransactionType As String, ByRef r_vDatacashResponseArray() As Object) As Integer
        Return DataCash(v_sGisDataModelCode:=v_sGisDataModelCode, v_sDatacashRequestType:=v_sDatacashRequestType, v_sDatacashRef:=v_sDatacashRef, v_sDatacashCardNum:=v_sDatacashCardNum, v_iDatacashExpMonth:=v_iDatacashExpMonth, v_iDatacashExpYear:=v_iDatacashExpYear, v_sDatacashAmt:=v_sDatacashAmt, v_sDatacashAuthCode:=v_sDatacashAuthCode, v_sDatacashSwitchExtraInfo:=v_sDatacashSwitchExtraInfo, v_lPolicyLinkID:=v_lPolicyLinkID, v_sDatacashTransactionType:=v_sDatacashTransactionType, r_vDatacashResponseArray:=r_vDatacashResponseArray, v_sCurrency:="", v_sDataCashStartDate:="", v_sCV2Code:="", v_sAVSStreet_Add1:="", v_sAVSStreet_Add2:="", v_sAVSStreet_Add3:="", v_sAVSStreet_Add4:="", v_sAVSPostcode:="", v_sCustomerStreet_Add1:="", v_sCustomerStreet_Add2:="", v_sCustomerStreet_Add3:="", v_sCustomerStreet_Add4:="", v_sCustomerPostcode:="", v_sCustomerPhoneNo:="", v_sCustomerForename:="", v_sCustomerSurname:="", v_sCustomerEmail:="", v_sVehicleReg:="")
    End Function

    Public Function DataCash(ByVal v_sGisDataModelCode As String, ByVal v_sDatacashRequestType As String, ByVal v_sDatacashRef As String, ByVal v_sDatacashCardNum As String, ByVal v_iDatacashExpMonth As Integer, ByVal v_iDatacashExpYear As Integer, ByVal v_sDatacashAmt As String, ByVal v_sDatacashAuthCode As String, ByVal v_sDatacashSwitchExtraInfo As String, ByVal v_lPolicyLinkID As Integer, ByVal v_sDatacashTransactionType As String, ByRef r_vDatacashResponseArray() As Object, ByVal v_sCurrency As String, ByVal v_sDataCashStartDate As String, ByVal v_sCV2Code As String, ByVal v_sAVSStreet_Add1 As String, ByVal v_sAVSStreet_Add2 As String, ByVal v_sAVSStreet_Add3 As String, ByVal v_sAVSStreet_Add4 As String, ByVal v_sAVSPostcode As String, ByVal v_sCustomerStreet_Add1 As String, ByVal v_sCustomerStreet_Add2 As String, ByVal v_sCustomerStreet_Add3 As String, ByVal v_sCustomerStreet_Add4 As String, ByVal v_sCustomerPostcode As String, ByVal v_sCustomerPhoneNo As String, ByVal v_sCustomerForename As String, ByVal v_sCustomerSurname As String, ByVal v_sCustomerEmail As String, ByVal v_sVehicleReg As String) As Integer
        Dim result As Integer = 0
        Dim sDCSwitchedOn, sEncryptionKey, sMerchantID, sMerchantPwd As String
        Dim iMode As Integer
        Dim sLogFile, sTimeoutSecs, sCurrency, sAuthCode As String
        Dim lReturn, lDCResult As Integer
        Dim sErrNum, sErrMsg As String
        Dim bNew As Boolean
        Dim sTransactType As String = ""
        ReDim r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSPolicy) ' SPW 040604
        Dim s, sXMLResponse, sConfPath, sCardInfoPath, sCV2AVSPolicy As String 'SPW 030604
        Dim bDC3rdManCheck As Boolean 'SPW030604
        Dim vResult, sDCAccountDescription As String
        Dim iDCAccountNo As Integer
        Dim sDCAccountKey As String = ""
        Dim bMultiAccount As Boolean

        'Changed the following to late bound
        Dim oDC As Object
        'SPW 301104
        Dim oDCResponse, oDCCardInfo, oDCConfig, oDCAgent As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            s = ""
            s = s & " v_sGISDataModelCode = " & v_sGisDataModelCode
            s = s & " v_sDatacashRequestType = " & v_sDatacashRequestType
            s = s & " v_DatacashRef = " & v_sDatacashRef
            s = s & " v_sDatacashCardNum (first 10 digits) = " & v_sDatacashCardNum.Substring(0, 10)
            s = s & " v_iDatacashExpMonth = " & CStr(v_iDatacashExpMonth)
            s = s & " v_iDatacashExpYear = " & CStr(v_iDatacashExpYear)
            s = s & " v_sDatacashAmt = " & v_sDatacashAmt
            s = s & " v_sDatacashAuthCode = " & v_sDatacashAuthCode
            s = s & " v_sDatacashSwitchExtraInfo = " & v_sDatacashSwitchExtraInfo
            s = s & " v_lPolicyLinkID = " & CStr(v_lPolicyLinkID)
            s = s & " v_sDatacashTransactionType = " & v_sDatacashTransactionType
            s = s & " v_sDataCashStartDate = " & v_sDataCashStartDate
            s = s & " v_sCV2Code = " & "#hidden#"
            s = s & " v_sCV2Code = " & v_sCV2Code
            s = s & " v_sAVSStreet_Add1 = " & v_sAVSStreet_Add1
            s = s & " v_sAVSStreet_Add2 = " & v_sAVSStreet_Add2
            s = s & " v_sAVSStreet_Add3 =  " & v_sAVSStreet_Add3
            s = s & " v_sAVSStreet_Add4 = " & v_sAVSStreet_Add4
            s = s & " v_sAVSPostcode = " & v_sAVSPostcode
            s = s & " v_sCustomerStreet_Add1 = " & v_sCustomerStreet_Add1
            s = s & " v_sCustomerStreet_Add2 = " & v_sCustomerStreet_Add2
            s = s & " v_sCustomerStreet_Add3 =  " & v_sCustomerStreet_Add3
            s = s & " v_sCustomerStreet_Add4 = " & v_sCustomerStreet_Add4
            s = s & " v_sCustomerPostcode = " & v_sCustomerPostcode
            s = s & " v_sCustomerPhoneNo = " & v_sCustomerPhoneNo
            s = s & " v_sCustomerForename = " & v_sCustomerForename
            s = s & " v_sCustomerSurname = " & v_sCustomerSurname
            s = s & " v_sCustomerEmail = " & v_sCustomerEmail
            s = s & " v_sVehicleReg = " & v_sVehicleReg

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Entering Datacash with parameters: " & s, vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash")

            lReturn = GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case v_sDatacashTransactionType
                Case "NB" : sTransactType = bGISTemp.GISNBTransTypePayment
                Case "MTA" : sTransactType = bGISTemp.GISMTATransTypePayment
            End Select

            lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=v_lPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=sTransactType, r_oDatabase:=m_oDatabase)

            'SPW 240804
            ' Check if using multiple datacash accounts or using the old single
            ' account transaction

            'Check if reg key exists for at least one datacash subkey if not then use single account

            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCAccountDescription, r_sSettingValue:=sDCAccountDescription, v_sSubKey:="Datacash_Acc1")

            bMultiAccount = sDCAccountDescription.Length > 0

            sDCAccountDescription = "DC"
            iDCAccountNo = 1
            Do While sDCAccountDescription <> ""

                lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCAccountDescription, r_sSettingValue:=sDCAccountDescription, v_sSubKey:="Datacash_Acc" & iDCAccountNo)

                If sDCAccountDescription = v_sDatacashTransactionType Then Exit Do
                iDCAccountNo += 1
            Loop

            'Set up account no variable so we get the right details
            If bMultiAccount Then
                sDCAccountKey = "Datacash_Acc" & iDCAccountNo
            Else
                sDCAccountKey = ""
            End If

            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCSwitchedOn, r_sSettingValue:=sDCSwitchedOn, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDCSwitchedOn = sDCSwitchedOn.Trim().ToUpper()

            ' If blank DC hostname then do not attempt DC transaction - CL230600
            If sDCSwitchedOn = "FALSE" Or sDCSwitchedOn = "0" Then

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash is SWITCHED OFF IN REGISTRY!!!.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash")

                Return result
            End If

            v_sDatacashRequestType = v_sDatacashRequestType.ToLower()

            Select Case v_sDatacashRequestType
                Case GISSharedConstants.GISDatacashTypeAuthorise, GISSharedConstants.GISDatacashTypePreAuthorise

                    Dim dbNumericTemp2 As Double
                    Dim dbNumericTemp As Double
                    If v_sDatacashRef = "" Or v_sDatacashCardNum = "" Or (Not Double.TryParse(CStr(v_iDatacashExpMonth), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Or (Not Double.TryParse(CStr(v_iDatacashExpYear), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Or v_sDatacashAmt = "" Then

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - some parameters for Auth are empty.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Case GISSharedConstants.GISDatacashTypeFulfill

                    If v_sDatacashAuthCode = "" Or v_sDatacashRef = "" Or v_sDatacashAmt = "" Then
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - some parameters for Fulfill are empty.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Case GISSharedConstants.GISDatacashTypeRefund

                    Dim dbNumericTemp4 As Double
                    Dim dbNumericTemp3 As Double
                    If v_sDatacashRef = "" Or v_sDatacashCardNum = "" Or (Not Double.TryParse(CStr(v_iDatacashExpMonth), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Or (Not Double.TryParse(CStr(v_iDatacashExpYear), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Or v_sDatacashAmt = "" Then

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - some parameters for Auth are empty.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                    If CSng(v_sDatacashAmt) < 0 Then

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - refund amount is not positive.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Case Else

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - invalid request type.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            '
            ' Get Merchant ID
            '
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCMerchantID, r_sSettingValue:=sMerchantID, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' Get Merchant pwd
            '
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCMerchantPwd, r_sSettingValue:=sMerchantPwd, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Logfile, Hostname, Port and Timeout are now specified in the
            ' Datacash configuration xml no need to set in regstry now

            '
            ' Get Datacash configuration file
            '
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCConfPath, r_sSettingValue:=sConfPath, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' Get Datacash cardinfo path
            '
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCCardInfoPath, r_sSettingValue:=sCardInfoPath, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not sCardInfoPath.EndsWith("\") Then sCardInfoPath = sCardInfoPath & "\"

            'SPW 030604
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCCV2AVSPolicy, r_sSettingValue:=sCV2AVSPolicy, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SPW 190704
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDC3rdManCheck, r_sSettingValue:=vResult, v_sSubKey:=sDCAccountKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bDC3rdManCheck = vResult.Trim().ToUpper() = "TRUE"

            iMode = 0 ' Do not modify the amount

            If Not False Then
                sCurrency = v_sCurrency
            Else
                lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDCCurrency, r_sSettingValue:=sCurrency)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sCurrency = ""
                End If
            End If

            If sCurrency = "" Then
                sCurrency = "GBP"
            End If

            ' Default these in case of failure

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashResult) = "Error - DataCash not called."

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashAuthCode) = "authcode"

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashUniqueRef) = "uniqueref"

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashTimeStamp) = "timestamp"

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashCardType) = "cardtype"

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashIssuer) = "issuer"

            r_vDatacashResponseArray(GISSharedConstants.GISDatacashCountry) = "country"

            r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSStatus) = "CV2AVSStatus"

            r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSReversalStatus) = "CV2AVSReversalStatus"

            r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSPolicy) = "CV2AVSPolicy"

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' NOW READY TO INTERACT WITH DATACASH
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'SPW 301104 Changed to use new datcash XML .Net API
            'Create Datacash request document object
            'oDC = New result.Document()

            ''Create datacash configuration object
            'oDCConfig = New result.Config()

            ''Create datacash agent object
            'oDCAgent = New result.Agent()

            If oDC Is Nothing Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - unable to instantiate DataCash.XMLTransaction COM object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                'Card info not found so not sending request, clean up
                oDC = Nothing
                oDCConfig = Nothing
                oDCAgent = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' SPW 061204 - New Datacash XML API                           '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Get the path to config files

            oDCConfig.setConfigFile(gPMFunctions.ToSafeString(sConfPath))

            'Set the config file

            lReturn = oDC.setConfig(oDCConfig)

            ' Set the Datacash client and password - MANDATORY for all transactions

            oDC.Set("Request.Authentication.client", gPMFunctions.ToSafeString(sMerchantID))

            oDC.Set("Request.Authentication.password", gPMFunctions.ToSafeString(sMerchantPwd))

            ' Set the amount - MANDATORY for Pre-auth
            ' optional on fulfil but we'll set it for the hell of it

            oDC.Set("Request.Transaction.TxnDetails.amount", gPMFunctions.ToSafeString(v_sDatacashAmt))
            'oDC.Set "Request.Transaction.TxnDetails.currency", sCurrency

            ' Two-Step process, Step One - Pre-auth

            Select Case v_sDatacashRequestType
                Case GISSharedConstants.GISDatacashTypePreAuthorise, GISSharedConstants.GISDatacashTypeAuthorise

                    'Set the Mandatory properties for pre-auth

                    oDC.Set("Request.Transaction.CardTxn.Card.pan", gPMFunctions.ToSafeString(v_sDatacashCardNum))

                    oDC.Set("Request.Transaction.CardTxn.Card.expirydate", StringsHelper.Format(v_iDatacashExpMonth, "00") & "/" & StringsHelper.Format(v_iDatacashExpYear, "00"))

                    oDC.Set("Request.Transaction.CardTxn.Card.startdate", gPMFunctions.ToSafeString(v_sDataCashStartDate))

                    oDC.Set("Request.Transaction.CardTxn.Card.issuenumber", gPMFunctions.ToSafeString(v_sDatacashSwitchExtraInfo))

                    ' Merchant reference
                    'oDC.reference v_sDatacashRef

                    oDC.Set("Request.Transaction.TxnDetails.merchantreference", gPMFunctions.ToSafeString(v_sDatacashRef))

                    ' Transaction type
                    'oDC.txn_method "pre"

                    oDC.Set("Request.Transaction.CardTxn.method", "pre")

                    'SPW 170604 If CV2 or Postcode set then need to provide the CV2AVS policy
                    If sCV2AVSPolicy.Length > 0 Then
                        If v_sAVSPostcode.Length > 0 Or v_sCV2Code.Length > 0 Then

                            oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.policy", CStr(sCV2AVSPolicy))

                        End If
                    End If

                    'SPW 010604
                    ' If CV2 Code provided set that too
                    If v_sCV2Code.Length > 0 And sCV2AVSPolicy.Length > 0 Then

                        oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.cv2", gPMFunctions.ToSafeString(v_sCV2Code))
                    End If

                    'SPW 170604 If postcode is set then we are using AVS so set the address fields
                    If v_sAVSPostcode.Length > 0 And sCV2AVSPolicy.Length > 0 Then

                        oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.postcode", gPMFunctions.ToSafeString(v_sAVSPostcode))

                        oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.street_address1", gPMFunctions.ToSafeString(v_sAVSStreet_Add1))

                        oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.street_address2", gPMFunctions.ToSafeString(v_sAVSStreet_Add2))

                        oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.street_address3", gPMFunctions.ToSafeString(v_sAVSStreet_Add3))

                        oDC.Set("Request.Transaction.CardTxn.Card.Cv2Avs.street_address4", gPMFunctions.ToSafeString(v_sAVSStreet_Add4))

                    End If

                    'If 3rd man checking enabled populate additional fields
                    If bDC3rdManCheck Then

                        oDC.Set("Request.Transaction.TxnDetails.Order.BillingAddress.streetaddress", gPMFunctions.ToSafeString(v_sAVSStreet_Add1))

                        oDC.Set("Request.Transaction.TxnDetails.Order.BillingAddress.moreaddress", gPMFunctions.ToSafeString(v_sAVSStreet_Add2))

                        oDC.Set("Request.Transaction.TxnDetails.Order.BillingAddress.city", gPMFunctions.ToSafeString(v_sAVSStreet_Add3))

                        oDC.Set("Request.Transaction.TxnDetails.Order.BillingAddress.postcode", gPMFunctions.ToSafeString(v_sAVSStreet_Add4))

                        oDC.Set("Request.Transaction.TxnDetails.Order.BillingAddress.country", "826")

                        oDC.Set("Request.Transaction.TxnDetails.Order.Recipient.Address.streetaddress", gPMFunctions.ToSafeString(v_sCustomerStreet_Add1))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Recipient.Address.moreaddress", gPMFunctions.ToSafeString(v_sCustomerStreet_Add2))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Recipient.Address.city", gPMFunctions.ToSafeString(v_sCustomerStreet_Add3))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Recipient.Address.postcode", gPMFunctions.ToSafeString(v_sCustomerPostcode))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Recipient.Address.country", "826")

                        oDC.Set("Request.Transaction.TxnDetails.Order.Customer.telephone", gPMFunctions.ToSafeString(v_sCustomerPhoneNo))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Customer.forename", gPMFunctions.ToSafeString(v_sCustomerForename))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Customer.surname", gPMFunctions.ToSafeString(v_sCustomerSurname))

                        oDC.Set("Request.Transaction.TxnDetails.Order.Customer.email", gPMFunctions.ToSafeString(v_sCustomerEmail))

                        'Waiting on its4me for this one. Need to know what element 3rd man want setting
                        'oDC.Set "Transaction.TxnDetails.Order.Customer.vehiclereg", v_sVehicleReg

                    End If

                    ' Two-Step process, Step Two - Fulfill
                Case GISSharedConstants.GISDatacashTypeFulfill

                    'Set the Mandatory properties for fulfill
                    ' authcode received from the Pre-auth response

                    oDC.Set("Request.Transaction.HistoricTxn.authcode", gPMFunctions.ToSafeString(v_sDatacashAuthCode))
                    ' the datacash unique ref from the pre-auth NOT THE MERCHANT REF

                    oDC.Set("Request.Transaction.HistoricTxn.reference", gPMFunctions.ToSafeString(v_sDatacashRef))

                    ' Transaction Type

                    oDC.Set("Request.Transaction.HistoricTxn.method", "fulfill")
                    'oDC.txn_method "fulfill"

                Case GISSharedConstants.GISDatacashTypeRefund
                    ' Mandatory properties for refund

                    oDC.Set("Request.Transaction.CardTxn.Card.pan", gPMFunctions.ToSafeString(v_sDatacashCardNum))

                    oDC.Set("Request.Transaction.CardTxn.Card.expirydate", StringsHelper.Format(v_iDatacashExpMonth, "00") & "/" & StringsHelper.Format(v_iDatacashExpYear, "00"))

                    oDC.Set("Request.Transaction.CardTxn.Card.startdate", gPMFunctions.ToSafeString(v_sDataCashStartDate))

                    oDC.Set("Request.Transaction.CardTxn.Card.issuenumber", gPMFunctions.ToSafeString(v_sDatacashSwitchExtraInfo))
                    ' Merchant reference

                    oDC.Set("Request.Transaction.TxnDetails.merchantreference", gPMFunctions.ToSafeString(v_sDatacashRef))

                    oDC.reference(gPMFunctions.ToSafeString(v_sDatacashRef))
                    ' Transaction type

                    oDC.Set("Request.Transaction.CardTxn.method", "refund")
                    'oDC.txn_method "refund"

            End Select

            If v_sDatacashRequestType = GISSharedConstants.GISDatacashTypePreAuthorise Or v_sDatacashRequestType = GISSharedConstants.GISDatacashTypeAuthorise Then

                'oDCCardInfo = New result.CardInfo()

                ' Set datacash cardinfo path
                'sXMLResponse = oDC.setCardInfo(sCardInfoPath)

                lReturn = oDCCardInfo.setPathToBinFiles(ToString(sCardInfoPath))

                lReturn = oDCCardInfo.setCardNumberFromDocument(oDC)

                lReturn = oDCCardInfo.locateCardDetails()

                'If cardinfo details located ok then continue
                If lReturn <> DC_SUCCESS Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashAuthCode) = "Failed to get card info"

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Failed - Error getting cardinfo", vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Send the request to datacash and get the XML response
            'sXMLResponse = oDC.send(sConfPath)

            lReturn = oDCAgent.setConfig(oDCConfig)

            sXMLResponse = oDCAgent.send(oDC)

            'oDCResponse = New result.Document()

            sXMLResponse = oDCResponse.getResponseDocument(oDCAgent)

            If oDCResponse.get("Response.status") = 1 Then

                ' Transaction OK

                'Populate the array with responses from datacash

                If Not (oDCResponse.get("Response.status") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashResult) = oDCResponse.get("Response.status")
                End If

                If Not (oDCResponse.get("CardTxn.authcode") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashAuthCode) = oDCResponse.get("Response.CardTxn.authcode")
                End If

                If Not (oDCResponse.get("datacash_reference") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashUniqueRef) = oDCResponse.get("Response.datacash_reference")
                End If

                If Not (oDCResponse.get("Response.time") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashTimeStamp) = oDCResponse.get("response.time")
                End If
                If v_sDatacashRequestType <> GISSharedConstants.GISDatacashTypeFulfill Then

                    If Not (oDCCardInfo.getScheme() Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDatacashCardType) = oDCCardInfo.getScheme()
                    End If

                    If Not (oDCCardInfo.getIssuer() Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDatacashIssuer) = oDCCardInfo.getIssuer()
                    End If

                    If Not (oDCCardInfo.getCountry() Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDatacashCountry) = oDCCardInfo.getCountry()
                    End If
                End If
            Else
                'Transaction declined, populate the array with reason for decline

                If Not (oDCResponse.get("Response.status") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashResult) = oDCResponse.get("Response.status")
                End If

                If Not (oDCResponse.get("Response.reason") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashAuthCode) = oDCResponse.get("Response.reason")
                End If

                If Not (oDCResponse.get("Response.datacash_reference") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashUniqueRef) = oDCResponse.get("Response.datacash_reference")
                End If

                If Not (oDCResponse.get("Response.time") Is Nothing) Then

                    r_vDatacashResponseArray(GISSharedConstants.GISDatacashTimeStamp) = oDCResponse.get("Response.time")
                End If
                If v_sDatacashRequestType <> GISSharedConstants.GISDatacashTypeFulfill Then

                    If Not (oDCCardInfo.getScheme() Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDatacashCardType) = oDCCardInfo.getScheme()
                    End If

                    If Not (oDCCardInfo.getIssuer() Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDatacashIssuer) = oDCCardInfo.getIssuer()
                    End If

                    If Not (oDCCardInfo.getCountry() Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDatacashCountry) = oDCCardInfo.getCountry()
                    End If
                End If

                'If transaction failed duer  to CV2AVS check, populate additional information

                If oDCResponse.get("Response.status") = 7 Then

                    If Not (oDCResponse.get("Response.CardTxn.Cv2Avs.cv2avs_status") Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSStatus) = oDCResponse.get("Response.CardTxn.Cv2Avs.cv2avs_status")
                    End If

                    If Not (oDCResponse.getAttribute("reversal") Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSReversalStatus) = oDCResponse.getAttribute("reversal")
                    End If

                    If Not (oDCResponse.get("Response.CardTxn.Cv2Avs.policy") Is Nothing) Then

                        r_vDatacashResponseArray(GISSharedConstants.GISDataCashCV2AVSPolicy) = oDCResponse.get("Response.CardTxn.Cv2Avs.policy")
                    End If
                End If
            End If

            s = ""

            s = s & " DC_Result = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashResult))

            s = s & " DC_AuthCode = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashAuthCode))

            s = s & " DC_UniqueRef = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashUniqueRef))

            s = s & " DC_TimeStamp = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashTimeStamp))

            s = s & " DC_CardType = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashCardType))

            s = s & " DC_Issuer = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashIssuer))

            s = s & " DC_Country = " & CStr(r_vDatacashResponseArray(GISSharedConstants.GISDatacashCountry))

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Datacash Server returns: " & s, vApp:=ACApp, vClass:=ACClass, vMethod:="Datacash")

            oDC = Nothing
            oDCCardInfo = Nothing
            oDCAgent = Nothing
            oDCResponse = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataCash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataCash", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BankAccountValidation
    '
    ' Description: Perform Bank Account Validation transaction - initiated
    '              from web pages...calls bGISPromptInterface component to
    '              send an XML message with the details in. Prompt return to
    '              us a Status Code indicating whether the customer will be
    '              allowed credit or not.
    '
    ' CJB 111000 Created
    ' ***************************************************************** '

    Public Function BankAccountValidation(ByVal v_sGisDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sQuoteRef As String, ByVal v_lPolicyLinkID As Integer, ByVal v_sBankAccountValidationSenderID As String, ByVal v_sBankAccountValidationCoverType As String, ByVal v_sBankAccountValidationGnetClientCode As String, ByVal v_sBankAccountValidationBusinessStatus As String, ByVal v_sBankAccountValidationBankAccountName As String, ByVal v_sBankAccountValidationBankAccountNo As String, ByVal v_sBankAccountValidationBankSortCode As String, ByRef r_sBankAccountValidationStatusCode As String) As Integer

        Dim result As Integer = 0
        Dim s As String = ""
        Dim bNew As Boolean
        Dim lReturn As Integer
        Dim oGISPromptInterface As bGISPromptInterface.Application
        Dim sURL As String = "" 'CJB 081100
        Dim boPromptEnableStatus As Boolean 'CJB 081100

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Log a msg that we're in here !
            s = ""
            s = s & " v_sGISDataModelCode = " & v_sGisDataModelCode
            s = s & " v_sBusinessTypeCode = " & v_sBusinessTypeCode
            s = s & " v_lPolicyLinkID = " & CStr(v_lPolicyLinkID)
            s = s & " v_sBankAccountValidationSenderID = " & v_sBankAccountValidationSenderID
            s = s & " v_sBankAccountValidationCoverType = " & v_sBankAccountValidationCoverType
            s = s & " v_sBankAccountValidationGnetClientCode = " & v_sBankAccountValidationGnetClientCode
            s = s & " v_sBankAccountValidationBusinessStatus = " & v_sBankAccountValidationBusinessStatus
            s = s & " v_sBankAccountValidationBankAccountName = " & v_sBankAccountValidationBankAccountName
            s = s & " v_sBankAccountValidationBankAccountNo = " & v_sBankAccountValidationBankAccountNo
            s = s & " v_sBankAccountValidationBankSortCode = " & v_sBankAccountValidationBankSortCode

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Entering Bank Account Validation (bGIS) with parameters: " & s, vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation")

            ' Update the Trans Status to say we are about to perform Bank Account Validation
            lReturn = bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=v_lPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISNBTransTypeBankAccountValidation, r_oDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Policy Link Bank Account Validation Status", vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Check if the Prompt URL held in the registry is blank in which case you'd get an error if you
            'proceeded and also as at 7/11/00 RC told me to treat as meaning Prompt was disabled and
            'to continue without doing any Prompt processing...

            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISSharedConstants.GISRegPromptInterfaceURL, r_sSettingValue:=sURL, v_sSubKey:=GISSharedConstants.GISRegSubKey)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If sURL.Trim() = "" Then

                boPromptEnableStatus = False
                r_sBankAccountValidationStatusCode = "00"

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="In BankAccountValidation, after GetPMRegSetting -- sURL is" & sURL, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Else
                boPromptEnableStatus = True
            End If

            If boPromptEnableStatus Then
                'Log a debug message that oGISPromptInterface.BankAccountValidation is about to be called
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="About to call:oGISPromptInterface.BankAccountValidation with v_sSenderID=" & v_sBankAccountValidationSenderID &
                                                       ", v_sCoverType=" & v_sBankAccountValidationCoverType & ", v_sGnetClientCode=" & v_sBankAccountValidationGnetClientCode & ", v_sBusinessStatus=" & v_sBankAccountValidationBusinessStatus &
                                                       ", v_sBankAccountName=" & v_sBankAccountValidationBankAccountName & ", v_sBankAccountNo=" & v_sBankAccountValidationBankAccountNo & ", v_sBankSortCode=" & v_sBankAccountValidationBankSortCode, vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                'Create reference to the GIS Prompt Interface component
                oGISPromptInterface = New bGISPromptInterface.Application()

                If oGISPromptInterface Is Nothing Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Bank Account Validation Failed - unable to instantiate bGISPromptInterface COM object.", vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Call the GIS Prompt Interface BankAccountValidation Method
                lReturn = oGISPromptInterface.BankAccountValidation(v_sSenderID:=v_sBankAccountValidationSenderID, v_sCoverType:=v_sBankAccountValidationCoverType, v_sGnetClientCode:=v_sBankAccountValidationGnetClientCode, v_sBusinessStatus:=v_sBankAccountValidationBusinessStatus, v_sBankAccountName:=v_sBankAccountValidationBankAccountName, v_sBankAccountNo:=v_sBankAccountValidationBankAccountNo, v_sBankSortCode:=v_sBankAccountValidationBankSortCode, v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_sQuoteRef:=v_sQuoteRef, r_sStatusCode:=r_sBankAccountValidationStatusCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn

                    'Check the type of error from Prompt - if a bank a/c error, sort code error or a handled error
                    'returned in an ERROR element of the XML response then show the status etc,
                    'else, just show error code...
                    'NOTE that the error codes, 10500 and 10501 are handled in the web pages and result in a
                    'specific bank a/c validation error being returned or a General Prompt error shown.
                    If lReturn = ToSafeDouble(GISPromptConstants.PromptBankAccountError) Or lReturn = ToSafeDouble(GISPromptConstants.PromptSortCodeError) Or lReturn = ToSafeDouble(GISPromptConstants.PromptOtherError) Then
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPromptInterface.BankAccountValidation Returned with StatusCode=" & r_sBankAccountValidationStatusCode, vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Else
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPromptInterface.BankAccountValidation Failed with lReturn=" & lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If

                    Return result

                End If

                'Log a debug message that oGISPromptInterface.BankAccountValidation has returned
                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPromptInterface.BankAccountValidation Returned with StatusCode=" & r_sBankAccountValidationStatusCode, vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                oGISPromptInterface = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BankAccountValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BankAccountValidation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
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
    '        Note that this method may originate (via Seller Tool) at any point in
    '        the web pages as soon as the amount to finance is known. Be aware that
    '        if this is at the point of quoting then you may use the AfterQuote method
    '        in the BOM (see I4M BOM for an example) to automatically call the Premium
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

    Public Function CalcPaymentMethodCharge(ByVal v_sCalcPaymentMethodChargeProductFamily As String, ByVal v_sBusinessTypeCode As String, ByVal v_sGisDataModelCode As String, ByVal v_sCalcPaymentMethodChargeTransactionType As String, ByVal v_sCalcPaymentMethodChargePaymentMethod As String, ByVal v_sCalcPaymentMethodChargeStartDate As String, ByVal v_sCalcPaymentMethodChargeAmountToFinance As String, ByVal v_sCalcPaymentMethodChargeNoOfInstalments As String, ByVal v_sCalcPaymentMethodChargeActionType As String, ByVal v_sCalcPaymentMethodChargeRequestedDepositPercent As String, ByRef r_sCalcPaymentMethodChargeInterestRate As String, ByRef r_sCalcPaymentMethodChargeAPR As String, ByRef r_sCalcPaymentMethodChargeInterestCost As String, ByRef r_sCalcPaymentMethodChargeNoOfInstalments As String, ByRef r_sCalcPaymentMethodChargeFirstInstalment As String, ByRef r_sCalcPaymentMethodChargeOthInstalments As String, ByRef r_sCalcPaymentMethodChargeDeposit As String, ByRef r_sCalcPaymentMethodChargeArrangementFee As String, ByRef r_sCalcPaymentMethodChargeDepositPercent As String, ByRef r_sCalcPaymentMethodChargeCompanyName As String, ByRef r_sCalcPaymentMethodChargeCompanyNo As String, ByRef r_sCalcPaymentMethodChargeSchemeName As String, ByRef r_sCalcPaymentMethodChargeSchemeNo As String, ByRef r_sCalcPaymentMethodChargeSchemeVer As String, ByRef r_sCalcPaymentMethodChargeBasisOfCalc As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim oClsPF As clsPremiumFinance = New clsPremiumFinance
            'Call the GIS GISPremiumFinance CalculateFinance Method - note that all the o/p parms come back in an array
            lReturn = oClsPF.CalculateFinance(v_sCalcPaymentMethodChargeProductFamily, v_sBusinessTypeCode, v_sGisDataModelCode, v_sCalcPaymentMethodChargeTransactionType, v_sCalcPaymentMethodChargePaymentMethod, v_sCalcPaymentMethodChargeStartDate, v_sCalcPaymentMethodChargeAmountToFinance, v_sCalcPaymentMethodChargeNoOfInstalments, v_sCalcPaymentMethodChargeActionType, v_sCalcPaymentMethodChargeRequestedDepositPercent, vResultArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Inside NBQuoteAfter: Failed after calling oGISPremiumFinance.CalculateFinance with the following parameters: sProductFamily=" & v_sCalcPaymentMethodChargeProductFamily & ", sBusinessTypeCode=" & v_sBusinessTypeCode & ", sDataModelCode=" & v_sGisDataModelCode &
                                                  ", sTransactionType=" & v_sCalcPaymentMethodChargeTransactionType & ", sPaymentMethod=" & v_sCalcPaymentMethodChargePaymentMethod & ", Date=" & v_sCalcPaymentMethodChargeStartDate &
                                                  ", vPremium=" & v_sCalcPaymentMethodChargeAmountToFinance & ", sNoOfInstalments=" & v_sCalcPaymentMethodChargeNoOfInstalments & ", sActionType=" & v_sCalcPaymentMethodChargeActionType & ", sRequestedDepositPercent=" & v_sCalcPaymentMethodChargeRequestedDepositPercent & ", RETURN CODE=" & CStr(lReturn) &
                                                  ", RESULT STRING=" & CStr(vResultArray), vApp:=ACApp, vClass:=ACClass, vMethod:="CalcPaymentMethodCharge")

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Extract each data item out of the o/p array and store in byref vars to send back in xml response msg

            r_sCalcPaymentMethodChargeInterestRate = CStr(vResultArray(2))

            r_sCalcPaymentMethodChargeAPR = CStr(vResultArray(3))

            r_sCalcPaymentMethodChargeInterestCost = CStr(vResultArray(4))

            r_sCalcPaymentMethodChargeNoOfInstalments = CStr(vResultArray(5))

            r_sCalcPaymentMethodChargeFirstInstalment = CStr(vResultArray(6))

            r_sCalcPaymentMethodChargeOthInstalments = CStr(vResultArray(7))

            r_sCalcPaymentMethodChargeDeposit = CStr(vResultArray(8))

            r_sCalcPaymentMethodChargeArrangementFee = CStr(vResultArray(9))

            r_sCalcPaymentMethodChargeDepositPercent = CStr(vResultArray(11))

            r_sCalcPaymentMethodChargeCompanyName = CStr(vResultArray(12))

            r_sCalcPaymentMethodChargeCompanyNo = CStr(vResultArray(13))

            r_sCalcPaymentMethodChargeSchemeName = CStr(vResultArray(14))

            r_sCalcPaymentMethodChargeSchemeNo = CStr(vResultArray(15))

            r_sCalcPaymentMethodChargeSchemeVer = CStr(vResultArray(16))

            r_sCalcPaymentMethodChargeBasisOfCalc = CStr(vResultArray(17))

            'Log a debug message that oGISPremiumFinance.CalcPaymentMethodCharge has returned
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oGISPremiumFinance.CalcPaymentMethodCharge Returned with InterestRate=" & r_sCalcPaymentMethodChargeInterestRate &
                                                   ", APR=" & r_sCalcPaymentMethodChargeAPR & ", Interest Cost=" & r_sCalcPaymentMethodChargeInterestCost &
                                                   ", No Of Instalments=" & r_sCalcPaymentMethodChargeNoOfInstalments & ", First Instalment=" & r_sCalcPaymentMethodChargeFirstInstalment &
                                                   ", Other Instalments=" & r_sCalcPaymentMethodChargeOthInstalments & ", Deposit=" & r_sCalcPaymentMethodChargeDeposit &
                                                   ", Arrangement Fee=" & r_sCalcPaymentMethodChargeArrangementFee & ", Deposit %=" & r_sCalcPaymentMethodChargeDepositPercent &
                                                   ", Company Name=" & r_sCalcPaymentMethodChargeCompanyName & ", Company No=" & r_sCalcPaymentMethodChargeCompanyNo &
                                                   ", Scheme Name=" & r_sCalcPaymentMethodChargeSchemeName & ", Scheme No=" & r_sCalcPaymentMethodChargeSchemeNo &
                                                   ", Scheme Ver=" & r_sCalcPaymentMethodChargeSchemeVer & ", Basis Of Calc=" & r_sCalcPaymentMethodChargeBasisOfCalc, vApp:=ACApp, vClass:=ACClass, vMethod:="CalcPaymentMethodCharge", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcPaymentMethodCharge Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcPaymentMethodCharge", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
    ' Edit History: CJB 060802 Added r_vInterestAmount return parameter.
    '
    ' ***************************************************************** '
    Public Function PremiumFinanceQuote(ByVal v_vDataModelCode As Object, ByVal v_vBusinessTypeCode As Object, ByVal v_vBusinessStatus As Object, ByVal v_vPremiumAmount As Object, ByVal v_vPremiumFinanceRef As Object, ByVal v_vEffectiveDate As Object, ByVal v_vPolicyNo As Object, ByRef r_vStatusCode As String, ByRef r_vStatusExplanation As String, ByRef r_vTotalPayable As Object, ByRef r_vNumberOfInstalmentsLeft As Object, ByRef r_vFirstInstalAmt As Object, ByRef r_vSubsequentInstalAmt As Object, ByRef r_vActualPaymentDate As Object, ByRef r_vInterestAmount As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim oGISPromptInterface As bGISPromptInterface.Application
            Dim sURL As String = ""
            Dim bNew As Boolean

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Entering Premium Finance Quote (bGIS) with parameters: " &
                                                   "v_vGISDataModelCode = " & CStr(v_vDataModelCode) &
                                                   " v_vBusinessTypeCode = " & CStr(v_vBusinessTypeCode) &
                                                   " v_vPremiumAmount = " & CStr(v_vPremiumAmount) &
                                                   " v_vPremiumFinanceRef = " & CStr(v_vPremiumFinanceRef) &
                                                   " v_vEffectiveDate = " & CStr(v_vEffectiveDate) &
                                                   " v_vPolicyNo = " & CStr(v_vPolicyNo), vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote")

            'Check if the Prompt URL held in the registry is blank in which case you'd get an error
            'if you proceeded and also it would mean Prompt was disabled and to continue without
            'doing any Prompt processing...
            lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISSharedConstants.GISRegPromptInterfaceURL, r_sSettingValue:=sURL, v_sSubKey:=GISSharedConstants.GISRegSubKey), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMRegSetting failed attempting to get " & GISSharedConstants.GISRegPromptInterfaceURL & " with lReturn:" & CStr(lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return lReturn
            End If

            If sURL.Trim() = "" Then
                r_vStatusCode = GISPromptConstants.PromptDisabled
                r_vStatusExplanation = GISPromptConstants.PromptDisabledMessage

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Bypassing Premium Finance Quote since PromptInterfaceURL in the registry is not set - therefore, Prompt disabled...", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Else

                'Create reference to the GIS Prompt Interface component
                oGISPromptInterface = New bGISPromptInterface.Application()

                If oGISPromptInterface Is Nothing Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Premium Finance Quote Failed - unable to instantiate bGISPromptInterface COM object.", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Call the GIS Prompt Interface PremiumFinanceQuote Method

                lReturn = oGISPromptInterface.PremiumFinanceQuote(v_vDataModelCode:=v_vDataModelCode, v_vBusinessTypeCode:=v_vBusinessTypeCode, v_vBusinessStatus:=v_vBusinessStatus, v_vPremiumAmount:=v_vPremiumAmount, v_vPremiumFinanceRef:=v_vPremiumFinanceRef, v_vEffectiveDate:=v_vEffectiveDate, v_vPolicyNo:=v_vPolicyNo, r_vStatusCode:=r_vStatusCode, r_vStatusExplanation:=r_vStatusExplanation, r_vTotalPayable:=r_vTotalPayable, r_vNumberOfInstalmentsLeft:=r_vNumberOfInstalmentsLeft, r_vFirstInstalAmt:=r_vFirstInstalAmt, r_vSubsequentInstalAmt:=r_vSubsequentInstalAmt, r_vActualPaymentDate:=r_vActualPaymentDate, r_vInterestAmount:=r_vInterestAmount)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGISPromptInterface.PremiumFinanceQuote Failed with lReturn=" & lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'Log a debug message that oGISPromptInterface.PremiumFinanceQuote has returned

                GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGISPromptInterface.PremiumFinanceQuote Returned with parameters:" &
                                                       "r_vStatusCode=" & r_vStatusCode &
                                                       " r_vStatusExplanation=" & r_vStatusExplanation &
                                                       " r_vTotalPayable=" & CStr(r_vTotalPayable) &
                                                       " r_vNumberOfInstalmentsLeft=" & CStr(r_vNumberOfInstalmentsLeft) &
                                                       " r_vFirstInstalAmt=" & CStr(r_vFirstInstalAmt) &
                                                       " r_vSubsequentInstalAmt=" & CStr(r_vSubsequentInstalAmt) &
                                                       " r_vActualPaymentDate=" & CStr(r_vActualPaymentDate) &
                                                       " r_vInterestAmount=" & CStr(r_vInterestAmount), vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                oGISPromptInterface = Nothing
            End If

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PremiumFinanceQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PremiumFinanceQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadFromXML
    '
    ' Description: Loads up a Data Set using the supplied XML Streams.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (LoadFromXML) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function LoadFromXML(ByVal v_sDataModelCode As String, ByVal v_sXMLDataSet As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDataSet = New cGISDataSetControl.Application()
    '
    ' Load the Data Set with an Empty Data Set for this Model
    'lReturn = CType(GetDataModelDef(v_sDataModelCode), gPMConstants.PMEReturnCode)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    '
    ' Update with the Data Set
    '
    'Return m_oDataSet.UpdateDataSetFromXML(v_sXMLDataSet:=gpmfunctions.ToSafeString(v_sXMLDataSet))
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '



    ' ***************************************************************** '
    ' Name: GetDataModelDef
    '
    ' Description: Load the Stored Data Set Definition from file.
    '
    '
    ' ***************************************************************** '
    Public Function GetDataModelDef(ByVal v_sGisDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDefFile, sDataSetFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Path/FileNames for stored EMPTY Data Set Files
            ' of this Data Model Type
            lReturn = CType(GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a New Data Set
            m_oDataSet = New cGISDataSetControl.Application()

            ' Try to load from the Empty XML files
            lReturn = m_oDataSet.LoadFromXMLFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile)

            ' If we could NOT Load from stored Empty Files
            ' then error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModelDefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModelDef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsurerABICode
    '
    ' Description: Get the insurer ABI code from the gis_insurer table in
    '              the database with the given polaris_insurer_no value.
    '
    ' CJB 15/12/2000
    '
    ' ***************************************************************** '
    Public Function GetInsurerABICode(ByVal v_iPolarisInsurerNo As Integer, ByRef r_sInsurer_ABI_Code As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT abi_81_insurer "
            sSQL = sSQL & "FROM GIS_Insurer "
            sSQL = sSQL & "WHERE polaris_insurer_no = {polaris_insurer_no}"

            m_oDatabase.Parameters.Clear()

            ' lPolicyBinderID
            lReturn = m_oDatabase.Parameters.Add(sName:="polaris_insurer_no", vValue:=CStr(v_iPolarisInsurerNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter polaris_insurer_no", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerABICode")
                Return result
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectInsurerABICode", bStoredProcedure:=False, vResultArray:=vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQLAction", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerABICode")
                Return result
            End If

            If Informations.IsArray(vResultArray) Then

                r_sInsurer_ABI_Code = CStr(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run-time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerABICode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
End Class
