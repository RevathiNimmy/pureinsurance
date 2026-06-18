Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

Friend NotInheritable Class SIRDocTemplate
    Implements IDisposable
    Private Const ACClass As String = "SIRDocTemplate"

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
    ' Update Status
    Private m_iDatabaseStatus As Integer
    ' Instance of Data component
    Private m_dSIRDocTemplate As dSIRDocTemplate.SIRDocTemplate
    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Primary Keys to work with
    Private m_lDocumentTemplateId As Integer

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
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
            m_dSIRDocTemplate = New dSIRDocTemplate.SIRDocTemplate()

            m_lReturn = m_dSIRDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRDocTemplate IsNot Nothing Then
                    m_dSIRDocTemplate.Dispose()
                End If
                m_dSIRDocTemplate = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRDocTemplate.
    '
    ' ***************************************************************** '

    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocumentTemplateId As Object = Nothing,
                                Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                                Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                                Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                                Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                                Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                                Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                                Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing,
                                Optional ByRef vIsVisibleFromWeb As Object = Nothing, Optional ByRef vIsVisibleFromClientManager As Object = Nothing,
                                Optional ByRef vArchiveWithNoPrint As Object = Nothing, Optional ByRef vEmailAsBody As Object = Nothing,
                                Optional ByRef vSpoolDocument As Object = Nothing, Optional ByRef r_sCCMDocumentName As String = "") As Integer ''Saurabh - Added there new parameters for Tech Spec PGR005 Automated Emails

        '
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults




            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType,
                                                vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode, vDescription:=vDescription,
                                                vSourceId:=vSourceId, vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById,
                                                vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified,
                                                vIsDeleted:=vIsDeleted, vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId,
                                                vIsEditableAfterMerging:=vIsEditableAfterMerging, vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb,
                                                vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint, vEmailAsBody:=vEmailAsBody,
                                                vSpoolDocument:=vSpoolDocument, r_sCCMDocumentName:=r_sCCMDocumentName), gPMConstants.PMEReturnCode)


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
    ' Description: Sets the supplied SIRDocTemplate property values.
    '
    ' ***************************************************************** '
    'AK 090402 add chaser info

    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vDocumentTemplateId As Object = Nothing,
                                  Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                                  Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                                  Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                                  Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                                  Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                                  Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                                  Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vPrinter As Object = Nothing,
                                  Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing,
                                  Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing,
                                  Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsVisibleFromWeb As Object = Nothing,
                                  Optional ByRef vIsVisibleFromClientManager As Object = Nothing, Optional ByRef vArchiveWithNoPrint As Object = Nothing,
                                  Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing,
                                  Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vTemplateGroupID As Object = Nothing,
                                  Optional ByRef vTemplateSubGroupID As Object = Nothing, Optional ByRef vIsInternalOnly As Object = Nothing,
                                  Optional ByRef vIsSelectedByDefault As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing,
                                  Optional ByRef vEmailSubTemplateCode As Object = Nothing, Optional ByRef vEmailAttachmentTemplateCode As Object = Nothing, Optional ByRef r_sCCMDocumentName As Object = Nothing, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode, vDescription:=vDescription, vSourceId:=vSourceId, vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vIsDeleted:=vIsDeleted, vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging, vPrinter:=vPrinter, vChaser:=vChaser, vDocumentFilter:=vDocumentFilter, vCopyOfOriginal:=vCopyOfOriginal, vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID, vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb, vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint, vEmailAsBody:=vEmailAsBody, vSpoolDocument:=vSpoolDocument, vArchiveAsText:=vArchiveAsText, vArchiveAsXML:=vArchiveAsXML), gPMConstants.PMEReturnCode) ''Saurabh - Tech spec PGR005 - Automated Emails

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode, vDescription:=vDescription, vSourceId:=vSourceId, vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vIsDeleted:=vIsDeleted, vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging, vPrinter:=vPrinter, vChaser:=vChaser, vDocumentFilter:=vDocumentFilter, vCopyOfOriginal:=vCopyOfOriginal, vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID, vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb, vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint, vEmailAsBody:=vEmailAsBody, vSpoolDocument:=vSpoolDocument, vArchiveAsText:=vArchiveAsText, vArchiveAsXML:=vArchiveAsXML), gPMConstants.PMEReturnCode) ''Saurabh - Tech spec PGR005 - Automated Emails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRDocTemplate

                If Not Informations.IsNothing(vDocumentTemplateId) Then
                    If .DocumentTemplateId.Equals(0) Or (.DocumentTemplateId <> vDocumentTemplateId) Then
                        .DocumentTemplateId = vDocumentTemplateId
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vSlotNumber) Then
                    If Not (Convert.IsDBNull(vSlotNumber) Or Informations.IsNothing(vSlotNumber)) Then
                        If Object.Equals(.SlotNumber, Nothing) Or (("" & .SlotNumber).Trim() <> vSlotNumber.ToString().Trim()) Then
                            .SlotNumber = vSlotNumber
                            bDataChanged = True
                        End If
                    End If
                End If

                If Not Informations.IsNothing(vCode) Then
                    If String.IsNullOrEmpty(.Code) Or (.Code.Trim() <> vCode.Trim()) Then
                        .Code = vCode
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vDescription) Then
                    If String.IsNullOrEmpty(.Description) Or (.Description.Trim() <> vDescription.Trim()) Then
                        .Description = vDescription
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vSourceId) Then
                    If .SourceId.Equals(0) Or (.SourceId <> vSourceId) Then
                        .SourceId = vSourceId
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vDocumentTypeId) Then
                    If .DocumentTypeId.Equals(0) Or (.DocumentTypeId <> vDocumentTypeId) Then
                        .DocumentTypeId = vDocumentTypeId
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vCreatedById) Then
                    If .CreatedById.Equals(0) Or (.CreatedById <> vCreatedById) Then
                        .CreatedById = vCreatedById
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vDateCreated) Then
                    If .DateCreated.Equals(DateTime.FromOADate(0)) Or (.DateCreated <> vDateCreated) Then
                        .DateCreated = vDateCreated
                        bDataChanged = True
                    End If
                End If

                If .CreatedById.Equals(0) Then
                    .CreatedById = m_iUserID
                    .DateCreated = DateTime.Now
                    bDataChanged = True
                End If

                If Not Informations.IsNothing(vModifiedById) Then
                    If .ModifiedById.Equals(0) Or (.ModifiedById <> vModifiedById) Then
                        .ModifiedById = vModifiedById
                        bDataChanged = True
                    End If
                Else
                    .ModifiedById = m_iUserID
                End If

                If Not Informations.IsNothing(vLastModified) Then
                    If .LastModified.Equals(DateTime.FromOADate(0)) Or (.LastModified <> vLastModified) Then
                        .LastModified = vLastModified
                        bDataChanged = True
                    End If
                Else
                    .LastModified = DateTime.Now
                End If

                If Not Informations.IsNothing(vIsDeleted) Then
                    If .IsDeleted.Equals(0) Or (.IsDeleted <> vIsDeleted) Then
                        .IsDeleted = vIsDeleted
                        bDataChanged = True
                    End If
                End If

                If Not (Informations.IsNothing(vSlotNumber) Or Informations.IsDBNull(vSlotNumber)) Then
                    If Object.Equals(.SlotNumber, Nothing) Or (.SlotNumber <> vSlotNumber) Then
                        .SlotNumber = vSlotNumber
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vRiskCodeId) Then
                    If Object.Equals(.RiskCodeId, Nothing) Or ((Convert.IsDBNull(.RiskCodeId) Or Informations.IsNothing(.RiskCodeId)) And Not (Convert.IsDBNull(vRiskCodeId) Or Informations.IsNothing(vRiskCodeId))) Or (Not (Convert.IsDBNull(.RiskCodeId) Or Informations.IsNothing(.RiskCodeId)) And (Convert.IsDBNull(vRiskCodeId) Or Informations.IsNothing(vRiskCodeId))) Then
                        .RiskCodeId = vRiskCodeId
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vRiskGroupId) Then
                    If Object.Equals(.RiskGroupId, Nothing) Or ((Convert.IsDBNull(.RiskGroupId) Or Informations.IsNothing(.RiskGroupId)) And Not (Convert.IsDBNull(vRiskGroupId) Or Informations.IsNothing(vRiskGroupId))) Or (Not (Convert.IsDBNull(.RiskGroupId) Or Informations.IsNothing(.RiskGroupId)) And (Convert.IsDBNull(vRiskGroupId) Or Informations.IsNothing(vRiskGroupId))) Then
                        .RiskGroupId = vRiskGroupId
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vIsEditableAfterMerging) Then
                    If .IsEditableAfterMerging.Equals(0) Or (.IsEditableAfterMerging <> vIsEditableAfterMerging) Then
                        .IsEditableAfterMerging = vIsEditableAfterMerging
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vPrinter) Then
                    If String.IsNullOrEmpty(.Printer) Or (.Printer <> vPrinter) Then
                        .Printer = vPrinter
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vChaser) Then
                    If String.IsNullOrEmpty(.Chaser) Or (.Chaser <> vChaser) Then
                        .Chaser = vChaser
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDocumentFilter) Then
                    If String.IsNullOrEmpty(.DocumentFilter) Or (.DocumentFilter <> vDocumentFilter) Then
                        .DocumentFilter = vDocumentFilter
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vCopyOfOriginal) Then
                    If .CopyOfOriginal.Equals(0) Or (.CopyOfOriginal <> vCopyOfOriginal) Then
                        .CopyOfOriginal = vCopyOfOriginal
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vOriginalDocumentTemplateID) Then
                    If Object.Equals(.OriginalDocumentTemplateID, Nothing) OrElse (Not .OriginalDocumentTemplateID.Equals(vOriginalDocumentTemplateID)) Then
                        .OriginalDocumentTemplateID = vOriginalDocumentTemplateID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vEffectiveDate) Then
                    If Informations.IsDBNull(vEffectiveDate) Then 'vEffectiveDate Is Nothing Then
                        vEffectiveDate = "#12:00:01#"
                    End If
                    If .EffectiveDate.Equals(DateTime.FromOADate(0)) Or (.EffectiveDate <> vEffectiveDate) Then
                        .EffectiveDate = vEffectiveDate
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vIsVisibleFromWeb) Then
                    If vIsVisibleFromWeb.Equals(False) Or (vIsVisibleFromWeb <> .IsVisibleFromWeb) Then
                        .IsVisibleFromWeb = vIsVisibleFromWeb
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vIsVisibleFromClientManager) Then
                    If vIsVisibleFromClientManager.Equals(False) Or (vIsVisibleFromClientManager <> .IsVisibleFromClientManager) Then
                        .IsVisibleFromClientManager = vIsVisibleFromClientManager
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vArchiveWithNoPrint) Then
                    If vArchiveWithNoPrint.Equals(False) Or (vArchiveWithNoPrint <> .ArchiveWithNoPrint) Then
                        .ArchiveWithNoPrint = vArchiveWithNoPrint
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vEmailAsBody) Then
                    If vEmailAsBody.Equals(False) Or (vEmailAsBody <> .EmailAsBody) Then
                        .EmailAsBody = vEmailAsBody
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vSpoolDocument) Then
                    If vSpoolDocument.Equals(False) Or (vSpoolDocument <> .SpoolDocument) Then
                        .SpoolDocument = vSpoolDocument
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vArchiveAsText) Then
                    If vArchiveAsText.Equals(False) Or (vArchiveAsText <> .ArchiveAsText) Then
                        .ArchiveAsText = vArchiveAsText
                        bDataChanged = True
                    End If
                End If
                If Not Informations.IsNothing(vArchiveAsXML) Then
                    If vArchiveAsXML.Equals(False) Or (vArchiveAsXML <> .ArchiveAsXML) Then
                        .ArchiveAsXML = vArchiveAsXML
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vTemplateGroupID) Then
                    If vTemplateGroupID.Equals(0) Or (vTemplateGroupID <> .TemplateGroupID) Then
                        .TemplateGroupID = vTemplateGroupID
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vTemplateSubGroupID) Then
                    If vTemplateSubGroupID.Equals(0) Or (vTemplateSubGroupID <> .TemplateSubGroupID) Then
                        .TemplateSubGroupID = vTemplateSubGroupID
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vIsSelectedByDefault) Then
                    If vIsSelectedByDefault.Equals(False) Or (vIsSelectedByDefault <> .IsSelectedByDefault) Then
                        .IsSelectedByDefault = vIsSelectedByDefault
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vIsInternalOnly) Then
                    If vIsInternalOnly.Equals(False) Or (vIsInternalOnly <> .IsInternalOnly) Then
                        .IsInternalOnly = vIsInternalOnly
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(r_sCCMDocumentName) Then
                    If r_sCCMDocumentName = "None" Then
                        r_sCCMDocumentName = String.Empty
                    End If

                    If String.IsNullOrEmpty(.CCMDocumentName) OrElse (.CCMDocumentName <> r_sCCMDocumentName) Then
                        .CCMDocumentName = r_sCCMDocumentName
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vEmailSubTemplateCode) Then
                    If String.IsNullOrEmpty(.EmailSubTemplateCode) Or (.EmailSubTemplateCode.Trim() <> vEmailSubTemplateCode.Trim()) Then
                        .EmailSubTemplateCode = vEmailSubTemplateCode
                        bDataChanged = True
                    End If
                Else
                    .EmailSubTemplateCode = Nothing
                End If

                If Not Informations.IsNothing(vEmailAttachmentTemplateCode) Then
                    If String.IsNullOrEmpty(.EmailAttachmentTemplateCode) Or (.EmailAttachmentTemplateCode.Trim() <> vEmailAttachmentTemplateCode.Trim()) Then
                        .EmailAttachmentTemplateCode = vEmailAttachmentTemplateCode
                        bDataChanged = True
                    End If
                Else
                    .EmailAttachmentTemplateCode = Nothing
                End If

                If Not String.IsNullOrEmpty(v_sUniqueId) Then
                    .UniqueId = v_sUniqueId
                    .ScreenHierarchy = v_sScreenHierarchy
                End If

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

    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vDocumentTemplateId As Object = Nothing,
                                  Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                                  Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                                  Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#,
                                  Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                                  Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                                  Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                                  Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vPrinter As Object = Nothing,
                                  Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing,
                                  Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing,
                                  Optional ByRef vEffectiveDate As Date = #12/30/1899#, Optional ByRef vIsVisibleFromWeb As Object = Nothing,
                                  Optional ByRef vIsVisibleFromClientManager As Object = Nothing, Optional ByRef vArchiveWithNoPrint As Object = Nothing,
                                  Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing,
                                  Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vTemplateGroupID As Object = Nothing,
                                  Optional ByRef vTemplateSubGroupID As Object = Nothing, Optional ByRef vIsInternalOnly As Object = Nothing,
                                  Optional ByRef vIsSelectedByDefault As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing,
                                  Optional ByRef vEmailSubTemplateCode As Object = Nothing, Optional ByRef vEmailAttachmentTemplateCode As Object = Nothing, Optional ByRef r_sCCMDocumentName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Set Property values.
            With m_dSIRDocTemplate
                vDocumentTemplateId = .DocumentTemplateId
                vCode = .Code
                vDescription = .Description
                vSourceId = .SourceId
                vDocumentTypeId = .DocumentTypeId
                vCreatedById = .CreatedById
                vDateCreated = .DateCreated
                vModifiedById = .ModifiedById
                vLastModified = .LastModified
                vIsDeleted = .IsDeleted
                vSlotNumber = .SlotNumber
                vRiskCodeId = .RiskCodeId
                vRiskGroupId = .RiskGroupId
                vIsEditableAfterMerging = .IsEditableAfterMerging
                vPrinter = .Printer
                vChaser = .Chaser
                vDocumentFilter = .DocumentFilter
                vCopyOfOriginal = .CopyOfOriginal
                vOriginalDocumentTemplateID = .OriginalDocumentTemplateID
                vEffectiveDate = .EffectiveDate
                vIsVisibleFromWeb = .IsVisibleFromWeb
                vIsVisibleFromClientManager = .IsVisibleFromClientManager
                vArchiveWithNoPrint = .ArchiveWithNoPrint
                vEmailAsBody = .EmailAsBody
                vSpoolDocument = .SpoolDocument
                vTemplateGroupID = .TemplateGroupID
                vTemplateSubGroupID = .TemplateSubGroupID
                vIsInternalOnly = .IsInternalOnly
                vIsSelectedByDefault = .IsSelectedByDefault
                iStatus = m_iDatabaseStatus
                r_sCCMDocumentName = .CCMDocumentName
                vEmailSubTemplateCode = .EmailSubTemplateCode
                vEmailAttachmentTemplateCode = .EmailAttachmentTemplateCode
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

            With m_dSIRDocTemplate

                ' Set Data object primary key
                .DocumentTemplateId = DocumentTemplateId

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

            With m_dSIRDocTemplate

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRDocTemplate Added
                DocumentTemplateId = .DocumentTemplateId

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
        Dim sDocumentDirectory As String = ""
        Dim sClauseDocTemplateDirectory, sOriginalDocTemplateFile As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRDocTemplate

                ' Set Data object primary key
                .DocumentTemplateId = DocumentTemplateId

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If .CopyOfOriginal = 1 Then
                    'We need to delete the copied template

                    'Get the original template file details
                    m_lReturn = CType(GetDocumentDirectory(sDocumentDirectory), gPMConstants.PMEReturnCode)
                    sClauseDocTemplateDirectory = sDocumentDirectory & "\Type " & CStr(.DocumentTypeId)
                    sOriginalDocTemplateFile = sClauseDocTemplateDirectory & "\Doc " & CStr(DocumentTemplateId) & ".zip"

                    'Delete it
                    m_lReturn = CType(DeleteFile(sOriginalDocTemplateFile), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

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

            With m_dSIRDocTemplate

                ' Set Data object primary key
                .DocumentTemplateId = DocumentTemplateId

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

    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing,
                                       Optional ByRef vDocumentTemplateId As Object = Nothing,
                                       Optional ByRef vCode As Object = Nothing,
                                       Optional ByRef vDescription As Object = Nothing, Optional ByRef vSourceId As Object = Nothing,
                                       Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing,
                                       Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing,
                                       Optional ByRef vLastModified As Date = #12/30/1899#, Optional ByRef vIsDeleted As Object = Nothing,
                                       Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vRiskCodeId As Object = Nothing,
                                       Optional ByRef vRiskGroupId As Object = Nothing, Optional ByRef vIsEditableAfterMerging As Object = Nothing,
                                       Optional ByRef vPrinter As Object = Nothing, Optional ByRef vChaser As Object = Nothing,
                                       Optional ByRef vDocumentFilter As Object = Nothing, Optional ByRef vCopyOfOriginal As Object = Nothing,
                                       Optional ByRef vOriginalDocumentTemplateID As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing,
                                       Optional ByRef vIsVisibleFromWeb As Object = Nothing, Optional ByRef vIsVisibleFromClientManager As Object = Nothing,
                                       Optional ByRef vArchiveWithNoPrint As Object = Nothing, Optional ByRef vEmailAsBody As Object = Nothing,
                                       Optional ByRef vSpoolDocument As Object = Nothing, Optional ByRef vArchiveAsText As Object = Nothing,
                                       Optional ByRef vArchiveAsXML As Object = Nothing,
                                       Optional ByRef r_sCCMDocumentName As Object = Nothing) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If (Informations.IsNothing(vDocumentTemplateId)) OrElse (vDocumentTemplateId.Equals(0)) OrElse (bDefaultAll) Then
            vDocumentTemplateId = 0
        End If

        If (Informations.IsNothing(vCode)) OrElse (Object.Equals(vCode, Nothing)) OrElse (bDefaultAll) Then
            vCode = DBNull.Value
        End If

        If (Informations.IsNothing(vDescription)) OrElse (Object.Equals(vDescription, Nothing)) OrElse (bDefaultAll) Then
            vDescription = DBNull.Value
        End If

        If (Informations.IsNothing(vSourceId)) OrElse (vSourceId.Equals(0)) OrElse (bDefaultAll) Then
            vSourceId = m_iSourceID
        End If

        If (Informations.IsNothing(vDocumentTypeId)) OrElse (Object.Equals(vDocumentTypeId, Nothing)) OrElse (bDefaultAll) Then
            vDocumentTypeId = DBNull.Value
        End If

        If (Informations.IsNothing(vCreatedById)) OrElse (vCreatedById.Equals(0)) OrElse (bDefaultAll) Then
            vCreatedById = m_iUserID
        End If

        If (Informations.IsNothing(vDateCreated)) OrElse (vDateCreated.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If

        If (Informations.IsNothing(vModifiedById)) OrElse (vModifiedById.Equals(0)) OrElse (bDefaultAll) Then
            vModifiedById = m_iUserID
        End If

        If (Informations.IsNothing(vLastModified)) OrElse (vLastModified.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vLastModified = DateTime.Now
        End If

        If (Informations.IsNothing(vIsDeleted)) OrElse (vIsDeleted.Equals(0)) OrElse (bDefaultAll) Then
            vIsDeleted = 0
        End If

        If (Informations.IsNothing(vSlotNumber)) OrElse (Object.Equals(vSlotNumber, Nothing)) OrElse (bDefaultAll) Then
            vSlotNumber = DBNull.Value
        End If

        If (Informations.IsNothing(vRiskCodeId)) OrElse (Object.Equals(vRiskCodeId, Nothing)) OrElse (bDefaultAll) Then
            vRiskCodeId = DBNull.Value
        End If

        If (Informations.IsNothing(vRiskGroupId)) OrElse (Object.Equals(vRiskGroupId, Nothing)) OrElse (bDefaultAll) Then
            vRiskGroupId = DBNull.Value
        End If

        If (Informations.IsNothing(vIsEditableAfterMerging)) Then
            vIsEditableAfterMerging = 1
        End If

        If (Informations.IsNothing(vPrinter)) OrElse (String.IsNullOrEmpty(vPrinter)) OrElse (bDefaultAll) Then
            vPrinter = ""
        End If

        If (Informations.IsNothing(vChaser)) OrElse (String.IsNullOrEmpty(vChaser)) OrElse (bDefaultAll) Then
            vChaser = ""
        End If

        If (Informations.IsNothing(vDocumentFilter)) OrElse (String.IsNullOrEmpty(vDocumentFilter)) OrElse (bDefaultAll) Then
            vDocumentFilter = ""
        End If

        If (Informations.IsNothing(vCopyOfOriginal)) OrElse (vCopyOfOriginal.Equals(0)) OrElse (bDefaultAll) Then
            vCopyOfOriginal = 0
        End If

        If (Informations.IsNothing(vOriginalDocumentTemplateID)) OrElse (Object.Equals(vOriginalDocumentTemplateID, Nothing)) OrElse (bDefaultAll) Then
            vOriginalDocumentTemplateID = DBNull.Value
        End If
        If (Informations.IsNothing(vEffectiveDate)) OrElse (Object.Equals(vEffectiveDate, Nothing)) OrElse (bDefaultAll) Then
            vEffectiveDate = DBNull.Value
        End If

        If Informations.IsNothing(vIsVisibleFromWeb) OrElse vIsVisibleFromWeb.Equals(False) OrElse bDefaultAll Then
            vIsVisibleFromWeb = False
        End If

        If Informations.IsNothing(vIsVisibleFromClientManager) OrElse vIsVisibleFromClientManager.Equals(False) OrElse bDefaultAll Then
            vIsVisibleFromClientManager = False
        End If

        If Informations.IsNothing(vArchiveWithNoPrint) OrElse vArchiveWithNoPrint.Equals(False) OrElse bDefaultAll Then
            vArchiveWithNoPrint = False
        End If

        If Informations.IsNothing(vSpoolDocument) OrElse vSpoolDocument.Equals(False) OrElse bDefaultAll Then
            vSpoolDocument = False
        End If

        If Informations.IsNothing(vArchiveAsText) OrElse vArchiveAsText.Equals(False) Then
            vArchiveAsText = False
        End If
        If Informations.IsNothing(vArchiveAsXML) OrElse vArchiveAsXML.Equals(False) Then
            vArchiveAsXML = False
        End If
        If Informations.IsNothing(vEmailAsBody) OrElse vEmailAsBody.Equals(False) OrElse bDefaultAll Then
            vEmailAsBody = False
        End If
        If (Informations.IsNothing(r_sCCMDocumentName)) OrElse (Object.Equals(r_sCCMDocumentName, Nothing)) OrElse r_sCCMDocumentName = "None" OrElse (bDefaultAll) Then
            r_sCCMDocumentName = DBNull.Value
        End If
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRDocTemplate for Consistency.
    '
    ' ***************************************************************** '
    'AK 090402 - added chaser parameter
    Private Function Validate(Optional ByRef vDocumentTemplateId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing, Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vPrinter As Object = Nothing, Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing, Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsVisibleFromWeb As Object = Nothing, Optional ByRef vIsVisibleFromClientManager As Object = Nothing, Optional ByRef vArchiveWithNoPrint As Object = Nothing, Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing, Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing) As Integer ''Saurabh - Added 3 new parameters for Tech Spec PGR005-Automated Emails
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        If Not Informations.IsNothing(vDocumentTemplateId) Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vDocumentTemplateId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
End Class

