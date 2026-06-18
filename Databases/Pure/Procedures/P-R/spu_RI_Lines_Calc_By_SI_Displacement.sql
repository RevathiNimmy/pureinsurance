SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI_Lines_Calc_By_SI_Displacement'
GO


CREATE PROCEDURE spu_RI_Lines_Calc_By_SI_Displacement
    @insurance_file_cnt int,
    @ri_arrangement_id int
AS

Declare @old_ri_arrangement_id int,
	@si_displacement money,
	@band_premium_changed money,
	@band_si_changed money

	IF EXISTS(Select NULL from Insurance_File 
		Where insurance_file_cnt = @insurance_file_cnt AND insurance_file_type_id IN (4, 7))
	BEGIN
  	    Select @old_ri_arrangement_id = RIA_OLD.ri_arrangement_id, 
			@si_displacement = RIA_NEW.sum_insured - (RIA_OLD.sum_insured * -1) From RI_Arrangement RIA_OLD
		INNER JOIN (Select risk_cnt, ri_band_id, sum_insured From RI_Arrangement Where ri_arrangement_id = @ri_arrangement_id) RIA_NEW
			ON RIA_NEW.risk_cnt = RIA_OLD.risk_cnt AND RIA_NEW.ri_band_id = RIA_OLD.ri_band_id
			Where RIA_OLD.original_flag = 1

	    -- Ensure availability of old RI arrangement, change in SI and presence of multiple priorities
	    IF ISNULL(@old_ri_arrangement_id, 0) > 0 AND @si_displacement <> 0
		AND Exists (Select NULL From ri_arrangement_line 
				Where ri_arrangement_id = @ri_arrangement_id AND sum_Insured <> 0
					Having count(distinct priority) > 1)
	    BEGIN
		-- Ensure if displacement calc is possible by matching priorities and it's corresponding reinsurers
		IF NOT EXISTS (
		Select NULL From
			(Select priority, default_share_percent, treaty_id, party_cnt
				From RI_Arrangement_Line RIL
				Where ri_arrangement_id = @old_ri_arrangement_id AND treaty_id IS NOT NULL) RI_Old
		FULL OUTER JOIN
			(Select priority, default_share_percent, treaty_id, party_cnt
				From RI_Arrangement_Line RIL
				Where ri_arrangement_id = @ri_arrangement_id AND treaty_id IS NOT NULL) RI_New
			ON RI_Old.Priority = RI_New.Priority AND RI_Old.default_share_percent = RI_New.default_share_percent
				AND RI_Old.treaty_id = RI_New.treaty_id
			Where RI_Old.priority IS NULL OR RI_New.priority IS NULL ) 
			BEGIN
				Select  @band_premium_changed = SUM(premium),
					@band_si_changed = SUM(sum_insured) 
				From    RI_Arrangement   
				Where   ri_arrangement_id IN (@old_ri_arrangement_id, @ri_arrangement_id)

				-- Update premium percent by matching chanage in SI
				Update RI_Arrangement_Line
					Set Premium_value = 
						Case When Line_SI_Changed = 0 Then 
							TTY_SI_Changed.premium_value
					        Else 
							TTY_SI_Changed.premium_value + (@band_premium_changed * (Convert(float, Line_SI_Changed) / @band_si_changed)) End
				From RI_Arrangement_Line RIL INNER JOIN
				(Select DISTINCT RI_New.Treaty_Id, RI_New.Sum_Insured - RI_Old.Sum_Insured 'Line_SI_Changed', 
				    RI_New.priority, RI_Old.Premium_Value, RI_New.party_cnt From
					(Select priority, default_share_percent, treaty_id, party_cnt,
						sum_insured * -1 'sum_insured', premium_value * -1 'premium_value'
							From RI_Arrangement_Line RIL
								Where ri_arrangement_id = @old_ri_arrangement_id) RI_Old
				INNER JOIN
					(Select priority, default_share_percent, treaty_id, sum_insured, party_cnt 
							From RI_Arrangement_Line RIL
								Where ri_arrangement_id = @ri_arrangement_id) RI_New
				ON RI_Old.Priority = RI_New.Priority 
					AND RI_Old.default_share_percent = RI_New.default_share_percent
					AND ISNULL(RI_Old.treaty_id, RI_Old.party_cnt) = ISNULL(RI_New.treaty_id, RI_New.party_cnt)) TTY_SI_Changed
				ON ISNULL(RIL.Treaty_Id, RIL.party_cnt) = ISNULL(TTY_SI_Changed.Treaty_Id, TTY_SI_Changed.party_cnt)
					AND RIL.priority = TTY_SI_Changed.priority 
						AND RIL.RI_Arrangement_Id = @ri_arrangement_id

				
				Update RI_Arrangement_Line 
					Set premium_percent = 
					CASE WHEN RIL.sum_insured=0 OR premium_value=0  OR premium=0 THEN 0 
					ELSE (convert(float, premium_value) / premium) * 100 END
				From RI_Arrangement_Line RIL
					INNER JOIN RI_Arrangement RIA ON RIA.RI_Arrangement_Id = RIL.RI_Arrangement_Id
				Where RIA.RI_Arrangement_Id = @ri_arrangement_id
			END
 	    END
	END
Go
