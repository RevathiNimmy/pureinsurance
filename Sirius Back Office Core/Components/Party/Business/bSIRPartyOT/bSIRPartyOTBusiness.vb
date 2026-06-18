Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no.129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 08/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyOT.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
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
    Private Const description As Integer = 0
    Private Const code As Integer = 1
    Private Const name As Integer = 2
    Private Const party_other_posting_type As Integer = 3

    Private Const active_indicator As Integer = 0
    Private Const after_hours_indicator As Integer = 1
    Private Const priority_indicator As Integer = 2
    Private Const tpa_settle_direcly As Integer = 3

    Private Const supplier_business_id As Integer = 0
    Private Const supplier_speciality_id As Integer = 1

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' PRIVATE Data Members (Begin)

    ' Collection of SIRPartyOTs (Private)
    Private m_oSIRPartiesOT As bSIRPartyOT.SIRPartiesOT

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

    ' Primary Keys to work with
    Private m_lAddressCnt As Integer
    Private m_lPartyCnt As Integer

    Private m_sPartyName As String = ""
    Private m_sPartyTypeDesc As String = ""
    Private m_vSupplierBusinessArray As Object
    Private m_vAccidentsArray As Object

    'SD 17/09/2002 Add Supplier party type code and extra details
    Private m_sPartyTypeCode As String = ""
    Private m_vExtraSupplierDetails As Object


    Private m_oSIRParty As bSIRParty.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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
                Case Is > m_oSIRPartiesOT.Count()
                    m_lCurrentRecord = m_oSIRPartiesOT.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartiesOT.Count()

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


    Public Property AddressCnt() As Integer
        Get

            Return m_lAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lAddressCnt = Value

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
            m_sUsername = sUsername
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

            '***************************************

            m_oSIRParty = New bSIRParty.Business
            m_lReturn = m_oSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '***************************************


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyOTs Collection
            m_oSIRPartiesOT = New bSIRPartyOT.SIRPartiesOT()

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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                m_oSIRParty = Nothing
                m_oSIRPartiesOT = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Get address details for party.
    '
    ' ***************************************************************** '
    Public Function GetAddressDetails(ByRef vAddresses As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyOT - need to hit core for address stuff
            oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()

            m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyOT.bSIRParty.GetAddressDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddresses:=vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyOT = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAddresses(ByRef vPartyCnt As Object, Optional ByRef vAddAddresses As Object = Nothing, Optional ByRef vDeleteAddresses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyOT - need to hit core for address updates
            oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()

            m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyOT.bSIRParty.UpdateAddresses(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddAddresses:=vAddAddresses, vDeleteAddresses:=vDeleteAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
    ' Name: Clear
    '
    ' Description: Clear internal collections etc
    '
    '
    ' ***************************************************************** '
    Public Function Clear() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lCurrentRecord = 0

            ' Create SIRPartyOTs Collection
            m_oSIRPartiesOT = Nothing
            m_oSIRPartiesOT = New bSIRPartyOT.SIRPartiesOT()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRTextFile.
    '
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIROthParty As bSIRPartyOT.SIRPartyOT = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 3) As Object
        Dim vRepeatTypeID As Object = Nothing
        Dim vEventTypeID As Object = Nothing
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 146
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Supplier_Business"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "Supplier_Speciality"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = "Driver_Status"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = "License_Type"
            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIROthParty = m_oSIRPartiesOT.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oSIROthParty

                        ' {* USER DEFINED CODE (Begin) *}
                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vRepeatTypeID


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vEventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIROthParty

                        ' {* USER DEFINED CODE (Begin) *}
                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vRepeatTypeID


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vEventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRTextFile reference
            oSIROthParty = Nothing

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

    'DC090402 added for other party types
    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyOT directly into the database.
    '        Note: The SIRPartyOT will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyTypeId As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vPartyStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vExternalId As Object = Nothing, Optional ByRef vReg_Number As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyFP
            oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()
            m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyOT.bSIRParty.DirectAdd(vPartyTypeId:=vPartyTypeId, vShortname:=vShortname, vName:=vName, vResolvedName:=vName, vCurrencyID:=vCurrencyID, vStatus:=vStatus, vLastModified:=vLastModified, vDateCreated:=vDateCreated)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyOT = Nothing
                Return result
            End If

            ' Retrieve Primary Key of new Core record

            vPartyCnt = oSIRPartyOT.bSIRParty.PartyCnt
            oSIRPartyOT.PartyCnt = vPartyCnt

            ' Populate SIRPartyFP Attributes
            m_lReturn = CType(oSIRPartyOT.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vPartyStatus:=vPartyStatus, vReferenceNumber:=vReferenceNumber, vExternalId:=vExternalId, vRegNumber:=vReg_Number, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyOT = Nothing
                Return result
            End If

            ' Add the SIRPartyFP to the Database
            m_lReturn = CType(oSIRPartyOT.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyOT = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyOT Added
            With oSIRPartyOT
                PartyCnt = .PartyCnt
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetOtherPartyPostingType
    '
    ' Description:
    '
    ' History: 11/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetOtherPartyPostingType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyOtherPostingType As String) As Integer

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object = Nothing
            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sPartyOtherPostingType = ""

            sSQL = "SELECT party_other_posting_type.code " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM Party, Party_Type, party_other_posting_type " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE party_cnt = " & CStr(v_lPartyCnt) &
                   " AND Party.party_type_id = Party_Type.party_type_id" &
                   " AND Party_type.party_other_posting_type_id = party_other_posting_type.party_other_posting_type_id"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOtherPartyPostingType", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                r_sPartyOtherPostingType = gPMFunctions.NullToString(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherPartyPostingType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherPartyPostingType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetPartyAdditionalDetails(ByVal v_lPartyCnt As Integer, ByRef r_sPartyName As String, ByRef r_sPartyTypeDesc As String, ByRef r_sPartyTypeCode As String, ByRef r_sPartyOtherPostingType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyAdditionalDetails"
        Dim sSQL As String = ""
        Dim r_vResults(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try


            'Set Key Object Property.
            PartyCnt = v_lPartyCnt
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            Dim lReturn As gPMConstants.PMEReturnCode = m_oDatabase.Parameters.Add(sName:="v_lPartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'm_lReturn = AddInputParameter(v_sName:="v_lPartyCnt", v_vValue:="v_lPartyCnt", v_iType:="PMLong")


            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetOtherPartyAdditionalDetailsSQL, sSQLName:=kGetOtherPartyAdditionalDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetOtherPartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Not Informations.IsArray(r_vResults) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            r_sPartyTypeDesc = gPMFunctions.NullToString(r_vResults(description, 0))

            r_sPartyTypeCode = gPMFunctions.NullToString(r_vResults(code, 0))
            m_sPartyTypeCode = r_sPartyTypeCode
            r_sPartyName = gPMFunctions.NullToString(r_vResults(name, 0))

            r_sPartyOtherPostingType = gPMFunctions.NullToString(r_vResults(party_other_posting_type, 0))

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = PMEReturnCode.PMFalse
        End Try

        Return result

    End Function

    ''' <summary>
    ''' GetOtherPartyDetails
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_vPartyDetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetOtherPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vPartyDetail() As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOtherPartyDetails"
        Try
            Dim sSQL As String = ""
            Dim r_vResults(,) As Object = Nothing
            result = gPMConstants.PMEReturnCode.PMTrue
            'Set Key Object Property.
            PartyCnt = v_lPartyCnt
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            Dim lReturn As gPMConstants.PMEReturnCode = m_oDatabase.Parameters.Add(sName:="v_lPartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'm_lReturn = AddInputParameter(v_sName:="PartyCnt", v_vValue:="v_lPartyCnt", v_iType:="PMLong")


            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetOtherPartyDetailsSQL, sSQLName:=kGetOtherPartyDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetOtherPartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Not Informations.IsArray(r_vResults) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim vPartyDetail(3) As Object

            vPartyDetail(0) = r_vResults(active_indicator, 0)

            vPartyDetail(1) = r_vResults(after_hours_indicator, 0)

            vPartyDetail(2) = r_vResults(priority_indicator, 0)
            vPartyDetail(3) = r_vResults(tpa_settle_direcly, 0)
            r_vPartyDetail = vPartyDetail

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
            result = PMEReturnCode.PMFalse
        Finally

        End Try
        Return result


        '        GoTo Finally_Renamed


        '        ' DO Not Call any functions before here or the error will be lost

        '        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)


        '        ' If you want to rollback a transaction or something, do it here

        'Finally_Renamed:

        '        Return result

        '        Resume

        '        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPartySupplierBusiness(ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartySupplierBusiness"

        Const kSupplierBusiness As Integer = 2

        Try
            Dim vBusinessArray(,) As Object = Nothing
            Dim vSpecialityArray As Object

            Dim iSpecialityCount As Integer

            Dim sSQL As String = ""
            Dim r_vResults(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set Key Object Property.
            PartyCnt = v_lPartyCnt

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()


            ' Add Required Stored Procedure Parameters
            Dim lReturn As gPMConstants.PMEReturnCode = m_oDatabase.Parameters.Add(sName:="v_lPartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPartySupplierBusinessSQL, sSQLName:=kGetPartySupplierBusinessName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetPartySupplierBusinessSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(r_vResults) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim lCurrentBusiness As Integer = 0
            Dim iBusinessCount As Integer = -1


            ' Yes, load them into the collection

            For iRecord As Integer = 0 To r_vResults.GetUpperBound(1) 'lRecordCount&
                ' Set oFields to refer to next Record
                'Set oFields = m_oDatabase.Records.Item(CVar(iRecord)).Fields

                'Check to see if we have found a new business type.
                If gPMFunctions.NullToLong(r_vResults(kSupplierBusiness, iRecord)) <> lCurrentBusiness Then

                    iBusinessCount += 1

                    'Temporarily store current business type
                    lCurrentBusiness = gPMFunctions.ToSafeLong(r_vResults(kSupplierBusiness, iRecord))

                    ReDim Preserve vBusinessArray(1, iBusinessCount)

                    'Assign current business id to array.

                    vBusinessArray(0, iBusinessCount) = lCurrentBusiness

                    'Clear Speciality array
                    ReDim vSpecialityArray(0)
                    iSpecialityCount = 0

                End If

                'Continue looping until all specialities have been found for
                'the current business type.
                ReDim Preserve vSpecialityArray(iSpecialityCount)

                vSpecialityArray(iSpecialityCount) = gPMFunctions.ToSafeLong(r_vResults(supplier_speciality_id, iRecord))

                'Assign specialities for this business type to the Business Array.


                vBusinessArray(1, iBusinessCount) = vSpecialityArray
                iSpecialityCount += 1

            Next iRecord

            r_vResultArray = vBusinessArray
        Catch excep As System.Exception





            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
            result = gPMConstants.PMEReturnCode.PMError

            Return result
        Finally


        End Try

        Return result

        '        GoTo Finally_Renamed


        '        ' DO Not Call any functions before here or the error will be lost

        '        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)


        '        ' If you want to rollback a transaction or something, do it here

        'Finally_Renamed:

        '        Return result

        '        Resume

        '        Return result
    End Function



    Public Function UpdateOtherPartyDetails() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            m_oDatabase.Parameters.Clear()

            ' Add Party Code as an INPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Add the other three parameters for a Supplier party (null otherwise)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="active_indicator", vValue:=m_vExtraSupplierDetails(ACActiveIndicator), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="after_hours_indicator", vValue:=m_vExtraSupplierDetails(ACAfterHoursIndicator), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="priority_indicator", vValue:=m_vExtraSupplierDetails(ACPriorityIndicator), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_TPA_settle_directly", vValue:=m_vExtraSupplierDetails(ACTPASettleDirectly), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPartyOtherSQL, sSQLName:=ACAddPartyOtherName, bStoredProcedure:=ACAddPartyOtherStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception


            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOtherPartyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOtherPartyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function AddPartySupplierBusiness(Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lBusinessId, lSpecialityId As Integer
        Dim vSpecialityArray() As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            ' Add Party Code as an INPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPartSupplierBussSQL, sSQLName:=ACDelPartSupplierBussName, bStoredProcedure:=ACDelPartSupplierBussStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '    If Not IsArray(v_vArray) Then
            If Not Informations.IsArray(m_vSupplierBusinessArray) Then
                'If we have no array then all Businesses must have been removed, so skip
                'remainder of function.
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                End If

                Return result
            End If

            For iBusinessCount As Integer = 0 To m_vSupplierBusinessArray.GetUpperBound(1)
                lBusinessId = CInt(m_vSupplierBusinessArray(0, iBusinessCount))

                vSpecialityArray = m_vSupplierBusinessArray(1, iBusinessCount)

                'Check speciality array is populated.
                If Informations.IsArray(vSpecialityArray) Then

                    If Not Object.Equals(vSpecialityArray, Nothing) Then

                        For iSpecialityCount As Integer = 0 To vSpecialityArray.GetUpperBound(0)


                            lSpecialityId = CInt(vSpecialityArray(iSpecialityCount))

                            ' Clear the Database Parameters Collection
                            m_oDatabase.Parameters.Clear()

                            ' Add Party Code as an INPUT param for an insert
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            ' Add specialty_id as an INPUT param for an insert
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="supplier_speciality_id", vValue:=CStr(lSpecialityId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            ' Add business_id as an INPUT param for an insert
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="supplier_business_id", vValue:=CStr(lBusinessId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPartSuppBussSQL, sSQLName:=ACAddPartSuppBussName, bStoredProcedure:=ACAddPartSuppBussStored)


                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        Next iSpecialityCount

                    Else
                        'Do we need to store Business links without specialities ??

                    End If
                Else

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add Party Code as an INPUT param for an insert
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Add specialty_id as an INPUT param for an insert

                    'developer guide no.85
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="supplier_speciality_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Add business_id as an INPUT param for an insert
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="supplier_business_id", vValue:=CStr(lBusinessId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPartSuppBussSQL, sSQLName:=ACAddPartSuppBussName, bStoredProcedure:=ACAddPartSuppBussStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            Next iBusinessCount

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartySupplierBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartySupplierBusiness", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetConvictionsAndAccidents(ByRef r_vConvictions(,) As Object, ByRef r_vAccidents(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetConvictionsSQL, sSQLName:=ACGetConvictionsName, bStoredProcedure:=ACGetConvictionsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vConvictions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccidentsSQL, sSQLName:=ACGetAccidentsName, bStoredProcedure:=ACGetAccidentsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAccidents)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetConvictionsAndAccidents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetConvictionsAndAccidents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateAccidents(Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(m_vAccidentsArray) Then
                For lCount As Integer = m_vAccidentsArray.GetLowerBound(1) To m_vAccidentsArray.GetUpperBound(1)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=CStr(sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    Exit For
                Next
            End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteAccidentsSQL, sSQLName:=ACDeleteAccidentsName, bStoredProcedure:=ACDeleteAccidentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(m_vAccidentsArray) Then

                For lCount As Integer = m_vAccidentsArray.GetLowerBound(1) To m_vAccidentsArray.GetUpperBound(1)

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="previous_accidents_id", vValue:=CStr(lCount + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="date", vValue:=CStr(m_vAccidentsArray(2, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(m_vAccidentsArray(3, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_at_fault", vValue:=CStr(m_vAccidentsArray(4, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=CStr(sUniqueId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy & $"\Accident({CStr(m_vAccidentsArray(3, lCount))})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddAccidentsSQL, sSQLName:=ACAddAccidentsName, bStoredProcedure:=ACAddAccidentsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next lCount
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAccidents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAccidents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressTypeLookups
    '
    ' Description: Get address type lookups.
    '
    ' ***************************************************************** '
    Public Function GetAddressTypeLookups(ByRef vAddressTypes As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyAG - need to hit core for address stuff
            oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()

            m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyOT.bSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oSIRPartiesOT.Count()
                Select Case m_oSIRPartiesOT.Item(lSub).DatabaseStatus
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'SD 17/09/2002 Supplier party type extra details
    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRPartyAG into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vAccidentsArray As Object = Nothing, Optional ByRef vPartyTypeCode As Object = Nothing, Optional ByRef vSupplierBusinessArray As Object = Nothing, Optional ByRef vExtraSupplierDetails As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBranch As Object = Nothing, Optional ByRef vSubBranch As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT
        Dim lPartyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Store array of Business/Speciality links in this object for use in Update.
            m_vSupplierBusinessArray = vSupplierBusinessArray

            'SD 17/09/2002 Supplier party type extra details

            m_vExtraSupplierDetails = vExtraSupplierDetails

            m_vAccidentsArray = vAccidentsArray

            'Get party type id for an agent

            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=CStr(vPartyTypeCode), v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartiesOT.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRPartyOT
            oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()
            m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRPartyOT Attributes

            m_lReturn = CType(oSIRPartyOT.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=CInt(vPartyCnt), vDateOfBirth:=vDateOfBirth, vGender:=vGender, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vPartyStatus:=vStatus, vReferenceNumber:=vReferenceNumber, vRegNumber:=vRegNumber, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy, vExtraSupplierPartyDetails:=m_vExtraSupplierDetails), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyOT = Nothing
                Return result
            End If

            ' Add SIRPartyAG to collection
            If m_oSIRPartiesOT.Count = 0 Then
                m_oSIRPartiesOT.Add(Nothing)
            End If
            m_lReturn = CType(m_oSIRPartiesOT.Add(oNewSIRPartyOT:=oSIRPartyOT), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyOT = Nothing
                Return result
            End If

            m_lReturn = oSIRPartyOT.bSIRParty.EditAdd(lRow:=lRow, vPartyTypeId:=ToSafeInteger(lPartyTypeId),
                                                      vShortname:=vShortname, vName:=vName,
                                                      vResolvedName:=vResolvedName,
                                                      vCurrencyID:=vCurrencyID, vSourceID:=vBranch,
                                                      vSubBranchID:=vSubBranch, sUniqueId:=ToSafeString(vUniqueId), sScreenHierarchy:=ToSafeString(vScreenHierarchy))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyOT = Nothing
                Return result
            End If

            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartiesOT and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = 0, Optional ByRef vPartyCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oFields As DataRow
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartiesOT.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vPartyCnt)) And (Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyCnt=" & vPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vPartyCnt) Then

                ' Create New SIRPartyAG
                oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()
                m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oSIRPartyOT
                    .PartyCnt = vPartyCnt

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRPartyAG to collection
                If m_oSIRPartiesOT.Count = 0 Then
                    m_oSIRPartiesOT.Add(Nothing)
                End If
                m_lReturn = CType(m_oSIRPartiesOT.Add(oNewSIRPartyOT:=oSIRPartyOT), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the core Business method

                m_lReturn = oSIRPartyOT.bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRPartyOT = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()
                    m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    ' Set oFields to refer to one Record
                    'developer guide no. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRPartyOT
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        m_lReturn = .bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=ToSafeInteger(.PartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                    ' Add SIRPartyAG to collection
                    If m_oSIRPartiesOT.Count = 0 Then
                        m_oSIRPartiesOT.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSIRPartiesOT.Add(oNewSIRPartyOT:=oSIRPartyOT), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRPartyOT = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRPartiesOT and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing) As Integer


        Dim result As Integer = 0

        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartiesOT.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRPartyOT = m_oSIRPartiesOT.Item(m_lCurrentRecord)

            ' Get the SIRPartyAG Property Values

            'developer guide no.98
            m_lReturn = CType(oSIRPartyOT.GetProperties(iStatus, vPartyCnt:=vPartyCnt, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vPartyStatus:=vStatus, vReferenceNumber:=vReferenceNumber, vRegNumber:=vRegNumber, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Core details

            m_lReturn = oSIRPartyOT.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SP231198

            m_lReturn = oSIRPartyOT.bSIRParty.GetNext(vCurrencyID:=vCurrencyID, vShortname:=vShortname, vName:=vName, vPartyID:=vPartyID, vSourceID:=vSourceID, vSubBranchID:=vSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetPartyTypeDescription(ByVal v_sPartyTypeCode As String, ByRef r_sPartyTypeDesc As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        'developer guide no. 112
        Dim oFields As DataRow
        Dim lRecordCount As Integer

        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            v_sPartyTypeCode = v_sPartyTypeCode.Trim()

            sSQL = "SELECT description " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM Party_Type " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE code = '" & v_sPartyTypeCode & "'"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyTypeDescription", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            'developer guide no. 111
            oFields = m_oDatabase.Records.Item(0).Fields()
            r_sPartyTypeDesc = gPMFunctions.NullToString(oFields("description"))

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyTypeDescription Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyTypeDescription", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetPartyType(ByVal v_sPartyTypeCode As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURENAME As String = "GetPartyType"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            v_sPartyTypeCode = v_sPartyTypeCode.Trim()

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_type_code", vValue:=v_sPartyTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=AC_SQL_PARTYTYPESELECT_SQL, sSQLName:=AC_SQL_PARTYTYPESELECT_NAME, bStoredProcedure:=AC_SQL_PARTYTYPESELECT_SP, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURENAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Update(Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRPartiesOT.Count()
                oSIRPartyOT = m_oSIRPartiesOT.Item(lSub)


                Select Case oSIRPartyOT.DatabaseStatus
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

                        m_lReturn = oSIRPartyOT.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        ' Retrieve Primary Key of Core Item added

                        PartyCnt = oSIRPartyOT.bSIRParty.PartyCnt
                        oSIRPartyOT.PartyCnt = PartyCnt

                        'm_lReturn = CommitTrans()
                        m_lReturn = CType(oSIRPartyOT.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        'Save Business/Supplier links to database.
                        m_lReturn = AddPartySupplierBusiness(sUniqueId, sScreenHierarchy)

                        m_lReturn = UpdateAccidents(sUniqueId, sScreenHierarchy)

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
                        m_lReturn = CType(oSIRPartyOT.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRPartyOT.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        'Save Business/Supplier links to database.
                        m_lReturn = AddPartySupplierBusiness(sUniqueId, sScreenHierarchy)

                        m_lReturn = UpdateAccidents(sUniqueId, sScreenHierarchy)


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
                        m_lReturn = CType(oSIRPartyOT.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRPartyOT.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRPartyAG
            With oSIRPartyOT
                PartyCnt = .PartyCnt
            End With

            ' Release last reference
            oSIRPartyOT = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRPartiesOT.Count()

                        ' With the item
                        With m_oSIRPartiesOT.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRPartiesOT.Delete(lSub)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'SD 17/09/2002 Supplier party type extra details
    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRPartyAG
    '              specified and updates the SIRPartyOT with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vAccidentsArray As Object = Nothing, Optional ByRef vSupplierBusinessArray As Object = Nothing, Optional ByRef vExtraSupplierDetails As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBranch As Object = Nothing, Optional ByRef vSubBranch As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Store array of Business/Speciality links in this object for use in Update.
            m_vSupplierBusinessArray = vSupplierBusinessArray

            'SD 17/09/2002 Supplier party type extra details

            m_vExtraSupplierDetails = vExtraSupplierDetails
            m_vAccidentsArray = vAccidentsArray

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartiesOT.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRPartyOT = m_oSIRPartiesOT.Item(lRow)

            ' Check the Status of the SIRPartyOT

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRPartyOT.DatabaseStatus
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

            ' Update SIRPartyOT Attributes

            m_lReturn = CType(oSIRPartyOT.SetProperties(iStatus:=iStatus, vPartyCnt:=CInt(vPartyCnt), vDateOfBirth:=vDateOfBirth, vGender:=vGender, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vPartyStatus:=vStatus, vReferenceNumber:=vReferenceNumber, vRegNumber:=vRegNumber, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy, vExtraSupplierPartyDetails:=m_vExtraSupplierDetails), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyOT = Nothing
                Return result
            End If

            m_lReturn = oSIRPartyOT.bSIRParty.EditUpdate(lRow:=lRow, vShortname:=vShortname,
                                                         vName:=vName, vResolvedName:=vResolvedName,
                                                         vCurrencyID:=vCurrencyID, vSourceID:=vBranch,
                                                         vSubBranchID:=vSubBranch, sUniqueId:=ToSafeString(vUniqueId), sScreenHierarchy:=ToSafeString(vScreenHierarchy))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyOT = Nothing
                Return result
            End If

            ' Release reference to SIRPartyOT
            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyCnt
    '
    ' Description: Get party count for a given reference (ie shortname)
    '
    ' ***************************************************************** '
    Public Function GetPartyCnt(Optional ByRef vPartyRef As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oSIRPartyOT As bSIRPartyOT.SIRPartyOT
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyOT - need to for to core for shortname
            oSIRPartyOT = New bSIRPartyOT.SIRPartyOT()

            m_lReturn = CType(oSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyOT.bSIRParty.GetPartyCnt(vPartyRef:=vPartyRef, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyOT = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaultCountryCode
    '
    ' Description: Get Country Code (eg - 'GBR') from system home_country_id.
    '
    ' Author: RWH
    '
    ' CTAF 141200 - Changed to use vResultArray to get the result.
    '               This was stopping reports from working for some reason.
    '
    ' ***************************************************************** '

    Public Function GetDefaultCountryCode(ByVal v_iCountryID As Integer, ByRef r_sCountryCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT code FROM Country WHERE country_id = " & v_iCountryID

            ' Perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDefaultCountryCode", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Informations.IsArray(vResultArray) Then

                r_sCountryCode = CStr(vResultArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultCountryCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCountryCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return SiriusCoreFunc.GetSubBranches(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_vSubBranchArray:=r_vSubBranchArray)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartySystemTypeCode
    '
    ' Description: Gets the SystemTypeCode from a party's type code
    ' IR 30/06/2003
    '
    ' ***************************************************************** '
    Public Function GetPartySystemTypeCode(ByVal v_sPartyTypeCode As String) As String

        Dim result As String = String.Empty
        Const sFunctionName As String = "GetPartySystemTypeCode"
        Dim vPartySysType(,) As Object = Nothing

        Try

            result = ""

            ' Clear Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add input parameter
            If m_oDatabase.Parameters.Add(sName:="PartyTypeCode", vValue:=v_sPartyTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sPartyTypeCode", v_sPartyTypeCode)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
            End If

            ' Execute stored proc
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartySystemTypeCodeSQL, sSQLName:=ACGetPartySystemTypeCodeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vPartySysType)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Informations.IsArray(vPartySysType) Then

                    result = CStr(vPartySysType(0, 0))
                End If
            End If
            Return result

        Catch excep As System.Exception


            result = CStr(gPMConstants.PMEReturnCode.PMError)


            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sPartyTypeCode", v_sPartyTypeCode)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-08-2005 : 360 = Taxes on Claims
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            lReturn = m_oSIRParty.GetPartyDetails(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdatePartyDetails(ByVal v_lPartyCnt As Integer, ByVal v_vPartyDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oSIRParty.UpdatePartyDetails(v_lPartyCnt:=v_lPartyCnt, v_vPartyDetails:=v_vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'Changes for WPR-42
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)



                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                Next iParam

                m_lReturn = .SQLSelect("spu_other_Party_PLLSource", sPickListType & " PickList Load", True, , vResultArray)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Select Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer

        Dim result As Integer = 0

        Try

            BeginTrans()

            If vFKArray.GetUpperBound(1) > 4 And sPickListType.Trim().ToUpper() = "SOURCE" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(0) To vFKArray.GetUpperBound(0) - 1
                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                'Developer Guide No. 39
                m_lReturn = .SQLAction("spu_Other_Party_PLDSource", sPickListType & " PickList Delete", True)

                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()

                        'Load the FK parameters
                        'For iParam = LBound(vFKArray, 2) To UBound(vFKArray, 2)


                        .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        'Next iParam


                        .Parameters.Add("Branchid", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        .Parameters.Add(sName:="UniqueId", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        .Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        'Call the SP
                        'Developer Guide No. 39
                        m_lReturn = .SQLAction("spu_Other_Party_PLSSource", sPickListType & " PickList Load", True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey
                End If
            End With

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction("spu_Update_Other_Party_User_Source", sPickListType & " PickList Load", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    RollbackTrans()
                    Return m_lReturn
                End If


            End With


            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()
            Return result

        End Try
    End Function

    Public Function DeleteAddress(ByVal party_cnt As Integer, ByVal address_cnt As Integer) As Integer

        Dim m_lReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            With m_oDatabase

                .Parameters.Clear()
                'Developer Guide No. 40
                .Parameters.Add("is_deleted", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("party_cnt", party_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("address_cnt", address_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACDelAddressSQL, sSQLName:=ACDelAddressName, bStoredProcedure:=ACDelAddressStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Return m_lReturn
            End With
        Catch excep As System.Exception

            m_lReturn = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Addresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_lReturn
        End Try
    End Function


    'End Changes for WPR-42

End Class
