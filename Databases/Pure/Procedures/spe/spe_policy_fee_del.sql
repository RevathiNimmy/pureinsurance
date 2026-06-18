SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_policy_fee_del'
GO

 
CREATE PROCEDURE spe_policy_fee_del
    @insurance_file_cnt        int

AS
BEGIN

     DELETE FROM policy_fee WHERE insurance_file_cnt=@insurance_file_cnt

END
GO

