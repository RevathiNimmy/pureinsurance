SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_consultant_upd'
GO

CREATE PROCEDURE spe_party_consultant_upd
    @party_cnt int,
    @forename varchar(60),
    @initials varchar(20),
    @department_id int,
    @party_title_code varchar(70),
    @commission_cnt int = NULL,
	@user_id int = NULL,
	@unique_id varchar(50) = NULL,
	@screen_hierarchy varchar(500) = NULL
AS
BEGIN
UPDATE party_consultant
    SET
    forename=@forename,
    initials=@initials,
    department_id=@department_id,
    party_title_code=@party_title_code,
    commission_cnt=@commission_cnt,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy
WHERE party_cnt = @party_cnt
END

GO

