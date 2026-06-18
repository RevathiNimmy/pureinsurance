Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportGeneralLedger 
	' ***************************************************************** '
	' Class Name: Business
	'
	' Date: 08/10/1998
	'
	' Description: Creatable Business class which contains all the
	'              methods, Business rules required to manipulate
	'              a SIRAddress.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "Business"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	'Header Array Constants
	Private Const ACHDocumentOrDate As Integer = 9
	Private Const ACHDateFrom As Integer = 10
	Private Const ACHDateTo As Integer = 11
	Private Const ACHDocumentIdFrom As Integer = 12
	Private Const ACHDocumentIdTo As Integer = 13
	Private Const ACHeaderDetailSize As Integer = 13
	
	'Detail Array Constants
	
	Private Const ACDCompanyCode As Integer = 0
	Private Const ACDPostingDate As Integer = 1
	Private Const ACDSubBranchCode As Integer = 2
	Private Const ACDAccountName As Integer = 3
	Private Const ACDJournalLegType As Integer = 4
	Private Const ACDAmount As Integer = 5
	
	Private Const ACDetailDataSize As Integer = 5
	
	
	'Node Group Array
	Private Const ACAParentNodeId As Integer = 0
	Private Const ACADescription As Integer = 1
	Private Const ACANodeId As Integer = 2
	
	'Return from structure tree query
	Private Const ACSNodeId As Integer = 0
	Private Const ACSDescription As Integer = 1
	Private Const ACSAccumulateNodes As Integer = 2
	
	'Status code constants
	
	''Private Const ACStatusCode1 = "1 - Invalid Date Range"
	''Private Const ACStatusCode2 = "2 - Invalid Document Id"
	''Private Const ACStatusCode3 = "3 - Non Zero Balance"
	''Private Const ACStatusCode4 = "4 - HeaderData is not an Array"
	''Private Const ACStatusCode5 = "5 - HeaderData Invalid Number of Columns"
	''Private Const ACStatusCode6 = "6 - Invalid Document or Date flag"
	''Private Const ACStatusCode7 = "7 - Invalid Date Format"
	''Private Const ACStatusCode8 = "8 - Failed to get documentId From Date"
	''Private Const ACStatusCode9 = "9 - System Error"
	
	
	Private Const ACName As Integer = 0
	Private Const ACValue As Integer = 1
	
	' PRIVATE Data Members (Begin)
	
	' Database Class (Private)
	Private m_oDatabase As dPMDAO.Database
	Private m_oBusiness As Business
	
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
	
	' Close Database Flag (Private)
	Private m_bCloseDatabase As Boolean
	
	' Error Code (Private)
	Private m_lReturn As Integer
	
	' Process Mode Properties
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	'SQL Statements
	
	' Delete contents of general_ledger_group table
	Private Const ACDeleteGeneralLedgerGroupStored As Boolean = True
	Private Const ACDeleteGeneralLedgerGroupName As String = "DeleteGeneralLedgerGroup"
    'developer guide no. 39
    Private Const ACDeleteGeneralLedgerGroupSQL As String = "spu_ACT_general_ledger_group_del"
	
	' Insert a record into general_ledger_group table
	Private Const ACAddGeneralLedgerGroupStored As Boolean = True
	Private Const ACAddGeneralLedgerGroupName As String = "AddGeneralLedgerGroup"
    'developer guide no. 39
    Private Const ACAddGeneralLedgerGroupSQL As String = "spu_ACT_general_ledger_group_add"
	
	' Select transactions for grouped accounts export
	Private Const ACSelectGeneralLedgerGroupStored As Boolean = True
	Private Const ACSelectGeneralLedgerGroupName As String = "SelectGeneralLedgerGroup"
    'developer guide no. 39
    Private Const ACSelectGeneralLedgerGroupSQL As String = "spu_ACT_general_ledger_group_sel"
	
	' Select transactions for grouped accounts export
	Private Const ACSelectGeneralLedgerStored As Boolean = True
	Private Const ACSelectGeneralLedgerName As String = "SelectGeneralLedger"
    'developer guide no. 39
    Private Const ACSelectGeneralLedgerSQL As String = "spu_ACT_general_ledger_sel"
	
	' Select transactions for grouped accounts export
	Private Const ACSelectDocumentStored As Boolean = True
	Private Const ACSelectDocumentName As String = "SelectDocument"
    'developer guide no. 39
    Private Const ACSelectDocumentSQL As String = "spu_ACT_select_document"
	
	'Select nodes from structure tree which are not themselves accounts
	Private Const ACSelectGroupNodesStored As Boolean = True
	Private Const ACSelectGroupNodesName As String = "SelectDocument"
    'developer guide no. 39
    Private Const ACSelectGroupNodesSQL As String = "spu_ACT_group_nodes_sel"
	
	'Check to see if any children of the current node are themselves accounts
	Private Const ACCheckChildAccountNodesStored As Boolean = True
	Private Const ACCheckChildAccountNodesName As String = "CheckChildAccountNodes"
    'developer guide no. 39
    Private Const ACCheckChildAccountNodesSQL As String = "spu_ACT_check_child_account_nodes_sel"
	
	'Get the maximum/minimum document ids for a given date
	Private Const ACMaxMinDocumentIdStored As Boolean = True
	Private Const ACMaxMinDocumentIdName As String = "MaxMinDocumentId"
    'developer guide no. 39
    Private Const ACMaxMinDocumentIdSQL As String = "spu_ACT_max_min_document_id_sel"
	
	
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
	
	
	' ***************************************************************** '
	' Name: Start
	'
	' Description: Main public method called from Sirius Hub
	'
	' History: 20/11/2002 sj - Created.
	'
	' ***************************************************************** '
	Public Function Start(ByVal v_sInterfaceCode As String, ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByVal v_sHeaderXML As String, ByRef r_vHeaderData As Object, ByRef r_vDetailData As Object) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim bUseDocumentIds As Boolean
			Dim dtDateFrom, dtDateTo As Date
			Dim lDocumentIdFrom, lDocumentIdTo As Integer
			
			r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
			r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
			
			'Validate the Header data array

			m_lReturn = ValidateHeaderData(v_vHeaderData:=r_vHeaderData, r_sStatusCode:=r_sStatusCode, r_bUseDocumentIds:=bUseDocumentIds, r_dtDateFrom:=dtDateFrom, r_dtDateTo:=dtDateTo, r_lDocumentIdFrom:=lDocumentIdFrom, r_lDocumentIdTo:=lDocumentIdTo)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        If r_sStatusCode = "" Then
				'            r_sStatusCode = ACStatusCode9
				'        End If
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ValidateHeaderData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
				Return result
			End If
			
			' Delete the contents of the general_ledger_group table
			m_lReturn = DeleteGeneralLedgerGroupTable()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        If r_sStatusCode = "" Then
				'            r_sStatusCode = ACStatusCode9
				'        End If
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="DeleteGeneralLedgerGroupTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
				Return result
			End If
			
			'Traverse the structure tree creating records in the general_ledger_group table
			m_lReturn = TraverseStructureTree(0)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        If r_sStatusCode = "" Then
				'            r_sStatusCode = ACStatusCode9
				'        End If
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="TraverseStructureTree Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
				Return result
			End If
			
			'Build the output detail data array

			m_lReturn = BuildDetailDataArray(v_lDocumentIdFrom:=lDocumentIdFrom, v_lDocumentIdTo:=lDocumentIdTo, r_vDetailData:=r_vDetailData)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
				r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
				Return gPMConstants.PMEReturnCode.PMNotFound
			End If
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'        If r_sStatusCode = "" Then
				'            r_sStatusCode = ACStatusCode9
				'        End If
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildDetailDataArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
				Return result
			End If
			
			r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
			r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: ValidateHeaderData
	'
	' Description:
	'
	' History: 20/11/2002 sj - Created.
	'
	' ***************************************************************** '
	Private Function ValidateHeaderData(ByVal v_vHeaderData() As Object, ByRef r_sStatusCode As String, ByRef r_bUseDocumentIds As Boolean, ByRef r_dtDateFrom As Date, ByRef r_dtDateTo As Date, ByRef r_lDocumentIdFrom As Integer, ByRef r_lDocumentIdTo As Integer) As Integer
		
		Dim result As Integer = 0
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim sDate As String = ""
			
			'Check to see if we have a valid array
			If Not Information.IsArray(v_vHeaderData) Then
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Header Data is not an array", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
				'      r_sStatusCode = ACStatusCode4
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Check the array has the correct number of columns

			If v_vHeaderData(ACName).GetUpperBound(0) <> ACHeaderDetailSize Then
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Header Data has the wrong number of columns", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
				'      r_sStatusCode = ACStatusCode5
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Validate the document or date flag

            Select Case CStr(v_vHeaderData(ACValue)(ACHDocumentOrDate)).ToUpper()
                Case "D"
                    r_bUseDocumentIds = False
                Case "I"
                    r_bUseDocumentIds = True
                Case Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Document or Date flag", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    '          r_sStatusCode = ACStatusCode6
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            If r_bUseDocumentIds Then
                'If we are using document ids then validate them

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(v_vHeaderData(ACValue)(ACHDocumentIdTo)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    '           r_sStatusCode = ACStatusCode2
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(v_vHeaderData(ACValue)(ACHDocumentIdFrom)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    '          r_sStatusCode = ACStatusCode2
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lDocumentIdFrom = CInt(Conversion.Val(CStr(v_vHeaderData(ACValue)(ACHDocumentIdFrom))))

                r_lDocumentIdTo = CInt(Conversion.Val(CStr(v_vHeaderData(ACValue)(ACHDocumentIdTo))))

                'Validate document id from against database
                m_lReturn = ValidateDocumentId(r_lDocumentIdFrom)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Document Id failed for " & r_lDocumentIdFrom, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    '          r_sStatusCode = ACStatusCode2
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Validate document id to against database
                m_lReturn = ValidateDocumentId(r_lDocumentIdTo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Document Id failed for " & r_lDocumentIdTo, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    '          r_sStatusCode = ACStatusCode2
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If Not r_bUseDocumentIds Then
                'We are using dates

                'Validate date from

                m_lReturn = ValidateDate(v_vDate:=CStr(v_vHeaderData(ACValue)(ACHDateFrom)), r_dtDate:=r_dtDateFrom, r_sStatusCode:=r_sStatusCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate date failed for " & CStr(v_vHeaderData(ACValue)(ACHDateFrom)), vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Validate date to

                m_lReturn = ValidateDate(v_vDate:=CStr(v_vHeaderData(ACValue)(ACHDateTo)), r_dtDate:=r_dtDateTo, r_sStatusCode:=r_sStatusCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate date failed for " & CStr(v_vHeaderData(ACValue)(ACHDateTo)), vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If r_dtDateFrom > r_dtDateTo Then
                    '           r_sStatusCode = ACStatusCode1
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Date from is after date to", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get document id from date from
                m_lReturn = GetDocumentIdFromDate(r_lDocumentId:=r_lDocumentIdFrom, v_dtDate:=r_dtDateFrom, v_bFromDate:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get document id from date from failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    '          r_sStatusCode = ACStatusCode8
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get document id from date to
                m_lReturn = GetDocumentIdFromDate(r_lDocumentId:=r_lDocumentIdTo, v_dtDate:=r_dtDateTo, v_bFromDate:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get document id from date to failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    '        r_sStatusCode = ACStatusCode8
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'check that the document ID's are valid
                If r_lDocumentIdFrom > r_lDocumentIdTo Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No documents exist in the date range supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateHeaderData")
                    '          r_sStatusCode = ACStatusCode1
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

    End Function
    ' ***************************************************************** '
    ' Name: ValidateDate
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateDate(ByVal v_vDate As String, ByRef r_dtDate As Date, ByRef r_sStatusCode As String) As Integer

        Dim result As Integer = 0
        

            Dim sDate As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            'Default date if not set
            If v_vDate.Trim() = "" Then
                r_dtDate = DateTime.Now
                Return result
            End If

            If Not Information.IsDate(v_vDate) Then
                '       r_sStatusCode = ACStatusCode7
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                r_dtDate = CDate(v_vDate)
            End If

            Return result

    End Function
    ' ***************************************************************** '
    ' Name: ValidateDocumentId
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateDocumentId(ByVal v_lDocumentId As Integer) As Integer 

        Dim result As Integer = 0 
        

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = "" 
            Dim vResultArray(,) As Object 

            With m_oDatabase

                .Parameters.Clear()

                'document_id_from
                m_lReturn = .Parameters.Add(sName:="document_id", vValue:=CStr(v_lDocumentId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '        sSql = "SELECT document_id FROM document " & vbCrLf
                '        sSql = sSql & "WHERE document_id = " & v_lDocumentId

                m_lReturn = .SQLSelect(sSQL:=ACSelectDocumentSQL, sSQLName:=ACSelectDocumentName, bStoredProcedure:=ACSelectDocumentStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACSelectDocumentSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateDocumentId")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentIdFromDate
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocumentIdFromDate(ByRef r_lDocumentId As Integer, ByVal v_dtDate As Date, ByVal v_bFromDate As Boolean) As Integer 

        Dim result As Integer = 0 
        

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Dim sSQL As String
            Dim vResultArray(,) As Object 
            Dim iMaxInd As Integer 
            '    Dim sDate1 As String
            '    Dim sDate2 As String
            '    Dim sQualifier As String

            '    sDate1 = FormatField(PMFormatDateLong, v_dtDate)
            '    sDate2 = FormatField(PMFormatDateLong, DateAdd("d", 1, v_dtDate))
            '    If v_bFromDate = True Then
            '        sQualifier = "MIN"
            '    Else
            '        sQualifier = "MAX"
            '    End If
            If v_bFromDate Then
                iMaxInd = 0
            Else
                iMaxInd = 1
            End If

            With m_oDatabase

                '        sSQL = "select " & sQualifier & "(document_id) from document" & vbCrLf
                '        sSQL = sSQL & "where created_date >= " & "'" & sDate1 & "'" & vbCrLf
                '        sSQL = sSQL & "and created_date  < " & "'" & sDate2 & "'"

                .Parameters.Clear()

                'Minimum or Maximum
                m_lReturn = .Parameters.Add(sName:="max_ind", vValue:=CStr(iMaxInd), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for " & "max_ind", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentIdFromDate")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'effective_date
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for " & "effective_date", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentIdFromDate")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACMaxMinDocumentIdSQL, sSQLName:=ACMaxMinDocumentIdName, bStoredProcedure:=ACMaxMinDocumentIdStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMaxMinDocumentIdSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentIdFromDate")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                r_lDocumentId = CInt(Conversion.Val(CStr(vResultArray(0, 0))))
            End If

            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: TraverseStructureTree
    '
    ' Description:
    '
    ' History: 19/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function TraverseStructureTree(ByVal v_iNodeId As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

        Dim vNodeArray(,) As Object
        Dim vGroupNodeArray(,) As Object
            Dim bHasChildAccountNodes As Boolean

            m_lReturn = GetStructureTreeDetails(v_lParentNodeId:=v_iNodeId, r_vNodeArray:=vNodeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetStructureTreeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TraverseStructureTree")
                Return result
            End If

            If Not Information.IsArray(vNodeArray) Then
                Return result
            End If


            For i As Integer = 0 To vNodeArray.GetUpperBound(1)

                'Has this node got any child notes which are accounts (account_id is not null)

                bHasChildAccountNodes = HasChildAccountNodes(CInt(vNodeArray(ACSNodeId, i)))

                ' Do we need to group this structure tree folder

                If CDbl(vNodeArray(ACSAccumulateNodes, i)) = 1 Then
                    'If this node has child accounts then add it to the array
                    If bHasChildAccountNodes Then



                        InsertIntoNodeGroupArray(v_lNodeId:=CInt(vNodeArray(ACSNodeId, i)), v_lParentNodeId:=CInt(vNodeArray(ACSNodeId, i)), v_sDescription:=CStr(vNodeArray(ACSDescription, i)), r_vNodeArray:=vGroupNodeArray)
                    Else
                        'The node has no child accounts so drill down futher



                        m_lReturn = BuildGroupArray(v_lNodeId:=CInt(Conversion.Val(CStr(vNodeArray(ACSNodeId, i)))), r_vNodeArray:=vGroupNodeArray, v_sDescription:=CStr(vNodeArray(ACSDescription, i)), v_lParentNodeId:=CInt(vNodeArray(ACSNodeId, i)))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildGroupArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TraverseStructureTree")
                            Return result
                        End If
                    End If
                    'If we have found any nodes then add them to the database
                    If Information.IsArray(vGroupNodeArray) Then

                        m_lReturn = BuildGeneralLedgerGroup(v_vGroupNodeArray:=vGroupNodeArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildGeneralLedgerGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TraverseStructureTree")
                            Return result
                        End If

                    End If
                End If


                If Not bHasChildAccountNodes Then
                    Debug.WriteLine(vNodeArray(ACSDescription, i))
                Else
                    Debug.WriteLine(VB6.TabLayout(vNodeArray(ACSDescription, i), "***"))
                End If

                'Carry on down structure tree searching for any folders to group

                m_lReturn = TraverseStructureTree(v_iNodeId:=CInt(vNodeArray(ACSNodeId, i)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Recursive call to TraverseStructureTree Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TraverseStructureTree")
                    Return result
                End If

            Next i


            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: BuildGroupArray
    '
    ' Description:
    '
    ' History: 19/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function BuildGroupArray(ByVal v_lNodeId As Integer, ByRef r_vNodeArray(,) As Object, ByVal v_sDescription As String, ByVal v_lParentNodeId As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

        Dim vNodeArray(,) As Object
        Dim vGroupNodeArray(,) As Object

            m_lReturn = GetStructureTreeDetails(v_lParentNodeId:=v_lNodeId, r_vNodeArray:=vNodeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetStructureTreeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildGroupArray")
                Return result
            End If

            If Not Information.IsArray(vNodeArray) Then
                Return result
            End If


            For i As Integer = 0 To vNodeArray.GetUpperBound(1)


                If HasChildAccountNodes(CInt(vNodeArray(ACSNodeId, i))) Then


                    InsertIntoNodeGroupArray(v_lNodeId:=CInt(vNodeArray(ACSNodeId, i)), v_lParentNodeId:=v_lParentNodeId, v_sDescription:=v_sDescription, r_vNodeArray:=r_vNodeArray)

                Else
                    'The node has no child accounts so drill down futher

                    m_lReturn = BuildGroupArray(v_lNodeId:=CInt(vNodeArray(ACSNodeId, i)), r_vNodeArray:=r_vNodeArray, v_sDescription:=v_sDescription, v_lParentNodeId:=v_lParentNodeId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Recursive call to BuildGroupArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildGroupArray")
                        Return result
                    End If
                End If

            Next i


            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetStructureTreeDetails
    '
    ' Description:
    '
    ' History: 19/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetStructureTreeDetails(ByVal v_lParentNodeId As Integer, ByRef r_vNodeArray(,) As Object) As Integer 

        Dim result As Integer = 0 
        

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = "" 
        Dim vResultArray(,) As Object

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="parent_node_id", vValue:=CStr(v_lParentNodeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m-oDatabase..Parameters.Add Failed for parent_node_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructureTreeDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '        sSql = "select s.node_id, e.element_name, x.group_for_gl_export_ind" & vbCrLf
                '        sSql = sSql & "from structuretree s, element e, elementextras x" & vbCrLf
                '        sSql = sSql & "Where s.element_id = e.element_id" & vbCrLf
                '        sSql = sSql & "AND e.element_id = x.element_id" & vbCrLf
                '        sSql = sSql & "AND s.account_id is null" & vbCrLf
                '        sSql = sSql & "AND s.parent_node_id = " & v_lParentNodeId

                m_lReturn = .SQLSelect(sSQL:=ACSelectGroupNodesSQL, sSQLName:=ACSelectGroupNodesName, bStoredProcedure:=ACSelectGroupNodesStored, vResultArray:=r_vNodeArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACSelectGroupNodesSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructureTreeDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return result

    End Function

    Private Function HasChildAccountNodes(ByVal v_lNodeId As Integer) As Boolean 

        Dim result As Boolean = False 
        

            Dim sSQL As String = "" 
            Dim vResultArray(,) As Object 


            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="parent_node_id", vValue:=CStr(v_lNodeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for parent_node_id", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertIntoNodeGroupArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '        sSql = "SELECT account_id FROM structuretree " & vbCrLf
                '        sSql = sSql & "WHERE parent_node_id = " & v_lNodeId & vbCrLf
                '        sSql = sSql & "AND account_id IS NOT NULL"

                m_lReturn = .SQLSelect(sSQL:=ACCheckChildAccountNodesSQL, sSQLName:=ACCheckChildAccountNodesName, bStoredProcedure:=ACCheckChildAccountNodesStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACCheckChildAccountNodesSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertIntoNodeGroupArray")
                    Return False
                End If
            End With

            If Information.IsArray(vResultArray) Then
                result = True
            End If

            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: InsertIntoNodeGroupArray
    '
    ' Description:
    '
    ' History: 19/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Sub InsertIntoNodeGroupArray(ByVal v_lNodeId As Integer, ByVal v_lParentNodeId As Integer, ByVal v_sDescription As String, ByRef r_vNodeArray(,) As Object)

        

            Dim lIndex As Integer

            If Information.IsArray(r_vNodeArray) Then
                lIndex = r_vNodeArray.GetUpperBound(1) + 1
                ReDim Preserve r_vNodeArray(2, lIndex)
            Else
                lIndex = 0
                ReDim r_vNodeArray(2, lIndex)
            End If


            r_vNodeArray(ACANodeId, lIndex) = v_lNodeId

            r_vNodeArray(ACAParentNodeId, lIndex) = v_lParentNodeId

            r_vNodeArray(ACADescription, lIndex) = v_sDescription


    End Sub
    'Private Sub DebugCollection()
    '
    'If m_cNodeCollection Is Nothing = True Then
    '    Exit Sub
    'End If
    'Dim vArray As Variant
    'Dim i As Integer
    '
    'For Each vArray In m_cNodeCollection
    '    For i = 0 To UBound(vArray, 2)
    '        Debug.Print vArray(0, i) & " : " & vArray(1, i) & " : " & vArray(2, i)
    '    Next i
    'Next vArray
    '
    'End Sub
    ' ***************************************************************** '
    '
    ' Name: DeleteGeneralLedgerGroupTable
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteGeneralLedgerGroupTable() As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                m_lReturn = .SQLAction(sSQL:=ACDeleteGeneralLedgerGroupSQL, sSQLName:=ACDeleteGeneralLedgerGroupName, bStoredProcedure:=ACDeleteGeneralLedgerGroupStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACDeleteGeneralLedgerGroupSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGeneralLedgerGroupTable")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: BuildGeneralLedgerGroup
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function BuildGeneralLedgerGroup(ByVal v_vGroupNodeArray(,) As Object) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Information.IsArray(v_vGroupNodeArray) Then
                Return result
            End If


            For i As Integer = 0 To v_vGroupNodeArray.GetUpperBound(1)



                m_lReturn = AddGeneralLedgerGroup(v_lParentStructureTreeId:=CInt(Conversion.Val(CStr(v_vGroupNodeArray(ACAParentNodeId, i)))), v_sDescription:=CStr(v_vGroupNodeArray(ACADescription, i)), v_lChildStructureTreeId:=CInt(Conversion.Val(CStr(v_vGroupNodeArray(ACANodeId, i)))))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AddGeneralLedgerGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildGeneralLedgerGroup")
                    Return result
                End If

            Next i

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddGeneralLedgerGroup
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AddGeneralLedgerGroup(ByVal v_lParentStructureTreeId As Integer, ByVal v_sDescription As String, ByVal v_lChildStructureTreeId As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                'parent_structure_tree_id
                m_lReturn = .Parameters.Add(sName:="parent_structure_tree_id", vValue:=CStr(v_lParentStructureTreeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for parent_structure_tree_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGeneralLedgerGroup")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'description
                m_lReturn = .Parameters.Add(sName:="description", vValue:=v_sDescription, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for description", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGeneralLedgerGroup")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'parent_structure_tree_id
                m_lReturn = .Parameters.Add(sName:="child_structure_tree_id", vValue:=CStr(v_lChildStructureTreeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for child_structure_tree_id", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGeneralLedgerGroup")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACAddGeneralLedgerGroupSQL, sSQLName:=ACAddGeneralLedgerGroupName, bStoredProcedure:=ACAddGeneralLedgerGroupStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACAddGeneralLedgerGroupSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGeneralLedgerGroup")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: BuildDetailDataArray
    '
    ' Description:
    '
    ' History: 20/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function BuildDetailDataArray(ByVal v_lDocumentIdFrom As Integer, ByVal v_lDocumentIdTo As Integer, ByRef r_vDetailData As Array) As Integer 

        Dim result As Integer = 0 
        

            Dim vGroupArray, vAccountArray, vDetailDataValues As Object 
            Dim lUboundAccount, lUboundGroup, lUboundSum, lRowSum As Integer 

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                'document_id_from
                m_lReturn = .Parameters.Add(sName:="document_id_from", vValue:=CStr(v_lDocumentIdFrom), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for document_id_from", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'document_to_id
                m_lReturn = .Parameters.Add(sName:="document_id_to", vValue:=CStr(v_lDocumentIdTo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for document_id_to", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Get an array of the group accounts
                m_lReturn = .SQLSelect(sSQL:=ACSelectGeneralLedgerGroupSQL, sSQLName:=ACSelectGeneralLedgerGroupName, bStoredProcedure:=ACSelectGeneralLedgerGroupStored, vResultArray:=vGroupArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACSelectGeneralLedgerGroupSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get an array of the other accounts
                m_lReturn = .SQLSelect(sSQL:=ACSelectGeneralLedgerSQL, sSQLName:=ACSelectGeneralLedgerName, bStoredProcedure:=ACSelectGeneralLedgerStored, vResultArray:=vAccountArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACSelectGeneralLedgerSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            If Not Information.IsArray(vAccountArray) And Not Information.IsArray(vGroupArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            If Not Information.IsArray(vAccountArray) Then
                'no Account array so just return the  Group array

                lUboundGroup = vGroupArray.GetUpperBound(1)

                ReDim vDetailDataValues(ACDetailDataSize, lUboundGroup) 

                For lRow As Integer = 0 To lUboundGroup
                    For lCol As Integer = 0 To ACDetailDataSize


                        vDetailDataValues(lCol, lRow) = CStr(vGroupArray(lCol, lRow)).Trim()
                    Next lCol
                Next lRow
                'add the hub specific column data


                m_lReturn = AddHUBColumnsToDetailArray(v_sStatusCode:=gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS, v_sStatusMsg:=gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS, r_vResultArray:=vDetailDataValues, v_sUsername:=m_sUsername)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add hub specific data to the detail array", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = AddResultArrayToDetailArray(r_vDetailData, vDetailDataValues, v_sUsername:=m_sUsername)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add the result array to the detail array", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            'add the account array to the detaildata

            lUboundAccount = vAccountArray.GetUpperBound(1)

            ReDim vDetailDataValues(ACDetailDataSize, lUboundAccount) 

            For lRow As Integer = 0 To lUboundAccount
                For lCol As Integer = 0 To ACDetailDataSize


                    vDetailDataValues(lCol, lRow) = CStr(vAccountArray(lCol, lRow)).Trim()
                Next lCol
            Next lRow

            If Not Information.IsArray(vGroupArray) Then
                'add the hub column data

                m_lReturn = AddHUBColumnsToDetailArray(v_sStatusCode:=gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS, v_sStatusMsg:=gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS, r_vResultArray:=vDetailDataValues, v_sUsername:=m_sUsername)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add hub specific data to the detail array", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = AddResultArrayToDetailArray(r_vDetailData, vDetailDataValues, v_sUsername:=m_sUsername)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add the result array to the detail array", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result

            End If

            'Add the grouped accounts to the normal accounts

            lUboundGroup = vGroupArray.GetUpperBound(1)
            lUboundSum = lUboundAccount + lUboundGroup + 1

            ReDim Preserve vDetailDataValues(ACDetailDataSize, lUboundSum) 

            lRowSum = lUboundAccount + 1
            For lRowGroup As Integer = 0 To lUboundGroup
                For lCol As Integer = 0 To ACDetailDataSize


                    vDetailDataValues(lCol, lRowSum) = CStr(vGroupArray(lCol, lRowGroup)).Trim()
                Next lCol
                lRowSum += 1
            Next lRowGroup


            m_lReturn = AddHUBColumnsToDetailArray(v_sStatusCode:=gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS, v_sStatusMsg:=gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS, r_vResultArray:=vDetailDataValues, v_sUsername:=m_sUsername)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add hub specific data to the detail array", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = AddResultArrayToDetailArray(r_vDetailData, vDetailDataValues, v_sUsername:=m_sUsername)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add the result array to the detail array", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildDetailDataArray")
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function
	
	'#Region " Private Methods "
	Public Sub New()
		MyBase.New()
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Class_Initialize
		' PURPOSE: Class initialisation
		' AUTHOR: Steve Watton
		' DATE: 02/04/2002, 12:08:23
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
End Class

