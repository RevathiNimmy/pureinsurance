Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'Developer Guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 07/10/2003
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


    ' ***************************************************************** '
    ' Edit History :
    '
    ' RAW 13/01/2003 : PS187 : replaced hard-coded sql that deleted from
    '                          TransMatch with stored procedure
    '****************************************************************** '

    Private Const ACClass As String = "Business"

    ' Return value
    Private m_lReturn As Integer

    ' Instance of database object
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_oDocumentPost As Object
    Private m_oAutoNumber As Object
    Private m_oMatchPost As bACTMatchPost.Form
    Private m_oTransDetail As bACTTransDetail.Form
    Private m_sUnderwritingOrAgency As String = ""
    'sj 28/04/2003 - start
    Private m_oPMLock As bPMLock.User
    Private m_oFindTransaction As bACTFindTransaction.Business
    'sj 28/04/2003 - end

    ' Product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
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



            ' Get an instance of the database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            ' Remove component services

            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID
            m_iUserID = iUserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            'sj 28/04/2003 - start

            m_oPMLock = New bPMLock.User
            m_lReturn = m_oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oFindTransaction = New bACTFindTransaction.Business
            m_lReturn = m_oFindTransaction.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 28/04/2003 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                    m_oDatabase = Nothing
                End If
                If m_oMatchPost IsNot Nothing Then
                    m_oMatchPost.Dispose()
                    m_oMatchPost = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oPMLock IsNot Nothing Then
                    m_oPMLock.Dispose()
                    m_oPMLock = Nothing
                End If
                If m_oFindTransaction IsNot Nothing Then
                    m_oFindTransaction.Dispose()
                    m_oFindTransaction = Nothing
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SearchDetails
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function SearchDetails(ByVal v_lMarkedStatus As Integer, ByVal v_lMonth As Integer, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vDateTo As String = "", Optional ByRef lNumberOfRecords As Integer = 0, Optional ByRef r_vResultArray(,) As Object = Nothing, Optional ByRef r_cTotalReconciled As Decimal = 0, Optional ByRef r_cTotalUnreconciled As Decimal = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add parameters


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_vAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If (Not Information.IsNothing(v_vDateTo)) And (v_vDateTo <> "") Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="date_to", vValue:=v_vDateTo, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else

                'Developer Guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="date_to", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If v_lMarkedStatus <> -1 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="marked_status", vValue:=CStr(v_lMarkedStatus), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'Developer Guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="marked_status", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If v_lMonth = -1 Or v_lMonth = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="month", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                'Developer Guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="month", vValue:=CStr(v_lMonth), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            'Developer Guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_reconciled", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


            'Developer Guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_unreconciled", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_oDatabase.QueryTimeout = 0

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACBankReconciliationSQL, sSQLName:=ACBankReconciliationName, bStoredProcedure:=ACBankReconciliationStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_cTotalReconciled = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("total_reconciled").Value)
            r_cTotalUnreconciled = gPMFunctions.NullToCurrency(m_oDatabase.Parameters.Item("total_unreconciled").Value)

            ' Check there's some results
            If Not Information.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    ''
    '' Name: GetBankReconciliationTotals
    ''
    '' Description:
    ''
    '' History: 29/04/2003 sj - Created.
    ''
    '' ***************************************************************** '
    'Public Function GetBankReconciliationTotals( _
    ''    ByVal v_lMonth As Long, _
    ''    ByVal v_lAccountID As Long, _
    ''    ByVal v_vDateTo As Variant, _
    ''    ByRef r_vTotalArray As Variant) As Long
    '
    '    On Error GoTo Err_GetBankReconciliationTotals
    '
    '    GetBankReconciliationTotals = PMTrue
    '
    '    ' Clear the parameters
    '    m_oDatabase.Parameters.Clear
    '
    '    ' Add parameters
    '
    '    m_lReturn& = m_oDatabase.Parameters.Add( _
    ''        sName:="account_id", _
    ''        vValue:=v_lAccountID, _
    ''        idirection:=PMParamInput, _
    ''        iDataType:=PMLong)
    '
    '    If (IsMissing(v_vDateTo) = False) And (v_vDateTo <> "") Then
    '        m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="date_to", _
    ''            vValue:=v_vDateTo, _
    ''            idirection:=PMParamInput, _
    ''            iDataType:=PMDate)
    '    Else
    '        m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="date_to", _
    ''            vValue:=Null, _
    ''            idirection:=PMParamInput, _
    ''            iDataType:=PMDate)
    '    End If
    '
    '    If (v_lMonth& <> -1) Then
    '        m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="month", _
    ''            vValue:=v_lMonth&, _
    ''            idirection:=PMParamInput, _
    ''            iDataType:=PMLong)
    '    Else
    '        m_lReturn& = m_oDatabase.Parameters.Add( _
    ''            sName:="month", _
    ''            vValue:=Null, _
    ''            idirection:=PMParamInput, _
    ''            iDataType:=PMLong)
    '    End If
    '
    '
    '    ' Perform the SQL
    '    m_lReturn& = m_oDatabase.SQLSelect( _
    ''            sSQL:=ACBankReconciliationTotalsSQL, _
    ''            sSQLName:=ACBankReconciliationTotalsName, _
    ''            bStoredProcedure:=ACBankReconciliationTotalsStored, _
    ''            lNumberRecords:=PMAllRecords, _
    ''            vResultArray:=r_vTotalArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetBankReconciliationTotals = PMFalse
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_GetBankReconciliationTotals:
    '
    '    GetBankReconciliationTotals = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetBankReconciliationTotals Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetBankReconciliationTotals", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: MarkTransaction
    '
    ' Description: Creates a fake TransMatch entry to say that it's
    '              ready to be paid. This'll get over-written when it
    '              is actually paid.
    '
    ' See Also: UnMarkTransaction
    '
    ' ***************************************************************** '
    Public Function MarkTransaction(ByVal v_lTransactionID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal) As Integer

        Dim result As Integer = 0
        Dim dtAccountingDate As Date
        Dim lMatchID As Integer

        Dim lAllocationID As Integer
        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Byte
        'DD 05/08/2002: Multi-Branch
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oMatchPost Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_oMatchPost = New bACTMatchPost.Form
                m_lReturn = m_oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If

            ' Set the month to now for marking purposes
            dtAccountingDate = DateTime.Now

            ' DD 05/08/2002: Get the SubBranch
            m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vTransDetailID:=CStr(v_lTransactionID))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the match group

            m_lReturn = m_oMatchPost.AddMatchGroup(v_dtMatchDate:=dtAccountingDate, r_vMatchId:=lMatchID, v_lSubBranchID:=lSubBranchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set up the data
            lAllocationID = 0
            'EK 090200
            '    cBaseAmount@ = CCur(0)
            '    cCurrencyAmount@ = CCur(0)
            cBaseAmount = v_cPayment
            cCurrencyAmount = v_cPayment
            '
            vdCurrencyBaseXRate = 0

            ' Add the blank match

            m_lReturn = m_oMatchPost.AddMatchTrans(v_lAllocationdetailID:=lAllocationID, v_lTransDetailID:=v_lTransactionID, v_iCurrencyID:=v_iCurrencyID, v_cBaseMatchAmount:=cBaseAmount, v_cCurrencyMatchAmount:=cCurrencyAmount, v_vdCurrencyMatchXRate:=vdCurrencyBaseXRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Write the match posts

            m_lReturn = m_oMatchPost.Commit()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate

            m_oMatchPost.Dispose()
            m_oMatchPost = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MarkTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MarkTransaction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnMarkTransaction
    '
    ' Description: UnMarks a transaction.
    '
    ' History: 07/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UnMarkTransaction(ByVal v_lTransDetailID As Integer) As Integer

        'Dim sSQL As String                  ' RAW 13/01/2003 : PS187 : replaced by stored procedure
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 13/01/2003 : PS187 : replaced with stored procedure
            '    sSQL$ = "DELETE FROM TransMatch " & _
            ''            "WHERE transdetail_id = {transdetail_id} " & _
            ''            "AND allocationdetail_id IS null"
            ' RAW 13/01/2003 : PS187 : end

            ' Clear paramters
            m_oDatabase.Parameters.Clear()

            ' Add transdetail_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRecordsAffected = gPMConstants.PMAllRecords

            ' Perform Query
            ' RAW 13/01/2003 : PS187 : replaced hard-coded sql with stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUnMarkTransMatchSQL, sSQLName:=ACUnMarkTransMatchName, bStoredProcedure:=ACUnMarkTransMatchStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return PMNotFound if no records were deleted
            If lRecordsAffected = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnMarkTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnMarkTransaction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: Reconcile
    '
    ' Description: Sets the spare field in the bank Transaction to "Reconciled"
    '
    ' ***************************************************************** '
    Public Function Reconcile(ByVal vTransDetailIDs() As Object) As Integer

        Dim result As Integer = 0
        Dim sSpare As String = ""
        Dim lTransactionId As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oTransDetail Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_oTransDetail = New bACTTransDetail.Form
                m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If
            For lRow As Integer = 0 To vTransDetailIDs.GetUpperBound(0)


                lTransactionId = CInt(vTransDetailIDs(lRow))
                ' Get Transaction


                m_lReturn = m_oTransDetail.GetDetails(vTransDetailID:=lTransactionId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oTransDetail.GetNext(vTransDetailID:=lTransactionId, vSpare:=sSpare)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sSpare = "RECONCILED"

                ' Update

                m_lReturn = m_oTransDetail.EditUpdate(lRow:=1, vTransDetailID:=lTransactionId, vSpare:=sSpare)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oTransDetail.Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lRow
            ' Terminate

            m_oTransDetail.Dispose()
            m_oTransDetail = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Reconcile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Reconcile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetSymbolForCurrency
    '
    ' Description:
    '
    ' History: 01/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSymbolForCurrency(ByVal v_lCurrencyID As Integer, ByRef r_sSymbol As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT symbol FROM Currency WHERE currency_id = " & v_lCurrencyID

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSymbolForCurrency", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Information.IsArray(vResultArray) Then

                r_sSymbol = CStr(vResultArray(0, 0)).Trim()
            Else
                ' Default to GBP
                r_sSymbol = "£"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSymbolForCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSymbolForCurrency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'sj 28/04/2003 - start
    ' ***************************************************************** '
    '
    ' Name: LockBankAccount
    '
    ' Description:
    '
    ' History: 28/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function LockBankAccount(ByVal v_lAccountID As Integer, ByRef r_sCurrentlyLockedBy As String) As Integer

        Dim result As Integer = 0
        Try



            Return m_oPMLock.LockKey(sKeyName:=g_kLockBankReconciliation, vKeyValue:=v_lAccountID, iUserID:=m_iUserID, sCurrentlyLockedBy:=r_sCurrentlyLockedBy)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockBankAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockBankAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnLockBankAccount
    '
    ' Description:
    '
    ' History: 28/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function UnLockBankAccount(ByVal v_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Try



            Return m_oPMLock.UnLockKey(sKeyName:=g_kLockBankReconciliation, vKeyValue:=v_lAccountID, iUserID:=m_iUserID)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnLockBankAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockBankAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetAccountBalance
    '
    ' Description:
    '
    ' History: 29/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountBalance(ByRef r_vAccountBalance As Double, ByVal v_vAccountID As Object, ByVal v_vAccountingDate As Object, ByRef r_sFormattedBalance As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sAccountName, sContactName, sPhoneAreaCode, sPhoneNumber, sPhoneExtension As String
            Dim dAccountBalance As Double
            Dim lAccountCurrencyId As Integer
            Dim sAccountCode As String = ""

            ' Get the Account details



            m_lReturn = m_oFindTransaction.GetAccountDetails(r_lAccountID:=CInt(v_vAccountID), sAccountName:=sAccountName, sContactName:=sContactName, sPhoneAreaCode:=sPhoneAreaCode, sPhoneNumber:=sPhoneNumber, sPhoneExtension:=sPhoneExtension, r_vdAccountBalance:=dAccountBalance, r_sAccountCode:=sAccountCode, r_vlAccountCurrencyId:=lAccountCurrencyId, v_dtAccountingDate:=CDate(v_vAccountingDate))

            If lAccountCurrencyId <> 0 Then

                m_lReturn = m_oFindTransaction.FormatCurrency(vCurrencyID:=lAccountCurrencyId, vCurrencyAmount:=dAccountBalance, vFormattedCurrency:=r_sFormattedBalance, vConversionDate:=DateTime.Today)
            Else

                m_lReturn = m_oFindTransaction.FormatCurrency(vCurrencyID:=m_iCurrencyID, vCurrencyAmount:=dAccountBalance, vFormattedCurrency:=r_sFormattedBalance, vConversionDate:=DateTime.Today)
            End If

            r_vAccountBalance = dAccountBalance

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'sj 28/04/2003 - end


    ' ***************************************************************** '
    '
    ' Name: CurrencyFormat
    '
    ' Description: Used to make sure all figures are in the correct currency format
    '
    ' History: PSL 08/10/2003
    ' DJM 04/03/2004 : Changed to use a passed in currency rather than always using the base currency.
    ' ***************************************************************** '
    Public Function CurrencyFormat(ByVal v_cAmount As Decimal, ByVal v_iCurrencyID As Integer, ByRef r_sFormattedAmount As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oFindTransaction.FormatCurrency(vCurrencyID:=v_iCurrencyID, vCurrencyAmount:=v_cAmount, vFormattedCurrency:=r_sFormattedAmount, vConversionDate:=DateTime.Today)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CurrencyFormat Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CurrencyFormat", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
