SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLDropProcedure 'spu_update_coinsurer_to_event'
GO


CREATE  PROCEDURE spu_update_coinsurer_to_event
    @event_cnt INT,
    @insurance_file_cnt INT
AS
BEGIN

DECLARE	@Event_party_cnt int,
	@Event_COB_rating_section_id int,
	@Event_sequence int,
	@Event_share_percent numeric(19,4),
	@Event_premium_exc_tax numeric(19,4),
	@Event_premium_inc_tax numeric(19,4),
	@Event_tax_group_id int,
	@Event_commission_percent numeric(19,4) ,
	@Event_commission_charge numeric(19,4) ,
	@Event_commission_exc_tax numeric(19,4),
	@Event_commission_inc_tax numeric(19,4),
	@Event_commission_tax_group_id int,
	@Event_base_premium_exc_tax numeric(19,4),
	@Event_base_premium_inc_tax numeric(19,4),
	@Event_base_commission_charge numeric(19,4) ,
	@Event_base_commission_exc_tax numeric(19,4),
	@Event_base_commission_inc_tax numeric(19,4) 


DECLARE C_Event_Coinsurer CURSOR FAST_FORWARD FOR
	SELECT 
	party_cnt,
	COB_rating_section_id,
	[sequence],
	share_percent,
	premium_exc_tax,
	premium_inc_tax,
	tax_group_id,
	commission_percent,
	commission_charge,
	commission_exc_tax,
	commission_inc_tax,
	commission_tax_group_id,
	base_premium_exc_tax,
	base_premium_inc_tax,
	base_commission_charge,
	base_commission_exc_tax,
	base_commission_inc_tax
	FROM event_policy_coinsurers_section
	WHERE Insurance_File_cnt = @event_cnt

OPEN C_Event_Coinsurer

FETCH NEXT FROM C_Event_Coinsurer INTO
	@Event_party_cnt,
	@Event_COB_rating_section_id,
	@Event_sequence,
	@Event_share_percent,
	@Event_premium_exc_tax,
	@Event_premium_inc_tax,
	@Event_tax_group_id,
	@Event_commission_percent,
	@Event_commission_charge,
	@Event_commission_exc_tax,
	@Event_commission_inc_tax,
	@Event_commission_tax_group_id,
	@Event_base_premium_exc_tax,
	@Event_base_premium_inc_tax,
	@Event_base_commission_charge,
	@Event_base_commission_exc_tax,
	@Event_base_commission_inc_tax 

WHILE (@@FETCH_STATUS = 0)
BEGIN

	IF exists (SELECT * FROM policy_coinsurers_section
		WHERE (COB_Rating_Section_Id = @Event_COB_rating_section_id
		       OR COB_Rating_Section_id is NULL)
		AND Insurance_File_Cnt = @insurance_file_cnt 
		AND party_cnt = @Event_party_cnt)



  	BEGIN
		UPDATE  S
		SET S.premium_exc_tax = S.premium_exc_tax + ES.premium_exc_tax,
		S.premium_inc_tax = S.premium_inc_tax + ES.premium_inc_tax,
		S.commission_charge = S.commission_charge + ES.commission_charge,
		S.commission_exc_tax = S.commission_exc_tax + ES.commission_exc_tax,
		S.commission_inc_tax = S.commission_inc_tax + ES.commission_inc_tax
		FROM policy_coinsurers_section S
		JOIN Event_policy_coinsurers_section ES ON ISNULL(S.COB_Rating_Section_id,0) = ISNULL(ES.COB_Rating_Section_id,0)
		WHERE (ES.COB_Rating_Section_Id = @Event_COB_Rating_Section_id
			OR ES.COB_Rating_Section_Id is NULL)
		AND ES.insurance_file_cnt = @event_cnt
		AND S.insurance_file_cnt =  @insurance_file_Cnt
		AND S.party_cnt = @Event_party_cnt
		AND ES.party_cnt = @Event_party_cnt

  	END

	FETCH NEXT FROM C_Event_Coinsurer INTO
		@Event_party_cnt,
		@Event_COB_rating_section_id,
		@Event_sequence,
		@Event_share_percent,
		@Event_premium_exc_tax,
		@Event_premium_inc_tax,
		@Event_tax_group_id,
		@Event_commission_percent,
		@Event_commission_charge,
		@Event_commission_exc_tax,
		@Event_commission_inc_tax,
		@Event_commission_tax_group_id,
		@Event_base_premium_exc_tax,
		@Event_base_premium_inc_tax,
		@Event_base_commission_charge,
		@Event_base_commission_exc_tax,
		@Event_base_commission_inc_tax 

END

CLOSE C_Event_Coinsurer
DEALLOCATE C_Event_Coinsurer

--exec spu_update_insurance_file_from_COB_Sections @insurance_file_cnt,@event_cnt

END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

