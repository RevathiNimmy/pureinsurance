SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Rule_Set_add'
GO

CREATE PROCEDURE spu_Rule_Set_add
    @rule_set_id int OUTPUT,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @rule_set_type int,
    @file_name varchar(255),
    @live tinyint,
	@risk_type_rule_set_type_id int
AS
/*************************************************************************/
/* 1.0  08/01/2001 RWH Add a new Rule Set           */
/*************************************************************************/
BEGIN

IF @rule_set_id = 0
                SELECT @rule_set_id = NULL

IF @rule_set_id IS NULL
                SELECT @rule_set_id = MAX(rule_set_id) + 1
    FROM Rule_Set

IF @rule_set_id IS NULL
    SELECT @rule_set_id = 1

INSERT INTO Rule_Set (
    rule_set_id ,
    caption_id ,
    code ,
    description ,
    is_deleted ,
    effective_date ,
    rule_set_type ,
    file_name ,
    live,
	risk_type_rule_set_type_id)
VALUES (
    @rule_set_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date,
    @rule_set_type,
    @file_name,
    @live,
	@risk_type_rule_set_type_id)
END

BEGIN

SELECT rule_set_id = @rule_set_id

END
GO

