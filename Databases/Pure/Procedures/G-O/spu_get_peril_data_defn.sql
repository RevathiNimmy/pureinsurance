EXECUTE DDLDropProcedure 'spu_get_peril_data_defn'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE spu_get_peril_data_defn
@Peril_type_id int
AS
SELECT Peril_data_definition.peril_data_defn_id,
Peril_data_definition.Caption, Peril_data_definition.Description,
Peril_data_definition.type, Peril_data_definition.display_order,
Peril_data_definition.Mandatory,
Peril_data_definition.read_only,
Peril_data_definition.Claim_party_type_id,
Claim_Party_type.Description AS Expr1,
Peril_data_definition.Claim_Lookup_id,
Claim_Lookup.Lookup_tablename,
ISNULL(Claim_Tab.Caption,'')
FROM Peril_data_definition LEFT OUTER JOIN
Claim_Lookup ON
Peril_data_definition.Claim_Lookup_id = Claim_Lookup.claim_lookup_id
LEFT OUTER JOIN
Claim_Party_type ON
Peril_data_definition.Claim_party_type_id = Claim_Party_type.Claim_Party_type_id
LEFT OUTER JOIN CLaim_Tab ON
Peril_data_definition.Tab_ID = Claim_Tab.Claim_Tab_ID
WHERE (Peril_data_definition.Peril_type_id = @Peril_type_id)
order by Peril_data_definition.display_order




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

