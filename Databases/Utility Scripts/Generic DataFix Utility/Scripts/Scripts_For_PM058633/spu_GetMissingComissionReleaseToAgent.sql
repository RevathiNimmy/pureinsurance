SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_GetMissingComissionReleaseToAgent'

GO
CREATE PROCEDURE spu_GetMissingComissionReleaseToAgent
AS 

DECLARE @insurance_File_cnt INT
DECLARE @insurance_Ref VARCHAR(30)

Create table #ResultMissingComm
(
	insurance_File_cnt int,
	insurance_Ref VARCHAR(30)
)
insert into #ResultMissingComm 
select a.insurance_file_cnt,i.insurance_ref from PM058633_DataFix_Part1_log a
join insurance_file i on a.insurance_file_cnt = i.insurance_File_cnt
--where i.insurance_Ref = 'HEPMOT00135578'

Declare c Cursor For select insurance_File_cnt,insurance_Ref from #ResultMissingComm
Open c
Fetch next From c into @insurance_File_cnt,@insurance_Ref
While @@Fetch_Status=0 Begin
		IF (Select Count(pfi.pfinstalments_id) from pfpremiumfinance pf
	join pfinstalments pfi on pf.pfprem_finance_cnt = pfi.pfprem_finance_cnt and pf.pfprem_finance_version = pfi.pfprem_finance_version
	where insurance_file_cnt = @insurance_File_cnt and pfi.status = 3) > 0
		BEGIN
			exec spu_InsertMissingComissionReleaseToAgent @insurance_Ref
		END
Fetch next From c into @insurance_File_cnt,@insurance_Ref
End
Close c
Deallocate c

If(OBJECT_ID('tempdb..#ResultMissingComm') Is Not Null)
Begin
    Drop Table #ResultMissingComm
End