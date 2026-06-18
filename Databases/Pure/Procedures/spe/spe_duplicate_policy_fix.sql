SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
SET NOCOUNT ON
GO
Execute DDLDropProcedure 'spe_duplicate_policy_fix'
GO

Create Procedure spe_duplicate_policy_fix
AS
BEGIN

--BEGIN TRANSACTION

DECLARE
	@InsFoldCnt 	INT,
	@InsFoldCntNew 	INT,
	@InsFoldIDNew	INT, 
	@InsFileCnt	INT,
	@SourceID	INT,
	@InsFileRef	VARCHAR(30),
	@PartyCnt	INT,
	@IncepDate	DATETIME,
	@sTable		VARCHAR(20)


DECLARE CurSelectPolicies CURSOR FOR
	select ifil.insurance_file_cnt, ifil.insurance_ref, ifil.insurance_folder_cnt, 
		ifil.source_id, ifil.insured_cnt, ifil.cover_start_date
	from insurance_file ifil
	join party p on p.party_cnt = ifil.insured_cnt
	join insurance_folder ifol on ifol.insurance_folder_cnt = ifil.insurance_folder_cnt
	join party p2 on p2.party_cnt = ifol.insurance_holder_cnt
	where ifol.insurance_holder_cnt <> ifil.insured_cnt
	and insurance_ref not in (select code from insurance_folder)

-- Rename two manually fixed records, so that the script below doesn't clash
update insurance_folder set code = RTRIM(LTRIM(code)) + '-X' where code='TBA-DT72/00000'
update insurance_folder set code = RTRIM(LTRIM(code)) + '-X' where code='TBA/00000'


OPEN CurSelectPolicies

FETCH NEXT FROM CurSelectPolicies 
INTO @InsFileCnt, @InsFileRef, @InsFoldCnt, @SourceID, @PartyCnt, @IncepDate


WHILE @@FETCH_STATUS = 0
BEGIN

	select @InsFileCnt, @InsFileRef, @InsFoldCnt, @SourceID

	set @InsFoldIDNew=0
	
	--select @sTable, @InsFoldIDNew
	
	-- Add a new Insurance_Folder record
	insert into Insurance_Folder (	insurance_folder_id, 
					source_id, 
					insurance_holder_cnt, 
					code, 
					description, 
					inception_date, 
					arc_archive_folder_id, 
					quote_insurance_ref, 
					next_insurance_ref, 
					last_insurance_ref, 
					renewal_count, 
					renewal_NCD_Year, 
					renewal_NCD_contents, 
					last_edi_message_count_received, 
					last_edi_message_count_sent)

	values (			@InsFoldIDNew, 
					@SourceID, 
					@PartyCnt, 
					@InsFileRef, 
					null, 
					@IncepDate,
					NULL,                  
					'',                  
					'',                 
					'',                 
					0,             
					NULL,             
					NULL,                 
					NULL,                            
					NULL)

	-- Get the Insurance_Folder_Cnt
	select	@InsFoldCntNew = @@IDENTITY
	select * from insurance_folder where insurance_folder_cnt = @InsFoldCntNew
	

	-- Now link the insurance_file record to the new insurance_folder record
	update insurance_file 
	set insurance_folder_cnt = @InsFoldCntNew 
	where insurance_file_cnt = @InsFileCnt

	-- Update the event_log too
	update event_log 
	set insurance_folder_cnt = @InsFoldCntNew 
	where insurance_file_cnt = @InsFileCnt

	-- update the insured name on the policy record
	update insurance_file 
	set insured_name = (select name from party where party_cnt=@PartyCnt)
	where insurance_file_cnt = @InsFileCnt


	FETCH NEXT FROM CurSelectPolicies 
	INTO @InsFileCnt, @InsFileRef, @InsFoldCnt, @SourceID, @PartyCnt, @IncepDate
END

CLOSE CurSelectPolicies
DEALLOCATE CurSelectPolicies
END

--ROLLBACK TRANSACTION
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
SET NOCOUNT OFF
Go