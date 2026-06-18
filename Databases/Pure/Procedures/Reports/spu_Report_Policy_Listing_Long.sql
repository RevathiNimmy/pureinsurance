SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Policy_Listing_Long
GO

CREATE PROCEDURE spu_Report_Policy_Listing_Long
    @PersonalClient VARCHAR(255),  
    @GroupClient VARCHAR(255),  
    @CorporateClient VARCHAR(255),  
    @PolicyNum VARCHAR(255)  
AS  
  
/**********************************************************************************************************************************  
** Created by Jude Killip  
** 14/09/2000  
** RSA Reports - PolicyListingLong.rpt  
**              (main report)  
**********************************************************************************************************************************  
** 15/09/2000 - JMK - added cursor to sort subagents  
** 23/09/2000 - JMK - moved Agent Details to new Procedure  
** 20/10/2000 - JMK - use insurance_holder_cnt for Client and insured_cnt for Insured  
**  
** 17/11/2000 - Jude Killip     Ins_File_Tax_Value.percent: not a valid column name in SQL7  
**                              ... use "0" for now, to be resolved.  
**  
** 04/12/2000 - Jude Killip     Ins_File_Tax_Value.percent, [percent] is OK  
**                              Use parameters to speed up select when client or policy is selected  
**  
** 18/04/2001 - Jude Killip     initial selection of latest version of policies: #tempInsFileCnt  
**                              update taxes, renewal status  
**  
** 02/06/2001 - Jude Killip     temp table definition corrections (party shortname)  
**  
** 05/07/2001 - Jude Killip     get insurance folder cnt (to include claim details of previous versions)  
**  
** 06/07/2001 - Jude Killip     move Reinsurance to new procedure  
** 07/07/2005 JT  Insured name need to be varchar(255)  
***********************************************************************************************************************************/  
BEGIN

SET NOCOUNT ON
DECLARE @CurrentDate DATETIME
SELECT @CurrentDate = GETDATE()  
  
/* --for testing  
DECLARE @PersonalClient varchar (20),  
        @GroupClient varchar (20),  
        @CorporateClient varchar (20),  
        @PolicyNum varchar (30)  
  
SELECT --@PersonalClient = 'ALL',  
        @PersonalClient = 'saturday',  
        @GroupClient = 'ALL',  
        --@GroupClient = 'WESTP',  
        @CorporateClient = 'ALL',  
        --@CorporateClient = 'ADDERLEYA',  
        @PolicyNum = 'ALL'  
        --@PolicyNum = 'HOM/   /POL/00156'  
*/  
  
DECLARE @sClient VARCHAR (30),  
        @iInsFileCnt INT
/* get Client shortname from one of the 3 Client parameters */  
IF @PersonalClient <> 'ALL'  
BEGIN  
    SELECT @sClient = @PersonalClient  
END  
ELSE IF @GroupClient <> 'ALL'  
BEGIN  
    SELECT @sClient = @GroupClient  
END  
ELSE  
BEGIN  
    SELECT @sClient = @CorporateClient  
END  
  
/* Temp table for insurance_file_cnt filter  
*/  
CREATE TABLE #tempInsFileCnt  
(  
        InsFileCnt INT,  
        InsFolderCnt INT  
)  
  
/* Get insurance_file_cnt selection criteria depending on parameters */  
  
IF @PolicyNum <> 'ALL' AND rtrim(@PolicyNum) <> ''              -- specific Policy  
BEGIN  
    INSERT INTO #tempInsFileCnt  
        SELECT insurance_file_cnt,  
            insurance_folder_cnt  
        FROM    insurance_file  
        WHERE   insurance_file_cnt IN  
            (  
            SELECT  max(insurance_file_cnt)  
            FROM    insurance_file  
            WHERE   insurance_ref LIKE @PolicyNum  
            AND     cover_start_date <= @CurrentDate  
            AND     expiry_date >= @CurrentDate  
            GROUP BY insurance_folder_cnt  
            )  
  
END  
ELSE IF @sClient <> 'ALL'                                       -- specific Client  
BEGIN  
    INSERT INTO #tempInsFileCnt  
        SELECT ifi.insurance_file_cnt,  
            ifi.insurance_folder_cnt  
        FROM Insurance_file ifi  
        JOIN Insurance_Folder ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
        LEFT OUTER JOIN Party pClient ON ifo.insurance_holder_cnt = pClient.party_cnt  
        WHERE pClient.shortname LIKE @sClient  
        AND   insurance_file_cnt IN  
            (  
            SELECT  max(insurance_file_cnt)  
            FROM    insurance_file  
            WHERE   cover_start_date <= @CurrentDate  
            AND     expiry_date >= @CurrentDate  
            GROUP BY insurance_folder_cnt  
            )  
  
END  
ELSE                                                            -- select ALL  
BEGIN  
    INSERT INTO #tempInsFileCnt  
        SELECT insurance_file_cnt,  
            insurance_folder_cnt  
        FROM Insurance_file  
        WHERE insurance_file_cnt IN  
            (  
            SELECT  max(insurance_file_cnt)  
            FROM    insurance_file  
            WHERE   cover_start_date <= @CurrentDate  
            AND     expiry_date >= @CurrentDate  
            GROUP BY insurance_folder_cnt  
            )  
END  
  
/* The data is retrieved in chunks to prevent the total column width  
    exceeding the max 1967 bytes  
*/  
-- Policy Details - Policy, Client, Agent, Product  
CREATE TABLE #tempRSAPolListingL1  
(  
    InsuranceCnt INT,  
    FolderCnt INT,
    ClientCode VARCHAR (20) NULL,           --party.shortname  
    PolicyCode VARCHAR (30) NULL,           --ifi.insurance_ref
    PolicyDescription VARCHAR (255) NULL,   --ifo.description
    Insured VARCHAR (255) NULL,             --party.resolved_name
    AlternativeRef VARCHAR (80) NULL,       --ifi.alternate_reference
    AlternativeAC VARCHAR (20) NULL,        --party.shortname
    CoverFromDate DATETIME NULL,            --ifi.cover_start_date  
    CoverToDate DATETIME NULL,              --ifi.expiry_date  
    AgentCode VARCHAR (20) NULL,            --party.shortname
    AgentName VARCHAR (255) NULL,           --party.resolved_name
    ProductCode VARCHAR (10) NULL,          --product.code
    ProductDesc VARCHAR (255) NULL          --product.description
)  
-- Policy Details - Analysis, Branch, Business  
CREATE TABLE #tempRSAPolListingL2  
(  
    InsuranceCnt INT,
    AnalysisCode VARCHAR (10) NULL,         --Analysis_code.code
    AnalysisDesc VARCHAR (255) NULL,        --Analysis_code.description
    BranchCode VARCHAR (10) NULL,           --Source.code
    BranchName VARCHAR (255) NULL,          --Source.description
    TypeOfBusinessCode VARCHAR (10) NULL,   --Business_Type.code
    TypeOfBusinessDesc VARCHAR (255) NULL,  --Business_Type.description
    RenewalStatus VARCHAR (255) NULL,       --Renewal_Status_Type.description
    CurrencyCode VARCHAR (10) NULL,         --Currency.code
    PaymentMethod VARCHAR (60) NULL,        --ifi.payment_method
    ReplacesPolNum VARCHAR (50) NULL        --ifi.old_policy_number
)  
  
-- Tax  
CREATE TABLE #tempRSAPolListingL4  
(  
    InsuranceCnt INT,
    TaxPerc DECIMAL (19,4) NULL,            --Insurance_File_Tax.percentage
    TaxPrem DECIMAL (19,4) NULL,            --Insurance_File_Tax.premium
    TaxVal DECIMAL (19,4) NULL,             --Insurance_File_Tax.value
    TaxDescription VARCHAR (255) NULL       --Tax_Type.description
)  
  
-- Policy Details - Policy, Client, Agent, Product  
INSERT INTO #tempRSAPolListingL1  
    SELECT ifi.insurance_file_cnt,  
        InsFolderCnt,  
        pClient.shortname,  
        ifi.insurance_ref,  
        ifo.description,  
        pInsured.resolved_name,  
        ifi.alternate_reference,  
        pAlt.shortname,  
        ifi.cover_start_date,  
        ifi.expiry_date,  
        pAgent.shortname,  
        pAgent.resolved_name,  
        prod.code,  
        prod.description  
    FROM Insurance_folder ifo  
    JOIN Insurance_file ifi         ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
    JOIN #tempInsFileCnt ifc        ON ifi.insurance_file_cnt = ifc.InsFileCnt  
    LEFT OUTER JOIN Party pClient   ON ifo.insurance_holder_cnt = pClient.party_cnt  
    LEFT OUTER JOIN Party pInsured  ON ifi.insured_cnt = pInsured.party_cnt  
    LEFT OUTER JOIN Party pAlt      ON ifi.alternate_account_cnt = pAlt.party_cnt  
    LEFT OUTER JOIN Party pAgent    ON ifi.lead_agent_cnt = pAgent.party_cnt  
    LEFT OUTER JOIN Product prod    ON ifi.Product_id = prod.product_id  
  
-- Policy Details - Analysis, Branch, Business  
INSERT INTO #tempRSAPolListingL2  
    SELECT ifi.insurance_file_cnt,  
        an.code,  
        an.description,  
        s.code,  
        s.description,  
        bt.code,  
        bt.description,  
        (SELECT top 1 rst.description  
        FROM Renewal_Status rs  
        JOIN Renewal_Status_Type rst ON ifi.insurance_file_cnt = rs.insurance_file_cnt  
        WHERE rs.renewal_status_type_id = rst.renewal_status_type_id  
        ),  
        cur.code,  
        ifi.payment_method,  
        ifi.old_policy_number  
    FROM Insurance_file ifi  
    JOIN #tempInsFileCnt ifc            ON ifi.insurance_file_cnt = ifc.InsFileCnt  
    LEFT OUTER JOIN Analysis_code an    ON ifi.analysis_code_id = an.analysis_code_id  
    LEFT OUTER JOIN Source s            ON ifi.source_id = s.source_id  
    LEFT OUTER JOIN Business_Type bt    ON ifi.business_type_id = bt.business_type_id  
    LEFT OUTER JOIN Currency cur        ON ifi.currency_id = cur.currency_id  
  
-- Tax  
INSERT INTO #tempRSAPolListingL4  
    SELECT ift.insurance_file_cnt,  
        ift.percentage,  
        ift.premium,  
        ift.value,  
        tt.description  
    FROM Tax_Calculation ift  
    JOIN #tempInsFileCnt ifc    ON ift.insurance_file_cnt = ifc.InsFileCnt  
    JOIN Tax_Band tb            ON ift.tax_band_id = tb.tax_band_id  
    JOIN Tax_Type tt            ON tt.tax_type_id = tb.tax_type_id  
  
DROP TABLE #tempInsFileCnt  
SET NOCOUNT OFF  
  
-- Squirt it all out for the report  
SELECT * 
FROM 
    #tempRSAPolListingL1 L1
    LEFT OUTER JOIN #tempRSAPolListingL2 L2
        ON L1.InsuranceCnt = L2.InsuranceCnt    
    LEFT OUTER JOIN #tempRSAPolListingL4 L4  
        ON L1.InsuranceCnt = L4.InsuranceCnt 
  
DROP TABLE #tempRSAPolListingL1  
DROP TABLE #tempRSAPolListingL2  
DROP TABLE #tempRSAPolListingL4  

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

