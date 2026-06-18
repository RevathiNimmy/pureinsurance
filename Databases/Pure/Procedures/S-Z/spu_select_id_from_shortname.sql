SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_select_id_from_shortname'
GO

CREATE PROCEDURE spu_select_id_from_shortname
    @party_cnt int OUTPUT,
    @source_id smallint,
    @party_shortname varchar(60),
    @UserID int
AS

SELECT @party_cnt = party_cnt
FROM Party
WHERE shortname = @party_shortname
AND source_id NOT IN (SELECT source_id FROM PMUser_Source WHERE user_id = @UserID)
AND (@source_id IS NULL OR source_id = @source_id)


SELECT @party_cnt = IsNull(@party_cnt,-1)

GO