Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    '
    ' History :
    ' CJB 08/03/2005 PN19313 For new renewal what-if quotes only, if the quote has not been saved and the
    '                user is exiting, delete the quote and the policy version. This is done in iPMUScreenControl and
    '                uctRiskScreenControl but new keyarray values in here were required - RenewalConfirmationMode and
    '                IsWhatIfQ.
    ' CJB 20/10/2006 PN24176 Changed ProcessInterface to pass KeyArray to frmInterface. This is all required to
    '                eventually pass into Dynamic Logic - necessary to determine which risk code has been selected
    '                so that Stargate Risk Screens can be viewed in Back Office and other values may be useful too.
    '

    Private Const ACClass As String = "Interface"

    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer

    ' Process mode variables
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_bTaskSet As Boolean


    ' Party Count
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    ' Insurance stuff
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lRiskId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lProductId As Integer
    Private m_lScreenId As Integer
    'Developer Guide No.101
    Private m_oGIS As Object
    Private m_vRiskTypeDetails As Object 'passed to child from parent
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)
    Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)

    ' AMB 10/01/03 - Start - IAG Spec 217
    Private m_oGISOriginal As Object
    ' AMB 10/01/03 - End - IAG Spec 217

    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH BEGIN
    '                   Add new variables for claim stuff
    '-------------------------------------------------------------------------------------
    Private m_lClaimID As Integer
    Private m_lClaimPerilID As Integer
    Private m_lPerilID As Integer
    Private m_lClaimWorkID As Integer
    Private m_lClaimWorkPerilID As Integer
    Private m_sClaimTransactionType As String = ""
    Private m_lClaimInsFileCnt As Integer
    Private m_lClaimRiskId As Integer
    Private m_bLossSchedule As Boolean
    Private m_lLossScheduleTypeId As Integer
    Private m_lPerilTypeId As Integer
    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH END
    '-------------------------------------------------------------------------------------

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20021024 : NRMA Changes - Sirius Process No 126 - Start
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private m_lClaimMode As Integer
    Private m_bShowModeLessForm As Boolean
    Private oInterface As frmInterface
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20021024 : NRMA Changes - Sirius Process No 126 - End
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private m_bSubScreen As Boolean
    Private m_sParentOIKey As String = ""
    Private m_sChildOIKey As String = ""
    Private m_sParentObjectName As String = ""
    Private m_sChildObjectName As String = ""
    Private m_sGISObjectName As String = ""
    Private m_vScreenValues As Object
    Private m_lChildIndex As Integer 'index number of child record beinf added (used as count var in XML)

    Private m_iIsRiAtRiskLevel As Integer
    Private m_iIsAutoReinsured As Integer

    Private m_iSourceID As Integer

    Private m_lReturn As Integer

    Private m_lObjectType As Integer 'Risk , specials, associated client etc

    'DP 30/05/2003 - added for Stargate 1.2
    Private m_cList As Collection
    Private m_aArray() As Object

    Private m_vXMLDataSet As Object


    'DP 30/05/2003 - added for passing data to child screen
    Private m_aChildDataFromParent As Object

    'True if we are processing a copied risk
    Private m_bCopyRisk As Boolean

    Private m_lGISPolicyLinkID As Integer

    Private m_bRenewalConfirmationMode As Boolean
    Private m_bIsWhatIfQ As Boolean
    Private m_sQemCode As String = ""
    Private m_vKeyArray(,) As Object

    Private m_dtPolicyStartDate As Date
    Private m_dtPolicyEndDate As Date
    Private m_lAgentCnt As Integer
    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer
    Private m_lCountryId As Integer
    Private m_sUsername As String = ""
    Private m_lOldInsuranceFileCnt As Integer
    Private m_sMtaType As String = ""
    Private m_bNoTransactions As Boolean

    Private m_lCaseID As Integer
    Private m_lBaseCaseID As Integer
    Private m_sCaseNumber As String = ""
    Private Const GISDMTypeRisk As Integer = 1
    Private m_bIsSilentQuote As Boolean = False
    Private m_bReserveLimitExceeded As Boolean
    Private m_dExceededReserve As Decimal

    Public Property ReserveLimitExceeded() As Boolean
        Get
            Return m_bReserveLimitExceeded
        End Get
        Set(ByVal Value As Boolean)
            m_bReserveLimitExceeded = Value
        End Set
    End Property


    Public ReadOnly Property XMLDataSet() As Object
        Get
            Return m_vXMLDataSet
        End Get
    End Property


    'DP 30/05/2003 - for Stargate v1.2
    Public WriteOnly Property List() As Collection
        Set(ByVal Value As Collection)
            m_cList = Value
        End Set
    End Property

    Public WriteOnly Property vArray() As Object()
        Set(ByVal Value As Object())
            m_aArray = Value
        End Set
    End Property
    'DP end

    'DP 30/05/2003 - added for passing data to child screen from parent
    Public WriteOnly Property ChildDataFromParent() As Object
        Set(ByVal Value As Object)


            m_aChildDataFromParent = Value
        End Set
    End Property
    'DP end
    Public WriteOnly Property ChildIndex() As Integer
        Set(ByVal Value As Integer)
            m_lChildIndex = Value
        End Set
    End Property
    Public WriteOnly Property RiskTypeDetails() As Object
        Set(ByVal Value As Object)


            m_vRiskTypeDetails = Value
        End Set
    End Property

    Public Property ObjectType() As Integer
        Get
            Return m_lObjectType
        End Get
        Set(ByVal Value As Integer)
            m_lObjectType = Value
        End Set
    End Property

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
            m_bTaskSet = True
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
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

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    Public Property SubScreen() As Boolean
        Get
            Return m_bSubScreen
        End Get
        Set(ByVal Value As Boolean)
            m_bSubScreen = Value
        End Set
    End Property

    Public Property ParentOIKey() As String
        Get
            Return m_sParentOIKey
        End Get
        Set(ByVal Value As String)
            m_sParentOIKey = Value
        End Set
    End Property

    Public Property ChildOIKey() As String
        Get
            Return m_sChildOIKey
        End Get
        Set(ByVal Value As String)
            m_sChildOIKey = Value
        End Set
    End Property

    Public Property ParentObjectName() As String
        Get
            Return m_sParentObjectName
        End Get
        Set(ByVal Value As String)
            m_sParentObjectName = Value
        End Set
    End Property

    Public Property ChildObjectName() As String
        Get
            Return m_sChildObjectName
        End Get
        Set(ByVal Value As String)
            m_sChildObjectName = Value
        End Set
    End Property

    Public Property GISObjectName() As String
        Get
            Return m_sGISObjectName
        End Get
        Set(ByVal Value As String)
            m_sGISObjectName = Value
        End Set
    End Property
    'Developer Guide No.101
    Public Property GIS() As Object
        Get
            Return m_oGIS
        End Get
        Set(ByVal Value As Object)
            m_oGIS = Value
        End Set
    End Property


    Public Property GISOriginal() As Object
        Get
            ' AMB 10/01/03 - Start - IAG 217 Spec
            Return m_oGISOriginal
            ' AMB 10/01/03 - End - IAG 217 Spec
        End Get
        Set(ByVal Value As Object)
            ' AMB 10/01/03 - Start - IAG 217 Spec
            m_oGISOriginal = Value
            ' AMB 10/01/03 - End - IAG 217 Spec
        End Set
    End Property

    Public Property ScreenValues() As Object
        Get
            Return m_vScreenValues
        End Get
        Set(ByVal Value As Object)


            m_vScreenValues = Value
        End Set
    End Property

    Public Property IsRiAtRiskLevel() As Integer
        Get
            Return m_iIsRiAtRiskLevel
        End Get
        Set(ByVal Value As Integer)
            m_iIsRiAtRiskLevel = Value
        End Set
    End Property


    Public Property IsAutoReinsured() As Integer
        Get
            Return m_iIsAutoReinsured
        End Get
        Set(ByVal Value As Integer)
            m_iIsAutoReinsured = Value
        End Set
    End Property

    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH BEGIN
    '                   Add new property get/let for Claims stuff
    '-------------------------------------------------------------------------------------

    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property


    Public Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property


    Public Property ClaimPerilID() As Integer
        Get
            Return m_lClaimPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPerilID = Value
        End Set
    End Property


    Public Property WorkClaimID() As Integer
        Get
            Return m_lClaimWorkID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimWorkID = Value
        End Set
    End Property


    Public Property WorkClaimPerilID() As Integer
        Get
            Return m_lClaimWorkPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimWorkPerilID = Value
        End Set
    End Property


    Public Property ClaimTransactionType() As String
        Get
            Return m_sClaimTransactionType
        End Get
        Set(ByVal Value As String)
            m_sClaimTransactionType = Value
        End Set
    End Property


    Public Property ClaimInsFileCnt() As Integer
        Get
            Return m_lClaimInsFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimInsFileCnt = Value
        End Set
    End Property


    Public Property ClaimRiskId() As Integer
        Get
            Return m_lClaimRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimRiskId = Value
        End Set
    End Property

    'CMG/PB 12092002 LossSchedule

    Public Property LossSchedule() As Boolean
        Get
            Return m_bLossSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bLossSchedule = Value
        End Set
    End Property


    Public Property LossScheduleTypeId() As Integer
        Get
            Return m_lLossScheduleTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lLossScheduleTypeId = Value
        End Set
    End Property


    Public Property PerilTypeId() As Integer
        Get
            Return m_lPerilTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeId = Value
        End Set
    End Property
    'End CMG

    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH END
    '-------------------------------------------------------------------------------------

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021024 - NRMA Changes - Sirius Process No 126 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public WriteOnly Property ShowModeLessForm() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowModeLessForm = Value
        End Set
    End Property
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021024 - NRMA Changes - Sirius Process No 126 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Public WriteOnly Property CopyRisk() As Boolean
        Set(ByVal Value As Boolean)
            m_bCopyRisk = Value
        End Set
    End Property

    Public WriteOnly Property GISPolicyLinkID() As Integer
        Set(ByVal Value As Integer)
            m_lGISPolicyLinkID = Value
        End Set
    End Property
    'WPR 33-75 added
    Public Property SourceId() As Short
        Get
            SourceId = m_iSourceID
        End Get
        Set(ByVal Value As Short)
            m_iSourceID = Value
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
    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property

    Public WriteOnly Property IsSilentQuote() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsSilentQuote = Value
        End Set
    End Property

    Public Property ExceededReserve() As Decimal
        Get
            Return m_dExceededReserve
        End Get
        Set(ByVal Value As Decimal)
            m_dExceededReserve = Value
        End Set
    End Property


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

#If quoteTiming Then

			QueryPerformanceFrequency performanceFreq
#End If
#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "initialisePMURiskStart"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "initialisePMURisk"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If


            ' CTAF 300800
            If g_oObjectManager Is Nothing Then
                g_oObjectManager = New bObjectManager.ObjectManager()
            End If

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bObjectManager.ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "initialisePMURiskEnd"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If
#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "initialisePMURiskEnd"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

            m_iSourceID = g_oObjectManager.SourceID
            m_sUsername = g_oObjectManager.UserName

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
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
                If m_bShowModeLessForm Then
                    ' We need to Unload it here
                    '        If oInterface.Visible = True Then
                    oInterface.Close()
                    '        End If
                    oInterface = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeys
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return the insurance file/folder counts
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)
            ReDim vKeyArray(1, 16)
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)

            ' Insurance File Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

            ' Insurance Folder Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFolderCnt

            ' Insurance Folder Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameRiskID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lRiskId

            ' Risk Cnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameRiskCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lRiskId

            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH BEGIN
            '                   Add new keys for claim stuff
            '-------------------------------------------------------------------------------------
            ' Claim Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameRealClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lClaimID
            ' Peril Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNamePerilID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lPerilID
            ' Claim Peril Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClaimPerilID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lClaimPerilID
            ' WORK Claim Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameWorkClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lClaimWorkID
            ' WORK Claim Peril Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameWorkClaimPerilID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_lClaimWorkPerilID
            ' Claim Transaction Type

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameClaimTransactionType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_sClaimTransactionType
            ' Claim Insurance File Count

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameClaimInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lClaimInsFileCnt
            ' Claim Risk Id

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameClaimRiskID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_lClaimRiskId
            ' CMG/PB 12092002 Loss Schedule

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNameLossSchedule

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_bLossSchedule


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameLossScheduleTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = m_lLossScheduleTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = "qem_code"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = m_sQemCode
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyNameScreenCaption

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)
            ' End CMG
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH END
            '-------------------------------------------------------------------------------------

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetQemCode(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sQemCode As String) As Integer
        Dim result As Integer = 0
        Dim bGis As Object
        Const sFunctionName As String = "GetQemCode"


        Dim oGIS As bGIS.Application
        Dim sDmCode As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oGIS As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oGIS, "bGis.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oGIS = temp_oGIS
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get instance of bGis.Application", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_GetQemCode
        End If

        'Get XML

        m_lReturn = oGIS.GetQemDmCode(v_lInsuranceFileCnt, r_sQemCode:=r_sQemCode, r_sDmCode:=sDmCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get run GetQemCode", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_GetQemCode
        End If

        GoTo Exit_GetQemCode

Exit_GetQemCode:
        ' terminate the business object.
        If Not (oGIS Is Nothing) Then

            oGIS.Dispose()
            oGIS = Nothing
        End If
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            For iLoop1 As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)).Trim()
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameRiskTypeID

                        m_lRiskTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        ' BSJ 21/08/01 - SBO Uses Risk Group ID for Type ID
                    Case PMNavKeyConst.PMKeyNameRiskGroupID

                        m_lRiskTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameProductID

                        m_lProductId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameRiskID

                        m_lRiskId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        ' BSJ 21/08/01 - SBO Uses Risk Cnt for Risk ID
                    Case PMNavKeyConst.PMKeyNameRiskCnt

                        m_lRiskId = CInt(Conversion.Val(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))))

                    Case "GIS_Screen_id"

                        m_lScreenId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        ' BSJ 21/08/01 - SBO Uses Risk Screen ID for gis Screen ID
                    Case PMNavKeyConst.PMKeyNameRiskScreenID

                        m_lScreenId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "GIS"
                        m_oGIS = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)

                    Case "sub_screen"

                        m_bSubScreen = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "ChildOIKey"

                        m_sChildOIKey = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        '-------------------------------------------------------------------------------------
                        '   15/07/2002  RVH BEGIN
                        '                   Add new variables for claim stuff
                        '-------------------------------------------------------------------------------------
                    Case PMNavKeyConst.PMKeyNameRealClaimID

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNamePerilID, PMNavKeyConst.PMKeyNameClaimPerilID

                        m_lPerilID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameWorkClaimID

                        m_lClaimWorkID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameWorkClaimPerilID

                        m_lClaimWorkPerilID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameClaimTransactionType

                        m_sClaimTransactionType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameClaimInsFileCnt

                        m_lClaimInsFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameClaimRiskID

                        m_lClaimRiskId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        '-------------------------------------------------------------------------------------
                        '   15/07/2002  RVH END
                        '-------------------------------------------------------------------------------------
                        'CMG/PB 12092002 LossSchedule
                    Case PMNavKeyConst.PMKeyNameLossSchedule

                        m_bLossSchedule = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    Case PMNavKeyConst.PMKeyNameLossScheduleTypeId

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) = "" Then
                            m_lLossScheduleTypeId = 0
                        Else

                            m_lLossScheduleTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        End If
                    Case PMNavKeyConst.PMKeyNamePerilTypeId

                        m_lPerilTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        'End CMG

                    Case PMNavKeyConst.PMKeyNameClaimMode
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'RAM20021024 : NRMA Changes - Sirius Process No 126
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        m_lClaimMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameRenewalConfirmationMode 'PN19313

                        m_bRenewalConfirmationMode = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameIsWhatIFQ 'PN19313

                        m_bIsWhatIfQ = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "qem_code"

                        m_sQemCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNamePolicyStartDate

                        m_dtPolicyStartDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "policy_end_date"

                        m_dtPolicyEndDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameAgentCnt

                        m_lAgentCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameRiskCodeID

                        m_lRiskCodeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameRiskGroupID

                        m_lRiskGroupId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameCountryId

                        m_lCountryId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "old_insurance_file_cnt"

                        m_lOldInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "MTA_type"

                        m_sMtaType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameNoTransaction

                        m_bNoTransactions = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)
                    Case PMNavKeyConst.PMKeyNameScreenCaption

                        m_sScreenCaption = gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5.1.1)
                    Case "is_reserve_limit_exceeded"

                        m_bReserveLimitExceeded = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)))
                End Select

            Next iLoop1

            ' Save the incoming keyarray for passing on to frmInterface...uctRiskScreen...Dynamic Logic  PN24176
            m_vKeyArray = vKeyArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            'Developer Guide No 143. 
            m_iTask% = CInt(vTask)
            m_bTaskSet = True
            m_lNavigate = CInt(vNavigate)
            m_lProcessMode = CInt(vProcessMode)
            m_sTransactionType = CStr(vTransactionType)
            m_dtEffectiveDate = CDate(vEffectiveDate)

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessInterface
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        'Dim oInterface As frmInterface
        'Dim result As Integer = 0
        Dim sXml As String = ""



        'result = gPMConstants.PMEReturnCode.PMTrue
        oInterface = New frmInterface()

        ' Set the party count
        With oInterface
            .Task = m_iTask%
            .TransactionType = m_sTransactionType
            .PartyCnt = m_lPartyCnt
            .ShortName = m_sShortName
            .SourceID = m_iSourceID
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .RiskTypeId = m_lRiskTypeId
            .RiskId = m_lRiskId
            .ProductId = m_lProductId
            .SubScreen = m_bSubScreen
            .ScreenId = m_lScreenId
            .ParentOIKey = m_sParentOIKey
            .ChildOIKey = m_sChildOIKey
            .ParentObjectName = m_sParentObjectName
            .ChildObjectName = m_sChildObjectName
            .ObjectType = m_lObjectType


            .RiskTypeDetails = m_vRiskTypeDetails
            .ChildIndex = m_lChildIndex
            .ScreenCaption = m_sScreenCaption

            If Not (m_oGIS Is Nothing) Then
                .GIS = m_oGIS
            End If
            ' AMB 10/01/03 - Start - IAG 217 Spec
            If Not (m_oGISOriginal Is Nothing) Then


                'Developer Guide No. 24
                .GISOriginal = m_oGISOriginal
            End If
            ' AMB 10/01/03 - Start - IAG 217 Spec
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH BEGIN
            '                   Set new properties for Claims stuff
            '-------------------------------------------------------------------------------------
            .ClaimID = m_lClaimID
            .PerilID = m_lPerilID
            .ClaimPerilID = m_lClaimPerilID
            .WorkClaimID = m_lClaimWorkID
            .WorkClaimPerilID = m_lClaimWorkPerilID
            .ClaimTransactionType = m_sClaimTransactionType
            .ClaimInsFileCnt = m_lClaimInsFileCnt
            .ClaimRiskId = m_lClaimRiskId
            'CMG/PB 13092002 LossSchedule
            .LossSchedule = m_bLossSchedule
            .LossScheduleTypeId = m_lLossScheduleTypeId
            .PerilTypeId = m_lPerilTypeId
            'End CMG
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH END
            '-------------------------------------------------------------------------------------
            'DP 30/05/2003 - added for Stargate 1.2
            If Not (m_cList Is Nothing) Then
                .List = m_cList
            End If
            If Information.IsArray(m_aArray) Then
                .vArray = VB6.CopyArray(m_aArray)
            End If
            'DP end

            'DP 30/05/2003 - added for passing data from parent to child
            If Information.IsArray(m_aChildDataFromParent) Then


                .ChildDataFromParent = m_aChildDataFromParent
            End If
            'DP end

            .ClaimMode = m_lClaimMode
            .CopyRisk = m_bCopyRisk

            .RenewalConfirmationMode = m_bRenewalConfirmationMode 'PN19313
            .IsWhatIfQ = m_bIsWhatIfQ 'PN19313

            .GISPolicyLinkID = m_lGISPolicyLinkID

            .KeyArray = m_vKeyArray 'PN24176
            .CaseID = m_lCaseID
            .BaseCaseID = m_lBaseCaseID
            .CaseNumber = m_sCaseNumber
            .CallingAppName = m_sCallingAppName
            .NoTransactions = m_bNoTransactions
        End With
#If quoteTiming Then

			performanceCtr(performancecntrCntr, 1) = "    Load uct Interface"
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

        'Developer Guide No. 68

#If quoteTiming Then

			Dim sTotals As String
			Dim cTotal As Currency
			Dim iLoop As Integer
			cTotal = 0
			For iLoop = 0 To performancecntrCntr - 1 Step 2
			sTotals = sTotals & Format(iLoop / 2 + 1, "00") & vbTab & performanceCtr(iLoop, 1) & vbTab & Format((performanceCtr(iLoop + 1, 0) - performanceCtr(iLoop, 0)) / performanceFreq, "0.0000") & vbTab & Format((performanceCtr(iLoop + 1, 0) - performanceCtr(0, 0)) / performanceFreq, "0.0000") & vbCrLf
			Next
			Clipboard.SetText Clipboard.GetText & vbCrLf & sTotals
			'    MsgBox sTotals
#End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'RAM20021024 : NRMA Changes - Sirius Process No 126 - Start
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_bIsSilentQuote Then
            m_lReturn = oInterface.ProcessOKClickForSilentQuote()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ProcessInterface = PMEReturnCode.PMFalse
            End If

        Else
            If m_bShowModeLessForm Then
                ' Show the form
                oInterface.Show()
            Else
                'Get Qem Code
                If m_sQemCode = "" Then
                    If m_sTransactionType = gPMConstants.PMTransactionTypeMTA Then
                        m_lReturn = GetQemCode(v_lInsuranceFileCnt:=m_lOldInsuranceFileCnt, r_sQemCode:=m_sQemCode)
                    ElseIf m_sTransactionType = gPMConstants.PMTransactionTypeNB OrElse _
                            m_sTransactionType = gPMConstants.PMTransactionTypeRenewals Then
                        m_lReturn = GetQemCode(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_sQemCode:=m_sQemCode)
                    End If

                End If

                ' Show the form (if required)
                If m_sQemCode = "BROKERLINK" Then
                    'Run NB Quote Stateless
                    m_lReturn = RunFormlessNBQuote(v_lInsFileCnt:=m_lInsuranceFileCnt, _
                                                       v_sDataModelCode:="MULTIPAC", _
                                                       v_sXmlDataset:=sXml)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    End If
                Else
                    oInterface.ShowDialog()
                End If
            End If
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'RAM20021024 : NRMA Changes - Sirius Process No 126 - End
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ' Get the insurance file cnt
        With oInterface
            m_lInsuranceFileCnt = .InsuranceFileCnt
            m_lInsuranceFolderCnt = .InsuranceFolderCnt
            m_lRiskId = .RiskId
            m_iIsRiAtRiskLevel = .IsRiAtRiskLevel
            m_iIsAutoReinsured = .IsAutoReinsured


            m_vScreenValues = .ScreenValues
            If m_sQemCode <> "BROKERLINK" Then m_lStatus = .Status
            m_sChildOIKey = .ChildOIKey
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH BEGIN
            '                   Get new properties for Claims stuff
            '-------------------------------------------------------------------------------------
            m_lClaimID = .ClaimID
            m_lPerilID = .PerilID
            m_lClaimPerilID = .ClaimPerilID
            m_lClaimWorkID = .WorkClaimID
            m_lClaimWorkPerilID = .WorkClaimPerilID
            m_sClaimTransactionType = .ClaimTransactionType
            m_lClaimInsFileCnt = .ClaimInsFileCnt
            m_lClaimRiskId = .ClaimRiskId


            m_vXMLDataSet = .XMLDataSet
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH END
            '-------------------------------------------------------------------------------------
            m_sCaseNumber = .CaseNumber
            m_bReserveLimitExceeded = .ReserveLimitExceeded
            m_dExceededReserve = .ExceededReserve
        End With

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'RAM20021024 : NRMA Changes - Sirius Process No 126 - Start
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_bShowModeLessForm Then
            ' Don't terminate it here
        Else
            ' Remove the instance
            oInterface.Close()
            oInterface = Nothing
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'RAM20021024 : NRMA Changes - Sirius Process No 126 - End
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return nResult

    End Function
    Private Function RunFormlessNBQuote(ByVal v_lInsFileCnt As Integer, ByVal v_sDataModelCode As String, ByVal v_sXmlDataset As String) As Integer
        Dim result As Integer = 0
        Dim bGis As Object

        Const sFunctionName As String = "RunFormlessNBQuote"


        Dim oGIS As bGIS.Application
        Dim sXml, sXmlDef As String
        Dim vAdditionalData As Object
        Dim dtCoverStartDate As Date
        Dim lPolicyTypeID, lQuoteType, lTransactionType As Integer
        Dim sXmlStore As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oGIS As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oGIS, "bGis.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oGIS = temp_oGIS
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get instance of bGis.Application", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_RunFormlessNBQuote
        End If

        'Get XML

        m_lReturn = oGIS.LoadFromDB(r_sXMLDataSetDef:=sXmlDef, r_sXMLDataset:=sXml, v_sGisDataModelCode:=v_sDataModelCode, r_vInsuranceFileCnt:=v_lInsFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get run LoadFromDB", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_RunFormlessNBQuote
        End If

        If (m_iTask <> gPMConstants.PMEComponentAction.PMView) Or Not (m_bTaskSet) Then
            'Clear Old Quote

            m_lReturn = oGIS.ClearPBQuoteOutputs(v_sGisDataModelCode:=v_sDataModelCode, r_sXMLDataset:=sXml)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get clear old outputs", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                GoTo Exit_RunFormlessNBQuote
            End If

            'Remove Old Quotes From DB

            m_lReturn = oGIS.SaveToDB(v_sGisDataModelCode:=v_sDataModelCode, r_sXMLDataset:=sXml)
        Else
            sXmlStore = sXml
        End If

        m_lReturn = GetPolicyTypeIDForInsFile(v_lInsuranceFileCnt:=v_lInsFileCnt, r_lPolicyTypeID:=lPolicyTypeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get policy type id for InsFile", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_RunFormlessNBQuote
        End If

        'Prepare Additional Data array for NBQuote
        ReDim vAdditionalData(1, 11)

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "CHILD_OIKEY"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sChildOIKey

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "DATA_MODEL_TYPE"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = GISDMTypeRisk

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "POLICY_TYPE_ID"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = lPolicyTypeID

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Party_Cnt"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lPartyCnt

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Policy_Start_Date"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_dtPolicyStartDate

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "Policy_End_Date"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_dtPolicyEndDate

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "Agent_Cnt"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lAgentCnt

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "Risk_Code_Id"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lRiskCodeId

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "Risk_Group_Id"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_lRiskTypeId

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = "Country_Id"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_lCountryId

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = "old_insurance_file_cnt"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lOldInsuranceFileCnt

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "MTA_type"

        vAdditionalData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_sMtaType

        If m_dtPolicyStartDate = #12/30/1899# Then
            dtCoverStartDate = DateTime.Now
        Else
            dtCoverStartDate = m_dtPolicyStartDate
        End If

        m_lReturn = GetTransactionType(r_llTransactionType:=lTransactionType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get transaction type", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_RunFormlessNBQuote
        End If

        EncodeTransactionScreenAndType(lQuoteType, lTransactionType, m_lScreenId, 1) 'Quote

        'Set Gis Process Modes

        m_lReturn = oGIS.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        'Run NB Quote Stateful

        m_lReturn = oGIS.NBQuoteStateful(v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=dtCoverStartDate, v_lQuoteType:=lQuoteType, r_vAdditionalDataArray:=vAdditionalData, v_lRiskGroupID:=m_lRiskTypeId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get run NBQuote stateful", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_RunFormlessNBQuote
        End If

        'SaveToDB
        If ((m_iTask <> gPMConstants.PMEComponentAction.PMView) And (m_bTaskSet)) Or Not (m_bTaskSet) Then

            m_lReturn = oGIS.SaveToDBStateful()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'Error Silently, assume cancelled
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to save to db", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, bSilent:=True)
                GoTo Exit_RunFormlessNBQuote
            End If
        End If


        v_sXmlDataset = oGIS.ReturnAsXML(r_sXMLDataset:=v_sXmlDataset)

        If (m_iTask = gPMConstants.PMEComponentAction.PMView) And (m_bTaskSet) Then

            m_lReturn = oGIS.SaveToDB(v_sGisDataModelCode:=v_sDataModelCode, r_sXMLDataset:=sXmlStore)
        End If

        GoTo Exit_RunFormlessNBQuote

Exit_RunFormlessNBQuote:
        ' terminate the business object.
        If Not (oGIS Is Nothing) Then

            oGIS.Dispose()
            oGIS = Nothing
        End If
        Return result
        Resume
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: EncodeTransactionScreenAndType
    '
    ' Description: Encodes Transaction, Screen id and tYpe from encoded value
    '              Originally TTTSSYY
    '              Now        1TTTSSSSYY
    '
    ' History: 19/12/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Integer, ByRef r_lGISScreenId As Integer, ByRef r_lQuoteType As Byte)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".EncodeTransactionScreenAndType")

        Try

            'new format 1TTTSSSSYY
            r_lEncoded = 1000000000 + (r_lTransactionType * 1000000) + (r_lGISScreenId * 100) + r_lQuoteType

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".EncodeTransactionScreenAndType")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".EncodeTransactionScreenAndType")


            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeTransactionScreenAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Function GetTransactionType(ByRef r_llTransactionType As Integer) As Integer
        Dim result As Integer = 0

        Dim oBusiness As bSIRRiskScreen.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRRiskScreen.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Set the process modes for the busines object.

            m_lReturn = oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)


            r_llTransactionType = oBusiness.GetTransactionType()


            oBusiness.Dispose()

            oBusiness = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Public Function GetPolicyTypeIDForInsFile(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPolicyTypeID As Integer) As Integer
        Dim result As Integer = 0
        Dim bSirRiskScreen As Object

        Const sFunctionName As String = "GetPolicyTypeIDForInsFile"


        Dim oRiskStateless As bSIRRiskScreen.Stateless

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRiskStateless As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRiskStateless, "bSirRiskScreen.Stateless", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRiskStateless = temp_oRiskStateless
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get instance of bSirRiskScreen.Stateless", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            End If


            m_lReturn = oRiskStateless.GetPolicyTypeIDForInsFile(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lPolicyTypeID:=r_lPolicyTypeID)


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Policy Type ID for InsFile", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' terminate the business object.
            If Not (oRiskStateless Is Nothing) Then

                oRiskStateless.Dispose()
                
                oRiskStateless = Nothing
            End If

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name          : switchTo
    '
    ' Description   : Switches the focus to this form. Called from the
    '                   Interface which creates this
    ' Edit History  :
    ' RAM20021024   : Created
    ' ***************************************************************** '
    Public Function SwitchTo() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not oInterface.Visible Then
                ' Make it visible if it is hide
                oInterface.Visible = True
            End If

            ' Set the focus to it, (we have the form loaded and staying behind)
            oInterface.SwitchTo()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="switchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



End Class

