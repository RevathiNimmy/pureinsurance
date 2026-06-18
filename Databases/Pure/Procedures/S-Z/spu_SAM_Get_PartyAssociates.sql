SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_PartyAssociates'
GO

--Start (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.17)  
CREATE PROCEDURE spu_SAM_Get_PartyAssociates  


@party_cnt INT

AS

BEGIN

 SELECT

 pr.party_cnt ,

 pr.relation_cnt,

 pr.relationship_type_id,

 pr.[description],

 --Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

 p.shortname,

 p.resolved_name,

 C.iso_code CurrencyCode

 --End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

 FROM Party_Relationship pr

 INNER join party p ON pr.relation_cnt=p.party_cnt

 INNER join Currency c ON p.currency_id=c.currency_id

 WHERE pr.party_cnt=@party_cnt

 ORDER BY relation_cnt ASC

 END

--End (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.17)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO  
