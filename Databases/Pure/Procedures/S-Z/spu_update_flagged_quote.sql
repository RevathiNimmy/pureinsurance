SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_flagged_quote'
GO

-- Created: PW041002

CREATE PROCEDURE spu_update_flagged_quote
    @insurance_file_cnt integer,
    @follow_up_due_date datetime = NULL ,
    @follow_up_note varchar(255) = NULL,
    @referred_to varchar(255) = NULL

AS

BEGIN

    DELETE FROM Flagged_Quote
          WHERE insurance_file_cnt = @insurance_file_cnt

    IF @follow_up_note IS NOT NULL
        BEGIN
        INSERT INTO Flagged_Quote (
            insurance_file_cnt,
            follow_up_due_date,
            description,
            referred_to)
        VALUES (
            @insurance_file_cnt,
            @follow_up_due_date,
            @follow_up_note,
            @referred_to)
    END

END
GO

