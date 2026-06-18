Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports dPMDAO

'developer guide no. 129 (guide)
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("SIRDocTemplate_NET.SIRDocTemplate")>
Public NotInheritable Class SIRDocTemplate
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIREvent
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIREvent attributes.
    '
    ' Edit History:
    ' RKS 02/05/2005 Added DocumentFilter property
    ' RKS 03/05/2005 Added CopyOfOriginal & OriginalDocumentTemplateID
    ' ***************************************************************** '


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
    Private Const ACClass As String = "SIRDocTemplate"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lDocumentTemplateId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lDocumentTypeId As Integer
    Private m_iCreatedById As Integer
    Private m_dtDateCreated As Date
    Private m_iModifiedById As Integer
    Private m_dtLastModified As Date
    Private m_iIsDeleted As Integer
    Private m_vSlotNumber As Object
    Private m_vRiskCodeId As Object
    Private m_vRiskGroupId As Object
    Private m_iIsEditableAfterMerging As Integer
    Private m_sDocumentFilter As String = ""
    Private m_iCopyOfOriginal As Integer
    Private m_vOriginalDocumentTemplateID As Object
    Private m_dtEffectiveDate As Date
    Private m_bIsVisibleFromWeb As Boolean
    Private m_bIsVisibleFromClientManager As Boolean
    Private m_sPrinter As String = ""
    Private m_sChaser As String = ""
    Private m_bArchiveWithNoPrint As Boolean
    Private m_bEmailAsBody As Boolean
    Private m_bSpoolDocument As Boolean
    Private m_bArchiveAsText As Boolean
    Private m_lTemplateGroupID As Integer
    Private m_lTemplateSubGroupID As Integer
    Private m_bIsInternalOnly As Boolean
    Private m_bIsSelectedByDefault As Boolean
    Private m_bArchiveAsXML As Boolean
    Private m_sEmailSubTemplateCode As String = ""
    Private m_sEmailAttachmentTemplateCode As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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

    Public Property DocumentTemplateId() As Integer
        Get
            Return m_lDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTemplateId = Value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
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

    Public Property SourceId() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public Property DocumentTypeId() As Integer
        Get
            Return m_lDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
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

    Public Property IsDeleted() As Integer
        Get
            Return m_iIsDeleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeleted = Value
        End Set
    End Property

    Public Property SlotNumber() As Object
        Get
            Return m_vSlotNumber
        End Get
        Set(ByVal Value As Object)


            m_vSlotNumber = Value
        End Set
    End Property

    Public Property RiskCodeId() As Object
        Get
            Return m_vRiskCodeId
        End Get
        Set(ByVal Value As Object)


            m_vRiskCodeId = Value
        End Set
    End Property

    Public Property RiskGroupId() As Object
        Get
            Return m_vRiskGroupId
        End Get
        Set(ByVal Value As Object)


            m_vRiskGroupId = Value
        End Set
    End Property

    Public Property IsEditableAfterMerging() As Integer
        Get
            Return m_iIsEditableAfterMerging
        End Get
        Set(ByVal Value As Integer)
            m_iIsEditableAfterMerging = Value
        End Set
    End Property

    Public Property Printer() As String
        Get
            Return m_sPrinter
        End Get
        Set(ByVal Value As String)
            m_sPrinter = Value
        End Set
    End Property


    Public Property DocumentFilter() As String
        Get
            Return m_sDocumentFilter
        End Get
        Set(ByVal Value As String)
            m_sDocumentFilter = Value
        End Set
    End Property


    Public Property CopyOfOriginal() As Integer
        Get
            Return m_iCopyOfOriginal
        End Get
        Set(ByVal Value As Integer)
            m_iCopyOfOriginal = Value
        End Set
    End Property


    Public Property OriginalDocumentTemplateID() As Object
        Get
            Return m_vOriginalDocumentTemplateID
        End Get
        Set(ByVal Value As Object)


            m_vOriginalDocumentTemplateID = Value
        End Set
    End Property

    Public Property Chaser() As String
        Get
            Return m_sChaser
        End Get
        Set(ByVal Value As String)
            m_sChaser = Value
        End Set
    End Property

    Public Property IsVisibleFromWeb() As Boolean
        Get
            Return m_bIsVisibleFromWeb
        End Get
        Set(ByVal Value As Boolean)
            m_bIsVisibleFromWeb = Value
        End Set
    End Property


    Public Property IsVisibleFromClientManager() As Boolean
        Get
            Return m_bIsVisibleFromClientManager
        End Get
        Set(ByVal Value As Boolean)
            m_bIsVisibleFromClientManager = Value
        End Set
    End Property

    Public Property ArchiveWithNoPrint() As Boolean
        Get
            Return m_bArchiveWithNoPrint
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveWithNoPrint = Value
        End Set
    End Property


    Public Property EmailAsBody() As Boolean
        Get
            Return m_bEmailAsBody
        End Get
        Set(ByVal Value As Boolean)
            m_bEmailAsBody = Value
        End Set
    End Property

    Public Property SpoolDocument() As Boolean
        Get
            Return m_bSpoolDocument
        End Get
        Set(ByVal Value As Boolean)
            m_bSpoolDocument = Value
        End Set
    End Property

    Public Property ArchiveAsText() As Boolean
        Get
            Return m_bArchiveAsText
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveAsText = Value
        End Set
    End Property
    Public Property ArchiveAsXML() As Boolean
        Get
            Return m_bArchiveAsXML
        End Get
        Set(ByVal Value As Boolean)
            m_bArchiveAsXML = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public Property TemplateGroupID() As Integer
        Get
            Return m_lTemplateGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lTemplateGroupID = Value
        End Set
    End Property

    Public Property TemplateSubGroupID() As Integer
        Get
            Return m_lTemplateSubGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lTemplateSubGroupID = Value
        End Set
    End Property

    Public Property IsInternalOnly() As Boolean
        Get
            Return m_bIsInternalOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bIsInternalOnly = Value
        End Set
    End Property

    Public Property IsSelectedByDefault() As Boolean
        Get
            Return m_bIsSelectedByDefault
        End Get
        Set(ByVal Value As Boolean)
            m_bIsSelectedByDefault = Value
        End Set
    End Property

    Public Property EmailSubTemplateCode() As String
        Get
            Return m_sEmailSubTemplateCode
        End Get
        Set(ByVal value As String)
            m_sEmailSubTemplateCode = value
        End Set
    End Property

    Public Property EmailAttachmentTemplateCode() As String
        Get
            Return m_sEmailAttachmentTemplateCode
        End Get
        Set(ByVal value As String)
            m_sEmailAttachmentTemplateCode = value
        End Set
    End Property

    Public Property CCMDocumentName() As String

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
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
                'Developer Guide No 111
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

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
    'developer guide no. 112 (guide)
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                DocumentTemplateId = oFields("document_template_id")
                Code = oFields("code")

                If Convert.IsDBNull(oFields("description")) Or Informations.IsNothing(oFields("description")) Then
                    Description = Nothing
                Else
                    Description = oFields("description")
                End If

                SourceId = oFields("source_id")
                DocumentTypeId = oFields("document_type_id")

                If Convert.IsDBNull(oFields("created_by_id")) Or Informations.IsNothing(oFields("created_by_id")) Then

                    CreatedById = Nothing
                Else
                    CreatedById = oFields("created_by_id")
                End If

                If Convert.IsDBNull(oFields("date_created")) Or Informations.IsNothing(oFields("date_created")) Then

                    DateCreated = Nothing
                Else
                    DateCreated = oFields("date_created")
                End If

                If Convert.IsDBNull(oFields("modified_by_id")) Or Informations.IsNothing(oFields("modified_by_id")) Then

                    ModifiedById = Nothing
                Else
                    ModifiedById = oFields("modified_by_id")
                End If

                If Convert.IsDBNull(oFields("last_modified")) Or Informations.IsNothing(oFields("last_modified")) Then

                    LastModified = Nothing
                Else
                    LastModified = oFields("last_modified")
                End If
                IsDeleted = oFields("is_deleted")

                If Convert.IsDBNull(oFields("slot_number")) Or Informations.IsNothing(oFields("slot_number")) Then

                    SlotNumber = Nothing
                Else

                    SlotNumber = oFields("slot_number")
                End If

                If Convert.IsDBNull(oFields("risk_code_id")) Or Informations.IsNothing(oFields("risk_code_id")) Then

                    RiskCodeId = Nothing
                Else

                    RiskCodeId = oFields("risk_code_id")
                End If

                If Convert.IsDBNull(oFields("risk_group_id")) Or Informations.IsNothing(oFields("risk_group_id")) Then

                    RiskGroupId = Nothing
                Else

                    RiskGroupId = oFields("risk_group_id")
                End If
                IsEditableAfterMerging = oFields("is_editable_after_merging")

                If Convert.IsDBNull(oFields("printer")) Or Informations.IsNothing(oFields("printer")) Then
                    Printer = ""
                Else
                    'DJM 08/04/2002 : Trim the following line in case it's a single space.
                    Printer = oFields("printer").Trim()
                End If

                'AK 090402

                If Convert.IsDBNull(oFields("chaser_description")) Or Informations.IsNothing(oFields("chaser_description")) Then
                    Chaser = ""
                Else
                    Chaser = oFields("chaser_description").Trim()
                End If

                DocumentFilter = "" & oFields("document_filter")

                CopyOfOriginal = gPMFunctions.ToSafeInteger(oFields("copy_of_original"), 0)

                OriginalDocumentTemplateID = oFields("original_document_template_id")

                EffectiveDate = oFields("effective_date")

                IsVisibleFromWeb = (gPMFunctions.ToSafeInteger(oFields("is_visible_from_web"), 0) = 1)

                IsVisibleFromClientManager = (gPMFunctions.ToSafeInteger(oFields("is_visible_from_client_manager"), 0) = 1)

                ArchiveWithNoPrint = (gPMFunctions.ToSafeInteger(oFields("archive_with_no_print"), 0) = 1)

                EmailAsBody = (gPMFunctions.ToSafeInteger(oFields("email_as_body"), 0) = 1)

                SpoolDocument = (gPMFunctions.ToSafeInteger(oFields("spool_document"), 0) = 1)

                ArchiveAsText = (gPMFunctions.ToSafeInteger(oFields("archive_as_text"), 0) = 1)
                ArchiveAsXML = (gPMFunctions.ToSafeInteger(oFields("archive_as_xml"), 0) = 1)

                TemplateGroupID = gPMFunctions.ToSafeInteger(oFields("document_template_group_id"), Nothing)

                TemplateSubGroupID = gPMFunctions.ToSafeInteger(oFields("document_template_sub_group_id"), Nothing)

                IsInternalOnly = (gPMFunctions.ToSafeInteger(oFields("is_portal_internal_only"), 0) = 1)

                IsSelectedByDefault = (gPMFunctions.ToSafeInteger(oFields("is_portal_selected_by_default"), 0) = 1)

                EmailSubTemplateCode = gPMFunctions.ToSafeString(oFields("email_sub_template_code"))
                EmailAttachmentTemplateCode = gPMFunctions.ToSafeString(oFields("email_attachment_template_code"))

                If Convert.IsDBNull(oFields("CCMDocumentTemplate")) OrElse String.IsNullOrEmpty(oFields("CCMDocumentTemplate")) Then
                    CCMDocumentName = ""
                Else
                    CCMDocumentName = oFields("CCMDocumentTemplate").Trim()
                End If
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

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="code", vValue:=Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(SourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_type_id", vValue:=CStr(DocumentTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="created_by_id", vValue:=CStr(CreatedById), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 113
            If DateCreated = #12:00:00 AM# OrElse Informations.IsNothing(DateCreated) Then
                m_lReturn = .Parameters.Add(sName:="date_created", vValue:=#12/29/1899#, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="date_created", vValue:=DateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="modified_by_id", vValue:=CStr(ModifiedById), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 113
            If LastModified = #12:00:00 AM# OrElse Informations.IsNothing(LastModified) Then
                m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=#12/29/1899#, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="last_modified", vValue:=LastModified, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_deleted", vValue:=CStr(IsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="slot_number", vValue:=SlotNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=RiskCodeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If RiskCodeId.Equals(DBNull.Value) Then
                RiskCodeId = 0
            End If
            If RiskCodeId = 0 Then

                'developer guide no. 85 (guide)
                m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                'RWH(26/07/01) Printer.
                m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=RiskCodeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="risk_group_id", vValue:=RiskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If RiskGroupId.Equals(DBNull.Value) Then
                RiskGroupId = 0
            End If
            If RiskGroupId = 0 Then

                'developer guide no. 85 (guide)
                m_lReturn = .Parameters.Add(sName:="risk_group_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                'RWH(26/07/01) Printer.
                m_lReturn = .Parameters.Add(sName:="risk_group_id", vValue:=RiskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_editable_after_merging", vValue:=CStr(IsEditableAfterMerging), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Printer.Trim() = "" Then
                'developer guide no. 85 (guide)
                m_lReturn = .Parameters.Add(sName:="printer", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                'RWH(26/07/01) Printer.
                m_lReturn = .Parameters.Add(sName:="printer", vValue:=Printer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            'AK - 090402 : Insert NULL into parameter if the chaser is an empty string.
            If Chaser.Trim() = "" Then

                'developer guide no. 85 (guide)
                m_lReturn = .Parameters.Add(sName:="chaser", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="chaser", vValue:=Chaser, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="document_filter", vValue:=DocumentFilter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If DocumentFilter.Trim() = "" Then

                'developer guide no. 85 (guide)
                m_lReturn = .Parameters.Add(sName:="document_filter", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="document_filter", vValue:=DocumentFilter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="copy_of_original", vValue:=CStr(CopyOfOriginal), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="original_document_template_id", vValue:=OriginalDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'developer guide no.40
            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=EffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR - NEXUS MTA
            m_lReturn = .Parameters.Add(sName:="is_visible_from_web", vValue:=CStr(If(IsVisibleFromWeb, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="is_visible_from_client_manager", vValue:=CStr(If(m_bIsVisibleFromClientManager, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="archive_with_no_print", vValue:=CStr(If(m_bArchiveWithNoPrint, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="email_as_body", vValue:=CStr(If(m_bEmailAsBody, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="spool_document", vValue:=CStr(If(m_bSpoolDocument, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="archive_as_text", vValue:=CStr(If(m_bArchiveAsText, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="archive_as_xml", vValue:=CStr(If(m_bArchiveAsXML, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .Parameters.Add(sName:="document_template_group_id", vValue:=If(m_lTemplateGroupID = 0, DBNull.Value, m_lTemplateGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_template_sub_group_id", vValue:=If(m_lTemplateSubGroupID = 0, DBNull.Value, m_lTemplateSubGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_portal_internal_only", vValue:=CStr(If(m_bIsInternalOnly, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_portal_selected_by_default", vValue:=CStr(If(m_bIsSelectedByDefault, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="email_sub_template_code", vValue:=EmailSubTemplateCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="email_attachment_template_code", vValue:=EmailAttachmentTemplateCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=m_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If String.IsNullOrEmpty(CCMDocumentName) Then
                m_lReturn = .Parameters.Add(sName:="CCMDocumentName", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="CCMDocumentName", vValue:=CCMDocumentName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
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
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="document_template_id", vValue:=CStr(DocumentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            m_lReturn = .Parameters.Add(sName:="document_template_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            DocumentTemplateId = .Parameters.Item("document_template_id").Value

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

