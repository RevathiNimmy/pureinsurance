SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_GetSelectableRiskExtras'
GO


CREATE PROCEDURE spu_SAM_GetSelectableRiskExtras
    @risk_group_id integer,
    @Gis_Scheme_id integer = 0
AS
Begin
if @Gis_Scheme_Id = 0
Begin
	select 
		fee_amounts.fee_percentage, 
		fee_amounts.fee_amount, 
		party.shortname, 
		party.resolved_name
	from 
		fee_amounts
		join party on party.party_cnt = fee_amounts.party_cnt
	where 
		fee_amounts.is_deleted = 0 and 
		fee_amounts.display_on_quotes=1 and fee_amounts.transaction_type_id is null and
		risk_group_id=@risk_group_id
End
Else
Begin
	select 
		fee_amounts.fee_percentage, 
		fee_amounts.fee_amount, 
		party.shortname, 
		party.resolved_name
	from 
		fee_amounts
		join party on party.party_cnt = fee_amounts.party_cnt
		join gis_Scheme_extras on gis_scheme_extras.party_cnt = party.party_cnt 
			and gis_scheme_extras.gis_scheme_id = @gis_scheme_id
	where 
		fee_amounts.is_deleted = 0 and 
		fee_amounts.display_on_quotes=1 and fee_amounts.transaction_type_id is null and
		risk_group_id=@risk_group_id
End

End
Go
