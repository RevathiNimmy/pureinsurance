Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("SIREvent_NET.SIREvent")>
Public NotInheritable Class SIREvent
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIREvent
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIREvent attributes.
    '
    ' CJB 07/04/2004 Add ShortDescription
    '
    ' Edit History:
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
    Private Const ACClass As String = "SIREvent"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lEventCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_vInsuranceFolderCnt As Object
    Private m_vInsuranceFileCnt As Object
    Private m_vClaimCnt As Object
    Private m_vDocumentCnt As Object
    Private m_vOldAddressCnt As Object
    Private m_vNewAddressCnt As Object
    Private m_vCampaignId As Object
    Private m_vDocumentType As Object
    Private m_vReportType As Object
    Private m_vEventType As Object
    Private m_dtEventDate As Date
    Private m_vDescription As Object

    ' CJB 07/04/2004
    Private m_vShortDescription As Object

    ' CTAF 240800 - OldPartyTypeID
    Private m_lOldPartyTypeID As Integer

    'sj 30/09/2002 - start
    Private m_vEventLogSubjectId As Object
    Private m_vEventTypeCode As Object
    Private m_vAccountKey As Object
    'sj 30/09/2002 - end

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    'sj 15/09/2003 - Start
    Private m_vFSAComplaintFolderCnt As Object
    'sj 15/09/2003 - End
    Private m_vPriorityCode As Object '2005 StickyNotes
    Private m_vIsCompleted As Object '2005 StickyNotes
    Private m_vPerilId As Object 'S4B Claim Enhancements R&D 2005
    'Plico 24-28
    'developer guide no. 101 (Guide)
    Private m_sRTFText As Object
    Private m_vCaseID As Object
    Private m_vIsManualDescription As Object
    Private m_vBatchID As Object
    Private m_sDocument_Path As String
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    'sj 30/09/2002 - end

    Public Property OldPartyTypeID() As Integer
        Get

            Return m_lOldPartyTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lOldPartyTypeID = Value

        End Set
    End Property

    'sj 30/09/2002 - start
    Public Property AccountKey() As Object
        Get
            Return m_vAccountKey
        End Get
        Set(ByVal Value As Object)


            m_vAccountKey = Value
        End Set
    End Property
    Public Property EventLogSubjectId() As Object
        Get
            Return m_vEventLogSubjectId
        End Get
        Set(ByVal Value As Object)


            m_vEventLogSubjectId = Value
        End Set
    End Property
    Public Property EventTypeCode() As Object
        Get
            Return m_vEventTypeCode
        End Get
        Set(ByVal Value As Object)


            m_vEventTypeCode = Value
        End Set
    End Property
    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    Public Property EventCnt() As Integer
        Get

            Return m_lEventCnt

        End Get
        Set(ByVal Value As Integer)

            m_lEventCnt = Value

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

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

    Public Property DocumentCnt() As Object
        Get

            Return m_vDocumentCnt

        End Get
        Set(ByVal Value As Object)



            m_vDocumentCnt = Value

        End Set
    End Property

    Public Property OldAddressCnt() As Object
        Get

            Return m_vOldAddressCnt

        End Get
        Set(ByVal Value As Object)



            m_vOldAddressCnt = Value

        End Set
    End Property

    Public Property NewAddressCnt() As Object
        Get

            Return m_vNewAddressCnt

        End Get
        Set(ByVal Value As Object)



            m_vNewAddressCnt = Value

        End Set
    End Property

    Public Property CampaignId() As Object
        Get

            Return m_vCampaignId

        End Get
        Set(ByVal Value As Object)



            m_vCampaignId = Value

        End Set
    End Property

    Public Property DocumentType() As Object
        Get

            Return m_vDocumentType

        End Get
        Set(ByVal Value As Object)



            m_vDocumentType = Value

        End Set
    End Property

    Public Property ReportType() As Object
        Get

            Return m_vReportType

        End Get
        Set(ByVal Value As Object)



            m_vReportType = Value

        End Set
    End Property

    Public Property EventType() As Object
        Get

            Return m_vEventType

        End Get
        Set(ByVal Value As Object)



            m_vEventType = Value

        End Set
    End Property

    Public Property UserId() As Integer
        Get

            Return m_iUserID

        End Get
        Set(ByVal Value As Integer)

            m_iUserID = Value

        End Set
    End Property

    Public Property EventDate() As Date
        Get

            Return m_dtEventDate

        End Get
        Set(ByVal Value As Date)

            m_dtEventDate = Value

        End Set
    End Property

    Public Property Description() As Object
        Get

            Return m_vDescription

        End Get
        Set(ByVal Value As Object)



            m_vDescription = Value

        End Set
    End Property

    ' CJB 07/04/2004
    Public Property ShortDescription() As Object
        Get
            Return m_vShortDescription
        End Get
        Set(ByVal Value As Object)


            m_vShortDescription = Value
        End Set
    End Property
    '2005 StickyNotes
    Public Property PriorityCode() As Object
        Get
            Return m_vPriorityCode
        End Get
        Set(ByVal Value As Object)


            m_vPriorityCode = Value
        End Set
    End Property
    Public Property IsCompleted() As Object
        Get
            Return m_vIsCompleted
        End Get
        Set(ByVal Value As Object)


            m_vIsCompleted = Value
        End Set
    End Property
    'S4B Claim Enhancements R&D 2005
    Public Property PerilId() As Object
        Get
            Return m_vPerilId
        End Get
        Set(ByVal Value As Object)


            m_vPerilId = Value
        End Set
    End Property

    Public Property CaseID() As Object
        Get
            Return m_vCaseID
        End Get
        Set(ByVal Value As Object)


            m_vCaseID = Value
        End Set
    End Property

    'developer guide no. 101 (Guide)
    Public Property RTFText() As Object
        Get
            Return m_sRTFText
        End Get
        'developer guide no. 101 (Guide)
        Set(ByVal Value As Object)
            m_sRTFText = Value
        End Set
    End Property


    Public Property IsManualDescription() As Object
        Get

            Return m_vIsManualDescription

        End Get
        Set(ByVal Value As Object)



            m_vIsManualDescription = Value

        End Set
    End Property


    Public Property BatchID() As Object
        Get
            Return m_vBatchID
        End Get
        Set(ByVal Value As Object)


            m_vBatchID = Value
        End Set
    End Property

    'sj 15/09/2003 - Start

    Public Property FSAComplaintFolderCnt() As Object
        Get
            Return m_vFSAComplaintFolderCnt
        End Get
        Set(ByVal Value As Object)


            m_vFSAComplaintFolderCnt = Value
        End Set
    End Property

    Public Property Document_Path() As String
        Get
            Return m_sDocument_Path
        End Get
        Set(ByVal Value As String)

            m_sDocument_Path = Value
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

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

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

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

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
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
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
                'developer guide no. 111
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
    '
    ' CJB 07/04/2004 Add ShortDescription
    '
    ' ***************************************************************** '
    'developer guide no. 112 (Guide)
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                EventCnt = oFields("event_cnt")
                PartyCnt = oFields("party_cnt")

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

                If Convert.IsDBNull(oFields("document_cnt")) Or Informations.IsNothing(oFields("document_cnt")) Then

                    DocumentCnt = Nothing
                Else

                    DocumentCnt = oFields("document_cnt")
                End If

                If Convert.IsDBNull(oFields("old_address_cnt")) Or Informations.IsNothing(oFields("old_address_cnt")) Then

                    OldAddressCnt = Nothing
                Else

                    OldAddressCnt = oFields("old_address_cnt")
                End If

                If Convert.IsDBNull(oFields("new_address_cnt")) Or Informations.IsNothing(oFields("new_address_cnt")) Then

                    NewAddressCnt = Nothing
                Else

                    NewAddressCnt = oFields("new_address_cnt")
                End If

                If Convert.IsDBNull(oFields("campaign_id")) Or Informations.IsNothing(oFields("campaign_id")) Then

                    CampaignId = Nothing
                Else

                    CampaignId = oFields("campaign_id")
                End If

                If Convert.IsDBNull(oFields("document_type_id")) Or Informations.IsNothing(oFields("document_type_id")) Then

                    DocumentType = Nothing
                Else

                    DocumentType = oFields("document_type_id")
                End If

                If Convert.IsDBNull(oFields("report_type_id")) Or Informations.IsNothing(oFields("report_type_id")) Then

                    ReportType = Nothing
                Else

                    ReportType = oFields("report_type_id")
                End If

                EventType = oFields("event_type_id")
                UserId = oFields("user_id")
                EventDate = oFields("event_date")

                If Convert.IsDBNull(oFields("description")) Or Informations.IsNothing(oFields("description")) Then

                    Description = Nothing
                Else

                    Description = oFields("description")
                End If

                ' CTAF 240800

                If Convert.IsDBNull(oFields("old_party_type_id")) Or Informations.IsNothing(oFields("old_party_type_id")) Then

                    OldPartyTypeID = Nothing
                Else
                    OldPartyTypeID = oFields("old_party_type_id")
                End If

                'sj 30/09/2002 - start

                If Convert.IsDBNull(oFields("event_log_subject_id")) Or Informations.IsNothing(oFields("event_log_subject_id")) Then

                    EventLogSubjectId = Nothing
                Else

                    EventLogSubjectId = oFields("event_log_subject_id")
                End If

                If Convert.IsDBNull(oFields("account_key")) Or Informations.IsNothing(oFields("account_key")) Then

                    AccountKey = Nothing
                Else

                    AccountKey = oFields("account_key")
                End If

                If Convert.IsDBNull(oFields("event_type_code")) Or Informations.IsNothing(oFields("event_type_code")) Then

                    EventTypeCode = Nothing
                Else

                    EventTypeCode = oFields("event_type_code")
                End If
                'sj 30/09/2002 - end

                ' CJB 07/04/2004

                If Convert.IsDBNull(oFields("short_description")) Or Informations.IsNothing(oFields("short_description")) Then

                    ShortDescription = Nothing
                Else

                    ShortDescription = oFields("short_description")
                End If


                If Convert.IsDBNull(oFields("peril_id")) Or Informations.IsNothing(oFields("peril_id")) Then

                    PerilId = Nothing
                Else

                    PerilId = oFields("peril_id")
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
    ' CJB 07/04/2004 Add ShortDescription
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(InsuranceFolderCnt) OrElse InsuranceFolderCnt Is DBNull.Value OrElse InsuranceFolderCnt = 0 Then
                m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=InsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(InsuranceFileCnt) OrElse InsuranceFileCnt Is DBNull.Value OrElse InsuranceFileCnt = 0 Then
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=InsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="claim_cnt", vValue:=ClaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_cnt", vValue:=DocumentCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="old_address_cnt", vValue:=OldAddressCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="new_address_cnt", vValue:=NewAddressCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="campaign_id", vValue:=CampaignId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_type_id", vValue:=DocumentType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="report_type_id", vValue:=ReportType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="event_type_id", vValue:=EventType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(UserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="event_date", vValue:=EventDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '        If Not IsNull(Description) Then
            '            m_lReturn = ValidateSQL(Description)
            '        End If

            m_lReturn = .Parameters.Add(sName:="description", vValue:=Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' CTAF 240800 (I dont like With statements. They suck)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="old_party_type_id", vValue:=CStr(OldPartyTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'sj 30/09/2002 - start
        m_lReturn = m_oDatabase.Parameters.Add(sName:="event_log_subject_id", vValue:=EventLogSubjectId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = m_oDatabase.Parameters.Add(sName:="account_key", vValue:=AccountKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = m_oDatabase.Parameters.Add(sName:="event_type_code", vValue:=EventTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'sj 30/09/2002 - end

        'sj 15/09/2003 - Start

        'developer guide no. 98 ' Following Line gives error hence need to be changed.
        'If (String.IsNullOrEmpty(m_vFSAComplaintFolderCnt)) Then
        If Informations.IsDBNull(m_vFSAComplaintFolderCnt) Or Informations.IsNothing(m_vFSAComplaintFolderCnt) Then
            m_oDatabase.Parameters.Add(sName:="fsa_complaint_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else
            m_oDatabase.Parameters.Add(sName:="fsa_complaint_folder_cnt", vValue:=m_vFSAComplaintFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If
        'sj 15/09/2003 - End

        ' CJB 07/04/2004
        '    If Not IsNull(ShortDescription) Then
        '        m_lReturn = ValidateSQL(ShortDescription)
        '    End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="short_description", vValue:=ShortDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '2005 StickyNotes Pass new parameters Priority Code & Completion Indicator
        m_lReturn = m_oDatabase.Parameters.Add(sName:="priority_code", vValue:=PriorityCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_completed", vValue:=IsCompleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_id", vValue:=PerilId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="case_id", vValue:=CaseID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="rtf_text", vValue:=RTFText, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'developer guide no. 98 (Guide)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_manual_description", vValue:=m_vIsManualDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'developer guide no. 98 (Guide)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_id", vValue:=m_vBatchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Path", vValue:=m_sDocument_Path, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
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

            m_lReturn = .Parameters.Add(sName:="event_cnt", vValue:=CStr(EventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            m_lReturn = .Parameters.Add(sName:="event_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            EventCnt = .Parameters.Item("event_cnt").Value

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

    'sj 15/09/2003 - End

    ' ***************************************************************** '
    ' Name: UpdateEventLogClaimPolicy (Public)
    '
    ' Description: Updates records in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateEventLogClaimPolicy() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="claim_cnt", vValue:=ClaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=InsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=InsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateEventLogClaimPolicySQL, sSQLName:=ACUpdateEventLogClaimPolicyName, bStoredProcedure:=ACUpdateEventLogClaimPolicyStored, lRecordsAffected:=lRecordsAffected)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEventLogClaimPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

