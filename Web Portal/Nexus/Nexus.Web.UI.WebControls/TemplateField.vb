Imports System.Web.UI.WebControls
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library

Public Class TemplateField : Inherits System.Web.UI.WebControls.TemplateField

    Private _DataType As String

    Public Overrides Property HeaderText() As String
        Get

            'Replace merge field with Transaction Currency Symbol
            MyBase.HeaderText = Replace(MyBase.HeaderText, "[!Currency!]", TransactionCurrency.Symbol)
            Return (MyBase.HeaderText)
        End Get
        Set(ByVal value As String)
            MyBase.HeaderText = value
        End Set
    End Property

    Public Property DataType() As String
        Get
            Return _DataType
        End Get
        Set(ByVal value As String)
            _DataType = value

            Dim oFormatString As Config.FormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString(_DataType.ToString)

            If oFormatString IsNot Nothing Then
                If MyBase.ItemStyle.CssClass = String.Empty Then
                    MyBase.ItemStyle.CssClass = oFormatString.ItemStyleCssClass
                End If

                If MyBase.HeaderStyle.CssClass = String.Empty Then
                    MyBase.HeaderStyle.CssClass = oFormatString.HeaderStyleCssClass
                End If

            End If

        End Set
    End Property
End Class
