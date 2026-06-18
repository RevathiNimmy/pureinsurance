SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_select_name_from_id'
GO
CREATE PROCEDURE spu_select_name_from_id
    @party_cnt INT,
    @shortname VARCHAR(20) OUTPUT
AS
DECLARE @rowcount INT
BEGIN
            SELECT @rowcount = SUM(1)
            FROM party
            WHERE party_cnt = @party_cnt

            IF @rowcount = 1
            BEGIN
                        SELECT @shortname = shortname

                        FROM party

                        WHERE party_cnt = @party_cnt
            END
            ELSE
            BEGIN
                        SELECT @party_cnt = -1
            END
END
GO


