SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_receipt_details'
GO

CREATE PROCEDURE spu_get_receipt_details  
    @perilid int,  
    @claimid int,  
    @type int  
AS  
  
BEGIN
	SELECT sum(amount) 
	FROM Receipt 
	WHERE recovery_id IN  
		(SELECT recovery_id 
		FROM Recovery 
		WHERE recovery_type_id IN (
			SELECT recovery_type_id 
			FROM recovery_type 
			WHERE is_salvage=@type)  
	AND claim_peril_id=@perilid)  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
