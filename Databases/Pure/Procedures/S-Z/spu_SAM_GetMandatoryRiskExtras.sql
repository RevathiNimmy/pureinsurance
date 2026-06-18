SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_GetMandatoryRiskExtras'
GO


CREATE PROCEDURE spu_SAM_GetMandatoryRiskExtras
    @risk_group_id integer,
    @transaction_type varchar(3)
AS
Begin
	select 
		fee_amounts.fee_percentage, 
		fee_amounts.fee_amount, 
		party.shortname, 
		party.resolved_name
	from 
		fee_amounts
		join party on party.party_cnt = fee_amounts.party_cnt
		left outer join transaction_type on transaction_type.transaction_type_id = fee_amounts.transaction_type_id
	where 
		fee_amounts.is_deleted = 0 and 
		fee_amounts.display_on_quotes=1 and 
		((@transaction_type='NB' and transaction_type.code in ('G_NB', 'NB D')) or
		 (@transaction_type='MTA' and transaction_type.code in ('G_MTA', 'MTA D')) or
		 (@transaction_type='RNL' and transaction_type.code in ('PMBRN', 'RN D', 'G_RENEW'))) and 
		risk_group_id=@risk_group_id 
End
Go