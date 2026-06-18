SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_header_add'
GO

CREATE PROCEDURE spe_GIS_user_def_header_add
    @GIS_user_def_header_id int OUTPUT ,
    @caption_id int ,
    @code char(10) ,
    @description varchar(255) ,
    @is_deleted tinyint ,
    @effective_date datetime ,
    @Parent int,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)

AS

BEGIN
INSERT INTO GIS_user_def_header (
    caption_id,
    code,
    description,
    is_deleted,
    effective_date,
    Parent,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES (
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date,
    @Parent,
	@user_id,
	@unique_id,
	@screen_hierarchy)
END

BEGIN
SELECT @GIS_user_def_header_id = SCOPE_IDENTITY()
END

GO

