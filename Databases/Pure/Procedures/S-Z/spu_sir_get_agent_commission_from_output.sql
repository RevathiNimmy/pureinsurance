SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure spu_sir_get_agent_commission_from_output
GO

CREATE PROCEDURE spu_sir_get_agent_commission_from_output    
    @insurance_file_cnt int,     
	@party_id INT,
	@commission_band_id INT, 
    @commission_rate NUMERIC(19, 10) OUTPUT
AS      
      
 DECLARE @sql nvarchar(max)
 DECLARE @DataModelCode VARCHAR(25)  
  
 SELECT  
 @DataModelCode=LTRIM(RTRIM(gdm.code))
 FROM gis_policy_link gpl  
 INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = gpl.gis_data_model_id  
 INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = gpl.risk_id  
 INNER JOIN insurance_file ifi ON ifi.insurance_file_cnt  = ifrl.insurance_file_cnt  
 WHERE ifi.insurance_file_cnt = @insurance_file_cnt  
  
 Select @SQL =	N'SELECT TOP 1  @commission_rate=OC.Commission_percent_overriden From ' + @DataModelCode + '_Output_Commission OC ' + Char(13) +
				'INNER JOIN Party P On OC.Agent_Party_Code = P.Shortname AND OC.Agent_Party_Code = LTrim(Rtrim(P.shortname)) ' + Char(13) +   
				'LEFT JOIN ' + @DataModelCode + '_Policy_Binder PB ON PB.' + @DataModelCode + '_policy_binder_id = OC.' + @DataModelCode + '_policy_binder_id ' + Char(13) +   
				'LEFT JOIN gis_policy_link gpl ON gpl.gis_policy_link_id = PB.gis_policy_link_id  ' + Char(13) +    
				'LEFT JOIN Commission_Band CB ON CB.code = OC.Commission_band_code   ' + Char(13) +  
				'LEFT JOIN insurance_file_risk_link ifr ON gpl.risk_id = ifr.risk_cnt   ' + Char(13) +  
				'LEFT JOIN risk ON risk.risk_cnt = ifr.risk_cnt   ' + Char(13) +  
				'WHERE ifr.insurance_file_cnt = ' + Convert(Varchar(15),@insurance_file_cnt) +  Char(13) +  
				'AND ISNULL(CB.is_deleted,0) = 0  AND ISNULL(risk.is_risk_selected,0) = 1 ' + Char(13) +  
				'AND CB.Commission_band_id = ' + Convert(Varchar(15),@commission_band_id) + Char(13) +   
				'AND P.party_cnt = ' + Convert(Varchar(15),@party_id)  + Char(13) +  
				'Order by risk.risk_cnt,' +  @DataModelCode + '_Output_Commission_Id desc'

EXEC sp_executesql @SQL, N'@commission_rate NUMERIC(19, 10) output', @commission_rate output


