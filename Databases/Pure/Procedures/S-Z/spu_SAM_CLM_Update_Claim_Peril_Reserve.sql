SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Update_Claim_Peril_Reserve'
GO

CREATE PROCEDURE spu_SAM_CLM_Update_Claim_Peril_Reserve  
  
@claim_peril_id integer,  
@type_code varchar(50),  
@revision_amount money,  
@transaction_type varchar(10), 
@grossReserve money,
@tax money,
@revisedGrossReserve money = 0,
@revisedTaxReserve money = 0,
@reserve_id integer OUTPUT  
  
AS  
  
BEGIN  
  
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
     ((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) + @revision_amount) / ISNULL(sum_insured,0)) * 100  
    ELSE 0  
    END  ,
	Gross_Reserve = @grossReserve,
	tax = @tax,
	Revised_Gross_reserve = @revisedGrossReserve,
	Revised_Tax_reserve = @revisedTaxReserve
   WHERE reserve_id = @reserve_id  
  END  
  
 ELSE  
  BEGIN  
   UPDATE reserve SET revised_reserve = revised_reserve + @revision_amount,  
   this_revision = @revision_amount,  
   revised_reserve_entered = 0,  
   revision_count = revision_count + 1,
   average =  CASE  
    WHEN ISNULL(sum_insured,0) <> 0 THEN  
     ((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) + @revision_amount) / ISNULL(sum_insured,0)) * 100  
    ELSE 0  
    END  ,
	Gross_Reserve = @grossReserve,
	tax = @tax,
	Revised_Gross_reserve = @revisedGrossReserve,
	Revised_Tax_reserve = @revisedTaxReserve
  WHERE reserve_id = @reserve_id  
 END  
  
END  





GO
