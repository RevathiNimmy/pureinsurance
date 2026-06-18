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
    ' Date: 10/10/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' RAW 14/02/2003 : ISS2153 : changed GetAccountID function to handle accounts from any LedgerType
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    Dim m_frmInterface As frmInterface

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    'Private m_lStatus As Long

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lPartyCnt As Integer
    Private m_sAccountCode As String = ""
    Private m_lAccountId As Integer
    Private m_lPremiumFinanceCnt As Integer
    Private m_vTransactionArray As Object
    Private m_vSourceArray As Object
    Private m_iBranchID As Integer
    Private m_lDrillLevel As Integer
    Private m_iDrillCompany As Integer
    Private m_bIsAgent As Boolean 'DD04022002
    Private m_lInsuranceFileCnt As Integer
    Private m_lPlanInsuranceFolderCnt As Integer 'Thinh Nguyen 01/02/2004
    Private m_bIsSingleInstalmentPlan As Boolean
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' PMAuthorityLevel
    Private m_lPMAuthorityLevel As Integer

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return VB6.CopyArray(m_vSourceArray)

        End Get
        Set(ByVal Value As Object)

            ' Set the valid sources for the user
            m_vSourceArray = Value
        End Set
    End Property
    Public Property TransactionArray() As Object
        Get
            ' Return the Transaction Array
            Return m_vTransactionArray
        End Get
        Set(ByVal Value As Object)
            ' Set the valid sources for the user


            m_vTransactionArray = Value
        End Set
    End Property
    Public Property DrillLevel() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lDrillLevel
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lDrillLevel = Value
        End Set
    End Property

    Public Property DrillCompany() As Integer
        Get

            ' Return the objects parameter value.
            Return m_iDrillCompany

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_iDrillCompany = Value

        End Set
    End Property

    Public Property PremiumFinanceCnt() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lPremiumFinanceCnt

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lPremiumFinanceCnt = Value

        End Set
    End Property
    ' {* USER DEFINED CODE (End) *}
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

    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

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
                g_iUserID = .UserID
                g_sUserName = .UserName
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bACTFindTransaction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oSirConfig As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oSirConfig, "bSIRSolutionConfig.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oSirConfig = temp_g_oSirConfig

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSirSolutionConfig.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            ' the public object manager.
            Dim temp_g_oOrionLink As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oOrionLink, "bSirOrionLink.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oOrionLink = temp_g_oOrionLink

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTInsurerPaymentGroups.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            ' the public object manager.
            Dim temp_g_oPremiumFinance As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oPremiumFinance = temp_g_oPremiumFinance

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRPremiumFinance.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))

                Return result
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If g_oSirConfig IsNot Nothing Then
                    g_oSirConfig.Dispose()
                    g_oSirConfig = Nothing
                End If
                If g_oOrionLink IsNot Nothing Then
                    g_oOrionLink.Dispose()
                    g_oOrionLink = Nothing
                End If
                If g_oPremiumFinance IsNot Nothing Then
                    g_oPremiumFinance.Dispose()
                    g_oPremiumFinance = Nothing
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

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClientCode

                        m_sAccountCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameFinancePlanCnt

                        m_lPremiumFinanceCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePlanInsuranceFolderCnt

                        m_lPlanInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePlanIsSingleInstalment
                        m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' Name: GetSummary
    '
    ' Description: Used by Navigator. Passes summary information back
    '              for display on the interface.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vSummaryArray = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 2)

            ' Assign the key array with the parameter members.
            'TR - Add the Array of TransDetails (TransID, InsuranceRef, Amount)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFinancePlanTransactions


            If Convert.IsDBNull(m_vTransactionArray) Or IsNothing(m_vTransactionArray) Or Object.Equals(m_vTransactionArray, Nothing) Then

                m_vTransactionArray = 0
                If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_vTransactionArray

            'TR - Add the InsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFileCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinanceAmountNetPremium

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTypeOfBusiness) Then

                m_sTransactionType = CStr(vTypeOfBusiness)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                'developer guide no. 8
                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTypeOfBusiness:=vTypeOfBusiness, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' Get all of the lookup values as related to effective date
            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)
            ' Check for errors.

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' Name: GetID (Standard Method)
    '
    ' Description: Gets the AccountID from the PartyCnt
    '      DD040202 - enhancements for Agents
    '
    ' ***************************************************************** '
    Private Function GetAccountID(ByRef vPartyCnt As Object, ByRef vAccountID As Integer, ByRef r_bIsAgent As Boolean) As Integer

        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim sLedgerTypeCode As String = "" ' RAW 14/02/2003 : ISS2153 : added

        ' RAW 14/02/2003 : ISS2153 : removed redundant variables
        'DD040202 - added seperate return variables for GetAccountIDs
        'Dim lSalesAccountId As Long
        'Dim lPurchaseAccountId As Long
        'Dim lInsurerAccountId As Long
        'Dim lAgentAccountId As Long
        'Dim lFeeAccountId As Long
        'Dim lCommissionAccountId As Long
        'Dim lDiscountAccountId As Long
        'Dim lPremiumFinanceAccountId As Long
        'Dim lSubAGentAccountId As Long
        'Dim lNominalAccountId As Long
        'Dim lOtherPartyPayAccountId As Long
        'Dim lOtherPartyRecAccountId As Long



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the ID from the busines object.
        '    'AJM 12112001 - adding missing fields
        '    'DD 04022002 - returning into one variable does not work - only the first parameter will
        '    'return correctly. Changed returns to seperate variables and then combined.
        '       RAW 14/02/2003 : ISS2153 : replaced with call to GetAccountFromParty
        '    m_lReturn& = g_oOrionLink.GetAccountIDs( _
        ''        r_lSalesAccountID:=lSalesAccountId, _
        ''        r_lPurchaseAccountID:=lPurchaseAccountId, _
        ''        r_lInsurerAccountID:=lInsurerAccountId, _
        ''        r_lAgentAccountID:=lAgentAccountId, _
        ''        r_lFeeAccountID:=lFeeAccountId, _
        ''        r_lCommissionAccountID:=lCommissionAccountId, _
        ''        r_lDiscountAccountID:=lDiscountAccountId, _
        ''        r_lPremiumFinanceAccountID:=lPremiumFinanceAccountId, _
        ''        r_lSubAgentAccountID:=lSubAGentAccountId, _
        ''        r_lNominalAccountID:=lNominalAccountId, _
        ''        r_lOtherPartyPayAccountID:=lOtherPartyPayAccountId, _
        ''        r_lOtherPartyRecAccountID:=lOtherPartyRecAccountId, _
        ''        v_vPartyCnt:=m_lPartyCnt)



        m_lReturn = g_oOrionLink.GetAccountFromParty(v_lPartyCnt:=m_lPartyCnt, r_lAccountId:=lAccountID, r_sLedgerTypeCode:=sLedgerTypeCode)

        ' Check for errors
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Or m_lReturn = gPMConstants.PMEReturnCode.PMError Then
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the AccountID from the Party", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID")
        End If

        ' RAW 14/02/2003 : ISS2153 : changed following call to new function
        '    ' Return the first non-zero id
        '    m_lAccountId = lSalesAccountId + lPurchaseAccountId + lInsurerAccountId + _
        ''                    lAgentAccountId + lFeeAccountId + lCommissionAccountId + _
        ''                    lDiscountAccountId + lPremiumFinanceAccountId + lSubAGentAccountId + _
        ''                    lNominalAccountId + lOtherPartyPayAccountId + lOtherPartyRecAccountId

        m_lAccountId = lAccountID
        vAccountID = lAccountID

        r_bIsAgent = (sLedgerTypeCode = gSIRLibrary.g_ksAgentLedgerTypeCode)
        ' RAW 14/02/2003 : ISS2153 : end

        ' Return the value.

        Return m_lReturn

    End Function

    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    '
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'Valid source has been passed in via keys
        If m_iBranchID > 0 Then
            ReDim m_vSourceArray(3, 3)
            m_vSourceArray(1, 1) = m_iBranchID
            m_vSourceArray(2, 1) = ""
            m_vSourceArray(3, 1) = ""
            Return result
        End If
        'Call PMUser to get the Sources
        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_g_oPMUser As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Display error stating the problem.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If


        m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If
        ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            g_oPMUser.Dispose()
            g_oPMUser = Nothing
        End If

        Return result

    End Function
    'UPGRADE_NOTE: (7001) The following declaration (ValidSource) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidSource(ByVal vSource As Object) As Boolean
    'Dim result As Boolean = False
    'If Not Information.IsArray(m_vSourceArray) Then
    'Return True
    'End If
    'If (m_lDrillLevel) > 0 Then

    'If CInt(vSource) = DrillCompany Then
    'result = True
    'End If
    'Return result
    'End If
    'For 'i As Integer = 1 To m_vSourceArray.GetUpperBound(1)

    'If CInt(m_vSourceArray(1, i)) = CInt(vSource) Then
    'result = True
    'End If
    'Next i
    'Return result
    'End Function
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

        m_lReturn = CType(GetAccountID(vPartyCnt:=m_lPartyCnt, vAccountID:=m_lAccountId, r_bIsAgent:=m_bIsAgent), gPMConstants.PMEReturnCode)


        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

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
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New frmInterface()

        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}
            'developer guide no. 24
            .SourceArray = m_vSourceArray
            .AccountCode = m_sAccountCode
            .AccountID = m_lAccountId
            .IsAgent = m_bIsAgent
            .PremiumFinanceCnt = m_lPremiumFinanceCnt
            .PlanInsuranceFolderCnt = m_lPlanInsuranceFolderCnt
            .IsSingleInstalmentPlan = m_bIsSingleInstalmentPlan
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.

        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        m_lReturn = CType(m_frmInterface.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the status.
            result = gPMConstants.PMEReturnCode.PMFalse
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
        With m_frmInterface
            m_lStatus = .Status
            m_sStepStatus.Value = .StepStatus
            m_lInsuranceFileCnt = .InsuranceFileCnt


            m_vTransactionArray = .TransactionArray
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

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
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> 0 Then
                result = m_frmInterface.ErrorNumber
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
        ' Error Section.
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

End Class

