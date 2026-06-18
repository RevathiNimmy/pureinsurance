SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_calculate_claims_ri_method_2_full_RI2007'
GO  
CREATE PROCEDURE spu_calculate_claims_ri_method_2_full_RI2007  
    @claim_id INT,  
    @ri_arrangement_id INT,  
    @total_reserve MONEY,  
    @total_payment MONEY,  
    @Reapply_TX INT = 0,  
    @Recovery TINYINT = 2,  
    @Reapply_Treaty INT = 0  
AS  
BEGIN  
DECLARE      
        @line_id INT,      
        @sum_insured MONEY,      
        @this_reserve MONEY,      
        @this_payment MONEY,      
        @os_reserve MONEY,      
        @os_payment MONEY,      
        @ri_type VARCHAR(3),      
        @product_option VARCHAR(20),      
        @lower_limit MONEY,      
        @line_limit MONEY,      
        @remaining_limit MONEY,      
        @default_share_percent NUMERIC(19,8),      
        @CedePremiumOnly INT,      
        @Gross_SumInsured FLOAT,      
        @Net_SumInsured FLOAT,      
        @os_SumInsured FLOAT,      
  @running_SumInsured FLOAT,      
        @this_SumInsured MONEY,      
        @Reserve MONEY,      
        @Payment MONEY,      
        @Gross_Reserve_to_date MONEY,      
        @Gross_This_reserve MONEY,      
        @Gross_Net_Reserve MONEY,      
        @Gross_Net_Payment MONEY,      
        @Gross_Payment_to_date MONEY,      
        @Gross_This_Payment MONEY,      
        @total_reserve_used MONEY,      
        @total_payment_done MONEY,      
        @this_reserve_used MONEY,      
        @this_payment_done MONEY,      
        @QSReserve MONEY,      
        @QSPayment MONEY,      
        @this_share_percent FLOAT,      
        @Recovery_payment MONEY,      
        @retained MONEY,      
        @FACRetained MONEY,      
        @retained_reserve MONEY,      
        @retained_payment MONEY,      
        @is_obligatory TINYINT,   
  @obligatory_SI MONEY,  
        @obligatory_reserve MONEY,      
        @obligatory_payment MONEY,      
        @treaty_type_id INT,      
        @priority INT,      
        @IsPortfolioTransferred INT,      
        @RetainedRemainingReserve MONEY,      
        @RetainedthisPayment MONEY,      
        @Retainedpayment MONEY,      
        @extended_limit_amount MONEY,      
        @extended_limit_Enabled INT,      
        @ri_band_id INT,     
  @FACSumInsured FLOAT,  
        @FACTotalReserve MONEY,      
        @FACTotalPayment MONEY,      
        @FACPTSummary MONEY,      
        @FACSummaryReserve MONEY,      
        @FACSummaryPayment MONEY,      
        @Running_Reserve MONEY,      
        @Running_Payment MONEY,      
  @priority_Reserve MONEY,      
        @priority_Payment MONEY,      
        @ri_model_line_id INT,      
  @ret_line_id int,    
  @is_variableQuotaShare tinyint,  --quotashare config      
  @manually_added_treaty bit,  
  @SI_Manually_added_treaty MONEY,
  @reinsurance_type_id INT,
  @is_edited_line BIT;  
      
    -- Initialize variables      
    SET @RetainedthisPayment = 0;      
    SET @Net_SumInsured = 0.00;      
      
    SELECT      
        @os_reserve = ISNULL(@total_reserve, 0),      
        @os_payment = ISNULL(@total_payment, 0),      
        @Gross_Net_Reserve = 0,      
        @Gross_Net_Payment = 0;      
      
    -- GET GROSS THIS VALUES      
    SELECT      
        @Gross_Reserve_to_date = SUM(ISNULL(reserve, 0)),      
        @Gross_Payment_to_date = ISNULL(SUM(Payment), 0) + ISNULL(SUM(Salvage), 0) + ISNULL(SUM(Recovery), 0),      
        @Recovery_payment = ISNULL(SUM(Salvage), 0) + ISNULL(SUM(Recovery), 0)      
    FROM Claim_RI_Arrangement_Line      
    WHERE Claim_id = @Claim_id      
      AND ri_arrangement_id = @ri_arrangement_id      
      AND NOT (Type = 'FX' AND retained = 1);      
      
    SELECT @Gross_This_Reserve = @Total_reserve - @Gross_Reserve_to_date;      
    SELECT @Gross_This_Payment = @Total_Payment - @Gross_Payment_to_date;      
    
      
    -- CHECK EXTENDED LIMITS      
    SELECT      
        @extended_limit_Enabled = ISNULL(Is_extended_limit_applied, 0),      
        @extended_limit_amount = ISNULL(ra.Extended_limit_amount, 0),      
        @ri_band_id = cra.ri_band_id      
    FROM RI_Arrangement ra      
    INNER JOIN Claim_RI_Arrangement cra ON ra.ri_arrangement_id = cra.original_ri_arrangement_id      
 WHERE cra.claim_ri_arrangement_id = @ri_arrangement_id      
      AND cra.claim_id = @claim_id;      
      
    -- CHECK FOR PORTFOLIO TRANSFER      
    SET @IsPortfolioTransferred = CASE      
        WHEN EXISTS (      
            SELECT 1      
            FROM Claim_pt_log CPT      
            INNER JOIN claim clm ON CPT.base_claim_id = clm.base_claim_id      
            WHERE clm.Claim_id = @claim_id      
        ) THEN 1      
        ELSE 0      
    END;      
      
    -- FOR IS_OBLIGATORY = 1      
        SELECT @Gross_SumInsured = ISNULL(Sum_insured, 0)      
        FROM Claim_ri_Arrangement      
        WHERE Claim_id = @Claim_id      
          AND ri_arrangement_id = @ri_arrangement_id;      
      
        UPDATE claim_ri_arrangement_line      
        SET Sum_Insured = @Gross_SumInsured * default_share_percent * 0.01      
        WHERE claim_id = @claim_id      
          AND type = 'T'      
          AND ISNULL(is_obligatory, 0) = 1      
          AND ri_arrangement_id = @ri_arrangement_id
          AND ISNULL(is_edited, 0) = 0;          
      
        SELECT @Net_SumInsured = @Gross_SumInsured - SUM(ISNULL(Sum_insured, 0))      
        FROM Claim_ri_Arrangement_line      
        WHERE ri_arrangement_id = @ri_arrangement_id      
          AND (      
                (Type IN ('F', 'FX') AND ISNULL(retained, 0) = 0)      
                OR (type = 'T' AND ISNULL(is_obligatory, 0) = 1)      
              );      
      
        -- CALCULATE THE NET SUM INSURED      
  SET @Net_SumInsured = ISNULL(@Net_SumInsured, @Gross_SumInsured);      
        SET @os_SumInsured = ISNULL(@Net_SumInsured, @Gross_SumInsured);      
  SET @running_SumInsured = @os_SumInsured      
      
        SELECT      
            @FACPTSummary = ISNULL(SUM(ISNULL(claim_incurred_to_date, 0)), 0),      
            @FACSummaryReserve =  ISNULL(SUM(ISNULL(Reserve_to_date, 0)), 0),      
            @FACSummaryPayment =  ISNULL(SUM(ISNULL(payment_to_date, 0)), 0) -      
                                  ISNULL(SUM(ISNULL(salvage_to_date, 0)), 0) -      
                                  ISNULL(SUM(ISNULL(recovery_to_date, 0)), 0)      
        FROM Claim_RI_Arrangement_Line      
        WHERE claim_id = @claim_id      
          AND ri_arrangement_id = @ri_arrangement_id      
          AND is_pt_archive = 1      
          AND type IN ('F');      
      
        -- UPDATE FOR 'T' WITH OBLIGATORY      
        UPDATE claim_ri_arrangement_line      
        SET      
            this_share_percent = CASE      
                WHEN ISNULL(@Gross_SumInsured, 0) = 0 THEN 0      
                ELSE ISNULL(Sum_Insured, 0) / @Gross_SumInsured * 100.00000000      
            END,      
            default_share_percent = CASE      
                WHEN ISNULL(@Gross_SumInsured, 0) = 0 THEN 0      
                ELSE ISNULL(Sum_Insured, 0) / @Gross_SumInsured * 100.00000000      
            END      
        WHERE claim_id = @claim_id      
          AND type = 'T'      
          AND ISNULL(is_obligatory, 0) = 1      
          AND ri_arrangement_id = @ri_arrangement_id
          AND ISNULL(is_edited, 0) = 0;    
      
        UPDATE cral      
        SET     
            this_reserve = CASE      
                WHEN @os_reserve <> 0 THEN      
                    ((@total_reserve) *    CASE WHEN ISNULL(@Gross_SumInsured, 0) = 0 THEN 0      
                  ELSE (ISNULL(Sum_Insured, 0) / @Gross_SumInsured)  END) -    
                   ISNULL(cral.Reserve, 0)      
                ELSE 0      
            END,      
            this_payment = CASE      
                WHEN @os_payment <> 0 AND @Recovery NOT IN (0, 1) THEN      
                    ((@total_payment ) *      
     CASE WHEN ISNULL(@Gross_SumInsured, 0) = 0 THEN 0      
        ELSE (ISNULL(Sum_Insured, 0) / @Gross_SumInsured)  END) -    
                    (ISNULL(cral.Payment, 0) + ISNULL(cral.recovery, 0)  +  ISNULL(cral.salvage, 0)  )    
                ELSE 0      
            END ,    
    this_salvage =   CASE      
                WHEN @os_payment <> 0  AND  @Recovery = 1 THEN      
                    ((@total_payment ) *      
     CASE WHEN ISNULL(@Gross_SumInsured, 0) = 0 THEN 0      
        ELSE (ISNULL(Sum_Insured, 0) / @Gross_SumInsured)  END) -    
                    (ISNULL(cral.Payment, 0) + ISNULL(cral.recovery, 0)  +  ISNULL(cral.salvage, 0)  )    
                ELSE 0      
            END ,    
    this_recovery = CASE      
                WHEN @os_payment <> 0 AND  @Recovery = 0 THEN      
                    ((@total_payment ) *      
     CASE WHEN ISNULL(@Gross_SumInsured, 0) = 0 THEN 0      
        ELSE (ISNULL(Sum_Insured, 0) / @Gross_SumInsured)  END) -    
                    (ISNULL(cral.Payment, 0) + ISNULL(cral.recovery, 0)  +  ISNULL(cral.salvage, 0)  )     
                ELSE 0      
            END     
             
        FROM claim_ri_arrangement_line cral      
        INNER JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id      
        WHERE cral.claim_id = @claim_id      
          AND cral.ri_arrangement_id = @ri_arrangement_id      
          AND cral.type = 'T'      
          AND ISNULL(cral.Is_Obligatory, 0) = 1;      
      
        SELECT   
      @obligatory_SI = SUM(cral.sum_insured),  
            @obligatory_reserve = SUM(cral.reserve) + SUM(cral.this_reserve),      
            @obligatory_payment = SUM(cral.payment)  + SUM(cral.this_payment) +     
          SUM(cral.recovery)  + SUM(cral.this_recovery) +    
          SUM(cral.salvage)  + SUM(cral.this_salvage)    
   FROM claim_ri_arrangement_line cral      
        INNER JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id      
        WHERE cral.claim_id = @claim_id      
          AND cral.ri_arrangement_id = @ri_arrangement_id      
          AND cral.type = 'T'      
          AND ISNULL(cral.Is_Obligatory, 0) = 1;      
      
        -- UPDATE 'F' TYPE LINES     
  SET @FACSumInsured = ISNULL(@Gross_SumInsured,0) - ISNULL(@obligatory_SI,0)  
        UPDATE cral      
        SET       
      this_share_percent =  ( CASE WHEN ISNULL(@FACSumInsured, 0) = 0 THEN 0      
                  ELSE ((ISNULL(Sum_Insured, 0) / @FACSumInsured) *100)  END ),   
            this_reserve = CASE      
                WHEN @os_reserve <> 0 THEN      
                    ((@total_reserve - ISNULL(@obligatory_reserve, 0) ) *      
               CASE WHEN ISNULL(@FACSumInsured, 0) = 0 THEN 0      
     ELSE (ISNULL(Sum_Insured, 0) / @FACSumInsured)  END) -    
                    ISNULL(cral.Reserve, 0)      
                ELSE 0      
            END,      
            this_payment = CASE      
                WHEN @os_payment <> 0 and @Recovery NOT IN (0, 1) THEN      
                    ((@total_payment - ISNULL(@obligatory_payment, 0)   ) *      
                   CASE WHEN ISNULL(@FACSumInsured, 0) = 0 THEN 0      
                   ELSE (ISNULL(Sum_Insured, 0) / @FACSumInsured)  END) -    
                    (ISNULL(cral.Payment, 0) + ISNULL(cral.recovery, 0)  +  ISNULL(cral.salvage, 0)  )      
                ELSE 0      
            END  ,    
     this_recovery= CASE      
                WHEN @os_payment <> 0 and @Recovery  = 0 THEN      
                    ((@total_payment - ISNULL(@obligatory_payment, 0)) *      
     CASE WHEN ISNULL(@FACSumInsured, 0) = 0 THEN 0      
     ELSE (ISNULL(Sum_Insured, 0) / @FACSumInsured)  END) -    
                    (ISNULL(cral.Payment, 0) + ISNULL(cral.recovery, 0)  +  ISNULL(cral.salvage, 0)  )     
                ELSE 0      
            END  ,    
       this_salvage = CASE      
                WHEN @os_payment <> 0 and @Recovery  = 1 THEN      
                    ((@total_payment - ISNULL(@obligatory_payment, 0) ) *      
       CASE WHEN ISNULL(@FACSumInsured, 0) = 0 THEN 0      
       ELSE (ISNULL(Sum_Insured, 0) / @FACSumInsured)  END) -    
                    (ISNULL(cral.Payment, 0) + ISNULL(cral.recovery, 0)  +  ISNULL(cral.salvage, 0)  )     
                ELSE 0      
            END      
    
    
        FROM claim_ri_arrangement_line cral      
        WHERE cral.claim_id = @claim_id      
          AND cral.ri_arrangement_id = @ri_arrangement_id      
          AND cral.type = 'F'      
          AND ISNULL(cral.retained, 0) = 0;      
      
        -- UPDATE 'FX' type lines     
        UPDATE cral      
        SET      
            this_reserve = CASE      
                WHEN @os_reserve - ISNULL(@obligatory_reserve,0) > cral.lower_limit THEN      
                    CASE      
                        WHEN @os_reserve - ISNULL(@obligatory_reserve,0) > cral.line_limit THEN      
                            CASE      
                                WHEN cral.Reserve >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0) - cral.Reserve      
                            END      
                        ELSE      
                      CASE      
                                WHEN cral.Reserve >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((@os_reserve - ISNULL(@obligatory_reserve,0) - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0 )- cral.Reserve      
                            END      
                    END      
                ELSE 0      
            END,      
            this_payment =  CASE WHEN @Recovery NOT IN (0,1) THEN     
   CASE    
                WHEN @os_payment - ISNULL(@obligatory_payment,0) > cral.lower_limit THEN      
                    CASE      
                        WHEN @os_payment - ISNULL(@obligatory_payment,0)> cral.line_limit THEN      
                            CASE      
                                WHEN (cral.Payment + cral.salvage + cral.recovery) >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0 )    
        -(cral.Payment + cral.salvage + cral.recovery)     
                            END      
                        ELSE      
                            CASE      
                                WHEN (cral.Payment + cral.salvage + cral.recovery) >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((@os_payment  - ISNULL(@obligatory_payment,0)- cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0)     
        -(cral.Payment + cral.salvage + cral.recovery)     
                       END      
                    END      
                ELSE 0      
            END    
   ELSE 0 END,    
    this_recovery =   CASE WHEN @Recovery = 0 THEN     
    CASE      
                WHEN @os_payment - ISNULL(@obligatory_payment,0) > cral.lower_limit THEN      
                    CASE      
                        WHEN @os_payment - ISNULL(@obligatory_payment,0)> cral.line_limit THEN      
                            CASE      
                                WHEN (cral.Payment + cral.salvage + cral.recovery) >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0 )    
        -(cral.Payment + cral.salvage + cral.recovery)     
                            END      
                        ELSE      
                            CASE      
                                WHEN (cral.Payment + cral.salvage + cral.recovery) >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((@os_payment  - ISNULL(@obligatory_payment,0)- cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0)    
        -(cral.Payment + cral.salvage + cral.recovery)     
                       END      
                    END      
                ELSE 0      
            END      
   ELSE    
      0    
   END,    
   this_salvage =   CASE WHEN @Recovery = 1 THEN     
    CASE      
                WHEN @os_payment - ISNULL(@obligatory_payment,0) > cral.lower_limit THEN      
                    CASE      
                        WHEN @os_payment - ISNULL(@obligatory_payment,0)> cral.line_limit THEN      
                            CASE      
                                WHEN (cral.Payment + cral.salvage + cral.recovery) >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0 )    
        -(cral.Payment + cral.salvage + cral.recovery)     
                            END      
                        ELSE      
                            CASE      
                                WHEN (cral.Payment + cral.salvage + cral.recovery) >= (cral.line_limit - cral.lower_limit) * ISNULL(cral.Participation_Percent, 100) / 100.0 THEN 0      
                                ELSE (((@os_payment  - ISNULL(@obligatory_payment,0)- cral.lower_limit) * ISNULL(cral.Participation_Percent, 100)) / 100.0)     
        -(cral.Payment + cral.salvage + cral.recovery)     
                       END      
                    END      
                ELSE 0      
            END      
   ELSE    
      0    
   END    
        FROM claim_ri_arrangement_line cral      
        WHERE cral.claim_id = @claim_id      
          AND cral.ri_arrangement_id = @ri_arrangement_id      
          AND cral.type = 'FX'      
          AND ISNULL(cral.retained, 0) = 0;      
      
        -- CALCULATE GROSS NET RESERVE AND PAYMENT      
        SELECT      
            @FACTotalReserve = SUM(ISNULL(this_reserve, 0)) + SUM(ISNULL(reserve, 0)),      
            @FACTotalPayment = SUM(ISNULL(this_payment, 0)) + SUM(ISNULL(payment, 0)) +    
          SUM(ISNULL(this_recovery, 0)) + SUM(ISNULL(recovery, 0)) +    
          SUM(ISNULL(this_salvage, 0)) + SUM(ISNULL(salvage, 0))     
        FROM Claim_ri_Arrangement_line      
        WHERE ri_arrangement_id = @ri_arrangement_id      
          AND (      
                (Type IN ('F', 'FX') AND ISNULL(retained, 0) = 0)      
                OR (Type = 'T' AND ISNULL(is_obligatory, 0) = 1)      
              );      
      
        -- FINAL GROSS NET RESERVE AND PAYMENT      
        SET @Gross_Net_reserve = ISNULL(@total_reserve,0) -      
            (CASE WHEN @IsPortfolioTransferred = 1 THEN ISNULL(@FACSummaryReserve, 0) ELSE 0 END)   -    
            ISNULL(@FACTotalReserve, 0);      
        SET @Gross_Net_Payment = ISNULL(@total_payment,0) -      
            (CASE WHEN @IsPortfolioTransferred = 1 THEN ISNULL(@FACSummaryPayment, 0) ELSE 0 END) -      
            ISNULL(@FACTotalPayment, 0);      
      
  IF @Gross_This_Reserve = 0 AND @Gross_this_payment = 0     AND ISNULL(@Gross_Net_Payment, 0) = 0 AND ISNULL(@Gross_Net_Reserve, 0) = 0     RETURN;  
    -- GET THE RET LINE ID TO UPDATE THE REMAINING RESERVE AND PAYMENT TO RET LINE      
    SELECT  @ret_line_id = ri_arrangement_line_id      
    FROM    claim_ri_arrangement_line      
    WHERE   ri_arrangement_id = @ri_arrangement_id    and claim_id = @claim_id      
    AND     type In ('R')      
      
        DECLARE      
            @last_priority INT,      
            @QsTotal NUMERIC(19, 5),      
            @number_of_lines FLOAT,      
            @priority_si MONEY,      
            @priority_limit MONEY,      
            @first_priority INT,      
            @ParticipationPercent FLOAT;      
      
        -- Set default values      
        SELECT      
            @last_priority = -666,      
            @priority_si = 0;      
      
        SELECT      
            @QSReserve = ISNULL(SUM(CASE WHEN cral.type IN ('T', 'TFS') THEN cral.Reserve END), 0),      
            @QSPayment = ISNULL(SUM(CASE      
                          WHEN cral.type IN ('T', 'TFS') THEN   
      ISNULL(cral.Payment, 0) + ISNULL(cral.salvage, 0) + ISNULL(cral.recovery, 0)  END), 0)      
        FROM claim_ri_arrangement_line cral      
        LEFT JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id      
        WHERE cral.claim_id = @claim_id      
          AND cral.ri_arrangement_id = @ri_arrangement_id      
          AND ISNULL(cral.is_obligatory, 0) = 0      
         --AND ISNULL(cral.manually_added, 0) = 0      
          AND cral.Type IN ('R', 'T', 'TX', 'TC', 'TFS', 'PX');      
      
        DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR      
        SELECT      
            cral.ri_arrangement_line_id,      
            cral.type,      
            ISNULL(cral.lower_limit, 0),      
            ISNULL(cral.line_limit, 0),      
            ISNULL(cral.Reserve, 0),    
   ISNULL(cral.Payment, 0) + ISNULL(cral.salvage, 0) + ISNULL(cral.recovery, 0),      
   CASE WHEN cral.type in  ('TC','PX','TX') OR ISNULL(rml.Treaty_Type_id, 0) = 2 THEN ROUND(ISNULL(NULLIF(rml.ceding_rate , 0), cral.default_share_percent),4) 
		WHEN ISNULL(rml.Treaty_Type_id, 0) = 1 THEN ROUND(ISNULL(NULLIF(rml.share_percent, 0), cral.this_share_percent),4) 
		ELSE ROUND(ISNULL(cral.default_share_percent,0),4) END default_share_percent,      
             ROUND(ISNULL(NULLIF(cral.this_share_percent, 0), cral.default_share_percent),4),  
            CASE WHEN ISNULL(cral.manually_added,0) = 1 AND cral.type = 'T' THEN 1  
                 WHEN ISNULL(cral.manually_added,0) = 1 AND cral.type IN ('TX','PX','TC') THEN 2  
                 WHEN cral.type IN ('TX','PX','TC') THEN 2 ELSE  ISNULL(rml.treaty_type_id, 1) END treaty_type_id,      
            ISNULL(rml.number_of_lines, 1),      
            cral.priority,      
            rml.ri_model_line_id, ISNULL(cral.manually_added,0), cral.sum_insured,
            ISNULL(t.reinsurance_type_id, 0), ISNULL(cral.is_edited,0)
        FROM claim_ri_arrangement_line cral      
        LEFT JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id
        LEFT JOIN treaty t ON cral.treaty_id = t.treaty_id
        WHERE cral.claim_id = @claim_id      
          AND cral.ri_arrangement_id = @ri_arrangement_id      
          AND ISNULL(cral.is_obligatory, 0) = 0      
          AND cral.Type IN ('R', 'T', 'TX', 'TC', 'TFS', 'PX')   
		  --and ISNULL(is_pt_archive ,0) = 0
        ORDER BY cral.priority ASC, ISNULL(rml.treaty_type_id, 1) ASC, cral.line_limit DESC;      
      
        OPEN RI_Cursor;      
        FETCH NEXT FROM RI_Cursor INTO      
            @line_id, @ri_type, @lower_limit, @line_limit, @Reserve, @Payment,      
            @default_share_percent, @this_share_percent, @treaty_type_id,      
            @number_of_lines, @priority, @ri_model_line_id,@manually_added_treaty,@SI_Manually_added_treaty,
            @reinsurance_type_id, @is_edited_line;      
      
       SELECT @first_priority = @priority;      
       SELECT @priority_si = @os_SumInsured;      
       SELECT @Running_Reserve = @Gross_Net_Reserve;      
       SELECT @Running_Payment = @Gross_Net_Payment;    
       
        WHILE @@FETCH_STATUS = 0      
        BEGIN      

    -- Skip QSR nodes (reinsurance_type_id=14) - handled by post-loop QSR split
    IF @reinsurance_type_id = 14
    BEGIN
      FETCH NEXT FROM RI_Cursor INTO
          @line_id, @ri_type, @lower_limit, @line_limit, @Reserve, @Payment,
          @default_share_percent, @this_share_percent, @treaty_type_id,
          @number_of_lines, @priority, @ri_model_line_id,@manually_added_treaty,@SI_Manually_added_treaty,
          @reinsurance_type_id, @is_edited_line;
      CONTINUE
    END
       
    SET @is_variableQuotaShare = 0;
    SELECT  @is_variableQuotaShare = IsNULL(Is_VariableQuotaShare,0)      
    FROM ri_model_line      
    WHERE    ri_model_line_id = @ri_model_line_id      
    
    IF @is_variableQuotaShare = 1    
    BEGIN     
        
   IF @ri_type = 'T'     
   BEGIN    
    --check for Quotashare to get share percent    
    EXEC spu_GetClaimRIVariableQuotaSharePercent_RI2007     
     @ri_model_line_id = @ri_model_line_id,    
     @sum_insured = @priority_si,    
     @ri_arrangement_line_id = @line_id,    
     @claim_id = @claim_id    
       
    -- Refresh @default_percent if variable quota share was applied    
    SELECT @default_share_percent = default_share_percent    
    FROM Claim_RI_Arrangement_line    
    WHERE ri_arrangement_line_id = @line_id    
    AND claim_id = @claim_id    
   END    
       
    END    
    
	IF ISNULL(@last_priority, -666) <> @priority      
     BEGIN      
    -- Update priority and get new priorities total limit      
     SELECT      
      @last_priority = @priority,      
      @priority_limit = @line_limit ,      
      @priority_si = @running_SumInsured,      
      @priority_Reserve =  @Running_Reserve ,      
      @priority_Payment =   @Running_Payment ,      
      @QsTotal = 0,      
      @QSReserve = 0,    
      @QSPayment = 0;    
      
     IF ISNULL(@line_limit, 0) = 0      
     SELECT @priority_limit = @os_SumInsured;      
      
		 IF @extended_limit_Enabled = 1      
			AND ISNULL(@extended_limit_amount, 0) > 0       
			AND @priority_limit > ISNULL(@extended_limit_amount, 0)      
		 BEGIN      
		  SET @priority_limit = @extended_limit_amount;      
		 END      
      
           SELECT @ParticipationPercent = ISNULL(Participation_Percent, 0) / 100.0      
            FROM Claim_RI_Arrangement_line      
                WHERE claim_id = @Claim_Id   AND ri_arrangement_line_id = @line_id;      
      
                 IF @ParticipationPercent = 0  SET @ParticipationPercent = 1;      
      END       -- end of last and current priority check
            SET @this_reserve = 0;      
            SET @this_payment = 0;      
            SET @this_SumInsured = 0;      
   IF @treaty_type_id = 1  -- PROPORTIONAL      
            BEGIN
            -- Guard: negative default_share_percent is invalid for proportional lines;
            -- skip allocation to prevent running totals from inflating
            IF ISNULL(@default_share_percent, 0) < 0
            BEGIN
                SET @this_SumInsured  = 0;
                SET @this_share_percent = 0;
                SET @this_reserve  = 0;
                SET @this_payment  = 0;
            END
            ELSE
            BEGIN
            SET @this_SumInsured = CASE WHEN ISNULL(@manually_added_treaty,0) = 1 OR ISNULL(@is_edited_line,0) = 1 THEN ISNULL(@SI_Manually_added_treaty,0)
            ELSE CASE WHEN @priority_si * @default_share_percent * 0.01 <= @priority_limit * @number_of_lines * @default_share_percent * 0.01      
          THEN @priority_si * @default_share_percent * 0.01      
          ELSE @priority_limit * @number_of_lines * @default_share_percent * 0.01      
           END END ;      
    --this share % for prop is always using  Net Sum Insured    
                SET @this_share_percent = CASE      
                    WHEN ISNULL(@Net_SumInsured, 0) = 0  THEN 0  ELSE ISNULL(@this_SumInsured, 0) / ISNULL(@Net_SumInsured, 0)      
                END;      
    -- clamp SI: cannot exceed or go below what remains in the running total
                -- Skip clamp for manually added treaties (ri_model_line_id IS NULL) - their SI is fixed
                IF @ri_model_line_id IS NOT NULL
                BEGIN
                    IF @running_SumInsured >= 0
                    BEGIN
                        SET @this_SumInsured = GREATEST(@this_SumInsured, 0);
                        SET @this_SumInsured = LEAST(@this_SumInsured, @running_SumInsured);
                    END
                    ELSE
                    BEGIN
                        SET @this_SumInsured = LEAST(@this_SumInsured, 0);
                        SET @this_SumInsured = GREATEST(@this_SumInsured, @running_SumInsured);
                    END
                END
                -- recalculate this_share_percent after SI clamp
                SET @this_share_percent = CASE
                    WHEN ISNULL(@Net_SumInsured, 0) = 0 THEN 0
                    ELSE ISNULL(@this_SumInsured, 0) / ISNULL(@Net_SumInsured, 0)
                END;
    -- reserve and payment use the clamped share percent against Gross_Net values
                SET @this_reserve  = ROUND((@Gross_Net_Reserve * @this_share_percent) - @Reserve, 2);
                SET @this_payment  = ROUND((@Gross_Net_Payment * @this_share_percent) - @payment, 2);
      
                IF @ri_type = 'T'      
				BEGIN      
                    SET @QsTotal = ISNULL(@QsTotal, 0) + @this_SumInsured;    
					SET @QSReserve = @QSReserve + @this_reserve;      
					SET @QSPayment = @QSPayment + @this_payment;   
                END
            END -- end negative share% guard
            END      
            ELSE IF @treaty_type_id = 2 -- NON-PROPORTIONAL      
            BEGIN      
      
                DECLARE @cede_premium_only TINYINT = 0;      
                SELECT @cede_premium_only = ISNULL(cede_premium_only, 0)      
                FROM ri_model_line      
                WHERE ri_model_line_id = @ri_model_line_id;      
      
                IF @cede_premium_only = 0 AND @ri_type <> 'TC'      
                BEGIN    
				   DECLARE @SI_Line_limit MONEY  

				    -- EXTENDED RI LIMIT IS ONLY FOR SI  
				    -- WHEN QS AND XOL ARE IN THE SAME PRIORITY
					-- THEN THE TOTAL PRIORITY LIMIT CANT BE MORE THAN QS LINE LIMIT
				   IF @priority_limit - @QsTotal < @line_limit AND @QsTotal <> 0   
				   SELECT @SI_Line_limit = @priority_limit - @QsTotal;  
			   ELSE   
			   BEGIN  
					SET  @SI_Line_limit = @line_limit  
			   END  

			SET @this_SumInsured = CASE WHEN ISNULL(@manually_added_treaty,0) = 1 OR ISNULL(@is_edited_line,0) = 1 THEN ISNULL(@SI_Manually_added_treaty,0)
			ELSE CASE  WHEN @priority_si - @QsTotal > @lower_limit THEN      
			CASE  WHEN @priority_si - @QsTotal > @SI_Line_limit THEN GREATEST( @SI_Line_limit - @lower_limit,0) 
			 ELSE GREATEST(@priority_si - @lower_limit - @QsTotal, 0)  END ELSE 0      
			END END;   
     END   
     IF @ri_type <> 'TC'  
     BEGIN  
      -- XOL Reserve calculation
      SET @this_reserve = CASE WHEN @priority_Reserve > @lower_limit THEN   
          CASE WHEN @priority_Reserve > @line_limit   
           THEN GREATEST(@line_limit - @lower_limit - @Reserve, 0)     
           ELSE GREATEST(@priority_Reserve - @lower_limit - @Reserve, 0) END ELSE 0 END;    
                  
    SET @this_reserve = @this_reserve * @ParticipationPercent;      
                -- THIS% NOT REQUIRED FOR XOLS  
    SET @this_share_percent = 0;      
    
                SET @this_payment = CASE WHEN @priority_Payment > @lower_limit THEN      
           CASE WHEN @priority_Payment > @line_limit   
            THEN GREATEST(@line_limit - @lower_limit - @Payment, 0)   
            ELSE GREATEST(@priority_Payment - @lower_limit - @Payment, 0)  END  ELSE 0 END;    
     END  
END    
	 -- PBI 26251: Apply reinstatement capacity cap for TX after layer calc
            IF @ri_type = 'TX'
            BEGIN
                EXEC spu_calculate_claims_ri_reinstatement_TX_RI2007
                   @claim_id = @claim_id,
                   @ri_arrangement_id = @ri_arrangement_id,
                    @line_id = @line_id,
                    @ri_model_line_id = @ri_model_line_id,
                    @this_reserve = @this_reserve OUTPUT,
                    @this_payment = @this_payment OUTPUT
            END

		 --End of Reinstatement
		 --UPDATE RI ARRANGEMENT LINES  
            UPDATE claim_ri_arrangement_line      
            SET      
                Sum_Insured = CASE WHEN ISNULL(manually_added,0) = 1 OR ISNULL(is_edited,0) = 1 THEN sum_insured ELSE ISNULL(@this_SumInsured, 0) END,      
                this_reserve = ISNULL(@this_reserve, 0),      
    default_share_percent = ISNULL(@default_share_percent,0),      
                this_salvage = CASE      
                    WHEN @Recovery = 1 THEN ISNULL(@this_payment, 0)      
                    ELSE ISNULL(this_salvage, 0)      
                END,      
                this_recovery = CASE      
                    WHEN @Recovery = 0 THEN ISNULL(@this_payment, 0)      
                    ELSE ISNULL(this_recovery, 0)      
                END,      
                this_payment = CASE      
                    WHEN @Recovery NOT IN (0, 1) THEN ISNULL(@this_payment, 0)      
                    ELSE ISNULL(this_payment, 0)      
                END,      
                this_share_percent = ISNULL(@this_share_percent, 0) * 100      
				WHERE claim_id = @claim_id      
				AND ri_arrangement_line_id = @line_id;      
				-- UPDATE THE RUNNING VALUES TO FURTHER ALLOCATE
				SET @running_SumInsured = @running_SumInsured - @this_SumInsured;
				SET @Running_Reserve = @Running_Reserve - @this_reserve - @Reserve;
				SET @Running_Payment = @Running_Payment - @this_payment - @Payment;      
      
      
            FETCH NEXT FROM RI_Cursor INTO      
                @line_id, @ri_type, @lower_limit, @line_limit, @Reserve, @Payment,      
                @default_share_percent, @this_share_percent, @treaty_type_id,      
                @number_of_lines, @priority, @ri_model_line_id,@manually_added_treaty,@SI_Manually_added_treaty,
                @reinsurance_type_id, @is_edited_line;
        END      
      
    --UPDATE RETENTION WITH REMAINING RESERVE /PAYMENT VALUES
     UPDATE Claim_RI_Arrangement_Line
     SET this_reserve = ISNULL(this_reserve,0) + CASE WHEN @Running_Reserve <> 0 THEN ISNULL(@Running_Reserve,0) ELSE 0 END,
     this_payment = ISNULL(this_payment,0) + CASE WHEN @Running_Payment <> 0 AND @Recovery NOT IN (0,1) THEN ISNULL(@Running_Payment,0) ELSE 0 END,
     this_recovery = ISNULL(this_recovery,0) + CASE WHEN @Running_Payment <> 0 AND @Recovery = 0 THEN ISNULL(@Running_Payment,0) ELSE 0 END,
     this_salvage = ISNULL(this_salvage,0) + CASE WHEN @Running_Payment <> 0 AND @Recovery = 1 THEN ISNULL(@Running_Payment,0) ELSE 0 END
     WHERE claim_id = @claim_id AND ri_arrangement_line_id = @ret_line_id;      
      
        CLOSE RI_Cursor;      
        DEALLOCATE RI_Cursor;

    -- QSR SPLIT: Split R's SI and reserve/payment among QSR nodes (reinsurance_type_id=14)
    IF EXISTS (SELECT 1 FROM claim_ri_arrangement_line cral
               INNER JOIN treaty t ON cral.treaty_id = t.treaty_id
               WHERE cral.claim_id = @claim_id
               AND cral.ri_arrangement_id = @ri_arrangement_id
               AND t.reinsurance_type_id = 14)
    BEGIN
      DECLARE @ret_si NUMERIC(19,5), @ret_reserve MONEY, @ret_payment MONEY
      DECLARE @total_qsr_si NUMERIC(19,5) = 0, @total_qsr_reserve MONEY = 0, @total_qsr_payment MONEY = 0
      DECLARE @qsr_line_id INT, @qsr_default_perc FLOAT, @qsr_is_edited BIT
      DECLARE @qsr_si NUMERIC(19,5), @qsr_reserve MONEY, @qsr_payment MONEY
      DECLARE @qsr_this_share_pct FLOAT

      SELECT @ret_si = sum_insured,
             @ret_reserve = ISNULL(reserve,0) + ISNULL(this_reserve,0),
             @ret_payment = ISNULL(payment,0) + ISNULL(this_payment,0) + ISNULL(recovery,0) + ISNULL(this_recovery,0) + ISNULL(salvage,0) + ISNULL(this_salvage,0)
      FROM claim_ri_arrangement_line WHERE ri_arrangement_line_id = @ret_line_id AND claim_id = @claim_id

      DECLARE QSR_Cursor CURSOR FAST_FORWARD FOR
        SELECT cral.ri_arrangement_line_id, ISNULL(rml.share_percent, cral.default_share_percent), ISNULL(cral.is_edited, 0)
        FROM claim_ri_arrangement_line cral
        INNER JOIN treaty t ON cral.treaty_id = t.treaty_id
        LEFT JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id
        WHERE cral.claim_id = @claim_id
          AND cral.ri_arrangement_id = @ri_arrangement_id
          AND t.reinsurance_type_id = 14

      OPEN QSR_Cursor
      FETCH NEXT FROM QSR_Cursor INTO @qsr_line_id, @qsr_default_perc, @qsr_is_edited

      WHILE @@FETCH_STATUS = 0
      BEGIN
        IF @qsr_is_edited = 1
        BEGIN
          SELECT @qsr_si = ABS(ISNULL(sum_insured, 0)),
                 @qsr_reserve = ISNULL(reserve,0) + ISNULL(this_reserve,0),
                 @qsr_payment = ISNULL(payment,0) + ISNULL(this_payment,0) + ISNULL(recovery,0) + ISNULL(this_recovery,0) + ISNULL(salvage,0) + ISNULL(this_salvage,0)
          FROM claim_ri_arrangement_line WHERE ri_arrangement_line_id = @qsr_line_id AND claim_id = @claim_id
        END
        ELSE
        BEGIN
          SET @qsr_si = ROUND(ABS(@ret_si) * @qsr_default_perc / 100, 2)
          SET @qsr_this_share_pct = CASE WHEN ISNULL(@Net_SumInsured, 0) = 0 THEN 0 ELSE @qsr_si / @Net_SumInsured END
          SET @qsr_reserve = ROUND(@ret_reserve * @qsr_default_perc / 100, 2)
          SET @qsr_payment = ROUND(@ret_payment * @qsr_default_perc / 100, 2)

          DECLARE @qsr_existing_reserve MONEY, @qsr_existing_payment MONEY
          SELECT @qsr_existing_reserve = ISNULL(reserve, 0),
                 @qsr_existing_payment = ISNULL(payment, 0) + ISNULL(salvage, 0) + ISNULL(recovery, 0)
          FROM claim_ri_arrangement_line WHERE ri_arrangement_line_id = @qsr_line_id AND claim_id = @claim_id

          UPDATE claim_ri_arrangement_line
          SET sum_insured = @qsr_si,
              default_share_percent = @qsr_default_perc,
              this_share_percent = @qsr_this_share_pct * 100,
              this_reserve = @qsr_reserve - @qsr_existing_reserve,
              this_payment = CASE WHEN @Recovery NOT IN (0,1) THEN @qsr_payment - @qsr_existing_payment ELSE ISNULL(this_payment,0) END,
              this_recovery = CASE WHEN @Recovery = 0 THEN @qsr_payment - @qsr_existing_payment ELSE ISNULL(this_recovery,0) END,
              this_salvage = CASE WHEN @Recovery = 1 THEN @qsr_payment - @qsr_existing_payment ELSE ISNULL(this_salvage,0) END
          WHERE ri_arrangement_line_id = @qsr_line_id AND claim_id = @claim_id
        END

        SET @total_qsr_si = @total_qsr_si + @qsr_si
        SET @total_qsr_reserve = @total_qsr_reserve + @qsr_reserve
        SET @total_qsr_payment = @total_qsr_payment + @qsr_payment

        FETCH NEXT FROM QSR_Cursor INTO @qsr_line_id, @qsr_default_perc, @qsr_is_edited
      END
      CLOSE QSR_Cursor
      DEALLOCATE QSR_Cursor

      -- Reduce R by total QSR amounts (remainder approach: R = original R values - QSR totals)
      DECLARE @r_new_si NUMERIC(19,5) = ABS(@ret_si) - @total_qsr_si
      DECLARE @r_new_share_pct FLOAT = CASE WHEN ISNULL(@Net_SumInsured,0) = 0 THEN 0 ELSE @r_new_si / @Net_SumInsured END
      DECLARE @r_new_reserve MONEY = @ret_reserve - @total_qsr_reserve
      DECLARE @r_new_payment MONEY = @ret_payment - @total_qsr_payment

      DECLARE @r_existing_reserve MONEY, @r_existing_payment MONEY
      SELECT @r_existing_reserve = ISNULL(reserve, 0),
             @r_existing_payment = ISNULL(payment, 0) + ISNULL(salvage, 0) + ISNULL(recovery, 0)
      FROM claim_ri_arrangement_line WHERE ri_arrangement_line_id = @ret_line_id AND claim_id = @claim_id

      -- Preserve R SI if it was manually edited
      DECLARE @r_is_edited BIT = 0
      SELECT @r_is_edited = ISNULL(is_edited, 0) FROM claim_ri_arrangement_line
      WHERE ri_arrangement_line_id = @ret_line_id AND claim_id = @claim_id

      UPDATE claim_ri_arrangement_line
      SET sum_insured = CASE WHEN @r_is_edited = 1 THEN sum_insured ELSE @r_new_si END,
          this_share_percent = CASE WHEN @r_is_edited = 1
              THEN CASE WHEN ISNULL(@Net_SumInsured,0) = 0 THEN 0
                        ELSE sum_insured / @Net_SumInsured * 100 END
              ELSE @r_new_share_pct * 100 END,
          this_reserve = @r_new_reserve - @r_existing_reserve,
          this_payment = CASE WHEN @Recovery NOT IN (0,1) THEN @r_new_payment - @r_existing_payment ELSE ISNULL(this_payment,0) END,
          this_recovery = CASE WHEN @Recovery = 0 THEN @r_new_payment - @r_existing_payment ELSE ISNULL(this_recovery,0) END,
          this_salvage = CASE WHEN @Recovery = 1 THEN @r_new_payment - @r_existing_payment ELSE ISNULL(this_salvage,0) END
      WHERE ri_arrangement_line_id = @ret_line_id AND claim_id = @claim_id
    END
      
  -- Reserve + Payments - Salvage - Recovery    
  EXEC spu_Calculate_Claims_Incurred_to_date @claim_id,@ri_arrangement_id      
END     
GO