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
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' SP011298 - changes to support new business roadmap
    ' SP130199 - Change for Tim
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    Private m_bDontDeleteScheme As Boolean
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_vFinancePlanTransactions As Object

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    '
    Private m_lFinancePlanCnt As Integer
    Private m_lFinancePlanVersion As Integer
    Private m_vFinancePlanArray As Object
    Private m_vFinancePlanMTATransArray As Object
    Private m_vFinancePlanTransArray As Object
    Private m_sTransType As String = ""
    Private m_bHistory As Boolean
    Private m_bSpawned As Boolean
    Private m_sRunningContext As String = ""
    Private m_iMTAType As Integer
    Private m_sPaymentMethod As String = "" 'PN12107

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030404   : Issue 2915 Changes    - START
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private m_iBankAccountDetails As Object
    Private m_iCreditCardDetails As Object
    Private m_bUseExistingBankDetails As Boolean
    Private m_bUseExistingCreditCardDetails As Boolean
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030404   : Issue 2915 Changes    - END
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    'DC300904 PN14114 check for underwriting/broking
    Private m_sUnderwriting As String = ""

    ' Member to hold instance of form interface
    Private m_ofrmInterface As iPMBFinancePlanMaint.frmInterface

    Private m_lSilentTransact As Integer
    'Start  Written Status
    Private m_insuranceFileCnt As Integer
    'End  Written Status
    Private m_bIsBackdatedMTARequired As Boolean
    Private m_iAccountId As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    'PN74070
    Public WriteOnly Property DontDeleteScheme() As Boolean
        Set(ByVal Value As Boolean)

            m_bDontDeleteScheme = Value

        End Set
    End Property

    Public WriteOnly Property SilentTransact() As Integer
        Set(ByVal Value As Integer)
            m_lSilentTransact = Value
        End Set
    End Property

    Public WriteOnly Property AccountId() As Integer
        Set(ByVal Value As Integer)
            m_iAccountId = Value
        End Set
    End Property
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
            m_lPMAuthorityLevel = CStr(Value)
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public Property partyCnt() As Integer
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
    Public Property FinancePlanMTATransArray() As Object
        Get

            Return m_vFinancePlanMTATransArray

        End Get
        Set(ByVal Value As Object)

            m_vFinancePlanMTATransArray = Value

        End Set
    End Property
    Public Property History() As Boolean
        Get

            Return m_bHistory

        End Get
        Set(ByVal Value As Boolean)

            m_bHistory = Value

        End Set
    End Property
    Public Property RunningContext() As String
        Get
            Return m_sRunningContext
        End Get
        Set(ByVal Value As String)
            m_sRunningContext = Value
        End Set
    End Property

    Public Property Spawned() As Boolean
        Get
            Return m_bSpawned
        End Get
        Set(ByVal Value As Boolean)
            m_bSpawned = Value
        End Set
    End Property

    Public Property FinancePlanCnt() As Integer
        Get

            Return m_lFinancePlanCnt

        End Get
        Set(ByVal Value As Integer)

            m_lFinancePlanCnt = Value

        End Set
    End Property
    Public Property FinancePlanVersion() As Integer
        Get

            Return m_lFinancePlanVersion

        End Get
        Set(ByVal Value As Integer)

            m_lFinancePlanVersion = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
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

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
                g_sUserName = .UserName
                g_lCountryID = .CountryID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            g_bEditFinancePlanAuthority = True '2005 Client Manager Security

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide 76
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Dim temp_g_oFindInsurance As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oFindInsurance, "bSIRFindInsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oFindInsurance = temp_g_oFindInsurance


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to process the interface.
                iPMFunc.LogMessage( _
                        iType:=gPMConstants.PMELogLevel.PMLogError, _
                        sMsg:="Failed to get bSIRFindInsurance.Form", _
                        vApp:=ACApp, _
                        vClass:=ACClass, _
                        vMethod:="Form_Initialise", _
                        vErrNo:=Err.Number, _
                        vErrDesc:=Err.Description)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Function
            End If


            'sj 3/11/99 - start
            'g_bGenericConnectionStatus = g_oObjectManager.GenericConnectionStatus
            'sj 3/11/99 - end

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = PMProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'developer guide no. 39 (no solution)
                'App.HelpFile = sHelpFile
            End If

            'Party Bank Details
            Dim temp_g_oPartyBank As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oPartyBank, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oPartyBank = temp_g_oPartyBank

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No 76
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If Not Spawned Then
                    If g_oBusiness IsNot Nothing Then
                        g_oBusiness.Dispose()
                        g_oBusiness = Nothing
                    End If
                    If g_oPartyBank IsNot Nothing Then
                        g_oPartyBank.Dispose()
                        g_oPartyBank = Nothing
                    End If
                    If g_oObjectManager IsNot Nothing Then
                        g_oObjectManager.Dispose()
                        g_oObjectManager = Nothing
                    End If
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the correct key array item.
                'SP011298 - changes to support new business roadmap

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameFinancePlanCnt
                        m_lFinancePlanCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameFinancePlanVersion
                        m_lFinancePlanVersion = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameFinancePlanTransactions
                        If m_sTransactionType = "NB" Or m_sTransactionType = "MTR" Then

                            m_vFinancePlanTransArray = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                        Else

                            m_vFinancePlanMTATransArray = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                        End If
                    Case PMNavKeyConst.PMKeyNameTaskGroupCode
                        m_sTransType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameMTAType

                        m_iMTAType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'PN12107
                    Case gSIRLibrary.SIRLookupPaymentMethod

                        m_sPaymentMethod = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        '2005 Client Manager Security
                    Case PMKeyNameFinancePlanEditAuthority

                        g_bEditFinancePlanAuthority = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "sg_pfprem_finance_transactions"

                        m_vFinancePlanTransactions = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                    Case PMNavKeyConst.PMKeyNameIsOutOfSequence

                        m_bIsBackdatedMTARequired = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'Start Written Status
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        m_insuranceFileCnt = CLng(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'End Written Status
                    Case "Payment Account ID"
                        m_iAccountId = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                End Select

            Next lRow
            If m_sTransactionType = "MTC" Then
                m_lSilentTransact = 1
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

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

            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If

            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If

            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If

            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oFindTransaction Is Nothing) Then

                m_lReturn = g_oFindTransaction.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTypeOfBusiness:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not (g_oBusiness Is Nothing) Then
                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")
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
    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        'DC300904 PN14114 option for instalments
        Dim result As Integer = 0
        Dim sInstalment As String = ""
        'Start Written Status
        Dim oInsuranceFileType As Object = Nothing
        Dim vResultArray(,) As Object = Nothing
        Dim iInsuranceFileTypeID As Integer
        Const kMethodName As String = "Start"
        'End Written Status
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'PN12107
            ' Dont do anything if it wasnt an instalment
            If ((m_sPaymentMethod <> "") And (m_sPaymentMethod.ToLower() <> "instalments")) Or m_bIsBackdatedMTARequired Then

                'DC300904 PN14114 get option for instalments
                m_lReturn = CType(iPMFunc.GetSystemOption(200, sInstalment), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not get System Option: " & "Allow Instalments", ACApp, ACClass, "Start")
                    Return result
                End If

                'DC300904 PN14114 get whether underwriting or broking

                If g_oBusiness.GetHiddenOption(r_sValue:=m_sUnderwriting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return result
                End If

                m_sUnderwriting = m_sUnderwriting.ToUpper()

                'DC221004 PN16050 decision made if instalments option set do not run this component
                '        'DC300904 PN14114 if underwriting then exit, but only exit in broking
                '
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Return result
                '

            End If
            'Start Written Status
            m_lReturn = g_oObjectManager.GetInstance( _
                                            oObject:=oInsuranceFileType, _
                                            sClassName:="bControlTrans.Automated", _
                                            vInstanceManager:=PMGetLocalBusiness)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(Start, "Failed to get instance of bControlTrans.Automated", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oInsuranceFileType.GetInsuranceRef(m_insuranceFileCnt, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to call the method GetInsuranceRef of bControlTrans.Automated", gPMConstants.PMELogLevel.PMLogError)
            End If

            oInsuranceFileType.Dispose()
            oInsuranceFileType = Nothing

            If IsArray(vResultArray) Then
                iInsuranceFileTypeID = ToSafeInteger(vResultArray(2, 0))
            End If

            If iInsuranceFileTypeID = 11 Then
                Exit Function
            End If
            'End Written Status
            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Start Written Status
            If Not oInsuranceFileType Is Nothing Then
                oInsuranceFileType.Dispose()
                oInsuranceFileType = Nothing
            End If
            'End Written Status
            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_lSilentTransact = 1 Then
            m_ofrmInterface.SilentTransaction()
        Else
            ' Display the interface.
            m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)
        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    ' Edit History  :
    ' RAM20030404   : Issue 2915 Changes
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmInterface = New iPMBFinancePlanMaint.frmInterface()

        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}
            .FinancePlanCnt = m_lFinancePlanCnt
            .FinancePlanVersion = m_lFinancePlanVersion
            .MTAVersion = m_lFinancePlanVersion

            'developer guide no. 24 
            .FinancePlanMTATransArray = m_vFinancePlanMTATransArray

            'developer guide no. 24
            .FinancePlanTransArray = m_vFinancePlanTransArray
            .RunningContext = m_sRunningContext
            .History = m_bHistory
            .Spawned = m_bSpawned
            .MTAType = m_iMTAType

            'developer guide no. 24
            .SGTransactionArray = m_vFinancePlanTransactions

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030404   : Set the following additional Properties
            '                   Ref. Issue 2915 Changes
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            .UseExistingBankDetails = m_bUseExistingBankDetails
            .UseExistingCreditCardDetails = m_bUseExistingCreditCardDetails

            .SelectedBankAccountDetails = m_iBankAccountDetails

            .SelectedCreditCardDetails = m_iCreditCardDetails
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030404   : Issue 2915 Changes - END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            .SilentTransact = m_lSilentTransact
            .DontDeleteScheme = m_bDontDeleteScheme
            .AccountID = m_iAccountId
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        m_ofrmInterface.LoadFunction()

        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmInterface.Close()
        m_ofrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_ofrmInterface, lDisplayState)
        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_ofrmInterface.ErrorNumber <> 0 Then
                result = m_ofrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '***************************************************************************
    ' Name          : SetSelectedPaymentDetails
    ' Description   : Public function to set the details of the selected
    '                   payment options (i.e. Bank Account Details or Credit Card Details)
    ' Author        : Ram Chandrabose
    ' Created on    : 2003/04/04
    ' Notes         : 1. Introduced as a part of Issue 2915 Changes
    '                 2. If these values are set, then the screen should pre populate
    '                       the supplied values in appropriate fields.
    '                 3. Also, these fields should be diabled, so that the user can't
    '                       edit these.
    ' Edit History  :
    ' RAM20030404   : Created
    '***************************************************************************
    Public Function SetSelectedPaymentDetails(ByVal v_vSelectedPaymentDetails As Object, ByVal v_bUseExistingBankAccount As Boolean, ByVal v_bUseExistingCreditCard As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bUseExistingBankDetails = v_bUseExistingBankAccount
            m_bUseExistingCreditCardDetails = v_bUseExistingCreditCard

            If Information.IsArray(v_vSelectedPaymentDetails) Then
                If v_bUseExistingBankAccount Then

                    m_iBankAccountDetails = v_vSelectedPaymentDetails

                    m_iCreditCardDetails = ""
                ElseIf v_bUseExistingCreditCard Then

                    m_iBankAccountDetails = ""

                    m_iCreditCardDetails = v_vSelectedPaymentDetails
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetSelectedPaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetSelectedPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
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

