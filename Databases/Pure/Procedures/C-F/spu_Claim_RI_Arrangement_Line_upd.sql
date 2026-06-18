SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_upd'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_Line_upd  
	@claim_id int,  
	@ri_arrangement_line_id int,  
	@this_share_percent float,  
	@agreement_code varchar(255),  
	@this_reserve money,  
	@this_payment money,
	@Grouping integer=0,
	@Sum_insured money =0,
 	@Trans_type varchar(10)=''   
AS  
 If @Grouping =0  
BEGIN 
   Update  claim_ri_arrangement_line  
    Set     this_share_percent = @this_share_percent * 100,  
            agreement_code = @agreement_code,  
            this_reserve = @this_reserve,  
            this_payment = @this_payment  
    Where   claim_id = @claim_id  
    And     ri_arrangement_line_id = @ri_arrangement_line_id  
	IF @Sum_insured >0 
	    Update  claim_ri_arrangement_line  
	    Set    Sum_Insured =@sum_Insured   
	    Where   claim_id = @claim_id  
	    And     ri_arrangement_line_id = @ri_arrangement_line_id  
	   	
	 END 
 ELSE  
--Update the FAC XOL Line  
  
  Update claim_ri_arrangement_line  
  Set         agreement_code = @agreement_code,  
  this_reserve = @this_reserve,  
  this_payment = @this_payment  
  Where   claim_id = @claim_id  
  And Grouping= @Grouping AND ISNULL(retained,0) = 0
--Update Salvage and Third Party Values if transcation type are Salvage & Third Party
IF @Trans_type = 'C_SA'
    Update  claim_ri_arrangement_line  
    Set    This_salvage = @this_payment   
    Where   claim_id = @claim_id  
    And     ri_arrangement_line_id = @ri_arrangement_line_id  	   			
IF @Trans_type = 'C_RV'
    Update  claim_ri_arrangement_line  
    Set    This_recovery = @this_payment   
    Where   claim_id = @claim_id  
    And     ri_arrangement_line_id = @ri_arrangement_line_id  	   			


IF @Grouping > 0
BEGIN
	EXEC spu_Claim_RI_Arrangement_Line_upd_FAC_XOL @GroupingFACXOL =@Grouping,
	@claim_id = @claim_id, @ri_arrangement_line_id = @ri_arrangement_line_id , @agreement_code = @agreement_code

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
