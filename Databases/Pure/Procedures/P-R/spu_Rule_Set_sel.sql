SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Rule_Set_sel'
GO

CREATE PROCEDURE spu_Rule_Set_sel
    @rule_set_id int = NULL,
    @rule_set_type int = NULL
AS
/*************************************************************************/
/* 1.0  04/01/2001 RWH Original (Based on SP Original)           */
/*************************************************************************/
BEGIN

IF @rule_set_id IS NOT NULL
BEGIN
    SELECT  rs.rule_set_id,
        rs.caption_id,
        rs.code,
        rs.description,
        rs.is_deleted,
        rs.effective_date,
        rs.rule_set_type,
        rs.file_name,
        rs.live,
        c.caption,
		ISNULL(rs.risk_type_rule_set_type_id,0) 

    FROM    Rule_Set rs,
        PMCaption c

    WHERE   rs.rule_set_id = @rule_set_id
        AND rs.caption_id = c.caption_id

END
ELSE
BEGIN
    IF @rule_set_type IS NOT NULL
    BEGIN
        SELECT  rs.rule_set_id,
            rs.caption_id,
            rs.code,
            rs.description,
            rs.is_deleted,
            rs.effective_date,
            rs.rule_set_type,
            rs.file_name,
            rs.live,
            c.caption

        FROM    Rule_Set rs,
            PMCaption c

        WHERE   rs.rule_set_type = @rule_set_type   /*rule set types set up as constants in code. */
            AND rs.caption_id = c.caption_id

    END
END

END
GO

