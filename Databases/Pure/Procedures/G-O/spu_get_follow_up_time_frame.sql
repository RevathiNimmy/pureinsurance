SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_follow_up_time_frame'
GO

-- Created: PW041002

CREATE PROCEDURE spu_get_follow_up_time_frame
    @follow_up_time_frame integer OUTPUT,
    @insurance_file_cnt integer
AS

BEGIN

        SELECT @follow_up_time_frame = p.follow_up_time_frame
          FROM Insurance_File i
    INNER JOIN Product p
            ON i.product_id = p.product_id
         WHERE i.insurance_file_cnt = @insurance_file_cnt

END
GO

