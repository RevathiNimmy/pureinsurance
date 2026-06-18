Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportOneOffReceipts 
	'====================================================================
	'   Class/Module: ExportRecurringPayments
	'   Description : Class implementation of use case:
	'Export for InterfaceCode: "ONEOFF"'
	'
	'====================================================================
	'   Maintenance History
	'
	'    28 February 2002    Paul Cunnigham    Created.
	'
	'====================================================================
	
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "ExportOneOffPayments"
	
	'#Region " Private fields "
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_oBusiness As Business
	Private m_oDatabase As dPMDAO.Database
	'#End Region
	
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
	
	'#Region " Private Enums "
	'Columns in HeaderData array
	'Private Enum HeaderDataCols
	'    MediaTypeCode = 0
	'    BranchCode
	'    ExtractDate
	'End Enum
	'#End Region
	
	'#Region " Stored Procedures "
	'Private Const ksSPExportExtractTransSQL = "{call spu_ACT_Spoke_ExportExtractTrans (?,?,?)}"
	'Private Const ksSPExportExtractTransName = "GetExportExtractTrans"
	'Private Const ksSPExportExtractTransStored = True
	'#End Region
	
	'#Region " Friend Properties "
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
	'#End Region
	
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
	
	
	'#Region " Friend Methods "
	Friend Function Start(ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Start
		' PURPOSE: Start process for use case
		' AUTHOR: Paul Cunnigham
		' DATE: 28 February 2003, 11:45:03
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Const ACMethod As String = "Start"
		
		Dim vResults, vHeaderArrayValues As Object
		
		Dim dtDueDateLimit As Date
		
		Dim dTotalAmount As Double
		Dim lBatchID, lTotalTransactions As Integer
		
		Dim lPFInstalmentsResultsId, lStatusId As Integer
		
		Dim sSQLWhere, sSQLWhereBatch, sSQLInsert, sSelectFields, sSourceList, sTableName As String
		Dim sSQLUpdate As New StringBuilder
		
		Dim vBatchId As Object
		Dim vDetailData( ,  ) As Object
		
		Dim sStr() As String
		Dim lMediaCodeID As Integer
		Dim sMediaCode, sBatchType As String
		Dim lBatchTypeID As Integer
		
		'SMJB CQ1130 08/08/03
		Dim vCredit( ,  ) As Object, vDebit( ,  ) As Object
		Dim bTransactionActive As Boolean
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
		
		bTransactionActive = False
		
		'We need valid database and business objects
		If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Business and Database object are not set")
		End If
		
		'OK do the Export processing...
		
		If Not Information.IsArray(r_vHeaderData) Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Header data array empty, unable to proceed further.")
		End If
		
		'Get the values from the header array


		vHeaderArrayValues = r_vHeaderData(conValue)
		
		'Initialise variables
		lStatusId = 0
		dTotalAmount = 0
		
		'Prepare SQL Search criteria from supplied header data.

        dtDueDateLimit = DateTime.Today.AddDays(CDbl(vHeaderArrayValues(ehdLeadDays)))


        If Strings.Len(CStr(vHeaderArrayValues(ehdSourceList))) > 0 Then

            sSourceList = CStr(vHeaderArrayValues(ehdSourceList)).Replace(";", "','")
        End If

        With m_oDatabase

            sSQLWhere = "WHERE [CLI].transaction_date <= '" & dtDueDateLimit.ToString("yyyy/MM/dd") & "'" & Strings.Chr(13) & Strings.Chr(10)

            If sSourceList.Length > 0 Then
                'Must be some source items listed.
                sSQLWhere = sSQLWhere & "AND [SRC1].code IN ('" & sSourceList & "')" & Strings.Chr(13) & Strings.Chr(10)
            End If

            'and batchid is null


            sSQLWhere = sSQLWhere & "AND UPPER([MTP].code) = '" & CStr(vHeaderArrayValues(ehdMediaTypeCode)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

            ' KG 01/08/03
            ' CQ 2056 - Only export Transactions that have an ID (i.e. that have been posted)
            ' DD 13/08/2003: Extended to include Salvage Receipts
            sSQLWhere = sSQLWhere & "AND ([CLI].transdetail_id IS NOT NULL OR [CLI].cashlistitem_receipt_type_id=4)" & Strings.Chr(13) & Strings.Chr(10)

            'SMJB 08/08/03 - Filter out reversed transactions
            sSQLWhere = sSQLWhere & "AND [CLI].cashlistitem_reverse_reason_id IS NULL" & Strings.Chr(13) & Strings.Chr(10)


            If CInt(vHeaderArrayValues(ehdAccountsFilter)) = 1 Then
                sSQLWhere = sSQLWhere & "AND [SRC1].source_id = [SRC2].source_id" & Strings.Chr(13) & Strings.Chr(10)
            ElseIf CInt(vHeaderArrayValues(ehdAccountsFilter)) = 2 Then
                sSQLWhere = sSQLWhere & "AND [SRC1].source_id <> [SRC2].source_id" & Strings.Chr(13) & Strings.Chr(10)
            End If

            'sw 18/03/2003
            'SMJB CQ1130 Moved this to the bottom of the SQL and separated it so that I can re-use the SQL
            'for the grouping process without having to re-generate the entire where clause
            'The only difference is that I will have a batch id as this process has updated it
            sSQLWhereBatch = sSQLWhereBatch & "AND [CLI].Batch_ID is Null" & Strings.Chr(13) & Strings.Chr(10)


            If RetrieveRecords(v_sWhereClause:=sSQLWhere & sSQLWhereBatch, r_vResults:=vResults, v_bGroupRecords:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error(s) during retrieval of records")
            End If

            'Nothing to do if no records found
            If Information.IsArray(vResults) Then
                If m_oBusiness.GetIDValueFromCode(v_sTableName:="BatchStatus", v_bGettingCode:=False, r_sCode:=conBICode, r_lID:=lStatusId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.BatchStatus." & conBICode)
                End If


                sMediaCode = CStr(vHeaderArrayValues(ehdMediaTypeCode))

                lMediaCodeID = 0

                If m_oBusiness.GetIDValueFromCode(v_sTableName:="mediatype", v_bGettingCode:=False, r_sCode:=sMediaCode, r_lID:=lMediaCodeID) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.mediatype." & sMediaCode)
                End If


                sBatchType = CStr(vHeaderArrayValues(ehdBatchTypeCode))

                If m_oBusiness.GetIDValueFromCode(v_sTableName:="batch_type", v_bGettingCode:=False, r_sCode:=sBatchType, r_lID:=lBatchTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.batch_type." & sBatchType)
                End If

                'Begin a transaction
                If .SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to start transaction")
                End If
                bTransactionActive = True



                m_lReturn = CType(m_oBusiness.CreateBatchRecord(r_lBatchID:=lBatchID, v_lBatchStatusID:=lStatusId, v_lCompanyID:=m_iSourceID, v_lUserID:=m_iUserID, v_sBatchRef:=r_sBatchRef, v_dtCreatedDate:=DateTime.Today, v_sComment:=CStr(vHeaderArrayValues(ehdBatchCode)), v_lBatchTypeID:=lBatchTypeID, v_dtExportDate:=DateTime.Today, v_lMediaTypeID:=lMediaCodeID, v_sInterfaceCode:=r_sInterfaceCode, v_iAutoClose:=Conversion.Val(CStr(vHeaderArrayValues(ehdAutoClose)))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while inserting new record into Batch table")
                End If

                'pass the batch ref back as the batch id
                r_sBatchRef = CStr(lBatchID)

                'update the batch record
                m_lReturn = CType(m_oBusiness.UpdateBatchRef(v_lBatchID:=lBatchID, v_sBatchRef:=CStr(lBatchID)), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error whilst updating Batch record")
                End If

                If m_oBusiness.GetIDValueFromCode(v_sTableName:="PFInstalments_Result", v_bGettingCode:=False, r_sCode:=conWaitingForCollectionCode, r_lID:=lPFInstalmentsResultsId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.PFInstalments_Result." & conWaitingForCollectionCode)
                End If

                'Add one array element to r_vDetailData per row in vResults

                vDetailData = ArraysHelper.RedimPreserve(Of Object(,))(vDetailData, New Integer() {gHUBSpokeConstants.eddAgent - gHUBSpokeConstants.eddDetailId + 1, vResults.GetUpperBound(conRows - 1) + 1}, New Integer() {gHUBSpokeConstants.eddDetailId, 0})

                lTotalTransactions = vResults.GetUpperBound(conRows - 1) + 1


                For iLoop As Integer = 0 To vResults.GetUpperBound(conRows - 1)
                    'Link CashLinkItem records to the newly created batch record using batch_id column.
                    sSQLUpdate = New StringBuilder("UPDATE cashlistitem ")
                    sSQLUpdate.Append("SET batch_id = " & lBatchID & Strings.Chr(13) & Strings.Chr(10))

                    sSQLUpdate.Append("WHERE cashlistitem_id = " & CStr(vResults(oopCashListItemCashListItemId, iLoop)))

                    sTableName = "cashlistitem"

                    If .SQLAction(sSQL:=sSQLUpdate.ToString(), sSQLName:="Update " & sTableName, bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error while updating " & sTableName)
                    End If


                    If m_oBusiness.PopulateDetailArray(v_sInterfaceCode:=r_sInterfaceCode, r_vDetailData:=vDetailData, v_vResults:=vResults, v_iElementNumber:=iLoop) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error populating detail array")
                    End If


                    dTotalAmount += CDbl(vResults(oopCashListItemAmount, iLoop))
                Next iLoop

                'round to 2 dp SW 17-04-2003
                dTotalAmount = Math.Round(dTotalAmount, 2)

                'Update header array with total batch amount

                r_vHeaderData(conValue)(ehdBatchAmount) = dTotalAmount

                r_vHeaderData(conValue)(ehdTotalNoOfTransactions) = lTotalTransactions

                lStatusId = 0
                If m_oBusiness.GetIDValueFromCode(v_sTableName:="BatchStatus", v_bGettingCode:=False, r_sCode:=conBECode, r_lID:=lStatusId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.BatchStatus." & conBECode)
                End If

                'Update batch status to 'exported' (BE).
                'Update Batch Total Amount and Total Transactions fields.
                sSQLUpdate = New StringBuilder("UPDATE Batch" & Strings.Chr(13) & Strings.Chr(10))
                sSQLUpdate.Append("SET batchstatus_id = " & lStatusId & conComma & Strings.Chr(13) & Strings.Chr(10))
                sSQLUpdate.Append("total_amount = " & dTotalAmount & conComma & Strings.Chr(13) & Strings.Chr(10))
                sSQLUpdate.Append("total_transactions = " & lTotalTransactions & Strings.Chr(13) & Strings.Chr(10))
                sSQLUpdate.Append("WHERE batch_id = " & lBatchID)

                If .SQLAction(sSQL:=sSQLUpdate.ToString(), sSQLName:="Update Batch details", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while updating batch status")
                End If


                'If we have results then we need to add a new element to the
                'r_vDetailData array

                If AddResultArrayToDetailArray(v_vDetailArray:=r_vDetailData, r_vResultArray:=vDetailData, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add results to Detail Array")
                End If
            End If

            'SMJB CQ1130 07/08/03
            'Now we need to get the same records, but this time we need to group it by cashlist_id
            'We have to change the WhereBatch clause because batch_id is not null anymore
            sSQLWhereBatch = "AND [CLI].Batch_ID = " & lBatchID

            'Retreive the records
            If RetrieveRecords(v_sWhereClause:=sSQLWhere & sSQLWhereBatch, r_vResults:=vResults, v_bGroupRecords:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error(s) during retrieval of records")
            End If

            'Loop through creating our arrays for the PostTransaction function
            If Information.IsArray(vResults) Then


                For lCount As Integer = 0 To vResults.GetUpperBound(1)
                    ReDim Preserve vCredit(2, lCount)
                    ReDim Preserve vDebit(2, lCount)


                    vCredit(0, lCount) = vResults(1, lCount)


                    vCredit(1, lCount) = vResults(3, lCount)



                    vDebit(0, lCount) = vResults(2, lCount)


                    vDebit(1, lCount) = vResults(3, lCount)
                Next

                'Add our credit and debit transactions
                If m_oBusiness.PostTransaction(v_vCreditAccount:=vCredit, v_vDebitAccount:=vDebit, v_sComment:="Total", v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error(s) posting transaction totals")
                End If
            End If
            'Commit the transactions
            If .SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while commiting transaction")
            End If
            bTransactionActive = False

        End With
		
		
		
		
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
		
		If Information.IsArray(vResults) Then
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			result = gPMConstants.PMEReturnCode.PMNotFound
		End If
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Constants.vbObjectError
				' Log internal failure.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
		End Select
		
		'Rollback any active transactions
		If bTransactionActive Then
			m_oDatabase.SQLRollbackTrans()
		End If
		
		Finally
		
		
		End Try
		Return result
	End Function
	'#End Region
	
	
	
	'#Region " Private Methods "
	Public Sub New()
		MyBase.New()
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Class_Initialize
		' PURPOSE: Class initialisation
		' AUTHOR: Paul Cunnigham
		' DATE: 11 November 2002, 12:08:23
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		
		Try
		
		'Class initialisation
		m_oBusiness = Nothing
		m_oDatabase = Nothing
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
	 
		
		End Try
	End Sub
	
	
	' ***************************************************************** '
	' Name: RetrieveRecords
	'
	' Description: Retrieves all the records associated with a batch
	'
	' ***************************************************************** '
	Public Function RetrieveRecords(ByVal v_sWhereClause As String, ByRef r_vResults(,) As Object, ByVal v_bGroupRecords As Boolean) As Integer 
		
		Dim result As Integer = 0 
		Const ACMethod As String = "RetrieveRecords"
		
		Dim sSQLSearch As String = "" 
		Dim vResults(,) As Object 
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'Need to take the relevant records from the CashListItem table
		'Criteria to be used.
		'*************************************************************************
		'CashListItem.Transaction_Date <= (Today + Lead Days)
		'CashListItem.Source = On Source List
		'CashListItem.MediaType.MediaType_Validation = Media Type Validation
		'If Accounts Filter = 1
		'CashListItem.Source = Party.Source
		'If Accounts Filter = 2
		'CashListItem.Source <> Party.Source
		'*************************************************************************
		
		'sw issue 2866 13/03/2003 added in extra fields for cc details.
		
		
		If v_bGroupRecords Then
			sSQLSearch = "SELECT [CLD].cashlist_drawer_id, [CLD].collection_account_id, [CLD].suspense_account_id, Sum([CLI].amount)[Total]"
			sSQLSearch = sSQLSearch & " FROM CashListItem[CLI]"
			sSQLSearch = sSQLSearch & " LEFT JOIN CashList[CL] ON [CL].CashList_id = [CLI].CashList_id"
			sSQLSearch = sSQLSearch & " INNER JOIN CashList_Drawer[CLD] ON [CLD].CashList_Drawer_id = [CL].CashList_Drawer_id"
			sSQLSearch = sSQLSearch & " LEFT JOIN Source[SRC1] ON [SRC1].source_id = [CL].Company_id"
			sSQLSearch = sSQLSearch & " LEFT JOIN MediaType[MTP] on [MTP].mediatype_id = [CLI].mediatype_id"
			sSQLSearch = sSQLSearch & " LEFT JOIN Account[ACC] ON [ACC].account_id = [CLI].account_id"
			sSQLSearch = sSQLSearch & " LEFT JOIN Party[PTY] ON [PTY].party_cnt = [ACC].account_key"
			sSQLSearch = sSQLSearch & " LEFT JOIN Source[SRC2] ON [SRC2].source_id = [PTY].source_id "
			
			sSQLSearch = sSQLSearch & v_sWhereClause
			
			sSQLSearch = sSQLSearch & Strings.Chr(13) & Strings.Chr(10) &  _
			             "GROUP BY [CLD].cashlist_drawer_id, [CLD].collection_account_id, [CLD].suspense_account_id"
		Else
			sSQLSearch = "SELECT [CLI].cashlistitem_id, [PTY].shortname, [PTY].name, [CLI].payment_name,"
			sSQLSearch = sSQLSearch & " [CLI].amount, [CLI].cc_number, [CLI].cc_expiry_date, [CLI].cc_start_date,"
			sSQLSearch = sSQLSearch & " [CLI].cc_issue, [CLI].cc_pin, [CLI].transdetail_id, [PTY].alternative_identifier,"
			sSQLSearch = sSQLSearch & " [PTY].agent_cnt" & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " FROM CashListItem[CLI]" & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " LEFT JOIN CashList[CL] ON [CL].CashList_id = [CLI].CashList_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " LEFT JOIN Source[SRC1] ON [SRC1].source_id = [CL].Company_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " LEFT JOIN MediaType[MTP] on [MTP].mediatype_id = [CLI].mediatype_id " & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " LEFT JOIN Account[ACC] ON [ACC].account_id = [CLI].account_id" & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " LEFT JOIN Party[PTY] ON [PTY].party_cnt = [ACC].account_key" & Strings.Chr(13) & Strings.Chr(10)
			sSQLSearch = sSQLSearch & " LEFT JOIN Source[SRC2] ON [SRC2].source_id = [PTY].source_id " & Strings.Chr(13) & Strings.Chr(10)
			
			sSQLSearch = sSQLSearch & v_sWhereClause
		End If
		
		
		If m_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Installment-Payment Search", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults) <> gPMConstants.PMEReturnCode.PMTrue Then
			
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error(s) during retrieval of records")
		End If
		
		'Error if we don't retrive any records?
		'    If Not (IsArray(vResults)) Then
		'        Err.Raise vbObjectError, ACMethod, "Search returned no payment/installment results"
		'    End If
		
		'Pass back the complete SQL query results set.


		r_vResults = vResults
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Constants.vbObjectError
				' Log internal failure.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
		End Select
		
		Finally
		
		End Try
		Return result
	End Function
End Class
