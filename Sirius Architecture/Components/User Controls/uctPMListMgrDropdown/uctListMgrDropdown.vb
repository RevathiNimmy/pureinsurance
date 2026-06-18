Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("uctDropdown_NET.uctDropdown")> _
Partial Public Class uctDropdown
    Inherits System.Windows.Forms.UserControl
    Public Event DataModelChange()
    Public Event LoginChange()
    Public Event LongListChange()
    Public Event ListManagerChange()
    Public Event MouseIconChange()
    Public Event LockedChange()
    Public Event ListIndexChange()
    Public Event ListChange()
    Public Event ItemDataChange()
    Public Event VehicleListIdChange()
    Public Event VehicleMakeChange()
    Public Event AutoCompleteTextChange()
    Public Event AllowAbiCodeEntryChange()
    Public Event PropertyIdChange()
    Public Event ToolTipTextChange()
    Public Event TextChange()
    Public Event SelTextChange()
    Public Event SelStartChange()
    Public Event SelLengthChange()
    Public Event MousePointerChange()
    Public Event ForeColorChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event BackColorChange()
    ' ####################################################################################
    ' PMListMgrDropdown control
    '
    ' History
    ' RDC 01102002 changed module-level vars declared 'dim' to 'private'
    ' CJB 27042005 changed FillCombo to add "" not " " to first position of combos as when
    '              the former was selected and the combo dropped down again the list would
    '              auto filter to show only items with a space in...this is far from intuitive!
    ' ####################################################################################


    ' RDC 01102002 already defined in gPMConstants.bas
    'Private Const PMFalse = 0
    'Private Const PMTrue = 1
    'Private Const PMFail = 10
    'Private Const PMError = 11
    'Private Const PMOK = 20
    'Private Const PMCancel = 21
    'Private Const PMNavigate = 30

    ' New Polaris Data Types
    Private Const GEMPolUnknown As Integer = 0
    Private Const GEMPolDate As Integer = 1
    Private Const GEMPolNumeric As Integer = 2
    Private Const GEMPolShortList As Integer = 3
    Private Const GEMPolLongList As Integer = 4
    Private Const GEMPolText As Integer = 5
    Private Const GEMPolNumeric2 As Integer = 6
    Private Const GEMPolRef As Integer = 9

    Private Const ACApp As String = "PMListMgrDropdown"
    Private Const ACClass As String = "uctDropdown"

    Private m_oListManager As Object

    'Default Property Values:
    Private Const M_DEF_DATAMODEL As String = "GIIM"
    Private Const M_DEF_PROPERTYID As String = ""
    Private Const M_DEF_LONGLIST As Boolean = False
    'RSBCQ1123 - start
    'This will force the lastest version of the list items to be copied to the client.
    Private Const M_DEF_LOGIN As Boolean = True
    'RSBCQ1123 - end
    Private Const M_DEF_READONLY As Boolean = False
    'SJ 05/04/2005 - start
    Private Const M_DEF_ALLOW_ABI_CODE_ENTRY As Boolean = False
    Private Const M_DEF_AUTO_COMPLETE_TEXT As Boolean = False
    'SJ 05/04/2005 - end

    Private Const M_DEF_VEHICLE_MAKE As String = ""
    Private Const M_DEF_VEHICLE_LIST_ID As String = ""

    'Property Variables:
    Private m_DataModel As String = ""
    Private m_ListManager As Object
    Private m_LongList As Boolean
    Private m_Login As Boolean
    Private m_PropertyId As String = ""
    ' RDC 01102002
    Dim m_bReadOnly As Boolean

    'SJ 01/04/2005 - start
    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Const CB_FINDSTRING As Integer = &H14CS
    Private Const CB_ERR As Integer = (-1)
    Private m_bEditFromCode As Boolean
    Private m_bAllowAbiCodeEntry As Boolean
    Private m_bAutoCompleteText As Boolean
    Private m_bDataChanged As Boolean
    'SJ 01/04/2005 - end

    Private m_sVehicleMake As String = ""
    Private m_sVehicleListId As String = ""
    Private m_sSaveVehicleMake As String = ""

    'Event Declarations:
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseDown
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseMove
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs) 'MappingInfo=UserControl,UserControl,-1,MouseUp
    Event Change(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboControl,cboControl,-1,Change
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboControl,cboControl,-1,Click
    Event DropDown(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboControl,cboControl,-1,DropDown
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs) 'MappingInfo=cboControl,cboControl,-1,DblClick
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs) 'MappingInfo=cboControl,cboControl,-1,KeyDown
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs) 'MappingInfo=cboControl,cboControl,-1,KeyPress
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs) 'MappingInfo=cboControl,cboControl,-1,KeyUp

    Private Sub cboControl_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboControl.DropDown
        RaiseEvent DropDown(Me, Nothing)


        'SJ 04/04/2005 - start
        m_bEditFromCode = True
        'SJ 04/04/2005 - end
        If cboControl.Items.Count = 0 Then
            Dim lReturn As Integer = FillCombo(False)
        End If

    End Sub

    Private Sub cboControl_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboControl.Enter


        Dim lReturn As Integer = FillCombo(True)

    End Sub

    ' RDC 01102002 if property readOnly is true, only items in the list are selectable
    Private Sub cboControl_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboControl.Leave

        Dim sText, sAbiCode As String
        Dim lReturn As gPMConstants.PMEReturnCode

        'SJ 04/04/2005 - start
        If m_bAllowAbiCodeEntry And m_bDataChanged And m_bReadOnly And cboControl.Text.Trim() <> "" And m_sVehicleMake.Trim() = "" And Not (m_oListManager Is Nothing) Then

            lReturn = m_oListManager.GetABICodeFromDescription(v_sPropertyId:=m_PropertyId.TrimStart(), v_sDescription:=cboControl.Text.Trim(), r_sABICode:=sAbiCode)
            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Or sAbiCode = "" Then

                lReturn = m_oListManager.GetDescriptionFromAbiCode(v_sPropertyId:=m_PropertyId.TrimStart(), v_sABICode:=cboControl.Text.Trim(), r_sDescription:=sText)
                If lReturn = gPMConstants.PMEReturnCode.PMTrue And sText.Trim() <> "" Then
                    cboControl.Text = sText
                End If
            End If
        End If
        m_bDataChanged = False
        'SJ 04/04/2005 - end

        'If Not (m_bReadOnly) Then
        '    ' not ReadOnly, so don't check it
        '    Exit Sub
        'End If

        sText = cboControl.Text.Trim()

        Dim bFound As Boolean = False

        ' check all list entries
        For iLoop As Integer = 0 To cboControl.Items.Count - 1
            If sText.ToUpper() = VB6.GetItemString(cboControl, iLoop).ToUpper() Then
                ' found it
                cboControl.Text = VB6.GetItemString(cboControl, iLoop)
                bFound = True
                Exit For
            End If
        Next

        If Not (bFound) Then
            ' not in list, so reject it
            cboControl.Text = ""
        End If


    End Sub

    Private Sub UserControl_Initialize()

        'Dim lReturn As Long

        '    Set m_oListManager = CreateObject(Class:="iGEMListManager.Interface")

        '    lReturn& = m_oListManager.Initialise

        '    lReturn& = m_oListManager.CheckListVersions

        m_oListManager = Nothing
    End Sub

    Private Sub uctDropdown_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        cboControl.Top = 0
        cboControl.Left = 0
        MyBase.Height = cboControl.Height
        cboControl.Width = MyBase.Width

    End Sub

    '
    ' Fills combo box with polaris list via ComponentManager
    '
    Private Function FillCombo(ByVal bGotFocus As Boolean) As Integer

        Dim result As Integer = 0
        Dim vListArray() As Object
        Dim sMatchString As String = ""
        Dim lNumItems As Integer

        Dim lReturn As Integer
        Dim sText As String = ""

        Dim bRefill As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            sMatchString = ""

            If m_sVehicleMake.Trim() = "" And m_PropertyId.Trim() = "" Then
                Return result
            End If

            If m_oListManager Is Nothing Then
                m_oListManager = New iGISListManager.Interface_Renamed()
            End If

            lReturn = m_oListManager.Initialise

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FillCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", excep:=New Exception("ListManager.Initialise Returned Error"))

                Return result
            End If


            lReturn = m_oListManager.CheckListVersions(v_sGISDataModelCode:=m_DataModel)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FillCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", excep:=New Exception("ListManager.CheckListVersions Returned Error"))

                Return result
            End If
            If m_sVehicleMake <> "" Then

                m_oListManager.VehicleListId = m_sVehicleListId
            End If

            With cboControl

                ' Check tag
                If (m_PropertyId.TrimStart() = "") And (m_sVehicleMake.TrimStart() = "") Then
                    Return result
                End If

                ' Save text
                sText = .Text

                If bGotFocus Then
                    bRefill = False
                Else
                    bRefill = m_LongList
                End If

                If m_sVehicleMake <> m_sSaveVehicleMake Then
                    bRefill = True
                    sText = ""
                    m_sSaveVehicleMake = m_sVehicleMake
                End If

                ' If it's not a refill, it only needs filling once
                If (Not bRefill) And (.Items.Count > 0) Then
                    ' Return successful
                    Return result
                End If

                ' If it's a refill, then only return matching items
                If bRefill Then
                    'SJ 04/04/2005 - start
                    If m_bAllowAbiCodeEntry And IsWildCard(.Text) Then
                        sMatchString = .Text
                        'SJ 04/04/2005 - end
                    Else
                        sMatchString = ""
                    End If
                End If

                ' Get the List from the list manager
                If m_sVehicleMake = "" Then
                    If sMatchString <> "" Then


                        lReturn = m_oListManager.GetList(v_sPropertyId:=m_PropertyId.TrimStart(), r_vListData:=vListArray, v_vSearchString:=sMatchString)
                    Else


                        lReturn = m_oListManager.GetList(v_sPropertyId:=m_PropertyId.TrimStart(), r_vListData:=vListArray)

                    End If
                Else

                    lReturn = m_oListManager.GetVehicleModels(r_vListData:=vListArray, v_vMake:=m_sVehicleMake)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FillCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", excep:=New Exception("ListManager.GetList Returned Error"))

                    Return result
                End If

                If Not Information.IsArray(vListArray) Then
                    .Items.Clear()
                    Return result
                End If

                ' Put the list into the Array

                lNumItems = vListArray.GetUpperBound(0)
                If Information.IsArray(vListArray) Then
                    .Items.Clear()
                    .Items.Add("")

                    For lItem As Integer = 0 To lNumItems


                        .Items.Add(CStr(vListArray(lItem)).Trim())

                    Next
                End If

                'sj 15/02/99 - end

                ' Restore text
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    .Text = sText
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FillCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", excep:=excep)

            MessageBox.Show(excep.Message, Application.ProductName)

            Return result



            Return result
        End Try
    End Function

    'SJ 04/04/2005 - start
    Private Function IsWildCard(ByVal v_sText As String) As Boolean

        If v_sText.Trim() = "" Then
            Return False
        End If

        Return v_sText.IndexOf("*"c) >= 0 Or v_sText.IndexOf("#"c) >= 0

    End Function
    'SJ 04/04/2005 - end

    ' **************************************************************
    ' Name : ParseTag
    '
    ' Description : Splits a screen control tag into the Polaris
    ' Property Type and Property ID and the Database Table and
    ' and Field Names.
    '
    ' **************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (ParseTag) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ParseTag(ByVal sTag As String, Optional ByRef iPropertyType As Integer = 0, Optional ByRef lPropertyID As Integer = 0, Optional ByRef sTable As String = "", Optional ByRef sField As String = "") As Integer
    '
    'Dim result As Integer = 0
    'Dim sTmpPropertyType, sTmpPropertyID, sTmpTable, sTmpField, sTmpTag As String
    'Dim iChar As Integer
    'Dim sChar As String = ""
    'Dim bComma As Boolean
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Initialise variables
    'sTmpPropertyType = ""
    'sTmpPropertyID = ""
    'sTmpTable = ""
    'sTmpField = ""
    '
    ' Get a copy of the tag to play with
    'sTmpTag = sTag
    '
    ' Property type is the first character
    'sTmpPropertyType = sTag.Substring(0, 2)
    'iPropertyType = Conversion.Val(sTmpPropertyType)
    '
    ' Property ID is the next 8 characters
    'sTmpPropertyID = sTmpTag.Substring(2, Math.Min(sTmpTag.Length, 8)).Trim()
    'lPropertyID = CInt(Conversion.Val(sTmpPropertyID))
    '
    '
    ' Trim off the first 10 characters
    'sTmpTag = sTmpTag.Substring(sTmpTag.Length - (sTmpTag.Length - 10))
    '
    'bComma = False
    '
    'iChar = (sTmpTag.IndexOf(","c) + 1)
    '
    'If iChar = 0 Then
    'sTmpTable = sTmpTag
    'Else
    'sTmpTable = sTmpTag.Substring(0, iChar - 1)
    'sTmpField = sTmpTag.Substring(iChar)
    'End If
    '
    'sTable = sTmpTable.Trim()
    'sField = sTmpField.Trim()
    '
    'Return result
    '
    'Catch 
    '
    '
    '
    '
    ' Log Error.
    '    LogMessage _
    'iType:=PMLogOnError, _
    'sMsg:="Invalid Tag : " & sTmpTag$, _
    'vApp:=ACApp, _
    'vClass:=ACClass, _
    'vMethod:="ParseTag", _
    'vErrNo:=Err.Number, _
    'vErrDesc:=Err.Description
    '
    'Return gPMConstants.PMEReturnCode.PMError


    '
    'Return result
    'End Try
    'End Function

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_PropertyId = M_DEF_PROPERTYID
        m_LongList = M_DEF_LONGLIST
        m_DataModel = M_DEF_DATAMODEL
        'SJ 04/04/2005 - start
        m_bAllowAbiCodeEntry = M_DEF_ALLOW_ABI_CODE_ENTRY
        m_bAutoCompleteText = M_DEF_AUTO_COMPLETE_TEXT
        m_sVehicleMake = M_DEF_VEHICLE_MAKE
        m_sVehicleListId = M_DEF_VEHICLE_LIST_ID
        'SJ 04/04/2005 - end
    End Sub

    'Load property values from storage


    'developer guide no. 1(No Solutions)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        cboControl.BackColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("BackColor", &H80000005)))
        cboControl.Enabled = CBool(PropBag.ReadProperty("Enabled", True))
        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", True))
        'developer guide no. 2(No Solutions)
        'Font = PropBag.ReadProperty("Font", Ambient.Font)
        Font = PropBag.ReadProperty("Font", MyBase.Font)



        cboControl.ForeColor = ColorTranslator.FromOle(CInt(PropBag.ReadProperty("ForeColor", &H80000008)))



        cboControl.Cursor = PropBag.ReadProperty("MousePointer", 0)


        cboControl.SelectionLength = PropBag.ReadProperty("SelLength", 0)


        cboControl.SelectionStart = PropBag.ReadProperty("SelStart", 0)


        cboControl.SelectedText = CStr(PropBag.ReadProperty("SelText", ""))


        cboControl.Text = CStr(PropBag.ReadProperty("Text", ""))


        ToolTip1.SetToolTip(cboControl, CStr(PropBag.ReadProperty("ToolTipText", "")))


        m_PropertyId = CStr(PropBag.ReadProperty("PropertyId", M_DEF_PROPERTYID))
        'SJ 04/04/2005 - start


        m_bAllowAbiCodeEntry = CBool(PropBag.ReadProperty("AllowAbiCodeEntry", M_DEF_ALLOW_ABI_CODE_ENTRY))


        m_bAutoCompleteText = CBool(PropBag.ReadProperty("AutoCompleteText", M_DEF_AUTO_COMPLETE_TEXT))


        m_sVehicleMake = CStr(PropBag.ReadProperty("VehicleMake", M_DEF_VEHICLE_MAKE))


        m_sVehicleListId = CStr(PropBag.ReadProperty("VehicleListId", M_DEF_VEHICLE_LIST_ID))
        'SJ 04/04/2005 - end
        'TO DO: The member you have mapped to contains an array of data.
        '   You must supply the code to persist the array.  A prototype
        '   line is shown next:
        '    For Index% = 0 To cboControl.ListCount
        '        cboControl.ItemData(Index%) = PropBag.ReadProperty("ItemData" & Index%, 0)
        '    Next Index%
        'TO DO: The member you have mapped to contains an array of data.
        '   You must supply the code to persist the array.  A prototype
        '   line is shown next:
        '    cboControl.List(Index) = PropBag.ReadProperty("List" & Index, "")
        '    cboControl.ListIndex = PropBag.ReadProperty("ListIndex", 0)



        'developer guide no. 5(No Solutions)
        'cboControl.Locked = PropBag.ReadProperty("Locked", False)
        cboControl.Enabled = True

        MouseIcon = PropBag.ReadProperty("MouseIcon", Nothing)

        m_ListManager = PropBag.ReadProperty("ListManager", Nothing)


        m_LongList = CBool(PropBag.ReadProperty("LongList", M_DEF_LONGLIST))


        m_Login = CBool(PropBag.ReadProperty("Login", M_DEF_LOGIN))


        m_DataModel = CStr(PropBag.ReadProperty("DataModel", M_DEF_DATAMODEL))
        ' RDC 01102002


        m_bReadOnly = CBool(PropBag.ReadProperty("ReadOnly", M_DEF_READONLY))
    End Sub

    Private Sub UserControl_Terminate()

        If Not (m_oListManager Is Nothing) Then

            m_oListManager.Dispose()
        End If

        m_oListManager = Nothing

    End Sub

    'Write property values to storage


    'developer guide no. 1(No Solutions)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        Dim Index As Integer

        PropBag.WriteProperty("BackColor", ColorTranslator.ToOle(cboControl.BackColor), &H80000005)

        PropBag.WriteProperty("Enabled", cboControl.Enabled, True)


        'developer guide no. 2(No Solutions)
        'PropBag.WriteProperty("Font", Font, Ambient.Font)
        PropBag.WriteProperty("Font", Font, MyBase.Font)

        PropBag.WriteProperty("ForeColor", ColorTranslator.ToOle(cboControl.ForeColor), &H80000008)

        PropBag.WriteProperty("MousePointer", cboControl.Cursor, 0)

        PropBag.WriteProperty("SelLength", cboControl.SelectionLength, 0)

        PropBag.WriteProperty("SelStart", cboControl.SelectionStart, 0)

        PropBag.WriteProperty("SelText", cboControl.SelectedText, "")

        PropBag.WriteProperty("Text", cboControl.Text, "")

        PropBag.WriteProperty("ToolTipText", ToolTip1.GetToolTip(cboControl), "")

        PropBag.WriteProperty("PropertyId", m_PropertyId, M_DEF_PROPERTYID)
        'SJ 04/04/2005 - start

        PropBag.WriteProperty("AllowAbiCodeEntry", m_bAllowAbiCodeEntry, M_DEF_ALLOW_ABI_CODE_ENTRY)

        PropBag.WriteProperty("AutoCompleteText", m_bAutoCompleteText, M_DEF_AUTO_COMPLETE_TEXT)

        PropBag.WriteProperty("VehicleMake", m_sVehicleMake, M_DEF_VEHICLE_MAKE)

        PropBag.WriteProperty("VehicleListId", m_sVehicleListId, M_DEF_VEHICLE_LIST_ID)
        'SJ 04/04/2005 - end
        'TO DO: The member you have mapped to contains an array of data.
        '   You must supply the code to persist the array.  A prototype
        '   line is shown next:
        '    Call PropBag.WriteProperty("ItemData" & Index, cboControl.ItemData(Index), 0)
        'TO DO: The member you have mapped to contains an array of data.
        '   You must supply the code to persist the array.  A prototype
        '   line is shown next:
        '    Call PropBag.WriteProperty("List" & Index, cboControl.List(Index), "")

        PropBag.WriteProperty("ListIndex", cboControl.SelectedIndex, 0)


        'developer guide no. 5(No Solutions)
        'PropBag.WriteProperty("Locked", cboControl.Locked, False)
        PropBag.WriteProperty("Locked", cboControl.Enabled, False)

        PropBag.WriteProperty("MouseIcon", MouseIcon, Nothing)

        PropBag.WriteProperty("ListManager", m_ListManager, Nothing)

        PropBag.WriteProperty("LongList", m_LongList, M_DEF_LONGLIST)

        PropBag.WriteProperty("Login", m_Login, M_DEF_LOGIN)

        PropBag.WriteProperty("DataModel", m_DataModel, M_DEF_DATAMODEL)

        PropBag.WriteProperty("ReadOnly", m_bReadOnly, M_DEF_READONLY)
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Appearance
    <Browsable(False)> _
    <Description("Returns/sets whether or not an object is painted at run time with 3-D effects.")> _
    Public ReadOnly Property Appearance() As Integer
        Get

            'developer guide no. 43(No Solutions)
            'Return cboControl.Appearance
            Return cboControl.FlatStyle
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,BackColor

    <Browsable(True)> _
    <Description("Returns/sets the background color used to display text and graphics in an object.")> _
    Public Overrides Property BackColor() As Color
        Get
            Return cboControl.BackColor
        End Get
        Set(ByVal Value As Color)
            cboControl.BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Enabled

    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return cboControl.Enabled
        End Get
        Set(ByVal Value As Boolean)
            cboControl.Enabled = Value
            MyBase.Enabled = Value
            RaiseEvent EnabledChange()
            'force update of ToolTipText if necessary
            'developer guide no. 3(No Solutions)
            If ToolTip1.GetToolTip(cboControl) <> Me.ToolTipText And Value Then
                'developer guide no. 3(No Solutions)
                ToolTip1.SetToolTip(cboControl, Me.ToolTipText)
            End If

        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Font

    <Browsable(True)> _
    <Description("Returns a Font object.")> _
    Public Overrides Property Font() As Font
        Get
            Return cboControl.Font
        End Get
        Set(ByVal Value As Font)
            cboControl.Font = Value
            RaiseEvent FontChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,ForeColor

    <Browsable(True)> _
    <Description("Returns/sets the foreground color used to display text and graphics in an object.")> _
    Public Overrides Property ForeColor() As Color
        Get
            Return cboControl.ForeColor
        End Get
        Set(ByVal Value As Color)
            cboControl.ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,ListCount
    <Browsable(False)> _
    <Description("Returns the number of items in the list portion of a control.")> _
    Public ReadOnly Property ListCount() As Integer
        Get
            Return cboControl.Items.Count
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,MousePointer

    <Browsable(True)> _
       <Description("Returns/sets the type of mouse pointer displayed when over part of an object.")> _
    Public Property MousePointer() As System.Windows.Forms.Cursor
        'developer guide no. 101
        Get
            Return cboControl.Cursor
        End Get
        Set(ByVal Value As System.Windows.Forms.Cursor)

            cboControl.Cursor = Value
            RaiseEvent MousePointerChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,SelLength

    <Browsable(True)> _
    <Description("Returns/sets the number of characters selected.")> _
    Public Property SelLength() As Integer
        Get
            Return cboControl.SelectionLength
        End Get
        Set(ByVal Value As Integer)
            cboControl.SelectionLength = Value
            RaiseEvent SelLengthChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,SelStart

    <Browsable(True)> _
    <Description("Returns/sets the starting point of text selected.")> _
    Public Property SelStart() As Integer
        Get
            Return cboControl.SelectionStart
        End Get
        Set(ByVal Value As Integer)
            cboControl.SelectionStart = Value
            RaiseEvent SelStartChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,SelText

    <Browsable(True)> _
    <Description("Returns/sets the string containing the currently selected text.")> _
    Public Property SelText() As String
        Get
            Return cboControl.SelectedText
        End Get
        Set(ByVal Value As String)
            cboControl.SelectedText = Value
            RaiseEvent SelTextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Sorted
    <Browsable(False)> _
    <Description("Indicates whether the elements of a control are automatically sorted alphabetically.")> _
    Public ReadOnly Property Sorted() As Boolean
        Get
            Return cboControl.Sorted
        End Get
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Text

    <Browsable(True)> _
    <Description("Returns/sets the text contained in the control.")> _
    Public Overrides Property Text() As String
        Get
            Return cboControl.Text
        End Get
        Set(ByVal Value As String)
            cboControl.Text = Value
            RaiseEvent TextChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,ToolTipText

    <Browsable(True)> _
    <Description("Returns/sets the text displayed when the mouse is paused over the control.")> _
    Public Property ToolTipText() As String
        Get
            Return ToolTip1.GetToolTip(cboControl)
        End Get
        Set(ByVal Value As String)
            ToolTip1.SetToolTip(cboControl, Value)
            RaiseEvent ToolTipTextChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("Property Id of the List as it is in the List Manager")> _
    Public Property PropertyId() As String
        Get
            Return m_PropertyId
        End Get
        Set(ByVal Value As String)
            m_PropertyId = Value
            RaiseEvent PropertyIdChange()
        End Set
    End Property
    'SJ 04/04/2005 - start
    <Browsable(True)> _
    Public Property AllowAbiCodeEntry() As Boolean
        Get
            Return m_bAllowAbiCodeEntry
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowAbiCodeEntry = Value
            RaiseEvent AllowAbiCodeEntryChange()
        End Set
    End Property
    <Browsable(True)> _
    Public Property AutoCompleteText() As Boolean
        Get
            Return m_bAutoCompleteText
        End Get
        Set(ByVal Value As Boolean)
            m_bAutoCompleteText = Value
            RaiseEvent AutoCompleteTextChange()
        End Set
    End Property
    <Browsable(True)> _
    Public Property VehicleMake() As String
        Get
            Return m_sVehicleMake
        End Get
        Set(ByVal Value As String)
            m_sVehicleMake = Value
            RaiseEvent VehicleMakeChange()
        End Set
    End Property
    <Browsable(True)> _
    Public Property VehicleListId() As String
        Get
            Return m_sVehicleListId
        End Get
        Set(ByVal Value As String)
            m_sVehicleListId = Value
            RaiseEvent VehicleListIdChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,ItemData

    <Browsable(True)> _
    <Description("Returns/sets a specific number for each item in a ComboBox or ListBox control.")> _
    Public Property ItemData(ByVal Index As Integer) As Integer
        Get
            Return VB6.GetItemData(cboControl, Index)
        End Get
        Set(ByVal Value As Integer)
            VB6.SetItemData(cboControl, Index, Value)
            RaiseEvent ItemDataChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,List

    <Browsable(True)> _
    <Description("Returns/sets the items contained in a control's list portion.")> _
    Public Property List(ByVal Index As Integer) As String
        Get
            Return VB6.GetItemString(cboControl, Index)
        End Get
        Set(ByVal Value As String)
            VB6.SetItemString(cboControl, Index, Value)
            RaiseEvent ListChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,ListIndex

    <Browsable(True)> _
    <Description("Returns/sets the index of the currently selected item in the control.")> _
    Public Property ListIndex() As Integer
        Get
            Return cboControl.SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            cboControl.SelectedIndex = Value
            RaiseEvent ListIndexChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Locked

    <Browsable(True)> _
    <Description("Determines whether a control can be edited.")> _
    Public Property Locked() As Boolean
        Get

            Return UpgradeStubs.VB_ComboBox.getLocked(cboControl)
        End Get
        Set(ByVal Value As Boolean)

            UpgradeStubs.VB_ComboBox.setLocked(cboControl, Value)
            RaiseEvent LockedChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=UserControl,UserControl,-1,MouseIcon

    <Browsable(True)> _
    <Description("Sets a custom mouse icon.")> _
    Public Property MouseIcon() As Image
        Get

            Return UpgradeStubs.VB_UserControl.getMouseIcon(Me.Parent)
        End Get
        Set(ByVal Value As Image)

            UpgradeStubs.VB_UserControl.setMouseIcon(Me.Parent, Value)
            RaiseEvent MouseIconChange()
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Style
    <Browsable(False)> _
    <Description("Returns/sets a value that determines the type of control and the behavior of its list box portion.")> _
    Public ReadOnly Property style() As Integer
        Get
            Return cboControl.DropDownStyle
        End Get
    End Property


    <Browsable(True)> _
    <Description("A reference to GIS List Manager")> _
    Public Property ListManager() As Object
        Get
            Return m_ListManager
        End Get
        Set(ByVal Value As Object)
            m_ListManager = Value
            RaiseEvent ListManagerChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("Indicates whether or not the List to be displayed is a Long List")> _
    Public Property LongList() As Boolean
        Get
            Return m_LongList
        End Get
        Set(ByVal Value As Boolean)
            m_LongList = Value
            RaiseEvent LongListChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Property Login() As Boolean
        Get
            Return m_Login
        End Get
        Set(ByVal Value As Boolean)
            m_Login = Value
            RaiseEvent LoginChange()
        End Set
    End Property


    <Browsable(True)> _
    <Description("Name of the GIS data model list to use")> _
    Public Property DataModel() As String
        Get
            Return m_DataModel
        End Get
        Set(ByVal Value As String)
            m_DataModel = Value
            RaiseEvent DataModelChange()
        End Set
    End Property

    ' RDC 01102002

    <Browsable(True)> _
    Public Property ReadOnly_Renamed() As Boolean
        Get
            Return m_bReadOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
        End Set
    End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,hWnd
    <Browsable(False)> _
    <Description("Returns a handle (from Microsoft Windows) to an object's window.")> _
    Public ReadOnly Property hwnd() As Integer
        Get
            Return cboControl.Handle.ToInt32()
        End Get
    End Property


    Private isInitializingComponent As Boolean
    Private Sub cboControl_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboControl.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        '    'SJ 01/04/2005 - start
        'Dim i As Integer
        'Dim j As Integer
        'Dim strPartial, strTotal As String

        'If m_bAutoCompleteText Then
        '    'Prevent processing as a result of changes from code
        '    If m_bEditFromCode Then
        '        m_bEditFromCode = False
        '        Exit Sub
        '    End If

        '    With cboControl
        '        'Lookup list item matching text so far
        '        strPartial = .Text
        '        Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(strPartial)
        '        Try
        '            i = SendMessage(.Handle.ToInt32(), CB_FINDSTRING, -1, tmpPtr)
        '            strPartial = Marshal.PtrToStringAnsi(tmpPtr)
        '        Finally
        '            Marshal.FreeHGlobal(tmpPtr)
        '        End Try
        '        'If match found, append unmatched characters
        '        If i <> CB_ERR Then
        '            'Get full text of matching list item
        '            strTotal = VB6.GetItemString(cboControl, i)
        '            'Compute number of unmatched characters
        '            j = strTotal.Length - strPartial.Length
        '            '
        '            If j <> 0 Then
        '                'Append unmatched characters to string
        '                m_bEditFromCode = True
        '                .Text = strTotal
        '                'Select unmatched characters
        '                .SelectionStart = strPartial.Length
        '                .SelectionLength = j
        '            End If
        '        End If
        '    End With
        'End If
        m_bDataChanged = True

        RaiseEvent Change(Me, Nothing)

    End Sub

    Private Sub cboControl_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboControl.SelectionChangeCommitted
        RaiseEvent Click(Me, Nothing)
    End Sub

    Private Sub cboControl_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboControl.DoubleClick
        RaiseEvent DblClick(Me, Nothing)
    End Sub

    Private Sub cboControl_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboControl.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        'SJ 01/04/2005 - start
        If m_bAutoCompleteText Then
            Select Case KeyCode
                Case Keys.Delete, Keys.Back
                    m_bEditFromCode = True
            End Select
        End If
        'SJ 01/04/2005 - end
        RaiseEvent KeyDown(Me, New KeyDownEventArgs(KeyCode, Shift))
    End Sub
    'developer guide no. 78
    Private Sub cboControl_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboControl.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        RaiseEvent KeyPress(Me, New KeyPressEventArgs(KeyAscii))
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboControl_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboControl.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        RaiseEvent KeyUp(Me, New KeyUpEventArgs(KeyCode, Shift))
    End Sub
    'SJ 04/04/2005 - end
    '
    ''WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    ''MappingInfo=UserControl,UserControl,-1,hWnd
    'Public Property Get hWnd() As Long
    '    hWnd = UserControl.hWnd
    'End Property

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,AddItem
    Public Sub AddItem(ByRef Item As String, Optional ByRef Index As Object = Nothing)
        cboControl.Items.Insert(Index, Item)
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,Clear
    Public Sub Clear()
        cboControl.Items.Clear()
    End Sub

    Private Sub uctDropdown_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub uctDropdown_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, x, y))
    End Sub

    Private Sub uctDropdown_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, x, y))
    End Sub

    'WARNING! DO NOT REMOVE OR MODIFY THE FOLLOWING COMMENTED LINES!
    'MappingInfo=cboControl,cboControl,-1,RemoveItem
    Public Sub RemoveItem(ByRef Index As Integer)
        cboControl.Items.RemoveAt(CShort(Index))
    End Sub
End Class