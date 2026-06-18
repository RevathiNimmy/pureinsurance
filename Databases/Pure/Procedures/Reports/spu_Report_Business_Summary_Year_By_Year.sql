EXEC DDLDropProcedure spu_Report_Business_Summary_Year_By_Year
GO

CREATE PROCEDURE spu_Report_Business_Summary_Year_By_Year
    @branch_id int,
    @report_on VARCHAR(50),	
    @year_1 INT = Null,
    @year_2 INT =Null,
    @group_by VARCHAR(255),
    @then_by VARCHAR(255)
AS


if @then_by=@group_by OR @then_by=''
	SET @then_by ='None'

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

DECLARE @first_period_end_date_1 DATETIME
DECLARE @first_period_end_date_2 DATETIME
DECLARE @first_period_end_date_3 DATETIME
DECLARE @first_period_end_date_4 DATETIME
DECLARE @first_period_end_date_5 DATETIME
DECLARE @period_count INT

DECLARE @Current_Start_Date_1 DATETIME
DECLARE @Current_End_Date_1 DATETIME
DECLARE @Current_Start_Date_2 DATETIME
DECLARE @Current_End_Date_2 DATETIME
DECLARE @Current_Start_Date_3 DATETIME
DECLARE @Current_End_Date_3 DATETIME
DECLARE @Current_Start_Date_4 DATETIME
DECLARE @Current_End_Date_4 DATETIME
DECLARE @Current_Start_Date_5 DATETIME
DECLARE @Current_End_Date_5 DATETIME
DECLARE @start_date DATETIME
DECLARE @end_date DATETIME
DECLARE @year INT

SET NOCOUNT ON

IF @report_on='Accounting Year' 
	BEGIN
		SELECT @year= (MIN( year_name)) FROM period WHERE period_end_complete=0
		AND company_id  =ISNULL(@branch_id,1)
 		
		SELECT  
	 	@first_period_end_date_1= MIN( period_end_date), @Current_end_Date_1=MAX( period_end_date)
		FROM Period  
		WHERE  company_id  =isnull(@branch_id,1)
		    AND year_name  = cast(@year as varchar(20))

		SELECT  
	 	@first_period_end_date_2= MIN( period_end_date), @Current_end_Date_2=MAX( period_end_date)
		FROM Period  
		WHERE  company_id  =ISNULL(@branch_id,1)
		    AND year_name  = CAST(@year-1 as varchar(20))

		SELECT  
	 	@first_period_end_date_3= MIN( period_end_date), @Current_end_Date_3=MAX( period_end_date)
		FROM Period  
		WHERE  company_id  =ISNULL(@branch_id,1)
		    AND year_name  = CAST(@year-2 as varchar(20))

		SELECT  
	 	@first_period_end_date_4= MIN( period_end_date), @Current_end_Date_4=MAX( period_end_date)
		FROM Period  
		WHERE  company_id  =ISNULL(@branch_id,1)
		    AND year_name  = CAST(@year-3 as varchar(20))

		SELECT  
	 	@first_period_end_date_5= MIN( period_end_date), @Current_end_Date_5=MAX( period_end_date)
		FROM Period  
		WHERE  company_id  =ISNULL(@branch_id,1)
		    AND year_name  = CAST(@year-4 as varchar(20))

	---Finding @Current_Start_Date_1	
		SELECT @period_count = count(period_id) from period where year_name=@year 
			AND company_id  =ISNULL(@branch_id,1) group by year_name 
		IF @period_count=12 
			SELECT @Current_Start_Date_1=cast(month(@first_period_end_date_1)as varchar) + '-01-' + cast(year(@first_period_end_date_1)as varchar)
		ELSE IF @period_count=13
			SELECT @Current_Start_Date_1=dateadd(d,-28,@first_period_end_date_1)
		ELSE IF @period_count=4
			SELECT @Current_Start_Date_1=dateadd(M,-3,@first_period_end_date_1)
		ELSE
			SELECT @Current_Start_Date_1=@first_period_end_date_1

	---Finding @Current_Start_Date_2
		SELECT @period_count = count(period_id) from period where year_name=@year-1 
			AND company_id  =ISNULL(@branch_id,1) group by year_name 
		IF @period_count=12 
			SELECT @Current_Start_Date_2=cast(month(@first_period_end_date_2)as varchar) + '-01-' + cast(year(@first_period_end_date_2)as varchar)
		ELSE IF @period_count=13
			SELECT @Current_Start_Date_2=dateadd(d,-28,@first_period_end_date_2)
		ELSE IF @period_count=4
			SELECT @Current_Start_Date_2=dateadd(M,-3,@first_period_end_date_2)
		ELSE
			SELECT @Current_Start_Date_2=@first_period_end_date_2

	---Finding @Current_Start_Date_3	
		SELECT @period_count = count(period_id) from period where year_name=@year-2 
			AND company_id  =ISNULL(@branch_id,1) group by year_name 
		IF @period_count=12 
			SELECT @Current_Start_Date_3=cast(month(@first_period_end_date_3)as varchar) + '-01-' + cast(year(@first_period_end_date_3)as varchar)
		ELSE IF @period_count=13
			SELECT @Current_Start_Date_3=dateadd(d,-28,@first_period_end_date_3)
		ELSE IF @period_count=4
			SELECT @Current_Start_Date_3=dateadd(M,-3,@first_period_end_date_3)
		ELSE
			SELECT @Current_Start_Date_3=@first_period_end_date_3

	---Finding @Current_Start_Date_4	
		SELECT @period_count = count(period_id) from period where year_name=@year-3 
			AND company_id  =ISNULL(@branch_id,1) group by year_name 
		IF @period_count=12 
			SELECT @Current_Start_Date_4=cast(month(@first_period_end_date_4)as varchar) + '-01-' + cast(year(@first_period_end_date_4)as varchar)
		ELSE IF @period_count=13
			SELECT @Current_Start_Date_4=dateadd(d,-28,@first_period_end_date_4)
		ELSE IF @period_count=4
			SELECT @Current_Start_Date_4=dateadd(M,-3,@first_period_end_date_4)
		ELSE
			SELECT @Current_Start_Date_4=@first_period_end_date_4

	---Finding @Current_Start_Date_5
		SELECT @period_count = count(period_id) from period where year_name=@year-4 
			AND company_id  =ISNULL(@branch_id,1) group by year_name 
		IF @period_count=12 
			SELECT @Current_Start_Date_5=cast(month(@first_period_end_date_5)as varchar) + '-01-' + cast(year(@first_period_end_date_5)as varchar)
		ELSE IF @period_count=13
			SELECT @Current_Start_Date_5=dateadd(d,-28,@first_period_end_date_5)
		ELSE IF @period_count=4
			SELECT @Current_Start_Date_5=dateadd(M,-3,@first_period_end_date_5)
		ELSE
			SELECT @Current_Start_Date_5=@first_period_end_date_5
		
		IF ISDATE(@Current_Start_Date_5) =1
			SELECT @start_date =@Current_Start_Date_5
		ELSE IF ISDATE(@Current_Start_Date_4) =1
			SELECT @start_date =@Current_Start_Date_4
		ELSE IF ISDATE(@Current_Start_Date_3) =1
			SELECT @start_date =@Current_Start_Date_3
		ELSE IF ISDATE(@Current_Start_Date_2) =1
			SELECT @start_date =@Current_Start_Date_2
		ELSE IF ISDATE(@Current_Start_Date_1) =1
			SELECT @start_date =@Current_Start_Date_1
		ELSE
			SELECT @start_date =GETDATE()

		SELECT @end_date=ISNULL(@Current_end_Date_1,GETDATE())

	END
ELSE IF @report_on='Calendar Year' 
	BEGIN
		SELECT  @Current_Start_Date_1 = '01-01-' + cast(year(getdate()) as varchar)
		SELECT  @Current_end_Date_1 = '12-31-' + cast(year(getdate()) as varchar)
		SELECT  @Current_Start_Date_2 =DATEADD(year, -1, @Current_Start_Date_1)
		SELECT  @Current_end_Date_2 = DATEADD(year, -1, @Current_end_Date_1)
		SELECT  @Current_Start_Date_3 =DATEADD(year, -1, @Current_Start_Date_2)
		SELECT  @Current_end_Date_3 = DATEADD(year, -1, @Current_end_Date_2)
		SELECT  @Current_Start_Date_4 =DATEADD(year, -1, @Current_Start_Date_3)
		SELECT  @Current_end_Date_4 = DATEADD(year, -1, @Current_end_Date_3)
		SELECT  @Current_Start_Date_5 =DATEADD(year, -1, @Current_Start_Date_4)
		SELECT  @Current_end_Date_5 = DATEADD(year, -1, @Current_end_Date_4)
		SELECT @start_date =ISNULL(@Current_Start_Date_5,DATEADD(year, -4, @Current_Start_Date_1)) 
		SELECT @end_date=isnull(@Current_end_Date_1,getdate())
	END

ELSE 
	BEGIN		
		SELECT  @Current_Start_Date_1 = '01-01-' + cast(@year_1 as varchar)
		SELECT  @Current_end_Date_1 = '12-31-' + cast(@year_1 as varchar)
		SELECT  @Current_Start_Date_2 ='01-01-' + cast(@year_2 as varchar)
		SELECT  @Current_end_Date_2 = '12-31-' + cast(@year_2 as varchar)
		IF @year_1 > @year_2 
			BEGIN
				SELECT @start_date =@Current_Start_Date_2
				SELECT @end_date=@Current_end_Date_1
			END
		ELSE
			BEGIN
				SELECT @start_date = @Current_Start_Date_1
				SELECT @end_date=@Current_end_Date_2
			END		
	END

CREATE TABLE #Document_Temp
	(
	   document_id INT,
	   document_date DATETIME,
	   premium numeric(19, 4),
	   commission numeric(19, 4),
	   group_code_1 VARCHAR(255),
	   group_code_2 VARCHAR(255)
	)

CREATE TABLE #transactions
    (
	group_code_1 VARCHAR(255),
	group_code_2 VARCHAR(255),
	year_1 VARCHAR(50),
        premium_amount_1 numeric(19, 4) NULL,
        comm_amount_1 numeric(19, 4) NULL,
	year_2 VARCHAR(50),
        premium_amount_2 numeric(19, 4) NULL,
        comm_amount_2 numeric(19, 4) NULL,
	year_3 VARCHAR(50),
        premium_amount_3 numeric(19, 4) NULL,
        comm_amount_3 numeric(19, 4) NULL,
	year_4 VARCHAR(50),
        premium_amount_4 numeric(19, 4) NULL,
        comm_amount_4 numeric(19, 4) NULL,
	year_5 VARCHAR(50),
        premium_amount_5 numeric(19, 4) NULL,
        comm_amount_5 numeric(19, 4) NULL
    )
------------------------------------------------

	INSERT INTO #Document_Temp
	
	(
	    document_id,
	    document_date,
	    premium,
	    commission
	)
	SELECT 
	    D.document_id,
	    D.document_date,
	    (
		SELECT ISNULL(TD.Amount ,0)
		FROM Transdetail TD 
		WHERE TD.Document_id=D.document_id
		AND TD.document_sequence=1
	    ),

	    (
		SELECT ISNULL(Sum(TD.Amount),0)
			FROM transdetail TD
			JOIN transdetail_type TT
			ON TT.Transdetail_type_id=TD.transdetail_type_id
			WHERE TD.document_id= D.Document_id
			AND TT.code='COMM'
	    )
	    
	FROM Document D
	WHERE D.documenttype_id IN (4, 5, 15, 16, 17, 18, 30,31, 32, 35, 36)
	AND D.company_id = ISNULL(@branch_id, D.company_id)
	AND D.document_date BETWEEN @start_date AND @end_date


---------------------------------------------------------------
-- Updating Group By 
---------------------------------------------------------------
if  @group_by  ='Branch'
	BEGIN
		UPDATE IT SET
			IT.group_code_1=ISNULL(S.code,'')
			FROM #Document_Temp IT 
			JOIN document D
			ON IT.document_id=D.document_id
			JOIN Source S
			ON 
			S.source_id=D.company_id
	END

else if @group_by ='Client' 
	BEGIN
		UPDATE IT
		SET IT.group_code_1=ISNULL(P.shortName, '')
		FROM #Document_Temp IT
		    JOIN transdetail T
			ON IT.document_id=T.document_id
                    JOIN account A
	                ON A.account_id = T.account_id
 			JOIN ledger L
		      ON A.ledger_id= L.Ledger_id
		       AND Ledger_short_name='SA'
	            JOIN Party P
	                ON P.party_cnt = A.account_key
                     AND 1= (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name='SA'
				where td.document_id=IT.document_id
			    )

		-- Updating Multiple Clients
		UPDATE IT
		SET IT.group_code_1='Multiple Client'
		FROM #Document_Temp IT
			   WHERE 1 < (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name='SA'
				where td.document_id=IT.document_id
			    )


	END
else if @group_by ='Insurer' 
	BEGIN
		UPDATE IT
		SET IT.group_code_1=ISNULL(P.shortName, '')
		FROM #Document_Temp IT
		    JOIN document D 
			ON D.document_id = IT.document_id
		    JOIN Insurance_File I
    		   	ON I.insurance_file_cnt = D.insurance_file_cnt
                    JOIN Party P
    			ON P.party_cnt = I.lead_insurer_cnt 

		-- Updating Multiple Insurers
		UPDATE IT
		SET IT.group_code_1='Multiple Insurer' 
			FROM #Document_Temp IT
			WHERE IT.group_code_1 ='MULTI'
                        
	END


else if @group_by ='Third Party' 
	Begin
		UPDATE IT
		SET IT.group_code_1=ISNULL(P.shortName, '')
		FROM #Document_Temp IT
		    JOIN transdetail T
			ON IT.document_id=T.document_id
                    JOIN account A
	                ON A.account_id = T.account_id
	            JOIN Party P
	                ON P.party_cnt = A.account_key
		    JOIN ledger L
			ON L.ledger_id=A.ledger_id
			and L.Ledger_short_name IN('AG','UB','TR')
                     AND 1= (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name IN('AG','UB','TR')
				where td.document_id=IT.document_id
			    )

		-- Updating Multiple third parties
		UPDATE IT
		SET IT.group_code_1='Multiple Third Party'
		FROM #Document_Temp IT
			   WHERE 1 < (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name IN('AG','UB','TR')
				where td.document_id=IT.document_id
			    )
	     
	END

else if @group_by ='Account Executive' 
	BEGIN
		UPDATE IT
		SET IT.group_code_1=ISNULL(P_AccExec.shortName, '')
		FROM #Document_Temp IT
		    JOIN transdetail T
			ON IT.document_id=T.document_id
                    JOIN account A
                        ON A.account_id = T.account_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
			   WHERE 1 = (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
                    		JOIN party P
                        		ON P.party_cnt = A.account_key
                    		JOIN party P_AccExec
                        		ON P_AccExec.party_cnt = P.consultant_cnt
					AND P_AccExec.Party_type_id IN(5,19)--(Acc Executive and Exec Handler)
				where td.document_id=IT.document_id
				)

		-- Updating Multiple Account Executives
		UPDATE IT
		SET IT.group_code_1='Multiple Account Executive'
		FROM #Document_Temp IT
			   WHERE 1 < (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
                    		JOIN party P
                        		ON P.party_cnt = A.account_key
                    		JOIN party P_AccExec
                        		ON P_AccExec.party_cnt = P.consultant_cnt
					AND P_AccExec.Party_type_id IN(5,19)--(Acc Executive and Exec Handler)
				where td.document_id=IT.document_id
			    ) 
	END
else if @group_by = 'Account Handler' 
	BEGIN
               UPDATE IT
		SET IT.group_code_1= ISNULL(AccH.shortname, '')
		    FROM #Document_Temp IT
		    JOIN Document D 
			ON D.document_id = IT.document_id	
                    LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
                    LEFT JOIN Party AccH
                        ON AccH.party_cnt = IFI.account_handler_cnt 
	END

else if @group_by = 'Business Type' 
	BEGIN
               UPDATE IT
		SET IT.group_code_1= ISNULL(BT.description, '')
		    FROM #Document_Temp IT
		    JOIN Document D 
			ON D.document_id = IT.document_id	
                    LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
                    LEFT JOIN business_type BT
                        ON BT.business_type_id = IFI.business_type_id 
	END

else if @group_by = 'Risk' 
	BEGIN
               UPDATE IT
		SET IT.group_code_1= ISNULL(rc.code, '')
		    FROM #Document_Temp IT
		    JOIN Document D 
			ON D.document_id = IT.document_id	
                    LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
		    LEFT JOIN risk_code rc
    			ON rc.risk_code_id = IFI.risk_code_id
	END

else if @group_by='Transaction Type' 
	BEGIN
		UPDATE IT
		SET IT.group_code_1=
		     (
        		SELECT CASE
        			WHEN D.documenttype_id IN (4,5) THEN 'New Business'
        			WHEN D.documenttype_id IN (15,16) THEN 'Renewals'
        			WHEN D.documenttype_id IN (17,18) THEN 'Adjustments'
        			WHEN D.documenttype_id IN (31,32) THEN 'Short Term'
        			WHEN D.documenttype_id IN (35,36) THEN 'Transfers'
        			WHEN D.documenttype_id IN (30) THEN 'Client Fee'
        		END
		      )
		     FROM #Document_Temp IT
		     JOIN Document D 
		     ON D.document_id = IT.document_id	
    		  
	END

else if @group_by='Period/Month' 
	BEGIN
	IF @report_on ='Accounting Year'
		BEGIN
			UPDATE IT
			SET IT.group_code_1=	
				(SELECT top 1 Period_name FROM Period WHERE 
				MONTH(period_end_date)=MONTH(IT.Document_Date)
				AND YEAR(period_end_date)=YEAR(IT.Document_Date)
				AND DAY(period_end_date)>=DAY(IT.Document_Date))
			FROM #Document_Temp IT 					
		END
	 ELSE
		BEGIN
			UPDATE IT
			SET IT.group_code_1=
			     (
	        		SELECT CASE
	        			WHEN Month(D.document_date) =1 THEN 'January'
	        			WHEN Month(D.document_date) =2 THEN 'February'
	        			WHEN Month(D.document_date) =3 THEN 'March'
	        			WHEN Month(D.document_date) =4 THEN 'April'
	        			WHEN Month(D.document_date) =5 THEN 'May'
	        			WHEN Month(D.document_date) =6 THEN 'June'
	        			WHEN Month(D.document_date) =7 THEN 'July'
	        			WHEN Month(D.document_date) =8 THEN 'August'
	        			WHEN Month(D.document_date) =9 THEN 'September'
	        			WHEN Month(D.document_date) =10 THEN 'October'
	        			WHEN Month(D.document_date) =11 THEN 'November'
	        			WHEN Month(D.document_date) =12 THEN 'December'
	        		END
			      )
			     FROM #Document_Temp IT
			     JOIN Document D 
			     ON D.document_id = IT.document_id	
		END

	END	      
---------------------------------------------------------------
-- Updating Then By 
---------------------------------------------------------------
if  @then_by  ='None'
	BEGIN
		UPDATE IT SET
			IT.group_code_2=NULL
			FROM #Document_Temp IT 
	END 

else if  @then_by  ='Branch'
	BEGIN
		UPDATE IT SET
			IT.group_code_2=ISNULL(S.code,'')
			FROM #Document_Temp IT 
			JOIN document D
			ON IT.document_id=D.document_id
			JOIN Source S
			ON 
			S.source_id=D.company_id
	END

else if @then_by ='Client' 
	BEGIN
		UPDATE IT
		SET IT.group_code_2=ISNULL(P.shortName, '')
		FROM #Document_Temp IT
		    JOIN transdetail T
			ON IT.document_id=T.document_id
                    JOIN account A
	                ON A.account_id = T.account_id
 			JOIN ledger L
		      ON A.ledger_id= L.Ledger_id
		       AND Ledger_short_name='SA'
	            JOIN Party P
	                ON P.party_cnt = A.account_key
                     AND 1= (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name='SA'
				where td.document_id=IT.document_id
			    )
		-- Updating Multiple Clients
		UPDATE IT
		SET IT.group_code_2='Multiple Client'
		FROM #Document_Temp IT
			   WHERE 1 < (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name='SA'
				where td.document_id=IT.document_id
			    )


	END
else if @then_by ='Insurer' 
	BEGIN
		UPDATE IT
		SET IT.group_code_2=ISNULL(P.shortName, '')
		FROM #Document_Temp IT
		    JOIN document D 
			ON D.document_id = IT.document_id
		    JOIN Insurance_File I
    		   	ON I.insurance_file_cnt = D.insurance_file_cnt
                    JOIN Party P
    			ON P.party_cnt = I.lead_insurer_cnt 

		-- Updating Multiple Insurers
		UPDATE IT
		SET IT.group_code_2='Multiple Insurer' 
			FROM #Document_Temp IT
			WHERE IT.group_code_2 ='MULTI'
                        
	END


else if @then_by ='Third Party' 
	Begin
		UPDATE IT
		SET IT.group_code_2=ISNULL(P.shortName, '')
		FROM #Document_Temp IT
		    JOIN transdetail T
			ON IT.document_id=T.document_id
                    JOIN account A
	                ON A.account_id = T.account_id
	            JOIN Party P
	                ON P.party_cnt = A.account_key
		    JOIN ledger L
			ON L.ledger_id=A.ledger_id
			and L.Ledger_short_name IN('AG','UB')
                     AND 1= (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name IN('AG','UB','TR')
				where td.document_id=IT.document_id
			    )

		-- Updating Multiple third parties
		UPDATE IT
		SET IT.group_code_2='Multiple Third Parties'
		FROM #Document_Temp IT
			   WHERE 1 < (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
				JOIN ledger L
				      ON A.ledger_id= L.Ledger_id
		                        AND Ledger_short_name IN('AG','UB','TR')
				where td.document_id=IT.document_id
			    )
	     
	END

else if @then_by ='Account Executive' 
	BEGIN
		UPDATE IT
		SET IT.group_code_2=ISNULL(P_AccExec.shortName, '')
		FROM #Document_Temp IT
		    JOIN transdetail T
			ON IT.document_id=T.document_id
                    JOIN account A
                        ON A.account_id = T.account_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
			   WHERE 1 = (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
                    		JOIN party P
                        		ON P.party_cnt = A.account_key
                    		JOIN party P_AccExec
                        		ON P_AccExec.party_cnt = P.consultant_cnt
					AND P_AccExec.Party_type_id IN(5,19)--(Acc Executive and Exec Handler)
				where td.document_id=IT.document_id
				)

		-- Updating Multiple Account Executives
		UPDATE IT
		SET IT.group_code_2='Multiple Account Executive'
		FROM #Document_Temp IT
			   WHERE 1 < (
				SELECT COUNT(DISTINCT td.Account_id) from transdetail td 
		                JOIN account A
			              ON A.account_id = td.account_id
                    		JOIN party P
                        		ON P.party_cnt = A.account_key
                    		JOIN party P_AccExec
                        		ON P_AccExec.party_cnt = P.consultant_cnt
					AND P_AccExec.Party_type_id IN(5,19)--(Acc Executive and Exec Handler)
				where td.document_id=IT.document_id
			    ) 
	END
else if @then_by = 'Account Handler' 
	BEGIN
               UPDATE IT
		SET IT.group_code_2= ISNULL(AccH.resolved_name, '')
		    FROM #Document_Temp IT
		    JOIN Document D 
			ON D.document_id = IT.document_id	
                    LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
                    LEFT JOIN Party AccH
                        ON AccH.party_cnt = IFI.account_handler_cnt 
	END

else if @then_by = 'Business Type' 
	BEGIN
               UPDATE IT
		SET IT.group_code_2= ISNULL(BT.description, '')
		    FROM #Document_Temp IT
		    JOIN Document D 
			ON D.document_id = IT.document_id	
                    LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
                    LEFT JOIN business_type BT
                        ON BT.business_type_id = IFI.business_type_id 
	END

else if @then_by = 'Risk' 
	BEGIN
               UPDATE IT
		SET IT.group_code_2= ISNULL(rc.code, '')
		    FROM #Document_Temp IT
		    JOIN Document D 
			ON D.document_id = IT.document_id	
                    LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
		    LEFT JOIN risk_code rc
    			ON rc.risk_code_id = IFI.risk_code_id
	END

else if @then_by='Transaction Type' 
	BEGIN
		UPDATE IT
		SET IT.group_code_2=
		     (
        		SELECT CASE
        			WHEN D.documenttype_id IN (4,5) THEN 'New Business'
        			WHEN D.documenttype_id IN (15,16) THEN 'Renewals'
        			WHEN D.documenttype_id IN (17,18) THEN 'Adjustments'
        			WHEN D.documenttype_id IN (31,32) THEN 'Short Term'
        			WHEN D.documenttype_id IN (35,36) THEN 'Transfers'
        			WHEN D.documenttype_id IN (30) THEN 'Client Fee'
        		END
		      )
		     FROM #Document_Temp IT
		     JOIN Document D 
		     ON D.document_id = IT.document_id	
    		  
	END

else if @then_by='Period/Month' 
	BEGIN
	IF @report_on ='Accounting Year'
		BEGIN
			UPDATE IT
			SET IT.group_code_2=	
				(SELECT top 1 Period_name FROM Period WHERE 
				MONTH(period_end_date)=MONTH(IT.Document_Date)
				AND YEAR(period_end_date)=YEAR(IT.Document_Date)
				AND DAY(period_end_date)>=DAY(IT.Document_Date))
			FROM #Document_Temp IT 					
		END
	 ELSE
		BEGIN
			UPDATE IT
			SET IT.group_code_2=
			     (
	        		SELECT CASE
	        			WHEN Month(D.document_date) =1 THEN 'January'
	        			WHEN Month(D.document_date) =2 THEN 'February'
	        			WHEN Month(D.document_date) =3 THEN 'March'
	        			WHEN Month(D.document_date) =4 THEN 'April'
	        			WHEN Month(D.document_date) =5 THEN 'May'
	        			WHEN Month(D.document_date) =6 THEN 'June'
	        			WHEN Month(D.document_date) =7 THEN 'July'
	        			WHEN Month(D.document_date) =8 THEN 'August'
	        			WHEN Month(D.document_date) =9 THEN 'September'
	        			WHEN Month(D.document_date) =10 THEN 'October'
	        			WHEN Month(D.document_date) =11 THEN 'November'
	        			WHEN Month(D.document_date) =12 THEN 'December'
	        		END
			      )
			     FROM #Document_Temp IT
			     JOIN Document D 
			     ON D.document_id = IT.document_id	
		END
	END	

-------------------------------------------
--Inserting  datas into #transaction table
-------------------------------------------

	INSERT INTO #transactions
		
		(
		    group_code_1,
		    group_code_2,
		    year_1,	
		    Premium_amount_1,
		    comm_amount_1
		)
	
	select group_code_1,group_code_2,year(@Current_Start_Date_1),
	isnull(sum(premium),0), isnull(sum(commission),0)
	FROM  #Document_Temp 
	where document_date between  @Current_Start_Date_1  AND @Current_end_Date_1 
	group by group_code_1,group_code_2
---------------------------------------------------------
	INSERT INTO #transactions
		
		(
		    group_code_1,
		    group_code_2,
		    year_2,	
		    Premium_amount_2,
		    comm_amount_2
		)
	
	select group_code_1,group_code_2,year(@Current_Start_Date_2),
	isnull(sum(premium),0), isnull(sum(commission),0)
	FROM  #Document_Temp 
	where document_date between  @Current_Start_Date_2  AND @Current_end_Date_2
	group by group_code_1,group_code_2
------------------------------------------------------------------
	INSERT INTO #transactions
		
		(
		    group_code_1,
		    group_code_2,
		    year_3,	
		    Premium_amount_3,
		    comm_amount_3
		)
	
	select group_code_1,group_code_2,year(@Current_Start_Date_3),
	isnull(sum(premium),0), isnull(sum(commission),0)
	FROM  #Document_Temp 
	where document_date between  @Current_Start_Date_3  AND @Current_end_Date_3
	group by group_code_1,group_code_2
------------------------------------------------------------------
	INSERT INTO #transactions
		
		(
		    group_code_1,
		    group_code_2,
		    year_4,	
		    Premium_amount_4,
		    comm_amount_4
		)
	
	select group_code_1,group_code_2,year(@Current_Start_Date_4),
	isnull(sum(premium),0), isnull(sum(commission),0)
	FROM  #Document_Temp 
	where document_date between  @Current_Start_Date_4  AND @Current_end_Date_4
	group by group_code_1,group_code_2

--------------------------------------------------------------------------
	INSERT INTO #transactions
		
		(
		    group_code_1,
		    group_code_2,
		    year_5,	
		    Premium_amount_5,
		    comm_amount_5
		)
	
	select group_code_1,group_code_2,year(@Current_Start_Date_5),
	isnull(sum(premium),0), isnull(sum(commission),0)
	FROM  #Document_Temp 
	where document_date between  @Current_Start_Date_5  AND @Current_end_Date_5
	group by group_code_1,group_code_2


UPDATE #transactions SET year_1=year(@Current_Start_Date_1)WHERE year_1 IS NULL
UPDATE #transactions SET year_2=year(ISNULL(@Current_Start_Date_2,DATEADD(year, -1, @Current_Start_Date_1)))WHERE year_2 IS NULL
UPDATE #transactions SET year_3=year(ISNULL(@Current_Start_Date_3,DATEADD(year, -2, @Current_Start_Date_1)))WHERE year_3 IS NULL
UPDATE #transactions SET year_4=year(ISNULL(@Current_Start_Date_4,DATEADD(year, -3, @Current_Start_Date_1)))WHERE year_4 IS NULL
UPDATE #transactions SET year_5=year(ISNULL(@Current_Start_Date_5,DATEADD(year, -4, @Current_Start_Date_1)))WHERE year_5 IS NULL
UPDATE #transactions SET premium_amount_1=0 WHERE premium_amount_1 IS NULL
UPDATE #transactions SET comm_amount_1=0 WHERE comm_amount_1 IS NULL
UPDATE #transactions SET premium_amount_2=0 WHERE premium_amount_2 IS NULL
UPDATE #transactions SET comm_amount_2=0 WHERE comm_amount_2 IS NULL
UPDATE #transactions SET premium_amount_3=0 WHERE premium_amount_3 IS NULL
UPDATE #transactions SET comm_amount_3=0 WHERE comm_amount_3 IS NULL
UPDATE #transactions SET premium_amount_4=0 WHERE premium_amount_4 IS NULL
UPDATE #transactions SET comm_amount_4=0 WHERE comm_amount_4 IS NULL
UPDATE #transactions SET premium_amount_5=0 WHERE premium_amount_5 IS NULL
UPDATE #transactions SET comm_amount_5=0 WHERE comm_amount_5 IS NULL
UPDATE #transactions SET group_code_1 = 'No ' + @group_by WHERE group_code_1 is Null
UPDATE #transactions SET group_code_2 = 'No ' + @then_by WHERE group_code_2 is Null AND @then_by <>'None'

SET NOCOUNT OFF

select * from #transactions


