SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_sir_del_risks_ri'
GO

CREATE PROCEDURE spu_sir_del_risks_ri
    @insurance_file_cnt INTEGER
AS

BEGIN

    DECLARE @risk_cnt INTEGER
	DECLARE @ri_arrangement_id INTEGER
	
    If Exists(SELECT r.risk_cnt  
        FROM risk r  
	INNER JOIN insurance_file_risk_link ifrl  
        ON r.risk_cnt = ifrl.risk_cnt  
        WHERE insurance_file_cnt = @insurance_file_cnt AND ifrl.original_risk_cnt IS NOT NULL)
		
    BEGIN 
	
    DECLARE c_risk CURSOR FAST_FORWARD FOR
        SELECT r.risk_cnt
        FROM risk r
		INNER JOIN insurance_file_risk_link ifrl
        ON r.risk_cnt = ifrl.risk_cnt
        WHERE insurance_file_cnt = @insurance_file_cnt AND ifrl.original_risk_cnt IS NOT NULL

    OPEN c_risk
    FETCH NEXT FROM c_risk INTO @risk_cnt
    WHILE @@FETCH_STATUS = 0
    BEGIN
		DELETE FROM RI_Arrangement_line_Broker_Participants
				FROM RI_Arrangement_line_Broker_Participants RIBr
					INNER JOIN ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBr.ri_arrangement_line_id
					INNER JOIN ri_arrangement ria ON ria.ri_arrangement_id =ril.ri_arrangement_id 
					WHERE risk_cnt = @risk_cnt 
				
		DELETE FROM ri_arrangement_Line 
				FROM ri_arrangement_Line ril
					INNER JOIN ri_arrangement ria ON ria.ri_arrangement_id = ril.ri_arrangement_id
					WHERE risk_cnt = @risk_cnt 
		DELETE FROM ri_arrangement WHERE risk_cnt = @risk_cnt
	FETCH NEXT FROM c_risk INTO @risk_cnt
	END
		
	CLOSE c_risk
	DEALLOCATE c_risk
		END
	ELSE
	BEGIN
	
	DECLARE c_risk CURSOR FAST_FORWARD FOR  
				SELECT r.risk_cnt  
				FROM risk r  
				INNER JOIN insurance_file_risk_link ifrl  
				ON r.risk_cnt = ifrl.risk_cnt  
				WHERE insurance_file_cnt = @insurance_file_cnt   
		  
	OPEN c_risk  
		FETCH NEXT FROM c_risk INTO @risk_cnt  
		WHILE @@FETCH_STATUS = 0  
	BEGIN  
		  
			SELECT @ri_arrangement_id = RI_Arrangement_id FROM RI_Arrangement 
				WHERE risk_cnt = @risk_cnt
			UPDATE RI_Arrangement SET is_modified =0 WHERE ri_arrangement_id = @ri_arrangement_id
		  
			FETCH NEXT FROM c_risk INTO @risk_cnt  
	END  
		  
		CLOSE c_risk  
		DEALLOCATE c_risk  
		
	END	 
 
		
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
         