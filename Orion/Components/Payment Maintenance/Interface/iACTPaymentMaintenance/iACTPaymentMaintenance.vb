Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    Private Const ACClass As String = "Interface"

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    'Developer Guide 50
    Dim frmInterface As frmInterface

    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    Private m_lCashListID As Integer
    Private m_lCashListTypeID As Integer
    Private m_iCashListCompanyID As Integer
    'developer guide no. 101
    Private m_vSourceArray As Object

    Private m_lReturn As gPMConstants.PMEReturnCode

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

    Public ReadOnly Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return VB6.CopyArray(m_vSourceArray)

        End Get
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle, sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Const kMethodName As String = "Initialise"

        Try


            result = gPMConstants.PMEReturnCode.pmtrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initialise the object manager")
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserID = .UserID
                g_sUsername.Value = .UserName
            End With

            'BB Set Orion Company ID
            g_iCompanyID = g_iSourceID

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to retrive Helpfile")
                Return result
            End If
            If sHelpFile <> "" Then
                'archana todolist
                'App.HelpFile = sHelpFile
            End If

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            Dim temp_g_oACTCashListBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oACTCashListBusiness, "bACTFindCashListItem.Form", vInstanceManager:="ClientManager")
            g_oACTCashListBusiness = temp_g_oACTCashListBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initialise bACTFindCashListItem.Form")
                Return result
            End If

            Dim temp_g_obACTFindTransaction As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_obACTFindTransaction, "bACTFindTransaction.Business", vInstanceManager:="ClientManager")
            g_obACTFindTransaction = temp_g_obACTFindTransaction

            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initialise bACTFindTransaction.Business")
                Return result
            End If

            Dim temp_g_oACTDocumentReversal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oACTDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:="ClientManager")
            g_oACTDocumentReversal = temp_g_oACTDocumentReversal

            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initilise bACTDocumentReversal.Business")
                Return result
            End If

            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bACTPaymentMaintenance.Form", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initilise bACTPaymentMaintenance.Form")
                Return result
            End If

            Dim temp_g_obACTCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_obACTCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:="ClientManager")
            g_obACTCurrencyConvert = temp_g_obACTCurrencyConvert

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to initilise bACTCurrencyConvert.Form")
                Return result
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

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
                If g_oACTCashListBusiness IsNot Nothing Then
                    g_oACTCashListBusiness.Dispose()
                    g_oACTCashListBusiness = Nothing
                End If
                If g_oACTDocumentReversal IsNot Nothing Then
                    g_oACTDocumentReversal.Dispose()
                    g_oACTDocumentReversal = Nothing
                End If
                If g_obACTFindTransaction IsNot Nothing Then
                    g_obACTFindTransaction.Dispose()
                    g_obACTFindTransaction = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If g_obACTCurrencyConvert IsNot Nothing Then
                    g_obACTCurrencyConvert.Dispose()

                End If
                g_obACTCurrencyConvert = Nothing
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
        Const kMethodName As String = "SetKeys"

        Try


            result = gPMConstants.PMEReturnCode.pmtrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="SetKeys method Failed")
                Return result
            End If

            With frmInterface
                If Information.IsArray(vKeyArray) Then
                    .IsNavigatorProcess = True
                End If
                ' Step through the key array.
                For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                    Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                        Case "BranchId"

                            .BranchId = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "MaxRowToFetch"

                            .MaxRowToFetch = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "BankAccountId"

                            .BankAccountId = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "UserId"

                            .UserId = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "ClientCode"

                            .ClientCode = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "ClientAccNumber"

                            .ClientAccNumber = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "PayeeName"

                            .PayeeName = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "PolicyClaimNumber"

                            .PolicyClaimNumber = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "PaymentTypeId"

                            .PaymentTypeId = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "PaymentMediaTypeId"

                            .PaymentMediaTypeId = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "MediaFrom"

                            .MediaFrom = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                        Case "MediaTo"

                            .MediaTo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                        Case "AmoutFrom"

                            .AmoutFrom = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                        Case "AmountTo"

                            .AmountTo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                        Case "DateFrom"

                            .DateFrom = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                        Case "DateTo"

                            .DateTo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                        Case "ShowOnlyOutstanding"

                            .ShowOnlyOutstanding = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "PaymentStatusId"

                            .PaymentStatus = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case "BatchReference"

                            .BatchReference = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()
                    End Select
                Next lRow
            End With


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
        Const kMethodName As String = "GetKeys"
        Dim lRow As Integer

        Try


            result = gPMConstants.PMEReturnCode.pmtrue

            ' {* USER DEFINED CODE (Begin) *}


            '    ReDim vKeyArray(0 To 1, 0 To 2)
            '
            '    vKeyArray(PMKeyName, 0) = ACTKeyNameCashListId
            '    vKeyArray(PMKeyValue, 0) = m_lCashListID&
            '
            '    vKeyArray(PMKeyName, 1) = ACTKeyNameCashListTypeId
            '    vKeyArray(PMKeyValue, 1) = m_lCashListTypeID&
            '
            '    vKeyArray(PMKeyName, 2) = ACTKeyNameBranchID
            '    vKeyArray(PMKeyValue, 2) = m_iCashListCompanyID


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSummary"
        Dim lRow As Integer

        Try


            result = gPMConstants.PMEReturnCode.pmtrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            vSummaryArray = ""
            ' {* USER DEFINED CODE (End) *}


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"
        Try



            result = gPMConstants.PMEReturnCode.pmtrue

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
            If g_oBusiness Is Nothing Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                    'Raise Error.
                    gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to call SetProcessModes method")
                    Return result
                End If
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)
        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
        Const kMethodName As String = "SetStatus"
        Try


            result = gPMConstants.PMEReturnCode.pmtrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Start"
        Try


            result = gPMConstants.PMEReturnCode.pmtrue
            'eck090500
            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck090500

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetID (Standard Method)
    '
    ' Description: Gets the ID for the search parameter from the
    '              business object.
    '
    ' ***************************************************************** '
    Public Function GetID(ByRef vSearch As Object, ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetID"
        Dim lId As Integer

        Try


            result = gPMConstants.PMEReturnCode.pmtrue

            ' Get the ID from the busines object.

            'archana todolist function does not exist in bACTPaymentMaintenance checked in green code as well as in vb 6 code.s
            'm_lReturn = g_oBusiness.GetID(vSearch:=vSearch, vID:=vID)

            ' Check for errors
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Or m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to call GetID method")
                Return result
            End If

            ' Return the value.
            result = m_lReturn


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
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
        Const kMethodName As String = "GetValidSources"


        result = gPMConstants.PMEReturnCode.pmtrue
        'Call PMUser to get the Sources
        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_g_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
            ' Failed to get an instance of the business object.
            'Raise Error.
            gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to get instance of bPMUser.Business")
            Return result

        End If


        m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
        If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
            'Raise Error.
            gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to get valid sources")
            Return result

        End If
        ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            g_oPMUser.Dispose()
            g_oPMUser = Nothing
        End If


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"


        result = gPMConstants.PMEReturnCode.pmtrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
            'Raise Error.
            gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="LoadInterface method Failed")
            Return result
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
            'Raise Error.
            gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="ShowInterface method Failed")
            Return result
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
            'Raise Error.
            gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="UnLoadInterface method Failed")
            Return result
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
        Const kMethodName As String = "LoadInterface"

        'developer guide no. 50
        frmInterface = New frmInterface
        result = gPMConstants.PMEReturnCode.pmtrue

        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            'eck090500
            'developer guide no. 24
            .SourceArray = m_vSourceArray
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        m_lReturn = CType(frmInterface.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
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
        Const kMethodName As String = "UnLoadInterface"


        result = gPMConstants.PMEReturnCode.pmtrue

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status
            m_sStepStatus.Value = .StepStatus

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing


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
        Const kMethodName As String = "ShowInterface"


        result = gPMConstants.PMEReturnCode.pmtrue

        ' Display the interface.
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result
    End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        Try




        Catch ex As Exception

            ' Error Section.

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=ex)

        Finally


        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class