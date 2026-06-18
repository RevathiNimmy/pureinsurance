Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 02 Sep 2003
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History: AMB - created for ICB Deferred RI development
    '               This is a cloned version of the Get Claim Letter component
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer


    Private m_oCheckDeferredRI As bCLMCheckDeferredRI.Business

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lClaimID As Integer

    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_sClaimRef As String = ""

    'WR5
    Private m_bCheckDeferredRI As Boolean
    Private m_iWorkFlowId As Integer

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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oCheckDeferredRI As Object
            If g_oObjectManager.GetInstance(temp_m_oCheckDeferredRI, "bCLMCheckDeferredRI.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oCheckDeferredRI = temp_m_oCheckDeferredRI

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                m_oCheckDeferredRI = temp_m_oCheckDeferredRI
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
                If m_oCheckDeferredRI IsNot Nothing Then
                    m_oCheckDeferredRI.Dispose()
                    m_oCheckDeferredRI = Nothing
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
                    Case PMNavKeyConst.PMKeyNameClaimID

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameCheckDeferredReinsurance
                        m_bCheckDeferredRI = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId
                        m_iWorkFlowId = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

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
    ' AMB 11-Sep-03: 1.8.6 Deferred Reinsurance development
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim sClientName As String = ""
        Dim lRiskStatusID As Integer
        Dim sRiskStatusCode As String = ""
        Dim vResultArray(,) As Object ' eugh



        If Not m_bCheckDeferredRI And m_iWorkFlowId >= 1 And m_iWorkFlowId <= 3 Then ' If applicable to the Roadmap
            'Do nothing
        Else
            ' get the claim risk's status

            m_lReturn = m_oCheckDeferredRI.GetClaimRiskStatus(v_lClaimID:=m_lClaimID, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Return result
            End If

            If Information.IsArray(vResultArray) Then ' a status was returned, otherwise we just drop out
                lRiskStatusID = gPMFunctions.NullToLong(vResultArray(0, 0))
                sRiskStatusCode = gPMFunctions.NullToString(vResultArray(1, 0)).Trim().ToUpper()

                If sRiskStatusCode = ksDeferredRIRiskStatus Then

                    MessageBox.Show("Payment cannot be processed - reinsurance is deferred." & _
                                    Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    "A work manager reminder task will be created for this claim payment.", "Deferred Reinsurance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    ' need policy holder's name for the work manager task,
                    ' might as well get it back up there ^ to avoid another DB hit
                    sClientName = gPMFunctions.NullToString(vResultArray(2, 0))


                    m_lReturn = m_oCheckDeferredRI.AddTaskToWorkManager(v_sClientName:=sClientName, v_sDescription:="Claim Payment required - Deferred Reinsurance on Risk", v_dtDueDate:=DateTime.Now)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Return result
                    End If

                    ' stop the roadmap
                    result = gPMConstants.PMEReturnCode.PMTrue
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                    Return result

                End If

            End If
        End If

        ' reinsurance is not deferred...the roadmap, continues...
        m_lStatus = gPMConstants.PMEReturnCode.PMOK


        Return gPMConstants.PMEReturnCode.PMTrue

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
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.
            'vSummaryArray(PMNavSummHeading, 0) = "Claim Reference"

            'JMK 16/05/2001 get the Claim Reference (to display in Roadmap)
            '    vSummaryArray(PMNavSummHeading, 0) = "claim_ref"
            '    vSummaryArray(PMNavSummValue, 0) = m_sClaimNumber

            ''    vSummaryArray(PMNavSummValue, 0) = m_sClaimRef$

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        ' <-- waste some processor cycles here...
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
End Class