SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Commission_Register
GO

CREATE PROCEDURE spu_Report_Commission_Register
AS
  
/**********************************************************************************************************************************  
** Created by Jude Killip  
** 14/08/2000  
** RSA Reports - Commission_Register.rpt  
**  Created with dummy data to build the report  
**********************************************************************************************************************************  
** 12/10/2000 - JMK - Update to use DB  
**  
** 02/11/2000 - JMK - Bring in line with Premium Register  
***********************************************************************************************************************************/  
BEGIN

    CREATE TABLE #tempRSACommReg  
    (  
            StatsFolderCnt INT,  
            PolNum VARCHAR (30) NULL,  
            InsFileCnt INT,  
            Client VARCHAR (20) NULL,  
            TransTypeID INT NULL,  
            TransType VARCHAR(10) NULL,  
            ProductCode VARCHAR (10) NULL,  
            TransDate DATETIME NULL,  
            Agency VARCHAR (20) NULL,  
            FromDate DATETIME NULL,  
            ToDate DATETIME NULL,  
            Product VARCHAR (255) NULL,  
            RiskType VARCHAR (255) NULL,  
            StatsDetailType CHAR(1),  
            RiskTypeCode INT NULL,  
            TOB varchar (10) NULL,  
            TransRef VARCHAR (30) NULL,  
            SumInsured DECIMAL (19,4) NULL,  
            GrossCommission DECIMAL (19,4) NULL,  
            Duties DECIMAL (19,4) NULL,  
            QSLocal DECIMAL (19,4) NULL,  
            FacReins DECIMAL (19,4) NULL,  
            NetCommission DECIMAL (19,4) NULL,  
    )  
      
    -- Add Premium Records Gross  
    INSERT INTO #tempRSACommReg  
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
                ISNULL(sd.lead_commission_value_home,0) +  
                    ISNULL(sd.sub_commission_value_home,0),   -- gross  
                NULL,                   -- tax  
                NULL,                   -- QSLocal  
                NULL,                   -- FAC  
                NULL                    -- net  
            FROM 
                Stats_Folder sf
                INNER JOIN Stats_Detail sd
                    ON sf.stats_folder_cnt = sd.stats_folder_cnt 
                LEFT OUTER JOIN Product p
                    ON sf.product_id = p.product_id     
                LEFT OUTER JOIN Risk_Type rt  
                    ON sd.risk_type_id = rt.risk_type_id  
            WHERE 
                sd.stats_detail_type = 'g'  
                AND (
                     ISNULL(sd.lead_commission_value_home,0) <> 0  
                     OR ISNULL(sd.sub_commission_value_home,0) <> 0
                    )  
      
    -- Add Premium Records Tax  
    INSERT INTO #tempRSACommReg  
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
                ISNULL(sd.lead_commission_value_home,0) +  
                    ISNULL(sd.sub_commission_value_home,0),   -- tax  
                NULL,                   -- QSLocal  
                NULL,                   -- FAC  
                NULL                    -- net  
            FROM 
                Stats_Folder sf
                INNER JOIN Stats_Detail sd
                    ON sf.stats_folder_cnt = sd.stats_folder_cnt
                LEFT OUTER JOIN Product p
                    ON sf.product_id = p.product_id      
                LEFT OUTER JOIN Risk_Type rt    
                    ON sd.risk_type_id = rt.risk_type_id             
            WHERE 
                sd.stats_detail_type = 't'  
                AND (
                     ISNULL(sd.lead_commission_value_home,0) <> 0  
                     OR ISNULL(sd.sub_commission_value_home,0) <> 0
                    )  
      
    -- Add Premium Records QSLocal (Treaty)  
    INSERT INTO #tempRSACommReg  
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
                ISNULL(sd.lead_commission_value_home,0) +  
                    ISNULL(sd.sub_commission_value_home,0),   -- QSLocal  
                NULL,                   -- FAC  
                NULL                    -- net  
            FROM 
                Stats_Folder sf
                INNER JOIN Stats_Detail sd
                    ON sf.stats_folder_cnt = sd.stats_folder_cnt  
                LEFT OUTER JOIN Product p
                    ON sf.product_id = p.product_id    
                LEFT OUTER JOIN Risk_Type rt
                    ON sd.risk_type_id = rt.risk_type_id  
            WHERE 
                sd.stats_detail_type = 'T'  
                AND (
                     ISNULL(sd.lead_commission_value_home,0) <> 0  
                     OR ISNULL(sd.sub_commission_value_home,0) <> 0
                    )  
      
    -- Add Premium Records FAC  
    INSERT INTO #tempRSACommReg  
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
                ISNULL(sd.lead_commission_value_home,0) +  
                    ISNULL(sd.sub_commission_value_home,0),   -- FAC  
                NULL                    -- net  
            FROM 
                Stats_Folder sf
                INNER JOIN Stats_Detail sd
                    ON sf.stats_folder_cnt = sd.stats_folder_cnt 
                LEFT OUTER JOIN Product p
                    ON sf.product_id = p.product_id    
                LEFT OUTER JOIN Risk_Type rt  
                    ON sd.risk_type_id = rt.risk_type_id 
            WHERE 
                sd.stats_detail_type = 'F'  
                AND (
                     ISNULL(sd.lead_commission_value_home,0) <> 0  
                     OR ISNULL(sd.sub_commission_value_home,0) <> 0
                    )  
      
    -- Add Premium Records Net  
    INSERT INTO #tempRSACommReg  
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
                ISNULL(sd.lead_commission_value_home,0) +  
                    ISNULL(sd.sub_commission_value_home,0)    -- net  
            FROM 
                Stats_Folder sf
                INNER JOIN Stats_Detail sd
                    ON  sf.stats_folder_cnt = sd.stats_folder_cnt  
                LEFT OUTER JOIN Product p
                    ON sf.product_id = p.product_id    
                LEFT OUTER JOIN Risk_Type rt
                    ON sd.risk_type_id = rt.risk_type_id    
            WHERE 
                sd.stats_detail_type = 'n'  
                AND (
                     ISNULL(sd.lead_commission_value_home,0) <> 0  
                     OR ISNULL(sd.sub_commission_value_home,0) <> 0
                    )  
      
    SELECT * 
    FROM #tempRSACommReg  
    
    DROP TABLE #tempRSACommReg  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
