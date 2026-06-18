SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_update_core_fieldsets'
GO


CREATE PROCEDURE spu_update_core_fieldsets
    @main_group VARCHAR(255),
	@sub_group VARCHAR(255),
	@coreFieldset VARCHAR(255)
AS


BEGIN

UPDATE wp_fields
SET table_name = @coreFieldset
WHERE main_group=@main_group
AND sub_group=@sub_group 

END
GO


