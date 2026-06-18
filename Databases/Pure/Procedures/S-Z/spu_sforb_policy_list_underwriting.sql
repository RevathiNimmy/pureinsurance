SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sforb_policy_list_underwriting'
GO
CREATE PROCEDURE spu_sforb_policy_list_underwriting
    @ins_file_type char(10),
    @insurance_ref varchar(255) = NULL,
    @source_id smallint = NULL,
    @transaction_type varchar(50) = NULL
AS
BEGIN

	DECLARE @multi_company SMALLINT
	
	IF ISNULL(@transaction_type,'') IN ('NB','MTA')
	BEGIN	
		SELECT @multi_company = SUM(CAST(value AS INT))
		FROM Hidden_Options
		WHERE (option_number = 16 OR option_number = 37)
		AND	value = '1'
		IF @multi_company IS NULL
		BEGIN
			SELECT @multi_company = 0
		END
	END
	ELSE
	BEGIN
		SELECT @multi_company=0
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
            Party2.shortname AS agent_name,
            Insurance_File.this_premium,
            Insurance_File.policy_type_id,
            Policy_Type.description AS policy_type,
            Insurance_File.gemini_policy_status,
            Insurance_File_type.description AS type_desc,
            ''

            FROM Insurance_File
            INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
            INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
            INNER JOIN Insurance_File_Type ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
            INNER JOIN Product ON Product.product_id = Insurance_File.product_id
            INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id
            INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id
            INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
            LEFT OUTER JOIN Party AS Party2 ON Insurance_File.lead_agent_cnt = Party2.party_cnt
            LEFT OUTER JOIN Insurance_File_Status ON Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id

            WHERE Insurance_file.policy_ignore IS NULL
            AND Insurance_File_Type.code IN ('POLICY', 'MTA PERM', 'MTA TEMP')
			AND (Insurance_File.source_id = @source_id OR @multi_company <> 2)
			
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
            Party2.shortname AS agent_name,
            Insurance_File.this_premium,
            Insurance_File.policy_type_id,
            Policy_Type.description AS policy_type,
            Insurance_File.gemini_policy_status,
            Insurance_File_type.description AS type_desc,
            ''

            FROM Insurance_File
            INNER JOIN Insurance_Folder ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
            INNER JOIN Insurance_File_System ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
            INNER JOIN Insurance_File_Type ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
            INNER JOIN product ON Product.product_id = Insurance_File.product_id
            INNER JOIN PMCaption ON PMCaption.caption_id = Product.caption_id
            INNER JOIN Policy_Type ON Insurance_file.policy_type_id = Policy_Type.policy_type_id
            INNER JOIN Party ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
            LEFT OUTER JOIN Party AS Party2 ON Insurance_File.lead_agent_cnt = Party2.party_cnt
            LEFT OUTER JOIN Insurance_File_Status ON Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id

            WHERE Insurance_file.policy_ignore IS NULL
            AND (Insurance_File.source_id = @source_id OR @multi_company <> 2)

            ORDER BY Insurance_File_System.date_created DESC
    END
END
GO
