Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Modified by Sumeet Singh on 5/11/2010 6:47:51 PM refer developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class AutoMTAMerge 
	
	Private Const ACClass As String = "AutoMTAMerge"
	
	'Array for storing list of risks for merging MTA changes
	Private Const ACRPreChangeRiskCnt As Integer = 0
	Private Const ACRPostChangeRiskCnt As Integer = 1
	Private Const ACRCurrentRiskCnt As Integer = 2
	Private Const ACRRiskStatusFlag As Integer = 3
	Private Const ACRArrayIndex As Integer = 4
	Private Const ACRStatus As Integer = 5
	Private Const ACRSize As Integer = 5
	
	Private Const ACRiskDataRiskCnt As Integer = 0
	Private Const ACRiskDataRiskFolderCnt As Integer = 2
	Private Const ACRiskDataRiskStatusFlag As Integer = 30
	
	' FindRisk
	Private m_oFindRisk As Object
	' RiskData
	Private m_oRiskData As Object
	Private m_oPreChangeRiskData As ColWrapper
	Private m_oPostChangeRiskData As ColWrapper
	Private m_oGIS As Object
	Private m_oDataset As Object
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bUseFindRisk As Boolean
	Private m_vPostChangeRiskArray As Object
	Private m_vPreChangeRiskArray( ,  ) As Object
	Private m_vCurrentRiskArray As Object
	Private m_vListRiskArray( ,  ) As Object
	Private m_iColRiskCnt As Integer
	Private m_iColRiskFolderCnt As Integer
	Private m_iColRiskStatusFlag As Integer
	
	' InsuranceFolderCnt
	Private m_lInsuranceFolderCnt As Integer
	' PreChangeInsFileCnt
	Private m_lPreChangeInsFileCnt As Integer
	' PostChangeInsFileCnt
	Private m_lPostChangeInsFileCnt As Integer
	' CurrentInsFileCnt
	Private m_lCurrentInsFileCnt As Integer
	' RiskCount
	Private m_lRiskCount As Integer
	' CurrentRiskIndex
	Private m_lCurrentRiskIndex As Integer
	' FindRiskArray
	Private m_vFindRiskArray As Object
	' DataModelCode
	Private m_sDataModelCode As String = ""
	' NewRiskCnt
	Private m_lNewRiskCnt As Integer
	' XMLDataSetDef
	Private m_sXMLDataSetDef As String = ""
	' XMLDataSet
	Private m_sXMLDataSet As String = ""
	
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
	Public WriteOnly Property NewRiskCnt() As Integer
		Set(ByVal Value As Integer)
			m_lNewRiskCnt = Value
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
			If Information.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRCurrentRiskCnt, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
		End Get
	End Property
	Public ReadOnly Property PreChangeRiskCnt() As Integer
		Get
			If Information.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRPreChangeRiskCnt, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
		End Get
	End Property
	
	Public ReadOnly Property PostChangeRiskCnt() As Integer
		Get
			If Information.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRPostChangeRiskCnt, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
		End Get
	End Property
	
	Public ReadOnly Property MergeStatus() As String
		Get
			If Information.IsArray(m_vListRiskArray) Then
				Return CStr(m_vListRiskArray(ACRStatus, m_lCurrentRiskIndex))
			Else
				Return ""
			End If
		End Get
	End Property
	
	Public ReadOnly Property FindRiskArrayIndex() As Integer
		Get
			
			If Information.IsArray(m_vListRiskArray) Then
				Return CInt(m_vListRiskArray(ACRArrayIndex, m_lCurrentRiskIndex))
			Else
				Return -1
			End If
			
		End Get
	End Property
	Public ReadOnly Property FindRiskArray() As Object
		Get
			If Information.IsArray(m_vListRiskArray) Then
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
			
			If Information.IsArray(m_vListRiskArray) Then
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
				
				'Use find risk to get the risk data
				m_lReturn = GetListOfRisksUsingFindRisk()
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingFindRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
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
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			'Index the post/pre data arrays using collections
			m_lReturn = BuildCollections()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildCollections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Compare the Post change risks to the Pre change risks to identify any that have been added
			m_lReturn = ComparePostPreData()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ComparePostPreData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Get all the current risks, flag which ones need to be merged or deleted
			m_lReturn = ProcessCurrentRisks()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCurrentRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function GetListOfRisksUsingFindRisk() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get current risks

			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_vCurrentRiskArray, v_vInsuranceFileCnt:=m_lCurrentInsFileCnt)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			'Get pre change risks

			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_vPreChangeRiskArray, v_vInsuranceFileCnt:=m_lPreChangeInsFileCnt)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			'Get post change risks

			m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=m_vPostChangeRiskArray, v_vInsuranceFileCnt:=m_lPostChangeInsFileCnt)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk")
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingFindRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingFindRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function GetListOfRisksUsingRiskData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get current risks

			m_lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=m_lCurrentInsFileCnt, r_vResultArray:=m_vCurrentRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Get pre change risks

			m_lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=m_lPreChangeInsFileCnt, r_vResultArray:=m_vPreChangeRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Get post change risks

			m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=m_lPostChangeInsFileCnt, r_vResultArray:=m_vPostChangeRiskArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisksUsingRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisksUsingRiskData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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


            For iCnt As Integer = 0 To m_vPostChangeRiskArray.GetUpperBound(1)


                lRiskFolderCnt = CInt(m_vPostChangeRiskArray(m_iColRiskFolderCnt, iCnt))

                'Does the risk exist prior to the change
                m_lReturn = CType(m_oPreChangeRiskData.Item(v_vKey:=CStr(lRiskFolderCnt), r_vItem:=iIndex, r_vExists:=bExists), gPMConstants.PMEReturnCode)

                If Not bExists Then
                    'Risk has been added post change
                    If Information.IsArray(m_vListRiskArray) Then
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

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bExists As Boolean
            Dim lRiskFolderCnt As Integer
            Dim iPreChangeIndex, iPostChangeIndex As Integer
            Dim lUbound As Integer


            For iCnt As Integer = 0 To m_vCurrentRiskArray.GetUpperBound(1)

                If Information.IsArray(m_vListRiskArray) Then
                    lUbound = m_vListRiskArray.GetUpperBound(1) + 1
                    ReDim Preserve m_vListRiskArray(ACRSize, lUbound)
                Else
                    lUbound = 0
                    ReDim m_vListRiskArray(ACRSize, lUbound)
                    ReDim Preserve m_vListRiskArray(ACRSize, lUbound)
                End If

                m_vListRiskArray(ACRArrayIndex, lUbound) = iCnt


                lRiskFolderCnt = CInt(m_vCurrentRiskArray(m_iColRiskFolderCnt, iCnt))

                'Does the risk exist prechange
                m_lReturn = CType(m_oPreChangeRiskData.Item(v_vKey:=CStr(lRiskFolderCnt), r_vItem:=iPreChangeIndex, r_vExists:=bExists), gPMConstants.PMEReturnCode)

                If Not bExists Then
                    'Risk has been added as a result of the current change so do not need
                    'to merge
                    m_vListRiskArray(ACRPreChangeRiskCnt, lUbound) = 0
                    m_vListRiskArray(ACRPostChangeRiskCnt, lUbound) = 0

                    m_vListRiskArray(ACRCurrentRiskCnt, lUbound) = m_vCurrentRiskArray(m_iColRiskCnt, iCnt)
                    m_vListRiskArray(ACRStatus, lUbound) = gSIRLibrary.ACRStatusNoMerge
                Else
                    'The risk exists pre change so should exist post change
                    'but for a renewal the risk will be deleted completely
                    m_lReturn = CType(m_oPostChangeRiskData.Item(v_vKey:=CStr(lRiskFolderCnt), r_vItem:=iPostChangeIndex, r_vExists:=bExists), gPMConstants.PMEReturnCode)

                    m_vListRiskArray(ACRPreChangeRiskCnt, lUbound) = m_vPreChangeRiskArray(m_iColRiskCnt, iPreChangeIndex)

                    m_vListRiskArray(ACRCurrentRiskCnt, lUbound) = m_vCurrentRiskArray(m_iColRiskCnt, iCnt)

                    If bExists Then

                        m_vListRiskArray(ACRPostChangeRiskCnt, lUbound) = m_vPostChangeRiskArray(m_iColRiskCnt, iPostChangeIndex)

                        If CStr(m_vPostChangeRiskArray(m_iColRiskStatusFlag, iPostChangeIndex)) = "D" Then
                            'Risk was deleted post change
                            m_vListRiskArray(ACRStatus, lUbound) = gSIRLibrary.ACRStatusDeletedPostChange
                        Else
                            'We need to merge in the changes
                            m_vListRiskArray(ACRStatus, lUbound) = gSIRLibrary.ACRStatusMerge
                        End If
                    Else
                        m_vListRiskArray(ACRPostChangeRiskCnt, lUbound) = 0
                        m_vListRiskArray(ACRStatus, lUbound) = gSIRLibrary.ACRStatusDeletedPostChange
                    End If

                End If
            Next iCnt

            Return result

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

                m_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=m_sXMLDataSetDef, r_sXMLDataset:=m_sXMLDataSet, r_sGisDataModelCode:=m_sDataModelCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskId:=m_lNewRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oGis.LoadRiskFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
                    Return result
                End If
                Return result
            End If

            lPreChangeRiskCnt = CInt(m_vListRiskArray(ACRPreChangeRiskCnt, m_lCurrentRiskIndex))
            lPostChangeRiskCnt = CInt(m_vListRiskArray(ACRPostChangeRiskCnt, m_lCurrentRiskIndex))
            lCurrentRiskCnt = CInt(m_vListRiskArray(ACRCurrentRiskCnt, m_lCurrentRiskIndex))

            'Call method on Gis to do the merge

            m_lReturn = m_oGIS.MTADiffAndMerge(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lOMTARiskID:=lPreChangeRiskCnt, v_lNMTARiskID:=m_lNewRiskCnt, v_lIMTARiskID:=lPostChangeRiskCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="MTADiffAndMerge Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
                Return result
            End If

            'Load the dataset

            m_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=m_sXMLDataSetDef, r_sXMLDataset:=m_sXMLDataSet, r_sGisDataModelCode:=m_sDataModelCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskId:=m_lNewRiskCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oGis.LoadRiskFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
                Return result
            End If

            'We need to clear the output objects as they will have been remerged
            m_lReturn = CType(ClearOutputObjects(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ClearOutputObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges")
                Return result
            End If

            'TestStub lPostChangeRiskCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: TestStub
    '
    ' Description:
    '
    ' History: 13/02/2003 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (TestStub) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub TestStub(ByVal v_lPostChangeRiskCnt As Integer)
    '
    'Try 
    '
    'Dim sXMLDataSetDef, sXMLDataSet As String
    'Dim lPolicyLinkId As Integer
    'Dim sObjectName, sPropertyName As String
    'Dim vOIKeyArray As Object
    'Dim bisassumedInfo As Boolean
    'Dim vValue As Object
    '
    'lPolicyLinkId = -1
    'sObjectName = "Trailer"
    'sPropertyName = "SumInsured"
    '
    'Load the original risk to get the property value

    'm_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGisDataModelCode:=m_sDataModelCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskId:=v_lPostChangeRiskCnt)
    '    m_lReturn = m_oGis.LoadFromDB( _
    ''        r_sXMLDataSetDef:=sXMLDataSetDef, _
    ''        r_sXMLDataSet:=sXMLDataSet, _
    ''        v_sGisDataModelCode:=m_sDataModelCode, _
    ''        r_vInsuranceFileCnt:=m_lInsuranceFolderCnt, _
    ''        r_vPolicyLinkID:=lPolicyLinkId, _
    ''        r_vRiskID:=v_lPostChangeRiskCnt)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub")
    'Exit Sub
    'End If
    '

    'm_lReturn = m_oDataset.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub")
    'Exit Sub
    'End If
    '

    'm_lReturn = m_oDataset.GetAllOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.GetAllOIKey Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub")
    'Exit Sub
    'End If
    '
    'If Information.IsArray(vOIKeyArray) Then


    'm_lReturn = m_oDataset.GetPropertyValue(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, v_sOiKey:=CStr(vOIKeyArray(0)), r_vPropertyValue:=vValue, r_bIsAssumedInfo:=bisassumedInfo)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.SetPropertyValue Failed for " & sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyRiskChanges")
    'End If
    'End If
    '
    'lPolicyLinkId = -1
    '
    'Load the new risk

    'm_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGisDataModelCode:=m_sDataModelCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskId:=m_lNewRiskCnt)
    '    m_lReturn = m_oGis.LoadFromDB( _
    ''        r_sXMLDataSetDef:=sXMLDataSetDef, _
    ''        r_sXMLDataSet:=sXMLDataSet, _
    ''        v_sGisDataModelCode:=m_sDataModelCode, _
    ''        r_vInsuranceFileCnt:=m_lInsuranceFolderCnt, _
    ''        r_vPolicyLinkID:=lPolicyLinkId, _
    ''        r_vRiskID:=m_lNewRiskCnt)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub")
    'Exit Sub
    'End If
    '

    'm_lReturn = m_oDataset.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub")
    'Exit Sub
    'End If
    '

    'm_lReturn = m_oDataset.GetAllOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.GetAllOIKey Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub")
    'Exit Sub
    'End If
    '
    'If Information.IsArray(vOIKeyArray) Then
    '


    'm_lReturn = m_oDataset.SetPropertyValue(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, v_sOiKey:=CStr(vOIKeyArray(0)), v_vPropertyValue:=vValue, v_bisassumedInfo:=False)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.SetPropertyValue Failed for " & sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyRiskChanges")
    'Exit Sub
    'End If
    'End If
    '

    'm_lReturn = m_oDataset.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
    'Exit Sub
    'End If
    '
    ' Save it to the DataBase

    'm_lReturn = m_oGIS.SaveToDB(v_sGisDataModelCode:=m_sDataModelCode, r_sXMLDataset:=sXMLDataSet)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
    'Exit Sub
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TestStub Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TestStub", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

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

            Dim sObjectName As String = ""
            Dim vOIKeyArray As Object

            sObjectName = m_sDataModelCode.Trim() & "_" & "Output"


            m_lReturn = m_oDataset.LoadFromXML(v_sXMLDataSetDef:=m_sXMLDataSetDef, v_sXMLDataSet:=m_sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDataset.GetAllOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataset.GetAllOIKey Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vOIKeyArray) Then
                Return result
            End If


            For i As Integer = 0 To vOIKeyArray.GetUpperBound(0)



                m_lReturn = m_oDataset.DelObjectInstance(v_sObjectName:=sObjectName, v_sOiKey:=CStr(vOIKeyArray(i)))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataset.DelObjectInstance Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

            'Return as XmL String

            m_lReturn = m_oDataset.ReturnAsXML(r_sXMLDataSetDef:=m_sXMLDataSetDef, r_sXMLDataset:=m_sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearOutputObjects")
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