Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports System.Text.RegularExpressions
Imports System.Web.HttpContext
Imports NexusProvider

''' <summary>
''' Web implementation of the BackOffice FindControl, works both server and client side with CallBacks
''' </summary>
''' <remarks></remarks>
<DefaultProperty("Text"), ToolboxData("<{0}:LookupList runat=server></{0}:LookupList>"), ValidationProperty("Value")>
Public Class FindControl : Inherits WebControl : Implements ICallbackEventHandler

    Protected oXMLResults As XmlElement
    Protected sGridViewCallBack, sSortByColumn, sColumnHeaders As String
    Protected oMappedControls, oDisplayControls As FindControlCriteriaCollection
    Protected bShowHeader, bAllowColumnSorting As Boolean
    Shared sPrevSortColumn As String = ""
    Shared bDescSortflag As Boolean = False
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> Property FindControlKey() As Integer
        Get
            Return ViewState("FindControlKey")
        End Get
        Set(ByVal value As Integer)
            ViewState("FindControlKey") = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Category("Data")> WriteOnly Property AllowColumnSorting() As Boolean
        Set(ByVal value As Boolean)
            bAllowColumnSorting = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Category("Data")> WriteOnly Property DefaultSort() As String
        Set(ByVal value As String)
            sSortByColumn = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Category("Data")> WriteOnly Property ShowHeader() As Boolean
        Set(ByVal value As Boolean)
            bShowHeader = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <Category("Data")> WriteOnly Property ColumnHeaders() As String
        Set(ByVal value As String)
            sColumnHeaders = value
        End Set
    End Property
    ''' <summary>
    ''' semicolon seperated list of risk control id's that contain the search criteria and the selected result
    ''' </summary>
    ''' <value>semicolon seperated list of risk control id's</value>
    ''' <remarks></remarks>
    <Category("Data")> Public WriteOnly Property MappedControls() As String
        Set(ByVal value As String)

            'split the ; seperated string of controls into search criteria collection

            oMappedControls = New FindControlCriteriaCollection
            oMappedControls.AddRange([Array].ConvertAll(value.Replace(" ", "").Replace(vbCr, "").Replace(vbLf, "") _
                .Split(";".ToCharArray, System.StringSplitOptions.RemoveEmptyEntries),
                New Converter(Of String, FindControlCriteriaItem)(AddressOf FindControlCriteriaItem.MapControl)))

        End Set
    End Property
    ''' <summary>
    ''' semicolon seperated list of Core control id's that contain the selected result
    ''' </summary>
    ''' <value>semicolon seperated list of Core control id's<</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> Public Property DisplayControls() As String
        Get
            Dim sReturn As String = Nothing
            If oDisplayControls IsNot Nothing Then
                sReturn = oDisplayControls.ToString
            End If
            Return sReturn
        End Get
        Set(ByVal value As String)

            'split the ; seperated string of controls into search criteria collection

            oDisplayControls = New FindControlCriteriaCollection
            oDisplayControls.AddRange([Array].ConvertAll(value.Replace(" ", "").Replace(vbCr, "").Replace(vbLf, "") _
                .Split(";".ToCharArray, System.StringSplitOptions.RemoveEmptyEntries),
                New Converter(Of String, FindControlCriteriaItem)(AddressOf FindControlCriteriaItem.DispalyControl)))

        End Set

    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property SearchCriteria() As FindControlCriteriaCollection
        Get
            If ViewState("SearchCriteria") Is Nothing Then
                Return New FindControlCriteriaCollection
            Else
                Return ViewState("SearchCriteria")
            End If
        End Get
        Set(ByVal value As FindControlCriteriaCollection)
            ViewState("SearchCriteria") = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <exception cref="ArgumentNullException">FindControlKey is a required attribute</exception>
    ''' <remarks></remarks>
    Private Sub FindControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If String.IsNullOrEmpty(FindControlKey) Then
            Throw New ArgumentNullException("FindControlKey")
        End If

        EnableViewState = True

    End Sub
    ''' <summary>
    ''' Below code works condtionally for oDisplayControls and oMappedControls on the basis of 
    ''' DisplayControls property is specified in Findcontrol definition or not.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        MyBase.OnInit(e)

        Dim sbJSVariables As New Text.StringBuilder
        sbJSVariables.Append("var sSearchCritiera_" & ClientID & ";" & vbCr)
        sbJSVariables.Append("var sSelection_" & ClientID & ";" & vbCr)
        sbJSVariables.Append("var oControls_" & ClientID & " = new Array(")

        If Not String.IsNullOrEmpty(DisplayControls) Then

            For Each oCriteria As FindControlCriteriaItem In oDisplayControls

                Dim oControl As Control = Parent.FindControl(oCriteria.ControlName)
                If oControl Is Nothing Then
                    Throw New ArgumentException("DisplayControls attribute contains invalid controls", "DisplayControls")
                Else
                    sbJSVariables.Append("'" & oControl.ClientID & "', ")
                End If
            Next
        Else
            For Each oCriteria As FindControlCriteriaItem In oMappedControls

                Dim oControl As Control = Parent.FindControl(oCriteria.ControlName)
                If oControl Is Nothing Then
                    Throw New ArgumentException("MappedControls attribute contains invalid controls", "MappedControls")
                Else
                    sbJSVariables.Append("'" & oControl.ClientID & "', ")
                End If
            Next
        End If

        sbJSVariables.Remove(sbJSVariables.Length - 2, 2)
        sbJSVariables.Append(");")

        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "FindControl", My.Resources.FindControl, True)

        'DH - 21-01-08 - Can't get web resources to work a the moment, so we'll use a scriptblock for now, see above.
        'Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "FindControl", _
        '    Page.ClientScript.GetWebResourceUrl(GetType(FindControl), "NexusProvider.FindControl.js"))

        Page.ClientScript.RegisterStartupScript(Me.GetType, "FindControl_" & ClientID, sbJSVariables.ToString, True)

        Dim ltResults As New Literal
        ltResults.ID = ClientID & "_Results"
        ltResults.Text = "<div id='FindResultContainer" & ClientID & "' class='modal fade' style='overflow-y:hidden;' tabindex='-1' role='dialog' aria-labelledby='myModalLabel'><div id='modalfindpage' class='modal-dialog modal-dialog-centered' style='max-width:90vw;width:90vw;' role='document'><div class='modal-content' style='height:400px;display:flex;flex-direction:column'><div class='modal-header bootstrap-dialog-draggable'><div class='bootstrap-dialog-header'><div class='bootstrap-dialog-title' id='myModalLabel'>Found Items</div><div class='bootstrap-dialog-close-button float-end' style='display:contents'><button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='close'></button></div></div></div><div id=""" & ltResults.ID & """" & " class=""modal-body p-3" & IIf(String.IsNullOrEmpty(CssClass), "", " " & CssClass) & """ style=""flex:1;overflow-y:auto""" & "></div></div></div></div>"

        Dim btnClear As New LinkButton
        btnClear.ID = ClientID & "_btnClear"
        btnClear.Text = "<i class='fa fa-times' aria-hidden='true'></i> Clear"
        btnClear.SkinID = "btnSM"
        btnClear.CausesValidation = False
        AddHandler btnClear.Click, AddressOf ClearClick
        btnClear.OnClientClick = "javascript:ClearSearch('" & ltResults.ClientID & "', oControls_" & ClientID & "); return false;"

        Dim btnFind As New LinkButton
        btnFind.ID = ClientID & "_hypFind"
        btnFind.Text = "<i class='fa fa-search' aria-hidden='true'></i> Find"
        'btnFind.CssClass = "thickbox"
        btnFind.SkinID = "btnSM"
        btnFind.CausesValidation = False
        AddHandler btnFind.Click, AddressOf FindClick
        'Dim alt As String = "#TB_inline?height=540px&width=778px&inlineId=FindResultContainer" & ClientID
        'btnFind.Attributes.Add("alt", alt)
        btnFind.Attributes.Add("data-bs-toggle", "modal")
        btnFind.Attributes.Add("data-bs-target", "#FindResultContainer" & ClientID)
        btnFind.OnClientClick = "javascript:sSearchCritiera_" & ClientID & " = FindSearch('" & ltResults.ClientID & "', oControls_" _
                & ClientID & "); setTimeout(""" & Page.ClientScript.GetCallbackEventReference(Me, "'<FindControl>' + sSearchCritiera_" _
                & ClientID & " + '</FindControl>'", "FindControl", "'" & ClientID & "'", False) & """, 1); return false;"

        Dim btnSelect As New LinkButton()
        btnSelect.ID = ClientID & "_btnSelect"
        btnSelect.Text = "Select"
        btnSelect.SkinID = "btnSM"
        btnSelect.Visible = False
        btnSelect.CausesValidation = False
        AddHandler btnSelect.Click, AddressOf SelectClick

        MyBase.Controls.Add(btnFind)
        MyBase.Controls.Add(btnClear)
        MyBase.Controls.Add(btnSelect)
        MyBase.Controls.Add(ltResults)


    End Sub

    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return sGridViewCallBack

    End Function

    ''' <summary>
    ''' Event raise by OnClick of the clear button, will clear the search criteria and any results displayed.
    ''' from list of Mapped Controls Or Display Controls on the basis DisplayControls property is specified 
    ''' in Findcontrol definition or not.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ClearClick(ByVal sender As Object, ByVal e As System.EventArgs)


        If Not String.IsNullOrEmpty(DisplayControls) Then
            For Each oCriteria As FindControlCriteriaItem In oDisplayControls

                Dim oControl As Control = Parent.FindControl(oCriteria.ControlName)
                Select Case True
                    Case TypeOf oControl Is TextBox
                        CType(oControl, TextBox).Text = String.Empty
                    Case TypeOf oControl Is LookupList
                        CType(oControl, LookupList).Value = -1
                    Case TypeOf oControl Is HtmlControls.HtmlInputHidden
                        CType(oControl, HtmlControls.HtmlInputHidden).Value = String.Empty
                End Select

            Next
        Else
            For Each oCriteria As FindControlCriteriaItem In oMappedControls

                Dim oControl As Control = Parent.FindControl(oCriteria.ControlName)
                Select Case True
                    Case TypeOf oControl Is TextBox
                        CType(oControl, TextBox).Text = String.Empty
                    Case TypeOf oControl Is LookupList
                        CType(oControl, LookupList).Value = -1
                    Case TypeOf oControl Is HtmlControls.HtmlInputHidden
                        CType(oControl, HtmlControls.HtmlInputHidden).Value = String.Empty
                End Select

            Next
        End If

        CType(FindControl(ClientID & "_btnSelect"), LinkButton).Visible = False
        CType(FindControl(ClientID & "_Results"), Literal).Text = String.Empty

    End Sub

    ''' <summary>
    ''' Event to handle the OnClick event of the find button. This will make the neccessary
    ''' SAM call and display an results found in Mapped Controls Or Display Controls on the 
    ''' basis DisplayControls property is specified in Findcontrol definition or not.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Only raised when the FindControl is NOT working in clientside mode</remarks>
    Private Sub FindClick(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not String.IsNullOrEmpty(DisplayControls) Then
            Dim iItem As Integer
            Dim oSearchCriteria As FindControlCriteriaCollection = oMappedControls

            For iItem = 0 To oMappedControls.Count - 1
                Dim oDisplayControl As Control = Parent.FindControl(oDisplayControls.Item(iItem).ControlName)
                Select Case True
                    Case TypeOf oDisplayControl Is TextBox
                        oSearchCriteria.Item(iItem).Value = CType(oDisplayControl, TextBox).Text
                    Case TypeOf oDisplayControl Is LookupList
                        oSearchCriteria.Item(iItem).Value = CType(oDisplayControl, LookupList).Text
                End Select

            Next
            SearchCriteria = oSearchCriteria
            Search()
        Else

            Dim oSearchCriteria As FindControlCriteriaCollection = oMappedControls
            For Each oCriteria As FindControlCriteriaItem In oSearchCriteria

                Dim oControl As Control = Parent.FindControl(oCriteria.ControlName)
                Select Case True
                    Case TypeOf oControl Is TextBox
                        oCriteria.Value = CType(oControl, TextBox).Text
                    Case TypeOf oControl Is LookupList
                        oCriteria.Value = CType(oControl, LookupList).Text
                End Select

            Next

            SearchCriteria = oSearchCriteria
            Search()
        End If



        CType(FindControl(ClientID & "_Results"), Literal).Text = "<div id=""" & ClientID & "_Results""" _
            & IIf(String.IsNullOrEmpty(CssClass), "", " class=""" & CssClass & """") & ">" & sGridViewCallBack & "</div>"

    End Sub

    ''' <summary>
    ''' Event raised when one of the search results is selected, this will populate the 
    ''' Mapped Controls Or Display Controls on the basis DisplayControls property is specified 
    ''' in Findcontrol definition or not.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Only raised when the FindControl is NOT working in clientside mode</remarks>
    Private Sub SelectClick(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not String.IsNullOrEmpty(DisplayControls) Then
            SearchCriteria = oDisplayControls
        Else
            SearchCriteria = oMappedControls
        End If

        If Page.Request.Form(ClientID & "_radio") IsNot Nothing Then
            SelectItem(Page.Request.Form(ClientID & "_radio"))
        End If

    End Sub

    ''' <summary>
    ''' Populates the mapped controls Or Display Controls with the currently selected item from 
    ''' the search results on the basis DisplayControls property is specified 
    ''' in Findcontrol definition or not.
    ''' </summary>
    ''' <param name="v_iSelectedIndex">The index of the currently selected radio button from the FindControl search results</param>
    ''' <remarks></remarks>
    Private Sub SelectItem(ByVal v_iSelectedIndex As Integer)

        Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider

        Try

            oXMLResults = Current.Cache("XMLResults" & ClientID)

            Dim oXMLNode As XmlNode = oXMLResults.SelectSingleNode("Row[" & (v_iSelectedIndex + 1).ToString() & "]")

            If oXMLNode IsNot Nothing Then

                If oXMLNode.HasChildNodes Then

                    Dim i As IEnumerator = oXMLNode.ChildNodes.GetEnumerator

                    While i.MoveNext

                        For Each oItem As FindControlCriteriaItem In oMappedControls
                            If oItem.ObjectName & "__" & oItem.PropertyName = CType(i.Current, XmlNode).Name.ToUpper() Then

                                oItem.Value = CType(i.Current, XmlNode).InnerText
                            End If
                        Next

                    End While

                    sGridViewCallBack = String.Empty

                    Dim iItem As Integer

                    For iItem = 0 To oMappedControls.Count - 1

                        Dim oControl As Control = Parent.FindControl(oMappedControls.Item(iItem).ControlName)

                        Select Case True
                            Case TypeOf oControl Is TextBox
                                CType(oControl, TextBox).Text = oMappedControls.Item(iItem).Value
                            Case TypeOf oControl Is LookupList
                                CType(oControl, LookupList).Value = oMappedControls.Item(iItem).Value
                            Case TypeOf oControl Is HtmlControls.HtmlInputHidden
                                CType(oControl, HtmlControls.HtmlInputHidden).Value = oMappedControls.Item(iItem).Value
                        End Select

                        sGridViewCallBack &= oMappedControls.Item(iItem).Value & ";"

                        'Updated this method to Populate the Diaplay Control with Values fetched from FindControlSearch
                        'SAM method on the basis on Mapped Control, There should be exactly the same number of Mapped control
                        'and Display Controls.Below block will only be executed if DisplayControls property is specified in Find Control.

                        If Not String.IsNullOrEmpty(DisplayControls) Then
                            Dim oDisplayControl As Control = Parent.FindControl(oDisplayControls.Item(iItem).ControlName)
                            Select Case True
                                Case TypeOf oDisplayControl Is TextBox
                                    CType(oDisplayControl, TextBox).Text = oMappedControls.Item(iItem).Value
                                Case TypeOf oDisplayControl Is LookupList
                                    CType(oDisplayControl, LookupList).Value = oMappedControls.Item(iItem).Value
                                Case TypeOf oDisplayControl Is HtmlControls.HtmlInputHidden
                                    CType(oDisplayControl, HtmlControls.HtmlInputHidden).Value = oMappedControls.Item(iItem).Value
                            End Select
                        End If

                    Next
                    If Not String.IsNullOrEmpty(DisplayControls) Then
                        sGridViewCallBack &= sGridViewCallBack
                    End If

                End If

            End If

        Finally
            oWebService = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Breaked down the callback request and carry out the actions define, either search for results or select result
    ''' </summary>
    ''' <param name="eventArgument">XML object containing the search criteria and current selection (if any)</param>
    ''' <remarks>The search criteria will always be passed, even on a result selection call as we need the criteria
    ''' to retrieved the results from SAM (this is cached by search criteria though) as theres no other way to identify
    ''' previous results with the users session</remarks>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        Dim oXML As New XPath.XPathDocument(New XmlTextReader(New System.IO.StringReader(eventArgument)))
        Dim oNav As XPath.XPathNavigator = oXML.CreateNavigator()
        Dim oIterator As XPath.XPathNodeIterator

        'Sorting the column
        oIterator = oNav.Select("/FindControl/SortByColumn")
        While (oIterator.MoveNext())
            DefaultSort = oIterator.Current.InnerXml
            SortByColumn(True)
            '   SelectItem(oIterator.Current.InnerXml)
        End While

        oIterator = oNav.Select("/FindControl/Selection")

        While (oIterator.MoveNext())

            SelectItem(oIterator.Current.InnerXml)

        End While

        If oIterator.Count = 0 Then
            oIterator = oNav.Select("/FindControl/Search")
            While (oIterator.MoveNext())

                If oIterator.Current.MoveToFirstChild() Then

                    Dim i As Integer = 0
                    Dim j As Integer = 0

                    Do
                        If i <= oMappedControls.Count - 1 Then
                            oMappedControls.Item(i).Value = oIterator.Current.InnerXml()
                            i += 1
                        Else
                            If Not String.IsNullOrEmpty(DisplayControls) Then
                                oDisplayControls.Item(j).Value = oIterator.Current.InnerXml()
                                j += 1
                            End If
                        End If


                    Loop While (oIterator.Current.MoveToNext())

                    SearchCriteria = oMappedControls
                    Search(True)
                    'As No sorting Order is supplied , so Every Time a fresh Search is performed 
                    'response will get sorted in Ascending Order by Default
                    sPrevSortColumn = String.Empty
                    bDescSortflag = False
                    SortByColumn()
                End If

            End While

        End If

    End Sub

    ''' <summary>
    ''' Performs a search via SAM, using the current search criteria and displays the results as a list of radio buttons
    ''' </summary>
    ''' <param name="bIsCallBack">Determines whether the method was called from a callback event or serverside</param>
    ''' <remarks></remarks>
    Public Sub Search(Optional ByVal bIsCallBack As Boolean = False, Optional ByVal bSortXML As Boolean = False)

        Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider

        Try
            sGridViewCallBack = "An error occurred"
            If bSortXML = False Then
                oXMLResults = oWebService.FindControlSearch(SearchCriteria, FindControlKey)
                If oXMLResults IsNot Nothing Then
                    Current.Cache.Insert("XMLResults" & ClientID, oXMLResults)
                End If
            Else
                oXMLResults = Current.Cache("XMLResults" & ClientID)
            End If

            If oXMLResults Is Nothing Then
                sGridViewCallBack = "<div style='display:flex;align-items:center;height:100%'>No results found</div>"
            Else
                Dim sbResults As New Text.StringBuilder()
                Dim oXMLNodes As XmlNodeList = oXMLResults.SelectNodes("Row")

                sbResults.Append("<table class=""grid-table"">" & vbCr)

                'To Show the Header
                If bShowHeader = True Then
                    'if column header is provided
                    Dim sColumnHeader() As String = Nothing
                    If sColumnHeaders IsNot Nothing Then
                        sColumnHeader = sColumnHeaders.Split(",")
                    End If

                    sbResults.Append(vbTab & "<tr id='" & ClientID & "_result_" & "Header" & "'>" & vbCr)

                    'For Select Column
                    sbResults.Append(vbTab & vbTab & "<th>")
                    sbResults.Append("Select")
                    sbResults.Append("</th>" & vbCr)

                    'If column header is provided
                    If sColumnHeaders IsNot Nothing Then
                        'if bAllowColumnSorting is True
                        If bAllowColumnSorting = True Then
                            For i As Integer = 0 To sColumnHeader.Length - 1
                                sbResults.Append(vbTab & vbTab & "<th style='cursor:pointer;' id='" & ClientID & "_" & oXMLNodes(0).ChildNodes(i).Name & "" _
                                    & "' onclick=""setTimeout(&quot;" & Page.ClientScript.GetCallbackEventReference(Me,
                                    "'<FindControl><SortByColumn>" & oXMLNodes(0).ChildNodes(i).Name & "</SortByColumn></FindControl>'", "FindControl", "'" & ClientID & "'", False) _
                                    & "&quot;, 1);"">" & vbCr)
                                sbResults.Append(sColumnHeader(i))
                                sbResults.Append("</th>" & vbCr)
                            Next
                        Else
                            'if bAllowColumnSorting is False
                            For i As Integer = 0 To sColumnHeader.Length - 1
                                sbResults.Append(vbTab & vbTab & "<th>")
                                sbResults.Append(sColumnHeader(i))
                                sbResults.Append("</th>" & vbCr)
                            Next
                        End If
                    Else
                        'If column header is not provided
                        'if bAllowColumnSorting is True
                        If bAllowColumnSorting = True Then
                            For i As Integer = 0 To oXMLNodes(0).ChildNodes.Count - 1
                                Dim sColumHeader As String = oXMLNodes(0).ChildNodes(i).Name
                                Dim sName() As String = Regex.Split(sColumHeader, "__")
                                sbResults.Append(vbTab & vbTab & "<th style='cursor:pointer;' id='" & ClientID & "_" & oXMLNodes(0).ChildNodes(i).Name & "" _
                                   & "' onclick=""setTimeout(&quot;" & Page.ClientScript.GetCallbackEventReference(Me,
                                   "'<FindControl><SortByColumn>" & oXMLNodes(0).ChildNodes(i).Name & "</SortByColumn></FindControl>'", "FindControl", "'" & ClientID & "'", False) _
                                   & "&quot;, 1);"">" & vbCr)
                                sbResults.Append(sName(1))
                                sbResults.Append("</th>" & vbCr)
                            Next
                        Else
                            'if bAllowColumnSorting is False
                            For i As Integer = 0 To oXMLNodes(0).ChildNodes.Count - 1
                                Dim sColumHeader As String = oXMLNodes(0).ChildNodes(i).Name
                                Dim sName() As String = Regex.Split(sColumHeader, "__")
                                sbResults.Append(vbTab & vbTab & "<th>")
                                sbResults.Append(sName(1))
                                sbResults.Append("</th>" & vbCr)
                            Next
                        End If
                    End If
                End If
                For x As Integer = 0 To oXMLNodes.Count - 1

                    sbResults.Append(vbTab & "<tr id='" & ClientID & "_result_" & x & "'>" & vbCr)

                    sbResults.Append(vbTab & vbTab & "<td> <span class='asp-radio'><input type='radio' id='" & ClientID & "_radio_" _
                        & x & "' name='" & ClientID & "_radio' value='" & x & "' onclick=""$('#FindResultContainer" & ClientID & "').modal('hide');sSelection_" & ClientID _
                        & " = Select(this);setTimeout(&quot;" & Page.ClientScript.GetCallbackEventReference(Me,
                        "'<FindControl>' + sSearchCritiera_" & ClientID & " + sSelection_" & ClientID _
                        & " + '</FindControl>'", "SelectionPopulate", "oControls_" & ClientID, False) _
                        & "&quot;, 1);"" /> <label></label></span></td>" & vbCr)

                    For i As Integer = 0 To oXMLNodes(x).ChildNodes.Count - 1

                        sbResults.Append(vbTab & vbTab & "<td>")
                        sbResults.Append(oXMLNodes(x).ChildNodes(i).InnerText)
                        sbResults.Append("</td>" & vbCr)

                    Next

                Next

                sbResults.Append("</table>" & vbCr)

                sGridViewCallBack = sbResults.ToString()
                Page.ParseControl(sGridViewCallBack)

                If Not bIsCallBack Then
                    CType(FindControl(ClientID & "_btnSelect"), LinkButton).Visible = True
                End If

            End If

        Catch ex As NexusException

            '272 isn't a failure, just no results found, so change output to show message and continue
            If ex.Errors(0).Code = "272" Then
                sGridViewCallBack = "<div style='display:flex;align-items:center;height:100%'>No results found</div>"
            Else
                'Something else, so throw it back up the stack
                Throw
                sGridViewCallBack = "An error occurred"
            End If

        Finally
            oWebService = Nothing
        End Try

    End Sub
    Sub SortByColumn(Optional ByVal bSort As Boolean = False)
        If bAllowColumnSorting = True AndAlso String.IsNullOrEmpty(sSortByColumn) = False Then
            'Retreiving the Values in XML object
            oXMLResults = Current.Cache("XMLResults" & ClientID)

            'Loading the document after making it well formed
            Dim sXML As String = "<Start><Rows>" & oXMLResults.InnerXml & "</Rows></Start>"
            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(sXML)

            'Make a duplicate copy of the same xml
            Dim xmlDocCopy As New XmlDocument()
            xmlDocCopy.LoadXml(xmlDoc.OuterXml)
            xmlDocCopy.SelectSingleNode("//Rows").RemoveAll()

            'Sorting the rows based on the selection
            Dim node = xmlDoc.SelectSingleNode("//Rows")
            Dim navigator = node.CreateNavigator()
            Dim path As String = "Row/" & sSortByColumn
            Dim selectExpression = navigator.Compile(path)
            'Below code will execute only when user tries to sort by Column after page load
            'Else default Sort Direction - ASC will be implemented
            If bSort Then
                If (String.IsNullOrEmpty(sPrevSortColumn) OrElse sPrevSortColumn = sSortByColumn) AndAlso bDescSortflag = False Then
                    bDescSortflag = True
                Else
                    bDescSortflag = False
                End If
            End If
            sPrevSortColumn = sSortByColumn

            If bDescSortflag Then
                selectExpression.AddSort(".", XmlSortOrder.Descending, XmlCaseOrder.None, "", XmlDataType.Text)
            Else
                selectExpression.AddSort(".", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Text)
            End If
            Dim nodeIterator = navigator.Select(selectExpression)


            Dim iCount As Integer = 0
            While nodeIterator.MoveNext()
                Dim linkNode As XmlNode
                linkNode = xmlDoc.SelectSingleNode("//Row[" & sSortByColumn & "='" & nodeIterator.Current.Value & "']")
                Dim importedLinkNode = xmlDocCopy.ImportNode(linkNode, True)
                xmlDocCopy.SelectSingleNode("//Rows").AppendChild(importedLinkNode)
                xmlDoc.SelectSingleNode("//Row[" & sSortByColumn & "='" & nodeIterator.Current.Value & "']").RemoveAll()
            End While

            'sPrevSortColumn = sSortByColumn
            'If sPrevSortColumn = sSortByColumn And bDescSortflag = False Then
            '    bDescSortflag = True
            'Else
            '    bDescSortflag = False
            'End If
            'Writing back the sorting data
            oXMLResults = xmlDocCopy.FirstChild.FirstChild
            Current.Cache("XMLResults" & ClientID) = oXMLResults
            'sending the sorting data to the search method
            Search(True, True)
        End If
    End Sub
End Class
