Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms

'Developer Guide No: 129
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 11/08/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Variables for Find Claim
    Private m_lClaimID As Integer
    Private m_lOriginalClaimID As Integer
    Private m_sClaimNumber As String = ""
    Private m_nFindClaimMode As Integer
    Private m_lRiskTypeID As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Set to pmtrue to delete work table when cancelled
    Private m_lDeleteWorkTableFlag As Integer
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)
    Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)
    ' Indicate that we are running in balance and close mode
    Private m_bBalanceAndCloseClaim As Boolean

    ' AMB 04/03/2003: PS220/123 - Claims roadmaps
    Private m_bIsIAG As Boolean


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

    'This property is used to store the Claim Number being
    'passed from the previous screen
    Public Property ClaimNumber() As String
        Get

            Return m_sClaimNumber

        End Get
        Set(ByVal Value As String)

            m_sClaimNumber = Value

        End Set
    End Property

    'This property is used to store the Claim Id being
    'passed from the previous screen
    Public Property ClaimId() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property
    'This property is used to store the Claim Mode being
    'passed from the previous screen
    Public Property ClaimMode() As Integer
        Get

            'DC281100
            'ClaimMode = m_nFindClaimMode
            Return g_nPMMode

        End Get
        Set(ByVal Value As Integer)

            'DC281100
            'm_nFindClaimMode = iClaimMode
            ClaimMode = g_nPMMode

        End Set
    End Property
    'This property is used to store the Claim Id being
    'passed from the previous screen
    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeID = Value
        End Set
    End Property


    'TN20010605 start

    Public Property DeleteWorkTableFlag() As Integer
        Get
            m_lDeleteWorkTableFlag = m_lDeleteWorkTableFlag
        End Get
        Set(ByVal Value As Integer)
            m_lDeleteWorkTableFlag = Value
        End Set
    End Property
    'TN20010605 end


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :10/08/2000
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
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMInfoChklst.Business", vInstanceManager:="ClientManager")
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

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameClaimCnt

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimReference

                        m_sClaimNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameFindClaimMode
                        'DC281100
                        'm_nFindClaimMode = CInt(vKeyArray(PMKeyValue, lRow))
                        'AJM (26/07/2001) - if 'Pay' claim set mode to PMEdit

                        If m_sTransactionType = "C_CP" And CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = 3 Then
                            g_nPMMode = 2
                        Else

                            g_nPMMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameRiskTypeID

                        m_lRiskTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "DeleteWorkTableFlag"

                        m_lDeleteWorkTableFlag = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameBalanceAndCloseClaim
                        m_bBalanceAndCloseClaim = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)
                    Case PMNavKeyConst.PMKeyNameScreenCaption
                        m_sScreenCaption = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)

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
    ' Date :11/08/2000
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

            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)
            ReDim vKeyArray(1, 5)
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sClaimNumber

            'Setting the Task Value coming from Navigator


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFindClaimMode

            'DC281100
            'vKeyArray(PMKeyValue, 4) = m_nFindClaimMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = g_nPMMode
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameScreenCaption

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)



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
        '    GetSummary = PMTrue
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
    ' AMB 04/03/2003: PS220/123 - added CheckIAG for Claims Roadmaps development
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0
        Dim lShowInfo, lRiskTypeId As Integer
        Dim m_bWorkInfoOnlyFlag As Boolean
        Dim vUnderwritingOrAgency As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=vUnderwritingOrAgency)

            'Branch for UW as Broking does not require this functionality


            m_lReturn = g_oBusiness.GetRiskTypeId(v_lRiskCnt:=m_lRiskTypeID, r_lRiskTypeId:=lRiskTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed to process the interface
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = g_oBusiness.ShowInfoCheckList(v_lRiskTypeID:=lRiskTypeId, r_lShowInfo:=lShowInfo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed to process the interface
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If lShowInfo = 1 Then

                ' AMB 04/03/2003: PS220/123 - added for Claims Roadmaps development
                If CheckIAG() = gPMConstants.PMEReturnCode.PMTrue Then
                    ' if it's NOT IAG then continue
                    If Not (m_bIsIAG) Then
                        ' If we are balancing and closing this claim then continue
                        If Not m_bBalanceAndCloseClaim Then
                            ' Starts the interface processing.
                            m_lReturn = ProcessInterface()

                            ' Check for errors.
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to process the interface.
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                Else
                    ' it's all gone wrong
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            ElseIf lShowInfo = 0 Then
                result = gPMConstants.PMEReturnCode.PMTrue
                'Don't Show the interface and move to next
                m_ofrmInterface = New frmInterface
                m_lReturn = g_oBusiness.GetInfoOnlyFlag(v_lClaimId:=m_lClaimID, r_bStatus:=m_bWorkInfoOnlyFlag)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If m_bWorkInfoOnlyFlag Then

                        m_lReturn = g_oBusiness.GetOriginalClaimID(m_lClaimID, m_lOriginalClaimID)
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to process the interface.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_ofrmInterface.TransactionType = m_sTransactionType
                        m_ofrmInterface.CopyWorkToClaim(m_lClaimID)
                        m_ofrmInterface.UnlockClaim(m_lOriginalClaimID)

                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    End If
                End If
                Dispose()

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
    ' Name: CheckIAG
    '
    ' Description: Checks hidden options for IAG/NRMA.
    ' PSL 19/02/2003  Issue 2092
    ' AMB 04/03/2003: PS220/123 - modified for Claims Roadmaps development
    ' ***************************************************************** '

    Private Function CheckIAG() As Integer
        'developer guide no.101
        Dim vValue As Object


        'developer guide no.98
        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option for IAG", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIAG")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If gPMFunctions.ToSafeInteger(vValue) = 1 Then
            m_bIsIAG = True
        Else
            m_bIsIAG = False
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

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


        'RWH(15/06/01) If this is a claim we have just changed from
        'Info Only to Full Claim then change Task to Add.

        g_bPrevInfoOnlyStatus = False

        If m_sTransactionType <> "C_CO" Then

            m_lReturn = g_oBusiness.GetInfoOnlyStatus(v_lClaim_Id:=m_lClaimID, r_bInfoStatus:=g_bPrevInfoOnlyStatus)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If g_bPrevInfoOnlyStatus Then
                    m_iTask = gPMConstants.PMEComponentAction.PMAdd
                End If
            End If
        End If


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
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
    '***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_ofrmInterface = New frmInterface

        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Task = m_iTask

            .RiskTypeId = m_lRiskTypeID
            .ClaimNumber = m_sClaimNumber
            .ClaimId = m_lClaimID
            'DC281100
            '.ClaimMode = m_nFindClaimMode
            .ClaimMode = g_nPMMode

            'TN20010605 start
            .DeleteWorkTableFlag = m_lDeleteWorkTableFlag
            'TN20010605 end
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = m_ofrmInterface
        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)
        m_ofrmInterface.Text = m_ofrmInterface.Text & " " & m_sScreenCaption
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.3)
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
    ' Date :11/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface

            m_lStatus = .Status


            m_lClaimID = .ClaimId
            m_sClaimNumber = .ClaimNumber



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
    ' Date :11/08/2000
    '
    ' Edit History :Pandu
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

