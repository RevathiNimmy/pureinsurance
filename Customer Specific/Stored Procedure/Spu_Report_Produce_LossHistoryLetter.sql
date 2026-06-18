EXECUTE DDLDropProcedure 'Spu_Report_Produce_LossHistoryLetter'
GO

CREATE     PROCEDURE Spu_Report_Produce_LossHistoryLetter
 @GC_PartyCNT INT,  
 @PartyCNT  INT = NULL,  
 @Comments  VARCHAR(255) = '',  
 @InsuranceFileCNT INT = 0,  
 @IsLossHistoryOnly bit = 1,  
 @NumOFYears int =0      
AS  
BEGIN  
SET NOCOUNT ON  
    DECLARE @SQL NVARCHAR(4000) ,  
        @print VARCHAR(255) ,  
        @gis_data_model_code VARCHAR(10) ,  
        @object_name VARCHAR(70) ,  
        @property_name VARCHAR(70) ,  
        @table_name VARCHAR(70) ,  
        @column_name VARCHAR(70) ,  
        @policy_binder_id INTEGER ,  
        @search_value VARCHAR(255) ,  
        @CertHolderName VARCHAR(50) ,  
        @CertHolderAddress1 VARCHAR(50) ,  
        @CertHolderAddress2 VARCHAR(50) ,  
        @CertHolderAddress3 VARCHAR(50) ,  
        @CertHolderAddress4 VARCHAR(50) ,  
        @CertHolderPostalCode VARCHAR(50) ,  
        @ClientName VARCHAR(50) ,  
        @Identity INT ,  
        @Risk_Type VARCHAR(50) ,  
        @Col_Policy_Binder VARCHAR(50) ,  
        @ModelObjectName VARCHAR(50) ,  
        @Risk_Cnt INT ,  
        @RiskType_Code VARCHAR(50) ,  
        @IdentityRisk INT ,  
        @LicenseNum_PropertyName VARCHAR(50) ,  
        @License_table_name VARCHAR(50) ,  
        @LicenseNum_Column_Name VARCHAR(50) ,  
        @Current_Risk INT ,  
        @Current_Policy_Binder INT ,  
        @Current_ClaimId INT ,  
        @Current_ClaimRisk_Id INT ,  
        @IsClaimBuilderConfigured INT ,  
        @IsProductBuilderConfigured INT ,  
        @CBRisk_Type INT ,  
        @PartyType VARCHAR(10) ,  
        @Current_Claimnumber VARCHAR(50) ,  
        @Closed_date DATETIME ,  
        @Loss_Description VARCHAR(2000) ,  
        @License_Prefix_id INT ,  
        @License_Prefix VARCHAR(50) ,  
        @risk_start_date DATETIME ,  
        @risk_end_date DATETIME  
		DECLARE @NoInserted INT  
		DECLARE @Closed_Date_filter DATETIME  
		
	   IF  ISNULL(@NumOFYears,0) =0       
			BEGIN  
				set @Closed_Date_filter ='1900-01-01 00:00:00.000'  
			END   
		ELSE  
			BEGIN  
				SELECT  @Closed_Date_filter = GETDATE()            
				SELECT  @Closed_Date_filter=DATEADD(yy, -1*@NumOFYears, @Closed_Date_filter)  
			END  
      	
      
    SET @IsClaimBuilderConfigured = 0  
      
    SET @IsProductBuilderConfigured = 0  
      
    EXEC @IsClaimBuilderConfigured = DDLExistsTable 'cb_policy_binder'  
      
    IF @IsClaimBuilderConfigured = 1   
        EXEC @IsClaimBuilderConfigured = DDLExistsTable 'CB_CLAIM_LEVEL'  
          
    EXEC @IsProductBuilderConfigured = DDLExistsTable 'pb_policy_binder'  
      
    --Get the Client name and address  
    SELECT  @CertHolderName = P.resolved_name ,  
            @CertHolderAddress1 = A.Address1 ,  
            @CertHolderAddress2 = A.Address2 ,  
            @CertHolderAddress3 = A.Address3 ,  
            @CertHolderAddress4 = A.Address4 ,  
            @CertHolderPostalCode = A.Postal_Code  
    FROM    Party P  
            INNER JOIN Party_Address_Usage PAU ON P.party_cnt = PAU.party_cnt  
            INNER JOIN Address A ON A.Address_cnt = PAU.Address_cnt  
    WHERE   p.party_cnt = @GC_PartyCNT   
      
    SELECT  @ClientName = P.resolved_name ,  
            @PartyType = PT.Code  
    FROM    Party P  
            INNER JOIN Party_Address_Usage PAU ON P.party_cnt = PAU.party_cnt  
            INNER JOIN Address A ON A.Address_cnt = PAU.Address_cnt  
            INNER JOIN Party_Type PT ON P.Party_Type_Id = PT.Party_Type_Id  
    WHERE   p.party_cnt = @PartyCNT  
       
    CREATE TABLE #Matches_Found  
        (  
          ID INT IDENTITY ,  
          policy_binder_id INT ,  
          object_name VARCHAR(70) ,  
          property_name VARCHAR(70) ,  
          party_cnt INT ,  
          risk_cnt INT ,  
          risk_type_code VARCHAR(50) ,  
          risk_type_desc VARCHAR(100)  
        )  
          
    CREATE TABLE #Claim_Details  
        (  
          ID INT IDENTITY ,  
          risk_cnt INT ,  
          Gis_Policy_Link_id INT ,  
          CB_Policy_Binder_id INT ,  
          Claim_Id VARCHAR(50) ,  
          Date_Event DATETIME ,  
          Settlement VARCHAR(2000) ,  
          Date_Closed DATETIME ,  
          Loss_Desc VARCHAR(2000) ,  
          insurance_file_cnt INT ,  
          Claim_Number VARCHAR(50)  
        )  
          
    CREATE TABLE #PB_Risks  
        (  
          ID INT IDENTITY ,  
          Pb_Policy_binder_id INT ,  
          party_cnt INT ,  
        risk_cnt INT ,  
          cover_start_date DATETIME ,  
          cover_end_date DATETIME ,  
          License_Number VARCHAR(50)  
        )  
          
    CREATE TABLE #LegacyClaim_Details  
        (  
          ID INT IDENTITY ,  
          Pb_Policy_binder_id INT ,  
          risk_cnt INT ,  
          claim_Number VARCHAR(50) ,  
          Date_Event DATETIME ,  
          Settlement VARCHAR(2000) ,  
          Date_Closed DATETIME ,  
          Loss_Desc VARCHAR(2000) ,  
          Loss_Date DATETIME  
        )  
  
 CREATE TABLE #License  
        (   
  License_Prefix_id INT ,  
        License_Num VARCHAR(50) ,  
        Original_Start_Date DATETIME  
        )  
          
    CREATE TABLE #AssociatedParties ( Party_cnt INT )  
      
    CREATE TABLE #Risks  
        (  
          ID INT IDENTITY ,  
          Policy_Binder_Id INT ,  
          Risk_cnt INT ,  
          Insurance_File_cnt INT  
        )  
          
    INSERT  INTO #AssociatedParties  
            ( Party_cnt )  
    VALUES  ( @PartyCNT )  
      
    DECLARE c_search_properties CURSOR FAST_FORWARD FOR  
    SELECT  object_name,  
   table_name,  
   property_name,  
   column_name,  
   GDM.Code  
    FROM    gis_object o  
    INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)  
    INNER JOIN gis_data_model GDM ON (GDM.gis_data_model_id = O.gis_data_model_id)  
    WHERE   Specials_Type = 3  
    AND  GDM.gis_data_model_type_id=1  
    AND o.object_name NOT IN ('ERS_LIST', 'RES_LIST', 'PHY_LOCTEN')  
      
    OPEN c_search_properties  
    FETCH NEXT FROM c_search_properties  
 INTO    @object_name,  
   @table_name ,  
   @property_name,  
   @column_name,  
   @gis_data_model_code  
     
    WHILE ( @@FETCH_STATUS = 0 )   
        BEGIN --*  
             
   SELECT  @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, party_cnt, risk_cnt,risk_type_code,risk_type_desc)'  
            SELECT  @SQL = @SQL + ' SELECT ' + RTRIM(@gis_data_model_code) + '_policy_binder_id, ''' + @object_name + ''' , ''' + @property_name + ''' ,' + @column_name  
            SELECT  @SQL = @SQL + ', (SELECT risk_id FROM GIS_POLICY_LINK GPL WHERE GPL.gis_policy_link_id = ' + RTRIM(@gis_data_model_code) + '_policy_binder_id )'  
            SELECT  @SQL = @SQL + ', (SELECT RT.code FROM risk_type RT INNER JOIN Risk R ON R.risk_type_id = RT.risk_type_id ' + 'INNER JOIN GIS_POLICY_LINK GPL ON GPL.risk_id = r.risk_cnt'  
            SELECT  @SQL = @SQL + ' WHERE GPL.gis_policy_link_id = ' + RTRIM(@gis_data_model_code) + '_policy_binder_id)'  
            SELECT  @SQL = @SQL + ', (SELECT RT.description FROM risk_type RT INNER JOIN Risk R ON R.risk_type_id = RT.risk_type_id ' + 'INNER JOIN GIS_POLICY_LINK GPL ON GPL.risk_id = r.risk_cnt WHERE GPL.gis_policy_link_id = ' + RTRIM(@gis_data_model_code) + '_policy_binder_id)'  
            SELECT  @SQL = @SQL + ' FROM ' + RTRIM(@table_name)  
            SELECT  @SQL = @SQL + ' INNER JOIN #AssociatedParties AP ON ' + RTRIM(@table_name) + '.' + RTRIM(@column_name) + '= AP.Party_cnt'          
     
            EXEC(@SQL)  
              
            SELECT  @NoInserted = @@ROWCOUNT  
            SELECT  @Identity = @@identity  
              
            IF @Identity <> 0   
                BEGIN --**  
       
                    SELECT  @SQL = 'INSERT INTO #Risks(Risk_Cnt,Insurance_File_Cnt,Policy_Binder_Id) '  
                    SELECT  @SQL = @SQL + ' SELECT  ifrl.risk_cnt,ifrl.Insurance_File_cnt,gpl.gis_policy_link_id FROM ' + @table_name  
                    SELECT  @SQL = @SQL + ' INNER JOIN  GIS_POLICY_LINK GPL ON ' + RTRIM(@gis_data_model_code) + '_policy_binder_id = GPL.Gis_policy_Link_id'  
                    SELECT  @SQL = @SQL + ' INNER JOIN insurance_file_risk_link ifrl ON GPL.risk_id=ifrl.risk_cnt'  
                    SELECT  @SQL = @SQL + ' INNER JOIN #AssociatedParties AP ON ' + RTRIM(@table_name) + '.' + RTRIM(@column_name) + '= AP.Party_cnt'  
                      
                    EXEC(@SQL)  
              
    END --**  
      
   FETCH NEXT FROM c_search_properties  
   INTO    @object_name,  
     @table_name ,  
     @property_name,  
     @column_name,  
     @gis_data_model_code  
       
  END --*  
    
  --Test Code  
  --SELECT 'MATCHES FOUND'  
  --SELECT * FROM #Matches_Found  
  --End Test Code  
    
    CLOSE c_search_properties  
    DEALLOCATE c_search_properties  
  
 --Test Code  
 --SELECT 'RISKS'  
    --SELECT R.*, INS.insurance_ref, INS.policy_version FROM #Risks R LEFT JOIN Insurance_File INS ON R.insurance_file_cnt = INS.insurance_file_cnt  
    --End Test Code  
      
    SELECT  @NoInserted = @@ROWCOUNT  
    IF @NoInserted > 0   
        SELECT  @risk_start_date = inception_date  
        FROM    #Risks t_r  
                INNER JOIN risk r ON t_r.risk_cnt = r.risk_cnt  
                INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt  
        WHERE   ifrl.Insurance_File_Cnt = @InsuranceFileCNT  
                AND ifrl.status_flag <> 'D'  
        ORDER BY inception_date DESC  
          
          
  SELECT  @risk_end_date = inception_date  
  FROM    #Risks t_r  
    INNER JOIN risk r ON t_r.risk_cnt = r.risk_cnt  
    INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt  
  WHERE   ifrl.Insurance_File_Cnt = @InsuranceFileCNT  
    AND ifrl.status_flag = 'D'  
           
      
    IF @IsClaimBuilderConfigured = 1   
        BEGIN --***  
            INSERT  #Claim_Details  
       (risk_cnt ,  
     claim_id ,  
                    Gis_Policy_Link_id ,  
                    CB_Policy_Binder_id ,  
                    Settlement ,  
                    Date_Closed ,  
                    Claim_Number ,  
                    Loss_desc  
                    )  
            SELECT  m.risk_cnt ,  
     clm.claim_id ,  
                    gpl.Gis_Policy_Link_Id ,  
                    cbl.cb_policy_binder_id ,  
                    gis_u_detail.description ,  
                    CASE WHEN is_closed_check_status = 1 THEN cbcl.Closed_Date --clm.Claims_status_date  
     ELSE NULL  
                    END ,  
                    clm.Claim_number ,  
                    Clm.Description  
            FROM    claim clm  
     INNER JOIN claim_status cstat ON clm.claim_status_id = cstat.claim_status_id  
     INNER JOIN ( SELECT MAX(claim_id) AS claim_id ,  
          MAX(version_id) AS version_id ,  
          base_claim_id  
         FROM claim  
         WHERE is_dirty = 0  
         GROUP BY base_claim_id  
          ) claim_versions ON clm.claim_id = claim_versions.claim_id  
     INNER JOIN gis_policy_link gpl ON clm.claim_id = gpl.claim_id  
     INNER JOIN cb_policy_binder cbl ON cbl.cb_policy_binder_id = gpl.Gis_Policy_Link_Id  
     INNER JOIN CB_CLAIM_LEVEL cbcl ON cbcl.cb_policy_binder_id = gpl.Gis_Policy_Link_Id  
     INNER JOIN GIS_User_Def_Detail gis_u_detail ON gis_u_detail.GIS_User_Def_Detail_Id = cbcl.Settlement_Code  
     INNER JOIN #Matches_Found m ON clm.risk_type_id = m.risk_cnt  
     LEFT JOIN Progress_status ps ON clm.progress_status_id = ps.progress_status_id AND is_closed_check_status = 1  
                    WHERE   cbcl.File_Type IN ( 0, 471, 474, 472, 475 )  
  END --***  
  
 --TEST CODE  
 --SELECT 'CLAIM DETAILS'  
 --SELECT * FROM #Claim_Details  
 --TEST CODE  
      
      
    DECLARE @TempRiskCnt INT ,  
   @Coverage_Eff_date DATETIME ,  
   @Coverage_Exp_date DATETIME ,  
   @License_Num VARCHAR(50)  
     
    SELECT  @TempRiskCnt = ISNULL(MAX(Risk_Cnt), 0) + 1  
    FROM    #Matches_Found  
      
    --Find the Legacy Claims from the Party_Builder records  
    DECLARE @Legacy_claim_GPL_Id INT ,  
   @PB_policy_binder_ID INT  
      
    DECLARE c_Legacy_claims CURSOR FAST_FORWARD FOR  
    SELECT gis_policy_link_id  
    FROM gis_policy_link GPL  
    WHERE GPL.party_cnt = @PartyCNT  
    IF @IsProductBuilderConfigured = 1   
        BEGIN --*  
   OPEN c_Legacy_claims  
            FETCH NEXT FROM c_Legacy_claims  
   INTO @Legacy_claim_GPL_Id  
            WHILE ( @@FETCH_STATUS = 0 )   
                BEGIN --**  
                    SELECT  @PB_Policy_binder_id = pb.PB_policy_binder_ID ,  
                            @RiskType_Code = gs.code ,  
                            @CBRisk_Type = pbp.Risk_Type  
                    FROM    GIS_Screen GS  
                            INNER JOIN Party_Type PT ON GS.gis_screen_id = PT.gis_screen_id  
                            INNER JOIN Party P ON PT.party_type_id = P.party_type_id  
                            INNER JOIN gis_policy_link gpl ON gpl.party_cnt = p.party_cnt  
                            INNER JOIN PB_policy_binder PB ON PB.gis_policy_Link_Id = gpl.gis_policy_Link_Id  
                            INNER JOIN PB_PERCLNT pbp ON pb.PB_Policy_binder_id = pbp.PB_policy_binder_ID  
                    WHERE   P.party_cnt = @PartyCNT  
                            AND PB.gis_policy_Link_Id = @Legacy_claim_GPL_Id  
                            AND GS.is_deleted = 0  
                      
                    IF @RiskType_Code = 'PB_PHY' AND @CBRisk_Type = 469   
                        BEGIN --Physcians  
                            SELECT  @Coverage_Eff_date = Original_Start_Date ,  
                                    @ClientName = FName ,  
                                    @License_Num = License_Num  
                            FROM    PB_PHY PPHY  
                            WHERE   PPHY.PB_Policy_binder_id = @PB_Policy_binder_id  
                              
                            SELECT TOP 1 @Coverage_Exp_date = Coverage_Exp_Date  
                            FROM    PB_PHY_CLIENT PPC  
                            WHERE   PPC.PB_Policy_binder_id = @PB_Policy_binder_id  
       AND  Coverage_Exp_Date <> '2029-12-31 00:00:00.000'  
                            ORDER BY Coverage_Exp_Date DESC  
  
                            INSERT  INTO #PB_Risks  
                            VALUES  ( @PB_Policy_binder_id, @PartyCNT,  
                                      @TempRiskCnt, @Coverage_Eff_date,  
                                      @Coverage_Exp_date, @License_Num )  
  
                            INSERT  INTO #LegacyClaim_Details  
                            SELECT  pb_policy_binder_id ,  
                                    NULL ,  
                                    Claim_Num ,  
                                    Loss_Date ,  
                                    Settlement ,  
                                    Close_Date ,  
                                    Loss_Desc ,  
                                    Loss_Date  
                            FROM    PB_PHY_CLIAMS  
                            WHERE   PB_Policy_binder_id = @PB_Policy_binder_id  
                                    AND ClaimType IN ('Suit Closed', 'Open Suit', 'Closed Claim', 'Open Claim')    
                                   
                                   
                        END --Physcians  
                    ELSE   
                        IF ( @RiskType_Code = 'PB_PHY' AND @CBRisk_Type = 470 )   
                            BEGIN --Ancillaries  
                                SELECT  @Coverage_Eff_date = Original_Start_Date ,  
                                        @ClientName = FName ,  
                                        @License_Num = License_Num  
                                FROM    PB_ANC PANC  
                                WHERE   PANC.PB_Policy_binder_id = @PB_Policy_binder_id  
                                  
                                SELECT TOP 1  
                                        @Coverage_Exp_date = Coverage_Exp_Date  
                                FROM    PB_ANC_CLIENT PPC  
                                WHERE   PPC.PB_Policy_binder_id = @PB_Policy_binder_id  
        --AND  Coverage_Exp_Date <> '2029-12-31 00:00:00.000'  
                                ORDER BY Coverage_Exp_Date DESC  
                                  
                                INSERT  INTO #PB_Risks  
                                VALUES  ( @PB_Policy_binder_id, @PartyCNT,  
                                          @TempRiskCnt, @Coverage_Eff_date,  
                                          @Coverage_Exp_date, @License_Num )  
                                  
                                INSERT  INTO #LegacyClaim_Details  
                                SELECT  pb_policy_binder_id ,  
                                        NULL ,  
                                        Claim_Num ,  
                                        Loss_Date ,  
                                        Settlement ,  
                                        Close_Date ,  
                                        Loss_Desc ,  
                                        Loss_Date  
                                FROM    PB_ANC_CLAIMS  
                                WHERE   PB_Policy_binder_id = @PB_Policy_binder_id  
                                        AND ClaimType IN ('Suit Closed', 'Open Suit', 'Closed Claim', 'Open Claim')    
                                        
                            END --Ancillaries  
                        ELSE   
                            IF @RiskType_Code = 'PB_MFAC'   
                                BEGIN --Corporations  
                                    SELECT  @Coverage_Eff_date = Original_Start_Date ,  
                                            @License_Num = License_Number ,  
                                            @ClientName = Name  
                                    FROM    PB_MFAC PMFAC  
                                    WHERE   PMFAC.PB_Policy_binder_id = @PB_Policy_binder_id  
                                      
                                    SELECT TOP 1  
                                            @Coverage_Exp_date = Coverage_Exp_Date  
                                    FROM    PB_MFAC_CLIENT PMFACCLIENT  
                                    WHERE   PMFACCLIENT.PB_Policy_binder_id = @PB_Policy_binder_id  
                                    ORDER BY Coverage_Exp_Date DESC  
                                      
                                    INSERT  INTO #PB_Risks  
                                    VALUES  ( @PB_Policy_binder_id,  
                                              @PartyCNT, @TempRiskCnt,  
                                              @Coverage_Eff_date,  
                                              @Coverage_Exp_date,  
                                              @License_Num )  
                                                
                                    INSERT  INTO #LegacyClaim_Details  
                                    SELECT  pb_policy_binder_id ,  
                                            NULL ,  
                                            Claim_Num ,  
                                            Loss_Date ,  
                                            Settlement ,  
                                            Close_Date ,  
                                            Loss_Notes ,  
                                            Loss_Date  
                                    FROM    PB_MFAC_CLAIMS  
                                    WHERE   PB_Policy_binder_id = @PB_Policy_binder_id  
                                            AND ClaimType IN ('Suit Closed', 'Open Suit', 'Closed Claim', 'Open Claim')    
                                            
                                END --Corporations  
                      
                    UPDATE  #LegacyClaim_Details  
                    SET     risk_cnt = @TempRiskCnt  
                    WHERE   Pb_Policy_binder_id = @PB_Policy_binder_id  
                      
                    SET @TempRiskCnt = @TempRiskCnt + 1  
                      
                    FETCH NEXT FROM c_Legacy_claims  
     INTO @Legacy_claim_GPL_Id  
                END --**  
                  
            CLOSE c_Legacy_claims  
            DEALLOCATE c_Legacy_claims  
        END --*  
          
        --test code  
  --SELECT 'LEGACY CLAIM DETAILS'  
  --SELECT * FROM #LegacyClaim_Details  
        --SELECT @ClientName AS "client", @PB_Policy_binder_id AS "policy_binder_id", @RiskType_Code AS "risk type code", @CBRisk_Type AS "cbrisk type", @Coverage_Eff_date AS "coverage eff date", @Coverage_Exp_date AS "coverage exp date"  
        --SELECT @risk_start_date AS "risk start date"  
        --SELECT @risk_end_date AS "risk end date"  
        --end test code  
          
    CREATE TABLE #Record_Found  
        ( risk_cnt INT ,  
          claim_Number VARCHAR(50) ,  
          settlement VARCHAR(50) ,  
          date_closed DATETIME ,  
          Object_Name VARCHAR(70) ,  
          Property_Name VARCHAR(70) ,  
          Party_Cnt INT ,  
          cover_start_date DATETIME ,  
          expiry_date DATETIME ,  
          CertiHolderName VARCHAR(50) ,  
          CertiHolderAddress1 VARCHAR(50) ,  
          CertiHolderAddress2 VARCHAR(50) ,  
          CertiHolderAddress3 VARCHAR(50) ,  
          CertiHolderAddress4 VARCHAR(50) ,  
          CertiHolderPostalCode VARCHAR(50) ,  
          ClientName VARCHAR(50) ,  
          Loss_From_date DATETIME ,  
          Loss_Desc VARCHAR(2000) ,  
          Coverage_Eff_date DATETIME ,  
          Coverage_Exp_date DATETIME ,  
          License_Num VARCHAR(50)  
        )  
  
    --BEGIN --*  
        IF @risk_end_date IS NULL   
            SELECT  @risk_end_date = expiry_date  
            FROM    insurance_file ifi  
            WHERE   ifi.Insurance_File_Cnt = @InsuranceFileCNT  
              
        IF @risk_end_date IS NULL   
            SET @risk_end_date = '1900-01-01 00:00:00.000'  
            

              
        INSERT  INTO #Record_Found  
                --SELECT  DISTINCT  
                --        CD.risk_cnt ,  
                --        CD.claim_Number AS claim_Number ,  
                --        CD.settlement ,  
                --        CD.date_closed ,  
                --        m.Object_Name ,  
                --        m.Property_Name ,  
                --        m.Party_Cnt ,  
                --        @risk_start_date ,  
                --        CASE WHEN @risk_end_date < @Coverage_Exp_date  
                --             THEN @Coverage_Exp_date  
                --             ELSE @risk_end_date  
                --        END ,  
                --        @CertHolderName CertiHolderName ,  
                --        @CertHolderAddress1 CertiHolderAddress1 ,  
                --        @CertHolderAddress2 CertiHolderAddress2 ,  
                --        @CertHolderAddress3 CertiHolderAddress3 ,  
                --        @CertHolderAddress4 CertiHolderAddress4 ,  
                --        @CertHolderPostalCode CertiHolderPostalCode ,  
                --        @ClientName ClientName ,  
                --        C.Loss_From_date ,  
                --        CD.Loss_Desc ,  
                --        ISNULL(@Coverage_Eff_date,  
                --               ISNULL(@risk_start_date, '')) Coverage_Eff_date ,  
                --        ISNULL(@Coverage_Exp_date, '') Coverage_Exp_date ,  
                --        @License_Num License_Num  
                --FROM    gis_policy_link gpl  
                --INNER JOIN #Matches_Found m ON gpl.gis_policy_link_id = m.policy_binder_id  
                --INNER JOIN #Claim_Details CD ON CD.risk_cnt = m.risk_cnt  
                --INNER JOIN Claim C ON C.Claim_Id = CD.Claim_Id  
                --UNION ALL  
                SELECT  CD.Risk_Cnt ,  
                        CD.claim_Number ,  
                        CD.settlement ,  
                        CD.date_closed ,  
                        NULL Object_name ,  
                        NULL Property_Name ,  
                        @PartyCNT ,  
                        PBR.cover_start_date cover_start_date ,  
                        CASE WHEN @risk_end_date < PBR.Cover_end_date  
                             THEN PBR.Cover_end_date  
                             ELSE @risk_end_date  
                        END expirt_date ,  
                        @CertHolderName CertiHolderName ,  
                        @CertHolderAddress1 CertiHolderAddress1 ,  
                        @CertHolderAddress2 CertiHolderAddress2 ,  
                        @CertHolderAddress3 CertiHolderAddress3 ,  
                        @CertHolderAddress4 CertiHolderAddress4 ,  
                        @CertHolderPostalCode CertiHolderPostalCode ,  
                        @ClientName ClientName ,  
                        CD.Loss_date ,  
                        CD.Loss_Desc ,  
                        ISNULL(@Coverage_Eff_date, '') Coverage_Eff_date ,  
                        ISNULL(@Coverage_Exp_date, '') Coverage_Exp_date ,  
                        @License_Num License_Num  
                FROM    #PB_Risks PBR  
                INNER JOIN #LegacyClaim_Details CD ON CD.Risk_Cnt = PBR.Risk_Cnt  
                WHERE date_closed >=@Closed_Date_filter
                
                
    --END --*  
    --Test code  
 --SELECT 'RECORD FOUND'  
    --SELECT  * FROM #Record_Found ORDER BY loss_from_date DESC  
    --end test code  
  
  
      
    DECLARE @Record_Found_Count INT   
    SET @Record_Found_Count = (SELECT COUNT(*) FROM #Record_Found)  
      
    IF @Record_Found_Count = 0 --( SELECT COUNT(*) FROM #Record_Found ) = 0   
        BEGIN --*  
                                          
            --Test code  
            --SELECT @RiskType_Code AS "Risk Type Code", @CBRisk_Type AS "CBRisk Type Code"  
   --End Test Code  
  
   DECLARE @tablename nvarchar(50)  
     
   SET @tablename = (SELECT CASE @CBRisk_Type  
      WHEN 469 THEN 'PB_PHY'  
      WHEN 470 THEN 'PB_ANC'  
      ELSE 'PB_MFAC'   
      END)  
     
   --Test code  
   --SELECT @tablename as 'table name'     
            --End Test Code  
                  
            DECLARE c_search_license_num CURSOR FAST_FORWARD FOR  
            SELECT  DISTINCT  
    object_name,  
    table_name,  
    GDM.Code  
            FROM gis_data_model gdm   
            INNER JOIN gis_object o ON gdm.gis_data_model_id = O.gis_data_model_id   
            JOIN gis_property p ON o.gis_object_id=p.gis_object_id  
            WHERE GDM.gis_data_model_type_id=4  
            AND column_name = 'License_Num'  
   AND table_name = @tablename  
            AND EXISTS(SELECT 1 FROM gis_property p WHERE o.gis_object_id=p.gis_object_id AND column_name = 'License_Prefix')  
            AND EXISTS(SELECT 1 FROM gis_property p WHERE o.gis_object_id=p.gis_object_id AND column_name = 'Original_Start_Date')  
              
            OPEN c_search_license_num  
            FETCH NEXT FROM c_search_license_num  
            INTO    @object_name,  
     @table_name ,  
     @gis_data_model_code  
       
            WHILE ( @@FETCH_STATUS = 0 )   
                BEGIN --**  
               
                    SELECT @SQL = 'INSERT INTO #License '   
     SELECT @SQL = @SQL + 'SELECT License_Prefix, License_Num, Original_Start_Date FROM ' + @table_name + ' tb '  
     SELECT  @SQL = @SQL + ' JOIN ' + RTRIM(@gis_data_model_code) + '_policy_binder pb ON pb.' + RTRIM(@gis_data_model_code) + '_policy_binder_id = tb.' + RTRIM(@gis_data_model_code) + '_policy_binder_id '  
     SELECT  @SQL = @SQL + ' JOIN gis_policy_link gis ON gis.gis_policy_link_id = pb.gis_policy_link_id '  
     SELECT  @SQL = @SQL + ' INNER JOIN #AssociatedParties AP ON gis.party_cnt= AP.Party_cnt'        
                                        
                    EXEC (@SQL)  
                      
                    SELECT  @License_Num = ''  
                      
                    SELECT  @License_Prefix_id = License_Prefix_id ,  
                            @License_Num = License_Num ,  
                            @Coverage_Eff_date = Original_Start_Date  
                    FROM    #License  
                                                              
                    IF LEN(RTRIM(@License_Num)) <> 0   
                        BEGIN --***  
                            SELECT  @License_Prefix = gudd.description  
                            FROM    GIS_Property gp  
                                    INNER JOIN GIS_User_Def_Detail gudd ON gp.Specials_Type_Reference = gudd.gis_user_def_header_id  
                            WHERE   gp.property_name = 'License_Prefix'  
                                    AND gudd.gis_user_def_detail_id = @License_Prefix_id  
                            SELECT  @License_Num = @License_Prefix + '*'  
                                    + @License_Num  
                        END --***  
                          
  
                    FETCH NEXT FROM c_search_license_num   
                    INTO @object_name,   
       @table_name ,   
       @gis_data_model_code  
         
                END --**  
            CLOSE c_search_license_num  
            DEALLOCATE c_search_license_num   
  
   --TEST CODE  
   --SELECT 'LICENSE'  
   --SELECT @License_Prefix_id AS "license prefix id", @License_Num AS "license num", @Coverage_Eff_date AS "coverage eff date"  
   --SELECT * FROM #License  
   --END TEST CODE              
              
                
            SELECT DISTINCT  
                    NULL risk_cnt ,  
                    'No Claim' claim_Number ,  
                    NULL settlement ,  
                    NULL date_closed ,  
                    NULL Object_Name ,  
                    NULL Property_Name ,  
                    NULL Party_Cnt ,  
                    ISNULL(@Coverage_Eff_date, ISNULL(@risk_start_date, '')) cover_start_date ,  
                    ISNULL(@risk_end_date, '') expiry_date ,  
                    @CertHolderName CertiHolderName ,  
                    @CertHolderAddress1 CertiHolderAddress1 ,  
                    @CertHolderAddress2 CertiHolderAddress2 ,  
                    @CertHolderAddress3 CertiHolderAddress3 ,  
                    @CertHolderAddress4 CertiHolderAddress4 ,  
                    @CertHolderPostalCode CertiHolderPostalCode ,  
                    @ClientName ClientName ,  
                    NULL Loss_From_date ,  
                    NULL Loss_Desc ,  
                    ISNULL(@Coverage_Eff_date,  
                           ISNULL(@risk_start_date, '')) Coverage_Eff_date ,  
                    ISNULL(@Coverage_Exp_date, '') Coverage_Exp_date ,  
                    @License_Num License_Num  
        END --*  
    ELSE  
  BEGIN   
   SELECT  *  
   FROM #Record_Found  
   ORDER BY loss_from_date DESC  
        END  
          
    DROP TABLE #Matches_Found  
    DROP TABLE #AssociatedParties  
    DROP TABLE #Claim_Details  
    DROP TABLE #Risks  
    DROP TABLE #PB_Risks  
    DROP TABLE #LegacyClaim_Details  
    DROP TABLE #Record_Found  
    DROP TABLE #License  
    SET NOCOUNT OFF  
END  
  