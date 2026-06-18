Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    '*************************************************************************
    ' Class Name: Interface
    ' Date: 17/02/1997
    ' Description: Main public class to accompany the interface form.
    ' Edit History:
    ' SP011298 - changes to support new business roadmap
    ' SP130199 - Change for Tim
    '*************************************************************************

    '=================
    'Private Constants
    '=================
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Interface"

    '==========================
    'Private Standard Variables
    '==========================
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_iTask As Integer
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As String = ""

    '============================
    'Private Instalment Variables
    '============================
    Private m_sProduct_Code As String = ""
    Private m_vPremiumFinanceTransactions(,) As Object = Nothing
    Private m_lPremiumFinanceCnt As Integer
    Private m_lPremiumFinanceVersion As Integer
    Private m_crFinanceDeposit As Decimal
    Private m_lReturn As Integer
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_iMTAType As Integer
    Private m_lInsuranceFileCntRenewal As Integer
    Private m_bIsSingleInstalmentPlan As Boolean
    'TR - This one is just held locally as GetKeys requires it.
    Private m_sClient_Code As String = ""

    'TR - The User Interface
    Private m_ofrmInterface As iPMBFinancePlanQuote.frmInterface
    'PN61609
    Private m_bIsFinanceAmountNetPremium As Boolean

    '==========================
    'Standard Public Properties
    '==========================
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = gPMFunctions.ToSafeString(Value)
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    '==============
    'Public Methods
    '==============
    '*************************************************************************
    'Name:          SetKeys (Standard Method)
    'Description:   Accepts the Required Info from the Navigate functions, and
    '               Stores all of the parameter members with the key array.
    'History:
    '*************************************************************************
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sLocalProductCode As String = ""
        Dim lUboundArray, lLboundArray As Integer

        Try

            'Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'Check a valid array has been passed in
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Get the lower and upper boundaries
            lLboundArray = vKeyArray.GetLowerBound(1)
            lUboundArray = vKeyArray.GetUpperBound(1)

            'Step through the input array.
            For lRowIndex As Integer = lLboundArray To lUboundArray

                'TR - Loop through all the properties in the array and assign
                'their values to the appropriate local variable


                Select Case gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRowIndex)).Trim()
                    Case PMNavKeyConst.PMKeyNameFinancePlanTransactions '"pfprem_finance_transactions"
                        'TR - Get the array of Transactions passed in

                        m_vPremiumFinanceTransactions = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex)
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        m_lInsuranceFileCnt = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                    Case PMNavKeyConst.PMKeyNameTaskGroupCode 'aka "Product_code"
                        'TR - Set the transaction Type for the whole Project
                        sLocalProductCode = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex)).Trim().ToUpper()
                        Select Case sLocalProductCode
                            Case "R", "REN"
                                m_sProduct_Code = "REN"
                            Case "N", "NB", "NEW BUSINESS"
                                m_sProduct_Code = "NB"
                            Case "M", "MTA"
                                m_sProduct_Code = "MTA"
                            Case Else
                        End Select
                    Case PMNavKeyConst.PMKeyNameClientCode
                        m_sClient_Code = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                    Case PMNavKeyConst.PMKeyNamePartyCnt
                        m_lPartyCnt = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                    Case PMNavKeyConst.PMKeyNameFinancePlanCnt '"pfprem_finance_cnt"
                        m_lPremiumFinanceCnt = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                    Case PMNavKeyConst.PMKeyNameFinancePlanVersion '"pfprem_finance_version"
                        m_lPremiumFinanceVersion = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                        'TR - Use this Constant for MTAType as it's only ever going to be used here
                    Case PMNavKeyConst.PMKeyNameTransactionType
                        m_iMTAType = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                    Case PMNavKeyConst.PMKeyNamePlanIsSingleInstalment
                        m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                        'PN61609
                    Case PMNavKeyConst.PMKeyNameFinanceAmountNetPremium
                        m_bIsFinanceAmountNetPremium = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRowIndex))
                End Select
            Next lRowIndex

            'TR - Get any missing data
            'TR - If the Product Code is not know, then work it out from the
            'insurance File count

            'DC171104 PN16814 this seems too simple a change to resolves issue.
            '    instead of working out the product_code based on policy,
            '    as this component only seems to be used for NB or REN and it is required that
            '    all NB and REN schemes are to be offered, then seems to work fine
            '    (Generic NB and generic MTA roadmaps appear to use the iCNPFQuote component rather than this)
            '    will leave as before for underwriting incase it messes that up

            Dim sUnderwritingOrAgencyFlag As String = ""

            m_lReturn = iPMFunc.getUnderwritingOrAgency(sUnderwritingOrAgencyFlag)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If m_sProduct_Code.Trim().Length = 0 Then
                    m_lReturn = GetProductCodeFromInsuranceFile()
                End If
            End If

            'Thinh Nguyen - if we are doing an mta then make sure set the mta version of the insurance file count
            'don't ask me why we are setting MTA version to m_lInsuranceFileCntRenewal. Its a mystery to me too
            If m_lInsuranceFileCntRenewal = 0 Then
                If m_sProduct_Code = "MTA" Then
                    If Information.IsArray(m_vPremiumFinanceTransactions) Then

                        'm_lInsuranceFileCntRenewal = gPMFunctions.ToSafeInteger(m_vPremiumFinanceTransactions(3, 0))
                        If Not Information.IsNothing(m_vPremiumFinanceTransactions(3, 0)) AndAlso Convert.ToString(m_vPremiumFinanceTransactions(3, 0)).Trim() = "" Then
                            m_lInsuranceFileCntRenewal = gPMFunctions.ToSafeInteger(m_vPremiumFinanceTransactions(3, 0))
                        End If
                    End If
                End If
            End If


            m_lReturn = g_oBusiness.UpdateUserProperty(g_sUsername, "Plan Transactions", m_vPremiumFinanceTransactions)

            m_lReturn = g_oBusiness.UpdateUserProperty(g_sUsername, "Plan Version", m_lPremiumFinanceVersion)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name:         GetProductCodeFromInsuranceFile
    ' Description:  Gets details from the insurance file to determine if this
    '               Product is NB, MTA or REN
    '*************************************************************************
    Private Function GetProductCodeFromInsuranceFile() As Integer
        Dim result As Integer = 0

        Dim vArrayOfInsuranceFile As Object = Nothing
        Dim lFolderCnt, lRenewalCount, lPolicyVersion, lPFPremFinanceCnt, lPFPremFinanceVer, lOrigInsuranceFileCnt As Integer

        Dim obSIRInsuranceFile As bSIRInsuranceFile.Business

        Dim obSIRInsuranceFolder As bSIRInsuranceFolder.Business

        Dim obSIRPremiumFinance As bSIRPremiumFinance.Business

        Const k_sFunctionName As String = "GetProductCodeFromInsuranceFile"



        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Make sure that there is a valid Insurance File ID
        If m_lInsuranceFileCnt > 0 Then
            'TR - Create a InsuranceFile object
            Dim temp_obSIRInsuranceFile As Object = Nothing
            'Developer guide No 218
            m_lReturn = g_oObjectManager.GetInstance(temp_obSIRInsuranceFile, "bSIRInsuranceFile.Business", gPMConstants.PMGetViaClientManager)
            obSIRInsuranceFile = temp_obSIRInsuranceFile
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to bSIRInsuranceFile." & "business object", ACApp, ACClass, k_sFunctionName)
                Return result
            End If

            'TR - If that worked OK, get the Insurance File Object

            m_lReturn = obSIRInsuranceFile.GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get details for " & m_lInsuranceFileCnt, ACApp, ACClass, k_sFunctionName)
                Return result
            End If

            m_lReturn = obSIRInsuranceFile.GetNext(vArrayOfInsuranceFile)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to GetNext for " & m_lInsuranceFileCnt, ACApp, ACClass, k_sFunctionName)
                Return result
            End If

            'TR - Make sure there is an Insurance File
            If Information.IsArray(vArrayOfInsuranceFile) Then
                'TR - Get the folder count
                lFolderCnt = gPMFunctions.NullToLong(vArrayOfInsuranceFile(InsuranceFileConst.ACInsuranceFolderCnt))
            End If

            'TR - Destroy un-needed object
            obSIRInsuranceFile = Nothing

            'TR - Make sure there is a valid InsuranceFolder
            If lFolderCnt > 0 Then
                'TR - Create a InsuranceFolder object
                Dim temp_obSIRInsuranceFolder As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_obSIRInsuranceFolder, "bSIRInsuranceFolder.Business", gPMConstants.PMGetViaClientManager)
                obSIRInsuranceFolder = temp_obSIRInsuranceFolder
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to bSIRInsuranceFolder." & "business object", ACApp, ACClass, k_sFunctionName)
                    Return result
                End If
                With obSIRInsuranceFolder

                    m_lReturn = .GetDetails(vInsuranceFolderCnt:=lFolderCnt)

                    m_lReturn = .GetNext(vRenewalCount:=lRenewalCount)
                End With
            Else
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get " & "InsuranceFolder ID", ACApp, ACClass, k_sFunctionName)
                Return result
            End If

            'TR - If the renewal count > 0 then this is a Renewal
            If lRenewalCount > 0 Then
                m_sProduct_Code = "REN"
                'TR - Otherwise, look at the policy version for previous versions
            Else
                'TR - Get the Policy Version Number
                lPolicyVersion = gPMFunctions.NullToLong(vArrayOfInsuranceFile(InsuranceFileConst.ACPolicyVersion))
                'TR - If this is the first Policy Version, then this is New Business
                If lPolicyVersion <= 1 Then
                    m_sProduct_Code = "NB"
                Else
                    'TR - Get the previous (last) version of this Policy
                    'TR - Create a bSIRPremiumFinance object
                    Dim temp_obSIRPremiumFinance As Object = Nothing
                    m_lReturn = g_oObjectManager.GetInstance(temp_obSIRPremiumFinance, "bSIRPremiumFinance.Business", gPMConstants.PMGetViaClientManager)
                    obSIRPremiumFinance = temp_obSIRPremiumFinance
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to bSIRPremiumFinance." & "business object", ACApp, ACClass, k_sFunctionName)
                        Return result
                    End If
                    'TR - Try and Get a Plan for the previous version

                    m_lReturn = obSIRPremiumFinance.GetFinancePlanFromInsFolderAndVersion(lPolicyVersion, lFolderCnt, lPFPremFinanceCnt, lPFPremFinanceVer, lOrigInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Get Finance Plan.", ACApp, ACClass, k_sFunctionName)
                        Return result
                    End If

                    'TR - If there is a PLan then this is an MTA, otherwise it is NB
                    If lPFPremFinanceCnt > 0 And lPFPremFinanceVer > 0 Then
                        m_sProduct_Code = "MTA"
                        'TR - Swap over the Insurance file counts for Quoting Instalments
                        'TR - The New Insurance File
                        m_lInsuranceFileCntRenewal = m_lInsuranceFileCnt
                        'TR - The Insurance file with an existing Plan
                        m_lInsuranceFileCnt = lOrigInsuranceFileCnt
                        If m_lPremiumFinanceCnt = 0 Then
                            m_lPremiumFinanceCnt = lPFPremFinanceCnt
                        End If
                        If m_lPremiumFinanceVersion = 0 Then
                            m_lPremiumFinanceVersion = lPFPremFinanceVer
                        End If
                    Else
                        m_lInsuranceFileCntRenewal = 0
                        m_sProduct_Code = "NB"
                    End If
                End If
            End If
        End If

        Return result

    End Function

    '*************************************************************************
    ' Name:         Initialise (Standard Method)
    ' Description:  Entry point for any initialisation code for this object.
    '*************************************************************************
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle As String
        Dim sHelpFile As String = String.Empty
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            'Store the language ID from the object manager to the public variables
            'to enable us to use them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
                g_sUsername = .UserName
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Get description from the resource file.

                sTitle = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 302, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 303, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'TR10/03/03 Issue 2873 - Causing a problem and not used anywhere
            'g_bGenericConnectionStatus = g_oObjectManager.GenericConnectionStatus
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = PMProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'developer guide no. 39
                'App.HelpFile = sHelpFile
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to " & "initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name: Terminate (Standard Method)
    ' Description: Entry point for any termination code for this object.
    '*************************************************************************
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    '*************************************************************************
    ' Name:         GetKeys (Standard Method)
    ' Description:  Stores all of the key array with the parameter members.
    '*************************************************************************
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Re-populate the Oupput array to return data to calling client
            'AAB - 06/05/2003 I am adding the 6th element for the MTAType
            ReDim vKeyArray(1, 6)
            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameFinancePlanTransactions
            ' PW050302 - this should no longer get passed through - is causing Amount
            ' Financed to double - blank it out 'cos it could already be passed in
            ' a previous map step get keys. (solution advised by DD).
            ' ISS2715

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ""

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sClient_Code

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lPremiumFinanceCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lPremiumFinanceVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameFinanceDeposit

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_crFinanceDeposit

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameMTAType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_iMTAType
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name:         GetSummary (Standard Method)
    ' Description:  Stores all of the summary array with the parameter members
    '*************************************************************************
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer
        'TR - Nothing to do
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    '*************************************************************************
    ' Name: SetProcessModes (Standard Method)
    ' Description: Set the optional process modes.
    '*************************************************************************
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then

                m_iTask = gPMFunctions.ToSafeInteger(vTask)
            End If

            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CType(gPMFunctions.ToSafeInteger(vNavigate), gPMConstants.PMENavigateButtonStatus)
            End If

            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(gPMFunctions.ToSafeInteger(vProcessMode), gPMConstants.PMEProcessMode)
            End If

            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = gPMFunctions.ToSafeString(vTransactionType)
            End If

            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If
            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set " & "the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name:         Start (Standard Method)
    ' Description:  Entry point for the object to start its processing.
    '*************************************************************************
    Public Function Start() As Integer
        Return ProcessInterface()
    End Function

    '===============
    'Private Methods
    '===============
    '*************************************************************************
    ' Name:         ProcessInterface (Standard Method)
    ' Description:  Calls the appropriate methods to process the interface.
    '*************************************************************************
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            result = gPMConstants.PMEReturnCode.PMFalse

            m_lStatus = m_ofrmInterface.Status
            Return result
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    '*************************************************************************
    ' Name:         LoadInterface (Standard Method)
    ' Description:  Loads the instance of the interface into memory and
    '               passes the parameters in.
    '*************************************************************************
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmInterface = New iPMBFinancePlanQuote.frmInterface()

        'Set the necessary data on the Interace.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            'Control Properties
            .ProductCode = m_sProduct_Code


            .PremiumFinanceTransactions = m_vPremiumFinanceTransactions
            .PremiumFinanceCnt = m_lPremiumFinanceCnt
            .PremiumFinanceVersion = m_lPremiumFinanceVersion
            .InsuranceFileCnt_Renewal = m_lInsuranceFileCntRenewal
            .PartyCnt = m_lPartyCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .MTAType = m_iMTAType

            '12/05/2003 - PWC - ENDVR00000765
            'Get the source id that relates to the insurance file (ie don't use the global one)
            .GetSourceID()
            .IsSingleInstalmentPlan = m_bIsSingleInstalmentPlan
            'PN61609
            .FinanceAmountNetPremium = m_bIsFinanceAmountNetPremium
        End With

        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error, so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If
        Return result

    End Function

    '*************************************************************************
    ' Name:         UnLoadInterface (Standard Method)
    ' Description:  Loads all the useful data from the Interface form locally
    '               and then Unloads the instance of the interface from memory.
    '*************************************************************************
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the required data from the Interace. Only the PF keys are required
        With m_ofrmInterface
            'TR - Get the data from the Instalment form
            m_lPremiumFinanceCnt = .PremiumFinanceCnt
            m_lPremiumFinanceVersion = .PremiumFinanceVersion
            m_lStatus = .Status


            m_vPremiumFinanceTransactions = .PremiumFinanceTransactions
            m_crFinanceDeposit = .FinanceDeposit
            m_iMTAType = .MTAType
        End With


        ' Unload and destroy the instance of the interface from memory.
        m_ofrmInterface.Close()
        m_ofrmInterface = Nothing
        Return result

    End Function

    '*************************************************************************
    ' Name:         ShowInterface (Standard Method)
    ' Description:  Displays the instance of the interface using the
    '               display state.
    '*************************************************************************
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_ofrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            'If m_ofrmInterface.ErrorNumber <> 0 Then
            result = m_ofrmInterface.ErrorNumber
            ' End If
        End If
        Return result

    End Function

    '============
    'Class Events
    '============
    Public Sub New()
        MyBase.New()
        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to " & "initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        'Exit Sub
        'End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As Interface_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Interface_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Interface_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
