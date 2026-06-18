Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 09/09/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a BankAccount.
    '
    ' Edit History: Code changes by Santosh in the method DeleteItem
    '               to avoid runtime error as on 11-Jan-2010
    ' ***************************************************************** '


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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of BankAccounts (Private)
    Private m_oBankAccounts As bACTBankAccount.BankAccounts

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    ' Primary Keys to work with
    ' Source ID
    Private m_lBankACcountId As Integer
    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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
                Case Is > m_oBankAccounts.Count()
                    m_lCurrentRecord = m_oBankAccounts.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            If m_oBankAccounts.Count > 0 AndAlso m_oBankAccounts.Item(0) Is Nothing Then
                Return m_oBankAccounts.Count - 1
            Else
                Return m_oBankAccounts.Count
            End If

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

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property BankAccountID() As Integer
        Get

            Return m_lBankACcountId

        End Get
        Set(ByVal Value As Integer)

            m_lBankACcountId = Value

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database
            'ECK 16/7/99 - must use component services

            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now


            ' Create BankAccounts Collection
            m_oBankAccounts = New bACTBankAccount.BankAccounts()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


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
                m_oBankAccounts = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a ACTBankAccount
    '
    '
    ' ***************************************************************** '
    'Developer Guide no. 101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Dim oACTBankAccount As New bACTBankAccount.BankAccount
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object

        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 12
            vResultArray = Nothing
            ' Reset Table Array

            'developer guide no. 12
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupCurrency

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oACTBankAccount = m_oBankAccounts.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oACTBankAccount

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .CurrencyID
                        dtEffectiveDate = DateTime.Now

                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oACTBankAccount
                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .CurrencyID
                        dtEffectiveDate = DateTime.Now
                        '            ' {* USER DEFINED CODE (End) *}

                    End With

            End Select

            ' Default Effective Date to current date/time

            ' Release ACTBankAccount reference
            oACTBankAccount = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single BankAccount directly into the database.
    '        Note: The BankAccount will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vBankAccountId As Integer = 0, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, Optional ByRef vReconciledDate As Object = Nothing, Optional ByRef vDefaultBankAccountID As Object = Nothing, Optional ByRef v_iIsCashReceiveInThisCurrencyOnly As Integer = 0, Optional ByRef v_sStartChequeNumber As String = "") As Integer

        Dim result As Integer = 0
        Dim oBankAccount As bACTBankAccount.BankAccount

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new BankAccount
            oBankAccount = New bACTBankAccount.BankAccount()

            ' Populate BankAccount Attributes











            'Developer Guide no.98
            m_lReturn = SetProperties(oBankAccount, gPMConstants.PMEComponentAction.PMAdd, vBankAccountId:=vBankAccountId, vCurrencyId:=vCurrencyId, vCompanyID:=vCompanyID, vAccountId:=vAccountId, vBankID:=vBankID, vCode:=vCode, vBankAccountNo:=vBankAccountNo, vBankAccountName:=vBankAccountName, vDescription:=vDescription, vNextChequeNumber:=vNextChequeNumber, vReconciledDate:=vReconciledDate, vDefaultBankAccountID:=vDefaultBankAccountID, v_iIsCashReceiveInThisCurrencyOnly:=v_iIsCashReceiveInThisCurrencyOnly, vStartChequeNumber:=v_sStartChequeNumber)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the BankAccount to the Database
            m_lReturn = CType(AddItem(oBankAccount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the BankAccount Added

            If Not Informations.IsNothing(vBankAccountId) Then
                vBankAccountId = oBankAccount.BankAccountID
            End If

            ' {* USER DEFINED CODE (End) *}

            oBankAccount = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single BankAccount directly from the database.
    '        Note: The BankAccount will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, Optional ByRef vReconciledDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBankAccount As bACTBankAccount.BankAccount

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new BankAccount
            oBankAccount = New bACTBankAccount.BankAccount()

            ' Populate BankAccount Attributes










            'developer guide no. 98
            m_lReturn = CType(SetProperties(oBankAccount, gPMConstants.PMEComponentAction.PMDelete, vBankAccountId:=vBankAccountId, vCurrencyId:=vCurrencyId, vCompanyID:=vCompanyID, vAccountId:=vAccountId, vBankID:=vBankID, vCode:=vCode, vBankAccountNo:=vBankAccountNo, vBankAccountName:=vBankAccountName, vDescription:=vDescription, vNextChequeNumber:=vNextChequeNumber, vReconciledDate:=vReconciledDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the BankAccount to the Database
            m_lReturn = CType(DeleteItem(oBankAccount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oBankAccount = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the BankAccount.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, Optional ByRef vReconciledDate As Object = Nothing, Optional ByRef vDefaultBankAccountID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults











            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vBankAccountId:=vBankAccountId, vCurrencyId:=vCurrencyId, vCompanyID:=vCompanyID, vAccountId:=vAccountId, vBankID:=vBankID, vCode:=vCode, vBankAccountNo:=vBankAccountNo, vBankAccountName:=vBankAccountName, vDescription:=vDescription, vNextChequeNumber:=vNextChequeNumber, vReconciledDate:=vReconciledDate, vDefaultBankAccountID:=vDefaultBankAccountID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oFields As ADODB.Fields

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableBankAccount) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required BankAccounts and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vLockMode As Integer = 0, Optional ByRef v_iSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMultiCurrencyBankingOption As Integer = 5058

        Dim lRecordCount As Integer
        Dim oBankAccount As bACTBankAccount.BankAccount
        Dim sOptionValue As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now this function is change according to task 'Account Function and CCY Cash Allocation'
            'If "Multi currency banking option" is checked in system options then load only those bank
            'which have been assocatied with the given source_id otherwise keep existing functionalily as it is.

            'Now lets start with checking the system option

            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=kMultiCurrencyBankingOption, r_sOptionValue:=sOptionValue), gPMConstants.PMEReturnCode)



            ' Clear the Collection
            m_oBankAccounts.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = 0
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vBankAccountId) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vBankAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vBankAccountID =" & CStr(vBankAccountId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the BankAccountID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(vBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                'BB Add the CompanyID parameter (INPUT)
                If v_iSourceID <> 0 Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_Id", vValue:=CStr(v_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                Else
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_Id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllBankAccountLinkedToSourceSQL, sSQLName:=ACSelAllBankAccountLinkedToSourceName, bStoredProcedure:=ACSelAllBankAccountLinkedToSourceStored, lNumberRecords:=0)

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
                'developer guide no. 162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New BankAccount
                    oBankAccount = New bACTBankAccount.BankAccount()

                    m_lReturn = CType(SetPropertiesFromDB(oBankAccount:=oBankAccount, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add BankAccount to collection
                    If m_oBankAccounts.Count = 0 Then
                        m_oBankAccounts.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oBankAccounts.Add(oNewBankAccount:=oBankAccount), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oBankAccount = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Gets the required BankAccounts and populate the Collection
    ''' </summary>
    ''' <param name="vBankAccountId"></param>
    ''' <param name="vCurrencyId"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountId"></param>
    ''' <param name="vBankID"></param>
    ''' <param name="vCode"></param>
    ''' <param name="vBankAccountNo"></param>
    ''' <param name="vBankAccountName"></param>
    ''' <param name="vDescription"></param>
    ''' <param name="vNextChequeNumber"></param>
    ''' <param name="vReconciledDate"></param>
    ''' <param name="vDefaultBankAccountID"></param>
    ''' <param name="vBankAccountTypeId"></param>
    ''' <param name="v_iIsCashReceiveInThisCurrencyOnly"></param>
    ''' <param name="vStartChequeNumber"></param>
    ''' <param name="vFinancialInstitutionCode"></param>
    ''' <param name="vDirectDebitSupplierName"></param>
    ''' <param name="vDirectDebitSupplierID"></param>
    ''' <param name="vRemitter"></param>
    ''' <param name="vProcessingDays"></param>
    ''' <param name="r_sBIC"></param>
    ''' <param name="r_sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNext(Optional ByRef vBankAccountId As Object = Nothing,
                            Optional ByRef vCurrencyId As Object = Nothing,
                            Optional ByRef vCompanyID As Object = Nothing,
                            Optional ByRef vAccountId As Object = Nothing,
                            Optional ByRef vBankID As Object = Nothing,
                            Optional ByRef vCode As Object = Nothing,
                            Optional ByRef vBankAccountNo As Object = Nothing,
                            Optional ByRef vBankAccountName As Object = Nothing,
                            Optional ByRef vDescription As Object = Nothing,
                            Optional ByRef vNextChequeNumber As Object = Nothing,
                            Optional ByRef vReconciledDate As Object = Nothing,
                            Optional ByRef vDefaultBankAccountID As Object = Nothing,
                            Optional ByRef vBankAccountTypeId As Object = Nothing,
                            Optional ByRef v_iIsCashReceiveInThisCurrencyOnly As Integer = 0,
                            Optional ByRef vStartChequeNumber As Object = Nothing,
                            Optional ByRef vFinancialInstitutionCode As Object = Nothing,
                            Optional ByRef vDirectDebitSupplierName As Object = Nothing,
                            Optional ByRef vDirectDebitSupplierID As Object = Nothing,
                            Optional ByRef vRemitter As Object = Nothing,
                            Optional ByRef vProcessingDays As Object = Nothing,
                            Optional ByRef r_sBIC As String = "",
                            Optional ByRef r_sIBAN As String = "") As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oBankAccount As bACTBankAccount.BankAccount
        Dim nStatus As Integer

        Try

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oBankAccounts.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                nResult = PMEReturnCode.PMEOF
            End If

            oBankAccount = m_oBankAccounts.Item(m_lCurrentRecord)

            m_lReturn = CType(GetProperties(oBankAccount,
                                            nStatus,
                                            vBankAccountId:=vBankAccountId,
                                            vCurrencyId:=vCurrencyId,
                                            vCompanyID:=vCompanyID,
                                            vAccountId:=vAccountId,
                                            vBankID:=vBankID,
                                            vCode:=vCode,
                                            vBankAccountNo:=vBankAccountNo,
                                            vBankAccountName:=vBankAccountName,
                                            vDescription:=vDescription,
                                            vNextChequeNumber:=vNextChequeNumber,
                                            vReconciledDate:=vReconciledDate,
                                            vDefaultBankAccountID:=vDefaultBankAccountID,
                                            vBankAccountTypeId:=vBankAccountTypeId,
                                            v_iIsCashReceiveInThisCurrencyOnly:=v_iIsCashReceiveInThisCurrencyOnly,
                                            vStartChequeNumber:=vStartChequeNumber,
                                            vFinancialInstitutionCode:=vFinancialInstitutionCode,
                                            vDirectDebitSupplierName:=vDirectDebitSupplierName,
                                            vDirectDebitSupplierID:=vDirectDebitSupplierID,
                                            vRemitter:=vRemitter,
                                            vProcessingDays:=vProcessingDays,
                                            r_sBIC:=r_sBIC,
                                            r_sIBAN:=r_sIBAN), gPMConstants.PMEReturnCode)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
            End If

            oBankAccount = Nothing
            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOtherDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(Optional ByRef vAccountHolderId As Object = Nothing, Optional ByRef vAccountHolderName As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim oACTBankAccount As bACTBankAccount.BankAccount


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - ned to go to core for agent
            oACTBankAccount = New bACTBankAccount.BankAccount()
            m_lReturn = CType(oACTBankAccount.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_oDatabase), gPMConstants.PMEReturnCode)





            If (Not Informations.IsNothing(vAccountHolderId)) And (Not Object.Equals(vAccountHolderId, Nothing)) And (Not (Convert.IsDBNull(vAccountHolderId) Or Informations.IsNothing(vAccountHolderId))) Then

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountHolderDetailsSQL & CStr(vAccountHolderId), sSQLName:=ACGetAccountHolderDetailsName, bStoredProcedure:=ACGetAccountHolderDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vAccountHolderName = ""
                Else

                    vAccountHolderName = CStr(vResultArray(0, 0))
                End If

                'put summat ere

            End If

            oACTBankAccount = Nothing




            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetOtherDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CheckAccount (Public)
    '
    ' Description: Gets the required BankAccounts and populate the Collection
    '
    ' ***************************************************************** '
    Public Function CheckAccount(ByRef vAccountId As Object, Optional ByRef vBankAccountId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vBankAccountId)) Or (Not Double.TryParse(CStr(vBankAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vBankAccountId = 0
            End If

            ' Add the  AccountID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_id", vValue:=CStr(vAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the BankAccountID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(vBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckACcountSQL, sSQLName:=ACCheckACcountName, bStoredProcedure:=ACCheckACcountStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount <> 0 Then
                ' If we find records we have have a potential duplicate
                result = gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Adds the supplied BankAccount into the Collection.
    ''' After the Add, lKey should be equal to the number of items in the collection.
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="vBankAccountId"></param>
    ''' <param name="vCurrencyId"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountId"></param>
    ''' <param name="vBankID"></param>
    ''' <param name="vCode"></param>
    ''' <param name="vBankAccountNo"></param>
    ''' <param name="vBankAccountName"></param>
    ''' <param name="vDescription"></param>
    ''' <param name="vNextChequeNumber"></param>
    ''' <param name="vReconciledDate"></param>
    ''' <param name="vDefaultBankAccountID"></param>
    ''' <param name="v_iIsCashReceiveInThisCurrencyOnly"></param>
    ''' <param name="vStartChequeNumber"></param>
    ''' <param name="vFinancialInstitutionCode"></param>
    ''' <param name="vDirectDebitSupplierName"></param>
    ''' <param name="vDirectDebitSupplierID"></param>
    ''' <param name="vRemitter"></param>
    ''' <param name="vProcessingDays"></param>
    ''' <param name="r_sBIC"></param>
    ''' <param name="r_sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditAdd(ByRef lRow As Integer,
                            Optional ByRef vBankAccountId As Object = Nothing,
                            Optional ByRef vCurrencyId As Object = Nothing,
                            Optional ByRef vCompanyID As Object = Nothing,
                            Optional ByRef vAccountId As Object = Nothing,
                            Optional ByRef vBankID As Object = Nothing,
                            Optional ByRef vCode As Object = Nothing,
                            Optional ByRef vBankAccountNo As Object = Nothing,
                            Optional ByRef vBankAccountName As Object = Nothing,
                            Optional ByRef vDescription As Object = Nothing,
                            Optional ByRef vNextChequeNumber As Object = Nothing,
                            Optional ByRef vReconciledDate As Object = Nothing,
                            Optional ByRef vDefaultBankAccountID As Object = Nothing,
                            Optional ByRef v_iIsCashReceiveInThisCurrencyOnly As Integer = 0,
                            Optional ByRef vStartChequeNumber As Object = Nothing,
                            Optional ByRef vFinancialInstitutionCode As Object = Nothing,
                            Optional ByRef vDirectDebitSupplierName As Object = Nothing,
                            Optional ByRef vDirectDebitSupplierID As Object = Nothing,
                            Optional ByRef vRemitter As Object = Nothing,
                            Optional ByRef vProcessingDays As Object = Nothing,
                            Optional ByRef r_sBIC As String = "",
                            Optional ByRef r_sIBAN As String = "",
                            Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oBankAccount As bACTBankAccount.BankAccount

        Try

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oBankAccounts.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oBankAccount = New bACTBankAccount.BankAccount()

            m_lReturn = CType(SetProperties(oBankAccount, gPMConstants.PMEComponentAction.PMAdd,
                                            vBankAccountId:=vBankAccountId,
                                            vCurrencyId:=vCurrencyId,
                                            vCompanyID:=vCompanyID,
                                            vAccountId:=vAccountId,
                                            vBankID:=vBankID,
                                            vCode:=vCode,
                                            vBankAccountNo:=vBankAccountNo,
                                            vBankAccountName:=vBankAccountName,
                                            vDescription:=vDescription,
                                            vNextChequeNumber:=vNextChequeNumber,
                                            vReconciledDate:=vReconciledDate,
                                            vDefaultBankAccountID:=vDefaultBankAccountID,
                                            v_iIsCashReceiveInThisCurrencyOnly:=v_iIsCashReceiveInThisCurrencyOnly,
                                            vStartChequeNumber:=vStartChequeNumber,
                                            vFinancialInstitutionCode:=vFinancialInstitutionCode,
                                            vDirectDebitSupplierName:=vDirectDebitSupplierName,
                                            vDirectDebitSupplierID:=vDirectDebitSupplierID,
                                            vRemitter:=vRemitter,
                                            vProcessingDays:=vProcessingDays,
                                            r_sBIC:=r_sBIC,
                                            r_sIBAN:=r_sIBAN,
                                            vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                oBankAccount = Nothing
                Return nResult
            End If

            ' Add BankAccount to collection
            If m_oBankAccounts.Count = 0 Then
                m_oBankAccounts.Add(Nothing)
            End If
            m_lReturn = CType(m_oBankAccounts.Add(oNewBankAccount:=oBankAccount), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                oBankAccount = Nothing
                Return nResult
            End If

            oBankAccount = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Validates that this action is valid on the BankAccount specified and updates the BankAccount with the new values.
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="vBankAccountId"></param>
    ''' <param name="vCurrencyId"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountId"></param>
    ''' <param name="vBankID"></param>
    ''' <param name="vCode"></param>
    ''' <param name="vBankAccountNo"></param>
    ''' <param name="vBankAccountName"></param>
    ''' <param name="vDescription"></param>
    ''' <param name="vNextChequeNumber"></param>
    ''' <param name="vReconciledDate"></param>
    ''' <param name="vDefaultBankAccountID"></param>
    ''' <param name="v_iIsCashReceiveInThisCurrencyOnly"></param>
    ''' <param name="vStartChequeNumber"></param>
    ''' <param name="vFinancialInstitutionCode"></param>
    ''' <param name="vDirectDebitSupplierName"></param>
    ''' <param name="vDirectDebitSupplierID"></param>
    ''' <param name="vRemitter"></param>
    ''' <param name="vProcessingDays"></param>
    ''' <param name="r_sBIC"></param>
    ''' <param name="r_sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditUpdate(ByRef lRow As Integer,
                               Optional ByRef vBankAccountId As Object = Nothing,
                               Optional ByRef vCurrencyId As Object = Nothing,
                               Optional ByRef vCompanyID As Object = Nothing,
                               Optional ByRef vAccountId As Object = Nothing,
                               Optional ByRef vBankID As Object = Nothing,
                               Optional ByRef vCode As Object = Nothing,
                               Optional ByRef vBankAccountNo As Object = Nothing,
                               Optional ByRef vBankAccountName As Object = Nothing,
                               Optional ByRef vDescription As Object = Nothing,
                               Optional ByRef vNextChequeNumber As Object = Nothing,
                               Optional ByRef vReconciledDate As Object = Nothing,
                               Optional ByRef vDefaultBankAccountID As Object = Nothing,
                               Optional ByRef v_iIsCashReceiveInThisCurrencyOnly As Integer = 0,
                               Optional ByRef vStartChequeNumber As Object = Nothing,
                               Optional ByRef vFinancialInstitutionCode As Object = Nothing,
                               Optional ByRef vDirectDebitSupplierName As Object = Nothing,
                               Optional ByRef vDirectDebitSupplierID As Object = Nothing,
                               Optional ByRef vRemitter As Object = Nothing,
                               Optional ByRef vProcessingDays As Object = Nothing,
                               Optional ByRef r_sBIC As String = "",
                               Optional ByRef r_sIBAN As String = "",
                               Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oBankAccount As bACTBankAccount.BankAccount
        Dim nStatus As PMEComponentAction

        Try

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oBankAccounts.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oBankAccount = m_oBankAccounts.Item(lRow)

            ' Check the Status of the BankAccount

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oBankAccount.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    nStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    nStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            m_lReturn = CType(SetProperties(oBankAccount,
                                            nStatus,
                                            vBankAccountId:=vBankAccountId,
                                            vCurrencyId:=vCurrencyId,
                                            vCompanyID:=vCompanyID,
                                            vAccountId:=vAccountId,
                                            vBankID:=vBankID,
                                            vCode:=vCode,
                                            vBankAccountNo:=vBankAccountNo,
                                            vBankAccountName:=vBankAccountName,
                                            vDescription:=vDescription,
                                            vNextChequeNumber:=vNextChequeNumber,
                                            vReconciledDate:=vReconciledDate,
                                            vDefaultBankAccountID:=vDefaultBankAccountID,
                                            v_iIsCashReceiveInThisCurrencyOnly:=v_iIsCashReceiveInThisCurrencyOnly,
                                            vStartChequeNumber:=vStartChequeNumber,
                                            vFinancialInstitutionCode:=vFinancialInstitutionCode,
                                            vDirectDebitSupplierName:=vDirectDebitSupplierName,
                                            vDirectDebitSupplierID:=vDirectDebitSupplierID,
                                            vRemitter:=vRemitter,
                                            vProcessingDays:=vProcessingDays,
                                            r_sBIC:=r_sBIC,
                                            r_sIBAN:=r_sIBAN,
                                            vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                oBankAccount = Nothing
                Return nResult
            End If

            oBankAccount = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified BankAccount can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oBankAccount As bACTBankAccount.BankAccount

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oBankAccounts.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oBankAccount = m_oBankAccounts.Item(lRow)

            ' Check the Status of the BankAccount

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oBankAccount.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oBankAccount.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oBankAccount.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            oBankAccount.UniqueId = sUniqueId
            oBankAccount.ScreenHierarchy = sScreenHierarchy

            ' Release reference to BankAccount
            oBankAccount = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oBankAccounts.Count()
                Select Case m_oBankAccounts.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oBankAccount As bACTBankAccount.BankAccount
        Dim bTransStarted, bSourceAttached As Boolean
        Dim vArray As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oBankAccounts.Count()
                oBankAccount = m_oBankAccounts.Item(lSub)


                Select Case oBankAccount.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oBankAccount), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oBankAccount), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        'PN9167 Check That its OK to  Delete Item
                        m_lReturn = CType(CheckDeleteOK(oBankAccount), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMError
                            'PN4619
                            m_lReturn = CType(SetBankAccountIsDeleted(oBankAccount), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If
                            'PN49619End
                            Exit For
                        End If
                        'PN9167
                        m_lReturn = CType(SelectBankAccountDelay(lBankAccountID:=oBankAccount.BankAccountID, lMediaTypeID:=0, r_vBankAccountDelay:=vArray), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Informations.IsArray(vArray) Then
                            result = gPMConstants.PMEReturnCode.PMFail
                            Exit For
                        End If

                        ' Check if any source is attached with the branch.
                        m_lReturn = CType(CheckSourceAttachedWithBank(v_lBankAccountID:=oBankAccount.BankAccountID, r_bIsSourceAttachedWithBank:=bSourceAttached), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If bSourceAttached Then
                            Return gPMConstants.PMEReturnCode.PMRecordInUse
                        End If

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(DeleteItem(oBankAccount), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oBankAccount = Nothing

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
                    Do While lSub <= m_oBankAccounts.Count()

                        ' With the item
                        With m_oBankAccounts.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oBankAccounts.Delete(lSub)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oBankAccount:=oBankAccount), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add BankAccountID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(oBankAccount.BankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oBankAccount.BankAccountID = m_oDatabase.Parameters.Item("BankAccount_id").Value
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lBankACcountId = oBankAccount.BankAccountID
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oBankAccount:=oBankAccount), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add BankAccountID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(oBankAccount.BankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oBankAccount.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn& <> PMTrue) Then
        '    UpdateItem = PMFalse
        '    Exit Function
        'End If

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

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the BankAccountID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(oBankAccount.BankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=oBankAccount.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=oBankAccount.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Commented by Santosh Singh as on 11-Jan-2010 to fix the PN No.62453
        'This condition leads to error because if the respective SP does not delete rows
        'based on the specified condition.Ex. rows which needs to delete does not exists in respective table.

        'If record wasn't deleted, error
        'If (lRecordsAffected& > 0) Then
        'Deleted, No action required
        'Else
        'DeleteItem = PMFalse
        'Exit Function
        'End If
        'Comments Ends Here by santosh as on 11-Jan-2010
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CheckDeleteOK (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' 'PN9167 Check that it is OK to delete bank account
    ' ***************************************************************** '
    Private Function CheckDeleteOK(ByRef oBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the BankAccountID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(oBankAccount.BankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckDeleteOkSQL, sSQLName:=ACCheckDeleteOkName, bStoredProcedure:=ACCheckDeleteOkStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: SetBankAccountIsDeleted (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' 'PN4619 If Bank Account has cash just set the is_deleted flag
    ' ***************************************************************** '
    Private Function SetBankAccountIsDeleted(ByRef oBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the BankAccountID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(oBankAccount.BankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSetBankAccountIsDeletedSQL, sSQLName:=ACSetBankAccountIsDeletedName, bStoredProcedure:=ACSetBankAccountIsDeletedStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ''' <summary>
    ''' Sets the supplied BankAccount properties from a database record.
    ''' </summary>
    ''' <param name="oBankAccount"></param>
    ''' <param name="lRecordNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetPropertiesFromDB(ByRef oBankAccount As bACTBankAccount.BankAccount, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Dim oFields As DataRow

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()


        With oBankAccount

            .BankAccountID = oFields("bankaccount_id")

            If Convert.IsDBNull(oFields("currency_id")) Or Informations.IsNothing(oFields("currency_id")) Then
                .CurrencyID = 0
            Else
                .CurrencyID = oFields("currency_id")
            End If

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .CompanyID = 0
            Else
                .CompanyID = oFields("company_id")
            End If

            If Convert.IsDBNull(oFields("account_id")) Or Informations.IsNothing(oFields("account_id")) Then
                .AccountID = 0
            Else
                .AccountID = oFields("account_id")
            End If

            If Convert.IsDBNull(oFields("bank_id")) Or Informations.IsNothing(oFields("bank_id")) Then
                .BankID = 0
            Else
                .BankID = oFields("bank_id")
            End If

            If Convert.IsDBNull(oFields("code")) Or Informations.IsNothing(oFields("code")) Then
                .Code = ""
            Else
                .Code = oFields("code")
            End If

            If Convert.IsDBNull(oFields("bank_account_no")) Or Informations.IsNothing(oFields("bank_account_no")) Then
                .BankAccountNo = ""
            Else
                .BankAccountNo = oFields("bank_account_no")
            End If

            If Convert.IsDBNull(oFields("bank_account_name")) Or Informations.IsNothing(oFields("bank_account_name")) Then
                .BankAccountName = ""
            Else
                .BankAccountName = oFields("bank_account_name")
            End If

            If Convert.IsDBNull(oFields("description")) Or Informations.IsNothing(oFields("description")) Then
                .Description = ""
            Else
                .Description = oFields("description")
            End If


            If Convert.IsDBNull(oFields("next_cheque_number")) Or Informations.IsNothing(oFields("next_cheque_number")) Then
                .NextChequeNumber = 0
            Else
                .NextChequeNumber = oFields("next_cheque_number")
            End If


            If Convert.IsDBNull(oFields("reconciled_date")) Or Informations.IsNothing(oFields("reconciled_date")) Then

                .ReconciledDate = "01/01/1899"
            Else

                .ReconciledDate = oFields("reconciled_date")
            End If


            If Convert.IsDBNull(oFields("default_bank_account_id")) Or Informations.IsNothing(oFields("default_bank_account_id")) Then
                .DefaultBankAccountID = 0
            Else
                .DefaultBankAccountID = oFields("default_bank_account_id")
            End If


            If Convert.IsDBNull(oFields("bank_account_type_id")) Or Informations.IsNothing(oFields("bank_account_type_id")) Then
                .BankAccountTypeId = 0
            Else
                .BankAccountTypeId = oFields("bank_account_type_id")
            End If


            If Convert.IsDBNull(oFields("is_cash_receive_in_this_currency_only")) Or Informations.IsNothing(oFields("is_cash_receive_in_this_currency_only")) Then
                .IsCashReceiveInThisCurrencyOnly = 0
            Else
                .IsCashReceiveInThisCurrencyOnly = oFields("is_cash_receive_in_this_currency_only")
            End If


            If Convert.IsDBNull(oFields("start_cheque_number")) Or Informations.IsNothing(oFields("start_cheque_number")) Then
                .StartChequeNumber = ""
            Else
                .StartChequeNumber = oFields("start_cheque_number")
            End If

            If Convert.IsDBNull(oFields("financial_institution_code")) Or Informations.IsNothing(oFields("financial_institution_code")) Then
                .FinancialInstitutionCode = ""
            Else
                .FinancialInstitutionCode = oFields("financial_institution_code")
            End If


            If Convert.IsDBNull(oFields("direct_debit_supplier_name")) Or Informations.IsNothing(oFields("direct_debit_supplier_name")) Then
                .DirectDebitSupplierName = ""
            Else
                .DirectDebitSupplierName = oFields("direct_debit_supplier_name")
            End If

            If Convert.IsDBNull(oFields("direct_debit_supplier_id")) Or Informations.IsNothing(oFields("direct_debit_supplier_id")) Then
                .DirectDebitSupplierID = 0
            Else
                .DirectDebitSupplierID = oFields("direct_debit_supplier_id")
            End If

            If Convert.IsDBNull(oFields("remitter")) Or Informations.IsNothing(oFields("remitter")) Then
                .Remitter = ""
            Else
                .Remitter = oFields("remitter")
            End If

            If Convert.IsDBNull(oFields("processing_days")) Or Informations.IsNothing(oFields("processing_days")) Then
                .ProcessingDays = 0
            Else
                .ProcessingDays = oFields("processing_days")
            End If


            If Convert.IsDBNull(oFields("business_identifier_code")) Or Informations.IsNothing(oFields("business_identifier_code")) Then
                .BIC = String.Empty
            Else
                .BIC = oFields("business_identifier_code")
            End If


            If Convert.IsDBNull(oFields("international_bank_account_number")) Or Informations.IsNothing(oFields("international_bank_account_number")) Then
                .IBAN = String.Empty
            Else
                .IBAN = oFields("international_bank_account_number")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ''' <summary>
    ''' Sets the supplied BankAccount property values.
    ''' </summary>
    ''' <param name="oBankAccount"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vBankAccountId"></param>
    ''' <param name="vCurrencyId"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountId"></param>
    ''' <param name="vBankID"></param>
    ''' <param name="vCode"></param>
    ''' <param name="vBankAccountNo"></param>
    ''' <param name="vBankAccountName"></param>
    ''' <param name="vDescription"></param>
    ''' <param name="vNextChequeNumber"></param>
    ''' <param name="vReconciledDate"></param>
    ''' <param name="vDefaultBankAccountID"></param>
    ''' <param name="v_iIsCashReceiveInThisCurrencyOnly"></param>
    ''' <param name="vStartChequeNumber"></param>
    ''' <param name="vFinancialInstitutionCode"></param>
    ''' <param name="vDirectDebitSupplierName"></param>
    ''' <param name="vDirectDebitSupplierID"></param>
    ''' <param name="vRemitter"></param>
    ''' <param name="vProcessingDays"></param>
    ''' <param name="r_sBIC"></param>
    ''' <param name="r_sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetProperties(ByRef oBankAccount As bACTBankAccount.BankAccount,
                                   ByRef iStatus As Integer,
                                   Optional ByRef vBankAccountId As Object = Nothing,
                                   Optional ByRef vCurrencyId As Object = Nothing,
                                   Optional ByRef vCompanyID As Object = Nothing,
                                   Optional ByRef vAccountId As Object = Nothing,
                                   Optional ByRef vBankID As Object = Nothing,
                                   Optional ByRef vCode As Object = Nothing,
                                   Optional ByRef vBankAccountNo As Object = Nothing,
                                   Optional ByRef vBankAccountName As Object = Nothing,
                                   Optional ByRef vDescription As Object = Nothing,
                                   Optional ByRef vNextChequeNumber As Object = Nothing,
                                   Optional ByRef vReconciledDate As Object = Nothing,
                                   Optional ByRef vDefaultBankAccountID As Object = Nothing,
                                   Optional ByRef v_iIsCashReceiveInThisCurrencyOnly As Integer = 0,
                                   Optional ByRef vStartChequeNumber As Object = Nothing,
                                   Optional ByRef vFinancialInstitutionCode As Object = Nothing,
                                   Optional ByRef vDirectDebitSupplierName As Object = Nothing,
                                   Optional ByRef vDirectDebitSupplierID As Object = Nothing,
                                   Optional ByRef vRemitter As Object = Nothing,
                                   Optional ByRef vProcessingDays As Object = Nothing,
                                   Optional ByRef r_sBIC As Object = "",
                                   Optional ByRef r_sIBAN As Object = "",
                                   Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bDataChanged As Boolean

        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then
            m_lReturn = CType(CheckMandatory(vBankAccountId:=vBankAccountId, vCurrencyId:=vCurrencyId, vCompanyID:=vCompanyID, vAccountId:=vAccountId, vBankID:=vBankID, vCode:=vCode, vBankAccountNo:=vBankAccountNo, vBankAccountName:=vBankAccountName, vDescription:=vDescription, vNextChequeNumber:=vNextChequeNumber, vDefaultBankAccountID:=vDefaultBankAccountID, vStartChequeNumber:=vStartChequeNumber), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vBankAccountId:=vBankAccountId, vCurrencyId:=vCurrencyId, vCompanyID:=vCompanyID, vAccountId:=vAccountId, vBankID:=vBankID, vCode:=vCode, vBankAccountNo:=vBankAccountNo, vBankAccountName:=vBankAccountName, vDescription:=vDescription, vNextChequeNumber:=vNextChequeNumber, vReconciledDate:=vReconciledDate, vDefaultBankAccountID:=vDefaultBankAccountID, vStartChequeNumber:=vStartChequeNumber), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
        End If

        m_lReturn = CType(Validate(vBankAccountId:=vBankAccountId, vCurrencyId:=vCurrencyId, vCompanyID:=vCompanyID, vAccountId:=vAccountId, vBankID:=vBankID, vCode:=vCode, vBankAccountNo:=vBankAccountNo, vBankAccountName:=vBankAccountName, vDescription:=vDescription, vNextChequeNumber:=vNextChequeNumber, vReconciledDate:=vReconciledDate, vDefaultBankAccountID:=vDefaultBankAccountID, vStartChequeNumber:=vStartChequeNumber), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oBankAccount

            If Not Informations.IsNothing(vBankAccountId) Then
                If .BankAccountID <> vBankAccountId Then
                    .BankAccountID = vBankAccountId
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyId) Then
                If .CurrencyID <> vCurrencyId Then
                    .CurrencyID = vCurrencyId
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAccountId) Then
                If .AccountID <> vAccountId Then
                    .AccountID = vAccountId
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vBankID) Then
                If .BankID <> vBankID Then
                    .BankID = vBankID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCode) Then
                If .Code.Trim() <> vCode.Trim() Then
                    .Code = vCode
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vBankAccountNo) Then
                If .BankAccountNo.Trim() <> vBankAccountNo.Trim() Then
                    .BankAccountNo = vBankAccountNo
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vBankAccountName) Then
                If .BankAccountName.Trim() <> vBankAccountName.Trim() Then
                    .BankAccountName = vBankAccountName
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDescription) Then
                If .Description.Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vNextChequeNumber) Then
                If CStr(.NextChequeNumber).Trim() <> vNextChequeNumber Then
                    .NextChequeNumber = CInt(vNextChequeNumber)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vReconciledDate) Then
                If Not .ReconciledDate.Equals(vReconciledDate) Then
                    .ReconciledDate = vReconciledDate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDefaultBankAccountID) Then
                If .DefaultBankAccountID <> vDefaultBankAccountID Then
                    .DefaultBankAccountID = vDefaultBankAccountID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vStartChequeNumber) And ToSafeString(vStartChequeNumber) <> "0" Then
                If .StartChequeNumber.Trim() <> vStartChequeNumber Then
                    .StartChequeNumber = vStartChequeNumber
                    bDataChanged = True
                End If
            End If

            If Not False Then
                If .IsCashReceiveInThisCurrencyOnly <> v_iIsCashReceiveInThisCurrencyOnly Then
                    .IsCashReceiveInThisCurrencyOnly = v_iIsCashReceiveInThisCurrencyOnly
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vFinancialInstitutionCode) Then
                If .FinancialInstitutionCode.Trim() <> vFinancialInstitutionCode Then
                    .FinancialInstitutionCode = vFinancialInstitutionCode
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDirectDebitSupplierName) Then
                If .DirectDebitSupplierName.Trim() <> vDirectDebitSupplierName Then
                    .DirectDebitSupplierName = vDirectDebitSupplierName
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDirectDebitSupplierID) Then
                If CStr(.DirectDebitSupplierID).Trim() <> vDirectDebitSupplierID Then
                    .DirectDebitSupplierID = CInt(vDirectDebitSupplierID)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vRemitter) Then
                If .Remitter.Trim() <> vRemitter Then
                    .Remitter = vRemitter
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vProcessingDays) Then
                If CStr(.ProcessingDays).Trim() <> vProcessingDays Then
                    .ProcessingDays = CInt(vProcessingDays)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(r_sBIC) Then
                If .BIC.Trim() <> r_sBIC Then
                    .BIC = r_sBIC
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(r_sIBAN) Then
                If .IBAN.Trim() <> r_sIBAN Then
                    .IBAN = r_sIBAN
                    bDataChanged = True
                End If
            End If

            If Not String.IsNullOrEmpty(vUniqueId) Then
                .UniqueId = vUniqueId
                .ScreenHierarchy = vScreenHierarchy
            End If
            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return nResult

    End Function

    ''' <summary>
    ''' Returns the supplied BankAccount property values.
    ''' </summary>
    ''' <param name="oBankAccount"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vBankAccountId"></param>
    ''' <param name="vCurrencyId"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vAccountId"></param>
    ''' <param name="vBankID"></param>
    ''' <param name="vCode"></param>
    ''' <param name="vBankAccountNo"></param>
    ''' <param name="vBankAccountName"></param>
    ''' <param name="vDescription"></param>
    ''' <param name="vNextChequeNumber"></param>
    ''' <param name="vReconciledDate"></param>
    ''' <param name="vDefaultBankAccountID"></param>
    ''' <param name="vBankAccountTypeId"></param>
    ''' <param name="v_iIsCashReceiveInThisCurrencyOnly"></param>
    ''' <param name="vStartChequeNumber"></param>
    ''' <param name="vFinancialInstitutionCode"></param>
    ''' <param name="vDirectDebitSupplierName"></param>
    ''' <param name="vDirectDebitSupplierID"></param>
    ''' <param name="vRemitter"></param>
    ''' <param name="vProcessingDays"></param>
    ''' <param name="r_sBIC"></param>
    ''' <param name="r_sIBAN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetProperties(ByRef oBankAccount As bACTBankAccount.BankAccount,
                                   ByRef iStatus As Integer,
                                   Optional ByRef vBankAccountId As Integer = 0,
                                   Optional ByRef vCurrencyId As Integer = 0,
                                   Optional ByRef vCompanyID As Integer = 0,
                                   Optional ByRef vAccountId As Integer = 0,
                                   Optional ByRef vBankID As Integer = 0,
                                   Optional ByRef vCode As String = "",
                                   Optional ByRef vBankAccountNo As String = "",
                                   Optional ByRef vBankAccountName As String = "",
                                   Optional ByRef vDescription As String = "",
                                   Optional ByRef vNextChequeNumber As Integer = 0,
                                   Optional ByRef vReconciledDate As Object = Nothing,
                                   Optional ByRef vDefaultBankAccountID As Integer = 0,
                                   Optional ByRef vBankAccountTypeId As Integer = 0,
                                   Optional ByRef v_iIsCashReceiveInThisCurrencyOnly As Integer = 0,
                                   Optional ByRef vStartChequeNumber As String = "",
                                   Optional ByRef vFinancialInstitutionCode As String = "",
                                   Optional ByRef vDirectDebitSupplierName As String = "",
                                   Optional ByRef vDirectDebitSupplierID As Integer = 0,
                                   Optional ByRef vRemitter As String = "",
                                   Optional ByRef vProcessingDays As Integer = 0,
                                   Optional ByRef r_sBIC As String = "",
                                   Optional ByRef r_sIBAN As String = "") As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        With oBankAccount
            vBankAccountId = .BankAccountID
            vCurrencyId = .CurrencyID
            vCompanyID = .CompanyID
            vAccountId = .AccountID
            vBankID = .BankID
            vCode = .Code
            vBankAccountNo = .BankAccountNo
            vBankAccountName = .BankAccountName
            vDescription = .Description
            vNextChequeNumber = .NextChequeNumber
            vReconciledDate = .ReconciledDate
            vDefaultBankAccountID = .DefaultBankAccountID
            vBankAccountTypeId = .BankAccountTypeId
            v_iIsCashReceiveInThisCurrencyOnly = .IsCashReceiveInThisCurrencyOnly
            vStartChequeNumber = .StartChequeNumber
            vFinancialInstitutionCode = .FinancialInstitutionCode
            vDirectDebitSupplierName = .DirectDebitSupplierName
            vDirectDebitSupplierID = .DirectDebitSupplierID
            vRemitter = .Remitter
            vProcessingDays = .ProcessingDays
            r_sBIC = .BIC
            r_sIBAN = .IBAN
            iStatus = .DatabaseStatus
        End With

        Return nResult

    End Function

    ''' <summary>
    ''' Adds all of the INPUT parameters required for an Insert or Update.
    ''' </summary>
    ''' <param name="oBankAccount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddInputParam(ByRef oBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        With m_oDatabase

            If oBankAccount.CurrencyID < 1 Then
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(oBankAccount.CurrencyID),
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oBankAccount.AccountID < 1 Then
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(oBankAccount.AccountID),
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oBankAccount.BankID < 1 Then
                m_lReturn = .Parameters.Add(sName:="bank_id", vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="bank_id", vValue:=CStr(oBankAccount.BankID),
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oBankAccount.Code,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_account_no", vValue:=oBankAccount.BankAccountNo,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_account_name", vValue:=oBankAccount.BankAccountName,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oBankAccount.Description,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="next_cheque_number", vValue:=CStr(oBankAccount.NextChequeNumber),
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reconciled_date", vValue:=oBankAccount.ReconciledDate,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oBankAccount.DefaultBankAccountID < 1 Then
                m_lReturn = .Parameters.Add(sName:="default_bank_account_id", vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="default_bank_account_id", vValue:=CStr(oBankAccount.DefaultBankAccountID),
                                            iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
            End If

            m_lReturn = .Parameters.Add(sName:="is_cash_receive_in_this_currency_only", vValue:=CStr(oBankAccount.IsCashReceiveInThisCurrencyOnly),
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="start_cheque_number", vValue:=oBankAccount.StartChequeNumber,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="financial_institution_code", vValue:=oBankAccount.FinancialInstitutionCode,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="direct_debit_supplier_name", vValue:=oBankAccount.DirectDebitSupplierName,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="direct_debit_supplier_id", vValue:=CStr(oBankAccount.DirectDebitSupplierID),
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="remitter", vValue:=oBankAccount.Remitter,
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="processing_days", vValue:=CStr(oBankAccount.ProcessingDays),
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="sBusinessIdentifierCode", vValue:=CStr(oBankAccount.BIC),
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="sInternationalBankAccountNumber", vValue:=CStr(oBankAccount.IBAN),
                                        iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=oBankAccount.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=oBankAccount.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a BankAccount.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, Optional ByRef vReconciledDate As Object = Nothing, Optional ByRef vDefaultBankAccountID As Object = Nothing, Optional ByRef vStartChequeNumber As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vBankAccountId)) OrElse (vBankAccountId.Equals(0)) Or (bDefaultAll) Then
            vBankAccountId = 0
        End If



        If (Informations.IsNothing(vCurrencyId)) OrElse (vCurrencyId.Equals(0)) Or (bDefaultAll) Then
            vCurrencyId = 0
        End If



        If (Informations.IsNothing(vCompanyID)) OrElse (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vAccountId)) OrElse (vAccountId.Equals(0)) Or (bDefaultAll) Then
            vAccountId = 0
        End If



        If (Informations.IsNothing(vBankID)) OrElse (vBankID.Equals(0)) Or (bDefaultAll) Then
            vBankID = 0
        End If



        If (Informations.IsNothing(vCode)) OrElse (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Informations.IsNothing(vBankAccountNo)) OrElse (String.IsNullOrEmpty(vBankAccountNo)) Or (bDefaultAll) Then
            vBankAccountNo = ""
        End If



        If (Informations.IsNothing(vBankAccountName)) OrElse (String.IsNullOrEmpty(vBankAccountName)) Or (bDefaultAll) Then
            vBankAccountName = ""
        End If



        If (Informations.IsNothing(vDescription)) OrElse (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Informations.IsNothing(vNextChequeNumber)) OrElse (vNextChequeNumber.Equals(0)) Or (bDefaultAll) Then
            vNextChequeNumber = 0
        End If



        If (Informations.IsNothing(vReconciledDate)) OrElse (Object.Equals(vReconciledDate, Nothing)) Or (bDefaultAll) Then


            'Modified,change DBNull.Value to Nothing
            'vReconciledDate = DBNull.Value
            vReconciledDate = Nothing
        End If



        If Informations.IsNothing(vDefaultBankAccountID) OrElse vDefaultBankAccountID.Equals(0) Or (bDefaultAll) Then
            vDefaultBankAccountID = 0
        End If



        If (Informations.IsNothing(vStartChequeNumber)) OrElse (String.IsNullOrEmpty(vStartChequeNumber)) Or (bDefaultAll) Then
            vStartChequeNumber = "0"
        End If
        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a BankAccount.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, Optional ByRef vDefaultBankAccountID As Object = Nothing, Optional ByRef vStartChequeNumber As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vBankAccountId)) Or (Object.Equals(vBankAccountId, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyId)) Or (Object.Equals(vCurrencyId, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAccountId)) Or (Object.Equals(vAccountId, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBankID)) Or (Object.Equals(vBankID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCode)) Or (Object.Equals(vCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBankAccountNo)) Or (Object.Equals(vBankAccountNo, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBankAccountName)) Or (Object.Equals(vBankAccountName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If Informations.IsNothing(vDefaultBankAccountID) Or Object.Equals(vDefaultBankAccountID, Nothing) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        If (Informations.IsNothing(vStartChequeNumber)) Or (Object.Equals(vStartChequeNumber, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If
        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the BankAccount for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vBankAccountId As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vAccountId As Object = Nothing, Optional ByRef vBankID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBankAccountNo As Object = Nothing, Optional ByRef vBankAccountName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNextChequeNumber As Object = Nothing, Optional ByRef vReconciledDate As Object = Nothing, Optional ByRef vDefaultBankAccountID As Object = Nothing, Optional ByRef vStartChequeNumber As String = "") As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vBankAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCurrencyId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vAccountId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vBankID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(vDefaultBankAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Dim dbNumericTemp7 As Double
        If Not Double.TryParse(vStartChequeNumber, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) And vStartChequeNumber <> "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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



    ' ***************************************************************** '
    ' Name: GetBankAccountRules (Public)
    '
    ' Description: Gets the rules for the given bank accont
    '
    ' 31/01/2003 Steve Watton
    ' ***************************************************************** '

    Public Function GetBankAccountRules(ByVal v_lBankAccountID As Integer, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("bankaccountid", CStr(v_lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.SQLSelect(sSQL:=ACGetBankAccountRulesSQL, sSQLName:=ACGetBankAccountRulesName, bStoredProcedure:=True, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBankAccountRules Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankAccountRules", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AddBankAccountRule (Public)
    '
    ' Description: Adds a new bank account rules record to the db
    '
    ' 31/01/2003 Steve Watton
    ' ***************************************************************** '

    Public Function AddBankAccountRule(ByRef r_lBankAccountRuleID As Integer, ByRef v_lBankAccountID As Integer, ByRef v_lMediaTypeID As Integer, ByRef v_iMatchToTransdetail As Integer, ByRef v_iMatchAccountCode As Integer, ByRef v_iCodeIsMerchantNumber As Integer, ByRef v_iMatchBatchNumber As Integer, ByRef v_iBatchIsRemitCode As Integer, ByRef v_iMatchChequeNumber As Integer, ByRef v_iMatchAmount As Integer, ByRef v_iMatchDate As Integer, ByRef v_iSkipIfReasonNull As Integer, ByRef v_iActive As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("BankAccountRulesID", CStr(r_lBankAccountRuleID), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("BankAccountID", CStr(v_lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MediaTypeID", CStr(v_lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchToTransdetail", CStr(v_iMatchToTransdetail), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchAccountCode", CStr(v_iMatchAccountCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("CodeIsMerchantNumber", CStr(v_iCodeIsMerchantNumber), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchBatchNumber", CStr(v_iMatchBatchNumber), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("BatchIsRemitCode", CStr(v_iBatchIsRemitCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchChequeNumber", CStr(v_iMatchChequeNumber), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchAmount", CStr(v_iMatchAmount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchDate", CStr(v_iMatchDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("SkipIfReasonNull", CStr(v_iSkipIfReasonNull), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("Active", CStr(v_iActive), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.SQLAction(sSQL:=ACAddBankAccountRuleSQL, sSQLName:=ACAddBankAccountRuleName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            r_lBankAccountRuleID = m_oDatabase.Parameters.Item("BankAccountRulesID").Value


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddBankAccountRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBankAccountRule", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateBankAccountRule (Public)
    '
    ' Description: updates a bank account rules record direct to db
    '
    ' 31/01/2003 Steve Watton
    ' ***************************************************************** '

    Public Function UpdateBankAccountRule(ByRef v_lBankAccountRuleID As Integer, ByRef v_lBankAccountID As Integer, ByRef v_lMediaTypeID As Integer, ByRef v_iMatchToTransdetail As Integer, ByRef v_iMatchAccountCode As Integer, ByRef v_iCodeIsMerchantNumber As Integer, ByRef v_iMatchBatchNumber As Integer, ByRef v_iBatchIsRemitCode As Integer, ByRef v_iMatchChequeNumber As Integer, ByRef v_iMatchAmount As Integer, ByRef v_iMatchDate As Integer, ByRef v_iSkipIfReasonNull As Integer, ByRef v_iActive As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("BankAccountRulesID", CStr(v_lBankAccountRuleID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("BankAccountID", CStr(v_lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MediaTypeID", CStr(v_lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchToTransdetail", CStr(v_iMatchToTransdetail), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchAccountCode", CStr(v_iMatchAccountCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("CodeIsMerchantNumber", CStr(v_iCodeIsMerchantNumber), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchBatchNumber", CStr(v_iMatchBatchNumber), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("BatchIsRemitCode", CStr(v_iBatchIsRemitCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchChequeNumber", CStr(v_iMatchChequeNumber), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchAmount", CStr(v_iMatchAmount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("MatchDate", CStr(v_iMatchDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("SkipIfReasonNull", CStr(v_iSkipIfReasonNull), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("Active", CStr(v_iActive), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.SQLAction(sSQL:=ACUpdateBankAccountRuleSQL, sSQLName:=ACUpdateBankAccountRuleName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBankAccountRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBankAccountRule", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteBankAccountRule (Public)
    '
    ' Description: Deletes a rules record
    '
    ' 31/01/2003 Steve Watton
    ' ***************************************************************** '

    Public Function DeleteBankAccountRule(ByVal v_lBankAccountRuleID As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("bankaccountrulesid", CStr(v_lBankAccountRuleID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.SQLAction(sSQL:=ACDeleteBankAccountRuleSQL, sSQLName:=ACDeleteBankAccountRuleName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteBankAccountRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBankAccountRule", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DeleteAllRulesForAccount (Public)
    '
    ' Description: Deletes a rules record
    '
    ' 31/01/2003 Steve Watton
    ' ***************************************************************** '

    'UPGRADE_NOTE: (7001) The following declaration (DeleteAllRulesForAccount) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DeleteAllRulesForAccount(ByVal v_lBankAccountID As Integer) As Integer
    '
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'm_oDatabase.Parameters.Clear()
    '
    'If m_oDatabase.Parameters.Add("bankaccountid", CStr(v_lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then 'Return result
    '
    'If m_oDatabase.SQLAction(sSQL:=ACDeleteAllRulesForAccountSQL, sSQLName:=ACDeleteAllRulesForAccountName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then 'Return result
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllRulesForAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllRulesForAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: GetBankStatementBalance (Public)
    '
    ' Description: Gets the Bank Statement Balance field for the
    ' given bank account
    '
    ' DD 07/10/2003
    ' ***************************************************************** '

    Public Function GetBankStatementBalance(ByVal lAccountID As Integer, ByRef r_cBalance As Decimal) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase
                .Parameters.Clear()

                If .Parameters.Add("account_id", CStr(lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


                'developer guide no.85
                If .Parameters.Add("balance", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

                If .SQLSelect(sSQL:=ACGetBankStatementBalanceSQL, sSQLName:=ACGetBankStatementBalanceName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

                'Get the balance
                r_cBalance = gPMFunctions.NullToCurrency(.Parameters.Item("balance").Value)
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBankStatementBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankStatementBalance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateBankStatementBalance (Public)
    '
    ' Description: Updates the bank_statement_balance field on the
    ' Bank Account
    '
    ' 31/01/2003 Steve Watton
    ' ***************************************************************** '

    Public Function UpdateBankStatementBalance(ByVal lAccountID As Integer, ByVal cBalance As Decimal) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase
                .Parameters.Clear()

                If .Parameters.Add("account_id", CStr(lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

                If .Parameters.Add("balance", CStr(cBalance), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

                If .SQLAction(sSQL:=ACUpdateBankStatementBalanceSQL, sSQLName:=ACUpdateBankStatementBalanceName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBankStatementBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBankStatementBalance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Selects the BankAccount_Delay records for a Bank Account (and Media Type if > 0)
    ''' </summary>
    ''' <param name="lBankAccountID"></param>
    ''' <param name="lMediaTypeID"></param>
    ''' <param name="r_vBankAccountDelay"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelectBankAccountDelay(ByVal lBankAccountID As Integer, ByVal lMediaTypeID As Integer, ByRef r_vBankAccountDelay As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("bankaccount_id", CStr(lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("mediatype_id", gPMFunctions.ZeroToNull(CStr(lMediaTypeID)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                nResult = .SQLSelect("spu_ACT_Select_BankAccount_Delay", "Select Bank Account Delay", True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vBankAccountDelay)
            End With

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectBankAccountDelay", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
                    nResult = gPMConstants.PMEReturnCode.PMFalse
            End Select

        Finally

        End Try
        Return nResult
    End Function


    Public Function AddBankAccountDelay(ByVal lBankAccountID As Object, ByVal lMediaTypeID As Object, ByVal iDelay As Integer, Optional ByRef r_lBankAccountDelayID As Integer = 0, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddBankAccountDelay
        ' PURPOSE: Adds a BankAccount_Delay record
        ' AUTHOR: Danny Davis
        ' DATE: 02 October 2006, 11:28:58
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                .Parameters.Add("bankaccount_id", CStr(lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                .Parameters.Add("mediatype_id", CStr(lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("delay", CStr(iDelay), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                .Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                .Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                'developer guide no.85
                .Parameters.Add("bankaccount_delay_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                result = .SQLAction("spu_ACT_Add_BankAccount_Delay", "Add Bank Account Delay", True)
                r_lBankAccountDelayID = .Parameters.Item("bankaccount_delay_id").Value
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBankAccountDelay", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function


    Public Function UpdateBankAccountDelay(ByVal lBankAccountDelayID As Integer, ByVal lBankAccountID As Object, ByVal lMediaTypeID As Object, ByVal iDelay As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UpdateBankAccountDelay
        ' PURPOSE: Updates a BankAccount_Delay record
        ' AUTHOR: Danny Davis
        ' DATE: 02 October 2006, 11:28:58
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("bankaccount_delay_id", CStr(lBankAccountDelayID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                .Parameters.Add("bankaccount_id", CStr(lBankAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                .Parameters.Add("mediatype_id", CStr(lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("delay", CStr(iDelay), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                .Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                .Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                result = .SQLAction("spu_ACT_Update_BankAccount_Delay", "Update Bank Account Delay", True)
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBankAccountDelay", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function


    Public Function DeleteBankAccountDelay(ByVal lBankAccountDelayID As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DeleteBankAccountDelay
        ' PURPOSE: Delete a BankAccount_Delay record
        ' AUTHOR: Danny Davis
        ' DATE: 02 October 2006, 11:28:58
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("bankaccount_delay_id", CStr(lBankAccountDelayID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                .Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                .Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                result = .SQLAction("spu_ACT_Delete_BankAccount_Delay", "Delete Bank Account Delay", True)
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBankAccountDelay", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 16/05/2008 PK Created.

    ' Task: Account Function and CCY Cash Allocation
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "PickListLoad"

        Dim iParam As Integer = 0
        Dim lBankAccountID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the value of BankAccountID
            lBankAccountID = gPMFunctions.ToSafeLong(vFKArray(1, 0))

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and populate the linked sources in vResultArray
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourcesListForPickListSQL, sSQLName:=ACGetSourcesListForPickListName, bStoredProcedure:=ACGetSourcesListForPickListStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get sources linked with Bank ", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListSave
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History: 16/05/2008 PK Created.

    ' Task: Account Function and CCY Cash Allocation
    '
    ' ***************************************************************** '
    'Developer Guide no.101
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "PickListSave"

        Try

            Dim lBankAccountID, lSourceID, lRecordsAffected As Integer



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            'First Delete the existing records for This bankID (lBankAccountID)

            lBankAccountID = CInt(vFKArray(1, 0))

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(vFKArray(1, 2)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and populate the linked sources in vResultArray
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelSourcesLinkedWithBankAcSQL, sSQLName:=ACDelSourcesLinkedWithBankAcName, bStoredProcedure:=ACDelSourcesLinkedWithBankAcStored, lRecordsAffected:=lRecordsAffected)


            If Not Informations.IsArray(vKeys) Then
                'This is the case when no source has been added to BankAccount(Add a new bank account)
                'Then check is this account has been set as default' bank if yes then
                'Linked that that source as a default and Save Else Goto finally

                'Clear Parameters
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Call sp and Get the list of source ids which are linked with BankAccount_Default
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllSourcesLinkedWithBankAcDefaultSQL, sSQLName:=ACGetAllSourcesLinkedWithBankAcDefaultName, bStoredProcedure:=ACGetAllSourcesLinkedWithBankAcDefaultStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vKeys, bKeepNulls:=False)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get sources linked with Bank ", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            If Not Informations.IsArray(vKeys) Then
                Return result
            End If

            'Then Add the SourceID for this BankID (lBankAccountID)
            For Each vKeys_item As Object In vKeys

                lSourceID = gPMFunctions.ToSafeLong(vKeys_item)

                'Begin the Transaction
                BeginTrans()

                'Clear Parameters
                m_oDatabase.Parameters.Clear()

                'Adding Param BankAccountID
                m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Adding Param SourceID
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=CStr(vFKArray(1, 2)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
                End If


                'Call sp and add sources to the bank
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSourcesLinkedWithBankAcSQL, sSQLName:=ACAddSourcesLinkedWithBankAcName, bStoredProcedure:=ACAddSourcesLinkedWithBankAcStored, lRecordsAffected:=lRecordsAffected)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add sources to the bank", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Commit the Transaction
                CommitTrans()
            Next vKeys_item


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '        Return result
            '        Resume


            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckSourceAssociatedWithDefaultBank
    '
    ' Description: This function checks is the given branch is associated with the given default BankAccount
    '
    ' History: 16/05/2008 PK Created.

    ' Task: Account Function and CCY Cash Allocation
    '
    ' ***************************************************************** '
    Public Function CheckSourceAssociatedWithDefaultBank(ByVal v_lBankAccountID As Integer, ByVal v_lSourceID As Integer, ByRef r_bIsSourceAssociatedWithDefaultBank As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "CheckSourceAssociatedWithDefaultBank"


        Try

            Dim vResultArray(,) As Object = Nothing


            result = gPMConstants.PMEReturnCode.PMTrue

            r_bIsSourceAssociatedWithDefaultBank = False

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(v_lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and Get the list of source ids which are linked with BankAccount_Default
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllSourcesLinkedWithBankAcDefaultSQL, sSQLName:=ACGetAllSourcesLinkedWithBankAcDefaultName, bStoredProcedure:=ACGetAllSourcesLinkedWithBankAcDefaultStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get sources linked with Bank ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then

                For iCnt As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(1)
                    If v_lSourceID = gPMFunctions.ToSafeLong(vResultArray(0, iCnt)) Then
                        r_bIsSourceAssociatedWithDefaultBank = True
                        Exit For
                    End If
                Next iCnt
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckSourceAssociatedWithBank
    '
    ' Description: This function checks is the given branch is associated with the given BankAccount
    '
    ' History: 16/05/2008 PK Created.

    ' Task: Account Function and CCY Cash Allocation
    '
    ' ***************************************************************** '
    Public Function CheckSourceAssociatedWithBank(ByVal v_lBankAccountID As Integer, ByVal v_lSourceID As Integer, ByRef r_bIsSourceAssociatedWithBank As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "CheckSourceAssociatedWithDefaultBank"


        Try

            Dim vResultArray(,) As Object = Nothing


            result = gPMConstants.PMEReturnCode.PMTrue

            r_bIsSourceAssociatedWithBank = False

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(v_lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and populate the linked sources in vResultArray
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllSourcesLinkedWithBankAcSQL, sSQLName:=ACGetAllSourcesLinkedWithBankAcName, bStoredProcedure:=ACGetAllSourcesLinkedWithBankAcStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get sources linked with Bank ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then

                For iCnt As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(1)
                    If v_lSourceID = gPMFunctions.ToSafeLong(vResultArray(0, iCnt)) Then
                        r_bIsSourceAssociatedWithBank = True
                        Exit For
                    End If
                Next iCnt
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckSourceAttachedWithBank
    '
    ' Description: This function checks if any source is attached with the given BankAccount
    '
    ' History: 16/06/2008 Created - Gautam Poddar.
    '
    ' Task: Bank account deletion.
    '
    ' ***************************************************************** '
    Public Function CheckSourceAttachedWithBank(ByVal v_lBankAccountID As Integer, ByRef r_bIsSourceAttachedWithBank As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "CheckSourceAttachedWithBank"


        Try

            Dim vResultArray(,) As Object = Nothing
            Dim iCnt As Integer = 0


            result = gPMConstants.PMEReturnCode.PMTrue

            r_bIsSourceAttachedWithBank = False

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(v_lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and Get the list of source ids which are linked with BankAccount
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllSourcesLinkedWithBankAcSQL, sSQLName:=ACGetAllSourcesLinkedWithBankAcName, bStoredProcedure:=ACGetAllSourcesLinkedWithBankAcStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get sources attached with Bank ", gPMConstants.PMELogLevel.PMLogError)
            End If


            'If IsArray(vResultArray) = True then records found.
            r_bIsSourceAttachedWithBank = Informations.IsArray(vResultArray)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function IsChequeExistForBankAccount(ByVal v_lBankAccountID As Integer, ByRef r_bExit As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethod As String = "IsChequeExistForBankAccount"


        Try

            Dim vResultArray(,) As Object = Nothing
            Dim iCnt As Integer = Nothing


            result = gPMConstants.PMEReturnCode.PMTrue

            r_bExit = False

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=CStr(v_lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to add parameter", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Call sp and Get the list of source ids which are linked with BankAccount
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetChequeForBankAccountSQL, sSQLName:=ACGetChequeForBankAccountName, bStoredProcedure:=ACGetChequeForBankAccountStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to get cheques for a BankAccount ", gPMConstants.PMELogLevel.PMLogError)
            End If


            'If IsArray(vResultArray) = True then records found.
            r_bExit = Informations.IsArray(vResultArray)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse
        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
