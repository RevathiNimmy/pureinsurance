--For Earnings Report
DDLDropview 'dashAllPolicies_Earning'
go
Create view [dbo].[dashAllPolicies_Earning] as
select distinct 
	insurance_file.insurance_ref,
	insurance_file.insurance_file_cnt,
	insurance_file.insurance_folder_cnt,
	risk.risk_cnt,

	risk.description as Risk,
	ri_model.description as RIModel,
	ri_band.description as RIBand, 
	
	ri_arrangement.premium as RIPremium, 	
	ri_arrangement_line.commission_percent as RICommPercent,
	ri_arrangement_line.commission_value as RIComm

from 	
	insurance_file,
	insurance_file_risk_link,
	risk, 
	ri_arrangement,
	ri_model, 
	risk_type_ri_model_usage,
	ri_band, 
	ri_arrangement_line
where 
	--insurance_file.product_id = 36 and
	insurance_file.insurance_file_type_id in (2, 5) and --<> 1 exclude Quotation
	insurance_file.insurance_file_cnt=insurance_file_risk_link.insurance_file_cnt and
	
	insurance_file_risk_link.risk_cnt=risk.risk_cnt and

	risk.risk_cnt=ri_arrangement.risk_cnt and
	--ri_arrangement.original_flag=0 and 
	ri_arrangement.ri_model_id=ri_model.ri_model_id and 
	ri_arrangement.ri_band_id=risk_type_ri_model_usage.ri_band and
	ri_arrangement.ri_band_id=ri_band.ri_band_id and

	ri_model.is_deleted=0 and
	ri_model.ri_model_id=risk_type_ri_model_usage.ri_model_id and 
	
	ri_arrangement_line.ri_arrangement_id = ri_arrangement.ri_arrangement_id 
	