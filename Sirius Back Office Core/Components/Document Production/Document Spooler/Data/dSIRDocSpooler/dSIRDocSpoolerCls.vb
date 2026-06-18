Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129 (guide)
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("SIRDocSpooler_NET.SIRDocSpooler")>
Public NotInheritable Class SIRDocSpooler
    Implements IDisposable
    '*******************************************************************************
    ' Class Name: SIREvent
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIREvent attributes.
    '
    ' Edit History:
    '
    '   PW140105 - PN18078 - changes required for new Document Template ID field
    '              being added to the document_spooler table.
    '*******************************************************************************


    ' ************************************************
    ' Added to replace global variables 12/01/2004
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
    Private Const ACClass As String = "SIRDocSpooler"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lDocumentSpoolerId As Integer
    Private m_vDocumentTypeId As Object
    Private m_vPartyCnt As Object
    Private m_vInsuranceFolderCnt As Object
    Private m_vInsuranceFileCnt As Object
    Private m_vClaimCnt As Object
    Private m_sDescription As String = ""
    Private m_iIsDeletable As Integer
    Private m_iIsEditable As Integer
    Private m_iCreatedById As Integer
    Private m_dtDateCreated As Date
    Private m_iModifiedById As Integer
    Private m_dtLastModified As Date
    Private m_iTimesPrinted As Integer
    Private m_iTimesArchived As Integer
    Private m_lDocumentTemplateID As Integer ' PN18078

    'CJR 28/11/2002: Required for IAG - Proof of concept
    Private m_bBulkPrint As Boolean
    Private m_lDocumentStatusId As Integer
    Private m_dtStatusUpdated As Date

    ' PW230403 - new fields required for Document Issuance changes
    Private m_vUnzipped As Object
    Private m_vBatchRef As Object
    Private m_vTemplateCode As Object
    'DC240603 -ISS4097 -added new parameter
    Private m_lSourceId As Integer

    'TN2001030
    Private m_vPrinter As Object

    'sj 15/10/2002 - start
    Private m_vSpoolLevelInd As Object
    'sj 15/10/2002 - end

    Private m_iIsClient As Integer
    Private m_iIsAgent As Integer
    Private m_iIsOffice As Integer
    Private m_iProductionOrder As Integer

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)



    Public Property BulkPrint() As Boolean
        Get
            Return m_bBulkPrint
        End Get
        Set(ByVal Value As Boolean)
            m_bBulkPrint = Value
        End Set
    End Property


    Public Property DocumentStatusId() As Integer
        Get
            Return m_lDocumentStatusId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentStatusId = Value
        End Set
    End Property

    Public Property StatusUpdated() As Date
        Get
            Return m_dtStatusUpdated
        End Get
        Set(ByVal Value As Date)
            m_dtStatusUpdated = Value
        End Set
    End Property


    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    Public Property DocumentSpoolerId() As Integer
        Get
            Return m_lDocumentSpoolerId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentSpoolerId = Value
        End Set
    End Property

    Public Property DocumentTypeId() As Object
        Get
            Return m_vDocumentTypeId
        End Get
        Set(ByVal Value As Object)


            m_vDocumentTypeId = Value
        End Set
    End Property

    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)


            m_vPartyCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Object
        Get
            Return m_vInsuranceFolderCnt
        End Get
        Set(ByVal Value As Object)


            m_vInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Object
        Get
            Return m_vInsuranceFileCnt
        End Get
        Set(ByVal Value As Object)


            m_vInsuranceFileCnt = Value
        End Set
    End Property

    Public Property ClaimCnt() As Object
        Get
            Return m_vClaimCnt
        End Get
        Set(ByVal Value As Object)


            m_vClaimCnt = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property IsDeletable() As Integer
        Get
            Return m_iIsDeletable
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeletable = Value
        End Set
    End Property

    Public Property IsEditable() As Integer
        Get
            Return m_iIsEditable
        End Get
        Set(ByVal Value As Integer)
            m_iIsEditable = Value
        End Set
    End Property

    Public Property CreatedById() As Integer
        Get
            Return m_iCreatedById
        End Get
        Set(ByVal Value As Integer)
            m_iCreatedById = Value
        End Set
    End Property

    Public Property DateCreated() As Date
        Get
            Return m_dtDateCreated
        End Get
        Set(ByVal Value As Date)
            m_dtDateCreated = Value
        End Set
    End Property

    Public Property ModifiedById() As Integer
        Get
            Return m_iModifiedById
        End Get
        Set(ByVal Value As Integer)
            m_iModifiedById = Value
        End Set
    End Property

    Public Property LastModified() As Date
        Get
            Return m_dtLastModified
        End Get
        Set(ByVal Value As Date)
            m_dtLastModified = Value
        End Set
    End Property

    Public Property TimesPrinted() As Integer
        Get
            Return m_iTimesPrinted
        End Get
        Set(ByVal Value As Integer)
            m_iTimesPrinted = Value
        End Set
    End Property

    Public Property TimesArchived() As Integer
        Get
            Return m_iTimesArchived
        End Get
        Set(ByVal Value As Integer)
            m_iTimesArchived = Value
        End Set
    End Property

    'TN20010730 - start
    Public Property Printer() As Object
        Get
            Return m_vPrinter
        End Get
        Set(ByVal Value As Object)


            m_vPrinter = Value
        End Set
    End Property
    'TN20010730 - end
    'sj 15/10/2002 - start
    Public Property SpoolLevelInd() As Object
        Get
            Return m_vSpoolLevelInd
        End Get
        Set(ByVal Value As Object)


            m_vSpoolLevelInd = Value
        End Set
    End Property
    'sj 15/10/2002 - end


    Public Property IsClient() As Integer
        Get
            Return m_iIsClient
        End Get
        Set(ByVal Value As Integer)
            m_iIsClient = Value
        End Set
    End Property

    Public Property IsAgent() As Integer
        Get
            Return m_iIsAgent
        End Get
        Set(ByVal Value As Integer)
            m_iIsAgent = Value
        End Set
    End Property

    Public Property IsOffice() As Integer
        Get
            Return m_iIsOffice
        End Get
        Set(ByVal Value As Integer)
            m_iIsOffice = Value
        End Set
    End Property

    Public Property ProductionOrder() As Integer
        Get
            Return m_iProductionOrder
        End Get
        Set(ByVal Value As Integer)
            m_iProductionOrder = Value
        End Set
    End Property



    Public Property Unzipped() As Object
        Get

            Return m_vUnzipped

        End Get
        Set(ByVal Value As Object)



            m_vUnzipped = Value

        End Set
    End Property


    Public Property BatchRef() As Object
        Get

            Return m_vBatchRef

        End Get
        Set(ByVal Value As Object)



            m_vBatchRef = Value

        End Set
    End Property


    Public Property TemplateCode() As Object
        Get

            Return m_vTemplateCode

        End Get
        Set(ByVal Value As Object)



            m_vTemplateCode = Value

        End Set
    End Property

    'DC240603 -ISS4097 -new parameter

    Public Property SourceId() As Integer
        Get

            Return m_lSourceId

        End Get
        Set(ByVal Value As Integer)

            m_lSourceId = Value

        End Set
    End Property
    'DC240603 -end

    ' PN18078

    ' PN18078
    Public Property DocumentTemplateID() As Integer
        Get

            Return m_lDocumentTemplateID

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentTemplateID = Value

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer



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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIREvent
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = 0
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(1).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIREvent properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guide No 22
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields
                'developer guide no. 145 (guide)
                DocumentSpoolerId = oFields("document_spooler_id").Value

                If Convert.IsDBNull(oFields("document_type_id")) Or Informations.IsNothing(oFields("document_type_id")) Then

                    DocumentTypeId = Nothing
                Else

                    DocumentTypeId = oFields("document_type_id")
                End If

                If Convert.IsDBNull(oFields("party_cnt")) Or Informations.IsNothing(oFields("party_cnt")) Then

                    PartyCnt = Nothing
                Else

                    PartyCnt = oFields("party_cnt")
                End If

                If Convert.IsDBNull(oFields("insurance_folder_cnt")) Or Informations.IsNothing(oFields("insurance_folder_cnt")) Then

                    InsuranceFolderCnt = Nothing
                Else

                    InsuranceFolderCnt = oFields("insurance_folder_cnt")
                End If

                If Convert.IsDBNull(oFields("insurance_file_cnt")) Or Informations.IsNothing(oFields("insurance_file_cnt")) Then

                    InsuranceFileCnt = Nothing
                Else

                    InsuranceFileCnt = oFields("insurance_file_cnt")
                End If

                If Convert.IsDBNull(oFields("claim_cnt")) Or Informations.IsNothing(oFields("claim_cnt")) Then

                    ClaimCnt = Nothing
                Else

                    ClaimCnt = oFields("claim_cnt")
                End If

                If Convert.IsDBNull(oFields("description")) Or Informations.IsNothing(oFields("description")) Then

                    Description = Nothing
                Else
                    Description = oFields("description")
                End If
                'developer guide no. 145 (guide)
                IsDeletable = oFields("is_deletable").Value
                'developer guide no. 145 (guide)
                IsEditable = oFields("is_editable").Value

                If Convert.IsDBNull(oFields("created_by_id")) Or Informations.IsNothing(oFields("created_by_id")) Then

                    CreatedById = Nothing
                Else
                    'developer guide no. 145 (guide)
                    CreatedById = oFields("created_by_id").Value
                End If

                If Convert.IsDBNull(oFields("date_created")) Or Informations.IsNothing(oFields("date_created")) Then

                    DateCreated = Nothing
                Else
                    'developer guide no. 145 (guide)
                    DateCreated = oFields("date_created").Value
                End If

                If Convert.IsDBNull(oFields("modified_by_id")) Or Informations.IsNothing(oFields("modified_by_id")) Then

                    ModifiedById = Nothing
                Else
                    'developer guide no. 145 (guide)
                    ModifiedById = oFields("modified_by_id").Value
                End If

                If Convert.IsDBNull(oFields("last_modified")) Or Informations.IsNothing(oFields("last_modified")) Then

                    LastModified = Nothing
                Else
                    'developer guide no. 145 (guide)
                    LastModified = oFields("last_modified").Value
                End If
                'developer guide no. 145 (guide)
                TimesPrinted = oFields("times_printed").Value
                'developer guide no. 145 (guide)
                TimesArchived = oFields("times_archived").Value

                'CJR 28/11/2002: Required for IAG - Proof of concept
                'developer guide no. 145 (guide)
                BulkPrint = oFields("bulk_print").Value
                DocumentStatusId = oFields("document_status_id").Value
                StatusUpdated = oFields("status_updated").Value

                ' PW240304 - add new fields required for Document Issuance changes

                Unzipped = oFields("unzipped")

                BatchRef = oFields("batch_ref")

                TemplateCode = oFields("template_code")

                'developer guide no. 145 (guide)
                DocumentTemplateID = oFields("document_template_id").Value ' PN18078

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Note that exiting from a with is very bad, as there's a memory leak
        '    With m_oDatabase

        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_type_id", vValue:=DocumentTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=PartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=InsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=InsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_cnt", vValue:=ClaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deletable", vValue:=CStr(IsDeletable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_editable", vValue:=CStr(IsEditable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="created_by_id", vValue:=CStr(CreatedById), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'developer guide no. 113
        If DateCreated = #12:00:00 AM# OrElse Informations.IsNothing(DateCreated) Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_created", vValue:=#12/29/1899#, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_created", vValue:=DateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_by_id", vValue:=CStr(ModifiedById), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'developer guide no. 113
        If LastModified = #12:00:00 AM# OrElse Informations.IsNothing(LastModified) Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_modified", vValue:=#12/29/1899#, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_modified", vValue:=LastModified, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="times_printed", vValue:=CStr(TimesPrinted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="times_archived", vValue:=CStr(TimesArchived), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'TN20010730 - start
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Printer", vValue:=Printer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'TN20010730 - end
        'sj 15/10/2002 - start
        m_lReturn = m_oDatabase.Parameters.Add(sName:="spool_level_ind", vValue:=SpoolLevelInd, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'sj 15/10/2002 - end

        'DC240603 -ISS4097 -new parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(SourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'DC240603 -end

        ' Add Document Template ID param. PN18078.
        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(m_lDocumentTemplateID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_client", vValue:=CStr(IsClient), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_agent", vValue:=CStr(IsAgent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_Office", vValue:=CStr(IsOffice), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="production_order", vValue:=CStr(ProductionOrder), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="document_spooler_id", vValue:=CStr(DocumentSpoolerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="document_spooler_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            DocumentSpoolerId = .Parameters.Item("document_spooler_id").Value

        End With

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

