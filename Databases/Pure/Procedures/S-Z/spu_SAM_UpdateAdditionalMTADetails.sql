SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SAM_UpdateAdditionalMTADetails'
GO

CREATE  procedure spu_SAM_UpdateAdditionalMTADetails
 @insuranceFilecnt int,
 @InsuredName varchar(255),
 @InsuranceRef varchar(30),
 @SourceCode varchar(10),
 @LapsedDescription varchar(255),
 @AlternateReference varchar(80),
 @PolicyStatuscode varchar(10),
 @Analysiscode varchar(10),
 @Businesstypecode varchar(10),
 @DateIssued datetime,
 @ProposalDate datetime,
 @RenewalFrequencycode varchar(10),
 @LongTermUndertakingDate datetime,
 @RenewalStopcode varchar(10),
 @RenewalMethodcode varchar(10),
 @LapsedReasoncode varchar(10),
 @LapsedDate datetime,
 @IsReferredAtRenewal tinyint,
 @IsReferredOnMta tinyint,
 @PolicyStyleCode varchar(10),
 @AccountHandlerCount int,
 @QuoteExpiryDate datetime,
 @CreatedById INT=0,
 --Begin WPR36
 @PutOnNextInstalmentRenewal tinyint=0,
 @AnniversaryDate datetime='1/1/1900',
 @RenewalDate datetime='1/1/1900' ,
 @OldPolicyNumber VARCHAR(30)='',
 @UnderWritingYearCode VARCHAR(10) = NULL,
 @MTAReasonId SMALLINT= NULL,
 @SystemBaseDate DATETIME='1/1/1900',
 @sCoInsurancePlacement VARCHAR(10)=NULL,
 @is_backdated tinyint=0,
 @Correspondence_Type INT = NULL,
 @Default_Preferred_Correspondence INT = NULL,
 @Is_Agent_Correspondence TINYINT = 0,
 @ExpiryDate DATETIME = NULL
 --End WPR36

As  
Begin 
--Begin WPR36
 DECLARE @inception_date datetime  
 DECLARE @renewal_date datetime 
 DECLARE @OldAnniversayDate datetime
 DECLARE @Insurance_File_Type_Id Int

 SELECT @inception_date=insurance_folder.inception_date,@renewal_date=renewal_date,@OldAnniversayDate=insurance_file.anniversary_date,@Insurance_File_Type_Id = Insurance_File_Type_Id FROM insurance_file Inner Join insurance_folder ON insurance_file.insurance_folder_cnt=insurance_folder.insurance_folder_cnt WHERE insurance_file_cnt=@insuranceFilecnt
 IF (@AnniversaryDate='1/1/1900' And @OldAnniversayDate<='1/1/1900' )
   SET @AnniversaryDate=dateadd(year,1,@inception_date)
 ELSE
   SET @AnniversaryDate=@OldAnniversayDate
 IF @RenewalDate='1/1/1900'
   SET @RenewalDate=@renewal_date
  
 UPDATE [Insurance_File]  
 SET  
   [insured_name]=@InsuredName,  
   [source_id]=(select source_id from source where code=@SourceCode),  
   [lapsed_description]=@LapsedDescription,  
   [alternate_reference]=@AlternateReference,  
   [policy_status_id]=isnull((select policy_status_id from policy_status where code=@PolicyStatuscode),null),  
   [Analysis_code_id]=isnull((select analysis_code_id from analysis_code where code=@Analysiscode),null),  
   [business_type_id]=CASE @is_backdated WHEN 0 THEN(select Business_type_id from Business_type where code=@Businesstypecode) ELSE [business_type_id] END,  
   [date_issued]=@DateIssued,  
   [proposal_date]=@ProposalDate,  
   [renewal_frequency_id]=(select Renewal_frequency_id from Renewal_frequency where code=@RenewalFrequencycode),  
   [long_term_undertaking_date]=@LongTermUndertakingDate,  
   [renewal_stop_code_id]=isnull(@RenewalStopcode,null),  
   [renewal_method_id]=isnull((select Renewal_Method_id from Renewal_Method where code=@RenewalMethodcode),null),  
   [lapsed_reason_id]=isnull((select lapsed_reason_id from lapsed_reason where code=@LapsedReasoncode),null),  
   [lapsed_date]=@LapsedDate,  
   [is_referred_at_renewal] = @IsReferredAtRenewal,  
   [is_referred_on_mta] = @IsReferredOnMta,  
   [policy_style_id]=isnull((select policy_style_id from policy_style where code=@PolicyStyleCode),null),  
   [account_handler_cnt]=@AccountHandlerCount,  
   [quote_expiry_date]=@QuoteExpiryDate,  
   [put_on_next_instalment_renewal]=@PutOnNextInstalmentRenewal,
   [anniversary_date]= @AnniversaryDate,
   [renewal_date]= @RenewalDate  ,
   [old_policy_number]= @OldPolicyNumber,
   [MTA_reason_Id] = @MTAReasonId,
   [system_base_date] =@SystemBaseDate,   
   [coins_placement]= @sCoInsurancePlacement,
   [Correspondence_Type] = @Correspondence_Type,
   [Default_Preferred_Correspondence] = @Default_Preferred_Correspondence,
   [Is_Agent_Correspondence] = @Is_Agent_Correspondence
 WHERE  
 [insurance_file_cnt]=@insuranceFilecnt  
  
 IF @CreatedById<>0  
  BEGIN  
   UPDATE Insurance_File_System  
   SET Created_By_Id=@CreatedById,Modified_by_id=@CreatedById  
   WHERE insurance_file_cnt=@insuranceFilecnt  
  END  
  
 IF ISNULL(@UnderWritingYearCode,'')<>''
	 BEGIN
		 UPDATE [Insurance_File]
		 SET  [underwriting_year_id]=(select underwriting_year_id from Underwriting_Year where code=@UnderWritingYearCode)
		 WHERE insurance_file_cnt=@insuranceFilecnt  
	 END

 IF ISNULL(@ExpiryDate,'') <> '' and @Insurance_File_Type_Id = 7
	 BEGIN
		 UPDATE [Insurance_File]
		 SET  [expiry_date]= convert(date,@ExpiryDate)
		 WHERE insurance_file_cnt=@insuranceFilecnt
	 END
 
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
