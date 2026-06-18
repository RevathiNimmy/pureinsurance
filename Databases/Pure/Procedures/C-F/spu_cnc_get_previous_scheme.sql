ddldropprocedure spu_cnc_get_previous_scheme
go

CREATE PROCEDURE spu_cnc_get_previous_scheme
    @insurance_file_cnt     int
AS
BEGIN

if exists(
	SELECT null
	FROM gis_policy_link l,
	insurance_file_risk_link r,
	insurance_file_risk_link ro
	WHERE r.insurance_file_cnt = @insurance_file_cnt
	AND r.original_risk_cnt = ro.risk_cnt
	AND ro.insurance_file_cnt = l.insurance_file_cnt)
Begin
	SELECT l.gis_scheme_id, l.insurance_file_cnt
	FROM gis_policy_link l,
	insurance_file_risk_link r,
	insurance_file_risk_link ro
	WHERE r.insurance_file_cnt = @insurance_file_cnt
	AND r.original_risk_cnt = ro.risk_cnt
	AND ro.insurance_file_cnt = l.insurance_file_cnt
End
Else
Begin
	declare @Old_insurance_file_cnt int
	declare @Old_risk_cnt int
	select @old_risk_cnt = max(ro.risk_cnt) from insurance_file_risk_link r 
	join insurance_file_risk_link ro on ro.insurance_file_cnt = r.insurance_file_cnt
	and ro.risk_cnt < r.risk_cnt
	where r.insurance_file_cnt=@insurance_file_cnt and ro.status_flag='U'
	if @old_risk_cnt is not null
	Begin
		select @Old_insurance_file_cnt = min(insurance_file_cnt) from insurance_file_risk_link where risk_cnt = @old_risk_cnt
	End

	select gis_scheme_id, insurance_file_cnt from gis_policy_link where insurance_file_cnt = @old_insurance_file_cnt
End
END

