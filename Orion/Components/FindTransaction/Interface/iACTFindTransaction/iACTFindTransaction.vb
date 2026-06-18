Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 06 May 1997
    '
    ' Description: Main public class to accompany the interface form
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Collection of SelectedItems (Private)
    Private m_oSelectedItems As iACTFindTransaction.SelectedItems

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTypeOfBusiness As String = ""
    Private m_dtEffectiveDate As Date
    Private m_frmInterface As frmInterface

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lTransdetailID As Integer
    Private m_lAccountID As Integer
    Private m_sDocumentRef As String = ""
    Private m_lDocumentId As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lDrillLevel As Integer
    Private m_iDrillCompany As Integer
    Private m_vTransdetailIDs As Object
    Private m_lAllocationTransType As Integer
    Private m_lAllocationID As Integer

    Private m_lBatchID As Integer
    Private m_lCashListTypeID As Integer
    Private m_vSourceArray As Object
    Private m_iBranchID As Integer
    Private m_bOutstandingOnly As Boolean
    Private m_lCashListId As Integer
    ' AMB 24/02/2003: PS220 - added action key
    Private m_sActionKey As String = ""
    '27/05/2003 - PWC - 186 - Debt Roll-up
    Private m_bRollup As Boolean
    Private m_vSearchParams As Object
    'DD 4/10/2004: Used for excluding during Cash List Allocation
    Private m_lExcludeTransDetailID As Integer

    ' Stores the return value for a function call.
    Private m_lReturn As Integer

    ' Authority Level for Nav3
    Private m_lPMAuthorityLevel As Integer

    ' CJB 01/04/2004 New flag to denote if a refresh is required (for use when drilling)
    Private m_boDataChanged As Boolean

    'for reverse and replace transaction process
    Private m_bCalledViaClientManager As Boolean

    Private m_iSelectedSourceId As Integer
    Private m_iSelectedCurrencyId As Integer
    Private m_lInsuredAccountID As Integer
    Private m_bInsuredAccountView As Boolean
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    Private Const kSystemOptionEnhancedCaseSearching = 5099
    Private m_bEnhancedCaseSearching As Boolean
    ''' <summary>
    ''' Variable Declared to store user Party Information
    ''' </summary>
    Private m_vUserPartyArray as Object
    ' CJB 01/04/2004 New flag to denote if a refresh is required
    Public Property DataChanged() As Boolean
        Get
            ' Return the objects parameter value.
            Return m_boDataChanged
        End Get
        Set(ByVal Value As Boolean)
            ' Set the object parameter value.
            m_boDataChanged = Value
        End Set
    End Property

    Public Property ActionKey() As String
        Get
            ' AMB 24/02/2003: PS220 - added actionkey property
            Return m_sActionKey
        End Get
        Set(ByVal Value As String)
            ' AMB 24/02/2003: PS220 - added actionkey property
            m_sActionKey = Value
        End Set
    End Property

    '27/05/2003 - PWC - 186 - Debt Roll-up
    Public Property Rollup() As Boolean
        Get
            Return m_bRollup
        End Get
        Set(ByVal Value As Boolean)
            m_bRollup = Value
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
    Public ReadOnly Property TypeOfBusiness() As String
        Get
            ' Standard Property.
            ' Return the type of business.
            Return m_sTypeOfBusiness
        End Get
    End Property
    Public ReadOnly Property EffectiveDate() As Date
        Get
            ' Standard Property.
            ' Return the effective date.
            Return m_dtEffectiveDate
        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)
            g_iCompanyID = Value
        End Set
    End Property
    Public ReadOnly Property TransDetailId() As Integer
        Get
            Return m_lTransdetailID
        End Get
    End Property
    Public Property AccountID() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property
    Public Property DocumentRef() As String
        Get
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            m_sDocumentRef = Value
        End Set
    End Property
    Public Property DocumentId() As Integer
        Get
            Return m_lDocumentId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentId = Value
        End Set
    End Property
    'eck220800
    Public Property InsuranceRef() As String
        Get
            ' Return the objects parameter value.
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            ' Set the object parameter value.
            m_sInsuranceRef = Value
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

    Public Property SelectedItems() As iACTFindTransaction.SelectedItems
        Get

            Return m_oSelectedItems

        End Get
        Set(ByVal Value As iACTFindTransaction.SelectedItems)

            m_oSelectedItems = Value

        End Set
    End Property

    Public ReadOnly Property TransDetailIDs() As Object
        Get

            Return m_vTransdetailIDs

        End Get
    End Property

    Public Property AllocationTransType() As Integer
        Get

            Return m_lAllocationTransType

        End Get
        Set(ByVal Value As Integer)

            m_lAllocationTransType = Value

        End Set
    End Property

    Public Property AllocationID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAllocationID

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lAllocationID = Value

        End Set
    End Property

    Public Property CashListTypeID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lCashListTypeID

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lCashListTypeID = Value

        End Set
    End Property

    'DC140403 -ISS3503 -Start

    Public Property CashListID() As Integer
        Get
            ' Return the objects parameter value.
            Return m_lCashListId
        End Get
        Set(ByVal Value As Integer)
            ' Set the object parameter value.
            m_lCashListId = Value
        End Set
    End Property
    'DC140403 -ISS3503 -End

    Public WriteOnly Property OutstandingOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bOutstandingOnly = Value
        End Set
    End Property
    'eck090500
    Public ReadOnly Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return m_vSourceArray

        End Get
    End Property

    Public WriteOnly Property ExcludeTransDetailID() As Integer
        Set(ByVal Value As Integer)
            m_lExcludeTransDetailID = Value
        End Set
    End Property

    Public Property CalledViaClientManager() As Boolean
        Get
            Return m_bCalledViaClientManager
        End Get
        Set(ByVal Value As Boolean)
            m_bCalledViaClientManager = Value
        End Set
    End Property

    Public WriteOnly Property SelectedSourceId() As Integer
        Set(ByVal Value As Integer)
            m_iSelectedSourceId = Value
        End Set
    End Property

    Public WriteOnly Property SelectedCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_iSelectedCurrencyId = Value
        End Set
    End Property

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.1.1)

    Public Property InsuredAccountID() As Integer
        Get
            Return m_lInsuredAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lInsuredAccountID = Value
        End Set
    End Property

    Public Property InsuredAccountView() As Boolean
        Get
            Return m_bInsuredAccountView
        End Get
        Set(ByVal Value As Boolean)
            m_bInsuredAccountView = Value
        End Set
    End Property
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.3.1.1)
    ''' <summary>
    ''' Variable declared to Store Data related to User Party
    ''' </summary>
    ''' <returns>returns User party Array</returns>
    Public Property UserPartyArray() As Object
        Get
            Return m_vUserPartyArray
        End Get
        Set(value As Object)
            m_vUserPartyArray = value
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
        Dim sMessage, sTitle, sClassName, sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the object parameters to default values
            m_lTransdetailID = 0
            m_lAccountID = 0

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

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Abort application
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID etc from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername.Value = .UserName
                g_sPassword.Value = .Password
                g_iUserID = .UserID
                g_iCurrencyID = .CurrencyID
                g_iLogLevel = .LogLevel
                'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.3)
                g_sUserConfigXMLDataset = .UserConfigXMLDataSet
                'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.2.3)
            End With

            'BB Set Orion Company ID
            g_iCompanyID = g_iSourceID 'CurrentCompany()

            ' Get an instance of the business object via
            ' the public object manager.
            sClassName = "bACTFindTransaction.Business"
            m_lReturn = g_oObjectManager.GetInstance(oObject:=g_oBusiness, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetViaClientManager)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & sClassName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Get an instance of the Find Payment Maintenance
            ' the public object manager.
            Dim temp_m_oFindPaymentMaintenance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindPaymentMaintenance, "bACTPaymentMaintenance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oFindPaymentMaintenance = temp_m_oFindPaymentMaintenance

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & sClassName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Get an instance of the Currency Convertor
            ' the public object manager.
            Dim temp_m_obACTCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_obACTCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_obACTCurrencyConvert = temp_m_obACTCurrencyConvert

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & sClassName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Initialise the process modes with default values
            m_lReturn = CType(SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTypeOfBusiness:=gPMConstants.PMTransactionTypeGeneric, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to set default process modes
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oSelectedItems = New iACTFindTransaction.SelectedItems()

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            If sHelpFile <> "" Then

                'App.HelpFile = sHelpFile
            End If

            iPMBListEvents.g_oObjectManager = g_oObjectManager

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
                If m_oFindPaymentMaintenance IsNot Nothing Then
                    m_oFindPaymentMaintenance.Dispose()
                    m_oFindPaymentMaintenance = Nothing
                End If
                If m_obACTCurrencyConvert IsNot Nothing Then
                    m_obACTCurrencyConvert.Dispose()

                End If
                m_obACTCurrencyConvert = Nothing
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
                'eck040601 replace cInt with cLng

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAllocationId

                        m_lAllocationID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAllocationTransType

                        m_lAllocationTransType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameBatchID

                        m_lBatchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck170700
                    Case PMNavKeyConst.ACTKeyNameCashListTypeId

                        m_lCashListTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck090500
                    Case PMNavKeyConst.ACTKeyNameBranchID

                        m_iBranchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' AMB 24/02/2003: PS220 - added action key for Approve/Reject write-off
                    Case PMNavKeyConst.ACTKeyNameActionKey

                        m_sActionKey = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()

                    Case PMNavKeyConst.PMKeyNameFindTransRollUp

                        m_bRollup = Conversion.Val(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                    Case PMNavKeyConst.PMKeyNameFindTransSearchParams

                        m_vSearchParams = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                        'DC140403 -ISS3503
                    Case PMNavKeyConst.ACTKeyNameCashListId

                        m_lCashListId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

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

    ' ***************************************************************** '
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

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 3)

            ' Assign the key array with the parameter members.mmm

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameTransDetailID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lTransdetailID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameAccountID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lAccountID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "batch_set_id"
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lBatchID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameTransDetailIDs
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_vTransdetailIDs
            If m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation Then
                ReDim Preserve vKeyArray(1, 4)
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNamePrimaryTransDetailID
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lTransdetailID
            End If
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

                m_iTask = CInt(vTask)
            End If

            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If

            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Information.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness = CStr(vTypeOfBusiness)
            End If

            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

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
            m_lReturn = GetLookupValues()

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
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim sRollupOption As String = ""
        Const kiRollUpOption As Integer = 1031

        Dim sValue As String = ""
        Const kMethodName As String = "Start"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetValidSources()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetUserAgentDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CheckSecurity(r_bReverseTransactionsAuthority:=g_bReverseTransactionsAuthority, r_bReverseAllocationsAuthority:=g_bReverseAllocationsAuthority, r_bPerformAllocationsAuthority:=g_bPerformAllocationsAuthority, r_bReverseAndReplaceTransactionsAuthority:=g_bReverseReplaceTransactionsAuthority)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Test to see if roll ups are enabled
            If iPMFunc.RetrieveSingleSystemOption(v_iOptionNumber:=kiRollUpOption, r_sOptionValue:=sRollupOption) <> gPMConstants.PMEReturnCode.PMTrue Then

            End If

            'If rollups not enabled then claer
            If sRollupOption <> "1" Then
                m_bRollup = False
            End If

            ' Get System Option for Disable Wildcard Search
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bDisableWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnhancedCaseSearching
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionEnhancedCaseSearching, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetSystemOption for EnhancedCaseSearching Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bEnhancedCaseSearching = (sValue = "1")


            m_frmInterface = New frmInterface()

            ' Assign the parameters to the interface properties.
            With m_frmInterface
                .Rollup = m_bRollup
                .CallingAppName = m_sCallingAppName
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TypeOfBusiness = m_sTypeOfBusiness
                .EffectiveDate = m_dtEffectiveDate

                ' {* USER DEFINED CODE (Begin) *}
                .CalledViaClientManager = m_bCalledViaClientManager
                .OutstandingOnly = m_bOutstandingOnly
                .SourceArray = m_vSourceArray
                .ControllingInterface = Me
                .DocumentRef = m_sDocumentRef
                .DrillLevel = m_lDrillLevel
                .DrillCompany = m_iDrillCompany
                .AllocationTransType = m_lAllocationTransType
                .AllocationID = m_lAllocationID
                .CashListTypeID = m_lCashListTypeID
                .InsuranceRef = m_sInsuranceRef
                .SelectedCurrencyId = m_iSelectedCurrencyId
                .SelectedSourceId = m_iSelectedSourceId
                'Issue 4919 PSL 30/06/2003 Moved Acccount id from here
                .IsBatch = Not (m_lAccountID = 0)
                .CashListID = m_lCashListId

                'Debt Roll-up
                .SearchParams = m_vSearchParams
                .DocumentId = m_lDocumentId 'DD 21/08/2003: Added Document ID for drill downs
                .ExcludeTransDetailID = m_lExcludeTransDetailID

                'Issue 4919 PSL 30/06/2003 Moved Acccount id to here
                'Because this kicks of the form so any properties set after this
                'would be too late

                .InsuredAccountID = m_lInsuredAccountID
                .InsuredAccountView = m_bInsuredAccountView
                .AccountID = m_lAccountID
                ' {* USER DEFINED CODE (End) *}
                .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
                .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
                .EnhancedCaseSearching = m_bEnhancedCaseSearching
                .UserPartyArray = m_vUserPartyArray
            End With

            'Load the instance of the interface into memory.(To match functionality with VB)
            m_frmInterface.frmInterfaceLoad()

            If m_frmInterface.Name = "frmInterface" Then
                'Populate search params if any passed
                If Information.IsArray(m_vSearchParams) Then
                    m_lReturn = m_frmInterface.handleSearchParams()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                    m_frmInterface.FindNow()
                End If
            End If
            '?Form will already be loaded here!!?

            ' Check for any errors.
            If m_frmInterface.ErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return the error.
                result = m_frmInterface.ErrorNumber
                m_lReturn = UnLoadInterface()
                Return result
            End If

            ' CF070898 - Fixed overlap of forms.
            ' We'll use a loop here for each time its been drilled. Otherwise,
            ' it just gets moved once from the original parents position.
            ' Therefore forms 2 and 3 will appear on top of each other.
            If m_lDrillLevel > 0 Then
                For iLoop1 As Integer = 1 To m_lDrillLevel
                    m_lReturn = iACTFunc.SetChildFormPosition(m_frmInterface, m_frmInterface)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                Next iLoop1
            End If

            m_frmInterface.BringToFront()

            ' Display the interface.
            m_frmInterface.ShowDialog()

            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return the error.
                result = m_frmInterface.ErrorNumber
            End If

            m_lReturn = UnLoadInterface()

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
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Allows a controlling application to
    '               unload this instance of the interface.
    '
    ' ***************************************************************** '
    Public Function UnLoadInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' must return the status to the calling component
            m_lStatus = m_frmInterface.Status

            ' Unload and destroy the instance of the interface
            ' from memory.
            m_frmInterface.Close()
            m_frmInterface = Nothing

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload the interface from memory", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OnForm_UnLoad
    '
    ' Description: Called by the form unload event
    '
    ' ***************************************************************** '
    Public Function OnForm_Unload() As Integer

        Dim result As Integer = 0
        Dim oNavBatch As bPMNavBatch.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the property members from the interface parameters.
            With m_frmInterface
                m_lStatus = .Status

                ' {* USER DEFINED CODE (Begin) *}

                m_lAccountID = .AccountID
                m_lTransdetailID = .TransDetailId
                m_sDocumentRef = .DocumentRef

                m_vTransdetailIDs = .TransDetailIDs
                ' {* USER DEFINED CODE (End) *}
            End With

            ' Set up the batch with the TransDetailIDs

            'Tomo210199
            'But not if we're not running in a batch
            If m_lBatchID = 0 Then
                Return result
            End If
            'MKR 01/11/2004 PN 14833 Exiting the function if nothing to add in Batch
            If Not Information.IsArray(m_vTransdetailIDs) Then
                Return result
            End If
            'MKR 01/11/2004 PN 14833 --End
            Dim temp_oNavBatch As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oNavBatch, "bPMNavBatch.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oNavBatch = temp_oNavBatch
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the batch id
            oNavBatch.BatchSetID = m_lBatchID

            ' Add the batch data

            m_lReturn = oNavBatch.AddBatchRecord(v_vBatchArray:=m_vTransdetailIDs, v_sNavBatchCode:=gACTLibrary.ACTNavBatchFindTransToAllocation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate the object
            oNavBatch.Dispose()
            

            oNavBatch = Nothing

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload the interface from memory", vApp:=ACApp, vClass:=ACClass, vMethod:="OnForm_Unload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'eck090500
    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    '
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer
        Dim result As Integer = 0
        Dim oPMUser As bPMUser.Business

        Dim oSource As bPMSource.Business
        Dim sCode, sDesc As String



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get instance of bPMUser.Business
        Dim temp_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPMUser = temp_oPMUser

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get instance of bPMUser.Business")
        End If

        'Get instance of bPMSource.Business
        Dim temp_oSource As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oSource, "bPMSource.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oSource = temp_oSource

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get instance of bPMSource.Business")
        End If

        If m_iBranchID > 0 Then
            'Valid source has been passed in via keys

            m_lReturn = oSource.GetDetails(vSourceID:=m_iBranchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get source details")
            End If

            m_lReturn = oSource.GetNext(vCode:=sCode, vDescription:=sDesc)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get next source details")
            End If

            ReDim m_vSourceArray(2, 0)

            m_vSourceArray(1, 1) = m_iBranchID

            m_vSourceArray(2, 1) = sCode

            m_vSourceArray(3, 1) = sDesc
        Else
            'Get all sources for user (even closed ones)

            m_lReturn = oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get user sources")
            End If
        End If

        'Terminate and destroy the business objects
        If Not (oPMUser Is Nothing) Then

            oPMUser.Dispose()
            oPMUser = Nothing
        End If
        If Not (oSource Is Nothing) Then

            oSource.Dispose()
            oSource = Nothing
        End If

        Return result

    End Function
    
    ''' <summary>
    ''' GetUserAgentDetails Method added to fetch user party 
    ''' information from Database and set it to m_vUserPartyArray object
    ''' </summary>
    ''' <returns>returns nResult</returns>
    Private Function GetUserAgentDetails() As Integer
        Dim nResult As Integer = 0
        Dim oPMUser As bPMUser.Business
        Dim r_vUserPartyInfo As object

        nResult = gPMConstants.PMEReturnCode.PMTrue

        oPMUser = New bPMUser.Business()

        nResult = oPMUser.Initialise(sUsername:=g_sUsername.Value,sPassword:=g_sPassword.Value,iUserID:=g_iUserID,iSourceID:=g_iSourceID,iLanguageID:=g_iLanguageID,iCurrencyID:=g_iCurrencyID,iLogLevel:=g_iLogLevel,sCallingAppName:=g_sCallingAppName)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to initialise bPMUser.Business")
        End If

        nResult = oPMUser.GetUserPartyInfo(r_lUserId:=g_iUserID,r_vUserPartyInfo:=r_vUserPartyInfo)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get user sources")
        Else
            m_vUserPartyArray = r_vUserPartyInfo
        End If

        'Terminate and destroy the business objects
        If Not (oPMUser Is Nothing) Then

            oPMUser.Dispose()
            oPMUser = Nothing
        End If

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: CheckSecurity (Standard Method)
    '
    ' Description: Check whether the user has authority to view clients
    ' History:     2005 Client Security  20/04/2005
    '
    ' ***************************************************************** '
    Private Function CheckSecurity(ByRef r_bReverseTransactionsAuthority As Boolean, ByRef r_bReverseAllocationsAuthority As Boolean, ByRef r_bPerformAllocationsAuthority As Boolean, ByRef r_bReverseAndReplaceTransactionsAuthority As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckSecurity"
        Const kCanReverseReplaceTransactionArrPos As Integer = 5
        Const kParamCounts As Integer = 5

        Dim oUserAuthorities As Object
        Dim sAgencyOrUnderwriting, sValue As String
        Dim iIsReverseTransactions, iIsReverseAllocations, iIsPerformAllocations, iCanReverseAndReplaceTransactions As Integer
        Dim vParams As Object

        ReDim vParams(kParamCounts)


        result = gPMConstants.PMEReturnCode.PMTrue

        r_bReverseTransactionsAuthority = False
        r_bReverseAllocationsAuthority = False
        r_bPerformAllocationsAuthority = False
        r_bReverseAndReplaceTransactionsAuthority = False



        Return result
    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        Try

            'Default to true
            m_bRollup = True

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class