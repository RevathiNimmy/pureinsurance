Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmMain
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmMain"

    Private m_lReturn As Integer
    Private m_lError As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_vParties(,) As Object
    ' Instance of bSIRParty
    Private m_oBusiness As Object

    ' Status property
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' Array of party details
    Public ReadOnly Property PartyArray() As Object
        Get
            Return VB6.CopyArray(m_vParties)
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: DupeCheck
    '
    ' Description:
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DupeCheck(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            ' Going to use PMNotFound here, makes more sense
            result = gPMConstants.PMEReturnCode.PMNotFound

            ' Any array? Exit if not
            If Not Information.IsArray(m_vParties) Then
                Return gPMConstants.PMEReturnCode.PMEOF
            End If

            ' Check the array if party_cnt is already associated
            For iLoop1 As Integer = m_vParties.GetLowerBound(1) To m_vParties.GetUpperBound(1)
                ' Any match?
                If CDbl(m_vParties(0, iLoop1)) = v_lPartyCnt Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Pop up a friendly message on the screen
                    MessageBox.Show("This party is already queued to be converted.", "Add Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DupeCheck Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DupeCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DataToInterface
    '
    ' Description: Refreshes the list
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sKey, sText As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Zap the list
            lvwParties.Items.Clear()

            ' Any data to display?
            If Not Information.IsArray(m_vParties) Then
                Return result
            End If

            ' Loop round the array
            For iLoop1 As Integer = m_vParties.GetLowerBound(1) To m_vParties.GetUpperBound(1)

                ' Rando-key action
                sKey = "K" & CStr(m_vParties(ACArrayPartyCnt, iLoop1))
                ' Select the text
                sText = CStr(m_vParties(ACArrayShortName, iLoop1))
                ' Aiiiiiiiiiiiight

                lstItem = lvwParties.Items.Add(sKey, sText, "folder")
                ' Set the sub-items
                ListViewHelper.GetListViewSubItem(lstItem, 1).Text = CStr(m_vParties(ACArrayResolvedName, iLoop1))
                ' Select which party type it is
                ListViewHelper.GetListViewSubItem(lstItem, 2).Text = CStr(m_vParties(ACArrayPartyType, iLoop1))
                ' And the new type
                ListViewHelper.GetListViewSubItem(lstItem, 3).Text = CStr(m_vParties(ACArrayNewPartyType, iLoop1))

            Next iLoop1

            ' Auto size the list

            'developer guide no. 178
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwParties)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeyValue
    '
    ' Description:
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '

    'Private Function GetKeyValue(ByVal v_vKeyArray( ,  ) As Object, ByVal v_sKeyName As String, ByRef r_vValue As String) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Actually any keys?
    'If Not Information.IsArray(v_vKeyArray) Then
    'Return result
    'End If
    '
    'For 'iLoop1 As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)

    'If CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = v_sKeyName Then

    'r_vValue = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
    'End If
    'Next iLoop1
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeyValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetConversionType
    '
    ' Description:
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetConversionType(ByVal iIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' In future, it might be asked what sort of party to convert to
            ' For now we just swap PC and CC

            'ISS1118 slect on code rather than text description

            Select Case m_vParties(ACArrayPartyTypeCode, iIndex)
                Case SIRConst.SIRPartyTypePersonalClientCode
                    m_vParties(ACArrayNewPartyType, iIndex) = SIRConst.SIRPartyTypeCorporateClientText

                Case SIRConst.SIRPartyTypeCorporateClientCode
                    m_vParties(ACArrayNewPartyType, iIndex) = SIRConst.SIRPartyTypePersonalClientText

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("This sort of party is not supported at present : " & CStr(m_vParties(ACArrayPartyType, iIndex)), "Party Type", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetConversionType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetConversionType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddParty
    '
    ' Description: Adds a party
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function AddParty() As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object

        ' Reason for getting it every time: The thing only seems to work once

        ' Instance of Find Party

        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray As Object
        Dim iIndex As Integer
        Dim vPartyType As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Busy busy busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Use object manager to get a new instance of find party
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            ' Busy busy busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFindParty.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Start it

            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start iPMBFindParty.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Check the status of it

            If oFindParty.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' Check not already in array
                If Information.IsArray(m_vParties) Then

                    m_lReturn = DupeCheck(oFindParty.PartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        Return result
                    End If
                End If

                ' Stash it in the array
                If Not Information.IsArray(m_vParties) Then
                    iIndex = 0
                    ReDim m_vParties(ACArraySize, iIndex)
                Else
                    ' Get the upper limit and add one
                    iIndex = m_vParties.GetUpperBound(1) + 1
                    ' Resize the array
                    ReDim Preserve m_vParties(ACArraySize, iIndex)
                End If

                ' Store the details

                m_vParties(ACArrayPartyCnt, iIndex) = oFindParty.PartyCnt

                m_vParties(ACArrayShortName, iIndex) = oFindParty.Shortname

                m_vParties(ACArrayResolvedName, iIndex) = oFindParty.LongName

                m_vParties(ACArrayPartyTypeCode, iIndex) = oFindParty.PartyType

                'ISS1118 Determine the Party Type text
                Select Case m_vParties(ACArrayPartyTypeCode, iIndex)
                    Case SIRConst.SIRPartyTypePersonalClientCode
                        m_vParties(ACArrayPartyType, iIndex) = SIRConst.SIRPartyTypePersonalClientText
                    Case SIRConst.SIRPartyTypeAgentCode
                        m_vParties(ACArrayPartyType, iIndex) = SIRConst.SIRPartyTypeAgentText
                    Case SIRConst.SIRPartyTypeCorporateClientCode
                        m_vParties(ACArrayPartyType, iIndex) = SIRConst.SIRPartyTypeCorporateClientText
                    Case SIRConst.SIRPartyTypeGroupClientCode
                        m_vParties(ACArrayPartyType, iIndex) = SIRConst.SIRPartyTypeGroupClientText
                End Select


                ' Get the new party type
                m_lReturn = GetConversionType(iIndex:=iIndex)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Remove that last entry
                    If iIndex > 0 Then
                        ReDim Preserve m_vParties(ACArraySize, iIndex - 1)
                    Else
                        m_vParties = Nothing
                    End If
                    GoTo Err_Clearup
                End If

                ' Refresh the list
                m_lReturn = DataToInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo Err_Clearup
                End If

            End If

Err_Clearup:

            ' Terminate and remove it

            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result
        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = AddParty()

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetInterfaceDefaults
    '
    ' Description:
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set full row selekt0r on list view
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwParties.Handle.ToInt32(), v_vShowRowSelect:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RemoveItem
    '
    ' Description:
    '
    ' History: 18/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RemoveItem() As Integer

        Dim result As Integer = 0
        Dim lPartyCnt As Integer
        Dim sKey As String = ""
        Dim vNewArray(,) As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell the user to select a party if they click Remove and one isnt
            ' already selected.
            If lvwParties.FocusedItem Is Nothing Then
                MessageBox.Show("Please select a party to remove before clicking Remove", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            ' Get the party_cnt
            sKey = lvwParties.FocusedItem.Name
            lPartyCnt = CInt(sKey.Substring(sKey.Length - (sKey.Length - 1)))

            ' Do we have more than one party?
            If m_vParties.GetUpperBound(1) > 0 Then

                ' Yes, so..
                ' Make new array one less than old
                ReDim vNewArray(ACArraySize, m_vParties.GetUpperBound(1) - 1)

                iIndex = m_vParties.GetLowerBound(1)

                For iLoop1 As Integer = m_vParties.GetLowerBound(1) To m_vParties.GetUpperBound(1)

                    ' Need to copy this row or not?
                    If CDbl(m_vParties(ACArrayPartyCnt, iLoop1)) <> lPartyCnt Then


                        vNewArray(ACArrayPartyCnt, iIndex) = m_vParties(ACArrayPartyCnt, iLoop1)

                        vNewArray(ACArrayShortName, iIndex) = m_vParties(ACArrayShortName, iLoop1)

                        vNewArray(ACArrayResolvedName, iIndex) = m_vParties(ACArrayResolvedName, iLoop1)

                        vNewArray(ACArrayPartyType, iIndex) = m_vParties(ACArrayPartyType, iLoop1)

                        vNewArray(ACArrayNewPartyType, iIndex) = m_vParties(ACArrayNewPartyType, iLoop1)

                        iIndex += 1

                    End If

                Next iLoop1

                ' Re-assign the array

                m_vParties = vNewArray

            Else

                ' Just one, so to remove it, we nuke the array
                m_vParties = Nothing

            End If

            ' Refresh the list
            m_lReturn = DataToInterface()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EditParty
    '
    ' Description:
    '
    ' History: 22/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function EditParty(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String) As Integer

        Dim result As Integer = 0
        Dim oParty As Object
        Dim sClassName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' What type of interface do we want?

            Select Case v_sPartyType
                Case SIRConst.SIRPartyTypePersonalClientText

                    'developer guide no. 108
                    sClassName = "iPMBPartyPC.Interface_Renamed"
                Case SIRConst.SIRPartyTypeCorporateClientText
                    'developer guide no. 108
                    sClassName = "iPMBPartyCC.Interface_Renamed"

                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            ' Create the instance of the interface

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oParty, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="EditParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            oParty.CallingAppName = ACApp
            ' Set the task

            m_lReturn = oParty.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            ' Set the party count

            oParty.PartyCnt = v_lPartyCnt

            ' Start the interface

            m_lReturn = oParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove the instance

            oParty.Dispose()

            oParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ConvertParties
    '
    ' Description: Converts all the parties in the queue
    '
    ' History: 21/08/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ConvertParties() As Integer

        Dim result As Integer = 0
        Dim lPartyCnt, lPartyTypeOldID, lPartyTypeNewID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have actually some parties to convert
            If Not Information.IsArray(m_vParties) Then
                MessageBox.Show("You need to add some parties to convert first." & Environment.NewLine & _
                                "Click the Add button.", "Convert", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            For iLoop1 As Integer = m_vParties.GetLowerBound(1) To m_vParties.GetUpperBound(1)

                lPartyCnt = CInt(m_vParties(0, iLoop1))

                ' Get the original party type

                Select Case m_vParties(3, iLoop1)
                    Case SIRConst.SIRPartyTypePersonalClientText
                        lPartyTypeOldID = 1

                    Case SIRConst.SIRPartyTypeCorporateClientText
                        lPartyTypeOldID = 4

                End Select

                ' Get the new party type

                Select Case m_vParties(4, iLoop1)
                    Case SIRConst.SIRPartyTypePersonalClientText
                        lPartyTypeNewID = 1

                    Case SIRConst.SIRPartyTypeCorporateClientText
                        lPartyTypeNewID = 4

                End Select

                ' Set the mouse pointer to busy
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                ' Pass the data into the business to do the trickery

                m_lReturn = m_oBusiness.ConvertParty(v_lPartyCnt:=lPartyCnt, v_lPartyTypeOldID:=lPartyTypeOldID, v_lPartyTypeNewID:=lPartyTypeNewID, v_sPartyTypeOld:=m_vParties(3, iLoop1), v_sPartyTypeNew:=m_vParties(4, iLoop1))

                ' Revert the mouse pointer to default
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to convert parties.", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result

                End If

                ' Editing the party
                m_lReturn = EditParty(v_lPartyCnt:=lPartyCnt, v_sPartyType:=CStr(m_vParties(4, iLoop1)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1

            ' Exit out as theyre all ok
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Me.Hide()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertParties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertParties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdConvert_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConvert.Click

        Dim sPrompt As String = "Any custom party data will be deleted." & Strings.Chr(13) & Strings.Chr(10)
        sPrompt = sPrompt & "Do you wish to continue?"

        If MessageBox.Show(sPrompt, Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) = System.Windows.Forms.DialogResult.Yes Then
            m_lReturn = ConvertParties()
        End If
    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        m_lReturn = RemoveItem()

    End Sub

    Private Sub Form_Initialize_Renamed()

        Try

            ' Get an instance of bSIRParty.Business
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lError = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="From_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = SetInterfaceDefaults()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="From_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Remove the instance of the business object

        m_oBusiness.Dispose()

        m_oBusiness = Nothing

        eventArgs.Cancel = Cancel <> 0
    End Sub
End Class
