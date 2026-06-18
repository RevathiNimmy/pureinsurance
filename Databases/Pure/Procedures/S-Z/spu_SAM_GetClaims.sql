
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_SAM_GetClaims'
GO

/*******************************************************************************************************/
/* spu_SAM_GetClaims                                                                                   */
/* Get Claims for specified party                                                                      */
/* MKW27/03/06                                                                                         */
/*******************************************************************************************************/
CREATE PROCEDURE spu_SAM_GetClaims
    @party_cnt int = NULL
AS
BEGIN
select 
	claim.claim_id 'ClaimKey',
	claim.claim_number 'ClaimNumber' ,
	claim.policy_number 'PolicyNumber',
        ClaimStatus  = case claim.claim_status_id
		when 1 then 'Provisional Open' 
		when 2 then 'Live Open'
		when 3 then 'Closed'
		when 4 then 'ReOpened'
		when 5 then 'ReClosed'
		else ''
	end,
	claim.description as 'Regarding',
	handler.description as 'Handler',
	primary_cause.description as 'PrimaryCause',
	claim.loss_from_date as 'LossDate',
	claim.reported_date as 'ReportedDate',
	claim.insurer_name as 'Insurer',
	secondary_cause.description as 'SecondaryCause',
	town.description as 'Town',
	claim.location,
	insurance_file_type.description as 'PolicyType'
from 
	claim 
	join handler on claim.handler_id=handler.handler_id
	join primary_cause on claim.primary_cause_id = primary_cause.primary_cause_id
	join insurance_file on claim.policy_id = insurance_file.insurance_file_cnt
	join insurance_file_type on insurance_file.insurance_file_type_id=insurance_file_type.insurance_file_type_id
	LEFT OUTER JOIN secondary_cause on claim.secondary_cause_id = secondary_cause.secondary_cause_id
	LEFT OUTER JOIN town on claim.town = town.town_id
	

where
	claim.client_id=@party_cnt
end

GO
