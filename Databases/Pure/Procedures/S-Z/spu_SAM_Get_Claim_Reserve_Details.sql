SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Reserve_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Reserve_Details  
  
@claim_id integer  
  
AS  
  SELECT  
  
  r.Reserve_id,  
  r.claim_Peril_id,  
  r.Reserve_type_id,  
  rt.description as reserve_type_description,  
  r.Initial_reserve,  
  r.Paid_to_date,  
  r.Revised_reserve,  
  r.Sum_insured,  
  r.Revision_count,  
  r.Average,  
  r.this_revision,  
  r.this_payment,  
  r.Revised_Reserve_Entered,  
  r.base_reserve_id,  
  r.version_id,  
  rt.name as reserve_type_code,  
  rt.is_excess,
  rt.is_indemnity,
  rt.is_expense,
  rt.Description,
  r.Gross_Reserve,
  r.tax,
  r.Revised_Gross_Reserve,
  r.Revised_Tax_Reserve,
  r.paid_to_date_tax
  
 FROM reserve r  
  
  LEFT JOIN reserve_Type rt ON  
   rt.reserve_type_id = r.reserve_type_id  
  
 WHERE claim_peril_id in (select claim_peril_id from claim_peril where claim_id = @claim_id)  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
