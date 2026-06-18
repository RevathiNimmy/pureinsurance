SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI_Arrangement_make'
GO


CREATE PROCEDURE spu_RI_Arrangement_make
    @ri_arrangement_id int,
    @risk_type_id integer,
    @ri_band_id int,
    @effective_date datetime,
    @allow_deferred tinyint,
    @sum_insured money,
    @premium money,
    @line_limit money,
    @is_auto_reinsured tinyint,
    @source_id int,
    @policy_currency_id smallint,
    @policy_currency_rate float,
       @transtype varchar(10) = ''
AS

    Declare
        @model_currency_id smallint,
        @model_currency_rate float,
        @ri_model_id int,
        @ri_model_type int

		
      -- If this Risk is automatically reinsured
   If @is_auto_reinsured = 1
   BEGIN
   DELETE  Tax_Calculation
   WHERE ri_arrangement_line_id IN
    (SELECT ri_arrangement_line_id
     FROM  ri_arrangement_line ral
     LEFT JOIN ri_arrangement ra ON ral.ri_arrangement_id =ra.ri_arrangement_id
     WHERE  ral.ri_arrangement_id = @ri_arrangement_id
     And    ral.type <> 'F'
     And  Not (ral.type = 'T' and ISNULL(ral.line_limit,0) = 0 )
     AND ra.is_modified=0 )

     DELETE  ri_arrangement_line
     WHERE ri_arrangement_id IN
     (SELECT ral.ri_arrangement_id
      FROM  ri_arrangement_line ral
      LEFT JOIN ri_arrangement ra ON ral.ri_arrangement_id =ra.ri_arrangement_id
      WHERE  ral.ri_arrangement_id = @ri_arrangement_id
      And    ral.type <> 'F'      
      AND ra.is_modified=0 )
         And    type <> 'F'
     And  Not (type = 'T' and ISNULL(line_limit,0) = 0 and is_obligatory = 0 )
  END

   -- If we still have data in this arrangement don't update the model
    If Exists (Select * From ri_arrangement_line Where ri_arrangement_id = @ri_arrangement_id And type <> 'F' And  Not (type = 'T' and ISNULL(line_limit,0) = 0 ))
        Return

		
    -- Get the best RI Model for this ri_band and risk_type
    Select  @ri_model_id = Null
    Select  @ri_model_id = rmu.ri_model_id,
            @model_currency_id = currency_id,
            @ri_model_type = rm.ri_model_type
    From    risk_type_ri_model_usage rmu
    Join    ri_model rm
            On rm.ri_model_id = rmu.ri_model_id
    Where   rmu.risk_type_id = @risk_type_id
    And     rmu.ri_band = @ri_band_id
    And     rmu.is_deleted = 0
    And     rmu.effective_date <= @effective_date
    And    (rmu.expiry_date >= @effective_date or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')
    And    (rm.ri_model_type = 0
        Or (rm.ri_model_type = 2 And @allow_deferred = 1))
    And     rm.is_deleted = 0
    And     rm.effective_date <= @effective_date
    And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')
    Order By
            rm.ri_model_type Desc,    -- give priority to none-deferred models
            rmu.effective_date Asc   -- give priority to newer models

    IF @ri_model_type=2 BEGIN
       SET @line_limit=NULL
    END

	
    -- If model is not specified for band, check for a system default model
    If @ri_model_id Is Null
        Select  @ri_model_id = rm.ri_model_id,
                @model_currency_id = currency_id
        From    ri_model rm
        Where   rm.ri_model_type = 1 -- Default
        And     rm.is_deleted = 0
        And     rm.effective_date <= @effective_date
        And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')
        Order By
                rm.effective_date   -- give priority to newer models

       -- E007
       IF @ri_model_type =4
       BEGIN
              UPDATE  ri_arrangement
              SET     Cloned = 1
              WHERE   ri_arrangement_id = @ri_arrangement_id
       END
       ELSE
       BEGIN
              UPDATE  ri_arrangement
              SET     Cloned = 0
              WHERE   ri_arrangement_id = @ri_arrangement_id
       END
    -- Update the arrangement
    Update  ri_arrangement
    Set     ri_model_id = @ri_model_id
    Where   ri_arrangement_id = @ri_arrangement_id

    -- If different get combined rate, else set rate as 1
    If @model_currency_id <> @policy_currency_id Begin
        Execute spu_ACT_Get_Currency_Rate
        @model_currency_id,
            @source_id,
            @effective_date,
            @model_currency_rate Output

        Select @model_currency_rate = @model_currency_rate / @policy_currency_rate
    End Else
        Select @model_currency_rate = 1

		
    -- Insert the arrangement lines
    Insert Into RI_Arrangement_Line (
            ri_arrangement_id,
            type,
            treaty_id,
            default_share_percent,
            this_share_percent,
            premium_percent,
            commission_percent,
            agreement_code,
            priority,
            number_of_lines,
            line_limit,
            sum_insured,
            premium_value,
            commission_value,
           --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
            is_commission_modified,
           is_obligatory)
           --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
    Select  @ri_arrangement_id,
            Case When rt.code = 'RET' Then 'R' Else 'T' End,
            rml.treaty_id,
            rml.share_percent,
            0,
            0,
            tc.commission_percent,
            t.agreement_code,
            rml.priority,
            rml.number_of_lines,
            IsNull(@line_limit * @model_currency_rate, rml.line_limit * @model_currency_rate),
            0,
            0,
            0,
           --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
            0,
            rml.is_obligatory
           --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
    From    ri_model_line rml
    Join    Treaty t
            On t.treaty_id = rml.treaty_id
    Join    Reinsurance_type rt
            On rt.reinsurance_type_id = t.reinsurance_type_id
    Left Join
           -- Calculate a summary commission rate for each treaty
           (Select  treaty_id,
                    Sum(commission_percent * (share_percent / 100)) commission_percent
            From    treaty_party
            Group By
                    treaty_id) tc
            On tc.treaty_id = t.treaty_id
    Where   ri_model_id = @ri_model_id

	
       If ISNULL(@transtype,'') = 'MTC' BEGIN

       If NOT Exists (Select * from ri_arrangement_line Where   ri_arrangement_id = @ri_arrangement_id  And (type = 'T' and ISNULL(line_limit,0) = 0  ))
                     BEGIN
                     --Manually added T, must be carried over from original
              Insert Into RI_Arrangement_Line (
                           ri_arrangement_id,
                           type,
                           treaty_id,
                           default_share_percent,
                           this_share_percent,
                           premium_percent,
                           commission_percent,
                           agreement_code,
                           priority,
                           number_of_lines,
                           line_limit,
                           sum_insured,
                           premium_value,
                           commission_value,
                           is_commission_modified,
                           is_obligatory)
                     select
                     @ri_arrangement_id,
                     type,
                     RIL.treaty_id,
                     RIL.default_share_percent,
                     RIL.this_share_percent,
                     RIL.premium_percent,
                     RIL.commission_percent,
                     RIL.agreement_code,
                     RIL.priority,
                     RIL.number_of_lines,
                     RIL.line_limit,
                     RIL.sum_insured * (-1),
                     RIL.premium_value * (-1),
                     RIL.commission_value * (-1),
                     RIL.is_commission_modified,
                     RIL.is_obligatory
                     from RI_Arrangement(nolock) RI
                     JOIN RI_Arrangement_Line(nolock) RIL on RIL.ri_arrangement_id = RI.ri_arrangement_id
                     where RI.risk_cnt = (select risk_cnt from RI_Arrangement(nolock) where ri_arrangement_id = @ri_arrangement_id)
                     AND RI.original_flag = 1
                     AND RIL.type = 'T'
                     AND ISNULL(RIL.line_limit,0) = 0
                     AND RI.ri_band_id = ISNULL(@ri_band_id,0)

                     Order by RIL.ri_arrangement_id

                     END
       END

	   
    -- Recalculate the RI Arrangement Lines
    If @is_auto_reinsured = 1
        Execute spu_RI_Arrangement_calc
            @ri_arrangement_id = @ri_arrangement_id,
            @band_si = @sum_insured,
            @band_premium = @premium

If ISNULL(@transtype,'') = 'REN'
Update  ri_arrangement_line
        Set     premium_value = @premium*premium_percent/100 where ri_arrangement_id=@ri_arrangement_id and type='F'

GO


