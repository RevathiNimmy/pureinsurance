SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_calculate_claims_ri_method_2_full_original'
GO

CREATE PROCEDURE spu_calculate_claims_ri_method_2_full_original
  @claim_id          INT,
  @ri_arrangement_id INT,
  @total_reserve     MONEY,
  @total_payment     MONEY,
  --Arul Stephen
  @Reapply_TX        INT = 0,
  @Recovery          TINYINT=2
--End Arul Stephen
AS
  -- Note: This method will NOT allocate negative TOTAL reserve or payment amounts
  DECLARE
  -- The line details for the reinsurance
  @line_id               INT,
  @sum_insured           MONEY,
  -- The reserves and payments to be allocated to an RI line
  @this_reserve          MONEY,
  @this_payment          MONEY,
  -- Remaining allocation
  @os_reserve            MONEY,
  @os_payment            MONEY,
  @ri_type               VARCHAR(2),
  @product_option        VARCHAR(20),
  @lower_limit           MONEY,
  @line_limit            MONEY,
  @remaining_limit       MONEY,
  @default_share_percent numeric(19,8),
  @Gross_SumInsured      MONEY,
  @Net_SumInsured        MONEY,
  @os_SumInsured         MONEY,
  @this_SumInsured       MONEY,
  @Reserve               MONEY,
  @Payment               MONEY,
  @Gross_Reserve_to_date MONEY,
  @Gross_This_reserve    MONEY,
  @Gross_Net_Reserve     MONEY,
  @Gross_Net_Payment     MONEY,
  @Gross_Payment_to_date MONEY,
  @Gross_This_Payment    MONEY,
  @total_reserve_used    MONEY,--  Total Reserve used i.e Reserve to date + this reserve
  @total_payment_done    MONEY,-- Total Payment done i.e Paid to date + This payment
  @this_reserve_used     MONEY,
  @this_payment_done     MONEY,
  @treaty_reserve        MONEY,
  @treaty_payment        MONEY,
  @this_share_percent    numeric(19,8),
  @retained              TINYINT,
  @FACRetained           MONEY,
  @CatastropheCodeId    INT

  SELECT @CatastropheCodeId = ISNULL(catastrophe_code_id, 0) FROM claim WHERE claim_id = @claim_id

  SELECT @os_reserve = ISNULL(@total_reserve, 0),
		 @os_payment = ISNULL(@total_payment, 0),
		 @Gross_Net_Reserve = ISNULL(@total_reserve, 0),
		 @Gross_Net_Payment = ISNULL(@total_Payment, 0)

  SET @total_reserve_used = 0
  SET @total_payment_done = 0
  SET @this_reserve_used = 0
  SET @this_payment_done = 0

  -- See if we have anything to allocate
  --IF (@total_reserve > 0) OR (@total_payment > 0) BEGIN
  SELECT @product_option = '0'

  SELECT @product_option = ISNULL(VALUE, 0)
  FROM   Hidden_Options
  WHERE  option_number = 88

  SELECT @Gross_Reserve_to_date = SUM(ISNULL(reserve, 0))
  FROM   CLAIM_RI_ARRANGEMENT_LINE
  WHERE  Claim_id = @Claim_id
		 AND ri_arrangement_id = @ri_arrangement_id
		 AND NOT ( TYPE = 'FX'
				   AND retained = 1 )

  --Arul Stephen
  SELECT @Gross_Payment_to_date = ISNULL(SUM(Payment), 0) + ( ISNULL(SUM(Salvage), 0) + ISNULL(SUM(Recovery), 0) )
  FROM   CLAIM_RI_ARRANGEMENT_LINE
  WHERE  Claim_id = @Claim_id
		 AND ri_arrangement_id = @ri_arrangement_id
		 AND NOT ( TYPE = 'FX'
				   AND retained = 1 )

  --End Arul Stephen
  SELECT @Gross_This_Reserve = @Total_reserve - @Gross_Reserve_to_date

  SELECT @Gross_This_Payment = @Total_Payment - @Gross_Payment_to_date

  IF @product_option = '0'
	BEGIN
		-- Declare the cursor
		DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR
		  -- Select the treaties with standard priority, method and lines
		  -- Note: We do NOT allocate XOL here
		  SELECT ri_arrangement_line_id,
				 ISNULL(sum_insured, 0),
				 TYPE,
				 ISNULL(lower_limit, 0),-- these are not used for this cursor
				 ISNULL(line_limit, 0),-- but are here for the generic loop below
				 Reserve,
				 Payment,
				 ISNULL(default_share_percent, 0),
				 ISNULL(this_share_percent, 0)
		  FROM   claim_ri_arrangement_line
		  WHERE  claim_id = @claim_id
				 AND ri_arrangement_id = @ri_arrangement_id
				 AND TYPE IN ( 'R', 'T', 'F' ) -- Do not update XOL
		  -- As we are allocating additions we should do this from the top down so sort ascending
		  ORDER  BY priority ASC,
					number_of_lines ASC
	END
  ELSE
	BEGIN
		CREATE TABLE #RICursor
		  (
			 ri_arrangement_line_id INT,
			 sum_insured            MONEY,
			 TYPE                   VARCHAR(2),
			 lower_limit            MONEY,
			 line_limit             MONEY,
			 Reserve                MONEY,
			 Payment                MONEY,
			 default_share_percent  MONEY,
			 this_share_percent     MONEY
		  )

		IF @Reapply_TX = 1
		  BEGIN
			  SELECT @Gross_SumInsured = ISNULL(Sum_insured, 0)
			  FROM   Claim_ri_Arrangement
			  WHERE  Claim_id = @Claim_id
					 AND ri_arrangement_id = @ri_arrangement_id

			  SELECT @Net_SumInsured = @Gross_SumInsured - SUM(ISNULL(Sum_insured, 0))
			  FROM   Claim_ri_Arrangement_line
			  WHERE  ri_arrangement_id = @ri_arrangement_id
					 AND TYPE IN ( 'F', 'FX' )
					 AND retained = 0

			  SET @os_SumInsured=@Net_SumInsured

			  UPDATE claim_ri_arrangement_line
			  SET    Sum_Insured = @os_SumInsured * default_share_percent * 0.01
			  WHERE  claim_id = @claim_id
					 AND TYPE = 'T'

			  SELECT @os_SumInsured = @os_SumInsured - SUM(Sum_insured)
			  FROM   claim_ri_arrangement_line
			  WHERE  claim_id = @claim_id
					 AND ri_arrangement_id = @ri_arrangement_id
					 AND TYPE = 'T'

			  DECLARE Update_TX CURSOR FOR
				SELECT ri_arrangement_line_id,
					   ISNULL(sum_insured, 0),
					   TYPE,
					   ISNULL(lower_limit, 0),
					   ISNULL(line_limit, 0),
					   ISNULL(default_share_percent, 0)
				FROM   claim_ri_arrangement_line
				WHERE  claim_id = @claim_id
					   AND ri_arrangement_id = @ri_arrangement_id
					   AND TYPE IN ( 'R', 'TX' )
				ORDER  BY TYPE DESC,
						  ISNULL(line_limit, 0) DESC

			  OPEN Update_TX

			  FETCH NEXT FROM Update_TX INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit, @default_share_percent

			  WHILE @@FETCH_STATUS = 0
				BEGIN
					IF @ri_type = 'TX'
					  BEGIN
						  IF @os_SumInsured > @lower_limit
							BEGIN
								IF @os_SumInsured > @line_limit
								  SELECT @this_SumInsured = @line_limit - @lower_limit
								ELSE
								  SELECT @this_SumInsured = @os_SumInsured - @lower_limit
							END

						  SELECT @os_SumInsured = @os_SumInsured - @this_SumInsured
					  END

					IF @ri_type = 'R'
					  IF @os_SumInsured > 0
						BEGIN
							SET @this_SumInsured=@os_SumInsured
							SET @os_SumInsured=0
						END

					IF @ri_type IN ( 'TX', 'R' )
					  UPDATE claim_ri_arrangement_line
					  SET    Sum_Insured = @this_SumInsured
					  WHERE  claim_id = @claim_id
							 AND ri_arrangement_line_id = @line_id

					FETCH NEXT FROM Update_TX INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit, @default_share_percent
				END

			  CLOSE Update_TX

			  DEALLOCATE Update_TX
		  END

		--If the Reserve is Negative call spu_calculate_claims_ri_method_Negative_Reserve
		IF @Gross_This_Reserve < 0
			OR @Gross_this_payment < 0
		  BEGIN
			  --Set @Gross_This_Reserve = @Gross_This_Reserve * -1
			  EXEC spu_calculate_claims_ri_method_Negative_Reserve
				@claim_id = @claim_id,
				@ri_arrangement_id = @ri_arrangement_id,
				@total_reserve = @Total_reserve,
				--Arul Stephen
				@total_payment = @Total_Payment,
				@Recovery=@Recovery

			  --End Arul Stephen
			  RETURN
		  END

		-- This is for the new style RI Calculation
		-- Select the RI Lines in the order FAC Prop, FAC XOL, Treaty XOL, Treaty Prop, Retained
		INSERT INTO #RICursor
		SELECT ri_arrangement_line_id,
			   ISNULL(sum_insured, 0),
			   TYPE,
			   ISNULL(lower_limit, 0),
			   ISNULL(line_limit, 0),
			   ISNULL(Reserve, 0),
			   ISNULL(Payment, 0) + ISNULL(salvage, 0) + ISNULL(recovery, 0),
			   ISNULL(default_share_percent, 0),
			   ISNULL(this_share_percent, 0)
		FROM   claim_ri_arrangement_line
		WHERE  claim_id = @claim_id
			   AND ri_arrangement_id = @ri_arrangement_id
			   AND TYPE IN ( 'F' )
		ORDER  BY priority ASC,
				  number_of_lines ASC

		INSERT INTO #RICursor
		SELECT ri_arrangement_line_id,
			   ISNULL(sum_insured, 0),
			   TYPE,
			   ISNULL(lower_limit, 0),
			   ISNULL(line_limit, 0),
			   ISNULL(Reserve, 0),
			   ISNULL(Payment, 0) + ISNULL(salvage, 0) + ISNULL(recovery, 0),
			   ISNULL(default_share_percent, 0),
			   ISNULL(this_share_percent, 0)
		FROM   claim_ri_arrangement_line
		WHERE  claim_id = @claim_id
			   AND ri_arrangement_id = @ri_arrangement_id
			   AND TYPE IN ( 'FX', 'T' )
			   AND retained = 0
		ORDER  BY TYPE ASC,
				  line_limit

		INSERT INTO #RICursor
		SELECT ri_arrangement_line_id,
			   ISNULL(sum_insured, 0),
			   TYPE,
			   ISNULL(lower_limit, 0),
			   ISNULL(line_limit, 0),
			   ISNULL(Reserve, 0),
			   ISNULL(Payment, 0) + ISNULL(salvage, 0) + ISNULL(recovery, 0),
			   ISNULL(default_share_percent, 0),
			   ISNULL(this_share_percent, 0)
		FROM   claim_ri_arrangement_line
		WHERE  claim_id = @claim_id
			   AND ri_arrangement_id = @ri_arrangement_id
			   AND TYPE IN ( 'TX', 'R' )
		ORDER  BY TYPE DESC,
				  priority ASC,
				  number_of_lines ASC

		SELECT @treaty_reserve = SUM(Reserve),
			   @treaty_payment = SUM(Payment)
		FROM   #RICursor
		WHERE  TYPE = 'T'

		--Arul Stephen
		SET @treaty_reserve=ISNULL(@treaty_reserve, 0)
		SET @treaty_payment=ISNULL(@treaty_payment, 0)

		--End Arul Stephen
		-- Declare the cursor
		DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR
		  SELECT ri_arrangement_line_id,
				 ISNULL(sum_insured, 0),
				 TYPE,
				 ISNULL(lower_limit, 0),
				 ISNULL(line_limit, 0),
				 ISNULL(Reserve, 0),
				 ISNULL(Payment, 0),
				 ISNULL(default_share_percent, 0),
				 ISNULL(this_share_percent, 0)
		  FROM   #RICursor
	END

  -- Open treaty cursor
  OPEN RI_Cursor

  FETCH NEXT FROM RI_Cursor INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit, @Reserve, @Payment, @default_share_percent, @this_share_percent

  SET @retained=0

  WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @ParticipationPercent FLOAT
		DECLARE @IsMultiact INT
		DECLARE @IsRetained TINYINT

		SELECT @this_reserve = 0,
			   @this_payment = 0,
			   @IsRetained = 0

		/*
		  Treaty XOL Rows handled seperately From FAC XOL Rows.
		  For FAC XOL    -- Reserve is calculated based on Gross Reserve.
		  For Treaty XOL -- Reserve is calculated based on Gross Net Reserve.
		*/
		IF @ri_type = 'FX'
		  BEGIN
			  SELECT @ParticipationPercent = ISNULL(Participation_Percent, 0) / 100,
					 @IsRetained = ISNULL(retained, 0)
			  FROM   Claim_RI_Arrangement_line
			  WHERE  claim_id = @Claim_Id
					 AND ri_arrangement_line_id = @line_id

			  --Note:- @ParticipationPercent is made to 0 since the participationpercent is not given and
			  -- it has to be allocated as per  upper and lower limit
			  IF @ParticipationPercent = 0
				SET @ParticipationPercent=1

			 -- IF @Gross_Payment_to_date > 0
				--SELECT @payment = @Gross_Payment_to_date

			  IF @os_reserve > @lower_limit
				BEGIN
					IF @os_reserve > @line_limit
					  BEGIN
						  IF @Reserve >= ( @line_limit - @lower_limit ) * @ParticipationPercent
							SELECT @this_reserve = 0
						  ELSE
							--SELECT @this_reserve = (@line_limit - @lower_limit - @Reserve) * @ParticipationPercent
							SELECT @this_reserve = ( @line_limit - @lower_limit ) * @ParticipationPercent - @Reserve
					  END
					ELSE
					  BEGIN
						  IF @Reserve >= ( @line_limit - @lower_limit ) * @ParticipationPercent
							SELECT @this_reserve = 0
						  ELSE
							SELECT @this_reserve = ( @os_reserve - @lower_limit ) * @ParticipationPercent - @Reserve
					  END

					IF @this_reserve > @Gross_this_reserve - @this_reserve_used
					  SET @this_reserve = ( @Gross_this_reserve - @this_reserve_used ) * @ParticipationPercent

					IF @IsRetained = 0
					  BEGIN
						  SELECT @Gross_Net_Reserve = @Gross_Net_Reserve - @this_reserve - @Reserve
					  END
					ELSE
					  BEGIN
						  SELECT @FACRetained = ISNULL(@FACRetained, 0) + ISNULL(@this_reserve, 0) -- + ISNULL(@Reserve,0)
					  END
				END

			  IF ( @os_payment > @lower_limit
				   AND @line_limit > @lower_limit )
				  OR ( @Recovery != 2 )
				BEGIN
					IF @os_payment > @line_limit
					  BEGIN
						  IF @Payment >= ( @line_limit - @lower_limit ) * @ParticipationPercent
							SELECT @this_payment = 0
						  ELSE
							SELECT @this_payment = ( @line_limit - @lower_limit - @Payment ) * @ParticipationPercent
					  END
					ELSE
					  IF @Payment >= ( @line_limit - @lower_limit ) * @ParticipationPercent
						SELECT @this_payment = 0
					  ELSE
						IF @os_payment >= @line_limit - @Payment
						  SELECT @this_payment = ( @os_payment - @lower_limit - @Payment ) * @ParticipationPercent
						ELSE
						  SELECT @this_Payment = ( @os_payment - @lower_limit - @Payment ) * @ParticipationPercent

					IF @retained = 0
					  SELECT @Gross_Net_Payment = @Gross_Net_Payment - @this_payment - @Payment

					IF @this_payment > @Gross_this_payment - @this_payment_done
					  SET @this_payment = @Gross_this_payment - @this_payment_done
				END
		  END --Type='FX'
		ELSE
		  IF @ri_type = 'TX'
			BEGIN

				 IF @Gross_Net_Payment-@treaty_payment>=@line_limit
				  SELECT @this_payment=@line_limit - @lower_limit- @Payment
				 ELSE
				  IF @Gross_Net_Payment-@treaty_payment<=@lower_limit
				  SELECT @this_payment = 0
				 ELSE
				  SELECT @this_payment=(@Gross_Net_Payment-@treaty_payment)- @lower_limit- @Payment
				         --Type='FX'

				IF @Gross_Net_reserve - @treaty_reserve >= @line_limit
				  SELECT @this_reserve = @line_limit - @lower_limit - @Reserve
				ELSE
				  IF @Gross_Net_reserve - @treaty_reserve <= @lower_limit
					SELECT @this_reserve = 0
				  ELSE
					SELECT @this_reserve = ( @Gross_Net_reserve - @treaty_reserve ) - @lower_limit - @Reserve

				--SELECT @treaty_reserve = @treaty_reserve + @this_reserve+@Reserve
				--Now need to work on gross net not reducing
				IF @Gross_Net_Payment - @treaty_payment >= @line_limit
				  SELECT @this_payment = @line_limit - @lower_limit - @Payment
				ELSE
				  IF @Gross_Net_Payment - @treaty_payment <= @lower_limit
					SELECT @this_payment = 0
				  ELSE
					SELECT @this_payment = ( @Gross_Net_Payment - @treaty_payment ) - @lower_limit - @Payment
			END /* PN64438 (Parallel of PN69400) */
		  ELSE
			IF @ri_type = 'T'
			  BEGIN
				  IF @os_reserve > 0
					BEGIN
						-- Decide how much to allocate
						-- Allocate all the reserve
						SELECT @this_reserve = @Gross_Net_reserve * @this_share_percent / 100 - @Reserve,
							   @os_reserve = @os_reserve - @this_reserve - @Reserve,
							   --@Gross_Net_reserve = @os_reserve,
							   @treaty_reserve = @treaty_reserve + @this_reserve
					END

				  -- If we have payments and spare reserve allocate them
				  IF @os_payment > 0
					BEGIN
						-- Decide how much to allocate
					    SELECT  @this_payment = @Gross_Net_Payment * @this_share_percent/100 - @Payment,
                             @os_payment = @os_payment - @this_payment - @Payment,
                             --@Gross_Net_Payment = @os_payment,
                             @treaty_payment=@treaty_payment+@this_payment

					END
			  END
			ELSE
			  IF @ri_type = 'F'
				BEGIN

					  BEGIN
						  -- Decide how much to allocate
						  -- Allocate all the reserve
						  SELECT @this_reserve = round(@total_reserve * @this_share_percent / 100,2) - @Reserve,
								 @os_reserve = @os_reserve - @this_reserve - @Reserve,
								 @Gross_Net_reserve = @os_reserve
					  END

					-- If we have payments and spare reserve allocate them

					  BEGIN
						  -- Decide how much to allocate
						  SELECT @this_payment = round(@total_payment * @this_share_percent / 100,2) - @Payment,
								 @os_payment = @os_payment - @this_payment - @Payment,
								 @Gross_Net_Payment = @os_payment
					  END
				END
			  ELSE
				BEGIN --Retained line
					IF (ISNULL(@Total_Reserve, 0) - ISNULL(@total_reserve_used, 0) - ISNULL(@Reserve, 0) <> 0 AND @CatastropheCodeId = 0) OR (ISNULL(@Total_Reserve, 0) - ISNULL(@total_reserve_used, 0) - ISNULL(@Reserve, 0) <> 0 AND @CatastropheCodeId <> 0) OR @Gross_This_Reserve<0
					  BEGIN
						  --IF @sum_insured > @Total_Reserve - @total_reserve_used -@Reserve
						  SELECT @This_reserve = (ISNULL(@Total_Reserve, 0) - ISNULL(@total_reserve_used, 0)) * @default_share_percent / 100 - ISNULL(@Reserve, 0)
						  IF  @This_reserve>@line_limit
					 	  BEGIN
							SELECT @This_reserve=@line_limit
					 	  END
					  END

					--IF (ISNULL(@FACRetained,0) > 0) AND (ISNULL(@Total_Reserve,0) - ISNULL(@total_reserve_used,0) -ISNULL(@Reserve,0) = 0)
					-- IF (ISNULL(@FACRetained,0) > 0) AND (ISNULL(@Total_Reserve,0) - ISNULL(@total_reserve_used,0) -ISNULL(@This_reserve,0) = 0)
					--BEGIN
					--		SELECT @This_reserve = ISNULL(@Total_Reserve,0) - ISNULL(@total_reserve_used,0) + ISNULL(@FACRetained,0)
					--END
					-- If we have payments and spare reserve allocate them
					IF ISNULL(@Total_payment, 0) - ISNULL(@total_Payment_done, 0) - ISNULL(@Payment, 0) <> 0 OR @Gross_This_Payment<0
					  BEGIN -- Decide how much to allocate
						  SELECT @this_payment = (ISNULL(@Total_payment, 0) - ISNULL(@total_Payment_done, 0)) *@default_share_percent / 100 - ISNULL(@Payment, 0)
					  END
					ELSE
					  IF ISNULL(@Total_payment, 0) - ISNULL(@total_Payment_done, 0) - ISNULL(@Payment, 0) < 0
						SELECT @this_payment = @Gross_Net_Payment
						IF  @this_payment>@line_limit
						BEGIN
							SELECT @this_payment=@line_limit
						END
				END

		SET @total_reserve_used = ISNULL(@total_reserve_used, 0) + ISNULL(@This_Reserve, 0) + ISNULL(@Reserve, 0)
		SET @total_payment_done = ISNULL(@total_payment_done, 0) + ISNULL(@This_Payment, 0) + ISNULL(@Payment, 0)
		SET @this_reserve_used = ISNULL(@this_reserve_used, 0) + ISNULL(@This_Reserve, 0)
		SET @this_payment_done = ISNULL(@this_payment_done, 0) + ISNULL(@this_payment, 0)

		-- Update line with new values
		IF @Recovery = 1
		  BEGIN
			  UPDATE claim_ri_arrangement_line
			  SET    this_reserve = ISNULL(@this_reserve, 0),
					 this_salvage = ISNULL(@this_payment, 0)
			  WHERE  claim_id = @claim_id
					 AND ri_arrangement_line_id = @line_id
		  END
		ELSE
		  IF @Recovery = 0
			BEGIN
				UPDATE claim_ri_arrangement_line
				SET    this_reserve = ISNULL(@this_reserve, 0),
					   this_recovery = ISNULL(@this_payment, 0)
				WHERE  claim_id = @claim_id
					   AND ri_arrangement_line_id = @line_id
			END
		  ELSE
			BEGIN
				UPDATE claim_ri_arrangement_line
				SET    this_reserve = ISNULL(@this_reserve, 0),
					   this_payment = ISNULL(@this_payment, 0)
				WHERE  claim_id = @claim_id
					   AND ri_arrangement_line_id = @line_id
			END

		FETCH NEXT FROM RI_Cursor INTO @line_id, @sum_insured, @ri_type, @lower_limit, @line_limit, @Reserve, @Payment, @default_share_percent, @this_share_percent
	END

	IF ISNULL(@total_reserve, 0) - ISNULL(@total_reserve_used, 0) > 0
	Update Claim_RI_Arrangement_line set this_reserve=isnull(this_reserve,0)+ISNULL(@total_reserve, 0) - ISNULL(@total_reserve_used, 0)
	WHERE claim_id = @claim_id and type='R'

	IF ISNULL(@Total_payment, 0) - ISNULL(@total_Payment_done, 0) > 0
	Update Claim_RI_Arrangement_line set this_payment=isnull(this_payment,0)+ISNULL(@Total_payment, 0) - ISNULL(@total_Payment_done, 0)
	WHERE claim_id = @claim_id and type='R'

  CLOSE RI_Cursor

  DEALLOCATE RI_Cursor

  EXEC DDLDropTable '#RI_Cursor'
--    END -- (@total_reserve > 0) OR (@total_payment > 0)

