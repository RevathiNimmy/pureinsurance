Option Strict Off
Option Explicit On
Imports System
Module Module1
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	Private m_oDocManagerWrapper As bSIRDocManagerWrapper.Interface
	Private m_lReturn As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_lPartyCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_sPartyName As String = ""
	Private m_sInsuranceFileRef As String = ""
	
	Public Sub Main()
		
		Dim lDocTemplateID, lDocTypeID As Integer
		
		m_oDocManagerWrapper = New bSIRDocManagerWrapper.Interface()
		
		
		'    m_lReturn = m_oDocManagerWrapper.InitialiseBusiness( _
		''        "admin", "admin", 5, 1, 1, 26, 6, "TestStub")
		
		m_lReturn = m_oDocManagerWrapper.InitialiseBusiness("sirius", "XctqMUbg", 5, 1, 1, 26, 6, "TestStub")
		
		m_lPartyCnt = 13
		m_sPartyName = "DOOZER"
		m_lInsuranceFolderCnt = 22
		m_lInsuranceFileCnt = 22
		m_sInsuranceFileRef = "HEADOMOTPOL00018"
		
		
		m_lReturn = ProcessDocTemplate()
		
		m_oDocManagerWrapper.Dispose()
		m_oDocManagerWrapper = Nothing
	End Sub
	
	Private Function ProcessDocTemplate() As Byte
		
		m_oDocManagerWrapper.PartyCnt = m_lPartyCnt
		m_oDocManagerWrapper.PartyName = m_sPartyName
		m_oDocManagerWrapper.DocName = "Test Archive Doc"
		m_oDocManagerWrapper.InsuranceFolderCnt = m_lInsuranceFolderCnt
		m_oDocManagerWrapper.InsuranceFileCnt = m_lInsuranceFileCnt
		m_oDocManagerWrapper.InsuranceFileRef = m_sInsuranceFileRef
		
		
		m_oDocManagerWrapper.ProcessTypesDocsId = 6 'Renewal
		m_oDocManagerWrapper.DocumentTypeId = 8
		m_oDocManagerWrapper.DocumentTemplateId = 18
		
		m_oDocManagerWrapper.Mode = 3 'print doc in silent mode
		'm_oDocManagerWrapper.Mode = 4    'spool doc
		
		m_lReturn = m_oDocManagerWrapper.Start()
		
		If m_lReturn <> 1 Then
			Return 0
		End If
		
	End Function
End Module