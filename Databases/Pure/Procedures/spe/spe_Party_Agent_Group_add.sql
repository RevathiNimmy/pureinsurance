SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Agent_Group_add'
GO

CREATE PROCEDURE spe_Party_Agent_Group_add
    @party_cnt int,
    @group_active tinyint,
	@UserId int = null,
	@UniqueId varchar(50) = null,
	@ScreenHierarchy varchar(500) = null
AS
BEGIN
INSERT INTO Party_Agent_Group (
    party_cnt ,
    active,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES (
    @party_cnt,
    @group_active,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)
END

GO

