SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRREN_Select_AutoRenew'
GO


CREATE PROCEDURE spu_SIRREN_Select_AutoRenew
    @insurance_file_cnt int
AS

/*****************************************************************************
* History: CTAF 13062001 - Created
* TF271101 - Only use renewal insurance_file_cnt (passed in)
******************************************************************************/
BEGIN
    SELECT payment_method
    FROM insurance_file
    WHERE insurance_file_cnt = @insurance_file_cnt
END
GO


