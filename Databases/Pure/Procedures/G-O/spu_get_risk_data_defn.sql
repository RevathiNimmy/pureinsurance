EXECUTE DDLDropProcedure 'spu_get_risk_data_defn'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spu_get_risk_data_defn
@Risk_type_id int
AS
SELECT Risk_data_definition.risk_data_defn_id,
Risk_data_definition.Caption, Risk_data_definition.Description,
Risk_data_definition.type, Risk_data_definition.display_order,
Risk_data_definition.Mandatory,
Risk_data_definition.read_only,
Risk_data_definition.Claim_party_type_id,
Claim_Party_type.Description AS Expr1,
Risk_data_definition.Claim_Lookup_id,
Claim_Lookup.Lookup_tablename,
ISNULL(Claim_Tab.Caption,'')
FROM Risk_data_definition LEFT OUTER JOIN
Claim_Lookup ON
Risk_data_definition.Claim_Lookup_id = Claim_Lookup.claim_lookup_id
LEFT OUTER JOIN
Claim_Party_type ON
Risk_data_definition.Claim_party_type_id = Claim_Party_type.Claim_Party_type_id
LEFT OUTER JOIN CLaim_Tab ON
Risk_data_definition.Tab_ID = Claim_Tab.Claim_Tab_ID
WHERE (Risk_data_definition.Risk_type_id = @Risk_type_id)
order by Risk_data_definition.display_order



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


