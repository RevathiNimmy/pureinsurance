Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PickList_NET.PickList")> _
Partial Public Class PickList
    Inherits System.Windows.Forms.UserControl
    Public Event IsSearchableChange()
    Public Event AvailableCaptionChange()
    Public Event PickListTypeChange()
    Public Event BusinessObjectChange()
    Public Event EnabledChange()

    ' ***************************************************************** '
    ' Class Name: uctPickList
    '
    ' Date: 26/10/2001
    '
    ' Description: Generic Pick List Control
    '
    ' Edit History:
    ' DD26112001 - Created
    ' RAW 10/04/2003 : ENDVR633 : ensure that g_oObjectManager is instantiated and initialised before referencing it
    '                             log an error from within existing error handlers
    ' ***************************************************************** '

    ' BusinessObject
    Private m_sBusinessObject As String = ""
    Private m_sAvailableCaption As String = ""
    Private m_sPickListType As String = ""

    Private _ForeignKeys As Collection = Nothing
    Public Property ForeignKeys() As Collection
        Get
            If _ForeignKeys Is Nothing Then
                _ForeignKeys = New Collection()
            End If
            Return _ForeignKeys
        End Get
        Set(ByVal Value As Collection)
            _ForeignKeys = value
        End Set
    End Property
    Private m_vItemArray(,) As Object
    Private m_bIsSearchable As Boolean
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPickList"
    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (not mentioned in tech spec)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    'Private m_lReturn As Long
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (not mentioned in tech spec)

    Private m_sSearchString As String = ""
    Private m_lReturn As gPMConstants.PMEReturnCode
    '<Pankaj PN:38898>
    'Changes for WPR-42
    'Event Change(ByVal Sender As Object, ByVal e As EventArgs)
    Event Change(ByVal Sender As Object, ByVal e As ChangeEventArgs)
    'End Changes for WPR-42
    Event Find(ByVal Sender As Object, ByVal e As EventArgs)

    '</Pankaj PN:38898>

    <Browsable(False)> _
    Public ReadOnly Property SearchString() As String
        Get
            Return m_sSearchString
        End Get
    End Property


    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal Value As Boolean)

            lvwAll.Enabled = Value
            lvwContents.Enabled = Value

            cmdAddAll.Enabled = Value
            cmdAddOne.Enabled = Value
            cmdRemoveAll.Enabled = Value
            cmdRemoveOne.Enabled = Value

            MyBase.Enabled = Value

            RaiseEvent EnabledChange()

        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property GetSelectedItems() As Object
        Get
            Dim vResultArray As Object
            If lvwContents.Items.Count > 0 Then
                ReDim vResultArray(lvwContents.Items.Count - 1)
                For iRow As Integer = 1 To lvwContents.Items.Count

                    vResultArray(iRow - 1) = Mid(lvwContents.Items.Item(iRow - 1).Name, 2)
                Next iRow
            Else

                vResultArray = Nothing
            End If
            Return vResultArray
        End Get
    End Property

    'Start - Sankar - Bank Guarantee Bug Fixing
    <Browsable(False)> _
    Public ReadOnly Property GetItemDetails() As Object
        Get
            Dim vResultArray As Object
            If lvwContents.Items.Count > 0 Then
                ReDim vResultArray(lvwContents.Items.Count - 1, 1)
                For iRow As Integer = 1 To lvwContents.Items.Count

                    vResultArray(iRow - 1, 0) = Mid(lvwContents.Items.Item(iRow - 1).Name, 2)

                    vResultArray(iRow - 1, 1) = lvwContents.Items.Item(iRow - 1).Text.Trim()
                Next iRow
            Else

                vResultArray = Nothing
            End If
            Return vResultArray
        End Get
    End Property
    'End - Sankar - Bank Guarantee Bug Fixing

    <Browsable(True)> _
    Public Property BusinessObject() As String
        Get
            Return m_sBusinessObject
        End Get
        Set(ByVal Value As String)
            m_sBusinessObject = Value
            RaiseEvent BusinessObjectChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property PickListType() As String
        Get
            Return m_sPickListType
        End Get
        Set(ByVal Value As String)
            m_sPickListType = Value
            RaiseEvent PickListTypeChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property AvailableCaption() As String
        Get
            Return m_sAvailableCaption
        End Get
        Set(ByVal Value As String)
            m_sAvailableCaption = Value
            RaiseEvent AvailableCaptionChange()

            lvwAll.Columns.Item(0).Text = Value
        End Set
    End Property

    's watton 22-11-2002 added read only public property
    'front office receipting

    <Browsable(False)> _
    Public ReadOnly Property SelectedItems() As Integer
        Get

            Return lvwContents.Items.Count

        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ItemArray() As Object
        Get
            Return VB6.CopyArray(m_vItemArray)
        End Get
    End Property



    <Browsable(True)> _
    Public Property IsSearchable() As Boolean
        Get
            Return m_bIsSearchable
        End Get
        Set(ByVal Value As Boolean)
            m_bIsSearchable = Value
            If m_bIsSearchable Then
                txtFindText.Visible = True
                cmdFind.Visible = True
                lvwAll.Top = txtFindText.Top + txtFindText.Height

                lvwAll.Height = lvwContents.Height - txtFindText.Height
                txtFindText.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwAll.Width) - VB6.PixelsToTwipsX(cmdFind.Width) - 30)
                cmdFind.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(txtFindText.Left) + VB6.PixelsToTwipsX(txtFindText.Width) + 30)
            ElseIf Not m_bIsSearchable Then
                txtFindText.Visible = False
                cmdFind.Visible = False
                lvwAll.Top = lvwContents.Top

                lvwAll.Height = lvwContents.Height
            End If
            RaiseEvent IsSearchableChange()

        End Set
    End Property
    'Changes for WPR-42
    Public Enum ChangeAction
        AddAll
        DeleteAll
        Add
        Delete
    End Enum
    Public Class ChangeEventArgs
        Inherits EventArgs

        Public Action As ChangeAction
        Public Cancel As Boolean
        Public IsEmpty As Boolean

        Public Sub New(ByVal Action As ChangeAction)
            Me.Action = Action
            Cancel = False
            IsEmpty = False
        End Sub 'New

    End Class

    'End of Changes for WPR-42
    Private Sub cmdFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFind.Click
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)
        Dim sWildcardErrorMessage As String = ""
        Const kMethodName As String = "cmdFind_Click"

        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.2)
        Dim sValue As String = ""
        ' Get System Option for Disable Wildcard Search
        m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If
        m_bDisableWildcardSearchOption = (sValue = "1")

        ' Get System Option for m_bEnablePartialWildcardSearchOption
        m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If
        m_bEnablePartialWildcardSearchOption = (sValue = "1")
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.1.2)
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)
        'Check wildcard searches

        If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtFindText.Text, r_sErrorMessage:=sWildcardErrorMessage) Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            MessageBox.Show(sWildcardErrorMessage, "PickList")
            txtFindText.Focus()
            Exit Sub
        End If
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.3.2.2)

        m_sSearchString = txtFindText.Text
        lvwAll.Items.Clear()
        RaiseEvent Find(Me, Nothing)
        txtFindText.Text = ""
    End Sub

    Private Sub txtFindText_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtFindText.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode = 13 Then
            cmdFind_Click(cmdFind, New EventArgs())
        End If
    End Sub


    Private Sub UserControl_InitProperties()
        m_bIsSearchable = False
        cmdFind.Visible = False
        txtFindText.Visible = False
        lvwAll.Top = lvwContents.Top
    End Sub



    ' ***************************************************************** '
    '
    ' Name: Load
    '
    ' Description: Loads the data into the grid
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer
        Dim result As Integer = 0
        Dim oBO, vResultArray(,) As Object


        Try

            ' RAW 10/04/2003 : ENDVR633 : added
            If g_oObjectManager Is Nothing Then

                ' Create an instance of the object manager.
                g_oObjectManager = New bObjectManager.ObjectManager()

                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    g_oObjectManager = Nothing
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                    Return result
                End If
            End If
            ' RAW 10/04/2003 : ENDVR633 : end

            'DD 05/08/2004: Clear the lists first (allows for reload)
            lvwAll.Items.Clear()
            lvwContents.Items.Clear()

            m_lReturn = g_oObjectManager.GetInstance(oBO, BusinessObject, gPMConstants.PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            End If



            m_lReturn = oBO.PickListLoad(PickListType, PKArray(), vResultArray)


            m_vItemArray = vResultArray

            If Information.IsArray(vResultArray) Then

                For iRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If CStr(vResultArray(2, iRow)) = "0" Then
                        'sw front office receipting, check to see if the item has been marked as deleted before adding to the list

                        If (CStr(vResultArray(1, iRow)).IndexOf("(Deleted)") + 1) = 0 Then

                            lvwAll.Items.Add("L" & CStr(vResultArray(0, iRow)), CStr(vResultArray(1, iRow)).Trim(), "")
                        End If
                    Else

                        lvwContents.Items.Add("L" & CStr(vResultArray(0, iRow)), CStr(vResultArray(1, iRow)).Trim(), "")
                    End If
                Next iRow
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to load pick list", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function LoadSearched() As Integer
        Dim result As Integer = 0
        Dim oBO, vResultArray(,) As Object

        Try

            ' RAW 10/04/2003 : ENDVR633 : added
            If g_oObjectManager Is Nothing Then

                ' Create an instance of the object manager.
                g_oObjectManager = New bObjectManager.ObjectManager()

                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    g_oObjectManager = Nothing
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                    Return result
                End If
            End If
            ' RAW 10/04/2003 : ENDVR633 : end


            m_lReturn = g_oObjectManager.GetInstance(oBO, BusinessObject, gPMConstants.PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            End If


            m_lReturn = oBO.PickListLoad(PickListType, PKArray(), vResultArray)

            If Information.IsArray(vResultArray) Then

                For iRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                    'sw front office receipting, check to see if the item has been marked as deleted before adding to the list

                    If (CStr(vResultArray(1, iRow)).IndexOf("(Deleted)") + 1) = 0 Then

                        lvwAll.Items.Add("L" & CStr(vResultArray(0, iRow)), CStr(vResultArray(1, iRow)).Trim(), "")
                    End If
                Next iRow
            Else
                'MessageBox.Show("No Records exists for matching criteria.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                MessageBox.Show("No Records exists for matching criteria.", "prjPickList", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtFindText.Focus()
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to load pick list", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Save
    '
    ' Description: Saves the chosen items back to the database.
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function Save() As Integer
        Dim result As Integer = 0
        Dim oBO As Object
        Dim vResultArray() As Object

        Try

            ' RAW 10/04/2003 : ENDVR633 : added
            If g_oObjectManager Is Nothing Then

                ' Create an instance of the object manager.
                g_oObjectManager = New bObjectManager.ObjectManager()

                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    g_oObjectManager = Nothing
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                    Return result
                End If
            End If
            ' RAW 10/04/2003 : ENDVR633 : end

            m_lReturn = g_oObjectManager.GetInstance(oBO, BusinessObject, gPMConstants.PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            End If

            'Build the results
            If lvwContents.Items.Count > 0 Then
                ReDim vResultArray(lvwContents.Items.Count - 1)
                For iRow As Integer = 1 To lvwContents.Items.Count

                    vResultArray(iRow - 1) = Mid(lvwContents.Items.Item(iRow - 1).Name, 2)
                Next iRow
            Else

                vResultArray = Nothing
            End If


            m_lReturn = oBO.PickListSave(PickListType, PKArray(), vResultArray)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to save pick list", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'name: CheckIfIDIsSelected
    '
    'sw added for front office receipting spec 25-11-2002
    '
    'We need a function that will accept an ID and return PMTrue/PMFalse to indicate
    'whether or not the ID appears in the selected list
    Public Function CheckIfIDIsSelected(ByVal v_lLookForMeID As Integer) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For iTemp As Integer = 1 To lvwContents.Items.Count
                If lvwContents.Items.Item(iTemp - 1).Name = "L" & v_lLookForMeID Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            Next iTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to Check If ID Is Selected", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfIDIsSelected", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function PKArray() As Object
        Dim Key As Object
        Dim vArray(,) As Object

        If ForeignKeys.Count > 0 Then
            ReDim vArray(2, ForeignKeys.Count - 1)

            For iRow As Integer = 1 To ForeignKeys.Count
                Key = ForeignKeys(iRow)


                vArray(0, iRow - 1) = Key.KeyName


                vArray(1, iRow - 1) = Key.Value


                vArray(2, iRow - 1) = Key.ValueType
            Next iRow
        End If

        Return vArray
    End Function

    ' ***************************************************************** '
    ' Name: AddAllItems
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function AddAllItems() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAll.Items.Count
                'changes lvwAll.Items were not deleted properly
                Dim item1 As New ListViewItem
                item1 = lvwAll.Items.Item(0)
                'm_lReturn = CType(AddItem(lvwAll.Items.Item(0)), gPMConstants.PMEReturnCode)
                m_lReturn = CType(AddItem(item1), gPMConstants.PMEReturnCode)
            Next iTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to add all items", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function IsSelected(ByVal Item As String) As Boolean
        Dim result As Boolean = False
        If lvwContents.Items.Count > 0 Then
            For iRow As Integer = 1 To lvwContents.Items.Count
                If Item = Mid(lvwContents.Items.Item(iRow - 1).Name, 2) Then
                    result = True
                    Exit For
                End If
            Next iRow
        End If
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddItems
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddItems() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAll.Items.Count

                ' KG 19/06/03 - Exit when new listcount reached.
                If iTemp > lvwAll.Items.Count Then Exit For

                If lvwAll.Items.Item(iTemp - 1).Selected Then
                    If Not IsSelected(Item:=Mid(lvwAll.Items.Item(iTemp - 1).Name, 2)) Then
                        'changes lvwAll.Items were not deleted properly
                        Dim item1 As New ListViewItem
                        item1 = lvwAll.Items.Item(iTemp - 1)
                        m_lReturn = CType(AddItem(item1), gPMConstants.PMEReturnCode)
                        'm_lReturn = CType(AddItem(lvwAll.Items.Item(iTemp - 1)), gPMConstants.PMEReturnCode)
                        iTemp -= 1
                    End If
                End If
            Next iTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to Add Items", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddItem
    '
    ' Description:
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function AddItem(ByRef oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Try

            If oListItem Is Nothing Then
                Return result
            End If
            Dim IsExists As Boolean

            For i As Integer = 1 To lvwContents.Items.Count
                If lvwContents.Items.Item(i - 1).Name <> oListItem.Name Then
                    If i = lvwContents.Items.Count Then
                        IsExists = False
                    End If
                ElseIf lvwContents.Items.Item(i - 1).Name = oListItem.Name Then
                    IsExists = True
                    Exit For
                End If
            Next
            If Not IsExists Then
                lvwContents.Items.Add(oListItem.Name, oListItem.Text, "")
            End If

            'lvwAll.Items.RemoveAt(oListItem.Index)
            lvwAll.Items.Remove(oListItem)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to add item", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllItems
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function DeleteAllItems() As Integer

        Dim result As Integer = 0
        Dim sAssociatedSoruce As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwContents.Items.Count
                lvwContents.Items.Item(iTemp - 1).Selected = True

                '        m_lReturn = DeleteItem(lvwContents.ListItems.Item(iTemp), _
                ''                               r_bIsSourceAssociatedWithDefaultBank:=bIsSourceAssociatedWithDefaultBank)
                '        If bIsSourceAssociatedWithDefaultBank Then
                '            sAssociatedSoruce = lvwContents.ListItems.Item(iTemp).Text
                '        End If
            Next iTemp

            DeleteItems()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to delete all items", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItems
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItems() As Integer

        Dim result As Integer = 0
        Dim bIsSourceAssociatedWithDefaultBank As Boolean
        Dim sAssociatedSoruce As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwContents.Items.Count

                ' KG 19/06/03 - Exit when new listcount reached.
                If iTemp > lvwContents.Items.Count Then Exit For

                If lvwContents.Items.Item(iTemp - 1).Selected Then
                    'changes as delete is not functioning properly
                    Dim item1 As New ListViewItem
                    item1 = lvwContents.Items.Item(iTemp - 1)
                    m_lReturn = CType(DeleteItem(item1, r_bIsSourceAssociatedWithDefaultBank:=bIsSourceAssociatedWithDefaultBank), gPMConstants.PMEReturnCode)
                    'm_lReturn = CType(DeleteItem(lvwContents.Items.Item(iTemp - 1), r_bIsSourceAssociatedWithDefaultBank:=bIsSourceAssociatedWithDefaultBank), gPMConstants.PMEReturnCode)
                    If bIsSourceAssociatedWithDefaultBank Then
                        sAssociatedSoruce.Append(", '" & lvwContents.Items.Item(iTemp - 1).Text & "'")
                    Else
                        iTemp -= 1
                    End If
                End If
            Next iTemp

            If sAssociatedSoruce.ToString().Length > 1 Then
                sAssociatedSoruce = New StringBuilder(sAssociatedSoruce.ToString().Substring(sAssociatedSoruce.ToString().Length - (sAssociatedSoruce.ToString().Length - 1)))
            End If

            If sAssociatedSoruce.ToString().Trim().Length <> 0 Then
                MessageBox.Show("Branch(s) " & sAssociatedSoruce.ToString() & " linked with Bank Account Default and can not be delinked.", "Can't Delink Source!", MessageBoxButtons.OK)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to delete items", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: DeleteItem()
    '
    ' Description:
    '********************************************************************

    Private Function DeleteItem(ByRef oListItem As ListViewItem, ByRef r_bIsSourceAssociatedWithDefaultBank As Boolean) As Integer

        Dim result As Integer = 0
        Try

            Dim lSoruceID As Integer
            Dim IsExists As Boolean
            result = gPMConstants.PMEReturnCode.PMTrue

            If oListItem Is Nothing Then
                Return result
            End If

            'sw front office receipting, check to see if the item has been deleted, if so
            'remove it from the selected list and DONT add to the all list

            If (oListItem.Text.IndexOf("(Deleted)") + 1) = 0 Then
                'has not been deleted so add to the available list
                lSoruceID = gPMFunctions.ToSafeLong(Mid(oListItem.Name, 2))

                m_lReturn = CType(CheckSourceLinkedWithBankAccountDefault(v_lSourceID:=lSoruceID, r_bIsSourceAssociatedWithDefaultBank:=r_bIsSourceAssociatedWithDefaultBank), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                If r_bIsSourceAssociatedWithDefaultBank Then
                    Return result
                End If


                'has not been deleted so add to the available list
                For i As Integer = 1 To lvwAll.Items.Count
                    If lvwAll.Items.Item(i - 1).Name <> oListItem.Name Then
                        If i = lvwAll.Items.Count Then
                            IsExists = False
                        End If
                    ElseIf lvwAll.Items.Item(i - 1).Name = oListItem.Name Then
                        IsExists = True
                        Exit For
                    End If
                Next
            End If


            If Not IsExists Then
                lvwAll.Items.Add(oListItem.Name, oListItem.Text, "")
            End If

            lvwContents.Items.RemoveAt(oListItem.Index)

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 10/04/2003 : ENDVR633 : added
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. Failed to delete item", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAddAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAll.Click
        '<Pankaj PN:38898>
        If lvwAll.Items.Count > 0 Then
            'Changes for WPR-42
            Dim ChangeArgs As New ChangeEventArgs(ChangeAction.AddAll)

            RaiseEvent Change(Me, ChangeArgs)

            If ChangeArgs.Cancel Then Exit Sub
            'End Changes for WPR-42
        End If
        m_lReturn = AddAllItems()

    End Sub

    Private Sub cmdAddOne_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddOne.Click
        '<Pankaj PN:38898>
        If lvwAll.Items.Count > 0 Then
            'Changes for WPR-42
            Dim ChangeArgs As New ChangeEventArgs(ChangeAction.Add)

            If lvwAll.SelectedItems.Count <= 0 Then
                ChangeArgs.IsEmpty = True
            End If

            RaiseEvent Change(Me, ChangeArgs)

            If ChangeArgs.Cancel Then Exit Sub

            'End Changes for WPR-42
        End If
        m_lReturn = AddItems()

    End Sub

    Private Sub cmdRemoveAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveAll.Click
        '<Pankaj PN:38898>
        If lvwContents.Items.Count > 0 Then
            'Changes for WPR-42
            Dim ChangeArgs As New ChangeEventArgs(ChangeAction.DeleteAll)

            RaiseEvent Change(Me, ChangeArgs)

            If ChangeArgs.Cancel Then Exit Sub

            'End Changes for WPR-42
        End If
        m_lReturn = DeleteAllItems()

    End Sub

    Private Sub cmdRemoveOne_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveOne.Click
        '<Pankaj PN:38898>
        If lvwContents.Items.Count > 0 Then
            'Changes for WPR-42
            Dim ChangeArgs As New ChangeEventArgs(ChangeAction.Delete)

            If lvwContents.SelectedItems.Count <= 0 Then
                ChangeArgs.IsEmpty = True
            End If

            RaiseEvent Change(Me, ChangeArgs)

            If ChangeArgs.Cancel Then Exit Sub
            'End Changes for WPR-42
        End If

        m_lReturn = DeleteItems()

    End Sub

    Private Sub lvwAll_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwAll.ColumnClick
        If lvwAll.Items.Count > 0 Then
            lvwAll.Items.Item(0).Selected = True
        End If
    End Sub

    Private Sub lvwAll_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAll.DoubleClick
        cmdAddOne_Click(cmdAddOne, New EventArgs())
    End Sub

    Private Sub lvwContents_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwContents.ColumnClick
        If lvwContents.Items.Count > 0 Then
            lvwContents.Items.Item(0).Selected = True
        End If
    End Sub

    Private Sub lvwContents_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContents.DoubleClick
        cmdRemoveOne_Click(cmdRemoveOne, New EventArgs())
    End Sub



    'developer guide no. no solution 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)

        Try



            BusinessObject = CStr(PropBag.ReadProperty("BusinessObject"))


            PickListType = CStr(PropBag.ReadProperty("PickListType"))


            AvailableCaption = CStr(PropBag.ReadProperty("AvailableCaption"))


            Enabled = CBool(PropBag.ReadProperty("Enabled", True))


            IsSearchable = CBool(PropBag.ReadProperty("IsSearchable", False))

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub PickList_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            cmdAddOne.Left = MyBase.Width / 2 - (cmdAddOne.Width / 2)
            cmdAddAll.Left = MyBase.Width / 2 - (cmdAddAll.Width / 2)
            cmdRemoveOne.Left = MyBase.Width / 2 - (cmdRemoveOne.Width / 2)
            cmdRemoveAll.Left = MyBase.Width / 2 - (cmdRemoveAll.Width / 2)

            cmdAddOne.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(MyBase.Height) / 2) - ((VB6.PixelsToTwipsY(cmdRemoveAll.Top) + VB6.PixelsToTwipsY(cmdRemoveAll.Height) - VB6.PixelsToTwipsY(cmdAddOne.Top)) / 2) + 120)
            cmdAddAll.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdAddOne.Top) + VB6.PixelsToTwipsY(cmdAddOne.Height) + 120)
            cmdRemoveOne.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdAddAll.Top) + VB6.PixelsToTwipsY(cmdAddAll.Height) + 480)
            cmdRemoveAll.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdRemoveOne.Top) + VB6.PixelsToTwipsY(cmdRemoveOne.Height) + 120)

            lvwAll.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdAddOne.Left) - VB6.PixelsToTwipsX(lvwAll.Left) - 120)
            lvwContents.Height = MyBase.Height - VB6.TwipsToPixelsY(240)
            lvwContents.Width = lvwAll.Width
            If Not m_bIsSearchable Then
                lvwAll.Height = lvwContents.Height
            ElseIf m_bIsSearchable Then
                lvwAll.Height = lvwContents.Height - txtFindText.Height
                txtFindText.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwAll.Width) - VB6.PixelsToTwipsX(cmdFind.Width) - 30)
                cmdFind.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(txtFindText.Left) + VB6.PixelsToTwipsX(txtFindText.Width) + 30)
            End If

            lvwContents.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdAddOne.Left) + VB6.PixelsToTwipsX(cmdAddOne.Width) + 120)

            lvwAll.Columns.Item(0).Width = CInt(lvwAll.Width - VB6.TwipsToPixelsX(90))
            lvwContents.Columns.Item(0).Width = CInt(lvwContents.Width - VB6.TwipsToPixelsX(90))

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


    End Sub




    'developer guide no. no soultion 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BusinessObject", BusinessObject)

        PropBag.WriteProperty("PickListType", PickListType)

        PropBag.WriteProperty("AvailableCaption", AvailableCaption, "Available")

        PropBag.WriteProperty("Enabled", Enabled, True)

        PropBag.WriteProperty("IsSearchable", m_bIsSearchable, False)

    End Sub

    ' ***************************************************************** '
    ' Name: CheckSourceLinkedWithBankAccountDefault
    ' Description: This function will disallow user to delink those branches
    '              which are associated with BankAccount_Default(only for PickListType = BankAccount_Source)
    ' History:
    ' Created : Pankaj : 21-05-2008 :
    ' ***************************************************************** '
    Private Function CheckSourceLinkedWithBankAccountDefault(ByVal v_lSourceID As Integer, ByRef r_bIsSourceAssociatedWithDefaultBank As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethod As String = "CheckSourceLinkedWithBankAccountDefault"


        Try
            Dim lBankAccountID, oBO As Object


            result = gPMConstants.PMEReturnCode.PMTrue

            'Check the request valid for when picklistType = BankAccount_Source
            If m_sPickListType.ToUpper() <> ("BankAccount_Source").ToUpper() Then
                Return result
            End If

            'Extract the bank_id set in key collection


            lBankAccountID = ForeignKeys("BankAccountID").Value


            ' RAW 10/04/2003 : ENDVR633 : added
            If g_oObjectManager Is Nothing Then

                ' Create an instance of the object manager.
                g_oObjectManager = New bObjectManager.ObjectManager()

                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, kMethod & " Fails to Initialise Object Manager", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = g_oObjectManager.GetInstance(oBO, BusinessObject, gPMConstants.PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to Create Object of " & BusinessObject, gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oBO.CheckSourceAssociatedWithDefaultBank(v_lBankAccountID:=lBankAccountID, v_lSourceID:=v_lSourceID, r_bIsSourceAssociatedWithDefaultBank:=r_bIsSourceAssociatedWithDefaultBank)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethod & " Fails to check is source linked with Bank Account Default ", gPMConstants.PMELogLevel.PMLogError)
            End If


            oBO.Dispose()

            oBO = Nothing



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_oObjectManager.UserName, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: LoadFromArray
    '
    ' Description: Loads the data into the grid
    '
    ' History: Date:9-Sep-2008

    '  Created by:Arul Stephen
    ' ***************************************************************** '
    Public Function LoadFromArray(ByRef vResultArray(,) As Object, Optional ByRef vDefaultClauses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bChosen As Boolean

        Try

            m_vItemArray = VB6.CopyArray(vResultArray)

            'Clear the list
            lvwAll.Items.Clear()
            lvwContents.Items.Clear()

            If Information.IsArray(vResultArray) Then
                For iRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                    If Information.IsArray(vDefaultClauses) Then
                        bChosen = False
                        For i As Integer = vDefaultClauses.GetLowerBound(1) To vDefaultClauses.GetUpperBound(1)


                            If CStr(vDefaultClauses(0, i)).Trim() = CStr(vResultArray(0, iRow)).Trim() Then
                                bChosen = True
                                Exit For
                            End If
                        Next i
                    End If


                    If Not bChosen And CStr(vResultArray(3, iRow)) = "0" Then

                        lvwAll.Items.Add("L" & CStr(vResultArray(0, iRow)).Trim(), CStr(vResultArray(1, iRow)).Trim() & " " & CStr(vResultArray(2, iRow)).Trim(), "") 'PN69821 To display Discription With Code
                    End If
                Next iRow
            End If

            If Information.IsArray(vDefaultClauses) Then
                For iRow As Integer = vDefaultClauses.GetLowerBound(1) To vDefaultClauses.GetUpperBound(1)
                    ' upload all chosen ones
                    ' PN 69821
                    If String.IsNullOrEmpty(Convert.ToString(vDefaultClauses(1, iRow)).Trim) Then

                        lvwContents.Items.Add("L" & CStr(vDefaultClauses(0, iRow)).Trim(), CStr(vDefaultClauses(2, iRow)).Trim() & "  " & CStr(vDefaultClauses(3, iRow)).Trim(), "")
                    Else

                        lvwContents.Items.Add("L" & CStr(vDefaultClauses(0, iRow)).Trim(), CStr(vDefaultClauses(1, iRow)).Trim() & "  " & CStr(vDefaultClauses(2, iRow)).Trim(), "")
                    End If
                    'END 69821
                Next iRow
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unexpected error detected. LoadCluases Failed to load pick list", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
End Class
