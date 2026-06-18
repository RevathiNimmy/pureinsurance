Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/07/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '
    'Replaced iPMFunc.GetResData to GetResData in the whole document

    'developer guide no. 69
    Dim m_ofrmInterface As frmInterface


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Variables for Find Claim
    Private m_lClaimCnt As Integer
    Private m_sClaimRef As String = ""
    Private m_lInsuranceFilecnt As Integer
    Private m_sPolicyRef As String = ""
    'Private m_lAgentCnt As Long
    'Private m_sAgent As String
    Private m_lPolicyHolderCnt As Integer
    Private m_sPolicyHolder As String = ""
    Private m_nFindClaimMode As Integer
    ' CJB 240904 PN15172
    Private m_lRiskTypeId As Integer
    Private m_lPartyCnt As Integer
    Private m_bAskDPAQuestions As Boolean

    Private m_vSourceArray As Object ' MKW 190503 PN2032

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    Private m_IIsComplaint As Integer
    Private m_bSelectVersionEnabled As Boolean

    Private m_lRealClaimID As Integer
    Private m_bIncludeClosedClaims As Boolean
    Private m_iClaimWorkflowId As Integer
    Private m_lCaseID As Integer
    Private m_bRecovery As Boolean ' PN 58569


    Public WriteOnly Property IIsComplaint() As Integer
        Set(ByVal Value As Integer)

            m_IIsComplaint = True


        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
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

    'DC180202
    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public Property ClaimRef() As String
        Get

            Return m_sClaimRef

        End Get
        Set(ByVal Value As String)

            m_sClaimRef = Value

        End Set
    End Property

    Public Property ClaimCnt() As Integer
        Get

            Return m_lClaimCnt

        End Get
        Set(ByVal Value As Integer)

            m_lClaimCnt = Value

        End Set
    End Property

    Public WriteOnly Property RealClaimID() As Integer
        Set(ByVal Value As Integer)

            m_lRealClaimID = Value

        End Set
    End Property
    Public WriteOnly Property IncludeClosedClaims() As Integer
        Set(ByVal Value As Integer)

            m_bIncludeClosedClaims = Value

        End Set
    End Property

    Public Property PolicyRef() As String
        Get

            Return m_sPolicyRef

        End Get
        Set(ByVal Value As String)

            m_sPolicyRef = Value

        End Set
    End Property

    Public Property PolicyHolderCnt() As Integer
        Get

            Return m_lPolicyHolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPolicyHolderCnt = Value

        End Set
    End Property

    Public Property PolicyHolder() As String
        Get

            Return m_sPolicyHolder

        End Get
        Set(ByVal Value As String)

            m_sPolicyHolder = Value

        End Set
    End Property

    Public Property InsFileCnt() As Integer
        Get

            Return m_lInsuranceFilecnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFilecnt = Value

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

    Public Property AskDPAQuestions() As Boolean
        Get
            Return m_bAskDPAQuestions
        End Get
        Set(ByVal Value As Boolean)
            m_bAskDPAQuestions = Value
        End Set
    End Property

    Public WriteOnly Property CaseID() As Integer
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :15/07/2000
    '
    ' Edit History: Pandu
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

                g_iUserID = .UserID ' MKW 190503 PN2032 START
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
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMFindClaim.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            '    ' Get an instance of the business object via
            '    ' the public object manager.
            Dim temp_g_oBackofficelink As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBackofficelink, "bBackOfficeLink.bBOLink", vInstanceManager:="Clientmanager")
            g_oBackofficelink = temp_g_oBackofficelink

            ' Set g_oBackofficelink = New bBackOfficeLink.bBOLink


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
    ' Date :15/07/2000
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
    ' Description: Stores all of the parameter members with the key
    '              array.
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

            'Initialise variables
            m_bAskDPAQuestions = True

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



                'Developer Guide no. 248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameClaimCnt

                        m_lClaimCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimReference

                        m_sClaimRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFilecnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsReference

                        m_sPolicyRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyHolderCnt

                        m_lPolicyHolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyHolder

                        m_sPolicyHolder = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameFindClaimMode

                        m_nFindClaimMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameAskDPAQuestions

                        m_bAskDPAQuestions = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "SelectVersion"

                        m_bSelectVersionEnabled = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameIncludeClosedClaims

                        m_bIncludeClosedClaims = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId

                        m_iClaimWorkflowId = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
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
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 11)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePolicyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFilecnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sClaimRef


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameClientHolder

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sPolicyHolder

            'Setting the Task Value coming from Navigator


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFindClaimMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_iTask

            ' CJB 240904 PN15172

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameRiskTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lRiskTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lInsuranceFilecnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameRealClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_lRealClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameIncludeClosedClaims

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_bIncludeClosedClaims


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameClaimWorkflowId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_iClaimWorkflowId

            'PN 58569
            If m_bRecovery Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameRecoveryIsSalvage

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = Math.Abs(CInt(m_bRecovery))
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameRecoveryIsSalvage

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = -1
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
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        'Dim lRow As Long
        '
        '    On Error GoTo Err_GetSummary
        '
        'DC191203 PN9192 & PN9193 needs setting even tho no used as such
        Return gPMConstants.PMEReturnCode.PMTrue
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        '    ' Initialise the summary array with the number of
        '    ' items needed to be returned.
        '    ' Note: Remember arrays are zero based.
        '    ReDim vSummaryArray(PMNavSummValue, 0)
        '
        '    ' Assign the key array with the parameter members.
        '    vSummaryArray(PMNavSummHeading, 0) = "Claim Reference"
        '    vSummaryArray(PMNavSummValue, 0) = m_sClaimRef$
        '
        '    ' {* USER DEFINED CODE (End) *}
        '
        '    Exit Function
        '
        '
        'Err_GetSummary:
        '
        '    GetSummary = PMError
        '
        '    ' Log Error Message
        '    LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="GetSummary Failed", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="GetSummary", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description
        '
        '    Exit Function

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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' MKW 190503 PN2032
            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Carry on without defaults set
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
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

        If m_sClaimRef <> "" Then
            m_iClaimWorkflowId = gPMConstants.PMWorkflowPayClaim

            m_ofrmInterface.ProcessOKClick()
        Else
            If m_sTransactionType = "C_CP" And m_iClaimWorkflowId = 0 Then
                'set the workflowid
                m_iClaimWorkflowId = gPMConstants.PMWorkflowPayClaim
            End If
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
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    '***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.69
        m_ofrmInterface = New frmInterface
        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Task = m_iTask
            .SelectedClaimCnt = m_lClaimCnt
            .ClaimRef = m_sClaimRef

            .PolicyHolder = m_sPolicyHolder
            .PolicyRef = m_sPolicyRef

            ' MKW 190503 PN2032 START


            'developer guide no. 24
            .SourceArray = m_vSourceArray

            'MRH PN11811 10/05/2004
            .IsComplaint = m_IIsComplaint

            .AskDPAQuestions = m_bAskDPAQuestions
            .SelectVersionEnabled = m_bSelectVersionEnabled
            .IncludeClosedClaims = m_bIncludeClosedClaims
            .CaseID = m_lCaseID

        End With

        ' Load the instance of the interface into memory.
        m_ofrmInterface.frmInterfaceLoad()

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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface

            m_lStatus = .Status

            'DC180202
            m_iTask = .Task

            m_lClaimCnt = .ClaimCnt
            m_lInsuranceFilecnt = .InsuranceFileCnt
            m_sClaimRef = .ClaimRef
            m_sPolicyHolder = .PolicyHolder
            m_sPolicyRef = .PolicyRef

            ' CJB 240904 PN15172
            m_lRiskTypeId = .RiskTypeID
            m_lPartyCnt = .PartyCnt
            m_lRealClaimID = .RealClaimID
            m_bIncludeClosedClaims = .IncludeClosedClaims
            m_bRecovery = .Recovery

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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '    m_lReturn = SetWindowPlacement(frmInterface.hwnd, True)

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


    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    ' Added : MKW 190503 PN2032
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_g_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        '    ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse

            '        ' Display error stating the problem.

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

        '    ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            g_oPMUser.Dispose()
            g_oPMUser = Nothing

        End If

        Return result

    End Function
End Class

