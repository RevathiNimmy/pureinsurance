SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_update_coinsurer_from_event'
GO

CREATE  PROCEDURE spu_update_coinsurer_from_event
    @event_cnt INT,
    @insurance_file_cnt INT
AS
BEGIN

DECLARE	@Event_COB_rating_section_id int
DECLARE	@Event_party_cnt int

DECLARE C_Event_Coinsurer CURSOR FAST_FORWARD FOR
	SELECT 
	COB_rating_section_id,
	Party_cnt
	FROM event_policy_coinsurers_section
	WHERE Insurance_File_cnt = @event_cnt

OPEN C_Event_Coinsurer

FETCH NEXT FROM C_Event_Coinsurer INTO 
	@Event_COB_rating_section_id ,
	@Event_party_cnt
	
WHILE (@@FETCH_STATUS = 0)
BEGIN

	IF exists (SELECT * FROM policy_coinsurers_section
		WHERE (COB_Rating_Section_Id = @Event_COB_rating_section_id
		       OR COB_Rating_Section_id is NULL)
		AND Insurance_File_Cnt = @insurance_file_cnt
		AND party_cnt = @Event_party_cnt)
  	BEGIN
		UPDATE  S
		SET S.premium_exc_tax = S.premium_exc_tax - ES.premium_exc_tax,
		S.premium_inc_tax = S.premium_inc_tax - ES.premium_inc_tax,
		S.commission_charge = S.commission_charge - ES.commission_charge,
		S.commission_exc_tax = S.commission_exc_tax - ES.commission_exc_tax,
		S.commission_inc_tax = S.commission_inc_tax - ES.commission_inc_tax
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
		@Event_COB_rating_section_id,
		@Event_party_cnt

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

