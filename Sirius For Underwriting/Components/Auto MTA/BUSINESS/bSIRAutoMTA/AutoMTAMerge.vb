Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class AutoMTAMerge 
	
	Private Const ACClass As String = "AutoMTAMerge"
	
	'Array for storing list of risks for merging MTA changes
	Private Const ACRPreChangeRiskCnt As Integer = 0
	Private Const ACRPostChangeRiskCnt As Integer = 1
	Private Const ACRCurrentRiskCnt As Integer = 2
	Private Const ACRArrayIndex As Integer = 4
    Private Const ACRStatus As Integer = 5
    'WPR 33-75 added
    Private Const ACRNewRiskCnt As Short = 6
    Private Const ACRSize As Integer = 6
	
	Private Const ACRiskDataRiskCnt As Integer = 0
	Private Const ACRiskDataRiskFolderCnt As Integer = 2
	Private Const ACRiskDataRiskStatusFlag As Integer = 30
	
	Private m_oFindRisk As Object
	Private m_oRiskData As Object
	Private m_oPreChangeRiskData As ColWrapper
	Private m_oPostChangeRiskData As ColWrapper
	Private m_oGIS As Object
	Private m_oDataset As Object
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bUseFindRisk As Boolean
	Private m_vPostChangeRiskArray As Object
	Private m_vPreChangeRiskArray As Object
	Private m_vCurrentRiskArray As Object
    Private m_vListRiskArray(,) As Object
    Private m_oBaseRiskArray As Object
	Private m_iColRiskCnt As Integer
	Private m_iColRiskFolderCnt As Integer
	Private m_iColRiskStatusFlag As Integer
    Private m_dtColRiskInceptionDate As Object
    'Private m_dtColRiskInceptionDate As Date
	Private m_dtMTAStartDate As Date
	
	' InsuranceFolderCnt
	Private m_lInsuranceFolderCnt As Integer
	' PreChangeInsFileCnt
	Private m_lPreChangeInsFileCnt As Integer
	' PostChangeInsFileCnt
	Private m_lPostChangeInsFileCnt As Integer
	' CurrentInsFileCnt
    Private m_lCurrentInsFileCnt As Integer
    ' Base version
    Private m_nBaseInsFileCnt As Integer
	' RiskCount
	Private m_lRiskCount As Integer
	' CurrentRiskIndex
	Private m_lCurrentRiskIndex As Integer
	' FindRiskArray
	Private m_vFindRiskArray As Object
	' DataModelCode
	Private m_sDataModelCode As Object = ""
	' NewRiskCnt
	Private m_lNewRiskCnt As Integer
	' XMLDataSetDef
	Private m_sXMLDataSetDef As Object = ""
	' XMLDataSet
	Private m_sXMLDataSet As Object = ""

	' Added to replace global variables 26/11/2003
	Private m_sUsername As String = ""
	Private m_sPassword As String = ""
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	
	Public ReadOnly Property XMLDataSet() As String
		Get
			Return m_sXMLDataSet
		End Get
	End Property
	Public ReadOnly Property XMLDataSetDef() As String
		Get
			Return m_sXMLDataSetDef
		End Get
	End Property
    'WPR 33-75 added
    Public Property NewRiskCnt() As Integer
        Get
			If Informations.IsArray(m_vListRiskArray) = True Then
				NewRiskCnt = m_vListRiskArray(ACRNewRiskCnt, m_lCurrentRiskIndex)
			Else
				NewRiskCnt = -1
			End If
		End Get
		Set(ByVal Value As Integer)
			m_lNewRiskCnt = Value

			If Informations.IsArray(m_vListRiskArray) = True Then
				m_vListRiskArray(ACRNewRiskCnt, m_lCurrentRiskIndex) = Value
			End If
		End Set
	End Property
	Public WriteOnly Property DataModelCode() As String
		Set(ByVal Value As String)
			m_sDataModelCode = Value
		End Set
	End Property
	Public WriteOnly Property InsuranceFolderCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
		End Set
	End Property
	Public WriteOnly Property Gis() As Object
		Set(ByVal Value As Object)
			m_oGIS = Value
		End Set
	End Property
	Public WriteOnly Property Dataset() As Object
		Set(ByVal Value As Object)
			m_oDataset = Value
		End Set
	End Property


	Public ReadOnly Property CurrentRiskCnt() As Integer
		Get
			If Informations.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRCurrentRiskCnt, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
		End Get
	End Property
	Public ReadOnly Property PreChangeRiskCnt() As Integer
		Get
			If Informations.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRPreChangeRiskCnt, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
		End Get
	End Property

	Public ReadOnly Property PostChangeRiskCnt() As Integer
		Get
			If Informations.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRPostChangeRiskCnt, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
		End Get
	End Property

	Public ReadOnly Property MergeStatus() As String
		Get
			If Informations.IsArray(m_vListRiskArray) Then
				Return CStr(m_vListRiskArray(ACRStatus, m_lCurrentRiskIndex))
			Else
				Return ""
			End If
		End Get
	End Property

	Public ReadOnly Property FindRiskArrayIndex() As Integer
		Get

			If Informations.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRArrayIndex, m_lCurrentRiskIndex))
			Else
				Return -1
			End If

		End Get
	End Property
	Public ReadOnly Property FindRiskArray() As Object
		Get
			If Informations.IsArray(m_vListRiskArray) Then
				If CStr(m_vListRiskArray(ACRStatus, m_lCurrentRiskIndex)) = gSIRLibrary.ACRStatusAddPostChange Then
					Return m_vPostChangeRiskArray
				Else
					Return m_vCurrentRiskArray
				End If
			Else
				Return ""
			End If
		End Get
	End Property
	Public WriteOnly Property CurrentInsFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lCurrentInsFileCnt = Value
		End Set
	End Property
	Public WriteOnly Property PostChangeInsFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPostChangeInsFileCnt = Value
		End Set
	End Property
	Public WriteOnly Property PreChangeInsFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPreChangeInsFileCnt = Value
		End Set
	End Property

	Public WriteOnly Property FindRisk() As Object
		Set(ByVal Value As Object)
			m_oFindRisk = Value
		End Set
	End Property
	Public WriteOnly Property RiskData() As Object
		Set(ByVal Value As Object)
			m_oRiskData = Value
		End Set
	End Property
	Public ReadOnly Property RiskCount() As Integer
		Get

			If Informations.IsArray(m_vListRiskArray) Then
				m_lRiskCount = m_vListRiskArray.GetUpperBound(1)
			Else
				m_lRiskCount = -1
			End If

			Return m_lRiskCount
		End Get
	End Property
	Public WriteOnly Property CurrentRiskIndex() As Integer
		Set(ByVal Value As Integer)
			m_lCurrentRiskIndex = Value
		End Set
	End Property

	Public WriteOnly Property MTAStartDate() As Date
		Set(ByVal Value As Date)
			m_dtMTAStartDate = Value
		End Set
	End Property

	Public WriteOnly Property BaseInsFileCnt() As Integer
		Set(ByVal Value As Integer)
			m_nBaseInsFileCnt = Value
		End Set
	End Property

	' ***************************************************************** '
	'
	' Name: GetListOfRisks
	'
	' Description:
	'
	' History: 10/02/2003 sj - Created.
	'
	' ***************************************************************** '
	Public Function GetListOfRisks() As Integer

		Dim result As Integer = 0
		Try

			result = gPMConstants.PMEReturnCode.PMTrue

			If Not (m_oFindRisk Is Nothing) Then
				m_bUseFindRisk = True
				m_iColRiskFolderCnt = ACIRiskFolderCnt
				m_iColRiskCnt = ACIRiskId
				m_iColRiskStatusFlag = ACIRiskStatusFlag
				m_dtColRiskInceptionDate = ACIRiskInceptionDate
				'Use find risk to get the risk data
				m_lReturn = GetListOfRisksUsingFindRisk()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingFindRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Else
				m_bUseFindRisk = False
				m_iColRiskFolderCnt = ACRiskDataRiskFolderCnt
				m_iColRiskCnt = ACRiskDataRiskCnt
				m_iColRiskStatusFlag = ACRiskDataRiskStatusFlag
				'Use risk data to get the list of risks
				m_lReturn = GetListOfRisksUsingRiskData()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			'Index the post/pre data arrays using collections
			m_lReturn = BuildCollections()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildCollections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'Compare the Post change risks to the Pre change risks to identify any that have been added
			m_lReturn = ComparePostPreData()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ComparePostPreData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'Get all the current risks, flag which ones need to be merged or deleted
			m_lReturn = ProcessCurrentRisks()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCurrentRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	Public Function GetListOfRisksUsingFindRisk() As Integer

		Dim result As Integer = 0
		Try

			result = gPMConstants.PMEReturnCode.PMTrue

			'Get current risks
			'UPGRADE_TODO: (1067) Member SearchAll is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_vCurrentRiskArray, v_vInsuranceFileCnt:=ToSafeInteger(m_lCurrentInsFileCnt))

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			'Get pre change risks
			'UPGRADE_TODO: (1067) Member SearchAll is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_vPreChangeRiskArray, v_vInsuranceFileCnt:=ToSafeInteger(m_lPreChangeInsFileCnt))

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			'Get post change risks
			'UPGRADE_TODO: (1067) Member SearchAll is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_vPostChangeRiskArray, v_vInsuranceFileCnt:=ToSafeInteger(m_lPostChangeInsFileCnt))

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			'Get base change risks
			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_oBaseRiskArray, v_vInsuranceFileCnt:=ToSafeInteger(m_nBaseInsFileCnt))

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingFindRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	Public Function GetListOfRisksUsingRiskData() As Integer

		Dim result As Integer = 0
		Try

			result = gPMConstants.PMEReturnCode.PMTrue

			'Get current risks
			'UPGRADE_TODO: (1067) Member GetRisk is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=ToSafeInteger(m_lCurrentInsFileCnt), r_vResultArray:=m_vCurrentRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'Get pre change risks
			'UPGRADE_TODO: (1067) Member GetRisk is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=ToSafeInteger(m_lPreChangeInsFileCnt), r_vResultArray:=m_vPreChangeRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'Get post change risks
			'UPGRADE_TODO: (1067) Member GetRiskAllStatuses is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=ToSafeInteger(m_lPostChangeInsFileCnt), r_vResultArray:=m_vPostChangeRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			'Get base risks
			m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=ToSafeInteger(m_nBaseInsFileCnt), r_vResultArray:=m_oBaseRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	' ***************************************************************** '
	'
	' Name: BuildCollections
	'
	' Description:
	'
	' History: 10/02/2003 sj - Created.
	'
	' ***************************************************************** '
	Private Function BuildCollections() As Integer

		Dim result As Integer = 0


		result = gPMConstants.PMEReturnCode.PMTrue

		Dim lRiskFolderCnt As Integer

		m_oPreChangeRiskData = New ColWrapper()
		m_oPostChangeRiskData = New ColWrapper()

		'developer guide no 97. 
		m_lReturn = CType(m_oPreChangeRiskData.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName), gPMConstants.PMEReturnCode)

		m_lReturn = CType(m_oPostChangeRiskData.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName), gPMConstants.PMEReturnCode)



		For iCnt As Integer = 0 To m_vPreChangeRiskArray.GetUpperBound(1)
			lRiskFolderCnt = CInt(m_vPreChangeRiskArray(m_iColRiskFolderCnt, iCnt))
			m_lReturn = CType(m_oPreChangeRiskData.Add(v_vItem:=iCnt, v_vKey:=CStr(lRiskFolderCnt)), gPMConstants.PMEReturnCode)
		Next iCnt


		For iCnt As Integer = 0 To m_vPostChangeRiskArray.GetUpperBound(1)

			lRiskFolderCnt = CInt(m_vPostChangeRiskArray(m_iColRiskFolderCnt, iCnt))
			m_lReturn = CType(m_oPostChangeRiskData.Add(v_vItem:=iCnt, v_vKey:=CStr(lRiskFolderCnt)), gPMConstants.PMEReturnCode)
		Next iCnt

		Return result

	End Function

	' ***************************************************************** '
	'
	' Name: ComparePostPreData
	'
	' Description:
	'
	' History: 07/02/2003 sj - Created.
	'
	' ***************************************************************** '
	Private Function ComparePostPreData() As Integer

		Dim result As Integer = 0


		result = gPMConstants.PMEReturnCode.PMTrue

		Dim bExists As Boolean
		Dim lRiskFolderCnt As Integer
		Dim iIndex As Integer
		Dim lUbound As Integer
		Dim vRisks As Object
		Dim oBusiness As New Business
		Dim i As Integer

		m_lReturn = oBusiness.Initialise(sUserName:=m_sUsername,
										sPassword:=m_sPassword,
										iUserID:=m_iUserID,
										iSourceID:=m_iSourceID,
										iLanguageID:=m_iLanguageID,
										iCurrencyID:=m_iCurrencyID,
										iLogLevel:=m_iLogLevel,
										sCallingAppName:=m_sCallingAppName)
		For iCnt As Integer = 0 To m_vPostChangeRiskArray.GetUpperBound(1)


			lRiskFolderCnt = CInt(m_vPostChangeRiskArray(m_iColRiskFolderCnt, iCnt))

			'Does the risk exist prior to the change
			m_lReturn = CType(m_oPreChangeRiskData.Item(v_vKey:=CStr(lRiskFolderCnt), r_vItem:=iIndex, r_vExists:=bExists), gPMConstants.PMEReturnCode)

			If Not bExists Then

				'Get the deleted risk Cnt for current OOS version; need to pass older one as MTA link is not yet prepared
				m_lReturn = oBusiness.GetPolicyRisks(m_lIFileCnt:=m_lPreChangeInsFileCnt, vRisks:=vRisks)

				If ((If(m_vPostChangeRiskArray(m_dtColRiskInceptionDate, iCnt).Trim() = "", Date.MinValue, m_vPostChangeRiskArray(m_dtColRiskInceptionDate, iCnt))) >= m_dtMTAStartDate) Then
					'Risk has been added post change
					If Informations.IsArray(m_vListRiskArray) Then
						lUbound = m_vListRiskArray.GetUpperBound(1) + 1
						ReDim Preserve m_vListRiskArray(ACRSize, lUbound)
					Else
						lUbound = 0
						ReDim m_vListRiskArray(ACRSize, lUbound)
					End If
					m_vListRiskArray(ACRPreChangeRiskCnt, lUbound) = 0

					m_vListRiskArray(ACRPostChangeRiskCnt, lUbound) = m_vPostChangeRiskArray(m_iColRiskCnt, iCnt)
					m_vListRiskArray(ACRCurrentRiskCnt, lUbound) = 0
					m_vListRiskArray(ACRArrayIndex, lUbound) = iCnt
					m_vListRiskArray(ACRStatus, lUbound) = gSIRLibrary.ACRStatusAddPostChange
					If Informations.IsArray(vRisks) Then
						For i = 0 To UBound(vRisks, 2)
							If ToSafeLong(vRisks(3, i)) = ToSafeLong(m_vPostChangeRiskArray(24, iCnt), 0) And ToSafeString(vRisks(2, i)) = "D" Then
								' don't process
								m_vListRiskArray(ACRStatus, lUbound) = ACRStatusNoProcess
							End If
						Next i
					End If

				End If
			End If
		Next iCnt

		Return result

	End Function

	' ***************************************************************** '
	'
	' Name: ProcessCurrentRisks
	'
	' Description:
	'
	' History: 10/02/2003 sj - Created.
	'
	' ***************************************************************** '
	Private Function ProcessCurrentRisks() As Integer

		Dim nResult As Integer = 0


		nResult = gPMConstants.PMEReturnCode.PMTrue

		Dim bExists As Boolean
		Dim nRiskFolderCnt As Integer
		Dim iPreChangeIndex, iPostChangeIndex As Integer
		Dim iUbound As Integer
		Dim bPreChangeDeleted As Boolean
		Dim icount As Integer

		If Informations.IsArray(m_vCurrentRiskArray) Then
			For iCnt As Integer = 0 To m_vCurrentRiskArray.GetUpperBound(1)
				nRiskFolderCnt = m_vCurrentRiskArray(m_iColRiskFolderCnt, iCnt)
				'Does the risk exist prechange
				m_lReturn = m_oPreChangeRiskData.Item(
					v_vKey:=CStr(nRiskFolderCnt),
					r_vItem:=iPreChangeIndex,
					r_vExists:=bExists)


				bPreChangeDeleted = False

				If bExists = True Then
					For icount = 0 To m_vPreChangeRiskArray.GetUpperBound(1)
						If m_vPreChangeRiskArray(m_iColRiskFolderCnt, icount) = nRiskFolderCnt Then
							' check if deleted in previous version; ignore processing further
							If m_vPreChangeRiskArray(m_iColRiskStatusFlag, icount) = "D" Then
								bPreChangeDeleted = True
							End If
							Exit For
						End If
					Next icount
				End If

				If bPreChangeDeleted = False Then
					If Informations.IsArray(m_vListRiskArray) Then
						iUbound = m_vListRiskArray.GetUpperBound(1) + 1
						ReDim Preserve m_vListRiskArray(ACRSize, iUbound)
					Else
						iUbound = 0
						ReDim m_vListRiskArray(ACRSize, iUbound)
						ReDim Preserve m_vListRiskArray(ACRSize, iUbound)
					End If

					m_vListRiskArray(ACRArrayIndex, iUbound) = iCnt

					If Not bExists Then
						'Risk has been added as a result of the current change so do not need
						'to merge
						m_vListRiskArray(ACRPreChangeRiskCnt, iUbound) = 0
						m_vListRiskArray(ACRPostChangeRiskCnt, iUbound) = 0

						m_vListRiskArray(ACRCurrentRiskCnt, iUbound) = m_vCurrentRiskArray(m_iColRiskCnt, iCnt)
						m_vListRiskArray(ACRStatus, iUbound) = gSIRLibrary.ACRStatusNoMerge
					Else
						'The risk exists pre change so should exist post change
						'but for a renewal the risk will be deleted completely
						m_lReturn = CType(m_oPostChangeRiskData.Item(v_vKey:=CStr(nRiskFolderCnt), r_vItem:=iPostChangeIndex, r_vExists:=bExists), gPMConstants.PMEReturnCode)

						m_vListRiskArray(ACRPreChangeRiskCnt, iUbound) = m_vPreChangeRiskArray(m_iColRiskCnt, iPreChangeIndex)

						m_vListRiskArray(ACRCurrentRiskCnt, iUbound) = m_vCurrentRiskArray(m_iColRiskCnt, iCnt)

						If bExists Then

							m_vListRiskArray(ACRPostChangeRiskCnt, iUbound) = m_vPostChangeRiskArray(m_iColRiskCnt, iPostChangeIndex)

							If CStr(m_vPostChangeRiskArray(m_iColRiskStatusFlag, iPostChangeIndex)) = "D" Then
								'Risk was deleted post change
								m_vListRiskArray(ACRStatus, iUbound) = gSIRLibrary.ACRStatusDeletedPostChange
							Else
								'Mark for No Process
								m_vListRiskArray(ACRStatus, iUbound) = gSIRLibrary.ACRStatusNoProcess
								' override to no merge if was edited in base version
								For iCnt1 As Integer = 0 To UBound(m_oBaseRiskArray, 2)
									If m_oBaseRiskArray(m_iColRiskFolderCnt, iCnt1) = nRiskFolderCnt Then
										' check if was edited
										If ToSafeInteger(m_oBaseRiskArray(41, iCnt1)) = 1 Then
											m_vListRiskArray(ACRStatus, iUbound) = ACRStatusMerge
										End If
										Exit For
									End If
								Next iCnt1
							End If

						End If
					End If
				End If
			Next iCnt
		End If

		Return nResult

	End Function

	' ***************************************************************** '
	'
	' Name: MergeExistingMTAChanges
	'
	' Description:
	'
	' History: 10/02/2003 sj - Created.
	'
	' ***************************************************************** '
	Public Function MergeExistingMTAChanges() As Integer

		Dim result As Integer = 0
		Try

			result = gPMConstants.PMEReturnCode.PMTrue

			Dim lPreChangeRiskCnt, lPostChangeRiskCnt, lCurrentRiskCnt As Integer

			If CStr(m_vListRiskArray(ACRStatus, m_lCurrentRiskIndex)) <> gSIRLibrary.ACRStatusMerge Then
				'We don't need to merge
				'Load the dataset
				'UPGRADE_TODO: (1067) Member LoadRiskFromDB is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
				m_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=m_sXMLDataSetDef, r_sXMLDataset:=m_sXMLDataSet, r_sGisDataModelCode:=m_sDataModelCode, v_lInsuranceFileCnt:=ToSafeInteger(m_lInsuranceFolderCnt), v_lRiskId:=ToSafeInteger(m_lNewRiskCnt))
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oGis.LoadRiskFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
					Return result
				End If
				Return result
			End If

			lPreChangeRiskCnt = CInt(m_vListRiskArray(ACRPreChangeRiskCnt, m_lCurrentRiskIndex))
			lPostChangeRiskCnt = CInt(m_vListRiskArray(ACRPostChangeRiskCnt, m_lCurrentRiskIndex))
			lCurrentRiskCnt = CInt(m_vListRiskArray(ACRCurrentRiskCnt, m_lCurrentRiskIndex))

			'Call method on Gis to do the merge
			'UPGRADE_TODO: (1067) Member MTADiffAndMerge is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oGIS.MTADiffAndMerge(v_lInsuranceFolderCnt:=ToSafeInteger(m_lInsuranceFolderCnt), v_lOMTARiskID:=ToSafeInteger(lPreChangeRiskCnt), v_lNMTARiskID:=ToSafeInteger(m_lNewRiskCnt), v_lIMTARiskID:=ToSafeInteger(lPostChangeRiskCnt))
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="MTADiffAndMerge Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
				Return result
			End If

			'Load the dataset
			'UPGRADE_TODO: (1067) Member LoadRiskFromDB is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=m_sXMLDataSetDef, r_sXMLDataset:=m_sXMLDataSet, r_sGisDataModelCode:=m_sDataModelCode, v_lInsuranceFileCnt:=ToSafeInteger(m_lInsuranceFolderCnt), v_lRiskId:=ToSafeInteger(m_lNewRiskCnt))
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oGis.LoadRiskFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
				Return result
			End If

			'We need to clear the output objects as they will have been remerged
			m_lReturn = CType(ClearOutputObjects(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ClearOutputObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
				Return result
			End If

			'TestStub lPostChangeRiskCnt

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result
			'UPGRADE_TODO: (1065) Error handling statement (Resume) could not be converted. More Information: http://www.vbtonet.com/ewis/ewi1065.aspx
			'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			Return result
		End Try
	End Function
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

		Dim result As Integer = 0
		Try

			result = gPMConstants.PMEReturnCode.PMTrue
			'
			' *******************************************************************
			' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
			' Set Username and Password
			m_sUsername = sUsername
			m_sPassword = sPassword
			m_iUserID = iUserID
			m_sCallingAppName = sCallingAppName
			m_iLanguageID = iLanguageID
			m_iSourceID = iSourceID
			m_iCurrencyID = iCurrencyID
			m_iLogLevel = iLogLevel

			Return result

		Catch excep As System.Exception



			result = gPMConstants.PMEReturnCode.PMError

			' Log Error Message
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

			Return result

		End Try
	End Function

	' ***************************************************************** '
	' Name: ClearOutputObjects
	'
	' Description:
	'
	' History: 12/03/2003 sj - Created.
	'
	' ***************************************************************** '
	Private Function ClearOutputObjects() As Integer

		Dim result As Integer = 0


		result = gPMConstants.PMEReturnCode.PMTrue

		Dim sObjectName As Object = ""
		Dim vOIKeyArray As Object = Nothing

		sObjectName = m_sDataModelCode.Trim() & "_" & "Output"

		'UPGRADE_TODO: (1067) Member LoadFromXML is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
		m_lReturn = m_oDataset.LoadFromXML(v_sXMLDataSetDef:=m_sXMLDataSetDef, v_sXMLDataSet:=m_sXMLDataSet)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
			Return gPMConstants.PMEReturnCode.PMFalse
		End If

		'UPGRADE_TODO: (1067) Member GetAllOIKey is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
		m_lReturn = m_oDataset.GetAllOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray)

		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataset.GetAllOIKey Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
			Return gPMConstants.PMEReturnCode.PMFalse
		End If

		If Not Informations.IsArray(vOIKeyArray) Then
			Return result
		End If


		For i As Integer = 0 To vOIKeyArray.GetUpperBound(0)


			'UPGRADE_TODO: (1067) Member DelObjectInstance is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
			m_lReturn = m_oDataset.DelObjectInstance(v_sObjectName:=sObjectName, v_sOiKey:=CStr(vOIKeyArray(i)))
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataset.DelObjectInstance Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

		'Return as XmL String
		m_lReturn = m_oDataset.ReturnAsXML(r_sXMLDataSetDef:=m_sXMLDataSetDef, r_sXMLDataset:=m_sXMLDataSet)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function
	
	Protected Overrides Sub Finalize()
		
		m_oFindRisk = Nothing
		m_oRiskData = Nothing
		m_oPreChangeRiskData = Nothing
		m_oPostChangeRiskData = Nothing
		m_oGIS = Nothing
		m_oDataset = Nothing
		
	End Sub
End Class