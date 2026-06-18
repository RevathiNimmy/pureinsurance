SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_chk_main_reserve'
GO

CREATE PROCEDURE spu_chk_main_reserve  
    @perilid int,  
    @mode int  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
BEGIN
   	--If @mode=1 -- 0 represents Open Claim Mode  
	SELECT * 
	FROM Peril_Type_reserve_type 
	WHERE Peril_type_id IN 
		(SELECT peril_type_id 
		 FROM claim_peril 
		 WHERE  claim_peril_id=@perilid)
	AND is_main_reserve=1  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
