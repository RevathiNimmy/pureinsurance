-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    02 Oct 2003
--  Desc:    SFB 1.8.6 Accident Management development
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Event_Party_Extra_ins'
GO

CREATE PROCEDURE spu_Event_Party_Extra_ins
(
	@party_cnt int,
	@agency_number varchar(255) = NULL 
)
AS 

INSERT INTO 
	Event_Party_Extra
(
	party_cnt,
	agency_number
)
VALUES 
(
	@party_cnt,
	@agency_number
)

GO

