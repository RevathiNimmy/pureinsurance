Option Strict Off
Option Explicit On
Imports System.IO
'Developer Guide No. 129
Imports SSP.Shared
Imports Sspi.Common.Aws.S3

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 3/12/97
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a DocMan.
    '
    ' TO020999 - hacked from the DocuMaster version which uses control files
    ' to do what we want to do directly.
    '
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

    'For login messages to PMB log
    Private m_sLogMess() As String

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'Generic API object
    Private m_oAPI As bDOCAPI.API

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    'eck010201
    Private m_sSourceDesc As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
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

            ' Have we a valid Database Object Reference?


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            'Get the DocuMaster Generic API object reference for writing to the DB


            m_oAPI = New bDOCAPI.API
            m_lReturn = m_oAPI.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'CMG(SJP) 14/02/2003 START ISS1852
                'Ignore if it is an error caused by documaster not being installed
                If Informations.Err().Number <> 429 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    Dim nErrClear As Integer = Informations.Err().Clear
                End If
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
                m_oAPI = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: ProcessIndex
    '
    ' Description:
    '
    ' History: 02/09/1999 Tomo - Created.
    'DN 02/04/01 - Use InsuranceFolderCnt instead of InsuranceFileCnt for ExCode
    ' ***************************************************************** '
    Public Function ProcessIndex(ByRef lMode As Integer, ByRef iSourceID As Integer, ByRef lPartyId As Integer, ByRef sPartyName As String, ByRef lInsuranceFolderId As Integer, ByRef sInsuranceFileRef As String, ByRef lClaimId As Integer, ByRef sClaimRef As String) As Integer

        'We also need company and user codes, but those are provided in Initialise.
        'Yes, I know that company hasn't been bottomed out yet, but let's use Source for now.

        Dim result As Integer = 0
        Dim vIndexListData As Object
        Dim iEmptyOnly, iAccessLevel As Integer
        Dim sFolderName As String = ""

        Dim vValue As Object = Nothing
        Dim bFolderPerInsRef As Boolean
        Dim lInsuranceFileId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Stripped down, this is what it does when we do something to an index
            '(company, client, policy, claim)

            'Set up the codes in an array the new API will understand
            ReDim vIndexListData(1, 2)

            If lInsuranceFolderId <> 0 Then

                m_lReturn = GetInsuranceFolderSource(v_lInsuranceFolderId:=lInsuranceFolderId, r_iSourceId:=m_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_iSourceID = 1
                End If

                ' Else use the Party SourceId
            Else

                'DN 07/09/01 - Get company from PartyID
                m_lReturn = GetPartySource(lPartyId)
                'PN11501 eck 080404
                '        If m_lReturn <> PMTrue Then
                '            m_iSourceID = 1
                '        End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or m_iSourceID = 0 Then
                    m_iSourceID = iSourceID
                End If
            End If


            vIndexListData(0, 0) = m_iSourceID

            'Need the company description ...

            m_lReturn = CInt(GetCompany(m_iSourceID))
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                vIndexListData(1, 0) = m_sSourceDesc
            Else

                vIndexListData(1, 0) = "Company Details Unavailable"
            End If

            ' Get the product option for Adding Documaster folder per insurance-ref

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAddDocumasterFolderPerInsuranceRef, v_vBranch:=1, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method getUnderwritingOrAgency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIndex", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bFolderPerInsRef = gPMFunctions.ToSafeBoolean(vValue)

            If lPartyId <> 0 Then


                vIndexListData(0, 1) = lPartyId

                vIndexListData(1, 1) = sPartyName

                'DJM 19/09/2003 : Always put claim folders into client folder (not policy).
                If lClaimId <> 0 Then

                    vIndexListData(0, 2) = "C" & StringsHelper.Format(lClaimId, "000000000")
                    If sClaimRef.Trim().IndexOf(New String(" "c, 3)) >= 0 Then

                        vIndexListData(1, 2) = sClaimRef
                    Else
                        'DJM 02/01/2004 : Create folder name from IDs to ensure it is correct.
                        m_lReturn = GetFolderName(lInsuranceFolderId, lClaimId, sFolderName)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder name for document", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        Else
                            vIndexListData(1, 2) = sFolderName
                        End If

                    End If

                    m_oAPI.InsuranceNum = False
                ElseIf (lInsuranceFolderId <> 0) Then
                    If bFolderPerInsRef Then
                        m_lReturn = GetInitialPolicyForRef(lInsuranceFolderId, sInsuranceFileRef, lInsuranceFileId)
                        If lInsuranceFileId > 0 Then

                            vIndexListData(0, 2) = lInsuranceFileId

                            vIndexListData(1, 2) = sInsuranceFileRef
                        Else

                            vIndexListData(0, 2) = lInsuranceFolderId
                            If sInsuranceFileRef.Trim().IndexOf(New String(" "c, 3)) >= 0 Or lMode = 1 Then

                                vIndexListData(1, 2) = sInsuranceFileRef
                            Else
                                m_lReturn = GetFolderName(lInsuranceFolderId, lClaimId, sFolderName)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder name for document", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                Else
                                    vIndexListData(1, 2) = sFolderName
                                End If
                            End If

                            m_oAPI.InsuranceNum = False
                        End If
                    Else

                        vIndexListData(0, 2) = lInsuranceFolderId
                        'DC270405 PN20539 added check for adding mode as policy wont exist to get
                        'get folder name
                        If sInsuranceFileRef.Trim().IndexOf(New String(" "c, 3)) >= 0 Then

                            vIndexListData(1, 2) = sInsuranceFileRef
                        Else
                            'DJM 02/01/2004 : Create folder name from IDs to ensure it is correct.
                            m_lReturn = GetFolderName(lInsuranceFolderId, lClaimId, sFolderName)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder name for document", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                vIndexListData(1, 2) = sFolderName
                            End If

                        End If

                        m_oAPI.InsuranceNum = False
                        'MKW 060404 Removed as creating duplicate General Folders.
                    End If
                Else

                    m_oAPI.InsuranceNum = True

                    vIndexListData(0, 2) = "GENERAL"

                    vIndexListData(1, 2) = "GENERAL"
                End If

            End If

            Select Case lMode
                Case 1
                    'Add

                    m_lReturn = m_oAPI.AddIndex(vIndexArray:=vIndexListData)
                Case 2
                    'Update - same as add

                    m_lReturn = m_oAPI.AddIndex(vIndexArray:=vIndexListData)
                Case 3
                    'Delete - need values for empty and access

                    m_lReturn = m_oAPI.DelIndex(vIndexArray:=vIndexListData, iEmptyOnly:=iEmptyOnly, iAccessLevel:=iAccessLevel)
                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log failure to addindex
                m_sLogMess(1) = "Failed in AddIndex, check PMMessage table."

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            vIndexListData = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddDocument
    '
    ' Description:
    '
    ' History: 02/09/1999 Tomo - Created.
    ' eck 010201 Add optional SourceId
    ' FSA 3.2 Add Complaint Id and References
    ' ***************************************************************** '
    'developer guide no.101
    Public Function AddDocument(ByRef lPartyId As Integer, ByRef sPartyName As String, ByRef lInsuranceFolderId As Integer, ByRef sInsuranceFileRef As String, ByRef lClaimId As Integer, ByRef sClaimRef As String, ByRef lFSAComplaintFolderCnt As Integer, ByRef sFSAComplaintReference As String, ByRef sDocType As String, ByRef sPageType As String, ByRef sDocName As String, ByRef sFilename As String, ByRef sAnnotation As String, ByRef sKeywords() As String, ByRef lDocNumber As Integer, Optional ByRef vDocumentTemplateID As Object = Nothing, Optional ByRef bVisibleFromWeb As Boolean = False, Optional ByRef bCalledFromReportScheduler As Boolean = False, Optional ByRef sFrequency As String = "ANNUALLY", Optional ByRef bArchiveAsText As Boolean = False, Optional ByRef bArchiveAsXML As Boolean = False, Optional ByRef DocumentTemplateGroupID As Integer = 0, Optional ByRef DocumentTemplateSubGroupID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vIndexListData As Object
        Dim iAccessLevel As Integer
        Dim sFolderName As String = ""
        Dim lBaseClaimId As Integer

        ' RDC 29/09/2005 for ArchiveAsPDF system option
        Dim sArchiveAsPDF As String = String.Empty
        Dim sOutputFilePath As String = String.Empty
        'AR20060925 - PN30712
        Dim sUnderwritingOrAgency As String = ""
        Dim vValue As Object = Nothing
        Dim bZipped As Boolean

        Dim bFolderPerInsRef As Boolean
        Dim lInsuranceFileId As Integer
        Dim sExtension As String = ""
        Const ARCHIVE_AS_PDF As Integer = 5009

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AR20060925 - PN30712

            m_lReturn = bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, r_vUnderwriting:=vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method getUnderwritingOrAgency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                sUnderwritingOrAgency = gPMFunctions.ToSafeString(vValue)
            End If

            If Informations.IsNothing(vDocumentTemplateID) Then
                vDocumentTemplateID = 0
            End If

            If Not bArchiveAsText And Not bArchiveAsXML Then
                ' RDC 29/09/2005 get system option ArchiveAsPDF
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=ARCHIVE_AS_PDF, r_sOptionValue:=sArchiveAsPDF)

                If vDocumentTemplateID > 0 AndAlso sArchiveAsPDF = "1" And Not sFilename.ToUpper().EndsWith(".PDF") Then
                    ' create PDF version of input file
                    m_lReturn = CreatePDFDocument(sFilename:=sFilename, sOutputFilePath:=sOutputFilePath)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' switch to PDF version
                    sFilename = sOutputFilePath
                    sDocType = "F"
                    sPageType = "PDF"
                Else
                    Dim sExt As String
                    If Not String.IsNullOrEmpty(sDocName) Then
                        If sDocName.IndexOf(".") + 1 > 0 Then

                            sExt = Path.GetExtension(sDocName).Substring(1)
                            sDocName = Path.GetFileNameWithoutExtension(sDocName)
                        Else
                            sExt = Path.GetExtension(sFilename).Substring(1)
                        End If
                    Else
                        sExt = Path.GetExtension(sFilename).Substring(1)
                        sDocName = Path.GetFileNameWithoutExtension(sFilename)
                    End If
                    sDocType = GetDMEDocType(sExt)
                    sPageType = sExt
                End If

            End If

            ' Get the product option for Adding Documaster folder per insurance-ref

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAddDocumasterFolderPerInsuranceRef, v_vBranch:=1, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method getUnderwritingOrAgency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bFolderPerInsRef = gPMFunctions.ToSafeBoolean(vValue)



            If sDocType = kDocFileTypeZIP Then

                m_lReturn = ZipDocument(v_sFileName:=sFilename, r_sOutputFilePath:=sOutputFilePath)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                bZipped = True
                sFilename = sOutputFilePath

            End If

            'Stripped down, this is what it does when we add a document

            'Set up the codes in an array the new API will understand
            If bCalledFromReportScheduler Then
                ReDim vIndexListData(1, 3)
            Else
                ReDim vIndexListData(1, 2)
            End If


            ' RDT 09/05/03 - PN3899
            ' If the insurance folder is passed in store the document
            ' against the company from the Policy
            If lInsuranceFolderId <> 0 Then

                m_lReturn = GetInsuranceFolderSource(v_lInsuranceFolderId:=lInsuranceFolderId, r_iSourceId:=m_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_iSourceID = 1
                End If

                ' Else use the Party SourceId
            Else

                'DN 07/09/01 - Get company from PartyID
                m_lReturn = GetPartySource(lPartyId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_iSourceID = 1
                End If

            End If


            vIndexListData(0, 0) = m_iSourceID
            'Need company name

            m_lReturn = CInt(GetCompany(m_iSourceID))
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                vIndexListData(1, 0) = m_sSourceDesc
            Else

                vIndexListData(1, 0) = "Company Details Unavailable"
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                vIndexListData(0, 0) = 1

                vIndexListData(1, 0) = " "
            End If

            If bCalledFromReportScheduler Then

                vIndexListData(0, 1) = "REPORTS"

                vIndexListData(1, 1) = "REPORTS"
            Else

                vIndexListData(0, 1) = lPartyId

                vIndexListData(1, 1) = sPartyName
            End If


            'DJM 18/09/2003 : Always put claim folders into client folder (not policy).
            'FSA Phase 3.2
            If lFSAComplaintFolderCnt <> 0 Then

                vIndexListData(0, 2) = "COMPLAINTS"

                vIndexListData(1, 2) = "COMPLAINTS"

            ElseIf (lClaimId <> 0) Then
                m_lReturn = GetBaseClaimID(v_lClaim_Id:=lClaimId, r_lBaseClaim_Id:=lBaseClaimId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                vIndexListData(0, 2) = "C" & StringsHelper.Format(lBaseClaimId, "000000000")
                If sClaimRef.Trim().IndexOf(New String(" "c, 3)) >= 0 Then

                    vIndexListData(1, 2) = sClaimRef
                Else
                    'DJM 02/01/2004 : Create folder name from IDs to ensure it is correct.
                    m_lReturn = GetFolderName(lInsuranceFolderId, lBaseClaimId, sFolderName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder name for document", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else
                        vIndexListData(1, 2) = sFolderName
                    End If

                End If
            ElseIf (lInsuranceFolderId <> 0) Then
                If bFolderPerInsRef Then
                    m_lReturn = GetInitialPolicyForRef(lInsuranceFolderId, sInsuranceFileRef, lInsuranceFileId)
                    If lInsuranceFileId > 0 Then

                        vIndexListData(0, 2) = lInsuranceFileId

                        vIndexListData(1, 2) = sInsuranceFileRef
                    Else

                        vIndexListData(0, 2) = lInsuranceFolderId
                        If sInsuranceFileRef.Trim().IndexOf(New String(" "c, 3)) >= 0 Then

                            vIndexListData(1, 2) = sInsuranceFileRef
                        Else
                            m_lReturn = GetFolderName(lInsuranceFolderId, lBaseClaimId, sFolderName)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder name for document", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                vIndexListData(1, 2) = sFolderName
                            End If

                        End If

                        m_oAPI.InsuranceNum = False
                    End If
                Else

                    vIndexListData(0, 2) = lInsuranceFolderId
                    If sInsuranceFileRef.Trim().IndexOf(New String(" "c, 3)) >= 0 Then

                        vIndexListData(1, 2) = sInsuranceFileRef
                    Else
                        'DJM 02/01/2004 : Create folder name from IDs to ensure it is correct.
                        m_lReturn = GetFolderName(lInsuranceFolderId, lClaimId, sFolderName)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder name for document", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        Else
                            vIndexListData(1, 2) = sFolderName
                        End If

                    End If
                End If
            Else

                If bCalledFromReportScheduler Then

                    vIndexListData(0, 2) = sFrequency

                    vIndexListData(1, 2) = sFrequency


                    vIndexListData(0, 3) = DateTime.Now.ToString("yyyy/MM/dd")

                    vIndexListData(1, 3) = DateTime.Now.ToString("yyyy/MM/dd")
                Else

                    vIndexListData(0, 2) = "GENERAL"

                    vIndexListData(1, 2) = "GENERAL"
                End If
            End If


            iAccessLevel = 9



            'Now hit the generic API to update the DB
            If bVisibleFromWeb Then

                m_lReturn = m_oAPI.Add(vIndexArray:=vIndexListData, sDocName:=sDocName, sFilename:=sFilename, sDocType:=sDocType, sPageType:=sPageType, iAccessLevel:=iAccessLevel, sUsername:=m_sUsername, sKeywords:=sKeywords, sAnnotation:=sAnnotation, vDocumentTemplateID:=vDocumentTemplateID, bVisibleFromWeb:=bVisibleFromWeb)
            Else

                m_lReturn = m_oAPI.Add(vIndexArray:=vIndexListData, sDocName:=sDocName, sFilename:=sFilename, sDocType:=sDocType, sPageType:=sPageType, iAccessLevel:=iAccessLevel, sUsername:=m_sUsername, sKeywords:=sKeywords, sAnnotation:=sAnnotation, vDocumentTemplateID:=vDocumentTemplateID)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'This errored when error... m_sLogMess(1)-Why would you use this way?
                ReDim m_sLogMess(1)

                'Log failure to add
                m_sLogMess(1) = "Failed to ADD Document, check PMMessage table."

                ' ErrorLog "ProcessControlData", m_sLogMess(1)

                result = gPMConstants.PMEReturnCode.PMFalse

            End If


            lDocNumber = m_oAPI.DocNumber

            ' Update DOC_document with category/sub-category IDs if provided
            If lDocNumber > 0 AndAlso (DocumentTemplateGroupID > 0 OrElse DocumentTemplateSubGroupID > 0) Then
                Try
                    Dim sSql As String = "UPDATE DOC_document SET "
                    Dim bFirst As Boolean = True
                    If DocumentTemplateGroupID > 0 Then
                        sSql &= "document_template_group_id = " & DocumentTemplateGroupID.ToString()
                        bFirst = False
                    End If
                    If DocumentTemplateSubGroupID > 0 Then
                        If Not bFirst Then sSql &= ", "
                        sSql &= "document_template_sub_group_id = " & DocumentTemplateSubGroupID.ToString()
                    End If
                    sSql &= " WHERE doc_num = " & lDocNumber.ToString()
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSql, sSQLName:="UpdateDocCategoryIds", bStoredProcedure:=False)
                Catch ex As Exception
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update DOC_document with category IDs for doc_num=" & lDocNumber.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
                End Try
            End If

            ' RDC 29/09/2005
            If sArchiveAsPDF = "1" Or bZipped Then
                ' don't need to keep the PDF file
                m_lReturn = KillDocument(sOutputFilePath:=sOutputFilePath)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ZipDocument
    ' Description: Zips up a document
    '
    ' History: 02/05/2007 AR - created
    ' ***************************************************************** '
    Private Function ZipDocument(ByVal v_sFileName As String, ByRef r_sOutputFilePath As String) As Integer

        Dim result As Integer = 0


        Const INFINITE_LOOP_DETECT As Integer = 8192

        Dim iPos, iFileNumber As Integer
        Dim bContinue As Boolean
        Dim oZipper As bPMZipper.Business
        Dim sFilePath As String = ""
        Dim vReturn As Boolean

        result = gPMConstants.PMEReturnCode.PMTrue

        iPos = If(v_sFileName = "" And "." = "", 0, (v_sFileName.LastIndexOf(".") + 1))

        If iPos = 0 Then
            Return result
        End If

        Do
            iFileNumber += 1
            sFilePath = v_sFileName.Substring(0, iPos - 1) & "_" & CStr(iFileNumber) & ".zip"
            bContinue = Not gPMFunctions.FileExists(sFilePath)
        Loop While (Not bContinue) And (iFileNumber < INFINITE_LOOP_DETECT)

        If iFileNumber = INFINITE_LOOP_DETECT Then
            gPMFunctions.RaiseError("ZipDocument", "Could not find a valid filename to unzip the file " & v_sFileName & " into", gPMConstants.PMELogLevel.PMLogError)
        End If

        oZipper = New bPMZipper.Business()

        vReturn = oZipper.ZipFile(v_sFileName, sFilePath)
        If Not vReturn Or vReturn = gPMConstants.PMEReturnCode.PMError Then
            gPMFunctions.RaiseError("oZipper.Zip", "Failed to zip the file " & v_sFileName, gPMConstants.PMELogLevel.PMLogError)
        End If

        oZipper = Nothing

        r_sOutputFilePath = sFilePath

        Return result

    End Function

    Public Function GetDocument(ByVal v_lDocNum As Integer, ByVal v_bOutputAsPdf As Boolean, ByRef r_sDocumentPath As String) As Integer
        Dim result As Integer = 0
        'TODO
        'Dim oZipper As bSIRZipper.Zipper
        Dim sFilePath As String = ""
        Dim bDocumentZipped As Boolean
        Dim sDocType As String = String.Empty
        Dim sTempDocPath As String = String.Empty
        Dim sTempName As String = String.Empty
        Dim sTempDir As String = String.Empty
        Dim sDocName As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Current File Location
            Dim sSQL As String = ""
            Dim vResultArray(,) As Object = Nothing
            sSQL = "select dev.server_unc, dev.share_name, vol.directory, Page.page_name, Page.page_type, " &
                    "doc.zipped, doc.doc_type From doc_document doc " &
                    "join doc_page page on page.doc_num = doc.doc_num join doc_volume vol on vol.volume_id = page.volume_id " &
                    "join doc_device dev on dev.device_id = vol.device_id Where doc.doc_num = " & CStr(v_lDocNum)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentLocation", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sFilePath = ""
            sTempDir = ""
            If Informations.IsArray(vResultArray) Then
                'Server UNC

                sFilePath = CStr(vResultArray(0, 0)).Trim()

                sTempDir = CStr(vResultArray(0, 0)).Trim()

                'Share Name

                sFilePath = sFilePath & CStr(vResultArray(1, 0)).Trim()

                sTempDir = sTempDir & CStr(vResultArray(1, 0)).Trim()

                'Directory

                sFilePath = sFilePath & CStr(vResultArray(2, 0)).Trim()

                'Page Name

                sFilePath = sFilePath & CStr(vResultArray(3, 0)).Trim()

                sDocName = CStr(vResultArray(3, 0)).Trim().Replace("\", "")

                'Page Type

                sFilePath = sFilePath & "." & CStr(vResultArray(4, 0)).Trim()

                sDocName = sDocName & "." & CStr(vResultArray(4, 0)).Trim()


                bDocumentZipped = CStr(vResultArray(5, 0)).Trim() = "Y"


                sDocType = CStr(vResultArray(6, 0)).Trim()

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document location", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Dim cloudHostingOptionValue As String = ""
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

            If (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then
                Dim filePath As String = CStr(vResultArray(3, 0)).Trim() & "." & CStr(vResultArray(4, 0)).Trim()
                Dim s3FileName As String = filePath.Replace("\", "/").TrimStart("/")

                'Get Temporary Location
                sTempName = ""
                m_lReturn = DOCGeneralFunc.GetUniqueName(r_sName:=sTempName)
                sTempDocPath = sTempDir & "\tmp\" & sTempName
                m_lReturn = MakePath(sTempDocPath)
                sFilePath = Path.Combine(sTempDocPath, filePath.Substring(filePath.LastIndexOf("\") + 1))
                Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_DME_BUCKET_NAME"),
                    Environment.GetEnvironmentVariable("AWS_REGION"),
                    m_sUsername)
                m_lReturn = repository.DownloadFileAsync(s3FileName, sTempDocPath).Result
            End If

            If v_bOutputAsPdf Then

                If (sDocType.ToUpper() = "D") Or (sDocType.ToUpper() = "W") Then

                    'Get Temporary Location
                    m_lReturn = DOCGeneralFunc.GetUniqueName(r_sName:=sTempName)
                    sTempDocPath = sTempDir & "\tmp\" & sTempName & "\"
                    m_lReturn = MakePath(sTempDocPath)

                    'Copy Document To Temporary Location
                    m_lReturn = CopyFile(sFilePath, sTempDocPath & sDocName)


                    'Convert Doc To Pdf.
                    m_lReturn = ConvertDocToPdf(sFilename:=sTempDocPath & sDocName, sOutputFilePath:=r_sDocumentPath)


                    'Destroy copied file
                    m_lReturn = DOCGeneralFunc.DeleteFile(sTempDocPath & sDocName)


                ElseIf sDocType.ToUpper() = "F" Then


                    r_sDocumentPath = sFilePath


                ElseIf sDocType.ToUpper() = "H" Then


                    'Copy Document To Temporary Location
                    m_lReturn = CopyFile(sFilePath, sTempDocPath & sDocName)


                    'Convert Doc To Pdf.
                    m_lReturn = CreatePDFDocument(sFilename:=sTempDocPath & sDocName, sOutputFilePath:=r_sDocumentPath)


                    'Destroy copied file
                    m_lReturn = DOCGeneralFunc.DeleteFile(sTempDocPath & sDocName)


                Else


                    r_sDocumentPath = sFilePath


                End If


            ElseIf Not bDocumentZipped Then


                'Zip up original Document to temporary Location
                'TODO
                'oZipper = New bSIRZipper.Zipper()

                'm_lReturn = CInt(oZipper.ZipFile(sFilePath, sTempDocPath & sTempName & ".ZIP"))
                'oZipper = Nothing
                'Get Temporary Location
                If (sDocType.ToUpper() = "T") Then

                    r_sDocumentPath = sFilePath
                Else
                    'Get Temporary Location
                    m_lReturn = DOCGeneralFunc.GetUniqueName(r_sName:=sTempName)
                    sTempDocPath = sTempDir & "\tmp\" & sTempName & "\"
                    m_lReturn = MakePath(sTempDocPath)
                    'Return Zipped Document Location
                    r_sDocumentPath = sTempDocPath & sTempName & ".ZIP"
                End If



            Else


                'Return Location to existing zipped document.
                r_sDocumentPath = sFilePath


            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CreatePDFDocument
    '
    ' Description: create PDF version of input file in AddDocument. Used
    '              if system option ArchiveAsPDF is enabled
    '
    ' History: 29/09/2005 RDC - created
    ' ***************************************************************** '
    Private Function CreatePDFDocument(ByVal sFilename As String, ByRef sOutputFilePath As String) As Integer

        Dim result As Integer = 0
        Dim iPos As Integer
        Dim sTemp As String = ""
        Dim objConvert As SiriusDocumentUtility.Document


        result = gPMConstants.PMEReturnCode.PMFalse

        iPos = If(sFilename = "" And "." = "", 0, (sFilename.LastIndexOf(".") + 1))

        If iPos = 0 Then
            Return result
        End If

        sOutputFilePath = sFilename.Substring(0, iPos) & "pdf"

        'delete the output file if it exists


        If File.Exists(sOutputFilePath) Then

            File.Delete(sOutputFilePath)
        End If

        objConvert = New SiriusDocumentUtility.Document
        objConvert.Convert(sFilename, sOutputFilePath)

        objConvert = Nothing

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: ConvertDocToPdf
    '
    ' Description:
    '
    ' History: 18/04/2007 MKW - created
    ' ***************************************************************** '
    Private Function ConvertDocToPdf(ByVal sFilename As String, ByRef sOutputFilePath As String) As Integer
        Dim result As Integer = 0

        Dim iPos As Integer
        Dim sTemp As String = ""
        Dim oPDF As Object = Nothing
        Dim oFSO As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        iPos = If(sFilename = "" And "." = "", 0, (sFilename.LastIndexOf(".") + 1))

        If iPos = 0 Then
            Return result
        End If

        sOutputFilePath = sFilename.Substring(0, iPos) & "pdf"

        'delete the output file if it exists
        oFSO = New Object()

        If File.Exists(sOutputFilePath) Then

            File.Delete(sOutputFilePath)
        End If

        oFSO = Nothing

        ' Document Conversion Engine
        'TODO
        'oPDF = New DocConverter.DocConverterX()

        With oPDF

            .Convert(ToSafeString(sFilename), ToSafeString(sOutputFilePath), "-cPDF")
        End With

        oPDF = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Name: KillPDFDocument
    '
    ' Description: delete PDF version of file used in AddDocument if
    '              system option ArchiveAsPDF is enabled.
    '
    ' History: 29/09/2005 RDC - created
    ' ***************************************************************** '
    Private Function KillDocument(ByVal sOutputFilePath As String) As Integer

        Dim result As Integer = 0
        Dim oFSO As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        'delete the output file if it exists
        oFSO = New Object()

        If File.Exists(sOutputFilePath) Then

            File.Delete(sOutputFilePath)
        End If

        oFSO = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    'eck 010201
    Private Function GetCompany(ByRef iSourceID As Integer) As Object

        Dim result As Object = Nothing
        Dim oSource As bPMSource.Business



        result = gPMConstants.PMEReturnCode.PMTrue
        oSource = New bPMSource.Business
        m_lReturn = oSource.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        m_lReturn = oSource.GetDetails(vSourceID:=iSourceID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oSource = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oSource.GetNext(vSourceID:=iSourceID, vDescription:=m_sSourceDesc)


        oSource = Nothing

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function GetPartySource(ByRef lPartyId As Integer) As Integer

        Dim result As Integer = 0
        Dim oParty As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Services", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)



        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If
        With oParty

            .PartyCnt = lPartyId

            m_lReturn = .GetDetails()


            m_iSourceID = .SourceID
        End With

        ' Destroy Party object

        oParty.Dispose()

        oParty = Nothing
        Return result

    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFolderSource
    '
    ' Description: We get the Source Id from the Insurance File table as the Insurance
    '              Folder Source Id cannot be trusted.
    '
    ' History: 09/05/2003 RDT - Created.
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFolderSource(ByVal v_lInsuranceFolderId As Integer, ByRef r_iSourceId As Integer) As Integer

        Dim result As Integer = 0
        Dim oInsuranceFile As Object = Nothing
        Dim lInsuranceFileCnt As Object = 0
        Dim vFieldArray As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Create object
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFile, v_sClassName:="bSIRInsuranceFile.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Get the Insurance File Cnt

            m_lReturn = oInsuranceFile.GetFromTable(v_vTableName:="insurance_file", v_vFieldName:="MAX(insurance_file_cnt)", v_vKeyField:="insurance_folder_cnt", v_vKeyID:=ToSafeInteger(v_lInsuranceFolderId), r_vResult:=lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oInsuranceFile.GetDetails(vInsuranceFileCnt:=ToSafeInteger(lInsuranceFileCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oInsuranceFile.GetNext(r_vFieldArray:=vFieldArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Source ID from the array

            r_iSourceId = CInt(vFieldArray(5))

            'Destroy object

            oInsuranceFile.Dispose()
            oInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFolderSource Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolderSource", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    'DJM 02/01/2004
    Private Function GetFolderName(ByVal v_lInsuranceFolderId As Object, ByVal v_lClaimId As Integer, ByRef r_sFolderName As String) As Integer

        Dim result As Integer = 0

        Dim oFind As Object = Nothing
        Dim vSearchData As Object = Nothing
        Dim sRef As String
        Dim sDesc As Object
        Dim lInsFileCnt As Object
        'Claim Detail Variables
        Dim sClaimNo As String = String.Empty
        Dim sPolicyNo As String = String.Empty
        Dim lPolicyID As Integer
        Dim sDescription As String = ""
        Dim lClaimStatusID, lProgressStatusID, lPrimaryCauseID, lSecondaryCauseID, lCatastropheCodeID As Integer
        Dim sLossFromDate As String = String.Empty
        Dim sLossToDate As String = String.Empty
        Dim sReportedDate As String = String.Empty
        Dim sReportedToDate As String = String.Empty
        Dim sLastModifiedDate As String = String.Empty
        Dim lHandlerID, lCurrencyID As Integer
        Dim nInfoOnly, nLikelyClaim As Integer
        Dim sLocation As String = ""
        Dim lTown, lRiskTypeID As Integer
        Dim sClientName As String = ""
        Dim sClientAddress As Integer
        Dim sClientTelNo As String = String.Empty
        Dim sClientFaxNo As String = String.Empty
        Dim sClientMobileNo As String = String.Empty
        Dim sClientEMail As String = String.Empty
        Dim sClientClaimNo As String = String.Empty
        Dim sInsurerName As String = String.Empty
        Dim sClientTelNoOff As String = String.Empty
        Dim sInsurerAddress As Integer
        Dim sInsurerTelNo As String = String.Empty
        Dim sInsurerFaxNo As String = String.Empty
        Dim sInsurerEmail As String = String.Empty
        Dim sInsurerClaimNo As String = String.Empty
        Dim sInsurerContact As String = String.Empty
        Dim nVATRegistered As Integer
        Dim sVATRegisteredNo As String = String.Empty
        Dim sComments As String = String.Empty
        Dim sClaimsStatusDate As String = String.Empty
        Dim sClientShortName As String = String.Empty
        Dim sInsurerShortName As String = String.Empty
        Dim lUserDefFldA, lUserDefFldB, lUserDefFldC, lUserDefFldD, lUserDefFldE As Integer
        Dim iSourceID, iLanguageID As Integer
        Dim vUnderwritingYearID As Object = Nothing
        Dim lVersionId As Integer
        Dim vClaimHandled As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lClaimId <> 0 Then

            'Get component for retrieving of claim description
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oFind, v_sClassName:="bOpenClaim.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Select the Claim from the Database

            oFind.SetKeyID(ToSafeInteger(v_lClaimId))

            oFind.SelectSingle()
            'AR20060925 - PN30712 Add VersionId and ClaimHandled parameters

            oFind.GetProperties(0, ToSafeString(sClaimNo), ToSafeString(sPolicyNo), CInt(lPolicyID), ToSafeString(sDescription), ToSafeInteger(lClaimStatusID), lProgressStatusID.ToString, ToSafeInteger(lPrimaryCauseID), ToSafeInteger(lSecondaryCauseID), ToSafeInteger(lCatastropheCodeID), ToSafeString(sLossFromDate), ToSafeString(sLossToDate), ToSafeString(sReportedDate), ToSafeString(sReportedToDate), ToSafeString(sLastModifiedDate), ToSafeInteger(lHandlerID), ToSafeInteger(lCurrencyID), ToSafeInteger(nInfoOnly), ToSafeInteger(nLikelyClaim), ToSafeString(sLocation), ToSafeInteger(lTown), ToSafeInteger(lRiskTypeID), ToSafeString(sClientName), ToSafeString(sClientAddress), ToSafeString(sClientTelNo), ToSafeString(sClientFaxNo), ToSafeString(sClientMobileNo), ToSafeString(sClientEMail), ToSafeString(sClientClaimNo), ToSafeString(sInsurerName), ToSafeString(sInsurerAddress), ToSafeString(sInsurerTelNo), ToSafeString(sInsurerFaxNo), ToSafeString(sInsurerEmail), ToSafeString(sInsurerClaimNo), ToSafeString(sInsurerContact), ToSafeInteger(nVATRegistered), ToSafeString(sVATRegisteredNo), ToSafeString(sComments), ToSafeString(sClaimsStatusDate), ToSafeString(sClientShortName), ToSafeString(sInsurerShortName), ToSafeString(sClientTelNoOff), ToSafeInteger(lUserDefFldA), ToSafeInteger(lUserDefFldB), ToSafeInteger(lUserDefFldC), ToSafeInteger(lUserDefFldD), ToSafeInteger(lUserDefFldE), ToSafeInteger(iSourceID), ToSafeInteger(iLanguageID), CType(vUnderwritingYearID, Object), ToSafeInteger(lVersionId), CType(vClaimHandled, Object))

            sRef = sClaimNo
            sDesc = sDescription

            'Destroy the object

            oFind.Dispose()
            oFind = Nothing

        ElseIf v_lInsuranceFolderId <> 0 Then

            'Get component for retrieving of policy details
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oFind, v_sClassName:="bSIRFindInsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Get the policy details

            m_lReturn = oFind.GetAllPolicyVersion(r_vResultArray:=vSearchData, v_lInsuranceFolderCnt:=v_lInsuranceFolderId)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Informations.IsArray(vSearchData)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the details from the latest version


            lInsFileCnt = CInt(vSearchData(1, vSearchData.GetLowerBound(1)))


            sRef = CStr(vSearchData(4, vSearchData.GetLowerBound(1)))

            'Destroy FindInsurance object

            oFind.Dispose()
            oFind = Nothing

            'Get component for retrieving of policy description
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oFind, v_sClassName:="bSIRInsuranceFileSystem.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Get the policy details

            m_lReturn = oFind.GetDetails(vInsuranceFileCnt:=lInsFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the description

            m_lReturn = oFind.GetNext(vLastTransDescription:=sDesc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vSearchData) Then
                sRef = CStr(vSearchData(4, vSearchData.GetLowerBound(1)))
            Else
                oFind = Nothing
                sRef = "GENERAL"
            End If

            'Return the folder name
            If sRef Is Nothing Or sRef = "" Then
                r_sFolderName = "GENERAL"
            Else
                r_sFolderName = sRef.Trim()
            End If
        End If
        Return result

    End Function


    Public Function GetInitialPolicyForRef(ByVal v_lInsuranceFolderId As Integer, ByVal v_sInsuranceFileRef As String, ByRef r_lInsuranceFileId As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new ones
            lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=v_sInsuranceFileRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInitialInsuranceFileCntSQL, sSQLName:=ACGetInitialInsuranceFileCntName, bStoredProcedure:=ACGetInitialInsuranceFileCntStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lInsuranceFileId = CStr(vResultArray(0, 0))
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInitialPolicyForRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInitialPolicyForRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetBaseClaimID
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetBaseClaimID(ByVal v_lClaim_Id As Integer, ByRef r_lBaseClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBaseClaimID"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Id", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_CLM_Get_Base_Claim", sSQLName:="GetBaseClaimId", bStoredProcedure:=True, vResultArray:=vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "spu_GetBaseClaimId" & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResultArray) Then

                r_lBaseClaim_Id = CInt(vResultArray(0, 0))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '8.5 Copied function of Library DOCGeneralFunc.bas
    Public Function CopyFile(ByRef sFileIn As String, ByRef sFileOut As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            File.Copy(sFileIn, sFileOut)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy file '" & sFileIn & "' to '" & sFileOut & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ConvertHTMLDocToPDF(ByVal sFilename As String,
                                ByRef sOutputFilePath As String) As Integer


        Dim sArchiveAsPDF As String = String.Empty
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        'Const kMethodName As String = "ConvertHTMLDocToPDF"

        Try

            Const ARCHIVE_AS_PDF = 5009

            ConvertHTMLDocToPDF = result

            m_lReturn = bPMFunc.GetSystemOption(
                                v_sUsername:=m_sUsername,
                                v_sPassword:=m_sPassword,
                                v_iUserID:=m_iUserID,
                                v_iMainSourceID:=m_iSourceID,
                                v_iLanguageID:=m_iLanguageID,
                                v_iCurrencyID:=m_iCurrencyID,
                                v_iLogLevel:=m_iLogLevel,
                                v_sCallingAppName:=ACApp,
                                v_iOptionNumber:=ARCHIVE_AS_PDF,
                                r_sOptionValue:=sArchiveAsPDF)

            If sArchiveAsPDF = "1" And Right(sFilename.ToUpper, 4) <> ".PDF" Then
                m_lReturn = CreatePDFDocument(
                                    sFilename:=sFilename,
                                    sOutputFilePath:=sOutputFilePath)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Function
                End If
            End If

        Catch excep As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ConvertHTMLDocToPDF Falied", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertHTMLDocToPDF", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        Finally
        End Try
    End Function
    'Start - Prakash Varghese - PN71845
    ' ***************************************************************** '
    ' Name: ZipReport
    ' Description: This public method is created for SAM's GetReport functionality
    '              The functionality required could have been achieved by amending ZipDocument
    '              method. But to avoid any possible side effects to existing system,
    '              new method is created
    ' History: 13/05/2010 Prakash Varghese - created
    ' ***************************************************************** '
    Public Function ZipReport(ByVal v_sFileName As String,
                                ByRef r_sOutputFilePath As String,
                                Optional ByVal v_bExcludeDirectories As Boolean = False) As Integer

        Const INFINITE_LOOP_DETECT As Long = 8192
        Const kZipCommandSuccess As Integer = 0

        Dim iPos As Integer
        Dim iFileNumber As Integer
        Dim bContinue As Boolean
        Dim oZipper As Object
        Dim sFilePath As String
        Dim vReturn As Object
        Dim bIsHTML As Boolean
        Dim sPathComponents() As String
        Dim sPathDir As String = String.Empty
        Dim iCounter As Integer
        Dim iPathLevel As Integer
        Dim sTmpFileName As String
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        'Const kMethodName As String = "ZipReport"

        Try
            ZipReport = result

            iPos = v_sFileName.LastIndexOf(".") + 1

            If iPos = 0 Then
                Exit Function
            Else
                If v_sFileName.ToUpper.IndexOf(".HTML") + 1 > 0 Then
                    bIsHTML = True
                End If
            End If

            Do
                iFileNumber = iFileNumber + 1
                sFilePath = Informations.Left(v_sFileName, iPos - 1) & "_" & CStr(iFileNumber) & ".zip"
                bContinue = Not FileExists(sFilePath)
            Loop While (Not bContinue) And (iFileNumber < INFINITE_LOOP_DETECT)

            If iFileNumber = INFINITE_LOOP_DETECT Then
                RaiseError("ZipDocument", "Could not find a valid filename to unzip the file " & v_sFileName & " into", gPMConstants.PMELogLevel.PMLogError)
            End If

            oZipper = New bPMZipper.Business

            oZipper.NoDirectory = v_bExcludeDirectories

            vReturn = oZipper.ZipFile(ToSafeString(v_sFileName), ToSafeString(sFilePath))
            If vReturn = False Or vReturn = gPMConstants.PMEReturnCode.PMError Then
                RaiseError("oZipper.Zip", "Failed to zip the file " & v_sFileName, gPMConstants.PMELogLevel.PMLogError)
            End If

            If bIsHTML Then
                'For html file any dependent files need to be identified and added to zip file
                sPathComponents = sFilePath.Split("\")
                iPathLevel = sPathComponents.Length - 1
                If iPathLevel > 0 Then
                    sPathDir = sPathComponents(0)
                    For iCounter = 1 To iPathLevel - 1
                        sPathDir = sPathDir & "\" & sPathComponents(iCounter)
                    Next
                End If

                If IsFolderExists(sPathDir) Then
                    sPathDir = sPathDir & "\"
                    ' Dim index As Integer = 0
                    Dim sFolderName As New System.IO.DirectoryInfo(sPathDir)

                    Dim allfiles As IO.FileInfo() = sFolderName.GetFiles("*.*")

                    For Each File As FileInfo In allfiles
                        sTmpFileName = File.Name
                        If sTmpFileName.ToUpper.Contains(".HTML") AndAlso
                            sTmpFileName.ToUpper.Contains(".ZIP") Then
                            vReturn = oZipper.addFileToZIP(ToSafeString(sFilePath), ToSafeString(sPathDir & sTmpFileName))

                            If vReturn <> kZipCommandSuccess Then
                                RaiseError("oZipper.Zip", "Failed to zip the file " & sTmpFileName, gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    Next
                End If
            End If
            oZipper = Nothing
            r_sOutputFilePath = sFilePath

        Catch excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            ZipReport = result
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ZipReport Falied", vApp:=ACApp, vClass:=ACClass, vMethod:="ZipReport", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        Finally

        End Try
    End Function
    Public Function AddDocumentDirect(ByVal lFolderNum As Integer,
                                ByVal sDocType As String,
                                ByVal sPageType As String,
                                ByVal sDocName As String,
                                ByVal sFilename As String,
                                ByVal lDocNumber As Integer,
                                Optional ByVal vDocumentTemplateID As Object = Nothing,
                                Optional ByVal bVisibleFromWeb As Boolean = False,
                                Optional ByVal DocumentTemplateGroupID As Integer = 0,
                                Optional ByVal DocumentTemplateSubGroupID As Integer = 0) As Integer

        Dim sOutputFilePath As String = String.Empty
        Dim bZipped As Boolean
        AddDocumentDirect = gPMConstants.PMEReturnCode.PMTrue
        Try
            If sDocType = kDocFileTypeZIP Then

                m_lReturn = ZipDocument(v_sFileName:=sFilename, r_sOutputFilePath:=sOutputFilePath)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Function
                End If

                bZipped = True
                sFilename = sOutputFilePath

            End If
            'Now hit the generic API to update the DB
            m_lReturn = m_oAPI.AddDocumentDirect(
            sDocName:=sDocName,
            sFilename:=sFilename,
            sDocType:=sDocType,
            lFolderNum:=lFolderNum,
            iAccessLevel:=9,
            sUsername:=m_sUsername,
            vDocumentTemplateID:=vDocumentTemplateID,
            bVisibleFromWeb:=bVisibleFromWeb,
            sPageType:=sPageType)
            'TODO: Error Check

            lDocNumber = m_oAPI.DocNumber

            ' Update DOC_document with category/sub-category IDs if provided
            If lDocNumber > 0 AndAlso (DocumentTemplateGroupID > 0 OrElse DocumentTemplateSubGroupID > 0) Then
                Try
                    Dim sSql As String = "UPDATE DOC_document SET "
                    Dim bFirst As Boolean = True
                    If DocumentTemplateGroupID > 0 Then
                        sSql &= "document_template_group_id = " & DocumentTemplateGroupID.ToString()
                        bFirst = False
                    End If
                    If DocumentTemplateSubGroupID > 0 Then
                        If Not bFirst Then sSql &= ", "
                        sSql &= "document_template_sub_group_id = " & DocumentTemplateSubGroupID.ToString()
                    End If
                    sSql &= " WHERE doc_num = " & lDocNumber.ToString()
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSql, sSQLName:="UpdateDocCategoryIdsDirect", bStoredProcedure:=False)
                Catch ex2 As Exception
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update DOC_document with category IDs for doc_num=" & lDocNumber.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocumentDirect", vErrNo:=Informations.Err().Number, vErrDesc:=ex2.Message, excep:=ex2)
                End Try
            End If
        Catch ex As Exception


            AddDocumentDirect = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddDocumentDirect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDocumentDirect", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
        End Try
    End Function
    Private Function GetDMEDocType(ByVal sExt As String) As String
        Select Case sExt.ToUpper
            Case "TIF"
                Return "I"
            Case "TXT"
                Return "T"
            Case "RTF"
                Return "W"
            Case "DOC", "DOCX"
                Return "D"
            Case "XLS", "XLSX"
                Return "X"
            Case "PPT", "PPTX"
                Return "P"
            Case "MDB"
                Return "A"
            Case "HTM", "HTML"
                Return "H"
            Case "GIF"
                Return "G"
            Case "JPG", "JPEG", "PNG"
                Return "J"
            Case "EML", "MSG"
                Return "M"
            Case "PDF"
                Return "F"
            Case "HLP"
                Return "E"
            Case "ZIP", "RAR", "7Z"
                Return "Z"
            Case Else
                Return ""
        End Select
    End Function
End Class
