SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_Policy_Deductible_Desc'
GO

CREATE PROCEDURE spu_Get_Policy_Deductible_Desc
    @PolicyDeductiblesId int,
    @description varchar(200)	OUTPUT
    
AS

SELECT @description=description FROM Policy_Deductibles WHERE policy_deductibles_id=@PolicyDeductiblesId
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
