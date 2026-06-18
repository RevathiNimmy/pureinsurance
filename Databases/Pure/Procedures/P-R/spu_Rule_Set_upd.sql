SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Rule_Set_upd'
GO

CREATE PROCEDURE spu_Rule_Set_upd
    @rule_set_id int,
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
/* 1.0  08/01/2001 RWH Updates existing Rule Set           */
/*************************************************************************/
BEGIN

UPDATE Rule_Set
    SET
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    rule_set_type=@rule_set_type,
    file_name=@file_name,
    live=@live,
	risk_type_rule_set_type_id=@risk_type_rule_set_type_id
WHERE rule_set_id = @rule_set_id

END
GO

