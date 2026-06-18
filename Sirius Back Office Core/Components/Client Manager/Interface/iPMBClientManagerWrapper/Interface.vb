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
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/05/1999
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

    Private m_lPMAuthorityLevel As Integer

    Private m_sStepStatus As String = ""

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_vKeys As Object

    ' Information passed from Find to Client Manager
    Private m_sResolvedName As String = ""
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sType As String = ""
    ' To hold Whether Include Closed BRanch in FindParty is Chekd or not
    Private m_bIsincludeClosedBranchChecked As Boolean
    'sj 03/07/2002 - start
    Private m_bRestrictInsurerAccess As Boolean
    Private m_lUserInsurerCnt As Integer
    'sj 03/07/2002 - end
    ' pk
    Private m_lInsuranceFileCnt As Integer

    Private m_bSkipFindParty As Boolean

    Public WriteOnly Property ShortName() As String
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property



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

    ' Pk
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    ' Pk end


    Public Property SkipFindParty() As Boolean
        Get
            Return m_bSkipFindParty
        End Get
        Set(ByVal Value As Boolean)
            m_bSkipFindParty = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Initialise
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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

            ' R. Griffiths 2006-10-13


            m_vKeys = vKeyArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", excep:=excep)

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

            'R. Griffiths 2006-10-13


            vKeyArray = m_vKeys


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", excep:=excep)

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
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

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
        'developer guide no. 108
        Const sClassName As String = "iPMBFindParty.Interface_Renamed"

        Dim oFindParty As Object
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the instance of find party here, as we only really need it
            ' in this procedure

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oFindParty, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", excep:=New Exception(Information.Err().Description))
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' R. Griffiths 2006-10-13
            ' Set KeyArray

            If Not Object.Equals(m_vKeys, Nothing) Then

                m_lReturn = oFindParty.SetKeys(vKeyArray:=m_vKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set keys for FindParty.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", excep:=New Exception(Information.Err().Description))
                    Return m_lReturn
                End If
            End If


            ' Set the Specific Properties

            oFindParty.SkipFindParty = m_bSkipFindParty
            If m_sShortName.Trim().Length <> 0 Then

                oFindParty.ShortName = m_sShortName
            End If


            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get start FindParty.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", excep:=New Exception(Information.Err().Description))
                Return m_lReturn
            End If


            If oFindParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                m_lReturn = oFindParty.GetKeys(vKeyArray:=vKeyArray)


                m_lPartyCnt = CInt(vKeyArray(1, 0))

                m_sShortName = CStr(vKeyArray(1, 1))

                m_sResolvedName = CStr(vKeyArray(1, 2))

                m_sType = CStr(vKeyArray(1, 22))

                m_bIsincludeClosedBranchChecked = CBool(vKeyArray(1, 23))

            Else

                result = gPMConstants.PMEReturnCode.PMCancel

            End If

            'sj 03/07/2002 - start

            m_bRestrictInsurerAccess = oFindParty.RestrictInsurerAccess

            m_lUserInsurerCnt = oFindParty.UserInsurerCnt
            'sj 03/07/2002 - end

            ' Remove the instance of the object

            oFindParty.Dispose()
            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", excep:=excep)

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
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read from registry setting :" & Environment.NewLine &  _
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
    'vPartyType = m_sType.Substring(0, 1)
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
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClientManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClientManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    '
    ' ***************************************************************** '
    Private Function ShowClientManager() As Integer

        Dim result As Integer = 0
        Dim oCMManager As iPMBCMManager.Interface_Renamed
        Dim sType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'developer guide no. 109
            oCMManager = New iPMBCMManager.Interface_Renamed

            m_lReturn = CType(oCMManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not String.IsNullOrEmpty(m_sType) Then
                sType = m_sType.Substring(0, 1)
            End If

            ' Set the properties
            'sj 03/07/2002 - start
            oCMManager.RestrictInsurerAccess = m_bRestrictInsurerAccess
            oCMManager.UserInsurerCnt = m_lUserInsurerCnt
            'sj 03/07/2002 - end
            oCMManager.PartyCnt = m_lPartyCnt
            oCMManager.PartyShortName = m_sShortName
            oCMManager.PartyResolvedName = m_sResolvedName
            oCMManager.PartyType = sType
            oCMManager.IsIncludeClosedBranchChecked = m_bIsincludeClosedBranchChecked

            'pk
            If m_lInsuranceFileCnt <> 0 Then
                oCMManager.InsuranceFileCnt = m_lInsuranceFileCnt
                oCMManager.ShowPolicyList = gPMConstants.PMEReturnCode.PMTrue
            End If
            'end pk

            ' Launch it
            m_lReturn = oCMManager.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Client Manager Manager.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", excep:=New Exception(Information.Err().Description))

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
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowClientManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", excep:=excep)

            End If

            Return result


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

            Dim oFindParty As Object
            Const sClassName As String = "bSIRFindParty.Business"
            Const ACMaxSearchDetails As Integer = 500
            Dim vSearchData As Object

            result = gPMConstants.PMEReturnCode.PMTrue
            'Pk
            If m_lInsuranceFileCnt = 0 Then
                ' Call Find Party
                m_lReturn = CType(FindParty(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    Return result
                End If

            Else
                m_bRestrictInsurerAccess = True
                m_bIsincludeClosedBranchChecked = False
                m_lReturn = g_oObjectManager.GetInstance(oObject:=oFindParty, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalBusiness)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=New Exception(Information.Err().Description))
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oFindParty.RestrictInsuranceAccess = True


                m_lReturn = oFindParty.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=vSearchData, v_vInsuranceFileCnt:=m_lInsuranceFileCnt)
                ' Log Error Message
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start FindParty.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=New Exception(Information.Err().Description))
                    Return m_lReturn
                End If

                If Information.IsArray(vSearchData) Then

                    m_lPartyCnt = CInt(vSearchData(0, 0))

                    m_sShortName = CStr(vSearchData(2, 0)).Trim()

                    m_sResolvedName = CStr(vSearchData(3, 0)).Trim()

                    m_sType = gPMFunctions.ToSafeString(CStr(vSearchData(22, 0))).Trim()
                    m_lUserInsurerCnt = 0
                End If
            End If

            ' Client Manager
            m_lReturn = CType(ShowClientManager(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)

            Return result

        End Try
    End Function
End Class