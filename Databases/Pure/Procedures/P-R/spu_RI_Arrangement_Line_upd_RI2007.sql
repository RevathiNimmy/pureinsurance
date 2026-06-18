SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Arrangement_Line_upd_RI2007'
GO


 CREATE PROCEDURE spu_RI_Arrangement_Line_upd_RI2007      
    @ri_arrangement_line_id int,      
    @this_share_percent float,      
    @premium_percent float,      
    @commission_percent float,      
    @agreement_code varchar(255),      
    @sum_insured money,      
    @premium_value money,      
    @commission_value money,      
    @premium_tax money,      
    @commission_tax money,      
    @is_commission_modified tinyint,      
    @lower_limit money = 0,      
    @line_limit money = 0,      
    @retained_percentage float = 0,      
    @default_share_percent float = 0,
    @participation_percent float = 0  ,
	@fac_prop_premium_percent float = 0 ,
	@manually_added bit = 0,
	@is_edited bit = 0,
	@is_premium_edited bit = 0
AS          
    UPDATE  ri_arrangement_line      
    SET     this_share_percent = @this_share_percent * 100,      
   premium_percent = @premium_percent * 100,      
   commission_percent = @commission_percent * 100,      
   agreement_code = @agreement_code,      
   sum_insured = @sum_insured,      
   premium_value = @premium_value,      
   commission_value = @commission_value,      
   premium_tax = @premium_tax,      
   commission_tax = @commission_tax,      
   is_commission_modified = @is_commission_modified,      
   lower_limit = @lower_limit,      
   line_limit = @line_limit,      
   retained = @retained_percentage,      
   default_share_percent = @default_share_percent   * 100,
   participation_percent = @participation_percent ,
   FACPropPremiumPerc = @fac_prop_premium_percent * 100,
   manually_added = @manually_added,
   is_edited = @is_edited,
   is_premium_edited = @is_premium_edited
 WHERE   ri_arrangement_line_id = @ri_arrangement_line_id   

GO  
