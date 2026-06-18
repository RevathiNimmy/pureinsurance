Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml.Linq
Imports System.IO
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants
Imports System.Xml

Namespace Nexus

    Partial Class FIRE_FIREFIREF : Inherits BaseRisk

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)

            hvAgreedRateFinal.Value = GetValue("GROUP_FIRE", "FINAL_RATE_AGREED_RATE")
            Dim nSi As Integer = 0
            Dim nPre As Integer = 0

            If FIRE__RD1_MULTIPLE_BUILDINGS.Checked = True Then
                If FIRE__IPF.Rows.Count > 0 Then

                    For Each gridRow In FIRE__IPF.Rows
                        nSi = nSi + Convert.ToDecimal((gridRow.Cells(1).Text()))
                        nPre = nPre + Convert.ToDecimal((gridRow.Cells(2).Text()))
                    Next
                    FIRE__RD1_SI.Text = nSi
                    FIRE__RD1_PREMIUM.Text = nPre
                    FIRE__RD1_RATE.Text = (nPre * 100) / nSi

                    hvmultibulpre.Value = nPre
                    hvmultibulsi.Value = nSi
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "calculate_builder('FIRE__RD1_SI');", True)
                Else
                    hvmultibulpre.Value = ""
                    hvmultibulsi.Value = ""
                End If

            End If

            If (GetValue("FIRE", "RD1_MULTIPLE_BUILDINGS") = "1") Then
                SetChildValuesAtParent()
            End If

        End Sub

        Private Sub SetChildValuesAtParent()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim dBuildingSITotal As Decimal = 0
            Dim dInfEscTotal As Decimal = 0
            Dim sXML As String = oQuote.Risks(0).XMLDataset
            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.LoadXml(sXML)
            Dim oNodeList As XmlNodeList
            'Filtering the XML with Child Node
            oNodeList = xmlDoc.SelectNodes("//FIRE/IPF")
            Dim iCounter As Integer = 0
            If oNodeList IsNot Nothing And oNodeList.Count > 0 Then
                Dim dBuildingsSI As Decimal = 0
                Dim dEscalationSI As Decimal = 0
                Dim dInflationY1SI As Decimal = 0
                Dim dInflationY2SI As Decimal = 0
                For Each oNode As XmlNode In oNodeList
                    dBuildingsSI = IIf(oNode.Attributes("IPARTIES_ITEM_SI") IsNot Nothing, Convert.ToDecimal(oNode.Attributes("IPARTIES_ITEM_SI").Value), 0)
                    If (oNode.Attributes("IPARTIES_INFLATION_ESCALATION") IsNot Nothing AndAlso oNode.Attributes("IPARTIES_INFLATION_ESCALATION").Value = "1") Then
                        If (oNode.Attributes("IPARTIES_ESCALATION_SI") IsNot Nothing) Then
                            dEscalationSI = IIf(oNode.Attributes("IPARTIES_ESCALATION_SI") IsNot Nothing, Convert.ToDecimal(oNode.Attributes("IPARTIES_ESCALATION_SI").Value), 0)
                        End If

                        If (oNode.Attributes("IPARTIES_INFLATION_Y1_SI") IsNot Nothing) Then
                            dInflationY1SI = IIf(oNode.Attributes("IPARTIES_INFLATION_Y1_SI") IsNot Nothing, Convert.ToDecimal(oNode.Attributes("IPARTIES_INFLATION_Y1_SI").Value), 0)
                        End If
                        If (oNode.Attributes("IPARTIES_INFLATION_Y2_SI") IsNot Nothing) Then
                            dInflationY2SI = IIf(oNode.Attributes("IPARTIES_INFLATION_Y2_SI") IsNot Nothing, Convert.ToDecimal(oNode.Attributes("IPARTIES_INFLATION_Y2_SI").Value), 0)
                        End If




                    End If
                    dBuildingSITotal = dBuildingSITotal + dBuildingsSI
                    dInfEscTotal = dInfEscTotal + dEscalationSI + dInflationY1SI + dInflationY2SI

                Next
            End If
            hvBuildingTotalSI.Value = dBuildingSITotal
            hvInfEscTotalSI.Value = dInfEscTotal
        End Sub

        Public Overrides Sub PostDataSetWrite()
        End Sub

        Public Overrides Sub PreDataSetWrite()
        End Sub

    End Class

End Namespace
		