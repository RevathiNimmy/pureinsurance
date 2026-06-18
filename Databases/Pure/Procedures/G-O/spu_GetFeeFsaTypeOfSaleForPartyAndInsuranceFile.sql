SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetFeeFsaTypeOfSaleForPartyAndInsuranceFile'
GO


CREATE PROCEDURE spu_GetFeeFsaTypeOfSaleForPartyAndInsuranceFile
	@fee_party_cnt int,
	@insurance_file_cnt int
AS

select fee_amounts.fsa_type_of_sale_id from insurance_file 
join risk_code on insurance_file.risk_Code_id = risk_code.risk_code_id
join risk_group on risk_code.risk_group_id = risk_group.risk_group_id
join fee_amounts on fee_amounts.risk_group_id = risk_group.risk_group_id
where party_cnt=@fee_party_cnt
and insurance_file.insurance_file_cnt = @insurance_file_cnt
