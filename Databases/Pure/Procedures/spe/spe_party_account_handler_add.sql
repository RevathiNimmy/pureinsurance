SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_account_handler_add'
GO

CREATE PROCEDURE spe_party_account_handler_add
    @party_cnt int,
    @forename varchar(60),
    @initials varchar(20),
    @department_id int,
    @party_title_code varchar(70),
	@user_id int = NULL,
	@unique_id varchar(50) = NULL,
	@screen_hierarchy varchar(500) = NULL
AS
BEGIN
INSERT INTO party_account_handler (
    party_cnt ,
    forename ,
    initials ,
    department_id ,
    party_title_code,
	UserId,
	UniqueId,
	ScreenHierarchy )
VALUES (
    @party_cnt,
    @forename,
    @initials,
    @department_id,
    @party_title_code,
	@user_id,
	@unique_id,
	@screen_hierarchy)
END

GO

