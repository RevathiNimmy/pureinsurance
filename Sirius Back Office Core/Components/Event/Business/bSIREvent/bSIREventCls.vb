Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

Friend NotInheritable Class SIREvent
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIREvent
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIREvent attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/11/2003
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

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIREvent As dSIREvent.SIREvent
    'Private m_dSIREvent As dSIREvent.SIREvent

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lEventCnt As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

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

            ' Create instance of data class
            m_dSIREvent = New dSIREvent.SIREvent()
            'Set m_dSIREvent = New dSIREvent.SIREvent

            m_lReturn = m_dSIREvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)


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
                If m_dSIREvent IsNot Nothing Then
                    m_dSIREvent.Dispose()
                End If
            End If
            m_dSIREvent = Nothing
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIREvent.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults



            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=vEventDate, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIREvent property values.
    ' sj 30/09/2002 - Add Event Log Subject Id and Event Type Code
    ' CJB 07/04/2004 - Add ShortDescription
    ' 2005 StickyNotes New Properties - Priority Code & Completion Indicator
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Date = #12/30/1899#, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vEventLogSubjectId As Object = Nothing, Optional ByRef vEventTypeCode As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vFSAComplaintFolderCnt As Object = Nothing, Optional ByRef vShortDescription As Object = Nothing, Optional ByRef vPriorityCode As Object = Nothing, Optional ByRef vIsCompleted As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByVal vCaseID As Object = Nothing, Optional ByVal v_vRTFText As Object = "", Optional ByRef v_vIsManualDescription As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocument_Path As String = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' CJB 07/04/2004 - Added ShortDescription
                ' Default Any Missing Parameters
                ' 2005 StickyNotes - Priority Code & Completion Indicator
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=vEventDate, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vEventLogSubjectId:=vEventLogSubjectId, vEventTypeCode:=vEventTypeCode, vAccountKey:=vAccountKey, vShortDescription:=vShortDescription, vPriorityCode:=vPriorityCode, vIsCompleted:=vIsCompleted, vPerilId:=vPerilId, vCaseID:=vCaseID, v_vRTFText:=v_vRTFText, v_vIsManualDescription:=v_vIsManualDescription, vBatchID:=vBatchID, vDocument_Path:=vDocument_Path), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vEventCnt:=vEventCnt, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocumentCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentType, vReportType:=vReportType, vEventType:=vEventType, vUserId:=vUserId, vEventDate:=vEventDate, vDescription:=vDescription, vOldPartyTypeID:=vOldPartyTypeID, vDocument_Path:=vDocument_Path), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIREvent


                If Not Informations.IsNothing(vEventCnt) Then

                    If .EventCnt.Equals(0) Or (.EventCnt <> vEventCnt) Then
                        .EventCnt = vEventCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vPartyCnt) Then

                    If .PartyCnt.Equals(0) Or (.PartyCnt <> vPartyCnt) Then
                        .PartyCnt = vPartyCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vInsuranceFolderCnt) Then



                    If Object.Equals(.InsuranceFolderCnt, Nothing) OrElse (Not .InsuranceFolderCnt.Equals(vInsuranceFolderCnt)) Then
                        'developer guide no. 24
                        .InsuranceFolderCnt = vInsuranceFolderCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vInsuranceFileCnt) Then
                    'developer guide no. 44
                    If Object.Equals(.InsuranceFileCnt, Nothing) OrElse (Not .InsuranceFileCnt.Equals(vInsuranceFileCnt)) Then
                        'developer guide no. 24
                        .InsuranceFileCnt = vInsuranceFileCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vClaimCnt) Then
                    'developer guide no. 44
                    If Object.Equals(.ClaimCnt, Nothing) OrElse (Not .ClaimCnt.Equals(vClaimCnt)) Then
                        'developer guide no. 24
                        .ClaimCnt = vClaimCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDocumentCnt) Then
                    'developer guide no. 44
                    If Object.Equals(.DocumentCnt, Nothing) OrElse (Not .DocumentCnt.Equals(vDocumentCnt)) Then
                        'developer guide no. 24                       
                        .DocumentCnt = vDocumentCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vOldAddressCnt) Then
                    'developer guide no. 44
                    If Object.Equals(.OldAddressCnt, Nothing) OrElse (Not .OldAddressCnt.Equals(vOldAddressCnt)) Then
                        'developer guide no. 24
                        .OldAddressCnt = vOldAddressCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vNewAddressCnt) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.NewAddressCnt, Nothing) OrElse (Not .NewAddressCnt.Equals(vNewAddressCnt)) Then
                        'developer guide no. 24 (Guide)
                        .NewAddressCnt = vNewAddressCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vCampaignId) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.CampaignId, Nothing) OrElse (Not .CampaignId.Equals(vCampaignId)) Then
                        'developer guide no. 24 (Guide)
                        .CampaignId = vCampaignId
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDocumentType) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.DocumentType, Nothing) OrElse (Not .DocumentType.Equals(vDocumentType)) Then
                        'developer guide no. 24 (Guide)
                        .DocumentType = vDocumentType
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vReportType) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.ReportType, Nothing) OrElse (Not .ReportType.Equals(vReportType)) Then
                        'developer guide no. 24 (Guide)
                        .ReportType = vReportType
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vEventType) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.EventType, Nothing) OrElse (Not .EventType.Equals(vEventType)) Then
                        'developer guide no. 24 (Guide)
                        .EventType = vEventType
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vUserId) Then

                    If .UserId.Equals(0) Or (.UserId <> vUserId) Then
                        .UserId = vUserId
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vEventDate) Then

                    If .EventDate.Equals(DateTime.FromOADate(0)) Or (.EventDate <> vEventDate) Then
                        .EventDate = vEventDate
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDescription) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.Description, Nothing) OrElse (Not .Description.Equals(vDescription)) Then
                        'developer guide no. 24 (Guide)
                        .Description = vDescription
                        bDataChanged = True
                    End If
                End If

                ' CTAF 240800

                If Not Informations.IsNothing(vOldPartyTypeID) AndAlso Not vOldPartyTypeID Is DBNull.Value Then

                    If .OldPartyTypeID.Equals(0) Or (.OldPartyTypeID <> vOldPartyTypeID) Then
                        .OldPartyTypeID = vOldPartyTypeID
                        bDataChanged = True
                    End If
                End If

                'sj 30/09/2002 - start

                If Not Informations.IsNothing(vEventLogSubjectId) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.EventLogSubjectId, Nothing) OrElse (Not .EventLogSubjectId.Equals(vEventLogSubjectId)) Then
                        'developer guide no. 24 (Guide)
                        .EventLogSubjectId = vEventLogSubjectId
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vAccountKey) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.AccountKey, Nothing) OrElse (Not .AccountKey.Equals(vAccountKey)) Then
                        'developer guide no. 24 (Guide)
                        .AccountKey = vAccountKey
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vEventTypeCode) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.EventTypeCode, Nothing) OrElse (Not .EventTypeCode.Equals(vEventTypeCode)) Then
                        'developer guide no. 24 (Guide)
                        .EventTypeCode = vEventTypeCode
                        bDataChanged = True
                    End If
                End If
                'sj 30/09/2002 - end

                'sj 15/09/2002 - start

                If Not Informations.IsNothing(vFSAComplaintFolderCnt) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.FSAComplaintFolderCnt, Nothing) OrElse (Not .FSAComplaintFolderCnt.Equals(vFSAComplaintFolderCnt)) Then
                        'developer guide no. 24 (Guide)
                        .FSAComplaintFolderCnt = vFSAComplaintFolderCnt
                        bDataChanged = True
                    End If
                End If
                'sj 15/09/2002 - end

                ' CJB 07/04/2004

                If Not Informations.IsNothing(vShortDescription) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.ShortDescription, Nothing) OrElse (Not .ShortDescription.Equals(vShortDescription)) Then
                        'developer guide no. 24 (Guide)
                        .ShortDescription = vShortDescription
                        bDataChanged = True
                    End If
                End If
                '2005 StickyNotes

                If Not Informations.IsNothing(vPriorityCode) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.PriorityCode, Nothing) OrElse (Not .PriorityCode.Equals(vPriorityCode)) Then
                        'developer guide no. 24 (Guide)
                        .PriorityCode = vPriorityCode
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vIsCompleted) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.IsCompleted, Nothing) OrElse (Not .IsCompleted.Equals(vIsCompleted)) Then
                        'developer guide no. 24 (Guide)
                        .IsCompleted = vIsCompleted
                        bDataChanged = True
                    End If
                End If

                'S4B Claim Enhancements R&D 2005

                If Not Informations.IsNothing(vPerilId) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.PerilId, Nothing) OrElse (Not .PerilId.Equals(vPerilId)) Then
                        'developer guide no. 24 (Guide)
                        .PerilId = vPerilId
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vCaseID) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.CaseID, Nothing) OrElse (Not .CaseID.Equals(vCaseID)) Then
                        'developer guide no. 24 (Guide)
                        .CaseID = vCaseID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(v_vRTFText) Then

                    'developer guide no. 44 (Guide)
                    If Object.Equals(.RTFText, Nothing) OrElse (Not .RTFText.Equals(v_vRTFText)) Then
                        .RTFText = v_vRTFText
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(v_vIsManualDescription) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.IsManualDescription, Nothing) OrElse (Not .IsManualDescription.Equals(v_vIsManualDescription)) Then
                        'developer guide no. 24 (Guide)
                        .IsManualDescription = v_vIsManualDescription
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vBatchID) Then
                    'developer guide no. 44 (Guide)
                    If Object.Equals(.BatchID, Nothing) OrElse (Not .BatchID.Equals(vBatchID)) Then
                        'developer guide no. 24 (Guide)
                        .BatchID = vBatchID
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vDocument_Path) Then

                    If Object.Equals(.Document_Path, Nothing) OrElse (Not .BatchID.Equals(vDocument_Path)) Then

                        .Document_Path = vDocument_Path
                        bDataChanged = True
                    End If
                End If

                ' If we have changed one of the properties, update the status
                If bDataChanged Then
                    m_iDatabaseStatus = iStatus
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIREvent property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vEventCnt As Integer = 0, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Integer = 0, Optional ByRef vEventDate As Date = #12/30/1899#, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Integer = 0, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vBatchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIREvent
                'Developer Guie No 118
                vEventCnt = .EventCnt
                vPartyCnt = .PartyCnt
                vInsuranceFolderCnt = .InsuranceFolderCnt
                vInsuranceFileCnt = .InsuranceFileCnt
                vClaimCnt = .ClaimCnt
                vDocumentCnt = .DocumentCnt
                vOldAddressCnt = .OldAddressCnt
                vNewAddressCnt = .NewAddressCnt
                vCampaignId = .CampaignId
                vDocumentType = .DocumentType
                vReportType = .ReportType
                vEventType = .EventType
                vUserId = .UserId
                vEventDate = .EventDate
                vDescription = .Description
                vOldPartyTypeID = .OldPartyTypeID
                vPerilId = .PerilId
                vBatchID = .BatchID
                iStatus = m_iDatabaseStatus
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIREvent

                ' Set Data object primary key
                .EventCnt = EventCnt

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIREvent

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIREvent Added
                EventCnt = .EventCnt

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIREvent

                ' Set Data object primary key
                .EventCnt = EventCnt

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIREvent

                ' Set Data object primary key
                '        .EventCnt = EventCnt

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateEventLogClaimPolicy (Public)
    '
    ' Description: Updates All Claim Events record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateEventLogClaimPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIREvent

                ' Set Data object primary key
                '        .EventCnt = EventCnt

                ' Update the record on the database from the object
                'developer guide no. 83(Guide)
                m_lReturn = .UpdateEventLogClaimPolicy()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateEventLogClaimPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEventLogClaimPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIREvent.
    ' sj 30/09/2002 - Add Event Log Subject Id and Event Type Code
    ' CJB 07/04/2004 - Add ShortDescription
    ' 2005 StickyNotes - Add New Parameters - Priority Code & Completion Indicator
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vEventLogSubjectId As Object = Nothing, Optional ByRef vEventTypeCode As Object = Nothing, Optional ByRef vAccountKey As Object = Nothing, Optional ByRef vFSAComplaintFolderCnt As Object = Nothing, Optional ByRef vShortDescription As Object = Nothing, Optional ByRef vPriorityCode As Object = Nothing, Optional ByRef vIsCompleted As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByVal vCaseID As Object = Nothing, Optional ByRef v_vRTFText As Object = Nothing, Optional ByRef v_vIsManualDescription As Object = Nothing, Optional ByRef vBatchID As Object = Nothing, Optional ByRef vDocument_Path As String = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'Developer guie No 44

        If (Informations.IsNothing(vEventCnt)) OrElse (vEventCnt.Equals(0)) OrElse (bDefaultAll) Then
            vEventCnt = 0
        End If



        If (Informations.IsNothing(vPartyCnt)) OrElse (Object.Equals(vPartyCnt, Nothing)) OrElse (bDefaultAll) Then


            vPartyCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vInsuranceFolderCnt)) OrElse (Object.Equals(vInsuranceFolderCnt, Nothing)) OrElse (bDefaultAll) Then


            vInsuranceFolderCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vInsuranceFileCnt)) OrElse (Object.Equals(vInsuranceFileCnt, Nothing)) OrElse (bDefaultAll) Then


            vInsuranceFileCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vClaimCnt)) OrElse (Object.Equals(vClaimCnt, Nothing)) OrElse (bDefaultAll) Then


            vClaimCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vDocumentCnt)) OrElse (Object.Equals(vDocumentCnt, Nothing)) OrElse (bDefaultAll) Then


            vDocumentCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vOldAddressCnt)) OrElse (Object.Equals(vOldAddressCnt, Nothing)) OrElse (bDefaultAll) Then


            vOldAddressCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vNewAddressCnt)) OrElse (Object.Equals(vNewAddressCnt, Nothing)) OrElse (bDefaultAll) Then


            vNewAddressCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vCampaignId)) OrElse (Object.Equals(vCampaignId, Nothing)) OrElse (bDefaultAll) Then


            vCampaignId = DBNull.Value
        End If



        If (Informations.IsNothing(vDocumentType)) OrElse (Object.Equals(vDocumentType, Nothing)) OrElse (bDefaultAll) Then


            vDocumentType = DBNull.Value
        End If



        If (Informations.IsNothing(vReportType)) OrElse (Object.Equals(vReportType, Nothing)) OrElse (bDefaultAll) Then


            vReportType = DBNull.Value
        End If



        If (Informations.IsNothing(vEventType)) OrElse (Object.Equals(vEventType, Nothing)) OrElse (bDefaultAll) Then


            vEventType = DBNull.Value
        End If



        'developer guide no. 44 (Guide)
        If (Informations.IsNothing(vUserId)) OrElse (vUserId.Equals(0)) OrElse (bDefaultAll) Then
            vUserId = m_iUserID
        End If



        If (Informations.IsNothing(vEventDate)) OrElse (vEventDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vEventDate = DateTime.Today
        End If



        If (Informations.IsNothing(vDescription)) OrElse (Object.Equals(vDescription, Nothing)) OrElse (bDefaultAll) Then


            vDescription = DBNull.Value
        End If

        ' CTAF 240800


        If (Informations.IsNothing(vOldPartyTypeID)) OrElse (Object.Equals(vOldPartyTypeID, Nothing)) OrElse (bDefaultAll) Then


            vOldPartyTypeID = DBNull.Value
        End If

        'sj 30/09/2002 - start


        If (Informations.IsNothing(vEventLogSubjectId)) OrElse (Object.Equals(vEventLogSubjectId, Nothing)) OrElse (bDefaultAll) Then


            vEventLogSubjectId = DBNull.Value
        End If



        If (Informations.IsNothing(vAccountKey)) OrElse (Object.Equals(vAccountKey, Nothing)) OrElse (bDefaultAll) Then


            vAccountKey = DBNull.Value
        End If



        If (Informations.IsNothing(vEventTypeCode)) OrElse (Object.Equals(vEventTypeCode, Nothing)) OrElse (bDefaultAll) Then


            vEventTypeCode = DBNull.Value
        End If
        'sj 30/09/2002 - end

        'sj 15/09/2003 - Start


        If (Informations.IsNothing(vFSAComplaintFolderCnt)) OrElse (Object.Equals(vFSAComplaintFolderCnt, Nothing)) OrElse (bDefaultAll) Then


            vFSAComplaintFolderCnt = DBNull.Value
        End If
        'sj 15/09/2003 - End

        ' CJB 07/04/2004


        If (Informations.IsNothing(vShortDescription)) OrElse (Object.Equals(vShortDescription, Nothing)) OrElse (bDefaultAll) Then


            vShortDescription = DBNull.Value
        End If
        ' 2005 StickyNotes - Add New Parameters - Priority Code & Completion Indicator


        If (Informations.IsNothing(vPriorityCode)) OrElse (Object.Equals(vPriorityCode, Nothing)) OrElse (bDefaultAll) Then


            vPriorityCode = DBNull.Value
        End If


        If (Informations.IsNothing(vIsCompleted)) OrElse (Object.Equals(vIsCompleted, Nothing)) OrElse (bDefaultAll) Then


            vIsCompleted = DBNull.Value
        End If

        'S4B Claims Enhancements R&D 2005


        If (Informations.IsNothing(vPerilId)) OrElse (Object.Equals(vPerilId, Nothing)) OrElse (bDefaultAll) Then


            vPerilId = DBNull.Value
        End If

        'vCaseID


        If (Informations.IsNothing(vCaseID)) OrElse (Object.Equals(vCaseID, Nothing)) OrElse (bDefaultAll) Then


            vCaseID = DBNull.Value
        End If



        If (Informations.IsNothing(v_vRTFText)) OrElse (Object.Equals(v_vRTFText, Nothing)) OrElse (bDefaultAll) Then


            v_vRTFText = DBNull.Value
        End If




        If (Informations.IsNothing(v_vIsManualDescription)) OrElse (Object.Equals(v_vIsManualDescription, Nothing)) OrElse (bDefaultAll) Then


            v_vIsManualDescription = DBNull.Value
        End If



        If (Informations.IsNothing(vBatchID)) OrElse (Object.Equals(vBatchID, Nothing)) OrElse (bDefaultAll) Then


            vBatchID = DBNull.Value
        End If


        If (Informations.IsNothing(vDocument_Path)) OrElse (Object.Equals(vDocument_Path, Nothing)) OrElse (bDefaultAll) Then


            vDocument_Path = String.Empty
        End If
        'Ends

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIREvent for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vEventCnt As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDocumentCnt As Object = Nothing, Optional ByRef vOldAddressCnt As Object = Nothing, Optional ByRef vNewAddressCnt As Object = Nothing, Optional ByRef vCampaignId As Object = Nothing, Optional ByRef vDocumentType As Object = Nothing, Optional ByRef vReportType As Object = Nothing, Optional ByRef vEventType As Object = Nothing, Optional ByRef vUserId As Object = Nothing, Optional ByRef vEventDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vOldPartyTypeID As Object = Nothing, Optional ByRef vDocument_Path As String = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vEventCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vEventCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDocumentCnt) Then

            If Not (Convert.IsDBNull(vDocumentCnt) Or Informations.IsNothing(vDocumentCnt)) Then

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vDocumentCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vCampaignId) Then

            If Not (Convert.IsDBNull(vCampaignId) Or Informations.IsNothing(vCampaignId)) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vCampaignId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vUserId) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vUserId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

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

