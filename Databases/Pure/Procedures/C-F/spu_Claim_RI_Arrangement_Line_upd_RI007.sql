SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_upd_RI007'
GO
CREATE PROCEDURE spu_Claim_RI_Arrangement_Line_upd_RI007  
    @claim_id int,  
    @ri_arrangement_line_id int,  
    @this_share_percent float,  
    @agreement_code varchar(255),  
    @lower_limit money = 0,        
    @line_limit money = 0,        
    @retained float = 0,        
    @default_share_percent float = 0,  
    @participation_percent float = 0,
    @sum_insured money    	  
AS  
  
    Update  claim_ri_arrangement_line  
    Set     this_share_percent = @this_share_percent * 100,  
            agreement_code = @agreement_code,  
   	    lower_limit = @lower_limit,        
            line_limit = @line_limit,        
            retained = @retained,        
            default_share_percent = @default_share_percent   * 100,  
            participation_percent = @participation_percent,
	    sum_insured = @sum_insured       
    Where   claim_id = @claim_id  
    And     ri_arrangement_line_id = @ri_arrangement_line_id  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

