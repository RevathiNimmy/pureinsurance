Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
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


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Variables for Salvage Recovery
    Private m_lClaimID As Integer
    Private m_sClaimNumber As String = ""
    Private m_nFindClaimMode As Integer
    Private m_sPolicyHolder As String = ""
    Private m_sPolicyNo As String = ""
    Private m_lInsuranceFileCnt As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

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

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property
    Public Property ClaimMode() As Integer
        Get

            Return m_nFindClaimMode

        End Get
        Set(ByVal Value As Integer)

            m_nFindClaimMode = Value

        End Set
    End Property
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
            Dim temp_g_oBusiness1 As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness1, "bCLMSalvageRecovery.Business", vInstanceManager:="ClientManager")
            g_oBusiness1 = temp_g_oBusiness1

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

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
            If disposing Then
                If g_oBusiness1 IsNot Nothing Then
                    g_oBusiness1.Dispose()
                    g_oBusiness1 = Nothing
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



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameClaimCnt

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimReference

                        m_sClaimNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameFindClaimMode

                        m_nFindClaimMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        '            Case PMKeyNamePolicyHolder
                    Case PMNavKeyConst.PMKeyNameClientHolder

                        m_sPolicyHolder = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePolicyNumber

                        m_sPolicyNo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'TN20010329 Start
                    Case "insurancefile_cnt"

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'TN20010329 End
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
            ReDim vKeyArray(1, 4)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_sClaimNumber

            'Setting the Task Value coming from Navigator


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFindClaimMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_nFindClaimMode



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

            ' Set the process modes for the business object.
            If Not (g_oBusiness1 Is Nothing) Then

                m_lReturn = g_oBusiness1.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object (g_oBusiness1)", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

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
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim bInfoOnlyStatus As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        'RWH(06/04/2001) Sort out ClaimMode which is used throughout project.
        Select Case m_sTransactionType
            Case ACTransTypeClaimOpen
                ClaimMode = gPMConstants.PMEComponentAction.PMAdd

            Case ACTransTypeClaimRevise, ACTransTypeClaimPay
                ClaimMode = gPMConstants.PMEComponentAction.PMEdit

                'RWH(15/06/01) If this is a claim we have just changed from
                'Info Only to Full Claim then change Task to Add.

                If m_sTransactionType <> "C_CO" Then

                    m_lReturn = g_oBusiness1.GetInfoOnlyStatus(v_lClaim_Id:=m_lClaimID, r_bInfoStatus:=bInfoOnlyStatus)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If bInfoOnlyStatus Then
                            m_iTask = gPMConstants.PMEComponentAction.PMAdd
                        End If
                    End If
                End If



                'AJM 25/04/01 set mode for PMView
            Case ACTransTypeClaimSalvage
                ClaimMode = gPMConstants.PMEComponentAction.PMView

            Case Else

        End Select

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

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
    ' Date :11/08/2000
    '
    ' Edit History:Pandu
    '***************************************************************** '
    Dim frmSelectPeril As frmSelectPeril
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        With frmSelectPeril
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Task = m_iTask


            .ClaimNumber = m_sClaimNumber
            .ClaimId = m_lClaimID
            .ClaimMode = m_nFindClaimMode
            .ClientName = m_sPolicyHolder
            .PolicyNumber = m_sPolicyNo

            'TN20010329 Start
            .InsuranceFileCnt = m_lInsuranceFileCnt
            'TN20010329 End
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
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmSelectPeril

            m_lStatus = .Status


            m_lClaimID = .ClaimId
            m_sClaimNumber = .ClaimNumber



        End With

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
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

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
        Dispose()
    End Sub

End Class

