Option Strict Off
Option Explicit On
Imports System.Data
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Detail_NET.Detail")> _
Public NotInheritable Class Detail
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Detail
    '
    ' Date: 14/09/1998
    '
    ' Description: Describes the SIRInsuranceFile entity attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Detail"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Instance of Business class
    Private m_oSIRInsuranceFile As Business

    ' Instance of InsuranceFileSystem Business class
    Private m_oSIRInsuranceFileSystem As bSIRInsuranceFileSystem.Business

    ' Instance of InsuranceFolder Business class
    Private m_oSIRInsuranceFolder As bSIRInsuranceFolder.Business

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer

    ' DataBase Attributes for Insurance_File
    Private m_lInsuranceFileStructureID As Integer
    Private m_lInsuranceFileTypeID As Integer
    'Developer Guide No. 17 - Implemented in different places in below section
    Private m_vInsuranceFileStatusID As Object
    Private m_lInsuranceFileID As Integer
    Private m_lInsuranceFolderCnt As Integer
    'Developer Guide No. 128
    Private m_sInsuranceRef As String
    Private m_lProductID As Integer
    Private m_lLeadInsurerCnt As Integer
    Private m_lLeadAgentCnt As Integer
    Private m_vLeadAgentPercent As Object
    Private m_lAccountHandlerCnt As Integer
    Private m_lInsuredCnt As Integer
    Private m_iBusinessTypeID As Integer
    Private m_vCollectTypeID As Object
    Private m_lCollectionFromCnt As Integer
    'sj 19/07/2002 - start
    'Private m_iBranchID As Integer
    Private m_vSubBranchID As Object
    'sj 19/07/2002 - end
    Private m_vDateIssued As Object
    Private m_dtCoverStartDate As Date
    Private m_dtExpiryDate As Date
    Private m_dtRenewalDate As Date
    Private m_vRenewalMethodID As Object
    Private m_iRenewalFrequencyID As Integer
    Private m_iIsReferredAtRenewal As Integer
    Private m_vLapsedReasonID As Object
    Private m_vLapsedDate As Object
    Private m_vLapsedDescription As Object
    Private m_iIsReferredOnMta As Integer
    Private m_iPolicyVersion As Integer
    Private m_vGeminiPolicyStatus As Object
    Private m_vGeminiBusinessType As Object
    Private m_vDeferredInd As Object
    Private m_vPolicyIgnore As Object
    Private m_vBrokerCnt As Object
    Private m_vRiskCodeID As Object
    Private m_vAnalysisCodeID As Object
    Private m_vPolicyDeductiblesID As Object
    Private m_vPolicyLimitsID As Object
    Private m_vProposalDate As Object
    Private m_vDiaryDate As Object
    Private m_vReviewDate As Object
    Private m_vRenewalDayNumber As Object
    Private m_vPolicyTypeId As Object
    Private m_vIndicator As Object
    Private m_vClause As Object
    Private m_vCover As Object
    Private m_vArea As Object
    Private m_vLongTermUndertakingDate As Object
    Private m_vRenewalStopCodeID As Object
    Private m_vVBSType As Object
    Private m_vVBSStatus As Object
    Private m_vIsInsurerRateTable As Object
    Private m_vIsRelatedPolicies As Object
    Private m_vIsRetainedDocuments As Object
    Private m_vSchemesPostcode As Object
    Private m_vPaidDirect As Object
    Private m_vScheme As Object
    Private m_vBrokerageAmount As Object
    Private m_vIsMinimumBrokerageFlag As Object
    Private m_vAnnualPremium As Object
    Private m_vThisPremium As Object
    Private m_vNetPremium As Object
    Private m_vCommissionAmount As Object
    Private m_vIPTableAmount As Object
    Private m_vIPTPercentage As Object
    Private m_vIsIPTOverridden As Object
    Private m_vTaxAmount As Object
    Private m_vVatableAmount As Object
    Private m_vVatPercentage As Object
    Private m_vVatAmount As Object
    Private m_vPaymentMethod As Object
    Private m_vUserDefinedDataID As Object

    'TN20001127 - Doc Ref 10 (Start)
    Private m_vInsuredName As Object
    Private m_vAlternateReference As Object
    Private m_vIsClientInvoiced As Object
    Private m_vOldPolicyNumber As Object
    Private m_vQuoteExpiryDate As Object
    Private m_vAlternateAccountCnt As Object
    'TN20001127 - Doc Ref 10 (End)

    ' DataBase Attributes for Insurance_File_System
    Private m_lEndorsementCount As Integer
    Private m_iCreatedByID As Integer
    Private m_dtDateCreated As Date
    Private m_vModifiedByID As Object
    Private m_vLastModified As Object
    Private m_vLastTransDate As Object
    Private m_vLastTransTypeID As Object
    Private m_vLastTransDescription As Object
    Private m_vLastTransDebitCredit As Object
    Private m_vLastTransDocumentRef As Object
    Private m_vLastTransCoverStartDate As Object
    Private m_vLastTransExpiryDate As Object

    ' DataBase Attributes for Insurance_Folder
    Private m_lInsuranceFolderID As Integer
    Private m_lInsuranceHolderCnt As Integer
    Private m_sCode As String = ""
    Private m_vDescription As Object
    Private m_vInceptionDate As Object
    Private m_vArcArchiveFolderID As Object
    Private m_vQuoteInsuranceRef As Object
    Private m_vNextInsuranceRef As Object
    Private m_vLastInsuranceRef As Object
    Private m_lRenewalCount As Integer

    ' DataBase Attributes for Insurance File Entity
    Private m_sInsuranceHolder As String = ""

    'If Data Transfer In Use
    Private m_iDataTransfer As Integer
    ' AMB 24-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added
    Private m_vAnniversaryDate As Object

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property


    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

            'Not Required For Data Transfer
            'DC 24/11/99
            If m_iDataTransfer = gPMConstants.PMEReturnCode.PMFalse Then
                ' Set related business objects
                m_lReturn = CType(GetDetails(), gPMConstants.PMEReturnCode)
            End If

        End Set
    End Property

    Public ReadOnly Property InsuranceFileStructureID() As Integer
        Get

            Return m_lInsuranceFileStructureID

        End Get
    End Property

    Public ReadOnly Property InsuranceFileTypeID() As Integer
        Get

            Return m_lInsuranceFileTypeID

        End Get
    End Property

    Public ReadOnly Property InsuranceFileStatusID() As Integer
        Get

            Return m_vInsuranceFileStatusID

        End Get
    End Property

    Public ReadOnly Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
    End Property

    Public ReadOnly Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
    End Property

    Public ReadOnly Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
    End Property

    Public ReadOnly Property InsuranceRef() As String
        Get

            Return m_sInsuranceRef

        End Get
    End Property

    Public ReadOnly Property ProductID() As Integer
        Get

            Return m_lProductID

        End Get
    End Property

    Public ReadOnly Property LeadInsurerCnt() As Integer
        Get

            Return m_lLeadInsurerCnt

        End Get
    End Property

    Public ReadOnly Property LeadAgentCnt() As Integer
        Get

            Return m_lLeadAgentCnt

        End Get
    End Property

    Public ReadOnly Property LeadAgentPercent() As Integer
        Get

            Return m_vLeadAgentPercent

        End Get
    End Property

    Public ReadOnly Property AccountHandlerCnt() As Integer
        Get

            Return m_lAccountHandlerCnt

        End Get
    End Property

    Public ReadOnly Property InsuredCnt() As Integer
        Get

            Return m_lInsuredCnt

        End Get
    End Property

    Public ReadOnly Property BusinessTypeID() As Integer
        Get

            Return m_iBusinessTypeID

        End Get
    End Property

    Public ReadOnly Property CollectTypeID() As Integer
        Get

            Return m_vCollectTypeID

        End Get
    End Property

    Public ReadOnly Property CollectionFromCnt() As Integer
        Get

            Return m_lCollectionFromCnt

        End Get
    End Property

    'Public Property Get BranchID() As Integer
    '
    '    BranchID = m_iBranchID%
    '
    'End Property
    Public ReadOnly Property SubBranchID() As Integer
        Get

            Return m_vSubBranchID

        End Get
    End Property
    Public ReadOnly Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
    End Property

    Public ReadOnly Property LanguageID() As Integer
        Get

            Return m_iLanguageID

        End Get
    End Property

    Public ReadOnly Property DateIssued() As Integer
        Get

            Return m_vDateIssued

        End Get
    End Property

    Public ReadOnly Property CoverStartDate() As Date
        Get

            Return m_dtCoverStartDate

        End Get
    End Property

    Public ReadOnly Property ExpiryDate() As Date
        Get

            Return m_dtExpiryDate

        End Get
    End Property

    Public ReadOnly Property RenewalDate() As Date
        Get

            Return m_dtRenewalDate

        End Get
    End Property

    Public ReadOnly Property RenewalMethodID() As Integer
        Get

            Return m_vRenewalMethodID

        End Get
    End Property

    Public ReadOnly Property RenewalFrequencyID() As Integer
        Get

            Return m_iRenewalFrequencyID

        End Get
    End Property

    Public ReadOnly Property IsReferredAtRenewal() As Integer
        Get

            Return m_iIsReferredAtRenewal

        End Get
    End Property

    Public ReadOnly Property LapsedReasonID() As Integer
        Get

            Return m_vLapsedReasonID

        End Get
    End Property

    Public ReadOnly Property LapsedDate() As Integer
        Get

            Return m_vLapsedDate

        End Get
    End Property

    Public ReadOnly Property LapsedDescription() As Integer
        Get

            Return m_vLapsedDescription

        End Get
    End Property

    Public ReadOnly Property IsReferredOnMta() As Integer
        Get

            Return m_iIsReferredOnMta

        End Get
    End Property

    Public ReadOnly Property QuoteInsuranceRef() As Object
        Get

            Return m_vQuoteInsuranceRef

        End Get
    End Property

    Public ReadOnly Property NextInsuranceRef() As Object
        Get

            Return m_vNextInsuranceRef

        End Get
    End Property

    Public ReadOnly Property LastInsuranceRef() As Object
        Get

            Return m_vLastInsuranceRef

        End Get
    End Property

    Public ReadOnly Property EndorsementCount() As Integer
        Get

            Return m_lEndorsementCount

        End Get
    End Property

    Public ReadOnly Property RenewalCount() As Integer
        Get

            Return m_lRenewalCount

        End Get
    End Property

    Public ReadOnly Property CreatedByID() As Integer
        Get

            Return m_iCreatedByID

        End Get
    End Property

    Public ReadOnly Property DateCreated() As Date
        Get

            Return m_dtDateCreated

        End Get
    End Property

    Public ReadOnly Property ModifiedByID() As Object
        Get

            Return m_vModifiedByID

        End Get
    End Property

    Public ReadOnly Property LastModified() As Object
        Get

            Return m_vLastModified

        End Get
    End Property

    Public ReadOnly Property LastTransDate() As Object
        Get

            Return m_vLastTransDate

        End Get
    End Property

    Public ReadOnly Property LastTransTypeID() As Object
        Get

            Return m_vLastTransTypeID

        End Get
    End Property

    Public ReadOnly Property LastTransDescription() As Object
        Get

            Return m_vLastTransDescription

        End Get
    End Property

    Public ReadOnly Property LastTransDebitCredit() As Object
        Get

            Return m_vLastTransDebitCredit

        End Get
    End Property

    Public ReadOnly Property LastTransDocumentRef() As Object
        Get

            Return m_vLastTransDocumentRef

        End Get
    End Property

    Public ReadOnly Property LastTransCoverStartDate() As Object
        Get

            Return m_vLastTransCoverStartDate

        End Get
    End Property

    Public ReadOnly Property LastTransExpiryDate() As Object
        Get

            Return m_vLastTransExpiryDate

        End Get
    End Property

    Public ReadOnly Property InsuranceFolderID() As Integer
        Get

            Return m_lInsuranceFolderID

        End Get
    End Property

    Public ReadOnly Property InsuranceHolderCnt() As Integer
        Get

            Return m_lInsuranceHolderCnt

        End Get
    End Property

    Public ReadOnly Property Code() As String
        Get

            Return m_sCode

        End Get
    End Property

    Public ReadOnly Property Description() As Object
        Get

            Return m_vDescription

        End Get
    End Property

    Public ReadOnly Property InceptionDate() As Object
        Get

            Return m_vInceptionDate

        End Get
    End Property

    Public ReadOnly Property ArcArchiveFolderID() As Object
        Get

            Return m_vArcArchiveFolderID

        End Get
    End Property

    Public ReadOnly Property InsuranceHolder() As String
        Get

            Return m_sInsuranceHolder

        End Get
    End Property

    Public ReadOnly Property AnniversaryDate() As Integer
        Get
            ' AMB 24-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added
            Return m_vAnniversaryDate

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of InsuranceFile Business class
            m_oSIRInsuranceFile = New Business()

            m_lReturn = CType(m_oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of InsuranceFileSystem Business class
            m_oSIRInsuranceFileSystem = New bSIRInsuranceFileSystem.Business()

            m_lReturn = m_oSIRInsuranceFileSystem.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of InsuranceFolder Business class
            m_oSIRInsuranceFolder = New bSIRInsuranceFolder.Business()

            m_lReturn = m_oSIRInsuranceFolder.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oSIRInsuranceFile IsNot Nothing Then
                    m_oSIRInsuranceFile.Dispose()

                End If
                m_oSIRInsuranceFile = Nothing
                If m_oSIRInsuranceFileSystem IsNot Nothing Then
                    m_oSIRInsuranceFileSystem.Dispose()
                End If
                m_oSIRInsuranceFileSystem = Nothing
                If m_oSIRInsuranceFolder IsNot Nothing Then
                    m_oSIRInsuranceFolder.Dispose()
                End If
                m_oSIRInsuranceFolder = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDetails (Private)
    '
    ' Description: Gets the required SIRInsuranceFile entity properties.
    '
    ' ***************************************************************** '
    Private Function GetDetails() As Integer

        Dim result As Integer = 0
        'Developer Guide No 21. 
        Dim oFields As DataRow
        Dim vFieldArray As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the InsuranceFile Properties
        With m_oSIRInsuranceFile
            m_lReturn = CType(.GetDetails(, m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)

            m_lReturn = CType(.GetNext(r_vFieldArray:=vFieldArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            m_lReturn = CType(SetProperties(v_vFieldArray:=vFieldArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            vFieldArray = Nothing

        End With

        ' Set the InsuranceFileSystem Properties
        With m_oSIRInsuranceFileSystem
            m_lReturn = .GetDetails(, m_lInsuranceFileCnt)

            m_lReturn = .GetNext(vEndorsementCount:=m_lEndorsementCount, vCreatedByID:=m_iCreatedByID, vDateCreated:=m_dtDateCreated, vModifiedByID:=m_vModifiedByID, vLastModified:=m_vLastModified, vLastTransDate:=m_vLastTransDate, vLastTransTypeID:=m_vLastTransTypeID, vLastTransDescription:=m_vLastTransDescription, vLastTransDebitCredit:=m_vLastTransDebitCredit, vLastTransDocumentRef:=m_vLastTransDocumentRef, vLastTransCoverStartDate:=m_vLastTransCoverStartDate, vLastTransExpiryDate:=m_vLastTransExpiryDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        ' Set the InsuranceFolder Properties
        With m_oSIRInsuranceFolder
            m_lReturn = .GetDetails(, m_lInsuranceFolderCnt)

            m_lReturn = .GetNext(vInsuranceFolderID:=m_lInsuranceFolderID, vInsuranceHolderCnt:=m_lInsuranceHolderCnt, vCode:=m_sCode, vDescription:=m_vDescription, vInceptionDate:=m_vInceptionDate, vArcArchiveFolderID:=m_vArcArchiveFolderID, vQuoteInsuranceRef:=m_vQuoteInsuranceRef, vNextInsuranceRef:=m_vNextInsuranceRef, vLastInsuranceRef:=m_vLastInsuranceRef, vRenewalCount:=m_lRenewalCount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        ' Get Entity Details

        ' Clear the Database Parameters Collection
        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllEntityDetailsSQL, sSQLName:=ACGetAllEntityDetailsName, bStoredProcedure:=ACGetAllEntityDetailsStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            If m_oDatabase.Records.Count() < 1 Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set oFields to refer to one Record
            oFields = .Records.Item(0).Fields()

            ' Set properties from current record
            With oFields
                m_sInsuranceHolder = gPMFunctions.NullToString(oFields("insurance_holder"))
            End With
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIRInsuranceFile property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByVal v_vFieldArray() As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        'Developer Guide No. 98
        m_lInsuranceFileCnt = v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt)

        m_lInsuranceFileStructureID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)

        m_lInsuranceFileTypeID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)

        m_vInsuranceFileStatusID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)

        m_lInsuranceFileID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileID)

        m_iSourceID = v_vFieldArray(InsuranceFileConst.ACSourceID)

        m_lInsuranceFolderCnt = v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt)


        m_sInsuranceRef = v_vFieldArray(InsuranceFileConst.ACInsuranceRef)

        m_lProductID = v_vFieldArray(InsuranceFileConst.ACProductID)

        m_lLeadInsurerCnt = v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)

        m_lLeadAgentCnt = v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)

        m_vLeadAgentPercent = v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)

        m_lAccountHandlerCnt = v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)

        m_lInsuredCnt = v_vFieldArray(InsuranceFileConst.ACInsuredCnt)

        m_iBusinessTypeID = v_vFieldArray(InsuranceFileConst.ACBusinessTypeID)

        m_vCollectTypeID = v_vFieldArray(InsuranceFileConst.ACCollectTypeID)

        m_lCollectionFromCnt = v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)
        'sj 19/07/2002 - start


        m_vSubBranchID = v_vFieldArray(InsuranceFileConst.ACSubBranchID)
        'sj 19/07/2002 - end

        m_iCurrencyID = v_vFieldArray(InsuranceFileConst.ACCurrencyID)

        m_iLanguageID = v_vFieldArray(InsuranceFileConst.ACLanguageID)

        m_vDateIssued = v_vFieldArray(InsuranceFileConst.ACDateIssued)

        m_dtCoverStartDate = v_vFieldArray(InsuranceFileConst.ACCoverStartDate)

        m_dtExpiryDate = v_vFieldArray(InsuranceFileConst.ACExpiryDate)

        m_dtRenewalDate = v_vFieldArray(InsuranceFileConst.ACRenewalDate)

        m_vRenewalMethodID = v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)

        m_iRenewalFrequencyID = v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)

        m_iIsReferredAtRenewal = v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal)

        m_vLapsedReasonID = v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)

        m_vLapsedDate = v_vFieldArray(InsuranceFileConst.ACLapsedDate)

        m_vLapsedDescription = v_vFieldArray(InsuranceFileConst.ACLapsedDescription)

        m_iIsReferredOnMta = v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta)

        m_iPolicyVersion = v_vFieldArray(InsuranceFileConst.ACPolicyVersion)

        m_vGeminiPolicyStatus = v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)

        m_vGeminiBusinessType = v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)

        m_vDeferredInd = v_vFieldArray(InsuranceFileConst.ACDeferredInd)

        m_vPolicyIgnore = v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)

        m_vBrokerCnt = v_vFieldArray(InsuranceFileConst.ACBrokerCnt)

        m_vRiskCodeID = v_vFieldArray(InsuranceFileConst.ACRiskCodeId)

        m_vAnalysisCodeID = v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)

        m_vPolicyDeductiblesID = v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)

        m_vPolicyLimitsID = v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)

        m_vProposalDate = v_vFieldArray(InsuranceFileConst.ACProposalDate)

        m_vDiaryDate = v_vFieldArray(InsuranceFileConst.ACDiaryDate)

        m_vReviewDate = v_vFieldArray(InsuranceFileConst.ACReviewDate)

        m_vRenewalDayNumber = v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)

        m_vPolicyTypeId = v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)

        m_vIndicator = v_vFieldArray(InsuranceFileConst.ACIndicator)

        m_vClause = v_vFieldArray(InsuranceFileConst.ACClause)

        m_vCover = v_vFieldArray(InsuranceFileConst.ACCover)

        m_vArea = v_vFieldArray(InsuranceFileConst.ACArea)

        m_vLongTermUndertakingDate = v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)

        m_vRenewalStopCodeID = v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)

        m_vVBSType = v_vFieldArray(InsuranceFileConst.ACVBSType)

        m_vVBSStatus = v_vFieldArray(InsuranceFileConst.ACVBSStatus)

        m_vIsInsurerRateTable = v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)

        m_vIsRelatedPolicies = v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)

        m_vIsRetainedDocuments = v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)

        m_vSchemesPostcode = v_vFieldArray(InsuranceFileConst.ACSchemesPostcode)

        m_vPaidDirect = v_vFieldArray(InsuranceFileConst.ACPaidDirect)

        m_vScheme = v_vFieldArray(InsuranceFileConst.ACScheme)

        m_vBrokerageAmount = v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)

        m_vIsMinimumBrokerageFlag = v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag)

        m_vAnnualPremium = v_vFieldArray(InsuranceFileConst.ACAnnualPremium)

        m_vThisPremium = v_vFieldArray(InsuranceFileConst.ACThisPremium)

        m_vNetPremium = v_vFieldArray(InsuranceFileConst.ACNetPremium)

        m_vCommissionAmount = v_vFieldArray(InsuranceFileConst.ACCommissionAmount)

        m_vIPTableAmount = v_vFieldArray(InsuranceFileConst.ACIPTableAmount)

        m_vIPTPercentage = v_vFieldArray(InsuranceFileConst.ACIPTPercentage)

        m_vIsIPTOverridden = v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)

        m_vTaxAmount = v_vFieldArray(InsuranceFileConst.ACTaxAmount)

        m_vVatableAmount = v_vFieldArray(InsuranceFileConst.ACVatableAmount)

        m_vVatPercentage = v_vFieldArray(InsuranceFileConst.ACVatPercentage)

        m_vVatAmount = v_vFieldArray(InsuranceFileConst.ACVatAmount)

        m_vPaymentMethod = v_vFieldArray(InsuranceFileConst.ACPaymentMethod)

        m_vUserDefinedDataID = v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)


        m_vInsuredName = v_vFieldArray(InsuranceFileConst.ACInsuredName)

        m_vAlternateReference = v_vFieldArray(InsuranceFileConst.ACAlternateReference)

        m_vIsClientInvoiced = v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)

        m_vOldPolicyNumber = v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)

        m_vQuoteExpiryDate = v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)

        m_vAlternateAccountCnt = v_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt)

        m_vAnniversaryDate = v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)

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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

