SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Update_Claim_status'
GO

CREATE PROCEDURE spu_CLM_Update_Claim_status    
    
@claim_id INT,    
@claim_status_id INT    
    
AS    
    
BEGIN    
    
DECLARE @Progress_status_id AS INT,    
     @base_CLaim_id AS INT    
    
 UPDATE Claim    
 SET claim_status_id = @claim_status_id    
 WHERE claim_id = @claim_id    
    
IF @claim_status_id <> 3 AND @claim_status_id <> 5    
BEGIN    
    
 SELECT @base_CLaim_id = base_CLaim_id    
  FROM claim WHERE claim_id = @Claim_id    
  
	IF EXISTS(SELECT c.progress_status_id FROM claim c
	JOIN progress_status p ON c.Progress_Status_id = p.progress_status_id
	WHERE Claim_id = @claim_id AND p.is_closed_check_status = 1)
	BEGIN
		SELECT TOP 1 @Progress_status_id = c.progress_status_id    
		FROM claim c    
		INNER JOIN progress_status p    
		ON c.progress_status_id = p.progress_status_id    
		WHERE base_claim_id = @base_claim_id AND p.is_closed_check_status = 0 AND claim_id <> @Claim_id    
		ORDER BY claim_id DESC    
		if @progress_status_id IS NOT NULL    
		BEGIN    
			UPDATE Claim SET Progress_status_id = @progress_status_id    
			WHERE claim_id = @claim_id    
		END    
	END


 
END    
IF @claim_status_id = 3    
BEGIN    
 SELECT @progress_status_id = progress_status_id FROM Progress_Status WHERE code = 'CLOSED'    
 UPDATE c SET progress_status_id = @progress_status_id    
    FROM claim c INNER JOIN progress_status p ON c.progress_status_id = p.progress_status_id    
    WHERE p.is_closed_check_status = 0    
    AND c.claim_id = @claim_id    
END    
    
END 


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
