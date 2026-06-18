SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_del_follow_ups'
GO

-- Created: PW071002

CREATE PROCEDURE spu_del_follow_ups
    @insurance_file_cnt integer
AS
BEGIN

        DELETE
          FROM Flagged_Quote
         WHERE insurance_file_cnt = @insurance_file_cnt

END
GO
