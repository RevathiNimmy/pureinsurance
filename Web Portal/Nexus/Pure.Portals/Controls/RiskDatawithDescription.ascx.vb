Imports System.Xml
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_RiskDatawithDescription : Inherits System.Web.UI.UserControl

        Dim NavString, NodeString, AttribString, DataTypeString, ReturnString, ReturnString1, ReturnString2 As String
        Dim ShowTableHeaderRowString, TableRowHeadersString, CssClass, SeparatorString, TableColumnCSSString, TableLastRowCSSString As String
        Dim AttribNames, HeaderRowNames, ColumnCSSNames As String()
        Dim bPullMissingValueFromRating As Boolean = False
        Dim iDigitAfterDecimal As Integer = 0

        Public WriteOnly Property NodePath() As String
            Set(ByVal value As String)
                NodeString = value
            End Set
        End Property

        Public WriteOnly Property AttributeName() As String
            Set(ByVal value As String)
                AttribString = value
            End Set
        End Property

        Public WriteOnly Property DataType() As String
            Set(ByVal value As String)
                DataTypeString = value
            End Set
        End Property

        Public WriteOnly Property ShowTableHeaderRow() As String
            Set(ByVal value As String)
                ShowTableHeaderRowString = value
            End Set
        End Property

        Public WriteOnly Property TableCss() As String
            Set(ByVal value As String)
                CssClass = value
            End Set
        End Property

        Public WriteOnly Property TableRowHeaders() As String
            Set(ByVal value As String)
                TableRowHeadersString = value
            End Set
        End Property

        Public WriteOnly Property Separator() As String
            Set(ByVal value As String)
                SeparatorString = value
            End Set
        End Property

        Public WriteOnly Property TableColumnCSS() As String
            Set(ByVal value As String)
                TableColumnCSSString = value
            End Set
        End Property

        Public WriteOnly Property TableLastRowCSS() As String
            Set(ByVal value As String)
                TableLastRowCSSString = value
            End Set
        End Property

        Public Property PullMissingValueFromRating() As Boolean
            Get
                Return bPullMissingValueFromRating
            End Get
            Set(ByVal value As Boolean)
                bPullMissingValueFromRating = value
            End Set
        End Property

        Public WriteOnly Property DigitAfterDecimal() As Integer
            Set(ByVal value As Integer)
                iDigitAfterDecimal = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Session(CNQuote) IsNot Nothing Then

                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim DataModelCode As String = Session(CNDataModelCode)
                Dim Navigator As System.Xml.XPath.XPathNavigator
                Dim strDataset As New System.IO.StringReader(oQuote.Risks(0).XMLDataset)
                Dim InsuranceFileKey As String = oQuote.InsuranceFileKey
                Dim RiskKey As String = oQuote.Risks(0).Key
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oRatingCollection As NexusProvider.RatingCollection
                Try
                    oRatingCollection = oWebService.GetRatingDetails(RiskKey, InsuranceFileKey)
                Finally
                    oWebService = Nothing
                End Try
                'oQuote = Nothing

                Dim trDataset As New XmlTextReader(strDataset)
                Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)

                Navigator = Doc.CreateNavigator()

                If (DataTypeString = "Single") Then
                    ''no use of single line in this control
                ElseIf (DataTypeString = "Table") Then

                    Dim NoOfAttributes As Integer = 0, TempVar As Integer = 0, TempVar1 As Integer = 0
                    Dim TotalNoOfRecords As Integer = 0, TotalPremium As Double = 0.0

                    ReturnString = "<table class='" & CssClass & "'>"

                    If (ShowTableHeaderRowString = "True") Then
                        HeaderRowNames = TableRowHeadersString.Split(",")
                        ColumnCSSNames = TableColumnCSSString.Split(",")
                        ReturnString += "<tr>"
                        For TempVar = LBound(HeaderRowNames) To UBound(HeaderRowNames)
                            ReturnString += "<th class='" & ColumnCSSNames(TempVar) & "'>" & HeaderRowNames(TempVar) & "</th>"
                        Next
                        ReturnString += "</tr>"
                    End If

                    AttribNames = AttribString.Split(",")
                    NoOfAttributes = UBound(AttribNames)
                    Dim RT(NoOfAttributes, 1000) As String


                    For TempVar = LBound(AttribNames) To UBound(AttribNames)
                        TotalPremium = 0
                        NavString = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & NodeString & "/@" & AttribNames(TempVar)
                        Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                        'if xml does not have matching node then values are pulled from Rating collection
                        If NodeI.Count = 0 And PullMissingValueFromRating = True Then
                            Dim i As Integer
                            For i = 0 To oRatingCollection.Count - 1 Step 1
                                TempVar1 += 1
                                RT(TempVar, TempVar1) = oRatingCollection.GetValue(TempVar1 - 1, AttribNames(TempVar))
                            Next
                        End If
                        While NodeI.MoveNext
                            TempVar1 += 1
                            If AttribNames(TempVar) = "RISK_RATING_SECTION" Then
                                RT(TempVar, TempVar1) = oRatingCollection.Item(TempVar1 - 1).RatingSectionType.ToString()
                            Else
                                RT(TempVar, TempVar1) = NodeI.Current.Value
                                'This is to ROund the Values to Two Decimal 
                                If IsNumeric(RT(TempVar, TempVar1)) Then
                                    Dim current As Double = 0.0
                                    current = RT(TempVar, TempVar1)
                                    RT(TempVar, TempVar1) = FormatNumber(current, iDigitAfterDecimal)
                                End If
                            End If
                            If AttribNames(TempVar).Trim = "PREMIUM" Then
                                'PN 42457 
                                TotalPremium = TotalPremium + NodeI.Current.Value
                                'check here for ROunding the Premium
                                TotalPremium = FormatNumber(TotalPremium, iDigitAfterDecimal)
                            End If
                        End While
                        TotalNoOfRecords = TempVar1
                        TempVar1 = 0
                    Next


                    For TempVar = 1 To TotalNoOfRecords
                        ReturnString += "<tr>"
                        For TempVar1 = 0 To NoOfAttributes
                            ReturnString += "<td>" & RT(TempVar1, TempVar) & "</td>"
                        Next
                        ReturnString += "</tr>"
                    Next

                    ReturnString += "<tr class='" & TableLastRowCSSString & "'>" ''adding last line to show the total
                    ReturnString += "<td colspan='" & UBound(AttribNames) & "' align='right'>" & GetLocalResourceObject("lbl_totalmsg") & "</td>"
                    ReturnString += "<td>" & FormatNumber(TotalPremium, iDigitAfterDecimal) & "</td>"
                    ReturnString += "</tr>"

                    ReturnString += "</table>"
                    lblOutput.Text = ReturnString
                ElseIf (DataTypeString = "MultiLine") Then

                    Dim NoOfAttributes As Integer = 0, TempVar As Integer = 0, TempVar1 As Integer = 0
                    Dim TotalNoOfRecords As Integer = 0, TempString As String = "", TotalPremium As Double = 0

                    AttribNames = AttribString.Split(",")
                    NoOfAttributes = UBound(AttribNames)
                    Dim RT(NoOfAttributes, 1000) As String

                    For TempVar = LBound(AttribNames) To UBound(AttribNames)
                        NavString = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & NodeString & "/@" & AttribNames(TempVar)
                        Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                        'if xml does not have matching node then values are pulled from Rating collection
                        If NodeI.Count = 0 And PullMissingValueFromRating = True Then
                            Dim i As Integer
                            For i = 0 To oRatingCollection.Count - 1 Step 1
                                TempVar1 += 1
                                RT(TempVar, TempVar1) = oRatingCollection.GetValue(TempVar1 - 1, AttribNames(TempVar))
                            Next
                        End If
                        While NodeI.MoveNext
                            TempVar1 += 1
                            If AttribNames(TempVar) = "RISK_RATING_SECTION" Then
                                RT(TempVar, TempVar1) = oRatingCollection.Item(TempVar1 - 1).RatingSectionType.ToString()
                            Else
                                RT(TempVar, TempVar1) = NodeI.Current.Value
                                If IsNumeric(RT(TempVar, TempVar1)) Then
                                    Dim current As Double = 0.0
                                    current = RT(TempVar, TempVar1)
                                    RT(TempVar, TempVar1) = FormatNumber(current, iDigitAfterDecimal)
                                End If
                            End If
                            If AttribNames(TempVar).Trim = "PREMIUM" Then
                                TotalPremium = TotalPremium + NodeI.Current.Value
                                'check here for ROunding the Premium
                                TotalPremium = FormatNumber(TotalPremium, iDigitAfterDecimal)
                            End If
                        End While
                        TotalNoOfRecords = TempVar1
                        TempVar1 = 0
                    Next
                    'check here for ROunding the Premium
                    TotalPremium = FormatNumber(TotalPremium, iDigitAfterDecimal)

                    For TempVar = 1 To TotalNoOfRecords
                        For TempVar1 = 0 To NoOfAttributes
                            'ReturnString += "<td>" & RT(TempVar1, TempVar) & "</td>"
                            TempString = RT(TempVar1, TempVar)
                            If ((TempString.ToString.Trim <> "") And (TempVar1 <> NoOfAttributes)) Then
                                ReturnString += TempString & SeparatorString
                            Else
                                ReturnString += TempString
                            End If
                        Next
                        ReturnString += "<br/>"
                    Next

                    ReturnString += "<br/>" & GetLocalResourceObject("lbl_totalmsg") & "" & FormatNumber(TotalPremium, iDigitAfterDecimal)  ''adding last line to show the total"



                    lblOutput.Text = ReturnString
                End If
            End If

        End Sub

    End Class
End Namespace