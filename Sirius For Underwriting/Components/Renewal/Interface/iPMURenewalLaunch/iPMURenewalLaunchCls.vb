Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 26/09/00
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPMAuthorityLevel As Integer

    Private m_lInsuranceFileCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_sNavProcessCode As String = ""
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_bProcessValid As Boolean
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lRunMode As Integer

    'developer guide no. 50
    Dim frmInterface As frmInterface

    Private m_oBusiness As bSirRenewalLaunch.Business
    Private m_bCanTransferBroker As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property





    ' {* USER DEFINED CODE (Begin) *}


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

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Get an instance of the business object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRenewalLaunch.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSirRenewalLaunch.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
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
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameRunMode

                        m_lRunMode = CInt(Conversion.Val(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))))
                        '            Case "is_renewal_ammend"
                        '                IsRenewalAmmend = vKeyArray(PMKeyValue, lRow&)
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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



            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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




            result = gPMConstants.PMEReturnCode.PMError

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

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            'SetMousePointer PMMouseBusy

            Dim lPartyCnt, lInsuranceFolderCnt As Integer
            Dim sInsuranceRef As String = ""

            If m_lRunMode = 1 Then
                'Get client/policy details from the business object

                m_lReturn = m_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lPartyCnt:=lPartyCnt, r_lInsuranceFolderCnt:=lInsuranceFolderCnt, r_sInsuranceRef:=sInsuranceRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oBusiness.GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return result
                End If

                'Show the renewal events
                If m_sCallingAppName <> "iPMWrkComponentStarter" Then
                    m_lReturn = CType(ShowRenewalEvents(v_lPartyCnt:=lPartyCnt, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sInsuranceRef:=sInsuranceRef), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRenewalEvents Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Else

                GetBrokerTransferAuthority()

                frmInterface.ShowDialog()
                If frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    frmInterface.Close()
                    Return result
                Else
                    m_lRunMode = frmInterface.RenewalMode
                End If
                frmInterface.Close()
            End If

            m_lReturn = CType(AmendRenewal(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AmendRenewal Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'SetMousePointer PMMouseNormal

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    '
    ' Name: ShowRenewalEvents
    '
    ' Description:
    '
    ' History: 09/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ShowRenewalEvents(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sInsuranceRef As String) As Integer

        Dim result As Integer = 0
        Dim oListEvents As New iPMBListEvents.Interface_Renamed
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'developer guide no. 9
            m_lReturn = oListEvents.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oListEvents.Dispose()
                oListEvents = Nothing

                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oListEvents.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRenewalEvents")
                Return result
            End If

            oListEvents.PartyCnt = v_lPartyCnt
            oListEvents.InsuranceFolderCnt = v_lInsuranceFolderCnt
            oListEvents.InsuranceRef = v_sInsuranceRef
            oListEvents.InsuranceFileCnt = m_lInsuranceFileCnt
            oListEvents.EventGroupCode = "RENEWALS"
            'DGRIFF 18-03-2003 - Changed to False (CQ948)
            oListEvents.DisableEventGroupLookup = False

            m_lReturn = oListEvents.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oListEvents.Dispose()
                oListEvents = Nothing

                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oListEvents.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRenewalEvents")
                Return result
            End If

            oListEvents.Dispose()
            oListEvents = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRenewalEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRenewalEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            oListEvents.Dispose()

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AmendRenewal
    '
    ' Description:
    '
    ' History: 09/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AmendRenewal() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        Dim oRenewal As iPMURenewal.NavigatorV3
        'developer guide no. 101
        'Dim vKeyArray(1, 2) As Object
        Dim vKeyArray(1, 1) As Object

        oRenewal = New iPMURenewal.NavigatorV3()
        'developer guide no. 9
        m_lReturn = oRenewal.Initialise()

        If m_sCallingAppName = "iPMWrkComponentStarter" Then
            oRenewal.NavigatorV3_CallingAppName = "iPMWrkComponentStarter"
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oRenewal.Dispose()
            oRenewal = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oRenewal.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AmendRenewal")
            Return result
        End If

        'Insurance_file_cnt

        vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

        vKeyArray(1, 0) = m_lInsuranceFileCnt
        'Run mode

        vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameRunMode

        vKeyArray(1, 1) = m_lRunMode

        m_lReturn = oRenewal.NavigatorV3_SetKeys(vKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oRenewal.Dispose()
            oRenewal = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oRenewal.NavigatorV3_SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AmendRenewal")
            Return result
        End If

        m_lReturn = oRenewal.NavigatorV3_Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oRenewal.Dispose()
            oRenewal = Nothing

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oRenewal.NavigatorV3_Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AmendRenewal")
            Return result
        End If

        oRenewal.Dispose()
        oRenewal = Nothing

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Sub GetBrokerTransferAuthority()

        Dim vResult As Object



        m_lReturn = m_oBusiness.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="can_perform_broker_transfer", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMInteger, r_vResult:=vResult)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get user authority for broker transfer portfolio", ACApp, MessageBoxButtons.OK)
            Exit Sub
        End If

        m_bCanTransferBroker = (gPMFunctions.ToSafeLong(vResult, 0) = 1)
        'developer guide no. 50
        frmInterface = New frmInterface
        frmInterface.CanTransferBroker = m_bCanTransferBroker

        Exit Sub
    End Sub
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class