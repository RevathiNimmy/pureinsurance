SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Upd_Premium_Percent_RI2007'
GO


CREATE PROCEDURE spu_Upd_Premium_Percent_RI2007  
        @ri_arrangement_id        INT  
AS  
  
DECLARE @tot_premium        NUMERIC(19,4)  
  
SELECT       @tot_premium = SUM(premium_value)  
FROM         ri_arrangement_line  
WHERE        ri_arrangement_id = @ri_arrangement_id  
 
IF @tot_premium <> 0  
BEGIN  
 UPDATE ri_arrangement_line  
  SET premium_percent = premium_value/@tot_premium * 100  
  WHERE        ri_arrangement_id = @ri_arrangement_id  
END  
ELSE  
 UPDATE ri_arrangement_line  
 SET premium_percent = 0  
 WHERE        ri_arrangement_id = @ri_arrangement_id  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


