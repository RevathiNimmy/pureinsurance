Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctSIRSelectClauses_NET.uctSIRSelectClauses")> _
Partial Public Class uctSIRSelectClauses
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    '***********************************************************************************************
    'Created By     :   Arul Stephen
    'Date           :   02-Sep-2008
    'History        :   It is an New File
    '***********************************************************************************************

    '************************************************************************************************
    'Variable Declaration
    '************************************************************************************************

    Private Const ACClass As String = "uctSIRSelectClauses"


    'TODO no solution found yet
    'Private m_oBusiness As bSIRSelectClauses.Business
    Private m_oBusiness As Object

    Private m_iTask As Integer
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_vSelectedClauses As Object
    Private m_vProductList As Object
    Private m_vFinalSelectedClauses(,) As Object
    Private m_vSelectClause(,) As Object

    Private m_lSystemCurrencyId As Integer
    Private m_lWidth As Integer
    Private m_lHeight As Integer
    Private m_lStatus As Integer
    Private m_lProductID As Integer
    Private m_lRiskID As Integer
    Private m_lClauseID As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Private m_bViewMode                         As Boolean
    Private m_bIsInitialised As Boolean
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""


    '*************************************************************************************************
    'Properties
    '*************************************************************************************************


    <Browsable(True)> _
    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property ProductId() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property RiskId() As Integer
        Get
            Return m_lRiskID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskID = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property ClauseId() As Integer
        Get
            Return m_lClauseID
        End Get
        Set(ByVal Value As Integer)
            m_lClauseID = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property SystemCurrency() As Integer
        Get
            Return m_lSystemCurrencyId
        End Get
        Set(ByVal Value As Integer)
            m_lSystemCurrencyId = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (get SelectedClauses) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SelectedClauses() As Object
    'Return m_vSelectedClauses
    'End Function
    'UPGRADE_NOTE: (7001) The following declaration (let SelectedClauses) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub SelectedClauses(ByVal Value As Object)


    'm_vSelectedClauses = Value
    'End Sub

    '************************************************************************************************
    ' Business Started
    '************************************************************************************************

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check if already initialised
            If m_bIsInitialised Then
                Return result
            End If

            'Set m_colPaymentItems = New Collection

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With g_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRSelectClauses.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRSelectClauses.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            ' hold Initialised status
            m_bIsInitialised = True


        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    'This function will start the business
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = SetupSelectClausesDetailsListView()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupPolicyDetailsListView method Failed to setup the listview", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBusiness method Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (DataToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DataToInterface() As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "DataToInterface"
    'On Error GoTo Catch_Renamed
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_lReturn = PopulateScreen()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "PopulateScreen method Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    ' Do Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    '
    'Finally_Renamed: '
    '
    ' Set the mouse pointer to normal.
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    'Return result
    '
    'End Function


    Private Function PopulateScreen() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateScreen"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(PopulateClausesDetailsList(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClausesDetailsList method Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    Private Function PopulateClausesDetailsList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClausesDetailsList"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oListItem As ListViewItem
            Dim olistsubitem As ListViewItem.ListViewSubItem
            Dim bFlag As Boolean

            If gPMFunctions.IsArrayEmpty(m_vSelectClause) Then
                Return result
            End If

            lvwSelectClause.Items.Clear()

            For lSelectCount As Integer = m_vSelectClause.GetLowerBound(kDefaultRowIndex - 1) To m_vSelectClause.GetUpperBound(kDefaultRowIndex - 1)

                If m_vSelectClause(MainModule.ENSelectClause.id, lSelectCount) <> gPMConstants.PMEReturnCode.PMNotFound Then
                    bFlag = True
                    If lvwSelectClause.Items.Count >= 1 Then
                        For lGridDataCount As Integer = 0 To lvwSelectClause.Items.Count - 1
                            If Convert.ToString(lvwSelectClause.Items.Item(lGridDataCount).Tag) = ToSafeString(m_vSelectClause(MainModule.ENSelectClause.id, lSelectCount)).Trim() Then
                                bFlag = False
                            End If
                        Next
                    End If

                    If bFlag Then

                        oListItem = lvwSelectClause.Items.Add(ToSafeString(m_vSelectClause(MainModule.ENSelectClause.code, lSelectCount)).Trim(), "history")

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ToSafeString(m_vSelectClause(MainModule.ENSelectClause.Description, lSelectCount)).Trim()

                        If ToSafeDouble(m_vSelectClause(MainModule.ENSelectClause.Selected, lSelectCount)) = 0 Or ToSafeString(m_vSelectClause(MainModule.ENSelectClause.Selected, lSelectCount)) = "" Then
                            olistsubitem = New ListViewItem.ListViewSubItem
                            olistsubitem.Text = ""
                            oListItem.SubItems.Insert(2, olistsubitem)
                        ElseIf ToSafeDouble(m_vSelectClause(MainModule.ENSelectClause.Selected, lSelectCount)) = 1 Then
                            olistsubitem = New ListViewItem.ListViewSubItem
                            olistsubitem.Text = "saved"
                            oListItem.SubItems.Insert(2, olistsubitem)
                        End If

                        If ToSafeDouble(m_vSelectClause(ENSelectClause.Default_Renamed, lSelectCount)) = 0 Or ToSafeString(m_vSelectClause(ENSelectClause.Default_Renamed, lSelectCount)) = "" Then
                            olistsubitem = New ListViewItem.ListViewSubItem
                            olistsubitem.Text = ""
                            oListItem.SubItems.Insert(3, olistsubitem)
                        ElseIf ToSafeDouble(m_vSelectClause(ENSelectClause.Default_Renamed, lSelectCount)) = 1 Then
                            olistsubitem = New ListViewItem.ListViewSubItem
                            olistsubitem.Text = "saved"
                            oListItem.SubItems.Insert(3, olistsubitem)
                        End If

                        oListItem.Tag = ToSafeString(m_vSelectClause(MainModule.ENSelectClause.id, lSelectCount)).Trim()
                    End If
                End If

            Next lSelectCount



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    Private Sub cmdProperties_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdProperties.Click

        Const kMethodName As String = "cmdProperties_Click"
        Try



            m_lReturn = DataToChildForm()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DataToChildForm method Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Exit Sub
    End Sub

    Public Function DataToChildForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataToChildForm"
        Try



            Dim bFlag As Boolean
            bFlag = False

            result = gPMConstants.PMEReturnCode.PMTrue
            For lGridItemCount As Integer = 0 To lvwSelectClause.Items.Count - 1
                If lvwSelectClause.Items.Item(lGridItemCount).Selected Then
                    bFlag = True
                    Exit For
                End If
            Next

            If bFlag Then
                m_lReturn = ProcessClauses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessClauses Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                MessageBox.Show("No Clauses Selected", "Product", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    Public Function ProcessClauses() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessClauses"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Dim objClauses As New frmClauseSelection
            Dim lResultCount As Integer
            objClauses.Task = gPMConstants.PMEComponentAction.PMAdd
            objClauses.SystemCurrencyId = SystemCurrency
            ReDim m_vFinalSelectedClauses(MainModule.ENSelectClause.Branch, 0)


            For lSelectClauseCount As Integer = 0 To lvwSelectClause.Items.Count - 1

                If lvwSelectClause.Items.Item(lSelectClauseCount).Selected Then
                    ReDim Preserve m_vFinalSelectedClauses(MainModule.ENSelectClause.Branch, lResultCount)

                    m_vFinalSelectedClauses(MainModule.ENSelectClause.id, lResultCount) = Convert.ToString(lvwSelectClause.Items.Item(lSelectClauseCount).Tag)

                    'ARUL STEPHEN
                    'Start Girija - PN 54101
                    m_vFinalSelectedClauses(MainModule.ENSelectClause.code, lResultCount) = lvwSelectClause.Items.Item(lSelectClauseCount).Text
                    'End Girija - PN 54101
                    m_vFinalSelectedClauses(MainModule.ENSelectClause.Description, lResultCount) = lvwSelectClause.Items.Item(lSelectClauseCount).SubItems.Item(1).Text
                    'END ARUL STEPHEN

                    lResultCount += 1
                End If

            Next
            'developer guide no. 24
            objClauses.SelectedClauses = m_vFinalSelectedClauses
            objClauses.GetAllClausesAttachedToBranches = m_vSelectClause

            objClauses.ProductId = m_lProductID
            objClauses.ClauseId = m_lClauseID
            objClauses.RiskId = m_lRiskID
            objClauses.UniqueId = m_sUniqueId
            objClauses.ScreenHierarchy = m_sScreenHierarchy
            objClauses.ShowDialog()


        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    Private Sub lvwSelectClause_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectClause.DoubleClick
        Const kMethodName As String = "lvwSelectClause_DblClick"
        Try





            m_lReturn = DataToChildForm()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "lvwSelectClause_DblClick Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Exit Sub
    End Sub


    Private Function GetAllClauses() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAllClauses"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetAllClauses(v_lClauseType:=m_lClauseID, v_lRiskType:=m_lRiskID, v_lProduct_id:=m_lProductID, r_vReturnValues:=m_vSelectClause)

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetAllClauses Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lProductID = kSelectClauseNewRiskTypeOrProductTypeAdded And m_lRiskID = kSelectClauseNewRiskTypeOrProductTypeAdded Then
                cmdProperties.Enabled = False
            Else
                If m_lClauseID = kSelectClauseProductClauseId Or m_lClauseID = kSelectClauseRiskClauseId Then
                    m_lReturn = GetAllClauses()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetAllClauses Function Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            m_lReturn = PopulateScreen()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function


    Private Sub uctSIRSelectClauses_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try
            cmdProperties.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(MyBase.Width) - VB6.PixelsToTwipsX(cmdProperties.Width) - 50)
            cmdProperties.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(MyBase.Height) - VB6.PixelsToTwipsY(cmdProperties.Height) - 50)
            lvwSelectClause.Width = cmdProperties.Left - lvwSelectClause.Left
            lvwSelectClause.Height = MyBase.Height
            lvwSelectClause.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSelectClause.Width) / 4 + 1))
            lvwSelectClause.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSelectClause.Width) / 4 + 1))
            lvwSelectClause.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSelectClause.Width) / 4 + 1))
            lvwSelectClause.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSelectClause.Width) / 4 + 1))

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                m_vSelectedClauses = Nothing

                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    'This is to setup the columns
    Private Function SetupSelectClausesDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupSelectClausesDetailsListView"
        Dim lColWidth As Integer
        Dim sCaption As String = ""
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lColWidth = CInt((VB6.PixelsToTwipsX(lvwSelectClause.Width) - 100) / 10)

            lvwSelectClause.Columns.Clear()


            'developer guide no. 243
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSelectClause.Columns.Insert(kSelectClauseColHIndexCode, "", sCaption, CInt(VB6.TwipsToPixelsX(kSelectClauseWidthCode)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSelectClause.Columns.Insert(kSelectClauseColHIndexDescription, "", sCaption, CInt(VB6.TwipsToPixelsX(kSelectClauseColWidthDescription)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwSelected, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSelectClause.Columns.Insert(kSelectClauseColHIndexSelected, "", sCaption, CInt(VB6.TwipsToPixelsX(kSelectClauseColWidthSelected)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDefault, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSelectClause.Columns.Insert(kSelectClauseColHIndexDefault, "", sCaption, CInt(VB6.TwipsToPixelsX(kSelectClauseColWidthDefault)), HorizontalAlignment.Left, -1)




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    Private Sub lvwSelectClause_DrawSubItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvwSelectClause.DrawSubItem
        If e.SubItem.Text = "saved" Then
            e.Graphics.DrawImageUnscaled(ImageList.Images("saved"), e.Bounds)
        Else
            e.DrawDefault = True
        End If

    End Sub

    Private Sub lvwSelectClause_DrawColumnHeader(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvwSelectClause.DrawColumnHeader
        e.DrawDefault = True
    End Sub
End Class
