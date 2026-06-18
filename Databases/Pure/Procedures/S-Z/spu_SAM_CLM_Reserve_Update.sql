SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Reserve_Update'
GO

CREATE PROCEDURE spu_SAM_CLM_Reserve_Update  
  
@claim_peril_id integer,  
@type_code varchar(50),  
@revision_amount money,  
@transaction_type varchar(10),  
@grossReserve money=0,
@tax money=0,
@revisedGrossReserve money=0, 
@revisedTaxReserve money=0,
@reserve_id integer OUTPUT  
  
AS  
  
BEGIN  
  declare @ClaimsReservesareGross int =0

 if exists (select null from system_options where description ='Claims Reserves are Gross' AND value = 1)
 BEGIN
 set @ClaimsReservesareGross =1
 end
 SELECT @reserve_id = reserve_id  
 FROM reserve  
 WHERE claim_peril_id = @claim_peril_id  
 AND reserve_type_id in (  
  SELECT reserve_type_id  
  FROM reserve_type  
  WHERE name = @type_code)  
  
 IF @transaction_type = 'C_CO'  
  BEGIN  
   UPDATE reserve SET initial_reserve = @revision_amount,  
   this_revision = @revision_amount,  
   revised_reserve_entered = 0,  
   average =  CASE  
    WHEN ISNULL(sum_insured,0) <> 0 THEN  
     ((ISNULL(revised_reserve,0) + @revision_amount) / ISNULL(sum_insured,0)) * 100
    ELSE 0  
    END,
	Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else  @grossReserve end,  
  tax = case when @ClaimsReservesareGross =0 then 0 else @tax end,  
  Revised_Gross_Reserve =case when @ClaimsReservesareGross =0 then 0 else  @grossReserve end,  
  Revised_Tax_Reserve = case when @ClaimsReservesareGross =0 then 0 else @tax end 
   WHERE reserve_id = @reserve_id  
  END  
  
 ELSE  
  BEGIN  
   UPDATE reserve SET revised_reserve = revised_reserve - this_revision + @revision_amount,  
   this_revision = @revision_amount,  
   revised_reserve_entered = 0,  
   average =  CASE  
    WHEN ISNULL(sum_insured,0) <> 0 THEN  
     ((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) - ISNULL(this_revision,0) + @revision_amount) / ISNULL(sum_insured,0)) * 100  
    ELSE 0  
    END,
	Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else @grossReserve end,  
  tax = case when @ClaimsReservesareGross =0 then 0 else @tax end,  
  Revised_Gross_Reserve = case when @ClaimsReservesareGross =0 then 0 else @revisedGrossReserve end,  
  Revised_Tax_Reserve =  case when @ClaimsReservesareGross =0 then 0 else @revisedTaxReserve end  
  WHERE reserve_id = @reserve_id  
 END  
  
END  
GO