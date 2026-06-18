SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_Add_Credit_Control_Item_Instalments_InsFile'
GO

CREATE PROCEDURE spu_ACT_Add_Credit_Control_Item_Instalments_InsFile
		@Insurance_File_Cnt 	int,
	    @MTAType				INT = NULL		
AS
DECLARE @Trans_Type				char(10)
DECLARE @Account_ID				int
DECLARE @Document_ID			int
DECLARE @Document_Date			datetime
DECLARE @PFPrem_Finance_Cnt		int
DECLARE @PFPrem_Finance_Version	int
DECLARE @Amount					numeric(19,4)
DECLARE @Can_Auto_Cancel		int
DECLARE @Will_Auto_Cancel		int
DECLARE @Credit_Control_Step_ID	int
DECLARE @Created_Date			datetime	
DECLARE @Due_Date				datetime
DECLARE @Letter_Sent			int
DECLARE @Recurrence_Count		int
DECLARE @Number_OF_Days			int
DECLARE @PFInstalments_Id		int
DECLARE @Credit_Control_Item_ID	int
DECLARE @InstalmentNumber		int
Declare @peril_count int
Declare @auto_cancel_count int

select @peril_count=Count(Distinct peril_type.peril_type_id) 
	from peril_type
	Inner Join peril ON peril.peril_type_id=peril_type.peril_type_id    
    where risk_cnt IN (select risk_cnt    
    from insurance_file_risk_link    
    where insurance_file_cnt = @insurance_file_cnt)

select @auto_cancel_count=Count(peril_type_id)
	from peril_type PT    
    Where PT.is_auto_cancel = 1    
    and PT.peril_type_id in (    
         select peril_type_id from peril    
         where risk_cnt IN (select risk_cnt    
         from insurance_file_risk_link    
         where insurance_file_cnt = @insurance_file_cnt))
  
if @auto_cancel_count = @peril_count   
    select @can_auto_cancel = 1    
else    
    select @can_auto_cancel = 0    

-- FOR MTA ONLY
if @MTATYPE = 0 OR @MTATYPE = 1
	BEGIN	
		-- Get the Premium Finance CNT
		SELECT @PFPrem_Finance_Cnt = (SELECT TOP 1  PFPremiumFinance.pfprem_finance_cnt
									  FROM			PFInstalments INNER JOIN
													PFPremiumFinance ON PFInstalments.pfprem_finance_cnt =
													PFPremiumFinance.pfprem_finance_cnt AND
													PFInstalments.pfprem_finance_version =
													PFPremiumFinance.pfprem_finance_version
									  WHERE			(PFPremiumFinance.Insurance_File_Cnt = @Insurance_File_Cnt))
		
		-- Get the Premium Finance Version 
		SELECT @PFPrem_Finance_Version = (SELECT TOP 1  PFPremiumFinance.pfprem_finance_version
								     	  FROM          PFInstalments INNER JOIN
                		      							PFPremiumFinance ON PFInstalments.pfprem_finance_cnt =
                                		                PFPremiumFinance.pfprem_finance_cnt AND
		       			        						PFInstalments.pfprem_finance_version =
														PFPremiumFinance.pfprem_finance_version
			        					  WHERE        (PFPremiumFinance.Insurance_File_Cnt = @Insurance_File_Cnt))
		
		-- Get the existing Item which are NOT deleted.			        					  
		DECLARE ExistingItem_Cursor CURSOR FOR
			SELECT	Credit_Control_Item.credit_control_item_id,  PFInstalments.InstalmentNumber
			FROM	Credit_Control_Item INNER JOIN
		            PFInstalments ON Credit_Control_Item.PFInstalments_Id = PFInstalments.pfinstalments_id INNER JOIN
                	PFPremiumFinance ON Credit_Control_Item.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt AND
	                Credit_Control_Item.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version AND
          		    PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt AND
                    PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version
			WHERE	(PFInstalments.pfprem_finance_cnt = @PFPrem_Finance_Cnt) AND
				    (PFInstalments.pfprem_finance_version = (@PFPrem_Finance_Version - 1)) AND
					(Credit_Control_Item.Is_Deleted = 0)
			
			OPEN ExistingItem_Cursor
			FETCH NEXT FROM ExistingItem_Cursor INTO
					@Credit_Control_Item_ID, @InstalmentNumber
					
			DECLARE @Mta_Instalment_Cnt INT -- Mta installment id should starts from 1
			SELECT @Mta_Instalment_Cnt=1		
					
			WHILE @@FETCH_STATUS = 0
			Begin
				
				DECLARE NewInstalment_Cursor CURSOR FOR
					SELECT	PFPremiumFinance.TransType, DOC.document_id, DOC.document_date,
							PFPremiumFinance.Insurance_File_Cnt, PFPremiumFinance.pfprem_finance_cnt,
							PFPremiumFinance.pfprem_finance_version, PFInstalments.Amount,
							GETDATE() AS created_date, PFInstalments.pfinstalments_id
					FROM	PFInstalments INNER JOIN
	                        PFPremiumFinance ON PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt AND
                            PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version INNER JOIN
                            Document DOC INNER JOIN
                            TransDetail ON DOC.document_id = TransDetail.document_id ON PFPremiumFinance.PlanTransaction_id = TransDetail.transdetail_id
					WHERE   (PFPremiumFinance.Insurance_File_Cnt = @Insurance_File_Cnt) AND (PFInstalments.InstalmentNumber = @Mta_Instalment_Cnt)
					
				OPEN NewInstalment_Cursor
				FETCH NEXT FROM NewInstalment_Cursor INTO @Trans_Type, @Document_ID, @Document_Date,@Insurance_File_Cnt,
					                                      @PFPrem_Finance_Cnt, @PFPrem_Finance_Version, @Amount,
					                                      @Created_Date, @PfInstalments_ID
				WHILE @@FETCH_STATUS = 0
				BEGIN					UPDATE	Credit_Control_Item
					SET	    credit_control_reason = @Trans_Type,
							document_id = @Document_ID,
							document_date = @Document_Date,
							insurance_file_cnt = @Insurance_File_Cnt,
							pfprem_finance_cnt = @PFPrem_Finance_Cnt,
							pfprem_finance_version = @PFPrem_Finance_Version,
							amount = @Amount,
							created_date = @Created_Date,
                      		PFInstalments_Id = @PfInstalments_ID,
							can_auto_cancel = @Can_Auto_Cancel
					WHERE	credit_control_item_id = @Credit_Control_Item_ID
				
					SELECT @Mta_Instalment_Cnt= @Mta_Instalment_Cnt + 1
				
					FETCH NEXT FROM NewInstalment_Cursor INTO @Trans_Type, @Document_ID, @Document_Date,@Insurance_File_Cnt,
						                                      @PFPrem_Finance_Cnt, @PFPrem_Finance_Version, @Amount,
						                                      @Created_Date,@PfInstalments_ID		
					
					
				END
				CLOSE 	    NewInstalment_Cursor
				DEALLOCATE  NewInstalment_Cursor
				
					
				FETCH NEXT FROM ExistingItem_Cursor INTO @Credit_Control_Item_ID, @InstalmentNumber	
			END
			
		CLOSE 	    ExistingItem_Cursor
		DEALLOCATE  ExistingItem_Cursor
		
	END
		
ELSE
	BEGIN
		DECLARE CreditControl_Cursor CURSOR FOR
		SELECT	PFPremiumFinance.TransType, TransDetail.account_id, DOC.document_id, DOC.document_date,
				PFPremiumFinance.Insurance_File_Cnt, PFPremiumFinance.pfprem_finance_cnt,
				PFPremiumFinance.pfprem_finance_version, PFInstalments.Amount, 
				0 AS will_auto_cancel,
				ISNULL ((SELECT	MIN(Credit_Control_Step.credit_control_step_id)
		FROM	PFPremiumFinance INNER JOIN
		        PFRF ON PFPremiumFinance.pfrf_id = PFRF.pfrf_id INNER JOIN
        		Credit_Control_Rule ON PFRF.pffrequency_id = Credit_Control_Rule.pffrequency_id AND
		        PFPremiumFinance.CompanyNo = Credit_Control_Rule.source_id INNER JOIN
	      		Credit_Control_Step ON Credit_Control_Rule.credit_control_rule_id = Credit_Control_Step.credit_control_rule_id
		WHERE  (Credit_Control_Rule.business_type = 'INS') AND
		       (Credit_Control_Step.step_number = 1) AND
			   (PFPremiumFinance.Insurance_File_Cnt = @Insurance_File_Cnt) AND
               (Credit_Control_Rule.is_active = 1)
                AND (Credit_Control_rule.Product_id IS NULL  OR Credit_Control_rule.Product_id IN( SELECT Product_id FROM Insurance_file Where Insurance_File_Cnt =@Insurance_File_Cnt ))
                ), 0) as credit_control_step_id,
				GETDATE() AS created_date,
				DateAdd(day, (	SELECT	MIN(Credit_Control_Rule.Processing_Days)
								FROM	PFPremiumFinance INNER JOIN
                      					PFRF ON PFPremiumFinance.pfrf_id = PFRF.pfrf_id INNER JOIN
		                      			Credit_Control_Rule ON PFRF.pffrequency_id = Credit_Control_Rule.pffrequency_id AND
                		      			PFPremiumFinance.CompanyNo = Credit_Control_Rule.source_id
								WHERE	(Credit_Control_Rule.business_type = 'INS') AND
										(PFPremiumFinance.Insurance_File_Cnt = @Insurance_File_Cnt) AND
										(Credit_Control_Rule.is_active = 1)
                                         AND (Credit_Control_rule.Product_id IS NULL  OR Credit_Control_rule.Product_id IN( SELECT Product_id FROM Insurance_file Where Insurance_File_Cnt =@Insurance_File_Cnt ))
                                ), 
				PFInstalments.DueDate) as Due_Date,
				0 AS letter_sent, 0 AS recurrence_count, PFInstalments.PFInstalments_Id
		FROM	PFInstalments INNER JOIN
		        PFPremiumFinance ON PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt AND
		        PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version INNER JOIN
		        Document DOC INNER JOIN
		        TransDetail ON DOC.document_id = TransDetail.document_id ON PFPremiumFinance.PlanTransaction_id = TransDetail.transdetail_id
		WHERE   (PFPremiumFinance.Insurance_File_Cnt = @Insurance_File_Cnt)
			  AND (PFInstalments.InstalmentNumber > 0)     

		OPEN CreditControl_Cursor
		
		FETCH NEXT FROM CreditControl_Cursor INTO @Trans_Type, @Account_ID, @Document_ID, @Document_Date,
				                                  @Insurance_File_Cnt, @PFPrem_Finance_Cnt, @PFPrem_Finance_Version,
				                                  @Amount, @Will_Auto_Cancel, @Credit_Control_Step_ID, @Created_Date,
				                                  @Due_Date, @Letter_Sent, @Recurrence_Count,  @PfInstalments_ID
		WHILE @@FETCH_STATUS = 0
		Begin
			if @Credit_Control_Step_ID > 0
				Begin
					INSERT INTO Credit_Control_Item (credit_control_reason,
					       		 					 account_id,
							 						 document_id,
											 		 document_date,
							 						 insurance_file_cnt,
			                                         pfprem_finance_cnt,
				  									 pfprem_finance_version,
													 amount,
													 can_auto_cancel,
													 will_auto_cancel,
													 credit_control_step_id,
													 created_date,
													 due_date,
													 letter_sent,
													 recurrence_count,
													 PFInstalments_Id, 
													 is_deleted)
					
											 VALUES  (@Trans_Type,
													 @Account_ID,	
													 @Document_ID,
													 @Document_Date,
													 @Insurance_File_Cnt,
													 @PFPrem_Finance_Cnt,
													 @PFPrem_Finance_Version,
													 @Amount,
													 @Can_Auto_Cancel,
													 @Will_Auto_Cancel,
													 @Credit_Control_Step_ID,
													 @Created_Date,
													 @due_date,
													 @Letter_Sent,
													 @Recurrence_Count,
													 @PFInstalments_Id,
													 0)
				End
			FETCH NEXT FROM CreditControl_Cursor INTO @Trans_Type, @Account_ID, @Document_ID, @Document_Date,
					                                  @Insurance_File_Cnt, @PFPrem_Finance_Cnt, @PFPrem_Finance_Version,
					                                  @Amount, @Will_Auto_Cancel, @Credit_Control_Step_ID, @Created_Date,
					                                  @Due_Date, @Letter_Sent, @Recurrence_Count,  @PfInstalments_ID
		END
		CLOSE 	    CreditControl_Cursor
		DEALLOCATE  CreditControl_Cursor
	END

GO
