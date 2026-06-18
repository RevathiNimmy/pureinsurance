SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_SAM_Update_party'
GO

--Start (girija) - (UIIC WR27 - MTA Amend Client.doc)  
CREATE procedure spu_SAM_Update_party  
 @party_cnt int,  
 @ServiceLevelId int,  
 @AreaId int,  
 @AgentCnt int,  
 @IsProspect tinyint,  
 @IsAlsoAgent tinyint,  
 @CorrespondenceTypeId int,  
 @PaymentMethodCode varchar(70),  
 @ReminderTypeId int,  
 @PaymentTermCode varchar(70),  
 @RenewalStopCodeId int,  
 @LoyaltyNumber varchar(20),  
 @SeasonalGiftId int,  
 @CCJs int,
 @BlacklistReasonId int = null,
 @currency_id int,  
 @consultant_cnt int,
 @source_id int=null,
 @sub_branch_id int=null

as  
BEGIN
	DECLARE @Party_id INT
	DECLARE @sTable VARCHAR(50) 

	SET @party_id=0 
	  
	IF ISNULL(@sub_branch_id, 0) = 0  
	BEGIN  
	    EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT  
	END 

	UPDATE Party  
	SET  
		service_level_id=@ServiceLevelId,  
		area_id=@AreaId,  
		agent_cnt=@AgentCnt,  
		is_prospect=@IsProspect,  
		is_also_agent=@IsAlsoAgent,  
		correspondence_type_id=@CorrespondenceTypeId,  
		payment_method_code=@PaymentMethodCode,  
		reminder_type_id=@ReminderTypeId,  
		payment_term_code=@PaymentTermCode,  
		renewal_stop_code_id=@RenewalStopCodeId,  
		loyalty_number=@LoyaltyNumber,  
		seasonal_gift_id=@SeasonalGiftId,  
		CCJs=@CCJs,
		currency_id=@currency_id,
		consultant_cnt=@consultant_cnt,
		source_id=@source_id,
		sub_branch_id=@sub_branch_id,
		party_id=@party_id,
		blacklist_reason_id = @BlacklistReasonId
	WHERE party_cnt=@party_cnt  

	UPDATE account set currency_id=@currency_id  where account_key=@party_cnt

End  
--End (girija) - (UIIC WR27 - MTA Amend Client.doc)  


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

