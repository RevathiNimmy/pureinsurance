SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_getfields_for_rskdatadefn'
GO

CREATE PROCEDURE spu_getfields_for_rskdatadefn    
    @RiskType integer,    
    @ClaimID integer,    
    @mandatory integer    
AS    
    
--*******************************************************************************************    
-- Version      Author  Date        Desc    
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting    
--*******************************************************************************************    
    
DECLARE @AgentUnderwriter varchar(1)    
    
SELECT  @AgentUnderwriter = value    
FROM    hidden_options    
WHERE   branch_id = 1 and option_number = 1    
    
IF @AgentUnderwriter is null    
    SELECT @AgentUnderwriter = 'A'    
    
IF @AgentUnderwriter = ''    
    SELECT @AgentUnderwriter = 'A'    
    
--ED 07-10-2002 - Last three columns slected are specific to Broking and funtionality    
--                enabling definition of tab specific user defined fields.    
    
IF @AgentUnderwriter = 'A'    
    
    --Broking - Selection uses 'claim_user_defined_risk_data' in place of    
    --          'claim_user_defined_risk_data'    
    SELECT    
        rd.risk_data_defn_id,    
        rd.Risk_type_id,    
        rd.Description,    
        rd.Claim_party_type_id,    
        rd.Caption,    
        rd.type,    
        rd.code,    
        rd.display_order,    
        rd.Mandatory,    
        rd.read_only,    
        rd.Claim_Lookup_id,    
        cd.claim_user_def_risk_data_id,    
        cd.claim_id,    
        cd.risk_data_defn_id,    
        cd.Value,    
        ISNULL(ct.Claim_Tab_ID,1) AS Claim_Tab_ID,    
        ISNULL(ct.Caption,'') AS Tab_Caption,    
        ISNULL(ct.Display_Order,'') as Tab_Display_Order    
    FROM risk_data_definition rd    
    LEFT OUTER JOIN claim_user_defined_risk_data cd    
        ON cd.risk_data_defn_id = rd.risk_data_defn_id    
        AND cd.claim_id = @claimId    
        AND cd.claim_user_def_risk_data_id =    
            (    
                SELECT    
                    MAX(claim_user_def_risk_data_id)    
                FROM claim_user_defined_risk_data    
                WHERE risk_data_defn_id = cd.risk_data_defn_id    
                AND claim_id = cd.claim_id    
            )    
    LEFT OUTER JOIN Claim_Tab ct    
        ON ct.Claim_Tab_ID = rd.Tab_ID    
    WHERE rd.Type <> 6    
    AND rd.Risk_Type_Id = @RiskType    
    ORDER BY    
        ct.Display_Order,    
        rd.Display_Order    
    
ELSE    
    
    -- Underwriting - Select default values for Tab specific user defined fields not    
    --                not available for Underwriting    
    
    SELECT    
    risk_data_definition.risk_data_defn_id,    
    risk_data_definition.Risk_type_id,    
    risk_data_definition.Description,    
    risk_data_definition.Claim_party_type_id,    
    risk_data_definition.Caption,    
    risk_data_definition.type,    
    risk_data_definition.code,    
    risk_data_definition.display_order,    
    risk_data_definition.Mandatory,    
    risk_data_definition.read_only,    
    risk_data_definition.Claim_Lookup_id,    
    claim_user_defined_risk_data.claim_user_def_risk_data_id,    
    claim_user_defined_risk_data.claim_id,    
    claim_user_defined_risk_data.risk_data_defn_id,    
    claim_user_defined_risk_data.Value,    
    1 AS Claim_Tab_ID,    
    '' AS Tab_Caption,    
    '' as Tab_Display_Order    
    
    FROM risk_data_definition    
    LEFT OUTER JOIN claim_user_defined_risk_data ON    
                                          (risk_data_definition.risk_data_defn_id =    
                                           claim_user_defined_risk_data.risk_data_defn_id    
                                       AND claim_user_defined_risk_data.claim_id = @claimId)    
    WHERE risk_data_definition.Type <> 6    
      AND risk_data_definition.Risk_Type_Id = @RiskType    
    
    ORDER BY risk_data_definition.Display_Order    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
