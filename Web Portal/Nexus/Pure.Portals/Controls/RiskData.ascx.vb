Imports System.Xml
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class Controls_RiskData : Inherits System.Web.UI.UserControl

        Dim NavString, NodeString, AttribString, DataTypeString, ReturnString, ReturnString1, ReturnString2 As String
        Dim ShowTableHeaderRowString, TableRowHeadersString, CssClass, SeparatorString, TableColumnCSSString, TableColumnHeaderCSSString As String
        Dim AttribNames, HeaderRowNames, ColumnCSSNames, ColumnHeaderCSSNames As String()
        Dim iDigitAfterDecimal As Integer = 0
        Dim bShowCurrencySymbol As Boolean = False

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

        Public WriteOnly Property TableColumnHeaderCSS() As String
            Set(ByVal value As String)
                TableColumnHeaderCSSString = value
            End Set
        End Property

        Public WriteOnly Property DigitAfterDecimal() As Integer
            Set(ByVal value As Integer)
                iDigitAfterDecimal = value
            End Set
        End Property
        Public Property ShowCurrencySymbol() As Boolean
            Get
                Return bShowCurrencySymbol
            End Get
            Set(ByVal value As Boolean)
                bShowCurrencySymbol = value
            End Set
        End Property
        Public WriteOnly Property TableColumnCSS() As String
            Set(ByVal value As String)
                TableColumnCSSString = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Session(CNQuote) IsNot Nothing Then
                If CType(Session(CNQuote), NexusProvider.Quote).Risks.Count > 0 Then
                    If CType(Session(CNQuote), NexusProvider.Quote).Risks(0).XMLDataset IsNot Nothing AndAlso CType(Session(CNQuote), NexusProvider.Quote).Risks(0).XMLDataset <> "" Then
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        Dim DataModelCode As String = Session(CNDataModelCode)
                        Dim Navigator As System.Xml.XPath.XPathNavigator
                        Dim strDataset As New System.IO.StringReader(oQuote.Risks(0).XMLDataset)
                        'Not to Clear the OQuote Object.
                        'oQuote = Nothing

                        Dim trDataset As New XmlTextReader(strDataset)
                        Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)

                        Navigator = Doc.CreateNavigator()

                        If (DataTypeString = "Single") Then
                            NavString = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & NodeString & "/@" & AttribString
                            Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                            While NodeI.MoveNext
                            End While
                            If IsNumeric(NodeI.Current.Value) Then
                                lblOutput.Text = FormatNumber(NodeI.Current.Value, iDigitAfterDecimal)
                            Else
                                lblOutput.Text = NodeI.Current.Value
                            End If
                        ElseIf (DataTypeString = "Table") Then

                            Dim NoOfAttributes As Integer = 0, TempVar As Integer = 0, TempVar1 As Integer = 0, TotalNoOfRecords As Integer = 0

                            ReturnString = "<table class='" & CssClass & "'>"

                            If (ShowTableHeaderRowString = "True") Then
                                HeaderRowNames = TableRowHeadersString.Split(",")
                                ColumnHeaderCSSNames = TableColumnHeaderCSSString.Split(",")
                                ReturnString += "<tr>"
                                For TempVar = LBound(HeaderRowNames) To UBound(HeaderRowNames)
                                    ReturnString += "<th class='" & ColumnHeaderCSSNames(TempVar) & "'>" & HeaderRowNames(TempVar) & "</th>"
                                Next
                                ReturnString += "</tr>"
                            End If

                            AttribNames = AttribString.Split(",")
                            NoOfAttributes = UBound(AttribNames)
                            Dim RT(NoOfAttributes, 1000) As String

                            For TempVar = LBound(AttribNames) To UBound(AttribNames)
                                NavString = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & NodeString & "/@" & AttribNames(TempVar)
                                Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                                While NodeI.MoveNext
                                    TempVar1 += 1
                                    RT(TempVar, TempVar1) = NodeI.Current.Value
                                    If IsNumeric(RT(TempVar, TempVar1)) Then
                                        Dim current As Double = 0.0
                                        current = RT(TempVar, TempVar1)
                                        RT(TempVar, TempVar1) = FormatNumber(current, iDigitAfterDecimal)
                                    End If
                                End While
                                TotalNoOfRecords = TempVar1
                                TempVar1 = 0
                            Next

                            For TempVar = 1 To TotalNoOfRecords
                                ReturnString += "<tr>"
                                ColumnCSSNames = TableColumnCSSString.Split(",")
                                For TempVar1 = 0 To NoOfAttributes
                                    If ShowCurrencySymbol = True And IsNumeric(RT(TempVar1, TempVar)) Then
                                        ReturnString += "<td class='" & ColumnCSSNames(TempVar1) & "'>" & New Money(RT(TempVar1, TempVar), Session(CNCurrenyCode)).Formatted & "</td>"
                                    Else
                                        ReturnString += "<td class='" & ColumnCSSNames(TempVar1) & "'>" & RT(TempVar1, TempVar) & "</td>"
                                    End If
                                Next
                                ReturnString += "</tr>"
                            Next

                            ReturnString += "</table>"
                            lblOutput.Text = ReturnString
                        ElseIf (DataTypeString = "MultiLine") Then

                            Dim NoOfAttributes As Integer = 0, TempVar As Integer = 0, TempVar1 As Integer = 0, TotalNoOfRecords As Integer = 0, TempString As String = ""

                            AttribNames = AttribString.Split(",")
                            NoOfAttributes = UBound(AttribNames)
                            Dim RT(NoOfAttributes, 1000) As String

                            For TempVar = LBound(AttribNames) To UBound(AttribNames)
                                NavString = "DATA_SET/RISK_OBJECTS/" & DataModelCode & "_POLICY_BINDER/" & NodeString & "/@" & AttribNames(TempVar)
                                Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                                While NodeI.MoveNext
                                    TempVar1 += 1
                                    RT(TempVar, TempVar1) = NodeI.Current.Value
                                    If IsNumeric(RT(TempVar, TempVar1)) Then
                                        Dim current As Double = 0.0
                                        current = RT(TempVar, TempVar1)
                                        RT(TempVar, TempVar1) = FormatNumber(current, iDigitAfterDecimal)
                                    End If
                                End While
                                TotalNoOfRecords = TempVar1
                                TempVar1 = 0
                            Next

                            For TempVar = 1 To TotalNoOfRecords
                                For TempVar1 = 0 To NoOfAttributes
                                    'ReturnString += "<td>" & RT(TempVar1, TempVar) & "</td>"
                                    TempString = RT(TempVar1, TempVar)
                                    If ((TempString.ToString.Trim <> "") And (TempVar1 <> NoOfAttributes)) Then
                                        If ShowCurrencySymbol = True And IsNumeric(TempString) Then
                                            ReturnString += New Money(TempString, Session(CNCurrenyCode)).Formatted & SeparatorString
                                        Else
                                            ReturnString += TempString & SeparatorString
                                        End If
                                    Else
                                        If ShowCurrencySymbol = True And IsNumeric(TempString) Then
                                            ReturnString += New Money(TempString, Session(CNCurrenyCode)).Formatted
                                        Else
                                            ReturnString += TempString
                                        End If
                                    End If
                                Next
                                ReturnString += "<br/>"
                            Next

                            lblOutput.Text = ReturnString
                        End If
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace