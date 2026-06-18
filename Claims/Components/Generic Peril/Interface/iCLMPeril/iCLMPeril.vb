Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 23rd Aug 2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' 01/06/2005 : MKR : PN 21215 : Made txtComments uneditable while it is
    '              in view or disabled mode. Also made it scrollable.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    ' SET 01082002 - Scalability
    Private Const PMKeyNameNavigatorTitle1 As String = "navigator_title_1"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    'Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer
    Private m_cThisPayment As Decimal


    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
    Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
    'TN20010620 start
    Private m_lDisableScreen As gPMConstants.PMEReturnCode 'set to pmtrue to disable all controls
    Private m_bOpenClaimNoTrans As Boolean
    Private m_bReserveLimitExceeded As Boolean
    Private m_dExceededReserve As Decimal

    Public Property DisableScreen() As Integer
        Get
            Return m_lDisableScreen
        End Get
        Set(ByVal Value As Integer)
            m_lDisableScreen = Value
        End Set
    End Property
    'TN20010620 end

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

    Public Property ReserveLimitExceeded() As Boolean
        Get
            Return m_bReserveLimitExceeded
        End Get
        Set(ByVal Value As Boolean)
            m_bReserveLimitExceeded = Value
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
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise
        Dim result As Integer = 0
        Dim sTitle, sMessage As String

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

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMPeril.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Display error stating the problem.
                ' Get description from the resource file.

                'Developers Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Developers Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else

                m_lReturn = g_oBusiness.Initialise(g_oObjectManager.UserName, g_oObjectManager.Password, g_oObjectManager.UserID, g_oObjectManager.SourceID, g_oObjectManager.LanguageID, g_oObjectManager.CurrencyID, g_oObjectManager.LogLevel, "bCLMPeril")
            End If

            'DC270302 -only for underwriting


            Dim temp_g_oClaimTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oClaimTrans, "bControlTransClaims.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oClaimTrans = temp_g_oClaimTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bControlTransClaims object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
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

                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameOperateMode
                        m_iTask = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimPerilID
                        ' UNCOMMENT FOR INTEGRATION*****************************
                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then
                            m_lPerilID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameRealClaimID
                        ' UNCOMMENT FOR INTEGRATION*****************************
                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then
                            m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyRiskType
                        ' UNCOMMENT FOR INTEGRATION*****************************

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sRisktype = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        ' UNCOMMENT FOR INTEGRATION*****************************

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lInsurance_file_cnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameRiskID
                        ' UNCOMMENT FOR INTEGRATION*****************************

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lRiskID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyPerilTypeID, PMNavKeyConst.PMKeyNamePerilTypeId

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lPerilTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameRiskTypeID

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_lRiskTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameClaimReference

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sClaimRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameNoTransaction

                        m_bOpenClaimNoTrans = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
                    Case PMNavKeyConst.PMKeyNameScreenCaption

                        m_sScreenCaption = gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
                End Select

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
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
            ReDim vKeyArray(1, 4)
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)



            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameSumInsured

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_cTotalSumInsured ' Sum_insured


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeyNameCurrentReserve

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_cTotalCurrentReserve ' Current_reserve


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_cThisPayment


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = g_lPaymentCurrencyID
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameScreenCaption

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
            Return result

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameNavigatorTitle1

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sNavigatorTitle

            ' {* USER DEFINED CODE (End) *}

            Return result

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

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

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
        objfrmInterface = New frmInterface


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        With objfrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            'TN20010620 start
            .DisableScreen = m_lDisableScreen
            'TN20010620 end
            .IsOpenClaimNoTrans = m_bOpenClaimNoTrans

            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
            .ScreenCaption = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)

            'Making it enable so that user can scroll if the text is large...

            .txtComments.Enabled = True

            'DC210205 : PN18872 : set comments correctly
            If .Task = gPMConstants.PMEComponentAction.PMView Then
                .txtComments.ReadOnly = True
                .txtComments.ForeColor = SystemColors.GrayText
            Else
                .txtComments.ReadOnly = False
                .txtComments.ForeColor = ColorTranslator.FromOle(FileAttribute.Normal)
            End If

            If .DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then
                .txtComments.ReadOnly = True
                .txtComments.ForeColor = SystemColors.GrayText
            End If


        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = objfrmInterface
        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)

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
            m_cThisPayment = .ThisPayment
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

        m_bReserveLimitExceeded = objfrmInterface.ReserveLimitExceeded
        m_dExceededReserve = objfrmInterface.ExceededReserve

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

End Class

