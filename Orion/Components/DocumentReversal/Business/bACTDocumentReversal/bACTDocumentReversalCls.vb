Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Document Reversal Business
    '
    ' Date: ??/??/????
    '
    ' Description: ???
    '
    ' Edit History:
    ' DJM 27/05/2002 : The reversed transaction should use the operator id
    '                  of the person reversing the transaction rather than
    '                  the person who created the original transaction.
    ' ??? ??/??/???? : Created
    ' RAW 13/01/2003 : PS187 : replaced spu_ACT_Do_InsurerPayments with spu_ACT_Do_InsurerPayments_All
    ' RAW 12/03/2003 : ISS2893 : ReverseAllocation parameters referenced by name
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 10/12/2003
#Region "Private Variables"
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "Business"

    ' Return value
    Private m_lReturn As Integer

    ' Instance of database object
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_oDocument As bACTDocument.Form
    Private m_oTransDetail As bACTTransdetail.Form
    Private m_oAutoNumber As Object
    Private m_oAllocationManual As bACTAllocationManual.Business
    Private m_oPeriod As bACTPeriod.Form
    Private m_oInsuranceFile As bSIRInsuranceFile.Business
    Private m_oInsurerPayment As bACTInsurerPaymentSFU.Business

    'Document Properties
    Private m_lDocumentID As Integer
    Private m_lBatchId As Integer
    Private m_iCompanyID As Integer
    Private m_iPostingstatusID As Integer
    Private m_iDocumentTypeID As Integer
    Private m_lAuditSetId As Integer
    Private m_sDocumentRef As String = ""
    Private m_dtDocumentDate As Date
    Private m_dtCreatedDate As Date
    Private m_dtAuthorisedDate As Date
    Private m_sComment As String = ""
    Private m_lWriteOffReasonID As Integer
    Private m_lSubBranchId As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sReason As String = ""

    'Transaction Properties
    Private m_lTransdetailID As Integer
    Private m_lAccountID As Integer
    Private m_iTransPostingstatusID As Integer
    Private m_iTransCompanyID As Integer
    Private m_lPeriodID As Integer
    Private m_lTransDocumentID As Integer
    Private m_iDocumentSequence As Integer
    Private m_dtAccountingDate As Date
    Private m_cAmount As Decimal
    Private m_dBaseAmountUnRounded As Double
    Private m_iFullyMatched As Integer
    Private m_cCurrencyAmount As Decimal
    Private m_dCCyAmountUnrounded As Double
    Private m_dCurrencyBaseXRate As Double
    Private m_lEuro As Integer
    Private m_cEuroAmount As Decimal
    Private m_dEuroBaseXrate As Double
    Private m_dEuroCCyXrate As Double
    Private m_sTransComment As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_iOperatorID As Integer
    Private m_sPurchaseOrderNo As String = ""
    Private m_sPurchaseInvoiceNo As String = ""
    Private m_sDepartment As String = ""
    Private m_sSpare As String = ""
    Private m_dtRefDate As Date
    Private m_cRefAmount As Decimal
    Private m_dRefQuantity As Double
    Private m_sRefUnits As String = ""
    Private m_dOSBaseAmount As Double
    Private m_dOSCurrencyAmount As Double
    Private m_lTransdetailTypeID As Integer
    Private m_bIsClonedReversal As Boolean
    Private m_nInstalmentAccountID As Integer
    Private m_bRecallInstalmentFromInstalmentMaint As Boolean

    'eck200302
    Private m_vDocumentIds() As Object


    Private m_vTransArray(,) As Object
    '040400
    Private m_vAllocatedArray() As Object
    Private m_vAllocatedToArray() As Object
    'DJM 06/11/2002
    Private m_sBrokerage As String = ""
    Private m_sCommPay As String = ""
    Private m_vAssociatedDocuments(,) As Object

    'DC310105 : Process Reversing Introducer Transactions
    Private m_bIntroducer As Boolean
    Private m_vAssocIntroDocuments(,) As Object

    Private m_vAssocDirectToInsurer(,) As Object

    Private m_vUnderwriting As String = ""

    Private m_bIsCashlistItemReversal As Boolean
    Private m_lReversalTransDetailId As Integer

#End Region

#Region "Public Properties"
    ' Product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    Public Property DocumentId() As Integer
        Get
            Dim vDocumentID As Object = Nothing
            If m_lDocumentID <> 0 Then
                Return m_lDocumentID
            Else

                m_lReturn = GetDocumentFromTransaction(CInt(vDocumentID))
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    m_lDocumentID = CInt(vDocumentID)
                    Return m_lDocumentID
                Else
                    Return 0
                End If
            End If
        End Get
        Set(ByVal Value As Integer)

            m_lDocumentID = Value
        End Set
    End Property
    Public WriteOnly Property TransDetailId() As Integer
        Set(ByVal Value As Integer)
            m_lTransdetailID = Value
        End Set
    End Property

    'DC310105 : Process Reversing Introducer Transactions
    Public WriteOnly Property Introducer() As Boolean
        Set(ByVal Value As Boolean)
            m_bIntroducer = Value
        End Set
    End Property


    Public Property IsCashlistItemReversal() As Boolean
        Get
            Return m_bIsCashlistItemReversal
        End Get
        Set(ByVal Value As Boolean)
            m_bIsCashlistItemReversal = Value
        End Set
    End Property

    'Recall Reversal

    Public Property ReversalTransDetailId() As Integer
        Get
            Return m_lReversalTransDetailId
        End Get
        Set(ByVal Value As Integer)
            m_lReversalTransDetailId = Value
        End Set
    End Property

    Public Property IsClonedReversal() As Boolean
        Get
            Return m_bIsClonedReversal
        End Get
        Set(value As Boolean)
            m_bIsClonedReversal = value
        End Set
    End Property

    Public Property IsRecallInstalmentFromInstalmentMaint() As Boolean
        Get
            Return m_bRecallInstalmentFromInstalmentMaint
        End Get
        Set(ByVal Value As Boolean)
            m_bRecallInstalmentFromInstalmentMaint = Value
        End Set
    End Property

#End Region


#Region "Public Methods"

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            ' Get an instance of the database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            m_oDocument = New bACTDocument.Form
            m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oTransDetail = New bACTTransdetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oAllocationManual = New bACTAllocationManual.Business
            m_lReturn = m_oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck170800

            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Remove component services

            'DC250204 PN10641 - allow reversal of Future Annual Premium

            m_oInsuranceFile = New bSIRInsuranceFile.Business
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oInsurerPayment = New bACTInsurerPaymentSFU.Business
            m_lReturn = m_oInsurerPayment.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_vUnderwriting)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 16/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            ' Do nothing. Not important

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                If m_oDocument IsNot Nothing Then
                    m_oDocument.Dispose()
                    m_oDocument = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oAllocationManual IsNot Nothing Then
                    m_oAllocationManual.Dispose()
                    m_oAllocationManual = Nothing
                End If
                If m_oPeriod IsNot Nothing Then
                    m_oPeriod.Dispose()
                    m_oPeriod = Nothing
                End If
                If m_oInsuranceFile IsNot Nothing Then
                    m_oInsuranceFile.Dispose()
                    m_oInsuranceFile = Nothing
                End If
                If m_oInsurerPayment IsNot Nothing Then
                    m_oInsurerPayment.Dispose()
                    m_oInsurerPayment = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: SetStatus
    '
    ' Description:
    '
    ' History: 02/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SearchDetails
    '
    ' Description:
    '
    ' History: 02/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SearchDetails(ByVal v_lMarkedStatus As Integer, ByVal v_lMonth As Integer) As Integer
        Return SearchDetails(v_lMarkedStatus:=v_lMarkedStatus, v_lMonth:=v_lMonth, v_vAccountID:=Nothing, v_vDateTo:="", lNumberOfRecords:=0, r_vResultArray:=Nothing)
    End Function

    Public Function SearchDetails(ByVal v_lMarkedStatus As Integer, ByVal v_lMonth As Integer, ByVal v_vAccountID As Object, ByVal v_vDateTo As String, ByRef lNumberOfRecords As Integer, r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add parameters

            ' RAW 13/01/2003 : added
            m_lReturn = m_oDatabase.Parameters.Add(sName:="v_sLedgerCode", vValue:="Insurer", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            ' RAW 13/01/2003 : end

            ' RAW 13/01/2003 : parameter names changed

            If v_lMarkedStatus <> -1 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="v_iMarkedStatus", vValue:=CStr(v_lMarkedStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no. 85
                'm_lReturn = m_oDatabase.Parameters.Add(sName:="v_iMarkedStatus", vValue:=CStr(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="v_iMarkedStatus", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If v_lMonth <> -1 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="v_iMonth", vValue:=CStr(v_lMonth), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="v_iMonth", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="v_iAccountId", vValue:=CStr(v_vAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If (Not Informations.IsNothing(v_vDateTo)) And (v_vDateTo <> "") Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="v_dtDateTo", vValue:=v_vDateTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="v_dtDateTo", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            ' RAW 13/01/2003 : end

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsurerPaymentsSQL, sSQLName:=ACInsurerPaymentsName, bStoredProcedure:=ACInsurerPaymentsStored, lNumberRecords:=lNumberOfRecords, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check there's some results
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetFAPReversalInfo
    '
    ' Description: PN10641 : To get future annual premium for reversal
    '
    ' History: 02/09/1999 DC 250204
    '
    ' ***************************************************************** '

    Public Function GetFAPReversalInfo(ByVal v_lDocumentId As Integer) As Integer
        Return GetFAPReversalInfo(v_lDocumentId:=v_lDocumentId, r_vResultArray:=Nothing)
    End Function

    Public Function GetFAPReversalInfo(ByVal v_lDocumentId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentId", vValue:=CStr(v_lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFAPReversalInfoSQL, sSQLName:=ACGetFAPReversalInfoName, bStoredProcedure:=ACGetFAPReversalInfoStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check there's some results
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get FAP Reversal Info Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFAPReversalInfo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckMatch
    '
    ' Description:
    '
    ' History: 21/08/00 ECK  - Created.
    '
    ' ***************************************************************** '
    Public Function CheckMatch(ByVal v_lTransDetailID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "transdetail_id", v_lTransDetailID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "first_document_id", m_vDocumentIds(0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ISNULL(SUM(tm.base_match_amount),0)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM transdetail td" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN transdetail_type tt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON tt.transdetail_type_id = td.transdetail_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN transmatch tm" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON tm.transdetail_id = td.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND tm.is_reversed IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND tm.allocationdetail_id IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE td.transdetail_id = {transdetail_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ISNULL(tt.code,'') NOT IN ('BROK','BROK ADJ','FEE','CFEE','COMMPAY')" & Strings.ChrW(13) & Strings.ChrW(10)
            'This allocation is not linking the main transaction with a direct to insurer transaction
            sSQL = sSQL & "AND NOT EXISTS" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        FROM document d" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transdetail td" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON td.document_id = d.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND td.document_sequence = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN account a" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON a.account_id = td.account_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN ledger l" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON l.ledger_id = a.ledger_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND l.ledger_short_name IN ('SA', 'UB')" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transmatch tm" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON tm.transdetail_id = td.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND tm.is_reversed IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transmatch tm2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON tm2.match_id = tm.match_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND tm2.transdetail_id <> tm.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transdetail td2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON td2.transdetail_id = tm2.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN document d2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON d2.document_id = td2.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN documenttype dt2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON dt2.documenttype_id = d2.documenttype_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        WHERE td.transdetail_id = {transdetail_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND dt2.code IN ('DID', 'DIC')" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                    SUM(1)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                FROM transmatch" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                WHERE match_id = tm.match_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ) = 2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            'This allocation is not linking a direct to insurer transaction with the transaction originally selected
            sSQL = sSQL & "AND NOT EXISTS" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        FROM document d" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transdetail td" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON td.document_id = d.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND td.document_sequence = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN account a" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON a.account_id = td.account_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN ledger l" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON l.ledger_id = a.ledger_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND l.ledger_short_name IN ('SA', 'UB')" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transmatch tm" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON tm.transdetail_id = td.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND tm.is_reversed IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transmatch tm2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON tm2.match_id = tm.match_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            AND tm2.transdetail_id <> tm.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN transdetail td2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON td2.transdetail_id = tm2.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN document d2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON d2.document_id = td2.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN documenttype dt2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON dt2.documenttype_id = d2.documenttype_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        WHERE d.document_id = {first_document_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND td2.transdetail_id = {transdetail_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND dt2.code IN ('DID', 'DIC')" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                    SUM(1)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                FROM transmatch" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "                WHERE match_id = tm.match_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ) = 2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    )"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckTransMAtch", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check there's some results

            If CDbl(vResultArray(0, 0)) = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMatch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'r_vCreditTransDetailID & r_vDebitTransDetailID return the transdetail ID's of balancing transaction
    Public Function Start(Optional ByRef r_vCreditTransDetailID() As Object = Nothing, Optional ByRef r_vDebitTransDetailID() As Object = Nothing, Optional ByRef r_sFailureReason As String = "", Optional ByVal v_bDisableTransactions As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentsForReversal"

        Dim vResultArray(,) As Object = Nothing
        Dim lCashListItemID, lOppositeTransDetailID As Integer
        Dim oAllocationPost As bACTAllocationPost.Automated = Nothing
        Dim vCheckResults As Object = Nothing
        Dim sOptionValue As String = ""
        ' Dim bCreditControlEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.SQLBeginTrans()

            'Only procede if the transaction selected is a valid one.
            m_lReturn = CheckTransactionForReversal(v_bOnlyCheckForInvalidTransaction:=True, r_vCheckResults:=vCheckResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CheckTransactionForReversal", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
             If Informations.IsArray(vCheckResults) Then
                 If m_sCallingAppName = "iACTFindTransaction" OrElse m_sCallingAppName = "SiriusTransactionService" Then
                    r_sFailureReason = vCheckResults(1, 0)
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    gPMFunctions.RaiseError("CheckTransactionForReversal", "Transaction selected can not be reversed.", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If m_lDocumentID = 0 Then

                'Reverse out any valid allocation
                'Find the cashlisitemID from the transdetailID
                bPMAddParameter.AddParameterLite(m_oDatabase, "transdetail_id", m_lTransdetailID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectCLIIDfromTDIDSQL, sSQLName:=ACSelectCLIIDfromTDIDName, bStoredProcedure:=True, lNumberRecords:=1, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectCLIIDfromTDIDSQL, transdetail_id:=" & m_lTransdetailID, gPMConstants.PMELogLevel.PMLogError)
                End If
                If Informations.IsArray(vResultArray) Then

                    lCashListItemID = CInt(vResultArray(0, 0))
                    'The opposite transdetail from the CashListItem document

                    lOppositeTransDetailID = CInt(vResultArray(1, 0))
                End If

                If m_bRecallInstalmentFromInstalmentMaint Then
                    AddParameterLite(m_oDatabase, "transdetail_id", m_lTransdetailID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(
                        sSQL:=kSelectTransDetailSQL,
                        sSQLName:=kSelectTransDetailName,
                        bStoredProcedure:=True,
                        lNumberRecords:=1,
                        vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectTransDetailSQL, transdetail_id:=" & m_lTransdetailID, gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If Informations.IsArray(vResultArray) Then
                        m_nInstalmentAccountID = ToSafeInteger(vResultArray(1, 0))
                    End If
                End If

                'Create the AllocationPost object
                oAllocationPost = New bACTAllocationPost.Automated()

                'developer guide no. 84
                m_lReturn = oAllocationPost.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oAllocationPost.Initialise", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'If we have a cashlistitem_id then reverse allocate the cashlistitem
                If lCashListItemID <> 0 Then

                    m_lReturn = oAllocationPost.ReverseAllocation(v_lCashListItemID:=lCashListItemID, r_sFailureReason:=r_sFailureReason)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("oAllocationPost.ReverseAllocation", "v_lCashListItemID:=" & lCashListItemID, gPMConstants.PMELogLevel.PMLogError)
                    End If


                    'Change for Payment Maintenance
                    If IsCashlistItemReversal Then

                        m_lReturn = oAllocationPost.ReverseAllocation(v_lTransDetailID:=m_lTransdetailID, r_sFailureReason:=r_sFailureReason)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oAllocationPost.ReverseAllocation", "v_lTransDetailID:=" & lOppositeTransDetailID, gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else
                        'Reverse the opposite Transdetail in the document as well
                        'this handles the situation when either Batch posting is switched on
                        'or the Bank item has been reconciled.
                        m_lReturn = oAllocationPost.ReverseAllocation(v_lTransDetailID:=lOppositeTransDetailID, r_sFailureReason:=r_sFailureReason)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oAllocationPost.ReverseAllocation", "v_lTransDetailID:=" & lOppositeTransDetailID, gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If


                Else
                    'reverse on the transdetail_id (this will be 4 instalment receipts)
                    m_lReturn = oAllocationPost.ReverseAllocation(v_lTransDetailID:=m_lTransdetailID, r_sFailureReason:=r_sFailureReason, v_bDisableTransactions:=v_bDisableTransactions)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("oAllocationPost.ReverseAllocation", "v_lTransDetailID:=" & m_lTransdetailID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

                'Reverse the original allocation between the original document and it's direct to insurer transaction
                If Informations.IsArray(m_vAssocDirectToInsurer) Then
                    For lRow As Integer = 0 To m_vAssocDirectToInsurer.GetUpperBound(1)
                        m_lReturn = oAllocationPost.ReverseAllocation(v_lTransDetailID:=CInt(m_vAssocDirectToInsurer(1, lRow)), r_sFailureReason:=r_sFailureReason, v_bDisableTransactions:=v_bDisableTransactions)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oAllocationPost.ReverseAllocation", "v_lTransDetailID:=" & CInt(m_vAssocDirectToInsurer(1, lRow)), gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next
                End If

            End If



            For lRow As Integer = 0 To m_vDocumentIds.GetUpperBound(0)

                If Not Object.Equals(m_vDocumentIds(lRow), Nothing) Then
                    m_lDocumentID = CInt(m_vDocumentIds(lRow))

                    m_lReturn = GetDocument(vDocumentID:=m_lDocumentID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetDocument", "vDocumentID:=" & m_lDocumentID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = GetTransactions(vDocumentID:=m_lDocumentID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetTransactions", "vDocumentID:=" & m_lDocumentID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If m_bRecallInstalmentFromInstalmentMaint <> True Then
                        m_lReturn = CreateDocument(r_vCreditTransDetailID, r_vDebitTransDetailID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("CreateDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        m_lReturn = AllocateDocument(v_bDisableTransactions:=v_bDisableTransactions)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("AllocateDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If
            Next lRow

            If Informations.IsArray(m_vAssociatedDocuments) Then
                For lRow As Integer = 0 To m_vAssociatedDocuments.GetUpperBound(1)

                    ReDim m_vAllocatedArray(0)
                    ReDim m_vAllocatedToArray(0)

                    m_vAllocatedArray(0) = CStr(m_vAssociatedDocuments(1, lRow)) & "|" & CStr(m_vAssociatedDocuments(2, lRow))
                    m_vAllocatedToArray(0) = CStr(m_vAssociatedDocuments(3, lRow)) & "|" & CStr(m_vAssociatedDocuments(4, lRow))

                    m_lReturn = AllocateDocument(v_bDisableTransactions:=v_bDisableTransactions)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("AllocateDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next lRow

                m_lReturn = RecallReleasedAccountsTransaction(vAssociatedDocuments:=m_vAssociatedDocuments)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RecallReleasedAccountsTransaction", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


            If lCashListItemID > 0 Then

                'Mark the CashListItem as reversed
                bPMAddParameter.AddParameterLite(m_oDatabase, "cashlistitem_id", lCashListItemID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCashListItemMarkReversedSQL, sSQLName:=ACCashListItemMarkReversedName, bStoredProcedure:=ACCashListItemMarkReversedStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACCashListItemMarkReversedSQL, cashlistitem_id:=" & lCashListItemID, gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


            m_oDatabase.SQLCommitTrans()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            m_oDatabase.SQLRollbackTrans()

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oAllocationPost Is Nothing) Then
                oAllocationPost.Dispose()
            End If
            oAllocationPost = Nothing

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function DoPartialReceiptReversal(ByVal v_lReceiptDocumentId As Integer, ByVal v_sDoumentRef As String, ByVal v_dAmount As Double, ByVal v_dCurrencyAmount As Double, ByRef r_sDocumentRef As String) As Integer
        Dim sSpareComment As String = ""
        m_lReturn = GetDocument(v_lReceiptDocumentId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetDocument", "vDocumentID:=" & v_lReceiptDocumentId, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = GetTransactions(v_lReceiptDocumentId, v_dAmount, v_dCurrencyAmount)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetTransactions", "vDocumentID:=" & v_lReceiptDocumentId, gPMConstants.PMELogLevel.PMLogError)
        End If

        Dim r_vCreditTransDetailID As Object = Nothing
        Dim r_vDebitTransDetailID As Object = Nothing

        sSpareComment = "Rev " & v_sDoumentRef.Trim
        m_lReturn = CreateDocument(r_vCreditTransDetailID, r_vDebitTransDetailID, sSpareComment)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ''trans match arrach

        ' m_vAllocatedToArray = ""
        r_sDocumentRef = m_sDocumentRef
        m_lReturn = AllocateDocument(v_bDisableTransactions:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("AllocateDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return m_lReturn
    End Function

    Public Function DoReceiptTransdetail(ByVal v_lReceiptDocumentId As Integer, ByVal v_sReceiptDocumentRef As String, ByVal v_dAmount As Double, ByVal v_dCurrencyAmount As Double, ByRef r_sDocumentRef As String) As Integer

        Dim sSpareComment As String = ""
        m_lReturn = GetDocument(v_lReceiptDocumentId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetDocument", "vDocumentID:=" & v_lReceiptDocumentId, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = GetTransactions(v_lReceiptDocumentId, v_dAmount, v_dCurrencyAmount)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetTransactions", "vDocumentID:=" & v_lReceiptDocumentId, gPMConstants.PMELogLevel.PMLogError)
        End If

        Dim r_vCreditTransDetailID As Object = Nothing
        Dim r_vDebitTransDetailID As Object = Nothing

        sSpareComment = "Ref " & v_sReceiptDocumentRef.Trim
        m_lReturn = CreateDocument(r_vCreditTransDetailID, r_vDebitTransDetailID, sSpareComment, True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateDocument", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        r_sDocumentRef = m_sDocumentRef
        Return m_lReturn
    End Function

#End Region

    ''' <summary>
    ''' GetDocumentFromTransaction
    ''' </summary>
    ''' <param name="vDocumentID"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetDocumentFromTransaction(ByRef vDocumentID As Integer) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Dim sSQL As String = "SELECT document_id FROM transdetail WHERE transdetail_id = " & m_lTransdetailID

        With m_oDatabase
            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentFromTrans", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With
        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        vDocumentID = CInt(vResultArray(0, 0))
        Return result
    End Function

    ''' <summary>
    ''' GetDocumentsFromTransaction
    ''' </summary>
    ''' <param name="vDocumentIds"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDocumentsFromTransaction(ByRef vDocumentIds() As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not m_bIntroducer Then

            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    d.document_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    d.document_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    d.company_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    td.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM document d" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN transdetail td " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON td.document_id = d.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE td.transdetail_id = " & CStr(m_lTransdetailID)

        Else

            'DC310105 : So as to process all related transactions for an introducer transaction
            'get the document_id for the main transaction and take it from there
            sSQL = ""
            sSQL = sSQL & "SELECT "
            sSQL = sSQL & "    d.document_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    d.document_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    d.company_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    td2.transdetail_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM transdetail td " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN document d " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON rtrim(d.document_ref) = rtrim(substring(td.spare, 10, 20)) " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND d.company_id = td.company_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN transdetail td2 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON td2.document_id = d.document_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND td2.document_sequence = 1 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE td.transdetail_id = " & CStr(m_lTransdetailID)

        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentFromTrans", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ReDim vDocumentIds(0)


        vDocumentIds(0) = CInt(vResultArray(0, 0))

        Dim sDocumentRef As String = CStr(vResultArray(1, 0))

        m_lTransdetailID = CInt(vResultArray(3, 0))

        'Find commission transactions that are associated with this document

        m_lReturn = GetAssociatedDocuments(lDocumentId:=CInt(vResultArray(0, 0)), lTransdetailId:=m_lTransdetailID, vAssociatedDocuments:=m_vAssociatedDocuments)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'If we have found some associated documents then add them to the list of documents to reverse
        If Informations.IsArray(m_vAssociatedDocuments) Then
            ReDim Preserve vDocumentIds(m_vAssociatedDocuments.GetUpperBound(1) + vDocumentIds.GetUpperBound(0) + 1)
            For lRow As Integer = 0 To m_vAssociatedDocuments.GetUpperBound(1)

                vDocumentIds(lRow + 1) = CInt(m_vAssociatedDocuments(0, lRow))
            Next lRow
        End If

        'Find introducer transactions that are associated with this document

        m_lReturn = GetAssocIntroDocuments(lDocumentId:=CInt(vResultArray(0, 0)), vAssocIntroDocuments:=m_vAssocIntroDocuments)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(m_vAssocIntroDocuments) Then
            ReDim Preserve vDocumentIds(m_vAssocIntroDocuments.GetUpperBound(1) + vDocumentIds.GetUpperBound(0) + 1)
            For lRow As Integer = 0 To m_vAssocIntroDocuments.GetUpperBound(1)

                vDocumentIds(lRow + 1) = CInt(m_vAssocIntroDocuments(0, lRow))
            Next lRow
        End If

        'Find direct to insurer transactions that are linked to this document

        m_lReturn = GetAssocDirectToInsurerDocuments(v_lDocumentId:=CInt(vResultArray(0, 0)), r_vDocuments:=m_vAssocDirectToInsurer)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(m_vAssocDirectToInsurer) Then
            ReDim Preserve vDocumentIds(m_vAssocDirectToInsurer.GetUpperBound(1) + vDocumentIds.GetUpperBound(0) + 1)
            For lRow As Integer = 0 To m_vAssocDirectToInsurer.GetUpperBound(1)

                vDocumentIds(lRow + 1) = CInt(m_vAssocDirectToInsurer(0, lRow))
            Next lRow
        End If

        Return result
    End Function

    ''' <summary>
    ''' GetDocument
    ''' </summary>
    ''' <param name="vDocumentID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDocument(ByVal vDocumentID As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If vDocumentID = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDocument.GetDetails(vDocumentID:=vDocumentID)

        If Not (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            While m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                'AR20041001 - PN15316 Retrieve SubBranchID property of the document

                m_lReturn = m_oDocument.GetNext(vDocumentID:=m_lDocumentID, vBatchID:=m_lBatchId, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumentTypeID, vAuditsetID:=m_lAuditSetId, vDocumentRef:=m_sDocumentRef, vDocumentDate:=m_dtDocumentDate, vCreatedDate:=m_dtCreatedDate, vAuthorisedDate:=m_dtAuthorisedDate, vComment:=m_sComment, vWriteOffReasonID:=m_lWriteOffReasonID, vSubBranchID:=m_lSubBranchId, vInsuranceFileCnt:=m_lInsuranceFileCnt, vReason:=m_sReason)

                If m_lReturn = gPMConstants.PMEReturnCode.PMEOF Then
                    Return result
                End If
            End While
        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Get Document", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


        Return result
    End Function

    ''' <summary>
    ''' GetTransactions
    ''' </summary>
    ''' <param name="vDocumentID"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetTransactions(ByVal vDocumentID As Object, Optional ByVal v_dAmount As Double = 0,
                                    Optional ByVal v_dCurrencyAmount As Double = 0) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer

        result = gPMConstants.PMEReturnCode.PMTrue
        'add the line
        m_oTransDetail.m_oDatabase = m_oDatabase
        m_lReturn = m_oTransDetail.GetDetails(vDocumentID:=vDocumentID)

        If Not (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            ReDim m_vTransArray(ACIMAXCOLUMN, 0)
            'EK 040400
            ' Retrieve all of the details from the business object.
            'EK 100100 Added extra properties

            While m_oTransDetail.GetNext(vTransdetailID:=m_lTransdetailID, vAccountID:=m_lAccountID, vPostingstatusID:=m_iTransPostingstatusID, vCompanyID:=m_iTransCompanyID, vCurrencyID:=m_iCurrencyID, vPeriodID:=m_lPeriodID, vDocumentID:=m_lTransDocumentID, vDocumentSequence:=m_iDocumentSequence, vAccountingDate:=m_dtAccountingDate, vAmount:=m_cAmount, vBaseAmountUnrounded:=m_dBaseAmountUnRounded, vFullyMatched:=m_iFullyMatched, vCurrencyAmount:=m_cCurrencyAmount, vCurrencyAmountUnrounded:=m_dCCyAmountUnrounded, vCurrencyBaseXrate:=m_dCurrencyBaseXRate, vEuroCurrencyId:=m_lEuro, vEuroAmount:=m_cEuroAmount, vEuroBaseXRate:=m_dEuroBaseXrate, vEuroCcyXrate:=m_dEuroCCyXrate, vComment:=m_sTransComment, vInsuranceRef:=m_sInsuranceRef, vOperatorID:=m_iOperatorID, vPurchaseOrderNo:=m_sPurchaseOrderNo, vPurchaseInvoiceNo:=m_sPurchaseInvoiceNo, vDepartment:=m_sDepartment, vSpare:=m_sSpare, vRefDate:=m_dtRefDate, vRefAmount:=m_cRefAmount, vRefQuantity:=m_dRefQuantity, vRefUnits:=m_sRefUnits, vOSBaseAmount:=m_dOSBaseAmount, vOSCurrencyAmount:=m_dOSCurrencyAmount, vTransdetailTypeID:=m_lTransdetailTypeID) = gPMConstants.PMEReturnCode.PMTrue
                Dim sDocumentType As String = " "
                m_lReturn = GetDocumentType(nDocumentId:=m_lTransDocumentID, r_sDocumentType:=sDocumentType)


                If (m_bRecallInstalmentFromInstalmentMaint And m_nInstalmentAccountID = m_lAccountID And (Trim(sDocumentType)).ToUpper = "INC") Then
                    'Do nothing
                Else



                    lRow = m_vTransArray.GetUpperBound(1)
                    m_vTransArray(ACITransdetailID, lRow) = m_lTransdetailID
                    m_vTransArray(ACIAccountID, lRow) = m_lAccountID
                    m_vTransArray(ACIPostingstatusID, lRow) = m_iTransPostingstatusID
                    m_vTransArray(ACICompanyID, lRow) = m_iTransCompanyID
                    m_vTransArray(ACICurrencyID, lRow) = m_iCurrencyID
                    m_vTransArray(ACIPeriodID, lRow) = m_lPeriodID
                    m_vTransArray(ACIDocumentID, lRow) = m_lTransDocumentID
                    m_vTransArray(ACIDocumentSequence, lRow) = m_iDocumentSequence
                    m_vTransArray(ACIAccountingDate, lRow) = m_dtAccountingDate
                    m_vTransArray(ACIAmount, lRow) = m_cAmount
                    m_vTransArray(ACIBaseAmountUnRounded, lRow) = m_dBaseAmountUnRounded
                    m_vTransArray(ACIFullyMatched, lRow) = m_iFullyMatched
                    m_vTransArray(ACICurrencyAmount, lRow) = m_cCurrencyAmount
                    m_vTransArray(ACICCyAmountUnRounded, lRow) = m_dCCyAmountUnrounded

                    Dim bIsNegative As Boolean = False

                    If (m_cAmount * v_dAmount) < 0 Then
                        bIsNegative = True
                    End If

                    If v_dAmount <> 0 Then
                        If Not bIsNegative Then
                            m_vTransArray(ACIAmount, lRow) = Math.Abs(v_dAmount)
                            m_vTransArray(ACIBaseAmountUnRounded, lRow) = Math.Abs(v_dAmount)

                        Else
                            m_vTransArray(ACIAmount, lRow) = Math.Abs(v_dAmount) * -1
                            m_vTransArray(ACIBaseAmountUnRounded, lRow) = Math.Abs(v_dAmount) * -1
                        End If

                    End If
                    If v_dCurrencyAmount <> 0 Then
                        If Not bIsNegative Then
                            m_vTransArray(ACICurrencyAmount, lRow) = Math.Abs(v_dCurrencyAmount)
                            m_vTransArray(ACICCyAmountUnRounded, lRow) = Math.Abs(v_dCurrencyAmount)
                        Else
                            m_vTransArray(ACICurrencyAmount, lRow) = Math.Abs(v_dCurrencyAmount) * -1
                            m_vTransArray(ACICCyAmountUnRounded, lRow) = Math.Abs(v_dCurrencyAmount) * -1
                        End If

                    End If


                    m_vTransArray(ACICurrencyBaseXrate, lRow) = m_dCurrencyBaseXRate
                    m_vTransArray(ACIEuro, lRow) = m_lEuro
                    m_vTransArray(ACIEuroAmount, lRow) = m_cEuroAmount
                    m_vTransArray(ACIEuroBaseXrate, lRow) = m_dEuroBaseXrate
                    m_vTransArray(ACIEuroCCyXrate, lRow) = m_dEuroCCyXrate
                    m_vTransArray(ACIComment, lRow) = m_sTransComment
                    m_vTransArray(ACIInsuranceRef, lRow) = m_sInsuranceRef
                    m_vTransArray(ACIOperatorID, lRow) = m_iOperatorID
                    m_vTransArray(ACIPurchaseOrderNo, lRow) = m_sPurchaseOrderNo
                    m_vTransArray(ACIPurchaseInvoiceNo, lRow) = m_sPurchaseInvoiceNo
                    m_vTransArray(ACIDepartment, lRow) = m_sDepartment
                    m_vTransArray(ACISpare, lRow) = m_sSpare
                    m_vTransArray(ACIRefDate, lRow) = m_dtRefDate
                    m_vTransArray(ACIRefAmount, lRow) = m_cRefAmount
                    m_vTransArray(ACIRefQuantity, lRow) = m_dRefQuantity
                    m_vTransArray(ACIRefUnits, lRow) = m_sRefUnits
                    m_vTransArray(ACIOSBaseAmount, lRow) = m_dEuroCCyXrate
                    m_vTransArray(ACIOSCurrencyAmount, lRow) = m_dEuroCCyXrate
                    m_vTransArray(ACITransdetailTypeID, lRow) = m_lTransdetailTypeID

                    ' Increment the data array.
                    ReDim Preserve m_vTransArray(m_vTransArray.GetUpperBound(0), m_vTransArray.GetUpperBound(1) + 1)
                    'EK040400
                    If lRow = 0 Then
                        ReDim m_vAllocatedArray(lRow)
                    Else
                        ReDim Preserve m_vAllocatedArray(lRow)
                    End If
                    'eck090800 changes to keep in line with manual allocation part payment
                    'DC241103 PN8468 'was using wrong amount
                    m_vAllocatedArray(lRow) = CStr(m_lTransdetailID) & "|" & CStr(m_cAmount) 'm_cCurrencyAmount


                    If v_dAmount <> 0 Then
                        If Not bIsNegative Then

                            m_vAllocatedArray(lRow) = CStr(m_lTransdetailID) & "|" & CStr(Math.Abs(v_dAmount))
                        Else

                            m_vAllocatedArray(lRow) = CStr(m_lTransdetailID) & "|" & CStr(Math.Abs(v_dAmount) * -1)
                        End If

                    End If

                End If
            End While

            ReDim Preserve m_vTransArray(m_vTransArray.GetUpperBound(0), m_vTransArray.GetUpperBound(1) - 1)
            If Not (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return result
            End If
        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Get Document", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


        Return result
    End Function

    ''' <summary>
    ''' CreateDocument
    ''' </summary>
    ''' <param name="r_vCreditTransDetailID"></param>
    ''' <param name="r_vDebitTransDetailID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateDocument(Optional ByRef r_vCreditTransDetailID() As Object = Nothing, Optional ByRef r_vDebitTransDetailID() As Object = Nothing,
                                    Optional ByVal v_sSpareComment As String = "", Optional ByVal bCalledFromAgenReconReversal As Boolean = False) As Integer

        Dim nResult As Integer
        Dim sGroupCode As String = ""
        Dim sRangeCode As String = ""
        Dim dtTransDate As Date
        Dim nTransdetailId As Integer
        Dim iPeriodEndComplete As Integer
        Dim oPeriodEndComplete As Object = Nothing
        ' Dim dtPeriodStartDate As Date
        Dim dtAccountingDate As Date
        Dim oPeriodDetailsArray(,) As Object = Nothing



        nResult = PMEReturnCode.PMTrue

        'Update Comment On Existing Document
        m_sComment = m_sComment.Trim() & " Reversed"

        nResult = m_oDocument.EditUpdate(1, vDocumentID:=m_lDocumentID, vComment:=m_sComment)

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
            Return nResult
        End If

        nResult = m_oDocument.Update()

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
            Return nResult
        End If

        m_sComment = "Reversal of Document " & m_sDocumentRef
        If m_bIsClonedReversal Then
            m_iDocumentTypeID = kClonedReversedDocumentTypeId
        End If
        nResult = GetAutoNumValues(iDocumenttypeID:=m_iDocumentTypeID, sGroupCode:=sGroupCode, sRangeCode:=sRangeCode)

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
            Return nResult
        End If
        '
        'eck180500 CompanyID parameter
        'Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
        nResult = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iCompanyID, r_sDocumentRef:=m_sDocumentRef)
        'WPR78_Unique_Document_Reference_Number)

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
            Return nResult
        End If
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        m_sDocumentRef = sRangeCode & m_sDocumentRef

        dtTransDate = DateTime.Today

        nResult = m_oDocument.DirectAdd(vDocumentID:=m_lDocumentID, vCompanyID:=m_iCompanyID, vPostingstatusID:=m_iPostingstatusID, vDocumenttypeID:=m_iDocumentTypeID, vAuditsetID:=m_lAuditSetId, vBatchID:=m_lBatchId, vDocumentRef:=m_sDocumentRef, vDocumentDate:=dtTransDate, vCreatedDate:=DateTime.Today, vAuthorisedDate:=m_dtAuthorisedDate, vComment:=m_sComment, vWriteOffReasonID:=m_lWriteOffReasonID, vSubBranchID:=m_lSubBranchId, vInsuranceFileCnt:=m_lInsuranceFileCnt, vReason:=m_sReason)
        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
            Return nResult
        End If

        For iRow As Integer = 0 To m_vTransArray.GetUpperBound(1)
            m_lAccountID = CInt(m_vTransArray(ACIAccountID, iRow))
            m_iTransPostingstatusID = CInt(m_vTransArray(ACIPostingstatusID, iRow))
            m_iTransCompanyID = CInt(m_vTransArray(ACICompanyID, iRow))
            m_iCurrencyID = CInt(m_vTransArray(ACICurrencyID, iRow))
            m_lPeriodID = CInt(m_vTransArray(ACIPeriodID, iRow))
            m_lTransDocumentID = m_lDocumentID
            m_iDocumentSequence = CInt(m_vTransArray(ACIDocumentSequence, iRow))
            ' m_dtAccountingDate = CDate(m_vTransArray(ACIAccountingDate, lRow))
            m_dtAccountingDate = DateTime.Today
            m_cAmount = CDbl(m_vTransArray(ACIAmount, iRow)) * -1
            m_dBaseAmountUnRounded = CDbl(m_vTransArray(ACIBaseAmountUnRounded, iRow)) * -1
            m_iFullyMatched = CInt(m_vTransArray(ACIFullyMatched, iRow))
            m_cCurrencyAmount = CDbl(m_vTransArray(ACICurrencyAmount, iRow)) * -1
            m_dCCyAmountUnrounded = CDbl(m_vTransArray(ACICCyAmountUnRounded, iRow)) * -1
            m_dCurrencyBaseXRate = CDbl(m_vTransArray(ACICurrencyBaseXrate, iRow))
            m_lEuro = CInt(m_vTransArray(ACIEuro, iRow))
            m_cEuroAmount = CDbl(m_vTransArray(ACIEuroAmount, iRow)) * -1
            m_dEuroBaseXrate = CDbl(m_vTransArray(ACIEuroBaseXrate, iRow))
            m_dEuroCCyXrate = CDbl(m_vTransArray(ACIEuroCCyXrate, iRow))
            m_sTransComment = CStr(m_vTransArray(ACIComment, iRow))
            m_sInsuranceRef = CStr(m_vTransArray(ACIInsuranceRef, iRow))
            'DJM 27/05/2002 : The reversed transaction should use the operator id
            '                 of the person reversing the transaction rather than
            '                 the person who created the original transaction.

            m_iOperatorID = m_iUserID
            m_sPurchaseOrderNo = CStr(m_vTransArray(ACIPurchaseOrderNo, iRow))
            m_sPurchaseInvoiceNo = CStr(m_vTransArray(ACIPurchaseInvoiceNo, iRow))
            m_sDepartment = CStr(m_vTransArray(ACIDepartment, iRow))

            'DC310105 : Process Reversing Introducer Transactions
            If m_sSpare.StartsWith("INT COMM") Or m_sSpare.StartsWith("Reversal INT COMM") Then
                m_sSpare = "Revsl IC " & CStr(m_vTransArray(ACISpare, iRow)).Substring(9, Math.Min(CStr(m_vTransArray(ACISpare, iRow)).Length, 11))
            ElseIf m_sSpare.StartsWith("INT ADJ ") Or m_sSpare.StartsWith("Reversal INT ADJ ") Then
                m_sSpare = "Revl ICA" & CStr(m_vTransArray(ACISpare, iRow)).Substring(9, Math.Min(CStr(m_vTransArray(ACISpare, iRow)).Length, 11))
            ElseIf bCalledFromAgenReconReversal Then
                m_sSpare = CStr(m_vTransArray(ACISpare, iRow))
            Else
                m_sSpare = "Reversal " & CStr(m_vTransArray(ACISpare, iRow))
            End If

            'm_dtRefDate = CDate(m_vTransArray(ACIRefDate, lRow))
            m_dtRefDate = DateTime.Today
            m_cRefAmount = CDec(m_vTransArray(ACIRefAmount, iRow))
            m_dRefQuantity = CDbl(m_vTransArray(ACIRefQuantity, iRow))
            m_sRefUnits = CStr(m_vTransArray(ACIRefUnits, iRow))
            m_lTransdetailTypeID = CInt(m_vTransArray(ACITransdetailTypeID, iRow))

            nResult = m_oTransDetail.GetDetails(vTransdetailID:=m_vTransArray(ACITransdetailID, iRow))

            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                Return nResult
            End If

            While m_oTransDetail.GetNext(vTransdetailID:=m_lTransdetailID, vSpare:=m_sSpare) = PMEReturnCode.PMTrue
                'eck200302
                'DC310105 : Process Reversing Introducer Transactions
                If m_sSpare.StartsWith("COMM PAY") Then
                    m_sSpare = "Rever CP " & m_sSpare.Substring(m_sSpare.Length - 11)
                ElseIf m_sSpare.StartsWith("INT COMM") Then
                    m_sSpare = "Revsd IC " & m_sSpare.Substring(9, Math.Min(m_sSpare.Length, 11))
                ElseIf m_sSpare.StartsWith("INT ADJ ") Then
                    m_sSpare = "Revd ICA " & m_sSpare.Substring(9, Math.Min(m_sSpare.Length, 11))
                ElseIf bCalledFromAgenReconReversal Then
                    m_sSpare = CStr(m_vTransArray(ACISpare, iRow))
                Else
                    m_sSpare = "Reversed " & m_sSpare.Trim()
                End If

                nResult = m_oTransDetail.EditUpdate(1, vSpare:=m_sSpare)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                    Return nResult
                End If

                nResult = m_oTransDetail.Update()
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                    Return nResult
                End If
            End While
            'eck070700 Post with todays date
            'eck170800 And Post with todays Period ID !
            'eck050900 Period for client ledger
            'Getting details of the periods...

            nResult = m_oPeriod.GetDetails(m_lPeriodID)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                Return nResult
            End If

            'Checking whether the period is closed or not ...

            nResult = m_oPeriod.GetNext(vPeriodEndComplete:=oPeriodEndComplete)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                Return nResult
            End If

            iPeriodEndComplete = NullToInteger(oPeriodEndComplete)

            If iPeriodEndComplete = 0 Then 'Period is still open
                dtAccountingDate = m_dtAccountingDate
            Else
                'Period is closed then get the current open period

                nResult = m_oPeriod.GetCurrentPeriodDetails(r_vDetails:=oPeriodDetailsArray)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                    Return nResult
                End If

                If Informations.IsArray(oPeriodDetailsArray) Then

                    m_lPeriodID = CInt(oPeriodDetailsArray(ACIPDPeriodID, 0)) 'Saving in next open period
                    'Put in that period and use start date of the period
                    'Extracting the period start date of the current period

                    'm_lReturn = m_oPeriod.GetFirstDayOfPeriod(m_lPeriodID, dtPeriodStartDate)
                    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '    result = False
                    '    GoTo Err_CreateDocument
                    '    Return result
                    'End If
                    'dtAccountingDate = dtPeriodStartDate 'First date of the period
                    dtAccountingDate = DateTime.Today
                Else
                    'Details of current period not found
                    bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Current Period details not found", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocument")
                    RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                    Return nResult
                End If

            End If

            ' 2004-08-25 RAG - Fix for CMIB MIS Extract
            ' Set Spare column to "Reversal COMM" etc, rather than just "Reversal"
            ' So that meaningful reversal data can be extracted
            'DC070205 : PN18567 : set correctly for introducer trans
            If CStr(m_vTransArray(ACISpare, iRow)).StartsWith("INT COMM") Then
                m_sSpare = "Revsl IC " & CStr(m_vTransArray(ACISpare, iRow)).Substring(9, Math.Min(CStr(m_vTransArray(ACISpare, iRow)).Length, 11))
            ElseIf CStr(m_vTransArray(ACISpare, iRow)).StartsWith("INT ADJ ") Then
                m_sSpare = "Revl ICA " & CStr(m_vTransArray(ACISpare, iRow)).Substring(9, Math.Min(CStr(m_vTransArray(ACISpare, iRow)).Length, 11))
            Else
                m_sSpare = "Reversal " & CStr(m_vTransArray(ACISpare, iRow)) ' RAG set this again because it gets changed above!
            End If

            If v_sSpareComment <> "" Then
                m_sSpare = v_sSpareComment
            End If
            nResult = m_oTransDetail.DirectAdd(vTransdetailID:=nTransdetailId, vAccountID:=m_lAccountID, vPostingstatusID:=m_iTransPostingstatusID, vCompanyID:=m_iTransCompanyID, vCurrencyID:=m_iCurrencyID, vPeriodID:=m_lPeriodID, vDocumentID:=m_lTransDocumentID, vDocumentSequence:=m_iDocumentSequence, vAccountingDate:=dtAccountingDate, vAmount:=m_cAmount, vBaseAmountUnrounded:=m_dBaseAmountUnRounded, vFullyMatched:=m_iFullyMatched, vCurrencyAmount:=m_cCurrencyAmount, vCurrencyAmountUnrounded:=m_dCCyAmountUnrounded, vCurrencyBaseXrate:=m_dCurrencyBaseXRate, vEuroCurrencyId:=m_lEuro, vEuroAmount:=m_cEuroAmount, vEuroBaseXRate:=m_dEuroBaseXrate, vEuroCcyXrate:=m_dEuroCCyXrate, vComment:=m_sTransComment, vInsuranceRef:=m_sInsuranceRef, vOperatorID:=m_iOperatorID, vPurchaseOrderNo:=m_sPurchaseOrderNo, vPurchaseInvoiceNo:=m_sPurchaseInvoiceNo, vDepartment:=m_sDepartment, vSpare:=m_sSpare, vRefDate:=m_dtRefDate, vRefAmount:=m_cRefAmount, vRefQuantity:=m_dRefQuantity, vRefUnits:=m_sRefUnits, vTransdetailTypeID:=m_lTransdetailTypeID)

            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError("CreateDocument Failed", "CreateDocument Failed", PMELogLevel.PMLogError)
                Return nResult
            End If

            If m_iDocumentSequence = 1 AndAlso Informations.Left(m_sSpare.Trim().ToUpper(), 8) = "REVERSAL" Then
                ReversalTransDetailId = ToSafeLong(nTransdetailId, 0)
            End If
            'EK 040400
            If iRow = 0 Then
                ReDim m_vAllocatedToArray(iRow)
            Else
                ReDim Preserve m_vAllocatedToArray(iRow)
            End If
            'eck090800 changes to keep in line with manual allocation part payment
            'DC241103 PN8468 'was using wrong amount
            m_vAllocatedToArray(iRow) = CStr(nTransdetailId) & "|" & CStr(m_cAmount) 'm_cCurrencyAmount

            'sw build up arrays of transdetail ID's for credit and debit transactions
            If m_cAmount < 0 Then

                If Not Informations.IsNothing(r_vCreditTransDetailID) Then
                    If Informations.IsArray(r_vCreditTransDetailID) Then
                        ReDim Preserve r_vCreditTransDetailID(r_vCreditTransDetailID.GetUpperBound(0) + 1)

                        r_vCreditTransDetailID(r_vCreditTransDetailID.GetUpperBound(0)) = nTransdetailId
                    Else
                        ReDim r_vCreditTransDetailID(0)

                        r_vCreditTransDetailID(0) = nTransdetailId
                    End If
                End If
            Else

                If Not Informations.IsNothing(r_vDebitTransDetailID) Then
                    If Informations.IsArray(r_vDebitTransDetailID) Then
                        ReDim Preserve r_vDebitTransDetailID(r_vDebitTransDetailID.GetUpperBound(0) + 1)

                        r_vDebitTransDetailID(r_vDebitTransDetailID.GetUpperBound(0)) = nTransdetailId
                    Else
                        ReDim r_vDebitTransDetailID(0)

                        r_vDebitTransDetailID(0) = nTransdetailId
                    End If
                End If
            End If

        Next

        Return nResult

    End Function

    ''' <summary>
    ''' AllocateDocument
    ''' </summary>
    ''' <param name="v_bDisableTransactions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AllocateDocument(Optional ByVal v_bDisableTransactions As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim vKeys As Object

        Const kMethodName As String = "AllocateDocument"

        ' Dim sGroupCode As String
        ' Dim sRangeCode As String
        ' Dim nNumber As Integer
        ' Dim nTransdetailId As Integer
        Dim vMatchTrans As Object
        Dim bBeginTrans As Boolean
        Dim bProceed As Boolean
        Dim sDocumentType As String = ""
        Dim oResultArray As Object = Nothing
        Dim oArray As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Start SQL transaction and set flag if successful
            bBeginTrans = False

            If Not v_bDisableTransactions Then
                m_lReturn = BeginTrans()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BeginTrans", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bBeginTrans = True

            For lRow As Integer = m_vAllocatedArray.GetLowerBound(0) To m_vAllocatedArray.GetUpperBound(0)

                bProceed = True

                If m_bRecallInstalmentFromInstalmentMaint Then

                    AddParameterLite(m_oDatabase, "transdetail_id", ToSafeInteger(Informations.Left(m_vAllocatedArray(lRow), m_vAllocatedArray(lRow).IndexOf("|"))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=kSelectTransDetailSQL, sSQLName:=kSelectTransDetailName, bStoredProcedure:=True, lNumberRecords:=1, vResultArray:=oResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectTransDetailSQL, transdetail_id:=" & m_lTransdetailID, gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If Informations.IsArray(oResultArray) Then
                        m_lReturn = GetDocumentType(nDocumentId:=ToSafeInteger(oResultArray(6, 0)), r_sDocumentType:=sDocumentType)

                        If (Trim(sDocumentType)).ToUpper = "JN" Then

                            AddParameterLite(m_oDatabase, "document_id", ToSafeInteger(oResultArray(6, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetDocumentDetailSQL, sSQLName:=kGetDocumentDetailName, bStoredProcedure:=True, lNumberRecords:=1, vResultArray:=oArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetDocumentDetailSQL, document_id:=" & ToSafeInteger(oResultArray(6, 0)), gPMConstants.PMELogLevel.PMLogError)
                            End If

                            AddParameterLite(m_oDatabase, "insurance_file_cnt", ToSafeInteger(oArray(13, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAccountIDfromInsuranceFileCntSQL, sSQLName:=kGetAccountIDfromInsuranceFileCntName, bStoredProcedure:=True, lNumberRecords:=1,
                                vResultArray:=oArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetAccountIDfromInsuranceFileCntSQL, insurance_file_cnt:=" & ToSafeInteger(oArray(13, 0)), gPMConstants.PMELogLevel.PMLogError)
                            End If

                            If Informations.IsArray(oArray) Then
                                If ToSafeInteger(oArray(0, 0)) = ToSafeInteger(oResultArray(1, 0)) Then
                                    If ToSafeCurrency(oResultArray(48, 0)) <> ToSafeCurrency(oResultArray(9, 0)) Then
                                        bProceed = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                If bProceed Then


                    m_lReturn = m_oInsurerPayment.UnMarkTransaction(CInt(CStr(m_vAllocatedArray(lRow)).Substring(0, CStr(m_vAllocatedArray(lRow)).IndexOf("|"c))))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError("m_oInsurerPayment.UnMarkTransaction", "Function failed for transdetail_id " & CStr(m_vAllocatedArray(lRow)), gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ReDim vKeys(1, 2)
                    ' AccountID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountID

                    ' AllocatedTransID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_vAllocatedArray(lRow)

                    ' CashListItemID
                    ReDim vMatchTrans(0)

                    vMatchTrans(0) = m_vAllocatedToArray(lRow)

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans


                    m_lReturn = m_oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                    ' Set the keys

                    m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=vKeys)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oAllocationManual.SetKeys", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Pass accross the company for the document

                    m_oAllocationManual.CompanyId = m_iCompanyID

                    m_oAllocationManual.AllocatingReversal = True

                    'Start it
                    'Add the line,to assign the database
                    m_oAllocationManual.m_oDatabase = m_oDatabase
                    m_lReturn = m_oAllocationManual.Start(v_bDisableTransactions:=v_bDisableTransactions)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oAllocationManual.Start", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            Next lRow


            'Commit the Allocation
            If Not v_bDisableTransactions Then
                m_lReturn = CommitTrans()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CommitTrans", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If Not v_bDisableTransactions And bBeginTrans Then
                m_lReturn = RollbackTrans()
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
        End Try
        Return result
    End Function


    Private Function GetAutoNumValues(ByVal iDocumenttypeID As Integer, ByRef sGroupCode As String, ByRef sRangeCode As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Select Case iDocumenttypeID
            Case 1
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn
            Case 2
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef2
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSdn
            Case 3
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef3
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeScn
            Case 4
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef4
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSnd
            Case 5
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef5
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSnc
            Case 6
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef6
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCcr
            Case 7
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef7
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCdr
            Case 8
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef8
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeACc
            Case 9
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef9
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePpt
            Case 10
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef10
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRvj
            Case 11
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef11
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDpj
            Case 12
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef12
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRcj
            Case 13
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef13
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePin
            Case 14
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
            Case 15
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef15
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrd
            Case 16
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef16
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrc
            Case 17
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef17
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSed
            Case 18
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef18
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSec
            Case 19
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef19
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSat
            Case 20
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef20
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSaj
            Case 21
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef21
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSbd
            Case 22
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrp
            Case 23
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef23
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSpy
            Case 24
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef24
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSin
            Case 25
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef25
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePcn
            Case 26
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef26
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDia
            Case 27
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef27
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDir
            Case 28
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef28
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClp
            Case 29
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef29
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClr
            Case 30
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef30
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeFee
            Case 31
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef31
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShd
            Case 32
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef32
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShc
            Case 33
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef33
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDid
            Case 34
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef34
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDic
            Case 35
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef35
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrd
            Case 36
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef36
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrc
            Case 43
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef43
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeINC
            Case gACTLibrary.ACTDocTypeInstalmentRenewalCredit
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef45
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIRC
            Case gACTLibrary.ACTDocTypeInstalmentEndorsementCredit
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef46
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIEC
            Case gACTLibrary.ACTDocTypeCurrencyDifferenceCredit
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef49
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSCD
            Case gACTLibrary.ACTDocTypeClaimCloneReversal
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef58
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCLC
            Case Else
                'Default to Reverse Journal
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef10
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRvj
        End Select
        Return result
    End Function
    'eck090500
    ' ********************************************************************************* '
    ' Name: Private Function                                                            '
    '                                                                                   '
    ' Description: Checks that the document is valid for reversal                       '
    '                                                                                   '
    ' ********************************************************************************* '
    Private Function ValidDocumentType(ByVal vDocumentTypeID As Object) As Boolean

        Dim result As Boolean = False
        Dim vDocumentTypeArray As Object

        ReDim vDocumentTypeArray(20)

        vDocumentTypeArray(1) = gACTLibrary.ACTDocTypeNBDebit

        vDocumentTypeArray(2) = gACTLibrary.ACTDocTypeNBCredit

        vDocumentTypeArray(3) = gACTLibrary.ACTDocTypeRenewalDebit

        vDocumentTypeArray(4) = gACTLibrary.ACTDocTypeRenewalCredit

        vDocumentTypeArray(5) = gACTLibrary.ACTDocTypeEndorsementDebit

        vDocumentTypeArray(6) = gACTLibrary.ACTDocTypeEndorsementCredit

        vDocumentTypeArray(7) = gACTLibrary.ACTDocTypeShortPeriodDebit

        vDocumentTypeArray(8) = gACTLibrary.ACTDocTypeShortPeriodCredit

        vDocumentTypeArray(9) = gACTLibrary.ACTDocTypeTransferredDebit

        vDocumentTypeArray(10) = gACTLibrary.ACTDocTypetransferredCredit

        vDocumentTypeArray(11) = gACTLibrary.ACTDocTypeFee

        vDocumentTypeArray(12) = gACTLibrary.ACTDocTypeAdjustment

        vDocumentTypeArray(13) = gACTLibrary.ACTDocTypeReceipt

        vDocumentTypeArray(14) = gACTLibrary.ACTDocTypePayment

        vDocumentTypeArray(15) = gACTLibrary.ACTDocTypeJournal

        vDocumentTypeArray(16) = gACTLibrary.ACTDocTypeInstalmentNBCredit

        vDocumentTypeArray(17) = gACTLibrary.ACTDocTypeInstalmentRenewalCredit

        vDocumentTypeArray(18) = gACTLibrary.ACTDocTypeInstalmentEndorsementCredit

        vDocumentTypeArray(19) = gACTLibrary.ACTDocTypeDirectToInsurerDebit

        vDocumentTypeArray(20) = gACTLibrary.ACTDocTypeDirectToInsurerCredit

        If m_sCallingAppName = "bAllocationPost" Then
            ReDim Preserve vDocumentTypeArray(21)
            ReDim Preserve vDocumentTypeArray(22)
            ReDim Preserve vDocumentTypeArray(23)
            vDocumentTypeArray(21) = gACTLibrary.ACTDocTypeWriteOff
            vDocumentTypeArray(22) = ToSafeInteger(gACTLibrary.ACTDocTypeCurrencyDifferenceCredit)
            vDocumentTypeArray(23) = ToSafeInteger(gACTLibrary.ACTDocTypeCashDebit)
        ElseIf m_sCallingAppName = "bSIRCloneRIBatchProcess" Then
            ReDim Preserve vDocumentTypeArray(21)
            ReDim Preserve vDocumentTypeArray(22)
            ReDim Preserve vDocumentTypeArray(23)
            ReDim Preserve vDocumentTypeArray(24)
            vDocumentTypeArray(21) = gACTLibrary.ACTDocTypeClaimPayment
            vDocumentTypeArray(22) = gACTLibrary.ACTDocTypeClaimReceipt
            vDocumentTypeArray(23) = gACTLibrary.ACTDocTypeClaimOpen
            vDocumentTypeArray(24) = gACTLibrary.ACTDocTypeClaimAmend
        End If


        For lLoop As Integer = 1 To vDocumentTypeArray.GetUpperBound(0)


            If CInt(vDocumentTypeArray(lLoop)) = CInt(vDocumentTypeID) Then
                result = True
                Exit For
            End If
        Next
        Return result
    End Function
    'FSA Phase 3.2 Functions
    Private Function GetAssociatedDocuments(ByVal lDocumentId As Integer, ByVal lTransdetailId As Integer, ByRef vAssociatedDocuments As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAssociatedDocuments
        ' PURPOSE: Get Associated Documents
        ' AUTHOR: Elaine Knott
        ' DATE: JAN 2005
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue



        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="DocumentId", vValue:=CStr(lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="TransdetailId", vValue:=CStr(lTransdetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = .SQLSelect(sSQL:=ACGetReversedAccountsTransactionsforReversalSQL, sSQLName:=ACGetReversedAccountsTransactionsforReversalName, bStoredProcedure:=ACGetReversedAccountsTransactionsforReversalStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTDocumentReversal.Business - GetAssociatedDocuments -  Failed")
            End If




            vAssociatedDocuments = vResultArray

        End With


        Return result

    End Function

    Private Function GetAssocIntroDocuments(ByVal lDocumentId As Integer, ByRef vAssocIntroDocuments As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAssocIntroDocuments
        ' PURPOSE: Get Associated Introducer Documents
        ' AUTHOR: David Cleaver
        ' DATE: JAN 2005
        ' REMARKS: Reversing Introducer Transactions
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="DocumentId", vValue:=CStr(lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetIntroducerTransforReversalSQL, sSQLName:=ACGetIntroducerTransforReversalName, bStoredProcedure:=ACGetIntroducerTransforReversalStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTDocumentReversal.Business - GetAssocIntroDocuments -  Failed")
            End If




            vAssocIntroDocuments = vResultArray

        End With


        Return result

    End Function

    Public Function RecallReleasedAccountsTransaction(ByVal vAssociatedDocuments(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: RecallReleasedAccountsTransaction
        ' PURPOSE: Recall Released Transactions
        ' AUTHOR: Elaine Knott
        ' DATE: JAN 2005
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vAssociatedDocuments) Then
                Return result
            End If

            For lRow As Integer = 0 To vAssociatedDocuments.GetUpperBound(1)


                With m_oDatabase

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="TransdetailId", vValue:=CStr(CInt(vAssociatedDocuments(1, lRow))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    If CStr(vAssociatedDocuments(5, lRow)) = "" Then

                        'developer guide no. 85
                        m_lReturn = .Parameters.Add(sName:="AllocationId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else

                        m_lReturn = .Parameters.Add(sName:="AllocationId", vValue:=CStr(CInt(vAssociatedDocuments(5, lRow))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If

                    m_lReturn = .SQLAction(sSQL:=ACRecallReleasedAccountsTransactionsSQL, sSQLName:=ACRecallReleasedAccountsTransactionsName, bStoredProcedure:=ACRecallReleasedAccountsTransactionsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTDocumentReversal.Business - RecallReleasedAccountsTransaction -  Failed")
                    End If


                End With

            Next lRow





            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RecallReleasedAccountsTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function




    ' ***************************************************************** '
    ' Name: BeginTrans (Public)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'SW changed this to public 30/4/2003
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Public)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'SW changed this to public 30/4/2003
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Public)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'SW changed this to public 30/4/2003
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



#Region "Private Methods"
    'Author: Daniel Morey
    'This function checks the transaction that has already been passed in, to discover if it is a
    'valid transaction to be reversed. It can also check for any messages that need to be displayed
    'to the user for infomational purposes or to get the user to confirm the reversal.
    '
    'v_bOnlyCheckForInvalidTransaction = If this is set to true then the script only checks that this transaction is valid to reverse.
    'r_lCheckType = Returns the type of check that has produced a result (1 = Invalid Transaction, 2 = User message:OK/Cancel and 3 = User message:OK Only)
    'r_sCheckReason = Returns the text that needs to be displayed in the message.
    Public Function CheckTransactionForReversal(ByVal v_bOnlyCheckForInvalidTransaction As Boolean, ByRef r_vCheckResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckTransactionForReversal"

        Dim lNoOfResults As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim lOriginalDocumentID As Integer
        Dim lOriginalTransdetailID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Initialise
            lNoOfResults = 0
            lOriginalDocumentID = m_lDocumentID
            lOriginalTransdetailID = m_lTransdetailID

            ReDim r_vCheckResults(1, 0)

            'Get all of the documents that we will reverse if this reversal goes ahead
            m_lReturn = GetDocumentsForReversal()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetDocumentsForReversal", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Loop through all of the transactions to be reversed and check them all
            For lDocumentLoop As Integer = 0 To m_vDocumentIds.GetUpperBound(0)

                If Not Object.Equals(m_vDocumentIds(lDocumentLoop), Nothing) Then

                    m_lDocumentID = CInt(m_vDocumentIds(lDocumentLoop))

                    'Get the details of the transaction
                    m_lReturn = GetDocument(vDocumentID:=m_lDocumentID)

                    'Invalid Selection Check - Don't reverse if transaction is not of a valid type
                    If Not ValidDocumentType(vDocumentTypeID:=m_iDocumentTypeID) Then
                        ReDim Preserve r_vCheckResults(1, lNoOfResults)

                        r_vCheckResults(0, lNoOfResults) = "1"

                        r_vCheckResults(1, lNoOfResults) = "Cannot reverse this transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "This type of transaction can not be reversed."
                        lNoOfResults += 1
                        Return result
                    End If

                    'Invalid Selection Check - Don't reverse if transaction reverses another transaction
                    If m_sComment.IndexOf("Reversal ") >= 0 Then
                        ReDim Preserve r_vCheckResults(1, lNoOfResults)

                        r_vCheckResults(0, lNoOfResults) = "1"

                        r_vCheckResults(1, lNoOfResults) = "Cannot reverse this transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "This transaction reverses another transaction."
                        lNoOfResults += 1
                        Return result
                    End If

                    'Invalid Selection Check - Don't reverse if transaction has already been reversed
                    If m_sComment.IndexOf(" Reversed") >= 0 Then
                        ReDim Preserve r_vCheckResults(1, lNoOfResults)

                        r_vCheckResults(0, lNoOfResults) = "1"

                        r_vCheckResults(1, lNoOfResults) = "Cannot reverse this transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "This transaction has already been reversed."
                        lNoOfResults += 1
                        Return result
                    End If

                    'Invalid Selection Check - Don't reverse if transaction is reconciled
                    bPMAddParameter.AddParameterLite(m_oDatabase, "document_id", m_lDocumentID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckReconciledSQL, sSQLName:=ACCheckReconciledName, bStoredProcedure:=ACCheckReconciledStored, vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACCheckReconciledSQL", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If Informations.IsArray(vResultArray) Then
                        ReDim Preserve r_vCheckResults(1, lNoOfResults)

                        r_vCheckResults(0, lNoOfResults) = "1"

                        r_vCheckResults(1, lNoOfResults) = "Cannot reverse this transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "This transaction has been reconciled."
                        lNoOfResults += 1
                        Return result
                    End If

                    'Get the details of the transaction lines
                    m_lReturn = GetTransactions(vDocumentID:=m_lDocumentID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetTransactions", "vDocumentID:=" & m_lDocumentID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Loop through all of the transaction lines and check them all
                    For lTransDetailLoop As Integer = 0 To m_vTransArray.GetUpperBound(1)

                        'Invalid Selection Check - Don't reverse the transaction if it has a commission adjustment on it.
                        If CStr(m_vTransArray(ACISpare, lTransDetailLoop)).Trim() = "BROK ADJ" Then
                            ReDim Preserve r_vCheckResults(1, lNoOfResults)

                            r_vCheckResults(0, lNoOfResults) = "1"

                            r_vCheckResults(1, lNoOfResults) = "Cannot reverse this transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "This transaction has had it's commission adjusted."
                            lNoOfResults += 1
                            Return result
                        End If

                        'Invalid Selection Check - Don't reverse the transaction if a line has had an inappropriate allocation applied to it.
                    Next

                    If Not v_bOnlyCheckForInvalidTransaction Then

                        'User Message:OK/Cancel - Reversing direct to insurer transactions should warn the user that
                        'the DIC/DID transaction will also be reversed if they continue
                        bPMAddParameter.AddParameterLite(m_oDatabase, "document_id", m_lDocumentID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckDirectToInsurerSQL, sSQLName:=ACCheckDirectToInsurerName, bStoredProcedure:=ACCheckDirectToInsurerStored, vResultArray:=vResultArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACCheckDirectToInsurerSQL", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        If Informations.IsArray(vResultArray) Then
                            If Not Informations.IsArray(m_vAssocDirectToInsurer) Then
                                ReDim Preserve r_vCheckResults(1, lNoOfResults)

                                r_vCheckResults(0, lNoOfResults) = "2"

                                r_vCheckResults(1, lNoOfResults) = "This transaction has a corresponding DID/DIC transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "But DID/DIC transaction is not linked to corresponding original document. " & Strings.ChrW(13) & Strings.ChrW(10) & "Only the SND/SNC transaction will now be reversed."
                                lNoOfResults += 1
                            Else
                                ReDim Preserve r_vCheckResults(1, lNoOfResults)

                                r_vCheckResults(0, lNoOfResults) = "2"

                                r_vCheckResults(1, lNoOfResults) = "This transaction has a corresponding Direct Debit transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "Continuing this process will reverse BOTH transactions."
                                lNoOfResults += 1
                            End If
                        End If

                        'Only check original transaction for this check
                        If lDocumentLoop = 0 Then
                            'User Message:OK/Cancel - Reversing introducer transactions should warn the user that
                            'the related transaction will also be reversed if they continue
                            If m_bIntroducer Then
                                ReDim Preserve r_vCheckResults(1, lNoOfResults)

                                r_vCheckResults(0, lNoOfResults) = "2"

                                r_vCheckResults(1, lNoOfResults) = "This introducer transaction has related transactions." & Strings.ChrW(13) & Strings.ChrW(10) & "Continuing this process will reverse ALL related transactions."
                                lNoOfResults += 1
                            End If
                        End If

                        'Only check original transaction for this check
                        If lDocumentLoop = 0 Then
                            'User Message:OK Only - Reversing DIC/DID transactions should warn the user that
                            'the DIC/DID transaction will also be reversed if they continue
                            If m_iDocumentTypeID = gACTLibrary.ACTDocTypeDirectToInsurerDebit Or m_iDocumentTypeID = gACTLibrary.ACTDocTypeDirectToInsurerCredit Then
                                ReDim Preserve r_vCheckResults(1, lNoOfResults)

                                r_vCheckResults(0, lNoOfResults) = "3"

                                r_vCheckResults(1, lNoOfResults) = "This DID/DIC transaction has a corresponding transaction." & Strings.ChrW(13) & Strings.ChrW(10) & "Only the DID/DIC transaction will now be reversed." & Strings.ChrW(13) & Strings.ChrW(10) & "You may want to reverse the corresponding transaction after this."
                                lNoOfResults += 1
                            End If
                        End If

                    End If
                End If
            Next


            If Object.Equals(r_vCheckResults(0, 0), Nothing) Then

                'developer guide no. 101
                r_vCheckResults = Nothing
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            m_lDocumentID = lOriginalDocumentID
            m_lTransdetailID = lOriginalTransdetailID


        End Try
        Return result
    End Function

    Private Function GetDocumentsForReversal() As Integer

        Dim result As Integer = 0
        ' Const kMethodName As String = "GetDocumentsForReversal"





        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lDocumentID <> 0 Then

            'Set original transaction as the first transaction to reverse
            ReDim m_vDocumentIds(0)
            m_vDocumentIds(0) = m_lDocumentID

            m_lReturn = GetAssociatedDocuments(lDocumentId:=m_lDocumentID, lTransdetailId:=0, vAssociatedDocuments:=m_vAssociatedDocuments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetAssociatedDocuments", "lDocumentId:=" & m_lDocumentID, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(m_vAssociatedDocuments) Then
                ReDim Preserve m_vDocumentIds(m_vAssociatedDocuments.GetUpperBound(1) + 1)
                For lLoop As Integer = 0 To m_vAssociatedDocuments.GetUpperBound(1)
                    m_vDocumentIds(lLoop + 1) = CInt(m_vAssociatedDocuments(0, lLoop))
                Next
            End If

        Else

            m_lReturn = GetDocumentsFromTransaction(m_vDocumentIds)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetDocumentsFromTransaction", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If



        Return result
    End Function


    Private Function GetAssocDirectToInsurerDocuments(ByVal v_lDocumentId As Integer, ByRef r_vDocuments As Object) As Integer

        Dim result As Integer = 0
        ' Const kMethodName As String = "GetDocumentsForReversal"

        Dim vResultArray(,) As Object = Nothing




        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "document_id", v_lDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDirectToInsurerTransforReversalSQL, sSQLName:=ACGetDirectToInsurerTransforReversalName, bStoredProcedure:=ACGetDirectToInsurerTransforReversalStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetDirectToInsurerTransforReversalSQL", gPMConstants.PMELogLevel.PMLogError)
        End If



        r_vDocuments = vResultArray


        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (ReverseCreditControlItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ReverseCreditControlItem(ByVal v_lDocumentId As Integer) As Integer
    'Dim result As Integer = 0
    'Const kMethodName As String = "ReverseCreditControlItem"
    '
    'On Error GoTo Catch_Renamed
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    '.Parameters.Clear()
    '
    'm_lReturn = .Parameters.Add(sName:="document_id", vValue:=CStr(v_lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    '
    'm_lReturn = .SQLAction(sSQL:=ACReverseCreditControlItemSQL, sSQLName:=ACReverseCreditControlItemName, bStoredProcedure:=ACReverseCreditControlItemStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACReverseCreditControlItemSQL, Document_Id:=" & v_lDocumentId, gPMConstants.PMELogLevel.PMLogError)
    'End If
    'End With
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Start())
    '
    'Finally_Renamed: '
    '
    'Return result
    '
    ' This is for debugging only
    'Resume 
    '
    'Return result
    'End Function

    ''' <summary>
    ''' GetDocumentType
    ''' </summary>
    ''' <param name="nDocumentId"></param>
    ''' <param name="r_sDocumentType"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetDocumentType(ByVal nDocumentId As Integer, ByRef r_sDocumentType As String) As Integer

        ' Const kMethodName As String = "GetDocumentType"
        Dim oResultArray As Object = Nothing
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse

        AddParameterLite(m_oDatabase, "document_id", nDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

        nResult = m_oDatabase.SQLSelect(sSQL:=kGetDocumentDetailSQL, sSQLName:=kGetDocumentDetailName, bStoredProcedure:=kGetDocumentDetailStored, vResultArray:=oResultArray)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetDocumentDetailSQL", gPMConstants.PMELogLevel.PMLogError)
        End If
        If Informations.IsArray(oResultArray) Then
            r_sDocumentType = ToSafeString(oResultArray(18, 0))
        End If
        Return nResult
    End Function
#End Region

End Class
