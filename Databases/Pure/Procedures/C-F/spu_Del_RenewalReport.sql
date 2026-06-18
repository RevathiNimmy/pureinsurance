SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Del_RenewalReport'
GO


CREATE PROCEDURE spu_Del_RenewalReport
    @user_id int = NULL
AS


IF (@user_id IS NULL) or (@user_id = 0)
    DELETE Renewal_Report
ELSE
    DELETE Renewal_Report
    WHERE user_id = @user_id
GO


