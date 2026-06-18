
Namespace Nexus
    Partial Class Controls_calendar
        Inherits CMS.Library.Frontend.clsCMSPage
        Dim iFirstYear As Integer
        Dim iLastYear As Integer
       

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim iYear As Integer
            Dim oYear As System.Web.UI.WebControls.ListItem
            Dim oControl As System.Web.UI.HtmlControls.HtmlInputButton
            Dim dtDate As DateTime
            Calendar1.ShowNextPrevMonth = True
            If IsPostBack = False Then
                If IsDate(Request.QueryString("origdate")) Then
                    dtDate = CDate(Request.QueryString("origdate"))
                Else
                    dtDate = Now
                End If

                cboYears.Items.Clear()
                
                If Request.QueryString("FirstYear") <> 0 And Request.QueryString("LastYear") <> 0 Then

                    For iYear = LastYear To FirstYear Step -1
                        oYear = New System.Web.UI.WebControls.ListItem
                        oYear.Text = iYear.ToString
                        oYear.Value = iYear
                        cboYears.Items.Add(oYear)

                    Next
                Else
                    For iYear = Year(DateAdd(DateInterval.Year, 1, Now)) To 1900 Step -1
                        oYear = New System.Web.UI.WebControls.ListItem
                        oYear.Text = iYear.ToString
                        oYear.Value = iYear

                        cboYears.Items.Add(oYear)
                    Next
                End If
                

                cboYears.SelectedValue = Year(dtDate)
                cboMonths.SelectedValue = dtDate.Month

                Calendar1.SelectedDate = dtDate
                Calendar1.VisibleDate = dtDate

                oControl = cmdAccept
                If (oControl Is Nothing) = False Then
                    If Request.QueryString("Months") <> "" Then
                        oControl.Attributes("OnClick") = "opener.document.getElementById('" _
                            & Request.QueryString("QID") & "').value=document.getElementById('" _
                            & txtHidden.ClientID & "').value; opener.document.getElementById('" _
                            & Request.QueryString("AddQID") & "').value=document.getElementById('" _
                            & txtAddHidden.ClientID & "').value; window.close();"
                    Else
                        oControl.Attributes("OnClick") = "opener.document.getElementById('" _
                            & Request.QueryString("QID") & "').value=document.getElementById('" _
                            & txtHidden.ClientID & "').value; opener.document.getElementById('" _
                            & Request.QueryString("QID") & "').focus(); window.close();"
                    End If

                End If

                txtHidden.Value = FormatDateTime(dtDate, DateFormat.ShortDate)

                If Request.QueryString("Months") <> "" Then
                    dtDate = dtDate.AddMonths(Request.QueryString("Months")).AddDays(-1)
                    txtAddHidden.Value = FormatDateTime(dtDate, DateFormat.ShortDate)
                End If

            End If
            'Code added  on 07-01-2009 Begin
            'This If else will check for the Boundary Month(January and December) and Years(1900 and the preceeding year.)
            'This If else will check for the Boundary Month(January and December) and Years(Which is set as Default through Property LastYear and the preceeding year.)
            If Request.QueryString("FirstYear") <> 0 And Request.QueryString("LastYear") <> 0 Then
                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year = LastYear) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year = FirstYear) Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            Else
                If (cboMonths.SelectedValue.ToString() = "12" And cboYears.SelectedValue.ToString() = Date.Now.Year + 1) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (cboMonths.SelectedValue.ToString() = "1" And cboYears.SelectedValue.ToString() = "1900") Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"

                End If
            End If
            'Code added  on 07-01-2009 End
        End Sub
        Public Property FirstYear() As Integer
            Get
                Return Request.QueryString("FirstYear")
            End Get
            Set(ByVal value As Integer)
                iFirstYear = value
            End Set
        End Property
        Public Property LastYear() As Integer
            Get
                Return Request.QueryString("LastYear")
            End Get
            Set(ByVal value As Integer)
                iLastYear = value
            End Set
        End Property
        Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged

            Dim dtDate As DateTime = Calendar1.SelectedDate
            Calendar1.VisibleDate = dtDate

            txtHidden.Value = FormatDateTime(dtDate, DateFormat.ShortDate)

            If Request.QueryString("Months") <> "" Then
                dtDate = dtDate.AddMonths(Request.QueryString("Months")).AddDays(-1)
                txtAddHidden.Value = FormatDateTime(dtDate, DateFormat.ShortDate)
            End If
        End Sub

        Protected Sub cboMonths_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMonths.SelectedIndexChanged
            Calendar1.NextMonthText = "&gt" 'To display the Next and Previous Arrows on change of month 
            Calendar1.PrevMonthText = "&lt"
            Calendar1.VisibleDate = CDate(Format(CInt(cboYears.SelectedValue), "0000") & "-" & Format(CInt(cboMonths.SelectedValue), "00") & "-01 00:00:00")
            'This If else will check for the Boundary Month(January and December) and Years(1900 and the preceeding year.)
            'This If else will check for the Boundary Month(January and December) and Years(Which is set as Default through Property LastYear and the preceeding year.)
            If Request.QueryString("FirstYear") <> 0 And Request.QueryString("LastYear") <> 0 Then
                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year = LastYear) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year = FirstYear) Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            Else
                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year.ToString() = Date.Now.Year + 1) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year.ToString() = "1900") Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            End If
        End Sub

        Protected Sub cboYears_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYears.Load
            Calendar1.VisibleDate = CDate(Format(CInt(cboYears.SelectedValue), "0000") & "-" & Format(CInt(cboMonths.SelectedValue), "00") & "-01 00:00:00")
            Calendar1.NextMonthText = "&gt" 'To display the Next and Previous Arrows on change of month 
            Calendar1.PrevMonthText = "&lt"
            'This If else will check for the Boundary Month(January and December) and Years(1900 and the preceeding year.)
            'This If else will check for the Boundary Month(January and December) and Years(Which is set as Default through Property LastYear and the preceeding year.)

            If Request.QueryString("FirstYear") <> 0 And Request.QueryString("LastYear") <> 0 Then
                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year = LastYear) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year = FirstYear) Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            Else

                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year.ToString() = Date.Now.Year + 1) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year.ToString() = "1900") Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            End If
        End Sub

        Protected Sub Controls_calendar_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, "../../default.master")
        End Sub

        Protected Sub Calendar1_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged
            'Code added on 07-01-2009 Begin
            ' set the ddls to the correct month/year.
            cboMonths.ClearSelection()
            cboYears.ClearSelection()

            Calendar1.NextMonthText = "&gt" 'To display the Next and Previous Arrows on change of month 
            Calendar1.PrevMonthText = "&lt"
            Calendar1.ShowNextPrevMonth = True

            'This If else will check for the Boundary Month(January and December) and Years(1900 and the preceeding year.)
            'This If else will check for the Boundary Month(January and December) and Years(Which is set as Default through Property LastYear and the preceeding year.)

            If Request.QueryString("FirstYear") <> 0 And Request.QueryString("LastYear") <> 0 Then
                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year = LastYear) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year = FirstYear) Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            Else

                If (Calendar1.VisibleDate.Month.ToString() = "12" And Calendar1.VisibleDate.Year.ToString() = Date.Now.Year + 1) Then
                    Calendar1.NextMonthText = ""
                    Calendar1.PrevMonthText = "&lt"
                ElseIf (Calendar1.VisibleDate.Month.ToString() = "1" And Calendar1.VisibleDate.Year.ToString() = "1900") Then
                    Calendar1.PrevMonthText = ""
                    Calendar1.NextMonthText = "&gt"
                End If
            End If

            'Code added on 07-01-2009 End
            ' set the ddls to the correct month/year.
            cboMonths.ClearSelection()
            cboYears.ClearSelection()
            cboMonths.Items.FindByValue(Calendar1.VisibleDate.Month.ToString()).Selected = True
            cboYears.Items.FindByValue(Calendar1.VisibleDate.Year.ToString()).Selected = True
        End Sub

        
    End Class
End Namespace