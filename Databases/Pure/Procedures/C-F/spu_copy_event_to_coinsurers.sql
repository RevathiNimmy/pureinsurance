SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_event_to_coinsurers'
GO


CREATE PROCEDURE spu_copy_event_to_coinsurers
    @event_cnt int,
    @insurance_file_cnt int
AS


BEGIN
DELETE FROM
    policy_coinsurers
WHERE
    insurance_file_cnt = @insurance_file_cnt
INSERT INTO policy_coinsurers (
    insurance_file_cnt,
    party_cnt,
    coinsurer_count,
    coinsurer_percentage,
    coinsurer_value,
    coinsurer_commission_rate,
    coinsurer_commission_amount,
    coinsurer_ipt_amount,
    coinsurer_policy_number,  
    base_currency_id,
    Base_coinsurer_commission_amount,
    Base_coinsurer_value,
    insurance_section_id,
    coinsurer_net_commission,
    coinsurer_commission_tax,
    coinsurer_cover_percentage,
    risk_transfer_agreement,
    signed_line_percentage,
    linestands,
    written_line_percentage,
    signed_line_amount,
    bureau_party_cnt,
    isleadunderwriter
)
select @insurance_file_cnt,
    party_cnt,
    coinsurer_count,
    coinsurer_percentage,
    coinsurer_value,
    coinsurer_commission_rate,
    coinsurer_commission_amount,
    coinsurer_ipt_amount,
    coinsurer_policy_number,  
    base_currency_id,
    Base_coinsurer_commission_amount,
    Base_coinsurer_value,
    (select max(ICS.insurance_section_id) from 
		Insurance_COB_Section ICS
		JOIN Event_Insurance_COB_Section EIS on ISNULL(EIS.COB_Rating_Section_Id,0) = ISNULL(ICS.COB_Rating_Section_Id,0)
		JOIN Event_policy_coinsurers EPC2 On EPC2.insurance_section_id = EIS.insurance_section_id
		WHERE EPC2.insurance_file_cnt = @event_cnt
		AND EPC.insurance_section_id = EPC2.insurance_section_id),
    coinsurer_net_commission,
    coinsurer_commission_tax,
    coinsurer_cover_percentage,
    risk_transfer_agreement,
    signed_line_percentage,
    linestands,
    written_line_percentage,
    signed_line_amount,
    bureau_party_cnt,
    isleadunderwriter
from    event_policy_coinsurers EPC
where   insurance_file_cnt = @event_cnt
END
GO


