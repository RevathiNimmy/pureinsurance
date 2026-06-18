SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Live_Premiums_Ins'
GO

CREATE PROCEDURE spu_SirRen_Live_Premiums_Ins
    @insurance_folder_cnt int
AS

DECLARE @ins_ref varchar(50)
DECLARE @this_premium numeric(19, 2)
DECLARE @renewal_premium numeric(19, 2)
DECLARE @resolved_name varchar(100)
DECLARE @shortname varchar(50)
DECLARE @agent_name varchar(100)
DECLARE @insured_cnt int
DECLARE @bus_code varchar(10)
DECLARE @lead_insurer_cnt int
DECLARE @iPolicy_Insurance_File_Type_ID INT
DECLARE @iMTAPerm_Insurance_File_Type_ID INT
DECLARE @Quote decimal (9, 2)
DECLARE @Excess decimal (9, 2)
DECLARE @cntr int
DECLARE @Datamodel varchar(10) /* AK 180901 */
DECLARE @gis_screen_id int --TF180302
DECLARE @insurance_file_cnt INT
DECLARE @gis_scheme_id INT
DECLARE @net_renewal_premium numeric(19, 2)
DECLARE @addon_premium numeric(19, 2)

SELECT @iPolicy_Insurance_File_Type_ID = insurance_file_type_id
    FROM Insurance_File_Type
    WHERE code = 'POLICY'

SELECT @iMTAPerm_Insurance_File_Type_ID = insurance_file_type_id
    FROM Insurance_File_Type
    WHERE code = 'MTA PERM'

IF EXISTS (
    SELECT NULL from Insurance_File
    WHERE insurance_folder_cnt = @insurance_folder_cnt
    AND insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID
) BEGIN
    SELECT @ins_ref = i.insurance_ref,
        @this_premium = i.annual_premium,
        @resolved_name = p1.resolved_name,
        @shortname = p1.shortname,
        @agent_name = isnull(p2.resolved_name, '-'),
        @insured_cnt = i.insured_cnt,
        @bus_code = RG.description,
        @lead_insurer_cnt = i.lead_insurer_cnt,
        @gis_screen_id = ISNULL(RG.gis_screen_id, 0)
    FROM Insurance_Folder insfold
    INNER JOIN Insurance_File i ON i.insurance_folder_cnt = insfold.insurance_folder_cnt
    INNER JOIN gis_policy_link l ON i.insurance_file_cnt = l.insurance_file_cnt
    INNER JOIN gis_scheme s ON l.gis_scheme_id = s.gis_scheme_id
    INNER JOIN GIS_Business_Type gisbt ON i.gemini_business_type = gisbt.gis_business_type_id
    INNER JOIN party p1 ON i.insured_cnt = p1.party_cnt
    INNER JOIN Risk_Code RC ON RC.risk_code_id = I.risk_code_id
    INNER JOIN Risk_Group RG ON RG.risk_group_id = RC.risk_group_id
    --LEFT OUTER JOIN party p2 ON i.lead_insurer_cnt = p2.party_cnt
    LEFT OUTER JOIN party p2 ON i.lead_agent_cnt = p2.party_cnt
    WHERE i.insurance_folder_cnt = @insurance_folder_cnt
    AND i.insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID
    AND i.insurance_file_cnt = (
        SELECT MAX(insurance_file_cnt)
        FROM insurance_file
        WHERE insurance_folder_cnt = @insurance_folder_cnt
        AND insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID
        AND cover_start_date = (
            SELECT MAX(cover_start_date)
            FROM insurance_file
            WHERE insurance_folder_cnt = @insurance_folder_cnt
            AND insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID
        )
    )
END ELSE BEGIN
    SELECT @ins_ref = i.insurance_ref,
        @this_premium = i.annual_premium,
        @resolved_name = p1.resolved_name,
        @shortname = p1.shortname,
        @agent_name = isnull(p2.resolved_name, '-'),
        @insured_cnt = i.insured_cnt,
        @bus_code = gisbt.code,
        @lead_insurer_cnt = i.lead_insurer_cnt,
        @gis_screen_id = ISNULL(RG.gis_screen_id, 0)
    FROM Insurance_Folder insfold
    INNER JOIN Insurance_File i ON i.insurance_folder_cnt = insfold.insurance_folder_cnt
    INNER JOIN gis_policy_link l ON i.insurance_file_cnt = l.insurance_file_cnt
    INNER JOIN gis_scheme s ON l.gis_scheme_id = s.gis_scheme_id
    INNER JOIN GIS_Business_Type gisbt ON i.gemini_business_type = gisbt.gis_business_type_id
    INNER JOIN party p1 ON i.insured_cnt = p1.party_cnt
    INNER JOIN Risk_Code RC ON RC.risk_code_id = I.risk_code_id
    INNER JOIN Risk_Group RG ON RG.risk_group_id = RC.risk_group_id
    LEFT OUTER JOIN party p2 ON i.lead_agent_cnt = p2.party_cnt
    --LEFT OUTER JOIN party p2 ON i.lead_insurer_cnt = p2.party_cnt
    WHERE i.insurance_folder_cnt = @insurance_folder_cnt
    AND i.insurance_file_type_id = @iPolicy_Insurance_File_Type_ID
END

/* Fetch the renewal premium */
--TF191102 - Shouldn't we be using renewal_ins_file_cnt?
SELECT @renewal_premium = I.this_premium, 
       @insurance_file_cnt = R.renewal_insurance_file_cnt,
       @net_renewal_premium = I.net_premium
    FROM Insurance_File I,
         Renewal_Control R
    WHERE I.insurance_file_cnt = R.renewal_insurance_file_cnt
    AND   R.insurance_folder_cnt = @insurance_folder_cnt
/*
SELECT @renewal_premium = this_premium, @insurance_file_cnt = i.insurance_file_cnt,
       @net_renewal_premium = net_premium
    FROM insurance_file i,
    insurance_file_status s,
    insurance_file_type t
    WHERE i.insurance_file_status_id = s.insurance_file_status_id
    AND i.insurance_file_type_id = t.insurance_file_type_id
    AND i.insurance_folder_cnt = @insurance_folder_cnt
    AND i.lead_insurer_cnt = @lead_insurer_cnt
    AND s.code = 'REN'
    AND t.code = 'RENEWAL'
*/

SELECT @gis_scheme_id = renewal_gis_scheme_id FROM #IJM_Test

IF NOT EXISTS (SELECT code FROM pmproduct WHERE code = 'GeminiII')
BEGIN
    exec spu_cnc_get_addons @insurance_file_cnt, @gis_scheme_id, @net_renewal_premium, @addon_premium OUTPUT
END
SELECT @renewal_premium = @renewal_premium + @addon_premium

/* Update the temporary table with values */
UPDATE #IJM_Test
    SET insurance_ref = @ins_ref,
    this_premium = @this_premium,
    resolved_name = @resolved_name,
    shortname = @shortname,
    agent_name = @agent_name,
    insured_cnt = @insured_cnt,
    business_type_code = @bus_code,
    renewal_premium = @renewal_premium,
    insurer_id = @lead_insurer_cnt,
    gis_screen_id = @gis_screen_id
    WHERE insurance_folder_cnt = @insurance_folder_cnt

GO

