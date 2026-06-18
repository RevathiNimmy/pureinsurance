Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmProductLimit
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmProductLimit
	'
	' Date: 08 March 1999
	'
	' Description: Enter Product Licence Limit Details
	'
	' Edit History:
	'
	' DK050100 - Mainly copied from frmLicenceLimit
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "frmProductLimit"
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_iNewLicenceLimit As Integer
	'DAK050100
	Private m_iIsBlockAboveLicenceLimit As gPMConstants.PMEReturnCode
	Private m_iIsWarnAboveLicenceLimit As gPMConstants.PMEReturnCode
	Private m_oBusiness As bPMLicenceAdmin.LicenceAdmin
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property NewLicenceLimit() As Integer
		Get
			Return m_iNewLicenceLimit
		End Get
	End Property
	
	Public ReadOnly Property IsBlockAboveLicenceLimit() As Integer
		Get
			Return m_iIsBlockAboveLicenceLimit
		End Get
	End Property
	
	Public ReadOnly Property IsWarnAboveLicenceLimit() As Integer
		Get
			Return m_iIsWarnAboveLicenceLimit
		End Get
	End Property
	
	Public WriteOnly Property Business() As bPMLicenceAdmin.LicenceAdmin
		Set(ByVal Value As bPMLicenceAdmin.LicenceAdmin)
			m_oBusiness = Value
		End Set
	End Property

	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
		Me.Hide()
	End Sub

	Private Sub frmProductLimit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		txtLicenceFileXml.Text = System.Xml.Linq.XDocument.Parse(XmlFiledata).ToString()
		'txtLicenceFileXml_Click(sender, e)
	End Sub

	Private Sub txtLicenceFileXml_Enter(sender As Object, e As EventArgs) Handles txtLicenceFileXml.Enter
		Dim position As Integer = txtLicenceFileXml.Text.Length
		txtLicenceFileXml.Select(position, position)
	End Sub
End Class
