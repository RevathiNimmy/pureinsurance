SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_GetPartySchemeForScreenidDatamodelcodeRiskCode'
GO

create procedure spu_SAM_GetPartySchemeForScreenidDatamodelcodeRiskCode
	@Gis_screen_id int,
	@Risk_Code varchar(10)
as

declare @GISDataModel_Id int
select  @GISDataModel_Id = gis_data_model_id
from 	gis_screen
where   gis_screen_id = @Gis_screen_id

select 
	DISTINCT MAX(party.party_cnt) party_cnt,
	max(gis_scheme.gis_scheme_id) gis_scheme_id,
	max(risk_group.risk_group_id) risk_group_id,
	max(risk_group.code) RiskGroupCode,
	max(gdm.code)
from 
	risk_code
	INNER JOIN risk_group on risk_code.risk_group_id = risk_group.risk_Group_id
	INNER JOIN gis_qem_usage gqu ON risk_group.risk_group_id = gqu.risk_group_id
	INNER JOIN gis_scheme ON gqu.gis_scheme_id = gis_scheme.gis_scheme_id
	INNER JOIN gis_insurer ON gis_insurer.gis_insurer_id = gis_scheme.gis_insurer_id
	INNER JOIN party ON gis_insurer.abi_81_insurer = party.abi_code_on_81
	INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = gqu.gis_data_model_id
where 
	risk_group.gis_screen_id=@Gis_screen_id and
	gdm.gis_data_model_id = @GISDataModel_Id and
	risk_code.code = @Risk_Code

GO
