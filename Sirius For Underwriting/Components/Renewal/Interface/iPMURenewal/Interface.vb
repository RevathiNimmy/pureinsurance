Option Strict Off
Option Explicit On
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
    'developer guide no. 50
    Dim objfrmFilterRenewal As frmFilterRenewal
    Dim objfrmRenewal As frmRenewal
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

    ' Navigator starter
    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private m_bNavClosed As Boolean
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer
    'sj 09/10/2002 - start
    Private m_lRunMode As Integer ' 1 = IAG, 2 = Transfer, 3 = Amend, 4 = Accept
    'sj 09/10/2002 - end

    Private m_lLeadAgentCnt As Integer

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

            ' Create business object
            Dim temp_g_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oRenewal, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oRenewal = temp_g_oRenewal

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

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

                        g_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'sj 09/10/2002 - start
                    Case PMNavKeyConst.PMKeyNameRunMode

                        m_lRunMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'sj 09/10/2002 - end
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

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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
        Dim sTitle, sMessage As String
        Dim bProcessComplete As Boolean
        Dim sShortName As String = ""

        'developer guide no. 50
        objfrmFilterRenewal = New frmFilterRenewal
        objfrmRenewal = New frmRenewal
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lRunMode <> gPMConstants.ACRenewalModeTransfer Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                If m_sCallingAppName = "iPMWrkComponentStarter" Or m_sCallingAppName = "iPMURenSelection" Then
                    If m_lRunMode = 3 Or m_lRunMode = 1 Then

                        'developer guide no. 50
                        objfrmFilterRenewal.ShowDialog()
                        If objfrmFilterRenewal.Status = gPMConstants.PMEReturnCode.PMOK Then
                            objfrmRenewal.ProductId = objfrmFilterRenewal.ProductId
                            objfrmRenewal.RenewalDate = objfrmFilterRenewal.RenewalDate
                            objfrmRenewal.Source = objfrmFilterRenewal.SourceID
                            objfrmRenewal.AgentId = objfrmFilterRenewal.AgentId
                        Else
                            Return result
                        End If
                        'developer guide no. 50

                        objfrmRenewal.RunMode = gPMConstants.ACRenewalModeAmend
                        objfrmRenewal.ShowDialog()

                        Return result
                    Else
                        'developer guide no. 50
                        objfrmFilterRenewal.ShowDialog()
                    End If
                End If

                'developer guide no. 50

                If objfrmFilterRenewal.Status = gPMConstants.PMEReturnCode.PMOK Then
                    objfrmRenewal.ProductId = objfrmFilterRenewal.ProductId
                    objfrmRenewal.RenewalDate = objfrmFilterRenewal.RenewalDate
                    objfrmRenewal.Source = objfrmFilterRenewal.SourceID
                    objfrmRenewal.AgentId = objfrmFilterRenewal.AgentId
                Else
                    Return result
                End If
            ElseIf m_lRunMode = gPMConstants.ACRenewalModeTransfer Then
                m_lReturn = CType(SelectParty(vPartyCnt:=m_lLeadAgentCnt, vShortName:=sShortName, vSpecialParty:="AG", bIsInTransferMode:=True, bSupressCancelledAgents:=True), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        MessageBox.Show("Failed to get agent to filter renewal", ACApp, MessageBoxButtons.OK)
                    End If
                    Return result
                End If

                'developer guide no. 50
                objfrmRenewal.LeadAgentCnt = m_lLeadAgentCnt
            End If

            'developer guide no. 50
            objfrmRenewal.RunMode = m_lRunMode
            objfrmRenewal.ShowDialog()


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            If Not (objfrmFilterRenewal Is Nothing) Then
                objfrmFilterRenewal = Nothing

                objfrmRenewal = Nothing
            End If

        End Try

        'developer guide no. 50


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: StartNavProcess
    '
    ' Description: Start Nav process which can't be reached from WM.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (StartNavProcess) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function StartNavProcess(ByVal v_sProcessCode As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim vKeyArray As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get a new instance
    'm_oNavStart = New iPMNavStart.Interface_Renamed()
    '
    ' Set the process code
    'm_oNavStart.ProcessCode = v_sProcessCode
    '
    ' Dont exit the function yet
    'm_bNavClosed = False
    '
    'm_lReturn = CType(m_oNavStart, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'm_oNavStart = Nothing
    'Return result
    'End If
    '
    ' Pass InsuranceFileCnt
    ''ReDim vKeyArray(1, 0)

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt
    '
    'm_lReturn = m_oNavStart.SetKeys(vKeyArray:=vKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'm_oNavStart = Nothing
    'Return result
    'End If
    '
    ' Start the component
    'm_lReturn = m_oNavStart.Start()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'm_oNavStart = Nothing
    'Return result
    'End If
    '
    ' Wait for the process to finish
    'While (Not m_bNavClosed)
    'Application.DoEvents()
    'End While
    '
    ' Back to normal...
    ' The return status of Navigator is checked in m_oNavStart_SetProcessStatus
    '
    ' Remove the instance
    'm_lReturn = m_oNavStart.Terminate()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_oNavStart = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start New Process", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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


    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose
        m_bNavClosed = True
    End Sub

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' PW190802 - allow to suppress sub agents
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef vAddress1 As String = "", Optional ByRef bSuppressSubAgents As Boolean = False, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef bIsInTransferMode As Boolean = False, Optional ByRef bSupressCancelledAgents As Boolean = False) As Integer 'CT 19/07/00 added vResolvedName parameter


        Dim result As Integer = 0
        'developer guide no. 108 
        Dim oFindParty As Object
        Dim vKeyArray(,) As Object
        Dim lLower, lUpper As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 108
        oFindParty = CreateLateBoundObject("iPMBFindParty.Interface_Renamed")

        'developer guide no. 9
        m_lReturn = oFindParty.Initialise()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oFindParty.Dispose()
            oFindParty = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oFindParty.CallingAppName = ACApp
        'SD 31/07/2002
        m_lReturn = CType(oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=m_sTransactionType, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oFindParty.Dispose()
            oFindParty = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set appropriate key if agent only


        If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

            ReDim vKeyArray(1, 0)

            vKeyArray(0, 0) = "special_party"

            vKeyArray(1, 0) = vSpecialParty

            m_lReturn = CType(oFindParty.SetKeys(vKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (vSpecialParty = "AG") Or (vSpecialParty = "UB") Or (vSpecialParty = "AH") Then
                oFindParty.NotEditable = 1
            End If

            ' PW190802 - suppress sub agents if applicable
            oFindParty.SuppressSubAgents = bSuppressSubAgents
            oFindParty.IsInTransferMode = bIsInTransferMode
            oFindParty.SuppressCancelledAgents = bSupressCancelledAgents
        End If

        m_lReturn = CType(oFindParty.Start(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oFindParty.Dispose()
            oFindParty = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

            vPartyCnt = oFindParty.PartyCnt
            vShortName = oFindParty.ShortName

            'MSB 03/03/2003


            vDateCancelled = oFindParty.DateCancelled


            If Not Information.IsNothing(vName) Then
                vName = oFindParty.LongName
            End If
            'TN20000823 - fix CT

            If Not Information.IsNothing(vResolvedName) Then
                vResolvedName = oFindParty.ResolvedName 'CT 19/07/00
            End If

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            ' Return address line1 if requested

            If Not Information.IsNothing(vAddress1) Then
                ' Get the key array (only place it's stored)
                m_lReturn = CType(oFindParty.GetKeys(vKeyArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Walk the array to find the value

                lLower = vKeyArray.GetLowerBound(1)

                lUpper = vKeyArray.GetUpperBound(1)
                For lCount As Integer = lLower To lUpper

                    If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lCount)) = PMNavKeyConst.PMKeyNameAddLine1 Then

                        vAddress1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount))
                        Exit For
                    End If
                Next
            End If
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)
        Else
            result = oFindParty.Status
        End If

        oFindParty.Dispose()

        oFindParty = Nothing
        Return result
    End Function
End Class
