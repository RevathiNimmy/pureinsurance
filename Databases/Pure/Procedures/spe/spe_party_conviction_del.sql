SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_conviction_del'
GO

CREATE PROCEDURE spe_party_conviction_del
    @party_cnt int,
    @party_conviction_id int,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS

UPDATE party_conviction SET UserId = @UserId, UniqueId = @UniqueId, ScreenHierarchy = @ScreenHierarchy
WHERE party_cnt = @party_cnt AND party_conviction_id = @party_conviction_id

DELETE FROM party_conviction
WHERE party_cnt = @party_cnt AND party_conviction_id = @party_conviction_id

GO

