SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ri_arrangement_line_premiumpercent_upd'     
GO


CREATE PROCEDURE spu_ri_arrangement_line_premiumpercent_upd
@ri_arrangement_id	INT

AS

DECLARE @TotalPremium NUMERIC(19,8)

SELECT 	@TotalPremium = SUM(premium_value) 
FROM 	ri_arrangement_line
WHERE 	ri_arrangement_id=@ri_arrangement_id

IF @TotalPremium <> 0     
 UPDATE  ri_arrangement_line    
 SET  premium_percent=premium_value/@TotalPremium * 100    
 WHERE  ri_arrangement_id=@ri_arrangement_id    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
