SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Last_Print_Run_add'
GO

CREATE PROCEDURE spe_Last_Print_Run_add
    @renewal_status_cnt int,
    @UserID int

AS

BEGIN
DELETE Last_Print_Run WHERE renewal_status_cnt=@renewal_status_cnt

INSERT INTO Last_Print_Run (
    renewal_status_cnt, userid )
VALUES (
    @renewal_status_cnt,
    @UserID)
END

GO

