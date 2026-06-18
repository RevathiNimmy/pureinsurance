Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Text
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    'Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 08/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ***************************************************************** '
    ' Class Name: FindParty
    '
    ' Date: 27th September 1996
    '
    ' Description: Creatable FindParty class used by the Find Party
    '              lookup.
    '
    ' Edit History:
    '
    ' DJM 05/07/2002 : Added extra checks for the new source array parameter.
    ' DJM 01/07/2002 : Added source array as parameter to search functions.
    ' SP  01/12/1998 : changes to support new business roadmap
    ' MKR 10/02/2005 : PN 18683 - If a particular agent type is invisible then
    '                  the agents under that agent type must not be displayed.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStructure As String = ""

    'sj 3/11/99 - start
    Private m_lDataSource As Integer
    'Private m_cInvariantKeys As List(Of Object) = New List(Of Object)()
    Private m_cInvariantKeys As Dictionary(Of Integer, Object) = New Dictionary(Of Integer, Object)()


    'Private m_cInvariantKeys As Collection
    Private m_oPMBusiness As Object
    Private m_vPMResultArray As Object
    'sj 3/11/99 - end

    Private m_bOKToDelete As Boolean

    Private m_sNoDeleteReasons As String = ""

    Private m_bCheckedOKToDelete As Boolean

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    'DC160703 -ISS5384
    Private m_lPartySourceId As Integer

    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business


    'TN20000918 - hidden option
    Private m_sUnderwritingOrAgency As String = ""

    'SD 20/02/03
    Private m_bMultiTreeAccounting As Boolean


    'KB PN4881 23062003
    Private m_bEnableBranchSelectAtLogon As Boolean

    Private m_bMultiRestrictClientView As Boolean

    'JMK 18/10/2001 - another hidden option
    Private m_sUnderwritingType As String = ""

    Private m_bIgnoreDriversAndWitnesses As Boolean
    'sj 02/07/2002 - start
    Private m_bRestrictInsurerAccess As Boolean
    Private m_lUserInsurerCnt As Integer
    'sj 02/07/2002 - end

    'SJ 23/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 23/02/2004 - end

    ' RDC 20050901
    Private m_bSuppressCancelledAgents As Boolean

    Private m_oLookup As bPMLookup.Business

    ' Gaurav
    Private m_bIsRetained As Boolean
    Private m_iRetainedValue As Integer
    'developer guide no. 17
    Private m_sReinsuranceTypeArray As Object
    Private m_bIgnoreViewableOnlyAgents As Boolean
    ' Property used to append all the reinsurance types and make generic
    ' search criteria for reinsurance type

    'developer guide no. 17
    Public Property ReinsuranceTypeArray() As Object
        Get
            Return m_sReinsuranceTypeArray
        End Get
        Set(ByVal Value As Object)
            m_sReinsuranceTypeArray = Value
        End Set
    End Property


    ' Gaurav
    ' Property desined to find that whether we have inputed any retained filter
    ' on the query

    Public Property IsRetained() As Boolean
        Get
            Return m_bIsRetained
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRetained = Value
        End Set
    End Property


    Public Property RetainedValue() As Integer
        Get
            Return m_iRetainedValue
        End Get
        Set(ByVal Value As Integer)
            m_iRetainedValue = Value
        End Set
    End Property
    Public Property IgnoreViewableOnlyAgents() As Boolean
        Get
            Return m_bIgnoreViewableOnlyAgents
        End Get
        Set(ByVal Value As Boolean)
            m_bIgnoreViewableOnlyAgents = Value
        End Set
    End Property
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    'DC160703 -ISS5384 -start

    Public Property PartySourceId() As Integer
        Get

            Return m_lPartySourceId

        End Get
        Set(ByVal Value As Integer)

            m_lPartySourceId = Value

        End Set
    End Property
    'DC160703 -ISS5384 -end
    'sj 3/11/99 - start
    Public WriteOnly Property DataSource() As Integer
        Set(ByVal Value As Integer)
            m_lDataSource = Value
        End Set
    End Property
    'sj 3/11/99 - end

    Public ReadOnly Property OKToDelete() As Boolean
        Get

            If Not m_bCheckedOKToDelete Then
                m_lReturn = CheckOKToDelete()
            End If

            Return m_bOKToDelete

        End Get
    End Property

    Public ReadOnly Property NoDeleteReasons() As String
        Get

            Return m_sNoDeleteReasons

        End Get
    End Property


    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
    End Property

    Public Property IgnoreDriversAndWitnesses() As Boolean
        Get
            Return m_bIgnoreDriversAndWitnesses
        End Get
        Set(ByVal Value As Boolean)
            m_bIgnoreDriversAndWitnesses = Value
        End Set
    End Property

    ' RDC 20050901
    Public WriteOnly Property SuppressCancelledAgents() As Boolean
        Set(ByVal Value As Boolean)
            m_bSuppressCancelledAgents = Value
        End Set
    End Property

    Public WriteOnly Property RestrictInsuranceAccess() As Boolean
        Set(ByVal Value As Boolean)
            m_bRestrictInsurerAccess = Value
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Dim vValue As String = ""
            If bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vValue) = gPMConstants.PMEReturnCode.PMTrue Then
                m_bMultiTreeAccounting = (gPMFunctions.NullToString(vValue) = "1")
            Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If



            'KB PN4881

            If bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, m_iSourceID, vValue) = gPMConstants.PMEReturnCode.PMTrue Then
                m_bEnableBranchSelectAtLogon = (gPMFunctions.NullToString(vValue) = "1")
            Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            'SJ 23/02/2004 - start
            'Are we running the folgate branch acting as insurer solution
            m_lReturn = CType(SSP.Shared.bUnderwritingBranchFunc.GetUnderwritingBranchDetails(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase, v_sCallingAppName:=m_sCallingAppName, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            'SJ 23/02/2004 - end

            ' SET 18/04/2007
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiCoRestrictClientView, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue (Restricted Client View) Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_bMultiRestrictClientView = gPMFunctions.ToSafeDouble(vValue) = 1
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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                m_cInvariantKeys = Nothing
                m_oPMBusiness = Nothing

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
    ''' <summary>
    ''' SearchByQuery
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="r_lNumberOfRecords"></param>
    ''' <param name="v_vShortName"></param>
    ''' <param name="v_vName"></param>
    ''' <param name="v_vFileCode"></param>
    ''' <param name="v_vClientType"></param>
    ''' <param name="v_vStatusType"></param>
    ''' <param name="v_vAddress1"></param>
    ''' <param name="v_vPostalCode"></param>
    ''' <param name="v_vAreaCode"></param>
    ''' <param name="v_vNumber"></param>
    ''' <param name="v_vInsuranceRef"></param>
    ''' <param name="v_vDOB"></param>
    ''' <param name="v_vSwiftPartyID"></param>
    ''' <param name="v_vAgentCnt"></param>
    ''' <param name="v_vValidSourceArray"></param>
    ''' <param name="v_vClaimNumber"></param>
    ''' <param name="v_vRiskIndex"></param>
    ''' <param name="v_vAdditionalDataArray"></param>
    ''' <param name="v_vInsuranceFileCnt"></param>
    ''' <param name="v_bIgnoreSourceCheck"></param>
    ''' <param name="bLimitRecords"></param>
    ''' <param name="bIncludeAgent"></param>
    ''' <param name="v_vAgentGroupCnt"></param>
    ''' <param name="v_lNumberOfRecords"></param>
    ''' <param name="v_vCaseNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchByQuery(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer, Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vName As Object = Nothing, Optional ByVal v_vFileCode As Object = Nothing, Optional ByVal v_vClientType As Object = Nothing, Optional ByVal v_vStatusType As Object = Nothing, Optional ByVal v_vAddress1 As Object = Nothing, Optional ByVal v_vPostalCode As Object = Nothing, Optional ByVal v_vAreaCode As Object = Nothing, Optional ByVal v_vNumber As Object = Nothing, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vDOB As Object = Nothing, Optional ByVal v_vSwiftPartyID As Object = Nothing, Optional ByVal v_vAgentCnt As Object = Nothing, Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_vClaimNumber As Object = Nothing, Optional ByVal v_vRiskIndex As Object = Nothing, Optional ByVal v_vAdditionalDataArray As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = 0, Optional ByVal v_bIgnoreSourceCheck As Boolean = False, Optional ByVal bLimitRecords As Boolean = True, Optional ByVal bIncludeAgent As Boolean = False, Optional ByVal v_vAgentGroupCnt As Object = 0, Optional ByVal v_lNumberOfRecords As Integer = -1, Optional ByVal v_vCaseNumber As Object = Nothing, Optional ByVal v_vIsAny As Object = False) As Integer  '(Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.1.2)

        Dim nResult As Integer
        Dim oSQL, oSQLContact As CSelectANSI
        Dim iParamCount As Integer
        Dim sTemp As String = ""
        Dim bParameterDOB As Boolean

        Dim sShortName As String
        Dim sName As String
        Dim sFileCode As String
        Dim sClientType As String
        Dim bIsAnySelected As Boolean
        Dim sStatusType As String
        Dim sAddress1 As String
        Dim sPostalCode As String
        Dim sAreaCode As String
        Dim sNumber As String
        Dim sInsuranceRef As String
        Dim sCaseNumber As String
        Dim oDOB As Object
        Dim nSwiftPartyID As Integer
        Dim nAgentCnt As Integer
        Dim sRiskIndex As String
        Dim sClaimNumber As String
        Dim sGender As String = String.Empty
        Dim sSurname As String = String.Empty
        Dim nAgentGroupCnt As Integer
        Dim nLower, nUpper As Integer

        Dim bUseClientPolicyLinkage As Boolean
        Dim nReturn As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsNothing(v_vAdditionalDataArray) Then
                For nUpper = 0 To v_vAdditionalDataArray.GetUpperBound(1)

                    Select Case CStr(v_vAdditionalDataArray(0, nUpper)).ToLower()
                        Case "surname"

                            sSurname = CStr(v_vAdditionalDataArray(1, nUpper)).Trim()
                            m_lReturn = CType(bPMFunc.ValidateSQL(sSurname), gPMConstants.PMEReturnCode)
                        Case "gender"

                            sGender = CStr(v_vAdditionalDataArray(1, nUpper)).Trim()
                            m_lReturn = CType(bPMFunc.ValidateSQL(sGender), gPMConstants.PMEReturnCode)
                        Case "shortname"

                            v_vShortName = v_vAdditionalDataArray(1, nUpper)
                        Case "name"

                            v_vName = v_vAdditionalDataArray(1, nUpper)
                        Case "filecode"

                            v_vFileCode = v_vAdditionalDataArray(1, nUpper)
                        Case "clientcode"

                            v_vClientType = v_vAdditionalDataArray(1, nUpper)
                        Case "statustype"

                            v_vStatusType = v_vAdditionalDataArray(1, nUpper)
                        Case "address1"

                            v_vAddress1 = v_vAdditionalDataArray(1, nUpper)
                        Case "postalcode"

                            v_vPostalCode = v_vAdditionalDataArray(1, nUpper)
                        Case "areacode"

                            v_vAreaCode = v_vAdditionalDataArray(1, nUpper)
                        Case "number"

                            v_vNumber = v_vAdditionalDataArray(1, nUpper)
                        Case "insuranceref", "policy_no"

                            v_vInsuranceRef = v_vAdditionalDataArray(1, nUpper)
                        Case "dob"

                            v_vDOB = v_vAdditionalDataArray(1, nUpper)
                        Case "swiftpartyid"

                            v_vSwiftPartyID = v_vAdditionalDataArray(1, nUpper)
                        Case "agentcnt"

                            v_vAgentCnt = v_vAdditionalDataArray(1, nUpper)
                        Case "validsourcearray"

                            v_vValidSourceArray = v_vAdditionalDataArray(1, nUpper)
                        Case "claimnumber"

                            v_vClaimNumber = v_vAdditionalDataArray(1, nUpper)
                        Case "riskindex"

                            v_vRiskIndex = v_vAdditionalDataArray(1, nUpper)
                        Case "casenumber"

                            v_vCaseNumber = v_vAdditionalDataArray(1, nUpper)
                    End Select
                Next nUpper
            End If

            If Not Informations.IsNothing(v_vShortName) Then

                sShortName = CStr(v_vShortName).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sShortName), gPMConstants.PMEReturnCode)
            Else
                sShortName = ""
            End If

            If Not Informations.IsNothing(v_vName) Then

                sName = CStr(v_vName).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sName), gPMConstants.PMEReturnCode)
            Else
                sName = ""
            End If

            If Not Informations.IsNothing(v_vFileCode) Then

                sFileCode = CStr(v_vFileCode).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sFileCode), gPMConstants.PMEReturnCode)
            Else
                sFileCode = ""
            End If

            If Not Informations.IsNothing(v_vClientType) Then

                sClientType = CStr(v_vClientType).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sClientType), gPMConstants.PMEReturnCode)
            Else
                sClientType = ""
            End If

            If Not Informations.IsNothing(v_vIsAny) Then

                bIsAnySelected = CStr(v_vIsAny).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(bIsAnySelected), gPMConstants.PMEReturnCode)
            Else
                bIsAnySelected = False
            End If

            If Not Informations.IsNothing(v_vStatusType) Then

                sStatusType = CStr(v_vStatusType).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sStatusType), gPMConstants.PMEReturnCode)
            Else
                sStatusType = ""
            End If

            If Not Informations.IsNothing(v_vAddress1) Then

                sAddress1 = CStr(v_vAddress1).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sAddress1), gPMConstants.PMEReturnCode)
            Else
                sAddress1 = ""
            End If

            If Not Informations.IsNothing(v_vPostalCode) Then

                sPostalCode = CStr(v_vPostalCode).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sPostalCode), gPMConstants.PMEReturnCode)
            Else
                sPostalCode = ""
            End If

            If Not Informations.IsNothing(v_vAreaCode) Then

                sAreaCode = CStr(v_vAreaCode).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sAreaCode), gPMConstants.PMEReturnCode)
            Else
                sAreaCode = ""
            End If

            If Not Informations.IsNothing(v_vNumber) Then

                sNumber = CStr(v_vNumber).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sNumber), gPMConstants.PMEReturnCode)
            Else
                sNumber = ""
            End If

            If Not Informations.IsNothing(v_vInsuranceRef) Then

                sInsuranceRef = CStr(v_vInsuranceRef).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sInsuranceRef), gPMConstants.PMEReturnCode)
            Else
                sInsuranceRef = ""
            End If

            If Not Informations.IsNothing(v_vDOB) Then
                oDOB = v_vDOB
            Else
                oDOB = DBNull.Value
            End If

            If Not Informations.IsNothing(v_vSwiftPartyID) Then

                nSwiftPartyID = CInt(v_vSwiftPartyID)
            Else
                nSwiftPartyID = 0
            End If

            If Not Informations.IsNothing(v_vAgentCnt) Then

                nAgentCnt = CInt(v_vAgentCnt)
            Else
                nAgentCnt = 0
            End If

            nAgentGroupCnt = 0
            If Not False Then
                nAgentGroupCnt = v_vAgentGroupCnt
            End If

            If Not Informations.IsNothing(v_vRiskIndex) Then

                sRiskIndex = CStr(v_vRiskIndex).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sRiskIndex), gPMConstants.PMEReturnCode)
            Else
                sRiskIndex = ""
            End If

            If Not Informations.IsNothing(v_vClaimNumber) Then

                sClaimNumber = CStr(v_vClaimNumber).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sClaimNumber), gPMConstants.PMEReturnCode)
            Else
                sClaimNumber = ""
            End If

            If Not Informations.IsNothing(v_vCaseNumber) Then
                sCaseNumber = CStr(v_vCaseNumber).Trim()
                m_lReturn = CType(bPMFunc.ValidateSQL(sCaseNumber), gPMConstants.PMEReturnCode)
            Else
                sCaseNumber = ""
            End If

            ' Initialise working variables.
            oSQL = New CSelectANSI()
            oSQLContact = New CSelectANSI()
            iParamCount = 1
            Dim iSearchParamCount As Integer = 0 ' tracks only actual search criteria (not party type/status)
            bParameterDOB = False

            ' Create those parts of the query which are common to all conditional branches.
            oSQL.AddField("Party.party_cnt")
            oSQL.AddField("PMCaption.caption")
            oSQL.AddField("Party.shortname")
            oSQL.AddField("Party.resolved_name")
            oSQL.AddField("Address.address1")
            oSQL.AddField("(CASE WHEN Address.postal_code = " &
                          "CONVERT(varchar(20), Address.address_id) THEN '' " &
                          "ELSE Address.postal_code END) AS postal_code")
            oSQL.AddField("Party.source_id")
            oSQL.AddField("Party.party_id")
            If sAreaCode <> "" Or sNumber <> "" Then
                oSQL.AddField("TempContact.area_code")
                oSQL.AddField("TempContact.number")
            Else
                oSQL.AddField("'' as area_code")
                oSQL.AddField("'' as number")
            End If
            oSQL.AddField("Party.is_prospect")
            oSQL.AddField("Party.invariant_key")
            oSQL.AddField("'Sirius'")
            oSQL.AddField("' '")
            oSQL.AddField("case party_type.code when 'AG' then 'Agent' when 'CC' then 'Corporate Client' when 'GC' then 'Group Client' when 'PC' then 'Personal Client' else ' ' end")
            oSQL.AddField("Party.file_code")
            oSQL.AddTable("Party", , , True)
            oSQL.AddTable("Party_Type", "INNER", "Party.party_type_id = Party_Type.party_type_id", True)

            If sClientType <> "OT" Then
                oSQL.AddTable("source", "LEFT OUTER", "Party.source_id = source.source_id", True)
            Else
                oSQL.AddTable("other_party_branch", "LEFT OUTER", "Party.party_cnt = other_party_branch.party_cnt", True)
                oSQL.AddTable("source", "LEFT OUTER", "Party.source_id = source.source_id", True)
            End If

            If sGender <> "" Then
                oSQL.AddTable("Party_lifestyle", "INNER", "Party.party_cnt = Party_lifestyle.party_cnt", True)
            End If

            If m_bRestrictInsurerAccess Then
                'Only show parties which have policies for the nominated insurer
                oSQL.AddTable("insurance_file", "INNER", "Party.party_cnt = insurance_file.insured_cnt", True)
            ElseIf nAgentCnt <> 0 Then
                'Only show parties which have policies for the nominated insurer
                oSQL.AddTable("insurance_file", "LEFT OUTER", "Party.party_cnt = insurance_file.insured_cnt", True)
            End If

            oSQL.AddTable("PMCaption", "INNER", "Party_Type.caption_id = PMCaption.caption_id", True)

            oSQL.AddTable("(Party_Address_Usage " &
                          "INNER JOIN Address WITH(NOLOCK)" &
                          "ON Party_Address_Usage.address_cnt = Address.address_cnt " &
                          "INNER JOIN Address_Usage_Type WITH(NOLOCK) " &
                          "ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id " &
                          "AND Address_Usage_Type.code = '" & gSIRLibrary.SIRMainAddressABICode & "')", "LEFT OUTER", "Party.party_cnt = Party_Address_Usage.party_cnt")

            oSQL.AddFilter("Party.is_deleted = 0")

            If m_bIgnoreDriversAndWitnesses Then
                oSQL.AddFilter("Party_Type.code <> 'OTDRIVER'")
                oSQL.AddFilter("Party_Type.code <> 'OTWITNESS'")
            End If

            If sAreaCode <> "" Or sNumber <> "" Then
                oSQLContact.AddField("Party_Contact_Usage.party_cnt")
                oSQLContact.AddField("MIN(Contact.area_code) AS area_code")
                oSQLContact.AddField("MIN(Contact.number) AS number")
                oSQLContact.AddTable("Party_Contact_Usage", , , True)
                oSQLContact.AddTable("Contact", "INNER", "Party_Contact_Usage.contact_cnt = Contact.contact_cnt", True)
                oSQLContact.AddTable("Contact_Type", "INNER", "Contact.contact_type_id = Contact_Type.contact_type_id", True)
                oSQLContact.AddGroup("Party_Contact_Usage.party_cnt")
            End If

            ' Now do the conditional branches.
            If m_lPartyCnt <> 0 Then
                ' Primary key (party_cnt) provided, so just find that one record.
                oSQL.AddField("(SELECT TOP 1 date_of_birth FROM Party_Lifestyle WITH(NOLOCK) " &
                              "WHERE party_cnt = Party.party_cnt AND category = 1) AS date_of_birth")

                oSQL.AddField("Party.swift_party_id")
                If sAreaCode <> "" Or sNumber <> "" Then
                    oSQL.AddTable("(" & oSQLContact.SQL & ") AS TempContact", "LEFT OUTER", "Party.party_cnt = TempContact.party_cnt")
                End If

                oSQL.AddFilter("Party.party_cnt = " & m_lPartyCnt)

            ElseIf nSwiftPartyID <> 0 Then
                ' Alternate key (swift_party_id) provided, so just find that one record.

                oSQL.AddField("(SELECT TOP 1 date_of_birth FROM Party_Lifestyle WITH(NOLOCK) " &
                              "WHERE party_cnt = Party.party_cnt AND category = 1) AS date_of_birth")

                oSQL.AddField("Party.swift_party_id")
                If sAreaCode <> "" Or sNumber <> "" Then
                    oSQL.AddTable("(" & oSQLContact.SQL & ") AS TempContact", "LEFT OUTER", "Party.party_cnt = TempContact.party_cnt")
                End If

                oSQL.AddFilter("Party.swift_party_id = " & nSwiftPartyID)

            Else
                ' Search based on any optional parameters provided.

                If Informations.IsDate(oDOB) Then
                    oSQL.AddField("{date_of_birth} AS date_of_birth")
                    bParameterDOB = True
                Else
                    oSQL.AddField("(SELECT TOP 1 date_of_birth FROM Party_Lifestyle WITH(NOLOCK) " &
                                  "WHERE party_cnt = Party.party_cnt AND category = 1) AS date_of_birth")
                End If

                oSQL.AddField("Party.swift_party_id")

                oSQL.AddSort("Party.shortname")

                If Informations.IsDate(oDOB) Then
                    iParamCount += 1
                    iSearchParamCount += 1
                    oSQL.AddFilter("EXISTS (SELECT NULL FROM Party_Lifestyle WITH(NOLOCK) " &
                                   "WHERE party_cnt = Party.party_cnt AND category = 1 " &
                                   "AND date_of_birth = {date_of_birth})")
                    bParameterDOB = True
                End If

                If sAreaCode <> "" Or sNumber <> "" Then
                    If sAreaCode <> "" Then
                        iParamCount += 1
                        iSearchParamCount += 1
                        ' The UI validates this as a number so it cannot contain wildcards.
                        oSQLContact.AddFilter("Contact.area_code LIKE '" & sAreaCode & "'")
                    End If
                    If sNumber <> "" Then
                        iParamCount += 1
                        iSearchParamCount += 1
                        ' The UI validates this as a number so it cannot contain wildcards.
                        oSQLContact.AddFilter("Contact.number LIKE '" & sNumber & "'")
                    End If

                    oSQL.AddTable("(" & oSQLContact.SQL & ") AS TempContact", "INNER", "Party.party_cnt = TempContact.party_cnt")
                End If

                If sInsuranceRef <> "" Then
                    iParamCount += 1
                    iSearchParamCount += 1
                    If m_bUnderwritingBranchEnabled And m_bIsUnderwritingBranch Then
                        If sInsuranceRef.IndexOf("%"c) >= 0 Then
                            sTemp = "WHERE (Insurance_File.alternate_reference LIKE '" & sInsuranceRef & "' OR "
                            sTemp = sTemp & "Insurance_File.insurance_ref LIKE '" & sInsuranceRef & "')"
                        Else
                            sTemp = "WHERE (Insurance_File.alternate_reference = '" & sInsuranceRef & "' OR "
                            sTemp = sTemp & "Insurance_File.insurance_ref = '" & sInsuranceRef & "')"
                        End If
                        If m_bIsUnderwritingBranch Then
                            oSQL.AddFilter("EXISTS (SELECT NULL " &
                                           "FROM Insurance_Folder WITH(NOLOCK) INNER JOIN Insurance_File " &
                                           "ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt " &
                                           "INNER JOIN source WITH(NOLOCK) ON source.source_id = Insurance_file.source_id " &
                                           sTemp &
                                           "AND (Insurance_File.insured_cnt = Party.party_cnt " &
                                           "OR Insurance_File.collection_from_cnt = Party.party_cnt " &
                                           "OR Insurance_Folder.insurance_holder_cnt = Party.party_cnt) " &
                                           "AND insurance_file.source_id = source.source_id " &
                                           "AND (source.underwriting_branch_ind = 1 OR insurance_file.policy_type_id = 3 or insurance_file.source_id = " & CStr(m_iSourceID) & "))")
                        Else
                            oSQL.AddFilter("EXISTS (SELECT NULL " &
                                           "FROM Insurance_Folder WITH(NOLOCK) INNER JOIN Insurance_File " &
                                           "ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt " &
                                           "INNER JOIN source WITH(NOLOCK) ON source.source_id = Insurance_file.source_id " &
                                           sTemp &
                                           "AND (Insurance_File.insured_cnt = Party.party_cnt " &
                                           "OR Insurance_File.collection_from_cnt = Party.party_cnt " &
                                           "OR Insurance_Folder.insurance_holder_cnt = Party.party_cnt) " &
                                           "AND (source.underwriting_branch_ind = 0 or source.underwriting_branch_ind is NULL)" & ") ")
                        End If
                    Else
                        If sInsuranceRef.IndexOf("%"c) >= 0 Then
                            sTemp = "WHERE Insurance_File.insurance_ref LIKE '" & sInsuranceRef & "'"
                            oSQL.AddFilter("Insurance_File.insurance_ref LIKE '" & sInsuranceRef & "'")
                            
                            'oSQL.AddFilter("Insurance_File.insurance_ref LIKE '" & sInsuranceRef & "'" &
                                      ' " AND (insured_cnt = party.party_cnt OR collection_from_cnt = party.party_cnt) ")
                        Else
                            sTemp = "WHERE Insurance_File.insurance_ref = '" & sInsuranceRef & "'"
                            oSQL.AddFilter("Insurance_File.insurance_ref = '" & sInsuranceRef & "'")
                       
                        'oSQL.AddFilter("(EXISTS (SELECT * FROM insurance_file WITH(NOLOCK) " &
                                   'sTemp & " AND (insured_cnt = party.party_cnt OR collection_from_cnt = party.party_cnt))) ")                                  
                        End If
                    End If
                End If

                If sShortName <> "" Then
                    sShortName = Convert.ToString(sShortName).Trim
                    iParamCount += 1
                    iSearchParamCount += 1
                    If sShortName.IndexOf("%"c) >= 0 Then
                        oSQL.AddFilter("(Party.shortname LIKE '" & sShortName & "'" _
                               & " OR Party.Alternative_identifier LIKE '" & sShortName & "')")
                    Else

                        oSQL.AddFilter("(Party.shortname = '" & sShortName & "'" _
                               & " OR Party.Alternative_identifier = '" & sShortName & "')")

                    End If
                End If

                If sName <> "" Then
                    sName = Convert.ToString(sName).Trim
                    iParamCount += 1
                    iSearchParamCount += 1
                    If sName.IndexOf("%"c) >= 0 Then

                        oSQL.AddFilter("(((Party.resolved_name LIKE '" & sName & "'" _
                      & " AND ISNULL(party.resolved_name,'') <> '')" _
                        & " OR (party.name LIKE '" & sName & "'))" _
                        & " OR Party.trading_name LIKE '" & sName & "')")

                    Else

                        oSQL.AddFilter("(((Party.resolved_name = '" & sName & "'" _
                        & " AND ISNULL(party.resolved_name,'') <> '')" _
                        & " OR (party.name = '" & sName & "'))" _
                        & " OR Party.trading_name = '" & sName & "')")

                    End If
                End If

                If sFileCode <> "" Then
                    iParamCount += 1
                    iSearchParamCount += 1
                    If sFileCode.IndexOf("%"c) >= 0 Then
                        oSQL.AddFilter("Party.file_code LIKE '" & sFileCode & "'")
                    Else
                        oSQL.AddFilter("Party.file_code = '" & sFileCode & "'")
                    End If
                End If


                If sAddress1 <> "" Then
                    iParamCount += 1
                    iSearchParamCount += 1
                    If sAddress1.IndexOf("%"c) >= 0 Then
                        oSQL.AddFilter(" (Address.address1 LIKE '" & sAddress1 & "'" & " OR Address.address2 LIKE '" & sAddress1 & "')")
                    Else
                        oSQL.AddFilter("(Address.address1 = '" & sAddress1 & "'" & " OR Address.address2 = '" & sAddress1 & "')")
                    End If
                End If
                If sPostalCode <> "" Then
                    iParamCount += 1
                    iSearchParamCount += 1
                    If sPostalCode.IndexOf("%"c) >= 0 Then
                        oSQL.AddFilter("Address.postal_code LIKE '" & sPostalCode & "'")
                    Else
                        oSQL.AddFilter("Address.postal_code = '" & sPostalCode & "'")
                    End If
                    ' Ignore postcode search when there isn't one (postcode = address id)
                    oSQL.AddFilter("CONVERT(varchar(20), Address.address_id) <> Address.postal_code")
                End If

                If nAgentGroupCnt <> 0 Then
                    iParamCount += 1
                    iSearchParamCount += 1

                    oSQL.Distinct = True

                    'Added filter for the Agent Group Association
                    sTemp = "(Party.Agent_Cnt IN (SELECT party_cnt FROM party_agent  WHERE linked_account_group =" & nAgentGroupCnt & ")"
                    sTemp = sTemp & " Or Insurance_file.lead_agent_cnt IN (SELECT party_cnt FROM party_agent  WHERE linked_account_group =" & CStr(nAgentGroupCnt) & "))"

                    oSQL.AddFilter(sTemp)
                Else
                    If nAgentCnt <> 0 Then
                        iParamCount += 1
                        iSearchParamCount += 1
                        oSQL.Distinct = True

                        'Added filter for the lead Agent_Cnt on the Insurance File so that
                        'all parties with policies owned by the agent are returned.
                        sTemp = "(Party.Agent_Cnt = " & nAgentCnt
                        sTemp = sTemp & " Or Insurance_file.lead_agent_cnt = " & CStr(nAgentCnt) & ")"

                        oSQL.AddFilter(sTemp)
                    End If
                End If

                If sClientType.ToUpper() = "<ALL>" Then
                    iParamCount += 1

                    sTemp = "'" & PMBPartyTypePersonalClient & "'" &
                    ",'" & PMBPartyTypeCorporateClient & "'" &
                    ",'" & PMBPartyTypeGroupClient & "'"

                    If m_sStructure <> gSIRLibrary.SIRPMBSolution Then
                        sTemp = sTemp & ",'" & PMBConst.PMBPartyTypeAgent & "'"
                    End If
                    If bIncludeAgent Then
                        sTemp = sTemp & ",'" & PMBConst.PMBPartyTypeAgent & "'"
                    End If
                    oSQL.AddFilter("Party_Type.code in (" & sTemp & ")")
                ElseIf sClientType.IndexOf(PARTY_TYPE_DELIMITER) >= 0 Then
                    If FormatPartyList(sClientType) = gPMConstants.PMEReturnCode.PMTrue Then
                        iParamCount += 1
                        oSQL.AddFilter("Party_Type.code in (" & sClientType & ")")
                    Else
                        gPMFunctions.RaiseError(ACClass, "Call to method FormatPartyList failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                ElseIf sClientType <> "" Then
                    ' If Party Type is 'Other' then include composite code
                    ' passed from interface.
                    iParamCount += 1
                    If sClientType.Substring(0, 2) = PMBConst.PMBPartyTypeOther Then
                        If sClientType.Length = 2 Then
                            oSQL.AddFilter("Party_Type.code LIKE '" & sClientType & "%'")
                        Else
                            oSQL.AddFilter("Party_Type.code = '" & sClientType & "'")
                        End If
                    Else
                        Select Case sClientType
                            Case PMBConst.PMBPartyTypePersonalClientText
                                oSQL.AddFilter("Party_Type.code = '" & PMBConst.PMBPartyTypePersonalClient & "'")
                            Case PMBConst.PMBPartyTypeAgentText
                                oSQL.AddFilter("Party_Type.code = '" & PMBConst.PMBPartyTypeAgent & "'")
                                If IgnoreViewableOnlyAgents Then
                                    oSQL.AddFilter("isnull(Party_Agent.is_viewable_only,0) = 0")
                                End If
                            Case PMBConst.PMBPartyTypeCorporateClientText
                                oSQL.AddFilter("Party_Type.code = '" & PMBConst.PMBPartyTypeCorporateClient & "'")
                            Case PMBConst.PMBPartyTypeGroupClientText
                                If bIsAnySelected = True Then
                                    oSQL.AddFilter("Party_Type.code IN ('" & PMBConst.PMBPartyTypePersonalClient & "'" & " , '" & PMBConst.PMBPartyTypeCorporateClient & "')")
                                Else
                                    oSQL.AddFilter("Party_Type.code = '" & PMBConst.PMBPartyTypeGroupClient & "'")
                                End If
                            Case PMBConst.PMBPartyTypeConsultantText
                                oSQL.AddFilter("Party_Type.code = '(" & PMBConst.PMBPartyTypeConsultant & "' OR Party_Type.code = '" & PMBConst.PMBPartyTypeExecutiveHandler & "')")
                            Case PMBConst.PMBPartyTypeAccountHandlerText
                                oSQL.AddFilter("Party_Type.code = '(" & PMBConst.PMBPartyTypeAccountHandler & "' OR Party_Type.code = '" & PMBConst.PMBPartyTypeExecutiveHandler & "')")
                            Case PMBConst.PMBPartyTypeInsurerText
                                oSQL.AddFilter("Party_Type.code = '" & PMBConst.PMBPartyTypeInsurer & "'")
                            Case PMBConst.PMBPartyTypeBrokerText
                                oSQL.AddFilter("Party_Type.code = '" & PMBConst.PMBPartyTypeBroker & "'")
                            Case Else
                                oSQL.AddFilter("Party_Type.description = '" & sClientType & "'")
                        End Select
                    End If
                End If

                If sStatusType.ToUpper() <> "<ALL>" Then
                    iParamCount += 1
                    Select Case sStatusType.ToUpper
                        Case PMBConst.PMBProspectTypeProspectText.ToUpper
                            oSQL.AddFilter("Party.is_prospect = 1")
                        Case PMBConst.PMBProspectTypeClientText.ToUpper
                            oSQL.AddFilter("Party.is_prospect = 0")
                    End Select
                End If

                Dim sCaseClaimQuery As String
                If sClaimNumber <> "" Then

                    m_lReturn = CType(bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClientPolicyLinkage, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=nReturn), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve Product Option", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")
                        Return nResult
                    End If

                    If nReturn = "" Then
                        nReturn = CStr(0)
                    End If
                    bUseClientPolicyLinkage = CBool(nReturn)

                End If

                'CMG/PB 03092002 Bug fix 315 LIKE not =
                If bUseClientPolicyLinkage Then
                    sCaseClaimQuery = "Party.Party_cnt IN (SELECT PC.party_cnt FROM policy_client PC WITH(NOLOCK) " &
                                "INNER JOIN insurance_file F WITH(NOLOCK) ON PC.Insurance_Folder_Cnt = F.Insurance_Folder_Cnt "

                Else
                    sCaseClaimQuery = "Party.Party_cnt IN (SELECT F.insured_cnt FROM insurance_file F WITH(NOLOCK) "

                End If
                If (sClaimNumber <> "") And (sCaseNumber <> "") Then
                    sCaseClaimQuery = sCaseClaimQuery +
                                          "INNER JOIN claim C WITH(NOLOCK) ON F.Insurance_File_Cnt = C.policy_id AND C.claim_number LIKE '" & sClaimNumber & "' " &
                                          "INNER JOIN [case] WITH(NOLOCK) ON [Case].base_case_id = c.base_case_id AND [case].case_number LIKE '" & sCaseNumber & "') "
                    oSQL.AddFilter(sCaseClaimQuery)
                ElseIf (sClaimNumber = "") And (sCaseNumber <> "") Then
                    sCaseClaimQuery = sCaseClaimQuery +
                                   "INNER JOIN claim C WITH(NOLOCK) ON F.Insurance_File_Cnt = C.policy_id " &
                                   "INNER JOIN [case] WITH(NOLOCK) ON [Case].base_case_id = c.base_case_id AND [case].case_number LIKE '" & sCaseNumber & "') "
                    oSQL.AddFilter(sCaseClaimQuery)
                ElseIf (sClaimNumber <> "") And (sCaseNumber = "") Then
                    sCaseClaimQuery = sCaseClaimQuery +
                                         "INNER JOIN claim C WITH(NOLOCK) ON F.Insurance_File_Cnt = C.policy_id AND C.claim_number LIKE '" & sClaimNumber & "') "
                    oSQL.AddFilter(sCaseClaimQuery)
                End If
                If sSurname <> "" Then
                    oSQL.AddFilter("Party.name " & (If((sSurname.IndexOf("%"c) + 1), "LIKE ", "=")) & "'" & sSurname & "'")
                End If

                If sGender <> "" Then
                    oSQL.AddFilter("Party_lifestyle.gender_code " & (If((sGender.IndexOf("%"c) + 1), "LIKE ", "=")) & "'" & sGender & "'")
                End If

                If iSearchParamCount = 0 Then
                    'no search parameters passed - use stored procedure for performance
                    Return SearchByNoFilter(r_vResultArray, r_lNumberOfRecords, bLimitRecords, v_lNumberOfRecords)
                End If

                If iParamCount = 1 Then
                    'no parameters passed so query cannot be executed
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If Not v_bIgnoreSourceCheck Then

                If Not Informations.IsNothing(v_vValidSourceArray) Then
                    If Informations.IsArray(v_vValidSourceArray) And Not m_bMultiTreeAccounting Then

                        nLower = v_vValidSourceArray.GetLowerBound(1)

                        nUpper = v_vValidSourceArray.GetUpperBound(1)

                        For iLoop As Integer = nLower To nUpper

                            If iLoop = nLower Then
                                If sClientType <> "OT" Then
                                    sTemp = "(party.source_id IN ("
                                Else
                                    sTemp = "(other_party_branch.source_id IN ("
                                End If

                            End If
                            sTemp = sTemp & CStr(Val(CStr(v_vValidSourceArray(0, iLoop))))

                            If iLoop = nUpper Then
                                sTemp = sTemp & "))"
                                oSQL.AddFilter(sTemp)
                            Else
                                sTemp = sTemp & ","
                            End If
                        Next

                    End If
                End If
            End If

            'Only show parties which have policies for the nominated insurer
            If m_bRestrictInsurerAccess And m_lUserInsurerCnt > 0 Then
                oSQL.Distinct = True
                oSQL.AddFilter("insurance_file.lead_insurer_cnt = " & m_lUserInsurerCnt)
            End If

            If Not Informations.IsNothing(v_vInsuranceFileCnt) Then
                If m_bRestrictInsurerAccess And v_vInsuranceFileCnt > 0 Then
                    oSQL.Distinct = True
                    oSQL.AddFilter("insurance_file.insurance_file_cnt = " & v_vInsuranceFileCnt)
                End If
            End If

            oSQL.AddField("Address.address2")

            oSQL.AddField("' '") 'ref issue 20344
            'This will give (Closed) if the branch is closed
            oSQL.AddField(" (CASE source.is_deleted WHEN 1 THEN Rtrim(source.description)" &
                              " + ' (Closed)' WHEN 0 THEN source.description END) AS description ")

            'Added to Agent_Cnt to return result set.
            oSQL.AddField("Party.Agent_Cnt")

            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            ' Add parameters if required

            If bParameterDOB Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="date_of_birth", vValue:=CStr(oDOB), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter : date_of_birth", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")
                    Return nResult
                End If
            End If

            oSQL.AddField("record_status")

            oSQL.AddField("party_type.code")

            oSQL.AddField("Party_Agent.date_cancelled")
            oSQL.AddTable("Party_Agent", "LEFT OUTER", "Party.party_cnt = Party_Agent.party_cnt", True)
            oSQL.AddField("party_net_data.online_status")
            oSQL.AddTable("party_net_data", "LEFT OUTER", "Party.party_cnt = party_net_data.party_cnt", True)
            oSQL.AddTable("Party_Personal_Client", "LEFT OUTER", "Party.party_cnt = Party_Personal_Client.party_cnt")
            If m_bRestrictInsurerAccess = False And nAgentCnt = 0 Then
                oSQL.AddTable("Insurance_File", "LEFT OUTER", "(Insurance_File.insured_cnt = party.party_cnt OR Insurance_File.collection_from_cnt = party.party_cnt)")
            End If
            'To search from Insurance_File if insurance_ref contains '%'
            If sInsuranceRef.IndexOf("%"c) >= 0 Then
                oSQL.AddTable("( SELECT DISTINCT insured_cnt, collection_from_cnt, insurance_ref FROM Insurance_File WHERE Insurance_File.insurance_ref LIKE '" & sInsuranceRef & "') INF ", "INNER", "(INF.insured_cnt = party.party_cnt OR INF.collection_from_cnt = party.party_cnt)")

            End If
            oSQL.AddField("(CASE WHEN Party.Party_type_id='1' THEN Party.name ELSE '' END)AS name")
            oSQL.AddField("(CASE WHEN Party.Party_type_id='1' THEN Party_Personal_Client.forename ELSE '' END)AS forename")
            oSQL.AddField("Address.address3")
            oSQL.AddField("Address.address4")
            oSQL.AddField("Party.currency_id")
            oSQL.AddField("Party.domiciled_for_tax")
            oSQL.AddField(" '' ")
            If m_bMultiTreeAccounting And m_bMultiRestrictClientView Then
                oSQL.AddFilter("Party.source_id = " & m_iSourceID)
            End If
            oSQL.AddField("SL.Code as ServiceLevelCode")
            oSQL.AddField("SL.Description as ServiceLevelDescription")
            oSQL.AddTable("Service_Level AS SL", "LEFT OUTER", "Party.service_level_id = SL.service_level_id")

            ' Max 500 records
            Dim lMaxRecords As Integer
            If bLimitRecords Then
                If v_lNumberOfRecords = -1 Then
                    lMaxRecords = 500
                Else
                    lMaxRecords = v_lNumberOfRecords
                End If
            Else
                lMaxRecords = gPMConstants.PMAllRecords
            End If

            m_lError = m_oDatabase.SQLSelect(sSQL:=oSQL.SQL, sSQLName:=ACPartyFromQueryName, bStoredProcedure:=ACPartyFromQueryStored, lNumberRecords:=lMaxRecords, vResultArray:=r_vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMNotFound
            If Not Informations.IsArray(r_vResultArray) And m_lDataSource = PMSearchSirius Then
                nResult = gPMConstants.PMEReturnCode.PMNotFound
            End If

            If m_lDataSource = PMSearchPMB Then
                r_vResultArray = Nothing
            End If

            'If m_lDataSource = PMSearchPMB Or m_lDataSource = PMSearchSiriusPMB Then
            '    m_lReturn = CType(SearchPolicyMaster(r_lNumberOfRecords:=r_lNumberOfRecords, r_vResultArray:=r_vResultArray, v_vShortName:=v_vShortName, v_vName:=v_vName, v_vClientType:=v_vClientType, v_vStatusType:=v_vStatusType, v_vAddress1:=v_vAddress1, v_vPostalCode:=v_vPostalCode, v_vAreaCode:=v_vAreaCode, v_vNumber:=v_vNumber, v_vInsuranceRef:=v_vInsuranceRef), gPMConstants.PMEReturnCode)

            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        Return m_lReturn
            '    End If
            'End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' SearchByNoFilter - Calls spu_FindParty_NoFilter stored procedure
    ''' when user searches from portal without any filter criteria.
    ''' </summary>
    Private Function SearchByNoFilter(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer, Optional ByVal bLimitRecords As Boolean = True, Optional ByVal v_lNumberOfRecords As Integer = -1) As Integer

        Dim nResult As Integer

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim lMaxRecords As Integer
            If bLimitRecords Then
                lMaxRecords = If(v_lNumberOfRecords = -1, 500, v_lNumberOfRecords)
            Else
                lMaxRecords = 99999
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="MaxRecords", vValue:=CStr(lMaxRecords), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter : MaxRecords", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByNoFilter")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bMultiTreeAccounting AndAlso m_bMultiRestrictClientView Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="SourceId", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="SourceId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter : SourceId", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByNoFilter")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACPartyNoFilterSQL, sSQLName:=ACPartyNoFilterName, bStoredProcedure:=ACPartyNoFilterStored, vResultArray:=r_vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed with error: " & CStr(m_lError), vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByNoFilter")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByNoFilter Failed: " & excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByNoFilter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Private Function FormatPartyList(ByRef sPartyList As String) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "FormatPartyList"



        Dim sList() As String
        Dim sNewList As New StringBuilder

        result = gPMConstants.PMEReturnCode.PMTrue

        sList = sPartyList.Split(New String() {PARTY_TYPE_DELIMITER}, StringSplitOptions.None)

        If Informations.IsArray(sList) Then

            For Each sList_item As String In sList

                Select Case sList_item
                    Case PMBConst.PMBPartyTypePersonalClientText
                        sNewList.Append("'" & PMBConst.PMBPartyTypePersonalClient & "',")
                    Case PMBConst.PMBPartyTypeAgentText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeAgent & "',")
                    Case PMBConst.PMBPartyTypeCorporateClientText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeCorporateClient & "',")
                    Case PMBConst.PMBPartyTypeGroupClientText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeGroupClient & "',")
                    Case PMBConst.PMBPartyTypeConsultantText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeConsultant & "','" & PMBConst.PMBPartyTypeExecutiveHandler & "',")
                    Case PMBConst.PMBPartyTypeAccountHandlerText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeAccountHandler & "','" & PMBConst.PMBPartyTypeExecutiveHandler & "',")
                    Case PMBConst.PMBPartyTypeInsurerText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeInsurer & "',")
                    Case PMBConst.PMBPartyTypeBrokerText
                        sNewList.Append("'" & PMBConst.PMBPartyTypeBroker & "',")
                End Select

            Next sList_item

            If sNewList.ToString().EndsWith(",") Then
                sNewList = New StringBuilder(sNewList.ToString().Substring(0, sNewList.ToString().Length - 1))
            End If

        End If

        sPartyList = sNewList.ToString()

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckOKToDelete
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CheckOKToDelete() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sString As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_bOKToDelete = True

        'There are 6 reasons in PMB why a client cannot be deleted
        '1. Outstanding account records

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLiveTransactionDetailsSQL, sSQLName:="GetLiveTransactionDetailsName", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Live Transactions exist for this client"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        '2. Outstanding claims
        'No claims system as yet - but will need to check a system option when there is

        '3. Travel schemes
        'Not relevant

        '4. Live Policies (including being a sharer of premium)

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLivePolicyDetailsSQL, sSQLName:=ACGetLivePolicyDetailsName, bStoredProcedure:=ACGetLivePolicyDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Live Policies exist for this client"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSharedPremiumDetailsSQL, sSQLName:=ACGetSharedPremiumDetailsName, bStoredProcedure:=ACGetSharedPremiumDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "This client shares a premium on policies owned by other clients"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        '5. Sub Agent
        'Not relevant - but may be when Associated Clients is finally sorted.

        '6. Life Policies
        'Not relevant

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteParty
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteParty() As Integer

        Dim result As Integer = 0
        Dim lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeletePartySQL, sSQLName:=ACDeletePartyName, bStoredProcedure:=ACDeletePartyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=DBNull.Value, v_vInsuranceFileCnt:=DBNull.Value, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventDelClient, v_dtEventDate:=DateTime.Now, v_vDescription:=DBNull.Value), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UndeleteParty
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UndeleteParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUndeletePartySQL, sSQLName:=ACUndeletePartyName, bStoredProcedure:=ACUndeletePartyStored)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UndeleteParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UndeleteParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'sj 2/11/99 - start

    '****************************************************************** '
    ' Name: SearchPolicyMaster (Private)
    '
    ' Description:
    '
    '****************************************************************** '
    'developer guide no. 101
    'Private Function SearchPolicyMaster(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer, Optional ByVal v_vShortName As Object = "", Optional ByVal v_vName As Object = "", Optional ByVal v_vClientType As Object = "", Optional ByVal v_vStatusType As Object = "", Optional ByVal v_vAddress1 As Object = "", Optional ByVal v_vPostalCode As Object = "", Optional ByVal v_vAreaCode As Object = "", Optional ByVal v_vNumber As Object = 0, Optional ByVal v_vInsuranceRef As Object = 0) As Integer
    '    Dim result As Integer = 0

    '    'developer guide no. 17
    '    Dim vArray(,) As Object = Nothing



    '    result = gPMConstants.PMEReturnCode.PMTrue

    '    ' PM Business
    '    If m_oPMBusiness Is Nothing Then
    '        'deepak_todo:to be handled later
    '        'm_oPMBusiness = New bGIIPMBusiness.Business()
    '        m_oPMBusiness = New Object

    '        m_lReturn = Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            result = m_lReturn
    '            m_oPMBusiness = Nothing
    '            Return result
    '        End If
    '    End If

    '    ' Need a shortname or postal code to do the search
    '    If v_vShortName = "" And v_vPostalCode = "" Then
    '        Return result
    '    End If

    '    v_vShortName = v_vShortName.Trim().ToUpper()
    '    v_vPostalCode = v_vPostalCode.Trim().ToUpper()

    '    m_lReturn = CType(GetSearchDetailsFromLink(v_vShortName:=v_vShortName, v_vName:=v_vName, v_vAddress1:=v_vAddress1, v_vPostalCode:=v_vPostalCode, r_vResultArray:=vArray), gPMConstants.PMEReturnCode)

    '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        Return m_lReturn
    '    End If

    '    If m_lDataSource = PMSearchPMB Then
    '        'developer guide no. 12
    '        r_vResultArray = Nothing
    '    End If

    '    If Not Informations.IsArray(r_vResultArray) Then
    '        ' No data from sirius therefore nothing to merge
    '        'Just put this array into it
    '        r_vResultArray = vArray
    '        vArray = Nothing
    '        Return result
    '    End If

    '    ' Index any rows in the array which have an invariant key
    '    'deepak_tod:to be handled later
    '    'm_lReturn = CType(IndexByInvariantKey("UnicodeEncoding.Unicode.GetBytes"(r_vResultArray)), gPMConstants.PMEReturnCode)
    '    m_lReturn = IndexByInvariantKey(r_vResultArray:=r_vResultArray)

    '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        Return gPMConstants.PMEReturnCode.PMFalse
    '    End If

    '    m_lReturn = CType(MergeData(v_vPMArray:=vArray, r_vResultArray:=r_vResultArray, r_lNumberOfRecords:=r_lNumberOfRecords), gPMConstants.PMEReturnCode)

    '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        Return gPMConstants.PMEReturnCode.PMFalse
    '    End If

    '    Return result

    'End Function

    ' ***************************************************************** '
    ' Name: GetSearchDetailsFromLink
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'Private Function GetSearchDetailsFromLink(ByRef r_vResultArray(,) As Object, Optional ByVal v_vShortName As String = "", Optional ByVal v_vName As String = "", Optional ByVal v_vAddress1 As String = "", Optional ByVal v_vPostalCode As String = "") As Integer

    '    Dim result As Integer = 0
    '    Dim iFieldCount As Integer
    '    Dim lReturnValue As Integer
    '    Dim iNumber, j As Integer
    '    Dim bMatch As Boolean



    '    result = gPMConstants.PMEReturnCode.PMTrue


    '    'developer guide no. 12
    '    m_vPMResultArray = Nothing

    '    If v_vShortName <> "" Then


    '        m_lReturn = m_oPMBusiness.GetSchema(sSchema:="GIICLIENT", iFieldCount:=iFieldCount, lReturnValue:=lReturnValue)

    '        ' search the policy master client code
    '        'Only get the first 50 for the moment
    '        iNumber = 50


    '        m_lReturn = m_oPMBusiness.GetList(sList:="GIICLIENT", sParameter:=v_vShortName.ToUpper(), iNumber:=iNumber, vArray:=m_vPMResultArray, lReturnValue:=lReturnValue)

    '    Else


    '        m_lReturn = m_oPMBusiness.GetSchema(sSchema:="CLIENTPCO", iFieldCount:=iFieldCount, lReturnValue:=lReturnValue)

    '        'Search by postal code
    '        'Only get the first 50 for the moment
    '        iNumber = 50


    '        m_lReturn = m_oPMBusiness.GetList(sList:="CLIENTPCO", sParameter:=v_vPostalCode.ToUpper(), iNumber:=iNumber, vArray:=m_vPMResultArray, lReturnValue:=lReturnValue)
    '    End If


    '    If Not Informations.IsArray(m_vPMResultArray) Then
    '        ' Nothing to merge so exit !
    '        Return gPMConstants.PMEReturnCode.PMNotFound
    '    End If

    '    'Load the results into a temporary array in the same format
    '    'as the results from sirius

    '    j = 0


    '    For i As Integer = 0 To m_vPMResultArray.GetUpperBound(1)

    '        bMatch = True
    '        ' check the name
    '        If v_vName <> "" Then

    '            If (CStr(m_vPMResultArray(ACBClientName, i)).Trim().ToUpper().IndexOf(v_vName.Trim().ToUpper()) + 1) = 0 Then
    '                bMatch = False
    '            End If
    '        End If
    '        ' check the post code
    '        If v_vPostalCode <> "" Then

    '            If (CStr(m_vPMResultArray(ACBPostCode, i)).Trim().ToUpper().IndexOf(v_vPostalCode.Trim().ToUpper()) + 1) = 0 Then
    '                bMatch = False
    '            End If
    '        End If
    '        ' check the address
    '        If v_vAddress1 <> "" Then

    '            If (CStr(m_vPMResultArray(ACBAddress1, i)).Trim().ToUpper().IndexOf(v_vAddress1.Trim().ToUpper()) + 1) = 0 Then
    '                bMatch = False
    '            End If
    '        End If

    '        ' only return matched items
    '        If bMatch Then

    '            If j = 0 Then
    '                ReDim r_vResultArray(ACIMax, j)
    '            Else
    '                ReDim Preserve r_vResultArray(ACIMax, j)
    '            End If



    '            r_vResultArray(ACIInvariantKey, j) = m_vPMResultArray(ACBInvariantKey, i)


    '            r_vResultArray(ACIShortName, j) = m_vPMResultArray(ACBClientCode, i)


    '            r_vResultArray(ACILongName, j) = m_vPMResultArray(ACBClientName, i)


    '            r_vResultArray(ACIAddress1, j) = m_vPMResultArray(ACBAddress1, i)


    '            r_vResultArray(ACIPostalCode, j) = m_vPMResultArray(ACBPostCode, i)

    '            r_vResultArray(ACISource, j) = "Broking"

    '            j += 1

    '        End If



    '    Next i

    '    Return result

    'End Function
    ' ***************************************************************** '
    ' Name: IndexByInvariantKey
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function IndexByInvariantKey(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        For i As Integer = 0 To r_vResultArray.GetUpperBound(1)


            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(r_vResultArray(ACIInvariantKey, i)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                If CDbl(r_vResultArray(ACIInvariantKey, i)) <> 0 Then

                    AddInvariantKey(v_lInvariantKey:=CInt(r_vResultArray(ACIInvariantKey, i)), v_iSub:=i)
                End If
            End If
        Next i

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInvariantKey
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub AddInvariantKey(ByRef v_lInvariantKey As Integer, ByRef v_iSub As Integer)



        m_cInvariantKeys.Add(v_iSub, CStr(v_lInvariantKey))


    End Sub
    ' ***************************************************************** '
    ' Name: CheckInvariantKey
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function CheckInvariantKey(ByRef v_lInvariantKey As Integer, ByRef r_iSub As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            r_iSub = CInt(m_cInvariantKeys(CStr(v_lInvariantKey)))

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMNotFound
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: MergeData
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function MergeData(ByVal v_vPMArray(,) As Object, ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer) As Integer

        Dim result As Integer = 0
        Dim lFound As gPMConstants.PMEReturnCode
        Dim iMax0, iMax1, iSub As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        iMax0 = r_vResultArray.GetUpperBound(0)

        For i As Integer = v_vPMArray.GetLowerBound(1) To v_vPMArray.GetUpperBound(1)

            ' Check to see if client exists in sirius

            'developer guide no. 98
            lFound = CType(CheckInvariantKey(v_lInvariantKey:=v_vPMArray(ACIInvariantKey, i), r_iSub:=iSub), gPMConstants.PMEReturnCode)

            If lFound = gPMConstants.PMEReturnCode.PMTrue Then

                r_vResultArray(ACISource, iSub) = "Both"
            Else
                iMax1 = r_vResultArray.GetUpperBound(1) + 1
                ReDim Preserve r_vResultArray(iMax0, iMax1)
                For j As Integer = v_vPMArray.GetLowerBound(0) To v_vPMArray.GetUpperBound(0)


                    r_vResultArray(j, iMax1) = v_vPMArray(j, i)
                Next j
            End If

        Next i

        r_lNumberOfRecords = r_vResultArray.GetUpperBound(1) + 1

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetDetailsFromPMBArray
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'Public Function GetDetailsFromPMBArray(ByVal v_lInvariantKey As Integer, Optional ByRef r_sClientCode As String = "", Optional ByRef r_sClientName As String = "", Optional ByRef r_sAddress1 As String = "", Optional ByRef r_sAddress2 As String = "", Optional ByRef r_sAddress3 As String = "", Optional ByRef r_sAddress4 As String = "", Optional ByRef r_sPostcode As String = "", Optional ByRef r_sPortfolio As String = "", Optional ByRef r_sCustomerId As String = "", Optional ByRef r_sStatus As String = "", Optional ByRef r_sTelNo As String = "", Optional ByRef r_sAltTelNo As String = "", Optional ByRef r_sDOB As String = "", Optional ByRef r_sSex As String = "", Optional ByRef r_sMarried As String = "", Optional ByRef r_sChildren As String = "") As Integer

    '    Dim result As Integer = 0
    '    Dim vResultArray(,) As Object = Nothing
    '    Dim iNumber, iFieldCount As Integer
    '    Dim lReturnValue As Integer

    '    Try

    '        result = gPMConstants.PMEReturnCode.PMNotFound

    '        If Not Informations.IsArray(m_vPMResultArray) Then
    '            Return result
    '        End If


    '        For i As Integer = 0 To m_vPMResultArray.GetUpperBound(1)



    '            Dim dbNumericTemp As Double
    '            If Double.TryParse(CStr(m_vPMResultArray(ACBInvariantKey, i)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And CInt(m_vPMResultArray(ACBInvariantKey, i)) = v_lInvariantKey Then

    '                If Not False Then

    '                    r_sClientCode = CStr(m_vPMResultArray(ACBClientCode, i))
    '                End If

    '                If Not False Then

    '                    r_sClientName = CStr(m_vPMResultArray(ACBClientName, i))
    '                End If

    '                If Not False Then

    '                    r_sAddress1 = CStr(m_vPMResultArray(ACBAddress1, i))
    '                End If

    '                If Not False Then

    '                    r_sAddress2 = CStr(m_vPMResultArray(ACBAddress2, i))
    '                End If

    '                If Not False Then

    '                    r_sAddress3 = CStr(m_vPMResultArray(ACBAddress3, i))
    '                End If

    '                If Not False Then

    '                    r_sAddress4 = CStr(m_vPMResultArray(ACBAddress4, i))
    '                End If

    '                If Not False Then

    '                    r_sPostcode = CStr(m_vPMResultArray(ACBPostCode, i))
    '                End If

    '                If Not False Then

    '                    r_sPortfolio = CStr(m_vPMResultArray(ACBPortfolio, i))
    '                End If

    '                If Not False Then

    '                    r_sCustomerId = CStr(m_vPMResultArray(ACBCustomerID, i))
    '                End If

    '                If Not False Then

    '                    r_sStatus = CStr(m_vPMResultArray(ACBStatus, i))
    '                End If

    '                result = gPMConstants.PMEReturnCode.PMTrue

    '                Exit For
    '            End If

    '        Next i

    '        If r_sClientCode <> "" Then


    '            m_lReturn = m_oPMBusiness.GetSchema(sSchema:="MERCLIALL", iFieldCount:=iFieldCount, lReturnValue:=lReturnValue)

    '            ' search the policy master client code
    '            'Only get the first 50 for the moment
    '            iNumber = 50


    '            m_lReturn = m_oPMBusiness.GetList(sList:="MERCLIALL", sParameter:=r_sClientCode.ToUpper(), iNumber:=iNumber, vArray:=vResultArray, lReturnValue:=lReturnValue)

    '            If Informations.IsArray(vResultArray) Then

    '                For iNumber = 0 To vResultArray.GetUpperBound(1)


    '                    If r_sClientCode.ToUpper() = CStr(vResultArray(ACMAClientCode, iNumber)).ToUpper() Then


    '                        r_sTelNo = CStr(vResultArray(ACMATelNo, iNumber))

    '                        r_sAltTelNo = CStr(vResultArray(ACMAAltTelNo, iNumber))

    '                        r_sDOB = CStr(vResultArray(ACMADOB, iNumber))

    '                        r_sSex = CStr(vResultArray(ACMASex, iNumber))

    '                        r_sMarried = CStr(vResultArray(ACMAMarried, iNumber))

    '                        r_sChildren = CStr(vResultArray(ACMAChildren, iNumber))
    '                        Exit For
    '                    End If

    '                Next iNumber
    '            End If

    '        End If

    '        Return result

    '    Catch excep As System.Exception



    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error Message
    '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFromPMBArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsFromPMBArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return result

    '    End Try
    'End Function

    Public Function SearchAgent(ByRef r_vResultArray(,) As Object, Optional ByVal vShortname As Object = Nothing, Optional ByVal vname As Object = Nothing, Optional ByVal vpartyAgentDesc As Object = Nothing, Optional ByVal vCurrCode As String = Nothing, Optional ByVal vSubBraDesc As Object = Nothing, Optional ByVal vIsGrossAgent As Integer = 0) As Long
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CompanyID parameter (INPUT)
            If vShortname = "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=vShortname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            End If
            If vname = "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="name", vValue:=vname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            End If

            If vpartyAgentDesc = "(ALL)" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="partyAgentDesc", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            ElseIf vpartyAgentDesc = "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="partyAgentDesc", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="partyAgentDesc", vValue:=CStr(vpartyAgentDesc), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            End If
            If vCurrCode = "(ALL)" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="CurrCode", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                vCurrCode = vCurrCode.Substring(0, 3)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="CurrCode", vValue:=vCurrCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If vSubBraDesc = "(ALL)" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="SubBraDesc", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="SubBraDesc", vValue:=vSubBraDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            End If

            If vIsGrossAgent = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="IsGrossAgent", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="IsGrossAgent", vValue:=vIsGrossAgent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:="spu_Find_Agent", sSQLName:="spu_Find_Agent", bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        Catch excep As System.Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="SearchAgent", r_lFunctionReturn:=result, excep:=excep)
            Return result
        End Try

    End Function

    Public Function CheckOtherPartyBranchRecords() As Boolean

        Dim result As Boolean = False
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kCheckOtherPartyBranchRecordsSQL, sSQLName:=kCheckOtherPartyBranchRecordsName, bStoredProcedure:=kCheckOtherPartyBranchRecordsStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' No values, so return not found
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the array

            If gPMFunctions.ToSafeDouble(vResultArray(0, 0)) > 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckOtherPartyBranchRecords Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckOtherPartyBranchRecords", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        Finally
            If Informations.IsArray(vResultArray) Then
                vResultArray = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' SearchSpecialPartyByQuery
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="r_lNumberOfRecords"></param>
    ''' <param name="v_vShortName"></param>
    ''' <param name="v_vName"></param>
    ''' <param name="v_vFileCode"></param>
    ''' <param name="v_vClientType"></param>
    ''' <param name="v_vAgentType"></param>
    ''' <param name="v_vStatusType"></param>
    ''' <param name="v_vAddress1"></param>
    ''' <param name="v_vPostalCode"></param>
    ''' <param name="v_vAreaCode"></param>
    ''' <param name="v_vNumber"></param>
    ''' <param name="v_vInsuranceRef"></param>
    ''' <param name="v_vSwiftPartyID"></param>
    ''' <param name="v_vValidSourceArray"></param>
    ''' <param name="v_vBranch"></param>
    ''' <param name="v_vActiveStatus"></param>
    ''' <param name="v_bSuppressSubAgents"></param>
    ''' <param name="v_vPartyCnt"></param>
    ''' <param name="v_bIsInTransferMode"></param>
    ''' <param name="v_vRiskTransfer"></param>
    ''' <param name="v_vInsurerType"></param>
    ''' <param name="v_lCommissionLevel"></param>
    ''' <param name="V_BranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchSpecialPartyByQuery(ByRef r_vResultArray(,) As Object, ByRef r_lNumberOfRecords As Integer,
                                              Optional ByVal v_vShortName As Object = Nothing, Optional ByVal v_vName As Object = Nothing,
                                              Optional ByVal v_vFileCode As Object = Nothing, Optional ByVal v_vClientType As Object = Nothing,
                                              Optional ByVal v_vAgentType As Object = Nothing, Optional ByVal v_vStatusType As Object = Nothing,
                                              Optional ByVal v_vAddress1 As Object = Nothing, Optional ByVal v_vPostalCode As Object = Nothing,
                                              Optional ByVal v_vAreaCode As Object = Nothing, Optional ByVal v_vNumber As Object = Nothing,
                                              Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vSwiftPartyID As Object = Nothing,
                                              Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_vBranch As Object = Nothing,
                                              Optional ByVal v_vActiveStatus As Object = Nothing, Optional ByVal v_bSuppressSubAgents As Boolean = False,
                                              Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_bIsInTransferMode As Boolean = False,
                                              Optional ByVal v_vRiskTransfer As Object = Nothing, Optional ByVal v_vInsurerType As Object = Nothing,
                                              Optional ByVal v_lCommissionLevel As Integer = -1, Optional ByVal V_BranchCode As String = "",
                                              Optional ByVal sAgentGroup As String = "", Optional ByVal v_sSearchType As String = "",
                                              Optional ByVal v_vPhnNumber As Object = Nothing, Optional ByVal v_vClaimNumber As Object = Nothing,
                                              Optional ByVal v_vAgentCnt As Object = Nothing) As Integer

        Dim nResult As Integer
        Dim sbSQL As New StringBuilder
        Dim iParamCount As Integer
        Dim bInsRefSearch As Boolean
        Dim sShortName As String
        Dim sName As String
        Dim sFileCode As String
        Dim sPostalCode As String
        Dim sAddress1 As String
        Dim sPhnNumber As String
        Dim sClaimNumber As String
        Dim bNeedAddress As Boolean
        Dim bNeedInsurer As Boolean
        Dim bActiveStatus As Boolean
        Dim bAddOtherPartyBranch As Boolean
        Dim nOtherResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            nOtherResult = gPMConstants.PMEReturnCode.PMTrue
            nOtherResult = CheckOtherPartyBranchRecords() ' check added as query faiing in case of blank db
            If nOtherResult = gPMConstants.PMEReturnCode.PMTrue Then
                bAddOtherPartyBranch = True
            Else
                bAddOtherPartyBranch = False
            End If

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sbSQL = New StringBuilder("")
            sbSQL.Append("SELECT DISTINCT Party.party_cnt," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" party_type.description," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" shortname," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" name, " & Strings.ChrW(13) & Strings.ChrW(10))

            Select Case (v_vClientType)
                Case PMBConst.PMBPartyTypeAgentGroupText, PMBConst.PMBPartyTypeConsultantText, PMBConst.PMBPartyTypeExecutiveHandlerText, PMBConst.PMBPartyTypeAccountHandlerText, PMBConst.PMBPartyTypeFeeText, PMBConst.PMBPartyTypeDiscountText
                    bNeedAddress = False
                    bNeedInsurer = False
                Case PMBConst.PMBPartyTypeAgentText, PMBConst.PMBPartyTypeBrokerText, PMBConst.PMBPartyTypePersonalClientText, PMBConst.PMBPartyTypeThirdPartyAgentText
                    bNeedAddress = True
                    bNeedInsurer = False
                Case PMBConst.PMBPartyTypeInsurerText, PMBConst.PMBPartyTypeReinsurerText
                    bNeedAddress = True
                    bNeedInsurer = True
                Case PMBConst.PMBPartyTypeReassured
                    bNeedAddress = True
                    bNeedInsurer = False
                Case PMBConst.PMBPartyTypeOther
                    bNeedAddress = True
                    bNeedInsurer = False
                Case Else
                    bNeedAddress = True
                    bNeedInsurer = False
            End Select


            If (Not Informations.IsNothing(v_vActiveStatus)) AndAlso (Not String.IsNullOrEmpty(v_vActiveStatus)) Then
                If gPMFunctions.ToSafeDouble(v_vActiveStatus) <> 0 Then
                    bActiveStatus = True
                End If
            End If

            If bNeedAddress Then
                sbSQL.Append(" address.address1," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" postal_code = case address.postal_code")
                sbSQL.Append(" when convert(varchar(20),address.address_id) then ''")
                sbSQL.Append(" Else address.postal_code")
                sbSQL.Append(" end,")
            Else
                sbSQL.Append(" ''," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" ''," & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            sbSQL.Append(" Party.source_id," & Strings.ChrW(13) & Strings.ChrW(10))

            sbSQL.Append(" Party.party_id," & Strings.ChrW(13) & Strings.ChrW(10))

            If bNeedInsurer Then
                sbSQL.Append(" reinsurance_type," & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(" ''," & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            sbSQL.Append(" ''," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" ISNULL(is_prospect,0) is_prospect," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" ''," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" ''," & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" resolved_name," & Strings.ChrW(13) & Strings.ChrW(10))

            If v_vClientType = PMBConst.PMBPartyTypeAgentText Or v_vClientType = PMBConst.PMBPartyTypeBrokerText Then
                'Bring back agent description
                sbSQL.Append(" party_agent_type.description" & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(" ''" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            sbSQL.Append(", party.file_code, " & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append("'', " & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append("party.swift_party_id" & Strings.ChrW(13) & Strings.ChrW(10))
            If bNeedAddress Then
                sbSQL.Append(",Address.address2" & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(" ,''" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            If v_vClientType = PMBConst.PMBPartyTypeAgentGroupText Then
                sbSQL.Append(", Case isnull(Party_Agent_Group.Active,0)" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append("When 1 Then 'Active'")
                sbSQL.Append("When 0 Then 'Inactive'")
                sbSQL.Append("Else")
                sbSQL.Append("'<ALL>'")
                sbSQL.Append("End, " & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" Source.Description " & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(" ,''" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(",Source.Description " & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            sbSQL.Append(",0,record_status, party_type.code" & Strings.ChrW(13) & Strings.ChrW(10))


            If v_vClientType = PMBConst.PMBPartyTypeAgentText Then
                sbSQL.Append(" , CAST(Party_Agent.date_cancelled AS Varchar)" & Strings.ChrW(13) & Strings.ChrW(10))
                'TMP Allow Consolidated Commissioned Added
                sbSQL.Append(" ,Party_agent.Allow_consolidated_commission" & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(" , ''" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" , ''" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            If bNeedInsurer Then
                sbSQL.Append(", is_ri_broker")
            Else
                sbSQL.Append(" ,''" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If v_vClientType = PMBConst.PMBPartyTypePersonalClientText Then
                sbSQL.Append(",(CASE WHEN Party.Party_type_id='1' THEN Party.name ELSE '' END)AS name " & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(",(CASE WHEN Party.Party_type_id='1' THEN Party_Personal_Client.forename ELSE '' END)AS forename " & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(",Address.address3" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(",Address.address4" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            sbSQL.Append(",Party.domiciled_for_tax" & Strings.ChrW(13) & Strings.ChrW(10))

            If bNeedAddress Then
                sbSQL.Append(",country.code" & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(" ,''" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            sbSQL.Append(",'','','','',(SELECT CASE WHEN Party.service_level_id IS NOT NULL THEN(SELECT SL.code from Service_level SL WHERE SL.service_level_id = Party.service_level_id  ) ELSE '' END) ServiceLevelCode" & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(",(SELECT CASE WHEN Party.service_level_id IS NOT NULL THEN(SELECT SL.description from Service_level SL WHERE SL.service_level_id = Party.service_level_id  ) ELSE '' END) ServiceLevelDescription" & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(",party_type.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" FROM Party," & Strings.ChrW(13) & Strings.ChrW(10))

            'Only include these tables if we're actually looking for an agent
            If (v_vClientType = PMBConst.PMBPartyTypeAgentText) Or (v_vClientType = PMBConst.PMBPartyTypeBrokerText) Then
                sbSQL.Append(" Party_Agent_Type," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" Party_Agent," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" Party_Agent_Branch," & Strings.ChrW(13) & Strings.ChrW(10)) 'Agent Filtering
            End If

            If bNeedInsurer Then
                sbSQL.Append(" Party_Insurer," & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            If ReinsuranceTypeArray <> "" Then
                sbSQL.Append(" reinsurance_type," & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If Not (String.IsNullOrEmpty(sAgentGroup)) AndAlso v_vClientType = PMBConst.PMBPartyTypeAgentText Then
                sbSQL.Append(" (Select party_cnt, shortname as agentgroup From Party) AGG," & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            sbSQL.Append(" Party_Type" & Strings.ChrW(13) & Strings.ChrW(10))

            'Only use this table for finding agent groups
            If v_vClientType = PMBConst.PMBPartyTypeAgentGroupText Then
                sbSQL.Append(", Party_Agent_Group, " & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" Source " & Strings.ChrW(13) & Strings.ChrW(10))
            ElseIf v_vClientType = PMBConst.PMBPartyTypeAccountHandlerText Then
                sbSQL.Append(",Source, party_account_handler PAH, party_handler_branch PHB" & Strings.ChrW(13) & Strings.ChrW(10))
            Else
                sbSQL.Append(",Source " & Strings.ChrW(13) & Strings.ChrW(10))
                If bAddOtherPartyBranch = True Then
                    sbSQL.Append(",other_party_branch " & Strings.ChrW(13) & Strings.ChrW(10))
                End If

                'sj 22/08/2002 - end
            End If
            If bNeedAddress Then
                sbSQL.Append(", Address," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" Party_Address_Usage," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" Address_Usage_Type," & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" country" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            If (Not Informations.IsNothing(v_vPhnNumber)) AndAlso (Not String.IsNullOrEmpty(v_vPhnNumber)) Then
                sbSQL.Append(", Contact" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(", Party_Contact_Usage" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If v_vClientType = PMBConst.PMBPartyTypePersonalClientText Then
                sbSQL.Append(",Party_Personal_Client" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            sbSQL.Append(" WHERE Party_Type.party_type_id = Party.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sbSQL.Append(" AND Party.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10))

            If bNeedInsurer Then
                sbSQL.Append(" AND Party.party_cnt = Party_Insurer.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If IsRetained Then
                sbSQL.Append(" AND Party_Insurer.Is_retained = " & RetainedValue & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If ReinsuranceTypeArray <> "" Then
                sbSQL.Append(" AND Party_Insurer.Reinsurance_Type = Reinsurance_Type.Reinsurance_Type_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" AND Reinsurance_Type.Code IN " & ReinsuranceTypeArray & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            sbSQL.Append(" AND Source.source_id = Party.source_id" & Strings.ChrW(13) & Strings.ChrW(10))

            'Only use this table for finding agent groups
            If v_vClientType = PMBConst.PMBPartyTypeAgentGroupText Then
                sbSQL.Append(" AND Party_Agent_Group.party_cnt = Party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If bNeedAddress Then
                sbSQL.Append(" AND Party_Address_Usage.party_cnt = Party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" AND Address.country_id = country.country_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" AND Party_Address_Usage.address_cnt = Address.address_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" AND Address_Usage_Type.address_usage_type_id = Party_Address_Usage.address_usage_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
                sbSQL.Append(" AND Address_Usage_Type.code = '" & gSIRLibrary.SIRMainAddressABICode & "'" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If v_vClientType = PMBConst.PMBPartyTypePersonalClientText Then
                sbSQL.Append("AND Party_Personal_Client.party_cnt = Party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If IgnoreViewableOnlyAgents Then
                sbSQL.Append(" AND isnull(Party_Agent.is_viewable_only,0) = 0")
            End If

            ' Append the parameters to the Where clause
            iParamCount = 1

            ' Look for all matching parties
            If (Not Informations.IsNothing(v_vShortName)) AndAlso (Not String.IsNullOrEmpty(v_vShortName)) Then
                If v_vShortName <> "" Then
                    iParamCount += 1
                    sShortName = v_vShortName.Trim()
                    m_lReturn = CType(bPMFunc.ValidateSQL(sShortName), gPMConstants.PMEReturnCode)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    sbSQL.Append(" Shortname LIKE '" & sShortName & "'" & vbCrLf)
                End If
            End If

            If (Not Informations.IsNothing(v_vName)) AndAlso (Not String.IsNullOrEmpty(v_vName)) Then
                If v_vName <> "" Then
                    iParamCount += 1
                    sName = Convert.ToString(v_vName).Trim
                    m_lReturn = ValidateSQL(sName)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    sbSQL.Append(" ((LTRIM(RTRIM(Party.resolved_name)) LIKE '" & sName & "'" & vbCrLf)
                    sbSQL.Append("     AND ISNULL(party.resolved_name,'') <> '')" & Strings.ChrW(13) & Strings.ChrW(10))
                    sbSQL.Append(" OR (LTRIM(RTRIM(party.name)) LIKE '" & sName & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    sbSQL.Append("     AND ISNULL(party.name,'') <> ''))" & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If

            If v_vClientType = PMBConst.PMBPartyTypeAgentGroupText Then

                If ((Not Informations.IsNothing(v_vActiveStatus)) AndAlso (Not String.IsNullOrEmpty(v_vActiveStatus))) AndAlso gPMFunctions.ToSafeDouble(v_vActiveStatus) <> 2 Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    sbSQL.Append(" isnull(Party_Agent_Group.Active,0) = '" & v_vActiveStatus & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                End If

                'If a zero is passed this is the <ALL> option andalso no criteria is added.
                If ((Not Informations.IsNothing(v_vBranch)) AndAlso (Not String.IsNullOrEmpty(v_vBranch))) AndAlso gPMFunctions.ToSafeDouble(v_vBranch) <> 0 AndAlso v_vClientType <> PMBConst.PMBPartyTypeAgentGroupText Then
                    If v_vBranch <> "" Then
                        iParamCount += 1
                        If iParamCount > 1 Then
                            sbSQL.Append(" AND")
                        End If
                        sbSQL.Append(" Party.source_id = '" & v_vBranch.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vFileCode)) AndAlso (Not String.IsNullOrEmpty(v_vFileCode)) Then
                If v_vFileCode <> "" Then
                    iParamCount += 1
                    sFileCode = Convert.ToString(v_vFileCode).Trim
                    m_lReturn = ValidateSQL(sFileCode)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    If sFileCode.IndexOf("%"c) >= 0 Then
                        sbSQL.Append(" file_code LIKE '" & sFileCode & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sbSQL.Append(" file_code = '" & sFileCode & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vAddress1)) AndAlso (Not String.IsNullOrEmpty(v_vAddress1)) Then
                If v_vAddress1 <> "" Then
                    iParamCount += 1
                    sAddress1 = Convert.ToString(v_vAddress1).Trim
                    m_lReturn = ValidateSQL(sAddress1)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    If sAddress1.IndexOf("%"c) >= 0 Then
                        sbSQL.Append(" address.address1 LIKE '" & sAddress1 & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sbSQL.Append(" address.address1 = '" & sAddress1 & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vPostalCode)) AndAlso (Not String.IsNullOrEmpty(v_vPostalCode)) Then
                If v_vPostalCode <> "" Then
                    iParamCount += 1
                    sPostalCode = Convert.ToString(v_vPostalCode).Trim
                    m_lReturn = ValidateSQL(sPostalCode)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    If sPostalCode.IndexOf("%"c) >= 0 Then
                        sbSQL.Append(" address.postal_code LIKE '" & sPostalCode & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sbSQL.Append(" address.postal_code = '" & sPostalCode & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vPhnNumber)) AndAlso (Not String.IsNullOrEmpty(v_vPhnNumber)) Then
                If v_vPhnNumber <> "" Then
                    iParamCount += 1
                    sPhnNumber = Convert.ToString(v_vPhnNumber).Trim
                    m_lReturn = ValidateSQL(sPhnNumber)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    sbSQL.Append(" Party_Contact_Usage.party_cnt = Party.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10))
                    sbSQL.Append(" And Contact.contact_cnt = Party_Contact_Usage.contact_cnt " & Strings.ChrW(13) & Strings.ChrW(10))
                    If sPhnNumber.IndexOf("%"c) >= 0 Then
                        sbSQL.Append(" And (Contact.number LIKE '" & sPhnNumber & "' OR Contact.area_code LIKE '" & sPhnNumber & "')" & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sbSQL.Append(" And (Contact.number = '" & sPhnNumber & "' OR Contact.area_code = '" & sPhnNumber & "')" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            If (Not Informations.IsNothing(v_vClaimNumber)) AndAlso (Not String.IsNullOrEmpty(v_vClaimNumber)) Then
                If v_vClaimNumber <> "" Then
                    iParamCount += 1
                    sClaimNumber = Convert.ToString(v_vClaimNumber).Trim
                    m_lReturn = ValidateSQL(sClaimNumber)
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    If sClaimNumber.IndexOf("%"c) >= 0 Then
                        sbSQL.Append(" (party.party_cnt IN (select other_party_id from claim where Claim_Number Like '" & sClaimNumber & "')" & Strings.ChrW(13) & Strings.ChrW(10))
                        sbSQL.Append(" OR (party.party_cnt IN (select party_cnt from Claim_Payment where claim_id IN (select Claim_id from claim where Claim_Number Like '" & sClaimNumber & "'))))" & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sbSQL.Append(" (party.party_cnt IN (select other_party_id from claim where Claim_Number = '" & sClaimNumber & "')" & Strings.ChrW(13) & Strings.ChrW(10))
                        sbSQL.Append(" OR (party.party_cnt IN (select party_cnt from Claim_Payment where claim_id IN (select Claim_id from claim where Claim_Number = '" & sClaimNumber & "'))))" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            iParamCount += 1
            If v_vClientType IsNot Nothing Then
                If v_vClientType <> "<ALL>" Then
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    Select Case v_vClientType
                        Case PMBConst.PMBPartyTypePersonalClientText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypePersonalClient & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeAgentText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeAgent & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND Party_Agent.party_cnt = Party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND Party_agent.party_cnt= Party_agent_branch.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)) ''Agent Filtering
                            sbSQL.Append(" AND Party_Agent_Type.party_agent_type_id = Party_Agent.party_agent_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND Party_Agent_Type.is_visible = 1" & Strings.ChrW(13) & Strings.ChrW(10))
                            If Not String.IsNullOrEmpty(sAgentGroup) Then
                                sbSQL.Append(" AND AGG.party_cnt = party_agent.linked_account_group AND AGG.agentgroup = '" & sAgentGroup & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                            End If
                            Select Case CStr(v_vAgentType)
                                Case PMBConst.PMBAgentTypeBrokerText
                                    iParamCount += 1
                                    sbSQL.Append(" AND Party_Agent_Type.Party_Agent_Type_id = '" & PMBConst.PMBAgentTypeBroker & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                                Case PMBConst.PMBAgentTypeSubAgentText
                                    iParamCount += 1
                                    sbSQL.Append(" AND Party_Agent_Type.Party_Agent_Type_id = '" & PMBConst.PMBAgentTypeSubAgent & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                                Case PMBConst.PMBAgentTypeCommAccountText
                                    iParamCount += 1
                                    sbSQL.Append(" AND Party_Agent_Type.Party_Agent_Type_id = '" & PMBConst.PMBAgentTypeCommAccount & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                                Case PMBConst.PMBAgentTypeIntermediaryText
                                    iParamCount += 1
                                    sbSQL.Append(" AND Party_Agent_Type.Party_Agent_Type_id = '" & PMBConst.PMBAgentTypeIntermediary & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                                Case PMBConst.PMBAgentTypeIntroducerText
                                    iParamCount += 1
                                    sbSQL.Append(" AND Party_Agent_Type.Party_Agent_Type_id = '" & PMBConst.PMBAgentTypeIntroducer & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                                Case Else
                                    sbSQL.Append(" AND Party_Agent.party_cnt = Party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                                    sbSQL.Append(" AND Party_Agent_Type.party_agent_type_id = Party_Agent.party_agent_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
                                    If v_bSuppressSubAgents Then
                                        sbSQL.Append(" AND Party_Agent_Type.Party_Agent_Type_id <> '" & PMBConst.PMBAgentTypeSubAgent & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                                    End If

                            End Select

                        Case PMBConst.PMBPartyTypeCorporateClientText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeCorporateClient & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeGroupClientText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeGroupClient & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeConsultantText
                            sbSQL.Append(" (Party_type.code = '" & PMBConst.PMBPartyTypeConsultant & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" OR Party_type.code = '" & PMBConst.PMBPartyTypeExecutiveHandler & "')" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeAccountHandlerText

                            sbSQL.Append(" (Party_type.code = '" & PMBConst.PMBPartyTypeAccountHandler & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                            sbSQL.Append(" OR Party_type.code = '" & PMBConst.PMBPartyTypeExecutiveHandler & "')" & Strings.ChrW(13) & Strings.ChrW(10))

                            sbSQL.Append("  AND PAH.party_cnt = party.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10))

                            sbSQL.Append("  AND PHB.party_cnt = party.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeInsurerText, PMBConst.PMBPartyTypeReinsurerText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeInsurer & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeBrokerText

                            sbSQL.Append(" Party_Agent.party_cnt = Party.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND Party_Agent_Type.party_agent_type_id = Party_Agent.party_agent_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND Party_Agent_Type.is_visible = 1" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND ((Party_type.code = '" & PMBConst.PMBPartyTypeBroker & "')" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" OR (Party_Agent_Type.Party_Agent_Type_id = '" & PMBConst.PMBAgentTypeBroker & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                            sbSQL.Append(" AND Party_type.code = '" & PMBConst.PMBPartyTypeAgent & "'))" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeFeeText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeFee & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeExtraText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeExtra & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeDiscountText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeDiscount & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeCommissionAccountText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeCommissionAccount & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeExecutiveHandlerText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeExecutiveHandler & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeFinanceProviderText
                            sbSQL.Append(" Party_type.code = '" & PMBConst.PMBPartyTypeFinanceProvider & "'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case PMBConst.PMBPartyTypeOther
                            sbSQL.Append(" Party_type.code like '" & v_vClientType & "%'" & Strings.ChrW(13) & Strings.ChrW(10))

                        Case Else
                            If String.IsNullOrEmpty(v_sSearchType) AndAlso v_sSearchType = "" Then
                                sbSQL.Append(" Party_type.description = '" & v_vClientType & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                            Else

                                Select Case CStr(v_sSearchType)
                                    Case "OT"
                                        sbSQL.Append(" Party_type.code like '" & v_sSearchType & "%" & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                                    Case Else
                                        sbSQL.Append(" Party_type.code = '" & v_sSearchType & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                                End Select
                            End If
                    End Select

                    sbSQL.Append(" AND Party_Type.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10))

                End If
            End If

            If iParamCount = 1 Then
                'no parameters passed so query cannot be executed
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (Not Informations.IsNothing(v_vPartyCnt)) AndAlso (Not String.IsNullOrEmpty(v_vPartyCnt)) Then
                If v_vPartyCnt.ToString() <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sbSQL.Append(" AND")
                    End If
                    sbSQL.Append(" party.party_cnt = " & v_vPartyCnt.ToString().Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If

            If v_bIsInTransferMode Then
                sbSQL.Append(" AND IsNull(Party_Agent.is_in_transfer_mode,0) = 1" & Strings.ChrW(13) & Strings.ChrW(10))
            End If


            'DJM 01/07/2002 : Restrict by valid sources - CMG/PB 18072002 Only if not agent group search.
            'Agent Filtering

            Dim b_setallbrance As Boolean

            If v_vClientType = PMBConst.PMBAgentTypeAgentText And Informations.IsNothing(v_vValidSourceArray) And m_sTransactionType <> "" Then
                sbSQL.Append("AND party_agent_branch.source_id = " & m_iSourceID)
            ElseIf v_vClientType = PMBConst.PMBPartyTypeAccountHandlerText And Informations.IsNothing(v_vValidSourceArray) And m_sTransactionType <> "" Then
                sbSQL.Append(" AND PHB.source_id= " & v_vBranch & Strings.ChrW(13) & Strings.ChrW(10))
            Else

                If Not Information.IsNothing(v_vValidSourceArray) And v_vClientType <> PMBConst.PMBPartyTypeAgentGroupText AndAlso m_lPartySourceId = 0 Then
                    If Information.IsArray(v_vValidSourceArray) Then
                        If v_vValidSourceArray.GetUpperBound(1) > 0 Then
                            sbSQL.Append("AND (")
                            For iLoop As Integer = v_vValidSourceArray.GetLowerBound(1) To v_vValidSourceArray.GetUpperBound(1)
                                If v_vClientType = PMBConst.PMBAgentTypeAgentText Then
                                    sbSQL.Append("party.source_id = " & CStr(CInt(v_vValidSourceArray(0, iLoop))) & Strings.ChrW(13) & Strings.ChrW(10))
                                ElseIf v_vClientType = PMBConst.PMBPartyTypeAccountHandlerText Then
                                    'developer guide no. 162
                                    sbSQL.Append(" PHB.source_id= " & CStr(CInt(v_vValidSourceArray(0, iLoop))) & Strings.ChrW(13) & Strings.ChrW(10))
                                Else                                    'developer guide no. 162
                                    sbSQL.Append("party.source_id = " & CStr(CInt(v_vValidSourceArray(0, iLoop))) & Strings.ChrW(13) & Strings.ChrW(10))
                                    b_setallbrance = True
                                End If
                                If iLoop <> v_vValidSourceArray.GetUpperBound(1) Then
                                    sbSQL.Append("OR ")
                                End If
                            Next iLoop
                            sbSQL.Append(")" & Strings.ChrW(13) & Strings.ChrW(10))
                        End If
                    End If
                ElseIf m_lPartySourceId <> 0 Then
                    If v_vClientType = PMBConst.PMBAgentTypeAgentText Then
                        sbSQL.Append(" AND Party_agent_branch.source_id = " & CStr(m_lPartySourceId) & Strings.ChrW(13) & Strings.ChrW(10))
                    ElseIf v_vClientType = PMBConst.PMBPartyTypeAccountHandlerText Then
                        sbSQL.Append(" AND PHB.source_id= " & CStr(m_lPartySourceId) & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        sbSQL.Append(" AND party.source_id = " & CStr(m_lPartySourceId) & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            End If

            If ((Not Informations.IsNothing(v_vBranch)) AndAlso (Not String.IsNullOrEmpty(v_vBranch))) AndAlso gPMFunctions.ToSafeDouble(v_vBranch) <> 0 Then
                If v_vClientType = PMBConst.PMBAgentTypeAgentText AndAlso m_sTransactionType <> "" Then
                    sbSQL.Append(" AND party_agent_branch.source_id = " & v_vBranch & Strings.ChrW(13) & Strings.ChrW(10))
                ElseIf v_vClientType = PMBConst.PMBPartyTypeAccountHandlerText AndAlso m_sTransactionType <> "" Then
                    sbSQL.Append(" AND PHB.source_id = " & v_vBranch & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            ElseIf (Not Informations.IsNothing(V_BranchCode)) AndAlso (Not Informations.IsNothing(v_vValidSourceArray)) AndAlso
                V_BranchCode <> "" Then
                For iLoop As Integer = v_vValidSourceArray.GetLowerBound(1) To v_vValidSourceArray.GetUpperBound(1)
                    If v_vValidSourceArray(1, iLoop).ToString().Trim() = V_BranchCode.Trim() Then
                        If v_vClientType = PMBConst.PMBAgentTypeAgentText Then
                            sbSQL.Append(" AND party_agent_branch.source_id = " & v_vValidSourceArray(0, iLoop).ToString().Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                        ElseIf v_vClientType = PMBConst.PMBPartyTypeAccountHandlerText Then
                            sbSQL.Append(" AND PHB.source_id = " & v_vValidSourceArray(0, iLoop).ToString().Trim() &
                                Strings.ChrW(13) & Strings.ChrW(10))
                        ElseIf v_vClientType = PMBConst.PMBPartyTypeAgentGroupText Then
                            sbSQL.Append(" AND party.source_id = " & v_vValidSourceArray(0, iLoop).ToString().Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                        Else
                            If Not b_setallbrance Then
                                sbSQL.Append(" AND party.source_id = " & v_vValidSourceArray(0, iLoop).ToString().Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                            End If

                        End If
                    End If
                Next

            End If
            If Not Informations.IsNothing(v_lCommissionLevel) AndAlso bNeedInsurer = False AndAlso (v_vClientType = PMBConst.PMBPartyTypeAgentText Or v_vClientType = PMBConst.PMBPartyTypeBrokerText) Then
                If v_lCommissionLevel <> 0 Then
                    If v_lCommissionLevel > 0 Then
                        sbSQL.Append(" and Party_Agent.commission_level_id = " & v_lCommissionLevel & vbCrLf)
                    Else
                        sbSQL.Append(" and ISNULL(Party_Agent.commission_level_id, 0) = 0")
                    End If
                End If
                ' Else
                ' sbSQL.Append(" and ISNULL(Party_Agent.commission_level_id, 0) = 0")
            End If

            If m_bSuppressCancelledAgents Then
                sbSQL.Append(" and party_agent.date_cancelled = '29 dec 1899'" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            If (Not Informations.IsNothing(v_vAgentCnt) AndAlso Not String.IsNullOrEmpty(v_vAgentCnt) AndAlso v_vAgentCnt.ToString() <> "") Then
                If (v_vClientType = PMBConst.PMBPartyTypePersonalClientText Or v_vClientType = PMBConst.PMBPartyTypeGroupClientText Or v_vClientType = PMBConst.PMBPartyTypeAgentText Or v_vClientType = PMBConst.PMBPartyTypeCorporateClientText) Then
                    sbSQL.Append(" AND (Party.agent_cnt = " & v_vAgentCnt.ToString().Trim() & " OR Party.party_cnt = " & v_vAgentCnt.ToString().Trim() & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If

            ' Add the Order By clause
            sbSQL.Append(" ORDER BY Shortname" & Strings.ChrW(13) & Strings.ChrW(10))

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sbSQL.ToString(), sSQLName:=ACPartyFromQueryName, bStoredProcedure:=ACPartyFromQueryStored, lNumberRecords:=r_lNumberOfRecords, vResultArray:=r_vResultArray)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SearchSpecialPartyByQuery")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End With
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchSpecialPartyByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchSpecialPartyByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyType
    '
    ' Description: Return the party type
    '
    '
    ' ***************************************************************** '
    Public Function GetPartyType(ByRef lPartyCnt As Integer, ByRef sPartyTypeText As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'What type of party/client is it
            '    sSQL$ = "SELECT party_cnt, '" & SIRPartyTypePersonalClient & _
            '"' FROM party_personal_client WHERE party_cnt = " & m_lPartyCnt & _
            '"UNION SELECT party_cnt, '" & SIRPartyTypeAgent & _
            '"' FROM party_agent WHERE party_cnt = " & m_lPartyCnt & _
            '"UNION SELECT party_cnt, '" & SIRPartyTypeCorporateClient & _
            '"' FROM party_corporate_client WHERE party_cnt = " & m_lPartyCnt & _
            '"UNION SELECT party_cnt, '" & SIRPartyTypeGroupClient & _
            '"' FROM party_group_client WHERE party_cnt = " & m_lPartyCnt

            '    sSQL$ = "SELECT '" & SIRPartyTypePersonalClientText & _
            '"' FROM party_personal_client WHERE party_cnt = " & lPartyCnt & _
            '"UNION SELECT '" & SIRPartyTypeAgentText & _
            '"' FROM party_agent WHERE party_cnt = " & lPartyCnt & _
            '"UNION SELECT '" & SIRPartyTypeCorporateClientText & _
            '"' FROM party_corporate_client WHERE party_cnt = " & lPartyCnt & _
            '"UNION SELECT '" & SIRPartyTypeGroupClientText & _
            '"' FROM party_group_client WHERE party_cnt = " & lPartyCnt

            sSQL = "SELECT t.code from party_type t, party p" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "where t.party_type_id = p.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "and p.party_cnt = " & CStr(lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETPARTYTYPE", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find party type for party_cnt " & lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If

            'Return the type

            sPartyTypeText = CStr(vResultArray(0, 0)).Trim()

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CalcCombinedKey
    '
    ' Description: This is a wrapper to the component services function
    ' to derive the Unique Invariant Key from source and entity ID.
    '
    ' ***************************************************************** '
    Public Function calccombinedkey(ByVal v_lSourceID As Integer, ByVal v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(gPMComponentServices.calccombinedkey(v_lSourceID:=v_lSourceID, v_lKeyID:=v_lKeyID, r_lCombinedKeyID:=r_lCombinedKeyID), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcCombinedKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckAgencyAgreement
    '
    ' Description: Check if Agency Agreement exists
    '
    ' ***************************************************************** '
    Public Function CheckAgencyAgreement(ByVal v_lPartyCnt As Integer, ByRef r_bIsAgent As Boolean, ByRef r_bIsAgencyAgreementValid As Boolean) As Integer

        Dim result As Integer = 0
        Dim sPartyTypeText As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim dtAgencyAgreementDate, dtNextReviewDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Exit if not PartyType Agent
            m_lReturn = CType(GetPartyType(lPartyCnt:=v_lPartyCnt, sPartyTypeText:=sPartyTypeText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sPartyTypeText.Trim() = PMBConst.PMBPartyTypeAgentText.Trim() Then
                r_bIsAgent = True
            Else
                r_bIsAgent = False
                Return result
            End If


            ' Get Agency Agreement details from database
            With m_oDatabase
                .Parameters.Clear()

                sSQL = "SELECT agency_agreement_date,"
                sSQL = sSQL & " agency_next_review_date"
                sSQL = sSQL & " FROM Party_Agent"
                sSQL = sSQL & " WHERE party_cnt = " & CStr(v_lPartyCnt)

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="CheckAgencyAgreement", bStoredProcedure:=False)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (.Records.Count() <> 1) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'SD 01/08/2002 Scalablity changes
                dtAgencyAgreementDate = gPMFunctions.NullToDate(.Records.Item(1).Fields()("agency_agreement_date"))
                dtNextReviewDate = gPMFunctions.NullToDate(.Records.Item(1).Fields()("agency_next_review_date"))
            End With

            ' Check if valid
            'developer guide no. 40
            If DateTime.Parse(dtAgencyAgreementDate) <= DateTime.Parse(m_dtEffectiveDate) Then
                r_bIsAgencyAgreementValid = (DateTime.Parse(dtNextReviewDate) >= DateTime.Parse(m_dtEffectiveDate))
            Else
                r_bIsAgencyAgreementValid = False
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAgencyAgreement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAgencyAgreement ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetId (Public)
    '
    ' Description: Selects Policies by ID and populates the
    '              Base Details.
    '
    ' ***************************************************************** '
    Public Function GetID(ByRef lID As Integer, Optional ByVal vName As Object = Nothing, Optional ByVal vShortName As Object = Nothing, Optional ByVal vSourceId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRowsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Informations.IsNothing(vName) And Informations.IsNothing(vShortName) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lID = -1
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party cnt parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="Party_cnt", vValue:=lID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the source_id parameter (INPUT)


            m_lError = m_oDatabase.Parameters.Add(sName:="Source_Id", vValue:=If(Informations.IsNothing(vSourceId), DBNull.Value, vSourceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsNothing(vName) Then
                ' Add the name parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="Party_Name", vValue:=vName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            ElseIf (Not Informations.IsNothing(vShortName)) Then
                ' Add the shortname parameter (INPUT)
                m_lError = m_oDatabase.Parameters.Add(sName:="Party_ShortName", vValue:=vShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                'else case
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If



            If Not Informations.IsNothing(vName) Then
                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLAction(sSQL:=ACPartyFromNameSQL, sSQLName:=ACPartyFromNameName, bStoredProcedure:=ACPartyFromNameStored, lRecordsAffected:=lRowsAffected)
            ElseIf (Not Informations.IsNothing(vShortName)) Then
                ' Execute SQL Statement
                m_lError = m_oDatabase.SQLAction(sSQL:=ACPartyFromShortNameSQL, sSQLName:=ACPartyFromShortNameName, bStoredProcedure:=ACPartyFromShortNameStored, lRecordsAffected:=lRowsAffected)
            Else

            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Get the party_cnt of the record selected
            lID = m_oDatabase.Parameters.Item("party_cnt").Value

            If lID = -1 Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetName (Public)
    '
    ' Description: Selects the party name using the party ID.
    '
    ' ***************************************************************** '
    Public Function GetName(ByRef lPartyCnt As Integer, ByRef sPartyName As String) As Integer

        Dim result As Integer = 0
        Dim lRowsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=CStr(sPartyName), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACPartyFromCntSQL, sSQLName:=ACPartyFromCntName, bStoredProcedure:=ACPartyFromCntStored, lRecordsAffected:=lRowsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the party_cnt of the record selected
            If m_oDatabase.Parameters.Item("shortname").Value Is Nothing Or m_oDatabase.Parameters.Item("shortname").Value Is DBNull.Value Then
                sPartyName = String.Empty
            Else
                sPartyName = m_oDatabase.Parameters.Item("shortname").Value.ToString.Trim
            End If


            If sPartyName.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetResolvedName (Public)
    '
    ' Description: Selects the party ResolvedName using the party ID.
    '
    ' CT 17/08/00
    '
    ' ***************************************************************** '
    Public Function GetResolvedName(ByRef lPartyCnt As Integer, ByRef sPartyResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim lRowsAffected As Integer
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT resolved_name "
            sSQL = sSQL & "FROM party "
            sSQL = sSQL & "WHERE party_cnt= {party_cnt}"



            ' Add the parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResolvedName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetResolvedName", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, lNumberRecords:=lRowsAffected, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResolvedName")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the resolved name of the record selected

            sPartyResolvedName = CStr(vResultArray(0, 0))
            If sPartyResolvedName.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetResolvedName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResolvedName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:  (Public)
    '
    ' Description: Gets the Lookup values for a FindParty
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101(Guide)
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        'Dim oSIRPartyAGG As bSIRPartyAGG.SIRPartyAGG
        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'vResultArray = ""
            ' Reset Table Array

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names
            'sj 22/07/2002 - start
            'vTabArray(PMLookupTableName, 0) = SIRLookupBranch

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupSource
            'sj 22/07/2002 - end
            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Do not supply a key

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRPartyAGG reference
            '  Set oSIRPartyAGG = Nothing

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
    ' Name: GetStructure
    '
    ' Description: Return the party structure
    '
    ' CLG 20040217 : used to "SELECT code from party_structure" but as all DB's contained
    '                "PMB" in this table it now just returns "PMB"
    ' ***************************************************************** '
    Public Function GetStructure(ByRef sStructure As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Return the structure
            sStructure = "PMB"

            m_sStructure = sStructure

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStructure Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStructure", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' DD 31/10/2003: Made public for use by iPMBFindParty (FSA work)
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, ByVal v_vDescription As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSirEvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
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
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there
    '              are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ClearParameters()
    '
    'Try 
    '
    ' Clear the Databases Parameters Collection
    'If m_oDatabase.Parameters Is Nothing Then
    ' Do Nothing
    'Else
    'm_oDatabase.Parameters.Clear()
    'End If
    '
    '
    ' Added by Scalability Update Program - 30/07/2002
    'm_cInvariantKeys = New Collection()
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Clear Parameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearParameters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    m_sUnderwritingOrAgency = "A"
        '
        '    m_oDatabase.Parameters.Clear
        '
        '    sSQL = "SELECT value FROM hidden_options"
        '
        '    m_lReturn& = m_oDatabase.SQLSelect( _
        ''        sSQL:=sSQL, _
        ''        sSQLName:="GetHiddenOption", _
        ''        bStoredProcedure:=False)
        '
        '    If (m_lReturn& <> PMTrue) Then
        '        Exit Function
        '        ' Carry on without default set
        '    End If
        '
        '    If (m_oDatabase.Records.Count = 1) Then
        '        ' select first letter of the return field
        '        m_sUnderwritingOrAgency = Left$(CStr(m_oDatabase.Records.Item(1).Fields.Item("value").Value), 1)
        '    End If
        '
        '    If ((m_sUnderwritingOrAgency <> "A") And (m_sUnderwritingOrAgency <> "U")) Then
        '        m_sUnderwritingOrAgency = "A"
        '    End If
        'sj 19/06/2002 - start

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 18/10/2001    Created
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        m_lReturn = CType(bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingType), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getUnderwritingType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getUnderwritingType")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    'default to U
        '    m_sUnderwritingType = "U"
        '
        '    m_oDatabase.Parameters.Clear
        '
        '    sSQL = "SELECT UW_type FROM hidden_options"
        '
        '    m_lReturn& = m_oDatabase.SQLSelect( _
        ''        sSQL:=sSQL, _
        ''        sSQLName:="GetUnderwritingType", _
        ''        bStoredProcedure:=False)
        '
        '    If (m_lReturn& <> PMTrue) Then
        '        Exit Function
        '        ' Carry on with default
        '    End If
        '
        '    If (m_oDatabase.Records.Count = 1) Then
        '        ' select first letter of the return field
        '        m_sUnderwritingType = Left$(CStr(m_oDatabase.Records.Item(1).Fields.Item("UW_type").Value), 1)
        '    End If
        '
        '    If ((m_sUnderwritingType <> "A") And (m_sUnderwritingType <> "U")) Then
        '        m_sUnderwritingType = "U"
        '    End If
        'sj 19/06/2002 - start

        Return result

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



    Public Function GetOtherPartyTypes(ByRef r_vResultArray(,) As Object) As Integer
        ' ***************************************************************** '
        ' Name: GetOtherPartyTypes
        '
        ' Description: Return Other party Types for Lookup purposes. Can't
        '               use standard lookup procedures as we are retrieving
        '               just part of a table (the Other Types from the
        '               Party_Type table.
        '
        ' Author: Richard Hill (04/07/2000)
        ' ***************************************************************** '
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT * FROM party_type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE code LIKE '" & PMBConst.PMBPartyTypeOther & "%' AND is_deleted=0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ORDER BY description" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOtherPartyTypes", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find 'Other' party types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherPartyTypes", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If


            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherPartyTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherPartyTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetInsurerType
    '
    ' Description: Return the Insurer type
    ' ***************************************************************** '
    Public Function GetInsurerType(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInsurerTypeSQL, sSQLName:=kGetInsurerTypeName, bStoredProcedure:=kGetInsurerTypeStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find insurer type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerType", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetFullAddress
    '
    ' Description:
    '
    ' History: 17/11/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetFullAddress(ByVal v_lPartyCnt As Integer, ByRef r_vAddress1 As String, ByRef r_vAddress2 As String, ByRef r_vAddress3 As String, ByRef r_vAddress4 As String, ByRef r_vPostalCode As String, Optional ByRef r_vCountryID As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        'AK 240602 - correspondence address type
        Const ACAddressC As String = "3131 XCO"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            'AK 240602 - subquery may return multiple rows, extract correspondence address only
            sSQL = "SELECT address1, address2, address3, address4, postal_code, Country_id " &
                   "FROM Address WHERE address_cnt = (" &
                   "SELECT max(pa.address_cnt) FROM Party_Address_Usage pa, Address_Usage_Type au  WHERE pa.Party_cnt = {party_cnt} AND " &
                   " au.code = '" & ACAddressC & "' AND pa.address_usage_type_id = au.address_usage_type_id)"

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add party_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetFullAddress", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Any address?
            If Informations.IsArray(vResultArray) Then


                r_vAddress1 = CStr(vResultArray(0, 0))

                r_vAddress2 = CStr(vResultArray(1, 0))

                r_vAddress3 = CStr(vResultArray(2, 0))

                r_vAddress4 = CStr(vResultArray(3, 0))

                r_vPostalCode = CStr(vResultArray(4, 0))

                r_vCountryID = CStr(vResultArray(5, 0))

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFullAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFullAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsUserSystemAdministrator
    '
    ' Description:
    '
    ' History: 17/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function IsUserSystemAdministrator(ByRef r_bIsSystemAdministrator As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACIsUserSysAdminStored As Boolean = True
            Const ACIsUserSysAdminName As String = "IsUserSystemAdministrator"
            'developer guide no. 39 (Guide)
            Const ACIsUserSysAdminSQL As String = "spu_pmuser_is_sysadmin"
            'developer guide no. 71(Guide)
            Dim vResultArray(,) As Object = Nothing

            r_bIsSystemAdministrator = False

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for user_id = " & m_iUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If
            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'developer guide no. 40
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for effective_date = " & DateTime.Now, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsUserSysAdminSQL, sSQLName:=ACIsUserSysAdminName, bStoredProcedure:=ACIsUserSysAdminStored, lNumberRecords:=500, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed for " & ACIsUserSysAdminName, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Informations.IsArray(vResultArray) Then

                If Val(CStr(vResultArray(0, 0))) > 0 Then
                    r_bIsSystemAdministrator = True
                End If
            End If

            'spu_pmuser_is_sysadmin
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserSystemAdministrator Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function CheckInsurerAccess(ByRef r_bHasAccess As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            r_bHasAccess = False

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for user_id = " & m_iUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckInsurerAccessSQL, sSQLName:=ACCheckInsurerAccessName, bStoredProcedure:=ACCheckInsurerAccessStored, lNumberRecords:=500, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed for " & ACCheckInsurerAccessName, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserSystemAdministrator")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Informations.IsArray(vResultArray) Then
                r_bHasAccess = True
            End If

            'spu_pmuser_is_sysadmin
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInsurerAccess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInsurerAccess", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: RestrictInsurerAccess
    '
    ' Description:
    '
    ' History: 02/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RestrictInsurerAccess(ByVal v_lUserInsurerCnt As Integer, ByRef r_bRestrictInsurerAccess As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sPartyTypeCode As String = ""

            r_bRestrictInsurerAccess = False

            m_lReturn = CType(GetPartyType(lPartyCnt:=v_lUserInsurerCnt, sPartyTypeText:=sPartyTypeCode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyType Failed for party_cnt = " & v_lUserInsurerCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RestrictInsurerAccess")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sPartyTypeCode = PMBConst.PMBPartyTypeInsurer Then
                r_bRestrictInsurerAccess = True
            End If

            m_bRestrictInsurerAccess = r_bRestrictInsurerAccess
            m_lUserInsurerCnt = v_lUserInsurerCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestrictInsurerAccess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestrictInsurerAccess", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         GetFullAddress
    ' Author:       Alix Bergeret
    ' Description:  Return multiple addresses
    ' Date:         07/11/2002
    ' ***************************************************************** '
    Public Function GetMultipleAddresses(ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRowsAffected As Integer
        Dim sSQL As String = ""

        Try

            sSQL = "select address.address_cnt, party.party_cnt, address1, address2, address3, address4, postal_code, address_usage_type_id from address"
            sSQL = sSQL & " inner join party_address_usage on address.address_cnt = party_address_usage.address_cnt"
            sSQL = sSQL & " inner join party on party_address_usage.party_cnt = party.party_cnt"
            sSQL = sSQL & " WHERE party.party_cnt=" & v_lPartyCnt & " and address_usage_type_id not in(6) order by address.address_cnt DESC"

            ' Get records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetMultipleAddresses", bStoredProcedure:=False, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check r_vResultArray is array before returning
            If Not Informations.IsArray(r_vResultArray) Then

                ' Failed
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Could not find addresses", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMultipleAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            ' Return OK
            result = gPMConstants.PMEReturnCode.PMTrue



            '        Return result

        Catch ex As Exception
            ' Return Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMultipleAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMultipleAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name:         GetFSAPartyViewReasons
    ' Author:       Danny Davis
    ' Description:  Returns the FSA Party View Reasons
    ' Date:         28/10/2003
    ' ***************************************************************** '
    'MKW080104 PN9424 Allow Iscomplaint parameter
    Public Function GetFSAPartyViewReasons(ByRef r_vResultArray(,) As Object, Optional ByRef r_iIsComplaint As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            If False Then
                r_iIsComplaint = 0
            End If

            ' Return SQL Result
            'GetFSAPartyViewReasons = m_oDatabase.SQLSelect("{call spu_FSA_Party_View_Reason_selall}", _
            '"Select all FSA Party View Reason", True, , r_vResultArray)

            'MKW080104 PN9424 Supply parameter to stored procedure. START
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("include_complaints", CStr(r_iIsComplaint), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'developer guide no. 39 (Guide)
                GetFSAPartyViewReasons = .SQLSelect("spu_FSA_Party_View_Reason_selall", "Select all FSA Party View Reason", True, , r_vResultArray)
            End With
            'MKW080104 PN9424 Supply parameter to stored procedure. END

            Return result

        Catch excep As System.Exception



            ' Return Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFSAPartyViewReasons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFSAPartyViewReasons", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetFSAPartyQuestion
    ' Author:       Danny Davis
    ' Description:  Returns data for the Party's FSA Questions
    ' Date:         28/10/2003
    ' ***************************************************************** '
    Public Function GetFSAPartyQuestion(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByRef r_sPassword As String, ByRef r_vPostcode As String) As Integer

        Dim result As Integer = 0
        Dim sType As String = String.Empty
        Dim vAddress1 As String = String.Empty
        Dim vAddress2 As String = String.Empty
        Dim vAddress3 As String = String.Empty
        Dim vAddress4 As String = String.Empty

        Try

            If sPartyType = PMBConst.PMBPartyTypePersonalClient Then
                sType = "personal_client"
            Else
                sType = "corporate_client"
            End If

            ' Return SQL Result
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 39 (Guide)
                GetFSAPartyQuestion = .SQLSelect("spe_Party_" & sType & "_sel", "Select FSA Party Question", True)

                'check record count (MKW270104 PN9981)
                If (result = gPMConstants.PMEReturnCode.PMTrue) And (.Records.Count() > 0) Then
                    'Get the password
                    r_sPassword = gPMFunctions.NullToString(.Records.Item(1).Fields("tp_password"))

                    'Now get the postcode (the address fields are ignored)
                    result = GetFullAddress(lPartyCnt, vAddress1, vAddress2, vAddress3, vAddress4, r_vPostcode)
                End If
            End With

            Return result

        Catch excep As System.Exception



            ' Return Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFSAPartyQuestion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFSAPartyQuestion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         UpdateFSAPartyPassword
    ' Author:       Danny Davis
    ' Description:  Updates the TPPassword field on the Party
    ' Date:         28/10/2003
    ' ***************************************************************** '
    Public Function UpdateFSAPartyPassword(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByVal sPassword As String) As Integer

        Dim result As Integer = 0
        Dim sType As String = ""

        If sPartyType = PMBConst.PMBPartyTypePersonalClient Then
            sType = "personal_client"
        Else
            sType = "corporate_client"
        End If

        ' Return SQL Result
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("party_cnt", CStr(lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("tp_password", sPassword, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            'developer guide no. 39 (Guide)
            UpdateFSAPartyPassword = .SQLAction("spe_Party_" & sType & "_updpassword", "Select FSA Party Question", True)
        End With

        Return result



        ' Return Error
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFSAPartyPassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFSAPartyPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
    End Function


    ' ***************************************************************** '
    ' Check for associated clients
    ' ***************************************************************** '
    Public Function CheckAssociatedClients(ByVal v_lPartyCnt As Integer, ByRef r_lAssociatedClientCount As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sString As String = ""

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for associated clients
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", CStr(v_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAssociatedClientCountSQL, sSQLName:=ACGetAssociatedClientCountName, bStoredProcedure:=ACGetAssociatedClientCountStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then
                r_lAssociatedClientCount = gPMFunctions.NullToLong(vResultArray(0, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAssociatedClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAssociatedClients", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMError

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name:         GetVisibleAgentTypes
    ' Author:       David Cleaver
    ' Description:  Get All Visible Agent Types
    ' Date:         09/12/04
    ' ***************************************************************** '

    Public Function GetVisibleAgentTypes(ByRef r_vVisibleAgentTypes As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sString As String = ""

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetVisibleAgentTypesSQL, sSQLName:=ACGetVisibleAgentTypesName, bStoredProcedure:=ACGetVisibleAgentTypesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then


                r_vVisibleAgentTypes = vResultArray
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVisibleAgentTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVisibleAgentTypes", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMError

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyTypeByCode
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 22-06-2005 : PN21927
    ' ***************************************************************** '
    Public Function GetPartyTypeByCode(ByVal v_sCode As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyTypeByCode"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_sCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPartyTypeByCodeSQL, sSQLName:=kGetPartyTypeByCodeName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPartyTypeByCodeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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



    ' ***************************************************************** '
    ' Name: GetClientBlackListingReason
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 27-09-2005 : 358 - Client Blacklisting
    ' ***************************************************************** '
    Public Function GetClientBlackListingReason(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClientBlackListingReason"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", CStr(v_lPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetClientBlackListingReasonSQL, sSQLName:=kGetClientBlackListingReasonName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClientBlackListingReasonSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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


    ' ***************************************************************** '
    ' Name: GetCommissionFlag
    '
    ' Parameters: sShortName,i_rAllow_Commission_Flag
    '
    ' Description:
    '
    ' History:
    '           Created : Deepak 29/06/2006
    ' ***************************************************************** '
    Public Function GetCommissionFlag(ByRef sShortName As String, ByRef i_rAllow_Commission_Flag As Object) As Integer
        Dim result As Integer = 0
        Dim lRowsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party shortname parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_shortname", vValue:=sShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionFlag")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the allow commission flag parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="allow_commission_flag", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionFlag")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACPartyCommFromShortNameSQL, sSQLName:=ACPartyCommFromShortNameName, bStoredProcedure:=ACPartyCommFromShortNameStored, lRecordsAffected:=lRowsAffected)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionFlag")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' Get the allow_commission_flag of the record selected

            i_rAllow_Commission_Flag = m_oDatabase.Parameters.Item("allow_commission_flag").Value

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommissionFlag Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionFlag", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyAgentType
    '
    ' Description: Returns the party agent type
    ' ***************************************************************** '
    Public Function GetPartyAgentType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyAgentType As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT pat.code" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM party_agent pa " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN party_agent_type pat " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON pa.party_agent_type_id = pat.party_agent_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pa.party_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)

            ' Get party agent type from the database.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETPARTYAGENTTYPE", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                'Return the party agent type

                r_sPartyAgentType = CStr(vResultArray(0, 0)).Trim()
            Else
                r_sPartyAgentType = ""
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyAgentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyAgentType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAssociatedSubAgent
    '
    ' Description: Returns the Associated Sub-Agent details
    ' ***************************************************************** '
    Public Function GetAssociatedSubAgent(ByVal v_lLeadPartyCnt As Integer, ByRef r_vGetAssociatedSubAgent(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT p.party_cnt,p.shortname,pa.date_cancelled,p.name, " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CASE WHEN p.resolved_name  IS NULL THEN p.name " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHEN p.resolved_name='' THEN p.name " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ELSE p.resolved_name END 'resolved_name' " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM party_relationship pr " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN Party p ON pr.relation_cnt=p.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND commission_transaction=1 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN party_agent pa " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON p.party_cnt=pa.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pr.party_cnt = " & CStr(v_lLeadPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)

            ' Get associated sub agent details from the database.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GETASSOCIATEDSUBAGENT", bStoredProcedure:=False, vResultArray:=r_vGetAssociatedSubAgent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAssociatedSubAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAssociatedSubAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function GetAgentUserDetails(ByVal v_lPartyCnt As Integer, ByRef r_vGetAgentUserDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT pmu.user_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CASE WHEN pmu.full_name IS NULL THEN pmu.username " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "else pmu.full_name end " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM pmuser pmu " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN party_agent pa  " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON pa.party_cnt = pmu.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pmu.party_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND pmu.is_deleted =0 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND pmu.effective_date < = '" & CStr(DateTime.Now.ToString("yyyy-MM-dd")) & "' " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Order By " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(CASE WHEN pmu.full_name IS NULL THEN pmu.username else pmu.full_name end ) " & Strings.ChrW(13) & Strings.ChrW(10)
            ' Get associated agent details from the database.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAgentUserDetails", bStoredProcedure:=False, vResultArray:=r_vGetAgentUserDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentUserDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentUserDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ''' <summary>
    ''' Get the IsAgentReceiveCorrespondence Flag value
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_bCorrespondenceType"></param>
    ''' <returns></returns>
    ''' <remarks>WPR 3.1</remarks>
    Public Function CheckAgentReceiveCorrespondenceFlag(ByVal v_lPartyCnt As Integer, ByRef r_bCorrespondenceType As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            ' Get preferred correspondence from database.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kCheckReceivesCorrespondenceSQL, sSQLName:=kCheckReceivesCorrespondenceName, bStoredProcedure:=kCheckReceivesCorrespondenceStored, vResultArray:=vResult)


            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                r_bCorrespondenceType = -1
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Informations.IsNothing(vResult) Then
                r_bCorrespondenceType = ToSafeBoolean(vResult(0, 0))
            Else
                r_bCorrespondenceType = False
            End If
            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAgentReceiveCorrespondenceFlag Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAgentReceiveCorrespondenceFlag", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Finally

        End Try


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAccountBalance (Public)
    '
    ' Description: Gets Account Balance.
    '
    ' ***************************************************************** '
    Public Function GetAccountBalance(ByRef lPartyCnt As Integer, Optional ByRef dtResult As DataTable = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Execute SQL Statement
            m_lError = m_oDatabase.ExecuteDataTable(sSQL:=ACSelectAccountBalSQL, bStoredProcedure:=ACSelectAccountBalStored, sSQLName:=ACSelectAccountBalName, oRecordset:=dtResult)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetClaimIncurred (Public)
    '
    ' Description: Gets Claim Incurred.
    '
    ' ***************************************************************** '
    Public Function GetClaimIncurred(ByRef lPartyCnt As Integer, Optional ByRef dtResult As DataTable = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIncurred")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Execute SQL Statement
            m_lError = m_oDatabase.ExecuteDataTable(sSQL:=ACGetClaimIncurredSQL, bStoredProcedure:=ACGetClaimIncurredStored, sSQLName:=ACGetClaimIncurredName, oRecordset:=dtResult)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIncurred")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Some error has occured.
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimIncurred Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimIncurred", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function GetDocumentLibraryFromDB(ByVal v_lParty_Cnt As Integer, ByRef r_lDocument_Library As String, Optional ByVal v_lPartyShortName As String = "") As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lError = m_oDatabase.Parameters.Add(sName:="PartyCnt", vValue:=CStr(v_lParty_Cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentLibraryFromDB")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = m_oDatabase.Parameters.Add(sName:="PartyShortName", vValue:=CStr(v_lPartyShortName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentLibraryFromDB")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:="spu_SIR_Get_Party_Document_Library", sSQLName:="Get_Party_Document_Library", bStoredProcedure:=True)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentLibraryFromDB")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Item(0).Fields("DocumentLibrary") Is Nothing Then
                r_lDocument_Library = String.Empty
            Else
                r_lDocument_Library = m_oDatabase.Records.Item(0).Fields("DocumentLibrary")
            End If

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

        Catch excep As System.Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

End Class

