Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Configuration.ConfigurationManager

Namespace Nexus
    Partial Class LockManager
        Inherits Frontend.clsCMSPage

#Region "Page Events"

        ''' <summary>
        ''' Page Load Event to bind Lock data
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                BindGrid()
            End If
        End Sub



        ''' <summary>
        ''' Handle paging of grid on load of control
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvLockManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLockManager.Load
            If gvLockManager.PageCount = 1 Then
                gvLockManager.AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' Handles paging of the grid view control
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvLockManager_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) _
        Handles gvLockManager.PageIndexChanging
            gvLockManager.PageIndex = e.NewPageIndex
            BindGrid()
        End Sub

        ''' <summary>
        ''' Lock Manager Row COmmand to unlock a single row
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvLockManager_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) _
        Handles gvLockManager.RowCommand
            If e.CommandName = "Unlock" Then
                Dim bMaintainedSuccess As Boolean
                Dim oLockCollection As NexusProvider.LockCollection
                Dim oLock As New NexusProvider.Locks
                Dim bClearAll As Boolean = False
                Dim bLogout As Boolean = False


                oLockCollection = New NexusProvider.LockCollection
                Dim sLockDetails() As String = e.CommandArgument.ToString().Split("$")

                oLock.LockName = sLockDetails(0)
                oLock.LockValue = CType(sLockDetails(1), Integer)

                oLockCollection.Add(oLock)

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bClearAll, bLogout, Session(CNBranchCode).ToString())
                ViewState.Remove("LockCollection")
                BindGrid()
                If Not bMaintainedSuccess Then
                    Dim sScript As String = "alert('" & GetLocalResourceObject("err_ErrorOccured") & "');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "unlockerror", sScript, True)
                End If

            End If
        End Sub

        ''' <summary>
        ''' Set LockName and LockValue
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvLockManager_RowDataBound(ByVal sender As Object, _
                                                  ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLockManager.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lnkUnlockButton As LinkButton = CType(e.Row.FindControl("lnkUnlock"), LinkButton)

                'check Maintain locks role exists, if it doesn't then hide the Unlock button

                If Not UserCanDoTask("UnlockClaims") Then
                    btnUnlock.Visible = False
                    lnkUnlockButton.Visible = False
                Else
                    btnUnlock.Visible = True
                    lnkUnlockButton.Visible = True
                    lnkUnlockButton.CommandArgument = e.Row.Cells(1).Text & "$" & e.Row.Cells(2).Text
                End If

            End If
        End Sub


        ''' <summary>
        ''' Header Check box for all select
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim chkHSelect As CheckBox = CType(sender, CheckBox)
            Dim chkSelectRow As CheckBox
            If chkHSelect.Checked Then
                For Each oRow As GridViewRow In gvLockManager.Rows
                    chkSelectRow = CType(oRow.Cells(0).FindControl("chkSelect"), CheckBox)
                    chkSelectRow.Checked = True
                    chkSelectRow.Enabled = False
                Next
            Else
                For Each oRow As GridViewRow In gvLockManager.Rows
                    chkSelectRow = CType(oRow.Cells(0).FindControl("chkSelect"), CheckBox)
                    chkSelectRow.Checked = False
                    chkSelectRow.Enabled = True
                Next
            End If
        End Sub

        ''' <summary>
        ''' To unlock all the selected rows
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnUnlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnlock.Click
            If UserCanDoTask("UnlockClaims") Then
                Dim oWebService As NexusProvider.ProviderBase
                Dim oLockCollection As NexusProvider.LockCollection
                Dim chkSelect As CheckBox
                Dim bMaintainedSuccess As Boolean = False
                Dim bAllClear As Boolean = False
                Dim bLogout As Boolean = False

                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oLockCollection = New NexusProvider.LockCollection
                    Dim chkSelectAll As CheckBox = CType(gvLockManager.HeaderRow.Cells(0).FindControl("chkSelectAll"), CheckBox)

                    If chkSelectAll.Checked Then
                        bAllClear = True
                    Else

                        oLockCollection = New NexusProvider.LockCollection
                        For Each oRow As GridViewRow In gvLockManager.Rows
                            Dim oLock As New NexusProvider.Locks
                            chkSelect = CType(oRow.Cells(0).FindControl("chkSelect"), CheckBox)
                            If chkSelect.Checked AndAlso oRow.Cells(6).Text = "" Then
                                oLock.LockName = oRow.Cells(1).Text
                                oLock.LockValue = CType(oRow.Cells(2).Text, Integer)
                                oLockCollection.Add(oLock)
                            End If
                        Next
                    End If

                    bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                    ViewState.Remove("LockCollection")
                    BindGrid()

                    If Not bMaintainedSuccess Then
                        Dim sScript As String = "alert('" & GetLocalResourceObject("err_ErrorOccured") & "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "unlockerror", sScript, True)
                    End If
                Catch ex As System.Exception
                    Dim sScript As String = "alert('" & GetLocalResourceObject("err_ErrorOccured") & "');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "unlockerror", sScript, True)
                End Try
            End If
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Bind Locks grid with filtered dataset.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindGrid()
            If UserCanDoTask("UnlockClaims") Then
                Dim oWebService As NexusProvider.ProviderBase
                Dim oLockCollection As NexusProvider.LockCollection
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oLockCollection = New NexusProvider.LockCollection

                    If ViewState("LockCollection") IsNot Nothing AndAlso Cache.Item(ViewState("LockCollection")) IsNot Nothing Then
                        oLockCollection = CType(Cache.Item(ViewState("LockCollection")), NexusProvider.LockCollection)
                    Else
                        oLockCollection = oWebService.GetLockDetails(Session(CNBranchCode).ToString())
                        If ViewState("LockCollection") Is Nothing Then
                            Dim uLockCollCacheID As Guid
                            uLockCollCacheID = Guid.NewGuid()
                            ViewState.Add("LockCollection", uLockCollCacheID.ToString)
                        End If
                        Cache.Insert(ViewState("LockCollection").ToString(), oLockCollection, Nothing, DateTime.MaxValue, _
                                     TimeSpan.FromMinutes(5))
                    End If

                    If oLockCollection.Count > 0 Then
                        gvLockManager.DataSource = oLockCollection
                        gvLockManager.DataBind()
                        btnUnlock.Enabled = True
                    Else
                        gvLockManager.DataSource = Nothing
                        gvLockManager.DataBind()
                        btnUnlock.Enabled = False
                    End If

                Catch ex As System.Exception
                    Dim sScript As String = "alert('" & GetLocalResourceObject("err_ErrorOccured") & "');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "unlockerror", sScript, True)
                End Try
            End If
        End Sub

#End Region
    End Class
End Namespace
