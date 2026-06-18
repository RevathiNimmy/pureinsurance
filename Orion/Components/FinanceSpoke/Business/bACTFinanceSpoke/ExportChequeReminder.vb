Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportChequeReminder

    '====================================================================
    '   Class/Module: ExportUnPresentedChequeReminder
    '   Description : Class implementation of use case:
    'Export for InterfaceCode: "CHEQUE_REMINDER"
    '
    '====================================================================
    '   Maintenance History
    '
    '   05/02/2003  SW Created
    '
    'return status and message
    '====================================================================

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportUnPresentedChequeReminder"
    'developer guide no. 39
    Private Const ACCRGetChequeReminderSQL As String = "spu_ACT_Spoke_Get_Cheque_Reminder"
    Private Const ACCRGetChequeReminderName As String = "GetChequeReminder"

    'developer guide no. 39
    Private Const ACCRGetDocIDFromRefSQL As String = "spu_ACT_Spoke_Get_DocIDFromRef"
    Private Const ACCRGetDocIDFromRefName As String = "GetDocIDFromRef"

    'developer guide no. 39
    Private Const ACCRGetDocTypeIDFromDocTypeRefSQL As String = "spu_ACT_Spoke_Get_DocTypeIDFromDocTypeRef"
    Private Const ACCRGetDocTypeIDFromDocTypeRefName As String = "GetDocTypeIDFromDocTypeRef"

    'developer guide no. 39
    Private Const ACCRGetEventTypeIDFromCodeSQL As String = "spu_ACT_Spoke_Get_EventTypeIDFromCode"
    Private Const ACCRGetEventTypeIDFromCodeName As String = "GetEventTypeIDFromCode"

    'developer guide no. 39
    Private Const ACCRUpdateChequeReminderDateSQL As String = "spu_ACT_Spoke_Update_Cheque_Reminder"
    Private Const ACCRUpdateChequeReminderDateName As String = "UpdateChequeReminderDate"

    'End sw

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

    '#Region " Stored Procedures "

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
    ' PURPOSE: Start process for use case
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByVal v_vHeaderData() As Object) As Integer

        Dim result As Integer = 0
        Dim vHeaderInfo As Object
        Dim lMonths As Integer
        Dim sMediaTypeCode As String = ""
        Dim dtCutOffDate As Date
        Dim vPaymentDetails(,) As Object
        Dim lRowCount As Integer
        'Developer Guide no.108
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim oEventLog As bSIREvent.Business
        Dim lDocID, lDocTypeID, lEventTypeID As Integer

        Dim r_vTransDetails(2, 1) As Object



        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED



            vHeaderInfo = v_vHeaderData(1)


            lMonths = CInt(vHeaderInfo(ACCRMonths))

            sMediaTypeCode = CStr(vHeaderInfo(ACCRMediaTypeCode))

            'find the cut off date
            dtCutOffDate = DateTime.Today.AddMonths(-lMonths)

            'get the payment details

            m_lReturn = GetPaymentDetails(sMediaTypeCode, dtCutOffDate, vPaymentDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get payment details")
                Return result
            End If

            ' Get the Template ID for the un-presented cheque reminder letter
            m_lReturn = GetDocIDFromDocRef(ACCRChequeReminderTemplateCode, lDocID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get unpresented cheque reminder document ID")
                Return result
            End If

            '  get the document type id for the standard letter document type
            m_lReturn = GetDocTypeIDFromDocTypeRef(ACCRStandardLetterTypeCode, lDocTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get standard letter document type ID")
                Return result
            End If

            'get the event type id from the code for document production
            m_lReturn = GetEventTypeIDFromCode(ACCRDocumentProductionEvent, lEventTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get the document production event type ID")
                Return result
            End If


            ' Create an instance of the Event Log business object

            oEventLog = New bSIREvent.Business
            m_lReturn = oEventLog.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to create the Event Log Object")
                Return result
            End If


            If Information.IsArray(vPaymentDetails) Then
                'loop through the payments returned from getpaymentdetails

                lRowCount = vPaymentDetails.GetUpperBound(1)

                For lRow As Integer = 0 To lRowCount
                    ' Create an instance of the Document Manager Wrapper business object
                    'Developer Guide no.108
                    oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()

                    m_lReturn = CType(oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to create the Doc Manager Object")
                        Return result
                    End If

                    ' Set the appropriate properties
                    oDocManagerWrapper.DocumentTemplateId = lDocID 'unpresented cheque reminder letter

                    oDocManagerWrapper.DocumentTypeId = lDocTypeID 'standard letter retrieved earlier


                    oDocManagerWrapper.PartyCnt = CInt(vPaymentDetails(ACCRPaymentPartyCnt, lRow))

                    '#######################################################################
                    'This line of code has been commented out temporarily, however it will need
                    'to be included in release 4. SW 06/02/2003 (Doc Manager Wrapper does not support
                    'at the time of writing however an enhancement has been scheduled)
                    '
                    'oDocManagerWrapper.CashListItemID = CLng(vPaymentDetails(ACCRPaymentCashListItemID, lRow))
                    '
                    '#######################################################################
                    oDocManagerWrapper.SpoolDesc = "Unpresented Cheque Reminder Letter"

                    oDocManagerWrapper.Mode = ACCRSpoolDocMode

                    ' Call the Start Method of iPMBDocTemplate.Interface
                    m_lReturn = oDocManagerWrapper.Start()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Unable to spool the cheque reminder letter")
                        Return result
                    End If

                    oDocManagerWrapper.Dispose()

                    oDocManagerWrapper = Nothing

                    ' Generate an event log entry

                    ' Add an event


                    m_lReturn = oEventLog.DirectAdd(vPartyCnt:=CInt(vPaymentDetails(ACCRPaymentPartyCnt, lRow)), vEventType:=lEventTypeID, vUserID:=m_iUserID, vEventDate:=DateTime.Now, vDescription:="Unpresented Cheque Reminder Letter")

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to write to the Event Log Object")
                        Return result
                    End If

                    'update cheque reminder date

                    If UpdateChequeReminderDate(CInt(vPaymentDetails(ACCRPaymentCashListItemID, lRow))) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to update cheque reminder date")
                        Return result
                    End If

                Next
            End If

            ' Kill the event object

            oEventLog.Dispose()
            oEventLog = Nothing

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE


            If Information.IsArray(vPaymentDetails) Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPaymentDetails
    ' PURPOSE: gets the payment details that require an unpresented cheque reminder letter
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Function GetPaymentDetails(ByVal v_sMediaCode As String, ByVal v_dtCutOffDate As String, ByRef r_vResultArray(,) As Object) As gPMConstants.PMEReturnCode

        ' Clear the Database Parameters Collection
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        m_oDatabase.Parameters.Clear()

        ' Add date as an input param
        If m_oDatabase.Parameters.Add(sName:="date", vValue:=v_dtCutOffDate, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="mediacode", vValue:=v_sMediaCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACCRGetChequeReminderSQL, sSQLName:=ACCRGetChequeReminderName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetDocIDFromDocRef
    ' PURPOSE: gets the document template ID from the passed document template ref
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetDocIDFromDocRef(ByVal v_sDocRef As String, ByRef r_lDocID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vDocID(,) As Object




        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the docref parameter
        If m_oDatabase.Parameters.Add(sName:="docref", vValue:=v_sDocRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACCRGetDocIDFromRefSQL, sSQLName:=ACCRGetDocIDFromRefName, bStoredProcedure:=True, vResultArray:=vDocID) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If Not Information.IsArray(vDocID) Then
            Return result
        Else

            r_lDocID = CInt(vDocID(0, 0))
            Return gPMConstants.PMEReturnCode.PMTrue
        End If




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocIDFromDocRef failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocIDFromDocRef", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetDocTypeIDFromDocTypeRef
    ' PURPOSE: gets the document template ID from the passed document template ref
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetDocTypeIDFromDocTypeRef(ByVal v_sDocTypeRef As String, ByRef r_lDocTypeID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vDocTypeID(,) As Object





        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the doctyperef parameter
        If m_oDatabase.Parameters.Add(sName:="doctyperef", vValue:=v_sDocTypeRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACCRGetDocTypeIDFromDocTypeRefSQL, sSQLName:=ACCRGetDocTypeIDFromDocTypeRefName, bStoredProcedure:=True, vResultArray:=vDocTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If Not Information.IsArray(vDocTypeID) Then
            Return result
        Else

            r_lDocTypeID = CInt(vDocTypeID(0, 0))
            Return gPMConstants.PMEReturnCode.PMTrue
        End If




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTypeIDFromDocTypeRef failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTypeIDFromDocTypeRef", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GeteventTypeIDFromEventTypeCode
    ' PURPOSE: gets the event type ID from the code
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetEventTypeIDFromCode(ByVal v_sEventTypeCode As String, ByRef r_lEventTypeID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vEventTypeID(,) As Object




        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the eventtypecode parameter
        If m_oDatabase.Parameters.Add(sName:="eventtypecode", vValue:=v_sEventTypeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACCRGetEventTypeIDFromCodeSQL, sSQLName:=ACCRGetEventTypeIDFromCodeName, bStoredProcedure:=True, vResultArray:=vEventTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If Not Information.IsArray(vEventTypeID) Then
            Return result
        Else

            r_lEventTypeID = CInt(vEventTypeID(0, 0))
            Return gPMConstants.PMEReturnCode.PMTrue
        End If




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEventTypeIDFromCode failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEventTypeIDFromCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateChequeReminderDate
    ' PURPOSE: updates the cheque reminder date on CLI
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function UpdateChequeReminderDate(ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the eventtypecode parameter
        If m_oDatabase.Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACCRUpdateChequeReminderDateSQL, sSQLName:=ACCRUpdateChequeReminderDateName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Class

