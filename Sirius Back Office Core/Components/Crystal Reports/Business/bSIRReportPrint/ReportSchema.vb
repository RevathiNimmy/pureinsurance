Imports System.Xml.Serialization
Imports System.Collections.Generic

<XmlRoot(ElementName:="ConnectionProperties")>
Public Class ConnectionProperties

    <XmlElement(ElementName:="DataProvider")>
    Public Property DataProvider As String

    <XmlElement(ElementName:="ConnectString")>
    Public Property ConnectString As String

    <XmlElement(ElementName:="IntegratedSecurity")>
    Public Property IntegratedSecurity As Boolean
End Class

<XmlRoot(ElementName:="DataSource")>
Public Class DataSource

    <XmlElement(ElementName:="ConnectionProperties")>
    Public Property ConnectionProperties As ConnectionProperties

    <XmlElement(ElementName:="DataSourceID", [Namespace]:="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")>
    Public Property DataSourceID As String

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="DataSources")>
Public Class DataSources

    <XmlElement(ElementName:="DataSource")>
    Public Property DataSource As DataSource
End Class

<XmlRoot(ElementName:="Field")>
Public Class Field

    <XmlElement(ElementName:="DataField")>
    Public Property DataField As String

    <XmlElement(ElementName:="TypeName", [Namespace]:="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")>
    Public Property TypeName As String

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="Fields")>
Public Class Fields

    <XmlElement(ElementName:="Field")>
    Public Property Field As List(Of Field)
End Class

<XmlRoot(ElementName:="QueryParameter")>
Public Class QueryParameter

    <XmlElement(ElementName:="Value")>
    Public Property Value As String

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="QueryParameters")>
Public Class QueryParameters

    <XmlElement(ElementName:="QueryParameter")>
    Public Property QueryParameter As List(Of QueryParameter)
End Class

<XmlRoot(ElementName:="Query")>
Public Class Query

    <XmlElement(ElementName:="DataSourceName")>
    Public Property DataSourceName As String

    <XmlElement(ElementName:="CommandType")>
    Public Property CommandType As String

    <XmlElement(ElementName:="CommandText")>
    Public Property CommandText As String

    <XmlElement(ElementName:="QueryParameters")>
    Public Property QueryParameters As QueryParameters
End Class

<XmlRoot(ElementName:="DataSet")>
Public Class DataSet

    <XmlElement(ElementName:="Fields")>
    Public Property Fields As Fields

    <XmlElement(ElementName:="Query")>
    Public Property Query As Query

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="DataSets")>
Public Class DataSets

    <XmlElement(ElementName:="DataSet")>
    Public Property DataSet As DataSet
End Class

<XmlRoot(ElementName:="ReportParameter")>
Public Class ReportParameter

    <XmlElement(ElementName:="DataType")>
    Public Property DataType As String

    <XmlElement(ElementName:="Prompt")>
    Public Property Prompt As String

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String

    <XmlElement(ElementName:="ValidValues")>
    Public Property ValidValues As ValidValues

    <XmlElement(ElementName:="DefaultValue")>
    Public Property DefaultValue As DefaultValue
End Class

<XmlRoot(ElementName:="ParameterValue")>
Public Class ParameterValue

    <XmlElement(ElementName:="Value")>
    Public Property Value As String
End Class

<XmlRoot(ElementName:="ParameterValues")>
Public Class ParameterValues

    <XmlElement(ElementName:="ParameterValue")>
    Public Property ParameterValue As List(Of ParameterValue)
End Class

<XmlRoot(ElementName:="ValidValues")>
Public Class ValidValues

    <XmlElement(ElementName:="ParameterValues")>
    Public Property ParameterValues As ParameterValues
End Class

<XmlRoot(ElementName:="Values")>
Public Class Values

    <XmlElement(ElementName:="Value")>
    Public Property Value As String
End Class

<XmlRoot(ElementName:="DefaultValue")>
Public Class DefaultValue

    <XmlElement(ElementName:="Values")>
    Public Property Values As Values
End Class

<XmlRoot(ElementName:="ReportParameters")>
Public Class ReportParameters

    <XmlElement(ElementName:="ReportParameter")>
    Public Property ReportParameter As List(Of ReportParameter)
End Class

<XmlRoot(ElementName:="Style")>
Public Class Style

    <XmlElement(ElementName:="FontFamily")>
    Public Property FontFamily As String

    <XmlElement(ElementName:="FontSize")>
    Public Property FontSize As String

    <XmlElement(ElementName:="Color")>
    Public Property Color As String

    <XmlElement(ElementName:="FontWeight")>
    Public Property FontWeight As String

    <XmlElement(ElementName:="TextAlign")>
    Public Property TextAlign As String

    <XmlElement(ElementName:="Border")>
    Public Property Border As Border

    <XmlElement(ElementName:="LeftBorder")>
    Public Property LeftBorder As LeftBorder

    <XmlElement(ElementName:="RightBorder")>
    Public Property RightBorder As RightBorder

    <XmlElement(ElementName:="TopBorder")>
    Public Property TopBorder As TopBorder

    <XmlElement(ElementName:="BottomBorder")>
    Public Property BottomBorder As BottomBorder

    <XmlElement(ElementName:="Format")>
    Public Property Format As String

    <XmlElement(ElementName:="BackgroundColor")>
    Public Property BackgroundColor As String

    <XmlElement(ElementName:="TextDecoration")>
    Public Property TextDecoration As String
End Class

<XmlRoot(ElementName:="TextRun")>
Public Class TextRun

    <XmlElement(ElementName:="Value")>
    Public Property Value As String

    <XmlElement(ElementName:="Style")>
    Public Property Style As Style
End Class

<XmlRoot(ElementName:="TextRuns")>
Public Class TextRuns

    <XmlElement(ElementName:="TextRun")>
    Public Property TextRun As List(Of TextRun)
End Class

<XmlRoot(ElementName:="Paragraph")>
Public Class Paragraph

    <XmlElement(ElementName:="TextRuns")>
    Public Property TextRuns As TextRuns

    <XmlElement(ElementName:="Style")>
    Public Property Style As Style
End Class

<XmlRoot(ElementName:="Paragraphs")>
Public Class Paragraphs

    <XmlElement(ElementName:="Paragraph")>
    Public Property Paragraph As Paragraph
End Class

<XmlRoot(ElementName:="Border")>
Public Class Border

    <XmlElement(ElementName:="Color")>
    Public Property Color As String

    <XmlElement(ElementName:="Style")>
    Public Property Style As String

    <XmlElement(ElementName:="Width")>
    Public Property Width As String
End Class

<XmlRoot(ElementName:="LeftBorder")>
Public Class LeftBorder

    <XmlElement(ElementName:="Style")>
    Public Property Style As String
End Class

<XmlRoot(ElementName:="RightBorder")>
Public Class RightBorder

    <XmlElement(ElementName:="Style")>
    Public Property Style As String
End Class

<XmlRoot(ElementName:="TopBorder")>
Public Class TopBorder

    <XmlElement(ElementName:="Style")>
    Public Property Style As String
End Class

<XmlRoot(ElementName:="BottomBorder")>
Public Class BottomBorder

    <XmlElement(ElementName:="Style")>
    Public Property Style As String
End Class

<XmlRoot(ElementName:="Textbox")>
Public Class Textbox

    <XmlElement(ElementName:="Paragraphs")>
    Public Property Paragraphs As Paragraphs

    <XmlElement(ElementName:="Style")>
    Public Property Style As Style

    <XmlElement(ElementName:="CanGrow")>
    Public Property CanGrow As Boolean

    <XmlElement(ElementName:="KeepTogether")>
    Public Property KeepTogether As Boolean

    <XmlElement(ElementName:="Top")>
    Public Property Top As String

    <XmlElement(ElementName:="Left")>
    Public Property Left As String

    <XmlElement(ElementName:="Height")>
    Public Property Height As String

    <XmlElement(ElementName:="Width")>
    Public Property Width As String

    <XmlElement(ElementName:="ZIndex")>
    Public Property ZIndex As Integer

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="ReportItems")>
Public Class ReportItems

    <XmlElement(ElementName:="Textbox")>
    Public Property Textbox As List(Of Textbox)

    <XmlElement(ElementName:="Rectangle")>
    Public Property Rectangle As Rectangle

    <XmlElement(ElementName:="Subreport")>
    Public Property Subreport As List(Of Subreport)

    <XmlElement(ElementName:="Tablix")>
    Public Property Tablix As Tablix
End Class

<XmlRoot(ElementName:="PageHeader")>
Public Class PageHeader

    <XmlElement(ElementName:="PrintOnFirstPage")>
    Public Property PrintOnFirstPage As Boolean

    <XmlElement(ElementName:="PrintOnLastPage")>
    Public Property PrintOnLastPage As Boolean

    <XmlElement(ElementName:="ReportItems")>
    Public Property ReportItems As ReportItems

    <XmlElement(ElementName:="Height")>
    Public Property Height As String
End Class

<XmlRoot(ElementName:="PageFooter")>
Public Class PageFooter

    <XmlElement(ElementName:="PrintOnFirstPage")>
    Public Property PrintOnFirstPage As Boolean

    <XmlElement(ElementName:="PrintOnLastPage")>
    Public Property PrintOnLastPage As Boolean

    <XmlElement(ElementName:="ReportItems")>
    Public Property ReportItems As ReportItems

    <XmlElement(ElementName:="Height")>
    Public Property Height As String
End Class

<XmlRoot(ElementName:="Page")>
Public Class Page

    <XmlElement(ElementName:="PageHeader")>
    Public Property PageHeader As PageHeader

    <XmlElement(ElementName:="PageFooter")>
    Public Property PageFooter As PageFooter

    <XmlElement(ElementName:="PageHeight")>
    Public Property PageHeight As String

    <XmlElement(ElementName:="PageWidth")>
    Public Property PageWidth As String

    <XmlElement(ElementName:="LeftMargin")>
    Public Property LeftMargin As String

    <XmlElement(ElementName:="RightMargin")>
    Public Property RightMargin As String

    <XmlElement(ElementName:="TopMargin")>
    Public Property TopMargin As String

    <XmlElement(ElementName:="BottomMargin")>
    Public Property BottomMargin As String
End Class

<XmlRoot(ElementName:="TablixColumn")>
Public Class TablixColumn

    <XmlElement(ElementName:="Width")>
    Public Property Width As String
End Class

<XmlRoot(ElementName:="TablixColumns")>
Public Class TablixColumns

    <XmlElement(ElementName:="TablixColumn")>
    Public Property TablixColumn As TablixColumn
End Class

<XmlRoot(ElementName:="Rectangle")>
Public Class Rectangle

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlElement(ElementName:="KeepTogether")>
    Public Property KeepTogether As Boolean

    <XmlElement(ElementName:="ReportItems")>
    Public Property ReportItems As ReportItems

    <XmlElement(ElementName:="Style")>
    Public Property Style As Style

    <XmlElement(ElementName:="Top")>
    Public Property Top As String

    <XmlElement(ElementName:="Left")>
    Public Property Left As String

    <XmlElement(ElementName:="Height")>
    Public Property Height As String

    <XmlElement(ElementName:="Width")>
    Public Property Width As String

    <XmlElement(ElementName:="ZIndex")>
    Public Property ZIndex As Integer

    <XmlText>
    Public Property Text As String

    <XmlElement(ElementName:="PageBreak")>
    Public Property PageBreak As PageBreak
End Class

<XmlRoot(ElementName:="CellContents")>
Public Class CellContents

    <XmlElement(ElementName:="Rectangle")>
    Public Property Rectangle As Rectangle
End Class

<XmlRoot(ElementName:="TablixCell")>
Public Class TablixCell

    <XmlElement(ElementName:="CellContents")>
    Public Property CellContents As CellContents
End Class

<XmlRoot(ElementName:="TablixCells")>
Public Class TablixCells

    <XmlElement(ElementName:="TablixCell")>
    Public Property TablixCell As TablixCell
End Class

<XmlRoot(ElementName:="TablixRow")>
Public Class TablixRow

    <XmlElement(ElementName:="Height")>
    Public Property Height As String

    <XmlElement(ElementName:="TablixCells")>
    Public Property TablixCells As TablixCells
End Class

<XmlRoot(ElementName:="PageBreak")>
Public Class PageBreak

    <XmlElement(ElementName:="BreakLocation")>
    Public Property BreakLocation As String
End Class

<XmlRoot(ElementName:="Parameter")>
Public Class Parameter

    <XmlElement(ElementName:="Value")>
    Public Property Value As String

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="Parameters")>
Public Class Parameters

    <XmlElement(ElementName:="Parameter")>
    Public Property Parameter As Parameter
End Class

<XmlRoot(ElementName:="Subreport")>
Public Class Subreport

    <XmlElement(ElementName:="ReportName")>
    Public Property ReportName As String

    <XmlElement(ElementName:="Top")>
    Public Property Top As String

    <XmlElement(ElementName:="Height")>
    Public Property Height As String

    <XmlElement(ElementName:="Width")>
    Public Property Width As String

    <XmlElement(ElementName:="ZIndex")>
    Public Property ZIndex As Integer

    <XmlElement(ElementName:="Parameters")>
    Public Property Parameters As Parameters

    <XmlElement(ElementName:="Style")>
    Public Property Style As Style

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String

    <XmlElement(ElementName:="Left")>
    Public Property Left As String
End Class

<XmlRoot(ElementName:="TablixRows")>
Public Class TablixRows

    <XmlElement(ElementName:="TablixRow")>
    Public Property TablixRow As List(Of TablixRow)
End Class

<XmlRoot(ElementName:="TablixBody")>
Public Class TablixBody

    <XmlElement(ElementName:="TablixColumns")>
    Public Property TablixColumns As TablixColumns

    <XmlElement(ElementName:="TablixRows")>
    Public Property TablixRows As TablixRows
End Class

<XmlRoot(ElementName:="TablixMembers")>
Public Class TablixMembers

    <XmlElement(ElementName:="TablixMember")>
    Public Property TablixMember As List(Of TablixMember)
End Class

<XmlRoot(ElementName:="TablixColumnHierarchy")>
Public Class TablixColumnHierarchy

    <XmlElement(ElementName:="TablixMembers")>
    Public Property TablixMembers As TablixMembers
End Class

<XmlRoot(ElementName:="GroupExpressions")>
Public Class GroupExpressions

    <XmlElement(ElementName:="GroupExpression")>
    Public Property GroupExpression As String
End Class

<XmlRoot(ElementName:="Group")>
Public Class Group

    <XmlElement(ElementName:="GroupExpressions")>
    Public Property GroupExpressions As GroupExpressions

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String

    <XmlElement(ElementName:="PageBreak")>
    Public Property PageBreak As PageBreak
End Class

<XmlRoot(ElementName:="SortExpression")>
Public Class SortExpression

    <XmlElement(ElementName:="Value")>
    Public Property Value As String
End Class

<XmlRoot(ElementName:="SortExpressions")>
Public Class SortExpressions

    <XmlElement(ElementName:="SortExpression")>
    Public Property SortExpression As SortExpression
End Class

<XmlRoot(ElementName:="Visibility")>
Public Class Visibility

    <XmlElement(ElementName:="Hidden")>
    Public Property Hidden As Boolean
End Class

<XmlRoot(ElementName:="TablixMember")>
Public Class TablixMember

    <XmlElement(ElementName:="Visibility")>
    Public Property Visibility As Visibility

    <XmlElement(ElementName:="KeepWithGroup")>
    Public Property KeepWithGroup As String

    <XmlElement(ElementName:="Group")>
    Public Property Group As Group

    <XmlElement(ElementName:="TablixMembers")>
    Public Property TablixMembers As TablixMembers

    <XmlElement(ElementName:="SortExpressions")>
    Public Property SortExpressions As SortExpressions
End Class

<XmlRoot(ElementName:="TablixRowHierarchy")>
Public Class TablixRowHierarchy

    <XmlElement(ElementName:="TablixMembers")>
    Public Property TablixMembers As TablixMembers
End Class

<XmlRoot(ElementName:="Tablix")>
Public Class Tablix

    <XmlElement(ElementName:="TablixBody")>
    Public Property TablixBody As TablixBody

    <XmlElement(ElementName:="TablixColumnHierarchy")>
    Public Property TablixColumnHierarchy As TablixColumnHierarchy

    <XmlElement(ElementName:="TablixRowHierarchy")>
    Public Property TablixRowHierarchy As TablixRowHierarchy

    <XmlElement(ElementName:="NoRowsMessage")>
    Public Property NoRowsMessage As String

    <XmlElement(ElementName:="DataSetName")>
    Public Property DataSetName As String

    <XmlElement(ElementName:="Top")>
    Public Property Top As String

    <XmlElement(ElementName:="Left")>
    Public Property Left As String

    <XmlElement(ElementName:="Height")>
    Public Property Height As String

    <XmlElement(ElementName:="Width")>
    Public Property Width As String

    <XmlAttribute(AttributeName:="Name", [Namespace]:="")>
    Public Property Name As String

    <XmlText>
    Public Property Text As String
End Class

<XmlRoot(ElementName:="Body")>
Public Class Body

    <XmlElement(ElementName:="ReportItems")>
    Public Property ReportItems As ReportItems

    <XmlElement(ElementName:="Height")>
    Public Property Height As String
End Class

<XmlRoot(ElementName:="Report")>
Public Class Report

    <XmlElement(ElementName:="Description")>
    Public Property Description As String

    <XmlElement(ElementName:="DataSources")>
    Public Property DataSources As DataSources

    <XmlElement(ElementName:="DataSets")>
    Public Property DataSets As DataSets

    <XmlElement(ElementName:="ReportParameters")>
    Public Property ReportParameters As ReportParameters

    <XmlElement(ElementName:="Page")>
    Public Property Page As Page

    <XmlElement(ElementName:="Body")>
    Public Property Body As Body

    <XmlElement(ElementName:="Width")>
    Public Property Width As String

    <XmlElement(ElementName:="Code")>
    Public Property Code As String

        <XmlElement(ElementName:="ReportUnitType", [Namespace]:="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")>
        Public Property ReportUnitType As String

        <XmlAttribute(AttributeName:="rd", [Namespace]:="http://www.w3.org/2000/xmlns/")>
        Public Property Rd As String

        <XmlAttribute(AttributeName:="xmlns", [Namespace]:="")>
        Public Property Xmlns As String

        <XmlText>
        Public Property Text As String
    End Class
