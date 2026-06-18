SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Check_Recovery_Type'
GO

CREATE PROCEDURE spu_Check_Recovery_Type  
    @Recovery_type_id int,  
    @Peril_id int  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
BEGIN
 
    SELECT  recovery_type_id  
    FROM    recovery  
    WHERE   recovery_type_id = @recovery_type_id  
    AND     claim_peril_id = @peril_id  

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
