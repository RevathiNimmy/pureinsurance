Module UpgradeSupport
	'UPGRADE_NOTE: (7014) The property 'DAO_DBEngine_definst' could be being created from different 'UpgradeSupport' in a multiproject solution More Information: http://www.vbtonet.com/ewis/ewi7014.aspx
	Private _DAO_DBEngine_definst As DAO.DBEngine = Nothing
	Public ReadOnly Property DAO_DBEngine_definst() As DAO.DBEngine
		Get
			If _DAO_DBEngine_definst Is Nothing Then
				_DAO_DBEngine_definst = New DAO.DBEngine
			End If
			Return _DAO_DBEngine_definst
		End Get
	End Property
End Module