SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_claim_udf'
GO


CREATE PROCEDURE spu_wp_claim_udf
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @UserID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

	Declare @Option_value varchar(100),
		@option_number int,
		@source_id int ,
		@sql varchar(1024),
		@Column_name varchar(100),
		@Column_value varchar(255)

	--Retrieve Source ID from the insurance file
	select @source_id=ifi.source_id from insurance_file ifi, claim c where c.policy_id=ifi.insurance_file_cnt and c.claim_id=@ClaimCnt

	select @sql = ''

	DECLARE cUdfClaimFields CURSOR FAST_FORWARD FOR
	    select value, option_number from system_options where (option_number=2003 or option_number=2004 or option_number=2005 or option_number=2006 or option_number=2007) and value <> 0 and branch_id = @source_id
	OPEN cUdfClaimFields
	FETCH NEXT FROM cUdfClaimFields INTO @Option_value, @option_number

	if @@FETCH_STATUS=0
	Begin
		select @sql = 'select '
	End

	WHILE @@FETCH_STATUS = 0 
	BEGIN

		SELECT  @Column_name = replace(cap.caption,' ','_') FROM gis_user_def_header tn, pmcaption cap
		WHERE tn.GIS_user_def_header_id=@Option_value AND tn.caption_id = cap.caption_id and cap.language_id = 1			
		
		SELECT @Column_value = ''

		if @option_number=2003
		Begin
			SELECT @Column_value = cap.caption FROM GIS_user_def_detail GD, pmcaption cap WHERE GD.is_deleted = 0 AND GD.GIS_user_def_header_id = @option_value AND GD.effective_date <= getdate() AND GD.caption_id = cap.caption_id AND cap.language_id = 1 AND GD.GIS_user_def_detail_id=(select top 1 user_defined_field_a from claim where claim_id=@ClaimCnt)
		End

		if @option_number=2004
		Begin
			SELECT @Column_value = cap.caption FROM GIS_user_def_detail GD, pmcaption cap WHERE GD.is_deleted = 0 AND GD.GIS_user_def_header_id = @option_value AND GD.effective_date <= getdate() AND GD.caption_id = cap.caption_id AND cap.language_id = 1 AND GD.GIS_user_def_detail_id=(select top 1 user_defined_field_b from claim where claim_id=@ClaimCnt)
		End

		if @option_number=2005
		Begin
			SELECT @Column_value = cap.caption FROM GIS_user_def_detail GD, pmcaption cap WHERE GD.is_deleted = 0 AND GD.GIS_user_def_header_id = @option_value AND GD.effective_date <= getdate() AND GD.caption_id = cap.caption_id AND cap.language_id = 1 AND GD.GIS_user_def_detail_id=(select top 1 user_defined_field_c from claim where claim_id=@ClaimCnt)
		End

		if @option_number=2006
		Begin
			SELECT @Column_value = cap.caption FROM GIS_user_def_detail GD, pmcaption cap WHERE GD.is_deleted = 0 AND GD.GIS_user_def_header_id = @option_value AND GD.effective_date <= getdate() AND GD.caption_id = cap.caption_id AND cap.language_id = 1 AND GD.GIS_user_def_detail_id=(select top 1 user_defined_field_d from claim where claim_id=@ClaimCnt)
		End

		if @option_number=2007
		Begin
			SELECT @Column_value = cap.caption FROM GIS_user_def_detail GD, pmcaption cap WHERE GD.is_deleted = 0 AND GD.GIS_user_def_header_id = @option_value AND GD.effective_date <= getdate() AND GD.caption_id = cap.caption_id AND cap.language_id = 1 AND GD.GIS_user_def_detail_id=(select top 1 user_defined_field_e from claim where claim_id=@ClaimCnt)
		End

		select @column_value = isnull(@column_value,'')

		if @sql <> 'select '
		Begin
			select @sql = @sql + ', '
		End

		select @sql = @sql + '''' + @column_value + ''' as ' + @Column_name

	    FETCH NEXT FROM cUdfClaimFields INTO @Option_value, @option_number
	END
	
	CLOSE cUdfClaimFields

	DEALLOCATE cUdfClaimFields

	exec (@sql)
go