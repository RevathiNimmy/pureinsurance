SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_UpdateRiskStatus'
GO
CREATE PROCEDURE spu_UpdateRiskStatus
 @riskCnt int   
AS   
  
    Declare   
     @riskStatusID int,   
     @isDeferred int,   
--Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (6.1.1.1)   
     @SumInsured money,   
     @Premium money,   
     @AllcatedSumInsured money,   
     @AllocatedPremium money   
  
        SELECT  @Premium=Round(Sum(premium),2), @SumInsured=Round(Sum(sum_insured),2) 
                FROM ri_arrangement 
        WHERE risk_cnt = @riskCnt and original_flag = 0   
  
         SELECT  @AllcatedSumInsured=Round(Sum(ral.Sum_insured),2),@AllocatedPremium=Round(Sum(ral.Premium_value),2) 
                FROM ri_arrangement_line ral 
                INNER JOIN ri_arrangement ra 
                        ON ral.ri_arrangement_id = ra.ri_arrangement_id 
                WHERE risk_cnt = @riskCnt and original_flag = 0 and NOT(type != 'R' and ISNULL(retained,0) = 1) 
				--we are adding Retained amount of FAC to Base Retained line this to Original Retained
  
--End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (6.1.1.1)   
    -- Check if deferred RI exists for this risk   
    If Exists ( Select  *   
                From    ri_arrangement ra   
                Join    ri_model rm ON rm.ri_model_id = ra.ri_model_id   
                Where   ra.risk_cnt = @riskCnt   
                And     ra.original_flag = 0 -- only new records!   
                And     rm.ri_model_type = 2) -- deferred   
  
        -- Set risk status   
        Update  Risk   
        Set     risk_status_id = (Select risk_status_id   
                                    From risk_status   
                                    Where code = 'RIDEFERRED')   
        Where   risk_cnt = @riskCnt   
        And     risk_status_id = 8   
 --Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (6.1.1.1)   
  Else If @SumInsured <> @AllcatedSumInsured   
  
 Update  Risk   
      Set     risk_status_id = (Select risk_status_id   
                              From risk_status   
                                    Where code = 'PENDINGRI')   
        Where   risk_cnt = @riskCnt   
 --End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (6.1.1.1)   
    Else   
  BEGIN
        -- Set risk status   
        Update  Risk   
        Set     risk_status_id = (Select risk_status_id   
                                    From risk_status   
                                    Where code = 'QUOTED')   
        Where   risk_cnt = @riskCnt   
        And     risk_status_id in(8,4)

		Update insurance_file_pt_log set status_id=2 where risk_cnt=@riskCnt and status_id=1
		Update insurance_file_clone_log set status_id=2 where risk_cnt=@riskCnt and status_id=1

	END
