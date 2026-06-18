
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GetTransDetail_Auto_Allocation_For_Cancellation'
GO
CREATE PROCEDURE spu_GetTransDetail_Auto_Allocation_For_Cancellation 

@insurance_file_cnt int, 
@insurance_folder_cnt int

as 

	Declare @Policy_count  int
	Declare @bPerformAutoAllocate int
	Declare @Inception_Date_NB date
	Declare @Inception_Date_MTC date
	Declare @Original_insurance_file_cnt int
	Declare @bClaimExists int
	Declare @PFTransaction_id int


	Select @Policy_count= Count(Insurance_file_cnt) from insurance_file(nolock) where insurance_folder_cnt = @insurance_folder_cnt

	Select Top 1 @Original_insurance_file_cnt =Insurance_file_cnt , @Inception_Date_NB = cover_start_date
	from insurance_file(nolock) where insurance_folder_cnt = @insurance_folder_cnt
	order by insurance_file_cnt asc

	select @bClaimExists = Isnull(Count(Claim_ID),0) from claim(nolock) where policy_id = @Original_insurance_file_cnt

	Select @Inception_Date_MTC = cover_start_date from insurance_file(nolock) where insurance_file_cnt = @insurance_file_cnt


	select top 1 @PFTransaction_id = isnull(PFTransaction_id,0) from  PFInstalments(nolock) where pfprem_finance_cnt in ( select pfprem_finance_cnt 
	from PFPremiumFinance where  Insurance_file_cnt = @Original_insurance_file_cnt)


	SET @PFTransaction_id = isnull(@PFTransaction_id,0)

	PRINT @PFTransaction_id

	IF @Policy_count = 2 and @bClaimExists = 0 and (@Inception_Date_MTC = @Inception_Date_NB) and (@PFTransaction_id = 0)
		BEGIN

		Select td.transdetail_id, td.account_id, td.document_id,td.amount as amount,td.spare, 'NB' as transType, D.document_ref,td.outstanding_amount from TransDetail(nolock) as TD
		join document(nolock) d on  d.document_id = TD.document_id
		where d.insurance_file_cnt = @Original_insurance_file_cnt
		 and td.outstanding_amount <> 0
		union all
		Select td.transdetail_id, td.account_id, td.document_id,td.amount as amount,td.spare, 'MTC' as transType, D.document_ref,td.outstanding_amount  from TransDetail(nolock) as TD
		join document(nolock) d on  d.document_id = TD.document_id
		where d.insurance_file_cnt = @insurance_file_cnt
		 and td.outstanding_amount <> 0

	 END
	 Go
