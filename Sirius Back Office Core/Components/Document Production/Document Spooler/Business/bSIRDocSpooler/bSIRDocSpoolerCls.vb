Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'Developer Guide No.129
Friend NotInheritable Class SIRDocSpooler
    Implements IDisposable
    '*******************************************************************************
    ' Class Name: SIRDocSpooler
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIRDocSpooler attributes.
    '
    ' Edit History:
    '
    '   PW140105 - PN18078 - changes required for new Document Template ID field
    '              being added to the document_spooler table.
    '*******************************************************************************


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
    Private Const ACClass As String = "SIRDocSpooler"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRDocSpooler As dSIRDocSpooler.SIRDocSpooler
    'Private m_dSIRDocSpooler As dSIRDocSpooler.SIRDocSpooler

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lDocumentSpoolerId As Integer
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

    Public Property DocumentSpoolerId() As Integer
        Get

            Return m_lDocumentSpoolerId

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentSpoolerId = Value

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


            ' Create instance of data class
            m_dSIRDocSpooler = New dSIRDocSpooler.SIRDocSpooler()


            m_lReturn = m_dSIRDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)
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
                If m_dSIRDocSpooler IsNot Nothing Then
                    m_dSIRDocSpooler.Dispose()
                End If
                m_dSIRDocSpooler = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRDocSpooler.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived)

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
    ' Description: Sets the supplied SIRDocSpooler property values.
    '
    ' Hist : TN20010730 - add printer name
    ' ***************************************************************** '
    'DC240603 -ISS4097 -added source id
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing, Optional ByRef vPrinter As Object = Nothing, Optional ByRef vSpoolLevelInd As Object = Nothing, Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTemplateID As Object = Nothing, Optional ByVal v_iIsClient As Integer = 0, Optional ByVal v_iIsAgent As Integer = 0, Optional ByVal v_iIsOffice As Integer = 0, Optional ByVal v_iOrderByProductionOrder As Integer = 0) As Integer



        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'DC240603 -ISS4097 -added source id
                m_lReturn = DefaultParameters(bDefaultAll:=False, vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived, vSourceId:=vSourceId, vPrinter:=vPrinter, vDocumentTemplateID:=vDocumentTemplateID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRDocSpooler


                If Not Informations.IsNothing(vDocumentSpoolerId) Then

                    If .DocumentSpoolerId.Equals(0) Or (.DocumentSpoolerId <> vDocumentSpoolerId) Then
                        .DocumentSpoolerId = vDocumentSpoolerId
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDocumentTypeId) Then



                    'Developer Guide No. 44
                    If Object.Equals(.DocumentTypeId, Nothing) OrElse (Not .DocumentTypeId.Equals(vDocumentTypeId)) Then


                        .DocumentTypeId = vDocumentTypeId
                        bDataChanged = True
                    End If
                End If

                'TN20010730 - start

                If Not Informations.IsNothing(vPrinter) Then



                    'Developer Guide No. 44
                    If Object.Equals(.Printer, Nothing) OrElse Not .Printer.Equals(vPrinter) Then


                        .Printer = vPrinter
                        bDataChanged = True
                    End If
                End If
                'TN20010730 - end

                'sj 15/10/2002 - start

                If Not Informations.IsNothing(vSpoolLevelInd) Then



                    'Developer Guide No. 44
                    If Object.Equals(.SpoolLevelInd, Nothing) OrElse Not .SpoolLevelInd.Equals(vSpoolLevelInd) Then

                        .SpoolLevelInd = vSpoolLevelInd
                        bDataChanged = True
                    End If
                End If
                'sj 15/10/2002 - end


                If Not Informations.IsNothing(vPartyCnt) Then



                    'Developer Guide No. 44
                    If Object.Equals(.PartyCnt, Nothing) OrElse (Not .PartyCnt.Equals(vPartyCnt)) Then


                        .PartyCnt = vPartyCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vInsuranceFolderCnt) Then



                    'Developer Guide No. 44
                    If Object.Equals(.InsuranceFolderCnt, Nothing) OrElse (Not .InsuranceFolderCnt.Equals(vInsuranceFolderCnt)) Then

                        .InsuranceFolderCnt = vInsuranceFolderCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vInsuranceFileCnt) Then



                    'Developer Guide No. 44
                    If Object.Equals(.InsuranceFileCnt, Nothing) OrElse (Not .InsuranceFileCnt.Equals(vInsuranceFileCnt)) Then

                        .InsuranceFileCnt = vInsuranceFileCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vClaimCnt) Then



                    'Developer Guide No. 44
                    If Object.Equals(.ClaimCnt, Nothing) OrElse (Not .ClaimCnt.Equals(vClaimCnt)) Then

                        .ClaimCnt = vClaimCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDescription) Then

                    'Developer Guide No. 44
                    If String.IsNullOrEmpty(.Description) OrElse (.Description.Trim() <> vDescription.Trim()) Then
                        .Description = vDescription
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vIsDeletable) Then

                    'Developer Guide No. 44
                    If .IsDeletable.Equals(0) OrElse (.IsDeletable <> vIsDeletable) Then
                        .IsDeletable = vIsDeletable
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vIsEditable) Then

                    If .IsEditable.Equals(0) OrElse (.IsEditable <> vIsEditable) Then
                        .IsEditable = vIsEditable
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vCreatedById) Then

                    If .CreatedById.Equals(0) OrElse (.CreatedById <> vCreatedById) Then
                        .CreatedById = vCreatedById
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDateCreated) Then

                    If .DateCreated.Equals(DateTime.FromOADate(0)) OrElse (.DateCreated <> vDateCreated) Then
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

                    If .ModifiedById.Equals(0) OrElse (.ModifiedById <> vModifiedById) Then
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


                If Not Informations.IsNothing(vTimesPrinted) Then

                    If .TimesPrinted.Equals(0) OrElse (.TimesPrinted <> vTimesPrinted) Then
                        .TimesPrinted = vTimesPrinted
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vTimesArchived) Then

                    If .TimesArchived.Equals(0) OrElse (.TimesArchived <> vTimesArchived) Then
                        .TimesArchived = vTimesArchived
                        bDataChanged = True
                    End If
                End If

                'DC240603 -ISS4097

                If Not Informations.IsNothing(vSourceId) Then

                    If .SourceId.Equals(0) OrElse (.SourceId <> vSourceId) Then
                        .SourceId = vSourceId
                        bDataChanged = True
                    End If
                End If


                ' PN18078.

                If Not Informations.IsNothing(vDocumentTemplateID) Then

                    If .DocumentTemplateID.Equals(0) OrElse (.DocumentTemplateID <> vDocumentTemplateID) Then
                        .DocumentTemplateID = vDocumentTemplateID
                        bDataChanged = True
                    End If
                End If

                'Renewal Printing
                .IsClient = v_iIsClient
                .IsAgent = v_iIsAgent
                .IsOffice = v_iIsOffice
                .ProductionOrder = v_iOrderByProductionOrder

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
    ' Description: Returns the supplied SIRDocSpooler property values.
    '
    ' Hist : TN20010730 - add printer name
    ' ***************************************************************** '
    'DC240603 -ISS4097 -added new parameter source id
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing, Optional ByRef vPrinter As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRDocSpooler

                'developer guide no.118
                'start
                'If Not Informations.IsNothing(vDocumentSpoolerId) Then
                vDocumentSpoolerId = .DocumentSpoolerId
                'End If


                'If Not Informations.IsNothing(vDocumentTypeId) Then


                vDocumentTypeId = .DocumentTypeId
                'End If


                'If Not Informations.IsNothing(vPartyCnt) Then


                vPartyCnt = .PartyCnt
                'End If


                'If Not Informations.IsNothing(vInsuranceFolderCnt) Then


                vInsuranceFolderCnt = .InsuranceFolderCnt
                'End If


                'If Not Informations.IsNothing(vInsuranceFileCnt) Then


                vInsuranceFileCnt = .InsuranceFileCnt
                'End If


                'If Not Informations.IsNothing(vClaimCnt) Then


                vClaimCnt = .ClaimCnt
                'End If


                'If Not Informations.IsNothing(vDescription) Then
                vDescription = .Description
                'End If


                'If Not Informations.IsNothing(vIsDeletable) Then
                vIsDeletable = .IsDeletable
                'End If


                'If Not Informations.IsNothing(vIsEditable) Then
                vIsEditable = .IsEditable
                'End If


                'If Not Informations.IsNothing(vCreatedById) Then
                vCreatedById = .CreatedById
                ' End If


                'If Not Informations.IsNothing(vDateCreated) Then
                vDateCreated = .DateCreated
                'End If


                'If Not Informations.IsNothing(vModifiedById) Then
                vModifiedById = .ModifiedById
                'End If


                'If Not Informations.IsNothing(vLastModified) Then
                vLastModified = .LastModified
                'End If


                'If Not Informations.IsNothing(vTimesPrinted) Then
                vTimesPrinted = .TimesPrinted
                'End If


                'If Not Informations.IsNothing(vTimesArchived) Then
                vTimesArchived = .TimesArchived
                'End If

                'TN20010730 - start

                'If Not Informations.IsNothing(vPrinter) Then


                vPrinter = .Printer
                'End If
                'end
                'TN20010730 - end

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

            With m_dSIRDocSpooler

                ' Set Data object primary key
                .DocumentSpoolerId = DocumentSpoolerId

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

            With m_dSIRDocSpooler

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRDocSpooler Added
                DocumentSpoolerId = .DocumentSpoolerId

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

            With m_dSIRDocSpooler

                ' Set Data object primary key
                'No, don't.  It was set a while ago in SetProperties
                '        .DocumentSpoolerId = DocumentSpoolerId

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

            With m_dSIRDocSpooler

                ' Set Data object primary key
                .DocumentSpoolerId = DocumentSpoolerId

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
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRDocSpooler.
    '
    ' Hist : TN20010730 - add printer name
    ' ***************************************************************** '
    'DC240603 -ISS4097 -added source id
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing, Optional ByRef vPrinter As Object = Nothing, Optional ByRef vSpoolLevelInd As Object = Nothing, Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTemplateID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vDocumentSpoolerId)) Or (bDefaultAll) Then
            vDocumentSpoolerId = 0
        End If



        If (Informations.IsNothing(vDocumentTypeId)) Or (bDefaultAll) Then
            vDocumentTypeId = DBNull.Value
        End If



        If (Informations.IsNothing(vPartyCnt)) Or (bDefaultAll) Then
            vPartyCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vInsuranceFolderCnt)) Or (bDefaultAll) Then
            vInsuranceFolderCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vInsuranceFileCnt)) Or (bDefaultAll) Then
            vInsuranceFileCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vClaimCnt)) Or (bDefaultAll) Then
            vClaimCnt = DBNull.Value
        End If



        If (Informations.IsNothing(vDescription)) Or (bDefaultAll) Then
            vDescription = DBNull.Value
        End If



        If (Informations.IsNothing(vIsDeletable)) Or (bDefaultAll) Then
            vIsDeletable = 0
        End If



        If (Informations.IsNothing(vIsEditable)) Or (bDefaultAll) Then
            vIsEditable = 0
        End If



        If (Informations.IsNothing(vCreatedById)) Or (bDefaultAll) Then
            vCreatedById = m_iUserID
        End If



        If (Informations.IsNothing(vDateCreated)) Or vDateCreated = DateTime.MinValue Or (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If



        If (Informations.IsNothing(vModifiedById)) Or (bDefaultAll) Then
            vModifiedById = m_iUserID
        End If



        If (Informations.IsNothing(vLastModified)) Or vLastModified = DateTime.MinValue Or (bDefaultAll) Then
            vLastModified = DateTime.Now
        End If



        If (Informations.IsNothing(vTimesPrinted)) Or (bDefaultAll) Then
            vTimesPrinted = 0
        End If



        If (Informations.IsNothing(vTimesArchived)) Or (bDefaultAll) Then
            vTimesArchived = 0
        End If

        'TN20010730 - start


        If Informations.IsNothing(vPrinter) Then
            vPrinter = DBNull.Value
        End If
        'TN20010730 - end

        'sj 15/10/2002 - start


        If Informations.IsNothing(vSpoolLevelInd) Then
            vSpoolLevelInd = DBNull.Value
        End If
        'sj 15/10/2002 - end

        'DC240603 -ISS4097


        If (Informations.IsNothing(vSourceId)) Or (bDefaultAll) Then
            vSourceId = m_iSourceID
        End If

        ' PN18078.


        If (Informations.IsNothing(vDocumentTemplateID)) Or (bDefaultAll) Then
            vDocumentTemplateID = DBNull.Value
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRDocSpooler for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vDocumentSpoolerId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vDocumentSpoolerId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
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

