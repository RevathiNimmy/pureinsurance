SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_Limits_Desc'
GO

CREATE PROCEDURE spu_Get_Policy_Limits_Desc
    @PolicyLimitsId int,
    @description varchar(200)	OUTPUT
    
AS

SELECT @description=description FROM Policy_Limits WHERE policy_Limits_id=@PolicyLimitsId
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF