Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 08/08/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Auditset.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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

    ' Collection of Auditsets (Private)
    Private m_oAuditsets As bACTAuditset.Auditsets

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
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer


    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oAuditsets.Count()
                    m_lCurrentRecord = m_oAuditsets.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oAuditsets.Count()

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
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
            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
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

            ' Create Auditsets Collection
            m_oAuditsets = New bACTAuditset.Auditsets()


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

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
                    m_oLookup = Nothing
                End If
                m_oAuditsets = Nothing
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
    ' Description: Gets the Lookup values for a Auditset.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 1) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names
            'vTabArray(PMLookupTableName, 0) = PMLookupEventRepeatType
            'vTabArray(PMLookupTableName, 1) = PMLookupEventType

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oAuditset = m_oAuditsets.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oAuditset

                        ' {* USER DEFINED CODE (Begin) *}
                        'vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        'vTabArray(PMLookupKey, 1) = .EventTypeID
                        'dtEffectiveDate = .EffectiveDate
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oAuditset

                        ' {* USER DEFINED CODE (Begin) *}
                        'vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        'vTabArray(PMLookupKey, 1) = .EventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Auditset reference
            oAuditset = Nothing

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
    ' Description: Adds a single Auditset directly into the database.
    '        Note: The Auditset will NOT be added to the collection.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vAuditsetID As Integer = 0, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer

        ' AMB 05/02/2003 - parameters added above

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Auditset
            oAuditset = New bACTAuditset.Auditset()

            ' Populate Auditset Attributes









            m_lReturn = CType(SetProperties(oAuditset, gPMConstants.PMEComponentAction.PMAdd, vAuditsetID:=vAuditsetID, vCompanyID:=vCompanyID, vUserID:=vUserID, vPostedDate:=vPostedDate, vComment:=vComment, vDocumentID:=vDocumentID, vAuditSetTypeID:=vAuditSetTypeID, vApprovedDate:=vApprovedDate, vApprovedUserID:=vApprovedUserID, vRejected:=vRejected, vRejectedUserID:=vRejectedUserID, vCashListItemID:=vCashListItemID), gPMConstants.PMEReturnCode)

            ' AMB 05/02/2003
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Auditset to the Database
            m_lReturn = CType(AddItem(oAuditset), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Auditset Added

            If Not Informations.IsNothing(vAuditsetID) Then
                vAuditsetID = oAuditset.AuditsetID
            End If

            ' {* USER DEFINED CODE (End) *}

            oAuditset = Nothing

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
    ' Description: Deletes a single Auditset directly from the database.
    '        Note: The Auditset will NOT be deleted from the collection.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer

        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Auditset
            oAuditset = New bACTAuditset.Auditset()

            ' Populate Auditset Attributes










            m_lReturn = CType(SetProperties(oAuditset, gPMConstants.PMEComponentAction.PMDelete, vAuditsetID:=CInt(vAuditsetID), vCompanyID:=CInt(vCompanyID), vUserID:=CInt(vUserID), vPostedDate:=vPostedDate, vComment:=CStr(vComment), vDocumentID:=CStr(vDocumentID), vAuditSetTypeID:=CStr(vAuditSetTypeID), vApprovedDate:=vApprovedDate, vApprovedUserID:=CStr(vApprovedUserID), vRejected:=CStr(vRejected), vRejectedUserID:=CStr(vRejectedUserID), vCashListItemID:=CStr(vCashListItemID)), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Auditset to the Database
            m_lReturn = CType(DeleteItem(oAuditset), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oAuditset = Nothing

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
    ' Description: Returns the Default Values for the Auditset.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults













            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAuditsetID:=CByte(vAuditsetID), vCompanyID:=CByte(vCompanyID), vUserID:=CByte(vUserID), vPostedDate:=CDate(vPostedDate), vComment:=CStr(vComment), vDocumentID:=CByte(vDocumentID), vAuditSetTypeID:=CByte(vAuditSetTypeID), vApprovedDate:=CDate(vApprovedDate), vApprovedUserID:=CByte(vApprovedUserID), vRejected:=CInt(vRejected), vRejectedUserID:=CByte(vRejectedUserID), vCashListItemID:=CByte(vCashListItemID)), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

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
        'developer guide no 21
        Dim oFields As DataRow

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
            '    If (Trim$(vTable) <> PMTableAuditset) Then

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
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""
                            Case DbType.Date, ADODB.DataTypeEnum.adDBDate

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
    ' Description: Gets the required Auditsets and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oAuditset As bACTAuditset.Auditset

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oAuditsets.Clear()

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

            If Not Informations.IsNothing(vAuditsetID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vAuditsetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vAuditsetID =" & CStr(vAuditsetID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the AuditsetID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Auditset_id", vValue:=CStr(vAuditsetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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

                    ' Create New Auditset
                    oAuditset = New bACTAuditset.Auditset()

                    m_lReturn = CType(SetPropertiesFromDB(oAuditset:=oAuditset, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Auditset to collection
                    If (m_oAuditsets.Count = 0) Then
                        m_oAuditsets.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oAuditsets.Add(oNewAuditset:=oAuditset), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oAuditset = Nothing

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

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Auditsets and populate the Collection
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oAuditsets.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oAuditset = m_oAuditsets.Item(m_lCurrentRecord)

            ' Get the Auditset Property Values










            m_lReturn = CType(GetProperties(oAuditset, iStatus, vAuditsetID:=CInt(vAuditsetID), vCompanyID:=CInt(vCompanyID), vUserID:=CInt(vUserID), vPostedDate:=vPostedDate, vComment:=CStr(vComment), vDocumentID:=CInt(vDocumentID), vAuditSetTypeID:=CInt(vAuditSetTypeID), vApprovedDate:=vApprovedDate, vApprovedUserID:=CInt(vApprovedUserID), vRejected:=CInt(vRejected), vRejectedUserID:=CInt(vRejectedUserID), vCashListItemID:=CInt(vCashListItemID)), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oAuditset = Nothing


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
    ' Description: Adds the supplied Auditset into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oAuditsets.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Auditset
            oAuditset = New bACTAuditset.Auditset()

            ' Populate Auditset Attributes










            m_lReturn = CType(SetProperties(oAuditset, gPMConstants.PMEComponentAction.PMAdd, vAuditsetID:=CInt(vAuditsetID), vCompanyID:=CInt(vCompanyID), vUserID:=CInt(vUserID), vPostedDate:=vPostedDate, vComment:=CStr(vComment), vDocumentID:=CStr(vDocumentID), vAuditSetTypeID:=CStr(vAuditSetTypeID), vApprovedDate:=vApprovedDate, vApprovedUserID:=CStr(vApprovedUserID), vRejected:=CStr(vRejected), vRejectedUserID:=CStr(vRejectedUserID), vCashListItemID:=CStr(vCashListItemID)), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oAuditset = Nothing
                Return result
            End If

            ' Add Auditset to collection
            If (m_oAuditsets.Count = 0) Then
                m_oAuditsets.Add(Nothing)
            End If
            m_lReturn = CType(m_oAuditsets.Add(oNewAuditset:=oAuditset), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oAuditset = Nothing
                Return result
            End If

            oAuditset = Nothing

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
    ' Description: Validates that this action is valid on the Auditset
    '              specified and updates the Auditset with the new values.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAuditsets.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oAuditset = m_oAuditsets.Item(lRow)

            ' Check the Status of the Auditset

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oAuditset.DatabaseStatus
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

            ' Update Auditset Attributes










            m_lReturn = CType(SetProperties(oAuditset, iStatus, vAuditsetID:=CInt(vAuditsetID), vCompanyID:=CInt(vCompanyID), vUserID:=CInt(vUserID), vPostedDate:=vPostedDate, vComment:=CStr(vComment), vDocumentID:=CStr(vDocumentID), vAuditSetTypeID:=CStr(vAuditSetTypeID), vApprovedDate:=vApprovedDate, vApprovedUserID:=CStr(vApprovedUserID), vRejected:=CStr(vRejected), vRejectedUserID:=CStr(vRejectedUserID), vCashListItemID:=CStr(vCashListItemID)), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oAuditset = Nothing
                Return result
            End If

            ' Release reference to Auditset
            oAuditset = Nothing

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
    ' Description: Validate that the specified Auditset can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oAuditset As bACTAuditset.Auditset

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAuditsets.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oAuditset = m_oAuditsets.Item(lRow)

            ' Check the Status of the Auditset

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oAuditset.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oAuditset.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oAuditset.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Auditset
            oAuditset = Nothing

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
            For lSub As Integer = 1 To m_oAuditsets.Count()
                Select Case m_oAuditsets.Item(lSub).DatabaseStatus
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
        Dim oAuditset As bACTAuditset.Auditset
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oAuditsets.Count()
                oAuditset = m_oAuditsets.Item(lSub)


                Select Case oAuditset.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oAuditset), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oAuditset), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oAuditset), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oAuditset = Nothing

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
                    Do While lSub <= m_oAuditsets.Count()

                        ' With the item
                        With m_oAuditsets.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oAuditsets.Delete(lSub)

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
    Private Function AddItem(ByRef oAuditset As bACTAuditset.Auditset) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oAuditset:=oAuditset), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AuditsetID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Auditset_id", vValue:=oAuditset.AuditsetID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oAuditset.AuditsetID = m_oDatabase.Parameters.Item("Auditset_id").Value

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
    Private Function UpdateItem(ByRef oAuditset As bACTAuditset.Auditset) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oAuditset:=oAuditset), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AuditsetID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Auditset_id", vValue:=CStr(oAuditset.AuditsetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oAuditset.Timestamp, _
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
    Private Function DeleteItem(ByRef oAuditset As bACTAuditset.Auditset) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the AuditsetID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Auditset_id", vValue:=CStr(oAuditset.AuditsetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied Auditset properties from a database
    '              record.
    ' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oAuditset As bACTAuditset.Auditset, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no 21
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details
        With oAuditset

            .AuditsetID = oFields("auditset_id")

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .CompanyID = 0
            Else
                .CompanyID = oFields("company_id")
            End If

            If Convert.IsDBNull(oFields("user_id")) Or Informations.IsNothing(oFields("user_id")) Then
                .UserID = 0
            Else
                .UserID = oFields("user_id")
            End If

            If Convert.IsDBNull(oFields("posted_date")) Or Informations.IsNothing(oFields("posted_date")) Then
                .PostedDate = #12/30/1899#
            Else
                .PostedDate = oFields("posted_date")
            End If

            If Convert.IsDBNull(oFields("comment")) Or Informations.IsNothing(oFields("comment")) Then
                .Comment = ""
            Else
                .Comment = oFields("comment")
            End If

            ' AMB 05/02/2003 - fields added

            If Convert.IsDBNull(oFields("document_id")) Or Informations.IsNothing(oFields("document_id")) Then
                .DocumentID = 0
            Else
                .DocumentID = oFields("document_id")
            End If

            If Convert.IsDBNull(oFields("auditset_type_id")) Or Informations.IsNothing(oFields("auditset_type_id")) Then
                .AuditSetTypeID = 0
            Else
                .AuditSetTypeID = oFields("auditset_type_id")
            End If

            If Convert.IsDBNull(oFields("approved_date")) Or Informations.IsNothing(oFields("approved_date")) Then
                .ApprovedDate = #12/30/1899#
            Else
                .ApprovedDate = oFields("approved_date")
            End If

            If Convert.IsDBNull(oFields("approved_user_id")) Or Informations.IsNothing(oFields("approved_user_id")) Then
                .ApprovedUserID = 0
            Else
                .ApprovedUserID = oFields("approved_user_id")
            End If

            If Convert.IsDBNull(oFields("rejected")) Or Informations.IsNothing(oFields("rejected")) Then
                .Rejected = 0
            Else
                .Rejected = oFields("rejected")
            End If

            If Convert.IsDBNull(oFields("rejected_user_id")) Or Informations.IsNothing(oFields("rejected_user_id")) Then
                .RejectedUserID = 0
            Else
                .RejectedUserID = oFields("rejected_user_id")
            End If

            If Convert.IsDBNull(oFields("cashlistitem_id")) Or Informations.IsNothing(oFields("cashlistitem_id")) Then
                .CashListItemID = 0
            Else
                .CashListItemID = oFields("cashlistitem_id")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Auditset property values.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    'Modified
    'Private Function SetProperties(ByRef oAuditset As bACTAuditSet.Auditset, ByRef iStatus As Integer, Optional ByRef vAuditsetID As Integer = 0, Optional ByRef vCompanyID As Integer = 0, Optional ByRef vUserID As Integer = 0, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As String = "", Optional ByRef vDocumentID As String = "", Optional ByRef vAuditSetTypeID As String = "", Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As String = "", Optional ByRef vRejected As String = "", Optional ByRef vRejectedUserID As String = "", Optional ByRef vCashListItemID As String = "") As Integer
    Private Function SetProperties(ByRef oAuditset As bACTAuditset.Auditset, ByRef iStatus As Integer, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vAuditsetID:=vAuditsetID, vCompanyID:=vCompanyID, vUserID:=vUserID, vPostedDate:=vPostedDate, vComment:=vComment, vDocumentID:=vDocumentID, vAuditSetTypeID:=vAuditSetTypeID, vApprovedDate:=vApprovedDate, vApprovedUserID:=vApprovedUserID, vRejected:=vRejected, vRejectedUserID:=vRejectedUserID, vCashListItemID:=vCashListItemID), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters


            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vAuditsetID:=vAuditsetID, vCompanyID:=vCompanyID, vUserID:=vUserID, vPostedDate:=CDate(vPostedDate), vComment:=vComment, vDocumentID:=CByte(vDocumentID), vAuditSetTypeID:=CByte(vAuditSetTypeID), vApprovedDate:=CDate(vApprovedDate), vApprovedUserID:=CByte(vApprovedUserID), vRejected:=CInt(vRejected), vRejectedUserID:=CByte(vRejectedUserID), vCashListItemID:=CByte(vCashListItemID)), gPMConstants.PMEReturnCode)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vAuditsetID:=vAuditsetID, vCompanyID:=vCompanyID, vUserID:=vUserID, vPostedDate:=vPostedDate, vComment:=vComment, vDocumentID:=vDocumentID, vAuditSetTypeID:=vAuditSetTypeID, vApprovedDate:=vApprovedDate, vApprovedUserID:=vApprovedUserID, vRejected:=vRejected, vRejectedUserID:=vRejectedUserID, vCashListItemID:=vCashListItemID), gPMConstants.PMEReturnCode)
        ' AMB 05/02/2003 - Added parameters above

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oAuditset


            If Not Informations.IsNothing(vAuditsetID) Then
                If .AuditsetID <> vAuditsetID Then
                    .AuditsetID = vAuditsetID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vUserID) Then
                If .UserID <> vUserID Then
                    .UserID = vUserID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPostedDate) Then

                If .PostedDate <> CDate(vPostedDate) Then

                    .PostedDate = CDate(vPostedDate)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vComment) Then
                If .Comment.Trim() <> vComment.Trim() Then
                    .Comment = vComment
                    bDataChanged = True
                End If
            End If

            ' AMB 05/02/2003 - fields added

            If Not Informations.IsNothing(vDocumentID) Then
                If CStr(.DocumentID).Trim() <> vDocumentID.Trim() Then
                    .DocumentID = CInt(vDocumentID)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAuditSetTypeID) Then
                If CStr(.AuditSetTypeID).Trim() <> vAuditSetTypeID.Trim() Then
                    .AuditSetTypeID = CInt(vAuditSetTypeID)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vApprovedDate) Then

                If .ApprovedDate <> CStr(vApprovedDate).Trim() Then

                    .ApprovedDate = CDate(vApprovedDate)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vApprovedUserID) Then
                If CStr(.ApprovedUserID).Trim() <> vApprovedUserID.Trim() Then
                    .ApprovedUserID = CInt(vApprovedUserID)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRejected) Then
                If CStr(.Rejected).Trim() <> vRejected.Trim() Then
                    .Rejected = CInt(vRejected)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vRejectedUserID) Then
                If CStr(.RejectedUserID).Trim() <> vRejectedUserID.Trim() Then
                    .RejectedUserID = CInt(vRejectedUserID)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCashListItemID) Then
                If CStr(.CashListItemID).Trim() <> vCashListItemID.Trim() Then
                    .CashListItemID = CInt(vCashListItemID)
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
    ' Description: Returns the supplied Auditset property values.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oAuditset As bACTAuditset.Auditset, ByRef iStatus As Integer, Optional ByRef vAuditsetID As Integer = 0, Optional ByRef vCompanyID As Integer = 0, Optional ByRef vUserID As Integer = 0, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As String = "", Optional ByRef vDocumentID As Integer = 0, Optional ByRef vAuditSetTypeID As Integer = 0, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Integer = 0, Optional ByRef vRejected As Integer = 0, Optional ByRef vRejectedUserID As Integer = 0, Optional ByRef vCashListItemID As Integer = 0) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oAuditset


            If Not Informations.IsNothing(vAuditsetID) Then
                vAuditsetID = .AuditsetID
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                vCompanyID = .CompanyID
            End If


            If Not Informations.IsNothing(vUserID) Then
                vUserID = .UserID
            End If


            If Not Informations.IsNothing(vPostedDate) Then

                vPostedDate = .PostedDate
            End If


            If Not Informations.IsNothing(vComment) Then
                vComment = .Comment
            End If

            ' AMB 05/02/2003 - Added fields below

            If Not Informations.IsNothing(vDocumentID) Then
                vDocumentID = .DocumentID
            End If


            If Not Informations.IsNothing(vAuditSetTypeID) Then
                vAuditSetTypeID = .AuditSetTypeID
            End If


            If Not Informations.IsNothing(vApprovedDate) Then

                vApprovedDate = .ApprovedDate
            End If


            If Not Informations.IsNothing(vApprovedUserID) Then
                vApprovedUserID = .ApprovedUserID
            End If


            If Not Informations.IsNothing(vRejected) Then
                vRejected = .Rejected
            End If


            If Not Informations.IsNothing(vRejectedUserID) Then
                vRejectedUserID = .RejectedUserID
            End If


            If Not Informations.IsNothing(vCashListItemID) Then
                vCashListItemID = .CashListItemID
            End If

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
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oAuditset As bACTAuditset.Auditset) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If oAuditset.CompanyID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oAuditset.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAuditset.UserID < 1 Then

                'developer guide no 21
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(oAuditset.UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="posted_date", vValue:=oAuditset.PostedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="comment", vValue:=oAuditset.Comment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AMB 05/02/2003 - Added fields below
            If oAuditset.DocumentID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="document_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="document_id", vValue:=CStr(oAuditset.DocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAuditset.AuditSetTypeID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="auditset_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="auditset_type_id", vValue:=CStr(oAuditset.AuditSetTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="approved_date", vValue:=oAuditset.ApprovedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAuditset.ApprovedUserID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="approved_user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="approved_user_id", vValue:=CStr(oAuditset.ApprovedUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="rejected", vValue:=CStr(oAuditset.Rejected), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAuditset.RejectedUserID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="rejected_user_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="rejected_user_id", vValue:=CStr(oAuditset.RejectedUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAuditset.CashListItemID < 1 Then

                'developer guide no 85
                m_lReturn = .Parameters.Add(sName:="cashlistitem_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(oAuditset.CashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Auditset.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAuditsetID As Byte = 0, Optional ByRef vCompanyID As Byte = 0, Optional ByRef vUserID As Byte = 0, Optional ByRef vPostedDate As Date = #12/30/1899#, Optional ByRef vComment As String = "", Optional ByRef vDocumentID As Byte = 0, Optional ByRef vAuditSetTypeID As Byte = 0, Optional ByRef vApprovedDate As Date = #12/30/1899#, Optional ByRef vApprovedUserID As Byte = 0, Optional ByRef vRejected As gPMConstants.PMEReturnCode = 0, Optional ByRef vRejectedUserID As Byte = 0, Optional ByRef vCashListItemID As Byte = 0) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAuditsetID)) Or (vAuditsetID.Equals(0)) Or (bDefaultAll) Then
            vAuditsetID = 0
        End If



        If (Informations.IsNothing(vCompanyID)) Or (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vUserID)) Or (vUserID.Equals(0)) Or (bDefaultAll) Then
            vUserID = 0
        End If



        If (Informations.IsNothing(vPostedDate)) Or (vPostedDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vPostedDate = DateTime.Now
        End If

        'Developer Guide no 160

        If (Informations.IsNothing(vComment)) Or (bDefaultAll) Then
            vComment = ""
        End If

        ' AMB 05/02/2003 - Added fields below


        If (Informations.IsNothing(vDocumentID)) Or (vDocumentID.Equals(0)) Or (bDefaultAll) Then
            vDocumentID = 0
        End If



        If (Informations.IsNothing(vAuditSetTypeID)) Or (vAuditSetTypeID.Equals(0)) Or (bDefaultAll) Then
            vAuditSetTypeID = 0
        End If



        If (Informations.IsNothing(vApprovedDate)) Or (vApprovedDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vApprovedDate = DateTime.Now
        End If



        If (Informations.IsNothing(vApprovedUserID)) Or (vApprovedUserID.Equals(0)) Or (bDefaultAll) Then
            vApprovedUserID = 0
        End If



        If (Informations.IsNothing(vRejected)) Or (bDefaultAll) Then
            vRejected = gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Informations.IsNothing(vRejectedUserID)) Or (vRejectedUserID.Equals(0)) Or (bDefaultAll) Then
            vRejectedUserID = 0
        End If



        If (Informations.IsNothing(vCashListItemID)) Or (vCashListItemID.Equals(0)) Or (bDefaultAll) Then
            vCashListItemID = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Auditset.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAuditsetID)) Or (Object.Equals(vAuditsetID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vUserID)) Or (Object.Equals(vUserID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vPostedDate)) Or (Object.Equals(vPostedDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vComment)) Or (Object.Equals(vComment, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' AMB 05/02/2003 - Added fields below
        'eck030703 PN4207 commented out
        '    If (IsMissing(vDocumentID) = True) _
        ''            Or (IsEmpty(vDocumentID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vAuditSetTypeID) = True) _
        ''            Or (IsEmpty(vAuditSetTypeID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        'eck030703 PN4207 End
        '    If (IsMissing(vApprovedDate) = True) _
        ''            Or (IsEmpty(vApprovedDate) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vApprovedUserID) = True) _
        ''            Or (IsEmpty(vApprovedUserID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vRejected) = True) _
        ''            Or (IsEmpty(vRejected)  = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vRejectedUserID) = True) _
        ''            Or (IsEmpty(vRejectedUserID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If
        '
        '    If (IsMissing(vCashListItemID) = True) _
        ''            Or (IsEmpty(vCashListItemID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Auditset for Consistency.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vPostedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vAuditSetTypeID As Object = Nothing, Optional ByRef vApprovedDate As Object = Nothing, Optional ByRef vApprovedUserID As Object = Nothing, Optional ByRef vRejected As Object = Nothing, Optional ByRef vRejectedUserID As Object = Nothing, Optional ByRef vCashListItemID As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}



        'Modified
        'If Not Double.TryParse(CStr(vAuditsetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
        '	Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vAuditsetID) Then
            If Not Informations.IsNumeric(vAuditsetID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Modified
        'Dim dbNumericTemp2 As Double
        'If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If

        If Not Informations.IsNothing(vCompanyID) Then
            If Not Informations.IsNumeric(vCompanyID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Modified

        'If Not Double.TryParse(CStr(vUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vUserID) Then
            If Not Informations.IsNumeric(vUserID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        If Not Informations.IsNothing(vPostedDate) Then
            If Not Informations.IsDate(vPostedDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        ' AMB 05/02/2003 - Added fields below
        'Modified

        'If Not Double.TryParse(CStr(vDocumentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vDocumentID) Then
            If Not Informations.IsNumeric(vDocumentID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Modified

        'If Not Double.TryParse(CStr(vAuditSetTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vAuditSetTypeID) Then
            If Not Informations.IsNumeric(vAuditSetTypeID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        If Not Informations.IsNothing(vApprovedDate) Then
            If Not Informations.IsDate(vApprovedDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'Modified

        'If Not Double.TryParse(CStr(vApprovedUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vApprovedUserID) Then
            If Not Informations.IsNumeric(vApprovedUserID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Modified

        'If Not Double.TryParse(CStr(vRejected), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vRejected) Then
            If Not Informations.IsNumeric(vRejected) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Modified

        'If Not Double.TryParse(CStr(vRejectedUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vRejectedUserID) Then
            If Not Informations.IsNumeric(vRejectedUserID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Modified

        'If Not Double.TryParse(CStr(vCashListItemID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If
        If Not Informations.IsNothing(vCashListItemID) Then
            If Not Informations.IsNumeric(vCashListItemID) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
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


    '*************************************************************************
    'Name:          GetAuditSetFromCashListItemID
    'Description:   Gets an Audit Set for a prticular CashListItem
    'History:       07/02/2003 - TR - Created
    '*************************************************************************
    Public Function GetAuditSetFromCashListItemID(ByVal v_lCashListItemID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Use the database Object
            m_oDatabase.Parameters.Clear()

            'TR - Add the search parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAuditSetbyCashListSQL, sSQLName:=ACGetAuditSetbyCashListName, bStoredProcedure:=ACGetAuditSetbyCashListStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetAuditSetFromCashListItemID " & "Failed", ACApp, ACClass, "GetAuditSetFromCashListItemID", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
