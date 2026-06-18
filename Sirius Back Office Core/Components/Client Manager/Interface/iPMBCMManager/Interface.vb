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
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' *****************************************************************************
    '
    ' Name: Interface
    '
    ' Description: Acts as a wrapper between ClientManagerWrapper and ClientManager.
    '              Used to manage instances of ClientManager.
    '
    ' *****************************************************************************


    Private Const ACClass As String = "Interface"

    ' Private variables
    Private m_sPartyResolvedName As String = ""
    Private m_lPartyCnt As Integer
    Private m_sPartyShortName As String = ""
    Private m_sPartyType As String = ""
    'MSS040701
    Private m_lInsuranceFileCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lInsuranceFolderCnt As Integer

    Private m_lReturn As Integer

    Private m_sCallingAppName As String = ""

    ' Max number of client managers
    Private m_lMaxCMs As Integer

    ' CTAF 150801 - FromCopy
    Private m_bFromCopy As Boolean
    'sj 03/07/2002 - start
    ' RestrictInsurerAccess
    Private m_bRestrictInsurerAccess As Boolean
    Private m_lUserInsurerCnt As Integer
    'sj 03/07/2002 - end

    'Thinh Nguyen 14/01/2004 - option to show policy list
    Private m_lShowPolicyList As gPMConstants.PMEReturnCode
    'JT  PN-13238 29-10-2004
    Private m_bIsIncludeClosedBranchChecked As Boolean
    Public WriteOnly Property ShowPolicyList() As Integer
        Set(ByVal Value As Integer)
            m_lShowPolicyList = Value
        End Set
    End Property

    'sj 03/07/2002 - start
    Public WriteOnly Property RestrictInsurerAccess() As Boolean
        Set(ByVal Value As Boolean)
            m_bRestrictInsurerAccess = Value
        End Set
    End Property
    Public WriteOnly Property UserInsurerCnt() As Integer
        Set(ByVal Value As Integer)
            m_lUserInsurerCnt = Value
        End Set
    End Property
    'sj 03/07/2002 - end

    Public Property FromCopy() As Boolean
        Get
            Return m_bFromCopy
        End Get
        Set(ByVal Value As Boolean)
            m_bFromCopy = Value
        End Set
    End Property

    ' *************** PUBLIC PROPERTIES (BEGIN)************************
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PartyResolvedName() As String
        Get
            Return m_sPartyResolvedName
        End Get
        Set(ByVal Value As String)
            m_sPartyResolvedName = Value
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

    Public Property PartyShortName() As String
        Get
            Return m_sPartyShortName
        End Get
        Set(ByVal Value As String)
            m_sPartyShortName = Value
        End Set
    End Property

    Public Property PartyType() As String
        Get
            Return m_sPartyType
        End Get
        Set(ByVal Value As String)
            m_sPartyType = Value
        End Set
    End Property

    'MSS040701

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property


    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    'MSS040701

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property
    'JT PN-13238 To hold that whether the CheckBox of Include Closed Branch
    'was checked or not in FindParty 29-10-2004
    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIsIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIncludeClosedBranchChecked = Value
        End Set
    End Property

    ' *************** PUBLIC PROPERTIES (END)**************************

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Static bInitialised As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bInitialised Then
                Return result
            End If

            bInitialised = True

            ' Get an instance of object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Initialise it
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
    ' Description: Standard Terminate function.
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
                For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)
                    m_vCMArray(ACArrayObject, iLoop1) = Nothing
                Next iLoop1
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: CheckRunning
    '
    ' Description: Searchs the array and tries to find a client manager
    '              that's got the required party_Cnt open, and that's
    '              got a status of live.
    '
    ' ***************************************************************** '
    Private Function CheckRunning(ByRef r_bRunning As Boolean, ByRef r_oClientManager As Object) As Integer

        Dim result As Integer = 0
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bRunning = False

            ' Make sure we return an empty client manager if no match is found
            r_oClientManager = Nothing


            For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)


                If (CDbl(m_vCMArray(ACArrayPartyCnt, iLoop1)) = m_lPartyCnt) And (CDbl(m_vCMArray(ACArrayStatus, iLoop1)) = ACStatusLive) Then
                    r_oClientManager = m_vCMArray(ACArrayObject, iLoop1)
                End If
            Next iLoop1

            ' This could be removed...
            r_bRunning = Not (r_oClientManager Is Nothing)

            Return result

        Catch excep As System.Exception



            ' Error 5 occurs if the key isnt found in the collection
            ' Errors with Invalid Method or Procedure for some reason...
            If Information.Err().Number = 5 Then

                ' Key not in the collection
                r_bRunning = False

                Return result

            Else

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRunning Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRunning", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindEmpty
    '
    ' Description: Finds the first available empty client manager.
    '
    ' ***************************************************************** '
    Private Function FindEmpty(ByRef r_oClientManager As Object, ByRef r_iIndex As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Loop through the array

        For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)

            If CDbl(m_vCMArray(ACArrayStatus, iLoop1)) = ACStatusEmpty Then
                ' Set an instance of the object if its there
                r_oClientManager = m_vCMArray(ACArrayObject, iLoop1)
                r_iIndex = iLoop1
                Exit For
            End If
        Next iLoop1

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLiveCM
    '
    ' Description: Gets the number of live client managers.
    '
    ' ***************************************************************** '
    Private Function GetLiveCM(ByRef r_iCount As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        r_iCount = 0


        For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)
            ' if its live, then increase the count

            If CDbl(m_vCMArray(ACArrayStatus, iLoop1)) = ACStatusLive Then
                r_iCount += 1
            End If
        Next iLoop1

        Return result

    End Function
    Private Delegate Sub DlgCloseForm()


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Entry point for the program.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim oClientManager As Object
        Dim bRunning As Boolean
        Dim sMsg, sSettingValue As String
        Dim iIndex, iCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Debug or not debug...
            'frmDebug.Show

            ' Make sure the arrays ready to use.
            If Not Information.IsArray(m_vCMArray) Then
                ReDim m_vCMArray(2, 0)
            Else
                ' Expland the array for one more entry

                ReDim Preserve m_vCMArray(2, m_vCMArray.GetUpperBound(1) + 1)
            End If

            ' Check if we have an instance going already
            m_lReturn = CheckRunning(r_bRunning:=bRunning, r_oClientManager:=oClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we have then switch to that
            If bRunning Then

                ' Shrink the array

                ReDim Preserve m_vCMArray(2, m_vCMArray.GetUpperBound(1) - 1)


                m_lReturn = oClientManager.SwitchTo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' Not currently running, lets try and find an empty client manager
                m_lReturn = FindEmpty(r_oClientManager:=oClientManager, r_iIndex:=iIndex)


                If Not (oClientManager Is Nothing) Then
                    oClientManager.MDIForm.Invoke(New DlgCloseForm(AddressOf oClientManager.MDIForm.Close))
                    oClientManager.Dispose()
                    oClientManager = Nothing
                End If


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not (oClientManager Is Nothing) Then

                    ' We have a winner. Switch to this one instead.

                    oClientManager.PartyCnt = PartyCnt

                    oClientManager.PartyShortName = PartyShortName

                    oClientManager.PartyResolvedName = PartyResolvedName

                    oClientManager.PartyType = PartyType
                    'sj 03/07/2002 - start

                    oClientManager.RestrictInsurerAccess = m_bRestrictInsurerAccess

                    oClientManager.UserInsurerCnt = m_lUserInsurerCnt
                    'sj 03/07/2002 - end

                    'JT 29-10-2004 Whether CheckBox in FindParty is Chked or not

                    oClientManager.IsIncludeClosedBranchChecked = m_bIsIncludeClosedBranchChecked


                    m_vCMArray(ACArrayPartyCnt, iIndex) = PartyCnt

                    m_vCMArray(ACArrayStatus, iIndex) = ACStatusLive

                    ' Shrink the array

                    ReDim Preserve m_vCMArray(2, m_vCMArray.GetUpperBound(1) - 1)

                    ' Start it, so it loads the data

                    m_lReturn = oClientManager.Start()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Switch to it

                    m_lReturn = oClientManager.SwitchTo()

                    'MSS040701
                    If InsuranceFileCnt > 0 Then

                        If m_bFromCopy Then


                            m_lReturn = oClientManager.ShowPolicyEdit(v_lPartyCnt:=PartyCnt, v_sPartyType:=PartyType, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=InsuranceFileCnt, v_sShortName:=PartyShortName, v_sInsReference:=InsuranceRef, v_lPolicyTypeId:=0, v_bCopiedPolicy:=True)
                        Else


                            m_lReturn = oClientManager.ShowPolicy(v_lPartyCnt:=PartyCnt, v_sPartyType:=PartyType, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=InsuranceFileCnt, v_sShortName:=PartyShortName, v_sInsReference:=InsuranceRef, v_lPolicyTypeId:=0)
                        End If

                    End If

                    'Thinh Nguyen 14/01/2004 - show policy list
                    If m_lShowPolicyList = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = oClientManager.ShowPolicyList(v_lPartyCnt:=m_lPartyCnt, v_sPartyShortName:=m_sPartyShortName)
                    End If
                Else

                    ' Read the Maximum CMs from the registry
                    m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACRegSettingMaximumCM, r_sSettingValue:=sSettingValue)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sSettingValue.Trim() = "") Then
                        ' Instead of causing a fuss, we'll just default
                        ' to a value
                        sSettingValue = "2"
                    Else
                        m_lMaxCMs = CInt(sSettingValue)
                    End If

                    ' Get the number of active/live client managers
                    m_lReturn = GetLiveCM(r_iCount:=iCount)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Make sure we haven't exceed the limit
                    If iCount < m_lMaxCMs Then

                        ' Create a new instance of the client manager
                        oClientManager = CreateLateBoundObject("iPMBClientManager.Interface_Renamed")

                        oClientManager.InitaliseModuleClass()

                        ' Set the properties

                        oClientManager.PartyCnt = PartyCnt


                        oClientManager.PartyShortName = PartyShortName

                        oClientManager.PartyResolvedName = PartyResolvedName

                        oClientManager.PartyType = PartyType
                        'sj 03/07/2002 - start

                        oClientManager.RestrictInsurerAccess = m_bRestrictInsurerAccess

                        oClientManager.UserInsurerCnt = m_lUserInsurerCnt
                        'sj 03/07/2002 - end

                        'JT 29-10-2004 Whether CheckBox in FindParty is Chked or not

                        oClientManager.IsIncludeClosedBranchChecked = m_bIsIncludeClosedBranchChecked

                        ' initialise

                        m_lReturn = oClientManager.Initialise(r_oCMManager:=Me)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' and start it

                        m_lReturn = oClientManager.Start()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        iIndex = m_vCMArray.GetUpperBound(1)

                        m_vCMArray(ACArrayObject, iIndex) = oClientManager

                        m_vCMArray(ACArrayPartyCnt, iIndex) = m_lPartyCnt

                        m_vCMArray(ACArrayStatus, iIndex) = ACStatusLive

                        'MSS040701
                        If InsuranceFileCnt > 0 Then

                            ' CTAF 150801
                            If m_bFromCopy Then


                                m_lReturn = oClientManager.ShowPolicyEdit(v_lPartyCnt:=PartyCnt, v_sPartyType:=PartyType, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=InsuranceFileCnt, v_sShortName:=PartyShortName, v_sInsReference:=InsuranceRef, v_lPolicyTypeId:=0, v_bCopiedPolicy:=True)
                            Else


                                m_lReturn = oClientManager.ShowPolicy(v_lPartyCnt:=PartyCnt, v_sPartyType:=PartyType, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=InsuranceFileCnt, v_sShortName:=PartyShortName, v_sInsReference:=InsuranceRef, v_lPolicyTypeId:=0)
                            End If

                        End If

                        'Thinh Nguyen 14/01/2004 - show policy list
                        If m_lShowPolicyList = gPMConstants.PMEReturnCode.PMTrue Then

                            m_lReturn = oClientManager.ShowPolicyList(v_lPartyCnt:=m_lPartyCnt, v_sPartyShortName:=m_sPartyShortName)
                        End If

                    Else

                        ' Shrink the array

                        ReDim Preserve m_vCMArray(2, m_vCMArray.GetUpperBound(1) - 1)

                        ' Show a message saying the maximum number of client managers
                        ' has been reached
                        sMsg = "You have reached the maximum number of" & Environment.NewLine & _
                               "allowed Client Managers (" & CStr(m_lMaxCMs) & ")."
                        MessageBox.Show(sMsg, "Limit reached", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetIndex
    '
    ' Description: Loops through the array to find the index in the
    '              array for the party_cnt
    '
    ' ***************************************************************** '
    Private Function GetIndex(ByVal v_lPartyCnt As Integer, ByRef r_iIndex As Integer, Optional ByRef v_bLiveOnly As Boolean = False) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        r_iIndex = -1


        ' Set all dead object managers to have party count of -1

        For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)

            If CDbl(m_vCMArray(ACArrayStatus, iLoop1)) = ACStatusDead Then

                m_vCMArray(ACArrayPartyCnt, iLoop1) = -1
            End If
        Next iLoop1


        If v_bLiveOnly Then

            ' Loop through our array until we find the right party

            For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)

                If CDbl(m_vCMArray(ACArrayPartyCnt, iLoop1)) = v_lPartyCnt Then

                    If CDbl(m_vCMArray(ACArrayStatus, iLoop1)) <> ACStatusDead Then
                        r_iIndex = iLoop1
                        Exit For
                    End If
                End If
            Next iLoop1

        Else

            ' Loop through our array until we find the right party

            For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)

                If CDbl(m_vCMArray(ACArrayPartyCnt, iLoop1)) = v_lPartyCnt Then
                    r_iIndex = iLoop1
                    Exit For
                End If
            Next iLoop1

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: TerminateCallback
    '
    ' Description: Called by Client Manager to tell this object that
    '              it's terminating and therefore remove it from the
    '              collection.
    '
    ' ***************************************************************** '
    Public Function TerminateCallback(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer
        Dim oObject As Object
        Dim bTerminate As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the position of the party
            m_lReturn = GetIndex(v_lPartyCnt:=v_lPartyCnt, r_iIndex:=iIndex, v_bLiveOnly:=True)

            ' Set it to dead

            m_vCMArray(ACArrayStatus, iIndex) = ACStatusDead

            If Not (m_vCMArray(ACArrayObject, iIndex) Is Nothing) Then

                oObject = m_vCMArray(ACArrayObject, iIndex)

                'commenting this due to behavioural differences between VB 6.0 and VB.NET
                'if this is called and more than one client manager windows are open
                'it will kill all the shared resources like g_oObjectManager and this will
                'effect the previously opened iPMBClientManager instance.
                'm_lReturn = oObject.Terminate()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Remove instances
                m_vCMArray(ACArrayObject, iIndex) = Nothing

                oObject = Nothing
            End If

            ' Get the number of live CM's
            ' If its 0 then terminate ourself

            bTerminate = True


            For iLoop1 As Integer = m_vCMArray.GetLowerBound(1) To m_vCMArray.GetUpperBound(1)

                If CDbl(m_vCMArray(ACArrayStatus, iLoop1)) <> ACStatusDead Then
                    bTerminate = False
                End If
            Next iLoop1

            If bTerminate Then
                ' Call terminate
                Dispose()
                m_vCMArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TerminateCallback Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateCallback", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ImEmpty
    '
    ' Description: Called by a client manager to say theyre empty and
    '              can be recycled.
    '
    ' ***************************************************************** '
    Public Function ImEmpty(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Find position in array
            m_lReturn = GetIndex(v_lPartyCnt:=v_lPartyCnt, r_iIndex:=iIndex, v_bLiveOnly:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetIndex for PartyCnt : " & v_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="ImEmpty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Update status

            m_vCMArray(ACArrayStatus, iIndex) = ACStatusEmpty

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImEmpty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImEmpty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ChangeParty
    '
    ' Description: Called by a client manager when its changing it's party
    '
    ' ***************************************************************** '
    Public Function ChangeParty(ByVal v_lOldPartyCnt As Integer, ByVal v_lNewPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetIndex(v_lPartyCnt:=v_lOldPartyCnt, r_iIndex:=iIndex)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Change the party count

            m_vCMArray(ACArrayPartyCnt, iIndex) = v_lNewPartyCnt

            ' Its changing, so its active.

            m_vCMArray(ACArrayStatus, iIndex) = ACStatusLive

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsOpen
    '
    ' Description: Checks the array to see if the passed party_cnt is
    '              already open.
    '
    ' ***************************************************************** '
    Public Function IsOpen(ByVal v_lPartyCnt As Integer, ByRef r_bIsOpen As Boolean) As Integer

        Dim result As Integer = 0
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Try and get the index
            m_lReturn = GetIndex(v_lPartyCnt:=v_lPartyCnt, r_iIndex:=iIndex)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' if its -1 then it wasn't found
            If iIndex = -1 Then
                r_bIsOpen = False
            Else
                ' Make sure it is actually there and in the client manager

                r_bIsOpen = (CDbl(m_vCMArray(ACArrayStatus, iIndex)) = ACStatusLive)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsOpen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsOpen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ************************************************************************** '
    ' Name: CheckCMAvailability
    '
    ' Description: Checks whether we can open the CM for the client or not,
    '              i.e. to check whether the limit for live CM exhausted or not.
    '
    ' Added : 24/05/2005 By MKR : PN 20162
    ' ************************************************************************** '
    Public Function CheckCMAvailability(ByRef r_bCanOpen As Boolean, ByRef r_lMaxCMs As Integer) As Integer

        Dim result As Integer = 0
        Dim iCount As Integer
        Dim sSettingValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read the Maximum CMs from the registry
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACRegSettingMaximumCM, r_sSettingValue:=sSettingValue)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sSettingValue.Trim() = "") Then
                ' Instead of causing a fuss, we'll just default
                ' to a value
                sSettingValue = "2"
            Else
                m_lMaxCMs = CInt(sSettingValue)
            End If

            ' Try and get the no of Live CM
            m_lReturn = GetLiveCM(r_iCount:=iCount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lMaxCMs = m_lMaxCMs

            r_bCanOpen = (iCount < m_lMaxCMs)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCMAvailability Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCMAvailability", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
