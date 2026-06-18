SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_upd'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_upd
    @GIS_user_def_detail_id int,
    @GIS_user_def_header_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @Parent int,
    @GIS_user_def_header_inds_id int,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)

AS
BEGIN

UPDATE GIS_user_def_detail
    SET
    GIS_user_def_header_id=@GIS_user_def_header_id,
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date,

    Parent=@Parent,
    GIS_user_def_header_inds_id=@GIS_user_def_header_inds_id,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy

WHERE GIS_user_def_detail_id = @GIS_user_def_detail_id

END

GO

