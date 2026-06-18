EXEC DDLDropProcedure 'spu_add_stats_details_spread_ri'
GO

CREATE Procedure spu_add_stats_details_spread_ri  
 @stats_folder_cnt INT,  
 @stats_detail_type VARCHAR(3)  
AS  
 DECLARE @peril_risk_id INT,  
            @stats_count INT,  
   @stats_version INT  

    DECLARE Spread_Cursor CURSOR FAST_FORWARD FOR  
        SELECT risk_id, stats_version, count(distinct(class_of_business_id))  
  FROM Stats_Detail  
  WHERE stats_folder_cnt=@stats_folder_cnt  
  AND stats_detail_type=@stats_detail_type  
  AND stats_version IN (0,1)  
  GROUP BY risk_id, stats_version  

    OPEN Spread_Cursor  
    FETCH NEXT FROM Spread_Cursor  
        INTO    @peril_risk_id,  
                @stats_version,  
    @stats_count  

    WHILE (@@FETCH_STATUS = 0)  
    BEGIN  
  UPDATE   Stats_Detail  
  SET   this_premium_original=this_premium_original/@stats_count,  
     this_premium_home=this_premium_home/@stats_count,  
     this_premium_system=this_premium_system/@stats_count,  
     lead_commission_value_home=lead_commission_value_home/@stats_count,  
     lead_commission_value_system=lead_commission_value_system/@stats_count,  
     stats_version=13  
  WHERE  stats_folder_cnt=@stats_folder_cnt  
  AND  risk_id=@peril_risk_id  
  AND  stats_detail_type = @stats_detail_type  
  AND  stats_version = @stats_version  
				
     FETCH NEXT FROM Spread_Cursor  
         INTO    @peril_risk_id,  
                 @stats_version,  
     @stats_count  
	
    END  
    CLOSE Spread_Cursor  
    DEALLOCATE Spread_Cursor  
GO
