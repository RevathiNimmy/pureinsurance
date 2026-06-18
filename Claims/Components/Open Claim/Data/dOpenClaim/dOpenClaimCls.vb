Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
'Developer Guide no 129
<System.Runtime.InteropServices.ProgId("OpenClaim_NET.OpenClaim")>
Public NotInheritable Class OpenClaim
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: dOpenClaim
    '
    ' Date: 14-JUN-00
    '
    ' Description: Describes the OpenClaim attributes.
    '
    ' Edit History: Written by Sravan Kumar.G
    '               Pandu (24-07-2000)
    '
    ' Jude Killip   17/04/2001:     All dates data types changed from string to variant
    '                               and converted to date (cdate()) before update
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    Private m_sSiriusProduct As String = ""

    ' Constant for the functions to identify which class this is.

    Private Const ACClass As String = "OpenClaim"
    'Added Claim_id -Pandu
    Private m_lClaimid As Integer
    Private m_sClaimNo As String = ""
    Private m_sPolicyNo As String = ""
    Private m_lPolicyID As Integer
    Private m_sDescription As String = ""
    Private m_lClaimStatusID As Integer
    Private m_lProgressStatusID As Integer
    Private m_lPrimaryCauseID As Integer
    Private m_lSecondaryCauseID As Integer
    Private m_lCatastropheCodeID As Integer
    Private m_sLossFromDate As Object
    Private m_sLossToDate As Object
    Private m_sReportedDate As Object
    Private m_sReportedToDate As Object
    Private m_sLastModifiedDate As Object
    Private m_lHandlerID As Integer
    Private m_lCurrencyID As Integer
    Private m_nInfoOnly As Integer
    Private m_nLikelyClaim As Integer
    Private m_sLocation As String = ""
    Private m_lTown As Integer
    Private m_lRiskTypeID As Integer
    Private m_sClientName As String = ""

    'Private m_sClientAddress As String
    'Changed Datatype to long -Pandu
    Private m_sClientAddress As Integer
    Private m_sClientTelNo As String = ""
    Private m_sClientFaxNo As String = ""
    Private m_sClientMobileNo As String = ""
    Private m_sClientEMail As String = ""
    Private m_sClientClaimNo As String = ""
    Private m_sInsurerName As String = ""

    'Private m_sInsurerAddress As String
    'Changed Datatype to integer -Pandu

    Private m_sInsurerAddress As Integer
    Private m_sInsurerTelNo As String = ""
    Private m_sInsurerFaxNo As String = ""
    Private m_sInsurerEmail As String = ""
    Private m_sInsurerClaimNo As String = ""
    Private m_sInsurerContact As String = ""
    Private m_nVATRegistered As Integer
    Private m_sVATRegisteredNo As String = ""
    Private m_sComments As String = ""
    'DC240402
    Private m_vClaimComments As Object

    'Added Additional Parameters -Pandu
    Private m_sClaimsStatusDate As Object
    Private m_sClientShortName As String = ""
    Private m_sInsurerShortName As String = ""
    Private m_sClientTelNoOff As String = ""

    Public m_lUserDefFldA As Integer
    Public m_lUserDefFldB As Integer
    Public m_lUserDefFldC As Integer
    Public m_lUserDefFldD As Integer
    Public m_lUserDefFldE As Integer

    Public m_vUnderwritingYearID As Object

    'Stores Claim Handled Value
    Private m_vClaimHandled As Object

    'S4B Claim Enhancements R&D 2005
    Private m_sDriverTitle As String = ""
    Private m_sDriverForename As String = ""
    Private m_sDriverSurname As String = ""
    Private m_vDatePassedTest As Object
    Private m_sEmployeeTitle As String = ""
    Private m_sEmployeeForename As String = ""
    Private m_sEmployeeSurname As String = ""
    Private m_lEmployeeLengthOfService As Integer
    Private m_bEmployeePreviousClaim As Boolean
    Private m_sEmployeePreviousClaimDetails As String = ""
    Private m_bULR As Boolean
    Private m_sRecoveryAgent As String = ""
    Private m_bSolicitorAppointed As Boolean
    Private m_sSolicitorName As String = ""
    Private m_sULRLossDetails As String = ""
    Private m_lClaimAtFaultId As Integer
    Private m_bBonusAffected As Boolean
    Private m_lPolicyDeductibleId As Integer
    Private m_dNonStandardExcess As Double
    Private m_sSubsidiaryCompanyName As String = ""
    Private m_lVersionId As Integer
    Private m_sCaseNumber As String = ""
    Private m_lCaseID As Integer
    Private m_lBaseCaseID As Integer
    Private m_TPA As Integer
    Private m_TPAName As String

    Private m_oSystemOption As bSIROptions.Business

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_iUserOtherPartyID As Integer

    Public Property VersionId() As Integer
        Get
            Return m_lVersionId
        End Get
        Set(ByVal Value As Integer)
            m_lVersionId = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    'DN 27/03/01 - Start
    Public Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property


    Public Property LanguageID() As Integer
        Get
            Return m_iLanguageID
        End Get
        Set(ByVal Value As Integer)
            m_iLanguageID = Value
        End Set
    End Property
    'DN 27/03/01 - End
    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)
            m_oDatabase = Value
        End Set
    End Property
    'Added Insurer Short Name Properties -Pandu

    Private Property InsurerShortName() As String
        Get
            Return m_sInsurerShortName
        End Get
        Set(ByVal Value As String)
            m_sInsurerShortName = Value
        End Set
    End Property
    'Added Client Short Name Properties -Pandu

    Private Property ClientShortName() As String
        Get
            Return m_sClientShortName
        End Get
        Set(ByVal Value As String)
            m_sClientShortName = Value
        End Set
    End Property

    'Added Client Tel Number  office  Properties -Pandu


    Private Property ClientTelNoOff() As String
        Get
            Return m_sClientTelNoOff
        End Get
        Set(ByVal Value As String)
            m_sClientTelNoOff = Value
        End Set
    End Property
    'Added Claims Status Properties -Pandu

    Private Property ClaimsStatusDate() As Object
        Get
            Return m_sClaimsStatusDate
        End Get
        Set(ByVal Value As Object)


            m_sClaimsStatusDate = Value
        End Set
    End Property
    'Added Claimid  Properties -Pandu

    Public Property Claimid() As Integer
        Get
            Return m_lClaimid
        End Get
        Set(ByVal Value As Integer)
            m_lClaimid = Value
        End Set
    End Property

    Public Property UserDefFldA() As Integer
        Get
            Return m_lUserDefFldA
        End Get
        Set(ByVal Value As Integer)
            m_lUserDefFldA = Value
        End Set
    End Property

    Public Property UserDefFldB() As Integer
        Get
            Return m_lUserDefFldB
        End Get
        Set(ByVal Value As Integer)
            m_lUserDefFldB = Value
        End Set
    End Property

    Public Property UserDefFldC() As Integer
        Get
            Return m_lUserDefFldC
        End Get
        Set(ByVal Value As Integer)
            m_lUserDefFldC = Value
        End Set
    End Property

    Public Property UserDefFldD() As Integer
        Get
            Return m_lUserDefFldD
        End Get
        Set(ByVal Value As Integer)
            m_lUserDefFldD = Value
        End Set
    End Property

    Public Property UserDefFldE() As Integer
        Get
            Return m_lUserDefFldE
        End Get
        Set(ByVal Value As Integer)
            m_lUserDefFldE = Value
        End Set
    End Property

    Public Property UnderwritingYearID() As Object
        Get
            Return m_vUnderwritingYearID
        End Get
        Set(ByVal Value As Object)


            m_vUnderwritingYearID = Value
        End Set
    End Property


    Public Property ClaimHandled() As Object
        Get
            Return m_vClaimHandled
        End Get
        Set(ByVal Value As Object)


            m_vClaimHandled = Value
        End Set
    End Property


    Private Property PolicyNo() As String
        Get
            Return m_sPolicyNo
        End Get
        Set(ByVal Value As String)
            m_sPolicyNo = Value
        End Set
    End Property


    Private Property PolicyID() As Integer
        Get
            Return m_lPolicyID
        End Get
        Set(ByVal Value As Integer)
            m_lPolicyID = Value
        End Set
    End Property

    'DC020802 -was Private

    Public Property ClaimNo() As String
        Get
            Return m_sClaimNo
        End Get
        Set(ByVal Value As String)
            m_sClaimNo = Value
        End Set
    End Property


    Private Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property



    Private Property ClientEmail() As String
        Get
            Return m_sClientEMail
        End Get
        Set(ByVal Value As String)
            m_sClientEMail = Value
        End Set
    End Property


    Private Property ClientFaxNo() As String
        Get
            Return m_sClientFaxNo
        End Get
        Set(ByVal Value As String)
            m_sClientFaxNo = Value
        End Set
    End Property


    Private Property ClientTelNo() As String
        Get
            Return m_sClientTelNo
        End Get
        Set(ByVal Value As String)
            m_sClientTelNo = Value
        End Set
    End Property

    'Changed DataType To long - Pandu


    Private Property ClientAddress() As Integer
        Get
            Return m_sClientAddress
        End Get
        Set(ByVal Value As Integer)
            m_sClientAddress = Value
        End Set
    End Property

    Private Property ClaimStatusID() As Integer
        Get
            Return m_lClaimStatusID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimStatusID = Value
        End Set
    End Property


    Private Property ProgressStatusID() As Integer
        Get
            Return m_lProgressStatusID
        End Get
        Set(ByVal Value As Integer)
            m_lProgressStatusID = Value
        End Set
    End Property


    Private Property PrimaryCauseID() As Integer
        Get
            Return m_lPrimaryCauseID
        End Get
        Set(ByVal Value As Integer)
            m_lPrimaryCauseID = Value
        End Set
    End Property


    Private Property SecondaryCauseID() As Integer
        Get
            Return m_lSecondaryCauseID
        End Get
        Set(ByVal Value As Integer)
            m_lSecondaryCauseID = Value
        End Set
    End Property


    Private Property CatastropheCodeID() As Integer
        Get
            Return m_lCatastropheCodeID
        End Get
        Set(ByVal Value As Integer)
            m_lCatastropheCodeID = Value
        End Set
    End Property


    Private Property InsurerEmail() As String
        Get
            Return m_sInsurerEmail
        End Get
        Set(ByVal Value As String)
            m_sInsurerEmail = Value
        End Set
    End Property



    Private Property InsurerContact() As String
        Get
            Return m_sInsurerContact
        End Get
        Set(ByVal Value As String)
            m_sInsurerContact = Value
        End Set
    End Property


    Private Property ClientName() As String
        Get
            Return m_sClientName
        End Get
        Set(ByVal Value As String)
            m_sClientName = Value
        End Set
    End Property


    Private Property RiskTypeID() As Integer
        Get
            Return m_lRiskTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeID = Value
        End Set
    End Property


    Private Property Town() As Integer
        Get
            Return m_lTown
        End Get
        Set(ByVal Value As Integer)
            m_lTown = Value
        End Set
    End Property


    Private Property Location() As String
        Get
            Return m_sLocation
        End Get
        Set(ByVal Value As String)
            m_sLocation = Value
        End Set
    End Property


    Private Property LikelyClaim() As Integer
        Get
            Return m_nLikelyClaim
        End Get
        Set(ByVal Value As Integer)
            m_nLikelyClaim = Value
        End Set
    End Property


    Private Property InfoOnly() As Integer
        Get
            Return m_nInfoOnly
        End Get
        Set(ByVal Value As Integer)
            m_nInfoOnly = Value
        End Set
    End Property


    Private Property HandlerID() As Integer
        Get
            Return m_lHandlerID
        End Get
        Set(ByVal Value As Integer)
            m_lHandlerID = Value
        End Set
    End Property


    Private Property Comments() As String
        Get
            Return m_sComments
        End Get
        Set(ByVal Value As String)
            m_sComments = Value
        End Set
    End Property


    Private Property VATRegistered() As Integer
        Get
            Return m_nVATRegistered
        End Get
        Set(ByVal Value As Integer)
            m_nVATRegistered = Value
        End Set
    End Property


    Private Property InsurerClaimNo() As String
        Get
            Return m_sInsurerClaimNo
        End Get
        Set(ByVal Value As String)
            m_sInsurerClaimNo = Value
        End Set
    End Property


    Private Property InsurerFaxNo() As String
        Get
            Return m_sInsurerFaxNo
        End Get
        Set(ByVal Value As String)
            m_sInsurerFaxNo = Value
        End Set
    End Property


    Private Property InsurerTelNo() As String
        Get
            Return m_sInsurerTelNo
        End Get
        Set(ByVal Value As String)
            m_sInsurerTelNo = Value
        End Set
    End Property

    'Changed Data Type To long - Pandu


    Private Property InsurerAddress() As Integer
        Get
            Return m_sInsurerAddress
        End Get
        Set(ByVal Value As Integer)
            m_sInsurerAddress = Value
        End Set
    End Property

    Private Property ClientClaimNo() As String
        Get
            Return m_sClientClaimNo
        End Get
        Set(ByVal Value As String)
            m_sClientClaimNo = Value
        End Set
    End Property


    Private Property InsurerName() As String
        Get
            Return m_sInsurerName
        End Get
        Set(ByVal Value As String)
            m_sInsurerName = Value
        End Set
    End Property


    Private Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property


    Private Property LossFromDate() As Object
        Get
            Return m_sLossFromDate
        End Get
        Set(ByVal Value As Object)


            m_sLossFromDate = Value
        End Set
    End Property


    Private Property LossToDate() As Object
        Get
            Return m_sLossToDate
        End Get
        Set(ByVal Value As Object)


            m_sLossToDate = Value
        End Set
    End Property


    Private Property ReportedDate() As Object
        Get
            Return m_sReportedDate
        End Get
        Set(ByVal Value As Object)


            m_sReportedDate = Value
        End Set
    End Property


    Private Property ReportedToDate() As Object
        Get
            Return m_sReportedToDate
        End Get
        Set(ByVal Value As Object)


            m_sReportedToDate = Value
        End Set
    End Property


    Private Property LastModifiedDate() As Object
        Get
            Return m_sLastModifiedDate
        End Get
        Set(ByVal Value As Object)


            m_sLastModifiedDate = Value
        End Set
    End Property


    Private Property ClientMobileNo() As String
        Get
            Return m_sClientMobileNo
        End Get
        Set(ByVal Value As String)
            m_sClientMobileNo = Value
        End Set
    End Property


    Private Property VATRegisteredNo() As String
        Get
            Return m_sVATRegisteredNo
        End Get
        Set(ByVal Value As String)
            m_sVATRegisteredNo = Value
        End Set
    End Property
    'DC150402 -Start


    'Private Function ClaimComments() As Object
    'Return m_vClaimComments
    'End Function

    'Private Sub ClaimComments(ByVal Value As Object)


    'm_vClaimComments = Value
    'End Sub
    'DC150402 -End
    'S4B Claim Enhancements R&D 2005

    Private Property DriverTitle() As String
        Get
            Return m_sDriverTitle
        End Get
        Set(ByVal Value As String)
            m_sDriverTitle = Value
        End Set
    End Property


    Private Property DriverForename() As String
        Get
            Return m_sDriverForename
        End Get
        Set(ByVal Value As String)
            m_sDriverForename = Value
        End Set
    End Property


    Private Property DriverSurname() As String
        Get
            Return m_sDriverSurname
        End Get
        Set(ByVal Value As String)
            m_sDriverSurname = Value
        End Set
    End Property


    Private Property DatePassedTest() As Object
        Get
            Return m_vDatePassedTest
        End Get
        Set(ByVal Value As Object)


            m_vDatePassedTest = Value
        End Set
    End Property


    Private Property EmployeeTitle() As String
        Get
            Return m_sEmployeeTitle
        End Get
        Set(ByVal Value As String)
            m_sEmployeeTitle = Value
        End Set
    End Property


    Private Property EmployeeForename() As String
        Get
            Return m_sEmployeeForename
        End Get
        Set(ByVal Value As String)
            m_sEmployeeForename = Value
        End Set
    End Property


    Private Property EmployeeSurname() As String
        Get
            Return m_sEmployeeSurname
        End Get
        Set(ByVal Value As String)
            m_sEmployeeSurname = Value
        End Set
    End Property


    Private Property EmployeeLengthOfService() As Integer
        Get
            Return m_lEmployeeLengthOfService
        End Get
        Set(ByVal Value As Integer)
            m_lEmployeeLengthOfService = Value
        End Set
    End Property


    Private Property EmployeePreviousClaim() As Boolean
        Get
            Return m_bEmployeePreviousClaim
        End Get
        Set(ByVal Value As Boolean)
            m_bEmployeePreviousClaim = Value
        End Set
    End Property


    Private Property EmployeePreviousClaimDetails() As String
        Get
            Return m_sEmployeePreviousClaimDetails
        End Get
        Set(ByVal Value As String)
            m_sEmployeePreviousClaimDetails = Value
        End Set
    End Property


    Private Property ULR() As Boolean
        Get
            Return m_bULR
        End Get
        Set(ByVal Value As Boolean)
            m_bULR = Value
        End Set
    End Property


    Private Property RecoveryAgent() As String
        Get
            Return m_sRecoveryAgent
        End Get
        Set(ByVal Value As String)
            m_sRecoveryAgent = Value
        End Set
    End Property


    Private Property SolicitorAppointed() As Boolean
        Get
            Return m_bSolicitorAppointed
        End Get
        Set(ByVal Value As Boolean)
            m_bSolicitorAppointed = Value
        End Set
    End Property


    Private Property SolicitorName() As String
        Get
            Return m_sSolicitorName
        End Get
        Set(ByVal Value As String)
            m_sSolicitorName = Value
        End Set
    End Property


    Private Property ULRLossDetails() As String
        Get
            Return m_sULRLossDetails
        End Get
        Set(ByVal Value As String)
            m_sULRLossDetails = Value
        End Set
    End Property


    Private Property ClaimAtFaultId() As Integer
        Get
            Return m_lClaimAtFaultId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimAtFaultId = Value
        End Set
    End Property


    Private Property BonusAffected() As Boolean
        Get
            Return m_bBonusAffected
        End Get
        Set(ByVal Value As Boolean)
            m_bBonusAffected = Value
        End Set
    End Property


    Private Property PolicyDeductibleId() As Integer
        Get
            Return m_lPolicyDeductibleId
        End Get
        Set(ByVal Value As Integer)
            m_lPolicyDeductibleId = Value
        End Set
    End Property


    Private Property NonStandardExcess() As Double
        Get
            Return m_dNonStandardExcess
        End Get
        Set(ByVal Value As Double)
            m_dNonStandardExcess = Value
        End Set
    End Property


    Private Property SubsidiaryCompanyName() As String
        Get
            Return m_sSubsidiaryCompanyName
        End Get
        Set(ByVal Value As String)
            m_sSubsidiaryCompanyName = Value
        End Set
    End Property


    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property

    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property


    Public Property BaseCaseID() As Integer
        Get
            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseID = Value
        End Set
    End Property

    Private Property TPA() As Integer
        Get
            Return m_TPA
        End Get
        Set(ByVal Value As Integer)
            m_TPA = Value
        End Set
    End Property

    Private Property TPAName() As String
        Get
            Return m_TPAName
        End Get
        Set(ByVal Value As String)
            m_TPAName = Value
        End Set
    End Property


    ' *****************************************************************'
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' *****************************************************************'
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            'S4B Claim Enhancements R&D 2005
            m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sSiriusProduct), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

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
    ' Name: Add Claim
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '

    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as OUTPUT parameters
            m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' add user details the creation of the version of the record
            AddInputParameter(v_sName:="created_by_id", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParams(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.SQLBeginTrans()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

            m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Primary Key of the record inserted
            m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetClaimNo(), gPMConstants.PMEReturnCode)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC240402 -Start
    ' ***************************************************************** '
    ' Name: Add Claim Comments
    '
    ' Description: Adds to Database
    '
    ' ***************************************************************** '

    Public Function AddClaimComments(ByRef m_vClaimComments As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            For lCount As Integer = m_vClaimComments.GetLowerBound(1) To m_vClaimComments.GetUpperBound(1)

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(m_lClaimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Comment_Id", vValue:=CStr(lCount + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Line", vValue:=CStr(m_vClaimComments(0, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClaimCommentsSQL, sSQLName:=ACAddClaimCommentsName, bStoredProcedure:=ACAddClaimCommentsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Update Claim Comments
    '
    ' Description: Updates Database
    '
    ' ***************************************************************** '
    Public Function UpdateClaimComments(ByRef m_vClaimComments As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(m_lClaimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimCommentsSQL, sSQLName:=ACDeleteClaimCommentsName, bStoredProcedure:=ACDeleteClaimCommentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            For lCount As Integer = m_vClaimComments.GetLowerBound(1) To m_vClaimComments.GetUpperBound(1)

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(m_lClaimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Comment_Id", vValue:=CStr(lCount + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Line", vValue:=CStr(m_vClaimComments(0, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClaimCommentsSQL, sSQLName:=ACAddClaimCommentsName, bStoredProcedure:=ACAddClaimCommentsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -End

    ' ***************************************************************** '
    ' Name: Update Claim
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

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParams(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete Claim    ------   NOT USED CURRENTLY    ------
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

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

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle Claim
    '
    ' Description: Selects the required Claim from the database
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

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
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored) ', |    bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set properties
            'Developer Guide no.162
            m_lReturn = CType(SetPropertiesFromDB(oFields:=m_oDatabase.Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -Start
    ' ***************************************************************** '
    ' Name: Select Comments For Claim
    '
    ' Description: Selects the required Comments from the database
    '
    ' ***************************************************************** '
    Public Function GetClaimComments(ByRef m_vClaimComments As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(Claimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AddInputParams = PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="comment_type", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AddInputParams = PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="entity_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AddInputParams = PMFalse
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimCommentsSQL, sSQLName:=ACGetClaimComments, bStoredProcedure:=ACGetClaimCommentsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If



            m_vClaimComments = vArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -End

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Claim properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guide no.21
    Private Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With oFields
            TPAName = gPMFunctions.NullToString(oFields("other_party_name"))
            TPA = gPMFunctions.NullToInteger(oFields("other_party_id"))
            ClaimNo = gPMFunctions.NullToString(oFields("Claim_Number"))
            PolicyID = gPMFunctions.NullToLong(oFields("Policy_ID"))
            PolicyNo = gPMFunctions.NullToString(oFields("Policy_Number"))
            Description = gPMFunctions.NullToString(oFields("Description"))
            ClaimStatusID = gPMFunctions.NullToLong(oFields("Claim_Status_ID"))
            ProgressStatusID = gPMFunctions.NullToLong(oFields("Progress_Status_ID"))
            PrimaryCauseID = gPMFunctions.NullToLong(oFields("Primary_Cause_ID"))

            ' AMB 23/07/2003: 1.8.6 Data transfer bug fixing - use NullToXXX and
            ' do NOT use vbNull - this gets type-coerced to a value of 1, not zero
            SecondaryCauseID = gPMFunctions.NullToLong(oFields("Secondary_Cause_ID"))
            CatastropheCodeID = gPMFunctions.NullToLong(oFields("Catastrophe_Code_ID"))

            ' AMB 23/07/2003: 1.8.6 Data transfer bug fixing - dates are the only
            ' exception, because they are stored a variants and need to be
            ' displayed as empty strings on screen

            If Convert.IsDBNull(oFields("Loss_From_Date")) Or Informations.IsNothing(oFields("Loss_From_Date")) Then

                LossFromDate = ""
            Else

                LossFromDate = oFields("Loss_From_Date")
            End If


            If Convert.IsDBNull(oFields("Loss_To_Date")) Or Informations.IsNothing(oFields("Loss_To_Date")) Then

                LossToDate = ""
            Else

                LossToDate = oFields("Loss_To_Date")
            End If


            If Convert.IsDBNull(oFields("Reported_Date")) Or Informations.IsNothing(oFields("Reported_Date")) Then

                ReportedDate = ""
            Else

                ReportedDate = oFields("Reported_Date")
            End If


            If Convert.IsDBNull(oFields("Reported_To_Date")) Or Informations.IsNothing(oFields("Reported_To_Date")) Then

                ReportedToDate = ""
            Else

                ReportedToDate = oFields("Reported_To_Date")
            End If


            If Convert.IsDBNull(oFields("Last_Modified_Date")) Or Informations.IsNothing(oFields("Last_Modified_Date")) Then

                LastModifiedDate = ""
            Else

                LastModifiedDate = oFields("Last_Modified_Date")
            End If

            HandlerID = gPMFunctions.NullToLong(oFields("Handler_ID"))
            CurrencyID = gPMFunctions.NullToLong(oFields("Currency_ID"))
            '        InfoOnly = .Item("Info_Only").Value

            If gPMFunctions.NullToBoolean(oFields("Info_Only")) Then
                InfoOnly = 1
            Else
                InfoOnly = 0
            End If

            If gPMFunctions.NullToBoolean(oFields("Likely_Claim")) Then
                LikelyClaim = 1
            Else
                LikelyClaim = 0
            End If

            ' AMB 23/07/2003: 1.8.6 Data transfer bug fixing - use NullToXXX and
            ' do NOT use vbEmpty - this gets type-coerced to a value of 0, not ""
            ' do NOT use vbNull - this gets type-coerced to a value of 1, not zero
            Location = gPMFunctions.NullToString(oFields("Location"))
            Town = gPMFunctions.NullToLong(oFields("Town"))

            RiskTypeID = gPMFunctions.NullToLong(oFields("Risk_Type_ID"))
            ClientName = gPMFunctions.NullToString(oFields("Client_Name"))
            ClientAddress = gPMFunctions.NullToLong(oFields("Client_Address"))

            ' AMB 23/07/2003: 1.8.6 Data transfer bug fixing - use NullToXXX and
            ' do NOT use vbEmpty - this gets type-coerced to a value of 0, not ""
            ' do NOT use vbNull - this gets type-coerced to a value of 1, not ""
            ClientTelNo = gPMFunctions.NullToString(oFields("Client_Tel_No"))
            ClientFaxNo = gPMFunctions.NullToString(oFields("Client_Fax_No"))
            ClientMobileNo = gPMFunctions.NullToString(oFields("Client_Mobile_No"))
            ClientEmail = gPMFunctions.NullToString(oFields("Client_Email"))
            ClientClaimNo = gPMFunctions.NullToString(oFields("Client_Claim_Number"))
            InsurerName = gPMFunctions.NullToString(oFields("Insurer_Name"))
            InsurerAddress = gPMFunctions.NullToLong(oFields("Insurer_Address"))
            InsurerTelNo = gPMFunctions.NullToString(oFields("Insurer_Tel_No"))
            InsurerFaxNo = gPMFunctions.NullToString(oFields("Insurer_Fax_No"))
            InsurerEmail = gPMFunctions.NullToString(oFields("Insurer_EMail"))
            InsurerClaimNo = gPMFunctions.NullToString(oFields("Insurer_Claim_Number"))
            InsurerContact = gPMFunctions.NullToString(oFields("Insurer_Contact"))

            If gPMFunctions.NullToBoolean(oFields("VAT_Registered")) Then
                VATRegistered = 1
            Else
                VATRegistered = 0
            End If

            ' AMB 23/07/2003: 1.8.6 Data transfer bug fixing - use NullToXXX and
            ' do NOT use vbNull - this gets type-coerced to a value of 1, not ""
            VATRegisteredNo = gPMFunctions.NullToString(oFields("VAT_Reg_No"))
            Comments = gPMFunctions.NullToString(oFields("Comments"))

            'Added New DataBase Fields to be fetched -Pandu

            If Convert.IsDBNull(oFields("Claims_Status_Date")) Or Informations.IsNothing(oFields("Claims_Status_Date")) Then

                ClaimsStatusDate = ""
            Else

                ClaimsStatusDate = oFields("Claims_Status_Date")
            End If

            ClientShortName = gPMFunctions.NullToString(oFields("Client_Short_Name"))

            InsurerShortName = gPMFunctions.NullToString(oFields("Insurer_Short_Name"))

            ClientTelNoOff = gPMFunctions.NullToString(oFields("Client_Tel_No_off"))


            If Convert.IsDBNull(oFields("user_defined_field_A")) Or Informations.IsNothing(oFields("user_defined_field_A")) Then
                UserDefFldA = 0
            Else
                UserDefFldA = oFields("user_defined_field_A")
            End If


            If Convert.IsDBNull(oFields("user_defined_field_B")) Or Informations.IsNothing(oFields("user_defined_field_B")) Then
                UserDefFldB = 0
            Else
                UserDefFldB = oFields("user_defined_field_B")
            End If


            If Convert.IsDBNull(oFields("user_defined_field_C")) Or Informations.IsNothing(oFields("user_defined_field_C")) Then
                UserDefFldC = 0
            Else
                UserDefFldC = oFields("user_defined_field_C")
            End If


            If Convert.IsDBNull(oFields("user_defined_field_D")) Or Informations.IsNothing(oFields("user_defined_field_D")) Then
                UserDefFldD = 0
            Else
                UserDefFldD = oFields("user_defined_field_D")
            End If


            If Convert.IsDBNull(oFields("user_defined_field_E")) Or Informations.IsNothing(oFields("user_defined_field_E")) Then
                UserDefFldE = 0
            Else
                UserDefFldE = oFields("user_defined_field_E")
            End If

            'DD 29/03/2004

            UnderwritingYearID = oFields("underwriting_year_id")

            VersionId = oFields("version_id")



            CaseNumber = gPMFunctions.ToSafeString(oFields("case_number"))
            CaseID = gPMFunctions.ToSafeLong(oFields("base_case_id"))


        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParams (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' Date       :24-07-2000
    '
    ' Edit History :Pandu -(Added New Parameters)
    ' ***************************************************************** '
    Private Function AddInputParams() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '1
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Policy_ID", vValue:=CStr(PolicyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '2
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Policy_Number", vValue:=PolicyNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '3
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=Description, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '4
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Status_ID", vValue:=CStr(ClaimStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '5
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Progress_Status_ID", vValue:=CStr(ProgressStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '6
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Primary_Cause_ID", vValue:=CStr(PrimaryCauseID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '7
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Secondary_Cause_ID", vValue:=CStr(SecondaryCauseID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '8
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Catastrophe_Code_ID", vValue:=CStr(CatastropheCodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '9
        'JMK 17/04/2001
        If Not Informations.IsDate(LossFromDate) Then

            'Developer Guide No.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Loss_From_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Loss_From_Date", vValue:=LossFromDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '10
        'TN20002311 (Start)
        If Not Informations.IsDate(LossToDate) Then

            'Developer Guide No.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Loss_To_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'DC09032001 added CDate
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Loss_To_Date", vValue:=LossToDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If

        '        m_lReturn& =m_oDatabase.Parameters.Add( _
        ''         sName:="Loss_To_Date", _
        ''         vValue:=LossToDate, _
        ''         iDirection:=PMParamInput, _
        ''         iDataType:=PMString)

        'TN20002311 (End)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '11
        'JMK 17/04/2001
        If Not Informations.IsDate(ReportedDate) Then

            'Developer Guide No.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Reported_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Reported_Date", vValue:=ReportedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '12
        'DC09032001 added CDate
        'JMK 17/04/2001
        If Not Informations.IsDate(ReportedToDate) Then

            'Developer Guide No.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Reported_To_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Reported_To_Date", vValue:=ReportedToDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '13
        'JMK 17/04/2001
        If Not Informations.IsDate(LastModifiedDate) Then

            'Developer Guide No.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Last_Modified_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Last_Modified_Date", vValue:=LastModifiedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '14
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Handler_ID", vValue:=CStr(HandlerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '15
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_ID", vValue:=CStr(CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '16
        'developer guide no.85
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Info_Only", vValue:=InfoOnly, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '17
        'developer guide no.85
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Likely_Claim", vValue:=LikelyClaim, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '18
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Location", vValue:=Location, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '19
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Town", vValue:=CStr(Town), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '20
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Type_ID", vValue:=CStr(RiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '21
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Name", vValue:=ClientName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Changed Data Type To Long -Pandu
        '22
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Address", vValue:=CStr(ClientAddress), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '23
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Tel_No", vValue:=ClientTelNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '24
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Fax_No", vValue:=ClientFaxNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '25
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Mobile_No", vValue:=ClientMobileNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '26
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_EMail", vValue:=ClientEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '27
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Claim_Number", vValue:=ClientClaimNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '28
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Name", vValue:=InsurerName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Changed Data Type To Long -Pandu
        '29
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Address", vValue:=CStr(InsurerAddress), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '30
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Tel_No", vValue:=InsurerTelNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '31
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Fax_no", vValue:=InsurerFaxNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '32
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_EMail", vValue:=InsurerEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '33
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Claim_Number", vValue:=InsurerClaimNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '34
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Contact", vValue:=InsurerContact, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '35
        m_lReturn = m_oDatabase.Parameters.Add(sName:="VAT_Registered", vValue:=CStr(VATRegistered), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '36
        m_lReturn = m_oDatabase.Parameters.Add(sName:="VAT_Reg_No", vValue:=VATRegisteredNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '37
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Comments", vValue:=Comments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Added ClaimStatus Date Parameter -Pandu
        '38
        'JMK 17/04/2001
        If Not Informations.IsDate(ClaimsStatusDate) Then

            'Developer Guide No.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_Status_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_Status_Date", vValue:=ClaimsStatusDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Added Client Short Name Parameter -Pandu
        '39
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Short_Name", vValue:=ClientShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Added Insurer Short Name Parameter -Pandu
        '40
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Short_Name", vValue:=InsurerShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Added Client Office Telephone Number Parameter -Pandu
        '41
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Tel_No_off", vValue:=ClientTelNoOff, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '42
        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserDefFldA", vValue:=CStr(UserDefFldA), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '43
        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserDefFldB", vValue:=CStr(UserDefFldB), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '44
        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserDefFldC", vValue:=CStr(UserDefFldC), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '45

        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserDefFldD", vValue:=CStr(UserDefFldD), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '46
        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserDefFldE", vValue:=CStr(UserDefFldE), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '47 RWH(10/11/2000) Extra param to pass in generated claim no.
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Number", vValue:=ClaimNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '48 SJP(21/02/2003) Extra param to pass in branch id for system option query.
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_Id", vValue:=CStr(SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '49 DD 29/03/2004 - Underwriting Year.
        m_lReturn = m_oDatabase.Parameters.Add(sName:="underwriting_year_id", vValue:=UnderwritingYearID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="driver_title", vValue:=DriverTitle, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="driver_forename", vValue:=DriverForename, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="driver_surname", vValue:=DriverSurname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="date_passed_test", vValue:=DatePassedTest, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="employee_title", vValue:=EmployeeTitle, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="employee_forename", vValue:=EmployeeForename, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="employee_surname", vValue:=EmployeeSurname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="employee_length_of_service", vValue:=CStr(EmployeeLengthOfService), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="employee_previous_claim", vValue:=CStr(If(EmployeePreviousClaim, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="employee_previous_claim_details", vValue:=EmployeePreviousClaimDetails, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="ulr", vValue:=CStr(If(ULR, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="recovery_agent", vValue:=RecoveryAgent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="solicitor_appointed", vValue:=CStr(If(SolicitorAppointed, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="solicitor_name", vValue:=SolicitorName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="ulr_loss_details", vValue:=ULRLossDetails, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_at_fault_id", vValue:=CStr(ClaimAtFaultId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="bonus_affected", vValue:=CStr(If(BonusAffected, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_deductible_id", vValue:=CStr(PolicyDeductibleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="non_standard_excess", vValue:=CStr(NonStandardExcess), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '50 S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="subsidiary_company_name", vValue:=SubsidiaryCompanyName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '70 AB - Claim Handled
        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_handled", vValue:=ClaimHandled, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = m_oDatabase.Parameters.Add(sName:="tpa", vValue:=CStr(TPA), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If gPMFunctions.ToSafeLong(BaseCaseID) <> 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="base_case_id", vValue:=CStr(BaseCaseID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
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

        '    With m_oDatabase

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(Claimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'AddInputParams = PMFalse
            Return result
        End If

        '    End With

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

        '    With m_oDatabase

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    End With

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

        '    With m_oDatabase


        If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("Claim_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("Claim_id").Value)) Then

            Claimid = m_oDatabase.Parameters.Item("Claim_id").Value
        Else

        End If

        '    End With

        Return result

    End Function
    ' PRIVATE Methods (End)



    Public Sub New()
        MyBase.New()

        ' Class Initialise


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


    ' ***************************************************************** '
    ' Name: SetKeyID (Public)
    '
    ' Description: Sets the Claim No to retrieve the Record
    '
    ' ***************************************************************** '

    Public Function SetKeyID(ByVal vvntClaimId As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Claimid = vvntClaimId


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    Public Function CheckMandatory() As Boolean

        Dim result As Boolean = False

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If False Or False Or False Or False Or False Or False Or False Or False Or False Or False Or False Or False Or False Or False Or False Or False Then

                Return False

            Else

                Return True
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the values from the input params to the Properties
    'AB - Added Claim Handled
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByVal PMMode As Integer, ByVal vvntClaimNo As Object, ByVal vvntPolicyNo As Object, ByVal vvntPolicyID As Object, ByVal vvntDescription As Object, ByVal vvntClaimStatusID As Object, ByVal vvntProgressStatusID As Object, ByVal vvntPrimaryCauseID As Object, ByVal vvntSecondaryCauseID As Object, ByVal vvntCatastropheCodeID As Object, ByVal vvntLossFromDate As Object, ByRef vvntLossToDate As Object, ByVal vvntReportedDate As Object, ByVal vvntReportedToDate As Object, ByVal vvntLastModifiedDate As Object, ByVal vvntHandlerID As Object, ByVal vvntCurrencyID As Object, ByVal vvntInfoOnly As Object, ByVal vvntLikelyClaim As Object, ByVal vvntLocation As Object, ByVal vvntTown As Object, ByVal vvntRiskTypeID As Object, ByVal vvntClientName As Object, ByVal vvntClientAddress As Object, ByVal vvntClientTelNo As Object, ByVal vvntClientFaxNo As Object, ByVal vvntClientMobileNo As Object, ByVal vvntClientEmail As Object, ByVal vvntClientClaimNo As Object, ByVal vvntInsurerName As Object, ByVal vvntInsurerAddress As Object, ByVal vvntInsurerTelNo As Object, ByVal vvntInsurerFaxNo As Object, ByVal vvntInsurerEmail As Object, ByVal vvntInsurerClaimNo As Object, ByVal vvntInsurerContact As Object, ByVal vvntVATRegistered As Object, ByVal vvntVATRegisteredNo As Object, ByVal vvntComments As Object, ByVal vvntClaimsStatusDate As Object, ByVal vvntClientShortName As Object, ByRef vvntInsurerShortName As Object, ByVal vvntClientTelNoOff As Object, ByVal vvntClaimId As Object, ByVal vvntUserDefFldA As Object, ByVal vvntUserDefFldB As Object, ByVal vvntUserDefFldC As Object, ByVal vvntUserDefFldD As Object, ByVal vvntUserDefFldE As Object, ByVal vvntSourceID As Object, ByVal vvntLanguageID As Object, ByVal vvntUnderwritingYearID As Object, ByVal vvntClaimHandled As Object, Optional ByRef v_vBaseCaseID As Object = Nothing, Optional ByVal v_iUserOtherPartyID As Integer = 0) As Object



        ClaimNo = CStr(vvntClaimNo)

        PolicyNo = CStr(vvntPolicyNo)

        PolicyID = CInt(vvntPolicyID)

        Description = CStr(vvntDescription)

        ClaimStatusID = CInt(vvntClaimStatusID)

        ProgressStatusID = CInt(vvntProgressStatusID)

        PrimaryCauseID = CInt(vvntPrimaryCauseID)

        SecondaryCauseID = CInt(vvntSecondaryCauseID)

        CatastropheCodeID = CInt(vvntCatastropheCodeID)
        TPA = v_iUserOtherPartyID

        LossFromDate = CDate(vvntLossFromDate)
        'AJM 02/05/01 Prevent type 13 mismatch if date field = ""
        If vvntLossToDate <> "" Then

            LossToDate = CDate(vvntLossToDate)
        Else

            LossToDate = ""
        End If




        ReportedDate = CDate(vvntReportedDate)


        ReportedToDate = CDate(vvntReportedToDate)
        '    If vvntReportedToDate = "" Then
        '
        '        ReportedToDate = ""
        '    Else
        '
        '        ReportedToDate = vvntReportedToDate
        '
        '    End If



        LastModifiedDate = CDate(vvntLastModifiedDate)

        HandlerID = CInt(vvntHandlerID)

        CurrencyID = CInt(vvntCurrencyID)

        InfoOnly = CInt(vvntInfoOnly)

        LikelyClaim = CInt(vvntLikelyClaim)

        Location = CStr(vvntLocation)

        Town = CInt(vvntTown)

        RiskTypeID = CInt(vvntRiskTypeID)

        ClientName = CStr(vvntClientName)

        ClientAddress = CInt(CStr(vvntClientAddress))

        ClientTelNo = CStr(vvntClientTelNo)

        ClientFaxNo = CStr(vvntClientFaxNo)

        ClientMobileNo = CStr(vvntClientMobileNo)

        ClientEmail = CStr(vvntClientEmail)

        ClientClaimNo = CStr(vvntClientClaimNo)

        InsurerName = CStr(vvntInsurerName)

        InsurerAddress = CInt(CStr(vvntInsurerAddress))

        InsurerTelNo = CStr(vvntInsurerTelNo)

        InsurerFaxNo = CStr(vvntInsurerFaxNo)

        InsurerEmail = CStr(vvntInsurerEmail)

        InsurerClaimNo = CStr(vvntInsurerClaimNo)

        InsurerContact = CStr(vvntInsurerContact)

        VATRegistered = CInt(vvntVATRegistered)

        VATRegisteredNo = CStr(vvntVATRegisteredNo)

        Comments = CStr(vvntComments)

        'Added Properties -Pandu

        If vvntClaimsStatusDate <> "" Then

            ClaimsStatusDate = CDate(vvntClaimsStatusDate)
        Else

            ClaimsStatusDate = ""
        End If

        ClientShortName = CStr(vvntClientShortName)

        InsurerShortName = CStr(vvntInsurerShortName)

        ClientTelNoOff = CStr(vvntClientTelNoOff)

        Claimid = CInt(vvntClaimId)

        'Added User defined properties


        UserDefFldA = CInt(vvntUserDefFldA)

        UserDefFldB = CInt(vvntUserDefFldB)

        UserDefFldC = CInt(vvntUserDefFldC)

        UserDefFldD = CInt(vvntUserDefFldD)

        UserDefFldE = CInt(vvntUserDefFldE)

        'DN 27/03/01 - Add SourceID and Language properties

        SourceID = CInt(vvntSourceID)

        LanguageID = CInt(vvntLanguageID)
        TPA = v_iUserOtherPartyID

        'DD 29/03/2004


        UnderwritingYearID = vvntUnderwritingYearID


        ClaimHandled = vvntClaimHandled


        If Not Informations.IsNothing(v_vBaseCaseID) Then
            BaseCaseID = gPMFunctions.ToSafeLong(v_vBaseCaseID)
        End If

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: SetAdditionalProperties
    '
    ' Description: Populate additional claim properties
    '
    ' History: A.Robinson S4B Claim Enhancements R&D 2005
    ' ***************************************************************** '
    Public Function SetAdditionalProperties(ByVal vvDriverTitle As Object, ByVal vvDriverForename As Object, ByVal vvDriverSurname As Object, ByVal vvDatePassedTest As Object, ByVal vvEmployeeTitle As Object, ByVal vvEmployeeForename As Object, ByVal vvEmployeeSurname As Object, ByVal vvEmployeeLengthOfService As Integer, ByVal vvEmployeePreviousClaim As Object, ByVal vvEmployeePreviousClaimDetails As Object, ByVal vvULR As Object, ByVal vvRecoveryAgent As Object, ByVal vvSolicitorAppointed As Object, ByVal vvSolicitorName As Object, ByVal vvULRLossDetails As Object, ByVal vvClaimAtFaultId As Integer, ByVal vvBonusAffected As Object, ByVal vvPolicyDeductibleId As Integer, ByVal vvNonStandardExcess As Object, ByVal vvSubsidiaryCompanyName As Object) As Integer

        DriverTitle = gPMFunctions.ToSafeString(vvDriverTitle)
        DriverForename = gPMFunctions.ToSafeString(vvDriverForename)
        DriverSurname = gPMFunctions.ToSafeString(vvDriverSurname)


        DatePassedTest = vvDatePassedTest
        EmployeeTitle = gPMFunctions.ToSafeString(vvEmployeeTitle)
        EmployeeForename = gPMFunctions.ToSafeString(vvEmployeeForename)
        EmployeeSurname = gPMFunctions.ToSafeString(vvEmployeeSurname)
        EmployeeLengthOfService = gPMFunctions.ToSafeLong(vvEmployeeLengthOfService)
        EmployeePreviousClaim = gPMFunctions.ToSafeBoolean(vvEmployeePreviousClaim)
        EmployeePreviousClaimDetails = gPMFunctions.ToSafeString(vvEmployeePreviousClaimDetails)
        ULR = gPMFunctions.ToSafeBoolean(vvULR)
        RecoveryAgent = gPMFunctions.ToSafeString(vvRecoveryAgent)
        SolicitorAppointed = gPMFunctions.ToSafeBoolean(vvSolicitorAppointed)
        SolicitorName = gPMFunctions.ToSafeString(vvSolicitorName)
        ULRLossDetails = gPMFunctions.ToSafeString(vvULRLossDetails)
        ClaimAtFaultId = gPMFunctions.ToSafeLong(vvClaimAtFaultId)
        BonusAffected = gPMFunctions.ToSafeBoolean(vvBonusAffected)
        PolicyDeductibleId = gPMFunctions.ToSafeLong(vvPolicyDeductibleId)
        NonStandardExcess = gPMFunctions.ToSafeDouble(vvNonStandardExcess)
        SubsidiaryCompanyName = gPMFunctions.ToSafeString(vvSubsidiaryCompanyName)

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Gets the values from the Properties to the input params
    'AB - Added ClaimHandled
    ' ***************************************************************** '
    Public Function GetProperties(ByVal PMMode As Integer, ByRef rsClaimNo As String, ByRef rsPolicyNo As String, ByRef rlPolicyID As Integer, ByRef rsDescription As String, ByRef rlClaimStatusID As Integer, ByRef rlProgressStatusID As Integer, ByRef rlPrimaryCauseID As Integer, ByRef rlSecondaryCauseID As Integer, ByRef rlCatastropheCodeID As Integer, ByRef rsLossFromDate As Object, ByRef rsLossToDate As Object, ByRef rsReportedDate As Object, ByRef rsReportedToDate As Object, ByRef rsLastModifiedDate As Object, ByRef rlHandlerID As Integer, ByRef rlCurrencyID As Integer, ByRef rnInfoOnly As Integer, ByRef rnLikelyClaim As Integer, ByRef rsLocation As String, ByRef rlTown As Integer, ByRef rlRiskTypeID As Integer, ByRef rsClientName As String, ByRef rsClientAddress As Integer, ByRef rsClientTelNo As String, ByRef rsClientFaxNo As String, ByRef rsClientMobileNo As String, ByRef rsClientEMail As String, ByRef rsClientClaimNo As String, ByRef rsInsurerName As String, ByRef rsInsurerAddress As Integer, ByRef rsInsurerTelNo As String, ByRef rsInsurerFaxNo As String, ByRef rsInsurerEmail As String, ByRef rsInsurerClaimNo As String, ByRef rsInsurerContact As String, ByRef rnVATRegistered As Integer, ByRef rsVATRegisteredNo As String, ByRef rsComments As String, ByRef rsClaimsStatusDate As Object, ByRef rsClientShortName As String, ByRef rsInsurerShortName As String, ByRef rsClientTelNoOff As String, ByRef rsUserDefFldA As Integer, ByRef rsUserDefFldB As Integer, ByRef rsUserDefFldC As Integer, ByRef rsUserDefFldD As Integer, ByRef rsUserDefFldE As Integer, ByRef rsSourceID As Integer, ByRef rsLanguageID As Integer, ByRef rsUnderwritingYearID As Object, ByRef rlVersionID As Integer, ByRef rvClaimHandled As Object, Optional ByRef r_sCaseNumber As String = "", Optional ByRef r_lCaseID As Integer = 0, Optional ByRef otherpartyID As Object = Nothing, Optional ByRef otherpartyName As Object = Nothing) As Object
        otherpartyName = TPAName
        otherpartyID = TPA
        rsClaimNo = ClaimNo
        rsPolicyNo = PolicyNo
        rlPolicyID = PolicyID
        rsDescription = Description
        rlClaimStatusID = ClaimStatusID
        rlProgressStatusID = ProgressStatusID
        rlPrimaryCauseID = PrimaryCauseID
        rlSecondaryCauseID = SecondaryCauseID
        rlCatastropheCodeID = CatastropheCodeID


        rsLossFromDate = LossFromDate


        rsLossToDate = LossToDate


        rsReportedDate = ReportedDate


        rsReportedToDate = ReportedToDate


        rsLastModifiedDate = LastModifiedDate
        rlHandlerID = HandlerID
        rlCurrencyID = CurrencyID
        rnInfoOnly = InfoOnly
        rnLikelyClaim = LikelyClaim
        rsLocation = Location
        rlTown = Town
        rlRiskTypeID = RiskTypeID
        rsClientName = ClientName
        rsClientAddress = ClientAddress
        rsClientTelNo = ClientTelNo
        rsClientFaxNo = ClientFaxNo
        rsClientMobileNo = ClientMobileNo
        rsClientEMail = ClientEmail
        rsClientClaimNo = ClientClaimNo
        rsInsurerName = InsurerName
        rsInsurerAddress = InsurerAddress
        rsInsurerTelNo = InsurerTelNo
        rsInsurerFaxNo = InsurerFaxNo
        rsInsurerEmail = InsurerEmail
        rsInsurerClaimNo = InsurerClaimNo
        rsInsurerContact = InsurerContact
        rnVATRegistered = VATRegistered
        rsVATRegisteredNo = VATRegisteredNo
        rsComments = Comments

        'Added New Properties -Pandu


        rsClaimsStatusDate = ClaimsStatusDate
        rsClientShortName = ClientShortName
        rsInsurerShortName = InsurerShortName
        rsClientTelNoOff = ClientTelNoOff

        'Added for userdefined fields

        rsUserDefFldA = UserDefFldA
        rsUserDefFldB = UserDefFldB
        rsUserDefFldC = UserDefFldC
        rsUserDefFldD = UserDefFldD
        rsUserDefFldE = UserDefFldE

        'DN 27/03/01 - Added SourceID and LanguageID
        rsSourceID = SourceID
        rsLanguageID = LanguageID

        rlVersionID = VersionId

        'DD 29/03/2004


        rsUnderwritingYearID = UnderwritingYearID



        rvClaimHandled = ClaimHandled
        r_sCaseNumber = CaseNumber
        r_lCaseID = CaseID

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: GetAdditionalProperties
    '
    ' Description: Retrieve additional claim properties
    '
    ' History: A.Robinson S4B Claim Enhancements R&D 2005
    ' ***************************************************************** '
    Public Function GetAdditionalProperties(ByRef rsDriverTitle As String, ByRef rsDriverForename As String, ByRef rsDriverSurname As String, ByRef rvDatePassedTest As Object, ByRef rsEmployeeTitle As String, ByRef rsEmployeeForename As String, ByRef rsEmployeeSurname As String, ByRef rlEmployeeLengthOfService As Integer, ByRef rbEmployeePreviousClaim As Boolean, ByRef rsEmployeePreviousClaimDetails As String, ByRef rbULR As Boolean, ByRef rsRecoveryAgent As String, ByRef rbSolicitorAppointed As Boolean, ByRef rsSolicitorName As String, ByRef rsULRLossDetails As String, ByRef rlClaimAtFaultId As Integer, ByRef rbBonusAffected As Boolean, ByRef rlPolicyDeductibleId As Integer, ByRef rdNonStandardExcess As Double, ByRef rsSubsidiaryCompanyName As String) As Integer

        rsDriverTitle = DriverTitle
        rsDriverForename = DriverForename
        rsDriverSurname = DriverSurname


        rvDatePassedTest = DatePassedTest
        rsEmployeeTitle = EmployeeTitle
        rsEmployeeForename = EmployeeForename
        rsEmployeeSurname = EmployeeSurname
        rlEmployeeLengthOfService = EmployeeLengthOfService
        rbEmployeePreviousClaim = EmployeePreviousClaim
        rsEmployeePreviousClaimDetails = EmployeePreviousClaimDetails
        rbULR = ULR
        rsRecoveryAgent = RecoveryAgent
        rbSolicitorAppointed = SolicitorAppointed
        rsSolicitorName = SolicitorName
        rsULRLossDetails = ULRLossDetails
        rlClaimAtFaultId = ClaimAtFaultId
        rbBonusAffected = BonusAffected
        rlPolicyDeductibleId = PolicyDeductibleId
        rdNonStandardExcess = NonStandardExcess
        rsSubsidiaryCompanyName = SubsidiaryCompanyName

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' History: 27/03/01 created by DN based on function in dSIRParty
    ' ***************************************************************** '
    Private Function GetOption(ByRef r_iOptionNumber As Integer, ByRef v_vOptionValues As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oSystemOption = New bSIROptions.Business()

        m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        m_lReturn = m_oSystemOption.GetOptionForAllSources(iOptionNumber:=r_iOptionNumber, vValues:=v_vOptionValues)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' History: 27/03/01 created by DN based on function in dSIRParty
    '
    ' ***************************************************************** '

    'Private Function UpdateFileMaster(ByRef lMode As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim iOptionNumber As Integer
    'Dim vOptionValues(,) As Object
    'Dim bDocumasterEnabled As Boolean
    'Dim sOptionValue As String = ""
    'Dim lPartyCnt As Integer
    'Dim sClaimRef As String = ""
    'Dim lInsuranceFolderCnt As Integer
    'Dim sInsuranceFileRef As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'iOptionNumber = 10
    'sOptionValue = ""
    '
    'm_lReturn = CType(GetOption(r_iOptionNumber:=iOptionNumber, v_vOptionValues:=vOptionValues), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'bDocumasterEnabled = False

    'For 'i As Integer = vOptionValues.GetLowerBound(1) To vOptionValues.GetUpperBound(1)

    'If CStr(vOptionValues(1, i)) = "1" Then
    'bDocumasterEnabled = True
    'Exit For
    'End If
    'Next i
    '
    'Not set up - do nothing
    'If Not (bDocumasterEnabled) Then
    'Return result
    'End If
    '
    'If m_oSIRDOCAPI Is Nothing Then
    'm_oSIRDOCAPI = New bSIRDOCAPI.Form()
    '
    'm_lReturn = m_oSIRDOCAPI.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=SourceID, iLanguageID:=LanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    'End If
    'End If
    '
    'm_lReturn = CType(GetDocumasterPartyCnt(lPartyCnt), gPMConstants.PMEReturnCode)
    'DJM 19/09/2003 : Get insurance_folder_cnt so that the claim is added to the same branch as the policy.
    'm_lReturn = CType(GetDocumasterInsuranceFolderCnt(lInsuranceFolderCnt, sInsuranceFileRef), gPMConstants.PMEReturnCode)
    '
    'sClaimRef = m_sClaimNo & "   " & m_sDescription
    '
    'm_lReturn = m_oSIRDOCAPI.ProcessIndex(lMode:=lMode, iSourceID:=SourceID, lPartyID:=lPartyCnt, sPartyName:=ClientShortName, lInsuranceFolderID:=lInsuranceFolderCnt, sInsuranceFileRef:=sInsuranceFileRef, lClaimid:=Claimid, sClaimRef:=sClaimRef)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process index via DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    'End If
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFileMaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetDocumasterPartyCnt (Private)
    '
    ' Description: Get an option.
    '
    ' History: 27/03/01 created by DN to get party cnt for use in documaster
    ' ***************************************************************** '
    Private Function GetDocumasterPartyCnt(ByRef lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=ClientShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyCntSQL, sSQLName:=ACGetPartyCntName, bStoredProcedure:=ACGetPartyCntStored, lNumberRecords:=0, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any records ?
        If Not Informations.IsArray(vArray) Then
            ' No Records, return PMFalse
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        lPartyCnt = CInt(CStr(vArray(0, 0)).Trim())

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetClaimNo (Private)
    '
    ' Description: Get the claim number for recently added claim
    '
    ' History: 27/03/01 created by DN to get claim number for use in documaster
    ' ***************************************************************** '
    Private Function GetClaimNo() As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(Claimid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimNoSQL, sSQLName:=ACGetClaimNoName, bStoredProcedure:=ACGetClaimNoStored, lNumberRecords:=0, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any records ?
        If Not Informations.IsArray(vArray) Then
            ' No Records, return PMFalse
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        ClaimNo = CStr(vArray(0, 0)).Trim()

        Return result

    End Function

    'DJM 19/09/03
    ' ***************************************************************** '
    ' Name: GetDocumasterInsuranceFolderCnt (Private)
    '
    ' Description: Get the InsuranceFolderCnt
    '
    ' ***************************************************************** '
    Private Function GetDocumasterInsuranceFolderCnt(ByRef r_lInsuranceFolderCnt As Integer, ByRef r_sInsuranceFileRef As String) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lPolicyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFolderCntSQL, sSQLName:=ACGetInsuranceFolderCntName, bStoredProcedure:=ACGetInsuranceFolderCntStored, lNumberRecords:=0, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any records ?
        If Not Informations.IsArray(vArray) Then
            ' No Records, return PMFalse
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        r_lInsuranceFolderCnt = CInt(vArray(0, 0))


        Dim auxVar As Object = vArray(2, 0)



        r_sInsuranceFileRef = CStr(vArray(1, 0)).Trim() & New String(" "c, 3) & (If(Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar), "", CStr(vArray(2, 0)))).Trim()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-03-2006 : Claims Versioning
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
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

    Public Function UpdateClaimPolicyDetails() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '1
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Policy_ID", vValue:=CStr(PolicyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '2
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Policy_Number", vValue:=PolicyNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '3
            If Not Informations.IsDate(LastModifiedDate) Then

                'Developer Guide No.85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Last_Modified_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no.40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Last_Modified_Date", vValue:=LastModifiedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '4
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Name", vValue:=ClientName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '5
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Address", vValue:=CStr(ClientAddress), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '6
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Tel_No", vValue:=ClientTelNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '7
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Fax_No", vValue:=ClientFaxNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '8
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Mobile_No", vValue:=ClientMobileNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '9
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_EMail", vValue:=ClientEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '10
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Claim_Number", vValue:=ClientClaimNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '11
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Name", vValue:=InsurerName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '12
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Address", vValue:=CStr(InsurerAddress), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '13
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Tel_No", vValue:=InsurerTelNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '14
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Fax_no", vValue:=InsurerFaxNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '15
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_EMail", vValue:=InsurerEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '16
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Claim_Number", vValue:=InsurerClaimNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '17
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Contact", vValue:=InsurerContact, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '18
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Short_Name", vValue:=ClientShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '19
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurer_Short_Name", vValue:=InsurerShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '20
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Client_Tel_No_off", vValue:=ClientTelNoOff, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateClaimPolicyDetailsSQL, sSQLName:=ACUpdateClaimPolicyDetailsName, bStoredProcedure:=ACUpdateClaimPolicyDetailsStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check to see that the record was Updated OK
            If lRecordsAffected > 0 Then
                ' Updated No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

