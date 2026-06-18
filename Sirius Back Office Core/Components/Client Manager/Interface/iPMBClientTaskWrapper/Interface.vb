Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' CJB 260105 - Interface.ShowClientManager changed to process keyarray values
    '              in a loop rather than being hardcoded positions which didn't work.
    '              Needed to do this for Perkins Slade Retail Logic card processing
    '              integration development (renewal card processing errors will
    '              generate WM tasks).
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'developer guide no. 107 (Guide)
    Public g_oObjectManager As bObjectManager.ObjectManager
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

    Private m_lPMAuthorityLevel As Integer

    Private m_sStepStatus As String = ""

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_vKeys(,) As Object

    ' Information passed from Find to Client Manager
    Private m_sResolvedName As String = ""
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_iType As Integer
    'JT PN-13238
    Private m_bIsIncludeClosedBranchChecked As Boolean

    ' PRIVATE Data Members (End)

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

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Initialise
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description:
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
                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(vKeyArray) Then
                m_vKeys = vKeyArray
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try




            Return gPMConstants.PMEReturnCode.PMTrue

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

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindParty
    '
    ' Description: Calls the find party component.
    '
    ' ***************************************************************** '
    Public Function FindParty() As Integer

        Dim result As Integer = 0
        'developer guide no. 108 (Guide)
        Const sClassName As String = "iPMBFindParty.Interface_Renamed"

        Dim oFindParty, vKeyArray As Object

        Dim m_bViewAuthority As Boolean '2005 Client manager Security
        Dim m_bEditAuthority As Boolean '2005 Client manager Security
        Dim m_bDeleteAuthority As Boolean '2005 Client Manager Security
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bViewAuthority = True '2005 Client manager Security
            m_bEditAuthority = True '2005 Client manager Security
            m_bDeleteAuthority = True '2005 Client Manager Security
            ' Get the instance of find party here, as we only really need it
            ' in this procedure

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oFindParty, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oFindParty.CheckSecurity(r_bViewAuthority:=m_bViewAuthority, r_bEditAuthority:=m_bEditAuthority, r_bDeleteAuthority:=m_bDeleteAuthority)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '2005 Client manager Security
            If Not m_bViewAuthority Then
                MessageBox.Show("User does not have access to client data", "Access Denied", MessageBoxButtons.OK)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return gPMConstants.PMEReturnCode.PMCancel

            End If


            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get start FindParty.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return m_lReturn
            End If


            If oFindParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                m_lReturn = oFindParty.GetKeys(vKeyArray:=vKeyArray)


                m_lPartyCnt = CInt(vKeyArray(1, 0))

                m_sShortName = CStr(vKeyArray(1, 1))

                m_sResolvedName = CStr(vKeyArray(1, 2))

                m_iType = CInt(vKeyArray(1, 8))
                'JT PN-13238

                m_bIsIncludeClosedBranchChecked = CBool(vKeyArray(1, 23))

            Else

                result = gPMConstants.PMEReturnCode.PMCancel

            End If

            ' Remove the instance of the object

            oFindParty.Dispose()
            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ClientManager
    '
    ' Description: Calls ClientManager with the information from
    '              Find Party
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClientManager) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ClientManager() As Integer
    '
    'Dim result As Integer = 0
    'Dim dRetVal As Double
    'Dim sCommand, sPath As String
    'Dim vPartyType As String = ""
    '
    'Const sSettingName As String = "ClientManager.exe"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Registry setting needed ...
    ' HKEY_LocalMachine\Software\PM\SiriusSolutions\Client\ClientManager.exe
    '
    ' Get the path from the registry
    'm_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=sSettingName, r_sSettingValue:=sPath), gPMConstants.PMEReturnCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read from registry setting :" & Environment.NewLine &  _
    '                   "HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Client\" & sSettingName, vApp:=ACApp, vClass:=ACClass, vMethod:="ClientManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Check it exists first
    'If FileSystem.Dir(sPath, FileAttribute.Normal) = "" Then
    'MessageBox.Show(sPath & " does not exist." & Environment.NewLine & "Current Directory : " & Directory.GetCurrentDirectory(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Select Case m_iType
    'Case 1
    'vPartyType = "P"
    'Case 2
    'vPartyType = "C"
    'Case 3
    'vPartyType = "G"
    'End Select
    '
    ' Party Count|Short Name|Resolved Name|Type
    'sCommand = sPath & " " & CStr(m_lPartyCnt) & "|" &  _
    '           m_sShortName & "|" &  _
    '           m_sResolvedName & "|" &  _
    '           vPartyType & "|"
    '
    ' Try and exectute

    'Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sCommand)
    'startInfo.WindowStyle = ProcessWindowStyle.Maximized
    'dRetVal = Process.Start(startInfo).Id
    'If dRetVal = 0 Then
    'MessageBox.Show("Failed to start " & sPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClientManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClientManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ShowClientManager
    '
    ' Description: Either activates an exist client manager, or opens
    '              a new one.
    ' Date = 15/03/05 PN=18080 "client Manger is working differenly
    ' from 1.6 to  1.8"
    ' ***************************************************************** '
    Private Function ShowClientManager() As Integer
        Dim result As Integer = 0


        Dim oCMManager As iPMBCMManager.Interface_Renamed
        Dim sType As String = ""

        Dim oDocBusiness As bSIRDocTemplate.Business
        Dim vArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'developer guide no. 109 (Guide)
            oCMManager = New iPMBCMManager.Interface_Renamed

            m_lReturn = CType(oCMManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Needed if the task was added directly
            ' ie: behave like a blank client because we won't have one
            If Not Information.IsArray(m_vKeys) Then
                oCMManager.InsuranceFolderCnt = 0
                oCMManager.InsuranceFileCnt = 0
                oCMManager.PartyCnt = m_lPartyCnt
                oCMManager.PartyShortName = m_sShortName
                oCMManager.PartyResolvedName = m_sResolvedName
                oCMManager.IsIncludeClosedBranchChecked = m_bIsIncludeClosedBranchChecked
                Select Case m_iType
                    Case 1
                        sType = "P"
                    Case 2
                        sType = "C"
                    Case 3
                        sType = "G"
                End Select

                oCMManager.PartyType = sType

            Else

                ' Changed this to use std keyarray name lookup rather than hardoded positions as previously this
                ' didn't work anyway as you may add them in order but the sp orders them! Done as part of Retail Logic
                ' integration development.

                ' Step through the key array.
                For lRow As Integer = m_vKeys.GetLowerBound(1) To m_vKeys.GetUpperBound(1)

                    ' Assign the parameter member with the correct key array item.

                    Select Case CStr(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                        Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                            oCMManager.InsuranceFileCnt = CInt(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        Case PMNavKeyConst.PMKeyNamePartyCnt
                            oCMManager.PartyCnt = CInt(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        Case PMNavKeyConst.PMKeyNamePartyType
                            oCMManager.PartyType = CStr(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Substring(0, 1)

                        Case PMNavKeyConst.PMKeyNamePartyResolvedName, "resolved_name"
                            oCMManager.PartyResolvedName = CStr(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        Case PMNavKeyConst.PMKeyNameShortName
                            oCMManager.PartyShortName = CStr(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt
                            oCMManager.InsuranceFolderCnt = CInt(m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Case PMNavKeyConst.PMKeyNameRunMode
                            If m_vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow) = "POLICYLIST" Then
                                oCMManager.ShowPolicyList = gPMConstants.PMEReturnCode.PMTrue
                            End If

                    End Select
                Next lRow

                If oCMManager.InsuranceFileCnt > 0 Then

                    Dim temp_oDocBusiness As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oDocBusiness, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oDocBusiness = temp_oDocBusiness


                    m_lReturn = oDocBusiness.GetPolicyRef(r_vArray:=vArray, v_lInsuranceFileCnt:=m_vKeys(1, 0))


                    oCMManager.InsuranceRef = CStr(vArray(0, 0))
                End If

            End If

            ' Launch it
            m_lReturn = oCMManager.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Client Manager Manager.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If

            Return result
        Catch excep As System.Exception



            If Information.Err().Number = 429 Then
                ' Tried to do a get object, but the object hasn't been
                ' created yet. Create it now, then resume.

                oCMManager = New iPMBCMManager.Interface_Renamed()

                ' Initialise it
                m_lReturn = CType(oCMManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                End If




            Else

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowClientManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End If

            Return result

        End Try

    End Function


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Entry point for the program
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' SET 19112002 ISS1311
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If Not Information.IsArray(m_vKeys) Then
                ' Call Find Party
                m_lReturn = CType(FindParty(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lStatus = gPMConstants.PMEReturnCode.PMFail
                    Else
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    End If
                    Return result
                End If
            End If

            ' Client Manager
            m_lReturn = CType(ShowClientManager(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lStatus = gPMConstants.PMEReturnCode.PMFail
                Return result
            End If

            '    m_lReturn = CompleteSchedTask(v_lPMWrkTaskInstanceCnt:=m_vKeys(2, 0))
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Do nothing for the minute. Not very serious
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CompleteSchedTask(ByVal v_lPMWrkTaskInstanceCnt As Integer) As Integer
        Dim result As Integer = 0


        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oBusiness As bPMWrkManager.FormClass
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of business object
            Dim temp_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMWrkManager.FormClass", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            ' Complete the Task

            lReturn = oBusiness.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Complete Task")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompleteSchedTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompleteSchedTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
