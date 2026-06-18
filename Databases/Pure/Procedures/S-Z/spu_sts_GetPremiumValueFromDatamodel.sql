ddldropprocedure spu_sts_GetPremiumValueFromDatamodel
go

create procedure spu_sts_GetPremiumValueFromDatamodel(@RiskCnt int, @Premium_minus_Ipt numeric(19,4) OUTPUT)
as
declare @GDMCODE varchar(10)
declare @SQL nvarchar(1024)
declare @ParamDefinition nvarchar(255)

select @GDMCODE = rtrim(Max(gdm.code)) from 
	risk 
	join insurance_file_risk_link ifrl on ifrl.risk_cnt = risk.risk_cnt 
	join gis_policy_link gpl on gpl.insurance_file_cnt = ifrl.insurance_file_cnt
	join gis_data_model gdm on gdm.gis_data_model_id = gpl.gis_data_model_id
where 
	risk.risk_cnt=1305

select @sql = N'select @Premium_Minus_Ipt=max(premium_minus_ipt) from 
	risk 
	join insurance_file_risk_link ifrl on ifrl.risk_cnt = risk.risk_cnt 
	join gis_policy_link gpl on gpl.insurance_file_cnt = ifrl.insurance_file_cnt
	join ' + @GDMCODE + '_policy_binder PB on pb.gis_policy_link_id = gpl.gis_policy_link_id
	join ' + @GDMCODE + '_output OT on ot.' + @GDMCODE + '_policy_binder_id = PB.' + @GDMCODE + '_policy_binder_id
	where 
	risk.risk_cnt=@RiskCnt'

Set @ParamDefinition = N'@RiskCnt int,@Premium_Minus_Ipt Numeric(19,4) OUTPUT'

execute sp_executesql
	@Sql,
	@ParamDefinition,
	@RiskCnt,
	@Premium_Minus_Ipt OUTPUT