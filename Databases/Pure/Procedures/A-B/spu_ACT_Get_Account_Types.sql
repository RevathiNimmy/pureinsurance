SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_Account_Types'
GO

CREATE PROCEDURE spu_ACT_Get_Account_Types  

AS 
BEGIN  
 
	SELECT 
	  0 AS ledger_id,
	 '(all)' AS ledger_name
	UNION
	SELECT 
	    ledger.ledger_id,  
	    ledger.ledger_name  
	FROM ledger   
	WHERE ledger.ledger_id in (2,4,5,6,7,8,9,10) 
	UNION   
	SELECT  
	    21 AS ledger_id,  
	    'Write Off' AS ledger_name  
	UNION  
	SELECT  
	    22 AS ledger_id,  
	    'Extra' AS ledger_name  
END  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
