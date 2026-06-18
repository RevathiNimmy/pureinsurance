Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Account
    Implements IDisposable
    ' *****************************************************************************
    ' Class      : Account
    ' Description: Creates an account
    '              This code was taken from bACTSiriusAccount (Orion)
    ' SJP04072002 Account Key now party count to allow for more than 8 branches
    ' *****************************************************************************


    ' ************************************************
    ' Added to replace global variables 27/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Name of this class
    Private Const ACClass As String = "Account"

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Objects used
    Private m_oAccount As bACTAccount.Form
    Private m_oExplorer As bACTExplorer.Form
    Private m_oLedger As bACTLedger.Form

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Standard Initialise function.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String) As Integer





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' New instance of service component services

            ' Account object

            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Explorer object

            m_oExplorer = New bACTExplorer.Form
            m_lReturn = m_oExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTExplorer.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Ledger object

            m_oLedger = New bACTLedger.Form
            m_lReturn = m_oLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTLedger.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Standard terminate function.
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
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()

                End If
                m_oAccount = Nothing
                If m_oExplorer IsNot Nothing Then
                    m_oExplorer.Dispose()

                End If
                m_oExplorer = Nothing
                If m_oLedger IsNot Nothing Then
                    m_oLedger.Dispose()
                End If
                m_oLedger = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a PMAccountComm.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByVal vLedgerFlag As String = "", Optional ByVal vSourceID As Object = Nothing, Optional ByVal vShortName As String = "", Optional ByVal vPartyType As Object = Nothing, Optional ByVal vPartyID As Object = Nothing, Optional ByVal vName As String = "", Optional ByVal vCurrencyID As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vLedgerFlag)) Or (String.IsNullOrEmpty(vLedgerFlag)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vSourceID)) Or (Object.Equals(vSourceID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vPartyType)) Or (Object.Equals(vPartyType, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vPartyID)) Or (Object.Equals(vPartyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vName)) Or (String.IsNullOrEmpty(vName)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vCurrencyID)) Or (vCurrencyID.Equals(0)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the PMAccountComm for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByVal vLedgerFlag As String = "", Optional ByVal vSourceID As Object = Nothing, Optional ByVal vShortName As String = "", Optional ByVal vPartyType As Object = Nothing, Optional ByVal vPartyID As Object = Nothing, Optional ByVal vName As String = "", Optional ByVal vCurrencyID As Integer = 0, Optional ByVal vAddress1 As String = "", Optional ByVal vAddress2 As String = "", Optional ByVal vAddress3 As String = "", Optional ByVal vAddress4 As String = "", Optional ByVal vPostalCode As Object = Nothing, Optional ByVal vContactName As String = "", Optional ByVal vContactTelIntCode As Object = Nothing, Optional ByVal vContactTelAreaCode As String = "", Optional ByVal vContactTelNumber As String = "", Optional ByVal vContactFaxIntCode As Object = Nothing, Optional ByVal vContactFaxAreaCode As String = "", Optional ByVal vContactFaxNumber As String = "", Optional ByVal vSalesAccountID As Integer = 0, Optional ByVal vSalesAccountCode As String = "", Optional ByVal vPurchaseAccountID As Integer = 0, Optional ByVal vPurchaseAccountCode As String = "") As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Information.IsNothing(vSourceID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vCurrencyID) Then
            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vPartyID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vPartyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vAddress1) Then
            'pinstripe col length for address lines is 30 so truncate
            If vAddress1.Length > 30 Then
                vAddress1 = vAddress1.Substring(0, 30)
            End If
        End If


        If Not Information.IsNothing(vAddress2) Then
            'pinstripe col length for address lines is 30 so truncate
            If vAddress2.Length > 30 Then
                vAddress2 = vAddress2.Substring(0, 30)
            End If
        End If


        If Not Information.IsNothing(vAddress3) Then
            'pinstripe col length for address lines is 30 so truncate
            If vAddress3.Length > 30 Then
                vAddress3 = vAddress3.Substring(0, 30)
            End If
        End If


        If Not Information.IsNothing(vAddress4) Then
            'pinstripe col length for address lines is 30 so truncate
            If vAddress4.Length > 30 Then
                vAddress4 = vAddress4.Substring(0, 30)
            End If
        End If


        If Not Information.IsNothing(vContactTelNumber) Then
            'pinstripe col length for tel number lines is 20 so truncate
            If vContactTelNumber.Length > 20 Then
                vContactTelNumber = vContactTelNumber.Substring(0, 20)
            End If
        End If


        If Not Information.IsNothing(vContactFaxNumber) Then
            'pinstripe col length for fax number lines is 20 so truncate
            If vContactFaxNumber.Length > 20 Then
                vContactFaxNumber = vContactFaxNumber.Substring(0, 20)
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    Public Function CreateAccount(ByVal v_oDatabase As dPMDAO.Database, Optional ByVal vLedgerFlag As String = "", Optional ByVal vSourceID As Object = Nothing, Optional ByVal vShortName As String = "", Optional ByVal vPartyType As Object = Nothing, Optional ByVal vPartyID As Object = Nothing, Optional ByVal vName As String = "", Optional ByVal vCurrencyID As Integer = 0, Optional ByVal vAddress1 As String = "", Optional ByVal vAddress2 As String = "", Optional ByVal vAddress3 As String = "", Optional ByVal vAddress4 As String = "", Optional ByVal vPostalCode As Object = Nothing, Optional ByVal vContactName As String = "", Optional ByVal vContactTelIntCode As Object = Nothing, Optional ByVal vContactTelAreaCode As String = "", Optional ByVal vContactTelNumber As String = "", Optional ByVal vContactFaxIntCode As Object = Nothing, Optional ByVal vContactFaxAreaCode As String = "", Optional ByVal vContactFaxNumber As String = "", Optional ByRef vSalesAccountID As Integer = 0, Optional ByRef vSalesAccountCode As String = "", Optional ByRef vPurchaseAccountID As Integer = 0, Optional ByRef vPurchaseAccountCode As String = "", Optional ByVal vPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lAccountID, lPurgefrequencyID As Integer
        Dim iCurrencyID, iAccounttypeID As Integer
        Dim lLedgerID As Integer
        Dim sAccountName, sShortCode As String
        Dim bRestrictEnquiry, bRestrictUpdate, bDeleteAtPurge As Boolean
        Dim sContactName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode As String
        Dim iAddressCountry As Integer
        Dim sPhoneAreaCode, sPhoneNumber, sPhoneExtension, sFaxAreaCode, sFaxNumber, sFaxExtension As String
        Dim vAccountStatusID As Integer

        ' For Full Key
        Dim lElementID, lNodeId As Integer

        ' TF110401
        Dim lAccountKey As Integer
        'DD 08/08/2002
        Dim lSubBranchID As Integer

        Dim vValue As String = "" 'MKW24062004 PN12470

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'check all of the required parameters have been passed
            m_lReturn = CType(CheckMandatory(vLedgerFlag, vSourceID, vShortName, vPartyType, vPartyID, vName, vCurrencyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'some parameters are missing
                Return gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If

            'validate the passed parameters
            m_lReturn = CType(Validate(vLedgerFlag, vSourceID, vShortName, vPartyType, vPartyID, vName, vCurrencyID, vAddress1, vAddress2, vAddress3, vAddress4, vPostalCode, vContactName, vContactTelIntCode, vContactTelAreaCode, vContactTelNumber, vContactFaxIntCode, vContactFaxAreaCode, vContactFaxNumber, vSalesAccountID, vSalesAccountCode, vPurchaseAccountID, vPurchaseAccountCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'some parameters are missing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            vAccountStatusID = ACTConst.ACTAccountStatusActive
            lPurgefrequencyID = ACTConst.ACTPurgeFreqNever
            iCurrencyID = vCurrencyID
            sAccountName = vName
            bRestrictEnquiry = False
            bRestrictUpdate = False
            bDeleteAtPurge = False


            If Information.IsNothing(vContactName) Then
                sContactName = ""
            Else
                sContactName = vContactName
            End If

            sAddress1 = ""
            sAddress2 = ""
            sAddress3 = ""
            sAddress4 = ""
            sPostalCode = ""
            iAddressCountry = 1


            If Information.IsNothing(vContactTelAreaCode) Then
                sPhoneAreaCode = ""
            Else
                sPhoneAreaCode = vContactTelAreaCode
            End If


            If Information.IsNothing(vContactTelNumber) Then
                sPhoneNumber = ""
            Else
                sPhoneNumber = vContactTelNumber
            End If

            sPhoneExtension = ""


            If Information.IsNothing(vContactFaxAreaCode) Then
                sFaxAreaCode = ""
            Else
                sFaxAreaCode = vContactFaxAreaCode
            End If


            If Information.IsNothing(vContactFaxNumber) Then
                sFaxNumber = ""
            Else
                sFaxNumber = vContactFaxNumber
            End If

            sFaxExtension = ""

            ' Both or Sales

            sShortCode = vShortName

            ' TF110401 - Get correct UIK for account_key

            '   SJP 04072002 - Account Key is now = Party Count
            '       Still passed into CalcCombinedKey but should just be
            '           returned.
            lAccountKey = vPartyCnt


            m_lReturn = CType(gPMComponentServices.calccombinedkey(v_lSourceID:=CInt(vSourceID), v_lKeyID:=CInt(vPartyID), r_lCombinedKeyID:=lAccountKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Generate Account Key.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MKW24062004 PN12470 check System Options
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)
            If vValue = "1" Then
                'Get SubBranch for Logon Company.

                m_lReturn = CType(bACTFunc.GetSubBranchID(v_oDatabase:=v_oDatabase, r_lSubBranchID:=lSubBranchID, v_vSourceID:=CStr(vSourceID)), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(bACTFunc.GetSubBranchID(v_oDatabase:=v_oDatabase, r_lSubBranchID:=lSubBranchID, v_vPartyCnt:=CStr(vPartyCnt)), gPMConstants.PMEReturnCode)
            End If

            'DD 08/08/2002: Added for multi-branch accounting
            'm_lReturn = GetSubBranchID(v_oDatabase:=v_oDatabase, _
            'r_lSubBranchID:=lSubBranchID, v_vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Get Sub Branch ID.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vLedgerFlag = "B" Or vLedgerFlag = "S" Then

                ' First Create Sales Ledger Account
                iAccounttypeID = ACTConst.ACTAccountTypeAsset

                'MKW200104 PN9853 Call additional procedure (with correct parameters) START
                'DD 08/08/2002: Added for multi-branch accounting
                '        m_lReturn = GetLedgerID(v_oDatabase:=v_oDatabase, _
                'r_lLedgerID:=lLedgerID, v_sLedgerCode:="SA", _
                'v_lSubBranchID:=lSubBranchID)
                m_lReturn = bACTFunc.GetLedgerIDFromShortName(v_oDatabase:=v_oDatabase, r_lLedgerID:=lLedgerID, v_sShortName:="SA", v_lSubBranchID:=lSubBranchID)
                'MKW200104 PN9853 Call additional procedure (with correct parameters) END

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Get Ledger ID.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' TF110401 - Pass correct Account Key and PartySourceID

                m_lReturn = m_oAccount.DirectAdd(vAccountId:=lAccountID, vPurgeFrequencyId:=lPurgefrequencyID, vAccounttypeID:=iAccounttypeID, vCurrencyID:=iCurrencyID, vLedgerId:=lLedgerID, vAccountName:=sAccountName, vShortCode:=sShortCode, vRestrictEnquiry:=bRestrictEnquiry, vRestrictUpdate:=bRestrictUpdate, vDeleteAtPurge:=bDeleteAtPurge, vContactName:=sContactName, vAddress1:=sAddress1, vAddress2:=sAddress2, vAddress4:=sAddress4, vPostalCode:=sPostalCode, vAddressCountry:=iAddressCountry, vPhoneAreaCode:=sPhoneAreaCode, vPhoneExtension:=sPhoneExtension, vFaxAreaCode:=sFaxAreaCode, vFaxNumber:=sFaxNumber, vFaxExtension:=sFaxExtension, vAccountStatusID:=vAccountStatusID, vAccountKey:=lAccountKey, vPartySourceID:=vSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Return the ids

                vSalesAccountID = lAccountID
                vSalesAccountCode = sShortCode

                ' Get the Node of The Ledger to create a child in it


                m_lReturn = m_oLedger.GetLedgerNodeId("Client Ledger", lNodeId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lElementID = m_oExplorer.InsertElement(sShortCode)

                If lElementID > 0 Then

                    lNodeId = m_oExplorer.InsertNode(lParentNodeId:=lNodeId, lElementID:=lElementID, vAccountId:=lAccountID)
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If vLedgerFlag = "B" Or vLedgerFlag = "P" Then

                '  Create Purchase Ledger Account
                ' BAD BAD BAD HARDCODED LEDGER ID

                iAccounttypeID = ACTConst.ACTAccountTypeLiability
                lLedgerID = 3
                'sShortCode = "P" & vShortName

                ' CTAF 070200 - Added PartyId
                ' TF110401 - Pass correct Account Key and PartySourceID

                m_lReturn = m_oAccount.DirectAdd(vAccountId:=lAccountID, vPurgeFrequencyId:=lPurgefrequencyID, vAccounttypeID:=iAccounttypeID, vCurrencyID:=iCurrencyID, vLedgerId:=lLedgerID, vAccountName:=sAccountName, vShortCode:=sShortCode, vRestrictEnquiry:=bRestrictEnquiry, vRestrictUpdate:=bRestrictUpdate, vDeleteAtPurge:=bDeleteAtPurge, vContactName:=sContactName, vAddress1:=sAddress1, vAddress2:=sAddress2, vAddress4:=sAddress4, vPostalCode:=sPostalCode, vAddressCountry:=iAddressCountry, vPhoneAreaCode:=sPhoneAreaCode, vPhoneExtension:=sPhoneExtension, vFaxAreaCode:=sFaxAreaCode, vFaxNumber:=sFaxNumber, vFaxExtension:=sFaxExtension, vAccountStatusID:=vAccountStatusID, vAccountKey:=lAccountKey, vPartySourceID:=vSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Return Ids

                vPurchaseAccountID = lAccountID
                vPurchaseAccountCode = sShortCode

                ' Get the Node of The Ledger to create a child in it


                m_lReturn = m_oLedger.GetLedgerNodeId("Purchase Ledger", lNodeId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lElementID = m_oExplorer.InsertElement(sShortCode)

                If lElementID > 0 Then

                    lNodeId = m_oExplorer.InsertNode(lParentNodeId:=lNodeId, lElementID:=lElementID, vAccountId:=lAccountID)
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

