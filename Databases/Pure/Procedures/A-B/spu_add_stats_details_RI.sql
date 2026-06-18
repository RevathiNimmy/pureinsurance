
Execute DDLDropProcedure 'spu_add_stats_details_RI'
GO
CREATE PROCEDURE spu_add_stats_details_RI  
    @stats_folder_cnt int,    
    @is_pt int=0    
AS  
  
DECLARE @insurance_file_cnt int,  
        @source_id int   
DECLARE @Is_PortFolioTransfer INT    
SET   @Is_PortFolioTransfer = @is_pt    
  
DECLARE @is_coinsured_policy          INT,
		@retained_coins_percent       FLOAT,
		@company_id                   INT,
		@out_currency_id              INT,
		@out_currency_rate            NUMERIC(19, 8),
		@out_system_rate              NUMERIC(19, 8),
		@return_status                INT,
		@out_stats_detail_id          INT,
		@out_stats_detail_type        CHAR(3),
		@out_currency_code            VARCHAR(10),
		@RI2007Enabled                INT,
		@product_id                   INT,
		@ri_manual_premium_adjustment INT,
		@base_decimals                INT,
		@System_decimals              INT,
		@out_ri_agreement_code        VARCHAR(20),
        @business_type_code                     VARCHAR(10),    
        @effective_ri_date                      DATETIME    

        declare @document_ref	varchar(25)	 	 
      
    SELECT  
            @insurance_file_cnt = IFL.insurance_file_cnt,  
            @company_id = IFL.source_id,  
            @out_currency_id = IFL.currency_id,  
            @out_currency_rate = IFL.currency_base_xrate,  
            @out_system_rate = IFL.system_base_xrate,  
            @out_currency_code = C.code,  
            @product_id=IFL.product_id,  
            @business_type_code = BT.code,  
            @ri_manual_premium_adjustment=P.ri_manual_premium_adjustment  
            ,@document_ref = sf.document_ref
    FROM Stats_Folder SF  
    JOIN Insurance_File IFL ON SF.insurance_file_cnt = IFL.insurance_file_cnt  
    JOIN Currency C                 ON IFL.currency_id            = C.currency_id  
    JOIN Business_Type BT   ON IFL.business_type_id = BT.business_type_id  
    JOIN Product P                  ON IFl.product_id = P.product_id  
    WHERE SF.stats_folder_cnt = @stats_folder_cnt  
  
    Select @RI2007Enabled = ISNULL(value,0) From Hidden_options  WHERE option_number= 88  
  
    --If this is a coinsured policy then retrieve the party_cnt for Retained.  
    IF @business_type_code like 'COIN%'  
    BEGIN  
        --Establish percentage retained.  
        SELECT  @retained_coins_percent = ISNULL(SUM(cv.share_percent), 0) / 100  
        FROM    Coi_Value cv  
        JOIN    Party_Insurer pin  
            ON  pin.party_cnt = cv.party_cnt  
        WHERE   insurance_file_cnt = @insurance_file_cnt  
        AND     pin.is_retained = 1  
  
        SET @is_coinsured_policy = 1  
    END  
        if substring(LTRIM(RTRIM(@document_ref)),0,4) ='SDD'
	begin
	set @out_currency_rate = 0
	end
      EXEC spu_ACT_Do_Currency_Conversion  
            @company_id = @company_id,  
            @currency_id = @out_currency_id,  
            @currency_amount_unrounded = 1.00,  
            @mode = 'ALL',  
            @currency_base_xrate = @out_currency_rate OUTPUT,  
            @system_base_xrate = @out_system_rate OUTPUT,  
            @base_decimals = @base_decimals OUTPUT,  
            @system_decimals = @system_decimals OUTPUT,  
           @return_status = @return_status OUTPUT  

 IF @out_system_rate = 0 SELECT @out_system_rate=1 SELECT   @out_system_rate=1/@out_system_rate
  
 CREATE TABLE #Perils
 	(
 		 ID                              INT IDENTITY PRIMARY KEY,
 		 Risk_cnt                        INT,
 		 risk_type_id                    INT,
 		 Risk_Type_Code                  VARCHAR(10),
 		 peril_id                        INT,
 		 Peril_Description               VARCHAR(30),
 		 peril_type_id                   INT,
 		 Peril_Type_Code                 VARCHAR(10),
 		 policy_section_type_id          INT,
 		 Policy_Section_Type_Code        VARCHAR(10),
 		 peril_class_of_business_id      INT,
 		 peril_class_Of_Business_Code    VARCHAR(10),
 		 peril_annual_premium            NUMERIC(21, 6),
 		 peril_this_premium_original     NUMERIC(21, 6),
 		 peril_lead_commission_value     NUMERIC(21, 6),
 		 peril_sub_commission_value      NUMERIC(21, 6),
 		 peril_this_sum_insured          NUMERIC(21, 6),
 		 peril_rating_section_id         INT,
 		 peril_ri_band                   INT,
 		 peril_is_share_with_co_insurers TINYINT,
 		 peril_is_Levy_Tax               TINYINT,
 		 peril_original_flag             TINYINT,
 		 reinsurance_type_code           VARCHAR(100),
 		 insurance_file_type_id          INT,
 		 ri_arrangement_id               INT,
 		 orig_ri_arrangement_id          INT,
 		 peril_total_premium             NUMERIC(19, 4),
 		 peril_total_original_premium    NUMERIC(19, 4),
 		 ri_share_percent                FLOAT,
 		 ri_party_cnt                    INT,
 		 ri_shortname                    VARCHAR(255),
 		 treaty_premium_percent          FLOAT,
 		 treaty_commission_percent       FLOAT,
 		 treaty_sum_insured              MONEY,
 		 treaty_premium_value            MONEY,
 		 treaty_commission_value         MONEY,
 		 treaty_original_sum_insured     MONEY,
 		 is_commission_modified          TINYINT,
 		 ri_arrangement_line_id          INT,
 		 sum_insured_change              NUMERIC(19, 4),
 		 RI_Type                         VARCHAR(5),
 		 RI2007                          INT,
 		 line_limit                      NUMERIC(19, 4),
 		 [Priority]                      INT,
 		 stats_version                   INT,
 		 treaty_sum_insured_change       NUMERIC(19, 4),
 		 treaty_sum_insured_change_home  NUMERIC(19, 4),
		 risk_pro_rata_rate              FLOAT,
         ri_pro_rata_rate                   FLOAT    
 	) 
              
SELECT @effective_ri_date = MAX(effective_date)    
      FROM RI_Arrangement RA    
      JOIN insurance_file_risk_link IFRL ON RA.risk_cnt = IFRL.risk_cnt    
      WHERE IFRL.insurance_file_cnt = @insurance_file_cnt    
IF @Is_PortFolioTransfer = 1    

BEGIN  
if (@RI2007Enabled =1)

BEGIN  
 ;WITH RICTE(risk_cnt,ri_arrangement_id,original_flag,ri_band_id,ri_model_id,pro_rata_rate )    
AS
      (SELECT  RA.risk_cnt, RA.RI_arrangement_id, RA.original_flag, RA.ri_band_id, RA.ri_model_id,isnull(ra.pro_rata_rate,1)   From RI_Arrangement RA    
	JOIN insurance_file_risk_link IFRL ON 
	RA.risk_cnt = IFRL.risk_cnt
      WHERE IFRL.insurance_file_cnt = @insurance_file_cnt    
                  AND  RA.effective_date = @effective_ri_date)    
  
INSERT INTO #Perils (  
            Risk_cnt,  
            risk_type_id,  
            Risk_Type_Code,  
            peril_id ,  
            Peril_Description,  
            peril_type_id,  
            Peril_Type_Code,  
            policy_section_type_id,  
            Policy_Section_Type_Code,  
            peril_class_of_business_id ,  
            peril_Class_Of_Business_Code,  
            peril_annual_premium,  
            peril_this_premium_original,  
            peril_lead_commission_value,  
            peril_sub_commission_value,  
            peril_this_sum_insured,  
            peril_rating_section_id,  
            peril_ri_band,  
            peril_is_share_with_co_insurers,  
            peril_is_Levy_Tax ,  
            peril_original_flag ,  
            insurance_file_type_id,  
            ri_arrangement_id,  
            orig_ri_arrangement_id,  
            ri_share_percent,  
            ri_party_cnt,  
            ri_shortname,  
            treaty_premium_percent,  
            treaty_commission_percent,  
            treaty_sum_insured,  
            treaty_premium_value,  
            treaty_commission_value,  
            reinsurance_type_code,  
            treaty_original_sum_insured,  
            is_commission_modified,  
            ri_arrangement_line_id,  
            RI_Type,  
            line_limit,  
            [Priority],  
            RI2007, risk_pro_rata_rate , ri_pro_rata_rate    
      )  
  
        SELECT  P.risk_cnt,  
                R.risk_type_id,  
                RT.code Risk_Type_Code,  
                P.peril_id,  
                P.Description,  
                P.peril_type_id,  
                PT.code Peril_Type_Code,  
                RS.policy_section_type_id,  
                PS.code Policy_Section_Type_Code,  
                P.class_of_business_id,  
                CB.code Class_Of_Business_Code,  
                P.annual_premium,  
                P.this_premium,  
                P.lead_commission_value,  
                P.sub_commission_value,  
                P.sum_insured,  
                P.rating_section_id,  
                P.ri_band,  
                RT.is_share_with_co_insurers,  
                ISNULL(P.is_levy_tax,0) Is_Levy_Tax,  
                RS.original_flag,  
                ifile.insurance_file_type_id,  
                RA.ri_arrangement_id,  
                RAL.ri_arrangement_id,    

				RAL.this_share_percent,  
				COALESCE(T.treaty_id,RAL.Party_cnt),  
				COALESCE(RTRIM(T.Code),RTRIM(Party.shortname)),  
				RAL.premium_percent,  
				RAL.commission_percent,  
				RAL.sum_insured,  
				RAL.premium_value, 

				CASE WHEN RAL.type = 'T' OR RAL.type ='TFS' Then NULL  
					 ELSE RAL.commission_value end,  

				COALESCE(ReT.code,ReT2.Code),  
				NULL AS Original_Sum_insured,  
				RAL.is_commission_modified,  
				RAL.ri_arrangement_line_id,  
				RAL.Type,  
				RAL.line_limit,  
				RAL.Priority,  
  
				CASE WHEN RAL.type = 'T' OR RAL.type ='TFS' OR RAL.type ='F' Then 0  
				WHEN RAL.type = 'TX' OR RAL.type ='TC' OR RAL.type ='FX' OR RAL.type ='PX' Then 1 end,
                r.pro_rata_rate,    
                Ra.pro_rata_rate    
               
        FROM    Insurance_File_Risk_Link IFR    
        JOIN    Peril P                        ON IFR.risk_cnt = P.risk_cnt    
        JOIN    Peril_Type PT                  ON P.peril_type_id = PT.peril_type_id    
        JOIN    Rating_Section RS              ON P.rating_section_id = RS.rating_section_id    
                                              AND P.Risk_cnt = RS.Risk_cnt    
        JOIN    Class_Of_Business CB           ON P.class_of_business_id = CB.class_of_business_id    
        JOIN    Risk R                         ON P.risk_cnt = R.risk_cnt    
        JOIN    Risk_Type RT                   ON R.risk_type_id = RT.risk_type_id    
        LEFT JOIN    
                Policy_Section_Type PS         ON RS.policy_section_type_id = PS.policy_section_type_id    
        JOIN    Insurance_file ifile             ON Ifile.insurance_file_cnt =  IFR.insurance_file_cnt    
    
        JOIN            RICTE RA                      ON RA.risk_cnt = P.risk_cnt AND RA.ri_band_id = P.ri_band AND RA.original_flag = RS.original_flag    
        LEFT JOIN    RICTE RA1                           ON RA1.risk_cnt = P.risk_cnt AND RA1.ri_band_id = P.ri_band AND RA1.original_flag <> RS.original_flag    
    
        JOIN      RI_Arrangement_Line  RAL         ON (RA.ri_arrangement_id = RAL.ri_arrangement_id OR (RA1.ri_arrangement_id = RAL.ri_arrangement_id))    
        LEFT JOIN Treaty T                        ON RAL.treaty_id = T.treaty_id    
        LEFT JOIN    Party_Insurer PIN             ON PIN.party_cnt = RAL.party_cnt    
            LEFT JOIN    Party                               ON Party.party_cnt = RAL.party_cnt    
    
        LEFT JOIN   Reinsurance_Type ReT            ON ReT.reinsurance_type_id = T.reinsurance_type_id    
        LEFT JOIN Reinsurance_Type ReT2         ON Ret2.reinsurance_type_id = PIN.reinsurance_type    
    
        WHERE   IFR.insurance_file_cnt = @insurance_file_cnt    
       -- AND     (IFR.status_flag <> 'U' OR @is_pt =1 )    
    
        AND (IFR.original_risk_cnt IS NULL    
                OR (IFR.original_risk_cnt IS NOT NULL AND ISNULL (IFR.is_risk_edited, 0) = 1)    
                OR (IFR.status_flag IN ('C','D') AND ISNULL(IFR.is_manually_changed, 0) = 0))    
    
        AND    (P.is_premium = 1               -- Only select perils which are 'FAP' or 'SI'    
             OR P.is_sum_insured = 1    
             OR IsNull(P.is_levy_tax, 0) = 1)  -- Thinh Nguyen (19/07/2002) also pick up levy tax    
        AND  ISNULL(P.this_premium, 0) != 0 --Only get details for non-zero premiums    
        AND     (RS.original_flag=0  )    
        AND RAL.type <> 'R'    
        ORDER BY P.rating_section_id ASC    
    
END   
ELSE
BEGIN
;WITH RICTE(risk_cnt,ri_arrangement_id,original_flag,ri_band_id,ri_model_id,pro_rata_rate )
AS
      (SELECT  RA.risk_cnt, RA.RI_arrangement_id, RA.original_flag, RA.ri_band_id, RA.ri_model_id,isnull(ra.pro_rata_rate,1)   From RI_Arrangement RA
	JOIN insurance_file_risk_link IFRL ON	RA.risk_cnt = IFRL.risk_cnt
      WHERE IFRL.insurance_file_cnt = @insurance_file_cnt
)
INSERT INTO #Perils (
            Risk_cnt,            
risk_type_id,            
Risk_Type_Code,           
 peril_id ,           
 Peril_Description,            
peril_type_id,            
Peril_Type_Code,            
policy_section_type_id,            
Policy_Section_Type_Code,            
peril_class_of_business_id ,           
peril_Class_Of_Business_Code,            
peril_annual_premium,            
peril_this_premium_original,            
peril_lead_commission_value,            
peril_sub_commission_value,            
peril_this_sum_insured,            
peril_rating_section_id,            
peril_ri_band,            
peril_is_share_with_co_insurers,            
peril_is_Levy_Tax ,           
 peril_original_flag ,            
insurance_file_type_id,           
 ri_arrangement_id,            
orig_ri_arrangement_id,           
 ri_share_percent,           
 ri_party_cnt,           
 ri_shortname,            
treaty_premium_percent,           
 treaty_commission_percent,        
    treaty_sum_insured,
            treaty_premium_value,            
treaty_commission_value,

            reinsurance_type_code,           
 treaty_original_sum_insured,          
  is_commission_modified,           
 ri_arrangement_line_id,           
 RI_Type,           
 line_limit,          
  [Priority],           
 RI2007, 
risk_pro_rata_rate , 
ri_pro_rata_rate

      )


        SELECT  P.risk_cnt,               
 R.risk_type_id,
                RT.code Risk_Type_Code,
                P.peril_id,
                P.Description,
                P.peril_type_id,
                PT.code Peril_Type_Code,
                RS.policy_section_type_id,
                PS.code Policy_Section_Type_Code,
                P.class_of_business_id,
                CB.code Class_Of_Business_Code,
                P.annual_premium,
                P.this_premium,
                P.lead_commission_value,
                P.sub_commission_value,
                P.sum_insured,
                P.rating_section_id,
                P.ri_band,
                RT.is_share_with_co_insurers,
                ISNULL(P.is_levy_tax,0) Is_Levy_Tax,
                RS.original_flag,
                ifile.insurance_file_type_id,
                RA.ri_arrangement_id,
                RAL.ri_arrangement_id,
		RAL.this_share_percent,
				COALESCE(T.treaty_id,RAL.Party_cnt),
				COALESCE(RTRIM(T.Code),RTRIM(Party.shortname)),
				RAL.premium_percent,
				RAL.commission_percent,
				RAL.sum_insured,
				RAL.premium_value,
				CASE WHEN RAL.type = 'T' OR RAL.type ='TFS' Then NULL
					 ELSE RAL.commission_value end,
				COALESCE(ReT.code,ReT2.Code),
				NULL AS Original_Sum_insured,
				RAL.is_commission_modified,
				RAL.ri_arrangement_line_id,
				RAL.Type,
				RAL.line_limit,
				RAL.Priority,

				CASE WHEN RAL.type = 'T' OR RAL.type ='TFS' OR RAL.type ='F' Then 0
				WHEN RAL.type = 'TX' OR RAL.type ='TC' OR RAL.type ='FX' or RAL.type = 'PX' Then 1 end,
                r.pro_rata_rate,
                Ra.pro_rata_rate
        FROM    Insurance_File_Risk_Link IFR

      JOIN    Peril P                        ON IFR.risk_cnt = P.risk_cnt
        JOIN    Peril_Type PT                  ON P.peril_type_id = PT.peril_type_id

        JOIN    Rating_Section RS              ON P.rating_section_id = RS.rating_section_id
                                              AND P.Risk_cnt = RS.Risk_cnt
        JOIN    Class_Of_Business CB           ON P.class_of_business_id = CB.class_of_business_id

        JOIN    Risk R                         ON P.risk_cnt = R.risk_cnt

        JOIN    Risk_Type RT                   ON R.risk_type_id = RT.risk_type_id
        LEFT JOIN

               Policy_Section_Type PS         ON RS.policy_section_type_id = PS.policy_section_type_id

        JOIN    Insurance_file ifile             ON Ifile.insurance_file_cnt =  IFR.insurance_file_cnt
        JOIN            RICTE RA                      ON RA.risk_cnt = P.risk_cnt AND RA.ri_band_id = P.ri_band AND RA.original_flag = RS.original_flag

        LEFT JOIN    RICTE RA1                           ON RA1.risk_cnt = P.risk_cnt AND RA1.ri_band_id = P.ri_band AND RA1.original_flag <> RS.original_flag

        JOIN      RI_Arrangement_Line  RAL         ON (RA.ri_arrangement_id = RAL.ri_arrangement_id)

        LEFT JOIN Treaty T                        ON RAL.treaty_id = T.treaty_id

        LEFT JOIN    Party_Insurer PIN             ON PIN.party_cnt = RAL.party_cnt

            LEFT JOIN    Party                               ON Party.party_cnt = RAL.party_cnt

       LEFT JOIN   Reinsurance_Type ReT            ON ReT.reinsurance_type_id = T.reinsurance_type_id

        LEFT JOIN Reinsurance_Type ReT2         ON Ret2.reinsurance_type_id = PIN.reinsurance_type

        WHERE   IFR.insurance_file_cnt = @insurance_file_cnt

        AND (IFR.original_risk_cnt IS NULL

                OR (IFR.original_risk_cnt IS NOT NULL AND ISNULL (IFR.is_risk_edited, 0) = 1)
                OR (IFR.status_flag IN ('C','D') AND ISNULL(IFR.is_manually_changed, 0) = 0))
        AND    (P.is_premium = 1               -- Only select perils which are 'FAP' or 'SI'
             OR P.is_sum_insured = 1
             OR IsNull(P.is_levy_tax, 0) = 1)  -- Thinh Nguyen (19/07/2002) also pick up levy tax
        AND  ISNULL(P.this_premium, 0) != 0 --Only get details for non-zero premiums       
        AND RAL.type <> 'R'
        ORDER BY P.rating_section_id ASC
END

END
  
IF @Is_PortFolioTransfer = 0    
BEGIN    
 ;WITH RICTE(risk_cnt,ri_arrangement_id,original_flag,ri_band_id,ri_model_id,pro_rata_rate )    
 AS    
      (SELECT  RA.risk_cnt, RA.RI_arrangement_id, RA.original_flag, RA.ri_band_id, RA.ri_model_id,isnull(ra.pro_rata_rate,1)   From RI_Arrangement RA    
      JOIN insurance_file_risk_link IFRL ON    
      RA.risk_cnt = IFRL.risk_cnt    
      WHERE IFRL.insurance_file_cnt = @insurance_file_cnt    
                  AND version_id = 1 )    
    
INSERT INTO #Perils (    
            Risk_cnt,    
            risk_type_id,    
            Risk_Type_Code,    
            peril_id ,    
            Peril_Description,    
            peril_type_id,    
            Peril_Type_Code,    
            policy_section_type_id,    
            Policy_Section_Type_Code,    
            peril_class_of_business_id ,    
            peril_Class_Of_Business_Code,    
            peril_annual_premium,    
            peril_this_premium_original,    
            peril_lead_commission_value,    
            peril_sub_commission_value,    
            peril_this_sum_insured,    
            peril_rating_section_id,    
            peril_ri_band,    
            peril_is_share_with_co_insurers,    
            peril_is_Levy_Tax ,    
            peril_original_flag ,    
            insurance_file_type_id,    
            ri_arrangement_id,    
            orig_ri_arrangement_id,    
            ri_share_percent,    
            ri_party_cnt,    
            ri_shortname,    
            treaty_premium_percent,    
            treaty_commission_percent,    
            treaty_sum_insured,    
            treaty_premium_value,    
            treaty_commission_value,    
            reinsurance_type_code,    
            treaty_original_sum_insured,    
            is_commission_modified,    
            ri_arrangement_line_id,    
            RI_Type,    
            line_limit,    
            [Priority],    
            RI2007, risk_pro_rata_rate , ri_pro_rata_rate    
      )    
    
        SELECT  P.risk_cnt,    
                R.risk_type_id,    
                RT.code Risk_Type_Code,    
                P.peril_id,    
                P.description,    
                P.peril_type_id,    
                PT.code Peril_Type_Code,    
                RS.policy_section_type_id,    
                PS.code Policy_Section_Type_Code,    
                P.class_of_business_id,    
                CB.code Class_Of_Business_Code,    
                P.annual_premium,    
                P.this_premium,    
                P.lead_commission_value,    
                P.sub_commission_value,    
                P.sum_insured,    
                P.rating_section_id,    
                P.ri_band,    
                RT.is_share_with_co_insurers,    
                ISNULL(P.is_levy_tax,0) Is_Levy_Tax,    
                RS.original_flag,    
                ifile.insurance_file_type_id,    
                RA.ri_arrangement_id,    
                RA1.ri_arrangement_id,    
    
                RAL.this_share_percent,    
                COALESCE(T.treaty_id,RAL.Party_cnt),    
                        COALESCE(RTRIM(T.Code),RTRIM(Party.shortname)),    
                        RAL.premium_percent,    
                        RAL.commission_percent,    
                        RAL.sum_insured,    
                        RAL.premium_value, --treaty_premium_value    
    
                        CASE WHEN RAL.type = 'T' OR RAL.type ='TFS' Then NULL    
                              ELSE RAL.commission_value end,    
    
                        COALESCE(ReT.code,ReT2.Code),    
                        NULL as Original_Sum_insured,    
                        RAL.is_commission_modified,    
                        RAL.ri_arrangement_line_id,    
                        RAL.Type,    
                        RAL.line_limit,    
                        RAL.Priority,    
               
                        CASE WHEN RAL.type = 'T' OR RAL.type ='TFS' OR RAL.type ='F' Then 0    
                     WHEN RAL.type = 'TX' OR RAL.type ='TC' OR RAL.type ='FX'  or RAL.type = 'PX'Then 1 end,    
                r.pro_rata_rate,    
             Ra.pro_rata_rate    
  
        FROM		Insurance_File_Risk_Link IFR  
        JOIN		Peril P                        ON IFR.risk_cnt = P.risk_cnt  
        JOIN		Peril_Type PT                  ON P.peril_type_id = PT.peril_type_id  
        JOIN		Rating_Section RS              ON P.rating_section_id = RS.rating_section_id  
														AND P.Risk_cnt = RS.Risk_cnt  
        JOIN		Class_Of_Business CB           ON P.class_of_business_id = CB.class_of_business_id  
        JOIN		Risk R                         ON P.risk_cnt = R.risk_cnt  
        JOIN		Risk_Type RT                   ON R.risk_type_id = RT.risk_type_id  
        LEFT JOIN   Policy_Section_Type PS         ON RS.policy_section_type_id = PS.policy_section_type_id  
        JOIN		Insurance_file ifile           ON Ifile.insurance_file_cnt =  IFR.insurance_file_cnt  
        JOIN        RICTE RA                       ON RA.risk_cnt = P.risk_cnt AND RA.ri_band_id = P.ri_band AND RA.original_flag = RS.original_flag  
        LEFT JOIN   RICTE RA1                      ON RA1.risk_cnt = P.risk_cnt AND RA1.ri_band_id = P.ri_band AND RA1.original_flag <> RS.original_flag  
        JOIN		RI_Arrangement_Line  RAL       ON (RA.ri_arrangement_id = RAL.ri_arrangement_id )  
        LEFT JOIN	Treaty T                       ON RAL.treaty_id = T.treaty_id  
        LEFT JOIN   Party_Insurer PIN              ON PIN.party_cnt = RAL.party_cnt  
        LEFT JOIN   Party                          ON Party.party_cnt = RAL.party_cnt  
        LEFT JOIN   Reinsurance_Type ReT           ON ReT.reinsurance_type_id = T.reinsurance_type_id  
        LEFT JOIN	Reinsurance_Type ReT2          ON Ret2.reinsurance_type_id = PIN.reinsurance_type  
  
        WHERE   IFR.insurance_file_cnt = @insurance_file_cnt  
        AND     (IFR.status_flag NOT IN ('U','R') )  
  
        AND (IFR.original_risk_cnt IS NULL  
                OR (IFR.original_risk_cnt IS NOT NULL AND ISNULL (IFR.is_risk_edited, 0) = 1)  
                OR (IFR.status_flag IN ('C','D') AND ISNULL(IFR.is_manually_changed, 0) = 0))  
  
        AND    (P.is_premium = 1               -- Only select perils which are 'FAP' or 'SI'  
             OR P.is_sum_insured = 1  
             OR IsNull(P.is_levy_tax, 0) = 1)  -- Thinh Nguyen (19/07/2002) also pick up levy tax  
        AND  ISNULL(P.this_premium, 0) != 0 --Only get details for non-zero premiums  
        AND RAL.type <> 'R'  
        ORDER BY P.rating_section_id ASC  
  
END    
  
  
            UPDATE P SET P.treaty_original_sum_insured = (RAL2.sum_insured)  
            FROM #Perils P  
            JOIN RI_Arrangement_Line RAL2 ON RAL2.treaty_id = P.ri_party_cnt  
            AND RAL2.ri_arrangement_id = P.orig_ri_arrangement_id  
            WHERE P.RI_Type IN('TX', 'TC', 'T', 'TFS','PX')  
  
            UPDATE P SET P.treaty_original_sum_insured = (RAL2.sum_insured)  
            FROM #Perils P  
            JOIN RI_Arrangement_Line RAL2 ON RAL2.party_cnt = P.ri_party_cnt  
            AND RAL2.ri_arrangement_id = P.orig_ri_arrangement_id  
            AND RAL2.line_limit = P.line_limit  
            WHERE P.RI_Type ='FX'  
  
            UPDATE P SET P.treaty_original_sum_insured = (RAL2.sum_insured)  
            FROM #Perils P  
            JOIN RI_Arrangement_Line RAL2 ON RAL2.party_cnt = P.ri_party_cnt  
            AND RAL2.ri_arrangement_id = P.orig_ri_arrangement_id AND RAL2.type ='F'  
            AND RAL2.priority = P.Priority  
            AND RAL2.line_limit = P.line_limit  
            WHERE P.RI_Type ='F'  
  
----------------------------------------------------------------------------------  
-- FOR spu_add_stats_details_fac and spu_add_stats_detais_treaty  
----------------------------------------------------------------------------------  
   		DECLARE  @transaction_type_id INT , @insurance_file_type_id INT
		SELECT @transaction_type_id=ISNULL(transaction_type_id,0) ,@insurance_file_type_id =ISNULL(i.insurance_file_type_id,0) FROM Stats_Folder s join Insurance_File i on i.insurance_file_cnt=s.insurance_file_cnt  where stats_folder_cnt=@stats_folder_cnt
  
		IF @transaction_type_id <> 21  OR (@transaction_type_id <> 22 and @RI2007Enabled=0)
			BEGIN 
            	Update P SET P.treaty_premium_value = P.peril_this_premium_original * P.treaty_premium_percent / 100  
            	FROM #Perils P  
            	WHERE P.peril_this_premium_original <> 0 AND P.treaty_premium_percent <> 0  
            	AND P.RI2007 = 0  
            END

            Update P SET P.treaty_premium_value = P.peril_this_premium_original * P.treaty_premium_percent / 100  
            FROM #Perils P  
            WHERE P.peril_this_premium_original <> 0 AND (P.ri_share_percent <> 0 or P.treaty_premium_percent <>0)
            AND P.RI2007 = 0  
			AND P.RI_Type = 'F'
  
            Update P SET P.treaty_premium_value = 0.0  
            FROM #Perils P  
            WHERE P.peril_this_premium_original = 0 AND P.RI2007 = 0  
  
            Update P SET  
            P.treaty_sum_insured = (CASE WHEN P.RI_Type='F' THEN P.peril_this_sum_insured * P.ri_share_percent / 100  
                                                      ELSE P.peril_this_sum_insured * P.treaty_premium_percent / 100 end)  
            FROM #Perils P    WHERE P.RI2007 = 0  
  
            -- Account for Coinsurance.  
            IF (@is_coinsured_policy > 0)  
            BEGIN  
                  UPDATE P SET treaty_premium_value =  treaty_premium_value * @retained_coins_percent,  
                        treaty_sum_insured = treaty_sum_insured * @retained_coins_percent  
                  FROM #Perils P  
            END  
  
            --If there is no change in gross premium for a risk then post zero comm  
            --@out_sum_insured_change = 0 wouldn't help because there could be a change in Treaty party  
            --specific to MTAs only  
            Update P SET P.treaty_commission_value = 0  
            FROM #Perils P  
            WHERE P.RI2007 = 0  
            AND (SELECT SUM(Round(premium, 2))  
                          FROM   ri_arrangement  
                          WHERE  risk_cnt = P.Risk_cnt  
                          GROUP  BY risk_cnt) = 0  
                  AND EXISTS (SELECT     NULL  
                                          FROM       insurance_file ifi  
                                          INNER JOIN insurance_file_type ift  
                                                ON ifi.insurance_file_type_id = ift.insurance_file_type_id  
                                          WHERE      insurance_file_cnt = @insurance_file_cnt  
                                                            AND ift.code LIKE 'MTA%')  
  
  
            Update P SET P.treaty_commission_value = P.treaty_premium_value * P.treaty_commission_percent / 100  
            FROM #Perils P  
  
            UPDATE P SET  P.treaty_sum_insured_change  = P.treaty_sum_insured  + ISNULL(P.treaty_original_sum_insured,0)  
            FROM #Perils P  
    
    
            IF @Is_PortFolioTransfer = 1 AND @RI2007Enabled = 1   
             BEGIN    
    
                UPDATE P SET  P.treaty_premium_value  = -P.treaty_premium_value, P.treaty_commission_value  = -P.treaty_commission_value    
                        FROM #Perils P    
                        JOIN RI_Arrangement RAL2 ON RAL2.ri_arrangement_id = P.orig_ri_arrangement_id WHERE RI2007 = 0  AND RAL2.original_flag=1    
    
				UPDATE P SET  P.treaty_premium_value  = P.treaty_premium_value * P.ri_pro_rata_rate / P.risk_pro_rata_rate,    
						P.treaty_commission_value  = P.treaty_commission_value* P.ri_pro_rata_rate / P.risk_pro_rata_rate    
				FROM #Perils P WHERE RI2007 = 0  
	        END    

			-- Get next stats_detail_id and set type  
			SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)  
			FROM    Stats_Detail  
			WHERE   stats_folder_cnt = @stats_folder_cnt  
  
			-- Insert the Stats Detail  ( FOR   spu_add_stats_details_treaty)  
			INSERT INTO Stats_Detail  
                  (stats_folder_cnt,  
                  stats_detail_id,  
                  stats_detail_type,  
                  risk_id,  
                  risk_type_id,  
                  risk_type_code,  
                  peril_id,  
                  peril_description,  
                  peril_type_id,  
                  peril_type_code,  
                  policy_section_type_id,  
                  policy_section_type_code,  
                  class_of_business_id,  
                  class_of_business_code,  
                  ri_party_cnt,  
                  ri_shortname,  
                  ri_party_type,  
                  ri_share_percent,  
                  ri_agreement_code,  
                  annual_premium,  
                  currency_code,  
                  currency_rate,  

                  this_premium_original,  
                  this_premium_home,  
                  this_premium_system,  
                  commission_percent,  
                  lead_commission_value_home,  
                  lead_commission_value_system,  
                  sub_commission_value_home,  
                  sub_commission_value_system,  
                  sum_insured_home,  
                  sum_insured_system,  
                  sum_insured_currency_code,  
                  sum_insured_change,  
                  is_commission_modified,  
                  ri_arrangement_line_id  
  
                  )  
              SELECT      @stats_folder_cnt,  
                          @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY P.ID) As ROW,  
                           CASE WHEN P.RI_Type ='F'THEN 'FAC'  
                                  ELSE 'TTY' end, --@out_stats_detail_type,  
                          P.Risk_cnt,  
                          P.risk_type_id,  
                          P.Risk_type_code,  
                          P.peril_id,  
                          P.peril_description,  
                          P.peril_type_id,  
                          P.peril_type_code,  
                          P.Policy_section_type_id,  
                          P.Policy_section_type_code,  
                          P.peril_class_of_business_id,  
                          P.peril_class_of_business_code,  
                          P.ri_party_cnt,  
                          P.ri_shortname,  
                           P.reinsurance_type_code,  
                           P.ri_share_percent,  
                          @out_ri_agreement_code,  
                          -P.peril_annual_premium,  
                          @out_currency_code,  
                          @out_currency_rate,  
                          -P.treaty_premium_value, --this_premium_original  
                          - ROUND(P.treaty_premium_value * @out_currency_rate , @base_decimals),  
                          - ROUND(P.treaty_premium_value * @out_currency_rate * @out_system_rate, @system_decimals),  
                          P.treaty_commission_percent,  
                          CASE WHEN P.RI_Type ='F' THEN - ROUND(P.treaty_commission_value * @out_currency_rate, @base_decimals)  
                                  ELSE - P.treaty_commission_value * @out_currency_rate end,  
                          CASE WHEN P.RI_Type ='F' THEN - ROUND(P.treaty_commission_value * @out_currency_rate * @out_system_rate, @base_decimals)  
                                  ELSE - P.treaty_commission_value * @out_currency_rate * @out_system_rate  end,  

                           -ROUND(P.peril_sub_commission_value * @out_currency_rate , @base_decimals),  
                          -ROUND(P.peril_sub_commission_value * @out_currency_rate * @out_system_rate , @base_decimals),  
                          -ROUND(P.treaty_sum_insured * @out_currency_rate , @base_decimals),  
                          -ROUND(P.treaty_sum_insured * @out_currency_rate * @out_system_rate, @base_decimals),  
                          @out_currency_code,  
                          - ROUND(P.treaty_sum_insured_change * @out_currency_rate , @Base_decimals),  
                          P.is_commission_modified,  
                          P.RI_Arrangement_Line_ID  
        FROM #Perils P  
        WHERE P.peril_is_Levy_Tax <> 1  
        AND RI2007 = 0 AND ((P.treaty_premium_value <> 0) OR (P.treaty_sum_insured <> 0) OR (P.treaty_commission_value <> 0))  
            ------------------------------------------------------------------  
  
            IF @RI2007Enabled = 1  
            BEGIN  
                  Update P SET P.peril_total_premium = (  
                  SELECT SUM(Peril.this_premium)  
                  FROM Peril JOIN      Insurance_File_Risk_Link IFR ON IFR.risk_cnt = Peril.risk_cnt  
                  JOIN      Rating_Section RS ON Peril.rating_section_id = RS.rating_section_id  
                              AND Peril.Risk_cnt = RS.Risk_cnt  
                              WHERE Peril.risk_cnt= P.Risk_cnt  
                              AND   IFR.status_flag NOT IN ('U','R') 
                              AND RS.original_flag = 0  
                              AND ( Peril.is_premium = 1 OR Peril.is_sum_insured = 1 OR IsNull(Peril.is_levy_tax, 0) = 0 )  
                  )  
                  FROM #Perils P  
                  WHERE RI2007 = 1  
  
                  Update P SET P.peril_total_original_premium = ISNULL((  
                  SELECT SUM(Peril.this_premium)  
                  FROM Peril JOIN      Insurance_File_Risk_Link IFR ON IFR.risk_cnt = Peril.risk_cnt  
                  JOIN      Rating_Section RS ON Peril.rating_section_id = RS.rating_section_id  
                              AND Peril.Risk_cnt = RS.Risk_cnt  
                              WHERE Peril.risk_cnt= P.Risk_cnt  
                              AND   IFR.status_flag NOT IN ('U','R') 
                              AND RS.original_flag = 1  
                              AND ( Peril.is_premium = 1 OR Peril.is_sum_insured = 1 OR IsNull(Peril.is_levy_tax, 0) = 0 )  
                  ),0)  
                  FROM #Perils P  
                  WHERE RI2007 = 1  
  
                  Update P SET P.treaty_premium_value = P.peril_this_premium_original * P.treaty_premium_percent / 100,  
                              P.treaty_sum_insured = (CASE WHEN P.RI_Type='FX' THEN P.peril_this_sum_insured * P.ri_share_percent / 100  
                                                                        ELSE P.peril_this_sum_insured * P.treaty_premium_percent / 100 end),  
                              P.stats_version = 13  
                  FROM #Perils P  
                  WHERE (P.peril_total_premium <> 0 AND P.peril_original_flag = 0)  
                        OR (P.peril_total_original_premium <> 0 AND P.peril_original_flag = 1)  
                  AND P.RI2007 = 1  
  
                  Update P SET  
                  P.treaty_sum_insured = (CASE WHEN P.RI_Type='FX' THEN P.peril_this_sum_insured * P.ri_share_percent / 100  
                                                            ELSE P.peril_this_sum_insured * P.treaty_premium_percent / 100 end ) ,  
                  P.stats_version = 0  
                  FROM #Perils P  
                  WHERE P.stats_version is NULL AND P.peril_is_Levy_Tax <> 1  AND P.peril_original_flag = 0  
                  AND P.RI2007 = 1  
  
                  Update P SET  
                  P.treaty_sum_insured = (CASE WHEN P.RI_Type='FX' THEN P.peril_this_sum_insured * P.ri_share_percent / 100  
                                                           ELSE P.peril_this_sum_insured * P.treaty_premium_percent / 100 end ) ,  
                                    P.stats_version = 1  
                  FROM  #Perils P  
                  WHERE P.stats_version is NULL AND P.peril_is_Levy_Tax <> 1  AND P.peril_original_flag = 1  
                  AND RI2007 =1  
  
                  Update      P SET P.stats_version = 13  
                  FROM #Perils P WHERE    P.stats_version IS NULL AND RI2007 =1  
  
                        -- Account for Coinsurance.  
                  IF (@is_coinsured_policy > 0)  
                  BEGIN  
                        UPDATE P SET treaty_premium_value =  treaty_premium_value * @retained_coins_percent,  
                              treaty_sum_insured = treaty_sum_insured * @retained_coins_percent  
                        FROM #Perils P  
                  END  
  
                  ---- calculate lead_commission from adjusted premium  
            UPDATE P SET P.treaty_commission_value =  treaty_premium_value * treaty_commission_percent / 100  
                        , treaty_sum_insured_change  = P.treaty_sum_insured  + ISNULL(P.treaty_original_sum_insured,0)  
                  FROM #Perils P  
  
  
    
                  IF @Is_PortFolioTransfer =1    
                   BEGIN    
    
                        UPDATE P SET  P.treaty_premium_value  = -P.treaty_premium_value, P.treaty_commission_value  = -P.treaty_commission_value    
                                    FROM #Perils P    
                                    JOIN RI_Arrangement RAL2 ON RAL2.ri_arrangement_id = P.orig_ri_arrangement_id    
                                    WHERE RI2007 = 1  AND RAL2.original_flag=1    
    
                        UPDATE P SET  P.treaty_premium_value  = P.treaty_premium_value * P.ri_pro_rata_rate / P.risk_pro_rata_rate,    
                                    P.treaty_commission_value  = P.treaty_commission_value* P.ri_pro_rata_rate / P.risk_pro_rata_rate    
                        FROM #Perils P WHERE RI2007 = 1    
                  END    
    
  
  
                    -- Get next stats_detail_id and set type  
        SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) ,  
                @out_stats_detail_type = 'TYX'  
        FROM    Stats_Detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
  
             -- Insert the Stats Detail  
            INSERT INTO Stats_Detail (  
                    stats_folder_cnt,  
                    stats_detail_id,  
                    stats_detail_type,  
                    risk_id,  
                    risk_type_id,  
                    risk_type_code,  
                    peril_id,  
                    peril_description,  
                    peril_type_id,  
                    peril_type_code,  
                    policy_section_type_id,  
                    policy_section_type_code,  
                    class_of_business_id,  
                    class_of_business_code,  
                    ri_party_cnt,  
                    ri_shortname,  
                    ri_party_type,  
                    ri_share_percent,  
                    ri_agreement_code,  
                    annual_premium,  
                    currency_code,  
                    currency_rate,  
  
                    this_premium_original,  
                    this_premium_home,  
                    this_premium_system,  
  
                    commission_percent,  
                    lead_commission_value_home,  
                    lead_commission_value_system,  
                    sub_commission_value_home,  
                    sub_commission_value_system,  
                    sum_insured_home,  
                    sum_insured_system,  
                    sum_insured_currency_code,  
                    sum_insured_change,  
                    is_commission_modified,  
                              stats_version,  
                              ri_arrangement_line_id  
                              )  
            SELECT  @stats_folder_cnt,  
                    @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY P.ID) As ROW,  
                              CASE WHEN P.RI_Type ='FX'THEN 'FAX'  
                                      ELSE 'TYX' end, --@out_stats_detail_type,  
                    P.Risk_cnt,  
                    P.risk_type_id,  
                    P.risk_type_code,  
                    P.peril_id,  
                    P.peril_description,  
                    P.peril_type_id,  
                    P.peril_type_code,  
                    P.policy_section_type_id,  
                    P.policy_section_type_code,  
                    P.Peril_class_of_business_id,  
                    P.Peril_class_of_business_code,  
  
                    P.ri_party_cnt,  
                    P.ri_shortname,  
                    P.reinsurance_type_code,  
                    P.ri_share_percent,  
                    NULL,  
                    -P.Peril_annual_premium,  
                    @out_currency_code,  
                    @out_currency_rate,  
  
                    -P.treaty_premium_value,  
                    - ROUND(P.treaty_premium_value *  @out_currency_rate,@base_decimals),  
                    - ROUND(P.treaty_premium_value *  @out_currency_rate * @out_system_rate,@system_decimals),  
  
                     P.treaty_commission_percent,  
                     CASE WHEN P.RI_Type ='FX' THEN - ROUND(P.treaty_commission_value * @out_currency_rate, @base_decimals)  
                                      ELSE      -P.treaty_commission_value * @out_currency_rate end,  
                      CASE WHEN P.RI_Type ='FX' THEN - ROUND(P.treaty_commission_value * @out_currency_rate * @out_system_rate, @System_decimals)  
                                      ELSE      -P.treaty_commission_value * @out_currency_rate * @out_system_rate end,  
  
                    -P.Peril_sub_commission_value * @out_currency_rate,  
                    -P.Peril_sub_commission_value * @out_currency_rate * @out_system_rate,  
  
                    - ROUND(P.treaty_sum_insured * @out_currency_rate,@base_decimals),  
                    - ROUND(P.treaty_sum_insured * @out_currency_rate * @out_system_rate,@system_decimals),  
                     @out_currency_code,  
                    -ROUND(P.treaty_sum_insured_change * @out_currency_rate, @base_decimals),  
                    P.is_commission_modified,  
                              P.stats_version,  
                              P.ri_arrangement_line_id  
  
                        FROM #Perils P  
                        WHERE ((P.treaty_premium_value <> 0) OR (P.treaty_sum_insured <> 0) OR (P.treaty_commission_value <> 0)  )  
                        AND P.peril_is_levy_tax <> 1   AND P.RI2007 = 1  
  
            END -- RI2007Enabled  
    -- ********************************************************************  
    --          SPREAD THE RI ACROSS COB WHERE PERIL PREMIUM=0  
    -- ********************************************************************  
      EXEC spu_add_stats_details_spread_ri @stats_folder_cnt, 'FAX'  
  
      EXEC spu_add_stats_details_spread_ri @stats_folder_cnt, 'TYX'  
  
    -- ********************************************************************  
    --            TAX RECORDS FOR TREATY PREMIUM AND COMMISSION  
    -- ********************************************************************  
  
Create Table #Tax  
      (  
            tax_band_id INT,  
            tax_Premium NUMERIC(19,4),  
            tax_percentage FLOAT,  
            tax_value NUMERIC(19,4),  
            tax_is_value TINYINT,  
            tax_Band_code VARCHAR(10),  
            risk_cnt INT,  
            risk_type_id INT,  
            risk_type_code VARCHAR(10),  
            transType VARCHAR(10),  
            ri_party_cnt INT,  
            is_withholding_tax TINYINT,  
            ri_arrangement_line_id INT,  
            tax_value_home NUMERIC(19,4),  
            tax_value_system NUMERIC(19,4),  
            Multiplier INT,  
            RI_Type VARCHAR(5),
	    Original_flag tinyint   
      )  
  
      INSERT INTO #Tax  
      (  
            tax_band_id,  
            tax_Premium,  
            tax_percentage,  
            tax_value ,  
            tax_is_value,  
            tax_Band_code,  
            risk_cnt,  
            risk_type_id,  
            risk_type_code,  
            transType,  
            ri_party_cnt ,  
            is_withholding_tax ,  
            ri_arrangement_line_id,  
	    RI_Type,
	    Original_flag  
            )  
  
      SELECT  tc.tax_band_id,  
                tc.premium,  
                tc.percentage,  
                tc.value,  
                tc.is_value,  
                tb.code,  
                        tc.risk_cnt,  
                R.risk_type_id,  
                RT.code,  
                TC.transtype,  
                tc.ri_party_cnt,  
                TG.is_withholding_tax,  
                        RAL.ri_arrangement_line_id,  
                RAL.type,
		RA.original_flag  
                  --    CASE WHEN RAL.Type = 'TX' OR RAL.Type = 'TC' THEN 1  
                              -- WHEN RAL.TYPE = 'T' OR RAL.TYPE = 'F' OR  RAL.TYPE = 'R' OR RAL.TYPE = 'TFS'   THEN 0 end  
  
        FROM    Tax_Calculation TC  
        JOIN    tax_band TB                 ON tb.tax_band_id = tc.tax_band_id  
        JOIN    Tax_Type TT                 ON tt.tax_type_id = tb.tax_type_id  
        JOIN    Tax_Group TG                ON tg.tax_group_id = tc.tax_group_id  
        JOIN    Risk R                      ON R.risk_cnt = TC.risk_cnt  
        JOIN    Risk_Type RT                ON RT.risk_type_id = R.risk_type_id  
        JOIN    Insurance_File_Risk_Link RL ON RL.risk_cnt = TC.risk_cnt  
                                           AND RL.insurance_file_cnt = TC.insurance_file_cnt  
        LEFT JOIN    ri_arrangement_line RAL  
                                            ON RAL.ri_arrangement_line_id= TC.ri_arrangement_line_ID  
        JOIN         ri_arrangement Ra     ON RAL.ri_arrangement_id= RA.ri_arrangement_id  
        WHERE   TC.insurance_file_cnt = @insurance_file_cnt  
        AND     TC.transtype IN ('TTRITP','TTRITC','TTRIFP','TTRIFC')  
        AND     R.is_risk_selected = 1  
        AND     RL.status_flag NOT IN ('U','R') AND (RL.original_risk_cnt IS NULL  
                  OR (RL.original_risk_cnt IS NOT NULL AND ISNULL (RL.is_risk_edited, 0) = 1))  
		AND     RA.version_id = (SELECT max(version_id) from ri_arrangement Where risk_cnt = R.risk_cnt  AND (effective_date = @effective_ri_date OR effective_date IS NULL OR ISNULL(@RI2007Enabled, 0) = 0))
        -- AND     (RAL.Type = 'FX' OR RAL.Type = 'TX' OR RAL.Type = 'TC')  
  
        DECLARE @TaxRowCount INT  
        SELECT @TaxRowCount = Count(*) FROM #Tax  
        IF  @TaxRowCount > 0  
            BEGIN  
                  UPDATE T SET T.tax_value_home  = T.tax_value * @out_currency_rate  
                  FROM #Tax T  
  
                  UPDATE T SET T.Multiplier  = 1  
                  FROM #Tax T WHERE ISNULL(T.is_withholding_tax,0)  = 0  
  
                  UPDATE T SET T.Multiplier  = - 1  
                  FROM #Tax T WHERE ISNULL(T.is_withholding_tax,0)  <> 0  
  
                  UPDATE T SET T.tax_value   = - 1 * tax_value  
                  FROM #Tax T WHERE transtype NOT IN ( 'TTRITP','TTRIFP' )  
  
                  UPDATE T SET T.tax_value_system  = T.tax_value_home * @out_system_rate  
                  FROM #Tax T  
  
                    -- Get next stats_detail_id and set type  
                  SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0)  
                  FROM    Stats_Detail  
                  WHERE   stats_folder_cnt = @stats_folder_cnt  
  
         -- Insert the Stats Detail  
            INSERT INTO Stats_Detail (  
                stats_folder_cnt,  
                stats_detail_id,  
                stats_detail_type,  
                risk_id,  
                risk_type_id,  
                risk_type_code,  
                ri_share_percent,  
                this_premium_original,  
                this_premium_home,  
                this_premium_system,  
                currency_rate,  
                currency_code,  
                tax_type_id,  
                tax_type_code,  
                tax_value,  
                ri_party_cnt,  
                ri_arrangement_line_id  
            )  
            SELECT  
					@stats_folder_cnt,  
					@out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY T.risk_cnt),  
					CASE  
						WHEN T.TransType = 'TTRIFP' AND T.RI_Type IN('F','FX')THEN 'TFP'  
						WHEN T.TransType <> 'TTRIFP' AND T.RI_Type IN('F','FX') THEN 'TFC'  
						WHEN T.TransType = 'TTRITP' AND (T.RI_Type NOT IN('F','FX')) THEN 'TTP'  
						ELSE 'TTC' end,  
					T.risk_cnt,  
					T.risk_type_id,  
					t.risk_type_code,  
					t.tax_percentage,  
					-T.tax_value*multiplier,  
					-T.tax_value_home*multiplier,  
					-T.tax_value_system*multiplier,  
					@out_currency_rate,  
					@out_currency_code,  
					T.tax_band_id,  
					T.tax_band_code,  
					-T.tax_value * multiplier,  
					T.ri_party_cnt,  
					T.RI_Arrangement_Line_ID  
					FROM #Tax T  
					WHERE T.tax_value <> 0  

                             -- Get next stats_detail_id and set type  
                SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1  
                FROM    Stats_Detail  
                WHERE   stats_folder_cnt = @stats_folder_cnt  
  
                        INSERT INTO Stats_Detail (  
                stats_folder_cnt,  
                stats_detail_id,  
                stats_detail_type,  
                risk_id,  
                risk_type_id,  
                risk_type_code,  
                ri_shortname,  
                ri_share_percent,  
                this_premium_original,  
                this_premium_home,  
                this_premium_system,  
                currency_rate,  
                currency_code,  
                tax_type_id,  
                tax_type_code,  
                tax_value,  
                ri_arrangement_line_id   
                        )  
            SELECT  
                @stats_folder_cnt,  
                @out_stats_detail_id + ROW_NUMBER() OVER(ORDER BY t.risk_cnt) ,  
                'TAN',  
                T.risk_cnt,  
                T.risk_type_id,  
                T.risk_type_code,  
                'NOTA' + RTRIM(T.tax_band_code),  
                T.tax_percentage,  
                T.tax_value * multiplier,  
                T.tax_value_home * multiplier,  
                T.tax_value_system * multiplier,  
                @out_currency_rate,  
                @out_currency_code,  
                T.tax_band_id,  
				T.tax_band_code,  
				T.tax_value * multiplier,  
				T.RI_Arrangement_Line_ID  
			FROM #Tax T  WHERE T.tax_value<>0  
                  END  
