-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    02 Oct 2003
--  Desc:    SFB 1.8.6 Accident Management development
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Event_Party_Extra_upd'
GO

CREATE PROCEDURE spu_Event_Party_Extra_upd
(
	@party_cnt int,
	@agency_number varchar(255) = NULL 
)
AS 

UPDATE 
	Event_Party_Extra
SET
	agency_number = @agency_number
WHERE
	party_cnt = @party_cnt 

GO

