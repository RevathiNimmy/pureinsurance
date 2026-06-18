Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 05/05/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.

    Dim objfrmInterface As New frmInterface
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_sStepStatus As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lPartyCnt As Integer
    Private m_sRiskGroup As String = ""
    Private m_lRiskGroupID As Integer
    Private m_vExistingFees As Object
    Private m_sAccountType As String = ""
    Private m_dFeePercentage As Double
    Private m_cFeeAmount As Decimal
    'EK 14/09/99 New data for Extras
    Private m_dFeeCommissionPercentage As Double
    Private m_cFeeCommissionAmount As Decimal
    'PSA 22/06/00
    Private m_iDisplayOnQuotes As Integer
    Private m_cPremium As Decimal
    ' CJB 270802 - TransactionTypeID
    Private m_lTransactionTypeID As Integer
    'DC140303 -ISS3018 -bring into line from 1.6.9
    ' CTAF 280502 - TaxRatesID
    Private m_lTaxRatesID As Integer

    'Datasure
    Private m_lTaxGroupId As Integer
    Private m_lCommissionTaxGroupId As Integer

    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Private m_lExtraSchemeID As Integer
    Private m_sExtraSchemeDesc As String = ""

    Private m_lCurrencyID As Integer
    Private m_sCurrencyName As String = ""

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_iFsaTypeOfSaleId As Integer

    Private m_bHideScheme As Boolean
    Public Property HideScheme() As Boolean
        Get
            Return m_bHideScheme
        End Get
        Set(ByVal Value As Boolean)
            m_bHideScheme = Value
        End Set
    End Property

    'DC140303 -ISS3018 -bring into line from 1.6.9
    ' CTAF 280502 - TaxRatesID
    Public Property TaxRatesID() As Integer
        Get
            Return m_lTaxRatesID
        End Get
        Set(ByVal Value As Integer)
            m_lTaxRatesID = Value
        End Set
    End Property
    'New for Datasure
    Public Property TaxGroupID() As Integer
        Get
            Return m_lTaxGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lTaxGroupId = Value
        End Set
    End Property
    Public Property CommissionTaxGroupID() As Integer
        Get
            Return m_lCommissionTaxGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionTaxGroupId = Value
        End Set
    End Property
    'Datasure End
    Public WriteOnly Property ExistingFees() As Object
        Set(ByVal Value As Object)


            m_vExistingFees = Value
        End Set
    End Property
    Public Property AccountType() As String
        Get
            Return m_sAccountType
        End Get
        Set(ByVal Value As String)
            m_sAccountType = Value
        End Set
    End Property

    Public Property Premium() As Decimal
        Get
            Return m_cPremium
        End Get
        Set(ByVal Value As Decimal)
            m_cPremium = Value
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


    'Added 991006 MK
    Public Property RiskGroupID() As Integer
        Get
            Return m_lRiskGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskGroupID = Value
        End Set
    End Property

    'Added 991004 MK
    Public Property RiskGroup() As String
        Get
            Return m_sRiskGroup
        End Get
        Set(ByVal Value As String)
            m_sRiskGroup = Value
        End Set
    End Property

    Public Property FeePercentage() As Double
        Get
            Return m_dFeePercentage
        End Get
        Set(ByVal Value As Double)
            m_dFeePercentage = Value
        End Set
    End Property

    Public Property FeeAmount() As Decimal
        Get
            Return m_cFeeAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cFeeAmount = Value
        End Set
    End Property
    'EK 14/09/99 New Properties for Extras
    Public Property FeeCommissionPercentage() As Double
        Get
            Return m_dFeeCommissionPercentage
        End Get
        Set(ByVal Value As Double)
            m_dFeeCommissionPercentage = Value
        End Set
    End Property

    Public Property FeeCommissionAmount() As Decimal
        Get
            Return m_cFeeCommissionAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cFeeCommissionAmount = Value
        End Set
    End Property

    'EK 14/09/99 End
    'PSA 23/06/00
    'PSA 23/06/00
    Public Property DisplayOnQuotes() As Integer
        Get
            Return m_iDisplayOnQuotes
        End Get
        Set(ByVal Value As Integer)
            m_iDisplayOnQuotes = Value
        End Set
    End Property
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Public Property ExtraSchemeID() As Integer
        Get
            Return m_lExtraSchemeID
        End Get
        Set(ByVal Value As Integer)
            m_lExtraSchemeID = Value
        End Set
    End Property
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Public Property ExtraSchemeDesc() As String
        Get
            Return m_sExtraSchemeDesc
        End Get
        Set(ByVal Value As String)
            m_sExtraSchemeDesc = Value
        End Set
    End Property
    ' CJB 270802 - TransactionTypeID
    Public Property TransactionTypeID() As Integer
        Get
            Return m_lTransactionTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionTypeID = Value
        End Set
    End Property
    ' CJB 270802 - TransactionTypeID

    Public Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property

    Public Property CurrencyName() As String
        Get
            Return m_sCurrencyName
        End Get
        Set(ByVal Value As String)
            m_sCurrencyName = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property


    Public Property FSATypeOfSaleId() As Integer
        Get
            Return m_iFsaTypeOfSaleId
        End Get
        Set(ByVal Value As Integer)
            m_iFsaTypeOfSaleId = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
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
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMerror

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
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


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                End Select
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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


            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vSummaryArray(1, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameNavigatorTitle1

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sNavigatorTitle

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
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

        ' Assign the parameters to the interface properties.

        With objfrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .AccountType = m_sAccountType
            .RiskGroup = m_sRiskGroup
            .RiskGroupID = m_lRiskGroupID
            .FeeAmount = m_cFeeAmount
            .FeePercentage = m_dFeePercentage
            .FeeCommissionAmount = m_cFeeCommissionAmount
            .FeeCommissionPercentage = m_dFeeCommissionPercentage
            .TransactionTypeID = m_lTransactionTypeID
            'Datasure
            '.TaxRatesID = m_lTaxRatesID
            .TaxGroupID = m_lTaxGroupId
            .CommissionTaxGroupID = m_lCommissionTaxGroupId
            'DatasureEnd
            .ExtraSchemeID = m_lExtraSchemeID
            .ExtraSchemeDesc = m_sExtraSchemeDesc
            .DisplayOnQuotes = m_iDisplayOnQuotes
            .CurrencyID = m_lCurrencyID
            .FSATypeOfSaleId = m_iFsaTypeOfSaleId
            .HideScheme = m_bHideScheme
        End With

        ' Load the instance of the interface into memory.

        Dim tempLoadForm As frmInterface = objfrmInterface

        ' Check if we have had an error so far.

        If objfrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.

            result = objfrmInterface.ErrorNumber
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

        With objfrmInterface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus

            'Added 991008 MK
            If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                m_sAccountType = .AccountType
                m_lRiskGroupID = VB6.GetItemData(.cboRiskGroup, .cboRiskGroup.SelectedIndex)
                m_sRiskGroup = .cboRiskGroup.Text
                m_dFeePercentage = .FeePercentage
                m_cFeeAmount = .FeeAmount
                m_dFeeCommissionPercentage = .FeeCommissionPercentage
                m_cFeeCommissionAmount = .FeeCommissionAmount
                m_iDisplayOnQuotes = .DisplayOnQuotes
                m_lTransactionTypeID = .TransactionTypeID
                'Datasure
                'm_lTaxRatesID = .TaxRatesID
                m_lTaxGroupId = .TaxGroupID
                m_lCommissionTaxGroupId = .CommissionTaxGroupID

                m_lExtraSchemeID = .ExtraSchemeID
                m_sExtraSchemeDesc = .ExtraSchemeDesc
                m_lCurrencyID = .CurrencyID
                m_sCurrencyName = .CurrencyName
                m_iFsaTypeOfSaleId = .FSATypeOfSaleId
            End If

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.

        objfrmInterface.Close()

        objfrmInterface = Nothing

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

        VB6.ShowForm(objfrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.

            If objfrmInterface.ErrorNumber <> 0 Then

                result = objfrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

