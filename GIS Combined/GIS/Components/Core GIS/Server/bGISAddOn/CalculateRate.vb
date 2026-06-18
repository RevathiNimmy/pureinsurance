Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("CalculateRate_NET.CalculateRate")> _
Public NotInheritable Class CalculateRate 
	
	' ************************************************
	' Added to replace global variables 19/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	Private Const ACClass As String = "CalculateRate"
	
	Private Const ACGetAddOnSQL As String = "{call spu_GIS_Add_On_sel(?,?,?,?,?)}"
	Private Const ACGetAddOnName As String = "GetAddOnPremium"
	Private Const ACGetAddOnStored As Boolean = True
	
	Private m_oDatabase As dPMDAO.Database
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Public Function NBCalculateAddOnPremium(ByVal v_sGisDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sAddOnCode As String, ByVal v_sAddOnCoverLevelCode As String, ByVal v_dtEffectiveDate As Date, ByVal v_cInsurancePremium As Decimal, ByRef r_cAddOnPremium As Decimal, ByRef r_cAddOnIPTAmount As Decimal, ByRef r_cAddOnIPTRate As Decimal, ByRef r_cAddOnFee As Decimal, ByRef r_cAddOnRate As Decimal, Optional ByRef r_cAddOnVATAmount As Decimal = 0, Optional ByRef r_cAddOnVATRate As Decimal = 0, Optional ByRef r_cAddOnCommissionAmount As Decimal = 0, Optional ByRef r_cAddOnCommissionRate As Decimal = 0, Optional ByRef r_lAddOnPartyCnt As Integer = 0) As Integer
		
		' ***************************************************************** '
		' Name: NBCalculateAddOnPremium
		'
		' Description: Retrieves and calculates the Add on premium rates given
		'   DataModelID, BusinessTypeID, AddOnCode, AddOnCoverLevelCode and an
		'   Effective Date
		'
		' ***************************************************************** '
		
		Dim result As Integer = 0
		Try 
			
			Dim bNew As Boolean
            Dim vArray(,) As Object
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'RJG 05/06/2000 - Ensure all return values are initially set to 0
			r_cAddOnPremium = 0
			r_cAddOnIPTAmount = 0
			r_cAddOnIPTRate = 0
			r_cAddOnFee = 0
			r_cAddOnRate = 0
			
			If Not False Then
				r_cAddOnVATAmount = 0
			End If
			
			If Not False Then
				r_cAddOnVATRate = 0
			End If
			
			If Not False Then
				r_cAddOnCommissionAmount = 0
			End If
			
			If Not False Then
				r_cAddOnCommissionRate = 0
			End If
			
			'RJG 02/06/00 - Use the Data Model Specifc DSN
			m_lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=v_sGisDataModelCode, r_oDatabase:=m_oDatabase, r_bNew:=bNew), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'RJG 02/06/00 Clear the Parameters collection
			m_oDatabase.Parameters.Clear()
			
			' Data Model ID Input Param
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=v_sBusinessTypeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_code", vValue:=v_sAddOnCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_add_on_cover_level_code", vValue:=v_sAddOnCoverLevelCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the SQL
			m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAddOnSQL, ssqlname:=ACGetAddOnName, bstoredprocedure:=ACGetAddOnStored, vresultarray:=vArray, bkeepnulls:=True, lnumberrecords:=gPMConstants.PMAllRecords)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Information.IsArray(vArray) Then
				'RJG 05/06/2000 - If the Business fee is not supplied then use the Business Rate
                Dim auxVar_3 As Object = vArray(0, 0)


                If Convert.IsDBNull(auxVar_3) Or IsNothing(auxVar_3) Then
                    Dim auxVar As Object = vArray(1, 0)


                    If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then

                        If CDbl(vArray(1, 0)) <> 0 Then

                            r_cAddOnPremium = v_cInsurancePremium / 100 * CDbl(vArray(1, 0))
                        End If

                        r_cAddOnRate = CDec(vArray(1, 0))
                    Else
                        'RJG 05/06/2000 - If both the Add on fee and Rates are Null then return PMFalse
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    'RJG 05/06/2000 - If the Fee and the rate are supplied the use the fee, calculate
                    'the rate and add the two together.
                    Dim auxVar_2 As Object = vArray(1, 0)


                    If Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Then

                        If CDbl(vArray(1, 0)) <> 0 Then


                            r_cAddOnPremium = (v_cInsurancePremium / 100 * CDbl(vArray(1, 0))) + CDbl(vArray(0, 0))
                        Else

                            r_cAddOnPremium = CDec(vArray(0, 0))
                        End If

                        r_cAddOnRate = CDec(vArray(1, 0))
                    Else

                        r_cAddOnPremium = CDec(vArray(0, 0))
                    End If

                    r_cAddOnFee = CDec(vArray(0, 0))
                End If

                'RJG 05/06/2000 - Calculcate the IPT on the Add On


                If CDbl(vArray(2, 0)) > 0 Then
                    Dim auxVar_4 As Object = vArray(2, 0)


                    If Not (Convert.IsDBNull(auxVar_4) Or IsNothing(auxVar_4)) Then

                        r_cAddOnIPTAmount = r_cAddOnPremium / 100 * CDbl(vArray(2, 0))

                        r_cAddOnIPTRate = CDec(vArray(2, 0))
                    End If
                Else
                    'RJG 15/03/2001 - Calculate the VAT for the Add On
                    Dim auxVar_5 As Object = vArray(3, 0)


                    If Not (Convert.IsDBNull(auxVar_5) Or IsNothing(auxVar_5)) Then
                        If Not False Then

                            r_cAddOnVATAmount = r_cAddOnPremium / 100 * CDbl(vArray(3, 0))
                        End If

                        If Not False Then

                            r_cAddOnVATRate = CDec(vArray(3, 0))
                        End If
                    End If
                End If

                'RJG 15/03/2001 - Get the commission amount / Percentage and Party Count
                'If the commission amount is not 0 use that otherwise use the rate to calculate


                Dim auxVar_7 As Object = vArray(4, 0)


                If Not (Convert.IsDBNull(auxVar_7) Or IsNothing(auxVar_7)) And CDbl(vArray(4, 0)) > 0 Then
                    If Not False Then

                        r_cAddOnCommissionAmount = CDec(vArray(4, 0))
                    End If
                Else
                    'Use the rate to calculate the commission

                    Dim auxVar_6 As Object = vArray(5, 0)


                    If Not (Convert.IsDBNull(auxVar_6) Or IsNothing(auxVar_6)) And CDbl(vArray(5, 0)) > 0 Then
                        If Not False Then

                            r_cAddOnCommissionAmount = r_cAddOnPremium / 100 * CDbl(vArray(5, 0))
                        End If

                        If Not False Then

                            r_cAddOnCommissionRate = CDec(vArray(5, 0))
                        End If
                    End If
                End If

                'RJG 15/03/2001 - Pass the PartyCnt back
                If Not False Then
                    Dim auxVar_8 As Object = vArray(6, 0)


                    If Not (Convert.IsDBNull(auxVar_8) Or IsNothing(auxVar_8)) Then

                        r_lAddOnPartyCnt = CInt(vArray(6, 0))
                    End If
                End If
				
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			' Log Error Message
            'developer guide no. 180(Latest guide)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBCalculateAddOnPremiumFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBCalculateAddOnPremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
