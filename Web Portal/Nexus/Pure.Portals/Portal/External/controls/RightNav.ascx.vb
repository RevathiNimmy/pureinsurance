
Partial Class portal_External_controls_RightNav
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim RequestedPageURL As String = LCase(Request.Url.Segments(Request.Url.Segments.Length - 1).ToString)

        'If CheckURL(LCase(hypBrokerHomePage.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypBrokerHomePage.Visible = False
        'ElseIf CheckURL(LCase(hypNewQuote.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypNewQuote.Visible = False
        'ElseIf CheckURL(LCase(hypEnquiry.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypEnquiry.Visible = False
        'ElseIf CheckURL(LCase(hypRenewalSelection.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypRenewalSelection.Visible = False
        'ElseIf CheckURL(LCase(hypViewRenewal.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypViewRenewal.Visible = False
        'ElseIf CheckURL(LCase(hypMTA.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypMTA.Visible = False
        'ElseIf CheckURL(LCase(hypTasks.NavigateUrl), LCase(RequestedPageURL)) Then
        '    hypTasks.Visible = False
        'End If
    End Sub
    'Function CheckURL(ByVal PageURL, ByVal CurrentURL) As Boolean
    '    Dim intX As Integer
    '    Dim CheckSamePageStatus As Boolean
    '    Dim astrSplitItems As String() = Split(PageURL, "/")
    '    For intX = 0 To UBound(astrSplitItems)
    '        If intX = UBound(astrSplitItems) Then
    '            If astrSplitItems(intX) = CurrentURL Then
    '                CheckSamePageStatus = True
    '            End If
    '        End If
    '    Next
    '    If CheckSamePageStatus = True Then
    '        CheckURL = True
    '    Else
    '        CheckURL = False
    '    End If

    'End Function
End Class
