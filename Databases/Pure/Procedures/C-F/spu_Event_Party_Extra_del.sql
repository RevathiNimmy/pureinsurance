-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    02 Oct 2003
--  Desc:    SFB 1.8.6 Accident Management development
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Event_Party_Extra_del'
GO

CREATE PROCEDURE spu_Event_Party_Extra_del
(
	@party_cnt int
)
AS 

DELETE FROM 
	Event_Party_Extra
WHERE
	party_cnt = @party_cnt

GO



