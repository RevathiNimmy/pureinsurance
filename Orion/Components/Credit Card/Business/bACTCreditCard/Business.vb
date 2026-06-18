Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    'Link to Retail Logic Code
    'Private m_oSolveSE As SolveSE

    'The Collect Payment result array
    Private Enum eCCCollectPayment
        cashlisttype_id = 0
        collector_code
        amount
        cc_name
        cc_customer
        cc_number
        cc_expiry_date
        cc_start_date
        cc_issue
        cc_pin
        cc_auth_code
        cc_manual_auth_code
        cc_transaction_code
        address1
        postal_code
        connector_host
        connector_port
        connector_timeout
    End Enum

    'The Media Type Connector result array
    Private Enum eCCValidate
        mediatype_connector_id = 0
        code
        Description
        connector_host
        connector_port
        connector_timeout
    End Enum

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    ' PUBLIC Property Procedures (End)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Retail Logic SolveSE Class for later
            ' as we expand we will add more classes for other connectors
            'm_oSolveSE = New SolveSE()
            'm_oSolveSE.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    '
    ' Name: GetMediaTypeIssuerAndConnectorData
    '
    ' Description: Gets data required from the MediaType_Issuer and
    '              MediaType_Connector tables for a given MediaType_Issuer_ID.
    '
    ' ***************************************************************** '
    Public Function GetMediaTypeIssuerAndConnectorData(ByVal v_lMediaType_Issuer_ID As Integer, ByRef r_vOutputDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="mediatype_issuer_id", vValue:=CStr(v_lMediaType_Issuer_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", mediatype_issuer_id:=" & CStr(v_lMediaType_Issuer_ID))
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelMediaTypeIssuerAndConnectorDataSQL, sSQLName:=ACSelMediaTypeIssuerAndConnectorDataName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vOutputDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", sSQL = " & ACSelMediaTypeIssuerAndConnectorDataSQL)
            End If


        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypeIssuerAndConnectorData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypeIssuerAndConnectorData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPreviouslyUsedCCNumbers
    '
    ' Description: Gets data related to previously used Credit Cards.
    '
    ' ***************************************************************** '
    Public Function GetPreviouslyUsedCCNumbers(ByVal v_lAccountID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lMediatypeIssuerID As Integer, ByVal v_bIsClaimTypePayment As Boolean, ByRef r_vOutputDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", account_id = " & CStr(v_lAccountID))
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", insurance_file_cnt = " & CStr(v_lInsuranceFileCnt))
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="mediatype_issuer_id", vValue:=CStr(v_lMediatypeIssuerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", mediatype_issuer_id = " & CStr(v_lMediatypeIssuerID))
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_claim_type_payment", vValue:=CStr(v_bIsClaimTypePayment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", is_claim_type_payment = " & CStr(v_bIsClaimTypePayment))
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelPreviosulyUsedCCDataSQL, sSQLName:=ACSelPreviosulyUsedCCDataName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vOutputDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", sSQL=" & ACSelPreviosulyUsedCCDataSQL)
            End If


        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviouslyUsedCCNumbers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreviouslyUsedCCNumbers", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    Public Function AuthorisePayment(ByVal sMediaTypeConnector As String, ByVal bIsReceipt As Boolean, ByVal cCCAmount As Decimal, ByVal lCCCurrencyID As Integer, ByVal sCCNumber As String, ByVal sCCName As String, ByVal sCCExpiry As String, ByVal sCCStart As String, ByVal sCCIssue As String, ByVal sCCPin As String, ByVal sCCAddress1 As String, ByVal sCCPostcode As String, ByVal sCCCustomerFlag As String, ByRef r_sCCReturnStatus As String, ByRef r_sCCAutoAuthCode As String, ByRef r_sCCTransactionCode As String, Optional ByRef r_sResultXML As String = "", Optional ByRef bCancel As Boolean = False) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AuthorisePayment
        ' PURPOSE: Authorise a payment from a credit card
        ' AUTHOR: Danny Davis
        ' DATE: 13 January 2005, 10:53:41
        ' RETURNS: PMTrue for success, along with Authorisation Code
        '          PMFalse for failure, along with Error Message
        '          PMFail for comms failure
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all the information we need from the database
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("mediatype_connector_code", sMediaTypeConnector, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_lReturn = .SQLSelect("{call spu_ACT_Select_MediaType_Connector_for_code(?)}", "Select Select_MediaType_Connector_for_code", True, , vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", CollectPayment, spu_ACT_Select_MediaType_Connector_for_code failed.")
                End If
            End With

            If Informations.IsArray(vResultArray) Then
                Select Case sMediaTypeConnector
                    Case "RL"
                        'Perform the retail logic Complete Transaction process
                        'this will collect the money from the customer



                        'result = m_oSolveSE.SolveSE(iSolveSEMessageID:=MainModule.eSolveSEMessage.eAuthorisationAndSettle, sHost:=CStr(vResultArray(eCCValidate.connector_host, 0)), sPort:=CStr(vResultArray(eCCValidate.connector_port, 0)), iTimeout:=CInt(vResultArray(eCCValidate.connector_timeout, 0)), bIsReceipt:=bIsReceipt, sSourceID:=GetICCS(), sCCNumber:=sCCNumber, sCCStartDate:=sCCStart, sCCEndDate:=sCCExpiry, sCCIssue:=sCCIssue, sCCPin:=sCCPin, sCCAddress1:=sCCAddress1, sCCPostcode:=sCCPostcode, sCCCustomerPresent:=sCCCustomerFlag, sCCKeyedOrSwiped:="keyed", cCCAmount:=cCCAmount, r_sCCAuthorisationCode:=r_sCCAutoAuthCode, r_sCCTransactionCode:=r_sCCTransactionCode, r_sExtendedError:=r_sCCReturnStatus, r_sResultXML:=r_sResultXML)
                    Case Else
                        'Not a supported credit card, skip
                        'send PMFalse to force UI to do manual validation
                        result = gPMConstants.PMEReturnCode.PMFalse
                End Select
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AuthorisePayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function


    Public Function CollectPayment(Optional ByVal lCashListItemID As Integer = 0, Optional ByVal sMediaTypeConnector As String = "", Optional ByVal sCCTransactionCode As String = "", Optional ByRef r_sCCReturnStatus As String = "", Optional ByRef r_sResultXML As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CollectPayment
        ' PURPOSE: Commit the validated Connector payment
        ' AUTHOR: Danny Davis
        ' DATE: 13 January 2005, 10:54:05
        ' RETURNS: PMTrue for success
        '          PMFalse for failure, along with Error Message
        '          PMFail for comms failure
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sUniqueID As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            sMediaTypeConnector = ""

            If lCashListItemID <> 0 Then
                'Get all the information we need from the database using CashListItem
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("cashlistitem_id", CStr(lCashListItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .SQLSelect("{call spu_ACT_Select_CollectCCPayment_For_CashListItem (?)}", "Select CollectCCPayment_For_CashListItem", True, , vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", CollectPayment, spu_ACT_Select_CollectCCPayment_For_CashListItem failed.")
                    End If
                End With
                If Informations.IsArray(vResultArray) Then

                    sMediaTypeConnector = CStr(vResultArray(eCCCollectPayment.collector_code, 0)).Trim()
                End If
            Else
                'This is a manual completion without the receipt
                'Get all the information we need from the database
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("mediatype_connector_code", sMediaTypeConnector, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    m_lReturn = .SQLSelect("{call spu_ACT_Select_MediaType_Connector_for_code(?)}", "Select Select_MediaType_Connector_for_code", True, , vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", CollectPayment, spu_ACT_Select_MediaType_Connector_for_code failed.")
                    End If
                End With
                If Informations.IsArray(vResultArray) Then

                    sMediaTypeConnector = CStr(vResultArray(eCCValidate.code, 0)).Trim()
                End If
            End If

            ''If we get nothing then a Connector is not set up
            'Select Case sMediaTypeConnector
            '	Case "RL"
            '		'Perform the Retail Logic Complete Transaction process
            '		'this will collect the money from the customer
            '		If lCashListItemID <> 0 Then

            '			result = m_oSolveSE.SolveSE(iSolveSEMessageID:=MainModule.eSolveSEMessage.eCompleteTransaction, sHost:=CStr(vResultArray(eCCCollectPayment.connector_host, 0)), sPort:=CStr(vResultArray(eCCCollectPayment.connector_port, 0)), iTimeout:=CInt(vResultArray(eCCCollectPayment.connector_timeout, 0)), bIsReceipt:=True, sSourceID:=GetICCS(), sCCNumber:=CStr(vResultArray(eCCCollectPayment.cc_number, 0)), sCCStartDate:=CStr(vResultArray(eCCCollectPayment.cc_start_date, 0)), sCCEndDate:=CStr(vResultArray(eCCCollectPayment.cc_expiry_date, 0)), sCCIssue:=CStr(vResultArray(eCCCollectPayment.cc_issue, 0)), sCCPin:=CStr(vResultArray(eCCCollectPayment.cc_pin, 0)), sCCAddress1:=CStr(vResultArray(eCCCollectPayment.address1, 0)), sCCPostcode:=CStr(vResultArray(eCCCollectPayment.postal_code, 0)), sCCCustomerPresent:=CStr(vResultArray(eCCCollectPayment.cc_customer, 0)), sCCKeyedOrSwiped:="keyed", cCCAmount:=CDec(vResultArray(eCCCollectPayment.amount, 0)), r_sCCAuthorisationCode:=CStr(vResultArray(eCCCollectPayment.cc_auth_code, 0)), r_sCCTransactionCode:=CStr(vResultArray(eCCCollectPayment.cc_transaction_code, 0)), r_sExtendedError:=r_sCCReturnStatus, r_sResultXML:=r_sResultXML)
            '		Else
            '			'Perform the Manual Complete Transaction process
            '			'this will collect the money from the customer



            '			result = m_oSolveSE.SolveSE(iSolveSEMessageID:=MainModule.eSolveSEMessage.eCompleteTransaction, sHost:=CStr(vResultArray(eCCValidate.connector_host, 0)), sPort:=CStr(vResultArray(eCCValidate.connector_port, 0)), iTimeout:=CInt(vResultArray(eCCValidate.connector_timeout, 0)), bIsReceipt:=True, sSourceID:=GetICCS(), sCCNumber:="", sCCStartDate:="", sCCEndDate:="", sCCIssue:="", sCCPin:="", sCCAddress1:="", sCCPostcode:="", sCCCustomerPresent:="", sCCKeyedOrSwiped:="keyed", cCCAmount:=0, r_sCCAuthorisationCode:="", r_sCCTransactionCode:=sCCTransactionCode, r_sExtendedError:=r_sCCReturnStatus, r_sResultXML:=r_sResultXML)
            '		End If

            '	Case Else
            '		'Not a supported credit card, skip
            'End Select


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CollectPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    Public Function CancelPayment(ByVal sMediaTypeConnector As String, ByVal sCCTransactionCode As String, Optional ByRef r_sCCReturnStatus As String = "", Optional ByRef r_sResultXML As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CancelPayment
        ' PURPOSE: Cancels a Payment previously marked for authorisation
        ' AUTHOR: Danny Davis
        ' DATE: 17 January 2005, 13:48:49
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all the information we need from the database
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("mediatype_connector_code", sMediaTypeConnector, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_lReturn = .SQLSelect("{call spu_ACT_Select_MediaType_Connector_for_code(?)}", "Select Select_MediaType_Connector_for_code", True, , vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", CollectPayment, spu_ACT_Select_MediaType_Connector_for_code failed.")
                End If
            End With

            If Informations.IsArray(vResultArray) Then
                'Select Case sMediaTypeConnector
                '	Case "RL"
                '		'Perform the retail logic Complete Transaction process
                '		'this will collect the money from the customer



                '		result = m_oSolveSE.SolveSE(iSolveSEMessageID:=MainModule.eSolveSEMessage.eCancelTransaction, sHost:=CStr(vResultArray(eCCValidate.connector_host, 0)), sPort:=CStr(vResultArray(eCCValidate.connector_port, 0)), iTimeout:=CInt(vResultArray(eCCValidate.connector_timeout, 0)), bIsReceipt:=True, sSourceID:=GetICCS(), sCCNumber:="", sCCStartDate:="", sCCEndDate:="", sCCIssue:="", sCCPin:="", sCCAddress1:="", sCCPostcode:="", sCCCustomerPresent:="", sCCKeyedOrSwiped:="keyed", cCCAmount:=0, r_sCCAuthorisationCode:="", r_sCCTransactionCode:=sCCTransactionCode, r_sExtendedError:=r_sCCReturnStatus, r_sResultXML:=r_sResultXML)
                '	Case Else
                '		'Not a supported credit card, skip
                '		'send PMFalse to force UI to do manual validation
                '		result = gPMConstants.PMEReturnCode.PMFalse
                'End Select
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AuthorisePayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetICCS
    ' Description: Gets the ICCS number from the Sirius Architechure
    ' database.
    ' ***************************************************************** '
    Private Function GetICCS() As String

        Dim result As String = String.Empty


        result = "0000"

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add the InsuranceFileID parameter (INPUT)


            'developer guide no. 85
            m_lReturn = .Parameters.Add("ICCS", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = .SQLAction(sSQL:="{call spu_pm_iccs(?)}", sSQLName:="spu_pm_iccs", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = gPMFunctions.ToSafeString(.Parameters.Item("ICCS").Value, "0000")
        End With

        Return result

    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' GetDefaultCreditCardByAccount
    ''' </summary>
    ''' <param name="v_nAccountID"></param>
    ''' <param name="r_vOutputDetails"></param>
    ''' <returns></returns>
    Public Function GetDefaultCreditCardByAccount(ByVal v_nAccountID As Integer, ByRef r_vOutputDetails(,) As Object) As Integer

        Dim nResult As Integer = 0

        Try
            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="accountid", vValue:=CStr(v_nAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.Parameters.Add", "nResult = " & nResult & ", sCallingAppName = " & ACApp & ", account_id = " & CStr(v_nAccountID))
            End If

            ' Execute SQL Statement
            nResult = m_oDatabase.SQLSelect(sSQL:=kGetDefaultCreditcardByAccountSQL, sSQLName:=kGetDefaultCreditcardByAccountName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vOutputDetails)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.SQLSelect", "nResult = " & nResult & ", sCallingAppName = " & ACApp & ", sSQL=" & ACSelPreviosulyUsedCCDataSQL)
            End If

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviouslyUsedCCNumbers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreviouslyUsedCCNumbers", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            nResult = PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

End Class

