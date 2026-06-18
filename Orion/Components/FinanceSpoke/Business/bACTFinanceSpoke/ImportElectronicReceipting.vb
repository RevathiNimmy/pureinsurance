Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'Developer Guide no.129
Imports SharedFiles
Imports System.Runtime.ExceptionServices
Friend NotInheritable Class ImportElecReceipting
    '====================================================================
    '   Class/Module: ImportElecReceipting

    '
    '====================================================================
    '   Maintenance History
    '
    'sw 09/01/2003 - Created


    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database

    ' ************************************************
    ' Added to replace global variables 24/09/2003
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

    Private oSharedStorage As SharedStorage
    Private oScriptControl As MSScriptControl.ScriptControl

    'developer guide no. 39
    Private Const ACGetMediaIDSQL As String = "spu_ACT_Get_MediaID_From_code"
    Private Const ACGetMediaIDName As String = "GetMediaID"

    'developer guide no. 39
    Private Const ACGetReceiptIDSQL As String = "spu_ACT_Get_ReceiptID_From_Code"
    Private Const ACGetReceiptIDName As String = "GetReceiptID"

    'developer guide no. 39
    Private Const ACCheckPartyExistsSQL As String = "spu_ACT_Check_Party_Exists"
    Private Const ACCheckPartyExistsName As String = "CheckPartyExists"

    'developer guide no. 39
    Private Const ACGetAccountAddressSQL As String = "spu_ACT_Get_Account_Address"
    Private Const ACGetAccountAddressName As String = "GetAccountAddress"

    'developer guide no. 39
    Private Const ACGetAccountIDsFromBatchSourceCodeSQL As String = "spu_ACT_Get_AccountIDs_From_BatchSourceCode"
    Private Const ACGetAccountIDsFromBatchSourceCodeName As String = "GetAccountIDsFromBatchSourceCode"

    'developer guide no. 39
    Private Const ACGetMatchingDebitsForBatchSQL As String = "spu_ACT_Get_Matching_Debits_For_Batch"
    Private Const ACGetMatchingDebitsForBatchName As String = "GetMatchingDebitsForBatch"

    Const ACClass As String = "ImportElecReceipting"


    Friend WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property

    Friend WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    Friend Function PassThroughLogin(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PassThroughLogin
        ' PURPOSE: Pass through the module level login information to the Class.
        ' This is for COM+. Normally a business class will not require this but the Spoke
        ' design means that Classes are instantiated by the Business class and can
        ' no longer rely on global variables.
        ' AUTHOR: Danny Davis
        ' DATE: 24 September 2003, 11:55 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PassThroughLogin", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: Start
    ' PURPOSE:steps through data passed in header and detail parameters, and creates, posts and allocates
    '         receipts accordingly
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Public Function Start(ByVal v_sBatchRef As Integer, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object, ByVal v_sHeaderXML As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        Dim oCashListPostItem As Object
        Dim oCashList As bACTCashList.Form
        Dim oAllocate As bACTAllocate.Business
        Dim oDocumentPost As bACTDocumentPost.Form
        Dim oAutoNumber As bACTAutoNumber.Business
        Dim oCashListItem As bACTCashlistitem.Form
        Dim oCashListPost As bACTCashListPost.Automated
        Dim vHeaderCol, vHeaderInfo As Object
        Dim vDetailCol As Array
        Dim vDetailInfo As Array
        'Developer Guide No 17
        Dim vPostDebt As Object
        Dim vAccount As Object
        Dim lRowTotal As Integer
        Dim vKeyArray(,) As Object

        Dim lMediaID As Integer
        Dim sCSV As New StringBuilder
        Dim dblBatchTotal, dblElementSum As Double
        Dim lCashListID, lReceiptTypeID As Integer
        Dim bDebtorExists As Boolean
        Dim lBankAccountID, lCollectionAccountID, lClientAccountID, lBatchID, lBatchSourceID As Integer

        Dim sContactName, sAddress1, sAddress2, sAddress3, sAddress4, sPostCode As String
        Dim lAddressCountry, lCashListItemID, lCLITransDetailID, lCreditTransDetailId, lDebitTransDetailId As Integer

        Dim bTranStarted As Boolean
        Dim lNumberRangeID, lNumber As Integer
        Dim sDocumentRef As String = ""
        Dim lDocumentID As Integer


        Dim cBatchTotal, cCurrencyAmount As Decimal

        Dim vMatchingDebits As Object
        Dim vCashListItem() As Object

        Const cOpen As Integer = 2
        Const cReceipt As Integer = 2
        Const cUnAllocated As Integer = 1
        Const ACMethod As String = "Start"
        Const cBatchStatusComplete As Integer = 3
        Const cBatchTypeElecReceiptID As Integer = 2
        Const cMiscDebtorAccount As String = "Misc Debtor"



        Try

            'define header detail columns


            vHeaderCol = r_vHeaderData(0)


            ReDim Preserve vHeaderCol(vHeaderCol.GetUpperBound(0))

            'define header info


            vHeaderInfo = r_vHeaderData(1)


            ReDim Preserve vHeaderInfo(vHeaderInfo.GetUpperBound(0))

            'define detail cols

            vDetailCol = r_vDetailData(0)

            vDetailCol = ArraysHelper.RedimPreserve(Of Object())(vDetailCol, New Integer() {vDetailCol.GetUpperBound(0) + 1}, New Integer() {0})

            'define detail info

            vDetailInfo = r_vDetailData(1)

            lRowTotal = vDetailInfo.GetUpperBound(1)

            vDetailInfo = ArraysHelper.RedimPreserve(Of Object(,))(vDetailInfo, New Integer() {vDetailInfo.GetUpperBound(0) + 1, lRowTotal + 1}, New Integer() {0, 0})

            'check to see whether the control batch total held in the header data
            'is equal to the total of the individual postings


            dblBatchTotal = CDbl(vHeaderInfo(ACERControlTotal))

            For lCount As Integer = 0 To lRowTotal
                dblElementSum += CDbl(vDetailInfo(ACERAmount, lCount))
            Next

            If Math.Round(dblElementSum, 2) <> Math.Round(dblBatchTotal, 2) Then
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SW Issue 3076 Check the number of transactions specified in the header is
            'the same as the actual no of transactions

            If (lRowTotal + 1) <> CInt(vHeaderInfo(ACERTotalNoOfTransactions)) Then
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_BUSINESS_VALIDATION_FAILED
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create a transaction
            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to create transaction")
            End If
            bTranStarted = True

            'get account Ids from batch source code

            m_lReturn = CType(GetAccountIDs(CStr(vHeaderInfo(ACERSourceCode)), lBankAccountID, lCollectionAccountID, lBatchSourceID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to establish account ID's")
            End If

            'create a new batch record

            m_lReturn = CType(m_oBusiness.CreateBatchRecord(r_lBatchID:=lBatchID, v_lBatchStatusID:=cBatchStatusComplete, v_lCompanyID:=m_iSourceID, v_lUserID:=m_iUserID, v_sBatchRef:="", v_dtCreatedDate:=DateTime.Now, v_dtAccountingDate:=DateTime.Now, v_lBatchTypeID:=cBatchTypeElecReceiptID, v_lBatchSourceID:=lBatchSourceID, v_sXML:=v_sHeaderXML, v_cTotalAmount:=dblBatchTotal, v_lTotalTransactions:=CInt(vHeaderInfo(ACERTotalNoOfTransactions)), v_dtImportedDate:=DateTime.Now, v_sInterfaceCode:=gHUBSpokeConstants.ksICElectronicReceipting), gPMConstants.PMEReturnCode)

            'check the return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error writing batch record")
            End If

            'update the batch ref with the ID just generated

            'create a new batch record
            m_lReturn = CType(m_oBusiness.UpdateBatchRef(v_lBatchID:=lBatchID, v_sBatchRef:=CStr(lBatchID)), gPMConstants.PMEReturnCode)
            'check the return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error updating batch record")
            End If

            'sw 20/03/2003 Assign the Sirius batch ID to the header data

            vHeaderInfo(ACERBatchIDSIR) = lBatchID

            'assign back to the header data


            r_vHeaderData(1) = vHeaderInfo

            'create the other business objects required for processing

            oCashList = New bACTCashList.Form
            m_lReturn = oCashList.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'check the return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "unable to create business object " & _
                                           "bACTCashList.Form")
            End If


            oCashListItem = New bACTCashListItem.Form
            m_lReturn = oCashListItem.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'check the return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "unable to create business object " & _
                                           "bACTCashListItem.Form")
            End If


            oAllocate = New bACTAllocate.Business
            m_lReturn = oAllocate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'check the return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "unable to create business object " & _
                                           "bACTAllocate.Business")
            End If


            oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'check the return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "unable to create business object " & _
                                           "bACTCashListPost.Automated")
            End If

            For lCount As Integer = 0 To lRowTotal

                'initialise the return values
                vAccount = ""
                vPostDebt = ""

                m_lReturn = CType(CheckDebtorExists(CStr(vDetailInfo(ACERDebtorNumber, lCount)), bDebtorExists), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to check for debtor")
                End If

                'claims specific stuff will need to be added in here as parameters to this call

                'run the script to find valid account
                m_lReturn = CType(RunScript(v_vDebtor:=CStr(vDetailInfo(ACERDebtorNumber, lCount)), v_vAgreement:=CStr(vDetailInfo(ACERClaimDebtID, lCount)), v_vClaimNumber:=CStr(vDetailInfo(ACERClaimNo, lCount)), v_vAmount:=CStr(vDetailInfo(ACERAmount, lCount)), v_vDebtorExists:=CStr(bDebtorExists), r_vPostDebt:=vPostDebt, r_vAccount:=vAccount), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", error running script")
                End If

                If lCount = 0 Then
                    'first time through the loop so create the cashlist

                    m_lReturn = oCashList.DirectAdd(vCashListID:=lCashListID, vCashListStatusID:=cOpen, vCashListTypeID:=cReceipt, vCashListRef:="", vCompanyID:=m_iSourceID, vBankAccountID:=lBankAccountID, vCurrencyID:=m_iCurrencyID, vListDate:=DateTime.Now, vControlTotal:=vHeaderInfo(ACERControlTotal), vBatch_id:=lBatchID, vItemCount:=vHeaderInfo(ACERTotalNoOfTransactions))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to add new cash list")
                    End If
                End If

                'build up the CSV format string

                sCSV = New StringBuilder(CStr(vDetailCol(ACERDebtorNumber - ACHubSpecificColumnCount)).Trim() & ", " & _
                       "" & CStr(vDetailInfo(ACERDebtorNumber, lCount)).Trim() & ", ")
                sCSV.Append(CStr(vDetailCol(ACERClaimDebtID - ACHubSpecificColumnCount)).Trim() & ", " & CStr(vDetailInfo(ACERClaimDebtID, lCount)).Trim() & ", ")
                sCSV.Append(CStr(vDetailCol(ACERClaimNo - ACHubSpecificColumnCount)).Trim() & _
                            ", " & CStr(vDetailInfo(ACERClaimNo, lCount)).Trim() & ", ")
                sCSV.Append(CStr(vDetailCol(ACERBankStatementCode - ACHubSpecificColumnCount)).Trim() & ", " & _
                            CStr(vDetailInfo(ACERBankStatementCode, lCount)).Trim() & ", ")
                sCSV.Append(CStr(vDetailCol(ACERBankStatementRef - ACHubSpecificColumnCount)).Trim() & ", " & CStr(vDetailInfo(ACERBankStatementRef, lCount)).Trim() & ", ")
                sCSV.Append(CStr(vDetailCol(ACERBankStatementParticulars - ACHubSpecificColumnCount)).Trim() & ", " & _
                            CStr(vDetailInfo(ACERBankStatementParticulars, lCount)).Trim() & ", ")
                sCSV.Append(CStr(vDetailCol(ACERBankStatementOtherPartyName - ACHubSpecificColumnCount)).Trim() & ", " & _
                            CStr(vDetailInfo(ACERBankStatementOtherPartyName, lCount)).Trim())

                'get the address details from account.
                m_lReturn = CType(GetAccountAddress(vAccount, lClientAccountID, sContactName, sAddress1, sAddress2, sAddress3, sAddress4, sPostCode, lAddressCountry), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to get account address")
                End If

                'find the receipt type ID for the receipt to be created
                m_lReturn = CType(GetReceiptID(CInt(Conversion.Val(CStr(vDetailInfo(ACERClaimDebtID, lCount)))), lReceiptTypeID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to establish receipt type")
                End If

                'find the media type id
                m_lReturn = CType(GetMediaID(CStr(vDetailInfo(ACERMediaTypeCode, lCount)), lMediaID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to establish media type")
                End If

                'Add the cashlistitem corresponding to the batch item
                ReDim vCashListItem(gACTLibrary.eCashListItem.LastItem)

                vCashListItem(gACTLibrary.eCashListItem.CashlistitemID) = lCashListItemID

                vCashListItem(gACTLibrary.eCashListItem.AllocationstatusID) = 1

                vCashListItem(gACTLibrary.eCashListItem.MediaRef) = lMediaID

                vCashListItem(gACTLibrary.eCashListItem.CashlistID) = lCashListID

                vCashListItem(gACTLibrary.eCashListItem.AccountID) = lClientAccountID

                vCashListItem(gACTLibrary.eCashListItem.MediaRef) = ""

                vCashListItem(gACTLibrary.eCashListItem.OurRef) = ""

                vCashListItem(gACTLibrary.eCashListItem.TheirRef) = ""

                vCashListItem(gACTLibrary.eCashListItem.Transaction_Date) = DateTime.Today.ToString("dd/MM/yyyy")

                vCashListItem(gACTLibrary.eCashListItem.Batch_id) = lBatchID

                vCashListItem(gACTLibrary.eCashListItem.Amount) = vDetailInfo(ACERAmount, lCount)

                vCashListItem(gACTLibrary.eCashListItem.Amount_Tendered) = vDetailInfo(ACERAmount, lCount)

                vCashListItem(gACTLibrary.eCashListItem.pmuser_id) = m_iUserID

                vCashListItem(gACTLibrary.eCashListItem.CashListItem_receipt_type_id) = lReceiptTypeID

                vCashListItem(gACTLibrary.eCashListItem.XML_Object) = sCSV.ToString()

                vCashListItem(gACTLibrary.eCashListItem.ContactName) = sContactName

                vCashListItem(gACTLibrary.eCashListItem.Address1) = sAddress1

                vCashListItem(gACTLibrary.eCashListItem.Address2) = sAddress2

                vCashListItem(gACTLibrary.eCashListItem.Address3) = sAddress3

                vCashListItem(gACTLibrary.eCashListItem.Address4) = sAddress4

                vCashListItem(gACTLibrary.eCashListItem.PostalCode) = sPostCode

                vCashListItem(gACTLibrary.eCashListItem.AddressCountry) = lAddressCountry


                m_lReturn = oCashListItem.DirectAdd(vCashListItem)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to add cashlistitem")
                End If

                'Retrieve the cashlistitem id from the array

                lCashListItemID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))

                'post cashlisttiem

                '########################################################################


                oCashListPost = New bACTCashListPost.Automated
                m_lReturn = oCashListPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                'check the return code
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "unable to create business object " & _
                                               "bACTCashListPost.Automated")
                End If

                ReDim vKeyArray(1, 1)

                vKeyArray(0, 0) = PMNavKeyConst.ACTKeyNameCashListId

                vKeyArray(1, 0) = lCashListID


                vKeyArray(0, 1) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeyArray(1, 1) = lCashListItemID


                m_lReturn = oCashListPost.SetKeys(vKeyArray:=vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to set keys for cash list post")
                End If


                m_lReturn = oCashListPost.PostUnallocatedCash(v_vCashListID:=lCashListID, v_vCashListItemID:=lCashListItemID, v_vBatchID:=lBatchID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to post unallocated cash")
                End If


                lCLITransDetailID = oCashListPost.CashTransId


                oCashListPost.Dispose()

                oCashListPost = Nothing

                'end of posting
                '#######################################################################

                'allocate the cashlistitem
                'Perform the auto allocation using the new auto allocate routine in the Allocate component


                'SW Issue 2806 06-03-2003.
                'do not attempt to autoallocate if the posting account is the Misc Debtor account

                If vAccount <> cMiscDebtorAccount Then



                    m_lReturn = oAllocate.AutoAllocate(lCLITransDetailID, "", v_lCashListItemID:=lCashListItemID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'do nothing, continue with rest of batch, not all items need to allocate
                        'for batch to process
                    End If
                End If

                '#######################################################

                'some extra functionality will need to go here sepcific to claims
                '(See electronic receipting spec)
                'At present this cannot be implemented due to incomplete dependancies
                'I understand that the IAG release date for the claims dependancies is late 2003
                'sw 01/2003

                '#######################################################

                'we also need to populate the individual record status's
                'to indicate success for hub communication

                r_vDetailData(conValue)(ACERRecordStatus, lCount) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS

                r_vDetailData(conValue)(ACERRecordMessage, lCount) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS
            Next

            'DD 26/08/2003: Only do this if we have more than one receipt
            If lRowTotal > 0 Then
                'all receipts have been created and allocated correctly
                'post a credit transaction for the total amount against client Account
                'do this so that the batch can be seen lumped together as one transaction


                oAutoNumber = New bACTAutoNumber.Business
                m_lReturn = oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                'check the return code
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "unable to create business object " & _
                                               "bACTCashListPost.Automated")
                End If

                'generate a document reference

                m_lReturn = oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSrp, r_lNumberRangeID:=lNumberRangeID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to get number range")
                End If
                'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                m_lReturn = oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSrp)

                'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable generate number")
                End If
                'Format the number
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'sDocumentRef = Format(lNumber, "00000000")

                sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeSrp & sDocumentRef

                'add the document

                m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeReceipt, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=DateTime.Today, v_sComment:="", v_vBatchID:=lBatchID, r_vDocumentId:=lDocumentID, r_vDocSourceId:=m_iSourceID)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to add document")
                End If

                cBatchTotal = gACTLibrary.ACTSigned(dblBatchTotal, gACTLibrary.ACTEAccountSign.acteSignCredit)

                'add the crediting transaction

                m_lReturn = oDocumentPost.AddTransaction(v_lAccountId:=lCollectionAccountID, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBatchTotal, v_cCurrencyAmount:=cBatchTotal, v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=lCreditTransDetailId, v_vDocumentSequence:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to add collection credit transaction")
                End If

                'now post a balancing debit transaction against the same account using the same document
                cBatchTotal = gACTLibrary.ACTSigned(dblBatchTotal, gACTLibrary.ACTEAccountSign.acteSignDebit)


                m_lReturn = oDocumentPost.AddTransaction(v_lAccountId:=lCollectionAccountID, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBatchTotal, v_cCurrencyAmount:=cBatchTotal, v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=2)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to add transaction")
                End If

                'Allocate the credit against the debit transactions for the individual receipts

                'Get the debits to allocate the new credit posting against
                m_lReturn = CType(GetMatchingDebitsForBatch(lBatchID, lDocumentID, vMatchingDebits), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to match debt")
                End If

                'Allocate the total credit just posted against the original
                'matching debits

                m_lReturn = CType(AllocateTotal(r_lAccountID:=lCollectionAccountID, r_lTransDetailId:=lCreditTransDetailId, r_cAmount:=cBatchTotal, r_vTransDetails:=vMatchingDebits, r_oAllocate:=oAllocate), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", unable to allocate")
                End If


            End If

            'Commit the changes
            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to commit trans")
                Return result
            End If

            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_COMPLETE

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

            If bTranStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If

            result = gPMConstants.PMEReturnCode.PMError

            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED

            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Return result

            End Select

        Finally

            If Not (oAllocate Is Nothing) Then

                oAllocate.Dispose()
                oAllocate = Nothing
            End If
            If Not (oCashListPost Is Nothing) Then

                oCashListPost.Dispose()
                oCashListPost = Nothing
            End If
            If Not (oCashList Is Nothing) Then

                oCashList.Dispose()
                oCashList = Nothing
            End If
            If Not (oCashListItem Is Nothing) Then

                oCashListItem.Dispose()
                oCashListItem = Nothing
            End If
            If Not (oDocumentPost Is Nothing) Then

                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If
            If Not (oAutoNumber Is Nothing) Then

                oAutoNumber.Dispose()
                oAutoNumber = Nothing
            End If


        End Try
        Return result
    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetMatchingDebitsForBatch
    ' PURPOSE:
    ' AUTHOR: Steve Watton  -  gets the debit transactions for a given batch
    ' DATE: 13/01/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Private Function GetMatchingDebitsForBatch(ByVal v_lBatchID As Integer, ByVal v_lDocumentId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetMatchingDebitsForBatch"



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="batchid", vValue:=CStr(v_lBatchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="documentid", vValue:=CStr(v_lDocumentId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACGetMatchingDebitsForBatchSQL, sSQLName:=ACGetMatchingDebitsForBatchName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        result = gPMConstants.PMEReturnCode.PMTrue

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
        Resume


        Select Case Information.Err().Number
            Case Constants.vbObjectError
                ' Log internal failure.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source)

                result = gPMConstants.PMEReturnCode.PMFalse

                GoTo Finally_Renamed

            Case Else
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                result = gPMConstants.PMEReturnCode.PMError

                GoTo Finally_Renamed

        End Select

Finally_Renamed:

        Return result

    End Function




    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: AllocateTotal
    ' PURPOSE:
    ' AUTHOR: Steve Watton  -  Virtual cut and paste from iACTCashList.Banking
    ' DATE: 13/01/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Private Function AllocateTotal(ByRef r_lAccountID As Integer, ByRef r_lTransDetailId As Integer, ByRef r_cAmount As Decimal, ByRef r_vTransDetails As Array, ByRef r_oAllocate As Object) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "AllocateTotal"

        Dim lNewUpperBound As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Add the new transaction to the array (this is the format that
            'PerformAutoAllocation requires)
            lNewUpperBound = r_vTransDetails.GetUpperBound(1) + 1
            r_vTransDetails = ArraysHelper.RedimPreserve(Of Object(,))(r_vTransDetails, New Integer() {r_vTransDetails.GetUpperBound(0) - r_vTransDetails.GetLowerBound(0) + 1, lNewUpperBound - r_vTransDetails.GetLowerBound(1) + 1}, New Integer() {r_vTransDetails.GetLowerBound(0), r_vTransDetails.GetLowerBound(1)})


            r_vTransDetails(0, lNewUpperBound) = r_lTransDetailId

            r_vTransDetails(1, lNewUpperBound) = r_cAmount * -1


            r_vTransDetails(2, lNewUpperBound) = r_vTransDetails(1, lNewUpperBound)

            'Attempt to do the allocation

            If r_oAllocate.PerformAutoAllocation(r_lAccountID:=r_lAccountID, r_lTransDetailId:=r_lTransDetailId, v_vOSTransactions:=r_vTransDetails) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to allocate total")
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As System.Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select

        Finally
        End Try

        Return result

    End Function





    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetAccountIDs
    ' PURPOSE: gets bank account ID and corresponding account ID from the batch source code
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetAccountIDs(ByVal v_sBatchSourceCode As String, ByRef r_lBankAccountID As Integer, ByRef r_lAccountID As Integer, ByRef r_lBatchSourceID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sBatchSourceCode.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACGetAccountIDsFromBatchSourceCodeSQL, sSQLName:=ACGetAccountIDsFromBatchSourceCodeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If Information.IsArray(vResultArray) Then

            r_lBankAccountID = CInt(vResultArray(0, 0))

            r_lAccountID = CInt(vResultArray(1, 0))

            r_lBatchSourceID = CInt(vResultArray(2, 0))
            result = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CheckDebtorExists
    ' PURPOSE:checks to see whether a debtor exists for a given debtor number (party.shortname)
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function CheckDebtorExists(ByVal sDebtorNumber As String, ByRef bDebtorExists As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="debtornumber", vValue:=sDebtorNumber.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACCheckPartyExistsSQL, sSQLName:=ACCheckPartyExistsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If Information.IsArray(vResultArray) Then

            bDebtorExists = Conversion.Val(CStr(vResultArray(0, 0))) > 0
            result = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetReceiptID
    ' PURPOSE:Returns the valid receipt type ID from the code, dependant on whether the
    '         recovery agreement ID has been set
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Function GetReceiptID(ByVal v_lRecoveryAgreementID As Integer, ByRef r_lReceiptTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        If v_lRecoveryAgreementID <> 0 Then
            'ADD clmrec PARAM
            If m_oDatabase.Parameters.Add(sName:="code", vValue:="CLMREC", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
        Else
            'ADD STD PARAM
            If m_oDatabase.Parameters.Add(sName:="code", vValue:="STD", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
        End If

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACGetReceiptIDSQL, sSQLName:=ACGetReceiptIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If Information.IsArray(vResultArray) Then

            r_lReceiptTypeID = CInt(vResultArray(0, 0))
            result = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetAccountAddress
    ' PURPOSE:Returns the Address for the account ID passed
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Function GetAccountAddress(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer, ByRef r_sContactName As String, ByRef r_sAddress1 As String, ByRef r_sAddress2 As String, ByRef r_sAddress3 As String, ByRef r_sAddress4 As String, ByRef r_sPostCode As String, ByRef r_lAddressCountry As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Const cAccountID As Integer = 0
        Const cContactName As Integer = 1
        Const cAddress1 As Integer = 2
        Const cAddress2 As Integer = 3
        Const cAddress3 As Integer = 4
        Const cAddress4 As Integer = 5
        Const cPostCode As Integer = 6
        Const cAddressCountry As Integer = 7



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        'ADD clmrec PARAM
        If m_oDatabase.Parameters.Add(sName:="accountshortcode", vValue:=v_sShortCode.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACGetAccountAddressSQL, sSQLName:=ACGetAccountAddressName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If Information.IsArray(vResultArray) Then

            r_lAccountID = CInt(vResultArray(cAccountID, 0))

            r_sContactName = CStr(vResultArray(cContactName, 0)).Trim()

            r_sAddress1 = CStr(vResultArray(cAddress1, 0)).Trim()

            r_sAddress2 = CStr(vResultArray(cAddress2, 0)).Trim()

            r_sAddress3 = CStr(vResultArray(cAddress3, 0)).Trim()

            r_sAddress4 = CStr(vResultArray(cAddress4, 0)).Trim()

            r_sPostCode = CStr(vResultArray(cPostCode, 0)).Trim()

            r_lAddressCountry = IIf(CStr(vResultArray(cAddressCountry, 0)).Trim() <> "", CInt(CStr(vResultArray(cAddressCountry, 0)).Trim()), 0)
            result = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetMediaID
    ' PURPOSE:Gets the media ID from the code
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetMediaID(ByRef v_sMediaCode As String, ByRef r_lMediaID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="mediacode", vValue:=v_sMediaCode.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACGetMediaIDSQL, sSQLName:=ACGetMediaIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If Information.IsArray(vResultArray) Then

            r_lMediaID = CInt(vResultArray(0, 0))
            result = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: RunScript
    ' PURPOSE:Runs the Rules file for electronic receipting
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    'Developer Guide No 101
	<HandleProcessCorruptedStateExceptions>
    Private Function RunScript(Optional ByVal v_vDebtor As Object = Nothing, Optional ByVal v_vAgreement As Object = Nothing, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vAmount As Object = Nothing, Optional ByVal v_vDebtorExists As Object = Nothing, Optional ByVal v_vClaimExists As Object = Nothing, Optional ByVal v_vAgreementExists As Object = Nothing, Optional ByVal v_vDebtorCount As Object = Nothing, Optional ByVal v_vAgreementCount As Object = Nothing, Optional ByVal v_vUnpaidAmount As Object = Nothing, Optional ByRef r_vPostDebt As Object = Nothing, Optional ByRef r_vAccount As Object = Nothing) As Integer

        Dim sScript As String = ""

        Try

            m_lReturn = CType(GetScriptFile(sScript), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create script control object
            oScriptControl = New MSScriptControl.ScriptControl()

            'create shared storage object, used to hold values that are read/writable from the VB script file
            oSharedStorage = New SharedStorage()

            oScriptControl.Language = "VBScript"

            'populate the values that are held in the script  control object

            If Not Information.IsNothing(v_vDebtor) Then
                If v_vDebtor <> "" Then
                    oSharedStorage.Debtor = v_vDebtor.Trim()
                End If
            End If


            If Not Information.IsNothing(v_vAgreement) Then
                If v_vAgreement <> "" Then
                    oSharedStorage.Agreement = v_vAgreement.Trim()
                End If
            End If


            If Not Information.IsNothing(v_vClaimNumber) Then
                If v_vClaimNumber <> "" Then
                    oSharedStorage.ClaimNumber = v_vClaimNumber.Trim()
                End If
            End If


            If Not Information.IsNothing(v_vAmount) Then
                If v_vAmount <> "" Then
                    oSharedStorage.Amount = v_vAmount.Trim()
                End If
            End If


            If Not Information.IsNothing(v_vDebtorExists) Then
                If v_vDebtorExists <> "" Then
                    oSharedStorage.DebtorExists = v_vDebtorExists.Trim()
                End If
            End If


            If Not Information.IsNothing(v_vClaimExists) Then
                If v_vClaimExists <> "" Then
                    oSharedStorage.ClaimExists = v_vClaimExists.Trim()
                Else
                    oSharedStorage.ClaimExists = CStr(False).Trim()
                End If
            Else
                oSharedStorage.ClaimExists = CStr(False).Trim()
            End If


            If Not Information.IsNothing(v_vAgreementExists) Then
                If v_vAgreementExists <> "" Then
                    oSharedStorage.AgreementExists = v_vAgreementExists.Trim()
                Else
                    oSharedStorage.AgreementExists = CStr(False).Trim()
                End If
            Else
                oSharedStorage.AgreementExists = CStr(False).Trim()
            End If


            If Not Information.IsNothing(v_vDebtorCount) Then
                If v_vDebtorCount <> "" Then
                    oSharedStorage.DebtorCount = v_vDebtorCount.Trim()
                Else
                    oSharedStorage.DebtorCount = CStr(0).Trim()
                End If
            Else
                oSharedStorage.DebtorCount = CStr(0).Trim()
            End If


            If Not Information.IsNothing(v_vAgreementCount) Then
                If v_vAgreementCount <> "" Then
                    oSharedStorage.AgreementCount = v_vAgreementCount.Trim()
                Else
                    oSharedStorage.AgreementCount = CStr(0).Trim()
                End If
            Else
                oSharedStorage.AgreementCount = CStr(0).Trim()
            End If


            If Not Information.IsNothing(v_vUnpaidAmount) Then
                If v_vUnpaidAmount <> "" Then
                    oSharedStorage.UnpaidAmount = v_vUnpaidAmount.Trim()
                Else
                    oSharedStorage.UnpaidAmount = CStr(0).Trim()
                End If
            Else
                oSharedStorage.UnpaidAmount = CStr(0).Trim()
            End If

            'Add reference to sharedstorage object on the scriptcontrol object
            oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)

            'read in the script and run it
            oScriptControl.AddCode(sScript.Trim())

            oScriptControl.Run("Start")



            r_vPostDebt = oSharedStorage.PostDebt



            r_vAccount = oSharedStorage.Account

            oSharedStorage = Nothing

            oScriptControl = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RunScript", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

            oSharedStorage = Nothing
            oScriptControl = Nothing

        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetScriptFile
    ' PURPOSE:locate the appropriate script file
    ' AUTHOR: Steve Watton
    ' DATE: 08/01/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetScriptFile(ByRef v_sScript As String) As Integer

        Dim result As Integer = 0
        Dim sPathName, sFullPath As String
        Dim intFile As Integer
        Dim lFileLength As Integer

        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String



        result = gPMConstants.PMEReturnCode.PMTrue
        '
        'get the path to the validation script from thje registry
        '
        lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

        'Build the path to the script file
        sFullPath = sPathName & "\BatchReceiptRules.rul"

        'locate the file
        If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        intFile = FileSystem.FreeFile()

        FileSystem.FileOpen(intFile, sFullPath, OpenMode.Input)

        lFileLength = FileSystem.LOF(intFile)

        'read the basic into the string variable
        sStr2 = FileSystem.InputString(intFile, lFileLength)

        FileSystem.FileClose(intFile)

        'build the full script
        sStr = ""

        sStr = sStr & "Option Explicit" & Strings.Chr(13) & Strings.Chr(10)

        sStr = sStr & sStr2 & Strings.Chr(13) & Strings.Chr(10)

        'return the script
        v_sScript = sStr.Trim()

        Return result

    End Function
End Class
