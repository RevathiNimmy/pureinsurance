Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 23/07/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Ledger.
    '
    ' Edit History:
    ' DD 01/08/2002: Alterations for Multi-Branch Accounting
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Ledgers (Private)
    Private m_oLedgers As bACTLedger.Ledgers

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

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
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer


    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    Public Shared iCache As ICacheManager
    Private m_sCachePath As String = String.Empty
    Private m_sMultiTreeAccounting As String = String.Empty
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
                Case Is > m_oLedgers.Count()
                    m_lCurrentRecord = m_oLedgers.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oLedgers.Count()

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

    Public Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileID = Value

        End Set
    End Property

    Public Property RiskID() As Integer
        Get

            Return m_lRiskID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskID = Value

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database
            'EK 5/11/99 Use component services
            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

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

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create Ledgers Collection
            m_oLedgers = New bACTLedger.Ledgers()


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception

            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sCachePath.Substring(m_sCachePath.Length - 1, 1) <> "\" Then
                m_sCachePath += "\"
            End If

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername,
                                                    v_sPassword:=m_sPassword,
                                                    v_iUserID:=m_iUserID,
                                                    v_iMainSourceID:=m_iSourceID,
                                                    v_iLanguageID:=m_iLanguageID,
                                                    v_iCurrencyID:=m_iCurrencyID,
                                                    v_iLogLevel:=m_iLogLevel,
                                                    v_sCallingAppName:=m_sCallingAppName,
                                                    v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting,
                                                    v_vBranch:=1,
                                                    r_vUnderwriting:=m_sMultiTreeAccounting)




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
                m_oLedgers = Nothing
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
    ' Description: Gets the Lookup values for a Ledger.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer

        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gACTLibrary.ACTLookupLedgerType

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oLedger = m_oLedgers.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oLedger

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .LedgertypeID

                        'BB dtEffectiveDate = .EffectiveDate
                        'BB Default Effective Date to current date/time
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oLedger

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = .LedgertypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Ledger reference
            oLedger = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Ledger directly into the database.
    '        Note: The Ledger will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vLedgerID As Integer = 0, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Ledger
            oLedger = New bACTLedger.Ledger()

            ' Populate Ledger Attributes









            'developer guide no.98
            m_lReturn = SetProperties(oLedger, gPMConstants.PMEComponentAction.PMAdd,
                 vLedgerID:=vLedgerID,
                 vCompanyID:=vCompanyID,
                 vSubBranchID:=vSubBranchID,
                 vLedgerName:=vLedgerName,
                 vLedgerShortName:=vLedgerShortName,
                 vMappingID:=vMappingID,
                 vLedgertypeID:=vLedgertypeID,
                 vIsDeletable:=vIsDeletable,
                 vCurrentPeriodID:=vCurrentPeriodID,
                 vSequence:=vSequence)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Ledger to the Database
            m_lReturn = AddItem(oLedger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Ledger Added

            If Not Informations.IsNothing(vLedgerID) Then
                vLedgerID = oLedger.LedgerID
            End If

            ' {* USER DEFINED CODE (End) *}

            oLedger = Nothing

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
    ' Description: Deletes a single Ledger directly from the database.
    '        Note: The Ledger will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Ledger
            oLedger = New bACTLedger.Ledger()

            ' Populate Ledger Attributes










            'developer guide no.98
            m_lReturn = SetProperties(oLedger, gPMConstants.PMEComponentAction.PMDelete,
                vLedgerID:=vLedgerID,
                vCompanyID:=vCompanyID,
                vSubBranchID:=vSubBranchID,
                vLedgerName:=vLedgerName,
                vLedgerShortName:=vLedgerShortName,
                vMappingID:=vMappingID,
                vLedgertypeID:=vLedgertypeID,
                vIsDeletable:=vIsDeletable,
                vCurrentPeriodID:=vCurrentPeriodID,
                vSequence:=vSequence)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Ledger to the Database
            m_lReturn = DeleteItem(oLedger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oLedger = Nothing

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
    ' Description: Returns the Default Values for the Ledger.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults










            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vLedgerID:=CByte(vLedgerID), vCompanyID:=CByte(vCompanyID), vSubBranchID:=CByte(vSubBranchID), vLedgerName:=CStr(vLedgerName), vLedgerShortName:=CStr(vLedgerShortName), vMappingID:=CByte(vMappingID), vLedgertypeID:=CByte(vLedgertypeID), vIsDeletable:=CByte(vIsDeletable), vCurrentPeriodID:=CByte(vCurrentPeriodID), vSequence:=CByte(vSequence))

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
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer
        Return GetCaptions(vID:=vID, vFieldArray:=vFieldArray, vResultArray:=vResultArray, vTable:=Nothing)
    End Function

    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer

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
            '    If (Trim$(vTable) <> PMTableLedger) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CheckID(vID:=vID)

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

                    'AK 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        'Milan_TODO: 
                        'Part of Iteration 3
                        Select Case oFields(vFieldArray(lSub)).Type
                            'Developer Guide No 47(no solutions)
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String

                                vResults(lSub) = ""
                                'Developer Guide No 47(no solutions)
                                'Case DbType.Date, adDBDate
                            Case DbType.Date

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
    ' Description: Gets the required Ledgers and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vLockMode As Integer = 0, Optional ByRef vSubBranchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oLedger As bACTLedger.Ledger
        Dim sContent(1) As String
        Dim sCacheFilename As String = "Ledger"
        Dim sFilePath As String = ""
        Dim oFieldNameArray As Object = Nothing
        Dim sKey As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the subbranch

            If Informations.IsNothing(vSubBranchID) Then


                vSubBranchID = DBNull.Value
            End If

            ' Clear the Collection
            m_oLedgers.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vLedgerID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vLedgerID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vLedgerID =" & CStr(vLedgerID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the LedgerID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Ledger_id", vValue:=CStr(vLedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
                'If Hidden Option is off, we only return data for sub_branch_id = 1
                If m_sMultiTreeAccounting = "" Then
                    ReDim oFieldNameArray(0)
                    Dim nRowCount As Integer = 0
                    sKey = sCacheFilename.ToUpper() + "" + nRowCount.ToString.Trim.ToUpper() + "_" + vSubBranchID.ToString()

                    'Check for Zero th row and keep on increasing nRowCount till we do not find
                    Do While Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey)))
                        oLedger = New bACTLedger.Ledger()
                        oLedger = iCache.GetData(sKey)
                        If (m_oLedgers.Count = 0) Then
                            m_oLedgers.Add(Nothing)
                        End If
                        m_lReturn = m_oLedgers.Add(oNewLedger:=oLedger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        oLedger = Nothing
                        nRowCount = nRowCount + 1
                        sKey = sCacheFilename.ToUpper() + nRowCount.ToString.Trim.ToUpper()
                    Loop

                    If nRowCount > 0 Then
                        Return result
                    End If
                End If

                'BB Add the CompanyID parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Sub Branch ID parameter

                m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=vSubBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

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
                sFilePath = m_sCachePath + sCacheFilename + ".xml"
                If Not FileExists(sFilePath) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sFilePath)
                    fileIO.Close()
                    File.WriteAllLines(sFilePath, sContent)
                End If

                ' Yes, load them into the collection
                'Developer Guide No 111
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Ledger
                    oLedger = New bACTLedger.Ledger()

                    m_lReturn = SetPropertiesFromDB(oLedger:=oLedger, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Ledger to collection
                    If (m_oLedgers.Count = 0) Then
                        m_oLedgers.Add(Nothing)
                    End If
                    m_lReturn = m_oLedgers.Add(oNewLedger:=oLedger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    sKey = sCacheFilename.ToUpper() + lSub.ToString()
                    If Not iCache Is Nothing Then
                        Dim atTimePeriod As AbsoluteTime = New AbsoluteTime(TimeSpan.FromMinutes(10))
                        iCache.Add(sKey, oLedger, CacheItemPriority.Normal, Nothing, New FileDependency(sFilePath), atTimePeriod)
                    End If

                    oLedger = Nothing

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
    'EK 11/11/99 New Method to get ledgers for closure
    ' Name: GetClosures (Public)
    '
    ' Description: Gets the required Ledgers for closureand populate the Collection
    '
    ' ********************************************************************** '
    Public Function GetClosures(ByRef vLedgerID As Object) As Integer
        Return GetClosures(vLedgerID:=vLedgerID, vLockMode:=PMELockMode.PMNoLock)
    End Function
    Public Function GetClosures(ByRef vLedgerID As Object, ByRef vLockMode As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oLedger As bACTLedger.Ledger

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oLedgers.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If



            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vLedgerID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vLedgerID =" & CStr(vLedgerID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetClosures")

                Return result

            End If

            ' Add the LedgerID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Ledger_id", vValue:=CStr(vLedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClosuresSQL, sSQLName:=ACGetClosuresName, bStoredProcedure:=ACGetClosuresStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection

                'Developer Guide No 111
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New Ledger
                    oLedger = New bACTLedger.Ledger()

                    m_lReturn = SetPropertiesFromDB(oLedger:=oLedger, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Ledger to collection
                    If (m_oLedgers.Count = 0) Then
                        m_oLedgers.Add(Nothing)
                    End If
                    m_lReturn = m_oLedgers.Add(oNewLedger:=oLedger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oLedger = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClosures Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClosures", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Gets Ledger Node Id
    ''' </summary>
    ''' <param name="v_sLedgerName"></param>
    ''' <param name="v_lNodeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLedgerNodeId(ByVal v_sLedgerName As String, ByRef v_lNodeId As Integer) As Integer

        ' This routine returns NodeId from Ledger Name
        ' as defined in the mapping table
        ' should really be via Ledger

        Dim nResult As Integer = 0
        Dim sContent(1) As String
        Dim sCacheFilename As String = "LedgerNode"
        Dim sFilePath As String = ""
        Dim oFieldNameArray As Object = Nothing
        Dim sKey As String = String.Empty
        Try

            nResult = PMEReturnCode.PMTrue

            ReDim oFieldNameArray(0)
            sKey = sCacheFilename.ToUpper() + " " + v_sLedgerName.Trim.ToUpper() + m_iSourceID.ToString()

            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                oFieldNameArray = iCache.GetData(sKey)
            End If

            If Not Informations.IsNothing(oFieldNameArray(0)) Then
                v_lNodeId = oFieldNameArray(0)
                Return nResult
            End If


            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ledger_name", vValue:=v_sLedgerName, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'DD 01/08/2002: Added for multi-branch accounting
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectLedgerNodeSQL, sSQLName:=ACSelectLedgerNodeName, bStoredProcedure:=ACSelectLedgerNodeStored, lNumberRecords:=0)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Count() <> 1 Then
                Return PMEReturnCode.PMFalse
            End If


            v_lNodeId = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("node_id"))
            oFieldNameArray(0) = v_lNodeId


            sFilePath = m_sCachePath + sCacheFilename + ".xml"
            If Not File.Exists(sFilePath) Then
                Dim fileIO As FileStream
                fileIO = File.Create(sFilePath)
                fileIO.Close()
                File.WriteAllLines(sFilePath, sContent)
            End If

            If Not iCache Is Nothing Then
                Dim atTimePeriod As AbsoluteTime = New AbsoluteTime(TimeSpan.FromMinutes(10))
                iCache.Add(sKey, oFieldNameArray, CacheItemPriority.Normal, Nothing, New FileDependency(sFilePath), atTimePeriod)
            End If



            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetLedgerNodeId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerNodeId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function

    'eck310800
    ' ***************************************************************** '
    ' Name: GetNodeFromLedger (Public)
    '
    ' Description: Gets Node Using Ledger Name not Mapping Description
    '
    ' DD 01/08/2002: New parameter for Multi-Branch Accounting
    ' ***************************************************************** '
    Public Function GetNodeFromLedger(ByVal v_sLedgerName As String, ByVal v_lSubBranchID As Integer, ByRef r_lNodeId As Integer) As Integer

        ' This routine returns NodeId from Ledger Name
        ' as defined in the Ledger table

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ledger_name", vValue:=v_sLedgerName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 01/08/2002: Added for multi-branch accounting
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_branch_id", vValue:=v_lSubBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectNodeFromLedgerSQL, sSQLName:=ACSelectNodeFromLedgerName, bStoredProcedure:=ACSelectNodeFromLedgerStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Count() <> 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Gudie No 162
            r_lNodeId = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("node_id"))

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNodeFromLedger Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeFromLedger", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Ledgers and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oLedgers.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oLedger = m_oLedgers.Item(m_lCurrentRecord)

            ' Get the Ledger Property Values

            'Developer Guide No.  
            m_lReturn = GetProperties(oLedger, iStatus, vLedgerID:=vLedgerID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vLedgerName:=vLedgerName, vLedgerShortName:=vLedgerShortName, vMappingID:=vMappingID, vLedgertypeID:=vLedgertypeID, vIsDeletable:=vIsDeletable, vCurrentPeriodID:=vCurrentPeriodID, vSequence:=vSequence)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oLedger = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Ledger into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oLedgers.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Ledger
            oLedger = New bACTLedger.Ledger()

            ' Populate Ledger Attributes










            'developer guide no.98
            m_lReturn = SetProperties(oLedger, gPMConstants.PMEComponentAction.PMAdd,
                 vLedgerID:=vLedgerID,
                 vCompanyID:=vCompanyID,
                 vSubBranchID:=vSubBranchID,
                 vLedgerName:=vLedgerName,
                 vLedgerShortName:=vLedgerShortName,
                 vMappingID:=vMappingID,
                 vLedgertypeID:=vLedgertypeID,
                 vIsDeletable:=vIsDeletable,
                 vCurrentPeriodID:=vCurrentPeriodID,
                 vSequence:=vSequence)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oLedger = Nothing
                Return result
            End If

            ' Add Ledger to collection
            If (m_oLedgers.Count = 0) Then
                m_oLedgers.Add(Nothing)
            End If
            m_lReturn = m_oLedgers.Add(oNewLedger:=oLedger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oLedger = Nothing
                Return result
            End If

            oLedger = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Ledger
    '              specified and updates the Ledger with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oLedgers.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oLedger = m_oLedgers.Item(lRow)

            ' Check the Status of the Ledger

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oLedger.DatabaseStatus
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

            ' Update Ledger Attributes

            'developer guide no.98
            m_lReturn = SetProperties(oLedger, iStatus,
                 vLedgerID:=vLedgerID,
                 vCompanyID:=vCompanyID,
                 vSubBranchID:=vSubBranchID,
                 vLedgerName:=vLedgerName,
                 vLedgerShortName:=vLedgerShortName,
                 vMappingID:=vMappingID,
                 vLedgertypeID:=vLedgertypeID,
                 vIsDeletable:=vIsDeletable,
                 vCurrentPeriodID:=vCurrentPeriodID,
                 vSequence:=vSequence)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oLedger = Nothing
                Return result
            End If

            ' Release reference to Ledger
            oLedger = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Ledger can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oLedger As bACTLedger.Ledger

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oLedgers.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oLedger = m_oLedgers.Item(lRow)

            ' Check the Status of the Ledger

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oLedger.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oLedger.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oLedger.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Ledger
            oLedger = Nothing

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
            For lSub As Integer = 1 To m_oLedgers.Count()
                Select Case m_oLedgers.Item(lSub).DatabaseStatus
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
        Dim oLedger As bACTLedger.Ledger
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oLedgers.Count()
                oLedger = m_oLedgers.Item(lSub)


                Select Case oLedger.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = AddItem(oLedger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = UpdateItem(oLedger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = DeleteItem(oLedger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oLedger = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oLedgers.Count()

                        ' With the item
                        With m_oLedgers.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oLedgers.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
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
    '****************************************************************** '
    ' Name: GetMappingDetails (Public)
    '
    ' Description: Selects Mapping details by Company and optional
    '              Mapping Type
    '
    '****************************************************************** '
    Public Function GetMappingDetails(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal iCompanyID As Integer) As Integer
        Return GetMappingDetails(lNumberOfRecords:=lNumberOfRecords, vResultArray:=vResultArray, iCompanyID:=iCompanyID, vMapTypeID:=0)
    End Function

    Public Function GetMappingDetails(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, ByVal iCompanyID As Integer, ByVal vMapTypeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CompanyID parameter (INPUT)

            ' MapTypeID

            If Informations.IsNothing(vMapTypeID) Then

                vMapTypeID = Nothing
            Else
                If vMapTypeID = -1 Then

                    vMapTypeID = Nothing
                End If
            End If
            ' Add the MapTypeID parameter (INPUT)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllMappingSQL, sSQLName:=ACGetAllMappingName, bStoredProcedure:=ACGetAllMappingStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMappingDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMNotFound
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ''''' MessageBox.Show(Informations.Err().Description, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMappingDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMappingDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Return result

Err_Parameter_Add:

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMappingDetails", excep:=ex)

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
    Private Function AddItem(ByRef oLedger As bACTLedger.Ledger) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add LedgerID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="ledger_id", vValue:=CStr(oLedger.LedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oLedger:=oLedger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oLedger.LedgerID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("Ledger_id").Value)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oLedger As bACTLedger.Ledger) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add LedgerID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Ledger_id", vValue:=CStr(oLedger.LedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oLedger:=oLedger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oLedger.Timestamp, _
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
    Private Function DeleteItem(ByRef oLedger As bACTLedger.Ledger) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the LedgerID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Ledger_id", vValue:=CStr(oLedger.LedgerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Ledger properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oLedger As bACTLedger.Ledger, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0

        ' Developer Guide No 112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oLedger

            .LedgerID = gPMFunctions.NullToLong(oFields("ledger_id"))
            .CompanyID = gPMFunctions.NullToLong(oFields("company_id"))
            .SubBranchID = gPMFunctions.NullToLong(oFields("sub_branch_id"))
            .LedgerName = gPMFunctions.NullToString(oFields("ledger_name"))
            .LedgerShortName = gPMFunctions.NullToString(oFields("ledger_short_name"))
            .MappingID = gPMFunctions.NullToLong(oFields("mapping_id"))
            .LedgertypeID = gPMFunctions.NullToLong(oFields("ledgertype_id"))
            .IsDeletable = gPMFunctions.NullToLong(oFields("is_deletable"))
            .CurrentPeriodID = gPMFunctions.NullToLong(oFields("current_period_id"))
            .Sequence = gPMFunctions.NullToLong(oFields("sequence"))
            .Sequence = gPMFunctions.NullToLong(oFields("sequence"))
            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Ledger property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function SetProperties(ByRef oLedger As bACTLedger.Ledger, ByRef iStatus As Integer, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        'JK081298 - Default parameters moved to check any missing values
        'passed through before setting them

        ' Default Any Missing Parameters
        m_lReturn = DefaultParameters(bDefaultAll:=False, vLedgerID:=vLedgerID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vLedgerName:=vLedgerName, vLedgerShortName:=vLedgerShortName, vMappingID:=vMappingID, vLedgertypeID:=vLedgertypeID, vIsDeletable:=vIsDeletable, vCurrentPeriodID:=vCurrentPeriodID, vSequence:=vSequence)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vLedgerID:=vLedgerID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vLedgerName:=vLedgerName, vLedgerShortName:=vLedgerShortName, vMappingID:=vMappingID, vLedgertypeID:=vLedgertypeID, vIsDeletable:=vIsDeletable, vCurrentPeriodID:=vCurrentPeriodID, vSequence:=vSequence)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vLedgerID:=vLedgerID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vLedgerName:=vLedgerName, vLedgerShortName:=vLedgerShortName, vMappingID:=vMappingID, vLedgertypeID:=vLedgertypeID, vIsDeletable:=vIsDeletable, vCurrentPeriodID:=vCurrentPeriodID, vSequence:=vSequence)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oLedger


            If Not Informations.IsNothing(vLedgerID) Then
                If .LedgerID <> vLedgerID Then
                    .LedgerID = vLedgerID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vSubBranchID) Then
                If .SubBranchID <> vSubBranchID Then
                    .SubBranchID = vSubBranchID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vLedgerName) Then
                If .LedgerName.Trim() <> vLedgerName.Trim() Then
                    .LedgerName = vLedgerName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vLedgerShortName) Then
                If .LedgerShortName.Trim() <> vLedgerShortName.Trim() Then
                    .LedgerShortName = vLedgerShortName
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vMappingID) Then
                If .MappingID <> vMappingID Then
                    .MappingID = vMappingID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vLedgertypeID) Then
                If .LedgertypeID <> vLedgertypeID Then
                    .LedgertypeID = vLedgertypeID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vIsDeletable) Then
                If .IsDeletable <> vIsDeletable Then
                    .IsDeletable = vIsDeletable
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrentPeriodID) Then
                If .CurrentPeriodID <> vCurrentPeriodID Then
                    .CurrentPeriodID = vCurrentPeriodID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vSequence) Then
                If .Sequence <> vSequence Then
                    .Sequence = vSequence
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
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Ledger property values.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Private Function GetProperties(ByRef oLedger As bACTLedger.Ledger, ByRef iStatus As Integer, Optional ByRef vLedgerID As Integer = 0, Optional ByRef vCompanyID As Integer = 0, Optional ByRef vSubBranchID As Integer = 0, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As String = "", Optional ByRef vMappingID As Integer = 0, Optional ByRef vLedgertypeID As Integer = 0, Optional ByRef vIsDeletable As Integer = 0, Optional ByRef vCurrentPeriodID As Integer = 0, Optional ByRef vSequence As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oLedger

            'Developer Guide No 143
            'Starts

            vLedgerID = .LedgerID




            vCompanyID = .CompanyID




            vSubBranchID = .SubBranchID




            vLedgerName = .LedgerName




            vLedgerShortName = .LedgerShortName




            vMappingID = .MappingID




            vLedgertypeID = .LedgertypeID




            vIsDeletable = .IsDeletable




            vCurrentPeriodID = .CurrentPeriodID




            vSequence = .Sequence

            'Ends
            iStatus = .DatabaseStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oLedger As bACTLedger.Ledger) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' CTAF 020101 - Rearranged order of parameters
        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="sequence", vValue:=CStr(oLedger.Sequence), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oLedger.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=CStr(oLedger.SubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ledger_name", vValue:=oLedger.LedgerName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ledger_short_name", vValue:=oLedger.LedgerShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oLedger.MappingID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="mapping_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="mapping_id", vValue:=oLedger.MappingID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oLedger.LedgertypeID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="ledgertype_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="ledgertype_id", vValue:=CStr(oLedger.LedgertypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="current_period_id", vValue:=CStr(oLedger.CurrentPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_deletable", vValue:=CStr(oLedger.IsDeletable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Ledger.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no.44
        'start
        If (Informations.IsNothing(vLedgerID)) OrElse (vLedgerID.Equals(0)) Or (bDefaultAll) Then
            vLedgerID = 0
        End If



        If (Informations.IsNothing(vCompanyID)) OrElse (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vSubBranchID)) OrElse (vSubBranchID.Equals(0)) Or (bDefaultAll) Then
            vSubBranchID = 0
        End If



        If (Informations.IsNothing(vLedgerName)) OrElse (String.IsNullOrEmpty(vLedgerName)) Or (bDefaultAll) Then
            vLedgerName = ""
        End If



        If (Informations.IsNothing(vLedgerShortName)) OrElse (String.IsNullOrEmpty(vLedgerShortName)) Or (bDefaultAll) Then
            vLedgerShortName = ""
        End If



        If (Informations.IsNothing(vMappingID)) OrElse (vMappingID.Equals(0)) Or (bDefaultAll) Then
            vMappingID = 0
        End If



        If (Informations.IsNothing(vLedgertypeID)) OrElse (vLedgertypeID.Equals(0)) Or (bDefaultAll) Then
            vLedgertypeID = 0
        End If



        If (Informations.IsNothing(vIsDeletable)) OrElse (vIsDeletable.Equals(0)) Or (bDefaultAll) Then
            vIsDeletable = 0
        End If



        If (Informations.IsNothing(vCurrentPeriodID)) OrElse (vCurrentPeriodID.Equals(0)) Or (bDefaultAll) Then
            vCurrentPeriodID = 0
        End If



        If (Informations.IsNothing(vSequence)) OrElse (vSequence.Equals(0)) Or (bDefaultAll) Then
            vSequence = 0
        End If

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Ledger.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vLedgerID)) Or (Object.Equals(vLedgerID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vSubBranchID)) Or (Object.Equals(vSubBranchID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vLedgerName)) Or (Object.Equals(vLedgerName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vLedgerShortName)) Or (Object.Equals(vLedgerShortName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vMappingID)) Or (Object.Equals(vMappingID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vLedgertypeID)) Or (Object.Equals(vLedgertypeID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vIsDeletable)) Or (Object.Equals(vIsDeletable, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrentPeriodID)) Or (Object.Equals(vCurrentPeriodID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vSequence)) Or (Object.Equals(vSequence, Nothing)) Then
            result = gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Ledger for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vLedgerID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vLedgerName As Object = Nothing, Optional ByRef vLedgerShortName As Object = Nothing, Optional ByRef vMappingID As Object = Nothing, Optional ByRef vLedgertypeID As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vCurrentPeriodID As Object = Nothing, Optional ByRef vSequence As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vLedgerID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vSubBranchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vMappingID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vLedgertypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(vIsDeletable), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp7 As Double
        If Not Double.TryParse(CStr(vCurrentPeriodID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp8 As Double
        If Not Double.TryParse(CStr(vSequence), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
