SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Premium_Register
GO

CREATE PROCEDURE spu_Report_Premium_Register
AS  
  
/**********************************************************************************************************************************  
** Created by Jude Killip  
** 09/08/2000  
** RSA Reports - PremiumRegister.rpt  
**  Created with dummy data to build the report  
**********************************************************************************************************************************  
** 12/10/2000 - JMK -   Update to use DB  
**  
** 19/10/2000 - JMK -   Add fields stats_folder_cnt and stats_detail_type  
**                      Add tax (Duties) and treaty (QSLocal) records  
**  
** 02/11/2000 - JMK -   Fix error where Risk table used wrongly instead of Risk_Type  
***********************************************************************************************************************************/  
BEGIN

CREATE TABLE #tempRSAPremReg  
(  
        StatsFolderCnt INT,  
        PolNum VARCHAR (30) NULL,  
        InsFileCnt INT,  
        Client VARCHAR (20) NULL,  
        TransTypeID INT NULL,  
        TransType VARCHAR (10) NULL,  
        ProductCode VARCHAR (10) NULL,  
        TransDate DATETIME NULL,  
        Agency VARCHAR (20) NULL,  
        FromDate DATETIME NULL,  
        ToDate DATETIME NULL,  
        Product VARCHAR (255) NULL,  
        RiskType VARCHAR (255) NULL,  
        StatsDetailType CHAR(1),  
        RiskTypeCode INT NULL,  
        TOB VARCHAR (10) NULL,  
        TransRef VARCHAR (30) NULL,  
        SumInsured DECIMAL (19,4) NULL,  
        GrossRefundOrPremium DECIMAL (19,4) NULL,  
        Duties DECIMAL (19,4) NULL,  
        QSLocal DECIMAL (19,4) NULL,  
        FacReins DECIMAL (19,4) NULL,  
        NetRefundOrPremium DECIMAL (19,4) NULL,  
)  
  
-- Add Premium Records Gross  
INSERT INTO #tempRSAPremReg  
        SELECT 
            sf.stats_folder_cnt,  
            sf.insurance_ref,  
            sf.insurance_file_cnt,  
            sf.insurance_holder_shortname,  
            sf.transaction_type_id,  
            sf.transaction_type_code,  
            sf.product_code,  
            sf.transaction_date,  
            sf.agent_shortname,  
            sf.cover_start_date,  
            sf.expiry_date,  
            p.description,  
            rt.description,  
            sd.stats_detail_type,  
            sd.risk_id int,  
            sd.class_of_business_code,  
            NULL,  
            sd.sum_insured_home,  
            sd.this_premium_home,   -- gross  
            NULL,                   -- tax  
            NULL,                   -- QSLocal  
            NULL,                   -- FAC  
            NULL                    -- net  
        FROM 
            Stats_Folder sf
            INNER JOIN Stats_Detail sd
                ON sf.stats_folder_cnt = sd.stats_folder_cnt
                AND sd.stats_detail_type = 'g'  
                AND ISNULL(sd.this_premium_home,0) <> 0  
            LEFT OUTER JOIN Product p
                ON sf.product_id = p.product_id    
            LEFT OUTER JOIN Risk_Type rt  
                ON sd.risk_type_id = rt.risk_type_id
  
-- Add Premium Records Tax  
INSERT INTO #tempRSAPremReg  
        SELECT 
            sf.stats_folder_cnt,  
            sf.insurance_ref,  
            sf.insurance_file_cnt,  
            sf.insurance_holder_shortname,  
            sf.transaction_type_id,  
            sf.transaction_type_code,  
            sf.product_code,  
            sf.transaction_date,  
            sf.agent_shortname,  
            sf.cover_start_date,  
            sf.expiry_date,  
            p.description,  
            rt.description,  
            sd.stats_detail_type,  
            sd.risk_id int,  
            sd.class_of_business_code,  
            NULL,  
            sd.sum_insured_home,  
            NULL,                   -- gross  
            sd.this_premium_home,   -- tax  
            NULL,                   -- QSLocal  
            NULL,                   -- FAC  
            NULL                    -- net  
        FROM 
            Stats_Folder sf
            INNER JOIN Stats_Detail sd
                ON sf.stats_folder_cnt = sd.stats_folder_cnt
                AND sd.stats_detail_type = 't'  
                AND ISNULL(sd.this_premium_home,0) <> 0   
            LEFT OUTER JOIN Product p
                ON sf.product_id = p.product_id 
            LEFT OUTER JOIN Risk_Type rt
                ON sd.risk_type_id = rt.risk_type_id    
  
-- Add Premium Records QSLocal (Treaty)  
INSERT INTO #tempRSAPremReg  
        SELECT 
            sf.stats_folder_cnt,  
            sf.insurance_ref,  
            sf.insurance_file_cnt,  
            sf.insurance_holder_shortname,  
            sf.transaction_type_id,  
            sf.transaction_type_code,  
            sf.product_code,  
            sf.transaction_date,  
            sf.agent_shortname,  
            sf.cover_start_date,  
            sf.expiry_date,  
            p.description,  
            rt.description,  
            sd.stats_detail_type,  
            sd.risk_id int,  
            sd.class_of_business_code,  
            NULL,  
            sd.sum_insured_home,  
            NULL,                   -- gross  
            NULL,                   -- tax  
            sd.this_premium_home,   -- QSLocal  
            NULL,                   -- FAC  
            NULL                    -- net  
        FROM 
            Stats_Folder sf
            INNER JOIN Stats_Detail sd
                ON sf.stats_folder_cnt = sd.stats_folder_cnt
                AND sd.stats_detail_type = 'T'  
                AND ISNULL(sd.this_premium_home,0) <> 0   
            LEFT OUTER JOIN Product p
                ON sf.product_id = p.product_id   
            LEFT OUTER JOIN Risk_Type rt 
                ON sd.risk_type_id = rt.risk_type_id  
  
-- Add Premium Records FAC  
INSERT INTO #tempRSAPremReg  
        SELECT 
            sf.stats_folder_cnt,  
            sf.insurance_ref,  
            sf.insurance_file_cnt,  
            sf.insurance_holder_shortname,  
            sf.transaction_type_id,  
            sf.transaction_type_code,  
            sf.product_code,  
            sf.transaction_date,  
            sf.agent_shortname,  
            sf.cover_start_date,  
            sf.expiry_date,  
            p.description,  
            rt.description,  
            sd.stats_detail_type,  
            sd.risk_id int,  
            sd.class_of_business_code,  
            NULL,  
            sd.sum_insured_home,  
            NULL,                   -- gross  
            NULL,                   -- tax  
            NULL,                   -- QSLocal  
            sd.this_premium_home,   -- FAC  
            NULL                    -- net  
        FROM 
            Stats_Folder sf
            INNER JOIN Stats_Detail sd
                ON sf.stats_folder_cnt = sd.stats_folder_cnt
                AND sd.stats_detail_type = 'F'  
                AND ISNULL(sd.this_premium_home,0) <> 0    
            LEFT OUTER JOIN Product p
                ON sf.product_id = p.product_id      
            LEFT OUTER JOIN Risk_Type rt  
                ON sd.risk_type_id = rt.risk_type_id  
  
-- Add Premium Records Net  
INSERT INTO #tempRSAPremReg  
        SELECT 
            sf.stats_folder_cnt,  
            sf.insurance_ref,  
            sf.insurance_file_cnt,  
            sf.insurance_holder_shortname,  
            sf.transaction_type_id,  
            sf.transaction_type_code,  
            sf.product_code,  
            sf.transaction_date,  
            sf.agent_shortname,  
            sf.cover_start_date,  
            sf.expiry_date,  
            p.description,  
            rt.description,  
            sd.stats_detail_type,  
            sd.risk_id int,  
            sd.class_of_business_code,  
            NULL,  
            sd.sum_insured_home,  
            NULL,                   -- gross  
            NULL,                   -- tax  
            NULL,                   -- QSLocal  
            NULL,                   -- FAC  
            sd.this_premium_home    -- net  
        FROM 
            Stats_Folder sf
            INNER JOIN Stats_Detail sd
                ON sf.stats_folder_cnt = sd.stats_folder_cnt 
                AND sd.stats_detail_type = 'n'  
                AND ISNULL(sd.this_premium_home,0) <> 0  
            LEFT OUTER JOIN Product p
                ON sf.product_id = p.product_id
            LEFT OUTER JOIN Risk_Type rt  
                ON sd.risk_type_id = rt.risk_type_id  
  
SELECT * FROM #tempRSAPremReg  
DROP TABLE #tempRSAPremReg  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

