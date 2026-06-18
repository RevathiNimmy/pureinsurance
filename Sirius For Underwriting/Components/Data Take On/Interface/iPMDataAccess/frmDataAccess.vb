Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDataAccess
	Inherits System.Windows.Forms.Form
	
	Private vAlarmDetailIDArray As Object
	Private vConstructionTypeDetailIDArray As Object
	Private vContentsDeductibleDetailIDArray As Object
	Private vCoverDetailIDArray As Object
	Private vFloodingAreaDetailIDArray As Object
	Private vGaragedDetailIDArray As Object
	Private vHouseHoldDeductibleDetailIDArray As Object
	Private vImportDetailIDArray As Object
	Private vIndexLinkingDetailIDArray As Object
	Private vIslandDetailIDArray As Object
	Private vMoneyDetailIDArray As Object
	Private vMotorCoverDetailIDArray As Object
	Private vVehicleModelDetailIDArray As Object
	Private vVehicleMakeDetailIDArray As Object
	Private vVehicleUseDetailIDArray As Object
	Private vNCBYearsDetailIDArray As Object
	Private vAnalysisCodeDetailIDArray As Object
	Private vAccumulationCodeDetailIDArray As Object
	Private vAreaCodeDetailIDArray As Object
	Private oAccess As clsAccessFunctions
	Private vBusinessTypeDetailIDArray As Object
	Private bIsMotor As Boolean
	
	Private m_lInsuranceFolderCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_lProductID As Integer
	Private m_lScreenID As Integer
	Private m_lLeadAgentCnt As Integer
	Private m_iSourceID As Integer
	
	Private m_lReturn As Integer
	Private m_lErrorNumber As Integer
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Close()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: cmdWriteToDatabase_Click()
	'
	' Description:
	'
	' History: 15/08/2000 MSB - Created.
	'
	' ***************************************************************** '
	Private Sub cmdWriteToDatabase_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdWriteToDatabase.Click
		
		Dim lResult As Integer
		
		Try 
			
			cmdCancel.Enabled = False
			
			'Set all the objects up
			lResult = CType(oAccess, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to initialise", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Open the access database
			lResult = oAccess.OpenAccessDatabase()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to Open Access Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Open the Sirius database
			lResult = oAccess.OpenSiriusDatabase()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to Open Sirius Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Process the UD lookups
			lResult = ProcessUDLookups()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process UD lookup details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Create\Add the Island Lookup
			'What's this for?
			'    lResult = AddIslandLookup
			
			'    If (lResult <> PMTrue) Then
			'        MsgBox "Failed to Add\Create Island Lookup", vbCritical, "Error"
			'        lResult = oAccess.Terminate
			'        cmdCancel.Enabled = True
			'        Exit Sub
			'    End If
			
			
			'Process the PM lookups
			lResult = ProcessPMLookups()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process PM lookup details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Process Lead Agents
			lResult = ProcessLeadAgent()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process Lead Agent", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			lResult = oAccess.ReopenParty()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to reopen party services", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Process Personal Client
			lResult = ProcessPersonalClient()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process Personal Clients", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			lResult = oAccess.ReopenParty()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to reopen party services", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'Process Corporate Client
			lResult = ProcessCorporateClient()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process Corporate Clients", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'    Process Motor Policies
			lResult = ProcessMotorPolicies()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process Motor Policies", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			'    Process Household Policies
			lResult = ProcessHouseholdPolicies()
			
			If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to process Household Policies", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                oAccess.Dispose()
				cmdCancel.Enabled = True
				Exit Sub
			End If
			
			lResult = oAccess.CloseAccessDatabase()
			
			lResult = oAccess.CloseDatabase()
			
			cmdCancel.Enabled = True
			MessageBox.Show("Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdWriteToDatabase_Click Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="cmdWriteToDatabase_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: ProcessPMLookups
	'
	' Description:
	'
	' History: 30/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Private Function ProcessPMLookups() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = ProcessPMLookup(sTableName:="accumulation code", sPMTableName:="accumulation", bArchitecture:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = ProcessPMLookup(sTableName:="analysis codes", sPMTableName:="analysis_code", bArchitecture:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = ProcessPMLookup(sTableName:="contact type", sPMTableName:="contact_type", bArchitecture:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = ProcessPMLookup(sTableName:="island", sPMTableName:="area", bArchitecture:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = ProcessPMLookup(sTableName:="branch", sPMTableName:="source", bArchitecture:=True)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = ProcessPMLookup(sTableName:="business type", sPMTableName:="business_type", bArchitecture:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPMLookups Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessPMLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: ProcessPMLookup
	'
	' Description:
	'
	' History: 30/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Private Function ProcessPMLookup(ByRef sTableName As String, ByRef sPMTableName As String, ByRef bArchitecture As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim vArray As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lblProgress.Visible = True
			lblProgress.Text = "Processing Lookup - " & sTableName
			

			m_lReturn = GetPMLookup(vArray:=CStr(vArray), sTableName:=sTableName)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				lblProgress.Visible = False
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = WritePMLookup(vArray:=vArray, sPMTableName:=sPMTableName, bArchitecture:=bArchitecture)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				'        Exit Function
			End If
			

			m_lReturn = UpdatePMLookup(vArray:=CStr(vArray), sTableName:=sTableName)
			
			lblProgress.Visible = False
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPMLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessPMLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetPMLookup
	'
	' Description:
	'
	' History: 30/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
    Private Function GetPMLookup(ByRef vArray As String, ByRef sTableName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL, sID, sCode, sCount As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case sTableName
                Case "accumulation code"
                    sID = "accumulation code id"
                    sCode = "accumulation code"
                    sCount = "accumulation count"
                Case "analysis codes"
                    sID = "analysis code id"
                    sCode = "analysis code"
                    sCount = "analysis count"
                Case "area code"
                    sID = "area code id"
                    sCode = "area code"
                    sCount = "area count"
                Case "branch"
                    sID = "branch id"
                    sCode = "branch"
                    sCount = "branch count"
                Case "business type"
                    sID = "business type id"
                    sCode = "business type"
                    sCount = "business count"
                Case "status"
                    sID = "status id"
                    sCode = "status"
                    sCount = "status count"
                Case "island"
                    sID = "island id"
                    sCode = "island"
                    sCount = "area count"
                Case Else
                    vArray = Nothing
                    Return result
            End Select

            sSQL = "SELECT [{id}], [{code}], [{count}] FROM [{table_name}]"

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=sID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="count", vValue:=sCount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get " & sTableName & " Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetPMLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	'
	' Name: WritePMLookup
	'
	' Description:
	'
	' History: 30/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Private Function WritePMLookup(ByRef vArray( ,  ) As Object, ByRef sPMTableName As String, ByRef bArchitecture As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim vPMColumns, vAddColumns, vPMValues, vValues As Object
		Dim sColumns As New StringBuilder
        Dim sID As String = ""
        Dim lID, lCode, lDescription, lEffectiveDate, lIsDeleted, lCaptionId, lQuickCode, lCountryId As Integer
		Dim bFound As Boolean
		Dim sCaption As String = ""
		Dim lCaption As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Not Information.IsArray(vArray) Then
				Return result
			End If
			
			If bArchitecture Then

				m_lReturn = m_oArchLookupMaintenance.GetColumns(v_sTableName:=sPMTableName, v_sProductCode:="SirSol", r_vColumns:=vPMColumns)
			Else

				m_lReturn = m_oLookupMaintenance.GetColumns(v_sTableName:=sPMTableName, v_sProductCode:="SirSol", r_vColumns:=vPMColumns)
			End If
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			sColumns = New StringBuilder("")

			For lTemp As Integer = vPMColumns.GetLowerBound(1) To vPMColumns.GetUpperBound(1)

                sColumns.Append("," & CStr(vPMColumns(1, lTemp)))
            Next lTemp

            sColumns = New StringBuilder(sColumns.ToString().Substring(1))

            If bArchitecture Then

                m_lReturn = m_oArchLookupMaintenance.GetDetails(v_sTableName:=sPMTableName, v_sColumns:=sColumns.ToString(), r_vData:=vPMValues)
            Else

                m_lReturn = m_oLookupMaintenance.GetDetails(v_sTableName:=sPMTableName, v_sColumns:=sColumns.ToString(), r_vData:=vPMValues)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ReDim vAddColumns(1, vPMColumns.GetUpperBound(1))


            For lTemp As Integer = vPMColumns.GetLowerBound(1) To vPMColumns.GetUpperBound(1)


                vAddColumns(0, lTemp) = vPMColumns(2, lTemp)


                vAddColumns(1, lTemp) = vPMColumns(1, lTemp)
            Next lTemp

            sID = (sPMTableName & "_id").ToUpper()

            'Find the id column

            For lTemp As Integer = vPMColumns.GetLowerBound(1) To vPMColumns.GetUpperBound(1)

                Select Case CStr(vPMColumns(1, lTemp)).ToUpper()
                    Case sID
                        lID = lTemp
                    Case "CODE"
                        lCode = lTemp
                    Case "DESCRIPTION"
                        lDescription = lTemp
                    Case "CAPTION_ID"
                        lCaptionId = lTemp
                    Case "EFFECTIVE_DATE"
                        lEffectiveDate = lTemp
                    Case "IS_DELETED"
                        lIsDeleted = lTemp
                    Case "COUNTRY_ID"
                        lCountryId = lTemp
                    Case "QUICK_CODE"
                        lQuickCode = lTemp
                End Select
            Next lTemp

            'Loop around the values
            '    For lTemp = LBound(vArray, 2) To UBound(vArray, 2)
            '
            '        If (CStr(vArray(2, lTemp)) = "") Or (CStr(vArray(2, lTemp)) = "0") Then
            '            vArray(2, lTemp) = -1
            '
            '            ReDim vValues(UBound(vPMColumns, 2))
            '
            '            For lTemp2 = LBound(vPMColumns, 2) To UBound(vPMColumns, 2)
            '                vValues(lTemp2) = Null
            '            Next lTemp2
            '
            '            vValues(lID) = 0
            '            vValues(lCaptionId) = 0
            '            vValues(lCode) = vArray(1, lTemp)
            '            vValues(lDescription) = vArray(1, lTemp)
            '            vValues(lIsDeleted) = 0
            '            vValues(lEffectiveDate) = Now
            '
            '            If (lQuickCode <> 0) Then
            '                vValues(lQuickCode) = vArray(1, lTemp)
            '            End If
            '
            '            m_lReturn = m_oLookupMaintenance.Add(v_sTableName:=sPMTableName, _
            ''                                                 v_vColumns:=vAddColumns, _
            ''                                                 r_vValues:=vValues)
            '
            '            If (m_lReturn <> PMTrue) Then
            '                WritePMLookup = PMFalse
            '                Exit Function
            '            End If
            '
            '            vArray(2, lTemp) = vValues(lID)
            '
            '        Else
            '            If IsArray(vPMValues) Then
            '                For lTemp2 = LBound(vPMValues, 2) To UBound(vPMValues, 2)
            '
            '                    If (vPMValues(lID, lTemp2) = vArray(2, lTemp)) Then
            '                        ReDim vValues(UBound(vPMColumns, 2))
            '
            '                        For lTemp3 = LBound(vPMColumns, 2) To UBound(vPMColumns, 2)
            '                            vValues(lTemp3) = vPMValues(lTemp3, lTemp2)
            '                        Next lTemp3
            '
            '                        vValues(lCode) = vArray(1, lTemp)
            '                        vValues(lDescription) = vArray(1, lTemp)
            '
            '                        If (lQuickCode <> 0) Then
            '                            vValues(lQuickCode) = vArray(1, lTemp)
            '                        End If
            '
            '                        m_lReturn = m_oLookupMaintenance.Update(v_sTableName:=sPMTableName, _
            ''                                                                v_vColumns:=vAddColumns, _
            ''                                                                v_vValues:=vValues)
            '
            '                        If (m_lReturn <> PMTrue) Then
            '                            WritePMLookup = PMFalse
            '                            Exit Function
            '                        End If
            '
            '                        Exit For
            '                    End If
            '
            '                Next lTemp2
            '
            '            End If
            '
            '        End If
            '    Next lTemp

            'Rewritten to cope with setting up stuff on both...
            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                bFound = False

                If Information.IsArray(vPMValues) Then

                    For lTemp2 As Integer = vPMValues.GetLowerBound(1) To vPMValues.GetUpperBound(1)

                        'Check the code as well
                        'And the description




                        If (vPMValues(lID, lTemp2).Equals(vArray(2, lTemp))) Or (CStr(vPMValues(lCode, lTemp2)).Trim().ToUpper() = CStr(vArray(1, lTemp)).Trim().ToUpper()) Or (CStr(vPMValues(lDescription, lTemp2)).Trim().ToUpper() = CStr(vArray(1, lTemp)).Trim().ToUpper()) Then
                            'Oops, is it deleted?

                            If CDbl(vPMValues(lIsDeleted, lTemp2)) = 0 Then
                                'We don't do a damn thing...

                                ReDim vValues(vPMColumns.GetUpperBound(1))

                                'Set the value, just in case


                                vArray(2, lTemp) = vPMValues(lID, lTemp2)

                                '                    m_lReturn = m_oLookupMaintenance.Update(v_sTableName:=sPMTableName, _
                                ''                                                            v_vColumns:=vAddColumns, _
                                ''                                                            v_vValues:=vValues)

                                bFound = True

                                Exit For
                            End If
                        End If
                    Next lTemp2
                End If

                If Not bFound Then

                    vArray(2, lTemp) = -1


                    ReDim vValues(vPMColumns.GetUpperBound(1))


                    For lTemp2 As Integer = vPMColumns.GetLowerBound(1) To vPMColumns.GetUpperBound(1)


                        vValues(lTemp2) = DBNull.Value
                    Next lTemp2


                    sCaption = CStr(vArray(1, lTemp))

                    m_lReturn = GetCaptionID(v_sCaption:=sCaption, r_lCaptionID:=lCaption)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    vValues(lID) = 0

                    vValues(lCaptionId) = lCaption


                    vValues(lCode) = vArray(1, lTemp)


                    vValues(lDescription) = vArray(1, lTemp)

                    vValues(lIsDeleted) = 0

                    vValues(lEffectiveDate) = DateTime.Today

                    If lQuickCode <> 0 Then


                        vValues(lQuickCode) = vArray(1, lTemp)
                    End If

                    If lCountryId <> 0 Then

                        vValues(lCountryId) = 2
                    End If

                    If bArchitecture Then

                        m_lReturn = m_oArchLookupMaintenance.Add(v_sTableName:=sPMTableName, v_vColumns:=vAddColumns, r_vValues:=vValues)
                    Else

                        m_lReturn = m_oLookupMaintenance.Add(v_sTableName:=sPMTableName, v_vColumns:=vAddColumns, r_vValues:=vValues)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If



                    vArray(2, lTemp) = vValues(lID)
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WritePMLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WritePMLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdatePMLookup
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    'Private Function UpdatePMLookup(ByRef vArray As String, ByRef sTableName As String) As Integer
    Private Function UpdatePMLookup(ByRef vArray As Object, ByRef sTableName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL, sID, sCode, sCount As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            Select Case sTableName
                Case "accumulation code"
                    sID = "accumulation code id"
                    sCode = "accumulation code"
                    sCount = "accumulation count"
                Case "analysis codes"
                    sID = "analysis code id"
                    sCode = "analysis code"
                    sCount = "analysis count"
                Case "area code"
                    sID = "area code id"
                    sCode = "area code"
                    sCount = "area count"
                Case "branch"
                    sID = "branch id"
                    sCode = "branch"
                    sCount = "branch count"
                Case "business type"
                    sID = "business type id"
                    sCode = "business type"
                    sCount = "business count"
                Case "status"
                    sID = "status id"
                    sCode = "status"
                    sCount = "status count"
                Case "island"
                    sID = "island id"
                    sCode = "island"
                    sCount = "area count"
                Case Else
                    vArray = Nothing
                    Return result
            End Select

            sSQL = "UPDATE [{table_name}] SET [{table_count}] = {cnt} WHERE [{table_id}] = {id}"
            'developer guide no.18
            'For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
            For lTemp As Integer = LBound(vArray, 1) To UBound(vArray, 1)

                m_oAccessDatabase.Parameters.Clear()

                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="table_count", vValue:=sCount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=vArray(2, lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=vArray, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="table_id", vValue:=sID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'm_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=vArray(0, lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=vArray, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update " & sTableName & " Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePMLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdatePMLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessUDLookups
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessUDLookups() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessUDLookup(sTableName:="alarm", lTableId:=17)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="construction type", lTableId:=6)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="contents deductible", lTableId:=8)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="cover", lTableId:=15)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="flooding area", lTableId:=7)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="garaged", lTableId:=18)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="household deductible", lTableId:=8)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="high risk", lTableId:=10)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="import", lTableId:=19)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="index linking", lTableId:=14)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="island", lTableId:=2)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="money", lTableId:=9)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="motor cover", lTableId:=15)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="vehicle make", lTableId:=4)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="vehicle model", lTableId:=3)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="vehicle use", lTableId:=16)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessUDLookup(sTableName:="ncb years", lTableId:=22)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Lead Agent ?!?!?!?!
            '    sSqlString = "SELECT [lead agent id], [lead agent], [lead count] FROM [lead agent]"
            '    sSQLName = "Retrieve Lead Agent"
            '    bStoredProc = False
            '
            '    lResult = m_oAccessDatabase.SQLSelect(sSqlString, sSQLName, bStoredProc, , m_vLeadAgentArray)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessUDLookups Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessUDLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessUDLookup
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessUDLookup(ByRef sTableName As String, ByRef lTableId As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lblProgress.Visible = True
            lblProgress.Text = "Processing Lookup - " & sTableName

            m_lReturn = GetUDLookup(vArray:=vArray, sTableName:=sTableName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lblProgress.Visible = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = WriteUDLookup(vArray:=vArray, lTableId:=lTableId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                '        Exit Function
            End If

            m_lReturn = UpdateUDLookup(vArray:=vArray, sTableName:=sTableName)

            lblProgress.Visible = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessUDLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessUDLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUDLookup
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetUDLookup(ByRef vArray(,) As Object, ByRef sTableName As String) As Integer 

        Dim result As Integer = 0 
        Dim sSQL, sShortName As String 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT [{table_name} id], [{table_name}], [{short_table_name} count] FROM [{table_name}]"

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sTableName = "vehicle make" Then
                sShortName = "make"
            ElseIf (sTableName = "vehicle model") Then
                sShortName = "model"
            ElseIf (sTableName = "vehicle use") Then
                sShortName = "use"
            Else
                sShortName = sTableName

                If sShortName.IndexOf(" "c) >= 0 Then
                    sShortName = sShortName.Substring(0, sShortName.IndexOf(" "c))
                End If
            End If

            m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="short_table_name", vValue:=sShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get " & sTableName & " Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUDLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetUDLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteUDLookup
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function WriteUDLookup(ByRef vArray(,) As Object, ByRef lTableId As Integer) As Integer

        Dim result As Integer = 0
        Dim vLookupArray(,) As Object

        'Positional constants for the array
        Const ACHLookupDetailId As Integer = 0
        Const ACHLookupHeaderId As Integer = 1
        Const ACHCaptionId As Integer = 2
        Const ACHCode As Integer = 3
        Const ACHDescription As Integer = 4
        Const ACHIsDeleted As Integer = 5
        Const ACHEffectiveDate As Integer = 6
        Const ACHParentId As Integer = 7
        Const ACHLookupHeaderIndsId As Integer = 8

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            ReDim vLookupArray(ACHLookupHeaderIndsId, vArray.GetUpperBound(1))

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                If CStr(vArray(2, lTemp)) = "" Or (CStr(vArray(2, lTemp)) = "0") Then

                    vLookupArray(ACHLookupDetailId, lTemp) = -1
                Else


                    vLookupArray(ACHLookupDetailId, lTemp) = vArray(2, lTemp)
                End If

                vLookupArray(ACHLookupHeaderId, lTemp) = lTableId

                vLookupArray(ACHCaptionId, lTemp) = 0


                vLookupArray(ACHCode, lTemp) = vArray(1, lTemp)


                vLookupArray(ACHDescription, lTemp) = vArray(1, lTemp)

                vLookupArray(ACHIsDeleted, lTemp) = 0

                vLookupArray(ACHEffectiveDate, lTemp) = DateTime.Today

                vLookupArray(ACHParentId, lTemp) = -1


                vLookupArray(ACHLookupHeaderIndsId, lTemp) = DBNull.Value

            Next lTemp


            m_lReturn = m_oUserDetail.DataTakeOn(vLookupDetails:=vLookupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                vArray(2, lTemp) = vLookupArray(ACHLookupDetailId, lTemp)
            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteUDLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WriteUDLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUDLookup
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateUDLookup(ByRef vArray(,) As Object, ByRef sTableName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim sShortName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            sShortName = sTableName

            If sShortName = "vehicle make" Then
                sShortName = "make"
            ElseIf (sTableName = "vehicle model") Then
                sShortName = "model"
            ElseIf (sTableName = "vehicle use") Then
                sShortName = "use"
            Else
                If sShortName.IndexOf(" "c) >= 0 Then
                    sShortName = sShortName.Substring(0, sShortName.IndexOf(" "c))
                End If
            End If


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                sSQL = "UPDATE [{table_name}] SET [{short_table_name} count] = {cnt} WHERE [{table_name} id] = {id}"

                m_oAccessDatabase.Parameters.Clear()

                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="short_table_name", vValue:=sShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=CStr(vArray(2, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=CStr(vArray(0, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update " & sTableName & " Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUDLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdateUDLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessLeadAgent
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessLeadAgent() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lblProgress.Visible = True
            lblProgress.Text = "Processing Lead Agents"

            m_lReturn = GetLeadAgent(vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lblProgress.Visible = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then
                proProgress.Visible = True
                proProgress.Minimum = 0

                proProgress.Maximum = vArray.GetUpperBound(1) + 1
            End If


            m_lReturn = WriteLeadAgent(vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                '        Exit Function
            End If

            m_lReturn = UpdateLeadAgent(vArray:=vArray)

            lblProgress.Visible = False
            proProgress.Visible = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessLeadAgent Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessLeadAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLeadAgent
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetLeadAgent(ByRef vArray(,) As Object) As Integer 

        Dim result As Integer = 0 
        Dim sSQL As String = "" 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT [Lead Agent ID], [Lead Agent], [Lead Count]" & Strings.Chr(13) & Strings.Chr(10) & _
                   "FROM [Lead Agent]" & Strings.Chr(13) & Strings.Chr(10)

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Lead Agent Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLeadAgent Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetLeadAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteLeadAgent
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function WriteLeadAgent(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                proProgress.Value = lTemp


                If (CStr(vArray(2, lTemp)) = "") Or (CStr(vArray(2, lTemp)) = "0") Then
                    'New one - maybe

                    m_lReturn = CheckParty(lPartyCnt:=lPartyCnt, sShortName:=CStr(vArray(1, lTemp)))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        MessageBox.Show("Error calling Check Party - " & CStr(vArray(1, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_oParty.PartyCnt = lPartyCnt

                    vArray(2, lTemp) = lPartyCnt

                    If lPartyCnt <> 0 Then

                        m_lReturn = m_oParty.GetDetails

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Error calling Get Details - " & lPartyCnt, "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Else
                    'Old one


                    m_oParty.PartyCnt = vArray(2, lTemp)

                    m_lReturn = m_oParty.GetDetails

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Error calling Get Details - " & lPartyCnt, "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                'Need to set up the client details for Agent
                'Set up surname


                m_oParty.Name = vArray(1, lTemp)
                'Set up shortname


                m_oParty.Shortname = vArray(1, lTemp)
                'Set up party type

                m_oParty.PartyType = "AG"


                If CDbl(vArray(2, lTemp)) = 0 Then
                    'Set up address 1

                    m_oParty.Address1 = "Default Agent Address Line 1"
                    'Set up address 2

                    m_oParty.Address2 = "Default Agent Address Line 2"
                    'Set up address 3

                    m_oParty.Address3 = "Default Agent Address Line 3"
                    'Set up address 4

                    m_oParty.Address4 = "Default Agent Address Line 4"
                    'Set up postcode

                    m_oParty.PostalCode = ""


                    m_oParty.ContactArray = ""

                End If


                If (CStr(vArray(2, lTemp)) = "") Or (CStr(vArray(2, lTemp)) = "0") Then
                    'Create the party

                    m_lReturn = m_oParty.CreateParty
                Else
                    'Nothing worthy of update
                    '            m_lReturn = m_oParty.UpdateParty
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Error Creating the party - " & CStr(vArray(1, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                vArray(2, lTemp) = m_oParty.PartyCnt

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteLeadAgent Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WriteLeadAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateLeadAgent
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateLeadAgent(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            sSQL = "UPDATE [Lead Agent] SET [Lead Count] = {cnt} WHERE [Lead Agent ID] = {id}"


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                m_oAccessDatabase.Parameters.Clear()


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=CStr(vArray(2, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=CStr(vArray(0, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Lead Agent Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLeadAgent Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdateLeadAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessPersonalClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessPersonalClient() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lblProgress.Visible = True
            lblProgress.Text = "Processing Personal Clients"

            m_lReturn = GetPersonalClient(vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lblProgress.Visible = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then
                proProgress.Visible = True
                proProgress.Minimum = 0

                proProgress.Maximum = vArray.GetUpperBound(1) + 1


                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    proProgress.Value = lTemp


                    m_lReturn = WritePersonalClient(vArray:=vArray, lTemp:=lTemp)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If

                Next lTemp
            End If


            m_lReturn = UpdatePersonalClient(vArray:=vArray)

            lblProgress.Visible = False
            proProgress.Visible = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessPersonalClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessPersonalClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPersonalClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetPersonalClient(ByRef vArray(,) As Object) As Integer 

        Dim result As Integer = 0 
        Dim sSQL As String = "" 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT P.ID, P.[Type of Client], P.[Client Code], " & Strings.Chr(13) & Strings.Chr(10) & _
                   "T.[Title], P.[Last Name], P.Forename, P.Initials, A.[Lead Count], I.[Area Count]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[File Code], P.[Address Line1], P.[Address Line2], P.[Address Line3], P.[Address Line4], P.Postcode, P.[Date of Birth]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Contact Type Id1], P.Description1, P.[Area Code1], P.[Tel Number1], P.[Ext Number1]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Contact Type Id2], P.Description2, P.[Area Code2], P.[Tel Number2], P.[Ext Number2]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Contact Type Id3], P.Description3, P.[Area Code3], P.[Tel Number3], P.[Ext Number3]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Contact Type Id4], P.Description4, P.[Area Code4], P.[Tel Number4], P.[Ext Number4]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Contact Type Id5], P.Description5, P.[Area Code5], P.[Tel Number5], P.[Ext Number5]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Contact Type Id6], P.Description6, P.[Area Code6], P.[Tel Number6], P.[Ext Number6]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "P.[Party Count] FROM [Personal] P, [Title] T, [Lead Agent] A, [Island] I" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE P.[Title ID] = T.[Title ID]" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AND P.[Lead Agent ID] = A.[Lead Agent ID]" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AND P.[Area Code ID] = I.[Island ID]" & Strings.Chr(13) & Strings.Chr(10)

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Personal Client Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPersonalClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetPersonalClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WritePersonalClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function WritePersonalClient(ByRef vArray(,) As Object, ByRef lTemp As Integer) As Integer

        Dim result As Integer = 0
        Dim vContactArray(,) As Object
        Dim lPartyCnt As Integer
        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (CStr(vArray(m_cPersonalPartyCount, lTemp)) = "") Or (CStr(vArray(m_cPersonalPartyCount, lTemp)) = "0") Then

                'New one - maybe

                m_lReturn = CheckParty(lPartyCnt:=lPartyCnt, sShortName:=CStr(vArray(m_cPersonalClientCode, lTemp)))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Error calling Check Party - " & CStr(vArray(m_cPersonalClientCode, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_oParty.PartyCnt = lPartyCnt

                vArray(m_cPersonalPartyCount, lTemp) = lPartyCnt

                If lPartyCnt <> 0 Then

                    m_lReturn = m_oParty.GetDetails

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Error calling Get Details - " & lPartyCnt, "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    Return result

                End If
            Else
                'Old one


                m_oParty.PartyCnt = vArray(m_cPersonalPartyCount, lTemp)

                m_lReturn = m_oParty.GetDetails

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Error calling Get Details - " & lPartyCnt, "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result

            End If

            'Need to set up the client details for personal details
            'Set up surname


            m_oParty.Name = vArray(m_cPersonalLastName, lTemp)
            'Set up initial


            m_oParty.Initials = vArray(m_cPersonalInitials, lTemp)
            'Set up first name


            m_oParty.Forename = vArray(m_cPersonalForename, lTemp)
            'Set up shortname


            m_oParty.Shortname = vArray(m_cPersonalClientCode, lTemp)
            'Set up party type

            m_oParty.PartyType = "PC"

            'Set up address 1


            m_oParty.Address1 = vArray(m_cPersonalAddressLine1, lTemp)
            'Set up address 2


            m_oParty.Address2 = vArray(m_cPersonalAddressLine2, lTemp)
            'Set up address 3


            m_oParty.Address3 = vArray(m_cPersonalAddressLine3, lTemp)
            'Set up address 4


            m_oParty.Address4 = vArray(m_cPersonalAddressLine4, lTemp)
            'Set up postcode


            m_oParty.PostalCode = vArray(m_cPersonalPostcode, lTemp)

            'add date of birth


            m_oParty.DateOfBirth = vArray(m_cPersonalDateOfBirth, lTemp)
            'add agent cnt


            m_oParty.AgentCnt = vArray(m_cPersonalLeadAgent, lTemp)
            'add area id


            m_oParty.AreaId = vArray(m_cPersonalAreaCode, lTemp)
            'add file code


            m_oParty.FileCode = vArray(m_cPersonalFileCode, lTemp)
            'add party table code


            m_oParty.PartyTitleCode = vArray(m_cPersonalTitle, lTemp)


            m_oParty.CurrencyCode = "BHS"



            m_oParty.PaymentMethodCode = Nothing



            m_oParty.PaymentTermCode = Nothing



            m_oParty.RenewalStopCodeId = Nothing


            m_oParty.NationalityCode = "BAHAMIAN"


            m_oParty.SourceID = g_iSourceID


            sTemp = CStr(vArray(m_cPersonalForename, lTemp)).Trim() & " "

            '    If (Trim$(vArray(m_cPersonalInitials, lTemp)) <> "") Then
            '        sTemp = sTemp & Trim$(vArray(m_cPersonalInitials, lTemp)) & " "
            '    End If


            sTemp = sTemp & CStr(vArray(m_cPersonalLastName, lTemp)).Trim()


            m_oParty.ResolvedName = sTemp

            'Create the personal contact details
            m_lReturn = CreatePersonalContactDetails(vArray:=vArray, lTemp:=lTemp, vContactArray:=vContactArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_oParty.ContactArray = vContactArray

            vContactArray = Nothing


            m_lReturn = m_oParty.CreateParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error Creating the party - " & CStr(vArray(m_cPersonalClientCode, lTemp)), Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            vArray(m_cPersonalPartyCount, lTemp) = m_oParty.PartyCnt


            m_lReturn = UpdateOrion(lPartyCnt:=m_oParty.PartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error Creating the party on Orion - " & CStr(vArray(m_cPersonalClientCode, lTemp)), Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WritePersonalClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WritePersonalClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdatePersonalClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdatePersonalClient(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            sSQL = "UPDATE [Personal] SET [Party count] = {cnt} WHERE [ID] = {id}"


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                m_oAccessDatabase.Parameters.Clear()


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=CStr(vArray(m_cPersonalPartyCount, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=CStr(vArray(m_cPersonalID, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Personal Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePersonalClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdatePersonalClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreatePersonalContactDetails
    '
    ' Description:
    '
    ' History: 18/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Private Function CreatePersonalContactDetails(ByRef vArray(,) As Object, ByRef lTemp As Integer, ByRef vContactArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lID As Integer
        Dim sCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Put all the contact details into an array

            vContactArray = Nothing


            If (CStr(vArray(m_cPersonalContactType1, lTemp)) = "") Or (CStr(vArray(m_cPersonalContactType1, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cPersonalContactType1, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim vContactArray(4, 0)


            vContactArray(0, 0) = sCode


            vContactArray(1, 0) = vArray(m_cPersonalAreaCode1, lTemp)


            vContactArray(2, 0) = vArray(m_cPersonalTelNumber1, lTemp)


            vContactArray(3, 0) = vArray(m_cPersonalExtNumber1, lTemp)


            vContactArray(4, 0) = vArray(m_cPersonalDescription1, lTemp)


            If (CStr(vArray(m_cPersonalContactType2, lTemp)) = "") Or (CStr(vArray(m_cPersonalContactType2, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cPersonalContactType2, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 1)


            vContactArray(0, 1) = sCode


            vContactArray(1, 1) = vArray(m_cPersonalAreaCode2, lTemp)


            vContactArray(2, 1) = vArray(m_cPersonalTelNumber2, lTemp)


            vContactArray(3, 1) = vArray(m_cPersonalExtNumber2, lTemp)


            vContactArray(4, 1) = vArray(m_cPersonalDescription2, lTemp)


            If (CStr(vArray(m_cPersonalContactType3, lTemp)) = "") Or (CStr(vArray(m_cPersonalContactType3, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cPersonalContactType3, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 2)


            vContactArray(0, 2) = sCode


            vContactArray(1, 2) = vArray(m_cPersonalAreaCode3, lTemp)


            vContactArray(2, 2) = vArray(m_cPersonalTelNumber3, lTemp)


            vContactArray(3, 2) = vArray(m_cPersonalExtNumber3, lTemp)


            vContactArray(4, 2) = vArray(m_cPersonalDescription3, lTemp)


            If (CStr(vArray(m_cPersonalContactType4, lTemp)) = "") Or (CStr(vArray(m_cPersonalContactType4, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cPersonalContactType4, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 3)


            vContactArray(0, 3) = sCode


            vContactArray(1, 3) = vArray(m_cPersonalAreaCode4, lTemp)


            vContactArray(2, 3) = vArray(m_cPersonalTelNumber4, lTemp)


            vContactArray(3, 3) = vArray(m_cPersonalExtNumber4, lTemp)


            vContactArray(4, 3) = vArray(m_cPersonalDescription4, lTemp)


            If (CStr(vArray(m_cPersonalContactType5, lTemp)) = "") Or (CStr(vArray(m_cPersonalContactType5, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cPersonalContactType5, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 4)


            vContactArray(0, 4) = sCode


            vContactArray(1, 4) = vArray(m_cPersonalAreaCode5, lTemp)


            vContactArray(2, 4) = vArray(m_cPersonalTelNumber5, lTemp)


            vContactArray(3, 4) = vArray(m_cPersonalExtNumber5, lTemp)


            vContactArray(4, 4) = vArray(m_cPersonalDescription5, lTemp)


            If (CStr(vArray(m_cPersonalContactType6, lTemp)) = "") Or (CStr(vArray(m_cPersonalContactType6, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cPersonalContactType6, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 5)


            vContactArray(0, 5) = sCode


            vContactArray(1, 5) = vArray(m_cPersonalAreaCode6, lTemp)


            vContactArray(2, 5) = vArray(m_cPersonalTelNumber6, lTemp)


            vContactArray(3, 5) = vArray(m_cPersonalExtNumber6, lTemp)


            vContactArray(4, 5) = vArray(m_cPersonalDescription6, lTemp)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePersonalContactDetails Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CreatePersonalContactDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessCorporateClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessCorporateClient() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lblProgress.Visible = True
            lblProgress.Text = "Processing Corporate Clients"

            m_lReturn = GetCorporateClient(vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lblProgress.Visible = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then
                proProgress.Visible = True
                proProgress.Minimum = 0

                proProgress.Maximum = vArray.GetUpperBound(1) + 1


                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    proProgress.Value = lTemp


                    m_lReturn = WriteCorporateClient(vArray:=vArray, lTemp:=lTemp)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If

                Next lTemp
            End If

            m_lReturn = UpdateCorporateClient(vArray:=vArray)

            lblProgress.Visible = False
            proProgress.Visible = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCorporateClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessCorporateClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCorporateClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetCorporateClient(ByRef vArray(,) As Object) As Integer 

        Dim result As Integer = 0 
        Dim sSQL As String = "" 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT C.ID, C.[Type of Client], C.[Client Code], " & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Trading Name], A.[Lead Count], I.[Area Count], C.[File Code], " & _
                   "C.[Address Line1], C.[Address Line2], C.[Address Line3], C.[Address Line4], C.Postcode," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Contact Type1], C.Description1, C.[Area Code1], C.[Tel Number1], C.[Ext Number1]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Contact Type2], C.Description2, C.[Area Code2], C.[Tel Number2], C.[Ext Number2]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Contact Type3], C.Description3, C.[Area Code3], C.[Tel Number3], C.[Ext Number3]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Contact Type4], C.Description4, C.[Area Code4], C.[Tel Number4], C.[Ext Number4]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Contact Type5], C.Description51, C.[Area Code5], C.[Tel Number5], C.[Ext Number5]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Contact Type6], C.Description6, C.[Area Code6], C.[Tel Number6], C.[Ext Number6]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "C.[Party Count] FROM [Corporate] C, [Lead Agent] A, [Island] I" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE C.[Lead Agent ID] = A.[Lead Agent ID]" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AND C.[Area Code ID] = I.[Island ID]" & Strings.Chr(13) & Strings.Chr(10)

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Corporate Client Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCorporateClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetCorporateClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteCorporateClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function WriteCorporateClient(ByRef vArray(,) As Object, ByRef lTemp As Integer) As Integer

        Dim result As Integer = 0
        Dim vContactArray As String = ""
        Dim lPartyCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (CStr(vArray(m_cCorporatePartyCount, lTemp)) = "") Or (CStr(vArray(m_cCorporatePartyCount, lTemp)) = "0") Then
                'New one - maybe

                m_lReturn = CheckParty(lPartyCnt:=lPartyCnt, sShortName:=CStr(vArray(m_cCorporateClientCode, lTemp)))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Error calling Check Party - " & CStr(vArray(m_cCorporateClientCode, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_oParty.PartyCnt = lPartyCnt

                vArray(m_cCorporatePartyCount, lTemp) = lPartyCnt

                If lPartyCnt <> 0 Then

                    m_lReturn = m_oParty.GetDetails

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Error calling Get Details - " & lPartyCnt, "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    Return result

                End If
            Else
                'Old one


                m_oParty.PartyCnt = vArray(m_cCorporatePartyCount, lTemp)

                m_lReturn = m_oParty.GetDetails

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Error calling Get Details - " & lPartyCnt, "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result

            End If



            m_oParty.Name = vArray(m_cCorporateTradingName, lTemp)


            m_oParty.Shortname = vArray(m_cCorporateClientCode, lTemp)

            m_oParty.PartyType = "CC"



            m_oParty.Address1 = vArray(m_cCorporateAddressLine1, lTemp)


            m_oParty.Address2 = vArray(m_cCorporateAddressLine2, lTemp)


            m_oParty.Address3 = vArray(m_cCorporateAddressLine3, lTemp)


            m_oParty.Address4 = vArray(m_cCorporateAddressLine4, lTemp)


            m_oParty.PostalCode = vArray(m_cCorporatePostcode, lTemp)



            m_oParty.AgentCnt = vArray(m_cCorporateLeadAgent, lTemp)


            m_oParty.FileCode = vArray(m_cCorporateFileCode, lTemp)


            m_oParty.AreaId = vArray(m_cCorporateAreaCode, lTemp)

            'Create the Corporate contact details
            m_lReturn = CreateCorporateContactDetails(vArray:=vArray, lTemp:=lTemp, vContactArray:=vContactArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error Creating Contact Details", "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return m_lReturn
            End If


            m_oParty.ContactArray = vContactArray

            vContactArray = ""


            m_oParty.CurrencyCode = "BHS"



            m_oParty.PaymentMethodCode = Nothing



            m_oParty.PaymentTermCode = Nothing



            m_oParty.RenewalStopCodeId = Nothing


            m_oParty.SourceID = g_iSourceID



            m_oParty.ResolvedName = vArray(m_cCorporateTradingName, lTemp)


            m_lReturn = m_oParty.CreateParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error Creating the party - " & CStr(vArray(m_cCorporateClientCode, lTemp)), Application.ProductName)
                Return m_lReturn
            End If



            vArray(m_cCorporatePartyCount, lTemp) = m_oParty.PartyCnt


            m_lReturn = UpdateOrion(lPartyCnt:=m_oParty.PartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error Creating the party on Orion - " & CStr(vArray(m_cPersonalClientCode, lTemp)), Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteCorporateClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WriteCorporateClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateCorporateClient
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateCorporateClient(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            sSQL = "UPDATE [Corporate] SET [Party count] = {cnt} WHERE [ID] = {id}"


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                m_oAccessDatabase.Parameters.Clear()


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=CStr(vArray(m_cCorporatePartyCount, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=CStr(vArray(m_cCorporateID, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Corporate Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCorporateClient Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdateCorporateClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateCorporateContactDetails
    '
    ' Description:
    '
    ' History: 18/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function CreateCorporateContactDetails(ByRef vArray(,) As Object, ByRef lTemp As Integer, ByRef vContactArray As Object) As Integer

        Dim result As Integer = 0
        Dim sCode As String = ""
        Dim lID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Put all the corporate contact details into an array

            vContactArray = Nothing


            If (CStr(vArray(m_cCorporateContactType1, lTemp)) = "") Or (CStr(vArray(m_cCorporateContactType1, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cCorporateContactType1, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim vContactArray(4, 0)


            vContactArray(0, 0) = sCode


            vContactArray(1, 0) = vArray(m_cCorporateAreaCode1, lTemp)


            vContactArray(2, 0) = vArray(m_cCorporateTelNumber1, lTemp)


            vContactArray(3, 0) = vArray(m_cCorporateExtNumber1, lTemp)


            vContactArray(4, 0) = vArray(m_cCorporateDescription1, lTemp)


            If (CStr(vArray(m_cCorporateContactType2, lTemp)) = "") Or (CStr(vArray(m_cCorporateContactType2, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cCorporateContactType2, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 1)


            vContactArray(0, 1) = sCode


            vContactArray(1, 1) = vArray(m_cCorporateAreaCode2, lTemp)


            vContactArray(2, 1) = vArray(m_cCorporateTelNumber2, lTemp)


            vContactArray(3, 1) = vArray(m_cCorporateExtNumber2, lTemp)


            vContactArray(4, 1) = vArray(m_cCorporateDescription2, lTemp)


            If (CStr(vArray(m_cCorporateContactType3, lTemp)) = "") Or (CStr(vArray(m_cCorporateContactType3, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cCorporateContactType3, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 2)


            vContactArray(0, 2) = sCode


            vContactArray(1, 2) = vArray(m_cCorporateAreaCode3, lTemp)


            vContactArray(2, 2) = vArray(m_cCorporateTelNumber3, lTemp)


            vContactArray(3, 2) = vArray(m_cCorporateExtNumber3, lTemp)


            vContactArray(4, 2) = vArray(m_cCorporateDescription3, lTemp)


            If (CStr(vArray(m_cCorporateContactType4, lTemp)) = "") Or (CStr(vArray(m_cCorporateContactType4, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cCorporateContactType4, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 3)


            vContactArray(0, 3) = sCode


            vContactArray(1, 3) = vArray(m_cCorporateAreaCode4, lTemp)


            vContactArray(2, 3) = vArray(m_cCorporateTelNumber4, lTemp)


            vContactArray(3, 3) = vArray(m_cCorporateExtNumber4, lTemp)


            vContactArray(4, 3) = vArray(m_cCorporateDescription4, lTemp)


            If (CStr(vArray(m_cCorporateContactType5, lTemp)) = "") Or (CStr(vArray(m_cCorporateContactType5, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cCorporateContactType5, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 4)


            vContactArray(0, 4) = sCode


            vContactArray(1, 4) = vArray(m_cCorporateAreaCode5, lTemp)


            vContactArray(2, 4) = vArray(m_cCorporateTelNumber5, lTemp)


            vContactArray(3, 4) = vArray(m_cCorporateExtNumber5, lTemp)


            vContactArray(4, 4) = vArray(m_cCorporateDescription5, lTemp)


            If (CStr(vArray(m_cCorporateContactType6, lTemp)) = "") Or (CStr(vArray(m_cCorporateContactType6, lTemp)) = "0") Then
                Return result
            End If


            lID = CInt(vArray(m_cCorporateContactType6, lTemp))

            m_lReturn = GetContactType(lID:=lID, sCode:=sCode)

            If sCode = "" Then
                Return result
            End If

            ReDim Preserve vContactArray(4, 5)


            vContactArray(0, 5) = sCode


            vContactArray(1, 5) = vArray(m_cCorporateAreaCode6, lTemp)


            vContactArray(2, 5) = vArray(m_cCorporateTelNumber6, lTemp)


            vContactArray(3, 5) = vArray(m_cCorporateExtNumber6, lTemp)


            vContactArray(4, 5) = vArray(m_cCorporateDescription6, lTemp)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateCorporateContactDetails Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CreateCorporateContactDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetContactType
    '
    ' Description:
    '
    ' History: 05/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetContactType(ByRef lID As Integer, ByRef sCode As String) As Integer 

        Dim result As Integer = 0 
        Static vArray(,) As Object
        Dim sSQLString, sSQLName As String 
        Dim bStoredProc As Boolean 
        Dim lResult As gPMConstants.PMEReturnCode 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sCode = ""

            If Not Information.IsArray(vArray) Then
                sSQLString = "SELECT [contact type id], [contact type] FROM [contact type]"

                sSQLName = "Retrieve Contact Type"
                bStoredProc = False


                vArray = Nothing

                lResult = m_oAccessDatabase.SQLSelect(sSQL:=sSQLString, sSQLName:=sSQLName, bStoredProcedure:=bStoredProc, vResultArray:=vArray)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            If Information.IsArray(vArray) Then

                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    If CDbl(vArray(0, lTemp)) = lID Then

                        sCode = CStr(vArray(1, lTemp))
                        Exit For
                    End If
                Next lTemp
            End If


            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetContactType Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetContactType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckParty
    '
    ' Description:
    '
    ' History: 24/01/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CheckParty(ByRef lPartyCnt As Integer, ByRef sShortName As String) As Integer 

        Dim result As Integer = 0 
        Dim vArray(,) As Object 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lPartyCnt = 0

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="shortname", vValue:=sShortName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckPartySQL, sSQLName:=ACCheckPartyName, bStoredProcedure:=ACCheckPartyStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                lPartyCnt = CInt(vArray(0, 0))
            End If


            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckParty Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CheckParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetResolvedName
    '
    ' Description:
    '
    ' History: 24/01/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetResolvedName(ByRef lPartyCnt As Integer, ByRef sResolvedName As String) As Integer 

        Dim result As Integer = 0 
        Dim vArray(,) As Object 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetResolvedNameSQL, sSQLName:=ACGetResolvedNameName, bStoredProcedure:=ACGetResolvedNameStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                sResolvedName = CStr(vArray(0, 0))
            End If


            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetResolvedName Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetResolvedName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateOrion
    '
    ' Description:
    '
    ' History: 16/02/2001 Tomo - Created.  Nicked from uctPartyPCControl
    '
    ' ***************************************************************** '
    Private Function UpdateOrion(ByRef lPartyCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    If Not m_oBusiness.IsOrionInstalled Then
            '        Exit Function
            '    End If

            If m_oOrionUpdate Is Nothing Then
                Dim temp_m_oOrionUpdate As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oOrionUpdate, "bSIROrionUpdate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oOrionUpdate = temp_m_oOrionUpdate

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oOrionUpdate.SiriusToOrion(v_lPartyCnt:=lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oOrionUpdate.Dispose()

            m_oOrionUpdate = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrion Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: ProcessMotorPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessMotorPolicies() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lblProgress.Visible = True
            lblProgress.Text = "Processing Motor Policies"

            m_lReturn = GetMotorPolicies(vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then
                proProgress.Visible = True
                proProgress.Minimum = 0

                proProgress.Maximum = vArray.GetUpperBound(1) + 1


                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    proProgress.Value = lTemp


                    m_lReturn = WriteMotorPolicies(vArray:=vArray, lTemp:=lTemp)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If
                Next lTemp

            End If


            m_lReturn = UpdateMotorPolicies(vArray:=vArray)

            lblProgress.Visible = False
            proProgress.Visible = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessMotorPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessMotorPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetMotorPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetMotorPolicies(ByRef vArray(,) As Object) As Integer 

        Dim result As Integer = 0 
        Dim sSQL As String = "" 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT MP.[Motor Policy ID], MP.[Client Code], MP.[Policy Number], MP.[Status Id]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Product Type], MP.[Branch id], MP.[Lead Agent ID], MP.[Type of Client]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Analysis Code id], MP.[Business Type id], MP.Regarding, MP.[Cover from], MP.[Cover to]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Original Inception], MP.Renewal, MP.[Proposal Date], MP.Issued," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Old Policy Number], MP.[Co-insurance], MP.[Refer at renewal], MP.[Motor Cover id]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Island id], MP.[Vehicle Year], MP.[Vehicle Make id], MP.[Vehicle Model id]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Vehicle Model Type], MP.[Vehicle Use id], MP.[Vehicle Value], MP.[Engine CC]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Vehicle Serial Number], MP.[Plate Number], MP.[Certificate Number]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Time of issue] , MP.Mileage, MP.[Alarm id], MP.[Garaged id], MP.[Import id]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Insurance History], MP.[NCB Received], MP.[NCB years], MP.[Multi Vehicle Discount]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Claim Loading], MP.[Driver Name1], MP.[Driver Island1], MP.[Driver Sex1]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Driver DOB1], MP.[Drive License Date1], MP.[Driver Name2], MP.[Driver Island2]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Driver Sex2], MP.[Driver DOB2], MP.[Drive License Date2], MP.[Driver Name3]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Driver Island3], MP.[Driver Sex3], MP.[Driver DOB3], MP.[Drive License Date3]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Driver Name4], MP.[Driver Island4], MP.[Driver Sex4], MP.[Driver DOB4]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Drive License Date4], MP.[Driver Name5], MP.[Driver Island5], MP.[Driver Sex5]," & Strings.Chr(13) & Strings.Chr(10) & _
                   "MP.[Driver DOB5], MP.[Drive License Date5], MP.[Insurance File Count]" & Strings.Chr(13) & Strings.Chr(10) & _
                   "FROM [Motor Policy] MP" & Strings.Chr(13) & Strings.Chr(10)

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Motor Policy Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMotorPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetMotorPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteMotorPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function WriteMotorPolicies(ByRef vArray(,) As Object, ByRef lTemp As Integer) As Integer

        Dim result As Integer = 0
        Dim bDuff As Boolean
        Dim sPolicyRef, sResolvedName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (CStr(vArray(m_cMotorInsuranceFileCount, lTemp)) = "") Or (CStr(vArray(m_cMotorInsuranceFileCount, lTemp)) = "0") Then
                'New one

                m_oInsuranceFile.InsuranceFolderCnt = 0

                m_oInsuranceFile.InsuranceFileCnt = 0

                m_oInsuranceFile.InsuranceFolderCode = ""
            Else
                'Old one


                m_oInsuranceFile.InsuranceFileCnt = vArray(m_cMotorInsuranceFileCount, lTemp)

                m_lReturn = m_oInsuranceFile.GetDetails

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Error calling Get Details - " & CStr(vArray(m_cMotorInsuranceFileCount, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result

            End If

            m_lReturn = GetMotorIds(vMotorArray:=vArray, lTemp:=lTemp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error calling Get Motor Ids", "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bDuff = False


            If CStr(vArray(m_cMotorCoverFrom, lTemp)) = "" Then


                m_oInsuranceFile.CoverStartDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cMotorCoverFrom, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.CoverStartDate = vArray(m_cMotorCoverFrom, lTemp)
            End If


            If CStr(vArray(m_cMotorCoverTo, lTemp)) = "" Then


                m_oInsuranceFile.ExpiryDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cMotorCoverTo, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.ExpiryDate = vArray(m_cMotorCoverTo, lTemp)
            End If


            If CStr(vArray(m_cMotorProposalDate, lTemp)) = "" Then


                m_oInsuranceFile.ProposalDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cMotorProposalDate, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.ProposalDate = vArray(m_cMotorProposalDate, lTemp)
            End If


            If CStr(vArray(m_cMotorOriginalInception, lTemp)) = "" Then


                m_oInsuranceFile.InceptionDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cMotorOriginalInception, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.InceptionDate = vArray(m_cMotorOriginalInception, lTemp)
            End If


            If CStr(vArray(m_cMotorIssued, lTemp)) = "" Then


                m_oInsuranceFile.DateIssued = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cMotorIssued, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.DateIssued = vArray(m_cMotorIssued, lTemp)
            End If


            If CStr(vArray(m_cMotorRenewal, lTemp)) = "" Then


                m_oInsuranceFile.RenewalDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cMotorRenewal, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.RenewalDate = vArray(m_cMotorRenewal, lTemp)
            End If


            If CStr(vArray(m_cMotorDriverDOB1, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverDOB1, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverDOB2, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverDOB2, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverDOB3, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverDOB3, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverDOB4, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverDOB4, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverDOB5, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverDOB5, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverLicenseDate1, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverLicenseDate1, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverLicenseDate2, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverLicenseDate2, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverLicenseDate3, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverLicenseDate3, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverLicenseDate4, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverLicenseDate4, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cMotorDriverLicenseDate5, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cMotorDriverLicenseDate5, lTemp)) < #1/1/1900#)
            End If

            If bDuff Then

                MessageBox.Show("Policy " & CStr(vArray(m_cMotorOldPolicyNumber, lTemp)) & " has invalid date", "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If


            m_oInsuranceFile.InsuranceFileStructure = "PMB"

            m_oInsuranceFile.PolicyTypeId = 5

            m_oInsuranceFile.ProductID = m_lProductID


            m_oInsuranceFile.AnnualPremium = 0

            m_oInsuranceFile.ThisPremium = 0

            m_oInsuranceFile.NetPremium = 0


            m_lReturn = GetResolvedName(lPartyCnt:=CInt(vArray(m_cMotorClientCode, lTemp)), sResolvedName:=sResolvedName)


            m_oInsuranceFile.InsuredName = sResolvedName



            m_oInsuranceFile.InsuranceFolderDescription = vArray(m_cMotorRegarding, lTemp)



            m_oInsuranceFile.AnalysisCodeId = vArray(m_cMotorAnalysisCode, lTemp)


            If CStr(vArray(m_cMotorBranch, lTemp)) = "0" Then
                m_iSourceID = g_iSourceID
            Else

                m_iSourceID = CInt(vArray(m_cMotorBranch, lTemp))
            End If


            m_oInsuranceFile.SourceID = m_iSourceID


            If CStr(vArray(m_cMotorBusinessType, lTemp)) = "" Then

                m_oInsuranceFile.BusinessTypeID = 0
            Else


                m_oInsuranceFile.BusinessTypeID = vArray(m_cMotorBusinessType, lTemp)
            End If


            m_lLeadAgentCnt = CInt(vArray(m_cMotorAgentCode, lTemp))

            m_oInsuranceFile.LeadAgentCnt = m_lLeadAgentCnt


            If CStr(vArray(m_cMotorStatus, lTemp)) = "1" Then


                m_oInsuranceFile.InsuranceFileStatusID = Nothing
            Else

                m_oInsuranceFile.InsuranceFileStatusID = 1
            End If


            If CStr(vArray(m_cMotorReferAtRenewal, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cMotorReferAtRenewal, lTemp) = 1
            Else

                vArray(m_cMotorReferAtRenewal, lTemp) = 0
            End If



            m_oInsuranceFile.IsReferredAtRenewal = vArray(m_cMotorReferAtRenewal, lTemp)


            If CStr(vArray(m_cMotorPolicyNumber, lTemp)) = "" Then
                m_lReturn = GetPolicyNumber(sPolicyRef:=sPolicyRef)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Failed To Get Policy Number For" & CStr(vArray(m_cMotorOldPolicyNumber, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                vArray(m_cMotorPolicyNumber, lTemp) = sPolicyRef

            End If



            m_oInsuranceFile.InsuranceFolderCode = vArray(m_cMotorPolicyNumber, lTemp)


            m_oInsuranceFile.InsuranceRef = vArray(m_cMotorPolicyNumber, lTemp)


            m_oInsuranceFile.OldPolicyNumber = vArray(m_cMotorOldPolicyNumber, lTemp)



            m_oInsuranceFile.InsuranceHolderCnt = CInt(vArray(m_cMotorClientCode, lTemp))


            m_oInsuranceFile.InsuredCnt = CInt(vArray(m_cMotorClientCode, lTemp))


            m_oInsuranceFile.CurrencyCode = "BHS"


            m_oInsuranceFile.RenewalFrequency = "ANNUAL"

            'Create the policy

            m_lReturn = m_oInsuranceFile.CreatePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error creating Motor Policy " & CStr(vArray(m_cMotorPolicyNumber, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return m_lReturn
            End If



            vArray(m_cMotorInsuranceFileCount, lTemp) = m_oInsuranceFile.InsuranceFileCnt


            m_lInsuranceFileCnt = m_oInsuranceFile.InsuranceFileCnt

            m_lInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt

            'Now do the GIS...
            m_lReturn = SetGISMotorValues(vMotorArray:=vArray, lCount:=lTemp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error setting GIS data for " & CStr(vArray(m_cMotorPolicyNumber, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteMotorPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WriteMotorPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetMotorIds
    '
    ' Description:
    '
    ' History: 16/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetMotorIds(ByRef vMotorArray(,) As Object, ByRef lTemp As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQLString, sSQLName As String
        Dim bStoredProc As Boolean
        Dim lResult As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vArray = Nothing


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorClientCode, lTemp)) Or IsNothing(vMotorArray(m_cMotorClientCode, lTemp))) Then


                If CStr(vMotorArray(m_cMotorClientCode, lTemp)) = "" Then


                    vMotorArray(m_cMotorClientCode, lTemp) = DBNull.Value
                Else
                    'Retrieve Motor Client Code
                    '        If (vMotorArray(m_cMotorTypeOfClient, lTemp) = "Personal") Then

                    sSQLString = "SELECT [Party Count] FROM [Personal] " & _
                                 "WHERE [Client Code] = '" & CStr(vMotorArray(m_cMotorClientCode, lTemp)) & "'"
                    '        Else

                    sSQLString = sSQLString & " UNION SELECT [Party Count] FROM [Corporate] " & _
                                 "WHERE [Client Code] = '" & CStr(vMotorArray(m_cMotorClientCode, lTemp)) & "'"
                    '        End If

                    sSQLName = "Retrieve Motor Client Code"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorClientCode, lTemp) = vArray(0, 0)
                    Else


                        vMotorArray(m_cMotorClientCode, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorAgentCode, lTemp)) Or IsNothing(vMotorArray(m_cMotorAgentCode, lTemp))) Then


                If CStr(vMotorArray(m_cMotorAgentCode, lTemp)) = "" Then


                    vMotorArray(m_cMotorAgentCode, lTemp) = DBNull.Value
                Else
                    'Retrieve Household Analysis code

                    sSQLString = "SELECT [lead count] FROM [lead agent] " & _
                                 "WHERE [lead agent id] = " & CStr(vMotorArray(m_cMotorAgentCode, lTemp))

                    sSQLName = "Retrieve Agent"
                    bStoredProc = False


                    vArray = Nothing

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorAgentCode, lTemp) = vArray(0, 0)
                    Else


                        vMotorArray(m_cMotorAgentCode, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorAnalysisCode, lTemp)) Or IsNothing(vMotorArray(m_cMotorAnalysisCode, lTemp))) Then


                If CStr(vMotorArray(m_cMotorAnalysisCode, lTemp)) = "" Then


                    vMotorArray(m_cMotorAnalysisCode, lTemp) = DBNull.Value
                Else
                    'Retrieving the analysis code text

                    sSQLString = "SELECT [Analysis Count] FROM [analysis codes] " & _
                                 "WHERE [Analysis Code Id] = " & CStr(vMotorArray(m_cMotorAnalysisCode, lTemp))

                    sSQLName = "Retrieve Analysis Code"
                    bStoredProc = False


                    vArray = Nothing

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorAnalysisCode, lTemp) = vArray(0, 0)
                    Else


                        vMotorArray(m_cMotorAnalysisCode, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorIsland, lTemp)) Or IsNothing(vMotorArray(m_cMotorIsland, lTemp))) Then


                If CStr(vMotorArray(m_cMotorIsland, lTemp)) = "" Then


                    vMotorArray(m_cMotorIsland, lTemp) = DBNull.Value
                Else
                    'Retrieving the analysis code text

                    sSQLString = "SELECT [Island Count] FROM [Island] " & _
                                 "WHERE [Island Id] = " & CStr(vMotorArray(m_cMotorIsland, lTemp))

                    sSQLName = "Retrieve Island"
                    bStoredProc = False


                    vArray = Nothing

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorIsland, lTemp) = vArray(0, 0)
                    Else


                        vMotorArray(m_cMotorIsland, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorBranch, lTemp)) Or IsNothing(vMotorArray(m_cMotorBranch, lTemp))) Then


                If CStr(vMotorArray(m_cMotorBranch, lTemp)) <> "" Then
                    'Retrieving the branch text

                    sSQLString = "SELECT [branch count] FROM branch " & _
                                 "WHERE [branch id] = " & CStr(vMotorArray(m_cMotorBranch, lTemp))

                    sSQLName = "Retrieving Branch Text"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorBranch, lTemp) = vArray(0, 0)
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorBusinessType, lTemp)) Or IsNothing(vMotorArray(m_cMotorBusinessType, lTemp))) Then


                If CStr(vMotorArray(m_cMotorBusinessType, lTemp)) <> "" Then
                    'Retrieving the Business type

                    sSQLString = "SELECT [business count] FROM [business type] " & _
                                 "WHERE [business type id] = " & CStr(vMotorArray(m_cMotorBusinessType, lTemp))

                    sSQLName = "Retrieving Business Type"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorBusinessType, lTemp) = vArray(0, 0)
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorCover, lTemp)) Or IsNothing(vMotorArray(m_cMotorCover, lTemp))) Then


                If CStr(vMotorArray(m_cMotorCover, lTemp)) <> "" Then

                    If CStr(vMotorArray(m_cMotorCover, lTemp)) = "1" Then
                        m_lProductID = 5005
                    End If


                    If CStr(vMotorArray(m_cMotorCover, lTemp)) = "2" Then
                        m_lProductID = 5003
                    End If


                    If CStr(vMotorArray(m_cMotorCover, lTemp)) = "3" Then
                        m_lProductID = 5002
                    End If


                    If CStr(vMotorArray(m_cMotorCover, lTemp)) = "4" Then
                        m_lProductID = 5007
                    End If


                    If CStr(vMotorArray(m_cMotorCover, lTemp)) = "5" Then
                        m_lProductID = 5003
                    End If

                    'Retrieve Cover

                    sSQLString = "SELECT [motor count] FROM [motor cover] WHERE [motor cover id] = " & _
                                 CStr(vMotorArray(m_cMotorCover, lTemp))
                    sSQLName = "Retrieve Cover Details"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorCover, lTemp) = vArray(0, 0)
                    End If


                    vArray = Nothing
                End If
            Else
                m_lProductID = 5003
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorVehicleModel, lTemp)) Or IsNothing(vMotorArray(m_cMotorVehicleModel, lTemp))) Then


                If CStr(vMotorArray(m_cMotorVehicleModel, lTemp)) <> "" Then
                    'Retrieve Vehicle Model

                    sSQLString = "SELECT [model count] FROM [vehicle model] " & _
                                 "WHERE [vehicle model id] = " & CStr(vMotorArray(m_cMotorVehicleModel, lTemp))

                    sSQLName = "Retrieve Vehicle Model"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorVehicleModel, lTemp) = vArray(0, 0)
                    Else

                        vMotorArray(m_cMotorVehicleModel, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorUse, lTemp)) Or IsNothing(vMotorArray(m_cMotorUse, lTemp))) Then


                If CStr(vMotorArray(m_cMotorUse, lTemp)) <> "" Then
                    'Retrieve Use

                    sSQLString = "SELECT [use count] FROM [vehicle use] " & _
                                 "WHERE [vehicle use id] = " & CStr(vMotorArray(m_cMotorUse, lTemp))

                    sSQLName = "Retrieve Motor Use"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorUse, lTemp) = vArray(0, 0)
                    Else

                        vMotorArray(m_cMotorUse, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorAlarm, lTemp)) Or IsNothing(vMotorArray(m_cMotorAlarm, lTemp))) Then


                If CStr(vMotorArray(m_cMotorAlarm, lTemp)) <> "" Then
                    'Retrieve Alarm

                    sSQLString = "SELECT [alarm count] FROM alarm WHERE [alarm id] = " & _
                                 CStr(vMotorArray(m_cMotorAlarm, lTemp))

                    sSQLName = "Retrieve Motor Alarm"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorAlarm, lTemp) = vArray(0, 0)
                    Else

                        vMotorArray(m_cMotorAlarm, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorImport, lTemp)) Or IsNothing(vMotorArray(m_cMotorImport, lTemp))) Then


                If CStr(vMotorArray(m_cMotorImport, lTemp)) <> "" Then
                    'Retrieve Import

                    sSQLString = "SELECT [import count] FROM import WHERE [import id] = " & _
                                 CStr(vMotorArray(m_cMotorImport, lTemp))
                    sSQLName = "Retrieve Motor Import"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorImport, lTemp) = vArray(0, 0)
                    Else

                        vMotorArray(m_cMotorImport, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorVehicleMake, lTemp)) Or IsNothing(vMotorArray(m_cMotorVehicleMake, lTemp))) Then


                If CStr(vMotorArray(m_cMotorVehicleMake, lTemp)) <> "" Then
                    'Retrieve Vehicle Make

                    sSQLString = "SELECT [make count] FROM [vehicle make] WHERE [vehicle make id] = " & _
                                 CStr(vMotorArray(m_cMotorVehicleMake, lTemp))
                    sSQLName = "Retrieve Vehicle Make"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorVehicleMake, lTemp) = vArray(0, 0)
                    Else

                        vMotorArray(m_cMotorVehicleMake, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vMotorArray(m_cMotorGaraged, lTemp)) Or IsNothing(vMotorArray(m_cMotorGaraged, lTemp))) Then


                If CStr(vMotorArray(m_cMotorGaraged, lTemp)) <> "" Then
                    'Retrieve Garaged

                    sSQLString = "SELECT [garaged count] FROM garaged WHERE [garaged id] = " & _
                                 CStr(vMotorArray(m_cMotorGaraged, lTemp))

                    sSQLName = "Retrieve Motor Garaged"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vMotorArray(m_cMotorGaraged, lTemp) = vArray(0, 0)
                    Else

                        vMotorArray(m_cMotorGaraged, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If

            '    If Not IsNull(vMotorArray(m_cMotorNCBYears, lTemp)) Then
            '
            '        If (vMotorArray(m_cMotorNCBYears, lTemp) <> "") Then
            '        'Retrieve NCB Years
            '        sSQLString = "SELECT [ncb count] FROM [ncb years] WHERE [ncb years id] = " & _
            ''                                                                    vMotorArray(m_cMotorNCBYears, lTemp)
            '
            '        sSQLName = "Retrieve NCB Years"
            '        bStoredProc = False
            '
            '        lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)
            '
            '        If (lResult <> PMTrue) Then
            '            GetMotorIds = lResult
            '            Exit Function
            '        End If
            '
            '        If IsArray(vArray) Then
            '            vMotorArray(m_cMotorNCBYears, lTemp) = vArray(0, 0)
            '        Else
            '            vMotorArray(m_cMotorNCBYears, lTemp) = ""
            '        End If
            '
            '        vArray = Nothing
            '        End If
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMotorIds Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetMotorIds", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetGISMotorValues
    '
    ' Description: Updates the GIS and business object.
    '
    ' ***************************************************************** '
    Public Function SetGISMotorValues(ByRef vMotorArray(,) As Object, ByRef lCount As Integer) As Integer

        Dim result As Integer = 0
        Dim vPolicyLinkId As Integer
        Dim vOIKeyArray, vChildOIKeyArray As Object
        Dim sOIKey, sParentObjectName, sChildObjectName, sParentOIKey, sChildOIKey, sMotorOIKey, sCoreOIKey As String
        Dim vRiskDetails, vRiskTypeDetails As Object
        Dim iNoOfDrivers As Integer
        Dim lPolicyLinkId, lPolicyBinderID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetGISInterfaceObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetBusinessObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                'Tidy up
                m_lReturn = RemoveGISInterfaceObject()

                Return result
            End If

            'Here we're doing the Motor

            '    m_lInsuranceFileCnt = vMotorArray(m_cMotorInsuranceFileCount, lCount)


            m_lRiskTypeID = 3
            m_lScreenID = 3
            m_lRiskID = 0

            m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oBusiness.ProductID = m_lProductID

            m_oBusiness.RiskId = m_lRiskID

            m_oBusiness.RiskTypeId = m_lRiskTypeID

            m_oBusiness.ScreenId = m_lScreenID

            'Get the Risk - if it doesn't exist create it, we need the number for the GIS

            m_lReturn = m_oBusiness.GetRisk(vRiskArray:=vRiskDetails, vRiskTypeArray:=vRiskTypeDetails)

            '    If (m_lRiskID = 0) Then

            m_lRiskID = m_oBusiness.RiskId
            '    End If



            vPolicyLinkId = Nothing

            '    m_lReturn = m_oGIS.LoadFromDB(v_sGISDataModelCode:="RSA", _
            'r_vInsuranceFileCnt:=m_lCombinedKey, _
            'r_vPolicyLinkId:=vPolicyLinkId)

            m_lReturn = m_oGIS.LoadFromDB(v_sGisDataModelCode:="RSA", r_vInsuranceFileCnt:=m_lInsuranceFolderCnt, r_vPolicyLinkID:=vPolicyLinkId, r_vRiskID:=m_lRiskID)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    'Create a new one

                    '            m_lReturn = m_oGIS.NewDataSet(v_sGISDataModelCode:="RSA", _
                    'r_lPolicyLinkId:=lPolicyLinkId, _
                    'r_sTopOIKey:=sOIKey, _
                    'v_vInsuranceFileCnt:=m_lCombinedKey)

                    m_lReturn = m_oGIS.NewDataSet(v_sGisDataModelCode:="RSA", r_lPolicyLinkID:=lPolicyLinkId, r_sTopOIKey:=sOIKey, v_vInsuranceFileCnt:=m_lInsuranceFolderCnt, v_vRiskID:=m_lRiskID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else

                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load from DB in GIS", vApp:=ACAPP, vClass:=ACClass, vMethod:="SetGISMotorValues")

                    m_lReturn = RemoveGISInterfaceObject()

                    Return result
                End If
            End If


            If Not (Convert.IsDBNull(vPolicyLinkId) Or IsNothing(vPolicyLinkId)) Then
                lPolicyLinkId = vPolicyLinkId
            End If

            ' Get the Top Level OI Key
            m_lReturn = m_oGIS.GetAllOIKey(v_sObjectName:="RSA_policy_binder", r_vOIKeyArray:=vOIKeyArray)

            If Information.IsArray(vOIKeyArray) Then


                sOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))
            End If

            vOIKeyArray = Nothing

            If sOIKey = "" Then
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:="RSA_policy_binder", r_sOIKey:=sOIKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Save it, we need the number
            m_lReturn = m_oGIS.SaveToDB()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lPolicyBinderID = 0 Then

                m_lReturn = m_oBusiness.GetBinder(lPolicyLinkId:=lPolicyLinkId, sDataModel:="RSA", lPolicyBinderID:=lPolicyBinderID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'ADDING THE MOTOR DETAILS BELOW
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "motor"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sMotorOIKey = sChildOIKey
                ' Else use the first one
            Else



                sMotorOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If

            'ADD ALL THE MOTOR DETAILS INTO THE RSA_MOTOR TABLE HERE
            'Cover
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="cover", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorCover, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Vehicle Year
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="vehicle_year", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorVehicleYear, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'vehicle make
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="vehicle_make", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorVehicleMake, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'vehicle model
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="vehicle_model", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorVehicleModel, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'vehicle model type
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="vehicle_model_type", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorVehicleModelType, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'vehicle value
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="vehicle_value", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorVehicleValue, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'use
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="usage", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorUse, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'engine cc
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="engine_cc", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorEngineCC, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'plate number
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="plate_number", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorPlateNumber, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'time of issue
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="certificate_issued", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorTimeOfIssue, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'alarm
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="alarm", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorAlarm, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'import
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="imported_from", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorImport, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'garaged
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="garaged", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorGaraged, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'serial number
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="vehicle_serial_number", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorVehicleSerialNumber, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'certificate no
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="certificate_number", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorCertificateNumber, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'mileage
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="mileage", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorMileage, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'claim loading
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="claim_loading", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorClaimLoading, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If CStr(vMotorArray(m_cMotorInsuranceHistory, lCount)).ToUpper() = "TRUE" Then

                vMotorArray(m_cMotorInsuranceHistory, lCount) = 1
            Else

                vMotorArray(m_cMotorInsuranceHistory, lCount) = 0
            End If

            'insurance history
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="insurance_history", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorInsuranceHistory, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If CStr(vMotorArray(m_cMotorNCBReceived, lCount)).ToUpper() = "TRUE" Then

                vMotorArray(m_cMotorNCBReceived, lCount) = 1
            Else

                vMotorArray(m_cMotorNCBReceived, lCount) = 0
            End If

            'ncb Received
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="ncb_recd", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorNCBReceived, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ncb years
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="ncdyears", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorNCBYears, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'multi vehicle discount
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="motor", v_sPropertyName:="multi_vehicle_dis", v_sOIKey:=sMotorOIKey, v_vPropertyValue:=vMotorArray(m_cMotorMultiVehicleDiscount, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sParentObjectName = "motor"
            sParentOIKey = sMotorOIKey
            sChildObjectName = "driver"

            'Need to find out how many drivers there are for this policy
            iNoOfDrivers = 0






            If CStr(vMotorArray(m_cMotorDriverName5, lCount)) <> "" Then
                iNoOfDrivers = 5
            ElseIf CStr(vMotorArray(m_cMotorDriverName4, lCount)) <> "" Then
                iNoOfDrivers = 4
            ElseIf CStr(vMotorArray(m_cMotorDriverName3, lCount)) <> "" Then
                iNoOfDrivers = 3
            ElseIf CStr(vMotorArray(m_cMotorDriverName2, lCount)) <> "" Then
                iNoOfDrivers = 2
            ElseIf CStr(vMotorArray(m_cMotorDriverName1, lCount)) <> "" Then
                iNoOfDrivers = 1
            End If

            'Safest thing to do is to delete the drivers and recreate them...

            'See how many drivers there are on the policy
            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            If Information.IsArray(vChildOIKeyArray) Then

                For lTemp As Integer = vChildOIKeyArray.GetLowerBound(0) To vChildOIKeyArray.GetUpperBound(0)


                    m_lReturn = m_oGIS.DelObjectInstance(v_sObjectName:=sChildObjectName, v_sOIKey:=CStr(vChildOIKeyArray(lTemp)))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next lTemp
            End If

            'need to create as many instances as there are drivers
            For iDriverCount As Integer = 1 To iNoOfDrivers

                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Select Case iDriverCount
                    Case 1
                        ' Get the property name
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="name", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverName1, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Get the property gender
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="xxgender", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverSex1, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property date of birth
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_birth", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverDOB1, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property driving licence date
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_licence", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverLicenseDate1, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case 2
                        ' Get the property name
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="name", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverName2, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Get the property gender
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="xxgender", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverSex2, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property date of birth
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_birth", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverDOB2, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property driving licence date
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_licence", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverLicenseDate2, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case 3
                        ' Get the property name
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="name", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverName3, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Get the property gender
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="xxgender", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverSex3, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property date of birth
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_birth", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverDOB3, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property driving licence date
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_licence", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverLicenseDate3, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case 4
                        ' Get the property name
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="name", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverName4, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Get the property gender
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="xxgender", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverSex4, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property date of birth
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_birth", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverDOB4, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property driving licence date
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_licence", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverLicenseDate4, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case 5
                        ' Get the property name
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="name", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverName5, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Get the property gender
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="xxgender", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverSex5, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property date of birth
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_birth", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverDOB5, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Get the property driving licence date
                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="driver", v_sPropertyName:="date_of_licence", v_sOIKey:=sChildOIKey, v_vPropertyValue:=vMotorArray(m_cMotorDriverLicenseDate5, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case Else
                End Select
            Next iDriverCount

            'ADDING THE RSA CORE DETAILS HERE
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "core"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCoreOIKey = sChildOIKey

            Else



                sCoreOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If

            'ADD THE CORE STUFF HERE
            'Starting with island
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="island", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vMotorArray(m_cMotorIsland, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now the biggy - update the GIS
            m_lReturn = m_oGIS.SaveToDB()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = RemoveGISInterfaceObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                '        Exit Function
            End If

            m_lReturn = RemoveBusinessObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the GIS Motor Values", vApp:=ACAPP, vClass:=ACClass, vMethod:="SetGISMotorValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateMotorPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateMotorPolicies(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            sSQL = "UPDATE [Motor Policy] SET [Insurance File Count] = {cnt}, [Policy Number] = {policy_number} WHERE [Motor Policy ID] = {id}"

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                m_oAccessDatabase.Parameters.Clear()


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=CStr(vArray(m_cMotorInsuranceFileCount, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If CStr(vArray(m_cMotorPolicyNumber, lTemp)) = "" Then


                    vArray(m_cMotorPolicyNumber, lTemp) = DBNull.Value
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="policy_number", vValue:=CStr(vArray(m_cMotorPolicyNumber, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=CStr(vArray(m_cMotorID, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Motor Policy Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMotorPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdateMotorPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessHouseholdPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessHouseholdPolicies() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lblProgress.Visible = True
            lblProgress.Text = "Processing Household Policies"

            m_lReturn = GetHouseholdPolicies(vArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lblProgress.Visible = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then
                proProgress.Visible = True
                proProgress.Minimum = 0

                proProgress.Maximum = vArray.GetUpperBound(1) + 1


                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    proProgress.Value = lTemp


                    m_lReturn = WriteHouseholdPolicies(vArray:=vArray, lTemp:=lTemp)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If

                Next lTemp

            End If


            m_lReturn = UpdateHouseholdPolicies(vArray:=vArray)

            lblProgress.Visible = False
            proProgress.Visible = False

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessHouseholdPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ProcessHouseholdPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetHouseholdPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetHouseholdPolicies(ByRef vArray(,) As Object) As Integer 

        Dim result As Integer = 0 
        Dim sSQL As String = "" 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT HP.ID, HP.[Client Code], HP.[Policy Number], HP.[Status Id], HP.[Product Type], HP.[Branch Id], " & _
                   "HP.[Lead Agent ID], HP.[Type of Client], HP.[Analysis Code Id], HP.[Business Type Id], HP.Regarding, HP.[Cover from], " & _
                   "HP.[Cover to], HP.[Original Inception], HP.Renewal, HP.[Proposal Date], HP.Issued, HP.[Old Policy Number], " & _
                   "HP.[Co-insurance], HP.[Refer at renewal], HP.[Risk Address 1], HP.[Risk Address 2], HP.[Risk Address 3], HP.[Risk Address 4], " & _
                   "HP.[Island ID], HP.[Accumulation Code Id], HP.[Flooding Area Id], HP.[Water Front], HP.[Other Perils],HP.[Catastrophe Cover], " & _
                   "HP.[Construction Type Id], HP.[Index Linking Id], " & _
                   "HP.[Building Desc1], HP.[Building Ref1], HP.[Building Sum1], HP.[Buidling Date1], " & _
                   "HP.[Building Desc2], HP.[Building Ref2], HP.[Building Sum2], HP.[Buidling Date2], " & _
                   "HP.[Building Desc3], HP.[Building Ref3], HP.[Building Sum3], HP.[Buidling Date3], " & _
                   "HP.[Building Desc4], HP.[Building Ref4], HP.[Building Sum4], HP.[Buidling Date4], " & _
                   "HP.[Building Desc5], HP.[Building Ref5], HP.[Building Sum5], HP.[Buidling Date5], " & _
                   "HP.[Waterside Sum], HP.[Owner Liability], HP.[Alternative Acc], " & _
                   "HP.[Loss Payee Name], HP.[Loss Payee Addr1], HP.[Loss Payee Addr2], HP.[Loss Payee Addr3], " & _
                   "HP.[Loss Payee Addr4], HP.[Household Deductible Id], HP.[Contents Sum Insured], HP.[Satellite Sum Insured], " & _
                   "HP.[Money Id], HP.[Accidental Damage], HP.[Full Theft], HP.[Temp Contents Removal], " & _
                   "HP.[Pedal Cycle Sum], HP.[Employers Liability Sum], "

            sSQL = sSQL & "HP.[Contents Deductible Id], HP.[Personal Unspecified Sum]," & _
                   "HP.[Possession Description1], HP.[Possession Reference1], HP.[Possession Sum Insured1], HP.[Dated Added1], " & _
                   "HP.[Possession Description2], HP.[Possession Reference2], HP.[Possession Sum Insured2], HP.[Dated Added2], " & _
                   "HP.[Possession Description3], HP.[Possession Reference3], HP.[Possession Sum Insured3], HP.[Dated Added3], " & _
                   "HP.[Possession Description4], HP.[Possession Reference4], HP.[Possession Sum Insured4], HP.[Dated Added4], " & _
                   "HP.[Possession Description5], HP.[Possession Reference5], HP.[Possession Sum Insured5], HP.[Dated Added5], " & _
                   "HP.[Possession Description6], HP.[Possession Reference6], HP.[Possession Sum Insured6], HP.[Dated Added6], " & _
                   "HP.[Possession Description7], HP.[Possession Reference7], HP.[Possession Sum Insured7], HP.[Dated Added7], " & _
                   "HP.[Single Item Limit], HP.[Possession Description8], HP.[Possession Reference8], HP.[Possession Sum Insured8], HP.[Dated Added8], " & _
                   "HP.[Possession Description9] , HP.[Possession Reference9], HP.[Possession Sum Insured9],HP.[Dated Added9], " & _
                   "HP.[High Risk Description1], HP.[High Risk Reference1], HP.[High Risk Sum1], HP.[High Risk Date1], HP.[High Risk Valuation1], HP.[Valuation Date1], " & _
                   "HP.[High Risk Description2], HP.[High Risk Reference2], HP.[High Risk Sum2], HP.[High Risk Date2], HP.[High Risk Valuation2], HP.[Valuation Date2], " & _
                   "HP.[High Risk Description3], HP.[High Risk Reference3], HP.[High Risk Sum3], HP.[High Risk Date3], HP.[High Risk Valuation3], HP.[Valuation Date3], "

            sSQL = sSQL & "HP.[High Risk Description4], HP.[High Risk Reference4], HP.[High Risk Sum4], HP.[High Risk Date4], HP.[High Risk Valuation4], HP.[Valuation Date4], " & _
                   "HP.[High Risk Description5], HP.[High Risk Reference5], HP.[High Risk Sum5], HP.[High Risk Date5], HP.[High Risk Valuation5], HP.[Valuation Date5], " & _
                   "HP.[High Risk Description6], HP.[High Risk Reference6], HP.[High Risk Sum6], HP.[High Risk Date6], HP.[High Risk Valuation6], HP.[Valuation Date6], " & _
                   "HP.[High Risk Description7], HP.[High Risk Reference7], HP.[High Risk Sum7], HP.[High Risk Date7], HP.[High Risk Valuation7], HP.[Valuation Date7], " & _
                   "HP.[High Risk Description8], HP.[High Risk Reference8], HP.[High Risk Sum8], HP.[High Risk Date8], HP.[High Risk Valuation8], HP.[Valuation Date8], " & _
                   "HP.[High Risk Description9], HP.[High Risk Reference9], HP.[High Risk Sum9], HP.[High Risk Date9], HP.[High Risk Valuation9], HP.[Valuation Date9], " & _
                   "HP.[Insurance File Count] FROM [Household Policy] HP"

            m_oAccessDatabase.Parameters.Clear()

            m_lReturn = m_oAccessDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Household Policy Details", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHouseholdPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetHouseholdPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteHouseholdPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function WriteHouseholdPolicies(ByRef vArray(,) As Object, ByRef lTemp As Integer) As Integer

        Dim result As Integer = 0
        Dim bDuff As Boolean
        Dim sPolicyRef, sResolvedName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If


            If (CStr(vArray(m_cHouseHoldInsuranceFileCount, lTemp)) = "") Or (CStr(vArray(m_cHouseHoldInsuranceFileCount, lTemp)) = "0") Then
                'New one

                m_oInsuranceFile.InsuranceFolderCnt = 0

                m_oInsuranceFile.InsuranceFileCnt = 0

                m_oInsuranceFile.InsuranceFolderCode = ""
            Else
                'Old one


                m_oInsuranceFile.InsuranceFileCnt = vArray(m_cHouseHoldInsuranceFileCount, lTemp)

                m_lReturn = m_oInsuranceFile.GetDetails

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            m_lReturn = GetHouseholdIds(vHouseholdArray:=vArray, lTemp:=lTemp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bDuff = False


            If CStr(vArray(m_cHouseHoldCoverFrom, lTemp)) = "" Then


                m_oInsuranceFile.CoverStartDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldCoverFrom, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.CoverStartDate = vArray(m_cHouseHoldCoverFrom, lTemp)
            End If


            If CStr(vArray(m_cHouseHoldCoverTo, lTemp)) = "" Then


                m_oInsuranceFile.ExpiryDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldCoverTo, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.ExpiryDate = vArray(m_cHouseHoldCoverTo, lTemp)
            End If


            If CStr(vArray(m_cHouseHoldRenewal, lTemp)) = "" Then


                m_oInsuranceFile.RenewalDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldRenewal, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.RenewalDate = vArray(m_cHouseHoldRenewal, lTemp)
            End If


            If CStr(vArray(m_cHouseHoldOriginalInception, lTemp)) = "" Then


                m_oInsuranceFile.InceptionDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldOriginalInception, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.InceptionDate = vArray(m_cHouseHoldOriginalInception, lTemp)
            End If


            If CStr(vArray(m_cHouseHoldProposalDate, lTemp)) = "" Then


                m_oInsuranceFile.ProposalDate = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldProposalDate, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.ProposalDate = vArray(m_cHouseHoldProposalDate, lTemp)
            End If


            If CStr(vArray(m_cHouseHoldIssued, lTemp)) = "" Then


                m_oInsuranceFile.DateIssued = Nothing
            Else

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldIssued, lTemp)) < #1/1/1900#)


                m_oInsuranceFile.DateIssued = vArray(m_cHouseHoldIssued, lTemp)
            End If


            If CStr(vArray(m_cHouseHoldBuildingDate1, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldBuildingDate1, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldBuildingDate2, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldBuildingDate2, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldBuildingDate3, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldBuildingDate3, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldBuildingDate4, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldBuildingDate4, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldBuildingDate5, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldBuildingDate5, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded1, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded1, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded2, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded2, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded3, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded3, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded4, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded4, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded5, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded5, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded6, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded6, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded7, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded7, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded8, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded8, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldDateAdded9, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldDateAdded9, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate1, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate1, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate2, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate2, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate3, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate3, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate4, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate4, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate5, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate5, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate6, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate6, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate7, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate7, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate8, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate8, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskDate9, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskDate9, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate1, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate1, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate2, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate2, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate3, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate3, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate4, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate4, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate5, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate5, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate6, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate6, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate7, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate7, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate8, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate8, lTemp)) < #1/1/1900#)
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuationDate9, lTemp)) <> "" Then

                bDuff = bDuff Or (CDate(vArray(m_cHouseHoldHighRiskValuationDate9, lTemp)) < #1/1/1900#)
            End If

            If bDuff Then

                MessageBox.Show("Policy " & CStr(vArray(m_cHouseHoldOldPolicyNumber, lTemp)) & " has invalid date", "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'check for the boolean values

            If CStr(vArray(m_cHouseHoldCoInsurance, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldCoInsurance, lTemp) = 1
            Else

                vArray(m_cHouseHoldCoInsurance, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldWaterFront, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldWaterFront, lTemp) = 1
            Else

                vArray(m_cHouseHoldWaterFront, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldOtherPerils, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldOtherPerils, lTemp) = 1
            Else

                vArray(m_cHouseHoldOtherPerils, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldCatastropheCover, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldCatastropheCover, lTemp) = 1
                m_lProductID = 5004
            Else

                vArray(m_cHouseHoldCatastropheCover, lTemp) = 0
                m_lProductID = 5035
            End If


            If CStr(vArray(m_cHouseHoldAlternativeAcc, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldAlternativeAcc, lTemp) = 1
            Else

                vArray(m_cHouseHoldAlternativeAcc, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldOwnerLiability, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldOwnerLiability, lTemp) = 1
            Else

                vArray(m_cHouseHoldOwnerLiability, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldAccidentalDamage, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldAccidentalDamage, lTemp) = 1
            Else

                vArray(m_cHouseHoldAccidentalDamage, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldFullTheft, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldFullTheft, lTemp) = 1
            Else

                vArray(m_cHouseHoldFullTheft, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldTempContentsRemoval, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldTempContentsRemoval, lTemp) = 1
            Else

                vArray(m_cHouseHoldTempContentsRemoval, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation1, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation1, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation1, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation2, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation2, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation2, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation3, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation3, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation3, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation4, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation4, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation4, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation5, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation5, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation5, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation6, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation6, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation6, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation7, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation7, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation7, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation8, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation8, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation8, lTemp) = 0
            End If


            If CStr(vArray(m_cHouseHoldHighRiskValuation9, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldHighRiskValuation9, lTemp) = 1
            Else

                vArray(m_cHouseHoldHighRiskValuation9, lTemp) = 0
            End If


            m_oInsuranceFile.InsuranceFileStructure = "PMB"

            m_oInsuranceFile.PolicyTypeId = 5


            m_oInsuranceFile.AnnualPremium = 0

            m_oInsuranceFile.ThisPremium = 0

            m_oInsuranceFile.NetPremium = 0


            m_lReturn = GetResolvedName(lPartyCnt:=CInt(vArray(m_cHouseHoldClientCode, lTemp)), sResolvedName:=sResolvedName)


            m_oInsuranceFile.InsuredName = sResolvedName


            m_oInsuranceFile.ProductID = m_lProductID



            m_oInsuranceFile.AnalysisCodeId = vArray(m_cHouseHoldAnalysisCode, lTemp)


            m_oInsuranceFile.InsuranceFolderDescription = vArray(m_cHouseHoldRegarding, lTemp)



            m_oInsuranceFile.OldPolicyNumber = vArray(m_cHouseHoldOldPolicyNumber, lTemp)


            If CStr(vArray(m_cHouseHoldReferAtRenewal, lTemp)).ToUpper() = "TRUE" Then

                vArray(m_cHouseHoldReferAtRenewal, lTemp) = 1
            Else

                vArray(m_cHouseHoldReferAtRenewal, lTemp) = 0
            End If



            m_oInsuranceFile.IsReferredAtRenewal = vArray(m_cHouseHoldReferAtRenewal, lTemp)


            If CStr(vArray(m_cHouseHoldBranch, lTemp)) = "0" Then
                m_iSourceID = g_iSourceID
            Else

                m_iSourceID = CInt(vArray(m_cHouseHoldBranch, lTemp))
            End If


            m_oInsuranceFile.SourceID = m_iSourceID


            If CStr(vArray(m_cHouseHoldBusinessType, lTemp)) = "" Then

                m_oInsuranceFile.BusinessTypeID = 0
            Else


                m_oInsuranceFile.BusinessTypeID = vArray(m_cHouseHoldBusinessType, lTemp)
            End If


            m_lLeadAgentCnt = CInt(vArray(m_cHouseHoldAgentCode, lTemp))


            If CStr(vArray(m_cHouseHoldPolicyNumber, lTemp)) = "" Then
                m_lReturn = GetPolicyNumber(sPolicyRef:=sPolicyRef)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                vArray(m_cHouseHoldPolicyNumber, lTemp) = sPolicyRef

            End If



            m_oInsuranceFile.InsuranceRef = vArray(m_cHouseHoldPolicyNumber, lTemp)


            m_oInsuranceFile.InsuranceFolderCode = vArray(m_cHouseHoldPolicyNumber, lTemp)

            m_oInsuranceFile.LeadAgentCnt = m_lLeadAgentCnt


            m_oInsuranceFile.InsuranceFileStatusID = Nothing


            m_oInsuranceFile.LeadInsurerCnt = Nothing



            m_oInsuranceFile.InsuranceHolderCnt = CInt(vArray(m_cHouseHoldClientCode, lTemp))


            m_oInsuranceFile.InsuredCnt = CInt(vArray(m_cHouseHoldClientCode, lTemp))

            'Create the policy

            m_lReturn = m_oInsuranceFile.CreatePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error creating Household Policy " & CStr(vArray(m_cHouseHoldPolicyNumber, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            vArray(m_cHouseHoldInsuranceFileCount, lTemp) = m_oInsuranceFile.InsuranceFileCnt


            m_lInsuranceFileCnt = m_oInsuranceFile.InsuranceFileCnt

            m_lInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt

            'Now do the GIS...

            m_lReturn = SetGISHouseholdValues(vHouseholdArray:=vArray, lCount:=lTemp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Error setting GIS data for " & CStr(vArray(m_cHouseHoldPolicyNumber, lTemp)), "Data Take On", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteHouseholdPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="WriteHouseholdPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateHouseholdPolicies
    '
    ' Description:
    '
    ' History: 30/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateHouseholdPolicies(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            sSQL = "UPDATE [Household Policy] SET [Insurance File Count] = {cnt}, [Policy Number] = {policy_number} WHERE [ID] = {id}"

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                m_oAccessDatabase.Parameters.Clear()


                If CStr(vArray(m_cHouseHoldPolicyNumber, lTemp)) = "" Then


                    vArray(m_cHouseHoldPolicyNumber, lTemp) = DBNull.Value
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="cnt", vValue:=CStr(vArray(m_cHouseHoldInsuranceFileCount, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="policy_number", vValue:=CStr(vArray(m_cHouseHoldPolicyNumber, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oAccessDatabase.Parameters.Add(sName:="id", vValue:=CStr(vArray(m_cHouseHoldID, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oAccessDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Household Policy Details", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateHouseholdPolicies Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="UpdateHouseholdPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetHouseholdIds
    '
    ' Description:
    '
    ' History: 17/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetHouseholdIds(ByRef vHouseholdArray(,) As Object, ByRef lTemp As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQLString, sSQLName As String
        Dim bStoredProc As Boolean
        Dim lResult As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vArray = Nothing


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldClientCode, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldClientCode, lTemp))) Then

                If CStr(vHouseholdArray(m_vHouseHoldClientCode, lTemp)) = "" Then


                    vHouseholdArray(m_vHouseHoldClientCode, lTemp) = DBNull.Value
                Else
                    'Retrieve Household Client Code
                    '        If (vHouseholdArray(m_cHouseHoldTypeOfClient, lTemp) = "Personal") Then

                    sSQLString = "SELECT [Party Count] FROM [Personal] " & _
                                 "WHERE [Client Code] = '" & CStr(vHouseholdArray(m_cHouseHoldClientCode, lTemp)) & "'"
                    '        Else

                    sSQLString = sSQLString & " UNION SELECT [Party Count] FROM [Corporate] " & _
                                 "WHERE [Client Code] = '" & CStr(vHouseholdArray(m_cHouseHoldClientCode, lTemp)) & "'"
                    '        End If

                    sSQLName = "Retrieve Household client code"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldClientCode, lTemp) = vArray(0, 0)
                    Else


                        vHouseholdArray(m_vHouseHoldClientCode, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldBusinessType, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldBusinessType, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldBusinessType, lTemp)) <> "" Then
                    'Retrieving the Business type

                    sSQLString = "SELECT [business count] FROM [business type] " & _
                                 "WHERE [business type id] = " & CStr(vHouseholdArray(m_cHouseHoldBusinessType, lTemp))

                    sSQLName = "Retrieving Business Type"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldBusinessType, lTemp) = vArray(0, 0)
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldAgentCode, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldAgentCode, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldAgentCode, lTemp)) = "" Then


                    vHouseholdArray(m_cHouseHoldAgentCode, lTemp) = DBNull.Value
                Else
                    'Retrieve Household Analysis code

                    sSQLString = "SELECT [lead count] FROM [lead agent] " & _
                                 "WHERE [lead agent id] = " & CStr(vHouseholdArray(m_cHouseHoldAgentCode, lTemp))

                    sSQLName = "Retrieve Agent"
                    bStoredProc = False


                    vArray = Nothing

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldAgentCode, lTemp) = vArray(0, 0)
                    Else


                        vHouseholdArray(m_cHouseHoldAgentCode, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp)) = "" Then


                    vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp) = DBNull.Value
                Else
                    'Retrieve Household Analysis code

                    sSQLString = "SELECT [analysis count] FROM [analysis codes] " & _
                                 "WHERE [analysis code id] = " & CStr(vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp))

                    sSQLName = "Retrieve Analysis Code"
                    bStoredProc = False


                    vArray = Nothing

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp) = vArray(0, 0)
                    Else


                        vHouseholdArray(m_cHouseHoldAnalysisCode, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldBranch, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldBranch, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldBranch, lTemp)) = "" Then


                    vHouseholdArray(m_cHouseHoldBranch, lTemp) = DBNull.Value
                Else
                    'Retrieve Branch

                    sSQLString = "SELECT [branch count] from branch WHERE [branch id] = " & _
                                 CStr(vHouseholdArray(m_cHouseHoldBranch, lTemp))
                    sSQLName = "Retrieve HouseHold Branch"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldBranch, lTemp) = vArray(0, 0)
                    Else


                        vHouseholdArray(m_cHouseHoldBranch, lTemp) = DBNull.Value
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldIsland, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldIsland, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldIsland, lTemp)) <> "" Then
                    'Retrieve Household Island

                    sSQLString = "SELECT [island count] FROM island WHERE [island id] = " & _
                                 CStr(vHouseholdArray(m_cHouseHoldIsland, lTemp))
                    sSQLName = "Retrieve Household Island"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldIsland, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldIsland, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldAccumulationCode, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldAccumulationCode, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldAccumulationCode, lTemp)) <> "" Then
                    'Retrieve Household Accum Code

                    sSQLString = "SELECT [accumulation count] FROM [accumulation code] " & _
                                 "WHERE [accumulation code id] = " & CStr(vHouseholdArray(m_cHouseHoldAccumulationCode, lTemp))

                    sSQLName = "Retrieve Household Accum Code"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldAccumulationCode, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldAccumulationCode, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldFloodingArea, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldFloodingArea, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldFloodingArea, lTemp)) <> "" Then
                    'Retrieve HouseHold Flooding Area

                    sSQLString = "SELECT [flooding count] FROM [flooding area] " & _
                                 "WHERE [flooding area id] = " & CStr(vHouseholdArray(m_cHouseHoldFloodingArea, lTemp))

                    sSQLName = "Retrieve Household Flooding Area"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldFloodingArea, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldFloodingArea, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldConstructionType, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldConstructionType, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldConstructionType, lTemp)) <> "" Then
                    'Retrieve Construction

                    sSQLString = "SELECT [construction count] FROM [construction type] " & _
                                 "WHERE [construction type id] = " & CStr(vHouseholdArray(m_cHouseHoldConstructionType, lTemp))

                    sSQLName = "Retrieve Household Construction Type"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldConstructionType, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldConstructionType, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldIndexLinking, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldIndexLinking, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldIndexLinking, lTemp)) <> "" Then
                    'Retrieve Household Index Linking

                    sSQLString = "SELECT [index count] FROM [index linking] " & _
                                 "WHERE [index linking id] = " & CStr(vHouseholdArray(m_cHouseHoldIndexLinking, lTemp))

                    sSQLName = "Retrieve Household Index Linking"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldIndexLinking, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldIndexLinking, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldDeductible, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldDeductible, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldDeductible, lTemp)) <> "" Then
                    'Retrieve Deductible

                    sSQLString = "SELECT [household count] FROM [household deductible] WHERE [household deductible id] = " & _
                                 CStr(vHouseholdArray(m_cHouseHoldDeductible, lTemp))

                    sSQLName = "Retrieve Household Deductible"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldDeductible, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldDeductible, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldContentsDeductible, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldContentsDeductible, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldContentsDeductible, lTemp)) <> "" Then
                    'Retrieve Contents Deductible

                    sSQLString = "SELECT [contents count] FROM [contents deductible] " & _
                                 "WHERE [contents deductible id] = " & CStr(vHouseholdArray(m_cHouseHoldContentsDeductible, lTemp))

                    sSQLName = "Retrieve Contents Deductible"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldContentsDeductible, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldContentsDeductible, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldMoney, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldMoney, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldMoney, lTemp)) <> "" Then
                    'Retrieve HouseHold Money

                    sSQLString = "SELECT [money count] FROM [money] WHERE [money id] = " & _
                                 CStr(vHouseholdArray(m_cHouseHoldMoney, lTemp))
                    sSQLName = "Retrieve Household Money"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldMoney, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldMoney, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If


            If Not (Convert.IsDBNull(vHouseholdArray(m_cHouseHoldSingleItemLimit, lTemp)) Or IsNothing(vHouseholdArray(m_cHouseHoldSingleItemLimit, lTemp))) Then


                If CStr(vHouseholdArray(m_cHouseHoldSingleItemLimit, lTemp)) <> "" Then
                    'Retrieve HouseHold Money

                    sSQLString = "SELECT [high count] FROM [high risk] WHERE [high risk id] = " & _
                                 CStr(vHouseholdArray(m_cHouseHoldSingleItemLimit, lTemp))
                    sSQLName = "Retrieve High Risk Limit"
                    bStoredProc = False

                    lResult = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, bStoredProc, , vArray)

                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lResult
                    End If

                    If Information.IsArray(vArray) Then


                        vHouseholdArray(m_cHouseHoldSingleItemLimit, lTemp) = vArray(0, 0)
                    Else

                        vHouseholdArray(m_cHouseHoldSingleItemLimit, lTemp) = ""
                    End If


                    vArray = Nothing
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHouseholdIds Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetHouseholdIds", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetGISHouseholdValues
    '
    ' Description: Updates the GIS and business object.
    '
    ' ***************************************************************** '
    Public Function SetGISHouseholdValues(ByRef vHouseholdArray(,) As Object, ByRef lCount As Integer) As Integer

        Dim result As Integer = 0
        Dim lTemp As Integer
        Dim vPolicyLinkId As Integer
        Dim vOIKeyArray, vChildOIKeyArray As Object
        Dim sOIKey, sParentObjectName, sChildObjectName, sParentOIKey, sChildOIKey, sCoreOIKey, sHouseHoldContentsOIKey, sHouseHoldHighRiskOIKey As String
        Dim vRiskDetails, vRiskTypeDetails As Object
        Dim lPolicyLinkId, lPolicyBinderID As Integer
        Dim sHouseHoldPersonalOIKey, sHouseHoldBuildingsOIKey As String
        Dim vSumInsuredArray, vAddressCnt As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetGISInterfaceObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetBusinessObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                'Tidy up
                m_lReturn = RemoveGISInterfaceObject()

                Return result
            End If

            'Here we're doing the Household

            '    m_lInsuranceFileCnt = vHouseholdArray(m_cHouseHoldInsuranceFileCount, lCount)
            '    m_lProductID = 5004
            m_lRiskTypeID = 2
            m_lScreenID = 2
            m_lRiskID = 0

            m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oBusiness.ProductID = m_lProductID

            m_oBusiness.RiskId = m_lRiskID

            m_oBusiness.RiskTypeId = m_lRiskTypeID

            m_oBusiness.ScreenId = m_lScreenID

            'Get the Risk - if it doesn't exist create it, we need the number for the GIS

            m_lReturn = m_oBusiness.GetRisk(vRiskArray:=vRiskDetails, vRiskTypeArray:=vRiskTypeDetails)

            '    If (m_lRiskID = 0) Then

            m_lRiskID = m_oBusiness.RiskId
            '    End If


            vPolicyLinkId = Nothing

            '    m_lReturn = m_oGIS.LoadFromDB(v_sGISDataModelCode:="RSA", _
            'r_vInsuranceFileCnt:=m_lCombinedKey, _
            'r_vPolicyLinkId:=vPolicyLinkId)

            m_lReturn = m_oGIS.LoadFromDB(v_sGisDataModelCode:="RSA", r_vInsuranceFileCnt:=m_lInsuranceFolderCnt, r_vPolicyLinkID:=vPolicyLinkId, r_vRiskID:=m_lRiskID)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    'Create a new one

                    '            m_lReturn = m_oGIS.NewDataSet(v_sGISDataModelCode:="RSA", _
                    'r_lPolicyLinkId:=lPolicyLinkId, _
                    'r_sTopOIKey:=sOIKey, _
                    'v_vInsuranceFileCnt:=m_lCombinedKey)

                    m_lReturn = m_oGIS.NewDataSet(v_sGisDataModelCode:="RSA", r_lPolicyLinkID:=lPolicyLinkId, r_sTopOIKey:=sOIKey, v_vInsuranceFileCnt:=m_lInsuranceFolderCnt, v_vRiskID:=m_lRiskID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else

                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load from DB in GIS", vApp:=ACAPP, vClass:=ACClass, vMethod:="SetGISHouseholdValues")

                    m_lReturn = RemoveGISInterfaceObject()

                    Return result
                End If
            End If


            If Not (Convert.IsDBNull(vPolicyLinkId) Or IsNothing(vPolicyLinkId)) Then
                lPolicyLinkId = vPolicyLinkId
            End If

            ' Get the Top Level OI Key
            m_lReturn = m_oGIS.GetAllOIKey(v_sObjectName:="RSA_policy_binder", r_vOIKeyArray:=vOIKeyArray)

            If Information.IsArray(vOIKeyArray) Then


                sOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))
            End If

            vOIKeyArray = Nothing

            If sOIKey = "" Then
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:="RSA_policy_binder", r_sOIKey:=sOIKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Save it, we need the number
            m_lReturn = m_oGIS.SaveToDB()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lPolicyBinderID = 0 Then

                m_lReturn = m_oBusiness.GetBinder(lPolicyLinkId:=lPolicyLinkId, sDataModel:="RSA", lPolicyBinderID:=lPolicyBinderID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Get the OI key of household contents
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "household_contents"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sHouseHoldContentsOIKey = sChildOIKey

            Else



                sHouseHoldContentsOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If

            'contents sum insured
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="contents_si", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldContentsSumInsured, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'satellite sum insured
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="satellite_si", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldSatelliteSumInsured, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'money_cc
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="money_cc", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldMoney, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'is temp removal
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="temp_removal_contents", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldTempContentsRemoval, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'pedal cycle
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="pedal_cycle", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldPedalCycleSum, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'employers liability
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="employers_liability", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldEmployersLiabilitySum, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'contents deductible
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_contents", v_sPropertyName:="deductible", v_sOIKey:=sHouseHoldContentsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldContentsDeductible, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the OI for Household Personal table
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "household_personal"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sHouseHoldPersonalOIKey = sChildOIKey

            Else



                sHouseHoldPersonalOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If

            'unspecified sum
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_personal", v_sPropertyName:="personal_unspecified", v_sOIKey:=sHouseHoldPersonalOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldPersonalUnspecifiedSum, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Household buildings
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "household_buildings"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sHouseHoldBuildingsOIKey = sChildOIKey

            Else



                sHouseHoldBuildingsOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If

            'waterside sum insured
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_buildings", v_sPropertyName:="waterside_sum_insured", v_sOIKey:=sHouseHoldBuildingsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldWatersideSum, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'is owner liability
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_buildings", v_sPropertyName:="owner_liability", v_sOIKey:=sHouseHoldBuildingsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldOwnerLiability, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'alternative cc
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_buildings", v_sPropertyName:="alternative_accommodation", v_sOIKey:=sHouseHoldBuildingsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldAlternativeAcc, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'deductible
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_buildings", v_sPropertyName:="deductible", v_sOIKey:=sHouseHoldBuildingsOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldDeductible, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ADDING THE RSA CORE DETAILS HERE
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "Core"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCoreOIKey = sChildOIKey

            Else



                sCoreOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If






            m_lReturn = GetAddress(vAddressCnt:=CByte(vAddressCnt), sLine1:=CStr(vHouseholdArray(m_cHouseHoldRiskAddress1, lCount)), sLine2:=CStr(vHouseholdArray(m_cHouseHoldRiskAddress2, lCount)), sLine3:=CStr(vHouseholdArray(m_cHouseHoldRiskAddress3, lCount)), sLine4:=CStr(vHouseholdArray(m_cHouseHoldRiskAddress4, lCount)))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'address cnt
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="address_cnt", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vAddressCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'accidental damage
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="accidental_damage", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldAccidentalDamage, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'full theft
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="full_theft", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldFullTheft, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'island
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="island", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldIsland, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'accumulation code
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="accumulation", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldAccumulationCode, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'flooding area
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="flooding_area", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldFloodingArea, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'waterfront
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="waterfront", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldWaterFront, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'other perils
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="other_perils", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldOtherPerils, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'catastrophe cover
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="catastrophe", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldCatastropheCover, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'construction
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="construction_type", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldConstructionType, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'index linking
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="core", v_sPropertyName:="index_linking", v_sOIKey:=sCoreOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldIndexLinking, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ADDING THE RSA HIGH RISK DETAILS HERE
            sParentObjectName = "RSA_policy_binder"
            sParentOIKey = sOIKey
            sChildObjectName = "Household_high_risk"

            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=sChildObjectName, r_vChildOIKeyArray:=vChildOIKeyArray)

            ' If there aren't any
            If Not Information.IsArray(vChildOIKeyArray) Then

                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sChildObjectName, r_sOIKey:=sChildOIKey, v_sParentOIKey:=sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sHouseHoldHighRiskOIKey = sChildOIKey

            Else



                sHouseHoldHighRiskOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))

            End If

            'high risk
            m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:="household_high_risk", v_sPropertyName:="high_risk_single_limit", v_sOIKey:=sHouseHoldHighRiskOIKey, v_vPropertyValue:=vHouseholdArray(m_cHouseHoldSingleItemLimit, lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now the biggy - update the GIS
            m_lReturn = m_oGIS.SaveToDB()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now let's do the sums insured...
            'Buildings...

            If lPolicyBinderID = 0 Then

                m_lReturn = m_oBusiness.GetBinder(lPolicyLinkId:=lPolicyLinkId, sDataModel:="RSA", lPolicyBinderID:=lPolicyBinderID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ReDim vSumInsuredArray(8, 0)

            lTemp = 0



            vSumInsuredArray(0, lTemp) = DBNull.Value


            vSumInsuredArray(1, lTemp) = DBNull.Value


            vSumInsuredArray(2, lTemp) = DBNull.Value


            vSumInsuredArray(3, lTemp) = DBNull.Value


            vSumInsuredArray(4, lTemp) = DBNull.Value

            vSumInsuredArray(5, lTemp) = 0


            vSumInsuredArray(6, lTemp) = DBNull.Value

            vSumInsuredArray(7, lTemp) = 0

            vSumInsuredArray(8, lTemp) = 0


            If CStr(vHouseholdArray(m_cHouseHoldBuildingDesc1, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDesc1, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldBuildingRef1, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldBuildingSum1, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDate1, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldBuildingDesc2, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDesc2, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldBuildingRef2, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldBuildingSum2, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDate2, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldBuildingDesc3, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDesc3, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldBuildingRef3, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldBuildingSum3, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDate3, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldBuildingDesc4, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDesc4, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldBuildingRef4, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldBuildingSum4, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDate4, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldBuildingDesc5, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDesc5, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldBuildingRef5, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldBuildingSum5, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldBuildingDate5, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            m_lReturn = m_oBusiness.UpdateSumInsured(lPolicyLinkId:=lPolicyLinkId, sDataModel:="RSA", lSumInsuredType:=6, vSumInsuredArray:=vSumInsuredArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Personal Possessions...

            ReDim vSumInsuredArray(8, 0)

            lTemp = 0



            vSumInsuredArray(0, lTemp) = DBNull.Value


            vSumInsuredArray(1, lTemp) = DBNull.Value


            vSumInsuredArray(2, lTemp) = DBNull.Value


            vSumInsuredArray(3, lTemp) = DBNull.Value


            vSumInsuredArray(4, lTemp) = DBNull.Value

            vSumInsuredArray(5, lTemp) = 0


            vSumInsuredArray(6, lTemp) = DBNull.Value

            vSumInsuredArray(7, lTemp) = 0

            vSumInsuredArray(8, lTemp) = 0


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription1, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription1, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference1, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded1, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription2, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription2, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference2, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured2, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded2, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription3, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription3, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference3, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured3, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded3, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription4, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription4, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference4, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured4, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded4, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription5, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription5, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference5, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured5, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded5, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription6, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription6, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference6, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured6, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded6, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription7, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription7, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference7, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured7, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded7, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription8, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription8, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference8, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured8, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded8, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldPossessionDescription9, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldPossessionDescription9, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldPossessionReference9, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldPossessionSumInsured9, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldDateAdded9, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value

                vSumInsuredArray(5, lTemp) = 0


                vSumInsuredArray(6, lTemp) = DBNull.Value


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            m_lReturn = m_oBusiness.UpdateSumInsured(lPolicyLinkId:=lPolicyLinkId, sDataModel:="RSA", lSumInsuredType:=4, vSumInsuredArray:=vSumInsuredArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'High Risk...

            ReDim vSumInsuredArray(8, 0)

            lTemp = 0



            vSumInsuredArray(0, lTemp) = DBNull.Value


            vSumInsuredArray(1, lTemp) = DBNull.Value


            vSumInsuredArray(2, lTemp) = DBNull.Value


            vSumInsuredArray(3, lTemp) = DBNull.Value


            vSumInsuredArray(4, lTemp) = DBNull.Value

            vSumInsuredArray(5, lTemp) = 0


            vSumInsuredArray(6, lTemp) = DBNull.Value

            vSumInsuredArray(7, lTemp) = 0

            vSumInsuredArray(8, lTemp) = 0


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription1, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription1, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference1, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum1, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate1, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation1, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate1, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate1, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription2, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription2, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference2, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum2, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate2, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation2, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate2, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate2, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription3, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription3, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference3, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum3, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate3, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation3, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate3, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate3, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription4, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription4, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference4, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum4, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate4, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation4, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate4, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate4, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription5, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription5, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference5, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum5, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate5, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation5, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate5, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate5, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription6, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription6, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference6, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum6, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate6, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation6, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate6, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate6, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription7, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription7, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference7, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum7, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate7, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation7, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate7, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate7, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription8, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription8, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference8, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum8, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate8, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation8, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate8, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate8, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            If CStr(vHouseholdArray(m_cHouseHoldHighRiskDescription9, lCount)) <> "" Then
                lTemp += 1
                ReDim Preserve vSumInsuredArray(8, lTemp)


                vSumInsuredArray(0, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDescription9, lCount)


                vSumInsuredArray(1, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskReference9, lCount)


                vSumInsuredArray(2, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskSum9, lCount)


                vSumInsuredArray(3, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskDate9, lCount)


                vSumInsuredArray(4, lTemp) = DBNull.Value


                vSumInsuredArray(5, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuation9, lCount)

                If CStr(vHouseholdArray(m_cHouseHoldHighRiskValuationDate9, lCount)) = "" Then


                    vSumInsuredArray(6, lTemp) = DBNull.Value
                Else


                    vSumInsuredArray(6, lTemp) = vHouseholdArray(m_cHouseHoldHighRiskValuationDate9, lCount)
                End If


                vSumInsuredArray(7, lTemp) = DBNull.Value


                vSumInsuredArray(8, lTemp) = DBNull.Value
            End If


            m_lReturn = m_oBusiness.UpdateSumInsured(lPolicyLinkId:=lPolicyLinkId, sDataModel:="RSA", lSumInsuredType:=5, vSumInsuredArray:=vSumInsuredArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = RemoveGISInterfaceObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                '        Exit Function
            End If

            m_lReturn = RemoveBusinessObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACAPP, vClass:=ACClass, vMethod:="SetGISHouseholdValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAddress
    '
    ' Description:
    '
    ' History: 09/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetAddress(ByRef vAddressCnt As Byte, ByRef sLine1 As String, ByRef sLine2 As String, ByRef sLine3 As String, ByRef sLine4 As String) As Integer 

        Dim result As Integer = 0 
        Dim sSQL As String = "" 
        Dim vResultArray(,) As Object 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vAddressCnt = Nothing

            If sLine1 = "" Then
                Return result
            End If

            sSQL = "Select address_cnt from address" & Strings.Chr(13) & Strings.Chr(10) & _
                   "where address1 = {address1}" & Strings.Chr(13) & Strings.Chr(10) & _
                   "and address2 = {address2}" & Strings.Chr(13) & Strings.Chr(10) & _
                   "and address3 = {address3}" & Strings.Chr(13) & Strings.Chr(10) & _
                   "and address4 = {address4}" & Strings.Chr(13) & Strings.Chr(10)

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="address1", vValue:=sLine1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="address2", vValue:=sLine2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="address3", vValue:=sLine3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="address4", vValue:=sLine4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAddress", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then

                vAddressCnt = CByte(vResultArray(0, 0))
                Return result
            End If

            If m_oAddressObject Is Nothing Then
                Dim temp_m_oAddressObject As Object 
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oAddressObject, "bSIRAddress.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oAddressObject = temp_m_oAddressObject

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            vAddressCnt = 0


            m_lReturn = m_oAddressObject.DirectAdd(vAddressCnt:=vAddressCnt, vAddress1:=sLine1, vAddress2:=sLine2, vAddress3:=sLine3, vAddress4:=sLine4)

            '    m_lReturn = m_oAddressObject.DirectAdd(vAddress1:=sLine1, vAddress2:=sLine2, vAddress3:=sLine3, vAddress4:=sLine4)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            vAddressCnt = m_oAddressObject.AddressCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddress Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetGISInterfaceObject
    '
    ' Description:
    '
    ' History: 01/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetGISInterfaceObject() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oGIS Is Nothing) Then
                Return result
            End If

            '    Set m_oGIS = New iGIS.Application
            m_oGIS = New iGIS.Application()

            m_lReturn = CType(m_oGIS, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the GIS interface object", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetGISInterfaceObject")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business keys.

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISInterfaceObject Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetGISInterfaceObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RemoveGISInterfaceObject
    '
    ' Description:
    '
    ' History: 01/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function RemoveGISInterfaceObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oGIS Is Nothing) Then
                ' Terminate the GIS object
                m_oGIS.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oGIS = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveGISInterfaceObject Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="RemoveGISInterfaceObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBusinessObject
    '
    ' Description:
    '
    ' History: 01/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetBusinessObject() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oBusiness Is Nothing) Then
                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskScreen.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.
                '        sTitle = iPMFunc.GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFailTitle, _
                'iDataType:=PMResString)

                '        sMessage = iPMFunc.GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFail, _
                'iDataType:=PMResString)

                ' Display message.
                '        MsgBox sMessage, vbCritical, sTitle
                '        SetMousePointer PMMouseNormal
                Return m_lReturn
            End If

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACAPP, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusinessObject Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RemoveBusinessObject
    '
    ' Description:
    '
    ' History: 01/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function RemoveBusinessObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oBusiness Is Nothing) Then
                ' Terminate the business object

                m_oBusiness.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveBusinessObject Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="RemoveBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Sub frmDataAccess_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        oAccess = New clsAccessFunctions()
    End Sub

    Private Sub frmDataAccess_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If Not (oAccess Is Nothing) Then
            m_lReturn = oAccess.CloseAccessDatabase()

            m_lReturn = oAccess.CloseDatabase()

            m_lReturn = oAccess.CloseArchitectureDatabase()

        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddIslandLookup
    '
    ' Description:
    '
    ' History: 09/10/2000 MSB - Created.
    '
    ' ***************************************************************** '

    'UPGRADE_NOTE: (7001) The following declaration (AddIslandLookup) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddIslandLookup() As Integer
    '
    'Dim result As Integer = 0
    'Dim sSQLString, sSQLName As String
    'Dim vIslandArray(,) As Object
    'Dim bIfExists As Boolean
    'Dim lCaption As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Add the island details to area if it already doesn't exist
    'sSQLString = "SELECT [island id], [island], [island count] FROM [island]"
    'sSQLName = "Retrieve Island Details"
    '
    'm_lReturn = m_oAccessDatabase.SQLSelect(sSQLString, sSQLName, False,  , vIslandArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '

    'For 'iLookupCount As Integer = vIslandArray.GetLowerBound(1) To vIslandArray.GetUpperBound(1)
    '

    'If CStr(vIslandArray(2, iLookupCount)) <> "" Then

    'If CDbl(vIslandArray(2, iLookupCount)) > 0 Then
    'this means that entry already exists. update
    'bIfExists = True
    'Else
    'bIfExists = False
    'End If
    'Else
    'bIfExists = False
    'End If
    '

    'm_lReturn = GetCaptionID(v_sCaption:=CStr(vIslandArray(1, iLookupCount)), r_lCaptionID:=lCaption)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'If Not bIfExists Then
    'create new entry
    '
    'sSQLString = "INSERT INTO area('caption_id', 'code', 'description', 'is_deleted', 'effective_date') " & Strings.Chr(13) & Strings.Chr(10)

    'sSQLString = sSQLString & "VALUES ( " & CStr(lCaption) & ", '" & CStr(vIslandArray(1, iLookupCount)) & "', '"

    'sSQLString = sSQLString & CStr(vIslandArray(1, iLookupCount)) & "', " & CStr(0) & "," & DateTimeHelper.ToString(DateTime.Now) & ")"
    '
    'sSQLName = "Insert Values Into Area"
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQLString, sSQLName, False)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '

    'sSQLString = "SELECT area_id FROM area WHERE code = '" &  _
    '             CStr(vIslandArray(1, iLookupCount)) & "'"
    'sSQLName = "Retrieve Area ID"
    'm_lReturn = m_oDatabase.SQLSelect(sSQLString, sSQLName, False,  , m_vResultArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'If Information.IsArray(m_vResultArray) Then


    'vIslandArray(2, iLookupCount) = m_vResultArray(0, 0)
    'End If
    '
    'Else
    'already exists. update



    'sSQLString = "UPDATE area " & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "SET caption_id = " & CStr(lCaption) & ", " & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "code = '" & CStr(vIslandArray(1, iLookupCount)) & "', " & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "description = '" & CStr(vIslandArray(1, iLookupCount)) & "', " & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "is_deleted = 0" & ", " & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "effective_date = '" & DateTimeHelper.ToString(DateTime.Now) & "'" & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "WHERE area_id = " & CStr(vIslandArray(2, iLookupCount))
    '
    'sSQLName = "Update Area"
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQLString, sSQLName, False)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'End If
    '
    'Populate the island count with area id
    '


    'sSQLString = "UPDATE [island] " & Strings.Chr(13) & Strings.Chr(10) &  _
    '             "SET [island count] = " & CStr(vIslandArray(2, iLookupCount)) & Strings.Chr(13) & Strings.Chr(10) &  _
    '             " WHERE [island] = '" & CStr(vIslandArray(1, iLookupCount)) & "'"
    '
    'sSQLName = "Set island count"
    '
    'm_lReturn = m_oAccessDatabase.SQLAction(sSQLString, sSQLName, False)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'End If
    '
    'Next iLookupCount
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddIslandLookup Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="AddIslandLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetCaptionID
    '
    ' Description: Calls the spu_pm_caption_id_return stored procedure
    '              to either get or create a caption_id
    '
    ' ***************************************************************** '
    Private Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionID As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' g_iLanguageID

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oArchitectureDatabase Is Nothing Then
                m_lReturn = oAccess.OpenSiriusArchDatabase()
            End If

            ' Clear the parameters
            m_oArchitectureDatabase.Parameters.Clear()

            ' Add the parameters
            m_lReturn = m_oArchitectureDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(g_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oArchitectureDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oArchitectureDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oArchitectureDatabase.SQLAction(sSQL:=ACSQLCaptionReturn, sSQLName:=ACSQLCaptionReturnName, bStoredProcedure:=ACSQLCaptionReturnStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the returned caption_id
            r_lCaptionID = m_oArchitectureDatabase.Parameters.Item("caption_id").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyNumber
    '
    ' Description:
    '
    ' History: 20/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetPolicyNumber(ByRef sPolicyRef As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPolicyNumber Is Nothing Then
                Dim temp_m_oPolicyNumber As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_m_oPolicyNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPolicyNumber = temp_m_oPolicyNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oPolicyNumber.GeneratePolicyNumber(v_lBusinessType:=1, v_iBranch:=m_iSourceID, v_lProductID:=m_lProductID, v_lAgent:=m_lLeadAgentCnt, r_sGeneratedPolicyNumber:=sPolicyRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyNumber Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="GetPolicyNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
