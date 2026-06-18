SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_RI_Arrangement_Details_upd'
GO

CREATE Procedure spu_RI_Arrangement_Details_upd  
@risk_cnt int,  
@original_flag tinyint,  
@original_risk_cnt int  
  
As  
Begin  
  
Declare  
 @Premium float,  
 @ri_arrangement_id int,  
 @original_Ri_arrangement_id int,  
 @old_Ri_arrangement_id int,  
 @Ri_arrangement_id1 int,  
 @ri_arrangement_line_id int,  
 @type char(1),  
 @treaty_id int,  
        @party_cnt int,  
        @default_share_percent float,  
        @this_share_percent float,  
        @premium_percent float,  
        @commission_percent float,  
        @agreement_code varchar(255),  
        @priority int,  
        @number_of_lines smallint,  
        @line_limit money,  
        @sum_insured money,  
        @premium_value money,  
        @commission_value money,  
        @premium_tax money,  
        @commission_tax money,  
        @is_commission_modified tinyint,  
 	@insurance_file_cnt int,  
        @band_id int  
  
 Select @insurance_file_cnt = insurance_file_cnt   
  from insurance_file_risk_link where risk_cnt = @risk_cnt and status_flag = 'C'   
  
 DECLARE CUR_RI CURSOR FAST_FORWARD FOR  
 Select Ri_arrangement_id, premium, ri_band_id from Ri_arrangement  
 Where Risk_Cnt=@risk_cnt And Original_flag=@original_flag  
  
     OPEN CUR_RI  
     FETCH NEXT FROM CUR_RI INTO @ri_arrangement_id, @Premium, @Band_id  
  
     WHILE @@FETCH_STATUS = 0  
     BEGIN  
  
  Update Ri_Arrangement Set Premium = round(Premium, 2)  
  Where Ri_Arrangement_id = @Ri_Arrangement_id and ri_band_id = @band_id  
  
  Select @original_Ri_arrangement_id = ri_arrangement_id from Ri_arrangement  
   Where risk_cnt = @risk_cnt and Original_flag = 1 and ri_band_id = @Band_id  
  
  DECLARE C_RI CURSOR FAST_FORWARD FOR  
         SELECT ri_arrangement_line_id,type,Treaty_id,party_cnt,default_share_percent,  
         	this_share_percent,premium_percent,commission_percent,  
                agreement_code,priority,number_of_lines,line_limit,ral.sum_insured,premium_value,  
         	commission_value,premium_tax,commission_tax,is_commission_modified  
         FROM RI_Arrangement_Line ral  
  	 	INNER JOIN RI_Arrangement ra ON ra.ri_arrangement_id=ral.ri_arrangement_id  
         WHERE ra.risk_cnt=@risk_cnt and ra.ri_arrangement_id = @original_ri_arrangement_id  
  
  OPEN C_RI  
  FETCH NEXT FROM C_RI INTO @ri_arrangement_line_id,@type,@Treaty_id,@party_cnt,@default_share_percent,  
         	@this_share_percent,@premium_percent,@commission_percent,  
                @agreement_code,@priority,@number_of_lines,@line_limit,@sum_insured,@premium_value,  
         	@commission_value,@premium_tax,@commission_tax,@is_commission_modified  
  
  
      WHILE @@FETCH_STATUS = 0  
      BEGIN  
  
   	Update Ri_Arrangement_line   
        	Set this_share_percent = @this_share_percent,  
		premium_percent = @premium_percent,  
		sum_insured = -1 * @sum_insured
		Where ri_arrangement_id = @Ri_arrangement_id AND treaty_id = @treaty_id AND party_cnt = @party_cnt  
		And type = @type And priority = @priority AND number_of_lines = @number_of_lines  
  
       FETCH NEXT FROM C_RI INTO @ri_arrangement_line_id,@type,@Treaty_id,@party_cnt,@default_share_percent,  
  	        @this_share_percent,@premium_percent,@commission_percent,  
		@agreement_code,@priority,@number_of_lines,@line_limit,@sum_insured,@premium_value,  
        	@commission_value,@premium_tax,@commission_tax,@is_commission_modified  
      END  
  CLOSE C_RI  
  DEALLOCATE C_RI  
  
         Execute spu_RI_Arrangement_taxes  
	          @insurance_file_cnt = @insurance_file_cnt,  
        	  @risk_cnt = @risk_cnt,  
                  @ri_arrangement_id = @ri_arrangement_id,  
                  @band_premium = @premium  
  
  FETCH NEXT FROM CUR_RI INTO @ri_arrangement_id, @Premium, @Band_id  
  END  

 CLOSE CUR_RI  
 DEALLOCATE CUR_RI  
End

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

