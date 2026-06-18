Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No 129. 
Imports SharedFiles

Partial Friend Class frmMain
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmMain
    '
    ' Date: 16/11/1998
    '
    ' Description: Main form listing lookup tables that a user can maintain.
    '
    ' Edit History:
    ' DAK291099 - Change of parameter in Business.GetTables
    ' DAK251199 - Make business object global
    ' DAK011299 - Changed privilege levels
    ' DAK021299 - Clear list before getting new data
    ' DAK031299 - Set focus on the list view
    ' DAK061299 - Enable/disable Maintain button after list refreshed
    ' ***************************************************************** '


    Private Const ACClass As String = "frmMain"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Status
    Private m_lStatus As Integer
    ' PMAuthorityLevel
    Private m_lPMAuthorityLevel As Integer

    ' Array containing the names of the tables in the architecture
    Private m_vArchitectureTables(,) As Object

    Private Sub cboPMProducts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMProducts.Click

        ' Refresh the tables
        cmdMaintain.Enabled = False
        m_lReturn = CType(PopulateList(), gPMConstants.PMEReturnCode)

        ' PWF - Ensure we keep focus on the combo box
        cboPMProducts.Focus()

    End Sub

    Private Sub cmdMaintain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMaintain.Click

        m_lReturn = CType(StartMaintain(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Status = gPMConstants.PMEReturnCode.PMOK
        ' exit the program
        Me.Close()

    End Sub

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

        ' Get the architecture tables
        cboPMProducts.ItemId = 2
        cboPMProducts.DefaultItemId = 2
        m_lReturn = g_oBusiness.GetTables(v_lPMProductID:=cboPMProducts.ItemId, r_vArray:=m_vArchitectureTables)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub


    Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Me.cboPMProducts.FirstItem = ""
        iPMFunc.ShowFormInTaskBar_Detach()
        ' Developer Guide No. 
        cboPMProducts.RefreshList()
        ' Populate the tables
        cmdMaintain.Enabled = False
        m_lReturn = CType(PopulateList(), gPMConstants.PMEReturnCode)

    End Sub

    ' ***************************************************************** '
    ' Name: MaintainLookup
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function MaintainLookup(ByVal v_sTableName As String, ByVal v_lPMProductCode As Integer) As Integer
        Dim result As Integer = 0

        ' Instance of MaintainLookup
        'Developer Guide No. 101(Guide)
        Dim oMaintainLookup As Object
        Dim iPrivilegeLevel As Integer
        Dim sLinkedCaption, sLinkedObjectName, sLinkedClassName, sInterface_Component As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Dim temp_oMaintainLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oMaintainLookup, sClassName:="iPMMaintainLookup.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oMaintainLookup = temp_oMaintainLookup
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Reset the mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMMaintainLookup.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="MaintainLookup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'DAK251199

            m_lReturn = g_oBusiness.GetTable(v_lPMProductID:=cboPMProducts.ItemId, v_sTableName:=v_sTableName, r_iPrivilegeLevel:=iPrivilegeLevel, r_sLinkedCaption:=sLinkedCaption, r_sLinkedObjectName:=sLinkedObjectName, r_sLinkedClassName:=sLinkedClassName, r_sInterface_component:=sInterface_Component)
            If m_lReturn < 0 Then
                Throw New Exception()
            End If

            With oMaintainLookup
                ' Set the product family

                .ProductFamily = v_lPMProductCode
                ' Set the table

                .TableName = v_sTableName
                ' Set authority level

                .PMAuthorityLevel = PMAuthorityLevel
                ' Set Privilege level

                .PrivilegeLevel = iPrivilegeLevel
                ' Set Linked object details

                .LinkedCaption = sLinkedCaption

                .LinkedObjectName = sLinkedObjectName

                .LinkedClassName = sLinkedClassName

                .Interface_Component = sInterface_Component
            End With

            ' Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start it up

            m_lReturn = oMaintainLookup.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Start Lookup Maintenance", vApp:=ACApp, vClass:=ACClass, vMethod:="MaintainLookup")
                End If
            End If

            ' Terminate it

            oMaintainLookup.Dispose()
            oMaintainLookup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MaintainLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MaintainLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFamily
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetFamily(ByRef r_lFamily As Integer) As Integer

        Dim result As Integer = 0
        Dim sCode As String = ""
        Dim lId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the ID of the product family
            lId = cboPMProducts.ItemId

            ' Use the business to get the code
            'DAK251199

            m_lReturn = g_oBusiness.GetProductCode(v_lID:=lId, r_sCode:=sCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product code from ID", vApp:=ACApp, vClass:=ACClass, vMethod:="StartMaintain", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the family id from the code
            r_lFamily = gPMConstants.PMProductFamilyByCode(v_sPMProductCode:=sCode.Trim())

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFamily Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFamily", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartMaintain
    '
    ' Description: Gets the selected table and calls the start function.
    '
    ' ***************************************************************** '
    Private Function StartMaintain() As Integer

        Dim result As Integer = 0
        Dim lFamily As Integer
        Dim sTableName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide No. 12(No Solutions)
            'CMG/PB 21082002 Change to key value not formated displayed text
            sTableName = lvwTables.FocusedItem.Name

            m_lReturn = CType(GetFamily(r_lFamily:=lFamily), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the product family", vApp:=ACApp, vClass:=ACClass, vMethod:="StartMaintain", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = CType(MaintainLookup(v_sTableName:=sTableName, v_lPMProductCode:=lFamily), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK031299
            If Me.Visible Then
                lvwTables.Focus()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartMaintain Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartMaintain", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateList
    '
    ' Description: Populates the list
    '
    ' ***************************************************************** '
    Private Function PopulateList() As Integer

        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim vArray(,) As Object
        Dim sText, sKey, sUnderwritingOrAgency As String 'PN 32482 (RC)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DAK031299
            If Me.Visible Then
                lvwTables.Focus()
            End If

            'DAK021299
            lvwTables.Items.Clear()

            ' Set the pointer to the busy icon
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Call the business and get the tables
            'DAK251199

            m_lReturn = g_oBusiness.GetTables(v_lPMProductID:=cboPMProducts.ItemId, r_vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                'PN 32482 (RC)
                iPMFunc.getUnderwritingOrAgency(sUnderwritingOrAgency)

                ' Add them to the list

                For iLoop1 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    sKey = CStr(vArray(0, iLoop1)).Trim()
                    'CMG/PB 21082002 Replace underscores and convert to propercase
                    'PWF 28082002 Changed to custom format function, neater.
                    sText = gPMFunctions.ReformatText(sKey)

                    'PN 32482 (RC)
                    If Not (sKey.ToLower().Trim() = "payment_method" And sUnderwritingOrAgency = "U") Then


                        lstItem = lvwTables.Items.Add(sKey, sText, "table")
                        'DAK011299

                        'Developer Guide No. 12(No Solutions)
                        lstItem.ForeColor = Color.Black

                        If CInt(vArray(1, iLoop1)) = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit Then

                            'Developer Guide No. 12(No Solutions)
                            lstItem.ForeColor = Color.Gray
                        End If

                        If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser Then

                            If CInt(vArray(1, iLoop1)) = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserNone Or CInt(vArray(1, iLoop1)) = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserNone Or CInt(vArray(1, iLoop1)) = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminViewUserNone Then


                                'Developer Guide No. 12(No Solutions)
                                lstItem.ForeColor = Color.Gray
                            End If
                        End If

                    End If

                Next iLoop1

                ' CF 310899
                ' Select the first item
                lvwTables.FocusedItem = lvwTables.Items.Item(0)
                'DAK061299

                'Developer Guide No. 12(No Solutions)
                cmdMaintain.Enabled = Not (lvwTables.FocusedItem.ForeColor = Color.Gray)

            End If

            ' Size the columns
            m_lReturn = CType(ListView6Func.ListViewAutoSize(lvwTables), gPMConstants.PMEReturnCode)

            ' Reset the mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Static bTerminated As Boolean

        If bTerminated Then
            Exit Sub
        End If

        bTerminated = True

        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    ' Name: DupeCheck
    '
    ' Description: Removes any architecture tables from the current array
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DupeCheck) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DupeCheck(ByRef r_vArray( ,  ) As Object) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' If its in the architecture array, then...
    'For 'iLoop1 As Integer = r_vArray.GetLowerBound(1) To r_vArray.GetUpperBound(1)
    'For 'iLoop2 As Integer = m_vArchitectureTables.GetLowerBound(1) To m_vArchitectureTables.GetUpperBound(1)

    'If CStr(r_vArray(0, iLoop1)).Trim() = CStr(m_vArchitectureTables(0, iLoop2)).Trim() Then
    ' ...blank it out


    'r_vArray(0, iLoop1) = "*" & CStr(r_vArray(0, iLoop1))
    'End If
    'Next iLoop2
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DupeCheck Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DupeCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RemoveBanned
    '
    ' Description: Goes through the array and removes any "banned" tables
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RemoveBanned) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RemoveBanned(ByRef r_vArray( ,  ) As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim sText, sNewText As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Loop through the array
    'For 'iLoop1 As Integer = r_vArray.GetLowerBound(1) To r_vArray.GetUpperBound(1)
    '

    'sText = CStr(r_vArray(0, iLoop1))
    '
    'Select Case sText.ToLower()
    'Case "pmuser", "pmproduct", "pmnav_map", "pmnav_process", "pmuser_group", "pmwrk_task", "pmwrk_task_group", "currency"
    'sText = ""
    'End Select
    '
    ' if broking then remove the following tables...
    ' ...
    ' end if
    '

    'r_vArray(0, iLoop1) = sText
    '
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveBanned Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveBanned", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub lvwTables_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTables.DoubleClick

        If cmdMaintain.Enabled Then
            m_lReturn = CType(StartMaintain(), gPMConstants.PMEReturnCode)
        End If

    End Sub

    Private Sub lvwTables_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTables.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)

        'If Not (lvwTables.GetItemAt(x, y) Is Nothing) Then
        If lvwTables.Items.Count < 0 Then
            If Not (lvwTables.FocusedItem.Equals(Nothing)) Then

                'Developer Guide No. 12(No Solutions)
                cmdMaintain.Enabled = Not (lvwTables.FocusedItem.ForeColor = Color.Gray)
            Else
                cmdMaintain.Enabled = False
            End If
        End If
    End Sub

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property
End Class
