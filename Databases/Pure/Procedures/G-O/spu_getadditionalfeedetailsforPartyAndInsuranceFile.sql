ddldropprocedure spu_getadditionalfeedetailsforPartyAndInsuranceFile
go

CREATE PROCEDURE spu_getadditionalfeedetailsforPartyAndInsuranceFile
	@fee_party_cnt int,
	@insurance_file_cnt int
AS

--Party_Type.Code
--Tax_Group
declare @PartyTypeCode varchar(10)
declare @taxgroup_id int
declare @risk_code_id int

select @risk_code_id = risk_Code_id from insurance_file where insurance_file_cnt = @insurance_file_cnt

select @PartyTypeCode = code from party_type
	join party on party.party_type_id = party_type.party_type_id
	where party.party_cnt = @fee_party_cnt

exec spu_txn_getTaxRateForRiskExtra
	@RiskCodeId = @risk_code_id,
	@PartyCnt = @fee_party_cnt,
	@taxgroupid = @taxgroup_id output
 
select 	@taxgroup_id 'tax_group_id',
	@PartyTypeCode 'Code'