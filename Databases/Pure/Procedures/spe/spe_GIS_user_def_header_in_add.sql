SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_header_in_add'
GO

CREATE PROCEDURE spe_GIS_user_def_header_in_add
    @GIS_user_def_header_id int,
    @GIS_user_def_header_inds_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)

AS

BEGIN
INSERT INTO GIS_user_def_header_inds (
    GIS_user_def_header_id ,
    GIS_user_def_header_inds_id ,
    caption_id ,
    code ,
    description ,
    is_deleted ,
    effective_date,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES (
    @GIS_user_def_header_id,
    @GIS_user_def_header_inds_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date,
	@user_id,
	@unique_id,
	@screen_hierarchy)
END

GO

