SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sir_agent_commission_sel'
GO


CREATE PROCEDURE spu_sir_agent_commission_sel
    @insurance_file_cnt int
AS

BEGIN

    -- Read the commission display level option (5264: Display Commission at Commission Band Level)
    DECLARE @display_band_level BIT = 0

    SELECT @display_band_level = ISNULL(
        (SELECT CAST(so.value AS BIT) 
         FROM system_options so
         INNER JOIN insurance_file ifile ON ifile.insurance_file_cnt = @insurance_file_cnt
         INNER JOIN source s ON s.source_id = ifile.source_id
         WHERE so.branch_id = s.source_id 
         AND so.option_number = 5264
         AND so.value = '1'), 0)

    IF @display_band_level = 1
    BEGIN
        -- BAND LEVEL: Aggregate by commission_band_id
        SELECT
            P.shortname,
            PAT.Description,
            RT.Code,
            CB.Code,
            SUM(AC.premium) AS premium,
            CASE 
                WHEN SUM(AC.premium) = 0 THEN 0.00
                ELSE ROUND((SUM(AC.commission_value) / SUM(AC.premium)) * 100, 2)
            END AS commission_percentage,
            SUM(AC.commission_value) AS commission_value,
            AC.is_lead_agent,
            MAX(CAST(AC.is_amended AS INT)) AS is_amended,
            AC.party_cnt,
            PAT.Party_Agent_Type_id,
            AC.risk_type_id,
            AC.commission_band_id,
            C.description,
            CASE 
                WHEN COUNT(DISTINCT AC.tax_group_id) = 1 
                THEN MIN(AC.tax_group_id)
                ELSE NULL
            END AS tax_group_id,
            CASE 
                WHEN COUNT(DISTINCT AC.tax_group_id) = 1 
                THEN MIN(TG.description)
                ELSE 'Multiple'
            END AS [description],
            ROUND(SUM(AC.tax_amount), 2) AS 'tax_amount',
            SUM(AC.calculated_commission_value) AS calculated_commission_value,
            NULL AS override_reason,
            NULL AS maximum_rate,
            MAX(CAST(AC.Is_Value AS INT)) AS Is_Value,
            NULL AS peril_type_id,
            NULL AS class_of_business_id,
            MAX(CAST(AC.is_locked AS INT)) AS is_locked
        FROM agent_commission AC
        JOIN Risk_Type RT
            ON RT.Risk_Type_id = AC.Risk_Type_id
        JOIN commission_band CB
            ON CB.commission_band_id = AC.commission_band_id
        JOIN insurance_file I
            ON I.insurance_file_cnt = AC.insurance_file_cnt
        JOIN currency C
            ON C.currency_id = I.currency_id    
        JOIN Party P
            ON P.Party_cnt = AC.party_cnt
        JOIN Party_Agent PA
            ON PA.Party_cnt = P.Party_cnt
        JOIN Party_Agent_Type PAT
            ON PAT.Party_agent_Type_id = PA.Party_agent_type_id
        LEFT JOIN Tax_Group TG
            ON TG.tax_group_id = AC.tax_group_id
        WHERE AC.insurance_file_cnt = @insurance_file_cnt
        GROUP BY 
            P.shortname, PAT.Description, RT.Code, CB.Code,
            AC.is_lead_agent, AC.party_cnt, PAT.Party_Agent_Type_id,
            AC.risk_type_id, AC.commission_band_id, C.description
        ORDER BY AC.risk_type_id, AC.commission_band_id, AC.party_cnt
    END
    ELSE
    BEGIN
        -- PERIL TYPE LEVEL: Existing 6.3 behaviour (unchanged)
        SELECT
            P.shortname,
            PAT.Description,
            RT.Code,
            CB.Code, premium,
            AC.commission_percentage,
            AC.commission_value,
            AC.is_lead_agent,
            AC.is_amended,
            AC.party_cnt,
            PAT.Party_Agent_Type_id,
            AC.risk_type_id,
            AC.commission_band_id,
            C.description,
            AC.tax_group_id,
            TG.description,
            Round(AC.tax_amount,2) 'tax_amount',
            AC.calculated_commission_value,
            AC.override_reason,
--Start - Renuka - (WPR64 Paralleling)
	    AC.maximum_rate,
	    AC.Is_Value,
	    AC.peril_type_id,
	    AC.class_of_business_id,   
            AC.is_locked  
--End - Renuka - (WPR64 Paralleling)
        FROM agent_commission AC
        JOIN Risk_Type RT
            ON RT.Risk_Type_id = AC.Risk_Type_id
        JOIN commission_band CB
            ON CB.commission_band_id = AC.commission_band_id
        JOIN insurance_file I
            ON I.insurance_file_cnt = AC.insurance_file_cnt
        JOIN currency C
            ON C.currency_id = I.currency_id    
        JOIN  Party P
            ON P.Party_cnt = AC.party_cnt
        JOIN Party_Agent PA
            ON  PA.Party_cnt = P.Party_cnt
        JOIN Party_Agent_Type PAT
            ON PAT.Party_agent_Type_id = PA.Party_agent_type_id
        LEFT JOIN Tax_Group TG
            ON TG.tax_group_id = AC.tax_group_id
        WHERE AC.insurance_file_cnt = @insurance_file_cnt
        ORDER BY
            AC.risk_type_id,
            AC.commission_band_id,
            AC.party_cnt
    END
END

GO


