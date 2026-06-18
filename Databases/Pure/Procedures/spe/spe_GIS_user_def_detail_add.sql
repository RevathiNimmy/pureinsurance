SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_add'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_add
    @GIS_user_def_detail_id int OUTPUT ,
    @GIS_user_def_header_id int ,
    @caption_id int ,
    @code char(10) ,
    @description varchar(255) ,
    @is_deleted tinyint ,
    @effective_date datetime ,
    @Parent int ,
    @GIS_user_def_header_inds_id int,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)

AS

BEGIN
INSERT INTO GIS_user_def_detail (
    GIS_user_def_header_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date,
    Parent,
    GIS_user_def_header_inds_id,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES (
    @GIS_user_def_header_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date,
    @Parent,
    @GIS_user_def_header_inds_id,
	@user_id,
	@unique_id,
	@screen_hierarchy)
END

BEGIN
SELECT @GIS_user_def_detail_id = SCOPE_IDENTITY()
END

GO

