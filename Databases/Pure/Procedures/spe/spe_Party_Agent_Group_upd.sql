SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Agent_Group_upd'
GO

CREATE PROCEDURE spe_Party_Agent_Group_upd
    @party_cnt int,
    @group_active tinyint,
	@UserId int = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
AS
BEGIN
UPDATE Party_Agent_Group
    SET
    active = @group_active,
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
WHERE party_cnt = @party_cnt
END

GO

