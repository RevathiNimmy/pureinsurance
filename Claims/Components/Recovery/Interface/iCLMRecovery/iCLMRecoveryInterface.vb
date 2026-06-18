Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 11/08/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Interface"
    'Developer Guide no. 50
    Dim frmSelectPeril As frmSelectPeril
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Variables for Salvage Recovery
    Private m_lClaimId As Integer
    Private m_sClaimNumber As String = ""
    Private m_sPolicyHolder As String = ""
    Private m_sPolicyNo As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_bIsSalvage As Boolean

    ' Indicate that we are running in balance and close mode
    Private m_bBalanceAndCloseClaim As Boolean
    Private m_bDisplaySalvageRecovery As Boolean
    Private m_bDisplayThirdPartyRecovery As Boolean
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer
    'Start-(Arul Stephen)-(Issue No-PNREF86367)
    Private m_bFurtherReceipts As Boolean
    Private m_lOriginalClaimId As Integer
    'End-(Arul Stephen)-(Issue No-PNREF86367)
    Private m_bIsReserveUpdatednTaskCompleted As Boolean


    ' ***************************************************************** '
    '                       STANDARD PROPERTIES
    ' ***************************************************************** '
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
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property


    ' ***************************************************************** '
    '                        CUSTOM PROPERTIES
    ' ***************************************************************** '
    Public Property ClaimNumber() As String
        Get
            Return m_sClaimNumber
        End Get
        Set(ByVal Value As String)
            m_sClaimNumber = Value
        End Set
    End Property

    Public Property ClaimId() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    '
    ' Date :10/08/2000
    ' ***************************************************************** '
    Public Function Initialise() As Integer

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
            g_iLanguageID = g_oObjectManager.LanguageID
            g_iSourceID = g_oObjectManager.SourceID

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMRecovery.Business", vInstanceManager:="ClientManager")
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
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
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

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key array.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Arul Stephen
            Dim g_vIsRI2007 As String = ""
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=g_vIsRI2007)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetKeys", "getProductOptionValue Method failed to read the Product Option RI2007")
            End If
            'End Arul Stephen

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameClaimCnt

                        m_lClaimId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimReference

                        m_sClaimNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyHolder

                        m_sPolicyHolder = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyNumber

                        m_sPolicyNo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyID

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameRecoveryIsSalvage

                        m_bIsSalvage = gPMFunctions.ToSafeBoolean(CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameBalanceAndCloseClaim

                        m_bBalanceAndCloseClaim = gPMFunctions.ToSafeBoolean(CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameDisplaySalvageRecovery

                        m_bDisplaySalvageRecovery = gPMFunctions.ToSafeBoolean(CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameDisplayThirdPartyRecovery

                        m_bDisplayThirdPartyRecovery = gPMFunctions.ToSafeBoolean(CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                        'Arul Stephen
                    Case PMNavKeyConst.PMKeyNameDisplayClaimReinsurance
                        If g_vIsRI2007 = "1" Then

                            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow) = True
                        End If
                        'End Arul Stephen
                        'Start-(Arul Stephen)-(Issue No-PNREF86367)
                    Case "FurtherReceipts"

                        m_bFurtherReceipts = (CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = 1)
                    Case PMNavKeyConst.PMKeyNameRealClaimID

                        m_lOriginalClaimId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'End-(Arul Stephen)-(Issue No-PNREF86367)
                    Case "ReserveUpdatednTaskCompleted"

                        m_bIsReserveUpdatednTaskCompleted = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
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
    ' Description: Stores all of the key array with the parameter members.
    '
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'S4B Claim Enhancements R&D 2005


            ' Initialise the key array with the number of keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 4)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_iTask


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sClaimNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lClaimId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePolicyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "ReserveUpdatednTaskCompleted"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_bIsReserveUpdatednTaskCompleted


            ' Nothing new to set, just return

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
    ' Description: Stores all of the summary array with the parameter members.
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
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

                m_lNavigate = CType(CInt(vNavigate), gPMConstants.PMENavigateButtonStatus)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
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
    ' Date :11/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'S4B Claim Enhancements R&D 2005
            m_lReturn = iPMFunc.getUnderwritingOrAgency(g_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we are in balance and close claim mode do that else continue with UI
            If m_bBalanceAndCloseClaim Then
                ' Balance this recovery

                m_lReturn = g_oBusiness.BalanceRecovery(vClaimId:=m_lClaimId, vIsSalvage:=m_bIsSalvage)
            Else
                'S4B Claim Enhancements R&D 2005

                ' Determine running mode
                Select Case m_sTransactionType
                    Case "C_SA"
                        g_lRecoveryMode = MainModule.RecoveryModeEnum.RMSalvageReceipt
                    Case "C_RV"
                        g_lRecoveryMode = MainModule.RecoveryModeEnum.RMThirdPartyReceipt
                    Case Else
                        g_lRecoveryMode = IIf(m_bIsSalvage, MainModule.RecoveryModeEnum.RMSalvageReserve, MainModule.RecoveryModeEnum.RMThirdPartyReserve)
                End Select

                ' Starts the interface processing.
                m_lReturn = ProcessInterface()
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue
        If (m_bIsSalvage And m_bDisplaySalvageRecovery) Or (Not m_bIsSalvage And m_bDisplayThirdPartyRecovery) Then
            'Start-(Arul Stephen)-(Issue No-PNREF86367)
            If m_bFurtherReceipts Then
                m_lReturn = ReloadClaimDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessInterface", "ReloadClaimDetails method failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            'End-(Arul Stephen)-(Issue No-PNREF86367)

            ' Load the interface into memory.
            m_lReturn = LoadInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to load the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the interface from memory.
            m_lReturn = UnLoadInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to unload the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result

    End Function
    'Start-(Arul Stephen)-(Issue No-PNREF86367)
    ' ***************************************************************** '
    ' Name: ReloadClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description: Reload the claim details and re-lock the claim so
    '               no other updates can take place...
    '
    ' History:
    '           Created : MEvans : 26-11-2004 : PN17073
    ' ***************************************************************** '
    Public Function ReloadClaimDetails() As Integer
        Dim result As Integer = 0
        Dim bCLMFindClaim As Object

        Const kMethodName As String = "ReloadClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oClaimStatus As bCLMFindClaim.Business
        Dim lClaimId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' lock claim details
            lReturn = LockClaim()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("lockClaim", "Failed to lock claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create instance of bCLMFindClaim.Business
            Dim temp_oClaimStatus As Object
            lReturn = g_oObjectManager.GetInstance(temp_oClaimStatus, "bCLMFindClaim.Business", vInstanceManager:="ClientManager")
            oClaimStatus = temp_oClaimStatus
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bCLMFindClaim.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oClaimStatus.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMFindCLaim.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' reload the claim into the work tables

            lReturn = oClaimStatus.ProcessCopyClaim(v_lClaimId:=m_lClaimId, r_lCopyClaimId:=lClaimId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMFindClaim.Business.CopyClaimToWork Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lClaimId = lClaimId


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oClaimStatus = Nothing

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    '***************************************************************** '
    ' Name: LockClaim
    '
    ' Parameters: n/a
    '
    ' Description: Lock the specified claim using the pmlock method
    '
    ' History:
    '           Created : MEvans : 29-11-2004 : PN17073
    ' ***************************************************************** '
    Public Function LockClaim() As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object

        Const kMethodName As String = "LockClaim"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get instance of bpmlock.user
            Dim temp_oPMLock As Object
            lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_objectManger.GetInstance", "Failed to get instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' lock claim record based on the claim id

            lReturn = oPMLock.LockKey(sKeyName:=PMNavKeyConst.PMKeyNameRealClaimID, vKeyValue:=m_lOriginalClaimId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)

            ' check the return code
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                ' to determine if an error occurred or
                ' if the record is locked by someone else
                If sLockedBy = "ERROR" Then
                    gPMFunctions.RaiseError("LockKey", "Error trying to lock record", gPMConstants.PMELogLevel.PMLogError)
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    MessageBox.Show("Claim currently locked by " & sLockedBy & _
                                Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Find Claim")

                    Return result
                End If

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oPMLock = Nothing

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UnlockClaim
    ' Description:
    ' History: 22/01/2003 - Alix
    ' ***************************************************************** '

    'Private Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
    'Dim result As Integer = 0
    'Dim bPMLock As Object
    '

    'Dim oPMLock As bPMLock.User
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Get bPMLock
    'Dim temp_oPMLock As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oPMLock = temp_oPMLock
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    'Return result
    'End If
    '
    'If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

    'm_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)
    'End If
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End If
    '
    'oPMLock = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    'Return result
    'End Try
    'End Function

    'End-(Arul Stephen)-(Issue No-PNREF86367)

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
    '***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0

        'Developer Guide no. 50
        frmSelectPeril = New frmSelectPeril
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        With frmSelectPeril
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Task = m_iTask

            .ClaimId = m_lClaimId
            .ClaimNumber = m_sClaimNumber
            .ClientName = m_sPolicyHolder
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .IsSalvage = m_bIsSalvage
            .PolicyNumber = m_sPolicyNo
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmSelectPeril = frmSelectPeril

        ' Check if we have had an error so far.
        If frmSelectPeril.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmSelectPeril.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' Date :11/08/2000
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.

        m_lStatus = frmSelectPeril.Status


        m_lClaimId = frmSelectPeril.ClaimId
        m_sClaimNumber = frmSelectPeril.ClaimNumber

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmSelectPeril.Close()
        frmSelectPeril = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' Date :11/08/2000
    ' ***************************************************************** '
    Private Function ShowInterface(ByVal lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(frmSelectPeril, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmSelectPeril.ErrorNumber <> 0 Then
                result = frmSelectPeril.ErrorNumber
            End If
        End If

        Return result

    End Function

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

