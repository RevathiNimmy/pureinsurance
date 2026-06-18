Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/09/00
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

    Private m_oGetDocument As bCLMGetClaimLetter.Business
    Private m_oPMWrkTaskInstance As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lClaimID As Integer
    Private m_lPartyCnt As Integer
    Private m_sPartyShortname As String = ""
    Private m_sClaimNumber As String = ""
    Private m_lProcessType As Integer
    Private m_lPolicycnt As Integer
    Private m_sDocDescription As String = ""
    Private m_sTaskCode As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode


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
                g_sUserName = .UserName
                g_sPassword = .Password
                g_iUserID = .UserID
                g_iCurrencyID = .CurrencyID
                g_iLogLevel = .LogLevel
                g_iLanguageID = 1
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
            Dim temp_m_oGetDocument As Object
            If g_oObjectManager.GetInstance(temp_m_oGetDocument, "bCLMGetClaimLetter.Business", vinstancemanager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oGetDocument = temp_m_oGetDocument


                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                m_oGetDocument = temp_m_oGetDocument
            End If

            'MKW1500403 PN1523 Changed to use NavigatorV3 from interface. (to use keys).

            m_lReturn = g_oObjectManager.GetInstance(oobject:=m_oPMWrkTaskInstance, sclassname:="iPMWrkTaskInstance.NavigatorV3")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to create the business object
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oPMWrkTaskInstance = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create 'iPMWrkTaskInstance.NavigatorV3'.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If


            m_lReturn = CType(m_oPMWrkTaskInstance, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to create the business object
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oPMWrkTaskInstance = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Initialise 'iPMWrkTaskInstance.NavigatorV3'.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

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
                If m_oGetDocument IsNot Nothing Then
                    m_oGetDocument.Dispose()
                    m_oGetDocument = Nothing
                End If
                If m_oPMWrkTaskInstance IsNot Nothing Then
                    m_oPMWrkTaskInstance.Dispose()

                End If
                m_oPMWrkTaskInstance = Nothing
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
                    Case PMNavKeyConst.PMKeyNameClaimID

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimNumber

                        m_sClaimNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

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
        Dim vResultArray(,) As Object
        Dim vKeyArray(,) As Object
        Dim sTaskGroupCode As String = ""
        Dim lTaskGroupID, lTaskID As Integer
        Dim vArray(1, 4) As Object 'MKW150403 PN1523



        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim vKeyArray(1, 1)


        m_lReturn = m_oGetDocument.GetClientAndPolicyID(m_lClaimID, vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lPolicycnt = CInt(vResultArray(0, 0))


        m_lPartyCnt = CInt(vResultArray(1, 0))


        m_sPartyShortname = CStr(vResultArray(5, 0)).Trim()

        'MKW150403 PN1523 Changed to use NavigatorV3

        m_lReturn = m_oPMWrkTaskInstance.NavigatorV3_SetProcessModes(vTransactionType:=m_sTransactionType, vTask:=gPMConstants.PMEComponentAction.PMAdd)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC250601 set default task group to claims
        sTaskGroupCode = "Claims"
        m_sTaskCode = ""

        GetIDsFromCodes(v_sTaskGroupCode:=sTaskGroupCode, v_sTaskCode:=m_sTaskCode, r_lTaskGroupID:=lTaskGroupID, r_lTaskID:=lTaskID)

        'MKW150403 PN1523 START Removed Code
        'm_oPMWrkTaskInstance.PMWrkTaskGroupId = lTaskGroupID&
        'm_oPMWrkTaskInstance.PMWrkTaskId = lTaskID&
        'm_oPMWrkTaskInstance.Customer = m_sPartyShortname$
        'm_oPMWrkTaskInstance.Description = "CLAIM: " & m_sClaimNumber$
        '
        'm_lReturn& = m_oPMWrkTaskInstance.Start
        'MKW150403 PN1523 END Removed Code

        'MKW150403 PN1523 START Populate Keys Array

        vArray(0, 0) = "party_cnt"

        vArray(1, 0) = m_lPartyCnt

        vArray(0, 1) = PMNavKeyConst.PMKeyNameTaskGroupID

        vArray(1, 1) = lTaskGroupID

        vArray(0, 2) = PMNavKeyConst.PMKeyNameTaskID

        vArray(1, 2) = lTaskID

        vArray(0, 3) = PMNavKeyConst.PMKeyNameTaskCustomer

        vArray(1, 3) = m_sPartyShortname

        vArray(0, 4) = PMNavKeyConst.PMKeyNameTaskDescription

        vArray(1, 4) = "CLAIM: " & m_sClaimNumber


        m_lReturn = m_oPMWrkTaskInstance.NavigatorV3_SetKeys(vArray)

        m_lReturn = m_oPMWrkTaskInstance.NavigatorV3_Start
        'MKW150403 PN1523 END Populate Keys Array

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'MKW150403 PN1523 Changed to use NavigatorV3

        m_lStatus = m_oPMWrkTaskInstance.NavigatorV3_Status

        If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
            Return result
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
    ' Name: GetIDsFromCodes
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub GetIDsFromCodes(ByVal v_sTaskGroupCode As String, ByVal v_sTaskCode As String, ByRef r_lTaskGroupID As Integer, ByRef r_lTaskID As Integer)

        Dim oPMLookup As bPMLookup.Business



        r_lTaskGroupID = 0
        r_lTaskID = 0

        Dim temp_oPMLookup As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookup, "bPMLookup.Business", vinstancemanager:=gPMConstants.PMGetViaClientManager)
        oPMLookup = temp_oPMLookup
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If


        oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        If v_sTaskGroupCode.Trim() <> "" Then

            m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:="PMWrk_task_group", v_sCode:=v_sTaskGroupCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=r_lTaskGroupID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lTaskGroupID = 0
            End If
        End If

        If v_sTaskCode.Trim() <> "" Then

            m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:="PMWrk_Task", v_sCode:=v_sTaskCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=r_lTaskID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lTaskID = 0
            End If
        End If


        oPMLookup.Dispose()
        oPMLookup = Nothing



    End Sub
End Class
