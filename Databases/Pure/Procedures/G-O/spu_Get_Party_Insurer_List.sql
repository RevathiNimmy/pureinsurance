EXECUTE DDLDropProcedure 'spu_Get_Party_Insurer_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Party_Insurer_List

--
-- ED03072002 Retrieves list of PARTY of type INSURER
--

AS

SELECT party_cnt, resolved_name
  FROM party
 INNER JOIN party_type ON party.party_type_id = party_type.party_type_id
 WHERE Party_type.code = 'IN'
 ORDER BY resolved_name

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

