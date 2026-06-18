SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_lapse_reason_id'
GO

CREATE PROCEDURE spu_update_lapse_reason_id
    @lapsed_reason_id int,
    @insurance_file_cnt int
AS
UPDATE insurance_file
SET lapsed_reason_id=@lapsed_reason_id
WHERE insurance_file_cnt=@insurance_file_cnt
GO
