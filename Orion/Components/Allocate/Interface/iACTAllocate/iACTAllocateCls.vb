Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of SelectedItems (Private)

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
    'eck220800
    Private m_sInsuranceRef As String = ""
    Private m_vTransdetailIDs(,) As Object
    Private m_lAllocationTransType As Integer
    Private m_lAllocationID As Integer

    Private m_lBatchID As Integer
    Private m_lCashListTypeID As Integer
    Private m_vSourceArray As Object
    Private m_iBranchID As Integer

    Private m_bOutstandingOnly As Boolean

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Authority Level for Nav3
    Private m_lPMAuthorityLevel As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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

    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)
            g_iCompanyID = Value
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

    Public ReadOnly Property TransdetailID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lTransdetailID

        End Get
    End Property


    Public Property AccountID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lAccountID = Value

        End Set
    End Property


    Public Property DocumentRef() As String
        Get

            ' Return the objects parameter value.
            Return m_sDocumentRef

        End Get
        Set(ByVal Value As String)

            ' Set the object parameter value.
            m_sDocumentRef = Value

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


    Public ReadOnly Property TransdetailIDs() As Object
        Get

            Return VB6.CopyArray(m_vTransdetailIDs)

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

    Public WriteOnly Property OutstandingOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bOutstandingOnly = Value
        End Set
    End Property
    'eck090500
    Public ReadOnly Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return VB6.CopyArray(m_vSourceArray)

        End Get
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    '' FRIENDly Property Procedures (Begin)
    '
    '
    '' FRIENDly Property Procedures (End)


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
    Public Function Initialise() As Integer




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
            End With

            g_iCompanyID = g_iSourceID 'CurrentCompany()

            ' Get an instance of the business object via
            ' the public object manager.
            sClassName = "bACTAllocate.Business"
            m_lReturn = g_oObjectManager.GetInstance(oObject:=g_oBusiness, sClassName:=sClassName, vInstanceManager:="ClientManager")



            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

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


            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            'Developer guide No.39
            'If sHelpFile <> "" Then
            '	App.HelpFile = sHelpFile
            'End If


            m_lReturn = g_oBusiness.GetUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive User Authorities", Application.ProductName)
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
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAllocationId

                        m_lAllocationID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAllocationTransType

                        m_lAllocationTransType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameBatchID

                        m_lBatchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListTypeId

                        m_lCashListTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameBranchID

                        m_iBranchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

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
            ReDim vKeyArray(1, 2)

            ' Assign the key array with the parameter members.mmm

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameTransDetailID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lTransdetailID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lAccountID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "batch_set_id"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lBatchID

            If m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation Then
                ReDim Preserve vKeyArray(1, 3)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNamePrimaryTransDetailID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lTransdetailID
            End If

            ' {* USER DEFINED CODE (End) *}

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

            m_frmInterface = New frmInterface()
            ' Assign the parameters to the interface properties.
            With m_frmInterface
                .CallingAppName = m_sCallingAppName
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TypeOfBusiness = m_sTypeOfBusiness
                .EffectiveDate = m_dtEffectiveDate

                ' {* USER DEFINED CODE (Begin) *}

                .OutstandingOnly = m_bOutstandingOnly
                'Developer Guide no.24
                .SourceArray = m_vSourceArray

                .ControllingInterface = Me
                .DocumentRef = m_sDocumentRef
                .AllocationTransType = m_lAllocationTransType
                .AllocationID = m_lAllocationID
                .InsuranceRef = m_sInsuranceRef
                '
                .AccountID = m_lAccountID

                .IsBatch = False
                ' {* USER DEFINED CODE (End) *}

            End With



            ' Check for any errors.
            If m_frmInterface.ErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return the error.
                result = m_frmInterface.ErrorNumber
                m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)
                Return result
            End If



            m_frmInterface.BringToFront()

            ' Display the interface.
            m_frmInterface.ShowDialog()

            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return the error.
                result = m_frmInterface.ErrorNumber
            End If

            m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

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
                m_lTransdetailID = .TransdetailID
                m_sDocumentRef = .DocumentRef

                m_vTransdetailIDs = .TransdetailIDs

                ' {* USER DEFINED CODE (End) *}
            End With

            ' Set up the batch with the TransDetailIDs

            'Tomo210199
            'But not if we're not running in a batch
            If m_lBatchID = 0 Then
                Return result
            End If

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
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
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
        Dim temp_g_oPMUser As Object
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

    ' PRIVATE Methods (End)


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

