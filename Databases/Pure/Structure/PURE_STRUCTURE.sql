-- Sirius 1.8.x Database Upgrade file
-- Upgrade file starts

set quoted_identifier on set ansi_nulls on 
go

-- *****************************************************************************
-- Header Template

-- *****************************************************************************
-- * <ISSxxx / PSxxx>: <Enter description here>
-- * Author:   <Your name>
-- * Date:     <Date added>
-- *****************************************************************************

-- *****************************************************************************
-- * Author:        Gaurav Arora
-- * Date:           07 Nov 2007
-- * Purpose:     Add is_manual_description column to Event_Log table
-- *****************************************************************************
EXEC DDLAddColumn 'Event_Log', 'is_manual_description', 'TINYINT NULL'
GO

-- *****************************************************************************
-- * Author:   Andrew Robinson
-- * Date:     05/11/2007
-- * Purpose:  Add tax_amount_editable column to tax_group table
-- *****************************************************************************
EXEC DDLAddColumn 'tax_group', 'is_tax_amount_editable', 'tinyint NULL'
GO
-- *****************************************************************************  
-- * Author:       Deepak Mittal
-- * Date:         23-11-2007
-- * Purpose:      Increased the size of code column in state table
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'State', 'code', 'varchar(10) NOT NULL'
GO

-- *****************************************************************************  
-- * Author:       Danny Davis 
-- * Date:         26-11-2007
-- * Purpose:      Performance Indexes
-- *****************************************************************************
--EXEC DDLAddIndex 'Policy_fee_u','insurance_file_cnt'
--GO


-- *****************************************************************************  
-- * Author:       Danny Davis
-- * Date:         16-11-2007
-- * Purpose:      Indexes added to speed up RI screen display
-- *****************************************************************************
--EXEC DDLAddIndex 'RI_Arrangement_Line', 'grouping'
--GO
--EXEC DDLAddIndex 'RI_Arrangement_Line', 'ri_arrangement_id'
--GO
--EXEC DDLAddIndex 'RI_Arrangement_Line', 'treaty_id'
--GO
--EXEC DDLAddIndex 'RI_Arrangement_Line', 'party_cnt'
--GO

-- *****************************************************************************
-- *        :       Gis Scheme Edi Xsl Filename
-- * Author:        Matthew  Keough-West
-- * Date:          14/11/2007
-- *****************************************************************************
EXEC ddladdcolumn 'gis_scheme', 'edi_xsl_filename', 'varchar(255)'
go

-- *****************************************************************************
-- * Author:   Prabodh Mishra
-- * Date:     04-12-2007
-- * Purpose:  PN #39592
-- *****************************************************************************
EXEC DDLAddOrAlterColumn PFPremiumFinance, ClientPhoneNo, 'VARCHAR(255) NULL' 
GO

-- *****************************************************************************
-- * Author:   Andrew Robinson
-- * Date:     18/12/2007
-- * Purpose:  Nexus Mta - add flag for SSP sub-agent
-- *****************************************************************************
EXEC DDLAddColumn 'party_agent', 'is_ssp_subagent', 'bit NULL'
GO

-- *****************************************************************************
-- * Author:   Andrew Robinson 
-- * Date:     07/08/2007
-- * Purpose:  Add extra_amount_basis to fee_amount table
-- *****************************************************************************
EXEC DDLAddColumn 'fee_amounts', 'extra_amount_basis', 'tinyint NULL'
GO

-- *****************************************************************************
-- * Author: 		Pankaj Kaushik
-- * Date: 		10/01/2008
-- * Purpose:		1810 Sr8 Unattended Renewals
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Renewal_Exception_Reason'
IF @bExists = 0 BEGIN
    CREATE TABLE Renewal_Exception_Reason(
	[renewal_exception_reason_id] INT PRIMARY KEY,
	[code] Varchar(20) NOT NULL,
	[description] Varchar(255) NOT NULL,
	[caption_id] Int NOT NULL,
	[effective_date] Datetime NOT NULL,
	[is_deleted] Tinyint NOT NULL Default 0
	)
END
GO

EXEC DDLAddColumn 'Renewal_Status','renewal_exception_reason_id','Int'
GO
EXEC DDLAddColumn 'Renewal_Status','renewal_exception_notes','Varchar(1024)'
GO
EXEC DDLAddForeignKey 'Renewal_Status', 'renewal_exception_reason_id', @sRefTableName = 'renewal_exception_reason'
GO

-- *****************************************************************************  
-- * Author:       Dan Morey
-- * Date:         01-02-2008
-- * Purpose:      Risk Transfer, RMAR & Contract Certainty Changes.
-- *****************************************************************************
IF NOT EXISTS
    (
        SELECT
            NULL
        FROM sysobjects
        WHERE name = 'party_insurer_risk'
        AND xtype = 'U'
    )
BEGIN
    /*Add new table to hold the FSA details for insurer risks*/
    CREATE TABLE party_insurer_risk
    (
        party_cnt INT NOT NULL,
        risk_code_id INT NOT NULL,
        risk_transfer_agreement BIT NOT NULL,
        delegated_authority BIT NOT NULL
    )
    CREATE INDEX I__party_insurer_risk__party_cnt__risk_code_id ON party_insurer_risk (party_cnt, risk_code_id)
END

/*Add new columns to hold FSA details for extras*/
EXEC DDLAddColumn 'party_extra','risk_transfer_agreement','BIT NULL'
EXEC DDLAddColumn 'party_extra','delegated_authority','BIT NULL'
EXEC DDLAddColumn 'party_extra','fsa_product_id','INT NULL'
    
/*Rename column on risk code as it is now just a default*/
EXEC DDLAddColumn 'risk_code','default_delegated_authority','TINYINT DEFAULT 0 NOT NULL'
GO

IF EXISTS
    (
        SELECT
            NULL
        FROM sysobjects so
        JOIN syscolumns sc
            ON sc.id = so.id
            AND sc.name = 'is_delegated_authority'
        WHERE so.name = 'risk_code'
        AND so.xtype = 'U'
    )
BEGIN
    DECLARE @SQL AS VARCHAR(1000)
    
    SELECT @SQL = 'UPDATE risk_code SET default_delegated_authority = is_delegated_authority'

    EXEC (@SQL)
END

EXEC DDLDropDefault 'risk_code','is_delegated_authority'
EXEC DDLDropColumn 'risk_code','is_delegated_authority'

GO


-- *****************************************************************************
-- * Author: 		Pankaj Kaushik
-- * Date: 		18/01/2008
-- * Purpose:		Renewal Back office Changes
-- *****************************************************************************

EXEC DDLAddColumn 'Product','is_renewable','TINYINT NOT NULL DEFAULT 1'
GO
EXEC DDLAddColumn 'Product','is_renewal_selection_enabled','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','true_monthly_policy_renewal_communication','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','renewal_selection_man_review_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_selection_man_review_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_selection_invite_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_selection_invite_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_selection_update_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_selection_update_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','is_renewal_invite_enabled','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','renewal_invite_man_review_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_invite_man_review_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_invite_invite_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_invite_invite_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_invite_update_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_invite_update_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','is_renewal_update_enabled','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','renewal_update_man_review_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_update_man_review_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_update_invite_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_update_invite_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_update_update_template_id','INT'
GO
EXEC DDLAddColumn 'Product','renewal_update_update_attachment_template_id','INT'
GO
EXEC DDLAddColumn 'Product','is_agent_renewal_selection_enabled','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','is_agent_renewal_invite_enabled','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','is_agent_renewal_update_enabled','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Product','agent_renewal_man_review_template_id','INT'
GO
EXEC DDLAddColumn 'Product','agent_renewal_man_review_report_id','INT'
GO
EXEC DDLAddColumn 'Product','agent_renewal_invite_template_id','INT'
GO
EXEC DDLAddColumn 'Product','agent_renewal_invite_report_id','INT'
GO
EXEC DDLAddColumn 'Product','agent_renewal_update_template_id','INT'
GO
EXEC DDLAddColumn 'Product','agent_renewal_update_report_id','INT'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_selection_man_review_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_selection_man_review_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_selection_invite_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_selection_invite_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_selection_update_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_selection_update_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_invite_man_review_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_invite_man_review_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_invite_invite_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_invite_invite_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_invite_update_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_invite_update_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_update_man_review_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_update_man_review_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_update_invite_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_update_invite_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_update_update_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'renewal_update_update_attachment_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'agent_renewal_man_review_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'agent_renewal_man_review_report_id', @sRefTableName = 'Report'
GO
EXEC DDLAddForeignKey 'Product', 'agent_renewal_invite_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'agent_renewal_invite_report_id', @sRefTableName = 'Report'
GO
EXEC DDLAddForeignKey 'Product', 'agent_renewal_update_template_id', @sRefTableName = 'Document_Template'
GO
EXEC DDLAddForeignKey 'Product', 'agent_renewal_update_report_id', @sRefTableName = 'Report'
GO

-- *****************************************************************************
-- * Author:   Amit Kumar	
-- * Date:     13/02/2008
-- * Purpose:  Add Columns email_sent & email_sent_date to Renewal_Status table
-- *****************************************************************************

EXEC DDLAddColumn 'Renewal_Status','email_sent', 'tinyint NULL'
GO
EXEC DDLAddColumn 'Renewal_Status','email_sent_date',  'datetime NULL'
GO

EXEC DDLAddColumn 'Renewal_Report','insurance_file_cnt',  'INT NULL'
GO
EXEC DDLAddForeignKey 'Renewal_Report', 'insurance_file_cnt', @sRefTableName = 'insurance_file'
GO
-- *****************************************************************************  
-- * Author:     Sumeet Singh
-- * Date:         29-05-2013
-- * Purpose:    Caption irregularity corrected.
-- *****************************************************************************
DECLARE @PMCaption_caption_id INT
DECLARE @PMCaptionIDGen_caption_id INT

SELECT @PMCaption_caption_id = MAX(caption_id) from PMCaption
SELECT @PMCaptionIDGen_caption_id = MAX(caption_id) from PMCaptionIDGen

WHILE (@PMCaption_caption_id <> @PMCaptionIDGen_caption_id)
BEGIN
	INSERT INTO PMCaptionIDGen DEFAULT VALUES
	SET @PMCaptionIDGen_caption_id = @@IDENTITY
END

GO
-- *****************************************************************************  
-- * Author:       Deepak Mittal
-- * Date:         14-02-2008
-- * Purpose:      PVY London Market Development
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Insurer_Type'
IF @bExists = 0 
BEGIN
    CREATE TABLE Insurer_Type (
        insurer_Type_id INTEGER IDENTITY NOT NULL,
        caption_id INTEGER NOT NULL,
		code CHAR(10) NOT NULL,
		description VARCHAR(255),
		is_deleted TINYINT NOT NULL,
		effective_date DATETIME NOT NULL,
		CONSTRAINT PK__insurer_Type_id PRIMARY KEY CLUSTERED (
            insurer_Type_id)
    )

	DECLARE @lCaptionID integer
    EXEC spu_pm_caption_id_return 1, 'Bureau Account', @lCaptionID output
    INSERT INTO Insurer_Type(caption_id, code, description,is_deleted, effective_date)
        VALUES (@lCaptionID, 'BUREAU', 'Bureau Account',0, '2007-01-01')

    EXEC spu_pm_caption_id_return 1, 'Syndicate', @lCaptionID output
    INSERT INTO Insurer_Type(caption_id, code, description,is_deleted, effective_date)
        VALUES (@lCaptionID, 'SYNDICATE', 'Syndicate',0, '2007-01-01')

	EXEC spu_pm_caption_id_return 1, 'Insurer', @lCaptionID output
    INSERT INTO Insurer_Type(caption_id, code, description,is_deleted, effective_date)
        VALUES (@lCaptionID, 'INSURER', 'Insurer',0, '2007-01-01')

	EXEC spu_pm_caption_id_return 1, 'Underwriting Agent', @lCaptionID output
    INSERT INTO Insurer_Type(caption_id, code, description,is_deleted, effective_date)
        VALUES (@lCaptionID, 'UNDERAGENT', 'Underwriting Agent',0, '2007-01-01')
	
	EXEC spu_pm_caption_id_return 1, 'Wholesale Broker', @lCaptionID output
    INSERT INTO Insurer_Type(caption_id, code, description,is_deleted, effective_date)
        VALUES (@lCaptionID, 'WHOLEAGENT', 'Wholesale Broker',0, '2007-01-01')

	EXEC spu_pm_caption_id_return 1, 'Other', @lCaptionID output
    INSERT INTO Insurer_Type(caption_id, code, description,is_deleted, effective_date)
        VALUES (@lCaptionID, 'OTHER', 'Other',0, '2007-01-01')


END
GO

EXEC DDLAddColumn 'party_insurer', 'insurer_type_id', 'INT'
GO

DECLARE @insurer_type_id int
SELECT @insurer_type_id = insurer_type_id FROM insurer_type WHERE code = 'INSURER'
UPDATE party_insurer SET insurer_type_id = @insurer_type_id WHERE insurer_type_id IS NULL
GO

EXEC DDLAddOrAlterColumn 'party_insurer', 'insurer_type_id', 'INT NULL'
GO

EXEC DDLAddForeignKey 'party_insurer', 'insurer_type_id', @sRefTableName = 'Insurer_Type',
@sRefColumnName1='insurer_Type_id'
GO

EXEC DDLAddColumn 'party_insurer', 'BureauAccountParty', 'INT NULL'
GO

EXEC DDLAddForeignKey 'party_insurer', 'BureauAccountParty', @sRefTableName = 'party',
@sRefColumnName1='party_cnt'
GO

EXEC DDLAddColumn 'policy_coinsurers', 'signed_line_percentage', 'Numeric(19,4)'
GO
EXEC DDLAddColumn 'policy_coinsurers', 'linestands', 'TinyInt'
GO
EXEC DDLAddColumn 'policy_coinsurers', 'written_line_percentage', 'Numeric(19,4)'
GO
EXEC DDLAddColumn 'policy_coinsurers', 'signed_line_amount', 'Numeric(19,4)'
GO
EXEC DDLAddColumn 'policy_coinsurers', 'bureau_party_cnt', 'Integer NULL'
GO
EXEC DDLAddColumn 'policy_coinsurers', 'isleadunderwriter', 'TinyInt'
GO


UPDATE policy_coinsurers 
SET signed_line_percentage = 0 
WHERE signed_line_percentage IS NULL

UPDATE policy_coinsurers 
SET linestands = 0 
WHERE linestands IS NULL

UPDATE policy_coinsurers 
SET written_line_percentage = 0 
WHERE written_line_percentage IS NULL

UPDATE policy_coinsurers 
SET signed_line_amount = 0 
WHERE signed_line_amount IS NULL

UPDATE policy_coinsurers 
SET isleadunderwriter = 0 
WHERE isleadunderwriter IS NULL
GO

EXEC DDLAddOrAlterColumn 'policy_coinsurers', 'signed_line_percentage', 'Numeric(19,4) NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'policy_coinsurers', 'linestands', 'TinyInt NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'policy_coinsurers', 'written_line_percentage', 'Numeric(19,4) NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'policy_coinsurers', 'signed_line_amount', 'Numeric(19,4) NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'policy_coinsurers', 'isleadunderwriter', 'TinyInt NOT NULL'
GO
 
EXEC DDLAddColumn 'event_policy_coinsurers', 'signed_line_percentage', 'Numeric(19,4)'
GO
EXEC DDLAddColumn 'event_policy_coinsurers', 'linestands', 'TinyInt'
GO
EXEC DDLAddColumn 'event_policy_coinsurers', 'written_line_percentage', 'Numeric(19,4)'
GO
EXEC DDLAddColumn 'event_policy_coinsurers', 'signed_line_amount', 'Numeric(19,4)'
GO
EXEC DDLAddColumn 'event_policy_coinsurers', 'bureau_party_cnt', 'Integer NULL'
GO
EXEC DDLAddColumn 'event_policy_coinsurers', 'isleadunderwriter', 'TinyInt'
GO

UPDATE event_policy_coinsurers 
SET signed_line_percentage = 0 
WHERE signed_line_percentage IS NULL

UPDATE event_policy_coinsurers 
SET linestands = 0 
WHERE linestands IS NULL

UPDATE event_policy_coinsurers 
SET written_line_percentage = 0 
WHERE written_line_percentage IS NULL

UPDATE event_policy_coinsurers 
SET signed_line_amount = 0 
WHERE signed_line_amount IS NULL

UPDATE event_policy_coinsurers 
SET isleadunderwriter = 0 
WHERE isleadunderwriter IS NULL
GO

EXEC DDLAddOrAlterColumn 'event_policy_coinsurers', 'signed_line_percentage', 'Numeric(19,4) NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'event_policy_coinsurers', 'linestands', 'TinyInt NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'event_policy_coinsurers', 'written_line_percentage', 'Numeric(19,4) NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'event_policy_coinsurers', 'signed_line_amount', 'Numeric(19,4) NOT NULL'
GO
EXEC DDLAddOrAlterColumn 'event_policy_coinsurers', 'isleadunderwriter', 'TinyInt NOT NULL'
GO

-- *****************************************************************************
-- * Author:   Shankh Dhar Dubey
-- * Date:     15/02/2008
-- * Purpose:  PN 41178 - add party context (Risk or Peril) to claims_party_link
-- *****************************************************************************
EXEC DDLAddColumn 'claim_party_link', 'risk_type_id', 'int NULL'
EXEC DDLAddColumn 'claim_party_link', 'peril_type_id', 'int NULL'
GO

-- *****************************************************************************
-- * Author:        Samrendu Bhushan
-- * Date:           15 Feb 2008
-- * Purpose:     Add merchant_id column to Account table
-- *****************************************************************************
EXEC DDLAddColumn 'Account', 'merchant_id', 'varchar(20) NULL'
GO


-- *****************************************************************************
-- * Author:   Aaron Rhodes
-- * Date:     28/11/07
-- * Purpose:  Add extra columns/tables for the new pf connectivity functionality
-- *****************************************************************************
EXEC DDLAddColumn 'pfscheme', 'limittransactions', 'tinyint NULL'
EXEC DDLAddColumn 'pfscheme', 'transactionlimit', 'int NULL'
EXEC DDLAddColumn 'pfscheme', 'xsl_code', 'varchar (10)'

EXEC DDLAddColumn 'PFPremiumFinance', 'scheme_type', 'varchar(50)'
EXEC DDLAddColumn 'PFPremiumFinance', 'scheme_code', 'varchar(3)'

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME LIKE 'PFMessageHeader')
BEGIN
    CREATE TABLE PFMessageHeader
    (
    pfmessageheader_id INT IDENTITY(1,1),
    xsl_code VARCHAR(10),
    header_name VARCHAR(255),
    header_value VARCHAR(255),
    request_type INT
    )
END

-- *****************************************************************************  
-- * Author:      Deepak
-- * Date:        28-02-2008
-- * Purpose:     LOA - SMS Support 
-- *****************************************************************************

EXEC DDLAddColumn 'Event_log','batch_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=Event_log,@sColumnName1=batch_id,  
     @sRefTableName=Batch, @sRefColumnName1=Batch_id  
GO

-- *****************************************************************************  
-- * Author:      Gurucharan
-- * Date:        28-02-2008
-- * Purpose:     Party View 
-- *****************************************************************************
EXEC DDLAddColumn 'user_authorities', 'is_view_only_client_manager', 'tinyint NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'user_authorities', 'is_view_only_agents_maintenance', 'tinyint NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'user_authorities', 'is_view_only_account_handler_maintenance', 'tinyint NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'user_authorities', 'is_view_only_account_executive_maintenance', 'tinyint NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'user_authorities', 'is_view_only_insurer_maintenance', 'tinyint NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'user_authorities', 'is_view_only_other_party_maintenance', 'tinyint NOT NULL DEFAULT 0'
GO

-- *****************************************************************************
-- * Add new fields for Third Party Stargate PFSchemes
-- * Author:   Danny Davis
-- * Date:     21/12/2004
-- *****************************************************************************
EXECUTE DDLAddColumn 'PFScheme', 'provider_prem_threshold', 'INT NULL'
GO

-- *****************************************************************************
-- * Author:   Daniel Morey
-- * Date:     06/03/2008
-- * Purpose:  Added indexes to help prevent deadlocks.
-- *****************************************************************************
--EXEC DDLAddIndex 'insurance_cob_section','insurance_file_cnt'
--EXEC DDLAddIndex 'event_insurance_cob_section','insurance_file_cnt'
--GO
-- *****************************************************************************
-- * Author:   Deepak Arora	 
-- * Date:     10-03-2008
-- * Purpose:  Claims Payment Enhancement
-- *****************************************************************************	

EXEC DDLAddColumn 'Product','multiple_claims_payments','tinyint NULL'
EXEC DDLAddColumn 'Product','max_unauthorised_claim_value','Money NULL'
EXEC DDLAddColumn 'Product','max_unauthorised_no_claim_payments','tinyint NULL'
EXEC DDLAddColumn 'Product','run_authorisation_scripts_claim_payments','tinyint NULL'

GO

-- *****************************************************************************
-- * Author:   Daniel Morey
-- * Date:     06/03/2008
-- * Purpose:  Make insurance_section_id of the Event_Insurance_COB_Section table an identity column.
-- *****************************************************************************

CREATE TABLE dbo.Tmp_Event_Insurance_COB_Section
	(
	Insurance_file_cnt int NOT NULL,
	Insurance_section_id int NOT NULL IDENTITY (1, 1),
	COB_Rating_section_id int NULL,
	Premium_Excluding_Tax numeric(19, 4) NULL,
	Tax_applied numeric(19, 4) NULL,
	Premium_Including_Tax numeric(19, 4) NULL,
	Tax_group_id int NULL,
	Commission_Cnt int NULL,
	Commission_Percentage numeric(19, 4) NULL,
	Commission_Charge numeric(19, 4) NULL,
	Commission_Net numeric(19, 4) NULL,
	Commission_tax_applied numeric(19, 4) NULL,
	Commission_Payable numeric(19, 4) NULL,
	Commission_Tax_group_id int NULL,
	Is_minimum_brokerage tinyint NULL,
	Override_rate_table tinyint NULL,
	Base_Premium_Excluding_Tax numeric(19, 4) NULL,
	Base_Tax_Applied numeric(19, 4) NULL,
	Base_Premium_Including_Tax numeric(19, 4) NULL,
	Base_Commission_Charge numeric(19, 4) NULL,
	Base_Commission_Net numeric(19, 4) NULL,
	Base_Commission_Tax_Applied numeric(19, 4) NULL,
	Base_Commission_Payable numeric(19, 4) NULL,
	is_applied bit NULL
	)  ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_Event_Insurance_COB_Section ON
GO
IF EXISTS(SELECT * FROM dbo.Event_Insurance_COB_Section)
	 EXEC('INSERT INTO dbo.Tmp_Event_Insurance_COB_Section (Insurance_file_cnt, Insurance_section_id, COB_Rating_section_id, Premium_Excluding_Tax, Tax_applied, Premium_Including_Tax, Tax_group_id, Commission_Cnt, Commission_Percentage, Commission_Charge, Commission_Net, Commission_tax_applied, Commission_Payable, Commission_Tax_group_id, Is_minimum_brokerage, Override_rate_table, Base_Premium_Excluding_Tax, Base_Tax_Applied, Base_Premium_Including_Tax, Base_Commission_Charge, Base_Commission_Net, Base_Commission_Tax_Applied, Base_Commission_Payable, is_applied)
		SELECT Insurance_file_cnt, Insurance_section_id, COB_Rating_section_id, Premium_Excluding_Tax, Tax_applied, Premium_Including_Tax, Tax_group_id, Commission_Cnt, Commission_Percentage, Commission_Charge, Commission_Net, Commission_tax_applied, Commission_Payable, Commission_Tax_group_id, Is_minimum_brokerage, Override_rate_table, Base_Premium_Excluding_Tax, Base_Tax_Applied, Base_Premium_Including_Tax, Base_Commission_Charge, Base_Commission_Net, Base_Commission_Tax_Applied, Base_Commission_Payable, is_applied FROM dbo.Event_Insurance_COB_Section TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_Event_Insurance_COB_Section OFF
GO
DROP TABLE dbo.Event_Insurance_COB_Section
GO
EXECUTE sp_rename N'dbo.Tmp_Event_Insurance_COB_Section', N'Event_Insurance_COB_Section', 'OBJECT'
GO
ALTER TABLE dbo.Event_Insurance_COB_Section ADD CONSTRAINT
	PK__Event_Insurance_COB_Section PRIMARY KEY CLUSTERED 
	(
	Insurance_file_cnt,
	Insurance_section_id
	) ON [PRIMARY]

GO

-- *****************************************************************************
-- * Author:   Gurucharan Gulati
-- * Date:     10/03/2008
-- * Purpose:  Maintain Party Numbering Scheme
-- *****************************************************************************

EXEC DDLAddColumn 'Party_Type','is_on_numbering_scheme','TINYINT NOT NULL DEFAULT 0'
GO

EXEC DDLAddColumn 'Numbering_Scheme','party_type_id','SMALLINT NULL'
GO

EXEC DDLAddColumn 'Numbering_Scheme','is_read_only','TINYINT NOT NULL DEFAULT 0'
GO

EXEC DDLAddForeignKey 'Numbering_Scheme', 'party_type_id', @sRefTableName = 'Party_Type',@sRefColumnName1='party_type_id'
GO

EXEC DDLDropForeignKey 'party_type', 'number_scheme'
GO

Exec DDLDropColumn 'party_type','number_scheme'
Go


-- *****************************************************************************
-- * Author:   Deepak Arora	 
-- * Date:     12-03-2008
-- * Purpose:  Incurred Reserves Authority
-- *****************************************************************************

EXEC DDLAddColumn 'user_authorities','is_recommender','tinyint NULL'
EXEC DDLAddColumn 'user_authorities','recommender_currency_id','int NULL'
EXEC DDLAddColumn 'user_authorities','recommender_currency_amount','Money NULL'

GO

-- *****************************************************************************
-- * Author: 		Pankaj Kaushik
-- * Date: 		06/03/2008
-- * Purpose:		Renewal Printing
-- *****************************************************************************
EXEC DDLAddColumn 'PMB_Doc_Link','process_types_docs_id','INT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','functional_area','TINYINT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','product_id','INT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','source_id','INT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','is_client','TINYINT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','is_agent','TINYINT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','is_office','TINYINT'
GO
EXEC DDLAddColumn 'PMB_Doc_Link','production_order','TINYINT'
GO
EXEC DDLAddForeignKey 'PMB_Doc_Link', 'process_types_docs_id', @sRefTableName = 'Process_Types_Docs'
GO
EXEC DDLAddForeignKey 'PMB_Doc_Link', 'product_id', @sRefTableName = 'Product'
GO
EXEC DDLAddForeignKey 'PMB_Doc_Link', 'source_id', @sRefTableName = 'Source'
GO
EXEC DDLAddColumn 'Document_Spooler','is_client','TINYINT'
GO
EXEC DDLAddColumn 'Document_Spooler','is_agent','TINYINT'
GO
EXEC DDLAddColumn 'Document_Spooler','is_office','TINYINT'
GO
EXEC DDLAddColumn 'Document_Spooler','production_order','TINYINT NOT NULL DEFAULT 1'
GO

-- *****************************************************************************
-- * Author:        Sumit Bhardwaj
-- * Date:          13/03/2008
-- * Purpose:       Length increased
-- *****************************************************************************
execute DDLAddOrAlterColumn 'report_transaction', 'account_name2', 'varchar(255) NULL'
go

-- *****************************************************************************
-- * Author:        Amit Kumar
-- * Date:          14/03/2008
-- * Purpose:       PN42201 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'PFRF', 'MinInterest', 'NUMERIC(19,6)'
GO

-- *****************************************************************************
-- * Author:        Gurucharan Gulati
-- * Date:          14/03/2008
-- * Purpose:       Payment Maintenance 
-- *****************************************************************************

EXEC DDLDropColumn 'user_authorities','is_reverse_allocation'
GO
EXEC DDLDropColumn 'user_authorities','timeperiod_reverse_allocation'
GO
EXEC DDLAddColumn 'user_authorities','allow_reverse_allocations','tinyint NULL'
GO
EXEC DDLAddColumn 'user_authorities','reverse_allocations_days','smallint NULL'
GO

-- *****************************************************************************
-- * Author:        Samrendu Bhushan
-- * Date:          19/03/2008 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'PFRF', 'Rate1', 'NUMERIC(19,6)'
GO
EXEC DDLAddOrAlterColumn 'PFRF', 'Rate2', 'NUMERIC(19,6)'
GO
EXEC DDLAddOrAlterColumn 'PFRF', 'Rate3', 'NUMERIC(19,6)'
GO
EXEC DDLAddOrAlterColumn 'PFRF', 'Rate4', 'NUMERIC(19,6)'
GO
EXEC DDLAddOrAlterColumn 'PFRF', 'Rate5', 'NUMERIC(19,6)'
GO
-- *****************************************************************************
-- * Author:   Deepak Arora	 
-- * Date:     03-04-2008
-- * Purpose:  Incurred Reserves Authority
-- *****************************************************************************	

EXEC DDLAddColumn 'claim_payment','is_referred_for_recommendation','tinyint NULL'
EXEC DDLAddColumn 'claim_payment','recommended_by','smallint NULL'

EXEC DDLAddForeignKey 'claim_payment', 'recommended_by', @sRefTableName = 'pmuser',
@sRefColumnName1='user_id'

GO

IF NOT EXISTS
    (
        SELECT NULL
        FROM sysobjects
        WHERE name = 'cashlistitem_claim_link'
        AND xtype = 'U'
    )
BEGIN
    CREATE TABLE CashListItem_Claim_Link
    (
        cashlistitem_claim_link_id int IDENTITY(1,1),
        claim_payment_id int CONSTRAINT FK__Claim_Payment__claim_payment_id FOREIGN KEY References Claim_payment(Claim_payment_id),
        claim_receipt_id int CONSTRAINT FK__Claim_Receipt__claim_Receipt_id FOREIGN KEY References Claim_receipt(Claim_receipt_id),
        cashlistitem_id int NOT NULL CONSTRAINT FK__CashListItem__CashListItem_id FOREIGN KEY References CashListItem(CashListItem_id),
        is_deleted int NOT NULL   
    )

END

GO
-- *****************************************************************************
-- * Author:        Gurucharan Gulati
-- * Date:          02/04/2008
-- * Purpose:       Payment Maintenance
-- *****************************************************************************
EXEC DDLAddColumn 'CashListItem','cashlistitem_reversal_transdetail_id','INT NULL'
GO
EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_reversal_transdetail_id',
@sRefTableName = 'TransDetail',@sRefColumnName1 = 'transdetail_id'
GO


-- *****************************************************************************
-- * PN37247:  password_change_date field is not updated
-- * Author:   Rajesh Choudhary
-- * Date:     08 Apr 2008
-- *****************************************************************************

EXECUTE DDLDropTrigger 'PMUser_tru'
GO
CREATE TRIGGER PMUser_tru ON PMUser FOR UPDATE
AS
BEGIN

  IF UPDATE(password)
  BEGIN
    UPDATE pmuser set password_change_date = getdate() where user_id = (select user_id from inserted)
  END

END
GO

-- *****************************************************************************
-- * Author:        VineetChoudhary
-- * Date:          10/04/2008
-- * Purpose:       Column added in user_authorities for Reverse Replace Transaction
-- *****************************************************************************
Execute DDLAddColumn @sTableName='user_authorities',@sColumnName='can_reverse_and_replace_transactions',@sColumnDefinition=TinyInt
GO

-- *****************************************************************************
-- * Author:   Vijay Bhushan
-- * Date:     12/03/2008
-- * Purpose:  element_name field size changed (Electra M3 Gaps Phase 1 - Banking Process )
---- *****************************************************************************
--EXEC DDLDropIndex 'element','element_name'
--EXEC DDLAddOrAlterColumn 'element', 'element_name', 'char(255)'
--EXEC DDLAddIndex 'element','element_name'
GO

-- *****************************************************************************
-- * Author:        Vijay Bhushan
-- * Date:          12/03/2008
-- * Purpose:       New column added in Bank table (Electra M3 Gaps Phase 1 - Banking Process )
-- *****************************************************************************
EXEC DDLAddColumn 'bank', 'bank_account_type_id', 'INT'
GO
UPDATE bank SET bank_account_type_id= 1 WHERE bank_account_type_id IS NULL
GO
EXEC DDLAddOrAlterColumn 'bank', 'bank_account_type_id', 'INT NOT NULL'
GO

-- *****************************************************************************
-- * Author:        Vijay Bhushan
-- * Date:          12/03/2008
-- * Purpose:       New column added in BankAccount table (Electra M3 Gaps Phase 1 - Banking Process )
-- *****************************************************************************
EXEC DDLAddColumn 'bankaccount', 'default_bank_account_id', 'INT'
GO
IF NOT EXISTS(SELECT NULL FROM SYSOBJECTS WHERE NAME LIKE 'Bank_Account_Type')
BEGIN
CREATE TABLE Bank_Account_Type
(
    bank_account_type_id INT NOT NULL PRIMARY KEY,
    code VARCHAR(10) NOT NULL,
    description VARCHAR(50),
    caption_id INT NOT NULL,
    effective_date DATETIME NOT NULL,
    is_deleted TINYINT NOT NULL
)
END
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     18/04/2008
-- * Purpose:  Add pfmediatype_history_id to PFInstalments table
--             Column added to support BACS Development. The Purpose is to
--	       keep the history id for 0C and 0N enteries to be used at the time
-- 	       of instalment export
-- *****************************************************************************
EXEC DDLADDCOLUMN 'PFInstalments', 'pfmediatype_history_id','INTEGER NULL'
GO

EXEC DDLAddForeignKey @sTableName='PFInstalments',@sColumnName1=pfmediatype_history_id,@sRefTableName='PFMediaTypeHistory',@sRefColumnName1=pfMediaTypeHistory_id
GO

-- *****************************************************************************
-- * Author:        Gaurav Arora
-- * Date:          24 Apr 2008
-- * Purpose:
-- *                Add table to PMLookup
-- *****************************************************************************
If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Bank_Account_Type' AND xtype = 'U')
BEGIN
    CREATE TABLE Bank_Account_Type
    (
        bank_account_type_id 		INT 		PRIMARY KEY IDENTITY,
        caption_id 			INT 		NOT NULL,
        is_deleted 			TINYINT 	NOT NULL,
        effective_date 			DATETIME 	NOT NULL,
        description 			VARCHAR(255) 	NOT NULL,
        code 				VARCHAR(10)	NOT NULL
    )
End
GO
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Bank_Account_Type')
Begin
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'Bank_Account_Type', 3, 1)
End
GO

-- *****************************************************************************
-- * Author:        Gaurav Arora
-- * Date:          24 Apr 2008
-- * Purpose:       Bank_Payment_Type
-- *****************************************************************************
if NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Bank_Payment_Type')
BEGIN
    CREATE TABLE Bank_Payment_Type
    (
        bank_payment_type_id 		INT 		PRIMARY KEY IDENTITY,
        caption_id 			INT 		NOT NULL,
        is_deleted 			TINYINT 	NOT NULL,
        effective_date 			DATETIME 	NOT NULL,
        description 			VARCHAR(255) 	NOT NULL,
        code 				VARCHAR(10)	NOT NULL
    )
End
GO
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Bank_Payment_Type')
Begin
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'Bank_Payment_Type', 3, 1)
End
GO


-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         24 Apr 2008
-- * Purpose:      Party Bank Details
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Party_Bank'
IF @bExists = 0
BEGIN
    CREATE TABLE Party_Bank

	(
		party_bank_id		INT		NOT NULL IDENTITY,
		is_bank			TINYINT		NOT NULL,
		account_id		INT		NOT NULL,
		bank_payment_type_id	INT		NOT NULL,
		bank_account_type_id	INT,
		account_holder_name	VARCHAR(50)	NOT NULL,	
		account_number		VARCHAR(50),
		bank_name_id		INT,
		bank_branch		VARCHAR(50),
		bank_branch_code	VARCHAR(50),
		bank_add1		VARCHAR(40),
		bank_add2		VARCHAR(40),
		bank_add3		VARCHAR(40),
		bank_town		VARCHAR(40),
		bank_pcode		VARCHAR(20),
		bank_region		VARCHAR(40),
		bank_country		VARCHAR(30),
		cc_num			VARCHAR(30),
		cc_start_date		VARCHAR(10),
		cc_expiry_date		VARCHAR(10),
		cc_issue_num		VARCHAR(2),
		cc_pin			VARCHAR(20),
		is_registered		TINYINT,
		cc_add1			VARCHAR(100),
		cc_add2			VARCHAR(100),
		cc_add3			VARCHAR(100),
		cc_town			VARCHAR(100),
		cc_pcode		VARCHAR(20),
		cc_country		VARCHAR(30),
		is_deleted		TINYINT

		CONSTRAINT PK__party_bank_id PRIMARY KEY CLUSTERED (party_bank_id)
		CONSTRAINT FK__Party_Bank__account_id FOREIGN KEY (account_id) REFERENCES account(account_id),
		CONSTRAINT FK__Party_Bank__bank_name_id FOREIGN KEY (bank_name_id) REFERENCES cashlistitem_bank(cashlistitem_bank_id),
		CONSTRAINT FK__Party_Bank__bank_payment_type_id FOREIGN KEY (bank_payment_type_id) REFERENCES bank_payment_type(bank_payment_type_id),
		CONSTRAINT FK__Party_Bank__bank_account_type_id FOREIGN KEY (bank_account_type_id) REFERENCES bank_account_type(bank_account_type_id)
	)

END
GO
-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         24 Apr 2008
-- * Purpose:      Party Bank Details
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'party_bank_history'
IF @bExists = 0
BEGIN
    CREATE TABLE Party_Bank_History

	(
		party_bank_history_id	INT IDENTITY,
		party_bank_id		INT		NOT NULL,
		action_code		VARCHAR(50),
		account_id		INT		NOT NULL,
		bank_payment_type_id	INT		NOT NULL,
		bank_account_type_id	INT,
		account_holder_name	VARCHAR(50)	NOT NULL,	
		account_number		VARCHAR(50),
		bank_name_id		INT,
		bank_branch		VARCHAR(50),
		bank_branch_code	VARCHAR(50),
		bank_add1		VARCHAR(40),
		bank_add2		VARCHAR(40),
		bank_add3		VARCHAR(40),
		bank_town		VARCHAR(40),
		bank_pcode		VARCHAR(20),
		bank_region		VARCHAR(40),
		bank_country		VARCHAR(30),
		cc_num			VARCHAR(30),
		cc_start_date		VARCHAR(10),
		cc_expiry_date		VARCHAR(10),
		cc_issue_num		VARCHAR(2),
		cc_pin			VARCHAR(20),
		is_registered		TINYINT,
		cc_add1			VARCHAR(100),
		cc_add2			VARCHAR(100),
		cc_add3			VARCHAR(100),
		cc_town			VARCHAR(100),
		cc_pcode		VARCHAR(20),
		cc_country		VARCHAR(30),
		user_id			INT,
		date_modified		DATETIME

		CONSTRAINT PK__party_bank_history_id PRIMARY KEY CLUSTERED (party_bank_history_id)

		CONSTRAINT FK__Party_Bank_History__account_id FOREIGN KEY (account_id) REFERENCES account(account_id),
		CONSTRAINT FK__Party_Bank_History__bank_name_id FOREIGN KEY (bank_name_id) REFERENCES cashlistitem_bank(cashlistitem_bank_id),
		CONSTRAINT FK__Party_Bank_History__bank_payment_type_id FOREIGN KEY (bank_payment_type_id) REFERENCES bank_payment_type(bank_payment_type_id),
		CONSTRAINT FK__Party_Bank_History__bank_account_type_id FOREIGN KEY (bank_account_type_id) REFERENCES bank_account_type(bank_account_type_id)
	)
END
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     18/04/2008
-- * Purpose:  Add bank_payment_type_id to PFPremiumFinance table
-- *****************************************************************************
EXEC DDLADDCOLUMN 'PFPremiumFinance','bank_payment_type_id','INTEGER NULL'
GO

EXEC DDLAddForeignKey @sTableName='PFPremiumFinance',@sColumnName1=bank_payment_type_id,@sRefTableName='bank_payment_type',@sRefColumnName1=bank_payment_type_id
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     25/04/2008
-- * Purpose:  Add bank_payment_type_id to CashListItem table
-- *****************************************************************************
EXEC DDLADDCOLUMN 'CashListItem','bank_payment_type_id','INTEGER NULL'
GO

EXEC DDLAddForeignKey @sTableName='CashListItem',@sColumnName1=bank_payment_type_id,@sRefTableName='bank_payment_type',@sRefColumnName1=bank_payment_type_id
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     25/04/2008
-- * Purpose:  Add bank_payment_type_id to CashListItem table
-- *****************************************************************************
EXEC DDLADDCOLUMN 'Claim_Payment','bank_payment_type_id','INTEGER NULL'
GO

EXEC DDLAddForeignKey @sTableName='Claim_Payment',@sColumnName1=bank_payment_type_id,@sRefTableName='bank_payment_type',@sRefColumnName1=bank_payment_type_id
GO

-- *****************************************************************************
-- * Author:   Ashish Sachdeva
-- * Date:     06/05/2008
-- * Purpose:  PN 43988 Alter Foreign Key ON DELETE CASCADE
-- *****************************************************************************
EXEC DDLDropForeignKey @sTableName='PFMediaTypeHistory',
					@sColumnName1='pfprem_finance_cnt',
					@sColumnName2='pfprem_finance_version'	
GO

IF EXISTS(SELECT NULL FROM sysobjects WHERE name ='PFMediaTypeHistory' AND type='U')
BEGIN
ALTER TABLE PFMediaTypeHistory WITH NOCHECK ADD CONSTRAINT [FK_PFMediaTypeHistory_PFPremiumFinance] FOREIGN KEY 
	(
		[pfprem_finance_cnt],
		[pfprem_finance_version]
	) REFERENCES [PFPremiumFinance] (
		[pfprem_finance_cnt],
		[pfprem_finance_version]
	) ON DELETE CASCADE

END
GO

-- *****************************************************************************
-- * Author:   Krishan Kumar Gorav
-- * Date:     07/05/2008
-- * Purpose:  PN 44031 Remove identity constraint from id columns of tables Claims_Type_basis,Claims_Cover_basis
-- *****************************************************************************
EXEC DDLDropTable 'Tmp_Claims_Type_basis'
GO

if(SELECT count(*) FROM syscolumns WHERE status <> 128 AND 
	id = (SELECT id FROM sysobjects WHERE name = 'Claims_Type_basis') AND name='Claims_Type_basis_id')=0
BEGIN
	CREATE TABLE dbo.Tmp_Claims_Type_basis
		(
		Claims_type_basis_id int NOT NULL,
		Code varchar(10) NOT NULL,
		Description varchar(50) NULL,
		Caption_id int NOT NULL,
		Effective_date datetime NOT NULL,
		Is_Deleted tinyint NOT NULL
		)  ON [PRIMARY]
	
	IF EXISTS(SELECT * FROM dbo.Claims_Type_basis)
		 EXEC('INSERT INTO dbo.Tmp_Claims_Type_basis (Claims_type_basis_id, Code, Description, Caption_id, Effective_date, Is_Deleted)
			SELECT Claims_type_basis_id, Code, Description, Caption_id, Effective_date, Is_Deleted FROM dbo.Claims_Type_basis (HOLDLOCK TABLOCKX)')
	
	EXEC DDLDropConstraint 'Product','Claims_Type_basis_ID'
	
	EXEC DDLDropTable 'Claims_Type_basis'
	
	EXECUTE sp_rename N'dbo.Tmp_Claims_Type_basis', N'Claims_Type_basis', 'OBJECT'
	
	EXEC DDLADDPrimaryKey 'Claims_Type_basis','Claims_type_basis_id'

	EXEC DDLAddForeignKey @sTableName='Product', @sColumnName1='Claims_type_basis_id', @sRefTableName = 'Claims_Type_basis',@sRefColumnName1='Claims_type_basis_id'
	
END
GO

EXEC DDLDropTable 'Tmp_Claims_Cover_basis'
GO
if(SELECT count(*) FROM syscolumns WHERE status <> 128 AND 
	id = (SELECT id FROM sysobjects WHERE name = 'Claims_Cover_basis') AND name='Claims_Cover_basis_id')=0
BEGIN
	CREATE TABLE dbo.Tmp_Claims_Cover_basis
		(
		Claims_Cover_basis_id int NOT NULL,
		Code varchar(10) NOT NULL,
		Description varchar(50) NULL,
		Caption_id int NOT NULL,
		Effective_date datetime NOT NULL,
		Is_Deleted tinyint NOT NULL
		)  ON [PRIMARY]

	IF EXISTS(SELECT * FROM dbo.Claims_Cover_basis)
		 EXEC('INSERT INTO dbo.Tmp_Claims_Cover_basis (Claims_Cover_basis_id, Code, Description, Caption_id, Effective_date, Is_Deleted)
			SELECT Claims_Cover_basis_id, Code, Description, Caption_id, Effective_date, Is_Deleted FROM dbo.Claims_Cover_basis (HOLDLOCK TABLOCKX)')
	
	ALTER TABLE dbo.Product
		DROP CONSTRAINT FK__Product__Claims_Cover_basis_ID
	
	DROP TABLE dbo.Claims_Cover_basis
	
	EXECUTE sp_rename N'dbo.Tmp_Claims_Cover_basis', N'Claims_Cover_basis', 'OBJECT'
	
	EXEC DDLADDPrimaryKey 'Claims_Cover_basis','Claims_Cover_basis_id'
	
	EXEC DDLAddForeignKey @sTableName='Product', @sColumnName1='Claims_Cover_basis_id', @sRefTableName = 'Claims_Cover_basis',@sRefColumnName1='Claims_Cover_basis_id'
END
GO

-- *****************************************************************************
-- * Author:        Shankh Dhar Dubey
-- * Date:          20/05/2008
-- * Purpose:       PN 43658
-- *****************************************************************************
/*Rename column on risk code as it is now just a default*/
EXEC DDLAddColumn 'risk_code','is_delegated_authority','TINYINT DEFAULT 0 NOT NULL'
GO

IF EXISTS
    (
        SELECT
            NULL
        FROM sysobjects so
        JOIN syscolumns sc
            ON sc.id = so.id
            AND sc.name = 'default_delegated_authority'
        WHERE so.name = 'risk_code'
        AND so.xtype = 'U'
    )
BEGIN
    DECLARE @SQL AS VARCHAR(1000)
    
    SELECT @SQL = 'UPDATE risk_code SET is_delegated_authority = default_delegated_authority'

    EXEC (@SQL)
END

EXEC DDLDropDefault 'risk_code','default_delegated_authority'
EXEC DDLDropColumn 'risk_code','default_delegated_authority'

GO

-- *****************************************************************************
-- * Author:        Gurucharan Gulati
-- * Date:          22/05/2008
-- * Purpose:       Party Bank
-- *****************************************************************************
EXEC DDLAddColumn 'party_bank', 'name_on_card', 'varchar(100) NULL'
GO

EXEC DDLAddColumn 'party_bank', 'manual_auth_number', 'varchar(50) NULL'
GO

EXEC DDLAddColumn 'party_bank_history', 'name_on_card', 'varchar(100) NULL'
GO

EXEC DDLAddColumn 'party_bank_history', 'manual_auth_number', 'varchar(50) NULL'
GO

-- *****************************************************************************  
-- * Author:      Roopaly Rastogi
-- * Date:        22-05-2008
-- * Purpose:     1.12 WR25 - Change product at renewal
-- *****************************************************************************

EXEC DDLAddColumn 'Insurance_file','renewal_product_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=Insurance_file,@sColumnName1=renewal_product_id,  
     @sRefTableName=Product, @sRefColumnName1=Product_id  
GO

EXEC DDLAddColumn 'Insurance_file','original_product_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=Insurance_file,@sColumnName1=original_product_id,  
     @sRefTableName=Product, @sRefColumnName1=Product_id  
GO

-- *****************************************************************************
-- * Author: 		Pankaj Kaushik
-- * Date: 		    20/05/2008
-- * Purpose:		Account Function & CCY Cash Allocation
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'BankAccount_Source'
IF @bExists = 0 BEGIN
    CREATE TABLE BankAccount_Source(
	[bankaccount_id] INT,
	[source_id] INT
	)
END
GO

EXEC DDLAddForeignKey 'BankAccount_Source', 'bankaccount_id', @sRefTableName = 'bankaccount'
GO
EXEC DDLAddForeignKey 'BankAccount_Source', 'source_id', @sRefTableName = 'source'
GO

EXEC DDLAddColumn 'BankAccount', 'is_cash_receive_in_this_currency_only','TINYINT NOT NULL Default 0'
GO

-- *****************************************************************************
-- * Author:        Sandeep Raheja
-- * Date:          27/05/2008
-- * Purpose:       Add Risk Transfer Editable flag
-- *****************************************************************************
exec DDLAddColumn 'Insurance_File', 'risk_transfer_editable', 'Bit NULL'
exec DDLAddColumn 'Policy_coinsurers', 'risk_transfer_editable', 'Bit NULL'
exec DDLAddColumn 'Event_Policy_coinsurers', 'risk_transfer_editable', 'Bit NULL'
exec DDLAddColumn 'Event_Insurance_File', 'risk_transfer_editable', 'Bit NULL'
GO

-- *****************************************************************************
-- * Author:        Deepak Arora
-- * Date:          20/05/2008
-- * Purpose:       WR5 - Claims WorkFlow
-- *****************************************************************************

--Add column in progress status lookup table
EXEC DDLAddColumn 'progress_Status', 'is_claim_payment_valid', 'tinyint NULL' 

EXEC DDLAddColumn 'Product', 'bankAccount_Id', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claim_Value_For_Large_Loss_Advice', 'numeric(19, 4) NULL' 
EXEC DDLAddColumn 'Product', 'inclusion_of_CoInsurers_On_Claims', 'tinyint NULL' 
EXEC DDLAddColumn 'Product', 'allow_Negative_Reserve', 'tinyint NULL' 
EXEC DDLAddColumn 'Product', 'ext_Clm_Handler_Acknowledged_Task_Allowed_Time', 'numeric(3) NULL' 
EXEC DDLAddColumn 'Product', 'ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time', 'numeric(3) NULL' 
EXEC DDLAddColumn 'Product', 'valid_Policy_Version_At_Loss_Date', 'tinyint NULL' 

EXEC DDLAddColumn 'Product', 'is_Gross_Claim_Payment_Amount', 'tinyint NULL' 
EXEC DDLAddColumn 'Product', 'claim_Task_Group', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claim_User_Group', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claims_UDT_A', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claims_UDT_B', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claims_UDT_C', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claims_UDT_D', 'int NULL' 
EXEC DDLAddColumn 'Product', 'claims_UDT_E', 'int NULL' 
EXEC DDLAddColumn 'Product', 'is_Duplicate_Claim_Check_Enabled', 'tinyint NULL'
EXEC DDLAddColumn 'Product', 'is_Advanced_Tax_Script_Enabled', 'tinyint NULL' 
EXEC DDLAddColumn 'Product', 'is_Payment_Ref_Check_Enabled', 'tinyint NULL' 

EXEC DDLDropColumn 'Product', 'is_Recommender'
EXEC DDLAddColumn 'Product', 'is_Recommend_Claim_Payments', 'tinyint NULL' 




IF NOT EXISTS
    (
        SELECT NULL
        FROM sysobjects
        WHERE name = 'Product_Claims_Workflow'
        AND xtype = 'U'
    )
BEGIN

    CREATE TABLE Product_Claims_Workflow(
	[product_id] int NOT NULL,
	[claim_process_type_id] int NOT NULL,
	[check_unpaid_status] tinyint,
	[reinsurance_recovery] tinyint,
	[salvage_recovery] tinyint,
	[third_party_recovery] tinyint,
	[external_claim_handling] tinyint,
	[description_for_change_in_reserve] tinyint,
	[claim_notification_doc_message] tinyint,
	[generate_claim_notification_doc] tinyint,
	[claim_payment_process] tinyint,
	[check_deferred_reinsurance] tinyint,
	[fast_track_claims] tinyint,
	[reinsurance_payment] tinyint, 
	[description_for_change_in_payment] tinyint,
	[cash_payment_process] tinyint,
	[claim_payment_doc_message] tinyint,
	[generate_claim_payment_doc] tinyint,
	[make_further_payments] tinyint
	)

    EXEC DDLAddPrimaryKey 'Product_Claims_Workflow', 'claim_process_type_id', 'product_id'
    EXEC DDLAddForeignKey 'Product_Claims_Workflow', 'product_id', @sRefTableName = 'product'
END
GO

-- *****************************************************************************
-- * Author:   Krishan Kumar Gorav
-- * Date:     30/05/2008
-- * Purpose:  Scalability Changes(Creating Identity and drop constraints)
-- *****************************************************************************
DDLDropAlternateKey 'Address','address_id','Source_id',null,null,null,null,null,null,1
GO

--DDLDropIndex 'Insurance_File','Insurance_File_id','Source_id',null,null,null,null,null,null,1
--GO

DDLDropAlternateKey 'Insurance_Folder','Insurance_Folder_id','Source_id',null,null,null,null,null,null,1
GO

DDLDropAlternateKey 'Contact','contact_id','source_id',null,null,null,null,null,null,1
GO

DDLDropAlternateKey 'Transaction_Export_Folder','transaction_export_folder_id','source_id',null,null,null,null,null,null,1
GO

DDLDropAlternateKey 'Party','party_id','source_id',null,null,null,null,null,null,1
GO

EXEC DDLDropTable 'Tmp_GIS_Insurer'
GO
EXEC DDLDropTable 'Tmp_numbering_scheme'
GO
EXEC DDLDropTable 'Tmp_Accumulation'
GO
EXEC DDLDropTable 'Tmp_AuditSet'
GO
EXEC DDLDropTable 'Tmp_Batch_Rejection'
GO
EXEC DDLDropTable 'Tmp_Budget'
GO
EXEC DDLDropTable 'Tmp_Budget_Detail'
GO
EXEC DDLDropTable 'Tmp_CashListItem'
GO
EXEC DDLDropTable 'Tmp_Currency'
GO
EXEC DDLDropTable 'Tmp_Period'
GO
EXEC DDLDropTable 'Tmp_CashList'
GO
EXEC DDLDropTable 'Tmp_Batch'
GO
EXEC DDLDropTable 'Tmp_PMUser'
GO
EXEC DDLDropTable 'Tmp_PMWrk_Task_1'
GO
EXEC DDLDropTable 'Tmp_PMWrk_Task_Group'
GO
EXEC DDLDropTable 'Tmp_Claim_XOL_Arrangement'
GO
EXEC DDLDropTable 'Tmp_PMUser_Group'
GO
EXEC DDLDropTable 'Tmp_PMB_Doc_Link'
GO
EXEC DDLDropTable 'Tmp_GIS_Scheme_Group'
GO
EXEC DDLDropTable 'Tmp_PFPartners'
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'GIS_Insurer') AND name='gis_insurer_id')=0
BEGIN
    EXEC DDLDropForeignKey 'GIS_Scheme','gis_insurer_id'
    EXEC DDLAddIdentityColumn 'Gis_insurer', 'gis_insurer_id'
	EXEC DDLADDPrimaryKey 'GIS_Insurer','gis_insurer_id'
	EXEC DDLAddIndex 'GIS_Insurer','caption_id'
	EXEC DDLAddIndex 'GIS_Insurer','code'
	EXEC DDLAddIndex 'GIS_Insurer','party_cnt'
	EXEC DDLAddForeignKey @sTableName='GIS_Scheme', @sColumnName1='gis_insurer_id', @sRefTableName = 'Gis_Insurer',@sRefColumnName1='gis_insurer_id'
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'numbering_scheme') AND name='numbering_scheme_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'Abandoned_Numbers','numbering_scheme_id'
	--Create Identity column
        EXEC DDLAddIdentityColumn 'numbering_scheme','numbering_scheme_id'
	--Add constraints        
	EXEC DDLADDPrimaryKey 'numbering_scheme','numbering_scheme_id'
	EXEC DDLAddAlternateKey 'numbering_scheme','numbering_scheme_type_id','numbering_scheme'
	EXEC DDLAddIndex 'numbering_scheme','caption_id'
	EXEC DDLAddIndex 'numbering_scheme','code'
	EXEC DDLAddIndex 'numbering_scheme','numbering_scheme_type_id'
	EXEC DDLAddForeignKey @sTableName='numbering_scheme', @sColumnName1='numbering_scheme_type_id', @sRefTableName = 'numbering_scheme_type',@sRefColumnName1='numbering_scheme_type_id'
	EXEC DDLAddForeignKey @sTableName='numbering_scheme', @sColumnName1='party_type_id', @sRefTableName = 'Party_Type',@sRefColumnName1='party_type_id'
	EXEC DDLAddForeignKey @sTableName='Abandoned_Numbers', @sColumnName1='numbering_scheme_id', @sRefTableName = 'numbering_scheme',@sRefColumnName1='numbering_scheme_id'
END
GO


IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'Accumulation') AND name='accumulation_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'Risk','accumulation_id'	
	EXEC DDLDropForeignKey 'Accumulation','parent_id'
	--Create Identity column
	EXEC DDLAddIdentityColumn 'Accumulation','accumulation_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'Accumulation','accumulation_id'
	EXEC DDLAddIndex 'Accumulation','caption_id'
	EXEC DDLAddIndex 'Accumulation','code'
	EXEC DDLAddIndex 'Accumulation','quick_code'
	EXEC DDLAddIndex 'Accumulation','parent_id'
	EXEC DDLAddIndex 'Accumulation','accumulation_class_id'
	EXEC DDLAddForeignKey @sTableName='Accumulation', @sColumnName1='parent_id', @sRefTableName = 'Accumulation',@sRefColumnName1='accumulation_id'
	EXEC DDLAddForeignKey @sTableName='Accumulation', @sColumnName1='accumulation_class_id', @sRefTableName = 'Accumulation_Class',@sRefColumnName1='accumulation_class_id'
	EXEC DDLAddForeignKey @sTableName='Risk', @sColumnName1='accumulation_id', @sRefTableName = 'Accumulation',@sRefColumnName1='accumulation_id'
END
GO


IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'AuditSet') AND name='auditset_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'Document','auditset_id'   
	--Create Identity column
	EXEC DDLAddIdentityColumn 'AuditSet','auditset_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'AuditSet','auditset_id'
	EXEC DDLAddIndex 'AuditSet','company_id'
	EXEC DDLAddIndex 'AuditSet','document_id'	
	EXEC DDLAddIndex 'AuditSet','auditset_type_id'	
	EXEC DDLAddIndex 'AuditSet','cashlistitem_id'	
	EXEC DDLAddIndex 'AuditSet','user_id'
	EXEC DDLAddForeignKey @sTableName='AuditSet', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'
	EXEC DDLAddForeignKey @sTableName='AuditSet', @sColumnName1='approved_user_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'	
	EXEC DDLAddForeignKey @sTableName='AuditSet', @sColumnName1='rejected_user_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'	
	EXEC DDLAddForeignKey @sTableName='Document', @sColumnName1='auditset_id', @sRefTableName = 'AuditSet',@sRefColumnName1='auditset_id'	
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'Batch_Rejection') AND name='batch_rejection_id')=0
BEGIN
	--Drop all refernced by FK constraints
	--Create Identity column
	EXEC DDLAddIdentityColumn 'Batch_Rejection','batch_rejection_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'Batch_Rejection','batch_rejection_id'
	EXEC DDLAddForeignKey @sTableName='Batch_Rejection', @sColumnName1='pmuser_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'
	EXEC DDLAddForeignKey @sTableName='Batch_Rejection', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='Batch_Rejection', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'
	EXEC DDLAddForeignKey @sTableName='Batch_Rejection', @sColumnName1='cashlistitem_reverse_reason_id', @sRefTableName = 'CashListItem_Reverse_Reason',@sRefColumnName1='cashlistitem_reverse_reason_id'	
END
GO


IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'Budget') AND name='budget_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'Budget_Detail','budget_id'
	--Create Identity column
	EXEC DDLAddIdentityColumn 'Budget','budget_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'Budget','budget_id'	
	EXEC DDLAddIndex 'Budget','budget_ref'
	EXEC DDLAddIndex 'Budget','period_id'
	EXEC DDLAddIndex 'Budget','revises_budget_id'
	EXEC DDLAddIndex 'Budget','budget_status_id'	
	EXEC DDLAddForeignKey @sTableName='Budget_Detail', @sColumnName1='budget_id', @sRefTableName = 'Budget',@sRefColumnName1='budget_id'	
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'Budget_Detail') AND name='budget_detail_id')=0
BEGIN
	--Drop all refernced by FK constraints
	--Create Identity column
	EXEC DDLAddIdentityColumn 'Budget_Detail','budget_detail_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'Budget_Detail','budget_detail_id'	
	EXEC DDLAddIndex 'Budget_Detail','budget_id'
	EXEC DDLAddIndex 'Budget_Detail','account_id'
	EXEC DDLAddIndex 'Budget_Detail','period_id'
	EXEC DDLAddForeignKey @sTableName='Budget_Detail', @sColumnName1='budget_id', @sRefTableName = 'Budget',@sRefColumnName1='budget_id'	
END
GO


IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'CashListItem') AND name='cashlistitem_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'AllocationDetail','cashlistitem_id'	
	EXEC DDLDropForeignKey 'AuditSet','cashlistitem_id'	
	EXEC DDLDropForeignKey 'Batch_Rejection','cashlistitem_id'	
	EXEC DDLDropForeignKey 'CashListItem','replaces_cashlistitem_id'	
	EXEC DDLDropForeignKey 'CashListItem_Audit','cashlistitem_id'	
	EXEC DDLDropForeignKey 'CashListItem_Claim_Link','CashListItem_id'	
	EXEC DDLDropForeignKey 'CashListItem_Instalments','CashListItem_id'	
	EXEC DDLDropForeignKey 'Insurance_File','CashListItem_id'	
	EXEC DDLDropForeignKey 'transaction_comment','CashListItem_id'	
	--Create Identity column
	EXEC DDLAddIdentityColumn 'CashListItem','cashlistitem_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'CashListItem','cashlistitem_id'
	EXEC DDLAddIndex 'CashListItem','account_id'
	EXEC DDLAddIndex 'CashListItem','allocationstatus_id'
	EXEC DDLAddIndex 'CashListItem','cashlist_id'
	EXEC DDLAddIndex 'CashListItem','mediatype_id'
	EXEC DDLAddIndex 'CashListItem','transdetail_id'
	EXEC DDLAddIndex 'CashListItem','batch_id'
	EXEC DDLAddIndex 'CashListItem','cashlistitem_receipt_type_id'
	EXEC DDLAddIndex 'CashListItem','cashlistitem_receipt_status_id'
	EXEC DDLAddIndex 'CashListItem','cashlistitem_payment_type_id'
	EXEC DDLAddIndex 'CashListItem','cashlistitem_payment_status_id'
	EXEC DDLAddIndex 'CashListItem','media_ref'
	EXEC DDLAddIndex 'CashListItem','their_ref'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_reverse_pmuser_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_reversal_transdetail_id', @sRefTableName = 'TransDetail',@sRefColumnName1='transdetail_id'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='underwriting_year_id', @sRefTableName = 'Underwriting_Year',@sRefColumnName1='underwriting_year_id'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='exchange_rate_override_reason_id', @sRefTableName = 'Exchange_Rate_Override_Reason',@sRefColumnName1='exchange_rate_override_reason_id'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='mediatype_id', @sRefTableName = 'MediaType',@sRefColumnName1='mediatype_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='mediatype_issuer_id', @sRefTableName = 'MediaType_Issuer',@sRefColumnName1='MediaType_Issuer_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_receipt_status_id', @sRefTableName = 'CashListItem_Receipt_Status',@sRefColumnName1='cashlistitem_receipt_status_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_receipt_type_id', @sRefTableName = 'CashListItem_Receipt_Type',@sRefColumnName1='cashlistitem_receipt_type_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_reverse_reason_id', @sRefTableName = 'CashListItem_Reverse_Reason',@sRefColumnName1='cashlistitem_reverse_reason_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_bank_id', @sRefTableName = 'CashListItem_Bank',@sRefColumnName1='cashlistitem_bank_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_payment_status_id', @sRefTableName = 'CashListItem_Payment_Status',@sRefColumnName1='cashlistitem_payment_status_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_payment_type_id', @sRefTableName = 'CashListItem_Payment_Type',@sRefColumnName1='cashlistitem_payment_type_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlist_id', @sRefTableName = 'CashList',@sRefColumnName1='cashlist_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='replaces_cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='account_id', @sRefTableName = 'Account',@sRefColumnName1='account_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='allocationstatus_id', @sRefTableName = 'AllocationStatus',@sRefColumnName1='allocationstatus_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='bank_payment_type_id', @sRefTableName = 'Bank_Payment_Type',@sRefColumnName1='bank_payment_type_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlistitem_id', @sRefTableName = 'CashListItem',@sRefColumnName1='cashlistitem_id'	
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'Period') AND name='period_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'MatchGroup','period_id'
	EXEC DDLDropForeignKey 'Transdetail','period_id'
	--Create Identity column
    	EXEC DDLAddIdentityColumn 'Period','period_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'Period','period_id'
	EXEC DDLAddIndex 'Period','company_id'
	EXEC DDLAddIndex 'Period','sub_branch_id'
	EXEC DDLAddIndex 'Period','period_name'
	EXEC DDLAddForeignKey @sTableName='MatchGroup', @sColumnName1='period_id', @sRefTableName = 'Period',@sRefColumnName1='period_id'
	EXEC DDLAddForeignKey @sTableName='Transdetail', @sColumnName1='period_id', @sRefTableName = 'Period',@sRefColumnName1='period_id'
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'CashList') AND name='cashlist_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'CashList_Adjustment','cashlist_id'
	EXEC DDLDropForeignKey 'CashList_Cash','cashlist_id'
	EXEC DDLDropForeignKey 'CashListItem','cashlist_id'
	--Create Identity column
    	EXEC DDLAddIdentityColumn 'CashList','cashlist_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'CashList','cashlist_id'
	EXEC DDLAddIndex 'CashList','bankaccount_id'
	EXEC DDLAddIndex 'CashList','cashliststatus_id'
	EXEC DDLAddIndex 'CashList','cashlisttype_id'
	EXEC DDLAddIndex 'CashList','currency_id'
	EXEC DDLAddIndex 'CashList','company_id'
	EXEC DDLAddIndex 'CashList','sub_branch_id'
	EXEC DDLAddIndex 'CashList','cashlist_drawer_id'
	EXEC DDLAddIndex 'CashList','batch_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='pmuser_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='currency_id', @sRefTableName = 'Currency',@sRefColumnName1='currency_id'	
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='confirm_pmuser_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='confirm2_pmuser_id', @sRefTableName = 'PMUser',@sRefColumnName1='user_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='cashliststatus_id', @sRefTableName = 'CashListStatus',@sRefColumnName1='cashliststatus_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='cashlisttype_id', @sRefTableName = 'CashListType',@sRefColumnName1='cashlisttype_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='base_currency_id', @sRefTableName = 'Currency',@sRefColumnName1='currency_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='bankaccount_id', @sRefTableName = 'BankAccount',@sRefColumnName1='bankaccount_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='cashlist_drawer_id', @sRefTableName = 'CashList_Drawer',@sRefColumnName1='cashlist_drawer_id'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='cashlist_id', @sRefTableName = 'CashList',@sRefColumnName1='cashlist_id'
	EXEC DDLAddForeignKey @sTableName='CashList_Cash', @sColumnName1='cashlist_id', @sRefTableName = 'CashList',@sRefColumnName1='cashlist_id'
	EXEC DDLAddForeignKey @sTableName='CashList_Adjustment', @sColumnName1='cashlist_id', @sRefTableName = 'CashList',@sRefColumnName1='cashlist_id'
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'Batch') AND name='batch_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'Batch_Rejection','batch_id'
	EXEC DDLDropForeignKey 'CashList','batch_id'
	EXEC DDLDropForeignKey 'CashListItem','batch_id'
	EXEC DDLDropForeignKey 'Claim','batch_id'
	EXEC DDLDropForeignKey 'Document','batch_id'
	EXEC DDLDropForeignKey 'event_log','batch_id'
	EXEC DDLDropForeignKey 'PFInstalments','batch_id'
	EXEC DDLDropForeignKey 'TransDetail','batch_id'
	--Create Identity column
    EXEC DDLAddIdentityColumn 'Batch','batch_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'Batch','batch_id'
	EXEC DDLAddIndex 'Batch','batchstatus_id'
	EXEC DDLAddIndex 'Batch','company_id'
	EXEC DDLAddIndex 'Batch','user_id'
	EXEC DDLAddIndex 'Batch','batch_type_id'
	EXEC DDLAddIndex 'Batch','batch_source_id'
	EXEC DDLAddIndex 'Batch','cheque_start_number'
	EXEC DDLAddIndex 'Batch','batch_ref'
	EXEC DDLAddForeignKey @sTableName='Batch', @sColumnName1='batch_source_id', @sRefTableName = 'Batch_Source',@sRefColumnName1='batch_source_id'
	EXEC DDLAddForeignKey @sTableName='Batch', @sColumnName1='batch_type_id', @sRefTableName = 'Batch_Type',@sRefColumnName1='batch_type_id'
	EXEC DDLAddForeignKey @sTableName='Batch', @sColumnName1='batchstatus_id', @sRefTableName = 'BatchStatus',@sRefColumnName1='batchstatus_id'
	EXEC DDLAddForeignKey @sTableName='TransDetail', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='PFInstalments', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='event_log', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='Document', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='Claim', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='CashList', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
	EXEC DDLAddForeignKey @sTableName='Batch_Rejection', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
END
GO

DECLARE @SQL nvarchar(4000)
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PMUser_tru]') 
		and OBJECTPROPERTY(id, N'IsTrigger') = 1)
BEGIN
	SET @SQL='CREATE TRIGGER PMUser_tru ON dbo.PMUser FOR UPDATE
	AS
	BEGIN
	  IF UPDATE(password)
	  BEGIN
	    UPDATE pmuser set password_change_date = getdate() where user_id = (select user_id from inserted)
	  END
	END'
	exec (@SQL)
	SET @SQL=''
END
GO
DECLARE @SQL nvarchar(4000)
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PMUser_trd]') 
		and OBJECTPROPERTY(id, N'IsTrigger') = 1)
BEGIN
	SET @SQL='CREATE TRIGGER PMUser_trd
	ON dbo.PMUser
	FOR DELETE AS
	BEGIN
	SET NOCOUNT ON
	    DELETE
	    FROM    user_authorities
	    WHERE   user_id IN
	        (SELECT user_id FROM deleted WHERE user_id NOT IN
	            (SELECT user_id FROM inserted))
	SET NOCOUNT OFF
	END'
	exec (@SQL)
	SET @SQL=''
END
GO
DECLARE @SQL nvarchar(4000)
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PMUser_trd]') 
	and OBJECTPROPERTY(id, N'IsTrigger') = 1)
BEGIN
	SET @SQL='CREATE TRIGGER PMUser_tri
	ON dbo.PMUser
	FOR INSERT AS
	BEGIN
	SET NOCOUNT ON
	    INSERT
	    INTO    user_authorities
	        (user_id,
	         has_write_off_authority,
	         write_off_amount,
	         has_unrestricted_enquiry,
	         has_unrestricted_update)
	    SELECT
	        user_id,
	        1,
	        0,
	        0,
	        0
	    FROM    inserted
	    WHERE   user_id NOT IN (SELECT user_id FROM deleted)
	SET NOCOUNT OFF
	END'
	exec (@SQL)
	SET @SQL=''
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'PMWrk_Task') AND name='pmwrk_task_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'PMWrk_User_Quick_Start','pmwrk_task_id'
	EXEC DDLDropForeignKey 'Credit_Control_Step','pmwrk_task_id'
	EXEC DDLDropForeignKey 'PMWrk_Task_Group_Task','pmwrk_task_id'
	EXEC DDLDropForeignKey 'PMWrk_Task_Instance','pmwrk_task_id'
	--Create Identity column
    EXEC DDLAddIdentityColumn 'PMWrk_Task','pmwrk_task_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'PMWrk_Task','pmwrk_task_id'
	EXEC DDLAddIndex 'PMWrk_Task','caption_id'
	EXEC DDLAddIndex 'PMWrk_Task','code'
	EXEC DDLAddIndex 'PMWrk_Task','pmnav_process_id'
	EXEC DDLAddIndex 'PMWrk_Task','pmwrk_task_category_id'
	EXEC DDLAddCheck 'PMWrk_Task','type_of_task','([type_of_task] = 2 or ([type_of_task] = 1 or [type_of_task] = 0))'
	EXEC DDLAddForeignKey @sTableName='PMWrk_Task', @sColumnName1='pmwrk_task_category_id', @sRefTableName = 'PMWrk_Task_Category',@sRefColumnName1='pmwrk_task_category_id'
	EXEC DDLAddForeignKey @sTableName='PMWrk_Task_Instance', @sColumnName1='pmwrk_task_id', @sRefTableName = 'PMWrk_Task',@sRefColumnName1='pmwrk_task_id'
	EXEC DDLAddForeignKey @sTableName='PMWrk_Task_Group_Task', @sColumnName1='pmwrk_task_id', @sRefTableName = 'PMWrk_Task',@sRefColumnName1='pmwrk_task_id'	
	EXEC DDLAddForeignKey @sTableName='Credit_Control_Step', @sColumnName1='pmwrk_task_id', @sRefTableName = 'PMWrk_Task',@sRefColumnName1='pmwrk_task_id'
	EXEC DDLAddForeignKey @sTableName='PMWrk_User_Quick_Start', @sColumnName1='pmwrk_task_id', @sRefTableName = 'PMWrk_Task',@sRefColumnName1='pmwrk_task_id'
END
GO


IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'PMWrk_Task_Group') AND name='pmwrk_task_group_id')=0
BEGIN
	--Drop all refernced by FK constraints
	EXEC DDLDropForeignKey 'PMUser_Group_Activity','pmwrk_task_group_id'
	EXEC DDLDropForeignKey 'PMWrk_Task_Group_Task','pmwrk_task_group_id'
	EXEC DDLDropForeignKey 'PMWrk_Task_Instance','pmwrk_task_group_id'
	EXEC DDLDropForeignKey 'PMWrk_User_Quick_Start','pmwrk_task_group_id'	
	--Create Identity column
    EXEC DDLAddIdentityColumn 'PMWrk_Task_Group','pmwrk_task_group_id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'PMWrk_Task_Group','pmwrk_task_group_id'
	EXEC DDLAddIndex 'PMWrk_Task_Group','caption_id'
	EXEC DDLAddIndex 'PMWrk_Task_Group','code'
	EXEC DDLAddForeignKey @sTableName='PMWrk_User_Quick_Start', @sColumnName1='pmwrk_task_group_id', @sRefTableName = 'PMWrk_Task_Group',@sRefColumnName1='pmwrk_task_group_id'
	EXEC DDLAddForeignKey @sTableName='PMWrk_Task_Instance', @sColumnName1='pmwrk_task_group_id', @sRefTableName = 'PMWrk_Task_Group',@sRefColumnName1='pmwrk_task_group_id'
	EXEC DDLAddForeignKey @sTableName='PMWrk_Task_Group_Task', @sColumnName1='pmwrk_task_group_id', @sRefTableName = 'PMWrk_Task_Group',@sRefColumnName1='pmwrk_task_group_id'
	EXEC DDLAddForeignKey @sTableName='PMUser_Group_Activity', @sColumnName1='pmwrk_task_group_id', @sRefTableName = 'PMWrk_Task_Group',@sRefColumnName1='pmwrk_task_group_id'
END
GO


IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	ID = (SELECT id FROM sysobjects WHERE name = 'PMB_Doc_Link') AND name='PMB_Doc_Link_Id')=0
BEGIN
	--Drop all refernced by FK constraints
	--Create Identity column
    EXEC DDLAddIdentityColumn 'PMB_Doc_Link','PMB_Doc_Link_Id'
	--Add constraints
	EXEC DDLADDPrimaryKey 'PMB_Doc_Link','PMB_Doc_Link_Id'
	EXEC DDLAddIndex 'PMB_Doc_Link','GIS_Scheme_Id'
	EXEC DDLAddIndex 'PMB_Doc_Link','Process_Type_Id'
	EXEC DDLAddIndex 'PMB_Doc_Link','Document_Type_Id'
	EXEC DDLAddIndex 'PMB_Doc_Link','Document_Template_Id'
	EXEC DDLAddIndex 'PMB_Doc_Link','Agent_Cnt'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='product_id', @sRefTableName = 'Product',@sRefColumnName1='product_id'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='source_id', @sRefTableName = 'Source',@sRefColumnName1='source_id'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='Agent_Cnt', @sRefTableName = 'Party',@sRefColumnName1='party_cnt'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='Process_Type_Id', @sRefTableName = 'Process_type',@sRefColumnName1='Process_type_id'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='process_types_docs_id', @sRefTableName = 'Process_Types_Docs',@sRefColumnName1='process_types_docs_id'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='Document_Template_Id', @sRefTableName = 'Document_Template',@sRefColumnName1='document_template_id'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='Document_Type_Id', @sRefTableName = 'Document_Type',@sRefColumnName1='Document_Type_Id'
	EXEC DDLAddForeignKey @sTableName='PMB_Doc_Link', @sColumnName1='GIS_Scheme_Id', @sRefTableName = 'GIS_Scheme',@sRefColumnName1='gis_scheme_id'
END
GO



IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	id = (SELECT id FROM sysobjects WHERE name = 'PMUser_Group') AND name='pmuser_group_id')=0
BEGIN
    --Drop all refernced by FK constraints
    EXEC DDLDropForeignKey 'Credit_Control_Step','pmuser_group_id'
	EXEC DDLDropForeignKey 'Debtor_User_Groups','pmuser_group_id'
	EXEC DDLDropForeignKey 'PFRF','review_pmuser_group_id'
	EXEC DDLDropForeignKey 'PMUser_Group_Activity','pmuser_group_id'
	EXEC DDLDropForeignKey 'PMUser_Group_Group','pmuser_member_group_id'
	EXEC DDLDropForeignKey 'PMUser_Group_Group','pmuser_group_id'
	EXEC DDLDropForeignKey 'PMUser_Group_User','pmuser_group_id'
	EXEC DDLDropForeignKey 'PMWrk_Task_Instance','pmuser_group_id'
	--Create Identity column
    EXEC DDLAddIdentityColumn 'PMUser_Group','pmuser_group_id'
	--Add Constraints
    EXEC DDLADDPrimaryKey 'PMUser_Group','pmuser_group_id'
    EXEC DDLAddIndex 'PMUser_Group','caption_id'
	EXEC DDLAddIndex 'PMUser_Group','code'
    EXEC DDLAddForeignKey @sTableName='PMWrk_Task_Instance', @sColumnName1='pmuser_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='PMUser_Group_User', @sColumnName1='pmuser_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='PMUser_Group_Group', @sColumnName1='pmuser_member_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='PMUser_Group_Group', @sColumnName1='pmuser_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='PMUser_Group_Activity', @sColumnName1='pmuser_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='PFRF', @sColumnName1='review_pmuser_group_id',
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='Debtor_User_Groups', @sColumnName1='pmuser_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'
	EXEC DDLAddForeignKey @sTableName='Credit_Control_Step', @sColumnName1='pmuser_group_id', 
                        @sRefTableName = 'PMUser_Group',@sRefColumnName1='pmuser_group_id'

END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	id = (SELECT id FROM sysobjects WHERE name = 'GIS_Scheme_Group') AND name='gis_scheme_group_id')=0
BEGIN
    --Drop all refernced by FK constraints	
    EXEC DDLDropForeignKey 'GIS_Scheme_Group_Member','gis_scheme_group_id'
	--Create Identity column
    EXEC DDLAddIdentityColumn 'GIS_Scheme_Group','gis_scheme_group_id'
    --Add Constraints
    EXEC DDLADDPrimaryKey 'GIS_Scheme_Group','gis_scheme_group_id'
	EXEC DDLAddIndex 'GIS_Scheme_Group','caption_id'
	EXEC DDLAddIndex 'GIS_Scheme_Group','code'
	EXEC DDLAddIndex 'GIS_Scheme_Group','gis_business_type_id'
    EXEC DDLAddForeignKey @sTableName='GIS_Scheme_Group', @sColumnName1='gis_business_type_id', 
                        @sRefTableName = 'GIS_Business_Type',@sRefColumnName1='gis_business_type_id'
    EXEC DDLAddForeignKey @sTableName='GIS_Scheme_Group_Member', @sColumnName1='gis_scheme_group_id', 
                        @sRefTableName = 'GIS_Scheme_Group',@sRefColumnName1='gis_scheme_group_id'
    
END
GO

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	id = (SELECT id FROM sysobjects WHERE name = 'PFPartners') AND name='pfpartner_id')=0
BEGIN
    EXEC DDLAddIdentityColumn 'PFPartners','pfpartner_id'
END
GO
-- *****************************************************************************  
-- * Author:      Roopaly
-- * Date:        30-05-2008
-- * Purpose:     1.12 PLICO 45 - Out of Sequence Prior Periods
-- *****************************************************************************

EXEC DDLAddColumn 'User_Authorities','out_of_sequence_mta_authority','Tinyint NULL'
GO
EXEC DDLAddColumn 'product','out_of_sequence_mta_allocation','Tinyint NULL'
GO
EXEC DDLAddColumn 'product','out_of_sequence_mta_dates','Tinyint NULL'
GO

-- *****************************************************************************  
-- * Author:      Deepak
-- * Date:        14-06-2008
-- * Purpose:     Claims WorkFlow
-- *****************************************************************************

DDLADDColumn 'Process_Type','Functional_Area','tinyint'

GO
-- *****************************************************************************  
-- * Author:      Krishan Kumar Gaurav	
-- * Date:        02-06-2008
-- * Purpose:     To add start cheque number field
-- *****************************************************************************

EXEC DDLAddColumn 'BankAccount','Start_cheque_number','bigint NULL'
GO

-- *****************************************************************************  
-- * Author:      Krishan Kumar Gaurav	
-- * Date:        02-06-2008
-- * Purpose:     To add a field for Override Cheque Number permission
-- *****************************************************************************

EXEC DDLAddColumn 'User_Authorities','can_override_cheque_Numbers','Tinyint NULL'
GO

-- *****************************************************************************  
-- * Author:       Samrendu Bhushan
-- * Date:         16-06-2008
-- * Purpose:      Agent Payments Development
-- *****************************************************************************

EXEC DDLAddColumn 'party_agent','is_single_instalment_plan', 'tinyint NULL'
GO

EXEC DDLAddColumn 'party_agent','common_renewal_date', 'datetime NULL'
GO

-- *****************************************************************************  
-- * Author:      Roopaly Rastogi
-- * Date:        18-06-2008
-- * Purpose:     1.12 WR25 - Change product at renewal
-- *****************************************************************************

EXEC DDLAddColumn 'product','default_renewal_months','Integer NULL'
GO

-- *****************************************************************************
-- * Author:   Prabodh Mishra
-- * Date:     19/06/2008
-- * Purpose:  PN45727
-- *****************************************************************************
--EXEC DDLAddIndex 'document', 'insurance_file_cnt'
--GO

-- *****************************************************************************
-- * Author:	Gurucharan Gulati
-- * Date:		19/06/2008
-- * Purpose:	Batch Renewals-Multi-Threaded Controller
-- *****************************************************************************
EXEC DDLAddColumn 'Party_Agent','produce_agent_renewal_list','TINYINT NULL'
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Renewal_Job_Type'
IF @bExists = 0 BEGIN
	CREATE TABLE Batch_Renewal_Job_Type
	(
	[batch_renewal_job_type_id]		INT PRIMARY KEY,
	[code]							VARCHAR(20),
	[description]					VARCHAR(50),
	[is_deleted]					TINYINT NOT NULL,		
	[caption_id]					INT,
	[effective_date]				DATETIME NOT NULL
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Renewal_Job'
IF @bExists = 0 BEGIN
    CREATE TABLE Batch_Renewal_Job(
	[batch_renewal_job_id]			INT PRIMARY KEY IDENTITY,
	[code]							VARCHAR(20) NOT NULL,
	[description] 					VARCHAR(255) NOT NULL,
	[sam_server] 					VARCHAR(500) NOT NULL,
	[days_before_renewal_date] 		SMALLINT,
	[is_active] 					TINYINT NOT NULL,
	[batch_renewal_job_type_id]		INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job__batch_renewal_job_type_id FOREIGN KEY REFERENCES Batch_Renewal_Job_Type(batch_renewal_job_type_id),
	[renewal_docs_destination]		TINYINT NOT NULL,
	[report_sort_order]				TINYINT NOT NULL,
	[all_agents]					TINYINT NOT NULL,
	[pmuser_id]						INT NOT NULL,
	[date_created]					DATETIME NOT NULL,
	[date_updated]					DATETIME,
	[include_direct_policies]		TINYINT 								
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Renewal_Job_Products'
IF @bExists = 0 BEGIN
	CREATE TABLE Batch_Renewal_Job_Products(
	[batch_renewal_job_id] 			INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Products__batch_renewal_job_id FOREIGN KEY REFERENCES Batch_Renewal_Job(batch_renewal_job_id),
	[product_id]					INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Products__product_id FOREIGN KEY REFERENCES Product(product_id),
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Renewal_Job_Agents'
IF @bExists = 0 BEGIN
	CREATE TABLE Batch_Renewal_Job_Agents(
	[batch_renewal_job_id] 			INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Agents__batch_renewal_job_id FOREIGN KEY REFERENCES Batch_Renewal_Job(batch_renewal_job_id),
	[party_cnt]						INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Agents__party_cnt FOREIGN KEY REFERENCES Party(party_cnt),
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Renewal_Job_Branches'
IF @bExists = 0 BEGIN
	CREATE TABLE Batch_Renewal_Job_Branches(
	[batch_renewal_job_id] 			INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Branches__batch_renewal_job_id FOREIGN KEY REFERENCES Batch_Renewal_Job(batch_renewal_job_id),
	[source_id]						INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Branches__source_id FOREIGN KEY REFERENCES Source(source_id),
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Renewal_Job_Runs'
IF @bExists = 0 BEGIN
	CREATE TABLE Batch_Renewal_Job_Runs(
	[batch_renewal_job_runs_id]		INT PRIMARY KEY IDENTITY,
	[batch_renewal_job_id] 			INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Runs__batch_renewal_job_id FOREIGN KEY REFERENCES Batch_Renewal_Job(batch_renewal_job_id),
	[insurance_file_cnt]			INT NOT NULL CONSTRAINT FK__Batch_Renewal_Job_Runs__insurance_file_cnt FOREIGN KEY REFERENCES Insurance_File(insurance_file_cnt),
	[run_date]						DATETIME NOT NULL,
	[failure_reason]				VARCHAR(500),
	[document_printed]				VARCHAR(10)
	)
END
GO

-- *****************************************************************************  
-- * Author:      Krishan Kumar Gaurav	
-- * Date:        17-06-2008
-- * Purpose:     Claim Payment May Not Reserve Developement
-- *****************************************************************************

EXEC DDLAddColumn 'Product','payment_cannot_exceed_reserve','Tinyint NULL'
GO
-- *****************************************************************************  
-- * Author:     Deepak Arora	
-- * Date:        23-06-2008
-- * Purpose:     WR22 Agent Payments
-- *****************************************************************************
DDLADDCOLUMN 'Credit_Control_rule','product_id', 'INTEGER NULL'
GO
EXEC DDLAddForeignKey @sTableName='Credit_Control_rule', @sColumnName1='product_id', 
                        @sRefTableName = 'product',@sRefColumnName1='product_id'
GO
-- *****************************************************************************
-- * Author: 		Samrendu Bhushan
-- * Date: 		25/05/2008
-- * Purpose:		Agent Payments
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'PFPremiumFinance_Cancel_Reason'
IF @bExists = 0 BEGIN
    CREATE TABLE PFPremiumFinance_Cancel_Reason(
	[pfpremiumfinance_cancel_reason_id] INT PRIMARY KEY,
	[code] Varchar(20) NOT NULL,
	[description] Varchar(255) NOT NULL,
	[caption_id] Int NULL,
	[effective_date] Datetime NULL,
	[is_deleted] Tinyint NULL 
	)
END
GO

EXEC DDLAddColumn 'PFPremiumFinance','pfpremiumfinance_cancel_reason_id','Int NULL'
GO
EXEC DDLAddColumn 'PFPremiumFinance','is_cancel_policy_run','tinyint NULL'
GO
EXEC DDLAddForeignKey 'PFPremiumFinance', 'pfpremiumfinance_cancel_reason_id', @sRefTableName = 'PFPremiumFinance_Cancel_Reason'
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'PFPremiumFinance_Cancellation_Transactions'
IF @bExists = 0 BEGIN
    CREATE TABLE PFPremiumFinance_Cancellation_Transactions(
	[pfprem_finance_cnt] Int NOT NULL,
	[pfprem_finance_version] Int NOT NULL,
	[transdetail_id] Int NOT NULL
	)
END
GO

EXEC DDLAddForeignKey 'PFPremiumFinance_Cancellation_Transactions', 'transdetail_id', @sRefTableName = 'Transdetail'
GO

-- *****************************************************************************
-- * Author:  Deepak Arora
-- * Date:    25/06/2008
-- * Purpose: 
-- *****************************************************************************

EXEC DDLDropForeignKey 'CashListItem', 'Bank_Payment_type_id'
GO
EXEC DDLDropColumn 'CashListItem','Bank_Payment_type_id' 
GO
EXEC DDLDropForeignKey 'Claim_Payment', 'Bank_Payment_type_id'
GO
EXEC DDLDropColumn 'Claim_Payment','Bank_Payment_type_id' 
GO
EXEC DDLDropForeignKey 'PfPremiumFinance', 'Bank_Payment_type_id'
GO
EXEC DDLDropColumn 'PfPremiumFinance','Bank_Payment_type_id' 
GO

EXEC DDLDropForeignKey 'Party_Bank', 'Bank_account_type_id'
GO
EXEC DDLDropColumn 'Party_Bank','Bank_account_type_id'
GO

EXEC DDLDropForeignKey 'Party_bank_History', 'Bank_account_type_id'
GO
EXEC DDLDropColumn 'Party_bank_History','Bank_account_type_id' 
GO

EXEC DDLADDCOLUMN 'Party_Bank','account_type', 'VARCHAR(255) NULL'
GO
EXEC DDLADDCOLUMN 'Party_bank_History','account_type', 'VARCHAR(255) NULL'
GO

EXEC DDLAddColumn 'CashListItem','party_bank_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=CashListItem,@sColumnName1=party_bank_id,  
     @sRefTableName=Party_bank, @sRefColumnName1=party_bank_id  
GO


EXEC DDLAddColumn 'Claim_Payment','party_bank_id','Integer NULL'
GO
EXEC DDLAddForeignKey @sTableName=Claim_Payment,@sColumnName1=party_bank_id,  
     @sRefTableName=Party_bank, @sRefColumnName1=party_bank_id  
GO


EXEC DDLAddColumn 'PFPremiumFinance','party_bank_id','Integer NULL'
GO
EXEC DDLAddForeignKey @sTableName=Claim_Payment,@sColumnName1=party_bank_id,  
     @sRefTableName=Party_bank, @sRefColumnName1=party_bank_id  
GO

DECLARE @lCaptionID bigint
IF EXISTS(SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
BEGIN
    IF NOT EXISTS(Select 1 FROM Bank_payment_type WHERE CODE ='RECPAY')
    BEGIN
        
        EXEC DDLAlterColumn @sTableName='Party_bank', @sColumnName ='Bank_payment_Type_id',@sColumnDefinition='INT NULL'
        
        EXEC DDLAlterColumn @sTableName='Party_bank_History', @sColumnName ='Bank_payment_Type_id',@sColumnDefinition='INT NULL'
        
        Update Party_Bank Set Bank_payment_Type_id = NULL 
        
        Update Party_Bank_History SET bank_payment_type_id =NULL 
        
        Delete  FROM Bank_payment_type
           
        EXECUTE spu_pm_caption_id_return 1, 'Any', @lCaptionID OUTPUT
        INSERT INTO Bank_payment_type(caption_id, is_deleted,effective_date,Description,code)
        Values (@lCaptionID,0,getdate(),'Any','ANY') 

        EXECUTE spu_pm_caption_id_return 1, 'Instalments', @lCaptionID OUTPUT
        INSERT INTO Bank_payment_type(caption_id, is_deleted,effective_date,Description,code)
        Values (@lCaptionID,0,getdate(),'Instalments','INS')  
        
        EXECUTE spu_pm_caption_id_return 1, 'Claims', @lCaptionID OUTPUT
        INSERT INTO Bank_payment_type(caption_id, is_deleted,effective_date,Description,code)
        Values (@lCaptionID,0,getdate(),'Claims','CLM')  

        EXECUTE spu_pm_caption_id_return 1, 'Fees', @lCaptionID OUTPUT
        INSERT INTO Bank_payment_type(caption_id, is_deleted,effective_date,Description,code)
        Values (@lCaptionID,0,getdate(),'Fees','FEE')  

        EXECUTE spu_pm_caption_id_return 1, 'Commission', @lCaptionID OUTPUT
        INSERT INTO Bank_payment_type(caption_id, is_deleted,effective_date,Description,code)
        VALUES (@lCaptionID,0,getdate(),'Commission','COMM') 
        
        EXECUTE spu_pm_caption_id_return 1, 'Receipts/Payments', @lCaptionID OUTPUT
        INSERT INTO Bank_payment_type(caption_id, is_deleted,effective_date,Description,code)
        VALUES (@lCaptionID,0,getdate(),'Receipts/Payments','RECPAY') 

        UPDATE     Party_bank SET Bank_payment_Type_id = (SELECT MIN(Bank_payment_Type_id) FROM Bank_payment_type)
        UPDATE Party_bank_History SET Bank_payment_Type_id = (SELECT MIN(Bank_payment_Type_id) FROM Bank_payment_type)

        EXEC DDLAlterColumn @sTableName='Party_bank', @sColumnName ='Bank_payment_Type_id',@sColumnDefinition='INT NOT NULL'
        
        EXEC DDLAlterColumn @sTableName='Party_bank_History', @sColumnName ='Bank_payment_Type_id',@sColumnDefinition='INT NOT NULL'    

    END
	
    ELSE
    BEGIN
	IF EXISTS(SELECT 1 FROM PMCaption 
	JOIN BANK_PAYMENT_TYPE BPT ON PMCaption.Caption_Id=BPT.Caption_Id
	WHERE PMCaption.Caption<>BPT.Description)

	BEGIN
		UPDATE PMCaption SET Caption=BPT.Description from
		BANK_PAYMENT_TYPE BPT
		WHERE PMCaption.Caption_Id=BPT.Caption_Id
	END
    END
END
GO

-- *****************************************************************************
-- * Author:  Shubhankar Singh
-- * Date:    27/06/2008
-- * Purpose: Add Non-cluster index on tax_calculation and commision_arrangement
-- *****************************************************************************

--DDLAddIndex 'tax_calculation', 'agent_commission_cnt'
--GO
--DDLAddIndex 'commission_arrangement', 'effective_date'

--GO

-- *****************************************************************************
-- * Author:  Gurucharan Gulati
-- * Date:    27/06/2008
-- * Purpose: Add is_failed field under Batch_Renewal_Job_Runs
-- *****************************************************************************
EXEC DDLAddColumn 'Batch_Renewal_Job_Runs','is_failed','TINYINT DEFAULT 0'
GO
EXEC DDLAddColumn 'Batch_Renewal_Job_Runs','GUID','VARCHAR(100) NULL'
GO

-- *****************************************************************************
-- * Author:   Gautam Poddar 
-- * Date:     07/07/2008
-- * Purpose:  Add AutoGeneratedPlanRef to PFMediaTypeHistory table
-- *           For parallel fixing of PN: 45434 
-- *****************************************************************************
EXEC DDLADDCOLUMN 'PFMediaTypeHistory', 'AutoGeneratedPlanRef','VARCHAR(20) NULL'
GO

-- *****************************************************************************
-- * Author:   Krishan Kumar Gorav
-- * Date:     08/07/2008
-- * Purpose:  Add bank_reconciliation_date to transdetail table
-- *           For fixing of PN: 45620  
-- *****************************************************************************
EXEC DDLADDCOLUMN 'transdetail', 'bank_reconciliation_date','DATETIME NULL'
GO

-- *****************************************************************************
-- * Author:   Sandeep Kumar
-- * Date:     09/07/2008
-- * Purpose:  Add enable_mtc_rating_rule to product table
-- *****************************************************************************
EXEC DDLAddColumn 'product', 'enable_mtc_rating_rule', 'tinyint'
Go

-- *****************************************************************************
-- * Author:   Krishan Kumar Gorav
-- * Date:     23/07/2008
-- * Purpose:  Change datatype of column 
-- *****************************************************************************

EXEC DDLAlterColumn 'product','max_unAuthorised_no_claim_payments','int',0
GO

-- *****************************************************************************
-- * Author:   Krishan Kumar Gorav
-- * Date:     26/07/2008
-- * Purpose:  Add columns for Payment Type and Account Type in media type history table
-- *****************************************************************************

EXEC DDLAddColumn 'pfmediatypehistory','PaymentType','varchar(255)',0
EXEC DDLAddColumn 'pfmediatypehistory','AccountType','varchar(255)',0
GO

-- *****************************************************************************
-- * Author:   Amit Kumar
-- * Date:     18/08/2008
-- * Purpose:  Add is_visible_from_Client_Manager to Document_Template table
-- *****************************************************************************
EXEC DDLAddColumn 'document_template', 'is_visible_from_client_manager', 'tinyint DEFAULT 0 NOT NULL'
GO

-- *****************************************************************************
-- * Author:        Amit Kumar
-- * Date:          27/08/2008
-- * Purpose:       Claim Enahancements - Add id_dirty flag to case
-- *****************************************************************************

EXEC DDLAddColumn 'Case', 'is_dirty_case', 'TINYINT DEFAULT 0 NOT NULL'
GO

-- *****************************************************************************
-- * PN48084
-- * Author: Prabodh Mishra
-- * Date: 27/10/2008
-- *****************************************************************************
EXEC DDLAddColumn 'credit_control_item', 'is_balance_amount', 'tinyint NULL'
GO

-- *****************************************************************************
-- * Author:   Gurucharan Gulati
-- * Date:     29-10-2008
-- * Purpose:  PN48875/51809 - To avoid truncating characters of username
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'stats_folder', 'created_by_username', 'VARCHAR(255)'
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     06-11-2008
-- * Purpose:  WR6 Clause Grouping Development done under the scope of 1.13
-- *****************************************************************************

--Start (Arul Stephen) - (Tech Spec WR34 - Clause Grouping)
DECLARE @bExists TINYINT

EXECUTE @bExists = DDLExistsTable 'Wording_Product_Link'

IF @bExists = 0
BEGIN

	CREATE TABLE Wording_Product_Link

	(
		document_template_id 	INT  		NOT NULL,
		product_id 		INT	 	NOT NULL,
		branch_id 		INT		NOT NULL DEFAULT 1,
		[default] 		INT 		NOT NULL DEFAULT 0

                CONSTRAINT PK__Wording__Product_Link__document_template_id_Product_Id_Branch_Id PRIMARY KEY CLUSTERED (Document_template_id,Product_Id,branch_id)
		CONSTRAINT FK__Wording__Product_Link__document_template_id FOREIGN KEY (document_template_id) REFERENCES Document_Template(document_template_id),
	        CONSTRAINT FK__Wording__Product_Link__product_id FOREIGN KEY (product_id) REFERENCES product(product_id),
	        CONSTRAINT FK__Wording__Product_Link__branch_id FOREIGN KEY (branch_id) REFERENCES Source(source_id),

	)


END
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     06-11-2008
-- * Purpose:  WR6 Clause Grouping Development done under the scope of 1.13
-- *****************************************************************************
--Start (Arul Stephen) - (Tech Spec WR6 - Clause Grouping)
	DECLARE @ISEXISTSBranchId INT
	DECLARE @ISEXISTSDefault INT

	EXECUTE @ISEXISTSBranchId = DDLExistsColumn 'Wording_Risk_Type_link','branch_id'
	EXECUTE @ISEXISTSDefault = DDLExistsColumn 'Wording_Risk_Type_link','default'

		IF (@ISEXISTSBranchId = 0 AND @ISEXISTSDefault=0)
		BEGIN	
			--Start (Arul Stephen) - (Tech Spec WR6 - Clause Grouping)
			EXECUTE DDLAddColumn 'Wording_Risk_Type_link', 'branch_id', 'INT NULL', @bQuiet = 1
			--GO
			
			EXECUTE DDLAddForeignKey @sTableName = 'Wording_Risk_Type_link', @sColumnName1 = 'branch_id', @sRefTableName = 'source', @sRefColumnName1 = 'source_id', @bQuiet = 1
			--GO
			
			EXECUTE DDLAddColumn 'Wording_Risk_Type_link', 'default', 'TINYINT NULL', @bQuiet =1
			--GO
			--End -(Arul Stephen)-(Tech Spec WR6 - Clause Grouping)
	
	
			CREATE TABLE Tmb_Wording_Risk_Type_Link
			(
				document_template_id 	INT  		NOT NULL ,
				Risk_Type_id 		INT	 	NOT NULL ,
				branch_id 		INT		NULL  ,
				[default] 		TINYINT 	NULL 	

			)

	                IF EXISTS(SELECT * FROM dbo.Wording_Risk_Type_Link)
	
	                EXEC('INSERT INTO dbo.Tmb_Wording_Risk_Type_Link (document_template_id, Risk_Type_id, branch_id, [default])
	                       SELECT document_template_id, Risk_Type_id, branch_id, [default] FROM dbo.Wording_Risk_Type_Link')
	                
	                EXEC DDLDropConstraint 'Wording_Risk_Type_Link','document_template_id'
			EXEC DDLDropConstraint 'Wording_Risk_Type_Link','Risk_Type_id'
	                
	                EXEC DDLDropTable 'dbo.Wording_Risk_Type_Link'                
	                
			UPDATE Tmb_Wording_Risk_Type_link 
				SET branch_id=1 
				WHERE ISNULL(branch_id,0)=0
		
			UPDATE Tmb_Wording_Risk_Type_link 
				SET [default]=0 
				WHERE ISNULL([default],0)=0

	 		EXECUTE sp_rename 'dbo.Tmb_Wording_Risk_Type_Link', 'Wording_Risk_Type_Link'
	
			EXECUTE DDLAddORALTERColumn 'Wording_Risk_Type_link', 'branch_id', 'INT  NOT NULL', @bQuiet = 1
			EXECUTE DDLAddORALTERColumn 'Wording_Risk_Type_link', 'default', 'TINYINT NOT NULL', @bQuiet = 1
	              
	
	                EXECUTE DDLAddPrimaryKey 'Wording_Risk_Type_Link','Document_template_id','Risk_Type_Id','branch_id', @bQuiet = 1
	
	    		EXEC DDLAddForeignKey @sTableName='Wording_Risk_Type_Link', @sColumnName1='Document_Template_id', @sRefTableName = 'Document_Template',@sRefColumnName1='Document_Template_Id'
			EXEC DDLAddForeignKey @sTableName='Wording_Risk_Type_Link', @sColumnName1='Risk_Type_id', @sRefTableName = 'Risk_Type',@sRefColumnName1='Risk_Type_id'
			EXEC DDLAddForeignKey @sTableName='Wording_Risk_Type_Link', @sColumnName1='Branch_id', @sRefTableName = 'Source',@sRefColumnName1='Source_Id'
		
		END	

GO
--End -(Arul Stephen)-(Tech Spec WR6 - Clause Grouping)

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     08-11-2008
-- * Purpose:  user Level RI Display Screen Development done under the scope of 1.13
-- *****************************************************************************
--Start (Saurabh Agrawal) Tech Spec WR3 - user Level RI Display Screen (6.1)

EXECUTE DDLAddColumn 'User_Authorities', 'display_reinsurance', 'TINYINT  NOT NULL default  1', @bQuiet = 1
GO

EXECUTE DDLAddColumn 'User_Authorities', 'display_claim_reinsurance', 'TINYINT NOT NULL default  1', @bQuiet = 1
GO
--End (Saurabh Agrawal) Tech Spec WR3 - user Level RI Display Screen (6.1)


-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     08-11-2008
-- * Purpose:  Valiant - P16 - Broker Instalments.doc section(6.1)
-- *****************************************************************************
--Start (Sriram P )Tech Spec - Valiant - P16 - Broker Instalments.doc section(6.1)
EXECUTE DDLAddColumn 'PFRF', 'finance_net_commission', 'tinyint DEFAULT 0 NOT NULL', 1
GO
--End (Sriram P )Tech Spec - Valiant - P16 - Broker Instalments.doc section(6.1)



-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     5/06/2008
-- * Purpose:  Add enable_mtc_rating_rule to Product table
--             Column added to support MTC Rating Rules.
-- *****************************************************************************
EXEC DDLAddColumn 'product', 'enable_mtc_rating_rule', 'tinyint'
GO


-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     7/07/2008
-- * Purpose:  Add can_backdate_collection_date to user_authorities table. Collection_date and Comments to CashListItem
-- *****************************************************************************
EXEC DDLAddColumn 'user_authorities', 'can_backdate_collection_date', 'tinyint'
GO

EXEC DDLAddColumn 'cashlistitem', 'collection_date', 'datetime'
GO


EXEC DDLAddColumn 'cashlistitem', 'comments', 'varchar(100)'
GO


-- *****************************************************************************
-- * Author:      Rahul Jaiswal
-- * Date:        22-07-2008
-- * Purpose:     SMS Support 
-- *****************************************************************************

EXEC DDLAddColumn 'Event_log','batch_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=Event_log,@sColumnName1=batch_id,
     @sRefTableName=Batch, @sRefColumnName1=Batch_id  
GO

-- *****************************************************************************
-- * Author:      Rahul Jaiswal
-- * Date:        09-08-2008
-- * Purpose:     UIICWR59 â€“ Work Manager Enhancement
-- *****************************************************************************


EXEC DDLADDCOLUMN 'PmWrk_Task_Instance', 'Is_task_review','TINYINT NULL'
GO

EXEC DDLADDCOLUMN 'PmWrk_Task_Instance', 'Original_pmuser_group_id','INT NULL '
GO

EXEC DDLAddForeignKey 'PmWrk_Task_Instance', 'Original_pmuser_group_id', @sRefTableName = 'pmuser_group',@sRefColumnName1 = 'pmuser_group_id'
GO

-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Lookup Table for Bank Guarantee
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'BG_Status'
IF @bExists = 0
BEGIN
    CREATE TABLE BG_Status

	(
		BG_status_id	        INT             NOT NULL IDENTITY(1,1),
		Caption_id              INT             NOT NULL,
                Code                    CHAR(10)        NOT NULL,
                description             VARCHAR(255)    NULL,
                is_deleted              TINYINT         NOT NULL,
                effective_date          DATETIME        NOT NULL

		CONSTRAINT PK__BG_Status_id PRIMARY KEY CLUSTERED (BG_Status_id)
	)

END
GO

-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Master Table for keeping Bank Guarantee Data
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Bank_Guarantee'
IF @bExists = 0
BEGIN
    CREATE TABLE Bank_Guarantee

	(

		BG_id			                INT		NOT NULL IDENTITY,
		bank_name_id		                SMALLINT        NOT NULL,
		bank_branch		                VARCHAR(50)     NOT NULL,
		Party_Cnt		                INT		NOT NULL,
		custody_branch_id                       INT             NOT NULL,
                BG_ref			                VARCHAR(50)     NOT NULL,
		BG_currency_Id		                SMALLINT        NOT NULL,
		BG_limit		                NUMERIC(20,2)   NOT NULL,
		issue_date                              DATETIME        NOT NULL,
                available_bal		                NUMERIC(20,2)   NOT NULL,
		expiry_date		                DATETIME        NOT NULL,
		is_policy_lock		                TINYINT,
		is_deleted		                TINYINT,
		bg_Status_Id                            INT             NOT NULL


		CONSTRAINT PK__Bank_Guarantee_id PRIMARY KEY CLUSTERED (BG_id)
		CONSTRAINT FK__Bank_Guarantee__custody_branch_id FOREIGN KEY (custody_branch_id) REFERENCES source(source_id),
		CONSTRAINT FK__Bank_Guarantee__bg_status_id FOREIGN KEY (bg_status_id) REFERENCES BG_Status(bg_status_id),
		CONSTRAINT FK__Bank_Guarantee__Party_Cnt FOREIGN KEY (party_cnt) REFERENCES party(party_cnt),
		CONSTRAINT FK__Bank_Guarantee__bank_name_id FOREIGN KEY (bank_name_id) REFERENCES  Bank(bank_id),
		CONSTRAINT FK__Bank_Guarantee__BG_currency_id FOREIGN KEY (BG_currency_id) REFERENCES Currency(currency_id),
	)

END
GO


--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      BG and Branch link table.
--
--*****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'BG_Branch_Link'
IF @bExists = 0
BEGIN
    CREATE TABLE BG_Branch_Link

	(

		BG_Id			INT		NOT NULL,
		Source_Id		INT             NOT NULL


                CONSTRAINT PK__BG_Branch_Link__BG_Id_Source_Id PRIMARY KEY (BG_Id,Source_Id),
                CONSTRAINT FK__BG_Branch_Link__BG_Id FOREIGN KEY (BG_Id) REFERENCES Bank_Guarantee(BG_Id),
		CONSTRAINT FK__BG_Branch_Link__Branch_Id FOREIGN KEY (Source_Id) REFERENCES Source(source_id)
	)

END
GO

-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      BG and Product link table.
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'BG_Product_Link'
IF @bExists = 0
BEGIN
    CREATE TABLE BG_Product_Link

	(

		BG_Id			INT		NOT NULL,
		Product_Id		INT             NOT NULL

                CONSTRAINT PK__BG_Product_Link__BG_Id_Product_Id PRIMARY KEY (BG_Id,Product_Id),
		CONSTRAINT FK__BG_Product_Link__BG_Id FOREIGN KEY (BG_Id) REFERENCES Bank_Guarantee(BG_Id),
		CONSTRAINT FK__BG_Product_Link__Product_Id FOREIGN KEY (Product_Id) REFERENCES product(product_id),
	)

END
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Insurance File and BG Link
--*****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Insurance_File_BG_Link'
IF @bExists = 0
BEGIN
    CREATE TABLE Insurance_File_BG_Link

	(
		Insurance_File_BG_Link_Id	INT		NOT NULL IDENTITY,
		BG_id				INT,
		Insurance_File_Cnt		INT,
		Amount                          Numeric(20,2),
		DueDate                         DATETIME,
                BG_Status_Id                    INT,
                BG_Status_Date                  DATETIME

		CONSTRAINT PK__Insurance_File_BG_Link_id PRIMARY KEY CLUSTERED (Insurance_File_BG_Link_Id)
		CONSTRAINT FK__Insurance_File_BG_Link__BG_Id FOREIGN KEY (BG_Id) REFERENCES bank_guarantee(bg_id),
		CONSTRAINT FK__Insurance_File_BG_Link__Insurance_File_Cnt FOREIGN KEY (Insurance_File_Cnt) REFERENCES Insurance_File(Insurance_File_cnt),
		CONSTRAINT FK__Insurance_File_BG_Link__BG_Status FOREIGN KEY (BG_Status_Id) REFERENCES BG_Status(BG_Status_Id)
	)

END
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Bank Guarantee Table Altereations

--*****************************************************************************
EXEC DDLADDCOLUMN 'Event_Log', 'BG_Id','INT NULL'
GO

EXEC DDLADDCOLUMN 'Party_Agent', 'can_make_live_BankGuarantee','TINYINT NULL'
GO

EXEC DDLADDCOLUMN 'Product', 'can_make_live_BankGuarantee','TINYINT NULL'
GO

EXEC DDLADDCOLUMN 'User_Authorities', 'can_make_live_BankGuarantee','TINYINT NULL'
GO


DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'CashListItem_BG'
IF @bExists = 0 BEGIN
CREATE TABLE CashListItem_BG
(
	bg_id			INT,
	cashList_id		INT,
	cashListItem_id		INT,
	insurance_file_cnt	INT,
	amt_to_be_posted	Numeric(20,2)

        CONSTRAINT FK__CashListItem_BG__bg_id FOREIGN KEY (bg_id) REFERENCES bank_guarantee(bg_id),
        CONSTRAINT FK__CashListItem_BG__CashList_Id FOREIGN KEY (CashList_Id) REFERENCES CashList(cashList_Id),
        --CONSTRAINT FK__CashListItem_BG__CashListItem_Id FOREIGN KEY (CashListItem_Id) REFERENCES CashListItem(CashListItem_Id),
        CONSTRAINT FK__Bank_Guarantee__Insurance_File_Cnt FOREIGN KEY (insurance_file_cnt) REFERENCES  Insurance_File(insurance_file_cnt)

)
END
GO


--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Tech Spec WR34 - Claims Recovery Party Link

--*****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Recovery_Party_Type'
IF @bExists = 0
BEGIN

	CREATE TABLE Recovery_Party_Type
	(
		recovery_party_type_id 	INT  		NOT NULL IDENTITY,
		code 			CHAR (10) 	NOT NULL,
		description 		VARCHAR (50) 	NULL,
		caption_id 		INT 		NOT NULL,
		effective_date 		DATETIME	NOT NULL,
		is_deleted 		TINYINT 	NOT NULL,
		is_closed 		TINYINT 	NOT NULL

		CONSTRAINT PK__Recovery_Party_Type_Id PRIMARY KEY CLUSTERED (Recovery_Party_Type_Id)
	)


END

EXECUTE DDLAddColumn 'Recovery', 'recovery_party_type_id', 'INT NULL', 1
GO

EXECUTE DDLAddForeignKey @sTableName = 'Recovery', @sColumnName1 = 'recovery_party_type_id', @sRefTableName = 'Recovery_Party_Type', @sRefColumnName1 = 'recovery_party_type_id', @bQuiet = 1
GO

EXECUTE DDLAddColumn 'Recovery', 'recovery_party_cnt', 'INT NULL', 1
GO

EXECUTE DDLAddForeignKey @sTableName = 'Recovery', @sColumnName1 = 'recovery_party_cnt', @sRefTableName = 'party', @sRefColumnName1 = 'party_cnt', @bQuiet = 1
GO



--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      Tech Spec VAL P14 Policy Numbering

--*****************************************************************************
DECLARE @bExists TINYINT

EXECUTE @bExists = DDLExistsTable 'numbering_scheme_history'

IF @bExists = 0
BEGIN
CREATE TABLE numbering_scheme_history (

	numbering_scheme_history_id 			INT 	IDENTITY (1, 1) 	NOT NULL ,
	scheme_valid_from 				DATETIME 			NOT NULL ,
	numbering_scheme_id 				INT 				NOT NULL ,
	caption_id 					INT 				NOT NULL ,
	code 						CHAR(10)  			NOT NULL ,
	description 					VARCHAR(255)  			NULL ,
	is_deleted 					TINYINT 			NOT NULL ,
	effective_date 				        DATETIME 			NOT NULL ,
	numbering_scheme_type_id 			INT				NOT NULL ,
	numbering_scheme 				TINYINT 			NOT NULL ,
	is_generated 					TINYINT 			NOT NULL ,
	mask_code 					VARCHAR (20)  		        NOT NULL ,
	fixed_code 					VARCHAR (20) 			NULL ,
	next_number 					INT 				NOT NULL ,
	highest_number 				        INT 				NOT NULL ,
	step 						INT 				NOT NULL ,
	is_reuse_abandoned 				TINYINT				NOT NULL ,
	party_type_id 				        SMALLINT 			NULL ,
	is_read_only 					TINYINT 			NOT NULL ,
	CONSTRAINT [PK_numbering_scheme_history] PRIMARY KEY  CLUSTERED
	(
		[numbering_scheme_history_id]
	)  	ON [PRIMARY]
) ON [PRIMARY]
END
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      WR38 - Personal Client Resolved Name

--*****************************************************************************
--Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (6.1)
EXECUTE DDLAlterColumn 'Party', 'resolved_name', 'varchar(387) NULL', 1
--End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (6.1)


--*****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:
-- * Purpose:      Calliden WR3.2.1.1
--*****************************************************************************
EXEC DDLAddColumn 'ri_arrangement_line','ri_model_line_id','Int NULL'
GO
EXEC DDLDropForeignKey 'ri_arrangement_line', 'ri_model_line_id'
GO


--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      WR19 Cover Note Functionality
--*****************************************************************************
EXECUTE DDLAddColumn 'Product', 'Cover_Note_numbering_id', 'INT NULL', 1
Go

EXECUTE DDLAddForeignKey @sTableName = 'Product', @sColumnName1 = 'Cover_Note_numbering_id', @sRefTableName = 'Numbering_Scheme', @sRefColumnName1 = 'Numbering_Scheme_id', @bQuiet = 1
GO

EXECUTE DDLAddColumn 'Product', 'Cover_Note_Default_Period', 'INT NULL', 1
GO

EXECUTE DDLAddColumn 'Product', 'Cover_Note_reused_upto', 'INT NULL', 1
GO

EXECUTE DDLAddColumn 'Product', 'Cover_Note_doc_Template_id', 'INT NULL', 1
GO

EXECUTE DDLAddForeignKey @sTableName = 'Product', @sColumnName1 = 'Cover_Note_doc_Template_id', @sRefTableName = 'Document_Template', @sRefColumnName1 = 'Document_Template_id', @bQuiet = 1
GO


DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Risk_Cover_Note_Link'
IF @bExists = 0
   BEGIN
   CREATE TABLE Risk_Cover_Note_Link
   (
          Risk_Cover_Note_Link_Id  int                  NOT NULL IDENTITY,
          Risk_Id                  int                  NOT NULL,
          Cover_Note_Ref           varchar(50)          NULL,
          Cover_Note_From          datetime             NULL,
          Cover_Note_To            datetime             NULL

          CONSTRAINT PK__Risk_Cover_Note_Link_Id PRIMARY KEY CLUSTERED (Risk_Cover_Note_Link_Id)
          CONSTRAINT FK__Risk_Cover_Note_Link__Risk_Id FOREIGN KEY (Risk_Id) REFERENCES Risk(Risk_cnt)
)
END
GO


EXECUTE DDLAddColumn 'Product', 'allow_backdated_mtas', 'tinyint DEFAULT 0 Not NULL', 1
GO

--*****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:      QBENZ004 - Added new field for keeping the amount of net receipt
--*****************************************************************************
EXEC DDLAddColumn 'Recovery','this_receipt_net','Currency NULL'
GO

-- *****************************************************************************
-- * Author:       Prabodh Mishra
-- * Date:          06 Jan 2009
-- * Purpose:     PN48251 - Parallel fix
-- *****************************************************************************
--exec DDLDropIndex 'transdetail', 'account_id', 'transdetail_id', 'period_id', 'amount', 'currency_amount'
--GO
--exec DDLAddIndex 'transdetail', 'account_id', 'transdetail_id', 'period_id', 'outstanding_amount', 'outstanding_currency_amount'
--GO


-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         11 Jan 2009
-- * Purpose:      Tech Spec - PGR022 - Financial Interfaces
-- *****************************************************************************
EXECUTE DDLAddColumn 'bankaccount', 'financial_institution_code', 'varchar(50) NULL', 1
EXECUTE DDLAddColumn 'bankaccount', 'direct_debit_supplier_name', 'varchar(50) NULL', 1
EXECUTE DDLAddColumn 'bankaccount', 'direct_debit_supplier_id', 'Int NULL', 1
EXECUTE DDLAddColumn 'bankaccount', 'remitter', 'varchar(50) NULL', 1
EXECUTE DDLAddColumn 'bankaccount', 'processing_days', 'smallint NULL', 1


-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         11 Jan 2009
-- * Purpose:      Tech Spec PGR013 - SAM Cash Receipt.doc section
-- *****************************************************************************

--Start (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.1)
EXECUTE DDLAddColumn 'MediaType', 'refund_delay ', 'int DEFAULT 0 NOT NULL', 1
GO
--End (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.1)


-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         13 Jan 2009
-- * Purpose:      Tech Spec PGR005 - Automated Emails.doc 
-- *****************************************************************************
--Start(Saurabh Agrawal) Tech Spec PGR005 - Automated Emails(6.1.1)

EXECUTE DDLAddColumn 'Document_template','archive_with_no_print','tinyint NOT NULL Default 0', @BQuiet=1
EXECUTE DDLAddColumn 'Document_template','email_as_body','tinyint NOT NULL Default 0', @BQuiet=1
EXECUTE DDLAddColumn 'Document_template','spool_document','tinyint NOT NULL Default 0', @BQuiet=1

--End (Saurabh Agrawal) Tech Spec PGR005 - Automated Emails(6.1.1)

--Start(Saurabh Agrawal) Tech Spec PGR005 - Automated Emails(6.2.1)

EXECUTE DDLAddColumn 'PFScheme','ColNotDocID','int NULL', @BQuiet=1
EXECUTE DDLAddColumn 'PFScheme','ColNotNumDays','int NOT NULL Default 0', @BQuiet=1

--End(Saurabh Agrawal) Tech Spec PGR005 - Automated Emails(6.2.1)

--Start(Saurabh Agrawal) Tech Spec PGR005 - Automated Emails(6.3.1)

EXECUTE DDLAddColumn 'PFInstalments','notification_sent','smallint NOT NULL Default 0', @BQuiet=1
GO
--Start(Saurabh Agrawal) Tech Spec PGR005 - Automated Emails(6.3.1)

-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         6 Mar 2009
-- * Purpose:      Tech Spec ACR006 - FDMS Tracking Number
-- *****************************************************************************
EXEC DDLAddColumn 'CashListItem','cc_tracking_number','varchar(255) NULL',@bQuiet = 1
EXEC DDLAddColumn 'PFPremiumFinance','deposit_cc_tracking_number','varchar(255) NULL',@bQuiet = 1
EXEC DDLAddColumn 'PFMediaTypeHistory','deposit_cc_tracking_number','varchar(255) NULL',@bQuiet = 1
GO

-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:         24 Mar 2009
-- * Purpose:      LOA008 Account Handlers 
-- *****************************************************************************
DECLARE @bExists TINYINT 

EXECUTE @bExists = DDLExistsTable 'party_handler_branch' 

IF @bExists = 0 
Begin 
CREATE TABLE party_handler_branch 
	( 
        	party_cnt         INT                         NOT NULL , 
                source_id         INT                         NOT NULL , 
        CONSTRAINT [FK_party_handler_branch_Party] FOREIGN KEY (party_cnt) REFERENCES Party (party_cnt), 
        CONSTRAINT FK_party_handler_branch_Source FOREIGN KEY (source_id) REFERENCES Source (source_id)
	) 
                                
END 
GO 

-- *****************************************************************************
-- * Author:       Gaurav Arora
-- * Date:
-- * Purpose:     Out Of Sequence MTA
-- *****************************************************************************

--Start(Saurabh Agrawal) Bug Fixing Backdated MTA/MTC

EXECUTE DDLAddColumn 'Product','out_of_Sequence_MTA_UserGroup','int NOT NULL Default 0', @BQuiet=1
EXECUTE DDLAddColumn 'Product','out_of_Sequence_MTA_TaskGroup','int NOT NULL Default 0', @BQuiet=1
--End(Saurabh Agrawal) Bug Fixing Backdated MTA/MTC
GO
--DDLAddIndex 'Document', 'document_id','document_date'
--go

--DDLADDINDEX 'Tax_Calculation','claim_peril_id'
--go

--DDLADDINDEX 'Tax_Calculation','claim_receipt_item_id'
--go

--DDLADDINDEX 'Claim_Payment_Item','recovery_id'
--go

--DDLADDINDEX 'Claim_Payment_Item','reserve_id'
--go

--DDLADDINDEX 'Claim_Payment','claim_peril_id'
--go

--DDLADDINDEX 'Claim_Receipt_Item','recovery_id'
--go

--DDLADDINDEX 'Claim_Receipt_Item','reserve_id'
--go

--DDLADDINDEX 'Claim_Receipt','claim_id'
--go

--DDLADDINDEX 'Claim_Receipt','claim_peril_id'
--go

--DDLAddindex 'Claim_RI_Arrangement','ri_arrangement_id'
--go

--DDLAddindex 'Claim_XOL_Arrangement','ri_arrangement_id'
--go

--DDLAddindex 'Claim_RI_Arrangement_Line','ri_arrangement_id','grouping','claim_id','priority','ri_arrangement_line_id'
--go

--DDLAddindex 'Claim_RI_Arrangement_Line','ri_arrangement_line_id','type','claim_id','priority','ri_arrangement_id'
--go

--DDLAddindex 'RI_Arrangement_Line','party_cnt','grouping','ri_arrangement_id','priority','ri_arrangement_line_id'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1331132133_2_4')
--	CREATE STATISTICS [_dta_stat_1331132133_2_4] ON [RI_Arrangement]([risk_cnt], [ri_model_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1331132133_3_2_4')
--CREATE STATISTICS [_dta_stat_1331132133_3_2_4] ON [RI_Arrangement]([ri_band_id], [risk_cnt], [ri_model_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1331132133_8_2_4')
--CREATE STATISTICS [_dta_stat_1331132133_8_2_4] ON [RI_Arrangement]([is_modified], [risk_cnt], [ri_model_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1331132133_8_3_2_4')
--CREATE STATISTICS [_dta_stat_1331132133_8_3_2_4] ON [RI_Arrangement]([is_modified], [ri_band_id], [risk_cnt], [ri_model_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1411132418_3_2_5')
--CREATE STATISTICS [_dta_stat_1411132418_3_2_5] ON [RI_Arrangement_Line]([type], [ri_arrangement_id], [party_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1411132418_5_24_11_1')
--CREATE STATISTICS [_dta_stat_1411132418_5_24_11_1] ON [RI_Arrangement_Line]([party_cnt], [grouping], [priority], [ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1411132418_1_5_24_2')
--CREATE STATISTICS [_dta_stat_1411132418_1_5_24_2] ON [RI_Arrangement_Line]([ri_arrangement_line_id], [party_cnt], [grouping], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1411132418_2_5_24_11_1')
--CREATE STATISTICS [_dta_stat_1411132418_2_5_24_11_1] ON [RI_Arrangement_Line]([ri_arrangement_id], [party_cnt], [grouping], [priority], [ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_11_3')
--CREATE STATISTICS [_dta_stat_1715133501_11_3] ON [Claim_RI_Arrangement_Line]([priority], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_12_24')
--CREATE STATISTICS [_dta_stat_1715133501_12_24] ON [Claim_RI_Arrangement_Line]([number_of_lines], [claim_ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_11_2')
--CREATE STATISTICS [_dta_stat_1715133501_1_11_2] ON [Claim_RI_Arrangement_Line]([claim_id], [priority], [ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_3_6_31')
--CREATE STATISTICS [_dta_stat_1715133501_3_6_31] ON [Claim_RI_Arrangement_Line]([ri_arrangement_id], [party_cnt], [grouping])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_4_1_11')
--CREATE STATISTICS [_dta_stat_1715133501_4_1_11] ON [Claim_RI_Arrangement_Line]([type], [claim_id], [priority])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_3_31_2_1')
--CREATE STATISTICS [_dta_stat_1715133501_3_31_2_1] ON [Claim_RI_Arrangement_Line]([ri_arrangement_id], [grouping], [ri_arrangement_line_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_11_6_31_3')
--CREATE STATISTICS [_dta_stat_1715133501_11_6_31_3] ON [Claim_RI_Arrangement_Line]([priority], [party_cnt], [grouping], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_6_4_1_11_2')
--CREATE STATISTICS [_dta_stat_1715133501_6_4_1_11_2] ON [Claim_RI_Arrangement_Line]([party_cnt], [type], [claim_id], [priority], [ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_2_4_1_11_3_6')
--CREATE STATISTICS [_dta_stat_1715133501_2_4_1_11_3_6] ON [Claim_RI_Arrangement_Line]([ri_arrangement_line_id], [type], [claim_id], [priority], [ri_arrangement_id], [party_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_6_31_4_1_11_2')
--CREATE STATISTICS [_dta_stat_1715133501_6_31_4_1_11_2] ON [Claim_RI_Arrangement_Line]([party_cnt], [grouping], [type], [claim_id], [priority], [ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_6_31_1_3_11_2_4')
--CREATE STATISTICS [_dta_stat_1715133501_6_31_1_3_11_2_4] ON [Claim_RI_Arrangement_Line]([party_cnt], [grouping], [claim_id], [ri_arrangement_id], [priority], [ri_arrangement_line_id], [type])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_3_5_2')
--CREATE STATISTICS [_dta_stat_1507132760_3_5_2] ON [Claim_RI_Arrangement]([risk_cnt], [ri_model_id], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_1_3_5_2')
--CREATE STATISTICS [_dta_stat_1507132760_1_3_5_2] ON [Claim_RI_Arrangement]([claim_id], [risk_cnt], [ri_model_id], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_4_3_5_2_1')
--CREATE STATISTICS [_dta_stat_1507132760_4_3_5_2_1] ON [Claim_RI_Arrangement]([ri_band_id], [risk_cnt], [ri_model_id], [ri_arrangement_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_38_32')
--CREATE STATISTICS [_dta_stat_987866586_38_32] ON [Tax_Calculation]([spread_tax_across_instalments], [claim_payment_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_38_31')
--CREATE STATISTICS [_dta_stat_987866586_38_31] ON [Tax_Calculation]([spread_tax_across_instalments], [claim_peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_38_33')
--CREATE STATISTICS [_dta_stat_987866586_38_33] ON [Tax_Calculation]([spread_tax_across_instalments], [claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_38_34')
--CREATE STATISTICS [_dta_stat_987866586_38_34] ON [Tax_Calculation]([spread_tax_across_instalments], [claim_payment_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_38_35')
--CREATE STATISTICS [_dta_stat_987866586_38_35] ON [Tax_Calculation]([spread_tax_across_instalments], [claim_receipt_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_6_2')
--CREATE STATISTICS [_dta_stat_1974610423_6_2] ON [Claim_Payment_Item]([currency_id], [claim_payment_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_6_4')
--CREATE STATISTICS [_dta_stat_1974610423_6_4] ON [Claim_Payment_Item]([currency_id], [recovery_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_23_3')
--CREATE STATISTICS [_dta_stat_1686609397_23_3] ON [Claim_Payment]([insured_domiciled], [claim_peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_6_3')
--CREATE STATISTICS [_dta_stat_275128371_6_3] ON [Claim_Receipt_Item]([currency_id], [recovery_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_6_2')
--CREATE STATISTICS [_dta_stat_275128371_6_2] ON [Claim_Receipt_Item]([currency_id], [claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_2_1')
--CREATE STATISTICS [_dta_stat_275128371_2_1] ON [Claim_Receipt_Item]([claim_receipt_id], [claim_receipt_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_2_1')
--CREATE STATISTICS [_dta_stat_83127687_2_1] ON [Claim_Receipt]([claim_id], [claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_10_3')
--CREATE STATISTICS [_dta_stat_83127687_10_3] ON [Claim_Receipt]([insured_domiciled], [claim_peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1992394167_1_3')
--CREATE STATISTICS [_dta_stat_1992394167_1_3] ON [Claim_Party_Link]([claim_id], [Claim_Party_Link_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1188915307_12_2')
--CREATE STATISTICS [_dta_stat_1188915307_12_2] ON [Reserve]([Revised_Reserve_Entered], [claim_Peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1124915079_8_2')
--CREATE STATISTICS [_dta_stat_1124915079_8_2] ON [Recovery]([revision_count], [claim_Peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1124915079_1_2')
--CREATE STATISTICS [_dta_stat_1124915079_1_2] ON [Recovery]([Recovery_id], [claim_Peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1706853543_2_5')
--CREATE STATISTICS [_dta_stat_1706853543_2_5] ON [claim_link]([claim_id], [processed])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1706853543_2_3_5')
--CREATE STATISTICS [_dta_stat_1706853543_2_3_5] ON [claim_link]([claim_id], [link_type_id], [processed])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1706853543_1_4_5')
--CREATE STATISTICS [_dta_stat_1706853543_1_4_5] ON [claim_link]([claim_link_id], [link_id], [processed])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1706853543_1_4_2_5')
--CREATE STATISTICS [_dta_stat_1706853543_1_4_2_5] ON [claim_link]([claim_link_id], [link_id], [claim_id], [processed])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1706853543_5_3_1_4')
--CREATE STATISTICS [_dta_stat_1706853543_5_3_1_4] ON [claim_link]([processed], [link_type_id], [claim_link_id], [link_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1706853543_2_1_5_3_4')
--CREATE STATISTICS [_dta_stat_1706853543_2_1_5_3_4] ON [claim_link]([claim_id], [claim_link_id], [processed], [link_type_id], [link_id])
--go
--EXEC DDLAddIndex 'TransDetail',	'account_id', 'transdetail_id', 'document_id', 'period_id'
--go

--EXEC DDLAddIndex 'TransDetail', 'postingstatus_id', 'account_id', 'transdetail_id'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_1_3')
--CREATE STATISTICS [_dta_stat_2129442660_1_3] ON [TransDetail]([transdetail_id], [postingstatus_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_1_8')
--CREATE STATISTICS [_dta_stat_2129442660_1_8] ON [TransDetail]([transdetail_id], [document_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_9_2')
--CREATE STATISTICS [_dta_stat_2129442660_9_2] ON [TransDetail]([document_sequence], [account_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_1_9_2')
--CREATE STATISTICS [_dta_stat_2129442660_1_9_2] ON [TransDetail]([transdetail_id], [document_sequence], [account_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_1_2_3')
--CREATE STATISTICS [_dta_stat_2129442660_1_2_3] ON [TransDetail]([transdetail_id], [account_id], [postingstatus_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_8_27_2_7')
--CREATE STATISTICS [_dta_stat_2129442660_8_27_2_7] ON [TransDetail]([document_id], [spare], [account_id], [period_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_27_1_2_7')
--CREATE STATISTICS [_dta_stat_2129442660_27_1_2_7] ON [TransDetail]([spare], [transdetail_id], [account_id], [period_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_1_7_8_2_4')
--CREATE STATISTICS [_dta_stat_2129442660_1_7_8_2_4] ON [TransDetail]([transdetail_id], [period_id], [document_id], [account_id], [company_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_7_1_2_8_27')
--CREATE STATISTICS [_dta_stat_2129442660_7_1_2_8_27] ON [TransDetail]([period_id], [transdetail_id], [account_id], [document_id], [spare])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2129442660_7_1_2_8_4_27')
--CREATE STATISTICS [_dta_stat_2129442660_7_1_2_8_4_27] ON [TransDetail]([period_id], [transdetail_id], [account_id], [document_id], [company_id], [spare])
--go

--EXEC DDLAddIndex 'Event_Log', 'claim_cnt', 'event_date'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1529772507_14')
--CREATE STATISTICS [_dta_stat_1529772507_14] ON [event_log]([event_date])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1529772507_4_1')
--CREATE STATISTICS [_dta_stat_1529772507_4_1] ON [event_log]([insurance_file_cnt], [event_cnt])
--go

--EXEC DDLAddIndex 'RI_Arrangement', 'risk_cnt', 'original_flag', 'ri_model_id'
--go

--EXEC DDLAddIndex 'RI_Arrangement', 'risk_cnt', 'ri_band_id', 'original_flag', 'ri_arrangement_id'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1331132133_4_2_7')
--CREATE STATISTICS [_dta_stat_1331132133_4_2_7] ON [RI_Arrangement]([ri_model_id], [risk_cnt], [original_flag])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1331132133_1_2_3')
--CREATE STATISTICS [_dta_stat_1331132133_1_2_3] ON [RI_Arrangement]([ri_arrangement_id], [risk_cnt], [ri_band_id])
--go

--EXEC DDLAddIndex 'Peril', 'is_levy_tax', 'is_premium', 'is_taxed', 'risk_cnt'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1442820202_29_1_27')
--CREATE STATISTICS [_dta_stat_1442820202_29_1_27] ON [Peril]([is_levy_tax], [risk_cnt], [is_premium])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1442820202_30_1_29')
--CREATE STATISTICS [_dta_stat_1442820202_30_1_29] ON [Peril]([is_taxed], [risk_cnt], [is_levy_tax])
--go

--EXEC DDLAddIndex 'GIS_Policy_Link', 'claim_id'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_119671474_11_1')
--CREATE STATISTICS [_dta_stat_119671474_11_1] ON [GIS_Policy_Link]([risk_id], [gis_policy_link_id])
--go

--EXEC DDLAddIndex 'Insurance_File', 'insurance_file_cnt'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_1_8')
--CREATE STATISTICS [_dta_stat_758293761_1_8] ON [Insurance_File]([insurance_file_cnt], [insurance_ref])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_1_14_8')
--CREATE STATISTICS [_dta_stat_758293761_1_14_8] ON [Insurance_File]([insurance_file_cnt], [insured_cnt], [insurance_ref])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_1_9_7_8')
--CREATE STATISTICS [_dta_stat_758293761_1_9_7_8] ON [Insurance_File]([insurance_file_cnt], [product_id], [insurance_folder_cnt], [insurance_ref])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_14_1_9_7_8')
--CREATE STATISTICS [_dta_stat_758293761_14_1_9_7_8] ON [Insurance_File]([insured_cnt], [insurance_file_cnt], [product_id], [insurance_folder_cnt], [insurance_ref])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_1_36_7_44_3_9_11_4')
--CREATE STATISTICS [_dta_stat_758293761_1_36_7_44_3_9_11_4] ON [Insurance_File]([insurance_file_cnt], [policy_ignore], [insurance_folder_cnt], [Policy_type_id], [insurance_file_type_id], [product_id], [lead_agent_cnt], [insurance_file_status_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_7_44_3_9_11_4_8_1_36')
--CREATE STATISTICS [_dta_stat_758293761_7_44_3_9_11_4_8_1_36] ON [Insurance_File]([insurance_folder_cnt], [Policy_type_id], [insurance_file_type_id], [product_id], [lead_agent_cnt], [insurance_file_status_id], [insurance_ref], [insurance_file_cnt], [policy_ignore])
--go

--EXEC DDLAddIndex 'Party', 'party_cnt'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_146815585_7_1')
--CREATE STATISTICS [_dta_stat_146815585_7_1] ON [Party]([shortname], [party_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_146815585_27_1')
--CREATE STATISTICS [_dta_stat_146815585_27_1] ON [Party]([statements], [party_cnt])
--go

--EXEC DDLAddIndex 'PMWrk_Task_Instance', 'pmwrk_task_id', 'pmwrk_task_instance_cnt', 'task_due_date'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1845581613_7_4')
--CREATE STATISTICS [_dta_stat_1845581613_7_4] ON [PMWrk_Task_Instance]([pmuser_group_id], [pmwrk_task_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1845581613_6_7')
--CREATE STATISTICS [_dta_stat_1845581613_6_7] ON [PMWrk_Task_Instance]([task_due_date], [pmuser_group_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1845581613_1_4_6')
--CREATE STATISTICS [_dta_stat_1845581613_1_4_6] ON [PMWrk_Task_Instance]([pmwrk_task_instance_cnt], [pmwrk_task_id], [task_due_date])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1845581613_1_7_4_6')
--CREATE STATISTICS [_dta_stat_1845581613_1_7_4_6] ON [PMWrk_Task_Instance]([pmwrk_task_instance_cnt], [pmuser_group_id], [pmwrk_task_id], [task_due_date])
--go

--EXEC DDLAddIndex 'Stats_Folder', 'source_id'
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1755869322_40')
--CREATE STATISTICS [_dta_stat_1755869322_40] ON [Stats_Folder]([underwriting_year_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1755869322_4_40')
--CREATE STATISTICS [_dta_stat_1755869322_4_40] ON [Stats_Folder]([source_id], [underwriting_year_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1014294673_8_1')
--CREATE STATISTICS [_dta_stat_1014294673_8_1] ON [Insurance_Folder]([arc_archive_folder_id], [insurance_folder_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1014294673_4_1')
--CREATE STATISTICS [_dta_stat_1014294673_4_1] ON [Insurance_Folder]([insurance_holder_cnt], [insurance_folder_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_1_45')
--CREATE STATISTICS [_dta_stat_1686609397_1_45] ON [Claim_Payment]([claim_payment_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_23_2')
--CREATE STATISTICS [_dta_stat_1686609397_23_2] ON [Claim_Payment]([insured_domiciled], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_44_3')
--CREATE STATISTICS [_dta_stat_1686609397_44_3] ON [Claim_Payment]([base_claim_payment_id], [claim_peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_2_1_44')
--CREATE STATISTICS [_dta_stat_1686609397_2_1_44] ON [Claim_Payment]([claim_id], [claim_payment_id], [base_claim_payment_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_3_2_45_1')
--CREATE STATISTICS [_dta_stat_1686609397_3_2_45_1] ON [Claim_Payment]([claim_peril_id], [claim_id], [version_id], [claim_payment_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_1_20_2_21')
--CREATE STATISTICS [_dta_stat_1686609397_1_20_2_21] ON [Claim_Payment]([claim_payment_id], [treaty_id], [claim_id], [claim_payment_to_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_1_44_45_3')
--CREATE STATISTICS [_dta_stat_1686609397_1_44_45_3] ON [Claim_Payment]([claim_payment_id], [base_claim_payment_id], [version_id], [claim_peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_45_2_1_44')
--CREATE STATISTICS [_dta_stat_1686609397_45_2_1_44] ON [Claim_Payment]([version_id], [claim_id], [claim_payment_id], [base_claim_payment_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_44_1_3_2_45')
--CREATE STATISTICS [_dta_stat_1686609397_44_1_3_2_45] ON [Claim_Payment]([base_claim_payment_id], [claim_payment_id], [claim_peril_id], [claim_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_2_1_20_21_11_34_29_36')
--CREATE STATISTICS [_dta_stat_1686609397_2_1_20_21_11_34_29_36] ON [Claim_Payment]([claim_id], [claim_payment_id], [treaty_id], [claim_payment_to_id], [created_by], [document_id], [safe_harbour_id], [currency_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1686609397_2_1_3_20_21_11_34_29_36')
--CREATE STATISTICS [_dta_stat_1686609397_2_1_3_20_21_11_34_29_36] ON [Claim_Payment]([claim_id], [claim_payment_id], [claim_peril_id], [treaty_id], [claim_payment_to_id], [created_by], [document_id], [safe_harbour_id], [currency_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1925581898_2_3')
--CREATE STATISTICS [_dta_stat_1925581898_2_3] ON [PMWrk_Task_Inst_Key]([pmnav_key_id], [key_value])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1188915307_14_1')
--CREATE STATISTICS [_dta_stat_1188915307_14_1] ON [Reserve]([version_id], [Reserve_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1188915307_1_2')
--CREATE STATISTICS [_dta_stat_1188915307_1_2] ON [Reserve]([Reserve_id], [claim_Peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1188915307_14_2_1')
--CREATE STATISTICS [_dta_stat_1188915307_14_2_1] ON [Reserve]([version_id], [claim_Peril_id], [Reserve_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1188915307_1_13_14_2')
--CREATE STATISTICS [_dta_stat_1188915307_1_13_14_2] ON [Reserve]([Reserve_id], [base_reserve_id], [version_id], [claim_Peril_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_822293989_3_2')
--CREATE STATISTICS [_dta_stat_822293989_3_2] ON [insurance_file_risk_link]([status_flag], [risk_cnt])
--go


--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_379864420_5_2')
--CREATE STATISTICS [_dta_stat_379864420_5_2] ON [Risk]([risk_type_id], [risk_status_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_379864420_1_2_5_24')
--CREATE STATISTICS [_dta_stat_379864420_1_2_5_24] ON [Risk]([risk_cnt], [risk_status_id], [risk_type_id], [risk_number])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1124915079_10_1_11')
--CREATE STATISTICS [_dta_stat_1124915079_10_1_11] ON [Recovery]([base_recovery_id], [Recovery_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1124915079_2_11_10')
--CREATE STATISTICS [_dta_stat_1124915079_2_11_10] ON [Recovery]([claim_Peril_id], [version_id], [base_recovery_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1124915079_1_2_10_11')
--CREATE STATISTICS [_dta_stat_1124915079_1_2_10_11] ON [Recovery]([Recovery_id], [claim_Peril_id], [base_recovery_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_695673526_1_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16')
--CREATE STATISTICS [_dta_stat_695673526_1_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16] ON [GIS_Screen_Detail]([gis_screen_id], [screen_detail_cnt], [gis_object_id], [gis_property_id], [is_frame], [tab_number], [caption], [item_top], [item_left], [item_height], [item_width], [column_width], [pre_quote_requirement], [post_quote_requirement], [purchase_requirement], [parent_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_695673526_3_1_22_4_2_5_6_7_8_9_10_11_12_13_14_15')
--CREATE STATISTICS [_dta_stat_695673526_3_1_22_4_2_5_6_7_8_9_10_11_12_13_14_15] ON [GIS_Screen_Detail]([gis_object_id], [gis_screen_id], [child_screen_id], [gis_property_id], [screen_detail_cnt], [is_frame], [tab_number], [caption], [item_top], [item_left], [item_height], [item_width], [column_width], [pre_quote_requirement], [post_quote_requirement], [purchase_requirement])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_695673526_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17')
--CREATE STATISTICS [_dta_stat_695673526_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17] ON [GIS_Screen_Detail]([gis_screen_id], [screen_detail_cnt], [gis_property_id], [is_frame], [tab_number], [caption], [item_top], [item_left], [item_height], [item_width], [column_width], [pre_quote_requirement], [post_quote_requirement], [purchase_requirement], [parent_id], [help_text])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_31_17')
--CREATE STATISTICS [_dta_stat_987866586_31_17] ON [Tax_Calculation]([claim_peril_id], [class_of_business_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_1_21')
--CREATE STATISTICS [_dta_stat_987866586_1_21] ON [Tax_Calculation]([risk_cnt], [insurance_file_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_1_22')
--CREATE STATISTICS [_dta_stat_987866586_1_22] ON [Tax_Calculation]([risk_cnt], [transtype])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_22_21_1')
--CREATE STATISTICS [_dta_stat_987866586_22_21_1] ON [Tax_Calculation]([transtype], [insurance_file_cnt], [risk_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_21_20_1')
--CREATE STATISTICS [_dta_stat_987866586_21_20_1] ON [Tax_Calculation]([insurance_file_cnt], [tax_calculation_cnt], [risk_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_16_31_18')
--CREATE STATISTICS [_dta_stat_987866586_16_31_18] ON [Tax_Calculation]([state_id], [claim_peril_id], [tax_group_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_15_31_18_17')
--CREATE STATISTICS [_dta_stat_987866586_15_31_18_17] ON [Tax_Calculation]([country_id], [claim_peril_id], [tax_group_id], [class_of_business_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_20_1_22_21')
--CREATE STATISTICS [_dta_stat_987866586_20_1_22_21] ON [Tax_Calculation]([tax_calculation_cnt], [risk_cnt], [transtype], [insurance_file_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_12_31_18_17_16')
--CREATE STATISTICS [_dta_stat_987866586_12_31_18_17_16] ON [Tax_Calculation]([currency_id], [claim_peril_id], [tax_group_id], [class_of_business_id], [state_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_35_31_18_17_16_15')
--CREATE STATISTICS [_dta_stat_987866586_35_31_18_17_16_15] ON [Tax_Calculation]([claim_receipt_item_id], [claim_peril_id], [tax_group_id], [class_of_business_id], [state_id], [country_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_18_31_17_16_15_12_35')
--CREATE STATISTICS [_dta_stat_987866586_18_31_17_16_15_12_35] ON [Tax_Calculation]([tax_group_id], [claim_peril_id], [class_of_business_id], [state_id], [country_id], [currency_id], [claim_receipt_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_31_34_18_17_16_15_12_35')
--CREATE STATISTICS [_dta_stat_987866586_31_34_18_17_16_15_12_35] ON [Tax_Calculation]([claim_peril_id], [claim_payment_item_id], [tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [claim_receipt_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_18_17_16_15_12_2_32_33_34')
--CREATE STATISTICS [_dta_stat_987866586_18_17_16_15_12_2_32_33_34] ON [Tax_Calculation]([tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [tax_band_id], [claim_payment_id], [claim_receipt_id], [claim_payment_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_32_31_18_17_16_15_12_35_33')
--CREATE STATISTICS [_dta_stat_987866586_32_31_18_17_16_15_12_35_33] ON [Tax_Calculation]([claim_payment_id], [claim_peril_id], [tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [claim_receipt_item_id], [claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_31_33_18_17_16_15_12_35_34_32')
--CREATE STATISTICS [_dta_stat_987866586_31_33_18_17_16_15_12_35_34_32] ON [Tax_Calculation]([claim_peril_id], [claim_receipt_id], [tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [claim_receipt_item_id], [claim_payment_item_id], [claim_payment_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_2_31_18_17_16_15_12_35_33_34')
--CREATE STATISTICS [_dta_stat_987866586_2_31_18_17_16_15_12_35_33_34] ON [Tax_Calculation]([tax_band_id], [claim_peril_id], [tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [claim_receipt_item_id], [claim_receipt_id], [claim_payment_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_1_20_18_17_16_15_12_2_21_39_40_32_33_34_35')
--CREATE STATISTICS [_dta_stat_987866586_1_20_18_17_16_15_12_2_21_39_40_32_33_34_35] ON [Tax_Calculation]([risk_cnt], [tax_calculation_cnt], [tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [tax_band_id], [insurance_file_cnt], [base_tax_calculation_cnt], [version_id], [claim_payment_id], [claim_receipt_id], [claim_payment_item_id], [claim_receipt_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_987866586_18_17_16_15_12_35_33_34_32_2_31_1_20_21_39_40')
--CREATE STATISTICS [_dta_stat_987866586_18_17_16_15_12_35_33_34_32_2_31_1_20_21_39_40] ON [Tax_Calculation]([tax_group_id], [class_of_business_id], [state_id], [country_id], [currency_id], [claim_receipt_item_id], [claim_receipt_id], [claim_payment_item_id], [claim_payment_id], [tax_band_id], [claim_peril_id], [risk_cnt], [tax_calculation_cnt], [insurance_file_cnt], [base_tax_calculation_cnt], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_2_18')
--CREATE STATISTICS [_dta_stat_1507132760_2_18] ON [Claim_RI_Arrangement]([ri_arrangement_id], [base_claim_ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_6_1_2')
--CREATE STATISTICS [_dta_stat_1507132760_6_1_2] ON [Claim_RI_Arrangement]([claim_allocation_type], [claim_id], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_2_1_18')
--CREATE STATISTICS [_dta_stat_1507132760_2_1_18] ON [Claim_RI_Arrangement]([ri_arrangement_id], [claim_id], [base_claim_ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_3_2_4')
--CREATE STATISTICS [_dta_stat_1507132760_3_2_4] ON [Claim_RI_Arrangement]([risk_cnt], [ri_arrangement_id], [ri_band_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_5_1_2')
--CREATE STATISTICS [_dta_stat_1507132760_5_1_2] ON [Claim_RI_Arrangement]([ri_model_id], [claim_id], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_2_4_1')
--CREATE STATISTICS [_dta_stat_1507132760_2_4_1] ON [Claim_RI_Arrangement]([ri_arrangement_id], [ri_band_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_1_19_2_18')
--CREATE STATISTICS [_dta_stat_1507132760_1_19_2_18] ON [Claim_RI_Arrangement]([claim_id], [version_id], [ri_arrangement_id], [base_claim_ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_1_19_2_3_4')
--CREATE STATISTICS [_dta_stat_1507132760_1_19_2_3_4] ON [Claim_RI_Arrangement]([claim_id], [version_id], [ri_arrangement_id], [risk_cnt], [ri_band_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1507132760_1_3_4_18_2_19')
--CREATE STATISTICS [_dta_stat_1507132760_1_3_4_18_2_19] ON [Claim_RI_Arrangement]([claim_id], [risk_cnt], [ri_band_id], [base_claim_ri_arrangement_id], [ri_arrangement_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2056394395_7_2')
--CREATE STATISTICS [_dta_stat_2056394395_7_2] ON [Claim_Peril]([ri_band], [Claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_21_1')
--CREATE STATISTICS [_dta_stat_1974610423_21_1] ON [Claim_Payment_Item]([version_id], [claim_payment_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_6_3')
--CREATE STATISTICS [_dta_stat_1974610423_6_3] ON [Claim_Payment_Item]([currency_id], [reserve_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_20_1_21')
--CREATE STATISTICS [_dta_stat_1974610423_20_1_21] ON [Claim_Payment_Item]([base_claim_payment_item_id], [claim_payment_item_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_1_4_3_5_6_7')
--CREATE STATISTICS [_dta_stat_1974610423_1_4_3_5_6_7] ON [Claim_Payment_Item]([claim_payment_item_id], [recovery_id], [reserve_id], [recovery_type_id], [currency_id], [tax_group_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1974610423_2_1_4_3_5_6_7')
--CREATE STATISTICS [_dta_stat_1974610423_2_1_4_3_5_6_7] ON [Claim_Payment_Item]([claim_payment_id], [claim_payment_item_id], [recovery_id], [reserve_id], [recovery_type_id], [currency_id], [tax_group_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_183671702_2_8_1_15')
--CREATE STATISTICS [_dta_stat_183671702_2_8_1_15] ON [GIS_Property]([gis_object_id], [is_primary_key], [gis_property_id], [Specials_Type_Reference])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_183671702_1_2_3_4_13_14')
--CREATE STATISTICS [_dta_stat_183671702_1_2_3_4_13_14] ON [GIS_Property]([gis_property_id], [gis_object_id], [property_name], [column_name], [Edit_Flags], [Specials_Type])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_183671702_3_4_13_14_15_1_2')
--CREATE STATISTICS [_dta_stat_183671702_3_4_13_14_15_1_2] ON [GIS_Property]([property_name], [column_name], [Edit_Flags], [Specials_Type], [Specials_Type_Reference], [gis_property_id], [gis_object_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_133015655_1_6')
--CREATE STATISTICS [_dta_stat_133015655_1_6] ON [Document]([document_id], [documenttype_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_133015655_5_1')
--CREATE STATISTICS [_dta_stat_133015655_5_1] ON [Document]([postingstatus_id], [document_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_133015655_1_8')
--CREATE STATISTICS [_dta_stat_133015655_1_8] ON [Document]([document_id], [document_ref])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_133015655_5_6_1')
--CREATE STATISTICS [_dta_stat_133015655_5_6_1] ON [Document]([postingstatus_id], [documenttype_id], [document_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_133015655_1_3_8')
--CREATE STATISTICS [_dta_stat_133015655_1_3_8] ON [Document]([document_id], [company_id], [document_ref])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_133015655_8_6_1_3')
--CREATE STATISTICS [_dta_stat_133015655_8_6_1_3] ON [Document]([document_ref], [documenttype_id], [document_id], [company_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1624392856_10_1')
--CREATE STATISTICS [_dta_stat_1624392856_10_1] ON [Claim]([Catastrophe_code_id], [Claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1624392856_1_93')
--CREATE STATISTICS [_dta_stat_1624392856_1_93] ON [Claim]([Claim_id], [base_claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1624392856_98_1')
--CREATE STATISTICS [_dta_stat_1624392856_98_1] ON [Claim]([Document_Generated_Status], [Claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1624392856_59_92_58')
--CREATE STATISTICS [_dta_stat_1624392856_59_92_58] ON [Claim]([exchange_rate_override_reason_id], [claim_folder_id], [underwriting_year_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1624392856_1_59_92_58')
--CREATE STATISTICS [_dta_stat_1624392856_1_59_92_58] ON [Claim]([Claim_id], [exchange_rate_override_reason_id], [claim_folder_id], [underwriting_year_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_24_1')
--CREATE STATISTICS [_dta_stat_1715133501_24_1] ON [Claim_RI_Arrangement_Line]([claim_ri_arrangement_line_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_7_3')
--CREATE STATISTICS [_dta_stat_1715133501_7_3] ON [Claim_RI_Arrangement_Line]([xol_arrangement_id], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_3_4')
--CREATE STATISTICS [_dta_stat_1715133501_3_4] ON [Claim_RI_Arrangement_Line]([ri_arrangement_id], [type])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_12_1_3')
--CREATE STATISTICS [_dta_stat_1715133501_12_1_3] ON [Claim_RI_Arrangement_Line]([number_of_lines], [claim_id], [ri_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_3_5_6')
--CREATE STATISTICS [_dta_stat_1715133501_1_3_5_6] ON [Claim_RI_Arrangement_Line]([claim_id], [ri_arrangement_id], [treaty_id], [party_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_3_5_7')
--CREATE STATISTICS [_dta_stat_1715133501_1_3_5_7] ON [Claim_RI_Arrangement_Line]([claim_id], [ri_arrangement_id], [treaty_id], [xol_arrangement_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_7_3_5_6')
--CREATE STATISTICS [_dta_stat_1715133501_1_7_3_5_6] ON [Claim_RI_Arrangement_Line]([claim_id], [xol_arrangement_id], [ri_arrangement_id], [treaty_id], [party_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_26_25_5_6')
--CREATE STATISTICS [_dta_stat_1715133501_1_26_25_5_6] ON [Claim_RI_Arrangement_Line]([claim_id], [version_id], [base_claim_ri_arrangement_line_id], [treaty_id], [party_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_2_3_11_24_26_25')
--CREATE STATISTICS [_dta_stat_1715133501_1_2_3_11_24_26_25] ON [Claim_RI_Arrangement_Line]([claim_id], [ri_arrangement_line_id], [ri_arrangement_id], [priority], [claim_ri_arrangement_line_id], [version_id], [base_claim_ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_1_5_6_7_2_3_11_24')
--CREATE STATISTICS [_dta_stat_1715133501_1_5_6_7_2_3_11_24] ON [Claim_RI_Arrangement_Line]([claim_id], [treaty_id], [party_cnt], [xol_arrangement_id], [ri_arrangement_line_id], [ri_arrangement_id], [priority], [claim_ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1715133501_5_6_7_1_26_25_2_3_11_24')
--CREATE STATISTICS [_dta_stat_1715133501_5_6_7_1_26_25_2_3_11_24] ON [Claim_RI_Arrangement_Line]([treaty_id], [party_cnt], [xol_arrangement_id], [claim_id], [version_id], [base_claim_ri_arrangement_line_id], [ri_arrangement_line_id], [ri_arrangement_id], [priority], [claim_ri_arrangement_line_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_19_1')
--CREATE STATISTICS [_dta_stat_275128371_19_1] ON [Claim_Receipt_Item]([version_id], [claim_receipt_item_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_1_18_19')
--CREATE STATISTICS [_dta_stat_275128371_1_18_19] ON [Claim_Receipt_Item]([claim_receipt_item_id], [base_claim_receipt_item_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_1_3_5_4_6_10')
--CREATE STATISTICS [_dta_stat_275128371_1_3_5_4_6_10] ON [Claim_Receipt_Item]([claim_receipt_item_id], [recovery_id], [reserve_id], [recovery_type_id], [currency_id], [exchange_rate_override_reason_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_275128371_1_2_3_5_4_6_10')
--CREATE STATISTICS [_dta_stat_275128371_1_2_3_5_4_6_10] ON [Claim_Receipt_Item]([claim_receipt_item_id], [claim_receipt_id], [recovery_id], [reserve_id], [recovery_type_id], [currency_id], [exchange_rate_override_reason_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_1_34')
--CREATE STATISTICS [_dta_stat_83127687_1_34] ON [Claim_Receipt]([claim_receipt_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_33_2')
--CREATE STATISTICS [_dta_stat_83127687_33_2] ON [Claim_Receipt]([base_claim_receipt_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_10_2')
--CREATE STATISTICS [_dta_stat_83127687_10_2] ON [Claim_Receipt]([insured_domiciled], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_1_33')
--CREATE STATISTICS [_dta_stat_83127687_1_33] ON [Claim_Receipt]([claim_receipt_id], [base_claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_3_2_1')
--CREATE STATISTICS [_dta_stat_83127687_3_2_1] ON [Claim_Receipt]([claim_peril_id], [claim_id], [claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_3_34_2')
--CREATE STATISTICS [_dta_stat_83127687_3_34_2] ON [Claim_Receipt]([claim_peril_id], [version_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_3_1_33')
--CREATE STATISTICS [_dta_stat_83127687_3_1_33] ON [Claim_Receipt]([claim_peril_id], [claim_receipt_id], [base_claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_34_1_33_2')
--CREATE STATISTICS [_dta_stat_83127687_34_1_33_2] ON [Claim_Receipt]([version_id], [claim_receipt_id], [base_claim_receipt_id], [claim_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_2_3_34_1')
--CREATE STATISTICS [_dta_stat_83127687_2_3_34_1] ON [Claim_Receipt]([claim_id], [claim_peril_id], [version_id], [claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_34_3_1_33')
--CREATE STATISTICS [_dta_stat_83127687_34_3_1_33] ON [Claim_Receipt]([version_id], [claim_peril_id], [claim_receipt_id], [base_claim_receipt_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_83127687_2_1_33_3_34')
--CREATE STATISTICS [_dta_stat_83127687_2_1_33_3_34] ON [Claim_Receipt]([claim_id], [claim_receipt_id], [base_claim_receipt_id], [claim_peril_id], [version_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_1723869208_1_2_4_5_9_11_13_15')
--CREATE STATISTICS [_dta_stat_1723869208_1_2_4_5_9_11_13_15] ON [Stats_Detail]([stats_folder_cnt], [stats_detail_id], [risk_id], [risk_type_id], [peril_type_id], [policy_section_type_id], [class_of_business_id], [tax_type_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_918294331_4')
--CREATE STATISTICS [_dta_stat_918294331_4] ON [Insurance_File_System]([date_created])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_918294331_1_4')
--CREATE STATISTICS [_dta_stat_918294331_1_4] ON [Insurance_File_System]([insurance_file_cnt], [date_created])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_727673640_3_1')
--CREATE STATISTICS [_dta_stat_727673640_3_1] ON [GIS_User_Def_Detail]([caption_id], [gis_user_def_detail_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_727673640_2_9')
--CREATE STATISTICS [_dta_stat_727673640_2_9] ON [GIS_User_Def_Detail]([gis_user_def_header_id], [GIS_user_def_header_inds_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_727673640_1_2_3')
--CREATE STATISTICS [_dta_stat_727673640_1_2_3] ON [GIS_User_Def_Detail]([gis_user_def_detail_id], [gis_user_def_header_id], [caption_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_727673640_4_1_9')
--CREATE STATISTICS [_dta_stat_727673640_4_1_9] ON [GIS_User_Def_Detail]([code], [gis_user_def_detail_id], [GIS_user_def_header_inds_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_727673640_1_9_2_4')
--CREATE STATISTICS [_dta_stat_727673640_1_9_2_4] ON [GIS_User_Def_Detail]([gis_user_def_detail_id], [GIS_user_def_header_inds_id], [gis_user_def_header_id], [code])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2132918670_1_51')
--CREATE STATISTICS [_dta_stat_2132918670_1_51] ON [Account]([account_id], [account_key])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2132918670_9_51')
--CREATE STATISTICS [_dta_stat_2132918670_9_51] ON [Account]([ledger_id], [account_key])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2132918670_1_9_51')
--CREATE STATISTICS [_dta_stat_2132918670_1_9_51] ON [Account]([account_id], [ledger_id], [account_key])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_2132918670_56_51_1')
--CREATE STATISTICS [_dta_stat_2132918670_56_51_1] ON [Account]([client_money_calc_account_type], [account_key], [account_id])
--go

-- *****************************************************************************
-- * Author:       Surender Singh
-- * Date:         7 April 2009
-- * Purpose:      Tech Spec - LOA010 Claim payment Improvements
-- *****************************************************************************
EXECUTE DDLAddColumn 'claim_payment','our_ref','varchar(30) NULL', @BQuiet=1
go

-- *****************************************************************************
-- * Author:       Surender Singh
-- * Date:         9 April 2009
-- * Purpose:      EFT Number Final
-- *****************************************************************************
EXECUTE DDLAddColumn 'MediaType', 'numbering_scheme_id', 'int NULL', @bQuiet = 1
go

EXEC DDLAddForeignKey @sTableName='MediaType', @sColumnName1='numbering_scheme_id',
@sRefTableName = 'numbering_scheme',@sRefColumnName1='numbering_scheme_id'
go

EXECUTE DDLAddColumn 'MediaType', 'is_readonly', 'tinyint NULL', @bQuiet = 1
go

EXECUTE DDLAddColumn 'Numbering_Scheme', 'date_last_generated', 'datetime NULL', @bQuiet = 1
go

EXECUTE DDLAddColumn 'Numbering_Scheme', 'is_reset_daily', 'smallint NULL', @bQuiet = 1
go

EXECUTE DDLAddColumn 'Numbering_Scheme_history', 'date_last_generated', 'datetime NULL', @bQuiet = 1
go

EXECUTE DDLAddColumn 'Numbering_Scheme_history', 'is_reset_daily', 'smallint NULL', @bQuiet = 1
go

-- *****************************************************************************  
-- * Author:       Prabodh Mishra
-- * Date:          14 Apr 2009
-- * Purpose:     PN58172
-- *****************************************************************************
--Exec DDLAddIndex 'claim', 'policy_number' 
--GO

-- *****************************************************************************  
-- * Author:       Gaurav Arora
-- * Date:          
-- * Purpose:     PGR003 - 2'nd Version
-- *****************************************************************************

--Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
DDLAddColumn 'Party_Bank', 'cc_tracking_number','VARCHAR(255) NULL',1
GO

DDLAddColumn 'Party_Bank_History', 'cc_tracking_number','VARCHAR(255) NULL',1
GO
--End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)


-- *****************************************************************************  
-- * Author:       Gaurav Arora
-- * Date:          
-- * Purpose:     TRAC 3039
-- *****************************************************************************

EXEC DDLAddColumn 'PMUser','user_config_xml_dataset','Text NULL',@bQuiet = 1
GO
--Start (Prakash Varghese) - (Tech Spec - WCR044 - Bill History.doc) - (5.1.1)
--EXEC DDLAddIndex @sTableName='PFInstalments', @sColumnName1='PFTransaction_Id'
--GO
--End (Prakash Varghese) - (Tech Spec - WCR044 - Bill History.doc) - (5.1.1)

-- *****************************************************************************  
-- * Author:       Prabodh Mishra
-- * Date:          01 Jun 2009
-- * Purpose:     PN59895
-- *****************************************************************************
--Exec DDLADDINDEX 'claim', 'policy_id' 
--GO

-- *****************************************************************************  
-- * Author:       Prabodh Mishra
-- * Date:          13 Jul 2009
-- * Purpose:     PN53347
-- *****************************************************************************
-- TAKE OFF PK
DDLDropPrimaryKey 'ACTNumber', 'actnumber_id', 'actnumber_range_id', 'company_id'
GO

-- ADD NEW IDENTITY
DDLAddColumn 'ACTNumber', 'AutoActNumber_ID', 'INT IDENTITY'
GO

-- ADD PK
DDLAddPrimaryKey  'ACTNumber', 'AutoActNumber_ID'
GO

-- *****************************************************************************
-- * Author:      Gaurav Arora
-- * Date:        04 Sept 2009
-- * Purpose: Add Original Due Date column to PFInstalments table
-- 		  DueDate Column is overwritten at the of failure in 
--		  collection. Now to Cancel a policy from the 
--		  DueDate of failed instalment we need to pick it 
--		  from this newly added column
-- *****************************************************************************
EXEC DDLAddColumn 'PFInstalments', 'original_DueDate', 'datetime NULL'
GO

--*****************************************************************************
-- * Author:   Gurucharan Gulati
-- * Date:     23 Sept 2009
-- * Purpose:  Increase column precision up to 10 decimal places.
-- *****************************************************************************

EXEC DDLAlterColumn 'TransDetail','currency_base_xrate','numeric(19, 10)',0
GO


--*****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     10 Nov 2009
-- * Purpose:  PGR 8.8 Renewals.
-- *****************************************************************************
EXEC DDLAddColumn 'Product','renewal_period','INT'
GO

IF EXISTS
    (
        SELECT
            NULL
        FROM sysobjects so
        JOIN syscolumns sc
            ON sc.id = so.id
            AND sc.name = 'renewal_weeks'
        WHERE so.name = 'product'
        AND so.xtype = 'U'
    )
BEGIN
    DECLARE @SQL AS VARCHAR(1000)
    
    SELECT @SQL = 'UPDATE Product SET renewal_period = renewal_weeks * 7'

    EXEC (@SQL)
END

EXEC DDLDropColumn 'Product','renewal_weeks'

GO

-- *****************************************************************************
-- * Author:   Krishan Kumar Gaurav
-- * Date:     04/11/2009
-- * Purpose:  Add batch_id,visible_from_web columns to Doc_Document table
-- *****************************************************************************
EXEC DDLAddColumn 'Doc_Document', 'batch_id', 'int NULL'
GO

EXEC DDLAddColumn 'Doc_Document', 'visible_from_web', 'bit NULL'
GO

-- *****************************************************************************
-- * Author:   Krishan Kumar Gaurav
-- * Date:     04/11/2009
-- * Purpose:  Add archive_as_text columns to Document_Template table
-- *****************************************************************************
EXEC DDLAddColumn 'Document_Template', 'archive_as_text', 'bit NOT NULL DEFAULT(0)'
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora
-- * Date:     12/11/2009
-- * Purpose:  New use_nb_payment_term_at_renselection column in Product table
-- *****************************************************************************
EXEC DDLAddColumn 'Product', 'use_nb_payment_term_at_renselection', 'TINYINT NOT NULL DEFAULT(0)'
GO

-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     13/11/2009
-- * Purpose:  PGR 8.1 Batch Notification
-- *****************************************************************************

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Batch_Notification_Status' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		Batch_Notification_Status(
						 Batch_Notification_Status_Id INT IDENTITY NOT NULL,
						 caption_id INT NULL ,
						 is_deleted BIT NOT NULL ,
						 effective_date DATETIME NOT NULL ,
						 description VARCHAR (255)NULL ,
						 code CHAR(10) NOT NULL 
						)	
	
	EXEC DDLAddPrimaryKey @sTableName='Batch_Notification_Status', @sColumnName1='Batch_Notification_Status_ID'
	EXEC DDLAddIndex 'Batch_Notification_Status', 'Code'
	EXEC DDLAddIndex 'Batch_Notification_Status', 'caption_id'
	
	DECLARE @lCaptionID INTEGER
	EXEC spu_pm_caption_id_return 1, 'New', @lCaptionID output
	INSERT INTO Batch_Notification_Status(caption_id, code, description,is_deleted, effective_date)
		VALUES (@lCaptionID, 'New', 'New',0, '2007-01-01')
	
	EXEC spu_pm_caption_id_return 1, 'Complete', @lCaptionID output
	INSERT INTO Batch_Notification_Status(caption_id, code, description,is_deleted, effective_date)
	    VALUES (@lCaptionID, 'Complete', 'Complete',0, '2007-01-01')
	
	EXEC spu_pm_caption_id_return 1, 'Failed', @lCaptionID output
	INSERT INTO Batch_Notification_Status(caption_id, code, description,is_deleted, effective_date)
		VALUES (@lCaptionID, 'Failed', 'Failed',0, '2007-01-01')
END
GO


DECLARE @Exists int
EXECUTE @Exists = DDLExistsTable 'Batch_Notification_Item' 
IF @Exists=0
BEGIN
CREATE TABLE 
	Batch_Notification_Item(
					   Batch_Notification_Item_Id INT IDENTITY NOT NULL,
					   Batch_Id INT,
					   Party_Key INT,
					   Insurance_File_Key INT,
					   insurance_folder_key INT,
					   claim_key INT,
					   batch_notification_status_id INT,
					   failure_text varchar(255)
					  )	
END
GO
EXEC DDLAddForeignKey @sTableName='Batch_Notification_Item', @sColumnName1='Batch_Id', @sRefTableName='Batch', @sRefColumnName1='Batch_Id'
GO
EXEC DDLAddForeignKey @sTableName='Batch_Notification_Item', @sColumnName1='batch_notification_status_id', @sRefTableName='batch_notification_status', @sRefColumnName1='batch_notification_status_id'
GO	

-- *****************************************************************************
-- * Author:        Khalid Naseem
-- * Date:          20/11/2009
-- * Purpose:       PN 65775
-- *****************************************************************************
EXEC DDLAddColumn 'TransDetail','PFInstalments_id','INT NULL'
GO
EXEC DDLAddForeignKey @sTableName='TransDetail', @sColumnName1='PFInstalments_id',
@sRefTableName = 'PFInstalments',@sRefColumnName1 = 'pfinstalments_id'
GO

-- *****************************************************************************  
-- * Author:      Amit Kumar
-- * Date:        7 Sep 2009
-- * Purpose:     Tech Spec - WPR12 - Enhancement Quote Collection Process 
-- *****************************************************************************

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'ChequeType' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		ChequeType(
			 ChequeType_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(10) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)	

END

EXEC DDLAddPrimaryKey @sTableName ='ChequeType', @sColumnName1 ='ChequeType_id' 

GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Cheque_Clearing_Type' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		Cheque_Clearing_Type(
			 Cheque_Clearing_Type_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(10) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)	

END

EXEC DDLAddPrimaryKey @sTableName ='Cheque_Clearing_Type', @sColumnName1 ='Cheque_Clearing_Type_id' 
GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Type_of_Card' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		Type_of_Card (
			 Type_of_Card_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(10) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)	
END
GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Insurance_File_Payment_Details' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		Insurance_File_Payment_Details (
			 Insurance_file_cnt INT NOT NULL,
			 CashListItem_id INT NULL ,
			 Document_id int NULL ,
			 Transdetail_id int NULL,
			 Amount money
			)	




INSERT INTO Insurance_file_payment_details Select DISTINCT DMT.Insurance_File_Cnt,CLI.cashlistitem_id,TDL.document_id,TDL.transdetail_id,sum(ADT1.alloc_ccy_amount) from
CashListItem CLI  
 INNER JOIN CashList CLS  
   ON CLS.CashList_Id=CLI.CashList_Id  
  INNER JOIN CashListType CLT  
   ON CLT.CashListType_Id=CLS.CashListType_id  
   AND CLT.Code='R'  
  INNER JOIN TransDetail TDL  
   ON TDL.TransDetail_Id=CLI.TransDetail_ID 
  INNER JOIN AllocationDetail ADT  
   ON ADT.TransDetail_Id=TDL.TransDetail_Id
  INNER JOIN AllocationDetail ADT1  
   ON ADT.Allocation_Id=ADT1.Allocation_Id  
   AND ADT1.TransDetail_Id <> ADT.TransDetail_Id
  INNER  JOIN TransDetail TDL1  
   ON TDL1.TransDetail_Id=ADT1.TransDetail_ID 
  INNER JOIN Document DMT  
   ON DMT.Document_Id=TDL1.Document_Id
WHERE DMT.Insurance_file_cnt is not null
AND DMT.Insurance_file_cnt not in (Select Insurance_file_cnt from Insurance_file_payment_details)
GROUP BY DMT.Insurance_File_Cnt,CLI.cashlistitem_id,TDL.document_id,TDL.transdetail_id
order by DMT.Insurance_file_cnt


END

EXEC DDLAddForeignKey @sTableName='Insurance_File_Payment_Details', @sColumnName1='Insurance_file_cnt', @sRefTableName='Insurance_file', @sRefColumnName1='Insurance_file_cnt'
EXEC DDLAddForeignKey @sTableName='Insurance_File_Payment_Details', @sColumnName1='CashListItem_id', @sRefTableName='CashListItem', @sRefColumnName1='CashListItem_id'
EXEC DDLAddForeignKey @sTableName='Insurance_File_Payment_Details', @sColumnName1='Document_id', @sRefTableName='Document', @sRefColumnName1='Document_id'
EXEC DDLAddForeignKey @sTableName='Insurance_File_Payment_Details', @sColumnName1='Transdetail_id', @sRefTableName='Transdetail', @sRefColumnName1='Transdetail_id'
GO

EXEC DDLAddColumn 'CashListItem','bank_location', 'Varchar(100)'
EXEC DDLAddColumn 'CashListItem','bank_branch',  'Varchar(50)'
EXEC DDLAddColumn 'CashListItem','chequetype_id', 'int'
EXEC DDLAddColumn 'CashListItem','Cheque_clearing_type_id', 'int'
EXEC DDLAddColumn 'CashListItem','cc_bank_id', 'int'
EXEC DDLAddColumn 'CashListItem','type_of_card_id', 'int'
EXEC DDLAddColumn 'CashListItem','cc_trans_slip_no', 'Varchar(20)'
GO

EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='chequetype_id', @sRefTableName='ChequeType', @sRefColumnName1='chequetype_id'
EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='Cheque_clearing_type_id', @sRefTableName='Cheque_Clearing_Type', @sRefColumnName1='Cheque_clearing_type_id'
Go

EXEC DDLAddColumn 'MediaType', 'Is_Additional_details', 'tinyint'
GO

EXEC DDLAddColumn 'Insurance_File', 'marked_for_collection', 'tinyint'
EXEC DDLAddColumn 'Insurance_File', 'marked_date', 'datetime'
GO

-- *****************************************************************************
-- * Author:   Khalid Naseem	
-- * Date:     17/11/2009
-- * Purpose:  Add Column batch_id to Insurance_file table
-- *****************************************************************************

EXEC DDLAddColumn 'Insurance_File','batch_id', 'int NULL'
GO

-- *****************************************************************************
-- * Author:   Amit Kumar
-- * Date:     30 Nov 2009
-- * Purpose:  RFC PLICO 14 - Manual Discount & Loading.
-- *****************************************************************************

EXEC DDLAddOrAlterColumn 'Insurance_File', 'manual_discount_percentage', 'numeric(11, 8) NULL'
GO

-- *****************************************************************************
-- * Author:   Gurucharan
-- * Date:     03 Dec 2009
-- * Purpose:  PGR8.5 Automated Batch Cycle
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Report_Scheduler'
IF @bExists = 0 BEGIN
    CREATE TABLE Report_Scheduler(
	[report_scheduler_id]			INT PRIMARY KEY IDENTITY,
	[report_id]						INT NOT NULL CONSTRAINT FK__Report_Scheduler__report_id FOREIGN KEY REFERENCES Report(Report_id),
	[frequency]						VARCHAR(50) NOT NULL,
	[export_pdf]					TINYINT DEFAULT(0),
	[archieve_pdf]					TINYINT DEFAULT(0),
	[export_csv]					TINYINT DEFAULT(0),
	[reportpath]					VARCHAR(100)							
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Report_Scheduler_Parameters'
IF @bExists = 0 BEGIN
	CREATE TABLE Report_Scheduler_Parameters(
	[report_scheduler_parameter_id]	INT PRIMARY KEY IDENTITY,
	[report_scheduler_id]			INT NOT NULL CONSTRAINT FK__Report_Scheduler_Parameters__report_scheduler_id FOREIGN KEY REFERENCES Report_Scheduler(report_scheduler_id), 
	[parameter_name] 				VARCHAR(100) NOT NULL,
	[default_value]					VARCHAR(100) NOT NULL,
	[data_type]						VARCHAR(100) NOT NULL,
	[prompt] 						VARCHAR(100) NOT NULL,
	[currentid_value] 				VARCHAR(100) NOT NULL,
	[party_search] 					VARCHAR(100) NOT NULL,
	[empty] 						VARCHAR(100) NOT NULL,
	[is_automatic]					TINYINT DEFAULT(0)
	)
END 
GO

-- *****************************************************************************
-- * Author:   Gurucharan
-- * Date:     03 Dec 2009
-- * Purpose:  PGR8.5 Automated Batch Cycle
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Scheduled_Report_Frequency' AND xtype = 'U')
BEGIN
    CREATE TABLE Scheduled_Report_Frequency
    (
        scheduled_report_frequency_id 		INT 		PRIMARY KEY IDENTITY,
		code 				VARCHAR(10)	NOT NULL,
		description 			VARCHAR(50) 	NOT NULL,
        caption_id 			INT 		NOT NULL,
		effective_date 			DATETIME 	NOT NULL,
        is_deleted 			TINYINT 	NOT NULL,
		is_closed 			TINYINT 	NOT NULL
          
    )
END
GO
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Scheduled_Report_Frequency')
BEGIN
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'Scheduled_Report_Frequency', 3, 0)
END
GO
-- *****************************************************************************
-- * Purpose:  Remove the STATISTICS
-- *****************************************************************************

DECLARE @sql NVARCHAR(MAX)
DECLARE statCursor CURSOR FOR 
SELECT 
    'DROP STATISTICS ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) 
                        + '.' + QUOTENAME(t.name) 
                        + '.' + QUOTENAME(st.name) AS sql
 FROM
    sys.stats AS st 
    INNER JOIN sys.tables AS t
        ON st.object_id = t.object_id 
  WHERE
    t.name IN( 'Agent_Commission' , 'doc_folder' , 'Account','Claim_RI_Arrangement_Line' ) and st.user_created = 1   
ORDER BY 1;

OPEN statCursor;

FETCH NEXT FROM statCursor INTO @sql
WHILE @@FETCH_STATUS = 0  
BEGIN  
    PRINT @sql
    EXEC sp_executesql @sql
    FETCH NEXT FROM statCursor INTO @sql
END  
CLOSE statCursor  
DEALLOCATE statCursor
GO

-- *****************************************************************************


--Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.1.1)
-- *****************************************************************************
-- * Author:   Prakash Varghese
-- * Date:     11/12/2009
-- * Purpose:  To map RI arrangement line Id to Stats_Detail table
-- *****************************************************************************
EXEC DDLAddColumn 'Stats_Detail', 'ri_arrangement_line_Id', 'int NULL'
GO
--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.1.1)

--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     6 Dec 2009
-- * Purpose:  PGR 8.11.
-- *****************************************************************************

DDLADDColumn 'PMB_Doc_Link','generate_through_BO', 'INT NOT NULL default 1'
GO
DDLADDColumn 'PMB_Doc_Link','generate_through_SAM', 'INT NOT NULL default 1'
GO

EXEC DDLAddColumn 'Peril_Type', 'is_stamp_duty_insurer', 'tinyint'
EXEC DDLAddColumn 'Peril_Type', 'is_stamp_duty_insured', 'tinyint'

Go

EXEC DDLAddColumn 'Product', 'is_roundoff_to_zero', 'tinyint'
GO
--End - Sankar - (WPR67 Paralleling)

--Start - Renuka - (WPR64 Paralleling)

EXECUTE DDLAddColumn 'Commission_Arrangement', 'Maximum_rate', 'Numeric(19,4) NULL', 1
GO

EXECUTE DDLAddColumn 'User_Authorities', 'Edit_Default_Commission', 'Tinyint DEFAULT 0 NOT NULL ', 1
GO

EXECUTE DDLAddColumn 'Agent_Commission', 'Maximum_rate', 'Numeric(19,4) NULL', 1
GO

EXECUTE DDLAddColumn 'Agent_Commission', 'is_value', 'TINYINT NULL', 1
GO


IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='User_Authorities'AND COLUMN_NAME ='Edit_Default_Commission')
BEGIN
	UPDATE User_Authorities SET Edit_Default_Commission = 1 
END

--End - Renuka - (WPR64 Paralleling)
GO

if not exists(SELECT * FROM  syscolumns WHERE id = OBJECT_ID('RI_Model_Line') and name = 'Is_Obligatory')

EXECUTE DDLAddColumn 'RI_Model_Line', 'Is_Obligatory', 'TinyInt NULL', @bQuiet = 1
GO

if not exists(SELECT * FROM  syscolumns WHERE id = OBJECT_ID('Audit_RI_Model_Line') and name = 'Is_Obligatory')

EXECUTE DDLAddColumn 'Audit_RI_Model_Line', 'Is_Obligatory', 'TinyInt NULL', @bQuiet = 1

GO

if not exists(SELECT * FROM  syscolumns WHERE id = OBJECT_ID('Audit_RI_Model_Line') and name = 'premium_calculation_basis_id')
EXEC DDLAddColumn 'Audit_RI_Model_Line', 'premium_calculation_basis_id', 'integer NULL'
EXEC DDLAddForeignKey 'Audit_RI_Model_Line', 'premium_calculation_basis_id', @sRefTableName = 'premium_calculation_basis'
go

if not exists(SELECT * FROM  syscolumns WHERE id = OBJECT_ID('RI_Arrangement_Line') and name = 'Is_Obligatory')
EXECUTE DDLAddColumn 'RI_Arrangement_Line', 'Is_Obligatory', 'TinyInt NULL', @bQuiet = 1
GO

if not exists(SELECT * FROM  syscolumns WHERE id = OBJECT_ID('Claim_RI_Arrangement_Line') and name = 'Is_Obligatory')
EXECUTE DDLAddColumn 'Claim_RI_Arrangement_Line', 'Is_Obligatory', 'TinyInt NULL', @bQuiet = 1
GO
--Start - Renuka - (WPR87 Paralleling)
EXECUTE DDLAddColumn 'Numbering_Scheme','Is_Reset_Number','TINYINT NOT NULL DEFAULT 0', @BQuiet=1
EXECUTE DDLAddColumn 'Numbering_Scheme_History','Is_Reset_Number','TINYINT NOT NULL DEFAULT 0', @BQuiet=1
EXECUTE DDLAddColumn 'Product','Change_Ren_Policy_No_Auto','TINYINT NOT NULL DEFAULT 0', @BQuiet=1

DECLARE @Exists int
EXECUTE @Exists = DDLExistsTable 'Period_Next_Number' 
IF @Exists=0
BEGIN
CREATE TABLE 
	Period_Next_Number(
					   Numbering_Scheme_Id INT,
					   Year_Name VARCHAR(10),
					   Next_Number INT
					  )	
END
GO
EXEC DDLAddForeignKey @sTableName='Period_Next_Number', @sColumnName1='Numbering_Scheme_Id', @sRefTableName='Numbering_Scheme', @sRefColumnName1='Numbering_Scheme_Id'
GO	

--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     20 Jan 2010
-- * Purpose:  WPR VB64
-- *****************************************************************************



--Start - Sankar - (WPRvb64 Media Type Status) - Paralleling

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'MediaType_Status' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		MediaType_Status(
						 MediaType_Status_Id INT IDENTITY NOT NULL,
						 caption_id INT NULL ,
						 is_deleted BIT NOT NULL ,
						 effective_date DATETIME NOT NULL ,
						 description VARCHAR (255)NULL ,
						 code CHAR(10) NOT NULL 
						)	
	
	EXEC DDLAddPrimaryKey @sTableName='MediaType_Status', @sColumnName1='MediaType_Status_Id'
	EXEC DDLAddIndex 'MediaType_Status', 'Code'
	EXEC DDLAddIndex 'MediaType_Status', 'caption_id'
	
	DECLARE @lCaptionID INTEGER
	EXEC spu_pm_caption_id_return 1, 'Sent For Clearance', @lCaptionID output
	INSERT INTO MediaType_Status(caption_id, code, description,is_deleted, effective_date)
		VALUES (@lCaptionID, 'SRPS', 'Sent For Clearance',0, '2007-01-01')
	
	EXEC spu_pm_caption_id_return 1, 'Cleared', @lCaptionID output
	INSERT INTO MediaType_Status(caption_id, code, description,is_deleted, effective_date)
	    VALUES (@lCaptionID, 'SRPC', 'Cleared',0, '2007-01-01')
	
	EXEC spu_pm_caption_id_return 1, 'Bounced', @lCaptionID output
	INSERT INTO MediaType_Status(caption_id, code, description,is_deleted, effective_date)
		VALUES (@lCaptionID, 'SRPB', 'Bounced',0, '2007-01-01')
END
GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Receipt_MediaType_Status_History' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		Receipt_MediaType_Status_History(
						 Receipt_MediaType_Status_History_Id INT IDENTITY NOT NULL,
						 CashListItem_Id INT NOT NULL ,
						 Document_Ref VARCHAR(25) NULL ,
						 Insurance_File_Cnt INT NULL,
						 MediaType_Id INT NULL,
						 MediaType_Status_Id INT NULL,
						 Comments VARCHAR (255)NULL ,
						 User_Id SMALLINT NULL,
						 Date_Modified DATETIME NULL 
						)	
	
	EXEC DDLAddPrimaryKey @sTableName='Receipt_MediaType_Status_History', @sColumnName1='Receipt_MediaType_Status_History_Id'
	
	EXEC DDLAddForeignKey @sTableName='Receipt_MediaType_Status_History', @sColumnName1='CashListItem_Id', @sRefTableName='CashListItem', @sRefColumnName1='CashListItem_Id'
	EXEC DDLAddForeignKey @sTableName='Receipt_MediaType_Status_History', @sColumnName1='MediaType_Id', @sRefTableName='MediaType', @sRefColumnName1='MediaType_Id'
	EXEC DDLAddForeignKey @sTableName='Receipt_MediaType_Status_History', @sColumnName1='MediaType_Status_Id', @sRefTableName='MediaType_Status', @sRefColumnName1='MediaType_Status_Id'
	EXEC DDLAddForeignKey @sTableName='Receipt_MediaType_Status_History', @sColumnName1='User_Id', @sRefTableName='PMUser', @sRefColumnName1='User_Id'
END
GO

EXEC DDLAddColumn 'MediaType','MediaType_Status_Id','INT NULL', @BQuiet=1
GO
EXEC DDLAddForeignKey @sTableName='MediaType', @sColumnName1='MediaType_Status_Id', @sRefTableName='MediaType_Status', @sRefColumnName1='MediaType_Status_Id'
GO

--By default, the media type status is to be 'Sent for clearance' for new installation and 'Cleared for upgrade
--As per current installation process(incremental) there is no explicit way to identify whether the current installation
--is a new installation or an upgrade. So, Checking the cashlist item table for idenitfying the nature of installation
IF EXISTS (
			SELECT
				1
			FROM
				CashListItem
			WHERE
				CashListItem_Id>0
		  )
BEGIN
	--Since cashlist item table has records, current installation is an upgrade installation. 
	--Set the default Status to 'Cleared' in MediaType table	
	UPDATE
		MediaType
	SET
		MediaType_Status_Id=MTS.MediaType_Status_Id
	FROM
		MediaType_Status MTS
	WHERE
		MTS.Code='SRPC'	
		AND MediaType.MediaType_Status_Id IS NULL

END
ELSE
BEGIN
	--Since cashlist item table has records, current installation is new installation. 
	--Set the default Status to 'Sent For Clearance' in MediaType table	
	UPDATE
		MediaType
	SET
		MediaType_Status_Id=MTS.MediaType_Status_Id
	FROM
		MediaType_Status MTS
	WHERE
		MTS.Code='SRPS'	
END
GO

EXEC DDLAddColumn 'CashListItem','MediaType_Status_Id','INT NULL', @BQuiet=1
GO
EXEC DDLAddForeignKey @sTableName='CashListItem', @sColumnName1='MediaType_Status_Id', @sRefTableName='MediaType_Status', @sRefColumnName1='MediaType_Status_Id'
GO	

--Update cashlist item entries to reflect the media type status 
UPDATE
	CashListItem 
SET
	MediaType_Status_Id=MTP.MediaType_Status_Id
FROM
	CashListItem CLI
		INNER JOIN CashList CLS
		ON CLI.Cashlist_ID=CLS.CashList_ID
	INNER JOIN CashListType CLT
		ON CLT.CashListType_Id=CLS.CashListType_Id
		AND CLT.CODE='R'
	INNER JOIN MediaType MTP
		ON MTP.MediaType_ID=CLI.MediaType_ID
WHERE
	CLI.MediaType_Status_Id IS NULL
GO

EXECUTE DDLAddColumn 'Product','Check_MediaType_Status_At_Claim_Payment','TINYINT NOT NULL DEFAULT 0', @BQuiet=1
GO
EXECUTE DDLAddColumn 'Product','Check_MediaType_Status_At_Policy_Refund','TINYINT NOT NULL DEFAULT 0', @BQuiet=1
GO
--End - Sankar - (WPRvb64 Media Type Status) - Paralleling



--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     22 Jan 2010
-- * Purpose:  WPR85
-- *****************************************************************************


-- Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

EXEC DDLAddColumn 'Product', 'can_make_live_cashdeposit', 'tinyint'
GO
EXEC DDLAddColumn 'User_Authorities', 'can_make_live_cashdeposit', 'tinyint'
GO
EXEC DDLAddColumn 'Party_Agent', 'can_make_live_cashdeposit', 'tinyint'
GO

--IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Account') AND NAME ='I__Account__short_code')  
--DROP INDEX I__Account__short_code ON dbo.Account;
--GO
--IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Account') AND NAME ='missing_index_318_317_Account')  
--DROP INDEX missing_index_318_317_Account ON dbo.Account;
--GO
--IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Account') AND NAME ='I__Account__account_key')  
--DROP INDEX I__Account__account_key ON dbo.Account;
--GO
--IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Account') AND NAME ='missing_index_14239_14238_Account')  
--DROP INDEX missing_index_14239_14238_Account ON dbo.Account;
--GO
--IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Account') AND NAME ='missing_index_14275_14274_Account')  
--DROP INDEX missing_index_14275_14274_Account ON dbo.Account;
--GO
--IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Account') AND NAME ='missing_index_14299_14298_Account')  
--DROP INDEX missing_index_14299_14298_Account ON dbo.Account;
--GO
--EXECUTE DDLAddOrAlterColumn '[Account]', 'Short_Code','Char(30)', @bQuiet = 1
--go
--CREATE NONCLUSTERED INDEX I__Account__short_code ON Account (short_code); 
--GO
--CREATE NONCLUSTERED INDEX I__Account__account_key ON Account (account_key); 
--GO
--CREATE NONCLUSTERED INDEX missing_index_318_317_Account ON Account (restrict_enquiry); 
--GO
--CREATE NONCLUSTERED INDEX missing_index_14239_14238_Account ON Account (restrict_enquiry, company_id); 
--GO
--CREATE NONCLUSTERED INDEX missing_index_14275_14274_Account ON Account (restrict_enquiry, company_id, ledger_id); 
--GO
--CREATE NONCLUSTERED INDEX missing_index_14299_14298_Account ON Account (company_id); 
--GO



DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'CashDeposit' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		CashDeposit(
					CashDeposit_ID INT IDENTITY NOT NULL,
					CashDeposit_Ref VARCHAR(30) NOT NULL,
					Account_ID INT NOT NULL,
					Party_ID INT NOT NULL,
					Is_SinglePolicy TINYINT DEFAULT 0,					
					Is_Deleted TINYINT DEFAULT 0,
					Date_Created DATETIME NOT NULL,
					User_ID SMALLINT NOT NULL
					)	

	EXEC DDLAddPrimaryKey @sTableName='CashDeposit', @sColumnName1='CashDeposit_Id'
	EXEC DDLAddIndex 'CashDeposit', 'CashDeposit_Ref'
	EXEC DDLAddIndex 'CashDeposit', 'Account_ID'
	EXEC DDLAddIndex 'CashDeposit', 'Party_ID'

	EXEC DDLAddForeignKey @sTableName='CashDeposit', @sColumnName1='Account_ID', @sRefTableName='Account', @sRefColumnName1='Account_ID'
	EXEC DDLAddForeignKey @sTableName='CashDeposit', @sColumnName1='Party_ID', @sRefTableName='Party', @sRefColumnName1='Party_Cnt'
	EXEC DDLAddForeignKey @sTableName='CashDeposit', @sColumnName1='User_ID', @sRefTableName='PMUser', @sRefColumnName1='User_ID'
END
GO

--CashDepositNumber Table
--This table is to maintain the numbering scheme of CashDeposit.(CLIENTCODE-CD00X)
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'CashDepositNumber' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		CashDepositNumber(
					Party_ID INT NOT NULL,
					Next_Number INT NOT NULL
					)	
	
	EXEC DDLAddPrimaryKey @sTableName='CashDepositNumber',@sColumnName1='Party_ID', @sColumnName2='Next_Number'
	
	EXEC DDLAddForeignKey @sTableName='CashDepositNumber', @sColumnName1='Party_ID', @sRefTableName='Party', @sRefColumnName1='Party_Cnt'
END
GO

--CashDeposit_Policy_Link
--This table is used to map the policy against a CD
--Eventhough the same thing can be achieved with a query, this approach would make it easy to find the Policies agianst a CD
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'CashDeposit_Policy_Link' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		CashDeposit_Policy_Link(
					CashDeposit_ID INT NOT NULL,
					Insurance_File_Cnt INT NOT NULL
					)	
	
	EXEC DDLAddPrimaryKey @sTableName='CashDeposit_Policy_Link',@sColumnName1='CashDeposit_ID',@sColumnName2='Insurance_File_Cnt'
	
	EXEC DDLAddForeignKey @sTableName='CashDeposit_Policy_Link', @sColumnName1='CashDeposit_ID', @sRefTableName='CashDeposit', @sRefColumnName1='CashDeposit_ID'
	EXEC DDLAddForeignKey @sTableName='CashDeposit_Policy_Link', @sColumnName1='Insurance_File_Cnt', @sRefTableName='Insurance_File', @sRefColumnName1='Insurance_File_Cnt'
END
GO

--CashDeposit_Branch_Link
--To map the branches associated with a CD
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'CashDeposit_Branch_Link' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		CashDeposit_Branch_Link(
					CashDeposit_ID INT NOT NULL,
					Branch_ID INT NOT NULL
					)	
	
	EXEC DDLAddPrimaryKey @sTableName='CashDeposit_Branch_Link',@sColumnName1='CashDeposit_ID',@sColumnName2='Branch_ID'
	
	EXEC DDLAddForeignKey @sTableName='CashDeposit_Branch_Link', @sColumnName1='CashDeposit_ID', @sRefTableName='CashDeposit', @sRefColumnName1='CashDeposit_ID'
	EXEC DDLAddForeignKey @sTableName='CashDeposit_Branch_Link', @sColumnName1='Branch_ID', @sRefTableName='Source', @sRefColumnName1='Source_ID'
END
GO

--CashDeposit_Product_Link
--To map products assiciated with a CD
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'CashDeposit_Product_Link' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		CashDeposit_Product_Link(
					CashDeposit_ID INT NOT NULL,
					Product_ID INT NOT NULL
					)	
	
	EXEC DDLAddPrimaryKey @sTableName='CashDeposit_Product_Link',@sColumnName1='CashDeposit_ID',@sColumnName2='Product_ID'
	EXEC DDLAddForeignKey @sTableName='CashDeposit_Product_Link', @sColumnName1='CashDeposit_ID', @sRefTableName='CashDeposit', @sRefColumnName1='CashDeposit_ID'
	EXEC DDLAddForeignKey @sTableName='CashDeposit_Product_Link', @sColumnName1='Product_ID', @sRefTableName='Product', @sRefColumnName1='Product_ID'
END
GO

-- End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling




--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     4 Feb 2010
-- * Purpose:  WPR78
-- *****************************************************************************

--Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)

DECLARE @Exists int

Execute @Exists = DDLExistsTable 'ACTUnique_Document_Number'
 
IF @Exists=0
BEGIN
	CREATE TABLE 	ACTUnique_Document_Number(
				
						 ACTNumber_Id 			INT 	NOT NULL,
					  	 ACTUnique_Document_Number_Id   INT 	NOT NULL,
						 Document_Number_Length 	TINYINT NOT NULL
						)
	EXEC DDLAddPrimaryKey @sTableName ='ACTUnique_Document_Number', @sColumnName1 ='ACTNumber_Id' 

END
GO


--End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)

-- *****************************************************************************
-- * Author:   Prakash Varghese
-- * Date:     17/12/2009
-- * Purpose:  To facilitate agent commission tax override
-- *****************************************************************************
--Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.1.1)

EXEC DDLAddColumn 'Agent_Commission', 'is_tax_amended', 'tinyint NULL DEFAULT (0)'
GO
--End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (7.1.1)

-- *****************************************************************************
-- * Author:   Sandeep Kumar	
-- * Date:     22/04/2010
-- * Purpose:  Reports Opening Error
-- *****************************************************************************

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Report_PaymentAndReceipt') AND NAME ='missing_index_15_14_Report_PaymentAndReceipt')  
DROP INDEX missing_index_15_14_Report_PaymentAndReceipt ON dbo.Report_PaymentAndReceipt;
GO
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.Report_PaymentAndReceipt') AND NAME ='ix_Report_PaymentAndReceipt_bank_branch_id')  
DROP INDEX ix_Report_PaymentAndReceipt_bank_branch_id ON dbo.Report_PaymentAndReceipt;
GO
EXEC DDLAddOrAlterColumn 'Report_PaymentAndReceipt','bank_account_code','char(30) Not Null',@bQuiet=1
CREATE NONCLUSTERED INDEX missing_index_15_14_Report_PaymentAndReceipt ON Report_PaymentAndReceipt(documenttype_id); 
GO
EXEC DDLAddOrAlterColumn 'Report_PaymentAndReceipt','account_code','char(30) Null',@bQuiet=1
EXEC DDLADDINDEX 'Report_PaymentAndReceipt','bank_branch_id'
EXEC DDLDROPINDEX 'Report_transaction','account_code'
EXEC DDLAddOrAlterColumn 'Report_transaction','account_code','char(30) Null',@bQuiet=1
EXEC DDLADDINDEX 'Report_transaction','account_code'
GO

-- *****************************************************************************
-- * Author:      Arul Stephen
-- * Date:        30 Mar 2010
-- * Purpose:     Harmony General
-- *****************************************************************************
--Start (Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(5.1.1)
EXECUTE DDLAddColumn 'Product','ri_manual_premium_adjustment','tinyint NOT NULL Default 0'
Go
--End (Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(5.1.1)

-- *****************************************************************************
-- * Author:      Rahul Jaiswal
-- * Date:        20 May 2010
-- * Purpose:     PN 71952
-- *****************************************************************************

EXECUTE DDLAlterColumn 'Audit_ri_model','description','varchar(255)'
GO

-- *****************************************************************************
-- * Author:      Rahul Jaiswal
-- * Date:        27 May 2010
-- * Purpose:     PN 72247
-- *****************************************************************************

DECLARE @count int
SELECT @count=count(*) from Type_of_card
if @count=0
DBCC CHECKIDENT ('Type_of_card', RESEED, 1)
SELECT @count=count(*) from ChequeType
if @count=0
DBCC CHECKIDENT ('ChequeType', RESEED, 1)
SELECT @count=count(*) from Cheque_Clearing_Type
if @count=0
DBCC CHECKIDENT ('Cheque_Clearing_Type', RESEED, 1)

GO

-- *****************************************************************************
-- * Author:      Shubhankar Singh
-- * Date:        14 July 2010
-- * Purpose:     E001 - VAT Provision Account
-- *****************************************************************************
EXECUTE DDLAddColumn 'tax_band_rate','is_suspended','tinyint NOT NULL Default 0', @BQuiet=1
EXECUTE DDLAddColumn 'tax_band_rate','suspended_account_code_suffix','char(1)', @BQuiet=1
EXECUTE DDLAddColumn 'tax_band_rate','TTRIPR','tinyint NOT NULL Default 0', @BQuiet=1	--RI Payments/Recoveries
EXECUTE DDLAddColumn 'tax_calculation','tax_band_rate_id','int', @BQuiet=1
EXECUTE DDLAddColumn 'tax_calculation','is_suspended','tinyint NOT NULL Default 0', @BQuiet=1
--EXECUTE DDLAddForeignKey @sTableName='tax_calculation',@sColumnName1='tax_band_rate_id',@sRefTableName='tax_band_rate',@sRefColumnName1='tax_band_rate_id'
EXECUTE DDLAddColumn 'cashlistitem','tax_band_id','INT NULL', @BQuiet=1
GO
-- *****************************************************************************
-- * Author:   Sandeep Kumar	
-- * Date:     27/04/2010
-- * Purpose:  Round Off Error
-- *****************************************************************************

EXEC DDLAddOrAlterColumn 'allocationdetail','Round_Off_Amount','numeric(19,4) Null',@bQuiet=1
GO

-- *****************************************************************************--  --*****************************************************************************
-- * Author:   Kirti Syal
-- * Date:     06 Jul 2010
-- * Purpose:  PN72761 Increased size of field "folder_name" in "doc_folder" table
-- *****************************************************************************
--EXEC DDLDropIndex 'doc_folder','folder_name'
--GO
EXEC DDLDropIndex 'doc_folder','folder_name'
EXEC DDLAddOrAlterColumn 'doc_folder','folder_name', 'varchar(285)' 
EXEC DDLADDINDEX 'doc_folder','folder_name'
GO

-- *****************************************************************************
-- * Script Debug Authority
-- * Author: Danny Davis (Paralleled by Richard Clarke)
-- * Date: 07/09/2010
-- * Purpose: Add fields for holding user authority
-- *****************************************************************************
EXEC DDLAddColumn 'User_Authorities', 'can_user_debug_dynamic_logic_scripts', 'tinyint NULL'
EXEC DDLAddColumn 'User_Authorities', 'user_server_scripts_run_in_debug', 'tinyint NULL'
GO


-- *****************************************************************************
-- * Author:       Vivek Gupta
-- * Date:
-- * Purpose:     Allow backdated cancellation
-- *****************************************************************************

EXECUTE DDLAddColumn 'Product','allow_backdated_can','tinyint NOT NULL Default 0'
Go
GO

-- *****************************************************************************
-- * Author:   Pratima Chand	
-- * Date:    9/8/2011
-- * Purpose:  sagicor WPR 13
-- *****************************************************************************

DECLARE @Exists int
EXECUTE @Exists = DDLExistsTable 'Party_Agent_Product' 
IF @Exists=0
BEGIN
CREATE TABLE 
	Party_Agent_Product(
					   Party_Cnt INT,
					   Product_Id INT
					  )	
END
GO
EXEC DDLAddForeignKey @sTableName='Party_Agent_Product', @sColumnName1='Party_Cnt', @sRefTableName='Party', @sRefColumnName1='Party_Cnt'
EXEC DDLAddForeignKey @sTableName='Party_Agent_Product', @sColumnName1='Product_Id', @sRefTableName='Product', @sRefColumnName1='Product_Id'
GO
-- *****************************************************************************
-- * Author:   Gauri Kapoor
-- * Date:     02/08/2010
-- * Purpose:  Sagicor  WPR 14
-- *****************************************************************************
DECLARE @bExists INT
EXECUTE @bExists = Ddlexiststable 'Commission_Level'

IF @bExists = 0
  BEGIN
      CREATE TABLE Commission_level
        (
           commission_level_id INT NOT NULL IDENTITY(1,1),
           code                VARCHAR(20) NOT NULL,
           description         VARCHAR(50) NOT NULL,
           caption_id          INT NULL,
           effective_date      DATETIME NOT NULL,
           is_deleted          TINYINT NOT NULL DEFAULT 0
        )

      EXEC Ddladdprimarykey
        'Commission_Level',
        'commission_level_id'

      EXEC Ddladdindex
        'Commission_Level',
        'code'
  END

GO 

EXEC DDLADDCOLUMN
  'Commission_Arrangement',
  'Commission_Level_ID',
  'INT NOT NULL DEFAULT 0'

GO

EXEC DdlDropForeignKey
  @sTableName='commission_arrangement',
  @sColumnName1='commission_level_id'
GO

EXEC Ddladdcolumn
  'agent_commission',
  'commission_level_id',
  'int null'
GO

EXEC Ddladdforeignkey
  @sTableName='agent_commission',
  @sColumnName1='commission_level_id',
  @sRefTableName='commission_level',
  @sRefColumnName1='commission_level_id'
GO

EXEC Ddladdcolumn
  'party_agent',
  'commission_level_id',
  'int null'
GO

EXEC Ddladdforeignkey
  @sTableName='party_agent',
  @sColumnName1='commission_level_id',
  @sRefTableName='commission_level',
  @sRefColumnName1='commission_level_id'

GO  

-- *****************************************************************************  
-- * Author:       Pratima Chand	
-- * Date:         9/8/2011
-- * Purpose:      PN74415
-- *****************************************************************************
EXEC DDLDropPrimaryKey @sTableName='commission_arrangement', @sColumnName1='Party_type', @sColumnName2='party_cnt',
					@sColumnName3='Product_id', @sColumnName4='risk_type_id',
					@sColumnName5='transaction_type_id', @sColumnName6='commission_band_id',
					@sColumnName7='effective_date'

EXEC DDLAddPrimaryKey @sTableName='commission_arrangement', @sColumnName1='Party_type', @sColumnName2='party_cnt',
					@sColumnName3='Product_id', @sColumnName4='risk_type_id',
					@sColumnName5='transaction_type_id', @sColumnName6='commission_band_id',
					@sColumnName7='effective_date', @sColumnName8='commission_level_id'
GO
-- *****************************************************************************
-- * Sharepoint Intergration
-- * Author: Danny Davis
-- * Date: 08/07/2011
-- * Purpose: Add tables for Sharepoint Integration
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Document_Template_Group' AND xtype = 'U')
BEGIN
    CREATE TABLE Document_Template_Group
    (
        document_template_group_id      INT 		PRIMARY KEY IDENTITY(1,1),
        code 							varchar(20)     NOT NULL,
        description 					VARCHAR(255) 	NULL,
		caption_id 						INT			NOT NULL,
        is_deleted 						TINYINT 	NOT NULL,
        effective_date 					DATETIME 	NOT NULL
    )
End
GO

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Document_Template_Sub_Group' AND xtype = 'U')
BEGIN
    CREATE TABLE Document_Template_Sub_Group
    (
        document_template_sub_group_id  INT 		PRIMARY KEY IDENTITY(1,1),
        document_template_group_id      INT 		NOT NULL,
		code 							varchar(20)	NOT NULL,
        description 					VARCHAR(255) 	NULL,
		caption_id 						INT			NOT NULL,
        is_deleted 						TINYINT 	NOT NULL,
        effective_date 					DATETIME 	NOT NULL
    )
End
GO

EXEC DDLAddForeignKey 'Document_Template_Sub_Group', 'document_template_group_id', @sRefTableName = 'document_template_group',
@sRefColumnName1='document_template_group_id'
GO


EXEC DDLAddColumn 'event_log', 'document_library_reference', 'varchar(255)'
Go
EXEC DDLAddColumn 'document_template', 'document_template_group_id', 'integer NULL'
Go
EXEC DDLAddColumn 'document_template', 'document_template_sub_group_id', 'integer NULL'
Go
EXEC DDLAddColumn 'document_template', 'is_portal_internal_only', 'tinyint NULL'
Go
EXEC DDLAddColumn 'document_template', 'is_portal_selected_by_default', 'tinyint NULL'
Go

EXEC DDLAddForeignKey 
		@sTableName='Document_Template', 
		@sColumnName1='document_template_group_id', 
		@sRefTableName = 'document_template_group',
		@sRefColumnName1='document_template_group_id'
GO

EXEC DDLAddForeignKey 
		@sTableName='Document_Template', 
		@sColumnName1='document_template_sub_group_id', 
		@sRefTableName = 'document_template_sub_group',
		@sRefColumnName1='document_template_sub_group_id'
GO

--*****************************************************************************
-- * Author	: Ram Chandrabose
-- * Date	: 09 Mar 2011
-- * Purpose: Pure Windows Service Development (EMail Integration)
-- ****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Background_Job'
IF @bExists = 0 BEGIN
    CREATE TABLE Background_Job(
		[background_job_id] INT IDENTITY NOT NULL,
		[description] nvarchar(255) NOT NULL,
		[job_xml] nvarchar(max)	NOT NULL,
		[job_status] nvarchar(1) NOT NULL,
		[job_service] nvarchar(255) NULL,
		[job_created] Datetime NOT NULL,
		[job_when_to_start] Datetime NOT NULL,
		[job_started] Datetime NULL,
		[job_completed] Datetime NULL,
		[job_expiry] Datetime NOT NULL,
		[job_user_id] smallInt NOT NULL,
		[failure_description] nvarchar(1000) NULL
	)	
	EXEC DDLAddPrimaryKey @sTableName='Background_Job', @sColumnName1='background_job_id'
	EXEC DDLAddForeignKey @sTableName='Background_Job', @sColumnName1='job_user_id', @sRefTableName='PMUser', @sRefColumnName1='User_ID'	
	EXEC DDLAddIndex 'Background_Job','job_service'
	EXEC DDLAddIndex 'Background_Job','job_status'
	EXEC DDLAddIndex 'Background_Job','job_when_to_start'
	EXEC DDLAddIndex 'Background_Job','job_user_id'	
END
GO

--*****************************************************************************
-- * Author	: Rahul Jaiswal
-- * Date	: 17 Aug 2011
-- * Purpose	: Sagicor WPR 8_11
-- ****************************************************************************
DDLADDCOLUMN 'Party_Agent','use_override_commission_renewal','INT'
GO

DDLADDCOLUMN 'agent_commission','peril_type_id','INT'
GO
DDLADDCOLUMN 'agent_commission','is_locked','TINYINT'
GO


--*****************************************************************************
-- * Author	: Sahil Ansari
-- * Date	: 28 Aug 2011
-- * Purpose	: Sagicor WPR 63
-- ****************************************************************************
DDLADDCOLUMN 'Insurance_File','base_insurance_folder_cnt','INT'
GO
DDLADDCOLUMN 'Insurance_File','quote_version','INT'
GO
DDLADDCOLUMN 'Insurance_File','quote_status_id','INT'
GO

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Insurance_File_Delete_Log' AND xtype = 'U')
BEGIN
    CREATE TABLE Insurance_File_Delete_Log
    (
        insurance_file_cnt				INT 		NOT NULL,
        status						INT		NOT NULL,
	deletion_date 					DATETIME	NOT NULL,        
        failure_description				VARCHAR(255)	NULL
    )
End
GO

DDLAddColumn 'Insurance_File_Delete_Log' ,'insurance_ref' ,'varchar(30) NOT NULL DEFAULT 0 '
GO
DDLAddColumn 'Insurance_File_Delete_Log' ,'insured_cnt' ,'INT NOT NULL DEFAULT 0 '
GO
DDLAddColumn 'Insurance_File_Delete_Log' ,'product_id' ,'INT NOT NULL DEFAULT 0 '
GO
DDLAddColumn 'Insurance_File_Delete_Log' ,'lead_agent_cnt' ,'INT NOT NULL DEFAULT 0 '
GO

EXEC DDLAddColumn 'Insurance_File_Delete_Log', 'quote_version', 'INT NULL DEFAULT 0 '
GO

EXEC DDLAddColumn 'Insurance_File_Delete_Log', 'quote_status_description', 'varchar(255)'
GO


If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Quote_Status' AND xtype = 'U')
BEGIN
    CREATE TABLE Quote_Status
    (
        quote_status_id					INT 		PRIMARY KEY,
        caption_id					INT		NOT NULL,
        code		 				VARCHAR(10)  	NOT NULL,
	description 					VARCHAR(255) 	NULL,
        is_deleted 					TINYINT 	NULL,
        effective_date 					DATETIME 	NULL
    )


UPDATE Insurance_File SET base_insurance_folder_cnt = ISNULL(base_insurance_folder_cnt,insurance_folder_cnt) where insurance_file_type_id = 1
UPDATE Insurance_File SET quote_version = ISNULL(quote_version,1) where insurance_file_type_id = 1
UPDATE Insurance_File SET quote_status_id  = ISNULL(quote_status_id,1) where insurance_file_type_id = 1

End
GO




IF EXISTS (SELECT *  FROM sys.key_constraints   WHERE object_id = OBJECT_ID(N'AK__Insurance_Folder__code')  AND parent_object_id = OBJECT_ID(N'insurance_folder') )   
ALTER TABLE insurance_folder DROP CONSTRAINT AK__Insurance_Folder__code


-- *****************************************************************************
-- * Author:        Sahil Ansari
-- * Date:          15 Oct 2011
-- * Purpose:       WPR 51
-- *****************************************************************************
EXEC DDLAddColumn 'CashList', 'is_split_receipt', 'Tinyint NULL'
GO

EXEC DDLAddColumn 'CashListItem', 'is_lead', 'Tinyint NULL'
GO

EXEC DDLAddColumn 'CashListItem', 'split_total', 'Numeric(19,4) NULL'
GO

-- *****************************************************************************
-- * Author:        Sandeep kumar
-- * Date:          27 11 2011
-- * Purpose:       WPR 48
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Party_Certificate_Year' AND xtype = 'U')
BEGIN
    CREATE TABLE Party_Certificate_Year
    (
        Party_Certificate_Year_ID		INT 	NOT NULL PRIMARY KEY IDENTITY(1,1),
        Party_Cnt						INT		NOT NULL CONSTRAINT FK__Party_Certificate_Year__Party_cnt FOREIGN KEY References Party(Party_cnt),
		CertYearCode 					CHAR(10)	NOT NULL,        
        CertYearDescription				VARCHAR(255) NOT NULL,
        CertYearStartDate				DATETIME NOT NULL,
        CertYearEndDate					DATETIME NOT NULL,
        IsDeleted						TINYINT NOT NULL,
    )
End
GO

--*****************************************************************************
-- * Author	: Azeej Usmani
-- * Date	: 17 Oct 2011
-- * Purpose	: Sagicor WPR 73-74
-- ****************************************************************************

EXEC DDLADDCOLUMN 'Insurance_File','Contact_user_id','SMALLINT NULL'
GO
EXEC DDLAddForeignKey @sTableName='Insurance_File', @sColumnName1='Contact_user_id', @sRefTableName='PMUser', @sRefColumnName1='User_ID'	
GO

-- *****************************************************************************
-- * Author:   Danny Davis
-- * Date:     18/10/2011
-- * Purpose:  Extend Background Job for Retries
-- *****************************************************************************
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='job_retry_count', @sColumnDefinition='int NULL'
GO 

-- *****************************************************************************
-- * Author:   Danny Davis
-- * Date:     18/10/2011
-- * Purpose:  Extend Background Job to handle changing references
-- *****************************************************************************
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='party_code', @sColumnDefinition='varchar(255) NULL'
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='insurance_ref', @sColumnDefinition='varchar(255) NULL'
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='claim_number', @sColumnDefinition='varchar(255) NULL'
GO 
-- *****************************************************************************
-- * Author:   Ashish Sachdeva	
-- * Date:     14/04/2010
-- * Purpose:  WPR14 Catlin MID  
-- *****************************************************************************
--mid_policy
DECLARE @Exists INT
EXECUTE @Exists = DDLExistsTable 'mid_policy' 
IF @Exists=0
BEGIN
	CREATE TABLE	mid_policy
					(
					mid_policy_id			INT IDENTITY,
					mid_status_id			INT NOT NULL,
					insurance_file_cnt		INT NOT NULL,
					insurance_folder_cnt	INT NOT NULL,					
					update_type				VARCHAR(1) NOT NULL,
					ppcc					INT NULL,
					batch_id				INT NULL,
					reject_reference		VARCHAR(35) NULL,
					reject_error_codes		VARCHAR(255) NULL,
					ppcc_expected			INT NULL
					)	

	EXEC DDLAddPrimaryKey 'mid_policy','mid_policy_id'
	
	EXEC DDLAddForeignKey @sTableName='mid_policy',@sColumnName1= 'insurance_file_cnt',	@sRefTableName = 'insurance_file', @sRefColumnName1='insurance_file_cnt'  
	
	EXEC DDLAddForeignKey @sTableName='mid_policy',@sColumnName1= 'insurance_folder_cnt', @sRefTableName = 'insurance_folder', @sRefColumnName1='insurance_folder_cnt'  
	
	EXEC DDLAddForeignKey @sTableName='mid_policy',@sColumnName1= 'batch_id', @sRefTableName = 'batch', @sRefColumnName1='batch_id'  
	
END
GO
--mid_vehicle
DECLARE @Exists INT
EXECUTE @Exists = DDLExistsTable 'mid_vehicle' 
IF @Exists=0
BEGIN
	CREATE TABLE	mid_vehicle
					(
					mid_vehicle_id			INT IDENTITY,
					mid_policy_id			INT NOT NULL, 
					mid_status_id			INT NOT NULL,
					update_type				VARCHAR(1) NOT NULL,
					Registration			VARCHAR(12) NOT NULL,
					is_foreign_registration	BIT NOT NULL,
					is_trade_registration	BIT NOT NULL,
					Make					VARCHAR(15) NOT NULL,
					Model					VARCHAR(15) NOT NULL,
					on_date					DATETIME NOT NULL,
					off_date				DATETIME NOT NULL,
					reject_reference		VARCHAR(35) NULL,
					reject_error_codes		VARCHAR(255) NULL
					)	

	EXEC DDLAddPrimaryKey 'mid_vehicle','mid_vehicle_id'
	
	EXEC DDLAddForeignKey @sTableName='mid_vehicle',@sColumnName1= 'mid_policy_id',	@sRefTableName = 'mid_policy', @sRefColumnName1='mid_policy_id'  
	
END
GO
--mid_status
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'mid_status'
IF @bExists = 0 BEGIN
    CREATE TABLE mid_status(
	[mid_status_id] INT NOT NULL IDENTITY(1,1),
	[code] Varchar(10) NOT NULL,
	[description] Varchar(255) NOT NULL,
	[caption_id] Int NOT NULL,
	[effective_date] Datetime NOT NULL,
	[is_deleted] Tinyint NOT NULL Default 0	
	)
	
	EXEC DDLADDPrimaryKey 'mid_status','mid_status_id'
	EXEC DDLAddIndex 'mid_status','code'
END
GO
DDLAddColumn 'Insurance_File','number_of_fleet_vehicles','INT'
GO

-- *****************************************************************************
-- * Author:   Ashish Sachdeva	
-- * Date:     04/05/2010
-- * Purpose:  WPR14 Catlin MID  (For MID File Name)
-- *****************************************************************************

DDLAddColumn @sTableName = Batch, @sColumnName = import_file_name, @sColumnDefinition = 'VARCHAR(255) NULL', @BQuiet=1
GO

-- *****************************************************************************
-- * Author:   Richard Clarke
-- * Date:     19/09/2011
-- * Purpose:  WPR21 ARCH
-- *****************************************************************************

DDLAddColumn @sTableName = PFFrequency, @sColumnName = is_available_on_client_screen, @sColumnDefinition = 'TINYINT NULL', @BQuiet = 1
GO
DDLAddColumn @sTableName = PFFrequency, @sColumnName = is_available_on_instalment_screen, @sColumnDefinition = 'TINYINT NULL', @BQuiet = 1
GO

-- THIS ALTER COLUMN CAN ONLY HAPPEN FOR EXISTING CUSTOMERS AFTER THEIR PFFrequency table has had existing payment term code values added to it 
-- AND AN UPDATE SCRIPT HAS BEEN EXECUTED WHICH UDPATES ALL party.payment_term_codes to number values
-- update party set payment_term_code = (select pffrequency_id from PFFrequency where description = party.payment_term_code)
--DDLAlterColumn party, payment_term_code, 'int null'

DDLAddColumn @sTableName = transdetail, @sColumnName = due_date, @sColumnDefinition = 'DATETIME NULL', @BQuiet = 1
Go
DDLAddColumn @sTableName = credit_control_rule, @sColumnName = use_due_date, @sColumnDefinition = 'tinyint NULL', @BQuiet = 1
Go


-- *****************************************************************************
-- * Author:   Sahil Ansari
-- * Date:     29/01/2012
-- * Purpose:  Posting based on COB 
-- *****************************************************************************

EXEC DDLAddColumn 'Agent_Commission', 'class_of_business_id', 'INT NULL '
GO


--*****************************************************************************
-- * Author	: Roopaly Rastogi
-- * Date	: 23 Sep 2011
-- * Purpose: PLICO 51
-- ****************************************************************************
Exec DDLAddColumn "GIS_property","is_formatted_text",tinyint
GO

-- *****************************************************************************
-- * Author:   Danny Davis
-- * Date:     18/10/2011
-- * Purpose:  Extend Insured Name to match Party
-- *****************************************************************************
EXEC DDLAlterColumn @sTableName='Insurance_File', @sColumnName='insured_name', @sColumnDefinition='varchar(255)'
GO
EXEC DDLAlterColumn @sTableName='Event_Insurance_File', @sColumnName='insured_name', @sColumnDefinition='varchar(255)'
GO


-- *****************************************************************************
-- * Author:   Danny Davis
-- * Date:     18/10/2011
-- * Purpose:  Extend Background Job for Retries
-- *****************************************************************************
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='job_retry_count', @sColumnDefinition='int NULL'
GO 

-- *****************************************************************************
-- * Author:   Danny Davis
-- * Date:     18/10/2011
-- * Purpose:  Extend Background Job to handle changing references
-- *****************************************************************************
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='party_code', @sColumnDefinition='varchar(255) NULL'
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='insurance_ref', @sColumnDefinition='varchar(255) NULL'
EXEC DDLAddOrAlterColumn @sTableName='Background_Job', @sColumnName='claim_number', @sColumnDefinition='varchar(255) NULL'
GO

-- *****************************************************************************
-- * Author:   Danny Davis
-- * Date:     26/10/2011
-- * Purpose:  Ensure Fee records start from 1
-- *****************************************************************************
EXEC DDLReSeedIdentity 'Fee_Amounts' 
GO
-- *****************************************************************************
-- * Author:   Deepak Arora
-- * Date:     25/01/2012
-- * Purpose:  To Reduce the Time taken by spu_ACT_GLExport
-- *****************************************************************************
--DDLAddINDEX 'Claim_payment','Document_id'
--go
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     25/01/2012
-- * Purpose:  To Remove the error in Create_Manual_Journal
-- *****************************************************************************
EXEC DDLAddColumn 'PMB_Doc_Link','is_message','TINYINT'
GO


-- *****************************************************************************
-- * Author:   Sumeet Singh
-- * Date:     22/03/2012
-- * Purpose: ClientAddr1,ClientAddr2,ClientAddr3,ClientAddr4 column size increased from 25 to 60 as in the Address table.
-- *****************************************************************************
EXEC DDLDropIndex 'PFPremiumFinance','ClientAddr1'
EXEC DDLAddOrAlterColumn 'PFPremiumFinance', 'ClientAddr1', 'varchar(60)'
EXEC DDLAddIndex 'PFPremiumFinance','ClientAddr1'
GO
EXEC DDLDropIndex 'PFPremiumFinance','ClientAddr2'
EXEC DDLAddOrAlterColumn 'PFPremiumFinance', 'ClientAddr2', 'varchar(60)'
EXEC DDLAddIndex 'PFPremiumFinance','ClientAddr2'
GO
EXEC DDLDropIndex 'PFPremiumFinance','ClientAddr3'
EXEC DDLAddOrAlterColumn 'PFPremiumFinance', 'ClientAddr3', 'varchar(60)'
EXEC DDLAddIndex 'PFPremiumFinance','ClientAddr3'
GO
EXEC DDLDropIndex 'PFPremiumFinance','ClientAddr4'
EXEC DDLAddOrAlterColumn 'PFPremiumFinance', 'ClientAddr4', 'varchar(60)'
EXEC DDLAddIndex 'PFPremiumFinance','ClientAddr4'
GO

-- *****************************************************************************
-- * Author:   Shubhankar Singh
-- * Date:     18/01/2011
-- * Purpose:  WPR42
-- *****************************************************************************

EXEC DDLAddColumn 'Product', 'TMPautrenfac', 'tinyint NULL'
GO

--added by archana for WPR33
-- *****************************************************************************
-- * Author:       Krishan Kumar Gorav	
-- * Date:         8/12/2010 WPR 33 Clear Quote 
-- * Purpose:     IsDirty Flag for backdated Versions
-- *****************************************************************************

EXEC DDLAddColumn 'mta_insurance_file_link', 'IsDirty', 'bit NULL' 
Go
-- *****************************************************************************
-- * Author:       Vidya Rangdale	
-- * Date:         02/04/2012 
-- * Purpose:     PN Number 2053
-- *****************************************************************************

--Drop Constaints
DDLDropForeignKey  'tax_band_rate' ,'class_of_business_id'
go
DDLDropForeignKey  'tax_band_rate' ,'country_id'
go
DDLDropForeignKey  'tax_band_rate' ,'currency_id'
go
DDLDropForeignKey  'tax_band_rate' ,'state_id'
go
DDLDropForeignKey  'tax_band_rate' ,'tax_band_id'
go
DDLDropForeignKey  'tax_calculation' ,'tax_band_rate_id'
go



--DDLDropIndex 'tax_band_rate','caption_id'
--go
--DDLDropIndex 'tax_band_rate','code'
--go
--DDLDropIndex 'tax_band_rate','tax_band_id'
--go
--DDLDropIndex 'tax_band_rate','tax_band_rate_id'
--go
DDLDropPrimarykey 'tax_band_rate','tax_band_rate_id'
go

--Add Identity
ddladdidentitycolumn 'tax_band_rate', 'tax_band_rate_id'
go

--Add Default Zeros
ALTER TABLE tax_band_rate ADD  CONSTRAINT taxbandrate_isSuspended_default  DEFAULT ('0') FOR  is_suspended
go
ALTER TABLE tax_band_rate ADD  CONSTRAINT taxbandrate_TTRIPR_default  DEFAULT ('0') FOR  TTRIPR
go


-- Now Add all Constraints

DDLAddForeignKey @sTableName='tax_band_rate',@sColumnName1='class_of_business_id',@sRefTableName='class_of_business',@sRefColumnName1='class_of_business_id'

go
DDLAddForeignKey @sTableName='tax_band_rate',@sColumnName1='country_id',@sRefTableName='country',@sRefColumnName1='country_id'
go

DDLAddForeignKey @sTableName='tax_band_rate',@sColumnName1='currency_id',@sRefTableName='currency',@sRefColumnName1= 'currency_id'
go
DDLAddForeignKey @sTableName='tax_band_rate',@sColumnName1='state_id',@sRefTableName='state',@sRefColumnName1='state_id'
go
DDLAddForeignKey @sTableName='tax_band_rate',@sColumnName1='tax_band_id',@sRefTableName='tax_band',@sRefColumnName1='Tax_band_id'
go

DDLaddprimarykey @sTableName= 'tax_band_rate',@sColumnName1= 'tax_band_rate_id'  
go
--DDLAddForeignKey @sTableName='tax_calculation',@sColumnName1='tax_band_rate_id',@sRefTableName='tax_band_rate',@sRefColumnName1= 'tax_band_rate_id'
--go

--DDLAddIndex 'tax_band_rate', 'caption_id' 
--go
--DDLAddIndex 'tax_band_rate', 'code'
--go
--DDLAddIndex 'tax_band_rate', 'tax_band_id'
--go
--DDLAddIndex 'tax_band_rate', 'tax_band_rate_id'
--go




-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  Increase client_name and insurer_name
-- *****************************************************************************

EXEC DDLALTERCOLUMN 'claim','client_name','varchar(255)'
GO

EXEC DDLALTERCOLUMN 'claim','insurer_name','varchar(255)'
GO

-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR29 - New Field for PartyKey
-- *****************************************************************************

EXEC DDLADDCOLUMN 'pmwrk_task_instance','party_cnt','INT Foreign Key references Party(Party_cnt)'
GO

-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR36 - New Field for ultimate_payee
-- *****************************************************************************

EXEC DDLADDCOLUMN 'claim_payment','ultimate_payee','VARCHAR(255)'
GO

--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR08 - New Field for TPA
-- *****************************************************************************

EXECUTE DDLADDCOLUMN 'claim','other_party_id','INT FOREIGN KEY REFERENCES Party(Party_cnt)'
GO

--*****************************************************************************
-- * Author:   Ashish Sachdeva
-- * Date:     07/06/2012
-- * Purpose:  WPR22 - New Field for Report Schedular for SeprateBy
-- *****************************************************************************

EXECUTE DDLAddColumn "Report_Scheduler","SeprateBy","VARCHAR(255) NULL",1
GO


--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     15/06/2012
-- * Purpose:  WPR40 - New Field for Account Locked and Incorrect Attempts Count
-- *****************************************************************************

EXEC DDLADDCOLUMN  'PMUser','incorrect_attempt_count','INT DEFAULT 0'
GO
EXEC DDLADDCOLUMN 'PMUser','is_locked','tinyint DEFAULT 0'
GO


Update PMUSER set incorrect_attempt_count=0
WHERE incorrect_attempt_count IS NULL
GO

Update PMUSER set is_locked=0
WHERE is_locked IS NULL
GO



-- *****************************************************************************
-- * Author:  Rahul Jaiswal
-- * Date:    26/06/2012
-- * Purpose: List Maintenance
-- *****************************************************************************


Declare @Table varchar(250)
Declare @version int
DECLARE @v_SQL nVARCHAR(4000)
Declare @ColumnExists int
Declare @TableExists int
Declare @SPName varchar(500)
Declare @SQL nvarchar(1000)

Declare UDL Cursor 
FOR select name from sys.tables where name like'%%UDl_%'
OPEN UDL

FETCH Next from UDL into @Table
WHILE @@Fetch_status = 0
BEGIN
	
	--SET @table = 'udl_'+ @table
	
	EXEC @ColumnExists = DDLExistsColumn @table,'udl_version'
	EXEC @TableExists = DDLExistsTable @table
	
	If @ColumnExists = 0 AND @TableExists = 1
	BEGIN
		EXEC DDLAddColumn  @table,'udl_version','INT NULL'	
	    IF ((select Count(*) from GIS_List_Type Where code=substring(@table,5,len(@table)-4))>0)
		BEGIN
		SET @V_SQL='SELECT @version= MAX(version)  from gis_list_type JOIN gis_list_type_usage '
		SET @V_SQL= @V_SQL + ' ON gis_list_type.gis_list_type_id = gis_list_type_usage.gis_list_type_id '
		SET @V_SQL= @V_SQL + ' Where code=''' + substring(@table,5,len(@table)-4)+ ''' group by code'
		
		EXEC SP_EXECUTESQL @V_SQL,N'@Version INT OUTPUT',@VERSION OUTPUT
		END
		ELSE
		BEGIN
		    SET @version=1
		END
		SET @V_SQL = 'UPDATE '+  @table + ' SET udl_version = '+ CONVERT(varchar,@version) + ' WHERE udl_version IS NULL'
	
		EXEC SP_EXECUTESQL @V_SQL

	--	Drop the Get_field SPs

		Declare curSP Cursor FOR
		select [Name] from sysobjects where name like 'spg_GetField__UDL%' AND XTYPE = 'P'
		open curSP
		
		Fetch next from curSP into @SPName
		While @@Fetch_status =0 
		BEGIN
			Set @SQL = ' DROP Procedure  ' + @SPName 	
			EXEC SP_EXECUTESQL @SQL
			Fetch next from curSP into @SPName
		END
		Close curSP
		Deallocate curSP

	END
	FETCH Next from UDL into @Table	

END
Close UDL
Deallocate UDL
GO

--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR08 - New Field for TPA
-- *****************************************************************************

-- *****************************************************************************
-- * Author:       Archana Tokas
-- * Date:         25 July 2012
-- * Purpose:      WPR42-Arch-Created table for Other Party Branches
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Other_Party_Branch'
IF @bExists = 0
BEGIN
    CREATE TABLE Other_Party_Branch

	(
		party_cnt		INT		NOT NULL ,
		source_id		INT		NOT NULL
		
		CONSTRAINT FK__party_cnt FOREIGN KEY (party_cnt) REFERENCES Party(party_cnt),
		CONSTRAINT FK__source_id FOREIGN KEY (source_id) REFERENCES source(source_id)
	)

 Insert into Other_Party_Branch (party_cnt,source_id)
 select party_cnt,source_id from party P where P.party_type_id in (select Party_type_id from party_type where code like'OT%' and is_deleted=0)
 and P.is_deleted=0 and P.party_cnt not in (Select party_cnt from Other_Party_Branch )
END
GO
-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         10 Aug 2012
-- * Purpose:      WPR01- Claim BDX
-- *****************************************************************************
-- * Author:      Maninder Kaur
-- * Date:        18 Jun 2012
-- * Purpose:     Add use_for_refund_when_expired and use_for_backdated_nb column to tax_band_rate table
-- *****************************************************************************
EXEC DDLAddColumn 'tax_band_rate', 'use_for_refund_when_expired', 'TINYINT NULL'
GO

EXEC DDLAddColumn 'tax_band_rate', 'use_for_backdated_nb', 'TINYINT NULL'
GO



-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  Increase client_name and insurer_name
-- *****************************************************************************

EXEC DDLALTERCOLUMN 'claim','client_name','varchar(255)'
GO

EXEC DDLALTERCOLUMN 'claim','insurer_name','varchar(255)'
GO

-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR29 - New Field for PartyKey
-- *****************************************************************************

EXEC DDLADDCOLUMN 'pmwrk_task_instance','party_cnt','INT Foreign Key references Party(Party_cnt)'
GO

-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR36 - New Field for ultimate_payee
-- *****************************************************************************

EXEC DDLADDCOLUMN 'claim_payment','ultimate_payee','VARCHAR(255)'
GO

--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR08 - New Field for TPA
-- *****************************************************************************

EXECUTE DDLADDCOLUMN 'claim','other_party_id','INT FOREIGN KEY REFERENCES Party(Party_cnt)'
GO

--*****************************************************************************
-- * Author:   Ashish Sachdeva
-- * Date:     07/06/2012
-- * Purpose:  WPR22 - New Field for Report Schedular for SeprateBy
-- *****************************************************************************

EXECUTE DDLAddColumn "Report_Scheduler","SeprateBy","VARCHAR(255) NULL",1
GO


--*****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     15/06/2012
-- * Purpose:  WPR40 - New Field for Account Locked and Incorrect Attempts Count
-- *****************************************************************************

EXEC DDLADDCOLUMN  'PMUser','incorrect_attempt_count','INT DEFAULT 0'
GO
EXEC DDLADDCOLUMN 'PMUser','is_locked','tinyint DEFAULT 0'
GO

EXECUTE DDLADDCOLUMN 'claim','other_party_id','INT FOREIGN KEY REFERENCES Party(Party_cnt)'
GO
-- *****************************************************************************
-- * Author:       Rahul Jaiswal
-- * Date:         10 Aug 2012
-- * Purpose:      WPR01- Claim BDX
-- *****************************************************************************

Update PMUSER set incorrect_attempt_count=0
WHERE incorrect_attempt_count IS NULL
GO

Update PMUSER set is_locked=0
WHERE is_locked IS NULL
GO

-- *****************************************************************************

EXEC DDLADDCOLUMN 'party_other','is_TPA_settle_directly','tinyint'
GO

-- * End of File
-- *****************************************************************************
--DDLALTERCOLUMN 'PFPremiumFinance','BankSortCode','varchar(20)'
GO
--DDLALTERCOLUMN 'PFMediaTypeHistory','BankSortCode','varchar(20)'
GO
-- *****************************************************************************
-- * Agent Collection & Commission: Added DB fileds is_gross_agent
-- * Author: Vijay Pal
-- * Date: 20/06/2012
-- *****************************************************************************
EXEC DDLAddColumn 'Party_Agent','is_gross_agent','Tinyint NOT NULL Default(0)'
GO
EXEC DDLAddColumn 'party_agent','bankaccount_id','INT NULL'
GO
EXEC DDLAddForeignKey 'party_agent', 'bankaccount_id', @sRefTableName = 'bankaccount', @sRefColumnName1='bankaccount_id'
GO

EXEC DDLAddColumn 'transmatch','InstalmentNumber','INT NULL'
GO


DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'TransDetail_Selection'
IF @bExists = 0 BEGIN
CREATE TABLE TransDetail_Selection(
[transdetail_id]				INT			NOT NULL,
[user_id]					SMALLINT		NOT NULL,
[session_guid]					Varchar(40)		NOT NULL,
[transdetail_selection_type_id] 		Int			NOT NULL,
[linked_to_transdetail_id]			Int			NULL,
[allocated_base_amount]				NUMERIC(19,4)		NULL,
[write_off_base_amount]				NUMERIC(19,4)		NULL,
[write_off_account_id]				Int			NULL,
[write_off_reason_id]				Int			NULL
)
END
GO


DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'TransDetail_Selection_Type'
IF @bExists = 0 BEGIN
CREATE TABLE TransDetail_Selection_Type(
[transdetail_selection_type_id]			INT			NOT NULL,
[code]						Varchar(20)		NOT NULL,
[description]					Varchar(255)		NOT NULL,
[caption_id]					Int			NOT NULL,
[effective_date]				DATETIME		NOT NULL,
[is_deleted]					TinyInt			NOT NULL
)

-- Add Primary Key
EXEC DDLADDPrimaryKey 'TransDetail_Selection_Type','transdetail_selection_type_id'
END
GO

Exec DDLAddForeignKey	
@sTableName = 'TransDetail_Selection', 
@sColumnName1 = 'transdetail_id', 
@sRefTableName = 'TransDetail'
GO

-- Add foreign key 
Exec DDLAddForeignKey	
@sTableName = 'TransDetail_Selection', 
@sColumnName1 = 'user_id', 
@sRefTableName = 'PMUser'
GO

-- Add foreign key 
Exec DDLAddForeignKey	
@sTableName = 'TransDetail_Selection', 
@sColumnName1 = 'transdetail_selection_type_id', 
@sRefTableName = 'TRANSDETAIL_SELECTION_TYPE'
GO

-- Add foreign key 
Exec DDLAddForeignKey 
@sTableName = 'TransDetail_Selection', 
@sColumnName1 = 'linked_to_transdetail_id', 
@sRefTableName = 'TRANSDETAIL',
@sRefColumnName1 = 'transdetail_id'  
GO

-- Add foreign key 
Exec DDLAddForeignKey 
@sTableName = 'TransDetail_Selection', 
@sColumnName1 = 'write_off_account_id', 
@sRefTableName = 'Account',
@sRefColumnName1 = 'account_id'  
GO

-- Add foreign key 
Exec DDLAddForeignKey
@sTableName = 'TransDetail_Selection', 
@sColumnName1 = 'write_off_reason_id', 
@sRefTableName = 'write_off_reason'
GO

EXEC DDLAddColumn 'Transdetail','commission_payment_batch_id', 'Int Default NULL'
GO

-- Add foreign key 
Exec DDLAddForeignKey	
@sTableName = 'Transdetail', 
@sColumnName1 = 'commission_payment_batch_id', 
@sRefTableName = 'Batch'
GO

-- Add Non-Clustered Index
--EXEC DDLAddIndex 'Transdetail','commission_payment_batch_id'
--GO
EXEC DDLDROPPRIMARYKEY 'TransDetail_Selection','transdetail_id','user_id'
GO
EXEC DDLADDCOLUMN 'Transdetail_selection', 'instalmentnumber','INT NULL'
GO
DDLAddColumn 'Released_Accounts_Transactions','pfinstalments_id','INT NULL'
GO
EXEC DDLAddColumn 'Party','is_grouped_Claim_settlement','Tinyint NULL'
GO

-- *****************************************************************************
-- * Author:   Saurabh Gupta
-- * Date:     1/08/2012
-- * Purpose:  Increase the length of the column (refer 3201).
-- *****************************************************************************

EXEC DDLALTERCOLUMN 'RI_Arrangement_line','TYPE','VARCHAR(3)'
GO


-- *****************************************************************************
-- * Author:   Saurabh Gupta
-- * Date:     9/08/2012
-- * Purpose:  Increase the length of the column (refer 3251).
-- *****************************************************************************

EXEC DDLALTERCOLUMN 'Claim_RI_Arrangement_Line','TYPE','VARCHAR(3)'
GO

-- *****************************************************************************
-- * Author:   Vidya Rangdale
-- * Date:     9/8/2012
-- * Purpose:  WPR 35 - Written Status
-- *****************************************************************************
EXEC DDLAddColumn 'Product', 'allow_written_status', 'tinyint NOT NULL Default 0',@BQuiet=1
GO
EXEC DDLAddColumn 'Product', 'written_task_manager_days', 'INT NULL' , @BQuiet=1
GO
EXEC DDLAddColumn 'Product', 'written_rem_user_group', 'INT NULL' , @BQuiet=1
GO
EXEC DDLAddColumn 'Product', 'written_rem_task_group', 'INT NULL' , @BQuiet=1

GO

IF NOT EXISTS (SELECT 1 FROM insurance_file_type WHERE code = 'WRITTEN')
BEGIN
	DECLARE @max_insurance_file_type_id int
	DECLARE @valuetouseinsurance_file_type int
	SELECT @max_insurance_file_type_id = ISNULL(max(insurance_file_type_id),1)
	FROM insurance_file_type
	SET @valuetouseinsurance_file_type = @max_insurance_file_type_id  + 1

	DECLARE @lCaptionID int
	EXEC spu_pm_caption_id_return 1, 'Written', @lCaptionID output
	INSERT INTO insurance_file_type(insurance_file_type_id,caption_id, code, description,var_data_structure_id,is_deleted, effective_date)
	VALUES (@valuetouseinsurance_file_type,@lCaptionID, 'WRITTEN', 'Written',null,0, getdate())
END

IF NOT EXISTS (SELECT 1 FROM process_type WHERE code = 'WNB')
BEGIN
	DECLARE @max_process_type_id int
	DECLARE @valuetouseprocess_type int
	SELECT @max_process_type_id = ISNULL(max(process_type_id),1)
	FROM process_type
	SET @valuetouseprocess_type = @max_process_type_id  + 1

	DECLARE @lCaptionID1 int
	EXEC spu_pm_caption_id_return 1, 'New Business Written', @lCaptionID1 output
	INSERT INTO process_type(process_type_id,caption_id, code, description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
	VALUES (@valuetouseprocess_type,@lCaptionID1, 'WNB', 'New Business Written',0, getdate(),0,1)
END

IF NOT EXISTS (SELECT 1 FROM process_type WHERE code = 'WRN')
BEGIN
	DECLARE @max_process_type_id1 int
	DECLARE @valuetouseprocess_type1 int
	SELECT @max_process_type_id1 = ISNULL(max(process_type_id),1)
	FROM process_type
	SET @valuetouseprocess_type1 = @max_process_type_id1  + 1

	DECLARE @lCaptionID2 int
	EXEC spu_pm_caption_id_return 1, 'Renewal Acceptance Written', @lCaptionID2 output
	INSERT INTO process_type(process_type_id,caption_id, code, description,is_deleted, effective_date,is_editable_after_merging,Functional_Area)
	VALUES (@valuetouseprocess_type1,@lCaptionID2, 'WRN', 'Renewal Acceptance Written',0, getdate(),0,1)
END

IF NOT EXISTS (SELECT 1 FROM renewal_status_type WHERE code = 'Written')
BEGIN
	DECLARE @max_renewal_status_type_id int
	DECLARE @valuetouserenewal_status_type int
	SELECT @max_renewal_status_type_id = ISNULL(max(renewal_status_type_id),1)
	FROM renewal_status_type
	SET @valuetouserenewal_status_type = @max_renewal_status_type_id  + 1
	DECLARE @lCaptionID3 int
	EXEC spu_pm_caption_id_return 1, 'Written - Awaiting Update', @lCaptionID3 output
	INSERT INTO renewal_status_type(renewal_status_type_id,caption_id, code, description,is_deleted, effective_date)
	VALUES (@valuetouserenewal_status_type,@lCaptionID3, 'Written', 'Written - Awaiting Update',0, getdate())
END
-- *****************************************************************************
-- * Author:   Ramesh
-- * Date:     18 JUL 2012
-- * Purpose:  WPR808_Renewal_Payment_Plan_Selection
-- *****************************************************************************
EXEC DDLAddColumn 'Product', 'use_prior_term_scheme_at_ren', 'TINYINT NOT NULL DEFAULT(0)'
GO

-- *****************************************************************************
-- * Author:   Ashish Sachdeva
-- * Date:     13/05/2010
-- * Purpose:  DRE Integration Section 6
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'risk_type_rule_set_type'
IF @bExists = 0 BEGIN
    CREATE TABLE risk_type_rule_set_type(
	[risk_type_rule_set_type_id] INT NOT NULL IDENTITY(1,1),
	[code] Varchar(10) NOT NULL,
	[description] Varchar(255) NOT NULL,
	[caption_id] Int NOT NULL,
	[effective_date] Datetime NOT NULL,
	[is_deleted] Tinyint NOT NULL Default 0	
	)
	
	EXEC DDLADDPrimaryKey 'risk_type_rule_set_type','risk_type_rule_set_type_id'
	EXEC DDLAddIndex 'risk_type_rule_set_type','code'
END
GO

DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = risk_type_rule_set_type_id , @sColumnDefinition = 'INT NULL', @bQuiet = 1
GO
DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = dre_executor_url , @sColumnDefinition = 'VARCHAR(255) NULL', @bQuiet = 1
GO
DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = dre_default_token , @sColumnDefinition = 'VARCHAR(255) NULL', @bQuiet = 1
GO

-- *****************************************************************************
-- * Author:   Ashish Sachdeva
-- * Date:     10/08/2010
-- * Purpose:  DRE Integration (Enhancement suggested by Steve & Rob)
-- *****************************************************************************
DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = dre_default , @sColumnDefinition = 'TINYINT NULL', @bQuiet = 1
GO
DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = dre_validation , @sColumnDefinition = 'TINYINT NULL', @bQuiet = 1
GO
DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = dre_quote , @sColumnDefinition = 'TINYINT NULL', @bQuiet = 1
GO
DDLADDColumn @sTableName = risk_type_rule_set, @sColumnName = post_dre_script , @sColumnDefinition = 'TINYINT NULL', @bQuiet = 1
GO
-- *****************************************************************************
-- * Author:   Maninder Kaur
-- * Date:     18 Feb 2013
-- * Purpose:  Increase column precision up to 10 decimal places.
-- *****************************************************************************

EXEC DDLAlterColumn 'Commission_Arrangement','rate','numeric(19, 10)',0
EXEC DDLAlterColumn 'Agent_Commission','commission_percentage','numeric(19, 10)',0
GO
EXECUTE DDLAddColumn 'PFScheme','is_plan_reference_editable','Tinyint NULL Default 1'
GO
-- *****************************************************************************
-- * Author:   Deepak Arora
-- * Date:     05/11/2012
-- * Purpose:  WORKITEM :4046
-- *****************************************************************************
IF EXISTS( SELECT 1 FROM PMProduct_Lookup WHERE lookup_table_name = 'Bank_Payment_Type' AND  is_generic_maintenance = 1 )
	Update PMProduct_Lookup SET is_generic_maintenance = 0 WHERE lookup_table_name = 'Bank_Payment_Type'
GO

-- *****************************************************************************
-- * PN56575
-- * Author: Vaibhav Singla
-- * Date: 18/12/2012
-- *****************************************************************************
EXEC DDLAddColumn 'PFMediaTypeHistory','is_cardholder','Tinyint Null'
GO
-- *****************************************************************************
-- * Author:   Deepak Arora
-- * Date:     14/01/2013
-- * Purpose:  WORKITEM :4818
-- *****************************************************************************
--EXEC DDLADDINDEX 'pfpremiumfinance','party_bank_id'
--GO
--EXEC DDLADDINDEX 'cashlistitem','party_bank_id'
--GO
--EXEC DDLADDINDEX 'claim_payment','party_bank_id'
--GO
--EXEC DDLADDINDEX 'TransDetail','PFInstalments_id'
--GO

-- *****************************************************************************
-- * Author:   Vidya Rangdale 
-- * Date:     06/03/2013
-- * Purpose:  Add is_chase_cycle_property to gis_property table
-- *****************************************************************************
EXEC DDLAddColumn 'gis_property', 'is_chase_cycle_property', 'int'
GO

	
-- *****************************************************************************
-- * Author:   Vidya Rangdale 
-- * Date:     06/03/2013
-- * Purpose:  Add new Chase_cycle_rule table
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Chase_cycle_rule'
IF @bExists = 0 BEGIN

CREATE TABLE Chase_cycle_rule(
	[chase_cycle_rule_id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NULL,
	[source_id] [int] NOT NULL,
	[Gis_data_model_id] [int] NOT NULL,
	[Gis_property_id] [int] NULL,
	[chase_cycle_status_udl_value_id] [int] NOT NULL,
	[is_active] [tinyint] NULL,
	[processing_days] [smallint] NULL,
	[use_effective_date] [tinyint] NULL,
	[use_greater_of_transaction_and_effective_date] [tinyint] NULL,
	[product_id] [int] NULL,
	[Include_cancelled_policies] [tinyint] NULL,
	[Cancelled_only] [tinyint] NULL,
 CONSTRAINT [PK_Chase_cycle_rule] PRIMARY KEY CLUSTERED 
(
	[chase_cycle_rule_id] ASC
)
) 

end
GO
EXEC DDLAddForeignKey @sTableName='Chase_cycle_rule',@sColumnName1='product_id',@sRefTableName='Product', @sRefColumnName1='Product_id' 
EXEC DDLAddForeignKey @sTableName='Chase_cycle_rule', @sColumnName1='source_id', @sRefTableName = 'Source',@sRefColumnName1='source_id'

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Chase_Cycle_Step'
IF @bExists = 0 BEGIN
CREATE TABLE	Chase_Cycle_Step (
	[chase_cycle_step_id] [int] IDENTITY(1,1) NOT NULL,
	[chase_cycle_rule_id] [int] NOT NULL,
	[step_number] [smallint] NULL,
	[number_of_days] [smallint] NULL,
	[document_template_id] [int] NULL,
	[pmwrk_task_id] [int] NULL,
	[pmuser_group_id] [int] NULL,
	[check_auto_cancel] [tinyint] NULL,
	[auto_cancel_policy] [tinyint] NULL,
	[next_step] [smallint] NULL,
	[previous_step] [smallint] NULL,
	[step_description] [varchar](255) NULL,
	[pmwrk_task_group_id] [int] NULL,
 CONSTRAINT [PK_Chase_Cycle_Step] PRIMARY KEY CLUSTERED 
(
	[chase_cycle_step_id] ASC
)
) 

end
GO
EXEC DDLAddForeignKey @sTableName='Chase_Cycle_Step', @sColumnName1='Document_Template_Id', @sRefTableName = 'Document_Template',@sRefColumnName1='document_template_id' 

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Chase_cycle_item'
IF @bExists = 0 
	BEGIN
		CREATE TABLE Chase_cycle_item(
			[chase_cycle_item_id] [int] IDENTITY(1,1) NOT NULL,
			[chase_cycle_reason] [varchar](50) NOT NULL,
			[insurance_folder_cnt] [int] NOT NULL,
			[Insurance_file_cnt] [int] NOT NULL,
			[can_auto_cancel] [tinyint] NULL,
			[will_auto_cancel] [tinyint] NULL,
			[chase_cycle_step_id] [int] NOT NULL,
			[created_date] [datetime] NOT NULL,
			[due_date] [datetime] NULL,
			[letter_sent] [tinyint] NULL,
			[pmuser_group_id] [int] NULL,
			[pmuser_id] [int] NULL,
			[is_deleted] [tinyint] NULL,
			[risk_cnt] [int] NULL,
			 CONSTRAINT [PK_Chase_cycle_item] PRIMARY KEY CLUSTERED 
				(
					[chase_cycle_item_id] ASC
				)
		) 
	end
Else
	BEGIN
		Declare @ColumnExists int
		EXEC @ColumnExists = DDLExistsColumn 'Chase_cycle_item','risk_cnt'
			If @ColumnExists = 0
				BEGIN
					EXEC DDLAddColumn  'Chase_cycle_item','risk_cnt','INT NULL'
				END
	END
GO
EXEC DDLAddForeignKey @sTableName='Chase_cycle_item',@sColumnName1= 'insurance_folder_cnt', @sRefTableName = 'insurance_folder', @sRefColumnName1='insurance_folder_cnt'
EXEC DDLAddForeignKey @sTableName='Chase_cycle_item', @sColumnName1='Insurance_file_cnt', @sRefTableName='Insurance_file', @sRefColumnName1='Insurance_file_cnt'
EXEC DDLAddForeignKey @sTableName='Chase_cycle_item', @sColumnName1='risk_cnt', @sRefTableName='risk', @sRefColumnName1='risk_cnt'
-- *****************************************************************************
-- * Author:   Vidya Rangdale 
-- * Date:    12/04/2013
-- * Purpose:  Add new  code  Transaction_Type table for Renewal lapse
-- *****************************************************************************
IF NOT EXISTS (SELECT code FROM Transaction_Type WHERE code = 'RENLAP')

begin

declare @Transaction_Type_id as int

select @Transaction_Type_id=MAX(Transaction_Type_id) from transaction_type
SET @Transaction_Type_id=@Transaction_Type_id+1

DECLARE @lCaptionID int

	EXEC spu_pm_caption_id_return 1, 'Lapse Renewal', @lCaptionID output
	
insert into  Transaction_Type (transaction_type_id,caption_id,code,description,transaction_type_basis,is_deleted,effective_date)
values (@Transaction_Type_id,@lCaptionID,'RENLAP','Lapse Renewal','A',0,GETDATE())

end
GO
-- *****************************************************************************
-- * Author:   Navneet Kharwanda
-- * Date:     16/04/2013
-- * Purpose:  Add archive_as_xml columns to Document_Template table
-- *****************************************************************************
EXEC DDLAddColumn 'Document_Template', 'archive_as_xml', 'bit NOT NULL DEFAULT(0)'
GO

-- *****************************************************************************  
-- * Author:       Gaurav Arora
-- * Date:         25-05-2013
-- * Purpose:      Removed MTA_reason_Code to Insurance_File table
-- *****************************************************************************
EXEC DDLDropColumn 'Insurance_File','MTA_reason_code'
GO

-- *****************************************************************************  
-- * Author:       Gaurav Arora
-- * Date:         25-05-2013
-- * Purpose:      Add MTA_reason_Id to Insurance_File table
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'Insurance_File', 'MTA_reason_id', 'SMALLINT NULL'
GO

-- *****************************************************************************
-- * Author:   Goldy Saini
-- * Date:     04 Mar 2014
-- * Purpose:  Add a column in Write_Off_Table and PF Instalments table to store Write_Off_Reason_Id (INS 07)
-- **************************************************************************************************

EXEC DDLAddColumn 'Write_Off_Reason', 'is_Only_Valid_for_Instalment', 'Tinyint NULL Default 0'
GO

EXEC DDLAddColumn 'PFInstalments', 'Write_Off_Reason_id', 'Integer NULL'
GO

UPDATE PFInstalments SET write_off_reason_id = NULL WHERE  write_off_reason_id = 0
GO

Exec DDLAddForeignKey @sTableName='PFInstalments', @sColumnName1='Write_Off_Reason_id',
	@sRefTableName='Write_Off_reason', @sRefColumnName1='write_off_reason_id'
	
GO

-- *****************************************************************************
-- * Author:   Goldy Saini
-- * Date:     10 Mar 2014
-- * Purpose:  Add a column in PF Instalments table
-- **************************************************************************************************

EXEC DDLAddColumn 'PFInstalments', 'write_off_transdetail_id', 'Integer NULL'
GO

Exec DDLAddForeignKey @sTableName='PFInstalments', @sColumnName1='write_off_transdetail_id',
	@sRefTableName='Transdetail', @sRefColumnName1='Transdetail_id'
	
GO
-- *****************************************************************************  
-- * Author:       Tariq Rashid
-- * Date:         28-04-2014
-- * Purpose:      Add Three new Fields to Product Table
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'Product', 'is_reserves_read_only', 'TINYINT NULL'
GO
EXEC DDLAddOrAlterColumn 'Product', 'is_recoveries_read_only', 'TINYINT NULL'
GO
EXEC DDLAddOrAlterColumn 'Product', 'is_payments_read_only', 'TINYINT NULL'
GO

-- ****************************************************************************
-- Author: Vijay Pal
-- Date: 05/05/2014
-- Sub Agent Commission Spread QBECALINS16-18 - Instalment and Credit Control Merge
-- *****************************************************************************
EXEC DDLAddColumn 'PFScheme','spread_subagent_commission','Tinyint NULL'
GO
EXEC DDLAddColumn 'PFScheme','commission_subagent_suspense_account_id','Int NULL'
GO
EXEC DDLAddForeignKey 'PFScheme', 'commission_subagent_suspense_account_id', @sRefTableName = 'account', @sRefColumnName1='account_id'
GO
-- *****************************************************************************
-- Author:   Vijay Pal
-- Date:     05/05/2014
-- Purpose:  Add jump_to_next_step_broker to credit_control_step table-QBECALINS16-18
-- *****************************************************************************
EXEC DDLAddColumn 'credit_control_step', 'jump_to_next_step_broker', 'tinyint'
Go
EXEC DDLAddColumn 'credit_control_step', 'single_instalment_jump_to_next_step_broker', 'tinyint'
Go
EXEC DDLAddColumn 'credit_control_step', 'single_instalment_account_number_of_days', 'smallint'
Go
EXEC DDLAddColumn 'credit_control_step', 'single_instalment_account_tollerance_amount', 'Numeric(19,4)'
Go
EXEC DDLAddColumn 'credit_control_step', 'single_instalment_broker_letter_id', 'int'
Go
-- *****************************************************************************
EXEC DDLAddColumn 'party_bank','Bank_Name','VARCHAR(100)'
GO
EXEC DDLAddColumn 'cashlistitem','cashlistitem_Bank','VARCHAR(100)'
GO
EXEC DDLAddColumn 'Party_Bank_History','Bank_Name','VARCHAR(100)'
GO
EXEC DDLAddIndex 'Transdetail', 'spare'
GO    
EXEC DDLAddColumn 'PFMediaTypeHistory','is_cardholder','Tinyint Null'
GO

-- *****************************************************************************  
-- * Author:       Rachana Srivastava
-- * Date:         18-04-2014
-- * Purpose:      Add 
-- *****************************************************************************
EXEC DDLAddColumn  'Address', 'address5', 'varchar(60) NULL' -- Property Description
GO

EXEC DDLAddColumn  'Address', 'address6', 'varchar(60) NULL' -- GNAFID
GO

EXEC DDLAddColumn  'Address', 'address7', 'varchar(60) NULL' -- dpid
GO

EXEC DDLAddColumn  'Address', 'address8', 'varchar(60) NULL' -- dpid barcode
GO

EXEC DDLAddColumn  'Address', 'address9', 'varchar(60) NULL' -- latitiude
GO

EXEC DDLAddColumn  'Address', 'address10', 'varchar(60) NULL' -- longitude
GO

-- *****************************************************************************  
-- * Author:       Sahil Ansari
-- * Date:         22-05-2014
-- * Purpose:      Parallel from 1.13.6
-- *****************************************************************************
EXECUTE DDLAddColumn 'Credit_Control_Rule','use_inception_date','tinyint NULL'
Go
-- *****************************************************************************
-- * Author:        Samarjeet Singh
-- * Date:          26 Apr 2014
-- * Purpose:
-- *                Add table to PMLookup
-- *****************************************************************************
If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Association_Type' AND xtype = 'U')
BEGIN
    CREATE TABLE Association_Type
    (
        Association_Type_id 		INT 		IDENTITY,
        caption_id 			INT 		NOT NULL,
        is_deleted 			TINYINT 	NOT NULL,
        effective_date 			DATETIME 	NOT NULL,
        description 			VARCHAR(255) 	NOT NULL,
        code 				VARCHAR(10)	NOT NULL
    CONSTRAINT PK__Association_Type_Association_Type_id  PRIMARY KEY CLUSTERED (Association_Type_id)  
    )
End
GO

-- *****************************************************************************
-- * Author:       SamarJeet Singh
-- * Date:         24 Apr 2008
-- * Purpose:  Add Tbale to store for policy associates
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Insurance_file_associates'
IF @bExists = 0
BEGIN
    CREATE TABLE Insurance_file_associates

	(
		Insurance_file_associates_cnt	INT IDENTITY,
                Insurance_file_cnt              INT NOT NULL,
		Party_cnt		        INT NOT NULL,
		Association_type_id	        INT NOT NULL,
		Association_detail		VARCHAR(250),
		date_attached		        DATE,
                date_removed		        DATE,
                Is_Deleted                      Tinyint
		CONSTRAINT PK__Insurance_file_associates_cnt PRIMARY KEY CLUSTERED (Insurance_file_associates_cnt)
		
	)
END
Go
EXEC DDLAddForeignKey @sTableName='Insurance_file_associates',@sColumnName1='Insurance_file_cnt',@sRefTableName='Insurance_file', @sRefColumnName1='Insurance_file_cnt' 
EXEC DDLAddForeignKey @sTableName='Insurance_file_associates', @sColumnName1='Party_cnt', @sRefTableName = 'Party',@sRefColumnName1='Party_cnt'
EXEC DDLAddForeignKey @sTableName='Insurance_file_associates', @sColumnName1='Association_type_id', @sRefTableName = 'Association_Type',@sRefColumnName1='Association_Type_id'
Go
EXEC DDLAddIndex 'Insurance_file_associates','Party_cnt'
GO

-- *****************************************************************************
-- * Author:       TARIQ RASHID
-- * Date:         18/06/2014
-- * Purpose:  Temporary Data Holder between SPS
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'tblTaxBandInfo'
IF @bExists = 0
BEGIN
	CREATE TABLE tblTaxBandInfo
	( ReserveID INT,
	TaxBandID INT,
	Rate FLOAT,
	IsValue TINYINT,
	ClassOfBusinessID INT,
	TaxAmount CURRENCY)  
END
GO


-- *****************************************************************************
-- * Author:       MANSI
-- * Date:         30/06/2014
-- * Purpose:      Change datatype of BankAccount table from smallint to int
--paralleling from etana	
-- *****************************************************************************

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
       ID = (SELECT id FROM sysobjects WHERE name = 'bank') AND name='bank_id')=0
BEGIN
            EXEC DDLDropForeignKey 'bank','head_office'
            EXEC DDLDropForeignKey 'bank','bank_country'
            EXEC DDLDropForeignKey 'Bank_Guarantee','bank_name_id'
            EXEC DDLDropForeignKey 'BankAccount','bank_id'
            EXEC DDLDropPrimaryKey   'bank', 'bank_id'
            EXEC DDLAlterColumn 'Bank', 'bank_id', 'int'
            EXEC DDLAddIdentityColumn 'bank', 'bank_id'
            EXEC DDLADDPrimaryKey 'bank','bank_id'
            EXEC DDLAddIndex 'bank','bank_country'
            EXEC DDLAddIndex 'bank','code'
            EXEC DDLDropIndex 'BankAccount', 'Bank_id'
            EXEC DDLAlterColumn 'BankAccount', 'Bank_id', 'int'
            EXEC DDLAddIndex 'BankAccount', 'bank_id'
            EXEC DDLAlterColumn 'Bank_Guarantee', 'Bank_name_id', 'int'
            EXEC DDLDropIndex 'Bank', 'head_office'
            EXEC DDLAlterColumn 'Bank', 'head_office', 'int'
            EXEC DDLAddIndex 'Bank', 'head_office'
            EXEC DDLAddForeignKey @sTableName='BankAccount', @sColumnName1='bank_id', @sRefTableName = 'bank',@sRefColumnName1='bank_id'
            EXEC DDLAddForeignKey @sTableName='Bank_Guarantee', @sColumnName1='bank_name_id', @sRefTableName = 'bank',@sRefColumnName1='bank_id'
            EXEC DDLAddForeignKey @sTableName='bank', @sColumnName1='head_office', @sRefTableName = 'bank',@sRefColumnName1='bank_id'
            EXEC DDLAddForeignKey @sTableName='bank', @sColumnName1='bank_country', @sRefTableName = 'country',@sRefColumnName1='country_id'
END
GO

-- *****************************************************************************  
-- * Author:      Vidya Rangdale
-- * Date:         June 2014
-- * Purpose:     Password Access to Pure Insurance Back-Office and Web Portals
-- *****************************************************************************
EXEC DDLAddColumn 'PMUser', 'is_temp_password', 'BIT NOT NULL DEFAULT(0)'
GO
EXEC DDLAddColumn 'PMUser', 'incorrect_attempt_count', 'INT NOT NULL DEFAULT 0'
GO
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'PMUser_Password_History'
IF @bExists = 0 BEGIN
	CREATE TABLE PMUser_Password_History
	(
	user_id smallint FOREIGN KEY References pmuser(user_id),
	historic_password varchar(255) ,
	date_added Datetime
	)

END

GO
EXEC DDLAddColumn @sTableName='pmuser', @sColumnName ='secure_password',@sColumnDefinition='varchar(255) NULL'
Go
EXEC DDLAlterColumn @sTableName='pmuser', @sColumnName ='password',@sColumnDefinition='varchar(255) NULL'
GO
-- *****************************************************************************
-- * Author:      SAUMITRA BHATNAGAR
-- * Date:        13/07/2014
-- * Purpose:     Holds the UnConfirmed Status of the Added Policy Associate for the current policy version
-- *****************************************************************************
EXEC DDLAddColumn 'Insurance_file_associates', 'is_AddUnConfirmed', 'TINYINT NULL'
GO

-- *****************************************************************************
-- * Author:      SAUMITRA BHATNAGAR
-- * Date:        13/07/2014
-- * Purpose:     Holds the UnConfirmed Status of the Deleted Policy Associate for the current policy version
-- *****************************************************************************
EXEC DDLAddColumn 'Insurance_file_associates', 'is_DelUnConfirmed', 'TINYINT NULL'
GO

-- *****************************************************************************
-- * Author:      Sahil Ansari
-- * Date:        13/07/2014
-- * Purpose:     same size as that of ClientCode in Party table
-- *****************************************************************************
EXEC DDLAlterColumn @sTableName='PFPremiumFinance', @sColumnName ='ClientCode',@sColumnDefinition='varchar(20) NULL'
GO
-- *****************************************************************************  
-- * Author:       Samarjeet Singh
-- * Date:         09-07-2014
-- * Purpose:      NEW Table for External_Workflow_Config 
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'External_Workflow_Config')
BEGIN
 CREATE TABLE External_Workflow_Config
  (
  External_Workflow_Config_id INT  IDENTITY NOT NULL,
  Schedule_backGroundjob_ForFailure             TINYINT
  CONSTRAINT PK__External_Workflow_Config PRIMARY KEY (External_Workflow_Config_id)
  )
End
GO

-- *****************************************************************************  
-- * Author:       Samarjeet Singh
-- * Date:         10-04-2014
-- * Purpose:      NEW Table for holding the  Usergroup Configure in ExterWorkflowConfigurationTask
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'External_WorkFlow_usergroups')
BEGIN
 CREATE TABLE External_WorkFlow_usergroups
  (
  external_Workflow_usergroups_id INT  Identity NOT NULL,
  usergroup_id             INT
  CONSTRAINT PK__External_WorkFlow_usergroups PRIMARY KEY (external_Workflow_usergroups_id)
  )
EXEC DDLAddForeignKey @sTableName='External_WorkFlow_usergroups', @sColumnName1='usergroup_id', @sRefTableName = 'PMUser_Group',@sRefColumnName1='PMUser_Group_id'
End
GO

-- *****************************************************************************  
-- * Author:       Samarjeet Singh
-- * Date:         10-04-2014
-- * Purpose:      to implement External Work Flow Task for E5
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='PMWrk_task_parent_instance_cnt',@sColumnDefinition='Int'
GO
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='external_Workflow_id',@sColumnDefinition='uniqueidentifier'
Go
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO
EXEC DDLAddForeignKey @sTableName='PmWrk_task_Instance',@sColumnName1='PMWrk_task_parent_instance_cnt',@sRefTableName='PmWrk_task_Instance', @sRefColumnName1='pmwrk_task_instance_cnt' 
GO
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='ExternalTask_Category_Id',@sColumnDefinition='Int'
GO

-- *****************************************************************************
-- * Author:      Sahil Ansari
-- * Date:        29/07/2014
-- * Purpose:     WPR13 /E5
-- *****************************************************************************

EXEC DDLAddColumn @sTableName='PMNav_Key', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO


EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='Is_system_lock',@sColumnDefinition='TinyInt'
GO


EXEC DDLAddColumn @sTableName='pmlock_last_unlock', @sColumnName ='Is_system_lock',@sColumnDefinition='TinyInt'
GO

-- *****************************************************************************  
-- * Author:       Samarjeet Singh
-- * Date:         11-10-2014
-- * Purpose:      To Add new Configuration in User maintenance Against Instalment status locking(PN72529)
-- *****************************************************************************
EXECUTE DDLAddColumn 'User_Authorities','can_update_instalment_status','Tinyint NOT NULL Default 0'
GO

-- *****************************************************************************
-- * Author:      Sahil Ansari
-- * Date:        26/09/2014
-- * Purpose:     WPR13 /E5
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='Background_Job', @sColumnName ='last_job_retry_time',@sColumnDefinition='datetime'
GO


-- *****************************************************************************
-- * Author:      Sahil Ansari
-- * Date:        06/11/2014
-- * Purpose:     RACTI JIRA 138, Claim Locking
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='lock3_value',@sColumnDefinition='varchar(250)'
GO
EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='is_exclusive_lock',@sColumnDefinition='Int NOT NULL Default 0'
GO



IF NOT EXISTS( SELECT 1 FROM SYS.Types WHERE NAME LIKE 'TPMLock')
BEGIN
CREATE TYPE TPMLock AS TABLE
(
	[lock_name] [char](30) NOT NULL,
	[lock_value] [int] NOT NULL
)
END
GO
-- *****************************************************************************
-- * Author:      Tariq Rashid
-- * Date:        30/03/2015
-- * Purpose:     Removing tax_band_rate_id
EXEC DDLAddIndex 'Insurance_File_associates','insurance_file_cnt'
GO
EXEC DDLAddIndex 'peril','risk_cnt'
GO
-- *****************************************************************************
-- * Author:      Samarjeet Singh
-- * Date:        27/04/2015
-- * Purpose:     Added Column to filterout the Mediatype for Claim payment
EXEC DDLAddColumn 'MediaType', 'is_Claim_Payment', 'TINYINT'
Go
EXEC DDLAddColumn 'MediaType', 'is_Claim_Recovery', 'TINYINT'
GO

-- *****************************************************************************  
-- * Author:      TARIQ RASHID
-- * Date:        14 AUG 2015
-- * Purpose:     RACTI JIRA SSP-2105, SSP-2099
-- *****************************************************************************
ALTER TABLE Credit_Control_Item SET (LOCK_ESCALATION=DISABLE)
GO

-- *****************************************************************************  
-- * Author:     Navneet Kharwanda
-- * Date:        09 Aug 2016
-- * Purpose:     RACTI JIRA SSP-2573
-- *****************************************************************************
EXECUTE DDLAddColumn 'User_Authorities','can_edit_instalment_date','Tinyint NOT NULL Default 0'
EXECUTE DDLAddColumn 'User_Authorities','edit_instalment_by_no_of_days','INT'

-- * Author:     Shivraj Rathor
-- * Date:        01 Dec 2016
-- * Purpose:     RACTI JIRA SSP-2133
ALTER TABLE PFPremiumFinance ALTER COLUMN BankAccountName VARCHAR(50)
Go
ALTER TABLE PFPremiumFinance ALTER COLUMN BankAccountNo VARCHAR(50)
Go
ALTER TABLE PFPremiumFinance ALTER COLUMN BankSortCode VARCHAR(50)
Go
ALTER TABLE PFPremiumFinance ALTER COLUMN BankBranch VARCHAR(50)
Go
ALTER TABLE PFMediaTypeHistory ALTER COLUMN BankAccountName VARCHAR(50)
Go
ALTER TABLE PFMediaTypeHistory ALTER COLUMN BankAccountNo VARCHAR(50)
Go
ALTER TABLE PFMediaTypeHistory ALTER COLUMN BankSortCode VARCHAR(50)
Go
ALTER TABLE PFMediaTypeHistory ALTER COLUMN BankBranch VARCHAR(50)
Go

-- *****************************************************************************
-- * Author:       Priyanka Sehgal
-- * Date:         27 Sep 2012
-- * Purpose:      WPR43- Product_source
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Product_source'
IF @bExists = 0 BEGIN
   CREATE TABLE Product_source
(
product_id INT FOREIGN KEY REFERENCES product(product_id),
source_id INT FOREIGN KEY REFERENCES source(source_id)
)

INSERT INTO Product_source (product_id,source_id)
SELECT product_id,source_id from Product,source

END
GO

EXEC DDLAddColumn 'Product','bind_renewal_without_invitation','tinyint NULL'
GO
update	 product set bind_renewal_without_invitation=0 where bind_renewal_without_invitation is NULL


-- *****************************************************************************
-- * Author:   Goldy Saini
-- * Date:     30/04/2014
-- * Purpose:  Coinsurance Placement
-- *****************************************************************************
	Declare @ColumnExists int
    Declare @TableExists int
    Declare @Table varchar(250)
	SET @Table = 'insurance_file'
	EXEC @ColumnExists = DDLExistsColumn @table,'coins_placement'
	EXEC @TableExists = DDLExistsTable @table
	
	If @ColumnExists = 0 AND @TableExists = 1
	BEGIN
		EXEC DDLADDCOLUMN 'insurance_file','coins_placement','VARCHAR(10) NULL'
	END
GO



-- *****************************************************************************
-- * Author:   Goldy Saini
-- * Date:     30/04/2014
-- * Purpose:  Coinsurance Placement
-- *****************************************************************************
	Declare @ColumnExists int
    Declare @TableExists int
    Declare @Table varchar(250)
	SET @Table = 'insurance_file'
	EXEC @ColumnExists = DDLExistsColumn @table,'coins_placement'
	EXEC @TableExists = DDLExistsTable @table
	
	If @ColumnExists = 0 AND @TableExists = 1
	BEGIN
		EXEC DDLADDCOLUMN 'insurance_file','coins_placement','VARCHAR(10) NULL'
	END
GO
--PM027828--
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[ReleaseManualTransProcess]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		--/****** Object:  Table [ReleaseManualTransProcess]    Script Date: 22/03/2012 ******/
		CREATE TABLE [ReleaseManualTransProcess] (
			[ProcessRunning] int NOT NULL ,
			[StartTime] DateTime NULL 
		) 
		--Insert default value
		INSERT INTO [ReleaseManualTransProcess]([ProcessRunning], [StartTime])
		VALUES (0,GETDATE())
	END
-- *****************************************************************************
-- * Author:   Rahul Jaiswal
-- * Date:     28/03/2012
-- * Purpose:  WPR29 - New Field for PartyKey
-- *****************************************************************************

EXEC DDLADDCOLUMN 'pmwrk_task_instance','party_cnt','INT Foreign Key references Party(Party_cnt)'

-- *****************************************************************************
-- * Author:   Ashwani Kumar
-- * Date:     24/09/2013
-- * Purpose:  Increased the size of conviction_date column in party_conviction table 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'party_conviction', 'conviction_date', 'varchar(40) NOT NULL'
GO
-- *****************************************************************************
-- * Author:   Abhishek
-- * Date:     01/08/2019
-- * Purpose:  Increased the size of sentence_effective_date column in party_conviction table 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'party_conviction', 'sentence_effective_date', 'varchar(40)'
GO
EXEC DDLAddColumn 'Product', 'is_enable_PrePayment', 'tinyint NULL'
GO
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     25/07/2013
-- * Purpose:  WPR53
-- *****************************************************************************

EXEC DDLAddColumn 'Product', 'Mandatory_Risk_Type_Id', 'INT NULL'
GO
EXEC DDLAddColumn 'Risk', 'Is_Mandatory_Risk', 'TINYINT NULL'
GO
-- Script to add column to GIS Output table
	DECLARE @code VARCHAR(20)
	DECLARE GIS_OUTPUT_Cursor CURSOR FAST_FORWARD FOR  
	SELECT RTRIM(code) FROM GIS_Data_Model
	WHERE gis_data_model_type_id = 1 -- ie Risk

	OPEN GIS_OUTPUT_Cursor  
	FETCH NEXT FROM GIS_OUTPUT_Cursor INTO @code    
	-- Start processing  
	WHILE (@@FETCH_STATUS = 0)  
	BEGIN
	SELECT @code = @code + '_OUTPUT'
	IF EXISTS(SELECT * FROM SYSOBJECTS where name = @code)
	BEGIN	
	EXEC DDLAddColumn @code, 'Disable_Original_ProRata', 'TINYINT NULL'				
	EXEC DDLAddColumn @code, 'Disable_New_ProRata', 'TINYINT NULL'				
	END		
	-- Get Next Record
	FETCH NEXT FROM GIS_OUTPUT_Cursor INTO @code  
	END

	-- Close the cursor
	Close GIS_OUTPUT_Cursor  
	Deallocate GIS_OUTPUT_Cursor
GO
	EXEC DDLAlterColumn 'Rating_Section', 'Annual_Rate','NUMERIC(21, 6)'
	EXEC DDLAlterColumn 'Peril', 'Annual_Rate','NUMERIC(21, 6)'
GO
	--TARIQ
	UPDATE GIS_Property
	SET data_type = 1 
	WHERE gis_property_id in (
	SELECT DISTINCT GISP.gis_property_id FROM GIS_Property GISP
	JOIN GIS_Object GISO ON	GISP.gis_object_id = GISO.gis_object_id
	JOIN GIS_Data_Model GDM ON GISO.gis_data_model_id = GDM.gis_data_model_id
	JOIN sys.objects SO ON SO.name = GISO.table_name
	JOIN sys.columns SC ON SC.name = GISP.column_name AND SO.object_id = SC.object_id
	WHERE SC.system_type_id = 61 AND GISP.data_type <> 1)


	GO
	-- Script to change Percentage type to 6DP for all GIS tables
	IF EXISTS(Select NULL From gis_property gp (NOLOCK)
				Inner Join gis_object(NOLOCK) gob ON gob.gis_object_id = gp.gis_object_id
		Where data_type = 22 and 15<> (SELECT ISNULL(NUMERIC_PRECISION,0) + ISNULL(NUMERIC_SCALE,0)  
FROM INFORMATION_SCHEMA.COLUMNS
WHERE 
     TABLE_NAME = gob.table_name  AND 
     COLUMN_NAME = gp.column_name ))
	 BEGIN

	DECLARE @table_name VARCHAR(255)
	DECLARE @column_name VARCHAR(255)
	DECLARE @NUMERICPREC INT
	DECLARE GIS_Tables_Cursor CURSOR FAST_FORWARD FOR  
		Select go1.table_name, gp.column_name 
			From gis_property gp
				Inner Join gis_object go1 ON go1.gis_object_id = gp.gis_object_id
		Where data_type = 22
		Order By go1.table_name 


	OPEN GIS_Tables_Cursor  
	FETCH NEXT FROM GIS_Tables_Cursor INTO @table_name, @column_name    
	-- Start processing  
	WHILE (@@FETCH_STATUS = 0)  
	BEGIN
		SELECT @NUMERICPREC=NUMERIC_PRECISION
		FROM INFORMATION_SCHEMA.COLUMNS  where TABLE_NAME= @table_name  AND COLUMN_NAME= @column_name
		IF @NUMERICPREC <= 9 
		BEGIN
			EXEC DDLAlterColumn @table_name, @column_name,'NUMERIC(19, 6)'
		END

		-- Get Next Record
		FETCH NEXT FROM GIS_Tables_Cursor INTO @table_name, @column_name     
	END

	-- Close the cursor
	Close GIS_Tables_Cursor  
	Deallocate GIS_Tables_Cursor
END
GO

	-- Script to change DP of rate column in output tables
	DECLARE @code VARCHAR(255)
	DECLARE GIS_OUTPUT_Cursor CURSOR FAST_FORWARD FOR  
	SELECT RTRIM(code) + '_OUTPUT' FROM GIS_Data_Model WHERE gis_data_model_type_id = 1 -- ie Risk

	OPEN GIS_OUTPUT_Cursor  
	FETCH NEXT FROM GIS_OUTPUT_Cursor INTO @code    
	-- Start processing  
	WHILE (@@FETCH_STATUS = 0)  
	BEGIN
	IF EXISTS(SELECT * FROM SYSOBJECTS where name = @code)
	    BEGIN	
		EXEC DDLAlterColumn @code, 'Rate','NUMERIC(21, 6)'
	    END		
	-- Get Next Record
	FETCH NEXT FROM GIS_OUTPUT_Cursor INTO @code  
	END

	-- Close the cursor
	Close GIS_OUTPUT_Cursor  
	Deallocate GIS_OUTPUT_Cursor
GO
	
-- *****************************************************************************
-- * Author:   Shubhankar Singh
-- * Date:     17 Aug 2010
-- * Purpose:  Parallel of Development E005 Part1.
-- *****************************************************************************
EXECUTE DDLAddColumn 'RI_Model_Line','cede_premium_only','tinyint NOT NULL Default 0'
GO

IF (OBJECT_ID('RIBrokerParticipantOnTreaty') IS NULL)
BEGIN
	CREATE TABLE RIBrokerParticipantOnTreaty
        (
		participantontreaty_id int identity ,
		treaty_id int ,
		treaty_party_id int ,
		associated_party_cnt int ,
		party_cnt int ,
		participant_percent numeric(11,6) 
	)
     ALTER TABLE RIBrokerParticipantOnTreaty ADD
          CONSTRAINT FK__RIBrokerParticipantOnTreaty__treaty_id FOREIGN KEY (treaty_id)
          REFERENCES treaty (treaty_id)
END 
GO

-- *****************************************************************************
-- * Author:   Gaurav Arora	
-- * Date:     10/10/2010
-- * Purpose:  PN 75218
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'ri_arrangement_line', 'type', 'varchar(3)'
GO

-- *****************************************************************************
-- * Author:   Santosh Payasi	
-- * Date:     12/10/2010
-- * Purpose:  PN 75405
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'Claim_ri_Arrangement_Line', 'type', 'varchar(3)'
GO

-- *****************************************************************************
-- * Author:   Bushra Khatoon	
-- * Date:     28/01/2011
-- * Purpose:  WPR55
-- *****************************************************************************

EXEC DDLAddColumn 'RI_Arrangement', 'Extended_limit_amount', 'MONEY NULL'
EXEC DDLAddColumn 'RI_Arrangement', 'Is_extended_limit_applied', 'TINYINT NULL'

GO 

-- *****************************************************************************
-- * Author:   Gurucharan Gulati	
-- * Date:     28/03/2011
-- * Purpose:  E007
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Insurance_File_Cloned_RI_Usage' AND xtype = 'U')
BEGIN
    CREATE TABLE Insurance_File_Cloned_RI_Usage
    (
		ins_file_cloned_RI_usage_id		INT 		PRIMARY KEY IDENTITY,
		insurance_file_cnt 			INT 		NOT NULL CONSTRAINT FK__Insurance_File__insurance_file_cnt FOREIGN KEY References Insurance_File(insurance_file_cnt),
		[status] 				INT 		NOT NULL --CHECK ([status] > 0 and [status] < 3)
    )
END
GO

IF NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Claim_Cloned_RI_Usage' AND xtype = 'U')
BEGIN
    CREATE TABLE Claim_Cloned_RI_Usage
    (
		claim_cloned_RI_usage_id		INT 		PRIMARY KEY IDENTITY,
		Old_insurance_file_cnt 			INT 		NOT NULL FOREIGN KEY References Insurance_File(insurance_file_cnt),
		New_insurance_file_cnt			INT		NOT NULL FOREIGN KEY References Insurance_File(insurance_file_cnt),
		Old_Risk_Cnt				INT		NOT NULL FOREIGN KEY References Risk(risk_cnt),	
		New_Risk_Cnt				INT		NOT NULL FOREIGN KEY References Risk(risk_cnt),	
		[status] 				INT 		NOT NULL --CHECK ([status] > 0 and [status] < 3)
    )
END
GO

EXEC DDLAddColumn 'RI_Arrangement','Cloned','TINYINT DEFAULT 0 NOT NULL'
GO

EXEC DDLAddColumn 'Claim_RI_Arrangement','Cloned','TINYINT DEFAULT 0 NOT NULL'
GO


-- *****************************************************************************
-- * Author:        Kuljeet Kaur
-- * Date:          06/04/2011
-- * Purpose:       E007
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Proportional_RI_Calculation_Method'
IF @bExists = 0 BEGIN
	CREATE TABLE Proportional_RI_Calculation_Method
	(
	Proportional_RI_Calculation_Method_id int Primary Key,
	caption_id int,
	code char(10),
	description  varchar(255),
	is_deleted tinyint,
	effective_date datetime
	)

DECLARE @lCaptionID INT
EXECUTE spu_pm_caption_id_return 1, 'Underwriting Year', @lCaptionID OUTPUT

INSERT INTO Proportional_RI_Calculation_Method(Proportional_RI_Calculation_Method_id,caption_id,code,description,is_deleted,effective_date) 
VALUES(1,@lCaptionID,'UNDERWRYR','Underwriting Year',0,'2011-01-01')

INSERT INTO Proportional_RI_Calculation_Method(Proportional_RI_Calculation_Method_id,caption_id,code,description,is_deleted,effective_date) 
VALUES(2,@lCaptionID,'ACCOUNTYR','Accounting Year',0,'2011-01-01')

END
GO

-- *****************************************************************************
-- * Author:        Kuljeet Kaur
-- * Date:          06/04/2011
-- * Purpose:       E007
-- *****************************************************************************

EXEC DDLAddColumn 'RI_Band', 'Proportional_RI_Cal_Method', 'INT DEFAULT 2'
GO
EXEC DDLAddForeignKey 'RI_Band', 'Proportional_RI_Cal_Method', @sRefTableName = 'Proportional_RI_Calculation_Method'
GO

-- *****************************************************************************
-- * Author:        Kuljeet Kaur
-- * Date:          06/04/2011
-- * Purpose:       E007
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Insurance_File_PT_RI_Usage'
IF @bExists = 0 BEGIN
	CREATE TABLE Insurance_File_PT_RI_Usage
	(
	ins_file_PT_RI_usage_id int Primary Key,
	insurance_file_cnt int,
	status int,
	TransferDate datetime
	)
END
GO

EXEC DDLAddForeignKey 'Insurance_File_PT_RI_Usage', 'insurance_file_cnt', @sRefTableName = 'insurance_file'
GO

-- *****************************************************************************
-- * Author:        Kuljeet Kaur
-- * Date:          06/04/2011
-- * Purpose:       E007
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'PT_RI_status_type'
IF @bExists = 0 BEGIN
	CREATE TABLE PT_RI_status_type
	(
	PT_RI_status_type_id int Primary Key,
	caption_id int,
	code char(10),
	description  varchar(255),
	is_deleted tinyint,
	effective_date datetime
	)

INSERT INTO PT_RI_status_type(PT_RI_status_type_id,caption_id,code,description,is_deleted,effective_date) 
VALUES(1,563,'MANREVIEW ','Awaiting Manual Review',0,'2011-01-01')

INSERT INTO PT_RI_status_type(PT_RI_status_type_id,caption_id,code,description,is_deleted,effective_date) 
VALUES(2,567,'UPDATE','Awaiting Update',0,'2011-01-01')

END
GO

-- *****************************************************************************
-- * Author:       Nishchal Upadhayay
-- * Date:         18-02-2014
-- * Purpose:      Add Missing Codes to ACTNUMBER_RANGE
-- *****************************************************************************
-- * Author:        Rahul Jaiswal
-- * Date:          10/05/2011
-- * Purpose:       E007
-- *****************************************************************************


ddladdcolumn 'ri_arrangement','xol_ri_model_id','int FOREIGN KEY REFERENCES ri_model(ri_model_id)'
GO

Update ri_arrangement SET xol_ri_model_id = ri_model_id WHERE xol_ri_model_id is NULL
GO

-- *****************************************************************************
-- * Author:        Kuljeet Kaur
-- * Date:          24/05/2011
-- * Purpose:       E007
-- *****************************************************************************

IF(SELECT count(*) FROM syscolumns WHERE status = 128 AND 
	id = (SELECT id FROM sysobjects WHERE name = 'Insurance_File_PT_RI_Usage') AND name='ins_file_PT_RI_usage_id')=0
BEGIN
    EXEC DDLAddIdentityColumn 'Insurance_File_PT_RI_Usage','ins_file_PT_RI_usage_id'
END
GO

-- *****************************************************************************
-- * Author:        Rahul Jaiswal
-- * Date:          29/05/2011
-- * Purpose:       PN 78444
-- *****************************************************************************

DDLADDCOLUMN 'Insurance_File_Cloned_RI_Usage','new_insurance_file_cnt','INT'
GO


-- *****************************************************************************
-- * Author:        Rahul Jaiswal
-- * Date:          29/05/2011
-- * Purpose:       PN 78422
-- *****************************************************************************
DDLADDCOLUMN 'claim_ri_arrangement','xol_ri_model_id','INT'
GO
UPDATE claim_ri_arrangement SET xol_ri_model_id=ri_model_id WHERE xol_ri_model_id IS NULL
GO
EXEC DDLAddOrAlterColumn 'TransDetail', 'spare', 'varchar(100)'
GO

-- *****************************************************************************
-- * Author:        Rahul Jaiswal
-- * Date:          07/06/2011
-- * Purpose:       PN 78531
-- *****************************************************************************

DDLADDCOLUMN 'Insurance_File_PT_RI_Usage','new_insurance_file_cnt','INT'
GO
-- *****************************************************************************
-- * Author:   Shubhankar Singh
-- * Date:     07-July-2010
-- * Purpose:  Development E016.
-- *****************************************************************************
EXEC DDLAddColumn 'treaty_party','is_Reinsurer_Approved','tinyint'
GO

-- *****************************************************************************  
-- * Author:      Manish Arora
-- * Date:        05 August 2013
-- * Purpose:     Policy Fees default to Incorrect setting - Issue paralleled PM026653
-- *****************************************************************************
EXEC DDLAddColumn 'policy_fee_u', 'FeeTypePercent', 'bit NULL'
GO


-- *****************************************************************************  
-- * Author:      Rahul Jaiswal
-- * Date:        03 Oct 2013
-- * Purpose:     IFRS - Survival Risk
-- *****************************************************************************

DDLADDCOLUMN 'risk_type','Claims_Cover_basis_ID', 'INT'
GO

DDLADDCOLUMN 'risk_type','Claims_type_basis_ID', 'INT'
GO

DDLADDFOREIGNKEY 'risk_type',@sColumnName1='Claims_Cover_basis_ID',@sRefTableName='Claims_Cover_basis'
GO

DDLADDFOREIGNKEY 'risk_type',@sColumnName1='Claims_type_basis_ID',@sRefTableName='Claims_type_basis'
GO

declare @bExists TINYINT
declare @sOwnerName sysname
declare @ssql nvarchar(500)
EXECUTE @bExists = DDLExistsColumn 'product', 'Claims_Cover_basis_ID', @sOwnerName OUT

IF @bExists =1
BEGIN

SELECT @ssql='UPDATE rt
	SET 
		rt.Claims_Cover_basis_ID= p.Claims_Cover_basis_ID,
		rt.Claims_type_basis_ID =p.Claims_type_basis_ID 
	FROM product p 
		JOIN 
			Product_Risk_Type_Group prt ON p.product_id=prt.product_id
		JOIN 
			Risk_Type_Usage rtu ON rtu.risk_type_group_id=prt.risk_type_group_id
		JOIN 
			Risk_Type rt ON rtu.risk_type_id=rt.risk_type_id'
EXECUTE sp_executesql @ssql
END

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF54')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 54 ,	'DOCREF54'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IID')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)  
VALUES (54 ,	'IID' ,      	54 ,	'IID'    )  
END
GO

DDLDROPFOREIGNKEY 'product','Claims_Cover_basis_ID'
GO

DDLDROPFOREIGNKEY 'product','Claims_type_basis_ID'
GO

DDLDROPCOLUMN 'product','Claims_Cover_basis_ID'
GO

DDLDROPCOLUMN 'product','Claims_type_basis_ID'
GO
-- *****************************************************************************  
-- * Author:      Vijay Pal
-- * Date:         October 2013
-- * Purpose:     Password Access to Pure Insurance Back-Office and Web Portals
-- *****************************************************************************
EXEC DDLAddColumn 'PMUser', 'is_temp_password', 'BIT NOT NULL DEFAULT(0)'
GO
EXEC DDLAddColumn 'PMUser', 'incorrect_attempt_count', 'INT NOT NULL DEFAULT 0'
GO


DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'PMUser_Password_History'
IF @bExists = 0 BEGIN
	CREATE TABLE PMUser_Password_History
	(
	user_id smallint FOREIGN KEY References pmuser(user_id),
	historic_password varchar(255) ,
	date_added Datetime
	)

END
GO
EXEC DDLAddColumn @sTableName='pmuser', @sColumnName ='secure_password',@sColumnDefinition='varchar(255) NULL'
GO
EXEC DDLAddOrAlterColumn 'pmuser', 'password', 'varchar(255) NULL'
GO

-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        08 Nov 2013
-- * Purpose:     RND003 - DMEToSharePoint
-- *****************************************************************************
EXEC DDLAddColumn 'Doc_Document','DME_Migration_Status_id','INT'
EXEC DDLAddColumn 'Doc_Document','migration_id','INT'

IF Not Exists (Select 1 From Batch_Type Where Code = 'DMEMIG')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'DMEMIG', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'DME Migration',
        'DMEMIG'
    )
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'DME_Migration_Status'
IF @bExists = 0 BEGIN
CREATE TABLE DME_Migration_Status(
[DME_Migration_Status_id]			INT			NOT NULL  IDENTITY(1,1),
[code]						Varchar(20)		NOT NULL,
[description]					Varchar(255)		NOT NULL,
[caption_id]					Int			NOT NULL,
[effective_date]				DATETIME		NOT NULL,
[is_deleted]					TinyInt			NOT NULL
)

-- Add Primary Key
EXEC DDLADDPrimaryKey 'DME_Migration_Status', 'DME_Migration_Status_id'

IF NOT EXISTS (SELECT lookup_table_name 
FROM PMProduct_Lookup 
WHERE lookup_table_name = 'DME_Migration_Status')
BEGIN
 INSERT INTO PMProduct_Lookup
 (pmproduct_id, lookup_table_name, edit_privilege_level, 
 is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'DME_Migration_Status', 3, 0, 0)
END


Exec DDLAddForeignKey	
@sTableName = 'Doc_Document', 
@sColumnName1 = 'dme_migration_status_id', 
@sRefTableName = 'DME_Migration_Status'
END
GO

-- *****************************************************************************  
-- * Author:      Vijay Pal
-- * Date:         December 2013
-- * Purpose:     Pure 3.2 IFRS - Ex-gratia Payments
-- *****************************************************************************
EXEC DDLAddColumn 'Claim_Payment', 'is_ex_gratia', 'TINYINT NOT NULL DEFAULT 0' 
GO

EXEC DDLAddColumn 'Insurance_file','out_of_sequence_replaced','bit NULL'
GO
EXEC DDLAddColumn 'Insurance_file', 'risk_processed', 'bit NULL'
GO

Declare @bExists integer  

-- Check that the constraint exists.  
Execute @bExists = DDLExistsForeignKey 'mta_insurance_file_link',  'insurance_file_cnt'

If @bExists = 0
BEGIN
	-- Delete redundant records
	-- Little odd at this place but certain versions are going missing 
	-- and it's required to add a foreign key to identify the process responsible
	Delete mta_insurance_file_link From mta_insurance_file_link mifl
		Left Join insurance_file ifi On ifi.insurance_file_cnt = mifl.insurance_file_cnt
		Where ifi.insurance_file_cnt is null
		
	EXEC DDLAddForeignKey @sTableName='mta_insurance_file_link',@sColumnName1='Insurance_file_cnt',  
						@sRefTableName='Insurance_file', @sRefColumnName1='Insurance_file_cnt'  
END
GO

--Exec DDLAddIndex 'tax_calculation', 'policy_fee_u_id'
--GO


-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        22 Jan 2014
-- * Purpose:     Market Integration
-- *****************************************************************************
EXEC DDLAddColumn 'gis_data_model', 'is_imported_marketplace_data_model', 'tinyint DEFAULT 0 NOT NULL'
GO
EXEC DDLAddColumn 'gis_data_model', 'is_marketplace_data_model', 'tinyint DEFAULT 0 NOT NULL'
GO
EXEC DDLAddColumn 'GIS_USER_DEF_HEADER', 'system_generated', 'int NULL'
GO
EXEC DDLAddColumn 'GIS_USER_DEF_DETAIL', 'system_generated', 'int NULL'
GO

-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        19 Feb 2014
-- * Purpose:     Market Integration
-- *****************************************************************************
EXEC DDLAddColumn 'insurance_file', 'is_marketplace_policy', 'tinyint DEFAULT 0 NOT NULL'
GO

-- *****************************************************************************
-- * Author:      John Durnall
-- * Date:        18/04/2012
-- * Purpose:     wpr085 Add product dropdown to default bank account
-- *****************************************************************************
EXEC DDLADDCOLUMN 'BankAccount_Default','product_id','INT NULL',1
GO
EXEC DDLAddForeignKey 'BankAccount_Default', 'product_id', @sRefTableName = 'product'
GO

-- *****************************************************************************  
-- * Author:      Vidya Rangdale
-- * Date:        09-09-2014
-- * Purpose:      PM028205 - A table to remove min-1 logic
-- *****************************************************************************

DECLARE @exists INT
EXEC DDLEXISTSTABLE 'DocumentTempIDSequence',@exists OUT
IF ISNULL(@exists,0)=0 BEGIN
CREATE TABLE DocumentTempIDSequence
(
ID INT IDENTITY(-1,-1)
)

DECLARE @document_template_id INT
select @document_template_id=MIN(document_template_id) from document_template

SET IDENTITY_INSERT DocumentTempIDSequence ON 
INSERT INTO DocumentTempIDSequence (ID) VALUES (@document_template_id)
SET IDENTITY_INSERT DocumentTempIDSequence OFF
END
GO
-- *****************************************************************************
-- * Author:       Rakesh Barolia
-- * Date:          25/08/2014
-- * Purpose:       WPR46
-- *****************************************************************************

EXEC DDLADDCOLUMN 'Product','do_not_delete_renQuote_on_mta','TINYINT DEFAULT 0 NULL',1

GO 
-- *****************************************************************************
-- * Author:       Maninder Kaur
-- * Date:          15/09/2014
-- * Purpose:       WPR77
-- *****************************************************************************
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='User_Authorities' AND 
				COLUMN_NAME ='Agent_editable_During_MTA_MTC')
BEGIN
--Add Columns
EXEC DDLADDCOLUMN 'User_Authorities','Edit_Default_Commission_NB_RN','TINYINT DEFAULT 0 NOT NULL',1
EXEC DDLADDCOLUMN 'User_Authorities','Edit_Default_Commission_MTA','TINYINT DEFAULT 0 NOT NULL',1
EXEC DDLADDCOLUMN 'User_Authorities','Edit_Default_Commission_MTC','TINYINT DEFAULT 0 NOT NULL',1
EXEC DDLADDCOLUMN 'User_Authorities','Edit_Default_Commission_MTR','TINYINT DEFAULT 0 NOT NULL',1
EXEC DDLADDCOLUMN 'User_Authorities','Agent_Editable_During_MTA_MTC','TINYINT DEFAULT 1 NOT NULL',1


EXEC('UPDATE User_Authorities SET Edit_Default_Commission_NB_RN=1,Edit_Default_Commission_MTA=1,
							Edit_Default_Commission_MTC=1,Edit_Default_Commission_MTR=1
						WHERE Edit_Default_Commission=1')

END
						
GO

-- *****************************************************************************
-- * Author:       Maninder Kaur
-- * Date:          15/09/2014
-- * Purpose:       WPR90
-- *****************************************************************************

EXEC DDLADDCOLUMN 'Product','Unified_Renewal_Date_Is_Read_Only','TINYINT DEFAULT 0 NULL',1

GO        

-- *****************************************************************************  
-- * Author:      Prabodh Mishra
-- * Date:        25 Jun 2012
-- * Purpose:     Tech Spec - WPR105 - Debit Order Processing
-- *****************************************************************************

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'DOPaymentTerms' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		DOPaymentTerms(
			 DOPaymentTerms_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(10) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)	

END

EXEC DDLAddPrimaryKey @sTableName ='DOPaymentTerms', @sColumnName1 ='DOPaymentTerms_id' 

GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'CollectionFrequency' 
IF @bExists=0
BEGIN
	CREATE TABLE 
		CollectionFrequency(
			 CollectionFrequency_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(10) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)	

END

EXEC DDLAddPrimaryKey @sTableName ='CollectionFrequency', @sColumnName1 ='CollectionFrequency_id' 
GO

EXEC DDLAddColumn 'Insurance_file','DOPaymentTerms_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=Insurance_file,@sColumnName1=DOPaymentTerms_id,  
     @sRefTableName=DOPaymentTerms, @sRefColumnName1=DOPaymentTerms_id  
GO


EXEC DDLAddColumn 'Insurance_file','CollectionFrequency_id','Integer NULL'
GO

EXEC DDLAddForeignKey @sTableName=Insurance_file,@sColumnName1=CollectionFrequency_id,  
     @sRefTableName=CollectionFrequency, @sRefColumnName1=CollectionFrequency_id  
GO

EXEC DDLADDCOLUMN 'Product','default_cover_to_date_to_last_day','TINYINT DEFAULT 0 NULL',1
GO 

EXEC DDLADDCOLUMN 'Transaction_Export_Detail','fee_type','VARCHAR(50)',1
GO 

EXEC DDLADDCOLUMN 'TransDetail','fee_type','VARCHAR(50)',1
GO 

EXEC DDLADDCOLUMN 'TransDetail_Type','is_extended','TINYINT',1
GO 

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'TransDetailEx' 
IF @bExists=0
BEGIN

	CREATE TABLE [TransDetailEx] (
		[transdetailex_id] int NOT NULL IDENTITY(1, 1),
		[document_id] int NOT NULL,						
		[transdetail_id] int NOT NULL,
		[portion_no] int NOT NULL,						
		[effective_date] datetime NOT NULL,
		[period_id] int NOT NULL,						
		[account_id] int NOT NULL,						
		[currency_id] smallint NOT NULL,				
		[currency_amount] money NOT NULL,
		[currency_base_xrate] numeric(19, 10) NULL,		
		[outstanding_currency_amount] money NOT NULL,	
		[account_currency_id] smallint NOT NULL,		
		[account_amount] money NOT NULL,
		[account_base_xrate] numeric(19, 10) NULL,		
		[outstanding_account_amount] money NOT NULL,	
		[system_currency_id] smallint NOT NULL,			
		[system_amount] money NOT NULL,
		[system_base_xrate] numeric(19, 10) NULL,		
		[outstanding_system_amount] money NOT NULL
	)

	EXEC DDLAddPrimaryKey @sTableName ='TransDetailEx', @sColumnName1 ='TransDetailEx_id' 
	EXEC DDLAddForeignKey 'TransDetailEx', 'transdetail_id', @sRefTableName = 'transdetail', @sRefColumnName1='transdetail_id'
	EXEC DDLAddForeignKey 'TransDetailEx', 'document_id', @sRefTableName = 'document', @sRefColumnName1='document_id'
	EXEC DDLAddForeignKey 'TransDetailEx', 'account_id', @sRefTableName = 'account', @sRefColumnName1='account_id'
	EXEC DDLAddForeignKey 'TransDetailEx', 'period_id', @sRefTableName = 'period', @sRefColumnName1='period_id'
	EXEC DDLAddForeignKey 'TransDetailEx', 'currency_id', @sRefTableName = 'currency', @sRefColumnName1='currency_id'
	EXEC DDLAddForeignKey 'TransDetailEx', 'account_currency_id', @sRefTableName = 'currency', @sRefColumnName1='currency_id'
END
GO

-- *****************************************************************************  
-- * Author:      Prabodh Mishra
-- * Date:        25 Jun 2012
-- * Purpose:     Tech Spec - WPR81 - Transaction Type & Audit Trail
-- *****************************************************************************
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'AllocationBatch' 
IF @bExists=0
BEGIN

	CREATE TABLE [AllocationBatch] (
		[allocationbatch_id] int NOT NULL IDENTITY(1, 1),
		[allocationbatch_date] datetime NOT NULL,
		[period_id] int NOT NULL,
		[is_reversed] bit,
		[reversed_allocation_batch_id] int
	)

	EXEC DDLAddPrimaryKey @sTableName ='allocationbatch', @sColumnName1 ='allocationbatch_id' 
	
	EXEC DDLAddForeignKey 'allocationbatch', 'reversed_allocation_batch_id', 
					@sRefTableName = 'allocationbatch', @sRefColumnName1='allocationbatch_id'
	EXEC DDLAddForeignKey 'allocationbatch', 'period_id', 
					@sRefTableName = 'period', @sRefColumnName1='period_id'

	EXEC DDLAddColumn 'Allocation', 'allocationbatch_id', 'INT NULL'
	EXEC DDLAddForeignKey @sTableName='allocation', @sColumnName1 = 'allocationbatch_id', 
					@sRefTableName = 'allocationbatch', @sRefColumnName1='allocationbatch_id'

	EXEC DDLAddColumn 'AllocationDetail', 'alloc_account_amount', 'NUMERIC(19, 4) NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'alloc_system_amount', 'NUMERIC(19, 4) NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'is_reversed', 'TINYINT NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'allocation_reversed_date', 'DATETIME NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'MarkedForCollection_Type', 'INT NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'transdetailex_id', 'INT NULL'	
END
GO

-- *****************************************************************************  
-- * Author:      Prabodh Mishra
-- * Date:        11 Jul 2012
-- * Purpose:     Microsoft lab results
-- *****************************************************************************
--DDLADDINDEX GIS_POLICY_LINK,GIS_POLICY_LINK_ID
--GO
--DDLADDINDEX TransDetail,postingstatus_id
--GO
--DDLADDINDEX  Insurance_File,insurance_ref 
--GO
--DDLADDINDEX  Party,party_type_id 
--GO
--ddladdindex 'tax_calculation', 'ri_arrangement_line_id'
--GO
--DDLADDINDEX [Insurance_File], [Base_Insurance_File_Cnt]
--GO
--ddldropindex Hidden_options, branch_id, option_number, value
--GO
--DDLADDINDEX [RI_Arrangement_Line] , [ri_arrangement_id],[type],[ri_arrangement_line_id] 
--GO
--ddladdindex [policy_fee_u], [insurance_file_cnt],[risk_cnt],[policy_fee_u_id]
--GO
--ddladdindex [GIS_Policy_Link] , [case_id]
--GO
--ddladdindex insurance_file , insurance_folder_cnt, insurance_file_cnt
--GO
--ddladdindex [Claim] , [base_case_id] , [Currency_id] 
--GO
--ddladdindex Tax_Calculation , insurance_file_cnt, risk_cnt, transtype, value
--GO

-- *****************************************************************************  
-- * Author:      sandeep Kumar
-- * Date:        23 Jul 2012
-- * Purpose:     Tech Spec - WPR1061
-- *****************************************************************************
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'MakeLiveOptions' 
IF @bExists=0
BEGIN

	CREATE TABLE [MakeLiveOptions] (
		[MakeLiveOptions_id] int NOT NULL IDENTITY(1, 1),
		[code] VARCHAR(20) NULL,
		[description] VARCHAR(50) NULL,
		[effective_date] datetime NOT NULL,
		[caption_id] int NULL,
		[is_deleted] TINYINT NOT NULL
	)

	EXEC DDLAddPrimaryKey @sTableName ='MakeLiveOptions', @sColumnName1 ='MakeLiveOptions_id' 
		
END
GO

EXEC DDLAddColumn 'Fee_Amounts', 'MakeLiveOptions_id', 'INT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'DoPaymentTerms_id', 'INT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'Calculation_Basis', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'Is_Prorated', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'Is_Override', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'Use_When_Deleted', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'User_Id', 'INT NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'Date', 'DATETIME NULL'
GO
EXEC DDLAddColumn 'Fee_Amounts', 'Timestamp', 'DATETIME NULL'
GO

EXEC DDLAddColumn 'policy_fee_u', 'MakeLiveOptions_id', 'INT NULL'
GO
EXEC DDLAddColumn 'policy_fee_u', 'DoPaymentTerms_id', 'INT NULL'
GO
EXEC DDLAddColumn 'policy_fee_u', 'Calculation_Basis', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'policy_fee_u', 'Is_Prorated', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'policy_fee_u', 'Pro_rata_rate', 'NUMERIC(19,8) NULL'
GO
EXEC DDLAddColumn 'policy_fee_u', 'Is_Override', 'TINYINT NULL'
GO
EXEC DDLAddColumn 'policy_fee_u', 'fee_amount_id', 'INT NULL'
GO

-- *****************************************************************************  
-- * Author:      Richard Taylor
-- * Date:        17 Aug 2012
-- * Purpose:     SQL Performance Tuning
-- *****************************************************************************

-- Create an index on Agent Commission using explicit SQL rather than DDL as I want to 
-- specify non-key colums in the Index usinf the Include keyword.
--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = 'I__Agent_Commission__insurance_file_cnt__party_cnt')
--CREATE NONCLUSTERED INDEX 
--  [I__Agent_Commission__insurance_file_cnt__party_cnt] 
--  ON [dbo].[Agent_Commission] ( [insurance_file_cnt] ASC, [party_cnt] ASC ) 
--  include ( [risk_type_id], [commission_band_id], [commission_percentage], 
--[is_amended], [tax_group_id], [Maximum_rate], [is_value]) WITH (sort_in_tempdb =
-- OFF, ignore_dup_key = OFF, drop_existing = OFF, online = OFF) ON [PRIMARY] 
--GO 

--DDLADDINDEX [Insurance_File], [insurance_folder_cnt] , [cover_start_date]  , [insurance_file_type_id] , [insurance_file_cnt] , [lead_agent_cnt] 
--GO

--DDLADDINDEX [Insurance_File], [insurance_file_cnt] , [lead_agent_cnt]  
--GO

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_22_3')
--CREATE STATISTICS [_dta_stat_758293761_22_3] ON 
--[dbo].[Insurance_File]([cover_start_date], [insurance_file_type_id]) 
--GO 

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_3_1_11_7')
--CREATE STATISTICS [_dta_stat_758293761_3_1_11_7] ON 
--[dbo].[Insurance_File]([insurance_file_type_id], [insurance_file_cnt], [lead_agent_cnt], [insurance_folder_cnt])
--GO 

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_1_22_3_11')
--CREATE STATISTICS [_dta_stat_758293761_1_22_3_11] ON 
--[dbo].[Insurance_File]([insurance_file_cnt], [cover_start_date], [insurance_file_type_id], [lead_agent_cnt])
--GO 

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_758293761_7_1_11_22_3')
--CREATE STATISTICS [_dta_stat_758293761_7_1_11_22_3] ON 
--[dbo].[Insurance_File]([insurance_folder_cnt], [insurance_file_cnt], [lead_agent_cnt], [cover_start_date],
-- [insurance_file_type_id]) 
--GO  



---- Create an index on Policy Fee using explicit SQL rather than DDL as I want to 
---- specify non-key colums in the Index usinf the Include keyword.
--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = 'I__policy_fee_u__risk_cnt__insurance_file_cnt')
--CREATE NONCLUSTERED INDEX [I__policy_fee_u__risk_cnt__insurance_file_cnt] ON [dbo].[policy_fee_u] 
--(
--	[risk_cnt] ASC,
--	[insurance_file_cnt] ASC
--)
--INCLUDE ( [policy_fee_u_id]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]



---- Create an index on Policy Fee using explicit SQL rather than DDL as I want to 
---- specify non-key colums in the Index usinf the Include keyword.
--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = 'I__GIS_Screen_Detail__pre_quote_requirement__gis_object_id__gis_screen_id__gis_property_id')
--CREATE NONCLUSTERED INDEX 
--[I__GIS_Screen_Detail__pre_quote_requirement__gis_object_id__gis_screen_id__gis_property_id] 
--  ON [dbo].[GIS_Screen_Detail] ( [pre_quote_requirement] ASC, [gis_object_id] 
--ASC, [gis_screen_id] ASC, [gis_property_id] ASC ) 
--  include ( [PMFormat]) WITH (sort_in_tempdb = OFF, ignore_dup_key = OFF, 
--drop_existing = OFF, online = OFF) ON [PRIMARY] 
--GO

--DDLADDINDEX [Risk] , [risk_cnt] , [gis_screen_id] 
--GO

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_379864420_26_22_1')
--CREATE STATISTICS [_dta_stat_379864420_26_22_1] ON [dbo].[Risk]([is_risk_selected], [gis_screen_id], [risk_cnt])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_695673526_4_1')
--CREATE STATISTICS [_dta_stat_695673526_4_1] ON [dbo].[GIS_Screen_Detail]([gis_property_id], [gis_screen_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_695673526_3_1_4')
--CREATE STATISTICS [_dta_stat_695673526_3_1_4] ON [dbo].[GIS_Screen_Detail]([gis_object_id], [gis_screen_id], [gis_property_id])
--go

--IF NOT EXISTS (SELECT name FROM sysindexes
--    WHERE name = '_dta_stat_183671702_1_4')
--CREATE STATISTICS [_dta_stat_183671702_1_4] ON [dbo].[GIS_Property]([gis_property_id], [column_name])
--go
-- *****************************************************************************  
-- * Author:      Sandeep Kumar
-- * Date:        21 Jul 2014
-- * Purpose:     Tech Spec - WPR81 - Transaction Type & Audit Trail
-- *****************************************************************************
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'AllocationBatch' 
IF @bExists=0
BEGIN

	CREATE TABLE [AllocationBatch] (
		[allocationbatch_id] int NOT NULL IDENTITY(1, 1),
		[allocationbatch_date] datetime NOT NULL,
		[period_id] int NOT NULL,
		[is_reversed] bit,
		[reversed_allocation_batch_id] int
	)

	EXEC DDLAddPrimaryKey @sTableName ='allocationbatch', @sColumnName1 ='allocationbatch_id' 
	
	EXEC DDLAddForeignKey 'allocationbatch', 'reversed_allocation_batch_id', 
					@sRefTableName = 'allocationbatch', @sRefColumnName1='allocationbatch_id'
	EXEC DDLAddForeignKey 'allocationbatch', 'period_id', 
					@sRefTableName = 'period', @sRefColumnName1='period_id'

	EXEC DDLAddColumn 'Allocation', 'allocationbatch_id', 'INT NULL'
	EXEC DDLAddForeignKey @sTableName='allocation', @sColumnName1 = 'allocationbatch_id', 
					@sRefTableName = 'allocationbatch', @sRefColumnName1='allocationbatch_id'

	EXEC DDLAddColumn 'AllocationDetail', 'alloc_account_amount', 'NUMERIC(19, 4) NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'alloc_system_amount', 'NUMERIC(19, 4) NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'is_reversed', 'TINYINT NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'allocation_reversed_date', 'DATETIME NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'MarkedForCollection_Type', 'INT NULL'
	EXEC DDLAddColumn 'AllocationDetail', 'transdetailex_id', 'INT NULL'	
END
GO

-- *****************************************************************************  
-- * Author:      Prabodh Mishra
-- * Date:        29 Oct 2012
-- * Purpose:     OOS deadlock - PN80061
-- *****************************************************************************
--Exec DDLAddIndex 'tax_calculation', 'policy_fee_u_id'
--GO

-- *****************************************************************************  
-- * Author:      Vidya Rangdale
-- * Date:        16 Sep 2014
-- * Purpose:     WPR113
-- *****************************************************************************

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Batch_Renewal_Job_Run_Status' 
IF @bExists=0
BEGIN

CREATE TABLE [Batch_Renewal_Job_Run_Status](
	[batch_renewal_job_run_status_id] [int] NOT NULL,
	[caption_id] [int] NULL,
	[is_deleted] [bit] NOT NULL,
	[effective_date] [datetime] NOT NULL,
	[description] [varchar](255) NOT NULL,
	[code] [char](10) NOT NULL
	)

	EXEC DDLAddPrimaryKey @sTableName ='Batch_Renewal_Job_Run_Status', @sColumnName1 ='batch_renewal_job_run_status_id'
END
GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Batch_Renewal_Job_Run_Insurance_Folder' 
IF @bExists=0
BEGIN

CREATE TABLE [Batch_Renewal_Job_Run_Insurance_Folder](
	[batch_id] [int] NOT NULL,
	[insurance_folder_cnt] [int] NOT NULL,
	[batch_renewal_job_id] [int] NOT NULL,
	[batch_renewal_job_run_status_id] [int] NOT NULL,
	[recalculate_commission] [bit] NOT NULL,
	[recalculate_fees] [bit] NOT NULL,
	[recalculate_taxes] [bit] NOT NULL,
	[old_insurance_file_cnt] [int] NULL,
	[new_insurance_file_cnt] [int] NULL,
	[message] [varchar](64) NULL
	)

	EXEC DDLAddPrimaryKey @sTableName ='Batch_Renewal_Job_Run_Insurance_Folder', @sColumnName1 = 'batch_id', @sColumnName2 = 'insurance_folder_cnt'
	EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'USER',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Batch_Renewal_Job_Run_Insurance_Folder'
END
GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Batch_Renewal_Job_Run_Risk' 
IF @bExists=0
BEGIN

CREATE TABLE [dbo].[Batch_Renewal_Job_Run_Risk](
	[batch_id] [int] NOT NULL,
	[insurance_folder_cnt] [int] NOT NULL,
	[risk_folder_cnt] [int] NOT NULL,
	[rerate] [bit] NOT NULL,
	[recalculate_reinsurance] [bit] NOT NULL,
	[recalculate_fees] [bit] NOT NULL,
	[recalculate_taxes] [bit] NOT NULL
	)

	EXEC DDLAddPrimaryKey @sTableName ='Batch_Renewal_Job_Run_Risk', @sColumnName1 ='batch_id', @sColumnName2 ='insurance_folder_cnt', @sColumnName3 ='risk_folder_cnt'
END
GO

EXEC DDLAddForeignKey @sTableName='Batch_Renewal_Job_Run_Insurance_Folder', @sColumnName1='batch_id', @sRefTableName = 'Batch',@sRefColumnName1='batch_id'
GO
EXEC DDLAddForeignKey @sTableName='Batch_Renewal_Job_Run_Insurance_Folder', @sColumnName1='batch_renewal_job_id', @sRefTableName = 'Batch_Renewal_Job',@sRefColumnName1='batch_renewal_job_id'
GO
EXEC DDLAddForeignKey @sTableName='Batch_Renewal_Job_Run_Insurance_Folder', @sColumnName1='batch_renewal_job_run_status_id', @sRefTableName = 'Batch_Renewal_Job_Run_Status',@sRefColumnName1='batch_renewal_job_run_status_id'
GO
EXEC DDLAddForeignKey @sTableName='Batch_Renewal_Job_Run_Insurance_Folder', @sColumnName1='insurance_folder_cnt', @sRefTableName = 'Insurance_Folder',@sRefColumnName1='insurance_folder_cnt'
GO
EXEC DDLAddForeignKey @sTableName='Batch_Renewal_Job_Run_Risk', @sColumnName1='risk_folder_cnt', @sRefTableName = 'Risk_Folder',@sRefColumnName1='risk_folder_cnt'
GO
EXEC DDLAddOrAlterColumn 'Batch_Renewal_Job_Run_Insurance_Folder', 'message', 'varchar(256)'
GO
-- *****************************************************************************  
-- * Author:      Azeej Usmani
-- * Date:        16 Jan 2013
-- * Purpose:     Tech Spec - WPR100(a)
-- *****************************************************************************

EXEC DDLADDCOLUMN 'ri_arrangement','prop_calc_method_id','INT'
GO
EXEC DDLADDCOLUMN 'ri_arrangement','xol_calc_method_id','INT'
GO

-- *****************************************************************************  
-- * Author:      Azeej Usmani
-- * Date:        16 Feb 2013
-- * Purpose:     Tech Spec - WPR100(b)
-- *****************************************************************************

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'insurance_file_pt_log' 
IF @bExists=0
BEGIN

CREATE TABLE [insurance_file_pt_log](
	[insurance_file_cnt] [int] NOT NULL,
	[insurance_folder_cnt] [int] NOT NULL,
	[risk_cnt] [int] NOT NULL,
	[status_id] [int] NOT NULL --0 for failed, 1 for RI Calculated, 2 for Posted
	)

END

EXEC DDLAddForeignKey 'insurance_file_pt_log', 'insurance_file_cnt', @sRefTableName = 'insurance_file', @sRefColumnName1='insurance_file_cnt'
GO
EXEC DDLAddForeignKey 'insurance_file_pt_log', 'insurance_folder_cnt', @sRefTableName = 'insurance_folder', @sRefColumnName1='insurance_folder_cnt'
GO
EXEC DDLAddForeignKey 'insurance_file_pt_log', 'risk_cnt', @sRefTableName = 'risk', @sRefColumnName1='risk_cnt'
GO
--EXEC DDLADDCOLUMN 'insurance_file_pt_log', 'insurance_file_pt_log_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='insurance_file_pt_log', @sColumnName1 ='insurance_file_pt_log_ID'
--GO
--EXEC DDLAddIndex 'insurance_file_pt_log','insurance_file_pt_log_ID'
--GO
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Claim_pt_log' 
IF @bExists=0
BEGIN

CREATE TABLE [Claim_pt_log](
	[insurance_file_cnt] [int] NOT NULL,
	[base_claim_id] [int] NOT NULL,
	[claim_id] [int] NOT NULL,
	[status_id] [int] NOT NULL,--0 for failed, 1 for RI Calculated, 2 for Posted
	)

END

GO
--EXEC DDLADDCOLUMN 'Claim_pt_log', 'Claim_pt_log_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='Claim_pt_log', @sColumnName1 ='Claim_pt_log_ID'
--GO
--EXEC DDLAddIndex 'Claim_pt_log','Claim_pt_log_ID'
--GO
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'insurance_file_clone_log' 
IF @bExists=0
BEGIN

CREATE TABLE [insurance_file_clone_log](
	[insurance_file_cnt] [int] NOT NULL,
	[insurance_folder_cnt] [int] NOT NULL,
	[risk_cnt] [int] NOT NULL,
	[status_id] [int] NOT NULL,--0 for failed, 1 for RI Calculated, 2 for Posted
	)


END
EXEC DDLAddForeignKey 'insurance_file_clone_log', 'insurance_file_cnt', @sRefTableName = 'insurance_file', @sRefColumnName1='insurance_file_cnt'
EXEC DDLAddForeignKey 'insurance_file_clone_log', 'insurance_folder_cnt', @sRefTableName = 'insurance_folder', @sRefColumnName1='insurance_folder_cnt'
EXEC DDLAddForeignKey 'insurance_file_clone_log', 'risk_cnt', @sRefTableName = 'risk', @sRefColumnName1='risk_cnt'

EXEC DDLADDCOLUMN 'ri_arrangement','version_id','INT'
GO
EXEC DDLADDCOLUMN 'ri_arrangement','pro_rata_rate','float'
GO
--EXEC DDLADDCOLUMN 'insurance_file_clone_log', 'insurance_file_clone_log_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='insurance_file_clone_log', @sColumnName1 ='insurance_file_clone_log_ID'
--GO
--EXEC DDLAddIndex 'insurance_file_clone_log','insurance_file_clone_log_ID'
--GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Risk_RI_Status' 
IF @bExists=0
BEGIN
CREATE TABLE 
		Risk_RI_Status(
			 status_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(30) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)		

END
GO
EXEC DDLAddPrimaryKey @sTableName ='Risk_RI_Status', @sColumnName1 ='status_id' 
GO
--EXEC DDLAddIndex 'Risk_RI_Status','status_id'
--GO

EXEC DDLAddForeignKey @sTableName=insurance_file_pt_log,@sColumnName1=status_id,  
     @sRefTableName=Risk_RI_Status, @sRefColumnName1=status_id  
GO

EXEC DDLAddForeignKey @sTableName=insurance_file_clone_log,@sColumnName1=status_id,  
     @sRefTableName=Risk_RI_Status, @sRefColumnName1=status_id  
GO

EXEC DDLAddForeignKey @sTableName=Claim_pt_log,@sColumnName1=status_id,  
     @sRefTableName=Risk_RI_Status, @sRefColumnName1=status_id  
GO
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'RI_Version_Type' 
IF @bExists=0
BEGIN
CREATE TABLE 
		RI_Version_Type(
			 RI_Version_Type_id INT IDENTITY(1,1) NOT NULL,
			 caption_id INT NULL ,
			 code CHAR(30) NULL ,
			 Description Varchar(255) NULL,
			 effective_date datetime,
			 is_deleted bit
			)		

END
GO
EXEC DDLAddPrimaryKey @sTableName ='RI_Version_Type', @sColumnName1 ='RI_Version_Type_id' 
GO
--EXEC DDLAddIndex 'RI_Version_Type','RI_Version_Type_id'
--GO
EXEC DDLADDCOLUMN 'ri_arrangement','RI_Version_Type_id','INT'
GO
EXEC DDLADDCOLUMN 'ri_arrangement','Effective_Date','DATETIME'
GO

EXEC DDLAddForeignKey @sTableName='ri_arrangement',@sColumnName1='RI_Version_Type_id',  
     @sRefTableName='RI_Version_Type', @sRefColumnName1='RI_Version_Type_id'  
GO

EXEC DDLADDCOLUMN 'insurance_file_pt_log','Effective_Date','DATETIME'
GO

EXEC DDLADDCOLUMN 'Claim_pt_log','Effective_Date','DATETIME'
GO

-- *****************************************************************************  
-- * Author:      Azeej Usmani
-- * Date:        23 APR 2013
-- * Purpose:     WPR100(b) - to maintain deleted data in archive tables for clone transfer
-- *****************************************************************************

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'RI_Arrangement_Line_Archive' 
IF @bExists=0
BEGIN 
CREATE TABLE RI_Arrangement_Line_Archive(
	ri_arrangement_line_id int NOT NULL,
	ri_arrangement_id int NULL,
	type varchar(3) NULL,
	treaty_id int NULL,
	party_cnt int NULL,
	default_share_percent float NOT NULL,
	this_share_percent float NOT NULL,
	premium_percent float NOT NULL,
	commission_percent float NOT NULL,
	agreement_code varchar(255) NULL,
	priority int NOT NULL,
	number_of_lines smallint NOT NULL,
	line_limit money NOT NULL,
	sum_insured money NOT NULL,
	premium_value money NOT NULL,
	commission_value money NOT NULL,
	premium_tax money NULL,
	commission_tax money NULL,
	is_commission_modified tinyint NOT NULL,
	retained float NULL,
	lower_limit money NULL,
	participation_percent float NULL,
	grouping int NULL,
	ri_model_line_id int NULL,
	Is_Obligatory tinyint NULL
)
END
GO
--EXEC DDLADDCOLUMN 'RI_Arrangement_Line_Archive', 'RI_Arrangement_Line_Archive_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='RI_Arrangement_Line_Archive', @sColumnName1 ='RI_Arrangement_Line_Archive_ID'
--GO
--EXEC DDLAddIndex 'RI_Arrangement_Line_Archive','RI_Arrangement_Line_Archive_ID'
--GO
--EXEC DDLAddForeignKey 'RI_Arrangement_Line_Archive', 'ri_arrangement_id', @sRefTableName = 'ri_arrangement', @sRefColumnName1='ri_arrangement_id'
--GO
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'RI_Arrangement_line_Broker_Participants_Archive' 
IF @bExists=0
BEGIN
CREATE TABLE [RI_Arrangement_line_Broker_Participants_Archive](
	[ri_arrangement_line_id] [int] NOT NULL,
	[ri_party_cnt] [int] NOT NULL,
	[participation_percent] [float] NOT NULL
) 
END
GO
--EXEC DDLADDCOLUMN 'RI_Arrangement_line_Broker_Participants_Archive', 'RI_Arrangement_line_Broker_Participants_Archive_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='RI_Arrangement_line_Broker_Participants_Archive', @sColumnName1 ='RI_Arrangement_line_Broker_Participants_Archive_ID'
--GO
--EXEC DDLAddIndex 'RI_Arrangement_line_Broker_Participants_Archive','RI_Arrangement_line_Broker_Participants_Archive_ID'
--GO
--EXEC DDLAddForeignKey 'RI_Arrangement_line_Broker_Participants_Archive', 'ri_arrangement_line_id', @sRefTableName = 'ri_arrangement_line', @sRefColumnName1='ri_arrangement_line_id'
--GO
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Claim_RI_Arrangement_Line_Archive' 
IF @bExists=0
BEGIN
CREATE TABLE [Claim_RI_Arrangement_Line_Archive](
	[claim_id] [int] NOT NULL,
	[ri_arrangement_line_id] [int] NOT NULL,
	[ri_arrangement_id] [int] NULL,
	[type] [varchar](3) NULL,
	[treaty_id] [int] NULL,
	[party_cnt] [int] NULL,
	[xol_arrangement_id] [int] NULL,
	[default_share_percent] [float] NOT NULL,
	[this_share_percent] [float] NULL,
	[agreement_code] [varchar](255) NULL,
	[priority] [int] NOT NULL,
	[number_of_lines] [smallint] NOT NULL,
	[line_limit] [money] NOT NULL,
	[sum_insured] [money] NULL,
	[reserve] [money] NOT NULL,
	[payment] [money] NOT NULL,
	[salvage] [money] NOT NULL,
	[recovery] [money] NOT NULL,
	[this_reserve] [money] NOT NULL,
	[this_payment] [money] NOT NULL,
	[this_salvage] [money] NOT NULL,
	[this_recovery] [money] NOT NULL,
	[claim_ri_arrangement_line_id] [int] NOT NULL,
	[base_claim_ri_arrangement_line_id] [int] NULL,
	[version_id] [int] NULL,
	[original_ri_arrangement_line_id] [int] NULL,
	[retained] [float] NULL,
	[lower_limit] [money] NULL,
	[participation_percent] [float] NULL,
	[grouping] [int] NULL,
	[Is_Obligatory] [tinyint] NULL
)
END
GO
--EXEC DDLADDCOLUMN 'Claim_RI_Arrangement_Line_Archive', 'Claim_RI_Arrangement_Line_Archive_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='Claim_RI_Arrangement_Line_Archive', @sColumnName1 ='Claim_RI_Arrangement_Line_Archive_ID'
--GO
--EXEC DDLAddIndex 'Claim_RI_Arrangement_Line_Archive','Claim_RI_Arrangement_Line_Archive_ID'
--GO
--EXEC DDLAddForeignKey 'Claim_RI_Arrangement_Line_Archive', 'ri_arrangement_line_id', @sRefTableName = 'ri_arrangement_line', @sRefColumnName1='ri_arrangement_line_id'
--GO
DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Claim_RI_Arrangement_line_Broker_Participants_Archive' 
IF @bExists=0
BEGIN
CREATE TABLE [Claim_RI_Arrangement_line_Broker_Participants_Archive](
	[claim_ri_arrangement_line_id] [int] NOT NULL,
	[ri_party_cnt] [int] NOT NULL,
	[participation_percent] [float] NOT NULL
) 
END

GO
--EXEC DDLADDCOLUMN 'Claim_RI_Arrangement_line_Broker_Participants_Archive', 'Claim_RI_Arrangement_line_Broker_Participants_Archive_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='Claim_RI_Arrangement_line_Broker_Participants_Archive', @sColumnName1 ='Claim_RI_Arrangement_line_Broker_Participants_Archive_ID'
--GO
--EXEC DDLAddIndex 'Claim_RI_Arrangement_line_Broker_Participants_Archive','Claim_RI_Arrangement_line_Broker_Participants_Archive_ID'
--GO
--EXEC DDLAddForeignKey 'Claim_RI_Arrangement_line_Broker_Participants_Archive', 'claim_ri_arrangement_line_id', @sRefTableName = 'claim_ri_arrangement_line', @sRefColumnName1='claim_ri_arrangement_line_id'
--GO

DECLARE @bExists INT
EXEC @bExists = DDLExistsTable 'Claim_RI_Arrangement_Archive' 
IF @bExists=0
BEGIN
CREATE TABLE [Claim_RI_Arrangement_Archive](
	[claim_id] [int] NOT NULL,
	[ri_arrangement_id] [int] NOT NULL,
	[risk_cnt] [int] NOT NULL,
	[ri_band_id] [int] NULL,
	[ri_model_id] [int] NULL,
	[claim_allocation_type] [tinyint] NULL,
	[sum_insured] [money] NOT NULL,
	[reserve] [money] NOT NULL,
	[payment] [money] NOT NULL,
	[salvage] [money] NOT NULL,
	[recovery] [money] NOT NULL,
	[is_modified] [tinyint] NULL,
	[this_reserve] [money] NOT NULL,
	[this_payment] [money] NOT NULL,
	[this_salvage] [money] NOT NULL,
	[this_recovery] [money] NOT NULL,
	[claim_ri_arrangement_id] [int] NOT NULL,
	[base_claim_ri_arrangement_id] [int] NULL,
	[version_id] [int] NULL,
	[original_ri_arrangement_id] [int] NULL,
	[ri_arrangement_version] [int] NULL,
	[Cloned] [tinyint] NOT NULL,
	[xol_ri_model_id] [int] NULL
) 
END
GO
--EXEC DDLADDCOLUMN 'Claim_RI_Arrangement_Archive', 'Claim_RI_Arrangement_Archive_ID','INT NOT NULL'
--GO
--EXEC DDLAddPrimaryKey @sTableName ='Claim_RI_Arrangement_Archive', @sColumnName1 ='Claim_RI_Arrangement_Archive_ID'
--GO
--EXEC DDLAddIndex 'Claim_RI_Arrangement_Archive','Claim_RI_Arrangement_Archive_ID'
--GO

EXEC DDLADDCOLUMN 'Claim_RI_Arrangement_Line','ri_model_line_id','INT'
GO

--EXEC DDLAddForeignKey 'Claim_RI_Arrangement_Archive', 'ri_arrangement_id', @sRefTableName = 'ri_arrangement', @sRefColumnName1='ri_arrangement_id'
--GO
-- *****************************************************************************  
-- * Author:       Azeej Usmani
-- * Date:         25-02-2014
-- * Purpose:      Date Versioning Development
-- *****************************************************************************
DECLARE @exists int
EXEC DDLEXISTSTABLE 'Risk_Type_RI_Limit_Version',@exists OUT
IF ISNULL(@exists,0)=0 BEGIN
CREATE TABLE Risk_Type_RI_Limit_Version(
	[risk_type_ri_limit_version_id] [int] identity primary key NOT NULL,
	[risk_type_id] [int] NOT NULL,
	[description] varchar (255),
	[ri_limit_start_date] [datetime] NULL,
	[ri_limit_end_date] [datetime] NULL
) ON [PRIMARY]

END
GO
--EXEC DDLAddIndex 'Risk_Type_RI_Limit_Version','risk_type_ri_limit_version_id'
--GO
EXEC DDLADDCOLUMN 'risk_type_ri_properties','risk_type_ri_limit_version_id','INT'
GO
EXEC DDLADDCOLUMN 'Risk_Type_RI_Values','risk_type_ri_limit_version_id','INT'
GO

IF EXISTS(SELECT NULL FROM risk_type_ri_properties where risk_type_id Is NOT Null and risk_type_ri_limit_version_id  is null)
BEGIN

DECLARE @risk_type_id INT
DECLARE @risk_type_ri_limit_version_id INT

DECLARE UPD_RI_LIMITCURSOR CURSOR FAST_FORWARD FOR
SELECT DISTINCT risk_type_id  from risk_type_ri_properties where risk_type_ri_limit_version_id  is null

OPEN UPD_RI_LIMITCURSOR

FETCH NEXT FROM UPD_RI_LIMITCURSOR INTO @risk_type_id

WHILE @@FETCH_STATUS=0
BEGIN
	INSERT INTO [Risk_Type_RI_Limit_Version] (  
		risk_type_id ,  
		description  ,  
		ri_limit_start_date,
		ri_limit_end_date )  
	VALUES (  
		@risk_type_id,  
		'RILimit',  
		'2000-01-01',
		'2014-06-30')  

	SELECT @risk_type_ri_limit_version_id = @@IDENTITY

	UPDATE risk_type_ri_properties SET risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id where risk_type_id =@risk_type_id
	
	UPDATE Risk_Type_RI_Values SET risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id where risk_type_id =@risk_type_id
	
	FETCH NEXT FROM UPD_RI_LIMITCURSOR INTO @risk_type_id
END

CLOSE UPD_RI_LIMITCURSOR
DEALLOCATE UPD_RI_LIMITCURSOR

END
GO

EXEC DDLDropPrimaryKey 'risk_type_ri_properties', 'risk_type_id', 'risk_type_ri_properties_seq_id'
GO
EXEC DDLDropPrimaryKey 'risk_type_ri_values','risk_type_id', 'gis_user_def_header_inds_id1', 'gis_user_def_header_inds_id2', 'gis_user_def_header_inds_id3'
GO

EXEC DDLADDCOLUMN 'risk_type_ri_properties', 'risk_type_ri_limit_version_id', 'INT'
GO
UPDATE risk_type_ri_properties 
SET risk_type_ri_limit_version_id = 1 
WHERE risk_type_ri_limit_version_id IS NULL
GO
EXEC DDLADDCOLUMN 'Risk_Type_RI_Values', 'risk_type_ri_limit_version_id', 'INT'
GO
UPDATE Risk_Type_RI_Values 
SET risk_type_ri_limit_version_id = 1 
WHERE risk_type_ri_limit_version_id IS NULL
GO
EXEC DDLAlterColumn 'risk_type_ri_properties', 'risk_type_ri_limit_version_id', 'INT NOT NULL'
GO
EXEC DDLAlterColumn 'Risk_Type_RI_Values', 'risk_type_ri_limit_version_id', 'INT NOT NULL'
GO

EXEC DDLAddPrimaryKey  'risk_type_ri_properties', 'risk_type_id', 'risk_type_ri_properties_seq_id','risk_type_ri_limit_version_id'
GO
EXEC DDLAddPrimaryKey 'risk_type_ri_values','risk_type_id', 'gis_user_def_header_inds_id1', 'gis_user_def_header_inds_id2', 'gis_user_def_header_inds_id3','risk_type_ri_limit_version_id'
GO

EXEC DDLAddForeignKey @sTableName=risk_type_ri_properties,@sColumnName1=risk_type_ri_limit_version_id,  
     @sRefTableName=Risk_Type_RI_Limit_Version, @sRefColumnName1=risk_type_ri_limit_version_id  
GO
EXEC DDLAddForeignKey @sTableName=Risk_Type_RI_Values,@sColumnName1=risk_type_ri_limit_version_id,  
     @sRefTableName=Risk_Type_RI_Limit_Version, @sRefColumnName1=risk_type_ri_limit_version_id 
GO

-- *****************************************************************************  
-- * Author:       Azeej Usmani
-- * Date:         25-02-2014
-- * Purpose:      CLAIM INCURRED Development
-- *****************************************************************************

EXEC DDLAddColumn 'Claim_RI_Arrangement','incurred_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement','reserve_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement','payment_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement','salvage_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement','recovery_to_date','Money'
GO
EXEC DDLADDCOLUMN 'Claim_RI_Arrangement','extended_limit_amount','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line','reserve_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line','payment_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line','salvage_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line','recovery_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line','claim_incurred_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line','is_pt_archive','TINYINT NOT NULL DEFAULT 0'
GO

Exec DDLDropAlternateKey 'Claim_RI_Arrangement_Line', 'claim_id', 'ri_arrangement_id', 'priority', 'ri_arrangement_line_id'
  GO
Exec DDLAddAlternateKey  'Claim_RI_Arrangement_Line', 'claim_id', 'ri_arrangement_id', 'priority', 'ri_arrangement_line_id','is_pt_archive'
  GO
Exec DDLDropPrimaryKey  'Claim_RI_Arrangement_Line', 'claim_id','ri_arrangement_line_id'
  GO    
Exec DDLAddPrimaryKey  'Claim_RI_Arrangement_Line', 'claim_id','ri_arrangement_line_id','is_pt_archive'
  GO
  
--EXEC DDLADDINDEX 'Claim_ri_arrangement','claim_ri_arrangement_id'
--GO


--EXEC DDLDropIndex 'RI_Arrangement_Line','party_cnt'
--EXEC DDLDropIndex 'RI_Arrangement_Line','treaty_id'


--EXEC DDLADDINDEX 'Claim_RI_Arrangement_line_Broker_Participants','claim_ri_arrangement_line_id'
--GO
--EXEC DDLADDINDEX 'insurance_file_pt_log','insurance_file_cnt'
--GO
--EXEC DDLADDINDEX 'insurance_file_clone_log','insurance_file_cnt'
--GO

-- *****************************************************************************
-- * Author:      Tracy Deaville
-- * Date:        20/02/2012
-- * Purpose:     OOS MTA re-design
-- * 		  Changed to match tech spec
-- *****************************************************************************
--delete FKs from all the Gis tables that customer have created to GIS_Policy_link table
--to do this iterate through all the entries in the GIS_Data_Model table 
DECLARE GIS_Cursor Cursor 
FOR
SELECT 	'ALTER TABLE '+object_name(a.parent_object_id)+
	' DROP CONSTRAINT '+ a.name
FROM 	sys.foreign_keys a
     	join sys.foreign_key_columns b
                    ON a.object_id=b.constraint_object_id
     	join sys.columns c
                    ON b.constraint_column_id = c.column_id
	          and b.parent_object_id=c.object_id
     	join sys.columns d
                    ON b.referenced_column_id = d.column_id
	          and b.referenced_object_id = d.object_id
WHERE   object_name(b.referenced_object_id) in
	('gis_policy_link')
ORDER BY c.name

OPEN GIS_Cursor
DECLARE @SQLString nVarchar(255)

FETCH NEXT FROM GIS_Cursor INTO @SQLString 
WHILE (@@FETCH_STATUS <> -1)
	BEGIN
		IF (@@FETCH_STATUS <> -2)
		EXECUTE sp_executesql @SQLString
		FETCH NEXT FROM GIS_Cursor INTO @SQLString 
	END
CLOSE GIS_Cursor
DEALLOCATE GIS_Cursor
GO
EXEC DDLDropPrimaryKey 'GIS_Policy_Link', 'gis_policy_link_id'
GO

--EXEC DDLADDINDEX GIS_POLICY_LINK,GIS_POLICY_LINK_ID
--GO

EXEC DDLAddColumn 'Claim_RI_Arrangement_Archive','incurred_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Archive','reserve_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Archive','payment_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Archive','salvage_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Archive','recovery_to_date','Money'
GO
EXEC DDLADDCOLUMN 'Claim_RI_Arrangement_Archive','extended_limit_amount','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','ri_model_line_id','INT'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','reserve_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','payment_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','salvage_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','recovery_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','claim_incurred_to_date','Money'
GO
EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive','is_pt_archive','TINYINT NOT NULL DEFAULT 0'
GO
EXEC DDLAddColumn 'Debtor_User_Groups', 'Is_Payment_Type_Claim_Payment', 'tinyint NULL'
GO
--*****************************************************************************
-- * Author:       sandeep kumar
-- * Date:          6/11/2015
-- * Purpose:       WPR91
-- *****************************************************************************

EXEC DDLADDCOLUMN 'TransMatch','CashListItem_ID','INT DEFAULT 0 NULL',1

GO
EXEC DDLADDCOLUMN 'CashListItem','TaxAmount','Numeric(19,4) DEFAULT 0 NULL',1

GO
EXEC DDLADDCOLUMN 'CashList','PMNav_Batch_Key','INT DEFAULT 0 NULL',1

-- *****************************************************************************
-- * Author:        Goldy Saini
-- * Date:          06/06/2015
-- * Purpose:       WPR90-A
-- *****************************************************************************

EXEC DDLADDCOLUMN 'Product','unified_renewal_date_is_read_only','TINYINT DEFAULT 0 NULL',1

GO        

-- *****************************************************************************
-- * Author:        Goldy Saini
-- * Date:          25/08/2015
-- * Purpose:       TFS WI-12768
-- *****************************************************************************
Exec DDLDropAlternateKey 'Risk_Folder', 'risk_folder_id', 'source_id', null, null, null, null, null, null, 1
Go
Exec DDLAlterColumn 'Risk_Folder', 'code', 'varchar(40) NULL', 1
Go

-- *****************************************************************************
-- * Author:       Nishchal Upadhayay
-- * Date:         18-02-2014
-- * Purpose:      Add Missing Codes to ACTNUMBER_GROUP
-- *****************************************************************************

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF37')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 37 ,	'DOCREF37'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF38')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 38 ,	'DOCREF38'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF39')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 39 ,	'DOCREF39'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF40')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 40 ,	'DOCREF40'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF41')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 41 ,	'DOCREF41'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF42')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 42 ,	'DOCREF42'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF43')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 43 ,	'DOCREF43'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF44')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 44 ,	'DOCREF44'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF45')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 45 ,	'DOCREF45'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF46')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 46 ,	'DOCREF46'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF47')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 47 ,	'DOCREF47'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF48')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 48 ,	'DOCREF48'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF50')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 50 ,	'DOCREF50'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF51')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 51 ,	'DOCREF51'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF52')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 52 ,	'DOCREF52'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF53')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 53 ,	'DOCREF53'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF54')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 54 ,	'DOCREF54'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF55')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 55 ,	'DOCREF55'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

-- *****************************************************************************  
-- * Author:       Nishchal Upadhayay
-- * Date:         18-02-2014
-- * Purpose:      Add Missing Codes to ACTNUMBER_RANGE
-- *****************************************************************************
IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IDR')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (37 ,	'IDR' ,      	37 ,	'IDR'    ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'ICR')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (38 ,	'ICR' ,      	38 ,	'ICR'    )     
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'ICA')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (39 ,	'ICA' ,     	39 ,	'ICA'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'CLO')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (40 ,	'CLO' ,      	40 ,	'CLO'    )    
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'CLA')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (41 ,	'CLA' ,      	41 ,	'CLA'    )  
END
GO
 
IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IND')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (42 ,	'IND' ,      	42 ,	'IND'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'INC')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (43 ,	'INC' ,      	43 ,	'INC'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IED')
BEGIN  
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (44 ,	'IED' ,      	44 ,	'IED'    ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IEC')
BEGIN   
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (45 ,	'IEC' ,      	45 ,	'IEC'    )    
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IRD')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (46 ,	'IRD' ,      	46 ,	'IRD'    )    
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IRC')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (47 ,	'IRC' ,      	47 ,	'IRC'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'BJN')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (48 ,	'BJN' ,      	48 ,	'BJN'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SDD')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (50 ,	'SDD' ,     	50 ,	'SDD'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SPD')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (51 ,	'SPD' ,      	51 ,	'SPD'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SID')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (52 ,	'SID' ,      	52 ,	'SID'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SIC')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (53 ,	'SIC' ,      	53 ,	'SIC'    ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IID')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)  
VALUES (54 ,	'IID' ,      	54 ,	'IID'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IIC')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (55 ,	'IIC' ,      	55 ,	'IIC'    )
END
GO   


-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        08 Nov 2013
-- * Purpose:     RND003 - DMEToSharePoint
-- *****************************************************************************
EXEC DDLAddColumn 'Doc_Document','DME_Migration_Status_id','INT'
EXEC DDLAddColumn 'Doc_Document','migration_id','INT'

IF Not Exists (Select 1 From Batch_Type Where Code = 'DMEMIG')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'DMEMIG', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'DME Migration',
        'DMEMIG'
    )
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'DME_Migration_Status'
IF @bExists = 0 BEGIN
CREATE TABLE DME_Migration_Status(
[DME_Migration_Status_id]			INT			NOT NULL  IDENTITY(1,1),
[code]						Varchar(20)		NOT NULL,
[description]					Varchar(255)		NOT NULL,
[caption_id]					Int			NOT NULL,
[effective_date]				DATETIME		NOT NULL,
[is_deleted]					TinyInt			NOT NULL
)

-- Add Primary Key
EXEC DDLADDPrimaryKey 'DME_Migration_Status', 'DME_Migration_Status_id'

IF NOT EXISTS (SELECT lookup_table_name 
FROM PMProduct_Lookup 
WHERE lookup_table_name = 'DME_Migration_Status')
BEGIN
 INSERT INTO PMProduct_Lookup
 (pmproduct_id, lookup_table_name, edit_privilege_level, 
 is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'DME_Migration_Status', 3, 0, 0)
END


Exec DDLAddForeignKey	
@sTableName = 'Doc_Document', 
@sColumnName1 = 'dme_migration_status_id', 
@sRefTableName = 'DME_Migration_Status'
END
GO
--***********************************
-- 3.2SR2 Performance Lab Changes
-- 10/07/2014
--***********************************
--IF Exists ( SELECT * from sys.indexes WHERE NAME ='IX_Claim_is_dirty')
--	DROP INDEX IX_Claim_is_dirty ON Claim
--GO
--CREATE NONCLUSTERED INDEX IX_Claim_is_dirty
--    ON Claim(is_dirty)
--INCLUDE
--    (claim_id, base_claim_id)
--GO
--EXEC DDLADDINDEX 'Event_log','CASE_id'
--GO
--EXEC DDLADDINDEX 'Claim_ri_arrangement','claim_ri_arrangement_id'
--GO
--EXEC DDLADDINDEX 'mta_insurance_file_link','new_linked_insurance_file_cnt'
--GO
--EXEC DDLADDINDEX 'GIS_property','property_name'
--GO
--IF Exists ( SELECT * from sys.indexes WHERE NAME ='IX_CurrencyRate_effective_from')
--	DROP INDEX IX_CurrencyRate_effective_from ON CurrencyRate
--GO

--CREATE NONCLUSTERED INDEX IX_CurrencyRate_effective_from
--ON CurrencyRate(effective_from)
--INCLUDE
--(rate_against_base)

--GO
--IF Exists ( SELECT * from sys.indexes WHERE NAME ='IX_ri_arrangement_line_Broker_Participants_ri_arrangement_line_id')
--	DROP INDEX IX_ri_arrangement_line_Broker_Participants_ri_arrangement_line_id ON ri_arrangement_line_Broker_Participants
--GO

--CREATE NONCLUSTERED INDEX IX_ri_arrangement_line_Broker_Participants_ri_arrangement_line_id
--ON ri_arrangement_line_Broker_Participants(ri_arrangement_line_id)
--INCLUDE(ri_party_cnt)

--GO
--DDLADDINDEX 'insurance_file_risk_link','original_risk_cnt'
--GO


	

-- *****************************************************************************
-- * Author:  Ritu Sharma
-- * Date:    24/11/2014
-- * Purpose: To edit rate in Tax Band from Lookup
-- *****************************************************************************

EXEC DDLDropForeignKey 'Tax_Calculation', 'Tax_band_rate_id'
GO

--EXEC DDLAddIndex 'Commission_Arrangement', 'Party_type', 'party_cnt', 'Product_id', 'risk_type_id', 'transaction_type_id', 'commission_band_id',  'Commission_Level_ID', 'tax_group_id'
--GO

EXEC DDLAddColumn 'claim','SearchResultsCol1','varchar(255)'
GO


-- *****************************************************************************
-- * Author:   Kapil sanotra
-- * Date:     22/04/2015
-- * Purpose:   Remove identity constraint from id columns of tables UDL_DVLA
-- * 24/04/2015 Richard Clarke - added check for UDL_DVLA table as errors if not present
-- *****************************************************************************

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'UDL_DVLA'

IF @bExists <> 0 
BEGIN

	EXEC DDLDropTable 'Tmp_UDL_DVLA'

	IF(SELECT count(*) FROM syscolumns WHERE status <> 128 AND 
		id = (SELECT id FROM sysobjects WHERE name = 'UDL_DVLA') AND name='udl_dvla_id')=0
	BEGIN
		CREATE TABLE [dbo].[Tmp_UDL_DVLA](
		[udl_dvla_id] [int]  NOT NULL,
		[caption_id] [int] NOT NULL,
		[code] [char](10) NOT NULL,
		[description] [varchar](255) NULL,
		[is_deleted] [tinyint] NOT NULL,
		[effective_date] [datetime] NOT NULL,
		[vrm] [varchar](255) NULL,
		[fuel] [varchar](255) NULL,
		[transmission] [varchar](255) NULL,
		[make] [varchar](255) NULL,
		[model] [varchar](255) NULL,
		[engine_no] [varchar](255) NULL,
		[DVLA_BPD] [varchar](255) NULL,
		[SMMT_DPC] [varchar](255) NULL,
		[engine_size] [varchar](255) NULL,
		[DVLA_BPC] [varchar](255) NULL,
		[make_code] [varchar](255) NULL,
		[fuel_code] [varchar](255) NULL,
		[SMMT_DPD] [varchar](255) NULL,
		[colour_code] [varchar](255) NULL,
		[door_plan] [varchar](255) NULL,
		[colour] [varchar](255) NULL,
		[model_code] [varchar](255) NULL,
		[transmission_code] [varchar](255) NULL,
		[VIN] [varchar](255) NULL,
		[version] [varchar](255) NULL,
		[MFR_end_year] [varchar](255) NULL,
		[transmission2] [varchar](255) NULL,
		[model2] [varchar](255) NULL,
		[make2] [varchar](255) NULL,
		[fuel_code2] [varchar](255) NULL,
		[match_level] [varchar](255) NULL,
		[DVLACode] [varchar](255) NULL,
		[enginesize2] [varchar](255) NULL,
		[body_type] [varchar](255) NULL,
		[MFR_start_yr] [varchar](255) NULL,
		[number_doors] [varchar](255) NULL
	) ON [PRIMARY]

		IF EXISTS(SELECT * FROM dbo.UDL_DVLA)
			 EXEC('INSERT INTO [dbo].[Tmp_UDL_DVLA]
			   ([udl_dvla_id]
			   ,[caption_id]
			   ,[code]
			   ,[description]
			   ,[is_deleted]
			   ,[effective_date]
			   ,[vrm]
			   ,[fuel]
			   ,[transmission]
			   ,[make]
			   ,[model]
			   ,[engine_no]
			   ,[DVLA_BPD]
			   ,[SMMT_DPC]
			   ,[engine_size]
			   ,[DVLA_BPC]
			   ,[make_code]
			   ,[fuel_code]
			   ,[SMMT_DPD]
			   ,[colour_code]
			   ,[door_plan]
			   ,[colour]
			   ,[model_code]
			   ,[transmission_code]
			   ,[VIN]
			   ,[version]
			   ,[MFR_end_year]
			   ,[transmission2]
			   ,[model2]
			   ,[make2]
			   ,[fuel_code2]
			   ,[match_level]
			   ,[DVLACode]
			   ,[enginesize2]
			   ,[body_type]
			   ,[MFR_start_yr]
			   ,[number_doors])
		
				SELECT udl_dvla_id,
		  [caption_id]
		  ,[code]
		  ,[description]
		  ,[is_deleted]
		  ,[effective_date]
		  ,[vrm]
		  ,[fuel]
		  ,[transmission]
		  ,[make]
		  ,[model]
		  ,[engine_no]
		  ,[DVLA_BPD]
		  ,[SMMT_DPC]
		  ,[engine_size]
		  ,[DVLA_BPC]
		  ,[make_code]
		  ,[fuel_code]
		  ,[SMMT_DPD]
		  ,[colour_code]
		  ,[door_plan]
		  ,[colour]
		  ,[model_code]
		  ,[transmission_code]
		  ,[VIN]
		  ,[version]
		  ,[MFR_end_year]
		  ,[transmission2]
		  ,[model2]
		  ,[make2]
		  ,[fuel_code2]
		  ,[match_level]
		  ,[DVLACode]
		  ,[enginesize2]
		  ,[body_type]
		  ,[MFR_start_yr]
		  ,[number_doors]
	  FROM [dbo].[UDL_DVLA]')
					
		DROP TABLE dbo.UDL_DVLA
		
		EXECUTE sp_rename N'dbo.Tmp_UDL_DVLA', N'UDL_DVLA', 'OBJECT'

	END
END

-- *****************************************************************************  
-- * Author:       Anshul Jha
-- * Date:         14-08-2015
-- * Purpose:      NEW Table for External_Workflow_Config 
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'External_Workflow_Config')
BEGIN
 CREATE TABLE External_Workflow_Config
  (
  External_Workflow_Config_id INT  IDENTITY NOT NULL,
  Schedule_backGroundjob_ForFailure             TINYINT
  CONSTRAINT PK__External_Workflow_Config PRIMARY KEY (External_Workflow_Config_id)
  )
End
GO

-- *****************************************************************************  
-- * Author:       Anshul Jha
-- * Date:         14-08-2015
-- * Purpose:      NEW Table for holding the  Usergroup Configure in ExterWorkflowConfigurationTask
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'External_WorkFlow_usergroups')
BEGIN
 CREATE TABLE External_WorkFlow_usergroups
  (
  external_Workflow_usergroups_id INT  Identity NOT NULL,
  usergroup_id             INT
  CONSTRAINT PK__External_WorkFlow_usergroups PRIMARY KEY (external_Workflow_usergroups_id)
  )
EXEC DDLAddForeignKey @sTableName='External_WorkFlow_usergroups', @sColumnName1='usergroup_id', @sRefTableName = 'PMUser_Group',@sRefColumnName1='PMUser_Group_id'
End
GO

-- *****************************************************************************  
-- * Author:       Anshul Jha
-- * Date:         14-08-2015
-- * Purpose:      to implement External Work Flow Task for E5
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='PMWrk_task_parent_instance_cnt',@sColumnDefinition='Int'
GO
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='external_Workflow_id',@sColumnDefinition='uniqueidentifier'
Go
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO
EXEC DDLAddForeignKey @sTableName='PmWrk_task_Instance',@sColumnName1='PMWrk_task_parent_instance_cnt',@sRefTableName='PmWrk_task_Instance', @sRefColumnName1='pmwrk_task_instance_cnt' 
GO
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='ExternalTask_Category_Id',@sColumnDefinition='Int'
GO

-- *****************************************************************************
-- * Author:      Anshul Jha
-- * Date:         14-08-2015
-- * Purpose:     WPR13 /E5
-- *****************************************************************************

EXEC DDLAddColumn @sTableName='PMNav_Key', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO


EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='Is_system_lock',@sColumnDefinition='TinyInt'
GO


EXEC DDLAddColumn @sTableName='pmlock_last_unlock', @sColumnName ='Is_system_lock',@sColumnDefinition='TinyInt'
GO
-- *****************************************************************************

EXECUTE DDLAlterColumn 'Party_bank', 'account_holder_name', 'varchar(255) NULL', 1
GO
EXECUTE DDLAlterColumn 'Party_bank_history', 'account_holder_name', 'varchar(255) NULL', 1
GO
EXECUTE DDLAlterColumn 'PFPremiumFinance', 'BankAccountName', 'varchar(255) NULL', 1
GO
EXECUTE DDLAlterColumn 'PFMediaTypeHistory', 'BankAccountName', 'varchar(255) NULL', 1
GO
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'Claim', 'Insurer_Contact', 'varchar(255)  NULL'
GO

exec ddladdindex claim_payment , 'is_referred'

Go

-- *****************************************************************************
-- * Author:        Joginder Sharma
-- * Date:          24/04/2015
-- * Purpose:       RFC02-Log Claims Outside of Policy Period
-- *****************************************************************************
EXEC DDLAddColumn 'Risk_Type', 'Attach_Claim_Outside_Of_Policy_Period', 'TINYINT NULL'
GO


-- *****************************************************************************
-- * Author:      Yogender Singh
-- * Date:        26/09/2014
-- * Purpose:     4.1 paralleling
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='Background_Job', @sColumnName ='last_job_retry_time',@sColumnDefinition='datetime'
GO 

-- *****************************************************************************
-- * Author:   Sahil Ansari
-- * Date:     02/11/2015
-- * Purpose:   SEPA 
-- *****************************************************************************
EXEC DDLAddColumn 'Party_Bank', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'Party_Bank', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'PFPremiumFinance', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'PFPremiumFinance', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'pfmediatypehistory', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'pfmediatypehistory', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'Claim_Payment', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'Claim_Payment', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'CashListItem', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'CashListItem', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'BankAccount', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'BankAccount', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'Party_Bank_History', 'business_identifier_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'Party_Bank_History', 'international_bank_account_number', 'VARCHAR(50) NULL'
GO
-- *****************************************************************************
-- * Author:   Deepak Arora
-- * Date:     12/01/2016
-- * Purpose:   Index added based on the observations in Performance Lab 4.0
-- *****************************************************************************


--EXEC DDLADdINDEX 'Party_Bank', 'Account_id'
--GO
--EXEC DDLADdINDEX 'Party_Bank_history', 'Account_id'
--GO
--IF Exists( SELECT * FROM SYS.Indexes WHERE NAME LIKE 'IX_Claim__Claim_status_id__is_dirty')
--BEGIN
--	DROP INDEX Claim.IX_Claim__Claim_status_id__is_dirty
--	END
--CREATE NONCLUSTERED INDEX IX_Claim__Claim_status_id__is_dirty
--ON [dbo].[Claim] ([Claim_Status_id],[is_dirty])
--INCLUDE ([Claim_id])
--GO

--IF Not Exists ( select 1 From sys.indexes where name ='IX_tax_group_risk_cnt_COB')
--BEGIN 
--CREATE NONCLUSTERED INDEX IX_tax_group_risk_cnt_COB 
--    ON Peril(tax_group) Include (risk_cnt, class_of_business_id)
--END

--GO

--EXEC DDLAddIndex 'Doc_Folder','Folder_name'
--GO
--EXEC DDLDROPINDEX 'stats_folder','source_id'
--GO
--IF Not Exists ( select 1 From sys.indexes where name ='IX_Stata_folder_source_id_stats_folder_id')
--BEGIN 
--CREATE NONCLUSTERED INDEX IX_Stata_folder_source_id_stats_folder_id 
--    ON stats_folder(source_id) Include (stats_folder_id)
--END
-- *****************************************************************************
-- * Author:        Pk
-- * Date:          16/11/2015
-- * Purpose:       IH WPR-10 AND WPR 5
-- *****************************************************************************
EXEC DDLAddColumn 'User_Authorities', 'can_change_instalment_default_currency', 'TINYINT NULL'
GO 

EXEC DDLAddColumn 'PFPremiumFinance', 'use_trans_currency', 'TINYINT NULL'
GO 

EXEC DDLAddColumn 'PFRF', 'single_instalment_per_month', 'TINYINT NULL'
GO 
EXEC DDLAddColumn 'PFRF', 'first_instalment_align_with_day_in_month', 'TINYINT NULL'
GO 

EXEC DDLAddColumn 'PFPremiumFinance', 'CardHolderCountry', 'VARCHAR(225) NULL'
GO 


-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         12-07-2016
-- * Purpose:      Increased the size of ClientCode column in PFPremiumFinance 
-- *****************************************************************************
-- * Author:      Sahil Ansari
-- * Date:        26/09/2014
-- * Purpose:     WPR13 /E5
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='Background_Job', @sColumnName ='last_job_retry_time',@sColumnDefinition='datetime'
GO
-- *****************************************************************************
-- *****************************************************************************
-- * Author:  Samarjeet Singh
-- * Date:     07/01/2016
-- * Purpose:  EH008212 
-- * Remark :  Paralleled code from Arch_Dev against EH008212 (WI-11663)
-- *****************************************************************************

EXEC DDLAddColumn 'Document_Template_Group', 'document_template_sub_group_id', 'integer NULL'
GO

EXEC DDLAddForeignKey 
		@sTableName='Document_Template_Group', 
		@sColumnName1='document_template_sub_group_id', 
		@sRefTableName = 'document_template_sub_group',
		@sRefColumnName1='document_template_sub_group_id'
GO
-- *****************************************************************************
-- *****************************************************************************
-- * Author:  Navneet Kharwanda
-- * Date:     16/07/2018
-- * Purpose:  PM053435 
-- * Remark : Inncrease Length of PayeeName as Same as PayeeName of Claim_Payment and Claim_Receipt table 
-- *****************************************************************************
EXEC DDLAlterColumn @sTableName='CashListItem', @sColumnName ='Payment_Name',@sColumnDefinition='varchar(255)'
GO
-- *****************************************************************************
-- *****************************************************************************--  --*****************************************************************************
-- * Author:   Shivraj Rathor
-- * Date:     30 May 2016
-- * Purpose:  TFS-16265 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'Doc_Annotation', 'ann_text', 'VARCHAR(255)'
GO
EXEC DDLAddOrAlterColumn 'PFPremiumFinance', 'ClientCode', 'varchar(20) NULL'
Go

-- *****************************************************************************
-- * Author:        AMITA AGGARWAL
-- * Date:          17/10/2016
-- * Purpose:		Add table Correspondence_Type to PMLookup
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Correspondence_Type' AND xtype = 'U')
BEGIN
	CREATE TABLE Correspondence_Type
(
	Correspondence_Type_ID SMALLINT PRIMARY KEY IDENTITY,
	caption_id INT NOT NULL,
	code CHAR(10) NOT NULL,
	description VARCHAR(255) NOT NULL,
	is_deleted TINYINT NOT NULL,
	effective_date DATETIME NOT NULL
)

End
GO
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Correspondence_Type')
Begin
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'Correspondence_Type', 3, 1)
End
GO

-- *****************************************************************************
-- * Author:      Amrita Garg
-- * Date:        06/05/2017
-- * Purpose:     Add the column receives_client_correspondence
--				  Paralle wpr3.0 from BFM to IH
-- *****************************************************************************
EXEC DDLAddColumn 'Party_Agent', 'Receives_Client_Correspondence', 'TINYINT DEFAULT 0 NOT NULL'
GO

-- *****************************************************************************
-- * Author:      Amrita Garg
-- * Date:        06/05/2017
-- * Purpose:     Added the columns email_sub_template_code and email_attachment_template_code
--				  Paralle wpr3.0 from BFM to IH
-- *****************************************************************************
EXEC DDLAddColumn 'document_template', 'email_sub_template_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'document_template', 'email_attachment_template_code', 'VARCHAR(250) NULL'
GO


-- *****************************************************************************
-- * Author:      AMITA AGGARWAL
-- * Date:        09/11/2016
-- * Purpose:     Add the column correspondence_method_id
-- *****************************************************************************
EXEC DDLAddColumn 'Insurance_File', 'Correspondence_Type', 'Int NULL'
GO

EXEC DDLAddColumn 'Insurance_File', 'Default_Preferred_Correspondence', 'Int NULL'
GO

EXEC DDLAddColumn 'Insurance_File', 'Is_Agent_Correspondence', 'TINYINT Default 0 NOT NULL'
GO


-- *****************************************************************************  
-- * Author:       Anshul Jha
-- * Date:         06-11-2015
-- * Purpose:      NEW Table for External_Workflow_Config 
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'External_Workflow_Config')
BEGIN
 CREATE TABLE External_Workflow_Config
  (
  External_Workflow_Config_id INT  IDENTITY NOT NULL,
  Schedule_backGroundjob_ForFailure             TINYINT
  CONSTRAINT PK__External_Workflow_Config PRIMARY KEY (External_Workflow_Config_id)
  )
End
GO

-- *****************************************************************************
GO

    DECLARE @ColumnExists INT
    Declare @TableExists INT
    DECLARE @Table VARCHAR(250)
	SET @Table = 'product'
	EXEC @ColumnExists = DDLExistsColumn @table,'reset_reserves_at_reopen_claim'
	EXEC @TableExists = DDLExistsTable @table
	
	IF @ColumnExists = 0 AND @TableExists = 1
	BEGIN
		EXEC DDLAddColumn 'product', 'reset_reserves_at_reopen_claim', 'Tinyint NULL Default 0'
	END
GO

-- *****************************************************************************  
-- * Author:       Anshul Jha
-- * Date:         06-11-2015
-- * Purpose:      NEW Table for holding the  Usergroup Configure in ExterWorkflowConfigurationTask
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'External_WorkFlow_usergroups')
BEGIN
 CREATE TABLE External_WorkFlow_usergroups
  (
  external_Workflow_usergroups_id INT  Identity NOT NULL,
  usergroup_id             INT
  CONSTRAINT PK__External_WorkFlow_usergroups PRIMARY KEY (external_Workflow_usergroups_id)
  )
EXEC DDLAddForeignKey @sTableName='External_WorkFlow_usergroups', @sColumnName1='usergroup_id', @sRefTableName = 'PMUser_Group',@sRefColumnName1='PMUser_Group_id'
End
GO

-- *****************************************************************************  
-- * Author:       Anshul Jha
-- * Date:         06-11-2015
-- * Purpose:      to implement External Work Flow Task for E5
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='PMWrk_task_parent_instance_cnt',@sColumnDefinition='Int'
GO
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='external_Workflow_id',@sColumnDefinition='uniqueidentifier'
Go
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO
EXEC DDLAddForeignKey @sTableName='PmWrk_task_Instance',@sColumnName1='PMWrk_task_parent_instance_cnt',@sRefTableName='PmWrk_task_Instance', @sRefColumnName1='pmwrk_task_instance_cnt' 
GO
EXEC DDLAddColumn @sTableName='PMWrk_Task_Instance', @sColumnName ='ExternalTask_Category_Id',@sColumnDefinition='Int'
GO

-- *****************************************************************************
-- * Author:      Anshul Jha
-- * Date:         06-11-2015
-- * Purpose:     WPR13 /E5
-- *****************************************************************************

EXEC DDLAddColumn @sTableName='PMNav_Key', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO

EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='Is_system_lock',@sColumnDefinition='TinyInt'
GO

EXEC DDLAddColumn @sTableName='pmlock_last_unlock', @sColumnName ='Is_system_lock',@sColumnDefinition='TinyInt'
GO


-- *****************************************************************************
-- * Author:      Anshul Jha
-- * Date:        06-11-2015
-- * Purpose:     WPR13 /E5
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='Background_Job', @sColumnName ='last_job_retry_time',@sColumnDefinition='datetime'
GO

-- *****************************************************************************
-- * Author:      George Harris
-- * Date:        11-01-2019
-- * Purpose:     Removed the stats_folder_id as it is not used anymore TFS - Defect 43917
-- *****************************************************************************

IF EXISTS (SELECT * FROM SYS.indexes WHERE NAME='IX_Stata_folder_source_id_stats_folder_id')
DROP INDEX IX_Stata_folder_source_id_stats_folder_id ON Stats_folder
GO
IF EXISTS (SELECT * FROM SYS.indexes WHERE NAME='IX_Stats_Folder_source_id')
DROP INDEX IX_Stats_Folder_source_id ON Stats_folder
GO
GO
IF EXISTS (SELECT * FROM SYS.indexes WHERE NAME='IX_Stats_Folder_source_id_6886C')
DROP INDEX IX_Stats_Folder_source_id_6886C ON Stats_folder
GO
IF EXISTS (SELECT * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='Stats_folder' and COLUMN_NAME='Stats_Folder_ID')
ALTER TABLE Stats_folder
DROP Column Stats_Folder_ID
GO

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     12/12/2016
-- * Purpose:  Added Batch Description column in Batch Table
-- *****************************************************************************
EXEC DDLAddColumn 'Batch', 'Description', 'Varchar(255)'
GO

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     12/12/2016
-- * Purpose:  Added Batch Completed_Date column in Batch Table
-- *****************************************************************************
EXEC DDLAddColumn 'Batch', 'Completed_Date', 'DateTime'
GO

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     12/12/2016
-- * Purpose:  Added column to give access for View Batch Process Status
-- *****************************************************************************
EXEC DDLAddColumn 'User_Authorities', 'has_ViewBatchProcessStatus', 'TINYINT NULL'
GO

-- *****************************************************************************  
-- * Author:       Anshika Gupta
-- * Date:         15-12-2016
-- * Purpose:      COMPILED RULES WPR012
-- *****************************************************************************
EXEC DDLAddColumn 'GIS_Screen', 'risk_type_rule_set_type_id', 'int'
GO
EXEC DDLAddColumn 'GIS_Screen', 'file_name_Defaults', 'varchar(255) NULL'
GO 
EXEC DDLAddColumn 'GIS_Screen', 'file_name_Validation', 'varchar(255) NULL'
GO 
EXEC DDLAddColumn 'User_Authorities', 'allow_receipt_reversal', 'TINYINT NULL'
GO
EXEC DDLAddOrAlterColumn 'Rule_set', 'file_name', 'varchar(255)'
GO
EXEC DDLAddColumn 'Rule_set', 'risk_type_rule_set_type_id', 'int'
GO
EXEC DDLAddForeignKey @sTableName='Rule_set', @sColumnName1='risk_type_rule_set_type_id', @sRefTableName = 'risk_type_rule_set_type',@sRefColumnName1='risk_type_rule_set_type_id'
GO

EXEC DDLAddOrAlterColumn 'risk_type_rule_set_type', 'file_name', 'varchar(255)'
GO
-- *****************************************************************************
-- * Author:   Anshika Gupta
-- * Date:     21/12/2016
-- * Purpose:   
--			  
-- *****************************************************************************
IF NOT EXISTS(
    SELECT *
    FROM sys.columns
    WHERE Name      = N'Rule_type'
      AND Object_ID = Object_ID(N'Tax_group'))
BEGIN
    CREATE TABLE #TempTaxGroup
(
	tax_group_id	INT,
	caption_id	INT,
	description	VARCHAR(255),
	effective_date	DATETIME,
	is_deleted	TINYINT,
	code	VARCHAR(10),
	is_withholding_tax	TINYINT,
	advanced_tax_script	VARCHAR(50),
	is_coinsurer_multiple_tax_group	TINYINT,
	is_tax_amount_editable	TINYINT
)

	INSERT INTO #TempTaxGroup
	SELECT 
		tax_group_id,
		caption_id,
		description,
		effective_date,
		is_deleted,
		code,
		is_withholding_tax,
		advanced_tax_script,
		is_coinsurer_multiple_tax_group,
		is_tax_amount_editable
	FROM Tax_Group

	EXEC DDLDROPColumn 'Tax_group','is_coinsurer_multiple_tax_group'
	EXEC DDLDROPColumn 'Tax_group','is_tax_amount_editable'
	EXEC DDLDROPColumn 'Tax_group','advanced_tax_script'

	EXEC DDLADDColumn 'Tax_group','Rule_Type','int'
	EXEC DDLADDForeignKey  @sTableName='Tax_group',@scolumnName1='Rule_type',@sRefTableName='risk_type_rule_set_type',@sRefColumnName1='risk_type_rule_set_type_id'

	EXEC DDLADDColumn 'Tax_group','advanced_tax_script','varchar(50)'
	EXEC DDLADDColumn 'Tax_group','is_coinsurer_multiple_tax_group','TINYINT'
	EXEC DDLADDColumn 'Tax_group','is_tax_amount_editable','TINYINT'

	UPDATE Tax_group
	SET Tax_group.advanced_tax_script = #TempTaxGroup.advanced_tax_script,
		Tax_group.is_coinsurer_multiple_tax_group = #TempTaxGroup.is_coinsurer_multiple_tax_group,
		Tax_group.is_tax_amount_editable = #TempTaxGroup.is_tax_amount_editable
	FROM Tax_group JOIN #TempTaxGroup 
	ON Tax_group.tax_group_id = #TempTaxGroup.tax_group_id

	DROP TABLE #TempTaxGroup
END
GO
-- *****************************************************************************
-- * Author:   Suman Anjna
-- * Date:     16/02/2017
-- * Purpose:  Sequence order correction for Rule Type column(Defect - 22215)
-- *****************************************************************************
IF ((SELECT ORDINAL_POSITION
    FROM INFORMATION_SCHEMA.COLUMNS
	WHERE TABLE_NAME = 'TAX_GROUP'
		AND COLUMN_NAME = 'Rule_Type') <> 8)
BEGIN
    CREATE TABLE #TempTaxGroup
	(
		tax_group_id	INT,
		caption_id	INT,
		description	VARCHAR(255),
		effective_date	DATETIME,
		is_deleted	TINYINT,
		code	VARCHAR(10),
		is_withholding_tax	TINYINT,
		advanced_tax_script	VARCHAR(50),
		rule_type int,
		is_coinsurer_multiple_tax_group	TINYINT,
		is_tax_amount_editable	TINYINT
	)

	INSERT INTO #TempTaxGroup
	SELECT 
		tax_group_id,
		caption_id,
		description,
		effective_date,
		is_deleted,
		code,
		is_withholding_tax,
		advanced_tax_script,
		Rule_Type,
		is_coinsurer_multiple_tax_group,
		is_tax_amount_editable
	FROM Tax_Group

	EXEC DDLDropForeignKey 'Tax_Group','Rule_Type'

	EXEC DDLDROPColumn 'Tax_group','is_tax_amount_editable'
	EXEC DDLDROPColumn 'Tax_group','is_coinsurer_multiple_tax_group'
	EXEC DDLDROPColumn 'Tax_group','Rule_Type'
	EXEC DDLDROPColumn 'Tax_group','advanced_tax_script'

	EXEC DDLADDColumn 'Tax_group','Rule_Type','int'
	EXEC DDLADDForeignKey  @sTableName='Tax_group',@scolumnName1='Rule_type',@sRefTableName='risk_type_rule_set_type',@sRefColumnName1='risk_type_rule_set_type_id'
	
	EXEC DDLADDColumn 'Tax_group','advanced_tax_script','varchar(50)'
	EXEC DDLADDColumn 'Tax_group','is_coinsurer_multiple_tax_group','TINYINT'
	EXEC DDLADDColumn 'Tax_group','is_tax_amount_editable','TINYINT'

	UPDATE Tax_group
	SET Tax_group.Rule_Type = #TempTaxGroup.rule_type,
		Tax_group.advanced_tax_script = #TempTaxGroup.advanced_tax_script,
		Tax_group.is_coinsurer_multiple_tax_group = #TempTaxGroup.is_coinsurer_multiple_tax_group,
		Tax_group.is_tax_amount_editable = #TempTaxGroup.is_tax_amount_editable
	FROM Tax_group JOIN #TempTaxGroup 
	ON Tax_group.tax_group_id = #TempTaxGroup.tax_group_id

	DROP TABLE #TempTaxGroup
END
GO
-- *****************************************************************************
-- * End of script
-- *****************************************************************************
-- *****************************************************************************
-- * Author:   Anshika Gupta
-- * Date:     21/12/2016
-- * Purpose:  Query for existing cutomers to pick compiled rule value 
--			   in Screen designer
-- *****************************************************************************
IF EXISTS(
    SELECT 1
    FROM sys.columns
    WHERE Name      = N'compiled_rules'
      AND Object_ID = Object_ID(N'GIS_Data_Model'))
	  BEGIN
	  CREATE TABLE #tempGISDataModel
		(code	VARCHAR(25),
		compiled_rules TINYINT DEFAULT 0 NOT NULL
		)

		INSERT into #tempGISDataModel EXECUTE  sp_executesql N'SELECT 		code,		compiled_rules		FROM GIS_Data_Model '

		UPDATE GIS_Screen
		SET GIS_Screen.risk_type_rule_set_type_id = CASE WHEN #tempGISDataModel.compiled_rules = 1 THEN 3 ELSE 1 END,
			GIS_Screen.file_name_Defaults = CASE WHEN #tempGISDataModel.compiled_rules = 1 THEN  RTRIM(GIS_Screen.code) + '.' + RTRIM(GIS_Screen.code) + 'Defaults' ELSE RTRIM(GIS_Screen.code) + 'Def.Rul' END,
			GIS_Screen.file_name_Validation = CASE WHEN #tempGISDataModel.compiled_rules = 1 THEN  RTRIM(GIS_Screen.code) + '.' + RTRIM(GIS_Screen.code) + 'Validation' ELSE RTRIM(GIS_Screen.code) + 'Val.Rul' END
		FROM GIS_Screen JOIN #tempGISDataModel 
		ON GIS_Screen.code = #tempGISDataModel.code
		
		DROP TABLE #tempGISDataModel

		EXEC DDLDropConstraint 'GIS_Data_Model','compiled_rules'
		EXEC DDLDropColumn 'GIS_Data_Model', 'compiled_rules'
		
		UPDATE risk_type_rule_set
		SET file_name = RTRIM(Code) + '.Rating'
		WHERE risk_type_rule_set_type_id = 3

	  END
	GO

-- *****************************************************************************  
-- * Author:       Anshika Gupta
-- * Date:         06-22-2017
-- * Purpose:      CCM INTEGRATION WPR04
-- *****************************************************************************
EXEC DDLADDColumn 'system_option_configuration','parent_name','VARCHAR(50)'
EXEC DDLADDColumn 'system_option_configuration','control_name','VARCHAR(50)'
EXEC DDLADDCOLUMN 'Document_Template', 'CCMDocumentTemplate', 'VARCHAR(255)'
EXEC DDLADDCOLUMN 'Document_Template', 'DocumentFieldsetFieldList', 'VARCHAR(MAX)'
EXEC DDLADDCOLUMN 'Document_Template', 'CCMRefreshDate', 'DateTime'
EXEC DDLADDCOLUMN 'wp_fields', 'Table_Name', 'VARCHAR(255)'
EXEC DDLADDCOLUMN 'wp_fields', 'DataStructure_Name', 'VARCHAR(255)'

IF NOT EXISTS(Select 1 from sys.types WHERE Name = 'CCMWPFields')
BEGIN
CREATE TYPE CCMWPFields AS TABLE  
(
	Table_Name VARCHAR(255),
	Column_name VARCHAR(255)
)
END

IF NOT EXISTS(Select * from sys.tables WHERE Name = 'CCMStatus')
BEGIN
CREATE TABLE CCMStatus
(
	CCMStatus_id INT PRIMARY KEY,
	caption_id INT,
	code CHAR(10),
	description VARCHAR(255),
	is_deleted TINYINT,
	effective_date DATETIME
)
END
-- *****************************************************************************  
--* End of script
-- *****************************************************************************
GO
-- *****************************************************************************
-- * Author:       Aditya Pratap Singh
-- * Date:         13-02-2017
-- * Purpose:      NEW Table for pie_History 
-- *****************************************************************************
IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'PH_Id' AND Object_ID = Object_ID(N'pie_History'))
BEGIN
DROP TABLE Pie_History
END
GO
IF NOT EXISTS(SELECT 1 FROM SYS.Tables WHERE NAME LIKE 'pie_History')
BEGIN
CREATE TABLE [dbo].[pie_History](
	[Pie_History_Id] [int] IDENTITY(1,1) NOT NULL,
	[DataModels] [varchar](500) NULL,
	[Udls] [varchar](500) NULL,
	[AdditionalOptions] [varchar](500) NULL,
	[CreatedTime] [datetime] NOT NULL DEFAULT (getdate()),
	[IsImport] [dbo].[Boolean] NOT NULL,
	[IsExport] [dbo].[Boolean] NOT NULL,
	[PMUser_Id] [varchar](500) NOT NULL,
	[Comment] [varchar](500) NULL,
	[MultipleServersPath] [varchar](500) NULL
)
END
GO

-- *****************************************************************************
-- * Author:       Shipra Gupta
-- * Date:         08-03-2017
-- * Purpose:      NEW Type to save Product's branches and causations 
-- *****************************************************************************
IF NOT EXISTS(Select 1 from sys.types WHERE Name = 'TPRODUCT')
BEGIN
Create TYPE TPRODUCT AS TABLE  
(
	product_id Integer,
	Linked_id Integer
)
END
-- *****************************************************************************  
--* End of script
-- *****************************************************************************
GO


-- *****************************************************************************
-- * Author:      Hardeep Singh
-- * Date:        11/03/2016
-- * Purpose:     to increase the column datatype size
-- *****************************************************************************
Declare @Desclen as Int

select @Desclen = character_maximum_length  from information_schema.columns  
where table_name = 'claim' and column_name='Description' and DATA_TYPE = 'varchar'

IF(@Desclen<1000)
	BEGIN
		 ALTER TABLE [claim] ALTER COLUMN [Description] varchar(1000) 
	END
Go
-- *****************************************************************************
-- * Author:      Samarjeet Singh
-- * Date:        02/05/2016
-- * Purpose:     to increase the column datatype size
-- *****************************************************************************
Declare @Insurer_Con_len as Int

select @Insurer_Con_len = character_maximum_length  from information_schema.columns  
where table_name = 'claim' and column_name='Insurer_Contact' and DATA_TYPE = 'varchar'

IF(@Insurer_Con_len<255)
	BEGIN
		 ALTER TABLE [claim] ALTER COLUMN Insurer_Contact varchar(255) 
	END
Go


-- *****************************************************************************  
-- * Author:       Nishchal Upadhayay
-- * Date:         18-02-2014
-- * Purpose:      Add Missing Codes to ACTNUMBER_GROUP
-- *****************************************************************************

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF37')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 37 ,	'DOCREF37'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF38')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 38 ,	'DOCREF38'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF39')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 39 ,	'DOCREF39'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF40')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 40 ,	'DOCREF40'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF41')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 41 ,	'DOCREF41'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF42')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 42 ,	'DOCREF42'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF43')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 43 ,	'DOCREF43'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF44')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 44 ,	'DOCREF44'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF45')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 45 ,	'DOCREF45'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF46')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 46 ,	'DOCREF46'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF47')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 47 ,	'DOCREF47'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF48')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 48 ,	'DOCREF48'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF50')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 50 ,	'DOCREF50'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF51')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 51 ,	'DOCREF51'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF52')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 52 ,	'DOCREF52'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF53')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 53 ,	'DOCREF53'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF54')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 54 ,	'DOCREF54'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_GROUP WHERE code = 'DOCREF55')
BEGIN
INSERT INTO ACTNumber_Group (actnumber_group_id ,code ,caption_id ,description, is_reset_yearly ,is_deleted ,effective_date)
VALUES( 55 ,	'DOCREF55'  ,	207 ,	'Document Reference' ,	0 ,	0 ,	'2002-10-08 00:00:00.000' )
END
GO

-- *****************************************************************************  
-- * Author:       Nishchal Upadhayay
-- * Date:         18-02-2014
-- * Purpose:      Add Missing Codes to ACTNUMBER_RANGE
-- *****************************************************************************
IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IDR')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (37 ,	'IDR' ,      	37 ,	'IDR'    ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'ICR')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (38 ,	'ICR' ,      	38 ,	'ICR'    )     
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'ICA')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (39 ,	'ICA' ,     	39 ,	'ICA'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'CLO')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (40 ,	'CLO' ,      	40 ,	'CLO'    )    
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'CLA')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (41 ,	'CLA' ,      	41 ,	'CLA'    )  
END
GO
 
IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IND')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (42 ,	'IND' ,      	42 ,	'IND'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'INC')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (43 ,	'INC' ,      	43 ,	'INC'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IED')
BEGIN  
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (44 ,	'IED' ,      	44 ,	'IED'    ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IEC')
BEGIN   
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (45 ,	'IEC' ,      	45 ,	'IEC'    )    
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IRD')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (46 ,	'IRD' ,      	46 ,	'IRD'    )    
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IRC')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (47 ,	'IRC' ,      	47 ,	'IRC'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'BJN')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (48 ,	'BJN' ,      	48 ,	'BJN'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SDD')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (50 ,	'SDD' ,     	50 ,	'SDD'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SPD')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (51 ,	'SPD' ,      	51 ,	'SPD'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SID')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (52 ,	'SID' ,      	52 ,	'SID'    )   
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'SIC')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (53 ,	'SIC' ,      	53 ,	'SIC'    ) 
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IID')
BEGIN
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)  
VALUES (54 ,	'IID' ,      	54 ,	'IID'    )  
END
GO

IF NOT EXISTS (SELECT code FROM ACTNUMBER_RANGE WHERE code = 'IIC')
BEGIN 
INSERT INTO  ACTNumber_Range  (actnumber_range_id ,code ,actnumber_group_id,description)
VALUES (55 ,	'IIC' ,      	55 ,	'IIC'    )
END
GO   


-- *****************************************************************************  
-- * Author:      Ashish Sachdeva
-- * Date:        08 Nov 2013
-- * Purpose:     RND003 - DMEToSharePoint
-- *****************************************************************************
EXEC DDLAddColumn 'Doc_Document','DME_Migration_Status_id','INT'
EXEC DDLAddColumn 'Doc_Document','migration_id','INT'
GO
IF Not Exists (Select 1 From Batch_Type Where Code = 'DMEMIG')
BEGIN
    Declare @lCaptionID integer
    Declare @Batch_Type_Id integer
    
    Execute spu_pm_caption_id_return 1, 'DMEMIG', @lCaptionID output
    
    Select @Batch_Type_Id = Max(Batch_type_Id)+1 From Batch_Type
    
    Insert into Batch_type 
    (
        Batch_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code
    )
    Values
    (
        @Batch_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'DME Migration',
        'DMEMIG'
    )
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'DME_Migration_Status'
IF @bExists = 0 BEGIN
CREATE TABLE DME_Migration_Status(
[DME_Migration_Status_id]			INT			NOT NULL  IDENTITY(1,1),
[code]						Varchar(20)		NOT NULL,
[description]					Varchar(255)		NOT NULL,
[caption_id]					Int			NOT NULL,
[effective_date]				DATETIME		NOT NULL,
[is_deleted]					TinyInt			NOT NULL
)

-- Add Primary Key
EXEC DDLADDPrimaryKey 'DME_Migration_Status', 'DME_Migration_Status_id'

IF NOT EXISTS (SELECT lookup_table_name 
FROM PMProduct_Lookup 
WHERE lookup_table_name = 'DME_Migration_Status')
BEGIN
 INSERT INTO PMProduct_Lookup
 (pmproduct_id, lookup_table_name, edit_privilege_level, 
 is_generic_maintenance, linked_data_mandatory)
 VALUES       (2, 'DME_Migration_Status', 3, 0, 0)
END


Exec DDLAddForeignKey	
@sTableName = 'Doc_Document', 
@sColumnName1 = 'dme_migration_status_id', 
@sRefTableName = 'DME_Migration_Status'
END
GO

-- *****************************************************************************
-- * Author:      Swati Saxena
-- * Date:        09 Nov 2015
-- * Purpose:     CAPRICORN CR040, Locking
-- *****************************************************************************
EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='lock3_value',@sColumnDefinition='varchar(250)'
GO
EXEC DDLAddColumn @sTableName='PMLock', @sColumnName ='is_exclusive_lock',@sColumnDefinition='Int NOT NULL Default 0'
GO
-- *****************************************************************************
-- * Author:   Keni Christina
-- * Date:     10-03-2015
-- * Purpose:  PM041410 - To avoid truncating characters of Client Name
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'stats_folder', 'insurance_holder_name', 'VARCHAR(255)'
GO

-- *****************************************************************************
-- * Author:   Ramesh Kumar
-- * Date:     15-05-2015
-- * Purpose:  PM042183 - field limited in length
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem', 'their_ref', 'VARCHAR(100)'
GO


IF NOT EXISTS( SELECT 1 FROM SYS.Types WHERE NAME LIKE 'TPMLock')
BEGIN
CREATE TYPE TPMLock AS TABLE
(
	[lock_name] [char](30) NOT NULL,
	[lock_value] [int] NOT NULL
)
END
GO
EXEC DDLAddColumn @sTableName='PMNav_Key', @sColumnName ='Is_External_WorkItem',@sColumnDefinition='Int'
GO

EXEC DDLAddColumn @sTableName='risk_type_rule_set', @sColumnName ='pre_pre_rule',@sColumnDefinition='tinyint'
GO
-- *****************************************************************************
-- * Author:   Suman Anjna
-- * Date:     19/06/2017
-- * Purpose:  Defect - 24980
-- *****************************************************************************
IF COLUMNPROPERTY( OBJECT_ID('Document_Template'),'Code','PRECISION') < 20
BEGIN
	EXEC DDLDropIndex @sTableName = Document_Template,	@sColumnName1 = Code, @bQuiet = NULL
	EXEC DDLAlterColumn @sTableName='Document_Template', @sColumnName ='Code',@sColumnDefinition='char(20) NOT NULL'
	EXEC DDLAddIndex @sTableName = Document_Template, @sColumnName1 = Code,	@bQuiet = NULL
END
GO
-- *****************************************************************************  
-- * Author:       Anurag Gupta
-- * Date:         30/05/2017
-- * Purpose:      Add Three new Fields to Product Table
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'Product', 'is_reserves_read_only', 'TINYINT NULL'
GO
EXEC DDLAddOrAlterColumn 'Product', 'is_recoveries_read_only', 'TINYINT NULL'
GO
EXEC DDLAddOrAlterColumn 'Product', 'is_payments_read_only', 'TINYINT NULL'
GO
-- *****************************************************************************
-- * Author:       TARIQ RASHID
-- * Date:         30/12/2015
-- * Purpose:  Temporary Data Holder between SPS
-- *****************************************************************************
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'tblTaxBandInfo'
IF @bExists = 0
BEGIN
	CREATE TABLE tblTaxBandInfo
	( ReserveID INT,
	TaxBandID INT,
	Rate FLOAT,
	IsValue TINYINT,
	ClassOfBusinessID INT,
	TaxAmount CURRENCY)  
END
GO

-- *****************************************************************************
-- * Author:   Suman Anjna
-- * Date:     19/06/2017
-- * Purpose:  Defect - 24980
-- *****************************************************************************
IF COLUMNPROPERTY( OBJECT_ID('Document_Template'),'Code','PRECISION') < 20
BEGIN
	EXEC DDLDropIndex @sTableName = Document_Template,	@sColumnName1 = Code, @bQuiet = NULL
	EXEC DDLAlterColumn @sTableName='Document_Template', @sColumnName ='Code',@sColumnDefinition='char(20) NOT NULL'
	EXEC DDLAddIndex @sTableName = Document_Template, @sColumnName1 = Code,	@bQuiet = NULL
END
-- *****************************************************************************  
-- * Author:      Aditya Pratap Singh
-- * Date:        19 June 2017
-- * Purpose:     TUG002 - TowerGate AIUA Event Log and Incoming Emails
-- *****************************************************************************

GO
IF Not Exists (Select 1 From event_type Where Code = 'ExtDoc')
BEGIN
    Declare @lCaptionID integer
    Declare @Event_Type_Id integer
    Declare @Event_Type_Group_Id integer

    Execute spu_pm_caption_id_return 1, 'ExtDoc', @lCaptionID output
    
    Select @Event_Type_Id = Max(Event_type_Id)+1 From event_type
    Select @Event_Type_Group_Id = event_type_group_id  From event_type_group where code='DOCUMENTS'

    Insert into Event_type 
    (
        Event_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code,
              event_type_group_id
    )
    Values
    (
        @Event_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'External Document Stored',
        'ExtDoc',
              @Event_Type_Group_Id
    )
END
GO


-- *****************************************************************************  
-- * Author:      Aditya Pratap Singh
-- * Date:        19 June 2017
-- * Purpose:     TUG002 - TowerGate AIUA Event Log and Incoming Emails
-- *****************************************************************************

GO
IF Not Exists (Select 1 From event_type Where Code = 'EMailSent')
BEGIN
    Declare @lCaptionID integer
    Declare @Event_Type_Id integer
    Declare @Event_Type_Group_Id integer

    Execute spu_pm_caption_id_return 1, 'EMailSent', @lCaptionID output
    
    Select @Event_Type_Id = Max(Event_type_Id)+1 From event_type
    Select @Event_Type_Group_Id = event_type_group_id  From event_type_group where code='MAILSHOTS'

    Insert into Event_type 
    (
        Event_type_id,
        Caption_id,
        is_deleted,
        effective_date,
        Description,
        Code,
              event_type_group_id
    )
    Values
    (
        @Event_Type_Id,
        @lCaptionID,
        0,
        GetDate(),
        'Email Sent',
        'EMailSent',
              @Event_Type_Group_Id
    )
END
GO

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     20/06/2017
-- * Purpose:  TUG002 - TowerGate AIUA Event Log and Incoming Emails
-- *****************************************************************************
EXEC DDLAddColumn 'Event_Log', 'Document_Path', 'Varchar(255)'
GO

-- *****************************************************************************
-- * Author:   Preeti
-- * Date:     28/06/2017
-- * Purpose:  set old_policy_number column lenght same as insurance_ref column
-- *****************************************************************************
EXECUTE DDLAlterColumn 'insurance_file','old_policy_number','varchar(30)'
GO

-- *****************************************************************************
-- * Author:   Anshika Gupta
-- * Date:     06/07/2017
-- * Purpose:  WPR005 Instalment Paralleling
-- *****************************************************************************
EXECUTE DDLAlterColumn @sTableName='PFPremiumFinance', @sColumnName ='ClientCode',@sColumnDefinition='varchar(20) NULL'
GO
EXECUTE DDLAddColumn 'User_Authorities','can_update_instalment_status','Tinyint NOT NULL Default 0'
GO
EXECUTE DDLADDCOLUMN 'PFScheme','Spread_subagent_Commission','Tinyint NULL'
GO

-- *****************************************************************************
-- * Author:   Suman Anjna
-- * Date:     July 2017
-- * Purpose:  MID 2
-- *****************************************************************************
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME = 'Supplier_Type')
BEGIN
    CREATE TABLE Supplier_Type(
	[Supplier_Type_id] INT IDENTITY(1,1),
	[code] Varchar(20) NOT NULL,
	[description] Varchar(255) NOT NULL,
	[caption_id] Int NOT NULL,
	[effective_date] Datetime NOT NULL,
	[is_deleted] Tinyint NOT NULL 
	)

EXEC DDLAddPrimaryKey 'Supplier_Type', 'Supplier_Type_id'
END
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME = 'MID_Rule')
BEGIN
    CREATE TABLE MID_Rule(
	[MID_Rule_id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Effective_Date] [date] NOT NULL,
	[Start_Date] [date] NOT NULL,
	[Expiry_Date] [date] NULL,
	[MID_Type] [varchar](20) NOT NULL,
	[Supplier_Type_id] [int] NOT NULL,
	[Supplier_id] [int] NOT NULL,
	[Insurer_id] [int] NOT NULL,
	[Delegated_Authority_id] [int] NULL,
	[Site_Number] [int] NULL,
	[DA_Branch_id] VARCHAR(20) NULL,
	[PMUser_Group_id] [int] NOT NULL,
	[PMwrk_Task_Group_id] [int] NOT NULL,
	[FileName] [varchar](255) NOT NULL,
	[Test_Indicator] [bit] NOT NULL,
	[File_Seq_Num_Start] VARCHAR(6) NOT NULL,
	[Current_File_Seq_Num] VARCHAR(6) NOT NULL,
	[Is_Deleted] [bit] NOT NULL,
	[Source_id] [int] NOT NULL	
	)
	
	EXEC DDLAddPrimaryKey   'MID_Rule', 'MID_Rule_id'

	EXEC DDLAddForeignKey	'MID_Rule', 'Supplier_Type_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL,
							'Supplier_Type','Supplier_Type_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL
							
	EXEC DDLAddForeignKey	'MID_Rule', 'PMUser_Group_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL,
							'PMUser_Group','PMUser_Group_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL
							
	EXEC DDLAddForeignKey	'MID_Rule', 'PMwrk_Task_Group_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL,
							'PMWrk_Task_Group','PMwrk_Task_Group_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL
							
	EXEC DDLAddForeignKey	'MID_Rule', 'Source_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL,
							'Source','Source_id', NULL,NULL,NULL,NULL,NULL,NULL,NULL
		
END
ELSE	
BEGIN
		EXEC DDLDropColumn 'MID_Rule','DA_Branch_id'
END
GO				
	EXECUTE DDLADDCOLUMN 'MID_Policy','MID_Type','varchar(4)'
	EXECUTE DDLADDCOLUMN 'MID_Vehicle','permitted_drivers','varchar(2)'
	EXECUTE DDLADDCOLUMN 'MID_Vehicle','class_use','varchar(3)'
	
GO

	EXECUTE DDLADDCOLUMN 'PFScheme','business_identifier_code_mandatory','Tinyint NULL'
GO
	EXECUTE DDLADDCOLUMN 'PFScheme','international_bank_account_number_mandatory','Tinyint NULL'
GO


-- *****************************************************************************
-- * Author:   Preeti
-- * Date:     28/06/2017
-- * Purpose:  set old_policy_number column lenght same as insurance_ref column
-- *****************************************************************************
EXECUTE DDLAlterColumn 'event_insurance_file','old_policy_number','varchar(30)'
GO


-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     15/11/2017
-- * Purpose:  Added Is_Deposit_Override_Allowed column in PFRF Table
-- *****************************************************************************
EXEC DDLAddColumn 'PFRF', 'is_deposit_override_allowed', 'TINYINT DEFAULT 1 NOT NULL'

GO
-- * Author:   Shivraj Rathor
-- * Date:     27/11/2017
-- * Purpose:  Added apply_fee_percentages_to_fees and apply_fee_percentages_to_taxes column in PFRF Table

-- *****************************************************************************

EXEC DDLAddColumn 'PFRF', 'apply_fee_percentages_to_fees', 'TINYINT DEFAULT 1 NOT NULL'
EXEC DDLAddColumn 'PFRF', 'apply_fee_percentages_to_taxes', 'TINYINT DEFAULT 1 NOT NULL'

GO 

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     15/12/2017
-- * Purpose:  Added Is_Valid_For_Instalments column in Write_Off_Reason Table
-- *****************************************************************************
EXEC DDLAddColumn 'Write_Off_Reason', 'is_valid_for_instalments', 'TINYINT'
GO

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     21/12/2017
-- * Purpose:  Added Write_Off_Reason_ID column in PFInstalments Table
-- *****************************************************************************
EXEC DDLAddColumn 'PFInstalments', 'write_off_reason_id', 'INTEGER NULL'
GO


-- *****************************************************************************
-- * Author:        Deepak Arora
-- * Date:          29/12/2017
-- * Purpose:
-- *                Add table to PMLookup
-- *****************************************************************************
If NOT EXISTS(SELECT NULL FROM SYS.OBJECTS WHERE NAME='Association_Type')
BEGIN
    CREATE TABLE Association_Type
    (
        Association_Type_id 	INT 		IDENTITY,
        caption_id 				INT 		NOT NULL,
        code 					VARCHAR(10)	NOT NULL,
        description 			VARCHAR(255) 	NOT NULL,
		is_deleted 				TINYINT 	NOT NULL,
        effective_date 			DATETIME 	NOT NULL
        CONSTRAINT PK__Association_Type_Association_Type_id  PRIMARY KEY CLUSTERED (Association_Type_id)  
    )
End

GO
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Insurance_file_associates'
IF @bExists = 0
BEGIN
    CREATE TABLE Insurance_file_associates
	(
        Insurance_file_associates_cnt	INT IDENTITY,
        Insurance_file_cnt              INT NOT NULL,
        Party_cnt						INT NOT NULL,
        Association_type_id				INT NOT NULL,
        Association_detail				VARCHAR(250),
        date_attached					DATE,
        date_removed					DATE,
        Is_Deleted                      TINYINT,
        is_AddUnConfirmed				TINYINT NULL,    --Holds the UnConfirmed Status of the Added Policy Associate for the current policy version
        is_DelUnConfirmed				TINYINT NULL	 -- Holds the UnConfirmed Status of the Deleted Policy Associate for the current policy version	
        CONSTRAINT PK__Insurance_file_associates_cnt PRIMARY KEY CLUSTERED (Insurance_file_associates_cnt)
		
	)
END
Go
EXEC DDLAddForeignKey @sTableName='Insurance_file_associates', @sColumnName1='Insurance_file_cnt',@sRefTableName='Insurance_file', @sRefColumnName1='Insurance_file_cnt' 
EXEC DDLAddForeignKey @sTableName='Insurance_file_associates', @sColumnName1='Party_cnt', @sRefTableName = 'Party',@sRefColumnName1='Party_cnt'
EXEC DDLAddForeignKey @sTableName='Insurance_file_associates', @sColumnName1='Association_type_id', @sRefTableName = 'Association_Type',@sRefColumnName1='Association_Type_id'
Go
EXEC DDLAddIndex 'Insurance_file_associates','Party_cnt'
GO

-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     21/12/2017
-- * Purpose:  Added Write_Off_Transdetail_ID column in PFInstalments Table
-- *****************************************************************************
EXEC DDLAddColumn 'PFInstalments', 'write_off_transdetail_id', 'INTEGER NULL'
GO
-- *****************************************************************************
-- * Author:   Aditya Pratap Singh
-- * Date:     21/12/2017
-- * Purpose:  Set Write_Off_Transdetail_ID column as Foreign Key in PFInstalments Table
-- *****************************************************************************
EXEC DDLAddForeignKey @sTableName='PFInstalments', @sColumnName1='write_off_transdetail_id', @sRefTableName = 'TransDetail',@sRefColumnName1='TransDetail_Id'
GO
-- *****************************************************************************  
-- * Author:       Samar Jeet
-- * Date:         14-Feb-2018
-- * Purpose:      Added columns in Address table against WPR-22
-- *****************************************************************************
EXEC DDLAddColumn  'Address', 'address5', 'varchar(60) NULL' -- Property Description
GO
EXEC DDLAlterColumn @sTableName='Event_Public_Text', @sColumnName ='text_line',@sColumnDefinition='varchar(7500)'

EXEC DDLAddColumn  'Address', 'address6', 'varchar(60) NULL' -- GNAFID
GO

EXEC DDLAddColumn  'Address', 'address7', 'varchar(60) NULL' -- dpid
GO

EXEC DDLAddColumn  'Address', 'address8', 'varchar(60) NULL' -- dpid barcode
GO

EXEC DDLAddColumn  'Address', 'address9', 'varchar(60) NULL' -- latitiude
GO

EXEC DDLAddColumn  'Address', 'address10', 'varchar(60) NULL' -- longitude
GO
-- *****************************************************************************  
-- * Author:       Samar Jeet
-- * Date:         30-March-2018
-- * Purpose:     Increased the length of EventType required by Product Manager
-- *****************************************************************************
EXEC DDLAlterColumn @sTableName='Event_Public_Text', @sColumnName ='text_line',@sColumnDefinition='varchar(7500)'
GO
IF EXISTS(
    SELECT 1
    FROM sys.columns
    WHERE Name      = N'is_Only_Valid_for_Instalment'
      AND Object_ID = Object_ID(N'Write_Off_Reason'))
	  BEGIN
	   DECLARE @sSQL VARCHAR(100)
	   SET @sSQL='UPDATE Write_Off_Reason
		SET is_valid_for_instalments = is_Only_Valid_for_Instalment'

		EXEC(@sSQL)
		
		EXEC DDLDropConstraint 'Write_Off_Reason','is_Only_Valid_for_Instalment'
		EXEC DDLDropColumn 'Write_Off_Reason', 'is_Only_Valid_for_Instalment'
	  END
GO
--Start - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)

-- *****************************************************************************
-- * Author:   Amit Tyagi
-- * Date:     27-03-2019
-- * Purpose:  WPR05 paralleling WPR27
-- 

-- *****************************************************************************
-- * Sub Agent Commission Spread
-- * Author: Mohd Sajid
-- * Date: 07/09/2020
-- *****************************************************************************
EXEC DDLAddColumn 'PFScheme','spread_subagent_commission','Tinyint NULL'
GO
EXEC DDLAddColumn 'PFScheme','commission_subagent_suspense_account_id','Int NULL'
GO
EXEC DDLAddForeignKey 'PFScheme', 'commission_subagent_suspense_account_id', @sRefTableName = 'account', @sRefColumnName1='account_id'
GO

-- *****************************************************************************
-- * PM058643 - QBE 5 Changes
-- * Author: Rahul Kumar Gupta
-- * Date: 31/07/2020
-- *****************************************************************************

EXEC DDLAddColumn 'Product', 'Quote_all_risk_NB', 'TINYINT NULL'
GO 

EXEC DDLAddColumn 'Product', 'Quote_all_risk_MTC', 'TINYINT NULL'
GO 

EXEC DDLAddColumn 'Product', 'Quote_all_risk_MTA', 'TINYINT NULL'
GO 

EXEC DDLAddColumn 'Product', 'Quote_all_risk_RENEWAL', 'TINYINT NULL'
GO 


-- *****************************************************************************
-- *****************************************************************************
-- *****************************************************************************--  --*****************************************************************************
-- * Author:   Ankur Anand
-- * Date:     09 September 2019
-- * Purpose:  TFS-50578 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem', 'address1', 'VARCHAR(60)'
GO
-- *****************************************************************************
-- *****************************************************************************--  --*****************************************************************************
-- * Author:   Ankur Anand
-- * Date:     09 September 2019
-- * Purpose:  TFS-50578 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem', 'address2', 'VARCHAR(60)'
GO
-- *****************************************************************************
-- *****************************************************************************--  --*****************************************************************************
-- * Author:   Ankur Anand
-- * Date:     09 September 2019
-- * Purpose:  TFS-50578 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem', 'address3', 'VARCHAR(60)'
GO
-- *****************************************************************************
-- *****************************************************************************--  --*****************************************************************************
-- * Author:   Ankur Anand
-- * Date:     09 September 2019
-- * Purpose:  TFS-50578 
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem', 'address4', 'VARCHAR(60)'
GO
Go
EXEC DDLAddOrAlterColumn 'claim', 'Client_tel_no','varchar(255)'
Go
EXEC DDLAddOrAlterColumn 'claim', 'Client_fax_no','varchar(255)'
Go
EXEC DDLAddOrAlterColumn 'claim', 'Client_mobile_no','varchar(255)'
Go
EXEC DDLAddOrAlterColumn 'claim', 'Client_email','varchar(255)'
Go
EXEC DDLAddOrAlterColumn 'claim', 'insurer_tel_no','varchar(255)'
Go
EXEC DDLAddOrAlterColumn 'claim', 'insurer_fax_no','varchar(255)'
Go
EXEC DDLAddOrAlterColumn 'claim', 'insurer_email','varchar(255)'

-- *****************************************************************************
-- * Author:   Amit Tyagi
-- * Date:     24-04-2019
-- * Purpose:  WPR05 paralleling WPR27
-- *****************************************************************************

EXEC DDLAddColumn 'Product', 'Auto_Renew_BDMPolicy', 'TINYINT NULL'
GO 



	
EXECUTE DDLADDCOLUMN 'MID_Policy','Policyholder_Name', 'varchar(385)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Policyholder_DOB','datetime'
EXECUTE DDLADDCOLUMN 'MID_Policy','Address1','VARCHAR(60)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Address2','VARCHAR(60)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Address3','VARCHAR(60)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Address4','VARCHAR(60)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Address5','VARCHAR(40)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Address6','VARCHAR(40)'
EXECUTE DDLADDCOLUMN 'MID_Policy','PostCode','VARCHAR(20)'
EXECUTE DDLADDCOLUMN 'MID_Policy','Policyholder_DrivingOtherVehicles','varchar(1)'
EXECUTE DDLADDCOLUMN 'MID_Policy','ExcludeFromExport','bit DEFAULT 0'
EXECUTE DDLADDCOLUMN 'MID_Vehicle','Cover_Type','varchar(2)'
EXECUTE DDLADDCOLUMN 'MID_Vehicle','VIN','varchar(20)'
GO

-- *****************************************************************************
-- * Author:       Sandeep Kumar
-- * Date:          20/05/2019
-- * Purpose:
-- *                Add table to PMLookup
-- *****************************************************************************
If NOT EXISTS(SELECT NULL FROM SYS.OBJECTS WHERE NAME='RI_Override_Reason')
BEGIN
    CREATE TABLE RI_Override_Reason
    (
        RI_Override_Reason_id 	INT 		IDENTITY,
        caption_id 				INT 		NOT NULL,
        code 					VARCHAR(10)	NOT NULL,
        description 			VARCHAR(255) 	NOT NULL,
		is_deleted 				TINYINT 	NOT NULL,
        effective_date 			DATETIME 	NOT NULL
        CONSTRAINT PK__RI_Override_Reason_RI_Override_Reason_id  PRIMARY KEY CLUSTERED (RI_Override_Reason_id)  
    )
END
GO
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'RI_Override_Reason')
BEGIN
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'RI_Override_Reason', 3, 1)
END
GO
EXEC DDLAddColumn  'RI_Arrangement', 'ri_override_reason_id', 'int' 
GO
EXECUTE DDLADDCOLUMN 'MID_Policy','DA_Branch_id','varchar(20)'	
GO
Exec DDLAddColumn "GIS_property","is_claim360display",tinyint
GO
EXEC DDLAddColumn "Party_Bank","is_default",tinyint
GO
EXEC DDLAddColumn "Party_Bank_History","is_default",tinyint
GO
EXEC DDLAddColumn "CashListItem","cc_insurance_file_cnt",INT
GO
EXEC DDLAddColumn "CashListItem","cc_token_id","VARCHAR(255)"
GO
EXEC DDLAddColumn "PFpremiumFinance","VIAPaymentHub",tinyint 
GO

-- *****************************************************************************  
-- * Author:       Inder Singh
-- * Date:         05-05-2020
-- * Purpose:      PRE Version Selection
-- *****************************************************************************
EXEC DDLAddColumn 'risk_type_rule_set', 'pre_version', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'risk_type_rule_set', 'pre_ruleset_effective_date', 'VARCHAR(50) NULL'
GO 
EXEC DDLAddColumn 'risk_type_rule_set', 'pre_child_ruleset_effectivedate', 'TINYINT NULL'
GO 

IF EXISTS
    (
        SELECT option_number 
		FROM System_Options
		WHERE description LIKE 'Enable Exclusive Locking:' AND option_number=5115
    )
BEGIN
	UPDATE System_Option_Configuration SET option_number=5174 WHERE option_number=5115
END
GO


--end - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
GO
EXEC DDLADDColumn 'policy_standard_wording', 'do_not_merge' ,'INT DEFAULT(0)'
GO
-- PM054785 (Sajid)
DDLALTERCOLUMN 'PFPremiumFinance','BankAccountName','varchar(50)'
GO
DDLALTERCOLUMN 'pfMediaTypeHistory','BankAccountName','varchar(50)'
GO


-- *****************************************************************************
-- * Author:        AMITA AGGARWAL
-- * Date:          17/10/2016
-- * Purpose:		Add table Correspondence_Type to PMLookup
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Correspondence_Type' AND xtype = 'U')
BEGIN
	CREATE TABLE Correspondence_Type
(
	Correspondence_Type_ID SMALLINT PRIMARY KEY IDENTITY,
	caption_id INT NOT NULL,
	code CHAR(10) NOT NULL,
	description VARCHAR(255) NOT NULL,
	is_deleted TINYINT NOT NULL,
	effective_date DATETIME NOT NULL
)

End
GO
IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'Correspondence_Type')
Begin
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'Correspondence_Type', 3, 1)
End
GO

-- *****************************************************************************
-- * Author:      Amrita Garg
-- * Date:        06/05/2017
-- * Purpose:     Add the column receives_client_correspondence
--				  Paralle wpr3.0 from BFM to IH
-- *****************************************************************************
EXEC DDLAddColumn 'Party_Agent', 'Receives_Client_Correspondence', 'TINYINT DEFAULT 0 NOT NULL'
GO

-- *****************************************************************************
-- * Author:      Amrita Garg
-- * Date:        06/05/2017
-- * Purpose:     Added the columns email_sub_template_code and email_attachment_template_code
--				  Paralle wpr3.0 from BFM to IH
-- *****************************************************************************
EXEC DDLAddColumn 'document_template', 'email_sub_template_code', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'document_template', 'email_attachment_template_code', 'VARCHAR(250) NULL'
GO


-- *****************************************************************************
-- * Author:      AMITA AGGARWAL
-- * Date:        09/11/2016
-- * Purpose:     Add the column correspondence_method_id
-- *****************************************************************************
EXEC DDLAddColumn 'Insurance_File', 'Correspondence_Type', 'Int NULL'
GO

EXEC DDLAddColumn 'Insurance_File', 'Default_Preferred_Correspondence', 'Int NULL'
GO

EXEC DDLAddColumn 'Insurance_File', 'Is_Agent_Correspondence', 'TINYINT Default 0 NOT NULL'
GO


-- *****************************************************************************
-- * Author:       Deepak Raj Singh
-- * Date:         20 Dec 2017
-- * Purpose:      Party History
-- ***************************************************************************** 

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Party_History'
IF @bExists = 0
BEGIN
	CREATE TABLE Party_History 
		(
			Party_History_Id		INT IDENTITY,
			Party_Cnt				INT,
			Party_Data				VARCHAR(MAX),
			Party_Builder_Data		VARCHAR(MAX),
			Date_Of_Change			DATETIME,
			User_Changed			VARCHAR(50)

			CONSTRAINT PK__Party_History_Id PRIMARY KEY CLUSTERED (party_history_id)
		)
END
-- *****************************************************************************
-- * Author:       Navneet Kharwanda
-- * Date:         04 Apr 2018
-- * Purpose:      Party Conviction Column Definition Alter varchar(20) to Varchar(40)
-- *****************************************************************************
 
 EXEC DDLAlterColumn @sTableName='party_conviction' ,@sColumnName ='conviction_date',@sColumnDefinition='varchar(40)'

 -- *****************************************************************************
-- * Author:       Navneet Kharwanda
-- * Date:         17 Apr 2020
-- * Purpose:      Rename Column Name for more Clearity
-- *****************************************************************************

IF EXISTS(SELECT * FROM sys.columns 
          WHERE Name = N'do_not_delete_ren_quote_on_MTA'
          AND Object_ID = Object_ID(N'Product'))
BEGIN
	EXECUTE sp_rename 'Product.do_not_delete_ren_quote_on_MTA', 'Delete_And_ReRun_RenQuote' , 'COLUMN'
 END
ELSE
BEGIN
    EXEC DDLAddColumn 'Product','Delete_And_ReRun_RenQuote','TINYINT NOT NULL DEFAULT 0'
END
GO

-- *****************************************************************************  
-- * Author:       Inder Singh
-- * Date:         05-05-2020
-- * Purpose:      PRE Version Selection

-- *****************************************************************************  
-- * Author:      Mohammad Sajid 
-- * Date:         08-04-2021
-- * Purpose:      Performance Indexes
-- *****************************************************************************

IF NOT EXISTS (SELECT 1
	FROM sys.indexes 
	WHERE name='I_InsuranceFile_Policy_Ignore_covering' AND object_id = OBJECT_ID('[Insurance_File]'))

BEGIN
 CREATE NONCLUSTERED INDEX [I_InsuranceFile_Policy_Ignore_covering]
  ON [Insurance_File] ([policy_ignore])
INCLUDE ([insurance_file_cnt],[insurance_file_type_id],[source_id],
[insurance_folder_cnt],[insurance_ref],[out_of_sequence_replaced])

END 


-- *****************************************************************************  
-- * Author:      Ramesh Kumar
-- * Date:         23-04-2021
-- * Purpose:      
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'insurance_file', 'posting_period_id', 'INT'
GO
EXEC DDLAddIndex 'Gis_policy_link', 'gis_policy_link_id', NULL
UPDATE risk_type_rule_set SET pre_version = 'DREORPRE1' WHERE pre_version IS NULL AND risk_type_rule_set_type_id = 2
GO
UPDATE risk_type_rule_set SET pre_ruleset_effective_date = 'InceptionTPI' WHERE pre_ruleset_effective_date IS NULL AND risk_type_rule_set_type_id = 2
GO
 

-- *****************************************************************************
EXEC DDLAddColumn 'risk_type_rule_set', 'pre_version', 'VARCHAR(50) NULL'
GO
EXEC DDLAddColumn 'risk_type_rule_set', 'pre_ruleset_effective_date', 'VARCHAR(50) NULL'
GO 
EXEC DDLAddColumn 'risk_type_rule_set', 'pre_child_ruleset_effectivedate', 'TINYINT NULL'
GO 

UPDATE risk_type_rule_set SET pre_version = 'DREORPRE1' WHERE pre_version IS NULL AND risk_type_rule_set_type_id = 2
GO
UPDATE risk_type_rule_set SET pre_ruleset_effective_date = 'InceptionTPI' WHERE pre_ruleset_effective_date IS NULL AND risk_type_rule_set_type_id = 2
GO


EXEC DDLAddIndex 'PFPremiumFinance','Plantransaction_id', @sIncludeColumnNames = 'StatusInd'
GO

EXEC DDLAddIndex 'Claim','is_dirty', @sIncludeColumnNames = 'base_claim_id'
GO


EXEC DDLAddColumn 'Reserve', 'Gross_Reserve', 'Currency NULL'
GO
EXEC DDLAddColumn 'Reserve', 'Tax', 'Currency NULL'
GO
EXEC DDLAddColumn 'Reserve', 'Revised_Tax_Reserve', 'Currency NULL'
GO
EXEC DDLAddColumn 'Reserve', 'Revised_Gross_Reserve', 'Currency NULL'
GO
EXEC DDLAddColumn 'Reserve', 'paid_to_date_tax', 'Currency NULL'
GO

-- ********AZD 96621 List Batch Processes *********************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='BatchProcesses_List' AND xtype = 'U')
BEGIN
    CREATE TABLE BatchProcesses_List
    (
        batchProcesses_list_id 	INT 		 PRIMARY KEY IDENTITY,
		description 			VARCHAR(50)  NOT NULL,
        is_deleted 			    TINYINT 	 NOT NULL default(0),
		pmwrk_task_id			INT 	 NOT NULL
		
          
    )
END
GO


--*****************************
If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Batch_Scheduler' AND xtype = 'U')
BEGIN
    CREATE TABLE Batch_Scheduler
    (
	    batch_scheduler_id 		INT 		 PRIMARY KEY IDENTITY,
        batchprocesses_list_id 	INT 		 NOT NULL,
		process					VARCHAR(100),
		description 			VARCHAR(50)  NOT NULL,
        frequency				VARCHAR(50) NOT NULL,
		frequencydescription	VARCHAR(MAX) NOT NULL,
        is_deleted 			    TINYINT 	 NOT NULL default(0)
          
    )
END

EXEC DDLAddOrAlterColumn 'Batch_Scheduler','process', 'VARCHAR(100)'
EXEC DDLAddOrAlterColumn 'Batch_Scheduler','description', 'VARCHAR(200)'
EXEC DDLAddOrAlterColumn 'Batch_Scheduler','batch_file_name', 'VARCHAR(200)'
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Scheduler_FrequencyParameters'
IF @bExists = 0 BEGIN
	CREATE TABLE Batch_Scheduler_FrequencyParameters(
	[batchfrequency_parameter_id]	INT PRIMARY KEY IDENTITY,
	[batch_scheduler_id]			INT NOT NULL CONSTRAINT FK__Batch_Scheduler_FrequencyParameters_batch_scheduler_id FOREIGN KEY REFERENCES Batch_Scheduler(batch_scheduler_id), 
	[frequency_type] 				VARCHAR(100),
	[parameter_name] 				VARCHAR(100) NOT NULL,
	[default_value]					VARCHAR(100) NOT NULL,
	[data_type]						VARCHAR(100) NOT NULL,
	--[prompt] 						VARCHAR(100) NOT NULL,
	[currentid_value] 				VARCHAR(100) NOT NULL
	
	)
END 
GO


--*********************
IF NOT EXISTS(SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'BatchFrequencyDetailType')
BEGIN
CREATE TYPE BatchFrequencyDetailType AS TABLE(
	FrequencyType VARCHAR(200)	,
	ParameterName VARCHAR(300) NOT NULL,
	DefaultValue VARCHAR(300),
	DataType VARCHAR(100),
	CurrentValue VARCHAR(300)
)
END
GO


DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Batch_Scheduler_ScheduledProcessParameters'
IF @bExists = 0 
BEGIN
CREATE TABLE Batch_Scheduler_ScheduledProcessParameters(
	[batchprocess_parameter_id]	INT PRIMARY KEY IDENTITY,
	[batch_scheduler_id]			INT NOT NULL CONSTRAINT FK__Batch_Scheduler_ProcessParameters_batch_scheduler_id FOREIGN KEY REFERENCES Batch_Scheduler(batch_scheduler_id), 
	[control_name] 				VARCHAR(100) NOT NULL,
	[default_value]					VARCHAR(100) NOT NULL,
	[data_type]						VARCHAR(100) NOT NULL,
	[currentid_value] 				VARCHAR(100) NOT NULL
	
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'BatchProcessParameterDetailType')
BEGIN
CREATE TYPE BatchProcessParameterDetailType AS TABLE(
	[Id] [VARCHAR](100) NULL,
	ParameterName VARCHAR(300) NOT NULL,
	DefaultValue VARCHAR(300),
	DataType VARCHAR(100),
	CurrentValue VARCHAR(300)

)
END
GO
-- *****************************************************************************  
-- * Author:      Deepak Arora
-- * Date:        Sept-07-2021
-- * Purpose:     Import Lookup
-- *****************************************************************************
IF NOT EXISTS( SELECT 1 FROM SYS.Types WHERE NAME LIKE 'CodeTableType')
BEGIN
CREATE TYPE CodeTableType AS TABLE
(
	Code VARCHAR(10)
)
END
GO

execute DDLAddOrAlterColumn 'User_Authorities', 'Currency_Loss_Gain_Limit', 'Numeric(19,4) NULL'

execute DDLAddOrAlterColumn 'User_Authorities', 'loss_gain_currency_id', 'SMALLINT NULL'

GO


-- *****************************************************************************  
-- * Author:      Rahul Jaiswal
-- * Date:         01-03-2022
-- * Purpose:      CR25 Underwriting Run Off Treaty
-- *****************************************************************************

DDLADDCOLUMN 'RI_BAND','use_anniversary_date_for_TMP','tinyint'
GO



execute DDLAddOrAlterColumn 'insurance_file', 'Sender_Email', 'varchar(255) NULL'

execute DDLAddOrAlterColumn 'insurance_file', 'Receiver_Email', 'varchar(255) NULL'

GO


-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         06-07-2022
-- * Purpose:      Agent Auto Reconciliation XML Import tables 
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Auto_ReconciliationRS' AND xtype = 'U')
BEGIN
	CREATE TABLE Auto_ReconciliationRS
(
	Auto_ReconciliationRS_ID int IDENTITY(1,1) PRIMARY KEY,
	Batch_ID INT NOT NULL,
    InsurerID VARCHAR(50),
    InsurerName VARCHAR(100),
	DateGenerated DATETIME
)

End
GO

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Premium_ReconciliationRS' AND xtype = 'U')
BEGIN
	CREATE TABLE Premium_ReconciliationRS
(
	Premium_ReconciliationRS_ID int IDENTITY(1,1) PRIMARY KEY,
	Auto_ReconciliationRS_ID INT NOT NULL,
	batch_id INT,
    Reconciliation_ID varchar(40),
    Agent_Group_Code varchar(40),
    Agent_Group_Name varchar(100),
    Agent_Account_Ref varchar(40),  
    Payment_Reference_Number varchar(40)
)

End
GO


If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Account_Entry_RS' AND xtype = 'U')
BEGIN
	CREATE TABLE Account_Entry_RS
(
	Premium_ReconciliationRS_ID INT, 
	batch_id INT NOT NULL, 
    ACC_Reference_Number VARCHAR(80),
	Effective_Date DATETIME,
	Client_Name VARCHAR(255),
	Policy_Number VARCHAR(40),
	Gross_Amount_Due NUMERIC(20,4),
	Commission_Due NUMERIC(20,4),
	Net_Amount_Due NUMERIC(20,4),
	Gross_Amount_Paid NUMERIC(20,4),
	Commission_Paid	 NUMERIC(20,4),
	Net_Amount_Paid	 NUMERIC(20,4),
	Revenue_Type VARCHAR(100),
	Posted_Date	DATETIME,
	Premium_Finance_Transaction TINYINT,
	Transaction_Status VARCHAR(2),
	Allocation_ID int, 
    Comments VARCHAR(255)
)

End
GO

-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         14-03-2023
-- * Purpose:      Table to maintain Agent Commission Level
-- *****************************************************************************

If NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Agent_Commission_Level' AND xtype = 'U')
BEGIN
	CREATE TABLE Agent_Commission_Level
(
Agent_Commission_Level_Id  INT NOT NULL IDENTITY PRIMARY KEY,
Party_agent_cnt INT NOT NULL CONSTRAINT FK__Party__Party_cnt FOREIGN KEY References Party(Party_cnt),
Commission_level_id INT NOT NULL CONSTRAINT FK__Commission_Level__Commission_level_id FOREIGN KEY References Commission_Level(Commission_level_id),
Effective_date DATETIME NOT NULL,
Is_deleted TINYINT NOT NULL)

INSERT INTO Agent_Commission_Level(Party_agent_cnt,Commission_level_id,Effective_date,Is_deleted)
SELECT Party_cnt Party_agent_cnt, Commission_Level_Id , csd.cover_start_date , 0
FROM party_agent, (select top 1  cover_start_date from Insurance_File order by insurance_file_cnt) csd
WHERE commission_level_id IS NOT NULL

END 
GO 

-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         15-03-2023
-- * Purpose:      EH031529 - Copy Policy to Quote
-- *****************************************************************************

EXEC DDLAddColumn 'Product', 'is_retain_policy_number_on_copy', 'TINYINT NULL'
GO 

UPDATE Product SET is_retain_policy_number_on_copy = 0 WHERE is_retain_policy_number_on_copy IS NULL
GO
-- *****************************************************************************  
-- * Author:       Kusum Mittal
-- * Date:         13-09-2023
-- * Purpose:      PM100719 - Creating Sequence to handle max number + 1 for table PFPremiumFinance
-- *****************************************************************************

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'SEQ_PremiumFinanceCnt') AND type = 'SO')
	BEGIN
		DECLARE @FinancePlanCnt INT  = 1
		SELECT  
			@FinancePlanCnt = ISNULL(MAX(pfprem_finance_cnt), 0) + 1 
		FROM PFPremiumFinance  

		EXEC ('CREATE SEQUENCE SEQ_PremiumFinanceCnt
			START WITH ' + @FinancePlanCnt +
			' INCREMENT BY 1;')
	END
GO

-- *****************************************************************************  
-- * Author:       Ashok Sahu
-- * Date:         21-09-2023
-- * Purpose:      Table to maintain Users for Search Transaction Selected Column
-- *****************************************************************************

IF NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='Search_Transaction_Selected_Column' AND xtype = 'U')
BEGIN
	CREATE TABLE Search_Transaction_Selected_Column
	(
		UserName VARCHAR(255) NOT NULL PRIMARY KEY, 
		TransDetailKeys BOOLEAN, 
		BranchKey BOOLEAN, 
		BalanceType BOOLEAN,
		Account BOOLEAN, 
		DocRef BOOLEAN,
        AltRef BOOLEAN , 
		EffectiveDate BOOLEAN,
		TransDate BOOLEAN, 
		DueDate BOOLEAN, 
		MediaType BOOLEAN, 
		AccountAmount_CurrencyAmount BOOLEAN,
		PrimarySettled BOOLEAN,
        OutstandingAmount BOOLEAN, 
		PaidDate BOOLEAN, 
        DocumentTypeCode BOOLEAN,
		Reference BOOLEAN, 
		OperatorName BOOLEAN, 
		Period BOOLEAN, 
		DocTypeGroupCode BOOLEAN, 
		Client BOOLEAN, 
		ClientCode BOOLEAN, 
		MediaRef BOOLEAN,
        AccountKey BOOLEAN,
		PayeeName BOOLEAN, 
		UnderwritingYear BOOLEAN, 
		AccountOutStandingAmount BOOLEAN, 
		CurrencyAmount BOOLEAN, 
		OutStandingCurrencyAmount BOOLEAN,
        BGRef BOOLEAN, 
		CashListkey BOOLEAN, 
		IsLeadagent BOOLEAN, 
		IsSplitReceipt BOOLEAN,
		FOREIGN KEY (UserName) REFERENCES PMUser(username)
 )

End
GO
-- *****************************************************************************  
-- * Author:       Mansi Jain
-- * Date:         06-11-2023
-- * Purpose:      Column added to add comments for payment approval
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem' , 'authorization_comment' , 'VARCHAR(MAX)'

-- *****************************************************************************  
-- * Author:       Mansi Jain
-- * Date:         21-11-2023
-- * Purpose:      Table added for saving selected columns
-- *****************************************************************************


IF NOT EXISTS(SELECT NULL FROM SYSOBJECTS
	WHERE NAME='User_Preferred_Column_List' AND xtype = 'U')
BEGIN
	CREATE TABLE User_Preferred_Column_List
	(
		UserName VARCHAR(255) NOT NULL , 
		InterfaceName VARCHAR(100), 
		ColumnList VARCHAR(1000)
		FOREIGN KEY (username) REFERENCES PMUser(username)
 )

End
GO

-- *****************************************************************************  
-- * Author:       Sandeep Kumar
-- * Date:         06-12-2023
-- * Purpose:      Column added to assign the document library to the party
-- *****************************************************************************
EXEC DDLAddColumn 'Party', 'DocumentLibrary', 'VARCHAR(MAX)'
GO

-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         11-10-2022
-- * Purpose:      CR26 - Anniversary Date Change
-- *****************************************************************************

EXEC DDLAddColumn 'Product', 'anniversary_date_editable', 'TINYINT NULL'
EXEC DDLAddColumn 'Product', 'disable_cover_start_date_on_REN', 'TINYINT NULL'
GO 

UPDATE Product SET disable_cover_start_date_on_Ren = 0 WHERE disable_cover_start_date_on_Ren IS NULL
UPDATE Product SET anniversary_date_editable = 0 WHERE anniversary_date_editable IS NULL
GO 

-- *****************************************************************************  
-- * Author:      Rohit Kumar Mishra
-- * Date:         06-03-2024
-- * Purpose:     Increased size of currency rate column
-- *****************************************************************************

IF Exists ( SELECT * from sys.indexes WHERE NAME ='IX_CurrencyRate_effective_from')
 DROP INDEX IX_CurrencyRate_effective_from ON CurrencyRate
GO

EXEC DDLAlterColumn 'CurrencyRate','rate_against_base','numeric(19, 10)',0
GO

EXEC DDLAlterColumn 'AllocationDetail','orig_xrate','numeric(19, 10)',0
GO

EXEC DDLAlterColumn 'AllocationDetail','effective_xrate','numeric(19, 10)',0
GO

-- *****************************************************************************  
-- * Author:       Kusum Mittal
-- * Date:         23-04-2024
-- * Purpose:      PM101358 - Pure Windows Service (Email sending and document archiving)
-- *****************************************************************************

	DECLARE @bExists TINYINT
	EXECUTE @bExists = DDLExistsTable 'Background_Job_InProcess'
	IF @bExists = 0 BEGIN
		CREATE TABLE Background_Job_InProcess(
		[Background_Job_id] INT PRIMARY KEY	,
		[job_status] NVARCHAR(2) NOT NULL
		)
	END
	GO

-- *********************************************************************************************** 
-- * Author:       Amita Aggarwal
-- * Date:         03-07-2024
-- * Purpose:      Upgrade ICBL Paralleling enhancement EH015460  
-- ***********************************************************************************************   
	EXEC DDLAddOrAlterColumn 'CashListItem', 'media_ref', 'varchar(100)'
	GO
	EXEC DDLAddOrAlterColumn 'Cheque', 'media_ref', 'varchar(100)'
	GO
	EXEC DDLAddOrAlterColumn 'Claim_Payment', 'media_ref', 'varchar(100)'
	Go
	EXEC DDLAddOrAlterColumn 'Report_PaymentandReceipt', 'media_ref', 'varchar(100)'
	GO

	
--*****************************************************************************
-- * Author:        Sumeet Singh
-- * Date:          23/02/2018
-- * Purpose:       Default UDL table for Core UDL_Reserve_Limit -- E7(ICBL)  
-- *****************************************************************************
DECLARE @bTableExists TINYINT
EXECUTE @bTableExists = DDLExistsTable 'claim_reserve_limit'

IF @bTableExists <> 1 
BEGIN
	CREATE TABLE [dbo].[claim_reserve_limit](
		[claim_reserve_limit_id] [int] NOT NULL,
		[caption_id] [int] NOT NULL,
		[code] [char](10) NOT NULL,
		[description] [varchar](255) NULL,
		[is_deleted] [tinyint] NOT NULL,
		[effective_date] [datetime] NOT NULL,
		[UDL_version] [int] NULL,
		[Reserve_Aggregate_Limit] [varchar](255) NULL,
		[User_Group_Code] [varchar](20) NULL,
		[User_Name] [varchar](255) NULL,
	 CONSTRAINT [PK__claim_reserve_limit] PRIMARY KEY CLUSTERED 
	(
		[claim_reserve_limit_id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]	

IF NOT EXISTS (SELECT lookup_table_name FROM PMProduct_Lookup WHERE lookup_table_name = 'claim_reserve_limit')
Begin
        INSERT INTO PMProduct_Lookup
                     (pmproduct_id, lookup_table_name, edit_privilege_level, is_generic_maintenance)
        VALUES     (2, 'claim_reserve_limit', 3, 1)
End

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'UDL_RESERVE_LIMIT'

	IF @bExists = 1 
	BEGIN
	INSERT INTO claim_reserve_limit SELECT * FROM UDL_RESERVE_LIMIT
	END
	
END
GO

-- *********************************************************************************************** 
-- * Author:       Amita Aggarwal
-- * Date:         08-10-2024
-- * Purpose:      Merged ICBL enhancement E6: Commission Maintenance - Pulls Inception Rate
-- ***********************************************************************************************   
EXEC DDLAddOrAlterColumn 'Product', 'use_policy_inception_date', 'tinyint'
GO

-- *********************************************************************************************** 
-- * Author:       Ramesh Kumar
-- * Date:         12-11-2024
-- * Purpose:      Audit Trail Enhencement
-- ***********************************************************************************************   
EXEC DDLAddColumn 'PFScheme','PFScheme_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddColumn 'PFScheme','UserId', 'INT'
EXEC DDLAddColumn 'PFScheme','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'PFScheme','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'PFRF','UserId', 'INT'
EXEC DDLAddColumn 'PFRF','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'PFRF','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddPrimaryKey 'PFRF','pfrf_id'

EXEC DDLAddColumn 'PFSchemeProducts','UserId', 'INT'
EXEC DDLAddColumn 'PFSchemeProducts','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'PFSchemeProducts','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'PFSchemeSource','UserId', 'INT'
EXEC DDLAddColumn 'PFSchemeSource','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'PFSchemeSource','ScreenHierarchy', 'VARCHAR(500)'	

EXEC DDLAddColumn 'Commission_Arrangement','UserId', 'INT'
EXEC DDLAddColumn 'Commission_Arrangement','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Commission_Arrangement','ScreenHierarchy', 'VARCHAR(500)'

exec DDLDropPrimaryKey 'Commission_Arrangement','Party_type', 'party_cnt', 'Product_id', 'risk_type_id', 'transaction_type_id', 'commission_band_id', 'effective_date', 'commission_level_id'
EXEC DDLAddColumn 'commission_arrangement','commission_arrangement_id', 'INT NOT NULL IDENTITY'
exec DDLAddPrimaryKey 'Commission_Arrangement', 'commission_arrangement_id'

EXEC DDLAddColumn 'party','UserId', 'INT'
EXEC DDLAddColumn 'party','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'party','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Address','UserId', 'INT'
EXEC DDLAddColumn 'Address','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Address','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Contact','UserId', 'INT'
EXEC DDLAddColumn 'Contact','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Contact','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'fee_amounts','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'fee_amounts','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Insurer','UserId', 'INT'
EXEC DDLAddColumn 'Party_Insurer','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Insurer','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'party_bank','UserId', 'INT'
EXEC DDLAddColumn 'party_bank','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'party_bank','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Treaty','UserId', 'INT'
EXEC DDLAddColumn 'Treaty','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Treaty','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Treaty_Party','UserId', 'INT'
EXEC DDLAddColumn 'Treaty_Party','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Treaty_Party','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'RI_Model','UserId', 'INT'
EXEC DDLAddColumn 'RI_Model','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'RI_Model','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'RI_Model_Line','UserId', 'INT'
EXEC DDLAddColumn 'RI_Model_Line','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'RI_Model_Line','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Credit_Control_Rule','UserId', 'INT'
EXEC DDLAddColumn 'Credit_Control_Rule','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Credit_Control_Rule','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Credit_Control_Step','UserId', 'INT'
EXEC DDLAddColumn 'Credit_Control_Step','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Credit_Control_Step','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'GIS_User_Def_Header','UserId', 'INT'
EXEC DDLAddColumn 'GIS_User_Def_Header','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'GIS_User_Def_Header','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'GIS_user_def_header_inds','UserId', 'INT'
EXEC DDLAddColumn 'GIS_user_def_header_inds','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'GIS_user_def_header_inds','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'GIS_user_def_detail','UserId', 'INT'
EXEC DDLAddColumn 'GIS_user_def_detail','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'GIS_user_def_detail','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Agent','UserId', 'INT'
EXEC DDLAddColumn 'Party_Agent','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Agent','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddOrAlterColumn 'PMUSER','user_config_xml_dataset','VARCHAR(MAX)'
EXEC DDLAddColumn 'PMUser', 'ModifiedBy', 'INT'
EXEC DDLAddColumn 'PMUser','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'PMUser','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'User_Authorities', 'ModifiedBy', 'INT'
EXEC DDLAddColumn 'User_Authorities','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'User_Authorities','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'pmuser_group_user','pmuser_group_user_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddColumn 'pmuser_group_user','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'pmuser_group_user','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'pmuser_source','pmuser_source_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddColumn 'pmuser_source','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'pmuser_source','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Agent_Commission_Level','UserId', 'INT'
EXEC DDLAddColumn 'Agent_Commission_Level','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Agent_Commission_Level','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Agent_Docs','UserId', 'INT'
EXEC DDLAddColumn 'Agent_Docs','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Agent_Docs','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Agent_Branch','party_agent_branch_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Party_Agent_Branch', 'party_agent_branch_id'
EXEC DDLAddColumn 'Party_Agent_Branch','UserId', 'INT'
EXEC DDLAddColumn 'Party_Agent_Branch','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Agent_Branch','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'CurrencyRate','currency_rate_id', 'INT NOT NULL IDENTITY' 
EXEC DDLAddColumn 'CurrencyRate','UserId', 'INT'
EXEC DDLAddColumn 'CurrencyRate','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'CurrencyRate','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Address_Usage','is_deleted', 'INT'

EXEC DDLAddColumn 'Credit_Control_Rule_Insurance_File_Status','UserId', 'INT'
EXEC DDLAddColumn 'Credit_Control_Rule_Insurance_File_Status','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Credit_Control_Rule_Insurance_File_Status','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Agent_Product','party_agent_product_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Party_Agent_Product', 'party_agent_product_id'
EXEC DDLAddColumn 'Party_Agent_Product','UserId', 'INT'
EXEC DDLAddColumn 'Party_Agent_Product','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Agent_Product','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Relationship','UserId', 'INT'
EXEC DDLAddColumn 'Party_Relationship','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Relationship','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Handler','UserId', 'INT'
EXEC DDLAddColumn 'Party_Handler','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Handler','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'party_handler_branch','party_handler_branch_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'party_handler_branch', 'party_handler_branch_id'
EXEC DDLAddColumn 'party_handler_branch','UserId', 'INT'
EXEC DDLAddColumn 'party_handler_branch','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'party_handler_branch','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddOrAlterColumn 'Source','FSA_staffWording','VARCHAR(MAX)'
EXEC DDLAddColumn 'Source','UserId', 'INT'
EXEC DDLAddColumn 'Source','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Source','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'CompanyCurrency','CompanyCurrency_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'CompanyCurrency', 'CompanyCurrency_id'
EXEC DDLAddColumn 'CompanyCurrency','UserId', 'INT'
EXEC DDLAddColumn 'CompanyCurrency','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'CompanyCurrency','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Bank','UserId', 'INT'
EXEC DDLAddColumn 'Bank','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Bank','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'BankAccount','UserId', 'INT'
EXEC DDLAddColumn 'BankAccount','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'BankAccount','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'BankAccount_Delay','UserId', 'INT'
EXEC DDLAddColumn 'BankAccount_Delay','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'BankAccount_Delay','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'BankAccount_Source','BankAccount_Source_Id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'BankAccount_Source', 'BankAccount_Source_Id'
EXEC DDLAddColumn 'BankAccount_Source','UserId', 'INT'
EXEC DDLAddColumn 'BankAccount_Source','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'BankAccount_Source','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Currency','UserId', 'INT'
EXEC DDLAddColumn 'Currency','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Currency','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Tax_group_tax_band','UserId', 'INT'
EXEC DDLAddColumn 'Tax_group_tax_band','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Tax_group_tax_band','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'tax_band_rate','UserId', 'INT'
EXEC DDLAddColumn 'tax_band_rate','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'tax_band_rate','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddOrAlterColumn 'Peril_Type_Usage','allocate_percent','NUMERIC(7,2)'
EXEC DDLAddColumn 'peril_type_usage','UserId', 'INT'
EXEC DDLAddColumn 'peril_type_usage','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'peril_type_usage','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'earning_pattern_usage','UserId', 'INT'
EXEC DDLAddColumn 'earning_pattern_usage','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'earning_pattern_usage','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'party_other','UserId', 'INT'
EXEC DDLAddColumn 'party_other','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'party_other','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'party_conviction','UserId', 'INT'
EXEC DDLAddColumn 'party_conviction','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'party_conviction','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'previous_accidents','UserId', 'INT'
EXEC DDLAddColumn 'previous_accidents','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'previous_accidents','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Supplier_Business','UserId', 'INT'
EXEC DDLAddColumn 'Party_Supplier_Business','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Supplier_Business','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Other_Party_Branch','Other_Party_Branch_Id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Other_Party_Branch','Other_Party_Branch_Id'
EXEC DDLAddColumn 'Other_Party_Branch','UserId', 'INT'
EXEC DDLAddColumn 'Other_Party_Branch','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Other_Party_Branch','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Party_Agent_Group','UserId', 'INT'
EXEC DDLAddColumn 'Party_Agent_Group','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Party_Agent_Group','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'PMUser_Authority_Level','PMUser_Authority_Level_Id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'PMUser_Authority_Level', 'PMUser_Authority_Level_Id'
EXEC DDLAddColumn 'PMUser_Authority_Level','UserId', 'INT'
EXEC DDLAddColumn 'PMUser_Authority_Level','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'PMUser_Authority_Level','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Numbering_Scheme','UserId', 'INT'
EXEC DDLAddColumn 'Numbering_Scheme','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Numbering_Scheme','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Document_Template','UserId', 'INT'
EXEC DDLAddColumn 'Document_Template','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Document_Template','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'party_certificate_year','UserId', 'INT'
EXEC DDLAddColumn 'party_certificate_year','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'party_certificate_year','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'risk_type_usage','UserId', 'INT'
EXEC DDLAddColumn 'risk_type_usage','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'risk_type_usage','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddOrAlterColumn 'Risk_Type','stamp_duty_rate1','NUMERIC(7,2)'
EXEC DDLAddOrAlterColumn 'Risk_Type','stamp_duty_rate2','NUMERIC(7,2)'
EXEC DDLAddColumn 'Risk_Type','UserId', 'INT'
EXEC DDLAddColumn 'Risk_Type','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Risk_Type','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Risk_Type_RI_Model_Usage','UserId', 'INT'
EXEC DDLAddColumn 'Risk_Type_RI_Model_Usage','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Risk_Type_RI_Model_Usage','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'risk_type_ri_properties','UserId', 'INT'
EXEC DDLAddColumn 'risk_type_ri_properties','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'risk_type_ri_properties','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Risk_Type_RI_Limit_Version','UserId', 'INT'
EXEC DDLAddColumn 'Risk_Type_RI_Limit_Version','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Risk_Type_RI_Limit_Version','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddOrAlterColumn 'risk_type_rule_set','script_quote','VARCHAR(MAX)'
EXEC DDLAddColumn 'risk_type_rule_set','UserId', 'INT'
EXEC DDLAddColumn 'risk_type_rule_set','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'risk_type_rule_set','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Risk_Type_Rating_section_type','risk_type_rating_section_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Risk_Type_Rating_section_type', 'risk_type_rating_section_id'
EXEC DDLAddColumn 'Risk_Type_Rating_section_type','UserId', 'INT'
EXEC DDLAddColumn 'Risk_Type_Rating_section_type','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Risk_Type_Rating_section_type','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'wording_Risk_Type_link','UserId', 'INT'
EXEC DDLAddColumn 'wording_Risk_Type_link','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'wording_Risk_Type_link','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'risk_type_ri_values','UserId', 'INT'
EXEC DDLAddColumn 'risk_type_ri_values','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'risk_type_ri_values','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Primary_Cause_Risk_Type_Group','UserId', 'INT'
EXEC DDLAddColumn 'Primary_Cause_Risk_Type_Group','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Primary_Cause_Risk_Type_Group','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Primary_Cause_Risk_Type_Group','UserId', 'INT'
EXEC DDLAddColumn 'Primary_Cause_Risk_Type_Group','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Primary_Cause_Risk_Type_Group','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Product','UserId', 'INT'
EXEC DDLAddColumn 'Product','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Product_Claims_Workflow','claims_events_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Product_Claims_Workflow', 'claims_events_id'
EXEC DDLAddColumn 'Product_Claims_Workflow','UserId', 'INT'
EXEC DDLAddColumn 'Product_Claims_Workflow','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product_Claims_Workflow','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Product_MTA_Events','mta_events_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Product_MTA_Events', 'mta_events_id'
EXEC DDLAddColumn 'Product_MTA_Events','UserId', 'INT'
EXEC DDLAddColumn 'Product_MTA_Events','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product_MTA_Events','ScreenHierarchy', 'VARCHAR(500)'


EXEC DDLAddColumn 'Product_Claim_Events','claim_events_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Product_Claim_Events', 'claim_events_id'
EXEC DDLAddColumn 'Product_Claim_Events','UserId', 'INT'
EXEC DDLAddColumn 'Product_Claim_Events','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product_Claim_Events','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Product_Source','product_source_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'Product_Source', 'product_source_id'
EXEC DDLAddColumn 'Product_Source','UserId', 'INT'
EXEC DDLAddColumn 'Product_Source','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product_Source','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'pmb_doc_link','UserId', 'INT'
EXEC DDLAddColumn 'pmb_doc_link','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'pmb_doc_link','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Product_Allowed_Causation','UserId', 'INT'
EXEC DDLAddColumn 'Product_Allowed_Causation','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product_Allowed_Causation','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'wording_product_link','UserId', 'INT'
EXEC DDLAddColumn 'wording_product_link','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'wording_product_link','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddOrAlterColumn 'index_linking_detail','percentage','NUMERIC(7,2)'
EXEC DDLAddColumn 'index_linking_detail','UserId', 'INT'
EXEC DDLAddColumn 'index_linking_detail','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'index_linking_detail','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Report_Group_Contents','UserId', 'INT'
EXEC DDLAddColumn 'Report_Group_Contents','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Report_Group_Contents','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'Report_Group_User_Groups','UserId', 'INT'
EXEC DDLAddColumn 'Report_Group_User_Groups','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Report_Group_User_Groups','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'RI_Band_Version','ri_band_version_id', 'INT NOT NULL IDENTITY'
EXEC DDLAddPrimaryKey 'RI_Band_Version', 'ri_band_version_id'

EXEC DDLAddColumn 'Product_Risk_Type_Group','UserId', 'INT'
EXEC DDLAddColumn 'Product_Risk_Type_Group','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'Product_Risk_Type_Group','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLAddColumn 'RIModelCurrencyRates','UserId', 'INT'
EXEC DDLAddColumn 'RIModelCurrencyRates','UniqueId', 'VARCHAR(50)'
EXEC DDLAddColumn 'RIModelCurrencyRates','ScreenHierarchy', 'VARCHAR(500)'

EXEC DDLDropColumn 'Party_Supplier_Business','UserId', 1
EXEC DDLDropColumn 'Party_Supplier_Business','UniqueId', 1
EXEC DDLDropColumn 'Party_Supplier_Business','ScreenHierarchy', 1

IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='configuration_audit_master')
CREATE TABLE dbo.configuration_audit_master (
	configuration_audit_master_id		INT	IDENTITY NOT NULL,
	Module_id							INT,
	ModuleName							VARCHAR(128),
	UniqueId							VARCHAR(50),
    UpdateDate							DATETIME,
    UserId								VARCHAR(128)
);
GO

  
IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='configuration_audit_details')
CREATE TABLE configuration_audit_details(
	configuration_audit_master_id		INT,
	configuration_audit_detail_id		INT IDENTITY NOT NULL,
	Type								CHAR(1),
    TableName							VARCHAR(100),
	key_field_name						VARCHAR(100),
    key_field_value						VARCHAR(100),
	key_field_desc						VARCHAR(500),
    FieldName							VARCHAR(100),
	FieldDisplayName					VARCHAR(500),
    OldValue							VARCHAR(500),
    NewValue							VARCHAR(500)
);
GO

IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='foreign_key_table')
CREATE TABLE dbo.foreign_key_table (
    table_name							VARCHAR(100),
    column_name							VARCHAR(100),
	foreign_key_table_name				VARCHAR(100),
	foreign_key_column_name				VARCHAR(100),
    foreign_key_value_column_name		VARCHAR(100)
);
GO


IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='Audit_Trail_Modules')
CREATE TABLE Audit_Trail_Modules (
    Modules_id			INT,
    ModuleName			VARCHAR(50)
);
GO

IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='Audit_trail_custom_fields')
CREATE TABLE dbo.Audit_trail_custom_fields(	
    TableName							VARCHAR(100),
    FieldName							VARCHAR(100),
    fieldValue							VARCHAR(100),
    FieldDescription					VARCHAR(1000)
);
GO

EXEC DDLAddPrimaryKey @sTableName='configuration_audit_master',@sColumnName1='configuration_audit_master_id'
EXEC DDLAddForeignKey @sTableName='configuration_audit_details',@sColumnName1='configuration_audit_master_id',@sRefTableName='configuration_audit_master',@sRefColumnName1='configuration_audit_master_id'



IF objectproperty(object_id('dbo.DDLAddOrAlterExtendedProperty'), 'IsProcedure') = 1 BEGIN
    DROP PROCEDURE dbo.DDLAddOrAlterExtendedProperty
END
GO
CREATE PROCEDURE DDLAddOrAlterExtendedProperty
@TableName VARCHAR(100),
@ColumnName VARCHAR(100),
@Description VARCHAR(500)
AS

IF NOT EXISTS (SELECT 1 FROM sys.extended_properties  WHERE 
        name = N'MS_Description' 
        AND major_id = OBJECT_ID(@TableName) 
        AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(@TableName) AND name = @ColumnName)
)
EXEC sys.sp_addextendedproperty 
    @name=N'MS_Description', @value=@Description,
    @level0type=N'SCHEMA',@level0name=N'dbo', 
    @level1type=N'TABLE',@level1name=@TableName,
    @level2type=N'COLUMN',@level2name=@ColumnName
ELSE
	EXEC sys.sp_updateextendedproperty
    @name=N'MS_Description', @value=@Description,
    @level0type=N'SCHEMA',@level0name=N'dbo', 
    @level1type=N'TABLE',@level1name=@TableName,
    @level2type=N'COLUMN',@level2name=@ColumnName

GO  

EXEC DDLAddColumn 'system_options','UserId', 'INT'
GO
EXEC DDLAddColumn 'system_options','UniqueId', 'VARCHAR(50)'
GO

-- *****************************************************************************
-- * Author:        Amita Aggarwal
-- * Date:          25/10/2024
-- * Purpose:       EH11 - Extended Rule check for Batch Renewal Job (ICBL WA4 2017) - Parallel from 3.2 SR0 ICBL
-- *****************************************************************************
EXEC DDLAddColumn 'Batch_Renewal_Job', 'run_extended_rule', 'TINYINT NULL'
GO

-- *********************************************************************************************** 
-- * Author:       Ramesh Kumar
-- * Date:         11-01-2019
-- * Purpose:      EH022753 - Parallel to Main 
-- *********************************************************************************************** 
EXEC DDLAddOrAlterColumn 'cashlistitem','insurance_ref','VARCHAR(30)'
GO

-- *****************************************************************************  
 --* Author:       Garima Garg
-- * Date:         28-06-2019
-- * Purpose:      Increased the size of 'our_ref' column in 'claim_payment' table
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'claim_payment', 'our_ref', 'varchar(255) NULL'
GO

-- *****************************************************************************  
--  * Author:       Garima Garg
-- * Date:         28-06-2019
-- * Purpose:      Increased the size of 'our_ref' column in 'CashListItem' table
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'CashListItem', 'our_ref', 'varchar(255) NULL'
GO

-- *****************************************************************************
-- * Author:   kapil Sanotra 
-- * Date:     10-12-2024
-- * Purpose:  Manual journal Authority
-- *****************************************************************************

EXEC DDLAddColumn 'user_authorities','has_ManualJournal_authority','tinyint NULL'
EXEC DDLAddColumn 'user_authorities','ManualJournal_currency_id','int NULL'
EXEC DDLAddColumn 'user_authorities','ManualJournal_currency_amount','Money NULL'

GO

-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         02-12-2024
-- * Purpose:      EH100570 - Changes to Claim Payable 
-- *****************************************************************************
EXEC DDLAddColumn 'claim_payment', 'Payee_Account_type', 'VARCHAR(255) NULL'
GO

-- *****************************************************************************
-- * Purpose:     Add is_final_payment column to claim table
-- *****************************************************************************
EXEC DDLAddColumn 'claim', 'is_final_payment', 'INT NULL'
GO



IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='ManualJournal')
CREATE TABLE ManualJournal
(
ManualJournal_id	INT IDENTITY,
CreatedDate			Date,
DocumentType_id		INT,
Source_id			INT,
is_reffered		    TINYINT,	
PMuser_id			INT,	
Comment				VARCHAR(500),
Reverses_on			DATE,
Recurring_Occurs	INT,
PerPeriodOnDay		INT,
PerMonthOnDay		INT,
PerQuarterOnDay		INT,
Authorisation_comment VARCHAR(1000)
);
GO
 
 IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='ManualJournalDetail')
CREATE TABLE ManualJournalDetail
(
ManualJournalDetail_id INT IDENTITY,
ManualJournal_id	INT,
Account_id			INT,
Amount				NUMERIC(19,2),
Currency_id			INT,
Currency_rate		NUMERIC(19,8),
Base_Amount			NUMERIC(19,4),
Alternate_ref		VARCHAR(50),
Comment				VARCHAR(500),
UnderwritingYear_id INT,
CostCenterId		INT,
Transdetail_id			INT,
Insurance_ref		VARCHAR(30),
Purchase_Order_No	VARCHAR(40),
Purchase_Invoice_No	VARCHAR(40)
);
GO

EXECUTE DDLDropTrigger 'PMUser_tru'
GO

IF NOT EXISTS(SELECT NULL FROM sys.objects WHERE name='ManualJournalApproval')
CREATE TABLE ManualJournalApproval
 (
 approval_cnt 		 INT IDENTITY(1,1),
 manualjournal_id 	 INT,
 approval_user_group INT,
 user_id 			 INT,
 approved 			 TINYINT,
 approval_date 		 DATETIME
 );
 GO
-- *****************************************************************************
-- * AZD 23304 - QBE 3.2 Changes
-- * Author: Baladhivya T
-- * Date: 02/01/2025
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'transdetail', 'reference','varchar(80)'
Go
EXEC DDLAddOrAlterColumn 'insurance_file', 'alternate_reference','varchar(80)'
Go
EXEC DDLAddOrAlterColumn 'event_insurance_file', 'alternate_reference','varchar(80)'
Go
EXEC DDLAddOrAlterColumn 'ManualJournal', 'Authorisation_comment','varchar(1000)'
Go


	--*****************************************************************************
-- * Author:   Prince
-- * Date:     11 March 2025
-- * Purpose:  Econet Enhancement 23945.
-- *****************************************************************************
 

   EXEC DDLAddColumn 'Peril_Type', 'add_stamp_duty_in_first_instalment', 'tinyint'

   Go

-- *****************************************************************************  
-- * Author:       Deepak Singh
-- * Date:         01 April 2025
-- * Purpose:      Indexes added to speed up proc 'spu_SAM_Get_Claim_Payments_Details' used in claim payment
-- *****************************************************************************
EXEC DDLAddIndex @sTableName='AllocationDetail', @sColumnName1='document_ref', @sIncludeColumnNames='allocation_id'
GO

EXEC DDLAddIndex @sTableName='StatsFolder', @sColumnName1='payment_id', @sIncludeColumnNames='document_ref'
GO

-- *****************************************************************************
-- * Author:   kapil sanotra
-- * Date:     25/09/2017
-- * Purpose:  Recommender threshold
-- *****************************************************************************
	EXEC DDLAddOrAlterColumn 'Product', 'Authorisation_Threshold',  'money NULL'
	GO

-- *****************************************************************************
-- * User mapping Pure Insurance SSO Changes
-- * Author: Anil Kumar
-- * Date: 18/04/2025
-- *****************************************************************************
	EXEC DDLADDCOLUMN 'PMUser','sso_preferred_username',' VARCHAR(255)'
	Go

-- *****************************************************************************
-- * User LookUp Audit Trail
-- * Author: Ramesh Sharma
-- * Date: 30/05/2025
-- *****************************************************************************
	IF NOT EXISTS(SELECT 1 FROM sys.types WHERE Name = 'lookup_data')
	BEGIN
	CREATE TYPE lookup_data AS TABLE  --User-Defined Table Type
	(
		Column_Name VARCHAR(100),
		Column_DisplayName VARCHAR(100),
		Column_Value VARCHAR(255)

	)
	END

	EXEC DDLAddPrimaryKey 'Earning_Pattern_Usage','Earning_Pattern_Usage_id'

    DECLARE @bExists TINYINT
	EXECUTE @bExists = DDLExistsTable 'Registry_Setting'
	IF @bExists = 0 
	BEGIN
	CREATE TABLE Registry_Setting (
		registry_setting_id  INT IDENTITY(1,1),
		KeyPath				 VARCHAR(255) ,
		KeyName              VARCHAR(255) ,
		KeyType				 VARCHAR(255) ,
		keydata				 VARCHAR(255) ,
		System_id			 INT,
		System_Logged_in_User VARCHAR(255))
	END

-- *****************************************************************************
-- * Ri Band Version maintenance table
-- * Author: Sravanti Pasumarti
-- * Date: 30/05/2025
-- *****************************************************************************
DECLARE @Table VARCHAR(250)
DECLARE @TableExists INT
SET @Table = 'RI_Band_Version'
EXEC @TableExists = DDLExistsTable @table
IF @TableExists = 0
BEGIN
CREATE TABLE RI_Band_Version (
    code CHAR(10) NOT NULL,
    caption_id INT NOT NULL,
    [description] VARCHAR(255) NOT NULL,
	effective_date DATE NOT NULL,
	ri_band_id INT NOT NULL,
    Date_for_Treaty_XOL_Calculation_id INT,
    XOL_Treaty_To_Recover_From_id INT,
    Proportional_RI_Cal_Method INT,
	use_anniversary_date_for_TMP tinyint,
	UserId  INT,
	UniqueId VARCHAR(50),
	ScreenHierarchy VARCHAR(500)
)
    DECLARE @sql NVARCHAR(MAX) = '
	INSERT INTO ri_band_Version 
    (ri_band_id, caption_id, code, description, effective_date, Date_for_Treaty_XOL_Calculation_id, XOL_Treaty_To_Recover_From_id, Proportional_RI_Cal_Method, use_anniversary_date_for_TMP)
     SELECT ri_band_id, caption_id, code, description,effective_date, Date_for_Treaty_XOL_Calculation_id, XOL_Treaty_To_Recover_From_id, Proportional_RI_Cal_Method, use_anniversary_date_for_TMP
     FROM ri_band';

     EXEC sp_executesql @sql;
END

EXEC DDLAddOrAlterColumn 'RI_Band_Version','description','VARCHAR(255)'

GO
--*****************************************************************************
-- * Author:	Sravanti Pasumarti
-- * Date:	    30/09/2025
-- * Purpose:	Removal of columns from ri_band 
 --*            Now these will be available under Ri_band_version table
-- *****************************************************************************

EXECUTE DDLDropForeignKey'ri_band','Date_for_Treaty_XOL_Calculation_id'
GO
EXECUTE DDLDropForeignKey 'ri_band','Proportional_RI_Cal_Method'
GO
EXECUTE DDLDropForeignKey 'ri_band' ,'XOL_Treaty_To_Recover_From_id'
GO
EXECUTE DDLDropDefault 'ri_band','Proportional_RI_Cal_Method'
GO
EXECUTE DDLDropColumn 'ri_band','Date_for_Treaty_XOL_Calculation_id'
GO
EXECUTE DDLDropColumn 'ri_band','Proportional_RI_Cal_Method'
GO
EXECUTE DDLDropColumn 'ri_band','XOL_Treaty_To_Recover_From_id'
GO
EXECUTE DDLDropColumn 'ri_band','use_anniversary_date_for_TMP'
GO

-- *****************************************************************************  
-- * Author:      Ashok Kumar Sahu
-- * Date:         06-08-2025
-- * Purpose:     Store the ri_arrangement_line_id version wise on multiple RI refresh 
-- *****************************************************************************

	EXEC DDLAddColumn 'ri_arrangement_line_archive', 'ri_arrangement_line_Version_id', 'TINYINT NULL'
	GO
	EXEC DDLAddColumn 'ri_arrangement_line_archive', 'created_date', 'DATETIME NULL'
	GO
-- *****************************************************************************
-- * RIModelCurrencyRates table
-- * Author: Sanjana Gulia
-- * Date: 07/11/2025
-- *****************************************************************************
DECLARE @Table VARCHAR(250)
DECLARE @TableExists INT
SET @Table = 'RIModelCurrencyRates'
EXEC @TableExists = DDLExistsTable @table
IF @TableExists = 0
BEGIN
    CREATE TABLE dbo.RIModelCurrencyRates (
        RIModelCurrencyRates_ID INT IDENTITY(1,1) PRIMARY KEY,
        currency_id INT NULL, 
		ri_model_id int null,
        conversion_rate DECIMAL(18,6) NULL
    );
END

Go

	EXECUTE DDLDropView 'party_account_handler'
	GO

	CREATE VIEW dbo.party_account_handler AS
	SELECT ph.party_cnt, ph.forename, ph.initials, ph.department_id, ph.party_title_code ,ph.UserId,ph.UniqueId,ph.ScreenHierarchy 
	FROM party_handler ph  
	INNER JOIN party p  
	ON ph.party_cnt = p.party_cnt  
	INNER JOIN party_type pt  
	ON pt.party_type_id = p.party_type_id  
	WHERE (pt.code = 'AH')

	GO

	EXECUTE DDLDropView 'party_consultant'
	GO

	CREATE VIEW dbo.party_consultant AS
	SELECT 
		ph.party_cnt, 
		ph.forename, 
		ph.initials, 
		ph.department_id, 
		ph.party_title_code, 
		ph.commission_cnt,
		ph.UserId,
		ph.UniqueId,
		ph.ScreenHierarchy
	FROM 
		party_handler ph  
		INNER JOIN party p ON ph.party_cnt = p.party_cnt  
		INNER JOIN party_type pt ON pt.party_type_id = p.party_type_id  
	WHERE 
		pt.code = 'CO';

	GO


-- *****************************************************************************
-- * Author:       CR03 - Document Upload Category Selection - Prince
-- * Date:         05/05/2026
-- * Purpose:      Add document_template_group_id and document_template_sub_group_id
-- *               columns to DOC_document for storing category/sub-category
-- *               directly on uploaded documents
-- *****************************************************************************
EXEC DDLAddColumn 'Doc_Document', 'document_template_group_id', 'INT NULL'
GO
EXEC DDLAddColumn 'Doc_Document', 'document_template_sub_group_id', 'INT NULL'
GO

-- *****************************************************************************
-- * Author:   Sanjana Gulia
-- * Date:     12-12-2025
-- * Purpose:  Add treaty_premium_type to ri_model table
-- *****************************************************************************
EXEC DDLAddColumn 'RI_Model', 'treaty_premium_type', 'tinyint NULL'
GO
EXEC DDLAddColumn 'Audit_ri_model', 'treaty_premium_type', 'tinyint NULL'
GO


-- *****************************************************************************
-- * Author:   Sanjana Gulia
-- * Date:     16-12-2025
-- * Purpose:  Add Premium_Calculation_Basis table 

DECLARE @Table VARCHAR(250)
DECLARE @TableExists INT
SET @Table = 'Premium_Calculation_Basis'
EXEC @TableExists = DDLExistsTable @table
IF @TableExists = 0
BEGIN
-- Premium_Calculation_Basis Table Creation Script

CREATE TABLE Premium_Calculation_Basis (
    premium_calculation_basis_id INTEGER NOT NULL,
    caption_id INTEGER NOT NULL,
    code CHAR(10) NOT NULL,
    description VARCHAR(255) NULL,
    is_deleted TINYINT NOT NULL,
    effective_date DATETIME NOT NULL,
    CONSTRAINT PK_Premium_Calculation_Basis PRIMARY KEY (premium_calculation_basis_id),
);

END
Go

EXEC DDLAddColumn 'Premium_Calculation_Basis', 'reinsurance_type_id', 'integer NULL'
GO

EXEC DDLAddForeignKey 'Premium_Calculation_Basis', 'reinsurance_type_id', @sRefTableName = 'reinsurance_type'
GO
EXEC DDLAddColumn 'Premium_Calculation_Basis', 'calculation_factors', 'varchar(100) NULL'
GO

-- *****************************************************************************
-- * Author:   Sanjana Gulia
-- * Date:     16-12-2025
-- * Purpose:  Add treaty_premium_type to ri_model table
-- *****************************************************************************
EXEC DDLAddColumn 'RI_Model_Line', 'premium_calculation_basis_id', 'integer NULL'
GO

EXEC DDLAddForeignKey 'RI_Model_Line', 'premium_calculation_basis_id', @sRefTableName = 'premium_calculation_basis'
GO

-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         24-11-2025
-- * Purpose:      EH100709 - Arch void transactions
-- *****************************************************************************

EXEC DDLAddColumn 'User_Authorities', 'void_policy_version', 'Varchar(30) NULL'
EXEC DDLAddColumn 'Product', 'void_policy_version', 'TINYINT NULL'
EXEC DDLAddColumn 'Insurance_file','original_insurance_file_type_id','Integer NULL'

Go

UPDATE Product SET void_policy_version = 0 where void_policy_version IS NULL
UPDATE User_Authorities SET void_policy_version = 'Not Allowed' where void_policy_version IS NULL
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'void_reverse_transaction_log'
IF @bExists = 0 BEGIN
    CREATE TABLE void_reverse_transaction_log(
	[reverse_transaction_log_id] INT IDENTITY(1,1) NOT NULL,
	[void_insurance_file_cnt] INT NOT NULL,
	[user_id] INT,
	[reversal_date] DATETIME	
	)
END
GO

DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'void_reverse_transaction_log_detail'
IF @bExists = 0 BEGIN
    CREATE TABLE void_reverse_transaction_log_detail(
	[reverse_transaction_log_detail_id] INT IDENTITY(1,1) NOT NULL,
	[reverse_transaction_log_id] INT NOT NULL,
	[allocation_id] INT NOT NULL,
	[account_id] INT NOT NULL,
	[is_reverse_allocated] TINYINT
	)
END
GO

-- *****************************************************************************
-- * Author:       Shipali Sharma
-- * Date:         15-1-2026
-- * Purpose:      Variable Quota Share Configuration Table Structure
-- *****************************************************************************

EXECUTE DDLAddColumn 'RI_Model_Line', 'Is_VariableQuotaShare', 'TinyInt NULL', @bQuiet = 1
GO
-- Create Variable Quota Share Configuration table
DECLARE @bExists TINYINT
EXECUTE @bExists = DDLExistsTable 'Variable_Quota_Share_Config'
IF @bExists = 0 BEGIN
    CREATE TABLE Variable_Quota_Share_Config(
        [variable_quota_share_id] INT IDENTITY(1,1) PRIMARY KEY,
        [treaty_id] INT NOT NULL,
        [sa_lower_limit] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [sa_upper_limit] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [share_percent] DECIMAL(5,2) NOT NULL DEFAULT 0,
        [treaty_limit] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [created_date] DATETIME NOT NULL DEFAULT GETDATE(),
        [modified_date] DATETIME NOT NULL DEFAULT GETDATE(),
        [is_deleted] TINYINT NOT NULL DEFAULT 0,
		[ri_model_line_id] INT NULL,
		[ri_model_id] INT NULL
    )
END
GO

-- Add foreign key constraint
EXEC DDLAddForeignKey 'Variable_Quota_Share_Config', 'treaty_id', @sRefTableName = 'Treaty'
GO
-- Add foreign key constraint for ri_model_id
EXEC DDLAddForeignKey 'Variable_Quota_Share_Config', 'ri_model_id', @sRefTableName = 'RI_Model'
GO
-- Add indexes for performance
EXEC DDLAddIndex 'Variable_Quota_Share_Config', 'treaty_id'
GO
EXEC DDLAddIndex 'Variable_Quota_Share_Config', 'ri_model_id'
GO

-- *****************************************************************************  
-- * Author:       Amita Aggarwal
-- * Date:         28-1-2026
-- * Purpose:      EH100709 - Arch void transactions
-- *****************************************************************************

EXEC DDLAddColumn 'void_reverse_transaction_log_detail','parent_document_ref','varchar(30) NULL'
GO

-- *****************************************************************************  
-- * Author:       Ramesh Kumar
-- * Date:         25-02-2026
-- * Purpose:      Quote Versioning
-- *****************************************************************************
EXEC DDLAddColumn 'product','is_quote_versioning','TINYINT DEFAULT 0 NOT NULL'
GO
EXEC DDLAddColumn 'product','delete_quote_after','INT DEFAULT 0 NOT NULL'
Go

EXEC DDLAddColumn 'insurance_file','quote_base_insurance_file_cnt','INT'
GO

-- *****************************************************************************  
-- * Author:       Sravanti Pasumarti
-- * Date:         11 Feb 2026
-- * Purpose:      Added Column for FAC Premium
-- *****************************************************************************

EXEC DDLAddColumn 'ri_arrangement_line','FACPropPremiumPerc','float NULL'

GO


-- *****************************************************************************
-- * Author:   Sravanti Pasumarti
-- * Date:     25-Feb-2026
-- * Purpose:  Add manually_added column to ri_Arrangement_line table
-- *****************************************************************************
EXEC DDLAddColumn 'ri_Arrangement_line','manually_added','TINYINT NULL'
GO

-- *****************************************************************************
-- * Author:   Sravanti Pasumarti
-- * Date:     25-Feb-2026
-- * Purpose:  Create index on RI_Arrangement_Line (ri_arrangement_id, manually_added)
-- *****************************************************************************

EXEC DDLAddIndex 'RI_Arrangement_Line', 'ri_arrangement_id', 'manually_added'

GO

-- *****************************************************************************  
-- * Author:       Shipali Sharma
-- * Date:         6 Mar 2026
-- * Purpose:      Added Column for Treaty
-- *****************************************************************************

EXEC DDLAddColumn 'treaty','treaty_limit','DECIMAL(18,2) DEFAULT NULL NULL'

EXEC DDLAddColumn 'treaty','currency_id','INT DEFAULT 0'

EXEC DDLAddColumn 'treaty','reinstatements','INT DEFAULT NULL'


GO

-- *****************************************************************************
-- * Author:   Sravanti Pasumarti
-- * Date:     25-Feb-2026
-- * Purpose:  Add manually_added column to claim_ri_Arrangement_line table
-- *****************************************************************************
EXEC DDLAddColumn 'Claim_ri_Arrangement_Line','manually_added','TINYINT NULL'


GO

-- *****************************************************************************
-- * Author:   Sravanti Pasumarti
-- * Date:     17-Mar-2026
-- * Purpose:  Add manually_added column to RI_Arrangement_Line_Archive table
-- *****************************************************************************
EXEC DDLAddColumn 'RI_Arrangement_Line_Archive','manually_added','TINYINT NULL'


GO

-- *****************************************************************************
-- * PBI 26251: Reinstatement of XOL (Claims)
-- * Author:   Shipali Sharma
-- * Date:     2025-03-17
-- * Purpose:  Add current_reinstatement_count column to Treaty table to track
--            the number of times a treaty has been reinstated during the current period
-- *****************************************************************************
EXEC DDLAddColumn 'Treaty', 'current_reinstatement_count', 'INT DEFAULT 0'
GO

-- *****************************************************************************
-- * PBI 35359: RI Model Editable
-- * Author:   Shipali Sharma
-- * Date:     2026-03-24
-- * Purpose:  Add is_edited column to RI_Arrangement_Line and
--             Claim_ri_Arrangement_Line tables to track manually
--             overridden treaty lines
-- *****************************************************************************
EXEC DDLAddColumn 'RI_Arrangement_Line', 'is_edited', 'bit NULL'
GO
EXEC DDLAddColumn 'Claim_ri_Arrangement_Line', 'is_edited', 'bit NULL'
GO

-- *****************************************************************************
-- * ADO #39452: Instalment for Claim Recovery - Scheme Configuration
-- * Author:   AI Agent (AIDLC)
-- * Date:     2026-05-06
-- * Purpose:  Add recovery_instalments_enabled column to Product table
--             to enable/disable recovery instalment functionality per product.
-- *****************************************************************************
EXEC DDLAddColumn 'Product', 'recovery_instalments_enabled', 'TINYINT NOT NULL DEFAULT 0'
GO

-- *****************************************************************************
-- * PBI 37147: Surplus Lines in Decimals
-- * Author:   Sravanti Pasumarti
-- * Date:     06-May-2026
-- * Purpose:  Widen number_of_lines from smallint to DECIMAL(10,2) to support
--             fractional surplus treaty lines (e.g. 10.25, 5.50)
-- *****************************************************************************
EXEC DDLAddOrAlterColumn 'RI_Model_Line','number_of_lines', 'DECIMAL(10,2) NOT NULL'
EXEC DDLAddOrAlterColumn 'RI_Arrangement_Line','number_of_lines', 'DECIMAL(10,2) NOT NULL'
EXEC DDLAddOrAlterColumn 'RI_Arrangement_Line_Archive','number_of_lines', 'DECIMAL(10,2) NOT NULL'
EXEC DDLAddOrAlterColumn 'Claim_RI_Arrangement_Line', 'number_of_lines', 'DECIMAL(10,2) NOT NULL'
EXEC DDLAddOrAlterColumn 'Claim_RI_Arrangement_Line_Archive', 'number_of_lines', 'DECIMAL(10,2) NOT NULL'
GO

-- *****************************************************************************
-- * PBI 35359: RI Editable
-- * Author:   Shipali Sharma
-- * Date:     2026-05-15
-- * Purpose:  Add is_edited column to RI_Arrangement_Line_Archive table 
-- *****************************************************************************
EXEC DDLAddColumn 'RI_Arrangement_Line_Archive','is_edited','TINYINT NULL'
GO

-- *****************************************************************************
-- * ADO #39457: Instalment for Claim Recovery - Scheme Configuration
-- * Author:   AI Agent (AIDLC)
-- * Date:     2026-05-06
-- * Purpose:  Add transaction_type column to PFRate table to distinguish
--             rate records by recovery type (Salvage/Third-Party).
-- *****************************************************************************
EXEC DDLAddColumn 'PFRF', 'transaction_type', 'TINYINT NULL'
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CH__PFRF__transaction_type')
    EXEC DDLAddCheck 'PFRF', 'transaction_type', '([transaction_type] IS NULL OR [transaction_type] IN (1, 2))'
GO

-- * End of script
-- *****************************************************************************

-- *****************************************************************************
-- * ADO #39455: Instalment for Claim Recovery - Validation Support
-- * Author:   AI Agent (AIDLC)
-- * Date:     2026-05-11
-- * Purpose:  Add claim_recovery_transaction_id column to PFPremiumFinance
--             to link instalment plans to CLR recovery transactions.
-- *****************************************************************************
EXEC DDLAddColumn 'PFPremiumFinance', 'claim_recovery_transaction_id', 'INT NULL'
GO


-- *****************************************************************************
-- * ADO #39456: Instalment for Claim Recovery - Scheme Configuration
-- * Author:   Kapil Sanotra
-- * Date:     2026-05-18
-- * Purpose:  Add scheme_type column to PFScheme table to distinguish
--             Premium Finance (1) from Claim Recovery (2) schemes.
-- *****************************************************************************
EXEC DDLAddColumn 'PFScheme', 'scheme_type', 'TINYINT NULL DEFAULT 1'
GO

-- *****************************************************************************
-- * PBI 35359: RI Manual Premium Adjustment
-- * Author:   
-- * Date:     2026-06-01
-- * Purpose:  Add is_premium_edited column to RI_Arrangement_Line to track
--             lines where the user has directly edited the premium value,
--             preventing the treaty premium calc proc from overwriting it.
-- *****************************************************************************
EXEC DDLAddColumn 'RI_Arrangement_Line', 'is_premium_edited', 'BIT NULL'
GO

EXEC DDLAddColumn 'RI_Arrangement_Line_Archive', 'is_premium_edited', 'BIT NULL'
GO

EXEC DDLAddColumn 'Claim_RI_Arrangement_Line', 'is_premium_edited', 'BIT NULL'
GO

EXEC DDLAddColumn 'Claim_RI_Arrangement_Line_Archive', 'is_premium_edited', 'BIT NULL'
GO

-- *****************************************************************************
-- * PBI 39413: New User Authority to Extract Client Data
-- * Author:   Shipali Sharma
-- * Date:     2026-06-09
-- * Purpose:  Add can_extract_client_data column to User_Authorities table
--             to control access to the Extract Client Data functionality.
-- *****************************************************************************
EXEC DDLAddColumn 'User_Authorities', 'can_extract_client_data', 'TINYINT NULL'
GO

-- *****************************************************************************
-- * PBI #37524: Instalment for Claim Recovery - New Plan
-- * Author:   Kapil Sanotra
-- * Date:     2026-06
-- * Purpose:  Add source_type column to PFPremiumFinance to distinguish
--             Premium Finance ('PF') from Claim Recovery ('CLR') plans.
-- *****************************************************************************
EXEC DDLAddColumn 'PFPremiumFinance', 'source_type', 'VARCHAR(10) NULL'
GO

-- *****************************************************************************
-- * PBI #37524: Instalment for Claim Recovery - New Plan
-- * Author:   Kapil Sanotra
-- * Date:     2026-06
-- * Purpose:  Add claim_number column to PFPremiumFinance to store the
--             associated claim reference for Claim Recovery instalment plans.
-- *****************************************************************************
EXEC DDLAddColumn 'PFPremiumFinance', 'claim_number', 'VARCHAR(30) NULL'
GO
