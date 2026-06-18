Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 29/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a ACTInvoice.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of ACTInvoices (Private)
    Private m_oACTInvoices As bACTInvoice.ACTInvoices

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Primary Keys to work with
    Private m_lInvoiceID As Integer

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' PRIVATE Data Members (End)

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetAccountName(ByRef lAccountID As Integer, ByRef sAccountName As String) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Returns the account name from the account ID
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Account name using the account id
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountNameSQL, sSQLName:=ACGetAccountNameName, bStoredProcedure:=ACGetAccountNameStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the account name
            'AK 230702 - check for the null value

            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(1).Fields()("short_code")) Or IsNothing(m_oDatabase.Records.Item(1).Fields()("short_code"))) Then
                sAccountName = m_oDatabase.Records.Item(1).Fields()("short_code")
            Else
                sAccountName = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

   
    Public Function GetNewID() As Integer
        'Function to get the Next ID from the invoice table

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNewIDSQL, sSQLName:=ACGetNewIDName, bStoredProcedure:=ACGetNewIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set oFields to refer to one Record
            'AK 230702 - scalability - check for null values


            'Developer Guide No. 111
            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("MaxID")) Or IsNothing(m_oDatabase.Records.Item(0).Fields()("MaxID"))) Then
                Return m_oDatabase.Records.Item(0).Fields()("MaxID") + 1
            Else
                Return 1
            End If

        Catch
        End Try



        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNewID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


        Return result
    End Function


    Public Function GetAccountID(ByRef r_lSupplierID As Integer, ByVal v_sShortCode As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAccountID
        ' PURPOSE: Rewritten for handling Multi-Branch accounts
        ' AUTHOR: Danny Davis
        ' DATE: 10 July 2003, 05:03 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            Dim lRecordCount As Integer

            'Returns the account ID from the short_code

            result = gPMConstants.PMEReturnCode.PMTrue
            r_lSupplierID = 0

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("company_id", CStr(m_iSourceID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No. 85
                .Parameters.Add("sub_branch_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("ShortCode", v_sShortCode.Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                'Developer Guide No. 85
                .Parameters.Add("AccountID", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:="spu_ACT_Get_AccountID_From_ShortCode", sSQLName:="spu_ACT_Get_AccountID_From_ShortCode", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            'Get the account ID
            r_lSupplierID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("AccountID").Value)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function GetTransdetailTypeId(ByRef sCode As String, ByRef iTransdetailTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        'Returns the account name from the account ID
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Account name using the account id
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="transdetail_type_code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransdetailTypeIdSQL, sSQLName:=ACGetTransdetailTypeIdName, bStoredProcedure:=ACGetTransdetailTypeIdStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the type id

            'Developer Guide No.111
            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("transdetail_type_id")) Or IsNothing(m_oDatabase.Records.Item(0).Fields()("transdetail_type_id"))) Then
                iTransdetailTypeID = m_oDatabase.Records.Item(0).Fields()("transdetail_type_id")
            Else
                iTransdetailTypeID = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransdetailTypeId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransdetailTypeId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' PUBLIC Property Procedures (Begin)

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oACTInvoices.Count()
                    m_lCurrentRecord = m_oACTInvoices.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oACTInvoices.Count()

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property InvoiceID() As Integer
        Get

            Return m_lInvoiceID

        End Get
        Set(ByVal Value As Integer)

            m_lInvoiceID = Value

        End Set
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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


            ' Create new instance of component services


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove instance of component services

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create ACTInvoices Collection
            m_oACTInvoices = New bACTInvoice.ACTInvoices()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oACTInvoice As bACTInvoice.ACTInvoice) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="invoice_number", vValue:=oACTInvoice.InvoiceNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide no. 40
            m_lReturn = .Parameters.Add(sName:="invoice_date", vValue:=oACTInvoice.InvoiceDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="order_no", vValue:=oACTInvoice.OrderNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oACTInvoice.Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(oACTInvoice.SupplierID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oACTInvoice.Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reference", vValue:=oACTInvoice.Reference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oACTInvoice.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam(ByRef oACTInvoice As bACTInvoice.ACTInvoice) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="invoice_id", vValue:=CStr(oACTInvoice.InvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oACTInvoice As bACTInvoice.ACTInvoice) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add PrimaryKey parameters
        m_lReturn = CType(AddKeyInputParam(oACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Retain the Primary Key of the ACTInvoice Added
        InvoiceID = oACTInvoice.InvoiceID

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


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
                m_oACTInvoices = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied ACTInvoice property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oACTInvoice As bACTInvoice.ACTInvoice, ByRef iStatus As Integer, Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceNumber As String = "", Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As String = "", Optional ByRef vDescription As String = "", Optional ByRef vSupplierID As Integer = 0, Optional ByRef vCode As String = "", Optional ByRef vReference As String = "", Optional ByRef vCompanyID As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Default Any Missing Parameters

            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vInvoiceID:=vInvoiceID, vInvoiceNumber:=vInvoiceNumber, vInvoiceDate:=CDate(vInvoiceDate), vOrderNo:=vOrderNo, vDescription:=vDescription, vSupplierID:=vSupplierID, vCode:=vCode, vReference:=vReference, vCompanyID:=CByte(vCompanyID)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vInvoiceID:=vInvoiceID, vInvoiceNumber:=vInvoiceNumber, vInvoiceDate:=vInvoiceDate, vOrderNo:=vOrderNo, vDescription:=vDescription, vSupplierID:=vSupplierID, vCode:=vCode, vReference:=vReference, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oACTInvoice


            If Not Information.IsNothing(vInvoiceID) Then
                If .InvoiceID <> vInvoiceID Then
                    .InvoiceID = vInvoiceID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vInvoiceNumber) Then
                If .InvoiceNumber.Trim() <> vInvoiceNumber.Trim() Then
                    .InvoiceNumber = vInvoiceNumber
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vInvoiceDate) Then

                If DateTimeHelper.ToString(.InvoiceDate).Trim() <> CStr(vInvoiceDate).Trim() Then

                    .InvoiceDate = CDate(vInvoiceDate)
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vOrderNo) Then
                If .OrderNo.Trim() <> vOrderNo.Trim() Then
                    .OrderNo = vOrderNo
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDescription) Then
                If .Description.Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vSupplierID) Then
                If .SupplierID <> vSupplierID Then
                    .SupplierID = vSupplierID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vCode) Then
                If .Code.Trim() <> vCode.Trim() Then
                    .Code = vCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vReference) Then
                If .Reference.Trim() <> vReference.Trim() Then
                    .Reference = vReference
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vCompanyID) Then
                If CStr(.CompanyID).Trim() <> vCompanyID.Trim() Then
                    .CompanyID = CInt(vCompanyID)
                    bDataChanged = True
                End If
            End If

            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied ACTInvoice properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oACTInvoice As bACTInvoice.ACTInvoice, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No.112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        'Developer Guide No. 111
        oFields = m_oDatabase.Records.Item(0).Fields()


        With oACTInvoice
            'AK 230702 - scalability - check for null values - start

            If Not (Convert.IsDBNull(oFields("invoice_id")) Or IsNothing(oFields("invoice_id"))) Then
                .InvoiceID = oFields("invoice_id")
            Else
                .InvoiceID = 0
            End If

            If Not (Convert.IsDBNull(oFields("invoice_number")) Or IsNothing(oFields("invoice_number"))) Then
                .InvoiceNumber = oFields("invoice_number")
            Else
                .InvoiceNumber = CStr(0)
            End If

            If Not (Convert.IsDBNull(oFields("invoice_date")) Or IsNothing(oFields("invoice_date"))) Then
                .InvoiceDate = oFields("invoice_date")
            Else
                .InvoiceDate = #12/29/1899#
            End If

            If Not (Convert.IsDBNull(oFields("order_no")) Or IsNothing(oFields("order_no"))) Then
                .OrderNo = oFields("order_no")
            Else
                .OrderNo = CStr(0)
            End If

            If Not (Convert.IsDBNull(oFields("description")) Or IsNothing(oFields("description"))) Then
                .Description = oFields("description")
            Else
                .Description = ""
            End If

            If Not (Convert.IsDBNull(oFields("account_id")) Or IsNothing(oFields("account_id"))) Then
                .SupplierID = oFields("account_id")
            Else
                .SupplierID = 0
            End If

            If Not (Convert.IsDBNull(oFields("code")) Or IsNothing(oFields("code"))) Then
                .Code = oFields("code")
            Else
                .Code = ""
            End If

            If Not (Convert.IsDBNull(oFields("reference")) Or IsNothing(oFields("reference"))) Then
                .Reference = oFields("reference")
            Else
                .Reference = ""
            End If

            If Not (Convert.IsDBNull(oFields("company_id")) Or IsNothing(oFields("company_id"))) Then
                .CompanyID = oFields("company_id")
            Else
                .CompanyID = CInt("")
            End If

        End With

        Return result

    End Function
    ' PUBLIC Methods (End)


    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oACTInvoice As bACTInvoice.ACTInvoice) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddKeyInputParam(oACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK
        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' PUBLIC Methods (End)

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the ACTInvoice for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Information.IsNothing(vInvoiceID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vSupplierID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vSupplierID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function
    ' PRIVATE Methods (End)




    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single ACTInvoice directly into the database.
    '        Note: The ACTInvoice will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoice As bACTInvoice.ACTInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTInvoice
            oACTInvoice = New bACTInvoice.ACTInvoice()

            ' Populate ACTInvoice Attributes








            m_lReturn = CType(SetProperties(oACTInvoice:=oACTInvoice, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vInvoiceID:=CInt(vInvoiceID), vInvoiceNumber:=CStr(vInvoiceNumber), vInvoiceDate:=vInvoiceDate, vOrderNo:=CStr(vOrderNo), vDescription:=CStr(vDescription), vSupplierID:=CInt(vSupplierID), vCode:=CStr(vCode), vReference:=CStr(vReference), vCompanyID:=CStr(vCompanyID)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoice = Nothing
                Return result
            End If

            ' Add the ACTInvoice to the Database
            m_lReturn = CType(AddItem(oACTInvoice), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoice = Nothing
                Return result
            End If

            ' Retain the Primary Key of the ACTInvoice Added
            With oACTInvoice
                InvoiceID = .InvoiceID
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oACTInvoice = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a ACTInvoice.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Object = 0, Optional ByRef vInvoiceNumber As String = "", Optional ByRef vInvoiceDate As Date = #12/30/1899#, Optional ByRef vOrderNo As String = "", Optional ByRef vDescription As String = "", Optional ByRef vSupplierID As Object = 0, Optional ByRef vCode As String = "", Optional ByRef vReference As String = "", Optional ByRef vCompanyID As Object = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vInvoiceID)) Or (vInvoiceID.Equals(0)) Or (bDefaultAll) Then
            vInvoiceID = 0
        End If



        If (Information.IsNothing(vInvoiceNumber)) Or (String.IsNullOrEmpty(vInvoiceNumber)) Or (bDefaultAll) Then
            vInvoiceNumber = ""
        End If



        If (Information.IsNothing(vInvoiceDate)) Or (vInvoiceDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vInvoiceDate = DateTime.Today
        End If



        If (Information.IsNothing(vOrderNo)) Or (String.IsNullOrEmpty(vOrderNo)) Or (bDefaultAll) Then
            vOrderNo = ""
        End If



        If (Information.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Information.IsNothing(vSupplierID)) Or (vSupplierID.Equals(0)) Or (bDefaultAll) Then
            vSupplierID = 0
        End If



        If (Information.IsNothing(vCode)) Or (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Information.IsNothing(vReference)) Or (String.IsNullOrEmpty(vReference)) Or (bDefaultAll) Then
            vReference = ""
        End If



        If (Information.IsNothing(vCompanyID)) Or (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oACTInvoice As bACTInvoice.ACTInvoice) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add PrimaryKey as INPUT parameters
        m_lReturn = CType(AddKeyInputParam(oACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single ACTInvoice directly from the database.
    '        Note: The ACTInvoice will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="invoice_id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required ACTInvoices and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vInvoiceID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oACTInvoice As bACTInvoice.ACTInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oACTInvoices.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key

            Dim dbNumericTemp2 As Double

            If (Not Information.IsNothing(vInvoiceID)) And (Not Double.TryParse(CStr(vInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vInvoiceID=" & CStr(vInvoiceID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Information.IsNothing(vInvoiceID) Then

                ' Create New ACTInvoice
                oACTInvoice = New bACTInvoice.ACTInvoice()

                ' Add the AccountID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="invoice_id", vValue:=CStr(vInvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records
                m_oDatabase.Parameters.Clear()

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New Invoice
                    oACTInvoice = New bACTInvoice.ACTInvoice()

                    m_lReturn = CType(SetPropertiesFromDB(oACTInvoice:=oACTInvoice, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oACTInvoice.DatabaseStatus = Task

                    ' Add ACTInvoice to collection
                    m_lReturn = CType(m_oACTInvoices.Add(oNewACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oACTInvoice = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the ACTInvoice.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults









            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vInvoiceID:=CByte(vInvoiceID), vInvoiceNumber:=CStr(vInvoiceNumber), vInvoiceDate:=CDate(vInvoiceDate), vOrderNo:=CStr(vOrderNo), vDescription:=CStr(vDescription), vSupplierID:=CByte(vSupplierID), vCode:=CStr(vCode), vReference:=CStr(vReference), vCompanyID:=CByte(vCompanyID)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required ACTInvoices and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oACTInvoice As bACTInvoice.ACTInvoice
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oACTInvoices.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oACTInvoice = m_oACTInvoices.Item(m_lCurrentRecord)

            ' Get the ACTInvoice Property Values








            'Developer Guide No.98
            m_lReturn = CType(GetProperties(oACTInvoice, iStatus, vInvoiceID:=vInvoiceID, vInvoiceNumber:=vInvoiceNumber, vInvoiceDate:=vInvoiceDate, vOrderNo:=vOrderNo, vDescription:=vDescription, vSupplierID:=vSupplierID, vCode:=vCode, vReference:=vReference, vCompanyID:=vCompanyID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTInvoice = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied ACTInvoice property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oACTInvoice As bACTInvoice.ACTInvoice, ByRef iStatus As Integer, Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceNumber As String = "", Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As String = "", Optional ByRef vDescription As String = "", Optional ByRef vSupplierID As Integer = 0, Optional ByRef vCode As String = "", Optional ByRef vReference As String = "", Optional ByRef vCompanyID As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oACTInvoice

            'Developer guide no.143
            vInvoiceID = .InvoiceID


            vInvoiceNumber = .InvoiceNumber


            vInvoiceDate = .InvoiceDate


            vOrderNo = .OrderNo


            vDescription = .Description


            vSupplierID = .SupplierID


            vCode = .Code


            vReference = .Reference


            vCompanyID = CStr(.CompanyID)


            iStatus = .DatabaseStatus

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied ACTInvoice into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoice As bACTInvoice.ACTInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is
            'the same as the Interface Form - 1 (because we havent added
            'this one yet.)
            If m_oACTInvoices.Count() <> (lRow - 1) Then
                'If m_oACTInvoices.Count() <> (lRow) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new ACTInvoice
            oACTInvoice = New bACTInvoice.ACTInvoice()

            ' Populate ACTInvoice Attributes

            m_lReturn = CType(SetProperties(oACTInvoice:=oACTInvoice, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vInvoiceID:=CInt(vInvoiceID), vInvoiceNumber:=CStr(vInvoiceNumber), vInvoiceDate:=vInvoiceDate, vOrderNo:=CStr(vOrderNo), vDescription:=CStr(vDescription), vSupplierID:=CInt(vSupplierID), vCode:=CStr(vCode), vReference:=CStr(vReference), vCompanyID:=CStr(vCompanyID)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTInvoice = Nothing
                Return result
            End If

            ' Add ACTInvoice to collection
            m_lReturn = CType(m_oACTInvoices.Add(oNewACTInvoice:=oACTInvoice), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoice = Nothing
                Return result
            End If

            oACTInvoice = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the ACTInvoice
    '              specified and updates the ACTInvoice with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oACTInvoice As bACTInvoice.ACTInvoice
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTInvoices.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oACTInvoice = m_oACTInvoices.Item(lRow)

            ' Check the Status of the ACTInvoice

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oACTInvoice.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update ACTInvoice Attributes

            m_lReturn = CType(SetProperties(oACTInvoice:=oACTInvoice, iStatus:=iStatus, vInvoiceID:=CInt(vInvoiceID), vInvoiceNumber:=CStr(vInvoiceNumber), vInvoiceDate:=vInvoiceDate, vOrderNo:=CStr(vOrderNo), vDescription:=CStr(vDescription), vSupplierID:=CInt(vSupplierID), vCode:=CStr(vCode), vReference:=CStr(vReference), vCompanyID:=CStr(vCompanyID)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTInvoice = Nothing
                Return result
            End If

            ' Release reference to ACTInvoice
            oACTInvoice = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oACTInvoices.Count()
                Select Case m_oACTInvoices.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified ACTInvoice can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTInvoice As bACTInvoice.ACTInvoice

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTInvoices.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oACTInvoice = m_oACTInvoices.Item(lRow)

            ' Check the Status of the ACTInvoice

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oACTInvoice.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oACTInvoice.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oACTInvoice.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to ACTInvoice
            oACTInvoice = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oACTInvoice As bACTInvoice.ACTInvoice
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oACTInvoices.Count()
                oACTInvoice = m_oACTInvoices.Item(lSub)


                Select Case oACTInvoice.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(AddItem(oACTInvoice), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(UpdateItem(oACTInvoice), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(DeleteItem(oACTInvoice), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the ACTInvoice
            With oACTInvoice
                InvoiceID = .InvoiceID
            End With

            ' Release last reference
            oACTInvoice = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oACTInvoices.Count()

                        ' With the item
                        With m_oACTInvoices.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oACTInvoices.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceNumber As Object = Nothing, Optional ByRef vInvoiceDate As Object = Nothing, Optional ByRef vOrderNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSupplierID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Information.IsNothing(vInvoiceNumber)) Or (Object.Equals(vInvoiceNumber, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vInvoiceDate)) Or (Object.Equals(vInvoiceDate, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vOrderNo)) Or (Object.Equals(vOrderNo, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vSupplierID)) Or (Object.Equals(vSupplierID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vCode)) Or (Object.Equals(vCode, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vReference)) Or (Object.Equals(vReference, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


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
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'MKW 140203 Insert as part of 1.6.9 --> 1.8.6 Catchup PN1466
    'IJB 15/11/2002 : Inserted following function to allow checking for existing
    '                 invoice numbers.
    ' ***************************************************************** '
    ' Name: CheckIfInvoiceNumberExists (Public)
    ' Description: Check If Invoice Number Exists
    ' ***************************************************************** '
    Public Function CheckIfInvoiceNumberExists(ByRef r_vAccountID As String, ByVal v_vInvoiceNumber As Object) As Integer

        Dim result As Integer = 0
        Dim vDuplicateArray(,) As Object
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = "SELECT account_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "FROM invoice" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE account_id = '" & r_vAccountID & "' "

            sSQL = sSQL & "AND invoice_number = '" & CStr(v_vInvoiceNumber) & "'"

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="CheckIfPolNoExists", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vDuplicateArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            If Information.IsArray(vDuplicateArray) Then

                If CStr(vDuplicateArray(0, 0)) = "" Then
                    r_vAccountID = CStr(0)
                Else
                    r_vAccountID = CStr(vDuplicateArray(0, 0))
                End If
            Else
                r_vAccountID = CStr(0)
            End If

            'return the data
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfPolicyNumberExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfInvoiceNumberExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
