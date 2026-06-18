SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_follow_ups'
GO

-- Created: PW071002

CREATE PROCEDURE spu_get_follow_ups
    @run_date datetime
AS
BEGIN

        SELECT f.insurance_file_cnt,
               i.insured_name,
               i.insurance_ref,
               f.description
          FROM Flagged_Quote f
    INNER JOIN Insurance_File i
            ON f.insurance_file_cnt = i.insurance_file_cnt
         WHERE f.follow_up_due_date = @run_date
           AND (i.insurance_file_type_id = 1 OR i.insurance_file_type_id = 4)

END
GO
