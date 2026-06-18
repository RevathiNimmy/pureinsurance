EXECUTE DDLDropProcedure 'spu_Get_MTA_Reason_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_MTA_Reason_List  
@Risk_Group char(10)  
AS  
    SELECT mta.[Description] FROM MTA_Reason mta  
    LEFT JOIN risk_group rg ON rg.risk_group_id=mta.risk_group_id  
    WHERE (rg.code=@Risk_Group OR mta.risk_group_id is null)  
    AND mta.is_deleted = 0  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
