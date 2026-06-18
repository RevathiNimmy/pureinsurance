SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sforb_policy_list_agency_party'
GO

-- sj  03/07/2002 - Add user_insurer_cnt for restricting user access by insurer
-- CJB 05/09/2002 - Fix change made by TF - replace = with like in where clause
-- sj  19/09/2002 - Show cancelled policies in list
-- mkw 09/04/2003 - Retrieve risk code description instead of insurance file type description
-- DD  08/10/2003 - Added parameters to handle multi-company filtering
CREATE PROCEDURE spu_sforb_policy_list_agency_party
    @ins_file_type char(10),
    @insurance_ref varchar(255) = NULL,
    @party_cnt int,
    @user_insurer_cnt int = NULL,
    @source_id smallint=NULL,
    @transaction_type varchar(50)=NULL
AS
BEGIN
	DECLARE @multi_company SMALLINT
	
	-- Only filter for MTAs
	IF CHARINDEX('MTA',ISNULL(@transaction_type,''))>0 BEGIN
		-- Is the setup Multi-Company (16) with Branch Logon (37)
		SELECT	@multi_company=SUM(CAST(value AS INT))
		FROM	Hidden_Options
		WHERE	option_number=16 OR option_number=37
		AND	value='1'

		IF @multi_company IS NULL
			SELECT @multi_company=0
	END
	ELSE
		SELECT @multi_company=0
	
    -- Create the temp table
    CREATE TABLE #PolList (
        insurance_file_cnt int PRIMARY KEY CLUSTERED
    )

    -- Populate it
    IF @insurance_ref IS NOT NULL BEGIN
        IF @user_insurer_cnt IS NOT NULL BEGIN
            INSERT INTO #PolList
                SELECT MAX(ifi.insurance_file_cnt)
                FROM Insurance_File AS ifi
                INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                WHERE ifo.insurance_holder_cnt = @party_cnt
                AND ifi.insurance_file_type_id IN (1,2,5,6,10)
                AND ifi.policy_ignore IS NULL
                AND ifi.insurance_ref LIKE @insurance_ref
                AND ifi.lead_insurer_cnt = @user_insurer_cnt
                AND (ifi.source_id=@source_id OR @multi_company<>2)	-- Multi-company filter
                GROUP BY ifi.insurance_folder_cnt
        END ELSE BEGIN
            INSERT INTO #PolList
                SELECT MAX(ifi.insurance_file_cnt)
                FROM Insurance_File AS ifi
                INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                WHERE ifo.insurance_holder_cnt = @party_cnt
                AND ifi.insurance_file_type_id IN (1,2,5,6,10)
                AND ifi.policy_ignore IS NULL
                AND ifi.insurance_ref LIKE @insurance_ref
                AND (ifi.source_id=@source_id OR @multi_company<>2)	-- Multi-company filter
                GROUP BY ifi.insurance_folder_cnt
        END
    END ELSE BEGIN
        IF @user_insurer_cnt IS NOT NULL BEGIN
            INSERT INTO #PolList
                SELECT MAX(ifi.insurance_file_cnt)
                FROM Insurance_File AS ifi
                INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                WHERE ifo.insurance_holder_cnt = @party_cnt
                AND ifi.insurance_file_type_id IN (1,2,5,6,10)
                AND ifi.policy_ignore IS NULL
                AND ifi.lead_insurer_cnt = @user_insurer_cnt
                AND (ifi.source_id=@source_id OR @multi_company<>2)	-- Multi-company filter
                GROUP BY ifi.insurance_folder_cnt
        END ELSE BEGIN
            INSERT INTO #PolList
                SELECT MAX(ifi.insurance_file_cnt)
                FROM Insurance_File AS ifi
                INNER JOIN Insurance_Folder AS ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
                WHERE ifo.insurance_holder_cnt = @party_cnt
                AND ifi.insurance_file_type_id IN (1,2,5,6,10)
                AND ifi.policy_ignore IS NULL
                AND (ifi.source_id=@source_id OR @multi_company<>2)	-- Multi-company filter
                GROUP BY ifi.insurance_folder_cnt
        END
    END

    IF ISNULL(@ins_file_type, '') = 'POLICY' BEGIN
        -- Do the business
        SELECT Insurance_File.insurance_file_id,
            Insurance_File.source_id AS ins_file_source_id,
            Insurance_File.insurance_file_cnt,
            Insurance_File.insurance_ref,
            Insurance_File_System.last_trans_description AS insurance_folder_code,
            Insurance_File_type.code AS type_code,
            Party.name AS insured_name,
            Party.shortname AS insured_shortname,
            Party.party_id,
            Party.source_id AS party_source_id,
            Insurance_File.renewal_date,
            Insurance_Folder.insurance_holder_cnt,
            Insurance_Folder.insurance_folder_cnt,
            Insurance_File.product_id,
            Product.code,
            PMCaption.caption,
            Insurance_File.lead_agent_cnt,
            Insurance_File_System.date_created,
            Insurance_File_status.code AS status_code,
            Party2.shortname AS insurer_name,
            Insurance_File.this_premium,
            Insurance_File.policy_type_id,
            Policy_Type.description AS policy_type,
            Insurance_File.gemini_policy_status,
            --Insurance_File_type.description AS type_desc,
            Risk_code.description AS type_desc,
            '',
            '',
            '',
            Insurance_File.tax_amount, 
            null,
            Insurance_File.alternate_reference,
            Source.underwriting_branch_ind

            FROM Insurance_File
            INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
            INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
            INNER JOIN Insurance_File_Type ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
            INNER JOIN Product ON Product.product_id = Insurance_File.product_id
            INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id
            INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id
            INNER JOIN #PolList AS PL ON Insurance_File.insurance_file_cnt = PL.insurance_file_cnt
            INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
            INNER JOIN Risk_code On Insurance_file.Risk_code_ID = Risk_Code.Risk_Code_ID
            INNER JOIN Source ON Source.source_id = Insurance_File.source_id
            LEFT OUTER JOIN Party AS Party2 ON Insurance_File.lead_insurer_cnt = Party2.party_cnt
            LEFT OUTER JOIN Insurance_File_Status ON Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id

            WHERE Insurance_file.policy_ignore IS NULL
            AND party.party_cnt = @party_cnt
            AND Insurance_File_Type.code IN ('POLICY', 'MTA PERM', 'MTA TEMP', 'MTAPERMCAN')

            ORDER BY Insurance_File_System.date_created DESC
    END ELSE BEGIN
        -- Do the business
        SELECT Insurance_File.insurance_file_id,
            Insurance_File.source_id AS ins_file_source_id,
            Insurance_File.insurance_file_cnt,
            Insurance_File.insurance_ref,
            Insurance_File_System.last_trans_description AS insurance_folder_code,
            Insurance_File_type.code AS type_code,
            Party.name AS insured_name,
            Party.shortname AS insured_shortname,
            Party.party_id,
            Party.source_id AS party_source_id,
            Insurance_File.renewal_date,
            Insurance_Folder.insurance_holder_cnt,
            Insurance_Folder.insurance_folder_cnt,
            Insurance_File.product_id,
            Product.code,
            PMCaption.caption,
            Insurance_File.lead_agent_cnt,
            Insurance_File_System.date_created,
            Insurance_File_status.code AS status_code,
            Party2.shortname AS insurer_name,
            Insurance_File.this_premium,
            Insurance_File.policy_type_id,
            Policy_Type.description AS policy_type,
            Insurance_File.gemini_policy_status,
            --Insurance_File_type.description AS type_desc,
            Risk_code.description AS type_desc,
            '',
            '',
            '',
            Insurance_File.tax_amount, 
            null,
            Insurance_File.alternate_reference,
            Source.underwriting_branch_ind

            FROM Insurance_File
            INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
            INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
            INNER JOIN Insurance_File_Type ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
            INNER JOIN product ON Product.product_id = Insurance_File.product_id
            INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id
            INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id
            INNER JOIN #PolList AS PL ON Insurance_File.insurance_file_cnt = PL.insurance_file_cnt
            INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
            INNER JOIN Risk_code On Insurance_file.Risk_code_ID = Risk_Code.Risk_Code_ID
            INNER JOIN Source ON Source.source_id = Insurance_File.source_id
            LEFT OUTER JOIN Party AS Party2 ON Insurance_File.lead_insurer_cnt = Party2.party_cnt
            LEFT OUTER JOIN Insurance_File_Status ON Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id

            WHERE Insurance_file.policy_ignore IS NULL
            AND party.party_cnt = @party_cnt
            AND Insurance_File_Type.code LIKE RTRIM(ISNULL(@ins_file_type, '%'))

            ORDER BY Insurance_File_System.date_created DESC
    END

    DROP TABLE #PolList
END
GO
