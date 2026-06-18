SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_upd_FAC_XOL'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_Line_upd_FAC_XOL  
	@GroupingFACXOL int=0,  	
	@claim_id int,
	@ri_arrangement_line_id int,
	@agreement_code varchar(255) =''

AS

DECLARE @Gross_this_reserve Money
DECLARE @os_reserve Money
DECLARE @RI_Arrangement_Id Numeric
SELECT @RI_Arrangement_Id = ri_arrangement_id from claim_ri_arrangement_line where claim_id = @claim_id And ri_arrangement_line_id = @ri_arrangement_line_id  
SELECT @Gross_this_reserve = this_reserve FROM claim_ri_arrangement WHERE ri_arrangement_id = @RI_Arrangement_Id and claim_id = @claim_id 
SELECT @os_reserve = SUM(this_reserve) from claim_ri_arrangement_line Where claim_id=@claim_id And ri_arrangement_id = @RI_Arrangement_Id AND type in ('F') 

SET @os_reserve = @Gross_this_reserve - @os_reserve

DECLARE @ri_Arrangement_line_id1 NUMERIC
DECLARE @line_limit MONEY
DECLARE @lower_limit MONEY
DECLARE @ParticipationPercent FLOAT
DECLARE @reserve MONEY
DECLARE @this_reserve MONEY
DECLARE @Gross_Net_Reserve MONEY

SET @Gross_Net_Reserve = 0
  DECLARE Upd_clm_ri_Lines_FACXOL CURSOR  FOR
  SELECT ri_Arrangement_line_id,
    line_limit,
    lower_limit,
    Participation_Percent,
	Reserve
  FROM   Claim_ri_Arrangement_Line
  WHERE  Claim_Id = @claim_id
	and ri_arrangement_id = @RI_Arrangement_Id and type='FX' and grouping = @GroupingFACXOL


 SELECT ri_Arrangement_line_id,
    line_limit,
    lower_limit,
    Participation_Percent,
	Reserve
  FROM   Claim_ri_Arrangement_Line
  WHERE  Claim_Id = 66145
	and ri_arrangement_id = 96192 and type='FX' and grouping = 204838

  OPEN Upd_clm_ri_Lines_FACXOL

  FETCH NEXT FROM Upd_clm_ri_Lines_FACXOL
  INTO 
	@ri_Arrangement_line_id1,
	@line_limit,
    @lower_limit,
    @ParticipationPercent,
	@Reserve

  WHILE @@FETCH_STATUS = 0
  BEGIN

    IF @os_reserve>@lower_limit
    BEGIN
	      IF @os_reserve>@line_limit
	      BEGIN
			        IF @Reserve>= (@line_limit - @lower_limit) * @ParticipationPercent
			          SELECT @this_reserve = 0
			        ELSE
			          --SELECT @this_reserve = (@line_limit - @lower_limit - @Reserve) * @ParticipationPercent  
			          SELECT @this_reserve = (@line_limit - @lower_limit ) * (@ParticipationPercent/100) - @Reserve
			      	END
	      ELSE
	      BEGIN
		        IF @Reserve>=( @line_limit - @lower_limit ) * (@ParticipationPercent/100)
		          SELECT @this_reserve = 0
		        ELSE
		          SELECT @this_reserve = (@os_reserve - @lower_limit ) * (@ParticipationPercent/100) - @Reserve
		      	END

      	IF @this_reserve > @Gross_this_reserve -- - @this_reserve_used
        --Set @this_reserve = (@Gross_this_reserve - @this_reserve_used) * @ParticipationPercent
		Set @this_reserve = (@Gross_this_reserve) * @ParticipationPercent


		SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve - @Reserve

		Update claim_ri_arrangement_line  
		Set         agreement_code = @agreement_code,  
		--this_reserve = @this_reserve * participation_percent/100 --,  
		this_reserve = @this_reserve
		--this_payment = @this_payment  
		Where   claim_id = @claim_id  
		And Grouping= @GroupingFACXOL
		AND ri_Arrangement_line_id = @ri_Arrangement_line_id1

    END

   FETCH NEXT FROM Upd_clm_ri_Lines_FACXOL
   INTO @ri_Arrangement_line_id1,
	@line_limit,
    @lower_limit,
    @ParticipationPercent,
	@Reserve

  END

 CLOSE Upd_clm_ri_Lines_FACXOL
 DEALLOCATE Upd_clm_ri_Lines_FACXOL

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO