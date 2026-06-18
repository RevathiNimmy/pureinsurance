Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129
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
    '              a Document.
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

    ' Collection of Documents (Private)
    Private m_oDocuments As bACTDocument.Documents

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
    Private m_oLookup As bPMLookup.Business

    'DC030806
    Private m_lDocumentId As Integer

    'DC040806
    Private m_oSystemOption As bSIROptions.Business
    Private m_oS4BDatabase As dPMDAO.Database

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
                Case Is > m_oDocuments.Count()
                    m_lCurrentRecord = m_oDocuments.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oDocuments.Count()

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
    Public ReadOnly Property Details() As Documents
        Get

            Return m_oDocuments

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    'DC030806

    Public Property DocumentID() As Integer
        Get

            Return m_lDocumentId

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentId = Value

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

            'SD 24/07/2002 variable name change

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
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

            ' Create Documents Collection
            m_oDocuments = New bACTDocument.Documents()

            ' Create PM Lookup Business Object
            '    Set m_oLookup = New bPMLookup.Business
            '
            '    ' Initialise PM Lookup Business passing our Database Reference.
            '    m_lReturn& = m_oLookup.Initialise( _
            ''        sUsername:=sUsername, _
            ''        sPassword:=sPassword, _
            ''        iUserID:=iUserID, _
            ''        iSourceID:=iSourceID, _
            ''        iLanguageID:=iLanguageID, _
            ''        iCurrencyID:=iCurrencyID, _
            ''        iLogLevel:=iLogLevel, _
            ''        sCallingAppName:=ACApp, _
            ''        vDatabase:=m_oDatabase)

            'SD 24/07/2002 variable name change

            m_oLookup = New BPMLOOKUP.Business
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            ' Remove component services

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
                m_oDocuments = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                m_oDatabase = Nothing
            End If
        End If

        ' Release reference to PM Data Access Object

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
    ' Description: Gets the Lookup values for a Document.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 33
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document = Nothing
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
                oDocument = m_oDocuments.Item(m_lCurrentRecord)
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
                    With oDocument

                        ' {* USER DEFINED CODE (Begin) *}
                        'vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        'vTabArray(PMLookupKey, 1) = .EventTypeID
                        'dtEffectiveDate = .EffectiveDate
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oDocument

                        ' {* USER DEFINED CODE (Begin) *}
                        'vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        'vTabArray(PMLookupKey, 1) = .EventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Document reference
            oDocument = Nothing

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
    ' Description: Adds a single Document directly into the database.
    '        Note: The Document will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'sj 01/08/2002 - Add InsuranceFileCnt and Reason as parameters
    Public Function DirectAdd(Optional ByRef vDocumentID As Integer = 0, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Document
            oDocument = New bACTDocument.Document()

            ' Populate Document Attributes
            'sj 01/08/2002 - Add InsuranceFileCnt and Reason as parameters

            'Developer Guide No 98
            m_lReturn = SetProperties(oDocument, gPMConstants.PMEComponentAction.PMAdd, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID, vInsuranceFileCnt:=vInsuranceFileCnt, vReason:=vReason, vSubBranchID:=vSubBranchID, vClaimID:=vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Document to the Database
            m_lReturn = AddItem(oDocument)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Document Added

            If Not Informations.IsNothing(vDocumentID) Then
                vDocumentID = oDocument.DocumentID
            End If

            ' {* USER DEFINED CODE (End) *}

            oDocument = Nothing

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
    ' Description: Deletes a single Document directly from the database.
    '        Note: The Document will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Document
            oDocument = New bACTDocument.Document()

            ' Populate Document Attributes

            'Developer Guide No 98
            m_lReturn = SetProperties(oDocument, gPMConstants.PMEComponentAction.PMDelete, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID, vClaimID:=vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Document to the Database
            m_lReturn = DeleteItem(oDocument)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oDocument = Nothing

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
    ' Description: Returns the Default Values for the Document.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults











            'Developer Guide No 101
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID)

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
        'Developer Guide No 21
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
            '    If (Trim$(vTable) <> PMTableDocument) Then

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
            'Developer Guide No 162
            oFields = m_oDatabase.Records.Item(0).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'PWF 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type

                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar
                                vResults(lSub) = ""
                                ' Case DbType.Date, adDBDate
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
    ' Description: Gets the required Documents and populate the Collection
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetDetails(Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vLockMode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oDocument As bACTDocument.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oDocuments.Clear()

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

            If Not Informations.IsNothing(vDocumentID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vDocumentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vDocumentID =" & CStr(vDocumentID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If
                'eck260201 changed datattype to Long from Integer
                ' Add the DocumentID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_id", vValue:=CStr(vDocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

                    ' Create New Document
                    oDocument = New bACTDocument.Document()

                    'Developer Guide No 147
                    m_lReturn = SetPropertiesFromDB(oDocument:=oDocument, lRecordNumber:=lSub - 1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Document to collection
                    If (m_oDocuments.Count = 0) Then
                        m_oDocuments.Add(Nothing)
                    End If
                    m_lReturn = m_oDocuments.Add(oNewDocument:=oDocument)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oDocument = Nothing

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
    ' Description: Gets the required Documents and populate the Collection
    '
    ' Change History:
    ' AR20041001 - PN15316 Added optional parameter SubBranchId
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oDocuments.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oDocument = m_oDocuments.Item(m_lCurrentRecord)

            ' Get the Document Property Values
            ' AR20041001 - PN15316 Retrieve the SubBranchId from the document properties

            'Developer Guide No 98
            m_lReturn = GetProperties(oDocument, iStatus, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID, vSubBranchID:=vSubBranchID, vInsuranceFileCnt:=vInsuranceFileCnt, vReason:=vReason, vClaimID:=vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oDocument = Nothing


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
    ' Description: Adds the supplied Document into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oDocuments.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Document
            oDocument = New bACTDocument.Document()

            ' Populate Document Attributes


            'Developer Guide No 98
            m_lReturn = SetProperties(oDocument, gPMConstants.PMEComponentAction.PMAdd, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID, vClaimID:=vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oDocument = Nothing
                Return result
            End If

            ' Add Document to collection
            If (m_oDocuments.Count = 0) Then
                m_oDocuments.Add(Nothing)
            End If
            m_lReturn = m_oDocuments.Add(oNewDocument:=oDocument)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oDocument = Nothing
                Return result
            End If

            oDocument = Nothing

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
    ' Description: Validates that this action is valid on the Document
    '              specified and updates the Document with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oDocuments.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oDocument = m_oDocuments.Item(lRow)

            ' Check the Status of the Document

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oDocument.DatabaseStatus
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

            ' Update Document Attributes

            'Developer Guide No.98
            m_lReturn = SetProperties(oDocument, iStatus, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID, vInsuranceFileCnt:=vInsuranceFileCnt, vReason:=vReason, vClaimID:=vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oDocument = Nothing
                Return result
            End If

            ' Release reference to Document
            oDocument = Nothing

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
    ' Description: Validate that the specified Document can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oDocument As bACTDocument.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oDocuments.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oDocument = m_oDocuments.Item(lRow)

            ' Check the Status of the Document

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oDocument.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oDocument.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oDocument.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Document
            oDocument = Nothing

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
            For lSub As Integer = 1 To m_oDocuments.Count()
                Select Case m_oDocuments.Item(lSub).DatabaseStatus
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
        Dim oDocument As bACTDocument.Document
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oDocuments.Count()
                oDocument = m_oDocuments.Item(lSub)


                Select Case oDocument.DatabaseStatus
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
                        m_lReturn = AddItem(oDocument)
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
                        m_lReturn = UpdateItem(oDocument)
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
                        m_lReturn = DeleteItem(oDocument)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oDocument = Nothing

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
                    Do While lSub <= m_oDocuments.Count()

                        ' With the item
                        With m_oDocuments.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oDocuments.Delete(lSub)

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

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '          17/06/2003 KG - Pasted this module in.
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
    ' Name: CheckValid
    '
    ' Description: Makes sure a passed dd/mm/yy combo is valid, if the day
    '              exceeds the days in the month, then its set to the last
    '              day of the month.
    '
    ' ***************************************************************** '
    Private Function CheckValid(ByRef r_iDate As Integer, ByVal v_iMonth As Integer, ByVal v_iYear As Integer) As Integer

        Dim result As Integer = 0
        Dim iMonthDays(11) As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        iMonthDays(0) = 31 ' jan
        iMonthDays(1) = 28 ' feb
        iMonthDays(2) = 31 ' mar
        iMonthDays(3) = 30 ' apr
        iMonthDays(4) = 31 ' may
        iMonthDays(5) = 30 ' jun
        iMonthDays(6) = 31 ' jul
        iMonthDays(7) = 31 ' aug
        iMonthDays(8) = 30 ' sep
        iMonthDays(9) = 31 ' oct
        iMonthDays(10) = 30 ' nov
        iMonthDays(11) = 31 ' dec

        ' Check for leap year or not
        If (v_iYear / 4) = 0 Then
            ' Adjust february if its a leap year
            iMonthDays(1) = 29
        End If

        If r_iDate > iMonthDays(v_iMonth - 1) Then
            r_iDate = iMonthDays(v_iMonth - 1)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDatePlusXMonths
    '
    ' Description: Adds v_vMonths to the month, and sets the day to
    '              the offset
    '
    ' ***************************************************************** '
    Public Function GetDatePlusXMonths(ByVal v_vCurrentDate As Date, ByRef r_vNextDate As String, ByVal v_vOffset As Object, ByVal v_vMonths As Object) As Integer

        Dim result As Integer = 0
        Dim iMonth, iDay, iYear As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CF071098 - Possibly find a solution using DateAdd function
            'v_vNextDate = DateAdd("m", 1, v_vCurrentDate)

            ' Split the date up into components
            iMonth = v_vCurrentDate.Month
            iYear = v_vCurrentDate.Year

            ' Use the day the user wants

            iDay = CInt(v_vOffset)

            ' Increment the month

            iMonth += CInt(v_vMonths)

            'KN(CMG) 28/02/03 PN2486 - ECK 030703 PN4247
            ' Loop round to the next year, if needed
            '    If (iMonth% > 12) Then
            '        iMonth% = 1
            '        iYear% = iYear% + 1
            '    End If
            Do While iMonth > 12

                iMonth -= 12
                iYear += 1

            Loop

            ' Make sure the days are valid for that month
            m_lReturn = CheckValid(r_iDate:=iDay, v_iMonth:=iMonth, v_iYear:=iYear)

            ' concatenate the date
            r_vNextDate = CStr(iDay) & "/" & CStr(iMonth) & "/" & CStr(iYear)

            ' format it to dd/mm/yy so it dd and mm dont get mixed up

            'Tomo281101 - added in for 1.8 merge
            'eck041001 remove this line which causes problems with non UK currency
            '    r_vNextDate = Format(r_vNextDate, "dd/mm/yy")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDatePlusXMonthsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDatePlusXMonths", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDateNext
    '
    ' Description: Given the date, returns the equivalent date next
    '              period, month or quarter.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetDateNext(ByVal v_iNextType As Integer, ByVal v_vCurrentDate As Object, ByVal v_vOffset As Object, ByRef r_vNextDate As Object) As Integer

        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Form
        Dim lPeriodID As Integer
        Dim dtLastDate As Date

        ' "Next" Types
        Const NEXT_PERIOD As Integer = 1
        Const NEXT_MONTH As Integer = 2
        Const NEXT_QUARTER As Integer = 3

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' What do we want the next of?

            Select Case v_iNextType
                Case NEXT_PERIOD


                    ' Create an instance of the period object

                    oPeriod = New bACTPeriod.Form
                    m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    ' Remove Server Component Services

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the current period

                    m_lReturn = oPeriod.GetPeriodForDate(dtDateInPeriod:=v_vCurrentDate, lPeriodID:=lPeriodID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the end date for the current period

                    m_lReturn = oPeriod.GetDetails(vPeriodID:=lPeriodID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the end date for that period

                    m_lReturn = oPeriod.GetNext(vPeriodID:=lPeriodID, vPeriodEndDate:=dtLastDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add the offset
                    r_vNextDate = dtLastDate.AddDays(v_vOffset.ToOADate())

                    ' Remove the instance of the object

                    oPeriod.Dispose()
                    oPeriod = Nothing

                Case NEXT_MONTH

                    ' Get the date next month

                    'Developer Guide No 98
                    m_lReturn = GetDatePlusXMonths(v_vCurrentDate:=v_vCurrentDate, r_vNextDate:=r_vNextDate, v_vOffset:=v_vOffset, v_vMonths:=1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get date next month.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDateNext", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                Case NEXT_QUARTER

                    ' Get the date next month

                    'Developer Guide No 98
                    m_lReturn = GetDatePlusXMonths(v_vCurrentDate:=v_vCurrentDate, r_vNextDate:=r_vNextDate, v_vOffset:=v_vOffset, v_vMonths:=3)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get date next month.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDateNext", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                Case Else
                    ' Bad next type passed
                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDateNext", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDateNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerForAccount
    '
    ' Description: Gets the ledger that the account passed.
    '
    ' ***************************************************************** '
    Private Function GetLedgerForAccount(ByVal v_lAccountID As Integer, ByRef r_lLedgerID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' construct the sql
        sSQL = "SELECT ledger_id FROM account WHERE account_id = " & v_lAccountID

        ' perform the sql
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLedgerForAccount", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' check the result
        If Not Informations.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMFalse
        Else

            r_lLedgerID = CInt(vResultArray(0, 0))
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AddCompleteDocument (Public)
    '
    ' Description: Process a complete Document  with its
    '              sub-ordinate transaction details
    '
    ' ***************************************************************** '
    Public Function AddCompleteDocument(ByVal v_vInputs() As Object, ByRef r_vOutputs() As Object, ByRef r_vSuspenseArray(,) As Object, ByRef r_sAccountingBasis As String) As Integer

        'Objects
        Dim result As Integer = 0
        Dim oPeriod As bACTPeriod.Form
        Dim oTransDetail As Object = Nothing

        Dim iCompanyID, iUserID As Integer
        Dim dtPostedDate As Date
        Dim sComment As String = ""
        Dim dtCreatedDate, dtAuthorisedDate, dtAccountingDate As Date

        'Document Variables
        Dim m_lDocumentId As Integer
        Dim iPostingstatusID, iDocumenttypeID As Integer
        Dim sDocumentRef As New StringsHelper.FixedLengthString(25)
        Dim dtDocumentDate As Date
        Dim lRow As Integer

        'TransactionDetail Variables
        Dim lTransDetailID, lAccountID As Integer
        Dim iCurrencyID As Integer
        Dim lPeriodID As Integer
        Dim iDocumentSequence As Integer
        Dim cAmount As Decimal
        Dim iFullyMatched As Integer
        Dim cCurrencyAmount As Decimal

        Dim vdCurrencyBaseXrate As Object
        Dim iThirdCurrency As Integer
        Dim cThirdCurrencyAmount As Decimal
        Dim vdThirdCurrencyBaseXrate As Object

        Dim lDestinationAccountId As Integer

        Dim sInsuranceRef As New StringsHelper.FixedLengthString(30)
        Dim iOperatorID As Integer
        Dim sPurchaseOrderNo As New StringsHelper.FixedLengthString(40)
        Dim sPurchaseInvoiceNo As New StringsHelper.FixedLengthString(40)
        Dim sDepartment As New StringsHelper.FixedLengthString(20)
        Dim sSpare As New StringsHelper.FixedLengthString(20)
        'DC160904 PN13880 added DepartmentId for Cost Centre
        Dim lDepartmentId As Integer
        'DC020806 Added transaction type id
        Dim iTransdetailTypeId As Integer
        'DC030806
        Dim vSuspenseArray(,) As Object = Nothing
        'DC040806
        Dim sAccountingBasisOption As String = ""

        Dim lWriteOffReasonID As Integer

        Dim dtRefDate As Date
        Dim cRefAmount As Decimal
        'Developer Guide no 101
        Dim vdRefQuantity As Object
        Dim sRefUnits As New StringsHelper.FixedLengthString(30)

        'General Variables
        Dim vDocId As Object
        Dim cBalanceCheck, cCurBalanceCheck As Decimal
        Dim bSingleCurrencyDoc As Boolean
        Dim iDocCurrencyID As Integer
        Dim vEuroCcyXRate As Object
        Dim lLedgerID As Integer
        Dim dtDateInPeriod As Date
        Dim bTax As Boolean
        Dim lTaxTransId As Integer

        Dim vValue As Object = Nothing
        Dim sAccountingBasis As String
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instace of TransDetail business object
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oTransDetail, v_sClassName:="bACTTransDetail.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of Period business object

            oPeriod = New bACTPeriod.Form
            m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the ledger for the account of the primary transaction

            m_lReturn = GetLedgerForAccount(v_lAccountID:=CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetLowerBound(0))(ACTBatchConst.ACTTransDetailAccountID)), r_lLedgerID:=lLedgerID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CF080299 - Set the date first (this helps)

            dtDateInPeriod = CDate(v_vInputs(ACTBatchConst.ACTDocumentAccountingDate))

            ' Get the posting period for the document

            m_lReturn = oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodID, lLedgerID:=lLedgerID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            v_vInputs(ACTBatchConst.ACTDocumentAccountingDate) = dtDateInPeriod

            'General
            'MKW140203 Catchup 1.6.9 to 1.8.6 PN1466 PN1966
            'iCompanyID = m_iSourceID%

            iCompanyID = CInt(v_vInputs(ACTBatchConst.ACTDocumentCompany))

            iUserID = m_iUserID
            ' CF0402099 - posting date
            dtPostedDate = DateTime.Now
            dtCreatedDate = DateTime.Now
            dtAuthorisedDate = DateTime.Now

            dtAccountingDate = CDate(v_vInputs(ACTBatchConst.ACTDocumentAccountingDate))

            ' Start Document DB Transaction
            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cBalanceCheck = 0

            'Add document

            iPostingstatusID = CInt(v_vInputs(ACTBatchConst.ACTDocumentStatus))

            iDocumenttypeID = CInt(v_vInputs(ACTBatchConst.ACTDocumentType))

            sDocumentRef.Value = CStr(v_vInputs(ACTBatchConst.ACTDocumentRef)).Trim()

            dtDocumentDate = CDate(v_vInputs(ACTBatchConst.ACTDocumentDate))
            dtCreatedDate = DateTime.Now
            dtAuthorisedDate = DateTime.Now

            sComment = CStr(v_vInputs(ACTBatchConst.ACTDocumentComments)).Trim()
            lWriteOffReasonID = 0

            ' Do a Direct add so we can get the ID
            m_lReturn = DirectAdd(vDocumentID:=m_lDocumentId, vCompanyID:=iCompanyID, vPostingstatusID:=iPostingstatusID, vDocumenttypeID:=iDocumenttypeID, vDocumentRef:=sDocumentRef.Value, vDocumentDate:=dtDocumentDate, vCreatedDate:=dtCreatedDate, vAuthorisedDate:=dtAuthorisedDate, vComment:=sComment, vWriteOffReasonID:=lWriteOffReasonID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            ' CF 250998
            ' These values will need updating for the change to TransDetail table
            '
            bTax = False
            bSingleCurrencyDoc = True


            iDocCurrencyID = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetLowerBound(0))(ACTBatchConst.ACTTransDetailCurrencyID)) 'document currency is currency of first transaction

            Dim vTransIds As Array = Array.CreateInstance(GetType(Object), New Integer() {v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetUpperBound(0) - v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetLowerBound(0) + 1}, New Integer() {v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetLowerBound(0)})

            For j As Integer = v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetLowerBound(0) To v_vInputs(ACTBatchConst.ACTDocumentTransactions).GetUpperBound(0) ' for all transactions in document

                lAccountID = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailAccountID))

                iCurrencyID = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailCurrencyID))
                If iCurrencyID <> iDocCurrencyID Then bSingleCurrencyDoc = False
                iDocumentSequence = j

                dtAccountingDate = CDate(v_vInputs(ACTBatchConst.ACTDocumentAccountingDate))

                cAmount = CDec(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailAmount))
                iFullyMatched = False

                cCurrencyAmount = CDec(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailCurrencyAmount))

                vdCurrencyBaseXrate = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailCurrencyBaseXrate))

                iThirdCurrency = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailThirdCurrency))

                cThirdCurrencyAmount = CDec(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailThirdCurrencyAmount))

                vdThirdCurrencyBaseXrate = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailThirdCurrencyBaseXrate))

                sComment = CStr(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailComment))
                '        sProject = v_vInputs(ACTDocumentTransactions)(j)(ACTTransDetailProject)
                '        sContract = v_vInputs(ACTDocumentTransactions)(j)(ACTTransDetailContract)
                '        sProduct = v_vInputs(ACTDocumentTransactions)(j)(ACTTransDetailProduct)
                '        sDepartment = v_vInputs(ACTDocumentTransactions)(j)(ACTTransDetailDepartment)
                '        sAgent = v_vInputs(ACTDocumentTransactions)(j)(ACTTransDetailAgent)
                '        sClient = v_vInputs(ACTDocumentTransactions)(j)(ACTTransDetailClient)
                'Developer Guide No 236
                'Starts

                'sInsuranceRef.Value = CType(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailInsuranceRef), FixedLengthString)

                'iOperatorID = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailOperatorID))

                'sPurchaseOrderNo.Value = CType(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailPurchaseOrderNo), FixedLengthString)

                'sPurchaseInvoiceNo.Value = CType(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailPurchaseInvoiceNo), FixedLengthString)

                'sDepartment.Value = CType(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailDepartment), FixedLengthString)

                'sSpare.Value = CType(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailSpare), FixedLengthString)
                If v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailInsuranceRef) IsNot Nothing Then
                    sInsuranceRef.Value = v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailInsuranceRef)
                End If

                iOperatorID = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailOperatorID))

                If v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailPurchaseOrderNo) IsNot Nothing Then
                    sPurchaseOrderNo.Value = v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailPurchaseOrderNo)
                End If

                If v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailPurchaseInvoiceNo) IsNot Nothing Then
                    sPurchaseInvoiceNo.Value = v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailPurchaseInvoiceNo)
                End If

                If v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailDepartment) IsNot Nothing Then
                    sDepartment.Value = v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailDepartment)
                End If

                If v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailSpare) IsNot Nothing Then
                    sSpare.Value = v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailSpare)
                End If
                'Ends

                lDepartmentId = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailDepartmentId))

                'DC020806 added transaction type id

                iTransdetailTypeId = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransdetailTypeId))


                dtRefDate = CDate(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailRefDate))

                cRefAmount = CDec(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailRefAmount))

                vdRefQuantity = CInt(v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailRefQuantity))

                'Developer Guide No 236
                If v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailRefUnits) IsNot Nothing Then
                    sRefUnits.Value = v_vInputs(ACTBatchConst.ACTDocumentTransactions)(j)(ACTBatchConst.ACTTransDetailRefUnits)
                End If

                'Developer Guide No 254
                If dtRefDate = DateTime.MinValue Or dtRefDate = #12/30/1899# Then
                    dtRefDate = DateTime.Now
                End If

                'If dtAccountingDate = CDate("00:00:00") Then
                If dtAccountingDate = DateTime.MinValue Or dtAccountingDate = #12/30/1899# Then
                    dtAccountingDate = DateTime.Now
                End If

                vEuroCcyXRate = 1

                lRow = j
                'DC160904 PN13880 added DepartmentId for Cost Centre
                'DC020806 added transdetail type id

                m_lReturn = oTransDetail.EditAdd(lRow:=ToSafeInteger(lRow), vTransdetailID:=ToSafeInteger(lTransDetailID), vAccountID:=ToSafeInteger(lAccountID),
                                                 vPostingstatusID:=ToSafeInteger(iPostingstatusID), vCompanyID:=ToSafeInteger(iCompanyID), vCurrencyID:=ToSafeInteger(iCurrencyID),
                                                 vPeriodID:=ToSafeInteger(lPeriodID), vDocumentID:=ToSafeInteger(m_lDocumentId), vDocumentSequence:=ToSafeInteger(iDocumentSequence),
                                                 vAccountingDate:=ToSafeDate(dtAccountingDate), vAmount:=ToSafeDecimal(cAmount), vFullyMatched:=ToSafeInteger(iFullyMatched),
                                                 vCurrencyAmount:=ToSafeDecimal(cCurrencyAmount), vCurrencyBaseXrate:=vdCurrencyBaseXrate, vEuroCurrencyId:=ToSafeInteger(iThirdCurrency),
                                                 vEuroAmount:=ToSafeDecimal(cThirdCurrencyAmount), vEuroBaseXRate:=vdThirdCurrencyBaseXrate, vEuroCcyXRate:=vEuroCcyXRate, vComment:=ToSafeString(sComment),
                                                 vInsuranceRef:=ToSafeString(sInsuranceRef.Value), vOperatorID:=ToSafeInteger(iOperatorID), vPurchaseOrderNo:=ToSafeString(sPurchaseOrderNo.Value), vPurchaseInvoiceNo:=ToSafeString(sPurchaseInvoiceNo.Value),
                                                 vDepartment:=ToSafeString(sDepartment.Value), vSpare:=ToSafeString(sSpare.Value), vRefDate:=ToSafeDate(dtRefDate), vRefAmount:=ToSafeDecimal(cRefAmount), vRefQuantity:=vdRefQuantity,
                                                 vRefUnits:=ToSafeString(sRefUnits.Value), vDepartmentId:=ToSafeInteger(lDepartmentId), vTransdetailTypeId:=ToSafeInteger(iTransdetailTypeId))

                'vThirdCurrency:=iThirdCurrency, vThirdCurrencyAmount:=cThirdCurrencyAmount, _
                ''vThirdCurrencyBaseXrate:=vdThirdCurrencyBaseXrate, _
                '
                '    Optional vTransdetailID As Variant, _
                ''    Optional vAccountID As Variant, _
                ''    Optional vPostingstatusID As Variant, _
                ''    Optional vCompanyID As Variant, _
                ''    Optional vCurrencyID As Variant, _
                ''    Optional vPeriodID As Variant, _
                ''    Optional vDocumentID As Variant, Optional vDocumentSequence As Variant, _
                ''    Optional vAccountingDate As Variant, _
                ''    Optional vAmount As Variant, Optional vBaseAmountUnrounded As Variant, _
                ''    Optional vFullyMatched As Variant, _
                ''    Optional vCurrencyAmount As Variant, Optional vCurrencyAmountUnrounded As Variant, _
                ''    Optional vCurrencyBaseXrate As Variant, _
                ''    Optional vEuroCurrencyId As Variant, Optional vEuroAmount As Variant, _
                ''    Optional vEuroBaseXRate As Variant, Optional vEuroCcyXrate As Variant, _
                ''    Optional vComment As Variant, _
                ''    Optional vInsuranceRef As Variant, _
                ''    Optional vOperatorID As Variant, _
                ''    Optional vPurchaseOrderNo As Variant, Optional vPurchaseInvoiceNo As Variant, _
                ''    Optional vDepartment As Variant, _
                ''    Optional vSpare As Variant, _
                ''    Optional vRefDate As Variant, Optional vRefAmount As Variant, _
                ''    Optional vRefQuantity As Variant, Optional vRefUnits As Variant) As Long
                '
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                cCurBalanceCheck += cCurrencyAmount 'add up all the currency amounts
                cBalanceCheck += cAmount 'add up all the base amounts

                vTransIds(j) = lTransDetailID

                'DC030806
                If sSpare.Value.TrimEnd() = "TAX" Then

                    bTax = True

                    m_lReturn = GetDestinationAccountId(lAccountID, lDestinationAccountId)

                End If

            Next j

            'TODO LIST
            'If bSingleCurrencyDoc Then
            '	If cCurBalanceCheck <> 0 Then 'document out of balance
            '		result = gPMConstants.PMEReturnCode.PMFalse
            '		m_lReturn = RollbackTrans()
            '		Return result
            '	End If
            'Else
            '	If cBalanceCheck <> 0 Then 'document out of balance (possible roundings)
            '		result = gPMConstants.PMEReturnCode.PMFalse
            '		m_lReturn = RollbackTrans()
            '		Return result
            '	End If
            'End If




            vDocId = New Object() {m_lDocumentId, vTransIds}

            ' Update the Transaction details

            oTransDetail.m_oDatabase = m_oDatabase
            m_lReturn = oTransDetail.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            ' All OK so Commit the DB transaction
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            oTransDetail = Nothing
            oPeriod = Nothing

            'DC030806 Get Transacion Ids
            m_lReturn = GetTransIds(m_lDocumentId, vTransIds)

            vDocId = New Object() {m_lDocumentId, vTransIds}


            r_vOutputs = vDocId

            'DC040806
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnhancedAccountingBasis, v_vBranch:=1, r_vUnderwriting:=vValue)

            'DC030806 Set Suspense Transactions
            sAccountingBasis = ""

            If bTax And vValue = "1" Then

                m_lReturn = GetTaxTransId(m_lDocumentId, lTaxTransId)

                If Not Informations.IsArray(vSuspenseArray) Then
                    ReDim vSuspenseArray(8, 0)
                Else

                    ReDim Preserve vSuspenseArray(8, vSuspenseArray.GetUpperBound(1) + 1)
                End If


                vSuspenseArray(0, vSuspenseArray.GetUpperBound(1)) = lTaxTransId


                vSuspenseArray(1, vSuspenseArray.GetUpperBound(1)) = 0


                vSuspenseArray(2, vSuspenseArray.GetUpperBound(1)) = 0


                vSuspenseArray(3, vSuspenseArray.GetUpperBound(1)) = lDestinationAccountId


                vSuspenseArray(4, vSuspenseArray.GetUpperBound(1)) = 0


                vSuspenseArray(5, vSuspenseArray.GetUpperBound(1)) = iDocumenttypeID


                vSuspenseArray(6, vSuspenseArray.GetUpperBound(1)) = iTransdetailTypeId


                vSuspenseArray(7, vSuspenseArray.GetUpperBound(1)) = "TAX"


                vSuspenseArray(8, vSuspenseArray.GetUpperBound(1)) = "TAX"

                m_lReturn = GetOption(v_iOptionNumber:=4012, r_sOptionValue:=sAccountingBasisOption)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    sAccountingBasisOption = ""
                End If

            End If


            r_vSuspenseArray = vSuspenseArray

            r_sAccountingBasis = sAccountingBasisOption

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCompleteDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCompleteDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateNumber
    '
    ' Description: Wrapper call to the bPMAutoNumber function
    'EK 220200
    ' ***************************************************************** '
    'eck170500 Add company ID
    Public Function GenerateNumber(ByRef v_sGroupCode As String, ByRef v_sRangeCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the autonumber object

            '    m_lReturn& = gPMComponentServices.CreateBusinessObject( _
            ''        r_oObject:=oPMAutoNumber, _
            ''        v_sClassName:="bACTAutoNumber.business", _
            ''        v_sCallingAppName:=ACApp, _
            ''        v_sUserName:=m_sUsername$, _
            ''        v_sPassword:=m_sPassword$, _
            ''        v_iUserID:=m_iUserID%, _
            ''        v_iSourceID:=m_iSourceID%, _
            ''        v_iLanguageId:=m_iLanguageID%, _
            ''        v_iCurrencyID:=m_iCurrencyID%, _
            ''        v_iLogLevel:=m_iLogLevel%)

            ' Steve Watton - 06/02/2003

            oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' Get the number range
            'EK 220200 Use passed range and group

            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=v_sGroupCode, v_sRangeCode:=v_sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Generate the next number
                'eck170500

                m_lReturn = oPMAutoNumber.GenerateNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_lNumber:=r_lNumber)
            End If

            ' terminate the object

            oPMAutoNumber.Dispose()
            oPMAutoNumber = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateNumberFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PoolNumber
    '
    ' Description: Wrapper call to the bPMAutoNumber pool function
    ' ***************************************************************** '
    'eck050600
    Public Function PoolNumber(ByRef v_sRangeCode As String, ByRef v_sGroupCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the autonumber object

            '    m_lReturn& = gPMComponentServices.CreateBusinessObject( _
            ''        r_oObject:=oPMAutoNumber, _
            ''        v_sClassName:="bACTAutoNumber.business", _
            ''        v_sCallingAppName:=ACApp, _
            ''        v_sUserName:=m_sUsername$, _
            ''        v_sPassword:=m_sPassword$, _
            ''        v_iUserID:=m_iUserID%, _
            ''        v_iSourceID:=m_iSourceID%, _
            ''        v_iLanguageId:=m_iLanguageID%, _
            ''        v_iCurrencyID:=m_iCurrencyID%, _
            ''        v_iLogLevel:=m_iLogLevel%)

            ' Steve Watton - 06/02/2003

            oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' Get the number range
            'EK 220200 Use passed range and group

            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=v_sGroupCode, v_sRangeCode:=v_sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Generate the next number

                m_lReturn = oPMAutoNumber.PoolNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_lNumber:=r_lNumber)
            End If

            ' terminate the object

            oPMAutoNumber.Dispose()
            oPMAutoNumber = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PoolNumberFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PoolNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************** '
    ' Name: GetGroupIDFromTypeID
    '
    ' Description: Given a documenttype_id this function will hit the
    '              database, and return the corresponding doctypegroup_id
    '
    ' ********************************************************************** '
    Public Function GetGroupIDFromTypeID(ByVal v_lDocumentTypeID As Integer, ByRef r_lDocTypeGroupID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the SQL statement
            sSQL = "SELECT doctypegroup_id " & _
                   "FROM DocumentType " & _
                   "WHERE documenttype_id = " & CStr(v_lDocumentTypeID)

            ' Perform the select
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetGroupIDFromTypeID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the returned value
            If Informations.IsArray(vResultArray) Then

                r_lDocTypeGroupID = CInt(vResultArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGroupIDFromTypeIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupIDFromTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oDocument As bACTDocument.Document) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add DocumentID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_id", vValue:=CStr(oDocument.DocumentID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oDocument:=oDocument)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oDocument.DocumentID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("Document_id").Value)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oDocument As bACTDocument.Document) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add DocumentID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_id", vValue:=CStr(oDocument.DocumentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oDocument:=oDocument)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oDocument.Timestamp, _
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
    Private Function DeleteItem(ByRef oDocument As bACTDocument.Document) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the DocumentID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_id", vValue:=CStr(oDocument.DocumentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied Document properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oDocument As bACTDocument.Document, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No.21
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oDocument

            .DocumentID = gPMFunctions.NullToLong(oFields("document_id"))
            .CompanyID = gPMFunctions.NullToLong(oFields("company_id"))
            .PostingstatusID = gPMFunctions.NullToLong(oFields("postingstatus_id"))
            .DocumenttypeID = gPMFunctions.NullToLong(oFields("documenttype_id"))
            .AuditsetID = gPMFunctions.NullToLong(oFields("auditset_id"))
            .BatchID = gPMFunctions.NullToLong(oFields("batch_id"))
            .DocumentRef = gPMFunctions.NullToString(oFields("document_ref"))
            .DocumentDate = gPMFunctions.NullToDate(oFields("document_date"))
            .CreatedDate = gPMFunctions.NullToDate(oFields("created_date"))
            .AuthorisedDate = gPMFunctions.NullToDate(oFields("authorised_date"))
            .Comment = gPMFunctions.NullToString(oFields("comment"))
            .WriteOffReasonID = gPMFunctions.NullToLong(oFields("write_off_reason_id"))
            .SubBranchID = gPMFunctions.NullToLong(oFields("sub_branch_id"))
            .InsuranceFileCnt = gPMFunctions.NullToLong(oFields("insurance_file_cnt"))
            .Reason = gPMFunctions.NullToString(oFields("reason"))
            .ClaimId = gPMFunctions.NullToLong(oFields("claim_id"))
            .TermsOfPaymentId = gPMFunctions.NullToLong(oFields("terms_of_payment_id"))
            .PaymentDueDate = gPMFunctions.NullToDate(oFields("payment_due_date"))
            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Document property values.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Private Function SetProperties(ByRef oDocument As bACTDocument.Document, ByRef iStatus As Integer, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID, vInsuranceFileCnt:=vInsuranceFileCnt, vReason:=vReason, vSubBranchID:=vSubBranchID, vClaimID:=vClaimID, v_vTermsOfPaymentId:=v_vTermsOfPaymentId, v_vPaymentDueDate:=v_vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vDocumentID:=vDocumentID, vCompanyID:=vCompanyID, vPostingstatusID:=vPostingstatusID, vDocumenttypeID:=vDocumenttypeID, vAuditsetID:=vAuditsetID, vBatchID:=vBatchID, vDocumentRef:=vDocumentRef, vDocumentDate:=vDocumentDate, vCreatedDate:=vCreatedDate, vAuthorisedDate:=vAuthorisedDate, vComment:=vComment, vWriteOffReasonID:=vWriteOffReasonID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oDocument


            If Not Informations.IsNothing(vDocumentID) Then
                If .DocumentID <> vDocumentID Then
                    .DocumentID = vDocumentID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPostingstatusID) Then
                If .PostingstatusID <> vPostingstatusID Then
                    .PostingstatusID = vPostingstatusID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDocumenttypeID) Then
                If .DocumenttypeID <> vDocumenttypeID Then
                    .DocumenttypeID = vDocumenttypeID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAuditsetID) Then
                If .AuditsetID <> vAuditsetID Then
                    .AuditsetID = vAuditsetID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBatchID) Then
                If .BatchID <> vBatchID Then
                    .BatchID = vBatchID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDocumentRef) Then
                If .DocumentRef.Trim() <> vDocumentRef.Trim() Then
                    .DocumentRef = vDocumentRef
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDocumentDate) Then
                If .DocumentDate <> vDocumentDate Then
                    .DocumentDate = vDocumentDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCreatedDate) Then
                If .CreatedDate <> vCreatedDate Then
                    .CreatedDate = vCreatedDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAuthorisedDate) Then
                If .AuthorisedDate <> vAuthorisedDate Then
                    .AuthorisedDate = vAuthorisedDate
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vComment) Then
                If .Comment.Trim() <> vComment.Trim() Then
                    .Comment = vComment
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vWriteOffReasonID) Then
                If CStr(.WriteOffReasonID).Trim() <> CStr(vWriteOffReasonID).Trim() Then
                    .WriteOffReasonID = vWriteOffReasonID
                    bDataChanged = True
                End If
            End If

            'sj 01/08/2002 - start

            If Not Informations.IsNothing(vInsuranceFileCnt) Then
                If CStr(.InsuranceFileCnt).Trim() <> CStr(vInsuranceFileCnt).Trim() Then
                    .InsuranceFileCnt = vInsuranceFileCnt
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vReason) Then
                If .Reason.Trim() <> vReason.Trim() Then
                    .Reason = vReason
                    bDataChanged = True
                End If
            End If
            'sj 01/08/2002 - end

            'pwf 29/08/2002 - sub branch


            If Not (Informations.IsNothing(vSubBranchID) Or Convert.IsDBNull(vSubBranchID) Or Informations.IsNothing(vSubBranchID)) Then
                If CStr(.SubBranchID).Trim() <> CStr(vSubBranchID).Trim() Then
                    .SubBranchID = vSubBranchID
                    bDataChanged = True
                End If
            End If



            If Not (Informations.IsNothing(vClaimID) Or Convert.IsDBNull(vClaimID) Or Informations.IsNothing(vClaimID)) Then
                If .ClaimId <> vClaimID Then
                    .ClaimId = vClaimID
                    bDataChanged = True
                End If
            End If
            'S4BDAT004


            If Not (Informations.IsNothing(v_vTermsOfPaymentId) Or Convert.IsDBNull(v_vTermsOfPaymentId) Or Informations.IsNothing(v_vTermsOfPaymentId)) Then
                If .TermsOfPaymentId <> v_vTermsOfPaymentId Then
                    .TermsOfPaymentId = v_vTermsOfPaymentId
                    bDataChanged = True
                End If
            End If


            If Not (Informations.IsNothing(v_vPaymentDueDate) Or Convert.IsDBNull(v_vPaymentDueDate) Or Informations.IsNothing(v_vPaymentDueDate)) Then
                If .PaymentDueDate <> v_vPaymentDueDate Then
                    .PaymentDueDate = v_vPaymentDueDate
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
    ' Description: Returns the supplied Document property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oDocument As bACTDocument.Document, ByRef iStatus As Integer, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oDocument
            'Developer Guide No143
            'Starts

            ''S4BDAT004
            vDocumentID = .DocumentID

            vCompanyID = .CompanyID

            vPostingstatusID = .PostingstatusID

            vDocumenttypeID = .DocumenttypeID

            vAuditsetID = .AuditsetID

            vBatchID = .BatchID

            vDocumentRef = .DocumentRef

            vDocumentDate = .DocumentDate

            vCreatedDate = .CreatedDate

            vAuthorisedDate = .AuthorisedDate

            vComment = .Comment

            vWriteOffReasonID = .WriteOffReasonID

            vSubBranchID = .SubBranchID

            vInsuranceFileCnt = .InsuranceFileCnt

            'developer guide no. 142
            ' vReason = CInt(.Reason)
            vReason = .Reason

            vReason = .ClaimId

            'S4BDAT004

            v_vTermsOfPaymentId = .TermsOfPaymentId

            v_vPaymentDueDate = .PaymentDueDate

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
    Private Function AddInputParam(ByRef oDocument As bACTDocument.Document) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If oDocument.CompanyID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oDocument.CompanyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oDocument.PostingstatusID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="postingstatus_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="postingstatus_id", vValue:=CStr(oDocument.PostingstatusID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oDocument.DocumenttypeID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="documenttype_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="documenttype_id", vValue:=CStr(oDocument.DocumenttypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oDocument.AuditsetID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="auditset_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="auditset_id", vValue:=CStr(oDocument.AuditsetID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oDocument.BatchID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="batch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="batch_id", vValue:=CStr(oDocument.BatchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_ref", vValue:=oDocument.DocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="document_date", vValue:=oDocument.DocumentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="created_date", vValue:=oDocument.CreatedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="authorised_date", vValue:=oDocument.AuthorisedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="comment", vValue:=oDocument.Comment, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Write Off Reason
            'sj 01/08/2002 - change type to PMLong
            If oDocument.WriteOffReasonID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="write_off_reason_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Else
                m_lReturn = .Parameters.Add(sName:="write_off_reason_id", vValue:=CStr(oDocument.WriteOffReasonID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 01/08/2002 - start
            ' InsuranceFileCnt
            If oDocument.InsuranceFileCnt < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(oDocument.InsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Reason
            m_lReturn = .Parameters.Add(sName:="reason", vValue:=oDocument.Reason, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 01/08/2002 - end

            ' pwf 29/08/2002 - sub_branch_id
            If oDocument.SubBranchID < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="sub_branch_id", vValue:=CStr(oDocument.SubBranchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            'S4B Claim Enhancements R&D 2005
            If oDocument.ClaimId < 1 Then

                'Developer Guide No 85
                m_lReturn = .Parameters.Add(sName:="claim_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="claim_id", vValue:=CStr(oDocument.ClaimId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'S4BDAT004
            m_lReturn = .Parameters.Add(sName:="terms_of_payment_id", vValue:=CStr(oDocument.TermsOfPaymentId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide no 113
            Dim dPaymentDueDate As Object
            If (oDocument.PaymentDueDate.Equals(DateTime.MinValue) OrElse oDocument.PaymentDueDate = #12/30/1899# OrElse oDocument.PaymentDueDate = #12/29/1899#) Then
                dPaymentDueDate = DBNull.Value
            Else
                dPaymentDueDate = oDocument.PaymentDueDate
            End If
            m_lReturn = .Parameters.Add(sName:="payment_due_date", vValue:=dPaymentDueDate, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'S4BDAT004 END
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Document.
    '
    ' ***************************************************************** '
    'sj 01/08/2002 - Add InsuranceFileCnt and Reason as parameters
    'Developer Guide No 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vReason As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef v_vTermsOfPaymentId As Object = Nothing, Optional ByRef v_vPaymentDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'Developer Guide No 151

        If (Informations.IsNothing(vDocumentID)) OrElse (vDocumentID.Equals(0)) OrElse (bDefaultAll) Then
            vDocumentID = 0
        End If

        If (Informations.IsNothing(vCompanyID)) OrElse (vCompanyID.Equals(0)) OrElse (bDefaultAll) Then
            vCompanyID = 0
        End If

        If (Informations.IsNothing(vPostingstatusID)) OrElse (vPostingstatusID.Equals(0)) OrElse (bDefaultAll) Then
            vPostingstatusID = 0
        End If

        If (Informations.IsNothing(vDocumenttypeID)) OrElse (vDocumenttypeID.Equals(0)) OrElse (bDefaultAll) Then
            vDocumenttypeID = 0
        End If

        If (Informations.IsNothing(vAuditsetID)) OrElse (vAuditsetID.Equals(0)) OrElse (bDefaultAll) Then
            vAuditsetID = 0
        End If

        If (Informations.IsNothing(vBatchID)) OrElse (vBatchID.Equals(0)) OrElse (bDefaultAll) Then
            vBatchID = 0
        End If
        'Developer Guide no. 160
        If (Informations.IsNothing(vDocumentRef)) OrElse (bDefaultAll) Then
            vDocumentRef = ""
        End If

        If (Informations.IsNothing(vDocumentDate)) OrElse (vDocumentDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vDocumentDate = DateTime.Today
        End If

        If (Informations.IsNothing(vCreatedDate)) OrElse (vCreatedDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vCreatedDate = DateTime.Today
        End If

        If (Informations.IsNothing(vComment)) OrElse (bDefaultAll) Then
            vComment = ""
        End If

        If (Informations.IsNothing(vWriteOffReasonID)) OrElse (vWriteOffReasonID.Equals(0)) OrElse (bDefaultAll) Then
            vWriteOffReasonID = 0
        End If

        'sj 01/08/2002 - start


        If (Informations.IsNothing(vInsuranceFileCnt)) OrElse (vInsuranceFileCnt.Equals(0)) OrElse (bDefaultAll) Then
            vInsuranceFileCnt = 0
        End If

        If (Informations.IsNothing(vReason)) OrElse (bDefaultAll) Then
            vReason = ""
        End If
        'sj 01/08/2002 - end

        'pwf sub branch default


        If (Informations.IsNothing(vSubBranchID)) OrElse (vSubBranchID.Equals(0)) OrElse (bDefaultAll) Then
            vSubBranchID = 0
        End If

        'S4B Claim Enhancements R&D 2005


        If (Informations.IsNothing(vClaimID)) OrElse (vClaimID.Equals(0)) OrElse (bDefaultAll) Then
            vClaimID = 0
        End If

        If (Informations.IsNothing(v_vTermsOfPaymentId)) OrElse (v_vTermsOfPaymentId.Equals(0)) OrElse (bDefaultAll) Then
            v_vTermsOfPaymentId = 0
        End If

        If (Informations.IsNothing(v_vPaymentDueDate)) OrElse (Object.Equals(v_vPaymentDueDate, Nothing)) OrElse (bDefaultAll) Then
            v_vPaymentDueDate = DBNull.Value
        End If
        'Ends

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Document.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        'Developer Guide No 151
        If (Informations.IsNothing(vDocumentID)) OrElse (Object.Equals(vDocumentID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vCompanyID)) OrElse (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vPostingstatusID)) OrElse (Object.Equals(vPostingstatusID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vDocumenttypeID)) OrElse (Object.Equals(vDocumenttypeID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vDocumentRef)) OrElse (Object.Equals(vDocumentRef, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vDocumentDate)) OrElse (Object.Equals(vDocumentDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vCreatedDate)) OrElse (Object.Equals(vCreatedDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vComment)) OrElse (Object.Equals(vComment, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If
        'Ends


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Document for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAuditsetID As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vDocumentDate As Object = Nothing, Optional ByRef vCreatedDate As Object = Nothing, Optional ByRef vAuthorisedDate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vDocumentID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vDocumentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCompanyID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPostingstatusID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vPostingstatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDocumenttypeID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vDocumenttypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAuditsetID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vAuditsetID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vBatchID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vBatchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDocumentDate) Then
            If Not Informations.IsDate(vDocumentDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCreatedDate) Then
            If Not Informations.IsDate(vCreatedDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAuthorisedDate) Then
            If Not Informations.IsDate(vAuthorisedDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vWriteOffReasonID) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vWriteOffReasonID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
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


    Public Function GetAutoNumValues(ByVal v_iDocumentTypeID As Integer, ByRef r_sGroupCode As String, ByRef r_sRangeCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAutoNumValues"

        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Group Code

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_group_id", vValue:=CStr(v_iDocumentTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:='actnumber_group_id'", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACACTNumberGroupSelSQL, sSQLName:=ACACTNumberGroupSelName, bStoredProcedure:=ACACTNumberGroupSelStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=" & ACACTNumberGroupSelName, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "No results returned", gPMConstants.PMELogLevel.PMLogError)
            Else

                r_sGroupCode = CStr(vResultArray(1, 0)).Trim()
            End If

            'Get Range Code

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_range_id", vValue:=CStr(v_iDocumentTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:='actnumber_range_id'", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACACTNumberRangeSelSQL, sSQLName:=ACACTNumberRangeSelName, bStoredProcedure:=ACACTNumberRangeSelStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=" & ACACTNumberRangeSelName, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "No results returned", gPMConstants.PMELogLevel.PMLogError)
            Else

                r_sRangeCode = CStr(vResultArray(2, 0)).Trim()
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume


            '        Return result
        End Try
        Return result
    End Function

    Private Function GetTransIds(ByVal v_lDocumentId As Integer, ByRef r_vTransIds As Object) As Integer
        Dim Catch_Renamed As Boolean = False

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function GetDestinationAccountId
        ' PURPOSE: Get Destination Account Id for suspense transaction
        ' AUTHOR: David Cleaver
        ' DATE: AUG 2006
        ' REMARKS: Datasure
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACGetTransIdsStored As Boolean = False
        Const ACGetTransIdsName As String = "GetTransIds"
        Dim sGetTransIdsSQL As String = ""
        Dim vResults(,) As Object = Nothing
        Dim bError As Boolean

        Try
            Catch_Renamed = True
            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                sGetTransIdsSQL = "SELECT transdetail_id " &
                                  "FROM transdetail " &
                                  "WHERE document_id  = {v_lDocumentId}"

                m_lReturn = .Parameters.Add(sName:="v_lDocumentId", vValue:=CStr(v_lDocumentId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=sGetTransIdsSQL, sSQLName:=ACGetTransIdsName, bStoredProcedure:=ACGetTransIdsStored, vResultArray:=vResults)

            End With

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then



                r_vTransIds = vResults

            Else
                bError = True
            End If

            If bError Then
                Throw New System.Exception(1.ToString() + ", " + +", bACTDocument.Form GetTransIds Failed to get transdetail ids")
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransIds", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select
            End If
Finally_Renamed:
        End Try
        Return result
    End Function

    Private Function GetDestinationAccountId(ByVal v_lAccountID As Integer, ByRef r_lDestinationAccountId As Integer) As Integer
        Dim Catch_Renamed As Boolean = False
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function GetDestinationAccountId
        ' PURPOSE: Get Destination Account Id for suspense transaction
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2 + tidy up (PN22188)
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACGetDestinationAccountIdStored As Boolean = False
        Const ACGetDestinationAccountIdName As String = "GetDestinationAccountId"
        Dim sGetDestinationAccountIdSQL As String = ""
        Dim vResults(,) As Object = Nothing
        Dim bError As Boolean

        Try
            Catch_Renamed = True
            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                sGetDestinationAccountIdSQL = "SELECT ea.account_map_id " &
                                              "FROM elementextras ea " &
                                              "JOIN structuretree s on s.element_id = ea.element_id " &
                                              "WHERE s.account_id  = {v_lAccountID}"

                m_lReturn = .Parameters.Add(sName:="v_lAccountId", vValue:=CStr(v_lAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=sGetDestinationAccountIdSQL, sSQLName:=ACGetDestinationAccountIdName, bStoredProcedure:=ACGetDestinationAccountIdStored, vResultArray:=vResults)

            End With

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If Informations.IsArray(vResults) Then

                    If Not (CStr(vResults(0, 0)).Trim() = "") And Not (CStr(vResults(0, 0)).Trim() = "0") Then

                        r_lDestinationAccountId = CInt(vResults(0, 0))
                    Else
                        bError = True
                    End If
                Else
                    bError = True
                End If
            Else
                bError = True
            End If

            If bError Then
                Throw New System.Exception(1.ToString() + ", " + +", " + "bACTImportSiriusTrans.Form GetDestinationAccountId Failed to get destinationAccountID for " & "Tax Account. Please ensure that the account is mapped to an income account in Account " & "Explorer/Balance Sheet/Liabilities/Current Liabilities/Commission. Right click and select Extras.")
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDestinationAccountId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select
            End If
Finally_Renamed:
        End Try
        Return result
    End Function

    Private Function GetTaxTransId(ByVal v_lDocumentId As Integer, ByRef r_lTaxTransId As Integer) As Integer
        Dim Catch_Renamed As Boolean = False

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function GetTaxTransId
        ' PURPOSE: Get Tax Transdetail Id for suspense transaction
        ' AUTHOR: David Cleaver
        ' DATE: AUG 2006
        ' REMARKS: Datasure
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACGetTaxTransIdStored As Boolean = False
        Const ACGetTaxTransIdName As String = "GetTransId"
        Dim sGetTaxTransIdSQL As String = ""
        Dim vResults(,) As Object = Nothing
        Dim bError As Boolean

        Try
            Catch_Renamed = True
            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                sGetTaxTransIdSQL = "SELECT transdetail_id " &
                                    "FROM transdetail " &
                                    "WHERE document_id  = {v_lDocumentId} " &
                                    "AND spare = 'TAX'"

                m_lReturn = .Parameters.Add(sName:="v_lDocumentId", vValue:=CStr(v_lDocumentId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=sGetTaxTransIdSQL, sSQLName:=ACGetTaxTransIdName, bStoredProcedure:=ACGetTaxTransIdStored, vResultArray:=vResults)

            End With

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                r_lTaxTransId = CInt(vResults(0, 0))

            Else
                bError = True
            End If

            If bError Then
                Throw New System.Exception(1.ToString() + ", " + +", bACTDocument.Form GetTaxTransId Failed to get transdetail ids")
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            'Developer Guide No 32

            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case Else
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxTransId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select
            End If
Finally_Renamed:
        End Try
        Return result
    End Function

    'DC040806
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer
        Return GetOption(v_iOptionNumber:=v_iOptionNumber, r_sOptionValue:=r_sOptionValue, vDatabase:=Nothing)
    End Function

    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByRef vDatabase As Object) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then
                ' Get Reference to Database
                'SD 24/07/2002 rename variable
                ' PWF 16/10/2002 - Must not use module level CloseDatabase flag!!!

                m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=m_oS4BDatabase, v_vDatabase:=vDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="gPMComponentServices.CheckDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Get Instance of System Option Business
                'SD 24/07/2002 rename variable

                m_oSystemOption = New bSIROptions.Business
                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oS4BDatabase)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="gPMComponentServices.CreateBusinessObject Failed for bSIROptions.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If



            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the option number " & v_iOptionNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption")

                Return result
            End If

            m_oSystemOption.Dispose()



            m_oSystemOption = Nothing
            m_lReturn = m_oS4BDatabase.CloseDatabase()

            m_oS4BDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    ' ***************************************************************** '
    ' Name: GenerateDocumentReference
    '
    ' Description: Wrapper call to the bACTAutoNumber.GenerateDocumentReferenceNumber
    '               function. This replaces GenerateNumber method
    '
    ' ***************************************************************** '

    Public Function GenerateDocumentReferenceNumber(ByRef v_sGroupCode As String, ByRef v_sRangeCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateDocumentReferenceNumber"

        Dim oACTAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            oACTAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oACTAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get the number range
            'EK 220200 Use passed range and group

            m_lReturn = oACTAutoNumber.GetNumberRange(v_sGroupCode:=v_sGroupCode, v_sRangeCode:=v_sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute bACTAutoNumber.GetNumberRange", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Generate the next docuemnt reference

            m_lReturn = oACTAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=v_sRangeCode, v_lNumberRangeID:=lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_sDocumentRef:=r_sDocumentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute bACTAutoNumber.GenerateDocumentReference", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' terminate the object

            oACTAutoNumber.Dispose()
            oACTAutoNumber = Nothing


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
    'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

