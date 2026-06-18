EXECUTE DDLDropProcedure 'spu_PFRF_saa'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PFRF_saa
    @CompanyNo int,
    @SchemeNo int,
    @SchemeVersion int
AS BEGIN

    SELECT
        CompanyNo,
        SchemeNo,
        SchemeVersion,
        StartDate,
        ProductFamily,
        ArrangementFee,
        Mnemonic,
        EndDate,
        Protect,
        DaysDelay,
        DepositReq,
        DepositPC,
        AllowProtection,
        ProtectRate,
        MinInterest,
        Min1,
        Max1,
        Rate1,
        Min2,
        Max2,
        Rate2,
        Min3,
        Max3,
        Rate3,
        Min4,
        Max4,
        Rate4,
        Min5,
        Max5,
        Rate5,
        AlLowOveride,
        MinMTA,
        pffrequency_id,
        tax_charged_to,
        fee_type,
        fee_charged_to,
        protection_type,
        protection_charged_to,
        deposit_type,
        deposit_charged_to,
        backdated_rollup_to,
        align_to,start_limit,
        recollect_on_next,recollect_days,
        retry_limit,mta_on_next_instalment,
        existing_days_delay,
	advance_instalments,
	review_pmuser_group_id,
	remainder_amount_threshhold,
	remainder_amount_at_end,
	maximum_instalments,single_instalment_per_month,
	first_instalment_align_with_day_in_month,
	is_deposit_override_allowed
    FROM PFRF
    WHERE CompanyNo = @CompanyNo AND SchemeNo = @SchemeNo AND SchemeVersion = @SchemeVersion
    ORDER BY CompanyNo ASC, SchemeNo ASC, SchemeVersion ASC

END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

