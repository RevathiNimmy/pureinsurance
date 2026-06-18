Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.IO
'Imports System.Net
'Imports System.Net.Mail
Imports System.Runtime.InteropServices.ComTypes
'Imports System.Net.Mail
Imports System.Security.Cryptography
Imports Aspose.Email
Imports Aspose.Email.Clients
Imports SSP.Shared
Imports LinkedResource =Aspose.Email.LinkedResource ' System.Net.Mail.LinkedResource
Imports MailMessage = Aspose.Email.MailMessage

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRDocTemplate.
    '
    ' Edit History:
    ' SJP14062002 moved to uniform Product Options scheme and gSIRLibrary.bas
    ' RKS 02/05/2005 Added DocumentFilter
    ' RKS 03/05/2005 Added CopyOfOriginal & OriginalDocumentTemplateID
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

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
    Private m_oWord As Object
    Private m_lWordHwnd As Long
    Private m_sWordVersion As String
    Dim m_oSiriusDocumentUtility As Object
    ' ************************************************
    ' Collection of SIRDocTemplates (Private)
    Private m_oSIRDocTemplates As bSIRDocTemplate.SIRDocTemplates
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Error Code (Private)
    Private m_lReturn As Integer
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private lPMAuthorityLevel As Integer
    ' Primary Keys to work with
    Private m_lDocumentTemplateId As Integer
    Private m_lDocumentTypeId As Integer
    Private m_vRiskLinkArray(,) As Object 'RWH(21/08/2000) - RSAIB Process 12
    Private m_vClauseArray() As Object 'RWH(21/08/2000) - RSAIB Process 12
    Private Const lCLAUSE_DOC_ID As Integer = 7 'RWH(21/08/2000) - RSAIB Process 12
    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    ' PM System Option Business Component (Private)
    Private m_oSystemOption As bSIROptions.Business
    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business
    Private m_sUnderwritingOrAgency As String = ""
    Private m_sEventDescription As String = ""
    Private m_sSMTPEmailFrom As String = String.Empty
    Private m_bIsNonBatchProcess As Boolean
    <ThreadStatic()> Private m_bIsCalledFromBatchProcess As Boolean

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            Value = Value
        End Set
    End Property
    Public Property CurrentRecord() As Integer
        Get
            Return m_lCurrentRecord
        End Get
        Set(ByVal Value As Integer)
            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRDocTemplates.Count()
                    m_lCurrentRecord = m_oSIRDocTemplates.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property
    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oSIRDocTemplates.Count()
        End Get
    End Property
    Public Property DocumentTemplateId() As Integer
        Get
            Return m_lDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTemplateId = Value
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
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If
            Return m_sUnderwritingOrAgency
        End Get
    End Property
    'sj 26/06/2002 - start
    Public ReadOnly Property ChaserLettersEnabled() As Boolean
        Get
            Dim vValue As String = ""

            m_lReturn = GetChaserLettersEnabled(r_vValue:=vValue)
            Return Val(vValue) = 1

        End Get
    End Property
    'sj 26/06/2002 - end
    Public Property EventDescription() As String
        Get
            Return m_sEventDescription
        End Get
        Set(ByVal Value As String)
            m_sEventDescription = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property IsNonBatchProcess() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsNonBatchProcess = Value
        End Set
    End Property

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

            m_lReturn = dPMDAO.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRDocTemplates Collection
            m_oSIRDocTemplates = New bSIRDocTemplate.SIRDocTemplates()

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
    Private Function ConvertDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sDestDocument As String) As Integer
        Dim result As Integer = 0
        ' Dim SiriusDocumentUtility As Object
        Const kMethodName As String = "ConvertDocumentUsingSiriusDocumentUtility"
        Dim oConvert As SiriusDocumentUtility.Document


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()

            oConvert.Convert(v_sSourceDocument, v_sDestDocument)


        Catch ex As Exception

            If m_bIsCalledFromBatchProcess = True Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)
            Else
                bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="")
            End If

        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '        
            '        Return result
        End Try
        Return result
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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                If m_oSystemOption IsNot Nothing Then
                    m_oSystemOption.Dispose()
                    m_oSystemOption = Nothing
                End If
                If Not m_oSIRDocTemplates Is Nothing Then
                    m_oSIRDocTemplates = Nothing
                End If

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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRDocTemplate.
    '
    '
    ' ***************************************************************** '

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 2) As Object
        Dim vDocumentTypeId As Object = Nothing
        Dim vRiskCodeId As Object = Nothing
        Dim vRiskGroupId As Object = Nothing
        ' {* USER DEFINED CODE (End) *}

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array

            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "document_type"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "risk_code"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = "risk_group"

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRDocTemplate = m_oSIRDocTemplates.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key
                    'But here we are now passing the type...
                    If DocumentTypeId = 0 Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    Else

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = DocumentTypeId
                    End If

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oSIRDocTemplate

                        ' {* USER DEFINED CODE (Begin) *}


                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vDocumentTypeId:=vDocumentTypeId, vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vDocumentTypeId


                        If Convert.IsDBNull(vRiskCodeId) Or Informations.IsNothing(vRiskCodeId) Then

                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = 0
                        Else


                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vRiskCodeId
                        End If


                        If Convert.IsDBNull(vRiskGroupId) Or Informations.IsNothing(vRiskGroupId) Then

                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = 0
                        Else


                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vRiskGroupId
                        End If
                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRDocTemplate



                        m_lReturn = .GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vDocumentTypeId:=vDocumentTypeId, vRiskCodeId:=vRiskCodeId, vRiskGroupId:=vRiskGroupId)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vDocumentTypeId


                        If Convert.IsDBNull(vRiskCodeId) Or Informations.IsNothing(vRiskCodeId) Then

                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = 0
                        Else


                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vRiskCodeId
                        End If


                        If Convert.IsDBNull(vRiskGroupId) Or Informations.IsNothing(vRiskGroupId) Then

                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = 0
                        Else


                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vRiskGroupId
                        End If

                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRDocTemplate reference
            oSIRDocTemplate = Nothing

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

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRDocTemplate directly into the database.
    '        Note: The SIRDocTemplate will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vDocumentTemplateId As Object = Nothing,
                            Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                            Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                            Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                            Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                            Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                            Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                            Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vRiskLinkArray(,) As Object = Nothing,
                            Optional ByRef vClauseArray() As Object = Nothing, Optional ByRef vPrinter As Object = Nothing,
                            Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing,
                            Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing,
                            Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsVisibleFromWeb As Boolean = False,
                            Optional ByRef vIsVisibleFromClientManager As Boolean = False, Optional ByRef vArchiveWithNoPrint As Object = Nothing,
                            Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing,
                            Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vTemplateGroupID As Object = Nothing,
                            Optional ByRef vTemplateSubGroupID As Object = Nothing, Optional ByRef vIsSelectedByDefault As Object = Nothing,
                            Optional ByRef vInternalOnly As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing,
                            Optional ByRef vEmailSubTemplateCode As Object = Nothing, Optional ByRef vEmailAttachmentTemplateCode As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRDocTemplate
            oSIRDocTemplate = New bSIRDocTemplate.SIRDocTemplate()
            m_lReturn = oSIRDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            ' Populate SIRDocTemplate Attributes
            m_lReturn = oSIRDocTemplate.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd,
                                                      vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode,
                                                      vDescription:=vDescription, vSourceId:=vSourceId,
                                                      vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById,
                                                      vDateCreated:=vDateCreated, vModifiedById:=vModifiedById,
                                                      vLastModified:=vLastModified, vIsDeleted:=vIsDeleted,
                                                      vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId,
                                                      vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging,
                                                      vPrinter:=vPrinter, vChaser:=vChaser, vDocumentFilter:=vDocumentFilter,
                                                      vCopyOfOriginal:=vCopyOfOriginal, vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID,
                                                      vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb,
                                                      vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint,
                                                      vEmailAsBody:=vEmailAsBody, vSpoolDocument:=vSpoolDocument,
                                                      vArchiveAsText:=vArchiveAsText, vTemplateGroupID:=vTemplateGroupID,
                                                      vTemplateSubGroupID:=vTemplateSubGroupID, vIsInternalOnly:=vInternalOnly,
                                                      vIsSelectedByDefault:=vIsSelectedByDefault, vArchiveAsXML:=vArchiveAsXML,
                                                      vEmailSubTemplateCode:=vEmailSubTemplateCode, vEmailAttachmentTemplateCode:=vEmailAttachmentTemplateCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocTemplate = Nothing
                Return result
            End If

            ' Add the SIRDocTemplate to the Database
            m_lReturn = oSIRDocTemplate.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocTemplate = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRDocTemplate Added
            With oSIRDocTemplate
                DocumentTemplateId = .DocumentTemplateId
            End With

            vDocumentTemplateId = DocumentTemplateId

            oSIRDocTemplate = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRDocTemplate directly from the database.
    '        Note: The SIRDocTemplate will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vDocumentTemplateId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRDocTemplate
            oSIRDocTemplate = New bSIRDocTemplate.SIRDocTemplate()
            m_lReturn = oSIRDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Set SIRDocTemplate Primary Key


            m_lReturn = oSIRDocTemplate.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vDocumentTemplateId:=vDocumentTemplateId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocTemplate = Nothing
                Return result
            End If

            ' Delete the SIRDocTemplate from the Database
            m_lReturn = oSIRDocTemplate.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocTemplate = Nothing
                Return result
            End If

            oSIRDocTemplate = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Check whether given code already exists in document template table
    ''' </summary>
    ''' <param name="v_sDocCode"></param>
    ''' <param name="r_bDocCodeExists"></param>
    ''' <returns></returns>
    ''' <remarks>WPR 3</remarks>
    Public Function ValidateDocumentCode(ByVal v_sDocCode As String, ByRef r_bDocCodeExists As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            If v_sDocCode IsNot Nothing AndAlso Not String.IsNullOrEmpty(v_sDocCode) Then
                sSQL = "Select document_template_id from document_template where code = '" & v_sDocCode.Trim() & "'"
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ValidateDocumentCode", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bDocCodeExists = Informations.IsArray(vArray)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateDocumentCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateDocumentCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        Finally
            vArray = Nothing
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
    ' Name: CheckCode (Public)
    '
    ' Description: Checks to see if the supplied code already exists.
    '
    ' ***************************************************************** '
    Public Function CheckCode(ByRef vCode As Object, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#) As Integer
        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckCodeSQL, sSQLName:=ACCheckCodeName, bStoredProcedure:=ACCheckCodeStored, lNumberRecords:=0)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRDocTemplates and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Integer = 0, Optional ByRef vDocumentTemplateId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Dim oFields As DataRow
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate
        Dim sSQL As String = ""
        Dim bDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRDocTemplates.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = 0
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vDocumentTemplateId)) And (Not Double.TryParse(CStr(vDocumentTemplateId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vDocumentTemplateId=" & vDocumentTemplateId, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vDocumentTemplateId) Then

                ' Create New SIRDocTemplate
                oSIRDocTemplate = New bSIRDocTemplate.SIRDocTemplate()
                m_lReturn = oSIRDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oSIRDocTemplate
                    .DocumentTemplateId = vDocumentTemplateId

                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRDocTemplate to collection
                If m_oSIRDocTemplates.Count = 0 Then
                    m_oSIRDocTemplates.Add(Nothing)
                End If
                m_lReturn = m_oSIRDocTemplates.Add(oNewSIRDocTemplate:=oSIRDocTemplate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRDocTemplate = Nothing

            Else

                ' No Key, Get All Records for the parameters passed
                sSQL = "SELECT document_template_id FROM document_template"
                bDone = False

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

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
                    oSIRDocTemplate = New bSIRDocTemplate.SIRDocTemplate()
                    m_lReturn = Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record

                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRDocTemplate
                        .DocumentTemplateId = gPMFunctions.NullToLong(oFields("document_template_id"))

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRDocTemplate to collection
                    If m_oSIRDocTemplates.Count = 0 Then
                        m_oSIRDocTemplates.Add(Nothing)
                    End If
                    m_lReturn = m_oSIRDocTemplates.Add(oNewSIRDocTemplate:=oSIRDocTemplate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRDocTemplate = Nothing
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
    ' Name: GetOtherDetails (Public)
    '
    ' Description: Gets the required information about slots...
    '
    ' ***************************************************************** '
    'MKW020503 Added parameter for retrieving ClaimTextFileSlots (optional).

    Public Function GetOtherDetails(ByRef vClientArray() As Object, ByRef vPolicyArray() As Object, ByRef vDocumentTypeArray(,) As Object, ByRef vSourceArray(,) As Object, ByRef vRiskBySource(,) As Object, ByRef vRiskGroupBySource(,) As Object, Optional ByRef vClaimArray() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lSourceId As Integer
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT document_type_id, is_editable_after_merging " &
                   "FROM document_type "

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetEditable", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vDocumentTypeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get those sources ID's that are accessible by logged in user
            sSQL = "SELECT source_id, code, description" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM source" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND source_id NOT IN" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "(SELECT source_id FROM pmuser_source" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE [user_id] = " & CStr(m_iUserID) & ")" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "UNION" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "SELECT 0, '(All)', 'All branches'" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "ORDER BY source_id" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSources", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSourceArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'This is pants.  In the interface we use the source id as the index for this array,
            'which goes berserk once a source has been deleted...
            ReDim vRiskBySource(1, vSourceArray.GetUpperBound(1))
            ReDim vRiskGroupBySource(1, vSourceArray.GetUpperBound(1))
            ReDim vClientArray(vSourceArray.GetUpperBound(1)) 'CT 21/12/00
            ReDim vPolicyArray(vSourceArray.GetUpperBound(1)) 'CT 21/12/00
            ReDim vClaimArray(vSourceArray.GetUpperBound(1))

            For lTemp As Integer = vSourceArray.GetLowerBound(1) To vSourceArray.GetUpperBound(1)


                lSourceId = CInt(vSourceArray(0, lTemp))


                vRiskBySource(0, lTemp) = lSourceId

                vRiskGroupBySource(0, lTemp) = lSourceId

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(lSourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskCodeSQL, sSQLName:=ACGetRiskCodeName, bStoredProcedure:=ACGetRiskCodeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                vRiskBySource(1, lTemp) = vArray

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(lSourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskGroupSQL, sSQLName:=ACGetRiskGroupName, bStoredProcedure:=ACGetRiskGroupStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDatabase.Parameters.Clear()

                '        vRiskGroupBySource(lTemp) = vArray

                vRiskGroupBySource(1, lTemp) = vArray

                sSQL = "SELECT tfd.slot_number, " &
                       "tfd.description " &
                       "FROM text_file_description tfd, " &
                       "entity_type et " &
                       "WHERE tfd.entity_type_id = et.entity_type_id " &
                       "AND et.code = 'CLIENT' " &
                       "AND source_id IN (0," & CStr(lSourceId) & ")"

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetClientTexts", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                vClientArray(lTemp) = vArray

                ' MKW020603 PN4432 - Code Change to stored procedures END

                sSQL = "SELECT tfd.slot_number, " &
                       "tfd.description " &
                       "FROM text_file_description tfd, " &
                       "entity_type et " &
                       "WHERE tfd.entity_type_id = et.entity_type_id " &
                       "AND et.code = 'POLICY' " &
                       "AND source_id IN (0," & CStr(lSourceId) & ")"

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyTexts", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                vPolicyArray(lTemp) = vArray

                'MKW020503 PN3890 START Retrieve Claim Text File Slots

                If Not (Informations.IsNothing(vClaimArray)) Then
                    sSQL = "SELECT tfd.slot_number, " &
                           "tfd.description " &
                           "FROM text_file_description tfd, " &
                           "entity_type et " &
                           "WHERE tfd.entity_type_id = et.entity_type_id " &
                           "AND et.code = 'CLAIM' " &
                           "AND source_id IN (0," & CStr(lSourceId) & ")"

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetClaimTexts", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    vClaimArray(lTemp) = vArray
                End If
                'MKW020503 PN3890 END

            Next lTemp

            vArray = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDescription (Public)
    '
    ' Description: Gets the description for the template...
    '
    ' ***************************************************************** '
    Public Function GetDescription(ByRef lDocumentTemplateId As Integer, ByRef sDocumentTemplateDescription As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT description " &
                   "FROM document_template " &
                   "WHERE document_template_id = " & CStr(lDocumentTemplateId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDocumentTemplateDescription = ""

            If Informations.IsArray(vArray) Then

                sDocumentTemplateDescription = CStr(vArray(0, 0)).Trim()
                vArray = Nothing
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDescription Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescription", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetIsEditable (Public)
    '
    ' Description: Gets the description for the template...
    '
    ' DC201004 PN15952 get iseditable flag for particular document template
    ' ***************************************************************** '
    Public Function GetIsEditable(ByVal v_lDocTemplateId As Integer, ByRef r_iIsEditable As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT is_editable_after_merging " &
                   "FROM document_template " &
                   "WHERE document_template_id = " & CStr(v_lDocTemplateId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetIsEditable", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_iIsEditable = 0

            If Informations.IsArray(vArray) Then

                r_iIsEditable = CInt(CStr(vArray(0, 0)).Trim())
                vArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIsEditable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIsEditable", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInformation (Public)
    '
    ' Description: Gets the information for party, policy and claim
    '
    ' DC290501
    ' KB 07012003 Added lInsuranceFileCnt to enable documents via task to go to the
    '                     correct place
    ' ***************************************************************** '
    Public Function GetInformation(ByRef lPartyCnt As Integer, ByRef lInsuranceFolderCnt As Integer, ByRef lClaimCnt As Integer, ByRef sPartyName As String, ByRef sInsuranceFileRef As String, ByRef sClaimRef As String, ByRef lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sPartyName = ""

            If lPartyCnt <> 0 Then

                sSQL = "SELECT resolved_name " &
                       "FROM party " &
                       "WHERE party_cnt = " & CStr(lPartyCnt)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Information For Party", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vArray) Then

                    sPartyName = CStr(vArray(0, 0)).Trim()
                    vArray = Nothing
                End If

            End If

            sInsuranceFileRef = ""

            If lInsuranceFolderCnt <> 0 Then
                ' MS180601 - Not sure how this ever worked. The SQL Statement had a syntax error
                ' (no space between the fields and the Where clause....also 'if' is a reserved word in later SQL
                ' and cannot be used as a table alias. Field names seem different too, so I am
                ' leaving it here just in case we are meant to be getting a different field than the insurance_ref
                sSQL = "SELECT i.insurance_ref " &
                       "FROM insurance_file i, insurance_folder ip " &
                       "WHERE ip.insurance_folder_cnt = " & CStr(lInsuranceFolderCnt) &
                       " AND i.insurance_folder_cnt = ip.insurance_folder_cnt"

                If lInsuranceFileCnt <> 0 Then
                    sSQL = sSQL & " AND i.insurance_file_cnt = " & CStr(lInsuranceFileCnt)
                End If

                'MS End
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Information For Policy", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vArray) Then

                    sInsuranceFileRef = CStr(vArray(0, 0)).Trim()
                    vArray = Nothing
                End If

            Else

                'KB07012003   if we are archiving a document from a task but at policy level we dont have
                ' the insurance folder count but we do have the insurance file count
                ' use this to get the insurnace folder cnt and then the document will be archived in the
                ' correct place
                ' Only do this if not via claims - just in case

                If (lInsuranceFileCnt <> 0) And (lClaimCnt = 0) Then
                    sSQL = "Select i.insurance_folder_cnt " &
                           "FROM insurance_file i " &
                           "WHERE i.insurance_file_cnt = " & CStr(lInsuranceFileCnt)

                    'Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Information for Policy", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Informations.IsArray(vArray) Then

                        lInsuranceFolderCnt = CInt(CStr(vArray(0, 0)).Trim())
                        vArray = Nothing
                    End If
                End If
            End If

            sClaimRef = ""

            If lClaimCnt <> 0 Then

                sSQL = "SELECT claim_number " &
                       "FROM claim " &
                       "WHERE claim_id = " & CStr(lClaimCnt)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vArray) Then

                    sClaimRef = CStr(vArray(0, 0)).Trim()
                    vArray = Nothing
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInformation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInformation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRDocTemplates and populate the Collection
    '
    ' ***************************************************************** '
    'AK 090402 - added another parameter for chasers
    Public Function GetNext(Optional ByRef vDocumentTemplateId As Object = Nothing,
                            Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                            Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                            Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                            Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                            Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                            Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                            Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vRiskLinkArray(,) As Object = Nothing,
                            Optional ByRef vClauseArray() As Object = Nothing, Optional ByRef vPrinter As Object = Nothing,
                            Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing,
                            Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing,
                            Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsVisibleFromWeb As Boolean = False,
                            Optional ByRef vIsVisibleFromClientManager As Boolean = False, Optional ByRef vArchiveWithNoPrint As Object = Nothing,
                            Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing,
                            Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vTemplateGroupID As Object = Nothing,
                            Optional ByRef vTemplateSubGroupID As Object = Nothing, Optional ByRef vIsSelectedByDefault As Object = Nothing,
                            Optional ByRef vIsInternalOnly As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing,
                            Optional ByRef vEmailSubTemplateCode As Object = Nothing, Optional ByRef vEmailAttachmentTemplateCode As Object = Nothing, Optional ByRef r_sCCMDocumentName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRDocTemplates.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If
            oSIRDocTemplate = New bSIRDocTemplate.SIRDocTemplate()
            oSIRDocTemplate = m_oSIRDocTemplates.Item(m_lCurrentRecord)

            ' Get the SIRDocTemplate Property Values
            m_lReturn = oSIRDocTemplate.GetProperties(iStatus, vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode,
                                                      vDescription:=vDescription, vSourceId:=vSourceId,
                                                      vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById,
                                                      vDateCreated:=vDateCreated, vModifiedById:=vModifiedById,
                                                      vLastModified:=vLastModified, vIsDeleted:=vIsDeleted,
                                                      vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId,
                                                      vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging,
                                                      vPrinter:=vPrinter, vChaser:=vChaser, vDocumentFilter:=vDocumentFilter,
                                                      vCopyOfOriginal:=vCopyOfOriginal, vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID,
                                                      vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb,
                                                      vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint,
                                                      vEmailAsBody:=vEmailAsBody, vSpoolDocument:=vSpoolDocument,
                                                      vArchiveAsText:=vArchiveAsText, vTemplateGroupID:=vTemplateGroupID,
                                                      vTemplateSubGroupID:=vTemplateSubGroupID, vIsInternalOnly:=vIsInternalOnly,
                                                      vIsSelectedByDefault:=vIsSelectedByDefault, vArchiveAsXML:=vArchiveAsXML,
                                                      vEmailSubTemplateCode:=vEmailSubTemplateCode, vEmailAttachmentTemplateCode:=vEmailAttachmentTemplateCode, r_sCCMDocumentName:=r_sCCMDocumentName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = GetRiskClauseLinks(vDocumentTemplateId, vRiskLinkArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            oSIRDocTemplate = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRDocTemplate into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    '           RWH(26/07/01) Added printer.
    '           AK 090402 - added another parameter for chasers
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vDocumentTemplateId As Object = Nothing,
                            Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                            Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                            Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                            Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                            Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                            Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                            Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vRiskLinkArray(,) As Object = Nothing,
                            Optional ByRef vClauseArray() As Object = Nothing, Optional ByRef vPrinter As Object = Nothing,
                            Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing,
                            Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing,
                            Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsVisibleFromWeb As Boolean = False,
                            Optional ByRef vIsVisibleFromClientManager As Boolean = False, Optional ByRef vArchiveWithNoPrint As Object = Nothing,
                            Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing,
                            Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vTemplateGroupID As Object = Nothing,
                            Optional ByRef vTemplateSubGroupID As Object = Nothing, Optional ByRef vIsSelectedByDefault As Object = Nothing,
                            Optional ByRef vIsInternalOnly As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing,
                            Optional ByRef vEmailSubTemplateCode As Object = Nothing, Optional ByRef vEmailAttachmentTemplateCode As Object = Nothing, Optional ByRef r_sCCMDocumentName As Object = Nothing, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(21/08/2000) RSAIB Process 12.
            If m_lDocumentTypeId = lCLAUSE_DOC_ID Then
                m_vRiskLinkArray = vRiskLinkArray
            End If
            m_vClauseArray = vClauseArray

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRDocTemplates.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRDocTemplate
            oSIRDocTemplate = New bSIRDocTemplate.SIRDocTemplate()
            m_lReturn = oSIRDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            m_lReturn = oSIRDocTemplate.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd,
                                                      vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode,
                                                      vDescription:=vDescription, vSourceId:=vSourceId,
                                                      vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById,
                                                      vDateCreated:=vDateCreated, vModifiedById:=vModifiedById,
                                                      vLastModified:=vLastModified, vIsDeleted:=vIsDeleted,
                                                      vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId,
                                                      vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging,
                                                      vPrinter:=vPrinter, vChaser:=vChaser, vDocumentFilter:=vDocumentFilter,
                                                      vCopyOfOriginal:=vCopyOfOriginal, vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID,
                                                      vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb,
                                                      vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint,
                                                      vEmailAsBody:=vEmailAsBody, vSpoolDocument:=vSpoolDocument,
                                                      vArchiveAsText:=vArchiveAsText, vTemplateGroupID:=vTemplateGroupID,
                                                      vTemplateSubGroupID:=vTemplateSubGroupID, vIsInternalOnly:=vIsInternalOnly,
                                                      vIsSelectedByDefault:=vIsSelectedByDefault, vArchiveAsXML:=vArchiveAsXML,
                                                      vEmailSubTemplateCode:=vEmailSubTemplateCode, vEmailAttachmentTemplateCode:=vEmailAttachmentTemplateCode, r_sCCMDocumentName:=r_sCCMDocumentName, v_sUniqueId:=v_sUniqueId, v_sScreenHierarchy:=v_sScreenHierarchy)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRDocTemplate = Nothing
                Return result
            End If

            ' Add SIRDocTemplate to collection
            If m_oSIRDocTemplates.Count = 0 Then
                m_oSIRDocTemplates.Add(Nothing)
            End If
            m_lReturn = m_oSIRDocTemplates.Add(oNewSIRDocTemplate:=oSIRDocTemplate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocTemplate = Nothing
                Return result
            End If

            oSIRDocTemplate = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRDocTemplate
    '              specified and updates the SIRDocTemplate with the new values.
    '
    ' ***************************************************************** '
    'AK 090402 - added another parameter for chasers
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vDocumentTemplateId As Object = Nothing,
                            Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing,
                            Optional ByRef vSourceId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing,
                            Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing,
                            Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                            Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing,
                            Optional ByRef vRiskCodeId As Object = Nothing, Optional ByRef vRiskGroupId As Object = Nothing,
                            Optional ByRef vIsEditableAfterMerging As Object = Nothing, Optional ByRef vRiskLinkArray(,) As Object = Nothing,
                            Optional ByRef vClauseArray() As Object = Nothing, Optional ByRef vPrinter As Object = Nothing,
                            Optional ByRef vChaser As Object = Nothing, Optional ByRef vDocumentFilter As Object = Nothing,
                            Optional ByRef vCopyOfOriginal As Object = Nothing, Optional ByRef vOriginalDocumentTemplateID As Object = Nothing,
                            Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsVisibleFromWeb As Boolean = False,
                            Optional ByRef vIsVisibleFromClientManager As Boolean = False, Optional ByRef vArchiveWithNoPrint As Object = Nothing,
                            Optional ByRef vEmailAsBody As Object = Nothing, Optional ByRef vSpoolDocument As Object = Nothing,
                            Optional ByRef vArchiveAsText As Object = Nothing, Optional ByRef vTemplateGroupID As Object = Nothing,
                            Optional ByRef vTemplateSubGroupID As Object = Nothing, Optional ByRef vIsSelectedByDefault As Object = Nothing,
                            Optional ByRef vIsInternalOnly As Object = Nothing, Optional ByRef vArchiveAsXML As Object = Nothing,
                            Optional ByRef vEmailSubTemplateCode As Object = Nothing, Optional ByRef vEmailAttachmentTemplateCode As Object = Nothing, Optional ByRef r_sCCMDocumentName As Object = Nothing, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(21/08/2000) RSAIB Process 12.
            If m_lDocumentTypeId = lCLAUSE_DOC_ID Then
                m_vRiskLinkArray = vRiskLinkArray
            End If
            m_vClauseArray = vClauseArray

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRDocTemplates.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRDocTemplate = m_oSIRDocTemplates.Item(lRow)

            ' Check the Status of the SIRDocTemplate

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRDocTemplate.DatabaseStatus
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


            m_lReturn = oSIRDocTemplate.SetProperties(iStatus:=iStatus,
                                                      vDocumentTemplateId:=vDocumentTemplateId, vCode:=vCode,
                                                      vDescription:=vDescription, vSourceId:=vSourceId,
                                                      vDocumentTypeId:=vDocumentTypeId, vCreatedById:=vCreatedById,
                                                      vDateCreated:=vDateCreated, vModifiedById:=vModifiedById,
                                                      vLastModified:=vLastModified, vIsDeleted:=vIsDeleted,
                                                      vSlotNumber:=vSlotNumber, vRiskCodeId:=vRiskCodeId,
                                                      vRiskGroupId:=vRiskGroupId, vIsEditableAfterMerging:=vIsEditableAfterMerging,
                                                      vPrinter:=vPrinter, vChaser:=vChaser, vDocumentFilter:=vDocumentFilter,
                                                      vCopyOfOriginal:=vCopyOfOriginal, vOriginalDocumentTemplateID:=vOriginalDocumentTemplateID,
                                                      vEffectiveDate:=vEffectiveDate, vIsVisibleFromWeb:=vIsVisibleFromWeb,
                                                      vIsVisibleFromClientManager:=vIsVisibleFromClientManager, vArchiveWithNoPrint:=vArchiveWithNoPrint,
                                                      vEmailAsBody:=vEmailAsBody, vSpoolDocument:=vSpoolDocument,
                                                      vArchiveAsText:=vArchiveAsText, vTemplateGroupID:=vTemplateGroupID,
                                                      vTemplateSubGroupID:=vTemplateSubGroupID, vIsInternalOnly:=vIsInternalOnly,
                                                      vIsSelectedByDefault:=vIsSelectedByDefault, vArchiveAsXML:=vArchiveAsXML,
                                                      vEmailSubTemplateCode:=vEmailSubTemplateCode, vEmailAttachmentTemplateCode:=vEmailAttachmentTemplateCode, r_sCCMDocumentName:=r_sCCMDocumentName, v_sUniqueId:=v_sUniqueId, v_sScreenHierarchy:=v_sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRDocTemplate = Nothing
                Return result
            End If

            ' Release reference to SIRDocTemplate
            oSIRDocTemplate = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRDocTemplate can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRDocTemplates.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRDocTemplate = m_oSIRDocTemplates.Item(lRow)

            ' Check the Status of the SIRDocTemplate

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRDocTemplate.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRDocTemplate.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRDocTemplate.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIRDocTemplate
            oSIRDocTemplate = Nothing

            Return result

        Catch excep As System.Exception




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
            For lSub As Integer = 1 To m_oSIRDocTemplates.Count()
                Select Case m_oSIRDocTemplates.Item(lSub).DatabaseStatus
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
        Dim oSIRDocTemplate As bSIRDocTemplate.SIRDocTemplate = Nothing
        Dim bTransStarted As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRDocTemplates.Count()
                oSIRDocTemplate = m_oSIRDocTemplates.Item(lSub)
                Select Case oSIRDocTemplate.DatabaseStatus
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
                        m_lReturn = oSIRDocTemplate.AddItem()
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
                        m_lReturn = oSIRDocTemplate.UpdateItem()
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
                        m_lReturn = oSIRDocTemplate.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRDocTemplate
            With oSIRDocTemplate
                DocumentTemplateId = .DocumentTemplateId
            End With

            ' Release last reference
            oSIRDocTemplate = Nothing

            'RWH(21/08/2000) - RSAIB Process 12.
            ' If we haven't already started a transaction start one.
            If Not bTransStarted Then
                m_lReturn = BeginTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                bTransStarted = True
            End If

            m_lReturn = UpdateDocumentClauseLinks()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lDocumentTypeId = lCLAUSE_DOC_ID Then

                m_lReturn = UpdateDocumentRiskLinks()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'KN (CMG) 04/11/2002
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

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
                    Do While lSub <= m_oSIRDocTemplates.Count()

                        ' With the item
                        With m_oSIRDocTemplates.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRDocTemplates.Delete(lSub)

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
            m_lReturn = RollbackTrans()
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAll (Public)
    '
    ' Description: Gets everything as an array
    '
    ' ***************************************************************** '
    Public Function SearchAll(ByRef r_vResultArray(,) As Object, Optional ByVal v_vDocumentTypeId As String = "") As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Check for Valid parameters
            Dim dbNumericTemp As Double

            If (Not Informations.IsNothing(v_vDocumentTypeId)) And (Not Double.TryParse(v_vDocumentTypeId, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : v_vDocumentTypeId=" & v_vDocumentTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return result
            End If

            sSQL = ""
            sSQL = sSQL & "SELECT dte.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dte.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dte.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dty.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dty.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "c.caption" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "FROM document_template dte," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "document_type dty," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "PMCaption c" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "WHERE dte.document_type_id = dty.document_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND dty.caption_id = c.caption_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE c.language_id = " & CStr(m_iLanguageID) & Strings.ChrW(13) & Strings.ChrW(10)

            'append the parameters to the where clause


            If Not Informations.IsNothing(v_vDocumentTypeId) Then
                If (v_vDocumentTypeId <> "") And (v_vDocumentTypeId <> "%") Then
                    'document type is present
                    sSQL = sSQL & "AND dty.document_type_id = " & CStr(CInt(v_vDocumentTypeId)) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'add the order by clause
            sSQL = sSQL & "ORDER BY dte.code"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyType
    '
    ' Description: Gets the party_type from the party_cnt
    '
    ' MSS030701
    ' MSS040701 Added resolved_name
    ' ***************************************************************** '

    Public Function GetPartyType(ByRef r_vPartytype(,) As Object, ByVal v_lPartyCnt As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = "SELECT pt.Code, p.resolved_name" &
                   " FROM Party p LEFT OUTER JOIN Party_Type pt ON p.party_type_id = pt.party_type_id" &
                   " WHERE p.Party_cnt =  " & v_lPartyCnt

            With m_oDatabase
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyType", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vPartytype)
            End With


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyRef
    '
    ' Description: Gets the policy reference
    '
    ' MSS040701
    ' ***************************************************************** '
    Public Function GetPolicyRef(ByRef r_vArray(,) As Object, ByVal v_lInsuranceFileCnt As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "Select insurance_ref From Insurance_File Where Insurance_File_Cnt = " & v_lInsuranceFileCnt

            With m_oDatabase
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyRef", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vArray)
            End With


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddTaskID
    '
    ' Description: Adds task template ID to the document template
    '
    ' MSS270601
    ' ***************************************************************** '

    Public Function AddTaskID(ByVal lDocumentTemplateId As String, ByVal lTaskTempID As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "Update Document_Template Set pmwrk_task_instance_temp_cnt = " & lTaskTempID &
                   " Where document_template_id = " & lDocumentTemplateId

            With m_oDatabase
                m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="AddTaskID", bStoredProcedure:=False)
            End With


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskID
    '
    ' Description: Gets task template ID from document template
    '
    ' MSS270601
    ' ***************************************************************** '

    Public Function GetTaskID(ByRef r_vTaskId(,) As Object, ByVal m_lDocumentTemplateId As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "Select pmwrk_task_instance_temp_cnt From Document_Template " &
                   "Where document_template_id = " & m_lDocumentTemplateId


            With m_oDatabase
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskID", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vTaskId)
            End With


            Return m_lReturn

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyPolicy (Public)
    '
    ' Description: Get Party and/or Policy Ref
    '
    ' MSS290601 - Created
    ' ***************************************************************** '


    Public Function GetPartyPolicy(ByRef r_vArray() As Object, ByVal m_lInsuranceFileCnt As Integer, ByVal m_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vInsArray(,) As Object = Nothing
        Dim vPartyArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim r_vArray(2) 'MKW 181003 PN7287 1.8.5 to 1.8.6 Catchup

        If m_lPartyCnt > 0 Then
            sSQL = "Select shortname From Party Where Party_Cnt = " & m_lPartyCnt

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectParty", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vPartyArray)
            'MKW 181003 PN7287 1.8.5 to 1.8.6 Catchup START
            If Informations.IsArray(vPartyArray) Then


                r_vArray(0) = CStr(vPartyArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party Details Missing for Party_cnt : " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            'MKW 181003 PN7287 1.8.5 to 1.8.6 Catchup END

        End If

        If m_lInsuranceFileCnt > 0 Then
            'MKW 181003 PN7287 1.8.5 to 1.8.6 Catchup
            sSQL = "Select Insurance_Ref, insurance_folder_cnt From Insurance_File Where Insurance_File_Cnt = " & m_lInsuranceFileCnt

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectInsRef", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vInsArray)
            'MKW 181003 PN7287 1.8.5 to 1.8.6 Catchup START
            If Informations.IsArray(vInsArray) Then
                r_vArray(1) = CStr(vInsArray(0, 0)).Trim()
                r_vArray(2) = vInsArray(1, 0)
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insurance Folder Details Missing for Insurance File Cnt : " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            'MKW 181003 PN7287 1.8.5 to 1.8.6 Catchup END

        End If


        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
        ' Resume

        'Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' eck 10/01/2001 Passed event description
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, ByVal v_sDescription As String, Optional ByVal v_lFSAComplaintFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If
            'eck100101 added description
            If r_lEventCnt = 0 Then

                m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_sDescription, vFSAComplaintFolderCnt:=If(v_lFSAComplaintFolderCnt = 0, DBNull.Value, v_lFSAComplaintFolderCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            Else

                'TODO 
                'm_lReturn = m_oEvent.DirectUpdate(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vFSAComplaintFolderCnt:=IIf(v_lFSAComplaintFolderCnt = 0, DBNull.Value, v_lFSAComplaintFolderCnt))
                'm_lReturn = m_oEvent.DirectUpdate(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vFSAComplaintFolderCnt:=IIf(v_lFSAComplaintFolderCnt = 0, DBNull.Value, v_lFSAComplaintFolderCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then
                m_oSystemOption = New bSIROptions.Business()

                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckDuplicates
    '
    ' Description:
    '
    ' History: 22/12/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CheckDuplicates(ByVal v_lSourceID As Integer, ByVal v_lDocumentId As Integer, ByVal v_lDocumentTypeId As Integer, ByVal v_lSlotNumber As Integer, ByVal v_vRiskCodeId As String, ByVal v_vRiskGroupId As String, ByRef r_bDuplicates As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = ""
            sSQL = sSQL & "SELECT document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM document_template" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "WHERE document_type_id = " & CStr(v_lDocumentTypeId) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND source_id = " & CStr(v_lSourceID) & Strings.ChrW(13) & Strings.ChrW(10)

            If v_lDocumentId <> 0 Then
                sSQL = sSQL & "AND document_template_id <> " & CStr(v_lDocumentId) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQL = sSQL & "AND slot_number = " & CStr(v_lSlotNumber) & Strings.ChrW(13) & Strings.ChrW(10)

            'DN 03/12/01 - Allow both code and group to be set to NULL

            If Not (Convert.IsDBNull(v_vRiskCodeId) Or Informations.IsNothing(v_vRiskCodeId)) Then
                sSQL = sSQL & "AND risk_code_id = " & v_vRiskCodeId & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "AND risk_code_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            End If


            If Not (Convert.IsDBNull(v_vRiskGroupId) Or Informations.IsNothing(v_vRiskGroupId)) Then
                sSQL = sSQL & "AND risk_group_id = " & v_vRiskGroupId & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "AND risk_group_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckDuplicates", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bDuplicates = Informations.IsArray(vArray)

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lDocumentTemplateId"></param>
    ''' <param name="r_lNewDocumentTemplateID"></param>
    ''' <param name="v_bCalledFromDocumentTemplate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DuplicateDocument(ByVal lDocumentTemplateId As Integer,
                                      ByRef r_lNewDocumentTemplateID As Integer,
                                      Optional ByVal v_bCalledFromDocumentTemplate As Boolean = False) As Integer
        Dim nResult As Integer = 0
        Dim oCode As Object = Nothing
        Dim oDescription As Object = Nothing
        Dim oSourceId As Object = Nothing
        Dim sDocumentTypeId As String = ""
        Dim oCreatedById As Object = Nothing
        Dim oDateCreated As Object = Nothing
        Dim oModifiedById As Object = Nothing
        Dim oLastModified As Object = Nothing
        Dim oIsDeleted As Object = Nothing
        Dim oSlotNumber As Object = Nothing
        Dim oRiskCodeId As Object = Nothing
        Dim oRiskGroupId As Object = Nothing
        Dim oIsEditableAfterMerging As Object = Nothing
        Dim oPrinter As Object = Nothing
        Dim oChaser As Object = Nothing
        Dim oDocumentFilter As Object = Nothing
        Dim oCopyOfOriginal As Object = Nothing
        Dim oOriginalDocumentTemplateID As Object = Nothing
        Dim sDocumentDirectory As String = ""
        Dim sClauseDocTemplateDirectory As String
        Dim sOriginalDocZipFile As String = ""
        Dim sCopiedDocZipFile As String = ""
        Dim sNewDocZipFile As String = ""
        Dim sUniqueFile As String = ""
        Dim sTempZipDirectory As String = ""
        Dim sOriginalDocTemplateXMLFile As String = ""
        Dim sNewDocTemplateXMLFile As String = ""
        Dim oEffectiveDate As Object = Nothing
        Dim sEditCode As String = ""
        Dim vEmailSubTemplateCode As Object = Nothing, vEmailAttachmentTemplateCode As Object = Nothing
        Dim vIsVisibleFromClientManager As Boolean = False
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve the Original Document Details
            m_lReturn = GetDetails(vDocumentTemplateId:=lDocumentTemplateId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", GetDetails failed.")
            End If

            m_lReturn = GetNext(vDocumentTemplateId:=lDocumentTemplateId, vCode:=oCode,
                                vDescription:=oDescription, vSourceId:=oSourceId,
                                vDocumentTypeId:=sDocumentTypeId,
                                vCreatedById:=oCreatedById,
                                vModifiedById:=oModifiedById,
                                vIsDeleted:=oIsDeleted, vSlotNumber:=oSlotNumber,
                                vRiskCodeId:=oRiskCodeId, vRiskGroupId:=oRiskGroupId,
                                vIsEditableAfterMerging:=oIsEditableAfterMerging,
                                vPrinter:=oPrinter, vChaser:=oChaser,
                                vDocumentFilter:=oDocumentFilter, vCopyOfOriginal:=oCopyOfOriginal,
                                vOriginalDocumentTemplateID:=oOriginalDocumentTemplateID,
                                vEffectiveDate:=oEffectiveDate,
                                vEmailSubTemplateCode:=vEmailSubTemplateCode, vEmailAttachmentTemplateCode:=vEmailAttachmentTemplateCode, vIsVisibleFromClientManager:=vIsVisibleFromClientManager)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", GetNext failed.")
            End If

            'Get the original template file details
            m_lReturn = GetDocumentDirectory(sDocumentDirectory)
            sClauseDocTemplateDirectory = sDocumentDirectory & "\Type " & sDocumentTypeId
            sOriginalDocZipFile = sClauseDocTemplateDirectory & "\Doc " & CStr(lDocumentTemplateId) & ".zip"

            'create a unique file to copy the original
            m_lReturn = bPMDocFunctions.GetUniqueName(sUniqueFile)
            If String.IsNullOrEmpty(sUniqueFile) Then
                sUniqueFile = System.IO.Path.GetRandomFileName()
                If String.IsNullOrEmpty(sUniqueFile) Then
                    Throw New System.Exception(m_lReturn.ToString() + ", " + +", GetUniqueName failed.")
                End If
            End If

            sCopiedDocZipFile = sClauseDocTemplateDirectory & "\" & sUniqueFile & ".zip"

            'Create a copy of original doc template
            m_lReturn = bPMDocFunctions.CopyFile(sOriginalDocZipFile, sCopiedDocZipFile, True)

            'original document templated edited
            'add a new record to document_template similar to original document template
            r_lNewDocumentTemplateID = 0
            oLastModified = DateTime.Now

            If v_bCalledFromDocumentTemplate Then
                If ToSafeInteger(lDocumentTemplateId) < 0 Then
                    m_lReturn = GetUniqueClauseCode(Trim(Left(oCode.ToString.Replace("_" & oCode.Split("_")(oCode.Split("_").Length - 1), ""), 7)),
                                                        r_sDocumentTemplateCode:=sEditCode)
                Else
                    m_lReturn = GetUniqueClauseCode(Trim(Left(oCode, 7)), r_sDocumentTemplateCode:=sEditCode)
                End If

                Dim sTempEditCode() As String
                sTempEditCode = sEditCode.Split("_")
                sTempEditCode(sTempEditCode.Length - 1) = "ED" & sTempEditCode(sTempEditCode.Length - 1)

                sEditCode = String.Join("_", sTempEditCode)

                m_lReturn = DirectAdd(vDocumentTemplateId:=r_lNewDocumentTemplateID,
                                            vCode:=sEditCode, vDescription:=oDescription,
                                            vSourceId:=oSourceId, vDocumentTypeId:=sDocumentTypeId,
                                            vCreatedById:=m_iUserID,
                                            vModifiedById:=m_iUserID,
                                            vIsDeleted:=oIsDeleted, vSlotNumber:=oSlotNumber,
                                            vRiskCodeId:=oRiskCodeId, vRiskGroupId:=oRiskGroupId,
                                            vIsEditableAfterMerging:=oIsEditableAfterMerging,
                                            vPrinter:=oPrinter, vChaser:=oChaser,
                                            vDocumentFilter:=oDocumentFilter,
                                            vCopyOfOriginal:=If(m_sCallingAppName.ToUpper().Trim() = ("iPMBDocTemplate").ToUpper, 0, 1),
                                            vOriginalDocumentTemplateID:=lDocumentTemplateId,
                                            vEffectiveDate:=oEffectiveDate,
                                            vEmailSubTemplateCode:=vEmailSubTemplateCode,
                                            vEmailAttachmentTemplateCode:=vEmailAttachmentTemplateCode,
                                      vIsVisibleFromClientManager:=vIsVisibleFromClientManager)

                Dim sTempDescription() As String
                If ToSafeInteger(lDocumentTemplateId) < 0 Then
                    sTempDescription = oDescription.ToString.Split("_COPY_")
                    If sTempDescription.Length = 1 Then
                        If (oDescription.length > 240) Then
                            sTempDescription(sTempDescription.Length - 1) = oDescription.SubString(0, 240).trim() & "_COPY_" & r_lNewDocumentTemplateID
                        Else
                            sTempDescription(sTempDescription.Length - 1) = oDescription.trim() & "_COPY_" & r_lNewDocumentTemplateID
                        End If
                    Else
                        If Not sTempDescription(sTempDescription.Length - 1).Contains(lDocumentTemplateId) Then
                            sTempDescription(sTempDescription.Length - 1) = sTempDescription(sTempDescription.Length - 1) & "_COPY_" & r_lNewDocumentTemplateID
                        Else
                            sTempDescription(sTempDescription.Length - 1) = r_lNewDocumentTemplateID
                        End If
                    End If
                Else
                    ReDim sTempDescription(0)
                    If (oDescription.length > 240) Then
                        sTempDescription(0) = oDescription.SubString(0, 240).trim() & "_COPY_" & r_lNewDocumentTemplateID
                    Else
                        sTempDescription(0) = oDescription.trim() & "_COPY_" & r_lNewDocumentTemplateID
                    End If

                End If

                m_lReturn = UpdateDocumentTemplateDescription(v_lDocumentTemplateId:=
                              ToSafeLong(r_lNewDocumentTemplateID),
                              v_sDescription:=String.Join("_", sTempDescription))

                'This else condition is of no use as per client requirement
            Else
                m_lReturn = DirectAdd(vDocumentTemplateId:=r_lNewDocumentTemplateID,
                                      vCode:=oCode,
                                      vDescription:=oDescription,
                                      vSourceId:=oSourceId, vDocumentTypeId:=sDocumentTypeId,
                                      vCreatedById:=m_iUserID, vDateCreated:=oDateCreated,
                                      vModifiedById:=m_iUserID, vLastModified:=oLastModified,
                                      vIsDeleted:=oIsDeleted, vSlotNumber:=oSlotNumber,
                                      vRiskCodeId:=oRiskCodeId, vRiskGroupId:=oRiskGroupId,
                                      vIsEditableAfterMerging:=oIsEditableAfterMerging,
                                      vPrinter:=oPrinter, vChaser:=oChaser, vDocumentFilter:=oDocumentFilter,
                                      vCopyOfOriginal:=1, vOriginalDocumentTemplateID:=lDocumentTemplateId,
                                      vEffectiveDate:=oEffectiveDate)
            End If
            'copy the edited document template file with newly created document
            'template file
            sNewDocZipFile = sClauseDocTemplateDirectory & "\Doc " & CStr(r_lNewDocumentTemplateID) & ".zip"
            m_lReturn = bPMDocFunctions.CopyFile(sOriginalDocZipFile, sNewDocZipFile, True)

            'last thing unzip the newly created doc template and rename the file with
            'new doc template id and zip again
            sTempZipDirectory = sClauseDocTemplateDirectory & "\" & sUniqueFile
            sOriginalDocTemplateXMLFile = sTempZipDirectory & "\Doc " & CStr(lDocumentTemplateId) & ".xml"
            sNewDocTemplateXMLFile = sClauseDocTemplateDirectory & "\Doc " &
                CStr(r_lNewDocumentTemplateID) & ".xml"

            m_lReturn = CreateFolderTree(sTempZipDirectory)
            m_lReturn = UnZip(sNewDocZipFile, sTempZipDirectory, True)

            m_lReturn = bPMDocFunctions.CopyFile(sOriginalDocTemplateXMLFile, sNewDocTemplateXMLFile, True, True)
            m_lReturn = Zip(sNewDocZipFile, "xml")

            m_lReturn = DeleteFile(sTempZipDirectory & ".zip")
            m_lReturn = DelDirectory(sTempZipDirectory)
        Catch ex As Exception
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DuplicateDocument",
                               vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            nResult = gPMConstants.PMEReturnCode.PMFalse

            Return nResult
        End Try
        Return nResult

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

    Public Function GetPreferredContact(ByVal v_lPartyCnt As Integer, ByRef r_vContactTypeID As Integer) As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetRiskClauseLinks
    '
    ' Description:
    '
    ' History: 21/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskClauseLinks(ByRef lClauseId As Integer, ByRef vRiskArray As Object) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQLString As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vRiskArray = Nothing

            '    lRecords& = PMAllRecords


            sSQLString = "spu_risk_type_clause_link_sel"

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="clause_id", vValue:=CStr(lClauseId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'PSL 16/07/2003
                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=DateTime.Today.ToString("yyyyMMdd"), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=sSQLString, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            End With

            vRiskArray = vResultArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskClauseLinks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskClauseLinks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateDocumentRiskLinks
    '
    ' Description:
    '
    ' History: 21/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateDocumentRiskLinks() As Integer
        Dim result As Integer = 0


        Const sDELETE_LINKS As String = "spu_wording_risk_link_del"
        Const sUPDATE_LINKS As String = "spu_wording_risk_link_add"



        result = gPMConstants.PMEReturnCode.PMTrue

        '    m_oDatabase.SQLBeginTrans

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(m_lDocumentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sDELETE_LINKS, sSQLName:="", bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        If Informations.IsArray(m_vRiskLinkArray) Then
            For iUpdateCount As Integer = 0 To m_vRiskLinkArray.GetUpperBound(1)
                If CStr(m_vRiskLinkArray(3, iUpdateCount)) <> "" Then

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(m_lDocumentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_vRiskLinkArray(0, iUpdateCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    ''61983 start
                    If m_iSourceID > 0 Then
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If
                    ''61983 end
                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sUPDATE_LINKS, sSQLName:="", bStoredProcedure:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

            Next iUpdateCount
        End If

        '    m_oDatabase.SQLCommitTrans

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateDocumentClauseLinks
    '
    ' Description: Removes existing clause links to parent and adds
    '               current ones.
    '
    ' History: 22/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateDocumentClauseLinks() As Integer
        Dim result As Integer = 0


        Const sDELETE_LINKS As String = "spu_wording_wording_link_del"
        Const sUPDATE_LINKS As String = "spu_wording_wording_link_add"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(m_lDocumentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sDELETE_LINKS, sSQLName:="", bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        If Informations.IsArray(m_vClauseArray) Then
            For iUpdateCount As Integer = 0 To m_vClauseArray.GetUpperBound(0)
                If CStr(m_vClauseArray(iUpdateCount)) <> "" Then

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(m_lDocumentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="calls_template_id", vValue:=CStr(m_vClauseArray(iUpdateCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sUPDATE_LINKS, sSQLName:="", bStoredProcedure:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If

            Next iUpdateCount
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:
    ' 14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function


    ' ***************************************************************** '
    ' Name: GetAvailablePrinters
    '
    ' Description: Gets and returns the list of Printers, and the
    '              default printer.
    '
    ' ***************************************************************** '
    Public Function GetAvailablePrinters(ByRef r_vPrinterArray() As String, ByRef r_sDefaultPrinter As String) As Integer

        Dim result As Integer = 0
        Dim lNoOfPrinters, lSub As Integer
        Dim sSystemName As String = "" 'MKW120503 PN3850


        result = gPMConstants.PMEReturnCode.PMTrue
        'PrinterHelper.Printers.Clear()
        ' Initialise
        r_sDefaultPrinter = ""

        ' Return the Default Printer Name
        Dim ps As New Printing.PrinterSettings()
        r_sDefaultPrinter = ps.PrinterName
        ps = Nothing

        ' If there are no printers, exit
        If r_sDefaultPrinter = "" Then
            r_vPrinterArray = Nothing
            Return result
        End If

        ' Get the number of Printers
        lNoOfPrinters = (Printing.PrinterSettings.InstalledPrinters.Count) - 1

        ' If there are none, exit
        If lNoOfPrinters < 0 Then
            r_vPrinterArray = Nothing
            Return result
        End If

        ' Size the Printer array accordingly
        'Changes as per VB code
        'ReDim r_vPrinterArray(lNoOfPrinters)

        'MKW120503 PN3850 Retrieve local system name
        'mkw080604 PN12225 Do not include sid in systemname
        gPMFunctions.GetSystemNameNoSID(sSystemName)

        ReDim r_vPrinterArray(lNoOfPrinters)
        lSub = -1
        For Each printerName As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters

            lSub += 1

            If printerName.Trim().StartsWith("\\") Then
                r_vPrinterArray(lSub) = printerName
            Else
                r_vPrinterArray(lSub) = "\\" & sSystemName & "\" & printerName
            End If

        Next

        Return result

Err_GetAvailablePrinters:

        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the List of Printers", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailablePrinters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTemplatePrinter
    '
    ' Description: get default printer attached to this template
    '
    ' History: 01/08/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function GetTemplatePrinter(ByVal v_lDocTemplateId As Integer, ByRef r_sPrinterName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_sPrinterName = ""

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(v_lDocTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTemplatePrinterSQL, sSQLName:=ACGetTemplatePrinterName, bStoredProcedure:=ACGetTemplatePrinterStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sPrinterName = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplatePrinter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplatePrinter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAvailableChasers
    '
    ' Description: Gets and returns the list of Chasers
    '
    ' AK 090402 - created
    ' ***************************************************************** '
    Public Function GetAvailableChasers(ByRef r_vChaserArray As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllChaserSQL, sSQLName:=ACGetAllChaserName, bStoredProcedure:=ACGetAllChaserStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_vChaserArray = vResultArray

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the List of Chasers", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableChasers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTemplateChaser
    '
    ' Description: get default Chaser attached to this template
    '
    ' History: AK 090402 - Created.
    '
    ' ***************************************************************** '
    Public Function GetTemplateChaser(ByVal v_lDocTemplateId As Integer, ByRef r_sChaserName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_sChaserName = ""

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(v_lDocTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTemplateChaserSQL, sSQLName:=ACGetTemplateChaserName, bStoredProcedure:=ACGetTemplateChaserStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sChaserName = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateChaser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateChaser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetChaserLettersEnabled
    '
    ' Description:
    '
    ' History: 26/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetChaserLettersEnabled(ByRef r_vValue As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTChaserLettersEnabled, v_vBranch:=m_iSourceID, r_vUnderwriting:=r_vValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaserLettersEnabled")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Public Function DeleteDocumentLink(ByVal lDocumentTemplateId As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim r_vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'VB 04/02/2005 PN 17177
            'Foreign key constraint conflict
            'DELETE statement conflicted with COLUMN REFERENCE constraint
            'FK__wording_risk_type_link__document_template_id'.
            'The conflict occurred in table 'wording_risk_type_link',
            'column 'document_template_id'.

            'As the physical document is not present, delete link from
            'wording_risk_type_link table also before deleting from Document_Template
            sSQL = "Delete from wording_risk_type_link" &
                   " Where document_template_id = " & CStr(lDocumentTemplateId)

            With m_oDatabase
                m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="DeleteDocumentLink", bStoredProcedure:=False)
            End With


            sSQL = "Delete from PMB_Doc_Link" &
                   " Where document_template_id = " & CStr(lDocumentTemplateId)

            With m_oDatabase
                m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="DeleteDocumentLink", bStoredProcedure:=False)
            End With

            ' if copies exists for this document template then do not delete it
            sSQL = "Select top 1 * from Document_Template" &
                   " Where original_document_template_id = " & CStr(lDocumentTemplateId)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Select document template", bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If Not Informations.IsArray(r_vResults) Then
                sSQL = "Delete from Policy_Standard_Wording" &
                       " Where document_template_id = " & CStr(lDocumentTemplateId)

                With m_oDatabase
                    m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="DeleteDocumentLink", bStoredProcedure:=False)
                End With

                If lDocumentTemplateId <> 0 Then
                    sSQL = "Delete from Document_Template" &
                           " Where document_template_id = " & CStr(lDocumentTemplateId)
                End If

                With m_oDatabase
                    m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="DeleteDocumentLink", bStoredProcedure:=False)
                End With
            End If


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteDocumentLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocumentLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name          : GetTemplateIdFromCode
    '
    ' Description   : Function to get the DocumentTemplate ID, & Type ID
    ' Note          : Copied from bSIRFieldManager
    ' Edit History  :
    ' RAM20050124   : Created
    ' ***************************************************************** '
    Public Function GetTemplateFromCode(ByVal sCode As String, ByRef lDocId As Integer, ByRef lDocType As Integer, ByRef sDocDesc As String, Optional ByVal v_dtEffectiveDate As Date = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0) As Integer


        Dim result As Integer = 0
        Dim sDocDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sCode.Trim() = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(lDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_type_id", vValue:=CStr(lDocType), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'AJM 8/3/01 - need to add description parameter so that SP works
            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=sDocDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_cnt", vValue:=v_lClaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetDocumentTemplateSQL, sSQLName:=ACGetDocumentTemplateName, bStoredProcedure:=ACGetDocumentTemplateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_template_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_template_id").Value)) Then
                lDocId = m_oDatabase.Parameters.Item("document_template_id").Value
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_type_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_type_id").Value)) Then
                lDocType = m_oDatabase.Parameters.Item("document_type_id").Value
            End If


            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("description").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("description").Value)) Then
                sDocDesc = m_oDatabase.Parameters.Item("description").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentTemplateWordingWordingLinks
    '
    ' Parameters: n/a
    '
    ' Description: Returns all document template codes that reference
    '               the specified document template
    '
    ' History:
    '           Created : MEvans : 06-04-2005 : PN17177
    ' ***************************************************************** '
    Public Function GetDocumentTemplateWordingWordingLinks(ByVal v_lDocumentTemplateId As Integer, ByRef r_vResults(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentTemplateWordingWordingLinks"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="document_template_id", v_vValue:=v_lDocumentTemplateId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetDocumentTemplateWordingWordingLinksSQL, sSQLName:=kGetDocumentTemplateWordingWordingLinksName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetDocumentTemplateWordingWordingLinksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                           ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetUserPrinter
    '
    ' Description: get default printer attached to this user
    '
    ' History: 06/08/2007. PN 34077
    '
    ' ***************************************************************** '
    Public Function GetUserPrinter(ByRef r_sPrinterName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_sPrinterName = ""

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserName", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserPrinterSQL, sSQLName:=ACGetUserPrinterName, bStoredProcedure:=ACGetUserPrinterStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sPrinterName = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserPrinter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserPrinter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Public Function GetClientCode(ByVal v_iPartyID As Integer, ByRef r_vClientarray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClientCode"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_iPartyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, CStr(v_iPartyID) & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientCodeSQL, sSQLName:=ACGetClientCode, bStoredProcedure:=ACGetClientCodeStored, vResultArray:=r_vClientarray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetClientCodeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function GetPMWrkTaskID(ByVal v_sTaskCode As String, ByRef r_vTaskId(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPMWrkTaskID"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="taskcode", vValue:=v_sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get Id for the task code", gPMConstants.PMELogLevel.PMLogError)

            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPMwrkTaskIdSQL, sSQLName:=ACGetPMwrkTaskId, bStoredProcedure:=ACGetPMwrkTaskIdStored, vResultArray:=r_vTaskId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetPMwrkTaskIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function



    Public Function GetPartyMainEmailAddress(ByVal v_lParty_cnt As String, ByRef v_sEmailAddress As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object = Nothing
        Const kMethodName As String = "GetPartyMainEmailAddress"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=v_lParty_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter party_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type", vValue:="MEMAIL", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter contact_type", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelEmailContactSQL, sSQLName:=ACSelEmailContactName, bStoredProcedure:=ACSelEmailContactStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSelEmailContactSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim nlength As Integer = vResultArray.GetUpperBound(1)
            For nTemp As Integer = 0 To nlength
                If (nTemp = nlength) Then
                    v_sEmailAddress += ToSafeString(vResultArray(2, nTemp))
                Else
                    v_sEmailAddress += ToSafeString(vResultArray(2, nTemp)) + ";"
                End If
            Next

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Overloads Function GetFurtherDetails(ByVal v_lDocTemplateId As Integer, ByRef r_bArchiveWithNoPrint As Boolean, ByRef r_bEmailAsBody As Boolean, ByRef r_bSpoolDocument As Boolean, ByRef r_bArchiveAsText As Boolean, ByRef r_sDocumentDescription As String, ByRef r_bArchiveAsXML As Boolean, ByRef r_sCCMDocumentName As String) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const kMethodName As String = "GetFurtherDetails"
        Try
            Dim oResultArray(,) As Object = Nothing

            Const ACArchiveWithNoPrintField As Integer = 0
            Const ACEmailasBodyField As Integer = 1
            Const ACSpoolDocumentField As Integer = 2
            Const ACArchiveAsTextField As Integer = 3
            Const ACDocumentDescriptionField As Integer = 4
            Const ACArchiveAsXMLField As Integer = 5
            Const ACCCMDocNameField As Integer = 6

            If v_lDocTemplateId <> 0 Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add("docTemplateId", CStr(v_lDocTemplateId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter to database object", gPMConstants.PMELogLevel.PMLogError)
                End If
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFurtherDetailsSQL, sSQLName:=ACGetFurtherDetailsName, bStoredProcedure:=ACGetFurtherDetailsStored, vResultArray:=oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(oResultArray) Then
                    gPMFunctions.RaiseError(kMethodName, ACGetFurtherDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                r_bArchiveWithNoPrint = (gPMFunctions.ToSafeInteger(oResultArray(ACArchiveWithNoPrintField, 0), 0) = 1)
                r_bEmailAsBody = (gPMFunctions.ToSafeInteger(oResultArray(ACEmailasBodyField, 0), 0) = 1)
                r_bSpoolDocument = (gPMFunctions.ToSafeInteger(oResultArray(ACSpoolDocumentField, 0), 0) = 1)
                r_bArchiveAsText = gPMFunctions.ToSafeBoolean(oResultArray(ACArchiveAsTextField, 0))
                r_bArchiveAsXML = gPMFunctions.ToSafeBoolean(oResultArray(ACArchiveAsXMLField, 0))
                r_sDocumentDescription = gPMFunctions.ToSafeString(oResultArray(ACDocumentDescriptionField, 0))
                r_sCCMDocumentName = gPMFunctions.ToSafeString(oResultArray(ACCCMDocNameField, 0))
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        Finally
        End Try
        Return nResult
    End Function

    Public Overloads Function GetFurtherDetails(ByVal v_lDocTemplateId As Integer, ByRef r_bArchiveWithNoPrint As Boolean, ByRef r_bEmailAsBody As Boolean, ByRef r_bSpoolDocument As Boolean, ByRef r_bArchiveAsText As Boolean, Optional ByRef r_sDocumentDescription As String = "", Optional ByRef r_bArchiveAsXML As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetFurtherDetails"
        Try
            Dim vResultArray(,) As Object = Nothing

            Const ACArchiveWithNoPrintField As Integer = 0
            Const ACEmailasBodyField As Integer = 1
            Const ACSpoolDocumentField As Integer = 2
            Const ACArchiveAsTextField As Integer = 3
            Const ACDocumentDescriptionField As Integer = 4
            Const ACArchiveAsXMLField As Integer = 5


            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lDocTemplateId <> 0 Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add("docTemplateId", CStr(v_lDocTemplateId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter to database object", gPMConstants.PMELogLevel.PMLogError)
                End If
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFurtherDetailsSQL, sSQLName:=ACGetFurtherDetailsName, bStoredProcedure:=ACGetFurtherDetailsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                    gPMFunctions.RaiseError(kMethodName, ACGetFurtherDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                r_bArchiveWithNoPrint = (gPMFunctions.ToSafeInteger(vResultArray(ACArchiveWithNoPrintField, 0), 0) = 1)
                r_bEmailAsBody = (gPMFunctions.ToSafeInteger(vResultArray(ACEmailasBodyField, 0), 0) = 1)
                r_bSpoolDocument = (gPMFunctions.ToSafeInteger(vResultArray(ACSpoolDocumentField, 0), 0) = 1)
                r_bArchiveAsText = gPMFunctions.ToSafeBoolean(vResultArray(ACArchiveAsTextField, 0))
                r_bArchiveAsXML = gPMFunctions.ToSafeBoolean(vResultArray(ACArchiveAsXMLField, 0))
                r_sDocumentDescription = gPMFunctions.ToSafeString(vResultArray(ACDocumentDescriptionField, 0))
            End If



        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally


        End Try
        Return result
    End Function

    Public Function SendEMail(ByVal v_sTo As String, ByVal v_sSubject As String,
                              ByVal v_sMessagePath As String, ByVal v_sAttachment As Object,
                              Optional ByVal sCC As String = "",
                              Optional ByVal sBCC As String = "",
                              Optional ByVal bSaveEMLFile As Boolean = False,
                              Optional ByRef sEMLFile As String = "",
                              Optional ByVal v_sMessageText As String = "") As Integer



        Dim result As Integer = 0
        Dim iRow As Integer

        Dim sSMTPEmailServer As String = "",
            sSMTPEmailPort As String = "",
            sSMTPEmailBCC As String = " ",
            sClientDocumentFolder As String
        Dim sSMTPEmailFrom As String = ""

        Dim sFailMsg As String = ""
        Dim imageResource As LinkedResource = Nothing
        Dim htmlView As AlternateView = Nothing

        Const SMTP_Email_Server As Integer = 5045
        Const SMTP_Email_Port As Integer = 5046
        Const SMTP_Email_From As Integer = 5047
        Const SMTP_Email_BCC As Integer = 5094
        Const SMTP_USER_PWD As Integer = 5183
        Const SMTP_ENABLE_SSL As Integer = 5184
        Const SMTP_User_Name As Integer = 5244

        Const kMethodName As String = "SendEMail"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get System Option (SMTP Email From)
            If String.IsNullOrEmpty(m_sSMTPEmailFrom) Then
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_From, r_sOptionValue:=m_sSMTPEmailFrom, v_iSourceID:=m_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailMsg = "Failed to get the System option SMTP_Email_From"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception(sFailMsg)
                End If

                If m_sSMTPEmailFrom = "" Then
                    ' option not populated, no action
                    sFailMsg = "Invalid SMTP"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception(sFailMsg)
                End If
            End If

            ' Get System Option (SMTP Email BCC)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_BCC, r_sOptionValue:=sSMTPEmailBCC, v_iSourceID:=m_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_BCC"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            ' Get System Option (SMTP Email Server)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_Server, r_sOptionValue:=sSMTPEmailServer, v_iSourceID:=m_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_Server"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailServer = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            ' Get System Option (SMTP Email Port)
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_Email_Port, r_sOptionValue:=sSMTPEmailPort, v_iSourceID:=m_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to get the System option SMTP_Email_Port"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            If sSMTPEmailPort = "" Then
                ' option not populated, no action
                sFailMsg = "Invalid SMTP"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception(sFailMsg)
            End If

            Dim sSMTPUserPwd As String = ""
            Dim sEnableSSL As String = ""
            Dim m_sSMTPUserName As String = String.Empty
            ' Get SMTP User Name
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_User_Name, r_sOptionValue:=m_sSMTPUserName, v_iSourceID:=m_iSourceID)


            ' Get User Password
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_USER_PWD, r_sOptionValue:=sSMTPUserPwd, v_iSourceID:=m_iSourceID)

            ' Get Enable SSL 
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_ENABLE_SSL, r_sOptionValue:=sEnableSSL, v_iSourceID:=m_iSourceID)


            'Instantiate the License class
            Dim license As Aspose.Email.License = New Aspose.Email.License()
            license.SetLicense("Aspose.Totalfor.NET.lic")

            Dim MailClient As Aspose.Email.Clients.Smtp.SmtpClient = New Aspose.Email.Clients.Smtp.SmtpClient(sSMTPEmailServer, CInt(sSMTPEmailPort))
            Dim sUserPwd As String = GetOVal(sSMTPUserPwd)

            Dim sUserLogLevel As String = ""
            Dim iUserLogLevel As Integer = 0

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                             v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                                         v_sSettingName:=gPMConstants.PMRegKeyLogLevel,
                                         r_sSettingValue:=sUserLogLevel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iUserLogLevel = ToSafeInteger(m_iLogLevel)
            Else
                iUserLogLevel = ToSafeInteger(sUserLogLevel, m_iLogLevel)
            End If

            If iUserLogLevel = PMELogLevel.PMLogDebug2 Then
                ' Get the configured log file directory from registry (same location as user's main log file)
                Dim sConfiguredLogFile As String = ""
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser,
                                             v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                             v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                                             v_sSettingName:=gPMConstants.PMRegKeyLogFile,
                                             r_sSettingValue:=sConfiguredLogFile)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sConfiguredLogFile = ""
                End If


                If Not String.IsNullOrEmpty(sConfiguredLogFile) Then
                    Dim sLogDirectory As String = Path.GetDirectoryName(sConfiguredLogFile)
                    If Not String.IsNullOrEmpty(sLogDirectory) Then
                        Dim sLogFilePath As String = Path.Combine(sLogDirectory, "aspose_smtp_log.txt")
                        MailClient.LogFileName = sLogFilePath
                    End If
                End If
            End If

            If ToSafeString(sUserPwd).Trim <> "" Then
'MailClient.Credentials = New System.Net.NetworkCredential(m_sSMTPUserName, sUserPwd)
MailClient.Password = sUserPwd
                 MailClient.Username = m_sSMTPUserName
            End If

            If ToSafeInteger(sEnableSSL) = 1 Then
'MailClient.EnableSsl = True
MailClient.SecurityOptions = SecurityOptions.SSLExplicit
            End If

            Dim SMTPMessage As MailMessage = New MailMessage(m_sSMTPEmailFrom, v_sTo)

            If sCC <> "" Then

                Dim sSplitEmail As String() = sCC.Split(","c)

                For Each SplitEmail As String In sSplitEmail
                    Dim SMTPCC As MailAddress = New MailAddress(SplitEmail)
                    SMTPMessage.CC.Add(SMTPCC)
                Next
            End If

            If sBCC <> "" Then
                Dim SMTPBCC As MailAddress = New MailAddress(sBCC)
                SMTPMessage.Bcc.Add(SMTPBCC)
            End If

            If sSMTPEmailBCC <> "" Then
                Dim SMTPBCC As MailAddress = New MailAddress(sSMTPEmailBCC)
                SMTPMessage.Bcc.Add(SMTPBCC)
            End If

            SMTPMessage.Subject = v_sSubject

            ' Check if the path exists
            If v_sMessageText = "" And Not File.Exists(v_sMessagePath) Then
                sFailMsg = "Missing file / folder for e-mail body : " & v_sMessagePath
                Throw New Exception(sFailMsg)
            End If

            'Read the contents of the Body into string
            Dim sBody As String
            If v_sMessageText = "" Then
                Dim sMessageinHTML As String = v_sMessagePath.Substring(0, v_sMessagePath.LastIndexOf(".") + 1) & "htm"
                If Not File.Exists(sMessageinHTML) Then
                    ConvertDocumentUsingSiriusDocumentUtility(v_sMessagePath, sMessageinHTML)
                End If
                sBody = System.IO.File.ReadAllText(sMessageinHTML)
                SMTPMessage.IsBodyHtml = True

                'PM039688
                Dim imageList As New List(Of String)

                Dim doc As New System.Xml.XmlDocument
                doc.Load(sMessageinHTML)

                Dim images = doc.SelectNodes("//img")
                If images IsNot Nothing Then
                    For Each image As System.Xml.XmlNode In images
                        Dim src = image.Attributes("src", "")
                        Dim fileName = Path.GetFileName(src.Value)
                        If Not imageList.Contains(fileName) Then
                            imageList.Add(fileName)
                        End If
                        image.Attributes("src").Value = String.Format("cid:{0}", Path.GetFileNameWithoutExtension(fileName.Replace(" ", "_")))

                        src = Nothing
                        fileName = Nothing
                    Next
                End If
                'Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(doc.InnerXml, Nothing, "text/html")
                htmlView = AlternateView.CreateAlternateViewFromString(doc.InnerXml, Nothing, "text/html")

                For index As Integer = 0 To imageList.Count - 1
                    'Dim imageResource As LinkedResource = New LinkedResource(Path.Combine(Path.GetDirectoryName(sMessageinHTML), imageList(index)))
                    imageResource = New LinkedResource(Path.Combine(Path.GetDirectoryName(sMessageinHTML), imageList(index)))
                    imageResource.ContentId = Path.GetFileNameWithoutExtension(imageList(index).Replace(" ", "_"))
                    imageResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                    htmlView.LinkedResources.Add(imageResource)
                    ' SMTPMessage.LinkedResources.Add(imageResource)

                Next
                SMTPMessage.AlternateViews.Add(htmlView)

                imageList = Nothing
                doc = Nothing
                images = Nothing
            Else
                sBody = v_sMessageText
                SMTPMessage.IsBodyHtml = False
                SMTPMessage.Body = sBody
            End If

            'SMTPMessage.Body = sBody

            If Informations.IsArray(v_sAttachment) Then
                For iRow = v_sAttachment.GetLowerBound(0) To v_sAttachment.GetUpperBound(0)

                    ' Check if the path exists
                    If Not File.Exists(v_sAttachment(iRow)) Then
                        sFailMsg = "Missing file / folder for e-mail attachment(s) : " & v_sAttachment(iRow)
                        Throw New Exception(sFailMsg)
                    End If

                    Dim MailAttachment As Attachment = New Attachment(v_sAttachment(iRow))

                    SMTPMessage.Attachments.Add(MailAttachment)
                Next

            ElseIf TypeOf (v_sAttachment) Is String Then
                If v_sAttachment <> "" Then
                    ' Check if the path exists
                    If Not File.Exists(v_sAttachment) Then
                        sFailMsg = "Missing file / folder for e-mail attachment : " & v_sAttachment
                        Throw New Exception(sFailMsg)
                    End If

                    Dim MailAttachment As Attachment = New Attachment(v_sAttachment)

                    SMTPMessage.Attachments.Add(MailAttachment)
                End If
            End If
            MailClient.Send(SMTPMessage)
           ' Dim eml As MailMessage = MailMessage.Load(sEMLFile)
            If bSaveEMLFile Then
                'Save a copy of the EML message file (for archiving)
                'Check if file name should be subject line
                Dim SMTP_ENABLE_Subject As Integer = 5257
                Dim sEnableSubjectLine As String = ""
                ' Get Enable Subject Line 
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=SMTP_ENABLE_Subject, r_sOptionValue:=sEnableSubjectLine, v_iSourceID:=m_iSourceID)
                If Not String.IsNullOrEmpty(sEnableSubjectLine) AndAlso sEnableSubjectLine = "1" Then
                    If v_sMessagePath <> "" Then
                        sEMLFile = IO.Path.GetPathRoot(v_sMessagePath) & "\" & v_sSubject & ".EML"
                    Else
                        sClientDocumentFolder = CStr(GetRegistryValue(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocClient", 0))
                        sEMLFile = IO.Path.GetPathRoot(sClientDocumentFolder) & "\" & v_sSubject & ".EML"
                    End If
                Else
                    If v_sMessagePath <> "" Then
                        sEMLFile = v_sMessagePath.Substring(0, v_sMessagePath.LastIndexOf("\")) & "\" & "Email Sent by " & m_sSMTPEmailFrom & " at " & StringsHelper.Format(DateTime.Now, "yyyy-MM-dd_hhmmss") & ".EML"
                    Else
                        sClientDocumentFolder = CStr(GetRegistryValue(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocClient", 0))
                        sEMLFile = IO.Path.GetPathRoot(sClientDocumentFolder) & "\" & "ArchiveMAIL" + Date.Now + ".EML"
                    End If
                End If
                'Aspose.Email.Clients.Smtp
                'eml.Save(sEMLFile)
                SMTPMessage.Save(fileName:=sEMLFile)
            End If
            If Not imageResource Is Nothing Then
                imageResource.Dispose()
            End If
            htmlView.Dispose()

            ' SMTPMessage.LinkedResources.Dispose()

            'SMTPMessage.Attachments.Dispose()
           ' SMTPMessage.Dispose()

        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ex.InnerException.Message, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFailedEmail


        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetCopiesForDocumentTemplate
    '
    ' Parameters: na
    '
    ' Description: Returns all copies for a specified document template
    '
    ' History:
    '           Created : Krishan Kr Gorav
    ' ***************************************************************** '
    Public Function GetCopiesForDocumentTemplate(ByVal v_lDocumentTemplateId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCopiesForDocumentTemplate"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="document_template_id", v_vValue:=v_lDocumentTemplateId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetCopiesForDocumentTemplateSQL, sSQLName:=kGetCopiesForDocumentTemplateName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCopiesForDocumentTemplateSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function
    Public Function ConvertFormatType(ByVal sInputFileName As String,
                                ByVal sFormat As String,
                                Optional ByVal bIsHTML As Boolean = False) As Long


        Const kMethodName As String = "ConvertFormatType"
        Dim oFSO As Scripting.FileSystemObject
        Dim sOutputFilename As String
        Dim lReturn As Integer
        Try

            ConvertFormatType = gPMConstants.PMEReturnCode.PMTrue

            If bIsHTML = False Then
                oFSO = New Scripting.FileSystemObject

                If Not (oFSO.FileExists(sInputFileName)) Then
                    oFSO = Nothing
                    Exit Function
                End If
                sOutputFilename = oFSO.GetParentFolderName(sInputFileName)
                If (sOutputFilename.Substring(sOutputFilename.Length - 1)).Trim() <> "\" Then sOutputFilename = sOutputFilename & "\"
                sOutputFilename = sOutputFilename & oFSO.GetBaseName(sInputFileName) & "." & sFormat

                If sInputFileName = sOutputFilename Then
                    Exit Function
                End If

                lReturn = ConvertDocumentUsingSiriusDocumentUtility(sInputFileName, sOutputFilename)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("ConvertDocumentUsingSiriusDocumentUtility", "Failed to convert Document")
                End If
            Else
                oFSO = New Scripting.FileSystemObject
                sOutputFilename = oFSO.GetParentFolderName(sInputFileName)
                If (sOutputFilename.Substring(sOutputFilename.Length - 1)).Trim() <> "\" Then sOutputFilename = sOutputFilename & "\"
                sOutputFilename = sOutputFilename & oFSO.GetBaseName(sInputFileName) & "." & sFormat
                lReturn = CopyServerToClient(sInputFileName, sOutputFilename)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("ConvertDocumentUsingSiriusDocumentUtility", "Failed to convert Document")
                End If
            End If


        Catch ex As Exception

            oFSO = Nothing
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ex.InnerException.InnerException.Message, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            ' This is for debugging only
        End Try
        'earlier it doesnt return anything
        Return lReturn

    End Function


    Public Function CopyServerToClient(ByVal sInputFileName As String,
                                      ByVal sOutputFilename As String) As Long


        Const kMethodName As String = "CopyServerToClient"
        Dim lReturn As Integer
        Try

            lReturn = ConvertDocumentUsingSiriusDocumentUtility(sInputFileName, sOutputFilename)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = DeleteFile(sInputFileName)

            Return lReturn

        Catch ex As Exception

            lReturn = 0
            'If oTemplate Is Nothing Then  Else oTemplate = Nothing
            ' SET 18/10/2004 ISS13245
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ex.InnerException.InnerException.Message, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return lReturn

        End Try
    End Function
    Public Function ZipDocument(ByVal sPath As String,
                    ByVal sInputDocumentFileExtension As String) As Long


        ZipDocument = Zip(sPath, sInputDocumentFileExtension)

    End Function

    Public Function UnZipDocument(ByVal sZipFile As String,
                          ByVal sOutputDirectory As String,
                          Optional ByVal v_bDeleteZipFile As Boolean = False) As Long


        UnZipDocument = UnZip(sZipFile, sOutputDirectory, v_bDeleteZipFile)

    End Function

    ' ***************************************************************** '
    ' Name          : DeleteTemplate
    ' Description   : Remove Task From the DocumentTemplate
    ' Edit History  :
    ' RAM20031118   : Created
    ' RAM20031118   : PN Issue 8093 Changes
    ' ***************************************************************** '
    Public Function DeleteTemplate(ByVal nPMWrkTaskInstTempCnt As Integer) As Integer
        Dim nResult As Integer = 0
        Dim sSQL As String = ""

        nResult = gPMConstants.PMEReturnCode.PMTrue

        Try

            sSQL = "Delete From PMWrk_Task_Instance_Temp WHERE pmwrk_task_instance_temp_cnt = " & nPMWrkTaskInstTempCnt

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteTemplate", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Public Function GetOVal(ByVal encryptedtext As String) As String
        Dim sRetVal As String = ""

        Dim TripleDes As New TripleDESCryptoServiceProvider
        Dim sKey As String = "!@$1R1U5"
        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)


        Try

            ' Convert the encrypted text string to a byte array. 
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream. 
            Dim ms As New System.IO.MemoryStream
            ' Create the decoder to write to the stream. 
            Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string. 
            sRetVal = System.Text.Encoding.Unicode.GetString(ms.ToArray)

            TripleDes = Nothing
        Catch ex As Exception
            Return ""
        End Try

        Return sRetVal
    End Function

    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key. 
        Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash. 
        ReDim Preserve hash(length - 1)
        Return hash
    End Function
    ''' <summary>
    ''' GetUniqueClauseCode
    ''' </summary>
    ''' <param name="v_sCode"></param>
    ''' <param name="r_sDocumentTemplateCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUniqueClauseCode(
                                ByVal v_sCode As String,
                                ByRef r_sDocumentTemplateCode As String) As Long
        Const kMethodName As String = "GetUniqueClauseCode"
        Dim nResult As Long = 0
        Dim aoArray(,) As Object = Nothing
        Try
            GetUniqueClauseCode = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="code", v_vValue:=v_sCode,
                                          v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(
                                    sSQL:=kGetUniqueClauseCodeSQL,
                                    sSQLName:=kGetUniqueClauseCodeName,
                                    bStoredProcedure:=True,
                                    vResultArray:=aoArray,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, kGetUniqueClauseCodeSQL & " Failed",
                           gPMConstants.PMELogLevel.PMLogError)

            End If
            If Informations.IsArray(aoArray) Then
                r_sDocumentTemplateCode = ToSafeString(aoArray(0, 0))
            End If

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=kMethodName + " Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Informations.Err().Number,
                               vErrDesc:=excep.Message)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' UpdateDocumentTemplateDescription
    ''' </summary>
    ''' <param name="v_lDocumentTemplateId"></param>
    ''' <param name="v_sDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDocumentTemplateDescription(ByVal v_lDocumentTemplateId As Long,
                                                        ByVal v_sDescription As String) As Long

        Const kMethodName As String = "UpdateDocumentTeplateDescription"
        Dim nResult As Long = 0

        Try

            UpdateDocumentTemplateDescription = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="document_template_id", v_vValue:=v_lDocumentTemplateId,
                                          v_iType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter(v_sName:="description", v_vValue:=v_sDescription,
                                          v_iType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateDocumentTemplateDescSQL,
                                              sSQLName:=kUpdateDocumentTemplateDescName,
                                              bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(m_lReturn, "Stored procedure " & kUpdateDocumentTemplateDescSQL & " failed.")
            End If
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=kMethodName + " Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Informations.Err().Number,
                               vErrDesc:=excep.Message)

            Return nResult
        End Try
    End Function


    Public Function GetEVal(ByVal plaintext As String) As String
        Dim sKey As String = "!@$1R1U5"

        Dim TripleDes As New TripleDESCryptoServiceProvider

        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)

        Dim sRetVal As String = ""

        ' Convert the plaintext string to a byte array. 
        Dim plaintextBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(plaintext)

        ' Create the stream. 
        Dim ms As New System.IO.MemoryStream
        ' Create the encoder to write to the stream. 
        Dim encStream As New CryptoStream(ms,
                                    TripleDes.CreateEncryptor(),
                                    System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string. 
        sRetVal = Convert.ToBase64String(ms.ToArray)

        TripleDes = Nothing
        Return sRetVal
    End Function

    Public Function GetPolicyLevelEmailAddress(ByVal v_lInsurance_File_Cnt As Integer) As Integer
        Return GetPolicyLevelEmailAddress(v_lInsurance_File_Cnt, String.Empty)
    End Function

    Public Function GetPolicyLevelEmailAddress(ByVal v_lInsurance_File_Cnt As Integer, ByRef v_sRecipientEmailAddress As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyLevelEmailAddress"
        Dim vResultArray As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=v_lInsurance_File_Cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Logged_In_User", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Logged_In_User", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_ID", vValue:=m_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter Branch_ID", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IsNonBatchProcess", vValue:=m_bIsNonBatchProcess, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter IsNonBatchProcess", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelPolicyLevelEmailSQL, sSQLName:=ACSelPolicyLevelEmailName, bStoredProcedure:=ACSelPolicyLevelEmailStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSelPolicyLevelEmailSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsNothing(vResultArray) Then
                m_sSMTPEmailFrom = ToSafeString(vResultArray(0, 0)).Trim()
                v_sRecipientEmailAddress = ToSafeString(vResultArray(1, 0)).Trim()
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function GetSenderEmailAddress(ByVal v_lInsurance_File_Cnt As String) As Integer
        Return GetSenderEmailAddress(v_lInsurance_File_Cnt, String.Empty)
    End Function

    Public Function GetSenderEmailAddress(ByVal v_lInsurance_File_Cnt As String, ByRef v_sEmailAddress As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetEmailSenderAddress"
        Dim vResultArray As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=v_lInsurance_File_Cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter party_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Logged_In_User", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter party_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_ID", vValue:=m_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter party_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelSenderEmailSQL, sSQLName:=ACSelSenderEmailName, bStoredProcedure:=ACSelSenderEmailStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSelEmailContactSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsNothing(vResultArray) Then
                m_sSMTPEmailFrom = ToSafeString(vResultArray(0, 0)).Trim()
                v_sEmailAddress = ToSafeString(vResultArray(1, 0)).Trim()
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

End Class

