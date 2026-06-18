SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_PFRF_sel'
GO

CREATE PROCEDURE spu_PFRF_sel 
    @PFRF_ID int  
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
    R1Com,  
    Min2,  
    Max2,  
    Rate2,  
    R2Com,  
    Min3,  
    Max3,  
    Rate3,  
    R3Com,  
    Min4,  
    Max4,  
    Rate4,  
    R4Com,  
    Min5,  
    Max5,  
    Rate5,  
    R5Com,  
    AlLowOveride,  
    MinMTA,  
    MinMTAInstalments,  
    MinInterest,  
    pffrequency_id,  
    tax_charged_to,  
    fee_type,  
    fee_charged_to,  
    protection_type,  
    protection_charged_to,  
    deposit_type,  
    deposit_charged_to,  
    backdated_rollup_to,  
    align_to,  
    start_limit,  
    recollect_on_next,  
    recollect_days,  
    retry_limit,  
    mta_on_next_instalment,  
    existing_days_delay,  
    statement_pffrequency_id,  
    statement_report_id,  
    advance_instalments,  
    review_pmuser_group_id,  
    remainder_amount_threshhold,  
    remainder_amount_at_end,  
    maximum_instalments,  
    finance_net_commission,single_instalment_per_month,
	first_instalment_align_with_day_in_month,
	is_deposit_override_allowed ,
	apply_fee_percentages_to_fees,
	apply_fee_percentages_to_taxes
FROM PFRF  
WHERE  
    PFRF_ID = @PFRF_ID  
  
END  
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO